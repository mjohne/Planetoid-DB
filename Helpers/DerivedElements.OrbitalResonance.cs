/*
 * File:        DerivedElements.OrbitalResonance.cs
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
internal partial class DerivedElements
{
	/// <summary>Represents an orbital resonance between a planetoid and a solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="PlanetPeriod">The orbital period of the planet in years.</param>
	/// <param name="PlanetoidPeriod">The orbital period of the planetoid in years.</param>
	/// <param name="Ratio">The actual ratio of the planet's period to the planetoid's period.</param>
	/// <param name="ResonanceP">The p value in the integer resonance ratio p:q.</param>
	/// <param name="ResonanceQ">The q value in the integer resonance ratio p:q.</param>
	/// <param name="DeviationPercent">The percentage deviation of the actual ratio from the integer ratio.</param>
	/// <remarks>This record is used to represent an orbital resonance between a planetoid and a solar system planet.</remarks>
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
	internal record OrbitalResonance(string PlanetName, double PlanetPeriod, double PlanetoidPeriod, double Ratio, int ResonanceP, int ResonanceQ, double DeviationPercent)
	{
		/// <summary>Returns a string representation of the orbital resonance for debugging purposes.</summary>
		/// <returns>A string representation of the orbital resonance.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the orbital resonance for debugging.</remarks>
		private string DebuggerDisplay => ToString();
	}
}