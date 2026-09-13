/*
 * File:        DistributionsForm.HistogramBinResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Displays a histogram of counted planetoids for a selected orbital element or derived property.
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

namespace Planetoid_DB;

/// <summary>Represents a single histogram bin result, containing the range boundaries and the count of planetoids within that range.</summary>
/// <remarks>Histogram rows are sorted by their range start value before being displayed.</remarks>
internal partial class DistributionsForm
{
	/// <summary>Represents one counted histogram range.</summary>
	/// <param name="Start">The inclusive lower range boundary.</param>
	/// <param name="End">The exclusive upper range boundary.</param>
	/// <param name="Count">The number of planetoids inside the range.</param>
	/// <remarks>Histogram rows are sorted by their range start value before being displayed.</remarks>
	/// <returns>A new instance of the <see cref="HistogramBinResult"/> record struct.</returns>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed record HistogramBinResult(double Start, double End, int Count)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>The display text for the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
