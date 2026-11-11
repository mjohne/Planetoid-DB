/*
 * File:        AsteroidFamilyCluster.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides algorithms for asteroid family identification using the Zappalà distance metric and Hierarchical Clustering (HCM) in proper element space.
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

/// <summary>Represents an identified asteroid family cluster resulting from Hierarchical Clustering (HCM).</summary>
/// <param name="SeedElement">The seed/parent asteroid proper element around which the cluster was built.</param>
/// <param name="CutoffVelocityMetersPerSecond">The cutoff velocity threshold (d_cut) used for clustering in m/s.</param>
/// <param name="Members">The list of identified family members including the seed body.</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly record struct AsteroidFamilyCluster(ProperOrbitalElement SeedElement, double CutoffVelocityMetersPerSecond, IReadOnlyList<ProperOrbitalElement> Members)
{
	/// <summary>Gets the total member count of the family cluster.</summary>
	public int MemberCount => Members.Count;

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
