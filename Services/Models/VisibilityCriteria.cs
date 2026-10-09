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

using System.Diagnostics;

namespace Planetoid_DB.Services;

/// <summary>Represents the criteria used to decide whether a minor planet is observable.</summary>
/// <param name="MinimumAltitudeDegrees">Minimum altitude of the object above the horizon in degrees.</param>
/// <param name="MaximumSunAltitudeDegrees">Maximum altitude of the Sun in degrees (e.g. −12° for nautical, −18° for astronomical twilight).</param>
/// <param name="FaintestMagnitude">Faintest (largest) apparent magnitude [mag] that is still observable, or <c>null</c> to ignore the brightness.</param>
/// <param name="MinimumMoonSeparationDegrees">Minimum angular distance to the Moon in degrees.</param>
/// <remarks>The <see cref="VisibilityCriteria"/> record is used to specify the criteria for determining whether a minor planet is observable, including minimum altitude, maximum Sun altitude, faintest magnitude, and minimum Moon separation.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed record VisibilityCriteria(
	double MinimumAltitudeDegrees = 0.0,
	double MaximumSunAltitudeDegrees = -12.0,
	double? FaintestMagnitude = null,
	double MinimumMoonSeparationDegrees = 0.0)
{
	/// <summary>Gets the default visibility criteria (object above the horizon, Sun below −12°).</summary>
	/// <remarks>This property provides a convenient way to obtain a standard set of visibility criteria without having to specify the parameters explicitly.</remarks>
	public static VisibilityCriteria Default { get; } = new();

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
