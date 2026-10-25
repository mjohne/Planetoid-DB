/*
 * File:        MoidCalculator.CoarseMinimumResult.cs
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
/// <remarks>This partial class contains the <see cref="CoarseMinimumResult"/> record, which holds the results of the coarse grid search phase used as the starting point for local refinement.</remarks>
internal partial class MoidCalculator
{
	/// <summary>Represents the coarse-search minimum used as the starting point for local refinement.</summary>
	/// <param name="MinDistanceSquared">Squared distance at the best coarse grid sample.</param>
	/// <param name="BestF1">Best true anomaly on orbit 1 from the coarse grid.</param>
	/// <param name="BestF2">Best true anomaly on orbit 2 from the coarse grid.</param>
	/// <return>The coarse minimum result.</return>
	/// <remarks>This struct is used to return multiple values from the coarse grid search phase without needing to allocate a separate class or tuple, and is designed to be efficiently passed by value.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct CoarseMinimumResult(double MinDistanceSquared, double BestF1, double BestF2)
	{
		/// <summary>Returns a string representation of the coarse minimum result for debugging purposes.</summary>
		/// <returns>A string representation of the coarse minimum result for debugging purposes.</returns>
		/// <remarks>This method is used by the debugger to display a concise summary of the coarse minimum result.</remarks>
		private string DebuggerDisplay => ToString();
	}
}