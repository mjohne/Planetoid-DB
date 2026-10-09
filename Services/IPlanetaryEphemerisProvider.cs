/*
 * File:        IPlanetaryEphemerisProvider.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Defines a source of positions of the major solar system bodies.
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

/// <summary>Defines a source of positions of the major solar system bodies (e.g. JPL DE440/DE441 or an analytical theory).</summary>
/// <remarks>All positions are expressed in astronomical units [AU] in the ICRF/J2000 equatorial frame; times are Julian dates in TDB.</remarks>
internal interface IPlanetaryEphemerisProvider
{
	/// <summary>Gets a human-readable name of the ephemeris (e.g. "JPL DE440").</summary>
	/// <remarks>This name is used for display purposes only and does not affect the ephemeris calculations.</remarks>
	string Name { get; }

	/// <summary>Gets the first Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>Ephemerides are typically valid for a limited time span; this property indicates the earliest date for which the ephemeris can provide accurate positions.</remarks>
	double StartJulianDate { get; }

	/// <summary>Gets the last Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>Ephemerides are typically valid for a limited time span; this property indicates the latest date for which the ephemeris can provide accurate positions.</remarks>
	double EndJulianDate { get; }

	/// <summary>Gets the heliocentric position of a body.</summary>
	/// <param name="body">The body.</param>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The heliocentric position [AU], ICRF/J2000 equatorial.</returns>
	/// <remarks>This method returns the position of the specified body relative to the Sun, expressed in astronomical units [AU] in the ICRF/J2000 equatorial frame. The input Julian date must be within the valid range of the ephemeris.</remarks>
	Vector3d GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb);

	/// <summary>Gets the geocentric position of the Moon.</summary>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The geocentric position of the Moon [AU], ICRF/J2000 equatorial.</returns>
	/// <remarks>This method returns the position of the Moon relative to the Earth, expressed in astronomical units [AU] in the ICRF/J2000 equatorial frame. The input Julian date must be within the valid range of the ephemeris.</remarks>
	Vector3d GetGeocentricMoonPosition(double julianDateTdb);
}
