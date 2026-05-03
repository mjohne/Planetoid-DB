// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods for calculating the Minimum Orbit Intersection Distance (MOID) between a minor planet and the eight solar system planets.</summary>
/// <remarks>This class implements a fast, high-precision numerical double-grid search algorithm with local refinement,
/// equivalent to the approach used by the Minor Planet Center (MPC) for computing MOID values.
/// The method samples both orbits on a fine angular grid to locate the global minimum distance,
/// then refines it using coordinate-descent optimization to achieve sub-milliarcsecond accuracy.</remarks>
internal class MoidCalculator
{
	/// <summary>Represents the Keplerian orbital elements of a solar system planet.</summary>
	/// <param name="Name">The common name of the planet.</param>
	/// <param name="SemiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="Eccentricity">The orbital eccentricity (dimensionless).</param>
	/// <param name="InclinationDeg">The orbital inclination to the ecliptic in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <remarks>These mean orbital elements are referenced to the J2000.0 ecliptic and equinox,
	/// as listed in standard astronomical references (Standish 1992 / IAU).</remarks>
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
	/// <remarks>The MOID is the closest geometric approach distance between the two osculating orbits,
	/// independent of the bodies' actual positions at any epoch.</remarks>
	public record MoidResult(string PlanetName, double MoidAu);

	/// <summary>Mean orbital elements of the eight solar system planets at J2000.0 (ecliptic reference frame).</summary>
	/// <remarks>Elements are taken from the standard IAU/JPL mean orbital elements
	/// (Standish, E.M. 1992, "Keplerian Elements for Approximate Planetary Positions").</remarks>
	private static readonly PlanetElements[] Planets =
	[
		new(Name: "Mercury", SemiMajorAxis: 0.387098, Eccentricity: 0.205630, InclinationDeg: 7.005, LongitudeAscendingNodeDeg: 48.331, ArgumentPerihelionDeg: 29.124),
		new(Name: "Venus", SemiMajorAxis: 0.723332, Eccentricity: 0.006773, InclinationDeg: 3.395, LongitudeAscendingNodeDeg: 76.680, ArgumentPerihelionDeg: 54.884),
		new(Name: "Earth", SemiMajorAxis: 1.000000, Eccentricity: 0.016709, InclinationDeg: 0.000, LongitudeAscendingNodeDeg: 0.000, ArgumentPerihelionDeg: 288.064),
		new(Name: "Mars", SemiMajorAxis: 1.523679, Eccentricity: 0.093400, InclinationDeg: 1.850, LongitudeAscendingNodeDeg: 49.558, ArgumentPerihelionDeg: 286.502),
		new(Name: "Jupiter", SemiMajorAxis: 5.202603, Eccentricity: 0.048498, InclinationDeg: 1.303, LongitudeAscendingNodeDeg: 100.464, ArgumentPerihelionDeg: 273.867),
		new(Name: "Saturn", SemiMajorAxis: 9.537070, Eccentricity: 0.054151, InclinationDeg: 2.489, LongitudeAscendingNodeDeg: 113.665, ArgumentPerihelionDeg: 339.392),
		new(Name: "Uranus", SemiMajorAxis: 19.19126, Eccentricity: 0.047168, InclinationDeg: 0.773, LongitudeAscendingNodeDeg: 74.006, ArgumentPerihelionDeg: 96.999),
		new(Name: "Neptune", SemiMajorAxis: 30.06896, Eccentricity: 0.008586, InclinationDeg: 1.770, LongitudeAscendingNodeDeg: 131.784, ArgumentPerihelionDeg: 272.846),
	];

