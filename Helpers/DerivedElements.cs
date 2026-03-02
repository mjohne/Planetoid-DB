// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>
/// Provides methods for calculating various orbital elements.
/// </summary>
/// <remarks>
/// This class contains methods for calculating the semi-minor axis, linear eccentricity, major axis, minor axis, and other orbital elements.
/// </remarks>
internal class DerivedElements
{
	/// <summary>
	/// Calculates the semi-minor axis of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-minor axis of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the semi-minor axis of an ellipse.
	/// </remarks>
	public static double CalculateSemiMinorAxis(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * Math.Sqrt(d: 1 - (numericalEccentricity * numericalEccentricity));

	/// <summary>
	/// Calculates the linear eccentricity of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The linear eccentricity of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the linear eccentricity of an ellipse.
	/// </remarks>
	public static double CalculateLinearEccentricity(double semiMajorAxis, double numericalEccentricity)
	{
		double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return numericalEccentricity switch
		{
			0 => 0,
			< 1 and > 0 => Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) - (semiMinorAxis * semiMinorAxis)),
			> 1 => Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) + (semiMinorAxis * semiMinorAxis)),
			_ => 0
		};
	}

	/// <summary>
	/// Calculates the major axis of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The major axis of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the major axis of an ellipse.
	/// </remarks>
	public static double CalculateMajorAxis(double semiMajorAxis) => 2 * semiMajorAxis;

	/// <summary>
	/// Calculates the minor axis of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The minor axis of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the minor axis of an ellipse.
	/// </remarks>
	public static double CalculateMinorAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>
	/// Calculates the eccentric anomaly of an orbit.
	/// </summary>
	/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
	/// <returns>The eccentric anomaly of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the eccentric anomaly of an orbit.
	/// </remarks>
	public static double CalculateEccentricAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
	{
		const double k = Math.PI / 180.0;
		const int maxIteration = 30;
		int i = 0;
		double delta = Math.Pow(x: 10, y: -numberDecimalPlaces);
		meanAnomaly /= 360.0;
		meanAnomaly = 2.0 * Math.PI * (meanAnomaly - Math.Floor(d: meanAnomaly));
		double e = numericalEccentricity < 0.8 ? meanAnomaly : Math.PI;
		double f = e - (numericalEccentricity * Math.Sin(a: meanAnomaly)) - meanAnomaly;
		while ((Math.Abs(value: f) > delta) && (i < maxIteration))
		{
			e -= f / (1.0 - (numericalEccentricity * Math.Cos(d: e)));
			f = e - (numericalEccentricity * Math.Sin(a: e)) - meanAnomaly;
			i += 1;
		}
		e /= k;
		return Math.Round(a: e * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
	}

	/// <summary>
	/// Calculates the true anomaly of an orbit.
	/// </summary>
	/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
	/// <returns>The true anomaly of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the true anomaly of an orbit.
	/// </remarks>
	public static double CalculateTrueAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
	{
		const double k = Math.PI / 180.0;
		double e = CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: numberDecimalPlaces);
		double s = Math.Sin(a: e);
		double c = Math.Cos(d: e);
		double fak = Math.Sqrt(d: 1.0 - (numericalEccentricity * numericalEccentricity));
		double phi = Math.Atan2(y: fak * s, x: c - numericalEccentricity) / k;
		return Math.Round(a: phi * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
	}

	/// <summary>
	/// Calculates the perihelion distance of an orbit.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The perihelion distance of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the perihelion distance of an orbit.
	/// </remarks>
	public static double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 - numericalEccentricity) * semiMajorAxis;

	/// <summary>
	/// Calculates the aphelion distance of an orbit.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The aphelion distance of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the aphelion distance of an orbit.
	/// </remarks>
	public static double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 + numericalEccentricity) * semiMajorAxis;

	/// <summary>
	/// Calculates the longitude of the descending node of an orbit.
	/// </summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node of the orbit.</param>
	/// <returns>The longitude of the descending node of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the longitude of the descending node of an orbit.
	/// </remarks>
	public static double CalculateLongitudeDescendingNode(double longitudeAscendingNode) =>
		longitudeAscendingNode switch
		{
			>= 0 and < 180 => longitudeAscendingNode + 180,
			>= 180 and < 360 => longitudeAscendingNode - 180,
			_ => -1
		};

	/// <summary>
	/// Calculates the argument of aphelion of an orbit.
	/// </summary>
	/// <param name="argumentAphelion">The argument of perihelion of the orbit.</param>
	/// <returns>The argument of aphelion of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the argument of aphelion of an orbit.
	/// </remarks>
	public static double CalculateArgumentOfAphelion(double argumentAphelion) =>
		argumentAphelion switch
		{
			>= 0 and < 180 => argumentAphelion + 180,
			>= 180 and < 360 => argumentAphelion - 180,
			_ => -1
		};

	/// <summary>
	/// Calculates the focal parameter of an orbit.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The focal parameter of the orbit.</returns>
	/// <remarks>
	/// This method is used to calculate the focal parameter of an orbit.
	/// </remarks>
	public static double CalculateFocalParameter(double semiMajorAxis, double numericalEccentricity)
	{
		double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return numericalEccentricity switch
		{
			> 1 => semiMinorAxis * semiMinorAxis / Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) + (semiMinorAxis * semiMinorAxis)),
			> 0 and < 1 => semiMinorAxis * semiMinorAxis / Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) - (semiMinorAxis * semiMinorAxis)),
			_ => 2 * semiMajorAxis
		};
	}

	/// <summary>
	/// Calculates the semi-latus rectum of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-latus rectum of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the semi-latus rectum of an ellipse.
	/// </remarks>
	public static double CalculateSemiLatusRectum(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 - (numericalEccentricity * numericalEccentricity));

	/// <summary>
	/// Calculates the latus rectum of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The latus rectum of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the latus rectum of an ellipse.
	/// </remarks>
	public static double CalculateLatusRectum(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>
	/// Calculates the orbital period of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The orbital period of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the orbital period of an ellipse.
	/// </remarks>
	public static double CalculatePeriod(double semiMajorAxis) => Math.Sqrt(d: semiMajorAxis * semiMajorAxis * semiMajorAxis);

	/// <summary>
	/// Calculates the orbital area of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital area of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the orbital area of an ellipse.
	/// </remarks>
	public static double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity)
	{
		double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		double term1 = semiMajorAxis + semiMinorAxis;
		double diff = semiMajorAxis - semiMinorAxis;
		double term2 = 3 * (diff * diff) / (10 * term1);
		double term3 = Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) + (14 * semiMajorAxis * semiMinorAxis) + (semiMinorAxis * semiMinorAxis));
		return term1 + term2 + term3;
	}

	/// <summary>
	/// Calculates the orbital perimeter of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital perimeter of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the orbital perimeter of an ellipse.
	/// </remarks>
	public static double CalculateOrbitalPerimeter(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) * Math.PI;

	/// <summary>
	/// Calculates the semi-mean axis of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-mean axis of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the semi-mean axis of an ellipse.
	/// </remarks>
	public static double CalculateSemiMeanAxis(double semiMajorAxis, double numericalEccentricity) => (semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity)) / 2;

	/// <summary>
	/// Calculates the mean axis of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The mean axis of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the mean axis of an ellipse.
	/// </remarks>
	public static double CalculateMeanAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>
	/// Calculates the standard gravitational parameter of an ellipse.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The standard gravitational parameter of the ellipse.</returns>
	/// <remarks>
	/// This method is used to calculate the standard gravitational parameter of an ellipse.
	/// </remarks>
	public static double CalculateStandardGravitationalParameter(double semiMajorAxis) => 4 * (Math.PI * Math.PI) * (semiMajorAxis * semiMajorAxis * semiMajorAxis) / CalculatePeriod(semiMajorAxis: semiMajorAxis);

	/// <summary>
	/// Known orbital periods of the eight solar system planets in Julian years.
	/// </summary>
	/// <remarks>
	/// Values are derived from the mean orbital elements of the planets referenced to the J2000.0 epoch.
	/// </remarks>
	public static readonly (string Name, double Period)[] PlanetOrbitalPeriods =
	[
		("Mercury",   0.2408467),
		("Venus",     0.6151973),
		("Earth",     1.0000174),
		("Mars",      1.8808476),
		("Jupiter",  11.862615),
		("Saturn",   29.447498),
		("Uranus",   84.016846),
		("Neptune", 164.79132),
	];

	/// <summary>
	/// Represents a single orbital resonance result between a planetoid and a planet.
	/// </summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="PlanetPeriod">The orbital period of the planet in years.</param>
	/// <param name="PlanetoidPeriod">The orbital period of the planetoid in years.</param>
	/// <param name="Ratio">The ratio of the planetoid period to the planet period.</param>
	/// <param name="ResonanceP">The numerator of the nearest resonance fraction.</param>
	/// <param name="ResonanceQ">The denominator of the nearest resonance fraction.</param>
	/// <param name="DeviationPercent">The deviation from exact resonance in percent.</param>
	/// <remarks>
	/// An orbital resonance occurs when the ratio of two orbital periods is close to a ratio of small integers.
	/// </remarks>
	public readonly record struct OrbitalResonance(
		string PlanetName,
		double PlanetPeriod,
		double PlanetoidPeriod,
		double Ratio,
		int ResonanceP,
		int ResonanceQ,
		double DeviationPercent
	);

	/// <summary>
	/// Calculates the orbital resonances of a planetoid relative to all eight solar system planets.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis of the planetoid in AU.</param>
	/// <param name="maxNumerator">The maximum numerator value to consider when searching for resonance fractions. Default is 10.</param>
	/// <param name="maxDenominator">The maximum denominator value to consider when searching for resonance fractions. Default is 10.</param>
	/// <returns>A list of <see cref="OrbitalResonance"/> results, one per planet.</returns>
	/// <remarks>
	/// Uses Kepler's third law (T = sqrt(a³)) to derive the planetoid period in years,
	/// then searches for the nearest simple integer ratio p:q (p ≤ <paramref name="maxNumerator"/>,
	/// q ≤ <paramref name="maxDenominator"/>) for each planet.
	/// </remarks>
	public static List<OrbitalResonance> CalculateOrbitalResonances(double semiMajorAxis, int maxNumerator = 10, int maxDenominator = 10)
	{
		double planetoidPeriod = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		List<OrbitalResonance> results = new(capacity: PlanetOrbitalPeriods.Length);
		foreach ((string name, double planetPeriod) in PlanetOrbitalPeriods)
		{
			double ratio = planetoidPeriod / planetPeriod;
			int bestP = 1, bestQ = 1;
			double bestDeviation = double.MaxValue;
			for (int p = 1; p <= maxNumerator; p++)
			{
				for (int q = 1; q <= maxDenominator; q++)
				{
					double fraction = (double)p / q;
					double deviation = Math.Abs(value: ratio - fraction);
					if (deviation < bestDeviation)
					{
						bestDeviation = deviation;
						bestP = p;
						bestQ = q;
					}
				}
			}
			double exactFraction = (double)bestP / bestQ;
			double deviationPercent = exactFraction > 0 ? (bestDeviation / exactFraction) * 100.0 : 0.0;
			results.Add(item: new OrbitalResonance(
				PlanetName: name,
				PlanetPeriod: planetPeriod,
				PlanetoidPeriod: planetoidPeriod,
				Ratio: ratio,
				ResonanceP: bestP,
				ResonanceQ: bestQ,
				DeviationPercent: deviationPercent
			));
		}
		return results;
	}
}