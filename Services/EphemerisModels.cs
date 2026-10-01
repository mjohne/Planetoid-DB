/*
 * File:        EphemerisModels.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Defines the input and output models of the ephemeris calculation.
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

/// <summary>Represents the calculated position of a minor planet at a specific time.</summary>
/// <param name="Time">The instant of the observation in UTC.</param>
/// <param name="RightAscensionHours">The topocentric astrometric right ascension in hours (ICRF/J2000.0, range [0, 24)).</param>
/// <param name="DeclinationDegrees">The topocentric astrometric declination in degrees (ICRF/J2000.0, range [−90, +90]).</param>
/// <param name="AzimuthDegrees">The azimuth in degrees, measured from north (0°) through east (90°), range [0, 360).</param>
/// <param name="AltitudeDegrees">The apparent altitude above the horizon in degrees (refraction applied if enabled).</param>
/// <param name="DistanceAu">The topocentric distance Δ in AU.</param>
/// <param name="ApparentMagnitude">The apparent visual magnitude V in mag (H-G system), or <see cref="double.NaN"/> if H is unknown.</param>
/// <param name="IsVisible">Whether all visibility criteria are fulfilled.</param>
/// <param name="HeliocentricDistanceAu">The heliocentric distance r in AU.</param>
/// <param name="PhaseAngleDegrees">The phase angle (Sun–object–observer) in degrees.</param>
/// <param name="SolarElongationDegrees">The solar elongation (Sun–observer–object) in degrees.</param>
/// <param name="SunAltitudeDegrees">The apparent altitude of the Sun in degrees.</param>
/// <param name="MoonSeparationDegrees">The angular distance between the object and the Moon in degrees.</param>
internal sealed record EphemerisEntry(
	DateTimeOffset Time,
	double RightAscensionHours,
	double DeclinationDegrees,
	double AzimuthDegrees,
	double AltitudeDegrees,
	double DistanceAu,
	double ApparentMagnitude,
	bool IsVisible,
	double HeliocentricDistanceAu = double.NaN,
	double PhaseAngleDegrees = double.NaN,
	double SolarElongationDegrees = double.NaN,
	double SunAltitudeDegrees = double.NaN,
	double MoonSeparationDegrees = double.NaN);

/// <summary>Represents a geographic observing site on the WGS84 ellipsoid.</summary>
/// <param name="LatitudeDegrees">The geodetic latitude in degrees (north positive, range [−90, +90]).</param>
/// <param name="LongitudeDegrees">The geodetic longitude in degrees (east positive, range [−180, +360)).</param>
/// <param name="HeightMeters">The height above the WGS84 ellipsoid in metres.</param>
internal sealed record ObserverLocation(double LatitudeDegrees, double LongitudeDegrees, double HeightMeters = 0.0)
{
	/// <summary>Validates the observer location.</summary>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when a coordinate is outside its valid range.</exception>
	public void Validate()
	{
		if (!double.IsFinite(d: LatitudeDegrees) || LatitudeDegrees is < -90.0 or > 90.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LatitudeDegrees), actualValue: LatitudeDegrees, message: "The latitude must be between -90° and +90°.");
		}
		if (!double.IsFinite(d: LongitudeDegrees) || LongitudeDegrees is < -180.0 or >= 360.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LongitudeDegrees), actualValue: LongitudeDegrees, message: "The longitude must be between -180° and +360°.");
		}
		if (!double.IsFinite(d: HeightMeters) || HeightMeters is < -12000.0 or > 100000.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(HeightMeters), actualValue: HeightMeters, message: "The height must be between -12000 m and 100000 m.");
		}
	}
}

/// <summary>Represents the criteria used to decide whether a minor planet is observable.</summary>
/// <param name="MinimumObjectAltitudeDegrees">The minimum altitude of the object above the horizon in degrees.</param>
/// <param name="MaximumSunAltitudeDegrees">The highest permitted altitude of the Sun in degrees (e.g. −12° for nautical twilight).</param>
/// <param name="LimitingMagnitude">The faintest apparent magnitude that can still be observed (mag).</param>
/// <param name="MinimumMoonSeparationDegrees">The minimum angular distance to the Moon in degrees.</param>
internal sealed record VisibilityCriteria(
	double MinimumObjectAltitudeDegrees = 0.0,
	double MaximumSunAltitudeDegrees = -12.0,
	double LimitingMagnitude = 99.0,
	double MinimumMoonSeparationDegrees = 0.0);

/// <summary>Defines the orbit propagation model.</summary>
internal enum PropagationModel
{
	/// <summary>Unperturbed Keplerian motion around the Sun.</summary>
	TwoBody,
	/// <summary>Numerical integration including planetary, lunar and relativistic perturbations.</summary>
	Perturbed
}

/// <summary>Represents the parameters of an ephemeris calculation.</summary>
/// <param name="Elements">The orbital elements of the minor planet.</param>
/// <param name="Times">The instants for which positions are calculated; they are converted to UTC.</param>
/// <param name="Observer">The observing site.</param>
/// <param name="Criteria">The visibility criteria.</param>
/// <param name="Model">The orbit propagation model.</param>
/// <param name="ApplyRefraction">Whether atmospheric refraction is applied to the altitudes.</param>
internal sealed record EphemerisRequest(
	MinorPlanetElements Elements,
	IReadOnlyList<DateTimeOffset> Times,
	ObserverLocation Observer,
	VisibilityCriteria Criteria,
	PropagationModel Model = PropagationModel.Perturbed,
	bool ApplyRefraction = true)
{
	/// <summary>The maximum number of instants allowed in one request.</summary>
	public const int MaximumNumberOfTimes = 1_000_000;

	/// <summary>Converts a wall-clock time of a time zone to UTC, taking daylight saving time into account.</summary>
	/// <param name="localTime">The wall-clock time (its <see cref="DateTime.Kind"/> is ignored).</param>
	/// <param name="timeZone">The time zone in which <paramref name="localTime"/> is given.</param>
	/// <returns>The corresponding UTC instant. Ambiguous times (when clocks are turned back) are interpreted as standard time.</returns>
	/// <exception cref="ArgumentException">Thrown when the time does not exist in the time zone (skipped by a daylight saving time transition).</exception>
	public static DateTimeOffset ConvertToUtc(DateTime localTime, TimeZoneInfo timeZone)
	{
		ArgumentNullException.ThrowIfNull(argument: timeZone);
		DateTime unspecified = DateTime.SpecifyKind(value: localTime, kind: DateTimeKind.Unspecified);
		if (timeZone.IsInvalidTime(dateTime: unspecified))
		{
			throw new ArgumentException(message: $"The time {unspecified:yyyy-MM-dd HH:mm} does not exist in the time zone '{timeZone.Id}' (daylight saving time transition).", paramName: nameof(localTime));
		}
		return new DateTimeOffset(dateTime: TimeZoneInfo.ConvertTimeToUtc(dateTime: unspecified, sourceTimeZone: timeZone), offset: TimeSpan.Zero);
	}

	/// <summary>Creates an equidistant UTC time series between two instants (inclusive).</summary>
	/// <param name="start">The first instant (any offset; converted to UTC).</param>
	/// <param name="end">The last instant (any offset; converted to UTC).</param>
	/// <param name="step">The positive step width.</param>
	/// <returns>The UTC instants.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the step is not positive, the end lies before the start, or too many instants would result.</exception>
	/// <remarks>Because the series is generated in UTC, it is unaffected by daylight saving time transitions of the input offsets.</remarks>
	public static IReadOnlyList<DateTimeOffset> CreateTimeSeries(DateTimeOffset start, DateTimeOffset end, TimeSpan step)
	{
		if (step <= TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(step), actualValue: step, message: "The step must be positive.");
		}
		DateTimeOffset startUtc = start.ToUniversalTime();
		DateTimeOffset endUtc = end.ToUniversalTime();
		if (endUtc < startUtc)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(end), actualValue: end, message: "The end must not be before the start.");
		}
		long count = ((endUtc - startUtc).Ticks / step.Ticks) + 1;
		if (count > MaximumNumberOfTimes)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(step), actualValue: step, message: $"The time range would produce more than {MaximumNumberOfTimes} entries.");
		}
		List<DateTimeOffset> times = new(capacity: (int)count);
		for (long i = 0; i < count; i++)
		{
			times.Add(item: startUtc + TimeSpan.FromTicks(value: step.Ticks * i));
		}
		return times;
	}
}
