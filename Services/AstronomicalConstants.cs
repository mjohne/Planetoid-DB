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

/// <summary>Provides astronomical constants (IAU 2012 / JPL DE440) used by the ephemeris services.</summary>
/// <remarks>All constants are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
internal static class AstronomicalConstants
{
	/// <summary>Gaussian gravitational constant k [AU^(3/2) / d].</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double GaussianGravitationalConstant = 0.01720209895;

	/// <summary>Heliocentric gravitational parameter GM☉ = k² [AU³/d²].</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double SunGm = GaussianGravitationalConstant * GaussianGravitationalConstant;

	/// <summary>Astronomical unit [km] (IAU 2012 Resolution B2).</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double AstronomicalUnitKm = 149597870.7;

	/// <summary>Speed of light [AU/d].</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double SpeedOfLightAuPerDay = 299792.458 * TimeScales.SecondsPerDay / AstronomicalUnitKm;

	/// <summary>Earth/Moon mass ratio (DE440).</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double EarthMoonMassRatio = 81.3005682214972;

	/// <summary>WGS84 equatorial radius of the Earth [km].</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double EarthEquatorialRadiusKm = 6378.137;

	/// <summary>WGS84 flattening of the Earth.</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double EarthFlattening = 1.0 / 298.257223563;

	/// <summary>Obliquity of the ecliptic at J2000.0 [°] (IAU 1976).</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double ObliquityJ2000Degrees = 23.4392911;

	/// <summary>Degrees to radians conversion factor.</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public const double DegreesToRadians = Math.PI / 180.0;

	/// <summary>Radians to degrees conversion factor.</summary>	
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>	
	public const double RadiansToDegrees = 180.0 / Math.PI;

	/// <summary>Arcseconds to radians conversion factor.</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>	
	public const double ArcsecondsToRadians = Math.PI / (180.0 * 3600.0);

	/// <summary>Gets the rotation from ecliptic J2000.0 to the ICRF/J2000 equatorial frame.</summary>
	/// <remarks>IAU 2012 Resolution B2, DE440 compatible.</remarks>
	public static Matrix3d EclipticToEquatorialJ2000 { get; } = Matrix3d.RotationX(angleRadians: -ObliquityJ2000Degrees * DegreesToRadians);

	/// <summary>Gets the Sun/body mass ratios used for the perturbing bodies.</summary>
	/// <remarks>Values from the IAU 2009/2012 system of constants (DE430/DE440 compatible). Mars to Neptune refer to the system barycenters.</remarks>
	public static IReadOnlyDictionary<SolarSystemBody, double> SunToBodyMassRatio { get; } = new Dictionary<SolarSystemBody, double>
	{
		[key: SolarSystemBody.Mercury] = 6023597.400017,
		[key: SolarSystemBody.Venus] = 408523.718655,
		[key: SolarSystemBody.EarthMoonBarycenter] = 328900.559708565,
		[key: SolarSystemBody.Mars] = 3098703.590291,
		[key: SolarSystemBody.Jupiter] = 1047.348625,
		[key: SolarSystemBody.Saturn] = 3497.901768,
		[key: SolarSystemBody.Uranus] = 22902.981613,
		[key: SolarSystemBody.Neptune] = 19412.237346
	};
}
