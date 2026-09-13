/*
 * File:        TisserandParameterCalculator.TisserandResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Tisserand parameter of a minor planet relative to each of the eight solar system planets.
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

/// <summary>Provides methods for calculating the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>The Tisserand parameter is a quasi-conserved quantity derived from the Jacobi constant in the circular restricted three-body problem. It is defined as: <para><c>T_P = a_P / a + 2 * cos(i) * sqrt(a / a_P * (1 - e²))</c></para> where <c>a_P</c> is the semi-major axis of the reference planet, <c>a</c> is the semi-major axis of the minor planet, <c>e</c> is the eccentricity of the minor planet, and <c>i</c> is the orbital inclination of the minor planet. By convention <c>T_J</c> (relative to Jupiter) is the most commonly used form and is widely employed to classify small solar-system bodies.</remarks>
internal partial class TisserandParameterCalculator
{
	/// <summary>Represents the Tisserand parameter result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the reference planet.</param>
	/// <param name="TisserandValue">The computed Tisserand parameter value (dimensionless).</param>
	/// <return>The Tisserand result.</return>
	/// <remarks>Values near 3 (relative to Jupiter) indicate a Jupiter-family comet or Jupiter-crossing orbit. Values greater than 3 typically indicate an asteroid, while values less than 2 suggest a nearly isotropic comet.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal readonly record struct TisserandResult(string PlanetName, double TisserandValue)
	{
		/// <summary>Returns a string representation of the <see cref="TisserandResult"/> record for debugging purposes.</summary>
		/// <returns>A string representation of the <see cref="TisserandResult"/> record.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the record in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}