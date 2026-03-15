// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml;

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

	/// <summary>Gets the export data from the virtual list.</summary>
	/// <returns>An enumerable collection of tuples containing the index and name.</returns>
	/// <remarks>It iterates over the indices and creates the data on-the-fly, instead of accessing listView.Items.</remarks>
	private IEnumerable<(string Index, string Name)> GetExportData()
	{
		// If not in Virtual Mode, simply iterate over the items
		if (!listView.VirtualMode)
		{
			// Iterate over each item in the ListView
			foreach (ListViewItem item in listView.Items)
			{
				// Yield the index and name as a tuple
				yield return (Index: item.SubItems[index: 0].Text, Name: item.SubItems[index: 1].Text);
			}
			yield break;
		}
		// In Virtual Mode over the indices iterate
		for (int i = 0; i < listView.VirtualListSize; i++)
		{
			// Calculate the real database index
			int dbIndex = sortedIndices != null && i < sortedIndices.Count
				? sortedIndices[index: i]
				: virtualListOffset + i;
			// Generate item (we use the existing logic, but without GUI overhead)
			// Since CreateListViewItem returns a ListViewItem, we extract the data again.
			// Performance tip: For pure export, one could bypass CreateListViewItem and parse the string directly,
			// but this keeps the code consistent.
			ListViewItem? item = CreateListViewItem(index: dbIndex);
			// If the item is valid, yield the data
			if (item != null)
			{
				yield return (Index: item.Text, Name: item.SubItems[index: 1].Text);
			}
		}
	}

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
	/// <remarks>If no item is selected or the selected record is invalid, the method does nothing. When a valid
	/// planetoid is selected, the main form is brought to the foreground and displays the details of the selected
	/// planetoid.</remarks>
	private void SelectPlanetoidInMainForm()
	{
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
		int selectedIndex = listView.SelectedIndices[index: 0];
		// Calculate the real database index (considering virtual mode offset and sorting)
		int dbIndex = listView.VirtualMode
			? (sortedIndices != null && selectedIndex < sortedIndices.Count ? sortedIndices[index: selectedIndex] : virtualListOffset + selectedIndex)
			: selectedIndex;
		// Check if the index is valid
		if (dbIndex < 0 || dbIndex >= planetoidsDatabase.Count)
		{
			return;
		}
		// Get the record string
		string currentData = planetoidsDatabase[index: dbIndex];
		// Parse index and designation using shared parsing logic
		if (!TryParsePlanetoidRecord(record: currentData, recordIndex: dbIndex, parsedIndex: out string strIndex, parsedDesignation: out string strDesignation))
		{
			// If parsing fails, show an error message and return
			_ = MessageBox.Show(text: "Invalid record format.", caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			return;
		}
		// Jump to the record in the main form
		if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
		{
			mainForm.JumpToRecord(index: strIndex, designation: strDesignation);
			mainForm.BringToFront();
		}
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
	/// enables the load button if necessary and stores the currently selected index.</summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the SelectedIndexChanged event of the ListView.</remarks>
	private void SelectedIndexChanged(object? sender, EventArgs? e)
	{
		// Check if there are any selected indices
		if (listView.SelectedIndices.Count <= 0)
		{
			return;
		}
		// Get the selected index and item
		int index = listView.SelectedIndices[index: 0];
		ListViewItem item = listView.Items[index: index];
		// Update the status bar with the selected item's details
		SetStatusBar(label: labelInformation, text: $"{I18nStrings.Index}: {item.Text} - {item.SubItems[index: 1].Text}");
		// Enable the load button
		toolStripButtonLoad.Enabled = true;
		selectedIndex = index;
	}

	/// <summary>Escapes LaTeX special characters in a string.</summary>
	/// <param name="input">The input string, which may be <c>null</c>.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>This method is used to escape LaTeX special characters in the input string.</remarks>
	private static string EscapeLatex(string input)
	{
		// Handle null input
		if (input == null)
		{
			return string.Empty;
		}
		// Escape LaTeX special characters
		StringBuilder builder = new(capacity: input.Length);
		// Iterate over each character in the input string
		foreach (char ch in input)
		{
			// Escape special LaTeX characters
			switch (ch)
			{
				case '\\':
					builder.Append(value: "\\textbackslash{}");
					break;
				case '{':
					builder.Append(value: "\\{");
					break;
				case '}':
					builder.Append(value: "\\}");
					break;
				case '%':
					builder.Append(value: "\\%");
					break;
				case '$':
					builder.Append(value: "\\$");
					break;
				case '&':
					builder.Append(value: "\\&");
					break;
				case '#':
					builder.Append(value: "\\#");
					break;
				case '_':
					builder.Append(value: "\\_");
					break;
				case '^':
					builder.Append(value: "\\^{}");
					break;
				case '~':
					builder.Append(value: "\\~{}");
					break;
				default:
					builder.Append(value: ch); // FIX: Use Append(char) instead of Append(string) for single characters
					break;
			}
		}
		// Return the escaped string
		return builder.ToString();
	}

	/// <summary>Escapes Markdown-table-specific characters in a cell value.</summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The cell value escaped for use in a Markdown table.</returns>
	/// <remarks>This method is used to escape Markdown-table-specific characters in the cell value.</remarks>
	private static string EscapeMarkdownCell(string? value)
	{
		// Handle null input
		if (value is null)
		{
			return string.Empty;
		}
		// Escape the pipe character, which is used as a column separator in Markdown tables.
		return value.Replace(oldValue: "|", newValue: "\\|");
	}

	/// <summary>Escapes PostScript special characters in a string.</summary>
	/// <param name="input">The input string.</param>
	/// <returns>The escaped string suitable for PostScript output.</returns>
	/// <remarks>This method is used to escape PostScript special characters in the input string.</remarks>
	private static string EscapePostScript(string? input)
	{
		// Handle null input
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// The backslash must be replaced first, as it is the escape character.
		return input.Replace(oldValue: "\\", newValue: "\\\\")
					.Replace(oldValue: "(", newValue: "\\(")
					.Replace(oldValue: ")", newValue: "\\)");
	}

	/// <summary>Escapes characters for PDF string literals.</summary>
	/// <param name="text">The input text to escape.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>This method is used to escape special characters in the input text for PDF output.</remarks>
	private static string EscapePdf(string? text)
	{
		// Handle null or empty input
		if (string.IsNullOrEmpty(value: text))
		{
			return string.Empty;
		}
		// Escape special characters and control characters for PDF string literals
		StringBuilder builder = new(capacity: text.Length);
		// Iterate over each character in the input text
		foreach (char character in text)
		{
			switch (character)
			{
				case '\\':
					builder.Append(value: "\\\\");
					break;
				case '(':
					builder.Append(value: "\\(");
					break;
				case ')':
					builder.Append(value: "\\)");
					break;
				case '\n':
					builder.Append(value: "\\n");
					break;
				case '\r':
					builder.Append(value: "\\r");
					break;
				case '\t':
					builder.Append(value: "\\t");
					break;
				case '\b':
					builder.Append(value: "\\b");
					break;
				case '\f':
					builder.Append(value: "\\f");
					break;
				default:
					if (character < ' ')
					{
						// Escape remaining control characters using a three-digit octal code
						string octal = Convert.ToString(value: character, toBase: 8)!.PadLeft(totalWidth: 3, paddingChar: '0');
						builder.Append(value: '\\'); // Use Append(char) overload for single characters
						builder.Append(value: octal);
					}
					else
					{
						builder.Append(value: character); // Append the character directly using the char overload
					}
					break;
			}
		}
		return builder.ToString();
	}

	/// <summary>Escapes special characters for RTF output.</summary>
	/// <param name="input">The input string.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>This method is used to escape special characters in the input string for RTF output.</remarks>
	private static string EscapeRtf(string? input)
	{
		// Handle null or empty input
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Escape special characters and control characters for RTF
		StringBuilder builder = new(capacity: input.Length);
		// Iterate over each character in the input string
		foreach (char character in input)
		{
			switch (character)
			{
				case '\\':
					builder.Append(value: "\\\\");
					break;
				case '{':
					builder.Append(value: "\\{");
					break;
				case '}':
					builder.Append(value: "\\}");
					break;
				case '\n':
					builder.Append(value: "\\par ");
					break;
				default:
					if (character > 127)
					{
						// Escape remaining control characters using unicode format
						builder.Append(value: $"\\u{(int)character}?");
					}
					else
					{
						builder.Append(value: character); // Append the character directly
					}
					break;
			}
		}
		return builder.ToString();
	}

	/// <summary>Saves the list as an AsciiDoc document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As AsciiDoc" menu item.</remarks>
	private void SaveListViewResultsAsAsciiDoc()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogAsciiDoc = new()
		{
			Filter = "AsciiDoc files (*.adoc)|*.adoc|All files (*.*)|*.*",
			DefaultExt = "adoc",
			Title = "Save list as AsciiDoc"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogAsciiDoc, ext: saveFileDialogAsciiDoc.DefaultExt))
		{
			return;
		}
		// Write the data to the AsciiDoc file
		using StreamWriter writer = new(path: saveFileDialogAsciiDoc.FileName, append: false, encoding: Encoding.UTF8);
		// Document title
		writer.WriteLine(value: "= List of Readable Designations");
		writer.WriteLine();
		// Configure table
		writer.WriteLine(value: "[options=\"header\"]");
		writer.WriteLine(value: "|===");
		writer.WriteLine(value: "|Index|Designation");
		// Iterate data
		foreach ((string index, string name) in GetExportData())
		{
			string safeName = name.Replace(oldValue: "|", newValue: "\\|"); // Escape pipes in AsciiDoc tables
			writer.WriteLine(value: $"|{index}|{safeName}");
		}
		writer.WriteLine(value: "|===");
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a reStructuredText document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As reStructuredText" menu item.</remarks>
	private void SaveListViewResultsAsReStructuredText()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogRst = new()
		{
			Filter = "reStructuredText files (*.rst)|*.rst|All files (*.*)|*.*",
			DefaultExt = "rst",
			Title = "Save list as reStructuredText"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogRst, ext: saveFileDialogRst.DefaultExt))
		{
			return;
		}
		// Write the data to the reStructuredText file
		using StreamWriter writer = new(path: saveFileDialogRst.FileName, append: false, encoding: Encoding.UTF8);
		// Document title
		string title = "List of Readable Designations";
		writer.WriteLine(value: new string(c: '=', count: title.Length));
		writer.WriteLine(value: title);
		writer.WriteLine(value: new string(c: '=', count: title.Length));
		writer.WriteLine();
		// Iterate data to determine max column widths
		int maxIndexLength = "Index".Length;
		int maxNameLength = "Designation".Length;
		List<(string Index, string Name)> exportData = GetExportData().ToList();
		foreach ((string index, string name) in exportData)
		{
			if (index.Length > maxIndexLength)
			{
				maxIndexLength = index.Length;
			}

			if (name.Length > maxNameLength)
			{
				maxNameLength = name.Length;
			}
		}
		// Add some padding
		maxIndexLength += 2;
		maxNameLength += 2;
		// Helper to create separators
		string separator = $"+{new string(c: '-', count: maxIndexLength)}+{new string(c: '-', count: maxNameLength)}+";
		string headerSeparator = $"+{new string(c: '=', count: maxIndexLength)}+{new string(c: '=', count: maxNameLength)}+";
		// Write table header
		writer.WriteLine(value: separator);
		writer.WriteLine(value: $"| {"Index".PadRight(totalWidth: maxIndexLength - 1)}| {"Designation".PadRight(totalWidth: maxNameLength - 1)}|");
		writer.WriteLine(value: headerSeparator);
		// Write table rows
		foreach ((string index, string name) in exportData)
		{
			writer.WriteLine(value: $"| {index.PadRight(totalWidth: maxIndexLength - 1)}| {name.PadRight(totalWidth: maxNameLength - 1)}|");
			writer.WriteLine(value: separator);
		}
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a Textile document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As Textile" menu item.</remarks>
	private void SaveListViewResultsAsTextile()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogTextile = new()
		{
			Filter = "Textile files (*.textile)|*.textile|All files (*.*)|*.*",
			DefaultExt = "textile",
			Title = "Save list as Textile"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogTextile, ext: saveFileDialogTextile.DefaultExt))
		{
			return;
		}
		// Write the data to the Textile file
		using StreamWriter writer = new(path: saveFileDialogTextile.FileName, append: false, encoding: Encoding.UTF8);
		// Document title
		writer.WriteLine(value: "h1. List of Readable Designations");
		writer.WriteLine();
		// Write table header
		writer.WriteLine(value: "|_. Index |_. Designation |");
		// Write table rows
		foreach ((string index, string name) in GetExportData())
		{
			// Escape table cell dividers
			string safeName = name.Replace(oldValue: "|", newValue: "&#124;");
			string safeIndex = index.Replace(oldValue: "|", newValue: "&#124;");

			writer.WriteLine(value: $"| {safeIndex} | {safeName} |");
		}
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as an AbiWord document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As AbiWord" menu item.</remarks>
	private void SaveListViewResultsAsAbiword()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogAbiword = new()
		{
			Filter = "AbiWord documents (*.abw)|*.abw|All files (*.*)|*.*",
			DefaultExt = "abw",
			Title = "Save list as AbiWord"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogAbiword, ext: saveFileDialogAbiword.DefaultExt))
		{
			return;
		}
		// Write the data to the AbiWord file
		using StreamWriter writer = new(path: saveFileDialogAbiword.FileName, append: false, encoding: Encoding.UTF8);
		writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		writer.WriteLine(value: "<!DOCTYPE abiword PUBLIC \"-//ABISOURCE//DTD AWML 1.0 Strict//EN\" \"http://www.abisource.com/awml.dtd\">");
		writer.WriteLine(value: "<abiword xmlns:awml=\"http://www.abisource.com/awml.dtd\" version=\"1.9.2\" fileformat=\"1.0\" xmlns=\"http://www.abisource.com/awml.dtd\">");
		writer.WriteLine(value: "  <section>");
		writer.WriteLine(value: "    <p style=\"Heading 1\">List of Readable Designations</p>");
		writer.WriteLine(value: "    <table>");
		int row = 0;
		void WriteCell(string text, int col, int r)
		{
			string safeText = System.Net.WebUtility.HtmlEncode(value: text) ?? string.Empty;
			writer.WriteLine(value: $"      <cell left-attach=\"{col}\" right-attach=\"{col + 1}\" top-attach=\"{r}\" bottom-attach=\"{r + 1}\">");
			writer.WriteLine(value: $"        <p>{safeText}</p>");
			writer.WriteLine(value: "      </cell>");
		}
		WriteCell(text: "Index", col: 0, r: row);
		WriteCell(text: "Designation", col: 1, r: row);
		row++;
		foreach ((string index, string name) in GetExportData())
		{
			WriteCell(text: index, col: 0, r: row);
			WriteCell(text: name, col: 1, r: row);
			row++;
		}
		writer.WriteLine(value: "    </table>");
		writer.WriteLine(value: "  </section>");
		writer.WriteLine(value: "</abiword>");
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a WPS document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As WPS" menu item. It exports the data as HTML since WPS Office natively supports it.</remarks>
	private void SaveListViewResultsAsWps()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogWps = new()
		{
			Filter = "WPS Writer documents (*.wps)|*.wps|All files (*.*)|*.*",
			DefaultExt = "wps",
			Title = "Save list as WPS"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogWps, ext: saveFileDialogWps.DefaultExt))
		{
			return;
		}
		// Write the data to the WPS file (using HTML format internally for compatibility with WPS Office)
		using StreamWriter writer = new(path: saveFileDialogWps.FileName, append: false, encoding: Encoding.UTF8);
		writer.WriteLine(value: "<!DOCTYPE html>");
		writer.WriteLine(value: "<html>");
		writer.WriteLine(value: "<head>");
		writer.WriteLine(value: "<meta charset=\"utf-8\">");
		writer.WriteLine(value: "<title>List of Readable Designations</title>");
		writer.WriteLine(value: "<style>table { border-collapse: collapse; width: 100%; } th, td { border: 1px solid black; padding: 5px; text-align: left; }</style>");
		writer.WriteLine(value: "</head>");
		writer.WriteLine(value: "<body>");
		writer.WriteLine(value: "<h1>List of Readable Designations</h1>");
		writer.WriteLine(value: "<table>");
		writer.WriteLine(value: "<tr><th>Index</th><th>Designation</th></tr>");
		foreach ((string index, string name) in GetExportData())
		{
			string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
			string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
			writer.WriteLine(value: $"<tr><td>{safeIndex}</td><td>{safeName}</td></tr>");
		}
		writer.WriteLine(value: "</table>");
		writer.WriteLine(value: "</body>");
		writer.WriteLine(value: "</html>");
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as an ET spreadsheet document (WPS Spreadsheets).</summary>
	/// <remarks>This method is invoked when the user selects the "Save As ET" menu item. It exports the data as CSV since WPS Spreadsheets natively supports it.</remarks>
	private void SaveListViewResultsAsEt()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogEt = new()
		{
			Filter = "WPS Spreadsheets (*.et)|*.et|All files (*.*)|*.*",
			DefaultExt = "et",
			Title = "Save list as ET"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogEt, ext: saveFileDialogEt.DefaultExt))
		{
			return;
		}
		// Write the data to the ET file (using CSV format internally for compatibility with WPS Spreadsheets)
		using StreamWriter writer = new(path: saveFileDialogEt.FileName, append: false, encoding: Encoding.UTF8);
		writer.WriteLine(value: "Index,Designation");
		foreach ((string index, string name) in GetExportData())
		{
			// Escape quotes in CSV
			string safeIndex = index.Replace(oldValue: "\"", newValue: "\"\"");
			string safeName = name.Replace(oldValue: "\"", newValue: "\"\"");
			writer.WriteLine(value: $"\"{safeIndex}\",\"{safeName}\"");
		}
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a DocBook document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As DocBook" menu item.</remarks>
	private void SaveListViewResultsAsDocBook()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogDocBook = new()
		{
			Filter = "DocBook files (*.xml)|*.xml|All files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save list as DocBook"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogDocBook, ext: saveFileDialogDocBook.DefaultExt))
		{
			return;
		}
		// Write the data to the DocBook file
		XmlWriterSettings settings = new() { Indent = true };
		using XmlWriter writer = XmlWriter.Create(outputFileName: saveFileDialogDocBook.FileName, settings: settings);
		// Write XML document
		writer.WriteStartDocument();
		// DocBook Article root
		writer.WriteStartElement(localName: "article", ns: "http://docbook.org/ns/docbook");
		writer.WriteAttributeString(localName: "version", value: "5.0");
		// Title
		writer.WriteStartElement(localName: "title");
		writer.WriteString(text: "List of Readable Designations");
		writer.WriteEndElement();
		// Section
		writer.WriteStartElement(localName: "section");
		// Table
		writer.WriteStartElement(localName: "table");
		writer.WriteAttributeString(localName: "frame", value: "all");
		writer.WriteStartElement(localName: "title");
		writer.WriteString(text: "Planetoid Designations");
		writer.WriteEndElement();
		writer.WriteStartElement(localName: "tgroup");
		writer.WriteAttributeString(localName: "cols", value: "2");
		writer.WriteStartElement(localName: "colspec");
		writer.WriteAttributeString(localName: "colname", value: "c1");
		writer.WriteEndElement();
		writer.WriteStartElement(localName: "colspec");
		writer.WriteAttributeString(localName: "colname", value: "c2");
		writer.WriteEndElement();
		// Table Header
		writer.WriteStartElement(localName: "thead");
		writer.WriteStartElement(localName: "row");
		writer.WriteStartElement(localName: "entry");
		writer.WriteString(text: "Index");
		writer.WriteEndElement();
		writer.WriteStartElement(localName: "entry");
		writer.WriteString(text: "Designation");
		writer.WriteEndElement();
		writer.WriteEndElement(); // row
		writer.WriteEndElement(); // thead
								  // Table Body
		writer.WriteStartElement(localName: "tbody");
		foreach ((string index, string name) in GetExportData())
		{
			writer.WriteStartElement(localName: "row");
			writer.WriteStartElement(localName: "entry");
			writer.WriteString(text: index);
			writer.WriteEndElement();
			writer.WriteStartElement(localName: "entry");
			writer.WriteString(text: name);
			writer.WriteEndElement();
			writer.WriteEndElement(); // row
		}
		writer.WriteEndElement(); // tbody
		writer.WriteEndElement(); // tgroup
		writer.WriteEndElement(); // table
		writer.WriteEndElement(); // section
		writer.WriteEndElement(); // article
		writer.WriteEndDocument();
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a TOML document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As TOML" menu item.</remarks>
	private void SaveListViewResultsAsToml()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogToml = new()
		{
			Filter = "TOML files (*.toml)|*.toml|All files (*.*)|*.*",
			DefaultExt = "toml",
			Title = "Save list as TOML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogToml, ext: saveFileDialogToml.DefaultExt))
		{
			return;
		}
		// Write the data to the TOML file
		using StreamWriter writer = new(path: saveFileDialogToml.FileName, append: false, encoding: Encoding.UTF8);
		writer.WriteLine(value: "title = \"List of Readable Designations\"");
		writer.WriteLine(value: $"created_at = {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
		writer.WriteLine();
		foreach ((string index, string name) in GetExportData())
		{
			string safeIndex = index.Replace(oldValue: "\\", newValue: "\\\\").Replace(oldValue: "\"", newValue: "\\\"");
			string safeName = name.Replace(oldValue: "\\", newValue: "\\\\").Replace(oldValue: "\"", newValue: "\\\"");
			writer.WriteLine(value: "[[planetoids]]");
			writer.WriteLine(value: $"index = \"{safeIndex}\"");
			writer.WriteLine(value: $"designation = \"{safeName}\"");
			writer.WriteLine();
		}
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as an XPS document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As XPS" menu item.</remarks>
	private void SaveListViewResultsAsXps()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogXps = new()
		{
			Filter = "XML Paper Specification files (*.xps)|*.xps|All files (*.*)|*.*",
			DefaultExt = "xps",
			Title = "Save list as XPS"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogXps, ext: saveFileDialogXps.DefaultExt))
		{
			return;
		}
		// Create the XPS file (which is a ZIP archive)
		using FileStream fs = new(path: saveFileDialogXps.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);

		// 1. [Content_Types].xml
		ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
			writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\" />");
			writer.WriteLine(value: "  <Default Extension=\"fdoc\" ContentType=\"application/vnd.ms-package.xps-fixeddocument+xml\" />");
			writer.WriteLine(value: "  <Default Extension=\"fseq\" ContentType=\"application/vnd.ms-package.xps-fixeddocumentsequence+xml\" />");
			writer.WriteLine(value: "  <Default Extension=\"fpage\" ContentType=\"application/vnd.ms-package.xps-fixedpage+xml\" />");
			writer.WriteLine(value: "</Types>");
		}
		// 2. _rels/.rels
		ZipArchiveEntry rootRelsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: rootRelsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/fixedrepresentation\" Target=\"/FixedDocumentSequence.fseq\" />");
			writer.WriteLine(value: "</Relationships>");
		}
		// 3. FixedDocumentSequence.fseq
		ZipArchiveEntry fseqEntry = archive.CreateEntry(entryName: "FixedDocumentSequence.fseq", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: fseqEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<FixedDocumentSequence xmlns=\"http://schemas.microsoft.com/xps/2005/06\">");
			writer.WriteLine(value: "  <DocumentReference Source=\"/Documents/1/FixedDocument.fdoc\" />");
			writer.WriteLine(value: "</FixedDocumentSequence>");
		}
		// 4. FixedDocumentSequence.fseq.rels
		ZipArchiveEntry fseqRelsEntry = archive.CreateEntry(entryName: "_rels/FixedDocumentSequence.fseq.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: fseqRelsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rdoc1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"/Documents/1/FixedDocument.fdoc\" />");
			writer.WriteLine(value: "</Relationships>");
		}
		// 5. FixedDocument.fdoc
		ZipArchiveEntry fdocEntry = archive.CreateEntry(entryName: "Documents/1/FixedDocument.fdoc", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: fdocEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<FixedDocument xmlns=\"http://schemas.microsoft.com/xps/2005/06\">");
			writer.WriteLine(value: "  <PageContent Source=\"/Documents/1/Pages/1.fpage\" />");
			writer.WriteLine(value: "</FixedDocument>");
		}
		// 6. FixedDocument.fdoc.rels
		ZipArchiveEntry fdocRelsEntry = archive.CreateEntry(entryName: "Documents/1/_rels/FixedDocument.fdoc.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: fdocRelsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rpage1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"/Documents/1/Pages/1.fpage\" />");
			writer.WriteLine(value: "</Relationships>");
		}
		// 7. Pages/1.fpage
		ZipArchiveEntry fpageEntry = archive.CreateEntry(entryName: "Documents/1/Pages/1.fpage", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: fpageEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<FixedPage Width=\"816\" Height=\"1056\" xmlns=\"http://schemas.microsoft.com/xps/2005/06\" xml:lang=\"en-US\">");
			writer.WriteLine(value: "  <Glyphs Fill=\"#ff000000\" FontUri=\"\" FontRenderingEmSize=\"16\" OriginX=\"96\" OriginY=\"96\" UnicodeString=\"List of Readable Designations\" />");
			// We iterate through the data. Warning: Since this is a very simple XPS generator, we write everything on 1 page and just increase Y.
			// A real XPS would need pagination, font embedding, etc. But for this requirement, a simple text dump without font file is a minimalistic approach 
			// (some viewers might fail if FontUri is empty, but XPS supports standard core fonts by fallback in some viewers like Edge/IE).
			int currentY = 120;
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
				writer.WriteLine(value: $"  <Glyphs Fill=\"#ff000000\" FontUri=\"\" FontRenderingEmSize=\"12\" OriginX=\"96\" OriginY=\"{currentY}\" UnicodeString=\"{safeIndex}\" />");
				writer.WriteLine(value: $"  <Glyphs Fill=\"#ff000000\" FontUri=\"\" FontRenderingEmSize=\"12\" OriginX=\"192\" OriginY=\"{currentY}\" UnicodeString=\"{safeName}\" />");
				currentY += 16;
			}
			writer.WriteLine(value: "</FixedPage>");
		}
		// Note on XPS minimal compliance: A truly compliant XPS demands an interleaved generic TTF/ODTTF font and its rels binding.
		// However, without external packages, providing a full TTF stream manually is huge. The above is the minimum markup structure.
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a FictionBook2 document.</summary>
	/// <remarks>This method is invoked when the user selects the "Save As FictionBook2" menu item.</remarks>
	private void SaveListViewResultsAsFictionBook2()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogFb2 = new()
		{
			Filter = "FictionBook2 files (*.fb2)|*.fb2|All files (*.*)|*.*",
			DefaultExt = "fb2",
			Title = "Save list as FictionBook2"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogFb2, ext: saveFileDialogFb2.DefaultExt))
		{
			return;
		}
		// Write the data to the FictionBook2 file
		XmlWriterSettings settings = new() { Indent = true, Encoding = Encoding.UTF8 };
		using XmlWriter writer = XmlWriter.Create(outputFileName: saveFileDialogFb2.FileName, settings: settings);
		string fb2Ns = "http://www.gribuser.ru/xml/fictionbook/2.0";
		writer.WriteStartDocument();
		writer.WriteStartElement(localName: "FictionBook", ns: fb2Ns);
		writer.WriteAttributeString(prefix: "xmlns", localName: "l", ns: null, value: "http://www.w3.org/1999/xlink");
		// description
		writer.WriteStartElement(localName: "description", ns: fb2Ns);
		// title-info
		writer.WriteStartElement(localName: "title-info", ns: fb2Ns);
		writer.WriteElementString(localName: "genre", ns: fb2Ns, value: "reference");
		writer.WriteStartElement(localName: "author", ns: fb2Ns);
		writer.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
		writer.WriteElementString(localName: "last-name", ns: fb2Ns, value: "");
		writer.WriteEndElement(); // author
		writer.WriteElementString(localName: "book-title", ns: fb2Ns, value: "List of Readable Designations");
		writer.WriteElementString(localName: "lang", ns: fb2Ns, value: "en");
		writer.WriteEndElement(); // title-info
								  // document-info
		writer.WriteStartElement(localName: "document-info", ns: fb2Ns);
		writer.WriteStartElement(localName: "author", ns: fb2Ns);
		writer.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
		writer.WriteElementString(localName: "last-name", ns: fb2Ns, value: "");
		writer.WriteEndElement(); // author
		writer.WriteElementString(localName: "program-used", ns: fb2Ns, value: "Planetoid-DB");
		writer.WriteStartElement(localName: "date", ns: fb2Ns);
		writer.WriteAttributeString(localName: "value", value: DateTime.Now.ToString(format: "yyyy-MM-dd"));
		writer.WriteString(text: DateTime.Now.ToString(format: "yyyy-MM-dd"));
		writer.WriteEndElement(); // date
		writer.WriteElementString(localName: "id", ns: fb2Ns, value: Guid.NewGuid().ToString());
		writer.WriteElementString(localName: "version", ns: fb2Ns, value: "1.0");
		writer.WriteEndElement(); // document-info
		writer.WriteEndElement(); // description
								  // body
		writer.WriteStartElement(localName: "body", ns: fb2Ns);
		writer.WriteStartElement(localName: "title", ns: fb2Ns);
		writer.WriteStartElement(localName: "p", ns: fb2Ns);
		writer.WriteString(text: "List of Readable Designations");
		writer.WriteEndElement(); // p
		writer.WriteEndElement(); // title
		writer.WriteStartElement(localName: "section", ns: fb2Ns);
		writer.WriteStartElement(localName: "table", ns: fb2Ns);
		writer.WriteStartElement(localName: "tr", ns: fb2Ns);
		writer.WriteStartElement(localName: "th", ns: fb2Ns);
		writer.WriteString(text: "Index");
		writer.WriteEndElement(); // th
		writer.WriteStartElement(localName: "th", ns: fb2Ns);
		writer.WriteString(text: "Designation");
		writer.WriteEndElement(); // th
		writer.WriteEndElement(); // tr
		foreach ((string index, string name) in GetExportData())
		{
			writer.WriteStartElement(localName: "tr", ns: fb2Ns);
			writer.WriteStartElement(localName: "td", ns: fb2Ns);
			writer.WriteString(text: index);
			writer.WriteEndElement(); // td
			writer.WriteStartElement(localName: "td", ns: fb2Ns);
			writer.WriteString(text: name);
			writer.WriteEndElement(); // td
			writer.WriteEndElement(); // tr
		}
		writer.WriteEndElement(); // table
		writer.WriteEndElement(); // section
		writer.WriteEndElement(); // body
		writer.WriteEndElement(); // FictionBook
		writer.WriteEndDocument();
		// Show success message
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a Compiled HTML Help (CHM) document.</summary>
	/// <remarks>This method generates the necessary HTML and project files, then uses Microsoft HTML Help Workshop to compile them.</remarks>
	private void SaveListViewResultsAsChm()
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogChm = new()
		{
			Filter = "Compiled HTML Help files (*.chm)|*.chm|All files (*.*)|*.*",
			DefaultExt = "chm",
			Title = "Save list as CHM"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogChm, ext: saveFileDialogChm.DefaultExt))
		{
			return;
		}
		// Find hhc.exe
		string hhcPath = Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ProgramFilesX86), path2: @"HTML Help Workshop\hhc.exe");
		if (!File.Exists(path: hhcPath))
		{
			_ = MessageBox.Show(text: "Microsoft HTML Help Workshop is not installed or not found at the default location. Cannot compile CHM file.", caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			return;
		}
		// Create a temporary directory
		string tempDir = Path.Combine(path1: Path.GetTempPath(), path2: Guid.NewGuid().ToString());
		Directory.CreateDirectory(path: tempDir);
		try
		{
			string htmlPath = Path.Combine(path1: tempDir, path2: "index.html");
			string hhcFilePath = Path.Combine(path1: tempDir, path2: "toc.hhc");
			string hhpPath = Path.Combine(path1: tempDir, path2: "project.hhp");
			string chmTempPath = Path.Combine(path1: tempDir, path2: "project.chm");
			// 1. Generate HTML
			using (StreamWriter writer = new(path: htmlPath, append: false, encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>List of Readable Designations</title><style>table { border-collapse: collapse; width: 100%; } th, td { border: 1px solid #000; padding: 5px; text-align: left; }</style></head><body>");
				writer.WriteLine(value: "<h1>List of Readable Designations</h1>");
				writer.WriteLine(value: "<table><tr><th>Index</th><th>Designation</th></tr>");
				foreach ((string index, string name) in GetExportData())
				{
					string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
					string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
					writer.WriteLine(value: $"<tr><td>{safeIndex}</td><td>{safeName}</td></tr>");
				}
				writer.WriteLine(value: "</table></body></html>");
			}
			// 2. Generate TOC (.hhc)
			using (StreamWriter writer = new(path: hhcFilePath, append: false, encoding: Encoding.ASCII))
			{
				writer.WriteLine(value: "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">");
				writer.WriteLine(value: "<HTML><HEAD><meta name=\"GENERATOR\" content=\"Planetoid-DB\"></HEAD><BODY>");
				writer.WriteLine(value: "<OBJECT type=\"text/site properties\"><param name=\"ImageType\" value=\"Folder\"></OBJECT>");
				writer.WriteLine(value: "<UL>");
				writer.WriteLine(value: "<LI><OBJECT type=\"text/sitemap\">");
				writer.WriteLine(value: "<param name=\"Name\" value=\"List of Readable Designations\">");
				writer.WriteLine(value: "<param name=\"Local\" value=\"index.html\">");
				writer.WriteLine(value: "</OBJECT>");
				writer.WriteLine(value: "</UL></BODY></HTML>");
			}
			// 3. Generate Project (.hhp)
			using (StreamWriter writer = new(path: hhpPath, append: false, encoding: Encoding.ASCII))
			{
				writer.WriteLine(value: "[OPTIONS]");
				writer.WriteLine(value: "Compatibility=1.1 or later");
				writer.WriteLine(value: "Compiled file=project.chm");
				writer.WriteLine(value: "Contents file=toc.hhc");
				writer.WriteLine(value: "Default topic=index.html");
				writer.WriteLine(value: "Display compile progress=No");
				writer.WriteLine(value: "Language=0x409 English (United States)");
				writer.WriteLine(value: "Title=List of Readable Designations");
				writer.WriteLine(value: "");
				writer.WriteLine(value: "[FILES]");
				writer.WriteLine(value: "index.html");
			}
			// 4. Compile using hhc.exe
			ProcessStartInfo startInfo = new()
			{
				FileName = hhcPath,
				Arguments = $"\"{hhpPath}\"",
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false
			};
			using (Process? process = Process.Start(startInfo: startInfo))
			{
				process?.WaitForExit();
			}
			// 5. Copy the CHM to the destination
			if (File.Exists(path: chmTempPath))
			{
				File.Copy(sourceFileName: chmTempPath, destFileName: saveFileDialogChm.FileName, overwrite: true);
				_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			else
			{
				_ = MessageBox.Show(text: "Failed to compile the CHM file.", caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error saving as CHM.");
			_ = MessageBox.Show(text: $"Error saving as CHM: {ex.Message}", caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		finally
		{
			// Clean up temporary directory
			if (Directory.Exists(path: tempDir))
			{
				Directory.Delete(path: tempDir, recursive: true);
			}
		}
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
		labelInformation.Enabled = listView.Visible = toolStripButtonLoad.Enabled = toolStripDropDownButtonSaveList.Enabled = false;
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

	/// <summary>Handles the Click event of the Load button on the tool strip, initiating the selection of a planetoid and,
	/// when successful, closing the current form.</summary>
	/// <param name="sender">The source of the event, typically the Load button on the tool strip.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the Load button is clicked, this method calls the SelectPlanetoidInMainForm method to navigate to the selected planetoid record in the main form. After initiating the selection, it closes the current form to return control to the main form, and sets the dialog result to <see cref="DialogResult.OK"/> to signal a successful selection.</remarks>
	private void ToolStripButtonLoad_Click(object sender, EventArgs e)
	{
		// Select the planetoid in the main form
		SelectPlanetoidInMainForm();
		// Set the dialog result to OK and close the form
		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>Saves the current list as a CSV file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As CSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogCsv = new()
		{
			Filter = "Comma-separated values files (*.csv)|*.csv|All files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save list as CSV"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogCsv, ext: saveFileDialogCsv.DefaultExt))
		{
			return;
		}
		// Write the data to the CSV file
		using StreamWriter streamWriter = new(path: saveFileDialogCsv.FileName, append: false, encoding: Encoding.UTF8);
		foreach ((string? index, string? name) in GetExportData())
		{
			streamWriter.WriteLine(value: $"{index}; {name}");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an HTML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As HTML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogHtml = new()
		{
			Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save list as HTML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogHtml, ext: saveFileDialogHtml.DefaultExt))
		{
			return;
		}
		// Write the data to the HTML file
		using StreamWriter w = new(path: saveFileDialogHtml.FileName, append: false, encoding: Encoding.UTF8);
		// Write HTML header
		w.WriteLine(value: "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\"><title>Planetoid List</title>");
		w.WriteLine(value: "<style>body{font-family:sans-serif;} .idx{font-weight:bold;display:inline-block;width:60px;}</style></head><body>");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			w.WriteLine(value: $"<div><span class=\"idx\">{index}:</span> <span>{name}</span></div>");
		}
		// Write HTML footer
		w.WriteLine(value: "</body></html>");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an XML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As XML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogXml = new()
		{
			Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save list as XML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogXml, ext: saveFileDialogXml.DefaultExt))
		{
			return;
		}
		// Write the data to the XML file
		XmlWriterSettings settings = new() { Indent = true };
		using XmlWriter writer = XmlWriter.Create(outputFileName: saveFileDialogXml.FileName, settings: settings);
		// Write XML document
		writer.WriteStartDocument();
		writer.WriteStartElement(localName: "ListReadableDesignations", ns: "https://planetoid-db.de");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			writer.WriteStartElement(localName: "item");
			writer.WriteAttributeString(localName: "index", value: index);
			writer.WriteAttributeString(localName: "name", value: name);
			writer.WriteEndElement();
		}
		// End XML document
		writer.WriteEndElement();
		writer.WriteEndDocument();
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a JSON file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As JSON" menu item.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogJson = new()
		{
			Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save list as JSON"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogJson, ext: saveFileDialogJson.DefaultExt))
		{
			return;
		}
		// Prepare the export data
		var exportList = GetExportData().Select(selector: static x => new { x.Index, Designation = x.Name });
		// Serialize to JSON and write to file
		string jsonString = JsonSerializer.Serialize(value: exportList, options: new() { WriteIndented = true });
		File.WriteAllText(path: saveFileDialogJson.FileName, contents: jsonString);
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a SQL script.
	/// Exports the list as a series of SQL INSERT statements.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As SQL" menu item.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogSql = new()
		{
			Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save list as SQL"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogSql, ext: saveFileDialogSql.DefaultExt))
		{
			return;
		}
		// Write the data to the SQL file
		using StreamWriter streamWriter = new(path: saveFileDialogSql.FileName, append: false, encoding: Encoding.UTF8);
		// Define the table name
		string tableName = "Planetoids";
		// Write SQL header; Metadata and table creation (optional, but helpful)
		streamWriter.WriteLine(value: $"-- Export generated by Planetoid-DB");
		streamWriter.WriteLine(value: $"-- Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
		streamWriter.WriteLine();
		// Create table statement
		streamWriter.WriteLine(value: $"CREATE TABLE IF NOT EXISTS {tableName} (");
		streamWriter.WriteLine(value: $"    Id VARCHAR(20) PRIMARY KEY,");
		streamWriter.WriteLine(value: $"    Designation VARCHAR(255)");
		streamWriter.WriteLine(value: $"  );");
		streamWriter.WriteLine();
		// Start transaction (significantly speeds up import!)
		streamWriter.WriteLine(value: $"BEGIN TRANSACTION;");
		// Iterate over data (uses helper method from previous code)
		foreach ((string? index, string? name) in GetExportData())
		{
			// IMPORTANT: SQL Escaping for apostrophes (e.g. "O'Neil" -> "O''Neil")
			string safeIndex = index.Replace(oldValue: "'", newValue: "''");
			string safeName = name.Replace(oldValue: "'", newValue: "''");
			streamWriter.WriteLine(value: $"INSERT INTO {tableName} (Id, Designation) VALUES ('{safeIndex}', '{safeName}');");
		}
		// Commit transaction
		streamWriter.WriteLine(value: $"COMMIT;");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a Markdown table.
	/// Ideal for documentation, GitHub Readmes, or Wikis.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As Markdown" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogMarkdown = new()
		{
			Filter = "Markdown files (*.md)|*.md|All files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save list as Markdown"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMarkdown, ext: saveFileDialogMarkdown.DefaultExt))
		{
			return;
		}
		// Write the data to the Markdown file
		using StreamWriter streamWriter = new(path: saveFileDialogMarkdown.FileName, append: false, encoding: Encoding.UTF8);
		// Write Markdown table header
		streamWriter.WriteLine(value: "| Index | Readable designation |");
		// :--- means left-aligned
		streamWriter.WriteLine(value: "| :--- | :--- |");
		// Write each item as a table row
		foreach ((string? index, string? name) in GetExportData())
		{
			streamWriter.WriteLine(value: $"| {index} | {EscapeMarkdownCell(value: name)} |");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list in YAML format.
	/// A human-readable data serialization standard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As YAML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogYaml = new()
		{
			Filter = "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save list as YAML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogYaml, ext: saveFileDialogYaml.DefaultExt))
		{
			return;
		}
		// Write the data to the YAML file
		using StreamWriter streamWriter = new(path: saveFileDialogYaml.FileName, append: false, encoding: Encoding.UTF8);
		// Write YAML document header
		streamWriter.WriteLine(value: "---"); // Start of YAML document
		streamWriter.WriteLine(value: "# List of Readable Designations");
		streamWriter.WriteLine(value: $"created_at: \"{DateTime.Now:O}\"");
		streamWriter.WriteLine(value: "planetoids:");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			// YAML uses indentation (spaces) instead of brackets.
			streamWriter.WriteLine(value: "  - item:");
			streamWriter.WriteLine(value: $"      index: \"{index}\"");
			// Quotes are important if the name contains special characters
			streamWriter.WriteLine(value: $"      name: \"{name}\"");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a TSV (Tab-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As TSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogTsv = new()
		{
			Filter = "Tab-separated values files (*.tsv)|*.tsv|All files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save list as TSV"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogTsv, ext: saveFileDialogTsv.DefaultExt))
		{
			return;
		}
		// Write the data to the TSV file
		using StreamWriter streamWriter = new(path: saveFileDialogTsv.FileName, append: false, encoding: Encoding.UTF8);
		// Write TSV header
		streamWriter.WriteLine(value: "Index\tDesignation");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			streamWriter.WriteLine(value: $"{index}\t{name}");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a PSV (Pipe-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogPsv = new()
		{
			Filter = "Pipe-separated values files (*.psv)|*.psv|All files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save list as PSV"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPsv, ext: saveFileDialogPsv.DefaultExt))
		{
			return;
		}
		// Write the data to the PSV file
		using StreamWriter streamWriter = new(path: saveFileDialogPsv.FileName, append: false, encoding: Encoding.UTF8);
		// Write PSV header
		streamWriter.WriteLine(value: "Index|Designation");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			streamWriter.WriteLine(value: $"{index}|{name}");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the list as a LaTeX document.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As LaTeX" menu item.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogLatex = new()
		{
			Filter = "LaTeX files (*.tex)|*.tex|All files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save list as LaTeX"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialogLatex, ext: saveFileDialogLatex.DefaultExt))
		{
			return;
		}
		// Write the data to the LaTeX file
		using StreamWriter w = new(path: saveFileDialogLatex.FileName, append: false, encoding: Encoding.UTF8);
		// Write LaTeX document header
		w.WriteLine(value: "\\documentclass{article}");
		w.WriteLine(value: "\\usepackage[utf8]{inputenc}");
		w.WriteLine(value: "\\begin{document}");
		w.WriteLine(value: "\\begin{table}[h!]");
		w.WriteLine(value: "\\centering");
		// Definition: 2 columns, left-aligned (l) with a vertical line between them
		w.WriteLine(value: "\\begin{tabular}{|l|l|}");
		w.WriteLine(value: "\\hline");
		w.WriteLine(value: "Index & Designation \\\\");
		w.WriteLine(value: "\\hline");
		// Write each item
		foreach ((string? index, string? name) in GetExportData())
		{
			// LaTeX uses & as a separator and \\ for new lines
			// IMPORTANT: If names contain special characters like _ or %, they must be escaped.
			string escapedName = EscapeLatex(input: name);
			w.WriteLine(value: $"{index} & {escapedName} \\\\");
		}
		// Write table footer
		w.WriteLine(value: "\\hline");
		w.WriteLine(value: "\\end{tabular}");
		w.WriteLine(value: "\\caption{List of Readable Designations}");
		w.WriteLine(value: "\\end{table}");
		w.WriteLine(value: "\\end{document}");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a PostScript (.ps) file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PostScript" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogPostScript = new()
		{
			Filter = "PostScript files (*.ps)|*.ps|All files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save list as PostScript"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPostScript, ext: saveFileDialogPostScript.DefaultExt))
		{
			return;
		}
		// Write the data to the PostScript file
		using StreamWriter writer = new(path: saveFileDialogPostScript.FileName, append: false, encoding: Encoding.ASCII);
		// Constants for page layout
		const int pageHeight = 842; // A4 height in points
		const int marginTop = 50;
		const int marginBottom = 50;
		const int startY = pageHeight - marginTop;
		const int lineHeight = 14;
		int currentY = startY;
		int pageNumber = 1;
		// Write PostScript header
		writer.WriteLine(value: "%!PS-Adobe-3.0");
		writer.WriteLine(value: "%%Title: Planetoid List");
		writer.WriteLine(value: "%%Creator: Planetoid-DB");
		writer.WriteLine(value: "%%Pages: (atend)");
		writer.WriteLine(value: "%%PageOrder: Ascend");
		writer.WriteLine(value: "%%EndComments");
		// Font definitions (macros for shorter code)
		writer.WriteLine(value: "/FHeader { /Helvetica-Bold findfont 12 scalefont setfont } bind def");
		writer.WriteLine(value: "/FBody { /Helvetica findfont 10 scalefont setfont } bind def");
		// Function to write new page header
		static void WritePageHeader(StreamWriter w, int pageNum)
		{
			w.WriteLine(value: $"%%Page: {pageNum} {pageNum}");
			// Title
			w.WriteLine(value: "FHeader");
			w.WriteLine(value: $"50 {pageHeight - 30} moveto (List of Readable Designations - Page {pageNum}) show");
			// Column headers
			w.WriteLine(value: "50 {pageHeight - 50} moveto (Index) show");
			w.WriteLine(value: "120 {pageHeight - 50} moveto (Designation) show");
			// Separator line
			w.WriteLine(value: "50 {pageHeight - 55} moveto 500 0 rlineto stroke");
			// Activate body font
			w.WriteLine(value: "FBody");
		}
		// Start first page
		WritePageHeader(w: writer, pageNum: pageNumber);
		currentY = startY - 30; // Place below header
								// --- Iterate data ---
		foreach ((string Index, string Name) in GetExportData())
		{
			// Check if we are at the bottom (page break)
			if (currentY < marginBottom)
			{
				writer.WriteLine(value: "showpage"); // Finish page
				pageNumber++;
				WritePageHeader(w: writer, pageNum: pageNumber); // Start new page
				currentY = startY - 30;
			}
			// Write data
			// moveto x y: Move the cursor
			// show: Displays the text in parentheses
			string safeIndex = EscapePostScript(input: Index);
			string safeName = EscapePostScript(input: Name);
			writer.WriteLine(value: $"50 {currentY} moveto ({safeIndex}) show");
			writer.WriteLine(value: $"120 {currentY} moveto ({safeName}) show");
			currentY -= lineHeight;
		}
		// --- Finish document ---
		writer.WriteLine(value: "showpage"); // Finish last page
		writer.WriteLine(value: "%%Trailer");
		writer.WriteLine(value: $"%%Pages: {pageNumber}");
		writer.WriteLine(value: "%%EOF");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an uncompressed PDF file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PDF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogPdf = new()
		{
			Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save list as PDF"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPdf, ext: saveFileDialogPdf.DefaultExt))
		{
			return;
		}
		// Write directly to the FileStream to obtain exact byte positions for the XREF table.
		using FileStream fs = new(path: saveFileDialogPdf.FileName, mode: FileMode.Create);
		// PDF based on PDF 1.4 specification
		using StreamWriter w = new(stream: fs, encoding: Encoding.ASCII);
		// Tracking object offsets for XREF table
		List<long> objectOffsets = [];
		// Helper function: Writes an object and remembers the offset
		// Return value is the object ID (Index + 1)
		int StartNewObject()
		{
			w.Flush();
			objectOffsets.Add(item: fs.Position);
			int id = objectOffsets.Count;
			w.WriteLine(value: $"{id} 0 obj");
			return id;
		}
		// --- 1. header ---
		w.WriteLine(value: "%PDF-1.4");
		// binary comment line
		w.WriteLine(value: "%µµµµ");
		// --- 2. preparation of content ---
		// First, we generate the content streams (the text) for all pages.
		// We store the object IDs of the content streams to later associate them with the pages.
		List<int> pageContentObjIds = [];
		// Constants for page layout
		const int pageHeight = 842; // A4
		const int startY = 750;
		const int marginY = 50;
		const int lineHeight = 14;
		// Current writing position
		int currentY = startY;
		int currentContentObjId = 0;
		// Start first content stream
		currentContentObjId = StartNewObject();
		pageContentObjIds.Add(item: currentContentObjId);
		// Start the stream dictionary; Length is omitted here, consistent with subsequent pages.
		w.WriteLine(value: "<< >> stream");
		// PDF commands to set font and size; 0,0 is bottom-left
		// BT = Begin Text, /F1 = Font, 10 = Size
		w.WriteLine(value: "BT /F1 10 Tf");
		// Header for the first page
		w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 40} Tm (List of Readable Designations) Tj");
		w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 60} Tm (Index) Tj");
		w.WriteLine(value: $"1 0 0 1 120 {pageHeight - 60} Tm (Designation) Tj");
		// Move down to start position
		foreach ((string Index, string Name) in GetExportData())
		{
			// Check if we need a new page
			if (currentY < marginY)
			{
				// Close current stream
				w.WriteLine(value: "ET"); // End Text
				w.WriteLine(value: "endstream");
				w.WriteLine(value: "endobj");
				// Start new page
				currentContentObjId = StartNewObject();
				pageContentObjIds.Add(item: currentContentObjId);
				w.WriteLine(value: "<< >> stream");
				w.WriteLine(value: "BT /F1 10 Tf");
				// Header for the new page
				w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 40} Tm (List of Readable Designations - Cont.) Tj");
				w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 60} Tm (Index) Tj");
				w.WriteLine(value: $"1 0 0 1 120 {pageHeight - 60} Tm (Designation) Tj");
				currentY = startY;
			}
			// Write the actual data
			// Td = Move text position (relativ), aber wir nutzen Tm (Matrix) für absolute Positionierung hier einfacher
			// Index
			w.WriteLine(value: $"1 0 0 1 50 {currentY} Tm ({EscapePdf(text: Index)}) Tj");
			// Name
			w.WriteLine(value: $"1 0 0 1 120 {currentY} Tm ({EscapePdf(text: Name)}) Tj");
			currentY -= lineHeight;
		}
		// Close last stream
		w.WriteLine(value: "ET");
		w.WriteLine(value: "endstream");
		w.WriteLine(value: "endobj");
		// --- 3. Create Page Objects ---
		// Each page is its own object, pointing to its content stream
		List<int> pageObjIds = [];
		// Create a Page Object for each content stream
		foreach (int contentId in pageContentObjIds)
		{
			// Create Page Object
			int pageId = StartNewObject();
			pageObjIds.Add(item: pageId);
			w.WriteLine(value: "<<");
			w.WriteLine(value: "/Type /Page");
			// Parent will be referenced via a forward reference to the Pages root object.
			// The Pages root object is written after all page objects and the font object.
			// We calculate the ID of that parent object based on the number of remaining page objects and the font.
			int predictedParentId = objectOffsets.Count + (pageContentObjIds.Count - pageObjIds.Count) + 2;
			// Write Parent reference
			w.WriteLine(value: $"/Parent {predictedParentId} 0 R");
			w.WriteLine(value: "/MediaBox [0 0 595 842]"); // A4
			w.WriteLine(value: $"/Contents {contentId} 0 R");
			w.WriteLine(value: $"/Resources << /Font << /F1 {predictedParentId + 1} 0 R >> >>"); // Ref on Font object
			w.WriteLine(value: ">>");
			w.WriteLine(value: "endobj");
		}
		// --- 4. Pages Root Object (The "Parent") ---
		int pagesRootId = StartNewObject(); // Here we land at 'predictedParentId'
		w.WriteLine(value: "<<");
		w.WriteLine(value: "/Type /Pages");
		w.Write(value: "/Kids [");
		// List all page object references
		foreach (int pid in pageObjIds)
		{
			w.Write(value: $"{pid} 0 R ");
		}
		// Close Kids array
		w.WriteLine(value: "]");
		w.WriteLine(value: ">>");
		w.WriteLine(value: "endobj");
		// --- 5. Font Object ---
		int fontId = StartNewObject();
		w.WriteLine(value: "<<");
		w.WriteLine(value: "/Type /Font");
		w.WriteLine(value: "/Subtype /Type1");
		w.WriteLine(value: "/BaseFont /Helvetica");
		w.WriteLine(value: ">>");
		w.WriteLine(value: "endobj");
		// --- 6. Catalog Object ---
		int catalogId = StartNewObject();
		w.WriteLine(value: "<<");
		w.WriteLine(value: "/Type /Catalog");
		w.WriteLine(value: $"/Pages {pagesRootId} 0 R");
		w.WriteLine(value: ">>");
		w.WriteLine(value: "endobj");
		// --- 7. Cross-Reference Table (XREF) ---
		w.Flush();
		long xrefOffset = fs.Position;
		w.WriteLine(value: "xref");
		// +1 for entry 0
		w.WriteLine(value: $"0 {objectOffsets.Count + 1}");
		// Entry 0 is always free
		w.WriteLine(value: "0000000000 65535 f ");
		// Write each object offset
		foreach (long offset in objectOffsets)
		{
			// Format: 10 digits, zero-padded
			w.WriteLine(value: $"{offset:D10} 00000 n ");
		}
		// --- 8. Trailer ---
		w.WriteLine(value: "trailer");
		w.WriteLine(value: "<<");
		w.WriteLine(value: $"/Size {objectOffsets.Count + 1}");
		w.WriteLine(value: $"/Root {catalogId} 0 R");
		w.WriteLine(value: ">>");
		w.WriteLine(value: "startxref");
		w.WriteLine(value: xrefOffset);
		w.WriteLine(value: "%%EOF");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an EPUB file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As EPUB" menu item.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogEpub = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save list as EPUB"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogEpub, ext: saveFileDialogEpub.DefaultExt))
		{
			return;
		}
		// Create the EPUB file
		using FileStream fs = new(path: saveFileDialogEpub.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
		// 1. mimetype (must be first and uncompressed)
		ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
		using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
		{
			writer.Write(value: "application/epub+zip");
		}
		// 2. META-INF/container.xml
		ZipArchiveEntry containerEntry = archive.CreateEntry(entryName: "META-INF/container.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: containerEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\"?>");
			writer.WriteLine(value: "<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">");
			writer.WriteLine(value: "  <rootfiles>");
			writer.WriteLine(value: "    <rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/>");
			writer.WriteLine(value: "  </rootfiles>");
			writer.WriteLine(value: "</container>");
		}
		// 3. OEBPS/content.opf
		ZipArchiveEntry opfEntry = archive.CreateEntry(entryName: "OEBPS/content.opf", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: opfEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<package xmlns=\"http://www.idpf.org/2007/opf\" unique-identifier=\"BookId\" version=\"2.0\">");
			writer.WriteLine(value: "  <metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">");
			writer.WriteLine(value: "    <dc:title>Planetoid List</dc:title>");
			writer.WriteLine(value: "    <dc:language>en</dc:language>");
			writer.WriteLine(value: "    <dc:identifier id=\"BookId\" opf:scheme=\"UUID\">urn:uuid:12345678-1234-1234-1234-123456789012</dc:identifier>");
			writer.WriteLine(value: "    <dc:description>List of readable designations from the planetoids database.</dc:description>");
			writer.WriteLine(value: "    <dc:creator>Planetoid-DB</dc:creator>");
			writer.WriteLine(value: "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>");
			writer.WriteLine(value: "  </metadata>");
			writer.WriteLine(value: "  <manifest>");
			writer.WriteLine(value: "    <item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/>");
			writer.WriteLine(value: "    <item id=\"content\" href=\"content.xhtml\" media-type=\"application/xhtml+xml\"/>");
			writer.WriteLine(value: "  </manifest>");
			writer.WriteLine(value: "  <spine toc=\"ncx\">");
			writer.WriteLine(value: "    <itemref idref=\"content\"/>");
			writer.WriteLine(value: "  </spine>");
			writer.WriteLine(value: "</package>");
		}
		// 4. OEBPS/toc.ncx
		ZipArchiveEntry ncxEntry = archive.CreateEntry(entryName: "OEBPS/toc.ncx", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: ncxEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<ncx xmlns=\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\">");
			writer.WriteLine(value: "  <head>");
			writer.WriteLine(value: "    <meta name=\"dtb:uid\" content=\"urn:uuid:12345678-1234-1234-1234-123456789012\"/>");
			writer.WriteLine(value: "    <meta name=\"dtb:depth\" content=\"1\"/>");
			writer.WriteLine(value: "    <meta name=\"dtb:totalPageCount\" content=\"0\"/>");
			writer.WriteLine(value: "    <meta name=\"dtb:maxPageNumber\" content=\"0\"/>");
			writer.WriteLine(value: "  </head>");
			writer.WriteLine(value: "  <docTitle><text>Planetoid List</text></docTitle>");
			writer.WriteLine(value: "  <navMap>");
			writer.WriteLine(value: "    <navPoint id=\"navPoint-1\" playOrder=\"1\">");
			writer.WriteLine(value: "      <navLabel><text>List</text></navLabel>");
			writer.WriteLine(value: "      <content src=\"content.xhtml\"/>");
			writer.WriteLine(value: "    </navPoint>");
			writer.WriteLine(value: "  </navMap>");
			writer.WriteLine(value: "</ncx>");
		}

		// 5. OEBPS/content.xhtml
		ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "OEBPS/content.xhtml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
			writer.WriteLine(value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			writer.WriteLine(value: "<head>");
			writer.WriteLine(value: "  <title>Planetoid List</title>");
			writer.WriteLine(value: "  <style type=\"text/css\">");
			writer.WriteLine(value: "    body { font-family: sans-serif; }");
			writer.WriteLine(value: "    table { border-collapse: collapse; width: 100%; }");
			writer.WriteLine(value: "    th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
			writer.WriteLine(value: "    th { background-color: #f2f2f2; }");
			writer.WriteLine(value: "  </style>");
			writer.WriteLine(value: "</head>");
			writer.WriteLine(value: "<body>");
			writer.WriteLine(value: "  <h1>List of Readable Designations</h1>");
			writer.WriteLine(value: "  <table>");
			writer.WriteLine(value: "    <thead>");
			writer.WriteLine(value: "      <tr><th>Index</th><th>Designation</th></tr>");
			writer.WriteLine(value: "    </thead>");
			writer.WriteLine(value: "    <tbody>");
			// Use GetExportData() to retrieve items
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
				writer.WriteLine(value: $"      <tr><td>{safeIndex}</td><td>{safeName}</td></tr>");
			}
			writer.WriteLine(value: "    </tbody>");
			writer.WriteLine(value: "  </table>");
			writer.WriteLine(value: "</body>");
			writer.WriteLine(value: "</html>");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a Word document.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Word" menu item.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogWord = new()
		{
			Filter = "Word documents (*.docx)|*.docx|All files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save list as Word"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogWord, ext: saveFileDialogWord.DefaultExt))
		{
			return;
		}
		// Create the Word file
		using FileStream fs = new(path: saveFileDialogWord.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
		// 1. [Content_Types].xml
		ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
			writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
			writer.WriteLine(value: "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
			writer.WriteLine(value: "  <Override PartName=\"/word/document.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\"/>");
			writer.WriteLine(value: "</Types>");
		}
		// 2. _rels/.rels
		ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"word/document.xml\"/>");
			writer.WriteLine(value: "</Relationships>");
		}
		// 3. word/document.xml
		ZipArchiveEntry documentEntry = archive.CreateEntry(entryName: "word/document.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: documentEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<w:document xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
			writer.WriteLine(value: "  <w:body>");
			// Title
			writer.WriteLine(value: "    <w:p><w:pPr><w:pStyle w:val=\"Title\"/></w:pPr><w:r><w:t>List of Readable Designations</w:t></w:r></w:p>");
			// Table
			writer.WriteLine(value: "    <w:tbl>");
			writer.WriteLine(value: "      <w:tblPr>");
			writer.WriteLine(value: "        <w:tblStyle w:val=\"TableGrid\"/>");
			writer.WriteLine(value: "        <w:tblW w:w=\"0\" w:type=\"auto\"/>");
			writer.WriteLine(value: "        <w:tblBorders>");
			writer.WriteLine(value: "          <w:top w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "          <w:left w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "          <w:bottom w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "          <w:right w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "          <w:insideH w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "          <w:insideV w:val=\"single\" w:sz=\"4\" w:space=\"0\" w:color=\"auto\"/>");
			writer.WriteLine(value: "        </w:tblBorders>");
			writer.WriteLine(value: "      </w:tblPr>");
			// Header Row
			writer.WriteLine(value: "      <w:tr>");
			writer.WriteLine(value: "        <w:tc><w:p><w:r><w:t>Index</w:t></w:r></w:p></w:tc>");
			writer.WriteLine(value: "        <w:tc><w:p><w:r><w:t>Designation</w:t></w:r></w:p></w:tc>");
			writer.WriteLine(value: "      </w:tr>");
			// Data Rows
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
				writer.Write(value: "      <w:tr>");
				writer.Write(value: $"<w:tc><w:p><w:r><w:t>{safeIndex}</w:t></w:r></w:p></w:tc>");
				writer.Write(value: $"<w:tc><w:p><w:r><w:t>{safeName}</w:t></w:r></w:p></w:tc>");
				writer.WriteLine(value: "</w:tr>");
			}

			writer.WriteLine(value: "    </w:tbl>");
			writer.WriteLine(value: "  </w:body>");
			writer.WriteLine(value: "</w:document>");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an Excel file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Excel" menu item.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogExcel = new()
		{
			Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save list as Excel"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogExcel, ext: saveFileDialogExcel.DefaultExt))
		{
			return;
		}
		// Create the Excel file
		using FileStream fs = new(path: saveFileDialogExcel.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
		// 1. [Content_Types].xml
		ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
			writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
			writer.WriteLine(value: "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
			writer.WriteLine(value: "  <Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>");
			writer.WriteLine(value: "  <Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>");
			writer.WriteLine(value: "</Types>");
		}
		// 2. _rels/.rels
		ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>");
			writer.WriteLine(value: "</Relationships>");
		}
		// 3. xl/workbook.xml
		ZipArchiveEntry workbookEntry = archive.CreateEntry(entryName: "xl/workbook.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: workbookEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
			writer.WriteLine(value: "  <sheets>");
			writer.WriteLine(value: "    <sheet name=\"Planetoids\" sheetId=\"1\" r:id=\"rId1\"/>");
			writer.WriteLine(value: "  </sheets>");
			writer.WriteLine(value: "</workbook>");
		}
		// 4. xl/_rels/workbook.xml.rels
		ZipArchiveEntry workbookRelsEntry = archive.CreateEntry(entryName: "xl/_rels/workbook.xml.rels", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: workbookRelsEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
			writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>");
			writer.WriteLine(value: "</Relationships>");
		}
		// 5. xl/worksheets/sheet1.xml
		ZipArchiveEntry sheetEntry = archive.CreateEntry(entryName: "xl/worksheets/sheet1.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: sheetEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			writer.WriteLine(value: "<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
			writer.WriteLine(value: "  <sheetData>");
			// Header
			writer.WriteLine(value: "    <row>");
			writer.WriteLine(value: "      <c t=\"inlineStr\"><is><t>Index</t></is></c>");
			writer.WriteLine(value: "      <c t=\"inlineStr\"><is><t>Designation</t></is></c>");
			writer.WriteLine(value: "    </row>");
			// Data
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;

				writer.WriteLine(value: "    <row>");
				writer.WriteLine(value: $"      <c t=\"inlineStr\"><is><t>{safeIndex}</t></is></c>");
				writer.WriteLine(value: $"      <c t=\"inlineStr\"><is><t>{safeName}</t></is></c>");
				writer.WriteLine(value: "    </row>");
			}

			writer.WriteLine(value: "  </sheetData>");
			writer.WriteLine(value: "</worksheet>");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an ODT file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODT" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogOdt = new()
		{
			Filter = "OpenDocument Text (*.odt)|*.odt|All files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save list as ODT"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOdt, ext: saveFileDialogOdt.DefaultExt))
		{
			return;
		}
		// Create the ODT file
		using FileStream fs = new(path: saveFileDialogOdt.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
		// 1. mimetype (must be first and uncompressed)
		ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
		using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
		{
			writer.Write(value: "application/vnd.oasis.opendocument.text");
		}
		// 2. META-INF/manifest.xml
		ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.text\"/>");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
			writer.WriteLine(value: "</manifest:manifest>");
		}
		// 3. content.xml
		ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\">");
			writer.WriteLine(value: "  <office:body>");
			writer.WriteLine(value: "    <office:text>");
			// Title
			writer.WriteLine(value: "      <text:h text:outline-level=\"1\">List of Readable Designations</text:h>");
			// Table
			writer.WriteLine(value: "      <table:table>");
			writer.WriteLine(value: "        <table:table-column table:number-columns-repeated=\"2\"/>");
			// Header Row
			writer.WriteLine(value: "        <table:table-header-rows>");
			writer.WriteLine(value: "          <table:table-row>");
			writer.WriteLine(value: "            <table:table-cell><text:p>Index</text:p></table:table-cell>");
			writer.WriteLine(value: "            <table:table-cell><text:p>Designation</text:p></table:table-cell>");
			writer.WriteLine(value: "          </table:table-row>");
			writer.WriteLine(value: "        </table:table-header-rows>");
			// Data Rows
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;

				writer.WriteLine(value: "        <table:table-row>");
				writer.WriteLine(value: $"          <table:table-cell><text:p>{safeIndex}</text:p></table:table-cell>");
				writer.WriteLine(value: $"          <table:table-cell><text:p>{safeName}</text:p></table:table-cell>");
				writer.WriteLine(value: "        </table:table-row>");
			}
			writer.WriteLine(value: "      </table:table>");
			writer.WriteLine(value: "    </office:text>");
			writer.WriteLine(value: "  </office:body>");
			writer.WriteLine(value: "</office:document-content>");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an ODS file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODS" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogOds = new()
		{
			Filter = "OpenDocument Spreadsheet (*.ods)|*.ods|All files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save list as ODS"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOds, ext: saveFileDialogOds.DefaultExt))
		{
			return;
		}
		// Create the ODS file
		using FileStream fs = new(path: saveFileDialogOds.FileName, mode: FileMode.Create);
		using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
		// 1. mimetype (must be first and uncompressed)
		ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
		using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
		{
			writer.Write(value: "application/vnd.oasis.opendocument.spreadsheet");
		}
		// 2. META-INF/manifest.xml
		ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.spreadsheet\"/>");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
			writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
			writer.WriteLine(value: "</manifest:manifest>");
		}
		// 3. content.xml
		ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
		using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
		{
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\">");
			writer.WriteLine(value: "  <office:body>");
			writer.WriteLine(value: "    <office:spreadsheet>");
			writer.WriteLine(value: "      <table:table table:name=\"Planetoids\">");
			writer.WriteLine(value: "        <table:table-column table:number-columns-repeated=\"2\"/>");
			// Header Row
			writer.WriteLine(value: "        <table:table-row>");
			writer.WriteLine(value: "          <table:table-cell office:value-type=\"string\"><text:p>Index</text:p></table:table-cell>");
			writer.WriteLine(value: "          <table:table-cell office:value-type=\"string\"><text:p>Designation</text:p></table:table-cell>");
			writer.WriteLine(value: "        </table:table-row>");
			// Data Rows
			foreach ((string index, string name) in GetExportData())
			{
				string safeIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
				string safeName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;

				writer.WriteLine(value: "        <table:table-row>");
				writer.WriteLine(value: $"          <table:table-cell office:value-type=\"string\"><text:p>{safeIndex}</text:p></table:table-cell>");
				writer.WriteLine(value: $"          <table:table-cell office:value-type=\"string\"><text:p>{safeName}</text:p></table:table-cell>");
				writer.WriteLine(value: "        </table:table-row>");
			}
			writer.WriteLine(value: "      </table:table>");
			writer.WriteLine(value: "    </office:spreadsheet>");
			writer.WriteLine(value: "  </office:body>");
			writer.WriteLine(value: "</office:document-content>");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a simplified MOBI file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As MOBI" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogMobi = new()
		{
			Filter = "Mobi files (*.mobi)|*.mobi|All files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save list as MOBI"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMobi, ext: saveFileDialogMobi.DefaultExt))
		{
			return;
		}
		// 1. Generate Content (HTML)
		StringBuilder html = new();
		html.Append(value: "<html><head><meta charset=\"UTF-8\"><title>Planetoid List</title></head><body>");
		html.Append(value: "<h1>List of Readable Designations</h1>");
		// Mobi does not support all HTML tags, but basic tables often work in newer readers or are flattened.
		html.Append(value: "<table>");
		foreach ((string index, string name) in GetExportData())
		{
			string encodedIndex = System.Net.WebUtility.HtmlEncode(value: index) ?? string.Empty;
			string encodedName = System.Net.WebUtility.HtmlEncode(value: name) ?? string.Empty;
			html.Append(value: $"<tr><td>{encodedIndex}</td><td>{encodedName}</td></tr>");
		}
		html.Append(value: "</table></body></html>");
		byte[] bodyData = Encoding.UTF8.GetBytes(s: html.ToString());
		// 2. Chunk data (4096 bytes max per record is standard for PalmDoc)
		List<byte[]> textRecords = [];
		for (int i = 0; i < bodyData.Length; i += 4096)
		{
			int len = Math.Min(4096, bodyData.Length - i);
			byte[] chunk = new byte[len];
			Array.Copy(sourceArray: bodyData, sourceIndex: i, destinationArray: chunk, destinationIndex: 0, length: len);
			textRecords.Add(item: chunk);
		}
		// 3. Construct Headers
		// PDB Header: 78 bytes
		// Record List: 8 * NumRecords + 2 padding
		// Record 0: Header Record (PalmDOC + Mobi Header)
		// Records 1..N: Text
		// Record N+1: EOF (Optional/Standard)
		// Define a minimal Header Record (Record 0)
		// PalmDOC Header (16 bytes) + Mobi Header (min 232 bytes)
		// Using array for simplicity in binary writing
		byte[] headerRecord = new byte[256];
		// We will write into this buffer using a BinaryWriter on MemoryStream
		using (MemoryStream ms = new(buffer: headerRecord))
		using (BinaryWriter hw = new(output: ms))
		{
			// -- PalmDOC Header --
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)1)); // Compression: 1 = No Compression
			hw.Write(value: (short)0); // Unused
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: bodyData.Length)); // Text Length
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)textRecords.Count)); // Record Count
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)4096)); // Record Size
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)0)); // Encryption Type
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)0)); // Unknown
																					  // -- Mobi Header --
																					  // Identifier "MOBI"
			hw.Write(buffer: Encoding.ASCII.GetBytes(s: "MOBI"));
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 232)); // Header Length
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 2)); // Mobi Type: 2 = Book
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 65001)); // Text Encoding: 65001 = UTF-8
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 0x12345678)); // UniqueID ID
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 6)); // File Version
																			   // ... other fields are zeroed by default new byte[] ...;
																			   // First Non-Book Index (offset 80 in Mobi Header -> 16+80 = 96)
																			   // Points to EOF record usually or FLIS
			ms.Seek(offset: 96, loc: SeekOrigin.Begin);
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: textRecords.Count + 1)); // We have Header(0) + Text(1..N). So next is N+1.
																								   // Full Name Offset (offset 84 in Mobi Header -> 100)
			ms.Seek(offset: 100, loc: SeekOrigin.Begin);
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 0)); // No Full Name in this minimal version
																			   // Min Version (offset 104 -> 120)
			ms.Seek(offset: 120, loc: SeekOrigin.Begin);
			hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 6));
			// ...
		}
		// EOF Record (minimal content)
		byte[] eofRecord = [0xe9, 0x8e, 0x0d, 0x0a];
		// Total Records: Header + TextRecords + EOF
		int totalRecords = 1 + textRecords.Count + 1;
		using FileStream fs = new(path: saveFileDialogMobi.FileName, mode: FileMode.Create);
		using BinaryWriter w = new(output: fs);
		// --- PDB Header (78 bytes) ---
		string dbName = "Planetoids";
		byte[] nameBytes = new byte[32];
		Encoding.ASCII.GetBytes(s: dbName).CopyTo(array: nameBytes, index: 0);
		w.Write(buffer: nameBytes);
		w.Write(value: (short)0); // Attributes
		w.Write(value: (short)0); // Version
								  // Dates (seconds since 1904-01-01)
		uint secondsSince1904 = (uint)(DateTime.UtcNow - new DateTime(year: 1904, month: 1, day: 1)).TotalSeconds;
		w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (int)secondsSince1904)); // Creation
		w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (int)secondsSince1904)); // Modification
		w.Write(value: 0); // Backup
		w.Write(value: 0); // ModNum
		w.Write(value: 0); // AppInfoId
		w.Write(value: 0); // SortInfoId
		w.Write(buffer: Encoding.ASCII.GetBytes(s: "BOOK")); // Type
		w.Write(buffer: Encoding.ASCII.GetBytes(s: "MOBI")); // Creator
		w.Write(value: 0); // UniqueIDSeed
		w.Write(value: 0); // NextRecordListID
		w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)totalRecords)); // NumRecords
																							// --- Record List (8 bytes per record) ---
																							// Start of data is: 78 + (totalRecords * 8) + 2 (padding)
		int currentOffset = 78 + (totalRecords * 8) + 2;
		// 1. Header Record Info
		w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
		w.Write(value: 0); // Attributes/ID
		currentOffset += headerRecord.Length;
		// 2. Text Records Info
		foreach (byte[] rec in textRecords)
		{
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
			w.Write(value: 0);
			currentOffset += rec.Length;
		}
		// 3. EOF Record Info
		w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
		w.Write(value: 0);
		currentOffset += eofRecord.Length;
		// Padding (2 bytes)
		w.Write(value: (short)0);
		// --- Record Data ---
		// 1. Header
		w.Write(buffer: headerRecord);
		// 2. Text
		foreach (byte[] rec in textRecords)
		{
			w.Write(buffer: rec); // 4096 chunks
		}
		// 3. EOF
		w.Write(buffer: eofRecord);
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as an RTF file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As RTF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogRtf = new()
		{
			Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save list as RTF"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogRtf, ext: saveFileDialogRtf.DefaultExt))
		{
			return;
		}
		// Write the data to the RTF file using ASCII encoding
		using StreamWriter writer = new(path: saveFileDialogRtf.FileName, append: false, encoding: Encoding.ASCII);
		// Write RTF header
		writer.WriteLine(value: "{\\rtf1\\ansi\\deff0");
		writer.WriteLine(value: "{\\fonttbl{\\f0 Arial;}}");
		writer.WriteLine(value: "\\f0\\fs20"); // Font Arial, Size 10pt
											   // Title
		writer.WriteLine(value: "{\\pard\\b\\fs24 List of Readable Designations\\par\\par}");
		// Iterate data
		foreach ((string index, string name) in GetExportData())
		{
			// Start row definition
			writer.Write(value: "\\trowd\\trgaph108\\trleft-108");
			writer.Write(value: "\\cellx1440"); // Cell 1 width (approx 1 inch)
			writer.Write(value: "\\cellx5760"); // Cell 2 width (approx 3 inches more -> 4 inches total)
												// Cell 1 content
			writer.Write(value: "\\pard\\intbl ");
			writer.Write(value: EscapeRtf(input: index));
			writer.Write(value: "\\cell");
			// Cell 2 content
			writer.Write(value: "\\pard\\intbl ");
			writer.Write(value: EscapeRtf(input: name));
			writer.Write(value: "\\cell");
			// End row
			writer.WriteLine(value: "\\row");
		}
		// Close RTF
		writer.WriteLine(value: "}");
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Saves the current list as a text file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Text" menu item.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogText = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save list as Text"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogText, ext: saveFileDialogText.DefaultExt))
		{
			return;
		}
		// Write the data to the text file
		using StreamWriter writer = new(path: saveFileDialogText.FileName, append: false, encoding: Encoding.UTF8);
		// Iterate data
		foreach ((string index, string name) in GetExportData())
		{
			writer.WriteLine(value: $"{index}: {name}");
		}
		_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the 'Save As AsciiDoc' menu item and initiates saving the ListView results in AsciiDoc
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as an AsciiDoc-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e) => SaveListViewResultsAsAsciiDoc();

	/// <summary>Handles the click event for the 'Save As reStructuredText' menu item and initiates saving the current ListView
	/// results in reStructuredText format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as a reStructuredText-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e) => SaveListViewResultsAsReStructuredText();

	/// <summary>Handles the click event of the 'Save As Textile' menu item and initiates saving the ListView results in Textile
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables
	/// users to export the current ListView results as a Textile-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e) => SaveListViewResultsAsTextile();

	/// <summary>Handles the click event for the 'Save As Abiword' menu item and initiates saving the current list view results in
	/// Abiword format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Abiword" menu item, this event handler is invoked. It calls the SaveListViewResultsAsAbiword method, which generates an AWML (AbiWord XML) file with a .abw extension that can be opened in Abiword. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e) => SaveListViewResultsAsAbiword();

	/// <summary>Handles the Click event of the Save As WPS menu item and initiates saving the current ListView results in WPS
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As WPS" menu item, this event handler is invoked. It calls the SaveListViewResultsAsWps method, which generates an HTML file with a .wps extension that can be opened in WPS Writer. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e) => SaveListViewResultsAsWps();

	/// <summary>Handles the Click event of the 'Save As Et' menu item and initiates saving the current ListView results in the Et
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Et" menu item, this event handler is invoked. It calls the SaveListViewResultsAsEt method, which exports the data in a format compatible with WPS Spreadsheets (using CSV internally). If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e) => SaveListViewResultsAsEt();

	/// <summary>Handles the click event for the 'Save As DocBook' menu item, initiating the process to save the current list view
	/// results in DocBook format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As DocBook" menu item, this event handler is invoked. It calls the SaveListViewResultsAsDocBook method, which generates an XML document conforming to the DocBook schema, containing the list of readable designations. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e) => SaveListViewResultsAsDocBook();

	/// <summary>Handles the click event for the 'Save As TOML' menu item and initiates saving the current results in TOML format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As TOML" menu item, this event handler is invoked. It calls the SaveListViewResultsAsToml method, which generates the necessary TOML structure for the current results and saves it as a .toml file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e) => SaveListViewResultsAsToml();

	/// <summary>Handles the Click event of the Save As XPS menu item and initiates saving the current ListView results as an XPS
	/// document.</summary>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As XPS" menu item, this event handler is invoked. It calls the SaveListViewResultsAsXps method, which generates the necessary XML structure for an XPS document and saves it as a .xps file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e) => SaveListViewResultsAsXps();

	/// <summary>Handles the Click event of the Save As FictionBook2 menu item and initiates saving the current results in
	/// FictionBook2 format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As FictionBook2" menu item, this event handler is invoked. It calls the SaveListViewResultsAsFictionBook2 method, which generates an XML document conforming to the FictionBook2 schema, containing the list of readable designations. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e) => SaveListViewResultsAsFictionBook2();

	/// <summary>Handles the Click event of the Save As CHM menu item and initiates saving the current ListView results as a CHM
	/// file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As CHM" menu item, this event handler is invoked. It calls the SaveListViewResultsAsChm method, which generates the necessary HTML and project files, then uses Microsoft HTML Help Workshop to compile them into a CHM file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e) => SaveListViewResultsAsChm();


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