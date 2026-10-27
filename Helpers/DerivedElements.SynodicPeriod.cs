/*
 * File:        DerivedElements.SynodicPeriod.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides the result type for synodic period calculations.
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

internal partial class DerivedElements
{
	/// <summary>Represents the synodic period of a planetoid with a solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="PlanetPeriodYears">The orbital period of the planet in years.</param>
	/// <param name="SynodicPeriodYears">The synodic period in years; infinite when both periods are equal.</param>
	/// <remarks>This record is used to represent the synodic period between a planetoid and a solar system planet.</remarks>
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record SynodicPeriod(string PlanetName, double PlanetPeriodYears, double SynodicPeriodYears)
	{
		/// <summary>Returns a string representation of the synodic period for debugging purposes.</summary>
		/// <returns>A string representation of the synodic period.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the synodic period for debugging.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
