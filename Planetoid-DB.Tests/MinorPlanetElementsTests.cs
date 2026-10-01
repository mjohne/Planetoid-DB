/*
 * File:        MinorPlanetElementsTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests the parsing and validation of MPCORB records.
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

/// <summary>Tests the parsing and validation of MPCORB records.</summary>
public sealed class MinorPlanetElementsTests
{
	/// <summary>A valid record is parsed with all fields.</summary>
	[Fact]
	public void TryParse_ValidRecord_ReturnsElements()
	{
		Assert.True(condition: MinorPlanetElements.TryParse(record: TestData.BuildRecord(), elements: out MinorPlanetElements? elements, error: out string? error), userMessage: error);
		Assert.Equal(expected: "(1) Ceres", actual: elements.Designation);
		Assert.Equal(expected: 3.33, actual: elements.AbsoluteMagnitude, precision: 10);
		Assert.Equal(expected: 2460800.5, actual: elements.EpochJulianDateTt, precision: 10);
		Assert.Equal(expected: 0.0795434, actual: elements.Eccentricity, precision: 10);
		Assert.Equal(expected: 2.7660512, actual: elements.SemiMajorAxisAu, precision: 10);
	}

	/// <summary>A missing H is tolerated and yields an unknown magnitude.</summary>
	[Fact]
	public void TryParse_MissingAbsoluteMagnitude_UsesNaN()
	{
		MinorPlanetElements elements = MinorPlanetElements.Parse(record: TestData.BuildRecord(h: "", g: ""));
		Assert.True(condition: double.IsNaN(d: elements.AbsoluteMagnitude));
		Assert.Equal(expected: MinorPlanetElements.DefaultSlopeParameter, actual: elements.SlopeParameter);
	}

	/// <summary>Invalid records are rejected with an error message.</summary>
	/// <param name="caseName">The name of the invalid case.</param>
	[Theory]
	[InlineData("null")]
	[InlineData("empty")]
	[InlineData("truncated")]
	[InlineData("no designation")]
	[InlineData("bad epoch")]
	[InlineData("non-numeric element")]
	[InlineData("hyperbolic")]
	[InlineData("negative eccentricity")]
	[InlineData("zero semi-major axis")]
	[InlineData("inclination out of range")]
	[InlineData("mean anomaly out of range")]
	public void TryParse_InvalidRecord_ReturnsError(string caseName)
	{
		string? record = caseName switch
		{
			"null" => null,
			"empty" => "   ",
			"truncated" => TestData.BuildRecord()[..90],
			"no designation" => TestData.BuildRecord(designation: ""),
			"bad epoch" => TestData.BuildRecord(epoch: "X2555"),
			"non-numeric element" => TestData.BuildRecord(inclination: "abc"),
			"hyperbolic" => TestData.BuildRecord(eccentricity: "1.0000000"),
			"negative eccentricity" => TestData.BuildRecord(eccentricity: "-0.1"),
			"zero semi-major axis" => TestData.BuildRecord(semiMajorAxis: "0.0"),
			"inclination out of range" => TestData.BuildRecord(inclination: "181.0"),
			"mean anomaly out of range" => TestData.BuildRecord(meanAnomaly: "360.0"),
			_ => throw new ArgumentOutOfRangeException(paramName: nameof(caseName))
		};
		Assert.False(condition: MinorPlanetElements.TryParse(record: record, elements: out MinorPlanetElements? elements, error: out string? error));
		Assert.Null(@object: elements);
		Assert.False(condition: string.IsNullOrWhiteSpace(value: error));
		_ = Assert.Throws<FormatException>(testCode: () => MinorPlanetElements.Parse(record: record));
	}

	/// <summary>Packed epochs are decoded, including letters for days above 9.</summary>
	/// <param name="packed">The packed epoch.</param>
	/// <param name="expected">The expected Julian date.</param>
	[Theory]
	[InlineData("K2555", 2460800.5)]
	[InlineData("J9611", 2450083.5)]
	[InlineData("K251V", 2460706.5)]
	public void TryDecodePackedEpoch_ValidEpoch_ReturnsJulianDate(string packed, double expected)
	{
		Assert.True(condition: MinorPlanetElements.TryDecodePackedEpoch(packedEpoch: packed, julianDateTt: out double jd));
		Assert.Equal(expected: expected, actual: jd, precision: 6);
	}
}
