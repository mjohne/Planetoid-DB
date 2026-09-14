/*
 * File:        OrbitalResonancesOfAllMinorPlanetsForm.ResonanceResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for finding orbital resonances of all minor planets relative to the 8 known solar system planets.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents the form for calculating and displaying orbital resonances of all minor planets relative to the 8 known solar system planets.</summary>
/// <remarks>This class contains the logic for computing and displaying the orbital resonances of minor planets in relation to the major planets.</remarks>
internal partial class OrbitalResonancesOfAllMinorPlanetsForm
{
	/// <summary>Represents a single resonance result combining a planetoid designation with its resonance data.</summary>
	/// <param name="PlanetoidName">The readable designation or packed index of the planetoid.</param>
	/// <param name="Resonance">The computed orbital resonance data.</param>
	/// <remarks>This record is used to store the results of the resonance calculations for each planetoid.</remarks>
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
	private record ResonanceResult(string PlanetoidName, DerivedElements.OrbitalResonance Resonance)
	{
		/// <summary>Provides a string representation of the resonance result for debugging purposes.</summary>
		/// <remarks>This property is used by the debugger to display the resonance result in a readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}