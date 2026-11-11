/*
 * File:        DerivedElements.KozaiLidovPeriod.cs
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

/// <summary>Provides derived orbital elements for a planetoid, including Kozai–Lidov period data.</summary>
internal partial class DerivedElements
{
	/// <summary>Represents the Kozai-Lidov secular oscillation period for a planetoid perturbed by a specific solar system planet.</summary>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal readonly record struct KozaiLidovPeriod(string PlanetName, double PlanetSemiMajorAxisAu, double KozaiPeriodYears)
	{
		/// <summary>Returns a string representation of the Kozai-Lidov period for debugging purposes.</summary>
		/// <returns>A string representation of the Kozai-Lidov period.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the Kozai-Lidov period for debugging.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
