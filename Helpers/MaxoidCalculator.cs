using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating the Maximum Orbit Intersection Distance (MAXOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This class implements a fast, high-precision numerical double-grid search algorithm with local refinement. The method samples both orbits on a fine angular grid to locate the global maximum distance, then refines it using coordinate-ascent optimization to achieve sub-milliarcsecond accuracy.</remarks>
internal class MaxoidCalculator
{
	/// <summary>Number of angular samples per orbit used in the coarse search phase.</summary>
	/// <remarks>720 samples correspond to a 0.5° sampling interval over the full 0..2π range.</remarks>
	private const int GridSteps = 720;

	/// <summary>2π in radians.</summary>
	/// <remarks>Used for converting sampled angle indices to true anomaly values.</remarks>
	private static readonly double TwoPi = 2.0 * Math.PI;

	/// <summary>Angular step size in radians for the coarse grid.</summary>
	/// <remarks>This value is computed once and reused across all MAXOID evaluations.</remarks>
	private static readonly double GridStepSize = TwoPi / GridSteps;

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

	/// <summary>Represents the MAXOID result for a minor planet relative to a specific solar system planet.</summary>
	/// <param name="PlanetName">The name of the planet.</param>
	/// <param name="MaxoidAu">The Maximum Orbit Intersection Distance in AU.</param>
	/// <remarks>The MAXOID is the greatest geometric separation distance between the two osculating orbits, independent of the bodies' actual positions at any epoch.</remarks>
	public record MaxoidResult(string PlanetName, double MaxoidAu);

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
	private record PlanetComputationData(
		string Name,
		double SemiMajorAxis,
		double Eccentricity,
		double ArgumentPerihelionRad,
		double CosLongitudeAscendingNode,
		double SinLongitudeAscendingNode,
		double CosInclination,
		double SinInclination,
		double OneMinusEccentricitySquared);

	/// <summary>Represents the coarse-search maximum used as the starting point for local refinement.</summary>
	/// <param name="MaxDistanceSquared">Squared distance at the best coarse grid sample.</param>
	/// <param name="BestF1">Best true anomaly on orbit 1 from the coarse grid.</param>
	/// <param name="BestF2">Best true anomaly on orbit 2 from the coarse grid.</param>
	private record struct CoarseMaximumResult(double MaxDistanceSquared, double BestF1, double BestF2);

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

	/// <summary>Precomputed trigonometric/orbital constants for all planets.</summary>
	/// <remarks>Built once to avoid repeated degree-to-radian and trig conversions during bulk MAXOID runs.</remarks>
	private static readonly PlanetComputationData[] PrecomputedPlanets = BuildPrecomputedPlanets();

	/// <summary>Calculates the MAXOID between a minor planet and each of the eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the minor planet in AU.</param>
	/// <param name="eccentricity">The eccentricity of the minor planet's orbit.</param>
	/// <param name="inclinationDeg">The inclination of the minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node of the minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion of the minor planet's orbit in degrees.</param>
	/// <returns>A list of <see cref="MaxoidResult"/> records, one per planet, in order from Mercury to Neptune.</returns>
	/// <remarks>Uses a two-stage numerical algorithm: a coarse grid search (720 × 720 angular samples) to locate the approximate maximum, followed by coordinate-ascent refinement for high precision.</remarks>
	public static List<MaxoidResult> CalculateMaxoids(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg)
	{
		double[] maxoidValues = CalculateMaxoidsInPlanetOrder(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		List<MaxoidResult> results = new(capacity: Planets.Length);
		for (int i = 0; i < Planets.Length; i++)
		{
			results.Add(item: new MaxoidResult(PlanetName: Planets[i].Name, MaxoidAu: maxoidValues[i]));
		}
		return results;
	}

	/// <summary>Calculates MAXOIDs in planet order (Mercury…Neptune) as a fixed-size array.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the minor planet in AU.</param>
	/// <param name="eccentricity">The eccentricity of the minor planet's orbit.</param>
	/// <param name="inclinationDeg">The inclination of the minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node of the minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion of the minor planet's orbit in degrees.</param>
	/// <returns>An array of eight MAXOID values in AU ordered from Mercury to Neptune.</returns>
	/// <remarks>Optimized for bulk processing by computing the first-orbit coarse cache once and reusing it for all eight planet comparisons.</remarks>
	public static double[] CalculateMaxoidsInPlanetOrder(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg)
	{
		// Convert minor planet angles from degrees to radians
		double i1 = inclinationDeg * Math.PI / 180.0;
		double w1 = argumentPerihelionDeg * Math.PI / 180.0;
		double bigO1 = longitudeAscendingNodeDeg * Math.PI / 180.0;
		// Precompute trigonometric values for orbit 1 once per minor planet
		double cosO1 = Math.Cos(d: bigO1);
		double sinO1 = Math.Sin(a: bigO1);
		double cosi1 = Math.Cos(d: i1);
		double sini1 = Math.Sin(a: i1);
		double oneMinusE1Sq = 1.0 - (eccentricity * eccentricity);
		(double X, double Y, double Z)[] pos1Cache = ArrayPool<(double X, double Y, double Z)>.Shared.Rent(minimumLength: GridSteps);
		try
		{
			for (int idx = 0; idx < GridSteps; idx++)
			{
				double f = idx * GridStepSize;
				pos1Cache[idx] = OrbitPositionOptimized(
					a: semiMajorAxis, e: eccentricity, w: w1, f: f,
					cosO: cosO1, sinO: sinO1, cosi: cosi1, sini: sini1, oneMinusESq: oneMinusE1Sq);
			}
			double[] maxoidValues = new double[PrecomputedPlanets.Length];
			for (int i = 0; i < PrecomputedPlanets.Length; i++)
			{
				maxoidValues[i] = CalculateMaxoidUsingFirstOrbitCache(
					a1: semiMajorAxis, e1: eccentricity, w1: w1,
					cosO1: cosO1, sinO1: sinO1, cosi1: cosi1, sini1: sini1, oneMinusE1Sq: oneMinusE1Sq,
					pos1Cache: pos1Cache,
					planet: PrecomputedPlanets[i]);
			}
			return maxoidValues;
		}
		finally
		{
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos1Cache);
		}
	}

	/// <summary>Calculates the MAXOID between two minor planets using their Keplerian orbital elements.</summary>
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
	/// <returns>The MAXOID in AU between the two minor planets.</returns>
	/// <remarks>Uses the same two-stage numerical algorithm as <see cref="CalculateMaxoids"/>: a coarse grid search (720 × 720 angular samples) followed by coordinate-ascent refinement.</remarks>
	public static double CalculateMaxoidBetweenPlanetoids(
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
		// Calculate the MAXOID between the two orbits using the double-grid search algorithm
		return CalculateMaxoid(
			a1: semiMajorAxis1, e1: eccentricity1, i1: i1, w1: o1, bigO1: bigO1,
			a2: semiMajorAxis2, e2: eccentricity2, i2: i2, w2: o2, bigO2: bigO2);
	}

	/// <summary>Computes the MAXOID between two Keplerian orbits using a double-grid search with local refinement.</summary>
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
	/// <returns>The MAXOID in AU.</returns>
	/// <remarks>Stage 1: coarse double-grid search with <c>GridSteps = 720</c> steps per orbit (0.5° resolution). Stage 2: coordinate-ascent refinement starting from the best grid point, iteratively narrowing the search bracket until the step size falls below <c>1e-9</c> radians (≈ 0.2 milli-arcseconds), giving sub-milliarcsecond accuracy.</remarks>
	private static double CalculateMaxoid(
		double a1, double e1, double i1, double w1, double bigO1,
		double a2, double e2, double i2, double w2, double bigO2)
	{
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
					double f = idx * GridStepSize;
					pos1Cache[idx] = OrbitPositionOptimized(a: a1, e: e1, w: w1, f: f, cosO: cosO1, sinO: sinO1, cosi: cosi1, sini: sini1, oneMinusESq: oneMinusE1Sq);
					pos2Cache[idx] = OrbitPositionOptimized(a: a2, e: e2, w: w2, f: f, cosO: cosO2, sinO: sinO2, cosi: cosi2, sini: sini2, oneMinusESq: oneMinusE2Sq);
				});
			CoarseMaximumResult coarseMaximum = FindCoarseMaximum(pos1Cache: pos1Cache, pos2Cache: pos2Cache);
			// Local refinement via coordinate ascent with precomputed trig values
			return RefineMaximumOptimized(
				a1: a1, e1: e1, w1: w1, cosO1: cosO1, sinO1: sinO1, cosi1: cosi1, sini1: sini1, oneMinusE1Sq: oneMinusE1Sq,
				a2: a2, e2: e2, w2: w2, cosO2: cosO2, sinO2: sinO2, cosi2: cosi2, sini2: sini2, oneMinusE2Sq: oneMinusE2Sq,
				f1Start: coarseMaximum.BestF1, f2Start: coarseMaximum.BestF2, initialStep: GridStepSize,
				coarseMax: Math.Sqrt(d: coarseMaximum.MaxDistanceSquared));
		}
		finally
		{
			// Return rented arrays to pool
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos1Cache);
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos2Cache);
		}
	}

	/// <summary>Calculates the MAXOID to a planet while reusing a precomputed coarse cache for the first orbit.</summary>
	/// <param name="a1">Semi-major axis of orbit 1 (AU).</param>
	/// <param name="e1">Eccentricity of orbit 1.</param>
	/// <param name="w1">Argument of perihelion of orbit 1 (radians).</param>
	/// <param name="cosO1">Precomputed cos(Ω₁).</param>
	/// <param name="sinO1">Precomputed sin(Ω₁).</param>
	/// <param name="cosi1">Precomputed cos(i₁).</param>
	/// <param name="sini1">Precomputed sin(i₁).</param>
	/// <param name="oneMinusE1Sq">Precomputed 1 - e₁².</param>
	/// <param name="pos1Cache">Precomputed coarse position cache for orbit 1.</param>
	/// <param name="planet">Precomputed constants for the planetary orbit.</param>
	/// <returns>The refined MAXOID in AU.</returns>
	/// <remarks>Used by bulk planet calculations to avoid recomputing the first-orbit coarse samples eight times.</remarks>
	private static double CalculateMaxoidUsingFirstOrbitCache(
		double a1, double e1, double w1,
		double cosO1, double sinO1, double cosi1, double sini1, double oneMinusE1Sq,
		(double X, double Y, double Z)[] pos1Cache,
		PlanetComputationData planet)
	{
		(double X, double Y, double Z)[] pos2Cache = ArrayPool<(double X, double Y, double Z)>.Shared.Rent(minimumLength: GridSteps);
		try
		{
			_ = Parallel.For(
				fromInclusive: 0,
				toExclusive: GridSteps,
				parallelOptions: new ParallelOptions
				{
					MaxDegreeOfParallelism = Environment.ProcessorCount
				},
				body: idx =>
				{
					double f = idx * GridStepSize;
					pos2Cache[idx] = OrbitPositionOptimized(
						a: planet.SemiMajorAxis, e: planet.Eccentricity, w: planet.ArgumentPerihelionRad, f: f,
						cosO: planet.CosLongitudeAscendingNode, sinO: planet.SinLongitudeAscendingNode,
						cosi: planet.CosInclination, sini: planet.SinInclination, oneMinusESq: planet.OneMinusEccentricitySquared);
				});

			CoarseMaximumResult coarseMaximum = FindCoarseMaximum(pos1Cache: pos1Cache, pos2Cache: pos2Cache);
			return RefineMaximumOptimized(
				a1: a1, e1: e1, w1: w1, cosO1: cosO1, sinO1: sinO1, cosi1: cosi1, sini1: sini1, oneMinusE1Sq: oneMinusE1Sq,
				a2: planet.SemiMajorAxis, e2: planet.Eccentricity, w2: planet.ArgumentPerihelionRad,
				cosO2: planet.CosLongitudeAscendingNode, sinO2: planet.SinLongitudeAscendingNode, cosi2: planet.CosInclination, sini2: planet.SinInclination, oneMinusE2Sq: planet.OneMinusEccentricitySquared,
				f1Start: coarseMaximum.BestF1, f2Start: coarseMaximum.BestF2, initialStep: GridStepSize,
				coarseMax: Math.Sqrt(d: coarseMaximum.MaxDistanceSquared));
		}
		finally
		{
			ArrayPool<(double X, double Y, double Z)>.Shared.Return(array: pos2Cache);
		}
	}

	/// <summary>Finds the best coarse-grid candidate across two sampled orbits for the maximum distance.</summary>
	/// <param name="pos1Cache">Sampled positions for orbit 1.</param>
	/// <param name="pos2Cache">Sampled positions for orbit 2.</param>
	/// <returns>The best coarse-grid squared distance and associated anomaly pair.</returns>
	/// <remarks>Performs a lock-free parallel search and returns the best coarse starting point for local refinement.</remarks>
	private static CoarseMaximumResult FindCoarseMaximum(
		(double X, double Y, double Z)[] pos1Cache,
		(double X, double Y, double Z)[] pos2Cache)
	{
		long maxDistSquaredBits = BitConverter.DoubleToInt64Bits(value: 0.0);
		long bestF1Bits = 0;
		long bestF2Bits = 0;
		Partitioner<Tuple<int, int>> partitioner = Partitioner.Create(fromInclusive: 0, toExclusive: GridSteps, rangeSize: Math.Max(1, GridSteps / (Environment.ProcessorCount * 4)));
		_ = Parallel.ForEach(
			source: partitioner,
			parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
			body: range =>
			{
				double localMaxDistSq = 0.0;
				int localBestI = 0;
				int localBestJ = 0;
				for (int i = range.Item1; i < range.Item2; i++)
				{
					(double X, double Y, double Z) p1 = pos1Cache[i];
					if (Vector256.IsHardwareAccelerated && GridSteps >= Vector256<double>.Count)
					{
						int j = 0;
						int simdEnd = GridSteps - (GridSteps % 4);
						for (; j < simdEnd; j += 4)
						{
							for (int k = 0; k < 4; k++)
							{
								(double X, double Y, double Z) p2 = pos2Cache[j + k];
								double distSq = DistanceSquared(p1: p1, p2: p2);
								if (distSq > localMaxDistSq)
								{
									localMaxDistSq = distSq;
									localBestI = i;
									localBestJ = j + k;
								}
							}
						}
						for (; j < GridSteps; j++)
						{
							(double X, double Y, double Z) p2 = pos2Cache[j];
							double distSq = DistanceSquared(p1: p1, p2: p2);
							if (distSq > localMaxDistSq)
							{
								localMaxDistSq = distSq;
								localBestI = i;
								localBestJ = j;
							}
						}
					}
					else
					{
						for (int j = 0; j < GridSteps; j++)
						{
							(double X, double Y, double Z) p2 = pos2Cache[j];
							double distSq = DistanceSquared(p1: p1, p2: p2);
							if (distSq > localMaxDistSq)
							{
								localMaxDistSq = distSq;
								localBestI = i;
								localBestJ = j;
							}
						}
					}
				}
				while (true)
				{
					long currentMaxBits = Interlocked.Read(location: ref maxDistSquaredBits);
					double currentMax = BitConverter.Int64BitsToDouble(value: currentMaxBits);
					if (localMaxDistSq <= currentMax)
					{
						break;
					}
					long newMaxBits = BitConverter.DoubleToInt64Bits(value: localMaxDistSq);
					if (Interlocked.CompareExchange(location1: ref maxDistSquaredBits, value: newMaxBits, comparand: currentMaxBits) == currentMaxBits)
					{
						Interlocked.Exchange(location1: ref bestF1Bits, value: BitConverter.DoubleToInt64Bits(value: localBestI * GridStepSize));
						Interlocked.Exchange(location1: ref bestF2Bits, value: BitConverter.DoubleToInt64Bits(value: localBestJ * GridStepSize));
						break;
					}
				}
			});

		return new CoarseMaximumResult(
			MaxDistanceSquared: BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref maxDistSquaredBits)),
			BestF1: BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref bestF1Bits)),
			BestF2: BitConverter.Int64BitsToDouble(value: Interlocked.Read(location: ref bestF2Bits)));
	}

	/// <summary>Builds precomputed constants for all planetary comparison orbits.</summary>
	/// <returns>An array of precomputed per-planet constants in Mercury-to-Neptune order.</returns>
	private static PlanetComputationData[] BuildPrecomputedPlanets()
	{
		PlanetComputationData[] data = new PlanetComputationData[Planets.Length];
		for (int i = 0; i < Planets.Length; i++)
		{
			PlanetElements planet = Planets[i];
			double inclinationRad = planet.InclinationDeg * Math.PI / 180.0;
			double longitudeAscendingNodeRad = planet.LongitudeAscendingNodeDeg * Math.PI / 180.0;
			data[i] = new PlanetComputationData(
				Name: planet.Name,
				SemiMajorAxis: planet.SemiMajorAxis,
				Eccentricity: planet.Eccentricity,
				ArgumentPerihelionRad: planet.ArgumentPerihelionDeg * Math.PI / 180.0,
				CosLongitudeAscendingNode: Math.Cos(d: longitudeAscendingNodeRad),
				SinLongitudeAscendingNode: Math.Sin(a: longitudeAscendingNodeRad),
				CosInclination: Math.Cos(d: inclinationRad),
				SinInclination: Math.Sin(a: inclinationRad),
				OneMinusEccentricitySquared: 1.0 - (planet.Eccentricity * planet.Eccentricity));
		}
		return data;
	}

	/// <summary>Refines the MAXOID estimate using precomputed trigonometric values for maximum performance.</summary>
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
	/// <param name="f1Start">True anomaly of orbit 1 at the coarse maximum (radians).</param>
	/// <param name="f2Start">True anomaly of orbit 2 at the coarse maximum (radians).</param>
	/// <param name="initialStep">Half-width of the initial search bracket (radians).</param>
	/// <param name="coarseMax">The coarse maximum distance used as the initial best.</param>
	/// <returns>The refined MAXOID in AU.</returns>
	/// <remarks>Optimized version that reuses precomputed trigonometric values from the grid search phase.</remarks>
	private static double RefineMaximumOptimized(
		double a1, double e1, double w1, double cosO1, double sinO1, double cosi1, double sini1, double oneMinusE1Sq,
		double a2, double e2, double w2, double cosO2, double sinO2, double cosi2, double sini2, double oneMinusE2Sq,
		double f1Start, double f2Start, double initialStep, double coarseMax)
	{
		double f1 = f1Start;
		double f2 = f2Start;
		double maxDistSquared = coarseMax * coarseMax;
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
					// If this new point is farther apart, update the maximum and continue refining from there
					if (dSq > maxDistSquared)
					{
						maxDistSquared = dSq;
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

		return Math.Sqrt(d: maxDistSquared);
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
