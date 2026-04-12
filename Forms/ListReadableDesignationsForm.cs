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

	/// <summary>Selects the currently highlighted planetoid in the list view and navigates to its corresponding record in the main
	/// form.</summary>
	/// <remarks>If no item is selected or the selected record is invalid, the method returns <c>false</c> without performing any action. When a valid
	/// planetoid is selected, the main form is brought to the foreground and displays the details of the selected
	/// planetoid.</remarks>
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
			_ = MessageBox.Show(text: "Invalid record format.", caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
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

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Readable-Designation-List_{toolStripNumericUpDownMinimum.Value}-{toolStripNumericUpDownMaximum.Value}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the list view, title, file name, and an optional virtual row provider.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title. The dialog allows the user to choose where to save the exported file and what name to give it.
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog. If the user cancels the dialog, the method returns without performing any export action.
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress, and then invoke the specified export action with the text box containing the output, the title for the export, and the selected file name. After the export action is completed, reset the cursor to the default state.
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(listView, "List of readable designations", saveFileDialog.FileName, null);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and readable designation, enables the Go to object button if necessary and stores the currently selected index.</summary>
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

	/// <summary>Fired when the ListReadableDesignationsForm loads.
	/// Initializes UI state: clears the status area, disables controls until data is available,
	/// and sets numeric up/down ranges based on the loaded planetoids database.</summary>
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

	/// <summary>Handles the form Closed event.
	/// Cleans up resources and cancels any ongoing operations.</summary>
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

	/// <summary>Handles the retrieval of virtual items for the ListView.
	/// Dynamically creates ListViewItems when they are needed for display.</summary>
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

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as HTML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as JSON.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQL.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: ListViewExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Markdown.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Markdown files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: ListViewExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as YAML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: ListViewExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: ListViewExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: ListViewExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as LaTeX.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: ListViewExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PostScript.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: ListViewExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PDF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: ListViewExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as EPUB.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: ListViewExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Word.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Word documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: ListViewExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Excel.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: ListViewExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODT file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODT.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as ODT", exportAction: ListViewExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: ListViewExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as MOBI.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: ListViewExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as RTF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: ListViewExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Text files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: ListViewExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as AsciiDoc.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: ListViewExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a reStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a reStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as reStructuredText.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: ListViewExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Textile.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: ListViewExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Abiword.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: ListViewExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as WPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", exportAction: ListViewExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ET.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", exportAction: ListViewExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as DocBook.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: ListViewExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TOML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: ListViewExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: ListViewExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as FictionBook2.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: ListViewExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CHM.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: ListViewExporter.SaveAsChm);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQLite.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: ListViewExporter.SaveAsSqlite);

	#endregion

	#region Double-Click event handlers

	/// <summary>Handles the double-click event on the list view to navigate to the selected planetoid record in the main form.</summary>
	/// <remarks>If no item is selected or the selected record is invalid, the method does not perform any action.
	/// When a valid record is selected, the corresponding entry is located and displayed in the main form. An error
	/// message is shown if the record format is invalid.</remarks>
	/// <param name="sender">The source of the event, typically the list view control.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ListView_DoubleClick(object sender, EventArgs e) => SelectPlanetoidInMainForm();

	#endregion
}