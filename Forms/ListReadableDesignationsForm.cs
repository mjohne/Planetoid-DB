/*
 * File:        ListReadableDesignationsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form to list readable designations from the planetoids database.
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

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form to list readable designations from the planetoids database.</summary>
/// <remarks>This form is used to display a list of all readable designations from the planetoids database.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ListReadableDesignationsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of readable designations";

	/// <summary>Prepares the save dialog used for export operations.</summary>
	/// <param name="dialog">The dialog to configure before it is displayed.</param>
	/// <param name="ext">The file extension selected for the export.</param>
	/// <returns><see langword="true"/> if the user confirms the dialog; otherwise, <see langword="false"/>.</returns>
	/// <remarks>Overrides the default file naming to preserve the selected minimum and maximum range in the export file name.</remarks>
	protected override bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"Readable-Designation-List_{toolStripNumericUpDownMinimum.Value}-{toolStripNumericUpDownMaximum.Value}.{ext}";
		return dialog.ShowDialog(owner: this) == DialogResult.OK;
	}

	#endregion

	#region Constants

	/// <summary>Length of the index field in the planetoid record.</summary>
	/// <remarks>This constant defines the length of the index field in the planetoid record.</remarks>
	private const int indexLength = 7;

	/// <summary>Length of the name field in the planetoid record.</summary>
	/// <remarks>This constant defines the starting index of the name field in the planetoid record.</remarks>
	private const int nameStartIndex = 166;

	/// <summary>Length of the name field in the planetoid record.</summary>
	/// <remarks>This constant defines the length of the name field in the planetoid record.</remarks>
	private const int nameLength = 28;

	#endregion

	/// <summary>Offset for virtual mode to calculate the starting index in the database</summary>
	/// <remarks>This field is used to calculate the starting index in the database for virtual mode.</remarks>
	private int virtualListOffset = 0;

	/// <summary>List of planetoid records from the database</summary>
	/// <remarks>This list contains all the planetoid records retrieved from the database.</remarks>
	private IReadOnlyList<string> planetoidsDatabase = [];

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the <see cref="ListReadableDesignationsForm"/> class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>This field stores the index of the currently sorted column.</remarks>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered:
	/// <list type="bullet">
	/// <item><description><see cref="SortOrder.None"/>: No sorting is applied.</description></item>
	/// <item><description><see cref="SortOrder.Ascending"/>: Items are sorted in ascending order.</description></item>
	/// <item><description><see cref="SortOrder.Descending"/>: Items are sorted in descending order.</description></item>
	/// </list>
	/// This field is typically updated when the user clicks a column header in the list view to toggle the sort order.</summary>
	/// <remarks>This field stores the current sort order of the list view.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Stores the sorted indices for virtual mode to maintain sorting order.</summary>
	/// <remarks>This list maps the virtual list view indices to the actual database indices based on the current sort criteria.</remarks>
	private List<int>? sortedIndices;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="ListReadableDesignationsForm"/> class.</summary>
	/// <remarks>This constructor initializes the form and its components.</remarks>
	public ListReadableDesignationsForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a short string representation of the current instance for debugging purposes.</remarks>
	private string GetDebuggerDisplay() => ToString();


	/// <summary>Gets the selected index in the list view.</summary>
	/// <returns>The selected index if an item is selected; otherwise, -1.</returns>
	/// <remarks>This method is used to get the selected index in the list view.</remarks>
	public int GetSelectedIndex() => listView.SelectedIndices.Count > 0 ? listView.SelectedIndices[index: 0] : -1;

	/// <summary>Attempts to parse a planetoid record string into its index and designation components.</summary>
	/// <param name="record">The raw database record to parse.</param>
	/// <param name="parsedIndex">When this method returns <c>true</c>, contains the parsed index value.</param>
	/// <param name="parsedDesignation">When this method returns <c>true</c>, contains the parsed designation value.</param>
	/// <returns><c>true</c> if the record was successfully parsed; otherwise, <c>false</c>.</returns>
	/// <remarks>This method is used to extract the index and designation from a fixed-width planetoid record string. It validates the input and uses <see cref="ReadOnlySpan{T}"/> for efficient substring extraction without allocations.</remarks>
	private static bool TryParsePlanetoidRecord(string record, out string parsedIndex, out string parsedDesignation)
	{
		// Initialize output parameters
		parsedIndex = string.Empty;
		parsedDesignation = string.Empty;
		// Validate the input record
		if (string.IsNullOrWhiteSpace(value: record) || record.Length < nameStartIndex + nameLength)
		{
			// Log a warning and return false if the record is null, empty, or too short
			logger.Warn(message: $"The record is null, empty, or too short. Record length: {record?.Length ?? 0}");
			return false;
		}
		// Use ReadOnlySpan<char> for efficient substring extraction without allocations
		ReadOnlySpan<char> span = record.AsSpan();
		// Extract the index and designation from the fixed-width record
		parsedIndex = span[..indexLength].Trim().ToString();
		parsedDesignation = span.Slice(start: nameStartIndex, length: nameLength).Trim().ToString();
		// Log the parsed values for debugging purposes
		return true;
	}

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event. Updates the status bar with the selected planetoid's index and readable designation, enables the Go to object button if necessary and stores the currently selected index.</summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the SelectedIndexChanged event of the ListView.</remarks>
	private void SelectedIndexChanged(object? sender, EventArgs? e)
	{
		// If no item is selected, clear the status bar and disable the Go to object button
		int? dbIndex = GetSelectedDatabaseIndex();
		// If a valid index is selected, parse the record and update the status bar
		if (dbIndex.HasValue && dbIndex.Value >= 0 && dbIndex.Value < planetoidsDatabase.Count)
		{
			// Use the shared parsing logic to extract index and designation
			if (TryParsePlanetoidRecord(record: planetoidsDatabase[index: dbIndex.Value], parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
			{
				// Update the status bar with the selected planetoid's index and designation
				SetStatusBar(labelInformation, $"{I18nStrings.Index}: {strIndex} - {strDesignation}");
				// Enable the Go to object button
				toolStripButtonGoToObject.Enabled = true;
				// Log the selection change
				return;
			}
		}
		// If no valid selection, clear the status bar and disable the Go to object button
		SetStatusBar(labelInformation, string.Empty);
		toolStripButtonGoToObject.Enabled = false;
	}

	/// <summary>Gets the currently selected database index, considering virtual mode and sorting.</summary>
	/// <returns>The actual database index corresponding to the currently selected item in the ListView, or <c>null</c> if no item is selected.</returns>
	/// <remarks>This method returns the actual database index corresponding to the currently selected item in the ListView, taking into account any sorting and virtual list offset.</remarks>
	private int? GetSelectedDatabaseIndex()
	{
		// If no item is selected, log a warning and return null
		if (listView.SelectedIndices.Count == 0)
		{
			logger.Warn(message: "No item is selected in the list view when attempting to get the selected database index.");
			return null;
		}
		// Get the virtual index of the selected item
		int virtualIdx = listView.SelectedIndices[index: 0];
		// Calculate the actual database index based on sorting and virtual list offset
		return sortedIndices != null && virtualIdx < sortedIndices.Count ? sortedIndices[index: virtualIdx] : virtualListOffset + virtualIdx;
	}

	/// <summary>Restores the selection in the ListView based on the provided database index.</summary>
	/// <param name="selectedDbIndex">The database index of the item to select.</param>
	/// <remarks>This method restores the selection in the ListView based on the provided database index, taking into account any sorting and virtual list offset.</remarks>
	private void RestoreSelection(int? selectedDbIndex)
	{
		// If no database index is provided, log a warning and return
		if (!selectedDbIndex.HasValue)
		{
			logger.Warn(message: "No database index provided to restore selection.");
			return;
		}
		// Calculate the new virtual index based on sorting and virtual list offset
		int newVirtualIndex = sortedIndices != null ? sortedIndices.IndexOf(item: selectedDbIndex.Value) : selectedDbIndex.Value - virtualListOffset;
		// If the new virtual index is valid, select it and ensure it is visible
		if (newVirtualIndex >= 0 && newVirtualIndex < listView.VirtualListSize)
		{
			// Log the restoration of selection
			listView.SelectedIndices.Clear();
			// Add the new virtual index to the selected indices
			_ = listView.SelectedIndices.Add(itemIndex: newVirtualIndex);
			// Ensure the newly selected item is visible in the ListView
			listView.EnsureVisible(index: newVirtualIndex);
		}
	}

	/// <summary>Updates the column headers of the ListView to reflect the current sort order. Adds an ascending or descending indicator to the sorted column header and removes any indicators from other columns.</summary>
	/// <remarks>This method is called after sorting to visually indicate which column is currently sorted and in which order.</remarks>
	private void UpdateColumnHeaders()
	{
		// Iterate through all columns in the ListView
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Remove any existing sort indicators from the header text
			string headerText = listView.Columns[index: i].Text.TrimStart('▲', '▼', ' ');
			// If this is the currently sorted column, add the appropriate sort indicator
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// For other columns, just update the text without indicators
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
	}

	/// <summary>Sorts the virtual list based on the current sort column and order. This method updates the sortedIndices list to reflect the new order of items in the ListView, preserving the selection if possible.</summary>
	/// <remarks>This method is called when the user clicks a column header to sort the list. It uses a custom comparison to sort the indices based on the selected column and order, and then restores the selection based on the previously selected database index.</remarks>
	private void SortVirtualList()
	{
		// If there are no items, do not attempt to sort
		if (listView.VirtualListSize == 0)
		{
			logger.Warn(message: "Attempted to sort an empty virtual list.");
			return;
		}
		// Get the current count of items in the virtual list
		int count = listView.VirtualListSize;
		// Initialize sortedIndices if it is null or if the count has changed (e.g., due to a new list being loaded)
		if (sortedIndices == null || sortedIndices.Count != count)
		{
			// Initialize sortedIndices with the current range of indices based on the virtual list offset
			sortedIndices = [.. Enumerable.Range(start: virtualListOffset, count: count)];
		}
		// Remember the currently selected database index before sorting, so we can restore selection afterward
		int? selectedDbIndex = GetSelectedDatabaseIndex();
		// Sort the indices using a custom comparison that uses the precomputed sort keys
		sortedIndices.Sort(comparison: (a, b) =>
		{
			// Retrieve the records for the two indices being compared
			string recA = planetoidsDatabase[index: a];
			string recB = planetoidsDatabase[index: b];
			// Validate that both records are long enough for comparison
			if (recA.Length < nameStartIndex + nameLength || recB.Length < nameStartIndex + nameLength)
			{
				// Log a warning if one or both records are too short for comparison
				logger.Warn(message: $"One or both records are too short for comparison. Index A: {a}, Length: {recA.Length}; Index B: {b}, Length: {recB.Length}");
				return 0;
			}
			// Use ReadOnlySpan<char> for efficient substring extraction without allocations
			ReadOnlySpan<char> spanA = recA.AsSpan();
			ReadOnlySpan<char> spanB = recB.AsSpan();
			// Initialize the comparison result
			int result = 0;
			// Perform the comparison based on the currently sorted column. Sort by Index (Numeric comparison if possible, otherwise string comparison)
			if (sortColumn == 0)
			{
				// Sort by Index (Numeric comparison if possible, otherwise string comparison)
				ReadOnlySpan<char> indexA = spanA[..indexLength].Trim();
				ReadOnlySpan<char> indexB = spanB[..indexLength].Trim();
				// Attempt to parse the indices as integers for numeric comparison
				bool isNumA = int.TryParse(s: indexA, result: out int numA);
				bool isNumB = int.TryParse(s: indexB, result: out int numB);
				// If both indices are numeric, compare numerically; otherwise, compare as strings (case-insensitive)
				result = (isNumA && isNumB) ? numA.CompareTo(value: numB) : indexA.CompareTo(other: indexB, comparisonType: StringComparison.OrdinalIgnoreCase);
			}
			else if (sortColumn == 1) // Sort by Designation (String comparison)
			{
				// Sort by Designation (String comparison)
				ReadOnlySpan<char> desigA = spanA.Slice(start: nameStartIndex, length: nameLength).Trim();
				ReadOnlySpan<char> desigB = spanB.Slice(start: nameStartIndex, length: nameLength).Trim();
				result = desigA.CompareTo(other: desigB, comparisonType: StringComparison.OrdinalIgnoreCase);
			}
			// If the values are equal, we can optionally fall back to comparing the original indices to ensure a stable sort, but in this case we will just return 0 for equal values.
			return sortOrder == SortOrder.Descending ? -result : result;
		});
		// After sorting, restore the selection based on the remembered database index, if possible.
		RestoreSelection(selectedDbIndex: selectedDbIndex);
		listView.Invalidate();
		// Log the sorting action
		logger.Info(message: $"Virtual list sorted by column {sortColumn} in {sortOrder} order.");
	}

	/// <summary>Sets the reference to the planetoids database for this form. This method is used to provide the form with the necessary data to display and interact with the list of readable designations.</summary>
	/// <param name="database">The planetoids database to be referenced by this form.</param>
	/// <exception cref="ArgumentNullException">Thrown if the provided database is null.</exception>
	/// <remarks>This method is called to set the reference to the planetoids database, which is used for displaying and navigating through the list of readable designations.</remarks>
	public void SetDatabaseReference(IReadOnlyList<string> database)
	{
		planetoidsDatabase = database ?? throw new ArgumentNullException(paramName: nameof(database));
		logger.Info(message: $"Database reference set with {planetoidsDatabase.Count} records.");
	}

	/// <summary>Triggers navigation to the selected planetoid record in the main form. This method retrieves the selected database index, validates it, and attempts to parse the corresponding record. If successful, it navigates to the record in the main form; otherwise, it shows an error message.</summary>
	/// <remarks>This method is called when the user clicks the "Go to Object" button or double-clicks a list view item. It handles the navigation logic to the selected planetoid record in the main form.</remarks>
	private void TriggerNavigation()
	{
		// Retrieve the selected database index
		int? dbIndex = GetSelectedDatabaseIndex();
		// Validate the selected index and ensure it is within the bounds of the database
		if (!dbIndex.HasValue || dbIndex.Value < 0 || dbIndex.Value >= planetoidsDatabase.Count)
		{
			// Log a warning if no valid item is selected for navigation
			logger.Warn(message: "No valid item selected for navigation.");
			return;
		}
		// Attempt to parse the selected planetoid record
		if (TryParsePlanetoidRecord(record: planetoidsDatabase[index: dbIndex.Value], parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
		{
			// If the main form is open, navigate to the selected record
			if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
			{
				// Log the navigation action with the selected index and designation
				logger.Info(message: $"Navigating to planetoid record: Index={strIndex}, Designation={strDesignation}");
				// Call the JumpToRecord method on the main form to navigate to the selected record
				mainForm.JumpToRecord(index: strIndex, designation: strDesignation);
				// Bring the main form to the front after navigation
				mainForm.BringToFront();
			}
			// Set the dialog result to OK and close the form after successful navigation
			DialogResult = DialogResult.OK;
			Close();
		}
		// If the record format is invalid, log a warning and show an error message
		else
		{
			// Log a warning if the record format is invalid
			logger.Warn(message: $"Invalid record format for navigation at index {dbIndex.Value}. Record: {planetoidsDatabase[dbIndex.Value]}");
			// Show an error message if the record format is invalid
			ShowErrorMessage("Invalid record format.");
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the ListReadableDesignationsForm loads. Initializes UI state: clears the status area, disables controls until data is available, and sets numeric up/down ranges based on the loaded planetoids database.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form's UI components and state.</remarks>
	private void ListReadableDesignationsForm_Load(object? sender, EventArgs? e)
	{
		// Log the form load event
		logger.Info(message: "ListReadableDesignationsForm is loading.");
		// Clear the status bar on load
		ClearStatusBar(label: labelInformation);
		// Disable controls until data is available
		labelInformation.Enabled = listView.Visible = toolStripButtonGoToObject.Enabled = toolStripDropDownButtonSaveList.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			logger.Warn(message: "Planetoids database is empty on form load.");
			return;
		}
		// Set numeric up/down ranges based on the planetoids database
		toolStripNumericUpDownMinimum.Minimum = 1;
		toolStripNumericUpDownMaximum.Minimum = 1;
		toolStripNumericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMinimum.Value = 1;
		toolStripNumericUpDownMaximum.Value = planetoidsDatabase.Count;
		// Log the count of the planetoids database
		logger.Info(message: $"Form loaded with planetoids database count: {planetoidsDatabase.Count}");
	}

	/// <summary>Handles the form Closed event. Cleans up resources and cancels any ongoing operations.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the form is closed.</remarks>
	private void ListReadableDesignationsForm_FormClosed(object sender, FormClosedEventArgs e) =>
		// Clearing the token if the window is closed during work
		listView.Dispose();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// Log the column click event
		logger.Info(message: $"Column {e.Column} clicked for sorting.");
		// If the virtual list is empty, log a warning and return without sorting
		if (listView.VirtualListSize == 0)
		{
			logger.Warn(message: "Attempted to sort an empty virtual list on column click.");
			return;
		}
		// Toggle the sort order if the same column is clicked; otherwise, set to ascending
		sortOrder = (e.Column == sortColumn && sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
		// Update the sort column to the newly clicked column
		sortColumn = e.Column;
		// Log the new sort order
		logger.Info(message: $"Sorting by column {sortColumn} in {sortOrder} order.");
		// Update the column headers to reflect the new sort order
		UpdateColumnHeaders();
		// Sort the virtual list based on the new sort order
		SortVirtualList();
	}

	/// <summary>Handles the retrieval of virtual items for the ListView. Dynamically creates ListViewItems when they are needed for display.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to retrieve virtual items for the ListView.</remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Calculating the true index in the database based on the offset and sorting
		int realIndex = sortedIndices != null && e.ItemIndex < sortedIndices.Count
			? sortedIndices[index: e.ItemIndex]
			: virtualListOffset + e.ItemIndex;
		// Validate the real index and create the ListViewItem if valid
		if (realIndex >= 0 && realIndex < planetoidsDatabase.Count)
		{
			// Retrieve the current data for the real index
			string currentData = planetoidsDatabase[index: realIndex];
			// Use the shared parsing logic to extract index and designation
			if (TryParsePlanetoidRecord(record: currentData, parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
			{
				// Create a new ListViewItem with the parsed index and designation
				ListViewItem item = new(strIndex)
				{
					ToolTipText = $"{strIndex}: {strDesignation}"
				};
				// Add the designation as a subitem
				_ = item.SubItems.Add(text: strDesignation);
				e.Item = item;
				return;
			}
			// If parsing fails, log a warning
			logger.Warn($"Invalid record at index {realIndex}. Data too short or malformed.");
		}
		// If the real index is out of bounds, log an error and provide a placeholder item
		logger.Error($"Failed to retrieve virtual item for index {e.ItemIndex}. Real index: {realIndex}. Database count: {planetoidsDatabase.Count}.");
		e.Item = new ListViewItem(text: "Error");
		_ = e.Item.SubItems.Add(text: "Invalid Data");
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for the Create List button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the click event for the Create List button.</remarks>
	private void ButtonCreateList_Click(object? sender, EventArgs? e)
	{
		// Reset UI status
		ClearStatusBar(label: labelInformation);
		// Check if the database is loaded
		if (planetoidsDatabase.Count == 0)
		{
			logger.Warn(message: "Planetoids database is empty.");
			return;
		}
		// Validate the numeric up/down values
		try
		{
			// Validate the numeric up/down values
			listView.BeginUpdate();
			listView.Visible = false;
			listView.VirtualMode = false;
			// Clear selection before resetting, very important!
			listView.SelectedIndices.Clear();
			listView.Items.Clear();
			listView.Columns.Clear();
			// Add columns for index and readable designation
			listView.Columns.Add(new ColumnHeader { Text = I18nStrings.Index, TextAlign = HorizontalAlignment.Right, Width = 100 });
			listView.Columns.Add(new ColumnHeader { Text = "Readable Designation", TextAlign = HorizontalAlignment.Left, Width = 300 });
			// Calculate the range based on the numeric up/down values
			int min = (int)toolStripNumericUpDownMinimum.Value - 1;
			int max = (int)toolStripNumericUpDownMaximum.Value;
			int count = max - min;
			// If the count is less than or equal to zero, show a message and return
			if (count > 0)
			{
				// Virtual Mode configure
				sortedIndices = null;
				sortColumn = -1;
				sortOrder = SortOrder.None;
				virtualListOffset = min;
				// Enable virtual mode and set the virtual list size
				listView.VirtualMode = true;
				listView.VirtualListSize = count;
			}
			// If the count is zero or negative, just show an empty list
			listView.Visible = true;
		}
		// Handle any exceptions that occur during the list initialization
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error initializing virtual list.");
			ShowErrorMessage(message: $"Error loading list: {ex.Message}");
		}
		// Ensure that the UI is updated and the save button is enabled regardless of success or failure
		finally
		{
			listView.EndUpdate();
			toolStripDropDownButtonSaveList.Enabled = true;
		}
	}

	/// <summary>Handles the click event for the "Go to Object" button. This method triggers navigation to the selected planetoid record in the main form.</summary>
	/// <param name="sender">The source of the event, typically the "Go to Object" button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method calls the TriggerNavigation method to navigate to the selected planetoid record in the main form.</remarks>
	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
	{
		// Log the button click event
		logger.Info(message: "Go to Object button clicked.");
		// Trigger navigation to the selected planetoid record
		TriggerNavigation();
	}

	#endregion

	#region Double-Click event handlers

	/// <summary>Handles the double-click event on the list view to navigate to the selected planetoid record in the main form.</summary>
	/// <remarks>If no item is selected or the selected record is invalid, the method does not perform any action. When a valid record is selected, the corresponding entry is located and displayed in the main form. An error message is shown if the record format is invalid.</remarks>
	/// <param name="sender">The source of the event, typically the list view control.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ListView_DoubleClick(object sender, EventArgs e)
	{
		// Log the button click event
		logger.Info(message: "Go to Object button clicked.");
		// Trigger navigation to the selected planetoid record
		TriggerNavigation();
	}

	#endregion
}
