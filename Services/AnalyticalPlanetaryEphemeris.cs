/*
 * File:        AnalyticalPlanetaryEphemeris.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides approximate planetary positions from the JPL mean orbital elements.
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

/// <summary>Provides approximate planetary positions from the JPL mean Keplerian elements (Standish, valid 1800–2050) and a low-precision lunar theory.</summary>
/// <remarks>This provider is used as a fallback when no JPL DE440/DE441 file is available. The planetary positions are accurate to a few
/// arcseconds to arcminutes within 1800–2050 and degrade gracefully outside; the Moon is accurate to about 0.3°.
/// For high-accuracy results load a JPL DE440/DE441 binary file via <see cref="JplDevelopmentEphemeris"/>.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class AnalyticalPlanetaryEphemeris : IPlanetaryEphemerisProvider
{
	/// <summary>Mean elements and their rates per Julian century: a [AU], e, I [°], L [°], ϖ [°], Ω [°].</summary>
	/// <remarks>Source: Standish, E.M., "Keplerian Elements for Approximate Positions of the Major Planets", JPL IOM 312.F-98-048, 24 November 1998; see <see href="https://ssd.jpl.nasa.gov/planets/approx_pos.html">JPL's approximate positions documentation</see>.</remarks>
	private static readonly Dictionary<SolarSystemBody, double[]> Elements = new()
	{
		[key: SolarSystemBody.Mercury] = [0.38709927, 0.00000037, 0.20563593, 0.00001906, 7.00497902, -0.00594749, 252.25032350, 149472.67411175, 77.45779628, 0.16047689, 48.33076593, -0.12534081],
		[key: SolarSystemBody.Venus] = [0.72333566, 0.00000390, 0.00677672, -0.00004107, 3.39467605, -0.00078890, 181.97909950, 58517.81538729, 131.60246718, 0.00268329, 76.67984255, -0.27769418],
		[key: SolarSystemBody.EarthMoonBarycenter] = [1.00000261, 0.00000562, 0.01671123, -0.00004392, -0.00001531, -0.01294668, 100.46457166, 35999.37244981, 102.93768193, 0.32327364, 0.0, 0.0],
		[key: SolarSystemBody.Mars] = [1.52371034, 0.00001847, 0.09339410, 0.00007882, 1.84969142, -0.00813131, -4.55343205, 19140.30268499, -23.94362959, 0.44441088, 49.55953891, -0.29257343],
		[key: SolarSystemBody.Jupiter] = [5.20288700, -0.00011607, 0.04838624, -0.00013253, 1.30439695, -0.00183714, 34.39644051, 3034.74612775, 14.72847983, 0.21252668, 100.47390909, 0.20469106],
		[key: SolarSystemBody.Saturn] = [9.53667594, -0.00125060, 0.05386179, -0.00050991, 2.48599187, 0.00193609, 49.95424423, 1222.49362201, 92.59887831, -0.41897216, 113.66242448, -0.28867794],
		[key: SolarSystemBody.Uranus] = [19.18916464, -0.00196176, 0.04725744, -0.00004397, 0.77263783, -0.00242939, 313.23810451, 428.48202785, 170.95427630, 0.40805281, 74.01692503, 0.04240589],
		[key: SolarSystemBody.Neptune] = [30.06992276, 0.00026291, 0.00859048, 0.00005105, 1.77004347, 0.00035372, -55.12002969, 218.45945325, 44.96476227, -0.32241464, 131.78422574, -0.00508664]
	};

	/// <summary>Gets a human-readable name of the ephemeris.</summary>
	/// <remarks>This name is used for display purposes and may include information about the source or method of the ephemeris.</remarks>
	public string Name => "Analytical (JPL mean elements, 1800–2050)";

	/// <summary>Gets the first Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>This value indicates the earliest date for which the ephemeris can provide reliable positions. The accuracy of the analytical model degrades outside the range of 1800–2050.</remarks>
	public double StartJulianDate => 2268923.5; // 1500-01-01, degraded accuracy outside 1800–2050

	/// <summary>Gets the last Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>This value indicates the latest date for which the ephemeris can provide reliable positions. The accuracy of the analytical model degrades outside the range of 1800–2050.</remarks>
	public double EndJulianDate => 2816787.5; // 3000-01-01, degraded accuracy outside 1800–2050

	/// <summary>Gets the heliocentric position of a solar system body at a given Julian date (TDB).</summary>
	/// <param name="body">The solar system body for which to get the heliocentric position.</param>
	/// <param name="julianDateTdb">The Julian date (TDB) at which to get the position.</param>
	/// <returns>The heliocentric position of the specified body at the given Julian date.</returns>
	/// <remarks>This method uses the JPL mean orbital elements to compute the position of the specified body. The accuracy is generally within a few arcseconds to arcminutes for the planets and about 0.3° for the Moon within the range of 1800–2050.</remarks>
	public Vector3d GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		// The Moon's position is computed relative to the Earth-Moon barycenter, so we need to adjust for the Moon's position relative to the Earth.
		if (body == SolarSystemBody.Earth)
		{
			// The heliocentric position of the Earth is the position of the Earth-Moon barycenter minus the Moon's position scaled by the Earth-Moon mass ratio.
			Vector3d emb = GetHeliocentricPosition(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: julianDateTdb);
			// The Moon's position is subtracted from the Earth-Moon barycenter position, scaled by the mass ratio, to get the Earth's heliocentric position.
			return emb - (GetGeocentricMoonPosition(julianDateTdb: julianDateTdb) / (1.0 + AstronomicalConstants.EarthMoonMassRatio));
		}
		// For other planets, we use the JPL mean orbital elements to compute the heliocentric position.
		double[] el = Elements[key: body];
		// Compute the time in Julian centuries since J2000.0
		double t = (julianDateTdb - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		// The semi-major axis (a) is in astronomical units (AU)
		double a = el[0] + (el[1] * t);
		// The eccentricity (e) is dimensionless
		double e = el[2] + (el[3] * t);
		// The inclination (i) is in degrees
		double i = el[4] + (el[5] * t);
		// The mean longitude (l) is in degrees
		double l = el[6] + (el[7] * t);
		// The longitude of perihelion (varpi) is in degrees
		double varpi = el[8] + (el[9] * t);
		// The longitude of the ascending node (node) is in degrees
		double node = el[10] + (el[11] * t);
		// Compute the heliocentric position in ecliptic coordinates using Kepler's laws
		Vector3d ecliptic = OrbitPropagationService.KeplerPosition(
			semiMajorAxisAu: a,
			eccentricity: e,
			inclinationDegrees: i,
			longitudeOfAscendingNodeDegrees: node,
			argumentOfPerihelionDegrees: varpi - node,
			meanAnomalyDegrees: l - varpi);
		// Convert the ecliptic coordinates to equatorial coordinates using the J2000 transformation matrix
		return AstronomicalConstants.EclipticToEquatorialJ2000 * ecliptic;
	}

	/// <summary>Computes the geocentric position of the Moon using a low-precision lunar theory based on the Astronomical Almanac.</summary>
	/// <remarks>Low-precision lunar theory of the Astronomical Almanac (accuracy ≈ 0.3° in longitude, 0.2° in latitude).</remarks>
	public Vector3d GetGeocentricMoonPosition(double julianDateTdb)
	{
		// Compute the Moon's position using a low-precision lunar theory based on the Astronomical Almanac.
		const double d2r = AstronomicalConstants.DegreesToRadians;
		// Compute the time in Julian centuries since J2000.0
		double t = (julianDateTdb - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		// Compute the Moon's mean longitude (lambda), latitude (beta), and parallax using the low-precision lunar theory formulas.
		double lambda = 218.32 + (481267.881 * t)
			+ (6.29 * Math.Sin(a: (135.0 + (477198.87 * t)) * d2r))
			- (1.27 * Math.Sin(a: (259.3 - (413335.36 * t)) * d2r))
			+ (0.66 * Math.Sin(a: (235.7 + (890534.22 * t)) * d2r))
			+ (0.21 * Math.Sin(a: (269.9 + (954397.74 * t)) * d2r))
			- (0.19 * Math.Sin(a: (357.5 + (35999.05 * t)) * d2r))
			- (0.11 * Math.Sin(a: (186.5 + (966404.03 * t)) * d2r));
		// Compute the Moon's mean latitude (beta) using the low-precision lunar theory formulas.
		double beta = (5.13 * Math.Sin(a: (93.3 + (483202.02 * t)) * d2r))
			+ (0.28 * Math.Sin(a: (228.2 + (960400.89 * t)) * d2r))
			- (0.28 * Math.Sin(a: (318.3 + (6003.15 * t)) * d2r))
			- (0.17 * Math.Sin(a: (217.6 - (407332.21 * t)) * d2r));
		// Compute the Moon's parallax using the low-precision lunar theory formulas.
		double parallax = 0.9508
			+ (0.0518 * Math.Cos(d: (135.0 + (477198.87 * t)) * d2r))
			+ (0.0095 * Math.Cos(d: (259.3 - (413335.36 * t)) * d2r))
			+ (0.0078 * Math.Cos(d: (235.7 + (890534.22 * t)) * d2r))
			+ (0.0028 * Math.Cos(d: (269.9 + (954397.74 * t)) * d2r));
		// Compute the distance to the Moon in astronomical units (AU) using the parallax and the Earth's equatorial radius.
		double distanceAu = AstronomicalConstants.EarthEquatorialRadiusKm / Math.Sin(a: parallax * d2r) / AstronomicalConstants.AstronomicalUnitKm;
		// Remove the general precession in longitude to refer the position to the ecliptic J2000.0
		lambda -= 1.396971 * t;
		// Convert the ecliptic coordinates (lambda, beta, distance) to equatorial coordinates using the J2000 transformation matrix.
		(double sinL, double cosL) = Math.SinCos(x: lambda * d2r);
		// Compute the sine and cosine of the Moon's latitude (beta) for the conversion to equatorial coordinates.
		(double sinB, double cosB) = Math.SinCos(x: beta * d2r);
		// Compute the Moon's position in ecliptic coordinates (X, Y, Z) using the distance and the sine/cosine of lambda and beta.
		Vector3d ecliptic = new(X: distanceAu * cosB * cosL, Y: distanceAu * cosB * sinL, Z: distanceAu * sinB);
		// Convert the ecliptic coordinates to equatorial coordinates using the J2000 transformation matrix.
		return AstronomicalConstants.EclipticToEquatorialJ2000 * ecliptic;
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
