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

namespace Planetoid_DB.Services;

/// <summary>Represents options controlling the physical model of an ephemeris calculation.</summary>
/// <param name="IncludePlanetaryPerturbations">If <c>true</c>, the orbit is numerically integrated including the gravitational perturbations of the planets and the relativistic correction of the Sun; otherwise a pure two-body (Kepler) orbit is used.</param>
/// <param name="ApplyRefraction">If <c>true</c>, atmospheric refraction (standard atmosphere) is applied to the altitudes.</param>
internal sealed record EphemerisOptions(bool IncludePlanetaryPerturbations = true, bool ApplyRefraction = true)
{
	/// <summary>Gets the default options (perturbations and refraction enabled).</summary>
	public static EphemerisOptions Default { get; } = new();
}
