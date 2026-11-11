/*
 * File:        AsteroidFamilySearch.cs
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

/// <summary>Represents a neighbor candidate found in proper element phase space.</summary>
/// <param name="Element">The proper orbital element record of the neighbor candidate.</param>
/// <param name="DistanceMetersPerSecond">The Zappalà metric velocity distance d_v in m/s.</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly record struct ProperElementNeighbor(ProperOrbitalElement Element, double DistanceMetersPerSecond)
{
	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}

/// <summary>Provides static methods for phase-space distance calculations and asteroid family detection in proper element space (a_p, e_p, sin(i_p)).</summary>
internal static class AsteroidFamilySearch
{
	/// <summary>Calculates the Zappalà metric velocity distance (d_v in m/s) between two proper orbital element sets (Zappalà et al., 1990, 1995).</summary>
	/// <param name="e1">The reference proper element set.</param>
	/// <param name="e2">The target proper element set.</param>
	/// <returns>The quasi-velocity difference d_v in meters per second (m/s).</returns>
	/// <remarks>Calculated as d_v = V_k · √( 5/4 · (Δa_p / a_p)² + 2 · (Δe_p)² + 2 · (Δsin(i_p))² ), where V_k = n · a_p ≈ 29,780 / √a_p (in m/s) is the circular Keplerian orbital speed at semi-major axis a_p.</remarks>
	public static double CalculateZappalaVelocityDistance(ProperOrbitalElement e1, ProperOrbitalElement e2)
	{
		double meanAp = (e1.ProperSemiMajorAxisAu + e2.ProperSemiMajorAxisAu) / 2.0;
		if (meanAp <= 0.0)
		{
			return double.NaN;
		}
		// Keplerian orbital speed V_k in m/s at distance meanAp AU (V_Earth ≈ 29,780 m/s)
		double vkMetersPerSecond = 29_780.0 / Math.Sqrt(d: meanAp);
		double deltaA = Math.Abs(value: e1.ProperSemiMajorAxisAu - e2.ProperSemiMajorAxisAu) / meanAp;
		double deltaE = Math.Abs(value: e1.ProperEccentricity - e2.ProperEccentricity);
		double deltaSinI = Math.Abs(value: e1.ProperSinInclination - e2.ProperSinInclination);
		// Zappalà metric weights: k_1 = 5/4, k_2 = 2, k_3 = 2
		double sumSquares = (1.25 * deltaA * deltaA) + (2.0 * deltaE * deltaE) + (2.0 * deltaSinI * deltaSinI);
		return vkMetersPerSecond * Math.Sqrt(d: sumSquares);
	}

	/// <summary>Finds all nearest neighbors of a reference asteroid within a specified velocity cutoff distance d_cut.</summary>
	/// <param name="seedElement">The target reference asteroid proper element.</param>
	/// <param name="catalog">The collection of candidate proper elements to search through.</param>
	/// <param name="cutoffVelocityMetersPerSecond">The maximum velocity distance threshold d_cut in m/s (typically ~10 to 100 m/s for families).</param>
	/// <returns>A list of neighbor candidates ordered by ascending velocity distance.</returns>
	public static List<ProperElementNeighbor> FindNeighborsWithinCutoff(ProperOrbitalElement seedElement, IEnumerable<ProperOrbitalElement> catalog, double cutoffVelocityMetersPerSecond)
	{
		ArgumentNullException.ThrowIfNull(argument: catalog);
		List<ProperElementNeighbor> neighbors = [];
		foreach (ProperOrbitalElement candidate in catalog)
		{
			// Skip self-comparison
			if (string.Equals(a: seedElement.Designation, b: candidate.Designation, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			// Pre-filtering window in semi-major axis to avoid expensive sqrt/distance calls on distant bodies
			double deltaA = Math.Abs(value: candidate.ProperSemiMajorAxisAu - seedElement.ProperSemiMajorAxisAu);
			if (deltaA > 0.15)
			{
				continue;
			}
			double distance = CalculateZappalaVelocityDistance(e1: seedElement, e2: candidate);
			if (!double.IsNaN(d: distance) && distance <= cutoffVelocityMetersPerSecond)
			{
				neighbors.Add(item: new ProperElementNeighbor(Element: candidate, DistanceMetersPerSecond: distance));
			}
		}
		neighbors.Sort(comparison: static (n1, n2) => n1.DistanceMetersPerSecond.CompareTo(value: n2.DistanceMetersPerSecond));
		return neighbors;
	}

	/// <summary>Extracts a complete asteroid family cluster using the Hierarchical Clustering Method (HCM) single-linkage chaining algorithm.</summary>
	/// <param name="seedElement">The starting seed asteroid (e.g., Ceres, Flora, Koronis, Eos).</param>
	/// <param name="catalog">The full dataset of proper elements.</param>
	/// <param name="cutoffVelocityMetersPerSecond">The clustering cutoff velocity d_cut in m/s.</param>
	/// <returns>An <see cref="AsteroidFamilyCluster"/> structure containing all transitively linked family members.</returns>
	public static AsteroidFamilyCluster IdentifyFamilyClusterHcm(ProperOrbitalElement seedElement, IReadOnlyList<ProperOrbitalElement> catalog, double cutoffVelocityMetersPerSecond)
	{
		ArgumentNullException.ThrowIfNull(argument: catalog);
		HashSet<string> visitedDesignations = [with(comparer: StringComparer.OrdinalIgnoreCase), seedElement.Designation];
		List<ProperOrbitalElement> clusterMembers = [seedElement];
		Queue<ProperOrbitalElement> queue = new();
		queue.Enqueue(item: seedElement);
		// Single-linkage Breadth-First Search (BFS) chaining
		while (queue.Count > 0)
		{
			ProperOrbitalElement current = queue.Dequeue();
			List<ProperElementNeighbor> neighbors = FindNeighborsWithinCutoff(seedElement: current, catalog: catalog, cutoffVelocityMetersPerSecond: cutoffVelocityMetersPerSecond);
			foreach (ProperElementNeighbor neighbor in neighbors)
			{
				if (visitedDesignations.Add(item: neighbor.Element.Designation))
				{
					clusterMembers.Add(item: neighbor.Element);
					queue.Enqueue(item: neighbor.Element);
				}
			}
		}
		return new AsteroidFamilyCluster(SeedElement: seedElement, CutoffVelocityMetersPerSecond: cutoffVelocityMetersPerSecond, Members: clusterMembers);
	}
}
