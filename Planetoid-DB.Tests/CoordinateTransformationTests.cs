/*
 * File:        CoordinateTransformationTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests coordinate transformations, angle normalization and formatting.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Services;

namespace Planetoid_DB.Tests;

/// <summary>Tests coordinate transformations, angle normalization and formatting.</summary>
public sealed class CoordinateTransformationTests
{
	/// <summary>Angles outside [0, 360) are normalized, including azimuths above 360°.</summary>
	/// <param name="input">The input angle.</param>
	/// <param name="expected">The normalized angle.</param>
	[Theory]
	[InlineData(370.0, 10.0)]
	[InlineData(720.0, 0.0)]
	[InlineData(-10.0, 350.0)]
	[InlineData(-720.5, 359.5)]
	[InlineData(359.999, 359.999)]
	public void NormalizeDegrees_ReturnsRangeZeroTo360(double input, double expected)
	{
		double result = CoordinateTransformationService.NormalizeDegrees(degrees: input);
		Assert.Equal(expected: expected, actual: result, precision: 9);
		Assert.InRange(actual: result, low: 0.0, high: 359.9999999999);
	}

	/// <summary>The azimuth stays within [0, 360) even for sidereal times of many revolutions.</summary>
	[Fact]
	public void ToHorizontal_LargeSiderealTime_AzimuthIsNormalized()
	{
		for (int k = -5; k <= 5; k++)
		{
			for (double ra = 0.0; ra < 24.0; ra += 1.7)
			{
				(double az, _) = CoordinateTransformationService.ToHorizontal(rightAscensionHours: ra, declinationDegrees: 20.0, localApparentSiderealTimeRadians: (k * 2.0 * Math.PI) + 1.0, latitudeDegrees: 48.0);
				Assert.InRange(actual: az, low: 0.0, high: 359.9999999999);
			}
		}
	}

	/// <summary>An object on the meridian south of the zenith has azimuth 180°; one in the north has 0°.</summary>
	[Fact]
	public void ToHorizontal_Meridian_GivesSouthAndNorth()
	{
		(double azSouth, double altSouth) = CoordinateTransformationService.ToHorizontal(rightAscensionHours: 6.0, declinationDegrees: 0.0, localApparentSiderealTimeRadians: Math.PI / 2.0, latitudeDegrees: 50.0);
		Assert.Equal(expected: 180.0, actual: azSouth, precision: 6);
		Assert.Equal(expected: 40.0, actual: altSouth, precision: 6);
		(double azNorth, double altNorth) = CoordinateTransformationService.ToHorizontal(rightAscensionHours: 6.0, declinationDegrees: 80.0, localApparentSiderealTimeRadians: Math.PI / 2.0, latitudeDegrees: 50.0);
		Assert.True(condition: azNorth is < 1e-6 or > 360.0 - 1e-6, userMessage: azNorth.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
		Assert.Equal(expected: 60.0, actual: altNorth, precision: 6);
	}

	/// <summary>An object with a strongly negative declination is below the horizon for a northern observer.</summary>
	[Fact]
	public void ToHorizontal_SouthernObjectFromNorth_IsBelowHorizon()
	{
		(_, double altitude) = CoordinateTransformationService.ToHorizontal(rightAscensionHours: 3.0, declinationDegrees: -80.0, localApparentSiderealTimeRadians: 0.7, latitudeDegrees: 50.0);
		Assert.True(condition: altitude < 0.0);
	}

	/// <summary>Negative declinations are calculated with the correct sign.</summary>
	[Fact]
	public void ToRightAscensionDeclination_NegativeDeclination()
	{
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: new Vector3D(X: 0.0, Y: 1.0, Z: -1.0));
		Assert.Equal(expected: 6.0, actual: ra, precision: 9);
		Assert.Equal(expected: -45.0, actual: dec, precision: 9);
		(double ra2, _) = CoordinateTransformationService.ToRightAscensionDeclination(vector: new Vector3D(X: 0.0, Y: -1.0, Z: 0.0));
		Assert.Equal(expected: 18.0, actual: ra2, precision: 9);
	}

	/// <summary>Negative declinations between 0° and −1° keep their sign in sexagesimal notation.</summary>
	/// <param name="degrees">The declination.</param>
	/// <param name="expected">The expected text.</param>
	[Theory]
	[InlineData(-0.5, "-00 30 00.0")]
	[InlineData(-12.5125, "-12 30 45.0")]
	[InlineData(0.0, "+00 00 00.0")]
	[InlineData(89.99999, "+90 00 00.0")]
	public void FormatDeclination_KeepsSign(double degrees, string expected)
		=> Assert.Equal(expected: expected, actual: EphemerisExportService.FormatDeclination(degrees: degrees));

	/// <summary>Right ascensions are normalized and roll over at 24 h.</summary>
	/// <param name="hours">The right ascension in hours.</param>
	/// <param name="expected">The expected text.</param>
	[Theory]
	[InlineData(23.9999999, "00 00 00.00")]
	[InlineData(-1.0, "23 00 00.00")]
	[InlineData(25.5, "01 30 00.00")]
	[InlineData(12.0, "12 00 00.00")]
	public void FormatRightAscension_Normalizes(double hours, string expected)
		=> Assert.Equal(expected: expected, actual: EphemerisExportService.FormatRightAscension(hours: hours));

	/// <summary>Refraction lifts objects near the horizon by about 0.5°.</summary>
	[Fact]
	public void ApplyRefraction_AtHorizon_IsAboutHalfDegree()
	{
		double refracted = CoordinateTransformationService.ApplyRefraction(geometricAltitudeDegrees: 0.0);
		Assert.InRange(actual: refracted, low: 0.45, high: 0.65);
		Assert.Equal(expected: 90.0, actual: CoordinateTransformationService.ApplyRefraction(geometricAltitudeDegrees: 90.0), precision: 3);
	}

	/// <summary>Observer coordinates are validated.</summary>
	/// <param name="latitude">The latitude.</param>
	/// <param name="longitude">The longitude.</param>
	/// <param name="valid">Whether the location is valid.</param>
	[Theory]
	[InlineData(-90.0, -180.0, true)]
	[InlineData(90.0, 359.9, true)]
	[InlineData(90.1, 0.0, false)]
	[InlineData(0.0, 360.0, false)]
	[InlineData(0.0, -180.1, false)]
	[InlineData(double.NaN, 0.0, false)]
	public void ObserverLocation_Validate(double latitude, double longitude, bool valid)
	{
		ObserverLocation location = new(LatitudeDegrees: latitude, LongitudeDegrees: longitude);
		if (valid)
		{
			location.Validate();
		}
		else
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: location.Validate);
		}
	}
}
