/*
 * File:        FilterForm.OrbitalElementFilter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form for filtering data in the Planetoid database.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents a form for filtering data in the Planetoid database, specifically for orbital elements.</summary>
/// <remarks>This form allows users to specify minimum and maximum values for various orbital elements to filter the displayed data accordingly.</remarks>
internal partial class FilterForm
{
	/// <summary>Defines one fixed-width MPCORB orbital element field and the corresponding min/max spinbuttons.</summary>
	/// <param name="Name">The user-facing name of the orbital element.</param>
	/// <param name="Start">The starting index of the orbital element field in the MPCORB line.</param>
	/// <param name="Length">The fixed width of the orbital element field in the MPCORB line.</param>
	/// <param name="MinimumControl">The spinbutton control used to specify the minimum value for the orbital element.</param>
	/// <param name="MaximumControl">The spinbutton control used to specify the maximum value for the orbital element.</param>
	/// <remarks>This record struct is used to encapsulate the metadata and controls associated with a specific orbital element filter.</remarks>
	/// <returns>A new instance of the <see cref="OrbitalElementFilter"/> record struct.</returns>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct OrbitalElementFilter(string Name, int Start, int Length, KryptonNumericUpDown MinimumControl, KryptonNumericUpDown MaximumControl)
	{
		/// <summary>Returns a string representation of the orbital element filter for debugging purposes.</summary>
		/// <returns>A string representation of the orbital element filter.</returns>
		/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
		private string DebuggerDisplay => ToString();
	}
}