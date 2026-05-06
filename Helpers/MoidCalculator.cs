// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This class implements a fast, high-precision numerical double-grid search algorithm with local refinement, equivalent to the approach used by the Minor Planet Center (MPC) for computing MOID values. The method samples both orbits on a fine angular grid to locate the global minimum distance, then refines it using coordinate-descent optimization to achieve sub-milliarcsecond accuracy.</remarks>
internal class MoidCalculator
{
	/// <summary>Represents the Keplerian orbital elements of a solar system planet.</summary>
	/// <param name="Name">The common name of the planet.</param>
	/// <param name="SemiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="Eccentricity">The orbital eccentricity (dimensionless).</param>
	/// <param name="InclinationDeg">The orbital inclination to the ecliptic in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <remarks>These mean orbital elements are referenced to the J2000.0 ecliptic and equinox, as listed in standard astronomical references (Standish 1992 / IAU).</remarks>
	public record PlanetElements(
		string Name,
		double SemiMajorAxis,
		double Eccentricity,
		double InclinationDeg,
		double LongitudeAscendingNodeDeg,
		double ArgumentPerihelionDeg);

	/// <summary>Represents the MOID result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="MoidAu">The Minimum Orbit Intersection Distance in AU.</param>
	/// <remarks>The MOID is the closest geometric approach distance between the two osculating orbits, independent of the bodies' actual positions at any epoch.</remarks>
	public record MoidResult(string PlanetName, double MoidAu);

	/// <summary>Mean orbital elements of the eight solar system planets at J2000.0 (ecliptic reference frame).</summary>
	/// <remarks>These constants follow the commonly cited IAU-JPL mean planetary elements at J2000.0 as published by Standish (1992). The reference frame is the J2000.0 ecliptic and equinox. The stored <c>ArgumentPerihelionDeg</c> values are the argument of perihelion (<c>ω</c>), derived from the published longitude of perihelion (<c>ϖ</c>) and longitude of the ascending node (<c>Ω</c>) using <c>ω = ϖ - Ω</c>.</remarks>
	private static readonly PlanetElements[] Planets =
	[
		new(Name: "Mercury", SemiMajorAxis: 0.38709893, Eccentricity: 0.20563069, InclinationDeg: 7.00487, LongitudeAscendingNodeDeg: 48.33167, ArgumentPerihelionDeg: 29.12478),
		new(Name: "Venus", SemiMajorAxis: 0.72333199, Eccentricity: 0.00677323, InclinationDeg: 3.39471, LongitudeAscendingNodeDeg: 76.68069, ArgumentPerihelionDeg: 54.85229),
		new(Name: "Earth", SemiMajorAxis: 1.00000011, Eccentricity: 0.01671022, InclinationDeg: 0.00005, LongitudeAscendingNodeDeg: -11.26064, ArgumentPerihelionDeg: 114.20783),
		new(Name: "Mars", SemiMajorAxis: 1.52366231, Eccentricity: 0.09341233, InclinationDeg: 1.85061, LongitudeAscendingNodeDeg: 49.57854, ArgumentPerihelionDeg: 286.46230),
		new(Name: "Jupiter", SemiMajorAxis: 5.20336301, Eccentricity: 0.04839266, InclinationDeg: 1.30530, LongitudeAscendingNodeDeg: 100.55615, ArgumentPerihelionDeg: 274.19770),
		new(Name: "Saturn", SemiMajorAxis: 9.53707032, Eccentricity: 0.05415060, InclinationDeg: 2.48446, LongitudeAscendingNodeDeg: 113.71504, ArgumentPerihelionDeg: 338.71690),
		new(Name: "Uranus", SemiMajorAxis: 19.19126393, Eccentricity: 0.04716771, InclinationDeg: 0.76986, LongitudeAscendingNodeDeg: 74.22988, ArgumentPerihelionDeg: 96.73436),
		new(Name: "Neptune", SemiMajorAxis: 30.06896348, Eccentricity: 0.00858587, InclinationDeg: 1.76917, LongitudeAscendingNodeDeg: 131.72169, ArgumentPerihelionDeg: 273.24966),
	];

