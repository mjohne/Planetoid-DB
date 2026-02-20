// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;

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

	#endregion
}