// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace Planetoid_DB;

/// <summary>Form to display database information.</summary>
/// <remarks>This form provides a user interface for displaying information about the database.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseInformationForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DatabaseInformationForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public DatabaseInformationForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Database-Information.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	private static void SaveAsMdb()
	{
		using SaveFileDialog saveFileDialogMdb = new()
		{
			Filter = "Microsoft Access files (*.mdb)|*.mdb|All files (*.*)|*.*",
			DefaultExt = "mdb",
			Title = "Save database information as Microsoft Access"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMdb, ext: saveFileDialogMdb.DefaultExt))
		{
			return;
		}
		string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={saveFileDialogMdb.FileName};";
		using OleDbConnection connection = new(connectionString);
		connection.Open();
		string createTableQuery = @"CREATE TABLE DatabaseInformation (
  Name TEXT,
  Directory TEXT,
  Size TEXT,
  DateCreated TEXT,
  DateAccessed TEXT,
  DateModified TEXT,
  Attributes TEXT
);";
		try
		{
			using OleDbCommand command = new(cmdText: createTableQuery, connection: connection);
			_ = command.ExecuteNonQuery();

			// Insert a row with the current database file information.
			FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameMpcorb);
			const string insertQuery = @"
INSERT INTO DatabaseInformation (Name, Directory, Size, DateCreated, DateAccessed, DateModified, Attributes)
VALUES (@Name, @Directory, @Size, @DateCreated, @DateAccessed, @DateModified, @Attributes);";
			using OleDbCommand insertCommand = new(cmdText: insertQuery, connection: connection);
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@Name", value: fileInfo.Name);
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@Directory", value: fileInfo.DirectoryName ?? string.Empty);
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@Size", value: fileInfo.Length.ToString(provider: CultureInfo.InvariantCulture));
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@DateCreated", value: fileInfo.CreationTime.ToString(format: "G", provider: CultureInfo.CurrentCulture));
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@DateAccessed", value: fileInfo.LastAccessTime.ToString(format: "G", provider: CultureInfo.CurrentCulture));
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@DateModified", value: fileInfo.LastWriteTime.ToString(format: "G", provider: CultureInfo.CurrentCulture));
			_ = insertCommand.Parameters.AddWithValue(parameterName: "@Attributes", value: fileInfo.Attributes.ToString());
			_ = insertCommand.ExecuteNonQuery();

			_ = MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
		}
		catch (OleDbException ex)
		{
			logger.Error(exception: ex, message: "Failed to save database information to MDB file. The OLE DB provider may be unavailable.");
			_ = MessageBox.Show(
				text: "Saving the database information failed. The required OLE DB provider may not be installed on this system.",
				caption: I18nStrings.ErrorCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
		}
		catch (InvalidOperationException ex)
		{
			logger.Error(exception: ex, message: "Failed to save database information to MDB file due to an invalid operation.");
			_ = MessageBox.Show(
				text: "Saving the database information failed due to an unexpected error while accessing the database file.",
				caption: I18nStrings.ErrorCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the database information form loads.
	/// Populates UI labels with file information for the configured MPCORB.DAT file and displays detected file attributes.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the database information form loads.</remarks>
	private void DatabaseInformationForm_Load(object sender, EventArgs e)
	{
		// Path to the database file
		FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameMpcorb);
		// Check if the file exists
		if (!File.Exists(path: fileInfo.FullName))
		{
			// Log an error and show a message if the file does not exist
			logger.Error(message: $"Database file not found: {fileInfo.FullName}");
			ShowErrorMessage(message: $"Database file not found: {fileInfo.FullName}");
			return;
		}
		// Set the file information in the labels
		labelNameValue.Text = fileInfo.Name;
		// Set the file name in the label
		labelDirectoryValue.Text = fileInfo.DirectoryName;
		// Set the file size in the label
		labelSizeValue.Text = $"{fileInfo.Length:N0} {I18nStrings.BytesText}";
		// Set the file type in the label
		labelDateCreatedValue.Text = fileInfo.CreationTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file creation time in the label
		labelDateAccessedValue.Text = fileInfo.LastAccessTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file last access time in the label
		labelDateWritedValue.Text = fileInfo.LastWriteTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file attributes in the label
		labelAttributesValue.Text = $"{fileInfo.Attributes}";
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event of the copy to clipboard button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard button is clicked.</remarks>
	private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for copying to the clipboard
			contextMenuFullCopyToClipboard.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}

	/// <summary>Handles the click event of the copy to clipboard menu item for the database name.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database name is clicked.</remarks>
	private void MenuitemCopyToClipboardName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNameValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database path.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database path is clicked.</remarks>
	private void MenuitemCopyToClipboardPath_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDirectoryValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database size.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database size is clicked.</remarks>
	private void MenuitemCopyToClipboardSize_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSizeValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database creation date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database creation date is clicked.</remarks>
	private void MenuitemCopyToClipboardCreationDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateCreatedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database last access date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database last access date is clicked.</remarks>
	private void MenuitemCopyToClipboardLastAccessDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateAccessedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database last write date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database last write date is clicked.</remarks>
	private void MenuitemCopyToClipboardLastWriteDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateWritedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database attributes.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database attributes is clicked.</remarks>
	private void MenuitemCopyToClipboardAttributes_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAttributesValue.Text);

	/// <summary>Handles the click event of the save to file button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the save to file button is clicked.</remarks>
	private void ToolStripButtonSaveToFile_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for saving to a file
			contextMenuSaveToFile.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}


	#endregion

	/// <summary>Handles the Click event of the Save As Text menu item, allowing the user to export the contents of the table layout
	/// panel to a text file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the method exports the current list view results to the specified text file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As Text menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the text file to save the list view results; if the user confirms the save operation, call the SaveAsText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save as Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsText(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as LaTeX' menu item, allowing the user to export the contents of the table
	/// layout panel to a LaTeX file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the current table layout panel data to a LaTeX file at the specified location.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the LaTeX file to save the list view results; if the user confirms the save operation, call the SaveAsLatex method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save as LaTeX"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsLatex(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as Markdown' menu item, allowing the user to export the current table layout
	/// panel content to a Markdown file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the current table layout panel data is exported as a Markdown file. The export operation will not proceed
	/// if the dialog is canceled or invalid input is provided.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Markdown file to save the list view results; if the user confirms the save operation, call the SaveAsMarkdown method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Markdown Files (*.md)|*.md|All Files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save as Markdown"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsMarkdown(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as AsciiDoc' menu item, allowing the user to export the current list view
	/// results to an AsciiDoc file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout is exported as an AsciiDoc document. The export operation is only performed if the dialog is
	/// successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the AsciiDoc file to save the list view results; if the user confirms the save operation, call the SaveAsAsciiDoc method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*",
			DefaultExt = "adoc",
			Title = "Save as AsciiDoc"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsAsciiDoc(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as ReStructuredText' menu item, allowing the user to export the list view
	/// results to a ReStructuredText (.rst) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the current table layout is exported as a ReStructuredText file. The export includes the list of readable
	/// designations.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the ReStructuredText file to save the list view results; if the user confirms the save operation, call the SaveAsReStructuredText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*",
			DefaultExt = "rst",
			Title = "Save as ReStructuredText"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsReStructuredText(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save As Textile' menu item to export the contents of the table layout panel to a
	/// Textile file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the file name and location for the Textile
	/// export. If the user confirms the operation, the contents of the table layout panel are saved in Textile
	/// format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the textile file to save the list view results; if the user confirms the save operation, call the SaveAsTextile method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*",
			DefaultExt = "textile",
			Title = "Save as Textile"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsTextile(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as Word' menu item to export the contents of the table layout panel to a Word
	/// document.</summary>
	/// <remarks>This method displays a Save File dialog to allow the user to specify the destination for the
	/// exported Word document. If the user confirms the operation, the contents of the table layout panel are saved to the
	/// specified file in .docx format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Word file to save the list view results; if the user confirms the save operation, call the SaveAsWord method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save as Word"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsWord(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as ODT' menu item to export the contents of the table layout panel to an
	/// OpenDocument Text (.odt) file.</summary>
	/// <remarks>Displays a Save File dialog allowing the user to specify the destination file. If the user
	/// confirms, the method exports the current list view results to the specified ODT file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Text file to save the list view results; if the user confirms the save operation, call the SaveAsOdt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save as OpenDocument Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsOdt(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As RTF menu item, allowing the user to export the contents of the table layout
	/// panel to a Rich Text Format (RTF) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the method exports the current list view results to an RTF file. The exported file includes the readable
	/// designations displayed in the table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the Save As RTF menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the RTF file to save the list view results; if the user confirms the save operation, call the SaveAsRtf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save as RTF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsRtf(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save As Abiword' menu item, allowing the user to export the list view results to an
	/// Abiword (.abw) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the Abiword file location and name. If the user
	/// confirms, the current table layout is exported to the specified file in Abiword format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Abiword file to save the list view results; if the user confirms the save operation, call the SaveAsAbiword method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*",
			DefaultExt = "abw",
			Title = "Save as Abiword"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsAbiword(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As WPS menu item, allowing the user to export the current list view results to
	/// a WPS Writer file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the selected WPS file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Writer file to save the list view results; if the user confirms the save operation, call the SaveAsWps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*",
			DefaultExt = "wps",
			Title = "Save as WPS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsWps(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event of the 'Save as Excel' menu item to export the contents of the table layout panel to an
	/// Excel file.</summary>
	/// <remarks>This method displays a Save File dialog to the user for specifying the Excel file location and
	/// name. If the user confirms the operation, the contents of the table layout panel are exported to the specified
	/// Excel file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Excel file to save the list view results; if the user confirms the save operation, call the SaveAsExcel method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save as Excel"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsExcel(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as ODS' menu item, allowing the user to export the contents of the table
	/// layout panel to an OpenDocument Spreadsheet (ODS) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination ODS file. If the user confirms,
	/// the method exports the current table layout panel data to the selected file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Spreadsheet file to save the list view results; if the user confirms the save operation, call the SaveAsOds method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save as ODS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsOds(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as CSV' menu item, allowing the user to export the contents of the table
	/// layout panel to a CSV file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the contents of the table layout panel are exported to the selected CSV file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
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
		TableLayoutPanelExporter.SaveAsCsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As TSV menu item, allowing the user to export the contents of the table layout
	/// panel to a tab-separated values (TSV) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms the operation, the method exports the current data to a TSV file. The exported file can be used for data
	/// exchange or further processing in spreadsheet applications.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
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
		TableLayoutPanelExporter.SaveAsTsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as PSV' menu item to export the contents of the table layout panel to a
	/// pipe-separated values (PSV) file.</summary>
	/// <remarks>This method displays a Save File dialog allowing the user to specify the destination file for the
	/// PSV export. If the user confirms the operation, the current data is saved in PSV format. The export includes the
	/// list of readable designations from the table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
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
		TableLayoutPanelExporter.SaveAsPsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event for the 'Save As ET' menu item, allowing the user to export the current list view results
	/// to an ET file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the current table layout is exported to the specified ET file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the ET file to save the list view results; if the user confirms the save operation, call the SaveAsEt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "ET Files (*.et)|*.et|All Files (*.*)|*.*",
			DefaultExt = "et",
			Title = "Save as ET"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsEt(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as HTML' menu item to export the contents of the table layout panel to an HTML
	/// file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the destination file. If the user
	/// confirms, the method exports the current table layout panel data to the selected HTML file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the HTML file to save the list view results; if the user confirms the save operation, call the SaveAsHtml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save as HTML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsHtml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as XML' menu item to export the current list view results to an XML file.</summary>
	/// <remarks>This method displays a Save File dialog to the user for specifying the XML file location and name.
	/// If the user confirms the operation, the current table layout panel data is exported as an XML file. The export
	/// includes the list of readable designations.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XML file to save the list view results; if the user confirms the save operation, call the SaveAsXml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as XML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsXml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as DocBook' menu item, allowing the user to export the list view results to a
	/// DocBook XML file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout is exported as a DocBook XML file. The export operation may overwrite an existing file if the
	/// user selects one.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
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
		TableLayoutPanelExporter.SaveAsDocBook(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as JSON' menu item, allowing the user to export the current list view results
	/// to a JSON file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current data is exported to the selected JSON file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the JSON file to save the list view results; if the user confirms the save operation, call the SaveAsJson method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save as JSON"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsJson(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);

	}

	/// <summary>Handles the click event for the 'Save as YAML' menu item, allowing the user to export the current table layout
	/// panel data to a YAML file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms the operation, the current data from the table layout panel is exported to the selected YAML
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the YAML file to save the list view results; if the user confirms the save operation, call the SaveAsYaml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save as YAML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsYaml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as TOML' menu item, allowing the user to export the current list view results
	/// to a TOML file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout panel data is exported to the specified TOML file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
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
		TableLayoutPanelExporter.SaveAsToml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As SQL menu item to export the current list view results to a SQL file.</summary>
	/// <remarks>This method displays a Save File dialog to allow the user to specify the destination for the SQL
	/// export. If the user confirms the operation, the current contents of the table layout panel are exported as a SQL
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As SQL menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQL file to save the list view results; if the user confirms the save operation, call the SaveAsSql method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save as SQL"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsSql(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as SQLite' menu item, allowing the user to export the current list view
	/// results to a SQLite database file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the selected SQLite file. The export includes the current contents of the associated
	/// table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQLite file to save the list view results; if the user confirms the save operation, call the SaveAsSqlite method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*",
			DefaultExt = "sqlite",
			Title = "Save as SQLite"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsSqlite(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event of the 'Save as PDF' menu item to export the contents of the table layout panel to a PDF
	/// file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the destination for the PDF file. If the
	/// user confirms the operation, the contents of the table layout panel are exported to the specified PDF
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PDF file to save the list view results; if the user confirms the save operation, call the SaveAsPdf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save as PDF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsPdf(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As PostScript menu item to allow the user to export the list view results as a
	/// PostScript file.</summary>
	/// <remarks>This handler displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the current table layout is exported to a PostScript file. The export operation is only performed if the
	/// dialog is successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As PostScript menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PostScript file to save the list view results; if the user confirms the save operation, call the SaveAsPostScript method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save as PostScript"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsPostScript(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as EPUB' menu item, allowing the user to export the current list view results
	/// to an EPUB file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the EPUB file location and name. If the user
	/// confirms the operation, the current table layout is exported as an EPUB file. The export is only performed if the
	/// dialog is successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the EPUB file to save the list view results; if the user confirms the save operation, call the SaveAsEpub method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save as EPUB"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsEpub(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As MOBI menu item, allowing the user to export the current list view results to
	/// a MOBI file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the specified MOBI file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the MOBI file to save the list view results; if the user confirms the save operation, call the SaveAsMobi method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save as MOBI"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsMobi(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As XPS menu item, allowing the user to export the current list view results to
	/// an XPS file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination XPS file. If the user confirms,
	/// the current table layout panel content is exported to the specified XPS file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
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
		TableLayoutPanelExporter.SaveAsXps(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as FictionBook2' menu item, allowing the user to export the current list view
	/// results to a FictionBook2 (.fb2) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms the
	/// operation, the method exports the data to the specified FictionBook2 file format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
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
		TableLayoutPanelExporter.SaveAsFictionBook2(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as CHM' menu item, allowing the user to export the current list view results
	/// to a Compiled HTML Help (CHM) file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination and filename for
	/// the CHM export. If the user confirms the operation, the current table layout is exported to a CHM file at the
	/// specified location.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CHM file to save the list view results; if the user confirms the save operation, call the SaveAsChm method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*",
			DefaultExt = "chm",
			Title = "Save as CHM"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsChm(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}
}
