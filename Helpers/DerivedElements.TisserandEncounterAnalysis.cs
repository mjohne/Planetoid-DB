/*
 * File:        DerivedElements.TisserandEncounterAnalysis.cs
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

/// <summary>Provides derived orbital element calculations and planetary encounter analyses.</summary>
internal partial class DerivedElements
{
	/// <summary>Represents the result of a Tisserand parameter invariance analysis across a planetary encounter.</summary>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal readonly record struct TisserandEncounterAnalysis(double PreTisserand, double PostTisserand, double AbsoluteDelta, double RelativeDeviationPercent, bool IsQuasiConserved, OrbitDynamicalClass PreClassification, OrbitDynamicalClass PostClassification)
	{
		/// <summary>Returns a string representation of the Tisserand encounter analysis for debugging purposes.</summary>
		/// <returns>A string representation of the Tisserand encounter analysis.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the Tisserand encounter analysis for debugging.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
