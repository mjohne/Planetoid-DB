/*
 * File:        CitiesForm.StateGroupNode.cs
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

/// <summary>Displays and manages city entries grouped by state or territory.</summary>
internal partial class CitiesForm
{
	/// <summary>Represents a collapsible state node containing its child city entries.</summary>
	/// <remarks>Maintains the expansion state and sorted city list for a single state or territory.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed class StateGroupNode
	{
		/// <summary>Gets the name of the state or territory.</summary>
		/// <remarks>Used as the display label for the collapsible state node.</remarks>
		public string State { get; init; } = string.Empty;

		/// <summary>Gets or sets a value indicating whether the state node is currently expanded.</summary>
		/// <remarks>Defaults to <see langword="false"/> so all states start collapsed.</remarks>
		public bool IsExpanded { get; set; }

		/// <summary>Gets the list of city entries belonging to this state.</summary>
		/// <remarks>Sorted according to the active city-level column sort criteria.</remarks>
		public List<CityListEntry> Cities { get; } = [];

		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
