/*
 * File:        EphemerisEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents the calculated position of a minor planet at a specific time.
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

/// <summary>Represents the calculated position of a minor planet at a specific time.</summary>
/// <param name="Time">The instant of the position, always expressed in UTC (offset +00:00).</param>
/// <param name="RightAscensionHours">Apparent topocentric right ascension in hours [0 h, 24 h), true equator and equinox of date.</param>
/// <param name="DeclinationDegrees">Apparent topocentric declination in degrees [−90°, +90°], true equator and equinox of date.</param>
/// <param name="AzimuthDegrees">Azimuth in degrees [0°, 360°), measured from north through east.</param>
/// <param name="AltitudeDegrees">Altitude above the horizon in degrees [−90°, +90°] (refraction applied if requested).</param>
/// <param name="DistanceAu">Topocentric distance (observer–object) in astronomical units [AU].</param>
/// <param name="ApparentMagnitude">Apparent visual magnitude [mag] according to the IAU H,G system; <see cref="double.NaN"/> if the absolute magnitude is unknown.</param>
/// <param name="IsVisible"><c>true</c> if all visibility criteria are fulfilled; otherwise, <c>false</c>.</param>
/// <param name="HeliocentricDistanceAu">Heliocentric distance (Sun–object) in astronomical units [AU].</param>
/// <param name="SunAltitudeDegrees">Apparent altitude of the Sun in degrees.</param>
/// <param name="MoonSeparationDegrees">Angular distance between object and Moon in degrees.</param>
/// <param name="PhaseAngleDegrees">Phase angle Sun–object–observer in degrees.</param>
/// <param name="ElongationDegrees">Solar elongation of the object in degrees.</param>
internal sealed record EphemerisEntry(
	DateTimeOffset Time,
	double RightAscensionHours,
	double DeclinationDegrees,
	double AzimuthDegrees,
	double AltitudeDegrees,
	double DistanceAu,
	double ApparentMagnitude,
	bool IsVisible,
	double HeliocentricDistanceAu = double.NaN,
	double SunAltitudeDegrees = double.NaN,
	double MoonSeparationDegrees = double.NaN,
	double PhaseAngleDegrees = double.NaN,
	double ElongationDegrees = double.NaN)
{
	/// <summary>Gets a value indicating whether the object is above the (true) horizon.</summary>
	public bool IsAboveHorizon => AltitudeDegrees > 0.0;
}
