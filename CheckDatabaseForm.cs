// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Database Data Verification Form.</summary>
/// <remarks>This form is used to verify the integrity of database data files (e.g. ASTORB.DAT or MPCORB.DAT).</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class CheckDatabaseForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Information of local and online database";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Database-Information-local-online";

	/// <summary>Prepares the save dialog used for export operations.</summary>
	/// <param name="dialog">The dialog to configure before it is displayed.</param>
	/// <param name="ext">The file extension selected for the export.</param>
	/// <returns><see langword="true"/> if the user confirms the dialog; otherwise, <see langword="false"/>.</returns>
	/// <remarks>Overrides the default file naming to preserve the legacy non-timestamped export file name.</remarks>
	protected override bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"Database-Information-local-online.{ext}";
		return dialog.ShowDialog(owner: this) == DialogResult.OK;
	}

	#endregion

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
		AccessibleDescription = $"Shows the information about the {databaseName} database local and online";
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

	/// <summary>Copies the specified text to the clipboard.</summary>
	/// <param name="value">The text to copy to the clipboard.</param>
	/// <param name="propertyName">The name of the property being copied, used for logging and messages.</param>
	/// <remarks>This method checks if the value is null or whitespace before copying to the clipboard. If the value is invalid, it logs a warning and shows a message box.</remarks>
	private void CopyToClipboardSafe(string value, string propertyName)
	{
		// Check if the value is null or whitespace
		if (string.IsNullOrWhiteSpace(value))
		{
			// Log a warning and show a message box if the value is invalid
			string message = $"No {propertyName} available to copy to clipboard.";
			logger.Warn(message: message);
			_ = KryptonMessageBox.Show(owner: this, text: message, caption: "Warning", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Copy the value to the clipboard
		CopyToClipboard(text: value);
	}

	#endregion

	#region task methods

	/// <summary>Retrieves the last modified date and content length of the specified URI in a single request.</summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>A tuple containing the last modified date and the content length.</returns>
	/// <remarks>This method sends a HEAD request to the specified URI and retrieves both the last modified date and content length from the response headers.</remarks>
	private static async Task<(DateTime LastModified, long ContentLength)> GetOnlineFileInfoAsync(Uri uri)
	{
		// Send a HEAD request to the specified URI and retrieve the last modified date and content length
		try
		{
			// Send a HEAD request to the specified URI
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			// Send the request and get the response
			using HttpResponseMessage response = await client.SendAsync(request: request).ConfigureAwait(continueOnCapturedContext: false);
			// Check if the response is successful
			if (response.IsSuccessStatusCode)
			{
				// Retrieve the last modified date and content length from the response headers
				DateTime lastModified = response.Content.Headers.LastModified?.UtcDateTime ?? DateTime.MinValue;
				long contentLength = response.Content.Headers.ContentLength ?? 0;
				// Return the last modified date and content length as a tuple
				return (lastModified, contentLength);
			}
		}
		// Catch any HttpRequestException that occurs during the request
		catch (HttpRequestException ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: "Error retrieving online file information (HEAD request failed).");
			ShowErrorMessage(message: ex.Message);
		}
		// Return default values in case of error
		return (DateTime.MinValue, 0);
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
		// Get the last modified date and content length of the online file in a single request
		(DateTime datetimeFileOnline, long contentLengthOnline) = await GetOnlineFileInfoAsync(uri);
		// Local file last modified date
		DateTime datetimeFileLocal = DateTime.MinValue;
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
		labelContentLengthValueOnline.Text = $"{contentLengthOnline:N0} {I18nStrings.BytesText}";
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

	/// <summary>Event handler for double-clicking the "Update Needed" label to check for updates. Resets the displayed information and reloads the form data.</summary>
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
	/// <remarks>If the local modified date is not available, a warning message is displayed and nothing is copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalModifiedDate_Click(object sender, EventArgs e) => CopyToClipboardSafe(value: labelModifiedDateValueLocal.Text, propertyName: "local modified date");

	/// <summary>Handles the click event for copying the local content length value to the clipboard.</summary>
	/// <remarks>If the local content length value is not available, a warning message is displayed and nothing is copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalContentLength_Click(object sender, EventArgs e) => CopyToClipboardSafe(value: labelContentLengthValueLocal.Text, propertyName: "local content length");

	/// <summary>Handles the click event for copying the online modified date value to the clipboard.</summary>
	/// <remarks>If the online modified date value is not available, a warning message is displayed and nothing is copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineModifiedDate_Click(object sender, EventArgs e) => CopyToClipboardSafe(value: labelModifiedDateValueOnline.Text, propertyName: "online modified date");

	/// <summary>Handles the click event for copying the online content length value to the clipboard.</summary>
	/// <remarks>If the online content length value is not available, a warning message is displayed and nothing is copied to the clipboard.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineContentLength_Click(object sender, EventArgs e) => CopyToClipboardSafe(value: labelContentLengthValueOnline.Text, propertyName: "online content length");

	#endregion
}