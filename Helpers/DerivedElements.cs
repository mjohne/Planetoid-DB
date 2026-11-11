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
		(Name: "Mercury", Period:   0.240846),
		(Name: "Venus",   Period:   0.615197),
		(Name: "Earth",   Period:   1.000000),
		(Name: "Mars",    Period:   1.880848),
		(Name: "Jupiter", Period:  11.862615),
		(Name: "Saturn",  Period:  29.447498),
		(Name: "Uranus",  Period:  84.016846),
		(Name: "Neptune", Period: 164.791320)
	];

	/// <summary>Mean tropical year length, in days.</summary>
	private const double siderealEarthYearInDays = 365.242188;

	/// <summary>Represents the number of days in a Julian year.</summary>
	/// <remarks>The Julian year of 365.25 days is used for Julian date arithmetic and mean motion conversions.</remarks>
	private const double DaysPerJulianYear = 365.25;

	#region Helpers

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

	/// <summary>Returns a string representation of the orbital resonance for debugging purposes.</summary>
	/// <returns>A string representation of the orbital resonance.</returns>
	/// <remarks>This property is used to provide a human-readable representation of the orbital resonance for debugging.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	#endregion

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

	/// <summary>Calculates the orbital semi-perimeter of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The orbital semi-perimeter of the ellipse.</returns>
	/// <remarks>This method is used to calculate the orbital semi-perimeter of an ellipse.</remarks>
	public static double CalculateOrbitalSemiPerimeter(double semiMajorAxis, double numericalEccentricity)
	{
		// The semi-perimeter of an ellipse is half of the perimeter, so we can use the CalculateOrbitalPerimeter method and divide the result by 2.
		return CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / 2.0;
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

	/// <summary>Calculates the orbital arc length from the ascending node through perihelion to the descending node.</summary>
	/// <param name="semiMajorAxis">The orbit's semi-major axis.</param>
	/// <param name="numericalEccentricity">The orbit's numerical eccentricity.</param>
	/// <param name="argumentOfPerihelion">The argument of perihelion.</param>
	/// <returns>The arc length in astronomical units.</returns>
	/// <remarks>This method calculates the length of the arc from the ascending node to the perihelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double CalculateLengthOfPerihelionNodeArc(double semiMajorAxis, double numericalEccentricity, double argumentOfPerihelion)
	{
		// This method calculates the length of the arc from the ascending node to the perihelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.
		return OrbitalArcCalculator.AscendingNodePerihelionDescendingNodeArcLengthAU(semiMajorAxis: semiMajorAxis, eccentricity: numericalEccentricity, argumentOfPerihelion: argumentOfPerihelion);
	}

	/// <summary>Calculates the orbital arc length from the ascending node through aphelion to the descending node.</summary>
	/// <param name="semiMajorAxis">The orbit's semi-major axis.</param>
	/// <param name="numericalEccentricity">The orbit's numerical eccentricity.</param>
	/// <param name="argumentOfPerihelion">The argument of perihelion.</param>
	/// <returns>The arc length in astronomical units.</returns>
	/// <remarks>This method calculates the length of the arc from the ascending node to the aphelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double CalculateLengthOfAphelionNodeArc(double semiMajorAxis, double numericalEccentricity, double argumentOfPerihelion)
	{
		// This method calculates the length of the arc from the ascending node to the aphelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.
		return OrbitalArcCalculator.AscendingNodeAphelionDescendingNodeArcLengthAU(semiMajorAxis: semiMajorAxis, eccentricity: numericalEccentricity, argumentOfPerihelion: argumentOfPerihelion);
	}

	/// <summary>Calculates the angular arc from the ascending node through perihelion to the descending node.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="argumentOfPerihelion">The argument of perihelion.</param>
	/// <returns>The angular arc from the ascending node through perihelion to the descending node.</returns>
	/// <remarks>This method calculates the angular arc from the ascending node through perihelion to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double CalculateAngularOfPerihelionNodeArc(double semiMajorAxis, double numericalEccentricity, double argumentOfPerihelion)
	{
		return OrbitalArcCalculator.AscendingNodePerihelionDescendingNodeArcAngle(semiMajorAxis: semiMajorAxis, eccentricity: numericalEccentricity, argumentOfPerihelion: argumentOfPerihelion);
	}

	/// <summary>Calculates the angular arc from the ascending node through aphelion to the descending node.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the orbit.</param>
	/// <param name="argumentOfPerihelion">The argument of perihelion.</param>
	/// <returns>The angular arc from the ascending node through aphelion to the descending node.</returns>
	/// <remarks>This method calculates the angular arc from the ascending node through aphelion to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double CalculateAngularOfAphelionNodeArc(double semiMajorAxis, double numericalEccentricity, double argumentOfPerihelion)
	{
		return OrbitalArcCalculator.AscendingNodeAphelionDescendingNodeArcAngle(semiMajorAxis: semiMajorAxis, eccentricity: numericalEccentricity, argumentOfPerihelion: argumentOfPerihelion);
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
		// Convert the inclination from degrees to radians for the calculation.
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		// Calculate the Tisserand parameter using the formula: T = a_main/a + 2·cos(i)·sqrt(a(1-e²)/a_main).
		double term1 = semiMajorAxisMain / semiMajorAxis;
		// Calculate the second term of the Tisserand parameter formula.
		double term2 = 2.0 * Math.Cos(d: inclinationRad) * Math.Sqrt(d: semiMajorAxis * (1.0 - (numericalEccentricity * numericalEccentricity)) / semiMajorAxisMain);
		// Return the sum of the two terms as the Tisserand parameter.
		return term1 + term2;
	}

	/// <summary>Calculates the Tisserand parameter with respect to Mercury.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Mercury (dimensionless).</returns>
	public static double CalculateMercuryTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Mercury in astronomical units (AU).
		const double mercurySemiMajorAxis = 0.38709893;
		// Call the general Tisserand parameter calculation method with Mercury's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: mercurySemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Venus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Venus (dimensionless).</returns>
	public static double CalculateVenusTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Venus in astronomical units (AU).
		const double venusSemiMajorAxis = 0.72333199;
		// Call the general Tisserand parameter calculation method with Venus's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: venusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Earth (dimensionless).</returns>
	public static double CalculateEarthTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Earth in astronomical units (AU).
		const double earthSemiMajorAxis = 1.00000011;
		// Call the general Tisserand parameter calculation method with Earth's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: earthSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Mars.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Mars (dimensionless).</returns>
	public static double CalculateMarsTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Mars in astronomical units (AU).
		const double marsSemiMajorAxis = 1.52366231;
		// Call the general Tisserand parameter calculation method with Mars's semi-major axis and the provided parameters.
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
		// The semi-major axis of Jupiter in astronomical units (AU).
		const double jupiterSemiMajorAxis = 5.20336301;
		// Call the general Tisserand parameter calculation method with Jupiter's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: jupiterSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Saturn.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Saturn (dimensionless).</returns>
	public static double CalculateSaturnTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Saturn in astronomical units (AU).
		const double saturnSemiMajorAxis = 9.53707032;
		// Call the general Tisserand parameter calculation method with Saturn's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: saturnSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Uranus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Uranus (dimensionless).</returns>
	public static double CalculateUranusTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Uranus in astronomical units (AU).
		const double uranusSemiMajorAxis = 19.19126393;
		// Call the general Tisserand parameter calculation method with Uranus's semi-major axis and the provided parameters.
		return CalculateTisserandParameter(semiMajorAxisMain: uranusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates the Tisserand parameter with respect to Neptune.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The Tisserand parameter relative to Neptune (dimensionless).</returns>
	public static double CalculateNeptuneTisserandParameter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Neptune in astronomical units (AU).
		const double neptuneSemiMajorAxis = 30.06896348;
		// Call the general Tisserand parameter calculation method with Neptune's semi-major axis and the provided parameters.
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
		// Calculate the Tisserand parameter for the encounter body with respect to the main body.
		return Math.Sqrt(d: 3.0 - CalculateTisserandParameter(semiMajorAxisMain: semiMajorAxisMain, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination));
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Mercury.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Mercury.</returns>
	public static double CalculateEncounterVelocityRelativeToMercury(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Mercury in astronomical units (AU).
		const double mercurySemiMajorAxis = 0.38709893;
		// Call the general encounter velocity calculation method with Mercury's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: mercurySemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Venus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Venus.</returns>
	public static double CalculateEncounterVelocityRelativeToVenus(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Venus in astronomical units (AU).
		const double venusSemiMajorAxis = 0.72333199;
		// Call the general encounter velocity calculation method with Venus's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: venusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Earth.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Earth.</returns>
	public static double CalculateEncounterVelocityRelativeToEarth(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Earth in astronomical units (AU).
		const double earthSemiMajorAxis = 1.00000011;
		// Call the general encounter velocity calculation method with Earth's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: earthSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Mars.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Mars.</returns>
	public static double CalculateEncounterVelocityRelativeToMars(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Mars in astronomical units (AU).
		const double marsSemiMajorAxis = 1.52366231;
		// Call the general encounter velocity calculation method with Mars's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: marsSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Jupiter.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Jupiter.</returns>
	public static double CalculateEncounterVelocityRelativeToJupiter(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Jupiter in astronomical units (AU).
		const double jupiterSemiMajorAxis = 5.20336301;
		// Call the general encounter velocity calculation method with Jupiter's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: jupiterSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Saturn.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Saturn.</returns>
	public static double CalculateEncounterVelocityRelativeToSaturn(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Saturn in astronomical units (AU).
		const double saturnSemiMajorAxis = 9.53707032;
		// Call the general encounter velocity calculation method with Saturn's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: saturnSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Uranus.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Uranus.</returns>
	public static double CalculateEncounterVelocityRelativeToUranus(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Uranus in astronomical units (AU).
		const double uranusSemiMajorAxis = 19.19126393;
		// Call the general encounter velocity calculation method with Uranus's semi-major axis and the provided parameters.
		return CalculateEncounterVelocity(semiMajorAxisMain: uranusSemiMajorAxis, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination);
	}

	/// <summary>Calculates Öpik's encounter velocity relative to Neptune.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The dimensionless encounter velocity relative to Neptune.</returns>
	public static double CalculateEncounterVelocityRelativeToNeptune(double semiMajorAxis, double numericalEccentricity, double inclination)
	{
		// The semi-major axis of Neptune in astronomical units (AU).
		const double neptuneSemiMajorAxis = 30.06896348;
		// Call the general encounter velocity calculation method with Neptune's semi-major axis and the provided parameters.
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
		// Convert the orbital inclination from degrees to radians for the calculation.
		double inclinationRad = double.DegreesToRadians(degrees: orbitalInclination);
		// Calculate the Kozai quantity using the formula: sqrt(1 - e²) · cos(i), where e is the numerical eccentricity and i is the orbital inclination.
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
		// Convert the argument of perihelion and the longitude of the ascending node from degrees to radians for the calculation.
		double argumentPerihelionRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: argumentPerihelion));
		// Convert the longitude of the ascending node from degrees to radians for the calculation.
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		// Calculate the equinoctial orbital element h using the formula: h = e · sin(ϖ), where ϖ = ω + Ω is the longitude of perihelion.
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
		// Convert the argument of perihelion and the longitude of the ascending node from degrees to radians for the calculation.
		double argumentPerihelionRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: argumentPerihelion));
		// Convert the longitude of the ascending node from degrees to radians for the calculation.
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		// Calculate the equinoctial orbital element k using the formula: k = e · cos(ϖ), where ϖ = ω + Ω is the longitude of perihelion.
		return numericalEccentricity * Math.Cos(d: argumentPerihelionRad + lengthAscendingNodeRad);
	}

	/// <summary>Calculates the equinoctial orbital element p.</summary>
	/// <param name="orbitalInclination">The orbital inclination in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element p (dimensionless).</returns>
	/// <remarks>Calculated as p = tan(i / 2) · sin(Ω). Avoids singularities for zero eccentricity.</remarks>
	public static double CalculateEquinoctialOrbitalElementP(double orbitalInclination, double lengthAscendingNode)
	{
		// Convert the orbital inclination and the longitude of the ascending node from degrees to radians for the calculation.
		double orbitalInclinationRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: orbitalInclination));
		// Convert the longitude of the ascending node from degrees to radians for the calculation.
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		// Calculate the equinoctial orbital element p using the formula: p = tan(i / 2) · sin(Ω), where i is the orbital inclination and Ω is the longitude of the ascending node.
		return Math.Tan(a: orbitalInclinationRad / 2.0) * Math.Sin(a: lengthAscendingNodeRad);
	}

	/// <summary>Calculates the equinoctial orbital element q.</summary>
	/// <param name="orbitalInclination">The orbital inclination in degrees.</param>
	/// <param name="lengthAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <returns>The equinoctial element q (dimensionless).</returns>
	/// <remarks>Calculated as q = tan(i / 2) · cos(Ω). Avoids singularities for zero eccentricity.</remarks>
	public static double CalculateEquinoctialOrbitalElementQ(double orbitalInclination, double lengthAscendingNode)
	{
		// Convert the orbital inclination and the longitude of the ascending node from degrees to radians for the calculation.
		double orbitalInclinationRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: orbitalInclination));
		// Convert the longitude of the ascending node from degrees to radians for the calculation.
		double lengthAscendingNodeRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: lengthAscendingNode));
		// Calculate the equinoctial orbital element q using the formula: q = tan(i / 2) · cos(Ω), where i is the orbital inclination and Ω is the longitude of the ascending node.
		return Math.Tan(a: orbitalInclinationRad / 2.0) * Math.Cos(d: lengthAscendingNodeRad);
	}

	/// <summary>Calculates the mean motion n in degrees per year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean motion in deg/year.</returns>
	/// <remarks>Calculated as n = 360° / T.</remarks>
	public static double CalculateMeanMotion(double semiMajorAxis)
	{
		// The mean motion can be calculated by dividing 360 degrees by the sidereal orbital period of the object, which is determined by its semi-major axis.
		return 360.0 / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the mean motion n in radians per year.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean motion in rad/year.</returns>
	/// <remarks>Calculated as n = 2π / T.</remarks>
	public static double CalculateMeanMotionRad(double semiMajorAxis)
	{
		// The mean motion in radians per year can be calculated by dividing 2π by the sidereal orbital period of the object, which is determined by its semi-major axis.
		return 2.0 * Math.PI / CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
	}

	/// <summary>Calculates the mean daily motion n in degrees per day.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The mean daily motion in deg/day.</returns>
	/// <remarks>Calculated as n_day = (360° / T) / 365.242188.</remarks>
	public static double CalculateMeanDailyMotion(double semiMajorAxis)
	{
		// The mean daily motion can be calculated by dividing the mean motion (in degrees per year) by the sidereal Earth year in days (365.242188).
		return CalculateMeanMotion(semiMajorAxis: semiMajorAxis) / siderealEarthYearInDays;
	}

	// <summary>Calculates the mean daily orbital motion of an ellipse.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the ellipse in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity of the ellipse.</param>
	/// <returns>The mean daily orbital motion in AU/days.</returns>
	/// <remarks>Calculated using the formula = p/365.242188, where p is the orbital perimenter and 365.242188 is the sidereal orbital period in 365 days, 5 hours, 48 minutes and 45 seconds.</remarks>
	public static double CalculateMeanDailyOrbitalMotion(double semiMajorAxis, double numericalEccentricity)
	{
		// The mean daily orbital motion can be calculated by dividing the orbital perimeter by the sidereal Earth year in days (365.242188).
		return CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / siderealEarthYearInDays;
	}

	/// <summary>Calculates the instantaneous angular velocity dν/dt at a given true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <returns>The instantaneous angular velocity in rad/year.</returns>
	/// <remarks>Calculated as ω = h / r².</remarks>
	public static double CalculateAngularVelocity(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		// Calculate the current distance from the focus (r) at the given true anomaly using the semi-major axis and numerical eccentricity.
		double r = CalculateCurrentDistanceFromFocus(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly);
		// Calculate the specific angular momentum (h) using the semi-major axis and numerical eccentricity.
		double h = CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Calculate the instantaneous angular velocity (ω) using the formula: ω = h / r², where h is the specific angular momentum and r is the current distance from the focus.
		return h / (r * r);
	}

	/// <summary>Calculates the areal velocity dA/dt along the orbit.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The areal velocity in AU²/year.</returns>
	/// <remarks>Calculated as dA/dt = h / 2. Follows Kepler's second law (constant rate of area swept per time unit).</remarks>
	public static double CalculateArealVelocity(double semiMajorAxis, double numericalEccentricity)
	{
		// The areal velocity can be calculated as half of the specific angular momentum (h) of the orbit, which is determined by the semi-major axis and numerical eccentricity.
		return CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) / 2.0;
	}

	/// <summary>Calculates the maximum height z_max above the ecliptic plane.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="inclination">The orbital inclination in degrees.</param>
	/// <returns>The maximum distance from the ecliptic plane in AU.</returns>
	/// <remarks>Simplified as z_max = a · sin(i).</remarks>
	public static double CalculateMaxEclipticDistance(double semiMajorAxis, double inclination)
	{
		// Convert the inclination from degrees to radians for the calculation.
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		// The maximum height above the ecliptic plane can be calculated as the product of the semi-major axis and the sine of the inclination.
		return semiMajorAxis * Math.Sin(a: inclinationRad);
	}

	/// <summary>Calculates the heliocentric latitude b for a given orbital position.</summary>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <returns>The heliocentric latitude in degrees in the range [-90°, 90°].</returns>
	/// <remarks>Calculated as b = arcsin(sin(i) · sin(ω + ν)).</remarks>
	public static double CalculateHeliocentricLatitude(double argumentPerihelion, double trueAnomaly, double inclination)
	{
		// Convert the inclination, argument of perihelion, and true anomaly from degrees to radians for the calculation.
		double inclinationRad = double.DegreesToRadians(degrees: inclination);
		// Calculate the sum of the argument of perihelion and true anomaly, then convert it to radians.
		double uRad = double.DegreesToRadians(degrees: argumentPerihelion + trueAnomaly);
		// The heliocentric latitude can be calculated as the arcsine of the product of the sine of the inclination and the sine of the sum of the argument of perihelion and true anomaly.
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
		// Calculate the argument of latitude u = ω + ν.
		double argumentOfLatitude = argumentPerihelion + trueAnomaly;
		// The true longitude L is then calculated as the sum of the longitude of the ascending node and the argument of latitude, normalized to the range [0, 360°).
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
		// Convert the longitude of the ascending node, argument of perihelion, true anomaly, and inclination from degrees to radians for the calculation.
		double OmegaRad = double.DegreesToRadians(degrees: longitudeAscendingNode);
		double uRad = double.DegreesToRadians(degrees: argumentPerihelion + trueAnomaly);
		double iRad = double.DegreesToRadians(degrees: inclination);
		// Calculate the longitude offset using the arctangent of the sine of the argument of latitude multiplied by the cosine of the inclination, divided by the cosine of the argument of latitude.
		double lonOffset = Math.Atan2(Math.Sin(uRad) * Math.Cos(d: iRad), Math.Cos(d: uRad));
		// The heliocentric ecliptic longitude λ is then calculated as the sum of the longitude of the ascending node and the longitude offset, normalized to the range [0, 360°).
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
		// Calculate the argument of latitude u = ω + ν.
		double u = omega + nu;
		// Calculate the current distance from the focus (r) at the given true anomaly (nu) using the semi-major axis (a) and numerical eccentricity (e).
		double r = a * (1 - (e * e)) / (1 + (e * Math.Cos(d: double.DegreesToRadians(degrees: nu))));
		// Convert the longitude of the ascending node (Omega), argument of latitude (u), and inclination (i) from degrees to radians for the calculation.
		double OmegaRad = double.DegreesToRadians(degrees: Omega);
		double uRad = double.DegreesToRadians(degrees: u);
		double iRad = double.DegreesToRadians(degrees: i);
		// Calculate the Cartesian coordinates (x, y) in the ecliptic plane using the orbital elements and the current distance (r).
		double x = r * ((Math.Cos(d: OmegaRad) * Math.Cos(d: uRad)) - (Math.Sin(a: OmegaRad) * Math.Sin(a: uRad) * Math.Cos(d: iRad)));
		double y = r * ((Math.Sin(a: OmegaRad) * Math.Cos(d: uRad)) + (Math.Cos(d: OmegaRad) * Math.Sin(a: uRad) * Math.Cos(d: iRad)));
		// Calculate the heliocentric ecliptic longitude λ using the arctangent of y/x, converting the result from radians to degrees and normalizing it to the range [0, 360°).
		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: Math.Atan2(y: y, x: x)));
	}

	/// <summary>Calculates the characteristic energy C3 for interplanetary trajectory analysis.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>The characteristic energy C3 in AU²/year².</returns>
	/// <remarks>Calculated as C3 = -GM / a = 2 · ε (twice the specific orbital energy).</remarks>
	public static double CalculateCharacteristicEnergy(double semiMajorAxis)
	{
		// The characteristic energy C3 can be calculated as -GM / a, where GM is the standard gravitational parameter of the Sun and a is the semi-major axis of the orbit.
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
		// Return the radius of curvature R_c(ν) = p / (factor1 * factor2)
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
		// If the satellite density is zero or negative, return positive infinity to indicate an undefined Roche limit.
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
		// If the satellite density is zero or negative, return positive infinity to indicate an undefined Roche limit.
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
	/// <remarks>Values are approximate and based on observational data for common asteroid types.</remarks>
	public static double GetTypicalAsteroidDensity(AsteroidType type)
	{
		// Return typical mean densities for common asteroid types in g/cm³. Values are approximate and based on observational data.
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
		// Validate input parameters: diameter and density must be positive values.
		if (diameterKm <= 0.0 || densityGPerCm3 <= 0.0)
		{
			return 0.0;
		}
		// Convert radius to meters: R = (diameter / 2) * 1000
		double radiusMeters = diameterKm / 2.0 * 1000.0;
		// Convert density from g/cm³ to kg/m³: 1 g/cm³ = 1000 kg/m³
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		// Volume of a sphere: mpcorbDatReadableDesignationDelimiterBegin = (4/3) * π * r³
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
		// Get the typical density for the specified asteroid type in g/cm³.
		double density = GetTypicalAsteroidDensity(type: type);
		// Calculate the mass using the diameter and the typical density.
		return CalculateMassFromDiameter(diameterKm: diameterKm, densityGPerCm3: density);
	}

	/// <summary>Calculates the surface gravity of a spherical body.</summary>
	/// <param name="massKg">Mass in kilograms.</param>
	/// <param name="radiusKm">Mean radius in kilometers.</param>
	/// <returns>Surface gravity in m/s².</returns>
	/// <remarks>Calculated as g = G · m / r².</remarks>
	public static double CalculateSurfaceGravity(double massKg, double radiusKm)
	{
		// Validate input parameters: radius must be positive to avoid division by zero.
		if (radiusKm <= 0.0)
		{
			return 0.0;
		}
		// Gravitational constant in m³ kg⁻¹ s⁻²
		const double gravitationalConstant = 6.67430e-11;
		// Convert radius from kilometers to meters for the calculation.
		double radiusMeters = radiusKm * 1000.0;
		// Calculate surface gravity using the formula: g = G * m / r², where G is the gravitational constant, m is the mass, and r is the radius.
		return gravitationalConstant * massKg / (radiusMeters * radiusMeters);
	}

	/// <summary>Calculates the escape velocity from the surface of a spherical body.</summary>
	/// <param name="massKg">Mass in kilograms.</param>
	/// <param name="radiusKm">Mean radius in kilometers.</param>
	/// <returns>Escape velocity in m/s.</returns>
	/// <remarks>Calculated as v_esc = sqrt(2 · G · m / r).</remarks>
	public static double CalculateEscapeVelocity(double massKg, double radiusKm)
	{
		// Validate input parameters: radius must be positive to avoid division by zero.
		if (radiusKm <= 0.0)
		{
			return 0.0;
		}
		// Gravitational constant in m³ kg⁻¹ s⁻²
		const double gravitationalConstant = 6.67430e-11;
		// Convert radius from kilometers to meters for the calculation.
		double radiusMeters = radiusKm * 1000.0;
		// Calculate escape velocity using the formula: v_esc = sqrt(2 * G * m / r), where G is the gravitational constant, m is the mass, and r is the radius.
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
		// Check if the Earth MOID is within the critical threshold (0.0 to 0.05 AU)
		bool isMoidCritical = earthMoidAu is >= 0.0 and <= moidThresholdAu;
		// Check if the absolute magnitude H is within the critical threshold (H <= 22.0)
		bool isMagnitudeCritical = absoluteMagnitudeH <= magnitudeThresholdH;
		// Is classified as PHA if BOTH criteria are met
		bool isPha = isMoidCritical && isMagnitudeCritical;
		// Calculate estimated diameter in meters at the H = 22.0 threshold using geometric albedo = 0.14
		double estimatedMinDiameterMeters = CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: absoluteMagnitudeH, geometricAlbedo: 0.14) * 1000.0;
		// Return a PhaEvaluation structure containing the results of the evaluation
		return new PhaEvaluation(IsPha: isPha, IsMoidCritical: isMoidCritical, IsMagnitudeCritical: isMagnitudeCritical, EstimatedMinDiameterMeters: estimatedMinDiameterMeters);
	}

	/// <summary>Simple helper to check if an object is a Potentially Hazardous Asteroid (PHA).</summary>
	/// <param name="earthMoidAu">The Minimum Orbit Intersection Distance (MOID) to Earth in AU.</param>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag.</param>
	/// <returns>True if the object is a PHA; otherwise, false.</returns>
	public static bool IsPotentiallyHazardous(double earthMoidAu, double absoluteMagnitudeH)
	{
		// A PHA is defined as having an Earth MOID of 0.05 AU or less and an absolute magnitude H of 22.0 or less.
		return earthMoidAu >= 0.0 && earthMoidAu <= 0.05 && absoluteMagnitudeH <= 22.0;
	}

	/// <summary>Calculates the eccentric anomaly E (in degrees) from a given true anomaly ν (in degrees).</summary>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The eccentric anomaly in degrees in the range [0, 360).</returns>
	public static double CalculateEccentricAnomalyFromTrueAnomaly(double trueAnomaly, double numericalEccentricity)
	{
		// Convert the true anomaly from degrees to radians for the calculation.
		double nuRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: trueAnomaly));
		// Store the numerical eccentricity for use in the calculation.
		double e = numericalEccentricity;
		// Calculate the factor used in the transformation from true anomaly to eccentric anomaly.		
		double factor = Math.Sqrt(d: (1.0 - e) / (1.0 + e));
		// Calculate the tangent of half the eccentric anomaly using the formula: tan(E/2) = sqrt((1 - e) / (1 + e)) * tan(ν/2).
		double tanHalfE = factor * Math.Tan(a: nuRad / 2.0);
		// Calculate the eccentric anomaly E in radians by taking the arctangent of the tangent of half the eccentric anomaly and multiplying by 2.
		double eRad = 2.0 * Math.Atan(d: tanHalfE);
		// Convert the eccentric anomaly from radians to degrees and normalize it to the range [0, 360°).
		return NormalizeAngle360(degrees: double.RadiansToDegrees(radians: eRad));
	}

	/// <summary>Calculates the mean anomaly M (in degrees) from a given true anomaly ν (in degrees).</summary>
	/// <param name="trueAnomaly">The true anomaly in degrees.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity.</param>
	/// <returns>The mean anomaly in degrees in the range [0, 360).</returns>
	public static double CalculateMeanAnomalyFromTrueAnomaly(double trueAnomaly, double numericalEccentricity)
	{
		// First, calculate the eccentric anomaly E from the true anomaly ν using the provided numerical eccentricity.
		double eDeg = CalculateEccentricAnomalyFromTrueAnomaly(trueAnomaly: trueAnomaly, numericalEccentricity: numericalEccentricity);
		// Convert the eccentric anomaly from degrees to radians for the calculation.
		double eRad = double.DegreesToRadians(degrees: eDeg);
		// Store the numerical eccentricity for use in the calculation.
		double e = numericalEccentricity;
		// Calculate the mean anomaly M in radians using the formula: M = E - e * sin(E).
		double mRad = eRad - (e * Math.Sin(a: eRad));
		// Convert the mean anomaly from radians to degrees and normalize it to the range [0, 360°).
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
		// Convert period to days
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
		// Calculate the time to reach each node from the current true anomaly
		double timeToAscending = CalculateTimeToTargetAnomalyDays(currentTrueAnomaly: currentTrueAnomaly, targetTrueAnomaly: targetNuAscending, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Calculate the time to reach the descending node
		double timeToDescending = CalculateTimeToTargetAnomalyDays(currentTrueAnomaly: currentTrueAnomaly, targetTrueAnomaly: targetNuDescending, semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Return the results as a tuple
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
		// Calculate the Julian Date of perihelion passage using the formula: T_p = EpochJD - (M / 360°) * Period * 365.25, where M is the mean anomaly in degrees, and Period is the sidereal orbital period in years.
		return epochJd - (NormalizeAngle360(degrees: meanAnomaly) / 360.0 * CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) * 365.25);
	}

	/// <summary>Calculates the Julian Date of the aphelion passage prior to or near the reference epoch.</summary>
	/// <param name="meanAnomaly">The mean anomaly at epoch in degrees.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="epochJd">The reference epoch in Julian Days (TT).</param>
	/// <returns>The Julian Date (TT) of aphelion passage.</returns>
	/// <remarks>Calculated as T_a = EpochJD - ((M - 180°) / 360°) · Period · 365.25.</remarks>
	public static double CalculateAphelionDate(double meanAnomaly, double semiMajorAxis, double epochJd)
	{
		// Calculate the Julian Date of aphelion passage using the formula: T_a = EpochJD - ((M - 180°) / 360°) * Period * 365.25, where M is the mean anomaly in degrees, and Period is the sidereal orbital period in years.
		return epochJd - (NormalizeAngle360(degrees: meanAnomaly - 180.0) / 360.0 * CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis) * 365.25);
	}

	/// <summary>Calculates the exact ecliptic longitude and latitude of perihelion (J2000.0).</summary>
	/// <param name="inclination">The inclination in degrees.</param>
	/// <param name="longitudeAscendingNode">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelion">The argument of perihelion in degrees.</param>
	/// <returns>A tuple containing (LongitudeDegrees, LatitudeDegrees) on the ecliptic sphere.</returns>
	public static (double Longitude, double Latitude) CalculatePerihelionPosition(double inclination, double longitudeAscendingNode, double argumentPerihelion)
	{
		// Convert the inclination and argument of perihelion from degrees to radians for the calculation.
		double i = double.DegreesToRadians(degrees: inclination);
		double w = double.DegreesToRadians(degrees: argumentPerihelion);
		// Calculate the ecliptic longitude and latitude of perihelion using the formulas:
		double lon = NormalizeAngle360(degrees: longitudeAscendingNode + double.RadiansToDegrees(radians: Math.Atan2(y: Math.Cos(d: i) * Math.Sin(a: w), x: Math.Cos(d: w))));
		double lat = double.RadiansToDegrees(radians: Math.Asin(d: Math.Sin(a: i) * Math.Sin(a: w)));
		// Return the calculated longitude and latitude as a tuple.
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

	#region Apparent magnitude

	/// <summary>Calculates the apparent visual magnitude (m) of an asteroid using the IAU H-G magnitude system.</summary>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag.</param>
	/// <param name="slopeParameterG">The slope parameter G (typically ~0.15 for unknown surface properties).</param>
	/// <param name="heliocentricDistance">The distance from the Sun r in AU.</param>
	/// <param name="geocentricDistance">The distance from the Earth Δ in AU.</param>
	/// <param name="phaseAngle">The Sun-asteroid-Earth phase angle α in degrees (default is 0.0).</param>
	/// <returns>The calculated apparent magnitude in mag. Returns double.NaN if distances are non-positive.</returns>
	/// <remarks>Calculated using the IAU H-G magnitude system (Bowell et al., 1989): m = H + 5·log10(r·Δ) - 2.5·log10((1-G)·Φ₁ + G·Φ₂).</remarks>
	public static double CalculateApparentMagnitude(double absoluteMagnitudeH, double slopeParameterG, double heliocentricDistance, double geocentricDistance, double phaseAngle = 0.0)
	{
		// Validate input parameters: distances must be strictly positive.
		if (heliocentricDistance <= 0.0 || geocentricDistance <= 0.0)
		{
			return double.NaN;
		}
		// Convert phase angle α from degrees to radians and normalize
		double alphaRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: phaseAngle));
		double halfAlpha = alphaRad / 2.0;
		double tanHalfAlpha = Math.Tan(a: halfAlpha);
		// Calculate IAU standard H-G phase functions Φ₁ and Φ₂ (Bowell et al., 1989)
		double phi1 = Math.Exp(d: -3.33 * Math.Pow(x: tanHalfAlpha, y: 0.63));
		double phi2 = Math.Exp(d: -1.87 * Math.Pow(x: tanHalfAlpha, y: 1.22));
		// Combined phase integral Φ(α) = (1 - G)·Φ₁ + G·Φ₂
		double phaseFunction = ((1.0 - slopeParameterG) * phi1) + (slopeParameterG * phi2);
		// Apparent magnitude formula: m = H + 5·log10(r·Δ) - 2.5·log10(Φ(α))
		return absoluteMagnitudeH + (5.0 * Math.Log10(d: heliocentricDistance * geocentricDistance)) - (2.5 * Math.Log10(d: phaseFunction));
	}

	/// <summary>Calculates the apparent visual magnitude (m) assuming a standard slope parameter G = 0.15 and zero phase angle.</summary>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag.</param>
	/// <param name="heliocentricDistance">The distance from the Sun r in AU.</param>
	/// <param name="geocentricDistance">The distance from the Earth Δ in AU.</param>
	/// <returns>The calculated apparent magnitude in mag.</returns>
	public static double CalculateApparentMagnitude(double absoluteMagnitudeH, double heliocentricDistance, double geocentricDistance)
	{
		return CalculateApparentMagnitude(absoluteMagnitudeH: absoluteMagnitudeH, slopeParameterG: 0.15, heliocentricDistance: heliocentricDistance, geocentricDistance: geocentricDistance, phaseAngle: 0.0);
	}

	#endregion

	#region Geocentric distance

	/// <summary>Calculates the geocentric distance (Δ) from 3D heliocentric Cartesian coordinates.</summary>
	/// <param name="x">The heliocentric X position of the object in AU.</param>
	/// <param name="y">The heliocentric Y position of the object in AU.</param>
	/// <param name="z">The heliocentric Z position of the object in AU.</param>
	/// <param name="earthX">The heliocentric X position of Earth in AU.</param>
	/// <param name="earthY">The heliocentric Y position of Earth in AU.</param>
	/// <param name="earthZ">The heliocentric Z position of Earth in AU.</param>
	/// <returns>The geocentric distance Δ in AU.</returns>
	/// <remarks>Calculated as Δ = √((x - x_E)² + (y - y_E)² + (z - z_E)²).</remarks>
	public static double CalculateGeocentricDistance(double x, double y, double z, double earthX, double earthY, double earthZ)
	{
		// Calculate the difference vector between the object and Earth
		double dx = x - earthX;
		double dy = y - earthY;
		double dz = z - earthZ;
		// The geocentric distance (Δ) is the Euclidean norm of the difference vector.
		return Math.Sqrt(d: (dx * dx) + (dy * dy) + (dz * dz));
	}

	/// <summary>Calculates the geocentric distance (Δ) from heliocentric ecliptic spherical coordinates.</summary>
	/// <param name="heliocentricDistance">The distance of the object from the Sun r in AU.</param>
	/// <param name="heliocentricLongitude">The heliocentric ecliptic longitude λ in degrees.</param>
	/// <param name="heliocentricLatitude">The heliocentric ecliptic latitude b in degrees.</param>
	/// <param name="earthHeliocentricLongitude">The heliocentric ecliptic longitude of Earth λ_E in degrees.</param>
	/// <param name="earthHeliocentricDistance">The distance of Earth from the Sun R in AU (default is 1.00000011 AU).</param>
	/// <returns>The geocentric distance Δ in AU.</returns>
	/// <remarks>Converts ecliptic spherical coordinates to Cartesian coordinates and calculates the 3D distance to Earth.</remarks>
	public static double CalculateGeocentricDistance(double heliocentricDistance, double heliocentricLongitude, double heliocentricLatitude, double earthHeliocentricLongitude, double earthHeliocentricDistance = 1.00000011)
	{
		// Convert angles from degrees to radians
		double lRad = double.DegreesToRadians(degrees: heliocentricLongitude);
		double bRad = double.DegreesToRadians(degrees: heliocentricLatitude);
		double lEarthRad = double.DegreesToRadians(degrees: earthHeliocentricLongitude);
		// Convert object position to Cartesian coordinates
		double x = heliocentricDistance * Math.Cos(d: bRad) * Math.Cos(d: lRad);
		double y = heliocentricDistance * Math.Cos(d: bRad) * Math.Sin(a: lRad);
		double z = heliocentricDistance * Math.Sin(a: bRad);
		// Convert Earth position to Cartesian coordinates (assuming Earth ecliptic latitude b ≈ 0)
		double earthX = earthHeliocentricDistance * Math.Cos(d: lEarthRad);
		double earthY = earthHeliocentricDistance * Math.Sin(a: lEarthRad);
		return CalculateGeocentricDistance(x: x, y: y, z: z, earthX: earthX, earthY: earthY, earthZ: 0.0);
	}

	/// <summary>Calculates the geocentric distance (Δ) using the Law of Cosines given heliocentric distances and the heliocentric angle.</summary>
	/// <param name="heliocentricDistance">The distance of the object from the Sun r in AU.</param>
	/// <param name="earthHeliocentricDistance">The distance of Earth from the Sun R in AU.</param>
	/// <param name="heliocentricAngle">The angle at the Sun between the Sun-Earth vector and Sun-object vector in degrees.</param>
	/// <returns>The geocentric distance Δ in AU.</returns>
	/// <remarks>Calculated using the Law of Cosines: Δ = √(r² + R² - 2·r·R·cos(γ)).</remarks>
	public static double CalculateGeocentricDistance(double heliocentricDistance, double earthHeliocentricDistance, double heliocentricAngle)
	{
		double gammaRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: heliocentricAngle));
		double rSq = heliocentricDistance * heliocentricDistance;
		double earthRSq = earthHeliocentricDistance * earthHeliocentricDistance;
		return Math.Sqrt(d: rSq + earthRSq - (2.0 * heliocentricDistance * earthHeliocentricDistance * Math.Cos(d: gammaRad)));
	}

	#endregion

	#region Heliocentric distance

	/// <summary>Calculates the heliocentric distance (r) from 3D Cartesian coordinates.</summary>
	/// <param name="x">The heliocentric X position of the object in AU.</param>
	/// <param name="y">The heliocentric Y position of the object in AU.</param>
	/// <param name="z">The heliocentric Z position of the object in AU.</param>
	/// <returns>The heliocentric distance r in AU.</returns>
	/// <remarks>Calculated as r = √(x² + y² + z²).</remarks>
	public static double CalculateHeliocentricDistanceFromCartesianCoordinates(double x, double y, double z)
	{
		// The heliocentric distance (r) is the Euclidean norm of the position vector from the Sun.
		return Math.Sqrt(d: (x * x) + (y * y) + (z * z));
	}

	/// <summary>Calculates the heliocentric distance (r) using the conic section equation and true anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity e.</param>
	/// <param name="trueAnomaly">The true anomaly ν in degrees.</param>
	/// <returns>The heliocentric distance r in AU.</returns>
	/// <remarks>Calculated using the conic section equation: r = a(1 - e²) / (1 + e·cos(ν)).</remarks>
	public static double CalculateHeliocentricDistance(double semiMajorAxis, double numericalEccentricity, double trueAnomaly)
	{
		return CalculateCurrentDistanceFromFocus(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly);
	}

	/// <summary>Calculates the heliocentric distance (r) using the eccentric anomaly.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity e.</param>
	/// <param name="eccentricAnomaly">The eccentric anomaly E in degrees.</param>
	/// <returns>The heliocentric distance r in AU.</returns>
	/// <remarks>Calculated as r = a · (1 - e · cos(E)).</remarks>
	public static double CalculateHeliocentricDistanceFromEccentricAnomaly(double semiMajorAxis, double numericalEccentricity, double eccentricAnomaly)
	{
		double eRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: eccentricAnomaly));
		return semiMajorAxis * (1.0 - (numericalEccentricity * Math.Cos(d: eRad)));
	}

	#endregion

	#region Elongation

	/// <summary>Calculates the geocentric elongation (ψ) using the Law of Cosines from distances.</summary>
	/// <param name="heliocentricDistance">The distance of the object from the Sun r in AU.</param>
	/// <param name="geocentricDistance">The distance of the object from Earth Δ in AU.</param>
	/// <param name="earthHeliocentricDistance">The distance of Earth from the Sun R in AU (default is 1.00000011 AU).</param>
	/// <returns>The elongation angle at Earth (Sun-Earth-Object angle) in degrees [0°, 180°]. Returns double.NaN if distances are non-positive.</returns>
	/// <remarks>Calculated using the Law of Cosines: cos(ψ) = (R² + Δ² - r²) / (2 · R · Δ).</remarks>
	public static double CalculateElongation(double heliocentricDistance, double geocentricDistance, double earthHeliocentricDistance = 1.00000011)
	{
		// Validate input parameters: distances must be strictly positive.
		if (heliocentricDistance <= 0.0 || geocentricDistance <= 0.0 || earthHeliocentricDistance <= 0.0)
		{
			return double.NaN;
		}
		double rSq = heliocentricDistance * heliocentricDistance;
		double deltaSq = geocentricDistance * geocentricDistance;
		double earthRSq = earthHeliocentricDistance * earthHeliocentricDistance;
		// Cosine of elongation angle from Law of Cosines
		double cosPsi = (earthRSq + deltaSq - rSq) / (2.0 * earthHeliocentricDistance * geocentricDistance);
		// Clamp value to [-1.0, 1.0] to prevent floating point inaccuracy out-of-range errors
		cosPsi = Math.Clamp(value: cosPsi, min: -1.0, max: 1.0);
		// Return elongation in degrees
		return double.RadiansToDegrees(radians: Math.Acos(d: cosPsi));
	}

	/// <summary>Calculates the geocentric elongation (ψ) from 3D heliocentric Cartesian coordinates.</summary>
	/// <param name="x">The heliocentric X position of the object in AU.</param>
	/// <param name="y">The heliocentric Y position of the object in AU.</param>
	/// <param name="z">The heliocentric Z position of the object in AU.</param>
	/// <param name="earthX">The heliocentric X position of Earth in AU.</param>
	/// <param name="earthY">The heliocentric Y position of Earth in AU.</param>
	/// <param name="earthZ">The heliocentric Z position of Earth in AU.</param>
	/// <returns>The elongation angle at Earth (Sun-Earth-Object angle) in degrees [0°, 180°].</returns>
	/// <remarks>Calculated using the vector dot product between the Earth-Sun vector and Earth-Object vector.</remarks>
	public static double CalculateElongation(double x, double y, double z, double earthX, double earthY, double earthZ)
	{
		// Vector from Earth to Sun (-r_Earth)
		double esX = -earthX;
		double esY = -earthY;
		double esZ = -earthZ;
		// Vector from Earth to Object (r_Object - r_Earth)
		double eoX = x - earthX;
		double eoY = y - earthY;
		double eoZ = z - earthZ;
		// Vector magnitudes
		double magES = Math.Sqrt(d: (esX * esX) + (esY * esY) + (esZ * esZ));
		double magEO = Math.Sqrt(d: (eoX * eoX) + (eoY * eoY) + (eoZ * eoZ));
		if (magES <= 0.0 || magEO <= 0.0)
		{
			return double.NaN;
		}
		// Dot product angle calculation
		double dotProduct = (esX * eoX) + (esY * eoY) + (esZ * eoZ);
		double cosPsi = dotProduct / (magES * magEO);
		cosPsi = Math.Clamp(value: cosPsi, min: -1.0, max: 1.0);
		return double.RadiansToDegrees(radians: Math.Acos(d: cosPsi));
	}

	/// <summary>Calculates the geocentric elongation (ψ) from equatorial coordinates (Right Ascension and Declination).</summary>
	/// <param name="rightAscension">The object's right ascension α in degrees.</param>
	/// <param name="declination">The object's declination δ in degrees.</param>
	/// <param name="sunRightAscension">The Sun's right ascension α_Sun in degrees.</param>
	/// <param name="sunDeclination">The Sun's declination δ_Sun in degrees.</param>
	/// <returns>The angular distance (elongation) between the Sun and the object in degrees [0°, 180°].</returns>
	/// <remarks>Calculated using spherical trigonometry: cos(ψ) = sin(δ)·sin(δ_Sun) + cos(δ)·cos(δ_Sun)·cos(α - α_Sun).</remarks>
	public static double CalculateElongationFromEquatorial(double rightAscension, double declination, double sunRightAscension, double sunDeclination)
	{
		double raRad = double.DegreesToRadians(degrees: rightAscension);
		double decRad = double.DegreesToRadians(degrees: declination);
		double sunRaRad = double.DegreesToRadians(degrees: sunRightAscension);
		double sunDecRad = double.DegreesToRadians(degrees: sunDeclination);
		double cosPsi = (Math.Sin(a: decRad) * Math.Sin(a: sunDecRad)) + (Math.Cos(d: decRad) * Math.Cos(d: sunDecRad) * Math.Cos(d: raRad - sunRaRad));
		cosPsi = Math.Clamp(value: cosPsi, min: -1.0, max: 1.0);
		return double.RadiansToDegrees(radians: Math.Acos(d: cosPsi));
	}

	#endregion

	#region Phase angle

	/// <summary>Calculates the phase angle (α) at the object (Sun-Object-Earth angle) using the Law of Cosines from distances.</summary>
	/// <param name="heliocentricDistance">The distance of the object from the Sun r in AU.</param>
	/// <param name="geocentricDistance">The distance of the object from Earth Δ in AU.</param>
	/// <param name="earthHeliocentricDistance">The distance of Earth from the Sun R in AU (default is 1.00000011 AU).</param>
	/// <returns>The phase angle α at the object in degrees [0°, 180°]. Returns double.NaN if distances are non-positive.</returns>
	/// <remarks>Calculated using the Law of Cosines: cos(α) = (r² + Δ² - R²) / (2 · r · Δ).</remarks>
	public static double CalculatePhaseAngle(double heliocentricDistance, double geocentricDistance, double earthHeliocentricDistance = 1.00000011)
	{
		// Validate input parameters: distances must be strictly positive.
		if (heliocentricDistance <= 0.0 || geocentricDistance <= 0.0 || earthHeliocentricDistance <= 0.0)
		{
			return double.NaN;
		}
		double rSq = heliocentricDistance * heliocentricDistance;
		double deltaSq = geocentricDistance * geocentricDistance;
		double earthRSq = earthHeliocentricDistance * earthHeliocentricDistance;
		// Cosine of phase angle from Law of Cosines at the object
		double cosAlpha = (rSq + deltaSq - earthRSq) / (2.0 * heliocentricDistance * geocentricDistance);
		// Clamp value to [-1.0, 1.0] to prevent floating point inaccuracy out-of-range errors
		cosAlpha = Math.Clamp(value: cosAlpha, min: -1.0, max: 1.0);
		// Return phase angle in degrees
		return double.RadiansToDegrees(radians: Math.Acos(d: cosAlpha));
	}

	/// <summary>Calculates the phase angle (α) at the object (Sun-Object-Earth angle) from 3D heliocentric Cartesian coordinates.</summary>
	/// <param name="x">The heliocentric X position of the object in AU.</param>
	/// <param name="y">The heliocentric Y position of the object in AU.</param>
	/// <param name="z">The heliocentric Z position of the object in AU.</param>
	/// <param name="earthX">The heliocentric X position of Earth in AU.</param>
	/// <param name="earthY">The heliocentric Y position of Earth in AU.</param>
	/// <param name="earthZ">The heliocentric Z position of Earth in AU.</param>
	/// <returns>The phase angle α at the object in degrees [0°, 180°].</returns>
	/// <remarks>Calculated using the vector dot product between the Object-Sun vector (-r_Object) and Object-Earth vector (r_Earth - r_Object).</remarks>
	public static double CalculatePhaseAngle(double x, double y, double z, double earthX, double earthY, double earthZ)
	{
		// Vector from Object to Sun (-r_Object)
		double osX = -x;
		double osY = -y;
		double osZ = -z;
		// Vector from Object to Earth (r_Earth - r_Object)
		double oeX = earthX - x;
		double oeY = earthY - y;
		double oeZ = earthZ - z;
		// Vector magnitudes
		double magOS = Math.Sqrt(d: (osX * osX) + (osY * osY) + (osZ * osZ));
		double magOE = Math.Sqrt(d: (oeX * oeX) + (oeY * oeY) + (oeZ * oeZ));
		if (magOS <= 0.0 || magOE <= 0.0)
		{
			return double.NaN;
		}
		// Dot product angle calculation
		double dotProduct = (osX * oeX) + (osY * oeY) + (osZ * oeZ);
		double cosAlpha = dotProduct / (magOS * magOE);
		cosAlpha = Math.Clamp(value: cosAlpha, min: -1.0, max: 1.0);
		return double.RadiansToDegrees(radians: Math.Acos(d: cosAlpha));
	}

	#endregion

	#region Fraction illuminated

	/// <summary>Calculates the illuminated fraction k (phase / fraction illuminated) of an object from the phase angle α.</summary>
	/// <param name="phaseAngle">The Sun-Object-Earth phase angle α in degrees.</param>
	/// <returns>The fraction of the visible disk that is illuminated, in the range [0.0, 1.0].</returns>
	/// <remarks>Calculated as k = (1 + cos(α)) / 2 = cos²(α / 2).</remarks>
	public static double CalculateFractionIlluminated(double phaseAngle)
	{
		// Convert phase angle α from degrees to radians and normalize
		double alphaRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: phaseAngle));
		// Illuminated fraction formula k = (1 + cos(α)) / 2
		double fraction = (1.0 + Math.Cos(d: alphaRad)) / 2.0;
		// Clamp value to [0.0, 1.0] to eliminate small floating-point inaccuracies
		return Math.Clamp(value: fraction, min: 0.0, max: 1.0);
	}

	/// <summary>Calculates the illuminated fraction k (phase / fraction illuminated) directly from distances.</summary>
	/// <param name="heliocentricDistance">The distance of the object from the Sun r in AU.</param>
	/// <param name="geocentricDistance">The distance of the object from Earth Δ in AU.</param>
	/// <param name="earthHeliocentricDistance">The distance of Earth from the Sun R in AU (default is 1.00000011 AU).</param>
	/// <returns>The fraction of the visible disk that is illuminated, in the range [0.0, 1.0]. Returns double.NaN if distances are non-positive.</returns>
	/// <remarks>First calculates the phase angle α using the Law of Cosines, then evaluates k = (1 + cos(α)) / 2.</remarks>
	public static double CalculateFractionIlluminated(double heliocentricDistance, double geocentricDistance, double earthHeliocentricDistance = 1.00000011)
	{
		double phaseAngle = CalculatePhaseAngle(heliocentricDistance: heliocentricDistance, geocentricDistance: geocentricDistance, earthHeliocentricDistance: earthHeliocentricDistance);
		return double.IsNaN(d: phaseAngle) ? double.NaN : CalculateFractionIlluminated(phaseAngle: phaseAngle);
	}

	#endregion

	#region Angular diameter

	/// <summary>Calculates the apparent angular diameter of an object in arcseconds (").</summary>
	/// <param name="diameterKm">The mean physical diameter of the object in kilometers.</param>
	/// <param name="geocentricDistance">The distance of the object from Earth Δ in AU.</param>
	/// <returns>The apparent angular diameter in arcseconds (arcsec / "). Returns double.NaN if parameters are non-positive.</returns>
	/// <remarks>Calculated using exact spherical trigonometry: θ = 2 · arctan(D / (2 · Δ_km)), where 1 AU = 149,597,870.7 km.</remarks>
	public static double CalculateAngularDiameterArcseconds(double diameterKm, double geocentricDistance)
	{
		// Validate input parameters: diameter and distance must be strictly positive.
		if (diameterKm <= 0.0 || geocentricDistance <= 0.0)
		{
			return double.NaN;
		}
		// Exact length of 1 AU in kilometers (IAU 2012 definition)
		const double kmPerAu = 149_597_870.7;
		// Convert geocentric distance from AU to kilometers
		double geocentricDistanceKm = geocentricDistance * kmPerAu;
		// Exact angular diameter in radians: θ = 2 · arctan(D / (2 · Δ_km))
		double angularDiameterRad = 2.0 * Math.Atan(d: diameterKm / (2.0 * geocentricDistanceKm));
		// Convert radians to degrees and then to arcseconds (1° = 3600")
		return double.RadiansToDegrees(radians: angularDiameterRad) * 3600.0;
	}

	/// <summary>Calculates the apparent angular diameter of an object in milliarcseconds (mas).</summary>
	/// <param name="diameterKm">The mean physical diameter of the object in kilometers.</param>
	/// <param name="geocentricDistance">The distance of the object from Earth Δ in AU.</param>
	/// <returns>The apparent angular diameter in milliarcseconds (mas).</returns>
	public static double CalculateAngularDiameterMilliarcseconds(double diameterKm, double geocentricDistance)
	{
		return CalculateAngularDiameterArcseconds(diameterKm: diameterKm, geocentricDistance: geocentricDistance) * 1000.0;
	}

	#endregion

	/// <summary>Calculates the estimated duration of the opposition loop (retrograde motion) in days.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU.</param>
	/// <returns>The estimated duration of retrograde motion in days. Returns double.NaN if semiMajorAxis lower/equal than 0 or equal to 1.0.</returns>
	/// <remarks>Calculated using the analytical stationary point condition for coplanar circular orbits: cos(φ_stat) = √a / (a - √a + 1), where φ_stat is the heliocentric angle at stationarity. The retrograde duration is Δt = (φ_stat / π) · T_syn · 365.25 days.</remarks>
	public static double CalculateOppositionLoopDurationDays(double semiMajorAxis)
	{
		// Validate input: semi-major axis must be strictly positive and not equal to Earth's orbit (1.0 AU)
		if (semiMajorAxis is <= 0.0 or 1.0)
		{
			return double.NaN;
		}
		double sqrtA = Math.Sqrt(d: semiMajorAxis);
		// Cosine of heliocentric angle φ_stat at stationary points: cos(φ_stat) = √a / (a - √a + 1)
		double cosPhiStat = sqrtA / (semiMajorAxis - sqrtA + 1.0);
		// Clamp value to [-1.0, 1.0] to prevent floating point numerical out-of-bounds
		cosPhiStat = Math.Clamp(value: cosPhiStat, min: -1.0, max: 1.0);
		// Heliocentric stationary angle φ_stat in radians
		double phiStatRad = Math.Acos(d: cosPhiStat);
		// Synodic period T_syn in years: T_syn = 1 / |1 - a^(-3/2)|
		double synodicPeriodYears = CalculateSynodicOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		if (double.IsInfinity(d: synodicPeriodYears))
		{
			return double.NaN;
		}
		// Retrograde duration in days: Δt = (φ_stat / π) · T_syn · DaysPerJulianYear
		return phiStatRad / Math.PI * synodicPeriodYears * DaysPerJulianYear;
	}

	/// <summary>Calculates the estimated heliocentric stationary angle (φ_stat) in degrees at which retrograde motion begins and ends.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU.</param>
	/// <returns>The heliocentric angle φ_stat in degrees [0°, 90°]. Returns double.NaN if semiMajorAxis lower/equal than 0.</returns>
	/// <remarks>Calculated as φ_stat = arccos(√a / (a - √a + 1)).</remarks>
	public static double CalculateStationaryHeliocentricAngle(double semiMajorAxis)
	{
		if (semiMajorAxis <= 0.0)
		{
			return double.NaN;
		}
		double sqrtA = Math.Sqrt(d: semiMajorAxis);
		double cosPhiStat = Math.Clamp(value: sqrtA / (semiMajorAxis - sqrtA + 1.0), min: -1.0, max: 1.0);
		return double.RadiansToDegrees(radians: Math.Acos(d: cosPhiStat));
	}

	#region Relativistic perihelion precession

	/// <summary>Calculates the relativistic perihelion precession in arcseconds per century (arcsec/cy).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity e [0, 1).</param>
	/// <returns>The relativistic perihelion precession in arcseconds per century. Returns double.NaN for invalid or hyperbolic orbits.</returns>
	/// <remarks>Calculated using Einstein's formula for General Relativity: Δϖ_rad/orbit = (6 · π · G · M_Sun) / (c² · a · (1 - e²)). Converted to arcseconds per century using the orbital period T = a^(3/2) years.</remarks>
	public static double CalculateRelativisticPerihelionPrecessionArcsecondsPerCentury(double semiMajorAxis, double numericalEccentricity)
	{
		// Validate input parameters: semi-major axis must be positive and orbit must be closed (0 <= e < 1)
		if (semiMajorAxis <= 0.0 || numericalEccentricity is < 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		// Precession per orbit in arcseconds
		double arcsecondsPerOrbit = CalculateRelativisticPerihelionPrecessionArcsecondsPerOrbit(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		// Orbital period T in Julian years (Kepler's 3rd Law: T = a^(3/2))
		double periodYears = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		// Convert arcseconds per orbit to arcseconds per century
		return arcsecondsPerOrbit / periodYears * 100.0;
	}

	/// <summary>Calculates the relativistic perihelion precession per single orbit in arcseconds (arcsec/orbit).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity e [0, 1).</param>
	/// <returns>The relativistic perihelion precession per orbit in arcseconds.</returns>
	public static double CalculateRelativisticPerihelionPrecessionArcsecondsPerOrbit(double semiMajorAxis, double numericalEccentricity)
	{
		double radiansPerOrbit = CalculateRelativisticPerihelionPrecessionRadiansPerOrbit(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);
		return double.IsNaN(d: radiansPerOrbit) ? double.NaN : double.RadiansToDegrees(radians: radiansPerOrbit) * 3600.0;
	}

	/// <summary>Calculates the relativistic perihelion precession per single orbit in radians (rad/orbit).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="numericalEccentricity">The numerical eccentricity e [0, 1).</param>
	/// <returns>The relativistic perihelion precession per orbit in radians.</returns>
	public static double CalculateRelativisticPerihelionPrecessionRadiansPerOrbit(double semiMajorAxis, double numericalEccentricity)
	{
		if (semiMajorAxis <= 0.0 || numericalEccentricity is < 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		// Solar gravitational parameter (GM_Sun) in m³/s²
		const double gmSunM3S2 = 1.32712440018e20;
		// Speed of light (c) in m/s
		const double speedOfLightMS = 299_792_458.0;
		// Exact length of 1 AU in meters (IAU 2012 definition)
		const double metersPerAu = 149_597_870_700.0;
		// Convert semi-major axis to meters
		double semiMajorAxisMeters = semiMajorAxis * metersPerAu;
		// Einstein's formula for General Relativistic precession per orbit: Δϖ = (6 · π · GM) / (c² · a · (1 - e²))
		double denominator = speedOfLightMS * speedOfLightMS * semiMajorAxisMeters * (1.0 - (numericalEccentricity * numericalEccentricity));
		return 6.0 * Math.PI * gmSunM3S2 / denominator;
	}

	#endregion

	#region Yarkovsky drift rate

	/// <summary>Calculates the Yarkovsky effect semi-major axis drift rate (da/dt) in astronomical units per year (AU/yr).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="diameterKm">The mean physical diameter of the asteroid in kilometers.</param>
	/// <param name="densityGPerCm3">The mean bulk density in g/cm³ (e.g. 1.19 for Bennu, 2.7 for S-type).</param>
	/// <param name="obliquityDegrees">The spin axis obliquity γ in degrees [0°, 180°]. (γ &lt; 90°: prograde/outward, γ &gt; 90°: retrograde/inward).</param>
	/// <param name="thermalEfficiencyFactor">The thermal lag efficiency factor f_thermal (typically ~0.10 to 0.20, default is 0.15).</param>
	/// <param name="absorptionCoefficient">The absorption coefficient α = 1 - Bond Albedo (default is 0.90).</param>
	/// <returns>The semi-major axis drift rate da/dt in AU/year. Positive values indicate outward drift; negative values indicate inward drift. Returns double.NaN for invalid parameters.</returns>
	/// <remarks>Calculated using the diurnal Yarkovsky recoil thermal force model: da/dt = (2 · α · S₀ · f_thermal · cos(γ) · AU_m²) / (c · ρ · D · √(GM_Sun · a_m)). Prograde rotators (γ &lt; 90°) drift outward (da/dt &gt; 0); retrograde rotators (γ &gt; 90°) drift inward (da/dt &lt; 0).
	/// </remarks>
	public static double CalculateYarkovskyDriftRateAuPerYear(double semiMajorAxis, double diameterKm, double densityGPerCm3, double obliquityDegrees, double thermalEfficiencyFactor = 0.15, double absorptionCoefficient = 0.9)
	{
		// Validate input parameters: semi-major axis, diameter, and density must be strictly positive
		if (semiMajorAxis <= 0.0 || diameterKm <= 0.0 || densityGPerCm3 <= 0.0)
		{
			return double.NaN;
		}
		// Solar constant at 1 AU (W/m²)
		const double solarConstantS0 = 1361.0;
		// Speed of light c (m/s)
		const double speedOfLightMS = 299_792_458.0;
		// Solar gravitational parameter GM_Sun (m³/s²)
		const double gmSunM3S2 = 1.32712440018e20;
		// 1 AU in meters (IAU 2012)
		const double metersPerAu = 149_597_870_700.0;
		// Seconds in a Julian year (365.25 days)
		const double secondsPerJulianYear = DaysPerJulianYear * 86400.0;
		// Convert inputs to SI units
		double semiMajorAxisMeters = semiMajorAxis * metersPerAu;
		double diameterMeters = diameterKm * 1000.0;
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		double obliquityRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: obliquityDegrees));
		// Calculate numerator and denominator for da/dt in m/s
		double numerator = 2.0 * absorptionCoefficient * solarConstantS0 * thermalEfficiencyFactor * Math.Cos(d: obliquityRad) * (metersPerAu * metersPerAu);
		double denominator = speedOfLightMS * densityKgPerM3 * diameterMeters * Math.Sqrt(d: gmSunM3S2 * semiMajorAxisMeters);
		double driftRateMetersPerSecond = numerator / denominator;
		// Convert m/s to AU/year
		return driftRateMetersPerSecond * secondsPerJulianYear / metersPerAu;
	}

	/// <summary>Calculates the Yarkovsky effect semi-major axis drift rate in meters per year (m/yr).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="diameterKm">The mean physical diameter of the asteroid in kilometers.</param>
	/// <param name="densityGPerCm3">The mean bulk density in g/cm³.</param>
	/// <param name="obliquityDegrees">The spin axis obliquity γ in degrees [0°, 180°].</param>
	/// <param name="thermalEfficiencyFactor">The thermal lag efficiency factor (default is 0.15).</param>
	/// <param name="absorptionCoefficient">The absorption coefficient (default is 0.90).</param>
	/// <returns>The semi-major axis drift rate in meters per year (m/yr).</returns>
	public static double CalculateYarkovskyDriftRateMetersPerYear(double semiMajorAxis, double diameterKm, double densityGPerCm3, double obliquityDegrees, double thermalEfficiencyFactor = 0.15, double absorptionCoefficient = 0.9)
	{
		const double metersPerAu = 149_597_870_700.0;
		double driftRateAuPerYear = CalculateYarkovskyDriftRateAuPerYear(semiMajorAxis: semiMajorAxis, diameterKm: diameterKm, densityGPerCm3: densityGPerCm3, obliquityDegrees: obliquityDegrees, thermalEfficiencyFactor: thermalEfficiencyFactor, absorptionCoefficient: absorptionCoefficient);
		return driftRateAuPerYear * metersPerAu;
	}

	/// <summary>Calculates the Yarkovsky effect semi-major axis drift rate in 10⁻⁴ AU per million years (10⁻⁴ AU/Myr), matching MPC and NEODyS database conventions.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="diameterKm">The mean physical diameter of the asteroid in kilometers.</param>
	/// <param name="densityGPerCm3">The mean bulk density in g/cm³.</param>
	/// <param name="obliquityDegrees">The spin axis obliquity γ in degrees [0°, 180°].</param>
	/// <param name="thermalEfficiencyFactor">The thermal lag efficiency factor (default is 0.15).</param>
	/// <param name="absorptionCoefficient">The absorption coefficient (default is 0.90).</param>
	/// <returns>The drift rate in 10⁻⁴ AU/Myr (e.g. -19.0 for 101955 Bennu).</returns>
	public static double CalculateYarkovskyDriftRateAuPerMyr(double semiMajorAxis, double diameterKm, double densityGPerCm3, double obliquityDegrees, double thermalEfficiencyFactor = 0.15, double absorptionCoefficient = 0.9)
	{
		// 1 AU/yr = 10⁶ AU/Myr = 10¹⁰ (10⁻⁴ AU/Myr)
		double driftRateAuPerYear = CalculateYarkovskyDriftRateAuPerYear(semiMajorAxis: semiMajorAxis, diameterKm: diameterKm, densityGPerCm3: densityGPerCm3, obliquityDegrees: obliquityDegrees, thermalEfficiencyFactor: thermalEfficiencyFactor, absorptionCoefficient: absorptionCoefficient);
		return driftRateAuPerYear * 1e10;
	}

	#endregion

	#region Poynting-Robertson drift & Poynting-Robertson lifetime

	/// <summary>Calculates the Poynting-Robertson effect semi-major axis drift rate (da/dt) in AU per year for small particles and meteoroids.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a in AU.</param>
	/// <param name="diameterMeters">The particle/body diameter D in meters.</param>
	/// <param name="densityGPerCm3">The bulk density ρ in g/cm³ (default is 2.0 g/cm³).</param>
	/// <param name="radiationEfficiencyQpr">The radiation pressure efficiency factor Q_pr (default is 1.0 for geometric optics).</param>
	/// <returns>The semi-major axis drift rate da/dt in AU/year (always negative, indicating inward orbital decay towards the Sun). Returns double.NaN for invalid parameters.</returns>
	/// <remarks>Calculated using the Poynting-Robertson drag equation: da/dt = - (3 · L_Sun · Q_pr) / (4 · π · c² · ρ · D · a_m). Poynting-Robertson drag dominates over the Yarkovsky effect for micrometer- to centimeter-sized dust grains and small meteoroids.</remarks>
	public static double CalculatePoyntingRobertsonDriftRateAuPerYear(double semiMajorAxis, double diameterMeters, double densityGPerCm3 = 2.0, double radiationEfficiencyQpr = 1.0)
	{
		// Validate input parameters: semi-major axis, diameter, density, and Q_pr must be strictly positive
		if (semiMajorAxis <= 0.0 || diameterMeters <= 0.0 || densityGPerCm3 <= 0.0 || radiationEfficiencyQpr <= 0.0)
		{
			return double.NaN;
		}
		// Solar luminosity L_Sun in Watts (W)
		const double solarLuminosityWatts = 3.828e26;
		// Speed of light c in m/s
		const double speedOfLightMS = 299_792_458.0;
		// 1 AU in meters (IAU 2012 definition)
		const double metersPerAu = 149_597_870_700.0;
		// Seconds in a Julian year (365.25 days)
		const double secondsPerJulianYear = DaysPerJulianYear * 86400.0;
		// Convert inputs to SI units
		double semiMajorAxisMeters = semiMajorAxis * metersPerAu;
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		// da/dt in m/s: da/dt = - (3 · L_Sun · Q_pr) / (4 · π · c² · ρ · D · a)
		double denominator = 4.0 * Math.PI * speedOfLightMS * speedOfLightMS * densityKgPerM3 * diameterMeters * semiMajorAxisMeters;
		double driftRateMetersPerSecond = -(3.0 * solarLuminosityWatts * radiationEfficiencyQpr) / denominator;
		// Convert m/s to AU/year
		return driftRateMetersPerSecond * secondsPerJulianYear / metersPerAu;
	}

	/// <summary>Calculates the Poynting-Robertson orbital decay time (time to spiral into the Sun) in Julian years.</summary>
	/// <param name="initialSemiMajorAxis">The initial semi-major axis a in AU.</param>
	/// <param name="diameterMeters">The particle/body diameter D in meters.</param>
	/// <param name="densityGPerCm3">The bulk density ρ in g/cm³ (default is 2.0 g/cm³).</param>
	/// <param name="radiationEfficiencyQpr">The radiation pressure efficiency factor Q_pr (default is 1.0).</param>
	/// <returns>The total lifetime / orbital collapse time into the Sun in Julian years. Returns double.NaN for invalid parameters.</returns>
	/// <remarks>Calculated by integrating the Poynting-Robertson drift equation: t_PR = (2 · π · c² · ρ · D · a²) / (3 · L_Sun · Q_pr).</remarks>
	public static double CalculatePoyntingRobertsonLifetimeYears(double initialSemiMajorAxis, double diameterMeters, double densityGPerCm3 = 2.0, double radiationEfficiencyQpr = 1.0)
	{
		if (initialSemiMajorAxis <= 0.0 || diameterMeters <= 0.0 || densityGPerCm3 <= 0.0 || radiationEfficiencyQpr <= 0.0)
		{
			return double.NaN;
		}
		// Solar luminosity L_Sun in Watts (W)
		const double solarLuminosityWatts = 3.828e26;
		// Speed of light c in m/s
		const double speedOfLightMS = 299_792_458.0;
		// 1 AU in meters (IAU 2012 definition)
		const double metersPerAu = 149_597_870_700.0;
		// Seconds in a Julian year (365.25 days)
		const double secondsPerJulianYear = DaysPerJulianYear * 86400.0;
		double semiMajorAxisMeters = initialSemiMajorAxis * metersPerAu;
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		// Lifetime in seconds: t_PR = (2 · π · c² · ρ · D · a²) / (3 · L_Sun · Q_pr)
		double lifetimeSeconds = 2.0 * Math.PI * speedOfLightMS * speedOfLightMS * densityKgPerM3 * diameterMeters * semiMajorAxisMeters * semiMajorAxisMeters / (3.0 * solarLuminosityWatts * radiationEfficiencyQpr);
		// Convert seconds to Julian years
		return lifetimeSeconds / secondsPerJulianYear;
	}

	#endregion

	#region Beta parameter

	/// <summary>Calculates the beta parameter (β = F_rad / F_grav), the ratio of solar radiation pressure force to solar gravitational force.</summary>
	/// <param name="diameterMeters">The body or dust grain diameter D in meters.</param>
	/// <param name="densityGPerCm3">The bulk density ρ in g/cm³ (default is 2.0 g/cm³).</param>
	/// <param name="radiationEfficiencyQpr">The radiation pressure efficiency factor Q_pr (default is 1.0 for geometric optics / large particles).</param>
	/// <returns>The dimensionless beta parameter β. Returns double.NaN for non-positive inputs.</returns>
	/// <remarks>Calculated as β = (3 · L_Sun · Q_pr) / (8 · π · c · GM_Sun · ρ · D). Distance r cancels out because both forces scale with 1/r². Dynamics:
	/// - β &lt; 0.5: Bound elliptical orbits (radiation pressure reduces effective gravitational mass).
	/// - 0.5 &lt;= β &lt; 1.0: Hyperbolic escape orbits for dust released at perihelion (e.g. cometary dust tails).
	/// - β &gt;= 1.0: Total gravitational repulsion; particles are blown straight out of the solar system.</remarks>
	public static double CalculateBetaParameter(double diameterMeters, double densityGPerCm3 = 2.0, double radiationEfficiencyQpr = 1.0)
	{
		// Validate input parameters: diameter, density, and efficiency factor must be strictly positive
		if (diameterMeters <= 0.0 || densityGPerCm3 <= 0.0 || radiationEfficiencyQpr <= 0.0)
		{
			return double.NaN;
		}
		// Solar luminosity L_Sun in Watts (W)
		const double solarLuminosityWatts = 3.828e26;
		// Speed of light c in m/s
		const double speedOfLightMS = 299_792_458.0;
		// Solar gravitational parameter GM_Sun in m³/s²
		const double gmSunM3S2 = 1.32712440018e20;
		// Convert density from g/cm³ to kg/m³
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		// Distance-independent constant K_beta = (3 · L_Sun) / (8 · π · c · GM_Sun) ≈ 1.148474e-3 kg/m²
		double kBeta = 3.0 * solarLuminosityWatts / (8.0 * Math.PI * speedOfLightMS * gmSunM3S2);
		// β = (K_beta · Q_pr) / (ρ · D)
		return kBeta * radiationEfficiencyQpr / (densityKgPerM3 * diameterMeters);
	}

	/// <summary>Calculates the beta parameter (β) using object diameter given in kilometers.</summary>
	/// <param name="diameterKm">The physical diameter D in kilometers.</param>
	/// <param name="densityGPerCm3">The bulk density ρ in g/cm³.</param>
	/// <param name="radiationEfficiencyQpr">The radiation pressure efficiency factor Q_pr (default is 1.0).</param>
	/// <returns>The dimensionless beta parameter β.</returns>
	public static double CalculateBetaParameterFromKm(double diameterKm, double densityGPerCm3 = 2.0, double radiationEfficiencyQpr = 1.0)
	{
		return CalculateBetaParameter(diameterMeters: diameterKm * 1000.0, densityGPerCm3: densityGPerCm3, radiationEfficiencyQpr: radiationEfficiencyQpr);
	}

	/// <summary>Calculates the critical blow-off diameter in meters for a threshold beta value (e.g. β_crit = 0.5 or 1.0).</summary>
	/// <param name="criticalBeta">The critical beta threshold (default is 0.5 for perihelion dust ejection).</param>
	/// <param name="densityGPerCm3">The bulk density ρ in g/cm³ (default is 2.0 g/cm³).</param>
	/// <param name="radiationEfficiencyQpr">The radiation pressure efficiency factor Q_pr (default is 1.0).</param>
	/// <returns>The critical particle diameter in meters. Particles smaller than this diameter are blown into hyperbolic escape orbits.</returns>
	/// <remarks>Calculated as D_crit = (3 · L_Sun · Q_pr) / (8 · π · c · GM_Sun · ρ · β_crit).</remarks>
	public static double CalculateCriticalBlowoffDiameterMeters(double criticalBeta = 0.5, double densityGPerCm3 = 2.0, double radiationEfficiencyQpr = 1.0)
	{
		if (criticalBeta <= 0.0 || densityGPerCm3 <= 0.0 || radiationEfficiencyQpr <= 0.0)
		{
			return double.NaN;
		}
		// Solar luminosity L_Sun in Watts (W)
		const double solarLuminosityWatts = 3.828e26;
		// Speed of light c in m/s
		const double speedOfLightMS = 299_792_458.0;
		// Solar gravitational parameter GM_Sun in m³/s²
		const double gmSunM3S2 = 1.32712440018e20;
		// Convert density from g/cm³ to kg/m³
		double densityKgPerM3 = densityGPerCm3 * 1000.0;
		// Distance-independent constant K_beta = (3 · L_Sun) / (8 · π · c · GM_Sun) ≈ 1.148474e-3 kg/m²
		double kBeta = 3.0 * solarLuminosityWatts / (8.0 * Math.PI * speedOfLightMS * gmSunM3S2);
		// β = (K_beta · Q_pr) / (ρ · β_crit)
		return kBeta * radiationEfficiencyQpr / (densityKgPerM3 * criticalBeta);
	}

	#endregion

	/// <summary>Analyzes the invariance of the Tisserand parameter across a planetary encounter (Restricted Three-Body Problem).</summary>
	/// <param name="semiMajorAxisMain">The semi-major axis of the perturbing planet in AU (e.g. 5.20336301 for Jupiter).</param>
	/// <param name="preSemiMajorAxis">The pre-encounter semi-major axis in AU.</param>
	/// <param name="preEccentricity">The pre-encounter numerical eccentricity.</param>
	/// <param name="preInclination">The pre-encounter inclination in degrees.</param>
	/// <param name="postSemiMajorAxis">The post-encounter semi-major axis in AU.</param>
	/// <param name="postEccentricity">The post-encounter numerical eccentricity.</param>
	/// <param name="postInclination">The post-encounter inclination in degrees.</param>
	/// <returns>A TisserandEncounterAnalysis structure containing pre/post Tisserand values, absolute and relative deviations, and orbit dynamical classifications.</returns>
	/// <remarks>In the Circular Restricted Three-Body Problem (CR3BP), Tisserand's parameter T = a_p/a + 2·cos(i)·√(a(1-e²)/a_p) is quasi-conserved before and after close planetary encounters. Deviations exceeding ~1%–5% indicate non-zero planetary orbital eccentricity, multi-body gravitational interactions, or non-gravitational forces (e.g. Yarkovsky effect, cometary outgassing).</remarks>
	public static TisserandEncounterAnalysis CalculateTisserandEncounterAnalysis(double semiMajorAxisMain, double preSemiMajorAxis, double preEccentricity, double preInclination, double postSemiMajorAxis, double postEccentricity, double postInclination)
	{
		// Calculate pre-encounter and post-encounter Tisserand parameters
		double preTisserand = CalculateTisserandParameter(semiMajorAxisMain: semiMajorAxisMain, semiMajorAxis: preSemiMajorAxis, numericalEccentricity: preEccentricity, inclination: preInclination);
		double postTisserand = CalculateTisserandParameter(semiMajorAxisMain: semiMajorAxisMain, semiMajorAxis: postSemiMajorAxis, numericalEccentricity: postEccentricity, inclination: postInclination);
		// Absolute and percentage deviations
		double absoluteDelta = Math.Abs(value: postTisserand - preTisserand);
		double relativeDeviationPercent = preTisserand != 0.0 ? absoluteDelta / Math.Abs(value: preTisserand) * 100.0 : double.NaN;
		// Check if the encounter conserves Tisserand parameter within typical CR3BP tolerance (<= 5%)
		bool isQuasiConserved = relativeDeviationPercent <= 5.0;
		// Classify dynamical orbit types based on Tisserand parameter (e.g. Main Belt Asteroid vs. Jupiter-Family Comet)
		OrbitDynamicalClass preClassification = ClassifyOrbitByTisserand(tisserandParameter: preTisserand, semiMajorAxis: preSemiMajorAxis);
		OrbitDynamicalClass postClassification = ClassifyOrbitByTisserand(tisserandParameter: postTisserand, semiMajorAxis: postSemiMajorAxis);
		return new TisserandEncounterAnalysis(PreTisserand: preTisserand, PostTisserand: postTisserand, AbsoluteDelta: absoluteDelta, RelativeDeviationPercent: relativeDeviationPercent, IsQuasiConserved: isQuasiConserved, PreClassification: preClassification, PostClassification: postClassification);
	}

	/// <summary>Classifies the dynamical orbit type based on Jupiter's Tisserand parameter T_J and semi-major axis.</summary>
	/// <param name="tisserandParameter">The Tisserand parameter relative to Jupiter (T_J).</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <returns>An OrbitDynamicalClass enumeration value representing the orbital regime.</returns>
	/// <remarks>Classification standards (Levison &amp; Duncan, 1997):
	/// - T_J &gt; 3.0 (and a &lt; a_Jupiter): Main-belt / Near-Earth Asteroid.
	/// - 2.0 &lt;= T_J &lt;= 3.0: Jupiter-Family Comet (JFC).
	/// - T_J &lt; 2.0 (and a &gt; 0): Halley-type comet (HTC) / Damocloid / Centaur.
	/// - a &lt; 0: Hyperbolic / Interstellar object.</remarks>
	public static OrbitDynamicalClass ClassifyOrbitByTisserand(double tisserandParameter, double semiMajorAxis)
	{
		return semiMajorAxis < 0.0 ? OrbitDynamicalClass.HyperbolicOrInterstellar
			: tisserandParameter > 3.0 ? OrbitDynamicalClass.Asteroidal
			: tisserandParameter is >= 2.0 and <= 3.0 ? OrbitDynamicalClass.JupiterFamilyComet
			: OrbitDynamicalClass.HalleyTypeCometOrDamocloid;
	}

	/// <summary>Calculates the theoretical post-encounter semi-major axis a' given a conserved Tisserand parameter T, target eccentricity e', and inclination i'.</summary>
	/// <param name="tisserandParameter">The conserved pre-encounter Tisserand parameter T.</param>
	/// <param name="semiMajorAxisMain">The semi-major axis of the perturbing planet in AU.</param>
	/// <param name="postEccentricity">The target post-encounter eccentricity e'.</param>
	/// <param name="postInclination">The target post-encounter inclination i' in degrees.</param>
	/// <returns>The theoretical post-encounter semi-major axis a' in AU, solved numerically via Newton-Raphson iteration from k·x³ - T·x² + 1 = 0, where x = √(a'/a_p).</returns>
	public static double CalculateTheoreticalPostEncounterSemiMajorAxis(double tisserandParameter, double semiMajorAxisMain, double postEccentricity, double postInclination)
	{
		if (semiMajorAxisMain <= 0.0 || postEccentricity is < 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		double iRad = double.DegreesToRadians(degrees: NormalizeAngle360(degrees: postInclination));
		double k = 2.0 * Math.Cos(d: iRad) * Math.Sqrt(d: 1.0 - (postEccentricity * postEccentricity));
		// Solve k·x³ - T·x² + 1 = 0 for x = √(a'/a_p) using Newton-Raphson
		double x = 1.0; // Initial guess: a' ≈ a_p
		const int maxIterations = 100;
		const double tolerance = 1e-12;
		for (int iteration = 0; iteration < maxIterations; iteration++)
		{
			double f = (k * Math.Pow(x: x, y: 3)) - (tisserandParameter * x * x) + 1.0;
			if (Math.Abs(value: f) < tolerance)
			{
				break;
			}
			double fPrime = (3.0 * k * x * x) - (2.0 * tisserandParameter * x);
			if (Math.Abs(value: fPrime) < 1e-14)
			{
				break;
			}
			x -= f / fPrime;
		}
		if (double.IsNaN(d: x) || x <= 0.0)
		{
			return double.NaN;
		}
		// a' = a_p · x²
		return semiMajorAxisMain * x * x;
	}

	#region Moid drift analysis

	/// <summary>Calculates the temporal rate of change of MOID (dMOID/dt) in AU per century (AU/cy).</summary>
	/// <param name="initialMoidAu">The MOID in AU at the initial epoch (t1).</param>
	/// <param name="finalMoidAu">The MOID in AU at the final epoch (t2).</param>
	/// <param name="timeSpanYears">The elapsed time between epochs in Julian years (t2 - t1).</param>
	/// <returns>The temporal MOID drift rate in AU per century. Negative values indicate that the orbit is approaching the target planet's orbit.</returns>
	/// <remarks>Calculated as dMOID/dt = ((MOID_t2 - MOID_t1) / Δt_years) · 100.</remarks>
	public static double CalculateMoidDriftRateAuPerCentury(double initialMoidAu, double finalMoidAu, double timeSpanYears)
	{
		// Validate input parameters: time span must be non-zero and positive
		if (timeSpanYears <= 0.0 || initialMoidAu < 0.0 || finalMoidAu < 0.0)
		{
			return double.NaN;
		}
		// Calculate rate of change in AU per year and convert to AU per century
		double driftRateAuPerYear = (finalMoidAu - initialMoidAu) / timeSpanYears;
		return driftRateAuPerYear * 100.0;
	}

	/// <summary>Estimates the time in Julian years until the MOID reaches a specific target threshold distance (e.g. 0.0 AU or 0.05 AU).</summary>
	/// <param name="currentMoidAu">The current MOID in AU.</param>
	/// <param name="driftRateAuPerCentury">The secular MOID drift rate dMOID/dt in AU per century.</param>
	/// <param name="targetThresholdAu">The target MOID threshold in AU (default is 0.0 AU for exact orbit intersection).</param>
	/// <returns>The estimated time in Julian years until the threshold is reached. Returns PositiveInfinity if the drift rate moves away from the target, or NaN for invalid inputs.</returns>
	public static double CalculateYearsToMoidThreshold(double currentMoidAu, double driftRateAuPerCentury, double targetThresholdAu = 0.0)
	{
		if (currentMoidAu < 0.0 || targetThresholdAu < 0.0 || driftRateAuPerCentury == 0.0 || double.IsNaN(driftRateAuPerCentury))
		{
			return double.NaN;
		}
		// Distance difference to the target threshold
		double moidDelta = targetThresholdAu - currentMoidAu;
		double driftRateAuPerYear = driftRateAuPerCentury / 100.0;
		// Calculate time to threshold in years
		double yearsToThreshold = moidDelta / driftRateAuPerYear;
		// If time is negative, the orbit is drifting away from the threshold
		return yearsToThreshold < 0.0 ? double.PositiveInfinity : yearsToThreshold;
	}

	/// <summary>Performs a comprehensive secular drift analysis on MOID changes between two epochs.</summary>
	/// <param name="initialMoidAu">The MOID in AU at epoch 1.</param>
	/// <param name="finalMoidAu">The MOID in AU at epoch 2.</param>
	/// <param name="timeSpanYears">The time difference between epoch 1 and epoch 2 in Julian years.</param>
	/// <param name="absoluteMagnitudeH">The absolute magnitude H in mag (used to evaluate PHA transitions).</param>
	/// <param name="phaThresholdAu">The PHA MOID threshold in AU (default is 0.05 AU).</param>
	/// <returns>A MoidDriftAnalysis structure containing drift rate metrics, time projections, and PHA status transition flags.</returns>
	public static MoidDriftAnalysis AnalyzeMoidDriftRate(double initialMoidAu, double finalMoidAu, double timeSpanYears, double absoluteMagnitudeH = 22.0, double phaThresholdAu = 0.05)
	{
		// Calculate drift rate in AU per century
		double driftRateAuPerCentury = CalculateMoidDriftRateAuPerCentury(initialMoidAu: initialMoidAu, finalMoidAu: finalMoidAu, timeSpanYears: timeSpanYears);
		// Project time in years to exact intersection (MOID = 0.0 AU) and PHA threshold (MOID = 0.05 AU)
		double yearsToZeroMoid = CalculateYearsToMoidThreshold(currentMoidAu: finalMoidAu, driftRateAuPerCentury: driftRateAuPerCentury, targetThresholdAu: 0.0);
		double yearsToPhaThreshold = CalculateYearsToMoidThreshold(currentMoidAu: finalMoidAu, driftRateAuPerCentury: driftRateAuPerCentury, targetThresholdAu: phaThresholdAu);
		// Direction check: Is the orbit converging towards the target planet's orbit?
		bool isApproachingEarthOrbit = driftRateAuPerCentury < 0.0;
		// Initial and final PHA status evaluation
		bool isInitialPha = IsPotentiallyHazardous(earthMoidAu: initialMoidAu, absoluteMagnitudeH: absoluteMagnitudeH);
		bool isFinalPha = IsPotentiallyHazardous(earthMoidAu: finalMoidAu, absoluteMagnitudeH: absoluteMagnitudeH);
		// Transition states
		bool willBecomePha = !isInitialPha && isFinalPha;
		bool willCeasePha = isInitialPha && !isFinalPha;
		return new MoidDriftAnalysis(InitialMoidAu: initialMoidAu, FinalMoidAu: finalMoidAu, TimeSpanYears: timeSpanYears, DriftRateAuPerCentury: driftRateAuPerCentury, EstimatedYearsToZeroMoid: yearsToZeroMoid, EstimatedYearsToPhaThreshold: yearsToPhaThreshold, IsApproachingEarthOrbit: isApproachingEarthOrbit, WillBecomePha: willBecomePha, WillCeasePha: willCeasePha);
	}

	#endregion

	#region Kozai-Lidov period

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years for a small body perturbed by an outer massive planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the perturbed object in AU.</param>
	/// <param name="perturbingMassSolarMasses">The mass of the perturbing planet in solar masses (e.g. 1 / 1047.3486 for Jupiter).</param>
	/// <param name="perturbingSemiMajorAxis">The semi-major axis a_p of the perturbing planet in AU (must be greater than semiMajorAxis).</param>
	/// <param name="perturbingEccentricity">The numerical eccentricity e_p of the perturbing planet's orbit (default is 0.0489 for Jupiter).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN for invalid or non-hierarchical parameters.</returns>
	/// <remarks>
	/// Calculated using quadrupole secular perturbation theory (Innanen et al., 1997 / Kinoshita &amp; Nakai, 1999):
	/// T_Kozai = (M_Sun / M_p) · (P_p² / P) · (1 - e_p²)^(3/2), where P_p and P are orbital periods in years.
	/// Valid when the system is hierarchical (a &lt; a_p) and outside strong mean-motion resonances.
	/// </remarks>
	public static double CalculateKozaiLidovPeriodYears(double semiMajorAxis, double perturbingMassSolarMasses, double perturbingSemiMajorAxis, double perturbingEccentricity = 0.0)
	{
		// Validate input parameters: semi-major axes and perturbing mass must be positive, e_p in [0, 1)
		if (semiMajorAxis <= 0.0 || perturbingSemiMajorAxis <= 0.0 || perturbingMassSolarMasses <= 0.0 || perturbingEccentricity is < 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		// Ensure hierarchical setup: inner perturbed body (a < a_p)
		if (semiMajorAxis >= perturbingSemiMajorAxis)
		{
			return double.NaN;
		}
		// Orbital period of the perturbed inner body in years: P = a^(3/2)
		double periodYears = CalculateSiderealOrbitalPeriod(semiMajorAxis: semiMajorAxis);
		// Orbital period of the perturbing outer planet in years: P_p = a_p^(3/2)
		double perturbingPeriodYears = CalculateSiderealOrbitalPeriod(semiMajorAxis: perturbingSemiMajorAxis);
		// Eccentricity factor: (1 - e_p²)^(3/2)
		double eccentricityFactor = Math.Pow(x: 1.0 - (perturbingEccentricity * perturbingEccentricity), y: 1.5);
		// T_Kozai = (M_Sun / M_p) · (P_p² / P) · (1 - e_p²)^(3/2)
		double massRatio = 1.0 / perturbingMassSolarMasses;
		double periodRatio = perturbingPeriodYears * perturbingPeriodYears / periodYears;
		return massRatio * periodRatio * eccentricityFactor;
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Mercury as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the inner object in AU (must be less than Mercury's semi-major axis ~0.387 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~0.387 AU.</returns>
	/// <remarks>Uses Mercury parameters: M_Mercury = 1 / 6023600.0 M_Sun, a_Mercury = 0.38709893 AU, e_Mercury = 0.20563069.</remarks>
	public static double CalculateMercuryKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Mercury parameters
		const double mercuryMassSolarMasses = 1.0 / 6023600.0;
		const double mercurySemiMajorAxisAu = 0.38709893;
		const double mercuryEccentricity = 0.20563069;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: mercuryMassSolarMasses, perturbingSemiMajorAxis: mercurySemiMajorAxisAu, perturbingEccentricity: mercuryEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Venus as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the inner object in AU (must be less than Venus's semi-major axis ~0.723 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~0.723 AU.</returns>
	/// <remarks>Uses Venus parameters: M_Venus = 1 / 408523.71 M_Sun, a_Venus = 0.72333199 AU, e_Venus = 0.00677323.</remarks>
	public static double CalculateVenusKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Venus parameters
		const double venusMassSolarMasses = 1.0 / 408523.71;
		const double venusSemiMajorAxisAu = 0.72333199;
		const double venusEccentricity = 0.00677323;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: venusMassSolarMasses, perturbingSemiMajorAxis: venusSemiMajorAxisAu, perturbingEccentricity: venusEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Earth (Earth+Moon system) as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the inner object in AU (must be less than Earth's semi-major axis ~1.000 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~1.000 AU.</returns>
	/// <remarks>Uses Earth+Moon parameters: M_EarthMoon = 1 / 328900.56 M_Sun, a_Earth = 1.00000011 AU, e_Earth = 0.01671022.</remarks>
	public static double CalculateEarthKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Earth+Moon system parameters
		const double earthMassSolarMasses = 1.0 / 328900.56;
		const double earthSemiMajorAxisAu = 1.00000011;
		const double earthEccentricity = 0.01671022;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: earthMassSolarMasses, perturbingSemiMajorAxis: earthSemiMajorAxisAu, perturbingEccentricity: earthEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Mars as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the inner object in AU (must be less than Mars's semi-major axis ~1.524 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~1.524 AU.</returns>
	/// <remarks>Uses Mars parameters: M_Mars = 1 / 3098708.0 M_Sun, a_Mars = 1.52366231 AU, e_Mars = 0.09341233.</remarks>
	public static double CalculateMarsKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Mars parameters
		const double marsMassSolarMasses = 1.0 / 3098708.0;
		const double marsSemiMajorAxisAu = 1.52366231;
		const double marsEccentricity = 0.09341233;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: marsMassSolarMasses, perturbingSemiMajorAxis: marsSemiMajorAxisAu, perturbingEccentricity: marsEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Jupiter as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the asteroid or comet in AU (must be less than Jupiter's semi-major axis ~5.203 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years (typically ~10⁴ to 10⁵ years for main-belt asteroids).</returns>
	/// <remarks>Uses Jupiter parameters: M_J = 1 / 1047.3486 M_Sun, a_J = 5.20336301 AU, e_J = 0.0489.</remarks>
	public static double CalculateJupiterKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Jupiter parameters
		const double jupiterMassSolarMasses = 1.0 / 1047.3486;
		const double jupiterSemiMajorAxisAu = 5.20336301;
		const double jupiterEccentricity = 0.0489;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: jupiterMassSolarMasses, perturbingSemiMajorAxis: jupiterSemiMajorAxisAu, perturbingEccentricity: jupiterEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Saturn as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU (must be less than Saturn's semi-major axis ~9.537 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~9.537 AU.</returns>
	/// <remarks>Uses Saturn parameters: M_Saturn = 1 / 3497.898 M_Sun, a_Saturn = 9.53707032 AU, e_Saturn = 0.05415060.</remarks>
	public static double CalculateSaturnKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Saturn parameters
		const double saturnMassSolarMasses = 1.0 / 3497.898;
		const double saturnSemiMajorAxisAu = 9.53707032;
		const double saturnEccentricity = 0.05415060;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: saturnMassSolarMasses, perturbingSemiMajorAxis: saturnSemiMajorAxisAu, perturbingEccentricity: saturnEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Uranus as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU (must be less than Uranus's semi-major axis ~19.191 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~19.191 AU.</returns>
	/// <remarks>Uses Uranus parameters: M_Uranus = 1 / 22902.98 M_Sun, a_Uranus = 19.19126393 AU, e_Uranus = 0.04716771.</remarks>
	public static double CalculateUranusKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Uranus parameters
		const double uranusMassSolarMasses = 1.0 / 22902.98;
		const double uranusSemiMajorAxisAu = 19.19126393;
		const double uranusEccentricity = 0.04716771;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: uranusMassSolarMasses, perturbingSemiMajorAxis: uranusSemiMajorAxisAu, perturbingEccentricity: uranusEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation period in Julian years assuming Neptune as the primary perturbing planet.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU (must be less than Neptune's semi-major axis ~30.069 AU).</param>
	/// <returns>The secular Kozai-Lidov period in Julian years. Returns double.NaN if semiMajorAxis >= ~30.069 AU.</returns>
	/// <remarks>Uses Neptune parameters: M_Neptune = 1 / 19412.24 M_Sun, a_Neptune = 30.06896348 AU, e_Neptune = 0.00858587.</remarks>
	public static double CalculateNeptuneKozaiLidovPeriodYears(double semiMajorAxis)
	{
		// Neptune parameters
		const double neptuneMassSolarMasses = 1.0 / 19412.24;
		const double neptuneSemiMajorAxisAu = 30.06896348;
		const double neptuneEccentricity = 0.00858587;
		return CalculateKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis, perturbingMassSolarMasses: neptuneMassSolarMasses, perturbingSemiMajorAxis: neptuneSemiMajorAxisAu, perturbingEccentricity: neptuneEccentricity);
	}

	/// <summary>Calculates the secular Kozai-Lidov oscillation periods of a planetoid relative to all eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the planetoid in AU.</param>
	/// <returns>A list of KozaiLidovPeriod structures containing the planet name, planet semi-major axis, and calculated Kozai-Lidov period in Julian years.</returns>
	/// <remarks>The Kozai-Lidov period is evaluated for each planet where the planetoid orbits inner to the planet (a &lt; a_planet). Returns double.NaN for planets where the hierarchical condition (a &lt; a_planet) is not satisfied.</remarks>
	public static List<KozaiLidovPeriod> CalculateKozaiLidovPeriods(double semiMajorAxis)
	{
		List<KozaiLidovPeriod> results =
		[
			new(PlanetName: "Mercury", PlanetSemiMajorAxisAu:  0.38709893, KozaiPeriodYears: CalculateMercuryKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Venus",   PlanetSemiMajorAxisAu:  0.72333199, KozaiPeriodYears: CalculateVenusKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Earth",   PlanetSemiMajorAxisAu:  1.00000011, KozaiPeriodYears: CalculateEarthKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Mars",    PlanetSemiMajorAxisAu:  1.52366231, KozaiPeriodYears: CalculateMarsKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Jupiter", PlanetSemiMajorAxisAu:  5.20336301, KozaiPeriodYears: CalculateJupiterKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Saturn",  PlanetSemiMajorAxisAu:  9.53707032, KozaiPeriodYears: CalculateSaturnKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Uranus",  PlanetSemiMajorAxisAu: 19.19126393, KozaiPeriodYears: CalculateUranusKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis)),
			new(PlanetName: "Neptune", PlanetSemiMajorAxisAu: 30.06896348, KozaiPeriodYears: CalculateNeptuneKozaiLidovPeriodYears(semiMajorAxis: semiMajorAxis))
		];
		return results;
	}

	#endregion

	#region

	/// <summary>Calculates the Laplace coefficient b_{3/2}^{(1)}(α) using numerical Simpson integration.</summary>
	/// <param name="alpha">The ratio of semi-major axes α = a / a_planet (0 lower than α lower than 1).</param>
	/// <returns>The Laplace coefficient value b_{3/2}^{(1)}(α).</returns>
	/// <remarks>Calculated as (2/π) · ∫₀^π [cos(ψ) / (1 - 2α·cos(ψ) + α²)^(3/2)] dψ.</remarks>
	public static double CalculateLaplaceCoefficientB32_1(double alpha)
	{
		if (alpha is <= 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		const int steps = 360;
		double dPsi = Math.PI / steps;
		double sum = 0.0;
		for (int i = 0; i < steps; i++)
		{
			double psi = (i + 0.5) * dPsi;
			double denom = Math.Pow(x: 1.0 - (2.0 * alpha * Math.Cos(d: psi)) + (alpha * alpha), y: 1.5);
			sum += Math.Cos(d: psi) / denom;
		}
		return 2.0 / Math.PI * sum * dPsi;
	}

	/// <summary>Calculates the Laplace coefficient b_{3/2}^{(2)}(α) using numerical Simpson integration.</summary>
	/// <param name="alpha">The ratio of semi-major axes α = a / a_planet (0 lower than α lower than 1).</param>
	/// <returns>The Laplace coefficient value b_{3/2}^{(2)}(α).</returns>
	/// <remarks>Calculated as (2/π) · ∫₀^π [cos(2ψ) / (1 - 2α·cos(ψ) + α²)^(3/2)] dψ.</remarks>
	public static double CalculateLaplaceCoefficientB32_2(double alpha)
	{
		if (alpha is <= 0.0 or >= 1.0)
		{
			return double.NaN;
		}
		const int steps = 360;
		double dPsi = Math.PI / steps;
		double sum = 0.0;
		for (int i = 0; i < steps; i++)
		{
			double psi = (i + 0.5) * dPsi;
			double denom = Math.Pow(x: 1.0 - (2.0 * alpha * Math.Cos(d: psi)) + (alpha * alpha), y: 1.5);
			sum += Math.Cos(d: 2.0 * psi) / denom;
		}
		return 2.0 / Math.PI * sum * dPsi;
	}

	/// <summary>Approximates the forced eccentricity e_forced and forced inclination i_forced according to secular Laplace-Lagrange perturbation theory (Jupiter and Saturn perturbations).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the asteroid in AU (0.5 AU lower than a lower than 4.8 AU).</param>
	/// <param name="osculatingEccentricity">The osculating numerical eccentricity e.</param>
	/// <param name="osculatingInclinationDegrees">The osculating inclination i in degrees.</param>
	/// <returns>A LaplaceLagrangeForcedElements structure containing forced/free orbital parameters and secular precession frequency A.</returns>
	/// <remarks>Uses the classical linear secular Laplace-Lagrange model perturbed by Jupiter (j=5) and Saturn (j=6): e_forced ≈ ∑ (ν_j / (A - g_j)) · e_j, where A is the secular perihelion precession frequency of the asteroid. Free eccentricity e_free and free inclination i_free approximate the proper elements (e_p, i_p).</remarks>
	public static LaplaceLagrangeForcedElements CalculateLaplaceLagrangeForcedElements(double semiMajorAxis, double osculatingEccentricity, double osculatingInclinationDegrees)
	{
		// Validate semi-major axis range for main-belt / inner solar system asteroids
		const double jupiterSemiMajorAxis = 5.20336301;
		const double saturnSemiMajorAxis = 9.53707032;
		if (semiMajorAxis is <= 0.0 or >= jupiterSemiMajorAxis)
		{
			return new LaplaceLagrangeForcedElements(ForcedEccentricity: double.NaN, ForcedInclinationDegrees: double.NaN, FreeEccentricity: double.NaN, FreeInclinationDegrees: double.NaN, SecularPrecessionFrequencyArcsecPerYear: double.NaN);
		}
		// Planetary parameters: Jupiter (5) and Saturn (6)
		const double m5 = 1.0 / 1047.3486;
		const double m6 = 1.0 / 3497.898;
		const double e5 = 0.0489;
		const double e6 = 0.0565;
		// arcsec/yr (Jupiter fundamental frequency)
		const double g5 = 4.257;
		// arcsec/yr (Saturn fundamental frequency)
		const double g6 = 28.245;
		// arcsec/yr (Saturn nodal frequency)
		const double s6 = -26.345;
		// Saturn inclination in radians
		double i6Rad = double.DegreesToRadians(degrees: 2.485);
		// Ratios of semi-major axes α
		double alpha5 = semiMajorAxis / jupiterSemiMajorAxis;
		double alpha6 = semiMajorAxis / saturnSemiMajorAxis;
		// Mean motion n in arcsec/year (1 rad = 206,264.806247 arcsec)
		double nArcsecPerYear = CalculateMeanMotionRad(semiMajorAxis: semiMajorAxis) * 206_264.806247;
		// Laplace coefficients
		double b32_1_5 = CalculateLaplaceCoefficientB32_1(alpha: alpha5);
		double b32_1_6 = CalculateLaplaceCoefficientB32_1(alpha: alpha6);
		double b32_2_5 = CalculateLaplaceCoefficientB32_2(alpha: alpha5);
		double b32_2_6 = CalculateLaplaceCoefficientB32_2(alpha: alpha6);
		// Asteroid secular precession frequency A (arcsec/year)
		double frequencyA = 0.75 * nArcsecPerYear * ((m5 * alpha5 * b32_1_5) + (m6 * alpha6 * b32_1_6));
		double frequencyB = -frequencyA;
		// Coupling coefficients ν_5, ν_6 and μ_6
		double nu5 = 0.75 * nArcsecPerYear * m5 * alpha5 * b32_2_5;
		double nu6 = 0.75 * nArcsecPerYear * m6 * alpha6 * b32_2_6;
		double mu6 = -0.75 * nArcsecPerYear * m6 * alpha6 * b32_1_6;
		// Forced eccentricity vector components
		double ef5 = (frequencyA != g5) ? nu5 / (frequencyA - g5) * e5 : 0.0;
		double ef6 = (frequencyA != g6) ? nu6 / (frequencyA - g6) * e6 : 0.0;
		double forcedEccentricity = Math.Abs(value: ef5 + ef6);
		// Forced inclination (radians and degrees)
		double if6Rad = (frequencyB != s6) ? mu6 / (frequencyB - s6) * Math.Sin(a: i6Rad) : 0.0;
		double forcedInclinationDegrees = double.RadiansToDegrees(radians: Math.Abs(value: if6Rad));
		// Free elements (proper element approximations): e_free ≈ |e - e_forced|, i_free ≈ |i - i_forced|
		double freeEccentricity = Math.Max(val1: 0.0, val2: osculatingEccentricity - forcedEccentricity);
		double freeInclinationDegrees = Math.Max(val1: 0.0, val2: osculatingInclinationDegrees - forcedInclinationDegrees);
		return new LaplaceLagrangeForcedElements(ForcedEccentricity: forcedEccentricity, ForcedInclinationDegrees: forcedInclinationDegrees, FreeEccentricity: freeEccentricity, FreeInclinationDegrees: freeInclinationDegrees, SecularPrecessionFrequencyArcsecPerYear: frequencyA);
	}

	#endregion

	#region Secular resonance analysis

	/// <summary>Analyzes proximity to the primary secular resonances ν6 (Saturn perihelion precession) and ν16 (Saturn nodal precession).</summary>
	/// <param name="semiMajorAxis">The semi-major axis a of the object in AU.</param>
	/// <param name="osculatingEccentricity">The osculating numerical eccentricity e.</param>
	/// <param name="osculatingInclinationDegrees">The osculating inclination i in degrees.</param>
	/// <param name="frequencyToleranceArcsecPerYear">The frequency difference threshold in arcsec/year to classify resonance proximity (default is 3.0 arcsec/yr).</param>
	/// <returns>A SecularResonanceAnalysis structure detailing frequency offsets and secular resonance proximity flags.</returns>
	/// <remarks>
	/// The ν6 secular resonance occurs when the asteroid's perihelion precession frequency A matches Saturn's g6 frequency (~28.245 arcsec/yr).
	/// The ν16 secular resonance occurs when the asteroid's nodal precession frequency B matches Saturn's s6 frequency (~-26.345 arcsec/yr).
	/// </remarks>
	public static SecularResonanceAnalysis AnalyzeSecularResonances(double semiMajorAxis, double osculatingEccentricity, double osculatingInclinationDegrees, double frequencyToleranceArcsecPerYear = 3.0)
	{
		// Fundamental Saturn precession frequencies in arcsec/year. g6Saturn = Perihelion frequency g6; s6Saturn = Nodal frequency s6
		const double g6Saturn = 28.245;
		const double s6Saturn = -26.345;
		// Calculate Laplace-Lagrange secular frequencies A and B = -A
		LaplaceLagrangeForcedElements llElements = CalculateLaplaceLagrangeForcedElements(semiMajorAxis: semiMajorAxis, osculatingEccentricity: osculatingEccentricity, osculatingInclinationDegrees: osculatingInclinationDegrees);
		double frequencyA = llElements.SecularPrecessionFrequencyArcsecPerYear;
		if (double.IsNaN(d: frequencyA))
		{
			return new SecularResonanceAnalysis(SecularPerihelionFrequencyA: double.NaN, SecularNodalFrequencyB: double.NaN, DeltaNu6ArcsecPerYear: double.NaN, DeltaNu16ArcsecPerYear: double.NaN, IsInNu6Resonance: false, IsInNu16Resonance: false);
		}
		// Nodal precession frequency B = -A in linear secular theory
		double frequencyB = -frequencyA;
		// Frequency offsets to Saturn's fundamental frequencies
		double deltaNu6 = Math.Abs(value: frequencyA - g6Saturn);
		double deltaNu16 = Math.Abs(value: frequencyB - s6Saturn);
		// Proximity checks against tolerance
		bool isInNu6 = deltaNu6 <= frequencyToleranceArcsecPerYear;
		bool isInNu16 = deltaNu16 <= frequencyToleranceArcsecPerYear;
		return new SecularResonanceAnalysis(SecularPerihelionFrequencyA: frequencyA, SecularNodalFrequencyB: frequencyB, DeltaNu6ArcsecPerYear: deltaNu6, DeltaNu16ArcsecPerYear: deltaNu16, IsInNu6Resonance: isInNu6, IsInNu16Resonance: isInNu16);
	}

	#endregion
}
