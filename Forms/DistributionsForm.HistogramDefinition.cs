/*
 * File:        DistributionsForm.HistogramDefinition.cs
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

/// <summary>Represents a histogram definition for a specific orbital element or derived property.</summary>
/// <remarks>This class is used to define the available histogram definitions that can be selected in the DistributionsForm.</remarks>
internal partial class DistributionsForm
{
	/// <summary>Represents one selectable histogram definition.</summary>
	/// <param name="DisplayName">The user-facing name of the orbital element or property.</param>
	/// <param name="AxisLabel">The x-axis label for the chart.</param>
	/// <param name="UnitSuffix">The optional unit suffix used in formatted values.</param>
	/// <param name="StepOptions">The meaningful step sizes offered for the definition.</param>
	/// <param name="ValueSelector">The callback used to extract the numeric value from a raw MPCORB line.</param>
	/// <returns>The histogram definition instance.</returns>
	/// <remarks>The definition centralizes presentation metadata and parsing logic for one histogram mode.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed record HistogramDefinition(string DisplayName, string AxisLabel, string UnitSuffix, IReadOnlyList<StepOption> StepOptions, Func<string, double?> ValueSelector)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The histogram definition name.</returns>
		public override string ToString()
		{
			return DisplayName;
		}

		/// <summary>Gets a short debugger display string for this instance.</summary>
		/// <returns>The display text for the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
