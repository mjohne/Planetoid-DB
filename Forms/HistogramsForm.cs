// This file contains the implementation of the HistogramsForm,
// which displays histogram/bar charts of orbital elements and properties
// for all minor planets in the database.
using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using ScottPlot;
using ScottPlot.Plottables;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a histogram of counted planetoids for a selected orbital element or derived property.</summary>
/// <remarks>The form groups planetoids into selectable ranges, renders the distribution as a ScottPlot bar chart, and mirrors the counted bins in a tabular ListView. Users can optionally request live updates while the background counting operation is running.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class HistogramsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to record errors and cancellation events from the histogram generation workflow.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the source MPCORB records passed in by the main form.</summary>
	/// <remarks>A local copy of the required raw data is supplied by the main program so this form can generate its histogram independently.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the currently running cancellation token source.</summary>
	/// <remarks>This field is non-null only while histogram creation is in progress.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Stores the currently displayed histogram results.</summary>
	/// <remarks>The list is refreshed whenever the diagram and the ListView are updated.</remarks>
	private List<HistogramBinResult> _currentResults = [];

	/// <summary>Represents one selectable histogram step size.</summary>
	/// <param name="Value">The numeric width of a single histogram bin.</param>
	/// <param name="DisplayText">The text shown in the step-size drop-down.</param>
	/// <remarks>The display text is used directly by the ComboBox, so <see cref="ToString"/> returns <see cref="DisplayText"/>.</remarks>
	private sealed record StepOption(double Value, string DisplayText)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The preformatted step-size label.</returns>
		public override string ToString() => DisplayText;
	}

	/// <summary>Represents one selectable histogram definition.</summary>
	/// <param name="DisplayName">The user-facing name of the orbital element or property.</param>
	/// <param name="AxisLabel">The x-axis label for the chart.</param>
	/// <param name="UnitSuffix">The optional unit suffix used in formatted values.</param>
	/// <param name="StepOptions">The meaningful step sizes offered for the definition.</param>
	/// <param name="ValueSelector">The callback used to extract the numeric value from a raw MPCORB line.</param>
	/// <remarks>The definition centralizes presentation metadata and parsing logic for one histogram mode.</remarks>
	private sealed record HistogramDefinition(
		string DisplayName,
		string AxisLabel,
		string UnitSuffix,
		IReadOnlyList<StepOption> StepOptions,
		Func<string, double?> ValueSelector)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The histogram definition name.</returns>
		public override string ToString() => DisplayName;
	}

	/// <summary>Represents one counted histogram range.</summary>
	/// <param name="Start">The inclusive lower range boundary.</param>
	/// <param name="End">The exclusive upper range boundary.</param>
	/// <param name="Count">The number of planetoids inside the range.</param>
	/// <remarks>Histogram rows are sorted by their range start value before being displayed.</remarks>
	private sealed record HistogramBinResult(double Start, double End, int Count);

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="HistogramsForm"/> class.</summary>
	/// <param name="planetoids">The planetoid string records to process from the database.</param>
	/// <remarks>The main form passes the necessary raw MPCORB data to this dialog so the histogram can be generated without directly accessing shared UI state.</remarks>
	public HistogramsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Store the supplied planetoid data for histogram generation.
		_planetoids = planetoids;
		// Set up the orbital element selection and step-size options based on predefined histogram definitions.
		InitializeSelections();
		// Initialize the progress bar and taskbar progress indicator to a known state.
		UpdateRunningState(isRunning: false);
		ResetDisplayedResults();
		// The form is now ready for user interaction, and the logger can record any subsequent events.
		logger.Info(message: "HistogramsForm initialized with {0} planetoids.", argument: _planetoids.Count);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>The method currently returns the same string as <see cref="object.ToString()"/>, but it can be customized to include more specific information if needed.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Creates all histogram definitions supported by the form.</summary>
	/// <returns>A list of selectable histogram definitions.</returns>
	/// <remarks>The selectable items include directly stored orbital elements and a few useful derived properties computed from semi-major axis and eccentricity.</remarks>
	private static List<HistogramDefinition> CreateHistogramDefinitions() =>
		[
			new HistogramDefinition(
				DisplayName: "Semi-major axis", AxisLabel: "Semi-major axis (AU)", UnitSuffix: " AU",
				StepOptions: [
					new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
					new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
					new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
					new StepOption(Value: 1.0, DisplayText: "1 AU"),
					new StepOption(Value: 2.0, DisplayText: "2 AU")
				],
				ValueSelector: static line => TryParseSemiMajorAxis(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Eccentricity", AxisLabel: "Eccentricity", UnitSuffix: string.Empty,
				StepOptions: [
					new StepOption(Value: 0.01, DisplayText: "0.01"),
					new StepOption(Value: 0.05, DisplayText: "0.05"),
					new StepOption(Value: 0.1, DisplayText: "0.1"),
					new StepOption(Value: 0.2, DisplayText: "0.2")
				],
				ValueSelector: static line => TryParseEccentricity(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Inclination", AxisLabel: "Inclination (°)", UnitSuffix: "°",
				StepOptions: [
					new StepOption(Value: 1.0, DisplayText: "1°"),
					new StepOption(Value: 2.0, DisplayText: "2°"),
					new StepOption(Value: 5.0, DisplayText: "5°"),
					new StepOption(Value: 10.0, DisplayText: "10°")
				],
				ValueSelector: static line => TryParseInclination(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Mean anomaly", AxisLabel: "Mean anomaly (°)", UnitSuffix: "°",
				StepOptions: [
					new StepOption(Value: 5.0, DisplayText: "5°"),
					new StepOption(Value: 10.0, DisplayText: "10°"),
					new StepOption(Value: 15.0, DisplayText: "15°"),
					new StepOption(Value: 30.0, DisplayText: "30°")
				],
				ValueSelector: static line => TryParseMeanAnomaly(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Argument of perihelion", AxisLabel: "Argument of perihelion (°)", UnitSuffix: "°",
				StepOptions: [
					new StepOption(Value: 5.0, DisplayText: "5°"),
					new StepOption(Value: 10.0, DisplayText: "10°"),
					new StepOption(Value: 15.0, DisplayText: "15°"),
					new StepOption(Value: 30.0, DisplayText: "30°")
				],
				ValueSelector: static line => TryParseArgumentOfPerihelion(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Longitude of ascending node", AxisLabel: "Longitude of ascending node (°)", UnitSuffix: "°",
				StepOptions:
				[
					new StepOption(Value: 5.0, DisplayText: "5°"),
					new StepOption(Value: 10.0, DisplayText: "10°"),
					new StepOption(Value: 15.0, DisplayText: "15°"),
					new StepOption(Value: 30.0, DisplayText: "30°")
				],
				ValueSelector: static line => TryParseLongitudeOfAscendingNode(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Mean daily motion", AxisLabel: "Mean daily motion (°/day)", UnitSuffix: " °/day",
				StepOptions:
				[
					new StepOption(Value: 0.05, DisplayText: "0.05 °/day"),
					new StepOption(Value: 0.1, DisplayText: "0.1 °/day"),
					new StepOption(Value: 0.25, DisplayText: "0.25 °/day"),
					new StepOption(Value: 0.5, DisplayText: "0.5 °/day")
				],
				ValueSelector: static line => TryParseMeanDailyMotion(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Perihelion distance", AxisLabel: "Perihelion distance (AU)", UnitSuffix: " AU",
				StepOptions: [
					new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
					new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
					new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
					new StepOption(Value: 1.0, DisplayText: "1 AU")
				],
				ValueSelector: static line => TryParsePerihelionDistance(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Aphelion distance", AxisLabel: "Aphelion distance (AU)", UnitSuffix: " AU",
				StepOptions: [
					new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
					new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
					new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
					new StepOption(Value: 1.0, DisplayText: "1 AU"),
					new StepOption(Value: 2.0, DisplayText: "2 AU")
				],
				ValueSelector: static line => TryParseAphelionDistance(line: line, value: out double value) ? value : null),
			new HistogramDefinition(
				DisplayName: "Orbital period", AxisLabel: "Orbital period (years)", UnitSuffix: " years",
				StepOptions: [
					new StepOption(Value: 0.5, DisplayText: "0.5 years"),
					new StepOption(Value: 1.0, DisplayText: "1 year"),
					new StepOption(Value: 2.0, DisplayText: "2 years"),
					new StepOption(Value: 5.0, DisplayText: "5 years")
				],
				ValueSelector: static line => TryParseOrbitalPeriod(line: line, value: out double value) ? value : null)
		];

	/// <summary>Initializes the selectable orbital element and step-size drop-downs.</summary>
	/// <remarks>The step-size list is rebuilt automatically whenever the selected orbital element changes.</remarks>
	private void InitializeSelections()
	{
		// Populate the step-size ComboBox with the options from the first histogram definition, if available.
		toolStripComboBoxOrbitElement.Items.Clear();
		// Populate the orbital element ComboBox with all available histogram definitions.
		foreach (HistogramDefinition definition in CreateHistogramDefinitions())
		{
			// Add the definition to the orbital element selection ComboBox.
			_ = toolStripComboBoxOrbitElement.Items.Add(item: definition);
		}
		// Select the first orbital element definition by default, if any are available.
		if (toolStripComboBoxOrbitElement.Items.Count > 0)
		{
			// Trigger the SelectedIndexChanged event to populate the step-size ComboBox based on the first definition's options.
			toolStripComboBoxOrbitElement.SelectedIndex = 0;
		}
	}

	/// <summary>Gets the currently selected histogram definition.</summary>
	/// <returns>The selected histogram definition, or <see langword="null"/> if none is selected.</returns>
	private HistogramDefinition? GetSelectedDefinition() => toolStripComboBoxOrbitElement.SelectedItem as HistogramDefinition;

	/// <summary>Gets the currently selected histogram step size.</summary>
	/// <returns>The selected step size, or <see langword="null"/> if none is selected.</returns>
	private StepOption? GetSelectedStep() => toolStripComboBoxStepSize.SelectedItem as StepOption;

	/// <summary>Updates the toolbar state to reflect whether histogram creation is running.</summary>
	/// <param name="isRunning">True while the background task is active; otherwise false.</param>
	/// <remarks>The same toolbar button is used for both starting and canceling the operation to match the issue requirements.</remarks>
	private void UpdateRunningState(bool isRunning)
	{
		// Update the start/cancel button text and icon based on the running state.
		toolStripButtonStartCancel.Text = isRunning ? "&Cancel" : "&Start";
		// The icons are from the Fatcow set, which is included in the project resources. The "page white text" icon represents the start action, while the "cancel" icon represents the cancel action.
		toolStripButtonStartCancel.Image = isRunning
			? Resources.FatcowIcons16px.fatcow_cancel_16px
			: Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		// Enable or disable the orbital element and step-size selection, as well as the live display option, based on whether the histogram generation is currently running.
		toolStripComboBoxOrbitElement.Enabled = !isRunning;
		toolStripComboBoxStepSize.Enabled = !isRunning;
		toolStripButtonLiveDisplay.Enabled = !isRunning;
	}

	/// <summary>Resets the displayed ListView and chart results.</summary>
	/// <remarks>The method clears previously counted bins and redraws the empty chart using the currently selected histogram definition.</remarks>
	private void ResetDisplayedResults()
	{
		// Clear the current results list and the ListView items.
		_currentResults = [];
		// The ListView is updated in a batch to prevent flickering and improve performance during the clear operation.
		listViewResults.BeginUpdate();
		// Clear all items from the ListView to prepare for new results.
		listViewResults.Items.Clear();
		// Redraw the histogram plot with no data to reflect the reset state. The axes and labels are still configured based on the selected definition, but no bars are displayed.
		listViewResults.EndUpdate();
		// Update the histogram plot to reflect the cleared results. This ensures that the chart is synchronized with the empty ListView and ready for new data.
		UpdateHistogramPlot(definition: GetSelectedDefinition(), results: _currentResults);
	}

	/// <summary>Updates the progress bar value and taskbar progress indicator.</summary>
	/// <param name="percent">The progress percentage to display.</param>
	/// <remarks>The input value is clamped to the 0 to 100 range before it is shown.</remarks>
	private void UpdateProgress(int percent)
	{
		// Clamp the input percentage to ensure it stays within the valid range of 0 to 100.
		int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
		// Update the Krypton progress bar with the clamped percentage value and display it as text.
		kryptonProgressBar.Value = clampedPercent;
		kryptonProgressBar.Values.Text = $"{clampedPercent}%";
		// Update the taskbar progress indicator for this window, if supported and if the form's handle has been created. The progress value is set to the clamped percentage, with a maximum of 100.
		if (IsHandleCreated)
		{
			// The TaskbarProgress helper class abstracts the Windows taskbar API to allow setting the progress state and value for the application's taskbar button. This provides a visual indication of progress even when the form is minimized or not in the foreground.
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
		}
	}

	/// <summary>Applies counted histogram results to both the ListView and the chart.</summary>
	/// <param name="definition">The histogram definition that produced the results.</param>
	/// <param name="results">The histogram bins to display.</param>
	/// <remarks>The method keeps the chart and the tabular view synchronized.</remarks>
	private void ApplyResults(HistogramDefinition? definition, IReadOnlyList<HistogramBinResult> results)
	{
		// Update the current results list with the new histogram bins.
		_currentResults = [.. results];
		// The ListView is updated in a batch to prevent flickering and improve performance while adding multiple items.
		listViewResults.BeginUpdate();
		// Clear existing items from the ListView to prepare for the new results. This ensures that only the latest histogram bins are displayed.
		listViewResults.Items.Clear();
		// Add each histogram bin result as a new item in the ListView. The item text is formatted to show the range of values for the bin, and the subitems display the start, end, and count values. A tooltip is also set to show the count of planetoids in each bin when hovering over the item.
		foreach (HistogramBinResult result in results)
		{
			// Create a new ListViewItem for the histogram bin, with the main text showing the formatted range label. The tooltip displays the count of planetoids in the bin.
			ListViewItem item = new(text: FormatRangeLabel(start: result.Start, end: result.End, unitSuffix: definition?.UnitSuffix ?? string.Empty))
			{
				ToolTipText = $"{result.Count:N0} planetoids"
			};
			// Add subitems for the start, end, and count values of the histogram bin. The numeric values are formatted for display using the invariant culture to ensure consistent formatting regardless of the user's locale.
			_ = item.SubItems.Add(text: FormatNumericValue(value: result.Start));
			_ = item.SubItems.Add(text: FormatNumericValue(value: result.End));
			_ = item.SubItems.Add(text: result.Count.ToString(format: "N0", provider: CultureInfo.InvariantCulture));
			// Add the configured item to the ListView to display it in the tabular view.
			listViewResults.Items.Add(value: item);
		}
		// Redraw the histogram plot to reflect the new results. This ensures that the visual representation of the histogram is updated in sync with the ListView data.
		listViewResults.EndUpdate();
		// Update the histogram plot with the new results, using the selected histogram definition to configure the axes and labels appropriately.
		UpdateHistogramPlot(definition: definition, results: results);
	}

	/// <summary>Redraws the ScottPlot histogram based on the supplied results.</summary>
	/// <param name="definition">The histogram definition that produced the results.</param>
	/// <param name="results">The histogram bins to render.</param>
	/// <remarks>The chart always contains a title, axis labels, and a legend. When no results are available, the axes are still configured but no bars are plotted.</remarks>
	private void UpdateHistogramPlot(HistogramDefinition? definition, IReadOnlyList<HistogramBinResult> results)
	{
		// Clear existing plots and configure the title, axis labels, and legend based on the selected histogram definition. The title reflects the selected orbital element or property, and the axis labels are set accordingly. The legend is made visible and aligned to the upper right corner of the plot area.
		formsPlotHistogram.Plot.Clear();
		formsPlotHistogram.Plot.Title(text: definition is null ? "Orbit elements histogram" : $"Histogram of {definition.DisplayName}");
		formsPlotHistogram.Plot.Axes.Bottom.Label.Text = definition?.AxisLabel ?? "Selected element";
		formsPlotHistogram.Plot.Axes.Left.Label.Text = "Number of planetoids";
		formsPlotHistogram.Plot.Legend.IsVisible = true;
		formsPlotHistogram.Plot.Legend.Alignment = Alignment.UpperRight;
		// If a valid histogram definition is provided and there are results to display, create a bar plot. The bar heights are determined by the count of planetoids in each bin, optionally transformed to a logarithmic scale if the corresponding option is checked. The x-axis positions correspond to the bin indices, and the labels are formatted to show the range of values for each bin with the appropriate unit suffix.
		if (definition is not null && results.Count > 0)
		{
			// Determine whether to apply a logarithmic transformation to the count values based on the state of the log scale option. If log scale is enabled and the count is greater than zero, the height of the bar is set to the base-10 logarithm of (count + 1); otherwise, it is set to the raw count value.
			bool logScale = toolStripButtonLogScale.Checked;
			// Create arrays for the bar heights, labels, and positions based on the histogram results. The values array contains the heights of the bars, the labels array contains the formatted range labels for the x-axis ticks, and the positions array contains the corresponding x-axis positions for each bin.
			double[] values = [.. results.Select(selector: result => logScale && result.Count > 0 ? Math.Log10(d: result.Count + 1) : result.Count)];
			string[] labels = [.. results.Select(selector: result => FormatRangeLabel(start: result.Start, end: result.End, unitSuffix: definition.UnitSuffix))];
			double[] positions = [.. Enumerable.Range(start: 0, count: results.Count).Select(selector: static index => (double)index)];
			// Add a bar plot to the ScottPlot with the calculated positions and values. The bars are colored steel blue, and the legend text is set to "Planetoids" to indicate what the bars represent. The x-axis ticks are configured to use the custom labels corresponding to each histogram bin.
			BarPlot barPlot = formsPlotHistogram.Plot.Add.Bars(positions, values);
			barPlot.LegendText = "Planetoids";
			barPlot.Color = Colors.SteelBlue;
			// Configure the x-axis ticks to use the formatted range labels for each histogram bin. The NumericManual tick generator is used to specify custom tick positions and labels based on the calculated arrays.
			formsPlotHistogram.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(positions, labels);
		}
		// Auto-scale the axes to fit the displayed data, and apply a logarithmic tick generator to the y-axis if log scale is enabled. The log tick generator formats the labels to show the original count values corresponding to each logarithmic tick position.
		formsPlotHistogram.Plot.Axes.AutoScale();
		// If log scale is enabled, set the minimum of the y-axis to zero and use a custom tick generator that formats the tick labels as powers of ten. If log scale is not enabled, use the default automatic numeric tick generator.
		if (toolStripButtonLogScale.Checked)
		{
			// Set the minimum of the y-axis to zero to ensure that the logarithmic scale starts from a defined point. The custom tick generator formats the tick labels to show the original count values corresponding to each logarithmic tick position, using powers of ten for better readability.
			formsPlotHistogram.Plot.Axes.Left.Min = 0;
			// The NumericAutomatic tick generator is customized to format the tick labels as powers of ten minus one when log scale is enabled. If the tick value is greater than or equal to zero, it is formatted as 10 raised to the power of the tick value minus one; otherwise, an empty string is returned to avoid displaying negative ticks on a logarithmic scale.
			ScottPlot.TickGenerators.NumericAutomatic logTickGen = new()
			{
				LabelFormatter = static v => v >= 0 ? (Math.Pow(x: 10, y: v) - 1).ToString(format: "N0", provider: CultureInfo.InvariantCulture) : string.Empty
			};
			// Apply the custom logarithmic tick generator to the left (y) axis to format the tick labels appropriately when log scale is enabled.
			formsPlotHistogram.Plot.Axes.Left.TickGenerator = logTickGen;
		}
		// If log scale is not enabled, use the default automatic numeric tick generator for the y-axis to display the count values without logarithmic formatting.
		else
		{
			// Apply the default automatic numeric tick generator to the left (y) axis to display the count values without logarithmic formatting when log scale is not enabled.
			formsPlotHistogram.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic();
		}
		// Refresh the plot to apply all changes and render the updated histogram. This ensures that the visual representation of the histogram is updated to reflect the new data and configuration settings.
		formsPlotHistogram.Refresh();
	}

	/// <summary>Builds histogram bins for the selected definition on a background thread.</summary>
	/// <param name="definition">The selected histogram definition.</param>
	/// <param name="stepSize">The selected histogram bin size.</param>
	/// <param name="enableLiveDisplay">True to publish intermediate results while counting; otherwise false.</param>
	/// <param name="progress">Receives percentage updates for the progress bar.</param>
	/// <param name="liveResults">Receives intermediate histogram snapshots for live display.</param>
	/// <param name="cancellationToken">The token used to cancel the operation.</param>
	/// <returns>A sorted list of final histogram bins.</returns>
	/// <remarks>The method scans the supplied MPCORB lines once, counts values into bins, and optionally reports intermediate snapshots for live rendering.</remarks>
	private List<HistogramBinResult> BuildHistogram(
		HistogramDefinition definition,
		double stepSize,
		bool enableLiveDisplay,
		IProgress<int> progress,
		IProgress<List<HistogramBinResult>> liveResults,
		CancellationToken cancellationToken)
	{
		// Use a sorted dictionary to count the number of planetoids in each histogram bin, where the key is the bin index and the value is the count. The sorted dictionary ensures that the bins are automatically ordered by their index, which corresponds to their range in the histogram.
		SortedDictionary<int, int> counts = [];
		// The total number of planetoids is used to determine how frequently to report progress and live updates. Progress is reported every 1% of completion, and live updates are reported every 4% of completion, with a minimum interval of at least one processed item to ensure responsiveness even with small datasets.
		int total = _planetoids.Count;
		int progressInterval = Math.Max(val1: 1, val2: total / 100);
		int liveInterval = Math.Max(val1: 1, val2: total / 25);
		// Iterate through each planetoid record, extract the relevant value using the definition's ValueSelector, and count it into the appropriate histogram bin based on the selected step size. The bin index is calculated by dividing the value by the step size and taking the floor of the result. If the value is valid (not null and finite), it is counted into the corresponding bin in the sorted dictionary.
		for (int i = 0; i < total; i++)
		{
			// Check for cancellation at the start of each iteration to allow for responsive cancellation of the background operation. If cancellation has been requested, an OperationCanceledException will be thrown, which can be caught by the calling code to handle cleanup and update the UI accordingly.
			cancellationToken.ThrowIfCancellationRequested();
			// Use the ValueSelector function from the histogram definition to extract the numeric value from the current planetoid record. The ValueSelector is a callback that knows how to parse the specific orbital element or property from the raw MPCORB line. If the value is successfully extracted and is a finite number, it is processed for binning.
			double? value = definition.ValueSelector(arg: _planetoids[index: i]);
			// If the extracted value is valid (not null and finite), calculate the corresponding histogram bin index by dividing the value by the step size and taking the floor of the result. This determines which bin the value belongs to based on the defined step size for the histogram. The count for that bin is then incremented in the sorted dictionary.
			if (value.HasValue && double.IsFinite(d: value.Value))
			{
				// Calculate the bin index for the current value by dividing it by the step size and taking the floor of the result. This determines which histogram bin the value belongs to based on the defined step size.
				int binIndex = (int)Math.Floor(d: value.Value / stepSize);
				// Increment the count for the calculated bin index in the sorted dictionary. If the bin index does not exist in the dictionary, it will be added with an initial count of zero before incrementing. This allows for dynamic counting of bins as new indices are encountered during the iteration.
				counts.TryGetValue(key: binIndex, value: out int currentCount);
				// Update the count for the current bin index by incrementing it by one. If the bin index was not previously present in the dictionary, it will be added with a count of one.
				counts[key: binIndex] = currentCount + 1;
			}
			// Calculate the number of processed items and report progress at defined intervals. Progress is reported every 1% of completion, and live updates are reported every 4% of completion, with a minimum interval of at least one processed item to ensure responsiveness even with small datasets.
			int processed = i + 1;
			// Report progress to update the progress bar and taskbar indicator. The progress percentage is calculated based on the number of processed items relative to the total, and it is reported at defined intervals to avoid excessive updates while still providing feedback to the user.
			if (processed % progressInterval == 0 || processed == total)
			{
				// The progress percentage is calculated as the number of processed items multiplied by 100, divided by the total number of items. The Math.Max function is used to ensure that the total is at least 1 to avoid division by zero in cases where the dataset might be empty.
				progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
			}
			// If live display is enabled, report intermediate histogram results at defined intervals. Live updates are reported every 4% of completion, and the CreateHistogramResults method is called to convert the current counts into a list of histogram bin results that can be displayed in the chart and ListView. This allows the user to see the histogram being built in real-time as the data is processed.
			if (enableLiveDisplay && (processed % liveInterval == 0 || processed == total))
			{
				// Create a snapshot of the current histogram results by converting the counts into a list of HistogramBinResult objects. This snapshot represents the state of the histogram at the current point in processing and can be displayed in the chart and ListView for live updates.
				liveResults.Report(value: CreateHistogramResults(counts: counts, stepSize: stepSize));
			}
		}
		// After processing all planetoid records, convert the final counts into a sorted list of histogram bin results to be returned. The CreateHistogramResults method takes the sorted dictionary of counts and the step size to generate a list of HistogramBinResult objects that represent the final histogram bins, which can then be displayed in the chart and ListView.
		return CreateHistogramResults(counts: counts, stepSize: stepSize);
	}

	/// <summary>Converts raw bin counts into sorted histogram rows.</summary>
	/// <param name="counts">The counted planetoid totals per bin index.</param>
	/// <param name="stepSize">The width of each histogram bin.</param>
	/// <returns>A sorted list of histogram rows.</returns>
	/// <remarks>The method transforms the bin indices and counts into a list of HistogramBinResult objects, calculating the start and end values for each bin based on the step size. The resulting list is sorted by the bin index, which corresponds to the range of values for each histogram bin.</remarks>
	private static List<HistogramBinResult> CreateHistogramResults(SortedDictionary<int, int> counts, double stepSize)
		// Convert the sorted dictionary of counts into a list of HistogramBinResult objects, where each object represents a histogram bin with its start and end values calculated based on the bin index and step size. The start value of each bin is calculated as the bin index multiplied by the step size, and the end value is calculated as the start value plus the step size. The count for each bin is taken directly from the sorted dictionary.
		=> counts.Count == 0
		? []
		: [
			// Include all bin indices between the lowest and highest observed bins so zero-count gaps are represented explicitly in both chart and table output.
			.. Enumerable.Range(start: counts.First().Key, count: (counts.Last().Key - counts.First().Key) + 1).Select(selector: binIndex =>
			{
				// For each bin index in the full covered range, create a new HistogramBinResult object. The Start property is calculated as the bin index multiplied by the step size, and the End property is calculated as the start value plus the step size. The Count property is set to the count of planetoids for that bin, or zero if no values fell into that bin.
				counts.TryGetValue(key: binIndex, value: out int count);
				return new HistogramBinResult(
					Start: binIndex * stepSize,
					End: (binIndex + 1) * stepSize,
					Count: count);
			})
		];

	/// <summary>Formats a chart and ListView label for one histogram range.</summary>
	/// <param name="start">The inclusive lower boundary.</param>
	/// <param name="end">The exclusive upper boundary.</param>
	/// <param name="unitSuffix">The unit suffix to append to numeric values.</param>
	/// <returns>A formatted range label.</returns>
	/// <remarks>The method formats the start and end values of a histogram bin into a readable string, including the specified unit suffix. The start value is formatted as an inclusive lower boundary, while the end value is formatted as an exclusive upper boundary, with a double dot ("..") separator between them.</remarks>
	private static string FormatRangeLabel(double start, double end, string unitSuffix)
		// The range label is formatted to show the start and end values of the histogram bin, with the unit suffix appended to each value. The start value is formatted as an inclusive lower boundary, while the end value is formatted as an exclusive upper boundary, using a double dot ("..") separator between them for clarity.
		=> $"{FormatNumericValue(value: start)}{unitSuffix} .. {FormatNumericValue(value: end)}{unitSuffix}";

	/// <summary>Formats a numeric value for display in the chart and ListView.</summary>
	/// <param name="value">The value to format.</param>
	/// <returns>The formatted text.</returns>
	/// <remarks>The method formats a numeric value using the invariant culture to ensure consistent formatting regardless of the user's locale. The format string "0.####" is used to display up to four decimal places without trailing zeros, providing a clean and readable representation of the value for display in the chart and ListView.</remarks>
	private static string FormatNumericValue(double value) => value.ToString(format: "0.####", provider: CultureInfo.InvariantCulture);

	/// <summary>Attempts to parse a floating-point slice from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="startIndex">The inclusive start index of the numeric field.</param>
	/// <param name="length">The field length.</param>
	/// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The method extracts a substring from the specified start index and length, trims it, and attempts to parse it as a double using the invariant culture. This is used to extract various orbital elements and properties from the fixed-width fields in the raw MPCORB records.</remarks>
	private static bool TryParseValue(string line, int startIndex, int length, out double value)
	{
		// Initialize the output value to default to ensure it has a defined value in case parsing fails. The method will return false if the specified substring cannot be parsed as a double, or if the line does not contain enough characters for the specified start index and length.
		value = default;
		// Check if the line has enough characters to extract the specified substring based on the start index and length. If the line is too short, parsing cannot proceed and the method will return false.
		return line.Length >= startIndex + length && double.TryParse(
			s: line.Substring(startIndex: startIndex, length: length).Trim(),
			style: NumberStyles.Float,
			provider: CultureInfo.InvariantCulture,
			result: out value);
	}

	/// <summary>Attempts to parse the semi-major axis from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed semi-major axis if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The semi-major axis is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseSemiMajorAxis(string line, out double value) => TryParseValue(line: line, startIndex: 92, length: 11, value: out value);

	/// <summary>Attempts to parse the orbital eccentricity from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed eccentricity if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The eccentricity is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseEccentricity(string line, out double value) => TryParseValue(line: line, startIndex: 70, length: 9, value: out value);

	/// <summary>Attempts to parse the inclination from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed inclination if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The inclination is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseInclination(string line, out double value) => TryParseValue(line: line, startIndex: 59, length: 9, value: out value);

	/// <summary>Attempts to parse the mean anomaly from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean anomaly if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean anomaly is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseMeanAnomaly(string line, out double value) => TryParseValue(line: line, startIndex: 26, length: 9, value: out value);

	/// <summary>Attempts to parse the argument of perihelion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed argument of perihelion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The argument of perihelion is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseArgumentOfPerihelion(string line, out double value) => TryParseValue(line: line, startIndex: 37, length: 9, value: out value);

	/// <summary>Attempts to parse the longitude of the ascending node from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed longitude of the ascending node if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The longitude of the ascending node is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseLongitudeOfAscendingNode(string line, out double value) => TryParseValue(line: line, startIndex: 48, length: 9, value: out value);

	/// <summary>Attempts to parse the mean daily motion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean daily motion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean daily motion is located at a fixed position in the MPCORB record, and this method uses the TryParseValue helper to extract and parse it as a double.</remarks>
	private static bool TryParseMeanDailyMotion(string line, out double value) => TryParseValue(line: line, startIndex: 80, length: 11, value: out value);

	/// <summary>Attempts to parse the perihelion distance from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed perihelion distance if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The perihelion distance is calculated from the semi-major axis and eccentricity, which are located at fixed positions in the MPCORB record.</remarks>
	private static bool TryParsePerihelionDistance(string line, out double value)
	{
		value = default;
		return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) &&
		TryParseEccentricity(line: line, value: out double eccentricity) && (value = semiMajorAxis * (1 - eccentricity)) >= 0;
	}

	/// <summary>Attempts to parse the aphelion distance from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed aphelion distance if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The aphelion distance is calculated from the semi-major axis and eccentricity, which are located at fixed positions in the MPCORB record.</remarks>
	private static bool TryParseAphelionDistance(string line, out double value)
	{
		value = default;
		return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) &&
		TryParseEccentricity(line: line, value: out double eccentricity) && (value = semiMajorAxis * (1 + eccentricity)) >= 0;
	}

	/// <summary>Attempts to parse the orbital period from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed orbital period in years if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The orbital period is calculated from the semi-major axis, which is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseOrbitalPeriod(string line, out double value)
	{
		value = default;
		return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) && semiMajorAxis >= 0 && (value = Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3))) >= 0;
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the FormClosing event to cancel any running histogram operation.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the form-closing request.</param>
	/// <remarks>The running task is canceled so the form can close cleanly without leaving background work behind.</remarks>
	private void HistogramsForm_FormClosing(object? sender, FormClosingEventArgs e) => _cancellationTokenSource?.Cancel();

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the start/cancel button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click.</param>
	/// <remarks>When no task is running the handler starts histogram generation; otherwise it requests cancellation.</remarks>
	private async void ToolStripButtonStartCancel_Click(object? sender, EventArgs e)
	{
		// If a histogram generation task is currently running, request cancellation and return immediately. This allows the user to stop the ongoing operation without starting a new one. If no task is running, the method proceeds to validate the input selections and start a new histogram generation task.
		if (_cancellationTokenSource != null)
		{
			// Request cancellation of the running histogram generation task. This will signal the background operation to stop processing and allow the form to update the UI accordingly once cancellation is acknowledged.
			_cancellationTokenSource.Cancel();
			return;
		}
		// Validate that there is planetoid data available before attempting to generate a histogram. If the list of planetoids is empty, display an informational message to the user and return without starting the histogram generation process.
		if (_planetoids.Count == 0)
		{
			// Display an informational message to the user indicating that no planetoid data is available for histogram generation. This prevents the user from attempting to generate a histogram when there is no data to process, and provides clear feedback about the issue.
			_ = KryptonMessageBox.Show(text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Retrieve the selected histogram definition and step size from the UI. If either selection is invalid (null), display an informational message to the user and return without starting the histogram generation process. This ensures that the user has made valid selections before attempting to generate a histogram.
		HistogramDefinition? definition = GetSelectedDefinition();
		StepOption? step = GetSelectedStep();
		// If the histogram definition or step size is not selected, display an informational message to the user indicating that both selections are required for histogram generation. This provides clear feedback about the missing input and prevents the user from starting the process with incomplete selections.
		if (definition is null || step is null)
		{
			// Display an informational message to the user indicating that both an orbital element and a step size must be selected to generate a histogram. This ensures that the user understands the requirements for starting the histogram generation process and provides guidance on how to proceed.
			_ = KryptonMessageBox.Show(text: "Please select an orbital element and a step size.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		ResetDisplayedResults();
		ClearStatusBar(label: labelInformation);
		// Reset the progress bar to zero at the start of the histogram generation process. This ensures that the progress bar accurately reflects the progress of the new operation and provides clear feedback to the user from the beginning.
		UpdateProgress(percent: 0);
		// Update the UI to reflect that a histogram generation task is now running. This typically involves changing the text of the start/cancel button to indicate that it can be used to cancel the operation, and disabling or enabling other UI elements as appropriate to prevent conflicting actions while the task is running.
		UpdateRunningState(isRunning: true);
		// Determine whether live display is enabled based on the state of the corresponding button. If live display is enabled, intermediate results will be published while counting, allowing the user to see the histogram being built in real-time. If live display is not enabled, intermediate results will not be published, and the histogram will only be displayed once the counting process is complete.
		bool enableLiveDisplay = toolStripButtonLiveDisplay.Checked;
		// Create a new CancellationTokenSource for the histogram generation task. This token source will be used to signal cancellation if the user clicks the start/cancel button while the task is running, allowing for responsive cancellation of the background operation.
		_cancellationTokenSource = new CancellationTokenSource();
		// Start the histogram generation process on a background thread using Task.Run. The BuildHistogram method is called with the selected histogram definition, step size, live display option, progress reporter, live results reporter, and cancellation token. The method will return a list of final histogram bin results once the counting process is complete. The UI will be updated with intermediate results if live display is enabled, and the final results will be applied to the chart and ListView once the task completes.
		try
		{
			// Create progress reporters for updating the progress bar and live results. The UpdateProgress method will be called with percentage updates to reflect the progress of the histogram generation, while the liveResults reporter will receive intermediate histogram snapshots that can be displayed in real-time if live display is enabled.
			Progress<int> progress = new(handler: UpdateProgress);
			Progress<List<HistogramBinResult>> liveResults = new(handler: results => ApplyResults(definition: definition, results: results));
			// Run the BuildHistogram method on a background thread to perform the histogram generation without blocking the UI. The method will process the planetoid data, count values into bins based on the selected definition and step size, and return a list of final histogram bin results. Progress updates and live results will be reported through the provided progress reporters, and cancellation can be requested through the cancellation token.
			List<HistogramBinResult> finalResults = await Task.Run(
				function: () => BuildHistogram(
					definition: definition,
					stepSize: step.Value,
					enableLiveDisplay: enableLiveDisplay,
					progress: progress,
					liveResults: liveResults,
					cancellationToken: _cancellationTokenSource.Token),
				cancellationToken: _cancellationTokenSource.Token);
			// Once the histogram generation is complete, apply the final results to the chart and ListView. The ApplyResults method will update the UI to display the final histogram based on the generated bin results. Additionally, the information label will be updated to show a summary of the histogram, including the number of ranges and total counted planetoids.
			ApplyResults(definition: definition, results: finalResults);
			// Update the information label to summarize the results of the histogram generation. If no planetoid values were available for the selected histogram, a message indicating this will be displayed. Otherwise, a summary will show the number of ranges (bins) in the histogram and the total count of planetoids across all bins, formatted with thousands separators for readability.
			labelInformation.Text = finalResults.Count == 0
				? "No planetoid values were available for the selected histogram."
				: $"Histogram created with {finalResults.Count:N0} ranges and {finalResults.Sum(selector: static result => result.Count):N0} counted planetoids.";
		}
		// Handle the case where the histogram generation was canceled by the user.
		catch (OperationCanceledException)
		{
			// Log an informational message indicating that the histogram generation was canceled, and update the information label to reflect that the creation was canceled. This provides feedback to the user about the cancellation and ensures that the UI reflects the current state of the operation.
			logger.Info(message: "Orbit elements histogram generation was canceled by the user.");
			labelInformation.Text = "Histogram creation canceled.";
		}
		// Handle any unexpected exceptions that may occur during the histogram generation process. Log the error details and display an error message to the user indicating that an error occurred, along with the exception message for additional context. This ensures that any issues are properly logged for troubleshooting, and that the user is informed about the failure of the operation.
		catch (Exception ex)
		{
			// Log the exception details, including the message and stack trace, to help with troubleshooting and debugging. The error is logged at the error level to indicate that it is a significant issue that occurred during the histogram creation process.
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"An error has occurred during histogram creation: {ex.Message}");
		}
		// In the finally block, ensure that the CancellationTokenSource is disposed of and set to null to clean up resources. Additionally, update the UI to reflect that the histogram generation task is no longer running, allowing the user to start a new operation if desired.
		finally
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			UpdateRunningState(isRunning: false);
		}
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>Handles the SelectedIndexChanged event of the orbital-element drop-down.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>The step-size drop-down is repopulated with values that are meaningful for the selected histogram definition.</remarks>
	private void ToolStripComboBoxOrbitElement_SelectedIndexChanged(object? sender, EventArgs e)
	{
		// Retrieve the currently selected histogram definition based on the user's selection in the orbital-element drop-down. This definition contains information about the selected orbital element, including its display name, axis label, unit suffix, and available step size options. The selected definition will be used to update the UI and populate the step-size drop-down with relevant options for the chosen orbital element.
		HistogramDefinition? definition = GetSelectedDefinition();
		// Clear the step-size drop-down items to prepare for repopulation based on the newly selected histogram definition. This ensures that only relevant step size options are displayed for the selected orbital element, providing a better user experience and preventing invalid selections.
		toolStripComboBoxStepSize.Items.Clear();
		// If the selected histogram definition is null (which can occur if the selection is cleared), reset the displayed results and return without repopulating the step-size drop-down. This ensures that the UI remains consistent and does not display irrelevant options when no valid histogram definition is selected.
		if (definition is null)
		{
			// Reset the displayed results to clear any existing histogram data from the chart and ListView. This is done because there is no valid histogram definition selected, and it ensures that the UI does not show outdated or irrelevant information when the selection is cleared.
			ResetDisplayedResults();
			return;
		}
		// Repopulate the step-size drop-down with the options defined in the selected histogram definition. Each StepOption from the definition's StepOptions list is added to the drop-down, allowing the user to select a step size that is meaningful for the chosen orbital element. This provides a tailored set of options that are relevant to the specific histogram being generated.
		foreach (StepOption stepOption in definition.StepOptions)
		{
			// Add each step size option from the selected histogram definition to the step-size drop-down. The StepOption objects contain both the numeric value of the step size and a display text that is shown to the user. This allows for a user-friendly selection of step sizes that are appropriate for the selected orbital element.
			_ = toolStripComboBoxStepSize.Items.Add(item: stepOption);
		}
		// If there are step size options available for the selected histogram definition, select the first option by default. This provides a sensible default selection for the user, allowing them to start generating a histogram immediately without having to make an additional selection. If no step size options are available, reset the displayed results to clear any existing histogram data from the chart and ListView.
		if (toolStripComboBoxStepSize.Items.Count > 0)
		{
			// Select the first step size option by default if options are available. This ensures that there is a valid step size selected for the histogram generation process, and allows the user to start generating a histogram immediately without having to make an additional selection.
			toolStripComboBoxStepSize.SelectedIndex = 0;
		}
		// If no step size options are available for the selected histogram definition, reset the displayed results to clear any existing histogram data from the chart and ListView. This ensures that the UI remains consistent and does not show outdated or irrelevant information when there are no valid step size options for the selected orbital element.
		else
		{
			ResetDisplayedResults();
		}
	}

	/// <summary>Handles the SelectedIndexChanged event of the step-size drop-down.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>The empty chart is redrawn so the title and axes immediately reflect the current selection even before a run is started.</remarks>
	private void ToolStripComboBoxStepSize_SelectedIndexChanged(object? sender, EventArgs e) => UpdateHistogramPlot(definition: GetSelectedDefinition(), results: _currentResults);

	/// <summary>Handles the CheckedChanged event of the live-display button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>The button text mirrors the current state so users can immediately see whether live updates are enabled.</remarks>
	private void ToolStripButtonLiveDisplay_CheckedChanged(object? sender, EventArgs e) =>
		// Update the text of the live-display button to reflect its current checked state. If the button is checked, the text will show "On" to indicate that live updates are enabled; if it is unchecked, the text will show "Off" to indicate that live updates are disabled. This provides immediate visual feedback to the user about the state of the live display option.
		toolStripButtonLiveDisplay.Text = toolStripButtonLiveDisplay.Checked ? "On" : "Off";

	/// <summary>Handles the CheckedChanged event of the log-scale button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>Immediately redraws the chart using the current results with the new axis scaling.</remarks>
	private void ToolStripButtonLogScale_CheckedChanged(object? sender, EventArgs e) =>
		// Update the histogram plot to reflect the change in log scale setting. The UpdateHistogramPlot method will redraw the chart using the current results, applying the new axis scaling based on whether the log-scale button is checked or not. This allows the user to see the effect of enabling or disabling log scale on the histogram immediately.
		UpdateHistogramPlot(definition: GetSelectedDefinition(), results: _currentResults);

	#endregion
}
