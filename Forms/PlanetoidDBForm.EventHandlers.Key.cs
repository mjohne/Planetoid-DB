/*
 * File:        PlanetoidDbForm.EventHandlers.Key.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing key event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all key event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the KeyPress event for the ToolStripTextBoxGotoIndex. Ensures only numeric input and handles the Enter key to trigger navigation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="KeyPressEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to ensure only numeric input is allowed in the ToolStripTextBoxGotoIndex.</remarks>
	private void GotoIndex_KeyPress(object sender, KeyPressEventArgs e)
	{
		// Check if the pressed key is a control character or a digit
		if (!char.IsControl(c: e.KeyChar) && !char.IsDigit(c: e.KeyChar))
		{
			// If the pressed key is not a digit or control character, suppress the key event
			e.Handled = true;
		}
		// Check if the pressed key is a digit or control character
		if (e.KeyChar == Convert.ToChar(value: Keys.Return, provider: CultureInfo.CurrentCulture))
		{
			// If the Enter key is pressed, trigger the click event for the ToolStripButtonGoToIndex
			GoToIndex_Click(sender: null, e: null);
		}
	}
}
