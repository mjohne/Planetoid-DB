/*
 * File:        PlanetoidDbForm.EventHandlers.ButtonClick.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing ButtonClick event handler methods for the PlanetoidDbForm.
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
/// <remarks>This file contains all ButtonClick event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the ButtonClick event for the back history split button. Navigates one step back in the planetoid visit history.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clicking the button part of the split button navigates one step backward in the history.</remarks>
	private void HistoryBack_ButtonClick(object sender, EventArgs e)
	{
		logger.Info(message: "Navigating back in the planetoid history");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		NavigateHistoryBack();
	}

	/// <summary>Handles the ButtonClick event for the forward history split button. Navigates one step forward in the planetoid visit history.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clicking the button part of the split button navigates one step forward in the history.</remarks>
	private void HistoryForward_ButtonClick(object sender, EventArgs e)
	{
		logger.Info(message: "Navigating forward in the planetoid history");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		NavigateHistoryForward();
	}
}
