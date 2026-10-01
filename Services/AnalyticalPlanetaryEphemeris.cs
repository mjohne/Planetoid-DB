/*
 * File:        AnalyticalPlanetaryEphemeris.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides analytical planetary and lunar positions when no JPL DE file is available.
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

/// <summary>Provides analytical planetary and lunar positions when no JPL DE file is available.</summary>
/// <remarks>
/// Planets use the JPL Keplerian approximations by E. M. Standish (fitted to the JPL DE ephemerides, 1800–2050; Earth-Moon barycenter error about 20″).
/// The Moon uses a truncated lunar theory (Montenbruck &amp; Pfleger / Meeus, accuracy of a few arcminutes), which is sufficient for perturbations and Moon distance checks.
/// For the highest accuracy, use <see cref="JplSpkEphemeris"/> with a DE440 or DE441 file.
/// </remarks>
internal sealed class AnalyticalPlanetaryEphemeris : IPlanetaryEphemeris
{
	/// <summary>Keplerian elements and rates per Julian century: a [AU], e, I [°], L [°], ϖ [°], Ω [°].</summary>
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
	public string Name => "Analytical (JPL Keplerian approximation)";

	/// <inheritdoc/>
	public Vector3D GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		switch (body)
		{
			case SolarSystemBody.Sun:
				return Vector3D.Zero;
			case SolarSystemBody.Earth:
				return GetPlanet(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: julianDateTdb) - (GetGeocentricMoon(julianDateTdb: julianDateTdb) / (1.0 + AstronomicalConstants.EarthMoonMassRatio));
			case SolarSystemBody.Moon:
				Vector3D moon = GetGeocentricMoon(julianDateTdb: julianDateTdb);
				return GetPlanet(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: julianDateTdb) + (moon * (AstronomicalConstants.EarthMoonMassRatio / (1.0 + AstronomicalConstants.EarthMoonMassRatio)));
			default:
				return GetPlanet(body: body, julianDateTdb: julianDateTdb);
		}
	}

	/// <summary>Calculates the geocentric position of the Moon.</summary>
	/// <param name="julianDateTdb">The Julian date (TDB).</param>
	/// <returns>The geocentric lunar position in AU (equatorial ICRF/J2000.0).</returns>
	public static Vector3D GetGeocentricMoon(double julianDateTdb)
	{
		const double twoPi = 2.0 * Math.PI;
		const double arcsecondsPerRadian = 206264.806247;
		double t = AstronomicalTime.JulianCenturiesSinceJ2000(julianDate: julianDateTdb);
		double l0 = Frac(x: 0.606433 + (1336.855225 * t));
		double l = twoPi * Frac(x: 0.374897 + (1325.552410 * t));
		double ls = twoPi * Frac(x: 0.993133 + (99.997361 * t));
		double d = twoPi * Frac(x: 0.827361 + (1236.853086 * t));
		double f = twoPi * Frac(x: 0.259086 + (1342.227825 * t));
		double dl = (22640 * Math.Sin(a: l)) - (4586 * Math.Sin(a: l - (2 * d))) + (2370 * Math.Sin(a: 2 * d)) + (769 * Math.Sin(a: 2 * l))
			- (668 * Math.Sin(a: ls)) - (412 * Math.Sin(a: 2 * f)) - (212 * Math.Sin(a: (2 * l) - (2 * d))) - (206 * Math.Sin(a: l + ls - (2 * d)))
			+ (192 * Math.Sin(a: l + (2 * d))) - (165 * Math.Sin(a: ls - (2 * d))) - (125 * Math.Sin(a: d)) - (110 * Math.Sin(a: l + ls))
			+ (148 * Math.Sin(a: l - ls)) - (55 * Math.Sin(a: (2 * f) - (2 * d)));
		double s = f + ((dl + (412 * Math.Sin(a: 2 * f)) + (541 * Math.Sin(a: ls))) / arcsecondsPerRadian);
		double h = f - (2 * d);
		double n = (-526 * Math.Sin(a: h)) + (44 * Math.Sin(a: l + h)) - (31 * Math.Sin(a: -l + h)) - (23 * Math.Sin(a: ls + h))
			+ (11 * Math.Sin(a: -ls + h)) - (25 * Math.Sin(a: (-2 * l) + f)) + (21 * Math.Sin(a: -l + f));
		double longitude = twoPi * Frac(x: l0 + (dl / 1296.0e3));
		double latitude = ((18520.0 * Math.Sin(a: s)) + n) / arcsecondsPerRadian;
		double distanceKm = 385000.56 - (20905.355 * Math.Cos(d: l)) - (3699.111 * Math.Cos(d: (2 * d) - l)) - (2955.968 * Math.Cos(d: 2 * d))
			- (569.925 * Math.Cos(d: 2 * l)) + (48.888 * Math.Cos(d: ls)) - (3.149 * Math.Cos(d: 2 * f)) + (246.158 * Math.Cos(d: (2 * d) - (2 * l)))
			- (152.138 * Math.Cos(d: (2 * d) - ls - l)) - (170.733 * Math.Cos(d: (2 * d) + l)) - (204.586 * Math.Cos(d: (2 * d) - ls))
			- (129.620 * Math.Cos(d: l - ls)) + (108.743 * Math.Cos(d: d)) + (104.755 * Math.Cos(d: l + ls));
		double r = distanceKm / AstronomicalConstants.AstronomicalUnitKm;
		(double sinLat, double cosLat) = Math.SinCos(x: latitude);
		(double sinLon, double cosLon) = Math.SinCos(x: longitude);
		// Ecliptic of date -> equator of date -> J2000.0
		Vector3D eclipticOfDate = new(X: r * cosLat * cosLon, Y: r * cosLat * sinLon, Z: r * sinLat);
		double julianDateTt = julianDateTdb;
		Vector3D equatorOfDate = Matrix3D.RotationX(angleRadians: -CoordinateTransformationService.GetMeanObliquityRadians(julianDateTt: julianDateTt)) * eclipticOfDate;
		return CoordinateTransformationService.GetPrecessionMatrix(julianDateTt: julianDateTt).Transpose() * equatorOfDate;
	}

	/// <summary>Calculates the heliocentric position of a planet from the Keplerian approximation.</summary>
	/// <param name="body">The planet or the Earth-Moon barycenter.</param>
	/// <param name="julianDateTdb">The Julian date (TDB).</param>
	/// <returns>The heliocentric position in AU (equatorial ICRF/J2000.0).</returns>
	private static Vector3D GetPlanet(SolarSystemBody body, double julianDateTdb)
	{
		if (!Elements.TryGetValue(key: body, value: out double[]? el))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(body), actualValue: body, message: "The body is not supported by the analytical ephemeris.");
		}
		double t = AstronomicalTime.JulianCenturiesSinceJ2000(julianDate: julianDateTdb);
		double a = el[0] + (el[1] * t);
		double e = el[2] + (el[3] * t);
		double i = (el[4] + (el[5] * t)) * AstronomicalConstants.DegreesToRadians;
		double meanLongitude = el[6] + (el[7] * t);
		double longitudeOfPerihelion = el[8] + (el[9] * t);
		double node = el[10] + (el[11] * t);
		double argumentOfPerihelion = (longitudeOfPerihelion - node) * AstronomicalConstants.DegreesToRadians;
		double meanAnomaly = CoordinateTransformationService.NormalizeDegrees(degrees: meanLongitude - longitudeOfPerihelion) * AstronomicalConstants.DegreesToRadians;
		Vector3D ecliptic = OrbitPropagationService.GetEclipticPositionFromElements(semiMajorAxisAu: a, eccentricity: e, inclinationRadians: i,
			longitudeOfAscendingNodeRadians: node * AstronomicalConstants.DegreesToRadians, argumentOfPerihelionRadians: argumentOfPerihelion, meanAnomalyRadians: meanAnomaly);
		return CoordinateTransformationService.EclipticToEquatorial(ecliptic: ecliptic);
	}

	/// <summary>Returns the fractional part of a number in the range [0, 1).</summary>
	/// <param name="x">The number.</param>
	/// <returns>The fractional part.</returns>
	private static double Frac(double x) => x - Math.Floor(d: x);
}
