/*
 * File:        AstronomicalTime.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Provides conversions between UTC, Julian dates and the dynamical time scales TT and TDB.
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

/// <summary>Provides conversions between UTC, Julian dates and the dynamical time scales TT and TDB.</summary>
/// <remarks>All civil times are converted to UTC first. Since 1972 TT − UTC is derived from the leap second table (TAI − UTC + 32.184 s); earlier dates use the ΔT polynomials of Espenak &amp; Meeus.</remarks>
internal static class AstronomicalTime
{
	/// <summary>Julian date of 0001-01-01 00:00 (proleptic Gregorian calendar), the origin of <see cref="DateTime.Ticks"/>.</summary>
	private const double JulianDateOfDateTimeOrigin = 1721425.5;

	/// <summary>Difference TT − TAI in seconds.</summary>
	private const double TerrestrialTimeMinusTaiSeconds = 32.184;

	/// <summary>Leap second table: UTC date from which the value TAI − UTC (in seconds) applies.</summary>
	private static readonly (DateTime Since, double TaiMinusUtc)[] LeapSeconds =
	[
		(new DateTime(1972, 1, 1, 0, 0, 0, DateTimeKind.Utc), 10), (new DateTime(1972, 7, 1, 0, 0, 0, DateTimeKind.Utc), 11),
		(new DateTime(1973, 1, 1, 0, 0, 0, DateTimeKind.Utc), 12), (new DateTime(1974, 1, 1, 0, 0, 0, DateTimeKind.Utc), 13),
		(new DateTime(1975, 1, 1, 0, 0, 0, DateTimeKind.Utc), 14), (new DateTime(1976, 1, 1, 0, 0, 0, DateTimeKind.Utc), 15),
		(new DateTime(1977, 1, 1, 0, 0, 0, DateTimeKind.Utc), 16), (new DateTime(1978, 1, 1, 0, 0, 0, DateTimeKind.Utc), 17),
		(new DateTime(1979, 1, 1, 0, 0, 0, DateTimeKind.Utc), 18), (new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc), 19),
		(new DateTime(1981, 7, 1, 0, 0, 0, DateTimeKind.Utc), 20), (new DateTime(1982, 7, 1, 0, 0, 0, DateTimeKind.Utc), 21),
		(new DateTime(1983, 7, 1, 0, 0, 0, DateTimeKind.Utc), 22), (new DateTime(1985, 7, 1, 0, 0, 0, DateTimeKind.Utc), 23),
		(new DateTime(1988, 1, 1, 0, 0, 0, DateTimeKind.Utc), 24), (new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), 25),
		(new DateTime(1991, 1, 1, 0, 0, 0, DateTimeKind.Utc), 26), (new DateTime(1992, 7, 1, 0, 0, 0, DateTimeKind.Utc), 27),
		(new DateTime(1993, 7, 1, 0, 0, 0, DateTimeKind.Utc), 28), (new DateTime(1994, 7, 1, 0, 0, 0, DateTimeKind.Utc), 29),
		(new DateTime(1996, 1, 1, 0, 0, 0, DateTimeKind.Utc), 30), (new DateTime(1997, 7, 1, 0, 0, 0, DateTimeKind.Utc), 31),
		(new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc), 32), (new DateTime(2006, 1, 1, 0, 0, 0, DateTimeKind.Utc), 33),
		(new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc), 34), (new DateTime(2012, 7, 1, 0, 0, 0, DateTimeKind.Utc), 35),
		(new DateTime(2015, 7, 1, 0, 0, 0, DateTimeKind.Utc), 36), (new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc), 37)
	];

	/// <summary>Converts a UTC date and time to a Julian date (UTC-based).</summary>
	/// <param name="utc">The date and time; it is converted to UTC if it carries another offset.</param>
	/// <returns>The Julian date in days.</returns>
	public static double ToJulianDate(DateTimeOffset utc) => JulianDateOfDateTimeOrigin + (utc.UtcTicks / (double)TimeSpan.TicksPerDay);

	/// <summary>Converts a Julian date to a UTC <see cref="DateTimeOffset"/>.</summary>
	/// <param name="julianDate">The Julian date in days.</param>
	/// <returns>The corresponding date and time with offset zero.</returns>
	public static DateTimeOffset FromJulianDate(double julianDate) => new(ticks: (long)Math.Round(a: (julianDate - JulianDateOfDateTimeOrigin) * TimeSpan.TicksPerDay), offset: TimeSpan.Zero);

	/// <summary>Gets TT − UTC (for dates before 1972: ΔT = TT − UT) in seconds.</summary>
	/// <param name="time">The date and time; it is converted to UTC.</param>
	/// <returns>The difference in seconds.</returns>
	public static double GetTerrestrialTimeMinusUtcSeconds(DateTimeOffset time)
	{
		DateTime utc = time.UtcDateTime;
		if (utc >= LeapSeconds[0].Since)
		{
			double taiMinusUtc = LeapSeconds[0].TaiMinusUtc;
			foreach ((DateTime since, double value) in LeapSeconds)
			{
				if (utc < since)
				{
					break;
				}
				taiMinusUtc = value;
			}
			return taiMinusUtc + TerrestrialTimeMinusTaiSeconds;
		}
		return GetDeltaTBefore1972Seconds(year: utc.Year + ((utc.DayOfYear - 0.5) / 365.25));
	}

	/// <summary>Converts a UTC date and time to the Julian date in Terrestrial Time (TT).</summary>
	/// <param name="time">The date and time; it is converted to UTC.</param>
	/// <returns>The Julian date (TT).</returns>
	public static double ToJulianDateTerrestrialTime(DateTimeOffset time) => ToJulianDate(utc: time) + (GetTerrestrialTimeMinusUtcSeconds(time: time) / AstronomicalConstants.SecondsPerDay);

	/// <summary>Converts a Julian date in TT to Barycentric Dynamical Time (TDB).</summary>
	/// <param name="julianDateTt">The Julian date (TT).</param>
	/// <returns>The Julian date (TDB).</returns>
	/// <remarks>Uses the two leading periodic terms (accuracy about 30 µs), as recommended by the Explanatory Supplement.</remarks>
	public static double TerrestrialTimeToBarycentricDynamicalTime(double julianDateTt)
	{
		double g = (357.53 + (0.98560028 * (julianDateTt - AstronomicalConstants.JulianDateJ2000))) * AstronomicalConstants.DegreesToRadians;
		return julianDateTt + (((0.001657 * Math.Sin(a: g)) + (0.000014 * Math.Sin(a: 2.0 * g))) / AstronomicalConstants.SecondsPerDay);
	}

	/// <summary>Gets the number of Julian centuries since J2000.0.</summary>
	/// <param name="julianDate">The Julian date.</param>
	/// <returns>The number of Julian centuries.</returns>
	public static double JulianCenturiesSinceJ2000(double julianDate) => (julianDate - AstronomicalConstants.JulianDateJ2000) / AstronomicalConstants.DaysPerJulianCentury;

	/// <summary>Calculates ΔT for dates before 1972 using the polynomial expressions of Espenak &amp; Meeus (2006).</summary>
	/// <param name="year">The decimal year.</param>
	/// <returns>ΔT in seconds.</returns>
	private static double GetDeltaTBefore1972Seconds(double year)
	{
		double t;
		switch (year)
		{
			case >= 1961:
				t = year - 1975;
				return 45.45 + (1.067 * t) - (t * t / 260) - (t * t * t / 718);
			case >= 1941:
				t = year - 1950;
				return 29.07 + (0.407 * t) - (t * t / 233) + (t * t * t / 2547);
			case >= 1920:
				t = year - 1920;
				return 21.20 + (0.84493 * t) - (0.076100 * t * t) + (0.0020936 * t * t * t);
			case >= 1900:
				t = year - 1900;
				return -2.79 + (1.494119 * t) - (0.0598939 * t * t) + (0.0061966 * t * t * t) - (0.000197 * t * t * t * t);
			case >= 1860:
				t = year - 1860;
				return 7.62 + (0.5737 * t) - (0.251754 * t * t) + (0.01680668 * Math.Pow(x: t, y: 3)) - (0.0004473624 * Math.Pow(x: t, y: 4)) + (Math.Pow(x: t, y: 5) / 233174);
			case >= 1800:
				t = year - 1800;
				return 13.72 - (0.332447 * t) + (0.0068612 * t * t) + (0.0041116 * Math.Pow(x: t, y: 3)) - (0.00037436 * Math.Pow(x: t, y: 4)) + (0.0000121272 * Math.Pow(x: t, y: 5)) - (0.0000001699 * Math.Pow(x: t, y: 6)) + (0.000000000875 * Math.Pow(x: t, y: 7));
			default:
				double u = (year - 1820) / 100;
				return -20 + (32 * u * u);
		}
	}
}
