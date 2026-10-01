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
using Planetoid_DB.Services;

using ScottPlot.WinForms;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Represents a form for calculating and displaying ephemerides of a minor planet.</summary>
/// <remarks>
/// The user selects a time range (in any time zone; all calculations are done in UTC), the step width, the observing site
/// and visibility criteria. The calculation runs asynchronously and can be cancelled. The results can be exported as CSV and shown as a chart.
/// </remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class EphemerisForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The step units offered to the user.</summary>
	private static readonly (string Name, TimeSpan Unit)[] StepUnits = [("minutes", TimeSpan.FromMinutes(minutes: 1)), ("hours", TimeSpan.FromHours(hours: 1)), ("days", TimeSpan.FromDays(days: 1))];

	/// <summary>The raw MPCORB record of the minor planet.</summary>
	private readonly string? mpcorbRecord;

	/// <summary>The time zone combo box.</summary>
	private readonly KryptonComboBox comboBoxTimeZone = new() { DropDownStyle = ComboBoxStyle.DropDownList };

	/// <summary>The step unit combo box.</summary>
	private readonly KryptonComboBox comboBoxStepUnit = new() { DropDownStyle = ComboBoxStyle.DropDownList };

	/// <summary>The latitude input in degrees.</summary>
	private readonly KryptonNumericUpDown numericLatitude = CreateNumeric(minimum: -90m, maximum: 90m, value: 52.52m, decimals: 5);

	/// <summary>The longitude input in degrees (east positive).</summary>
	private readonly KryptonNumericUpDown numericLongitude = CreateNumeric(minimum: -180m, maximum: 180m, value: 13.405m, decimals: 5);

	/// <summary>The height input in metres.</summary>
	private readonly KryptonNumericUpDown numericHeight = CreateNumeric(minimum: -500m, maximum: 9000m, value: 34m, decimals: 0);

	/// <summary>The minimum object altitude in degrees.</summary>
	private readonly KryptonNumericUpDown numericMinimumAltitude = CreateNumeric(minimum: -10m, maximum: 90m, value: 15m, decimals: 1);

	/// <summary>The maximum Sun altitude in degrees.</summary>
	private readonly KryptonNumericUpDown numericMaximumSunAltitude = CreateNumeric(minimum: -18m, maximum: 90m, value: -12m, decimals: 1);

	/// <summary>The limiting magnitude in mag.</summary>
	private readonly KryptonNumericUpDown numericLimitingMagnitude = CreateNumeric(minimum: -30m, maximum: 99m, value: 16m, decimals: 1);

	/// <summary>The minimum Moon separation in degrees.</summary>
	private readonly KryptonNumericUpDown numericMinimumMoonSeparation = CreateNumeric(minimum: 0m, maximum: 180m, value: 10m, decimals: 1);

	/// <summary>The propagation model combo box.</summary>
	private readonly KryptonComboBox comboBoxModel = new() { DropDownStyle = ComboBoxStyle.DropDownList };

	/// <summary>The refraction check box.</summary>
	private readonly KryptonCheckBox checkBoxRefraction = new() { Checked = true };

	/// <summary>The button to load a JPL ephemeris file.</summary>
	private readonly KryptonButton buttonLoadJplEphemeris = new();

	/// <summary>The label showing the planetary ephemeris in use.</summary>
	private readonly KryptonLabel labelEphemerisSource = new();

	/// <summary>The cancel button.</summary>
	private readonly KryptonButton buttonCancel = new() { Enabled = false };

	/// <summary>The CSV export button.</summary>
	private readonly KryptonButton buttonExportCsv = new() { Enabled = false };

	/// <summary>The chart button.</summary>
	private readonly KryptonButton buttonChart = new() { Enabled = false };

	/// <summary>The calculated entries.</summary>
	private IReadOnlyList<EphemerisEntry> entries = [];

	/// <summary>The cancellation source of the running calculation.</summary>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>The optionally loaded JPL SPK ephemeris (DE440/DE441).</summary>
	private JplSpkEphemeris? jplEphemeris;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="EphemerisForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public EphemerisForm() : this(mpcorbRecord: null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="EphemerisForm"/> class for a minor planet.</summary>
	/// <param name="mpcorbRecord">The raw MPCORB record of the minor planet.</param>
	public EphemerisForm(string? mpcorbRecord)
	{
		// Initialize the form components
		InitializeComponent();
		this.mpcorbRecord = mpcorbRecord;
		BuildAdditionalControls();
	}

	#endregion

	#region helper methods

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Creates a numeric input.</summary>
	/// <param name="minimum">The minimum value.</param>
	/// <param name="maximum">The maximum value.</param>
	/// <param name="value">The initial value.</param>
	/// <param name="decimals">The number of decimal places.</param>
	/// <returns>The numeric input.</returns>
	private static KryptonNumericUpDown CreateNumeric(decimal minimum, decimal maximum, decimal value, int decimals) => new()
	{
		Minimum = minimum,
		Maximum = maximum,
		Value = value,
		DecimalPlaces = decimals,
		Increment = decimals == 0 ? 1m : 0.1m,
		Size = new Size(width: 92, height: 22)
	};

	/// <summary>Adds a labelled input to the main panel.</summary>
	/// <param name="text">The label text including units.</param>
	/// <param name="control">The input control.</param>
	/// <param name="x">The x position of the label.</param>
	/// <param name="y">The y position.</param>
	/// <param name="description">The accessible description, also shown in the status bar.</param>
	private void AddLabelled(string text, Control control, int x, int y, string description)
	{
		KryptonLabel label = new() { Location = new Point(x: x, y: y + 2), AutoSize = true };
		label.Values.Text = text;
		control.Location = new Point(x: x + 145, y: y);
		control.AccessibleName = text.Replace(oldValue: "&", newValue: string.Empty, comparisonType: StringComparison.Ordinal);
		control.AccessibleDescription = description;
		control.Enter += Control_Enter;
		control.Leave += Control_Leave;
		control.MouseEnter += Control_Enter;
		control.MouseLeave += Control_Leave;
		kryptonPanelMain.Controls.Add(value: label);
		kryptonPanelMain.Controls.Add(value: control);
	}

	/// <summary>Configures a button created in code.</summary>
	/// <param name="button">The button.</param>
	/// <param name="text">The button text.</param>
	/// <param name="location">The location.</param>
	/// <param name="description">The accessible description.</param>
	/// <param name="click">The click handler.</param>
	private void ConfigureButton(KryptonButton button, string text, Point location, string description, EventHandler click)
	{
		button.Values.Text = text;
		button.Location = location;
		button.Size = new Size(width: 105, height: 29);
		button.AccessibleName = text.Replace(oldValue: "&", newValue: string.Empty, comparisonType: StringComparison.Ordinal);
		button.AccessibleDescription = description;
		button.Click += click;
		button.Enter += Control_Enter;
		button.Leave += Control_Leave;
		button.MouseEnter += Control_Enter;
		button.MouseLeave += Control_Leave;
		kryptonPanelMain.Controls.Add(value: button);
	}

	/// <summary>Creates the additional input controls and arranges the layout.</summary>
	private void BuildAdditionalControls()
	{
		SuspendLayout();
		ClientSize = new Size(width: 980, height: 640);
		MinimumSize = new Size(width: 900, height: 520);

		// Time range (column 1)
		foreach (KryptonDateTimePicker picker in new[] { dateTimePickerEphemeridesBegin, dateTimePickerEphemeridesEnd })
		{
			picker.Format = DateTimePickerFormat.Custom;
			picker.CustomFormat = "yyyy-MM-dd HH:mm";
		}
		DateTime today = DateTime.Today;
		dateTimePickerEphemeridesBegin.Value = today.AddHours(value: 18);
		dateTimePickerEphemeridesEnd.Value = today.AddDays(value: 1).AddHours(value: 6);
		labelEphemeridesStepsInDays.Values.Text = "&Step width:";
		numericUpDownStepsInDays.DecimalPlaces = 2;
		numericUpDownStepsInDays.Maximum = 10000m;
		numericUpDownStepsInDays.Minimum = 0.01m;
		numericUpDownStepsInDays.Value = 30m;
		numericUpDownStepsInDays.Size = new Size(width: 92, height: 22);
		comboBoxStepUnit.Items.AddRange(items: [.. StepUnits.Select(selector: static u => (object)u.Name)]);
		comboBoxStepUnit.SelectedIndex = 0;
		comboBoxStepUnit.Location = new Point(x: 255, y: 76);
		comboBoxStepUnit.Size = new Size(width: 77, height: 22);
		comboBoxStepUnit.AccessibleName = "Step unit";
		comboBoxStepUnit.AccessibleDescription = "Selects the unit of the step width";
		kryptonPanelMain.Controls.Add(value: comboBoxStepUnit);
		foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
		{
			_ = comboBoxTimeZone.Items.Add(item: zone);
		}
		comboBoxTimeZone.DisplayMember = nameof(TimeZoneInfo.DisplayName);
		comboBoxTimeZone.SelectedItem = comboBoxTimeZone.Items.Cast<TimeZoneInfo>().FirstOrDefault(predicate: static z => z.Id == TimeZoneInfo.Local.Id);
		comboBoxTimeZone.Size = new Size(width: 173, height: 22);
		comboBoxTimeZone.DropDownWidth = 400;
		AddLabelled(text: "&Time zone of input:", control: comboBoxTimeZone, x: 14, y: 108, description: "Time zone of the begin and end time; all calculations are done in UTC");

		// Observer (column 2)
		AddLabelled(text: "&Latitude (°, N+):", control: numericLatitude, x: 350, y: 14, description: "Geodetic latitude of the observing site in degrees (north positive, WGS84)");
		AddLabelled(text: "L&ongitude (°, E+):", control: numericLongitude, x: 350, y: 45, description: "Geodetic longitude of the observing site in degrees (east positive, WGS84)");
		AddLabelled(text: "&Height (m):", control: numericHeight, x: 350, y: 76, description: "Height of the observing site above the WGS84 ellipsoid in metres");
		comboBoxModel.Items.AddRange(items: ["Perturbed (planets, Moon, GR)", "Two-body (Kepler)"]);
		comboBoxModel.SelectedIndex = 0;
		comboBoxModel.Size = new Size(width: 190, height: 22);
		AddLabelled(text: "Orbit &model:", control: comboBoxModel, x: 350, y: 108, description: "Orbit propagation model");

		// Visibility criteria (column 3)
		AddLabelled(text: "Min. object altitude (°):", control: numericMinimumAltitude, x: 700, y: 14, description: "Minimum altitude of the minor planet above the horizon in degrees");
		AddLabelled(text: "Max. Sun altitude (°):", control: numericMaximumSunAltitude, x: 700, y: 45, description: "Maximum altitude of the Sun in degrees (−12° = nautical twilight, −18° = astronomical twilight)");
		AddLabelled(text: "Limiting magnitude (mag):", control: numericLimitingMagnitude, x: 700, y: 76, description: "Faintest observable apparent magnitude V (99 = no limit)");
		AddLabelled(text: "Min. Moon distance (°):", control: numericMinimumMoonSeparation, x: 700, y: 108, description: "Minimum angular distance between the minor planet and the Moon in degrees");
		checkBoxRefraction.Values.Text = "Apply atmospheric &refraction";
		checkBoxRefraction.Location = new Point(x: 700, y: 140);
		checkBoxRefraction.AccessibleName = "Apply atmospheric refraction";
		checkBoxRefraction.AccessibleDescription = "Applies standard atmospheric refraction to the altitudes";
		kryptonPanelMain.Controls.Add(value: checkBoxRefraction);

		// Planetary ephemeris
		ConfigureButton(button: buttonLoadJplEphemeris, text: "&JPL DE file...", location: new Point(x: 350, y: 138), description: "Loads a JPL DE440/DE441 SPK file (.bsp) for the planetary positions", click: ButtonLoadJplEphemeris_Click);
		labelEphemerisSource.Location = new Point(x: 460, y: 143);
		labelEphemerisSource.AutoSize = true;
		labelEphemerisSource.Values.Text = new AnalyticalPlanetaryEphemeris().Name;
		kryptonPanelMain.Controls.Add(value: labelEphemerisSource);

		// Actions and progress
		buttonCalculate.Location = new Point(x: 14, y: 175);
		ConfigureButton(button: buttonCancel, text: "C&ancel", location: new Point(x: 125, y: 175), description: "Cancels the running calculation", click: ButtonCancel_Click);
		ConfigureButton(button: buttonExportCsv, text: "E&xport CSV...", location: new Point(x: 236, y: 175), description: "Exports the ephemeris as CSV (invariant culture, UTC)", click: ButtonExportCsv_Click);
		ConfigureButton(button: buttonChart, text: "C&hart", location: new Point(x: 347, y: 175), description: "Shows altitude and magnitude over time", click: ButtonChart_Click);
		progressBar.Location = new Point(x: 460, y: 180);
		progressBar.Size = new Size(width: 440, height: 20);
		progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		labelPercent.Location = new Point(x: 910, y: 178);
		labelPercent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		labelPercent.Values.Text = "0 %";

		// Result list
		listView.Location = new Point(x: 14, y: 215);
		listView.Size = new Size(width: 952, height: 385);
		listView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		listView.View = View.Details;
		listView.FullRowSelect = true;
		listView.ListView.VirtualMode = true;
		listView.ListView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		listView.AccessibleName = "Ephemeris";
		listView.AccessibleDescription = "Shows the calculated ephemeris";
		foreach ((string header, int width) in new[]
		{
			("Time (UTC)", 130), ("Local time", 130), ("RA (J2000, h m s)", 105), ("Dec (J2000, ° ′ ″)", 105),
			("Az (°, N=0 E=90)", 95), ("Alt (°)", 60), ("Δ (AU)", 85), ("r (AU)", 85), ("Phase (°)", 65),
			("Elong. (°)", 65), ("V (mag)", 60), ("Sun alt. (°)", 75), ("Moon dist. (°)", 85), ("Visible", 55)
		})
		{
			_ = listView.Columns.Add(text: header, width: width);
		}
		ResumeLayout(performLayout: true);
	}

	/// <summary>Builds the ephemeris request from the user input.</summary>
	/// <param name="elements">The orbital elements.</param>
	/// <returns>The request.</returns>
	/// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
	private EphemerisRequest BuildRequest(MinorPlanetElements elements)
	{
		TimeZoneInfo zone = comboBoxTimeZone.SelectedItem as TimeZoneInfo ?? TimeZoneInfo.Utc;
		DateTimeOffset start = EphemerisRequest.ConvertToUtc(localTime: dateTimePickerEphemeridesBegin.Value, timeZone: zone);
		DateTimeOffset end = EphemerisRequest.ConvertToUtc(localTime: dateTimePickerEphemeridesEnd.Value, timeZone: zone);
		TimeSpan step = StepUnits[Math.Max(val1: comboBoxStepUnit.SelectedIndex, val2: 0)].Unit * (double)numericUpDownStepsInDays.Value;
		return new EphemerisRequest(
			Elements: elements,
			Times: EphemerisRequest.CreateTimeSeries(start: start, end: end, step: step),
			Observer: new ObserverLocation(LatitudeDegrees: (double)numericLatitude.Value, LongitudeDegrees: (double)numericLongitude.Value, HeightMeters: (double)numericHeight.Value),
			Criteria: new VisibilityCriteria(
				MinimumObjectAltitudeDegrees: (double)numericMinimumAltitude.Value,
				MaximumSunAltitudeDegrees: (double)numericMaximumSunAltitude.Value,
				LimitingMagnitude: (double)numericLimitingMagnitude.Value,
				MinimumMoonSeparationDegrees: (double)numericMinimumMoonSeparation.Value),
			Model: comboBoxModel.SelectedIndex == 1 ? PropagationModel.TwoBody : PropagationModel.Perturbed,
			ApplyRefraction: checkBoxRefraction.Checked);
	}

	/// <summary>Enables or disables the input controls while a calculation is running.</summary>
	/// <param name="running">Whether a calculation is running.</param>
	private void SetRunningState(bool running)
	{
		foreach (Control control in kryptonPanelMain.Controls)
		{
			if (control != listView && control != progressBar && control != labelPercent)
			{
				control.Enabled = !running;
			}
		}
		buttonCancel.Enabled = running;
		buttonExportCsv.Enabled = !running && entries.Count > 0;
		buttonChart.Enabled = !running && entries.Count > 0;
	}

	/// <summary>Updates the progress display (must be called on the UI thread).</summary>
	/// <param name="percent">The progress in percent.</param>
	private void UpdateProgress(int percent)
	{
		progressBar.Value = Math.Clamp(value: percent, min: progressBar.Minimum, max: progressBar.Maximum);
		labelPercent.Values.Text = string.Create(provider: CultureInfo.CurrentCulture, handler: $"{percent} %");
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the Load event of the form.</remarks>
	private void EphemerisForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		if (MinorPlanetElements.TryParse(record: mpcorbRecord, elements: out MinorPlanetElements? elements, error: out _))
		{
			Text = $"Ephemerides – {elements.Designation}";
		}
	}

	/// <summary>Cancels a running calculation when the form is closing.</summary>
	/// <param name="e">The event data.</param>
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		cancellationTokenSource?.Cancel();
		base.OnFormClosing(e: e);
	}

	/// <summary>Releases the loaded JPL ephemeris when the form is closed.</summary>
	/// <param name="e">The event data.</param>
	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		jplEphemeris?.Dispose();
		jplEphemeris = null;
		base.OnFormClosed(e: e);
	}

	#endregion

	#region list view event handlers

	/// <summary>Provides the list view items in virtual mode.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		EphemerisEntry entry = entries[e.ItemIndex];
		CultureInfo c = CultureInfo.CurrentCulture;
		TimeZoneInfo zone = comboBoxTimeZone.SelectedItem as TimeZoneInfo ?? TimeZoneInfo.Utc;
		DateTime local = TimeZoneInfo.ConvertTimeFromUtc(dateTime: entry.Time.UtcDateTime, destinationTimeZone: zone);
		e.Item = new ListViewItem(items:
		[
			entry.Time.UtcDateTime.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.InvariantCulture),
			local.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.InvariantCulture),
			EphemerisExportService.FormatRightAscension(hours: entry.RightAscensionHours),
			EphemerisExportService.FormatDeclination(degrees: entry.DeclinationDegrees),
			entry.AzimuthDegrees.ToString(format: "F2", provider: c),
			entry.AltitudeDegrees.ToString(format: "F2", provider: c),
			entry.DistanceAu.ToString(format: "F6", provider: c),
			entry.HeliocentricDistanceAu.ToString(format: "F6", provider: c),
			entry.PhaseAngleDegrees.ToString(format: "F1", provider: c),
			entry.SolarElongationDegrees.ToString(format: "F1", provider: c),
			double.IsNaN(d: entry.ApparentMagnitude) ? "–" : entry.ApparentMagnitude.ToString(format: "F2", provider: c),
			entry.SunAltitudeDegrees.ToString(format: "F1", provider: c),
			entry.MoonSeparationDegrees.ToString(format: "F1", provider: c),
			entry.IsVisible ? "yes" : "no"
		]);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Calculate button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The calculation runs on a background thread; progress and results are marshalled back to the UI thread.</remarks>
	private async void ButtonCalculate_Click(object sender, EventArgs e)
	{
		if (!MinorPlanetElements.TryParse(record: mpcorbRecord, elements: out MinorPlanetElements? elements, error: out string? parseError))
		{
			logger.Warn(message: $"Ephemeris calculation not possible: {parseError}");
			ShowErrorMessage(message: $"The orbital elements of the minor planet cannot be used: {parseError}");
			return;
		}
		EphemerisRequest request;
		try
		{
			request = BuildRequest(elements: elements);
		}
		catch (ArgumentException ex)
		{
			ShowErrorMessage(message: ex.Message);
			return;
		}
		entries = [];
		listView.ListView.VirtualListSize = 0;
		UpdateProgress(percent: 0);
		using CancellationTokenSource cts = new();
		cancellationTokenSource = cts;
		SetRunningState(running: true);
		SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.CurrentCulture, handler: $"Calculating {request.Times.Count} positions..."));
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			// Progress<T> captures the UI SynchronizationContext, so UpdateProgress runs on the UI thread
			Progress<int> progress = new(handler: UpdateProgress);
			EphemerisService service = new(ephemeris: jplEphemeris);
			entries = await service.CalculateAsync(request: request, progress: progress, cancellationToken: cts.Token);
			listView.ListView.VirtualListSize = entries.Count;
			listView.Invalidate();
			int visibleCount = entries.Count(predicate: static x => x.IsVisible);
			SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.CurrentCulture, handler: $"{entries.Count} positions calculated in {stopwatch.Elapsed.TotalSeconds:F1} s, {visibleCount} visible ({service.EphemerisName})."));
		}
		catch (OperationCanceledException)
		{
			if (!IsDisposed)
			{
				SetStatusBar(label: labelInformation, text: "Calculation cancelled.");
			}
		}
		catch (Exception ex) when (ex is InvalidOperationException or ArgumentException or IOException)
		{
			logger.Error(exception: ex, message: "Ephemeris calculation failed.");
			ShowErrorMessage(message: $"The ephemeris calculation failed: {ex.Message}");
		}
		finally
		{
			cancellationTokenSource = null;
			if (!IsDisposed)
			{
				SetRunningState(running: false);
			}
		}
	}

	/// <summary>Cancels the running calculation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ButtonCancel_Click(object? sender, EventArgs e) => cancellationTokenSource?.Cancel();

	/// <summary>Loads a JPL DE440/DE441 SPK file.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ButtonLoadJplEphemeris_Click(object? sender, EventArgs e)
	{
		using OpenFileDialog dialog = new()
		{
			Filter = "JPL SPK ephemeris (*.bsp)|*.bsp|All files (*.*)|*.*",
			Title = "Load JPL DE440/DE441 ephemeris",
			Multiselect = true
		};
		if (dialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		try
		{
			JplSpkEphemeris loaded = new(filePaths: dialog.FileNames);
			jplEphemeris?.Dispose();
			jplEphemeris = loaded;
			labelEphemerisSource.Values.Text = loaded.Name;
			logger.Info(message: $"Loaded JPL ephemeris {loaded.Name}");
		}
		catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or NotSupportedException)
		{
			logger.Error(exception: ex, message: "Loading the JPL ephemeris failed.");
			ShowErrorMessage(message: $"The JPL ephemeris could not be loaded: {ex.Message}");
		}
	}

	/// <summary>Exports the ephemeris as CSV.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private async void ButtonExportCsv_Click(object? sender, EventArgs e)
	{
		using SaveFileDialog dialog = new()
		{
			Filter = "CSV files (*.csv)|*.csv",
			FileName = "Ephemeris.csv",
			Title = "Export ephemeris as CSV"
		};
		if (dialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		try
		{
			await EphemerisExportService.ExportCsvAsync(entries: entries, filePath: dialog.FileName);
			SetStatusBar(label: labelInformation, text: "Ephemeris exported.", additionalInfo: dialog.FileName);
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			logger.Error(exception: ex, message: "CSV export failed.");
			ShowErrorMessage(message: $"The export failed: {ex.Message}");
		}
	}

	/// <summary>Shows a chart of altitude, Sun altitude and magnitude over time.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ButtonChart_Click(object? sender, EventArgs e)
	{
		using Form chartForm = new() { Text = $"{Text} – chart", Size = new Size(width: 900, height: 600), StartPosition = FormStartPosition.CenterParent, TopMost = TopMost };
		FormsPlot formsPlot = new() { Dock = DockStyle.Fill };
		chartForm.Controls.Add(value: formsPlot);
		double[] xs = [.. entries.Select(selector: static x => x.Time.UtcDateTime.ToOADate())];
		formsPlot.Plot.Add.Scatter(xs: xs, ys: [.. entries.Select(selector: static x => x.AltitudeDegrees)]).LegendText = "Altitude (°)";
		formsPlot.Plot.Add.Scatter(xs: xs, ys: [.. entries.Select(selector: static x => x.SunAltitudeDegrees)]).LegendText = "Sun altitude (°)";
		if (entries.Any(predicate: static x => !double.IsNaN(d: x.ApparentMagnitude)))
		{
			formsPlot.Plot.Add.Scatter(xs: xs, ys: [.. entries.Select(selector: static x => x.ApparentMagnitude)]).LegendText = "V (mag)";
		}
		_ = formsPlot.Plot.Axes.DateTimeTicksBottom();
		formsPlot.Plot.Axes.Bottom.Label.Text = "Time (UTC)";
		formsPlot.Plot.Axes.Left.Label.Text = "Degrees / mag";
		_ = formsPlot.Plot.ShowLegend();
		formsPlot.Refresh();
		_ = chartForm.ShowDialog(owner: this);
	}

	#endregion
}
