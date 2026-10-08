/*
 * File:        TimeScales.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides conversions between UTC, TT, TDB and Julian dates.
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

/// <summary>Provides conversions between UTC, Terrestrial Time (TT), Barycentric Dynamical Time (TDB) and Julian dates.</summary>
/// <remarks>All civil times are handled strictly in UTC. Leap seconds are taken from the IERS Bulletin C table; before 1972 the Espenak–Meeus ΔT polynomials are used.</remarks>
internal static class TimeScales
{
	/// <summary>Julian date of the standard epoch J2000.0 (2000-01-01 12:00 TT).</summary>
	public const double J2000 = 2451545.0;

	/// <summary>Number of days per Julian century.</summary>
	public const double DaysPerJulianCentury = 36525.0;

	/// <summary>Seconds per day.</summary>
	public const double SecondsPerDay = 86400.0;

	/// <summary>The instant 2000-01-01 12:00:00 used as reference for Julian date conversions.</summary>
	private static readonly DateTime J2000DateTime = new(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc);

	/// <summary>Leap second table: UTC date from which the given TAI−UTC [s] applies.</summary>
	private static readonly (DateTime Since, int TaiMinusUtc)[] LeapSeconds =
	[
		(new DateTime(1972, 1, 1, 0, 0, 0, DateTimeKind.Utc), 10),
		(new DateTime(1972, 7, 1, 0, 0, 0, DateTimeKind.Utc), 11),
		(new DateTime(1973, 1, 1, 0, 0, 0, DateTimeKind.Utc), 12),
		(new DateTime(1974, 1, 1, 0, 0, 0, DateTimeKind.Utc), 13),
		(new DateTime(1975, 1, 1, 0, 0, 0, DateTimeKind.Utc), 14),
		(new DateTime(1976, 1, 1, 0, 0, 0, DateTimeKind.Utc), 15),
		(new DateTime(1977, 1, 1, 0, 0, 0, DateTimeKind.Utc), 16),
		(new DateTime(1978, 1, 1, 0, 0, 0, DateTimeKind.Utc), 17),
		(new DateTime(1979, 1, 1, 0, 0, 0, DateTimeKind.Utc), 18),
		(new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc), 19),
		(new DateTime(1981, 7, 1, 0, 0, 0, DateTimeKind.Utc), 20),
		(new DateTime(1982, 7, 1, 0, 0, 0, DateTimeKind.Utc), 21),
		(new DateTime(1983, 7, 1, 0, 0, 0, DateTimeKind.Utc), 22),
		(new DateTime(1985, 7, 1, 0, 0, 0, DateTimeKind.Utc), 23),
		(new DateTime(1988, 1, 1, 0, 0, 0, DateTimeKind.Utc), 24),
		(new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), 25),
		(new DateTime(1991, 1, 1, 0, 0, 0, DateTimeKind.Utc), 26),
		(new DateTime(1992, 7, 1, 0, 0, 0, DateTimeKind.Utc), 27),
		(new DateTime(1993, 7, 1, 0, 0, 0, DateTimeKind.Utc), 28),
		(new DateTime(1994, 7, 1, 0, 0, 0, DateTimeKind.Utc), 29),
		(new DateTime(1996, 1, 1, 0, 0, 0, DateTimeKind.Utc), 30),
		(new DateTime(1997, 7, 1, 0, 0, 0, DateTimeKind.Utc), 31),
		(new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc), 32),
		(new DateTime(2006, 1, 1, 0, 0, 0, DateTimeKind.Utc), 33),
		(new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc), 34),
		(new DateTime(2012, 7, 1, 0, 0, 0, DateTimeKind.Utc), 35),
		(new DateTime(2015, 7, 1, 0, 0, 0, DateTimeKind.Utc), 36),
		(new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc), 37)
	];

	/// <summary>Converts a <see cref="DateTime"/> to a Julian date in the same time scale.</summary>
	/// <param name="dateTime">The date and time. A <see cref="DateTimeKind.Local"/> value is converted to UTC first.</param>
	/// <returns>The Julian date [d].</returns>
	public static double ToJulianDate(DateTime dateTime)
	{
		DateTime utc = dateTime.Kind == DateTimeKind.Local ? dateTime.ToUniversalTime() : DateTime.SpecifyKind(value: dateTime, kind: DateTimeKind.Utc);
		return J2000 + ((utc - J2000DateTime).Ticks / (double)TimeSpan.TicksPerDay);
	}

	/// <summary>Converts a <see cref="DateTimeOffset"/> to a Julian date in UTC.</summary>
	/// <param name="time">The instant (any offset; it is converted to UTC).</param>
	/// <returns>The Julian date (UTC) [d].</returns>
	public static double ToJulianDateUtc(DateTimeOffset time) => ToJulianDate(dateTime: time.UtcDateTime);

	/// <summary>Converts a Julian date into a UTC <see cref="DateTimeOffset"/>.</summary>
	/// <param name="julianDate">The Julian date [d].</param>
	/// <returns>The instant with offset +00:00.</returns>
	public static DateTimeOffset FromJulianDate(double julianDate)
	{
		long ticks = (long)Math.Round(a: (julianDate - J2000) * TimeSpan.TicksPerDay);
		return new DateTimeOffset(dateTime: J2000DateTime.AddTicks(value: ticks), offset: TimeSpan.Zero);
	}

	/// <summary>Gets TT − UTC in seconds for the given UTC instant.</summary>
	/// <param name="utc">The UTC instant.</param>
	/// <returns>TT − UTC [s] (equal to ΔT before 1972, where UTC ≈ UT).</returns>
	public static double TtMinusUtcSeconds(DateTime utc)
	{
		if (utc < LeapSeconds[0].Since)
		{
			return DeltaTBefore1972(decimalYear: utc.Year + ((utc.DayOfYear - 0.5) / 365.25));
		}
		int taiMinusUtc = LeapSeconds[0].TaiMinusUtc;
		foreach ((DateTime since, int value) in LeapSeconds)
		{
			if (utc >= since)
			{
				taiMinusUtc = value;
			}
		}
		return taiMinusUtc + 32.184;
	}

	/// <summary>Converts a UTC instant to a Julian date in Terrestrial Time.</summary>
	/// <param name="time">The instant.</param>
	/// <returns>The Julian date (TT) [d].</returns>
	public static double ToJulianDateTt(DateTimeOffset time) => ToJulianDateUtc(time: time) + (TtMinusUtcSeconds(utc: time.UtcDateTime) / SecondsPerDay);

	/// <summary>Converts a Julian date in TT to TDB.</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>The Julian date (TDB) [d].</returns>
	/// <remarks>Uses the two leading periodic terms (amplitude 1.657 ms); the error is below 30 µs.</remarks>
	public static double TtToTdb(double julianDateTt)
	{
		double g = (357.53 + (0.98560028 * (julianDateTt - J2000))) * Math.PI / 180.0;
		return julianDateTt + (((0.001657 * Math.Sin(a: g)) + (0.000014 * Math.Sin(a: 2.0 * g))) / SecondsPerDay);
	}

	/// <summary>Computes ΔT before 1972 using the Espenak–Meeus polynomials.</summary>
	/// <param name="decimalYear">The decimal year.</param>
	/// <returns>ΔT [s].</returns>
	private static double DeltaTBefore1972(double decimalYear)
	{
		double y = decimalYear;
		if (y >= 1961)
		{
			double t = y - 1975;
			return 45.45 + (1.067 * t) - (t * t / 260) - (t * t * t / 718);
		}
		if (y >= 1941)
		{
			double t = y - 1950;
			return 29.07 + (0.407 * t) - (t * t / 233) + (t * t * t / 2547);
		}
		if (y >= 1920)
		{
			double t = y - 1920;
			return 21.20 + (0.84493 * t) - (0.076100 * t * t) + (0.0020936 * t * t * t);
		}
		if (y >= 1900)
		{
			double t = y - 1900;
			return -2.79 + (1.494119 * t) - (0.0598939 * t * t) + (0.0061966 * t * t * t) - (0.000197 * t * t * t * t);
		}
		double u = (y - 1820) / 100;
		return -20 + (32 * u * u);
	}
}
