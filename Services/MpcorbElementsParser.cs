/*
 * File:        MpcorbElementsParser.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Converts MPCORB records into numerical orbital elements.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Planetoid_DB.Services;

/// <summary>Converts MPCORB records into numerical orbital elements for the ephemeris calculation.</summary>
internal static class MpcorbElementsParser
{
	/// <summary>Tries to parse a raw fixed-width MPCORB line into orbital elements.</summary>
	/// <param name="rawLine">The raw MPCORB line.</param>
	/// <param name="elements">The parsed elements on success; otherwise <c>null</c>.</param>
	/// <param name="error">A description of the problem on failure; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> if the line could be parsed and validated; otherwise, <c>false</c>.</returns>
	public static bool TryParse(string? rawLine, [NotNullWhen(returnValue: true)] out MinorPlanetOrbitalElements? elements, [NotNullWhen(returnValue: false)] out string? error)
	{
		elements = null;
		PlanetoidRecord record;
		try
		{
			record = PlanetoidRecord.Parse(rawLine: rawLine ?? string.Empty);
		}
		catch (ArgumentException ex)
		{
			error = ex.Message;
			return false;
		}
		return TryCreate(record: record, elements: out elements, error: out error);
	}

	/// <summary>Tries to convert a parsed <see cref="PlanetoidRecord"/> into orbital elements.</summary>
	/// <param name="record">The MPCORB record.</param>
	/// <param name="elements">The parsed elements on success; otherwise <c>null</c>.</param>
	/// <param name="error">A description of the problem on failure; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> if the record could be converted and validated; otherwise, <c>false</c>.</returns>
	public static bool TryCreate(PlanetoidRecord record, [NotNullWhen(returnValue: true)] out MinorPlanetOrbitalElements? elements, [NotNullWhen(returnValue: false)] out string? error)
	{
		elements = null;
		if (!TryDecodePackedEpoch(packedEpoch: record.Epoch, julianDateTt: out double epochJd))
		{
			error = $"Invalid packed epoch '{record.Epoch}'.";
			return false;
		}
		if (!TryParseDouble(text: record.MeanAnomaly, value: out double meanAnomaly) ||
			!TryParseDouble(text: record.ArgPeri, value: out double argPeri) ||
			!TryParseDouble(text: record.LongAscNode, value: out double node) ||
			!TryParseDouble(text: record.Incl, value: out double incl) ||
			!TryParseDouble(text: record.OrbEcc, value: out double ecc) ||
			!TryParseDouble(text: record.SemiMajorAxis, value: out double a))
		{
			error = "The record contains non-numeric orbital elements.";
			return false;
		}
		double h = TryParseDouble(text: record.MagAbs, value: out double hValue) ? hValue : double.NaN;
		double g = TryParseDouble(text: record.SlopeParam, value: out double gValue) ? gValue : 0.15;
		string designation = string.IsNullOrWhiteSpace(value: record.DesignationName) ? record.Index : record.DesignationName;
		MinorPlanetOrbitalElements candidate = new(
			Designation: designation,
			EpochJulianDateTt: epochJd,
			MeanAnomalyDegrees: meanAnomaly,
			ArgumentOfPerihelionDegrees: argPeri,
			LongitudeOfAscendingNodeDegrees: node,
			InclinationDegrees: incl,
			Eccentricity: ecc,
			SemiMajorAxisAu: a,
			AbsoluteMagnitude: h,
			SlopeParameter: g);
		try
		{
			candidate.Validate();
		}
		catch (ArgumentException ex)
		{
			error = ex.Message;
			return false;
		}
		elements = candidate;
		error = null;
		return true;
	}

	/// <summary>Decodes an MPC packed date (e.g. <c>K25BL</c> = 2025-11-21) into a Julian date (0h TT).</summary>
	/// <param name="packedEpoch">The five-character packed epoch.</param>
	/// <param name="julianDateTt">The Julian date in TT on success.</param>
	/// <returns><c>true</c> if the packed epoch is valid; otherwise, <c>false</c>.</returns>
	/// <remarks>The packed date format is described at https://www.minorplanetcenter.net/iau/info/PackedDates.html.</remarks>
	public static bool TryDecodePackedEpoch(string? packedEpoch, out double julianDateTt)
	{
		julianDateTt = double.NaN;
		if (packedEpoch is null || packedEpoch.Length != 5)
		{
			return false;
		}
		int century = packedEpoch[0] switch
		{
			'I' => 18,
			'J' => 19,
			'K' => 20,
			'L' => 21,
			_ => -1
		};
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
		julianDateTt = TimeScales.ToJulianDate(dateTime: new DateTime(year: year, month: month, day: day, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc));
		return true;
	}

	/// <summary>Decodes a single packed digit (0–9, A–V).</summary>
	/// <param name="c">The packed character.</param>
	/// <returns>The decoded value or −1 if invalid.</returns>
	private static int DecodePackedDigit(char c) => c switch
	{
		>= '0' and <= '9' => c - '0',
		>= 'A' and <= 'V' => c - 'A' + 10,
		_ => -1
	};

	/// <summary>Parses a number using the invariant culture.</summary>
	/// <param name="text">The text to parse.</param>
	/// <param name="value">The parsed value.</param>
	/// <returns><c>true</c> if parsing succeeded and the value is finite; otherwise, <c>false</c>.</returns>
	private static bool TryParseDouble(string? text, out double value) =>
		double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value) && double.IsFinite(d: value);
}
