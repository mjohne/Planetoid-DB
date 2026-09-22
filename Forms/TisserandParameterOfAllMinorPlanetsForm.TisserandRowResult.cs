/*
 * File:        TisserandParameterOfAllMinorPlanetsForm.TisserandRowResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for displaying the Tisserand parameter of all minor planets relative to each of the eight solar system planets.
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

/// <summary>Form for displaying the Tisserand parameter of all minor planets relative to each of the eight solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their Tisserand parameters with respect to all eight planets. Results are presented in a ListView where each row corresponds to one planetoid and the eight Tisserand parameter columns correspond to Mercury through Neptune. The user can start and cancel the calculation at any time and track progress via the integrated progress bar.</remarks>
internal partial class TisserandParameterOfAllMinorPlanetsForm
{
	/// <summary>Represents one row in the Tisserand parameter results list: the planetoid name and one Tisserand value per planet.</summary>
	/// <param name="PlanetoidName">The designation of the minor planet.</param>
	/// <param name="TisserandValues">Array of eight Tisserand parameter values (dimensionless), one per planet in order Mercury–Neptune.</param>
	/// <remarks>The <paramref name="TisserandValues"/> array always has exactly eight elements corresponding to the eight solar system planets: Mercury (0), Venus (1), Earth (2), Mars (3), Jupiter (4), Saturn (5), Uranus (6), Neptune (7).</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private record TisserandRowResult(string PlanetoidName, double[] TisserandValues)
	{
		/// <summary>Returns a string representation of the Tisserand row result for debugging purposes.</summary>
		/// <returns>A string representation of the Tisserand row result.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the TisserandRowResult struct in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}