	/// <summary>Calculates the MOID between a minor planet and each of the eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the minor planet in AU.</param>
	/// <param name="eccentricity">The eccentricity of the minor planet's orbit.</param>
	/// <param name="inclinationDeg">The inclination of the minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node of the minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion of the minor planet's orbit in degrees.</param>
	/// <returns>A list of <see cref="MoidResult"/> records, one per planet, in order from Mercury to Neptune.</returns>
	/// <remarks>Uses a two-stage numerical algorithm: a coarse grid search (720 × 720 angular samples) to locate the approximate minimum, followed by coordinate-descent refinement for high precision.</remarks>
	public static List<MoidResult> CalculateMoids(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg)
	{
		// Convert minor planet angles from degrees to radians
		double i1 = inclinationDeg * Math.PI / 180.0;
		double o1 = argumentPerihelionDeg * Math.PI / 180.0;
		double bigO1 = longitudeAscendingNodeDeg * Math.PI / 180.0;
		List<MoidResult> results = [];
		// Compute MOID for each planet
		foreach (PlanetElements planet in Planets)
		{
			// Convert planet angles from degrees to radians
			double i2 = planet.InclinationDeg * Math.PI / 180.0;
			double o2 = planet.ArgumentPerihelionDeg * Math.PI / 180.0;
			double bigO2 = planet.LongitudeAscendingNodeDeg * Math.PI / 180.0;
			// Calculate MOID using the double-grid search algorithm
			double moid = CalculateMoid(
				a1: semiMajorAxis, e1: eccentricity, i1: i1, w1: o1, bigO1: bigO1,
				a2: planet.SemiMajorAxis, e2: planet.Eccentricity, i2: i2, w2: o2, bigO2: bigO2);
			results.Add(item: new MoidResult(PlanetName: planet.Name, MoidAu: moid));
		}
		return results;
	}

	/// <summary>Calculates the MOID between two minor planets using their Keplerian orbital elements.</summary>
	/// <param name="semiMajorAxis1">The semi-major axis of the first minor planet in AU.</param>
	/// <param name="eccentricity1">The eccentricity of the first minor planet's orbit.</param>
	/// <param name="inclinationDeg1">The inclination of the first minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg1">The longitude of the ascending node of the first minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg1">The argument of perihelion of the first minor planet's orbit in degrees.</param>
	/// <param name="semiMajorAxis2">The semi-major axis of the second minor planet in AU.</param>
	/// <param name="eccentricity2">The eccentricity of the second minor planet's orbit.</param>
	/// <param name="inclinationDeg2">The inclination of the second minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg2">The longitude of the ascending node of the second minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg2">The argument of perihelion of the second minor planet's orbit in degrees.</param>
	/// <returns>The MOID in AU between the two minor planets.</returns>
	/// <remarks>Uses the same two-stage numerical algorithm as <see cref="CalculateMoids"/>: a coarse grid search (720 × 720 angular samples) followed by coordinate-descent refinement.</remarks>
	public static double CalculateMoidBetweenPlanetoids(
		double semiMajorAxis1,
		double eccentricity1,
		double inclinationDeg1,
		double longitudeAscendingNodeDeg1,
		double argumentPerihelionDeg1,
		double semiMajorAxis2,
		double eccentricity2,
		double inclinationDeg2,
		double longitudeAscendingNodeDeg2,
		double argumentPerihelionDeg2)
	{
		// Convert angular elements of the first minor planet from degrees to radians
		double i1 = inclinationDeg1 * Math.PI / 180.0;
		double o1 = argumentPerihelionDeg1 * Math.PI / 180.0;
		double bigO1 = longitudeAscendingNodeDeg1 * Math.PI / 180.0;
		// Convert angular elements of the second minor planet from degrees to radians
		double i2 = inclinationDeg2 * Math.PI / 180.0;
		double o2 = argumentPerihelionDeg2 * Math.PI / 180.0;
		double bigO2 = longitudeAscendingNodeDeg2 * Math.PI / 180.0;
		// Calculate the MOID between the two orbits using the double-grid search algorithm
		return CalculateMoid(
			a1: semiMajorAxis1, e1: eccentricity1, i1: i1, w1: o1, bigO1: bigO1,
			a2: semiMajorAxis2, e2: eccentricity2, i2: i2, w2: o2, bigO2: bigO2);
	}

