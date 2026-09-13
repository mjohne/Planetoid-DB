/*
 * File:        AsteroidFamiliesForm.PlanetoidEntry.cs
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
	/// <summary>Represents parsed orbital parameters for a single planetoid.</summary>
	/// <param name="Index">MPCORB index or provisional designation of the planetoid.</param>
	/// <param name="Name">Proper name of the planetoid, if available; otherwise, an empty string.</param>
	/// <param name="SemiMajorAxis">Semi-major axis (a) in AU.</param>
	/// <param name="Eccentricity">Orbital eccentricity (e).</param>
	/// <param name="Inclination">Orbital inclination (i) in degrees.</param>
	/// <param name="MeanAnomaly">Mean anomaly (M) in degrees.</param>
	/// <param name="ArgPeri">Argument of perihelion (ω) in degrees.</param>
	/// <param name="LongAscNode">Longitude of the ascending node (Ω) in degrees.</param>
	/// <return>A new instance of the <see cref="PlanetoidEntry"/> record.</return>
	/// <remarks>This record is used to store the orbital elements of a planetoid for family detection.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed record PlanetoidEntry(string Index, string Name, double SemiMajorAxis, double Eccentricity, double Inclination, double MeanAnomaly, double ArgPeri, double LongAscNode)
	{
		/// <summary>Gets a string representation of the planetoid entry for debugging purposes.</summary>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		private string DebuggerDisplay => ToString();
	}
}