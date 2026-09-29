/*
 * File:        OrbitElementsGroupingForm.PlanetoidData.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Form to analyze and group planetoids based on common orbital element ranges.
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

/// <summary>Form to analyze and group planetoids based on common orbital element ranges.</summary>
/// <remarks>This form provides functionality to group planetoids based on their orbital elements, allowing for analysis of patterns and similarities.</remarks>
internal partial class OrbitElementsGroupingForm
{
	/// <summary>Represents immutable data for a planetoid, including its identifier, name, and associated orbital elements.</summary>
	/// <param name="Index">The unique identifier or catalog index for the planetoid.</param>
	/// <param name="Name">The name of the planetoid.</param>
	/// <param name="Elements">An array of double values representing the orbital elements of the planetoid. The array must not be null.</param>
	/// <remarks>This record is used to store and manage the relevant data for each planetoid during the grouping process. The Elements array typically includes values such as mean anomaly, argument of perihelion, longitude of ascending node, inclination, orbital eccentricity, motion, and semi-major axis.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private record PlanetoidData(string Index, string Name, double[] Elements)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}