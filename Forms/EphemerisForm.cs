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
using Planetoid_DB.Properties;
using Planetoid_DB.Services;

using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>Represents a form for calculating and displaying ephemerides of a minor planet.</summary>
/// <remarks>The user chooses a time range (UTC), a step size, the observer location and visibility criteria. The calculation runs asynchronously via <see cref="EphemerisService"/> and can be canceled; results are shown in a list and can be exported in multiple formats.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class EphemerisForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The analytical planetary ephemeris used when no JPL file is loaded.</summary>
	/// <remarks>This ephemeris is used as a fallback when no JPL DE440/DE441 file is available.</remarks>
	private static readonly AnalyticalPlanetaryEphemeris analyticalEphemeris = new();

	/// <summary>The loaded JPL DE440/DE441 file, if any.</summary>
	/// <remarks>This ephemeris is used for highest accuracy when available; it is disposed when the form closes.</remarks>
	private JplDevelopmentEphemeris? jplEphemeris;

	/// <summary>The orbital elements of the current minor planet, if valid.</summary>
	/// <remarks>This property holds the orbital elements of the minor planet currently being analyzed.</remarks>
	private MinorPlanetOrbitalElements? elements;

	/// <summary>The cancellation source of the running calculation, if any.</summary>
	/// <remarks>This property holds the cancellation token source used to cancel the running calculation.</remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>The result of the last calculation.</summary>
	/// <remarks>This property holds the ephemeris entries calculated in the last successful calculation.</remarks>
	private IReadOnlyList<EphemerisEntry> lastResult = [];

	/// <summary>Indicates that the form should close after a running calculation has stopped.</summary>
	/// <remarks>This flag is set when the user attempts to close the form while a calculation is running; the form will close after the calculation completes or is canceled.</remarks>
	private bool closeAfterCalculation;

	/// <summary>The virtual list view used to export every calculated ephemeris entry.</summary>
	/// <remarks>This list view is separate from the display list so the display limit does not truncate exports.</remarks>
	private readonly ListView exportListView = new() { VirtualMode = true };

	/// <summary>The maximum number of result rows displayed in the list.</summary>
	/// <remarks>This constant defines the maximum number of ephemeris entries that will be displayed in the list view; if more entries are calculated, only the first 2000 are shown.</remarks>
	private const int MaximumDisplayedResultPoints = 2000;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the list view used as the source for ephemeris exports.</summary>
	/// <remarks>The virtual list view exposes the complete, unrounded result set.</remarks>
	protected override ListView? ExportListView => exportListView;

	/// <summary>Gets the title used for exported ephemeris data.</summary>
	/// <remarks>Overrides the base export title for this form's results.</remarks>
	protected override string ExportTitle => "Ephemerides";

	/// <summary>Gets the virtual row provider used to export ephemeris results.</summary>
	/// <remarks>Returns values from the complete calculation result rather than the capped display list.</remarks>
	protected override Func<int, ListViewItem>? ExportVirtualRowProvider => GetExportListViewItem;

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
		// Validate the input
		logger.Info(message: $"Setting MPCORB record for ephemerides: {rawRecord}");
		// Parse the MPCORB record into orbital elements
		if (MpcorbElementsParser.TryParse(rawLine: rawRecord, elements: out MinorPlanetOrbitalElements? parsed, error: out string? error))
		{
			// Valid MPCORB record; store the parsed elements and update the form title
			elements = parsed;
			Text = $"Ephemerides: {parsed.Designation}";
		}
		// Invalid MPCORB record; clear the elements and log a warning
		else
		{
			elements = null;
			logger.Warn(message: $"Invalid MPCORB record for ephemerides: {error}");
			Text = "Ephemerides: invalid MPCORB record";
		}
	}

	/// <summary>Disposes resources owned by the form.</summary>
	/// <param name="disposing"><see langword="true"/> to dispose managed resources; otherwise, <see langword="false"/>.</param>
	/// <remarks>Cancels any running calculation and disposes the loaded ephemeris and export list.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
			jplEphemeris?.Dispose();
			jplEphemeris = null;
			exportListView?.Dispose();
			components?.Dispose();
		}
		base.Dispose(disposing);
	}

	/// <summary>Gets the planetary ephemeris currently in use.</summary>
	/// <remarks>This property returns the loaded JPL ephemeris if available; otherwise, it falls back to the analytical ephemeris.</remarks>
	private IPlanetaryEphemerisProvider PlanetaryEphemeris => jplEphemeris ?? (IPlanetaryEphemerisProvider)analyticalEphemeris;

	/// <summary>Enables or disables the input controls while a calculation is running.</summary>
	/// <param name="running"><c>true</c> if a calculation is running; otherwise, <c>false</c>.</param>
	/// <remarks>This method updates the enabled state of the buttons based on whether a calculation is in progress and whether valid orbital elements are available.</remarks>
	private void SetRunningState(bool running)
	{
		logger.Info(message: $"Setting running state to {running}");
		// Enable or disable buttons based on the running state and the presence of valid elements
		buttonCalculate.Enabled = !running && elements is not null;
		kryptonDropButtonLoadEphemerisFile.Enabled = !running;
		buttonCancel.Enabled = running;
		kryptonDropButtonSaveList.Enabled = !running && lastResult.Count > 0;
		kryptonButtonOpenWorldMap.Enabled = !running;
		contextMenuSaveToFile.Enabled = !running && lastResult.Count > 0;
	}

	/// <summary>Sets the progress bar and the percent label.</summary>
	/// <param name="percent">The progress in percent.</param>
	/// <remarks>This method updates the progress bar value and the associated label to reflect the current progress of the calculation.</remarks>
	private void SetProgress(int percent)
	{
		// Clamp the progress value to the range [0, 100]
		int value = Math.Clamp(value: percent, min: 0, max: 100);
		// Update the progress bar and label with the clamped value
		progressBar.Value = value;
		progressBar.Text = string.Create(provider: CultureInfo.InvariantCulture, handler: $"{value} %");
	}

	/// <summary>Creates the columns of the result list.</summary>
	/// <remarks>This method initializes the columns of the list view to display the ephemeris results, including time, coordinates, and visibility information.</remarks>
	private void CreateColumns()
	{
		logger.Info(message: "Creating list view columns for ephemerides");
		// Clear any existing columns in the list view
		listView.Columns.Clear();
		exportListView.Columns.Clear();
		// Define the headers for the list view columns
		string[] headers = ["Time (UTC)", "RA J2000 (h m s)", "Dec J2000 (° ′ ″)", "RA of date (h m s)", "Dec of date (° ′ ″)", "Azimuth (°)", "Altitude (°)", "Distance Δ (AU)", "Sun dist. r (AU)", "Mag. (mag)", "Phase (°)", "Elongation (°)", "Sun alt. (°)", "Moon dist. (°)", "Visible"];
		// Add each header to the list view with appropriate width based on the header length
		foreach (string header in headers)
		{
			// Add the column to the list view; use a wider width for longer headers
			_ = listView.Columns.Add(text: header, width: header.Length > 12 ? 110 : 90);
		}
		string[] exportHeaders = ["Time (UTC, ISO 8601)", "RA J2000 (h)", "Dec J2000 (°)", "RA of date (h)", "Dec of date (°)", "Azimuth (°)", "Altitude (°)", "Distance Δ (AU)", "Sun dist. r (AU)", "Mag. (mag)", "Phase (°)", "Elongation (°)", "Sun alt. (°)", "Moon dist. (°)", "Visible"];
		foreach (string header in exportHeaders)
		{
			_ = exportListView.Columns.Add(text: header);
		}
		logger.Info(message: "List view columns created successfully");
	}

	/// <summary>Creates an export row from the complete ephemeris result.</summary>
	/// <param name="index">The index of the result to export.</param>
	/// <returns>A list view item containing the result's full-precision values.</returns>
	private ListViewItem GetExportListViewItem(int index)
	{
		EphemerisEntry entry = lastResult[index];
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		return new ListViewItem(items:
		[
			entry.Time.UtcDateTime.ToString(format: "O", provider: invariantCulture),
			entry.AstrometricRightAscensionHours.ToString(format: "R", provider: invariantCulture),
			entry.AstrometricDeclinationDegrees.ToString(format: "R", provider: invariantCulture),
			entry.RightAscensionHours.ToString(format: "R", provider: invariantCulture),
			entry.DeclinationDegrees.ToString(format: "R", provider: invariantCulture),
			entry.AzimuthDegrees.ToString(format: "R", provider: invariantCulture),
			entry.AltitudeDegrees.ToString(format: "R", provider: invariantCulture),
			entry.DistanceAu.ToString(format: "R", provider: invariantCulture),
			entry.HeliocentricDistanceAu.ToString(format: "R", provider: invariantCulture),
			double.IsFinite(d: entry.ApparentMagnitude) ? entry.ApparentMagnitude.ToString(format: "R", provider: invariantCulture) : "–",
			entry.PhaseAngleDegrees.ToString(format: "R", provider: invariantCulture),
			entry.ElongationDegrees.ToString(format: "R", provider: invariantCulture),
			entry.SunAltitudeDegrees.ToString(format: "R", provider: invariantCulture),
			entry.MoonSeparationDegrees.ToString(format: "R", provider: invariantCulture),
			entry.IsVisible ? "yes" : "no"
		]);
	}

	/// <summary>Shows the result in the list view.</summary>
	/// <param name="entries">The ephemeris entries.</param>
	/// <remarks>Visible entries are highlighted.</remarks>
	private void ShowResult(IReadOnlyList<EphemerisEntry> entries)
	{
		// Use invariant culture for consistent formatting of numbers and dates
		CultureInfo ic = CultureInfo.InvariantCulture;
		// Begin updating the list view to prevent flickering during the update
		listView.BeginUpdate();
		// Clear the existing items in the list view
		try
		{
			// Log the update action for debugging purposes
			logger.Info(message: "Updating list view with ephemeris results");
			// Clear the existing items in the list view to prepare for new entries
			listView.Items.Clear();
			exportListView.VirtualListSize = entries.Count;
			// Determine the number of entries to display, limited by the maximum allowed
			int displayedCount = Math.Min(val1: entries.Count, val2: MaximumDisplayedResultPoints);
			// Create an array of ListViewItem objects to hold the entries to be displayed
			ListViewItem[] items = new ListViewItem[displayedCount];
			// Populate the ListViewItem array with the ephemeris entries, formatting each entry's data for display
			for (int i = 0; i < displayedCount; i++)
			{
				// Get the current ephemeris entry to be displayed
				EphemerisEntry e = entries[index: i];
				// Create a new ListViewItem with the formatted data from the ephemeris entry
				items[i] = new ListViewItem(items:
				[
					// Format the time in UTC with the specified format
					e.Time.UtcDateTime.ToString(format: "yyyy-MM-dd HH:mm", provider: ic),
					// Format the right ascension and declination for both J2000 and of date
					CoordinateTransformationService.FormatRightAscension(hours: e.AstrometricRightAscensionHours),
					// Format the declination for J2000
					CoordinateTransformationService.FormatDeclination(degrees: e.AstrometricDeclinationDegrees),
					// Format the right ascension and declination for the current date
					CoordinateTransformationService.FormatRightAscension(hours: e.RightAscensionHours),
					// Format the declination for the current date
					CoordinateTransformationService.FormatDeclination(degrees: e.DeclinationDegrees),
					// Format the azimuth and altitude with two decimal places
					e.AzimuthDegrees.ToString(format: "0.00", provider: ic),
					// Format the altitude with two decimal places
					e.AltitudeDegrees.ToString(format: "0.00", provider: ic),
					// Format the distance and heliocentric distance with six decimal places
					e.DistanceAu.ToString(format: "0.000000", provider: ic),
					// Format the heliocentric distance with six decimal places
					e.HeliocentricDistanceAu.ToString(format: "0.000000", provider: ic),
					// Format the apparent magnitude with one decimal place, or display a dash if not finite
					double.IsFinite(d: e.ApparentMagnitude) ? e.ApparentMagnitude.ToString(format: "0.0", provider: ic) : "–",
					// Format the phase angle, elongation, sun altitude, and moon separation with one decimal place
					e.PhaseAngleDegrees.ToString(format: "0.0", provider: ic),
					// Format the elongation with one decimal place
					e.ElongationDegrees.ToString(format: "0.0", provider: ic),
					// Format the sun altitude with one decimal place
					e.SunAltitudeDegrees.ToString(format: "0.0", provider: ic),
					// Format the moon separation with one decimal place
					e.MoonSeparationDegrees.ToString(format: "0.0", provider: ic),
					// Indicate whether the object is visible based on the visibility criteria
					e.IsVisible ? "yes" : "no"
				])
				// Set the background color of the item based on its visibility status
				{
					BackColor = e.IsVisible ? Color.Honeydew : listView.BackColor
				};
			}
			// Add the populated items to the list view for display
			listView.Items.AddRange(items: items);
		}
		// Ensure that the list view update is completed and the UI is refreshed, even if an exception occurs
		finally
		{
			logger.Info(message: "Finished updating list view with ephemeris results");
			listView.EndUpdate();
		}
	}

	/// <summary>Collects the calculation parameters from the controls.</summary>
	/// <returns>The time grid, observer, criteria and options.</returns>
	/// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
	/// <remarks>This method reads the user input from the form controls, validates it, and constructs the necessary parameters for the ephemeris calculation.</remarks>
	private (IReadOnlyList<DateTimeOffset> Times, ObserverLocation Observer, VisibilityCriteria Criteria, EphemerisOptions Options) ReadInput()
	{
		// Create DateTimeOffset instances for the begin and end times, specifying them as UTC
		DateTimeOffset begin = new(dateTime: DateTime.SpecifyKind(value: dateTimePickerEphemeridesBegin.Value, kind: DateTimeKind.Unspecified), offset: TimeSpan.Zero);
		DateTimeOffset end = new(dateTime: DateTime.SpecifyKind(value: dateTimePickerEphemeridesEnd.Value, kind: DateTimeKind.Unspecified), offset: TimeSpan.Zero);
		// Validate that the end time is not before the begin time; throw an exception if invalid
		if (end < begin)
		{
			// Log the invalid input and throw an exception to indicate the error
			logger.Error(message: "The end of the ephemerides must not be before the begin.");
			throw new ArgumentException(message: "The end of the ephemerides must not be before the begin.");
		}
		// Create a TimeSpan for the step size based on the user input in days
		TimeSpan step = TimeSpan.FromDays(value: (double)numericUpDownStepsInDays.Value);
		// Validate that the step size is at least one minute; throw an exception if invalid
		if (step < TimeSpan.FromMinutes(minutes: 1))
		{
			logger.Error(message: "The step size must be at least one minute (0.0007 days).");
			throw new ArgumentException(message: "The step size must be at least one minute (0.0007 days).");
		}
		// Check if the number of time points exceeds the maximum allowed; throw an exception if it does
		if (((end - begin).Ticks / step.Ticks) + 1 > EphemerisService.MaximumTimePoints)
		{
			logger.Error(message: $"Too many time points; at most {EphemerisService.MaximumTimePoints} are allowed. Increase the step size or shorten the range.");
			throw new ArgumentException(message: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Too many time points; at most {EphemerisService.MaximumTimePoints} are allowed. Increase the step size or shorten the range."));
		}
		// Create the time grid using the EphemerisService, which generates a list of DateTimeOffset values from begin to end with the specified step size
		IReadOnlyList<DateTimeOffset> times = EphemerisService.CreateTimeGrid(start: begin, end: end, step: step);
		// Create the observer location based on the user input for latitude, longitude, and elevation
		ObserverLocation observer = new(
			LatitudeDegrees: (double)numericUpDownLatitude.Value,
			LongitudeDegrees: (double)numericUpDownLongitude.Value,
			ElevationMeters: (double)numericUpDownElevation.Value);
		// Create the visibility criteria based on the user input for minimum altitude, maximum sun altitude, faintest magnitude, and minimum moon separation
		VisibilityCriteria criteria = new(
			MinimumAltitudeDegrees: (double)numericUpDownMinimumAltitude.Value,
			MaximumSunAltitudeDegrees: (double)numericUpDownMaximumSunAltitude.Value,
			FaintestMagnitude: checkBoxFaintestMagnitude.Checked ? (double)numericUpDownFaintestMagnitude.Value : null,
			MinimumMoonSeparationDegrees: (double)numericUpDownMinimumMoonSeparation.Value);
		// Create the ephemeris options based on the user input for planetary perturbations and refraction
		EphemerisOptions options = new(IncludePlanetaryPerturbations: checkBoxPerturbations.Checked, ApplyRefraction: checkBoxRefraction.Checked);
		// Return the collected parameters as a tuple
		return (Times: times, observer, criteria, options);
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method initializes the inputs with a 30-day range starting today (UTC).</remarks>
	private void EphemerisForm_Load(object sender, EventArgs e)
	{
		// Log the form load event for debugging purposes
		logger.Info(message: "Ephemeris form loaded");
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// Set the date pickers to a 30-day range starting from today in UTC
		DateTime today = DateTime.UtcNow.Date;
		dateTimePickerEphemeridesBegin.Value = today;
		dateTimePickerEphemeridesEnd.Value = today.AddDays(value: 30);
		// Create the columns for the ephemeris data grid
		CreateColumns();
		// Initialize the progress bar and percent label to 0%
		SetProgress(percent: 0);
		// Set the planetary ephemeris label to indicate which ephemeris is currently in use
		labelEphemerisSource.Text = $"Planetary ephemeris: {PlanetaryEphemeris.Name}";
		// Set the running state to false, indicating that no calculation is currently in progress
		SetRunningState(running: false);
		// If no valid MPCORB record is available, display a message in the status bar
		if (elements is null)
		{
			logger.Warn(message: "No valid MPCORB record available – the ephemerides cannot be calculated.");
			SetStatusBar(label: labelInformation, text: "No valid MPCORB record available – the ephemerides cannot be calculated.");
		}
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the JPL file.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>If a calculation is running, it is canceled and the form will close after the calculation stops. If no calculation is running, the JPL ephemeris file is disposed.</remarks>
	private void EphemerisForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		// Log the form closing event for debugging purposes
		if (cancellationTokenSource is not null)
		{
			// A calculation is currently running; log the cancellation and set the flag to close after calculation
			logger.Info(message: "Form is closing while a calculation is running; canceling the calculation and closing after completion.");
			e.Cancel = true;
			closeAfterCalculation = true;
			cancellationTokenSource.Cancel();
			return;
		}
		// No calculation is running; dispose of the JPL ephemeris if it exists
		else
		{
			// Log the disposal of the JPL ephemeris for debugging purposes
			logger.Info(message: "Form is closing; disposing of the JPL ephemeris if loaded.");
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
		// Log the button click event for debugging purposes
		logger.Info(message: "Calculate button clicked; starting ephemeris calculation.");
		// If no valid MPCORB record is available or a calculation is already running, do nothing
		if (elements is null || cancellationTokenSource is not null)
		{
			return;
		}
		// Read the input parameters from the form controls; if invalid, show a warning and return
		IReadOnlyList<DateTimeOffset> times;
		ObserverLocation observer;
		VisibilityCriteria criteria;
		EphemerisOptions options;
		try
		{
			// Read the input parameters from the form controls and validate them
			(times, observer, criteria, options) = ReadInput();
		}
		catch (ArgumentException ex)
		{
			// Log the invalid input and show a warning message box to the user
			logger.Warn(exception: ex, message: "Invalid input provided for ephemeris calculation.");
			_ = KryptonMessageBox.Show(text: ex.Message, caption: "Invalid input", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Start the calculation asynchronously using a cancellation token source
		using CancellationTokenSource cts = new();
		// Store the cancellation token source for potential cancellation of the calculation
		cancellationTokenSource = cts;
		// Clear the last result since a new calculation is starting
		lastResult = [];
		exportListView.VirtualListSize = 0;
		// Update the UI to indicate that a calculation is running
		SetRunningState(running: true);
		// Reset the progress bar and percent label to 0% at the start of the calculation
		SetProgress(percent: 0);
		// Update the status bar to indicate that the calculation is starting
		SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Calculating {times.Count} positions..."));
		// Start a stopwatch to measure the elapsed time of the calculation
		Stopwatch stopwatch = Stopwatch.StartNew();
		// Perform the ephemeris calculation asynchronously and handle any exceptions that may occur
		try
		{
			// Create an instance of the EphemerisService with the selected planetary ephemeris
			EphemerisService service = new(planetaryEphemeris: PlanetaryEphemeris);
			// Create a progress handler to update the progress bar and percent label during the calculation
			Progress<int> progress = new(handler: SetProgress);
			// Log the start of the ephemeris calculation for debugging purposes
			logger.Info(message: $"Starting ephemeris calculation for {elements.Designation} with {times.Count} positions.");
			// Perform the ephemeris calculation asynchronously, passing in the orbital elements, time grid, observer location, visibility criteria, options, progress handler, and cancellation token
			lastResult = await service.CalculateAsync(elements: elements, times: times, observer: observer, criteria: criteria, options: options, progress: progress, cancellationToken: cts.Token).ConfigureAwait(continueOnCapturedContext: true);
			// Stop the stopwatch to measure the elapsed time of the calculation
			stopwatch.Stop();
			// Log the completion of the ephemeris calculation for debugging purposes
			logger.Info(message: $"Completed ephemeris calculation for {elements.Designation} with {times.Count} positions.");
			// If there are results and debug logging is enabled, log the first result for detailed information
			if (lastResult.Count > 0 && logger.IsDebugEnabled)
			{
				// Log the first ephemeris entry with detailed information including time, coordinates, and distances
				EphemerisEntry first = lastResult[index: 0];
				logger.Debug(message: string.Create(provider: CultureInfo.InvariantCulture, handler: $"Ephemeris {elements.Designation} at {first.Time.UtcDateTime:O} (JD UTC {TimeScales.ToJulianDateUtc(time: first.Time):0.000000}, {PlanetaryEphemeris.Name}): RA/Dec J2000 {first.AstrometricRightAscensionHours:0.000000} h / {first.AstrometricDeclinationDegrees:0.00000}°, RA/Dec of date {first.RightAscensionHours:0.000000} h / {first.DeclinationDegrees:0.00000}°, Δ {first.DistanceAu:0.000000} AU, r {first.HeliocentricDistanceAu:0.000000} AU"));
			}
			// Display the calculated ephemeris results
			ShowResult(entries: lastResult);
			// Count the number of visible entries in the result for display in the status bar
			int visibleCount = lastResult.Count(predicate: static entry => entry.IsVisible);
			// Create a note indicating if the displayed results are limited to the maximum allowed
			string displayNote = lastResult.Count > MaximumDisplayedResultPoints ? $" The first {MaximumDisplayedResultPoints} are shown in the list." : string.Empty;
			// Update the status bar with the number of positions calculated, elapsed time, visible count, and any display note
			SetStatusBar(label: labelInformation, text: string.Create(provider: CultureInfo.InvariantCulture, handler: $"{lastResult.Count} positions calculated in {stopwatch.Elapsed.TotalSeconds:0.0} s; visible: {visibleCount}.{displayNote}"));
		}
		// Handle cancellation of the calculation gracefully
		catch (OperationCanceledException)
		{
			// Log the cancellation of the ephemeris calculation for debugging purposes
			logger.Warn(message: "Ephemeris calculation was canceled by the user.");
			SetStatusBar(label: labelInformation, text: "Calculation canceled.");
		}
		// Handle argument exceptions that may occur during the calculation, such as invalid input parameters
		catch (ArgumentException ex)
		{
			// Log the argument exception and show a warning message box to the user
			logger.Warn(exception: ex, message: "Ephemeris calculation failed.");
			SetStatusBar(label: labelInformation, text: ex.Message);
			_ = KryptonMessageBox.Show(text: ex.Message, caption: "Ephemerides", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
		}
		// Handle other exceptions that may occur during the calculation, such as I/O errors or invalid data
		catch (Exception ex) when (ex is IOException or InvalidDataException or InvalidOperationException)
		{
			// Log the exception and show an error message to the user
			logger.Error(exception: ex, message: "Ephemeris calculation failed.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"The ephemerides could not be calculated: {ex.Message}");
		}
		// Handle any other unexpected exceptions that may occur during the calculation
		catch (Exception ex)
		{
			// Log the unexpected exception and show an error message to the user
			logger.Error(exception: ex, message: "Unexpected error during ephemeris calculation.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"An unexpected error occurred during the ephemerides calculation: {ex.Message}");
		}
		// Ensure that the cancellation token source is cleared and the UI state is updated after the calculation completes or is canceled
		finally
		{
			// Log the completion of the ephemeris calculation and the update of the UI state
			logger.Info(message: "Ephemeris calculation finished; updating UI state.");
			// Clear the cancellation token source
			cancellationTokenSource = null;
			// If the form was requested to close after the calculation, close it now
			if (closeAfterCalculation)
			{
				closeAfterCalculation = false;
				Close();
			}
			// If the form is not disposed and not disposing, update the running state to false
			else if (!IsDisposed && !Disposing)
			{
				SetRunningState(running: false);
			}
			// If the form is disposed or disposing, dispose of the JPL ephemeris if it exists
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
	/// <remarks>Cancels the running calculation if any.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// Log the cancellation request
		logger.Info(message: "Cancellation requested by the user.");
		// Cancel the running calculation if a cancellation token source exists
		cancellationTokenSource?.Cancel();
	}

	/// <summary>Handles the Click event of the Load DE440/441 button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Loads a JPL DE binary file (e.g. <c>linux_p1550p2650.440</c> or <c>linux_m13000p17000.441</c>) for highest accuracy.</remarks>
	private void ButtonLoadEphemerisFile_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Load DE440/441 button clicked; prompting user to select a JPL ephemeris file.");
		// Show an OpenFileDialog to allow the user to choose the JPL DE binary file to load
		using OpenFileDialog openFileDialog = new()
		{
			Filter = "JPL DE binary files (*.440;*.441)|*.440;*.441|All files (*.*)|*.*",
			Title = "Load JPL DE440/DE441 binary ephemeris"
		};
		// If the user cancels the OpenFileDialog, return without loading a file
		if (openFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		// Attempt to load the selected JPL DE binary file and update the planetary ephemeris
		try
		{
			// Log the file loading operation
			logger.Info(message: $"Loading JPL DE ephemeris from file: {openFileDialog.FileName}");
			// Load the JPL DE binary file and create a new JplDevelopmentEphemeris instance
			JplDevelopmentEphemeris loaded = new(filePath: openFileDialog.FileName);
			// Dispose of the previous JPL ephemeris if it exists to free resources
			jplEphemeris?.Dispose();
			// Update the planetary ephemeris to the newly loaded JPL ephemeris
			jplEphemeris = loaded;
			// Update the label to indicate which planetary ephemeris is currently in use
			labelEphemerisSource.Text = $"Planetary ephemeris: {loaded.Name}";
			// Update the status bar to indicate that the JPL ephemeris has been successfully loaded
			SetStatusBar(label: labelInformation, text: $"Loaded {loaded.Name}");
		}
		// Handle exceptions that may occur during the loading of the JPL DE binary file, such as I/O errors or invalid data
		catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or NotSupportedException)
		{
			// Log the exception and show an error message to the user
			logger.Error(exception: ex, message: "Loading the JPL ephemeris failed.");
			ExportFeedbackHelper.ShowErrorMessage(message: $"The file could not be loaded as JPL DE ephemeris: {ex.Message}");
		}
	}

	/// <summary>Handles the Click event of the Open World Map button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method opens a world map dialog for selecting the observer's location. If the user confirms the selection, the latitude and longitude numeric up-down controls are updated accordingly.</remarks>
	private void KryptonButtonOpenWorldMap_Click(object sender, EventArgs e)
	{
		// Log the button click event for debugging purposes
		logger.Info(message: "Open World Map button clicked; opening world map dialog for observer location selection.");
		// Create and show the world map form as a modal dialog
		using WorldMapForm worldMapForm = new();
		/*
		// Set the initial latitude and longitude in the world map form based on the current numeric up-down values
		worldMapForm.Latitude = numericUpDownLatitude.Value;
		worldMapForm.Longitude = numericUpDownLongitude.Value;
		*/
		// Show the world map form and wait for the user to select a location or cancel
		_ = worldMapForm.ShowDialog(owner: this);
		// If the user confirms the selection in the world map form, update the latitude and longitude numeric up-down controls
		if (worldMapForm.DialogResult == DialogResult.OK)
		{
			// Log the selected latitude and longitude for debugging purposes
			logger.Info(message: $"User selected location from world map: Latitude {worldMapForm.Latitude}, Longitude {worldMapForm.Longitude}");
			// Update the numeric up-down controls with the selected latitude and longitude values
			numericUpDownLatitude.Value = worldMapForm.Latitude;
			numericUpDownLongitude.Value = worldMapForm.Longitude;
		}
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemDownloadDe440 menu item, which initiates the download of the JPL DE 440 ephemeris data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method checks for network availability before proceeding with the download. If the network is available, it opens a <see cref="DownloaderForm"/> to download the JPL DE 440 data.</remarks>
	private void ToolStripMenuItemDownloadDe440_Click(object sender, EventArgs e)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: "Download DE440 menu item clicked; checking network availability for JPL DE 440 download.");
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Show an error message indicating that there is no internet connection
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			// Exit the method early since the download cannot proceed without an internet connection
			return;
		}
		// Create and display the database downloader form with the specified download URL
		using DownloaderForm downloaderForm = new(url: Settings.Default.systemJplDe440Url);
		// Set the TopMost property of the downloader form to match the main form's TopMost property
		downloaderForm.TopMost = TopMost;
		// Show the downloader form as a modal dialog and check the result
		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			// Log that the user canceled the download and no action was taken
			logger.Info(message: "User canceled the JPL DE 440 download; no action taken.");
			// Exit the method early since the user canceled the download
			return;
		}
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemDownloadDe441 menu item, which initiates the download of the JPL DE 441 ephemeris data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method checks for network availability before proceeding with the download. If the network is available, it opens a <see cref="DownloaderForm"/> to download the JPL DE 441 data.</remarks>
	private void ToolStripMenuItemDownloadDe441_Click(object sender, EventArgs e)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: "Download DE441 menu item clicked; checking network availability for JPL DE 441 download.");
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Show an error message indicating that there is no internet connection
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			// Exit the method early since the download cannot proceed without an internet connection
			return;
		}
		// Create and display the database downloader form with the specified download URL
		using DownloaderForm downloaderForm = new(url: Settings.Default.systemJplDe441Url);
		// Set the TopMost property of the downloader form to match the main form's TopMost property
		downloaderForm.TopMost = TopMost;
		// Show the downloader form as a modal dialog and check the result
		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			// Log that the user canceled the download and no action was taken
			logger.Info(message: "User canceled the JPL DE 441 download; no action taken.");
			// Exit the method early since the user canceled the download
			return;
		}
	}

	/// <summary>Handles the Click event to open the JPL DE ephemeris download page.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void ToolStripMenuItemOpenJplDeWebPage_Click(object sender, EventArgs e)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: "Open JPL DE web page menu item clicked; opening the JPL DE ephemeris download page in the default web browser.");
		// Open the JPL DE ephemeris download page in the default web browser
		OpenWebsite(fileName: $"https://ssd.jpl.nasa.gov/ftp/eph/planets/Linux/");
	}

	#endregion
}
