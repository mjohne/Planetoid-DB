/*
 * File:        ProperOrbitalElement.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Model representing proper orbital elements and frequencies from the AstDyS database.
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

/// <summary>Represents a set of proper orbital elements and fundamental frequencies parsed from the AstDyS (Asteroids - Dynamic Site) database.</summary>
/// <param name="Designation">The asteroid designation or catalog number (e.g., "1", "101955", "2001 KX76").</param>
/// <param name="ProperSemiMajorAxisAu">The proper semi-major axis a_p in AU.</param>
/// <param name="ProperEccentricity">The proper numerical eccentricity e_p.</param>
/// <param name="ProperSinInclination">The sine of proper inclination sin(i_p).</param>
/// <param name="ProperInclinationDegrees">The proper inclination i_p in degrees.</param>
/// <param name="PerihelionFrequencyG">The fundamental perihelion precession frequency g in arcsec/year.</param>
/// <param name="NodalFrequencyS">The fundamental nodal precession frequency s in arcsec/year.</param>
/// <param name="MeanMotionN">The mean daily motion n in degrees/day.</param>
/// <param name="LyapunovExponent">The Lyapunov characteristic exponent (LCE), if available.</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly record struct ProperOrbitalElement(string Designation, double ProperSemiMajorAxisAu, double ProperEccentricity, double ProperSinInclination, double ProperInclinationDegrees, double PerihelionFrequencyG, double NodalFrequencyS, double MeanMotionN, double LyapunovExponent)
{
	/// <summary>Calculates the proper inclination in degrees from sin(i_p).</summary>
	/// <param name="sinInclination">The sine of proper inclination sin(i_p).</param>
	/// <returns>The proper inclination in degrees.</returns>
	public static double ConvertSinInclinationToDegrees(double sinInclination)
	{
		double clampedSin = Math.Clamp(value: sinInclination, min: -1.0, max: 1.0);
		return Math.Abs(value: double.RadiansToDegrees(radians: Math.Asin(d: clampedSin)));
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
