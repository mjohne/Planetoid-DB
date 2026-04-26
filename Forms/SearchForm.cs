// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Text;

namespace Planetoid_DB;

/// <summary>
/// Represents a form that provides advanced search functionality for planetoid records,
/// allowing users to search, filter, and export search results in various formats.
/// </summary>
/// <remarks>
/// The Search2Form enables users to perform text-based searches across multiple orbital
/// element fields within a planetoid database. Users can select which elements to search,
/// view results in a virtualized list, and export results to a wide range of file formats,
/// including text, spreadsheet, and markup formats. The form manages search progress,
/// cancellation, and error handling, and integrates with the main application to allow
/// navigation to specific records. Thread safety is maintained for search result access,
/// and the form provides feedback on long-running operations.
/// </remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class SearchForm : BaseKryptonForm
{
	/// <summary>List to store search results found during the search operation.</summary>
	/// <remarks>This list holds the search results as they are found during the search process. Each entry in the list represents a search result that matches the user's search criteria. The list is accessed and modified in a thread-safe manner to ensure that it can be safely updated from the background search task while being read by the UI thread for display.</remarks>
	private readonly List<SearchResult> _searchResults = [];

	/// <summary>Cancellation token source for managing search cancellation.</summary>
	/// <remarks>This token source is used to signal cancellation requests to the background search task, allowing the search operation to be stopped gracefully.</remarks>
	private CancellationTokenSource? _cts;

	/// <summary>Dictionary mapping orbital element names to functions that retrieve their values from a PlanetoidRecord.</summary>
	/// <remarks>This dictionary maps the names of orbital elements (e.g., "Epoch", "Mean anomaly") to functions that take a PlanetoidRecord as input and return the corresponding value as a string. This mapping allows the search functionality to dynamically access the relevant fields of the planetoid records based on user-selected search criteria.</remarks>
	private readonly Dictionary<string, Func<PlanetoidRecord, string>> _propertyMap = [];

	/// <summary>NLog logger for logging messages and errors.</summary>
	/// <remarks>This logger is used to log messages and errors that occur within the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>This field stores the index of the currently sorted column.</remarks>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered:
	/// <list type="bullet">
	/// <item><description><see cref="SortOrder.None"/>: No sorting is applied.</description></item>
	/// <item><description><see cref="SortOrder.Ascending"/>: Items are sorted in ascending order.</description></item>
	/// <item><description><see cref="SortOrder.Descending"/>: Items are sorted in descending order.</description></item>
	/// </list>
	/// This field is typically updated when the user clicks a column header in the list view to toggle the sort order.</summary>
	/// <remarks>This field stores the current sort order of the list view.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents the result of a search operation, containing information about the matched item.</summary>
	/// <remarks>This struct is used to store information about each search result found during the search operation. It includes the index of the record, its designation, the specific orbital element that matched the search criteria, and the value of that element. This structured format allows for easy management and display of search results in the user interface.</remarks>
	private struct SearchResult
	{
		/// <summary>Index of the matched record in the database.</summary>
		public string Index;
		/// <summary>Designation of the matched record.</summary>
		public string Designation;
		/// <summary>Name of the orbital element that matched the search criteria.</summary>
		public string Element;
		/// <summary>Value of the orbital element that matched the search criteria.</summary>
		public string Value;
	}

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="SearchForm"/> class.</summary>
	/// <remarks>This constructor sets up the form and initializes the property map for searching.</remarks>
	public SearchForm()
	{
		// Initialize the form components and set up the property map for searching.
		InitializeComponent();
		// Set up the mapping of orbital element names to their corresponding retrieval functions from a PlanetoidRecord.
		InitializePropertyMap();
	}

	#endregion

	#region helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom debugger display string.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Initializes the property map with mappings between property names and their corresponding accessors.</summary>
	/// <remarks>This method populates the internal property map with key-value pairs that associate human-readable property names with lambda expressions for accessing the corresponding properties of the target object. This mapping is typically used for dynamic property access or data export scenarios.</remarks>
	private void InitializePropertyMap()
	{
		// Populate the property map with key-value pairs, where the key is a human-readable property name and the value is a lambda expression that retrieves the corresponding property from a PlanetoidRecord. This allows for dynamic access to the properties of the records based on user selection.
		_propertyMap.Add(key: "Index No.", value: static r => r.Index);
		_propertyMap.Add(key: "Readable designation", value: static r => r.DesignationName);
		_propertyMap.Add(key: "Epoch", value: static r => r.Epoch);
		_propertyMap.Add(key: "Mean anomaly", value: static r => r.MeanAnomaly);
		_propertyMap.Add(key: "Argument of perihelion", value: static r => r.ArgPeri);
		_propertyMap.Add(key: "Longitude of ascending node", value: static r => r.LongAscNode);
		_propertyMap.Add(key: "Inclination", value: static r => r.Incl);
		_propertyMap.Add(key: "Orbital eccentricity", value: static r => r.OrbEcc);
		_propertyMap.Add(key: "Mean daily motion", value: static r => r.Motion);
		_propertyMap.Add(key: "Semi-major axis", value: static r => r.SemiMajorAxis);
		_propertyMap.Add(key: "Absolute magnitude", value: static r => r.MagAbs);
		_propertyMap.Add(key: "Slope parameter", value: static r => r.SlopeParam);
		_propertyMap.Add(key: "Reference", value: static r => r.Ref);
		_propertyMap.Add(key: "Number of observations", value: static r => r.NumberObservation);
		_propertyMap.Add(key: "Number of oppositions", value: static r => r.NumberOpposition);
		_propertyMap.Add(key: "Observation span", value: static r => r.ObsSpan);
		_propertyMap.Add(key: "r.m.s. residual", value: static r => r.RmsResidual);
		_propertyMap.Add(key: "Computer name", value: static r => r.ComputerName);
		_propertyMap.Add(key: "Flags", value: static r => r.Flags);
		_propertyMap.Add(key: "Date of last observation", value: static r => r.ObservationLastDate);
	}

	/// <summary>Displays a save file dialog and, if the user confirms, invokes the specified export action to save data to the selected file.</summary>
	/// <remarks>If the user cancels the save dialog, no export action is performed. Any exceptions thrown during the export are logged and displayed to the user in a message box. The cursor is set to a wait cursor during the export operation and reset afterward.</remarks>
	/// <param name="filter">The file type filter string that determines the choices available in the save file dialog. For example, "Text Files (*.txt)|*.txt".</param>
	/// <param name="defaultExt">The default file extension to use if the user does not specify one.</param>
	/// <param name="dialogTitle">The title displayed in the save file dialog window.</param>
	/// <param name="exportAction">An action to perform the export, which receives the ListView to export, a title string, the selected file path, and a function to retrieve virtual list view items.</param>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title. The dialog allows the user to choose where to save the exported file and what name to give it.
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle,
			FileName = $"Search-Results_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{defaultExt.TrimStart('.')}"
		};
		// Show the save dialog. If the user cancels, return without performing any export action.
		if (saveFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress, and then invoke the specified export action with the text box containing the output, the title for the export, and the selected file name. After the export action is completed, reset the cursor to the default state.
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(listViewResults, "Search Results", saveFileDialog.FileName, GetVirtualListViewItem);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			_ = MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Navigates to the object currently selected in the search results list, displaying it in the main application form
	/// if available.</summary>
	/// <param name="closeAfterNavigation">When <see langword="true"/>, closes this form after successfully navigating to the object.</param>
	/// <remarks>If no object is selected or the selected object is invalid, a message box is shown to inform the user. If the main form is not found, an error message is displayed. This method does not perform any action if a cancellation token source is active.</remarks>
	private void GoToObject(bool closeAfterNavigation = false)
	{
		// If a cancellation token source is active, it means a search operation is in progress, and we should not attempt to navigate to an object. In this case, simply return without doing anything.
		if (_cts != null)
		{
			return;
		}
		// Check if any item is selected in the list view. If not, show a warning message to the user and return without performing any navigation.
		if (listViewResults.SelectedIndices.Count == 0)
		{
			_ = MessageBox.Show(text: "Please select an object to go to.", caption: "Go To Object", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		// Get the index of the selected item in the list view. This index will be used to retrieve the corresponding search result from the _searchResults list.
		int selectedIndex = listViewResults.SelectedIndices[index: 0];
		// Declare a variable to hold the search result item that corresponds to the selected index. We will retrieve this item from the _searchResults list in a thread-safe manner.
		SearchResult item;
		// Lock the _searchResults list to ensure thread safety while accessing it. This is important because the search results may be updated by a background task while the user interacts with the UI.
		lock (_searchResults)
		{
			// Check if the selected index is within the valid range of the _searchResults list. If it is, retrieve the corresponding search result item. If not, show an error message to the user and return without performing any navigation.
			if (selectedIndex >= 0 && selectedIndex < _searchResults.Count)
			{
				// Retrieve the search result item that corresponds to the selected index from the _searchResults list.
				item = _searchResults[index: selectedIndex];
			}
			// If the selected index is out of range, show an error message to the user and return without performing any navigation.
			else
			{
				// Show an error message indicating that the selected index is out of range, which means that the user has selected an item that does not correspond to a valid search result. This could happen if the search results have been updated while the user is interacting with the list view.
				_ = MessageBox.Show(text: "Selected index is out of range.", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
				return;
			}
		}
		// Get the designation of the selected item. If the designation is null, empty, or consists only of whitespace, show an error message to the user and return without performing any navigation.
		string designation = item.Designation;
		if (string.IsNullOrWhiteSpace(value: designation))
		{
			// Show an error message indicating that the selected object does not have a valid designation, which is necessary for navigating to the object in the main form. This could happen if the search result item is incomplete or if there was an issue during the search process.
			_ = MessageBox.Show(text: "Selected object does not have a valid designation.", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			return;
		}
		// Attempt to find the main form of the application, which is expected to be of type PlanetoidDbForm. If the main form is found, call its JumpToRecord method to navigate to the record corresponding to the selected search result item. Then bring the main form to the front. If the main form is not found, show an error message to the user indicating that navigation cannot be performed.
		if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
		{
			// Call the JumpToRecord method of the main form, passing the index and designation of the selected search result item. This will navigate the main form to the corresponding record in the database.
			mainForm.JumpToRecord(index: item.Index, designation: item.Designation);
			mainForm.BringToFront();
			// Close the search form after jumping to the object in the main form if requested.
			if (closeAfterNavigation)
			{
				Close();
			}
		}
		// If the main form is not found, show an error message indicating that the main form is not available, which means that the application cannot navigate to the selected object. This could happen if the main form has not been opened yet or if it has been closed.
		else
		{
			// Show an error message indicating that the main form was not found, which means that the application cannot navigate to the selected object. This could happen if the main form has not been opened yet or if it has been closed. The user may need to open the main form and perform the search again to navigate to the desired object.
			_ = MessageBox.Show(text: "Main form not found. Cannot go to object.", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form and initializes the search UI.</summary>
	/// <param name="sender">The source of the event, typically the current form instance.</param>
	/// <param name="e">The event data associated with the Load event.</param>
	/// <remarks>Populates the checked list box with the available property names from the property map and disables actions that should remain unavailable until search results are present.</remarks>
	private void Search2Form_Load(object sender, EventArgs e)
	{
		// Populate the checked list box with the keys from the property map, allowing the user to select which properties to include in the search or export operations.
		foreach (string key in _propertyMap.Keys)
		{
			// Add each key from the property map to the checked list box as an item that the user can select.
			_ = kryptonCheckedListBoxElements.Items.Add(item: key);
		}
		toolStripButtonCancel.Enabled = false;
		toolStripButtonGoToObject.Enabled = false;
		contextMenuSaveToFile.Enabled = false;
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Search button, performing a search for planetoid records based on the user's input and selected orbital elements.</summary>
	/// <remarks>Displays progress and search results in the user interface. Disables the Search button and enables the Cancel button during the search operation. Notifies the user if required input is missing or if the database file cannot be found. Supports cancellation of the search operation.</remarks>
	/// <param name="sender">The source of the event, typically the Search button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private async void KryptonButtonSearch_Click(object sender, EventArgs e)
	{
		// Get the search text entered by the user in the search text box. If the search text is null, empty, or consists only of whitespace, show a warning message to the user and return without performing any search.
		string searchText = toolStripTextBoxSearch.Text;
		if (string.IsNullOrWhiteSpace(value: searchText))
		{
			// Show a warning message indicating that the user needs to enter a search term in order to perform a search. This is a validation step to ensure that the search operation has the necessary input to proceed.
			_ = MessageBox.Show(text: "Please enter a search term.", caption: "Search", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		// Check if the user has selected at least one orbital element to search in from the checked list box. If no elements are selected, show a warning message to the user and return without performing any search.
		if (kryptonCheckedListBoxElements.CheckedItems.Count == 0)
		{
			// Show a warning message indicating that the user needs to select at least one orbital element to search in. This is a validation step to ensure that the search operation has the necessary criteria to proceed.
			_ = MessageBox.Show(text: "Please select at least one orbital element to search in.", caption: "Search", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		// Get the file path of the planetoid database from the application settings. If the file does not exist at the specified path, show an error message to the user and return without performing any search.
		string filePath = Settings.Default.systemFilenameMpcorb;
		if (!File.Exists(path: filePath))
		{
			// Show an error message indicating that the database file could not be found at the specified path. This is a critical error that prevents the search operation from proceeding, as the search relies on reading data from the database file.
			_ = MessageBox.Show(text: $"Database file not found at: {filePath}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			return;
		}
		// If a cancellation token source is already active, it means that a search operation is currently in progress. In this case, we should not start a new search until the current one is completed or cancelled. Show a warning message to the user and return without performing any search.
		if (_cts != null)
		{
			_cts.Cancel();
			_cts.Dispose();
		}
		// Create a new cancellation token source for the upcoming search operation. This will allow the search to be cancelled if the user clicks the Cancel button.
		_cts = new CancellationTokenSource();
		CancellationToken token = _cts.Token;
		// Get the list of selected orbital element keys from the checked list box. This will determine which properties of the planetoid records will be searched for the specified search text.
		List<string> selectedKeys = [.. kryptonCheckedListBoxElements.CheckedItems.Cast<string>()];
		bool fullText = toolStripButtonFullText.Checked;
		// Disable the Search button and enable the Cancel button to reflect that a search operation is in progress. Reset the progress bar to 0% and clear any previous search results from the list view and the internal search results list.
		toolStripButtonSearch.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripButtonGoToObject.Enabled = false;
		kryptonCheckedListBoxElements.Enabled = false;
		contextMenuStripMark.Enabled = false;
		contextMenuSaveToFile.Enabled = false;
		toolStripTextBoxSearch.Enabled = false;
		toolStripButtonFullText.Enabled = false;
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Values.Text = "0 %";
		// Clear the internal search results list in a thread-safe manner to ensure that it is empty before starting the new search operation.
		lock (_searchResults)
		{
			_searchResults.Clear();
		}
		// Reset the virtual list size of the list view to 0 and invalidate it to clear any displayed items, preparing it to show the new search results as they are found.
		listViewResults.VirtualListSize = 0;
		listViewResults.Invalidate();
		// Update the progress bar text to indicate that the search is in progress.
		kryptonProgressBar.Text = "Searching...";
		// Create a list of active filters based on the selected orbital element keys. This list will contain key-value pairs where the key is the name of the orbital element and the value is a function that retrieves the corresponding property from a PlanetoidRecord. This allows the search operation to dynamically access the relevant properties of the records based on user selection.
		List<KeyValuePair<string, Func<PlanetoidRecord, string>>> activeFilters = [.. _propertyMap.Where(predicate: kv => selectedKeys.Contains(item: kv.Key))];
		// Start a background task to perform the search operation, passing the cancellation token to allow for cancellation. The search will read through the database file line by line, parse each line into a PlanetoidRecord, and check if any of the selected properties match the search text. Progress will be reported periodically to update the progress bar and the list view with found results.
		try
		{
			// Use Task.Run to perform the search operation on a background thread, allowing the UI to remain responsive. The search will read the database file, process each record, and update the search results and progress bar accordingly. The cancellation token is passed to allow the operation to be cancelled if needed.
			await Task.Run(action: () =>
			{
				// Open the database file for reading using a FileStream and a StreamReader. The file is opened with read access and shared read permissions to allow other processes to read the file while it is being searched.
				using FileStream fs = new(path: filePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read);
				using StreamReader reader = new(stream: fs, encoding: Encoding.UTF8);
				// Get the total length of the file to calculate progress as we read through it. Initialize variables to track the number of bytes processed and the number of lines read, which will be used to update the progress bar periodically.
				long totalLength = fs.Length;
				long processedBytes = 0;
				int lineCount = 0;
				// Read the file line by line until the end of the file is reached or a cancellation is requested. For each line, parse it into a PlanetoidRecord and check if any of the selected properties match the search text. If a match is found, create a SearchResult object and add it to the search results list in a thread-safe manner. Update the progress bar every 2000 lines to provide feedback on the search progress.
				string? line;
				while ((line = reader.ReadLine()) != null)
				{
					// Check if a cancellation has been requested. If so, break out of the loop to stop the search operation gracefully.
					if (token.IsCancellationRequested)
					{
						break;
					}
					// Update the count of processed bytes based on the length of the line just read. This is used to calculate the progress percentage for updating the progress bar.
					processedBytes += Encoding.UTF8.GetByteCount(s: line) + 2;
					lineCount++;
					// Skip lines that are too short to be valid records, as they cannot be parsed into a PlanetoidRecord. This is a simple validation step to avoid processing invalid data.
					if (line.Length < 200)
					{
						continue;
					}
					// Parse the line into a PlanetoidRecord using the static Parse method. If the designation name of the record is null, empty, or consists only of whitespace, skip this record and continue to the next line, as it is not valid for searching.
					PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: line);
					if (string.IsNullOrWhiteSpace(value: record.DesignationName))
					{
						continue;
					}
					// For each active filter (selected orbital element), retrieve the corresponding value from the record and check if it matches the search text. If full text search is enabled, check for an exact match; otherwise, check if the value contains the search text. If a match is found, create a SearchResult object with the relevant information and add it to the search results list in a thread-safe manner.
					foreach (KeyValuePair<string, Func<PlanetoidRecord, string>> filter in activeFilters)
					{
						// Retrieve the value of the current filter (orbital element) from the record using the corresponding function. This allows us to dynamically access the property of the record based on the user's selection.
						string value = filter.Value(arg: record);
						// Initialize a boolean variable to track whether a match is found based on the search criteria. This variable will be set to true if the value matches the search text according to the specified search mode (full text or partial match).
						bool match = false;
						// Check for a match based on the search criteria. If full text search is enabled, check for an exact match (ignoring case). If full text search is not enabled, check if the value contains the search text (ignoring case). This allows for flexible searching based on user preferences.
						if (fullText)
						{
							// If full text search is enabled, check if the value exactly matches the search text, ignoring case. This means that the search will only return results where the entire value is equal to the search text.
							if (string.Equals(a: value, b: searchText, comparisonType: StringComparison.OrdinalIgnoreCase))
							{
								match = true;
							}
						}
						// If full text search is not enabled, check if the value contains the search text, ignoring case. This means that the search will return results where the search text is a substring of the value.
						else
						{
							// If full text search is not enabled, check if the value contains the search text, ignoring case. This allows for partial matches, where the search text can be anywhere within the value.
							if (!string.IsNullOrEmpty(value: value) && value.Contains(value: searchText, comparisonType: StringComparison.OrdinalIgnoreCase))
							{
								match = true;
							}
						}
						// If a match is found, create a SearchResult object with the index, designation, element name, and value of the matched record. Then add this result to the _searchResults list in a thread-safe manner by locking the list during the update.
						if (match)
						{
							// Create a new SearchResult object to store the details of the matched record, including its index, designation, the name of the orbital element that matched, and the value of that element.
							SearchResult result = new()
							{
								Index = record.Index,
								Designation = record.DesignationName,
								Element = filter.Key,
								Value = value
							};
							// Add the SearchResult object to the _searchResults list in a thread-safe manner by locking the list during the update. This ensures that the list can be safely modified from the background search task while being read by the UI thread for display.
							lock (_searchResults)
							{
								_searchResults.Add(item: result);
							}
						}
					}
					// Update the progress bar every 2000 lines to provide feedback on the search progress. Calculate the percentage of the file that has been processed and update the progress bar value and text accordingly. Use BeginInvoke to ensure that the UI updates are performed on the UI thread, and handle any exceptions that may occur if the form is disposed during the update.
					if (lineCount % 2000 == 0)
					{
						// Calculate the percentage of the file that has been processed based on the number of bytes processed and the total length of the file. This percentage will be used to update the progress bar.
						int pct = (int)((double)processedBytes / totalLength * 100);
						// Ensure that the percentage does not exceed 100% due to any rounding issues or if the file is modified during the search. This is a safeguard to prevent the progress bar from displaying an invalid value.
						if (pct > 100)
						{
							pct = 100;
						}
						// Check if the form's handle is created and the form is not disposed or disposing before attempting to update the UI. This is important to avoid exceptions that can occur if the form has been closed while the background task is still running.
						if (IsHandleCreated && !IsDisposed && !Disposing)
						{
							// Use BeginInvoke to update the progress bar and the list view on the UI thread. This is necessary because the search operation is running on a background thread, and UI updates must be performed on the UI thread. Handle any exceptions that may occur if the form is disposed during the update to prevent crashes.
							try
							{
								// Update the progress bar value and text to reflect the current progress of the search operation. Also update the virtual list size of the list view to reflect the number of search results found so far. This provides real-time feedback to the user about the progress and results of the search.
								BeginInvoke(method: new Action(() =>
								{
									// Check again if the form is disposed or disposing before performing the UI update, as the state of the form may have changed since the previous check. If the form is disposed or disposing, return without attempting to update the UI to avoid exceptions.
									if (IsDisposed || Disposing)
									{
										return;
									}
									// Update the progress bar value to the calculated percentage and set the text to display the percentage. This gives the user a visual indication of how much of the search operation has been completed.
									kryptonProgressBar.Value = pct;
									kryptonProgressBar.Values.Text = $"{pct} %";
									// Update the taskbar progress indicator to reflect the current progress of the search operation. This provides additional feedback to the user through the taskbar, especially if the form is minimized.
									TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)pct, progressMax: 100);
									// Update the virtual list size of the list view to reflect the number of search results found so far. This allows the list view to display the new results as they are added to the _searchResults list.
									lock (_searchResults)
									{
										listViewResults.VirtualListSize = _searchResults.Count;
									}
								}));
							}
							// Catch exceptions that may occur if the form is disposed during the UI update. This can happen if the user closes the form while the background search task is still running. By catching these exceptions, we can prevent the application from crashing and allow the search task to exit gracefully.
							catch (ObjectDisposedException)
							{
								// Form is disposed; ignore update.
							}
							catch (InvalidOperationException)
							{
								// Form handle is no longer valid; ignore update.
							}
						}
					}
				}
			}, cancellationToken: token);
			// After the search operation is completed, update the progress bar text to indicate whether the search was cancelled or completed successfully, and display the number of entries found. Set the progress bar value to 100% to indicate that the search operation has finished.
			kryptonProgressBar.Text = token.IsCancellationRequested ? "Search cancelled." : $"Search completed. Found {_searchResults.Count} entries.";
			kryptonProgressBar.Value = 100;
			kryptonProgressBar.Values.Text = "100 %";
		}
		// Catch any exceptions that may occur during the search operation and display an error message to the user. This ensures that any unexpected issues during the search are communicated to the user without crashing the application.
		catch (Exception ex)
		{
			// Log the error using NLog to provide information about what went wrong during the search operation. This can help with debugging and understanding the circumstances of the error.
			logger.Error(ex, "An error occurred during the search operation.");
			kryptonProgressBar.Text = "Error during search.";
			// Show a message box to the user with the error message, allowing them to understand that an error occurred and what the error message is. This provides feedback to the user about the failure of the search operation.
			_ = MessageBox.Show(text: $"Search failed: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the Search button is re-enabled and the Cancel button is disabled to reflect that the search operation has ended. Update the virtual list size of the list view to reflect the final number of search results found, and invalidate the list view to refresh its display. Dispose of the cancellation token source to free resources, and set it to null to indicate that there is no active search operation.
		finally
		{
			// Re-enable the Search button and disable the Cancel button to reflect that the search operation has ended, allowing the user to start a new search if desired.
			toolStripButtonSearch.Enabled = true;
			toolStripButtonCancel.Enabled = false;
			kryptonCheckedListBoxElements.Enabled = true;
			contextMenuStripMark.Enabled = true;
			toolStripTextBoxSearch.Enabled = true;
			toolStripButtonFullText.Enabled = true;
			int searchResultCount;
			// Update the virtual list size of the list view to reflect the final number of search results found, and invalidate the list view to refresh its display with the new results.
			lock (_searchResults)
			{
				searchResultCount = _searchResults.Count;
				listViewResults.VirtualListSize = searchResultCount;
			}
			contextMenuSaveToFile.Enabled = searchResultCount > 0;
			toolStripButtonGoToObject.Enabled = listViewResults.SelectedIndices.Count > 0;
			// Invalidate the list view to force it to redraw and display the new search results. This is necessary because the virtual list view relies on the VirtualListSize property to determine how many items to display, and we have just updated that property.
			listViewResults.Invalidate();
			// Dispose of the cancellation token source to free resources, and set it to null to indicate that there is no active search operation. This is important for cleaning up resources and allowing a new search to be started in the future.
			_cts?.Dispose();
			// Set the cancellation token source to null to indicate that there is no active search operation. This also serves as a flag for other parts of the code to check if a search is currently in progress.
			_cts = null;
		}
	}

	/// <summary>Handles the Click event of the Cancel button, allowing the user to cancel an ongoing search operation.</summary>
	/// <param name="sender">The source of the event, typically the Cancel button.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>If a search operation is currently in progress (indicated by an active cancellation token source), this method will signal the cancellation token to request that the search operation stop. If no search operation is active, this method does nothing.</remarks>
	private void KryptonButtonCancel_Click(object sender, EventArgs e)
	{
		// Check if a cancellation token source is active, which indicates that a search operation is currently in progress. If it is active and a cancellation has not already been requested, call the Cancel method to signal the search operation to stop. This allows the user to cancel a long-running search if they no longer wish to wait for it to complete.
		if (_cts != null && !_cts.IsCancellationRequested)
		{
			_cts.Cancel();
		}
	}

	/// <summary>Handles the Click event to mark all orbital elements in the checked list box. Checks all items in the kryptonCheckedListBoxElements.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Mark All" menu item, this event handler is triggered. It iterates through all items in the kryptonCheckedListBoxElements and sets their checked state to true, effectively marking all orbital elements for inclusion in the search or export operations.</remarks>
	private void ToolStripMenuItemMarkAll_Click(object sender, EventArgs e)
	{
		// If there are no items in the checked list box, there is nothing to mark, so return without doing anything.
		if (kryptonCheckedListBoxElements.Items.Count == 0)
		{
			return;
		}
		// If all items in the checked list box are already checked, there is no need to mark them again, so return without doing anything.
		if (kryptonCheckedListBoxElements.Items.Count == kryptonCheckedListBoxElements.CheckedItems.Count)
		{
			return;
		}
		// Iterate through all items in the checked list box and set their checked state to true
		for (int i = 0; i < kryptonCheckedListBoxElements.Items.Count; i++)
		{
			kryptonCheckedListBoxElements.SetItemChecked(index: i, value: true);
		}
	}

	/// <summary>Handles the Click event to unmark all orbital elements in the checked list box. Unchecks all items in the kryptonCheckedListBoxElements.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Unmark All" menu item, this event handler is triggered. It iterates through all items in the kryptonCheckedListBoxElements and sets their checked state to false, effectively unmarking all orbital elements for exclusion from the search or export operations.</remarks>
	private void ToolStripMenuItemUnmarkAll_Click(object sender, EventArgs e)
	{
		// If there are no items in the checked list box, there is nothing to unmark, so return without doing anything.
		if (kryptonCheckedListBoxElements.Items.Count == 0)
		{
			return;
		}
		// If all items in the checked list box are already unchecked, there is no need to unmark them again, so return without doing anything.
		if (kryptonCheckedListBoxElements.CheckedItems.Count == 0)
		{
			return;
		}
		// Iterate through all items in the checked list box and set their checked state to false
		for (int i = 0; i < kryptonCheckedListBoxElements.Items.Count; i++)
		{
			kryptonCheckedListBoxElements.SetItemChecked(index: i, value: false);
		}
	}

	/// <summary>Handles the Click event to export the results as a text file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Text" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a text file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: ListViewExporter.SaveAsText);

	/// <summary>Handles the Click event to export the results as a LaTeX file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as LaTeX" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a LaTeX file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: ListViewExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the results as a Markdown file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Markdown" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a Markdown file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: ListViewExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the results as a Word document.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Word" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a Word document, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word Document", exportAction: ListViewExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the results as an OpenDocument Text file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as OpenDocument Text" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an OpenDocument Text file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: ListViewExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the results as a Rich Text Format file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Rich Text Format" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a Rich Text Format file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as Rich Text Format", exportAction: ListViewExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the results as an Excel spreadsheet.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Excel" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an Excel spreadsheet, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Spreadsheets (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel Spreadsheet", exportAction: ListViewExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the results as an OpenDocument Spreadsheet.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as OpenDocument Spreadsheet" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an OpenDocument Spreadsheet file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheets (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as OpenDocument Spreadsheet", exportAction: ListViewExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the results as a CSV file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as CSV" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a CSV file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as Comma-Separated Values", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the results as a TSV file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as TSV" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a TSV file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as Tab-Separated Values", exportAction: ListViewExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the results as a PSV file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as PSV" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a PSV file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as Pipe-Separated Values", exportAction: ListViewExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the results as an HTML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as HTML" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an HTML file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the results as an XML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as XML" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an XML file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the results as a JSON file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as JSON" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a JSON file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the results as a YAML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as YAML" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a YAML file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: ListViewExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the results as a SQL script.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as SQL" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a SQL script, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: ListViewExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the results as a PDF file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as PDF" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a PDF file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: ListViewExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the results as a PostScript file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as PostScript" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a PostScript file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: ListViewExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the results as an EPUB file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as EPUB" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an EPUB file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: ListViewExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the results as a Mobi file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Mobi" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a Mobi file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as Mobi", exportAction: ListViewExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the results as a TOML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as TOML" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a TOML file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: ListViewExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the results as an XPS file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as XPS" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an XPS file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: ListViewExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the results as a WPS Writer document.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as WPS" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a WPS Writer document, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer Documents (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: ListViewExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the results as a WPS Spreadsheet file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as WPS Spreadsheet" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a WPS Spreadsheet file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheet", exportAction: ListViewExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the results as a FictionBook2 file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as FictionBook2" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a FictionBook2 file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: ListViewExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the results as a CHM file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as CHM" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a CHM file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "CHM Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: ListViewExporter.SaveAsChm);

	/// <summary>Handles the Click event to export the results as a DocBook file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as DocBook" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a DocBook file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: ListViewExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the results as an AbiWord file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as AbiWord" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an AbiWord file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AbiWord Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as AbiWord", exportAction: ListViewExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the results as an AsciiDoc file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as AsciiDoc" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as an AsciiDoc file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: ListViewExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the results as a reStructuredText file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as reStructuredText" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a reStructuredText file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: ListViewExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the results as a Textile file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as Textile" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a Textile file, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: ListViewExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the results as a SQLite database.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>When the user clicks the "Save as SQLite" menu item, this event handler is triggered. It calls the PerformSaveExport method with parameters specific to exporting the search results as a SQLite database, including the file filter, default extension, dialog title, and the export action to be performed by the ListViewExporter.</remarks>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite3;*.sqlite;*.db)|*.sqlite3;*.sqlite;*.db|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: ListViewExporter.SaveAsSqlite);

	/// <summary>
	/// Handles the Go To Object toolbar button click by navigating to the selected object and closing the form afterwards.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
		=> GoToObject(closeAfterNavigation: true);

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles the DoubleClick event of the ListView, allowing the user to navigate to the selected object.</summary>
	/// <param name="sender">The source of the event, typically the ListView.</param>
	/// <param name="e">The event data associated with the double-click event.</param>	
	/// <remarks>When the user double-clicks on an item in the ListView, this event handler is triggered. It calls the GoToObject method to navigate to the selected object in the main application form. This provides a convenient way for users to quickly access the details of a search result by double-clicking on it.</remarks>
	private void ListViewResults_DoubleClick(object? sender, EventArgs e) => GoToObject();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// If there are no items, do not attempt to sort
		if (listViewResults.VirtualListSize == 0)
		{
			return;
		}
		// Determine the new sort order based on the clicked column
		if (e.Column == sortColumn)
		{
			// Toggle sort order if the same column is clicked
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			// Set new sort column and default to ascending order
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update column headers with sort indicators
		for (int i = 0; i < listViewResults.Columns.Count; i++)
		{
			// Remove existing sort indicators from the header text
			string headerText = listViewResults.Columns[index: i].Text;
			// Check for existing indicators and remove them
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listViewResults.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// For other columns, just update the text without indicators
			else
			{
				listViewResults.Columns[index: i].Text = headerText;
			}
		}
		// Sort the search results based on the selected column and sort order
		lock (_searchResults)
		{
			_searchResults.Sort(comparison: (a, b) =>
			{
				string valueA = GetColumnValue(result: a, columnIndex: sortColumn);
				string valueB = GetColumnValue(result: b, columnIndex: sortColumn);
				// Try to parse as numbers for numeric sorting
				bool isNumA = int.TryParse(s: valueA, result: out int numA);
				bool isNumB = int.TryParse(s: valueB, result: out int numB);
				int result;
				if (isNumA && isNumB)
				{
					// Numeric comparison
					result = numA.CompareTo(value: numB);
				}
				else
				{
					// String comparison (case-insensitive)
					result = string.Compare(strA: valueA, strB: valueB, comparisonType: StringComparison.OrdinalIgnoreCase);
				}
				// Apply sort order
				return sortOrder == SortOrder.Descending ? -result : result;
			});
		}
		// Refresh the ListView to reflect the new sort order
		listViewResults.Invalidate();
	}

	/// <summary>Gets the value for a specific column from a SearchResult.</summary>
	/// <param name="result">The SearchResult to get the value from.</param>
	/// <param name="columnIndex">The column index (0=Index, 1=Designation, 2=Element, 3=Value).</param>
	/// <returns>The string value for the specified column.</returns>
	private static string GetColumnValue(SearchResult result, int columnIndex) => columnIndex switch
	{
		0 => result.Index,
		1 => result.Designation,
		2 => result.Element,
		3 => result.Value,
		_ => string.Empty
	};

	/// <summary>Handles the SelectedIndexChanged event of the results ListView.</summary>
	/// <param name="sender">The source of the event, typically the ListView.</param>
	/// <param name="e">The event data associated with the selection change.</param>
	/// <remarks>Enables or disables the Go To Object toolbar button based on whether an item is currently selected in the list view.</remarks>
	private void ListViewResults_SelectedIndexChanged(object? sender, EventArgs e)
		=> toolStripButtonGoToObject.Enabled = listViewResults.SelectedIndices.Count > 0;

	#endregion

	#region RetrieveVirtualItem event handler

	/// <summary>Handles the RetrieveVirtualItem event for the results ListView.
	/// Provides virtual items on demand for display in the list view.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance containing the event data.</param>
	/// <remarks>This event is triggered when the ListView requires a virtual item to be displayed.</remarks>
	private void ListViewResults_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Attempt to retrieve the SearchResult corresponding to the requested item index, ensuring thread safety by locking the _searchResults list during access. If the index is valid, create a new ListViewItem populated with the data from the SearchResult; otherwise, return without setting the item, which should not occur if VirtualListSize is managed correctly.
		SearchResult item;
		lock (_searchResults)
		{
			// Check if the provided index is within the valid range of the search results list, and if so, retrieve the corresponding SearchResult; otherwise, return without setting the item, which should not occur if VirtualListSize is managed correctly
			if (e.ItemIndex >= 0 && e.ItemIndex < _searchResults.Count)
			{
				// Retrieve the SearchResult corresponding to the requested item index
				item = _searchResults[index: e.ItemIndex];
			}
			else
			{
				// If the index is out of range, return without setting the item. This should not happen if VirtualListSize is managed correctly, as it should reflect the number of items in _searchResults.
				return;
			}
		}
		// Create a new ListViewItem populated with the data from the retrieved SearchResult and assign it to the event args to be displayed in the ListView
		var lvi = new ListViewItem(text: item.Index);
		lvi.SubItems.Add(text: item.Designation);
		lvi.SubItems.Add(text: item.Element);
		lvi.SubItems.Add(text: item.Value);
		e.Item = lvi;
	}

	/// <summary>Creates a <see cref="ListViewItem"/> for the specified index from <see cref="_searchResults"/>.</summary>
	/// <param name="index">The zero-based index of the item to retrieve.</param>
	/// <returns>A <see cref="ListViewItem"/> populated with the data from <see cref="_searchResults"/>,
	/// or an empty <see cref="ListViewItem"/> when <paramref name="index"/> is out of range.</returns>
	/// <remarks>Used as the <c>virtualRowProvider</c> delegate when exporting via <see cref="Helpers.ListViewExporter"/>.</remarks>
	private ListViewItem GetVirtualListViewItem(int index)
	{
		// Check if the provided index is within the valid range of the search results list, and if so, create and return a new ListViewItem based on the corresponding SearchResult; otherwise, return an empty ListViewItem
		if (index >= 0 && index < _searchResults.Count)
		{
			// Retrieve the SearchResult corresponding to the requested item index
			SearchResult result = _searchResults[index: index];
			return new ListViewItem(items: [result.Index.ToString(), result.Designation, result.Element, result.Value]);
		}
		return new ListViewItem();
	}

	#endregion
}