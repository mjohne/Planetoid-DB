// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents a form that displays the list of MPC observatory codes and their corresponding locations.</summary>
/// <remarks>This form provides a two-column ListView with observatory codes and location names.
/// All data is built-in and does not require an internet connection.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ObservatoryCodesForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column in the ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header to toggle the sort order.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ObservatoryCodesForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public ObservatoryCodesForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Populates the ListView with the observatory code data from the embedded resource.</summary>
	/// <remarks>This method reads the <c>ObservatoryCodes</c> resource line by line, splits each entry by the pipe character, and populates the ListView with the code and location. It uses BeginUpdate/EndUpdate for performance when adding many items.</remarks>
	private void LoadObservatoryCodesList()
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// Populate the ListView
		try
		{
			// Set the cursor to wait while loading data
			Cursor.Current = Cursors.WaitCursor;
			// Use BeginUpdate/EndUpdate to improve performance when adding multiple items to the ListView
			listView.BeginUpdate();
			listView.Items.Clear();
			// Add each entry from the ObservatoryCodes resource to the ListView
			string resourceText = Properties.Resources.ObservatoryCodes ?? string.Empty;
			// Split the resource text into lines and process each line
			foreach (string resourceLine in resourceText.Split(separator: '\n'))
			{
				// Trim any trailing carriage return characters and skip empty lines
				string trimmedLine = resourceLine.TrimEnd(trimChars: ['\r']);
				// Skip empty lines
				if (string.IsNullOrEmpty(value: trimmedLine))
				{
					continue;
				}
				// Split the line into parts using the pipe character as a separator
				string[] parts = trimmedLine.Split(separator: '|');
				// If there are at least two parts (code and location), create a ListViewItem and add it to the ListView
				if (parts.Length >= 2)
				{
					ListViewItem item = new(text: parts[0]);
					item.SubItems.Add(text: parts[1]);
					listView.Items.Add(value: item);
				}
			}
			// Update status bar with count
			SetStatusBar(label: labelInformation, text: $"{listView.Items.Count} observatory codes loaded");
		}
		// Log the successful loading of observatory codes
		catch (Exception ex)
		{
			// Log the error and show a message box to the user
			logger.Error(message: $"An error occurred while loading observatory codes: {ex}");
			MessageBox.Show(text: $"An error has occurred while loading observatory codes: {ex.Message}", caption: "Load Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// Ensure that the cursor is reset and the ListView is updated even if an error occurs
		finally
		{
			// End the update of the ListView and reset the cursor
			listView.EndUpdate();
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set a default file name with timestamp to reduce accidental overwrites
		string timestamp = DateTime.Now.ToString(format: "yyyyMMdd_HHmmss");
		dialog.FileName = $"ObservatoryCodes_{timestamp}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the list view, title, file name, and an optional virtual row provider.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
	{
		// Create and configure the save file dialog
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog. If the user cancels, return without exporting.
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// Execute the export action
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(listView, "List of observatory codes", saveFileDialog.FileName, null);
		}
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and loads the observatory codes into the ListView.</remarks>
	private void ObservatoryCodesForm_Load(object sender, EventArgs e) =>
		// Clear the status bar and load the observatory codes into the ListView
		LoadObservatoryCodesList();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no items in the ListView, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			return;
		}
		// Determine the new sort order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to indicate the current sort column and order
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the ToolStripButton that displays information about observatory codes.</summary>
	/// <remarks>Displays a message box with information about observatory codes and a link to the Minor Planet Center website.</remarks>
	/// <param name="sender">The source of the event, typically the ToolStripButton that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripButtonInfoAboutObsCodes_Click(object sender, EventArgs e)
	{
		// Show a message box with information about observatory codes and a link to the Minor Planet Center website
		MessageBox.Show(text: "This application displays a list of observatory codes and their corresponding locations.\n\nYou can find more information about Observatory Codes at the Minor Planet Center website: https://minorplanetcenter.net/iau/info/ObservatoryCodes.html.", caption: "About Observatory Codes", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
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
}