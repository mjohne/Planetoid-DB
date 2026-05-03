// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Planetoid_DB;

/// <summary>Form for displaying observations of a minor planet fetched from the Minor Planet Center.</summary>
/// <remarks>This form retrieves the observation data for a given minor planet from the MPC website,
/// parses the observation records, and displays them in a ListView with 8 columns.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ObservationsForm : BaseKryptonForm
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

	/// <summary>The packed minor planet number or provisional designation used to query the MPC database.</summary>
	/// <remarks>This value is set via <see cref="SetIndexData"/> before the form is shown.</remarks>
	private string indexData = string.Empty;

	/// <summary>Shared <see cref="HttpClient"/> for HTTP requests. Reused to avoid socket exhaustion.</summary>
	/// <remarks>This <see cref="HttpClient"/> instance is shared across the application to improve performance and reduce resource usage.</remarks>
	private static readonly HttpClient httpClient = new()
	{
		// Set a reasonable timeout for HTTP requests to prevent hanging indefinitely
		Timeout = TimeSpan.FromSeconds(30)
	};

	/// <summary>Minimum number of characters an observation line must have to be parsed.</summary>
	/// <remarks>MPC observation records are fixed-width 80-character lines. Lines shorter than this are skipped.</remarks>
	private const int MinimumObservationLineLength = 80;

	/// <summary>Specifies the starting index for packed minor planet numbers.</summary>
	/// <remarks>This constant is used as the base value when working with packed representations of minor planet numbers. The value is typically used in calculations or conversions involving minor planet identifiers.</remarks>
	private const int PackedMinorPlanetNumberStart = 0;

	/// <summary>The length of the packed minor planet number field in the MPC observation record.</summary>
	/// <remarks>This constant is used to validate or format packed minor planet numbers to ensure consistency across the application.</remarks>
	private const int PackedMinorPlanetNumberLength = 5;

	/// <summary>The starting index of the packed provisional designation field in the MPC observation record.</summary>
	/// <remarks>This constant is used to validate or format packed provisional designations to ensure consistency across the application.</remarks>
	private const int PackedProvisionalDesignationStart = 5;

	/// <summary>The length of the packed provisional designation field in the MPC observation record.</summary>
	/// <remarks>This constant is used to validate or format packed provisional designations to ensure consistency across the application.</remarks>
	private const int PackedProvisionalDesignationLength = 7;

	/// <summary>Represents the starting index position of the discovery asterisk in the relevant data structure or format.</summary>
	/// <remarks>This constant is used to identify where the discovery asterisk begins. The specific meaning and usage depend on the context in which it is applied.</remarks>
	private const int DiscoveryAsteriskStart = 12;

	/// <summary>Represents the length of the discovery asterisk used in parsing or formatting operations.</summary>
	/// <remarks>This constant is used to validate or format the discovery asterisk to ensure consistency across the application.</remarks>
	private const int DiscoveryAsteriskLength = 1;

	/// <summary>Represents the start day of the observation period as a constant value.</summary>
	/// <remarks>This constant is used to identify the starting day of the observation period in the MPC observation record.</remarks>
	private const int DateOfObservationStart = 15;

	/// <summary>Represents the length of the observation period date field.</summary>
	/// <remarks>This constant is used to validate or format observation period date values to ensure consistency across the application.</remarks>
	private const int DateOfObservationLength = 17;

	/// <summary>Represents the starting value for observed rectascension measurements.</summary>
	/// <remarks>This constant is used to validate or format observed rectascension data to ensure consistency across the application.</remarks>
	private const int ObservedRectascensionStart = 32;

	/// <summary>Represents the fixed length, in characters, of the observed rectascension value.</summary>
	/// <remarks>This constant is used to validate or format observed rectascension data to ensure consistency across the application.</remarks>
	private const int ObservedRectascensionLength = 12;

	/// <summary>Represents the starting value for observed declination measurements.</summary>
	/// <remarks>This constant is used to validate or format observed declination data to ensure consistency across the application.</remarks>
	private const int ObservedDeclinationStart = 44;

	/// <summary>Represents the required length for an observed declination value.</summary>
	/// <remarks>This constant is typically used to validate or format observed declination data to ensure consistency across the application.</remarks>
	private const int ObservedDeclinationLength = 12;

	/// <summary>Represents the starting index for observed magnitude and band values in the data structure.</summary>
	/// <remarks>This constant is used to validate or format observed magnitude and band data to ensure consistency across the application.</remarks>
	private const int ObservedMagnitudeAndBandStart = 65;

	/// <summary>Represents the fixed length used for observed magnitude and band values.</summary>
	/// <remarks>This constant is used to validate or format observed magnitude and band data to ensure consistency across the application.</remarks>
	private const int ObservedMagnitudeAndBandLength = 6;

	/// <summary>Specifies the starting value for observatory codes.</summary>
	/// <remarks>This constant is used as the initial value when generating or validating observatory codes. The specific meaning and usage of this value depend on the context in which observatory codes are assigned or processed.</remarks>
	private const int ObservatoryCodeStart = 77;

	/// <summary>Represents the required length, in characters, for an observatory code.</summary>
	/// <remarks>This constant is used to validate or format observatory codes to ensure consistency across the application.</remarks>
	private const int ObservatoryCodeLength = 3;

	/// <summary>Lazy-initialized lookup dictionary that maps observatory codes to their location names.</summary>
	/// <remarks>Built once from the <c>ObservatoryCodes</c> resource on first access.</remarks>
	private static readonly Lazy<Dictionary<string, string>> _observatoryCodeLookup =
		new(valueFactory: BuildObservatoryCodeLookup);

	/// <summary>Builds the observatory code lookup dictionary from the embedded resource.</summary>
	/// <returns>A dictionary mapping observatory code strings to their location descriptions.</returns>
	/// <remarks>Each line of the resource has the format <c>CODE|Location</c>.</remarks>
	private static Dictionary<string, string> BuildObservatoryCodeLookup()
	{
		Dictionary<string, string> lookup = [];
		string resourceText = Properties.Resources.ObservatoryCodes ?? string.Empty;
		foreach (string resourceLine in resourceText.Split(separator: '\n'))
		{
			string trimmedLine = resourceLine.TrimEnd(trimChars: ['\r']);
			if (string.IsNullOrEmpty(trimmedLine))
			{
				continue;
			}
			string[] parts = trimmedLine.Split(separator: '|');
			if (parts.Length >= 2)
			{
				lookup[parts[0]] = parts[1];
			}
		}
		return lookup;
	}

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>This field is used to keep track of which column is currently being used for sorting in the ListView.</remarks>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered.</summary>
	/// <remarks>This field is used to determine the sort order (ascending, descending, or none) for the currently sorted column.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the compiled regular expression for matching the download link in the MPC observations section.</summary>
	/// <returns>A <see cref="Regex"/> instance for matching download links.</returns>
	[GeneratedRegex(pattern: @"<a\s+href=""([^""]+)""\s+target=""_blank"">download</a>", options: RegexOptions.IgnoreCase)]
	private static partial Regex DownloadLinkRegex();

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="ObservationsForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public ObservationsForm() =>
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the minor planet index data used to query the MPC observations page.</summary>
	/// <param name="indexData">The packed minor planet number or provisional designation.</param>
	/// <remarks>Call this method before showing the form so that the observation data is available on load.</remarks>
	public void SetIndexData(string indexData) => this.indexData = indexData;

	/// <summary>Fetches and parses the observation data from the MPC website, then populates the ListView.</summary>
	/// <remarks>This method performs an HTTP GET request to the MPC website, locates the observations download link, fetches the observation text file, and parses each line into the 8 observation fields.</remarks>
	private async Task LoadObservationsAsync()
	{
		// Clear existing items and status
		listView.Items.Clear();
		ClearStatusBar(label: labelInformation);
		// Validate that index data is provided before attempting to load observations
		if (string.IsNullOrWhiteSpace(value: indexData))
		{
			// If no index data is provided, show an error message and return early
			ShowErrorMessage(message: "No object identifier was provided.");
			return;
		}
		// Set the cursor to a wait cursor to indicate that a loading operation is in progress
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			// Fetch the HTML page for the object
			string pageUrl = MpcBaseUrl + Uri.EscapeDataString(stringToEscape: indexData);
			kryptonProgressBar.Text = "Loading observation data from " + pageUrl;
			kryptonProgressBar.Style = ProgressBarStyle.Marquee;
			string html = await httpClient.GetStringAsync(requestUri: pageUrl).ConfigureAwait(continueOnCapturedContext: true);
			kryptonProgressBar.Text = "Parsing observation data...";
			// Locate the <h2>Observations</h2> section and find the download link after it
			int observationsHeadingIndex = html.IndexOf(value: "<h2>Observations</h2>", comparisonType: StringComparison.Ordinal);
			// If the heading is not found, show an error message and return early
			if (observationsHeadingIndex < 0)
			{
				// If the Observations section is not found, show an error message and return early
				kryptonProgressBar.Style = ProgressBarStyle.Continuous;
				kryptonProgressBar.Text = "Observations section not found.";
				ShowErrorMessage(message: "Could not find the Observations section on the MPC page.");
				return;
			}
			// Search for <a href="..." target="_blank">download</a> after the heading
			string htmlAfterHeading = html[observationsHeadingIndex..];
			// Use the generated regex to find the download link in the HTML after the Observations heading
			Match downloadMatch = DownloadLinkRegex().Match(input: htmlAfterHeading);
			// If the download link is not found, show an error message and return early
			if (!downloadMatch.Success)
			{
				// If the download link is not found, show an error message and return early
				kryptonProgressBar.Style = ProgressBarStyle.Continuous;
				kryptonProgressBar.Text = "Observations download link not found.";
				ShowErrorMessage(message: "Could not find the observations download link on the MPC page.");
				return;
			}
			// Extract the relative URL from the regex match group
			string relativeUrl = downloadMatch.Groups[groupnum: 1].Value;
			// Convert relative URL to absolute URL
			// e.g. ../tmp2/~0uY1.txt → https://www.minorplanetcenter.net/tmp2/~0uY1.txt
			string absoluteUrl = ResolveUrl(baseUrl: MpcRootUrl, relativeUrl: relativeUrl);
			// Fetch the observations text file
			string obsText = await httpClient.GetStringAsync(requestUri: absoluteUrl).ConfigureAwait(continueOnCapturedContext: true);
			// Parse lines and populate ListView
			string[] lines = obsText.Split(separator: '\n');
			listView.BeginUpdate();
			try
			{
				// Each line is a fixed-width record with fields at specific character positions. We use the SafeSubstring helper to extract each field based on the defined constants for start index and length.
				foreach (string rawLine in lines)
				{
					// Trim any trailing carriage return characters from the line
					string line = rawLine.TrimEnd(trimChars: ['\r']);
					// Observation lines must be at least 80 characters according to the MPC format
					if (line.Length < MinimumObservationLineLength)
					{
						continue;
					}
					// Extract fields using 1-based column ranges specified in the issue
					string packedMinorPlanetNumber = SafeSubstring(value: line, startIndex: PackedMinorPlanetNumberStart, length: PackedMinorPlanetNumberLength);
					string packedProvisionalDesignation = SafeSubstring(value: line, startIndex: PackedProvisionalDesignationStart, length: PackedProvisionalDesignationLength);
					string discoveryAsterisk = SafeSubstring(value: line, startIndex: DiscoveryAsteriskStart, length: DiscoveryAsteriskLength);
					string dateOfObservation = SafeSubstring(value: line, startIndex: DateOfObservationStart, length: DateOfObservationLength);
					string observedRectascension = SafeSubstring(value: line, startIndex: ObservedRectascensionStart, length: ObservedRectascensionLength);
					string observedDeclination = SafeSubstring(value: line, startIndex: ObservedDeclinationStart, length: ObservedDeclinationLength);
					string observedMagnitudeAndBand = SafeSubstring(value: line, startIndex: ObservedMagnitudeAndBandStart, length: ObservedMagnitudeAndBandLength);
					string observatoryCode = SafeSubstring(value: line, startIndex: ObservatoryCodeStart, length: ObservatoryCodeLength);
					// Look up the location for the observatory code and append it to form the full observatory code entry
					string observatoryCodeEntry = _observatoryCodeLookup.Value.TryGetValue(key: observatoryCode, value: out string? observatoryLocation)
						? $"{observatoryCode} - {observatoryLocation}"
						: observatoryCode;
					// Create a ListViewItem with the packed minor planet number as the main text, and the other fields as subitems
					ListViewItem item = new(text: packedMinorPlanetNumber);
					item.SubItems.AddRange(items:
					[
						packedProvisionalDesignation,
						discoveryAsterisk,
						dateOfObservation,
						observedRectascension,
						observedDeclination,
						observedMagnitudeAndBand,
						observatoryCodeEntry
					]);
					listView.Items.Add(value: item);
				}
			}
			// In the event of an exception during parsing, log the error and show an error message to the user
			catch (Exception ex)
			{
				// Log the error with the exception details and the index data that was being loaded
				logger.Error(exception: ex, message: $"Error parsing observation data for '{indexData}': {ex.Message}");
				ShowErrorMessage(message: $"Error parsing observation data: {ex.Message}");
			}
			// In the finally block, ensure that EndUpdate is called on the ListView to refresh the display regardless of whether parsing succeeded or failed. This ensures that the user interface remains responsive and updates appropriately after the loading operation.
			finally
			{
				listView.EndUpdate();
			}
			// After adding all items, call EndUpdate to refresh the ListView display
			listView.EndUpdate();
			// Show summary information after loading
			kryptonProgressBar.Style = ProgressBarStyle.Continuous;
			kryptonProgressBar.Text = "Observation data loaded.";
			// Display the number of observations and the date range of the observations in a message box
			int count = listView.Items.Count;
			// If there are observations, show the first and last observation dates; otherwise, indicate that no observations were found
			if (count > 0)
			{
				// The date of observation is in the 4th column (index 3) of the ListView. We trim it to remove any extra whitespace.
				string firstDate = listView.Items[index: 0].SubItems[index: 3].Text.Trim();
				string lastDate = listView.Items[index: count - 1].SubItems[index: 3].Text.Trim();
				// Show a message box with the count of observations and the date range
				_ = MessageBox.Show(
					text: $"Number of observations: {count}\nFirst observation: {firstDate}\nLast observation: {lastDate}",
					caption: I18nStrings.InformationCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Information);
			}
			// If no observations were found, show an information message box indicating that
			else
			{
				// Show a message box indicating that no observations were found
				_ = MessageBox.Show(
					text: "No observations found.",
					caption: I18nStrings.InformationCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Information);
			}
			// Update the status bar with the count of loaded observations
			SetStatusBar(label: labelInformation, text: $"{count} observation(s) loaded.");
		}
		// Handle any exceptions that may occur during the loading and parsing process, logging the error and showing an error message to the user
		catch (Exception ex)
		{
			// Log the error with the exception details and the index data that was being loaded
			logger.Error(exception: ex, message: $"Error loading observations for '{indexData}': {ex.Message}");
			// Show an error message to the user indicating that an error occurred while loading observations, including the exception message for more details
			ShowErrorMessage(message: $"Error loading observations: {ex.Message}");
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the loading operation succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			// Reset the cursor to the default state after the loading operation is complete
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Safely extracts a substring, returning an empty string if the indices are out of range.</summary>
	/// <param name="value">The source string.</param>
	/// <param name="startIndex">Zero-based start index.</param>
	/// <param name="length">Number of characters to extract.</param>
	/// <returns>The trimmed substring, or an empty string if out of range.</returns>
	private static string SafeSubstring(string value, int startIndex, int length)
	{
		// If the start index is beyond the end of the string, return an empty string
		if (startIndex >= value.Length)
		{
			return string.Empty;
		}
		// Calculate the available length to extract, ensuring we don't go past the end of the string
		int availableLength = Math.Min(val1: length, val2: value.Length - startIndex);
		// Extract the substring using the calculated available length and trim any whitespace
		return value.Substring(startIndex: startIndex, length: availableLength).Trim();
	}

	/// <summary>Resolves a relative URL against the MPC root URL into an absolute URL.</summary>
	/// <param name="baseUrl">The base URL (e.g. https://www.minorplanetcenter.net).</param>
	/// <param name="relativeUrl">The relative URL to resolve (e.g. ../tmp2/~0uY1.txt).</param>
	/// <returns>The resolved absolute URL.</returns>
	private static string ResolveUrl(string baseUrl, string relativeUrl)
	{
		// Use Uri to resolve relative paths correctly
		Uri baseUri = new(uriString: baseUrl);
		Uri resolved = new(baseUri: baseUri, relativeUri: relativeUrl);
		// Return the absolute URI as a string
		return resolved.AbsoluteUri;
	}

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set a more specific default file name to reduce accidental overwrites
		string timestamp = DateTime.Now.ToString(format: "yyyyMMdd_HHmmss");
		dialog.FileName = $"Observations_{timestamp}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the list view, title, file name, and an optional virtual row provider.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
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
			exportAction(listView, "List of observations", saveFileDialog.FileName, null);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			// Reset the cursor to the default state after the export operation is complete
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Initiates the asynchronous loading of observatory codes and updates the user interface to reflect the loading state.</summary>
	/// <remarks>Disables relevant UI controls while loading is in progress to prevent user interaction, and re-enables them once loading is complete. Intended to be called from the UI thread.</remarks>
	private async void LoadObservatoryCodes()
	{
		// Clear the status bar and disable relevant UI controls while loading is in progress
		ClearStatusBar(label: labelInformation);
		toolStripButtonReload.Enabled = false;
		toolStripDropDownButtonSaveList.Enabled = false;
		await LoadObservationsAsync().ConfigureAwait(continueOnCapturedContext: true);
		toolStripButtonReload.Enabled = true;
		toolStripDropDownButtonSaveList.Enabled = true;
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and initiates the asynchronous loading of observation data.</remarks>
	private void ObservationsForm_Load(object sender, EventArgs e) =>
		// Clear the status bar and load the observatory codes when the form loads
		LoadObservatoryCodes();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no items in the ListView, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			return;
		}
		// Determine the new sort order based on whether the same column was clicked again or a different column was selected
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to indicate the current sort column and order using arrow indicators (▲ for ascending, ▼ for descending)
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Get the current header text for the column, removing any existing sort indicators
			string headerText = listView.Columns[index: i].Text;
			// Remove existing sort indicators (▲ or ▼) from the header text
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// If this column is the currently sorted column, prepend the appropriate sort indicator to the header text; otherwise, use the plain header text
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Set the ListViewItemSorter to a new instance of ListViewItemComparer with the selected column and sort order, then call Sort to apply the sorting to the ListView
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as HTML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as JSON.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQL.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: ListViewExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Markdown.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Markdown files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: ListViewExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as YAML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: ListViewExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: ListViewExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: ListViewExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as LaTeX.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: ListViewExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PostScript.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: ListViewExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PDF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: ListViewExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as EPUB.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: ListViewExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Word.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Word documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: ListViewExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Excel.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: ListViewExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODT file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODT.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as ODT", exportAction: ListViewExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: ListViewExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as MOBI.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: ListViewExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as RTF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: ListViewExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Text files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: ListViewExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as AsciiDoc.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: ListViewExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a reStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a reStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as reStructuredText.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: ListViewExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Textile.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: ListViewExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Abiword.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: ListViewExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as WPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", exportAction: ListViewExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ET.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", exportAction: ListViewExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as DocBook.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: ListViewExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TOML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: ListViewExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: ListViewExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as FictionBook2.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: ListViewExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CHM.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: ListViewExporter.SaveAsChm);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQLite.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: ListViewExporter.SaveAsSqlite);

	/// <summary>Handles the Click event of the Reload button to refresh the observatory codes displayed in the form.</summary>
	/// <param name="sender">The source of the event, typically the Reload button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the Reload button is clicked, this event handler clears the status bar and initiates the loading of observatory codes by calling the LoadObservatoryCodes method.</remarks>
	private void ToolStripButtonReload_Click(object sender, EventArgs e) =>
		// Clear the status bar and load the observatory codes when the form loads
		LoadObservatoryCodes();

	/// <summary>Handles the Click event of the Observatory Codes button to open the <see cref="ObservatoryCodesForm"/>.</summary>
	/// <param name="sender">The source of the event, typically the Observatory Codes button.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	/// <remarks>Opens the <see cref="ObservatoryCodesForm"/> as a modal dialog to display the list of observatory codes.</remarks>
	private void ToolStripButtonObservatoryCodes_Click(object sender, EventArgs e)
	{
		// Open the ObservatoryCodesForm as a modal dialog to display the list of observatory codes. The form is set to be topmost based on the current state of the main form to ensure it appears above other windows.
		using ObservatoryCodesForm formObservatoryCodes = new();
		formObservatoryCodes.TopMost = TopMost;
		_ = formObservatoryCodes.ShowDialog();
	}

	#endregion
}