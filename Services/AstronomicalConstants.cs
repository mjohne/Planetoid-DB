/*
 * File:        AstronomicalConstants.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides astronomical constants used by the ephemeris services.
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

/// <summary>Provides astronomical constants used by the ephemeris services.</summary>
/// <remarks>Mass parameters correspond to the JPL DE440/DE441 planetary ephemerides; all lengths are in astronomical units (AU) and all times in days unless stated otherwise.</remarks>
internal static class AstronomicalConstants
{
	/// <summary>Julian date of the standard epoch J2000.0 (2000-01-01 12:00 TT).</summary>
	public const double JulianDateJ2000 = 2451545.0;

	/// <summary>Number of days in a Julian century.</summary>
	public const double DaysPerJulianCentury = 36525.0;

	/// <summary>Number of seconds per day.</summary>
	public const double SecondsPerDay = 86400.0;

	/// <summary>Astronomical unit in kilometres (IAU 2012 Resolution B2).</summary>
	public const double AstronomicalUnitKm = 149597870.7;

	/// <summary>Speed of light in AU per day.</summary>
	public const double SpeedOfLightAuPerDay = 299792.458 * SecondsPerDay / AstronomicalUnitKm;

	/// <summary>Gaussian gravitational constant k in AU^(3/2) / day / M_sun^(1/2).</summary>
	public const double GaussianGravitationalConstant = 0.01720209895;

	/// <summary>Heliocentric gravitational parameter GM_sun in AU³/day².</summary>
	public const double GmSun = GaussianGravitationalConstant * GaussianGravitationalConstant;

	/// <summary>Ratio of the Earth mass to the Moon mass (DE440).</summary>
	public const double EarthMoonMassRatio = 81.3005682214972;

	/// <summary>Obliquity of the ecliptic at J2000.0 in arcseconds (IAU 1976, the reference used for MPCORB elements).</summary>
	public const double ObliquityJ2000Arcseconds = 84381.448;

	/// <summary>WGS84 equatorial radius in kilometres.</summary>
	public const double Wgs84EquatorialRadiusKm = 6378.137;

	/// <summary>WGS84 flattening.</summary>
	public const double Wgs84Flattening = 1.0 / 298.257223563;

	/// <summary>Conversion factor from degrees to radians.</summary>
	public const double DegreesToRadians = Math.PI / 180.0;

	/// <summary>Conversion factor from radians to degrees.</summary>
	public const double RadiansToDegrees = 180.0 / Math.PI;

	/// <summary>Conversion factor from arcseconds to radians.</summary>
	public const double ArcsecondsToRadians = Math.PI / (180.0 * 3600.0);

	/// <summary>Gets the sun-to-planet mass ratio of a perturbing body (DE440).</summary>
	/// <param name="body">The body.</param>
	/// <returns>The ratio M_sun / M_body.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when no mass is defined for the body.</exception>
	public static double GetSunToBodyMassRatio(SolarSystemBody body) => body switch
	{
		SolarSystemBody.Mercury => 6023657.33,
		SolarSystemBody.Venus => 408523.7188,
		SolarSystemBody.EarthMoonBarycenter => 328900.5596,
		SolarSystemBody.Earth => 328900.5596 * (1.0 + EarthMoonMassRatio) / EarthMoonMassRatio,
		SolarSystemBody.Moon => 328900.5596 * (1.0 + EarthMoonMassRatio),
		SolarSystemBody.Mars => 3098703.59,
		SolarSystemBody.Jupiter => 1047.348644,
		SolarSystemBody.Saturn => 3497.901768,
		SolarSystemBody.Uranus => 22902.951,
		SolarSystemBody.Neptune => 19412.25977,
		_ => throw new ArgumentOutOfRangeException(paramName: nameof(body), actualValue: body, message: "No mass defined for this body.")
	};
}
