// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;
using Planetoid_DB.Resources;

using System.Diagnostics;
using System.IO.Compression;

namespace Planetoid_DB;

/// <summary>Represents a form for archiving MPCORB files.</summary>
/// <remarks>This form provides functionality for archiving MPCORB files using various compression formats and levels.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]

public partial class ArchiveMpcorbForm : BaseKryptonForm
{
	/// <summary>NLog logger for logging messages and errors.</summary>
	/// <remarks>This logger is used to log messages and errors that occur within the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>The last modified date of the online MPCORB file.</summary>
	/// <remarks>This value is retrieved from the online MPCORB file and is used to determine if the local file is up-to-date.</remarks>
	private DateTime? _onlineLastModified;

	/// <summary>Provides a reusable instance of HttpClient for making HTTP requests throughout the application's lifetime.</summary>
	/// <remarks>Reusing a single HttpClient instance helps prevent socket exhaustion and ensures efficient resource management. It is recommended to use this instance for all HTTP operations within the application rather than creating new instances for each request.</remarks>
	private static readonly HttpClient _httpClient = new();

	/// <summary>Gets or sets the cancellation token source used to signal cancellation requests.</summary>
	/// <remarks>This property allows for the management of cancellation tokens, which can be used to cancel ongoing operations. Ensure to dispose of the cancellation token source when it is no longer needed to free up resources.</remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>Gets or sets the compression level used for data processing.</summary>
	/// <remarks>Adjust the compression level to optimize for either processing speed or reduced data size, depending on application requirements.</remarks>
	private string compressionString = "Optimal";

	/// <summary>Gets or sets the compression format used for data processing.</summary>
	/// <remarks>Adjust the compression format to optimize for either processing speed or reduced data size, depending on application requirements.</remarks>
	private string format = "Zip";

	/// <summary>Gets or sets the file extension used for the compressed archive.</summary>
	/// <remarks>This property determines the file extension for the compressed archive based on the selected compression format.</remarks>
	private string extension = ".zip";

	/// <summary>Gets the array of supported compression formats.</summary>
	/// <remarks>This array includes the formats 'Zip', 'GZip', and 'Brotli', which can be used for data compression and decompression operations.</remarks>
	private readonly string[] formats = ["Zip", "GZip", "Brotli"];

	/// <summary>Gets the array of supported compression levels.</summary>
	/// <remarks>This array includes the levels 'Optimal', 'Fastest', 'NoCompression', and 'SmallestSize', which can be used for data compression operations.</remarks>
	private readonly string[] compressionLevels = ["Optimal", "Fastest", "NoCompression", "SmallestSize"];

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom debugger display string.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Formats the given number of bytes into a human-readable string with appropriate units.</summary>
	/// <param name="bytes">The number of bytes to format.</param>
	/// <returns>A formatted string representing the size in appropriate units.</returns>
	/// <remarks>This method converts the given number of bytes into a human-readable string with appropriate units, such as KB, MB, GB, etc.</remarks>
	private static string FormatBytes(long bytes)
	{
		// Define size units
		string[] sizes = ["B", "KB", "MB", "GB", "TB"];
		// Calculate the appropriate unit
		double len = bytes;
		// Initialize the order of magnitude
		int order = 0;
		// Loop to find the appropriate unit
		while (len >= 1024 && order < sizes.Length - 1)
		{
			// Move to the next unit
			order++;
			// Convert to the next unit
			len /= 1024;
		}
		// Return the formatted string with two decimal places and the appropriate unit
		return $"{len:0.00} {sizes[order]}";
	}

