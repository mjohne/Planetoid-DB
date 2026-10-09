/*
 * File:        StateVector.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services.Models
 * Description: Represents a heliocentric state vector in the ICRF/J2000 equatorial frame.
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

namespace Planetoid_DB.Services;

/// <summary>Represents a heliocentric state vector in the ICRF/J2000 equatorial frame.</summary>
/// <param name="JulianDateTdb">Julian date in Barycentric Dynamical Time (TDB) [d].</param>
/// <param name="Position">Heliocentric position [AU].</param>
/// <param name="Velocity">Heliocentric velocity [AU/d].</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal readonly record struct StateVector(double JulianDateTdb, Vector3d Position, Vector3d Velocity)
{
	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}