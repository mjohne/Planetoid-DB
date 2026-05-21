using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using ScottPlot;
using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a histogram of counted planetoids for a selected orbital element or derived property.</summary>
/// <remarks>The form groups planetoids into selectable ranges, renders the distribution as a ScottPlot bar chart, and mirrors the counted bins in a tabular ListView. Users can optionally request live updates while the background counting operation is running.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitElementsGroupingForm : BaseKryptonForm
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

/// <summary>Initializes a new instance of the <see cref="OrbitElementsGroupingForm"/> class.</summary>
/// <param name="planetoids">The planetoid string records to process from the database.</param>
/// <remarks>The main form passes the necessary raw MPCORB data to this dialog so the histogram can be generated without directly accessing shared UI state.</remarks>
public OrbitElementsGroupingForm(IReadOnlyList<string> planetoids)
{
InitializeComponent();
_planetoids = planetoids;
InitializeSelections();
UpdateRunningState(isRunning: false);
ResetDisplayedResults();
logger.Info(message: "OrbitElementsGroupingForm initialized with {0} planetoids.", argument: _planetoids.Count);
}

#endregion

#region Helper methods

/// <summary>Returns a short debugger display string for this instance.</summary>
/// <returns>A string representation of the current instance for use in the debugger.</returns>
private string GetDebuggerDisplay() => ToString();

