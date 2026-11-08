/*
 * File:        PlanetoidDbForm.EventHandlers.SelectedIndexChanged.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing SelectedIndexChanged event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all SelectedIndexChanged event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Restarts the selection debounce timer.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ListViewReadableDesignations_SelectedIndexChanged(object sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: sender);
		ArgumentNullException.ThrowIfNull(argument: e);
		// Reset the timer as long as the user continues to click or scroll
		timerSelectionDebounce.Stop();
		timerSelectionDebounce.Start();
	}
}
