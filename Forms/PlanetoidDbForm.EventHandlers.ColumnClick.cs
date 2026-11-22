/*
 * File:        PlanetoidDbForm.EventHandlers.ColumnClick.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing ColumnClick event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all ColumnClick event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListViewReadableDesignations_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		logger.Info(message: "Column on ListViewReadableDesignations clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		ShowErrorMessage(message: "Sorting is not possible in this table!");
	}

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListViewDerivedData_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		logger.Info(message: "Column on ListViewDerivedData clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// If there are no items, do not attempt to sort
		if (listViewDerivedData.Items.Count == 0)
		{
			// No items to sort, ignore the request.
			logger.Debug(message: "Column click ignored because the ListView is empty.");
			// No items to sort, exit the method
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
		for (int i = 0; i < listViewDerivedData.Columns.Count; i++)
		{
			// Remove existing sort indicators from the header text
			string headerText = listViewDerivedData.Columns[index: i].Text;
			// Check for existing indicators and remove them
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.CurrentCulture) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.CurrentCulture))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listViewDerivedData.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// For other columns, just update the text without indicators
			else
			{
				listViewDerivedData.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort using a standard IComparer
		listViewDerivedData.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listViewDerivedData.Sort();
		logger.Info(message: "ListViewDerivedData sorted.");
	}
}
