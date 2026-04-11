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

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and readable designation,
	/// enables the Go to object button if necessary and stores the currently selected index.</summary>
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

	/// <summary>Handles the Click event of the Go To Object button on the tool strip, initiating the selection of a planetoid and,
	/// when successful, closing the current form.</summary>
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

	/// <summary>Saves the current list as a CSV file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As CSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CSV file to save the list view results; if the user confirms the save operation, call the SaveAsCsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save as CSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsCsv(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an HTML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As HTML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the HTML file to save the list view results; if the user confirms the save operation, call the SaveAsHtml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "HTML files (*.html)|*.html|All Files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save as HTML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsHtml(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an XML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As XML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XML file to save the list view results; if the user confirms the save operation, call the SaveAsXml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XML files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as XML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsXml(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a JSON file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As JSON" menu item.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the JSON file to save the list view results; if the user confirms the save operation, call the SaveAsJson method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "JSON files (*.json)|*.json|All Files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save as JSON"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsJson(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a SQL script.
	/// Exports the list as a series of SQL INSERT statements.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As SQL" menu item.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQL file to save the list view results; if the user confirms the save operation, call the SaveAsSql method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save as SQL"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsSql(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a Markdown table.
	/// Ideal for documentation, GitHub Readmes, or Wikis.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As Markdown" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Markdown file to save the list view results; if the user confirms the save operation, call the SaveAsMarkdown method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Markdown files (*.md)|*.md|All Files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save as Markdown"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsMarkdown(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the list in YAML format.
	/// A human-readable data serialization standard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As YAML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the YAML file to save the list view results; if the user confirms the save operation, call the SaveAsYaml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save as YAML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsYaml(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the list as a TSV (Tab-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As TSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TSV file to save the list view results; if the user confirms the save operation, call the SaveAsTsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save as TSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsTsv(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the list as a PSV (Pipe-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PSV file to save the list view results; if the user confirms the save operation, call the SaveAsPsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save as PSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsPsv(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the list as a LaTeX document.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As LaTeX" menu item.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the LaTeX file to save the list view results; if the user confirms the save operation, call the SaveAsLatex method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save as LaTeX"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsLatex(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a PostScript (.ps) file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PostScript" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PostScript file to save the list view results; if the user confirms the save operation, call the SaveAsPostScript method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save as PostScript"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsPostScript(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an uncompressed PDF file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PDF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PDF file to save the list view results; if the user confirms the save operation, call the SaveAsPdf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save as PDF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsPdf(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an EPUB file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As EPUB" menu item.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the EPUB file to save the list view results; if the user confirms the save operation, call the SaveAsEpub method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save as EPUB"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsEpub(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a Word document.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Word" menu item.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Word file to save the list view results; if the user confirms the save operation, call the SaveAsWord method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Word documents (*.docx)|*.docx|All Files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save as Word"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsWord(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an Excel file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Excel" menu item.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Excel file to save the list view results; if the user confirms the save operation, call the SaveAsExcel method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save as Excel"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsExcel(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an ODT file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODT" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Text file to save the list view results; if the user confirms the save operation, call the SaveAsOdt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save as ODT"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsOdt(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an ODS file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODS" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Spreadsheet file to save the list view results; if the user confirms the save operation, call the SaveAsOds method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save as ODS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsOds(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a simplified MOBI file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As MOBI" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the MOBI file to save the list view results; if the user confirms the save operation, call the SaveAsMobi method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save as MOBI"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsMobi(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as an RTF file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As RTF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Rich Text Format file to save the list view results; if the user confirms the save operation, call the SaveAsRtf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save as RTF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsRtf(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Saves the current list as a text file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Text" menu item.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the text file to save the list view results; if the user confirms the save operation, call the SaveAsText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Text files (*.txt)|*.txt|All Files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save as Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsText(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event for the 'Save As AsciiDoc' menu item and initiates saving the ListView results in AsciiDoc
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as an AsciiDoc-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the AsciiDoc file to save the list view results; if the user confirms the save operation, call the SaveAsAsciiDoc method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*",
			DefaultExt = "adoc",
			Title = "Save as AsciiDoc"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsAsciiDoc(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event for the 'Save As reStructuredText' menu item and initiates saving the current ListView
	/// results in reStructuredText format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as a reStructuredText-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the reStructuredText file to save the list view results; if the user confirms the save operation, call the SaveAsReStructuredText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*",
			DefaultExt = "rst",
			Title = "Save as reStructuredText"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsReStructuredText(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event of the 'Save As Textile' menu item and initiates saving the ListView results in Textile
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables
	/// users to export the current ListView results as a Textile-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Textile file to save the list view results; if the user confirms the save operation, call the SaveAsTextile method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Textile files (*.textile)|*.textile|All Files (*.*)|*.*",
			DefaultExt = "textile",
			Title = "Save as Textile"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsTextile(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event for the 'Save As Abiword' menu item and initiates saving the current list view results in
	/// Abiword format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Abiword" menu item, this event handler is invoked. It calls the SaveListViewResultsAsAbiword method, which generates an AWML (AbiWord XML) file with a .abw extension that can be opened in Abiword. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Abiword file to save the list view results; if the user confirms the save operation, call the SaveAsAbiword method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*",
			DefaultExt = "abw",
			Title = "Save as Abiword"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsAbiword(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the Save As WPS menu item and initiates saving the current ListView results in WPS
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As WPS" menu item, this event handler is invoked. It calls the SaveAsWps method, which generates an HTML file with a .wps extension that can be opened in WPS Writer. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Writer file to save the list view results; if the user confirms the save operation, call the SaveAsWps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*",
			DefaultExt = "wps",
			Title = "Save as WPS Writer"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsWps(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the 'Save As Et' menu item and initiates saving the current ListView results in the Et
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Et" menu item, this event handler is invoked. It calls the SaveAsEt method, which exports the data in a format compatible with WPS Spreadsheets (using CSV internally). If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Spreadsheets file to save the list view results; if the user confirms the save operation, call the SaveAsEt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*",
			DefaultExt = "et",
			Title = "Save as WPS Spreadsheets"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsEt(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event for the 'Save As DocBook' menu item, initiating the process to save the current list view
	/// results in DocBook format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As DocBook" menu item, this event handler is invoked. It calls the SaveAsDocBook method, which generates an XML document conforming to the DocBook schema, containing the list of readable designations. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the DocBook file to save the list view results; if the user confirms the save operation, call the SaveAsDocBook method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as DocBook"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsDocBook(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the click event for the 'Save As TOML' menu item and initiates saving the current results in TOML format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As TOML" menu item, this event handler is invoked. It calls the SaveAsToml method, which generates the necessary TOML structure for the current results and saves it as a .toml file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TOML file to save the list view results; if the user confirms the save operation, call the SaveAsToml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*",
			DefaultExt = "toml",
			Title = "Save as TOML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsToml(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the Save As XPS menu item and initiates saving the current ListView results as an XPS
	/// document.</summary>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As XPS" menu item, this event handler is invoked. It calls the SaveAsXps method, which generates the necessary XML structure for an XPS document and saves it as a .xps file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XPS file to save the list view results; if the user confirms the save operation, call the SaveAsXps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*",
			DefaultExt = "xps",
			Title = "Save as XPS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsXps(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the Save As FictionBook2 menu item and initiates saving the current results in
	/// FictionBook2 format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As FictionBook2" menu item, this event handler is invoked. It calls the SaveAsFictionBook2 method, which generates an XML document conforming to the FictionBook2 schema, containing the list of readable designations. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the FictionBook2 file to save the list view results; if the user confirms the save operation, call the SaveAsFictionBook2 method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*",
			DefaultExt = "fb2",
			Title = "Save as FictionBook2"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsFictionBook2(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the Save As CHM menu item and initiates saving the current ListView results as a CHM
	/// file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As CHM" menu item, this event handler is invoked. It calls the SaveAsChm method, which generates the necessary HTML and project files, then uses Microsoft HTML Help Workshop to compile them into a CHM file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CHM file to save the list view results; if the user confirms the save operation, call the SaveAsChm method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*",
			DefaultExt = "chm",
			Title = "Save as CHM"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsChm(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Handles the Click event of the Save As SQLite menu item and initiates saving the current ListView results as a SQLite
	/// file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As SQLite" menu item, this event handler is invoked. It calls the SaveAsSqlite method.</remarks>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQLite file to save the list view results; if the user confirms the save operation, call the SaveAsSqlite method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*",
			DefaultExt = "sqlite",
			Title = "Save as SQLite"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			ListViewExporter.SaveAsSqlite(listView: listView, title: "List of readable designations", fileName: saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

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