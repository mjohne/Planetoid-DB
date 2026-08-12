/*
 * File:        DerivedElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides methods for calculating various orbital elements.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Provides methods for calculating various orbital elements.</summary>
/// <remarks>This class contains methods for calculating the semi-minor axis, linear eccentricity, major axis, minor axis, and other orbital elements.</remarks>
internal class DerivedElements
{
	/// <summary>Represents the standard gravitational parameter (GM) for the Sun in AU^3/year^2.</summary>
	/// <remarks>The standard gravitational parameter is used in various orbital calculations.</remarks>
	private const double gm = 4.0 * Math.PI * Math.PI;

	/// <summary>Represents the orbital periods of the 8 solar system planets in years.</summary>
	/// <remarks>This array contains the names and orbital periods of the 8 planets in the solar system.</remarks>
	private static readonly (string Name, double Period)[] SolarSystemPlanets =
	[
		("Mercury", 0.240846),
		("Venus", 0.615197),
		("Earth", 1.000000),
		("Mars", 1.880848),
		("Jupiter", 11.862615),
		("Saturn", 29.447498),
		("Uranus", 84.016846),
		("Neptune", 164.791320)
	];

	/// <summary>Normalizes an angle in degrees to the range [0, 360).</summary>
	/// <param name="degrees">The angle in degrees to normalize.</param>
	/// <returns>The normalized angle in the range [0, 360).</returns>
	private static double NormalizeAngle360(double degrees)
	{
		// Normalize the angle to the range [0, 360) degrees.
		double angle = degrees % 360.0;
		// If the angle is negative, add 360 to bring it into the positive range.
		return angle < 0 ? angle + 360.0 : angle;
	}

	/// <summary>Calculates the semi-minor axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-minor axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the semi-minor axis of an ellipse.</remarks>
	public static double CalculateSemiMinorAxis(double semiMajorAxis, double numericalEccentricity)
	{
		// The semi-minor axis (b) of an ellipse can be calculated using the formula:
		// b = a * sqrt(1 - e^2), where a is the semi-major axis and e is the numerical eccentricity.
		double factor = 1.0 - (numericalEccentricity * numericalEccentricity);
		// For hyperbolic orbits (e > 1), the semi-minor axis is imaginary, so we take the absolute value.
		// For parabolic orbits (e = 1), the semi-minor axis is zero.
		// For elliptical orbits (0 <= e < 1), the semi-minor axis is real and positive.
		// For parabolic orbits, b = 0.
		// Therefore, we can use the absolute value of the factor to handle all cases.
		return semiMajorAxis * Math.Sqrt(d: Math.Abs(value: factor));
	}

	/// <summary>Calculates the linear eccentricity of an ellipse (c = a * e).</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The linear eccentricity of the ellipse.</returns>
	/// <remarks>This method is used to calculate the linear eccentricity of an ellipse.</remarks>
	public static double CalculateLinearEccentricity(double semiMajorAxis, double numericalEccentricity) => Math.Abs(value: semiMajorAxis * numericalEccentricity);

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
	/// <returns>The eccentric anomaly of the orbit.</returns>
	/// <remarks>This method is used to calculate the eccentric anomaly of an orbit.</remarks>
	public static double CalculateEccentricAnomaly(double meanAnomaly, double numericalEccentricity)
	{
		// Convert mean anomaly from degrees to radians and normalize it to [0, 360) degrees.
		double meanAnomalyRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: meanAnomaly));
		// Use Newton-Raphson iteration to solve Kepler's equation: M = E - e*sin(E)
		double e = numericalEccentricity;
		double eccentricAnomalyRad = e < 0.8 ? meanAnomalyRad : Math.PI;
		// Set a maximum number of iterations and a tolerance for convergence.
		const int maxIteration = 100;
		const double tolerance = 1e-12;
		// Iterate to find the eccentric anomaly.
		for (int i = 0; i < maxIteration; i++)
		{
			// Calculate the function value f(E) = E - e*sin(E) - M
			double f = eccentricAnomalyRad - (e * Math.Sin(a: eccentricAnomalyRad)) - meanAnomalyRad;
			// If the function value is within the tolerance, we have converged.
			if (Math.Abs(value: f) < tolerance)
			{
				break;
			}
			// Calculate the derivative f'(E) = 1 - e*cos(E)
			double fPrime = 1.0 - (e * Math.Cos(d: eccentricAnomalyRad));
			eccentricAnomalyRad -= f / fPrime;
		}
		// Convert the eccentric anomaly back to degrees and return it.
		return double.RadiansToDegrees(radians: eccentricAnomalyRad);
	}

	/// <summary>Calculates the true anomaly of an orbit.</summary>
	/// <param name="meanAnomaly">The mean anomaly of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The true anomaly of the orbit.</returns>
	/// <remarks>This method is used to calculate the true anomaly of an orbit.</remarks>
	public static double CalculateTrueAnomaly(double meanAnomaly, double numericalEccentricity)
	{
		// First, calculate the eccentric anomaly using the mean anomaly and numerical eccentricity.
		double eccentricAnomalyRad = double.DegreesToRadians(degrees: CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity));
		double e = numericalEccentricity;
		// Then, calculate the true anomaly using the eccentric anomaly and numerical eccentricity.
		double sinE = Math.Sin(a: eccentricAnomalyRad);
		double cosE = Math.Cos(d: eccentricAnomalyRad);
		double sqrtFactor = Math.Sqrt(d: Math.Max(val1: 0.0, val2: 1.0 - (e * e)));
		// Use the atan2 function to calculate the true anomaly in radians.
		double trueAnomalyRad = Math.Atan2(y: sqrtFactor * sinE, x: cosE - e);
		// Convert the true anomaly to degrees and normalize it to [0, 360) degrees.
		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: trueAnomalyRad));

	}

	/// <summary>Calculates the perihelion distance of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The perihelion distance of the orbit.</returns>
	/// <remarks>This method is used to calculate the perihelion distance of an orbit.</remarks>
	public static double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 - numericalEccentricity);

	/// <summary>Calculates the aphelion distance of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The aphelion distance of the orbit.</returns>
	/// <remarks>This method is used to calculate the aphelion distance of an orbit.</remarks>
	public static double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 + numericalEccentricity);

	/// <summary>Calculates the longitude of the descending node of an orbit.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node of the orbit.</param>
	/// <returns>The longitude of the descending node of the orbit.</returns>
	/// <remarks>This method is used to calculate the longitude of the descending node of an orbit.</remarks>
	public static double CalculateLongitudeDescendingNode(double longitudeAscendingNode) => NormalizeAngle360(degrees: longitudeAscendingNode + 180.0);

	/// <summary>Calculates the argument of aphelion of an orbit.</summary>
	/// <param name="argumentAphelion">The argument of perihelion of the orbit.</param>
	/// <returns>The argument of aphelion of the orbit.</returns>
	/// <remarks>This method is used to calculate the argument of aphelion of an orbit.</remarks>
	public static double CalculateArgumentOfAphelion(double argumentAphelion) => NormalizeAngle360(degrees: argumentAphelion + 180.0);

	/// <summary>Calculates the focal parameter of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The focal parameter of the orbit.</returns>
	/// <remarks>This method is used to calculate the focal parameter of an orbit.</remarks>
	public static double CalculateFocalParameter(double semiMajorAxis, double numericalEccentricity) => numericalEccentricity == 0
			? double.PositiveInfinity
			: semiMajorAxis * Math.Abs(value: 1.0 - (numericalEccentricity * numericalEccentricity)) / numericalEccentricity;

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
	public static double CalculatePeriod(double semiMajorAxis) => Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3));

	/// <summary>Calculates the orbital area of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital area of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital area of an ellipse.</remarks>
	public static double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity) => Math.PI * semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

	/// <summary>Calculates the orbital perimeter of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital perimeter of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital perimeter of an ellipse.</remarks>
	public static double CalculateOrbitalPerimeter(double semiMajorAxis, double numericalEccentricity)
	{
		// Use Ramanujan's approximation for the perimeter of an ellipse.
		double a = semiMajorAxis;
		double b = CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// If the semi-major and semi-minor axes are equal, the ellipse is a circle, and the perimeter is simply 2πa.
		if (a == b)
		{
			return 2.0 * Math.PI * a;
		}
		// For ellipses, use Ramanujan's approximation: P ≈ π * (a + b) * [1 + (3h / (10 + √(4 - 3h)))], where h = ((a - b)² / (a + b)²).
		double h = Math.Pow(x: a - b, y: 2) / Math.Pow(x: a + b, y: 2);
		return Math.PI * (a + b) * (1.0 + (3.0 * h / (10.0 + Math.Sqrt(d: 4.0 - (3.0 * h)))));
	}

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
	public static double CalculateStandardGravitationalParameter(double semiMajorAxis)
	{
		// The standard gravitational parameter (GM) can be calculated using Kepler's third law: GM = 4π²a³/T², where a is the semi-major axis and T is the orbital period.
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		return gm * Math.Pow(x: semiMajorAxis, y: 3) / (period * period);
	}

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
		// Calculate the orbital period of the planetoid using its semi-major axis.
		double planetoidPeriod = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		// Initialize a list to hold the results of the orbital resonance calculations.
		List<OrbitalResonance> results = new(capacity: SolarSystemPlanets.Length);
		// Iterate through each planet in the solar system to calculate its resonance with the planetoid.
		foreach ((string planetName, double planetPeriod) in SolarSystemPlanets)
		{
			// Calculate the ratio of the planet's orbital period to the planetoid's orbital period.
			double ratio = planetPeriod / planetoidPeriod;
			int bestP = 1;
			int bestQ = 1;
			double smallestDeviation = double.MaxValue;
			// Iterate through small integer values of p and q to find the closest resonance ratio.
			for (int p = 1; p <= 15; p++)
			{
				// Iterate through small integer values of q to find the closest resonance ratio.
				for (int q = 1; q <= 15; q++)
				{
					// Calculate the test ratio for the current p and q values.
					double testRatio = (double)p / q;
					// Calculate the percentage deviation of the test ratio from the actual ratio.
					double deviation = Math.Abs(value: testRatio - ratio) / ratio * 100.0;
					// If the current deviation is smaller than the smallest found so far, update the best p and q values.
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
				PlanetName: planetName,
				PlanetPeriod: planetPeriod,
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
	public static double CalculateDirectrix(double semiMajorAxis, double numericalEccentricity) => numericalEccentricity == 0 ? double.PositiveInfinity : semiMajorAxis / numericalEccentricity;

	/// <summary>Calculates the orbital velocity at perihelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The perihelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_p = sqrt(GM(1+e)/a(1-e)).</remarks>
	public static double CalculatePerihelionVelocity(double semiMajorAxis, double numericalEccentricity) => Math.Sqrt(d: gm * (1.0 + numericalEccentricity) / (semiMajorAxis * (1.0 - numericalEccentricity)));

	/// <summary>Calculates the orbital velocity at aphelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The aphelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_a = sqrt(GM(1-e)/a(1+e)).</remarks>
	public static double CalculateAphelionVelocity(double semiMajorAxis, double numericalEccentricity) => Math.Sqrt(d: gm * (1.0 - numericalEccentricity) / (semiMajorAxis * (1.0 + numericalEccentricity)));

	/// <summary>Calculates the mean orbital velocity.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean orbital velocity in AU/year.</returns>
	/// <remarks>Calculated as v_mean = 2πa/T.</remarks>
	public static double CalculateMeanOrbitalVelocity(double semiMajorAxis) => 2.0 * Math.PI * semiMajorAxis / CalculatePeriod(semiMajorAxis: semiMajorAxis);

	/// <summary>Calculates the current orbital velocity at a given true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The current orbital velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v = sqrt(GM(2/r - 1/a)).</remarks>
	public static double CalculateCurrentOrbitalVelocity(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: gm / p) * numericalEccentricity * Math.Sin(a: trueAnomalyRad);

	}

	/// <summary>Calculates the radial velocity component.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The radial velocity component in AU/year.</returns>
	/// <remarks>The radial component is perpendicular to the orbit.</remarks>
	public static double CalculateRadialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: gm / p) * (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRad)));
	}

	/// <summary>Calculates the tangential velocity component.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The tangential velocity component in AU/year.</returns>
	/// <remarks>The tangential component is along the orbit direction.</remarks>
	public static double CalculateTangentialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		// The tangential velocity component can be calculated using the vis-viva equation and the semi-latus rectum.
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		// Calculate the semi-latus rectum (p) of the orbit, which is used in the vis-viva equation.
		double semiLatusRectum = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Use the vis-viva equation to calculate the tangential velocity component: v_t = sqrt(GM/p) * (1 + e*cos(ν)), where ν is the true anomaly.
		//return Math.Sqrt(d: gm / semiLatusRectum) * (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRad)));
		return Math.Sqrt(d: gm / semiLatusRectum) * numericalEccentricity * Math.Sin(a: trueAnomalyRad);
	}

	/// <summary>Calculates the specific orbital energy.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The specific orbital energy in AU²/year².</returns>
	/// <remarks>Calculated as ε = -GM/(2a).</remarks>
	public static double CalculateSpecificOrbitalEnergy(double semiMajorAxis) => -gm / (2.0 * semiMajorAxis);

	/// <summary>Calculates the specific angular momentum.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The specific angular momentum in AU²/year.</returns>
	/// <remarks>Calculated as h = sqrt(GMa(1-e²)).</remarks>
	public static double CalculateSpecificAngularMomentum(double semiMajorAxis, double numericalEccentricity) => Math.Sqrt(d: gm * semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)));

	/// <summary>Calculates the vis-viva energy at a given position.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The vis-viva energy in AU²/year².</returns>
	/// <remarks>Calculated as E = v²/2 - GM/r.</remarks>
	public static double CalculateVisVivaEnergy(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		// Calculate the current orbital velocity using the vis-viva equation.
		double velocity = CalculateCurrentOrbitalVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly);
		// Convert the true anomaly from degrees to radians for the radius calculation.
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		// Calculate the current radius (distance from the focus) using the formula: r = a(1 - e²)/(1 + e*cos(ν)).
		double currentRadius = semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRad)));
		// Calculate the vis-viva energy using the formula: E = v²/2 - GM/r.
		return (velocity * velocity / 2.0) - (gm / currentRadius);
	}

	/// <summary>Calculates the longitude of perihelion.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>The longitude of perihelion in degrees.</returns>
	/// <remarks>Calculated as ϖ = Ω + ω.</remarks>
	public static double CalculateLongitudeOfPerihelion(double longitudeAscendingNode, double argumentPerihelion) => NormalizeAngle360(degrees: longitudeAscendingNode + argumentPerihelion);

	/// <summary>Calculates the mean longitude.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="meanAnomaly">The mean anomaly in degrees.</param>
	/// <returns>The mean longitude in degrees.</returns>
	/// <remarks>Calculated as λ = M + ϖ = M + Ω + ω.</remarks>
	public static double CalculateMeanLongitude(double longitudeAscendingNode, double argumentPerihelion, double meanAnomaly) => NormalizeAngle360(degrees: meanAnomaly + CalculateLongitudeOfPerihelion(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion));

	/// <summary>Calculates the argument of latitude.</summary>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The argument of latitude in degrees.</returns>
	/// <remarks>Calculated as u = ω + ν.</remarks>
	public static double CalculateArgumentOfLatitude(double argumentPerihelion, double trueAnomaly) => NormalizeAngle360(degrees: argumentPerihelion + trueAnomaly);

	/// <summary>Calculates the flight path angle.</summary>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The flight path angle in degrees.</returns>
	/// <remarks>Calculated as φ = arctan(e·sin(ν)/(1+e·cos(ν))).</remarks>
	public static double CalculateFlightPathAngle(double numericalEccentricity, double trueAnomaly)
	{
		// Convert the true anomaly from degrees to radians for the calculation.
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		// Calculate the flight path angle using the formula: φ = arctan(e·sin(ν)/(1+e·cos(ν))).
		double angle = Math.Atan(d: numericalEccentricity * Math.Sin(a: trueAnomalyRad) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRad))));
		// Convert the flight path angle back to degrees and return it.
		return double.RadiansToDegrees(radians: angle);
	}

	/// <summary>Calculates the time since perihelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time since perihelion in years.</returns>
	/// <remarks>Uses Kepler's equation and the mean motion.</remarks>
	public static double CalculateTimeSincePerihelion(double meanAnomaly, double semiMajorAxis)
	{
		// Calculate the orbital period of the orbit using the semi-major axis.
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		// Normalize the mean anomaly to a fraction of the full orbit (0 to 1).
		double meanAnomalyNormalized = meanAnomaly / 360.0;
		// Return the time since perihelion by multiplying the normalized mean anomaly by the orbital period.
		return meanAnomalyNormalized * period;
	}

	/// <summary>Calculates the time to next perihelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time to next perihelion in years.</returns>
	/// <remarks>Subtracts time since perihelion from the orbital period.</remarks>
	public static double CalculateTimeToNextPerihelion(double meanAnomaly, double semiMajorAxis) => CalculatePeriod(semiMajorAxis: semiMajorAxis) - CalculateTimeSincePerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);

	/// <summary>Calculates the time since aphelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time since aphelion in years.</returns>
	/// <remarks>Aphelion occurs at mean anomaly 180°.</remarks>
	public static double CalculateTimeSinceAphelion(double meanAnomaly, double semiMajorAxis)
	{
		// Calculate the orbital period of the orbit using the semi-major axis.
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		// Normalize the mean anomaly to find the anomaly from aphelion (mean anomaly - 180°).
		double anomalyFromAphelion = NormalizeAngle360(degrees: meanAnomaly - 180.0);
		// Return the time since aphelion by multiplying the normalized anomaly from aphelion by the orbital period.
		return anomalyFromAphelion / 360.0 * period;
	}

	/// <summary>Calculates the time to next aphelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time to next aphelion in years.</returns>
	/// <remarks>Subtracts time since aphelion from the orbital period.</remarks>
	public static double CalculateTimeToNextAphelion(double meanAnomaly, double semiMajorAxis) => CalculatePeriod(semiMajorAxis: semiMajorAxis) - CalculateTimeSinceAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);

	/// <summary>Calculates the synodic period with Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The synodic period in years.</returns>
	/// <remarks>Calculated as T_syn = 1/(|1/T₁ - 1/T₂|) where T₂ is Earth's period (1 year).</remarks>
	public static double CalculateSynodicPeriod(double semiMajorAxis)
	{
		// Calculate the orbital period of the object using its semi-major axis.
		double period = CalculatePeriod(semiMajorAxis: semiMajorAxis);
		// Calculate the absolute difference between the inverse of the object's period and Earth's period (1 year).
		double diff = Math.Abs(value: (1.0 / period) - 1.0);
		// Return the synodic period, handling the case where the difference is zero (which would imply an infinite synodic period).
		return diff == 0 ? double.PositiveInfinity : 1.0 / diff;

	}

	/// <summary>Calculates the Tisserand parameter with respect to Jupiter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter (dimensionless).</returns>
	/// <remarks>T_J = a_J/a + 2·cos(i)·sqrt(a(1-e²)/a_J) where a_J = 5.2 AU.</remarks>
	public static double CalculateTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// Jupiter's semi-major axis in AU
		const double jupiterSemiMajorAxis = 5.2;
		// Convert inclination from degrees to radians for the cosine calculation.
		double inclinationRadians = double.DegreesToRadians(degrees: inclination);
		// Calculate the Tisserand parameter using the formula: T_J = a_J/a + 2*cos(i)*sqrt(a(1-e²)/a_J).
		double term1 = jupiterSemiMajorAxis / semiMajorAxis;
		// Calculate the second term involving the inclination and eccentricity.
		double term2 = 2.0 * Math.Cos(d: inclinationRadians) * Math.Sqrt(d: semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / jupiterSemiMajorAxis);
		// Return the sum of the two terms to get the Tisserand parameter.
		return term1 + term2;
	}

	/// <summary>Calculates the mean distance from the focus (Sun).</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean distance from focus in AU.</returns>
	/// <remarks>Calculated as r_mean = a(1 + e²/2).</remarks>
	public static double CalculateMeanDistanceFromFocus(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1.0 + (numericalEccentricity * numericalEccentricity / 2.0));

	/// <summary>Calculates the geometric albedo-adjusted diameter.</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H.</param>
	/// <param name="geometricAlbedo">The geometric albedo (0.0 to 1.0).</param>
	/// <returns>The diameter in kilometers.</returns>
	/// <remarks>Calculated using D = 1329 / sqrt(albedo) * 10^(-0.2*H).</remarks>
	public static double CalculateGeometricAlbedoAdjustedDiameter(double absoluteMagnitude, double geometricAlbedo) => geometricAlbedo <= 0 ? 0.0 : 1329.0 / Math.Sqrt(d: geometricAlbedo) * Math.Pow(x: 10.0, y: -0.2 * absoluteMagnitude);
}