// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

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
	private List<string> planetoidsDatabase = [];

	/// <summary>Number of planetoids in the database.</summary>
	/// <remarks>This field keeps track of the total number of planetoids in the database.</remarks>
	private int numberPlanetoids;

	/// <summary>Index of the currently selected planetoid.</summary>
	/// <remarks>This index is used to keep track of the currently selected planetoid in the list.</remarks>
	private int selectedIndex;

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

	/// <summary>Creates a ListViewItem for the specified index.</summary>
	/// <param name="index">The index of the planetoid.</param>
	/// <returns>A ListViewItem representing the planetoid, or null if the index is invalid.</returns>
	/// <remarks>This method is used to create a ListViewItem for the specified index.</remarks>
	private ListViewItem? CreateListViewItem(int index)
	{
		// Check if the index is valid
		if (index < 0 || index >= numberPlanetoids)
		{
			// Log a warning and return null
			logger.Warn(message: $"Invalid index {index} requested.");
			return null;
		}
		// Get the current planetoid data
		string currentData = planetoidsDatabase[index];
		// Check if the current data is long enough
		if (currentData.Length < nameStartIndex + nameLength)
		{
			// Log a warning and return null
			logger.Warn(message: $"The record at index {index} is too short.");
			return null;
		}
		// Extract the index and designation name
		string strIndex = currentData[..indexLength].Trim();
		string strDesignationName = currentData.Substring(startIndex: nameStartIndex, length: nameLength).Trim();
		// Create and return the ListViewItem
		ListViewItem item = new(text: strIndex)
		{
			// Set the tool tip text
			ToolTipText = $"{strIndex}: {strDesignationName}"
		};
		// Add the designation name as a subitem
		item.SubItems.Add(text: strDesignationName);
		// Return the created item
		return item;
	}

	/// <summary>Fills the planetoids database with the provided list.</summary>
	/// <param name="arrTemp">The list to fill the database with.</param>
	/// <remarks>This method is used to fill the planetoids database with the provided list.</remarks>
	public void FillArray(List<string> arrTemp)
	{
		planetoidsDatabase = [.. arrTemp];
		numberPlanetoids = planetoidsDatabase.Count;
	}

	/// <summary>Sets the maximum index for the planetoids database.</summary>
	/// <param name="maxIndex">The maximum index.</param>
	/// <remarks>This method is used to set the maximum index for the planetoids database.</remarks>
	public void SetMaxIndex(int maxIndex) => numberPlanetoids = maxIndex;

	/// <summary>Gets the selected index in the list view.</summary>
	/// <returns>The selected index.</returns>
	/// <remarks>This method is used to get the selected index in the list view.</remarks>
	public int GetSelectedIndex() => selectedIndex;

	/// <summary>Tries to parse a fixed-width planetoid record into its index and designation components.</summary>
	/// <param name="record">The raw database record to parse.</param>
	/// <param name="recordIndex">The zero-based index of the record in the database, used for logging purposes.</param>
	/// <param name="parsedIndex">When this method returns <c>true</c>, contains the parsed index value.</param>
	/// <param name="parsedDesignation">When this method returns <c>true</c>, contains the parsed designation value.</param>
	/// <returns><c>true</c> if the record was successfully parsed; otherwise, <c>false</c>.</returns>
	private static bool TryParsePlanetoidRecord(string record, int recordIndex, out string parsedIndex, out string parsedDesignation)
	{
		// Initialize output parameters
		parsedIndex = string.Empty;
		parsedDesignation = string.Empty;
		// Validate the input record
		if (string.IsNullOrWhiteSpace(value: record))
		{
			logger.Warn(message: $"The record at index {recordIndex} is null, empty, or consists only of white-space characters.");
			return false;
		}
		// Check if the record is long enough to contain the expected fields
		if (record.Length < nameStartIndex + nameLength)
		{
			logger.Warn(message: $"The record at index {recordIndex} is too short.");
			return false;
		}
		// Extract the index and designation from the fixed-width record
		parsedIndex = record[..indexLength].Trim();
		parsedDesignation = record.Substring(startIndex: nameStartIndex, length: nameLength).Trim();
		return true;
	}

	/// <summary>Selects the currently highlighted planetoid in the list view and navigates to its corresponding record in the main form.</summary>
	/// <remarks>If no item is selected or the selected record is invalid, the method returns <c>false</c> without performing any action. When a valid planetoid is selected, the main form is brought to the foreground and displays the details of the selected planetoid.</remarks>
	/// <returns><c>true</c> if navigation to the selected planetoid succeeded; otherwise, <c>false</c>.</returns>
	private bool SelectPlanetoidInMainForm()
	{
		if (listView.SelectedIndices.Count == 0)
		{
			return false;
		}
		int selectedIndex = listView.SelectedIndices[index: 0];
		// Calculate the real database index (considering virtual mode offset and sorting)
		int dbIndex = listView.VirtualMode
			? (sortedIndices != null && selectedIndex < sortedIndices.Count ? sortedIndices[index: selectedIndex] : virtualListOffset + selectedIndex)
			: selectedIndex;
		// Check if the index is valid
		if (dbIndex < 0 || dbIndex >= planetoidsDatabase.Count)
		{
			return false;
		}
		// Get the record string
		string currentData = planetoidsDatabase[index: dbIndex];
		// Parse index and designation using shared parsing logic
		if (!TryParsePlanetoidRecord(record: currentData, recordIndex: dbIndex, parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
		{
			// If parsing fails, show an error message and return
			ShowErrorMessage(message: "Invalid record format.");
			return false;
		}
		// Jump to the record in the main form
		if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
		{
			mainForm.JumpToRecord(index: strIndex, designation: strDesignation);
			mainForm.BringToFront();
		}
		return true;
	}

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event. Updates the status bar with the selected planetoid's index and readable designation, enables the Go to object button if necessary and stores the currently selected index.</summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the SelectedIndexChanged event of the ListView.</remarks>
	private void SelectedIndexChanged(object? sender, EventArgs? e)
	{
		// Check if there are any selected indices
		if (listView.SelectedIndices.Count <= 0)
		{
			SetStatusBar(label: labelInformation, text: string.Empty);
			toolStripButtonGoToObject.Enabled = false;
			return;
		}
		// Get the selected virtual index
		int index = listView.SelectedIndices[index: 0];
		// Calculate the real database index (considering virtual mode offset and sorting)
		int dbIndex = listView.VirtualMode
			? (sortedIndices != null && index < sortedIndices.Count ? sortedIndices[index: index] : virtualListOffset + index)
			: index;
		// Derive display text from the backing data to avoid accessing Items in virtual mode
		if (dbIndex >= 0 && dbIndex < planetoidsDatabase.Count &&
			TryParsePlanetoidRecord(record: planetoidsDatabase[index: dbIndex], recordIndex: dbIndex, parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
		{
			SetStatusBar(label: labelInformation, text: $"{I18nStrings.Index}: {strIndex} - {strDesignation}");
		}
		// Enable the Go to object button
		toolStripButtonGoToObject.Enabled = true;
		selectedIndex = index;
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the ListReadableDesignationsForm loads. Initializes UI state: clears the status area, disables controls until data is available, and sets numeric up/down ranges based on the loaded planetoids database.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form's UI components and state.</remarks>
	private void ListReadableDesignationsForm_Load(object? sender, EventArgs? e)
	{
		// Clear the status bar on load
		ClearStatusBar(label: labelInformation);
		// Disable controls until data is available
		labelInformation.Enabled = listView.Visible = toolStripButtonGoToObject.Enabled = toolStripDropDownButtonSaveList.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Set numeric up/down ranges based on the planetoids database
		toolStripNumericUpDownMinimum.Minimum = 1;
		toolStripNumericUpDownMaximum.Minimum = 1;
		toolStripNumericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMinimum.Value = 1;
		toolStripNumericUpDownMaximum.Value = planetoidsDatabase.Count;
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
		// If there are no items, do not attempt to sort
		if (listView.VirtualListSize == 0)
		{
			return;
		}
		// Determine the new sort order based on the clicked column
		if (e.Column == sortColumn)
		{
			// Toggle sort order if the same column is clicked
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			// Set new sort column and default to ascending order
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update column headers with sort indicators
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Remove existing sort indicators from the header text
			string headerText = listView.Columns[index: i].Text;
			// Check for existing indicators and remove them
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column
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
		// Sort the indices based on the selected column and sort order
		int count = listView.VirtualListSize;
		// Initialize sortedIndices if it is null or if the count has changed (e.g., due to a new list being loaded)
		if (sortedIndices == null || sortedIndices.Count != count)
		{
			// Create a list of indices corresponding to the current virtual list size
			sortedIndices = [.. Enumerable.Range(start: virtualListOffset, count: count)];
		}
		// Before sorting, capture the currently selected database index (if any) so selection can be preserved.
		int? selectedDatabaseIndex = null;
		if (listView.SelectedIndices.Count > 0)
		{
			int selectedVirtualIndex = listView.SelectedIndices[index: 0];
			int realIndexBeforeSort = sortedIndices != null && selectedVirtualIndex < sortedIndices.Count
				? sortedIndices[index: selectedVirtualIndex]
				: virtualListOffset + selectedVirtualIndex;
			selectedDatabaseIndex = realIndexBeforeSort;
		}
		// Precompute sort keys once per index to avoid repeated substring/trim/parse work during comparison
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
		Dictionary<int, (bool HasNumeric, int NumericValue, string TextValue)> sortKeyCache = new(capacity: sortedIndices.Count);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
		foreach (int index in sortedIndices)
		{
			string rec = index >= 0 && index < planetoidsDatabase.Count ? planetoidsDatabase[index: index] : string.Empty;
			string value = string.Empty;
			// For column 0, we compare the index; for column 1, we compare the designation name
			switch (sortColumn)
			{
				case 0:
					value = rec.Length >= indexLength ? rec[..indexLength].Trim() : string.Empty;
					break;
				case 1:
					value = rec.Length >= nameStartIndex + nameLength
						? rec.Substring(startIndex: nameStartIndex, length: nameLength).Trim()
						: string.Empty;
					break;
			}
			bool hasNumeric = int.TryParse(s: value, result: out int numericValue);
			sortKeyCache[key: index] = (hasNumeric, numericValue, value);
		}
		// Sort the indices using a custom comparison that uses the precomputed sort keys
		sortedIndices.Sort(comparison: (a, b) =>
		{
			// Retrieve precomputed sort keys; if missing, fall back to empty defaults
			(bool HasNumeric, int NumericValue, string TextValue) = sortKeyCache.TryGetValue(key: a, value: out (bool HasNumeric, int NumericValue, string TextValue) ka) ? ka : (HasNumeric: false, NumericValue: 0, TextValue: string.Empty);
			(bool HasNumericB, int NumericValueB, string TextValueB) = sortKeyCache.TryGetValue(key: b, value: out (bool HasNumeric, int NumericValue, string TextValue) kb) ? kb : (HasNumeric: false, NumericValue: 0, TextValue: string.Empty);
			int result = HasNumeric && HasNumericB
				? NumericValue.CompareTo(value: NumericValueB)
				: string.Compare(
					strA: TextValue,
					strB: TextValueB,
					comparisonType: StringComparison.OrdinalIgnoreCase);
			// If both values have numeric representations, compare numerically; otherwise, compare as strings (case-insensitive)
			// If the values are equal, we can optionally fall back to comparing the original indices to ensure a stable sort, but in this case we will just return 0 for equal values.
			return sortOrder == SortOrder.Descending ? -result : result;
		});
		// After sorting, restore the selection based on the remembered database index, if possible.
		if (selectedDatabaseIndex.HasValue)
		{
			// Find the new virtual index of the previously selected database index after sorting
			int newVirtualIndex = sortedIndices != null
				? sortedIndices.IndexOf(item: selectedDatabaseIndex.Value)
				: selectedDatabaseIndex.Value - virtualListOffset;
			// If sortedIndices is not null, we can find the new virtual index directly; otherwise, we calculate it based on the offset.
			// If the new virtual index is valid, select it and ensure it is visible
			if (newVirtualIndex >= 0 && newVirtualIndex < listView.VirtualListSize)
			{
				listView.SelectedIndices.Clear();
				listView.SelectedIndices.Add(itemIndex: newVirtualIndex);
				listView.EnsureVisible(index: newVirtualIndex);
			}
		}
		// After sorting the indices, we need to refresh the ListView to reflect the new order. In virtual mode, this is done by invalidating the control, which will trigger it to request the items in the new order.
		listView.Invalidate();
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
		// Creating the item (uses the existing logic)
		ListViewItem? item = CreateListViewItem(index: realIndex);
		// If the item was created successfully, assign it.
		// If null is returned (error), create a placeholder to avoid crashes.
		if (item != null)
		{
			e.Item = item;
		}
		else
		{
			e.Item = new ListViewItem(text: "Error");
			e.Item.SubItems.Add(text: "Invalid Data");
		}
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
			return;
		}
		// Define columns (as in the original)
		ColumnHeader columnHeaderIndex = new()
		{
			Text = I18nStrings.Index,
			TextAlign = HorizontalAlignment.Right,
			Width = 100
		};
		ColumnHeader columnHeaderReadableDesignation = new()
		{
			Text = "Readable Designation",
			TextAlign = HorizontalAlignment.Left,
			Width = 300
		};
		// Begin UI update
		try
		{
			listView.BeginUpdate();
			// Reset list
			listView.Visible = false;
			// Clear selection before resetting, very important!
			listView.SelectedIndices.Clear();
			// Temporarily disable to clear
			listView.VirtualMode = false;
			listView.Items.Clear();
			listView.Columns.Clear();
			listView.Columns.AddRange(values: [columnHeaderIndex, columnHeaderReadableDesignation]);
			// Calculate range
			int min = (int)toolStripNumericUpDownMinimum.Value - 1;
			int max = (int)toolStripNumericUpDownMaximum.Value;
			int count = max - min;
			if (count <= 0)
			{
				listView.Visible = true;
				listView.EndUpdate();
				return;
			}
			// Virtual Mode configure
			sortedIndices = null;
			sortColumn = -1;
			sortOrder = SortOrder.None;
			virtualListOffset = min; // Start offset save
			listView.VirtualMode = true; // Activate virtual mode
			listView.VirtualListSize = count; // Set number of rows (triggers RetrieveVirtualItem when scrolling)
			listView.Visible = true;
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error initializing virtual list.");
			ShowErrorMessage(message: $"Error loading list: {ex.Message}");
		}
		finally
		{
			listView.EndUpdate();
			toolStripDropDownButtonSaveList.Enabled = true;
		}
	}

	/// <summary>Handles the Click event of the Go To Object button on the tool strip, initiating the selection of a planetoid and, when successful, closing the current form.</summary>
	/// <param name="sender">The source of the event, typically the Go To Object button on the tool strip.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the Go To Object button is clicked, this method calls the SelectPlanetoidInMainForm method to navigate to the selected planetoid record in the main form. Only if navigation succeeds, it closes the current form and sets the dialog result to <see cref="DialogResult.OK"/> to signal a successful selection.</remarks>
	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
	{
		// Select the planetoid in the main form; only close if navigation succeeded
		if (SelectPlanetoidInMainForm())
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}

	#endregion

	#region Double-Click event handlers

	/// <summary>Handles the double-click event on the list view to navigate to the selected planetoid record in the main form.</summary>
	/// <remarks>If no item is selected or the selected record is invalid, the method does not perform any action. When a valid record is selected, the corresponding entry is located and displayed in the main form. An error message is shown if the record format is invalid.</remarks>
	/// <param name="sender">The source of the event, typically the list view control.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ListView_DoubleClick(object sender, EventArgs e) => SelectPlanetoidInMainForm();

	#endregion
}
