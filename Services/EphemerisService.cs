/*
 * File:        EphemerisService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Calculates topocentric ephemerides of minor planets including visibility information.
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

/// <summary>Calculates topocentric ephemerides of minor planets including visibility information.</summary>
/// <remarks>
/// Pipeline per instant (all times UTC on input): UTC → TT → TDB; orbit propagation (two-body or perturbed);
/// light-time correction; topocentric astrometric RA/Dec (ICRF/J2000); annual aberration, precession and nutation
/// for the apparent place of date; horizontal coordinates with optional refraction; Sun and Moon positions;
/// H-G magnitude and visibility criteria.
/// </remarks>
internal sealed class EphemerisService
{
	/// <summary>Half width of the central difference used for the Earth velocity in days.</summary>
	private const double VelocityHalfStepDays = 0.005;

	/// <summary>Number of light-time iterations.</summary>
	private const int LightTimeIterations = 3;

	/// <summary>The planetary ephemeris used for Sun, Earth, Moon and perturbers.</summary>
	private readonly IPlanetaryEphemeris ephemeris;

	/// <summary>The orbit propagation service.</summary>
	private readonly OrbitPropagationService propagationService;

	/// <summary>Initializes a new instance of the <see cref="EphemerisService"/> class.</summary>
	/// <param name="ephemeris">The planetary ephemeris (e.g. JPL DE440/DE441); <c>null</c> selects the analytical ephemeris.</param>
	public EphemerisService(IPlanetaryEphemeris? ephemeris = null)
	{
		this.ephemeris = ephemeris ?? new AnalyticalPlanetaryEphemeris();
		propagationService = new OrbitPropagationService(ephemeris: this.ephemeris);
	}

	/// <summary>Gets the name of the planetary ephemeris in use.</summary>
	public string EphemerisName => ephemeris.Name;

	/// <summary>Calculates the ephemeris asynchronously on a background thread.</summary>
	/// <param name="request">The request.</param>
	/// <param name="progress">An optional progress receiver (percent, 0–100).</param>
	/// <param name="cancellationToken">A token to cancel the calculation.</param>
	/// <returns>The entries, ordered by time.</returns>
	public Task<IReadOnlyList<EphemerisEntry>> CalculateAsync(EphemerisRequest request, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(argument: request);
		return Task.Run(function: () => Calculate(request: request, progress: progress, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
	}

	/// <summary>Calculates the ephemeris synchronously.</summary>
	/// <param name="request">The request.</param>
	/// <param name="progress">An optional progress receiver (percent, 0–100).</param>
	/// <param name="cancellationToken">A token to cancel the calculation.</param>
	/// <returns>The entries, ordered by time.</returns>
	/// <exception cref="ArgumentException">Thrown when the request is invalid.</exception>
	public IReadOnlyList<EphemerisEntry> Calculate(EphemerisRequest request, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(argument: request);
		ArgumentNullException.ThrowIfNull(argument: request.Elements);
		ArgumentNullException.ThrowIfNull(argument: request.Times);
		ArgumentNullException.ThrowIfNull(argument: request.Criteria);
		request.Observer.Validate();
		if (request.Times.Count > EphemerisRequest.MaximumNumberOfTimes)
		{
			throw new ArgumentException(message: $"At most {EphemerisRequest.MaximumNumberOfTimes} instants are allowed.", paramName: nameof(request));
		}
		DateTimeOffset[] times = [.. request.Times.Select(selector: static t => t.ToUniversalTime()).Order()];
		OrbitPropagationService.OrbitPropagationSession session = propagationService.CreateSession(elements: request.Elements, model: request.Model);
		List<EphemerisEntry> entries = new(capacity: times.Length);
		int lastPercent = -1;
		for (int i = 0; i < times.Length; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			entries.Add(item: CalculateEntry(time: times[i], request: request, session: session, cancellationToken: cancellationToken));
			int percent = (int)((i + 1) * 100L / times.Length);
			if (percent != lastPercent)
			{
				lastPercent = percent;
				progress?.Report(value: percent);
			}
		}
		return entries;
	}

	/// <summary>Calculates a single ephemeris entry.</summary>
	/// <param name="time">The UTC instant.</param>
	/// <param name="request">The request.</param>
	/// <param name="session">The propagation session.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The entry.</returns>
	private EphemerisEntry CalculateEntry(DateTimeOffset time, EphemerisRequest request, OrbitPropagationService.OrbitPropagationSession session, CancellationToken cancellationToken)
	{
		double jdUtc = AstronomicalTime.ToJulianDate(utc: time);
		double jdTt = AstronomicalTime.ToJulianDateTerrestrialTime(time: time);
		double jdTdb = AstronomicalTime.TerrestrialTimeToBarycentricDynamicalTime(julianDateTt: jdTt);

		Matrix3D precessionNutation = CoordinateTransformationService.GetPrecessionNutationMatrix(julianDateTt: jdTt);
		double gast = CoordinateTransformationService.GetGreenwichApparentSiderealTimeRadians(julianDateUt: jdUtc, julianDateTt: jdTt);
		double last = gast + (request.Observer.LongitudeDegrees * AstronomicalConstants.DegreesToRadians);

		// Heliocentric observer position and velocity (ICRF/J2000, AU, AU/day)
		Vector3D observerGeocentric = precessionNutation.Transpose() * CoordinateTransformationService.GetObserverPositionTrueOfDate(observer: request.Observer, greenwichApparentSiderealTimeRadians: gast);
		Vector3D earth = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb);
		Vector3D earthVelocity = (ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb + VelocityHalfStepDays)
			- ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jdTdb - VelocityHalfStepDays)) / (2.0 * VelocityHalfStepDays);
		Vector3D observer = earth + observerGeocentric;

		// Minor planet with light-time correction (Sun-centred Keplerian back-step around the propagated state)
		StateVector state = session.PropagateTo(julianDateTdb: jdTdb, cancellationToken: cancellationToken);
		Vector3D emitted = state.Position;
		for (int iteration = 0; iteration < LightTimeIterations; iteration++)
		{
			double tau = (emitted - observer).Length / AstronomicalConstants.SpeedOfLightAuPerDay;
			emitted = OrbitPropagationService.PropagateTwoBody(state: state, julianDateTdb: jdTdb - tau).Position;
		}
		Vector3D topocentric = emitted - observer;
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: topocentric);
		double altitude = GetApparentAltitude(direction: topocentric, observerVelocity: earthVelocity, precessionNutation: precessionNutation, last: last, request: request, azimuth: out double azimuth);

		// Sun (light-time of ~8 min is irrelevant for the altitude limit)
		Vector3D sunTopocentric = -observer;
		double sunAltitude = GetApparentAltitude(direction: sunTopocentric, observerVelocity: earthVelocity, precessionNutation: precessionNutation, last: last, request: request, azimuth: out _);

		// Moon
		Vector3D moonTopocentric = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Moon, julianDateTdb: jdTdb) - observer;
		double moonSeparation = topocentric.AngleTo(other: moonTopocentric) * AstronomicalConstants.RadiansToDegrees;

		double distance = topocentric.Length;
		double heliocentricDistance = emitted.Length;
		double phaseAngle = VisibilityCalculator.CalculatePhaseAngleDegrees(heliocentricObjectPosition: emitted, observerToObject: topocentric);
		double elongation = sunTopocentric.AngleTo(other: topocentric) * AstronomicalConstants.RadiansToDegrees;
		double magnitude = VisibilityCalculator.CalculateApparentMagnitude(
			absoluteMagnitude: request.Elements.AbsoluteMagnitude,
			slopeParameter: request.Elements.SlopeParameter,
			heliocentricDistanceAu: heliocentricDistance,
			observerDistanceAu: distance,
			phaseAngleDegrees: phaseAngle);
		bool visible = VisibilityCalculator.IsVisible(
			objectAltitudeDegrees: altitude,
			sunAltitudeDegrees: sunAltitude,
			apparentMagnitude: magnitude,
			moonSeparationDegrees: moonSeparation,
			criteria: request.Criteria);

		return new EphemerisEntry(
			Time: time,
			RightAscensionHours: ra,
			DeclinationDegrees: dec,
			AzimuthDegrees: azimuth,
			AltitudeDegrees: altitude,
			DistanceAu: distance,
			ApparentMagnitude: magnitude,
			IsVisible: visible,
			HeliocentricDistanceAu: heliocentricDistance,
			PhaseAngleDegrees: phaseAngle,
			SolarElongationDegrees: elongation,
			SunAltitudeDegrees: sunAltitude,
			MoonSeparationDegrees: moonSeparation);
	}

	/// <summary>Calculates the apparent horizontal coordinates of a topocentric direction.</summary>
	/// <param name="direction">The topocentric geometric direction (ICRF/J2000).</param>
	/// <param name="observerVelocity">The heliocentric velocity of the observer in AU/day.</param>
	/// <param name="precessionNutation">The precession–nutation matrix (J2000 → true of date).</param>
	/// <param name="last">The local apparent sidereal time in radians.</param>
	/// <param name="request">The request (observer and refraction flag).</param>
	/// <param name="azimuth">The azimuth in degrees (north = 0°, east = 90°).</param>
	/// <returns>The altitude in degrees (refracted if enabled).</returns>
	private static double GetApparentAltitude(Vector3D direction, Vector3D observerVelocity, Matrix3D precessionNutation, double last, EphemerisRequest request, out double azimuth)
	{
		Vector3D apparent = precessionNutation * CoordinateTransformationService.ApplyAberration(direction: direction, observerVelocityAuPerDay: observerVelocity);
		(double raOfDate, double decOfDate) = CoordinateTransformationService.ToRightAscensionDeclination(vector: apparent);
		(azimuth, double altitude) = CoordinateTransformationService.ToHorizontal(rightAscensionHours: raOfDate, declinationDegrees: decOfDate, localApparentSiderealTimeRadians: last, latitudeDegrees: request.Observer.LatitudeDegrees);
		return request.ApplyRefraction ? CoordinateTransformationService.ApplyRefraction(geometricAltitudeDegrees: altitude) : altitude;
	}
}
