/*
 * File:        VisibilityCalculator.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Computes brightness and visibility of a minor planet.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Services;

/// <summary>Computes the apparent magnitude of a minor planet and evaluates visibility criteria.</summary>
internal static class VisibilityCalculator
{
	/// <summary>Computes the phase angle Sun–object–observer.</summary>
	/// <param name="heliocentricDistanceAu">The distance Sun–object [AU].</param>
	/// <param name="observerDistanceAu">The distance observer–object [AU].</param>
	/// <param name="sunObserverDistanceAu">The distance Sun–observer [AU].</param>
	/// <returns>The phase angle [°] in [0°, 180°].</returns>
	/// <remarks>The phase angle is the angle between the Sun and the observer as seen from the object. It is computed using the law of cosines based on the distances between the Sun, object, and observer.</remarks>
	public static double PhaseAngleDegrees(double heliocentricDistanceAu, double observerDistanceAu, double sunObserverDistanceAu)
	{
		// The distance from the Sun to the object
		double r = heliocentricDistanceAu;
		// The distance from the observer to the object
		double d = observerDistanceAu;
		// Use the law of cosines to compute the cosine of the phase angle
		double cos = ((r * r) + (d * d) - (sunObserverDistanceAu * sunObserverDistanceAu)) / (2.0 * r * d);
		// Clamp the cosine value to the valid range [-1, 1] to avoid NaN results from Math.Acos due to floating-point inaccuracies
		return Math.Acos(d: Math.Clamp(value: cos, min: -1.0, max: 1.0)) * AstronomicalConstants.RadiansToDegrees;
	}

	/// <summary>Computes the apparent visual magnitude according to the IAU H,G system (Bowell et al., 1989).</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H [mag].</param>
	/// <param name="slopeParameter">The slope parameter G.</param>
	/// <param name="heliocentricDistanceAu">The distance Sun–object [AU].</param>
	/// <param name="observerDistanceAu">The distance observer–object [AU].</param>
	/// <param name="phaseAngleDegrees">The phase angle [°].</param>
	/// <returns>The apparent magnitude [mag]; <see cref="double.NaN"/> if H is unknown.</returns>
	/// <remarks>This method implements the IAU H,G system for computing the apparent visual magnitude of a minor planet, based on its absolute magnitude, slope parameter, distances, and phase angle.</remarks>
	public static double ApparentMagnitude(double absoluteMagnitude, double slopeParameter, double heliocentricDistanceAu, double observerDistanceAu, double phaseAngleDegrees)
	{
		// If the absolute magnitude is not finite (e.g., NaN or Infinity), return NaN to indicate that the apparent magnitude cannot be computed
		if (!double.IsFinite(d: absoluteMagnitude))
		{
			return double.NaN;
		}
		// Compute the tangent of half the phase angle in radians
		double tanHalf = Math.Tan(a: phaseAngleDegrees * AstronomicalConstants.DegreesToRadians / 2.0);
		// Compute the first phase function phi1 using the phase angle
		double phi1 = Math.Exp(d: -3.33 * Math.Pow(x: tanHalf, y: 0.63));
		// Compute the second phase function phi2 using the phase angle
		double phi2 = Math.Exp(d: -1.87 * Math.Pow(x: tanHalf, y: 1.22));
		// Compute the combined phase function using the slope parameter G
		double phase = ((1.0 - slopeParameter) * phi1) + (slopeParameter * phi2);
		// Compute the phase term, ensuring that we do not take the logarithm of a non-positive number
		double phaseTerm = phase > 0.0 ? 2.5 * Math.Log10(d: phase) : -99.0;
		// Compute the apparent magnitude using the formula: m = H + 5 * log10(r * d) - 2.5 * log10(phase)
		return absoluteMagnitude + (5.0 * Math.Log10(d: heliocentricDistanceAu * observerDistanceAu)) - phaseTerm;
	}

	/// <summary>Determines whether all visibility criteria are fulfilled.</summary>
	/// <param name="altitudeDegrees">The altitude of the object [°].</param>
	/// <param name="sunAltitudeDegrees">The altitude of the Sun [°].</param>
	/// <param name="apparentMagnitude">The apparent magnitude [mag] (<see cref="double.NaN"/> if unknown).</param>
	/// <param name="moonSeparationDegrees">The angular distance to the Moon [°].</param>
	/// <param name="criteria">The visibility criteria.</param>
	/// <returns><c>true</c> if the object is observable; otherwise, <c>false</c>.</returns>
	/// <remarks>If the magnitude is unknown, the brightness criterion is ignored.</remarks>
	public static bool IsVisible(double altitudeDegrees, double sunAltitudeDegrees, double apparentMagnitude, double moonSeparationDegrees, VisibilityCriteria criteria)
	{
		// Validate arguments
		ArgumentNullException.ThrowIfNull(argument: criteria);
		// The object must always be above the horizon, in addition to the requested minimum altitude
		// The Sun must be below the requested maximum altitude (e.g. -6° for astronomical twilight)
		// The apparent magnitude must be less than or equal to the requested faintest magnitude (if known)
		// The angular distance to the Moon must be greater than or equal to the requested minimum separation
		return altitudeDegrees > 0.0
		 && altitudeDegrees >= criteria.MinimumAltitudeDegrees
		 && sunAltitudeDegrees <= criteria.MaximumSunAltitudeDegrees
		 && (criteria.FaintestMagnitude is not double limit || !double.IsFinite(d: apparentMagnitude) || apparentMagnitude <= limit)
		 && !(moonSeparationDegrees < criteria.MinimumMoonSeparationDegrees);
	}
}
