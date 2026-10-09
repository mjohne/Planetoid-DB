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

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="AverageCalculator"/>, <see cref="DerivedElements"/> and <see cref="TisserandParameterCalculator"/>.</summary>
public sealed class CalculatorHelperTests
{
	/// <summary>Verifies basic averages and that invalid values are ignored.</summary>
	[Fact]
	public void AverageCalculator_ComputesBasicAverages()
	{
		double[] values = [1, 2, 2, 3, 4, double.NaN, double.PositiveInfinity];

		Assert.Equal(expected: 2.4, actual: AverageCalculator.ArithmeticMean(values: values), precision: 12);
		Assert.Equal(expected: 2.0, actual: AverageCalculator.Median(values: values), precision: 12);
		Assert.Equal(expected: 2.0, actual: AverageCalculator.Mode(values: values), precision: 12);
		Assert.Equal(expected: 2.5, actual: AverageCalculator.Median(values: [1, 2, 3, 4]), precision: 12);
	}

	/// <summary>Verifies classical means for a known data set.</summary>
	[Fact]
	public void AverageCalculator_ComputesClassicalMeans()
	{
		double[] values = [1, 4];

		Assert.Equal(expected: 2.0, actual: AverageCalculator.GeometricMean(values: values), precision: 12);
		Assert.Equal(expected: 1.6, actual: AverageCalculator.HarmonicMean(values: values), precision: 12);
		Assert.Equal(expected: Math.Sqrt(d: 8.5), actual: AverageCalculator.QuadraticMean(values: values), precision: 12);
	}

	/// <summary>Verifies empty input yields NaN.</summary>
	[Fact]
	public void AverageCalculator_ReturnsNaNForEmptyInput()
	{
		Assert.True(condition: double.IsNaN(d: AverageCalculator.ArithmeticMean(values: [])));
		Assert.True(condition: double.IsNaN(d: AverageCalculator.Median(values: [])));
	}

	/// <summary>Verifies derived orbital elements for a known ellipse.</summary>
	[Fact]
	public void DerivedElements_ComputesEllipseQuantities()
	{
		Assert.Equal(expected: 1.6, actual: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 0.8, actual: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 3.2, actual: DerivedElements.CalculateAphelionDistance(semiMajorAxis: 2.0, numericalEccentricity: 0.6), precision: 12);
		Assert.Equal(expected: 8.0, actual: DerivedElements.CalculatePeriod(semiMajorAxis: 4.0), precision: 12);
	}

	/// <summary>Verifies the eccentric anomaly satisfies Kepler's equation.</summary>
	[Fact]
	public void DerivedElements_EccentricAnomalySatisfiesKeplerEquation()
	{
		const double meanAnomalyDeg = 60.0;
		const double e = 0.3;
		double eccentricAnomaly = DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomalyDeg, numericalEccentricity: e);
		double eRad = double.DegreesToRadians(degrees: eccentricAnomaly);
		double recovered = eRad - (e * Math.Sin(a: eRad));
		Assert.Equal(expected: double.DegreesToRadians(degrees: meanAnomalyDeg), actual: recovered, precision: 6);
	}

	/// <summary>Verifies Tisserand parameters for a circular coplanar orbit at Jupiter's distance equal 3.</summary>
	[Fact]
	public void Tisserand_JupiterOrbitYieldsThree()
	{
		TisserandParameterCalculator.TisserandResult[] results = TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: 5.20336301, eccentricity: 0.0, inclinationDeg: 0.0);

		Assert.Equal(expected: 8, actual: results.Length);
		Assert.Equal(expected: 3.0, actual: results.Single(r => r.PlanetName == "Jupiter").TisserandValue, precision: 9);
	}

	/// <summary>Verifies invalid orbit inputs are rejected.</summary>
	[Theory]
	[InlineData(1.0, 1.0)]
	[InlineData(1.0, -0.1)]
	[InlineData(0.0, 0.5)]
	[InlineData(double.NaN, 0.5)]
	[InlineData(double.PositiveInfinity, 0.5)]
	[InlineData(1.0, double.NaN)]
	[InlineData(1.0, double.PositiveInfinity)]
	public void Tisserand_RejectsInvalidInputs(double semiMajorAxis, double eccentricity)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: semiMajorAxis, eccentricity: eccentricity, inclinationDeg: 0.0));
	}

	/// <summary>Verifies non-finite inclinations are rejected.</summary>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void Tisserand_RejectsNonFiniteInclination(double inclinationDeg)
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => TisserandParameterCalculator.CalculateTisserandParameters(semiMajorAxis: 1.0, eccentricity: 0.5, inclinationDeg: inclinationDeg));
	}
}
