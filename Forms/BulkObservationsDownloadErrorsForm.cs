/*
 * File:        BulkObservationsDownloadErrorsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Dialog form that displays detailed bulk-download errors in a list view.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Dialog form that displays detailed bulk-download errors in a list view.</summary>
/// <remarks>This form is used to present a list of errors that occurred during the bulk download process.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed partial class BulkObservationsDownloadErrorsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of bulk download errors";

	/// <summary>Prepares the save dialog used for export operations.</summary>
	/// <param name="dialog">The dialog to configure before it is displayed.</param>
	/// <param name="ext">The file extension selected for the export.</param>
	/// <returns><see langword="true"/> if the user confirms the dialog; otherwise, <see langword="false"/>.</returns>
	/// <remarks>Overrides the default file naming to preserve the selected minimum and maximum range in the export file name.</remarks>
	protected override bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		dialog.FileName = $"Bulk-Download-Error-List.{ext}";
		return dialog.ShowDialog(owner: this) == DialogResult.OK;
	}

	#endregion

	#region Fields and Properties

	/// <summary>List of bulk download error entries.</summary>
	/// <remarks>This list contains all the error records passed to the form.</remarks>
	private readonly IReadOnlyList<BulkObservationsDownloadErrorEntry> errorEntries = [];

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the <see cref="BulkObservationsDownloadErrorsForm"/> class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>This field stores the index of the currently sorted column.</remarks>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered.</summary>
	/// <remarks>This field stores the current sort order of the list view.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Stores the sorted indices for virtual mode to maintain sorting order.</summary>
	/// <remarks>This list maps the virtual list view indices to the actual database indices based on the current sort criteria.</remarks>
	private List<int>? sortedIndices;

	#endregion

	#region Helpers

	/// <summary>Returns a string representation of the current object for debugging purposes.</summary>
	/// <returns>A string representation of the current object.</returns>
	/// <remarks>This property is used by the debugger to display a concise representation of the object.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Gets the currently selected database index, considering virtual mode and sorting.</summary>
	/// <returns>The actual database index corresponding to the currently selected item in the ListView, or <c>null</c> if no item is selected.</returns>
	/// <remarks>This method returns the actual index corresponding to the currently selected item in the ListView, taking into account any sorting.</remarks>
	private int? GetSelectedDatabaseIndex()
	{
		if (listView.SelectedIndices.Count == 0)
		{
			logger.Warn(message: "No item is selected in the list view when attempting to get the selected database index.");
			return null;
		}
		int virtualIdx = listView.SelectedIndices[index: 0];
		return sortedIndices != null && virtualIdx < sortedIndices.Count ? sortedIndices[index: virtualIdx] : virtualIdx;
	}

	/// <summary>Restores the selection in the ListView based on the provided database index.</summary>
	/// <param name="selectedDbIndex">The database index of the item to select.</param>
	/// <remarks>This method restores the selection in the ListView based on the provided database index, taking into account any sorting.</remarks>
	private void RestoreSelection(int? selectedDbIndex)
	{
		if (!selectedDbIndex.HasValue)
		{
			logger.Warn(message: "No database index provided to restore selection.");
			return;
		}
		int newVirtualIndex = sortedIndices != null ? sortedIndices.IndexOf(item: selectedDbIndex.Value) : selectedDbIndex.Value;
		if (newVirtualIndex >= 0 && newVirtualIndex < listView.VirtualListSize)
		{
			listView.SelectedIndices.Clear();
			_ = listView.SelectedIndices.Add(itemIndex: newVirtualIndex);
			listView.EnsureVisible(index: newVirtualIndex);
		}
	}

	/// <summary>Updates the column headers of the ListView to reflect the current sort order. Adds an ascending or descending indicator to the sorted column header and removes any indicators from other columns.</summary>
	/// <remarks>This method is called after sorting to visually indicate which column is currently sorted and in which order.</remarks>
	private void UpdateColumnHeaders()
	{
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text.TrimStart('▲', '▼', ' ');
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
	}

	/// <summary>Sorts the virtual list based on the current sort column and order. This method updates the sortedIndices list to reflect the new order of items in the ListView, preserving the selection if possible.</summary>
	/// <remarks>This method is called when the user clicks a column header to sort the list.</remarks>
	private void SortVirtualList()
	{
		if (listView.VirtualListSize == 0)
		{
			logger.Warn(message: "Attempted to sort an empty virtual list.");
			return;
		}
		int count = listView.VirtualListSize;
		if (sortedIndices == null || sortedIndices.Count != count)
		{
			sortedIndices = [.. Enumerable.Range(start: 0, count: count)];
		}
		int? selectedDbIndex = GetSelectedDatabaseIndex();
		sortedIndices.Sort(comparison: (a, b) =>
		{
			BulkObservationsDownloadErrorEntry entryA = errorEntries[index: a];
			BulkObservationsDownloadErrorEntry entryB = errorEntries[index: b];
			int result = sortColumn switch
			{
				0 => entryA.Timestamp.CompareTo(value: entryB.Timestamp),
				1 => string.Compare(strA: entryA.Url, strB: entryB.Url, comparisonType: StringComparison.OrdinalIgnoreCase),
				2 => string.Compare(
					strA: $"{entryA.ErrorType}: {entryA.ErrorDescription}",
					strB: $"{entryB.ErrorType}: {entryB.ErrorDescription}",
					comparisonType: StringComparison.OrdinalIgnoreCase),
				_ => 0
			};
			return sortOrder == SortOrder.Descending ? -result : result;
		});
		RestoreSelection(selectedDbIndex: selectedDbIndex);
		listView.Invalidate();
		logger.Info(message: $"Virtual list sorted by column {sortColumn} in {sortOrder} order.");
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="BulkObservationsDownloadErrorsForm"/> class.</summary>
	/// <param name="entries">The error entries to display.</param>
	/// <remarks>Configures the form's appearance and sets up the virtual list view with the provided error entries.</remarks>
	internal BulkObservationsDownloadErrorsForm(IReadOnlyList<BulkObservationsDownloadErrorEntry> entries)
	{
		logger.Info(message: "Initializing BulkObservationsDownloadErrorsForm...");
		ArgumentNullException.ThrowIfNull(argument: entries);
		InitializeComponent();

		errorEntries = entries;
		kryptonStatusStrip.Text = $"{errorEntries.Count} error(s)";

		listView.BeginUpdate();
		listView.VirtualMode = true;
		listView.VirtualListSize = errorEntries.Count;
		listView.EndUpdate();

		logger.Info(message: "BulkObservationsDownloadErrorsForm initialized.");
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		logger.Info(message: $"Column {e.Column} clicked for sorting.");
		if (listView.VirtualListSize == 0)
		{
			logger.Warn(message: "Attempted to sort an empty virtual list on column click.");
			return;
		}
		sortOrder = (e.Column == sortColumn && sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
		sortColumn = e.Column;
		logger.Info(message: $"Sorting by column {sortColumn} in {sortOrder} order.");
		UpdateColumnHeaders();
		SortVirtualList();
	}

	/// <summary>Handles the retrieval of virtual items for the ListView. Dynamically creates ListViewItems when they are needed for display.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to retrieve virtual items for the ListView.</remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: sender);
		ArgumentNullException.ThrowIfNull(argument: e);

		int realIndex = sortedIndices != null && e.ItemIndex < sortedIndices.Count
			? sortedIndices[index: e.ItemIndex]
			: e.ItemIndex;

		if (realIndex >= 0 && realIndex < errorEntries.Count)
		{
			BulkObservationsDownloadErrorEntry entry = errorEntries[index: realIndex];
			string timestampText = entry.Timestamp.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.CurrentCulture);
			string errorText = $"{entry.ErrorType}: {entry.ErrorDescription}";

			ListViewItem item = new(text: timestampText);
			_ = item.SubItems.Add(text: entry.Url);
			_ = item.SubItems.Add(text: errorText);
			e.Item = item;
			return;
		}

		logger.Error($"Failed to retrieve virtual item for index {e.ItemIndex}. Real index: {realIndex}. Database count: {errorEntries.Count}.");
		e.Item = new ListViewItem(text: "Error");
		_ = e.Item.SubItems.Add(text: string.Empty);
		_ = e.Item.SubItems.Add(text: "Invalid Data");
	}

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event. Updates the status bar with the selected error details.</summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the SelectedIndexChanged event of the ListView.</remarks>
	private void SelectedIndexChanged(object? sender, EventArgs? e)
	{
		int? dbIndex = GetSelectedDatabaseIndex();
		if (dbIndex.HasValue && dbIndex.Value >= 0 && dbIndex.Value < errorEntries.Count)
		{
			BulkObservationsDownloadErrorEntry entry = errorEntries[index: dbIndex.Value];
			SetStatusBar(labelInformation, $"{entry.Url} - {entry.ErrorDescription}");
			return;
		}
		SetStatusBar(labelInformation, string.Empty);
	}

	#endregion

	#region Form event handlers

	private void BulkObservationsDownloadErrorsForm_Load(object sender, EventArgs e)
	{
		logger.Info(message: "BulkObservationsDownloadErrorsForm is loading.");
		AudioPlayback.PlayOpenSound();
	}

	private void BulkObservationsDownloadErrorsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: sender);
		ArgumentNullException.ThrowIfNull(argument: e);
		logger.Info(message: "BulkObservationsDownloadErrorsForm is closing.");
		AudioPlayback.PlayCloseSound();
	}

	#endregion
}