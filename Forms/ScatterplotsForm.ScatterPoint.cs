/*
 * File:        ScatterplotsForm.ScatterPoint.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Displays a scatter plot of two selected orbital elements or derived properties for all planetoids in the database.
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

/// <summary>Displays a scatter plot of two selected orbital elements or derived properties for all planetoids in the database.</summary>
/// <remarks>The form plots each planetoid as a point with user-selected X-axis and Y-axis orbital elements. The chart and a tabular ListView are shown side by side. Users can optionally request live updates while the background data-collection operation is running.</remarks>
internal partial class ScatterplotsForm
{
	/// <summary>Represents one plotted scatter-plot data point.</summary>
	/// <param name="X">The X-axis value of the data point.</param>
	/// <param name="Y">The Y-axis value of the data point.</param>
	/// <remarks>Each data point corresponds to one planetoid whose X and Y values were both successfully parsed.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
	private readonly record struct ScatterPoint(double X, double Y)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}