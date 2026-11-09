/*
 * File:        OrbitalArcCalculator.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods to calculate the lengths and angles of orbital arcs for elliptical orbits.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

namespace Planetoid_DB.Helpers;

/// <summary>Calculates the lengths and angular measures of orbital arcs between the ascending and descending nodes, separated by perihelion and aphelion.</summary>
/// <remarks>Calculations assume an elliptical orbit with a positive semi-major axis and eccentricity from 0 inclusive to 1 exclusive. The argument of perihelion is specified in degrees.</remarks>
internal static class OrbitalArcCalculator
{
	/// <summary>The number of radians in a full circle (2π).</summary>
	/// <remarks>This constant is used for converting between radians and degrees in orbital calculations.</remarks>
	private const double TwoPi = 2.0 * Math.PI;

	/// <summary>The number of steps used for integration.</summary>
	/// <remarks>This constant is used for numerical integration in orbital calculations.</remarks>
	private const int IntegrationSteps = 4096;

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Length of the ascending-node–perihelion–descending-node arc in astronomical units (AU).</summary>
	/// <remarks>This method calculates the length of the arc from the ascending node to the perihelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double AscendingNodePerihelionDescendingNodeArcLengthAU(double semiMajorAxis, double eccentricity, double argumentOfPerihelion)
	{
		// Calculate the lengths of the arcs and the total circumference of the ellipse.
		return CalculateArcs(a: semiMajorAxis, e: eccentricity, omegaDegrees: argumentOfPerihelion).perihel;
	}

	/// <summary>Angular length of the perihel-node arc in degrees. The complete ellipse circumference represents 360 degrees.</summary>
	/// <remarks>This method calculates the angular length of the arc from the ascending node to the perihelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double AscendingNodePerihelionDescendingNodeArcAngle(double semiMajorAxis, double eccentricity, double argumentOfPerihelion)
	{
		// Calculate the lengths of the arcs and the total circumference of the ellipse.
		(double perihel, _, double circumference) = CalculateArcs(a: semiMajorAxis, e: eccentricity, omegaDegrees: argumentOfPerihelion);
		// Return the angular length of the perihel-node arc in degrees.
		return perihel / circumference * 360.0;
	}

	/// <summary>Length of the ascending-node–aphelion–descending-node arc in astronomical units (AU).</summary>
	/// <remarks>This method calculates the length of the arc from the ascending node to the aphelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double AscendingNodeAphelionDescendingNodeArcLengthAU(double semiMajorAxis, double eccentricity, double argumentOfPerihelion)
	{
		// Calculate the lengths of the arcs and the total circumference of the ellipse.
		return CalculateArcs(a: semiMajorAxis, e: eccentricity, omegaDegrees: argumentOfPerihelion).aphel;
	}

	/// <summary>Angular length of the aphel-node arc in degrees. The complete ellipse circumference represents 360 degrees.</summary>
	/// <remarks>This method calculates the angular length of the arc from the ascending node to the aphelion and then to the descending node, based on the semi-major axis, eccentricity, and argument of perihelion.</remarks>
	public static double AscendingNodeAphelionDescendingNodeArcAngle(double semiMajorAxis, double eccentricity, double argumentOfPerihelion)
	{
		// Calculate the lengths of the arcs and the total circumference of the ellipse.
		(double _, double aphel, double circumference) = CalculateArcs(a: semiMajorAxis, e: eccentricity, omegaDegrees: argumentOfPerihelion);
		// Return the angular length of the aphel-node arc in degrees.
		return aphel / circumference * 360.0;
	}

	/// <summary>
	/// Calculates the perihelion and aphelion arc lengths and the circumference of an elliptical orbit.
	/// </summary>
	/// <param name="a">The positive semimajor axis of the orbit.</param>
	/// <param name="e">The orbital eccentricity, in the range [0, 1).</param>
	/// <param name="omegaDegrees">The argument of perihelion, in degrees.</param>
	/// <returns>A tuple containing the perihelion arc length, aphelion arc length, and complete circumference.</returns>
	/// <exception cref="ArgumentOutOfRangeException">The semimajor axis is not positive, or the eccentricity is outside the range [0, 1).</exception>
	private static (double perihel, double aphel, double circumference) CalculateArcs(double a, double e, double omegaDegrees)
	{
		// Validate input parameters.
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value: a, other: 0.0);
		// Validate eccentricity is in the range [0, 1).
		if (e is < 0.0 or >= 1.0)
		{
			// Log a warning message if eccentricity is out of range.
			logger.Warn(message: "Eccentricity is out of range: {0}. Must be in the range [0, 1).", e);
			// Throw an exception if eccentricity is out of range.
			throw new ArgumentOutOfRangeException(paramName: nameof(e));
		}
		// Convert argument of perihelion to radians.
		double omega = omegaDegrees * Math.PI / 180.0;
		// True anomalies of the ascending and descending nodes.
		double nuAscending = -omega;
		double nuDescending = Math.PI - omega;
		// Convert true anomalies into eccentric anomalies.
		double eAscending = TrueToEccentricAnomaly(nu: nuAscending, e: e);
		double eDescending = TrueToEccentricAnomaly(nu: nuDescending, e: e);
		// Sort the eccentric anomalies.
		double lower = Math.Min(val1: eAscending, val2: eDescending);
		double upper = Math.Max(val1: eAscending, val2: eDescending);
		// Perihel arc: interval passing through E = 0.
		double perihelArc = IntegrateArc(start: upper, end: TwoPi, a: a, e: e) + IntegrateArc(start: 0.0, end: lower, a: a, e: e);
		// Aphel arc: complementary interval passing through E = PI.
		double aphelArc = IntegrateArc(start: lower, end: upper, a: a, e: e);
		// Complete ellipse circumference.
		double circumference = IntegrateArc(start: 0.0, end: TwoPi, a: a, e: e);
		// Return the calculated arc lengths and circumference.
		return (perihel: perihelArc, aphel: aphelArc, circumference);
	}

	/// <summary>Converts true anomaly to eccentric anomaly. Result is normalized to [0, 2 PI).</summary>
	/// <remarks>This method converts the true anomaly (nu) to the eccentric anomaly (E) using the relationship between the two anomalies for elliptical orbits. The result is normalized to the range [0, 2π).</remarks>
	private static double TrueToEccentricAnomaly(double nu, double e)
	{
		// Calculate the sine and cosine of the eccentric anomaly using the true anomaly and eccentricity.
		double sinE = Math.Sqrt(d: 1.0 - (e * e)) * Math.Sin(a: nu);
		double cosE = e + Math.Cos(d: nu);
		// Calculate the eccentric anomaly using the arctangent of the sine and cosine values.
		double E = Math.Atan2(y: sinE, x: cosE);
		// Normalize the eccentric anomaly to the range [0, 2π).
		if (E < 0.0)
		{
			E += TwoPi;
		}
		// Return the calculated eccentric anomaly.
		return E;
	}

	/// <summary>Numerically integrates the ellipse arc length between two eccentric anomalies using Simpson's rule.</summary>
	/// <remarks>This method uses Simpson's rule to numerically integrate the arc length of an elliptical orbit between two eccentric anomalies (start and end). The semi-major axis (a) and eccentricity (e) are used in the calculation.</remarks>
	private static double IntegrateArc(double start, double end, double a, double e)
	{
		// Validate input parameters.
		if (end <= start)
		{
			return 0.0;
		}
		// Validate semi-major axis is positive.
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value: a);
		// Validate eccentricity is within the valid range.
		if (e is < 0 or >= 1)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(e));
		}
		// Set the number of integration steps for Simpson's rule.
		int n = IntegrationSteps;
		// Calculate the step size for integration.
		double h = (end - start) / n;
		// Initialize the sum for Simpson's rule.
		double sum = 0.0;
		// Perform the integration using Simpson's rule.
		for (int i = 0; i <= n; i++)
		{
			// Calculate the current eccentric anomaly for this step.
			double E = start + (i * h);
			// Calculate the value of the integrand for this eccentric anomaly.
			double value = a * Math.Sqrt(d: 1.0 - (e * e * Math.Pow(x: Math.Cos(d: E), y: 2)));
			// Apply Simpson's rule coefficients based on the index of the step.
			if (i == 0 || i == n)
			{
				// For the first and last terms, add the value directly to the sum.
				sum += value;
			}
			// For even indices (excluding the first and last), multiply the value by 2 and add to the sum.
			else if (i % 2 == 0)
			{
				sum += 2.0 * value;
			}
			// For odd indices, multiply the value by 4 and add to the sum.
			else
			{
				sum += 4.0 * value;
			}
		}
		// Return the final result of the integration using Simpson's rule.
		return sum * h / 3.0;
	}
}
