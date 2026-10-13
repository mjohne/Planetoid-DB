/*
 * File:        ObservatoryCodesForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that displays the list of MPC observatory codes and their corresponding locations.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>Represents a form that displays the list of MPC observatory codes and their corresponding locations.</summary>
/// <remarks>This form provides an eight-column ListView with observatory codes and location data. All data is built-in and does not require an internet connection.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class ObservatoryCodesForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of observatory codes";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "ObservatoryCodes";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column in the ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header to toggle the sort order.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ObservatoryCodesForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public ObservatoryCodesForm()
	{
		// Initialize the form components
		InitializeComponent();
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Populates the ListView with the observatory code data from the embedded resource.</summary>
	/// <remarks>This method reads the <c>ObservatoryCodes</c> resource line by line, splits each entry by the pipe character, and populates the ListView with the code and location. It uses BeginUpdate/EndUpdate for performance when adding many items.</remarks>
	private void LoadObservatoryCodesList()
	{
		// Log the start of the loading process
		logger.Info(message: "Loading observatory codes into ListView.");
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// Populate the ListView
		try
		{
			// Set the cursor to wait while loading data
			Cursor.Current = Cursors.WaitCursor;
			// Use BeginUpdate/EndUpdate to improve performance when adding multiple items to the ListView
			listView.BeginUpdate();
			listView.Items.Clear();
			// Read the observatory codes from the embedded resource
			string resourceText = Properties.Resources.ObservatoryCodes ?? string.Empty;
			// Use a StringReader to read the resource text line by line
			using StringReader reader = new(s: resourceText);
			// Preallocate a list to hold the ListViewItems for performance
			List<ListViewItem> items = new(capacity: 4000);
			// Read each line from the resource text
			string? line;
			// Process each line to extract the observatory code and location
			while ((line = reader.ReadLine()) != null)
			{
				// Skip empty lines
				if (string.IsNullOrWhiteSpace(value: line))
				{
					continue;
				}
				// Split the line into parts using the pipe character as a separator, limiting to 8 parts (code, longitude, latitude, altitude, rho cos phi, rho sin phi,state and location)
				string[] parts = line.Split(separator: '|', count: 8);
				// If the line contains a code, longitude, latitude, altitude, rho cos phi, rho sin phi,state and location, create a ListViewItem and add it to the list
				if (parts.Length == 8)
				{
					for (int i = 1; i <= 5; i++)
					{
						if (double.TryParse(s: parts[i].Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double value))
						{
							parts[i] = value.ToString(provider: CultureInfo.CurrentCulture);
						}
					}
					ListViewItem item = new(parts[0].Trim());
					_ = item.SubItems.Add(text: parts[1].Trim());
					_ = item.SubItems.Add(text: parts[2].Trim());
					_ = item.SubItems.Add(text: parts[3].Trim());
					_ = item.SubItems.Add(text: parts[4].Trim());
					_ = item.SubItems.Add(text: parts[5].Trim());
					_ = item.SubItems.Add(text: parts[6].Trim());
					_ = item.SubItems.Add(text: parts[7].Trim());
					items.Add(item: item);
				}
			}
			// Add the items to the ListView
			listView.Items.AddRange([.. items]);
			// Update status bar with count
			SetStatusBar(label: labelInformation, text: $"{listView.Items.Count} observatory codes loaded");
		}
		// Log the successful loading of observatory codes
		catch (Exception ex)
		{
			// Log the error and show a message box to the user
			logger.Error(exception: ex, message: "An error occurred while loading observatory codes.");
			_ = KryptonMessageBox.Show(owner: this,
				text: $"An error has occurred while loading observatory codes: {ex.Message}",
				caption: "Load Error",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
			// Update the status bar to show a persistent error message
			SetStatusBar(label: labelInformation, text: "Error loading observatory codes");
		}
		// Ensure that the cursor is reset and the ListView is updated even if an error occurs
		finally
		{
			// Log the completion of the loading process
			logger.Info(message: "Finished loading observatory codes into ListView.");
			// End the update of the ListView and reset the cursor
			listView.EndUpdate();
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and loads the observatory codes into the ListView.</remarks>
	private void ObservatoryCodesForm_Load(object sender, EventArgs e)
	{
		// Log that the form has been loaded
		logger.Info(message: "ObservatoryCodesForm loaded.");
		// Clear the status bar and load the observatory codes into the ListView
		LoadObservatoryCodesList();
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// Log the column click event for sorting
		logger.Info(message: $"Column {e.Column} clicked for sorting.");
		// If there are no items in the ListView, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			logger.Warn(message: "Column click event ignored because the ListView is empty.");
			return;
		}
		// Determine the new sort order based on the clicked column and current sort state
		sortOrder = (e.Column == sortColumn && sortOrder == SortOrder.Ascending)
			? SortOrder.Descending
			: SortOrder.Ascending;
		// Update the sort column to the one that was clicked
		sortColumn = e.Column;
		// Update the column headers to indicate the current sort column and order
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.CurrentCulture) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.CurrentCulture))
			{
				headerText = headerText[2..];
			}
			// Add an indicator to the sorted column header
			if (i == sortColumn)
			{
				// Use an arrow to indicate the sort order in the column header
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				// Update the column header text with the sort indicator
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// Reset the header text for unsorted columns
			else
			{
				// Remove any sort indicator from the header text of unsorted columns
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
		// Log the completion of the sorting operation
		logger.Info(message: $"ListView sorted by column {e.Column} in {sortOrder} order.");
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the ToolStripButton that initiates the download of the latest observatory codes.</summary>
	/// <param name="sender">The source of the event, typically the ToolStripButton that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method checks for network availability before opening a downloader form to fetch the latest observatory codes from the specified URL.</remarks>
	private void ToolStripButtonDownloadLatestObsCodes_Click(object sender, EventArgs e)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: "Download Observatory Codes menu item clicked; checking network availability for observatory codes download.");
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log that there is no internet connection
			logger.Warn(message: "No internet connection available; cannot download observatory codes.");
			// Show an error message indicating that there is no internet connection
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			// Exit the method early since the download cannot proceed without an internet connection
			return;
		}
		// Determine the URL to use for downloading observatory codes, preferring user-defined settings over system defaults
		string obsCodesUrl = string.IsNullOrWhiteSpace(value: Settings.Default.userObsCodesUrl)
			? Settings.Default.systemObsCodesUrl
			: Settings.Default.userObsCodesUrl;
		// Create and display the database downloader form with the specified download URL
		using DownloaderForm downloaderForm = new(url: obsCodesUrl);
		// Set the TopMost property of the downloader form to match the main form's TopMost property
		downloaderForm.TopMost = TopMost;
		// Show the downloader form as a modal dialog and check the result
		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			// Log that the user canceled the download and no action was taken
			logger.Info(message: "User canceled the observatory codes download; no action taken.");
			// Exit the method early since the user canceled the download
			return;
		}
	}

	/// <summary>Handles the Click event of the ToolStripButton that opens the MPC Observatory Codes page in the default web browser.</summary>
	/// <param name="sender">The source of the event, typically the ToolStripButton that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method logs the action and opens the specified URL in the user's default web browser.</remarks>
	private void ToolStripButtonOpenMpcObsCodesPage_Click(object sender, EventArgs e)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: "Open MPC Observatory Codes page menu item clicked; preparing to open the page in the default web browser.");
		// Determine the URL to use for opening the MPC Observatory Codes page, preferring user-defined settings over system defaults
		string obsCodesPageUrl = string.IsNullOrWhiteSpace(value: Settings.Default.userMpcObsCodesPage)
			? Settings.Default.systemMpcObsCodesPage
			: Settings.Default.userMpcObsCodesPage;
		// Log the action of opening the MPC Observatory Codes page
		logger.Info(message: $"Opening MPC Observatory Codes page; {obsCodesPageUrl}");
		// Open the MPC Observatory Codes page in the default web browser
		OpenWebsite(fileName: obsCodesPageUrl);
	}

	/// <summary>Handles the MouseDoubleClick event on the ListView to copy the value of the double-clicked cell to the Windows clipboard.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Uses <see cref="ListView.HitTest(Point)"/> to identify the exact sub-item under the mouse pointer and copies its text via <see cref="BaseKryptonForm.CopyToClipboard(string)"/>.</remarks>
	private void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		// Log the mouse double-click event with the coordinates of the click
		logger.Info(message: $"Mouse double-click event at coordinates ({e.X}, {e.Y}) on ListView; attempting to copy cell value to clipboard.");
		// Hit-test to determine which cell (sub-item) was clicked
		ListViewHitTestInfo hit = listView.HitTest(x: e.X, y: e.Y);
		// If the hit test did not return a valid item or sub-item, exit early
		if (hit.Item is null || hit.SubItem is null)
		{
			// Log a warning that the double-click event was ignored due to no valid item or sub-item being found
			logger.Warn(message: "Mouse double-click event ignored; no valid ListView item or sub-item was found at the clicked location.");
			// Exit the method early since there is nothing to copy
			return;
		}
		// Retrieve the text of the sub-item that was double-clicked
		string text = hit.SubItem.Text;
		// If the text is null or empty, exit early without copying
		if (string.IsNullOrEmpty(value: text))
		{
			// Log a warning that the double-click event was ignored due to the sub-item having no text
			logger.Warn(message: "Mouse double-click event ignored; the clicked ListView sub-item has no text to copy.");
			// Exit the method early since there is no text to copy
			return;
		}
		// Log the action of copying the text to the clipboard
		logger.Info(message: $"Copying observatory information cell to clipboard: {text}");
		// Copy the text of the double-clicked cell to the Windows clipboard
		CopyToClipboard(text: text);
	}

	/// <summary>Handles the Mark on map button click.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Opens the world map at the selected observatory when its coordinates are valid.</remarks>
	private void ToolStripButtonMarkOnMap_Click(object sender, EventArgs e)
	{
		// Log the click on the button
		logger.Info(message: "Mark on map button clicked.");
		// Require a selected row
		if (listView.SelectedItems.Count == 0)
		{
			// Log a warning that no row is selected and the action cannot proceed
			logger.Warn(message: "Mark on map ignored because no row is selected.");
			// Show a warning message box to the user indicating that they need to select an observatory first
			_ = KryptonMessageBox.Show(owner: this,
				text: $"Mark on map ignored because no row is selected.\n\nPlease select an observatory first.",
				caption: "No row selected",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			// Update the status bar to inform the user that they need to select an observatory first
			SetStatusBar(label: labelInformation, text: "Please select an observatory first.");
			// Exit the method early since there is no selected observatory to mark on the map
			return;
		}
		ListViewItem item = listView.SelectedItems[index: 0];
		// The ListView values were formatted with the current culture when loading
		if (!decimal.TryParse(s: item.SubItems[index: 1].Text, style: NumberStyles.Float, provider: CultureInfo.CurrentCulture, result: out decimal longitude)
			|| !decimal.TryParse(s: item.SubItems[index: 2].Text, style: NumberStyles.Float, provider: CultureInfo.CurrentCulture, result: out decimal latitude))
		{
			// Log a warning that the selected row has no valid geographic coordinates
			logger.Warn(message: "Mark on map ignored because the selected row has no valid geographic coordinates.");
			// Show a warning message box to the user indicating that the selected observatory has no valid coordinates
			_ = KryptonMessageBox.Show(owner: this,
				text: $"Mark on map ignored because the selected row has no valid geographic coordinates.\n\nPlease select an observatory with valid coordinates.",
				caption: "Invalid coordinates",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			// Update the status bar to inform the user that the selected observatory has no valid geographic coordinates
			SetStatusBar(label: labelInformation, text: "The selected observatory has no valid geographic coordinates.");
			// Exit the method early since the selected observatory cannot be marked on the map
			return;
		}
		// MPC longitudes are given eastward from 0 to 360 degrees; the map expects -180 to 180
		if (longitude > 180m)
		{
			longitude -= 360m;
		}
		// Log the coordinates that will be marked on the map
		logger.Info(message: $"Marking observatory on map at coordinates: Latitude {latitude}, Longitude {longitude}");
		// Open the world map with a marker at the observatory position
		using WorldMapForm worldMapForm = new(latitude: latitude, longitude: longitude);
		_ = worldMapForm.ShowDialog(owner: this);
	}

	#endregion
}