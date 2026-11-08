/*
 * File:        PlanetoidDbForm.EventHandlers.DoubleClick.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing DoubleClick event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using Planetoid_DB.Properties;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all DoubleClick event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog for the corresponding entry if successful.</remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Get the current tag text from the sender control
		logger.Info(message: $"Double-click event triggered on control '{sender?.GetType().Name}' to open terminology dialog.");
		// Try to parse the index from the current tag text
		if (TryParseInt(input: currentTagText, value: out int index, errorMessage: out string errorMessage))
		{
			// Open the terminology dialog for the parsed index
			OpenTerminology(index: (uint)index);
			return;
		}
		// Log the error and show an error message
		logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
	}

	/// <summary>Handles the double-click event on the "Load Additional Databases on Startup" menu item.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method sets the checked state of the menu items to true and updates the visibility of the corresponding pages based on the checked state.</remarks>
	private void ToolStripMenuItemLoadAdditionalDatabasesOnStartup_DoubleClick(object sender, EventArgs e)
	{
		// Log the action of loading additional databases on startup via double-click
		logger.Info(message: "Loading additional databases on startup via double-click.");
		// Set the checked state of the menu items to true for loading additional databases on startup
		toolStripMenuItemLoadDatabaseOnStartupMpcorbJson.Checked = true;
		toolStripMenuItemLoadDatabaseOnStartupAstorbDat.Checked = true;
		toolStripMenuItemLoadDatabaseOnStartupAllnumCat.Checked = true;
		toolStripMenuItemLoadDatabaseOnStartupSingoppCat.Checked = true;
		toolStripMenuItemLoadDatabaseOnStartupUfitobsCat.Checked = true;
		// Update the visibility of the corresponding pages based on the checked state of the menu items
		kryptonPageMpcorbJson.Visible = toolStripMenuItemLoadDatabaseOnStartupMpcorbJson.Checked;
		kryptonPageAstorbDat.Visible = toolStripMenuItemLoadDatabaseOnStartupAstorbDat.Checked;
		kryptonPageAllnumCat.Visible = toolStripMenuItemLoadDatabaseOnStartupAllnumCat.Checked;
		kryptonPageSingoppCat.Visible = toolStripMenuItemLoadDatabaseOnStartupSingoppCat.Checked;
		kryptonPageUfitobsCat.Visible = toolStripMenuItemLoadDatabaseOnStartupUfitobsCat.Checked;
		// Save the user preferences for loading additional databases on startup
		Settings.Default.userLoadAdditionalDatabaseOnStartupMpcorbJson = toolStripMenuItemLoadDatabaseOnStartupMpcorbJson.Checked;
		Settings.Default.userLoadAdditionalDatabaseOnStartupAstorbDat = toolStripMenuItemLoadDatabaseOnStartupAstorbDat.Checked;
		Settings.Default.userLoadAdditionalDatabaseOnStartupAllnumCat = toolStripMenuItemLoadDatabaseOnStartupAllnumCat.Checked;
		Settings.Default.userLoadAdditionalDatabaseOnStartupSingoppCat = toolStripMenuItemLoadDatabaseOnStartupSingoppCat.Checked;
		Settings.Default.userLoadAdditionalDatabaseOnStartupUfitobsCat = toolStripMenuItemLoadDatabaseOnStartupUfitobsCat.Checked;
		Settings.Default.Save();
	}

	/// <summary>Handles the click event to show an Easter egg message.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show an Easter egg message when the user double-clicks on a control.</remarks>
	private void EasterEgg_DoubleClick(object sender, EventArgs e)
	{
		_ = KryptonMessageBox.Show(owner: this, text: I18nStrings.EasterEgg, caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}
}
