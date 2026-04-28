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
	private const string MpcBaseUrl = "https://www.minorplanetcenter.net/db_search/show_object?object_id=";

	/// <summary>Base URL of the Minor Planet Center website, used to resolve relative download links.</summary>
	private const string MpcRootUrl = "https://www.minorplanetcenter.net";

	/// <summary>The packed minor planet number or provisional designation used to query the MPC database.</summary>
	/// <remarks>This value is set via <see cref="SetIndexData"/> before the form is shown.</remarks>
	private string indexData = string.Empty;

	/// <summary>Shared <see cref="HttpClient"/> for HTTP requests. Reused to avoid socket exhaustion.</summary>
	private static readonly HttpClient httpClient = new()
	{
		Timeout = TimeSpan.FromSeconds(30)
	};

	/// <summary>Stores the index of the currently sorted column.</summary>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered.</summary>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

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
	public void SetIndexData(string indexData) =>
		this.indexData = indexData;

	/// <summary>Fetches and parses the observation data from the MPC website, then populates the ListView.</summary>
	/// <remarks>This method performs an HTTP GET request to the MPC website, locates the observations
	/// download link, fetches the observation text file, and parses each line into the 8 observation fields.</remarks>
	private async Task LoadObservationsAsync()
	{
		listView.Items.Clear();
		ClearStatusBar(label: labelInformation);

		if (string.IsNullOrWhiteSpace(value: indexData))
		{
			ShowErrorMessage(message: "No object identifier was provided.");
			return;
		}

		try
		{
			Cursor.Current = Cursors.WaitCursor;

			// Fetch the HTML page for the object
			string pageUrl = MpcBaseUrl + Uri.EscapeDataString(stringToEscape: indexData);
			string html = await httpClient.GetStringAsync(requestUri: pageUrl).ConfigureAwait(continueOnCapturedContext: true);

			// Locate the <h2>Observations</h2> section and find the download link after it
			int observationsHeadingIndex = html.IndexOf(value: "<h2>Observations</h2>", comparisonType: StringComparison.Ordinal);
			if (observationsHeadingIndex < 0)
			{
				ShowErrorMessage(message: "Could not find the Observations section on the MPC page.");
				return;
			}

			// Search for <a href="..." target="_blank">download</a> after the heading
			string htmlAfterHeading = html[observationsHeadingIndex..];
			Match downloadMatch = Regex.Match(
				input: htmlAfterHeading,
				pattern: @"<a\s+href=""([^""]+)""\s+target=""_blank"">download</a>",
				options: RegexOptions.IgnoreCase);

			if (!downloadMatch.Success)
			{
				ShowErrorMessage(message: "Could not find the observations download link on the MPC page.");
				return;
			}

			string relativeUrl = downloadMatch.Groups[groupnum: 1].Value;

			// Convert relative URL to absolute URL
			// e.g. ../tmp2/~0uY1.txt → https://www.minorplanetcenter.net/tmp2/~0uY1.txt
			string absoluteUrl = ResolveUrl(baseUrl: MpcRootUrl, relativeUrl: relativeUrl);

			// Fetch the observations text file
			string obsText = await httpClient.GetStringAsync(requestUri: absoluteUrl).ConfigureAwait(continueOnCapturedContext: true);

			// Parse lines and populate ListView
			string[] lines = obsText.Split(separator: '\n');
			listView.BeginUpdate();

			foreach (string rawLine in lines)
			{
				// Observation lines must be at least 80 characters according to the MPC format
				if (rawLine.Length < 15)
				{
					continue;
				}

				string line = rawLine.TrimEnd(trimChars: ['\r']);

				// Extract fields using 1-based column ranges specified in the issue
				string packedMinorPlanetNumber = SafeSubstring(value: line, startIndex: 0, length: 5);
				string packedProvisionalDesignation = SafeSubstring(value: line, startIndex: 5, length: 7);
				string discoveryAsterisk = SafeSubstring(value: line, startIndex: 12, length: 1);
				string dateOfObservation = SafeSubstring(value: line, startIndex: 15, length: 17);
				string observedRectascension = SafeSubstring(value: line, startIndex: 32, length: 12);
				string observedDeclination = SafeSubstring(value: line, startIndex: 44, length: 12);
				string observedMagnitudeAndBand = SafeSubstring(value: line, startIndex: 65, length: 6);
				string observatoryCode = SafeSubstring(value: line, startIndex: 77, length: 3);

				ListViewItem item = new(text: packedMinorPlanetNumber);
				item.SubItems.AddRange(items:
				[
					packedProvisionalDesignation,
					discoveryAsterisk,
					dateOfObservation,
					observedRectascension,
					observedDeclination,
					observedMagnitudeAndBand,
					observatoryCode
				]);
				listView.Items.Add(value: item);
			}

			listView.EndUpdate();

			// Show summary information after loading
			int count = listView.Items.Count;
			if (count > 0)
			{
				string firstDate = listView.Items[index: 0].SubItems[index: 3].Text.Trim();
				string lastDate = listView.Items[index: count - 1].SubItems[index: 3].Text.Trim();
				_ = MessageBox.Show(
					text: $"Number of observations: {count}\nFirst observation: {firstDate}\nLast observation: {lastDate}",
					caption: I18nStrings.InformationCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Information);
			}
			else
			{
				_ = MessageBox.Show(
					text: "No observations found.",
					caption: I18nStrings.InformationCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Information);
			}

			SetStatusBar(label: labelInformation, text: $"{count} observation(s) loaded.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error loading observations for '{indexData}': {ex.Message}");
			ShowErrorMessage(message: $"Error loading observations: {ex.Message}");
		}
		finally
		{
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
		if (startIndex >= value.Length)
		{
			return string.Empty;
		}
		int availableLength = Math.Min(val1: length, val2: value.Length - startIndex);
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
		return resolved.AbsoluteUri;
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and initiates the asynchronous loading of observation data.</remarks>
	private async void ObservationsForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		await LoadObservationsAsync().ConfigureAwait(continueOnCapturedContext: true);
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (listView.Items.Count == 0)
		{
			return;
		}

		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}

		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
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

		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion
}
