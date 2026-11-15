/*
 * File:        PlanetoidDbForm.EventHandlers.RetrieveVirtualItem.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing RetrieveVirtualItem event handler methods for the PlanetoidDbForm.
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
/// <remarks>This file contains all RetrieveVirtualItem event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Provides the virtual list item for the requested index, including its readable designation.</summary>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data containing the requested item index and receiving the virtual item.</param>
	private void ListViewReadableDesignations_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		int index = e.ItemIndex;
		if (index >= 0 && index < planetoidsDatabase.Count)
		{
			string entryStr = planetoidsDatabase[index: index];
			string mpcIndex = entryStr.Length >= mpcorbDatReadableDesignationDelimiterEnd ? entryStr[..7].Trim() : string.Empty;
			string readableDesignation = entryStr.Length >= mpcorbDatReadableDesignationDelimiterEnd ? entryStr[mpcorbDatReadableDesignationDelimiterBegin..mpcorbDatReadableDesignationDelimiterEnd].Trim() : string.Empty;
			ListViewItem item = new(text: (index + 1).ToString(provider: CultureInfo.InvariantCulture));
			_ = item.SubItems.Add(text: mpcIndex);
			_ = item.SubItems.Add(text: readableDesignation);
			e.Item = item;
		}
		else
		{
			logger.Warn($"Empty list view item at {index + 1}.");
			e.Item = new ListViewItem(items: [string.Empty, string.Empty]);
		}
	}
}
