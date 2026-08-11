/*
 * File:        TisserandParameterCalculator.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides methods for calculating the Tisserand parameter of a minor planet relative to each of the eight solar system planets.
 *
 * Autor:       Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Provides methods for calculating the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>The Tisserand parameter is a quasi-conserved quantity derived from the Jacobi constant in the circular restricted three-body problem. It is defined as: <para><c>T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))</c></para> where <c>a_P</c> is the semi-major axis of the reference planet, <c>a</c> is the semi-major axis of the minor planet, <c>e</c> is the eccentricity of the minor planet, and <c>i</c> is the orbital inclination of the minor planet. By convention <c>T_J</c> (relative to Jupiter) is the most commonly used form and is widely employed to classify small solar-system bodies.</remarks>
internal class TisserandParameterCalculator
{
	/// <summary>Represents the name and semi-major axis of a solar system planet used in Tisserand parameter calculations.</summary>
	/// <param name="Name">The common name of the planet.</param>
	/// <param name="SemiMajorAxis">The semi-major axis in AU (J2000.0 mean elements).</param>
	/// <remarks>Only the semi-major axis is required for the Tisserand parameter formula.</remarks>
	public readonly record struct PlanetData(string Name, double SemiMajorAxis);

	/// <summary>Represents the Tisserand parameter result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the reference planet.</param>
	/// <param name="TisserandValue">The computed Tisserand parameter value (dimensionless).</param>
	/// <remarks>Values near 3 (relative to Jupiter) indicate a Jupiter-family comet or Jupiter-crossing orbit. Values greater than 3 typically indicate an asteroid, while values less than 2 suggest a nearly isotropic comet.</remarks>
	public readonly record struct TisserandResult(string PlanetName, double TisserandValue);

	/// <summary>Mean semi-major axes of the eight solar system planets at J2000.0.</summary>
	/// <remarks>Values are taken from the standard IAU/JPL mean orbital elements (Standish, E.M. 1992, "Keplerian Elements for Approximate Planetary Positions").</remarks>
	private static readonly PlanetData[] Planets =
	[
		new(Name: "Mercury", SemiMajorAxis: 0.38709893),
		new(Name: "Venus",   SemiMajorAxis: 0.72333199),
		new(Name: "Earth",   SemiMajorAxis: 1.00000011),
		new(Name: "Mars",    SemiMajorAxis: 1.52366231),
		new(Name: "Jupiter", SemiMajorAxis: 5.20336301),
		new(Name: "Saturn",  SemiMajorAxis: 9.53707032),
		new(Name: "Uranus",  SemiMajorAxis: 19.19126393),
		new(Name: "Neptune", SemiMajorAxis: 30.06896348),
	];

	/// <summary>Calculates the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the minor planet in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity of the minor planet (dimensionless, 0 &lt;= e &lt; 1).</param>
	/// <param name="inclinationDeg">The orbital inclination of the minor planet to the ecliptic in degrees.</param>
	/// <returns>An array of <see cref="TisserandResult"/> records, one per planet, ordered from Mercury to Neptune.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="eccentricity"/> is outside the valid range 0 (inclusive) to 1 (exclusive).</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="semiMajorAxis"/> is not positive.</exception>
	/// <remarks>The Tisserand parameter is a quasi-conserved quantity derived from the Jacobi constant in the circular restricted three-body problem. It is defined as: <para><c>T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))</c></para> where <c>a_P</c> is the semi-major axis of the reference planet, <c>a</c> is the semi-major axis of the minor planet, <c>e</c> is the eccentricity of the minor planet, and <c>i</c> is the orbital inclination of the minor planet. By convention <c>T_J</c> (relative to Jupiter) is the most commonly used form and is widely employed to classify small solar-system bodies.</remarks>
	public static TisserandResult[] CalculateTisserandParameters(double semiMajorAxis, double eccentricity, double inclinationDeg)
	{
		// Guard against invalid math domains (hyperbolic/parabolic orbits)
		if (eccentricity is < 0.0 or >= 1.0)
		{
			// Throw an exception for invalid eccentricity values to prevent NaN results in the square root calculation
			throw new ArgumentOutOfRangeException(paramName: nameof(eccentricity), message: "Eccentricity must be between 0 (inclusive) and 1 (exclusive) for valid Tisserand parameters.");
		}
		if (semiMajorAxis <= 0.0)
		{
			// Throw an exception for non-positive semi-major axis values to prevent division by zero and invalid Tisserand parameter calculations
			throw new ArgumentOutOfRangeException(paramName: nameof(semiMajorAxis), message: "Semi-major axis must be positive for valid Tisserand parameters.");
		}
		// Pre-allocate the array based on known length to prevent dynamic resizing
		TisserandResult[] results = new TisserandResult[Planets.Length];
		// Guard against degenerate orbits
		if (semiMajorAxis <= 0.0)
		{
			// If the semi-major axis is zero or negative, return NaN for all planets to indicate an invalid orbit
			for (int i = 0; i < Planets.Length; i++)
			{
				// Use double.NaN to indicate that the Tisserand parameter cannot be computed for a degenerate orbit
				results[i] = new TisserandResult(PlanetName: Planets[i].Name, TisserandValue: double.NaN);
			}
			return results;
		}
		// Perform calculations once outside the loop
		double inclinationRad = double.DegreesToRadians(degrees: inclinationDeg);
		double cosInclination = Math.Cos(d: inclinationRad);
		double oneMinusESq = 1.0 - (eccentricity * eccentricity);
		// Use a standard for-loop (slightly faster than foreach for arrays in performance-critical code)
		for (int i = 0; i < Planets.Length; i++)
		{
			// Retrieve the planet data for the current iteration
			PlanetData planet = Planets[i];
			// Calculate the Tisserand parameter using the formula: T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))
			double tisserand = (planet.SemiMajorAxis / semiMajorAxis) + (2.0 * cosInclination * Math.Sqrt(d: semiMajorAxis / planet.SemiMajorAxis * oneMinusESq));
			// Store the result in the pre-allocated array
			results[i] = new TisserandResult(PlanetName: planet.Name, TisserandValue: tisserand);
		}
		// Return the array of Tisserand results for all planets
		return results;
	}
}