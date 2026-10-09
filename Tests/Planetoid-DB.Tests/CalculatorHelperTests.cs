/*
 * File:        CalculatorHelperTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for the calculation helpers.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="AverageCalculator"/>, <see cref="DerivedElements"/> and <see cref="TisserandParameterCalculator"/>.</summary>
/// <remarks>These tests verify the correctness of the calculation helpers for averages, derived orbital elements, and Tisserand parameters.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class CalculatorHelperTests
{
	/// <summary>Verifies basic averages and that invalid values are ignored.</summary>
	/// <remarks>The test data set contains some invalid values (NaN and Infinity) which should be ignored by the average calculations.</remarks>
	[Fact]
	public void AverageCalculatorComputesBasicAverages()
	{
		// Arrange a known data set with some invalid values (NaN and Infinity).
		double[] values = [1, 2, 2, 3, 4, double.NaN, double.PositiveInfinity];
		// The valid values are [1, 2, 2, 3, 4], which have an arithmetic mean of 2.4, a median of 2.0, and a mode of 2.0.
		Assert.Equal(expected: 2.4, actual: AverageCalculator.ArithmeticMean(values: values), precision: 12);
		Assert.Equal(expected: 2.0, actual: AverageCalculator.Median(values: values), precision: 12);
		Assert.Equal(expected: 2.0, actual: AverageCalculator.Mode(values: values), precision: 12);
		Assert.Equal(expected: 2.5, actual: AverageCalculator.Median(values: [1, 2, 3, 4]), precision: 12);
	}

	/// <summary>Verifies classical means for a known data set.</summary>
	/// <remarks>Classical means include geometric, harmonic, and quadratic means.</remarks>
	[Fact]
	public void AverageCalculatorComputesClassicalMeans()
	{
		// Arrange a known data set with two values.
		double[] values = [1, 4];
		// The geometric mean is sqrt(1*4) = 2, the harmonic mean is 2/(1/1 + 1/4) = 1.6, and the quadratic mean is sqrt((1^2 + 4^2)/2) = sqrt(8.5).
		Assert.Equal(expected: 2.0, actual: AverageCalculator.GeometricMean(values: values), precision: 12);
		Assert.Equal(expected: 1.6, actual: AverageCalculator.HarmonicMean(values: values), precision: 12);
		Assert.Equal(expected: Math.Sqrt(d: 8.5), actual: AverageCalculator.QuadraticMean(values: values), precision: 12);
	}

	/// <summary>Verifies empty input yields NaN.</summary>
	/// <remarks>All average calculations should return NaN for an empty input array.</remarks>
	[Fact]
	public void AverageCalculatorReturnsNaNForEmptyInput()
	{
		// Arrange an empty data set.
		double[] values = [];
		// Act & Assert
		Assert.True(condition: double.IsNaN(d: AverageCalculator.ArithmeticMean(values: values)));
		Assert.True(condition: double.IsNaN(d: AverageCalculator.Median(values: values)));
	}

	/// <summary>Verifies derived orbital elements for a known ellipse.</summary>
	/// <remarks>This test checks the calculations of semi-minor axis, perihelion distance, aphelion distance, and orbital period for a known elliptical orbit.</remarks>
	[Fact]
	public void DerivedElementsComputesEllipseQuantities()
	{
		// Arrange known values for a semi-major axis and numerical eccentricity.
		Assert.Equal(expected: 1.6, actual: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 0.8, actual: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 3.2, actual: DerivedElements.CalculateAphelionDistance(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 8.0, actual: DerivedElements.CalculatePeriod(semiMajorAxis: 4.0), precision: 12);
	}

	/// <summary>Verifies the eccentric anomaly satisfies Kepler's equation.</summary>
	/// <remarks>This test checks that the calculated eccentric anomaly satisfies Kepler's equation for a given mean anomaly and numerical eccentricity.</remarks>
	[Fact]
	public void DerivedElementsEccentricAnomalySatisfiesKeplerEquation()
	{
		// A mean anomaly of 60 degrees is chosen for this test.
		const double meanAnomalyDeg = 60.0;
		// A numerical eccentricity of 0.3 is chosen for this test.
		const double e = 0.3;
		// Calculate the eccentric anomaly and verify it satisfies Kepler's equation.
		double eccentricAnomaly = DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomalyDeg, numericalEccentricity: e);
		// Kepler's equation: M = E - e * sin(E), where M is the mean anomaly, E is the eccentric anomaly, and e is the numerical eccentricity.
		double eRad = double.DegreesToRadians(degrees: eccentricAnomaly);
		// Calculate the mean anomaly from the eccentric anomaly and numerical eccentricity.
		double recovered = eRad - (e * Math.Sin(a: eRad));
		// Assert that the recovered mean anomaly matches the original mean anomaly (in radians) within a reasonable precision.
		Assert.Equal(expected: double.DegreesToRadians(degrees: meanAnomalyDeg), actual: recovered, precision: 6);
	}

	/// <summary>Verifies Tisserand parameters for a circular coplanar orbit at Jupiter's distance equal 3.</summary>
	/// <remarks>This test checks that the Tisserand parameter for a circular coplanar orbit at Jupiter's distance is equal to 3.</remarks>
	[Fact]
	public void TisserandJupiterOrbitYieldsThree()
	{
		// Arrange a circular coplanar orbit at Jupiter's distance (semi-major axis = 5.20336301 AU, eccentricity = 0, inclination = 0 degrees).
		TisserandParameterCalculator.TisserandResult[] results = TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: 5.20336301, eccentricity: 0.0, inclinationDeg: 0.0);
		// There are 8 planets in the solar system, so we expect 8 results.
		Assert.Equal(expected: 8, actual: results.Length);
		// The Tisserand parameter for a circular coplanar orbit at Jupiter's distance should be equal to 3.
		Assert.Equal(expected: 3.0, actual: results.Single(r => r.PlanetName == "Jupiter").TisserandValue, precision: 9);
	}

	/// <summary>Verifies invalid orbit inputs are rejected.</summary>
	/// <remarks>This test checks that invalid semi-major axis and eccentricity values are rejected by the Tisserand parameter calculator.</remarks>
	[Theory]
	[InlineData(1.0, 1.0)]
	[InlineData(1.0, -0.1)]
	[InlineData(0.0, 0.5)]
	[InlineData(double.NaN, 0.5)]
	[InlineData(double.PositiveInfinity, 0.5)]
	[InlineData(1.0, double.NaN)]
	[InlineData(1.0, double.PositiveInfinity)]
	public void TisserandRejectsInvalidInputs(double semiMajorAxis, double eccentricity)
	{
		// Act & Assert: Expect an ArgumentOutOfRangeException for invalid semi-major axis or eccentricity values.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: semiMajorAxis, eccentricity: eccentricity, inclinationDeg: 0.0));
	}

	/// <summary>Verifies non-finite inclinations are rejected.</summary>
	/// <remarks>This test checks that non-finite inclination values (NaN, positive infinity, negative infinity) are rejected by the Tisserand parameter calculator.</remarks>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void TisserandRejectsNonFiniteInclination(double inclinationDeg)
	{
		// Act & Assert: Expect an ArgumentOutOfRangeException for non-finite inclination values.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: 1.0, eccentricity: 0.5, inclinationDeg: inclinationDeg));
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
