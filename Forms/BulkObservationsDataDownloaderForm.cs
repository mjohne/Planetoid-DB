// This file contains the implementation of BulkObservationsDataDownloaderForm,
// which provides bulk downloading of MPC observations data files for a configurable
// range of minor planets with start, pause, resume and cancel support.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Resources;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Planetoid_DB;

/// <summary>Form for bulk-downloading MPC observations data files for a range of minor planets.</summary>
/// <remarks>The form iterates over all planetoid database records from the configured minimum to the maximum index, fetches the observations HTML page for each, extracts the download link, and saves the data file to <c>%USERPROFILE%\Planetoid-DB\Observations\Data</c>. The download can be started, paused, resumed and cancelled at any time using the toolbar buttons.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class BulkObservationsDataDownloaderForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Base URL used to query the Minor Planet Center database for a specific object.</summary>
	/// <remarks>This URL is used to construct the full query URL by appending the minor planet identifier.</remarks>
	private const string MpcBaseUrl = "https://www.minorplanetcenter.net/db_search/show_object?object_id=";

	/// <summary>Base URL of the Minor Planet Center website, used to resolve relative download links.</summary>
	/// <remarks>This URL is used to construct absolute URLs for resources that are specified with relative paths on the MPC website.</remarks>
	private const string MpcRootUrl = "https://www.minorplanetcenter.net";

	/// <summary>Sub-directory under the user profile folder where downloaded observation files are saved.</summary>
	/// <remarks>Files are saved as <c>%USERPROFILE%\Planetoid-DB\Observations\Data\&lt;filename&gt;</c>.</remarks>
	private const string ObservationsDataSubPath = "Planetoid-DB/Observations/Data";

	/// <summary><see cref="HttpClient"/> instance for HTTP requests.</summary>
	/// <remarks>This <see cref="HttpClient"/> instance is initialized in the constructor and disposed when the form closes.</remarks>
	private readonly HttpClient _httpClient;

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Cancellation token source for the running download task.</summary>
	/// <remarks>Set to <c>null</c> when no download is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>The currently running download task.</summary>
	/// <remarks>Used to ensure proper cleanup by waiting for the task to complete before disposing resources.</remarks>
	private Task? _downloadTask;

	/// <summary>Indicates whether the form is being disposed.</summary>
	/// <remarks>Checked before HttpClient operations to prevent ObjectDisposedException.</remarks>
	private volatile bool _isDisposing;

	/// <summary>Indicates whether the download is currently paused.</summary>
	/// <remarks>Checked before processing each planetoid. Set to <c>true</c> by the Start/Pause button when the download is active, and back to <c>false</c> on resume.</remarks>
	private volatile bool _isPaused;

	/// <summary>Used to signal that a paused download should resume.</summary>
	/// <remarks>Created when the download is paused and completed (set) when the user presses the Start/Resume button.</remarks>
	private TaskCompletionSource<bool>? _resumeTcs;

	/// <summary>Stopwatch used to measure total elapsed time since the download started.</summary>
	/// <remarks>Started when the download begins and stopped when it finishes or is cancelled.</remarks>
	private readonly Stopwatch _elapsedStopwatch = new();

	/// <summary>Timer that fires every second to refresh the elapsed/estimated time display.</summary>
	/// <remarks>Started and stopped together with the download operation.</remarks>
	private readonly System.Windows.Forms.Timer _uiTimer;

	/// <summary>Total number of bytes downloaded across all files in the current run.</summary>
	/// <remarks>Reset to zero at the start of each new download session.</remarks>
	private long _totalBytesDownloaded;

	/// <summary>Byte size of the file that is currently being downloaded.</summary>
	/// <remarks>Updated whenever the HTTP response <c>Content-Length</c> header is known.</remarks>
	private long _currentFileSize;

	/// <summary>Number of download errors that occurred during the current session.</summary>
	/// <remarks>Incremented whenever fetching or saving a file fails. The failed file is skipped and downloading continues with the next planetoid.</remarks>
	private int _errorCount;

	/// <summary>In-memory list of detailed download errors for the current session.</summary>
	/// <remarks>Cleared automatically whenever a new bulk download is started.</remarks>
	private readonly List<BulkObservationsDownloadErrorEntry> _downloadErrors = [];

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the compiled regular expression for matching the download link in the MPC observations section.</summary>
	/// <returns>A <see cref="Regex"/> instance for matching download links.</returns>
	[GeneratedRegex(pattern: @"<a\s+href=""([^""]+)""\s+target=""_blank"">download</a>", options: RegexOptions.IgnoreCase)]
	private static partial Regex DownloadLinkRegex();

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="BulkObservationsDataDownloaderForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string. The form displays minimum/maximum spinners pre-populated with 1 and the count of records.</remarks>
	public BulkObservationsDataDownloaderForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		_planetoids = planetoids;
		// Initialize HttpClient with reasonable timeout
		_httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(value: 60)
		};
		// Configure UI timer (fires every second to update elapsed/estimated time labels)
		_uiTimer = new System.Windows.Forms.Timer { Interval = 1000 };
		_uiTimer.Tick += UiTimer_Tick;
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Prepares and displays a save file dialog with a default file name and initial directory set to the user's Documents folder.</summary>
	/// <remarks>The method sets the dialog's initial directory to the user's Documents folder and generates a default file name based on the current date and time. The dialog is shown modally, and the result indicates whether the user confirmed the selection.</remarks>
	/// <param name="dialog">The file dialog to configure and display. Must not be null.</param>
	/// <param name="ext">The file extension to use for the default file name, without the leading period.</param>
	/// <returns>true if the user selects a file and confirms the dialog; otherwise, false.</returns>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Bulk-Observations_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog(owner: Form.ActiveForm) == DialogResult.OK;
	}

	/// <summary>Displays a save dialog and exports the table layout panel contents using the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title. The dialog allows the user to choose where to save the exported file and what name to give it.
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog. If the user cancels the dialog, the method returns without performing any export action.
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress, and then invoke the specified export action with the text box containing the output, the title for the export, and the selected file name. After the export action is completed, reset the cursor to the default state.
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(tableLayoutPanel, "Bulk Observations", saveFileDialog.FileName);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			ShowErrorMessage(message: $"An error has occurred during export: {ex.Message}");
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Sets the minimum index value for the range spinner.</summary>
	/// <param name="minimum">The minimum 1-based planetoid index (usually 1).</param>
	/// <remarks>Call this before showing the form so the spinner is pre-populated correctly.</remarks>
	public void SetMinimum(int minimum)
	{
		// Clamp to valid spinner bounds
		decimal value = Math.Max(val1: 1, val2: minimum);
		numericUpDownMinimum.Minimum = value;
		numericUpDownMinimum.Value = value;
	}

	/// <summary>Sets the maximum index value for the range spinner.</summary>
	/// <param name="maximum">The maximum 1-based planetoid index (equal to the total number of records).</param>
	/// <remarks>Call this before showing the form so the spinner is pre-populated correctly.</remarks>
	public void SetMaximum(int maximum)
	{
		// Clamp to a positive value
		decimal value = Math.Max(val1: 1, val2: maximum);
		numericUpDownMaximum.Maximum = value;
		numericUpDownMaximum.Value = value;
	}

	/// <summary>Resolves a relative URL against the MPC root URL into an absolute URL.</summary>
	/// <param name="baseUrl">The base URL (e.g. https://www.minorplanetcenter.net).</param>
	/// <param name="relativeUrl">The relative URL to resolve (e.g. ../tmp2/~0uY1.txt).</param>
	/// <returns>The resolved absolute URL string.</returns>
	/// <remarks>This method uses the <see cref="Uri"/> class to correctly handle relative paths and ensure a valid absolute URL is returned.</remarks>
	private static string ResolveUrl(string baseUrl, string relativeUrl)
	{
		// Use Uri to resolve relative paths correctly
		Uri baseUri = new(uriString: baseUrl);
		Uri resolved = new(baseUri: baseUri, relativeUri: relativeUrl);
		// Return the absolute URI as a string
		return resolved.AbsoluteUri;
	}

	/// <summary>Derives a safe local filename from the given absolute URL.</summary>
	/// <param name="absoluteUrl">The fully-qualified URL whose filename part is extracted.</param>
	/// <returns>The last path segment of the URL, or a timestamped fallback name when the segment is empty or contains only whitespace.</returns>
	/// <remarks>This method extracts the last segment of the URL path, unescapes it, and returns it as the filename. If the last segment is empty or whitespace, a fallback name based on the current timestamp is returned to ensure a valid filename.</remarks>
	private static string GetFileNameFromUrl(string absoluteUrl)
	{
		// Use Uri to parse the path and extract only the last segment
		if (Uri.TryCreate(uriString: absoluteUrl, uriKind: UriKind.Absolute, result: out Uri? uri))
		{
			string lastSegment = uri.Segments[^1];
			if (!string.IsNullOrWhiteSpace(value: lastSegment))
			{
				return Uri.UnescapeDataString(stringToUnescape: lastSegment);
			}
		}
		// Fallback: use a timestamp
		return $"observation_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
	}

	/// <summary>Adds a detailed error log entry and updates the error counter.</summary>
	/// <param name="url">The URL that was being processed when the error occurred.</param>
	/// <param name="errorType">The high-level type/category of the error.</param>
	/// <param name="errorDescription">A descriptive explanation of the error.</param>
	private void AddDownloadError(string url, string errorType, string errorDescription)
	{
		_errorCount++;
		_downloadErrors.Add(new BulkObservationsDownloadErrorEntry(
			Timestamp: DateTime.Now,
			Url: string.IsNullOrWhiteSpace(value: url) ? "-" : url,
			ErrorType: errorType,
			ErrorDescription: errorDescription));
	}

	/// <summary>Updates the progress bar percentage and taskbar progress for the given file counts.</summary>
	/// <param name="downloaded">Number of successfully processed files so far.</param>
	/// <param name="total">Total number of files to process.</param>
	/// <remarks>This method calculates the percentage of completion and updates the progress bar and taskbar accordingly. It also avoids unnecessary UI updates if the percentage has not changed.</remarks>
	private void UpdateProgress(int downloaded, int total)
	{
		// Avoid division by zero
		if (total <= 0)
		{
			return;
		}
		// Calculate the percentage of completion
		int percentage = Math.Clamp(value: downloaded * 100 / total, min: 0, max: 100);
		// Avoid unnecessary UI updates
		if (kryptonProgressBar.Value != percentage)
		{
			kryptonProgressBar.Value = percentage;
			kryptonProgressBar.Text = $"{percentage}%";
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)percentage, progressMax: 100);
		}
	}

	/// <summary>Refreshes all status labels with the latest counters and timing information.</summary>
	/// <param name="status">Short description of the current operation (e.g., "Downloading …").</param>
	/// <param name="downloaded">Number of files successfully downloaded so far.</param>
	/// <param name="total">Total number of files to download in this session.</param>
	/// <remarks>This method updates the status labels with the latest counters and timing information.</remarks>
	private void UpdateStatusLabels(string status, int downloaded, int total)
	{
		labelStatusValue.Text = status;
		labelFileCountValue.Text = $"{downloaded}/{total}";
		labelFileSizeValue.Text = $"{_currentFileSize:N0} / {_totalBytesDownloaded:N0} {I18nStrings.BytesText}";
		labelErrorCountValue.Text = _errorCount.ToString();
	}

	/// <summary>Resets all status labels and the progress bar to their initial idle state.</summary>
	/// <remarks>This method clears all counters and resets the progress bar to 0%.</remarks>
	private void ResetStatusLabels()
	{
		// Reset counters and status text
		labelStatusValue.Text = "-";
		labelFileCountValue.Text = "-";
		labelFileSizeValue.Text = "-";
		labelTimeValue.Text = "-";
		labelErrorCountValue.Text = "0";
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
	}

	/// <summary>Bulk-downloads observation data files for each planetoid in the configured index range.</summary>
	/// <param name="token">Cancellation token to stop the download early.</param>
	/// <returns>A <see cref="Task"/> that completes when all files have been processed or the operation is cancelled.</returns>
	/// <remarks>For each planetoid the method:
	/// <list type="number">
	///   <item><description>Fetches the MPC observations HTML page.</description></item>
	///   <item><description>Extracts the observations download link from the page.</description></item>
	///   <item><description>Resolves the relative URL to an absolute URL.</description></item>
	///   <item><description>Downloads the observations file and saves it to <c>%USERPROFILE%\Planetoid-DB\Observations\Data</c>.</description></item>
	/// </list>
	/// If a single file fails the error is counted and the loop continues with the next planetoid.</remarks>
	private async Task DownloadAllAsync(CancellationToken token)
	{
		// Determine the 1-based index range from the spinner values
		int minimum = (int)numericUpDownMinimum.Value;
		int maximum = (int)numericUpDownMaximum.Value;
		int total = maximum - minimum + 1;
		int downloaded = 0;
		_errorCount = 0;
		_totalBytesDownloaded = 0;
		_currentFileSize = 0;
		// Build and ensure the target directory exists
		string targetDirectory = Path.Combine(
			Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile),
			ObservationsDataSubPath.Replace(oldChar: '/', newChar: Path.DirectorySeparatorChar));
		Directory.CreateDirectory(path: targetDirectory);
		// Start the elapsed-time stopwatch
		_elapsedStopwatch.Restart();
		_uiTimer.Start();
		try
		{
			for (int i = minimum; i <= maximum; i++)
			{
				// Check for cancellation before each iteration
				token.ThrowIfCancellationRequested();
				// Suspend here if the user has paused
				if (_isPaused)
				{
					labelStatusValue.Text = "Paused";
					await (_resumeTcs?.Task ?? Task.CompletedTask).ConfigureAwait(continueOnCapturedContext: true);
				}
				// Re-check after resume
				token.ThrowIfCancellationRequested();
				// Convert 1-based position to 0-based list index, guard against out-of-range
				int listIndex = i - 1;
				if (listIndex < 0 || listIndex >= _planetoids.Count)
				{
					continue;
				}
				// Extract the packed minor planet number (first 7 characters) from the database record
				string record = _planetoids[index: listIndex];
				string packedNumber = record.Length >= 7 ? record[..7].Trim() : record.Trim();
				if (string.IsNullOrWhiteSpace(value: packedNumber))
				{
					continue;
				}
				// Attempt to download and save the observations file for this planetoid
				string currentUrl = string.Empty;
				try
				{
					// Check if we're disposing before starting HTTP operations
					if (_isDisposing)
					{
						token.ThrowIfCancellationRequested();
					}
					// Build the MPC query URL for this object
					string pageUrl = MpcBaseUrl + Uri.EscapeDataString(stringToEscape: packedNumber);
					currentUrl = pageUrl;
					string statusMsg = $"Loading page: {pageUrl}";
					UpdateStatusLabels(status: statusMsg, downloaded: downloaded, total: total);
					logger.Debug(message: statusMsg);
					// Fetch the HTML page
					string html = await _httpClient.GetStringAsync(requestUri: pageUrl, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
					// Locate the Observations section heading
					int observationsHeadingIndex = html.IndexOf(value: "<h2>Observations</h2>", comparisonType: StringComparison.Ordinal);
					if (observationsHeadingIndex < 0)
					{
						logger.Warn(message: $"No Observations section found for '{packedNumber}'.");
						AddDownloadError(url: currentUrl, errorType: "Missing observations section", errorDescription: $"No observations section found for '{packedNumber}'.");
						UpdateStatusLabels(status: $"No observations section for {packedNumber}", downloaded: downloaded, total: total);
						continue;
					}
					// Search for the download link in the HTML after the heading
					string htmlAfterHeading = html[observationsHeadingIndex..];
					Match downloadMatch = DownloadLinkRegex().Match(input: htmlAfterHeading);
					if (!downloadMatch.Success)
					{
						logger.Warn(message: $"Download link not found for '{packedNumber}'.");
						AddDownloadError(url: currentUrl, errorType: "Missing download link", errorDescription: $"Download link not found for '{packedNumber}'.");
						UpdateStatusLabels(status: $"Download link not found for {packedNumber}", downloaded: downloaded, total: total);
						continue;
					}
					// Resolve the relative URL to an absolute URL
					string relativeUrl = downloadMatch.Groups[groupnum: 1].Value;
					string absoluteUrl = ResolveUrl(baseUrl: MpcRootUrl, relativeUrl: relativeUrl);
					currentUrl = absoluteUrl;
					// Derive the local filename from the absolute URL
					string fileName = GetFileNameFromUrl(absoluteUrl: absoluteUrl);
					string localFilePath = Path.Combine(targetDirectory, fileName);
					string downloadStatus = $"Downloading: {absoluteUrl}";
					UpdateStatusLabels(status: downloadStatus, downloaded: downloaded, total: total);
					logger.Debug(message: downloadStatus);
					// Check if we're disposing before starting download
					if (_isDisposing)
					{
						token.ThrowIfCancellationRequested();
					}
					// Download the file and save it to disk
					using HttpResponseMessage response = await _httpClient.GetAsync(requestUri: absoluteUrl, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
					_ = response.EnsureSuccessStatusCode();
					// Track file size for the status display
					_currentFileSize = response.Content.Headers.ContentLength ?? 0;
					await using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
					await using FileStream fileStream = new(path: localFilePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
					string savingStatus = $"Saving file: {localFilePath}";
					UpdateStatusLabels(status: savingStatus, downloaded: downloaded, total: total);
					logger.Debug(message: savingStatus);
					await contentStream.CopyToAsync(destination: fileStream, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
					// Accumulate the total bytes after saving
					_totalBytesDownloaded += _currentFileSize > 0 ? _currentFileSize : fileStream.Length;
					// Increment the success counter and refresh progress
					downloaded++;
					UpdateProgress(downloaded: downloaded, total: total);
					UpdateStatusLabels(status: $"Saved: {fileName}", downloaded: downloaded, total: total);
				}
				// A single file failure must not abort the entire session; log, count, and continue
				catch (OperationCanceledException)
				{
					// Propagate cancellation so the outer try/catch can handle it
					throw;
				}
				catch (ObjectDisposedException) when (_isDisposing)
				{
					// HttpClient was disposed during shutdown - treat as cancellation
					throw new OperationCanceledException();
				}
				catch (Exception ex)
				{
					AddDownloadError(url: currentUrl, errorType: ex.GetType().Name, errorDescription: ex.Message);
					logger.Error(exception: ex, message: $"Error downloading observations for '{packedNumber}': {ex.Message}");
					UpdateStatusLabels(status: $"Error for {packedNumber}: {ex.Message}", downloaded: downloaded, total: total);
				}
			}
			// All iterations completed
			string doneMsg = $"Done. {downloaded}/{total} files downloaded, {_errorCount} error(s).";
			labelStatusValue.Text = doneMsg;
			SetStatusBar(label: labelInformation, text: doneMsg);
			logger.Info(message: doneMsg);
		}
		finally
		{
			_elapsedStopwatch.Stop();
			_uiTimer.Stop();
		}
	}

	#endregion

	#region timer event handler

	/// <summary>Updates the elapsed / estimated remaining time label every second while a download is active.</summary>
	/// <param name="sender">Event source (the timer).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This handler fires on the UI thread.</remarks>
	private void UiTimer_Tick(object? sender, EventArgs e)
	{
		// Only update when the form is still open
		if (!IsHandleCreated || IsDisposed || Disposing)
		{
			return;
		}
		TimeSpan elapsed = _elapsedStopwatch.Elapsed;
		labelTimeValue.Text = $"{elapsed:hh\\:mm\\:ss}";
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event. Initialises the spinners and clears the status bar.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The spinner bounds are clamped to the number of loaded planetoid records.</remarks>
	private void BulkObservationsDataDownloaderForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		// Initialise spinner maximum to the number of loaded records (if not already set by the caller)
		if (_planetoids.Count > 0)
		{
			decimal count = _planetoids.Count;
			// Only override if the caller has not already called SetMaximum with a meaningful value
			if (numericUpDownMaximum.Value <= 1 && count > 1)
			{
				numericUpDownMaximum.Maximum = count;
				numericUpDownMaximum.Value = count;
			}
		}
	}

	/// <summary>Handles the FormClosing event. Cancels any active download and disposes resources.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>Ensures the background download is stopped when the form is closed. Resources are disposed in the Dispose method.</remarks>
	private void BulkObservationsDataDownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Signal cancellation to the running download
		_cancellationTokenSource?.Cancel();
		// Resume any awaiting pause so the download task can observe the cancellation
		_isPaused = false;
		_resumeTcs?.TrySetResult(result: true);
		// Stop the UI timer
		_uiTimer.Stop();
	}

	#endregion

	#region click event handlers

	/// <summary>Handles the Click event of the Start/Pause button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks><list type="bullet">
	///   <item><description>Idle → starts a new download session.</description></item>
	///   <item><description>Running → pauses the download; the button text changes to "Resume".</description></item>
	///   <item><description>Paused → resumes the download; the button text reverts to "Pause".</description></item>
	/// </list></remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// If a download is active and not paused, pause it
		if (_cancellationTokenSource != null && !_isPaused)
		{
			_isPaused = true;
			_resumeTcs = new TaskCompletionSource<bool>();
			buttonStart.Text = "&Resume";
			buttonStart.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
			SetStatusBar(label: labelInformation, text: "Download paused.");
			return;
		}
		// If paused, resume the download
		if (_cancellationTokenSource != null && _isPaused)
		{
			_isPaused = false;
			TaskCompletionSource<bool>? tcs = _resumeTcs;
			_resumeTcs = null;
			tcs?.TrySetResult(result: true);
			buttonStart.Text = "&Pause";
			buttonStart.Image = FatcowIcons16px.fatcow_control_pause_16px;
			SetStatusBar(label: labelInformation, text: "Download resumed.");
			return;
		}
		// Validate that there is data to process
		if (_planetoids.Count == 0)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Validate the range
		if (numericUpDownMinimum.Value > numericUpDownMaximum.Value)
		{
			ShowErrorMessage(message: "Minimum index must not be greater than the maximum index.");
			return;
		}
		// Start a new download session
		_downloadErrors.Clear();
		ResetStatusLabels();
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		buttonStart.Text = "&Pause";
		buttonStart.Image = FatcowIcons16px.fatcow_control_pause_16px;
		buttonCancel.Enabled = true;
		numericUpDownMinimum.Enabled = false;
		numericUpDownMaximum.Enabled = false;
		ClearStatusBar(label: labelInformation);
		// Run the download in a background task to keep the UI responsive
		try
		{
			_downloadTask = DownloadAllAsync(token: token);
			await _downloadTask.ConfigureAwait(continueOnCapturedContext: true);
		}
		// Handle cancellation gracefully
		catch (OperationCanceledException)
		{
			labelStatusValue.Text = "Cancelled";
			SetStatusBar(label: labelInformation, text: "Download cancelled.");
			logger.Info(message: "Bulk download cancelled by user.");
		}
		// Handle unexpected errors without crashing the form
		catch (Exception ex)
		{
			labelStatusValue.Text = $"Error: {ex.Message}";
			logger.Error(exception: ex, message: $"Bulk download failed: {ex.Message}");
			ShowErrorMessage(message: $"An error occurred during the bulk download: {ex.Message}");
		}
		// Reset UI regardless of outcome
		finally
		{
			buttonStart.Text = "&Start";
			buttonStart.Image = FatcowIcons16px.fatcow_control_play_16px;
			buttonCancel.Enabled = false;
			numericUpDownMinimum.Enabled = true;
			numericUpDownMaximum.Enabled = true;
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			_isPaused = false;
			_resumeTcs = null;
			_downloadTask = null;
		}
	}

	/// <summary>Handles the Click event of the Cancel button. Cancels the active download.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>If the download is currently paused, the pause is released first so the task can observe the cancellation token.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// If currently paused, unblock the awaiting task before cancelling
		if (_isPaused)
		{
			_isPaused = false;
			TaskCompletionSource<bool>? tcs = _resumeTcs;
			_resumeTcs = null;
			tcs?.TrySetResult(result: true);
		}
		_cancellationTokenSource?.Cancel();
		buttonCancel.Enabled = false;
		SetStatusBar(label: labelInformation, text: "Cancelling…");
	}

	/// <summary>Handles the Click event of the error log button and displays all captured download errors.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	private void ButtonErrorLog_Click(object sender, EventArgs e)
	{
		using BulkObservationsDownloadErrorsForm form = new(entries: _downloadErrors);
		_ = form.ShowDialog(owner: this);
	}

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a ReStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word document.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As RTF menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As SQL menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As PostScript menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

	#endregion
}
