// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>A form that displays all NLog log events captured during the current application session.</summary>
/// <remarks>
/// The form loads all stored log events from <see cref="LogEventStore"/> asynchronously when it opens.
/// The <see cref="ListView"/> operates in virtual mode for performance.
/// Users can delete selected entries or clear all entries via toolbar buttons.
/// </remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class LogViewerForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for this form.</summary>
	/// <remarks>This logger is used to log events and errors that occur within this form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>In-memory snapshot of <see cref="LogEventInfo"/> objects currently shown in the ListView.</summary>
	/// <remarks>This list is the backing store for virtual-mode item retrieval.</remarks>
	private List<LogEventInfo> _displayCache = [];

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="LogViewerForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public LogViewerForm() =>
		InitializeComponent();

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Formats a <see cref="DateTime"/> value for display in the Date/Time column.</summary>
	/// <param name="timestamp">The timestamp to format.</param>
	/// <returns>A formatted date/time string using <c>yyyy-MM-dd HH:mm:ss.fff</c>.</returns>
	private static string FormatTimestamp(DateTime timestamp) =>
		timestamp.ToString(format: "yyyy-MM-dd HH:mm:ss.fff", provider: CultureInfo.InvariantCulture);

	/// <summary>Creates a <see cref="ListViewItem"/> from a <see cref="LogEventInfo"/> for virtual-mode display.</summary>
	/// <param name="logEvent">The log event to convert into a list item.</param>
	/// <returns>A <see cref="ListViewItem"/> with sub-items for level, exception type, and message.</returns>
	private static ListViewItem CreateListViewItem(LogEventInfo logEvent)
	{
		string timestampText = FormatTimestamp(timestamp: logEvent.TimeStamp);
		string levelText = logEvent.Level?.Name ?? string.Empty;
		string exceptionType = logEvent.Exception?.GetType().Name ?? string.Empty;
		string message = logEvent.FormattedMessage ?? string.Empty;

		ListViewItem item = new(text: timestampText);
		_ = item.SubItems.Add(text: levelText);
		_ = item.SubItems.Add(text: exceptionType);
		_ = item.SubItems.Add(text: message);
		return item;
	}

	/// <summary>Updates the enabled state of the delete buttons based on the current state of the list view.</summary>
	private void UpdateButtonStates()
	{
		toolStripButtonDeleteAll.Enabled = _displayCache.Count > 0;
		toolStripButtonDeleteSelected.Enabled = listView.SelectedIndices.Count > 0;
	}

	/// <summary>Asynchronously loads all log events from <see cref="LogEventStore"/> into the ListView, reporting progress via the toolbar progress bar.</summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

		Stopwatch stopwatch = Stopwatch.StartNew();

		try
		{
			// Take a snapshot of the log event store on a background thread
			// and convert to display entries while reporting progress
			IProgress<int> progress = new Progress<int>(handler: percentage =>
			{
				if (percentage >= 0 && percentage <= 100)
				{
					kryptonProgressBar.Value = percentage;
					kryptonProgressBar.Values.Text = $"{percentage}%";
				}
			});

			List<LogEventInfo> snapshot = await Task.Run(function: () =>
			{
				// Get snapshot from the store
				List<LogEventInfo> events = LogEventStore.GetSnapshot();
				// Report initial progress
				progress.Report(value: 0);
				return events;
			});

			// The snapshot is ready; update the UI-bound cache on the UI thread
			_displayCache = snapshot;

			// Report complete loading
			int total = _displayCache.Count;
			for (int i = 0; i <= 100; i += 10)
			{
				progress.Report(value: i);
				await Task.Delay(millisecondsDelay: 1);
			}

			// Activate virtual mode with the loaded count
			listView.VirtualListSize = total;
			stopwatch.Stop();

			// Show completion message
			_ = KryptonMessageBox.Show(
				owner: this,
				text: $"{total} log {(total == 1 ? "entry" : "entries")} loaded in {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff} hh:mm:ss.fff.",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information);

			SetStatusBar(label: labelInformation, text: $"{total} log {(total == 1 ? "entry" : "entries")} loaded");
		}
		catch (Exception ex)
		{
			stopwatch.Stop();
			logger.Error(exception: ex, message: "An error occurred while loading log events.");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: $"An error occurred while loading log events: {ex.Message}",
				caption: I18nStrings.ErrorCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
			SetStatusBar(label: labelInformation, text: "Error loading log events");
		}
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
	private async void LogViewerForm_Load(object sender, EventArgs e) =>
		await LoadLogEventsAsync();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the <see cref="ListView.RetrieveVirtualItem"/> event to supply list items on demand.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance that contains the item index.</param>
	/// <remarks>Provides a <see cref="ListViewItem"/> created from the cached <see cref="LogEventInfo"/> at the requested index.</remarks>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		if (e.ItemIndex >= 0 && e.ItemIndex < _displayCache.Count)
		{
			e.Item = CreateListViewItem(logEvent: _displayCache[index: e.ItemIndex]);
		}
		else
		{
			e.Item = new ListViewItem(text: string.Empty);
		}
	}

	/// <summary>Handles the <see cref="ListView.SelectedIndexChanged"/> event and updates the delete-selected button state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void ListView_SelectedIndexChanged(object? sender, EventArgs e) =>
		toolStripButtonDeleteSelected.Enabled = listView.SelectedIndices.Count > 0;

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Delete Selected button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Removes all currently selected entries from both the <see cref="ListView"/> display cache
	/// and the underlying <see cref="LogEventStore"/>.
	/// </remarks>
	private void ToolStripButtonDeleteSelected_Click(object sender, EventArgs e)
	{
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}

		// Collect selected indices
		List<int> selectedIndices = [.. listView.SelectedIndices.Cast<int>()];

		// Remove from the backing store
		LogEventStore.RemoveAt(indices: selectedIndices);

		// Remove from the local display cache (descending to preserve indices)
		foreach (int index in selectedIndices.OrderByDescending(keySelector: i => i))
		{
			if (index >= 0 && index < _displayCache.Count)
			{
				_displayCache.RemoveAt(index: index);
			}
		}

		// Update the ListView virtual size
		listView.VirtualListSize = _displayCache.Count;
		// Clear selection
		listView.SelectedIndices.Clear();

		UpdateButtonStates();
		SetStatusBar(label: labelInformation, text: $"{_displayCache.Count} log {(_displayCache.Count == 1 ? "entry" : "entries")} remaining");
	}

	/// <summary>Handles the Click event of the Delete All button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Clears all entries from both the <see cref="ListView"/> display cache and the underlying <see cref="LogEventStore"/>.
	/// </remarks>
	private void ToolStripButtonDeleteAll_Click(object sender, EventArgs e)
	{
		// Clear the backing store
		LogEventStore.Clear();
		// Clear the local display cache
		_displayCache.Clear();
		// Reset the ListView virtual size
		listView.VirtualListSize = 0;

		UpdateButtonStates();
		SetStatusBar(label: labelInformation, text: "All log entries deleted");
	}

	#endregion
}
