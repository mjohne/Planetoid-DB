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

namespace Planetoid_DB.Services;

/// <summary>Provides approximate planetary positions from the JPL mean Keplerian elements (Standish, valid 1800–2050) and a low-precision lunar theory.</summary>
/// <remarks>
/// This provider is used as a fallback when no JPL DE440/DE441 file is available. The planetary positions are accurate to a few
/// arcseconds to arcminutes within 1800–2050 and degrade gracefully outside; the Moon is accurate to about 0.3°.
/// For high-accuracy results load a JPL DE440/DE441 binary file via <see cref="JplDevelopmentEphemeris"/>.
/// </remarks>
internal sealed class AnalyticalPlanetaryEphemeris : IPlanetaryEphemerisProvider
{
	/// <summary>Mean elements and their rates per Julian century: a [AU], e, I [°], L [°], ϖ [°], Ω [°].</summary>
	private static readonly Dictionary<SolarSystemBody, double[]> Elements = new()
	{
		[SolarSystemBody.Mercury] = [0.38709927, 0.00000037, 0.20563593, 0.00001906, 7.00497902, -0.00594749, 252.25032350, 149472.67411175, 77.45779628, 0.16047689, 48.33076593, -0.12534081],
		[SolarSystemBody.Venus] = [0.72333566, 0.00000390, 0.00677672, -0.00004107, 3.39467605, -0.00078890, 181.97909950, 58517.81538729, 131.60246718, 0.00268329, 76.67984255, -0.27769418],
		[SolarSystemBody.EarthMoonBarycenter] = [1.00000261, 0.00000562, 0.01671123, -0.00004392, -0.00001531, -0.01294668, 100.46457166, 35999.37244981, 102.93768193, 0.32327364, 0.0, 0.0],
		[SolarSystemBody.Mars] = [1.52371034, 0.00001847, 0.09339410, 0.00007882, 1.84969142, -0.00813131, -4.55343205, 19140.30268499, -23.94362959, 0.44441088, 49.55953891, -0.29257343],
		[SolarSystemBody.Jupiter] = [5.20288700, -0.00011607, 0.04838624, -0.00013253, 1.30439695, -0.00183714, 34.39644051, 3034.74612775, 14.72847983, 0.21252668, 100.47390909, 0.20469106],
		[SolarSystemBody.Saturn] = [9.53667594, -0.00125060, 0.05386179, -0.00050991, 2.48599187, 0.00193609, 49.95424423, 1222.49362201, 92.59887831, -0.41897216, 113.66242448, -0.28867794],
		[SolarSystemBody.Uranus] = [19.18916464, -0.00196176, 0.04725744, -0.00004397, 0.77263783, -0.00242939, 313.23810451, 428.48202785, 170.95427630, 0.40805281, 74.01692503, 0.04240589],
		[SolarSystemBody.Neptune] = [30.06992276, 0.00026291, 0.00859048, 0.00005105, 1.77004347, 0.00035372, -55.12002969, 218.45945325, 44.96476227, -0.32241464, 131.78422574, -0.00508664]
	};

	/// <inheritdoc/>
	public string Name => "Analytical (JPL mean elements, 1800–2050)";

	/// <inheritdoc/>
	public double StartJulianDate => 2268923.5; // 1500-01-01, degraded accuracy outside 1800–2050

	/// <inheritdoc/>
	public double EndJulianDate => 2816787.5; // 3000-01-01, degraded accuracy outside 1800–2050

	/// <inheritdoc/>
	public Vector3d GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		if (body == SolarSystemBody.Earth)
		{
			Vector3d emb = GetHeliocentricPosition(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: julianDateTdb);
			return emb - (GetGeocentricMoonPosition(julianDateTdb: julianDateTdb) / (1.0 + AstronomicalConstants.EarthMoonMassRatio));
		}
		double[] el = Elements[body];
		double t = (julianDateTdb - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		double a = el[0] + (el[1] * t);
		double e = el[2] + (el[3] * t);
		double i = el[4] + (el[5] * t);
		double l = el[6] + (el[7] * t);
		double varpi = el[8] + (el[9] * t);
		double node = el[10] + (el[11] * t);
		Vector3d ecliptic = OrbitPropagationService.KeplerPosition(
			semiMajorAxisAu: a,
			eccentricity: e,
			inclinationDegrees: i,
			longitudeOfAscendingNodeDegrees: node,
			argumentOfPerihelionDegrees: varpi - node,
			meanAnomalyDegrees: l - varpi);
		return AstronomicalConstants.EclipticToEquatorialJ2000 * ecliptic;
	}

	/// <inheritdoc/>
	/// <remarks>Low-precision lunar theory of the Astronomical Almanac (accuracy ≈ 0.3° in longitude, 0.2° in latitude).</remarks>
	public Vector3d GetGeocentricMoonPosition(double julianDateTdb)
	{
		const double d2r = AstronomicalConstants.DegreesToRadians;
		double t = (julianDateTdb - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		double lambda = 218.32 + (481267.881 * t)
			+ (6.29 * Math.Sin(a: (135.0 + (477198.87 * t)) * d2r))
			- (1.27 * Math.Sin(a: (259.3 - (413335.36 * t)) * d2r))
			+ (0.66 * Math.Sin(a: (235.7 + (890534.22 * t)) * d2r))
			+ (0.21 * Math.Sin(a: (269.9 + (954397.74 * t)) * d2r))
			- (0.19 * Math.Sin(a: (357.5 + (35999.05 * t)) * d2r))
			- (0.11 * Math.Sin(a: (186.5 + (966404.03 * t)) * d2r));
		double beta = (5.13 * Math.Sin(a: (93.3 + (483202.02 * t)) * d2r))
			+ (0.28 * Math.Sin(a: (228.2 + (960400.89 * t)) * d2r))
			- (0.28 * Math.Sin(a: (318.3 + (6003.15 * t)) * d2r))
			- (0.17 * Math.Sin(a: (217.6 - (407332.21 * t)) * d2r));
		double parallax = 0.9508
			+ (0.0518 * Math.Cos(d: (135.0 + (477198.87 * t)) * d2r))
			+ (0.0095 * Math.Cos(d: (259.3 - (413335.36 * t)) * d2r))
			+ (0.0078 * Math.Cos(d: (235.7 + (890534.22 * t)) * d2r))
			+ (0.0028 * Math.Cos(d: (269.9 + (954397.74 * t)) * d2r));
		double distanceAu = AstronomicalConstants.EarthEquatorialRadiusKm / Math.Sin(a: parallax * d2r) / AstronomicalConstants.AstronomicalUnitKm;
		// Remove the general precession in longitude to refer the position to the ecliptic J2000.0
		lambda -= 1.396971 * t;
		(double sinL, double cosL) = Math.SinCos(x: lambda * d2r);
		(double sinB, double cosB) = Math.SinCos(x: beta * d2r);
		Vector3d ecliptic = new(X: distanceAu * cosB * cosL, Y: distanceAu * cosB * sinL, Z: distanceAu * sinB);
		return AstronomicalConstants.EclipticToEquatorialJ2000 * ecliptic;
	}
}
