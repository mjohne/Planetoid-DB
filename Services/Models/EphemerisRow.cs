/*
 * File:        EphemerisRow.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services.Models
 * Description: Immutable record representing a single ephemeris data row for virtual ListView display.
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

namespace Planetoid_DB.Services.Models;

/// <summary>Represents calculated ephemeris parameters for a single time step.</summary>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly record struct EphemerisRow(DateTime Date, double JulianDate, double HeliocentricDistanceAu, double GeocentricDistanceAu, double PhaseAngleDegrees, double ElongationDegrees, double ApparentMagnitude, double AngularDiameterArcsec)
{
	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
