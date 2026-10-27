/*
 * File:        DerivedElements.PhaEvaluation.cs
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

internal partial class DerivedElements
{
	/// <summary>Represents the evaluation result of a Potentially Hazardous Asteroid (PHA) assessment.</summary>
	/// <param name="IsPha">Indicates whether the object meets all criteria to be classified as a PHA.</param>
	/// <param name="IsMoidCritical">Indicates whether the Earth MOID is within the critical threshold (&lt;= 0.05 AU).</param>
	/// <param name="IsMagnitudeCritical">Indicates whether the absolute magnitude is within the critical threshold (H &lt;= 22.0 mag).</param>
	/// <param name="EstimatedMinDiameterMeters">The estimated minimum diameter in meters for H = 22.0 mag (assuming standard albedo ~0.14).</param>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal readonly record struct PhaEvaluation(bool IsPha, bool IsMoidCritical, bool IsMagnitudeCritical, double EstimatedMinDiameterMeters)
	{
		/// <summary>Returns a string representation of the synodic period for debugging purposes.</summary>
		/// <returns>A string representation of the synodic period.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the synodic period for debugging.</remarks>
		private string DebuggerDisplay => ToString();
	}
}