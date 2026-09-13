/*
 * File:        MaxoidsOfOneMinorPlanetForm.OrbitalElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for displaying the Maximum Orbit Intersection Distance (MAXOID) of a minor planet relative to each of the eight solar system planets.
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

/// <summary>Represents the orbital elements of a minor planet used for MAXOID calculations.</summary>
/// <remarks>This record is used to encapsulate the orbital parameters of a minor planet for MAXOID calculations.</remarks>
internal partial class MaxoidsOfOneMinorPlanetForm
{
	/// <summary>Represents the orbital elements of a minor planet used for MAXOID calculations.</summary>
	/// <param name="SemiMajorAxis">The semi-major axis of the orbit in astronomical units (AU).</param>
	/// <param name="Eccentricity">The orbital eccentricity.</param>
	/// <param name="InclinationDeg">The orbital inclination in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <returns>A new instance of the <see cref="OrbitalElements"/> record.</returns>
	/// <remarks>This record is used to encapsulate the orbital parameters of a minor planet for MAXOID calculations.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record OrbitalElements(double SemiMajorAxis, double Eccentricity, double InclinationDeg, double LongitudeAscendingNodeDeg, double ArgumentPerihelionDeg)
	{
		/// <summary>Returns a string representation of the orbital elements for debugging purposes.</summary>		
		/// <returns>A string representation of the orbital elements.</returns>
		/// <remarks>This property is used to provide a visual representation of the orbital elements in the debugger.</remarks>
		private string DebuggerDisplay => ToString();
	}
}