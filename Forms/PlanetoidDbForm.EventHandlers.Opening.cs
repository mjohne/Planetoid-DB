/*
 * File:        PlanetoidDbForm.EventHandlers.Opening.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing Opening event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all Opening event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the Opening event of the ListView context menu. Disables items if no row is selected.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ListViewContextMenu_Opening(object? sender, CancelEventArgs e)
	{
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Cancel menu opening if no item is selected in VirtualMode
		if (listViewReadableDesignations.SelectedIndices.Count == 0)
		{
			e.Cancel = true;
		}
	}
}
