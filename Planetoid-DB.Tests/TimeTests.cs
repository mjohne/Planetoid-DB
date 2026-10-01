/*
 * File:        TimeTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests time scales, date changes, time zones and daylight saving time.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Services;

namespace Planetoid_DB.Tests;

/// <summary>Tests time scales, date changes, time zones and daylight saving time.</summary>
public sealed class TimeTests
{
	/// <summary>Gets the Central European time zone.</summary>
	private static TimeZoneInfo Berlin => TimeZoneInfo.FindSystemTimeZoneById(id: "Europe/Berlin");

	/// <summary>J2000.0 corresponds to JD 2451545.0.</summary>
	[Fact]
	public void ToJulianDate_J2000()
		=> Assert.Equal(expected: 2451545.0, actual: AstronomicalTime.ToJulianDate(utc: TestData.Utc(text: "2000-01-01T12:00:00Z")), precision: 9);

	/// <summary>The Julian date is continuous across a change of date and year.</summary>
	[Fact]
	public void ToJulianDate_DateChange_IsContinuous()
	{
		double before = AstronomicalTime.ToJulianDate(utc: TestData.Utc(text: "2024-12-31T23:59:59Z"));
		double after = AstronomicalTime.ToJulianDate(utc: TestData.Utc(text: "2025-01-01T00:00:00Z"));
		Assert.Equal(expected: 1.0 / 86400.0, actual: after - before, precision: 9);
		Assert.Equal(expected: 2460676.5, actual: after, precision: 9);
	}

	/// <summary>A local time with an offset gives the same Julian date as the equivalent UTC time.</summary>
	[Fact]
	public void ToJulianDate_OffsetIsIgnored()
	{
		DateTimeOffset local = new(year: 2025, month: 1, day: 1, hour: 1, minute: 0, second: 0, offset: TimeSpan.FromHours(value: 2));
		Assert.Equal(expected: AstronomicalTime.ToJulianDate(utc: TestData.Utc(text: "2024-12-31T23:00:00Z")), actual: AstronomicalTime.ToJulianDate(utc: local), precision: 12);
	}

	/// <summary>TT − UTC is 69.184 s since 2017.</summary>
	[Fact]
	public void TerrestrialTimeMinusUtc_Since2017()
		=> Assert.Equal(expected: 69.184, actual: AstronomicalTime.GetTerrestrialTimeMinusUtcSeconds(time: TestData.Utc(text: "2025-06-01T00:00:00Z")), precision: 3);

	/// <summary>Summer and winter times in Berlin are converted with the correct offset.</summary>
	[Fact]
	public void ConvertToUtc_SummerAndWinterTime()
	{
		Assert.Equal(expected: TestData.Utc(text: "2025-07-01T10:00:00Z"), actual: EphemerisRequest.ConvertToUtc(localTime: new DateTime(year: 2025, month: 7, day: 1, hour: 12, minute: 0, second: 0), timeZone: Berlin));
		Assert.Equal(expected: TestData.Utc(text: "2025-01-15T11:00:00Z"), actual: EphemerisRequest.ConvertToUtc(localTime: new DateTime(year: 2025, month: 1, day: 15, hour: 12, minute: 0, second: 0), timeZone: Berlin));
	}

	/// <summary>Times skipped by the spring transition are rejected.</summary>
	[Fact]
	public void ConvertToUtc_SkippedTime_Throws()
		=> Assert.Throws<ArgumentException>(testCode: () => EphemerisRequest.ConvertToUtc(localTime: new DateTime(year: 2025, month: 3, day: 30, hour: 2, minute: 30, second: 0), timeZone: Berlin));

	/// <summary>Ambiguous times of the autumn transition are interpreted as standard time.</summary>
	[Fact]
	public void ConvertToUtc_AmbiguousTime_UsesStandardTime()
		=> Assert.Equal(expected: TestData.Utc(text: "2025-10-26T01:30:00Z"), actual: EphemerisRequest.ConvertToUtc(localTime: new DateTime(year: 2025, month: 10, day: 26, hour: 2, minute: 30, second: 0), timeZone: Berlin));

	/// <summary>The kind of the input time is ignored; it is always interpreted in the given zone.</summary>
	[Fact]
	public void ConvertToUtc_IgnoresKind()
	{
		DateTime utcKind = new(year: 2025, month: 7, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc);
		Assert.Equal(expected: TestData.Utc(text: "2025-07-01T10:00:00Z"), actual: EphemerisRequest.ConvertToUtc(localTime: utcKind, timeZone: Berlin));
	}

	/// <summary>A time series across the spring transition is uniform in UTC.</summary>
	[Fact]
	public void CreateTimeSeries_AcrossDaylightSavingTime_IsUniformInUtc()
	{
		DateTimeOffset start = new(year: 2025, month: 3, day: 30, hour: 0, minute: 0, second: 0, offset: TimeSpan.FromHours(value: 1));
		DateTimeOffset end = new(year: 2025, month: 3, day: 30, hour: 4, minute: 0, second: 0, offset: TimeSpan.FromHours(value: 2));
		IReadOnlyList<DateTimeOffset> times = EphemerisRequest.CreateTimeSeries(start: start, end: end, step: TimeSpan.FromHours(value: 1));
		Assert.Equal(expected: 4, actual: times.Count);
		Assert.All(collection: times, action: static t => Assert.Equal(expected: TimeSpan.Zero, actual: t.Offset));
		for (int i = 1; i < times.Count; i++)
		{
			Assert.Equal(expected: TimeSpan.FromHours(value: 1), actual: times[i] - times[i - 1]);
		}
	}

	/// <summary>A time series crossing midnight contains both dates.</summary>
	[Fact]
	public void CreateTimeSeries_AcrossMidnight_ChangesDate()
	{
		IReadOnlyList<DateTimeOffset> times = EphemerisRequest.CreateTimeSeries(start: TestData.Utc(text: "2025-12-31T23:00:00Z"), end: TestData.Utc(text: "2026-01-01T01:00:00Z"), step: TimeSpan.FromMinutes(value: 30));
		Assert.Equal(expected: 5, actual: times.Count);
		Assert.Equal(expected: 2025, actual: times[0].Year);
		Assert.Equal(expected: 2026, actual: times[^1].Year);
		Assert.Equal(expected: 1, actual: times[2].Day);
	}

	/// <summary>Invalid series parameters are rejected.</summary>
	[Fact]
	public void CreateTimeSeries_InvalidArguments_Throw()
	{
		DateTimeOffset t = TestData.Utc(text: "2025-01-01T00:00:00Z");
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisRequest.CreateTimeSeries(start: t, end: t.AddDays(days: 1), step: TimeSpan.Zero));
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisRequest.CreateTimeSeries(start: t, end: t.AddDays(days: -1), step: TimeSpan.FromHours(value: 1)));
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisRequest.CreateTimeSeries(start: t, end: t.AddYears(years: 100), step: TimeSpan.FromMinutes(value: 1)));
	}
}
