/*
 * File:        EncounterVelocityOfAllMinorPlanetsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for displaying the encounter velocitys of all minor planets relative to each of the eight solar system planets.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Form for displaying the encounter velocitys of all minor planets relative to each of the eight solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their encounter velocitys with respect to all eight planets. Results are presented in a ListView where each row corresponds to one planetoid and the eight encounter velocitys columns correspond to Mercury through Neptune. The user can start and cancel the calculation at any time and track progress via the integrated progress bar.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class EncounterVelocityOfAllMinorPlanetsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Encounter velocitys of all minor planets";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "EncounterVelocityOfAllMinorPlanetsForm";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Number of planets whose Tisserand parameters are computed (Mercury through Neptune).</summary>
	/// <remarks>This constant matches the number of planets in <see cref="TisserandParameterCalculator"/>.</remarks>
	private const int PlanetCount = 8;

	/// <summary>Zero-based column index of the Planetoid name column.</summary>
	/// <remarks>Used for sorting comparisons against the specific column type.</remarks>
	private const int ColumnIndexPlanetoid = 0;

	/// <summary>Prefix length added to column headers to display sort indicators (e.g., "▲ " or "▼ ").</summary>
	/// <remarks>This is used to trim the sort indicator prefix when updating column headers.</remarks>
	private const int SortIndicatorPrefixLength = 2;

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>The complete list of Tisserand parameter results after the last completed calculation.</summary>
	/// <remarks>This list is only updated on the UI thread after the background calculation finishes.</remarks>
	private List<RowResult> _results = [];

	/// <summary>Cancellation token source for the running background calculation task.</summary>
	/// <remarks>Set to <c>null</c> when no calculation is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>The currently active sort column index, or -1 if no column sort is active.</summary>
	/// <remarks>Updated whenever the user clicks a column header to sort the list.</remarks>
	private int sortColumn = -1;

	/// <summary>The current sort order for the active sort column.</summary>
	/// <remarks>Defaults to <see cref="SortOrder.None"/>; toggles between Ascending and Descending on column clicks.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	private const double orbitalEartVelocityiInKm = 29.78498;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="EncounterVelocityOfAllMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public EncounterVelocityOfAllMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		// Log the initialization of the form with the count of planetoid records
		logger.Info(message: $"Initializing EncounterVelocityOfAllMinorPlanetsForm with {planetoids.Count} planetoid records.");
		InitializeComponent();
		// Cache the planetoid records for use during the calculation; this allows the calculation method to access the raw data without needing to pass it around or access it from a shared resource
		_planetoids = planetoids;
		// Set the initial state of the buttons and progress bar
		logger.Info(message: "EncounterVelocityOfAllMinorPlanetsForm initialized successfully.");
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is primarily intended for debugging purposes.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Updates the progress bar value and text label.</summary>
	/// <param name="percent">Progress value from 0 to 100.</param>
	/// <remarks>The percentage is displayed both in the progress bar's <c>Text</c> property and in the adjacent label.</remarks>
	private void UpdateProgress(int percent)
	{
		// Clamp the percentage value to ensure it stays within the valid range of 0 to 100
		int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
		kryptonProgressBar.Value = clampedPercent;
		kryptonProgressBar.Text = $"{clampedPercent}%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
	}

	/// <summary>Processes one raw MPCORB database record and appends the Tisserand parameter result row to <paramref name="results"/>.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="results">The list to which matching <see cref="RowResult"/> items are appended.</param>
	/// <remarks>Lines that are too short or have invalid orbital elements are silently skipped.</remarks>
	private static void ProcessPlanetoidLine(string line, List<RowResult> results)
	{
		// Validate the line length to ensure it contains the required orbital element fields
		if (line.Length < 103)
		{
			return;
		}
		// Extract and parse the semi-major axis (positions 92-102)
		string semiMajorAxisText = line.Substring(startIndex: 92, length: 11).Trim();
		if (!double.TryParse(s: semiMajorAxisText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis) || semiMajorAxis <= 0)
		{
			return;
		}
		// Extract and parse the eccentricity (positions 70-78)
		string eccentricityText = line.Substring(startIndex: 70, length: 9).Trim();
		if (!double.TryParse(s: eccentricityText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double eccentricity))
		{
			return;
		}
		// Extract and parse the inclination to the ecliptic (positions 59-67)
		string inclinationText = line.Substring(startIndex: 59, length: 9).Trim();
		if (!double.TryParse(s: inclinationText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double inclinationDeg))
		{
			return;
		}
		// Extract the designation (readable designation or packed designation)
		string designation = line.Length >= 194
			? line.Substring(startIndex: 166, length: 28).Trim()
			: line[..7].Trim();
		if (string.IsNullOrEmpty(value: designation))
		{
			designation = line[..7].Trim();
		}
		// Validate the orbital elements before calculation
		if (eccentricity is < 0.0 or >= 1.0)
		{
			logger.Warn(message: $"Invalid eccentricity {eccentricity} for planetoid {designation}. Skipping calculation.");
			ShowErrorMessage(message: "Eccentricity must be between 0 (inclusive) and 1 (exclusive) for valid Tisserand parameters.");
			return;
		}
		if (semiMajorAxis <= 0.0)
		{
			logger.Warn(message: $"Invalid semi-major axis {semiMajorAxis} for planetoid {designation}. Skipping calculation.");
			ShowErrorMessage(message: "Semi-major axis must be positive for valid Tisserand parameters.");
			return;
		}
		// Calculate the Tisserand parameters for all eight planets
		TisserandParameterCalculator.TisserandResult[] tisserandResults = TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: semiMajorAxis, eccentricity: eccentricity, inclinationDeg: inclinationDeg);
		// Build the Tisserand parameter array in planet order (Mercury … Neptune)
		double[] tisserandValues = new double[PlanetCount];
		// Loop through the calculated Tisserand results and populate the tisserandValues array
		for (int i = 0; i < Math.Min(val1: tisserandResults.Length, val2: PlanetCount); i++)
		{
			// Use the planet index to determine which planet's encounter velocity to calculate
			switch (i)
			{
				// Calculate the encounter velocity relative to the specific planet and convert it to km/s
				case 0: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToMercury(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Mercury
				case 1: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToVenus(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Venus
				case 2: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToEarth(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Earth
				case 3: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToMars(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Mars
				case 4: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToJupiter(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Jupiter
				case 5: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToSaturn(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Saturn
				case 6: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToUranus(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Uranus
				case 7: tisserandValues[i] = DerivedElements.CalculateEncounterVelocityRelativeToNeptune(semiMajorAxis: semiMajorAxis, numericalEccentricity: eccentricity, inclination: inclinationDeg) * orbitalEartVelocityiInKm; break; // Neptune
				default: break; // Should never happen, but included for completeness
			}
		}
		// Append the calculated Tisserand parameter values to the results list
		results.Add(item: new RowResult(PlanetoidName: designation, Values: tisserandValues));
	}

	/// <summary>Sorts <see cref="_results"/> by the currently selected column and sort order.</summary>
	/// <remarks>Column 0 (Planetoid) is sorted as a string; all other columns (Tisserand parameter values) are sorted numerically.</remarks>
	private void SortResults()
	{
		// If no column is selected for sorting, exit early
		int col = sortColumn;
		// Determine the sort order (ascending or descending) based on the current sortOrder value
		bool ascending = sortOrder == SortOrder.Ascending;
		// Sort the results based on the selected column and order
		if (col == ColumnIndexPlanetoid)
		{
			// Sort by Planetoid name (string comparison, case-insensitive)
			_results = ascending
				? [.. _results.OrderBy(keySelector: static r => r.PlanetoidName, comparer: StringComparer.OrdinalIgnoreCase)]
				: [.. _results.OrderByDescending(keySelector: static r => r.PlanetoidName, comparer: StringComparer.OrdinalIgnoreCase)];
		}
		// Sort by Tisserand parameter values (numeric comparison)
		else if (col is >= 1 and <= PlanetCount)
		{
			// Adjust the column index to match the zero-based index of the Values array
			int planetIndex = col - 1;
			// Sort by the corresponding Tisserand parameter value for the selected planet
			_results = ascending
				? [.. _results.OrderBy(keySelector: r => r.Values[planetIndex])]
				: [.. _results.OrderByDescending(keySelector: r => r.Values[planetIndex])];
		}
	}

	/// <summary>Selects the currently highlighted planetoid in the main form and navigates to its record in the owner form, if applicable.</summary>
	/// <remarks>If the owner form is a PlanetoidDbForm, this method calls its JumpToRecord method to display the selected planetoid. No action is taken if no item is selected or if the selection is invalid.</remarks>
	private void SelectedPlanetoidInMainForm()
	{
		// Ensure that an item is selected
		if (listView.SelectedIndices.Count == 0)
		{
			// Log a warning if no item is selected and exit the method
			logger.Warn(message: "No item selected in the ListView.");
			return;
		}
		// Get the index of the selected item in the ListView
		int index = listView.SelectedIndices[0];
		// Validate the index to ensure it is within the bounds of the results list
		if (index < 0 || index >= _results.Count)
		{
			// Log a warning if the selected index is out of bounds and exit the method
			logger.Warn(message: $"Selected index {index} is out of bounds for results list of size {_results.Count}. No action taken.");
			return;
		}
		// Retrieve the corresponding RowResult for the selected index
		RowResult result = _results[index: index];
		// If the Owner of this form is a PlanetoidDbForm, call its JumpToRecord method
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			// Log the navigation action to the main form for debugging purposes
			logger.Info(message: $"Navigating to planetoid {result.PlanetoidName} in the main form.");
			// Call the JumpToRecord method of the PlanetoidDbForm to display the selected planetoid
			planetoidDbForm.JumpToRecord(index: result.PlanetoidName, designation: result.PlanetoidName);
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the form Load event. Clears the status bar on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>Clears the status bar when the form is loaded.</remarks>
	private void EncounterVelocityOfAllMinorPlanetsForm_Load(object sender, EventArgs e)
	{
		// Log that the form has been loaded and clear the status bar
		logger.Info(message: "EncounterVelocityOfAllMinorPlanetsForm loaded.");
		// Clear the status bar to ensure no residual messages are displayed from previous operations
		ClearStatusBar(label: labelInformation);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
	}

	/// <summary>Handles the FormClosing event. Cancels any running calculation and disposes the cancellation token source.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>Cancels any running calculation and disposes the cancellation token source when the form is closing.</remarks>
	private void EncounterVelocityOfAllMinorPlanetsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing and check for any running calculations
		logger.Info(message: "EncounterVelocityOfAllMinorPlanetsForm is closing.");
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
		// If a calculation is currently running, signal cancellation and dispose of the token source
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}
	}

	#endregion

	#region RetrieveVirtualItem event handler

	/// <summary>Handles the RetrieveVirtualItem event for the VirtualMode ListView. Provides the <see cref="ListViewItem"/> for the requested index from <see cref="_results"/>.</summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> containing the requested item index.</param>
	/// <remarks>Called by the ListView for each visible row. Must be fast and must not modify <see cref="_results"/>.</remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Validate the requested index to ensure it is within the bounds of the results list
		if (e.ItemIndex < 0 || e.ItemIndex >= _results.Count)
		{
			// If the index is out of bounds, provide an empty ListViewItem to avoid exceptions
			e.Item = new ListViewItem();
			// Log a warning to indicate that an out-of-bounds index was requested
			logger.Warn(message: $"RetrieveVirtualItem called with out-of-bounds index {e.ItemIndex}. Returning empty ListViewItem.");
			return;
		}
		// Retrieve the corresponding RowResult for the requested index
		RowResult result = _results[e.ItemIndex];
		// Create a new ListViewItem with the planetoid name as the main text
		ListViewItem item = new(text: result.PlanetoidName);
		// Create an array of subitems for the encounter velocity values, formatted to six decimal places
		string[] subItems = new string[PlanetCount];
		// Loop through each planet's encounter velocity and format it for display
		for (int i = 0; i < PlanetCount; i++)
		{
			// Format the encounter velocity value to six decimal places using the current culture for consistency
			subItems[i] = result.Values[i].ToString(format: "F6", provider: CultureInfo.CurrentCulture);
		}
		// Add the formatted subitems to the ListViewItem
		item.SubItems.AddRange(items: subItems);
		// Assign the constructed ListViewItem to the event args to be displayed in the ListView
		e.Item = item;
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles the sort order for the clicked column (ascending/descending) and re-sorts the results list. Column headers are updated with a ▲ or ▼ indicator to show the current sort direction.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		logger.Info(message: $"Column {e.Column} clicked. Toggling sort order.");
		// If there are no results to sort, exit the method early
		if (_results.Count == 0)
		{
			return;
		}
		// Check if the clicked column is the same as the current sort column; if so, toggle the sort order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to reflect the current sort column and direction
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.Ordinal) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.Ordinal))
			{
				headerText = headerText[SortIndicatorPrefixLength..];
			}
			listView.Columns[index: i].Text = i == sortColumn ? $"{(sortOrder == SortOrder.Ascending ? "▲" : "▼")} {headerText}" : headerText;
		}
		// Re-sort the results and refresh the ListView
		SortResults();
		listView.Refresh();
		// Log the new sort order for debugging purposes
		logger.Info(message: $"Results sorted by column {sortColumn} in {(sortOrder == SortOrder.Ascending ? "ascending" : "descending")} order.");
	}

	/// <summary>Handles the DoubleClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item is double-clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListView_DoubleClick(object sender, EventArgs e)
	{
		// Log the double-click event and navigate to the selected planetoid in the main form
		logger.Info(message: "ListView item double-clicked. Navigating to selected planetoid in main form.");
		SelectedPlanetoidInMainForm();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start Calculation button. Validates the input, then starts the encounter velocity calculation for all minor planets asynchronously.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The calculation runs on a background thread. Progress is reported via the progress bar. The user can cancel at any time using the Cancel button.</remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// Log the start of the calculation and check if there are any planetoid records to process
		logger.Info(message: "Start button clicked. Beginning encounter velocity calculation.");
		if (_planetoids.Count == 0)
		{
			// Log a warning if there are no planetoid records and show an informational message box to the user
			logger.Warn(message: "No planetoid records available for calculation.");
			ShowWarnMessage(caption: I18nStrings.InformationCaption, message: "No planetoid data available.");
			return;
		}
		// Disable the Start button and save menu during the calculation
		toolStripDropDownButtonSaveToFile.Enabled = false;
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripButtonGoToObject.Enabled = false;
		_results = [];
		listView.VirtualListSize = 0;
		UpdateProgress(percent: 0);
		ClearStatusBar(label: labelInformation);
		// Create a new cancellation token source for the calculation task
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		// Create a local list to store results from the background calculation
		List<RowResult> localResults = [];
		// Create a progress reporter that updates the progress bar on the UI thread
		IProgress<int> progress = new Progress<int>(handler: UpdateProgress);
		try
		{
			// Calculate the total number of planetoids and determine the interval at which to report progress
			int total = _planetoids.Count;
			int reportInterval = Math.Max(val1: 1, val2: total / 100);
			// Run the calculation on a background thread
			await Task.Run(action: () =>
			{
				// Loop through each planetoid record and process it
				for (int i = 0; i < total; i++)
				{
					// Check for cancellation requests and throw if cancellation has been requested
					token.ThrowIfCancellationRequested();
					// Process the current planetoid line and append the result to the local results list
					ProcessPlanetoidLine(line: _planetoids[index: i], results: localResults);
					if (i % reportInterval == 0 || i == total - 1)
					{
						progress.Report(value: (i + 1) * 100 / total);
					}
				}
				// Log the completion of the calculation and the total number of results obtained
				logger.Info(message: $"Encounter velocity calculation completed. Total results: {localResults.Count}");
			}, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
		}
		// Catch the OperationCanceledException to handle user cancellation gracefully
		catch (OperationCanceledException)
		{
			logger.Warn(message: "Encounter velocity calculation cancelled by user.");
		}
		// Catch any other exceptions that may occur during the calculation
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error during calculation: {ex.Message}");
		}
		// Finally block to clean up resources and reset UI elements after the calculation completes or is cancelled
		finally
		{
			// Update the UI with the results if the form is still active and not disposed
			try
			{
				// Only update the UI if the form is still active and not disposed
				if (IsHandleCreated && !IsDisposed && !Disposing)
				{
					// Assign the local results to the main results list and update the ListView
					_results = localResults;
					listView.VirtualListSize = _results.Count;
					listView.Refresh();
					toolStripButtonStart.Enabled = true;
					toolStripButtonCancel.Enabled = false;
					toolStripButtonGoToObject.Enabled = _results.Count > 0;
					toolStripDropDownButtonSaveToFile.Enabled = _results.Count > 0;
				}
			}
			// Catch ObjectDisposedException and InvalidOperationException that may occur if the form is closing
			catch (ObjectDisposedException)
			{
				// Log a warning if the form was disposed before the calculation could complete
				logger.Warn(message: "Form disposed before calculation could complete. Skipping UI update.");
			}
			catch (InvalidOperationException)
			{
				// Log a warning if an invalid operation occurred during form shutdown
				logger.Warn(message: "Invalid operation during form shutdown. Skipping UI update.");
			}
			// Dispose of the cancellation token source to free resources
			finally
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
			}
		}
	}

	/// <summary>Handles the Click event of the Cancel button. Cancels the currently running calculation.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The calculation can be cancelled by the user at any time using the Cancel button.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// If a calculation is currently running, request cancellation and prevent repeated cancel clicks.
		if (_cancellationTokenSource != null)
		{
			logger.Info(message: "Cancel button clicked. Requesting cancellation of Encounter velocity calculation.");
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Handles the Click event of the "Go to Object" button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When the "Go to Object" button is clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
	{
		// Log the action of navigating to the selected planetoid in the main form
		logger.Info(message: "Go to Object button clicked. Navigating to selected planetoid in main form.");
		SelectedPlanetoidInMainForm();
		Close();
	}

	#endregion
}
