/*
 * File:        CoordinateTransformationService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides astronomical coordinate transformations.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;

namespace Planetoid_DB.Services;

/// <summary>Provides astronomical coordinate transformations: precession (IAU 1976), nutation (IAU 1980, principal terms), sidereal time, aberration, topocentric observer positions, horizontal coordinates and refraction.</summary>
/// <remarks>Angles carry their unit in the member names (…Degrees, …Hours, …Radians). Azimuth is measured from north through east.</remarks>
internal static class CoordinateTransformationService
{
	/// <summary>Normalizes an angle to the range [0°, 360°).</summary>
	/// <param name="degrees">The angle [°].</param>
	/// <returns>The normalized angle [°].</returns>
	/// <remarks>Negative angles are wrapped to the positive range. Angles greater than or equal to 360° are wrapped to the range [0°, 360°).</remarks>
	public static double NormalizeDegrees(double degrees)
	{
		// Use modulo to wrap the angle to the range [0°, 360°)
		double result = degrees % 360.0;
		// If the result is negative, add 360° to bring it into the range [0°, 360°)
		if (result < 0.0)
		{
			result += 360.0;
		}
		// If the result is exactly 360.0, return 0.0 to ensure the result is in [0°, 360°)
		return result >= 360.0 ? 0.0 : result;
	}

	/// <summary>Normalizes an hour angle or right ascension to the range [0 h, 24 h).</summary>
	/// <param name="hours">The angle [h].</param>
	/// <returns>The normalized angle [h].</returns>
	/// <remarks>Negative angles are wrapped to the positive range. Angles greater than or equal to 24 h are wrapped to the range [0 h, 24 h).</remarks>
	public static double NormalizeHours(double hours)
	{
		// Normalize hours by converting to degrees, normalizing, and converting back to hours
		return NormalizeDegrees(degrees: hours * 15.0) / 15.0;
	}

	/// <summary>Computes the precession matrix (IAU 1976) from J2000.0 to the mean equator and equinox of date.</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>The rotation matrix.</returns>
	/// <remarks>Accuracy about 0.1″ over ±2000 years from J2000.0.</remarks>
	public static Matrix3d PrecessionMatrix(double julianDateTt)
	{
		// Compute the precession angles in radians using the IAU 1976 model
		double t = (julianDateTt - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		double zeta = (2306.2181 + ((0.30188 + (0.017998 * t)) * t)) * t * AstronomicalConstants.ArcsecondsToRadians;
		double z = (2306.2181 + ((1.09468 + (0.018203 * t)) * t)) * t * AstronomicalConstants.ArcsecondsToRadians;
		double theta = (2004.3109 - ((0.42665 + (0.041833 * t)) * t)) * t * AstronomicalConstants.ArcsecondsToRadians;
		// Return the precession matrix as a product of rotation matrices
		return Matrix3d.RotationZ(angleRadians: -z) * Matrix3d.RotationY(angleRadians: theta) * Matrix3d.RotationZ(angleRadians: -zeta);
	}

	/// <summary>Computes the mean obliquity of the ecliptic (IAU 1980).</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>The mean obliquity [rad].</returns>
	/// <remarks>Accuracy about 0.01″ over ±2000 years from J2000.0.</remarks>
	public static double MeanObliquityRadians(double julianDateTt)
	{
		// Compute the mean obliquity in arcseconds using the IAU 1980 model
		double t = (julianDateTt - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		// Compute the mean obliquity in arcseconds
		double arcsec = 84381.448 - (46.8150 * t) - (0.00059 * t * t) + (0.001813 * t * t * t);
		// Convert arcseconds to radians
		return arcsec * AstronomicalConstants.ArcsecondsToRadians;
	}

	/// <summary>Computes the nutation in longitude and obliquity (IAU 1980, principal terms).</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>Δψ and Δε [rad].</returns>
	/// <remarks>Accuracy about 0.5″ in Δψ and 0.1″ in Δε.</remarks>
	public static (double DeltaPsiRadians, double DeltaEpsilonRadians) Nutation(double julianDateTt)
	{
		// Convert degrees to radians
		const double d2r = AstronomicalConstants.DegreesToRadians;
		// Time in Julian centuries since J2000.0
		double t = (julianDateTt - TimeScales.J2000) / TimeScales.DaysPerJulianCentury;
		// Longitude of the ascending node of the Moon's mean orbit on the ecliptic (Ω)
		double omega = (125.04452 - (1934.136261 * t)) * d2r;
		// Mean longitude of the Sun (l) and Moon (lp)
		double l = (280.4665 + (36000.7698 * t)) * d2r;
		double lp = (218.3165 + (481267.8813 * t)) * d2r;
		// Compute the nutation in longitude (Δψ) and obliquity (Δε) in arcseconds
		double dpsi = (-17.20 * Math.Sin(a: omega)) - (1.32 * Math.Sin(a: 2 * l)) - (0.23 * Math.Sin(a: 2 * lp)) + (0.21 * Math.Sin(a: 2 * omega));
		double deps = (9.20 * Math.Cos(d: omega)) + (0.57 * Math.Cos(d: 2 * l)) + (0.10 * Math.Cos(d: 2 * lp)) - (0.09 * Math.Cos(d: 2 * omega));
		// Convert arcseconds to radians and return the results
		return (dpsi * AstronomicalConstants.ArcsecondsToRadians, deps * AstronomicalConstants.ArcsecondsToRadians);
	}

	/// <summary>Computes the combined nutation–precession matrix from J2000.0 to the true equator and equinox of date.</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>The rotation matrix N·P.</returns>
	/// <remarks>Computes the combined nutation–precession matrix from J2000.0 to the true equator and equinox of date.</remarks>
	public static Matrix3d TrueOfDateMatrix(double julianDateTt)
	{
		// Compute the mean obliquity of the ecliptic (ε₀) in radians
		double eps0 = MeanObliquityRadians(julianDateTt: julianDateTt);
		// Compute nutation in longitude (Δψ) and obliquity (Δε)
		(double dpsi, double deps) = Nutation(julianDateTt: julianDateTt);
		// Compute the nutation matrix N = R_x(-ε₀ - Δε) · R_z(-Δψ) · R_x(ε₀)
		Matrix3d nutation = Matrix3d.RotationX(angleRadians: -(eps0 + deps)) * Matrix3d.RotationZ(angleRadians: -dpsi) * Matrix3d.RotationX(angleRadians: eps0);
		// Return the combined nutation–precession matrix
		return nutation * PrecessionMatrix(julianDateTt: julianDateTt);
	}

	/// <summary>Computes the Greenwich mean sidereal time (IAU 1982).</summary>
	/// <param name="julianDateUt1">The Julian date (UT1 ≈ UTC) [d].</param>
	/// <returns>GMST [°] in [0°, 360°).</returns>
	/// <remarks>Accuracy about 0.1″ over ±2000 years from J2000.0.</remarks>
	public static double GreenwichMeanSiderealTimeDegrees(double julianDateUt1)
	{
		// Time in days since J2000.0
		double d = julianDateUt1 - TimeScales.J2000;
		// Time in Julian centuries since J2000.0
		double t = d / TimeScales.DaysPerJulianCentury;
		// Compute GMST in degrees using the IAU 1982 model
		return NormalizeDegrees(degrees: 280.46061837 + (360.98564736629 * d) + (0.000387933 * t * t) - (t * t * t / 38710000.0));
	}

	/// <summary>Computes the Greenwich apparent sidereal time.</summary>
	/// <param name="julianDateUt1">The Julian date (UT1 ≈ UTC) [d].</param>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>GAST [°] in [0°, 360°).</returns>
	/// <remarks>Accuracy about 0.5″ over ±2000 years from J2000.0.</remarks>
	public static double GreenwichApparentSiderealTimeDegrees(double julianDateUt1, double julianDateTt)
	{
		// Compute nutation in longitude (Δψ) and obliquity (Δε)
		(double dpsi, double deps) = Nutation(julianDateTt: julianDateTt);
		// Compute the mean obliquity of the ecliptic (ε) in radians
		double eps = MeanObliquityRadians(julianDateTt: julianDateTt) + deps;
		// Compute GAST = GMST + Δψ * cos(ε) in degrees
		return NormalizeDegrees(degrees: GreenwichMeanSiderealTimeDegrees(julianDateUt1: julianDateUt1) + (dpsi * Math.Cos(d: eps) * AstronomicalConstants.RadiansToDegrees));
	}

	/// <summary>Computes the geocentric position of an observer in the true equator-of-date frame.</summary>
	/// <param name="observer">The observer location (WGS84).</param>
	/// <param name="localApparentSiderealTimeDegrees">The local apparent sidereal time [°].</param>
	/// <returns>The geocentric observer position [AU], true equator and equinox of date.</returns>
	/// <remarks>Uses the WGS84 ellipsoid to compute the geocentric position of an observer in the true equator-of-date frame. The local apparent sidereal time is used to compute the observer's position in the equatorial plane.</remarks>
	public static Vector3d ObserverPositionTrueOfDate(ObserverLocation observer, double localApparentSiderealTimeDegrees)
	{
		// Validate the observer location
		ArgumentNullException.ThrowIfNull(argument: observer);
		// Convert degrees to radians
		const double d2r = AstronomicalConstants.DegreesToRadians;
		// Compute the observer's geocentric position using the WGS84 ellipsoid
		double f = AstronomicalConstants.EarthFlattening;
		// Compute the sine and cosine of the observer's latitude
		(double sinPhi, double cosPhi) = Math.SinCos(x: observer.LatitudeDegrees * d2r);
		// Compute the radius of curvature in the prime vertical (N)
		double c = 1.0 / Math.Sqrt(d: (cosPhi * cosPhi) + ((1.0 - f) * (1.0 - f) * sinPhi * sinPhi));
		// Compute the radius of curvature in the meridian (M)
		double s = (1.0 - f) * (1.0 - f) * c;
		// Convert the observer's elevation from meters to kilometers
		double hKm = observer.ElevationMeters / 1000.0;
		// Compute the observer's position in the equatorial plane (X, Y) and Z coordinate
		double rhoCos = ((AstronomicalConstants.EarthEquatorialRadiusKm * c) + hKm) * cosPhi;
		double rhoSin = ((AstronomicalConstants.EarthEquatorialRadiusKm * s) + hKm) * sinPhi;
		// Compute the observer's position in the equatorial plane using the local apparent sidereal time
		(double sinLst, double cosLst) = Math.SinCos(x: localApparentSiderealTimeDegrees * d2r);
		// Return the geocentric observer position in AU
		return new Vector3d(X: rhoCos * cosLst, Y: rhoCos * sinLst, Z: rhoSin) / AstronomicalConstants.AstronomicalUnitKm;
	}

	/// <summary>Applies the annual aberration to a direction.</summary>
	/// <param name="direction">The geometric direction to the object (any length).</param>
	/// <param name="observerVelocity">The barycentric velocity of the observer [AU/d].</param>
	/// <returns>The apparent direction (unit vector).</returns>
	/// <remarks>Uses the relativistic formula for aberration (Explanatory Supplement, eq. 7.40). The observer velocity is assumed to be small compared to the speed of light.</remarks>
	public static Vector3d ApplyAberration(Vector3d direction, Vector3d observerVelocity)
	{
		// Normalize the direction vector to a unit vector
		Vector3d u = direction.Normalize();
		// Convert the observer velocity to a dimensionless vector (β = v/c)
		Vector3d beta = observerVelocity / AstronomicalConstants.SpeedOfLightAuPerDay;
		// Compute the inverse of the Lorentz factor (γ⁻¹ = √(1 - β·β))
		double gammaInv = Math.Sqrt(d: 1.0 - beta.Dot(other: beta));
		// Compute the dot product of the direction vector and the observer velocity (u·β)
		double ub = u.Dot(other: beta);
		// Compute the apparent direction vector using the relativistic aberration formula
		Vector3d apparent = ((gammaInv * u) + ((1.0 + (ub / (1.0 + gammaInv))) * beta)) / (1.0 + ub);
		// Normalize the apparent direction vector to a unit vector and return it
		return apparent.Normalize();
	}

	/// <summary>Converts a Cartesian equatorial vector into right ascension and declination.</summary>
	/// <param name="vector">The vector.</param>
	/// <returns>Right ascension [h] in [0 h, 24 h) and declination [°] in [−90°, +90°].</returns>
	/// <remarks>Right ascension is measured from the vernal equinox along the celestial equator. Declination is measured from the celestial equator towards the north celestial pole.</remarks>
	public static (double RightAscensionHours, double DeclinationDegrees) ToRightAscensionDeclination(Vector3d vector)
	{
		// Compute the right ascension and declination from the Cartesian vector
		double ra = Math.Atan2(y: vector.Y, x: vector.X) * AstronomicalConstants.RadiansToDegrees;
		double dec = Math.Atan2(y: vector.Z, x: Math.Sqrt(d: (vector.X * vector.X) + (vector.Y * vector.Y))) * AstronomicalConstants.RadiansToDegrees;
		// Normalize the right ascension to [0 h, 24 h) and return it along with the declination
		return (NormalizeDegrees(degrees: ra) / 15.0, dec);
	}

	/// <summary>Converts equatorial coordinates into horizontal coordinates.</summary>
	/// <param name="hourAngleDegrees">The local hour angle [°] (any value; it is normalized).</param>
	/// <param name="declinationDegrees">The declination [°].</param>
	/// <param name="latitudeDegrees">The geographic latitude of the observer [°].</param>
	/// <returns>Azimuth [°] in [0°, 360°) measured from north through east, and altitude [°] in [−90°, +90°].</returns>
	/// <remarks>Uses the standard spherical trigonometry formulas for the conversion. The azimuth is measured from north through east, and the altitude is measured from the horizon.</remarks>
	public static (double AzimuthDegrees, double AltitudeDegrees) EquatorialToHorizontal(double hourAngleDegrees, double declinationDegrees, double latitudeDegrees)
	{
		// Convert degrees to radians
		const double d2r = AstronomicalConstants.DegreesToRadians;
		// Compute the sine and cosine of the hour angle, declination, and latitude
		(double sinH, double cosH) = Math.SinCos(x: NormalizeDegrees(degrees: hourAngleDegrees) * d2r);
		(double sinD, double cosD) = Math.SinCos(x: declinationDegrees * d2r);
		(double sinP, double cosP) = Math.SinCos(x: latitudeDegrees * d2r);
		// Compute the sine of the altitude using the spherical trigonometry formula
		double sinAlt = (sinP * sinD) + (cosP * cosD * cosH);
		// Compute the altitude and azimuth in degrees
		double altitude = Math.Asin(d: Math.Clamp(value: sinAlt, min: -1.0, max: 1.0)) * AstronomicalConstants.RadiansToDegrees;
		double azimuth = Math.Atan2(y: -cosD * sinH, x: (sinD * cosP) - (cosD * sinP * cosH)) * AstronomicalConstants.RadiansToDegrees;
		// Normalize the azimuth to [0°, 360°) and return it along with the altitude
		return (NormalizeDegrees(degrees: azimuth), altitude);
	}

	/// <summary>Computes the atmospheric refraction for a standard atmosphere (Sæmundsson, 1986).</summary>
	/// <param name="trueAltitudeDegrees">The geometric (airless) altitude [°].</param>
	/// <returns>The refraction [°] to be added to the true altitude; 0 below −1°.</returns>
	/// <remarks>Uses the Sæmundsson (1986) formula for atmospheric refraction, which is accurate to about 0.1′ for altitudes above −1°. The refraction is zero below −1°.</remarks>
	public static double RefractionDegrees(double trueAltitudeDegrees)
	{
		// Return 0.0 for altitudes below −1° or above 90°
		if (trueAltitudeDegrees is < -1.0 or >= 90.0)
		{
			return 0.0;
		}
		// Compute the refraction in arcminutes using the Sæmundsson (1986) formula
		double h = trueAltitudeDegrees;
		double arcminutes = 1.02 / Math.Tan(a: (h + (10.3 / (h + 5.11))) * AstronomicalConstants.DegreesToRadians);
		// Correct the formula so that it yields exactly zero at the zenith and return the refraction in degrees, ensuring it is non-negative
		return Math.Max(val1: 0.0, val2: arcminutes + 0.0019279) / 60.0;
	}

	/// <summary>Formats a right ascension as "HHh MMm SS.SSs" using the invariant culture.</summary>
	/// <param name="hours">The right ascension [h].</param>
	/// <returns>The formatted string.</returns>
	/// <remarks>Right ascension is formatted in hours, minutes, and seconds with two decimal places for seconds. The invariant culture is used to ensure consistent formatting regardless of the system's locale settings.</remarks>
	public static string FormatRightAscension(double hours)
	{
		// Normalize the hours to [0 h, 24 h) and convert to total seconds
		double totalSeconds = Math.Round(value: NormalizeHours(hours: hours) * 3600.0, digits: 2);
		// Wrap around if the total seconds exceed 24 hours (86400 seconds)
		if (totalSeconds >= 86400.0)
		{
			totalSeconds -= 86400.0;
		}
		// Compute hours, minutes, and seconds from total seconds
		int h = (int)(totalSeconds / 3600.0);
		int m = (int)((totalSeconds - (h * 3600.0)) / 60.0);
		double s = totalSeconds - (h * 3600.0) - (m * 60.0);
		// Format the right ascension as "HHh MMm SS.SSs" using the invariant culture
		return string.Create(provider: CultureInfo.InvariantCulture, handler: $"{h:00}h {m:00}m {s:00.00}s");
	}

	/// <summary>Formats a declination as "±DD° MM′ SS.S″" using the invariant culture.</summary>
	/// <param name="degrees">The declination [°].</param>
	/// <returns>The formatted string (always with sign, also for −0° xx′).</returns>
	/// <remarks>Declination is formatted in degrees, minutes, and seconds with one decimal place for seconds. The invariant culture is used to ensure consistent formatting regardless of the system's locale settings.</remarks>
	public static string FormatDeclination(double degrees)
	{
		// Round the absolute value of degrees to total seconds and determine the sign
		char sign = degrees < 0.0 ? '-' : '+';
		// Compute total seconds from the absolute value of degrees
		double totalSeconds = Math.Round(value: Math.Abs(value: degrees) * 3600.0, digits: 1);
		// Compute degrees, minutes, and seconds from total seconds
		int d = (int)(totalSeconds / 3600.0);
		int m = (int)((totalSeconds - (d * 3600.0)) / 60.0);
		double s = totalSeconds - (d * 3600.0) - (m * 60.0);
		// Format the declination as "±DD° MM′ SS.S″" using the invariant culture
		return string.Create(provider: CultureInfo.InvariantCulture, handler: $"{sign}{d:00}° {m:00}′ {s:00.0}″");
	}
}