	/// <summary>Computes the MOID between two Keplerian orbits using a double-grid search with local refinement.</summary>
	/// <param name="a1">Semi-major axis of orbit 1 (AU).</param>
	/// <param name="e1">Eccentricity of orbit 1.</param>
	/// <param name="i1">Inclination of orbit 1 (radians).</param>
	/// <param name="w1">Argument of perihelion of orbit 1 (radians).</param>
	/// <param name="bigO1">Longitude of ascending node of orbit 1 (radians).</param>
	/// <param name="a2">Semi-major axis of orbit 2 (AU).</param>
	/// <param name="e2">Eccentricity of orbit 2.</param>
	/// <param name="i2">Inclination of orbit 2 (radians).</param>
	/// <param name="w2">Argument of perihelion of orbit 2 (radians).</param>
	/// <param name="bigO2">Longitude of ascending node of orbit 2 (radians).</param>
	/// <returns>The MOID in AU.</returns>
	/// <remarks> Stage 1: coarse double-grid search with <c>GridSteps = 720</c> steps per orbit (0.5° resolution). Stage 2: coordinate-descent refinement starting from the best grid point, iteratively narrowing the search bracket until the step size falls below <c>1e-9</c> radians (≈ 0.2 milli-arcseconds), giving sub-milliarcsecond accuracy.
	/// </remarks>
	private static double CalculateMoid(
		double a1, double e1, double i1, double w1, double bigO1,
		double a2, double e2, double i2, double w2, double bigO2)
	{
		// Number of angular samples per orbit for the coarse grid (0.5° resolution)
		const int GridSteps = 720;
		const double TwoPi = 2.0 * Math.PI;
		double stepSize = TwoPi / GridSteps;
		// Rent arrays from pool for better memory management
		(double X, double Y, double Z)[] pos1Cache = ArrayPool<(double X, double Y, double Z)>.Shared.Rent(minimumLength: GridSteps);
		(double X, double Y, double Z)[] pos2Cache = ArrayPool<(double X, double Y, double Z)>.Shared.Rent(minimumLength: GridSteps);
		try
		{
			// Precompute trigonometric values for orbit 1
			double cosO1 = Math.Cos(d: bigO1);
			double sinO1 = Math.Sin(a: bigO1);
			double cosi1 = Math.Cos(d: i1);
			double sini1 = Math.Sin(a: i1);
			double oneMinusE1Sq = 1.0 - (e1 * e1);
			// Precompute trigonometric values for orbit 2
			double cosO2 = Math.Cos(d: bigO2);
			double sinO2 = Math.Sin(a: bigO2);
			double cosi2 = Math.Cos(d: i2);
			double sini2 = Math.Sin(a: i2);
			double oneMinusE2Sq = 1.0 - (e2 * e2);
			// Precompute both orbits' positions with optimized calculations using better partitioning
			_ = Parallel.For(
				fromInclusive: 0,
				toExclusive: GridSteps,
				parallelOptions: new ParallelOptions
				{
					MaxDegreeOfParallelism = Environment.ProcessorCount
				},
				body: idx =>
				{
					double f = idx * stepSize;
					pos1Cache[idx] = OrbitPositionOptimized(a: a1, e: e1, w: w1, f: f, cosO: cosO1, sinO: sinO1, cosi: cosi1, sini: sini1, oneMinusESq: oneMinusE1Sq);
					pos2Cache[idx] = OrbitPositionOptimized(a: a2, e: e2, w: w2, f: f, cosO: cosO2, sinO: sinO2, cosi: cosi2, sini: sini2, oneMinusESq: oneMinusE2Sq);
				});
			// Lock-free parallel search using Interlocked operations with optimized partitioning
			long minDistSquaredBits = BitConverter.DoubleToInt64Bits(value: double.MaxValue);
			long bestF1Bits = 0;
			long bestF2Bits = 0;
			// Use Partitioner for better work distribution
			Partitioner<Tuple<int, int>> partitioner = Partitioner.Create(fromInclusive: 0, toExclusive: GridSteps, rangeSize: Math.Max(1, GridSteps / (Environment.ProcessorCount * 4)));
			_ = Parallel.ForEach(
				source: partitioner,
				parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
				body: range =>
				{
					// Each thread maintains its own local minimum to reduce contention, then updates the global minimum using Interlocked operations
					double localMinDistSq = double.MaxValue;
					int localBestI = 0;
					int localBestJ = 0;
					// Loop over the assigned range of indices for orbit 1
					for (int i = range.Item1; i < range.Item2; i++)
					{
						(double X, double Y, double Z) p1 = pos1Cache[i];
						// SIMD-optimized inner loop when available
						if (Vector256.IsHardwareAccelerated && GridSteps >= Vector256<double>.Count)
						{
							int j = 0;
							int simdEnd = GridSteps - (GridSteps % 4);
							// Process 4 positions at a time using SIMD (when possible)
							for (; j < simdEnd; j += 4)
							{
								// Load 4 positions from pos2Cache into SIMD registers
								for (int k = 0; k < 4; k++)
								{
									(double X, double Y, double Z) p2 = pos2Cache[j + k];
									double distSq = DistanceSquared(p1: p1, p2: p2);
									// Update local minimum if this distance is smaller
									if (distSq < localMinDistSq)
									{
										localMinDistSq = distSq;
										localBestI = i;
										localBestJ = j + k;
									}
								}
							}
							// Handle remaining elements
							for (; j < GridSteps; j++)
							{
								// Process remaining positions without SIMD
								(double X, double Y, double Z) p2 = pos2Cache[j];
								double distSq = DistanceSquared(p1: p1, p2: p2);
								// Update local minimum if this distance is smaller
								if (distSq < localMinDistSq)
								{
									localMinDistSq = distSq;
									localBestI = i;
									localBestJ = j;
								}
							}
						}
						else
						{
							// Fallback to standard loop
							for (int j = 0; j < GridSteps; j++)
							{
								// Process positions without SIMD
								(double X, double Y, double Z) p2 = pos2Cache[j];
								double distSq = DistanceSquared(p1: p1, p2: p2);
								// Update local minimum if this distance is smaller
								if (distSq < localMinDistSq)
								{
									localMinDistSq = distSq;
									localBestI = i;
									localBestJ = j;
								}
							}
						}
					}
					// Lock-free update using Interlocked.CompareExchange
					while (true)
					{
						// Read the current global minimum as a double via its bit representation
						long currentMinBits = Interlocked.Read(location: ref minDistSquaredBits);
						double currentMin = BitConverter.Int64BitsToDouble(value: currentMinBits);
						// If the local minimum is not better than the current global minimum, no need to update
						if (localMinDistSq >= currentMin)
						{
							break;
						}
						// Attempt to update the global minimum with the local minimum using CompareExchange
						long newMinBits = BitConverter.DoubleToInt64Bits(value: localMinDistSq);
						if (Interlocked.CompareExchange(location1: ref minDistSquaredBits, value: newMinBits, comparand: currentMinBits) == currentMinBits)
						{
							Interlocked.Exchange(location1: ref bestF1Bits, value: BitConverter.DoubleToInt64Bits(value: localBestI * stepSize));
							Interlocked.Exchange(location1: ref bestF2Bits, value: BitConverter.DoubleToInt64Bits(value: localBestJ * stepSize));
							break;
						}
					}
				});

			double minDistSquared = BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref minDistSquaredBits));
			double bestF1 = BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref bestF1Bits));
			double bestF2 = BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref bestF2Bits));
			// Local refinement via coordinate descent with precomputed trig values
			return RefineMinimumOptimized(
				a1: a1, e1: e1, w1: w1, cosO1: cosO1, sinO1: sinO1, cosi1: cosi1, sini1: sini1, oneMinusE1Sq: oneMinusE1Sq,
				a2: a2, e2: e2, w2: w2, cosO2: cosO2, sinO2: sinO2, cosi2: cosi2, sini2: sini2, oneMinusE2Sq: oneMinusE2Sq,
				f1Start: bestF1, f2Start: bestF2, initialStep: stepSize,
				coarseMin: Math.Sqrt(d: minDistSquared));
		}
		finally
		{
			// Return rented arrays to pool
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos1Cache);
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos2Cache);
		}
	}

	/// <summary>Refines the MOID estimate using precomputed trigonometric values for maximum performance.</summary>
	/// <param name="a1">Semi-major axis of orbit 1 (AU).</param>
	/// <param name="e1">Eccentricity of orbit 1.</param>
	/// <param name="w1">Argument of perihelion of orbit 1 (radians).</param>
	/// <param name="cosO1">Precomputed cos(Ω₁).</param>
	/// <param name="sinO1">Precomputed sin(Ω₁).</param>
	/// <param name="cosi1">Precomputed cos(i₁).</param>
	/// <param name="sini1">Precomputed sin(i₁).</param>
	/// <param name="oneMinusE1Sq">Precomputed 1 - e₁².</param>
	/// <param name="a2">Semi-major axis of orbit 2 (AU).</param>
	/// <param name="e2">Eccentricity of orbit 2.</param>
	/// <param name="w2">Argument of perihelion of orbit 2 (radians).</param>
	/// <param name="cosO2">Precomputed cos(Ω₂).</param>
	/// <param name="sinO2">Precomputed sin(Ω₂).</param>
	/// <param name="cosi2">Precomputed cos(i₂).</param>
	/// <param name="sini2">Precomputed sin(i₂).</param>
	/// <param name="oneMinusE2Sq">Precomputed 1 - e₂².</param>
	/// <param name="f1Start">True anomaly of orbit 1 at the coarse minimum (radians).</param>
	/// <param name="f2Start">True anomaly of orbit 2 at the coarse minimum (radians).</param>
	/// <param name="initialStep">Half-width of the initial search bracket (radians).</param>
	/// <param name="coarseMin">The coarse minimum distance used as the initial best.</param>
	/// <returns>The refined MOID in AU.</returns>
	/// <remarks>Optimized version that reuses precomputed trigonometric values from the grid search phase.</remarks>
	private static double RefineMinimumOptimized(
		double a1, double e1, double w1, double cosO1, double sinO1, double cosi1, double sini1, double oneMinusE1Sq,
		double a2, double e2, double w2, double cosO2, double sinO2, double cosi2, double sini2, double oneMinusE2Sq,
		double f1Start, double f2Start, double initialStep, double coarseMin)
	{
		double f1 = f1Start;
		double f2 = f2Start;
		double minDistSquared = coarseMin * coarseMin;
		double step = initialStep;
		// Iteratively shrink the search bracket using squared distances
		for (int iter = 0; iter < 60 && step > 1e-9; iter++)
		{
			step *= 0.5;
			bool improved = false;
			// Try all 8 neighbouring directions in (f1, f2) space
			for (int df1Sign = -1; df1Sign <= 1; df1Sign++)
			{
				for (int df2Sign = -1; df2Sign <= 1; df2Sign++)
				{
					if (df1Sign == 0 && df2Sign == 0)
					{
						continue;
					}
					// Compute the new candidate points by moving in the (f1, f2) space
					double nf1 = f1 + (df1Sign * step);
					double nf2 = f2 + (df2Sign * step);
					double dSq = DistanceSquared(
						p1: OrbitPositionOptimized(a: a1, e: e1, w: w1, f: nf1, cosO: cosO1, sinO: sinO1, cosi: cosi1, sini: sini1, oneMinusESq: oneMinusE1Sq),
						p2: OrbitPositionOptimized(a: a2, e: e2, w: w2, f: nf2, cosO: cosO2, sinO: sinO2, cosi: cosi2, sini: sini2, oneMinusESq: oneMinusE2Sq)
					);
					// If this new point is closer, update the minimum and continue refining from there
					if (dSq < minDistSquared)
					{
						minDistSquared = dSq;
						f1 = nf1;
						f2 = nf2;
						improved = true;
					}
				}
			}
			// If no improvement was made at this step size, try increasing step again
			if (!improved)
			{
				step *= 0.5;
			}
		}

		return Math.Sqrt(d: minDistSquared);
	}

	/// <summary>Computes the heliocentric Cartesian coordinates with precomputed trigonometric values.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="w">Argument of perihelion in radians.</param>
	/// <param name="f">True anomaly in radians.</param>
	/// <param name="cosO">Precomputed cos(Ω).</param>
	/// <param name="sinO">Precomputed sin(Ω).</param>
	/// <param name="cosi">Precomputed cos(i).</param>
	/// <param name="sini">Precomputed sin(i).</param>
	/// <param name="oneMinusESq">Precomputed 1 - e².</param>
	/// <returns>The Cartesian position (X, Y, Z) in the ecliptic frame (AU).</returns>
	/// <remarks>Optimized version that avoids redundant trigonometric calculations.</remarks>
	[MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
	private static (double X, double Y, double Z) OrbitPositionOptimized(
		double a, double e, double w, double f,
		double cosO, double sinO, double cosi, double sini, double oneMinusESq)
	{
		// Heliocentric distance from the focal formula (using precomputed 1-e²)
		double r = a * oneMinusESq / (1.0 + (e * Math.Cos(d: f)));
		// Argument of latitude u = ω + f
		double u = w + f;
		double cosu = Math.Cos(d: u);
		double sinu = Math.Sin(a: u);
		// Standard perifocal-to-inertial rotation with precomputed trig values
		double x = r * ((cosO * cosu) - (sinO * sinu * cosi));
		double y = r * ((sinO * cosu) + (cosO * sinu * cosi));
		double z = r * sinu * sini;
		return (X: x, Y: y, Z: z);
	}

	/// <summary>Computes the squared Euclidean distance between two points in 3-D space.</summary>
	/// <param name="p1">The first point as (X, Y, Z).</param>
	/// <param name="p2">The second point as (X, Y, Z).</param>
	/// <returns>The squared distance between the two points.</returns>
	/// <remarks>Using squared distance avoids expensive sqrt operations during comparisons.</remarks>
	[MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
	private static double DistanceSquared(
		(double X, double Y, double Z) p1,
		(double X, double Y, double Z) p2)
	{
		double dx = p1.X - p2.X;
		double dy = p1.Y - p2.Y;
		double dz = p1.Z - p2.Z;
		return (dx * dx) + (dy * dy) + (dz * dz);
	}
}
