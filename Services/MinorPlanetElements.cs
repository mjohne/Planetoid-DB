/*
 * File:        MinorPlanetElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents validated osculating orbital elements of a minor planet parsed from an MPCORB record.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Planetoid_DB.Services;

/// <summary>Represents validated heliocentric osculating orbital elements of a minor planet (ecliptic and equinox J2000.0).</summary>
/// <param name="Designation">The readable designation (or the packed designation if no readable one is available).</param>
/// <param name="AbsoluteMagnitude">The absolute magnitude H in mag, or <see cref="double.NaN"/> if unknown.</param>
/// <param name="SlopeParameter">The slope parameter G (dimensionless).</param>
/// <param name="EpochJulianDateTt">The osculation epoch as Julian date in Terrestrial Time (TT).</param>
/// <param name="MeanAnomalyDegrees">The mean anomaly at epoch in degrees.</param>
/// <param name="ArgumentOfPerihelionDegrees">The argument of perihelion ω in degrees (J2000.0).</param>
/// <param name="LongitudeOfAscendingNodeDegrees">The longitude of the ascending node Ω in degrees (J2000.0).</param>
/// <param name="InclinationDegrees">The inclination to the ecliptic in degrees (J2000.0).</param>
/// <param name="Eccentricity">The orbital eccentricity (dimensionless, 0 ≤ e &lt; 1).</param>
/// <param name="SemiMajorAxisAu">The semi-major axis in AU.</param>
internal sealed record MinorPlanetElements(
	string Designation,
	double AbsoluteMagnitude,
	double SlopeParameter,
	double EpochJulianDateTt,
	double MeanAnomalyDegrees,
	double ArgumentOfPerihelionDegrees,
	double LongitudeOfAscendingNodeDegrees,
	double InclinationDegrees,
	double Eccentricity,
	double SemiMajorAxisAu)
{
	/// <summary>Default slope parameter G used when the record does not provide one.</summary>
	public const double DefaultSlopeParameter = 0.15;

	/// <summary>Minimum length of an MPCORB record that contains all orbital elements (up to the semi-major axis).</summary>
	private const int MinimumRecordLength = 103;

	/// <summary>Parses an MPCORB.DAT fixed-width record.</summary>
	/// <param name="record">The raw MPCORB record.</param>
	/// <returns>The validated orbital elements.</returns>
	/// <exception cref="FormatException">Thrown when the record is missing, truncated, malformed or physically invalid.</exception>
	public static MinorPlanetElements Parse(string? record) => TryParse(record: record, elements: out MinorPlanetElements? elements, error: out string? error)
		? elements
		: throw new FormatException(message: error);

	/// <summary>Tries to parse an MPCORB.DAT fixed-width record.</summary>
	/// <param name="record">The raw MPCORB record.</param>
	/// <param name="elements">The parsed elements if successful; otherwise <c>null</c>.</param>
	/// <param name="error">A description of the problem if parsing failed; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> if the record is valid; otherwise <c>false</c>.</returns>
	public static bool TryParse(string? record, [NotNullWhen(returnValue: true)] out MinorPlanetElements? elements, [NotNullWhen(returnValue: false)] out string? error)
	{
		elements = null;
		if (string.IsNullOrWhiteSpace(value: record) || record.Length < MinimumRecordLength)
		{
			error = $"The MPCORB record is empty or shorter than {MinimumRecordLength} characters.";
			return false;
		}
		string packedDesignation = record[..7].Trim();
		if (packedDesignation.Length == 0)
		{
			error = "The MPCORB record has no designation.";
			return false;
		}
		double absoluteMagnitude = TryParseField(record: record, start: 8, length: 5, value: out double h) ? h : double.NaN;
		double slope = TryParseField(record: record, start: 14, length: 5, value: out double g) ? g : DefaultSlopeParameter;
		if (!TryDecodePackedEpoch(packedEpoch: record.Substring(startIndex: 20, length: 5), julianDateTt: out double epoch))
		{
			error = $"Invalid packed epoch '{record.Substring(startIndex: 20, length: 5)}'.";
			return false;
		}
		if (!TryParseField(record: record, start: 26, length: 9, value: out double meanAnomaly)
			|| !TryParseField(record: record, start: 37, length: 9, value: out double argPeri)
			|| !TryParseField(record: record, start: 48, length: 9, value: out double node)
			|| !TryParseField(record: record, start: 59, length: 9, value: out double incl)
			|| !TryParseField(record: record, start: 70, length: 9, value: out double ecc)
			|| !TryParseField(record: record, start: 92, length: 11, value: out double a))
		{
			error = "The MPCORB record contains a missing or non-numeric orbital element.";
			return false;
		}
		if (ecc is < 0.0 or >= 1.0)
		{
			error = $"Eccentricity {ecc.ToString(provider: CultureInfo.InvariantCulture)} is outside the elliptical range [0, 1).";
			return false;
		}
		if (a <= 0.0)
		{
			error = "The semi-major axis must be positive.";
			return false;
		}
		if (incl is < 0.0 or > 180.0 || meanAnomaly is < 0.0 or >= 360.0 || argPeri is < 0.0 or >= 360.0 || node is < 0.0 or >= 360.0)
		{
			error = "An angular orbital element is outside its valid range.";
			return false;
		}
		string designation = record.Length >= 194 ? record.Substring(startIndex: 166, length: 28).Trim() : string.Empty;
		elements = new MinorPlanetElements(
			Designation: designation.Length > 0 ? designation : packedDesignation,
			AbsoluteMagnitude: absoluteMagnitude,
			SlopeParameter: slope,
			EpochJulianDateTt: epoch,
			MeanAnomalyDegrees: meanAnomaly,
			ArgumentOfPerihelionDegrees: argPeri,
			LongitudeOfAscendingNodeDegrees: node,
			InclinationDegrees: incl,
			Eccentricity: ecc,
			SemiMajorAxisAu: a);
		error = null;
		return true;
	}

	/// <summary>Decodes an MPC packed epoch (e.g. <c>K2555</c>) into a Julian date (TT, 0h).</summary>
	/// <param name="packedEpoch">The five-character packed epoch.</param>
	/// <param name="julianDateTt">The decoded Julian date if successful.</param>
	/// <returns><c>true</c> if the packed epoch is valid; otherwise <c>false</c>.</returns>
	public static bool TryDecodePackedEpoch(string packedEpoch, out double julianDateTt)
	{
		julianDateTt = double.NaN;
		if (packedEpoch is not { Length: 5 })
		{
			return false;
		}
		int century = packedEpoch[0] switch { 'I' => 18, 'J' => 19, 'K' => 20, 'L' => 21, _ => -1 };
		if (century < 0 || !char.IsAsciiDigit(c: packedEpoch[1]) || !char.IsAsciiDigit(c: packedEpoch[2]))
		{
			return false;
		}
		int year = (century * 100) + ((packedEpoch[1] - '0') * 10) + (packedEpoch[2] - '0');
		int month = DecodePackedDigit(c: packedEpoch[3]);
		int day = DecodePackedDigit(c: packedEpoch[4]);
		if (month is < 1 or > 12 || day < 1 || day > DateTime.DaysInMonth(year: year, month: month))
		{
			return false;
		}
		julianDateTt = AstronomicalTime.ToJulianDate(utc: new DateTimeOffset(year: year, month: month, day: day, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero));
		return true;
	}

	/// <summary>Decodes a single packed digit (0–9, A–V for 10–31).</summary>
	/// <param name="c">The packed character.</param>
	/// <returns>The decoded value, or -1 if invalid.</returns>
	private static int DecodePackedDigit(char c) => c switch
	{
		>= '0' and <= '9' => c - '0',
		>= 'A' and <= 'V' => c - 'A' + 10,
		_ => -1
	};

	/// <summary>Parses a numeric fixed-width field using the invariant culture.</summary>
	/// <param name="record">The record.</param>
	/// <param name="start">The zero-based start column.</param>
	/// <param name="length">The field length.</param>
	/// <param name="value">The parsed value.</param>
	/// <returns><c>true</c> if the field contains a finite number; otherwise <c>false</c>.</returns>
	private static bool TryParseField(string record, int start, int length, out double value)
	{
		value = double.NaN;
		if (record.Length < start + length)
		{
			return false;
		}
		return double.TryParse(s: record.AsSpan(start: start, length: length).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value) && double.IsFinite(d: value);
	}
}
