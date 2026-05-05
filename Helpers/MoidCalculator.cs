// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Runtime.CompilerServices;

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

		// Precompute both orbits' Cartesian positions at all grid points
		(double X, double Y, double Z)[] pos1Cache = new (double X, double Y, double Z)[GridSteps];
		(double X, double Y, double Z)[] pos2Cache = new (double X, double Y, double Z)[GridSteps];

		for (int idx = 0; idx < GridSteps; idx++)
		{
			pos1Cache[idx] = OrbitPosition(a: a1, e: e1, i: i1, w: w1, bigO: bigO1, f: idx * stepSize);
			pos2Cache[idx] = OrbitPosition(a: a2, e: e2, i: i2, w: w2, bigO: bigO2, f: idx * stepSize);
		}

		// Parallel double-grid search using squared distances to avoid sqrt
		double minDistSquared = double.MaxValue;
		double bestF1 = 0.0;
		double bestF2 = 0.0;
		object lockObj = new();

		_ = Parallel.For(fromInclusive: 0, toExclusive: GridSteps, body: i =>
		{
			(double X, double Y, double Z) p1 = pos1Cache[i];
			double localMinDistSq = double.MaxValue;
			double localBestF1 = 0.0;
			double localBestF2 = 0.0;

			for (int j = 0; j < GridSteps; j++)
			{
				(double X, double Y, double Z) p2 = pos2Cache[j];
				double distSq = DistanceSquared(p1: p1, p2: p2);

				if (distSq < localMinDistSq)
				{
					localMinDistSq = distSq;
					localBestF1 = i * stepSize;
					localBestF2 = j * stepSize;
				}
			}

			// Thread-safe update of global minimum
			lock (lockObj)
			{
				if (localMinDistSq < minDistSquared)
				{
					minDistSquared = localMinDistSq;
					bestF1 = localBestF1;
					bestF2 = localBestF2;
				}
			}
		});

		// Local refinement via coordinate descent
		return RefineMinimum(
			a1: a1, e1: e1, i1: i1, w1: w1, bigO1: bigO1,
			a2: a2, e2: e2, i2: i2, w2: w2, bigO2: bigO2,
			f1Start: bestF1, f2Start: bestF2, initialStep: stepSize,
			coarseMin: Math.Sqrt(d: minDistSquared));
	}

	/// <summary>Refines the MOID estimate from a coarse grid minimum using coordinate descent.</summary>
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
	/// <param name="f1Start">True anomaly of orbit 1 at the coarse minimum (radians).</param>
	/// <param name="f2Start">True anomaly of orbit 2 at the coarse minimum (radians).</param>
	/// <param name="initialStep">Half-width of the initial search bracket (radians).</param>
	/// <param name="coarseMin">The coarse minimum distance used as the initial best.</param>
	/// <returns>The refined MOID in AU.</returns>
	/// <remarks>Each iteration halves the step size. The loop terminates when the step size drops below <c>1e-9</c> radians or after a maximum of 60 iterations.</remarks>
	private static double RefineMinimum(
		double a1, double e1, double i1, double w1, double bigO1,
		double a2, double e2, double i2, double w2, double bigO2,
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

					double nf1 = f1 + (df1Sign * step);
					double nf2 = f2 + (df2Sign * step);
					double dSq = DistanceSquared(
						p1: OrbitPosition(a: a1, e: e1, i: i1, w: w1, bigO: bigO1, f: nf1),
						p2: OrbitPosition(a: a2, e: e2, i: i2, w: w2, bigO: bigO2, f: nf2));

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

	/// <summary>Computes the heliocentric Cartesian coordinates of a point on a Keplerian orbit.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="i">Inclination in radians.</param>
	/// <param name="w">Argument of perihelion in radians.</param>
	/// <param name="bigO">Longitude of the ascending node in radians.</param>
	/// <param name="f">True anomaly in radians.</param>
	/// <returns>The Cartesian position (X, Y, Z) in the ecliptic frame (AU).</returns>
	/// <remarks>Uses the standard orbit-in-space transformation: <c>r = a(1−e²)/(1+e·cos f)</c>, then rotates by ω+f, Ω, and i.</remarks>
	[MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
	private static (double X, double Y, double Z) OrbitPosition(
		double a, double e, double i, double w, double bigO, double f)
	{
		// Heliocentric distance from the focal formula
		double r = a * (1.0 - (e * e)) / (1.0 + (e * Math.Cos(d: f)));
		// Argument of latitude u = ω + f
		double u = w + f;
		double cosO = Math.Cos(d: bigO);
		double sinO = Math.Sin(a: bigO);
		double cosu = Math.Cos(d: u);
		double sinu = Math.Sin(a: u);
		double cosi = Math.Cos(d: i);
		double sini = Math.Sin(a: i);
		// Standard perifocal-to-inertial rotation
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
