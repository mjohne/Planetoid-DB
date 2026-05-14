// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating various orbital elements.</summary>
/// <remarks>This class contains methods for calculating the semi-minor axis, linear eccentricity, major axis, minor axis, and other orbital elements.</remarks>
internal class DerivedElements
{
	/// <summary>Calculates the semi-minor axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-minor axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the semi-minor axis of an ellipse.</remarks>
	public static double CalculateSemiMinorAxis(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * Math.Sqrt(d: 1 - (numericalEccentricity * numericalEccentricity));

	/// <summary>Calculates the linear eccentricity of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The linear eccentricity of the ellipse.</returns>
	/// <remarks>This method is used to calculate the linear eccentricity of an ellipse.</remarks>
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

	/// <summary>Calculates the major axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The major axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the major axis of an ellipse.</remarks>
	public static double CalculateMajorAxis(double semiMajorAxis) => 2 * semiMajorAxis;

	/// <summary>Calculates the minor axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The minor axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the minor axis of an ellipse.</remarks>
	public static double CalculateMinorAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>Calculates the eccentric anomaly of an orbit.</summary>
	/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
	/// <returns>The eccentric anomaly of the orbit.</returns>
	/// <remarks>This method is used to calculate the eccentric anomaly of an orbit.</remarks>
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

	/// <summary>Calculates the true anomaly of an orbit.</summary>
	/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="numberDecimalPlaces">The number of decimal places for the result.</param>
	/// <returns>The true anomaly of the orbit.</returns>
	/// <remarks>This method is used to calculate the true anomaly of an orbit.</remarks>
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

	/// <summary>Calculates the perihelion distance of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The perihelion distance of the orbit.</returns>
	/// <remarks>This method is used to calculate the perihelion distance of an orbit.</remarks>
	public static double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 - numericalEccentricity) * semiMajorAxis;

