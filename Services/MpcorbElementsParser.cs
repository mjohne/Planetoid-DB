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
/// <remarks>The MPCORB format is described at https://www.minorplanetcenter.net/iau/info/MPOrbitFormat.html.</remarks>
internal static class MpcorbElementsParser
{
	/// <summary>Tries to parse a raw fixed-width MPCORB line into orbital elements.</summary>
	/// <param name="rawLine">The raw MPCORB line.</param>
	/// <param name="elements">The parsed elements on success; otherwise <c>null</c>.</param>
	/// <param name="error">A description of the problem on failure; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> if the line could be parsed and validated; otherwise, <c>false</c>.</returns>
	/// <remarks>This method first parses the raw line into a <see cref="PlanetoidRecord"/> and then converts it into <see cref="MinorPlanetOrbitalElements"/>. It validates the packed epoch, orbital elements, and other parameters.</remarks>
	public static bool TryParse(string? rawLine, [NotNullWhen(returnValue: true)] out MinorPlanetOrbitalElements? elements, [NotNullWhen(returnValue: false)] out string? error)
	{
		// Initialize the output parameters to indicate failure by default
		elements = null;
		// Try to parse the raw line into a PlanetoidRecord; if parsing fails, set the error message and return false
		PlanetoidRecord record;
		try
		{
			// Use the PlanetoidRecord.Parse method to parse the raw line; if rawLine is null, use an empty string to avoid exceptions
			record = PlanetoidRecord.Parse(rawLine: rawLine ?? string.Empty);
		}
		catch (ArgumentException ex)
		{
			// If parsing fails, set the error message and return false
			error = ex.Message;
			return false;
		}
		// Try to convert the parsed record into orbital elements; if conversion fails, set the error message and return false
		return TryCreate(record: record, elements: out elements, error: out error);
	}

	/// <summary>Tries to convert a parsed <see cref="PlanetoidRecord"/> into orbital elements.</summary>
	/// <param name="record">The MPCORB record.</param>
	/// <param name="elements">The parsed elements on success; otherwise <c>null</c>.</param>
	/// <param name="error">A description of the problem on failure; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> if the record could be converted and validated; otherwise, <c>false</c>.</returns>
	/// <remarks>This method validates the packed epoch, orbital elements, and other parameters. It constructs a <see cref="MinorPlanetOrbitalElements"/> instance if all values are valid.</remarks>
	public static bool TryCreate(PlanetoidRecord record, [NotNullWhen(returnValue: true)] out MinorPlanetOrbitalElements? elements, [NotNullWhen(returnValue: false)] out string? error)
	{
		// Initialize the output parameters to indicate failure by default
		elements = null;
		// Validate and decode the packed epoch into a Julian date in TT (Terrestrial Time)
		if (!TryDecodePackedEpoch(packedEpoch: record.Epoch, julianDateTt: out double epochJd))
		{
			// If the packed epoch is invalid, set the error message and return false
			error = $"Invalid packed epoch '{record.Epoch}'.";
			return false;
		}
		// Try to parse the orbital elements from the record; if any are non-numeric, set the error message and return false
		if (!TryParseDouble(text: record.MeanAnomaly, value: out double meanAnomaly) ||
			!TryParseDouble(text: record.ArgPeri, value: out double argPeri) ||
			!TryParseDouble(text: record.LongAscNode, value: out double node) ||
			!TryParseDouble(text: record.Incl, value: out double incl) ||
			!TryParseDouble(text: record.OrbEcc, value: out double ecc) ||
			!TryParseDouble(text: record.SemiMajorAxis, value: out double a))
		{
			// If any of the orbital elements are non-numeric, set the error message and return false
			error = "The record contains non-numeric orbital elements.";
			return false;
		}
		// Try to parse the absolute magnitude (H) and slope parameter (G); if H is non-numeric, set it to NaN; if G is non-numeric, set it to 0.15
		double h = TryParseDouble(text: record.MagAbs, value: out double hValue) ? hValue : double.NaN;
		double g = TryParseDouble(text: record.SlopeParam, value: out double gValue) ? gValue : 0.15;
		// Use the designation name if available; otherwise, use the index as the designation
		string designation = string.IsNullOrWhiteSpace(value: record.DesignationName) ? record.Index : record.DesignationName;
		// Create a new MinorPlanetOrbitalElements instance with the parsed values
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
		// Validate the candidate elements; if validation fails, set the error message and return false
		try
		{
			// The Validate method checks for valid ranges and constraints on the orbital elements
			candidate.Validate();
		}
		catch (ArgumentException ex)
		{
			// If validation fails, set the error message and return false
			error = ex.Message;
			return false;
		}
		// If all parsing and validation succeeded, set the output elements and clear the error message
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
		// Initialize the output parameter to NaN to indicate failure by default
		julianDateTt = double.NaN;
		// Validate the packed epoch format: it must be exactly 5 characters long
		if (packedEpoch is null || packedEpoch.Length != 5)
		{
			return false;
		}
		// Decode the century from the first character of the packed epoch
		int century = packedEpoch[0] switch
		{
			'I' => 18,
			'J' => 19,
			'K' => 20,
			'L' => 21,
			_ => -1
		};
		// Validate that the century is valid and that the year digits are numeric
		if (century < 0 || !char.IsAsciiDigit(c: packedEpoch[1]) || !char.IsAsciiDigit(c: packedEpoch[2]))
		{
			return false;
		}
		// Decode the year, month, and day from the packed epoch
		int year = (century * 100) + ((packedEpoch[1] - '0') * 10) + (packedEpoch[2] - '0');
		int month = DecodePackedDigit(c: packedEpoch[3]);
		int day = DecodePackedDigit(c: packedEpoch[4]);
		// Validate the month and day values to ensure they represent a valid date
		if (month is < 1 or > 12 || day < 1 || day > DateTime.DaysInMonth(year: year, month: month))
		{
			return false;
		}
		// Convert the decoded date to a Julian date in TT (Terrestrial Time) using the TimeScales helper class
		julianDateTt = TimeScales.ToJulianDate(dateTime: new DateTime(year: year, month: month, day: day, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc));
		// Return true to indicate successful decoding of the packed epoch
		return true;
	}

	/// <summary>Decodes a single packed digit (0–9, A–V).</summary>
	/// <param name="c">The packed character.</param>
	/// <returns>The decoded value or −1 if invalid.</returns>
	/// <remarks>The packed digit format is described at https://www.minorplanetcenter.net/iau/info/PackedDates.html.</remarks>
	private static int DecodePackedDigit(char c)
	{
		// The packed digit can be '0'–'9' (0–9) or 'A'–'V' (10–31). Return -1 for invalid characters.
		return c switch
		{
			>= '0' and <= '9' => c - '0',
			>= 'A' and <= 'V' => c - 'A' + 10,
			_ => -1
		};
	}

	/// <summary>Parses a number using the invariant culture.</summary>
	/// <param name="text">The text to parse.</param>
	/// <param name="value">The parsed value.</param>
	/// <returns><c>true</c> if parsing succeeded and the value is finite; otherwise, <c>false</c>.</returns>
	/// <remarks>This method uses <see cref="double.TryParse(string?, NumberStyles, IFormatProvider?, out double)"/> with <see cref="NumberStyles.Float"/> and <see cref="CultureInfo.InvariantCulture"/> to ensure consistent parsing of floating-point numbers regardless of the current culture.</remarks>
	private static bool TryParseDouble(string? text, out double value)
	{
		// Use invariant culture to parse the number, allowing for decimal points and scientific notation
		return double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value) && double.IsFinite(d: value);
	}
}
