/*
 * File:        LogViewerForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: A form that displays all NLog log events captured during the current application session.
 *
 * Autor:       Michael Johne
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

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>A form that displays all NLog log events captured during the current application session.</summary>
/// <remarks>The form loads all stored log events from <see cref="LogEventStore"/> asynchronously when it opens. The <see cref="ListView"/> operates in virtual mode for performance. Users can delete selected entries or clear all entries via toolbar buttons.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class LogViewerForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of log events";

	/// <summary>Prepares the save dialog used for export operations.</summary>
	/// <param name="dialog">The dialog to configure before it is displayed.</param>
	/// <param name="ext">The file extension selected for the export.</param>
	/// <returns><see langword="true"/> if the user confirms the dialog; otherwise, <see langword="false"/>.</returns>
	/// <remarks>Overrides the default file naming to use <c>logs.&lt;ext&gt;</c> as the suggested file name.</remarks>
	protected override bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"logs.{ext}";
		return dialog.ShowDialog(owner: this) == DialogResult.OK;
	}

	#endregion

	/// <summary>NLog logger instance for this form.</summary>
	/// <remarks>This logger is used to log events and errors that occur within this form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>In-memory snapshot of <see cref="LogEventInfo"/> objects currently shown in the ListView.</summary>
	/// <remarks>This list is the backing store for virtual-mode item retrieval.</remarks>
	private List<LogEventInfo> _displayCache = [];

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

	/// <summary>Stores the sorted indices for virtual mode to maintain sorting order.</summary>
	/// <remarks>This list maps the virtual list view indices to the actual <see cref="_displayCache"/> indices based on the current sort criteria.</remarks>
	private List<int>? sortedIndices;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="LogViewerForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public LogViewerForm() => InitializeComponent();

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Formats a <see cref="DateTime"/> value for display in the Date/Time column.</summary>
	/// <param name="timestamp">The timestamp to format.</param>
	/// <returns>A formatted date/time string using <c>yyyy-MM-dd HH:mm:ss.fff</c>.</returns>
	/// <remarks>This method ensures consistent formatting of timestamps in the ListView.</remarks>
	private static string FormatTimestamp(DateTime timestamp) => timestamp.ToString(format: "yyyy-MM-dd HH:mm:ss.fff", provider: CultureInfo.InvariantCulture);

	/// <summary>Creates a <see cref="ListViewItem"/> from a <see cref="LogEventInfo"/> for virtual-mode display.</summary>
	/// <param name="logEvent">The log event to convert into a list item.</param>
	/// <returns>A <see cref="ListViewItem"/> with sub-items for level, exception type, and message.</returns>
	/// <remarks>This method extracts relevant information from the log event and constructs a ListViewItem suitable for display in the virtual ListView.</remarks>
	private static ListViewItem CreateListViewItem(LogEventInfo logEvent)
	{
		// Format the timestamp, level, exception type, and message for display
		string timestampText = FormatTimestamp(timestamp: logEvent.TimeStamp);
		string levelText = logEvent.Level?.Name ?? string.Empty;
		string exceptionType = logEvent.Exception?.GetType().Name
			?? (logEvent.Properties.TryGetValue(key: "ExceptionTypeName", value: out object? stored) ? stored?.ToString() : null)
			?? string.Empty;
		string message = logEvent.FormattedMessage ?? string.Empty;
		// Create a new ListViewItem with the timestamp as the main text and add sub-items for level, exception type, and message
		ListViewItem item = new(text: timestampText);
		_ = item.SubItems.Add(text: levelText);
		_ = item.SubItems.Add(text: exceptionType);
		_ = item.SubItems.Add(text: message);
		// Return the constructed ListViewItem
		return item;
	}

	/// <summary>Updates the enabled state of the delete buttons based on the current state of the list view.</summary>
	/// <remarks>This method checks the number of items in the display cache and the number of selected items in the list view to determine whether the delete buttons should be enabled or disabled.</remarks>
	private void UpdateButtonStates()
	{
		// Enable the "Delete All" button if there are any items in the display cache
		toolStripButtonDeleteAll.Enabled = _displayCache.Count > 0;
		// Enable the "Delete Selected" button if there are any selected items in the list view
		toolStripButtonDeleteSelected.Enabled = listView.SelectedIndices.Count > 0;
	}

	/// <summary>Asynchronously loads all log events from <see cref="LogEventStore"/> into the ListView, reporting progress via the toolbar progress bar.</summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <remarks>This method retrieves a snapshot of log events on a background thread, updates the display cache, and refreshes the ListView in virtual mode. It also handles exceptions and updates the status bar accordingly.</remarks>
	private async Task LoadLogEventsAsync()
	{
		// Reset the ListView and progress bar
		listView.VirtualListSize = 0;
		_displayCache.Clear();
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Values.Text = "0%";
		ClearStatusBar(label: labelInformation);
		SetStatusBar(label: labelInformation, text: "Loading log events…");
		// Disable delete buttons during loading
		toolStripButtonDeleteSelected.Enabled = false;
		toolStripButtonDeleteAll.Enabled = false;
		// Start a stopwatch to measure loading time
		Stopwatch stopwatch = Stopwatch.StartNew();
		// Log the start of the loading process
		try
		{
			// Take a snapshot of the log event store on a background thread and convert to display entries while reporting progress
			IProgress<int> progress = new Progress<int>(handler: percentage =>
			{
				// Update the progress bar on the UI thread
				if (percentage is >= 0 and <= 100)
				{
					// Update the progress bar value and text
					kryptonProgressBar.Value = percentage;
					kryptonProgressBar.Values.Text = $"{percentage}%";
				}
			});
			// Run the snapshot retrieval on a background thread to avoid blocking the UI
			List<LogEventInfo> snapshot = await Task.Run(function: () =>
			{
				// Get snapshot from the store
				List<LogEventInfo> events = LogEventStore.GetSnapshot();
				// Report initial progress
				progress.Report(value: 0);
				return events;
			});
			logger.Info(message: $"Loaded {snapshot.Count} log events from the store.");
			// The snapshot is ready; update the UI-bound cache on the UI thread
			_displayCache = snapshot;
			// Reset sort state so indices are rebuilt on next column click
			sortedIndices = null;
			sortColumn = -1;
			sortOrder = SortOrder.None;
			// Report complete loading
			int total = _displayCache.Count;
			for (int i = 0; i <= 100; i += 10)
			{
				progress.Report(value: i);
				await Task.Delay(millisecondsDelay: 1);
			}
			// Activate virtual mode with the loaded count
			listView.VirtualListSize = total;
			// Stop the stopwatch and log the elapsed time
			stopwatch.Stop();
			// Log and Show completion message
			logger.Info(message: $"Log events loaded in {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff} hh:mm:ss.fff.");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: $"{total} log {(total == 1 ? "entry" : "entries")} loaded in {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff} hh:mm:ss.fff.",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information);
			// Update the status bar with the total number of entries loaded
			SetStatusBar(label: labelInformation, text: $"{total} log {(total == 1 ? "entry" : "entries")} loaded");
		}
		// Handle any exceptions that occur during loading
		catch (Exception ex)
		{
			// Stop the stopwatch in case of an error
			stopwatch.Stop();
			// Log the exception with NLog
			logger.Error(exception: ex, message: "An error occurred while loading log events.");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: $"An error occurred while loading log events: {ex.Message}",
				caption: I18nStrings.ErrorCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
			// Update the status bar to indicate an error occurred
			SetStatusBar(label: labelInformation, text: "Error loading log events");
		}
		// Ensure the progress bar and button states are updated regardless of success or failure
		finally
		{
			// Ensure progress bar reaches 100% and buttons are updated
			kryptonProgressBar.Value = 100;
			kryptonProgressBar.Values.Text = "100%";
			UpdateButtonStates();
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form and starts the asynchronous log event loading.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Loading is performed asynchronously to keep the UI responsive.</remarks>
	private async void LogViewerForm_Load(object sender, EventArgs e) => await LoadLogEventsAsync();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the <see cref="ListView.RetrieveVirtualItem"/> event to supply list items on demand.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance that contains the item index.</param>
	/// <remarks>Uses <see cref="sortedIndices"/> to map virtual indices to the correct <see cref="_displayCache"/> entry.</remarks>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		int realIndex = sortedIndices != null && e.ItemIndex < sortedIndices.Count
			? sortedIndices[index: e.ItemIndex]
			: e.ItemIndex;
		if (realIndex < 0 || realIndex >= _displayCache.Count)
		{
			logger.Warn(message: $"Invalid realIndex {realIndex} for ListView_RetrieveVirtualItem.");
			e.Item = new ListViewItem(text: "Error");
			return;
		}
		e.Item = CreateListViewItem(logEvent: _displayCache[index: realIndex]);
	}

	/// <summary>Handles the <see cref="ListView.SelectedIndexChanged"/> event and updates the delete-selected button state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Enables or disables the delete-selected button based on whether any items are currently selected in the ListView.</remarks>
	private void ListView_SelectedIndexChanged(object? sender, EventArgs e) => toolStripButtonDeleteSelected.Enabled = listView.SelectedIndices.Count > 0;

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// If there are no items, do not attempt to sort
		if (listView.VirtualListSize == 0)
		{
			logger.Warn(message: "Attempted to sort an empty list view.");
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
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Remove existing sort indicators from the header text
			string headerText = listView.Columns[index: i].Text;
			// Check for existing indicators and remove them
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column
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
		// Initialize sortedIndices if null or count has changed
		int count = _displayCache.Count;
		if (sortedIndices == null || sortedIndices.Count != count)
		{
			sortedIndices = [.. Enumerable.Range(start: 0, count: count)];
		}
		// Precompute sort keys once per index to avoid repeated work during comparison
		Dictionary<int, (bool HasNumeric, long NumericValue, string TextValue)> sortKeyCache = new(capacity: count);
		// Populate the sort key cache for each index in sortedIndices
		foreach (int index in sortedIndices)
		{
			// Skip invalid indices to avoid exceptions
			if (index < 0 || index >= _displayCache.Count)
			{
				logger.Warn(message: $"Invalid index {index} encountered while populating sort key cache.");
				continue;
			}
			// Retrieve the log event for the current index
			LogEventInfo logEvent = _displayCache[index: index];
			string value = sortColumn switch
			{
				0 => FormatTimestamp(timestamp: logEvent.TimeStamp),
				1 => logEvent.Level?.Name ?? string.Empty,
				2 => logEvent.Exception?.GetType().Name
					?? (logEvent.Properties.TryGetValue(key: "ExceptionTypeName", value: out object? stored) ? stored?.ToString() : null)
					?? string.Empty,
				3 => logEvent.FormattedMessage ?? string.Empty,
				_ => string.Empty
			};
			// For the timestamp column use the raw ticks for numeric comparison
			bool hasNumeric = sortColumn == 0 && (hasNumeric: true, numericValue: logEvent.TimeStamp.Ticks) is var _ && true;
			long numericValue = sortColumn == 0 ? logEvent.TimeStamp.Ticks : 0;
			sortKeyCache[key: index] = (hasNumeric, numericValue, value);
		}
		// Sort the indices using the precomputed sort keys
		sortedIndices.Sort(comparison: (a, b) =>
		{
			(bool HasNumeric, long NumericValue, string TextValue) = sortKeyCache.TryGetValue(key: a, value: out (bool HasNumeric, long NumericValue, string TextValue) va) ? va : (false, 0, string.Empty);
			(bool HasNumeric, long NumericValue, string TextValue) kb = sortKeyCache.TryGetValue(key: b, value: out (bool HasNumeric, long NumericValue, string TextValue) vb) ? vb : (false, 0, string.Empty);
			int result = HasNumeric && kb.HasNumeric
				? NumericValue.CompareTo(value: kb.NumericValue)
				: string.Compare(strA: TextValue, strB: kb.TextValue, comparisonType: StringComparison.OrdinalIgnoreCase);
			return sortOrder == SortOrder.Descending ? -result : result;
		});
		// Refresh the ListView to reflect the new order
		listView.Invalidate();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Delete Selected button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Removes all currently selected entries from both the <see cref="ListView"/> display cache and the underlying <see cref="LogEventStore"/>.</remarks>
	private void ToolStripButtonDeleteSelected_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Delete Selected button clicked.");
		// If no items are selected, do nothing
		if (listView.SelectedIndices.Count == 0)
		{
			logger.Warn(message: "Delete Selected button clicked with no selected items.");
			return;
		}
		// Collect selected indices (virtual indices in the current view)
		List<int> selectedViewIndices = [.. listView.SelectedIndices.Cast<int>()];

		// Map view indices to real indices when a sort is active
		List<int> selectedRealIndices = [.. selectedViewIndices
			.Select(i => sortedIndices != null && i >= 0 && i < sortedIndices.Count ? sortedIndices[i] : i)
			.Distinct()
			.OrderByDescending(i => i)];
		// Remove from the backing store (expects indices in snapshot order)
		LogEventStore.RemoveAt(indices: selectedRealIndices);
		// Remove from the local display cache (descending to preserve indices)
		foreach (int index in selectedRealIndices)
		{
			if (index >= 0 && index < _displayCache.Count)
			{
				_displayCache.RemoveAt(index: index);
			}
		}
		// Reset sorting state because index mappings are no longer valid after removals
		sortedIndices = null;
		sortColumn = -1;
		sortOrder = SortOrder.None;
		// Update the ListView virtual size
		listView.VirtualListSize = _displayCache.Count;
		// Clear selection
		listView.SelectedIndices.Clear();
		// Update button states and status bar
		UpdateButtonStates();
		SetStatusBar(label: labelInformation, text: $"{_displayCache.Count} log {(_displayCache.Count == 1 ? "entry" : "entries")} remaining");
	}

	/// <summary>Handles the Click event of the Delete All button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears all entries from both the <see cref="ListView"/> display cache and the underlying <see cref="LogEventStore"/>.</remarks>
	private void ToolStripButtonDeleteAll_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Delete All button clicked.");
		// Clear the backing store
		LogEventStore.Clear();
		// Clear the local display cache
		_displayCache.Clear();
		// Reset the ListView virtual size
		listView.VirtualListSize = 0;
		// Update button states and status bar
		UpdateButtonStates();
		SetStatusBar(label: labelInformation, text: "All log entries deleted");
	}

	#endregion
}