/// <summary>Creates all histogram definitions supported by the form.</summary>
/// <returns>A list of selectable histogram definitions.</returns>
/// <remarks>The selectable items include directly stored orbital elements and a few useful derived properties computed from semi-major axis and eccentricity.</remarks>
private static List<HistogramDefinition> CreateHistogramDefinitions() =>
[
new HistogramDefinition(
DisplayName: "Semi-major axis",
AxisLabel: "Semi-major axis (AU)",
UnitSuffix: " AU",
StepOptions:
[
new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
new StepOption(Value: 1.0, DisplayText: "1 AU"),
new StepOption(Value: 2.0, DisplayText: "2 AU")
],
ValueSelector: static line => TryParseSemiMajorAxis(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Eccentricity",
AxisLabel: "Eccentricity",
UnitSuffix: string.Empty,
StepOptions:
[
new StepOption(Value: 0.01, DisplayText: "0.01"),
new StepOption(Value: 0.05, DisplayText: "0.05"),
new StepOption(Value: 0.1, DisplayText: "0.1"),
new StepOption(Value: 0.2, DisplayText: "0.2")
],
ValueSelector: static line => TryParseEccentricity(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Inclination",
AxisLabel: "Inclination (°)",
UnitSuffix: "°",
StepOptions:
[
new StepOption(Value: 1.0, DisplayText: "1°"),
new StepOption(Value: 2.0, DisplayText: "2°"),
new StepOption(Value: 5.0, DisplayText: "5°"),
new StepOption(Value: 10.0, DisplayText: "10°")
],
ValueSelector: static line => TryParseInclination(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Mean anomaly",
AxisLabel: "Mean anomaly (°)",
UnitSuffix: "°",
StepOptions:
[
new StepOption(Value: 5.0, DisplayText: "5°"),
new StepOption(Value: 10.0, DisplayText: "10°"),
new StepOption(Value: 15.0, DisplayText: "15°"),
new StepOption(Value: 30.0, DisplayText: "30°")
],
ValueSelector: static line => TryParseMeanAnomaly(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Argument of perihelion",
AxisLabel: "Argument of perihelion (°)",
UnitSuffix: "°",
StepOptions:
[
new StepOption(Value: 5.0, DisplayText: "5°"),
new StepOption(Value: 10.0, DisplayText: "10°"),
new StepOption(Value: 15.0, DisplayText: "15°"),
new StepOption(Value: 30.0, DisplayText: "30°")
],
ValueSelector: static line => TryParseArgumentOfPerihelion(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Longitude of ascending node",
AxisLabel: "Longitude of ascending node (°)",
UnitSuffix: "°",
StepOptions:
[
new StepOption(Value: 5.0, DisplayText: "5°"),
new StepOption(Value: 10.0, DisplayText: "10°"),
new StepOption(Value: 15.0, DisplayText: "15°"),
new StepOption(Value: 30.0, DisplayText: "30°")
],
ValueSelector: static line => TryParseLongitudeOfAscendingNode(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Mean daily motion",
AxisLabel: "Mean daily motion (°/day)",
UnitSuffix: " °/day",
StepOptions:
[
new StepOption(Value: 0.05, DisplayText: "0.05 °/day"),
new StepOption(Value: 0.1, DisplayText: "0.1 °/day"),
new StepOption(Value: 0.25, DisplayText: "0.25 °/day"),
new StepOption(Value: 0.5, DisplayText: "0.5 °/day")
],
ValueSelector: static line => TryParseMeanDailyMotion(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Perihelion distance",
AxisLabel: "Perihelion distance (AU)",
UnitSuffix: " AU",
StepOptions:
[
new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
new StepOption(Value: 1.0, DisplayText: "1 AU")
],
ValueSelector: static line => TryParsePerihelionDistance(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Aphelion distance",
AxisLabel: "Aphelion distance (AU)",
UnitSuffix: " AU",
StepOptions:
[
new StepOption(Value: 0.1, DisplayText: "0.1 AU"),
new StepOption(Value: 0.25, DisplayText: "0.25 AU"),
new StepOption(Value: 0.5, DisplayText: "0.5 AU"),
new StepOption(Value: 1.0, DisplayText: "1 AU"),
new StepOption(Value: 2.0, DisplayText: "2 AU")
],
ValueSelector: static line => TryParseAphelionDistance(line: line, value: out double value) ? value : null),
new HistogramDefinition(
DisplayName: "Orbital period",
AxisLabel: "Orbital period (years)",
UnitSuffix: " years",
StepOptions:
[
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
toolStripComboBoxOrbitElement.Items.Clear();
foreach (HistogramDefinition definition in CreateHistogramDefinitions())
{
_ = toolStripComboBoxOrbitElement.Items.Add(value: definition);
}
if (toolStripComboBoxOrbitElement.Items.Count > 0)
{
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
toolStripButtonStartCancel.Text = isRunning ? "&Cancel" : "&Start";
toolStripButtonStartCancel.Image = isRunning
? Planetoid_DB.Resources.FatcowIcons16px.fatcow_cancel_16px
: Planetoid_DB.Resources.FatcowIcons16px.fatcow_page_white_text_16px;
toolStripComboBoxOrbitElement.Enabled = !isRunning;
toolStripComboBoxStepSize.Enabled = !isRunning;
toolStripButtonLiveDisplay.Enabled = !isRunning;
}

/// <summary>Resets the displayed ListView and chart results.</summary>
/// <remarks>The method clears previously counted bins and redraws the empty chart using the currently selected histogram definition.</remarks>
private void ResetDisplayedResults()
{
_currentResults = [];
listViewResults.BeginUpdate();
listViewResults.Items.Clear();
listViewResults.EndUpdate();
UpdateHistogramPlot(definition: GetSelectedDefinition(), results: _currentResults);
}

/// <summary>Updates the progress bar value and taskbar progress indicator.</summary>
/// <param name="percent">The progress percentage to display.</param>
/// <remarks>The input value is clamped to the 0 to 100 range before it is shown.</remarks>
private void UpdateProgress(int percent)
{
int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
kryptonProgressBar.Value = clampedPercent;
kryptonProgressBar.Text = $"{clampedPercent}%";
if (IsHandleCreated)
{
TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
}
}

/// <summary>Applies counted histogram results to both the ListView and the chart.</summary>
/// <param name="definition">The histogram definition that produced the results.</param>
/// <param name="results">The histogram bins to display.</param>
/// <remarks>The method keeps the chart and the tabular view synchronized.</remarks>
private void ApplyResults(HistogramDefinition? definition, IReadOnlyList<HistogramBinResult> results)
{
_currentResults = [.. results];
listViewResults.BeginUpdate();
listViewResults.Items.Clear();
foreach (HistogramBinResult result in results)
{
ListViewItem item = new(text: FormatRangeLabel(start: result.Start, end: result.End, unitSuffix: definition?.UnitSuffix ?? string.Empty))
{
ToolTipText = $"{result.Count:N0} planetoids"
};
_ = item.SubItems.Add(text: FormatNumericValue(value: result.Start));
_ = item.SubItems.Add(text: FormatNumericValue(value: result.End));
_ = item.SubItems.Add(text: result.Count.ToString(format: "N0", provider: CultureInfo.InvariantCulture));
listViewResults.Items.Add(item: item);
}
listViewResults.EndUpdate();
UpdateHistogramPlot(definition: definition, results: results);
}

/// <summary>Redraws the ScottPlot histogram based on the supplied results.</summary>
/// <param name="definition">The histogram definition that produced the results.</param>
/// <param name="results">The histogram bins to render.</param>
/// <remarks>The chart always contains a title, axis labels, and a legend. When no results are available, the axes are still configured but no bars are plotted.</remarks>
private void UpdateHistogramPlot(HistogramDefinition? definition, IReadOnlyList<HistogramBinResult> results)
{
formsPlotHistogram.Plot.Clear();
formsPlotHistogram.Plot.Title(definition is null ? "Orbit elements histogram" : $"Histogram of {definition.DisplayName}");
formsPlotHistogram.Plot.Axes.Bottom.Label.Text = definition?.AxisLabel ?? "Selected element";
formsPlotHistogram.Plot.Axes.Left.Label.Text = "Number of planetoids";
formsPlotHistogram.Plot.Legend.IsVisible = true;
formsPlotHistogram.Plot.Legend.Alignment = Alignment.UpperRight;

if (definition is not null && results.Count > 0)
{
double[] values = [.. results.Select(selector: static result => (double)result.Count)];
string[] labels = [.. results.Select(selector: result => FormatRangeLabel(start: result.Start, end: result.End, unitSuffix: definition.UnitSuffix))];
double[] positions = [.. Enumerable.Range(start: 0, count: results.Count).Select(selector: static index => (double)index)];
var barPlot = formsPlotHistogram.Plot.Add.Bars(positions, values);
barPlot.LegendText = "Planetoids";
barPlot.Color = Colors.SteelBlue;
formsPlotHistogram.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(positions, labels);
}

formsPlotHistogram.Plot.Axes.AutoScale();
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
SortedDictionary<int, int> counts = [];
int total = _planetoids.Count;
int progressInterval = Math.Max(val1: 1, val2: total / 100);
int liveInterval = Math.Max(val1: 1, val2: total / 25);
for (int i = 0; i < total; i++)
{
cancellationToken.ThrowIfCancellationRequested();
double? value = definition.ValueSelector(_planetoids[i]);
if (value.HasValue && double.IsFinite(value.Value))
{
int binIndex = (int)Math.Floor(d: value.Value / stepSize);
counts.TryGetValue(key: binIndex, value: out int currentCount);
counts[binIndex] = currentCount + 1;
}
int processed = i + 1;
if (processed % progressInterval == 0 || processed == total)
{
progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
}
if (enableLiveDisplay && (processed % liveInterval == 0 || processed == total))
{
liveResults.Report(value: CreateHistogramResults(counts: counts, stepSize: stepSize));
}
}
return CreateHistogramResults(counts: counts, stepSize: stepSize);
}

/// <summary>Converts raw bin counts into sorted histogram rows.</summary>
/// <param name="counts">The counted planetoid totals per bin index.</param>
/// <param name="stepSize">The width of each histogram bin.</param>
/// <returns>A sorted list of histogram rows.</returns>
private static List<HistogramBinResult> CreateHistogramResults(SortedDictionary<int, int> counts, double stepSize) =>
[
.. counts.Select(selector: static pair => pair).Select(selector: pair =>
new HistogramBinResult(
Start: pair.Key * stepSize,
End: (pair.Key + 1) * stepSize,
Count: pair.Value))
];

/// <summary>Formats a chart and ListView label for one histogram range.</summary>
/// <param name="start">The inclusive lower boundary.</param>
/// <param name="end">The exclusive upper boundary.</param>
/// <param name="unitSuffix">The unit suffix to append to numeric values.</param>
/// <returns>A formatted range label.</returns>
private static string FormatRangeLabel(double start, double end, string unitSuffix) =>
$"{FormatNumericValue(value: start)}{unitSuffix} .. {FormatNumericValue(value: end)}{unitSuffix}";

/// <summary>Formats a numeric value for display in the chart and ListView.</summary>
/// <param name="value">The value to format.</param>
/// <returns>The formatted text.</returns>
private static string FormatNumericValue(double value) => value.ToString(format: "0.####", provider: CultureInfo.InvariantCulture);

/// <summary>Attempts to parse a floating-point slice from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="startIndex">The inclusive start index of the numeric field.</param>
/// <param name="length">The field length.</param>
/// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseValue(string line, int startIndex, int length, out double value)
{
value = default;
return line.Length >= startIndex + length &&
double.TryParse(
s: line.Substring(startIndex: startIndex, length: length).Trim(),
style: NumberStyles.Float,
provider: CultureInfo.InvariantCulture,
result: out value);
}

/// <summary>Attempts to parse the semi-major axis from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed semi-major axis if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseSemiMajorAxis(string line, out double value) => TryParseValue(line: line, startIndex: 92, length: 11, value: out value);

/// <summary>Attempts to parse the orbital eccentricity from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed eccentricity if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseEccentricity(string line, out double value) => TryParseValue(line: line, startIndex: 70, length: 9, value: out value);

/// <summary>Attempts to parse the inclination from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed inclination if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseInclination(string line, out double value) => TryParseValue(line: line, startIndex: 59, length: 9, value: out value);

/// <summary>Attempts to parse the mean anomaly from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed mean anomaly if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseMeanAnomaly(string line, out double value) => TryParseValue(line: line, startIndex: 26, length: 9, value: out value);

/// <summary>Attempts to parse the argument of perihelion from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed argument of perihelion if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseArgumentOfPerihelion(string line, out double value) => TryParseValue(line: line, startIndex: 37, length: 9, value: out value);

/// <summary>Attempts to parse the longitude of the ascending node from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed longitude of the ascending node if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseLongitudeOfAscendingNode(string line, out double value) => TryParseValue(line: line, startIndex: 48, length: 9, value: out value);

/// <summary>Attempts to parse the mean daily motion from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed mean daily motion if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseMeanDailyMotion(string line, out double value) => TryParseValue(line: line, startIndex: 80, length: 11, value: out value);

/// <summary>Attempts to parse the perihelion distance from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed perihelion distance if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParsePerihelionDistance(string line, out double value)
{
value = default;
return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) &&
TryParseEccentricity(line: line, value: out double eccentricity)
? (value = semiMajorAxis * (1 - eccentricity)) >= 0
: false;
}

/// <summary>Attempts to parse the aphelion distance from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed aphelion distance if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseAphelionDistance(string line, out double value)
{
value = default;
return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) &&
TryParseEccentricity(line: line, value: out double eccentricity)
? (value = semiMajorAxis * (1 + eccentricity)) >= 0
: false;
}

/// <summary>Attempts to parse the orbital period from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed orbital period in years if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseOrbitalPeriod(string line, out double value)
{
value = default;
return TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis) && semiMajorAxis >= 0
? (value = Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3))) >= 0
: false;
}

#endregion

#region Form event handlers

/// <summary>Handles the FormClosing event to cancel any running histogram operation.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the form-closing request.</param>
/// <remarks>The running task is canceled so the form can close cleanly without leaving background work behind.</remarks>
private void OrbitElementsGroupingForm_FormClosing(object? sender, FormClosingEventArgs e)
{
if (_cancellationTokenSource != null)
{
_cancellationTokenSource.Cancel();
}
}

#endregion

#region Click event handlers

/// <summary>Handles the Click event of the start/cancel button.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the click.</param>
/// <remarks>When no task is running the handler starts histogram generation; otherwise it requests cancellation.</remarks>
private async void ToolStripButtonStartCancel_Click(object? sender, EventArgs e)
{
if (_cancellationTokenSource != null)
{
_cancellationTokenSource.Cancel();
return;
}
if (_planetoids.Count == 0)
{
_ = KryptonMessageBox.Show(text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
return;
}
HistogramDefinition? definition = GetSelectedDefinition();
StepOption? step = GetSelectedStep();
if (definition is null || step is null)
{
_ = KryptonMessageBox.Show(text: "Please select an orbital element and a step size.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
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
Progress<List<HistogramBinResult>> liveResults = new(handler: results => ApplyResults(definition: definition, results: results));
List<HistogramBinResult> finalResults = await Task.Run(
function: () => BuildHistogram(
definition: definition,
stepSize: step.Value,
enableLiveDisplay: enableLiveDisplay,
progress: progress,
liveResults: liveResults,
cancellationToken: _cancellationTokenSource.Token),
cancellationToken: _cancellationTokenSource.Token);
ApplyResults(definition: definition, results: finalResults);
labelInformation.Text = finalResults.Count == 0
? "No planetoid values were available for the selected histogram."
: $"Histogram created with {finalResults.Count:N0} ranges and {finalResults.Sum(selector: static result => result.Count):N0} counted planetoids.";
}
catch (OperationCanceledException)
{
logger.Info(message: "Orbit elements histogram generation was canceled by the user.");
labelInformation.Text = "Histogram creation canceled.";
}
catch (Exception ex)
{
logger.Error(exception: ex, message: ex.Message);
ShowErrorMessage(message: $"An error has occurred during histogram creation: {ex.Message}");
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

/// <summary>Handles the SelectedIndexChanged event of the orbital-element drop-down.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the selection change.</param>
/// <remarks>The step-size drop-down is repopulated with values that are meaningful for the selected histogram definition.</remarks>
private void ToolStripComboBoxOrbitElement_SelectedIndexChanged(object? sender, EventArgs e)
{
HistogramDefinition? definition = GetSelectedDefinition();
toolStripComboBoxStepSize.Items.Clear();
if (definition is null)
{
ResetDisplayedResults();
return;
}
foreach (StepOption stepOption in definition.StepOptions)
{
_ = toolStripComboBoxStepSize.Items.Add(value: stepOption);
}
if (toolStripComboBoxStepSize.Items.Count > 0)
{
toolStripComboBoxStepSize.SelectedIndex = 0;
}
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
	toolStripButtonLiveDisplay.Text = toolStripButtonLiveDisplay.Checked ? "On" : "Off";

#endregion
}
