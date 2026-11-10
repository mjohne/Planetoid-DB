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
	/// <remarks>J2000.0 is the standard epoch used in astronomy for celestial coordinates and orbital elements.</remarks>
	public const double J2000 = 2451545.0;

	/// <summary>Number of days per Julian century.</summary>
	/// <remarks>A Julian century is defined as 36525 days, used in astronomical calculations for long-term time scales.</remarks>
	public const double DaysPerJulianCentury = 36525.0;

	/// <summary>Seconds per day.</summary>
	/// <remarks>There are 86400 seconds in a standard day, used for time scale conversions.</remarks>
	public const double SecondsPerDay = 86400.0;

	/// <summary>The instant 2000-01-01 12:00:00 used as reference for Julian date conversions.</summary>
	/// <remarks>This is the reference instant for converting between <see cref="DateTime"/> and Julian dates, corresponding to J2000.0.</remarks>
	private static readonly DateTime J2000DateTime = new(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc);

	/// <summary>Leap second table: UTC date from which the given TAI−UTC [s] applies.</summary>
	/// <remarks>This table is based on the IERS Bulletin C leap second announcements. Each entry specifies the UTC date from which the corresponding TAI−UTC offset applies. The last entry corresponds to the most recent leap second adjustment.</remarks>
	private static readonly (DateTime Since, int TaiMinusUtc)[] LeapSeconds =
	[
		(Since: new DateTime(year: 1972, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 10),
		(Since: new DateTime(year: 1972, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 11),
		(Since: new DateTime(year: 1973, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 12),
		(Since: new DateTime(year: 1974, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 13),
		(Since: new DateTime(year: 1975, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 14),
		(Since: new DateTime(year: 1976, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 15),
		(Since: new DateTime(year: 1977, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 16),
		(Since: new DateTime(year: 1978, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 17),
		(Since: new DateTime(year: 1979, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 18),
		(Since: new DateTime(year: 1980, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 19),
		(Since: new DateTime(year: 1981, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 20),
		(Since: new DateTime(year: 1982, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 21),
		(Since: new DateTime(year: 1983, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 22),
		(Since: new DateTime(year: 1985, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 23),
		(Since: new DateTime(year: 1988, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 24),
		(Since: new DateTime(year: 1990, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 25),
		(Since: new DateTime(year: 1991, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 26),
		(Since: new DateTime(year: 1992, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 27),
		(Since: new DateTime(year: 1993, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 28),
		(Since: new DateTime(year: 1994, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 29),
		(Since: new DateTime(year: 1996, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 30),
		(Since: new DateTime(year: 1997, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 31),
		(Since: new DateTime(year: 1999, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 32),
		(Since: new DateTime(year: 2006, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 33),
		(Since: new DateTime(year: 2009, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 34),
		(Since: new DateTime(year: 2012, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 35),
		(Since: new DateTime(year: 2015, month: 7, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 36),
		(Since: new DateTime(year: 2017, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc), TaiMinusUtc: 37)
	];

	/// <summary>Converts a <see cref="DateTime"/> to a Julian date in the same time scale.</summary>
	/// <param name="dateTime">The date and time. A <see cref="DateTimeKind.Local"/> value is converted to UTC first.</param>
	/// <returns>The Julian date [d].</returns>
	/// <remarks>This method converts a <see cref="DateTime"/> to a Julian date, taking into account the time scale. If the input <see cref="DateTime"/> is in local time, it is first converted to UTC before calculating the Julian date.</remarks>
	public static double ToJulianDate(DateTime dateTime)
	{
		// Convert the input DateTime to UTC if it is in local time; otherwise, ensure it is treated as UTC
		DateTime utc = dateTime.Kind == DateTimeKind.Local ? dateTime.ToUniversalTime() : DateTime.SpecifyKind(value: dateTime, kind: DateTimeKind.Utc);
		// Calculate the Julian date by adding the number of days since J2000.0 to the J2000 constant
		return J2000 + ((utc - J2000DateTime).Ticks / (double)TimeSpan.TicksPerDay);
	}

	/// <summary>Converts a <see cref="DateTimeOffset"/> to a Julian date in UTC.</summary>
	/// <param name="time">The instant (any offset; it is converted to UTC).</param>
	/// <returns>The Julian date (UTC) [d].</returns>
	/// <remarks>This method converts a <see cref="DateTimeOffset"/> to a Julian date in UTC, regardless of the original offset. The input time is converted to UTC before calculating the Julian date.</remarks>
	public static double ToJulianDateUtc(DateTimeOffset time)
	{
		// Convert the input DateTimeOffset to UTC and then calculate the Julian date using the ToJulianDate method
		return ToJulianDate(dateTime: time.UtcDateTime);
	}

	/// <summary>Converts a Julian date into a UTC <see cref="DateTimeOffset"/>.</summary>
	/// <param name="julianDate">The Julian date [d].</param>
	/// <returns>The instant with offset +00:00.</returns>
	/// <remarks>This method converts a Julian date to a <see cref="DateTimeOffset"/> in UTC, using the J2000.0 reference instant as the basis for the conversion.</remarks>
	public static DateTimeOffset FromJulianDate(double julianDate)
	{
		// Calculate the number of ticks since J2000.0 and create a DateTimeOffset in UTC
		long ticks = (long)Math.Round(a: (julianDate - J2000) * TimeSpan.TicksPerDay);
		// Return a new DateTimeOffset representing the UTC instant corresponding to the given Julian date
		return new DateTimeOffset(dateTime: J2000DateTime.AddTicks(value: ticks), offset: TimeSpan.Zero);
	}

	/// <summary>Gets TT − UTC in seconds for the given UTC instant.</summary>
	/// <param name="utc">The UTC instant.</param>
	/// <returns>TT − UTC [s] (equal to ΔT before 1972, where UTC ≈ UT).</returns>
	/// <remarks>This method calculates the difference between Terrestrial Time (TT) and Coordinated Universal Time (UTC) in seconds for a given UTC instant. Before 1972, it uses the Espenak–Meeus ΔT polynomials to estimate the difference, while after 1972, it accounts for leap seconds based on the IERS Bulletin C table.</remarks>
	public static double TtMinusUtcSeconds(DateTime utc)
	{
		// If the UTC instant is before the first leap second, use the ΔT polynomial approximation for times before 1972
		if (utc < LeapSeconds[0].Since)
		{
			// Calculate the decimal year for the given UTC instant, accounting for the day of the year and assuming a year length of 365.25 days
			return DeltaTBefore1972(decimalYear: utc.Year + ((utc.DayOfYear - 0.5) / 365.25));
		}
		// For UTC instants after 1972, determine the current TAI−UTC offset based on the leap second table
		int taiMinusUtc = LeapSeconds[0].TaiMinusUtc;
		// Iterate through the leap second table to find the most recent leap second that applies to the given UTC instant
		foreach ((DateTime since, int value) in LeapSeconds)
		{
			// If the UTC instant is on or after the "since" date of the leap second, update the TAI−UTC offset
			if (utc >= since)
			{
				taiMinusUtc = value;
			}
		}
		// Return the total difference TT − UTC, which is equal to TAI−UTC + 32.184 seconds
		return taiMinusUtc + 32.184;
	}

	/// <summary>Converts a UTC instant to a Julian date in Terrestrial Time.</summary>
	/// <param name="time">The instant.</param>
	/// <returns>The Julian date (TT) [d].</returns>
	/// <remarks>This method converts a UTC instant to a Julian date in Terrestrial Time (TT) by first calculating the Julian date in UTC and then adding the TT−UTC difference in days.</remarks>
	public static double ToJulianDateTt(DateTimeOffset time)
	{
		// Convert the input DateTimeOffset to UTC and then calculate the Julian date in TT
		return ToJulianDateUtc(time: time) + (TtMinusUtcSeconds(utc: time.UtcDateTime) / SecondsPerDay);
	}

	/// <summary>Converts a Julian date in TT to TDB.</summary>
	/// <param name="julianDateTt">The Julian date (TT) [d].</param>
	/// <returns>The Julian date (TDB) [d].</returns>
	/// <remarks>Uses the two leading periodic terms (amplitude 1.657 ms); the error is below 30 µs.</remarks>
	public static double TtToTdb(double julianDateTt)
	{
		// Compute the mean anomaly of the Sun (in radians) based on the Julian date in TT
		double g = (357.53 + (0.98560028 * (julianDateTt - J2000))) * Math.PI / 180.0;
		// Return the Julian date in TDB by adding the periodic terms to the Julian date in TT, converted to days
		return julianDateTt + (((0.001657 * Math.Sin(a: g)) + (0.000014 * Math.Sin(a: 2.0 * g))) / SecondsPerDay);
	}

	/// <summary>Computes ΔT before 1972 using the Espenak–Meeus polynomials.</summary>
	/// <param name="decimalYear">The decimal year.</param>
	/// <returns>ΔT [s].</returns>
	/// <remarks>This method computes the difference ΔT (TT−UT) for years before 1972 using polynomial approximations provided by Espenak and Meeus. The input is a decimal year, and the output is the estimated ΔT in seconds.</remarks>
	private static double DeltaTBefore1972(double decimalYear)
	{
		// Use the Espenak–Meeus polynomial approximations for ΔT based on the decimal year. The following piecewise polynomial approximations are based on historical data and provide estimates for ΔT in seconds for different time periods.
		double y = decimalYear;
		// For years before -500, use a quadratic approximation
		if (y < -500)
		{
			double u = (y - 1820) / 100;
			return -20 + (32 * u * u);
		}
		// For years between -500 and 500, use a sixth-degree polynomial approximation
		if (y < 500)
		{
			double u = y / 100;
			return 10583.6 - (1014.41 * u) + (33.78311 * Math.Pow(x: u, y: 2)) - (5.952053 * Math.Pow(x: u, y: 3))
				- (0.1798452 * Math.Pow(x: u, y: 4)) + (0.022174192 * Math.Pow(x: u, y: 5)) + (0.0090316521 * Math.Pow(x: u, y: 6));
		}
		// For years between 500 and 1600, use a sixth-degree polynomial approximation
		if (y < 1600)
		{
			double u = (y - 1000) / 100;
			return 1574.2 - (556.01 * u) + (71.23472 * Math.Pow(x: u, y: 2)) + (0.319781 * Math.Pow(x: u, y: 3))
				- (0.8503463 * Math.Pow(x: u, y: 4)) - (0.005050998 * Math.Pow(x: u, y: 5)) + (0.0083572073 * Math.Pow(x: u, y: 6));
		}
		// For years between 1600 and 1700, use a cubic approximation
		if (y < 1700)
		{
			double t = y - 1600;
			return 120 - (0.9808 * t) - (0.01532 * t * t) + (t * t * t / 7129);
		}
		// For years between 1700 and 1800, use a quartic approximation
		if (y < 1800)
		{
			double t = y - 1700;
			return 8.83 + (0.1603 * t) - (0.0059285 * t * t) + (0.00013336 * t * t * t) - (Math.Pow(x: t, y: 4) / 1174000);
		}
		// For years between 1800 and 1860, use a seventh-degree polynomial approximation
		if (y < 1860)
		{
			double t = y - 1800;
			return 13.72 - (0.332447 * t) + (0.0068612 * Math.Pow(x: t, y: 2)) + (0.0041116 * Math.Pow(x: t, y: 3))
				- (0.00037436 * Math.Pow(x: t, y: 4)) + (0.0000121272 * Math.Pow(x: t, y: 5))
				- (0.0000001699 * Math.Pow(x: t, y: 6)) + (0.000000000875 * Math.Pow(x: t, y: 7));
		}
		// For years between 1860 and 1900, use a fifth-degree polynomial approximation
		if (y < 1900)
		{
			double t = y - 1860;
			return 7.62 + (0.5737 * t) - (0.251754 * Math.Pow(x: t, y: 2)) + (0.01680668 * Math.Pow(x: t, y: 3))
				- (0.0004473624 * Math.Pow(x: t, y: 4)) + (Math.Pow(x: t, y: 5) / 233174);
		}
		// For years between 1961 and 1986, use a cubic approximation
		if (y >= 1961)
		{
			double t = y - 1975;
			return 45.45 + (1.067 * t) - (t * t / 260) - (t * t * t / 718);
		}
		// For years between 1941 and 1961, use a cubic approximation
		if (y >= 1941)
		{
			double t = y - 1950;
			return 29.07 + (0.407 * t) - (t * t / 233) + (t * t * t / 2547);
		}
		// For years between 1920 and 1941, use a cubic approximation
		if (y >= 1920)
		{
			double t = y - 1920;
			return 21.20 + (0.84493 * t) - (0.076100 * t * t) + (0.0020936 * t * t * t);
		}
		// For years between 1900 and 1920, use a cubic approximation
		if (y >= 1900)
		{
			double t = y - 1900;
			return -2.79 + (1.494119 * t) - (0.0598939 * t * t) + (0.0061966 * t * t * t) - (0.000197 * t * t * t * t);
		}
		// If the year is not covered by the above cases, throw an exception indicating that the ΔT model does not cover the requested year
		throw new InvalidOperationException("The ΔT model does not cover the requested year.");
	}
}
