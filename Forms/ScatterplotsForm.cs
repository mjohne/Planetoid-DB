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

/// <summary>Displays a scatter plot of planetoid orbital elements or derived properties for two selectable axes.</summary>
/// <remarks>The form extracts numeric values for both axes from all planetoid records, renders the result as a ScottPlot scatter chart, and provides live-update support while the background extraction is running.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ScatterplotsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to record errors and cancellation events from the scatter plot generation workflow.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the source MPCORB records passed in by the main form.</summary>
	/// <remarks>A local copy of the required raw data is supplied by the main program so this form can generate its scatter plot independently.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the currently running cancellation token source.</summary>
	/// <remarks>This field is non-null only while scatter plot creation is in progress.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Stores the currently displayed scatter plot data points.</summary>
	/// <remarks>The arrays are refreshed whenever the chart is updated.</remarks>
	private double[] _currentXs = [];
	private double[] _currentYs = [];

	/// <summary>Represents one selectable scatter plot axis definition.</summary>
	/// <param name="DisplayName">The user-facing name of the orbital element or property.</param>
	/// <param name="AxisLabel">The axis label for the chart.</param>
	/// <param name="UnitSuffix">The optional unit suffix used in formatted values.</param>
	/// <param name="ValueSelector">The callback used to extract the numeric value from a raw MPCORB line.</param>
	/// <remarks>The definition centralizes presentation metadata and parsing logic for one scatter plot axis.</remarks>
	private sealed record ScatterplotDefinition(
		string DisplayName,
		string AxisLabel,
		string UnitSuffix,
		Func<string, double?> ValueSelector)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The scatter plot definition name.</returns>
		public override string ToString() => DisplayName;
	}

	/// <summary>Represents the extracted scatter plot data points.</summary>
	/// <param name="Xs">The X-axis values for all valid planetoid data points.</param>
	/// <param name="Ys">The Y-axis values for all valid planetoid data points.</param>
	/// <remarks>Only planetoids where both X and Y values could be extracted are included.</remarks>
	private sealed record ScatterplotResult(double[] Xs, double[] Ys);

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ScatterplotsForm"/> class.</summary>
	/// <param name="planetoids">The planetoid string records to process from the database.</param>
	/// <remarks>The main form passes the necessary raw MPCORB data to this dialog so the scatter plot can be generated without directly accessing shared UI state.</remarks>
	public ScatterplotsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Store the supplied planetoid data for scatter plot generation.
		_planetoids = planetoids;
		// Set up the orbital element selection for both X and Y axes based on predefined scatter plot definitions.
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
	/// <remarks>The method currently returns the same string as <see cref="ToString"/>, but it can be customized to include more specific information if needed.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Creates all scatter plot axis definitions supported by the form.</summary>
	/// <returns>A list of selectable scatter plot definitions.</returns>
	/// <remarks>The selectable items include directly stored orbital elements and a few useful derived properties computed from semi-major axis and eccentricity.</remarks>
	private static List<ScatterplotDefinition> CreateScatterplotDefinitions() =>
		[
			new ScatterplotDefinition(
				DisplayName: "Semi-major axis", AxisLabel: "Semi-major axis (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParseSemiMajorAxis(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Eccentricity", AxisLabel: "Eccentricity", UnitSuffix: string.Empty,
				ValueSelector: static line => TryParseEccentricity(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Inclination", AxisLabel: "Inclination (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseInclination(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Mean anomaly", AxisLabel: "Mean anomaly (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseMeanAnomaly(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Argument of perihelion", AxisLabel: "Argument of perihelion (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseArgumentOfPerihelion(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Longitude of ascending node", AxisLabel: "Longitude of ascending node (°)", UnitSuffix: "°",
				ValueSelector: static line => TryParseLongitudeOfAscendingNode(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Mean daily motion", AxisLabel: "Mean daily motion (°/day)", UnitSuffix: " °/day",
				ValueSelector: static line => TryParseMeanDailyMotion(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Perihelion distance", AxisLabel: "Perihelion distance (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParsePerihelionDistance(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Aphelion distance", AxisLabel: "Aphelion distance (AU)", UnitSuffix: " AU",
				ValueSelector: static line => TryParseAphelionDistance(line: line, value: out double value) ? value : null),
			new ScatterplotDefinition(
				DisplayName: "Orbital period", AxisLabel: "Orbital period (years)", UnitSuffix: " years",
				ValueSelector: static line => TryParseOrbitalPeriod(line: line, value: out double value) ? value : null)
		];

	/// <summary>Initializes the selectable orbital element drop-downs for both X and Y axes.</summary>
	/// <remarks>Both drop-downs are populated with the same set of definitions.</remarks>
	private void InitializeSelections()
	{
		// Clear and populate both axis ComboBoxes with all available scatter plot definitions.
		toolStripComboBoxXAxis.Items.Clear();
		toolStripComboBoxYAxis.Items.Clear();
		// Populate both ComboBoxes with the same definitions so either axis can display any orbital element or property.
		foreach (ScatterplotDefinition definition in CreateScatterplotDefinitions())
		{
			_ = toolStripComboBoxXAxis.Items.Add(item: definition);
			_ = toolStripComboBoxYAxis.Items.Add(item: definition);
		}
		// Select the first definition for the X-axis and the second for the Y-axis by default, if available.
		if (toolStripComboBoxXAxis.Items.Count > 0)
		{
			toolStripComboBoxXAxis.SelectedIndex = 0;
		}
		if (toolStripComboBoxYAxis.Items.Count > 1)
		{
			toolStripComboBoxYAxis.SelectedIndex = 1;
		}
		else if (toolStripComboBoxYAxis.Items.Count > 0)
		{
			toolStripComboBoxYAxis.SelectedIndex = 0;
		}
	}

	/// <summary>Gets the currently selected X-axis scatter plot definition.</summary>
	/// <returns>The selected definition, or <see langword="null"/> if none is selected.</returns>
	private ScatterplotDefinition? GetSelectedXDefinition() => toolStripComboBoxXAxis.SelectedItem as ScatterplotDefinition;

	/// <summary>Gets the currently selected Y-axis scatter plot definition.</summary>
	/// <returns>The selected definition, or <see langword="null"/> if none is selected.</returns>
	private ScatterplotDefinition? GetSelectedYDefinition() => toolStripComboBoxYAxis.SelectedItem as ScatterplotDefinition;

	/// <summary>Updates the toolbar state to reflect whether scatter plot creation is running.</summary>
	/// <param name="isRunning">True while the background task is active; otherwise false.</param>
	/// <remarks>The same toolbar button is used for both starting and canceling the operation.</remarks>
	private void UpdateRunningState(bool isRunning)
	{
		// Update the start/cancel button text and icon based on the running state.
		toolStripButtonStartCancel.Text = isRunning ? "&Cancel" : "&Start";
		toolStripButtonStartCancel.Image = isRunning
			? Resources.FatcowIcons16px.fatcow_cancel_16px
			: Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		// Enable or disable the axis selection and live display option based on whether the scatter plot generation is currently running.
		toolStripComboBoxXAxis.Enabled = !isRunning;
		toolStripComboBoxYAxis.Enabled = !isRunning;
		toolStripButtonLiveDisplay.Enabled = !isRunning;
	}

	/// <summary>Resets the displayed chart results.</summary>
	/// <remarks>The method clears previously plotted data and redraws the empty chart using the currently selected definitions.</remarks>
	private void ResetDisplayedResults()
	{
		// Clear the current data points.
		_currentXs = [];
		_currentYs = [];
		// Update the scatter plot to reflect the cleared results.
		UpdateScatterplotPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), xs: _currentXs, ys: _currentYs);
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

	/// <summary>Redraws the ScottPlot scatter plot based on the supplied data points.</summary>
	/// <param name="xDefinition">The X-axis definition that produced the data.</param>
	/// <param name="yDefinition">The Y-axis definition that produced the data.</param>
	/// <param name="xs">The X-axis values to plot.</param>
	/// <param name="ys">The Y-axis values to plot.</param>
	/// <remarks>The chart always contains a title, axis labels, and a legend. When no data is available, the axes are still configured but no points are plotted.</remarks>
	private void UpdateScatterplotPlot(ScatterplotDefinition? xDefinition, ScatterplotDefinition? yDefinition, double[] xs, double[] ys)
	{
		// Clear existing plots and configure the title, axis labels, and legend.
		formsPlotScatterplot.Plot.Clear();
		string title = xDefinition is not null && yDefinition is not null
			? $"{yDefinition.DisplayName} vs {xDefinition.DisplayName}"
			: "Orbit elements scatter plot";
		formsPlotScatterplot.Plot.Title(text: title);
		formsPlotScatterplot.Plot.Axes.Bottom.Label.Text = xDefinition?.AxisLabel ?? "X-axis";
		formsPlotScatterplot.Plot.Axes.Left.Label.Text = yDefinition?.AxisLabel ?? "Y-axis";
		formsPlotScatterplot.Plot.Legend.IsVisible = true;
		formsPlotScatterplot.Plot.Legend.Alignment = Alignment.UpperRight;
		// If valid definitions are provided and there are data points to display, create a scatter plot.
		if (xDefinition is not null && yDefinition is not null && xs.Length > 0 && ys.Length > 0)
		{
			Scatter scatter = formsPlotScatterplot.Plot.Add.Scatter(xs: xs, ys: ys);
			scatter.LegendText = "Planetoids";
			scatter.Color = Colors.SteelBlue;
			scatter.LineWidth = 0;
			scatter.MarkerSize = 2;
		}
		// Auto-scale the axes to fit the displayed data.
		formsPlotScatterplot.Plot.Axes.AutoScale();
		// Refresh the plot to apply all changes and render the updated scatter plot.
		formsPlotScatterplot.Refresh();
	}

	/// <summary>Builds scatter plot data for the selected definitions on a background thread.</summary>
	/// <param name="xDefinition">The selected X-axis definition.</param>
	/// <param name="yDefinition">The selected Y-axis definition.</param>
	/// <param name="enableLiveDisplay">True to publish intermediate results while extracting; otherwise false.</param>
	/// <param name="progress">Receives percentage updates for the progress bar.</param>
	/// <param name="liveResults">Receives intermediate scatter plot snapshots for live display.</param>
	/// <param name="cancellationToken">The token used to cancel the operation.</param>
	/// <returns>A result containing the extracted X and Y value arrays.</returns>
	/// <remarks>The method scans the supplied MPCORB lines once, extracts X and Y values from each record, and optionally reports intermediate snapshots for live rendering.</remarks>
	private ScatterplotResult BuildScatterplot(
		ScatterplotDefinition xDefinition,
		ScatterplotDefinition yDefinition,
		bool enableLiveDisplay,
		IProgress<int> progress,
		IProgress<ScatterplotResult> liveResults,
		CancellationToken cancellationToken)
	{
		// Use lists to collect the valid X and Y data points.
		List<double> xValues = [];
		List<double> yValues = [];
		// Calculate intervals for progress and live updates.
		int total = _planetoids.Count;
		int progressInterval = Math.Max(val1: 1, val2: total / 100);
		int liveInterval = Math.Max(val1: 1, val2: total / 25);
		// Iterate through each planetoid record and extract both X and Y values.
		for (int i = 0; i < total; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			// Extract both X and Y values from the current planetoid record.
			double? xValue = xDefinition.ValueSelector(arg: _planetoids[index: i]);
			double? yValue = yDefinition.ValueSelector(arg: _planetoids[index: i]);
			// Only include data points where both X and Y values are valid and finite.
			if (xValue.HasValue && double.IsFinite(d: xValue.Value) && yValue.HasValue && double.IsFinite(d: yValue.Value))
			{
				xValues.Add(item: xValue.Value);
				yValues.Add(item: yValue.Value);
			}
			// Report progress at defined intervals.
			int processed = i + 1;
			if (processed % progressInterval == 0 || processed == total)
			{
				progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
			}
			// If live display is enabled, report intermediate scatter plot results at defined intervals.
			if (enableLiveDisplay && (processed % liveInterval == 0 || processed == total))
			{
				liveResults.Report(value: new ScatterplotResult(Xs: [.. xValues], Ys: [.. yValues]));
			}
		}
		// Return the final collected data points.
		return new ScatterplotResult(Xs: [.. xValues], Ys: [.. yValues]);
	}

	/// <summary>Attempts to parse a floating-point slice from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="startIndex">The inclusive start index of the numeric field.</param>
	/// <param name="length">The field length.</param>
	/// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The method extracts a substring from the specified start index and length, trims it, and attempts to parse it as a double using the invariant culture.</remarks>
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
	/// <remarks>The semi-major axis is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseSemiMajorAxis(string line, out double value) => TryParseValue(line: line, startIndex: 92, length: 11, value: out value);

	/// <summary>Attempts to parse the orbital eccentricity from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed eccentricity if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The eccentricity is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseEccentricity(string line, out double value) => TryParseValue(line: line, startIndex: 70, length: 9, value: out value);

	/// <summary>Attempts to parse the inclination from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed inclination if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The inclination is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseInclination(string line, out double value) => TryParseValue(line: line, startIndex: 59, length: 9, value: out value);

	/// <summary>Attempts to parse the mean anomaly from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean anomaly if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean anomaly is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseMeanAnomaly(string line, out double value) => TryParseValue(line: line, startIndex: 26, length: 9, value: out value);

	/// <summary>Attempts to parse the argument of perihelion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed argument of perihelion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The argument of perihelion is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseArgumentOfPerihelion(string line, out double value) => TryParseValue(line: line, startIndex: 37, length: 9, value: out value);

	/// <summary>Attempts to parse the longitude of the ascending node from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed longitude of the ascending node if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The longitude of the ascending node is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseLongitudeOfAscendingNode(string line, out double value) => TryParseValue(line: line, startIndex: 48, length: 9, value: out value);

	/// <summary>Attempts to parse the mean daily motion from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed mean daily motion if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The mean daily motion is located at a fixed position in the MPCORB record.</remarks>
	private static bool TryParseMeanDailyMotion(string line, out double value) => TryParseValue(line: line, startIndex: 80, length: 11, value: out value);

	/// <summary>Attempts to parse the perihelion distance from a raw MPCORB record.</summary>
	/// <param name="line">The raw MPCORB line.</param>
	/// <param name="value">When this method returns, contains the parsed perihelion distance if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	/// <remarks>The perihelion distance is calculated from the semi-major axis and eccentricity.</remarks>
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
	/// <remarks>The aphelion distance is calculated from the semi-major axis and eccentricity.</remarks>
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
	/// <remarks>The orbital period is calculated from the semi-major axis using Kepler's third law.</remarks>
	private static bool TryParseOrbitalPeriod(string line, out double value)
	{
		value = default;
		return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) && semiMajorAxis >= 0 && (value = Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3))) >= 0;
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the FormClosing event to cancel any running scatter plot operation.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the form-closing request.</param>
	/// <remarks>The running task is canceled so the form can close cleanly without leaving background work behind.</remarks>
	private void ScatterplotsForm_FormClosing(object? sender, FormClosingEventArgs e) => _cancellationTokenSource?.Cancel();

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the start/cancel button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click.</param>
	/// <remarks>When no task is running the handler starts scatter plot generation; otherwise it requests cancellation.</remarks>
	private async void ToolStripButtonStartCancel_Click(object? sender, EventArgs e)
	{
		// If a scatter plot generation task is currently running, request cancellation and return immediately.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			return;
		}
		// Validate that there is planetoid data available before attempting to generate a scatter plot.
		if (_planetoids.Count == 0)
		{
			_ = KryptonMessageBox.Show(text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Retrieve the selected X and Y axis definitions from the UI.
		ScatterplotDefinition? xDefinition = GetSelectedXDefinition();
		ScatterplotDefinition? yDefinition = GetSelectedYDefinition();
		// If either definition is not selected, display an informational message.
		if (xDefinition is null || yDefinition is null)
		{
			_ = KryptonMessageBox.Show(text: "Please select an orbital element for both X and Y axes.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		ResetDisplayedResults();
		ClearStatusBar(label: labelInformation);
		UpdateProgress(percent: 0);
		UpdateRunningState(isRunning: true);
		bool enableLiveDisplay = toolStripButtonLiveDisplay.Checked;
		_cancellationTokenSource = new CancellationTokenSource();
		try
		{
			Progress<int> progress = new(handler: UpdateProgress);
			Progress<ScatterplotResult> liveResults = new(handler: result =>
			{
				_currentXs = result.Xs;
				_currentYs = result.Ys;
				UpdateScatterplotPlot(xDefinition: xDefinition, yDefinition: yDefinition, xs: _currentXs, ys: _currentYs);
			});
			ScatterplotResult finalResult = await Task.Run(
				function: () => BuildScatterplot(
					xDefinition: xDefinition,
					yDefinition: yDefinition,
					enableLiveDisplay: enableLiveDisplay,
					progress: progress,
					liveResults: liveResults,
					cancellationToken: _cancellationTokenSource.Token),
				cancellationToken: _cancellationTokenSource.Token);
			// Apply the final results to the chart.
			_currentXs = finalResult.Xs;
			_currentYs = finalResult.Ys;
			UpdateScatterplotPlot(xDefinition: xDefinition, yDefinition: yDefinition, xs: _currentXs, ys: _currentYs);
			// Update the information label to summarize the results.
			labelInformation.Text = finalResult.Xs.Length == 0
				? "No planetoid values were available for the selected scatter plot."
				: $"Scatter plot created with {finalResult.Xs.Length:N0} data points.";
		}
		catch (OperationCanceledException)
		{
			logger.Info(message: "Orbit elements scatter plot generation was canceled by the user.");
			labelInformation.Text = "Scatter plot creation canceled.";
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"An error has occurred during scatter plot creation: {ex.Message}");
		}
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
	/// <remarks>The chart is redrawn to reflect the new axis selection.</remarks>
	private void ToolStripComboBoxXAxis_SelectedIndexChanged(object? sender, EventArgs e) =>
		UpdateScatterplotPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), xs: _currentXs, ys: _currentYs);

	/// <summary>Handles the SelectedIndexChanged event of the Y-axis drop-down.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>The chart is redrawn to reflect the new axis selection.</remarks>
	private void ToolStripComboBoxYAxis_SelectedIndexChanged(object? sender, EventArgs e) =>
		UpdateScatterplotPlot(xDefinition: GetSelectedXDefinition(), yDefinition: GetSelectedYDefinition(), xs: _currentXs, ys: _currentYs);

	/// <summary>Handles the CheckedChanged event of the live-display button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the check-state change.</param>
	/// <remarks>The button text mirrors the current state so users can immediately see whether live updates are enabled.</remarks>
	private void ToolStripButtonLiveDisplay_CheckedChanged(object? sender, EventArgs e) =>
		toolStripButtonLiveDisplay.Text = toolStripButtonLiveDisplay.Checked ? "On" : "Off";

	#endregion
}
