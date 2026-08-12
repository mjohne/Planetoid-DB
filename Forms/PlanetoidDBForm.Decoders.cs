/*
 * File:        PlanetoidDbForm.Decoders.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class for PlanetoidDbForm containing methods for decoding MPCORB flags and references.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */


// This file is part of the PlanetoidDbForm partial class.
// It contains methods for decoding MPCORB flag, reference, packed epoch, and readable designation fields,
// as well as the static helpers DecodeReference, DecodeBase62, GetJournalName, DecodePackedEpochDate, and UnpackReadableDesignation.

using Krypton.Toolkit;

namespace Planetoid_DB;

/// <summary>Partial class for <see cref="PlanetoidDbForm"/> containing methods for decoding MPCORB flags and references.</summary>
/// <remarks>This partial class is part of the PlanetoidDbForm and provides functionality to decode the 4-hexdigit flag and compressed reference code from MPCORB.DAT.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Decodes the 4-hexdigit flag from MPCORB.DAT and displays the result in a KryptonMessageBox.</summary>
	/// <remarks>The flag encodes orbit type in the lower 6 bits and additional information in bits 6-15 according to MPC specifications.</remarks>
	private void DecodeMpcorbFlags()
	{
		// Get the flag text from the label
		string flagText = labelFlagsData.Text;
		// Validate that the flag text is not empty
		if (string.IsNullOrWhiteSpace(value: flagText))
		{
			logger.Warn(message: "Flag text is empty or whitespace");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "No flag data available.",
				caption: "Flag Decoder",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Validate that the flag text is a valid 4-hexdigit string
		try
		{
			// Parse the hex string to an integer
			int flagValue = Convert.ToInt32(value: flagText, fromBase: 16);
			// Extract orbit type (lower 6 bits); 0x3F = 0011 1111 (bits 0-5)
			int orbitType = flagValue & 0x3F;
			// Extract individual flag bits (bits 11-15) using bitwise AND and check if they are set
			bool isNeo = (flagValue & 2048) != 0;
			bool isLargeNeo = (flagValue & 4096) != 0;
			bool isOneOppObject = (flagValue & 8192) != 0;
			bool isCriticalList = (flagValue & 16384) != 0;
			bool isPha = (flagValue & 32768) != 0;
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: $"MPCORB Flag Decoder");
			_ = result.AppendLine(value: $"==================");
			_ = result.AppendLine(value: $"Hex Value: {flagText}");
			_ = result.AppendLine(value: $"Decimal Value: {flagValue}");
			_ = result.AppendLine();
			// Orbit type classification
			_ = result.AppendLine(value: "Orbit Classification:");
			string orbitTypeName = orbitType switch
			{
				1 => "Atira",
				2 => "Aten",
				3 => "Apollo",
				4 => "Amor",
				5 => "Object with q < 1.665 AU",
				6 => "Hungaria",
				7 => "Unused or internal MPC use only",
				8 => "Hilda",
				9 => "Jupiter Trojan",
				10 => "Distant object",
				_ => $"Undefined (value: {orbitType})"
			};
			_ = result.AppendLine(value: $"  {orbitTypeName}");
			_ = result.AppendLine();
			// Additional flags
			_ = result.AppendLine(value: "Additional Flags:");
			if (isNeo)
			{
				_ = result.AppendLine(value: "  ✓ Near-Earth Object (NEO)");
			}
			if (isLargeNeo)
			{
				_ = result.AppendLine(value: "  ✓ 1-km (or larger) NEO");
			}
			if (isOneOppObject)
			{
				_ = result.AppendLine(value: "  ✓ 1-opposition object seen at earlier opposition");
			}
			if (isCriticalList)
			{
				_ = result.AppendLine(value: "  ✓ Critical list numbered object");
			}
			if (isPha)
			{
				_ = result.AppendLine(value: "  ✓ Potentially Hazardous Asteroid (PHA)");
			}
			// If no additional flags are set
			if (!isNeo && !isLargeNeo && !isOneOppObject && !isCriticalList && !isPha)
			{
				_ = result.AppendLine(value: "  (none)");
			}
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "MPCORB Flag Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);

			logger.Info(message: $"Decoded MPCORB flag: {flagText} = {flagValue} ({orbitTypeName})");
		}
		// Handle format exceptions when parsing the hex string
		catch (FormatException ex)
		{
			logger.Error(exception: ex, message: $"Failed to parse flag value '{flagText}': {ex.Message}");
			ShowErrorMessage(message: $"Failed to parse flag value '{flagText}'.\n\nThe flag must be a valid hexadecimal number.\n\nError: {ex.Message}");
		}
		// Handle overflow exceptions when the hex value is too large to fit in an integer
		catch (OverflowException ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB flag: {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the flag.\n\nError: {ex.Message}");
		}
	}

	/// <summary>Decodes the compressed reference code from MPCORB.DAT and displays the full reference in a KryptonMessageBox.</summary>
	/// <remarks>Decodes various reference formats according to MPC specifications at http://www.minorplanetcenter.org/iau/info/References.html</remarks>
	private void DecodeMpcorbReference()
	{
		// Get the reference text from the label
		string compressedRef = labelReferenceData.Text;
		// Validate that the reference text is not empty
		if (string.IsNullOrWhiteSpace(value: compressedRef))
		{
			logger.Warn(message: "Reference text is empty or whitespace");
			_ = KryptonMessageBox.Show(owner: this, text: "No reference data available.", caption: "Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the reference and handle any exceptions that may occur during decoding
		try
		{
			string decodedReference = DecodeReference(compressedRef: compressedRef.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "MPCORB Reference Decoder");
			_ = result.AppendLine(value: "========================");
			_ = result.AppendLine(value: $"Compressed: {compressedRef}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Full Reference:");
			_ = result.AppendLine(value: $"  {decodedReference}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "MPCORB Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded MPCORB reference: '{compressedRef}' → '{decodedReference}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB reference '{compressedRef}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the reference:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes a compressed MPC reference string to its full form.</summary>
	/// <param name="compressedRef">The compressed reference string (typically 5 characters).</param>
	/// <returns>The full reference description.</returns>
	/// <remarks>Handles various formats including MPEC, MPC, MPS, and journal references according to MPC specifications.</remarks>
	private static string DecodeReference(string compressedRef)
	{
		// Validate input
		if (string.IsNullOrWhiteSpace(value: compressedRef))
		{
			return "Unknown reference";
		}
		// Pad the compressed reference to ensure it has at least 5 characters for consistent processing
		compressedRef = compressedRef.PadRight(totalWidth: 5);
		// Get the first character to determine the reference type
		char firstChar = compressedRef[index: 0];
		// 1: Temporary MPEC References
		if (firstChar == 'E')
		{
			return $"MPEC (temporary) - Half-month {compressedRef.AsSpan(start: 1, length: 1)}, Circular {compressedRef.AsSpan(start: 2, length: 3).TrimStart(trimChar: '0')}";
		}
		// 2A: Five-digit MPC numbers
		if (char.IsDigit(c: firstChar) && compressedRef.All(predicate: static c => char.IsDigit(c: c) || char.IsWhiteSpace(c: c)))
		{
			return int.TryParse(s: compressedRef.Trim(), result: out int mpcNumber) ? $"Minor Planet Circular (MPC) {mpcNumber}" : "Unknown reference";
		}
		// 2B: @ + four digits
		if (firstChar == '@' && int.TryParse(compressedRef.AsSpan(start: 1, length: 4), out int excess))
		{
			return $"Minor Planet Circular (MPC) {100000 + excess}";
		}
		// 2C: # + four Base-62 characters
		if (firstChar == '#')
		{
			return $"Minor Planet Circular (MPC) {110000 + DecodeBase62(encoded: compressedRef.AsSpan(start: 1, length: 4))}";
		}

		// 2D: Lowercase letter + four digits
		if (char.IsLower(c: firstChar) && int.TryParse(compressedRef.AsSpan(start: 1, length: 4), out int remainder))
		{
			return $"Minor Planet Supplement (MPS) {((firstChar - 'a') * 10000) + remainder}";
		}
		// 2E: Tilde + four Base-62 characters
		if (firstChar == '~')
		{
			return $"Minor Planet Supplement (MPS) {260000 + DecodeBase62(encoded: compressedRef.AsSpan(start: 1, length: 4))}";
		}
		// 2F: Single uppercase letter + four digits
		if (char.IsUpper(c: firstChar) && compressedRef.Length >= 2 && char.IsDigit(c: compressedRef[index: 1]) && int.TryParse(s: compressedRef.AsSpan(start: 1, length: 4), result: out int number))
		{
			return firstChar switch
			{
				'H' => $"Harvard Announcement Card (HAC) {number}",
				'I' => $"IAU Circular (IAUC) {number}",
				'M' => $"Minor Planet Circular (MPC) {number}",
				'R' => $"Planetenzirkular des Astronomischen Rechen-Institut (RI) {number}",
				_ => $"Journal '{firstChar}' #{number}"
			};
		}
		// 2G: Two or more leading letters (journal codes of varying length)
		if (compressedRef.Length >= 2 && char.IsLetter(c: firstChar))
		{
			// Count the number of leading letters in the compressed reference
			int lettersCount = compressedRef.TakeWhile(char.IsLetter).Count();
			// If there are at least two leading letters, attempt to decode the journal reference
			if (lettersCount >= 2)
			{
				// Extract the journal code and the remaining part of the reference
				string journalCode = compressedRef[..lettersCount];
				string remain = compressedRef[lettersCount..].Trim();
				string journalName = GetJournalName(code: journalCode);
				// If a valid journal name is found, format the output accordingly
				if (!string.IsNullOrEmpty(value: journalName))
				{
					return !string.IsNullOrEmpty(value: remain) && int.TryParse(s: remain, result: out int volOrCirc)
						? $"{journalName}, Vol./Circ. {volOrCirc}"
						: journalName;
				}
			}
		}
		// If none of the above formats matched, return an unknown reference format message
		logger.Warn(message: $"Unknown reference format for '{compressedRef}'");
		return $"Unknown reference format: {compressedRef.Trim()}";
	}

	/// <summary>Decodes a Base-62 encoded string to an integer.</summary>
	/// <param name="encoded">The Base-62 encoded span.</param>
	/// <returns>The decoded integer value.</returns>
	/// <remarks>Uses characters 0-9, A-Z, a-z to represent digits 0-61.</remarks>
	private static int DecodeBase62(ReadOnlySpan<char> encoded)
	{
		// Define the character set for Base-62 encoding
		const string base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		int result = 0;
		// Process each character in the encoded string
		foreach (char c in encoded)
		{
			// Find the index of the character in the Base-62 character set
			int digit = base62Chars.IndexOf(value: c);
			if (digit == -1)
			{
				// If the character is not found in the Base-62 set, throw a format exception
				throw new FormatException(message: $"Invalid Base-62 character: {c}");
			}
			result = (result * 62) + digit;
		}
		// Return the decoded integer value
		return result;
	}

	/// <summary>Decodes the packed epoch from the epoch label and displays the unpacked date in a KryptonMessageBox.</summary>
	/// <remarks>The packed epoch format is defined at https://www.minorplanetcenter.net/iau/info/PackedDates.html.</remarks>
	private void DecodePackedEpoch()
	{
		// Get the packed epoch text from the label
		string packedEpoch = labelEpochData.Text;
		// Validate that the packed epoch text is not empty
		if (string.IsNullOrWhiteSpace(value: packedEpoch))
		{
			logger.Warn(message: "Packed epoch text is empty or whitespace");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "No epoch data available.",
				caption: "Packed Epoch Decoder",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the packed epoch
		try
		{
			string decodedEpoch = DecodePackedEpochDate(packedEpoch: packedEpoch.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "Packed Epoch Decoder");
			_ = result.AppendLine(value: "====================");
			_ = result.AppendLine(value: $"Packed: {packedEpoch}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Unpacked Date (TT):");
			_ = result.AppendLine(value: $"  {decodedEpoch}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "Packed Epoch Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded packed epoch: '{packedEpoch}' → '{decodedEpoch}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding packed epoch '{packedEpoch}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the packed epoch:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes a packed MPC epoch string to its full date form.</summary>
	/// <param name="packedEpoch">The packed epoch string (5 characters, e.g. "K134Q").</param>
	/// <returns>The unpacked date as a string in <c>yyyy-MM-dd</c> format.</returns>
	/// <remarks>The packed epoch format is defined at https://www.minorplanetcenter.net/iau/info/PackedDates.html.
	/// Century letters: I = 1800, J = 1900, K = 2000. Month/day digits 1–9 map to 1–9; letters A–C (month) or A–V (day) map to 10–12 or 10–31 respectively.</remarks>
	internal static string DecodePackedEpochDate(string packedEpoch)
	{
		// Validate that the packed epoch is exactly 5 characters
		if (string.IsNullOrWhiteSpace(value: packedEpoch) || packedEpoch.Length != 5)
		{
			throw new FormatException(message: $"Packed epoch must be exactly 5 characters, got: '{packedEpoch}'");
		}
		// Decode the century from the first character
		int century = packedEpoch[index: 0] switch
		{
			'I' => 1800,
			'J' => 1900,
			'K' => 2000,
			_ => throw new FormatException(message: $"Unknown century character '{packedEpoch[index: 0]}' in packed epoch '{packedEpoch}'")
		};
		// Decode the two-digit year within the century
		if (!int.TryParse(s: packedEpoch.AsSpan(start: 1, length: 2), result: out int yearInCentury))
		{
			throw new FormatException(message: $"Invalid year digits in packed epoch '{packedEpoch}'");
		}
		int year = century + yearInCentury;
		// Decode the month character
		char monthChar = packedEpoch[index: 3];
		int month = monthChar switch
		{
			>= '1' and <= '9' => monthChar - '0',
			>= 'A' and <= 'C' => monthChar - 'A' + 10,
			_ => throw new FormatException(message: $"Invalid month character '{monthChar}' in packed epoch '{packedEpoch}'")
		};
		// Decode the day character
		char dayChar = packedEpoch[index: 4];
		int day = dayChar switch
		{
			>= '1' and <= '9' => dayChar - '0',
			>= 'A' and <= 'V' => dayChar - 'A' + 10,
			_ => throw new FormatException(message: $"Invalid day character '{dayChar}' in packed epoch '{packedEpoch}'")
		};
		// Return the unpacked date as a string in yyyy-MM-dd format
		return new DateOnly(year: year, month: month, day: day).ToString(format: "yyyy-MM-dd");
	}

	/// <summary>Decodes the readable designation from the label and displays the unpacked form in a KryptonMessageBox.</summary>
	/// <remarks>The packed designation format is defined at https://www.minorplanetcenter.net/iau/info/DesDoc.html and https://www.minorplanetcenter.net/iau/info/PackedDes.html.</remarks>
	private void DecodeReadableDesignation()
	{
		// Get the packed designation text from the label
		string packed = labelReadableDesignationData.Text;
		// Validate that the designation text is not empty
		if (string.IsNullOrWhiteSpace(value: packed))
		{
			logger.Warn(message: "Readable designation text is empty or whitespace");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "No readable designation data available.",
				caption: "Readable Designation Decoder",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the packed designation
		try
		{
			string unpacked = UnpackReadableDesignation(packed: packed.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "Readable Designation Decoder");
			_ = result.AppendLine(value: "============================");
			_ = result.AppendLine(value: $"Packed:   {packed}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Unpacked:");
			_ = result.AppendLine(value: $"  {unpacked}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "Readable Designation Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded readable designation: '{packed}' → '{unpacked}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding readable designation '{packed}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the readable designation:\n\n{ex.Message}");
		}
	}

	/// <summary>Unpacks a packed MPC designation string to its human-readable form.</summary>
	/// <param name="packed">The packed designation string as defined by the Minor Planet Center.</param>
	/// <returns>The unpacked, human-readable designation string.</returns>
	/// <remarks>Handles numbered asteroid designations, packed provisional designations (e.g. "J95X00A" → "1995 XA"),
	/// and survey designations (P-L, T-1, T-2, T-3) as specified at
	/// https://www.minorplanetcenter.net/iau/info/DesDoc.html and https://www.minorplanetcenter.net/iau/info/PackedDes.html.</remarks>
	internal static string UnpackReadableDesignation(string packed)
	{
		if (string.IsNullOrWhiteSpace(value: packed))
		{
			throw new FormatException(message: "Designation string must not be empty.");
		}
		// Already unpacked if it contains a space (e.g. "1995 XA")
		if (packed.Contains(value: ' '))
		{
			return packed;
		}
		// Survey designations: 6 chars — "PL" + 4 digits, or "T1"/"T2"/"T3" + 4 digits
		// e.g. "PL4354" → "4354 P-L", "T14354" → "4354 T-1"
		if (packed.Length == 6)
		{
			string prefix = packed[..2];
			string numPart = packed[2..];
			if (int.TryParse(s: numPart, result: out int surveyNum))
			{
				string surveyName = prefix switch
				{
					"PL" => "P-L",
					"T1" => "T-1",
					"T2" => "T-2",
					"T3" => "T-3",
					_ => string.Empty
				};
				if (!string.IsNullOrEmpty(value: surveyName))
				{
					return $"{surveyNum} {surveyName}";
				}
			}
		}
		// Provisional designations: 7 chars starting with century letter I, J, or K
		// e.g. "J95X00A" → "1995 XA", "J95X01L" → "1995 XL1", "J98SA8Q" → "1998 SQ108"
		if (packed.Length == 7 && packed[0] is 'I' or 'J' or 'K')
		{
			int century = packed[0] switch
			{
				'I' => 1800,
				'J' => 1900,
				_ => 2000  // 'K'
			};
			if (!int.TryParse(s: packed.AsSpan(start: 1, length: 2), result: out int yearInCentury))
			{
				throw new FormatException(message: $"Invalid year digits in packed designation '{packed}'");
			}
			int year = century + yearInCentury;
			char halfMonthLetter = packed[3];
			char subscriptTens = packed[4];
			char subscriptOnes = packed[5];
			char orderLetter = packed[6];
			// Decode the subscript tens character (0-9 → 0-9, A-Z → 10-35)
			int tens = subscriptTens switch
			{
				>= '0' and <= '9' => subscriptTens - '0',
				>= 'A' and <= 'Z' => subscriptTens - 'A' + 10,
				_ => throw new FormatException(message: $"Invalid subscript tens character '{subscriptTens}' in packed designation '{packed}'")
			};
			// subscriptOnes must be a digit
			if (subscriptOnes is < '0' or > '9')
			{
				throw new FormatException(message: $"Invalid subscript ones character '{subscriptOnes}' in packed designation '{packed}'");
			}
			int subscript = (tens * 10) + (subscriptOnes - '0');
			// Build the unpacked designation
			string subscriptStr = subscript == 0 ? string.Empty : subscript.ToString(provider: System.Globalization.CultureInfo.InvariantCulture);
			return $"{year} {halfMonthLetter}{orderLetter}{subscriptStr}";
		}
		// Numbered asteroid designations: 5 chars
		// "00001"–"99999" → numeric value (strip leading zeros)
		// "A0001"–"Z9999" → (A-Z encodes 10–35) * 10000 + remaining 4 digits
		// "a0000"–"z9999" → (a-z encodes 36–61) * 10000 + remaining 4 digits
		// "~xxxx" → very large numbers encoded in base-62 (out of scope for typical data)
		if (packed.Length == 5)
		{
			char first = packed[0];
			// All-digit: strip leading zeros
			if (char.IsAsciiDigit(c: first))
			{
				if (int.TryParse(s: packed, result: out int number))
				{
					return number.ToString(provider: System.Globalization.CultureInfo.InvariantCulture);
				}
			}
			// Alphanumeric prefix (A-Z or a-z) + 4 digits
			if (char.IsAsciiLetter(c: first))
			{
				int prefixValue = char.IsAsciiLetterUpper(c: first) ? first - 'A' + 10 : first - 'a' + 36;
				if (int.TryParse(s: packed.AsSpan(start: 1, length: 4), result: out int suffix))
				{
					int asteroidNumber = prefixValue * 10000 + suffix;
					return asteroidNumber.ToString(provider: System.Globalization.CultureInfo.InvariantCulture);
				}
			}
			// Tilde prefix encodes very large asteroid numbers (> 619999) in base-62 — not decoded here
			if (first == '~')
			{
				return $"Extended packed number (tilde format): {packed}";
			}
		}
		// Fallback: return the input unchanged
		return packed;
	}

	/// <summary>Gets the full journal name from a two-letter journal code.</summary>
	/// <param name="code">The two-letter journal code.</param>
	/// <returns>The full journal name, or an empty string if not found.</returns>
	/// <remarks>Supports various journal codes as specified by the Minor Planet Center; taken from https://www.minorplanetcenter.net/iau/info/References.html.</remarks>
	private static string GetJournalName(string code) => code switch
	{
		"AA" => "Astronomy and Astrophysics",
		"AB" => "Bulletin des Astrophysikalischen Observatoriums Abastumani",
		"AC" => "Astronomisches Zirkular der Akademie der Wissenschaften der UdSSR",
		"AE" => "Astronomical Papers prepared for the use of the American Ephemeris and Nautical Almanac",
		"AJ" => "Astronomical Journal",
		"AN" => "Astronomische Nachrichten",
		"AP" => "Astrophysical Journal Supplement",
		"As" => "Astronomy and Astrophysics Supplement",
		"BA" => "Bulletin Astronomique",
		"BB" => "Bulletin Astronomique de l'Observatoire Royal de Belgique, Uccle",
		"BC" => "Bulletin of the Astronomical Institutes of Czechoslovakia",
		"BG" => "Bulletin de l'Observatoire Astronomique de Beograd",
		"BN" => "Bulletin of the Astronomical Institutes of the Netherlands",
		"BP" => "Bulletin de la Societe des amis des sciences et des lettres de Poznan",
		"BZ" => "Beobachtungs-Zirkulare der Astronomischen Nachrichten",
		"CB" => "Comet Bulletin of the Orient Astronomical Association",
		"CC" => "Observatorio Astronomico de Cordoba, Serie Contribuciones",
		"CD" => "Tsirkulyari Rasadkhonai Stalinobod",
		"CK" => "Izvestiya Krymskoj Astrofizicheskoj Observatorii",
		"CM" => "Circulaire de l'Observatoire de Marseille",
		"CO" => "Odesskij Gosudarstvennyj Universitet Izvestiya Astronomicheskoj Observattorii",
		"CR" => "Comptes Rendus hebdomadaires de l'academie des sciences de Paris",
		"CS" => "Soobshcheniya Gosudarstvennogo Astronomicheskogo Instituta imeni P. K. Shternberga",
		"GO" => "Greenwich Observations",
		"HA" => "Harvard Annal",
		"HD" => "Veröffentlichungen der Landessternwarte Heidelberg",
		"HTCDR" => "Hipparcos-Tycho CD-ROM",
		"IHW" => "International Halley Watch CD-ROM",
		"Ic" => "Icarus",
		"JB" => "Journal of the British Astronomical Association",
		"JC" => "Japan Astronomical Study Association Circular",
		"JO" => "Journal des Observateurs",
		"KB" => "Bulletin of the Kwasan Observatory, Kyoto",
		"KK" => "Kiev Komet Tsirkular",
		"LB" => "Lick Observatory Bulletin",
		"LO" => "Lowell Observatory Bulletin",
		"LP" => "Publicaciones Observatorio Astronomico de La Plata",
		"MN" => "Monthly Notices of the Royal Astronomical Society",
		"NA" => "Annales de l'Observatoire de Nice",
		"NC" => "Nihondaira Observatory Circular",
		"NO" => "Publications of the U.S. Naval Observatory, Second Series",
		"NZ" => "Nachrichtenblatt der Astronomischen Zentralstelle",
		"OB" => "The Observatory",
		"PA" => "Publications of the Astronomical Society of the Pacific",
		"PC" => "Poulkovo Observatory Circular",
		"PD" => "Tartu Astronoomia Observatooriumi Publikatsioonid",
		"PK" => "Pyublikatsii Kievskoj Astronomicheskoj Observatorii",
		"PO" => "Perth Observatory Communication",
		"PP" => "Izvestiya Glavnoj Astronomicheskoj Observatorii v Pulkove",
		"PT" => "Pubblicazioni del Osservatorio di Torino",
		"PZ" => "Zirkular des Astronomischen Hauptobservatoriums Pulkowo",
		"RA" => "Ricerche Astronomiche",
		"RM" => "Memoirs of the Royal Astronomical Society",
		"SA" => "Monthly Notices of the Astronomical Society of Southern Africa",
		"SOB" => "Observatory Bulletin",
		"TB" => "Tokyo Astronomical Bulletin",
		"TC" => "Transval Observatory Circular",
		"TI" => "Astronomia-Optika Institucio, Universitato de Turku, Informo",
		"UC" => "Circular of the Union Observatory, Johannesburg",
		"WO" => "Astronomical Observations of the U.S. Naval Observatory, Washington",
		"WiA" => "Annalen der Sternwarte der Universität Wien",
		"pM" => "Mitteilungen der Nikolai-Hauptsternwarte zu Pulkowo",
		"CMC" => "Carlsberg Meridian Circle Publications",
		"APO" => "Annales de l'Observatoire de Paris: Observations",
		"AS" => "Acta Astronomica Sinica",
		"AZ" => "Astronomicheskij Zhurnal",
		"AcA" => "Acta Astronomica",
		_ => string.Empty
	};
}