	/// <summary>Updates the target text box with the newly formatted file name based on selected extension.</summary>
	/// <remarks>This method constructs a new file name using the current date and time, along with the selected compression format's extension, and updates the target text box accordingly.</remarks>
	private void UpdateTargetFileName()
	{
		// Determine the timestamp for the default file name based on the online last modified date or the current time if the online date is not available
		DateTime date = _onlineLastModified ?? DateTime.UtcNow;
		string timestamp = date.ToString(format: "yyyyMMddHHmmss");
		// Determine the file extension based on the selected compression format
		extension = format switch
		{
			"GZip" => ".gz",
			"Brotli" => ".br",
			_ => ".zip"
		};
		// Get the directory of the current target file path, or use an empty string if it cannot be determined
		string directory = Path.GetDirectoryName(path: kryptonTextBoxTarget.Text) ?? string.Empty;
		// Construct the new target file path using the directory, timestamp, and selected extension, and update the target text box
		kryptonTextBoxTarget.Text = Path.Combine(path1: directory, path2: $"MPCORB-{timestamp}{extension}");
	}

	#endregion

	#region Task methods

	/// <summary>Retrieves the last modified date of the online MPCORB.DAT.gz file.</summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the last modified date if available; otherwise, null.</returns>
	/// <remarks>This method sends a HEAD request to the specified URL to retrieve the last modified date of the MPCORB.DAT.gz file. If the request is successful and the Last-Modified header is present, the method returns the date in UTC. In case of any errors, the method logs the error and returns null.</remarks>
	private static async Task<DateTime?> GetOnlineLastModifiedAsync()
	{
		// Attempt to retrieve the last modified date of the online MPCORB file
		try
		{
			// Get the URL from settings or use the default URL if not set
			string uriString = Settings.Default.systemMpcorbDatGzUrl;
			if (string.IsNullOrEmpty(value: uriString))
			{
				uriString = "http://www.minorplanetcenter.org/iau/MPCORB/MPCORB.DAT.gz";
			}
			// Create an HTTP HEAD request to the specified URL
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: new Uri(uriString: uriString));
			// Send the request and get the response
			using HttpResponseMessage response = await _httpClient.SendAsync(request: request);
			// If the response is successful and the Last-Modified header is present, return the last modified date in UTC
			if (response.IsSuccessStatusCode && response.Content.Headers.LastModified.HasValue)
			{
				return response.Content.Headers.LastModified.Value.UtcDateTime;
			}
		}
		// Catch any exceptions that occur during the retrieval of the online last modified date, log the error, and show an error message
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error fetching online last modified date");
			ShowErrorMessage(message: $"Error fetching online last modified date: {ex.Message}");
		}
		return null;
	}

	/// <summary>Copies data from the source stream to the destination stream while reporting progress.</summary>
	/// <param name="source">The source stream to read from.</param>
	/// <param name="destination">The destination stream to write to.</param>
	/// <param name="totalBytes">The total number of bytes to copy.</param>
	/// <param name="outputStream">The output stream for progress reporting.</param>
	/// <param name="compressionLevel">The compression level used.</param>
	/// <param name="stopwatch">The stopwatch to measure elapsed time.</param>
	/// <param name="token">The cancellation token to observe.</param>
	/// <returns>A task representing the asynchronous copy operation.</returns>
	/// <remarks>This method reads data from the source stream and writes it to the destination stream in chunks, updating the progress bar and status label on the UI thread. It also calculates and displays statistics such as elapsed time, estimated remaining time, and compression ratio.</remarks>
	private async Task CopyStreamWithProgressAsync(Stream source, Stream destination, long totalBytes, Stream outputStream, string compressionLevel, Stopwatch stopwatch, CancellationToken token)
	{
		// Define a buffer for reading data in chunks
		// Use a buffer size of 80 KB, which is a common size for efficient file copying
		byte[] buffer = new byte[81920];
		// Initialize the total number of bytes read
		long totalRead = 0;
		// Read from the source stream until there are no more bytes to read
		int read;

		// Initialize the last report time to zero and set the report interval to 100 milliseconds
		long lastReportTime = 0;
		// Set the report interval to 100 milliseconds to control how often progress updates are sent to the UI
		const int reportIntervalMs = 100;

		// Loop to read data from the source stream in chunks
		while ((read = await source.ReadAsync(buffer, cancellationToken: token)) > 0)
		{
			// Check for cancellation before writing to the destination stream
			token.ThrowIfCancellationRequested();
			// Write the read bytes to the destination stream
			await destination.WriteAsync(buffer: buffer.AsMemory(start: 0, length: read), cancellationToken: token);
			// Update the total number of bytes read
			totalRead += read;
			// Check if it's time to report progress based on the elapsed time and the report interval
			if (totalBytes > 0 && (stopwatch.ElapsedMilliseconds - lastReportTime > reportIntervalMs || totalRead == totalBytes))
			{
				// Calculate the progress percentage
				int progress = (int)((double)totalRead / totalBytes * 100);
				// Get the total number of bytes written to the output stream
				long totalWritten = outputStream.Length;
				// Calculate elapsed time, estimated remaining time, and compression ratio
				TimeSpan elapsed = stopwatch.Elapsed;
				// Ensure that the total seconds is at least 0.001 to avoid division by zero in subsequent calculations
				double totalSeconds = Math.Max(0.001, elapsed.TotalSeconds);
				// Calculate the current processing rate in bytes per second
				double rate = totalRead / totalSeconds;
				// Calculate the estimated remaining time in seconds based on the current processing rate and the total bytes left to read
				double remainingSeconds = rate > 0 ? (totalBytes - totalRead) / rate : 0;
				// Create a TimeSpan object for the estimated remaining time
				TimeSpan remaining = TimeSpan.FromSeconds(value: remainingSeconds);
				// Calculate the compression ratio based on the total bytes read and written, handling the case where totalRead is zero to avoid division by zero
				double ratio = totalRead > 0 ? (double)totalWritten / totalRead : 0;
				// Calculate the estimated final size of the compressed file based on the current compression ratio
				long estimatedSize = (long)(totalBytes * ratio);
				// Update the last report time to the current elapsed milliseconds
				lastReportTime = stopwatch.ElapsedMilliseconds;
				// Update the progress bar and status label on the UI thread
				if (IsHandleCreated && !IsDisposed)
				{
					// Use a try-catch block to handle potential exceptions that may occur when updating the UI elements, such as ObjectDisposedException or InvalidOperationException
					try
					{
						// Use BeginInvoke to update the UI elements on the main thread, ensuring thread safety
						BeginInvoke(method: new Action(() =>
						{
							// Ensure the progress value does not exceed 100%
							int currentProgress = Math.Min(progress, 100);
							// Update the progress bar value and text to reflect the current progress percentage
							kryptonProgressBarToolStripItemCompression.Value = currentProgress;
							kryptonProgressBarToolStripItemCompression.Text = $"{currentProgress} %";
							// Update the tooltip text of the progress bar to provide additional information to the user
							kryptonProgressBarToolStripItemCompression.ToolTipText = kryptonProgressBarToolStripItemCompression.Text;
							// Update the status label with detailed information about the archiving process, including elapsed time, estimated remaining time, compression level, bytes read, bytes written, and estimated final size
							labelInformation.Text = $"Time: {elapsed:hh\\:mm\\:ss} / {remaining:hh\\:mm\\:ss} | Level: {compressionLevel} | Read: {FormatBytes(bytes: totalRead)} | Written: {FormatBytes(bytes: totalWritten)} | Est. Size: {FormatBytes(bytes: estimatedSize)}";
							// Update the Windows taskbar progress indicator to reflect the current progress percentage, ensuring that the progress value is clamped between 0 and 100
							TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)currentProgress, progressMax: 100);
						}));
					}
					catch (ObjectDisposedException)
					{
						// The form or its handle has been disposed; ignore further UI updates.
					}
					catch (InvalidOperationException)
					{
						// The form is not in a valid state for UI updates; ignore this progress update.
					}
				}
			}
		}
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the ArchiveMpcorbForm class.</summary>
	/// <remarks>This constructor sets up the form's components and prepares it for use.</remarks>
	public ArchiveMpcorbForm() => InitializeComponent();

	#endregion

	#region Form event handlers

	/// <summary>Handles the initialization of the ArchiveMpcorbForm when it is loaded, including setting the default file path, populating format and compression options, and updating the status with the online last modified date.</summary>
	/// <remarks>This method is called automatically when the ArchiveMpcorbForm is loaded. It sets up the user interface by selecting default values and attempts to retrieve the last modified date from an online source, updating the status label accordingly. If the online date cannot be retrieved, the status label will indicate the error or fallback to the current time.</remarks>
	/// <param name="sender">The source of the event, typically the ArchiveMpcorbForm instance.</param>
	/// <param name="e">The event data associated with the form load event.</param>
	private async void ArchiveMpcorbForm_Load(object sender, EventArgs e)
	{
		// Set default file path
		string defaultPath = Settings.Default.systemFilenameMpcorbDat;
		// If the default path exists, set it in the source textbox
		if (File.Exists(path: defaultPath))
		{
			// Set the default path in the source textbox
			kryptonTextBoxSource.Text = Path.GetFullPath(path: defaultPath);
		}
		// Update status label to indicate that the online date is being checked
		labelInformation.Text = "Checking online date...";
		// Attempt to retrieve the last modified date from the online source and update the status label accordingly
		try
		{
			// Retrieve the last modified date of the online MPCORB file
			_onlineLastModified = await GetOnlineLastModifiedAsync();
			// Update the status label with the retrieved online date or indicate that it could not be retrieved
			labelInformation.Text = _onlineLastModified.HasValue
				? $"Online date: {_onlineLastModified.Value}"
				: "Could not retrieve online date. Using current time.";
		}
		// Catch any exceptions that occur during the retrieval of the online date and update the status label with the error message
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error checking online date");
			ShowErrorMessage(message: $"Error checking online date: {ex.Message}");
		}
		// Update the target file name based on the selected format and online last modified date
		UpdateTargetFileName();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event of the KryptonButtonBrowse button, allowing the user to select a source file.</summary>
	/// <param name="sender">The source of the event, typically the KryptonButtonBrowse instance.</param>
	/// <param name="e">The event data associated with the button click event.</param>
	/// <remarks>This method opens a file dialog for the user to select a source file. If the current text in the source textbox is a valid file path, it will be pre-selected in the dialog. Once the user selects a file and confirms, the selected file path is set in the source textbox.</remarks>
	private void KryptonButtonBrowseSource_Click(object sender, EventArgs e)
	{
		// Create and configure an OpenFileDialog to allow the user to select a source file
		using OpenFileDialog openFileDialog = new()
		{
			Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*"
		};
		// If the current text in the source textbox is a valid file path, set it as the initial file name in the dialog
		if (File.Exists(path: kryptonTextBoxSource.Text))
		{
			openFileDialog.FileName = kryptonTextBoxSource.Text;
		}
		// Show the file dialog and if the user selects a file and clicks OK, set the selected file path in the source textbox
		if (openFileDialog.ShowDialog(owner: this) == DialogResult.OK)
		{
			kryptonTextBoxSource.Text = openFileDialog.FileName;
		}
	}

	/// <summary>Handles the click event for the 'Browse Target' button, allowing the user to select a target file path for the archive.</summary>
	/// <param name="sender">The source of the event, typically the button that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	/// <remarks>This event handler uses a SaveFileDialog to allow the user to select a target file path for the archive. The selected path is then displayed in the corresponding text box.</remarks>
	private void KryptonButtonBrowseTarget_Click(object sender, EventArgs e)
	{
		// Create and configure a SaveFileDialog to allow the user to select a target file path for the archive
		using SaveFileDialog saveFileDialog = new()
		{
			RestoreDirectory = false,
			InitialDirectory = Path.GetDirectoryName(path: kryptonTextBoxTarget.Text),
			FileName = Path.GetFileName(path: kryptonTextBoxTarget.Text),
			Filter = $"{format} Archive (*{extension})|*{extension}"
		};
		// Show the save file dialog and if the user does not select a file and click OK, leave the existing target path unchanged
		if (saveFileDialog.ShowDialog(owner: this) == DialogResult.OK)
		{
			kryptonTextBoxTarget.Text = saveFileDialog.FileName;
		}
	}

	/// <summary>Handles the click event for the archive button, initiating the asynchronous archiving process for the selected source file. If an archiving operation is already in progress, clicking the button cancels the current operation.</summary>
	/// <remarks>This method validates the existence of the source file, prompts the user to select a target archive file, and starts the archiving process in the background. The operation supports cancellation and progress reporting. If the process is cancelled or fails, the UI is updated accordingly and any partially created archive file is deleted.</remarks>
	/// <param name="sender">The source of the event, typically the archive button control that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private async void ToolStripButtonArchive_Click(object sender, EventArgs e)
	{
		// If a cancellation token source already exists, it means an archiving operation is in progress, so we cancel it and return
		if (cancellationTokenSource != null)
		{
			cancellationTokenSource.Cancel();
			return;
		}
		// Get the source file path from the textbox and check if it exists. If it does not exist, show an error message and return
		string sourceFile = kryptonTextBoxSource.Text;
		if (!File.Exists(path: sourceFile))
		{
			ShowErrorMessage(message: "Source file not found.");
			return;
		}
		// Try to parse the selected compression level string into a CompressionLevel enum value. If parsing fails, default to CompressionLevel.Optimal
		if (!Enum.TryParse(value: compressionString, result: out CompressionLevel compressionLevel))
		{
			compressionLevel = CompressionLevel.Optimal;
		}
		// Get the target file path from the save file dialog
		string targetFile = kryptonTextBoxTarget.Text;
		// Create a new CancellationTokenSource for the archiving operation and get the associated CancellationToken
		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = cancellationTokenSource.Token;
		// Update the UI to reflect that the archiving process has started, disabling relevant controls and resetting the progress bar
		toolStripButtonArchive.Text = "Cancel";
		toolStripButtonArchive.Image = FatcowIcons16px.fatcow_cancel_16px;
		kryptonProgressBarToolStripItemCompression.Value = 0;
		kryptonProgressBarToolStripItemCompression.Text = "0 %";
		kryptonProgressBarToolStripItemCompression.ToolTipText = kryptonProgressBarToolStripItemCompression.Text;
		labelInformation.Text = "Archiving...";
		toolStripDropDownButtonFormat.Enabled = false;
		toolStripDropDownButtonCompression.Enabled = false;
		groupBoxSource.Enabled = false;
		groupBoxTarget.Enabled = false;
		// Start the archiving process in a background task to avoid blocking the UI thread
		try
		{
			// Safe capture of values before entering Task.Run
			string currentFormat = format;
			string currentCompressionStr = compressionString;
			// Use Task.Run to perform the archiving operation asynchronously, allowing for cancellation and progress reporting
			await Task.Run(function: async () =>
			{
				// Start a stopwatch to measure the elapsed time of the archiving process
				Stopwatch stopwatch = Stopwatch.StartNew();
				// Open the source file for reading and the target file for writing, ensuring proper access and sharing modes
				using FileStream sourceStream = new(path: sourceFile, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read);
				using FileStream targetStream = new(path: targetFile, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
				// Get the total number of bytes in the source stream to calculate progress
				long totalBytes = sourceStream.Length;
				// Depending on the selected format, create the appropriate compression stream and copy the data from the source stream to the destination stream while reporting progress
				switch (currentFormat)
				{
					default:
					case "Zip":
						{
							using ZipArchive archive = new(stream: targetStream, mode: ZipArchiveMode.Create);
							ZipArchiveEntry entry = archive.CreateEntry(entryName: Path.GetFileName(path: sourceFile), compressionLevel: compressionLevel);
							using Stream entryStream = entry.Open();
							await CopyStreamWithProgressAsync(source: sourceStream, destination: entryStream, totalBytes: totalBytes, outputStream: targetStream, compressionLevel: currentCompressionStr, stopwatch: stopwatch, token: cancellationToken);
							break;
						}
					case "GZip":
						{
							using GZipStream compressionStream = new(stream: targetStream, compressionLevel: compressionLevel);
							await CopyStreamWithProgressAsync(source: sourceStream, destination: compressionStream, totalBytes: totalBytes, outputStream: targetStream, compressionLevel: currentCompressionStr, stopwatch: stopwatch, token: cancellationToken);
							break;
						}
					case "Brotli":
						{
							using BrotliStream compressionStream = new(stream: targetStream, compressionLevel: compressionLevel);
							await CopyStreamWithProgressAsync(source: sourceStream, destination: compressionStream, totalBytes: totalBytes, outputStream: targetStream, compressionLevel: currentCompressionStr, stopwatch: stopwatch, token: cancellationToken);
							break;
						}
				}
			}, cancellationToken: cancellationToken);
			// If the archiving process completes successfully without cancellation, update the status label and show a success message box
			labelInformation.Text = "Archiving completed successfully.";
			KryptonMessageBox.Show(owner: this, text: "Archiving completed successfully.", caption: "Success", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
		// Catch an OperationCanceledException to handle the case where the archiving process was cancelled by the user. Update the status label and attempt to delete the partially created target file if it exists
		catch (OperationCanceledException)
		{
			labelInformation.Text = "Archiving cancelled.";
			// Give Streams time to release locks before trying to delete
			await Task.Delay(millisecondsDelay: 100);
			if (File.Exists(path: targetFile))
			{
				try
				{
					File.Delete(path: targetFile);
				}
				catch (IOException ex)
				{
					logger.Warn(exception: ex, message: "Could not delete partial archive file after cancellation.");
				}
			}
		}
		// Catch any other exceptions that occur during the archiving process, update the status label, and show an error message box with the exception details
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Archiving failed.");
			labelInformation.Text = "Archiving failed.";
			ShowErrorMessage(message: $"Archiving failed: {ex.Message}");
		}
		// In the finally block, dispose of the cancellation token source if it exists, reset the UI controls to their default state, and reset the progress bar
		finally
		{
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
			toolStripButtonArchive.Text = "Archive";
			toolStripButtonArchive.Enabled = true;
			toolStripButtonArchive.Image = FatcowIcons16px.fatcow_package_16px;
			kryptonProgressBarToolStripItemCompression.Value = 0;
			kryptonProgressBarToolStripItemCompression.Text = "0 %";
			kryptonProgressBarToolStripItemCompression.ToolTipText = kryptonProgressBarToolStripItemCompression.Text;
			toolStripDropDownButtonFormat.Enabled = true;
			toolStripDropDownButtonCompression.Enabled = true;
			groupBoxSource.Enabled = true;
			groupBoxTarget.Enabled = true;
		}
	}

	/// <summary>Handles the click event for the 'Zip' format menu item, selecting the 'Zip' format and ensuring that other format options are deselected.</summary>
	/// <remarks>Use this event handler to switch the archive format to 'Zip' when the corresponding menu item is selected. This action will automatically deselect other available format options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemFormatZip_Click(object sender, EventArgs e)
	{
		// Uncheck the other format options when the "Zip" menu item is clicked
		toolStripMenuItemFormatGzip.Checked = false;
		toolStripMenuItemFormatBrotli.Checked = false;
		// Set the format to "Zip" when the corresponding menu item is clicked, and uncheck the other format options
		format = formats[0];
		// Determine the timestamp for the default file name based on the online last modified date or the current time if the online date is not available
		UpdateTargetFileName();
	}

	/// <summary>Handles the click event for the 'GZip' format menu item, selecting the 'GZip' format and ensuring that other format options are deselected.</summary>
	/// <remarks>Use this event handler to switch the archive format to 'GZip' when the corresponding menu item is selected. This action will automatically deselect other available format options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemFormatGzip_Click(object sender, EventArgs e)
	{
		// Uncheck the other format options when the "GZip" menu item is clicked
		toolStripMenuItemFormatZip.Checked = false;
		toolStripMenuItemFormatBrotli.Checked = false;
		// Set the format to "GZip" when the corresponding menu item is clicked, and uncheck the other format options
		format = formats[1];
		// Determine the timestamp for the default file name based on the online last modified date or the current time if the online date is not available
		UpdateTargetFileName();
	}

	/// <summary>Handles the click event for the 'Brotli' format menu item, selecting the 'Brotli' format and ensuring that other format options are deselected.</summary>
	/// <remarks>Use this event handler to switch the archive format to 'Brotli' when the corresponding menu item is selected. This action will automatically deselect other available format options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemFormatBrotli_Click(object sender, EventArgs e)
	{
		// Uncheck the other format options when the "Brotli" menu item is clicked
		toolStripMenuItemFormatZip.Checked = false;
		toolStripMenuItemFormatGzip.Checked = false;
		// Set the format to "Brotli" when the corresponding menu item is clicked, and uncheck the other format options
		format = formats[2];
		// Determine the timestamp for the default file name based on the online last modified date or the current time if the online date is not available
		UpdateTargetFileName();
	}

	/// <summary>Handles the click event for the 'Optimal' compression level menu item, selecting the 'Optimal' compression level and ensuring that other compression level options are deselected.</summary>
	/// <remarks>Use this event handler to switch the compression level to 'Optimal' when the corresponding menu item is selected. This action will automatically deselect other available compression level options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemCompressionOptimal_Click(object sender, EventArgs e)
	{
		// Uncheck the other compression level options when the "Optimal" menu item is clicked
		toolStripMenuItemCompressionFastest.Checked = false;
		toolStripMenuItemCompressionNo.Checked = false;
		toolStripMenuItemCompressionSmallestSize.Checked = false;
		// Set the compression level to "Optimal" when the corresponding menu item is clicked, and uncheck the other compression level options
		compressionString = compressionLevels[0];
	}

	/// <summary>Handles the click event for the 'Fastest' compression level menu item, selecting the 'Fastest' compression level and ensuring that other compression level options are deselected.</summary>
	/// <remarks>Use this event handler to switch the compression level to 'Fastest' when the corresponding menu item is selected. This action will automatically deselect other available compression level options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemCompressionFastest_Click(object sender, EventArgs e)
	{
		// Uncheck the other compression level options when the "Fastest" menu item is clicked
		toolStripMenuItemCompressionOptimal.Checked = false;
		toolStripMenuItemCompressionNo.Checked = false;
		toolStripMenuItemCompressionSmallestSize.Checked = false;
		// Set the compression level to "Fastest" when the corresponding menu item is clicked, and uncheck the other compression level options
		compressionString = compressionLevels[1];
	}

	/// <summary>Handles the click event for the 'No' compression level menu item, selecting the 'No' compression level and ensuring that other compression level options are deselected.</summary>
	/// <remarks>Use this event handler to switch the compression level to 'No' when the corresponding menu item is selected. This action will automatically deselect other available compression level options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemCompressionNo_Click(object sender, EventArgs e)
	{
		// Uncheck the other compression level options when the "No" menu item is clicked
		toolStripMenuItemCompressionOptimal.Checked = false;
		toolStripMenuItemCompressionFastest.Checked = false;
		toolStripMenuItemCompressionSmallestSize.Checked = false;
		// Set the compression level to "No" when the corresponding menu item is clicked, and uncheck the other compression level options
		compressionString = compressionLevels[2];
	}

	/// <summary>Handles the click event for the 'SmallestSize' compression level menu item, selecting the 'SmallestSize' compression level and ensuring that other compression level options are deselected.</summary>
	/// <remarks>Use this event handler to switch the compression level to 'SmallestSize' when the corresponding menu item is selected. This action will automatically deselect other available compression level options to maintain a single active selection.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing data related to the click event.</param>
	private void ToolStripMenuItemCompressionSmallestSize_Click(object sender, EventArgs e)
	{
		// Uncheck the other compression level options when the "SmallestSize" menu item is clicked
		toolStripMenuItemCompressionOptimal.Checked = false;
		toolStripMenuItemCompressionFastest.Checked = false;
		toolStripMenuItemCompressionNo.Checked = false;
		// Set the compression level to "SmallestSize" when the corresponding menu item is clicked, and uncheck the other compression level options
		compressionString = compressionLevels[3];
	}

	#endregion
}
