// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Resources;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Planetoid_DB;

/// <summary>Form for bulk-downloading MPC observations data files for a range of minor planets.</summary>
/// <remarks>The form iterates over all planetoid database records from the configured minimum to the maximum
/// index, fetches the observations HTML page for each, extracts the download link, and saves the data file
/// to <c>%USERPROFILE%\Planetoid-DB\Observations\Data</c>.  The download can be started, paused, resumed
/// and cancelled at any time using the toolbar buttons.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class BulkInformationsDataDownloaderForm : BaseKryptonForm
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

	/// <summary>Shared <see cref="HttpClient"/> for HTTP requests. Reused to avoid socket exhaustion.</summary>
	/// <remarks>This <see cref="HttpClient"/> instance is shared across the application to improve performance and reduce resource usage.</remarks>
	private static readonly HttpClient httpClient = new()
	{
		// Set a reasonable timeout for HTTP requests to prevent hanging indefinitely
		Timeout = TimeSpan.FromSeconds(value: 60)
	};

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Cancellation token source for the running download task.</summary>
	/// <remarks>Set to <c>null</c> when no download is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Indicates whether the download is currently paused.</summary>
	/// <remarks>Checked before processing each planetoid. Set to <c>true</c> by the Start/Pause button
	/// when the download is active, and back to <c>false</c> on resume.</remarks>
	private volatile bool _isPaused;

	/// <summary>Used to signal that a paused download should resume.</summary>
	/// <remarks>Created when the download is paused and completed (set) when the user presses the
	/// Start/Resume button.</remarks>
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
	/// <remarks>Incremented whenever fetching or saving a file fails. The failed file is skipped and
	/// downloading continues with the next planetoid.</remarks>
	private int _errorCount;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the compiled regular expression for matching the download link in the MPC observations section.</summary>
	/// <returns>A <see cref="Regex"/> instance for matching download links.</returns>
	[GeneratedRegex(pattern: @"<a\s+href=""([^""]+)""\s+target=""_blank"">download</a>", options: RegexOptions.IgnoreCase)]
	private static partial Regex DownloadLinkRegex();

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="BulkInformationsDataDownloaderForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.
	/// The form displays minimum/maximum spinners pre-populated with 1 and the count of records.</remarks>
	public BulkInformationsDataDownloaderForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		_planetoids = planetoids;
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
	/// <returns>The last path segment of the URL, or a timestamped fallback name when the segment is empty
	/// or contains only whitespace.</returns>
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

	/// <summary>Updates the progress bar percentage and taskbar progress for the given file counts.</summary>
	/// <param name="downloaded">Number of successfully processed files so far.</param>
	/// <param name="total">Total number of files to process.</param>
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
	private void UpdateStatusLabels(string status, int downloaded, int total)
	{
		labelStatusValue.Text = status;
		labelFileCountValue.Text = $"{downloaded}/{total}";
		labelFileSizeValue.Text = $"{_currentFileSize:N0} / {_totalBytesDownloaded:N0} {I18nStrings.BytesText}";
		labelErrorCountValue.Text = _errorCount.ToString();
	}

	/// <summary>Resets all status labels and the progress bar to their initial idle state.</summary>
	private void ResetStatusLabels()
	{
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
				try
				{
					// Build the MPC query URL for this object
					string pageUrl = MpcBaseUrl + Uri.EscapeDataString(stringToEscape: packedNumber);
					string statusMsg = $"Loading page: {pageUrl}";
					UpdateStatusLabels(status: statusMsg, downloaded: downloaded, total: total);
					logger.Debug(message: statusMsg);
					// Fetch the HTML page
					string html = await httpClient.GetStringAsync(requestUri: pageUrl, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
					// Locate the Observations section heading
					int observationsHeadingIndex = html.IndexOf(value: "<h2>Observations</h2>", comparisonType: StringComparison.Ordinal);
					if (observationsHeadingIndex < 0)
					{
						logger.Warn(message: $"No Observations section found for '{packedNumber}'.");
						_errorCount++;
						UpdateStatusLabels(status: $"No observations section for {packedNumber}", downloaded: downloaded, total: total);
						continue;
					}
					// Search for the download link in the HTML after the heading
					string htmlAfterHeading = html[observationsHeadingIndex..];
					Match downloadMatch = DownloadLinkRegex().Match(input: htmlAfterHeading);
					if (!downloadMatch.Success)
					{
						logger.Warn(message: $"Download link not found for '{packedNumber}'.");
						_errorCount++;
						UpdateStatusLabels(status: $"Download link not found for {packedNumber}", downloaded: downloaded, total: total);
						continue;
					}
					// Resolve the relative URL to an absolute URL
					string relativeUrl = downloadMatch.Groups[groupnum: 1].Value;
					string absoluteUrl = ResolveUrl(baseUrl: MpcRootUrl, relativeUrl: relativeUrl);
					// Derive the local filename from the absolute URL
					string fileName = GetFileNameFromUrl(absoluteUrl: absoluteUrl);
					string localFilePath = Path.Combine(targetDirectory, fileName);
					string downloadStatus = $"Downloading: {absoluteUrl}";
					UpdateStatusLabels(status: downloadStatus, downloaded: downloaded, total: total);
					logger.Debug(message: downloadStatus);
					// Download the file and save it to disk
					using HttpResponseMessage response = await httpClient.GetAsync(requestUri: absoluteUrl, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);
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
				catch (Exception ex)
				{
					_errorCount++;
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
	private void BulkInformationsDataDownloaderForm_Load(object sender, EventArgs e)
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
	/// <remarks>Ensures the background download and timer are stopped when the form is closed.</remarks>
	private void BulkInformationsDataDownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Signal cancellation to the running download
		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource?.Dispose();
		_cancellationTokenSource = null;
		// Resume any awaiting pause so the download task can observe the cancellation
		_isPaused = false;
		_resumeTcs?.TrySetResult(result: true);
		_resumeTcs = null;
		// Stop and dispose the UI timer
		_uiTimer.Stop();
		_uiTimer.Dispose();
	}

	#endregion

	#region click event handlers

	/// <summary>Handles the Click event of the Start/Pause button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// <list type="bullet">
	///   <item><description>Idle → starts a new download session.</description></item>
	///   <item><description>Running → pauses the download; the button text changes to "Resume".</description></item>
	///   <item><description>Paused → resumes the download; the button text reverts to "Pause".</description></item>
	/// </list>
	/// </remarks>
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
			_ = MessageBox.Show(
				text: "No planetoid data available.",
				caption: I18nStrings.InformationCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information);
			return;
		}
		// Validate the range
		if (numericUpDownMinimum.Value > numericUpDownMaximum.Value)
		{
			ShowErrorMessage(message: "Minimum index must not be greater than the maximum index.");
			return;
		}
		// Start a new download session
		ResetStatusLabels();
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		buttonStart.Text = "&Pause";
		buttonStart.Image = FatcowIcons16px.fatcow_control_pause_16px;
		buttonCancel.Enabled = true;
		numericUpDownMinimum.Enabled = false;
		numericUpDownMaximum.Enabled = false;
		ClearStatusBar(label: labelInformation);
		try
		{
			await DownloadAllAsync(token: token).ConfigureAwait(continueOnCapturedContext: true);
		}
		catch (OperationCanceledException)
		{
			labelStatusValue.Text = "Cancelled";
			SetStatusBar(label: labelInformation, text: "Download cancelled.");
			logger.Info(message: "Bulk download cancelled by user.");
		}
		catch (Exception ex)
		{
			labelStatusValue.Text = $"Error: {ex.Message}";
			logger.Error(exception: ex, message: $"Bulk download failed: {ex.Message}");
			ShowErrorMessage(message: $"An error occurred during the bulk download: {ex.Message}");
		}
		finally
		{
			// Reset UI regardless of outcome
			buttonStart.Text = "&Start";
			buttonStart.Image = FatcowIcons16px.fatcow_control_play_16px;
			buttonCancel.Enabled = false;
			numericUpDownMinimum.Enabled = true;
			numericUpDownMaximum.Enabled = true;
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			_isPaused = false;
			_resumeTcs = null;
		}
	}

	/// <summary>Handles the Click event of the Cancel button. Cancels the active download.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>If the download is currently paused, the pause is released first so the task can
	/// observe the cancellation token.</remarks>
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

	#endregion
}
