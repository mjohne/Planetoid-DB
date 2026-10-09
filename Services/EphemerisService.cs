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

using System.Diagnostics;

namespace Planetoid_DB.Services;

/// <summary>Calculates topocentric ephemerides (RA/Dec, azimuth/altitude, brightness, visibility) of minor planets.</summary>
/// <remarks>
/// <para>Pipeline per time point: UTC → TT/TDB, numerical propagation of the MPCORB osculating elements (Sun + planetary perturbations
/// + relativistic correction), light-time correction, topocentric parallax (WGS84), annual and diurnal aberration,
/// precession (IAU 1976) and nutation (IAU 1980) to the true equator of date, horizontal coordinates and optional refraction.</para>
/// <para>Both the apparent place (true equator and equinox of date) and the astrometric place (ICRF/J2000, as used by the
/// MPC ephemeris service) are returned. Their difference is dominated by precession (≈ 50″ per year since J2000).</para>
/// <para>All input and output times are UTC. UT1 is approximated by UTC (|UT1 − UTC| &lt; 0.9 s).</para>
/// </remarks>
/// <param name="planetaryEphemeris">The planetary ephemeris (JPL DE440/DE441 or analytical fallback).</param>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class EphemerisService(IPlanetaryEphemerisProvider planetaryEphemeris)
{
	/// <summary>The maximum number of time points of a single calculation.</summary>
	/// <remarks>This constant defines the upper limit for the number of time points that can be processed in a single ephemeris calculation to prevent excessive memory usage and ensure performance.</remarks>
	public const int MaximumTimePoints = 100_000;

	/// <summary>The rotation rate of the Earth [rad/d].</summary>
	/// <remarks>This constant represents the angular velocity of the Earth's rotation in radians per day, accounting for the sidereal day length.</remarks>
	private const double EarthRotationRadiansPerDay = 2.0 * Math.PI * 1.00273781191135448;

	/// <summary>The orbit propagator.</summary>
	/// <remarks>This instance of <see cref="OrbitPropagationService"/> is used to numerically propagate the orbital elements of minor planets, including planetary perturbations and relativistic corrections.</remarks>
	private readonly OrbitPropagationService propagator = new(planetaryEphemeris: planetaryEphemeris);

	/// <summary>Gets the planetary ephemeris used by this service.</summary>
	/// <remarks>This property exposes the planetary ephemeris provider used for calculating the positions of major solar system bodies, which is essential for accurate ephemeris calculations of minor planets.</remarks>
	public IPlanetaryEphemerisProvider PlanetaryEphemeris { get; } = planetaryEphemeris;

	/// <summary>Creates an equidistant time grid in UTC, including the start and (if reached exactly) the end.</summary>
	/// <param name="start">The start instant (any offset; converted to UTC).</param>
	/// <param name="end">The end instant (any offset; converted to UTC).</param>
	/// <param name="step">The step size (must be positive).</param>
	/// <returns>The time points with offset +00:00.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the step is not positive, the end precedes the start or too many points would result.</exception>
	/// <remarks>This method generates a list of equidistant time points between the specified start and end instants, ensuring that the step size is positive and that the total number of points does not exceed the defined maximum limit.</remarks>
	public static IReadOnlyList<DateTimeOffset> CreateTimeGrid(DateTimeOffset start, DateTimeOffset end, TimeSpan step)
	{
		// Validate the step size to ensure it is positive
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value: step, other: TimeSpan.Zero);
		// Convert start and end times to UTC for consistent comparison
		DateTimeOffset startUtc = start.ToUniversalTime();
		// Convert end time to UTC for consistent comparison
		DateTimeOffset endUtc = end.ToUniversalTime();
		// Validate that the end time is not earlier than the start time
		ArgumentOutOfRangeException.ThrowIfLessThan(value: endUtc, other: startUtc);
		// Calculate the total number of time points based on the step size
		long count = ((endUtc - startUtc).Ticks / step.Ticks) + 1;
		// Validate that the total number of time points does not exceed the maximum allowed
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: count, other: MaximumTimePoints);
		// Create an array to hold the calculated time points
		DateTimeOffset[] times = new DateTimeOffset[count];
		// Populate the array with equidistant time points starting from the start time
		for (int i = 0; i < count; i++)
		{
			// Calculate each time point by adding the step size multiplied by the index to the start time
			times[i] = startUtc + TimeSpan.FromTicks(value: step.Ticks * i);
		}
		// Return the array of time points as a read-only list
		return times;
	}

	/// <summary>Converts a wall-clock time in a time zone into UTC, handling daylight-saving transitions.</summary>
	/// <param name="localTime">The wall-clock time (its <see cref="DateTime.Kind"/> is ignored).</param>
	/// <param name="timeZone">The time zone of the wall-clock time.</param>
	/// <returns>The UTC instant (offset +00:00).</returns>
	/// <remarks>A non-existent time (spring-forward gap) is shifted forward by the length of the gap;
	/// an ambiguous time (fall-back overlap) is interpreted as standard time (the later instant).</remarks>
	public static DateTimeOffset LocalToUtc(DateTime localTime, TimeZoneInfo timeZone)
	{
		// Validate that the time zone is not null to prevent null reference exceptions
		ArgumentNullException.ThrowIfNull(argument: timeZone);
		// Create a DateTime with Unspecified kind to avoid automatic conversion by the system
		DateTime unspecified = DateTime.SpecifyKind(value: localTime, kind: DateTimeKind.Unspecified);
		// Check if the specified local time is invalid in the given time zone (e.g., during a daylight saving time transition)
		if (timeZone.IsInvalidTime(dateTime: unspecified))
		{
			// Get the UTC offset for a time 12 hours before the specified time to ensure we are in a valid period
			// Use the offset valid before the gap: e.g. 02:30 at a 02:00→03:00 transition becomes 03:30 local time
			TimeSpan offsetBefore = timeZone.GetUtcOffset(dateTime: unspecified.AddHours(value: -12));
			// Return the adjusted time in UTC by subtracting the offset before the gap
			return new DateTimeOffset(dateTime: DateTime.SpecifyKind(value: unspecified - offsetBefore, kind: DateTimeKind.Utc), offset: TimeSpan.Zero);
		}
		// Check if the specified local time is ambiguous in the given time zone (e.g., during a daylight saving time transition)
		return timeZone.IsAmbiguousTime(dateTime: unspecified)
			? new DateTimeOffset(dateTime: DateTime.SpecifyKind(value: TimeZoneInfo.ConvertTimeToUtc(dateTime: unspecified, sourceTimeZone: timeZone), kind: DateTimeKind.Utc), offset: TimeSpan.Zero)
			: new DateTimeOffset(dateTime: TimeZoneInfo.ConvertTimeToUtc(dateTime: unspecified, sourceTimeZone: timeZone), offset: TimeSpan.Zero);
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
	/// <remarks>This method runs the ephemeris calculation on a background thread, allowing the calling thread to remain responsive. It returns a task that completes with the calculated ephemeris entries.</remarks>
	public Task<IReadOnlyList<EphemerisEntry>> CalculateAsync(
		MinorPlanetOrbitalElements elements,
		IReadOnlyList<DateTimeOffset> times,
		ObserverLocation observer,
		VisibilityCriteria? criteria = null,
		EphemerisOptions? options = null,
		IProgress<int>? progress = null,
		CancellationToken cancellationToken = default)
	{
		// Run the calculation on a background thread using Task.Run, passing the cancellation token to allow for cancellation
		return Task.Run(function: () => Calculate(elements: elements, times: times, observer: observer, criteria: criteria, options: options, progress: progress, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
	}

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
	/// <remarks>This method performs the ephemeris calculation synchronously, returning the calculated entries directly. It validates the input parameters and throws exceptions for invalid inputs or if the calculation is canceled.</remarks>
	public IReadOnlyList<EphemerisEntry> Calculate(
		MinorPlanetOrbitalElements elements,
		IReadOnlyList<DateTimeOffset> times,
		ObserverLocation observer,
		VisibilityCriteria? criteria = null,
		EphemerisOptions? options = null,
		IProgress<int>? progress = null,
		CancellationToken cancellationToken = default)
	{
		// Validate that the orbital elements, time points, and observer location are not null to prevent null reference exceptions
		ArgumentNullException.ThrowIfNull(argument: elements);
		ArgumentNullException.ThrowIfNull(argument: times);
		ArgumentNullException.ThrowIfNull(argument: observer);
		// Validate the orbital elements and observer location to ensure they are within acceptable ranges and formats
		elements.Validate();
		observer.Validate();
		// Validate that the number of time points does not exceed the maximum allowed to prevent excessive memory usage
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: times.Count, other: MaximumTimePoints);
		// Use default visibility criteria and calculation options if they are not provided
		criteria ??= VisibilityCriteria.Default;
		options ??= EphemerisOptions.Default;
		// Convert the epoch of the orbital elements from TT to TDB for accurate propagation
		double epochTdb = TimeScales.TtToTdb(julianDateTt: elements.EpochJulianDateTt);
		// Convert the list of time points to an array of Julian Dates in TDB, validating that each time point is within the range of the planetary ephemeris
		double[] julianDatesTdb = new double[times.Count];
		// Loop through each time point to convert it to Julian Date in TDB and check if it is within the valid range of the planetary ephemeris
		for (int i = 0; i < times.Count; i++)
		{
			// Convert the current time point to UTC and then to Julian Date in TDB
			DateTimeOffset time = times[index: i];
			// Convert the time to Julian Date in UTC for range checking
			double jd = TimeScales.ToJulianDateUtc(time: time);
			// Check if the Julian Date is within the valid range of the planetary ephemeris, throwing an exception if it is not
			if (jd < PlanetaryEphemeris.StartJulianDate + 1.0 || jd > PlanetaryEphemeris.EndJulianDate - 1.0)
			{
				// Throw an exception indicating that the time point is outside the range of the planetary ephemeris, including the specific time and ephemeris name in the message
				throw new ArgumentOutOfRangeException(paramName: nameof(times), message: $"The time {time.UtcDateTime:O} is outside the range of the planetary ephemeris '{PlanetaryEphemeris.Name}'.");
			}
			// Convert the time to Julian Date in TDB for use in the ephemeris calculation
			julianDatesTdb[i] = TimeScales.TtToTdb(julianDateTt: TimeScales.ToJulianDateTt(time: time));
		}
		// Create an array of indices representing the order in which to process the time points, sorted by their absolute difference from the epoch in TDB
		// Integrate each direction outward from the epoch to reuse the integration between neighboring points
		int[] order = [.. Enumerable.Range(start: 0, count: times.Count).OrderBy(keySelector: i => Math.Abs(value: julianDatesTdb[i] - epochTdb))];
		// Create an array to hold the resulting ephemeris entries, initialized with the same length as the number of time points
		EphemerisEntry[] result = new EphemerisEntry[times.Count];
		// Get the state vector of the minor planet at the epoch of the orbital elements, which serves as the starting point for propagation
		StateVector epochState = OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: epochTdb);
		// Initialize nullable state vectors for forward and backward propagation, which will be used to store the last computed state in each direction
		StateVector? forward = null;
		// Initialize a variable to track the last reported progress percentage, starting with -1 to ensure the first report is sent
		StateVector? backward = null;
		// Loop through the ordered indices to calculate the ephemeris entries for each time point, propagating the state as needed
		int lastPercent = -1;
		for (int k = 0; k < order.Length; k++)
		{
			// Check for cancellation requests to allow the operation to be canceled gracefully
			cancellationToken.ThrowIfCancellationRequested();
			// Get the index of the current time point in the original times array
			int index = order[k];
			// Convert the current time point to UTC for use in the ephemeris calculation
			DateTimeOffset utc = times[index].ToUniversalTime();
			// Get the Julian Date in TDB for the current time point, which will be used for state propagation and ephemeris calculations
			double jdTdb = julianDatesTdb[index];
			// Determine the state vector of the minor planet at the current time point, either by using two-body propagation or by propagating from the last known state
			StateVector state;
			// If planetary perturbations are not included, calculate the state using two-body propagation directly from the orbital elements
			if (!options.IncludePlanetaryPerturbations)
			{
				// Calculate the state vector using two-body propagation based on the orbital elements and the current Julian Date in TDB
				state = OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: jdTdb);
			}
			// If the current time point is after the epoch, propagate forward from the last known forward state or the epoch state
			else if (jdTdb >= epochTdb)
			{
				// Later points are visited nearest to the epoch first and propagated progressively farther forward
				forward = propagator.Propagate(state: forward ?? epochState, targetJulianDateTdb: jdTdb, cancellationToken: cancellationToken);
				// Update the state vector to the newly propagated forward state
				state = forward.Value;
			}
			// If the current time point is before the epoch, propagate backward from the last known backward state or the epoch state
			else
			{
				// Propagate the state backward from the last known backward state or the epoch state to the current Julian Date in TDB
				// Earlier points are visited nearest to the epoch first and propagated progressively farther backward
				backward = propagator.Propagate(state: backward ?? epochState, targetJulianDateTdb: jdTdb, cancellationToken: cancellationToken);
				// Update the state vector to the newly propagated backward state
				state = backward.Value;
			}
			// Calculate the ephemeris entry for the current time point using the determined state vector and other parameters, and store it in the result array at the corresponding index
			result[index] = CalculateEntry(elements: elements, state: state, utc: utc, observer: observer, criteria: criteria, options: options);
			// Calculate the progress percentage based on the number of completed calculations and report it if it has changed since the last report
			int percent = (int)((k + 1) * 100L / order.Length);
			// Report progress only if the percentage has changed to avoid excessive reporting
			if (percent != lastPercent)
			{
				// Update the last reported percentage to the current percentage
				lastPercent = percent;
				progress?.Report(value: percent);
			}
		}
		// Return the array of calculated ephemeris entries as a read-only list
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
	/// <remarks>This method computes the topocentric ephemeris entry for a minor planet at a specific observation time, taking into account light-time correction, parallax, aberration, precession, nutation, and visibility criteria. It returns an <see cref="EphemerisEntry"/> containing the calculated values.</remarks>
	private EphemerisEntry CalculateEntry(MinorPlanetOrbitalElements elements, StateVector state, DateTimeOffset utc, ObserverLocation observer, VisibilityCriteria criteria, EphemerisOptions options)
	{
		// Convert the observation time to Julian Dates in UTC, TT, and TDB for use in calculations
		double jdUtc = TimeScales.ToJulianDateUtc(time: utc);
		double jdTt = TimeScales.ToJulianDateTt(time: utc);
		double jdTdb = state.JulianDateTdb;
		// Get the heliocentric position of the Earth at the observation time in TDB, which is used to calculate the observer's position and velocity
		Vector3d earth = PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb);
		// Calculate the heliocentric velocity of the Earth using a central difference approximation with a small time step, which is used for aberration correction
		const double dt = 0.01;
		// Use a central difference approximation to calculate the Earth's heliocentric velocity by evaluating the position at slightly earlier and later times
		Vector3d earthVelocity = (PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb + dt)
			- PlanetaryEphemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb - dt)) / (2.0 * dt);
		Matrix3d trueOfDate = CoordinateTransformationService.TrueOfDateMatrix(julianDateTt: jdTt);
		// The transpose of the true-of-date matrix is used to transform coordinates from the true equator and equinox of date to the ICRF/J2000 frame
		Matrix3d toIcrf = trueOfDate.Transpose();
		// Calculate the local apparent sidereal time at the observer's longitude, which is used to determine the observer's position in the true equator and equinox of date frame
		double last = CoordinateTransformationService.NormalizeDegrees(degrees: CoordinateTransformationService.GreenwichApparentSiderealTimeDegrees(julianDateUt1: jdUtc, julianDateTt: jdTt) + observer.LongitudeDegrees);
		// Calculate the observer's position in the true equator and equinox of date frame using the local apparent sidereal time and the observer's latitude and altitude
		Vector3d observerTod = CoordinateTransformationService.ObserverPositionTrueOfDate(observer: observer, localApparentSiderealTimeDegrees: last);
		// Transform the observer's position to the ICRF/J2000 frame and calculate the observer's rotation velocity due to the Earth's rotation
		Vector3d observerGeocentric = toIcrf * observerTod;
		// The observer's rotation velocity is calculated based on the Earth's rotation rate and the observer's position in the true equator and equinox of date frame
		Vector3d observerRotationVelocity = toIcrf * new Vector3d(X: -EarthRotationRadiansPerDay * observerTod.Y, Y: EarthRotationRadiansPerDay * observerTod.X, Z: 0.0);
		// The observer's heliocentric position and velocity are calculated by adding the Earth's heliocentric position and velocity to the observer's geocentric position and rotation velocity
		Vector3d observerHeliocentric = earth + observerGeocentric;
		// The observer's heliocentric velocity is calculated by adding the Earth's heliocentric velocity and the observer's rotation velocity
		Vector3d observerVelocity = earthVelocity + observerRotationVelocity;
		// The light-time correction is applied iteratively to account for the time it takes for light to travel from the object to the observer, adjusting the object's position accordingly
		Vector3d objectAtEmission = state.Position;
		// The topocentric vector from the observer to the object at the time of emission is calculated by subtracting the observer's heliocentric position from the object's position
		Vector3d topocentric = objectAtEmission - observerHeliocentric;
		// Iterate three times to refine the light-time correction, updating the object's position based on the calculated light travel time
		for (int i = 0; i < 3; i++)
		{
			// Calculate the light travel time in days based on the distance from the observer to the object and the speed of light in AU per day
			double tau = topocentric.Length / AstronomicalConstants.SpeedOfLightAuPerDay;
			// Calculate the object's position at the time of emission, either by applying the retardation method (if planetary perturbations are included) or by using two-body propagation (if not)
			objectAtEmission = options.IncludePlanetaryPerturbations
				? Retard(state: state, tau: tau)
				: OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: jdTdb - tau).Position;
			// Update the topocentric vector based on the object's position at the time of emission
			topocentric = objectAtEmission - observerHeliocentric;
		}
		// Convert the topocentric vector to right ascension and declination
		// Astrometric place (ICRF/J2000, as published by the MPC ephemeris service)
		(double astrometricRa, double astrometricDec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: topocentric);
		// Apply annual and diurnal aberration to the topocentric vector using the observer's velocity, then convert to right ascension and declination in the true equator and equinox of date frame
		Vector3d apparent = CoordinateTransformationService.ApplyAberration(direction: topocentric, observerVelocity: observerVelocity);
		// Convert the apparent vector to right ascension and declination
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: trueOfDate * apparent);
		// Convert the apparent right ascension and declination to horizontal coordinates (azimuth and altitude) for the observer's location
		(double azimuth, double altitude) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: last - (ra * 15.0), declinationDegrees: dec, latitudeDegrees: observer.LatitudeDegrees);
		// Store the geometric altitude (without refraction) for visibility calculations
		double geometricAltitude = altitude;
		// Apply atmospheric refraction correction to the altitude if requested in the options
		if (options.ApplyRefraction)
		{
			// Add the refraction correction to the altitude based on the true altitude in degrees
			altitude += CoordinateTransformationService.RefractionDegrees(trueAltitudeDegrees: altitude);
		}
		// Calculate the apparent position of the Sun for visibility calculations, using the observer's heliocentric position and velocity
		Vector3d sunApparent = CoordinateTransformationService.ApplyAberration(direction: -observerHeliocentric, observerVelocity: observerVelocity);
		// Convert the apparent Sun vector to right ascension and declination in the true equator and equinox of date frame
		(double sunRa, double sunDec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: trueOfDate * sunApparent);
		// Convert the Sun's right ascension and declination to horizontal coordinates (azimuth and altitude) for the observer's location
		double sunAltitude = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: last - (sunRa * 15.0), declinationDegrees: sunDec, latitudeDegrees: observer.LatitudeDegrees).AltitudeDegrees;
		// Apply atmospheric refraction correction to the Sun's altitude if requested in the options
		if (options.ApplyRefraction)
		{
			// Add the refraction correction to the Sun's altitude based on the true altitude in degrees
			sunAltitude += CoordinateTransformationService.RefractionDegrees(trueAltitudeDegrees: sunAltitude);
		}
		// Calculate the topocentric position of the Moon for visibility calculations, using the observer's geocentric position
		Vector3d moonTopocentric = PlanetaryEphemeris.GetGeocentricMoonPosition(julianDateTdb: jdTdb) - observerGeocentric;
		// Calculate the angular separation between the Moon and the object in degrees for visibility calculations
		double moonSeparation = moonTopocentric.AngleToDegrees(other: topocentric);
		// Calculate the heliocentric distance of the object at the time of emission, the topocentric distance from the observer, and the phase angle for brightness calculations
		double heliocentricDistance = objectAtEmission.Length;
		// Calculate the topocentric distance from the observer to the object
		double distance = topocentric.Length;
		// Calculate the phase angle in degrees using the heliocentric distance, observer distance, and Sun-observer distance
		double phaseAngle = VisibilityCalculator.PhaseAngleDegrees(heliocentricDistanceAu: heliocentricDistance, observerDistanceAu: distance, sunObserverDistanceAu: observerHeliocentric.Length);
		// Calculate the elongation in degrees, which is the angle between the Sun and the object as seen from the observer
		double elongation = (-observerHeliocentric).AngleToDegrees(other: topocentric);
		// Calculate the apparent magnitude of the object using its absolute magnitude, slope parameter, heliocentric distance, observer distance, and phase angle
		double magnitude = VisibilityCalculator.ApparentMagnitude(absoluteMagnitude: elements.AbsoluteMagnitude, slopeParameter: elements.SlopeParameter, heliocentricDistanceAu: heliocentricDistance, observerDistanceAu: distance, phaseAngleDegrees: phaseAngle);
		// Determine if the object is visible based on its geometric altitude, Sun altitude, apparent magnitude, Moon separation, and the specified visibility criteria
		bool visible = geometricAltitude > 0.0 && VisibilityCalculator.IsVisible(altitudeDegrees: altitude, sunAltitudeDegrees: sunAltitude, apparentMagnitude: magnitude, moonSeparationDegrees: moonSeparation, criteria: criteria);
		// Create and return a new EphemerisEntry with the calculated values for the observation time
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
			ElongationDegrees: elongation,
			GeometricAltitudeDegrees: geometricAltitude,
			AstrometricRightAscensionHours: astrometricRa,
			AstrometricDeclinationDegrees: astrometricDec);
	}

	/// <summary>Computes the position of the object a short time <paramref name="tau"/> before the state epoch (second-order Taylor series).</summary>
	/// <param name="state">The state at the observation time.</param>
	/// <param name="tau">The light time [d].</param>
	/// <returns>The heliocentric position at <c>t − τ</c> [AU].</returns>
	/// <remarks>This method calculates the position of a celestial object at a time slightly before the given state epoch, accounting for light travel time. It uses a second-order Taylor series expansion to estimate the position based on the current state vector and the specified light time.</remarks>
	private static Vector3d Retard(StateVector state, double tau)
	{
		// Calculate the position of the object a short time before the state epoch using a second-order Taylor series expansion
		double r = state.Position.Length;
		// Calculate the acceleration of the object due to the Sun's gravity
		Vector3d acceleration = state.Position * (-AstronomicalConstants.SunGm / (r * r * r));
		// Return the position of the object a short time before the state epoch using a second-order Taylor series expansion
		return state.Position - (state.Velocity * tau) + (acceleration * (0.5 * tau * tau));
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