	/// <summary>Calculates the MOID between a minor planet and each of the eight solar system planets.</summary>
	/// <param name="semiMajorAxis">The semi-major axis of the minor planet in AU.</param>
	/// <param name="eccentricity">The eccentricity of the minor planet's orbit.</param>
	/// <param name="inclinationDeg">The inclination of the minor planet's orbit to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node of the minor planet's orbit in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion of the minor planet's orbit in degrees.</param>
	/// <returns>A list of <see cref="MoidResult"/> records, one per planet, in order from Mercury to Neptune.</returns>
	/// <remarks>Uses a two-stage numerical algorithm: a coarse grid search (720 × 720 angular samples)
	/// to locate the approximate minimum, followed by coordinate-descent refinement for high precision.</remarks>
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
	/// <remarks>
	/// Stage 1: coarse double-grid search with <c>GridSteps = 720</c> steps per orbit (0.5° resolution).
	/// Stage 2: coordinate-descent refinement starting from the best grid point, iteratively
	/// narrowing the search bracket until the step size falls below <c>1e-9</c> radians
	/// (≈ 0.2 milli-arcseconds), giving sub-milliarcsecond accuracy.
	/// </remarks>
	private static double CalculateMoid(
		double a1, double e1, double i1, double w1, double bigO1,
		double a2, double e2, double i2, double w2, double bigO2)
	{
		// Number of angular samples per orbit for the coarse grid (0.5° resolution)
		const int GridSteps = 720;
		const double TwoPi = 2.0 * Math.PI;
		double stepSize = TwoPi / GridSteps;
		double minDist = double.MaxValue;
		double bestF1 = 0.0;
		double bestF2 = 0.0;
		// Precompute the Cartesian positions of orbit 1 at all grid points
		var pos1Cache = new (double X, double Y, double Z)[GridSteps];
		for (int idx = 0; idx < GridSteps; idx++)
		{
			pos1Cache[idx] = OrbitPosition(a: a1, e: e1, i: i1, w: w1, bigO: bigO1, f: idx * stepSize);
		}
		// Double-grid search: iterate over all (f1, f2) combinations
		for (int i = 0; i < GridSteps; i++)
		{
			var p1 = pos1Cache[i];
			double f1 = i * stepSize;
			for (int j = 0; j < GridSteps; j++)
			{
				double f2 = j * stepSize;
				var p2 = OrbitPosition(a: a2, e: e2, i: i2, w: w2, bigO: bigO2, f: f2);
				double dist = Distance(p1: p1, p2: p2);
				if (dist < minDist)
				{
					minDist = dist;
					bestF1 = f1;
					bestF2 = f2;
				}
			}
		}
		// Local refinement via coordinate descent
		return RefineMinimum(
			a1: a1, e1: e1, i1: i1, w1: w1, bigO1: bigO1,
			a2: a2, e2: e2, i2: i2, w2: w2, bigO2: bigO2,
			f1Start: bestF1, f2Start: bestF2, initialStep: stepSize,
			coarseMin: minDist);
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
	/// <remarks>Each iteration halves the step size. The loop terminates when the step size
	/// drops below <c>1e-9</c> radians or after a maximum of 60 iterations.</remarks>
	private static double RefineMinimum(
		double a1, double e1, double i1, double w1, double bigO1,
		double a2, double e2, double i2, double w2, double bigO2,
		double f1Start, double f2Start, double initialStep, double coarseMin)
	{
		double f1 = f1Start;
		double f2 = f2Start;
		double minDist = coarseMin;
		double step = initialStep;
		// Iteratively shrink the search bracket
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
					double nf1 = f1 + df1Sign * step;
					double nf2 = f2 + df2Sign * step;
					double d = Distance(
						p1: OrbitPosition(a: a1, e: e1, i: i1, w: w1, bigO: bigO1, f: nf1),
						p2: OrbitPosition(a: a2, e: e2, i: i2, w: w2, bigO: bigO2, f: nf2));
					if (d < minDist)
					{
						minDist = d;
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
		return minDist;
	}

	/// <summary>Computes the heliocentric Cartesian coordinates of a point on a Keplerian orbit.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="i">Inclination in radians.</param>
	/// <param name="w">Argument of perihelion in radians.</param>
	/// <param name="bigO">Longitude of the ascending node in radians.</param>
	/// <param name="f">True anomaly in radians.</param>
	/// <returns>The Cartesian position (X, Y, Z) in the ecliptic frame (AU).</returns>
	/// <remarks>Uses the standard orbit-in-space transformation:
	/// <c>r = a(1−e²)/(1+e·cos f)</c>, then rotates by ω+f, Ω, and i.</remarks>
	private static (double X, double Y, double Z) OrbitPosition(
		double a, double e, double i, double w, double bigO, double f)
	{
		// Heliocentric distance from the focal formula
		double r = a * (1.0 - e * e) / (1.0 + e * Math.Cos(f));
		// Argument of latitude u = ω + f
		double u = w + f;
		double cosO = Math.Cos(bigO);
		double sinO = Math.Sin(bigO);
		double cosu = Math.Cos(u);
		double sinu = Math.Sin(u);
		double cosi = Math.Cos(i);
		double sini = Math.Sin(i);
		// Standard perifocal-to-inertial rotation
		double x = r * (cosO * cosu - sinO * sinu * cosi);
		double y = r * (sinO * cosu + cosO * sinu * cosi);
		double z = r * sinu * sini;
		return (X: x, Y: y, Z: z);
	}

	/// <summary>Computes the Euclidean distance between two points in 3-D space.</summary>
	/// <param name="p1">The first point as (X, Y, Z).</param>
	/// <param name="p2">The second point as (X, Y, Z).</param>
	/// <returns>The distance between the two points.</returns>
	private static double Distance(
		(double X, double Y, double Z) p1,
		(double X, double Y, double Z) p2)
	{
		double dx = p1.X - p2.X;
		double dy = p1.Y - p2.Y;
		double dz = p1.Z - p2.Z;
		return Math.Sqrt(dx * dx + dy * dy + dz * dz);
	}
}
