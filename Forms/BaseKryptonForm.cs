// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB.Forms;

/// <summary>Base form providing common behaviours for application forms. Currently: enables <c>KeyPreview</c> and closes the form when the Escape key is pressed.</summary>
/// <remarks>This class serves as a base form for the application, providing common functionality and behaviors that can be shared across derived forms.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class BaseKryptonForm : KryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Stores the currently selected control for clipboard operations.</summary>
	/// <remarks>This control is used for clipboard operations such as copy and paste.</remarks>
	protected Control? currentControl;

	/// <summary>Initializes a new instance of the <see cref="BaseKryptonForm"/> class.</summary>
	/// <remarks>This constructor sets up the form to receive key events and handle them appropriately.</remarks>
	protected BaseKryptonForm()
	{
		// Ensure the form receives key events before child controls
		KeyPreview = true;
		KeyDown += BaseKryptonForm_KeyDown;
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation for the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Displays an error message.</summary>
	/// <param name="message">The error message.</param>
	/// <remarks>This method is used to display an error message to the user.</remarks>
	protected static void ShowErrorMessage(string message) =>
		// Show an error message box with the specified message
		_ = KryptonMessageBox.Show(text: message, caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);

	/// <summary>Copies the specified text to the clipboard and shows a confirmation dialog.</summary>
	/// <param name="text">The text to copy to the clipboard.</param>
	/// <remarks>On success an informational dialog is shown. On failure the exception is logged and an error dialog is displayed. This method is protected so derived forms can use it directly.</remarks>
	protected static void CopyToClipboard(string text)
	{
		// Try to copy the text to the clipboard
		try
		{
			// Copy the text to the clipboard
			Clipboard.SetText(text: text);
			_ = KryptonMessageBox.Show(text: I18nStrings.CopiedToClipboard, caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
		// Handle any exceptions that occur during the clipboard operation
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: ex.Message);
			// Show an error message
			ShowErrorMessage(message: $"Error copying to clipboard: {ex.Message}");
		}
	}

	/// <summary>Sets the status bar text and enables the information label when text is provided.</summary>
	/// <param name="label">The status label to update.</param>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>This method is used to set the status bar text and enable the information label.</remarks>
	protected static void SetStatusBar(ToolStripStatusLabel label, string text, string additionalInfo = "")
	{
		// Check if the label is null or text is null or whitespace
		if (label is null || string.IsNullOrWhiteSpace(value: text))
		{
			return;
		}
		// Set the status bar text and enable it
		label.Enabled = true;
		label.Text = string.IsNullOrWhiteSpace(value: additionalInfo) ? text : $"{text} - {additionalInfo}";
	}

	/// <summary>Clears the status bar text and disables the information label.</summary>
	/// <param name="label">The status label to clear.</param>
	/// <remarks>Resets the UI state of the status area so that no message is shown. Use when there is no status to display or when leaving a control.</remarks>
	protected static void ClearStatusBar(ToolStripStatusLabel label)
	{
		// Check if the label is null
		if (label is null)
		{
			return;
		}
		// Clear the status bar text and disable it
		label.Enabled = false;
		label.Text = string.Empty;
	}

	/// <summary>Gets the status label to be used for displaying information.
	/// Derived classes should override this property to provide the specific label.</summary>
	/// <remarks>This property is used to access the status label for displaying information.</remarks>
	protected virtual ToolStripStatusLabel? StatusLabel => null;

	/// <summary>Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.</summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is called when the mouse pointer enters a control or the control receives focus.</remarks>
	protected void Control_Enter(object? sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Check if the status label is null
		if (StatusLabel is null)
		{
			return;
		}
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
			SetStatusBar(label: StatusLabel, text: description);
		}
	}

	/// <summary>Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.</summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is called when the mouse pointer leaves a control or the control loses focus.</remarks>
	protected void Control_Leave(object? sender, EventArgs e)
	{
		// Check if the status label is not null
		if (StatusLabel != null)
		{
			// Clear the status bar text
			ClearStatusBar(label: StatusLabel);
		}
	}

	/// <summary>Handles double-click events on controls and copies their text to the clipboard.</summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method extracts text from the sender control or uses the current control's text for ToolStripItems, then copies it to the clipboard using the <see cref="CopyToClipboard"/> method.</remarks>
	protected void CopyToClipboard_DoubleClick(object sender, EventArgs e)
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
			// Copy the text to the clipboard using the base method
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

	/// <summary>Default KeyDown handler that closes the form when Escape is pressed.</summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Key event args.</param>
	/// <remarks>This method is used to handle key down events for the form.</remarks>
	private void BaseKryptonForm_KeyDown(object? sender, KeyEventArgs e)
	{
		// Close the form when Escape is pressed
		if (e.KeyCode == Keys.Escape && !this.InvokeRequired)
		{
			Close();
		}
	}

	/// <summary>Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to store the control that triggered the event for future reference.</remarks>
	protected virtual void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	/// <summary>Opens the specified website URL in the default web browser.</summary>
	/// <param name="fileName">The URL or file name to open in the default browser. If null, empty, or whitespace, an error is logged and shown.</param>
	/// <remarks>If the operation fails or the input is invalid, an error message is displayed and the exception or error is logged. The method uses the system's default browser to open the URL.</remarks>
	protected static void OpenWebsite(string fileName)
	{
		if (string.IsNullOrWhiteSpace(value: fileName))
		{
			const string message = "The parameter 'fileName' cannot be null, empty, or consist only of white-space characters.";
			logger.Error(message);
			ShowErrorMessage(message: $"Error opening the website: {message}");
			return;
		}
		try
		{
			// Open the website in the default browser
			using Process process = new();
			process.StartInfo = new ProcessStartInfo(fileName: fileName) { UseShellExecute = true };
			process.Start();
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: $"Error opening website '{fileName}'.");
			ShowErrorMessage(message: $"Error opening the website '{fileName}'. Please see the log for more details.");
		}
	}

	#region Export virtual properties

	/// <summary>Gets the <see cref="ListView"/> control used as the export source.</summary>
	/// <remarks>Override in derived forms that export from a <see cref="ListView"/>. Returns <see langword="null"/> by default.</remarks>
	protected virtual ListView? ExportListView => null;

	/// <summary>Gets the <see cref="TableLayoutPanel"/> control used as the export source.</summary>
	/// <remarks>Override in derived forms that export from a <see cref="TableLayoutPanel"/>. Returns <see langword="null"/> by default.</remarks>
	protected virtual TableLayoutPanel? ExportTableLayoutPanel => null;

	/// <summary>Gets the <see cref="TextBox"/> control used as the export source.</summary>
	/// <remarks>Override in derived forms that export from a <see cref="TextBox"/>. Returns <see langword="null"/> by default.</remarks>
	protected virtual TextBox? ExportTextBox => null;

	/// <summary>Gets the title string passed to the exporter when saving data.</summary>
	/// <remarks>Override in derived forms to provide a form-specific export title. Returns <see cref="string.Empty"/> by default.</remarks>
	protected virtual string ExportTitle => string.Empty;

	/// <summary>Gets the file name prefix used to compose the suggested file name in the save dialog.</summary>
	/// <remarks>Override in derived forms to provide a form-specific prefix. The base <see cref="PrepareSaveDialog"/> appends a timestamp and the file extension. Returns <c>"Export"</c> by default.</remarks>
	protected virtual string ExportFilePrefix => "Export";

	/// <summary>Gets an optional virtual-row provider for <see cref="ListView"/> export operations.</summary>
	/// <remarks>Override in derived forms that use a virtual-mode <see cref="ListView"/> to supply row data on demand. Returns <see langword="null"/> by default.</remarks>
	protected virtual Func<int, ListViewItem>? ExportVirtualRowProvider => null;

	#endregion

	#region PrepareSaveDialog

	/// <summary>Configures the save dialog with a default initial directory and suggested file name, then shows it.</summary>
	/// <param name="dialog">The file dialog to configure and display.</param>
	/// <param name="ext">The file extension used for the suggested file name.</param>
	/// <returns><see langword="true"/> if the user confirmed the dialog; <see langword="false"/> if the dialog was cancelled.</returns>
	/// <remarks>The suggested file name is composed of <see cref="ExportFilePrefix"/> and the current timestamp in the format <c>yyyy-MM-dd_HH-mm-ss</c>. Override in derived forms to customise the file naming or dialog behaviour.</remarks>
	protected virtual bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set the initial directory to the user's Documents folder and suggest a default file name with a timestamp
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"{ExportFilePrefix}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		return dialog.ShowDialog(owner: this) == DialogResult.OK;
	}

	#endregion

	#region PerformSaveExport

	/// <summary>Displays a save dialog and exports data using the appropriate exporter for the configured export control.</summary>
	/// <param name="filter">The file type filter string for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title displayed in the save dialog.</param>
	/// <param name="listViewAction">The export action invoked when <see cref="ExportListView"/> is set.</param>
	/// <param name="tableLayoutPanelAction">The export action invoked when <see cref="ExportTableLayoutPanel"/> is set.</param>
	/// <param name="textBoxAction">The optional export action invoked when <see cref="ExportTextBox"/> is set.</param>
	/// <remarks>The export target is selected in priority order: <see cref="ExportListView"/>, then <see cref="ExportTableLayoutPanel"/>, then <see cref="ExportTextBox"/>. The wait cursor is shown during export. Any exception is logged and displayed as an error message.</remarks>
	protected void PerformSaveExport(
		string filter, string defaultExt, string dialogTitle,
		Action<ListView, string, string, Func<int, ListViewItem>?> listViewAction,
		Action<TableLayoutPanel, string, string> tableLayoutPanelAction,
		Action<TextBox, string, string>? textBoxAction = null)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog; if the user cancels, return without performing the export
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			// Dispatch to the appropriate exporter based on the configured export control
			if (ExportListView is not null)
			{
				listViewAction(ExportListView, ExportTitle, saveFileDialog.FileName, ExportVirtualRowProvider);
			}
			else if (ExportTableLayoutPanel is not null)
			{
				tableLayoutPanelAction(ExportTableLayoutPanel, ExportTitle, saveFileDialog.FileName);
			}
			else if (ExportTextBox is not null && textBoxAction is not null)
			{
				textBoxAction(ExportTextBox, ExportTitle, saveFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"An error occurred during export: {ex.Message}");
			ShowErrorMessage(message: $"An error has occurred during export: {ex.Message}");
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region SaveAs click event handlers

	/// <summary>Handles the Click event to export data as a plain text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with text-specific parameters.</remarks>
	protected void SaveAsText_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", listViewAction: ListViewExporter.SaveAsText, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsText, textBoxAction: TextBoxExporter.SaveAsText);

	/// <summary>Handles the Click event to export data as a LaTeX file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with LaTeX-specific parameters.</remarks>
	protected void SaveAsLatex_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", listViewAction: ListViewExporter.SaveAsLatex, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsLatex, textBoxAction: TextBoxExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export data as a Markdown file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Markdown-specific parameters.</remarks>
	protected void SaveAsMarkdown_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", listViewAction: ListViewExporter.SaveAsMarkdown, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsMarkdown, textBoxAction: TextBoxExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export data as an AsciiDoc file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with AsciiDoc-specific parameters.</remarks>
	protected void SaveAsAsciiDoc_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", listViewAction: ListViewExporter.SaveAsAsciiDoc, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsAsciiDoc, textBoxAction: TextBoxExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export data as a reStructuredText file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with reStructuredText-specific parameters.</remarks>
	protected void SaveAsReStructuredText_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", listViewAction: ListViewExporter.SaveAsReStructuredText, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsReStructuredText, textBoxAction: TextBoxExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export data as a Textile file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Textile-specific parameters.</remarks>
	protected void SaveAsTextile_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", listViewAction: ListViewExporter.SaveAsTextile, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsTextile, textBoxAction: TextBoxExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export data as a Typst file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Typst-specific parameters.</remarks>
	protected void SaveAsTypst_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Typst Files (*.typ)|*.typ|All Files (*.*)|*.*", defaultExt: "typ", dialogTitle: "Save as Typst", listViewAction: ListViewExporter.SaveAsTypst, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsTypst, textBoxAction: TextBoxExporter.SaveAsTypst);


	/// <summary>Handles the Click event to export data as a Word document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Word-specific parameters.</remarks>
	protected void SaveAsWord_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", listViewAction: ListViewExporter.SaveAsWord, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsWord, textBoxAction: TextBoxExporter.SaveAsWord);

	/// <summary>Handles the Click event to export data as an OpenDocument Text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ODT-specific parameters.</remarks>
	protected void SaveAsOdt_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", listViewAction: ListViewExporter.SaveAsOdt, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsOdt, textBoxAction: TextBoxExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export data as a Rich Text Format file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with RTF-specific parameters.</remarks>
	protected void SaveAsRtf_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", listViewAction: ListViewExporter.SaveAsRtf, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsRtf, textBoxAction: TextBoxExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export data as an Abiword file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Abiword-specific parameters.</remarks>
	protected void SaveAsAbiword_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", listViewAction: ListViewExporter.SaveAsAbiword, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsAbiword, textBoxAction: TextBoxExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export data as a WPS Writer file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with WPS-specific parameters.</remarks>
	protected void SaveAsWps_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", listViewAction: ListViewExporter.SaveAsWps, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsWps, textBoxAction: TextBoxExporter.SaveAsWps);

	/// <summary>Handles the Click event to export data as an Excel spreadsheet.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Excel-specific parameters.</remarks>
	protected void SaveAsExcel_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Spreadsheets (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", listViewAction: ListViewExporter.SaveAsExcel, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsExcel, textBoxAction: TextBoxExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export data as an OpenDocument Spreadsheet file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ODS-specific parameters.</remarks>
	protected void SaveAsOds_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as OpenDocument Spreadsheet", listViewAction: ListViewExporter.SaveAsOds, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsOds, textBoxAction: TextBoxExporter.SaveAsOds);

	/// <summary>Handles the Click event to export data as a CSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with CSV-specific parameters.</remarks>
	protected void SaveAsCsv_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", listViewAction: ListViewExporter.SaveAsCsv, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsCsv, textBoxAction: TextBoxExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export data as a TSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with TSV-specific parameters.</remarks>
	protected void SaveAsTsv_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", listViewAction: ListViewExporter.SaveAsTsv, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsTsv, textBoxAction: TextBoxExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export data as a PSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PSV-specific parameters.</remarks>
	protected void SaveAsPsv_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", listViewAction: ListViewExporter.SaveAsPsv, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsPsv, textBoxAction: TextBoxExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export data as a WPS Spreadsheets (ET) file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ET-specific parameters.</remarks>
	protected void SaveAsEt_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", listViewAction: ListViewExporter.SaveAsEt, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsEt, textBoxAction: TextBoxExporter.SaveAsEt);

	/// <summary>Handles the Click event to export data as an HTML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with HTML-specific parameters.</remarks>
	protected void SaveAsHtml_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", listViewAction: ListViewExporter.SaveAsHtml, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsHtml, textBoxAction: TextBoxExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export data as an XML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with XML-specific parameters.</remarks>
	protected void SaveAsXml_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", listViewAction: ListViewExporter.SaveAsXml, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsXml, textBoxAction: TextBoxExporter.SaveAsXml);

	/// <summary>Handles the Click event to export data as a DocBook XML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with DocBook-specific parameters.</remarks>
	protected void SaveAsDocBook_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", listViewAction: ListViewExporter.SaveAsDocBook, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsDocBook, textBoxAction: TextBoxExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export data as a JSON file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with JSON-specific parameters.</remarks>
	protected void SaveAsJson_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", listViewAction: ListViewExporter.SaveAsJson, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsJson, textBoxAction: TextBoxExporter.SaveAsJson);

	/// <summary>Handles the Click event to export data as a YAML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with YAML-specific parameters.</remarks>
	protected void SaveAsYaml_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", listViewAction: ListViewExporter.SaveAsYaml, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsYaml, textBoxAction: TextBoxExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export data as a TOML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with TOML-specific parameters.</remarks>
	protected void SaveAsToml_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", listViewAction: ListViewExporter.SaveAsToml, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsToml, textBoxAction: TextBoxExporter.SaveAsToml);

	/// <summary>Handles the Click event to export data as a SQL script file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with SQL-specific parameters.</remarks>
	protected void SaveAsSql_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", listViewAction: ListViewExporter.SaveAsSql, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsSql, textBoxAction: TextBoxExporter.SaveAsSql);

	/// <summary>Handles the Click event to export data as a SQLite database file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with SQLite-specific parameters.</remarks>
	protected void SaveAsSqlite_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Database Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", listViewAction: ListViewExporter.SaveAsSqlite, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsSqlite, textBoxAction: TextBoxExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export data as a PDF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PDF-specific parameters.</remarks>
	protected void SaveAsPdf_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", listViewAction: ListViewExporter.SaveAsPdf, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsPdf, textBoxAction: TextBoxExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export data as a PostScript file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PostScript-specific parameters.</remarks>
	protected void SaveAsPostScript_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", listViewAction: ListViewExporter.SaveAsPostScript, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsPostScript, textBoxAction: TextBoxExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export data as an EPUB file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with EPUB-specific parameters.</remarks>
	protected void SaveAsEpub_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", listViewAction: ListViewExporter.SaveAsEpub, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsEpub, textBoxAction: TextBoxExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export data as a MOBI file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with MOBI-specific parameters.</remarks>
	protected void SaveAsMobi_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", listViewAction: ListViewExporter.SaveAsMobi, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsMobi, textBoxAction: TextBoxExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export data as a FictionBook2 file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with FictionBook2-specific parameters.</remarks>
	protected void SaveAsFictionBook2_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", listViewAction: ListViewExporter.SaveAsFictionBook2, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsFictionBook2, textBoxAction: TextBoxExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export data as a Compiled HTML Help (CHM) file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with CHM-specific parameters.</remarks>
	protected void SaveAsChm_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", listViewAction: ListViewExporter.SaveAsChm, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsChm, textBoxAction: TextBoxExporter.SaveAsChm);

	/// <summary>Handles the Click event to export data as an XPS document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with XPS-specific parameters.</remarks>
	protected void SaveAsXps_Click(object? sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", listViewAction: ListViewExporter.SaveAsXps, tableLayoutPanelAction: TableLayoutPanelExporter.SaveAsXps, textBoxAction: TextBoxExporter.SaveAsXps);

	#endregion
}