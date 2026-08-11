/*
 * File:        ExportEscapeHelper.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides static helper methods for escaping strings in various document formats, as well as shared UI feedback methods used by all exporter classes.
 *
 * Autor:       Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Text;

namespace Planetoid_DB;

/// <summary>Provides static helper methods for escaping strings in various document formats, as well as shared UI feedback methods used by all exporter classes.</summary>
/// <remarks>This class contains methods for escaping special characters in LaTeX, Markdown, PostScript, PDF, RTF, CSV, and TOML formats, and shared methods for displaying success and error messages during export operations.</remarks>
public static class ExportEscapeHelper
{
	/// <summary>Escapes LaTeX special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for LaTeX output.</returns>
	/// <remarks>LaTeX special characters that need escaping include: \ { } % $ &amp; # _ ^ ~. This method iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string.</remarks>
	public static string EscapeLatex(string? input)
	{
		// LaTeX special characters that need escaping: \ { } % $ & # _ ^ ~
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(capacity: input.Length);
		// Iterate through each character in the input string and escape special characters as needed.
		foreach (char ch in input)
		{
			_ = ch switch
			{
				'\\' => builder.Append(value: "\\textbackslash{}"),
				'{' => builder.Append(value: "\\{"),
				'}' => builder.Append(value: "\\}"),
				'%' => builder.Append(value: "\\%"),
				'$' => builder.Append(value: "\\$"),
				'&' => builder.Append(value: "\\&"),
				'#' => builder.Append(value: "\\#"),
				'_' => builder.Append(value: "\\_"),
				'^' => builder.Append(value: "\\^{}"),
				'~' => builder.Append(value: "\\~{}"),
				_ => builder.Append(value: ch),
			};
		}
		// Return the fully escaped string.
		return builder.ToString();
	}

	/// <summary>Escapes Markdown table cell characters.</summary>
	/// <param name="input">The raw cell value.</param>
	/// <returns>The escaped string suitable for Markdown table output.</returns>
	/// <remarks>In Markdown tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces all occurrences of '|' with '\|', which is the standard way to escape a pipe character in Markdown.</remarks>
	public static string EscapeMarkdownCell(string? input)
	{
		// In Markdown tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content.
		return string.IsNullOrEmpty(value: input) ? string.Empty : input.Replace(oldValue: "|", newValue: "\\|");
	}

	/// <summary>Escapes Typst table cell characters.</summary>
	/// <param name="input">The raw cell value.</param>
	/// <returns>The escaped string suitable for Typst table output.</returns>
	/// <remarks>In Typst tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces all occurrences of '|' with '\|'.</remarks>
	public static string EscapeTypstCell(string? input) => EscapeMarkdownCell(input: input);

	/// <summary>Escapes PostScript string literal characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for PostScript output.</returns>
	/// <remarks>In PostScript string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and parentheses with escaped versions to ensure that the resulting string can be safely included in a PostScript string literal.</remarks>
	public static string EscapePostScript(string? input)
	{
		// If the input string is null or empty, return an empty string to avoid processing.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Initialize a StringBuilder with an estimated capacity to reduce memory allocations.
		StringBuilder builder = new(input.Length + 5);
		// Iterate through each character in the input string and escape special characters as needed.
		foreach (char ch in input)
		{
			// Escape backslash and parentheses with a backslash. For other characters, append them as-is.
			_ = ch switch
			{
				'\\' => builder.Append(value: "\\\\"),
				'(' => builder.Append(value: "\\("),
				')' => builder.Append(value: "\\)"),
				_ => builder.Append(value: ch),
			};
		}
		// Return the fully escaped string.
		return builder.ToString();

	}

	/// <summary>Escapes PDF string literal characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for PDF output.</returns>
	/// <remarks>In PDF string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string. Control characters are escaped using backslash followed by a letter (e.g. \n for newline), while other non-printable characters are escaped using octal escape sequences.</remarks>
	public static string EscapePdf(string? input)
	{
		// In PDF string literals, the backslash, parentheses, and control characters need to be escaped.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(capacity: input.Length + 10);
		foreach (char ch in input)
		{
			// Escape backslash, parentheses, and control characters with a backslash. For other non-printable characters, use octal escape sequences.
			_ = ch switch
			{
				'\\' => builder.Append(value: "\\\\"),
				'(' => builder.Append(value: "\\("),
				')' => builder.Append(value: "\\)"),
				'\n' => builder.Append(value: "\\n"),
				'\r' => builder.Append(value: "\\r"),
				'\t' => builder.Append(value: "\\t"),
				'\b' => builder.Append(value: "\\b"),
				'\f' => builder.Append(value: "\\f"),
				_ => ch < ' ' ? builder.Append(value: $"\\{(int)ch:000}") : builder.Append(value: ch),
			};
		}
		// Return the fully escaped string.
		return builder.ToString();
	}

	/// <summary>Escapes RTF special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for RTF output.</returns>
	/// <remarks>In RTF, the backslash, braces, and control characters need to be escaped. Non-ASCII characters can be represented using Unicode escape sequences. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string. Backslashes and braces are escaped with a preceding backslash, newlines are replaced with the \par control word, and non-ASCII characters are represented using \uN? where N is the Unicode code point of the character.</remarks>
	public static string EscapeRtf(string? input)
	{
		// If the input string is null or empty, return an empty string to avoid processing.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(capacity: input.Length + 10);
		foreach (char ch in input)
		{
			// Escape backslash and braces with a backslash. For newlines, use the \par control word. For other non-ASCII characters, use Unicode escape sequences.
			switch (ch)
			{
				case '\\': _ = builder.Append(value: "\\\\"); break;
				case '{': _ = builder.Append(value: "\\{"); break;
				case '}': _ = builder.Append(value: "\\}"); break;
				// Ignore carriage return characters, as they are handled by the newline case; Bugfix: Prevents \r\par with Windows line breaks.
				case '\r': break;
				case '\n': _ = builder.Append(value: "\\par "); break;
				default:
					_ = ch > 127 ? builder.Append(value: $"\\u{(int)ch}?") : builder.Append(value: ch);
					break;
			}
		}
		// Return the fully escaped string.
		return builder.ToString();
	}

	/// <summary>Escapes a CSV field by doubling internal quotes and wrapping in double quotes.</summary>
	/// <param name="input">The raw field value.</param>
	/// <returns>The escaped CSV field suitable for CSV output.</returns>
	/// <remarks>In CSV, fields that contain commas, quotes, or newlines must be enclosed in double quotes, and internal double quotes are escaped by doubling them. This method first checks if the input field is null and treats it as an empty string; then it replaces any internal double quotes with two double quotes to escape them, and finally wraps the entire field in double quotes to ensure it is treated as a single field in the CSV output.</remarks>
	public static string EscapeCsvField(string? input)
	{
		// If the input string is null or empty, return an empty string to avoid processing.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Replace any internal double quotes with two double quotes to escape them, and wrap the entire field in double quotes.
		return $"\"{input.Replace("\"", "\"\"")}\"";

	}

	/// <summary>Escapes a TOML string value.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped TOML string value suitable for TOML output.</returns>
	/// <remarks>In TOML, basic string values are enclosed in double quotes, and backslashes and double quotes within the string must be escaped with a backslash. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and double quotes with escaped double quotes to ensure that the resulting string can be safely included as a basic string value in a TOML document.</remarks>
	public static string EscapeToml(string? input)
	{
		// If the input string is null or empty, return an empty string to avoid processing.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(input.Length + 5);
		// Iterate through each character in the input string and escape backslashes and double quotes with a backslash. For other characters, append them as-is.
		foreach (char ch in input)
		{
			_ = ch switch
			{
				'\\' => builder.Append(value: "\\\\"),
				'\"' => builder.Append(value: "\\\""),
				_ => builder.Append(value: ch),
			};
		}
		// Return the fully escaped string.
		return builder.ToString();
	}
}