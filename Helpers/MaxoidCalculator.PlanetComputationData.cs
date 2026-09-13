/*
 * File:        MaxoidCalculator.PlanetComputationData.cs
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

/// <summary>Represents precomputed values for one planet used during repeated MAXOID computations.</summary>
/// <remarks>This record is used internally by the MaxoidCalculator to store values that are expensive to compute repeatedly, improving performance during MAXOID calculations.</remarks>
internal partial class MaxoidCalculator
{
	/// <summary>Precomputed values for one planet used during repeated MAXOID computations.</summary>
	/// <param name="Name">Planet name.</param>
	/// <param name="SemiMajorAxis">Semi-major axis in AU.</param>
	/// <param name="Eccentricity">Orbital eccentricity.</param>
	/// <param name="ArgumentPerihelionRad">Argument of perihelion in radians.</param>
	/// <param name="CosLongitudeAscendingNode">Precomputed cos(Ω).</param>
	/// <param name="SinLongitudeAscendingNode">Precomputed sin(Ω).</param>
	/// <param name="CosInclination">Precomputed cos(i).</param>
	/// <param name="SinInclination">Precomputed sin(i).</param>
	/// <param name="OneMinusEccentricitySquared">Precomputed 1 - e².</param>
	/// <return>The precomputed planet computation data.</return>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private record PlanetComputationData(string Name, double SemiMajorAxis, double Eccentricity, double ArgumentPerihelionRad, double CosLongitudeAscendingNode, double SinLongitudeAscendingNode, double CosInclination, double SinInclination, double OneMinusEccentricitySquared)
	{
		/// <summary>Returns a string representation of the planet computation data for debugging purposes.</summary>
		/// <returns>A string representation of the planet computation data.</returns>
		/// <remarks>This property is used by the debugger to display the contents of the PlanetComputationData record in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}