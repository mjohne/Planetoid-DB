/*
 * File:        EphemerisService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Calculates topocentric ephemerides of minor planets.
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

/// <summary>Calculates topocentric ephemerides (RA/Dec, azimuth/altitude, brightness, visibility) of minor planets.</summary>
/// <remarks>
/// <para>Pipeline per time point: UTC → TT/TDB, numerical propagation of the MPCORB osculating elements (Sun + planetary perturbations
/// + relativistic correction), light-time correction, topocentric parallax (WGS84), annual and diurnal aberration,
/// precession (IAU 1976) and nutation (IAU 1980) to the true equator of date, horizontal coordinates and optional refraction.</para>
/// <para>All input and output times are UTC. UT1 is approximated by UTC (|UT1 − UTC| &lt; 0.9 s).</para>
/// </remarks>
/// <param name="planetaryEphemeris">The planetary ephemeris (JPL DE440/DE441 or analytical fallback).</param>
internal sealed class EphemerisService(IPlanetaryEphemerisProvider planetaryEphemeris)
{
	/// <summary>The maximum number of time points of a single calculation.</summary>
	public const int MaximumTimePoints = 100_000;

	/// <summary>The rotation rate of the Earth [rad/d].</summary>
	private const double EarthRotationRadiansPerDay = 2.0 * Math.PI * 1.00273781191135448;

	/// <summary>The orbit propagator.</summary>
	private readonly OrbitPropagationService propagator = new(planetaryEphemeris: planetaryEphemeris);

	/// <summary>Gets the planetary ephemeris used by this service.</summary>
	public IPlanetaryEphemerisProvider PlanetaryEphemeris { get; } = planetaryEphemeris;

	/// <summary>Creates an equidistant time grid in UTC, including the start and (if reached exactly) the end.</summary>
	/// <param name="start">The start instant (any offset; converted to UTC).</param>
	/// <param name="end">The end instant (any offset; converted to UTC).</param>
	/// <param name="step">The step size (must be positive).</param>
	/// <returns>The time points with offset +00:00.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the step is not positive, the end precedes the start or too many points would result.</exception>
	public static IReadOnlyList<DateTimeOffset> CreateTimeGrid(DateTimeOffset start, DateTimeOffset end, TimeSpan step)
	{
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value: step, other: TimeSpan.Zero);
		DateTimeOffset startUtc = start.ToUniversalTime();
		DateTimeOffset endUtc = end.ToUniversalTime();
		ArgumentOutOfRangeException.ThrowIfLessThan(value: endUtc, other: startUtc);
		long count = ((endUtc - startUtc).Ticks / step.Ticks) + 1;
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: count, other: MaximumTimePoints);
		DateTimeOffset[] times = new DateTimeOffset[count];
		for (int i = 0; i < count; i++)
		{
			times[i] = startUtc + TimeSpan.FromTicks(value: step.Ticks * i);
		}
		return times;
	}

	/// <summary>Converts a wall-clock time in a time zone into UTC, handling daylight-saving transitions.</summary>
	/// <param name="localTime">The wall-clock time (its <see cref="DateTime.Kind"/> is ignored).</param>
	/// <param name="timeZone">The time zone of the wall-clock time.</param>
	/// <returns>The UTC instant (offset +00:00).</returns>
	/// <remarks>
	/// A non-existent time (spring-forward gap) is shifted forward by the length of the gap;
	/// an ambiguous time (fall-back overlap) is interpreted as standard time (the later instant).
	/// </remarks>
	public static DateTimeOffset LocalToUtc(DateTime localTime, TimeZoneInfo timeZone)
	{
		ArgumentNullException.ThrowIfNull(argument: timeZone);
		DateTime unspecified = DateTime.SpecifyKind(value: localTime, kind: DateTimeKind.Unspecified);
		if (timeZone.IsInvalidTime(dateTime: unspecified))
		{
			// Use the offset valid before the gap: e.g. 02:30 at a 02:00→03:00 transition becomes 03:30 local time
			TimeSpan offsetBefore = timeZone.GetUtcOffset(dateTime: unspecified.AddHours(value: -12));
			return new DateTimeOffset(dateTime: DateTime.SpecifyKind(value: unspecified - offsetBefore, kind: DateTimeKind.Utc), offset: TimeSpan.Zero);
		}
		if (timeZone.IsAmbiguousTime(dateTime: unspecified))
		{
			TimeSpan standardOffset = timeZone.GetAmbiguousTimeOffsets(dateTime: unspecified).Min();
			return new DateTimeOffset(dateTime: DateTime.SpecifyKind(value: unspecified - standardOffset, kind: DateTimeKind.Utc), offset: TimeSpan.Zero);
		}
		return new DateTimeOffset(dateTime: TimeZoneInfo.ConvertTimeToUtc(dateTime: unspecified, sourceTimeZone: timeZone), offset: TimeSpan.Zero);
	}

	/// <summary>Calculates the ephemeris asynchronously on a background thread.</summary>
	/// <param name="elements">The orbital elements of the minor planet.</param>
	/// <param name="times">The time points (any offset; results are returned in UTC, in the given order).</param>
	/// <param name="observer">The observer location.</param>
	/// <param name="criteria">The visibility criteria; <c>null</c> for <see cref="VisibilityCriteria.Default"/>.</param>
	/// <param name="options">The calculation options; <c>null</c> for <see cref="EphemerisOptions.Default"/>.</param>
	/// <param name="progress">An optional progress receiver (0–100 %).</param>
	/// <param name="cancellationToken">A token to cancel the calculation.</param>
	/// <returns>The ephemeris entries.</returns>
	public Task<IReadOnlyList<EphemerisEntry>> CalculateAsync(
		MinorPlanetOrbitalElements elements,
		IReadOnlyList<DateTimeOffset> times,
		ObserverLocation observer,
		VisibilityCriteria? criteria = null,
		EphemerisOptions? options = null,
		IProgress<int>? progress = null,
		CancellationToken cancellationToken = default) =>
		Task.Run(function: () => Calculate(elements: elements, times: times, observer: observer, criteria: criteria, options: options, progress: progress, cancellationToken: cancellationToken), cancellationToken: cancellationToken);

	/// <summary>Calculates the ephemeris synchronously.</summary>
	/// <param name="elements">The orbital elements of the minor planet.</param>
	/// <param name="times">The time points (any offset; results are returned in UTC, in the given order).</param>
	/// <param name="observer">The observer location.</param>
	/// <param name="criteria">The visibility criteria; <c>null</c> for <see cref="VisibilityCriteria.Default"/>.</param>
	/// <param name="options">The calculation options; <c>null</c> for <see cref="EphemerisOptions.Default"/>.</param>
	/// <param name="progress">An optional progress receiver (0–100 %).</param>
	/// <param name="cancellationToken">A token to cancel the calculation.</param>
	/// <returns>The ephemeris entries.</returns>
	/// <exception cref="ArgumentException">Thrown when the elements or the observer are invalid.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when a time point is outside the planetary ephemeris or too many points are requested.</exception>
	/// <exception cref="OperationCanceledException">Thrown when the calculation is canceled.</exception>
	public IReadOnlyList<EphemerisEntry> Calculate(
		MinorPlanetOrbitalElements elements,
		IReadOnlyList<DateTimeOffset> times,
		ObserverLocation observer,
		VisibilityCriteria? criteria = null,
		EphemerisOptions? options = null,
		IProgress<int>? progress = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(argument: elements);
		ArgumentNullException.ThrowIfNull(argument: times);
		ArgumentNullException.ThrowIfNull(argument: observer);
		elements.Validate();
		observer.Validate();
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: times.Count, other: MaximumTimePoints);
		criteria ??= VisibilityCriteria.Default;
		options ??= EphemerisOptions.Default;
		double epochTdb = TimeScales.TtToTdb(julianDateTt: elements.EpochJulianDateTt);
		double[] julianDatesTdb = new double[times.Count];
		for (int i = 0; i < times.Count; i++)
		{
			DateTimeOffset time = times[i];
			double jd = TimeScales.ToJulianDateUtc(time: time);
			if (jd < PlanetaryEphemeris.StartJulianDate + 1.0 || jd > PlanetaryEphemeris.EndJulianDate - 1.0)
			{
				throw new ArgumentOutOfRangeException(paramName: nameof(times), message: $"The time {time.UtcDateTime:O} is outside the range of the planetary ephemeris '{PlanetaryEphemeris.Name}'.");
			}
			julianDatesTdb[i] = TimeScales.TtToTdb(julianDateTt: TimeScales.ToJulianDateTt(time: time));
		}
		// Integrate each direction outward from the epoch to reuse the integration between neighboring points
		int[] order = [.. Enumerable.Range(start: 0, count: times.Count).OrderBy(keySelector: i => Math.Abs(value: julianDatesTdb[i] - epochTdb))];
		EphemerisEntry[] result = new EphemerisEntry[times.Count];
		StateVector epochState = OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: epochTdb);
		StateVector? forward = null;
		StateVector? backward = null;
		int lastPercent = -1;
		for (int k = 0; k < order.Length; k++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			int index = order[k];
			DateTimeOffset utc = times[index].ToUniversalTime();
			double jdTdb = julianDatesTdb[index];
			StateVector state;
			if (!options.IncludePlanetaryPerturbations)
			{
				state = OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: jdTdb);
			}
			else if (jdTdb >= epochTdb)
			{
				forward = propagator.Propagate(state: forward ?? epochState, targetJulianDateTdb: jdTdb, cancellationToken: cancellationToken);
				state = forward.Value;
			}
			else
			{
				// Earlier points are visited nearest to the epoch first and propagated progressively farther backward
				backward = propagator.Propagate(state: backward ?? epochState, targetJulianDateTdb: jdTdb, cancellationToken: cancellationToken);
				state = backward.Value;
			}
			result[index] = CalculateEntry(elements: elements, state: state, utc: utc, observer: observer, criteria: criteria, options: options);
			int percent = (int)((k + 1) * 100L / order.Length);
			if (percent != lastPercent)
			{
				lastPercent = percent;
				progress?.Report(value: percent);
			}
		}
		return result;
	}

	/// <summary>Calculates a single ephemeris entry from the heliocentric state of the object at the observation time.</summary>
	/// <param name="elements">The orbital elements (used for H and G and for two-body light-time correction).</param>
	/// <param name="state">The geometric heliocentric state of the object at the observation time (TDB), ICRF.</param>
	/// <param name="utc">The observation time (UTC).</param>
	/// <param name="observer">The observer location.</param>
	/// <param name="criteria">The visibility criteria.</param>
	/// <param name="options">The calculation options.</param>
	/// <returns>The ephemeris entry.</returns>
	private EphemerisEntry CalculateEntry(MinorPlanetOrbitalElements elements, StateVector state, DateTimeOffset utc, ObserverLocation observer, VisibilityCriteria criteria, EphemerisOptions options)
	{
		double jdUtc = TimeScales.ToJulianDateUtc(time: utc);
		double jdTt = TimeScales.ToJulianDateTt(time: utc);
		double jdTdb = state.JulianDateTdb;

		// Earth and observer
		Vector3d earth = PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb);
		const double dt = 0.01;
		Vector3d earthVelocity = (PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb + dt)
			- PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb - dt)) / (2.0 * dt);
		Matrix3d trueOfDate = CoordinateTransformationService.TrueOfDateMatrix(julianDateTt: jdTt);
		Matrix3d toIcrf = trueOfDate.Transpose();
		double last = CoordinateTransformationService.NormalizeDegrees(degrees: CoordinateTransformationService.GreenwichApparentSiderealTimeDegrees(julianDateUt1: jdUtc, julianDateTt: jdTt) + observer.LongitudeDegrees);
		Vector3d observerTod = CoordinateTransformationService.ObserverPositionTrueOfDate(observer: observer, localApparentSiderealTimeDegrees: last);
		Vector3d observerGeocentric = toIcrf * observerTod;
		Vector3d observerRotationVelocity = toIcrf * new Vector3d(X: -EarthRotationRadiansPerDay * observerTod.Y, Y: EarthRotationRadiansPerDay * observerTod.X, Z: 0.0);
		Vector3d observerHeliocentric = earth + observerGeocentric;
		Vector3d observerVelocity = earthVelocity + observerRotationVelocity;

		// Light-time iteration: position of the object at the time of emission
		Vector3d objectAtEmission = state.Position;
		Vector3d topocentric = objectAtEmission - observerHeliocentric;
		for (int i = 0; i < 3; i++)
		{
			double tau = topocentric.Length / AstronomicalConstants.SpeedOfLightAuPerDay;
			objectAtEmission = options.IncludePlanetaryPerturbations
				? Retard(state: state, tau: tau)
				: OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: jdTdb - tau).Position;
			topocentric = objectAtEmission - observerHeliocentric;
		}

		// Apparent place of the object
		Vector3d apparent = CoordinateTransformationService.ApplyAberration(direction: topocentric, observerVelocity: observerVelocity);
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: trueOfDate * apparent);
		(double azimuth, double altitude) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: last - (ra * 15.0), declinationDegrees: dec, latitudeDegrees: observer.LatitudeDegrees);
		if (options.ApplyRefraction)
		{
			altitude += CoordinateTransformationService.RefractionDegrees(trueAltitudeDegrees: altitude);
		}

		// Sun
		Vector3d sunApparent = CoordinateTransformationService.ApplyAberration(direction: -observerHeliocentric, observerVelocity: observerVelocity);
		(double sunRa, double sunDec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: trueOfDate * sunApparent);
		double sunAltitude = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: last - (sunRa * 15.0), declinationDegrees: sunDec, latitudeDegrees: observer.LatitudeDegrees).AltitudeDegrees;
		if (options.ApplyRefraction)
		{
			sunAltitude += CoordinateTransformationService.RefractionDegrees(trueAltitudeDegrees: sunAltitude);
		}

		// Moon
		Vector3d moonTopocentric = PlanetaryEphemeris.GetGeocentricMoonPosition(julianDateTdb: jdTdb) - observerGeocentric;
		double moonSeparation = moonTopocentric.AngleToDegrees(other: topocentric);

		// Photometry
		double heliocentricDistance = objectAtEmission.Length;
		double distance = topocentric.Length;
		double phaseAngle = VisibilityCalculator.PhaseAngleDegrees(heliocentricDistanceAu: heliocentricDistance, observerDistanceAu: distance, sunObserverDistanceAu: observerHeliocentric.Length);
		double elongation = (-observerHeliocentric).AngleToDegrees(other: topocentric);
		double magnitude = VisibilityCalculator.ApparentMagnitude(absoluteMagnitude: elements.AbsoluteMagnitude, slopeParameter: elements.SlopeParameter, heliocentricDistanceAu: heliocentricDistance, observerDistanceAu: distance, phaseAngleDegrees: phaseAngle);
		bool visible = VisibilityCalculator.IsVisible(altitudeDegrees: altitude, sunAltitudeDegrees: sunAltitude, apparentMagnitude: magnitude, moonSeparationDegrees: moonSeparation, criteria: criteria);

		return new EphemerisEntry(
			Time: utc,
			RightAscensionHours: ra,
			DeclinationDegrees: dec,
			AzimuthDegrees: azimuth,
			AltitudeDegrees: altitude,
			DistanceAu: distance,
			ApparentMagnitude: magnitude,
			IsVisible: visible,
			HeliocentricDistanceAu: heliocentricDistance,
			SunAltitudeDegrees: sunAltitude,
			MoonSeparationDegrees: moonSeparation,
			PhaseAngleDegrees: phaseAngle,
			ElongationDegrees: elongation);
	}

	/// <summary>Computes the position of the object a short time <paramref name="tau"/> before the state epoch (second-order Taylor series).</summary>
	/// <param name="state">The state at the observation time.</param>
	/// <param name="tau">The light time [d].</param>
	/// <returns>The heliocentric position at <c>t − τ</c> [AU].</returns>
	private static Vector3d Retard(StateVector state, double tau)
	{
		double r = state.Position.Length;
		Vector3d acceleration = state.Position * (-AstronomicalConstants.SunGm / (r * r * r));
		return state.Position - (state.Velocity * tau) + (acceleration * (0.5 * tau * tau));
	}
}
