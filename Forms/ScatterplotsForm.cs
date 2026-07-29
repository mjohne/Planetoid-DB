// This file contains the implementation of the ScatterplotsForm,
// which displays scatter plots of orbital elements and properties
// for all minor planets in the database.
using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using ScottPlot;
using ScottPlot.Plottables;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a scatter plot of two selected orbital elements or derived properties for all planetoids in the database.</summary>
/// <remarks>The form plots each planetoid as a point with user-selected X-axis and Y-axis orbital elements. The chart and a tabular ListView are shown side by side. Users can optionally request live updates while the background data-collection operation is running.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ScatterplotsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to record errors and cancellation events from the scatter-plot generation workflow.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the source MPCORB records passed in by the main form.</summary>
	/// <remarks>A local copy of the required raw data is supplied by the main program so this form can generate its scatter plot independently.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the currently running cancellation token source.</summary>
	/// <remarks>This field is non-null only while scatter-plot creation is in progress.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Stores the currently displayed scatter-plot results.</summary>
	/// <remarks>The list is refreshed whenever the diagram and the ListView are updated.</remarks>
	private List<ScatterPoint> _currentResults = [];

	/// <summary>Stores the X-axis definition used for the currently displayed results.</summary>
	private ScatterDefinition? _currentXDefinition;

	/// <summary>Stores the Y-axis definition used for the currently displayed results.</summary>
	private ScatterDefinition? _currentYDefinition;

	/// <summary>Represents one selectable scatter-plot axis definition.</summary>
	/// <param name="DisplayName">The user-facing name of the orbital element or property.</param>
	/// <param name="AxisLabel">The axis label for the chart.</param>
	/// <param name="UnitSuffix">The optional unit suffix used in formatted values.</param>
	/// <param name="ValueSelector">The callback used to extract the numeric value from a raw MPCORB line.</param>
	/// <remarks>The definition centralises presentation metadata and parsing logic for one scatter-plot axis.</remarks>
	private sealed record ScatterDefinition(
		string DisplayName,
		string AxisLabel,
		string UnitSuffix,
		Func<string, double?> ValueSelector)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The scatter definition name.</returns>
		public override string ToString() => DisplayName;
	}

	/// <summary>Represents one plotted scatter-plot data point.</summary>
	/// <param name="X">The X-axis value of the data point.</param>
	/// <param name="Y">The Y-axis value of the data point.</param>
	/// <remarks>Each data point corresponds to one planetoid whose X and Y values were both successfully parsed.</remarks>
	private readonly record struct ScatterPoint(double X, double Y);

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ScatterplotsForm"/> class.</summary>
	/// <param name="planetoids">The planetoid string records to process from the database.</param>
	/// <remarks>The main form passes the necessary raw MPCORB data to this dialog so the scatter plot can be generated without directly accessing shared UI state.</remarks>
	public ScatterplotsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Store the supplied planetoid data for scatter-plot generation.
		_planetoids = planetoids;
		// Set up the X-axis and Y-axis element selections based on predefined scatter-plot definitions.
		InitializeSelections();
		// Initialize the progress bar and taskbar progress indicator to a known state.
		UpdateRunningState(isRunning: false);
		ResetDisplayedResults();
		// The form is now ready for user interaction, and the logger can record any subsequent events.
		logger.Info(message: "ScatterplotsForm initialized with {0} planetoids.", argument: _planetoids.Count);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>The method currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Creates all scatter-plot axis definitions supported by the form.</summary>
	/// <returns>A list of selectable scatter-plot axis definitions.</returns>
	/// <remarks>The selectable items include directly stored orbital elements and a few useful derived properties computed from semi-major axis and eccentricity.</remarks>
	private static List<ScatterDefinition> CreateScatterDefinitions() =>
		[
			new ScatterDefinition(
				DisplayName: "Semi-major axis", AxisLabel: "Semi-major axis (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParseSemiMajorAxis(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Eccentricity", AxisLabel: "Eccentricity", UnitSuffix: string.Empty,
				ValueSelector: static line => TryParseEccentricity(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Inclination", AxisLabel: "Inclination (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseInclination(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Mean anomaly", AxisLabel: "Mean anomaly (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseMeanAnomaly(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Argument of perihelion", AxisLabel: "Argument of perihelion (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseArgumentOfPerihelion(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Longitude of ascending node", AxisLabel: "Longitude of ascending node (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseLongitudeOfAscendingNode(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Mean daily motion", AxisLabel: "Mean daily motion (°/day)", UnitSuffix: " °/day",
				ValueSelector: static line => TryParseMeanDailyMotion(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Perihelion distance", AxisLabel: "Perihelion distance (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParsePerihelionDistance(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Aphelion distance", AxisLabel: "Aphelion distance (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParseAphelionDistance(line: line, value: out double value) ? value : null),
			new ScatterDefinition(
				DisplayName: "Orbital period", AxisLabel: "Orbital period (years)", UnitSuffix: " years",
				ValueSelector: static line => TryParseOrbitalPeriod(line: line, value: out double value) ? value : null)
		];

	/// <summary>Initializes the selectable X-axis and Y-axis element drop-downs.</summary>
	/// <remarks>Both drop-downs are populated with the same set of scatter-plot definitions so that any element can be selected for either axis.</remarks>
	private void InitializeSelections()
	{
		// Clear and repopulate the X-axis element ComboBox with all available scatter-plot definitions.
		toolStripComboBoxXAxis.Items.Clear();
		// Clear and repopulate the Y-axis element ComboBox with all available scatter-plot definitions.
		toolStripComboBoxYAxis.Items.Clear();
		List<ScatterDefinition> definitions = CreateScatterDefinitions();
		// Add each definition to both the X-axis and Y-axis ComboBoxes.
		foreach (ScatterDefinition definition in definitions)
		{
			_ = toolStripComboBoxXAxis.Items.Add(item: definition);
			_ = toolStripComboBoxYAxis.Items.Add(item: definition);
		}
		// Select the first element for the X-axis by default, and the second for the Y-axis (if available).
		if (toolStripComboBoxXAxis.Items.Count > 0)
		{
			// Trigger SelectedIndexChanged to refresh the chart with the default selection.
			toolStripComboBoxXAxis.SelectedIndex = 0;
		}
		if (toolStripComboBoxYAxis.Items.Count > 1)
		{
			// Select the second item as the default Y-axis so that both axes show different elements by default.
			toolStripComboBoxYAxis.SelectedIndex = 1;
		}
		else if (toolStripComboBoxYAxis.Items.Count > 0)
		{
			toolStripComboBoxYAxis.SelectedIndex = 0;
		}
	}

	/// <summary>Gets the currently selected X-axis scatter-plot definition.</summary>
	/// <returns>The selected X-axis scatter-plot definition, or <see langword="null"/> if none is selected.</returns>
	/// <remarks>The method casts the selected item to a <see cref="ScatterDefinition"/>; if the cast fails, it returns <see langword="null"/>.</remarks>
	private ScatterDefinition? GetSelectedXDefinition() => toolStripComboBoxXAxis.SelectedItem as ScatterDefinition;

	/// <summary>Gets the currently selected Y-axis scatter-plot definition.</summary>
	/// <returns>The selected Y-axis scatter-plot definition, or <see langword="null"/> if none is selected.</returns>
	/// <remarks>The method casts the selected item to a <see cref="ScatterDefinition"/>; if the cast fails, it returns <see langword="null"/>.</remarks>
	private ScatterDefinition? GetSelectedYDefinition() => toolStripComboBoxYAxis.SelectedItem as ScatterDefinition;

	/// <summary>Updates the toolbar state to reflect whether scatter-plot creation is running.</summary>
	/// <param name="isRunning">True while the background task is active; otherwise false.</param>
	/// <remarks>The same toolbar button is used for both starting and canceling the operation.</remarks>
	private void UpdateRunningState(bool isRunning)
	{
		// Update the start/cancel button text and icon based on the running state.
		toolStripButtonStartCancel.Text = isRunning ? "&Cancel" : "&Start";
		// The icons are from the Fatcow set, which is included in the project resources.
		toolStripButtonStartCancel.Image = isRunning
			? Resources.FatcowIcons16px.fatcow_cancel_16px
			: Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		// Enable or disable the axis element selections and the live display option based on whether scatter-plot generation is currently running.
		toolStripComboBoxXAxis.Enabled = !isRunning;
		toolStripComboBoxYAxis.Enabled = !isRunning;
		toolStripButtonLiveDisplay.Enabled = !isRunning;
	}

	/// <summary>Resets the displayed ListView and chart results.</summary>
	/// <remarks>The method clears previously collected data points and redraws the empty chart using the currently selected definitions.</remarks>
	private void ResetDisplayedResults()
	{
		// Clear the current results list and the ListView items.
		_currentResults = [];
		_currentXDefinition = null;
		_currentYDefinition = null;
		listViewResults.VirtualListSize = 0;
		// Update the scatter plot to reflect the cleared results.
		UpdateScatterPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), results: _currentResults);
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
		// Update the taskbar progress indicator for this window, if supported and if the form's handle has been created.
		if (IsHandleCreated)
		{
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
		}
	}

	/// <summary>Applies collected scatter-plot data points to both the ListView and the chart.</summary>
	/// <param name="xDefinition">The definition for the X-axis orbital element.</param>
	/// <param name="yDefinition">The definition for the Y-axis orbital element.</param>
	/// <param name="results">The data points to display.</param>
	/// <returns>The number of data points excluded from the plot because of a non-positive value under the active log scale.</returns>
	/// <remarks>The method keeps the chart and the tabular view synchronised.</remarks>
	private int ApplyResults(ScatterDefinition? xDefinition, ScatterDefinition? yDefinition, List<ScatterPoint> results)
	{
		_currentResults = results;
		_currentXDefinition = xDefinition;
		_currentYDefinition = yDefinition;
		// VirtualMode: only set the count; no ListViewItem objects are created.
		listViewResults.VirtualListSize = _currentResults.Count;
		listViewResults.Invalidate();
		return UpdateScatterPlot(xDefinition: xDefinition, yDefinition: yDefinition, results: results);
	}

	/// <summary>Redraws the ScottPlot scatter plot based on the supplied results.</summary>
	/// <param name="xDefinition">The definition for the X-axis orbital element.</param>
	/// <param name="yDefinition">The definition for the Y-axis orbital element.</param>
	/// <param name="results">The data points to render.</param>
	/// <returns>The number of data points excluded from the plot because of a non-positive value under the active log scale.</returns>
	/// <remarks>The chart always contains a title, axis labels, and a legend. When no results are available, the axes are still configured but no points are plotted.</remarks>
	private int UpdateScatterPlot(ScatterDefinition? xDefinition, ScatterDefinition? yDefinition, IReadOnlyList<ScatterPoint> results)
	{
		// Clear existing plots and configure the title and axis labels based on the selected definitions.
		formsPlotScatterplot.Plot.Clear();
		formsPlotScatterplot.Plot.Title(text: xDefinition is null || yDefinition is null
			? "Orbital elements scatter plot"
			: $"Scatter plot of {xDefinition.DisplayName} vs. {yDefinition.DisplayName}");
		formsPlotScatterplot.Plot.Axes.Bottom.Label.Text = xDefinition?.AxisLabel ?? "X axis";
		formsPlotScatterplot.Plot.Axes.Left.Label.Text = yDefinition?.AxisLabel ?? "Y axis";
		formsPlotScatterplot.Plot.Legend.IsVisible = true;
		formsPlotScatterplot.Plot.Legend.Alignment = Alignment.UpperRight;
		int excluded = 0;
		// If valid definitions are provided and there are results to display, create a scatter plot.
		if (xDefinition is not null && yDefinition is not null && results.Count > 0)
		{
			bool logX = toolStripButtonLogScaleX.Checked;
			bool logY = toolStripButtonLogScaleY.Checked;
			// Extract the X and Y coordinate arrays from the data points in a single pass.
			// When log scale is active, values are log10-transformed so the axis spacing is logarithmic.
			List<double> xList = new(capacity: results.Count);
			List<double> yList = new(capacity: results.Count);
			for (int i = 0; i < results.Count; i++)
			{
				double x = results[index: i].X;
				double y = results[index: i].Y;
				if (logX)
				{
					if (x <= 0)
					{
						excluded++;
						continue;
					}
					x = Math.Log10(d: x);
				}
				if (logY)
				{
					if (y <= 0)
					{
						excluded++;
						continue;
					}
					y = Math.Log10(d: y);
				}
				xList.Add(item: x);
				yList.Add(item: y);
			}
			double[] xs = [.. xList];
			double[] ys = [.. yList];
			// Add a scatter plot to the ScottPlot with the collected X and Y values.
			Scatter scatter = formsPlotScatterplot.Plot.Add.Scatter(xs: xs, ys: ys);
			scatter.LegendText = "Planetoids";
			scatter.Color = Colors.SteelBlue;
			scatter.MarkerSize = 2;
			scatter.LineWidth = 0;
		}
		// Apply logarithmic scale on the X axis if the toggle button is checked:
		// X values are already log10-transformed in UpdateScatterPlot when log scale is active.
		// Replace the tick generator with one that formats ticks as powers of 10.
		if (toolStripButtonLogScaleX.Checked)
		{
			ScottPlot.TickGenerators.NumericAutomatic logTicks = new()
			{
				LabelFormatter = static v => $"10^{v:0.##}",
				MinorTickGenerator = new ScottPlot.TickGenerators.LogMinorTickGenerator()
			};
			formsPlotScatterplot.Plot.Axes.Bottom.TickGenerator = logTicks;
		}
		else
		{
			formsPlotScatterplot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic();
		}
		// Apply logarithmic scale on the Y axis if the toggle button is checked.
		if (toolStripButtonLogScaleY.Checked)
		{
			ScottPlot.TickGenerators.NumericAutomatic logTicks = new()
			{
				LabelFormatter = static v => $"10^{v:0.##}",
				MinorTickGenerator = new ScottPlot.TickGenerators.LogMinorTickGenerator()
			};
			formsPlotScatterplot.Plot.Axes.Left.TickGenerator = logTicks;
		}
		else
		{
			formsPlotScatterplot.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic();
		}
		// Auto-scale the axes to fit the displayed data.
		formsPlotScatterplot.Plot.Axes.AutoScale();
		// Refresh the plot to apply all changes and render the updated scatter plot.
		formsPlotScatterplot.Refresh();
		return excluded;
	}

	/// <summary>Builds scatter-plot data points for the selected definitions on a background thread.</summary>
	/// <param name="xDefinition">The selected X-axis scatter-plot definition.</param>
	/// <param name="yDefinition">The selected Y-axis scatter-plot definition.</param>
	/// <param name="enableLiveDisplay">True to publish intermediate results while collecting; otherwise false.</param>
	/// <param name="progress">Receives percentage updates for the progress bar.</param>
	/// <param name="liveResults">Receives intermediate scatter-plot snapshots for live display.</param>
	/// <param name="cancellationToken">The token used to cancel the operation.</param>
	/// <returns>A list of final scatter-plot data points.</returns>
	/// <remarks>The method scans the supplied MPCORB lines once, extracts X and Y values for each planetoid, and optionally reports intermediate snapshots for live rendering.</remarks>
	private List<ScatterPoint> BuildScatterData(
		ScatterDefinition xDefinition,
		ScatterDefinition yDefinition,
		bool enableLiveDisplay,
		IProgress<int> progress,
		IProgress<List<ScatterPoint>> liveResults,
		CancellationToken cancellationToken)
	{
		// Accumulate the collected data points for all planetoids in a list.
		List<ScatterPoint> points = [];
		// The total number of planetoids is used to determine how frequently to report progress and live updates.
		int total = _planetoids.Count;
		int progressInterval = Math.Max(val1: 1, val2: total / 100);
		int liveInterval = Math.Max(val1: 1, val2: total / 25);
		// Iterate through each planetoid record, extract both X and Y values using the definitions' ValueSelectors.
		for (int i = 0; i < total; i++)
		{
			// Check for cancellation at the start of each iteration.
			cancellationToken.ThrowIfCancellationRequested();
			// Extract the X value from the current planetoid record.
			double? xValue = xDefinition.ValueSelector(arg: _planetoids[index: i]);
			// Extract the Y value from the current planetoid record.
			double? yValue = yDefinition.ValueSelector(arg: _planetoids[index: i]);
			// If both values are valid finite numbers, add the data point to the list.
			if (xValue.HasValue && double.IsFinite(d: xValue.Value)
				&& yValue.HasValue && double.IsFinite(d: yValue.Value))
			{
				// Add a new ScatterPoint with the extracted X and Y values.
				points.Add(item: new ScatterPoint(X: xValue.Value, Y: yValue.Value));
			}
			// Report progress at defined intervals.
			int processed = i + 1;
			if (processed % progressInterval == 0 || processed == total)
			{
				progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
			}
			// If live display is enabled, report a snapshot of the collected points for chart-only update.
			if (enableLiveDisplay && (processed % liveInterval == 0 || processed == total))
			{
				liveResults.Report(value: CreateLivePreviewSnapshot(points: points));
			}
		}
		return points;
	}

	/// <summary>Creates a bounded snapshot for live chart updates without copying the full accumulated results list.</summary>
	/// <param name="points">The currently accumulated scatter points.</param>
	/// <returns>A sampled snapshot suitable for intermediate live rendering.</returns>
	/// <remarks>The method creates a snapshot of the accumulated scatter points, limiting the number of points to a maximum for efficient live rendering.</remarks>
	private static List<ScatterPoint> CreateLivePreviewSnapshot(List<ScatterPoint> points)
	{
		const int maxPreviewPoints = 20_000;
		if (points.Count <= maxPreviewPoints)
		{
			return [.. points];
		}

		int step = (int)Math.Ceiling((double)points.Count / maxPreviewPoints);
		List<ScatterPoint> previewPoints = new(capacity: maxPreviewPoints);
		for (int i = 0; i < points.Count; i += step)
		{
			previewPoints.Add(item: points[i]);
		}

		return previewPoints;
	}

	/// <summary>Formats a numeric value for display in the chart and ListView.</summary>
	/// <param name="value">The value to format.</param>
	/// <returns>The formatted text.</returns>
	/// <remarks>The method formats a numeric value using the invariant culture to ensure consistent formatting regardless of the user's locale.</remarks>
	private static string FormatNumericValue(double value) => value.ToString(format: "0.####", provider: CultureInfo.InvariantCulture);

	/// <summary>Attempts to parse a floating-point slice from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="startIndex">The inclusive start index of the numeric field.</param>
	/// <param name="length">The field length.</param>
	/// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The method checks that the specified slice is within the bounds of the line and attempts to parse it as a double using invariant culture formatting. The slice is trimmed of whitespace before parsing.</remarks>
	private static bool TryParseValue(string line, int startIndex, int length, out double value)
	{
		value = default;
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
	/// <remarks>The semi-major axis is in astronomical units (AU), as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseSemiMajorAxis(string line, out double value) => TryParseValue(line: line, startIndex: 92, length: 11, value: out value);

	/// <summary>Attempts to parse the orbital eccentricity from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed eccentricity if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The eccentricity is a unitless value, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseEccentricity(string line, out double value) => TryParseValue(line: line, startIndex: 70, length: 9, value: out value);

	/// <summary>Attempts to parse the inclination from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed inclination if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The inclination is in degrees, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseInclination(string line, out double value) => TryParseValue(line: line, startIndex: 59, length: 9, value: out value);

	/// <summary>Attempts to parse the mean anomaly from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean anomaly if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean anomaly is in degrees, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseMeanAnomaly(string line, out double value) => TryParseValue(line: line, startIndex: 26, length: 9, value: out value);

	/// <summary>Attempts to parse the argument of perihelion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed argument of perihelion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The argument of perihelion is in degrees, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseArgumentOfPerihelion(string line, out double value) => TryParseValue(line: line, startIndex: 37, length: 9, value: out value);

	/// <summary>Attempts to parse the longitude of the ascending node from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed longitude of the ascending node if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The longitude of the ascending node is in degrees, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseLongitudeOfAscendingNode(string line, out double value) => TryParseValue(line: line, startIndex: 48, length: 9, value: out value);

	/// <summary>Attempts to parse the mean daily motion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean daily motion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean daily motion is in degrees per day, as specified in the MPCORB format documentation.</remarks>
	private static bool TryParseMeanDailyMotion(string line, out double value) => TryParseValue(line: line, startIndex: 80, length: 11, value: out value);

	/// <summary>Attempts to parse the perihelion distance from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed perihelion distance if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The perihelion distance is calculated using the semi-major axis and eccentricity, assuming the semi-major axis is in astronomical units (AU) and the distance is in AU.</remarks>
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
	/// <remarks>The aphelion distance is calculated using the semi-major axis and eccentricity, assuming the semi-major axis is in astronomical units (AU) and the distance is in AU.</remarks>
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
	/// <remarks>The orbital period is calculated using Kepler's third law, assuming the semi-major axis is in astronomical units (AU) and the period is in years.</remarks>
	private static bool TryParseOrbitalPeriod(string line, out double value)
	{
		value = default;
		return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) && semiMajorAxis >= 0 && (value = Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3))) >= 0;
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the FormClosing event to cancel any running scatter-plot operation.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the form-closing request.</param>
	/// <remarks>The running task is canceled so the form can close cleanly without leaving background work behind.</remarks>
	private void ScatterplotsForm_FormClosing(object? sender, FormClosingEventArgs e) => _cancellationTokenSource?.Cancel();

	/// <summary>Handles the RetrieveVirtualItem event of the ListView to supply items on demand.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data containing the requested item index.</param>
	/// <remarks>Called by the ListView in VirtualMode instead of storing all items in memory.</remarks>
	private void ListViewResults_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		if (e.ItemIndex < 0 || e.ItemIndex >= _currentResults.Count)
		{
			e.Item = new ListViewItem();
			return;
		}

		ScatterPoint point = _currentResults[e.ItemIndex];
		ListViewItem item = new(text: FormatNumericValue(value: point.X) + (_currentXDefinition?.UnitSuffix ?? string.Empty))
		{
			ToolTipText = $"X: {FormatNumericValue(value: point.X)}{_currentXDefinition?.UnitSuffix}, Y: {FormatNumericValue(value: point.Y)}{_currentYDefinition?.UnitSuffix}"
		};
		_ = item.SubItems.Add(text: FormatNumericValue(value: point.Y) + (_currentYDefinition?.UnitSuffix ?? string.Empty));
		e.Item = item;
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the start/cancel button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click.</param>
	/// <remarks>When no task is running the handler starts scatter-plot generation; otherwise it requests cancellation.</remarks>
	private async void ToolStripButtonStartCancel_Click(object? sender, EventArgs e)
	{
		// If a scatter-plot generation task is currently running, request cancellation and return immediately.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			return;
		}
		// Validate that there is planetoid data available before attempting to generate a scatter plot.
		if (_planetoids.Count == 0)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Retrieve the selected X-axis and Y-axis definitions from the UI.
		ScatterDefinition? xDefinition = GetSelectedXDefinition();
		ScatterDefinition? yDefinition = GetSelectedYDefinition();
		// If either definition is not selected, display an informational message to the user.
		if (xDefinition is null || yDefinition is null)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "Please select an orbital element for both axes.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		ResetDisplayedResults();
		ClearStatusBar(label: labelInformation);
		// Reset the progress bar to zero at the start of the scatter-plot generation process.
		UpdateProgress(percent: 0);
		// Update the UI to reflect that a scatter-plot generation task is now running.
		UpdateRunningState(isRunning: true);
		// Determine whether live display is enabled based on the state of the corresponding button.
		bool enableLiveDisplay = toolStripButtonLiveDisplay.Checked;
		// Create a new CancellationTokenSource for the scatter-plot generation task.
		_cancellationTokenSource = new CancellationTokenSource();
		try
		{
			// Create progress reporters for updating the progress bar and live results.
			Progress<int> progress = new(handler: UpdateProgress);
			// Live display updates only the chart to avoid expensive ListView rebuilds during data collection.
			Progress<List<ScatterPoint>> liveResults = new(handler: results => UpdateScatterPlot(xDefinition: xDefinition, yDefinition: yDefinition, results: results));
			// Run the BuildScatterData method on a background thread to perform the data collection without blocking the UI.
			List<ScatterPoint> finalResults = await Task.Run(
				function: () => BuildScatterData(
					xDefinition: xDefinition,
					yDefinition: yDefinition,
					enableLiveDisplay: enableLiveDisplay,
					progress: progress,
					liveResults: liveResults,
					cancellationToken: _cancellationTokenSource.Token),
				cancellationToken: _cancellationTokenSource.Token);
			// Once the scatter-plot generation is complete, apply the final results to the chart and ListView.
			int excluded = ApplyResults(xDefinition: xDefinition, yDefinition: yDefinition, results: finalResults);
			// Update the information label to summarise the results of the scatter-plot generation.
			labelInformation.Text = finalResults.Count == 0
				? "No planetoid values were available for the selected scatter plot."
				: excluded > 0
					? $"Scatter plot created with {finalResults.Count - excluded:N0} plotted planetoids ({excluded:N0} excluded: non-positive value for log scale)."
					: $"Scatter plot created with {finalResults.Count:N0} plotted planetoids.";
		}
		// Handle the case where the scatter-plot generation was canceled by the user.
		catch (OperationCanceledException)
		{
			logger.Info(message: "Orbital elements scatter-plot generation was canceled by the user.");
			labelInformation.Text = "Scatter plot creation canceled.";
		}
		// Handle any unexpected exceptions that may occur during the scatter-plot generation process.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"An error has occurred during scatter plot creation: {ex.Message}");
		}
		// In the finally block, ensure that the CancellationTokenSource is disposed of and set to null.
		finally
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			UpdateRunningState(isRunning: false);
		}
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>Handles the SelectedIndexChanged event of the X-axis drop-down.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>The empty chart is redrawn so the title and axes immediately reflect the current X-axis selection.</remarks>
	private void ToolStripComboBoxXAxis_SelectedIndexChanged(object? sender, EventArgs e) =>
		UpdateScatterPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), results: _currentResults);

	/// <summary>Handles the SelectedIndexChanged event of the Y-axis drop-down.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>The empty chart is redrawn so the title and axes immediately reflect the current Y-axis selection.</remarks>
	private void ToolStripComboBoxYAxis_SelectedIndexChanged(object? sender, EventArgs e) =>
		UpdateScatterPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), results: _currentResults);

	/// <summary>Handles the CheckedChanged event of the live-display button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>The button text mirrors the current state so users can immediately see whether live updates are enabled.</remarks>
	private void ToolStripButtonLiveDisplay_CheckedChanged(object? sender, EventArgs e) =>
		toolStripButtonLiveDisplay.Text = toolStripButtonLiveDisplay.Checked ? "On" : "Off";

	/// <summary>Handles the CheckedChanged event of the X-axis logarithmic scale button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>Immediately redraws the chart with the updated axis scale without requiring a new data run.</remarks>
	private void ToolStripButtonLogScaleX_CheckedChanged(object? sender, EventArgs e)
	{
		int excluded = UpdateScatterPlot(xDefinition: _currentXDefinition, yDefinition: _currentYDefinition, results: _currentResults);
		if (_currentResults.Count > 0)
		{
			labelInformation.Text = excluded > 0
				? $"{_currentResults.Count - excluded:N0} of {_currentResults.Count:N0} points plotted ({excluded:N0} excluded: non-positive value for log scale)."
				: $"Scatter plot updated with {_currentResults.Count:N0} plotted planetoids.";
		}
	}

	/// <summary>Handles the CheckedChanged event of the Y-axis logarithmic scale button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>Immediately redraws the chart with the updated axis scale without requiring a new data run.</remarks>
	private void ToolStripButtonLogScaleY_CheckedChanged(object? sender, EventArgs e)
	{
		int excluded = UpdateScatterPlot(xDefinition: _currentXDefinition, yDefinition: _currentYDefinition, results: _currentResults);
		if (_currentResults.Count > 0)
		{
			labelInformation.Text = excluded > 0
				? $"{_currentResults.Count - excluded:N0} of {_currentResults.Count:N0} points plotted ({excluded:N0} excluded: non-positive value for log scale)."
				: $"Scatter plot updated with {_currentResults.Count:N0} plotted planetoids.";
		}
	}

	#endregion
}