	/// <summary>Calculates the aphelion distance of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The aphelion distance of the orbit.</returns>
	/// <remarks>This method is used to calculate the aphelion distance of an orbit.</remarks>
	public static double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 + numericalEccentricity) * semiMajorAxis;

	/// <summary>Calculates the longitude of the descending node of an orbit.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node of the orbit.</param>
	/// <returns>The longitude of the descending node of the orbit.</returns>
	/// <remarks>This method is used to calculate the longitude of the descending node of an orbit.</remarks>
	public static double CalculateLongitudeDescendingNode(double longitudeAscendingNode) =>
		longitudeAscendingNode switch
		{
			>= 0 and < 180 => longitudeAscendingNode + 180,
			>= 180 and < 360 => longitudeAscendingNode - 180,
			_ => -1
		};

	/// <summary>Calculates the argument of aphelion of an orbit.</summary>
	/// <param name="argumentAphelion">The argument of perihelion of the orbit.</param>
	/// <returns>The argument of aphelion of the orbit.</returns>
	/// <remarks>This method is used to calculate the argument of aphelion of an orbit.</remarks>
	public static double CalculateArgumentOfAphelion(double argumentAphelion) =>
		argumentAphelion switch
		{
			>= 0 and < 180 => argumentAphelion + 180,
			>= 180 and < 360 => argumentAphelion - 180,
			_ => -1
		};

	/// <summary>Calculates the focal parameter of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The focal parameter of the orbit.</returns>
	/// <remarks>This method is used to calculate the focal parameter of an orbit.</remarks>
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

	/// <summary>Calculates the semi-latus rectum of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-latus rectum of the ellipse.</returns>
	/// <remarks>This method is used to calculate the semi-latus rectum of an ellipse.</remarks>
	public static double CalculateSemiLatusRectum(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 - (numericalEccentricity * numericalEccentricity));

	/// <summary>Calculates the latus rectum of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The latus rectum of the ellipse.</returns>
	/// <remarks>This method is used to calculate the latus rectum of an ellipse.</remarks>
	public static double CalculateLatusRectum(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>Calculates the orbital period of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The orbital period of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital period of an ellipse.</remarks>
	public static double CalculatePeriod(double semiMajorAxis) => Math.Sqrt(d: semiMajorAxis * semiMajorAxis * semiMajorAxis);

	/// <summary>Calculates the orbital area of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital area of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital area of an ellipse.</remarks>
	public static double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity)
	{
		double semiMinorAxis = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		double term1 = semiMajorAxis + semiMinorAxis;
		double diff = semiMajorAxis - semiMinorAxis;
		double term2 = 3 * (diff * diff) / (10 * term1);
		double term3 = Math.Sqrt(d: (semiMajorAxis * semiMajorAxis) + (14 * semiMajorAxis * semiMinorAxis) + (semiMinorAxis * semiMinorAxis));
		return term1 + term2 + term3;
	}

	/// <summary>Calculates the orbital perimeter of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital perimeter of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital perimeter of an ellipse.</remarks>
	public static double CalculateOrbitalPerimeter(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) * Math.PI;

	/// <summary>Calculates the semi-mean axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-mean axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the semi-mean axis of an ellipse.</remarks>
	public static double CalculateSemiMeanAxis(double semiMajorAxis, double numericalEccentricity) => (semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity)) / 2;

	/// <summary>Calculates the mean axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The mean axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the mean axis of an ellipse.</remarks>
	public static double CalculateMeanAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>Calculates the standard gravitational parameter of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The standard gravitational parameter of the ellipse.</returns>
	/// <remarks>This method is used to calculate the standard gravitational parameter of an ellipse.</remarks>
	public static double CalculateStandardGravitationalParameter(double semiMajorAxis) => 4 * (Math.PI * Math.PI) * (semiMajorAxis * semiMajorAxis * semiMajorAxis) / CalculatePeriod(semiMajorAxis: semiMajorAxis);

	/// <summary>Represents an orbital resonance between a planetoid and a solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="PlanetPeriod">The orbital period of the planet in years.</param>
	/// <param name="PlanetoidPeriod">The orbital period of the planetoid in years.</param>
	/// <param name="Ratio">The actual ratio of the planet's period to the planetoid's period.</param>
	/// <param name="ResonanceP">The p value in the integer resonance ratio p:q.</param>
	/// <param name="ResonanceQ">The q value in the integer resonance ratio p:q.</param>
	/// <param name="DeviationPercent">The percentage deviation of the actual ratio from the integer ratio.</param>
	/// <remarks>This record is used to represent an orbital resonance between a planetoid and a solar system planet.</remarks>
	public record OrbitalResonance(string PlanetName, double PlanetPeriod, double PlanetoidPeriod, double Ratio, int ResonanceP, int ResonanceQ, double DeviationPercent);

	/// <summary>Calculates the orbital resonances of a planetoid with the 8 solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the planetoid in AU.</param>
	/// <returns>A list of computed orbital resonances.</returns>
	/// <remarks>This method calculates the resonance with each major planet by finding the closest small-integer ratio.</remarks>
	public static List<OrbitalResonance> CalculateOrbitalResonances(double semiMajorAxis)
	{
		// Calculate the orbital period of the planetoid using Kepler's third law.
		double planetoidPeriod = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		// List to hold the results of the resonance calculations.
		List<OrbitalResonance> results = [];
		// Define the orbital periods of the 8 solar system planets in years.
		(string Name, double Period)[] planets =
		[
			(Name: "Mercury", Period: 0.240846),
			(Name: "Venus", Period: 0.615197),
			(Name: "Earth", Period: 1.000000),
			(Name: "Mars", Period: 1.880848),
			(Name: "Jupiter", Period: 11.862615),
			(Name: "Saturn", Period: 29.447498),
			(Name: "Uranus", Period: 84.016846),
			(Name: "Neptune", Period: 164.791320)
		];
		// Loop through each planet and calculate the resonance ratio and deviation.
		for (int i = 0; i < planets.Length; i++)
		{
			// Calculate the actual ratio of the planet's period to the planetoid's period.
			double ratio = planets[i].Period / planetoidPeriod;
			// Find the best integer ratio p:q that approximates the actual ratio.
			int bestP = 1;
			int bestQ = 1;
			// Initialize the smallest deviation to a large number.
			double smallestDeviation = double.MaxValue;
			// Loop through possible integer values for p and q to find the best resonance ratio.
			for (int p = 1; p <= 15; p++)
			{
				// Loop through possible integer values for q.
				for (int q = 1; q <= 15; q++)
				{
					// Calculate the test ratio for the current p and q.
					double testRatio = (double)p / q;
					// Calculate the percentage deviation of the test ratio from the actual ratio.
					double deviation = Math.Abs(value: testRatio - ratio) / ratio * 100.0;
					// If this deviation is smaller than the smallest deviation found so far, update the best p, q, and smallest deviation.
					if (deviation < smallestDeviation)
					{
						smallestDeviation = deviation;
						bestP = p;
						bestQ = q;
					}
				}
			}
			// Add the calculated resonance information to the results list.
			results.Add(item: new OrbitalResonance(
				PlanetName: planets[i].Name,
				PlanetPeriod: planets[i].Period,
				PlanetoidPeriod: planetoidPeriod,
				Ratio: ratio,
				ResonanceP: bestP,
				ResonanceQ: bestQ,
				DeviationPercent: smallestDeviation));
		}
		// Return the list of calculated orbital resonances.
		return results;
	}

	/// <summary>Calculates the directrix distance of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The directrix distance in AU.</returns>
	/// <remarks>The directrix is a line perpendicular to the major axis. For an ellipse, directrix = a/e.</remarks>
	public static double CalculateDirectrix(double semiMajorAxis, double numericalEccentricity)
	{
		if (numericalEccentricity == 0)
		{
			return double.PositiveInfinity;
		}
		return semiMajorAxis / numericalEccentricity;
	}

	/// <summary>Calculates the orbital velocity at perihelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The perihelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_p = sqrt(GM(1+e)/a(1-e)).</remarks>
	public static double CalculatePerihelionVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		return Math.Sqrt(d: standardGravitationalParameter * (1.0 + numericalEccentricity) / (semiMajorAxis * (1.0 - numericalEccentricity)));
	}

	/// <summary>Calculates the orbital velocity at aphelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The aphelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_a = sqrt(GM(1-e)/a(1+e)).</remarks>
	public static double CalculateAphelionVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		return Math.Sqrt(d: standardGravitationalParameter * (1.0 - numericalEccentricity) / (semiMajorAxis * (1.0 + numericalEccentricity)));
	}

	/// <summary>Calculates the mean orbital velocity.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean orbital velocity in AU/year.</returns>
	/// <remarks>Calculated as v_mean = 2πa/T.</remarks>
	public static double CalculateMeanOrbitalVelocity(double semiMajorAxis)
	{
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		return (2.0 * Math.PI * semiMajorAxis) / period;
	}

	/// <summary>Calculates the current orbital velocity at a given true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The current orbital velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v = sqrt(GM(2/r - 1/a)).</remarks>
	public static double CalculateCurrentOrbitalVelocity(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		double trueAnomalyRadians = trueAnomaly * Math.PI / 180.0;
		double currentRadius = semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRadians)));
		return Math.Sqrt(d: standardGravitationalParameter * ((2.0 / currentRadius) - (1.0 / semiMajorAxis)));
	}

	/// <summary>Calculates the radial velocity component.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The radial velocity component in AU/year.</returns>
	/// <remarks>The radial component is perpendicular to the orbit.</remarks>
	public static double CalculateRadialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		double trueAnomalyRadians = trueAnomaly * Math.PI / 180.0;
		double semiLatusRectum = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: standardGravitationalParameter / semiLatusRectum) * numericalEccentricity * Math.Sin(a: trueAnomalyRadians);
	}

	/// <summary>Calculates the tangential velocity component.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The tangential velocity component in AU/year.</returns>
	/// <remarks>The tangential component is along the orbit direction.</remarks>
	public static double CalculateTangentialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		double trueAnomalyRadians = trueAnomaly * Math.PI / 180.0;
		double semiLatusRectum = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: standardGravitationalParameter / semiLatusRectum) * (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRadians)));
	}

	/// <summary>Calculates the specific orbital energy.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The specific orbital energy in AU²/year².</returns>
	/// <remarks>Calculated as ε = -GM/(2a).</remarks>
	public static double CalculateSpecificOrbitalEnergy(double semiMajorAxis)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		return -standardGravitationalParameter / (2.0 * semiMajorAxis);
	}

	/// <summary>Calculates the specific angular momentum.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The specific angular momentum in AU²/year.</returns>
	/// <remarks>Calculated as h = sqrt(GMa(1-e²)).</remarks>
	public static double CalculateSpecificAngularMomentum(double semiMajorAxis, double numericalEccentricity)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		return Math.Sqrt(d: standardGravitationalParameter * semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)));
	}

	/// <summary>Calculates the vis-viva energy at a given position.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The vis-viva energy in AU²/year².</returns>
	/// <remarks>Calculated as E = v²/2 - GM/r.</remarks>
	public static double CalculateVisVivaEnergy(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		const double standardGravitationalParameter = 4.0 * Math.PI * Math.PI;
		double velocity = CalculateCurrentOrbitalVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly);
		double trueAnomalyRadians = trueAnomaly * Math.PI / 180.0;
		double currentRadius = semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRadians)));
		return (velocity * velocity / 2.0) - (standardGravitationalParameter / currentRadius);
	}

	/// <summary>Calculates the longitude of perihelion.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>The longitude of perihelion in degrees.</returns>
	/// <remarks>Calculated as ϖ = Ω + ω.</remarks>
	public static double CalculateLongitudeOfPerihelion(double longitudeAscendingNode, double argumentPerihelion)
	{
		double result = longitudeAscendingNode + argumentPerihelion;
		return result >= 360.0 ? result - 360.0 : result;
	}

	/// <summary>Calculates the mean longitude.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="meanAnomaly">The mean anomaly in degrees.</param>
	/// <returns>The mean longitude in degrees.</returns>
	/// <remarks>Calculated as λ = M + ϖ = M + Ω + ω.</remarks>
	public static double CalculateMeanLongitude(double longitudeAscendingNode, double argumentPerihelion, double meanAnomaly)
	{
		double longitudePerihelion = CalculateLongitudeOfPerihelion(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion);
		double result = meanAnomaly + longitudePerihelion;
		return result >= 360.0 ? result - 360.0 : result;
	}

	/// <summary>Calculates the argument of latitude.</summary>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The argument of latitude in degrees.</returns>
	/// <remarks>Calculated as u = ω + ν.</remarks>
	public static double CalculateArgumentOfLatitude(double argumentPerihelion, double trueAnomaly)
	{
		double result = argumentPerihelion + trueAnomaly;
		return result >= 360.0 ? result - 360.0 : result;
	}

	/// <summary>Calculates the flight path angle.</summary>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The flight path angle in degrees.</returns>
	/// <remarks>Calculated as φ = arctan(e·sin(ν)/(1+e·cos(ν))).</remarks>
	public static double CalculateFlightPathAngle(double numericalEccentricity, double trueAnomaly)
	{
		double trueAnomalyRadians = trueAnomaly * Math.PI / 180.0;
		double angle = Math.Atan(d: (numericalEccentricity * Math.Sin(a: trueAnomalyRadians)) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRadians))));
		return angle * 180.0 / Math.PI;
	}

	/// <summary>Calculates the time since perihelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time since perihelion in years.</returns>
	/// <remarks>Uses Kepler's equation and the mean motion.</remarks>
	public static double CalculateTimeSincePerihelion(double meanAnomaly, double semiMajorAxis)
	{
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		double meanAnomalyNormalized = meanAnomaly / 360.0;
		return meanAnomalyNormalized * period;
	}

	/// <summary>Calculates the time to next perihelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time to next perihelion in years.</returns>
	/// <remarks>Subtracts time since perihelion from the orbital period.</remarks>
	public static double CalculateTimeToNextPerihelion(double meanAnomaly, double semiMajorAxis)
	{
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		double timeSincePerihelion = CalculateTimeSincePerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);
		return period - timeSincePerihelion;
	}

	/// <summary>Calculates the time since aphelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time since aphelion in years.</returns>
	/// <remarks>Aphelion occurs at mean anomaly 180°.</remarks>
	public static double CalculateTimeSinceAphelion(double meanAnomaly, double semiMajorAxis)
	{
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		double anomalyFromAphelion = meanAnomaly - 180.0;
		if (anomalyFromAphelion < 0)
		{
			anomalyFromAphelion += 360.0;
		}
		return (anomalyFromAphelion / 360.0) * period;
	}

	/// <summary>Calculates the time to next aphelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time to next aphelion in years.</returns>
	/// <remarks>Subtracts time since aphelion from the orbital period.</remarks>
	public static double CalculateTimeToNextAphelion(double meanAnomaly, double semiMajorAxis)
	{
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		double timeSinceAphelion = CalculateTimeSinceAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);
		return period - timeSinceAphelion;
	}

	/// <summary>Calculates the synodic period with Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The synodic period in years.</returns>
	/// <remarks>Calculated as T_syn = 1/(|1/T₁ - 1/T₂|) where T₂ is Earth's period (1 year).</remarks>
	public static double CalculateSynodicPeriod(double semiMajorAxis)
	{
		const double earthPeriod = 1.0;
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		return 1.0 / Math.Abs(value: (1.0 / period) - (1.0 / earthPeriod));
	}

	/// <summary>Calculates the Tisserand parameter with respect to Jupiter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter (dimensionless).</returns>
	/// <remarks>T_J = a_J/a + 2·cos(i)·sqrt(a(1-e²)/a_J) where a_J = 5.2 AU.</remarks>
	public static double CalculateTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double jupiterSemiMajorAxis = 5.2;
		double inclinationRadians = inclination * Math.PI / 180.0;
		double term1 = jupiterSemiMajorAxis / semiMajorAxis;
		double term2 = 2.0 * Math.Cos(d: inclinationRadians) * Math.Sqrt(d: (semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity))) / jupiterSemiMajorAxis);
		return term1 + term2;
	}

	/// <summary>Calculates the mean distance from the focus (Sun).</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean distance from focus in AU.</returns>
	/// <remarks>Calculated as r_mean = a(1 + e²/2).</remarks>
	public static double CalculateMeanDistanceFromFocus(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1.0 + ((numericalEccentricity * numericalEccentricity) / 2.0));

	/// <summary>Calculates the geometric albedo-adjusted diameter.</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H.</param>
	/// <param name="geometricAlbedo">The geometric albedo (0.0 to 1.0).</param>
	/// <returns>The diameter in kilometers.</returns>
	/// <remarks>Calculated using D = 1329 / sqrt(albedo) * 10^(-0.2*H).</remarks>
	public static double CalculateGeometricAlbedoAdjustedDiameter(double absoluteMagnitude, double geometricAlbedo)
	{
		if (geometricAlbedo <= 0)
		{
			return 0;
		}
		return 1329.0 / Math.Sqrt(d: geometricAlbedo) * Math.Pow(x: 10.0, y: -0.2 * absoluteMagnitude);
	}
}