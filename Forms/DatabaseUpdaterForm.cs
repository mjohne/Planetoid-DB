// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Reflection;

namespace Planetoid_DB;

/// <summary>Database Updater Form that combines database checking and downloading functionality.</summary>
/// <remarks>This form merges <c>CheckDatabaseForm</c> and <c>DatabaseDownloaderForm</c> into a single tabbed
/// interface, allowing users to verify and update database files from one place.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseUpdaterForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Shared <see cref="HttpClient"/> used for all HTTP requests (both check and download).
	/// Reuse to avoid socket exhaustion.</summary>
	private static readonly HttpClient httpClient = new(handler: new HttpClientHandler
	{
		AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
		CheckCertificateRevocationList = true
	})
	{
		Timeout = TimeSpan.FromMinutes(value: 10)
	};

	/// <summary>Temporary filename (including path) used to store the downloaded gzip file.</summary>
	private readonly string _filenameTemp = Settings.Default.systemFilenameTemp;

	/// <summary>The URL used to retrieve information about the online database file (HEAD requests).</summary>
	private readonly string checkUrl;

	/// <summary>The URL used to download the database archive (.gz file).</summary>
	private readonly string downloadUrl;

	/// <summary>The path to the local database file used for comparison.</summary>
	private readonly string localFilePath;

	/// <summary>Output path (filename without extension) where the downloaded archive will be extracted.</summary>
	private readonly string extractFilePath;

	/// <summary>Cancellation token source used to cancel an ongoing download operation.</summary>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DatabaseUpdaterForm"/> class.</summary>
	/// <param name="checkUrl">The URL of the online database file to check (used for HEAD requests).</param>
	/// <param name="downloadUrl">The URL to download the database archive from.</param>
	/// <param name="localFilePath">The local file path of the database file to compare.</param>
	/// <param name="databaseName">The display name of the database (e.g. "ASTORB.DAT" or "MPCORB.DAT").</param>
	public DatabaseUpdaterForm(string checkUrl, string downloadUrl, string localFilePath, string databaseName)
	{
		InitializeComponent();
		// Enable double buffering for the download TableLayoutPanel to prevent flickering
		try
		{
			PropertyInfo? dbProp = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			dbProp?.SetValue(obj: tableLayoutPanelDownload, value: true, index: null);
			MethodInfo? setStyleMethod = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			setStyleMethod?.Invoke(obj: tableLayoutPanelDownload, parameters: [ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true]);
		}
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Could not set DoubleBuffered on tableLayoutPanelDownload");
		}
		// Validate inputs
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: checkUrl, paramName: nameof(checkUrl));
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: downloadUrl, paramName: nameof(downloadUrl));
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: localFilePath, paramName: nameof(localFilePath));
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: databaseName, paramName: nameof(databaseName));
		this.checkUrl = checkUrl;
		this.downloadUrl = downloadUrl;
		this.localFilePath = localFilePath;
		// Derive extraction path from downloadUrl
		if (!Uri.TryCreate(uriString: downloadUrl, uriKind: UriKind.Absolute, result: out Uri? parsedUri))
		{
			throw new ArgumentException(message: "downloadUrl is not in a valid format.", paramName: nameof(downloadUrl));
		}
		string localPath = parsedUri.LocalPath;
		extractFilePath = Path.Combine(
			Path.GetDirectoryName(path: _filenameTemp) ?? string.Empty,
			Path.GetFileNameWithoutExtension(path: localPath));
		// Set the form title
		Text = $"Update {databaseName}";
		// Set accessible info
		AccessibleDescription = $"Allows checking and downloading the {databaseName} database";
		AccessibleName = $"Update {databaseName}";
		// Set label texts for the check tab
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
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns><see langword="true"/> if the dialog was confirmed; otherwise, <see langword="false"/>.</returns>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"Database-Information-local-online.{ext}";
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
	{
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(tableLayoutPanelCheck, "Information of local and online database", saveFileDialog.FileName);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Extracts a GZIP-compressed file to the specified output file.</summary>
	/// <param name="gzipFilePath">Full path to the source .gz file.</param>
	/// <param name="outputFilePath">Full path where the decompressed file will be written.</param>
	/// <param name="token">A cancellation token to cancel the operation.</param>
	protected static async Task ExtractGzipFileAsync(string gzipFilePath, string outputFilePath, CancellationToken token)
	{
		await using FileStream sourceStream = new(path: gzipFilePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 4096, options: FileOptions.Asynchronous);
		await using FileStream targetStream = new(path: outputFilePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 4096, options: FileOptions.Asynchronous);
		await using GZipStream decompressionStream = new(stream: sourceStream, mode: CompressionMode.Decompress);
		await decompressionStream.CopyToAsync(destination: targetStream, cancellationToken: token);
	}

	/// <summary>Asynchronously determines whether an active internet connection is available by sending a HEAD request.</summary>
	/// <param name="client">The <see cref="HttpClient"/> instance used to send the request.</param>
	/// <param name="url">The URL to check connectivity against.</param>
	/// <returns><see langword="true"/> if the server is reachable; otherwise, <see langword="false"/>.</returns>
	private static async Task<bool> HasInternetAsync(HttpClient client, string url)
	{
		try
		{
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: url);
			using CancellationTokenSource cts = new(delay: TimeSpan.FromSeconds(seconds: 5));
			using HttpResponseMessage response = await client.SendAsync(
				request,
				completionOption: HttpCompletionOption.ResponseHeadersRead,
				cancellationToken: cts.Token);
			return response.IsSuccessStatusCode;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>Updates the progress bar and text based on the current and total bytes processed.</summary>
	/// <param name="current">The current number of bytes processed.</param>
	/// <param name="total">The total number of bytes to process.</param>
	/// <param name="downloadStopwatch">Optional stopwatch to calculate and display download speed and remaining time.</param>
	private void UpdateProgress(long current, long total, Stopwatch? downloadStopwatch = null)
	{
		if (total <= 0)
		{
			return;
		}
		int percentage = (int)(current * 100 / total);
		if (kryptonProgressBarDownload.Value != percentage)
		{
			kryptonProgressBarDownload.Value = percentage;
			kryptonProgressBarDownload.Text = $"{percentage}%";
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)Math.Min(percentage, 100), progressMax: 100);
		}
		if (downloadStopwatch != null)
		{
			double elapsedSeconds = downloadStopwatch.Elapsed.TotalSeconds;
			double bytesPerSecond = elapsedSeconds > 0 ? current / elapsedSeconds : 0;
			labelDownloadSpeedValue.Text = bytesPerSecond > 0 ? $"{bytesPerSecond:N0} {I18nStrings.BytesText}/s" : "...";
			TimeSpan elapsed = downloadStopwatch.Elapsed;
			if (total > 0 && bytesPerSecond > 0)
			{
				TimeSpan estimated = TimeSpan.FromSeconds(value: (total - current) / bytesPerSecond);
				labelTimeValue.Text = $"{elapsed:hh\\:mm\\:ss} / {estimated:hh\\:mm\\:ss}";
			}
			else
			{
				labelTimeValue.Text = $"{elapsed:hh\\:mm\\:ss}";
			}
		}
	}

	/// <summary>Starts the download workflow: validates network and input, disables/enables UI controls,
	/// downloads the file, extracts the GZIP archive and notifies the user.</summary>
	private async Task StartDownloadAsync()
	{
		bool isServerReachable = await HasInternetAsync(client: httpClient, url: downloadUrl);
		if (!isServerReachable)
		{
			logger.Error(message: "No internet connection available.");
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		if (string.IsNullOrWhiteSpace(value: downloadUrl))
		{
			_ = MessageBox.Show(text: "Please enter a valid URL!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		if (string.IsNullOrWhiteSpace(value: _filenameTemp))
		{
			_ = MessageBox.Show(text: "Please select a save location!", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		try
		{
			toolStripButtonDownload.Enabled = false;
			toolStripButtonCancel.Enabled = true;
			kryptonProgressBarDownload.Value = 0;
			kryptonProgressBarDownload.Text = "0%";
			UpdateProgress(current: 0, total: 100);
			cancellationTokenSource = new CancellationTokenSource();
			CancellationToken token = cancellationTokenSource.Token;
			await DownloadFileAsync(fileUrl: downloadUrl, destinationPath: _filenameTemp, token: token);
			labelStatusValue.Text = "Extracting...";
			kryptonProgressBarDownload.Style = ProgressBarStyle.Marquee;
			await ExtractGzipFileAsync(gzipFilePath: _filenameTemp, outputFilePath: extractFilePath, token: token);
			labelStatusValue.Text = "Download completed";
			_ = MessageBox.Show(text: "Download completed successfully!", caption: "Finished", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			logger.Info(message: "Download and extraction completed successfully.");
			DialogResult = DialogResult.OK;
		}
		catch (OperationCanceledException)
		{
			labelStatusValue.Text = "Download canceled";
			_ = MessageBox.Show(text: "Download canceled!", caption: "Canceled", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			logger.Info(message: "Download canceled by user.");
		}
		catch (Exception ex)
		{
			labelStatusValue.Text = "Download error";
			_ = MessageBox.Show(text: $"Error during download: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			logger.Error(exception: ex, message: "Download failed. Url={Url}, TempFile={TempFile}", args: (downloadUrl, _filenameTemp));
		}
		finally
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
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}
	}

	/// <summary>Downloads a file asynchronously from the specified <paramref name="fileUrl"/> and writes it to <paramref name="destinationPath"/>.</summary>
	/// <param name="fileUrl">The URL to download from.</param>
	/// <param name="destinationPath">Local file path where the downloaded data will be saved.</param>
	/// <param name="token">Token used to cancel the download operation.</param>
	private async Task DownloadFileAsync(string fileUrl, string destinationPath, CancellationToken token)
	{
		labelStatusValue.Text = "Downloading...";
		using HttpResponseMessage response = await httpClient.GetAsync(requestUri: fileUrl, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: token);
		_ = response.EnsureSuccessStatusCode();
		long? totalBytes = response.Content.Headers.ContentLength;
		DateTime? lastMod = response.Content.Headers.LastModified?.UtcDateTime;
		labelDateValue.Text = lastMod.HasValue ? lastMod.ToString() : "-";
		labelSourceValue.Text = fileUrl;
		labelSizeValue.Text = totalBytes.HasValue ? $"{totalBytes:N0} {I18nStrings.BytesText}" : "Unknown";
		await using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken: token);
		await using FileStream fileStream = new(path: destinationPath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
		byte[] buffer = new byte[8192];
		long totalRead = 0;
		int bytesRead;
		Stopwatch stopwatch = Stopwatch.StartNew();
		Stopwatch downloadStopwatch = Stopwatch.StartNew();
		while ((bytesRead = await contentStream.ReadAsync(buffer: buffer, cancellationToken: token)) > 0)
		{
			await fileStream.WriteAsync(buffer: buffer.AsMemory(start: 0, length: bytesRead), cancellationToken: token);
			totalRead += bytesRead;
			if (totalBytes.HasValue && (stopwatch.ElapsedMilliseconds > 100 || totalRead == totalBytes))
			{
				UpdateProgress(current: totalRead, total: totalBytes.Value, downloadStopwatch: downloadStopwatch);
				stopwatch.Restart();
			}
		}
		await fileStream.FlushAsync(cancellationToken: token);
	}

	#endregion

	#region task methods

	/// <summary>Retrieves the last modified date of the specified URI.</summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The date of the last modification or <see cref="DateTime.MinValue"/> in case of an error.</returns>
	private static async Task<DateTime> GetLastModifiedAsync(Uri uri)
	{
		try
		{
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			using HttpResponseMessage response = await httpClient.SendAsync(request: request).ConfigureAwait(continueOnCapturedContext: false);
			return response.IsSuccessStatusCode ? response.Content.Headers.LastModified?.UtcDateTime ?? DateTime.MinValue : DateTime.MinValue;
		}
		catch (HttpRequestException ex)
		{
			logger.Error(message: "Error retrieving last modified date.", exception: ex);
			ShowErrorMessage(message: ex.Message);
			return DateTime.MinValue;
		}
	}

	/// <summary>Retrieves the content length of the specified URI.</summary>
	/// <param name="uri">The URI of the resource.</param>
	/// <returns>The content length or 0 in case of error.</returns>
	private static async Task<long> GetContentLengthAsync(Uri uri)
	{
		try
		{
			using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
			using HttpResponseMessage response = await httpClient.SendAsync(request: request).ConfigureAwait(continueOnCapturedContext: false);
			return response.IsSuccessStatusCode ? response.Content.Headers.ContentLength ?? 0 : 0;
		}
		catch (HttpRequestException ex)
		{
			logger.Error(message: "Error retrieving content length.", exception: ex);
			ShowErrorMessage(message: ex.Message);
			return 0;
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Event handler for loading the form. Initialises the check tab with local and online database information.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private async void DatabaseUpdaterForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		Uri uri = new(uriString: checkUrl);
		DateTime datetimeFileLocal = DateTime.MinValue;
		DateTime datetimeFileOnline = await GetLastModifiedAsync(uri: uri).ConfigureAwait(continueOnCapturedContext: false);
		if (!File.Exists(path: localFilePath))
		{
			labelContentLengthValueLocal.Text = I18nStrings.NoFileFoundText;
			labelModifiedDateValueLocal.Text = I18nStrings.NoFileFoundText;
		}
		else
		{
			FileInfo fileInfo = new(fileName: localFilePath);
			datetimeFileLocal = fileInfo.LastWriteTime;
			labelContentLengthValueLocal.Text = $"{fileInfo.Length:N0} {I18nStrings.BytesText}";
			labelModifiedDateValueLocal.Text = datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture);
		}
		labelContentLengthValueOnline.Text = $"{await GetContentLengthAsync(uri: uri).ConfigureAwait(continueOnCapturedContext: false):N0} {I18nStrings.BytesText}";
		labelModifiedDateValueOnline.Text = datetimeFileOnline.ToString(provider: CultureInfo.CurrentCulture);
		if (datetimeFileOnline > datetimeFileLocal)
		{
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_new_16px;
			labelUpdateNeeded.Text = I18nStrings.UpdateRecommendedText;
		}
		else
		{
			labelUpdateNeeded.Values.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
			labelUpdateNeeded.Text = I18nStrings.NoUpdateNeededText;
		}
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>Event handler for double-clicking the "Update Needed" label to refresh check information.</summary>
	/// <param name="sender">The event source, typically the label being double-clicked.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <exception cref="ArgumentNullException">Thrown when the sender is null.</exception>
	private void LabelUpdateNeeded_DoubleClick(object sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: sender);
		labelContentLengthValueLocal.Text = string.Empty;
		labelModifiedDateValueLocal.Text = string.Empty;
		labelContentLengthValueOnline.Text = string.Empty;
		labelModifiedDateValueOnline.Text = string.Empty;
		labelUpdateNeeded.Text = string.Empty;
		labelUpdateNeeded.Values.Image = null;
		DatabaseUpdaterForm_Load(sender: sender, e: e);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for copying the local database modified date to the clipboard.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalModifiedDate_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelModifiedDateValueLocal.Text))
		{
			string msg = "No local modified date available to copy to clipboard.";
			logger.Warn(message: msg);
			MessageBox.Show(text: msg, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelModifiedDateValueLocal.Text);
	}

	/// <summary>Handles the click event for copying the local content length value to the clipboard.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseLocalContentLength_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelContentLengthValueLocal.Text))
		{
			string msg = "No local content length available to copy to clipboard.";
			logger.Warn(message: msg);
			MessageBox.Show(text: msg, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelContentLengthValueLocal.Text);
	}

	/// <summary>Handles the click event for copying the online modified date value to the clipboard.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineModifiedDate_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelModifiedDateValueOnline.Text))
		{
			string msg = "No online modified date available to copy to clipboard.";
			logger.Warn(message: msg);
			MessageBox.Show(text: msg, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelModifiedDateValueOnline.Text);
	}

	/// <summary>Handles the click event for copying the online content length value to the clipboard.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	private void MenuitemCopyToClipboardDatabaseOnlineContentLength_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(value: labelContentLengthValueOnline.Text))
		{
			string msg = "No online content length available to copy to clipboard.";
			logger.Warn(message: msg);
			MessageBox.Show(text: msg, caption: "Warning", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		CopyToClipboard(text: labelContentLengthValueOnline.Text);
	}

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

	/// <summary>Click handler for the Download button. Starts the download process.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private async void ButtonDownload_Click(object sender, EventArgs e) => await StartDownloadAsync();

	/// <summary>Click handler for the Cancel button. Cancels the active download operation if one is running.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void ButtonCancel_Click(object sender, EventArgs e) => cancellationTokenSource?.Cancel();

	#endregion
}
