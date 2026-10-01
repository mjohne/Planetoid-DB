/*
 * File:        OrbitPropagationService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Propagates the heliocentric state of a minor planet from its osculating elements.
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

/// <summary>Represents a heliocentric state vector.</summary>
/// <param name="JulianDateTdb">The Julian date (TDB) of the state.</param>
/// <param name="Position">The position in AU (equatorial ICRF/J2000.0).</param>
/// <param name="Velocity">The velocity in AU/day (equatorial ICRF/J2000.0).</param>
internal readonly record struct StateVector(double JulianDateTdb, Vector3D Position, Vector3D Velocity);

/// <summary>Propagates the heliocentric state of a minor planet from its osculating elements.</summary>
/// <remarks>
/// <para><see cref="PropagationModel.TwoBody"/> solves Kepler's equation analytically.</para>
/// <para><see cref="PropagationModel.Perturbed"/> integrates the equations of motion numerically (Cowell's method, adaptive Dormand–Prince 5(4))
/// including the direct and indirect perturbations of Mercury, Venus, Earth, Moon, Mars, Jupiter, Saturn, Uranus and Neptune,
/// taken from the configured <see cref="IPlanetaryEphemeris"/> (e.g. JPL DE440/DE441), and the post-Newtonian (general relativistic) correction of the Sun.</para>
/// </remarks>
internal sealed class OrbitPropagationService
{
	/// <summary>The bodies acting as perturbers.</summary>
	private static readonly SolarSystemBody[] Perturbers =
	[
		SolarSystemBody.Mercury, SolarSystemBody.Venus, SolarSystemBody.Earth, SolarSystemBody.Moon, SolarSystemBody.Mars,
		SolarSystemBody.Jupiter, SolarSystemBody.Saturn, SolarSystemBody.Uranus, SolarSystemBody.Neptune
	];

	/// <summary>The planetary ephemeris used for perturbations.</summary>
	private readonly IPlanetaryEphemeris ephemeris;

	/// <summary>Gravitational parameters (AU³/day²) of the perturbers.</summary>
	private readonly double[] perturberGm;

	/// <summary>Initializes a new instance of the <see cref="OrbitPropagationService"/> class.</summary>
	/// <param name="ephemeris">The planetary ephemeris used for perturbations.</param>
	public OrbitPropagationService(IPlanetaryEphemeris ephemeris)
	{
		this.ephemeris = ephemeris;
		perturberGm = [.. Perturbers.Select(selector: b => AstronomicalConstants.GmSun / AstronomicalConstants.GetSunToBodyMassRatio(body: b))];
	}

	/// <summary>Gets or sets the relative integration tolerance.</summary>
	public double RelativeTolerance { get; init; } = 1e-12;

	/// <summary>Gets or sets the maximum integration step in days.</summary>
	public double MaximumStepDays { get; init; } = 10.0;

	/// <summary>Creates a propagation session for a minor planet.</summary>
	/// <param name="elements">The orbital elements.</param>
	/// <param name="model">The propagation model.</param>
	/// <returns>A stateful session that efficiently propagates to consecutive times.</returns>
	public OrbitPropagationSession CreateSession(MinorPlanetElements elements, PropagationModel model) => new(owner: this, initial: GetStateAtEpoch(elements: elements), model: model);

	/// <summary>Calculates the heliocentric state at the osculation epoch.</summary>
	/// <param name="elements">The orbital elements.</param>
	/// <returns>The state vector at epoch (equatorial ICRF/J2000.0).</returns>
	public static StateVector GetStateAtEpoch(MinorPlanetElements elements)
	{
		double meanMotion = AstronomicalConstants.GaussianGravitationalConstant / Math.Pow(x: elements.SemiMajorAxisAu, y: 1.5);
		(Vector3D position, Vector3D velocity) = GetEclipticStateFromElements(
			semiMajorAxisAu: elements.SemiMajorAxisAu,
			eccentricity: elements.Eccentricity,
			inclinationRadians: elements.InclinationDegrees * AstronomicalConstants.DegreesToRadians,
			longitudeOfAscendingNodeRadians: elements.LongitudeOfAscendingNodeDegrees * AstronomicalConstants.DegreesToRadians,
			argumentOfPerihelionRadians: elements.ArgumentOfPerihelionDegrees * AstronomicalConstants.DegreesToRadians,
			meanAnomalyRadians: elements.MeanAnomalyDegrees * AstronomicalConstants.DegreesToRadians,
			meanMotionRadiansPerDay: meanMotion);
		// The osculation epoch is given in TT; the difference to TDB (< 2 ms) is negligible here
		return new StateVector(
			JulianDateTdb: elements.EpochJulianDateTt,
			Position: CoordinateTransformationService.EclipticToEquatorial(ecliptic: position),
			Velocity: CoordinateTransformationService.EclipticToEquatorial(ecliptic: velocity));
	}

	/// <summary>Propagates a state vector along an unperturbed Keplerian orbit around the Sun.</summary>
	/// <param name="state">The initial state.</param>
	/// <param name="julianDateTdb">The target Julian date (TDB).</param>
	/// <returns>The propagated state.</returns>
	public static StateVector PropagateTwoBody(StateVector state, double julianDateTdb)
	{
		// Convert the state into elements in the (arbitrary) frame of the state, advance the mean anomaly and convert back
		const double mu = AstronomicalConstants.GmSun;
		Vector3D r = state.Position, v = state.Velocity;
		Vector3D h = r.Cross(other: v);
		Vector3D eVec = (v.Cross(other: h) / mu) - r.Normalize();
		double e = eVec.Length;
		double a = 1.0 / ((2.0 / r.Length) - (v.Dot(other: v) / mu));
		if (a <= 0 || e >= 1.0)
		{
			throw new InvalidOperationException(message: "Two-body propagation requires an elliptical orbit.");
		}
		double n = Math.Sqrt(d: mu / (a * a * a));
		// Perifocal basis: P towards perihelion, Q in the orbital plane
		Vector3D w = h.Normalize();
		Vector3D p = e > 1e-12 ? eVec / e : r.Normalize();
		Vector3D q = w.Cross(other: p);
		double cosE0 = (1.0 - (r.Length / a)) / Math.Max(val1: e, val2: 1e-300);
		double sinE0 = r.Dot(other: v) / (e * Math.Sqrt(d: mu * a));
		double e0 = e > 1e-12 ? Math.Atan2(y: sinE0, x: cosE0) : Math.Atan2(y: r.Dot(other: q), x: r.Dot(other: p));
		double m0 = e0 - (e * Math.Sin(a: e0));
		double m = m0 + (n * (julianDateTdb - state.JulianDateTdb));
		double eccentricAnomaly = SolveKepler(meanAnomalyRadians: m, eccentricity: e);
		(double sinE, double cosE) = Math.SinCos(x: eccentricAnomaly);
		double sqrt1me2 = Math.Sqrt(d: 1.0 - (e * e));
		double denominator = 1.0 - (e * cosE);
		Vector3D position = (p * (a * (cosE - e))) + (q * (a * sqrt1me2 * sinE));
		Vector3D velocity = (p * (-a * n * sinE / denominator)) + (q * (a * n * sqrt1me2 * cosE / denominator));
		return new StateVector(JulianDateTdb: julianDateTdb, Position: position, Velocity: velocity);
	}

	/// <summary>Solves Kepler's equation M = E − e·sin(E) for elliptical orbits.</summary>
	/// <param name="meanAnomalyRadians">The mean anomaly in radians.</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <returns>The eccentric anomaly in radians.</returns>
	public static double SolveKepler(double meanAnomalyRadians, double eccentricity)
	{
		double m = Math.IEEERemainder(x: meanAnomalyRadians, y: 2.0 * Math.PI);
		double e = eccentricity < 0.8 ? m + (eccentricity * Math.Sin(a: m)) : Math.PI * Math.Sign(value: m == 0 ? 1 : m);
		for (int i = 0; i < 100; i++)
		{
			double delta = (e - (eccentricity * Math.Sin(a: e)) - m) / (1.0 - (eccentricity * Math.Cos(d: e)));
			e -= delta;
			if (Math.Abs(value: delta) < 1e-15)
			{
				break;
			}
		}
		return e;
	}

	/// <summary>Calculates the position in the ecliptic J2000.0 frame from Keplerian elements.</summary>
	/// <param name="semiMajorAxisAu">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The eccentricity.</param>
	/// <param name="inclinationRadians">The inclination in radians.</param>
	/// <param name="longitudeOfAscendingNodeRadians">The longitude of the ascending node in radians.</param>
	/// <param name="argumentOfPerihelionRadians">The argument of perihelion in radians.</param>
	/// <param name="meanAnomalyRadians">The mean anomaly in radians.</param>
	/// <returns>The position in AU.</returns>
	public static Vector3D GetEclipticPositionFromElements(double semiMajorAxisAu, double eccentricity, double inclinationRadians, double longitudeOfAscendingNodeRadians, double argumentOfPerihelionRadians, double meanAnomalyRadians)
		=> GetEclipticStateFromElements(semiMajorAxisAu: semiMajorAxisAu, eccentricity: eccentricity, inclinationRadians: inclinationRadians, longitudeOfAscendingNodeRadians: longitudeOfAscendingNodeRadians,
			argumentOfPerihelionRadians: argumentOfPerihelionRadians, meanAnomalyRadians: meanAnomalyRadians, meanMotionRadiansPerDay: 0.0).Position;

	/// <summary>Calculates the heliocentric acceleration of a massless body.</summary>
	/// <param name="julianDateTdb">The Julian date (TDB).</param>
	/// <param name="position">The heliocentric position in AU.</param>
	/// <param name="velocity">The heliocentric velocity in AU/day.</param>
	/// <returns>The acceleration in AU/day².</returns>
	internal Vector3D GetPerturbedAcceleration(double julianDateTdb, Vector3D position, Vector3D velocity)
	{
		const double mu = AstronomicalConstants.GmSun;
		double r = position.Length;
		double r3 = r * r * r;
		Vector3D acceleration = position * (-mu / r3);
		// Post-Newtonian correction of the Sun (PPN, β = γ = 1)
		const double c2 = AstronomicalConstants.SpeedOfLightAuPerDay * AstronomicalConstants.SpeedOfLightAuPerDay;
		acceleration += ((position * ((4.0 * mu / r) - velocity.Dot(other: velocity))) + (velocity * (4.0 * position.Dot(other: velocity)))) * (mu / (c2 * r3));
		// Direct and indirect planetary perturbations
		for (int i = 0; i < Perturbers.Length; i++)
		{
			Vector3D planet = ephemeris.GetHeliocentricPosition(body: Perturbers[i], julianDateTdb: julianDateTdb);
			Vector3D d = planet - position;
			double dLen = d.Length;
			double pLen = planet.Length;
			acceleration += ((d / (dLen * dLen * dLen)) - (planet / (pLen * pLen * pLen))) * perturberGm[i];
		}
		return acceleration;
	}

	/// <summary>Calculates the position and velocity in the ecliptic J2000.0 frame from Keplerian elements.</summary>
	/// <param name="semiMajorAxisAu">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The eccentricity.</param>
	/// <param name="inclinationRadians">The inclination in radians.</param>
	/// <param name="longitudeOfAscendingNodeRadians">The longitude of the ascending node in radians.</param>
	/// <param name="argumentOfPerihelionRadians">The argument of perihelion in radians.</param>
	/// <param name="meanAnomalyRadians">The mean anomaly in radians.</param>
	/// <param name="meanMotionRadiansPerDay">The mean motion in radians per day.</param>
	/// <returns>The position in AU and the velocity in AU/day.</returns>
	private static (Vector3D Position, Vector3D Velocity) GetEclipticStateFromElements(double semiMajorAxisAu, double eccentricity, double inclinationRadians, double longitudeOfAscendingNodeRadians, double argumentOfPerihelionRadians, double meanAnomalyRadians, double meanMotionRadiansPerDay)
	{
		double eccentricAnomaly = SolveKepler(meanAnomalyRadians: meanAnomalyRadians, eccentricity: eccentricity);
		(double sinE, double cosE) = Math.SinCos(x: eccentricAnomaly);
		double sqrt1me2 = Math.Sqrt(d: 1.0 - (eccentricity * eccentricity));
		double xOrbit = semiMajorAxisAu * (cosE - eccentricity);
		double yOrbit = semiMajorAxisAu * sqrt1me2 * sinE;
		double denominator = 1.0 - (eccentricity * cosE);
		double vxOrbit = -semiMajorAxisAu * meanMotionRadiansPerDay * sinE / denominator;
		double vyOrbit = semiMajorAxisAu * meanMotionRadiansPerDay * sqrt1me2 * cosE / denominator;
		(double sinW, double cosW) = Math.SinCos(x: argumentOfPerihelionRadians);
		(double sinO, double cosO) = Math.SinCos(x: longitudeOfAscendingNodeRadians);
		(double sinI, double cosI) = Math.SinCos(x: inclinationRadians);
		Vector3D p = new(X: (cosW * cosO) - (sinW * sinO * cosI), Y: (cosW * sinO) + (sinW * cosO * cosI), Z: sinW * sinI);
		Vector3D q = new(X: (-sinW * cosO) - (cosW * sinO * cosI), Y: (-sinW * sinO) + (cosW * cosO * cosI), Z: cosW * sinI);
		return ((p * xOrbit) + (q * yOrbit), (p * vxOrbit) + (q * vyOrbit));
	}

	/// <summary>Represents a stateful propagation of one minor planet.</summary>
	/// <remarks>Consecutive requests continue the integration from the last state, so a sorted time series is integrated only once.</remarks>
	internal sealed class OrbitPropagationSession
	{
		/// <summary>Dormand–Prince node coefficients.</summary>
		private static readonly double[] C = [0, 1.0 / 5, 3.0 / 10, 4.0 / 5, 8.0 / 9, 1, 1];

		/// <summary>Dormand–Prince stage coefficients.</summary>
		private static readonly double[][] A =
		[
			[],
			[1.0 / 5],
			[3.0 / 40, 9.0 / 40],
			[44.0 / 45, -56.0 / 15, 32.0 / 9],
			[19372.0 / 6561, -25360.0 / 2187, 64448.0 / 6561, -212.0 / 729],
			[9017.0 / 3168, -355.0 / 33, 46732.0 / 5247, 49.0 / 176, -5103.0 / 18656],
			[35.0 / 384, 0, 500.0 / 1113, 125.0 / 192, -2187.0 / 6784, 11.0 / 84]
		];

		/// <summary>Dormand–Prince error estimation coefficients (5th minus 4th order weights).</summary>
		private static readonly double[] E = [71.0 / 57600, 0, -71.0 / 16695, 71.0 / 1920, -17253.0 / 339200, 22.0 / 525, -1.0 / 40];

		/// <summary>The owning propagation service.</summary>
		private readonly OrbitPropagationService owner;

		/// <summary>The state at the osculation epoch.</summary>
		private readonly StateVector initial;

		/// <summary>The propagation model.</summary>
		private readonly PropagationModel model;

		/// <summary>The current integration state.</summary>
		private StateVector current;

		/// <summary>The last successful step size in days (signed).</summary>
		private double lastStep = 1.0;

		/// <summary>Initializes a new instance of the <see cref="OrbitPropagationSession"/> class.</summary>
		/// <param name="owner">The owning service.</param>
		/// <param name="initial">The state at epoch.</param>
		/// <param name="model">The propagation model.</param>
		public OrbitPropagationSession(OrbitPropagationService owner, StateVector initial, PropagationModel model)
		{
			this.owner = owner;
			this.initial = initial;
			this.model = model;
			current = initial;
		}

		/// <summary>Propagates the minor planet to the given time.</summary>
		/// <param name="julianDateTdb">The target Julian date (TDB).</param>
		/// <param name="cancellationToken">A token to cancel long integrations.</param>
		/// <returns>The heliocentric state at the requested time.</returns>
		public StateVector PropagateTo(double julianDateTdb, CancellationToken cancellationToken = default)
		{
			if (model == PropagationModel.TwoBody)
			{
				return PropagateTwoBody(state: initial, julianDateTdb: julianDateTdb);
			}
			// Restart from the epoch if the target lies on the other side of the epoch than the current state would require crossing
			if (Math.Abs(value: julianDateTdb - initial.JulianDateTdb) < Math.Abs(value: julianDateTdb - current.JulianDateTdb))
			{
				current = initial;
			}
			current = Integrate(state: current, targetJulianDateTdb: julianDateTdb, cancellationToken: cancellationToken);
			return current;
		}

		/// <summary>Integrates the equations of motion with an adaptive Dormand–Prince 5(4) method.</summary>
		/// <param name="state">The initial state.</param>
		/// <param name="targetJulianDateTdb">The target Julian date (TDB).</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The state at the target time.</returns>
		private StateVector Integrate(StateVector state, double targetJulianDateTdb, CancellationToken cancellationToken)
		{
			double t = state.JulianDateTdb;
			double span = targetJulianDateTdb - t;
			if (span == 0.0)
			{
				return state;
			}
			double direction = Math.Sign(value: span);
			double h = direction * Math.Min(val1: Math.Abs(value: lastStep), val2: owner.MaximumStepDays);
			double[] y = [state.Position.X, state.Position.Y, state.Position.Z, state.Velocity.X, state.Velocity.Y, state.Velocity.Z];
			double[][] k = [.. Enumerable.Range(start: 0, count: 7).Select(selector: _ => new double[6])];
			double[] yStage = new double[6];
			double[] yNew = new double[6];
			while (direction * (targetJulianDateTdb - t) > 1e-12)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (direction * (t + h - targetJulianDateTdb) > 0.0)
				{
					h = targetJulianDateTdb - t;
				}
				Derivative(t: t, y: y, dy: k[0]);
				for (int stage = 1; stage < 7; stage++)
				{
					for (int j = 0; j < 6; j++)
					{
						double sum = 0.0;
						for (int m = 0; m < stage; m++)
						{
							sum += A[stage][m] * k[m][j];
						}
						yStage[j] = y[j] + (h * sum);
					}
					Derivative(t: t + (C[stage] * h), y: yStage, dy: k[stage]);
					if (stage == 6)
					{
						Array.Copy(sourceArray: yStage, destinationArray: yNew, length: 6);
					}
				}
				double error = 0.0;
				for (int j = 0; j < 6; j++)
				{
					double errorEstimate = 0.0;
					for (int m = 0; m < 7; m++)
					{
						errorEstimate += E[m] * k[m][j];
					}
					double scale = 1e-16 + (owner.RelativeTolerance * Math.Max(val1: Math.Abs(value: y[j]), val2: Math.Abs(value: yNew[j])));
					error = Math.Max(val1: error, val2: Math.Abs(value: h * errorEstimate) / scale);
				}
				if (error <= 1.0)
				{
					t += h;
					Array.Copy(sourceArray: yNew, destinationArray: y, length: 6);
					lastStep = h;
				}
				double factor = error == 0.0 ? 5.0 : Math.Clamp(value: 0.9 * Math.Pow(x: error, y: -0.2), min: 0.2, max: 5.0);
				h = direction * Math.Min(val1: Math.Abs(value: h * factor), val2: owner.MaximumStepDays);
				if (Math.Abs(value: h) < 1e-10)
				{
					throw new InvalidOperationException(message: "The orbit integration step size became too small (close encounter or singularity).");
				}
			}
			return new StateVector(JulianDateTdb: targetJulianDateTdb, Position: new Vector3D(X: y[0], Y: y[1], Z: y[2]), Velocity: new Vector3D(X: y[3], Y: y[4], Z: y[5]));
		}

		/// <summary>Calculates the time derivative of the state vector.</summary>
		/// <param name="t">The Julian date (TDB).</param>
		/// <param name="y">The state (position and velocity).</param>
		/// <param name="dy">The derivative (velocity and acceleration).</param>
		private void Derivative(double t, double[] y, double[] dy)
		{
			Vector3D position = new(X: y[0], Y: y[1], Z: y[2]);
			Vector3D velocity = new(X: y[3], Y: y[4], Z: y[5]);
			Vector3D acceleration = owner.GetPerturbedAcceleration(julianDateTdb: t, position: position, velocity: velocity);
			dy[0] = y[3];
			dy[1] = y[4];
			dy[2] = y[5];
			dy[3] = acceleration.X;
			dy[4] = acceleration.Y;
			dy[5] = acceleration.Z;
		}
	}
}
