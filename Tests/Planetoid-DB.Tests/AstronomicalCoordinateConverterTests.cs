/*
 * File:        AstronomicalCoordinateConverterTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests for astronomical coordinate conversions.
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
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class AstronomicalCoordinateConverterTests
{
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Verifies the standard J2000 position of the Galactic center transforms to the Galactic origin.</summary>
	[Fact]
	public void GalacticCenterTransformsToGalacticOrigin()
	{
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 266.4051,
			secondAngleDegrees: -28.936175,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.Galactic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);

		Assert.True(condition: longitude is < 0.01 or > 359.99);
		Assert.InRange(actual: latitude, low: -0.01, high: 0.01);
	}

	/// <summary>Verifies IAU 1976 precession against the independent spherical-coordinate formula.</summary>
	[Fact]
	public void J2000CoordinatesPrecessToKnownMeanEquatorOfDate()
	{
		(double rightAscension, double declination) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 123.456,
			secondAngleDegrees: -21.987,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.EquatorialOfDate,
			dateTimeUtc: new DateTime(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);

		Assert.Equal(expected: 123.745314216, actual: rightAscension, precision: 8);
		Assert.Equal(expected: -22.068496426, actual: declination, precision: 8);
	}

	/// <summary>Verifies the mean-ecliptic rotation at J2000.0 using the standard mean obliquity.</summary>
	[Fact]
	public void MeanEquatorialCoordinatesAtJ2000TransformToKnownEclipticCoordinates()
	{
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 90.0,
			secondAngleDegrees: 23.4392911111111,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.Ecliptic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 11, minute: 58, second: 55, millisecond: 816, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);

		Assert.Equal(expected: 90.0, actual: longitude, precision: 8);
		Assert.Equal(expected: 0.0, actual: latitude, precision: 8);
	}

	/// <summary>Verifies the standard hour-angle-to-horizontal geometry at the equator.</summary>
	[Fact]
	public void EquatorialHourAngleTransformsToKnownHorizontalCoordinates()
	{
		(double azimuth, double altitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 90.0,
			secondAngleDegrees: 0.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.Horizontal,
			dateTimeUtc: new DateTime(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0,
			firstCoordinateIsHourAngle: true);

		Assert.Equal(expected: 270.0, actual: azimuth, precision: 8);
		Assert.Equal(expected: 0.0, actual: altitude, precision: 8);
	}

	/// <summary>Verifies the published J2000 position of the north supergalactic pole.</summary>
	[Fact]
	public void NorthSupergalacticPoleTransformsToKnownSupergalacticLatitude()
	{
		(double longitude, double latitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: 283.754090343,
			secondAngleDegrees: 15.708935529,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: AstronomicalCoordinateSystem.Supergalactic,
			dateTimeUtc: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc),
			observerLatitudeDegrees: 0.0,
			observerLongitudeDegrees: 0.0);

		_ = longitude;
		Assert.InRange(actual: latitude, low: 89.999, high: 90.0);
	}

	/// <summary>Verifies coordinate round trips through every supported frame, including date-dependent and horizontal frames.</summary>
	[Theory]
	[InlineData(nameof(AstronomicalCoordinateSystem.EquatorialOfDate))]
	[InlineData(nameof(AstronomicalCoordinateSystem.Ecliptic))]
	[InlineData(nameof(AstronomicalCoordinateSystem.Galactic))]
	[InlineData(nameof(AstronomicalCoordinateSystem.Supergalactic))]
	[InlineData(nameof(AstronomicalCoordinateSystem.Horizontal))]
	public void J2000CoordinatesRoundTripThroughOtherFrames(string intermediateFrame)
	{
		AstronomicalCoordinateSystem intermediate = Enum.Parse<AstronomicalCoordinateSystem>(value: intermediateFrame);
		DateTime dateTime = new(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc);
		const double originalLongitude = 123.456;
		const double originalLatitude = -21.987;

		(double intermediateLongitude, double intermediateLatitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: originalLongitude,
			secondAngleDegrees: originalLatitude,
			source: AstronomicalCoordinateSystem.EquatorialJ2000,
			target: intermediate,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: 51.5,
			observerLongitudeDegrees: -0.1);
		(double roundTripLongitude, double roundTripLatitude) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: intermediateLongitude,
			secondAngleDegrees: intermediateLatitude,
			source: intermediate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: 51.5,
			observerLongitudeDegrees: -0.1);

		double longitudeDifference = Math.Abs(value: roundTripLongitude - originalLongitude);
		longitudeDifference = Math.Min(val1: longitudeDifference, val2: 360.0 - longitudeDifference);
		Assert.InRange(actual: longitudeDifference, low: 0.0, high: 1e-9);
		Assert.Equal(expected: originalLatitude, actual: roundTripLatitude, precision: 9);
	}

	/// <summary>Verifies hour angle input is interpreted as local sidereal time minus right ascension.</summary>
	[Fact]
	public void HourAngleInputMatchesRightAscensionInput()
	{
		DateTime dateTime = new(year: 2026, month: 6, day: 15, hour: 3, minute: 30, second: 0, kind: DateTimeKind.Utc);
		const double latitude = 51.5;
		const double longitude = -0.1;
		const double rightAscensionDegrees = 120.0;
		double localSiderealDegrees = CoordinateTransformationService.NormalizeDegrees(
			degrees: CoordinateTransformationService.GreenwichMeanSiderealTimeDegrees(julianDateUt1: TimeScales.ToJulianDate(dateTime: dateTime)) + longitude);
		double hourAngleDegrees = localSiderealDegrees - rightAscensionDegrees;

		(double raResult, double decResult) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: rightAscensionDegrees,
			secondAngleDegrees: 20.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: latitude,
			observerLongitudeDegrees: longitude);
		(double hourAngleResult, double hourAngleDecResult) = AstronomicalCoordinateConverter.Convert(
			firstAngleDegrees: hourAngleDegrees,
			secondAngleDegrees: 20.0,
			source: AstronomicalCoordinateSystem.EquatorialOfDate,
			target: AstronomicalCoordinateSystem.EquatorialJ2000,
			dateTimeUtc: dateTime,
			observerLatitudeDegrees: latitude,
			observerLongitudeDegrees: longitude,
			firstCoordinateIsHourAngle: true);

		Assert.Equal(expected: raResult, actual: hourAngleResult, precision: 11);
		Assert.Equal(expected: decResult, actual: hourAngleDecResult, precision: 11);
	}

	/// <summary>Verifies invalid latitude values are rejected instead of producing invalid coordinates.</summary>
	[Fact]
	public void InvalidLatitudeIsRejected()
	{
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
