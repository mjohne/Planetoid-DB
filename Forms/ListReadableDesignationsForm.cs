using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Collections;
using System.Diagnostics;
using System.IO;
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
	private const int IndexLength = 7;

	/// <summary>
	/// Length of the name field in the planetoid record.
	/// </summary>
	/// <remarks>
	/// This constant defines the starting index of the name field in the planetoid record.
	/// </remarks>
	private const int NameStartIndex = 166;

	/// <summary>
	/// Length of the name field in the planetoid record.
	/// </summary>
	/// <remarks>
	/// This constant defines the length of the name field in the planetoid record.
	/// </remarks>
	private const int NameLength = 28;

	#endregion

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
	/// Cancellation token source for cancelling operations.
	/// </summary>
	/// <remarks>
	/// This token can be used to cancel ongoing operations.
	/// </remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>
	/// Stopwatch for measuring elapsed time.
	/// </summary>
	/// <remarks>
	/// This stopwatch is used to measure the elapsed time for various operations.
	/// </remarks>
	private readonly Stopwatch stopwatch = new();

	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the <see cref="ListReadableDesignationsForm"/> class.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to store the currently selected control for clipboard operations.
	/// </remarks>
	private Control? currentControl;

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
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to set the status bar text and enable the information label when text is provided.
	/// </remarks>
	private void SetStatusBar(string text, string additionalInfo = "")
	{
		// Check if the text is not null or whitespace
		if (string.IsNullOrWhiteSpace(value: text))
		{
			return;
		}
		// Set the status bar text and enable it
		labelInformation.Enabled = true;
		labelInformation.Text = string.IsNullOrWhiteSpace(value: additionalInfo) ? text : $"{text} - {additionalInfo}";
	}

	/// <summary>
	/// Clears the status bar text and disables the information label.
	/// </summary>
	/// <remarks>
	/// Resets the UI state of the status area so that no message is shown.
	/// Use when there is no status to display or when leaving a control.
	/// </remarks>
	private void ClearStatusBar()
	{
		// Clear the status bar text and disable it
		labelInformation.Enabled = false;
		labelInformation.Text = string.Empty;
	}

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
		if (currentData.Length < NameStartIndex + NameLength)
		{
			// Log a warning and return null
			logger.Warn(message: $"The record at index {index} is too short.");
			return null;
		}
		// Extract the index and designation name
		string strIndex = currentData[..IndexLength].Trim();
		string strDesignationName = currentData.Substring(startIndex: NameStartIndex, length: NameLength).Trim();
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
	/// Fills the planetoids database with the provided array list.
	/// </summary>
	/// <param name="arrTemp">The array list to fill the database with.</param>
	/// <remarks>
	/// This method is used to fill the planetoids database with the provided array list.
	/// </remarks>
	public void FillArray(ArrayList arrTemp)
	{
		planetoidsDatabase = [.. arrTemp.OfType<string>()];
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
	/// Gets the export data from the list view.
	/// </summary>
	/// <returns>An enumerable collection of tuples containing the index and name.</returns>
	/// <remarks>
	/// This method is used to retrieve the export data from the list view.
	/// </remarks>
	private IEnumerable<(string Index, string Name)> GetExportData()
	{
		// Iterate through each item in the list view and yield the index and name
		foreach (ListViewItem item in listView.Items)
		{
			// Yield the index and name as a tuple
			yield return (Index: item.SubItems[index: 0].Text, Name: item.SubItems[index: 1].Text);
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
		SetStatusBar(text: $"{I10nStrings.Index}: {item.Text} - {item.SubItems[index: 1].Text}");
		// Enable the load button
		buttonLoad.Enabled = true;
		selectedIndex = index;
	}

	/// <summary>
	/// Escapes LaTeX special characters in a string.
	/// </summary>
	/// <param name="input">The input string.</param>
	/// <returns>The escaped string.</returns>
	/// <remarks>
	/// This method is used to escape LaTeX special characters in the input string.
	/// </remarks>
	private static string EscapeLatex(string input)
	{
		if (input == null)
		{
			return string.Empty;
		}
		StringBuilder builder = new(capacity: input.Length);
		foreach (char ch in input)
		{
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
					builder.Append(value: ch);
					break;
			}
		}
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
		if (value is null)
		{
			return string.Empty;
		}
		// Escape the pipe character, which is used as a column separator in Markdown tables.
		return value.Replace(oldValue: "|", newValue: "\\|");
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
		ClearStatusBar();
		// Disable controls until data is available
		labelInformation.Enabled = listView.Visible = buttonCancel.Enabled = buttonLoad.Enabled = dropButtonSaveList.Enabled = false;
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

	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the MouseDown event for controls.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	#endregion

	#region click event handlers

	/// <summary>
	/// Handles the click event for the List button.
	/// </summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the click event for the List button.
	/// </remarks>
	private async void ButtonList_Click(object? sender, EventArgs? e)
	{
		// Local column headers for the list view
		ColumnHeader columnHeaderIndex = new()
		{
			Text = I10nStrings.Index,
			TextAlign = HorizontalAlignment.Right,
			Width = 100
		};
		// Readable designation column
		ColumnHeader columnHeaderReadableDesignation = new()
		{
			Text = "Readable Designation",
			TextAlign = HorizontalAlignment.Left,
			Width = 300
		};
		// Start measuring time
		stopwatch.Restart();
		// Reset UI state
		listView.Visible = false;
		listView.Items.Clear();
		listView.Columns.Clear();
		listView.Columns.AddRange(values: [columnHeaderIndex, columnHeaderReadableDesignation]);
		// Disable controls during processing
		numericUpDownMinimum.Enabled = false;
		numericUpDownMaximum.Enabled = false;
		buttonList.Enabled = false;
		dropButtonSaveList.Enabled = false;
		buttonLoad.Enabled = false;
		// Enable cancel button and progress bar
		buttonCancel.Enabled = true;
		progressBar.Enabled = true;
		// Set up progress bar
		int min = (int)numericUpDownMinimum.Value - 1;
		int max = (int)numericUpDownMaximum.Value;
		int total = max - min;
		progressBar.Maximum = total;
		progressBar.Value = 0;
		// Set taskbar progress state
		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = cancellationTokenSource.Token;
		// Set taskbar progress to normal state
		try
		{
			/*
			// Start loading items asynchronously
			token.ThrowIfCancellationRequested();
			TaskbarProgress.SetState(windowHandle: Handle, taskbarState: TaskbarProgress.TaskbarStates.Normal);
			*/
			// Load items in a background task
			List<ListViewItem> itemsToAdd = await Task.Run(function: () =>
			{
				// Prepare a list to hold the results
				List<ListViewItem> results = [];
				for (int i = min; i < max; i++)
				{
					// Check for cancellation
					if (token.IsCancellationRequested)
					{
						break;
					}
					// Create the ListViewItem
					ListViewItem? item = CreateListViewItem(index: i);
					// If the item is valid, add it to the results
					if (item != null)
					{
						results.Add(item);
					}
					// Update progress every 100 items
					if (i % 100 == 0)
					{
						// Update taskbar progress
						Invoke(method: () =>
						{
							progressBar.Value = i - min;
						});
					}
				}
				return results;
			}, cancellationToken: token);
			// Check for cancellation
			listView.BeginUpdate();
			listView.Items.AddRange(items: [.. itemsToAdd]);
			listView.EndUpdate();
			// Show the list view
			listView.Visible = true;
			// Finalize progress bar
			if (token.IsCancellationRequested)
			{
				// Set taskbar progress to no progress state
				MessageBox.Show(text: $"{listView.Items.Count} objects processed (Cancelled).", caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			else
			{
				// Inform the user that processing completed successfully
				MessageBox.Show(text: $"{listView.Items.Count} objects processed.", caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
		}
		// Handle exceptions
		catch (Exception ex)
		{
			// Log the error and show an error message
			logger.Error(exception: ex, message: "Error loading list.");
			ShowErrorMessage(message: $"Error loading list: {ex.Message}");
		}
		// Finalize UI state
		finally
		{
			// Stop measuring time
			stopwatch.Stop();
			// Restore UI state after loading operation
			buttonCancel.Enabled = false;
			buttonList.Enabled = true;
			dropButtonSaveList.Enabled = true;
			numericUpDownMinimum.Enabled = true;
			numericUpDownMaximum.Enabled = true;
			progressBar.Enabled = false;
			progressBar.Value = 0;
			// Reset taskbar progress
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
			// Dispose of the cancellation token source if it was created
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}
	}

	/// <summary>
	/// Cancels the ongoing operation.
	/// </summary>
	/// <remarks>
	/// This method is invoked when the user clicks the "Cancel" button.
	/// </remarks>
	private void ButtonCancel_Click(object? sender, EventArgs? e) => cancellationTokenSource?.Cancel();

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
	}

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer enters a control or the control receives focus.
	/// </remarks>
	private void Control_Enter(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the accessible description based on the sender type
		string? description = sender switch
		{
			Control c => c.AccessibleDescription,
			ToolStripItem t => t.AccessibleDescription,
			_ => null
		};
		// If a description is available, set it in the status bar
		if (description != null)
		{
			SetStatusBar(text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// If the <paramref name="sender"/> is a <see cref="Control"/>, its <see cref="Control.Text"/> value is copied to the clipboard.
	/// If the <paramref name="sender"/> is a <see cref="ToolStripItem"/>, its <see cref="ToolStripItem.Text"/> value is copied to the clipboard.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the text to copy based on the sender type
		string? textToCopy = sender switch
		{
			Control c => c.Text,
			ToolStripItem => currentControl?.Text,
			_ => null
		};
		// Check if the text to copy is not null or empty
		if (!string.IsNullOrEmpty(value: textToCopy))
		{
			// Assuming CopyToClipboard is a helper method in BaseKryptonForm or similar
			// If not, use Clipboard.SetText(textToCopy);
			try
			{
				CopyToClipboard(text: textToCopy);
			}
			// Log any exception that occurs during the clipboard operation
			catch (Exception ex)
			{
				logger.Error(exception: ex, message: "Failed to copy text to the clipboard.");
				throw new InvalidOperationException(message: "Failed to copy text to the clipboard.", innerException: ex);
			}
		}
	}

	#endregion
}