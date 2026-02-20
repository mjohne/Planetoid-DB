// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Resources;

using System.Globalization;

namespace Planetoid_DB;

/// <summary>
/// Database Data Verification Form.
/// </summary>
/// <remarks>
/// This form is used to verify the integrity of database data files (e.g. ASTORB.DAT or MPCORB.DAT).
/// </remarks>
public partial class CheckDatabaseForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors for the class.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// The HttpClient instance used for making HTTP requests.
	/// </summary>
	/// <remarks>
	/// This HttpClient is used to send requests and receive responses from web services.
	/// </remarks>
	private static readonly HttpClient client = new();

	/// <summary>
	/// The URL used to retrieve information about the online database file.
	/// </summary>
	private readonly string databaseUrl;

	/// <summary>
	/// The path to the local database file used for comparison.
	/// </summary>
	private readonly string localFilePath;

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CheckDatabaseForm"/> class.
	/// </summary>
	/// <param name="url">The URL of the online database file to check.</param>
	/// <param name="localFilePath">The local file path of the database file to compare.</param>
	/// <param name="databaseName">The display name of the database (e.g. "ASTORB.DAT" or "MPCORB.DAT").</param>
	/// <remarks>
	/// This constructor initializes the form components and configures the UI for the specified database.
	/// </remarks>
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

	#region task methods

	/// <summary>
	/// Retrieves the last modified date of the specified URI.
	/// </summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The date of the last modification or <see cref="DateTime.MinValue"/> in case of an error.</returns>
	/// <remarks>
	/// This method sends a HEAD request to the specified URI and retrieves the last modified date
	/// from the response headers.
	/// </remarks>
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

	/// <summary>
	/// The content length of the specified URI.
	/// </summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The content length or 0 in case of error.</returns>
	/// <remarks>
	/// This method sends a HEAD request to the specified URI and retrieves the content length
	/// from the response headers.
	/// </remarks>
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

	/// <summary>
	/// Event handler for loading the form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to initialize the form's UI elements with information from the database files.
	/// </remarks>
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

	/// <summary>
	/// Event handler for double-clicking the "Update Needed" label to check for updates.
	/// Resets the displayed information and reloads the form data.
	/// </summary>
	/// <param name="sender">The event source, typically the label being double-clicked.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This event is used to reset the displayed information and reload the form data.
	/// </remarks>
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
}
