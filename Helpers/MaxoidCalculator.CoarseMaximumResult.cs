/*
 * File:        MaxoidCalculator.CoarseMaximumResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.
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

/// <summary>Represents the coarse-search maximum used as the starting point for local refinement in the MAXOID calculation.</summary>
/// <remarks>This partial class contains the <see cref="CoarseMaximumResult"/> record, which holds the results of the coarse grid search phase used as the starting point for local refinement.</remarks>
internal partial class MaxoidCalculator
{
	/// <summary>Represents the coarse-search maximum used as the starting point for local refinement.</summary>
	/// <param name="MaxDistanceSquared">Squared distance at the best coarse grid sample.</param>
	/// <param name="BestF1">Best true anomaly on orbit 1 from the coarse grid.</param>
	/// <param name="BestF2">Best true anomaly on orbit 2 from the coarse grid.</param>
	/// <return>The coarse maximum result containing the squared distance and best true anomalies from the coarse grid search.</return>
	/// <remarks>This record struct is used to encapsulate the results of the coarse grid search phase in the MAXOID calculation, providing a starting point for local refinement to find the maximum orbit intersection distance.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private record struct CoarseMaximumResult(double MaxDistanceSquared, double BestF1, double BestF2)
	{
		/// <summary>Returns a string representation of the coarse maximum result for debugging purposes.</summary>
		/// <returns>A string representation of the coarse maximum result.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the CoarseMaximumResult struct in a human-readable format.</remarks>
		private readonly string DebuggerDisplay => ToString() ?? string.Empty;
	}
}