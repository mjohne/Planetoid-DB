/*
 * File:        CitiesForm.CityListEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Form to display a virtualized, collapsible tree-like list of cities grouped by state and return selected geographic coordinates.
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

namespace Planetoid_DB.Forms;

/// <summary>Provides a form for browsing, filtering, and selecting city and state geographic data.</summary>
/// <remarks>The form uses a virtualized list to organize cities under collapsible state nodes and provides geographic coordinates for each city entry.</remarks>
internal partial class CitiesForm
{
	/// <summary>Represents a single visible row in the virtualized tree list, either a collapsible state node or a detailed city row.</summary>
	/// <remarks>Used as the backing model for <see cref="ListView.RetrieveVirtualItem"/>.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed class CityListEntry
	{
		/// <summary>Gets a value indicating whether this entry represents a collapsible state node.</summary>
		/// <remarks>State nodes toggle expansion on activation rather than committing coordinates.</remarks>
		public bool IsStateNode { get; init; }

		/// <summary>Gets the owning <see cref="StateGroupNode"/> for state rows or the parent state node for city rows.</summary>
		/// <remarks>Enables toggling expansion and navigating back from a city row to its parent state node.</remarks>
		public StateGroupNode? ParentGroup { get; init; }

		/// <summary>Gets the name of the state or territory.</summary>
		/// <remarks>Displayed on state nodes and included in status bar and tooltip details for city entries.</remarks>
		public string State { get; init; } = string.Empty;

		/// <summary>Gets the name of the city.</summary>
		/// <remarks>Empty for state nodes.</remarks>
		public string City { get; init; } = string.Empty;

		/// <summary>Gets the geographic latitude in decimal degrees.</summary>
		/// <remarks>Positive values denote north; negative values denote south. Zero for state nodes.</remarks>
		public double Latitude { get; init; }

		/// <summary>Gets the geographic longitude in decimal degrees.</summary>
		/// <remarks>Positive values denote east; negative values denote west. Zero for state nodes.</remarks>
		public double Longitude { get; init; }

		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
