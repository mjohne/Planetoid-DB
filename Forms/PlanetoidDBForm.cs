using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;

using static Planetoid_DB.TerminologyForm;

namespace Planetoid_DB;

/// <summary>
/// Represents a form that displays terminology information.
/// </summary>
/// <remarks>
/// This form is responsible for displaying and managing terminology information within the application.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]

public partial class PlanetoidDbForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used throughout the application to log important events and errors.
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
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This control is used for clipboard operations such as copy and paste.
	/// </remarks>
	private Control? currentControl;

	/// <summary>
	/// Stores the current tag text of the control.
	/// </summary>
	/// <remarks>
	/// This string is used to store the current tag text of the control for clipboard operations.
	/// </remarks>
	private string currentTagText = string.Empty;

	/// <summary>
	/// Stores the current position in the planetoids database and the step position for navigation.
	/// </summary>
	/// <remarks>
	/// This integer is used to store the current position in the planetoids database and the step position for navigation.
	/// </remarks>
	private int currentPosition, stepPosition;

	/// <summary>
	/// Stores the planetoids database.
	/// </summary>
	/// <remarks>
	/// This list is used to store the planetoids database entries.
	/// </remarks>
	private readonly List<string> planetoidsDatabase = [];

	/// <summary>
	/// Splash screen form instance.
	/// </summary>
	/// <remarks>
	/// This form is displayed while the application is loading.
	/// </remarks>
	private readonly SplashScreenForm formSplashScreen = new();

	/// <summary>
	/// Filenames for the MPCORB database.
	/// </summary>
	/// <remarks>
	/// These strings are used to store the filenames for the MPCORB database.
	/// </remarks>
	private readonly string filenameMpcorb = Settings.Default.systemFilenameMpcorb;
	private readonly string filenameMpcorbTemp = Settings.Default.systemFilenameMpcorbTemp;

	/// <summary>
	/// URI for the MPCORB database.
	/// </summary>
	/// <remarks>
	/// This URI is used to access the MPCORB database.
	/// </remarks>
	private readonly Uri uriMpcorb = new(uriString: Settings.Default.systemMpcorbDatGzUrl);

	/// <summary>
	/// Cancellation token source for download operations.
	/// </summary>
	/// <remarks>
	/// This token source is used to cancel ongoing download operations.
	/// </remarks>
	private CancellationTokenSource? downloadCancellationTokenSource;

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

	/// <summary>
	/// Initializes a new instance of the <see cref="PlanetoidDbForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form and sets the version text.
	/// </remarks>
	public PlanetoidDbForm()
	{
		InitializeComponent();
		TextExtra = $"{Assembly.GetExecutingAssembly().GetName().Version}";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PlanetoidDbForm"/> class with a specified MPCORB.DAT file path.
	/// </summary>
	/// <param name="mpcorbDatFilePath">The file path to the MPCORB.DAT file.</param>
	/// <remarks>
	/// This constructor initializes the form and sets the version text.
	/// </remarks>
	public PlanetoidDbForm(string mpcorbDatFilePath)
	{
		// Initialize the form components
		InitializeComponent();
		TextExtra = $"{Assembly.GetExecutingAssembly().GetName().Version}";
		SetStatusBar(text: string.Empty);
		MpcOrbDatFilePath = mpcorbDatFilePath;
	}

	#endregion

	#region helper methods

	/// <summary>
	/// Gets the file path of the MPCORB.DAT file.
	/// </summary>
	/// <remarks>
	/// This property is used to store the file path of the MPCORB.DAT file.
	/// </remarks>
	private string MpcOrbDatFilePath { get; set; } = string.Empty;

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a custom display string for the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to set the status bar text and enable the information label when text is provided.
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
	/// Tries to parse an integer from the input string.
	/// </summary>
	/// <param name="input">The input string to parse.</param>
	/// <param name="value">The parsed integer value if successful.</param>
	/// <param name="errorMessage">An error message if parsing fails.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	/// <remarks>
	/// This method is used to try parsing an integer from the input string.
	/// </remarks>
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

	/// <summary>
	/// Restarts the application.
	/// </summary>
	/// <remarks>
	/// This method is used to restart the application.
	/// </remarks>
	private void Restart()
	{
		// Close the current form and start a new instance of the application
		_ = Process.Start(fileName: Application.ExecutablePath);
		Close();
	}

	/// <summary>
	/// Asks the user if they want to restart the application after downloading the database.
	/// </summary>
	/// <remarks>
	/// This method is used to ask the user if they want to restart the application after downloading the database.
	/// </remarks>
	private void AskForRestartAfterDownloadingDatabase()
	{
		// Ask the user if they want to restart the application after downloading the database
		// and show a message box with the option to restart or not
		// The message box will have the text "Download complete. Do you want to restart the application?"
		// and the caption "Information"
		// If the user clicks "Yes", restart the application
		// If the user clicks "No", do nothing
		if (MessageBox.Show(text: I10nStrings.DownloadCompleteAndRestartQuestionText, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information, defaultButton: MessageBoxDefaultButton.Button1) == DialogResult.Yes)
		{
			// Restart the application
			Restart();
		}
	}

	/// <summary>
	/// Navigates to the specified position in the planetoids database.
	/// </summary>
	/// <param name="position">The position to navigate to.</param>
	/// <remarks>
	/// This method is used to navigate to the specified position in the planetoids database.
	/// </remarks>
	private void GotoCurrentPosition(int position)
	{
		// Handle the case where the database is empty
		if (position < 0 || position >= planetoidsDatabase.Count)
		{
			toolStripLabelIndexPosition.ToolTipText = "Index: 0";
			return;
		}

		// Extract and display the data for the specified position
		// Each field is extracted using substring operations based on fixed positions
		// The extracted strings are trimmed to remove leading and trailing whitespace
		// Example for extracting a field:
		// string teilstring = planetoidsDatabase[position].ToString().Substring(startIndex: 0, length: 7).Trim();
		// int zahl = int.Parse(s: teilstring, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture);
		// The above example extracts a substring from the database entry at the specified position,
		// trims it, and converts it to an integer using invariant culture
		// The same approach is used for all other fields, adjusting the start index and length as needed
		// Note: The field lengths and positions are based on the MPCORB.DAT file format specification
		// The following fields are extracted:
		// Index number (positions 0-6)
		// Absolute magnitude (positions 8-12)
		// Slope parameter (positions 14-18)
		// Epoch (positions 20-24)
		// Mean anomaly at the epoch (positions 26-34)
		// Argument of perihelion (positions 37-45)
		// Longitude of the ascending node (positions 48-56)
		// Inclination to the ecliptic (positions 59-67)
		// Orbital eccentricity (positions 70-78)
		// Mean daily motion (positions 80-90)
		// Semi-major axis (positions 92-102)
		// Reference (positions 107-115)
		// Number of observations (positions 117-121)
		// Number of oppositions (positions 123-125)
		// Observation span (positions 127-135)
		// r.m.s. residual (positions 137-140)
		// Computer name (positions 150-159)
		// Flags (positions 161-164)
		// Readable designation (positions 166-193)
		// Date of last observation (positions 194-201)
		// Note: The substring method uses zero-based indexing
		// The lengths are inclusive of the start index
		// Example: Substring(startIndex: 0, length: 7) extracts characters from index 0 to 6
		// The extracted strings are trimmed to remove any leading or trailing whitespace
		// The extracted values are then displayed in the corresponding labels on the form
		// Example: labelIndexData.Text = extractedIndexString;
		// The above example sets the text of the label to the extracted index string
		// This process is repeated for all other fields
		// Note: Error handling is not included in this method
		// It is assumed that the data in the database is well-formed and follows the expected format
		//Achtung: Wenn später die Teilstrings in Zahlen konvertiert werden, dann muss darauf geachtet werden, dass die eingelesenen Zeichenketten keine Leerstrings sind.
		// if (teilstring == "0") zahl = 0; ...

		toolStripLabelIndexPosition.ToolTipText = $"Index: {currentPosition + 1}/{planetoidsDatabase.Count}";
		object? entry = planetoidsDatabase[index: position];
		labelIndexData.Text = entry?.ToString() is string entryStr ? entryStr[..7].Trim() : string.Empty;
		labelAbsoluteMagnitudeData.Text = entry?.ToString() is string entryStr1 ? entryStr1.Substring(startIndex: 8, length: 5).Trim() : string.Empty;
		labelSlopeParameterData.Text = entry?.ToString() is string entryStr2 ? entryStr2.Substring(startIndex: 14, length: 5).Trim() : string.Empty;
		labelEpochData.Text = entry?.ToString() is string entryStr3 ? entryStr3.Substring(startIndex: 20, length: 5).Trim() : string.Empty;
		labelMeanAnomalyAtTheEpochData.Text = entry?.ToString() is string entryStr4 ? entryStr4.Substring(startIndex: 26, length: 9).Trim() : string.Empty;
		labelArgumentOfPerihelionData.Text = entry?.ToString() is string entryStr5 ? entryStr5.Substring(startIndex: 37, length: 9).Trim() : string.Empty;
		labelLongitudeOfTheAscendingNodeData.Text = entry?.ToString() is string entryStr6 ? entryStr6.Substring(startIndex: 48, length: 9).Trim() : string.Empty;
		labelInclinationToTheEclipticData.Text = entry?.ToString() is string entryStr7 ? entryStr7.Substring(startIndex: 59, length: 9).Trim() : string.Empty;
		labelOrbitalEccentricityData.Text = entry?.ToString() is string entryStr8 ? entryStr8.Substring(startIndex: 70, length: 9).Trim() : string.Empty;
		labelMeanDailyMotionData.Text = entry?.ToString() is string entryStr9 ? entryStr9.Substring(startIndex: 80, length: 11).Trim() : string.Empty;
		labelSemiMajorAxisData.Text = entry?.ToString() is string entryStr10 ? entryStr10.Substring(startIndex: 92, length: 11).Trim() : string.Empty;
		labelReferenceData.Text = entry?.ToString() is string entryStr11 ? entryStr11.Substring(startIndex: 107, length: 9).Trim() : string.Empty;
		labelNumberOfObservationsData.Text = entry?.ToString() is string entryStr12 ? entryStr12.Substring(startIndex: 117, length: 5).Trim() : string.Empty;
		labelNumberOfOppositionsData.Text = entry?.ToString() is string entryStr13 ? entryStr13.Substring(startIndex: 123, length: 3).Trim() : string.Empty;
		labelObservationSpanData.Text = entry?.ToString() is string entryStr14 ? entryStr14.Substring(startIndex: 127, length: 9).Trim() : string.Empty;
		labelRmsResidualData.Text = entry?.ToString() is string entryStr15 ? entryStr15.Substring(startIndex: 137, length: 4).Trim() : string.Empty;
		labelComputerNameData.Text = entry?.ToString() is string entryStr16 ? entryStr16.Substring(startIndex: 150, length: 10).Trim() : string.Empty;
		labelFlagsData.Text = entry?.ToString() is string entryStr17 ? entryStr17.Substring(startIndex: 161, length: 4).Trim() : string.Empty;
		labelReadableDesignationData.Text = entry?.ToString() is string entryStr18 ? entryStr18.Substring(startIndex: 166, length: 28).Trim() : string.Empty;
		labelDateLastObservationData.Text = entry?.ToString() is string entryStr19 ? entryStr19.Substring(startIndex: 194, length: 8).Trim() : string.Empty;
		toolStripLabelIndexPosition.Text = $@"{I10nStrings.Index}: {position + 1:N0} / {planetoidsDatabase.Count:N0}";
		/* Original code:
		labelAbsoluteMagnitudeData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 8, length: 5).Trim();
		labelSlopeParameterData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 14, length: 5).Trim();
		labelEpochData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 20, length: 5).Trim();
		labelMeanAnomalyAtTheEpochData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 26, length: 9).Trim();
		labelArgumentOfPerihelionData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 37, length: 9).Trim();
		labelLongitudeOfTheAscendingNodeData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 48, length: 9).Trim();
		labelInclinationToTheEclipticData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 59, length: 9).Trim();
		labelOrbitalEccentricityData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 70, length: 9).Trim();
		labelMeanDailyMotionData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 80, length: 11).Trim();
		labelSemiMajorAxisData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 92, length: 11).Trim();
		labelReferenceData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 107, length: 9).Trim();
		labelNumberOfObservationsData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 117, length: 5).Trim();
		labelNumberOfOppositionsData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 123, length: 3).Trim();
		labelObservationSpanData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 127, length: 9).Trim();
		labelRmsResidualData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 137, length: 4).Trim();
		labelComputerNameData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 150, length: 10).Trim();
		labelFlagsData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 161, length: 4).Trim();
		labelReadableDesignationData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 166, length: 28).Trim();
		labelDateLastObservationData.Text = planetoidsDatabase[index: position].ToString().Substring(startIndex: 194, length: 8).Trim();
		toolStripLabelIndexPosition.Text = $@"{I10nStrings.Index}: {position + 1:N0} / {planetoidsDatabase.Count:N0}";
		*/
	}

	/// <summary>
	/// Retrieves the last modified date and time (in UTC) of the resource at the specified URI.
	/// </summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>
	/// The <see cref="DateTime"/> representing the last modified date and time in UTC if available; 
	/// otherwise, <see cref="DateTime.MinValue"/>.
	/// </returns>
	/// <remarks>
	/// This method is used to retrieve the last modified date and time of a resource.
	/// </remarks>
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
	/// This method is used to retrieve the content length of a resource.
	/// </remarks>
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
	/// Checks if an update for the MPCORB database is available.
	/// </summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>
	/// This method is used to check if an update for the MPCORB database is available.
	/// </remarks>
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
		// Get the last modified date of the online file

		//string url = uriMpcorb.ToString();
		//DateTime datetimeFileOnline = (await GetLastModifiedAsync(uri: new Uri(uriString: url), client: httpClient)).ToString();
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

	/// <summary>
	/// Loads a random minor planet from the database.
	/// </summary>
	/// <remarks>
	/// This method is used to load a random minor planet from the database.
	/// </remarks>
	private void LoadRandomMinorPlanet() => GotoCurrentPosition(position: currentPosition = new Random().Next(maxValue: planetoidsDatabase.Count + 1));

	/// <summary>
	/// Navigates to the beginning of the data.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate to the beginning of the data.
	/// </remarks>
	private void NavigateToTheBeginOfTheData() => GotoCurrentPosition(position: currentPosition = 0);

	/// <summary>
	/// Navigates backward by a specified step in the data.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate backward by a specified step in the data.
	/// </remarks>
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

	/// <summary>
	/// Navigates to the previous data entry.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate to the previous data entry in the planetoids database.
	/// </remarks>
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

	/// <summary>
	/// Navigates to the next data entry.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate to the next data entry in the planetoids database.
	/// </remarks>
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

	/// <summary>
	/// Navigates forward by a specified step in the data.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate forward by a specified step in the data.
	/// </remarks>
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

	/// <summary>
	/// Navigates to the end of the data.
	/// </summary>
	/// <remarks>
	/// This method is used to navigate to the end of the data.
	/// </remarks>
	private void NavigateToTheEndOfTheData() => GotoCurrentPosition(position: currentPosition = planetoidsDatabase.Count - 1);

	/// <summary>
	/// Opens the terminology form with the specified index.
	/// </summary>
	/// <param name="index">The index to set active in the terminology form.</param>
	/// <remarks>
	/// This method is used to open the terminology form with the specified index.
	/// </remarks>
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

	/// <summary>
	/// Opens the table mode form.
	/// </summary>
	/// <remarks>
	/// This method is used to open the table mode form.
	/// </remarks>
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

	/// <summary>
	/// Shows the application information form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the application information form.
	/// </remarks>
	private void ShowAppInfo()
	{
		// Create a new instance of the AppInfoForm
		using AppInfoForm formAppInfo = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formAppInfo.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formAppInfo.ShowDialog();
	}

	/// <summary>
	/// Shows the license form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the license form.
	/// </remarks>
	private void ShowLicense()
	{
		// Create a new instance of the LicenseForm
		using LicenseForm formLicense = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formLicense.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formLicense.ShowDialog();
	}

	/// <summary>
	/// Shows the records selection form.
	/// </summary>
	///	<remarks>
	///	This method is used to show the records selection form.
	/// </remarks>
	private void ShowRecordsSelection()
	{
		// Create a new instance of the RecordsSelectionForm
		using RecordsSelectionForm formRecordsSelection = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formRecordsSelection.TopMost = TopMost;
		// Show the records selection form as a modal dialog
		_ = formRecordsSelection.ShowDialog();
	}

	/// <summary>
	/// Shows the main records form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the main records form.
	/// </remarks>
	private void ShowRecordsMain()
	{
		// Create a new instance of the RecordsMainForm
		using RecordsMainForm formRecordsMain = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formRecordsMain.TopMost = TopMost;
		// Show the records form as a modal dialog
		_ = formRecordsMain.ShowDialog();
	}

	/// <summary>
	/// Shows the MPCORB data check form.
	/// </summary>
	/// <remarks>
	/// This method is used to check the MPCORB data for updates.
	/// </remarks>
	private async void ShowMpcorbDatCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		//if (!await HasInternetAsync(client: httpClient, url: uriMpcorb.OriginalString))
		{
			// Display an error message if the network is not available
			_ = MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		else
		{
			// Create and show the MPCORB data check form
			using CheckMpcorbDatForm formCheckMpcorbDat = new();
			// Set the TopMost property to true to keep the form on top of other windows
			formCheckMpcorbDat.TopMost = TopMost;
			// Show the MPCORB data check form as a modal dialog
			_ = formCheckMpcorbDat.ShowDialog();
		}
	}

	/// <summary>
	/// Shows the MPCORB data check form.
	/// </summary>
	/// <remarks>
	/// This method is used to check the ASTORB data for updates.
	/// </remarks>
	private void ShowAstorbDatCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			_ = MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		else
		{
			// Create and show the ASTORB data check form
			using CheckAstorbDatForm formCheckAstorbDat = new();
			// Set the TopMost property to true to keep the form on top of other windows
			formCheckAstorbDat.TopMost = TopMost;
			// Show the ASTORB data check form as a modal dialog
			_ = formCheckAstorbDat.ShowDialog();
		}
	}

	/// <summary>
	/// Shows the downloader form for the MPCORB database.
	/// </summary>
	/// <remarks>
	/// This method is used to show the downloader form for the MPCORB database.
	/// </remarks>
	private void ShowMpcorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			_ = MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
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
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>
	/// Shows the downloader form for the ASTORB database.
	/// </summary>
	/// <remarks>
	/// This method is used to show the downloader form for the ASTORB database.
	/// </remarks>
	private void ShowAstorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			_ = MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		else
		{
			// Create and show the downloader form for the MPCORB database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemAstorbDatGzUrl);
			// Set the TopMost property to true to keep the form on top of other windows
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog
			if (downloaderForm.ShowDialog() == DialogResult.OK)
			{
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>
	/// Shows the database information form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the database information form.
	/// </remarks>
	private void ShowDatabaseInformation()
	{
		// Create a new instance of the DatabaseInformationForm
		using DatabaseInformationForm formDatabaseInformation = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDatabaseInformation.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formDatabaseInformation.ShowDialog();
	}

	/// <summary>
	/// Shows the form to copy data to the clipboard.
	/// </summary>
	/// <remarks>
	/// This method is used to show the form for copying data to the clipboard.
	/// </remarks>
	private void ShowCopyDataToClipboard()
	{
		// Create a new list to store the data to copy
		List<string> dataToCopy = [
			labelIndexData.Text,
			labelReadableDesignationData.Text,
			labelEpochData.Text,
			labelMeanAnomalyAtTheEpochData.Text,
			labelArgumentOfPerihelionData.Text,
			labelLongitudeOfTheAscendingNodeData.Text,
			labelInclinationToTheEclipticData.Text,
			labelOrbitalEccentricityData.Text,
			labelMeanDailyMotionData.Text,
			labelSemiMajorAxisData.Text,
			labelAbsoluteMagnitudeData.Text,
			labelSlopeParameterData.Text,
			labelReferenceData.Text,
			labelNumberOfOppositionsData.Text,
			labelNumberOfObservationsData.Text,
			labelObservationSpanData.Text,
			labelRmsResidualData.Text,
			labelComputerNameData.Text,
			labelFlagsData.Text,
			labelDateLastObservationData.Text
		];
		// Create a new list to store the non-empty data items
		List<string> dataToCopyList = dataToCopy.Where(predicate: static item => !string.IsNullOrEmpty(value: item)).ToList();
		// Iterate through each item in the dataToCopy array
		// Create a new instance of the CopyDataToClipboardForm
		using CopyDataToClipboardForm formCopyDataToClipboard = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formCopyDataToClipboard.TopMost = TopMost;
		// Fill the form with the data to copy
		formCopyDataToClipboard.SetDatabase(list: dataToCopyList);
		// Show the copy data to clipboard form as a modal dialog
		_ = formCopyDataToClipboard.ShowDialog();
	}

	/// <summary>
	/// Shows the search form.
	/// </summary>
	///	<remarks>
	///	This method is used to show the search form.
	/// </remarks>
	private void ShowSearch()
	{
		// Create a new instance of the SearchForm
		using SearchForm formSearch = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formSearch.TopMost = TopMost;
		// Fill the form with the planetoids database
		formSearch.FillArray(arrTemp: planetoidsDatabase);
		// Set the maximum index for the search form
		formSearch.SetMaxIndex(maxIndex: planetoidsDatabase.Count);
		// Show the search form as a modal dialog
		_ = formSearch.ShowDialog();
		// Check if the dialog result is OK and the selected index is greater than 0
		_ = MessageBox.Show(text: formSearch.GetSelectedIndex().ToString());
		// If so, navigate to the current position in the database
		if (formSearch.DialogResult == DialogResult.OK && formSearch.GetSelectedIndex() > 0)
		{
			// Navigate to the current position in the database
			GotoCurrentPosition(position: formSearch.GetSelectedIndex());
		}
	}

	/// <summary>
	/// Shows the filter form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the filter form.
	/// </remarks>
	private void ShowFilter()
	{
		// Create a new instance of the FilterForm
		using FilterForm formFilter = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formFilter.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formFilter.ShowDialog();
	}

	/// <summary>
	/// Shows the settings form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the settings form.
	/// </remarks>
	private void ShowSettings()
	{
		// Create a new instance of the SettingsForm
		using SettingsForm formSettings = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formSettings.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formSettings.ShowDialog();
	}

	/// <summary>
	/// Opens the database differences form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the database differences form.
	/// </remarks>
	private void OpenDatabaseDifferences()
	{
		// Create a new instance of the DatabaseDifferencesForm
		using DatabaseDifferencesForm formDatabaseDifferences = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDatabaseDifferences.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formDatabaseDifferences.ShowDialog();
	}

	/// <summary>
	/// Lists readable designations.
	/// </summary>
	/// <remarks>
	/// This method is used to show the list of readable designations.
	/// </remarks>
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

	/// <summary>
	/// Exports the data sheet.
	/// </summary>
	///	<remarks>
	/// This method is used to export the data sheet.
	/// </remarks>
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
		double argumentAphelion = double.Parse(s: labelArgumentOfPerihelionData.Text, provider: provider);
		_ = orbitalElements.Add(value: labelIndexData.Text);
		_ = orbitalElements.Add(value: labelReadableDesignationData.Text);
		_ = orbitalElements.Add(value: labelEpochData.Text);
		_ = orbitalElements.Add(value: labelMeanAnomalyAtTheEpochData.Text);
		_ = orbitalElements.Add(value: labelArgumentOfPerihelionData.Text);
		_ = orbitalElements.Add(value: labelLongitudeOfTheAscendingNodeData.Text);
		_ = orbitalElements.Add(value: labelInclinationToTheEclipticData.Text);
		_ = orbitalElements.Add(value: labelOrbitalEccentricityData.Text);
		_ = orbitalElements.Add(value: labelMeanDailyMotionData.Text);
		_ = orbitalElements.Add(value: labelSemiMajorAxisData.Text);
		_ = orbitalElements.Add(value: labelAbsoluteMagnitudeData.Text);
		_ = orbitalElements.Add(value: labelSlopeParameterData.Text);
		_ = orbitalElements.Add(value: labelReferenceData.Text);
		_ = orbitalElements.Add(value: labelNumberOfOppositionsData.Text);
		_ = orbitalElements.Add(value: labelNumberOfObservationsData.Text);
		_ = orbitalElements.Add(value: labelObservationSpanData.Text);
		_ = orbitalElements.Add(value: labelRmsResidualData.Text);
		_ = orbitalElements.Add(value: labelComputerNameData.Text);
		_ = orbitalElements.Add(value: labelFlagsData.Text);
		_ = orbitalElements.Add(value: labelDateLastObservationData.Text);
		_ = orbitalElements.Add(value: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = orbitalElements.Add(value: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		// Create a new instance of the ExportDataSheetForm
		using ExportDataSheetForm formExportDataSheet = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formExportDataSheet.TopMost = TopMost;
		// Fill the form with the orbital elements
		formExportDataSheet.SetDatabase(list: [.. orbitalElements.Cast<string>()]);
		// Show the export data sheet form as a modal dialog
		_ = formExportDataSheet.ShowDialog();
	}

	/// <summary>
	/// Shows the print data sheet form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the print data sheet form.
	/// </remarks>
	private void PrintDataSheet()
	{
		// Create a new instance of the PrintDataSheetForm
		using PrintDataSheetForm formPrintDataSheet = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formPrintDataSheet.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formPrintDataSheet.ShowDialog();
	}

	/// <summary>
	/// Shows the derived orbit elements form.
	/// </summary>
	/// <remarks>
	/// This method is used to show the derived orbit elements form.
	/// </remarks>
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
		double argumentAphelion = double.Parse(s: labelArgumentOfPerihelionData.Text, provider: provider);
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		_ = derivedOrbitElements.Add(value: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		// Create a new instance of the DerivedOrbitElementsForm
		using DerivedOrbitElementsForm formDerivedOrbitElements = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formDerivedOrbitElements.TopMost = TopMost;
		// Fill the form with the derived orbit elements
		formDerivedOrbitElements.SetDatabase(list: [.. derivedOrbitElements.Cast<object>()]);
		// Show the derived orbit elements form as a modal dialog
		_ = formDerivedOrbitElements.ShowDialog();
	}

	/// <summary>
	/// Checks if the form should stay on top of other windows.
	/// </summary>
	/// <remarks>
	/// This method is used to check if the form should stay on top of other windows.
	/// </remarks>
	private void CheckStayOnTop() => TopMost = menuitemOptionStayOnTop.Checked;

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the PlanetoidDBForm.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form and its controls.
	/// </remarks>
	private void PlanetoidDBForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar();
		backgroundWorkerLoadingDatabase.WorkerReportsProgress = true;
		backgroundWorkerLoadingDatabase.WorkerSupportsCancellation = true;
		backgroundWorkerLoadingDatabase.ProgressChanged += BackgroundWorkerLoadingDatabase_ProgressChanged;
		backgroundWorkerLoadingDatabase.RunWorkerCompleted += BackgroundWorkerLoadingDatabase_RunWorkerCompleted;
		backgroundWorkerLoadingDatabase.RunWorkerAsync();
		formSplashScreen.Show();
	}

	/// <summary>
	/// Handles the shown event of the PlanetoidDBForm.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the shown event of the form.
	/// </remarks>
	private void PlanetoidDBForm_Shown(object sender, EventArgs e)
	{
		// Disable the background download label
		toolStripStatusLabelBackgroundDownload.Enabled = false;
		// Disable the background download progress bar
		toolStripProgressBarBackgroundDownload.Enabled = false;
		// Disable the cancel background download label
		toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
		// Hide the background download label
		toolStripStatusLabelBackgroundDownload.Visible = false;
		// Hide the background download progress bar
		toolStripProgressBarBackgroundDownload.Visible = false;
		// Hide the cancel background download label
		toolStripStatusLabelCancelBackgroundDownload.Visible = false;
		// Check if an update is available for the MPCORB database
		// and enable the timer for blinking the update label
		if (IsMpcorbDatUpdateAvailable())
		{
			timerBlinkForUpdateAvailable.Enabled = true;
			toolStripStatusLabelUpdate.Enabled = true;
		}
		// Otherwise, disable the timer and hide the update label
		else
		{
			timerBlinkForUpdateAvailable.Enabled = false;
			toolStripStatusLabelUpdate.Enabled = false;
			toolStripStatusLabelUpdate.Visible = false;
		}
		// Check if the form should stay on top of other windows
		CheckStayOnTop();
	}

	/// <summary>
	/// Handles the FormClosing event of the PlanetoidDBForm.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the form closing event.
	/// </remarks>
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

	/// <summary>
	/// Handles the DoWork event of the BackgroundWorker for loading the database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to load the database in a background thread.
	/// </remarks>
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
					_ = planetoidsDatabase.Add(value: readLine);
				}
			}
			fileStream.Close();
			streamReader.Close();
		}
		formSplashScreen.Close();
	}

	/// <summary>
	/// Handles the ProgressChanged event of the BackgroundWorker for loading the database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the progress changed event during the database loading process.
	/// </remarks>
	private static void BackgroundWorkerLoadingDatabase_ProgressChanged(object? sender, ProgressChangedEventArgs e)
	{
		//MessageBox.Show(text: e.ProgressPercentage.ToString());
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the RunWorkerCompleted event of the BackgroundWorker for loading the database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the completion of the database loading process.
	/// </remarks>
	private void BackgroundWorkerLoadingDatabase_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
	{
		toolStripTextBoxGotoIndex.Text = 1.ToString(); // Set the initial value of the goto index text box
		currentPosition = 0; // Set the current position to the first record
		stepPosition = 100; // Set the step position to 100
		GotoCurrentPosition(position: currentPosition); // Navigate to the current position
		Enabled = true; // Enable the form
	}

	#endregion

	#region Download and update database

	/// <summary>
	/// Handles the progress changed event during the download process.
	/// Updates the progress bar and taskbar progress.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DownloadProgressChangedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to update the progress bar and taskbar progress during the download process.
	/// </remarks>
	private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		// Update the progress bar value
		toolStripProgressBarBackgroundDownload.Value = e.ProgressPercentage;
		// Set the taskbar progress value
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
	}

	/// <summary>
	/// Extracts a GZIP-compressed file to the specified output file.
	/// </summary>
	/// <param name="gzipFilePath">Full path to the source .gz file.</param>
	/// <param name="outputFilePath">Full path where the decompressed file will be written.</param>
	/// <remarks>
	/// The method streams the compressed input to the output file using <see cref="GZipStream"/>.
	/// It throws exceptions (e.g. <see cref="IOException"/>, <see cref="InvalidDataException"/>) to the caller.
	/// </remarks>
	private static void ExtractGzipFile(string gzipFilePath, string outputFilePath)
	{
		// Create a new file stream for the gzip file
		using FileStream originalFileStream = new(path: gzipFilePath, mode: FileMode.Open, access: FileAccess.Read);
		// Create a new file stream for the output file
		using FileStream decompressedFileStream = new(path: outputFilePath, mode: FileMode.Create, access: FileAccess.Write);
		// Create a GZipStream for decompression
		using GZipStream decompressionStream = new(stream: originalFileStream, mode: CompressionMode.Decompress);
		// Copy the decompressed data to the output file stream
		decompressionStream.CopyTo(destination: decompressedFileStream);
	}

	/// <summary>
	/// Handles the completion of the download process.
	/// Manages file operations and updates the UI accordingly.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="AsyncCompletedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the completion of the download process.
	/// </remarks>
	private async void ToolStripStatusLabelUpdate_Click(object sender, EventArgs e)
	{
		// Check if the user wants to download the latest MPCORB.DAT file
		if (MessageBox.Show(text: I10nStrings.AskForDownloadingLatestMpcorbDatFile,
				caption: I10nStrings.AskForDownloadingLatestMpcorbDatFileCaption, buttons: MessageBoxButtons.YesNo,
				icon: MessageBoxIcon.Question) != DialogResult.Yes)
		{
			// User chose not to download the file, so we exit the method
			return;
		}

		// Start the download process
		toolStripStatusLabelUpdate.IsLink = false;
		toolStripStatusLabelUpdate.Enabled = false;
		toolStripStatusLabelUpdate.Visible = false;
		timerBlinkForUpdateAvailable.Enabled = false;
		toolStripStatusLabelBackgroundDownload.Visible = true;
		toolStripProgressBarBackgroundDownload.Visible = true;
		toolStripStatusLabelCancelBackgroundDownload.Visible = true;
		toolStripStatusLabelBackgroundDownload.Enabled = true;
		toolStripProgressBarBackgroundDownload.Enabled = true;
		toolStripStatusLabelCancelBackgroundDownload.Enabled = true;

		downloadCancellationTokenSource = new CancellationTokenSource();

		try
		{
			// Define the URI for the MPCORB.DAT.gz file
			// Use HttpClient to download the file asynchronously
			using (HttpResponseMessage response = await httpClient.GetAsync(requestUri: uriMpcorb, completionOption: HttpCompletionOption.ResponseHeadersRead))
			{
				// Ensure the response indicates success
				_ = response.EnsureSuccessStatusCode();
				// Get the total number of bytes to download
				long totalBytes = response.Content.Headers.ContentLength ?? -1L;
				// Open the content stream for reading
				using Stream contentStream = await response.Content.ReadAsStreamAsync();
				// Create a file stream for writing the downloaded file
				using FileStream fileStream = new(path: Settings.Default.systemFilenameMpcorbTemp, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 8192, useAsync: true);
				// Buffer for reading data
				byte[] buffer = new byte[8192];
				// Variables to track progress
				long totalRead = 0;
				int read;
				// Set the progress bar style based on whether totalBytes is known
				toolStripProgressBarBackgroundDownload.Style = totalBytes > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
				// Read the content stream in chunks and write to the file stream
				while ((read = await contentStream.ReadAsync(buffer)) > 0)
				{
					// Check if the download has been cancelled
					await fileStream.WriteAsync(buffer: buffer.AsMemory(start: 0, length: read));
					totalRead += read;
					// Update progress if totalBytes is known
					if (totalBytes > 0)
					{
						int percent = (int)(totalRead * 100 / totalBytes);
						// Update the progress bar value
						toolStripProgressBarBackgroundDownload.Value = Math.Min(percent, 100);
						// Set the taskbar progress value
						TaskbarProgress.SetValue(windowHandle: Handle, progressValue: percent, progressMax: 100);
					}
				}
			}
			// Replace the old MPCORB.DAT file with the newly downloaded one
			File.Delete(path: filenameMpcorb);
			// Set the progress bar style to Marquee to indicate processing
			toolStripProgressBarBackgroundDownload.Style = ProgressBarStyle.Marquee;
			// Create a new CancellationTokenSource for this download
			cancellationTokenSource = new CancellationTokenSource();
			// Extract the downloaded GZIP file
			await Task.Run(action: () =>
				ExtractGzipFile(gzipFilePath: filenameMpcorbTemp, outputFilePath: Settings.Default.systemFilenameMpcorb),
				cancellationToken: cancellationTokenSource.Token);
			// Delete the temporary GZIP file
			File.Delete(path: filenameMpcorbTemp);
			// Notify the user that the update was successful
			AskForRestartAfterDownloadingDatabase();
		}
		catch (OperationCanceledException)
		{
			// Handle the cancellation of the download process
			toolStripStatusLabelBackgroundDownload.Enabled = false;
			toolStripProgressBarBackgroundDownload.Enabled = false;
			toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
			toolStripStatusLabelBackgroundDownload.Visible = false;
			toolStripProgressBarBackgroundDownload.Visible = false;
			toolStripStatusLabelCancelBackgroundDownload.Visible = false;
			File.Delete(path: filenameMpcorbTemp);
			_ = MessageBox.Show(text: "Download cancelled", caption: "Cancel", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
		}
		catch (Exception ex)
		{
			// Handle any errors that occur during the download process
			_ = MessageBox.Show(text: ex.Message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error, defaultButton: MessageBoxDefaultButton.Button1);
			toolStripStatusLabelUpdate.IsLink = true;
			toolStripStatusLabelUpdate.Enabled = true;
			toolStripStatusLabelUpdate.Visible = true;
			timerBlinkForUpdateAvailable.Enabled = true;
			toolStripStatusLabelBackgroundDownload.Visible = false;
			toolStripProgressBarBackgroundDownload.Visible = false;
			toolStripStatusLabelCancelBackgroundDownload.Visible = false;
			toolStripStatusLabelBackgroundDownload.Enabled = false;
			toolStripProgressBarBackgroundDownload.Enabled = false;
			toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
			File.Delete(path: filenameMpcorbTemp);
		}
		// Reset the UI elements after the download process
		toolStripStatusLabelBackgroundDownload.Enabled = false;
		toolStripProgressBarBackgroundDownload.Enabled = false;
		toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
		toolStripStatusLabelBackgroundDownload.Visible = false;
		toolStripProgressBarBackgroundDownload.Visible = false;
		toolStripStatusLabelCancelBackgroundDownload.Visible = false;
		toolStripStatusLabelUpdate.IsLink = false;
		toolStripStatusLabelUpdate.Enabled = false;
		toolStripStatusLabelUpdate.Visible = false;
		timerBlinkForUpdateAvailable.Enabled = false;
		toolStripProgressBarBackgroundDownload.Value = toolStripProgressBarBackgroundDownload.Minimum;
		// Reset the taskbar progress
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
	}

	#endregion

	#region Timer event handlers

	/// <summary>
	/// Handles the tick event for checking new MPCORB data file.
	/// Calls the PlanetoidDBForm_Shown method.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to check for a new MPCORB data file.
	/// </remarks>
	private void TimerCheckForNewMpcorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

	/// <summary>
	/// Handles the tick event for blinking the update available status label.
	/// Toggles the ForeColor of the toolStripStatusLabelUpdate.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to blink the update available status label.
	/// </remarks>
	private void TimerBlinkForUpdateAvailable_Tick(object sender, EventArgs e) => toolStripStatusLabelUpdate.ForeColor = toolStripStatusLabelUpdate.ForeColor == SystemColors.HotTrack ? SystemColors.ControlText : SystemColors.HotTrack;

	#endregion

	#region Clear event handlers

	/// <summary>
	/// Clears the checked state of all navigation step menu items.
	/// </summary>
	/// <remarks>
	/// This method is used to clear the checked state of all navigation step menu items.
	/// </remarks>
	private void ToolStripMenuItem_Clear()
	{
		// Clear the checked state of all navigation step menu items
		menuitemNavigateStep10.Checked = false;
		menuitemNavigateStep100.Checked = false;
		menuitemNavigateStep1000.Checked = false;
		menuitemNavigateStep10000.Checked = false;
		menuitemNavigateStep100000.Checked = false;
	}

	#endregion

	#region KeyPress event handlers

	/// <summary>
	/// Handles the KeyPress event for the ToolStripTextBoxGotoIndex.
	/// Ensures only numeric input and handles the Enter key to trigger navigation.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="KeyPressEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to ensure only numeric input is allowed in the ToolStripTextBoxGotoIndex.
	/// </remarks>
	private void ToolStripTextBoxGotoIndex_KeyPress(object sender, KeyPressEventArgs e)
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
			ToolStripButtonGoToIndex_Click(sender: null, e: null);
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
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to store the control that triggered the event for future reference.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
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

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepToBegin.
	/// Navigates to the beginning of the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the beginning of the data.
	/// </remarks>
	private void ToolStripButtonStepToBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepBackward.
	/// Navigates backward by a specified step in the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate backward by a specified step in the data.
	/// </remarks>
	private void ToolStripButtonStepBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepBackwardOne.
	/// Navigates to the previous data entry.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	/// This method is used to navigate to the previous data entry.
	/// </remarks>
	private void ToolStripButtonStepBackwardOne_Click(object sender, EventArgs e) => NavigateToThePreviousData();

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepForwardOne.
	/// Navigates to the next data entry.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the next data entry.
	/// </remarks>
	private void ToolStripButtonStepForwardOne_Click(object sender, EventArgs e) => NavigateToTheNextData();

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepForward.
	/// Navigates forward by a specified step in the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate forward by a specified step in the data.
	/// </remarks>
	private void ToolStripButtonStepForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

	/// <summary>
	/// Handles the click event for the ToolStripButtonStepToEnd.
	/// Navigates to the end of the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to navigate to the end of the data.
	/// </remarks>
	private void ToolStripButtonStepToEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

	/// <summary>
	/// Handles the click event for the ToolStripButtonGoToIndex.
	/// Navigates to the specified index in the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripButtonGoToIndex_Click(object? sender, EventArgs? e)
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
			ShowErrorMessage(message: $"{nameof(ToolStripButtonGoToIndex_Click)}  {ex.Message}");
			_ = MessageBox.Show(text: ex.Message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// If the parsed index is out of range, show an error message
		// Otherwise, navigate to the specified index
		if (pos <= 0 || pos >= planetoidsDatabase.Count + 1)
		{
			// Log the error message
			logger.Error(message: "Index out of range");
			// Show an error message if the index is out of range
			ShowErrorMessage(message: $"{I10nStrings.IndexOutOfRange}");
		}
		else
		{
			// Navigate to the specified index
			currentPosition = pos - 1;
			GotoCurrentPosition(position: currentPosition);
		}
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemTerminology.
	/// Opens the terminology form with the specified index.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the terminology form with the specified index.
	/// </remarks>
	private void ToolStripMenuItemTerminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

	/// <summary>
	/// Handles the click event for the ToolStripButtonTerminology.
	/// Opens the terminology form with the specified index.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the terminology form with the specified index.
	/// </remarks>
	private void ToolStripButtonTerminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

	/// <summary>
	/// Handles the click event for the ToolStripStatusLabelCancelBackgroundDownload.
	/// Cancels the background download.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to cancel the background download.
	/// </remarks>
	private void ToolStripStatusLabelCancelBackgroundDownload_Click(object sender, EventArgs e)
	{
		// Cancel download
		toolStripStatusLabelBackgroundDownload.Enabled = false;
		toolStripProgressBarBackgroundDownload.Enabled = false;
		toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
		toolStripStatusLabelBackgroundDownload.Visible = false;
		toolStripProgressBarBackgroundDownload.Visible = false;
		toolStripStatusLabelCancelBackgroundDownload.Visible = false;
		downloadCancellationTokenSource?.Cancel();
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItem10.
	/// Sets the navigation step to 10 and updates the menu item checked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripMenuItem10_Click(object sender, EventArgs e)
	{
		// Set the step position to 10
		stepPosition = 10;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		menuitemNavigateStep10.Checked = true;
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItem100.
	/// Sets the navigation step to 100 and updates the menu item checked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripMenuItem100_Click(object sender, EventArgs e)
	{
		// Set the step position to 100
		stepPosition = 100;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		menuitemNavigateStep100.Checked = true;
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItem1000.
	/// Sets the navigation step to 1000 and updates the menu item checked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripMenuItem1000_Click(object sender, EventArgs e)
	{
		// Set the step position to 1000
		stepPosition = 1000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		menuitemNavigateStep1000.Checked = true;
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItem10000.
	/// Sets the navigation step to 10000 and updates the menu item checked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripMenuItem10000_Click(object sender, EventArgs e)
	{
		// Set the step position to 10000
		stepPosition = 10000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		menuitemNavigateStep10000.Checked = true;
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItem100000.
	/// Sets the navigation step to 100000 and updates the menu item checked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to a specific index in the data.
	/// </remarks>
	private void ToolStripMenuItem100000_Click(object sender, EventArgs e)
	{
		// Set the step position to 100000
		stepPosition = 100000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		menuitemNavigateStep100000.Checked = true;
	}

	/// <summary>
	/// Handles the click event for the MenuitemExit.
	/// Closes the application.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to close the application.
	/// </remarks>
	private void MenuitemExit_Click(object sender, EventArgs e) => Close();

	/// <summary>
	/// Handles the click event for the MenuitemAbout.
	/// Shows the application information form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the application information form.
	/// </remarks>
	private void MenuitemAbout_Click(object sender, EventArgs e) => ShowAppInfo();

	/// <summary>
	/// Handles the click event for the MenuitemLicense.
	/// Shows the license form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the license form.
	/// </remarks>
	private void MenuitemLicense_Click(object sender, EventArgs e) => ShowLicense();

	/// <summary>
	/// Handles the click event for the MenuitemOpenWebsitePDB.
	/// Opens the Planetoid Database website.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the Planetoid Database website.
	/// </remarks>
	private void MenuitemOpenWebsitePDB_Click(object sender, EventArgs e) => Process.Start(fileName: Settings.Default.systemHomepage);

	/// <summary>
	/// Handles the click event for the MenuitemOpenWebsiteMPC.
	/// Opens the Minor Planet Center website.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the Minor Planet Center website.
	/// </remarks>
	private void MenuitemOpenWebsiteMPC_Click(object sender, EventArgs e) => Process.Start(fileName: Settings.Default.systemWebsiteMpc);

	/// <summary>
	/// Handles the click event for the MenuitemOpenMPCORBWebsite.
	/// Opens the MPCORB website.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the MPCORB website.
	/// </remarks>
	private void MenuitemOpenMPCORBWebsite_Click(object sender, EventArgs e) => Process.Start(fileName: Settings.Default.systemWebsiteMpcorb);

	/// <summary>
	/// Handles the click event for the MenuitemDownloadMpcorbDat.
	/// Shows the downloader form for the MPCORB database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the downloader form for the MPCORB database.
	/// </remarks>
	private void MenuitemDownloadMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatDownloader();

	/// <summary>
	/// Handles the click event for the MenuitemDownloadAstorbDat.
	/// Shows the downloader form for the ASTORB database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the downloader form for the ASTORB database.
	/// </remarks>
	private void MenuitemDownloadAstorbDat_Click(object sender, EventArgs e) => ShowAstorbDatDownloader();

	/// <summary>
	/// Handles the click event for the MenuitemCheckMpcorbDat.
	/// Shows the MPCORB data check form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	/// This method is used to show the MPCORB data check form.
	/// </remarks>
	private void MenuitemCheckMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatCheck();

	/// <summary>
	/// Handles the click event for the MenuitemCheckMpcorbDat.
	/// Shows the ASTORB data check form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the ASTORB data check form.
	/// </remarks>
	private void MenuitemCheckAstorbDat_Click(object sender, EventArgs e) => ShowAstorbDatCheck();

	/// <summary>
	/// Handles the click event for the ToolStripButtonCheckMpcorbDat.
	/// Shows the MPCORB data check form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the MPCORB data check form.
	/// </remarks>
	private void ToolStripButtonCheckMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatCheck();

	/// <summary>
	/// Handles the click event for the ToolStripButtonDownloadMpcorbDat.
	/// Shows the downloader form for the MPCORB database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the downloader form for the MPCORB database.
	/// </remarks>
	private void ToolStripButtonDownloadMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatDownloader();

	/// <summary>
	/// Handles the click event for the ToolStripButtonAbout.
	/// Shows the application information form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the application information form.
	/// </remarks>
	private void ToolStripButtonAbout_Click(object sender, EventArgs e) => ShowAppInfo();

	/// <summary>
	/// Handles the click event for the ToolStripButtonOpenWebsitePDB.
	/// Opens the Planetoid Database website.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the Planetoid Database website.
	/// </remarks>
	private void ToolStripButtonOpenWebsitePDB_Click(object sender, EventArgs e) => Process.Start(fileName: Settings.Default.systemHomepage);

	/// <summary>
	/// Handles the click event for the ToolStripButtonTableMode.
	/// Opens the table mode form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the table mode form.
	/// </remarks>
	private void ToolStripButtonTableMode_Click(object sender, EventArgs e) => OpenTableMode();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemTableMode.
	/// Opens the table mode form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to open the table mode form.
	/// </remarks>
	private void ToolStripMenuItemTableMode_Click(object sender, EventArgs e) => OpenTableMode();

	/// <summary>
	/// Handles the click event for the ToolStripButtonDatabaseInformation.
	/// Shows the database information form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the database information form.
	/// </remarks>
	private void ToolStripButtonDatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

	/// <summary>
	/// Handles the click event for the ToolStripButtonPrint.
	/// Shows the print data sheet form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the print data sheet form.
	/// </remarks>
	private void ToolStripButtonPrint_Click(object sender, EventArgs e) => PrintDataSheet();

	/// <summary>
	/// Handles the click event for the ToolStripButtonCopyToClipboard.
	/// Shows the form to copy data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the form to copy data to the clipboard.
	/// </remarks>
	private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e) => ShowCopyDataToClipboard();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemPrint.
	/// Shows the print data sheet form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the print data sheet form.
	/// </remarks>
	private void ToolStripMenuItemPrint_Click(object sender, EventArgs e) => PrintDataSheet();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemSearch.
	/// Shows the search form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the search form.
	/// </remarks>
	private void ToolStripMenuItemSearch_Click(object sender, EventArgs e) => ShowSearch();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemDatabaseInformation.
	/// Shows the database information form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the database information form.
	/// </remarks>
	private void ToolStripMenuItemDatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

	/// <summary>
	/// Handles the click event for the ToolStripButtonLoadRandomMinorPlanet.
	/// Loads a random minor planet from the database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to load a random minor planet from the database.
	/// </remarks>
	private void ToolStripButtonLoadRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemRandomMinorPlanet.
	/// Loads a random minor planet from the database.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to load a random minor planet from the database.
	/// </remarks>
	private void ToolStripMenuItemRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateToTheBegin.
	/// Navigates to the beginning of the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the beginning of the data.
	/// </remarks>
	private void ToolStripMenuItemNavigateToTheBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateSomeDataBackward.
	/// Navigates backward by a specified step in the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate backward by a specified step in the data.
	/// </remarks>
	private void ToolStripMenuItemNavigateSomeDataBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateToThePreviousData.
	/// Navigates to the previous data entry.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the previous data entry.
	/// </remarks>
	private void ToolStripMenuItemNavigateToThePreviousData_Click(object sender, EventArgs e) => NavigateToThePreviousData();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateToTheNextData.
	/// Navigates to the next data entry.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the next data entry.
	/// </remarks>
	private void ToolStripMenuItemNavigateToTheNextData_Click(object sender, EventArgs e) => NavigateToTheNextData();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateSomeDataForward.
	/// Navigates forward by a specified step in the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate forward by a specified step in the data.
	/// </remarks>
	private void ToolStripMenuItemNavigateSomeDataForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemNavigateToTheEnd.
	/// Navigates to the end of the data.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to navigate to the end of the data.
	/// </remarks>
	private void ToolStripMenuItemNavigateToTheEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemSettings.
	/// Shows the settings form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the settings form.
	/// </remarks>
	private void ToolStripMenuItemSettings_Click(object sender, EventArgs e) => ShowSettings();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemFilter.
	/// Shows the filter form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the filter form.
	/// </remarks>
	private void ToolStripMenuItemFilter_Click(object sender, EventArgs e) => ShowFilter();

	/// <summary>
	/// Handles the click event for the ToolStripButtonFilter.
	/// Shows the filter form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the filter form.
	/// </remarks>
	private void ToolStripButtonFilter_Click(object sender, EventArgs e) => ShowFilter();

	/// <summary>
	/// Handles the click event for the ToolStripButtonDerivedOrbitElements.
	/// Shows the derived orbit elements form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the derived orbit elements form.
	/// </remarks>
	private void ToolStripButtonDerivedOrbitElements_Click(object sender, EventArgs e) => ShowDerivedOrbitElements();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemRestart.
	/// Restarts the application.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to restart the application.
	/// </remarks>
	private void ToolStripMenuItemRestart_Click(object sender, EventArgs e) => Restart();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemDerivedtiveOrbitElements.
	/// Shows the derived orbit elements form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the derived orbit elements form.
	/// </remarks>
	private void ToolStripMenuItemDerivedOrbitElements_Click(object sender, EventArgs e) => ShowDerivedOrbitElements();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemStayOnTop.
	/// Checks if the form should stay on top of other windows.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to check if the form should stay on top of other windows.
	/// </remarks>
	private void ToolStripMenuItemStayOnTop_Click(object sender, EventArgs e) => CheckStayOnTop();

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemEnableCopyingByDoubleClicking.
	/// Enables copying by double-clicking.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to enable copying by double-clicking.
	/// </remarks>
	private static void ToolStripMenuItemEnableCopyingByDoubleClicking_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemEnableLinkingToTerminology.
	/// Enables linking to terminology.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to enable linking to terminology.
	/// </remarks>
	private static void ToolStripMenuItemEnableLinkingToTerminology_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemIconSetSilk.
	/// Sets the icon set to Silk.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to set the icon set to Silk.
	/// </remarks>
	private static void ToolStripMenuItemIconSetSilk_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemIconSetFugue.
	/// Sets the icon set to Fugue.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to set the icon set to Fugue.
	/// </remarks>
	private static void ToolStripMenuItemIconSetFugue_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemIconSetFatcow.
	/// Sets the icon set to Fatcow.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to set the icon set to Fatcow.
	/// </remarks>
	private static void ToolStripMenuItemIconSetFatcow_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardReadableDesignation.
	/// Copies the readable designation to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the readable designation to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardReadableDesignation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReadableDesignationData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardEpoch.
	/// Copies the epoch to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the epoch to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardEpoch_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEpochData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardMeanAnomaly.
	/// Copies the mean anomaly to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the mean anomaly to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardMeanAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAnomalyAtTheEpochData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardArgumentOfPerihelion.
	/// Copies the argument of perihelion to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the argument of perihelion to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardArgumentOfPerihelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentOfPerihelionData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardLongitudeOfTheAscendingNode.
	/// Copies the longitude of the ascending node to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the longitude of the ascending node to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeOfTheAscendingNodeData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardInclinationToTheEcliptic.
	/// Copies the inclination to the ecliptic data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the inclination to the ecliptic data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardInclinationToTheEcliptic_Click(object sender, EventArgs e) => CopyToClipboard(text: labelInclinationToTheEclipticData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardOrbitalEccentricity.
	/// Copies the orbital eccentricity data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the orbital eccentricity data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardOrbitalEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalEccentricityData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardMeanDailyMotion.
	/// Copies the mean daily motion data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the mean daily motion data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardMeanDailyMotion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanDailyMotionData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardSemiMajorAxis.
	/// Copies the semi-major axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the semi-major axis data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardSemiMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMajorAxisData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardAbsoluteMagnitude.
	/// Copies the absolute magnitude data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the absolute magnitude data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardAbsoluteMagnitude_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAbsoluteMagnitudeData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardSlopeParameter.
	/// Copies the slope parameter data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the slope parameter data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardSlopeParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSlopeParameterData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardReference.
	/// Copies the reference data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the reference data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardReference_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReferenceData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfOppositions.
	/// Copies the number of oppositions data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the number of oppositions data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardNumberOfOppositions_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfOppositionsData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfObservations.
	/// Copies the number of observations data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the number of observations data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardNumberOfObservations_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfObservationsData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardObservationSpan.
	/// Copies the observation span data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the observation span data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardObservationSpan_Click(object sender, EventArgs e) => CopyToClipboard(text: labelObservationSpanData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardRmsResidual.
	/// Copies the RMS residual data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the RMS residual data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardRmsResidual_Click(object sender, EventArgs e) => CopyToClipboard(text: labelRmsResidualData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardComputerName.
	/// Copies the computer name data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the computer name data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardComputerName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelComputerNameData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardDateOfLastObservation.
	/// Copies the date of last observation data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the date of last observation data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardDateOfLastObservation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateLastObservationData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripMenuItemCopyToClipboardFlags.
	/// Copies the flags data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the flags data to the clipboard.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboardFlags_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFlagsData.Text);

	/// <summary>
	/// Handles the click event for the ToolStripButtonExport.
	/// Exports the data sheet.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the data sheet.
	/// </remarks>
	private void ToolStripButtonExport_Click(object sender, EventArgs e) => ExportDataSheet();

	/// <summary>
	/// Handles the click event for the MenuitemTopTenRecords.
	/// Shows the records selection form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records selection form.
	/// </remarks>
	private void MenuitemTopTenRecords_Click(object sender, EventArgs e) => ShowRecordsSelection();

	/// <summary>
	/// Handles the button click event for the SplitButtonTopTenRecords.
	/// Shows the records selection form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records selection form.
	/// </remarks>
	private void SplitButtonTopTenRecords_ButtonClick(object sender, EventArgs e) => ShowRecordsSelection();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsMeanAnomalyAtTheEpoch.
	/// Shows the main records form for mean anomaly at the epoch.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for mean anomaly at the epoch.
	/// </remarks>
	private void MenuitemRecordsMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsArgumentOfPerihelion.
	/// Shows the main records form for argument of perihelion.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for argument of perihelion.
	/// </remarks>
	private void MenuitemRecordsArgumentOfPerihelion_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsLongitudeOfTheAscendingNode.
	/// Shows the main records form for the longitude of the ascending node.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for the longitude of the ascending node.
	/// </remarks>
	private void MenuitemRecordsLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsInclination.
	/// Shows the main records form for inclination.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for inclination.
	/// </remarks>
	private void MenuitemRecordsInclination_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsOrbitalEccentricity.
	/// Shows the main records form for orbital eccentricity.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for orbital eccentricity.
	/// </remarks>
	private void MenuitemRecordsOrbitalEccentricity_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsMeanDailyMotion.
	/// Shows the main records form for mean daily motion.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for mean daily motion.
	/// </remarks>
	private void MenuitemRecordsMeanDailyMotion_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsSemiMajorAxis.
	/// Shows the main records form for semi-major axis.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for semi-major axis.
	/// </remarks>
	private void MenuitemRecordsSemiMajorAxis_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsAbsoluteMagnitude.
	/// Shows the main records form for absolute magnitude.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for absolute magnitude.
	/// </remarks>
	private void MenuitemRecordsAbsoluteMagnitude_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsSlopeParameter.
	/// Shows the main records form for slope parameter.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for slope parameter.
	/// </remarks>
	private void MenuitemRecordsSlopeParameter_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsNumberOfOppositions.
	/// Shows the main records form for number of oppositions.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for number of oppositions.
	/// </remarks>
	private void MenuitemRecordsNumberOfOppositions_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsNumberOfObservations.
	/// Shows the main records form for number of observations.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for number of observations.
	/// </remarks>
	private void MenuitemRecordsNumberOfObservations_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsObservationSpan.
	/// Shows the main records form for observation span.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for observation span.
	/// </remarks>
	private void MenuitemRecordsObservationSpan_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsRmsResidual.
	/// Shows the main records form for RMS residual.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for RMS residual.
	/// </remarks>
	private void MenuitemRecordsRmsResidual_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsComputerName.
	/// Shows the main records form for computer name.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for computer name.
	/// </remarks>
	private void MenuitemRecordsComputerName_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemRecordsDateOfTheLastObservation.
	/// Shows the main records form for date of the last observation.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the records main form for date of the last observation.
	/// </remarks>
	private void MenuitemRecordsDateOfTheLastObservation_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the click event for the MenuitemDistributionMeanAnomalyAtTheEpoch.
	/// Shows the distribution form for mean anomaly at the epoch.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for mean anomaly at the epoch.
	/// </remarks>
	private static void MenuitemDistributionMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionArgumentOfPerihelion.
	/// Shows the distribution form for argument of perihelion.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for argument of perihelion.
	/// </remarks>
	private static void MenuitemDistributionArgumentOfPerihelion_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionLongitudeOfTheAscendingNode.
	/// Shows the distribution form for longitude of the ascending node.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for longitude of the ascending node.
	/// </remarks>
	private static void MenuitemDistributionLongitudeOfTheAscendingNode_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionInclination.
	/// Shows the distribution form for inclination.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for inclination.
	/// </remarks>
	private static void MenuitemDistributionInclination_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionOrbitalEccentricity.
	/// Shows the distribution form for orbital eccentricity.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for orbital eccentricity.
	/// </remarks>
	private static void MenuitemDistributionOrbitalEccentricity_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionMeanDailyMotion.
	/// Shows the distribution form for mean daily motion.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for mean daily motion.
	/// </remarks>
	private static void MenuitemDistributionMeanDailyMotion_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionSemiMajorAxis.
	/// Shows the distribution form for semi-major axis.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for semi-major axis.
	/// </remarks>
	private static void MenuitemDistributionSemiMajorAxis_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionAbsoluteMagnitude.
	/// Shows the distribution form for absolute magnitude.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for absolute magnitude.
	/// </remarks>
	private static void MenuitemDistributionAbsoluteMagnitude_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionSlopeParameter.
	/// Shows the distribution form for slope parameter.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for slope parameter.
	/// </remarks>
	private static void MenuitemDistributionSlopeParameter_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionNumberOfOppositions.
	/// Shows the distribution form for number of oppositions.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for number of oppositions.
	/// </remarks>
	private static void MenuitemDistributionNumberOfOppositions_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionNumberOfObservations.
	/// Shows the distribution form for number of observations.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for number of observations.
	/// </remarks>
	private static void MenuitemDistributionNumberOfObservations_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionObservationSpan.
	/// Shows the distribution form for observation span.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for observation span.
	/// </remarks>
	private static void MenuitemDistributionObservationSpan_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionRmsResidual.
	/// Shows the distribution form for RMS residual.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for RMS residual.
	/// </remarks>
	private static void MenuitemDistributionRmsResidual_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistributionComputerName.
	/// Shows the distribution form for computer name.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for computer name.
	/// </remarks>
	private static void MenuitemDistributionComputerName_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the button click event for the SplitButtonDistribution.
	/// Shows the distribution form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the distribution form for the selected parameter.
	/// </remarks>
	private static void SplitButtonDistribution_ButtonClick(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the MenuitemDistribution.
	/// Shows the distribution form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for the selected parameter.
	/// </remarks>
	private static void MenuitemDistribution_Click(object sender, EventArgs e)
	{
		// TODO: Not implemented yet
		_ = MessageBox.Show(text: "Not implemented yet", caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}

	/// <summary>
	/// Handles the click event for the ToolStripButtonListReadableDesignations.
	/// Lists readable designations.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for the selected parameter.
	/// </remarks>
	private void ToolStripButtonListReadableDesignations_Click(object sender, EventArgs e) => ListReadableDesignations();

	/// <summary>
	/// Handles the click event for the MenuitemListReadableDesignations.
	/// Lists readable designations.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the distribution form for the selected parameter.
	/// </remarks>
	private void MenuitemListReadableDesignations_Click(object sender, EventArgs e) => ListReadableDesignations();

	/// <summary>
	/// Handles the click event for the ToolStripButtonLicense.
	/// Opens th license.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the license.
	/// </remarks>
	private void ToolStripButtonLicense_Click(object sender, EventArgs e) => ShowLicense();

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

	/// <summary>
	/// Handles double-click events on the control to open the terminology dialog.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method attempts to parse the current tag text as an integer and opens the terminology dialog
	/// for the corresponding entry if successful.
	/// </remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Try to parse the index from the current tag text
		// If successful, open the terminology dialog for that index
		// If parsing fails, log an error and show an error message
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

	/// <summary>
	/// Handles the double-click event to show an Easter egg message.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show an Easter egg message when the user double-clicks on a control.
	/// </remarks>
	private void EasterEgg_DoubleClick(object sender, EventArgs e) => MessageBox.Show(text: I10nStrings.EasterEgg, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);

	#endregion
}