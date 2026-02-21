// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Planetoid_DB;

/// <summary>
/// Form to display database information.
/// </summary>
/// <remarks>
/// This form provides a user interface for displaying information about the database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseInformationForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the database information form.
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
	/// Initializes a new instance of the <see cref="DatabaseInformationForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public DatabaseInformationForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Prepares the save dialog for exporting data.
	/// </summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>
	/// This method is used to prepare the save dialog for exporting data.
	/// </remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Database-Information.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>
	/// Escapes a string for use in a CSV file.
	/// </summary>
	/// <param name="text">The text to escape.</param>
	/// <param name="separator">The separator used in the CSV file.</param>
	/// <returns>The escaped text.</returns>
	/// <remarks>
	/// This method escapes special characters in a string for use in a CSV file.
	/// </remarks>
	private static string EscapeCsv(string text, string separator)
	{
		return text.Contains(value: separator) || text.Contains(value: '"') || text.Contains(value: '\n') || text.Contains(value: '\r')
			? $"\"{text.Replace(oldValue: "\"", newValue: "\"\"")}\""
			: text;
	}

	/// <summary>
	/// Saves the content of the table layout panel to a CSV file.
	/// </summary>
	/// <param name="path">The path to the file to save to.</param>
	/// <param name="separator">The separator to use in the CSV file (default is semicolon).</param>
	/// <remarks>
	/// This method iterates through the table layout panel controls and writes the content to a CSV file.
	/// </remarks>
	private void SaveTableToCsv(string path, string separator = ";")
	{
		// Create a string builder to build the CSV content
		StringBuilder csvContent = new();
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the CSV content
				_ = csvContent.AppendLine(value: $"{EscapeCsv(text: label, separator: separator)}{separator}{EscapeCsv(text: value, separator: separator)}");
			}
		}
		// Write the content to the file
		File.WriteAllText(path: path, contents: csvContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the database information form loads.
	/// Populates UI labels with file information for the configured MPCORB.DAT file and displays detected file attributes.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the database information form loads.
	/// </remarks>
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

	/// <summary>
	/// Handles the click event of the copy to clipboard button.
	/// </summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard button is clicked.
	/// </remarks>
	private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for copying to the clipboard
			contextMenuFullCopyToClipboard.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database name.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database name is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNameValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database path.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database path is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardPath_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDirectoryValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database size.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database size is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardSize_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSizeValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database creation date.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database creation date is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardCreationDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateCreatedValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database last access date.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database last access date is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardLastAccessDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateAccessedValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database last write date.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database last write date is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardLastWriteDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateWritedValue.Text);

	/// <summary>
	/// Handles the click event of the copy to clipboard menu item for the database attributes.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the copy to clipboard menu item for the database attributes is clicked.
	/// </remarks>
	private void MenuitemCopyToClipboardAttributes_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAttributesValue.Text);

	/// <summary>
	/// Handles the click event of the save to file button.
	/// </summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the save to file button is clicked.
	/// </remarks>
	private void ToolStripButtonSaveToFile_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for saving to a file
			contextMenuSaveToFile.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}

	/// <summary>
	/// Handles the click event for the 'Save As Text' menu item, allowing users to export database information to a text
	/// file.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a text file in a tab-separated format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogText = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save database information as text"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogText, ext: saveFileDialogText.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the tab-separated content
		StringBuilder textContent = new();
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the content with a space separator
				_ = textContent.AppendLine(value: $"{label} {value}");
			}
		}
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogText.FileName, contents: textContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the 'Save As LaTeX' menu item, allowing users to export database information to a LaTeX
	/// file.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a LaTeX file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogLatex = new()
		{
			Filter = "LaTeX files (*.tex)|*.tex|All files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save database information as LaTeX"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogLatex, ext: saveFileDialogLatex.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the LaTeX content
		StringBuilder latexContent = new();
		latexContent.AppendLine(value: @"\documentclass{article}");
		latexContent.AppendLine(value: @"\usepackage[utf8]{inputenc}");
		latexContent.AppendLine(value: @"\begin{document}");
		latexContent.AppendLine(value: @"\begin{tabular}{|l|l|}");
		latexContent.AppendLine(value: @"\hline");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the LaTeX content
				latexContent.AppendLine(value: $"{label} & {value} \\\\ \\hline");
			}
		}
		latexContent.AppendLine(value: @"\end{tabular}");
		latexContent.AppendLine(value: @"\end{document}");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogLatex.FileName, contents: latexContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the 'Save As Markdown' menu item, allowing users to export database information to a Markdown
	/// file.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a Markdown file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogMarkdown = new()
		{
			Filter = "Markdown files (*.md)|*.md|All files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save database information as Markdown"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMarkdown, ext: saveFileDialogMarkdown.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the Markdown content
		StringBuilder markdownContent = new();
		markdownContent.AppendLine(value: "| Label | Value |");
		markdownContent.AppendLine(value: "|-------|-------|");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the Markdown content
				markdownContent.AppendLine(value: $"| {label} | {value} |");
			}
		}
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogMarkdown.FileName, contents: markdownContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the 'Save As Word' menu item, allowing users to export database information to a Word
	/// file.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a Word file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogWord = new()
		{
			Filter = "Word files (*.doc)|*.doc|All files (*.*)|*.*",
			DefaultExt = "doc",
			Title = "Save database information as Word"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogWord, ext: saveFileDialogWord.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the Word content
		StringBuilder wordContent = new();
		wordContent.AppendLine(value: @"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'>");
		wordContent.AppendLine(value: @"<head><meta charset='utf-8'><title>Database Information</title></head>");
		wordContent.AppendLine(value: @"<body>");
		wordContent.AppendLine(value: @"<table border=""1"" style=""border-collapse: collapse; width: 100%;"">");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the Word content
				wordContent.AppendLine(value: $"  <tr><td>{label}</td><td>{value}</td></tr>");
			}
		}
		wordContent.AppendLine(value: @"</table>");
		wordContent.AppendLine(value: @"</body>");
		wordContent.AppendLine(value: @"</html>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogWord.FileName, contents: wordContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as ODT" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an ODT file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogOdt = new()
		{
			Filter = "OpenDocument Text files (*.odt)|*.odt|All files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save database information as ODT"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOdt, ext: saveFileDialogOdt.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the ODT content
		StringBuilder odtContent = new();
		odtContent.AppendLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		odtContent.AppendLine(value: "<office:document xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\" office:mimetype=\"application/vnd.oasis.opendocument.text\">");
		odtContent.AppendLine(value: "<office:body>");
		odtContent.AppendLine(value: "<office:text>");
		odtContent.AppendLine(value: "<table:table>");
		odtContent.AppendLine(value: "<table:table-column table:number-columns-repeated=\"2\"/>");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the ODT content
				odtContent.AppendLine(value: "<table:table-row>");
				odtContent.AppendLine(value: $"<table:table-cell><text:p>{label}</text:p></table:table-cell>");
				odtContent.AppendLine(value: $"<table:table-cell><text:p>{value}</text:p></table:table-cell>");
				odtContent.AppendLine(value: "</table:table-row>");
			}
		}
		odtContent.AppendLine(value: "</table:table>");
		odtContent.AppendLine(value: "</office:text>");
		odtContent.AppendLine(value: "</office:body>");
		odtContent.AppendLine(value: "</office:document>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogOdt.FileName, contents: odtContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as Excel" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an Excel file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogExcel = new()
		{
			Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*",
			DefaultExt = "xls",
			Title = "Save database information as Excel"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogExcel, ext: saveFileDialogExcel.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the Excel content
		StringBuilder excelContent = new();
		excelContent.AppendLine(value: @"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
		excelContent.AppendLine(value: @"<head><meta charset='utf-8'><title>Database Information</title>");
		excelContent.AppendLine(value: @"<!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Database Information</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->");
		excelContent.AppendLine(value: @"</head><body>");
		excelContent.AppendLine(value: @"<table border=""1"">");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the Excel content
				excelContent.AppendLine(value: $"  <tr><td>{label}</td><td>{value}</td></tr>");
			}
		}
		excelContent.AppendLine(value: @"</table>");
		excelContent.AppendLine(value: @"</body>");
		excelContent.AppendLine(value: @"</html>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogExcel.FileName, contents: excelContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as ODS" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an ODS file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogOds = new()
		{
			Filter = "OpenDocument Spreadsheet files (*.ods)|*.ods|All files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save database information as ODS"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogOds, ext: saveFileDialogOds.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the ODS content
		StringBuilder odsContent = new();
		odsContent.AppendLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		odsContent.AppendLine(value: "<office:document xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\" office:mimetype=\"application/vnd.oasis.opendocument.spreadsheet\">");
		odsContent.AppendLine(value: "<office:body>");
		odsContent.AppendLine(value: "<office:spreadsheet>");
		odsContent.AppendLine(value: "<table:table table:name=\"DatabaseInformation\">");
		odsContent.AppendLine(value: "<table:table-column table:number-columns-repeated=\"2\"/>");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the ODS content
				odsContent.AppendLine(value: "<table:table-row>");
				odsContent.AppendLine(value: $"<table:table-cell office:value-type=\"string\"><text:p>{label}</text:p></table:table-cell>");
				odsContent.AppendLine(value: $"<table:table-cell office:value-type=\"string\"><text:p>{value}</text:p></table:table-cell>");
				odsContent.AppendLine(value: "</table:table-row>");
			}
		}
		odsContent.AppendLine(value: "</table:table>");
		odsContent.AppendLine(value: "</office:spreadsheet>");
		odsContent.AppendLine(value: "</office:body>");
		odsContent.AppendLine(value: "</office:document>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogOds.FileName, contents: odsContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as RTF" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an RTF file in a tabular format. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogRtf = new()
		{
			Filter = "Rich Text Format files (*.rtf)|*.rtf|All files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save database information as RTF"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogRtf, ext: saveFileDialogRtf.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the RTF content
		StringBuilder rtfContent = new();
		rtfContent.AppendLine(value: @"{\rtf1\ansi\deff0");
		rtfContent.AppendLine(value: @"{\fonttbl{\f0\fnil\fcharset0 Calibri;}}");
		rtfContent.AppendLine(value: @"\viewkind4\uc1\pard\fs20");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the RTF content
				rtfContent.AppendLine(value: $@"\b {label}:\b0 {value}\par");
			}
		}
		rtfContent.AppendLine(value: "}");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogRtf.FileName, contents: rtfContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as CSV" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a CSV file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogCsv = new()
		{
			Filter = "Comma-separated values files (*.csv)|*.csv|All files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save database information as CSV"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogCsv, ext: saveFileDialogCsv.DefaultExt))
		{
			return;
		}
		SaveTableToCsv(path: saveFileDialogCsv.FileName, separator: ";");
	}

	/// <summary>
	/// Handles the click event for the "Save as TSV" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a TSV file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogTsv = new()
		{
			Filter = "Tab-separated values files (*.tsv)|*.tsv|All files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save database information as TSV"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogTsv, ext: saveFileDialogTsv.DefaultExt))
		{
			return;
		}
		SaveTableToCsv(path: saveFileDialogTsv.FileName, separator: "\t");
	}

	/// <summary>
	/// Handles the click event for the "Save as PSV" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a PSV file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogPsv = new()
		{
			Filter = "Pipe-separated values files (*.psv)|*.psv|All files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save database information as PSV"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPsv, ext: saveFileDialogPsv.DefaultExt))
		{
			return;
		}
		SaveTableToCsv(path: saveFileDialogPsv.FileName, separator: "|");
	}

	/// <summary>
	/// Handles the click event for the "Save as HTML" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an HTML file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogHtml = new()
		{
			Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save database information as HTML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogHtml, ext: saveFileDialogHtml.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the HTML content
		StringBuilder htmlContent = new();
		htmlContent.AppendLine(value: "<html>");
		htmlContent.AppendLine(value: "<head><title>Database Information</title></head>");
		htmlContent.AppendLine(value: "<body>");
		htmlContent.AppendLine(value: "<table border=\"1\">");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the HTML content
				htmlContent.AppendLine(value: $"  <tr><td>{label}</td><td>{value}</td></tr>");
			}
		}
		htmlContent.AppendLine(value: "</table>");
		htmlContent.AppendLine(value: "</body>");
		htmlContent.AppendLine(value: "</html>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogHtml.FileName, contents: htmlContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as XML" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an XML file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogXml = new()
		{
			Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save database information as XML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogXml, ext: saveFileDialogXml.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the XML content
		StringBuilder xmlContent = new();
		xmlContent.AppendLine(value: "<DatabaseInformation>");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the XML content
				xmlContent.AppendLine(value: $"  <{label}>{value}</{label}>");
			}
		}
		xmlContent.AppendLine(value: "</DatabaseInformation>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogXml.FileName, contents: xmlContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as JSON" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a JSON file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
	{
		using SaveFileDialog saveFileDialogJson = new()
		{
			Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save database information as JSON"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogJson, ext: saveFileDialogJson.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the JSON content
		StringBuilder jsonContent = new();
		jsonContent.AppendLine(value: "{");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the JSON content
				jsonContent.AppendLine(value: $"  \"{label}\": \"{value}\",");
			}
		}
		jsonContent.AppendLine(value: "}");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogJson.FileName, contents: jsonContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as YAML" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a YAML file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
	{
		using SaveFileDialog saveFileDialogYaml = new()
		{
			Filter = "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save database information as YAML"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogYaml, ext: saveFileDialogYaml.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the YAML content
		StringBuilder yamlContent = new();
		yamlContent.AppendLine(value: "DatabaseInformation:");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the YAML content
				yamlContent.AppendLine(value: $"  {label}: {value}");
			}
		}
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogYaml.FileName, contents: yamlContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as SQL" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an SQL file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogSql = new()
		{
			Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save database information as SQL"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogSql, ext: saveFileDialogSql.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the SQL content
		StringBuilder sqlContent = new();
		sqlContent.AppendLine(value: "CREATE TABLE DatabaseInformation (");
		sqlContent.AppendLine(value: "	Label NVARCHAR(255),");
		sqlContent.AppendLine(value: "	Value NVARCHAR(MAX)");
		sqlContent.AppendLine(value: ");");
		sqlContent.AppendLine();
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Escape single quotes for SQL
				label = label.Replace(oldValue: "'", newValue: "''");
				value = value.Replace(oldValue: "'", newValue: "''");
				// Append the line to the SQL content
				sqlContent.AppendLine(value: $"INSERT INTO DatabaseInformation (Label, Value) VALUES ('{label}', '{value}');");
			}
		}
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogSql.FileName, contents: sqlContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as PDF" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a PDF file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogPdf = new()
		{
			Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save database information as PDF"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPdf, ext: saveFileDialogPdf.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the PDF content
		StringBuilder pdfContent = new();
		List<long> offsets = [];
		// Helper function to add objects and track offsets
		void AddObject(int id, string content)
		{
			offsets.Add(item: pdfContent.Length);
			pdfContent.AppendLine(value: $"{id} 0 obj");
			pdfContent.AppendLine(value: content);
			pdfContent.AppendLine(value: "endobj");
		}
		// Header
		pdfContent.AppendLine(value: "%PDF-1.4");
		// 1: Catalog
		AddObject(id: 1, content: "<< /Type /Catalog /Pages 2 0 R >>");
		// 2: Pages
		AddObject(id: 2, content: "<< /Type /Pages /Kids [3 0 R] /Count 1 >>");
		// 3: Page
		AddObject(id: 3, content: "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents 5 0 R /Resources << /Font << /F1 4 0 R >> >> >>");
		// 4: Font
		AddObject(id: 4, content: "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
		// Build Content Stream
		StringBuilder streamContent = new();
		streamContent.AppendLine(value: "BT");
		streamContent.AppendLine(value: "/F1 12 Tf");
		streamContent.AppendLine(value: "50 750 Td");
		streamContent.AppendLine(value: "15 TL");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Escape special characters for PDF text strings
				string line = $"{label}: {value}".Replace(oldValue: "\\", newValue: "\\\\").Replace(oldValue: "(", newValue: "\\(").Replace(oldValue: ")", newValue: "\\)");
				streamContent.AppendLine(value: $"({line}) Tj");
				streamContent.AppendLine(value: "T*");
			}
		}
		streamContent.AppendLine(value: "ET");
		// 5: Content Object
		AddObject(id: 5, content: $"<< /Length {streamContent.Length} >>\nstream\n{streamContent}endstream");
		// XRef Table
		long xrefOffset = pdfContent.Length;
		pdfContent.AppendLine(value: "xref");
		pdfContent.AppendLine(value: $"0 {offsets.Count + 1}");
		pdfContent.AppendLine(value: "0000000000 65535 f ");
		foreach (long offset in offsets)
		{
			pdfContent.AppendLine(value: $"{offset:D10} 00000 n ");
		}
		// Trailer
		pdfContent.AppendLine(value: "trailer");
		pdfContent.AppendLine(value: $"<< /Size {offsets.Count + 1} /Root 1 0 R >>");
		pdfContent.AppendLine(value: "startxref");
		pdfContent.AppendLine(value: $"{xrefOffset}");
		pdfContent.AppendLine(value: "%%EOF");
		// Write the content to the file using Latin1 encoding to preserve byte offsets
		File.WriteAllText(path: saveFileDialogPdf.FileName, contents: pdfContent.ToString(), encoding: Encoding.Latin1);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as PostScript" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a PostScript file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
	{
		using SaveFileDialog saveFileDialogPostScript = new()
		{
			Filter = "PostScript files (*.ps)|*.ps|All files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save database information as PostScript"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogPostScript, ext: saveFileDialogPostScript.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the PostScript content
		StringBuilder psContent = new();
		psContent.AppendLine(value: "%!PS-Adobe-3.0");
		psContent.AppendLine(value: "%%Title: Database Information");
		psContent.AppendLine(value: "%%Pages: 1");
		psContent.AppendLine(value: "%%EndComments");
		psContent.AppendLine(value: "/Times-Roman findfont 12 scalefont setfont");
		psContent.AppendLine(value: "50 750 moveto");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the PostScript content
				psContent.AppendLine(value: $"({label}: {value}) show");
				psContent.AppendLine(value: "0 -20 rmoveto");
			}
		}
		psContent.AppendLine(value: "showpage");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogPostScript.FileName, contents: psContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as EPUB" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as an EPUB file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogEpub = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save database information as EPUB"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogEpub, ext: saveFileDialogEpub.DefaultExt))
		{
			return;
		}
		// Create the EPUB file as a ZipArchive
		using (FileStream zipToOpen = new(path: saveFileDialogEpub.FileName, mode: FileMode.Create))
		{
			using System.IO.Compression.ZipArchive archive = new(stream: zipToOpen, mode: System.IO.Compression.ZipArchiveMode.Create);
			// 1. mimetype (must be first, uncompressed)
			System.IO.Compression.ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: System.IO.Compression.CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open()))
			{
				writer.Write(value: "application/epub+zip");
			}
			// 2. META-INF/container.xml
			System.IO.Compression.ZipArchiveEntry containerEntry = archive.CreateEntry(entryName: "META-INF/container.xml");
			using (StreamWriter writer = new(stream: containerEntry.Open()))
			{
				writer.Write(value: "<?xml version=\"1.0\"?><container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\"><rootfiles><rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/></rootfiles></container>");
			}
			// 3. OEBPS/content.opf
			System.IO.Compression.ZipArchiveEntry opfEntry = archive.CreateEntry(entryName: "OEBPS/content.opf");
			using (StreamWriter writer = new(stream: opfEntry.Open()))
			{
				writer.Write(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?><package xmlns=\"http://www.idpf.org/2007/opf\" unique-identifier=\"uuid_id\" version=\"2.0\"><metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\"><dc:title>Database Information</dc:title><dc:language>en</dc:language><dc:identifier id=\"uuid_id\" opf:scheme=\"uuid\">urn:uuid:00000000-0000-0000-0000-000000000000</dc:identifier></metadata><manifest><item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/><item id=\"content\" href=\"content.xhtml\" media-type=\"application/xhtml+xml\"/></manifest><spine toc=\"ncx\"><itemref idref=\"content\"/></spine></package>");
			}
			// 4. OEBPS/toc.ncx
			System.IO.Compression.ZipArchiveEntry ncxEntry = archive.CreateEntry(entryName: "OEBPS/toc.ncx");
			using (StreamWriter writer = new(stream: ncxEntry.Open()))
			{
				writer.Write(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ncx xmlns=\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\"><head><meta name=\"dtb:uid\" content=\"urn:uuid:00000000-0000-0000-0000-000000000000\"/><meta name=\"dtb:depth\" content=\"1\"/><meta name=\"dtb:totalPageCount\" content=\"0\"/><meta name=\"dtb:maxPageNumber\" content=\"0\"/></head><docTitle><text>Database Information</text></docTitle><navMap><navPoint id=\"navPoint-1\" playOrder=\"1\"><navLabel><text>Database Information</text></navLabel><content src=\"content.xhtml\"/></navPoint></navMap></ncx>");
			}
			// 5. OEBPS/content.xhtml
			System.IO.Compression.ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "OEBPS/content.xhtml");
			using (StreamWriter writer = new(stream: contentEntry.Open()))
			{
				StringBuilder xhtmlContent = new();
				xhtmlContent.AppendLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				xhtmlContent.AppendLine(value: "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
				xhtmlContent.AppendLine(value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">");
				xhtmlContent.AppendLine(value: "<head><title>Database Information</title></head>");
				xhtmlContent.AppendLine(value: "<body>");
				xhtmlContent.AppendLine(value: "<h1>Database Information</h1>");
				xhtmlContent.AppendLine(value: "<table border=\"1\" style=\"width: 100%;\">");
				// Iterate through the rows of the table layout panel
				for (int row = 0; row < tableLayoutPanel.RowCount; row++)
				{
					// Get the control in the first column (label)
					Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
					// Get the control in the second column (value)
					Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
					// Check if both controls are not null
					if (controlLabel is not null && controlValue is not null)
					{
						// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
						string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
						string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
						// Append the line to the XHTML content
						xhtmlContent.AppendLine(value: $"<tr><td>{label}</td><td>{value}</td></tr>");
					}
				}
				xhtmlContent.AppendLine(value: "</table>");
				xhtmlContent.AppendLine(value: "</body>");
				xhtmlContent.AppendLine(value: "</html>");
				writer.Write(value: xhtmlContent.ToString());
			}
		}
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Save as Mobi" menu item.
	/// </summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method displays a Save File dialog to prompt the user for a file location and name. It
	/// collects the displayed database information from the form and saves it as a Mobi file. A
	/// confirmation message is shown upon successful completion.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog manually
		using SaveFileDialog saveFileDialogMobi = new()
		{
			Filter = "Mobi files (*.mobi)|*.mobi|All files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save database information as Mobi"
		};
		// Prepare the save dialog
		if (!PrepareSaveDialog(dialog: saveFileDialogMobi, ext: saveFileDialogMobi.DefaultExt))
		{
			return;
		}
		// Create a string builder to build the Mobi content (HTML based)
		StringBuilder mobiContent = new();
		mobiContent.AppendLine(value: "<html>");
		mobiContent.AppendLine(value: "<head><title>Database Information</title></head>");
		mobiContent.AppendLine(value: "<body>");
		mobiContent.AppendLine(value: "<h1>Database Information</h1>");
		// Iterate through the rows of the table layout panel
		for (int row = 0; row < tableLayoutPanel.RowCount; row++)
		{
			// Get the control in the first column (label)
			Control? controlLabel = tableLayoutPanel.GetControlFromPosition(column: 0, row: row);
			// Get the control in the second column (value)
			Control? controlValue = tableLayoutPanel.GetControlFromPosition(column: 1, row: row);
			// Check if both controls are not null
			if (controlLabel is not null && controlValue is not null)
			{
				// Get the text from the controls, preferring KryptonLabel.Values.Text if available, otherwise Control.Text
				string label = controlLabel is Krypton.Toolkit.KryptonLabel kLabel ? kLabel.Values.Text : controlLabel.Text;
				string value = controlValue is Krypton.Toolkit.KryptonLabel kValue ? kValue.Values.Text : controlValue.Text;
				// Append the line to the Mobi content
				mobiContent.AppendLine(value: $"<p><strong>{label}</strong>: {value}</p>");
			}
		}
		mobiContent.AppendLine(value: "</body>");
		mobiContent.AppendLine(value: "</html>");
		// Write the content to the file
		File.WriteAllText(path: saveFileDialogMobi.FileName, contents: mobiContent.ToString(), encoding: Encoding.UTF8);
		MessageBox.Show(text: I18nStrings.FileSavedSuccessfully, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	#endregion
}