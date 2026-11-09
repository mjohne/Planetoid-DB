/*
 * File:        DateFormatterExtensions.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods to format date strings in "YYYYMMDD" format.
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

/// <summary>Provides extension methods and helper functions for formatting date strings in "YYYYMMDD" format.</summary>
/// <remarks>This class provides extension methods for converting date strings in "YYYYMMDD" format into culture-specific date representations.</remarks>
internal static class DateFormatterExtensions
{
	/// <summary>The exact date format pattern, `yyyyMMdd`.</summary>
	/// <remarks>This constant is used to specify the exact format for parsing date strings in "YYYYMMDD" format.</remarks>
	private const string ExactDateFormat = "yyyyMMdd";

	#region Extension Methods (Throwing)

	/// <summary>Converts a string in "YYYYMMDD" format into a date representation based on a specific culture or the current system culture.</summary>
	/// <param name="input">The date string in "YYYYMMDD" format (e.g., "20261029").</param>
	/// <param name="targetCulture">Optional: The target culture. If null, CultureInfo.CurrentCulture is used.</param>
	/// <param name="useLongFormat">If true, the long format ("D") is used, otherwise the short format ("d").</param>
	/// <returns>The formatted date string.</returns>
	/// <exception cref="ArgumentException">Wird geworfen, wenn der Input-String ungültig oder null ist.</exception>
	public static string ToCultureDate(this string input, CultureInfo? targetCulture = null, bool useLongFormat = false)
	{
		ArgumentException.ThrowIfNullOrEmpty(argument: input);
		// Use the TryToCultureDate method to attempt the conversion and throw an exception if it fails
		return TryToCultureDate(input: input, formattedDate: out string result, targetCulture: targetCulture, useLongFormat: useLongFormat) ? result : throw new ArgumentException(message: $"The provided value '{input}' is not a valid date string in the format 'YYYYMMDD'.", paramName: nameof(input));
	}

	#endregion

	#region Extension Methods (Try-Pattern)

	/// <summary>Tries to convert a string in "YYYYMMDD" format into a date representation based on a specific culture or the current system culture.</summary>
	/// <param name="input">The date string in "YYYYMMDD" format.</param>
	/// <param name="formattedDate">The resulting date string or string.Empty on error.</param>
	/// <param name="targetCulture">Optional: The target culture. If null, CultureInfo.CurrentCulture is used.</param>
	/// <param name="useLongFormat">If true, the long format ("D") is used, otherwise the short format ("d").</param>
	/// <returns>True, wenn das Parsen und Formatieren erfolgreich war, sonst False.</returns>
	/// <remarks>This method attempts to parse the input string as a DateOnly using the exact format "yyyyMMdd" and formats it according to the specified culture and format specifier.</remarks>
	public static bool TryToCultureDate(this string input, out string formattedDate, CultureInfo? targetCulture = null, bool useLongFormat = false)
	{
		ArgumentException.ThrowIfNullOrEmpty(argument: input);
		// Validate input
		CultureInfo culture = targetCulture ?? CultureInfo.CurrentCulture;
		// Determine the format specifier based on the useLongFormat parameter
		string formatSpecifier = useLongFormat ? "D" : "d";
		// Attempt to parse the input string as a DateOnly using the exact format "yyyyMMdd"
		if (DateOnly.TryParseExact(s: input, format: ExactDateFormat, provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None, result: out DateOnly parsedDate))
		{
			// If parsing is successful, format the parsed date using the specified culture and format specifier
			formattedDate = parsedDate.ToString(format: formatSpecifier, provider: culture);
			return true;
		}
		// If parsing fails, set formattedDate to string.Empty and return false
		formattedDate = string.Empty;
		return false;
	}

	#endregion
}
