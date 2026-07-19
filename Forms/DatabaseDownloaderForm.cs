// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace Planetoid_DB;

/// <summary>Form to handle downloading updates for the application.</summary>
/// <remarks>This form provides a user interface for downloading and installing updates.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial class DatabaseDownloaderForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Shared <see cref="HttpClient"/> used for HTTP requests. Initialized in the constructor. Reuse to avoid socket exhaustion.</summary>
	/// <remarks>This HttpClient instance is reused for all HTTP requests to improve performance.</remarks>
	private static readonly HttpClient httpClient = new(handler: new HttpClientHandler
	{
		AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
		CheckCertificateRevocationList = true
	})
	{
		Timeout = TimeSpan.FromMinutes(value: 10)
	};

	/// <summary>Temporary filename (including path) used to store the downloaded gzip file.</summary>
	/// <remarks>This temporary file is used to store the downloaded gzip file before extraction.</remarks>
	private readonly string _filenameTemp = Settings.Default.systemFilenameTemp;

	/// <summary>Source URL to download the file from. Provided by the caller when the form is created.</summary>
	/// <remarks>This URL is used to initiate the download process.</remarks>
	private readonly string url;

	/// <summary>Output path (filename without extension) where the downloaded archive will be extracted.</summary>
	/// <remarks>This path is derived from the URL and is used to extract the contents of the downloaded archive.</remarks>
	private readonly string extractFilePath;

	/// <summary>Cancellation token source used to cancel an ongoing download operation. May be null when no download is active.</summary>
	/// <remarks>This token is used to cancel the download operation if needed.</remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DatabaseDownloaderForm"/> class. Prepares HTTP client and UI, stores the download <paramref name="url"/>, derives the extraction path and starts the download workflow.</summary>
	/// <param name="url">The URL to download the file from. The filename part is used to derive the extraction path.</param>
	/// <remarks>This constructor performs UI initialization and immediately begins the download by calling <see cref="StartDownloadAsync"/>. It must be called from the UI thread.</remarks>
	public DatabaseDownloaderForm(string url)
	{
		InitializeComponent();
		// Enable double buffering for the TableLayoutPanel to prevent flickering
		DoubleBufferingHelper.EnableDoubleBuffering(control: tableLayoutPanel);
		// Validate and store the URL
		if (string.IsNullOrWhiteSpace(value: url))
		{
			// Throw an exception if the URL is null or empty
			throw new ArgumentException(message: "URL cannot be null or empty.", paramName: nameof(url));
		}
		// Derive the extraction file path from the URL and temporary filename
		if (!Uri.TryCreate(uriString: url, uriKind: UriKind.Absolute, result: out Uri? parsedUri))
		{
			throw new ArgumentException(message: "URL is not in a valid format.", paramName: nameof(url));
		}
		this.url = url;
		string localPath = parsedUri.LocalPath;
		extractFilePath = Path.Combine(
			Path.GetDirectoryName(path: _filenameTemp) ?? string.Empty,
			Path.GetFileNameWithoutExtension(path: localPath));
		// Start download when form is shown
		//Shown += async (_, _) => await StartDownloadAsync();
	}

	/// <summary>Overrides the OnShown method to start the download workflow when the form is displayed.</summary>
	/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
	/// <remarks>This method is called when the form is first shown. It initiates the download process by calling <see cref="StartDownloadAsync"/>.</remarks>
	protected override async void OnShown(EventArgs e)
	{
		// Call the base class implementation of OnShown
		base.OnShown(e: e);
		// Start the download workflow asynchronously
		await StartDownloadAsync();
	}

	/// <summary>Overrides the OnFormClosing method to cancel any ongoing download operation when the form is closing.</summary>
	/// <param name="e">A <see cref="FormClosingEventArgs"/> that contains the event data.</param>
	/// <remarks>This method is called when the form is closing. It cancels any active download operation by calling <see cref="CancellationTokenSource.Cancel()"/> on the <see cref="cancellationTokenSource"/>.</remarks>
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		// Cancel any ongoing download operation if the cancellation token source is not null
		cancellationTokenSource?.Cancel();
		// Call the base class implementation of OnFormClosing
		base.OnFormClosing(e: e);
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Extracts the filename from the given URL and returns the full destination path within the temporary file directory.</summary>
	/// <param name="url">The absolute URL from which to extract the filename.</param>
	/// <returns>The full destination path combining the temporary file directory and the filename from the URL, or <see langword="null"/> if the URL is invalid or contains no filename.</returns>
	/// <remarks>Uses <see cref="Uri.LocalPath"/> to extract the filename. The result is combined with the directory of <see cref="_filenameTemp"/>.</remarks>
	private string? GetFileFromUrl(string url)
	{
		// Validate the URL and extract the filename
		if (!Uri.TryCreate(uriString: url, uriKind: UriKind.Absolute, result: out Uri? parsedUri))
		{
			return null;
		}
		// Extract the filename from the URL's local path
		string fileName = Path.GetFileName(path: parsedUri.LocalPath);
		// If the filename is null or whitespace, return null
		if (string.IsNullOrWhiteSpace(value: fileName))
		{
			return null;
		}
		// Combine the filename with the directory of the temporary file and return the full path
		return Path.Combine(path1: Path.GetDirectoryName(path: _filenameTemp) ?? string.Empty, path2: fileName);
	}

	/// <summary>Extracts a GZIP-compressed file to the specified output file.</summary>
	/// <param name="gzipFilePath">Full path to the source .gz file.</param>
	/// <param name="outputFilePath">Full path where the decompressed file will be written.</param>
	/// <param name="token">A cancellation token to cancel the operation.</param>
	/// <remarks>The method streams the compressed input to the output file using <see cref="GZipStream"/>. It throws exceptions (e.g. <see cref="IOException"/>, <see cref="InvalidDataException"/>) to the caller.</remarks>
	protected static async Task ExtractGzipFileAsync(string gzipFilePath, string outputFilePath, CancellationToken token)
	{
		// Open the gzip file and create a new file stream for the output file
		await using FileStream sourceStream = new(path: gzipFilePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 4096, options: FileOptions.Asynchronous);
		// Create a new file stream for the output file
		await using FileStream targetStream = new(path: outputFilePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 4096, options: FileOptions.Asynchronous);
		// Create a new GZipStream for decompression
		await using GZipStream decompressionStream = new(stream: sourceStream, mode: CompressionMode.Decompress);
		// Copy the decompressed data to the output file stream
		await decompressionStream.CopyToAsync(destination: targetStream, cancellationToken: token);
	}

	/// <summary>Asynchronously determines whether an active internet connection is available by sending a HEAD request to the specified URL.</summary>
	/// <remarks>This method uses a HEAD request to minimize bandwidth usage and applies a timeout of 5 seconds. If an exception occurs during the request, the method returns <see langword="false"/>.</remarks>
	/// <param name="client">The HttpClient instance used to send the request. This must be initialized before calling the method and should not be null.</param>
	/// <param name="url">The URL to which the connectivity check is sent. This must be a valid, non-null URI string.</param>
	/// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the response indicates success; otherwise, <see langword="false"/>.</returns>
	private static async Task<bool> HasInternetAsync(HttpClient client, string url)
	{
		// Use a 5-second timeout for the connectivity check
		try
		{
			// Use a HEAD request instead of GET to save bandwidth
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: url);
			// Timeout 5 seconds
			using CancellationTokenSource cts = new(delay: TimeSpan.FromSeconds(seconds: 5));
			// Send the request and get the response
			using HttpResponseMessage response = await client.SendAsync(
				request: request,
				completionOption: HttpCompletionOption.ResponseHeadersRead,
				cancellationToken: cts.Token);
			// Return true if the response status code indicates success (2xx); otherwise, return false
			return response.IsSuccessStatusCode;
		}
		// If any exception occurs (e.g., HttpRequestException, TaskCanceledException), return false
		catch
		{
			return false;
		}
	}

	/// <summary>Updates the progress bar and related labels based on the provided <see cref="DownloadProgressInfo"/>. This method is called from the download workflow to reflect the current download status in the UI.</summary>
	/// <param name="info">The download progress information.</param>
	/// <remarks>This method updates the progress bar, download speed label, and time label based on the current download progress.</remarks>
	private void UpdateProgress(DownloadProgressInfo info)
	{
		// If the total bytes is zero or negative, or if the form is disposed, do not update the UI
		if (info.TotalBytes <= 0 || IsDisposed)
		{
			return;
		}
		// Calculate the percentage of the download completed
		int percentage = (int)(info.CurrentBytes * 100 / info.TotalBytes);
		percentage = Math.Clamp(value: percentage, min: 0, max: 100);
		// Update the progress bar only if the percentage has changed
		if (kryptonProgressBarDownload.Value != percentage)
		{
			kryptonProgressBarDownload.Value = percentage;
			kryptonProgressBarDownload.Text = $"{percentage}%";
			// Update the Windows taskbar progress indicator if the form handle is created
			if (IsHandleCreated)
			{
				TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)Math.Min(percentage, 100), progressMax: 100);
			}
		}
		// Update the download speed label with formatted bytes per second or "..." if not available
		labelDownloadSpeedValue.Text = info.BytesPerSecond > 0 ? $"{info.BytesPerSecond:N0} {I18nStrings.BytesText}/s" : "...";
		// Update the time label with elapsed and estimated time in hh:mm:ss format
		labelTimeValue.Text = info.Estimated > TimeSpan.Zero
			? $"{info.Elapsed:hh\\:mm\\:ss} / {info.Estimated:hh\\:mm\\:ss}"
			: $"{info.Elapsed:hh\\:mm\\:ss}";
	}

	/// <summary>Starts the download workflow: validates network and input, disables/enables UI controls, downloads the file, extracts the GZIP archive and notifies the user.</summary>
	/// <remarks>This method is an async void entry point intended for UI usage. It uses an internal <see cref="CancellationTokenSource"/> to support cancellation, updates form controls (buttons, labels, progress bar) and handles exceptions internally (shows message boxes). No exceptions are propagated to the caller.</remarks>
	private async Task StartDownloadAsync()
	{
		// Check for internet connectivity before starting the download
		bool isServerReachable = await HasInternetAsync(client: httpClient, url: url);
		// If the server is not reachable, log an error, show a message box and return early
		if (!isServerReachable)
		{
			logger.Error(message: "No internet connection available.");
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		// Validate that the temporary filename is set; if not, show an error message and return early
		if (string.IsNullOrWhiteSpace(value: _filenameTemp))
		{
			ShowErrorMessage(message: "Please select a save location!");
			return;
		}
		// Log the start of the download operation with the URL and temporary filename
		logger.Info(message: "Starting download. Url={Url}, TempFile={TempFile}", argument1: url, argument2: _filenameTemp);
		// Disable the Download button and enable the Cancel button, reset progress bar and labels, and create a new CancellationTokenSource
		try
		{
			// Disable the Download button and enable the Cancel button
			toolStripButtonDownload.Enabled = false;
			toolStripButtonCancel.Enabled = true;
			// Reset progress bar and labels
			kryptonProgressBarDownload.Value = 0;
			kryptonProgressBarDownload.Text = "0%";
			// Create a new CancellationTokenSource for this download operation
			cancellationTokenSource = new CancellationTokenSource();
			CancellationToken token = cancellationTokenSource.Token;
			// Create a progress reporter that calls UpdateProgress on the UI thread
			Progress<DownloadProgressInfo> progress = new(handler: UpdateProgress);
			// Start the download operation asynchronously
			await DownloadFileAsync(fileUrl: url, destinationPath: _filenameTemp, progress: progress, token: token);
			// After download, check if the file is a GZIP archive and extract it; otherwise, move the file to the destination path
			if (Path.GetExtension(path: url).Equals(value: ".gz", comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the downloaded file is a GZIP archive, update the status label and progress bar style, then extract it
				labelStatusValue.Text = "Extracting...";
				kryptonProgressBarDownload.Style = ProgressBarStyle.Marquee;
				// Extract the GZIP file to the specified output path
				await ExtractGzipFileAsync(gzipFilePath: _filenameTemp, outputFilePath: extractFilePath, token: token);
			}
			// If the file is not a GZIP archive, move it to the destination path derived from the URL
			else
			{
				// Determine the destination file path based on the URL or use the extractFilePath as a fallback
				string destFilePath = GetFileFromUrl(url: url) ?? extractFilePath;
				// Move the downloaded file to the destination path only if it's different from the temporary filename (case-insensitive comparison)
				if (!string.Equals(a: _filenameTemp, b: destFilePath, comparisonType: StringComparison.OrdinalIgnoreCase))
				{
					File.Move(sourceFileName: _filenameTemp, destFileName: destFilePath, overwrite: true);
				}
			}
			// Update the status label to indicate that the download is completed, show a message box to the user, and log the successful completion
			labelStatusValue.Text = "Download completed";
			KryptonMessageBox.Show(owner: this, text: "Download completed successfully!", caption: "Finished", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: "Download and extraction completed successfully.");
			// Set the dialog result to OK to indicate success
			DialogResult = DialogResult.OK;
		}
		// Handle cancellation by the user: update the status label, show a message box, and log the cancellation
		catch (OperationCanceledException)
		{
			labelStatusValue.Text = "Download canceled";
			KryptonMessageBox.Show(owner: this, text: "Download canceled!", caption: "Canceled", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: "Download canceled by user.");
		}
		// Handle any other exceptions that occur during the download or extraction process: update the status label, show a message box with the error, and log the exception details
		catch (Exception ex)
		{
			labelStatusValue.Text = "Download error";
			ShowErrorMessage(message: $"Error during download: {ex.Message}");
			logger.Error(exception: ex, message: "Download failed. Url={Url}, TempFile={TempFile}", url, _filenameTemp);
		}
		// In the finally block, clean up resources and reset the UI controls regardless of success, cancellation, or error
		finally
		{
			// Attempt to delete the temporary file if it exists, logging a warning if deletion fails
			if (File.Exists(path: _filenameTemp))
			{
				try { File.Delete(path: _filenameTemp); }
				catch (Exception ex) { logger.Warn(exception: ex, message: "Failed to delete temporary file."); }
			}
			// Reset the UI controls to their initial state if the form is not disposed
			if (!IsDisposed)
			{
				toolStripButtonDownload.Enabled = true;
				toolStripButtonCancel.Enabled = false;
				labelDateValue.Text = "-";
				labelSizeValue.Text = "-";
				labelSourceValue.Text = "-";
				labelTimeValue.Text = "-";
				labelDownloadSpeedValue.Text = "-";
				kryptonProgressBarDownload.Value = 0;
				kryptonProgressBarDownload.Text = "0%";
				kryptonProgressBarDownload.Style = ProgressBarStyle.Blocks;
			}
			// Dispose of the cancellation token source and set it to null to free resources
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}
	}

	/// <summary>Asynchronously downloads a file from the specified URL to the given destination path, reporting progress and supporting cancellation.</summary>
	/// <param name="fileUrl">The URL of the file to download.</param>
	/// <param name="destinationPath">The local path where the downloaded file will be saved.</param>
	/// <param name="progress">An object to report download progress.</param>
	/// <param name="token">A cancellation token to cancel the download operation.</param>
	/// <returns>A task representing the asynchronous download operation.</returns>
	/// <remarks>This method updates the UI with the download progress and handles cancellation.</remarks>
	private async Task DownloadFileAsync(string fileUrl, string destinationPath, IProgress<DownloadProgressInfo> progress, CancellationToken token)
	{
		// Update the status label to indicate that the download is in progress
		labelStatusValue.Text = "Downloading...";
		// Send an HTTP GET request to the specified file URL, requesting only the headers initially to get content length and last modified date
		using HttpResponseMessage response = await httpClient.GetAsync(requestUri: fileUrl, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: token);
		// Ensure that the HTTP response indicates success; if not, throw an exception
		response.EnsureSuccessStatusCode();
		// Get the total bytes from the Content-Length header, if available
		long? totalBytes = response.Content.Headers.ContentLength;
		// Get the last modified date from the Last-Modified header, if available
		DateTime? lastMod = response.Content.Headers.LastModified?.UtcDateTime;
		// Update the UI labels with the last modified date, source URL, and total size
		labelDateValue.Text = lastMod.HasValue ? lastMod.ToString() : "-";
		labelSourceValue.Text = fileUrl;
		labelSizeValue.Text = totalBytes.HasValue ? $"{totalBytes:N0} {I18nStrings.BytesText}" : "Unknown";
		// Create a stream to read the content of the response and a file stream to write to the destination path
		await using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken: token);
		await using FileStream fileStream = new(path: destinationPath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
		// Create a buffer to hold chunks of data read from the content stream
		byte[] buffer = new byte[8192];
		long totalRead = 0;
		int bytesRead;
		// Create two stopwatches: one for updating the progress and one for measuring download speed
		Stopwatch updateStopwatch = Stopwatch.StartNew();
		Stopwatch downloadStopwatch = Stopwatch.StartNew();
		// Read from the content stream in a loop until there are no more bytes to read
		while ((bytesRead = await contentStream.ReadAsync(buffer: buffer, cancellationToken: token)) > 0)
		{
			// Write the read bytes to the file stream asynchronously
			await fileStream.WriteAsync(buffer: buffer.AsMemory(start: 0, length: bytesRead), cancellationToken: token);
			// Update the total number of bytes read so far
			totalRead += bytesRead;
			// Report progress if the total bytes is known and either 100 milliseconds have passed or the download is complete
			if (totalBytes.HasValue && (updateStopwatch.ElapsedMilliseconds > 100 || totalRead == totalBytes))
			{
				// Calculate the elapsed time in seconds and the download speed in bytes per second
				double elapsedSeconds = downloadStopwatch.Elapsed.TotalSeconds;
				double bytesPerSecond = elapsedSeconds > 0 ? totalRead / elapsedSeconds : 0;
				// Estimate the remaining time based on the download speed and total bytes
				TimeSpan estimated = bytesPerSecond > 0
					? TimeSpan.FromSeconds(value: (totalBytes.Value - totalRead) / bytesPerSecond)
					: TimeSpan.Zero;
				// Report the current download progress to the UI
				progress.Report(value: new DownloadProgressInfo(
					CurrentBytes: totalRead,
					TotalBytes: totalBytes.Value,
					BytesPerSecond: bytesPerSecond,
					Elapsed: downloadStopwatch.Elapsed,
					Estimated: estimated));
				// Restart the update stopwatch to measure the next interval for progress reporting
				updateStopwatch.Restart();
			}
		}
		// Flush the file stream to ensure all data is written to disk
		await fileStream.FlushAsync(cancellationToken: token);
	}

	#endregion

	#region Click event handlers

	/// <summary>Click handler for the Download button. Starts the download process.</summary>
	/// <param name="sender">The event source (the Download button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Download button is clicked.</remarks>
	private async void ButtonDownload_Click(object sender, EventArgs e) => await StartDownloadAsync();

	/// <summary>Click handler for the Cancel button. Cancels the active download operation if one is running.</summary>
	/// <param name="sender">The event source (the Cancel button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Cancel button is clicked.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e) => cancellationTokenSource?.Cancel();

	#endregion
}