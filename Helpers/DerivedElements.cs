/*
 * File:        DerivedElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
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

using System.Diagnostics;

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating various orbital elements.</summary>
/// <remarks>This class contains methods for calculating the semi-minor axis, linear eccentricity, major axis, minor axis, and other orbital elements.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class DerivedElements
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

	private const double siderealEarthYearInDays = 365.242188;

	/// <summary>Represents the number of days in a Julian year.</summary>
	/// <remarks>The Julian year of 365.25 days is used for Julian date arithmetic and mean motion conversions.</remarks>
	private const double DaysPerJulianYear = 365.25;

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
		// Für e größer 1 = |a|·√(e²−1) ????
	}

	/// <summary>Calculates the linear eccentricity of an ellipse (c = a * e).</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The linear eccentricity of the ellipse.</returns>
	/// <remarks>This method is used to calculate the linear eccentricity of an ellipse.</remarks>
	public static double CalculateLinearEccentricity(double semiMajorAxis, double numericalEccentricity)
	{
		// The linear eccentricity (c) of an ellipse can be calculated using the formula: c = a * e, where a is the semi-major axis and e is the numerical eccentricity.
		return Math.Abs(value: semiMajorAxis * numericalEccentricity);
	}

	/// <summary>Calculates the major axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The major axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the major axis of an ellipse.</remarks>
	public static double CalculateMajorAxis(double semiMajorAxis)
	{
		// The major axis (A) of an ellipse is simply twice the semi-major axis (a).
		return 2 * semiMajorAxis;
	}

	/// <summary>Calculates the minor axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The minor axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the minor axis of an ellipse.</remarks>
	public static double CalculateMinorAxis(double semiMajorAxis, double numericalEccentricity)
	{
		// The minor axis (B) of an ellipse is simply twice the semi-minor axis (b).
		return 2 * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
	}

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
	public static double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity)
	{
		// The perihelion distance (q) of an orbit can be calculated using the formula: q = a * (1 - e), where a is the semi-major axis and e is the numerical eccentricity.
		return semiMajorAxis * (1 - numericalEccentricity);
	}

	/// <summary>Calculates the aphelion distance of an orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The aphelion distance of the orbit.</returns>
	/// <remarks>This method is used to calculate the aphelion distance of an orbit.</remarks>
	public static double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity)
	{
		// The aphelion distance (Q) of an orbit can be calculated using the formula: Q = a * (1 + e), where a is the semi-major axis and e is the numerical eccentricity.
		return semiMajorAxis * (1 + numericalEccentricity);
	}

	/// <summary>Calculates the longitude of the descending node of an orbit.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node of the orbit.</param>
	/// <returns>The longitude of the descending node of the orbit.</returns>
	/// <remarks>This method is used to calculate the longitude of the descending node of an orbit.</remarks>
	public static double CalculateLongitudeDescendingNode(double longitudeAscendingNode)
	{
		// The longitude of the descending node (Ω') can be calculated using the formula: Ω' = Ω + 180°, where Ω is the longitude of the ascending node.
		return NormalizeAngle360(degrees: longitudeAscendingNode + 180.0);
	}

	/// <summary>Calculates the argument of aphelion of an orbit.</summary>
	/// <param name="argumentPerihelion">The argument of perihelion of the orbit.</param>
	/// <returns>The argument of aphelion of the orbit.</returns>
	/// <remarks>This method is used to calculate the argument of aphelion of an orbit.</remarks>
	public static double CalculateArgumentOfAphelion(double argumentPerihelion)
	{
		// The argument of aphelion (ω') can be calculated using the formula: ω' = ω + 180°, where ω is the argument of perihelion.
		return NormalizeAngle360(degrees: argumentPerihelion + 180.0);
	}

	/// <summary>Calculates the semi-latus rectum of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The semi-latus rectum of the ellipse.</returns>
	/// <remarks>This method is used to calculate the semi-latus rectum of an ellipse.</remarks>
	public static double CalculateSemiLatusRectum(double semiMajorAxis, double numericalEccentricity)
	{
		// The semi-latus rectum (p) of an ellipse can be calculated using the formula: p = a * (1 - e^2), where a is the semi-major axis and e is the numerical eccentricity.
		return semiMajorAxis * (1 - (numericalEccentricity * numericalEccentricity));
	}

	/// <summary>Calculates the latus rectum of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The latus rectum of the ellipse.</returns>
	/// <remarks>This method is used to calculate the latus rectum of an ellipse.</remarks>
	public static double CalculateLatusRectum(double semiMajorAxis, double numericalEccentricity)
	{
		// The latus rectum (L) of an ellipse can be calculated using the formula: L = 2 * p, where p is the semi-latus rectum.
		return 2 * CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
	}

	/// <summary>Calculates the sidereal orbital period of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <returns>The sidereal orbital period of the ellipse.</returns>
	/// <remarks>This method is used to calculate the sidereal orbital period of an ellipse.</remarks>
	public static double CalculateSiderealOrbitalPeriod(double semiMajorAxis)
	{
		// The sidereal orbital period (T) of an ellipse can be calculated using Kepler's third law: T = √(a^3), where a is the semi-major axis.
		return Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3));
	}

	/// <summary>Calculates the orbital area of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital area of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital area of an ellipse.</remarks>
	public static double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity)
	{
		// The orbital area (A) of an ellipse can be calculated using the formula: A = π * a * b, where a is the semi-major axis and b is the semi-minor axis.
		return Math.PI * semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
	}

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
	public static double CalculateSemiMeanAxis(double semiMajorAxis, double numericalEccentricity)
	{
		// The semi-mean axis (a_m) of an ellipse can be calculated using the formula: a_m = √(a * b), where a is the semi-major axis and b is the semi-minor axis.
		return Math.Sqrt(d: semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
	}

	/// <summary>Calculates the mean axis of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The mean axis of the ellipse.</returns>
	/// <remarks>This method is used to calculate the mean axis of an ellipse.</remarks>
	public static double CalculateMeanAxis(double semiMajorAxis, double numericalEccentricity)
	{
		// The mean axis (a_mean) of an ellipse can be calculated using the formula: a_mean = 2 * a_m, where a_m is the semi-mean axis.
		return 2 * CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
	}

	/// <summary>Calculates the distance from the ellipse center to the directrix line in AU.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The center-to-directrix distance in AU. Returns PositiveInfinity for circular orbits (e = 0).</returns>
	/// <remarks>Calculated as d_center = |a| / e.</remarks>
	public static double CalculateCenterDirectrixDistance(double semiMajorAxis, double numericalEccentricity)
	{
		return numericalEccentricity == 0.0 ? double.PositiveInfinity : Math.Abs(value: semiMajorAxis) / numericalEccentricity;
	}

	/// <summary>Calculates the distance from the focal point (Sun) to the directrix line in AU.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The focus-to-directrix distance in AU. Returns PositiveInfinity for circular orbits (e = 0) and NaN for parabolic orbits (e = 1.0).</returns>
	/// <remarks>Calculated as d_focus = p / e = |a · (1 - e²)| / e.</remarks>
	public static double CalculateFocusDirectrixDistance(double semiMajorAxis, double numericalEccentricity)
	{
		switch (numericalEccentricity)
		{
			case 0.0:
				return double.PositiveInfinity;
			case 1.0:
				// A parabola (e = 1) cannot be parameterized by semi-major axis a.
				return double.NaN;
			default:
				break;
		}

		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return p / numericalEccentricity;
	}

	/// <summary>Calculates the current radius (distance from the focus).</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The current radius (distance from the focus) in AU using the formula: r = a(1 - e²)/(1 + e*cos(ν)).</returns>
	/// <remarks>Calculated as r = a(1 - e²)/(1 + e*cos(ν)).</remarks>
	public static double CalculateCurrentDistanceFromFocus(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double trueAnomalyRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: trueAnomaly));
		// The current distance from the focus (Sun) can be calculated using the formula: r = a(1 - e²)/(1 + e*cos(ν)), where a is the semi-major axis, e is the numerical eccentricity and ν is the true anomaly.
		return semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / (1.0 + (numericalEccentricity * Math.Cos(d: trueAnomalyRad)));
	}

	/// <summary>Calculates the mean distance from the focus (Sun).</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean distance from focus in AU.</returns>
	/// <remarks>Calculated as r_mean = a(1 + e²/2).</remarks>
	public static double CalculateMeanDistanceFromFocus(double semiMajorAxis, double numericalEccentricity)
	{
		// The mean distance from the focus (Sun) can be calculated using the formula: r_mean = a(1 + e²/2), where a is the semi-major axis and e is the numerical eccentricity.
		return semiMajorAxis * (1.0 + (numericalEccentricity * numericalEccentricity / 2.0));
	}

	/// <summary>Calculates the orbital resonances of a planetoid with the 8 solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the planetoid in AU.</param>
	/// <returns>A list of computed orbital resonances.</returns>
	/// <remarks>This method calculates the resonance with each major planet by finding the closest small-integer ratio.</remarks>
	public static List<OrbitalResonance> CalculateOrbitalResonances(double semiMajorAxis)
	{
		// Calculate the orbital period of the planetoid using its semi-major axis.
		double planetoidPeriod = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
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

	/// <summary>Calculates the orbital velocity at perihelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The perihelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_p = sqrt(GM(1+e)/a(1-e)).</remarks>
	public static double CalculatePerihelionVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		// The perihelion velocity (v_p) can be calculated using the vis-viva equation: v_p = sqrt(GM(1+e)/a(1-e)).
		return Math.Sqrt(d: gm * (1.0 + numericalEccentricity) / (semiMajorAxis * (1.0 - numericalEccentricity)));
	}

	/// <summary>Calculates the orbital velocity at aphelion.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The aphelion velocity in AU/year.</returns>
	/// <remarks>Uses the vis-viva equation: v_a = sqrt(GM(1-e)/a(1+e)).</remarks>
	public static double CalculateAphelionVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		// The aphelion velocity (v_a) can be calculated using the vis-viva equation: v_a = sqrt(GM(1-e)/a(1+e)).
		return Math.Sqrt(d: gm * (1.0 - numericalEccentricity) / (semiMajorAxis * (1.0 + numericalEccentricity)));
	}

	/// <summary>Calculates the mean orbital velocity for a circular orbit at the semi-minor axis distance.</summary>
	/// <param name="semiMinorAxis">The semi-minor axis in AU.</param>
	/// <returns>The mean orbital velocity at semi-minor axis in AU/year.</returns>
	/// <remarks>Calculated as v_mean = 2πb/T(b), which equals the circular-equivalent speed sqrt(GM/b).</remarks>
	public static double CalculateMeanOrbitalVelocityatSemiMinorAxis(double semiMinorAxis)
	{
		// The circular-equivalent mean velocity at radius b follows from v = 2πb/T with the period from Kepler's third law.
		return 2.0 * Math.PI * semiMinorAxis / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMinorAxis);
	}

	/// <summary>Calculates the mean orbital velocity at semi-major axis.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean orbital velocity at semi-mayor in AU/year.</returns>
	/// <remarks>Calculated as v_mean = 2πa/T.</remarks>
	public static double CalculateMeanOrbitalVelocityatSemiMajorAxis(double semiMajorAxis)
	{
		// The mean orbital velocity (v_mean) can be calculated using the formula: v_mean = 2πa/T, where a is the semi-major axis and T is the orbital period.
		return 2.0 * Math.PI * semiMajorAxis / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the mean speed along an elliptical orbit in AU/year. Uses Ramanujan's approximation for the ellipse perimeter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean orbital velocity in AU/year.</returns>
	public static double CalculateMeanOrbitalVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		return CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the instantaneous orbital speed in AU/year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The current orbital velocity in AU/year.</returns>
	public static double CalculateCurrentOrbitalVelocity(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double nu = double.DegreesToRadians(degrees: trueAnomaly);
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		double factor = 1.0 + (2.0 * numericalEccentricity * Math.Cos(d: nu)) + (numericalEccentricity * numericalEccentricity);
		return Math.Sqrt(d: gm / p * factor);
	}

	/// <summary>Calculates the radial velocity component in AU/year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The radial velocity component in AU/year.</returns>
	public static double CalculateRadialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double nu = double.DegreesToRadians(degrees: trueAnomaly);
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: gm / p) * numericalEccentricity * Math.Sin(a: nu);
	}

	/// <summary>Calculates the tangential velocity component.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The tangential velocity component in AU/year.</returns>
	/// <remarks>The tangential component is along the orbit direction.</remarks>
	public static double CalculateTangentialVelocityComponent(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		double nu = double.DegreesToRadians(degrees: trueAnomaly);
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return Math.Sqrt(d: gm / p) * (1.0 + (numericalEccentricity * Math.Cos(d: nu)));
	}

	/// <summary>Calculates the specific angular momentum.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The specific angular momentum in AU²/year.</returns>
	/// <remarks>Calculated as h = sqrt(GMa(1-e²)). Unit is AU²/year (angular momentum per unit mass).</remarks>
	public static double CalculateSpecificOrbitalEnergy(double semiMajorAxis)
	{
		// The specific orbital energy (ε) can be calculated using the formula: ε = -GM/(2a), where a is the semi-major axis.
		return -gm / (2.0 * semiMajorAxis);
	}

	/// <summary>Calculates the specific angular momentum.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The specific angular momentum in AU²/year.</returns>
	/// <remarks>Calculated as h = sqrt(GMa(1-e²)). Unit is AU²/year (angular momentum per unit mass).</remarks>
	public static double CalculateSpecificAngularMomentum(double semiMajorAxis, double numericalEccentricity)
	{
		return Math.Sqrt(d: gm * semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)));
	}

	/// <summary>Calculates the longitude of perihelion.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>The longitude of perihelion in degrees.</returns>
	/// <remarks>Calculated as ϖ = Ω + ω.</remarks>
	public static double CalculateLongitudeOfPerihelion(double longitudeAscendingNode, double argumentPerihelion)
	{
		// The longitude of perihelion (ϖ) can be calculated using the formula: ϖ = Ω + ω, where Ω is the longitude of the ascending node and ω is the argument of perihelion.
		return NormalizeAngle360(degrees: longitudeAscendingNode + argumentPerihelion);
	}

	/// <summary>Calculates the mean longitude.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="meanAnomaly">The mean anomaly in degrees.</param>
	/// <returns>The mean longitude in degrees.</returns>
	/// <remarks>Calculated as λ = M + ϖ = M + Ω + ω.</remarks>
	public static double CalculateMeanLongitude(double longitudeAscendingNode, double argumentPerihelion, double meanAnomaly)
	{
		// The mean longitude (λ) can be calculated using the formula: λ = M + ϖ = M + Ω + ω, where M is the mean anomaly, ϖ is the longitude of perihelion, Ω is the longitude of the ascending node, and ω is the argument of perihelion.
		return NormalizeAngle360(degrees: meanAnomaly + CalculateLongitudeOfPerihelion(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion));
	}

	/// <summary>Calculates the argument of latitude.</summary>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The argument of latitude in degrees.</returns>
	/// <remarks>Calculated as u = ω + ν.</remarks>
	public static double CalculateArgumentOfLatitude(double argumentPerihelion, double trueAnomaly)
	{
		// The argument of latitude (u) can be calculated using the formula: u = ω + ν, where ω is the argument of perihelion and ν is the true anomaly.
		return NormalizeAngle360(degrees: argumentPerihelion + trueAnomaly);
	}

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
		double period = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		// Normalize the mean anomaly to a fraction of the full orbit (0 to 1).
		double meanAnomalyNormalized = NormalizeAngle360(degrees: meanAnomaly) / 360.0;
		// Return the time since perihelion by multiplying the normalized mean anomaly by the orbital period.
		return meanAnomalyNormalized * period;
	}

	/// <summary>Calculates the time to next perihelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time to next perihelion in years.</returns>
	/// <remarks>Subtracts time since perihelion from the orbital period.</remarks>
	public static double CalculateTimeToNextPerihelion(double meanAnomaly, double semiMajorAxis)
	{
		// The time to next perihelion can be calculated by subtracting the time since perihelion from the orbital period.
		return CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) - CalculateTimeSincePerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the time since aphelion passage.</summary>
	/// <param name="meanAnomaly">The current mean anomaly in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The time since aphelion in years.</returns>
	/// <remarks>Aphelion occurs at mean anomaly 180°.</remarks>
	public static double CalculateTimeSinceAphelion(double meanAnomaly, double semiMajorAxis)
	{
		// Calculate the orbital period of the orbit using the semi-major axis.
		double period = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
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
	public static double CalculateTimeToNextAphelion(double meanAnomaly, double semiMajorAxis)
	{
		// The time to next aphelion can be calculated by subtracting the time since aphelion from the orbital period.
		return CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) - CalculateTimeSinceAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the synodic orbital period with Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The synodic period in years.</returns>
	/// <remarks>Calculated as T_syn = 1/(|1/T₁ - 1/T₂|) where T₂ is Earth's period (1 year).</remarks>
	public static double CalculateSynodicOrbitalPeriod(double semiMajorAxis)
	{
		// Calculate the orbital period of the object using its semi-major axis.
		double period = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		// Calculate the absolute difference between the inverse of the object's period and Earth's period (1 year).
		double diff = Math.Abs(value: (1.0 / period) - 1.0);
		// Return the synodic period, handling the case where the difference is zero (which would imply an infinite synodic period).
		return diff == 0 ? double.PositiveInfinity : 1.0 / diff;
	}

	/// <summary>Calculates the Tisserand parameter with respect to a main body.</summary>
	/// <param name="semiMajorAxisMain">The semi-major axis of the main body in AU.</param>
	/// <param name="semiMajorAxis">The semi-major axis of the encounter body in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the encounter body.</param>
	/// <param name="inclination">The inclination of the encounter body in degrees.</param>
	/// <returns>The Tisserand parameter (dimensionless).</returns>
	/// <remarks>Calculated as T = a_main/a + 2·cos(i)·sqrt(a(1-e²)/a_main). Derived from the restricted three-body problem, conserving value during planetary close encounters.</remarks>
	public static double CalculateTisserandParameter(double semiMajorAxisMain, double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		double term1 = semiMajorAxisMain / semiMajorAxis;
		double term2 = 2.0 * Math.Cos(d: inclinationRad) * Math.Sqrt(d: semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / semiMajorAxisMain);
		return term1 + term2;
	}

	/// <summary>Calculates the Tisserand parameter with respect to Mercury.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Mercury (dimensionless).</returns>
	public static double CalculateMercuryTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double mercurySemiMajorAxis = 0.38709893;
		return CalculateTisserandParameter(semiMajorAxisMain: mercurySemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Venus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Venus (dimensionless).</returns>
	public static double CalculateVenusTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double venusSemiMajorAxis = 0.72333199;
		return CalculateTisserandParameter(semiMajorAxisMain: venusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Earth (dimensionless).</returns>
	public static double CalculateEarthTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double earthSemiMajorAxis = 1.00000011;
		return CalculateTisserandParameter(semiMajorAxisMain: earthSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Mars.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Mars (dimensionless).</returns>
	public static double CalculateMarsTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double marsSemiMajorAxis = 1.52366231;
		return CalculateTisserandParameter(semiMajorAxisMain: marsSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Jupiter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Jupiter (dimensionless).</returns>
	/// <remarks>T_J &lt; 2: Hyperbolic/cometary; 2 &lt; T_J &lt; 3: Jupiter-family comets; T_J &gt; 3: Main-belt asteroids.</remarks>
	public static double CalculateJupiterTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double jupiterSemiMajorAxis = 5.20336301;
		return CalculateTisserandParameter(semiMajorAxisMain: jupiterSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Saturn.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Saturn (dimensionless).</returns>
	public static double CalculateSaturnTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double saturnSemiMajorAxis = 9.53707032;
		return CalculateTisserandParameter(semiMajorAxisMain: saturnSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Uranus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Uranus (dimensionless).</returns>
	public static double CalculateUranusTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double uranusSemiMajorAxis = 19.19126393;
		return CalculateTisserandParameter(semiMajorAxisMain: uranusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Neptune.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Neptune (dimensionless).</returns>
	public static double CalculateNeptuneTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double neptuneSemiMajorAxis = 30.06896348;
		return CalculateTisserandParameter(semiMajorAxisMain: neptuneSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to a primary body.</summary>
	/// <param name="semiMajorAxisMain">The semi-major axis of the main body in AU.</param>
	/// <param name="semiMajorAxis">The semi-major axis of the encounter body in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the encounter body.</param>
	/// <param name="inclination">The orbital inclination of the encounter body in degrees.</param>
	/// <returns>The encounter velocity normalized to the primary body's circular velocity (dimensionless, v_rel / v_c).</returns>
	/// <remarks>Calculated as U = sqrt(3 - T), where T is the Tisserand parameter.</remarks>
	public static double CalculateEncounterVelocity(double semiMajorAxisMain, double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		return Math.Sqrt(d: 3.0 - CalculateTisserandParameter(semiMajorAxisMain: semiMajorAxisMain, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination));
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Mercury.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Mercury.</returns>
	public static double CalculateEncounterVelocityRelativeToMercury(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double mercurySemiMajorAxis = 0.38709893;
		return CalculateEncounterVelocity(semiMajorAxisMain: mercurySemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Venus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Venus.</returns>
	public static double CalculateEncounterVelocityRelativeToVenus(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double venusSemiMajorAxis = 0.72333199;
		return CalculateEncounterVelocity(semiMajorAxisMain: venusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Earth.</returns>
	public static double CalculateEncounterVelocityRelativeToEarth(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double earthSemiMajorAxis = 1.00000011;
		return CalculateEncounterVelocity(semiMajorAxisMain: earthSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Mars.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Mars.</returns>
	public static double CalculateEncounterVelocityRelativeToMars(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double marsSemiMajorAxis = 1.52366231;
		return CalculateEncounterVelocity(semiMajorAxisMain: marsSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Jupiter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Jupiter.</returns>
	public static double CalculateEncounterVelocityRelativeToJupiter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double jupiterSemiMajorAxis = 5.20336301;
		return CalculateEncounterVelocity(semiMajorAxisMain: jupiterSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Saturn.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Saturn.</returns>
	public static double CalculateEncounterVelocityRelativeToSaturn(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double saturnSemiMajorAxis = 9.53707032;
		return CalculateEncounterVelocity(semiMajorAxisMain: saturnSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Uranus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Uranus.</returns>
	public static double CalculateEncounterVelocityRelativeToUranus(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double uranusSemiMajorAxis = 19.19126393;
		return CalculateEncounterVelocity(semiMajorAxisMain: uranusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Neptune.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Neptune.</returns>
	public static double CalculateEncounterVelocityRelativeToNeptune(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		const double neptuneSemiMajorAxis = 30.06896348;
		return CalculateEncounterVelocity(semiMajorAxisMain: neptuneSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}
	/// <summary>Calculates the geometric albedo-adjusted diameter.</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H.</param>
	/// <param name="geometricAlbedo">The geometric albedo (0.0 to 1.0).</param>
	/// <returns>The diameter in kilometers.</returns>
	/// <remarks>Calculated using D = 1329 / sqrt(albedo) * 10^(-0.2*H).</remarks>
	public static double CalculateGeometricAlbedoAdjustedDiameter(double absoluteMagnitude, double geometricAlbedo)
	{
		// The geometric albedo-adjusted diameter can be calculated using the formula: D = 1329 / sqrt(albedo) * 10^(-0.2*H), where H is the absolute magnitude and albedo is the geometric albedo.
		return geometricAlbedo <= 0 ? double.NaN : 1329.0 / Math.Sqrt(d: geometricAlbedo) * Math.Pow(x: 10.0, y: -0.2 * absoluteMagnitude);
	}

	/// <summary>Calculates the Kozai integral / Kozai quantity.</summary>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="orbitalInclination">The orbital inclination in degrees.</param>
	/// <returns>The Kozai quantity (dimensionless).</returns>
	/// <remarks>Calculated as sqrt(1 - e²) · cos(i). Conserved parameter in Kozai-Lidov secular resonance dynamics.</remarks>
	public static double CalculateKozaiQuantity(double numericalEccentricity, double orbitalInclination)
	{
		double inclinationRad = double.DegreesToRadians(degrees: orbitalInclination);
		return Math.Sqrt(d: 1.0 - (numericalEccentricity * numericalEccentricity)) * Math.Cos(d: inclinationRad);
	}

	/// <summary>Calculates the equinoctial orbital element h.</summary>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element h (dimensionless).</returns>
	/// <remarks>Calculated as h = e · sin(ϖ), where ϖ = ω + Ω is the longitude of perihelion. Avoids singularities for zero inclination.</remarks>
	public static double CalculateEquinoctialOrbitalElementH(double numericalEccentricity, double argumentPerihelion, double lengthAscendingNode)
	{
		double argumentPerihelionRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: argumentPerihelion));
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		return numericalEccentricity * Math.Sin(a: argumentPerihelionRad + lengthAscendingNodeRad);
	}

	/// <summary>Calculates the equinoctial orbital element k.</summary>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element k (dimensionless).</returns>
	/// <remarks>Calculated as k = e · cos(ϖ), where ϖ = ω + Ω is the longitude of perihelion. Avoids singularities for zero inclination.</remarks>
	public static double CalculateEquinoctialOrbitalElementK(double numericalEccentricity, double argumentPerihelion, double lengthAscendingNode)
	{
		double argumentPerihelionRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: argumentPerihelion));
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		return numericalEccentricity * Math.Cos(d: argumentPerihelionRad + lengthAscendingNodeRad);
	}

	/// <summary>Calculates the equinoctial orbital element p.</summary>
	/// <param name="orbitalInclination">The orbital inclination in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element p (dimensionless).</returns>
	/// <remarks>Calculated as p = tan(i / 2) · sin(Ω). Avoids singularities for zero eccentricity.</remarks>
	public static double CalculateEquinoctialOrbitalElementP(double orbitalInclination, double lengthAscendingNode)
	{
		double orbitalInclinationRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: orbitalInclination));
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		return Math.Tan(a: orbitalInclinationRad / 2.0) * Math.Sin(a: lengthAscendingNodeRad);
	}

	/// <summary>Calculates the equinoctial orbital element q.</summary>
	/// <param name="orbitalInclination">The orbital inclination in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element q (dimensionless).</returns>
	/// <remarks>Calculated as q = tan(i / 2) · cos(Ω). Avoids singularities for zero eccentricity.</remarks>
	public static double CalculateEquinoctialOrbitalElementQ(double orbitalInclination, double lengthAscendingNode)
	{
		double orbitalInclinationRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: orbitalInclination));
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		return Math.Tan(a: orbitalInclinationRad / 2.0) * Math.Cos(d: lengthAscendingNodeRad);
	}

	// <summary>Calculates the mean daily orbital motion of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The mean daily orbital motion in AU/days.</returns>
	/// <remarks>Calculated using the formula = p/365.242188, where p is the orbital perimenter and 365.242188 is the sidereal orbital period in 365 days, 5 hours, 48 minutes and 45 seconds.</remarks>
	public static double CalculateMeanDailyOrbitalMotion(double semiMajorAxis, double numericalEccentricity)
	{
		return CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / siderealEarthYearInDays;
	}

	/// <summary>Calculates the mean motion n in degrees per year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean motion in deg/year.</returns>
	/// <remarks>Calculated as n = 360° / T.</remarks>
	public static double CalculateMeanMotion(double semiMajorAxis)
	{
		return 360.0 / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the mean daily motion n in degrees per day.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean daily motion in deg/day.</returns>
	/// <remarks>Calculated as n_day = (360° / T) / 365.242188.</remarks>
	public static double CalculateMeanDailyMotion(double semiMajorAxis)
	{
		return CalculateMeanMotion(semiMajorAxis: semiMajorAxis) / siderealEarthYearInDays;
	}

	/// <summary>Calculates the mean motion n in radians per year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean motion in rad/year.</returns>
	/// <remarks>Calculated as n = 2π / T.</remarks>
	public static double CalculateMeanMotionRad(double semiMajorAxis)
	{
		return 2.0 * Math.PI / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the instantaneous angular velocity dν/dt at a given true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="nu">The true anomaly in degrees.</param>
	/// <returns>The instantaneous angular velocity in rad/year.</returns>
	/// <remarks>Calculated as ω = h / r².</remarks>
	public static double CalculateAngularVelocity(double semiMajorAxis, double numericalEccentricity, double nu)
	{
		double r = CalculateCurrentDistanceFromFocus(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: nu);
		double h = CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return h / (r * r);
	}

	/// <summary>Calculates the areal velocity dA/dt along the orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The areal velocity in AU²/year.</returns>
	/// <remarks>Calculated as dA/dt = h / 2. Follows Kepler's second law (constant rate of area swept per time unit).</remarks>
	public static double CalculateArealVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		return CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / 2.0;
	}

	/// <summary>Calculates the maximum height z_max above the ecliptic plane.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="inclination">The orbital inclination in degrees.</param>
	/// <returns>The maximum distance from the ecliptic plane in AU.</returns>
	/// <remarks>Simplified as z_max = a · sin(i).</remarks>
	public static double CalculateMaxEclipticDistance(double semiMajorAxis, double inclination)
	{
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		return semiMajorAxis * Math.Sin(a: inclinationRad);
	}

	/// <summary>Calculates the heliocentric latitude b for a given orbital position.</summary>
	/// <param name="argPeri">The argument of perihelion in degrees.</param>
	/// <param name="trueAnom">The true anomaly in degrees.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The heliocentric latitude in degrees in the range [-90°, 90°].</returns>
	/// <remarks>Calculated as b = arcsin(sin(i) · sin(ω + ν)).</remarks>
	public static double CalculateHeliocentricLatitude(double argPeri, double trueAnom, double inclination)
	{
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		double uRad = double.DegreesToRadians(degrees: argPeri + trueAnom);
		return double.RadiansToDegrees(radians: Math.Asin(d: Math.Sin(a: inclinationRad) * Math.Sin(a: uRad)));
	}

	/// <summary>Calculates the true longitude L along the orbit.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The true longitude in degrees normalized to [0, 360°).</returns>
	/// <remarks>Calculated as L = Ω + ω + ν. Well-defined even for circular orbits where ω and ν individually have singularities.</remarks>
	public static double CalculateTrueLongitude(double longitudeAscendingNode, double argumentPerihelion, double trueAnomaly)
	{
		double argumentOfLatitude = argumentPerihelion + trueAnomaly;
		return NormalizeAngle360(degrees: longitudeAscendingNode + argumentOfLatitude);
	}

	/// <summary>Calculates the heliocentric ecliptic longitude λ projected onto the ecliptic plane.</summary>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The projected ecliptic longitude in degrees normalized to [0, 360°).</returns>
	/// <remarks>Calculated as λ = Ω + atan2(sin(u)·cos(i), cos(u)), where u = ω + ν.</remarks>
	public static double CalculateHeliocentricLongitude(double longitudeAscendingNode, double argumentPerihelion, double trueAnomaly, double inclination)
	{
		double OmegaRad = double.DegreesToRadians(degrees: longitudeAscendingNode);
		double uRad = double.DegreesToRadians(degrees: argumentPerihelion + trueAnomaly);
		double iRad = double.DegreesToRadians(degrees: inclination);

		double lonOffset = Math.Atan2(Math.Sin(uRad) * Math.Cos(d: iRad), Math.Cos(d: uRad));

		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: OmegaRad + lonOffset));
	}

	/// <summary>Calculates the heliocentric ecliptic longitude λ using full 3D Cartesian coordinates.</summary>
	/// <param name="a">The semi-major axis in AU.</param>
	/// <param name="e">The numerical eccentricity.</param>
	/// <param name="i">The inclination in degrees.</param>
	/// <param name="Omega">The longitude of the ascending node in degrees.</param>
	/// <param name="omega">The argument of perihelion in degrees.</param>
	/// <param name="nu">The true anomaly in degrees.</param>
	/// <returns>The ecliptic longitude in degrees normalized to [0, 360°).</returns>
	public static double CalculateHeliocentricLongitudeFull(double a, double e, double i, double Omega, double omega, double nu)
	{
		double u = omega + nu;
		double r = a * (1 - (e * e)) / (1 + (e * Math.Cos(d: double.DegreesToRadians(degrees: nu))));

		double OmegaRad = double.DegreesToRadians(degrees: Omega);
		double uRad = double.DegreesToRadians(degrees: u);
		double iRad = double.DegreesToRadians(degrees: i);

		double x = r * ((Math.Cos(d: OmegaRad) * Math.Cos(d: uRad)) - (Math.Sin(a: OmegaRad) * Math.Sin(a: uRad) * Math.Cos(d: iRad)));
		double y = r * ((Math.Sin(a: OmegaRad) * Math.Cos(d: uRad)) + (Math.Cos(d: OmegaRad) * Math.Sin(a: uRad) * Math.Cos(d: iRad)));

		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: Math.Atan2(y: y, x: x)));
	}

	/// <summary>Calculates the characteristic energy C3 for interplanetary trajectory analysis.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The characteristic energy C3 in AU²/year².</returns>
	/// <remarks>Calculated as C3 = -GM / a = 2 · ε (twice the specific orbital energy).</remarks>
	public static double CalculateCharacteristicEnergy(double semiMajorAxis)
	{
		return gm * -1.0 / semiMajorAxis;
	}

	/// <summary>Calculates the radius of curvature of the elliptical orbit at a given true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The radius of curvature in AU.</returns>
	/// <remarks>Calculated as R_c(ν) = a(1 - e²) / ((1 + e·cos(ν))³ · sqrt(1 + 2e·cos(ν) + e²)).</remarks>
	public static double CalculateRadiusOfCurvature(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		// Convert the true anomaly from degrees to radians.
		double trueAnomalyRad = double.DegreesToRadians(degrees: trueAnomaly);
		double e = numericalEccentricity;
		double cosNu = Math.Cos(d: trueAnomalyRad);

		// Calculate numerator: p = a * (1 - e^2)
		double numerator = semiMajorAxis * (1.0 - (e * e));

		// Calculate denominator components
		double factor1 = Math.Pow(x: 1.0 + (e * cosNu), y: 3);
		double factor2 = Math.Sqrt(d: 1.0 + (2.0 * e * cosNu) + (e * e));

		return numerator / (factor1 * factor2);
	}

	/// <summary>Calculates the Hill radius of a body (sphere of gravitational dominance against the Sun).</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the body's orbit in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="massBodyKg">The mass of the body in kilograms.</param>
	/// <returns>The conservative Hill radius in AU (at perihelion).</returns>
	/// <remarks>Calculated as r_Hill = a · (1 - e) · (m / (3 · M_Sun))^(1/3).</remarks>
	public static double CalculateHillRadius(double semiMajorAxis, double numericalEccentricity, double massBodyKg)
	{
		// Mass of the Sun in kg
		const double solarMassKg = 1.98847e30;

		// Minimum radius occurs at perihelion: q = a * (1 - e)
		double perihelionDistance = semiMajorAxis * (1.0 - numericalEccentricity);

		// Compute Hill radius in AU
		return perihelionDistance * Math.Cbrt(d: massBodyKg / (3.0 * solarMassKg));
	}

	/// <summary>Calculates the Laplace Sphere of Influence (SOI) radius of a body relative to the Sun.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="massBodyKg">The mass of the body in kilograms.</param>
	/// <returns>The Sphere of Influence radius in AU.</returns>
	/// <remarks>Calculated as r_SOI = a · (m / M_Sun)^(2/5).</remarks>
	public static double CalculateSphereOfInfluence(double semiMajorAxis, double massBodyKg)
	{
		// Mass of the Sun in kg
		const double solarMassKg = 1.98847e30;

		// Compute SOI radius in AU
		return semiMajorAxis * Math.Pow(x: massBodyKg / solarMassKg, y: 0.4);
	}

	/// <summary>Calculates the fluid Roche limit for a satellite orbiting the body.</summary>
	/// <param name="bodyRadiusKm">The mean radius of the primary body in kilometers.</param>
	/// <param name="bodyDensity">The mean density of the primary body (e.g., in g/cm³ or kg/m³).</param>
	/// <param name="satelliteDensity">The mean density of the orbiting satellite (same units as bodyDensity).</param>
	/// <returns>The fluid Roche limit in kilometers from the primary body's center.</returns>
	/// <remarks>Calculated as d_Roche = 2.44 · R · (ρ_body / ρ_satellite)^(1/3).</remarks>
	public static double CalculateFluidRocheLimit(double bodyRadiusKm, double bodyDensity, double satelliteDensity)
	{
		if (satelliteDensity <= 0.0)
		{
			return double.PositiveInfinity;
		}

		// 2.44 is the standard coefficient for fluid/deformable bodies
		return 2.44 * bodyRadiusKm * Math.Cbrt(d: bodyDensity / satelliteDensity);
	}

	/// <summary>Calculates the rigid Roche limit for a solid satellite orbiting the body.</summary>
	/// <param name="bodyRadiusKm">The mean radius of the primary body in kilometers.</param>
	/// <param name="bodyDensity">The mean density of the primary body.</param>
	/// <param name="satelliteDensity">The mean density of the rigid satellite.</param>
	/// <returns>The rigid Roche limit in kilometers from the primary body's center.</returns>
	/// <remarks>Calculated as d_Roche = 1.26 · R · (ρ_body / ρ_satellite)^(1/3).</remarks>
	public static double CalculateRigidRocheLimit(double bodyRadiusKm, double bodyDensity, double satelliteDensity)
	{
		if (satelliteDensity <= 0.0)
		{
			return double.PositiveInfinity;
		}

		// 1.26 is the standard coefficient for rigid/solid bodies
		return 1.26 * bodyRadiusKm * Math.Cbrt(d: bodyDensity / satelliteDensity);
	}

	/// <summary>Gets the typical mean density for a given asteroid taxonomic type in g/cm³.</summary>
	/// <param name="type">The asteroid taxonomic type.</param>
	/// <returns>The estimated density in g/cm³.</returns>
	public static double GetTypicalAsteroidDensity(AsteroidType type)
	{
		return type switch
		{
			AsteroidType.CType => 1.3,
			AsteroidType.SType => 2.7,
			AsteroidType.MType => 4.5,
			AsteroidType.IcyObject => 1.0,
			AsteroidType.VType => 3.0,
			_ => 2.0 // Generic default asteroid density
		};
	}

	/// <summary>Calculates the estimated mass of a body from its diameter and density.</summary>
	/// <param name="diameterKm">The mean diameter of the body in kilometers.</param>
	/// <param name="densityGPerCm3">The mean density in g/cm³ (e.g., 2.7 for S-type).</param>
	/// <returns>The estimated mass in kilograms.</returns>
	/// <remarks>Assumes a spherical body where Mass = (4/3) · π · R³ · ρ.</remarks>
	public static double CalculateMassFromDiameter(double diameterKm, double densityGPerCm3)
	{
		if (diameterKm <= 0.0 || densityGPerCm3 <= 0.0)
		{
			return 0.0;
		}

		// Convert radius to meters: R = (diameter / 2) * 1000
		double radiusMeters = diameterKm / 2.0 * 1000.0;

		// Convert density from g/cm³ to kg/m³: 1 g/cm³ = 1000 kg/m³
		double densityKgPerM3 = densityGPerCm3 * 1000.0;

		// Volume of a sphere: V = (4/3) * π * r³
		double volumeM3 = 4.0 / 3.0 * Math.PI * Math.Pow(x: radiusMeters, y: 3);

		// Mass in kg = Volume in m³ * Density in kg/m³
		return volumeM3 * densityKgPerM3;
	}

	/// <summary>Calculates the estimated mass directly using an asteroid type classification.</summary>
	/// <param name="diameterKm">The mean diameter in kilometers.</param>
	/// <param name="type">The taxonomic type of the asteroid.</param>
	/// <returns>The estimated mass in kilograms.</returns>
	public static double CalculateMassFromAsteroidType(double diameterKm, AsteroidType type)
	{
		double density = GetTypicalAsteroidDensity(type: type);
		return CalculateMassFromDiameter(diameterKm: diameterKm, densityGPerCm3: density);
	}

	/// <summary>Calculates the surface gravity of a spherical body.</summary>
	/// <param name="massKg">Mass in kilograms.</param>
	/// <param name="radiusKm">Mean radius in kilometers.</param>
	/// <returns>Surface gravity in m/s².</returns>
	/// <remarks>Calculated as g = G · m / r².</remarks>
	public static double CalculateSurfaceGravity(double massKg, double radiusKm)
	{
		if (radiusKm <= 0.0)
		{
			return 0.0;
		}

		const double gravitationalConstant = 6.67430e-11; // m³ kg⁻¹ s⁻²
		double radiusMeters = radiusKm * 1000.0;

		return gravitationalConstant * massKg / (radiusMeters * radiusMeters);
	}

	/// <summary>Calculates the escape velocity from the surface of a spherical body.</summary>
	/// <param name="massKg">Mass in kilograms.</param>
	/// <param name="radiusKm">Mean radius in kilometers.</param>
	/// <returns>Escape velocity in m/s.</returns>
	/// <remarks>Calculated as v_esc = sqrt(2 · G · m / r).</remarks>
	public static double CalculateEscapeVelocity(double massKg, double radiusKm)
	{
		if (radiusKm <= 0.0)
		{
			return 0.0;
		}

		const double gravitationalConstant = 6.67430e-11; // m³ kg⁻¹ s⁻²
		double radiusMeters = radiusKm * 1000.0;

		return Math.Sqrt(d: 2.0 * gravitationalConstant * massKg / radiusMeters);
	}

	/// <summary>Evaluates whether an asteroid is classified as a Potentially Hazardous Asteroid (PHA).</summary>
	/// <param name="earthMoidAu">The Minimum Orbit Intersection Distance (MOID) to Earth in AU.</param>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag.</param>
	/// <returns>A PhaEvaluation structure with detailed criteria checks.</returns>
	/// <remarks>Criteria: Earth MOID &lt;= 0.05 AU and H &lt;= 22.0 mag (NASA / JPL definition).</remarks>
	public static PhaEvaluation EvaluatePhaClassification(double earthMoidAu, double absoluteMagnitudeH)
	{
		// Critical thresholds defined by NASA/JPL/IAU
		const double moidThresholdAu = 0.05;
		const double magnitudeThresholdH = 22.0;

		bool isMoidCritical = earthMoidAu is >= 0.0 and <= moidThresholdAu;
		bool isMagnitudeCritical = absoluteMagnitudeH <= magnitudeThresholdH;

		// Is classified as PHA if BOTH criteria are met
		bool isPha = isMoidCritical && isMagnitudeCritical;

		// Calculate estimated diameter in meters at the H = 22.0 threshold using geometric albedo = 0.14
		double estimatedMinDiameterMeters = CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: absoluteMagnitudeH, geometricAlbedo: 0.14) * 1000.0;

		return new PhaEvaluation(IsPha: isPha, IsMoidCritical: isMoidCritical, IsMagnitudeCritical: isMagnitudeCritical, EstimatedMinDiameterMeters: estimatedMinDiameterMeters);
	}

	/// <summary>Simple helper to check if an object is a Potentially Hazardous Asteroid (PHA).</summary>
	/// <param name="earthMoidAu">The Minimum Orbit Intersection Distance (MOID) to Earth in AU.</param>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag.</param>
	/// <returns>True if the object is a PHA; otherwise, false.</returns>
	public static bool IsPotentiallyHazardous(double earthMoidAu, double absoluteMagnitudeH)
	{
		return earthMoidAu >= 0.0 && earthMoidAu <= 0.05 && absoluteMagnitudeH <= 22.0;
	}

	/// <summary>Calculates the eccentric anomaly E (in degrees) from a given true anomaly ν (in degrees).</summary>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The eccentric anomaly in degrees in the range [0, 360).</returns>
	public static double CalculateEccentricAnomalyFromTrueAnomaly(double trueAnomaly, double numericalEccentricity)
	{
		double nuRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: trueAnomaly));
		double e = numericalEccentricity;

		// tan(E/2) = sqrt((1-e)/(1+e)) * tan(ν/2)
		double factor = Math.Sqrt(d: (1.0 - e) / (1.0 + e));
		double tanHalfE = factor * Math.Tan(a: nuRad / 2.0);
		double eRad = 2.0 * Math.Atan(d: tanHalfE);

		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: eRad));
	}

	/// <summary>Calculates the mean anomaly M (in degrees) from a given true anomaly ν (in degrees).</summary>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean anomaly in degrees in the range [0, 360).</returns>
	public static double CalculateMeanAnomalyFromTrueAnomaly(double trueAnomaly, double numericalEccentricity)
	{
		double eDeg = CalculateEccentricAnomalyFromTrueAnomaly(trueAnomaly: trueAnomaly, numericalEccentricity: numericalEccentricity);
		double eRad = double.DegreesToRadians(degrees: eDeg);
		double e = numericalEccentricity;

		// Kepler's Equation: M = E - e · sin(E)
		double mRad = eRad - (e * Math.Sin(a: eRad));
		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: mRad));
	}

	/// <summary>Calculates the warning time / travel time from the current true anomaly to a target true anomaly (e.g., node passage or closest approach).</summary>
	/// <param name="currentTrueAnomaly">The current true anomaly in degrees.</param>
	/// <param name="targetTrueAnomaly">The target true anomaly in degrees (e.g. 360° - ω for the ascending node).</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The remaining travel time in days.</returns>
	public static double CalculateTimeToTargetAnomalyDays(double currentTrueAnomaly, double targetTrueAnomaly, double semiMajorAxis, double numericalEccentricity)
	{
		// Convert current and target true anomalies to mean anomalies
		double currentM = CalculateMeanAnomalyFromTrueAnomaly(trueAnomaly: currentTrueAnomaly, numericalEccentricity: numericalEccentricity);
		double targetM = CalculateMeanAnomalyFromTrueAnomaly(trueAnomaly: targetTrueAnomaly, numericalEccentricity: numericalEccentricity);

		// Calculate angular distance in Mean Anomaly (0 to 360 degrees)
		double deltaM = NormalizeAngle360(degrees: targetM - currentM);

		// Orbital period in years (converted to days: 1 year ≈ 365.25 days)
		double periodYears = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		double periodDays = periodYears * 365.25;

		// Remaining time: (ΔM / 360°) * Period
		return deltaM / 360.0 * periodDays;
	}

	/// <summary>Calculates the warning times in days until the object reaches its ascending and descending ecliptic nodes.</summary>
	/// <param name="currentTrueAnomaly">The current true anomaly in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>A tuple containing (TimeToAscendingNodeDays, TimeToDescendingNodeDays).</returns>
	/// <remarks>The ascending node occurs at ν = 360° - ω, and the descending node at ν = 180° - ω.</remarks>
	public static (double AscendingNodeDays, double DescendingNodeDays) CalculateNodePassageWarningTimes(double currentTrueAnomaly, double argumentPerihelion, double semiMajorAxis, double numericalEccentricity)
	{
		// Ascending node: u = ω + ν = 0° => ν_asc = 360° - ω
		double targetNuAscending = NormalizeAngle360(degrees: 360.0 - argumentPerihelion);

		// Descending node: u = ω + ν = 180° => ν_desc = 180° - ω
		double targetNuDescending = NormalizeAngle360(degrees: 180.0 - argumentPerihelion);

		double timeToAscending = CalculateTimeToTargetAnomalyDays(currentTrueAnomaly: currentTrueAnomaly, targetTrueAnomaly: targetNuAscending, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		double timeToDescending = CalculateTimeToTargetAnomalyDays(currentTrueAnomaly: currentTrueAnomaly, targetTrueAnomaly: targetNuDescending, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		return (AscendingNodeDays: timeToAscending, DescendingNodeDays: timeToDescending);
	}

	/// <summary>Calculates the Julian Date of the perihelion passage prior to or near the reference epoch.</summary>
	/// <param name="meanAnomaly">The mean anomaly at epoch in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="epochJd">The reference epoch in Julian Days (TT).</param>
	/// <returns>The Julian Date (TT) of perihelion passage.</returns>
	/// <remarks>Calculated as T_p = EpochJD - (M / 360°) · Period · 365.25.</remarks>
	public static double CalculatePerihelionDate(double meanAnomaly, double semiMajorAxis, double epochJd)
	{
		return epochJd - (NormalizeAngle360(degrees: meanAnomaly) / 360.0 * CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) * 365.25);
	}

	/// <summary>Calculates the exact ecliptic longitude and latitude of perihelion (J2000.0).</summary>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>A tuple containing (LongitudeDegrees, LatitudeDegrees) on the ecliptic sphere.</returns>
	public static (double Longitude, double Latitude) CalculatePerihelionPosition(double inclination, double longitudeAscendingNode, double argumentPerihelion)
	{
		double i = double.DegreesToRadians(degrees: inclination);
		double w = double.DegreesToRadians(degrees: argumentPerihelion);
		double lon = NormalizeAngle360(degrees: longitudeAscendingNode + double.RadiansToDegrees(radians: Math.Atan2(y: Math.Cos(d: i) * Math.Sin(a: w), x: Math.Cos(d: w))));
		double lat = double.RadiansToDegrees(radians: Math.Asin(d: Math.Sin(a: i) * Math.Sin(a: w)));
		return (Longitude: lon, Latitude: lat);
	}

	/// <summary>Calculates the orbital velocity of a circular orbit at the given heliocentric distance.</summary>
	/// <param name="heliocentricDistance">The heliocentric distance in AU.</param>
	/// <returns>The circular orbital velocity in AU/year.</returns>
	/// <remarks>Calculated as v_c = sqrt(GM/r).</remarks>
	public static double CalculateCircularOrbitalVelocity(double heliocentricDistance)
	{
		// The circular velocity follows from v_c = sqrt(GM/r).
		return Math.Sqrt(d: gm / heliocentricDistance);
	}

	/// <summary>Calculates the escape velocity at the given heliocentric distance.</summary>
	/// <param name="heliocentricDistance">The heliocentric distance in AU.</param>
	/// <returns>The escape velocity in AU/year.</returns>
	/// <remarks>Calculated as v_esc = sqrt(2·GM/r) = sqrt(2)·v_c.</remarks>
	public static double CalculateHeliocentricEscapeVelocity(double heliocentricDistance)
	{
		// The escape velocity follows from v_esc = sqrt(2·GM/r).
		return Math.Sqrt(d: 2.0 * gm / heliocentricDistance);
	}

	/// <summary>Calculates the heliocentric distance at the ascending node.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>The distance at the ascending node in AU.</returns>
	/// <remarks>At the ascending node the argument of latitude is zero, so the true anomaly is -ω and r = p/(1 + e·cos ω).</remarks>
	public static double CalculateAscendingNodeDistance(double semiMajorAxis, double numericalEccentricity, double argumentPerihelion)
	{
		// The semi-latus rectum p = a(1 - e²) is needed for the conic equation.
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Convert the argument of perihelion from degrees to radians.
		double argumentPerihelionRad = double.DegreesToRadians(degrees: argumentPerihelion);
		// At the ascending node cos(ν) = cos(ω).
		return p / (1.0 + (numericalEccentricity * Math.Cos(d: argumentPerihelionRad)));
	}

	/// <summary>Calculates the heliocentric distance at the descending node.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>The distance at the descending node in AU.</returns>
	/// <remarks>At the descending node the argument of latitude is 180°, so the true anomaly is 180° - ω and r = p/(1 - e·cos ω).</remarks>
	public static double CalculateDescendingNodeDistance(double semiMajorAxis, double numericalEccentricity, double argumentPerihelion)
	{
		// The semi-latus rectum p = a(1 - e²) is needed for the conic equation.
		double p = CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Convert the argument of perihelion from degrees to radians.
		double argumentPerihelionRad = double.DegreesToRadians(degrees: argumentPerihelion);
		// At the descending node cos(ν) = -cos(ω).
		return p / (1.0 - (numericalEccentricity * Math.Cos(d: argumentPerihelionRad)));
	}

	/// <summary>Calculates the ecliptic longitude and latitude of the aphelion.</summary>
	/// <param name="inclination">The inclination to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>A tuple with the ecliptic longitude and latitude of the aphelion in degrees.</returns>
	/// <remarks>Uses the same projection as the perihelion, but with ω + 180°.</remarks>
	public static (double Longitude, double Latitude) CalculateAphelionPosition(double inclination, double longitudeAscendingNode, double argumentPerihelion)
	{
		// The aphelion lies opposite the perihelion, i.e. at ω + 180°.
		return CalculatePerihelionPosition(inclination: inclination, longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion + 180.0);
	}

	/// <summary>Calculates the Julian date of the next perihelion passage after the reference epoch.</summary>
	/// <param name="meanAnomaly">The mean anomaly at the reference epoch in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="epochJd">The Julian date of the reference epoch.</param>
	/// <returns>The Julian date (TT) of the next perihelion passage.</returns>
	/// <remarks>Adds one sidereal period to the epoch of the last perihelion passage.</remarks>
	public static double CalculateNextPerihelionDate(double meanAnomaly, double semiMajorAxis, double epochJd)
	{
		// The next perihelion follows one full period after the previous one.
		return CalculatePerihelionDate(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis, epochJd: epochJd) + (CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) * DaysPerJulianYear);
	}

	/// <summary>Calculates the synodic period of a planetoid with each of the eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>A list of synodic periods, one per planet, in order from Mercury to Neptune.</returns>
	/// <remarks>Uses T_syn = 1/|1/T - 1/T_planet|. The value is infinite when both periods are equal.</remarks>
	public static List<SynodicPeriod> CalculateSynodicPeriods(double semiMajorAxis)
	{
		// Calculate the sidereal period of the planetoid once.
		double period = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		// Prepare one result entry per planet.
		List<SynodicPeriod> results = new(capacity: SolarSystemPlanets.Length);
		// Compare the planetoid with each planet of the solar system.
		foreach ((string planetName, double planetPeriod) in SolarSystemPlanets)
		{
			// The synodic period follows from the difference of the mean motions.
			double difference = Math.Abs(value: (1.0 / period) - (1.0 / planetPeriod));
			double synodicPeriod = difference == 0 ? double.PositiveInfinity : 1.0 / difference;
			results.Add(item: new SynodicPeriod(PlanetName: planetName, PlanetPeriodYears: planetPeriod, SynodicPeriodYears: synodicPeriod));
		}
		return results;
	}

	/// <summary>Returns a string representation of the orbital resonance for debugging purposes.</summary>
	/// <returns>A string representation of the orbital resonance.</returns>
	/// <remarks>This property is used to provide a human-readable representation of the orbital resonance for debugging.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}