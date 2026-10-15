/*
 * File:        AstronomicalCoordinateConverter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Converts positions between common astronomical coordinate systems.
 * Remarks: All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

namespace Planetoid_DB.Services;

/// <summary>Converts angular positions among common astronomical coordinate systems.</summary>
/// <remarks>All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.</remarks>
internal static class AstronomicalCoordinateConverter
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Rotation matrix from equatorial J2000.0 to galactic coordinates.</summary>
	/// <remarks>From the IAU 1958 galactic coordinate system definition.</remarks>
	private static readonly Matrix3d equatorialToGalactic = new(
		M11: -0.0548755604162154, M12: -0.8734370902348850, M13: -0.4838350155487132,
		M21: 0.4941094278755837, M22: -0.4448296299600112, M23: 0.7469822444972189,
		M31: -0.8676661490190047, M32: -0.1980763734312015, M33: 0.4559837761750669);

	/// <summary>Rotation matrix from galactic to supergalactic coordinates.</summary>
	/// <remarks>From the de Vaucouleurs supergalactic coordinate system definition.</remarks>
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
	public static (double FirstAngleDegrees, double SecondAngleDegrees) Convert(double firstAngleDegrees, double secondAngleDegrees, AstronomicalCoordinateSystem source, AstronomicalCoordinateSystem target, DateTime dateTimeUtc, double observerLatitudeDegrees, double observerLongitudeDegrees, bool firstCoordinateIsHourAngle = false)
	{
		// Validate the input angles and coordinate system requirements.
		ValidateAngles(firstAngleDegrees: firstAngleDegrees, secondAngleDegrees: secondAngleDegrees);
		// Validate the input angles and coordinate system requirements.
		if (secondAngleDegrees is < -90.0 or > 90.0)
		{
			// Log an error message for the second coordinate being out of range.
			logger.Error(message: "The second coordinate must be between -90° and +90°.");
			// The second coordinate must be between -90° and +90°.
			throw new ArgumentOutOfRangeException(paramName: nameof(secondAngleDegrees), message: "The second coordinate must be between -90° and +90°.");
		}
		// Validate the input angles and coordinate system requirements.
		if (firstCoordinateIsHourAngle && source != AstronomicalCoordinateSystem.EquatorialOfDate)
		{
			// Log an error message for the first coordinate being a local hour angle in a non-equatorial-of-date system.
			logger.Error(message: "A local hour angle can only be supplied for equatorial coordinates of date.");
			// A local hour angle can only be supplied for equatorial coordinates of date.
			throw new ArgumentException(message: "A local hour angle can only be supplied for equatorial coordinates of date.", paramName: nameof(firstCoordinateIsHourAngle));
		}
		// Validate the input angles and coordinate system requirements.
		bool needsObserver = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| firstCoordinateIsHourAngle;
		// Validate the input angles and coordinate system requirements.
		if (needsObserver)
		{
			if (!double.IsFinite(d: observerLatitudeDegrees) || observerLatitudeDegrees is < -90.0 or > 90.0)
			{
				// Log an error message for the observer latitude being out of range.
				logger.Error(message: "Observer latitude must be between -90° and +90°.");
				// Observer latitude must be between -90° and +90°.
				throw new ArgumentOutOfRangeException(paramName: nameof(observerLatitudeDegrees), message: "Observer latitude must be between -90° and +90°.");
			}
			// Validate the input angles and coordinate system requirements.
			if (!double.IsFinite(d: observerLongitudeDegrees))
			{
				// Log an error message for the observer longitude being out of range.
				logger.Error(message: "Observer longitude must be a finite number.");
				// Observer longitude must be a finite number.
				throw new ArgumentOutOfRangeException(paramName: nameof(observerLongitudeDegrees), message: "Observer longitude must be a finite number.");
			}
		}
		// Convert the input angles to a J2000 vector, then convert that vector to the target coordinate system.
		DateTime utc = DateTime.SpecifyKind(value: dateTimeUtc, kind: DateTimeKind.Utc);
		// Calculate the Julian dates for UT1 and TT, and compute the local sidereal time in degrees.
		double julianDateUt1 = TimeScales.ToJulianDate(dateTime: utc);
		double julianDateTt = TimeScales.ToJulianDateTt(time: new DateTimeOffset(dateTime: utc, offset: TimeSpan.Zero));
		// Calculate the local sidereal time in degrees, normalized to [0, 360).
		double localSiderealTimeDegrees = CoordinateTransformationService.NormalizeDegrees(degrees: CoordinateTransformationService.GreenwichMeanSiderealTimeDegrees(julianDateUt1: julianDateUt1) + observerLongitudeDegrees);
		// Calculate the precession matrix and mean obliquity of date in radians.
		Matrix3d precession = CoordinateTransformationService.PrecessionMatrix(julianDateTt: julianDateTt);
		// Calculate the mean obliquity of date in radians and the ecliptic-to-equatorial rotation matrix.
		double obliquityRadians = CoordinateTransformationService.MeanObliquityRadians(julianDateTt: julianDateTt);
		// Calculate the ecliptic-to-equatorial rotation matrix for the date of observation.
		Matrix3d eclipticToEquatorialOfDate = Matrix3d.RotationX(angleRadians: -obliquityRadians);
		// Convert the input angles to a J2000 vector, then convert that vector to the target coordinate system.
		Vector3d j2000Vector = ToJ2000Vector(firstAngleDegrees: firstAngleDegrees, secondAngleDegrees: secondAngleDegrees, source: source, firstCoordinateIsHourAngle: firstCoordinateIsHourAngle, localSiderealTimeDegrees: localSiderealTimeDegrees, observerLatitudeDegrees: observerLatitudeDegrees, precession: precession, eclipticToEquatorialOfDate: eclipticToEquatorialOfDate);
		// Convert the J2000 vector to the target coordinate system and return the resulting angles.
		return FromJ2000Vector(vector: j2000Vector, target: target, localSiderealTimeDegrees: localSiderealTimeDegrees, observerLatitudeDegrees: observerLatitudeDegrees, precession: precession, eclipticToEquatorialOfDate: eclipticToEquatorialOfDate);
	}

	/// <summary>Converts input coordinates from a specified source astronomical coordinate system to a J2000 vector.</summary>
	/// <param name="firstAngleDegrees">The first angle in degrees.</param>
	/// <param name="secondAngleDegrees">The second angle in degrees.</param>
	/// <param name="source">The source astronomical coordinate system.</param>
	/// <param name="firstCoordinateIsHourAngle">Indicates whether the first coordinate is an hour angle.</param>
	/// <param name="localSiderealTimeDegrees">The local sidereal time in degrees.</param>
	/// <param name="observerLatitudeDegrees">The observer's latitude in degrees.</param>
	/// <param name="precession">The precession matrix.</param>
	/// <param name="eclipticToEquatorialOfDate">The matrix to convert ecliptic coordinates to equatorial coordinates of date.</param>
	/// <returns>The J2000 vector corresponding to the input coordinates.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the source astronomical coordinate system is not recognized.</exception>
	/// <remarks>This method converts coordinates from the specified source astronomical coordinate system to a J2000 vector, taking into account precession and the conversion from ecliptic to equatorial coordinates of date.</remarks>
	private static Vector3d ToJ2000Vector(double firstAngleDegrees, double secondAngleDegrees, AstronomicalCoordinateSystem source, bool firstCoordinateIsHourAngle, double localSiderealTimeDegrees, double observerLatitudeDegrees, Matrix3d precession, Matrix3d eclipticToEquatorialOfDate)
	{
		// Convert the input angles to a spherical vector representation.
		Vector3d inputVector = SphericalVector(firstAngleDegrees: firstAngleDegrees, secondAngleDegrees: secondAngleDegrees);
		// Convert the input coordinates from the specified source astronomical coordinate system to a J2000 vector.
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

	/// <summary>Converts a J2000 vector to the specified target astronomical coordinate system.</summary>
	/// <param name="vector">The J2000 vector to convert.</param>
	/// <param name="target">The target astronomical coordinate system.</param>
	/// <param name="localSiderealTimeDegrees">The local sidereal time in degrees.</param>
	/// <param name="observerLatitudeDegrees">The observer's latitude in degrees.</param>
	/// <param name="precession">The precession matrix.</param>
	/// <param name="eclipticToEquatorialOfDate">The ecliptic to equatorial of date conversion matrix.</param>
	/// <returns>The converted angles in degrees.</returns>
	/// <exception cref="InvalidOperationException">Thrown when attempting to convert to horizontal coordinates, which are already handled separately.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the target astronomical coordinate system is not recognized.</exception>
	/// <remarks>This method handles the conversion of a J2000 vector to various astronomical coordinate systems, applying the necessary precession and transformation matrices.</remarks>
	private static (double FirstAngleDegrees, double SecondAngleDegrees) FromJ2000Vector(Vector3d vector, AstronomicalCoordinateSystem target, double localSiderealTimeDegrees, double observerLatitudeDegrees, Matrix3d precession, Matrix3d eclipticToEquatorialOfDate)
	{
		// If the target coordinate system is horizontal, convert the J2000 vector to horizontal coordinates using the precession matrix and observer's location.
		if (target == AstronomicalCoordinateSystem.Horizontal)
		{
			// Convert the J2000 vector to horizontal coordinates using the precession matrix and observer's location.
			return ToHorizontal(equatorialVectorOfDate: precession * vector, localSiderealTimeDegrees: localSiderealTimeDegrees, observerLatitudeDegrees: observerLatitudeDegrees);
		}
		// For other target coordinate systems, apply the appropriate transformations to obtain the target vector.
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
		// Convert the target vector to spherical coordinates and return the resulting angles in degrees.
		return SphericalCoordinates(vector: targetVector);
	}

	/// <summary>Converts horizontal coordinates (azimuth and altitude) to equatorial coordinates (right ascension and declination) for a given observer's location and local sidereal time.</summary>
	/// <param name="azimuthDegrees">The azimuth angle in degrees.</param>
	/// <param name="altitudeDegrees">The altitude angle in degrees.</param>
	/// <param name="localSiderealTimeDegrees">The local sidereal time in degrees.</param>
	/// <param name="observerLatitudeDegrees">The observer's latitude in degrees.</param>
	/// <returns>A 3D vector representing the equatorial coordinates.</returns>
	/// <remarks>This method performs the conversion from horizontal coordinates to equatorial coordinates using trigonometric relationships and the observer's location.</remarks>
	private static Vector3d HorizontalToEquatorial(double azimuthDegrees, double altitudeDegrees, double localSiderealTimeDegrees, double observerLatitudeDegrees)
	{
		// Convert the input angles from degrees to radians for trigonometric calculations.
		const double DegreesToRadians = AstronomicalConstants.DegreesToRadians;
		// Convert the azimuth, altitude, observer's latitude, and local sidereal time from degrees to radians.
		double azimuth = azimuthDegrees * DegreesToRadians;
		double altitude = altitudeDegrees * DegreesToRadians;
		double latitude = observerLatitudeDegrees * DegreesToRadians;
		double siderealTime = localSiderealTimeDegrees * DegreesToRadians;
		// Calculate the sine and cosine values for azimuth, altitude, latitude, and local sidereal time.
		(double sinAzimuth, double cosAzimuth) = Math.SinCos(x: azimuth);
		(double sinAltitude, double cosAltitude) = Math.SinCos(x: altitude);
		(double sinLatitude, double cosLatitude) = Math.SinCos(x: latitude);
		(double sinSiderealTime, double cosSiderealTime) = Math.SinCos(x: siderealTime);
		// Calculate the equatorial coordinates (X, Y, Z) based on the horizontal coordinates and observer's location.
		double east = cosAltitude * sinAzimuth;
		double north = cosAltitude * cosAzimuth;
		double up = sinAltitude;
		// Return the equatorial coordinates as a 3D vector.
		return new Vector3d(
			X: (cosLatitude * cosSiderealTime * up) - (sinLatitude * cosSiderealTime * north) - (sinSiderealTime * east),
			Y: (cosLatitude * sinSiderealTime * up) - (sinLatitude * sinSiderealTime * north) + (cosSiderealTime * east),
			Z: (sinLatitude * up) + (cosLatitude * north));
	}

	/// <summary>Converts equatorial coordinates (right ascension and declination) to horizontal coordinates (azimuth and altitude) for a given observer's location and local sidereal time.</summary>
	/// <param name="equatorialVectorOfDate">The equatorial coordinates vector of date.</param>
	/// <param name="localSiderealTimeDegrees">The local sidereal time in degrees.</param>
	/// <param name="observerLatitudeDegrees">The observer's latitude in degrees.</param>
	/// <returns>A tuple containing the azimuth and altitude in degrees.</returns>
	/// <remarks>This method converts equatorial coordinates to horizontal coordinates using the observer's location and local sidereal time.</remarks>
	private static (double FirstAngleDegrees, double SecondAngleDegrees) ToHorizontal(Vector3d equatorialVectorOfDate, double localSiderealTimeDegrees, double observerLatitudeDegrees)
	{
		// Convert the equatorial vector of date to right ascension and declination.
		(double rightAscensionHours, double declinationDegrees) = CoordinateTransformationService.ToRightAscensionDeclination(vector: equatorialVectorOfDate);
		// Calculate the hour angle in degrees based on the local sidereal time and right ascension.
		double hourAngleDegrees = CoordinateTransformationService.NormalizeDegrees(degrees: localSiderealTimeDegrees - (rightAscensionHours * 15.0));
		// Convert the equatorial coordinates (hour angle and declination) to horizontal coordinates (azimuth and altitude).
		(double azimuthDegrees, double altitudeDegrees) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: hourAngleDegrees, declinationDegrees: declinationDegrees, latitudeDegrees: observerLatitudeDegrees);
		// Return the azimuth and altitude in degrees.
		return (FirstAngleDegrees: azimuthDegrees, SecondAngleDegrees: altitudeDegrees);
	}

	/// <summary>Converts spherical coordinates (longitude and latitude in degrees) to a 3D vector.</summary>
	/// <param name="firstAngleDegrees">The first angle in degrees (typically longitude).</param>
	/// <param name="secondAngleDegrees">The second angle in degrees (typically latitude).</param>
	/// <returns>A 3D vector representing the spherical coordinates.</returns>
	/// <remarks>This method converts spherical coordinates (longitude and latitude) in degrees to a 3D Cartesian vector.</remarks>
	private static Vector3d SphericalVector(double firstAngleDegrees, double secondAngleDegrees)
	{
		// Convert the input angles from degrees to radians for trigonometric calculations.
		const double DegreesToRadians = AstronomicalConstants.DegreesToRadians;
		double longitude = firstAngleDegrees * DegreesToRadians;
		double latitude = secondAngleDegrees * DegreesToRadians;
		// Calculate the sine and cosine values for longitude and latitude.
		(double sinLongitude, double cosLongitude) = Math.SinCos(x: longitude);
		(double sinLatitude, double cosLatitude) = Math.SinCos(x: latitude);
		// Return the 3D vector representation of the spherical coordinates.
		return new Vector3d(X: cosLatitude * cosLongitude, Y: cosLatitude * sinLongitude, Z: sinLatitude);
	}

	/// <summary>Converts a 3D vector to spherical coordinates (longitude and latitude in degrees).</summary>
	/// <param name="vector">The 3D vector to convert.</param>
	/// <returns>A tuple containing the first angle (longitude) and second angle (latitude) in degrees.</returns>
	/// <remarks>This method converts a 3D Cartesian vector to spherical coordinates (longitude and latitude) in degrees.</remarks>
	private static (double FirstAngleDegrees, double SecondAngleDegrees) SphericalCoordinates(Vector3d vector)
	{
		// Calculate the longitude and latitude in degrees from the 3D vector.
		double longitude = Math.Atan2(y: vector.Y, x: vector.X) * AstronomicalConstants.RadiansToDegrees;
		double latitude = Math.Atan2(y: vector.Z, x: Math.Sqrt(d: (vector.X * vector.X) + (vector.Y * vector.Y))) * AstronomicalConstants.RadiansToDegrees;
		// Normalize the longitude to the range [0, 360) degrees and return the longitude and latitude.
		return (CoordinateTransformationService.NormalizeDegrees(degrees: longitude), latitude);
	}

	/// <summary>Validates that the input angles are finite numbers. Throws an ArgumentOutOfRangeException if either angle is not finite.</summary>
	/// <param name="firstAngleDegrees">The first angle (longitude) in degrees.</param>
	/// <param name="secondAngleDegrees">The second angle (latitude) in degrees.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when either angle is not a finite number.</exception>
	/// <remarks>This method checks if the provided angles are finite numbers and throws an exception if they are not, ensuring valid input for coordinate conversion.</remarks>
	private static void ValidateAngles(double firstAngleDegrees, double secondAngleDegrees)
	{
		// Validate that the input angles are finite numbers. If either angle is not finite, throw an ArgumentOutOfRangeException.
		if (!double.IsFinite(d: firstAngleDegrees))
		{
			// Log an error message for the first coordinate being out of range.
			logger.Error(message: "The first coordinate must be a finite number.");
			// The first coordinate must be a finite number.
			throw new ArgumentOutOfRangeException(paramName: nameof(firstAngleDegrees));
		}
		// Validate that the input angles are finite numbers. If either angle is not finite, throw an ArgumentOutOfRangeException.
		if (!double.IsFinite(d: secondAngleDegrees))
		{
			// Log an error message for the second coordinate being out of range.
			logger.Error(message: "The second coordinate must be a finite number.");
			// The second coordinate must be a finite number.
			throw new ArgumentOutOfRangeException(paramName: nameof(secondAngleDegrees));
		}
	}
}
