/*
 * File:        VisibilityCalculator.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Calculates brightness and visibility of a minor planet for an observer.
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

/// <summary>Calculates brightness and visibility of a minor planet for an observer.</summary>
internal static class VisibilityCalculator
{
	/// <summary>Calculates the phase angle (Sun–object–observer).</summary>
	/// <param name="heliocentricObjectPosition">The heliocentric position of the object in AU.</param>
	/// <param name="observerToObject">The vector from the observer to the object in AU.</param>
	/// <returns>The phase angle in degrees, range [0, 180].</returns>
	public static double CalculatePhaseAngleDegrees(Vector3D heliocentricObjectPosition, Vector3D observerToObject)
		=> heliocentricObjectPosition.AngleTo(other: observerToObject) * AstronomicalConstants.RadiansToDegrees;

	/// <summary>Calculates the apparent visual magnitude with the IAU H-G magnitude system (Bowell et al. 1989).</summary>
	/// <param name="absoluteMagnitude">The absolute magnitude H in mag.</param>
	/// <param name="slopeParameter">The slope parameter G.</param>
	/// <param name="heliocentricDistanceAu">The heliocentric distance r in AU.</param>
	/// <param name="observerDistanceAu">The distance to the observer Δ in AU.</param>
	/// <param name="phaseAngleDegrees">The phase angle in degrees.</param>
	/// <returns>The apparent magnitude V in mag, or <see cref="double.NaN"/> if H is unknown.</returns>
	public static double CalculateApparentMagnitude(double absoluteMagnitude, double slopeParameter, double heliocentricDistanceAu, double observerDistanceAu, double phaseAngleDegrees)
	{
		if (!double.IsFinite(d: absoluteMagnitude))
		{
			return double.NaN;
		}
		double tanHalfAlpha = Math.Tan(a: phaseAngleDegrees * AstronomicalConstants.DegreesToRadians / 2.0);
		double phi1 = Math.Exp(d: -3.33 * Math.Pow(x: tanHalfAlpha, y: 0.63));
		double phi2 = Math.Exp(d: -1.87 * Math.Pow(x: tanHalfAlpha, y: 1.22));
		double phaseFunction = ((1.0 - slopeParameter) * phi1) + (slopeParameter * phi2);
		return absoluteMagnitude + (5.0 * Math.Log10(d: heliocentricDistanceAu * observerDistanceAu)) - (2.5 * Math.Log10(d: Math.Max(val1: phaseFunction, val2: 1e-300)));
	}

	/// <summary>Determines whether all visibility criteria are fulfilled.</summary>
	/// <param name="objectAltitudeDegrees">The altitude of the object in degrees.</param>
	/// <param name="sunAltitudeDegrees">The altitude of the Sun in degrees.</param>
	/// <param name="apparentMagnitude">The apparent magnitude (NaN if unknown).</param>
	/// <param name="moonSeparationDegrees">The angular distance to the Moon in degrees.</param>
	/// <param name="criteria">The visibility criteria.</param>
	/// <returns><c>true</c> if the object is above the horizon limit, the sky is dark enough, the object is bright enough and far enough from the Moon.</returns>
	/// <remarks>An unknown magnitude only passes if no real limiting magnitude (≥ 99 mag) is configured.</remarks>
	public static bool IsVisible(double objectAltitudeDegrees, double sunAltitudeDegrees, double apparentMagnitude, double moonSeparationDegrees, VisibilityCriteria criteria)
	{
		bool aboveHorizon = objectAltitudeDegrees > criteria.MinimumObjectAltitudeDegrees;
		bool darkSky = sunAltitudeDegrees <= criteria.MaximumSunAltitudeDegrees;
		bool brightEnough = double.IsNaN(d: apparentMagnitude) ? criteria.LimitingMagnitude >= 99.0 : apparentMagnitude <= criteria.LimitingMagnitude;
		bool farFromMoon = moonSeparationDegrees >= criteria.MinimumMoonSeparationDegrees;
		return aboveHorizon && darkSky && brightEnough && farFromMoon;
	}
}
