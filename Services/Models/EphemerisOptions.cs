/*
 * File:        EphemerisOptions.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents options controlling the physical model of an ephemeris calculation.
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

/// <summary>Represents options controlling the physical model of an ephemeris calculation.</summary>
/// <param name="IncludePlanetaryPerturbations">If <c>true</c>, the orbit is numerically integrated including the gravitational perturbations of the planets and the relativistic correction of the Sun; otherwise a pure two-body (Kepler) orbit is used.</param>
/// <param name="ApplyRefraction">If <c>true</c>, atmospheric refraction (standard atmosphere) is applied to the altitudes.</param>
/// <remarks>The <see cref="EphemerisOptions"/> record is used to specify options that control the physical model of an ephemeris calculation, such as whether to include planetary perturbations and whether to apply atmospheric refraction.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed record EphemerisOptions(bool IncludePlanetaryPerturbations = true, bool ApplyRefraction = true)
{
	/// <summary>Gets the default options (perturbations and refraction enabled).</summary>
	/// <remarks>This property provides a convenient way to obtain a standard set of ephemeris options without having to specify the parameters explicitly.</remarks>
	public static EphemerisOptions Default { get; } = new();

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
