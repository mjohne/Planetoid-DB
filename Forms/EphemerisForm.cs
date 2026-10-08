/*
 * File:        EphemerisForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form for calculating and displaying ephemerides of a minor planet.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Services;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Represents a form for calculating and displaying ephemerides of a minor planet.</summary>
/// <remarks>
/// The user chooses a time range (UTC), a step size, the observer location and visibility criteria.
/// The calculation runs asynchronously via <see cref="EphemerisService"/> and can be canceled; the results are shown
/// in a list and an altitude chart and can be exported as CSV.
/// </remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class EphemerisForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The analytical planetary ephemeris used when no JPL file is loaded.</summary>
	private static readonly AnalyticalPlanetaryEphemeris analyticalEphemeris = new();

	/// <summary>The loaded JPL DE440/DE441 file, if any.</summary>
	private JplDevelopmentEphemeris? jplEphemeris;

	/// <summary>The orbital elements of the current minor planet, if valid.</summary>
	private MinorPlanetOrbitalElements? elements;

	/// <summary>The cancellation source of the running calculation, if any.</summary>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>The result of the last calculation.</summary>
	private IReadOnlyList<EphemerisEntry> lastResult = [];

	/// <summary>The observer location captured for the last calculation.</summary>
	private ObserverLocation? lastObserver;

	/// <summary>Indicates that the form should close after a running calculation has stopped.</summary>
	private bool closeAfterCalculation;

	/// <summary>The maximum number of result rows displayed in the list.</summary>
	private const int MaximumDisplayedResultPoints = 2000;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="EphemerisForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public EphemerisForm()
	{
		// Initialize the form components
		InitializeComponent();
		FormClosing += EphemerisForm_FormClosing;
	}

	#endregion

	#region helper methods

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Sets the minor planet from a raw MPCORB record.</summary>
	/// <param name="rawRecord">The raw MPCORB line of the minor planet.</param>
	/// <remarks>Invalid records are reported in the status bar and disable the calculation.</remarks>
	public void SetPlanetoidRecord(string? rawRecord)
	{
		if (MpcorbElementsParser.TryParse(rawLine: rawRecord, elements: out MinorPlanetOrbitalElements? parsed, error: out string? error))
		{
			elements = parsed;
			Text = $"Ephemerides: {parsed.Designation}";
		}
		else
		{
			elements = null;
			logger.Warn(message: $"Invalid MPCORB record for ephemerides: {error}");
			Text = "Ephemerides: invalid MPCORB record";
		}
	}

	/// <summary>Gets the planetary ephemeris currently in use.</summary>
	private IPlanetaryEphemerisProvider PlanetaryEphemeris => jplEphemeris ?? (IPlanetaryEphemerisProvider)analyticalEphemeris;

	/// <summary>Enables or disables the input controls while a calculation is running.</summary>
	/// <param name="running"><c>true</c> if a calculation is running; otherwise, <c>false</c>.</param>
	private void SetRunningState(bool running)
	{
		buttonCalculate.Enabled = !running && elements is not null;
		buttonLoadEphemerisFile.Enabled = !running;
		buttonCancel.Enabled = running;
		buttonExport.Enabled = !running && lastResult.Count > 0;
	}

	/// <summary>Sets the progress bar and the percent label.</summary>
	/// <param name="percent">The progress in percent.</param>
	private void SetProgress(int percent)
	{
		int value = Math.Clamp(value: percent, min: 0, max: 100);
		progressBar.Value = value;
		labelPercent.Text = string.Create(provider: CultureInfo.InvariantCulture, handler: $"{value} %");
	}

	/// <summary>Creates the columns of the result list.</summary>
	private void CreateColumns()
	{
		listView.Columns.Clear();
		string[] headers = ["Time (UTC)", "RA J2000 (h m s)", "Dec J2000 (° ′ ″)", "RA of date (h m s)", "Dec of date (° ′ ″)", "Azimuth (°)", "Altitude (°)", "Distance Δ (AU)", "Sun dist. r (AU)", "Mag. (mag)", "Phase (°)", "Elongation (°)", "Sun alt. (°)", "Moon dist. (°)", "Visible"];
		foreach (string header in headers)
		{
			_ = listView.Columns.Add(text: header, width: header.Length > 12 ? 110 : 90);
		}
	}

	/// <summary>Shows the result in the list view.</summary>
	/// <param name="entries">The ephemeris entries.</param>
	/// <remarks>Visible entries are highlighted.</remarks>
	private void ShowResult(IReadOnlyList<EphemerisEntry> entries)
	{
		CultureInfo ic = CultureInfo.InvariantCulture;
		listView.BeginUpdate();
		try
		{
			listView.Items.Clear();
			int displayedCount = Math.Min(val1: entries.Count, val2: MaximumDisplayedResultPoints);
			ListViewItem[] items = new ListViewItem[displayedCount];
			for (int i = 0; i < displayedCount; i++)
			{
				EphemerisEntry e = entries[i];
				items[i] = new ListViewItem(items:
				[
					e.Time.UtcDateTime.ToString(format: "yyyy-MM-dd HH:mm", provider: ic),
					CoordinateTransformationService.FormatRightAscension(hours: e.AstrometricRightAscensionHours),
					CoordinateTransformationService.FormatDeclination(degrees: e.AstrometricDeclinationDegrees),
					CoordinateTransformationService.FormatRightAscension(hours: e.RightAscensionHours),
					CoordinateTransformationService.FormatDeclination(degrees: e.DeclinationDegrees),
					e.AzimuthDegrees.ToString(format: "0.00", provider: ic),
					e.AltitudeDegrees.ToString(format: "0.00", provider: ic),
					e.DistanceAu.ToString(format: "0.000000", provider: ic),
					e.HeliocentricDistanceAu.ToString(format: "0.000000", provider: ic),
					double.IsFinite(d: e.ApparentMagnitude) ? e.ApparentMagnitude.ToString(format: "0.0", provider: ic) : "–",
					e.PhaseAngleDegrees.ToString(format: "0.0", provider: ic),
					e.ElongationDegrees.ToString(format: "0.0", provider: ic),
					e.SunAltitudeDegrees.ToString(format: "0.0", provider: ic),
					e.MoonSeparationDegrees.ToString(format: "0.0", provider: ic),
					e.IsVisible ? "yes" : "no"
				])
				{
					BackColor = e.IsVisible ? Color.Honeydew : listView.BackColor
				};
			}
			listView.Items.AddRange(items: items);
		}
		finally
		{
			listView.EndUpdate();
		}
		ShowChart(entries: entries);
	}

	/// <summary>Shows the altitude of the object over time in the chart.</summary>
	/// <param name="entries">The ephemeris entries.</param>
	private void ShowChart(IReadOnlyList<EphemerisEntry> entries)
	{
		formsPlot.Plot.Clear();
		if (entries.Count > 0)
		{
			double[] xs = [.. entries.Select(selector: static e => e.Time.UtcDateTime.ToOADate())];
			double[] ys = [.. entries.Select(selector: static e => e.AltitudeDegrees)];
			ScottPlot.Plottables.Scatter altitude = formsPlot.Plot.Add.Scatter(xs: xs, ys: ys);
			altitude.LegendText = "Altitude";
			altitude.MarkerSize = entries.Count > 500 ? 0 : 3;
			EphemerisEntry[] visible = [.. entries.Where(predicate: static e => e.IsVisible)];
			if (visible.Length > 0)
			{
				ScottPlot.Plottables.Scatter visiblePoints = formsPlot.Plot.Add.Scatter(
					xs: visible.Select(selector: static e => e.Time.UtcDateTime.ToOADate()).ToArray(),
					ys: visible.Select(selector: static e => e.AltitudeDegrees).ToArray());
				visiblePoints.LineWidth = 0;
				visiblePoints.MarkerSize = 6;
				visiblePoints.LegendText = "Visible";
			}
			_ = formsPlot.Plot.Add.HorizontalLine(y: 0.0);
			_ = formsPlot.Plot.Axes.DateTimeTicksBottom();
			formsPlot.Plot.Axes.Bottom.Label.Text = "Time (UTC)";
			formsPlot.Plot.Axes.Left.Label.Text = "Altitude (°)";
			formsPlot.Plot.ShowLegend();
			formsPlot.Plot.Axes.AutoScale();
		}
		formsPlot.Refresh();
	}

	/// <summary>Collects the calculation parameters from the controls.</summary>
	/// <returns>The time grid, observer, criteria and options.</returns>
	/// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
	private (IReadOnlyList<DateTimeOffset> Times, ObserverLocation Observer, VisibilityCriteria Criteria, EphemerisOptions Options) ReadInput()
	{
		// The date/time pickers show UTC wall-clock values
		DateTimeOffset begin = new(dateTime: DateTime.SpecifyKind(value: dateTimePickerEphemeridesBegin.Value, kind: DateTimeKind.Unspecified), offset: TimeSpan.Zero);
		DateTimeOffset end = new(dateTime: DateTime.SpecifyKind(value: dateTimePickerEphemeridesEnd.Value, kind: DateTimeKind.Unspecified), offset: TimeSpan.Zero);
		if (end < begin)
		{
			throw new ArgumentException(message: "The end of the ephemerides must not be before the begin.");
		}
		TimeSpan step = TimeSpan.FromDays(value: (double)numericUpDownStepsInDays.Value);
		if (step < TimeSpan.FromMinutes(minutes: 1))
		{
			throw new ArgumentException(message: "The step size must be at least one minute (0.0007 days).");
		}
		if (((end - begin).Ticks / step.Ticks) + 1 > EphemerisService.MaximumTimePoints)
		{
			throw new ArgumentException(message: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Too many time points; at most {EphemerisService.MaximumTimePoints} are allowed. Increase the step size or shorten the range."));
		}
		IReadOnlyList<DateTimeOffset> times = EphemerisService.CreateTimeGrid(start: begin, end: end, step: step);
		ObserverLocation observer = new(
			LatitudeDegrees: (double)numericUpDownLatitude.Value,
			LongitudeDegrees: (double)numericUpDownLongitude.Value,
			ElevationMeters: (double)numericUpDownElevation.Value);
		VisibilityCriteria criteria = new(
			MinimumAltitudeDegrees: (double)numericUpDownMinimumAltitude.Value,
			MaximumSunAltitudeDegrees: (double)numericUpDownMaximumSunAltitude.Value,
			FaintestMagnitude: checkBoxFaintestMagnitude.Checked ? (double)numericUpDownFaintestMagnitude.Value : null,
			MinimumMoonSeparationDegrees: (double)numericUpDownMinimumMoonSeparation.Value);
		EphemerisOptions options = new(IncludePlanetaryPerturbations: checkBoxPerturbations.Checked, ApplyRefraction: checkBoxRefraction.Checked);
		return (times, observer, criteria, options);
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method initializes the inputs with a 30-day range starting today (UTC).</remarks>
	private void EphemerisForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		formsPlot.AccessibleDescription = "Shows the altitude of the object over time (UTC)";
		DateTime today = DateTime.UtcNow.Date;
		dateTimePickerEphemeridesBegin.Value = today;
		dateTimePickerEphemeridesEnd.Value = today.AddDays(value: 30);
		CreateColumns();
		SetProgress(percent: 0);
		labelEphemerisSource.Text = $"Planetary ephemeris: {PlanetaryEphemeris.Name}";
		SetRunningState(running: false);
		if (elements is null)
		{
			SetStatusBar(label: labelInformation, text: "No valid MPCORB record available – the ephemerides cannot be calculated.");
		}
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the JPL file.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	private void EphemerisForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		if (cancellationTokenSource is not null)
		{
			e.Cancel = true;
			closeAfterCalculation = true;
			cancellationTokenSource.Cancel();
			return;
		}
		else
		{
			jplEphemeris?.Dispose();
			jplEphemeris = null;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Calculate button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Runs the calculation asynchronously; the UI stays responsive and the calculation can be canceled.</remarks>
	private async void ButtonCalculate_Click(object sender, EventArgs e)
	{
		if (elements is null || cancellationTokenSource is not null)
		{
			return;
		}
		IReadOnlyList<DateTimeOffset> times;
		ObserverLocation observer;
		VisibilityCriteria criteria;
		EphemerisOptions options;
		try
		{
			(times, observer, criteria, options) = ReadInput();
		}
		catch (ArgumentException ex)
		{
			_ = KryptonMessageBox.Show(text: ex.Message, caption: "Invalid input", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		using CancellationTokenSource cts = new();
		cancellationTokenSource = cts;
		lastResult = [];
		lastObserver = null;
		SetRunningState(running: true);
		SetProgress(percent: 0);
		SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Calculating {times.Count} positions..."));
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			EphemerisService service = new(planetaryEphemeris: PlanetaryEphemeris);
			Progress<int> progress = new(handler: SetProgress);
			lastResult = await service.CalculateAsync(elements: elements, times: times, observer: observer, criteria: criteria, options: options, progress: progress, cancellationToken: cts.Token);
			lastObserver = observer;
			if (lastResult.Count > 0 && logger.IsDebugEnabled)
			{
				EphemerisEntry first = lastResult[0];
				logger.Debug(message: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Ephemeris {elements.Designation} at {first.Time.UtcDateTime:O} (JD UTC {TimeScales.ToJulianDateUtc(time: first.Time):0.000000}, {PlanetaryEphemeris.Name}): RA/Dec J2000 {first.AstrometricRightAscensionHours:0.000000} h / {first.AstrometricDeclinationDegrees:0.00000}°, RA/Dec of date {first.RightAscensionHours:0.000000} h / {first.DeclinationDegrees:0.00000}°, Δ {first.DistanceAu:0.000000} AU, r {first.HeliocentricDistanceAu:0.000000} AU"));
			}
			ShowResult(entries: lastResult);
			int visibleCount = lastResult.Count(predicate: static entry => entry.IsVisible);
			string displayNote = lastResult.Count > MaximumDisplayedResultPoints ? $" The first {MaximumDisplayedResultPoints} are shown in the list." : string.Empty;
			SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.InvariantCulture, handler: $"{lastResult.Count} positions calculated in {stopwatch.Elapsed.TotalSeconds:0.0} s; visible: {visibleCount}.{displayNote}"));
		}
		catch (OperationCanceledException)
		{
			SetStatusBar(label: labelInformation, text: "Calculation canceled.");
		}
		catch (ArgumentException ex)
		{
			logger.Warn(exception: ex, message: "Ephemeris calculation failed.");
			SetStatusBar(label: labelInformation, text: ex.Message);
			_ = KryptonMessageBox.Show(text: ex.Message, caption: "Ephemerides", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
		}
		catch (Exception ex) when (ex is IOException or InvalidDataException or InvalidOperationException)
		{
			logger.Error(exception: ex, message: "Ephemeris calculation failed.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"The ephemerides could not be calculated: {ex.Message}");
		}
		finally
		{
			cancellationTokenSource = null;
			if (closeAfterCalculation)
			{
				closeAfterCalculation = false;
				Close();
			}
			else if (!IsDisposed && !Disposing)
			{
				SetRunningState(running: false);
			}
			else
			{
				jplEphemeris?.Dispose();
				jplEphemeris = null;
			}
		}
	}

	/// <summary>Handles the Click event of the Cancel button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void ButtonCancel_Click(object sender, EventArgs e) => cancellationTokenSource?.Cancel();

	/// <summary>Handles the Click event of the Export button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Exports the last result as CSV with invariant culture and UTC timestamps.</remarks>
	private async void ButtonExport_Click(object sender, EventArgs e)
	{
		if (lastResult.Count == 0 || lastObserver is null)
		{
			return;
		}
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
			DefaultExt = "csv",
			FileName = $"Ephemerides_{elements?.Designation.Trim().Replace(oldChar: ' ', newChar: '_') ?? "object"}.csv"
		};
		if (saveFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		try
		{
			await EphemerisExportService.ExportCsvAsync(filePath: saveFileDialog.FileName, entries: lastResult, designation: elements?.Designation, observer: lastObserver);
			SetStatusBar(label: labelInformation, text: $"Exported to {saveFileDialog.FileName}");
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			logger.Error(exception: ex, message: "Ephemeris export failed.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"Error saving as CSV: {ex.Message}");
		}
	}

	/// <summary>Handles the Click event of the Load DE440/441 button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Loads a JPL DE binary file (e.g. <c>linux_p1550p2650.440</c> or <c>linux_m13000p17000.441</c>) for highest accuracy.</remarks>
	private void ButtonLoadEphemerisFile_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new()
		{
			Filter = "JPL DE binary files (*.440;*.441)|*.440;*.441|All files (*.*)|*.*",
			Title = "Load JPL DE440/DE441 binary ephemeris"
		};
		if (openFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		try
		{
			JplDevelopmentEphemeris loaded = new(filePath: openFileDialog.FileName);
			jplEphemeris?.Dispose();
			jplEphemeris = loaded;
			labelEphemerisSource.Text = $"Planetary ephemeris: {loaded.Name}";
			SetStatusBar(label: labelInformation, text: $"Loaded {loaded.Name}");
		}
		catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or NotSupportedException)
		{
			logger.Error(exception: ex, message: "Loading the JPL ephemeris failed.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"The file could not be loaded as JPL DE ephemeris: {ex.Message}");
		}
	}

	#endregion
}
