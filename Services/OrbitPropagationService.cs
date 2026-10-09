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

using System.Diagnostics;

namespace Planetoid_DB.Services;

/// <summary>Propagates the heliocentric orbit of a minor planet.</summary>
/// <remarks>Two models are available: an analytical two-body (Kepler) solution and a numerical Cowell integration (classical fourth-order Runge–Kutta with adaptive step size) that includes the direct and indirect gravitational perturbations of Mercury to Neptune (Earth–Moon barycenter) taken from the configured <see cref="IPlanetaryEphemerisProvider"/>, and the post-Newtonian (Schwarzschild) correction of the Sun. All vectors are heliocentric, ICRF/J2000 equatorial, in AU and AU/d; times are TDB.</remarks>
/// <param name="planetaryEphemeris">The provider of planetary positions used for the perturbations.</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class OrbitPropagationService(IPlanetaryEphemerisProvider planetaryEphemeris)
{
	/// <summary>Bodies included in the perturbation model.</summary>
	/// <remarks>Earth–Moon barycenter is used instead of the Earth and Moon separately.</remarks>
	private static readonly SolarSystemBody[] PerturbingBodies =
	[
		SolarSystemBody.Mercury, SolarSystemBody.Venus, SolarSystemBody.EarthMoonBarycenter, SolarSystemBody.Mars,
		SolarSystemBody.Jupiter, SolarSystemBody.Saturn, SolarSystemBody.Uranus, SolarSystemBody.Neptune
	];

	/// <summary>Gravitational parameters of the perturbing bodies [AU³/d²], in the order of <see cref="PerturbingBodies"/>.</summary>
	/// <remarks>Computed from the Sun's GM and the Sun/body mass ratios.</remarks>
	private static readonly double[] PerturbingGm = [.. PerturbingBodies.Select(selector: b => AstronomicalConstants.SunGm / AstronomicalConstants.SunToBodyMassRatio[key: b])];

	/// <summary>Gets the planetary ephemeris used by this service.</summary>
	/// <remarks>This property exposes the <see cref="IPlanetaryEphemerisProvider"/> instance used for computing the perturbations of the minor planet's orbit.</remarks>
	public IPlanetaryEphemerisProvider PlanetaryEphemeris { get; } = planetaryEphemeris;

	/// <summary>Computes the heliocentric state vector of a minor planet from its osculating elements (two-body motion).</summary>
	/// <param name="elements">The orbital elements.</param>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The heliocentric state vector, ICRF/J2000 equatorial.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the semi-major axis or either Julian date is invalid.</exception>
	/// <remarks>The MPC epoch is given in TT; the difference TT−TDB (&lt; 2 ms) is negligible for the mean anomaly.</remarks>
	public static StateVector GetTwoBodyState(MinorPlanetOrbitalElements elements, double julianDateTdb)
	{
		// Validate the input elements and throw an exception if they are null
		ArgumentNullException.ThrowIfNull(argument: elements);
		// Validate the input elements and throw an exception if the semi-major axis is not positive
		double a = elements.SemiMajorAxisAu;
		if (!double.IsFinite(d: a) || a <= 0.0)
		{
			// Validate the input elements and throw an exception if the semi-major axis is not finite and positive
			throw new ArgumentOutOfRangeException(paramName: nameof(elements), actualValue: a, message: "The semi-major axis must be finite and positive.");
		}
		if (!double.IsFinite(d: julianDateTdb))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(julianDateTdb), actualValue: julianDateTdb, message: "The Julian date must be finite.");
		}
		if (!double.IsFinite(d: elements.EpochJulianDateTt))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(elements), actualValue: elements.EpochJulianDateTt, message: "The orbital epoch must be finite.");
		}
		// Compute the mean motion [°/d] from the semi-major axis [AU] using Kepler's third law
		double meanMotionDegPerDay = AstronomicalConstants.GaussianGravitationalConstant / (a * Math.Sqrt(d: a)) * AstronomicalConstants.RadiansToDegrees;
		// Compute the mean anomaly at the target time [°] by propagating from the epoch using the mean motion. The MPC epoch is given in TT; the difference TT−TDB (< 2 ms) is negligible for the mean anomaly.
		double meanAnomaly = elements.MeanAnomalyDegrees + (meanMotionDegPerDay * (julianDateTdb - elements.EpochJulianDateTt));
		// Normalize the mean anomaly to the range [0°, 360°)
		(Vector3d position, Vector3d velocity) = KeplerState(
			semiMajorAxisAu: a,
			eccentricity: elements.Eccentricity,
			inclinationDegrees: elements.InclinationDegrees,
			longitudeOfAscendingNodeDegrees: elements.LongitudeOfAscendingNodeDegrees,
			argumentOfPerihelionDegrees: elements.ArgumentOfPerihelionDegrees,
			meanAnomalyDegrees: meanAnomaly);
		// Convert the position and velocity from the reference plane of the elements (ecliptic) to the ICRF/J2000 equatorial frame
		Matrix3d rotation = AstronomicalConstants.EclipticToEquatorialJ2000;
		// Return the state vector with the computed position and velocity
		return new StateVector(JulianDateTdb: julianDateTdb, Position: rotation * position, Velocity: rotation * velocity);
	}

	/// <summary>Propagates a state vector to a target time.</summary>
	/// <param name="state">The initial state.</param>
	/// <param name="targetJulianDateTdb">The target Julian date (TDB) [d]; may be before or after the initial epoch.</param>
	/// <param name="cancellationToken">A token to cancel long integrations.</param>
	/// <returns>The propagated state.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when either Julian date is non-finite.</exception>
	/// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
	/// <remarks>This method uses a classical fourth-order Runge–Kutta integration with adaptive step size, including the direct and indirect gravitational perturbations of Mercury to Neptune (Earth–Moon barycenter) taken from the configured <see cref="IPlanetaryEphemerisProvider"/>, and the post-Newtonian (Schwarzschild) correction of the Sun. The integration stops when the target time is reached within a tolerance of 1e-9 days (~0.086 ms).</remarks>
	public StateVector Propagate(StateVector state, double targetJulianDateTdb, CancellationToken cancellationToken = default)
	{
		// Validate both dates before calculating the remaining time
		if (!double.IsFinite(d: state.JulianDateTdb))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(state), actualValue: state.JulianDateTdb, message: "The initial Julian date must be finite.");
		}
		if (!double.IsFinite(d: targetJulianDateTdb))
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(targetJulianDateTdb), actualValue: targetJulianDateTdb, message: "The target Julian date must be finite.");
		}
		StateVector current = state;
		int iterations = 0;
		while (Math.Abs(value: targetJulianDateTdb - current.JulianDateTdb) > 1e-9)
		{
			// Check for cancellation every 256 iterations to avoid excessive overhead
			if ((++iterations & 0xFF) == 0)
			{
				// Throw an OperationCanceledException if the cancellation token has been triggered
				cancellationToken.ThrowIfCancellationRequested();
			}
			// Compute the remaining time to the target date
			double remaining = targetJulianDateTdb - current.JulianDateTdb;
			// Compute the acceleration and the distance to the closest perturbing body at the current state
			(Vector3d acceleration, double closestApproachAu) = Acceleration(julianDateTdb: current.JulianDateTdb, position: current.Position, velocity: current.Velocity);
			// Compute the maximum step size based on the current state, closest approach, and acceleration
			double maxStep = ComputeStepSize(state: current, closestApproachAu: closestApproachAu, acceleration: acceleration);
			// Limit the step size to the remaining time to the target date, preserving the sign of the remaining time
			double h = Math.Sign(value: remaining) * Math.Min(val1: Math.Abs(value: remaining), val2: maxStep);
			// Perform a single Runge–Kutta step with the computed step size and acceleration
			current = RungeKutta4Step(state: current, h: h, k1Acceleration: acceleration);
		}
		// Return the final state with the target Julian date
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
	/// <remarks>This method is a convenience wrapper around <see cref="KeplerState(double,double,double,double,double,double)"/> that returns only the position vector.</remarks>
	public static Vector3d KeplerPosition(double semiMajorAxisAu, double eccentricity, double inclinationDegrees, double longitudeOfAscendingNodeDegrees, double argumentOfPerihelionDegrees, double meanAnomalyDegrees)
	{
		// Call the KeplerState method to compute the position and velocity, and return only the position
		return KeplerState(semiMajorAxisAu: semiMajorAxisAu, eccentricity: eccentricity, inclinationDegrees: inclinationDegrees, longitudeOfAscendingNodeDegrees: longitudeOfAscendingNodeDegrees, argumentOfPerihelionDegrees: argumentOfPerihelionDegrees, meanAnomalyDegrees: meanAnomalyDegrees).Position;
	}

	/// <summary>Computes the heliocentric state from Keplerian elements in the reference plane of the elements.</summary>
	/// <param name="semiMajorAxisAu">The semi-major axis [AU].</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <param name="inclinationDegrees">The inclination [°].</param>
	/// <param name="longitudeOfAscendingNodeDegrees">The longitude of the ascending node [°].</param>
	/// <param name="argumentOfPerihelionDegrees">The argument of perihelion [°].</param>
	/// <param name="meanAnomalyDegrees">The mean anomaly [°].</param>
	/// <returns>The position [AU] and velocity [AU/d] in the reference frame of the elements.</returns>
	/// <remarks>This method solves Kepler's equation to compute the eccentric anomaly, then computes the position and velocity in the orbital plane, and finally rotates them into the reference frame of the elements using the inclination, longitude of ascending node, and argument of perihelion.</remarks>
	public static (Vector3d Position, Vector3d Velocity) KeplerState(double semiMajorAxisAu, double eccentricity, double inclinationDegrees, double longitudeOfAscendingNodeDegrees, double argumentOfPerihelionDegrees, double meanAnomalyDegrees)
	{
		// Conversion factor from degrees to radians
		const double d2r = AstronomicalConstants.DegreesToRadians;
		// Convert the mean anomaly from degrees to radians
		double e = eccentricity;
		// Solve Kepler's equation to find the eccentric anomaly [rad] from the mean anomaly [rad] and eccentricity
		double eccentricAnomaly = SolveKepler(meanAnomalyRadians: meanAnomalyDegrees * d2r, eccentricity: e);
		// Compute the sine and cosine of the eccentric anomaly
		(double sinE, double cosE) = Math.SinCos(x: eccentricAnomaly);
		// Compute the square root of (1 - e²) for use in the position and velocity calculations
		double sqrt1me2 = Math.Sqrt(d: 1.0 - (e * e));
		// Compute the position in the x-direction using the eccentric anomaly and the semi-major axis
		double x = semiMajorAxisAu * (cosE - e);
		// Compute the position in the y-direction using the eccentric anomaly and the semi-major axis
		double y = semiMajorAxisAu * sqrt1me2 * sinE;
		// Compute the mean motion [rad/d] from the semi-major axis [AU] using Kepler's third law
		double n = AstronomicalConstants.GaussianGravitationalConstant / (semiMajorAxisAu * Math.Sqrt(d: semiMajorAxisAu));
		// Compute the factor for the velocity components using the mean motion, semi-major axis, and eccentric anomaly
		double factor = n * semiMajorAxisAu / (1.0 - (e * cosE));
		// Compute the velocity in the x-direction using the eccentric anomaly and the semi-major axis
		double vx = -factor * sinE;
		// Compute the velocity in the y-direction using the eccentric anomaly and the semi-major axis
		double vy = factor * sqrt1me2 * cosE;
		// Compute the sine and cosine of the argument of perihelion
		(double sw, double cw) = Math.SinCos(x: argumentOfPerihelionDegrees * d2r);
		// Compute the sine and cosine of the longitude of the ascending node
		(double sn, double cn) = Math.SinCos(x: longitudeOfAscendingNodeDegrees * d2r);
		// Compute the sine and cosine of the inclination angle
		(double si, double ci) = Math.SinCos(x: inclinationDegrees * d2r);
		// Compute the rotation matrix components for the transformation from the orbital plane to the reference frame of the elements
		Vector3d p = new(X: (cw * cn) - (sw * sn * ci), Y: (cw * sn) + (sw * cn * ci), Z: sw * si);
		// Compute the position and velocity in the reference frame of the elements by applying the rotation matrix to the position and velocity in the orbital plane
		Vector3d q = new(X: (-sw * cn) - (cw * sn * ci), Y: (-sw * sn) + (cw * cn * ci), Z: cw * si);
		// Return the position and velocity vectors in the reference frame of the elements
		return ((x * p) + (y * q), (vx * p) + (vy * q));
	}

	/// <summary>Solves Kepler's equation E − e·sin E = M with Newton's method.</summary>
	/// <param name="meanAnomalyRadians">The mean anomaly M [rad].</param>
	/// <param name="eccentricity">The eccentricity (0 ≤ e &lt; 1).</param>
	/// <returns>The eccentric anomaly E [rad].</returns>
	/// <remarks>This method uses a simple Newton iteration with a maximum of 50 iterations and a convergence tolerance of 1e-14. The initial guess for the eccentric anomaly is chosen based on the eccentricity and mean anomaly.</remarks>
	public static double SolveKepler(double meanAnomalyRadians, double eccentricity)
	{
		// Normalize the mean anomaly to the range [-π, π] using the IEEE remainder function
		double m = Math.IEEERemainder(x: meanAnomalyRadians, y: 2.0 * Math.PI);
		// Store the eccentricity in a local variable for use in the iteration
		double e = eccentricity;
		// Choose an initial guess for the eccentric anomaly based on the eccentricity and mean anomaly
		double eccentricAnomaly = e < 0.8 ? m : Math.PI * Math.Sign(value: m == 0.0 ? 1.0 : m);
		// Perform a Newton iteration to solve Kepler's equation, with a maximum of 50 iterations and a convergence tolerance of 1e-14
		for (int i = 0; i < 50; i++)
		{
			// Compute the sine and cosine of the current estimate of the eccentric anomaly
			(double s, double c) = Math.SinCos(x: eccentricAnomaly);
			// Compute the change in the eccentric anomaly using Newton's method
			double delta = (eccentricAnomaly - (e * s) - m) / (1.0 - (e * c));
			// Update the estimate of the eccentric anomaly by subtracting the change
			eccentricAnomaly -= delta;
			// Check for convergence by comparing the absolute value of the change to the tolerance
			if (Math.Abs(value: delta) < 1e-14)
			{
				// If the change is smaller than the tolerance, break out of the loop as we have converged
				break;
			}
		}
		// Return the computed eccentric anomaly in radians
		return eccentricAnomaly;
	}

	/// <summary>Computes the heliocentric acceleration of the minor planet.</summary>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <param name="position">The heliocentric position [AU].</param>
	/// <param name="velocity">The heliocentric velocity [AU/d].</param>
	/// <returns>The acceleration [AU/d²] and the distance to the closest perturbing body [AU].</returns>
	/// <remarks>This method computes the acceleration due to the Sun's gravity (Keplerian term), the post-Newtonian correction of the Sun (Schwarzschild, PPN β = γ = 1), and the direct and indirect gravitational perturbations of Mercury to Neptune (Earth–Moon barycenter) taken from the configured <see cref="IPlanetaryEphemerisProvider"/>. The distance to the closest perturbing body is also returned for use in adaptive step size control.</remarks>
	private (Vector3d Acceleration, double ClosestApproachAu) Acceleration(double julianDateTdb, Vector3d position, Vector3d velocity)
	{
		// Compute the distance from the Sun to the minor planet
		double r = position.Length;
		// Compute the cube of the distance for use in the acceleration calculations
		double r3 = r * r * r;
		// Compute the central (Keplerian) term of the acceleration
		Vector3d acceleration = position * (-AstronomicalConstants.SunGm / r3);
		// Compute the square of the speed of light in AU/d for use in the post-Newtonian correction
		double c2 = AstronomicalConstants.SpeedOfLightAuPerDay * AstronomicalConstants.SpeedOfLightAuPerDay;
		// Compute the dot product of the velocity with itself (v²) for use in the post-Newtonian correction
		double v2 = velocity.Dot(other: velocity);
		// Compute the post-Newtonian correction of the Sun (Schwarzschild, PPN β = γ = 1) and add it to the acceleration
		acceleration += AstronomicalConstants.SunGm / (c2 * r3) * ((((4.0 * AstronomicalConstants.SunGm / r) - v2) * position) + (4.0 * position.Dot(other: velocity) * velocity));
		// Initialize the closest approach distance to a large value
		double closest = double.MaxValue;
		// Loop over each perturbing body (Mercury to Neptune) to compute their contributions to the acceleration
		for (int i = 0; i < PerturbingBodies.Length; i++)
		{
			// Get the heliocentric position of the perturbing body at the given Julian date using the planetary ephemeris provider
			Vector3d planet = PlanetaryEphemeris.GetHeliocentricPosition(body: PerturbingBodies[i], julianDateTdb: julianDateTdb);
			// Compute the vector from the minor planet to the perturbing body
			Vector3d d = planet - position;
			// Compute the lengths of the vectors for use in the acceleration calculations
			double dLength = d.Length;
			// Compute the length of the perturbing body's position vector
			double pLength = planet.Length;
			// Update the closest approach distance if the current perturbing body is closer than the previous closest
			closest = Math.Min(val1: closest, val2: dLength);
			// Compute the acceleration contribution from the perturbing body using the direct and indirect terms and add it to the total acceleration
			acceleration += PerturbingGm[i] * ((d / (dLength * dLength * dLength)) - (planet / (pLength * pLength * pLength)));
		}
		// Return the total acceleration and the distance to the closest perturbing body
		return (acceleration, closest);
	}

	/// <summary>Determines the integration step size.</summary>
	/// <param name="state">The current state.</param>
	/// <param name="closestApproachAu">The distance to the closest perturbing body [AU].</param>
	/// <param name="acceleration">The current acceleration [AU/d²].</param>
	/// <returns>The maximum step size [d].</returns>
	/// <remarks>This method computes the step size based on the local orbital period (r^1.5), the distance to the closest perturbing body, and the magnitude of the acceleration. The step size is clamped to a minimum of 0.001 d and a maximum of 4 d.</remarks>
	private static double ComputeStepSize(StateVector state, double closestApproachAu, Vector3d acceleration)
	{
		// Compute the distance from the Sun to the minor planet
		double r = state.Position.Length;
		// About 1/400 of the local orbital period scale (r^1.5 days at 1 AU ≈ 1 day)

		// Clamp the step size to a minimum of 0.01 d and a maximum of 4 d to avoid excessively small or large steps
		double step = Math.Clamp(value: r * Math.Sqrt(d: r), min: 0.01, max: 4.0);
		// Ensure that the step size is not too large compared to the distance to the closest perturbing body and the current speed of the minor planet
		double speed = Math.Max(val1: state.Velocity.Length, val2: 1e-6);
		// Limit the step size based on the closest approach distance and the speed, with a minimum of 0.001 d
		step = Math.Min(val1: step, val2: Math.Max(val1: 0.001, val2: closestApproachAu / speed / 50.0));
		// Limit the step size based on the magnitude of the acceleration, with a minimum of 0.001 d and a maximum of 0.02 * speed / accelerationLength
		double accelerationLength = acceleration.Length;
		// If the acceleration is non-zero, limit the step size based on the acceleration magnitude
		if (accelerationLength > 0.0)
		{
			step = Math.Min(val1: step, val2: Math.Max(val1: 0.001, val2: 0.02 * speed / accelerationLength));
		}
		// Return the computed step size
		return step;
	}

	/// <summary>Performs a single classical Runge–Kutta step.</summary>
	/// <param name="state">The current state.</param>
	/// <param name="h">The step size [d] (may be negative).</param>
	/// <param name="k1Acceleration">The acceleration at the start of the step.</param>
	/// <returns>The new state.</returns>
	/// <remarks>This method implements the classical fourth-order Runge–Kutta integration scheme, using the provided acceleration at the start of the step and computing the intermediate accelerations at the midpoints and endpoint of the step.</remarks>
	private StateVector RungeKutta4Step(StateVector state, double h, Vector3d k1Acceleration)
	{
		// Store the current Julian date, position, and velocity for use in the Runge–Kutta calculations
		double t = state.JulianDateTdb;
		// Store the current position and velocity vectors
		Vector3d r = state.Position;
		// Store the current velocity vector
		Vector3d v = state.Velocity;
		// Compute the intermediate k1, k2, k3, and k4 vectors for position and velocity using the Runge–Kutta formulas
		Vector3d k1r = v;
		Vector3d k1v = k1Acceleration;
		Vector3d k2r = v + (0.5 * h * k1v);
		Vector3d k2v = Acceleration(julianDateTdb: t + (0.5 * h), position: r + (0.5 * h * k1r), velocity: k2r).Acceleration;
		Vector3d k3r = v + (0.5 * h * k2v);
		Vector3d k3v = Acceleration(julianDateTdb: t + (0.5 * h), position: r + (0.5 * h * k2r), velocity: k3r).Acceleration;
		Vector3d k4r = v + (h * k3v);
		Vector3d k4v = Acceleration(julianDateTdb: t + h, position: r + (h * k3r), velocity: k4r).Acceleration;
		// Compute the new position and velocity vectors using the weighted sum of the intermediate vectors
		Vector3d newPosition = r + (h / 6.0 * (k1r + (2.0 * k2r) + (2.0 * k3r) + k4r));
		Vector3d newVelocity = v + (h / 6.0 * (k1v + (2.0 * k2v) + (2.0 * k3v) + k4v));
		// Return the new state vector with the updated Julian date, position, and velocity
		return new StateVector(JulianDateTdb: t + h, Position: newPosition, Velocity: newVelocity);
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
