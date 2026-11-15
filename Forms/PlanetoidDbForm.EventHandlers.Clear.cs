/*
 * File:        PlanetoidDbForm.EventHandlers.Clear.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing clear event handler methods for the PlanetoidDbForm.
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
/// <remarks>This file contains all event handlers including form lifecycle events, Click handlers, BackgroundWorker events, Timer events, and UI interaction handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Clears the checked state of all navigation step menu items.</summary>
	/// <remarks>This method is used to clear the checked state of all navigation step menu items.</remarks>
	private void ToolStripMenuItem_Clear()
	{
		// Clear the checked state of all navigation step menu items
		toolStripMenuItemNavigateStep10.Checked = false;
		toolStripMenuItemNavigateStep100.Checked = false;
		toolStripMenuItemNavigateStep1000.Checked = false;
		toolStripMenuItemNavigateStep10000.Checked = false;
		toolStripMenuItemNavigateStep100000.Checked = false;
	}
}
