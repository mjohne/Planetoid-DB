/*
 * File:        IPlanetaryEphemeris.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Defines a source of heliocentric positions of the major solar system bodies.
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

/// <summary>Defines a source of heliocentric positions of the major solar system bodies.</summary>
/// <remarks>Implementations return positions in the equatorial ICRF/J2000.0 frame in AU. Implementations must be safe for use from a single background thread at a time.</remarks>
internal interface IPlanetaryEphemeris
{
	/// <summary>Gets a human-readable name of the ephemeris source.</summary>
	string Name { get; }

	/// <summary>Gets the heliocentric position of a body.</summary>
	/// <param name="body">The body (not <see cref="SolarSystemBody.Sun"/>).</param>
	/// <param name="julianDateTdb">The Julian date in Barycentric Dynamical Time (TDB).</param>
	/// <returns>The heliocentric position in AU (equatorial ICRF/J2000.0).</returns>
	Vector3D GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb);
}
