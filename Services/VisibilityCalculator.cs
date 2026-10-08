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
	public static double PhaseAngleDegrees(double heliocentricDistanceAu, double observerDistanceAu, double sunObserverDistanceAu)
	{
		double r = heliocentricDistanceAu;
		double d = observerDistanceAu;
		double cos = ((r * r) + (d * d) - (sunObserverDistanceAu * sunObserverDistanceAu)) / (2.0 * r * d);
		return Math.Acos(d: Math.Clamp(value: cos, min: -1.0, max: 1.0)) * AstronomicalConstants.RadiansToDegrees;
	}

	/// <summary>Computes the apparent visual magnitude according to the IAU H,G system (Bowell et al., 1989).</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H [mag].</param>
	/// <param name="slopeParameter">The slope parameter G.</param>
	/// <param name="heliocentricDistanceAu">The distance Sun–object [AU].</param>
	/// <param name="observerDistanceAu">The distance observer–object [AU].</param>
	/// <param name="phaseAngleDegrees">The phase angle [°].</param>
	/// <returns>The apparent magnitude [mag]; <see cref="double.NaN"/> if H is unknown.</returns>
	public static double ApparentMagnitude(double absoluteMagnitude, double slopeParameter, double heliocentricDistanceAu, double observerDistanceAu, double phaseAngleDegrees)
	{
		if (!double.IsFinite(d: absoluteMagnitude))
		{
			return double.NaN;
		}
		double tanHalf = Math.Tan(a: phaseAngleDegrees * AstronomicalConstants.DegreesToRadians / 2.0);
		double phi1 = Math.Exp(d: -3.33 * Math.Pow(x: tanHalf, y: 0.63));
		double phi2 = Math.Exp(d: -1.87 * Math.Pow(x: tanHalf, y: 1.22));
		double phase = ((1.0 - slopeParameter) * phi1) + (slopeParameter * phi2);
		double phaseTerm = phase > 0.0 ? 2.5 * Math.Log10(d: phase) : -99.0;
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
		ArgumentNullException.ThrowIfNull(argument: criteria);
		// The object must always be above the horizon, in addition to the requested minimum altitude
		if (altitudeDegrees <= 0.0 || altitudeDegrees < criteria.MinimumAltitudeDegrees)
		{
			return false;
		}
		if (sunAltitudeDegrees > criteria.MaximumSunAltitudeDegrees)
		{
			return false;
		}
		if (criteria.FaintestMagnitude is double limit && double.IsFinite(d: apparentMagnitude) && apparentMagnitude > limit)
		{
			return false;
		}
		return !(moonSeparationDegrees < criteria.MinimumMoonSeparationDegrees);
	}
}
