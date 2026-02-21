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

/// <summary>
/// Form to list readable designations from the planetoids database.
/// </summary>
/// <remarks>
/// This form is used to display a list of all readable designations from the planetoids database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ListReadableDesignationsForm : BaseKryptonForm
{
	#region Constants

	/// <summary>
	/// Length of the index field in the planetoid record.
	/// </summary>
	/// <remarks>
	/// This constant defines the length of the index field in the planetoid record.
	/// </remarks>
	private const int indexLength = 7;

	/// <summary>
	/// Length of the name field in the planetoid record.
	/// </summary>
	/// <remarks>
	/// This constant defines the starting index of the name field in the planetoid record.
	/// </remarks>
	private const int nameStartIndex = 166;

	/// <summary>
	/// Length of the name field in the planetoid record.
	/// </summary>
	/// <remarks>
	/// This constant defines the length of the name field in the planetoid record.
	/// </remarks>
	private const int nameLength = 28;

	#endregion

	/// <summary>
	/// Offset for virtual mode to calculate the starting index in the database
	/// </summary>
	/// <remarks>
	/// This field is used to calculate the starting index in the database for virtual mode.
	/// </remarks>
	private int virtualListOffset = 0;

	/// <summary>
	/// List of planetoid records from the database
	/// </summary>
	/// <remarks>
	/// This list contains all the planetoid records retrieved from the database.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>
	/// Number of planetoids in the database.
	/// </summary>
	/// <remarks>
	/// This field keeps track of the total number of planetoids in the database.
	/// </remarks>
	private int numberPlanetoids;

	/// <summary>
	/// Index of the currently selected planetoid.
	/// </summary>
	/// <remarks>
	/// This index is used to keep track of the currently selected planetoid in the list.
	/// </remarks>
	private int selectedIndex;

	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the <see cref="ListReadableDesignationsForm"/> class.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="ListReadableDesignationsForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form and its components.
	/// </remarks>
	public ListReadableDesignationsForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a short string representation of the current instance for debugging purposes.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Creates a ListViewItem for the specified index.
	/// </summary>
	/// <param name="index">The index of the planetoid.</param>
	/// <returns>A ListViewItem representing the planetoid, or null if the index is invalid.</returns>
	/// <remarks>
	/// This method is used to create a ListViewItem for the specified index.
	/// </remarks>
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

	/// <summary>
	/// Fills the planetoids database with the provided list.
	/// </summary>
	/// <param name="arrTemp">The list to fill the database with.</param>
	/// <remarks>
	/// This method is used to fill the planetoids database with the provided list.
	/// </remarks>
	public void FillArray(List<string> arrTemp)
	{
		planetoidsDatabase = [.. arrTemp];
		numberPlanetoids = planetoidsDatabase.Count;
	}

	/// <summary>
	/// Sets the maximum index for the planetoids database.
	/// </summary>
	/// <param name="maxIndex">The maximum index.</param>
	/// <remarks>
	/// This method is used to set the maximum index for the planetoids database.
	/// </remarks>
	public void SetMaxIndex(int maxIndex) => numberPlanetoids = maxIndex;

	/// <summary>
	/// Gets the selected index in the list view.
	/// </summary>
	/// <returns>The selected index.</returns>
	/// <remarks>
	/// This method is used to get the selected index in the list view.
	/// </remarks>
	public int GetSelectedIndex() => selectedIndex;

	/// <summary>
	/// Gets the export data from the virtual list.
	/// </summary>
	/// <returns>An enumerable collection of tuples containing the index and name.</returns>
	/// <remarks>
	/// It iterates over the indices and creates the data on-the-fly, instead of accessing listView.Items.
	/// </remarks>
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
			int dbIndex = virtualListOffset + i;
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

	/// <summary>
	/// Prepares the save dialog for exporting data.
	/// </summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>
	/// This method is used to prepare the save dialog for exporting data.
	/// </remarks>
	private bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Readable-Designation-List_{numericUpDownMinimum.Value}-{numericUpDownMaximum.Value}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>
	/// Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and readable designation,
	/// enables the load button if necessary and stores the currently selected index.
	/// </summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the SelectedIndexChanged event of the ListView.
	/// </remarks>
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
		buttonLoad.Enabled = true;
		selectedIndex = index;
	}

	/// <summary>
	/// Escapes LaTeX special characters in a string.
	/// </summary>
	/// <param name="input">The input string, which may be <c>null</c>.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>
	/// This method is used to escape LaTeX special characters in the input string.
	/// </remarks>
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

	/// <summary>
	/// Escapes Markdown-table-specific characters in a cell value.
	/// </summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The cell value escaped for use in a Markdown table.</returns>
	/// <remarks>
	/// This method is used to escape Markdown-table-specific characters in the cell value.
	/// </remarks>
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

	/// <summary>
	/// Escapes PostScript special characters in a string.
	/// </summary>
	/// <param name="input">The input string.</param>
	/// <returns>The escaped string suitable for PostScript output.</returns>
	/// <remarks>
	/// This method is used to escape PostScript special characters in the input string.
	/// </remarks>
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

	/// <summary>
	/// Escapes characters for PDF string literals.
	/// </summary>
	/// <param name="text">The input text to escape.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>
	/// This method is used to escape special characters in the input text for PDF output.
	/// </remarks>
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

	/// <summary>
	/// Escapes special characters for RTF output.
	/// </summary>
	/// <param name="input">The input string.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>
	/// This method is used to escape special characters in the input string for RTF output.
	/// </remarks>
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

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the ListReadableDesignationsForm loads.
	/// Initializes UI state: clears the status area, disables controls until data is available,
	/// and sets numeric up/down ranges based on the loaded planetoids database.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form's UI components and state.
	/// </remarks>
	private void ListReadableDesignationsForm_Load(object? sender, EventArgs? e)
	{
		// Clear the status bar on load
		ClearStatusBar(label: labelInformation);
		// Disable controls until data is available
		labelInformation.Enabled = listView.Visible = buttonLoad.Enabled = dropButtonSaveList.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Set numeric up/down ranges based on the planetoids database
		numericUpDownMinimum.Minimum = 1;
		numericUpDownMaximum.Minimum = 1;
		numericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		numericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		numericUpDownMinimum.Value = 1;
		numericUpDownMaximum.Value = planetoidsDatabase.Count;
	}

	/// <summary>
	/// Handles the form Closed event.
	/// Cleans up resources and cancels any ongoing operations.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form is closed.
	/// </remarks>
	private void ListReadableDesignationsForm_FormClosed(object sender, FormClosedEventArgs e) =>
		// Clearing the token if the window is closed during work
		listView.Dispose();

	#endregion

	#region ListView event handlers

	/// <summary>
	/// Handles the retrieval of virtual items for the ListView.
	/// Dynamically creates ListViewItems when they are needed for display.
	/// </summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to retrieve virtual items for the ListView.
	/// </remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Calculating the true index in the database based on the offset
		int realIndex = virtualListOffset + e.ItemIndex;
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

	/// <summary>
	/// Handles the click event for the List button.
	/// </summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the click event for the List button.
	/// </remarks>
	private void ButtonList_Click(object? sender, EventArgs? e)
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
			int min = (int)numericUpDownMinimum.Value - 1;
			int max = (int)numericUpDownMaximum.Value;
			int count = max - min;
			if (count <= 0)
			{
				listView.Visible = true;
				listView.EndUpdate();
				return;
			}
			// Virtual Mode configure
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
			dropButtonSaveList.Enabled = true;
		}
	}

	/// <summary>
	/// Saves the current list as a CSV file.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As CSV" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogCsv, ext: "csv"))
		{
			return;
		}
		// Write the data to the CSV file
		using StreamWriter streamWriter = new(path: saveFileDialogCsv.FileName, append: false, encoding: Encoding.UTF8);
		foreach ((string? index, string? name) in GetExportData())
		{
			streamWriter.WriteLine(value: $"{index}; {name}");
		}
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an HTML file.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As HTML" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogHtml, ext: "html"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an XML file.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As XML" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogXml, ext: "xml"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a JSON file.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As JSON" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogJson, ext: "json"))
		{
			return;
		}
		// Prepare the export data
		var exportList = GetExportData().Select(selector: static x => new { x.Index, Designation = x.Name });
		// Serialize to JSON and write to file
		string jsonString = JsonSerializer.Serialize(value: exportList, options: new() { WriteIndented = true });
		File.WriteAllText(path: saveFileDialogJson.FileName, contents: jsonString);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a SQL script.
	/// Exports the list as a series of SQL INSERT statements.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As SQL" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogSql, ext: "sql"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the list as a Markdown table.
	/// Ideal for documentation, GitHub Readmes, or Wikis.
	/// </summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As Markdown" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMarkdown, ext: "md"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the list in YAML format.
	/// A human-readable data serialization standard.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As YAML" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogYaml, ext: "yaml"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the list as a TSV (Tab-Separated Values) file.
	/// Ideal for spreadsheet applications.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As TSV" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogTsv, ext: "tsv"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the list as a PSV (Pipe-Separated Values) file.
	/// Ideal for spreadsheet applications.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As PSV" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPsv, ext: "psv"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the list as a LaTeX document.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As LaTeX" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
	{
		if (!PrepareSaveDialog(dialog: saveFileDialogLatex, ext: "tex"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a PostScript (.ps) file.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As PostScript" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPostScript, ext: "ps"))
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an uncompressed PDF file.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As PDF" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
	{
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPdf, ext: "pdf"))
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
		w.WriteLine(value: $"/Count {pageObjIds.Count}");
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
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an EPUB file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As EPUB" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogEpub = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save list as EPUB"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogEpub, ext: "epub"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a Word document.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As Word" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogWord = new()
		{
			Filter = "Word documents (*.docx)|*.docx|All files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save list as Word"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogWord, ext: "docx"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an Excel file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As Excel" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogExcel = new()
		{
			Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save list as Excel"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogExcel, ext: "xlsx"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an ODT file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As ODT" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogOdt = new()
		{
			Filter = "OpenDocument Text (*.odt)|*.odt|All files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save list as ODT"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOdt, ext: "odt"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an ODS file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As ODS" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogOds = new()
		{
			Filter = "OpenDocument Spreadsheet (*.ods)|*.ods|All files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save list as ODS"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOds, ext: "ods"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a simplified MOBI file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As MOBI" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogMobi = new()
		{
			Filter = "Mobi files (*.mobi)|*.mobi|All files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save list as MOBI"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMobi, ext: "mobi"))
		{
			return;
		}

		// 1. Generate Content (HTML)
		StringBuilder html = new();
		html.Append(value: "<html><head><title>Planetoid List</title></head><body>");
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
																			   // ... other fields are zeroed by default new byte[] ...

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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as an RTF file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As RTF" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogRtf = new()
		{
			Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save list as RTF"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogRtf, ext: "rtf"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Saves the current list as a text file.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>
	/// This method is invoked when the user selects the "Save As Text" menu item.
	/// </remarks>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
	{
		// Create a SaveFileDialog manually since it's not in the designer
		using SaveFileDialog saveFileDialogText = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save list as Text"
		};

		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogText, ext: "txt"))
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

		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	#endregion
}