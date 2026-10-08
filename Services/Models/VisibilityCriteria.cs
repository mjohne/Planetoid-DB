/*
 * File:        VisibilityCriteria.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents the criteria used to decide whether a minor planet is observable.
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

/// <summary>Represents the criteria used to decide whether a minor planet is observable.</summary>
/// <param name="MinimumAltitudeDegrees">Minimum altitude of the object above the horizon in degrees.</param>
/// <param name="MaximumSunAltitudeDegrees">Maximum altitude of the Sun in degrees (e.g. −12° for nautical, −18° for astronomical twilight).</param>
/// <param name="FaintestMagnitude">Faintest (largest) apparent magnitude [mag] that is still observable, or <c>null</c> to ignore the brightness.</param>
/// <param name="MinimumMoonSeparationDegrees">Minimum angular distance to the Moon in degrees.</param>
internal sealed record VisibilityCriteria(
	double MinimumAltitudeDegrees = 0.0,
	double MaximumSunAltitudeDegrees = -12.0,
	double? FaintestMagnitude = null,
	double MinimumMoonSeparationDegrees = 0.0)
{
	/// <summary>Gets the default visibility criteria (object above the horizon, Sun below −12°).</summary>
	public static VisibilityCriteria Default { get; } = new();
}
