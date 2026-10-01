/*
 * File:        CoordinateTransformationService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides reference frame and coordinate transformations for ephemeris calculations.
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

/// <summary>Provides reference frame and coordinate transformations for ephemeris calculations.</summary>
/// <remarks>
/// The internal reference frame is the equatorial ICRF/J2000.0 frame. The transformation to the true equator and equinox of date uses the IAU 1976 precession
/// and the leading terms of the IAU 1980 nutation series. Sidereal time follows the IAU 1982 GMST expression with the equation of the equinoxes.
/// All angles carry their unit in the member name.
/// </remarks>
internal static class CoordinateTransformationService
{
	/// <summary>Obliquity of the ecliptic at J2000.0 in radians.</summary>
	public const double ObliquityJ2000Radians = AstronomicalConstants.ObliquityJ2000Arcseconds * AstronomicalConstants.ArcsecondsToRadians;

	/// <summary>Rotation from ecliptic J2000.0 to equatorial J2000.0 coordinates.</summary>
	private static readonly Matrix3D EclipticToEquatorialMatrix = Matrix3D.RotationX(angleRadians: -ObliquityJ2000Radians);

	/// <summary>Normalizes an angle to the range [0°, 360°).</summary>
	/// <param name="degrees">The angle in degrees (may be negative or larger than 360°).</param>
	/// <returns>The normalized angle in degrees.</returns>
	public static double NormalizeDegrees(double degrees)
	{
		double result = degrees % 360.0;
		if (result < 0.0)
		{
			result += 360.0;
		}
		// Guard against rounding of tiny negative values to exactly 360
		return result >= 360.0 ? 0.0 : result;
	}

	/// <summary>Normalizes an angle to the range [0, 2π).</summary>
	/// <param name="radians">The angle in radians.</param>
	/// <returns>The normalized angle in radians.</returns>
	public static double NormalizeRadians(double radians)
	{
		double result = radians % (2.0 * Math.PI);
		if (result < 0.0)
		{
			result += 2.0 * Math.PI;
		}
		return result >= 2.0 * Math.PI ? 0.0 : result;
	}

	/// <summary>Transforms a vector from ecliptic J2000.0 to equatorial J2000.0 coordinates.</summary>
	/// <param name="ecliptic">The ecliptic vector.</param>
	/// <returns>The equatorial vector.</returns>
	public static Vector3D EclipticToEquatorial(Vector3D ecliptic) => EclipticToEquatorialMatrix * ecliptic;

	/// <summary>Transforms a vector from equatorial J2000.0 to ecliptic J2000.0 coordinates.</summary>
	/// <param name="equatorial">The equatorial vector.</param>
	/// <returns>The ecliptic vector.</returns>
	public static Vector3D EquatorialToEcliptic(Vector3D equatorial) => EclipticToEquatorialMatrix.Transpose() * equatorial;

	/// <summary>Calculates the mean obliquity of the ecliptic of date (IAU 1980).</summary>
	/// <param name="julianDateTt">The Julian date (TT).</param>
	/// <returns>The mean obliquity in radians.</returns>
	public static double GetMeanObliquityRadians(double julianDateTt)
	{
		double t = AstronomicalTime.JulianCenturiesSinceJ2000(julianDate: julianDateTt);
		return (AstronomicalConstants.ObliquityJ2000Arcseconds - (46.8150 * t) - (0.00059 * t * t) + (0.001813 * t * t * t)) * AstronomicalConstants.ArcsecondsToRadians;
	}

	/// <summary>Calculates the IAU 1976 precession matrix from J2000.0 to the mean equator and equinox of date.</summary>
	/// <param name="julianDateTt">The Julian date (TT).</param>
	/// <returns>The precession matrix.</returns>
	public static Matrix3D GetPrecessionMatrix(double julianDateTt)
	{
		double t = AstronomicalTime.JulianCenturiesSinceJ2000(julianDate: julianDateTt);
		double zeta = ((2306.2181 * t) + (0.30188 * t * t) + (0.017998 * t * t * t)) * AstronomicalConstants.ArcsecondsToRadians;
		double z = ((2306.2181 * t) + (1.09468 * t * t) + (0.018203 * t * t * t)) * AstronomicalConstants.ArcsecondsToRadians;
		double theta = ((2004.3109 * t) - (0.42665 * t * t) - (0.041833 * t * t * t)) * AstronomicalConstants.ArcsecondsToRadians;
		return Matrix3D.RotationZ(angleRadians: -z) * Matrix3D.RotationY(angleRadians: theta) * Matrix3D.RotationZ(angleRadians: -zeta);
	}

	/// <summary>Calculates the nutation in longitude and obliquity using the leading terms of the IAU 1980 series.</summary>
	/// <param name="julianDateTt">The Julian date (TT).</param>
	/// <returns>The nutation in longitude Δψ and in obliquity Δε, both in radians.</returns>
	/// <remarks>The truncated series is accurate to about 0.5″ in Δψ and 0.1″ in Δε.</remarks>
	public static (double LongitudeRadians, double ObliquityRadians) GetNutation(double julianDateTt)
	{
		double t = AstronomicalTime.JulianCenturiesSinceJ2000(julianDate: julianDateTt);
		double omega = (125.04452 - (1934.136261 * t)) * AstronomicalConstants.DegreesToRadians;
		double l = (280.4665 + (36000.7698 * t)) * AstronomicalConstants.DegreesToRadians;
		double lMoon = (218.3165 + (481267.8813 * t)) * AstronomicalConstants.DegreesToRadians;
		double dPsi = (-17.20 * Math.Sin(a: omega)) - (1.32 * Math.Sin(a: 2 * l)) - (0.23 * Math.Sin(a: 2 * lMoon)) + (0.21 * Math.Sin(a: 2 * omega));
		double dEps = (9.20 * Math.Cos(d: omega)) + (0.57 * Math.Cos(d: 2 * l)) + (0.10 * Math.Cos(d: 2 * lMoon)) - (0.09 * Math.Cos(d: 2 * omega));
		return (dPsi * AstronomicalConstants.ArcsecondsToRadians, dEps * AstronomicalConstants.ArcsecondsToRadians);
	}

	/// <summary>Calculates the combined precession-nutation matrix from J2000.0 to the true equator and equinox of date.</summary>
	/// <param name="julianDateTt">The Julian date (TT).</param>
	/// <returns>The precession-nutation matrix.</returns>
	public static Matrix3D GetPrecessionNutationMatrix(double julianDateTt)
	{
		double meanObliquity = GetMeanObliquityRadians(julianDateTt: julianDateTt);
		(double dPsi, double dEps) = GetNutation(julianDateTt: julianDateTt);
		Matrix3D nutation = Matrix3D.RotationX(angleRadians: -(meanObliquity + dEps)) * Matrix3D.RotationZ(angleRadians: -dPsi) * Matrix3D.RotationX(angleRadians: meanObliquity);
		return nutation * GetPrecessionMatrix(julianDateTt: julianDateTt);
	}

	/// <summary>Calculates the Greenwich apparent sidereal time.</summary>
	/// <param name="julianDateUt">The Julian date (UT1; UTC is used as approximation, |UT1 − UTC| &lt; 0.9 s).</param>
	/// <param name="julianDateTt">The Julian date (TT) used for the equation of the equinoxes.</param>
	/// <returns>The Greenwich apparent sidereal time in radians, range [0, 2π).</returns>
	public static double GetGreenwichApparentSiderealTimeRadians(double julianDateUt, double julianDateTt)
	{
		double d = julianDateUt - AstronomicalConstants.JulianDateJ2000;
		double t = d / AstronomicalConstants.DaysPerJulianCentury;
		double gmstDegrees = 280.46061837 + (360.98564736629 * d) + (0.000387933 * t * t) - (t * t * t / 38710000.0);
		(double dPsi, double dEps) = GetNutation(julianDateTt: julianDateTt);
		double equationOfEquinoxes = dPsi * Math.Cos(d: GetMeanObliquityRadians(julianDateTt: julianDateTt) + dEps);
		return NormalizeRadians(radians: (gmstDegrees * AstronomicalConstants.DegreesToRadians) + equationOfEquinoxes);
	}

	/// <summary>Calculates the geocentric position of an observer in the true equator-of-date frame.</summary>
	/// <param name="observer">The observer location (WGS84).</param>
	/// <param name="greenwichApparentSiderealTimeRadians">The Greenwich apparent sidereal time in radians.</param>
	/// <returns>The geocentric position vector in AU.</returns>
	public static Vector3D GetObserverPositionTrueOfDate(ObserverLocation observer, double greenwichApparentSiderealTimeRadians)
	{
		double lat = observer.LatitudeDegrees * AstronomicalConstants.DegreesToRadians;
		double localSiderealTime = greenwichApparentSiderealTimeRadians + (observer.LongitudeDegrees * AstronomicalConstants.DegreesToRadians);
		double oneMinusF = 1.0 - AstronomicalConstants.Wgs84Flattening;
		(double sinLat, double cosLat) = Math.SinCos(x: lat);
		double c = 1.0 / Math.Sqrt(d: (cosLat * cosLat) + (oneMinusF * oneMinusF * sinLat * sinLat));
		double s = oneMinusF * oneMinusF * c;
		double heightKm = observer.HeightMeters / 1000.0;
		double rhoCos = ((AstronomicalConstants.Wgs84EquatorialRadiusKm * c) + heightKm) * cosLat;
		double rhoSin = ((AstronomicalConstants.Wgs84EquatorialRadiusKm * s) + heightKm) * sinLat;
		return new Vector3D(X: rhoCos * Math.Cos(d: localSiderealTime), Y: rhoCos * Math.Sin(a: localSiderealTime), Z: rhoSin) / AstronomicalConstants.AstronomicalUnitKm;
	}

	/// <summary>Converts a Cartesian equatorial vector to right ascension and declination.</summary>
	/// <param name="vector">The vector (any length unit).</param>
	/// <returns>The right ascension in hours [0, 24) and the declination in degrees [−90, +90].</returns>
	public static (double RightAscensionHours, double DeclinationDegrees) ToRightAscensionDeclination(Vector3D vector)
	{
		double ra = NormalizeDegrees(degrees: Math.Atan2(y: vector.Y, x: vector.X) * AstronomicalConstants.RadiansToDegrees) / 15.0;
		double dec = Math.Atan2(y: vector.Z, x: Math.Sqrt(d: (vector.X * vector.X) + (vector.Y * vector.Y))) * AstronomicalConstants.RadiansToDegrees;
		return (ra >= 24.0 ? 0.0 : ra, dec);
	}

	/// <summary>Converts equatorial coordinates of date to horizontal coordinates.</summary>
	/// <param name="rightAscensionHours">The right ascension of date in hours.</param>
	/// <param name="declinationDegrees">The declination of date in degrees.</param>
	/// <param name="localApparentSiderealTimeRadians">The local apparent sidereal time in radians.</param>
	/// <param name="latitudeDegrees">The geodetic latitude of the observer in degrees.</param>
	/// <returns>The azimuth in degrees (north = 0°, east = 90°, range [0, 360)) and the geometric altitude in degrees.</returns>
	public static (double AzimuthDegrees, double AltitudeDegrees) ToHorizontal(double rightAscensionHours, double declinationDegrees, double localApparentSiderealTimeRadians, double latitudeDegrees)
	{
		double hourAngle = localApparentSiderealTimeRadians - (rightAscensionHours * 15.0 * AstronomicalConstants.DegreesToRadians);
		(double sinDec, double cosDec) = Math.SinCos(x: declinationDegrees * AstronomicalConstants.DegreesToRadians);
		(double sinLat, double cosLat) = Math.SinCos(x: latitudeDegrees * AstronomicalConstants.DegreesToRadians);
		(double sinH, double cosH) = Math.SinCos(x: hourAngle);
		double altitude = Math.Asin(d: Math.Clamp(value: (sinLat * sinDec) + (cosLat * cosDec * cosH), min: -1.0, max: 1.0));
		double azimuth = Math.Atan2(y: -cosDec * sinH, x: (sinDec * cosLat) - (cosDec * sinLat * cosH));
		return (NormalizeDegrees(degrees: azimuth * AstronomicalConstants.RadiansToDegrees), altitude * AstronomicalConstants.RadiansToDegrees);
	}

	/// <summary>Applies standard atmospheric refraction (Bennett 1982, 10 °C, 1010 hPa) to a geometric altitude.</summary>
	/// <param name="geometricAltitudeDegrees">The geometric (airless) altitude in degrees.</param>
	/// <returns>The apparent altitude in degrees; altitudes below −1° are returned unchanged.</returns>
	public static double ApplyRefraction(double geometricAltitudeDegrees)
	{
		if (geometricAltitudeDegrees < -1.0)
		{
			return geometricAltitudeDegrees;
		}
		// Sæmundsson's inverse of Bennett's formula: refraction in arcminutes for a true altitude
		double h = geometricAltitudeDegrees;
		double refractionArcminutes = 1.02 / Math.Tan(a: (h + (10.3 / (h + 5.11))) * AstronomicalConstants.DegreesToRadians);
		return h + (refractionArcminutes / 60.0);
	}

	/// <summary>Applies the annual aberration of light to a direction (first-order Lorentz transformation).</summary>
	/// <param name="direction">The geometric direction to the object.</param>
	/// <param name="observerVelocityAuPerDay">The barycentric or heliocentric velocity of the observer in AU/day.</param>
	/// <returns>The apparent unit direction.</returns>
	public static Vector3D ApplyAberration(Vector3D direction, Vector3D observerVelocityAuPerDay)
	{
		Vector3D u = direction.Normalize();
		Vector3D beta = observerVelocityAuPerDay / AstronomicalConstants.SpeedOfLightAuPerDay;
		return (u + beta - (u * u.Dot(other: beta))).Normalize();
	}
}
