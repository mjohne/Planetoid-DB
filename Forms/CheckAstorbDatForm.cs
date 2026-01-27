using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;

namespace Planetoid_DB;

/// <summary>
/// ASTRORB Data Verification Form.
/// </summary>
/// <remarks>
/// This form is used to verify the integrity of ASTRORB data files.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class CheckAstorbDatForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors for the class.
	/// </remarks>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// The HttpClient instance used for making HTTP requests.
	/// </summary>
	/// <remarks>
	/// This HttpClient is used to send requests and receive responses from web services.
	/// </remarks>
	private static readonly HttpClient Client = new();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to store the currently selected control for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CheckAstorbDatForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public CheckAstorbDatForm() =>
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
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method updates the status bar with the provided text and additional information.
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
			var response = await Client.SendAsync(request: new HttpRequestMessage(method: HttpMethod.Head, requestUri: uri)).ConfigureAwait(continueOnCapturedContext: false);
			// Check if the response is successful and return the last modified date
			return response.IsSuccessStatusCode ? response.Content.Headers.LastModified?.UtcDateTime ?? DateTime.MinValue : DateTime.MinValue;
		}
		catch (HttpRequestException)
		{
			// Log the exception
			Logger.Error(message: "Error retrieving last modified date.", exception: new HttpRequestException());
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
			HttpResponseMessage response = await Client.SendAsync(request: new HttpRequestMessage(method: HttpMethod.Head, requestUri: uri)).ConfigureAwait(continueOnCapturedContext: false);
			// Check if the response is successful and return the content length
			return response.IsSuccessStatusCode ? response.Content.Headers.ContentLength ?? 0 : 0;
		}
		catch (HttpRequestException)
		{
			// Log the exception
			Logger.Error(message: "Error retrieving last modified date.", exception: new HttpRequestException());
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
	private async void CheckAstorbDatForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar();
		// URL for the ASTRORB data file
		Uri uriAstorb = new(uriString: Settings.Default.systemAstorbDatUrl);
		// Local file last modified date
		DateTime datetimeFileLocal = DateTime.MinValue;
		// Online file last modified date
		DateTime datetimeFileOnline = await GetLastModifiedAsync(uri: uriAstorb).ConfigureAwait(continueOnCapturedContext: false);
		// Check if the local file exists
		if (!File.Exists(path: Settings.Default.systemFilenameAstorb))
		{
			// Set the content length and modified date labels to indicate no file found
			labelContentLengthValueLocal.Text = I10nStrings.NoFileFoundText;
			// Set the modified date label to indicate no file found
			labelModifiedDateValueLocal.Text = I10nStrings.NoFileFoundText;
		}
		else
		{
			// Get the last modified date of the local file
			FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameAstorb);
			// Get the file attributes
			datetimeFileLocal = fileInfo.LastWriteTime;
			// Set the content length and modified date labels to the local file's information
			labelContentLengthValueLocal.Text = $"{fileInfo.Length:N0} {I10nStrings.BytesText}";
			// Set the modified date label to the local file's last write time
			labelModifiedDateValueLocal.Text = datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture);
		}
		// Set the content length and modified date labels to the online file's information
		labelContentLengthValueOnline.Text = $"{await GetContentLengthAsync(uri: uriAstorb).ConfigureAwait(continueOnCapturedContext: false):N0} {I10nStrings.BytesText}";
		// Set the modified date label to the online file's last modified date
		labelModifiedDateValueOnline.Text = datetimeFileOnline.ToString(provider: CultureInfo.CurrentCulture);
		// Compare the last modified dates of the local and online files
		if (datetimeFileOnline > datetimeFileLocal)
		{
			// Set the label to indicate an update is needed
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_new_16px;
			// Set the label text to indicate an update is recommended
			labelUpdateNeeded.Text = I10nStrings.UpdateRecommendedText;
		}
		else
		{
			// Set the label to indicate no update is needed
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
			// Set the label text to indicate no update is needed
			labelUpdateNeeded.Text = I10nStrings.NoUpdateNeededText;
		}
	}

	/// <summary>
	/// Event handler for closing the form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to clean up resources when the form is closed.
	/// </remarks>
	private void CheckAstorbDatForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This event is used to update the status bar when a control or ToolStrip item is focused.
	/// </remarks>
	private void SetStatusBar_Enter(object sender, EventArgs e)
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
	/// This event is used to clear the status bar when the mouse leaves a control or the control loses focus.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

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
			// Try to set the clipboard text
			try { CopyToClipboard(text: textToCopy); }
			catch
			{ // Throw an exception
				throw new ArgumentException(message: "Unsupported sender type", paramName: nameof(sender));
			}
		}
	}

	/// <summary>
	/// Event handler for double-clicking the "Update Needed" label to check for updates.
	/// Resets the displayed information and reloads the form data.
	/// </summary>
	/// <param name="sender">The event source, typically the label being double-clicked.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This event is used to reset the displayed information and reload the form data.
	/// </remarks>
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
		CheckAstorbDatForm_Load(sender: sender, e: e);
	}

	#endregion
}
