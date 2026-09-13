/*
 * File:        MaxoidCalculator.MaxoidResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.
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

/// <summary>Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This class contains methods to compute the MAXOID, which is the greatest geometric separation distance between the osculating orbits of a minor planet and a solar system planet, independent of their actual positions at any epoch.</remarks>
internal partial class MaxoidCalculator
{
	/// <summary>Represents the MAXOID result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="MaxoidAu">The Maximum Orbit Intersection Distance in AU.</param>
	/// <return>The MAXOID result for the specified planet.</return>
	/// <remarks>The MAXOID is the greatest geometric separation distance between the two osculating orbits, independent of the bodies' actual positions at any epoch.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record MaxoidResult(string PlanetName, double MaxoidAu)
	{
		/// <summary>Returns a string representation of the MAXOID result for debugging purposes.</summary>
		/// <returns>A string representation of the MAXOID result.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the MaxoidResult record in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}