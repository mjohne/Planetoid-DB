/*
 * File:        PlanetoidDbForm.EventHandlers.DropDown.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing DropDown event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all DropDown event handlers.</remarks>
public partial class PlanetoidDbForm
{
	#region History DropDownOpening

	/// <summary>Handles the DropDownOpening event for the back history split button. Populates the drop-down menu with the back-history entries.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The drop-down lists the positions that can be reached by clicking back, from most recent to oldest. Clicking a menu item navigates directly to that position and adjusts the history stacks accordingly.</remarks>
	private void HistoryBack_DropDownOpening(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Clear existing items in the back-history drop-down menu
		toolStripSplitButtonHistoryBack.DropDownItems.Clear();
		// Build items from the back-history (top = most recent)
		int[] backPositions = [.. navigationHistoryBack];
		// Iterate through each position in the back-history
		foreach (int pos in backPositions)
		{
			// Capture the current position in a local variable to avoid closure issues
			int capturedPos = pos;
			// Get the planetoid name at the captured position
			string name = GetPlanetoidNameAtPosition(position: capturedPos);
			// Create a label for the menu item, using the planetoid name if available, otherwise using the index
			string label = string.IsNullOrWhiteSpace(value: name) ? $"{I18nStrings.Index}: {capturedPos + 1}" : name;
			// Create a new ToolStripMenuItem with the label and store the captured position in the Tag property
			ToolStripMenuItem item = new(text: label)
			{
				Tag = capturedPos,
				AutoToolTip = true,
				AccessibleRole = AccessibleRole.MenuItem,
				AccessibleName = label,
				AccessibleDescription = $"Navigate to history position {capturedPos + 1} ({label})"
			};
			// Handle the click event for the history item
			item.Click += (s, ev) =>
			{
				// Navigate directly to the chosen history position
				NavigateHistoryToPosition(targetPosition: capturedPos, fromBack: true);
			};
			// Add mouse enter and leave event handlers for visual feedback
			item.MouseEnter += Control_Enter;
			item.MouseLeave += Control_Leave;
			// Add the item to the back-history drop-down menu
			_ = toolStripSplitButtonHistoryBack.DropDownItems.Add(value: item);
		}
	}

	/// <summary>Handles the DropDownOpening event for the forward history split button. Populates the drop-down menu with the forward-history entries.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The drop-down lists the positions that can be reached by clicking forward, from most recent to oldest. Clicking a menu item navigates directly to that position and adjusts the history stacks accordingly.</remarks>
	private void HistoryForward_DropDownOpening(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Clear existing items in the forward-history drop-down menu
		toolStripSplitButtonHistoryForward.DropDownItems.Clear();
		// Build items from the forward-history (top = most recent)
		int[] forwardPositions = [.. navigationHistoryForward];
		// Iterate through each position in the forward-history
		foreach (int pos in forwardPositions)
		{
			// Capture the current position in a local variable to avoid closure issues
			int capturedPos = pos;
			// Get the planetoid name at the captured position
			string name = GetPlanetoidNameAtPosition(position: capturedPos);
			// Create a label for the menu item, using the planetoid name if available, otherwise using the index
			string label = string.IsNullOrWhiteSpace(value: name) ? $"{I18nStrings.Index}: {capturedPos + 1}" : name;
			// Create a new ToolStripMenuItem with the label and store the captured position in the Tag property
			ToolStripMenuItem item = new(text: label)
			{
				Tag = capturedPos,
				AutoToolTip = true,
				AccessibleRole = AccessibleRole.MenuItem,
				AccessibleName = label,
				AccessibleDescription = $"Navigate to history position {capturedPos + 1} ({label})"
			};
			// Handle the click event for the history item
			item.Click += (s, ev) =>
			{
				// Navigate directly to the chosen history position
				NavigateHistoryToPosition(targetPosition: capturedPos, fromBack: false);
			};
			// Add mouse enter and leave event handlers for visual feedback
			item.MouseEnter += Control_Enter;
			item.MouseLeave += Control_Leave;
			// Add the item to the forward-history drop-down menu
			_ = toolStripSplitButtonHistoryForward.DropDownItems.Add(value: item);
		}
	}

	#endregion

	#region Bookmark DropDownOpening

	/// <summary>Populates the MPCORB.DAT bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>
	/// <remarks>This method is called when the user opens the MPCORB.DAT bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for MPCORB.DAT, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListMpcorbDat_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the MPCORB.DAT bookmark drop-down
		logger.Info(message: "Populating MPCORB.DAT bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for MPCORB.DAT
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListMpcorbDat, bookmarks: bookmarksMpcorbDat, onClickHandler: BookmarkNavigateMpcorbDat_Click);
	}

	/// <summary>Populates the MPCORB.JSON bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>												   
	/// <remarks>This method is called when the user opens the MPCORB.JSON bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for MPCORB.JSON, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListMpcorbJson_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the MPCORB.JSON bookmark drop-down
		logger.Info(message: "Populating MPCORB.JSON bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for MPCORB.JSON
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListMpcorbJson, bookmarks: bookmarksMpcorbJson, onClickHandler: BookmarkNavigateMpcorbJson_Click);
	}

	/// <summary>Populates the ASTORB.DAT bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>
	/// <remarks>This method is called when the user opens the ASTORB.DAT bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for ASTORB.DAT, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListAstorbDat_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the ASTORB.DAT bookmark drop-down
		logger.Info(message: "Populating ASTORB.DAT bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for ASTORB.DAT
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListAstorbDat, bookmarks: bookmarksAstorbDat, onClickHandler: BookmarkNavigateAstorbDat_Click);
	}

	/// <summary>Populates the ALLNUM.CAT bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>
	/// <remarks>This method is called when the user opens the ALLNUM.CAT bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for ALLNUM.CAT, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListAllnumCat_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the ALLNUM.CAT bookmark drop-down
		logger.Info(message: "Populating ALLNUM.CAT bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for ALLNUM.CAT
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListAllnumCat, bookmarks: bookmarksAllnumCat, onClickHandler: BookmarkNavigateAllnumCat_Click);
	}

	/// <summary>Populates the SINGOPP.CAT bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>
	/// <remarks>This method is called when the user opens the SINGOPP.CAT bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for SINGOPP.CAT, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListSingoppCat_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the SINGOPP.CAT bookmark drop-down
		logger.Info(message: "Populating SINGOPP.CAT bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for SINGOPP.CAT
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListSingoppCat, bookmarks: bookmarksSingoppCat, onClickHandler: BookmarkNavigateSingoppCat_Click);
	}

	/// <summary>Populates the UFITOBS.CAT bookmark drop-down when the user opens it.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event data.</param>
	/// <remarks>This method is called when the user opens the UFITOBS.CAT bookmark drop-down menu. It populates the drop-down with the current list of bookmarks for UFITOBS.CAT, creating a menu item for each bookmark entry and associating it with the appropriate click event handler.</remarks>
	private void BookmarkListUfitobsCat_DropDownOpening(object? sender, EventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the action of populating the UFITOBS.CAT bookmark drop-down
		logger.Info(message: "Populating UFITOBS.CAT bookmark drop-down...");
		// Call the helper method to populate the drop-down with the current bookmarks for UFITOBS.CAT
		PopulateBookmarkDropDown(parentItem: toolStripMenuItemBookmarkListUfitobsCat, bookmarks: bookmarksUfitobsCat, onClickHandler: BookmarkNavigateUfitobsCat_Click);
	}

	#endregion
}
