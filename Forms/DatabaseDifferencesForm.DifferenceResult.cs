/*
 * File:        DatabaseDifferencesForm.DifferenceResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for comparing two MPCORB.DAT files and displaying the differences.
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

/// <summary>Form for comparing two MPCORB.DAT files and displaying the differences.</summary>
/// <remarks>This form allows users to select two MPCORB.DAT files, compare their contents, and view the differences in a user-friendly interface. The form includes functionality for saving the comparison results in various formats and navigating to specific records in the main form based on the differences identified.</remarks>
internal partial class DatabaseDifferencesForm
{
	/// <summary>Represents the result of a comparison, including the index, designation, and the difference observed.</summary>
	/// <param name="Index">The index of the item in the comparison, indicating its position in the original dataset.</param>
	/// <param name="Designation">The designation or label associated with the item being compared, providing context for the comparison.</param>
	/// <param name="Difference">The difference observed between the compared items, detailing the nature of the discrepancy.</param>
	/// <remarks>This record struct is used to encapsulate the result of a comparison between two items, including their index, designation, and the observed difference.</remarks>
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
	private record struct DifferenceResult(string Index, string Designation, string Difference)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
		[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
		private readonly string DebuggerDisplay => ToString();
	}
}