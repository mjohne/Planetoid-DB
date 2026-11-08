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
	/// <summary>Handles the DropDownOpening event for the back history split button. Populates the drop-down menu with the back-history entries.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The drop-down lists the positions that can be reached by clicking back, from most recent to oldest. Clicking a menu item navigates directly to that position and adjusts the history stacks accordingly.</remarks>
	private void HistoryBack_DropDownOpening(object sender, EventArgs e)
	{
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
}
