/*
 * File:        MaxoidCalculator.PlanetElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.
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

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This partial class contains the <see cref="PlanetElements"/> record, which holds the Keplerian orbital elements of a solar system planet.</remarks>
internal partial class MaxoidCalculator
{
	/// <summary>Represents the Keplerian orbital elements of a solar system planet.</summary>
	/// <param name="Name">The common name of the planet.</param>
	/// <param name="SemiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="Eccentricity">The orbital eccentricity (dimensionless).</param>
	/// <param name="InclinationDeg">The orbital inclination to the ecliptic in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <return>The planet elements.</return>
	/// <remarks>These mean orbital elements are referenced to the J2000.0 ecliptic and equinox, as listed in standard astronomical references (Standish 1992 / IAU).</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record PlanetElements(string Name, double SemiMajorAxis, double Eccentricity, double InclinationDeg, double LongitudeAscendingNodeDeg, double ArgumentPerihelionDeg)
	{
		/// <summary>Returns a string representation of the planet elements for debugging purposes.</summary>
		/// <returns>A string representation of the planet elements.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the PlanetElements record in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}