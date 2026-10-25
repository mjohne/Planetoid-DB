/*
 * File:        AstronomicalCoordinateConverterTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests for astronomical coordinate conversions.
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

using System.Diagnostics;

namespace Planetoid_DB.Tests;

/// <summary>Tests conversions among astronomical coordinate systems.</summary>
/// <remarks>All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.</remarks>
// Debugger display attribute provides a string representation for the debugger display, showing the result of the DebuggerDisplay property without quotes.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class AstronomicalCoordinateConverterTests
{
	/// <summary>Provides a string representation for the debugger display.</summary>
	/// <remarks>Returns the string representation of the object or an empty string if null.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Verifies the standard J2000 position of the Galactic center transforms to the Galactic origin.</summary>
	/// <remarks>Galactic longitude is defined to be 0° at the Galactic center, and Galactic latitude is defined to be 0° in the Galactic plane.</remarks>
	// Fact data is provided for the Galactic center coordinates in J2000.0 to test that they transform to the Galactic origin in Galactic coordinates.
	[Fact]
	public void GalacticCenterTransformsToGalacticOrigin()
	{
		// The Galactic center is at RA 17h 45m 40.04s, Dec -29° 00′ 28.1″ (J2000.0), which is approximately 266.4051°, -28.936175° in decimal degrees.
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 266.4051,
			secondAngleDegrees: -28.936175,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.Galactic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);
		// Galactic longitude is defined to be 0° at the Galactic center, and Galactic latitude is defined to be 0° in the Galactic plane.
		Assert.True(condition: longitude is < 0.01 or > 359.99);
		Assert.InRange(actual: latitude, low: -0.01, high: 0.01);
	}

	/// <summary>Verifies IAU 1976 precession against the independent spherical-coordinate formula.</summary>
	/// <remarks>Precession is applied to the mean equator of date, which is the same as the mean equator of J2000.0 at the epoch.</remarks>
	// Fact data is provided for a specific J2000 coordinate and date to test that the precession to the mean equator of date matches the expected result computed using the independent spherical-coordinate formula for precession.
	[Fact]
	public void J2000CoordinatesPrecessToKnownMeanEquatorOfDate()
	{
		// The expected result is computed using the independent spherical-coordinate formula for precession, which is accurate to 0.01 arcseconds over the range 1900–2100.
		(double rightAscension, double declination) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 123.456,
			secondAngleDegrees: -21.987,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.EquatorialOfDate,
			dateTimeUtc: new DateTime(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);
		// The expected result is computed using the independent spherical-coordinate formula for precession, which is accurate to 0.01 arcseconds over the range 1900–2100.
		Assert.Equal(expected: 123.745314216, actual: rightAscension, precision: 8);
		Assert.Equal(expected: -22.068496426, actual: declination, precision: 8);
	}

	/// <summary>Verifies the mean-ecliptic rotation at J2000.0 using the standard mean obliquity.</summary>
	/// <remarks>The mean obliquity of the ecliptic at J2000.0 is approximately 23.4392911111111°.</remarks>
	// Fact data is provided for a specific equatorial coordinate and the mean obliquity of the ecliptic at J2000.0 to test that the transformation to ecliptic coordinates matches the expected result.
	[Fact]
	public void MeanEquatorialCoordinatesAtJ2000TransformToKnownEclipticCoordinates()
	{
		// The mean obliquity of the ecliptic at J2000.0 is approximately 23.4392911111111°.
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 90.0,
			secondAngleDegrees: 23.4392911111111,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.Ecliptic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 11, minute: 58, second: 55, millisecond: 816, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);
		// The expected result is computed using the standard mean obliquity of the ecliptic at J2000.0, which is approximately 23.4392911111111°.
		Assert.Equal(expected: 90.0, actual: longitude, precision: 8);
		Assert.Equal(expected: 0.0, actual: latitude, precision: 8);
	}

	/// <summary>Verifies the standard hour-angle-to-horizontal geometry at the equator.</summary>
	/// <remarks>At the equator, an object on the celestial equator with a local hour angle of 90° is on the western horizon.</remarks>
	// Fact data is provided for a specific local hour angle and observer location at the equator to test that the transformation to horizontal coordinates matches the expected result of being on the western horizon.
	[Fact]
	public void EquatorialHourAngleTransformsToKnownHorizontalCoordinates()
	{
		// At the equator, an object on the celestial equator with a local hour angle of 90° is on the western horizon.
		(double azimuth, double altitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 90.0,
			secondAngleDegrees: 0.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.Horizontal,
			dateTimeUtc: new DateTime(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0,
			firstCoordinateIsHourAngle: true);
		// At the equator, an object on the celestial equator with a local hour angle of 90° is on the western horizon, which corresponds to an azimuth of 270° and an altitude of 0°.
		Assert.Equal(expected: 270.0, actual: azimuth, precision: 8);
		Assert.Equal(expected: 0.0, actual: altitude, precision: 8);
	}

	/// <summary>Verifies the published J2000 position of the north supergalactic pole.</summary>
	/// <remarks>The north supergalactic pole is at approximately RA 18h 55m 00s, Dec +15° 42′ 00″ (J2000.0), which is approximately 283.754090343°, +15.708935529° in decimal degrees.</remarks>						
	// Fact data is provided for the north supergalactic pole coordinates in J2000.0 to test that they transform to the expected supergalactic latitude.
	[Fact]
	public void NorthSupergalacticPoleTransformsToKnownSupergalacticLatitude()
	{
		// The north supergalactic pole is at approximately RA 18h 55m 00s, Dec +15° 42′ 00″ (J2000.0), which is approximately 283.754090343°, +15.708935529° in decimal degrees.
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 283.754090343,
			secondAngleDegrees: 15.708935529,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.Supergalactic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);
		// The north supergalactic pole is defined to be at supergalactic latitude +90°, so the longitude is irrelevant.
		_ = longitude;
		Assert.InRange(actual: latitude, low: 89.999, high: 90.0);
	}

	/// <summary>Verifies coordinate round trips through every supported frame, including date-dependent and horizontal frames.</summary>
	/// <remarks>Round trips through date-dependent frames are performed at a specific date and time, and round trips through horizontal frames are performed at a specific observer location.</remarks>
	// Theory data is provided for each intermediate frame to test round-trip conversions from J2000 coordinates: EquatorialOfDate, Ecliptic, Galactic, Supergalactic, and Horizontal.
	[Theory]
	// The following frames are tested for round-trip conversions from J2000 coordinates: EquatorialOfDate, Ecliptic, Galactic, Supergalactic, and Horizontal.
	[InlineData(data: nameof(AstronomicalCoordinateSystem.EquatorialOfDate))]
	[InlineData(data: nameof(AstronomicalCoordinateSystem.Ecliptic))]
	[InlineData(data: nameof(AstronomicalCoordinateSystem.Galactic))]
	[InlineData(data: nameof(AstronomicalCoordinateSystem.Supergalactic))]
	[InlineData(data: nameof(AstronomicalCoordinateSystem.Horizontal))]
	public void J2000CoordinatesRoundTripThroughOtherFrames(string intermediateFrame)
	{
		// The intermediate frame is specified as a string and parsed to the corresponding AstronomicalCoordinateSystem enum value.
		AstronomicalCoordinateSystem intermediate = Enum.Parse<AstronomicalCoordinateSystem>(value: intermediateFrame);
		// The round-trip conversion is performed at a specific date and time, and the observer location is specified for horizontal frames.
		DateTime dateTime = new(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc);
		// The original J2000 coordinates are specified in degrees.
		const double originalLongitude = 123.456;
		const double originalLatitude = -21.987;
		// The round-trip conversion is performed through the specified intermediate frame, and the results are compared to the original J2000 coordinates.
		(double intermediateLongitude, double intermediateLatitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: originalLongitude,
			secondAngleDegrees: originalLatitude,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: intermediate,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: 51.5,
			observerLongitudeDegrees: -0.1);
		// The round-trip conversion is performed through the specified intermediate frame, and the results are compared to the original J2000 coordinates.
		(double roundTripLongitude, double roundTripLatitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: intermediateLongitude,
			secondAngleDegrees: intermediateLatitude,
			source: intermediate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: 51.5,
			observerLongitudeDegrees: -0.1);
		// The round-trip conversion is performed through the specified intermediate frame, and the results are compared to the original J2000 coordinates.
		double longitudeDifference = Math.Abs(value: roundTripLongitude - originalLongitude);
		longitudeDifference = Math.Min(val1: longitudeDifference, val2: 360.0 - longitudeDifference);
		Assert.InRange(actual: longitudeDifference, low: 0.0, high: 1e-9);
		Assert.Equal(expected: originalLatitude, actual: roundTripLatitude, precision: 9);
	}

	/// <summary>Verifies hour angle input is interpreted as local sidereal time minus right ascension.</summary>
	/// <remarks>Hour angle is defined as local sidereal time minus right ascension, so the hour angle input should produce the same result as the right ascension input when converted to J2000 coordinates.</remarks>
	// Fact data is provided for a specific date and time, observer location, and right ascension value to test the hour angle input against the right ascension input.
	[Fact]
	public void HourAngleInputMatchesRightAscensionInput()
	{
		// The hour angle input is interpreted as local sidereal time minus right ascension, so the hour angle input should produce the same result as the right ascension input when converted to J2000 coordinates.
		DateTime dateTime = new(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc);
		// The observer location is specified for the hour angle input, and the right ascension value is specified in degrees.
		const double latitude = 51.5;
		const double longitude = -0.1;
		const double rightAscensionDegrees = 120.0;
		// The local sidereal time is computed from the Greenwich mean sidereal time and the observer's longitude, and the hour angle is computed as local sidereal time minus right ascension.
		double localSiderealDegrees = CoordinateTransformationService.NormalizeDegrees(
			degrees: CoordinateTransformationService.GreenwichMeanSiderealTimeDegrees(julianDateUt1: TimeScales.ToJulianDate(dateTime: dateTime)) + longitude);
		double hourAngleDegrees = localSiderealDegrees - rightAscensionDegrees;
		// The hour angle input is interpreted as local sidereal time minus right ascension, so the hour angle input should produce the same result as the right ascension input when converted to J2000 coordinates.
		(double raResult, double decResult) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: rightAscensionDegrees,
			secondAngleDegrees: 20.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: latitude,
			observerLongitudeDegrees: longitude);
		// The hour angle input is interpreted as local sidereal time minus right ascension, so the hour angle input should produce the same result as the right ascension input when converted to J2000 coordinates.
		(double hourAngleResult, double hourAngleDecResult) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: hourAngleDegrees,
			secondAngleDegrees: 20.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: latitude,
			observerLongitudeDegrees: longitude,
			firstCoordinateIsHourAngle: true);
		// The hour angle input is interpreted as local sidereal time minus right ascension, so the hour angle input should produce the same result as the right ascension input when converted to J2000 coordinates.
		Assert.Equal(expected: raResult, actual: hourAngleResult, precision: 11);
		Assert.Equal(expected: decResult, actual: hourAngleDecResult, precision: 11);
	}

	/// <summary>Verifies invalid latitude values are rejected instead of producing invalid coordinates.</summary>
	/// <remarks>Latitude values must be in the range [-90°, +90°], and any value outside this range should throw an ArgumentOutOfRangeException.</remarks>
	// Fact data is provided for an invalid latitude value of 91.0° to test that it is rejected by the converter.
	[Fact]
	public void InvalidLatitudeIsRejected()
	{
		// Latitude values must be in the range [-90°, +90°], and any value outside this range should throw an ArgumentOutOfRangeException.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 10.0,
			secondAngleDegrees: 91.0,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.Galactic,
			dateTimeUtc: DateTime.UtcNow,
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0));
	}
}
