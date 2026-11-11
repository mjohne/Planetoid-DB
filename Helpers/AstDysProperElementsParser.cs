/*
 * File:        AstDysProperElementsParser.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides high-performance parsing and dictionary indexing for AstDyS proper element catalog files.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods to parse, validate, and index AstDyS proper element catalog files (<c>allnum.prop</c>, <c>all.prop</c>, <c>cat.pro</c>).</summary>
internal static class AstDysProperElementsParser
{
	/// <summary>Parses a single text line from an AstDyS proper elements export file using high-performance span-based parsing.</summary>
	/// <param name="line">The text line to parse.</param>
	/// <param name="element">When this method returns, contains the parsed <see cref="ProperOrbitalElement"/> if successful; otherwise, default.</param>
	/// <returns><c>true</c> if the line was successfully parsed; otherwise, <c>false</c>.</returns>
	public static bool TryParseLine(string line, out ProperOrbitalElement element)
	{
		element = default;
		if (string.IsNullOrWhiteSpace(value: line))
		{
			return false;
		}
		ReadOnlySpan<char> span = line.AsSpan().Trim();
		// Skip comments, header lines, and column dividers
		if (span.IsEmpty || span.StartsWith(value: "!") || span.StartsWith(value: "designation", comparisonType: StringComparison.OrdinalIgnoreCase) || span.StartsWith(value: "name", comparisonType: StringComparison.OrdinalIgnoreCase) || span.StartsWith(value: "#"))
		{
			return false;
		}
		// Tokenize space/tab delimited fields
		string[] tokens = line.Split(separator: [' ', '\t'], options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		// AstDyS .prop files contain at least 7 fields: Designation, a_p, e_p, sin(i_p), g, s, n
		if (tokens.Length < 7)
		{
			return false;
		}
		CultureInfo culture = CultureInfo.InvariantCulture;
		try
		{
			// Clean designation token (strip surrounding quotes if present)
			string designation = tokens[0].Trim(trimChars: ['\'', '\"']);

			if (!double.TryParse(s: tokens[1], style: NumberStyles.Float, provider: culture, result: out double a_p) ||
				!double.TryParse(s: tokens[2], style: NumberStyles.Float, provider: culture, result: out double e_p) ||
				!double.TryParse(s: tokens[3], style: NumberStyles.Float, provider: culture, result: out double sin_i_p) ||
				!double.TryParse(s: tokens[4], style: NumberStyles.Float, provider: culture, result: out double g) ||
				!double.TryParse(s: tokens[5], style: NumberStyles.Float, provider: culture, result: out double s) ||
				!double.TryParse(s: tokens[6], style: NumberStyles.Float, provider: culture, result: out double n))
			{
				return false;
			}
			// Optional 8th field: Lyapunov exponent
			double lce = 0.0;
			if (tokens.Length >= 8)
			{
				_ = double.TryParse(s: tokens[7], style: NumberStyles.Float, provider: culture, result: out lce);
			}
			double i_p_deg = ProperOrbitalElement.ConvertSinInclinationToDegrees(sinInclination: sin_i_p);
			element = new ProperOrbitalElement(Designation: designation, ProperSemiMajorAxisAu: a_p, ProperEccentricity: e_p, ProperSinInclination: sin_i_p, ProperInclinationDegrees: i_p_deg, PerihelionFrequencyG: g, NodalFrequencyS: s, MeanMotionN: n, LyapunovExponent: lce);
			return true;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>Reads an AstDyS catalog file asynchronously and returns a dictionary indexed by designation for O(1) lookup.</summary>
	/// <param name="filePath">The full file path to the AstDyS catalog file.</param>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>A dictionary containing designation keys mapped to <see cref="ProperOrbitalElement"/> instances.</returns>
	public static async Task<Dictionary<string, ProperOrbitalElement>> ReadFileToDictionaryAsync(string filePath, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: filePath);
		if (!File.Exists(path: filePath))
		{
			throw new FileNotFoundException(message: $"AstDyS file not found: {filePath}", fileName: filePath);
		}
		Dictionary<string, ProperOrbitalElement> dictionary = new(capacity: 500_000, comparer: StringComparer.OrdinalIgnoreCase);
		using FileStream stream = new(path: filePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 65_536, useAsync: true);
		using StreamReader reader = new(stream: stream);
		string? line;
		while ((line = await reader.ReadLineAsync(cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) != null)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (TryParseLine(line: line, element: out ProperOrbitalElement element))
			{
				_ = dictionary.TryAdd(key: element.Designation, value: element);
			}
		}
		return dictionary;
	}

	/// <summary>Compares osculating orbital elements against proper orbital elements to measure secular displacement.</summary>
	/// <param name="osculatingSemiMajorAxis">The osculating semi-major axis a in AU.</param>
	/// <param name="osculatingEccentricity">The osculating numerical eccentricity e.</param>
	/// <param name="osculatingInclinationDegrees">The osculating inclination i in degrees.</param>
	/// <param name="properElement">The reference proper element record.</param>
	/// <returns>A tuple containing (DeltaA, DeltaE, DeltaI_Degrees) absolute differences.</returns>
	public static (double DeltaA, double DeltaE, double DeltaI_Degrees) CalculateSecularDisplacement(double osculatingSemiMajorAxis, double osculatingEccentricity, double osculatingInclinationDegrees, ProperOrbitalElement properElement)
	{
		double deltaA = Math.Abs(value: osculatingSemiMajorAxis - properElement.ProperSemiMajorAxisAu);
		double deltaE = Math.Abs(value: osculatingEccentricity - properElement.ProperEccentricity);
		double deltaI = Math.Abs(value: osculatingInclinationDegrees - properElement.ProperInclinationDegrees);
		return (DeltaA: deltaA, DeltaE: deltaE, DeltaI_Degrees: deltaI);
	}
}
