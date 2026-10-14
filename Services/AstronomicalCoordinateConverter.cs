/*
 * File:        AstronomicalCoordinateConverter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Converts positions between common astronomical coordinate systems.
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

/// <summary>Identifies a supported astronomical coordinate system.</summary>
internal enum AstronomicalCoordinateSystem
{
	/// <summary>Horizontal azimuth and altitude for the supplied observer and time.</summary>
	Horizontal,
	/// <summary>Equatorial coordinates in the fixed J2000.0 frame.</summary>
	EquatorialJ2000,
	/// <summary>Equatorial coordinates in the mean equator and equinox of date.</summary>
	EquatorialOfDate,
	/// <summary>Ecliptic coordinates in the mean ecliptic of date.</summary>
	Ecliptic,
	/// <summary>Galactic coordinates in the IAU-defined galactic frame.</summary>
	Galactic,
	/// <summary>Supergalactic coordinates in the de Vaucouleurs frame.</summary>
	Supergalactic
}

/// <summary>Converts angular positions among common astronomical coordinate systems.</summary>
/// <remarks>All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.</remarks>
internal static class AstronomicalCoordinateConverter
{
	private static readonly Matrix3d equatorialToGalactic = new(
		M11: -0.0548755604162154, M12: -0.8734370902348850, M13: -0.4838350155487132,
		M21: 0.4941094278755837, M22: -0.4448296299600112, M23: 0.7469822444972189,
		M31: -0.8676661490190047, M32: -0.1980763734312015, M33: 0.4559837761750669);

	private static readonly Matrix3d galacticToSupergalactic = new(
		M11: -0.735742574804, M12: 0.677261296414, M13: 0.0,
		M21: -0.074553778365, M22: -0.080991471307, M23: 0.993922590400,
		M31: 0.673145302109, M32: 0.731271165817, M33: 0.110081262225);

	/// <summary>Converts coordinates between two astronomical reference frames.</summary>
	/// <param name="firstAngleDegrees">The first input coordinate in degrees (right ascension or hour angle, longitude, or azimuth).</param>
	/// <param name="secondAngleDegrees">The second input coordinate in degrees (declination, latitude, altitude, or elevation).</param>
	/// <param name="source">The input coordinate system.</param>
	/// <param name="target">The desired output coordinate system.</param>
	/// <param name="dateTimeUtc">The UTC date and time used for date-dependent frames and local sidereal time.</param>
	/// <param name="observerLatitudeDegrees">The observer's geodetic latitude [°].</param>
	/// <param name="observerLongitudeDegrees">The observer's longitude east of Greenwich [°].</param>
	/// <param name="firstCoordinateIsHourAngle">Whether the first input coordinate is local hour angle rather than right ascension. This is supported only for equatorial-of-date input.</param>
	/// <returns>The first and second output coordinates in degrees.</returns>
	/// <remarks>Rotation-only transforms are used; proper motion, parallax, aberration, refraction, and polar motion are not applied. Horizontal coordinates use azimuth measured from north through east.</remarks>
	public static (double FirstAngleDegrees, double SecondAngleDegrees) Convert(
		double firstAngleDegrees,
		double secondAngleDegrees,
		AstronomicalCoordinateSystem source,
		AstronomicalCoordinateSystem target,
		DateTime dateTimeUtc,
		double observerLatitudeDegrees,
		double observerLongitudeDegrees,
		bool firstCoordinateIsHourAngle = false)
	{
		ValidateAngles(firstAngleDegrees: firstAngleDegrees, secondAngleDegrees: secondAngleDegrees);
		if (secondAngleDegrees is < -90.0 or > 90.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(secondAngleDegrees), message: "The second coordinate must be between -90° and +90°.");
		}
		if (firstCoordinateIsHourAngle && source != AstronomicalCoordinateSystem.EquatorialOfDate)
		{
			throw new ArgumentException(message: "A local hour angle can only be supplied for equatorial coordinates of date.", paramName: nameof(firstCoordinateIsHourAngle));
		}

		bool needsObserver = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| firstCoordinateIsHourAngle;
		if (needsObserver)
		{
			if (!double.IsFinite(observerLatitudeDegrees) || observerLatitudeDegrees is < -90.0 or > 90.0)
			{
				throw new ArgumentOutOfRangeException(paramName: nameof(observerLatitudeDegrees), message: "Observer latitude must be between -90° and +90°.");
			}
			if (!double.IsFinite(observerLongitudeDegrees))
			{
				throw new ArgumentOutOfRangeException(paramName: nameof(observerLongitudeDegrees));
			}
		}

		DateTime utc = DateTime.SpecifyKind(value: dateTimeUtc, kind: DateTimeKind.Utc);
		double julianDateUt1 = TimeScales.ToJulianDate(dateTime: utc);
		double julianDateTt = TimeScales.ToJulianDateTt(time: new DateTimeOffset(dateTime: utc, offset: TimeSpan.Zero));
		double localSiderealTimeDegrees = CoordinateTransformationService.NormalizeDegrees(
			degrees: CoordinateTransformationService.GreenwichMeanSiderealTimeDegrees(julianDateUt1: julianDateUt1) + observerLongitudeDegrees);
		Matrix3d precession = CoordinateTransformationService.PrecessionMatrix(julianDateTt: julianDateTt);
		double obliquityRadians = CoordinateTransformationService.MeanObliquityRadians(julianDateTt: julianDateTt);
		Matrix3d eclipticToEquatorialOfDate = Matrix3d.RotationX(angleRadians: -obliquityRadians);

		Vector3d j2000Vector = ToJ2000Vector(
			firstAngleDegrees: firstAngleDegrees,
			secondAngleDegrees: secondAngleDegrees,
			source: source,
			firstCoordinateIsHourAngle: firstCoordinateIsHourAngle,
			localSiderealTimeDegrees: localSiderealTimeDegrees,
			observerLatitudeDegrees: observerLatitudeDegrees,
			precession: precession,
			eclipticToEquatorialOfDate: eclipticToEquatorialOfDate);

		return FromJ2000Vector(
			vector: j2000Vector,
			target: target,
			localSiderealTimeDegrees: localSiderealTimeDegrees,
			observerLatitudeDegrees: observerLatitudeDegrees,
			precession: precession,
			eclipticToEquatorialOfDate: eclipticToEquatorialOfDate);
	}

	private static Vector3d ToJ2000Vector(
		double firstAngleDegrees,
		double secondAngleDegrees,
		AstronomicalCoordinateSystem source,
		bool firstCoordinateIsHourAngle,
		double localSiderealTimeDegrees,
		double observerLatitudeDegrees,
		Matrix3d precession,
		Matrix3d eclipticToEquatorialOfDate)
	{
		Vector3d inputVector = SphericalVector(firstAngleDegrees: firstAngleDegrees, secondAngleDegrees: secondAngleDegrees);
		return source switch
		{
			AstronomicalCoordinateSystem.Horizontal => precession.Transpose() * HorizontalToEquatorial(
				azimuthDegrees: firstAngleDegrees,
				altitudeDegrees: secondAngleDegrees,
				localSiderealTimeDegrees: localSiderealTimeDegrees,
				observerLatitudeDegrees: observerLatitudeDegrees),
			AstronomicalCoordinateSystem.EquatorialJ2000 => inputVector,
			AstronomicalCoordinateSystem.EquatorialOfDate => precession.Transpose() * SphericalVector(
				firstAngleDegrees: firstCoordinateIsHourAngle
					? CoordinateTransformationService.NormalizeDegrees(degrees: localSiderealTimeDegrees - firstAngleDegrees)
					: firstAngleDegrees,
				secondAngleDegrees: secondAngleDegrees),
			AstronomicalCoordinateSystem.Ecliptic => precession.Transpose() * (eclipticToEquatorialOfDate * inputVector),
			AstronomicalCoordinateSystem.Galactic => equatorialToGalactic.Transpose() * inputVector,
			AstronomicalCoordinateSystem.Supergalactic => equatorialToGalactic.Transpose() * (galacticToSupergalactic.Transpose() * inputVector),
			_ => throw new ArgumentOutOfRangeException(paramName: nameof(source))
		};
	}

	private static (double FirstAngleDegrees, double SecondAngleDegrees) FromJ2000Vector(
		Vector3d vector,
		AstronomicalCoordinateSystem target,
		double localSiderealTimeDegrees,
		double observerLatitudeDegrees,
		Matrix3d precession,
		Matrix3d eclipticToEquatorialOfDate)
	{
		if (target == AstronomicalCoordinateSystem.Horizontal)
		{
			return ToHorizontal(
				equatorialVectorOfDate: precession * vector,
				localSiderealTimeDegrees: localSiderealTimeDegrees,
				observerLatitudeDegrees: observerLatitudeDegrees);
		}

		Vector3d targetVector = target switch
		{
			AstronomicalCoordinateSystem.Horizontal => throw new InvalidOperationException("Horizontal coordinates were already handled."),
			AstronomicalCoordinateSystem.EquatorialJ2000 => vector,
			AstronomicalCoordinateSystem.EquatorialOfDate => precession * vector,
			AstronomicalCoordinateSystem.Ecliptic => eclipticToEquatorialOfDate.Transpose() * (precession * vector),
			AstronomicalCoordinateSystem.Galactic => equatorialToGalactic * vector,
			AstronomicalCoordinateSystem.Supergalactic => galacticToSupergalactic * (equatorialToGalactic * vector),
			_ => throw new ArgumentOutOfRangeException(paramName: nameof(target))
		};
		return SphericalCoordinates(vector: targetVector);
	}

	private static Vector3d HorizontalToEquatorial(double azimuthDegrees, double altitudeDegrees, double localSiderealTimeDegrees, double observerLatitudeDegrees)
	{
		const double DegreesToRadians = AstronomicalConstants.DegreesToRadians;
		double azimuth = azimuthDegrees * DegreesToRadians;
		double altitude = altitudeDegrees * DegreesToRadians;
		double latitude = observerLatitudeDegrees * DegreesToRadians;
		double siderealTime = localSiderealTimeDegrees * DegreesToRadians;
		(double sinAzimuth, double cosAzimuth) = Math.SinCos(x: azimuth);
		(double sinAltitude, double cosAltitude) = Math.SinCos(x: altitude);
		(double sinLatitude, double cosLatitude) = Math.SinCos(x: latitude);
		(double sinSiderealTime, double cosSiderealTime) = Math.SinCos(x: siderealTime);
		double east = cosAltitude * sinAzimuth;
		double north = cosAltitude * cosAzimuth;
		double up = sinAltitude;
		return new Vector3d(
			X: (cosLatitude * cosSiderealTime * up) - (sinLatitude * cosSiderealTime * north) - (sinSiderealTime * east),
			Y: (cosLatitude * sinSiderealTime * up) - (sinLatitude * sinSiderealTime * north) + (cosSiderealTime * east),
			Z: (sinLatitude * up) + (cosLatitude * north));
	}

	private static (double FirstAngleDegrees, double SecondAngleDegrees) ToHorizontal(Vector3d equatorialVectorOfDate, double localSiderealTimeDegrees, double observerLatitudeDegrees)
	{
		(double rightAscensionHours, double declinationDegrees) = CoordinateTransformationService.ToRightAscensionDeclination(vector: equatorialVectorOfDate);
		double hourAngleDegrees = CoordinateTransformationService.NormalizeDegrees(degrees: localSiderealTimeDegrees - (rightAscensionHours * 15.0));
		(double azimuthDegrees, double altitudeDegrees) = CoordinateTransformationService.EquatorialToHorizontal(
			hourAngleDegrees: hourAngleDegrees,
			declinationDegrees: declinationDegrees,
			latitudeDegrees: observerLatitudeDegrees);
		return (azimuthDegrees, altitudeDegrees);
	}

	private static Vector3d SphericalVector(double firstAngleDegrees, double secondAngleDegrees)
	{
		const double DegreesToRadians = AstronomicalConstants.DegreesToRadians;
		double longitude = firstAngleDegrees * DegreesToRadians;
		double latitude = secondAngleDegrees * DegreesToRadians;
		(double sinLongitude, double cosLongitude) = Math.SinCos(x: longitude);
		(double sinLatitude, double cosLatitude) = Math.SinCos(x: latitude);
		return new Vector3d(X: cosLatitude * cosLongitude, Y: cosLatitude * sinLongitude, Z: sinLatitude);
	}

	private static (double FirstAngleDegrees, double SecondAngleDegrees) SphericalCoordinates(Vector3d vector)
	{
		double longitude = Math.Atan2(y: vector.Y, x: vector.X) * AstronomicalConstants.RadiansToDegrees;
		double latitude = Math.Atan2(y: vector.Z, x: Math.Sqrt(d: (vector.X * vector.X) + (vector.Y * vector.Y))) * AstronomicalConstants.RadiansToDegrees;
		return (CoordinateTransformationService.NormalizeDegrees(degrees: longitude), latitude);
	}

	private static void ValidateAngles(double firstAngleDegrees, double secondAngleDegrees)
	{
		if (!double.IsFinite(firstAngleDegrees))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(firstAngleDegrees));
		}
		if (!double.IsFinite(secondAngleDegrees))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(secondAngleDegrees));
		}
	}
}
