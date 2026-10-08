/*
 * File:        OrbitPropagationService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Propagates the orbit of a minor planet (two-body or with perturbations).
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Services;

/// <summary>Propagates the heliocentric orbit of a minor planet.</summary>
/// <remarks>
/// Two models are available: an analytical two-body (Kepler) solution and a numerical Cowell integration (classical fourth-order
/// Runge–Kutta with adaptive step size) that includes the direct and indirect gravitational perturbations of Mercury to Neptune
/// (Earth–Moon barycenter) taken from the configured <see cref="IPlanetaryEphemerisProvider"/>, and the post-Newtonian
/// (Schwarzschild) correction of the Sun. All vectors are heliocentric, ICRF/J2000 equatorial, in AU and AU/d; times are TDB.
/// </remarks>
/// <param name="planetaryEphemeris">The provider of planetary positions used for the perturbations.</param>
internal sealed class OrbitPropagationService(IPlanetaryEphemerisProvider planetaryEphemeris)
{
	/// <summary>Bodies included in the perturbation model.</summary>
	private static readonly SolarSystemBody[] PerturbingBodies =
	[
		SolarSystemBody.Mercury, SolarSystemBody.Venus, SolarSystemBody.EarthMoonBarycenter, SolarSystemBody.Mars,
		SolarSystemBody.Jupiter, SolarSystemBody.Saturn, SolarSystemBody.Uranus, SolarSystemBody.Neptune
	];

	/// <summary>Gravitational parameters of the perturbing bodies [AU³/d²], in the order of <see cref="PerturbingBodies"/>.</summary>
	private static readonly double[] PerturbingGm = [.. PerturbingBodies.Select(selector: b => AstronomicalConstants.SunGm / AstronomicalConstants.SunToBodyMassRatio[b])];

	/// <summary>Gets the planetary ephemeris used by this service.</summary>
	public IPlanetaryEphemerisProvider PlanetaryEphemeris { get; } = planetaryEphemeris;

	/// <summary>Computes the heliocentric state vector of a minor planet from its osculating elements (two-body motion).</summary>
	/// <param name="elements">The orbital elements.</param>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The heliocentric state vector, ICRF/J2000 equatorial.</returns>
	public static StateVector GetTwoBodyState(MinorPlanetOrbitalElements elements, double julianDateTdb)
	{
		ArgumentNullException.ThrowIfNull(argument: elements);
		double a = elements.SemiMajorAxisAu;
		double meanMotionDegPerDay = AstronomicalConstants.GaussianGravitationalConstant / (a * Math.Sqrt(d: a)) * AstronomicalConstants.RadiansToDegrees;
		// The MPC epoch is given in TT; the difference TT−TDB (< 2 ms) is negligible for the mean anomaly
		double meanAnomaly = elements.MeanAnomalyDegrees + (meanMotionDegPerDay * (julianDateTdb - elements.EpochJulianDateTt));
		(Vector3d position, Vector3d velocity) = KeplerState(
			semiMajorAxisAu: a,
			eccentricity: elements.Eccentricity,
			inclinationDegrees: elements.InclinationDegrees,
			longitudeOfAscendingNodeDegrees: elements.LongitudeOfAscendingNodeDegrees,
			argumentOfPerihelionDegrees: elements.ArgumentOfPerihelionDegrees,
			meanAnomalyDegrees: meanAnomaly);
		Matrix3d rotation = AstronomicalConstants.EclipticToEquatorialJ2000;
		return new StateVector(JulianDateTdb: julianDateTdb, Position: rotation * position, Velocity: rotation * velocity);
	}

	/// <summary>Propagates a state vector to a target time.</summary>
	/// <param name="state">The initial state.</param>
	/// <param name="targetJulianDateTdb">The target Julian date (TDB) [d]; may be before or after the initial epoch.</param>
	/// <param name="cancellationToken">A token to cancel long integrations.</param>
	/// <returns>The propagated state.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
	public StateVector Propagate(StateVector state, double targetJulianDateTdb, CancellationToken cancellationToken = default)
	{
		StateVector current = state;
		int iterations = 0;
		while (Math.Abs(value: targetJulianDateTdb - current.JulianDateTdb) > 1e-9)
		{
			if ((++iterations & 0xFF) == 0)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			double remaining = targetJulianDateTdb - current.JulianDateTdb;
			(Vector3d acceleration, double closestApproachAu) = Acceleration(julianDateTdb: current.JulianDateTdb, position: current.Position, velocity: current.Velocity);
			double maxStep = ComputeStepSize(state: current, closestApproachAu: closestApproachAu, acceleration: acceleration);
			double h = Math.Sign(value: remaining) * Math.Min(val1: Math.Abs(value: remaining), val2: maxStep);
			current = RungeKutta4Step(state: current, h: h, k1Acceleration: acceleration);
		}
		return current with { JulianDateTdb = targetJulianDateTdb };
	}

	/// <summary>Computes the heliocentric position from Keplerian elements in the reference plane of the elements.</summary>
	/// <param name="semiMajorAxisAu">The semi-major axis [AU].</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <param name="inclinationDegrees">The inclination [°].</param>
	/// <param name="longitudeOfAscendingNodeDegrees">The longitude of the ascending node [°].</param>
	/// <param name="argumentOfPerihelionDegrees">The argument of perihelion [°].</param>
	/// <param name="meanAnomalyDegrees">The mean anomaly [°].</param>
	/// <returns>The position [AU] in the reference frame of the elements.</returns>
	public static Vector3d KeplerPosition(double semiMajorAxisAu, double eccentricity, double inclinationDegrees, double longitudeOfAscendingNodeDegrees, double argumentOfPerihelionDegrees, double meanAnomalyDegrees) =>
		KeplerState(semiMajorAxisAu: semiMajorAxisAu, eccentricity: eccentricity, inclinationDegrees: inclinationDegrees, longitudeOfAscendingNodeDegrees: longitudeOfAscendingNodeDegrees, argumentOfPerihelionDegrees: argumentOfPerihelionDegrees, meanAnomalyDegrees: meanAnomalyDegrees).Position;

	/// <summary>Computes the heliocentric state from Keplerian elements in the reference plane of the elements.</summary>
	/// <param name="semiMajorAxisAu">The semi-major axis [AU].</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <param name="inclinationDegrees">The inclination [°].</param>
	/// <param name="longitudeOfAscendingNodeDegrees">The longitude of the ascending node [°].</param>
	/// <param name="argumentOfPerihelionDegrees">The argument of perihelion [°].</param>
	/// <param name="meanAnomalyDegrees">The mean anomaly [°].</param>
	/// <returns>The position [AU] and velocity [AU/d] in the reference frame of the elements.</returns>
	public static (Vector3d Position, Vector3d Velocity) KeplerState(double semiMajorAxisAu, double eccentricity, double inclinationDegrees, double longitudeOfAscendingNodeDegrees, double argumentOfPerihelionDegrees, double meanAnomalyDegrees)
	{
		const double d2r = AstronomicalConstants.DegreesToRadians;
		double e = eccentricity;
		double eccentricAnomaly = SolveKepler(meanAnomalyRadians: meanAnomalyDegrees * d2r, eccentricity: e);
		(double sinE, double cosE) = Math.SinCos(x: eccentricAnomaly);
		double sqrt1me2 = Math.Sqrt(d: 1.0 - (e * e));
		double x = semiMajorAxisAu * (cosE - e);
		double y = semiMajorAxisAu * sqrt1me2 * sinE;
		double n = AstronomicalConstants.GaussianGravitationalConstant / (semiMajorAxisAu * Math.Sqrt(d: semiMajorAxisAu));
		double factor = n * semiMajorAxisAu / (1.0 - (e * cosE));
		double vx = -factor * sinE;
		double vy = factor * sqrt1me2 * cosE;
		(double sw, double cw) = Math.SinCos(x: argumentOfPerihelionDegrees * d2r);
		(double sn, double cn) = Math.SinCos(x: longitudeOfAscendingNodeDegrees * d2r);
		(double si, double ci) = Math.SinCos(x: inclinationDegrees * d2r);
		Vector3d p = new(X: (cw * cn) - (sw * sn * ci), Y: (cw * sn) + (sw * cn * ci), Z: sw * si);
		Vector3d q = new(X: (-sw * cn) - (cw * sn * ci), Y: (-sw * sn) + (cw * cn * ci), Z: cw * si);
		return ((x * p) + (y * q), (vx * p) + (vy * q));
	}

	/// <summary>Solves Kepler's equation E − e·sin E = M with Newton's method.</summary>
	/// <param name="meanAnomalyRadians">The mean anomaly M [rad].</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <returns>The eccentric anomaly E [rad].</returns>
	public static double SolveKepler(double meanAnomalyRadians, double eccentricity)
	{
		double m = Math.IEEERemainder(x: meanAnomalyRadians, y: 2.0 * Math.PI);
		double e = eccentricity;
		double eccentricAnomaly = e < 0.8 ? m : Math.PI * Math.Sign(value: m == 0.0 ? 1.0 : m);
		for (int i = 0; i < 50; i++)
		{
			(double s, double c) = Math.SinCos(x: eccentricAnomaly);
			double delta = (eccentricAnomaly - (e * s) - m) / (1.0 - (e * c));
			eccentricAnomaly -= delta;
			if (Math.Abs(value: delta) < 1e-14)
			{
				break;
			}
		}
		return eccentricAnomaly;
	}

	/// <summary>Computes the heliocentric acceleration of the minor planet.</summary>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <param name="position">The heliocentric position [AU].</param>
	/// <param name="velocity">The heliocentric velocity [AU/d].</param>
	/// <returns>The acceleration [AU/d²] and the distance to the closest perturbing body [AU].</returns>
	private (Vector3d Acceleration, double ClosestApproachAu) Acceleration(double julianDateTdb, Vector3d position, Vector3d velocity)
	{
		double r = position.Length;
		double r3 = r * r * r;
		// Central (Keplerian) term
		Vector3d acceleration = position * (-AstronomicalConstants.SunGm / r3);
		// Post-Newtonian correction of the Sun (Schwarzschild, PPN β = γ = 1)
		double c2 = AstronomicalConstants.SpeedOfLightAuPerDay * AstronomicalConstants.SpeedOfLightAuPerDay;
		double v2 = velocity.Dot(other: velocity);
		acceleration += (AstronomicalConstants.SunGm / (c2 * r3)) * ((((4.0 * AstronomicalConstants.SunGm / r) - v2) * position) + (4.0 * position.Dot(other: velocity) * velocity));
		// Direct and indirect planetary perturbations
		double closest = double.MaxValue;
		for (int i = 0; i < PerturbingBodies.Length; i++)
		{
			Vector3d planet = PlanetaryEphemeris.GetHeliocentricPosition(body: PerturbingBodies[i], julianDateTdb: julianDateTdb);
			Vector3d d = planet - position;
			double dLength = d.Length;
			double pLength = planet.Length;
			closest = Math.Min(val1: closest, val2: dLength);
			acceleration += PerturbingGm[i] * ((d / (dLength * dLength * dLength)) - (planet / (pLength * pLength * pLength)));
		}
		return (acceleration, closest);
	}

	/// <summary>Determines the integration step size.</summary>
	/// <param name="state">The current state.</param>
	/// <param name="closestApproachAu">The distance to the closest perturbing body [AU].</param>
	/// <param name="acceleration">The current acceleration [AU/d²].</param>
	/// <returns>The maximum step size [d].</returns>
	private static double ComputeStepSize(StateVector state, double closestApproachAu, Vector3d acceleration)
	{
		double r = state.Position.Length;
		// About 1/400 of the local orbital period scale (r^1.5 days at 1 AU ≈ 1 day)
		double step = Math.Clamp(value: r * Math.Sqrt(d: r), min: 0.01, max: 4.0);
		// Resolve close planetary encounters
		double speed = Math.Max(val1: state.Velocity.Length, val2: 1e-6);
		step = Math.Min(val1: step, val2: Math.Max(val1: 0.001, val2: closestApproachAu / speed / 50.0));
		// Resolve strong accelerations (e.g. near perihelion)
		double accelerationLength = acceleration.Length;
		if (accelerationLength > 0.0)
		{
			step = Math.Min(val1: step, val2: Math.Max(val1: 0.001, val2: 0.02 * speed / accelerationLength));
		}
		return step;
	}

	/// <summary>Performs a single classical Runge–Kutta step.</summary>
	/// <param name="state">The current state.</param>
	/// <param name="h">The step size [d] (may be negative).</param>
	/// <param name="k1Acceleration">The acceleration at the start of the step.</param>
	/// <returns>The new state.</returns>
	private StateVector RungeKutta4Step(StateVector state, double h, Vector3d k1Acceleration)
	{
		double t = state.JulianDateTdb;
		Vector3d r = state.Position;
		Vector3d v = state.Velocity;
		Vector3d k1r = v;
		Vector3d k1v = k1Acceleration;
		Vector3d k2r = v + (0.5 * h * k1v);
		Vector3d k2v = Acceleration(julianDateTdb: t + (0.5 * h), position: r + (0.5 * h * k1r), velocity: k2r).Acceleration;
		Vector3d k3r = v + (0.5 * h * k2v);
		Vector3d k3v = Acceleration(julianDateTdb: t + (0.5 * h), position: r + (0.5 * h * k2r), velocity: k3r).Acceleration;
		Vector3d k4r = v + (h * k3v);
		Vector3d k4v = Acceleration(julianDateTdb: t + h, position: r + (h * k3r), velocity: k4r).Acceleration;
		Vector3d newPosition = r + (h / 6.0 * (k1r + (2.0 * k2r) + (2.0 * k3r) + k4r));
		Vector3d newVelocity = v + (h / 6.0 * (k1v + (2.0 * k2v) + (2.0 * k3v) + k4v));
		return new StateVector(JulianDateTdb: t + h, Position: newPosition, Velocity: newVelocity);
	}
}
