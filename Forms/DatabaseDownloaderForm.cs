using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace Planetoid_DB
{
	/// <summary>
	/// Form to handle downloading updates for the application.
	/// </summary>
	/// <remarks>
	/// This form provides a user interface for downloading and installing updates.
	/// </remarks>
	[DebuggerDisplay(value: $"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public partial class DatabaseDownloaderForm : BaseKryptonForm
	{
		/// <summary>
		/// NLog logger instance for the class.
		/// </summary>
		/// <remarks>
		/// This logger is used to log messages for the database downloader.
		/// </remarks>
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
		/// Temporary filename (including path) used to store the downloaded gzip file.
		/// </summary>
		/// <remarks>
		/// This temporary file is used to store the downloaded gzip file before extraction.
		/// </remarks>
		private readonly string strFilenameTemp = Settings.Default.systemFilenameTemp;

		/// <summary>
		/// Source URL to download the file from. Provided by the caller when the form is created.
		/// </summary>
		/// <remarks>
		/// This URL is used to initiate the download process.
		/// </remarks>
		private readonly string url;

		/// <summary>
		/// Output path (filename without extension) where the downloaded archive will be extracted.
		/// </summary>
		/// <remarks>
		/// This path is derived from the URL and is used to extract the contents of the downloaded archive.
		/// </remarks>
		private readonly string extractFilePath;

		/// <summary>
		/// Cancellation token source used to cancel an ongoing download operation.
		/// May be null when no download is active.
		/// </summary>
		/// <remarks>
		/// This token is used to cancel the download operation if needed.
		/// </remarks>
		private CancellationTokenSource? cancellationTokenSource;

		/// <summary>
		/// Stores the currently selected control for clipboard operations.
		/// </summary>
		/// <remarks>
		/// This field is used to store the currently selected control for clipboard operations.
		/// </remarks>
		private Control currentControl;

		/// <summary>
		/// Stores the current tag text of the control.
		/// </summary>
		/// <remarks>
		/// This field is used to store the current tag text of the control.
		/// </remarks>
		private string currentTagText = string.Empty;

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseDownloaderForm"/> class.
		/// Prepares HTTP client and UI, stores the download <paramref name="url"/>, derives the extraction path
		/// and starts the download workflow.
		/// </summary>
		/// <param name="url">The URL to download the file from. The filename part is used to derive the extraction path.</param>
		/// <remarks>
		/// This constructor performs UI initialization and immediately begins the download by calling <see cref="StartDownloadAsync"/>.
		/// It must be called from the UI thread.
		/// </remarks>
		public DatabaseDownloaderForm(string url)
		{
			InitializeComponent();
			using HttpClient client = new();
			this.url = url;
			extractFilePath = Path.GetFileNameWithoutExtension(path: url);
			Shown += async (_, _) => await StartDownloadAsync();
		}

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
		/// Extracts a GZIP-compressed file to the specified output file.
		/// </summary>
		/// <param name="gzipFilePath">Full path to the source .gz file.</param>
		/// <param name="outputFilePath">Full path where the decompressed file will be written.</param>
		/// <remarks>
		/// The method streams the compressed input to the output file using <see cref="GZipStream"/>.
		/// It throws exceptions (e.g. <see cref="IOException"/>, <see cref="InvalidDataException"/>) to the caller.
		/// </remarks>
		protected static void ExtractGzipFile(string gzipFilePath, string outputFilePath)
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

		/// <summary>
		/// Checks if the device has an active internet connection.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> instance to use for the request.</param>
		/// <param name="url">The URL to check for internet connectivity.</param>
		/// <returns><c>true</c> if the device has an active internet connection; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// This method sends a GET request to the specified URL and checks the response status.
		/// </remarks>
		private static async Task<bool> HasInternetAsync(HttpClient client, string url)
		{
			// Send a GET request to the specified URL
			// and check if the response indicates success
			// Return true if the response is successful; otherwise, return false
			// Catch any exceptions and return false
			try
			{
				using HttpResponseMessage response = await client.GetAsync(
					requestUri: url, // The URL to check
					completionOption: HttpCompletionOption.ResponseHeadersRead // Only read headers to minimize data usage
				);
				return response.IsSuccessStatusCode;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Starts the download workflow: validates network and input, disables/enables UI controls,
		/// downloads the file, extracts the GZIP archive and notifies the user.
		/// </summary>
		/// <remarks>
		/// This method is an async void entry point intended for UI usage. It uses an internal
		/// <see cref="CancellationTokenSource"/> to support cancellation, updates form controls
		/// (buttons, labels, progress bar) and handles exceptions internally (shows message boxes).
		/// No exceptions are propagated to the caller.
		/// </remarks>
		private async Task StartDownloadAsync()
		{
			// Check if there is an internet connection available
			// If not, log the error and show an error message
			// Then return without proceeding further
			// This prevents attempting to download when offline.

			if (!NetworkInterface.GetIsNetworkAvailable())
			//if (!await HasInternetAsync(client: httpClient, url: url))
			{
				// Log the error if there is no internet connection
				Logger.Error(message: "No internet connection available.");
				// Show an error message if there is no internet connection
				ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
				return;
			}
			// Validate the URL and temporary filename
			// If invalid, show an error message and return
			// Start the download and extraction process
			// Update UI elements accordingly
			// Handle cancellation and errors gracefully
			// Finally, reset UI elements and clean up resources
			if (string.IsNullOrWhiteSpace(value: url))
			{
				// Show an error message if the URL is invalid
				_ = MessageBox.Show(text: "Please enter a valid URL!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
				return;
			}

			// Check if the temporary filename is valid
			// If invalid, show an error message and return
			// This ensures we have a valid location to save the downloaded file
			// before starting the download process.
			// If the filename is invalid, we cannot proceed with the download.
			// Show an error message to inform the user.
			if (string.IsNullOrWhiteSpace(value: strFilenameTemp))
			{
				// Show an error message if the temporary filename is invalid
				_ = MessageBox.Show(text: "Please select a save location!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
				return;
			}

			// Start the download process
			// Disable the Download button and enable the Cancel button
			// Reset progress bar and status labels
			// Use a CancellationTokenSource to support cancellation
			// Await the asynchronous download method
			// On success, extract the downloaded file and notify the user
			// Handle cancellation and errors appropriately
			// Finally, reset UI elements and clean up resources
			try
			{
				// Disable the Download button and enable the Cancel button
				buttonDownload.Enabled = false;
				buttonCancel.Enabled = true;
				// Reset status labels and progress bar
				progressBarDownload.Value = 0;
				progressBarDownload.Text = "0%";
				// Create a new CancellationTokenSource for this download
				cancellationTokenSource = new CancellationTokenSource();
				// Start downloading the file asynchronously
				await DownloadFileAsync(url: url, filePath: strFilenameTemp, cancellationToken: cancellationTokenSource.Token);
				// Extract the downloaded GZIP file
				labelStatusValue.Text = "Extracting...";
				progressBarDownload.Style = ProgressBarStyle.Marquee;
				await Task.Run(action: () =>
					ExtractGzipFile(gzipFilePath: strFilenameTemp, outputFilePath: extractFilePath),
					cancellationToken: cancellationTokenSource.Token);
				// Notify the user of successful completion
				labelStatusValue.Text = "Download completed";
				_ = MessageBox.Show(text: "Download completed successfully!", caption: "Finished", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
				Logger.Info(message: "Download and extraction completed successfully.");
				// Set the dialog result to OK and close the form
				DialogResult = DialogResult.OK;
				Close();
			}
			// Handle cancellation
			// Notify the user that the download was canceled
			// Handle other exceptions and notify the user of errors
			// Finally, reset UI elements and clean up resources
			catch (OperationCanceledException)
			{
				// Notify the user that the download was canceled
				labelStatusValue.Text = "Download canceled";
				_ = MessageBox.Show(text: "Download canceled!", caption: "Canceled", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
				Logger.Info(message: "Download canceled by user.");
				DialogResult = DialogResult.Cancel;
			}
			// Handle other exceptions
			// Notify the user of errors
			// Log the exception for debugging purposes
			// Finally, reset UI elements and clean up resources
			catch (Exception ex)
			{
				// Log the exception and show an error message
				labelStatusValue.Text = "Download error";
				_ = MessageBox.Show(text: $"Error during download: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
				Logger.Error(exception: ex, message: "Download failed. Url={Url}, TempFile={TempFile}", url, strFilenameTemp);
				DialogResult = DialogResult.Abort;
			}
			// Reset UI elements and clean up resources
			finally
			{
				// Reset UI elements
				buttonDownload.Enabled = true;
				buttonCancel.Enabled = false;
				// Reset status labels and progress bar
				labelDateValue.Text = "-";
				labelSizeValue.Text = "-";
				labelSourceValue.Text = "-";
				progressBarDownload.Value = 0;
				progressBarDownload.Text = "0%";
				// Dispose of the CancellationTokenSource
				cancellationTokenSource?.Dispose();
				cancellationTokenSource = null;
			}
		}

		/// <summary>
		/// Downloads a file asynchronously from the specified <paramref name="url"/> and writes it to <paramref name="filePath"/>.
		/// Updates UI elements (status, progress, size, source, date) while downloading and supports cancellation via <paramref name="cancellationToken"/>.
		/// </summary>
		/// <param name="url">The URL to download from.</param>
		/// <param name="filePath">Local file path where the downloaded data will be saved.</param>
		/// <param name="cancellationToken">Token used to cancel the download operation.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous download operation.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled through <paramref name="cancellationToken"/>.</exception>
		/// <exception cref="HttpRequestException">Thrown when the HTTP request fails (non-success status code).</exception>
		/// <remarks>
		/// The method streams the response to disk to avoid buffering the entire response in memory.
		/// Progress is reported by updating the form's progress bar when the content length is known.
		/// </remarks>
		private async Task DownloadFileAsync(string url, string filePath, CancellationToken cancellationToken)
		{
			// Update status label to indicate download is starting
			labelStatusValue.Text = "Downloading...";

			// Send an HTTP GET request to download the file
			using HttpResponseMessage response = await httpClient.GetAsync(requestUri: url, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: cancellationToken);
			// Ensure the response indicates success
			_ = response.EnsureSuccessStatusCode();

			// Get the total content length from the response headers
			long totalBytes = response.Content.Headers.ContentLength ?? -1L;
			// Determine if progress can be reported based on content length availability
			bool canReportProgress = totalBytes != -1;

			// Update UI elements with download information
			labelDateValue.Text = (await GetLastModifiedAsync(uri: new Uri(uriString: url), client: httpClient)).ToString();
			labelSourceValue.Text = url;
			//labelSizeValue.Text = canReportProgress ? $"{totalBytes / 1024.0 / 1024.0:F2} MB" : "Unknown";
			labelSizeValue.Text = canReportProgress ? $"{totalBytes:N0} {I10nStrings.BytesText}" : "Unknown";

			// Stream the response content to the specified file path
			using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
			// Create a file stream to write the downloaded content
			using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
			// Buffer for reading data
			byte[] buffer = new byte[8192];
			// Track total bytes read for progress reporting
			long totalRead = 0;
			// Number of bytes read in each iteration
			int bytesRead;

			// Read from the content stream and write to the file stream
			// Update progress bar if content length is known
			// Support cancellation via the provided token
			while ((bytesRead = await contentStream.ReadAsync(buffer: buffer.AsMemory(start: 0, length: buffer.Length), cancellationToken: cancellationToken)) > 0)
			{
				// Write the read bytes to the file
				await fileStream.WriteAsync(buffer: buffer.AsMemory(start: 0, length: bytesRead), cancellationToken: cancellationToken);
				// Update total bytes read
				totalRead += bytesRead;
				// Update progress bar if possible
				if (canReportProgress)
				{
					// Calculate progress percentage
					int percent = (int)(totalRead * 100L / totalBytes);
					// Update progress bar value and text
					progressBarDownload.Value = percent;
					progressBarDownload.Text = $"{percent}%";
				}
			}
		}

		#endregion

		#region Click event handlers

		/// <summary>
		/// Click handler for the Download button. Starts the download process.
		/// </summary>
		/// <param name="sender">The event source (the Download button).</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method is called when the Download button is clicked.
		/// </remarks>
		private async void ButtonDownload_Click(object sender, EventArgs e) => await StartDownloadAsync();

		/// <summary>
		/// Click handler for the Cancel button. Cancels the active download operation if one is running.
		/// </summary>
		/// <param name="sender">The event source (the Cancel button).</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method is called when the Cancel button is clicked.
		/// </remarks>
		private void ButtonCancel_Click(object sender, EventArgs e) => cancellationTokenSource?.Cancel();

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
			// Check the type of the sender and copy the text accordingly
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
			// Check if the sender is a ToolStripItem
			else if (sender is ToolStripItem)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: currentControl.Text);
			}
			// Unsupported type
			else
			{
				// Throw an exception
				throw new ArgumentException(message: "Unsupported sender type", paramName: nameof(sender));
			}
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
		private void SetStatusBar_Enter(object sender, EventArgs e)
		{
			// Set the status bar text based on the sender's accessible description
			switch (sender)
			{
				// If the sender is a control with an accessible description, set the status bar text
				// If the sender is a ToolStripItem with an accessible description, set the status bar text
				case Control { AccessibleDescription: not null } control:
					SetStatusBar(text: control.AccessibleDescription);
					break;
				case ToolStripItem { AccessibleDescription: not null } item:
					SetStatusBar(text: item.AccessibleDescription);
					break;
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
		/// This method is called when the mouse pointer leaves a control or the control loses focus.
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
			if (sender is Control control)
			{
				currentControl = control;
				if (control.Tag != null)
				{
					currentTagText = control.Tag.ToString();
				}
			}
		}

		#endregion
	}
}