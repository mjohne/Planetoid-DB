// This file is part of the PlanetoidDbForm partial class.
// It contains methods for decoding MPCORB flag and reference fields,
// as well as the static helpers DecodeReference, DecodeBase62, and GetJournalName.

using Krypton.Toolkit;

namespace Planetoid_DB;

/// <summary>Partial class for PlanetoidDbForm containing methods for decoding MPCORB flags and references.</summary>
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
			// Extract orbit type (lower 6 bits)
			int orbitType = flagValue & 0x3F; // 0x3F = 0011 1111 (bits 0-5)
											  // Extract individual flag bits
			bool isNeo = (flagValue & 2048) != 0;          // Bit 11
			bool isLargeNeo = (flagValue & 4096) != 0;     // Bit 12
			bool isOneOppObject = (flagValue & 8192) != 0; // Bit 13
			bool isCriticalList = (flagValue & 16384) != 0;// Bit 14
			bool isPha = (flagValue & 32768) != 0;         // Bit 15
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
		string referenceText = labelReferenceData.Text;
		// Validate that the reference text is not empty
		if (string.IsNullOrWhiteSpace(value: referenceText))
		{
			logger.Warn(message: "Reference text is empty or whitespace");
			_ = KryptonMessageBox.Show(owner: this, text: "No reference data available.", caption: "Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the reference and handle any exceptions that may occur during decoding
		try
		{
			string decodedReference = DecodeReference(compressedRef: referenceText.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "MPCORB Reference Decoder");
			_ = result.AppendLine(value: "========================");
			_ = result.AppendLine(value: $"Compressed: {referenceText}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Full Reference:");
			_ = result.AppendLine(value: $"  {decodedReference}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "MPCORB Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded MPCORB reference: '{referenceText}' → '{decodedReference}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB reference '{referenceText}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the reference:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes a compressed MPC reference string to its full form.</summary>
	/// <param name="compressedRef">The compressed reference string (typically 5 characters).</param>
	/// <returns>The full reference description.</returns>
	/// <remarks>Handles various formats including MPEC, MPC, MPS, and journal references according to MPC specifications.</remarks>
	private static string DecodeReference(string compressedRef)
	{
		if (string.IsNullOrWhiteSpace(value: compressedRef))
		{
			return "Unknown reference";
		}
		// Ensure the reference is exactly 5 characters for proper parsing
		compressedRef = compressedRef.PadRight(totalWidth: 5);
		char firstChar = compressedRef[index: 0];
		// 1: Temporary MPEC References (E + half-month + number)
		if (firstChar == 'E')
		{
			string halfMonth = compressedRef.Substring(startIndex: 1, length: 1);
			string circularNumber = compressedRef.Substring(startIndex: 2, length: 3).TrimStart(trimChar: '0');
			return $"MPEC (temporary) - Half-month {halfMonth}, Circular {circularNumber}";
		}
		// 2A: Five-digit MPC numbers (00001-99999)
		if (char.IsDigit(c: firstChar) && compressedRef.All(predicate: c => char.IsDigit(c: c) || char.IsWhiteSpace(c: c)))
		{
			if (int.TryParse(s: compressedRef.Trim(), result: out int mpcNumber))
			{
				return $"Minor Planet Circular (MPC) {mpcNumber}";
			}
		}
		// 2B: @ + four digits (MPC 100000-109999)
		if (firstChar == '@')
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int excess))
			{
				return $"Minor Planet Circular (MPC) {100000 + excess}";
			}
		}
		// 2C: # + four Base-62 characters (MPC 110000+)
		if (firstChar == '#')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Circular (MPC) {110000 + value}";
		}
		// 2D: Lowercase letter + four digits (MPS)
		if (char.IsLower(c: firstChar))
		{
			int multiplier = firstChar - 'a';
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int remainder))
			{
				int mpsNumber = (multiplier * 10000) + remainder;
				return $"Minor Planet Supplement (MPS) {mpsNumber}";
			}
		}
		// 2E: Tilde + four Base-62 characters (MPS 260000+)
		if (firstChar == '~')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Supplement (MPS) {260000 + value}";
		}
		// 2F: Single uppercase letter + four digits (various journals)
		if (char.IsUpper(c: firstChar) && compressedRef.Length >= 2 && char.IsDigit(c: compressedRef[index: 1]))
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int number))
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
		}
		// 2G: Two or more letters (various journals)
		if (compressedRef.Length >= 2)
		{
			string journalCode = compressedRef[..2].Trim();
			string remainder = compressedRef.Length > 2 ? compressedRef[2..].Trim() : "";
			// Attempt to get the journal name from the code
			string journalName = GetJournalName(code: journalCode);
			if (!string.IsNullOrEmpty(value: journalName))
			{
				return !string.IsNullOrEmpty(value: remainder) && int.TryParse(s: remainder, result: out int volOrCirc)
					? $"{journalName}, Vol./Circ. {volOrCirc}"
					: journalName;
			}
		}
		// If no known format matches, return the original compressed reference with a note
		return $"Unknown reference format: {compressedRef.Trim()}";
	}

	/// <summary>Decodes a Base-62 encoded string to an integer.</summary>
	/// <param name="encoded">The Base-62 encoded string.</param>
	/// <returns>The decoded integer value.</returns>
	/// <remarks>Uses characters 0-9, A-Z, a-z to represent digits 0-61.</remarks>
	private static int DecodeBase62(string encoded)
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

	/// <summary>Gets the full journal name from a two-letter journal code.</summary>
	/// <param name="code">The two-letter journal code.</param>
	/// <returns>The full journal name, or an empty string if not found.</returns>
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