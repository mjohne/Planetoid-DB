// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;

using static Planetoid_DB.TerminologyForm;

namespace Planetoid_DB;

/// <summary>Represents a form that displays terminology information.</summary>
/// <remarks>This form is responsible for displaying and managing terminology information within the application.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]

public partial class PlanetoidDbForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the current tag text of the control.</summary>
	/// <remarks>This string is used to store the current tag text of the control for clipboard operations.</remarks>
	private string currentTagText = string.Empty;

	/// <summary>Stores the current position in the planetoids database and the step position for navigation.</summary>
	/// <remarks>This integer is used to store the current position in the planetoids database and the step position for navigation.</remarks>
	private int currentPosition, stepPosition;

	/// <summary>Stores the planetoids database.</summary>
	/// <remarks>This list is used to store the planetoids database entries.</remarks>
	private readonly List<string> planetoidsDatabase = [];

	/// <summary>Splash screen form instance.</summary>
	/// <remarks>This form is displayed while the application is loading.</remarks>
	private readonly SplashScreenForm formSplashScreen = new();

	/// <summary>Filenames for the MPCORB database.</summary>
	/// <remarks>These strings are used to store the filenames for the MPCORB database.</remarks>
	private readonly string filenameMpcorb = Settings.Default.systemFilenameMpcorb;
	private readonly string filenameMpcorbTemp = Settings.Default.systemFilenameMpcorbTemp;

	/// <summary>Filenames for the ASTORB database.</summary>
	/// <remarks>These strings are used to store the filenames for the ASTORB database.</remarks>
	private readonly string filenameAstorb = Settings.Default.systemFilenameAstorb;
	private readonly string filenameAstorbTemp = Settings.Default.systemFilenameAstorbTemp;

	/// <summary>URI for the MPCORB database.</summary>
	/// <remarks>This URI is used to access the MPCORB database.</remarks>
	private readonly Uri uriMpcorb = new(uriString: Settings.Default.systemMpcorbDatGzUrl);

	/// <summary>URI for the ASTORB database.</summary>
	/// <remarks>This URI is used to access the ASTORB database.</remarks>
	private readonly Uri uriAstorb = new(uriString: Settings.Default.systemAstorbDatGzUrl);

	/*
	private readonly IProgress<int>? downloadProgress;
	private const int bufferSize = 81920; // 80 KB buffer size for downloading
	private const int defaultStepPosition = 10; // Default step position for navigation
	private const int maxRetries = 3; // Maximum number of retries for downloading
	private const int delayBetweenRetries = 2000; // Delay between retries in milliseconds
	private const int downloadTimeoutInSeconds = 300; // Timeout for download in seconds (5 minutes)
	private const int minFileSizeInBytes = 500_000; // Minimum file size in bytes (500 KB)
	private const int maxFileSizeInBytes = 50_000_000; // Maximum file size in bytes (50 MB)
	private const int mpcorbDatExpectedFileSizeInBytes = 12_000_000; // Expected file size for MPCORB.DAT (12 MB)
	private const int mpcorbDatGzExpectedFileSizeInBytes = 3_000_000; // Expected file size for MPCORB.DAT.GZ (3 MB)
	private const int minPlanetoidDatabaseEntries = 100_000; // Minimum number of entries in the planetoid database
	private const int maxPlanetoidDatabaseEntries = 1_000_000; // Maximum number of entries in the planetoid database
	private const int connectionTimeoutInSeconds = 10; // Timeout for connection in seconds
	private const int readWriteTimeoutInSeconds = 30; // Timeout for read/write operations in seconds
	private const int maxNetworkRetries = 3; // Maximum number of retries for network operations
	private const int delayBetweenNetworkRetries = 2000; // Delay between network retries in milliseconds
	private const string userAgentString = "PlanetoidDB/1.0"; // User-Agent string for HTTP requests
	private const string mpcorbDatGzFileExtension = ".gz"; // File extension for gzipped files
	private const string mpcorbDatFileExtension = ".dat"; // File extension for dat files
	private const string mpcorbDatBackupFileExtension = ".bak"; // File extension for backup files
	private const string mpcorbDatTempFileExtension = ".tmp"; // File extension for temporary files
	private const string mpcorbDatUrl = "https://minorplanetcenter.net/iau/MPCORB/MPCORB.DAT.gz"; // URL for the MPCORB.DAT.GZ file
	private const string mpcorbDatLocalFileName = "MPCORB.DAT"; // Local filename for the MPCORB.DAT file
	private const string mpcorbDatGzLocalFileName = "MPCORB.DAT.gz"; // Local filename for the MPCORB.DAT.GZ file
	private const string mpcorbDatBackupFileName = "MPCORB.DAT.bak"; // Local filename for the backup file
	private const string mpcorbDatTempFileName = "MPCORB.DAT.tmp"; // Local filename for the temporary file
	private const string dateFormat = "yyyy-MM-dd HH:mm:ss"; // Date format for displaying dates
	private const string isoDateFormat = "yyyy-MM-dd"; // ISO date format for parsing dates
	private const string timeFormat = "HH:mm:ss"; // Time format for displaying times
	private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss"; // DateTime format for displaying date and time
	private const string gZipMimeType = "application/gzip"; // MIME type for gzip files
	private const string octetStreamMimeType = "application/octet-stream"; // MIME type for binary files
	private const string textPlainMimeType = "text/plain"; // MIME type for plain text files
	private const string httpScheme = "http"; // HTTP scheme
	private const string httpsScheme = "https"; // HTTPS scheme
	private const string ftpScheme = "ftp"; // FTP scheme
	private const string fileScheme = "file"; // File scheme
	private const string localFilePrefix = "file://"; // Prefix for local file paths
	private const string tempFolderPath = "Temp"; // Temporary folder path
	private const string backupFolderPath = "Backup"; // Backup folder path
	private const string mpcorbDatFileHeader = "MPCORB"; // Expected header in the MPCORB.DAT file
	private const string mpcorbDatGzFileHeader = "\x1F\x8B"; // Expected header in the MPCORB.DAT.GZ file
	*/

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="PlanetoidDbForm"/> class.</summary>
	/// <remarks>This constructor initializes the form and sets the version text.</remarks>
	public PlanetoidDbForm()
	{
		InitializeComponent();
		TextExtra = $"{Assembly.GetExecutingAssembly().GetName().Version}";
		// Apply comprehensive flicker reduction for the TableLayoutPanel
		OptimizeTableLayoutPanelForFlickerReduction();
	}

	/// <summary>Initializes a new instance of the <see cref="PlanetoidDbForm"/> class with a specified MPCORB.DAT file path.</summary>
	/// <param name="mpcorbDatFilePath">The file path to the MPCORB.DAT file.</param>
	/// <remarks>This constructor initializes the form and sets the version text.</remarks>
	public PlanetoidDbForm(string mpcorbDatFilePath)
	{
		// Initialize the form components
		InitializeComponent();
		TextExtra = $"{Assembly.GetExecutingAssembly().GetName().Version}";
		ClearStatusBar(label: labelInformation);
		MpcOrbDatFilePath = mpcorbDatFilePath;
		// Apply comprehensive flicker reduction for the TableLayoutPanel
		OptimizeTableLayoutPanelForFlickerReduction();
	}

	/// <summary>Optimizes the TableLayoutPanel to eliminate flickering during label updates.</summary>
	/// <remarks>This method enables double buffering and optimized painting styles on the panel and all child labels.</remarks>
	private void OptimizeTableLayoutPanelForFlickerReduction()
	{
		// Use reflection to access the protected DoubleBuffered property and SetStyle method of the Control class, and apply them to the TableLayoutPanel and all child label controls to enable comprehensive double buffering and optimized painting styles. This approach helps to reduce flickering when updating the labels in the panel.
		try
		{
			// Enable double buffering for the TableLayoutPanel itself
			PropertyInfo? doubleBufferedProperty = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo? setStyleMethod = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			// Apply to the main panel
			doubleBufferedProperty?.SetValue(obj: tableLayoutPanelData, value: true, index: null);
			setStyleMethod?.Invoke(obj: tableLayoutPanelData, parameters: [
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw,
				true
			]);
			// Apply double buffering to all label controls within the panel to prevent individual label flickering
			foreach (Control control in tableLayoutPanelData.Controls)
			{
				if (control is Label or KryptonLabel)
				{
					doubleBufferedProperty?.SetValue(obj: control, value: true, index: null);
					setStyleMethod?.Invoke(obj: control, parameters: [
						ControlStyles.OptimizedDoubleBuffer |
						ControlStyles.AllPaintingInWmPaint,
						true
					]);
				}
			}
		}
		// Log a warning if enabling double buffering fails, but allow the application to continue running
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Failed to enable comprehensive double buffering on tableLayoutPanel. UI may experience flickering.");
		}
	}

	#endregion

	#region helper methods

	/// <summary>Gets the file path of the MPCORB.DAT file.</summary>
	/// <remarks>This property is used to store the file path of the MPCORB.DAT file.</remarks>
	private string MpcOrbDatFilePath { get; set; } = string.Empty;

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom display string for the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Tries to parse an integer from the input string.</summary>
	/// <param name="input">The input string to parse.</param>
	/// <param name="value">The parsed integer value if successful.</param>
	/// <param name="errorMessage">An error message if parsing fails.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	/// <remarks>This method is used to try parsing an integer from the input string.</remarks>
	public static bool TryParseInt(string input, out int value, out string errorMessage)
	{
		// Initialize output parameters
		value = 0;
		errorMessage = string.Empty;
		// Check if the input is null or whitespace
		if (string.IsNullOrWhiteSpace(value: input))
		{
			// Set the error message and return false
			errorMessage = "The entered text is empty or consists only of spaces.";
			return false;
		}
		// Try to parse the integer
		// If parsing fails, set the error message
		if (!int.TryParse(s: input, result: out value))
		{
			// Set the error message and return false
			errorMessage = $"The value \"{input}\" is not a valid integer.";
			return false;
		}
		// Parsing was successful
		return true;
	}

	/// <summary>Restarts the application.</summary>
	/// <remarks>This method is used to restart the application.</remarks>
	private void Restart()
	{
		// Close the current form and start a new instance of the application
		_ = Process.Start(fileName: Application.ExecutablePath);
		Close();
	}

	/// <summary>Asks the user if they want to restart the application after downloading the database.</summary>
	/// <remarks>This method is used to ask the user if they want to restart the application after downloading the database.</remarks>
	private void AskForRestartAfterDownloadingDatabase()
	{
		// Ask the user if they want to restart the application after downloading the database
		// and show a message box with the option to restart or not
		// The message box will have the text "Download complete. Do you want to restart the application?"
		// and the caption "Information"
		// If the user clicks "Yes", restart the application
		// If the user clicks "No", do nothing
		if (KryptonMessageBox.Show(text: I18nStrings.DownloadCompleteAndRestartQuestionText, caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.YesNo, icon: KryptonMessageBoxIcon.Question, defaultButton: KryptonMessageBoxDefaultButton.Button1) == DialogResult.Yes)
		{
			// Restart the application
			Restart();
		}
	}

	/// <summary>Navigates to the specified position in the planetoids database.</summary>
	/// <param name="position">The position to navigate to.</param>
	/// <remarks>This method is used to navigate to the specified position in the planetoids database.</remarks>
	internal void GotoCurrentPosition(int position)
	{
		// Handle the case where the database is empty
		if (position < 0 || position >= planetoidsDatabase.Count)
		{
			toolStripLabelIndexPosition.ToolTipText = "Index: 0";
			return;
		}
		// Get entry string once to avoid repeated ToString() calls
		string? entryStr = planetoidsDatabase[index: position]?.ToString();
		// If the entry string is null or empty, clear all labels and return early
		if (string.IsNullOrEmpty(value: entryStr))
		{
			// Clear all labels to indicate no data is available
			toolStripLabelIndexPosition.Text = string.Empty;
			tableLayoutPanelData.SuspendLayout();
			try
			{
				foreach (Control control in tableLayoutPanelData.Controls)
				{
					if (control is KryptonLabel or Label)
					{
						control.Text = string.Empty;
					}
				}
			}
			// Resume layout without performing layout to avoid unnecessary redraws
			finally
			{
				tableLayoutPanelData.ResumeLayout(performLayout: false);
			}
			return;
		}
		// Suspend both the panel layout and painting to eliminate all flicker
		tableLayoutPanelData.SuspendLayout();
		try
		{
			// Batch all text updates with minimal overhead
			toolStripLabelIndexPosition.ToolTipText = $"Index: {currentPosition + 1}/{planetoidsDatabase.Count}";
			// Update all labels in one go - use cached string reference
			labelIndexData.Text = entryStr[..7].Trim();
			labelAbsoluteMagnitudeData.Text = entryStr.Substring(startIndex: 8, length: 5).Trim();
			labelSlopeParameterData.Text = entryStr.Substring(startIndex: 14, length: 5).Trim();
			labelEpochData.Text = entryStr.Substring(startIndex: 20, length: 5).Trim();
			labelMeanAnomalyAtTheEpochData.Text = entryStr.Substring(startIndex: 26, length: 9).Trim();
			labelArgumentOfThePerihelionData.Text = entryStr.Substring(startIndex: 37, length: 9).Trim();
			labelLongitudeOfTheAscendingNodeData.Text = entryStr.Substring(startIndex: 48, length: 9).Trim();
			labelInclinationToTheEclipticData.Text = entryStr.Substring(startIndex: 59, length: 9).Trim();
			labelOrbitalEccentricityData.Text = entryStr.Substring(startIndex: 70, length: 9).Trim();
			labelMeanDailyMotionData.Text = entryStr.Substring(startIndex: 80, length: 11).Trim();
			labelSemiMajorAxisData.Text = entryStr.Substring(startIndex: 92, length: 11).Trim();
			labelReferenceData.Text = entryStr.Substring(startIndex: 107, length: 9).Trim();
			labelNumberOfObservationsData.Text = entryStr.Substring(startIndex: 117, length: 5).Trim();
			labelNumberOfOppositionsData.Text = entryStr.Substring(startIndex: 123, length: 3).Trim();
			labelObservationSpanData.Text = entryStr.Substring(startIndex: 127, length: 9).Trim();
			labelRmsResidualData.Text = entryStr.Substring(startIndex: 137, length: 4).Trim();
			labelComputerNameData.Text = entryStr.Substring(startIndex: 150, length: 10).Trim();
			labelFlagsData.Text = entryStr.Substring(startIndex: 161, length: 4).Trim();
			labelReadableDesignationData.Text = entryStr.Substring(startIndex: 166, length: 28).Trim();
			labelDateLastObservationData.Text = entryStr.Substring(startIndex: 194, length: 8).Trim();
			toolStripLabelIndexPosition.Text = $@"{I18nStrings.Index}: {position + 1:N0} / {planetoidsDatabase.Count:N0}";
		}
		finally
		{
			// Resume layout and perform any pending layout logic.
			tableLayoutPanelData.ResumeLayout(performLayout: true);
		}
	}

	/// <summary>Jumps to the record with the specified index or designation.</summary>
	/// <param name="index">The index of the record.</param>
	/// <param name="designation">The designation of the record.</param>
	internal void JumpToRecord(string index, string designation)
	{
		// Loop through the planetoids database to find the record with the specified index or designation
		for (int i = 0; i < planetoidsDatabase.Count; i++)
		{
			// Extract the current entry from the database
			string entry = planetoidsDatabase[index: i];
			// Check if the index matches the current entry's index (first 7 characters)
			if (!string.IsNullOrWhiteSpace(value: index) && entry.Length >= 7 && entry[..7].Trim().Equals(value: index, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the index matches, set the current position to the index and navigate to that position
				currentPosition = i;
				GotoCurrentPosition(position: currentPosition);
				return;
			}
			// If the index does not match, check if the designation matches the current entry's designation (characters 166-193)
			if (!string.IsNullOrEmpty(value: designation) && entry.Length >= 194 && entry.Substring(startIndex: 166, length: 28).Trim().Equals(value: designation, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the designation matches, set the current position to the index and navigate to that position
				currentPosition = i;
				GotoCurrentPosition(position: currentPosition);
				return;
			}
		}
		// If no matching record is found, show an error message box to the user
		ShowErrorMessage(message: "Record not found in the current loaded database.");
	}

	/// <summary>Retrieves the last modified date and time (in UTC) of the resource at the specified URI.</summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>The <see cref="DateTime"/> representing the last modified date and time in UTC if available; otherwise, <see cref="DateTime.MinValue"/>. </returns>
	/// <remarks>This method is used to retrieve the last modified date and time of a resource.</remarks>
	private static DateTime GetLastModified(Uri uri)
	{
		// Validate the input URI
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Use HttpClient to retrieve only the headers (HEAD request)
		using HttpClient client = new();
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request);
		// Check if the request was successful
		if (response.IsSuccessStatusCode)
		{
			// Check if the Last-Modified header is present and return its value
			if (response.Content.Headers.LastModified.HasValue)
			{
				// Return the last modified date in UTC
				return response.Content.Headers.LastModified.Value.UtcDateTime;
			}
		}
		// If the Last-Modified header is not present or the request failed, return DateTime.MinValue
		return DateTime.MinValue;
	}

	/// <summary>Gets the content length of the specified URI.</summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>The content length of the URI.</returns>
	/// <remarks>This method is used to retrieve the content length of a resource.</remarks>
	private static long GetContentLength(Uri uri)
	{
		// Validate the input URI
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Use HttpClient to retrieve only the headers (HEAD request)
		using HttpClient client = new();
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request);
		// Check if the request was successful
		if (response.IsSuccessStatusCode)
		{
			// Check if the Content-Length header is present and return its value
			if (response.Content.Headers.ContentLength.HasValue)
			{
				// Return the content length
				return response.Content.Headers.ContentLength.Value;
			}
		}
		// If the Content-Length header is not present or the request failed, return 0
		return 0;
	}

	/// <summary>Checks if an update for the MPCORB database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the MPCORB database is available.</remarks>
	private bool IsMpcorbDatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameMpcorb))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameMpcorb);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriMpcorb);
			// Get the content length of the online file
			_ = GetContentLength(uri: uriMpcorb);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
		}
		catch (WebException)
		{
			return false;
		}
		catch (IOException)
		{
			return false;
		}
	}

	/// <summary>Checks if an update for the ASTORB database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the ASTORB database is available.</remarks>
	private bool IsAstorbDatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameAstorb))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameAstorb);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriAstorb);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
		}
		catch (WebException)
		{
			return false;
		}
		catch (IOException)
		{
			return false;
		}
	}

	/// <summary>Loads a random minor planet from the database.</summary>
	/// <remarks>This method is used to load a random minor planet from the database.</remarks>
	private void LoadRandomMinorPlanet() => GotoCurrentPosition(position: currentPosition = new Random().Next(maxValue: planetoidsDatabase.Count + 1));

	/// <summary>Navigates to the beginning of the data.</summary>
	/// <remarks>This method is used to navigate to the beginning of the data.</remarks>
	private void NavigateToTheBeginOfTheData() => GotoCurrentPosition(position: currentPosition = 0);

	/// <summary>Navigates backward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate backward by a specified step in the data.</remarks>
	private void NavigateSomeDataBackward()
	{
		// Decrease the current position by the step size
		currentPosition -= stepPosition;
		if (currentPosition < 1)
		{
			// If the current position is less than 1, wrap around to the end of the database
			currentPosition += planetoidsDatabase.Count;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the previous data entry.</summary>
	/// <remarks>This method is used to navigate to the previous data entry in the planetoids database.</remarks>
	private void NavigateToThePreviousData()
	{
		// If the current position is 0, wrap around to the last entry in the database
		if (currentPosition == 0)
		{
			// Set the current position to the last entry in the database
			// This ensures that when the user navigates backward from the first entry, they go to the last entry
			// This is useful for circular navigation
			currentPosition = planetoidsDatabase.Count - 1;
		}
		else
		{
			// Decrease the current position by 1
			currentPosition--;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the next data entry.</summary>
	/// <remarks>This method is used to navigate to the next data entry in the planetoids database.</remarks>
	private void NavigateToTheNextData()
	{
		// If the current position is the last entry in the database, wrap around to the first entry
		if (currentPosition == planetoidsDatabase.Count - 1)
		{
			// Set the current position to 0 (the first entry in the database)
			// This ensures that when the user navigates forward from the last entry, they go to the first entry
			// This is useful for circular navigation
			currentPosition = 0;
		}
		else
		{
			// Increase the current position by 1
			currentPosition++;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates forward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate forward by a specified step in the data.</remarks>
	private void NavigateSomeDataForward()
	{
		// Increase the current position by the step size
		// This allows the user to navigate through the database in larger increments
		currentPosition += stepPosition;
		// If the current position exceeds the total number of entries in the database, wrap around to the beginning
		if (currentPosition > planetoidsDatabase.Count)
		{
			// Set the current position to the beginning of the database
			// This ensures that when the user navigates forward from the last entry, they go to the first entry
			// This is useful for circular navigation
			currentPosition -= planetoidsDatabase.Count;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the end of the data.</summary>
	/// <remarks>This method is used to navigate to the end of the data.</remarks>
	private void NavigateToTheEndOfTheData() => GotoCurrentPosition(position: currentPosition = planetoidsDatabase.Count - 1);

	/// <summary>Processes a designation string by removing parenthetical content, trimming whitespace, and replacing spaces with plus signs.</summary>
	/// <param name="input">The input designation string to process.</param>
	/// <returns>The processed string with parenthetical content removed, trimmed, and spaces replaced by plus signs.</returns>
	/// <remarks>This method is useful for preparing designation strings for URL queries. For example, "(449127) 2013 AS15" becomes "2013+AS15".</remarks>
	private static string ProcessDesignationForUrl(string input)
	{
		// Validate input
		if (string.IsNullOrWhiteSpace(value: input))
		{
			return string.Empty;
		}
		// Remove all content within parentheses (including the parentheses)
		string result = Regex.Replace(input: input, pattern: @"\([^)]*\)", replacement: string.Empty);
		// Trim leading and trailing whitespace
		result = result.Trim();
		// Replace all remaining spaces with nothing (remove spaces)
		result = result.Replace(oldValue: " ", newValue: "");
		return result;
	}

	/// <summary>Opens the terminology form with the specified index.</summary>
	/// <param name="index">The index to set active in the terminology form.</param>
	/// <remarks>This method is used to open the terminology form with the specified index.</remarks>
	private void OpenTerminology(uint index)
	{
		// Create a new instance of the TerminologyForm
		using TerminologyForm formTerminology = new();
		// Set the active terminology based on the index
		formTerminology.SelectedElement = index switch
		{
			0 => TerminologyElement.IndexNumber,
			1 => TerminologyElement.ReadableDesignation,
			2 => TerminologyElement.Epoch,
			3 => TerminologyElement.MeanAnomalyAtTheEpoch,
			4 => TerminologyElement.ArgumentOfThePerihelion,
			5 => TerminologyElement.LongitudeOfTheAscendingNode,
			6 => TerminologyElement.InclinationToTheEcliptic,
			7 => TerminologyElement.OrbitalEccentricity,
			8 => TerminologyElement.MeanDailyMotion,
			9 => TerminologyElement.SemiMajorAxis,
			10 => TerminologyElement.AbsoluteMagnitude,
			11 => TerminologyElement.SlopeParameter,
			12 => TerminologyElement.Reference,
			13 => TerminologyElement.NumberOfOppositions,
			14 => TerminologyElement.NumberOfObservations,
			15 => TerminologyElement.ObservationSpan,
			16 => TerminologyElement.RmsResidual,
			17 => TerminologyElement.ComputerName,
			18 => TerminologyElement.Flags,
			19 => TerminologyElement.DateOfLastObservation,
			20 => TerminologyElement.LinearEccentricity,
			21 => TerminologyElement.SemiMinorAxis,
			22 => TerminologyElement.MajorAxis,
			23 => TerminologyElement.MinorAxis,
			24 => TerminologyElement.EccentricAnomaly,
			25 => TerminologyElement.TrueAnomaly,
			26 => TerminologyElement.PerihelionDistance,
			27 => TerminologyElement.AphelionDistance,
			28 => TerminologyElement.LongitudeOfTheDescendingNode,
			29 => TerminologyElement.ArgumentOfTheAphelion,
			30 => TerminologyElement.FocalParameter,
			31 => TerminologyElement.SemiLatusRectum,
			32 => TerminologyElement.LatusRectum,
			33 => TerminologyElement.OrbitalPeriod,
			34 => TerminologyElement.OrbitalArea,
			35 => TerminologyElement.OrbitalPerimeter,
			36 => TerminologyElement.SemiMeanAxis,
			37 => TerminologyElement.MeanAxis,
			38 => TerminologyElement.StandardGravitationalParameter,
			_ => TerminologyElement.IndexNumber,
		};
		// Set the TopMost property to true to keep the form on top of other windows
		formTerminology.TopMost = TopMost;
		// Show the terminology form as a modal dialog
		_ = formTerminology.ShowDialog();
	}

	/// <summary>Opens the table mode form.</summary>
	/// <remarks>This method is used to open the table mode form.</remarks>
	private void OpenTableMode()
	{
		// Create a new instance of the TableModeForm
		using TableModeForm formTableMode = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formTableMode.TopMost = TopMost;
		// Fill the form with the planetoids database
		formTableMode.FillArray(arrTemp: planetoidsDatabase);
		// Show the table mode form as a modal dialog
		_ = formTableMode.ShowDialog();
	}

	/// <summary>Shows the orbital resonances form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis from the UI label and opens the <see cref="OrbitalResonancesOfOneMinorPlanetForm"/>.</remarks>
	private void ShowOrbitalResonances()
	{
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis))
		{
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return;
		}
		using OrbitalResonancesOfOneMinorPlanetForm formOrbitalResonances = new();
		formOrbitalResonances.TopMost = TopMost;
		formOrbitalResonances.SetSemiMajorAxis(semiMajorAxis: semiMajorAxis);
		_ = formOrbitalResonances.ShowDialog();
	}

	/// <summary>Shows the observations form for the current planetoid.</summary>
	/// <remarks>Passes the index data label text to the <see cref="ObservationsForm"/> and shows it as a modal dialog.</remarks>
	private void ShowObservations()
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		using ObservationsForm formObservations = new();
		formObservations.TopMost = TopMost;
		formObservations.SetIndexData(indexData: labelIndexData.Text);
		_ = formObservations.ShowDialog();
	}

	/// <summary>Shows the orbital resonances form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis from the UI label and opens the <see cref="OrbitalResonancesOfOneMinorPlanetForm"/>.</remarks>
	private void ShowOrbitElementsGrouping()
	{
		using OrbitElementsGroupingForm formOrbitElementsGrouping = new(planetoids: planetoidsDatabase);
		formOrbitElementsGrouping.TopMost = TopMost;
		_ = formOrbitElementsGrouping.ShowDialog();
	}

	/// <summary>Shows the asteroid families form.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can display asteroid families.</remarks>
	private void ShowAsteroidFamiliesDetection()
	{
		using AsteroidFamiliesForm formAsteroidFamilies = new(planetoids: planetoidsDatabase);
		formAsteroidFamilies.TopMost = TopMost;
		_ = formAsteroidFamilies.ShowDialog(owner: this);
	}

	/// <summary>Shows the orbital resonances of all minor planets form. Opens the form to find orbital resonances of all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowOrbitalResonancesOfAllMinorPlanets()
	{
		// Create a new instance of the OrbitalResonancesOfAllMinorPlanetsForm
		using OrbitalResonancesOfAllMinorPlanetsForm formOrbitalResonances = new(planetoids: planetoidsDatabase);
		formOrbitalResonances.TopMost = TopMost;
		_ = formOrbitalResonances.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs form for the current planetoid.</summary>
	/// <remarks>Parses the orbital elements from the UI labels and opens the <see cref="MoidsOfOneMinorPlanetForm"/>.</remarks>
	private void ShowMoids()
	{
		// Create a culture-specific format provider for parsing the orbital elements
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		// Parse the orbital elements from the corresponding labels on the form
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return;
		}
		// Parse the eccentricity from the corresponding label on the form
		if (!double.TryParse(s: labelOrbitalEccentricityData.Text, style: NumberStyles.Any, provider: provider, result: out double eccentricity))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse eccentricity: '{labelOrbitalEccentricityData.Text}'");
			ShowErrorMessage(message: $"Could not parse eccentricity value: '{labelOrbitalEccentricityData.Text}'");
			return;
		}
		// Parse the inclination to the ecliptic from the corresponding label on the form
		if (!double.TryParse(s: labelInclinationToTheEclipticData.Text, style: NumberStyles.Any, provider: provider, result: out double inclinationDeg))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse inclination: '{labelInclinationToTheEclipticData.Text}'");
			ShowErrorMessage(message: $"Could not parse inclination value: '{labelInclinationToTheEclipticData.Text}'");
			return;
		}
		// Parse the longitude of the ascending node from the corresponding label on the form
		if (!double.TryParse(s: labelLongitudeOfTheAscendingNodeData.Text, style: NumberStyles.Any, provider: provider, result: out double longitudeAscendingNodeDeg))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse longitude of ascending node: '{labelLongitudeOfTheAscendingNodeData.Text}'");
			ShowErrorMessage(message: $"Could not parse longitude of ascending node value: '{labelLongitudeOfTheAscendingNodeData.Text}'");
			return;
		}
		// Parse the argument of perihelion from the corresponding label on the form
		if (!double.TryParse(s: labelArgumentOfThePerihelionData.Text, style: NumberStyles.Any, provider: provider, result: out double argumentPerihelionDeg))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse argument of perihelion: '{labelArgumentOfThePerihelionData.Text}'");
			ShowErrorMessage(message: $"Could not parse argument of perihelion value: '{labelArgumentOfThePerihelionData.Text}'");
			return;
		}
		// Create a new instance of the MoidsOfOneMinorPlanetForm
		using MoidsOfOneMinorPlanetForm formMoids = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formMoids.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formMoids.SetOrbitalElements(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Show the MOIDs form as a modal dialog
		_ = formMoids.ShowDialog();
	}

	/// <summary>Shows the MOIDs of all minor planets form. Opens the form to find MOIDs of all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowMoidsOfAllMinorPlanets()
	{
		// Create a new instance of the MoidsOfAllMinorPlanetsForm
		using MoidsOfAllMinorPlanetsForm formMoidsOfAll = new(planetoids: planetoidsDatabase);
		formMoidsOfAll.TopMost = TopMost;
		_ = formMoidsOfAll.ShowDialog(owner: this);
	}

	/// <summary>Shows the Tisserand parameters form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis, eccentricity, and inclination from the UI labels and opens the <see cref="TisserandParameterOfOneMinorPlanetForm"/>.</remarks>
	private void ShowTisserandParameters()
	{
		// Create a culture-specific format provider for parsing the orbital elements
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		// Parse the semi-major axis from the corresponding label on the form
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return;
		}
		// Parse the eccentricity from the corresponding label on the form
		if (!double.TryParse(s: labelOrbitalEccentricityData.Text, style: NumberStyles.Any, provider: provider, result: out double eccentricity))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse eccentricity: '{labelOrbitalEccentricityData.Text}'");
			ShowErrorMessage(message: $"Could not parse eccentricity value: '{labelOrbitalEccentricityData.Text}'");
			return;
		}
		// Parse the inclination to the ecliptic from the corresponding label on the form
		if (!double.TryParse(s: labelInclinationToTheEclipticData.Text, style: NumberStyles.Any, provider: provider, result: out double inclinationDeg))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse inclination: '{labelInclinationToTheEclipticData.Text}'");
			ShowErrorMessage(message: $"Could not parse inclination value: '{labelInclinationToTheEclipticData.Text}'");
			return;
		}
		// Create a new instance of the TisserandParameterOfOneMinorPlanetForm
		using TisserandParameterOfOneMinorPlanetForm formTisserand = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formTisserand.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formTisserand.SetOrbitalElements(semiMajorAxis: semiMajorAxis, eccentricity: eccentricity, inclinationDeg: inclinationDeg);
		// Show the Tisserand parameters form as a modal dialog
		_ = formTisserand.ShowDialog();
	}

	/// <summary>Shows the Tisserand parameters of all minor planets form. Opens the form to compute Tisserand parameters for all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowTisserandParametersOfAllMinorPlanets()
	{
		// Create a new instance of the TisserandParameterOfAllMinorPlanetsForm
		using TisserandParameterOfAllMinorPlanetsForm formTisserandOfAll = new(planetoids: planetoidsDatabase);
		formTisserandOfAll.TopMost = TopMost;
		_ = formTisserandOfAll.ShowDialog(owner: this);
	}

	/// <summary>Shows the bulk observations data downloader form. Opens the form to download observation data files for a range of minor planets from the MPC website and save them to disk.</summary>
	/// <remarks>Passes the full planetoids database to the form and pre-populates the minimum (1) and maximum (database record count) spinners.</remarks>
	private void ShowBulkObservationDataDownloader()
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		using BulkObservationsDataDownloaderForm formBulkDownloader = new(planetoids: planetoidsDatabase);
		formBulkDownloader.TopMost = TopMost;
		formBulkDownloader.SetMinimum(minimum: 1);
		formBulkDownloader.SetMaximum(maximum: planetoidsDatabase.Count);
		_ = formBulkDownloader.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs relative to minor planets form. Opens the form to calculate the MOID between two user-selected minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can populate the combo boxes with all available planetoid designations.</remarks>
	private void ShowMoidsRelativeToMinorPlanets()
	{
		// Create a new instance of the MoidsRelativeToMinorPlanetsForm
		using MoidsRelativeToMinorPlanetsForm formMoidsRelative = new(planetoids: planetoidsDatabase);
		formMoidsRelative.TopMost = TopMost;
		_ = formMoidsRelative.ShowDialog(owner: this);
	}

	/// <summary>Shows the application information form.</summary>
	/// <remarks>This method is used to show the application information form.</remarks>
	private void ShowAppInfo()
	{
		// Create a new instance of the AppInfoForm
		using AppInfoForm formAppInfo = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formAppInfo.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formAppInfo.ShowDialog();
	}

	/// <summary>Displays the archive form as a modal dialog, ensuring it remains on top of other windows.</summary>
	/// <remarks>This method creates an instance of the ArchiveMpcorbForm and sets its TopMost property to true, which keeps the form above other application windows. The form is shown modally, meaning the user must interact with it before returning to the main application.</remarks>
	private void ShowArchive()
	{
		// Create a new instance of the ArchiveMpcorbForm
		using ArchiveMpcorbForm formArchive = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formArchive.TopMost = TopMost;
		// Show the archive form as a modal dialog
		_ = formArchive.ShowDialog();
	}

	/// <summary>Displays the archive comparison form as a modal dialog, allowing users to view differences between database archives.</summary>
	/// <remarks>The form is set to remain on top of other windows while it is open, ensuring that users can easily interact with it without losing focus.</remarks>
	private void ShowCompareArchives()
	{
		// Create a new instance of the DatabaseDifferencesForm
		using DatabaseDifferencesForm formDataDifferences = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDataDifferences.TopMost = TopMost;
		// Show the archive form as a modal dialog
		_ = formDataDifferences.ShowDialog();
	}

	/// <summary>Shows the license form.</summary>
	/// <remarks>This method is used to show the license form.</remarks>
	private void ShowLicense()
	{
		// Create a new instance of the LicenseForm
		using LicenseForm formLicense = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formLicense.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formLicense.ShowDialog();
	}

	/// <summary>Shows the records form that scans all orbital elements for maximum or minimum record values.</summary>
	/// <remarks>This method creates the <see cref="RecordsForm"/>, passes a copy of the current planetoid database, and displays the form as a modal dialog.</remarks>
	private void ShowRecords()
	{
		// Create a new instance of the RecordsForm
		using RecordsForm formRecords = new();
		// Pass a copy of the current database to the form
		formRecords.FillArray(arrTemp: planetoidsDatabase);
		// Set the TopMost property to keep the form on top of other windows
		formRecords.TopMost = TopMost;
		// Show the records form as a modal dialog
		_ = formRecords.ShowDialog();
	}

	/// <summary>Shows the records form that scans all orbital elements for maximum or minimum record values.</summary>
	/// <remarks>This method creates the <see cref="RecordsTop10Form"/>, passes a copy of the current planetoid database, and displays the form as a modal dialog.</remarks>
	private void ShowRecordsTop10()
	{
		// Create a new instance of the RecordsTop10Form
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: null);
		// Set the TopMost property to keep the form on top of other windows
		formRecordsTop10.TopMost = TopMost;
		// Show the records form as a modal dialog
		_ = formRecordsTop10.ShowDialog();
	}



	/// <summary>Shows the MPCORB data check form.</summary>
	/// <remarks>This method is used to check the MPCORB data for updates.</remarks>
	private async void ShowMpcorbDatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		//if (!await HasInternetAsync(client: httpClient, url: uriMpcorb.OriginalString))
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the MPCORB data check form
			using CheckDatabaseForm formCheckMpcorbDat = new(url: Settings.Default.systemMpcorbDatUrl, localFilePath: Settings.Default.systemFilenameMpcorb, databaseName: "MPCORB.DAT");
			// Set the TopMost property to true to keep the form on top of other windows
			formCheckMpcorbDat.TopMost = TopMost;
			// Show the MPCORB data check form as a modal dialog
			_ = formCheckMpcorbDat.ShowDialog();
		}
	}

	/// <summary>Shows the ASTORB data check form.</summary>
	/// <remarks>This method is used to check the ASTORB data for updates.</remarks>
	private void ShowAstorbDatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the ASTORB data check form
			using CheckDatabaseForm formCheckAstorbDat = new(url: Settings.Default.systemAstorbDatUrl, localFilePath: Settings.Default.systemFilenameAstorb, databaseName: "ASTORB.DAT");
			// Set the TopMost property to true to keep the form on top of other windows
			formCheckAstorbDat.TopMost = TopMost;
			// Show the ASTORB data check form as a modal dialog
			_ = formCheckAstorbDat.ShowDialog();
		}
	}

	/// <summary>Shows the downloader form for the MPCORB database.</summary>
	/// <remarks>This method is used to show the downloader form for the MPCORB database.</remarks>
	private void ShowMpcorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the MPCORB database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemMpcorbDatGzUrl);
			// Set the TopMost property to true to keep the form on top of other windows
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog
			if (downloaderForm.ShowDialog() == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = false;
				toolStripStatusLabelMpcorbDatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the downloader form for the ASTORB database.</summary>
	/// <remarks>This method is used to show the downloader form for the ASTORB database.</remarks>
	private void ShowAstorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the ASTORB database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemAstorbDatGzUrl);
			// Set the TopMost property to true to keep the form on top of other windows
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog
			if (downloaderForm.ShowDialog() == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = false;
				toolStripStatusLabelAstorbDatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the database information form.</summary>
	/// <remarks>This method is used to show the database information form.</remarks>
	private void ShowDatabaseInformation()
	{
		// Create a new instance of the DatabaseInformationForm
		using DatabaseInformationForm formDatabaseInformation = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDatabaseInformation.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formDatabaseInformation.ShowDialog();
	}

	/// <summary>Shows the search form.</summary>
	///	<remarks>This method is used to show the search form.</remarks>
	private void ShowSearch()
	{
		// Create a new instance of the SearchForm
		using SearchForm formSearch = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formSearch.TopMost = TopMost;

		_ = formSearch.ShowDialog();

		/*
		// Fill the form with the planetoids database
		formSearch.FillArray(arrTemp: planetoidsDatabase);
		// Set the maximum index for the search form
		formSearch.SetMaxIndex(maxIndex: planetoidsDatabase.Count);
		// Show the search form as a modal dialog
		_ = formSearch.ShowDialog();
		// Check if the dialog result is OK and the selected index is greater than 0
		_ = KryptonMessageBox.Show(text: formSearch.GetSelectedIndex().ToString());
		// If so, navigate to the current position in the database
		if (formSearch.DialogResult == DialogResult.OK && formSearch.GetSelectedIndex() > 0)
		{
			// Navigate to the current position in the database
			GotoCurrentPosition(position: formSearch.GetSelectedIndex());
		}
		*/
	}

	/// <summary>Shows the filter form.</summary>
	/// <remarks>This method is used to show the filter form.</remarks>
	private void ShowFilter()
	{
		// Create a new instance of the FilterForm
		using FilterForm formFilter = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formFilter.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formFilter.ShowDialog();
	}

	/// <summary>Shows the settings form.</summary>
	/// <remarks>This method is used to show the settings form.</remarks>
	private void ShowSettings()
	{
		// Create a new instance of the SettingsForm
		using SettingsForm formSettings = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formSettings.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formSettings.ShowDialog();
	}

	/// <summary>Lists readable designations.</summary>
	/// <remarks>This method is used to show the list of readable designations.</remarks>
	private void ListReadableDesignations()
	{
		// Create a new instance of the ListReadableDesignationsForm
		using ListReadableDesignationsForm formListReadableDesignations = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formListReadableDesignations.TopMost = TopMost;
		// Fill the form with the planetoids database
		formListReadableDesignations.FillArray(arrTemp: planetoidsDatabase);
		// Set the maximum index for the form
		formListReadableDesignations.SetMaxIndex(maxIndex: planetoidsDatabase.Count);
		// Show the list readable designations form as a modal dialog
		_ = formListReadableDesignations.ShowDialog();
		// Check if the dialog result is OK and the selected index is greater than 0
		if (formListReadableDesignations.DialogResult == DialogResult.OK && formListReadableDesignations.GetSelectedIndex() > 0)
		{
			// Navigate to the current position in the database
			GotoCurrentPosition(position: formListReadableDesignations.GetSelectedIndex());
		}
	}

	/// <summary>Exports the data sheet.</summary>
	///	<remarks>This method is used to export the data sheet.</remarks>
	private void ExportDataSheet()
	{
		// Create a new list to store the orbital elements
		List<string> orbitalElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentAphelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		orbitalElements.Add(item: labelIndexData.Text);
		orbitalElements.Add(item: labelReadableDesignationData.Text);
		orbitalElements.Add(item: labelEpochData.Text);
		orbitalElements.Add(item: labelMeanAnomalyAtTheEpochData.Text);
		orbitalElements.Add(item: labelArgumentOfThePerihelionData.Text);
		orbitalElements.Add(item: labelLongitudeOfTheAscendingNodeData.Text);
		orbitalElements.Add(item: labelInclinationToTheEclipticData.Text);
		orbitalElements.Add(item: labelOrbitalEccentricityData.Text);
		orbitalElements.Add(item: labelMeanDailyMotionData.Text);
		orbitalElements.Add(item: labelSemiMajorAxisData.Text);
		orbitalElements.Add(item: labelAbsoluteMagnitudeData.Text);
		orbitalElements.Add(item: labelSlopeParameterData.Text);
		orbitalElements.Add(item: labelReferenceData.Text);
		orbitalElements.Add(item: labelNumberOfOppositionsData.Text);
		orbitalElements.Add(item: labelNumberOfObservationsData.Text);
		orbitalElements.Add(item: labelObservationSpanData.Text);
		orbitalElements.Add(item: labelRmsResidualData.Text);
		orbitalElements.Add(item: labelComputerNameData.Text);
		orbitalElements.Add(item: labelFlagsData.Text);
		orbitalElements.Add(item: labelDateLastObservationData.Text);
		orbitalElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		// Create a new instance of the ExportDataSheetForm
		using ExportDataSheetForm formExportDataSheet = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formExportDataSheet.TopMost = TopMost;
		// Fill the form with the orbital elements
		formExportDataSheet.SetDatabase(list: [.. orbitalElements.Cast<string>()]);
		// Show the export data sheet form as a modal dialog
		_ = formExportDataSheet.ShowDialog();
	}

	/// <summary>Shows the print data sheet form.</summary>
	/// <remarks>This method is used to show the print data sheet form.</remarks>
	private void PrintDataSheet()
	{
		// Create a new list to store the orbital elements
		List<string> orbitalElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentAphelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		orbitalElements.Add(item: labelIndexData.Text);
		orbitalElements.Add(item: labelReadableDesignationData.Text);
		orbitalElements.Add(item: labelEpochData.Text);
		orbitalElements.Add(item: labelMeanAnomalyAtTheEpochData.Text);
		orbitalElements.Add(item: labelArgumentOfThePerihelionData.Text);
		orbitalElements.Add(item: labelLongitudeOfTheAscendingNodeData.Text);
		orbitalElements.Add(item: labelInclinationToTheEclipticData.Text);
		orbitalElements.Add(item: labelOrbitalEccentricityData.Text);
		orbitalElements.Add(item: labelMeanDailyMotionData.Text);
		orbitalElements.Add(item: labelSemiMajorAxisData.Text);
		orbitalElements.Add(item: labelAbsoluteMagnitudeData.Text);
		orbitalElements.Add(item: labelSlopeParameterData.Text);
		orbitalElements.Add(item: labelReferenceData.Text);
		orbitalElements.Add(item: labelNumberOfOppositionsData.Text);
		orbitalElements.Add(item: labelNumberOfObservationsData.Text);
		orbitalElements.Add(item: labelObservationSpanData.Text);
		orbitalElements.Add(item: labelRmsResidualData.Text);
		orbitalElements.Add(item: labelComputerNameData.Text);
		orbitalElements.Add(item: labelFlagsData.Text);
		orbitalElements.Add(item: labelDateLastObservationData.Text);
		orbitalElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));

		// Create a new instance of the PrintDataSheetForm
		using PrintDataSheetForm formPrintDataSheet = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formPrintDataSheet.TopMost = TopMost;
		// Fill the form with the planetoids database
		formPrintDataSheet.SetDatabase(db: [.. orbitalElements]);
		_ = formPrintDataSheet.ShowDialog();
	}

	/// <summary>Shows the derived orbit elements form.</summary>
	/// <remarks>This method is used to show the derived orbit elements form.</remarks>
	private void ShowDerivedOrbitElements()
	{
		// Create a new list to store the derived orbit elements
		List<string> derivedOrbitElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentPerihelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		double inclination = double.Parse(s: labelInclinationToTheEclipticData.Text, provider: provider);
		double absoluteMagnitude = double.Parse(s: labelAbsoluteMagnitudeData.Text, provider: provider);

		// Calculate true anomaly for velocity and energy calculations
		double trueAnomaly = DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8);

		// Original 19 elements
		derivedOrbitElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		derivedOrbitElements.Add(item: trueAnomaly.ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentPerihelion).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));

		// New 22 elements
		derivedOrbitElements.Add(item: DerivedElements.CalculateDirectrix(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePerihelionVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateAphelionVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanOrbitalVelocity(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateCurrentOrbitalVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateRadialVelocityComponent(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTangentialVelocityComponent(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSpecificOrbitalEnergy(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateVisVivaEnergy(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLongitudeOfPerihelion(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanLongitude(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion, meanAnomaly: meanAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateArgumentOfLatitude(argumentPerihelion: argumentPerihelion, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateFlightPathAngle(numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeSincePerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeToNextPerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeSinceAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeToNextAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSynodicPeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTisserandParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanDistanceFromFocus(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		// Assume standard albedo of 0.154 for C-type asteroids if not specified
		derivedOrbitElements.Add(item: DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: absoluteMagnitude, geometricAlbedo: 0.154).ToString(provider: provider));

		// Create a new instance of the DerivedOrbitElementsForm
		using DerivedOrbitElementsForm formDerivedOrbitElements = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDerivedOrbitElements.TopMost = TopMost;
		// Fill the form with the derived orbit elements
		formDerivedOrbitElements.SetDatabase(list: [.. derivedOrbitElements.Cast<object>()]);
		// Show the derived orbit elements form as a modal dialog
		_ = formDerivedOrbitElements.ShowDialog();
	}

	/// <summary>Checks if the form should stay on top of other windows.</summary>
	/// <remarks>This method is used to check if the form should stay on top of other windows.</remarks>
	private void CheckStayOnTop() => TopMost = toolStripMenuItemOptionStayOnTop.Checked;

	/// <summary>Displays the form's <see cref="openFileDialog"/> to allow the user to choose a local MPCORB.DAT file and restarts the application to load the selected file if confirmed.</summary>
	/// <remarks>Uses the pre-configured <see cref="openFileDialog"/> component. If the user selects a valid, non-empty file, the application prompts for confirmation and restarts with the selected file as a command-line argument. If the file is invalid or empty, an error message is shown and the operation is aborted. This method is intended for scenarios where the user needs to manually specify a new MPCORB.DAT data source.</remarks>
	private void OpenLocalMpcorbDat()
	{
		// Show the dialog and check if the user selected a file
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Get the selected file path
		string selectedFilePath = openFileDialog.FileName;
		// Validate the selected file
		if (string.IsNullOrWhiteSpace(value: selectedFilePath) || !File.Exists(path: selectedFilePath))
		{
			logger.Error(message: $"Selected file does not exist: {selectedFilePath}");
			ShowErrorMessage(message: "The selected file does not exist.");
			return;
		}
		// Check if the file has content
		FileInfo fileInfo = new(fileName: selectedFilePath);
		if (fileInfo.Length == 0)
		{
			logger.Error(message: $"Selected file is empty: {selectedFilePath}");
			ShowErrorMessage(message: "The selected file is empty.");
			return;
		}
		// If the file is valid, prompt the user to confirm restarting the application to load the new file
		try
		{
			logger.Info(message: $"User selected local MPCORB.DAT file: {selectedFilePath}");
			// Ask the user if they want to restart the application
			DialogResult result = KryptonMessageBox.Show(
				text: $"The application will restart to load the selected file:\n\n{selectedFilePath}\n\nDo you want to continue?",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.YesNo,
				icon: KryptonMessageBoxIcon.Question,
				defaultButton: KryptonMessageBoxDefaultButton.Button1);
			// If the user confirms, restart the application with the new file path as a command line argument
			if (result == DialogResult.Yes)
			{
				logger.Info(message: "Restarting application to load new MPCORB.DAT file");
				// Restart the application with the new file path as command line argument
				ProcessStartInfo startInfo = new()
				{
					FileName = Application.ExecutablePath,
					Arguments = $"\"{selectedFilePath}\"",
					UseShellExecute = true
				};
				_ = Process.Start(startInfo: startInfo);
				// Close the current application instance
				Application.Exit();
			}
		}
		// Handle any exceptions that may occur during the file selection and application restart process
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error while opening local MPCORB.DAT file: {ex.Message}");
			ShowErrorMessage(message: $"Error while opening the file:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes the 4-hexdigit flag from MPCORB.DAT and displays the result in a KryptonMessageBox.</summary>
	/// <remarks>The flag encodes orbit type in the lower 6 bits and additional information in bits 6-15 according to MPC specifications.</remarks>
	private void DecodeMpcorbFlags()
	{
		// Get the flag text from the label
		string flagText = labelFlagsData.Text;
		// Validate that the flag text is not empty
		if (string.IsNullOrWhiteSpace(value: flagText))
		{
			logger.Warn(message: "Flag text is empty or whitespace");
			_ = KryptonMessageBox.Show(
				text: "No flag data available.",
				caption: "Flag Decoder",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Validate that the flag text is a valid 4-hexdigit string
		try
		{
			// Parse the hex string to an integer
			int flagValue = Convert.ToInt32(value: flagText, fromBase: 16);
			// Extract orbit type (lower 6 bits)
			int orbitType = flagValue & 0x3F; // 0x3F = 0011 1111 (bits 0-5)
											  // Extract individual flag bits
			bool isNeo = (flagValue & 2048) != 0;          // Bit 11
			bool isLargeNeo = (flagValue & 4096) != 0;     // Bit 12
			bool isOneOppObject = (flagValue & 8192) != 0; // Bit 13
			bool isCriticalList = (flagValue & 16384) != 0;// Bit 14
			bool isPha = (flagValue & 32768) != 0;         // Bit 15
														   // Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: $"MPCORB Flag Decoder");
			_ = result.AppendLine(value: $"==================");
			_ = result.AppendLine(value: $"Hex Value: {flagText}");
			_ = result.AppendLine(value: $"Decimal Value: {flagValue}");
			_ = result.AppendLine();
			// Orbit type classification
			_ = result.AppendLine(value: "Orbit Classification:");
			string orbitTypeName = orbitType switch
			{
				1 => "Atira",
				2 => "Aten",
				3 => "Apollo",
				4 => "Amor",
				5 => "Object with q < 1.665 AU",
				6 => "Hungaria",
				7 => "Unused or internal MPC use only",
				8 => "Hilda",
				9 => "Jupiter Trojan",
				10 => "Distant object",
				_ => $"Undefined (value: {orbitType})"
			};
			_ = result.AppendLine(value: $"  {orbitTypeName}");
			_ = result.AppendLine();
			// Additional flags
			_ = result.AppendLine(value: "Additional Flags:");
			if (isNeo)
			{
				_ = result.AppendLine(value: "  ✓ Near-Earth Object (NEO)");
			}
			if (isLargeNeo)
			{
				_ = result.AppendLine(value: "  ✓ 1-km (or larger) NEO");
			}
			if (isOneOppObject)
			{
				_ = result.AppendLine(value: "  ✓ 1-opposition object seen at earlier opposition");
			}
			if (isCriticalList)
			{
				_ = result.AppendLine(value: "  ✓ Critical list numbered object");
			}
			if (isPha)
			{
				_ = result.AppendLine(value: "  ✓ Potentially Hazardous Asteroid (PHA)");
			}
			// If no additional flags are set
			if (!isNeo && !isLargeNeo && !isOneOppObject && !isCriticalList && !isPha)
			{
				_ = result.AppendLine(value: "  (none)");
			}
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(text: result.ToString(), caption: "MPCORB Flag Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);

			logger.Info(message: $"Decoded MPCORB flag: {flagText} = {flagValue} ({orbitTypeName})");
		}
		// Handle format exceptions when parsing the hex string
		catch (FormatException ex)
		{
			logger.Error(exception: ex, message: $"Failed to parse flag value '{flagText}': {ex.Message}");
			ShowErrorMessage(message: $"Failed to parse flag value '{flagText}'.\n\nThe flag must be a valid hexadecimal number.\n\nError: {ex.Message}");
		}
		// Handle overflow exceptions when the hex value is too large to fit in an integer
		catch (OverflowException ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB flag: {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the flag.\n\nError: {ex.Message}");
		}
	}

	/// <summary>Decodes the compressed reference code from MPCORB.DAT and displays the full reference in a KryptonMessageBox.</summary>
	/// <remarks>Decodes various reference formats according to MPC specifications at http://www.minorplanetcenter.org/iau/info/References.html</remarks>
	private void DecodeMpcorbReference()
	{
		// Get the reference text from the label
		string referenceText = labelReferenceData.Text;
		// Validate that the reference text is not empty
		if (string.IsNullOrWhiteSpace(value: referenceText))
		{
			logger.Warn(message: "Reference text is empty or whitespace");
			_ = KryptonMessageBox.Show(text: "No reference data available.", caption: "Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the reference and handle any exceptions that may occur during decoding
		try
		{
			string decodedReference = DecodeReference(compressedRef: referenceText.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "MPCORB Reference Decoder");
			_ = result.AppendLine(value: "========================");
			_ = result.AppendLine(value: $"Compressed: {referenceText}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Full Reference:");
			_ = result.AppendLine(value: $"  {decodedReference}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(text: result.ToString(), caption: "MPCORB Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded MPCORB reference: '{referenceText}' → '{decodedReference}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB reference '{referenceText}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the reference:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes a compressed MPC reference string to its full form.</summary>
	/// <param name="compressedRef">The compressed reference string (typically 5 characters).</param>
	/// <returns>The full reference description.</returns>
	private static string DecodeReference(string compressedRef)
	{
		if (string.IsNullOrWhiteSpace(value: compressedRef))
		{
			return "Unknown reference";
		}
		// Ensure the reference is exactly 5 characters for proper parsing
		compressedRef = compressedRef.PadRight(totalWidth: 5);
		char firstChar = compressedRef[index: 0];
		// 1: Temporary MPEC References (E + half-month + number)
		if (firstChar == 'E')
		{
			string halfMonth = compressedRef.Substring(startIndex: 1, length: 1);
			string circularNumber = compressedRef.Substring(startIndex: 2, length: 3).TrimStart(trimChar: '0');
			return $"MPEC (temporary) - Half-month {halfMonth}, Circular {circularNumber}";
		}
		// 2A: Five-digit MPC numbers (00001-99999)
		if (char.IsDigit(c: firstChar) && compressedRef.All(predicate: c => char.IsDigit(c: c) || char.IsWhiteSpace(c: c)))
		{
			if (int.TryParse(s: compressedRef.Trim(), result: out int mpcNumber))
			{
				return $"Minor Planet Circular (MPC) {mpcNumber}";
			}
		}
		// 2B: @ + four digits (MPC 100000-109999)
		if (firstChar == '@')
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int excess))
			{
				return $"Minor Planet Circular (MPC) {100000 + excess}";
			}
		}
		// 2C: # + four Base-62 characters (MPC 110000+)
		if (firstChar == '#')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Circular (MPC) {110000 + value}";
		}
		// 2D: Lowercase letter + four digits (MPS)
		if (char.IsLower(c: firstChar))
		{
			int multiplier = firstChar - 'a';
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int remainder))
			{
				int mpsNumber = (multiplier * 10000) + remainder;
				return $"Minor Planet Supplement (MPS) {mpsNumber}";
			}
		}
		// 2E: Tilde + four Base-62 characters (MPS 260000+)
		if (firstChar == '~')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Supplement (MPS) {260000 + value}";
		}
		// 2F: Single uppercase letter + four digits (various journals)
		if (char.IsUpper(c: firstChar) && compressedRef.Length >= 2 && char.IsDigit(c: compressedRef[index: 1]))
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int number))
			{
				return firstChar switch
				{
					'H' => $"Harvard Announcement Card (HAC) {number}",
					'I' => $"IAU Circular (IAUC) {number}",
					'M' => $"Minor Planet Circular (MPC) {number}",
					'R' => $"Planetenzirkular des Astronomischen Rechen-Institut (RI) {number}",
					_ => $"Journal '{firstChar}' #{number}"
				};
			}
		}
		// 2G: Two or more letters (various journals)
		if (compressedRef.Length >= 2)
		{
			string journalCode = compressedRef[..2].Trim();
			string remainder = compressedRef.Length > 2 ? compressedRef[2..].Trim() : "";
			// Attempt to get the journal name from the code
			string journalName = GetJournalName(code: journalCode);
			if (!string.IsNullOrEmpty(value: journalName))
			{
				return !string.IsNullOrEmpty(value: remainder) && int.TryParse(s: remainder, result: out int volOrCirc)
					? $"{journalName}, Vol./Circ. {volOrCirc}"
					: journalName;
			}
		}
		// If no known format matches, return the original compressed reference with a note
		return $"Unknown reference format: {compressedRef.Trim()}";
	}

	/// <summary>Decodes a Base-62 encoded string to an integer.</summary>
	/// <param name="encoded">The Base-62 encoded string.</param>
	/// <returns>The decoded integer value.</returns>
	/// <remarks>Uses characters 0-9, A-Z, a-z to represent digits 0-61.</remarks>
	private static int DecodeBase62(string encoded)
	{
		// Define the character set for Base-62 encoding
		const string base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		int result = 0;
		// Process each character in the encoded string
		foreach (char c in encoded)
		{
			// Find the index of the character in the Base-62 character set
			int digit = base62Chars.IndexOf(value: c);
			if (digit == -1)
			{
				// If the character is not found in the Base-62 set, throw a format exception
				throw new FormatException(message: $"Invalid Base-62 character: {c}");
			}
			result = (result * 62) + digit;
		}
		// Return the decoded integer value
		return result;
	}

	/// <summary>Gets the full journal name from a two-letter journal code.</summary>
	/// <param name="code">The two-letter journal code.</param>
	/// <returns>The full journal name, or an empty string if not found.</returns>
	private static string GetJournalName(string code) => code switch
	{
		"AA" => "Astronomy and Astrophysics",
		"AB" => "Bulletin des Astrophysikalischen Observatoriums Abastumani",
		"AC" => "Astronomisches Zirkular der Akademie der Wissenschaften der UdSSR",
		"AE" => "Astronomical Papers prepared for the use of the American Ephemeris and Nautical Almanac",
		"AJ" => "Astronomical Journal",
		"AN" => "Astronomische Nachrichten",
		"AP" => "Astrophysical Journal Supplement",
		"As" => "Astronomy and Astrophysics Supplement",
		"BA" => "Bulletin Astronomique",
		"BB" => "Bulletin Astronomique de l'Observatoire Royal de Belgique, Uccle",
		"BC" => "Bulletin of the Astronomical Institutes of Czechoslovakia",
		"BG" => "Bulletin de l'Observatoire Astronomique de Beograd",
		"BN" => "Bulletin of the Astronomical Institutes of the Netherlands",
		"BP" => "Bulletin de la Societe des amis des sciences et des lettres de Poznan",
		"BZ" => "Beobachtungs-Zirkulare der Astronomischen Nachrichten",
		"CB" => "Comet Bulletin of the Orient Astronomical Association",
		"CC" => "Observatorio Astronomico de Cordoba, Serie Contribuciones",
		"CD" => "Tsirkulyari Rasadkhonai Stalinobod",
		"CK" => "Izvestiya Krymskoj Astrofizicheskoj Observatorii",
		"CM" => "Circulaire de l'Observatoire de Marseille",
		"CO" => "Odesskij Gosudarstvennyj Universitet Izvestiya Astronomicheskoj Observattorii",
		"CR" => "Comptes Rendus hebdomadaires de l'academie des sciences de Paris",
		"CS" => "Soobshcheniya Gosudarstvennogo Astronomicheskogo Instituta imeni P. K. Shternberga",
		"GO" => "Greenwich Observations",
		"HA" => "Harvard Annal",
		"HD" => "Veröffentlichungen der Landessternwarte Heidelberg",
		"HTCDR" => "Hipparcos-Tycho CD-ROM",
		"IHW" => "International Halley Watch CD-ROM",
		"Ic" => "Icarus",
		"JB" => "Journal of the British Astronomical Association",
		"JC" => "Japan Astronomical Study Association Circular",
		"JO" => "Journal des Observateurs",
		"KB" => "Bulletin of the Kwasan Observatory, Kyoto",
		"KK" => "Kiev Komet Tsirkular",
		"LB" => "Lick Observatory Bulletin",
		"LO" => "Lowell Observatory Bulletin",
		"LP" => "Publicaciones Observatorio Astronomico de La Plata",
		"MN" => "Monthly Notices of the Royal Astronomical Society",
		"NA" => "Annales de l'Observatoire de Nice",
		"NC" => "Nihondaira Observatory Circular",
		"NO" => "Publications of the U.S. Naval Observatory, Second Series",
		"NZ" => "Nachrichtenblatt der Astronomischen Zentralstelle",
		"OB" => "The Observatory",
		"PA" => "Publications of the Astronomical Society of the Pacific",
		"PC" => "Poulkovo Observatory Circular",
		"PD" => "Tartu Astronoomia Observatooriumi Publikatsioonid",
		"PK" => "Pyublikatsii Kievskoj Astronomicheskoj Observatorii",
		"PO" => "Perth Observatory Communication",
		"PP" => "Izvestiya Glavnoj Astronomicheskoj Observatorii v Pulkove",
		"PT" => "Pubblicazioni del Osservatorio di Torino",
		"PZ" => "Zirkular des Astronomischen Hauptobservatoriums Pulkowo",
		"RA" => "Ricerche Astronomiche",
		"RM" => "Memoirs of the Royal Astronomical Society",
		"SA" => "Monthly Notices of the Astronomical Society of Southern Africa",
		"SOB" => "Observatory Bulletin",
		"TB" => "Tokyo Astronomical Bulletin",
		"TC" => "Transval Observatory Circular",
		"TI" => "Astronomia-Optika Institucio, Universitato de Turku, Informo",
		"UC" => "Circular of the Union Observatory, Johannesburg",
		"WO" => "Astronomical Observations of the U.S. Naval Observatory, Washington",
		"WiA" => "Annalen der Sternwarte der Universität Wien",
		"pM" => "Mitteilungen der Nikolai-Hauptsternwarte zu Pulkowo",
		"CMC" => "Carlsberg Meridian Circle Publications",
		"APO" => "Annales de l'Observatoire de Paris: Observations",
		"AS" => "Acta Astronomica Sinica",
		"AZ" => "Astronomicheskij Zhurnal",
		"AcA" => "Acta Astronomica",
		_ => string.Empty
	};

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and its controls.</remarks>
	private void PlanetoidDBForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		// Set the initial text of the MPCORB.DAT tab to indicate that the database is loading
		kryptonPageMpcorbDat.Text = $"MPCORB.DAT ({I18nStrings.DataLoading})";
		// Configure the BackgroundWorker for loading the database
		backgroundWorkerLoadingDatabase.WorkerReportsProgress = true;
		backgroundWorkerLoadingDatabase.WorkerSupportsCancellation = true;
		backgroundWorkerLoadingDatabase.ProgressChanged += BackgroundWorkerLoadingDatabase_ProgressChanged;
		backgroundWorkerLoadingDatabase.RunWorkerCompleted += BackgroundWorkerLoadingDatabase_RunWorkerCompleted;
		backgroundWorkerLoadingDatabase.RunWorkerAsync();
		// Show the splash screen while loading the database
		formSplashScreen.Show();
		// Attempt to get the last modified date of the MPCORB.DAT file and display it in the tab text
		string resolvedMpcOrbDatFilePath = string.IsNullOrWhiteSpace(value: MpcOrbDatFilePath) ? filenameMpcorb : MpcOrbDatFilePath;
		if (!string.IsNullOrWhiteSpace(value: resolvedMpcOrbDatFilePath))
		{
			// Use a try-catch block to handle potential exceptions when accessing the file information
			try
			{
				// Get the file information for the MPCORB.DAT file
				FileInfo fileInfo = new(fileName: resolvedMpcOrbDatFilePath);
				// Check if the file exists before attempting to access its properties
				if (fileInfo.Exists)
				{
					// Get the last modified date of the file in local time
					DateTime datetimeFileLocal = fileInfo.LastWriteTime;
					kryptonPageMpcorbDat.Text = $"MPCORB.DAT ({datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture)})";
				}
			}
			catch (ArgumentException)
			{
				// Ignore invalid file path and keep the default loading text.
			}
			catch (NotSupportedException)
			{
				// Ignore invalid file path format and keep the default loading text.
			}
			catch (PathTooLongException)
			{
				// Ignore invalid file path length and keep the default loading text.
			}
			catch (UnauthorizedAccessException)
			{
				// Ignore inaccessible file path and keep the default loading text.
			}
		}
	}

	/// <summary>Handles the shown event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the shown event of the form.</remarks>
	private void PlanetoidDBForm_Shown(object sender, EventArgs e)
	{
		// Check if an update is available for the MPCORB database and enable the timer for blinking the update label
		if (IsMpcorbDatUpdateAvailable())
		{
			toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = true;
			toolStripStatusLabelMpcorbDatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			toolStripStatusLabelMpcorbDatUpdate.Enabled = false;
			toolStripStatusLabelMpcorbDatUpdate.Visible = false;
		}
		// Check if an update is available for the ASTORB database and enable the timer for blinking the update label
		if (IsAstorbDatUpdateAvailable())
		{
			toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = true;
			toolStripStatusLabelAstorbDatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			toolStripStatusLabelAstorbDatUpdate.Enabled = false;
			toolStripStatusLabelAstorbDatUpdate.Visible = false;
		}
		// Check if the form should stay on top of other windows
		CheckStayOnTop();
	}

	/// <summary>Handles the FormClosing event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the form closing event.</remarks>
	private void PlanetoidDBForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Check if the file exists before attempting to delete it
		if (File.Exists(path: filenameMpcorbTemp))
		{
			// Delete the temporary file if it exists
			File.Delete(path: filenameMpcorbTemp);
		}
	}

	#endregion

	#region BackgroundWorker event handlers for database loading on start up

	/// <summary>Handles the DoWork event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to load the database in a background thread.</remarks>
	private void BackgroundWorkerLoadingDatabase_DoWork(object sender, DoWorkEventArgs e)
	{
		Enabled = false; // Disable the form while loading the database
		int lineNum = 0; // Variable to store the line number being read
		string filename = !string.IsNullOrEmpty(value: MpcOrbDatFilePath) ? MpcOrbDatFilePath : filenameMpcorb; // Get the file name from the path
		FileInfo fileInfo = new(fileName: filename);
		long fileSize = fileInfo.Length, fileSizeRead = 0; // Get the size of the file in bytes
														   // Open the file stream for reading
		using (FileStream fileStream = new(path: filename, mode: FileMode.Open))
		{
			// Create a new instance of the PlanetoidDatabase class
			StreamReader streamReader = new(stream: fileStream);
			// Show the splash screen
			formSplashScreen.Show();
			while (streamReader.Peek() != -1 && !backgroundWorkerLoadingDatabase.CancellationPending)
			{
				string? readLine = streamReader.ReadLine(); // Variable to store the read line from the file
				if (readLine != null)
				{
					fileSizeRead += readLine.Length;
				}
				// ReSharper disable once PossibleLossOfFraction
				float percent = 100 * fileSizeRead / fileSize; // Variable to store the percentage of the file read
															   // Report progress to the background worker
				formSplashScreen.SetProgressbar(value: (int)percent);
				lineNum++;
				// Check if the line number is greater than or equal to 44
				if ((lineNum >= 44) && (!string.IsNullOrEmpty(value: readLine)))
				{
					// Add the read line to the planetoids database
					planetoidsDatabase.Add(item: readLine);
				}
			}
			fileStream.Close();
			streamReader.Close();
		}
		formSplashScreen.Close();
	}

	/// <summary>Handles the ProgressChanged event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the progress changed event during the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_ProgressChanged(object? sender, ProgressChangedEventArgs e)
	{
		//KryptonMessageBox.Show(text: e.ProgressPercentage.ToString());
		// TODO: Not implemented yet
		_ = KryptonMessageBox.Show(text: "Not implemented yet", caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
	}

	/// <summary>Handles the RunWorkerCompleted event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the completion of the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
	{
		toolStripTextBoxGotoIndex.Text = 1.ToString(); // Set the initial value of the goto index text box
		currentPosition = 0; // Set the current position to the first record
		stepPosition = 100; // Set the step position to 100
		GotoCurrentPosition(position: currentPosition); // Navigate to the current position
		Enabled = true; // Enable the form
	}

	#endregion

	#region Timer event handlers

	/// <summary>Handles the tick event for checking new MPCORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new MPCORB data file.</remarks>
	private void TimerCheckForNewMpcorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

	/// <summary>Handles the tick event for checking new ASTORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new ASTORB data file.</remarks>
	private void TimerCheckForNewAstorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

	#endregion

	#region Clear event handlers

	/// <summary>Clears the checked state of all navigation step menu items.</summary>
	/// <remarks>This method is used to clear the checked state of all navigation step menu items.</remarks>
	private void ToolStripMenuItem_Clear()
	{
		// Clear the checked state of all navigation step menu items
		toolStripMenuItemNavigateStep10.Checked = false;
		toolStripMenuItemNavigateStep100.Checked = false;
		toolStripMenuItemNavigateStep1000.Checked = false;
		toolStripMenuItemNavigateStep10000.Checked = false;
		toolStripMenuItemNavigateStep100000.Checked = false;
	}

	#endregion

	#region KeyPress event handlers

	/// <summary>Handles the KeyPress event for the ToolStripTextBoxGotoIndex. Ensures only numeric input and handles the Enter key to trigger navigation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="KeyPressEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to ensure only numeric input is allowed in the ToolStripTextBoxGotoIndex.</remarks>
	private void GotoIndex_KeyPress(object sender, KeyPressEventArgs e)
	{
		// Check if the pressed key is a control character or a digit
		if (!char.IsControl(c: e.KeyChar) && !char.IsDigit(c: e.KeyChar))
		{
			// If the pressed key is not a digit or control character, suppress the key event
			e.Handled = true;
		}
		// Check if the pressed key is a digit or control character
		if (e.KeyChar == Convert.ToChar(value: Keys.Return, provider: CultureInfo.CurrentCulture))
		{
			// If the Enter key is pressed, trigger the click event for the ToolStripButtonGoToIndex
			GoToIndex_Click(sender: null, e: null);
		}
	}

	#endregion

	#region MouseDown event handlers

	/// <summary>Handles the MouseDown event for controls. Stores the control that triggered the event for future reference.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to store the control that triggered the event for future reference.</remarks>
	protected override void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
			// Store the current tag text of the control
			currentTagText = control.Tag?.ToString() ?? string.Empty;
		}
	}

	#endregion

	#region Click & ButtonClick event Handlers

	/// <summary>Handles the click event for the ToolStripMenuItemArchive. Opens the archive.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the archive.</remarks>
	private void Archive_Click(object sender, EventArgs e) => ShowArchive();

	/// <summary>Handles the click event for the ToolStripButtonGoToIndex. Navigates to the specified index in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void GoToIndex_Click(object? sender, EventArgs? e)
	{
		int pos = 0;
		// Try to parse the index from the ToolStripTextBoxGotoIndex
		try
		{
			// Parse the index from the text box
			pos = int.Parse(s: toolStripTextBoxGotoIndex.Text, provider: CultureInfo.CurrentCulture);
		}
		// Catch any exceptions that occur during parsing
		catch (Exception ex)
		{
			// Log the error message
			logger.Error(message: ex.Message);
			// Show an error message box with the exception message
			ShowErrorMessage(message: $"{nameof(GoToIndex_Click)}  {ex.Message}");
		}
		// If the parsed index is out of range, show an error message
		// Otherwise, navigate to the specified index
		if (pos <= 0 || pos >= planetoidsDatabase.Count + 1)
		{
			// Log the error message
			logger.Error(message: "Index out of range");
			// Show an error message if the index is out of range
			ShowErrorMessage(message: $"{I18nStrings.IndexOutOfRange}");
		}
		else
		{
			// Navigate to the specified index
			currentPosition = pos - 1;
			GotoCurrentPosition(position: currentPosition);
		}
	}

	/// <summary>Handles the click event for the ToolStripButtonTerminology. Opens the terminology form with the specified index.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the terminology form with the specified index.</remarks>
	private void Terminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

	/// <summary>Handles the click event for the ToolStripMenuItem10. Sets the navigation step to 10 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep10_Click(object sender, EventArgs e)
	{
		// Set the step position to 10
		stepPosition = 10;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep10.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem100. Sets the navigation step to 100 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep100_Click(object sender, EventArgs e)
	{
		// Set the step position to 100
		stepPosition = 100;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep100.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem1000. Sets the navigation step to 1000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep1000_Click(object sender, EventArgs e)
	{
		// Set the step position to 1000
		stepPosition = 1000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep1000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem10000. Sets the navigation step to 10000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep10000_Click(object sender, EventArgs e)
	{
		// Set the step position to 10000
		stepPosition = 10000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep10000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem100000. Sets the navigation step to 100000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep100000_Click(object sender, EventArgs e)
	{
		// Set the step position to 100000
		stepPosition = 100000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep100000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItemExit. Closes the application.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to close the application.</remarks>
	private void Exit_Click(object sender, EventArgs e) => Close();

	/// <summary>Handles the click event for the ToolStripMenuItemOpenWebsiteMPC. Opens the Minor Planet Center website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Minor Planet Center website.</remarks>
	private void OpenWebsiteMPC_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemWebsiteMpc);

	/// <summary>Handles the click event for the ToolStripMenuItemOpenMPCORBWebsite. Opens the MPCORB website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the MPCORB website.</remarks>
	private void OpenMpcorbDatWebsite_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemWebsiteMpcorb);

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadMpcorbDat. Shows the downloader form for the MPCORB database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the MPCORB database.</remarks>
	private void DownloadMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadAstorbDat. Shows the downloader form for the ASTORB database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the ASTORB database.</remarks>
	private void DownloadAstorbDat_Click(object sender, EventArgs e) => ShowAstorbDatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemCheckAstorbDat. Shows the ASTORB data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the ASTORB data check form.</remarks>
	private void CheckAstorbDatUpdate_Click(object sender, EventArgs e) => ShowAstorbDatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripButtonCheckMpcorbDatUpdate. Shows the MPCORB data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the MPCORB data check form.</remarks>
	private void CheckMpcorbDatUpdate_Click(object sender, EventArgs e) => ShowMpcorbDatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripButtonAbout. Shows the application information form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the application information form.</remarks>
	private void About_Click(object sender, EventArgs e) => ShowAppInfo();

	/// <summary>Handles the click event for the ToolStripButtonOpenWebsitePDB. Opens the Planetoid Database website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Planetoid Database website.</remarks>
	private void OpenWebsitePDB_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemHomepage);

	/// <summary>Handles the click event for the TableMode button. Opens the table mode form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the table mode form.</remarks>
	private void TableMode_Click(object sender, EventArgs e) => OpenTableMode();

	/// <summary>Handles the click event for the ToolStripButtonDatabaseInformation. Shows the database information form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the database information form.</remarks>
	private void DatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

	/// <summary>Handles the click event for the ToolStripMenuItemPrint. Shows the print data sheet form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the print data sheet form.</remarks>
	private void PrintDataSheet_Click(object sender, EventArgs e) => PrintDataSheet();

	/// <summary>Handles the click event for the ToolStripMenuItemSearch. Shows the search form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the search form.</remarks>
	private void Search_Click(object sender, EventArgs e) => ShowSearch();

	/// <summary>Handles the click event for the ToolStripButtonLoadRandomMinorPlanet. Loads a random minor planet from the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to load a random minor planet from the database.</remarks>
	private void LoadRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheBegin. Navigates to the beginning of the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the beginning of the data.</remarks>
	private void NavigateToTheBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateSomeDataBackward. Navigates backward by a specified step in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate backward by a specified step in the data.</remarks>
	private void NavigateSomeDataBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToThePreviousData. Navigates to the previous data entry.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the previous data entry.</remarks>
	private void NavigateToThePreviousData_Click(object sender, EventArgs e) => NavigateToThePreviousData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheNextData. Navigates to the next data entry.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the next data entry.</remarks>
	private void NavigateToTheNextData_Click(object sender, EventArgs e) => NavigateToTheNextData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateSomeDataForward. Navigates forward by a specified step in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate forward by a specified step in the data.</remarks>
	private void NavigateSomeDataForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheEnd. Navigates to the end of the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the end of the data.</remarks>
	private void NavigateToTheEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

	/// <summary>Handles the click event for the ToolStripMenuItemSettings. Shows the settings form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the settings form.</remarks>
	private void Settings_Click(object sender, EventArgs e) => ShowSettings();

	/// <summary>Handles the click event for the ToolStripButtonFilter. Shows the filter form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the filter form.</remarks>
	private void Filter_Click(object sender, EventArgs e) => ShowFilter();

	/// <summary>Handles the click event for the ToolStripButtonDerivedOrbitElements. Shows the derived orbit elements form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the derived orbit elements form.</remarks>
	private void DerivedOrbitElements_Click(object sender, EventArgs e) => ShowDerivedOrbitElements();

	/// <summary>Handles the click event for the ToolStripMenuItemRestart. Restarts the application.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to restart the application.</remarks>
	private void Restart_Click(object sender, EventArgs e) => Restart();

	/// <summary>Handles the click event for the ToolStripMenuItemStayOnTop. Checks if the form should stay on top of other windows.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check if the form should stay on top of other windows.</remarks>
	private void StayOnTop_Click(object sender, EventArgs e) => CheckStayOnTop();

	/// <summary>Handles the click event for the ToolStripMenuIndexNumberCopyToClipboard_Click. Copies the index number to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the index number to the clipboard.</remarks>
	private void CopyToClipboardIndexNumber_Click(object sender, EventArgs e) => CopyToClipboard(text: labelIndexData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardReadableDesignation. Copies the readable designation to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the readable designation to the clipboard.</remarks>
	private void CopyToClipboardReadableDesignation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReadableDesignationData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardEpoch. Copies the epoch to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the epoch to the clipboard.</remarks>
	private void CopyToClipboardEpoch_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEpochData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardMeanAnomaly. Copies the mean anomaly to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean anomaly to the clipboard.</remarks>
	private void CopyToClipboardMeanAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAnomalyAtTheEpochData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardArgumentOfThePerihelion. Copies the argument of perihelion to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the argument of perihelion to the clipboard.</remarks>
	private void CopyToClipboardArgumentOfThePerihelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentOfThePerihelionData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardLongitudeOfTheAscendingNode. Copies the longitude of the ascending node to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the longitude of the ascending node to the clipboard.</remarks>
	private void CopyToClipboardLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeOfTheAscendingNodeData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardInclinationToTheEcliptic. Copies the inclination to the ecliptic data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the inclination to the ecliptic data to the clipboard.</remarks>
	private void CopyToClipboardInclinationToTheEcliptic_Click(object sender, EventArgs e) => CopyToClipboard(text: labelInclinationToTheEclipticData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardOrbitalEccentricity. Copies the orbital eccentricity data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital eccentricity data to the clipboard.</remarks>
	private void CopyToClipboardOrbitalEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalEccentricityData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardMeanDailyMotion. Copies the mean daily motion data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean daily motion data to the clipboard.</remarks>
	private void CopyToClipboardMeanDailyMotion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanDailyMotionData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardSemiMajorAxis. Copies the semi-major axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-major axis data to the clipboard.</remarks>
	private void CopyToClipboardSemiMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMajorAxisData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardAbsoluteMagnitude. Copies the absolute magnitude data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the absolute magnitude data to the clipboard.</remarks>
	private void CopyToClipboardAbsoluteMagnitude_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAbsoluteMagnitudeData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardSlopeParameter. Copies the slope parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the slope parameter data to the clipboard.</remarks>
	private void CopyToClipboardSlopeParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSlopeParameterData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardReference. Copies the reference data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the reference data to the clipboard.</remarks>
	private void CopyToClipboardReference_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReferenceData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfOppositions. Copies the number of oppositions data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the number of oppositions data to the clipboard.</remarks>
	private void CopyToClipboardNumberOfOppositions_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfOppositionsData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfObservations. Copies the number of observations data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the number of observations data to the clipboard.</remarks>
	private void CopyToClipboardNumberOfObservations_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfObservationsData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardObservationSpan. Copies the observation span data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the observation span data to the clipboard.</remarks>
	private void CopyToClipboardObservationSpan_Click(object sender, EventArgs e) => CopyToClipboard(text: labelObservationSpanData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardRmsResidual. Copies the RMS residual data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the RMS residual data to the clipboard.</remarks>
	private void CopyToClipboardRmsResidual_Click(object sender, EventArgs e) => CopyToClipboard(text: labelRmsResidualData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardComputerName. Copies the computer name data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the computer name data to the clipboard.</remarks>
	private void CopyToClipboardComputerName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelComputerNameData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardDateOfLastObservation. Copies the date of last observation data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the date of last observation data to the clipboard.</remarks>
	private void CopyToClipboardDateOfLastObservation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateLastObservationData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardFlags. Copies the flags data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the flags data to the clipboard.</remarks>
	private void CopyToClipboardFlags_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFlagsData.Text);

	/// <summary>Handles the click event for the ToolStripButtonExport. Exports the data sheet.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the data sheet.</remarks>
	private void Export_Click(object sender, EventArgs e) => ExportDataSheet();

	/// <summary>Handles the button click event for the ToolStripSplitButtonTopTenRecords. Shows the records main form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the records main form.</remarks>
	private void Records_Click(object sender, EventArgs e) => ShowRecords();

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanAnomalyAtTheEpoch. Shows the top ten records form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean anomaly at the epoch.</remarks>
	private void RecordsMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for mean anomaly at the epoch
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Mean anomaly at the epoch");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanAnomalyAtTheEpoch. Shows the top ten records form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the argument of the perihelion.</remarks>
	private void RecordsArgumentOfThePerihelion_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for the argument of the perihelion
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Argument of the perihelion");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsLongitudeOfTheAscendingNode. Shows the top ten records form for the longitude of the ascending node.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the longitude of the ascending node.</remarks>
	private void RecordsLongitudeOfTheAscendingNode_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for the longitude of the ascending node
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Longitude of the ascending node");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsInclination. Shows the top ten records form for inclination.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for inclination.</remarks>
	private void RecordsInclination_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for inclination
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Inclination");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsOrbitalEccentricity. Shows the top ten records form for orbital eccentricity.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for orbital eccentricity.</remarks>
	private void RecordsOrbitalEccentricity_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for orbital eccentricity
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Orbital eccentricity");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanDailyMotion. Shows the top ten records form for mean daily motion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean daily motion.</remarks>
	private void RecordsMeanDailyMotion_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for mean daily motion
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Mean daily motion");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSemiMajorAxis. Shows the top ten records form for semi-major axis.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for semi-major axis.</remarks>
	private void RecordsSemiMajorAxis_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for semi-major axis
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Semi-major axis");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsAbsoluteMagnitude. Shows the top ten records form for absolute magnitude.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for absolute magnitude.</remarks>
	private void RecordsAbsoluteMagnitude_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for absolute magnitude
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Absolute magnitude");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSlopeParameter. Shows the top ten records form for slope parameter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for slope parameter.</remarks>
	private void RecordsSlopeParameter_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for slope parameter
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Slope parameter");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfOppositions. Shows the top ten records form for number of oppositions.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of oppositions.</remarks>
	private void RecordsNumberOfOppositions_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for number of oppositions
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Number of oppositions");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfObservations. Shows the top ten records form for number of observations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of observations.</remarks>
	private void RecordsNumberOfObservations_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for number of observations
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Number of observations");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsRmsResidual. Shows the top ten records form for RMS residual.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for RMS residual.</remarks>
	private void RecordsRmsResidual_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for RMS residual
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "r.m.s. residual");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog();
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionMeanAnomalyAtTheEpoch. Shows the distribution form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for mean anomaly at the epoch.</remarks>
	private void DistributionMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionArgumentOfThePerihelion. Shows the distribution form for argument of perihelion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for argument of perihelion.</remarks>
	private void DistributionArgumentOfThePerihelion_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionLongitudeOfTheAscendingNode. Shows the distribution form for longitude of the ascending node.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for longitude of the ascending node.</remarks>
	private void DistributionLongitudeOfTheAscendingNode_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionInclination. Shows the distribution form for inclination.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for inclination.</remarks>
	private void DistributionInclination_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionOrbitalEccentricity. Shows the distribution form for orbital eccentricity.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for orbital eccentricity.</remarks>
	private void DistributionOrbitalEccentricity_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionMeanDailyMotion. Shows the distribution form for mean daily motion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for mean daily motion.</remarks>
	private void DistributionMeanDailyMotion_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionSemiMajorAxis. Shows the distribution form for semi-major axis.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for semi-major axis.</remarks>
	private void DistributionSemiMajorAxis_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionAbsoluteMagnitude. Shows the distribution form for absolute magnitude.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for absolute magnitude.</remarks>
	private void DistributionAbsoluteMagnitude_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionSlopeParameter. Shows the distribution form for slope parameter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for slope parameter.</remarks>
	private void DistributionSlopeParameter_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionNumberOfOppositions. Shows the distribution form for number of oppositions.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for number of oppositions.</remarks>
	private void DistributionNumberOfOppositions_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionNumberOfObservations. Shows the distribution form for number of observations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for number of observations.</remarks>
	private void DistributionNumberOfObservations_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionObservationSpan. Shows the distribution form for observation span.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for observation span.</remarks>
	private void DistributionObservationSpan_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionRmsResidual. Shows the distribution form for RMS residual.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for RMS residual.</remarks>
	private void DistributionRmsResidual_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemDistributionComputerName. Shows the distribution form for computer name.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for computer name.</remarks>
	private void DistributionComputerName_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the button click event for the ToolStripSplitButtonDistribution. Shows the distribution form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>This method is used to show the distribution form for the selected parameter.</remarks>
	private void Distributions_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		ShowErrorMessage(message: "Not implemented yet");
	}

	/// <summary>Handles the click event for the ToolStripMenuItemListReadableDesignations. Lists readable designations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for the selected parameter.</remarks>
	private void ListReadableDesignations_Click(object sender, EventArgs e) => ListReadableDesignations();

	/// <summary>Handles the click event for the ToolStripButtonLicense. Opens the license.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the license.</remarks>
	private void License_Click(object sender, EventArgs e) => ShowLicense();

	/// <summary>Handles the click event for the Compare Databases menu item and initiates the process to compare database archives.</summary>
	/// <remarks>This method is intended to be used as an event handler for a menu item click event. It delegates the comparison operation to the ShowCompareArchives method.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void CompareDatabases_Click(object sender, EventArgs e) => ShowCompareArchives();

	/// <summary>Handles the click event for the ToolStripMenuItemOrbitalResonances. Shows the orbital resonances form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbital resonances form.</remarks>
	private void OrbitalResonances_Click(object sender, EventArgs e) => ShowOrbitalResonances();

	/// <summary>Handles the Click event of the ToolStripButtonObservations. Shows the observations form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the observations form.</remarks>
	private void Observations_Click(object sender, EventArgs e) => ShowObservations();

	/// <summary>Handles the click event for the ToolStripMenuItemOrbitElementsGrouping. Shows the orbit elements grouping form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbit elements grouping form.</remarks>
	private void OrbitElementsGrouping_Click(object sender, EventArgs e) => ShowOrbitElementsGrouping();

	/// <summary>Handles the click event for the ToolStripMenuItemAsteroidFamiliesDetection. Shows the asteroid families form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the asteroid families form.</remarks>
	private void AsteroidFamiliesDetection_Click(object sender, EventArgs e) => ShowAsteroidFamiliesDetection();

	/// <summary>Handles the click event for the MenuitemOrbitalResonancesOfAllMinorPlanets. Shows the orbital resonances of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbital resonances of all minor planets form.</remarks>
	private void OrbitalResonancesOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowOrbitalResonancesOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemMoids. Shows the MOIDs form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MOIDs form for the currently selected minor planet.</remarks>
	private void Moids_Click(object sender, EventArgs e) => ShowMoids();

	/// <summary>Handles the click event for the ToolStripMenuItemMoidsOfAllMinorPlanets. Shows the MOIDs of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MOIDs of all minor planets form.</remarks>
	private void MoidsOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowMoidsOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemTisserandParameters. Shows the Tisserand parameters form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the Tisserand parameters form for the currently selected minor planet.</remarks>
	private void TisserandParameters_Click(object sender, EventArgs e) => ShowTisserandParameters();

	/// <summary>Handles the click event for the ToolStripMenuItemTisserandParametersOfAllMinorPlanets. Shows the Tisserand parameters of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the Tisserand parameters of all minor planets form.</remarks>
	private void TisserandParametersOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowTisserandParametersOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemBulkObservationDataDownloader_Click. Shows the bulk observations data downloader form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the bulk observations data downloader form.</remarks>
	private void BulkObservationDataDownloader_Click(object sender, EventArgs e) => ShowBulkObservationDataDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemMoidsRelativeToMinorPlanets. Shows the MOIDs relative to minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method opens the form for calculating the MOID between two user-selected minor planets.</remarks>
	private void MoidsRelativeToMinorPlanets_Click(object sender, EventArgs e) => ShowMoidsRelativeToMinorPlanets();

	/// <summary>Handles the click event for the toolbar button that opens a local MPCORB.DAT file. Opens a file dialog to select a local MPCORB.DAT file, and if a valid file is selected, restarts the application with the selected file path as a command-line argument.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method allows the user to select a custom local MPCORB.DAT file instead of using the default one.</remarks>
	private void OpenLocalMpcorbDat_Click(object sender, EventArgs e) => OpenLocalMpcorbDat();

	/// <summary>Handles the Click event for the MPC Database menu item and opens the Minor Planet Center database page for the selected object.</summary>
	/// <remarks>This method constructs a URL to the Minor Planet Center database using the current object's identifier and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageMpcDatabase_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://www.minorplanetcenter.net/db_search/show_object?utf8=%E2%9C%93&object_id=" + labelIndexData.Text;
		OpenWebsite(fileName: dataPageUrl);
	}

	/// <summary>Handles the Click event for the JPL Small-Body Database menu item and opens the corresponding web page in the default browser.</summary>
	/// <remarks>This event handler constructs a URL to the JPL Small-Body Database using the current value of the index label and opens it in the user's default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageJplSmallBodyDatabase_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://ssd.jpl.nasa.gov/tools/sbdb_lookup.html#/?sstr=" + labelIndexData.Text + "&view=OPDA";
		OpenWebsite(fileName: dataPageUrl);
	}

	/// <summary>Handles the Click event for the Lowell Minor Planet Services menu item, opening the corresponding asteroid data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL for the Lowell Observatory's asteroid search page based on the current designation and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageLowellMinorPlanetServices_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://asteroid.lowell.edu/gui/search/" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text);
		OpenWebsite(fileName: dataPageUrl);
	}

	/// <summary>Handles the Click event for the Asteroids Dynamic Site menu item, opening the corresponding asteroid data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL for the selected asteroid using its readable designation and opens the associated data page in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageAsteroidsDynamicSite_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://newton.spacedys.com/astdys/index.php?pc=1.1.0&n=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text);
		OpenWebsite(fileName: dataPageUrl);
	}

	/// <summary>Handles the Click event for the menu item that opens the Near-Earth Objects dynamic site in a web browser.</summary>
	/// <remarks>This method constructs a URL for the Near-Earth Objects dynamic site using the current designation and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageNearEarthObjectsDynamicSite_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://newton.spacedys.com/neodys/index.php?pc=1.1.0&n=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text);
		OpenWebsite(fileName: dataPageUrl);
	}

	/// <summary>Handles the Click event for the Near-Earth Object Coordination Centre menu item, opening the corresponding ESA NEO data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL to the ESA Near-Earth Object Coordination Centre based on the current designation and opens it in the default web browser. Use this handler to provide quick access to detailed asteroid information from the application.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageNearEarthObjectCoordinationCentre_Click(object sender, EventArgs e)
	{
		string dataPageUrl = "https://neo.ssa.esa.int/search-for-asteroids?tab=summary&des=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text);
		OpenWebsite(fileName: dataPageUrl);
	}


	/// <summary>Handles the Click event for the menu item that opens all relevant data pages for the selected object in the default web browser.</summary>
	/// <remarks>This method constructs URLs for multiple astronomical data sources using the current object's identifiers and opens each page in the default web browser. The method is intended to provide quick access to external resources for further information about the selected object.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenAllDataPages_Click(object sender, EventArgs e)
	{
		OpenDataPageMpcDatabase_Click(sender: sender, e: e);
		OpenDataPageJplSmallBodyDatabase_Click(sender: sender, e: e);
		OpenDataPageLowellMinorPlanetServices_Click(sender: sender, e: e);
		OpenDataPageAsteroidsDynamicSite_Click(sender: sender, e: e);
		OpenDataPageNearEarthObjectsDynamicSite_Click(sender: sender, e: e);
		OpenDataPageNearEarthObjectCoordinationCentre_Click(sender: sender, e: e);
	}

	/// <summary>Handles the Click event for the label that displays MPCORB flag data and initiates decoding of the flags.</summary>
	/// <param name="sender">The source of the event, typically the label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method decodes the MPCORB flags when the label is clicked.</remarks>
	private void LabelFlagsData_Click(object sender, EventArgs e) => DecodeMpcorbFlags();

	/// <summary>Handles the Click event for the label that displays MPCORB reference data and initiates decoding of the reference.</summary>
	/// <param name="sender">The source of the event, typically the label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method decodes the MPCORB reference when the label is clicked.</remarks>
	private void LabelReferenceData_Click(object sender, EventArgs e) => DecodeMpcorbReference();

	/// <summary>Handles the Click event of the Observatory Codes button to open the <see cref="ObservatoryCodesForm"/>.</summary>
	/// <param name="sender">The source of the event, typically the Observatory Codes button.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	/// <remarks>Opens the <see cref="ObservatoryCodesForm"/> as a modal dialog to display the list of observatory codes.</remarks>
	private void ObservatoryCodes_Click(object sender, EventArgs e)
	{
		// Open the ObservatoryCodesForm as a modal dialog to display the list of observatory codes. The form is set to be topmost based on the current state of the main form to ensure it appears above other windows.
		using ObservatoryCodesForm formObservatoryCodes = new();
		formObservatoryCodes.TopMost = TopMost;
		_ = formObservatoryCodes.ShowDialog();
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog for the corresponding entry if successful.</remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Try to parse the index from the current tag text
		if (TryParseInt(input: currentTagText, value: out int index, errorMessage: out string errorMessage))
		{
			// Open the terminology dialog for the parsed index
			OpenTerminology(index: (uint)index);
			return;
		}
		// Log the error and show an error message
		logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
	}

	/// <summary>Handles the double-click event to show an Easter egg message.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show an Easter egg message when the user double-clicks on a control.</remarks>
	private void EasterEgg_DoubleClick(object sender, EventArgs e) => KryptonMessageBox.Show(text: I18nStrings.EasterEgg, caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);

	#endregion

	#region Icon set and Options event handlers

	/// <summary>Handles the click event for the Fatcow icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Fatcow.</remarks>
	private void IconSetFatcow_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Fatcow
		_ = KryptonMessageBox.Show(text: "Fatcow icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the Silk icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Silk.</remarks>
	private void IconSetSilk_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Silk
		_ = KryptonMessageBox.Show(text: "Silk icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the Fugue icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Fugue.</remarks>
	private void IconSetFugue_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Fugue
		_ = KryptonMessageBox.Show(text: "Fugue icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for enabling copying by double-clicking option.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method toggles the option to enable copying data by double-clicking on controls.</remarks>
	private void EnableCopyingByDoubleClicking_Click(object sender, EventArgs e)
	{
		// TODO: Implement enable/disable copying by double-clicking
		_ = KryptonMessageBox.Show(text: "Enable copying by double-clicking not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for enabling linking to terminology option.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method toggles the option to enable linking to terminology.</remarks>
	private void EnableLinkingToTerminology_Click(object sender, EventArgs e)
	{
		// TODO: Implement enable/disable linking to terminology
		_ = KryptonMessageBox.Show(text: "Enable linking to terminology not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	#endregion
}