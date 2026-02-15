using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>
/// Form responsible for downloading and installing the MPCORB.DAT database file.
/// Provides UI helpers, progress reporting and both WebClient- and HttpClient-based download paths.
/// </summary>
/// <remarks>
/// This form is used to download and install the MPCORB.DAT database file.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DownloadMpcorbDatForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Cancellation token source used to cancel an ongoing download operation.
	/// May be null when no download is active.
	/// </summary>
	/// <remarks>
	/// This token is used to cancel the download operation if needed.
	/// </remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>
	/// Filename (full path) for the local MPCORB data file.
	/// </summary>
	/// <remarks>
	/// This is the full path to the local MPCORB data file.
	/// </remarks>
	private readonly string strFilenameMpcorb = Settings.Default.systemFilenameMpcorb;

	/// <summary>
	/// Temporary filename (full path) used while downloading the MPCORB data file.
	/// </summary>
	/// <remarks>
	/// This is the full path to the temporary MPCORB data file.
	/// </remarks>
	private readonly string strFilenameMpcorbTemp = Settings.Default.systemFilenameMpcorbTemp;

	/// <summary>
	/// Remote URI of the MPCORB.GZ resource to download.
	/// </summary>
	/// <remarks>
	/// This is the remote URI of the MPCORB.GZ resource to download.
	/// </remarks>
	private readonly Uri strUriMpcorb = new(uriString: Settings.Default.systemMpcorbDatGzUrl);

	/// <summary>
	/// Instance of <see cref="WebClient"/> used for the legacy async download path.
	/// </summary>
	/// <remarks>
	/// This is the WebClient instance used for downloading the MPCORB.GZ file.
	/// </remarks>
	private readonly WebClient webClient = new();

	/// <summary>
	/// Shared <see cref="HttpClient"/> used for HTTP requests. Initialized in the constructor.
	/// Reuse to avoid socket exhaustion.
	/// </summary>
	/// <remarks>
	/// This HttpClient instance is reused for all HTTP requests to improve performance.
	/// </remarks>
	private static readonly HttpClient httpClient = new(handler: new HttpClientHandler
	{
		AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
	})
	{
		Timeout = TimeSpan.FromMinutes(10)
	};

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to store the currently selected control for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="DownloadMpcorbDatForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public DownloadMpcorbDatForm() =>
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
	/// Extracts a GZIP-compressed file to a specified output file.
	/// </summary>
	/// <remarks>
	/// This method is used to extract a GZIP-compressed file to a specified output file.
	/// </remarks>
	private static void ExtractGzipFile(string gzipFilePath, string outputFilePath)
	{
		// Open the gzip file and create a new file stream for the output file
		using FileStream originalFileStream = new(path: gzipFilePath, mode: FileMode.Open, access: FileAccess.Read);
		// Create a new file stream for the output file
		using FileStream decompressedFileStream = new(path: outputFilePath, mode: FileMode.Create, access: FileAccess.Write);
		// Create a new GZipStream for decompression
		using GZipStream decompressionStream = new(stream: originalFileStream, mode: CompressionMode.Decompress);
		// Copy the decompressed data to the output file stream
		decompressionStream.CopyTo(destination: decompressedFileStream);
	}

	/// <summary>
	/// Asynchronously retrieves the Last-Modified date of a resource.
	/// </summary>
	/// <param name="uri">The URI of the resource to query.</param>
	/// <param name="client">An <see cref="HttpClient"/> used to send the request.</param>
	/// <returns>
	/// The last modified <see cref="DateTime"/> in UTC if the header is present and the request succeeds;
	/// otherwise <see cref="DateTime.MinValue"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> or <paramref name="client"/> is null.</exception>
	/// <remarks>
	/// The method logs and displays errors and returns <see cref="DateTime.MinValue"/> on failure.
	/// A HEAD request is used to avoid downloading the response body.
	/// </remarks>
	private static async Task<DateTime?> GetLastModifiedAsync(Uri uri, HttpClient client)
	{
		// Validate input parameters
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		using HttpResponseMessage response = await client.SendAsync(request);
		// Return the Last-Modified date if available
		return !response.IsSuccessStatusCode ? null : (response.Content.Headers.LastModified?.UtcDateTime);
	}

	/// <summary>
	/// Gets the content length of the specified URI.
	/// </summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>The content length of the URI.</returns>
	/// <remarks>
	/// This method is used to retrieve the content length of a resource at the specified URI.
	/// </remarks>
	private static long GetContentLength(Uri uri)
	{
		try
		{
			// Create a new HttpRequestMessage with the HEAD method and the specified URI
			// Send the request and get the response
			// If the response is successful, return the content length or 0 if not available
			// If the response is not successful, return 0
			HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			// Send the request using the HttpClient instance
			HttpResponseMessage response = httpClient.Send(request: request);
			// Check if the response is successful and return the content length
			if (response.IsSuccessStatusCode)
			{
				// Return the content length or 0 if not available
				return response.Content.Headers.ContentLength ?? 0;
			}
			else
			{
				// Return 0 to indicate an error
				return 0;
			}
		}
		// Catch any exceptions that occur during the request
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: "Error retrieving content length.");
			// Show an error message with the exception message
			ShowErrorMessage(message: $"Error retrieving content length: {ex.Message}");
			// Return 0 to indicate an error
			return 0;
		}
	}

	/// <summary>
	/// Shows the MPCORB data check form.
	/// </summary>
	/// <remarks>
	/// This method is used to display the MPCORB data check form.
	/// </remarks>
	private static void ShowMpcorbDatCheck()
	{
		// Check if there is an internet connection available
		// If there is, create and show the CheckMpcorbDatForm
		// Otherwise, log the error and show an error message
		if (NetworkInterface.GetIsNetworkAvailable())
		{
			// Create and show the CheckMpcorbDatForm
			using CheckMpcorbDatForm formCheckMpcorbDat = new();
			// Show the form as a dialog
			_ = formCheckMpcorbDat.ShowDialog();
		}
		else
		{
			// Log the error if there is no internet connection
			logger.Error(message: "No internet connection available.");
			// Show an error message if there is no internet connection
			ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
		}
	}

	/// <summary>
	/// Handles the DownloadProgressChanged event of the WebClient control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="DownloadProgressChangedEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to update the progress bar and label with the current download progress.
	/// </remarks>
	private void ProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
	{
		// Set the progress bar style to continuous
		progressBarDownload.Value = e.ProgressPercentage;
		// Update the label with the current progress percentage
		labelDownload.Text = e.ProgressPercentage + I10nStrings.PercentSign;
		// Update the status bar with the current progress
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: e.ProgressPercentage, progressMax: 100);
	}

	/// <summary>
	/// Handles the DownloadFileCompleted event of the WebClient control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="AsyncCompletedEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to handle the completion of the download operation.
	/// </remarks>
	private async void Completed(object? sender, AsyncCompletedEventArgs e)
	{
		// Reset the taskbar progress
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
		if (e.Error == null)
		{
			// Set the status to "Refreshing database"
			labelStatusValue.Text = I10nStrings.StatusRefreshingDatabaseText;
			// Delete the existing file if it exists
			File.Delete(path: strFilenameMpcorb);
			// Set the progress bar style to marquee
			progressBarDownload.Style = ProgressBarStyle.Marquee;
			// Create a new CancellationTokenSource for this download
			cancellationTokenSource = new CancellationTokenSource();
			// Extract the downloaded GZIP file
			await Task.Run(action: () =>
				ExtractGzipFile(gzipFilePath: strFilenameMpcorbTemp, outputFilePath: strFilenameMpcorb),
				cancellationToken: cancellationTokenSource.Token);
			// Set the status to "Download complete"
			labelStatusValue.Text = I10nStrings.StatusDownloadCompleteText;
			// Enable the download and check for update buttons
			buttonDownload.Enabled = buttonCheckForUpdate.Enabled = true;
			// Set the dialog result to OK
			DialogResult = DialogResult.OK;
		}
		else
		{
			// Handle the error
			labelStatusValue.Text = e.Cancelled ? I10nStrings.StatusDownloadCancelled : $"{I10nStrings.StatusUnknownError} {e.Error}";
			// Clear the labels
			labelSourceValue.Text = labelDateValue.Text = labelSizeValue.Text = string.Empty;
			// Enable the download button
			buttonDownload.Enabled = true;
			// Enable the check for update button
			buttonCheckForUpdate.Enabled = true;
			// Disable the cancel button
			buttonCancelDownload.Enabled = false;
			// Reset the progress bar
			progressBarDownload.Value = 0;
			// Reset the download label
			labelDownload.Text = $"{progressBarDownload.Value}{I10nStrings.PercentSign}";
		}
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the DownloadMpcorbDatForm control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form and its controls.
	/// </remarks>

	[Obsolete(message: "Obsolete")]
	private void DownloadMpcorbDatForm_Load(object? sender, EventArgs? e)
	{
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
		// Set the initial status to "Nothing to do"
		labelStatusValue.Text = I10nStrings.StatusNothingToDoText;
		// Clear the labels
		labelDateValue.Text = labelSizeValue.Text = labelSourceValue.Text = "";
		// Hide the labels and disable the cancel button
		labelDateValue.Visible = labelSizeValue.Visible = labelSizeValue.Visible = labelSourceValue.Visible = buttonCancelDownload.Enabled = false;
		// Set the initial download progress to 0%
		labelDownload.Text = I10nStrings.NumberZero + I10nStrings.PercentSign;
		// Set the proxy to null to avoid using any proxy settings
		webClient.Proxy = null;
		// Event handler for download completion
		webClient.DownloadFileCompleted += Completed;
		// Event handler for download progress
		webClient.DownloadProgressChanged += ProgressChanged;
	}

	/// <summary>
	/// Handles the FormClosed event of the DownloadMpcorbDatForm control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to clean up resources when the form is closed.
	/// </remarks>
	[Obsolete(message: "Obsolete")]
	private void DownloadMpcorbDatForm_FormClosed(object? sender, FormClosedEventArgs? e)
	{
		webClient.CancelAsync();
		webClient.Dispose();
		cancellationTokenSource?.Dispose();
		Dispose();
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
			SetStatusBar(label: labelInformation, text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object? sender, EventArgs? e) => ClearStatusBar(label: labelInformation);

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the Download button to start the download process.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to start the download process when the Download button is clicked.
	/// </remarks>
	[Obsolete(message: "Obsolete")]
	private async void ButtonDownload_Click(object? sender, EventArgs? e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Check if there is an internet connection available
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log the error if there is no internet connection
			logger.Error(message: "No internet connection available.");
			// Show an error message if there is no internet connection
			ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
			return;
		}
		// Disable the download button
		buttonDownload.Enabled = false;
		// Enable the cancel button
		buttonCancelDownload.Enabled = true;
		// Disable the check for update button
		buttonCheckForUpdate.Enabled = false;
		// Set the source value to the URI
		labelSourceValue.Text = strUriMpcorb.AbsoluteUri;
		// Make the source value visible
		labelSourceValue.Visible = true;
		// Get the last modified date of the URI
		//labelDateValue.Text = GetLastModified(uri: strUriMpcorb).ToUniversalTime().ToString(provider: CultureInfo.CurrentCulture);
		string url = strUriMpcorb.AbsoluteUri;
		labelDateValue.Text = (await GetLastModifiedAsync(uri: new Uri(uriString: url), client: httpClient)).ToString();
		// Make the date value visible
		labelDateValue.Visible = true;
		// Set the size value to the content length of the URI
		labelSizeValue.Text = $"{GetContentLength(uri: strUriMpcorb):N0} {I10nStrings.BytesText}";
		// Make the size value visible
		labelSizeValue.Visible = true;
		// Set the status value to "Try to connect"
		labelStatusValue.Text = I10nStrings.StatusTryToConnect;
		//Try to download the file
		try
		{
			// Set the status value to "Downloading"
			labelStatusValue.Text = I10nStrings.StatusDownloading;
			// Start the download asynchronously
			webClient.DownloadFileAsync(address: strUriMpcorb, fileName: strFilenameMpcorbTemp);
		}
		catch (Exception ex)
		{
			// Set the status value to "Unknown error"
			labelStatusValue.Text = $"{I10nStrings.StatusUnknownError} {ex.Message}";
			// Enable the download button
			buttonDownload.Enabled = true;
			// Enable the check for update button
			buttonCheckForUpdate.Enabled = true;
			// Log and show an error message
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"{I10nStrings.StatusUnknownError} {ex.Message}");
		}
	}

	// Update the ButtonDownload_Click method to use HttpClient for downloading the file.
	private async void ButtonDownload2_Click(object? sender, EventArgs? e)
	{
		ArgumentNullException.ThrowIfNull(argument: sender);

		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			logger.Error(message: "No internet connection available.");
			ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
			return;
		}

		buttonDownload.Enabled = false;
		buttonCancelDownload.Enabled = true;
		buttonCheckForUpdate.Enabled = false;

		labelSourceValue.Text = strUriMpcorb.AbsoluteUri;
		labelSourceValue.Visible = true;

		// Get the last modified date of the URI
		//labelDateValue.Text = GetLastModified(uri: strUriMpcorb).ToUniversalTime().ToString(provider: CultureInfo.CurrentCulture);
		try
		{
			string url = strUriMpcorb.AbsoluteUri;
			labelDateValue.Text = (await GetLastModifiedAsync(uri: new Uri(uriString: url), client: httpClient)).ToString();
			labelDateValue.Visible = true;
		}
		catch (Exception ex)
		{
			labelStatusValue.Text = $"{I10nStrings.StatusUnknownError} {ex.Message}";
			buttonDownload.Enabled = true;
			buttonCancelDownload.Enabled = false;
			buttonCheckForUpdate.Enabled = true;
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"{I10nStrings.StatusUnknownError} {ex.Message}");
			return;
		}
		labelSizeValue.Text = $"{GetContentLength(uri: strUriMpcorb):N0} {I10nStrings.BytesText}";
		labelSizeValue.Visible = true;

		labelStatusValue.Text = I10nStrings.StatusTryToConnect;

		try
		{
			labelStatusValue.Text = I10nStrings.StatusDownloading;

			// Ersetze in der Methode ButtonDownload2_Click die Zeile:
			// using HttpResponseMessage response = await HttpClient.GetAsync(requestUri: strUriMpcorb, completionOption: HttpCompletionOption.ResponseHeadersRead);
			// durch:
			using HttpResponseMessage response = await httpClient.GetAsync(requestUri: strUriMpcorb, completionOption: HttpCompletionOption.ResponseHeadersRead);
			_ = response.EnsureSuccessStatusCode();

			using Stream contentStream = await response.Content.ReadAsStreamAsync();
			using FileStream fileStream = new(path: strFilenameMpcorbTemp, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
			await contentStream.CopyToAsync(destination: fileStream);

			labelStatusValue.Text = I10nStrings.StatusRefreshingDatabaseText;

			File.Delete(path: strFilenameMpcorb);
			// Extract the downloaded GZIP file
			progressBarDownload.Style = ProgressBarStyle.Marquee;
			await Task.Run(action: () =>
				ExtractGzipFile(gzipFilePath: strFilenameMpcorbTemp, outputFilePath: strFilenameMpcorb));
			labelStatusValue.Text = I10nStrings.StatusDownloadCompleteText;
			buttonDownload.Enabled = buttonCheckForUpdate.Enabled = true;
			DialogResult = DialogResult.OK;
		}
		catch (Exception ex)
		{
			labelStatusValue.Text = $"{I10nStrings.StatusUnknownError} {ex.Message}";
			buttonDownload.Enabled = true;
			buttonCheckForUpdate.Enabled = true;
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"{I10nStrings.StatusUnknownError} {ex.Message}");
		}
	}

	/// <summary>
	/// Handles the Click event of the Cancel Download button.
	/// Cancels the ongoing download operation if one is in progress.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to cancel the ongoing download operation if one is in progress.
	/// </remarks>
	[Obsolete(message: "Obsolete")]
	private void ButtonCancelDownload_Click(object? sender, EventArgs? e) => webClient.CancelAsync();

	/// <summary>
	/// Handles the FormClosing event of the DownloadMpcorbDatForm control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to cancel the ongoing download operation if one is in progress.
	/// </remarks>
	[Obsolete(message: "Obsolete")]
	private void DownloadMpcorbDatForm_FormClosing(object? sender, FormClosingEventArgs? e)
	{
		// Check if the form is closing and if a download is in progress
		// Cancel the download if it is in progress
		webClient.CancelAsync();
		// Dispose of the WebClient instance
		if (File.Exists(path: strFilenameMpcorbTemp))
		{
			// Delete the temporary file if it exists
			File.Delete(path: strFilenameMpcorbTemp);
		}
	}

	/// <summary>
	/// Handles the Click event of the Check for Update button to show the MPCORB data check form.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method is used to show the MPCORB data check form.
	/// </remarks>
	private void ButtonCheckForUpdate_Click(object? sender, EventArgs? e) => ShowMpcorbDatCheck();

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