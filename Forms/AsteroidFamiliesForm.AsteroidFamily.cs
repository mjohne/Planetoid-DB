/*
 * File:        AsteroidFamiliesForm.AsteroidFamily.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form to detect and display potential asteroid families based on orbital elements (a, e, i). Uses a binning algorithm to group planetoids whose semi-major axis, eccentricity, and inclination fall within user-defined tolerance ranges.
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

/// <summary>Represents the form for detecting and displaying potential asteroid families based on orbital elements (a, e, i).</summary>
/// <remarks>This form uses a binning algorithm to group planetoids whose semi-major axis, eccentricity, and inclination fall within user-defined tolerance ranges.</remarks>
internal partial class AsteroidFamiliesForm
{
	/// <summary>Represents a detected asteroid family with its member list.</summary>
	/// <remarks>This class is used to store the members of a detected asteroid family.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed class AsteroidFamily
	{
		/// <summary>Gets or sets the display name of this family.</summary>
		/// <remarks>This property is used to store the name of the asteroid family.</remarks>
		public string Name { get; set; } = string.Empty;

		/// <summary>Gets the list of member planetoids.</summary>							  
		/// <remarks>This list contains all planetoids that belong to this family.</remarks>
		public List<PlanetoidEntry> Members { get; } = [];

		/// <summary>Gets a string representation of the asteroid family for debugging purposes.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}