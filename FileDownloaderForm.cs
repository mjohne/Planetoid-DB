using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace Planetoid_DB
{
	/// <summary>
	/// Form to handle downloading updates for the application.
	/// </summary>
	[DebuggerDisplay(value: $"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public partial class FileDownloaderForm : KryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// HttpClient instance for downloading files
		private readonly HttpClient httpClient;

		// Filename for the temp file
		private readonly string strFilenameTemp = Settings.Default.systemFilenameTemp;

		// URL to download from
		private readonly string url;

		// Path to extract the downloaded file
		private readonly string extractFilePath;

		private CancellationTokenSource? cancellationTokenSource;

		#region helper methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		private static void ShowErrorMessage(string message) =>
			// Show an error message box with the specified message
			_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

		/// <summary>
		/// Extracts a GZIP-compressed file to a specified output file.
		/// </summary>
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

		private static DateTime GetLastModified(Uri uri, HttpClient httpClient)
		{
			try
			{
				HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
				HttpResponseMessage response = httpClient.Send(request: request);
				return response.IsSuccessStatusCode ? response.Content.Headers.LastModified?.UtcDateTime ?? DateTime.MinValue : DateTime.MinValue;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "Error retrieving last modified date.");
				ShowErrorMessage(message: $"Error retrieving last modified date: {ex.Message}");
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Copies the specified text to the clipboard and displays a confirmation message.
		/// </summary>
		/// <param name="text">The text to be copied.</param>
		private static void CopyToClipboard(string text)
		{
			try
			{
				// Copy the text to the clipboard
				Clipboard.SetText(text: text);
				_ = MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				// Log the exception and show an error message
				Logger.Error(exception: ex, message: ex.Message);
				// Show an error message
				ShowErrorMessage(message: $"File not found: {ex.Message}");
			}
		}

		/// <summary>
		/// Sets the status bar text.
		/// </summary>
		/// <param name="text">The main text to be displayed on the status bar.</param>
		/// <param name="additionalInfo">Additional information to be displayed alongside the main text.</param>
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
		/// Clears the status bar text.
		/// </summary>
		private void ClearStatusBar()
		{
			// Clear the status bar text and disable it
			labelInformation.Enabled = false;
			labelInformation.Text = string.Empty;
		}

		/// <summary>
		/// Shows the MPCORB data check form.
		/// </summary>
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
				Logger.Error(message: "No internet connection available.");
				// Show an error message if there is no internet connection
				ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
			}
		}

		// Update the status bar with the current progress
		//TaskbarProgress.SetValue(windowHandle: Handle, progressValue: e.ProgressPercentage, ProgressBar: 100);

		// Reset the taskbar progress
		//TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="FileDownloaderForm"/> class.
		/// </summary>
		/// <param name="url">The URL to download the file from.</param>
		public FileDownloaderForm(string url)
		{
			InitializeComponent();
			httpClient = new HttpClient();
			this.url = url;
			extractFilePath = Path.GetFileNameWithoutExtension(path: url);
			KeyDown += FileDownloaderForm_KeyDown;
			StartDownload();
		}

		#endregion

		/// <summary>
		/// Starts the download process.
		/// </summary>
		private async void StartDownload()
		{

			if (string.IsNullOrWhiteSpace(value: url))
			{
				_ = MessageBox.Show(text: "Please enter a valid URL!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
				return;
			}

			if (string.IsNullOrWhiteSpace(value: strFilenameTemp))
			{
				_ = MessageBox.Show(text: "Please select a save location!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
				return;
			}

			try
			{
				buttonDownload.Enabled = false;
				buttonCancel.Enabled = true;
				progressBarDownload.Value = 0;
				progressBarDownload.Text = "0%";

				cancellationTokenSource = new CancellationTokenSource();

				await DownloadFileAsync(url: url, filePath: strFilenameTemp, cancellationToken: cancellationTokenSource.Token);

				// Extract the downloaded GZIP file
				labelStatusValue.Text = "Extracting...";
				ExtractGzipFile(gzipFilePath: strFilenameTemp, outputFilePath: extractFilePath);

				labelStatusValue.Text = "Download completed";
				_ = MessageBox.Show(text: "Download completed successfully!", caption: "Finished", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
				Close();
			}
			catch (OperationCanceledException)
			{
				labelStatusValue.Text = "Download canceled";
				_ = MessageBox.Show(text: "Download canceled!", caption: "Canceled", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				labelStatusValue.Text = "Download error";
				_ = MessageBox.Show(text: $"Error during download: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
			finally
			{
				buttonDownload.Enabled = true;
				buttonCancel.Enabled = false;
				labelDateValue.Text = "-";
				labelSizeValue.Text = "-";
				labelSourceValue.Text = "-";
				progressBarDownload.Value = 0;
				progressBarDownload.Text = "0%";
				cancellationTokenSource?.Dispose();
				cancellationTokenSource = null;
			}
		}

		private async void ButtonDownload_Click(object sender, EventArgs e)
		{
			StartDownload();
		}

		private async Task DownloadFileAsync(string url, string filePath, CancellationToken cancellationToken)
		{
			labelStatusValue.Text = "Downloading...";

			using HttpResponseMessage response = await httpClient.GetAsync(requestUri: url, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: cancellationToken);
			_ = response.EnsureSuccessStatusCode();

			long totalBytes = response.Content.Headers.ContentLength ?? -1L;
			bool canReportProgress = totalBytes != -1;

			//labelDateValue.Text = GetLastModified(uri: new Uri(uriString: url), httpClient: httpClient).ToString(format: "yyyy-MM-dd HH:mm:ss");
			labelDateValue.Text = GetLastModified(uri: new Uri(uriString: url), httpClient: httpClient).ToString(provider: CultureInfo.CurrentCulture);
			//labelSizeValue.Text = canReportProgress ? $"{totalBytes / 1024.0 / 1024.0:F2} MB" : "Unknown";
			labelSourceValue.Text = url;
			labelSizeValue.Text = canReportProgress ? $"{totalBytes:N0} {I10nStrings.BytesText}" : "Unknown";

			using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
			using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
			byte[] buffer = new byte[8192];
			long totalRead = 0;
			int bytesRead;

			while ((bytesRead = await contentStream.ReadAsync(buffer: buffer.AsMemory(start: 0, length: buffer.Length), cancellationToken: cancellationToken)) > 0)
			{
				await fileStream.WriteAsync(buffer: buffer.AsMemory(start: 0, length: bytesRead), cancellationToken: cancellationToken);
				totalRead += bytesRead;

				if (canReportProgress)
				{
					int progress = (int)(totalRead * 100L / totalBytes);
					progressBarDownload.Value = progress;
					progressBarDownload.Text = $"{progress}%";
				}
			}
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			cancellationTokenSource?.Cancel();
		}

		#region DoubleClick event handler

		/// <summary>
		/// Called when a control is double-clicked to copy the text to the clipboard.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
		}

		#endregion

		#region KeyDown event handler

		/// <summary>
		/// Handles the KeyDown event of the FileDownloaderForm.
		/// Closes the form when the Escape key is pressed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void FileDownloaderForm_KeyDown(object? sender, KeyEventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			// Check if the Escape key is pressed
			if (e.KeyCode == Keys.Escape)
			{
				// Close the form
				Close();
			}
		}

		#endregion
	}
}
