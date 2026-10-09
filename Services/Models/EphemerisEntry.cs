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

using System.Diagnostics;

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
/// <param name="GeometricAltitudeDegrees">Geometric altitude before atmospheric refraction, if available.</param>
/// <param name="AstrometricRightAscensionHours">Astrometric topocentric right ascension in hours (ICRF/J2000, light-time corrected, without aberration, precession and nutation), as published by the Minor Planet Center.</param>
/// <param name="AstrometricDeclinationDegrees">Astrometric topocentric declination in degrees (ICRF/J2000, light-time corrected, without aberration, precession and nutation), as published by the Minor Planet Center.</param>
/// <remarks>The <see cref="EphemerisEntry"/> record encapsulates the calculated position and visibility information of a minor planet at a specific instant in time, including both topocentric and heliocentric coordinates, apparent magnitude, and various angular measurements relevant to observational astronomy.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
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
	double ElongationDegrees = double.NaN,
	double GeometricAltitudeDegrees = double.NaN,
	double AstrometricRightAscensionHours = double.NaN,
	double AstrometricDeclinationDegrees = double.NaN)
{
	/// <summary>Gets a value indicating whether the object is above the (true) horizon, based on the geometric altitude if available, otherwise the apparent altitude.</summary>
	/// <remarks>This property uses the geometric altitude if available; otherwise, it falls back to the apparent altitude.</remarks>
	public bool IsAboveHorizon => (double.IsNaN(d: GeometricAltitudeDegrees) ? AltitudeDegrees : GeometricAltitudeDegrees) > 0.0;

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
