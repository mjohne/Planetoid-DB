/*
 * File:        MoidCalculator.MoidResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.
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

/// <summary>Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This partial class contains the <see cref="MoidResult"/> record, which holds the MOID result for a minor planet relative to a specific solar system planet.</remarks>
internal partial class MoidCalculator
{
	/// <summary>Represents the MOID result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="MoidAu">The Minimum Orbit Intersection Distance in AU.</param>
	/// <return>The MOID result.</return>
	/// <remarks>The MOID is the closest geometric approach distance between the two osculating orbits, independent of the bodies' actual positions at any epoch.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record MoidResult(string PlanetName, double MoidAu)
	{
		/// <summary>Returns a string representation of the MOID result for debugging purposes.</summary>
		/// <returns>A string representation of the MOID result for debugging purposes.</returns>
		/// <remarks>This method is used by the debugger to display a concise summary of the MOID result.</remarks>
		private string DebuggerDisplay => ToString();
	}
}