// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Database Data Verification Form.</summary>
/// <remarks>This form is used to verify the integrity of database data files (e.g. ASTORB.DAT or MPCORB.DAT).</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class CheckDatabaseForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The HttpClient instance used for making HTTP requests.</summary>
	/// <remarks>This HttpClient is used to send requests and receive responses from web services.</remarks>
	private static readonly HttpClient client = new();

	/// <summary>The URL used to retrieve information about the online database file.</summary>
	/// <remarks>This URL is used to fetch the latest data for comparison with the local file.</remarks>
	private readonly string databaseUrl;

	/// <summary>The path to the local database file used for comparison.</summary>
	/// <remarks>This path is used to locate the local database file for comparison with the online version.</remarks>
	private readonly string localFilePath;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="CheckDatabaseForm"/> class.</summary>
	/// <param name="url">The URL of the online database file to check.</param>
	/// <param name="localFilePath">The local file path of the database file to compare.</param>
	/// <param name="databaseName">The display name of the database (e.g. "ASTORB.DAT" or "MPCORB.DAT").</param>
	/// <remarks>This constructor initializes the form components and configures the UI for the specified database.</remarks>
	public CheckDatabaseForm(string url, string localFilePath, string databaseName)
	{
		// Initialize the form components
		InitializeComponent();
		// Store the URL and local file path for later use
		this.databaseUrl = url;
		this.localFilePath = localFilePath;
		// Set the form title
		Text = $"Check {databaseName}";
		// Set accessible info
		AccessibleDescription = $"Shows the informations about the {databaseName} database local and online";
		AccessibleName = $"Check {databaseName}";
		// Set label texts for the database file columns
		labelDatabaseFileLocal.Values.Text = $"{databaseName} local";
		labelDatabaseFileLocal.AccessibleDescription = $"Information about the local {databaseName} file";
		labelDatabaseFileLocal.AccessibleName = $"Local {databaseName} file";
		labelDatabaseFileLocal.ToolTipValues.Description = $"Information about the local {databaseName} file";
		labelDatabaseFileLocal.ToolTipValues.Heading = $"Local {databaseName} file";
		labelDatabaseFileOnline.Values.Text = $"{databaseName} online";
		labelDatabaseFileOnline.AccessibleDescription = $"Information about the online {databaseName} file";
		labelDatabaseFileOnline.AccessibleName = $"Online {databaseName} file";
		labelDatabaseFileOnline.ToolTipValues.Description = $"Information about the online {databaseName} file";
		labelDatabaseFileOnline.ToolTipValues.Heading = $"Online {databaseName} file";
	}

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
		dialog.FileName = $"Database-Information-local-online.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
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
			exportAction(tableLayoutPanel, "Information of local and online database", saveFileDialog.FileName);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region task methods

	/// <summary>Retrieves the last modified date of the specified URI.</summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The date of the last modification or <see cref="DateTime.MinValue"/> in case of an error.</returns>
	/// <remarks>This method sends a HEAD request to the specified URI and retrieves the last modified date
	/// from the response headers.</remarks>
	private static async Task<DateTime> GetLastModifiedAsync(Uri uri)
	{
		try
		{
			// Send a HEAD request to the specified URI
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			using HttpResponseMessage response = await client.SendAsync(request: request).ConfigureAwait(continueOnCapturedContext: false);
			// Check if the response is successful and return the last modified date
			return response.IsSuccessStatusCode ? response.Content.Headers.LastModified?.UtcDateTime ?? DateTime.MinValue : DateTime.MinValue;
		}
		catch (HttpRequestException ex)
		{
			// Log the exception
			logger.Error(message: "Error retrieving last modified date.", exception: ex);
			// Show an error message
			ShowErrorMessage(message: ex.Message);
			// Return DateTime.MinValue to indicate an error
			return DateTime.MinValue;
		}
	}

	/// <summary>The content length of the specified URI.</summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The content length or 0 in case of error.</returns>
	/// <remarks>This method sends a HEAD request to the specified URI and retrieves the content length
	/// from the response headers.</remarks>
	private static async Task<long> GetContentLengthAsync(Uri uri)
	{
		try
		{
			// Send a HEAD request to the specified URI
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			using HttpResponseMessage response = await client.SendAsync(request: request).ConfigureAwait(continueOnCapturedContext: false);
			// Check if the response is successful and return the content length
			return response.IsSuccessStatusCode ? response.Content.Headers.ContentLength ?? 0 : 0;
		}
		catch (HttpRequestException ex)
		{
			// Log the exception
			logger.Error(message: "Error retrieving content length.", exception: ex);
			// Show an error message
			ShowErrorMessage(message: ex.Message);
			// Log the exception and return 0
			return 0;
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Event handler for loading the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This event is used to initialize the form's UI elements with information from the database files.</remarks>
	private async void CheckDatabaseForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// URI for the database file
		Uri uri = new(uriString: databaseUrl);
		// Local file last modified date
		DateTime datetimeFileLocal = DateTime.MinValue;
		// Online file last modified date
		DateTime datetimeFileOnline = await GetLastModifiedAsync(uri: uri).ConfigureAwait(continueOnCapturedContext: false);
		// Check if the local file exists
		if (!File.Exists(path: localFilePath))
		{
			// Set the content length and modified date labels to indicate no file found
			labelContentLengthValueLocal.Text = I18nStrings.NoFileFoundText;
			// Set the modified date label to indicate no file found
			labelModifiedDateValueLocal.Text = I18nStrings.NoFileFoundText;
		}
		else
		{
			// Get the last modified date of the local file
			FileInfo fileInfo = new(fileName: localFilePath);
			// Get the file attributes
			datetimeFileLocal = fileInfo.LastWriteTime;
			// Set the content length and modified date labels to the local file's information
			labelContentLengthValueLocal.Text = $"{fileInfo.Length:N0} {I18nStrings.BytesText}";
			// Set the modified date label to the local file's last write time
			labelModifiedDateValueLocal.Text = datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture);
		}
		// Set the content length and modified date labels to the online file's information
		labelContentLengthValueOnline.Text = $"{await GetContentLengthAsync(uri: uri).ConfigureAwait(continueOnCapturedContext: false):N0} {I18nStrings.BytesText}";
		// Set the modified date label to the online file's last modified date
		labelModifiedDateValueOnline.Text = datetimeFileOnline.ToString(provider: CultureInfo.CurrentCulture);
		// Compare the last modified dates of the local and online files
		if (datetimeFileOnline > datetimeFileLocal)
		{
			// Set the label to indicate an update is needed
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_new_16px;
			// Set the label text to indicate an update is recommended
			labelUpdateNeeded.Text = I18nStrings.UpdateRecommendedText;
		}
		else
		{
			// Set the label to indicate no update is needed
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
			// Set the label text to indicate no update is needed
			labelUpdateNeeded.Text = I18nStrings.NoUpdateNeededText;
		}
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>Event handler for double-clicking the "Update Needed" label to check for updates.
	/// Resets the displayed information and reloads the form data.</summary>
	/// <param name="sender">The event source, typically the label being double-clicked.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This event is used to reset the displayed information and reload the form data.</remarks>
	/// <exception cref="ArgumentNullException">Thrown when the sender is null.</exception>
	private void LabelUpdateNeeded_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Reset the displayed information
		labelContentLengthValueLocal.Text = string.Empty;
		labelModifiedDateValueLocal.Text = string.Empty;
		labelContentLengthValueOnline.Text = string.Empty;
		labelModifiedDateValueOnline.Text = string.Empty;
		labelUpdateNeeded.Text = string.Empty;
		labelUpdateNeeded.Values.Image = null;
		// Reload the form data
		CheckDatabaseForm_Load(sender: sender, e: e);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for copying the local database modified date to the clipboard.</summary>
	/// <remarks>If the local modified date is not available, a warning message is displayed and nothing is copied
	/// to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalModifiedDate_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelModifiedDateValueLocal.Text))
		{
			string messageNoLocalModifiedDateToCopyText = "No local modified date available to copy to clipboard.";
			logger.Warn(message: messageNoLocalModifiedDateToCopyText);
			MessageBox.Show(text: messageNoLocalModifiedDateToCopyText, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelModifiedDateValueLocal.Text);
	}

	/// <summary>Handles the click event for copying the local content length value to the clipboard.</summary>
	/// <remarks>If the local content length value is not available, a warning message is displayed and nothing is
	/// copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalContentLength_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelContentLengthValueLocal.Text))
		{
			string messageNoLocalContentLengthToCopyText = "No local content length available to copy to clipboard.";
			logger.Warn(message: messageNoLocalContentLengthToCopyText);
			MessageBox.Show(text: messageNoLocalContentLengthToCopyText, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelContentLengthValueLocal.Text);
	}

	/// <summary>Handles the click event for copying the online modified date value to the clipboard.</summary>
	/// <remarks>If the online modified date value is not available, a warning message is displayed and nothing is
	/// copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineModifiedDate_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelModifiedDateValueOnline.Text))
		{
			string messageNoOnlineModifiedDateToCopyText = "No online modified date available to copy to clipboard.";
			logger.Warn(message: messageNoOnlineModifiedDateToCopyText);
			MessageBox.Show(text: messageNoOnlineModifiedDateToCopyText, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelModifiedDateValueOnline.Text);
	}

	/// <summary>Handles the click event for copying the online content length value to the clipboard.</summary>
	/// <remarks>If the online content length value is not available, a warning message is displayed and nothing is
	/// copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineContentLength_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelContentLengthValueOnline.Text))
		{
			string messageNoOnlineContentLengthToCopyText = "No online content length available to copy to clipboard.";
			logger.Warn(message: messageNoOnlineContentLengthToCopyText);
			MessageBox.Show(text: messageNoOnlineContentLengthToCopyText, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelContentLengthValueOnline.Text);
	}

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as LaTeX.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Markdown.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as AsciiDoc.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a ReStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ReStructuredText.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Textile.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Word.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an OpenDocument Text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as OpenDocument Text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as RTF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Abiword.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as WPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Excel.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ET.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as HTML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as DocBook.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as JSON.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as YAML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TOML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQL.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQLite.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PDF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PostScript.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as EPUB.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as MOBI.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as FictionBook2.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CHM.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

	#endregion
}