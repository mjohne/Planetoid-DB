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
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Reset the timer as long as the user continues to click or scroll
		timerSelectionDebounce.Stop();
		timerSelectionDebounce.Start();

		/*
		// Check if any row is selected in VirtualMode using SelectedIndices
		if (listViewReadableDesignations.SelectedIndices.Count > 0)
		{
			int selectedIndex = listViewReadableDesignations.SelectedIndices[index: 0];
			// Validate bounds against planetoidsDatabase
			if (selectedIndex >= 0 && selectedIndex < planetoidsDatabase.Count)
			{
				// Avoid redundant navigation if clicking the already active item
				if (selectedIndex == currentMpcorbPosition)
				{
					return;
				}
				logger.Info(message: $"ListViewReadableDesignations clicked. Navigating to position: {selectedIndex + 1}");
				// Retrieve the entry from the underlying data source
				string entryStr = planetoidsDatabase[index: selectedIndex];
				string planetoidReadableDesignation = entryStr.Length >= mpcorbDatReadableDesignationDelimiterBegin ? entryStr[mpcorbDatReadableDesignationDelimiterBegin..mpcorbDatReadableDesignationDelimiterEnd].Trim() : string.Empty;
				labelInformation.Text = $"Selected item: {selectedIndex + 1} - {planetoidReadableDesignation}";

				// Push current position to navigation history before changing position
				PushNavigationHistory(previousPosition: currentMpcorbPosition);

				// Update positions and navigate across all loaded databases
				currentMpcorbPosition = selectedIndex;
				GotoCurrentMpcorbPosition(position: currentMpcorbPosition);
				
				currentAstorbPosition = currentMpcorbPosition;
				GotoCurrentAstorbPosition(position: currentAstorbPosition);

				currentMpcorbJsonPosition = currentMpcorbPosition;
				GotoCurrentMpcorbJsonPosition(position: currentMpcorbJsonPosition);

				currentAllnumCatPosition = currentMpcorbPosition;
				GotoCurrentAllnumCatPosition(position: currentAllnumCatPosition);

				currentSingoppCatPosition = currentMpcorbPosition;
				GotoCurrentSingoppCatPosition(position: currentSingoppCatPosition);

				currentUfitobsCatPosition = currentMpcorbPosition;
				GotoCurrentUfitobsCatPosition(position: currentUfitobsCatPosition);
				
			}
		}
		*/
	}
}
