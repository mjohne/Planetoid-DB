/*
 * File:        ScatterplotsForm.ScatterDefinition.cs
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

/// <summary>Represents the form that displays a scatter plot of two selected orbital elements or derived properties for all planetoids in the database.</summary>
/// <remarks>The form allows users to select which orbital elements or derived properties to plot on the X and Y axes, and it visualizes the data for all planetoids in the database.</remarks>
internal partial class ScatterplotsForm
{
	/// <summary>Represents one selectable scatter-plot axis definition.</summary>
	/// <param name="DisplayName">The user-facing name of the orbital element or property.</param>
	/// <param name="AxisLabel">The axis label for the chart.</param>
	/// <param name="UnitSuffix">The optional unit suffix used in formatted values.</param>
	/// <param name="ValueSelector">The callback used to extract the numeric value from a raw MPCORB line.</param>
	/// <returns>A new instance of the <see cref="ScatterDefinition"/> record.</returns>
	/// <remarks>The definition centralises presentation metadata and parsing logic for one scatter-plot axis.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed record ScatterDefinition(string DisplayName, string AxisLabel, string UnitSuffix, Func<string, double?> ValueSelector)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The scatter definition name.</returns>
		/// <remarks>The method overrides the default ToString() implementation to return the DisplayName, which is used for display in ComboBox controls.</remarks>
		public override string ToString()
		{
			return DisplayName;
		}

		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}