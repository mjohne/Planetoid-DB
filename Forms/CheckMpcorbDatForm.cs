// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>
/// MPCORB Data Verification Form.
/// </summary>
/// <remarks>
/// This form is used to verify the integrity of MPCORB data files.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class CheckMpcorbDatForm : BaseKryptonForm
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
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CheckMpcorbDatForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public CheckMpcorbDatForm() =>
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
		catch (HttpRequestException)
		{
			// Log the exception
			logger.Error(message: "Error retrieving last modified date.", exception: new HttpRequestException());
			// Show an error message
			ShowErrorMessage(message: new HttpRequestException().Message);
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
		catch (HttpRequestException)
		{
			// Log the exception
			logger.Error(message: "Error retrieving last modified date.", exception: new HttpRequestException());
			// Show an error message
			ShowErrorMessage(message: new HttpRequestException().Message);
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
	/// This event is used to initialize the form's UI elements with information from the assembly.
	/// </remarks>
	private async void CheckMpcorbDatForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// URL for the MPCORB data file
		Uri uriMpcorb = new(uriString: Settings.Default.systemMpcorbDatUrl);
		// Local file last modified date
		DateTime datetimeFileLocal = DateTime.MinValue;
		// Online file last modified date
		DateTime datetimeFileOnline = await GetLastModifiedAsync(uri: uriMpcorb).ConfigureAwait(continueOnCapturedContext: false);
		// Check if the local file exists
		if (!File.Exists(path: Settings.Default.systemFilenameMpcorb))
		{
			// Set the content length and modified date labels to indicate no file found
			labelContentLengthValueLocal.Text = I18nStrings.NoFileFoundText;
			// Set the modified date label to indicate no file found
			labelModifiedDateValueLocal.Text = I18nStrings.NoFileFoundText;
		}
		else
		{
			// Get the last modified date of the local file
			FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameMpcorb);
			// Get the file attributes
			datetimeFileLocal = fileInfo.LastWriteTime;
			// Set the content length and modified date labels to the local file's information
			labelContentLengthValueLocal.Text = $"{fileInfo.Length:N0} {I18nStrings.BytesText}";
			// Set the modified date label to the local file's last write time
			labelModifiedDateValueLocal.Text = datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture);
		}
		// Set the content length and modified date labels to the online file's information
		labelContentLengthValueOnline.Text = $"{await GetContentLengthAsync(uri: uriMpcorb).ConfigureAwait(continueOnCapturedContext: false):N0} {I18nStrings.BytesText}";
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
		CheckMpcorbDatForm_Load(sender: sender, e: e);
	}

	#endregion
}

