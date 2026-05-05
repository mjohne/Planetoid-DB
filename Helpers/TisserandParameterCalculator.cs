// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>The Tisserand parameter is a quasi-conserved quantity derived from the Jacobi constant in the circular restricted three-body problem. It is defined as: <para><c>T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))</c></para> where <c>a_P</c> is the semi-major axis of the reference planet, <c>a</c> is the semi-major axis of the minor planet, <c>e</c> is the eccentricity of the minor planet, and <c>i</c> is the orbital inclination of the minor planet. By convention <c>T_J</c> (relative to Jupiter) is the most commonly used form and is widely employed to classify small solar-system bodies.</remarks>
internal class TisserandParameterCalculator
{
	/// <summary>Represents the name and semi-major axis of a solar system planet used in Tisserand parameter calculations.</summary>
	/// <param name="Name">The common name of the planet.</param>
	/// <param name="SemiMajorAxis">The semi-major axis in AU (J2000.0 mean elements).</param>
	/// <remarks>Only the semi-major axis is required for the Tisserand parameter formula.</remarks>
	public record PlanetData(string Name, double SemiMajorAxis);

	/// <summary>Represents the Tisserand parameter result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the reference planet.</param>
	/// <param name="TisserandValue">The computed Tisserand parameter value (dimensionless).</param>
	/// <remarks>Values near 3 (relative to Jupiter) indicate a Jupiter-family comet or Jupiter-crossing orbit. Values greater than 3 typically indicate an asteroid, while values less than 2 suggest a nearly isotropic comet.</remarks>
	public record TisserandResult(string PlanetName, double TisserandValue);

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
	/// <param name="eccentricity">The orbital eccentricity of the minor planet (dimensionless, 0 ≤ e &lt; 1).</param>
	/// <param name="inclinationDeg">The orbital inclination of the minor planet to the ecliptic in degrees.</param>
	/// <returns>A list of <see cref="TisserandResult"/> records, one per planet, ordered from Mercury to Neptune.</returns>
	/// <remarks>Uses the standard three-body Tisserand formula: <para><c>T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))</c></para> The result is undefined (and returned as <see cref="double.NaN"/>) when <paramref name="semiMajorAxis"/> is zero or negative.</remarks>
	public static List<TisserandResult> CalculateTisserandParameters(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg)
	{
		List<TisserandResult> results = [];
		// Guard against degenerate orbits
		if (semiMajorAxis <= 0.0)
		{
			foreach (PlanetData planet in Planets)
			{
				results.Add(item: new TisserandResult(PlanetName: planet.Name, TisserandValue: double.NaN));
			}
			return results;
		}
		// Convert inclination to radians once
		double inclinationRad = inclinationDeg * Math.PI / 180.0;
		double cosInclination = Math.Cos(d: inclinationRad);
		double oneMinusESq = 1.0 - eccentricity * eccentricity;
		foreach (PlanetData planet in Planets)
		{
			// T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))
			double tisserand = planet.SemiMajorAxis / semiMajorAxis
				+ 2.0 * cosInclination * Math.Sqrt(d: semiMajorAxis / planet.SemiMajorAxis * oneMinusESq);
			results.Add(item: new TisserandResult(PlanetName: planet.Name, TisserandValue: tisserand));
		}
		return results;
	}
}
