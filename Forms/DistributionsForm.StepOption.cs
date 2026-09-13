/*
 * File:        DistributionsForm.StepOption.cs
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

/// <summary>Represents a selectable histogram step size for the DistributionsForm.</summary>
/// <remarks>This class is used to define the available step sizes that can be selected in the DistributionsForm.</remarks>
internal partial class DistributionsForm
{
	/// <summary>Represents one selectable histogram step size.</summary>
	/// <param name="Value">The numeric width of a single histogram bin.</param>
	/// <param name="DisplayText">The text shown in the step-size drop-down.</param>
	/// <remarks>The display text is used directly by the ComboBox, so <see cref="ToString"/> returns <see cref="DisplayText"/>.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private sealed record StepOption(double Value, string DisplayText)
	{
		/// <summary>Returns the display text shown inside ComboBox controls.</summary>
		/// <returns>The preformatted step-size label.</returns>
		public override string ToString()
		{
			return DisplayText;
		}

		/// <summary>Gets a short debugger display string for this instance.</summary>
		/// <returns>The display text for the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
