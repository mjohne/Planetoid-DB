/*
 * File:        MoidCalculator.PlanetComputationData.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.
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

/// <summary>Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This partial class contains the <see cref="PlanetComputationData"/> record, which holds precomputed values for a planet's orbit used during repeated MOID calculations. The precomputed values optimize the inner loop of the MOID calculation by avoiding repeated trigonometric and arithmetic computations for the planetary orbit during bulk processing.</remarks>
internal partial class MoidCalculator
{
	/// <summary>Precomputed values for one planet used during repeated MOID computations.</summary>
	/// <param name="Name">Planet name.</param>
	/// <param name="SemiMajorAxis">Semi-major axis in AU.</param>
	/// <param name="Eccentricity">Orbital eccentricity.</param>
	/// <param name="ArgumentPerihelionRad">Argument of perihelion in radians.</param>
	/// <param name="CosLongitudeAscendingNode">Precomputed cos(Ω).</param>
	/// <param name="SinLongitudeAscendingNode">Precomputed sin(Ω).</param>
	/// <param name="CosInclination">Precomputed cos(i).</param>
	/// <param name="SinInclination">Precomputed sin(i).</param>
	/// <param name="OneMinusEccentricitySquared">Precomputed 1 - e².</param>
	/// <return>The precomputed planet data.</return>
	/// <remarks>These precomputed values are used to optimize the inner loop of the MOID calculation by avoiding repeated trigonometric and arithmetic computations for the planetary orbit during bulk processing.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private record PlanetComputationData(string Name, double SemiMajorAxis, double Eccentricity, double ArgumentPerihelionRad, double CosLongitudeAscendingNode, double SinLongitudeAscendingNode, double CosInclination, double SinInclination, double OneMinusEccentricitySquared)
	{
		/// <summary>Returns a string representation of the precomputed planet data for debugging purposes.</summary>
		/// <returns>A string representation of the precomputed planet data for debugging purposes.</returns>
		/// <remarks>This property is used by the debugger to display a concise summary of the precomputed planet data.</remarks>
		private string DebuggerDisplay => ToString();
	}
}