/*
 * File:        MoidsOfAllMinorPlanetsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for displaying the Minimum Orbit Intersection Distance (MOID) of all minor planets relative to each of the eight solar system planets.
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

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for displaying the Minimum Orbit Intersection Distance (MOID) of all minor planets relative to each of the eight solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their MOIDs with respect to all eight planets. Results are presented in a ListView where each row corresponds to one planetoid and the eight MOID columns correspond to Mercury through Neptune. The user can start and cancel the calculation at any time and track progress via the integrated progress bar.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class MoidsOfAllMinorPlanetsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "MOIDs of all minor planets";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "MoidsOfAllMinorPlanets";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents one row in the MOID results list: the planetoid name and one MOID value per planet.</summary>
	/// <param name="PlanetoidName">The designation of the minor planet.</param>
	/// <param name="Moids">Array of eight MOID values in AU, one per planet in order Mercury–Neptune.</param>
	/// <remarks>The <paramref name="Moids"/> array always has exactly eight elements corresponding to the eight solar system planets: Mercury (0), Venus (1), Earth (2), Mars (3), Jupiter (4), Saturn (5), Uranus (6), Neptune (7).</remarks>
	private readonly record struct MoidRowResult(string PlanetoidName, double[] Moids);

	/// <summary>Number of planets whose MOIDs are computed (Mercury through Neptune).</summary>
	/// <remarks>This constant matches the number of planets in <see cref="MoidCalculator"/>.</remarks>
	private const int PlanetCount = 8;

	/// <summary>Zero-based column index of the Planetoid name column.</summary>
	/// <remarks>Used for sorting comparisons against the specific column type.</remarks>
	private const int ColumnIndexPlanetoid = 0;

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>The complete list of MOID results after the last completed calculation.</summary>
	/// <remarks>This list is only updated on the UI thread after the background calculation finishes.</remarks>
	private List<MoidRowResult> _results = [];

	/// <summary>Array of original column headers for the ListView.</summary>
	/// <remarks>Used to restore column headers when toggling sort arrows.</remarks>
	private readonly string[] _originalColumnHeaders;

	/// <summary>Cancellation token source for the running background calculation task.</summary>
	/// <remarks>Set to <c>null</c> when no calculation is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>The currently active sort column index, or -1 if no column sort is active.</summary>
	/// <remarks>Updated whenever the user clicks a column header to sort the list.</remarks>
	private int sortColumn = -1;

	/// <summary>The current sort order for the active sort column.</summary>
	/// <remarks>Defaults to <see cref="SortOrder.None"/>; toggles between Ascending and Descending on column clicks.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MoidsOfAllMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public MoidsOfAllMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		// Initialize the form components and controls
		InitializeComponent();
		// Store the planetoid data for processing
		_planetoids = planetoids;
		// Cache original header titles to safely toggle sort arrows
		_originalColumnHeaders = new string[listView.Columns.Count];
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			_originalColumnHeaders[i] = listView.Columns[index: i].Text;
		}
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is primarily intended for debugging purposes.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Selects the currently highlighted planetoid in the main form and navigates to its record in the owner form, if applicable.</summary>
	/// <returns><see langword="true"/> if navigation was performed; otherwise, <see langword="false"/>.</returns>
	/// <remarks>If the owner form is a PlanetoidDbForm, this method calls its JumpToRecord method to display the selected planetoid. No action is taken if no item is selected or if the selection is invalid.</remarks>
	private bool SelectedPlanetoidInMainForm()
	{
		// Ensure that an item is selected
		if (listView.SelectedIndices.Count == 0)
		{
			logger.Warn(message: "No planetoid selected in the list view.");
			return false;
		}
		// Get the index of the selected item
		int index = listView.SelectedIndices[index: 0];
		// Validate the index against the results list
		if (index < 0 || index >= _results.Count)
		{
			logger.Warn(message: $"Selected index {index} is out of bounds for results count {_results.Count}.");
			return false;
		}
		// Retrieve the corresponding MoidRowResult for the selected index
		MoidRowResult result = _results[index];
		// If the Owner of this form is a PlanetoidDbForm, call its JumpToRecord method
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			logger.Info(message: $"Navigating to planetoid {result.PlanetoidName} in the main form.");
			planetoidDbForm.JumpToRecord(index: result.PlanetoidName, designation: result.PlanetoidName);
			return true;
		}
		// If the owner is not a PlanetoidDbForm, no action is taken
		return false;
	}

	/// <summary>Updates the enabled state of the "Go to object" button.</summary>
	/// <remarks>The button is enabled only when a result row is selected.</remarks>
	private void UpdateGoToObjectButtonState() => toolStripButtonGoToObject.Enabled = listView.SelectedIndices.Count > 0;

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

	/// <summary>Parses a raw MPCORB line using <see cref="ReadOnlySpan{T}"/> to minimize allocations.</summary>
	/// <param name="line">The raw MPCORB line to parse.</param>
	/// <param name="result">The parsed <see cref="MoidRowResult"/> if successful; otherwise, <c>default</c>.</param>
	/// <returns><c>true</c> if parsing was successful; otherwise, <c>false</c>.</returns>
	/// <remarks>This method extracts the necessary orbital elements from the fixed-width MPCORB line format and computes the MOIDs for all eight planets. It uses <see cref="ReadOnlySpan{T}"/> for slicing/parsing to reduce intermediate string allocations, but still allocates the MOID array and the final designation string.</remarks>
	private static bool TryProcessPlanetoidLine(ReadOnlySpan<char> line, out MoidRowResult result)
	{
		// Initialize the out parameter to default in case of early return
		result = default;
		// Ensure the line is long enough to contain all required fields
		if (line.Length < 103)
		{
			logger.Warn(message: $"Line is too short to parse: {line}");
			return false;
		}
		// Extract and parse semi-major axis (positions 92-102)
		if (!double.TryParse(line.Slice(start: 92, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis) || semiMajorAxis <= 0)
		{
			logger.Warn(message: $"Invalid semi-major axis in line: {line}");
			return false;
		}
		// Extract and parse eccentricity (positions 70-78)
		if (!double.TryParse(line.Slice(start: 70, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double eccentricity))
		{
			logger.Warn(message: $"Invalid eccentricity in line: {line}");
			return false;
		}
		// Extract and parse inclination (positions 59-67)
		if (!double.TryParse(line.Slice(start: 59, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double inclinationDeg))
		{
			logger.Warn(message: $"Invalid inclination in line: {line}");
			return false;
		}
		// Extract and parse longitude of ascending node (positions 48-56)
		if (!double.TryParse(line.Slice(start: 48, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longitudeAscendingNodeDeg))
		{
			logger.Warn(message: $"Invalid longitude of ascending node in line: {line}");
			return false;
		}
		// Extract and parse argument of perihelion (positions 37-45)
		if (!double.TryParse(line.Slice(start: 37, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argumentPerihelionDeg))
		{
			logger.Warn(message: $"Invalid argument of perihelion in line: {line}");
			return false;
		}
		// Extract designation span
		ReadOnlySpan<char> designationSpan = line.Length >= 194 ? line.Slice(start: 166, length: 28).Trim() : line[..7].Trim();
		// Fallback to first 7 characters if the line is too short for the full designation
		if (designationSpan.IsEmpty)
		{
			logger.Warn(message: $"Designation span is empty in line: {line}");
			designationSpan = line[..7].Trim();
		}
		// Compute MOIDs for all eight planets using the extracted orbital elements
		double[] moids = MoidCalculator.CalculateMoidsInPlanetOrder(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Create the result record with the planetoid designation and computed MOIDs
		result = new MoidRowResult(designationSpan.ToString(), moids);
		// Indicate successful parsing and computation
		return true;
	}

	/// <summary>Sorts <see cref="_results"/> by the currently selected column and sort order.</summary>
	/// <remarks>Column 0 (Planetoid) is sorted as a string; all other columns (MOID values) are sorted numerically.</remarks>
	private void SortResults()
	{
		// If no sort column is selected, do not perform any sorting
		int col = sortColumn;
		// Determine if the sort order is ascending or descending
		bool ascending = sortOrder == SortOrder.Ascending;
		// Sort based on the selected column
		if (col == ColumnIndexPlanetoid)
		{
			// Sort by Planetoid name (string comparison, case-insensitive)
			_results.Sort(comparison: (x, y) => ascending
				? string.Compare(strA: x.PlanetoidName, strB: y.PlanetoidName, comparisonType: StringComparison.OrdinalIgnoreCase)
				: string.Compare(strA: y.PlanetoidName, strB: x.PlanetoidName, comparisonType: StringComparison.OrdinalIgnoreCase));
		}
		// Sort by MOID values for the selected planet column (numeric comparison)
		else if (col is >= 1 and <= PlanetCount)
		{
			// Adjust for zero-based index in the Moids array
			int planetIndex = col - 1;
			// Sort by MOID value for the selected planet
			_results.Sort(comparison: (x, y) => ascending
				? x.Moids[planetIndex].CompareTo(value: y.Moids[planetIndex])
				: y.Moids[planetIndex].CompareTo(value: x.Moids[planetIndex]));
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event. Clears the status bar on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>Clears the status bar when the form is loaded.</remarks>
	private void MoidsOfAllMinorPlanetsForm_Load(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

	/// <summary>Handles the FormClosing event. Cancels any running calculation.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>Cancels any running calculation when the form is closing.</remarks>
	private void MoidsOfAllMinorPlanetsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		logger.Info(message: "Form is closing. Cancelling any running calculation.");
		_cancellationTokenSource?.Cancel();
	}

	#endregion

	#region RetrieveVirtualItem event handler

	/// <summary>Handles the RetrieveVirtualItem event for the VirtualMode ListView. Provides the <see cref="ListViewItem"/> for the requested index from <see cref="_results"/>.</summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> containing the requested item index.</param>
	/// <remarks>Called by the ListView for each visible row. Must be fast and must not modify <see cref="_results"/>.</remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Validate the requested index against the results list
		if (e.ItemIndex < 0 || e.ItemIndex >= _results.Count)
		{
			// If the index is out of bounds, provide an empty ListViewItem to avoid exceptions
			logger.Warn(message: $"Requested index {e.ItemIndex} is out of bounds for results count {_results.Count}.");
			e.Item = new ListViewItem();
			return;
		}
		// Retrieve the corresponding MoidRowResult for the requested index
		MoidRowResult result = _results[e.ItemIndex];
		// Create a new ListViewItem with the planetoid name as the first column
		ListViewItem item = new(text: result.PlanetoidName);
		// Add the MOID values for each planet as subitems
		for (int i = 0; i < PlanetCount; i++)
		{
			item.SubItems.Add(text: result.Moids[i].ToString(provider: CultureInfo.InvariantCulture));
		}
		// Assign the constructed ListViewItem to the event args
		e.Item = item;
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start Calculation button. Validates the input, then starts the MOID calculation for all minor planets asynchronously.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The calculation runs on a background thread. Progress is reported via the progress bar. The user can cancel at any time using the Cancel button.</remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// Check if there are any planetoids to process
		if (_planetoids.Count == 0)
		{
			// Log and show an informational message box if no planetoid data is available
			logger.Warn(message: "No planetoid data available.");
			_ = KryptonMessageBox.Show(owner: this, text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Disable the Start button and enable the Cancel button to prevent multiple concurrent calculations
		toolStripDropDownButtonSaveToFile.Enabled = false;
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripButtonGoToObject.Enabled = false;
		listView.Enabled = false;
		// Clear previous results and reset the ListView
		_results = new List<MoidRowResult>(_planetoids.Count);
		listView.VirtualListSize = 0;
		// Reset the progress bar and status label
		UpdateProgress(percent: 0);
		ClearStatusBar(label: labelInformation);
		// Create a new cancellation token source for this calculation
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		// Create a progress reporter that updates the progress bar and status label
		IProgress<int> progress = new Progress<int>(handler: UpdateProgress);
		// Start the MOID calculation in a background task
		try
		{
			// Calculate the total number of planetoids and determine the reporting interval for progress updates
			int total = _planetoids.Count;
			int reportInterval = Math.Max(val1: 1, val2: total / 100);
			int processedCount = 0;
			// Run computation parallelized across CPU cores
			List<MoidRowResult> localResults = await Task.Run(() =>
			{
				// Use a thread-safe list to collect results from parallel tasks
				List<MoidRowResult> threadSafeResults = new(capacity: total);
				// Use Parallel.For to process each planetoid line in parallel
				_ = Parallel.For(
					// Define the range of indices to process
					fromInclusive: 0,
					toExclusive: total,
					// Define parallel options with cancellation support
					parallelOptions: new ParallelOptions { CancellationToken = token },
					// Initialize a local list for each thread to avoid contention
					localInit: () => new List<MoidRowResult>(),
					// Define the body of the parallel loop
					body: (i, loopState, localList) =>
					{
						// Process the planetoid line and compute MOIDs
						if (TryProcessPlanetoidLine(line: _planetoids[index: i].AsSpan(), result: out MoidRowResult result))
						{
							localList.Add(item: result);
						}
						// Increment the processed count and report progress at defined intervals
						int current = Interlocked.Increment(location: ref processedCount);
						if (current % reportInterval == 0 || current == total)
						{
							progress.Report(value: current * 100 / total);
						}
						// Return the local list of results for this thread
						return localList;
					},
					// Finalize the local list by adding it to the thread-safe results
					localFinally: localList =>
					{
						// Lock the shared results list to safely add the local results from this thread
						lock (threadSafeResults)
						{
							threadSafeResults.AddRange(collection: localList);
						}
					});
				// Log the completion of the MOID calculation with the total number of results
				logger.Info(message: $"MOID calculation completed. Total results: {threadSafeResults.Count}");
				// Return the aggregated results from all threads
				return threadSafeResults;
			}, token);
			// Update the main results list on the UI thread after successful completion
			_results = localResults;
		}
		// Handle cancellation gracefully
		catch (OperationCanceledException ex)
		{
			logger.Info(exception: ex, message: "MOID calculation cancelled by user.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error during calculation: {ex.Message}");
		}
		// Ensure that the UI is updated regardless of success, cancellation, or error
		finally
		{
			// Update the ListView and buttons on the UI thread after the calculation task completes
			try
			{
				// Only update the ListView if the form is still valid and not disposed
				if (IsHandleCreated && !IsDisposed && !Disposing)
				{
					// Set the virtual list size to the number of results and refresh the ListView
					listView.VirtualListSize = _results.Count;
					listView.Refresh();
					// Re-enable the ListView and buttons after the calculation is complete
					listView.Enabled = true;
					toolStripButtonStart.Enabled = true;
					toolStripButtonCancel.Enabled = false;
					// Update the "Go to object" button state based on the current selection
					UpdateGoToObjectButtonState();
					// Enable the "Save to file" button only if there are results to save
					toolStripDropDownButtonSaveToFile.Enabled = _results.Count > 0;
				}
			}
			// Catch specific exceptions that may occur if the form is closing or disposed during the update
			catch (Exception ex) when (ex is ObjectDisposedException or InvalidOperationException)
			{
				logger.Warn(exception: ex, message: "Exception occurred while updating the UI during form closing.");
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
			logger.Info(message: "User requested cancellation of the MOID calculation.");
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Handles the Click event of the "Go to Object" button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When the "Go to Object" button is clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/> and this form is closed.</remarks>
	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
	{
		// Attempt to select the currently highlighted planetoid in the main form
		if (SelectedPlanetoidInMainForm())
		{
			logger.Info(message: "Navigated to selected planetoid in main form. Closing MOID form.");
			// If successful, close this form to return focus to the main form
			Close();
		}
	}

	#endregion

	#region ColumnClick event handler

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles the sort order for the clicked column (ascending/descending) and re-sorts the results list. Column headers are updated with a ▲ or ▼ indicator to show the current sort direction.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no results, do not attempt to sort
		if (_results.Count == 0)
		{
			logger.Warn(message: "Column click ignored because there are no results to sort.");
			return;
		}
		// Clear selection state so index mismatch doesn't occur after sorting
		listView.SelectedIndices.Clear();
		// If the clicked column is the same as the current sort column, toggle the sort order; otherwise, set the new sort column and default to ascending order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to reflect the current sort order with arrows
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string baseHeader = _originalColumnHeaders[i];
			listView.Columns[i].Text = i == sortColumn
				? $"{(sortOrder == SortOrder.Ascending ? "▲" : "▼")} {baseHeader}"
				: baseHeader;
		}
		// Perform the sorting of the results based on the selected column and order
		SortResults();
		listView.Refresh();
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Enables the "Go to object" button when a row is selected.</remarks>
	private void ListView_SelectedIndexChanged(object sender, EventArgs e) => UpdateGoToObjectButtonState();

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item is double-clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListView_DoubleClick(object sender, EventArgs e)
	{
		logger.Info(message: "ListView item double-clicked. Navigating to selected planetoid in main form.");
		SelectedPlanetoidInMainForm();
	}

	#endregion
}
