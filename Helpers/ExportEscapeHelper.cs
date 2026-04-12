using System.Text;

namespace Planetoid_DB.Helpers;

/// <summary>
/// Provides static helper methods for escaping strings in various document formats.
/// </summary>
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
			switch (ch)
			{
				case '\\': builder.Append(value: "\\textbackslash{}"); break;
				case '{': builder.Append(value: "\\{"); break;
				case '}': builder.Append(value: "\\}"); break;
				case '%': builder.Append(value: "\\%"); break;
				case '$': builder.Append(value: "\\$"); break;
				case '&': builder.Append(value: "\\&"); break;
				case '#': builder.Append(value: "\\#"); break;
				case '_': builder.Append(value: "\\_"); break;
				case '^': builder.Append(value: "\\^{}"); break;
				case '~': builder.Append(value: "\\~{}"); break;
				default: builder.Append(value: ch); break;
			}
		}
		// Return the fully escaped string.
		return builder.ToString();
	}

	/// <summary>Escapes Markdown table cell characters.</summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The escaped string suitable for Markdown table output.</returns>
	/// <remarks>In Markdown tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces all occurrences of '|' with '\|', which is the standard way to escape a pipe character in Markdown.</remarks>
	public static string EscapeMarkdownCell(string? value)
	{
		// In Markdown tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content.
		return string.IsNullOrEmpty(value: value) ? string.Empty : value.Replace(oldValue: "|", newValue: "\\|");
	}

	/// <summary>Escapes PostScript string literal characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for PostScript output.</returns>
	/// <remarks>In PostScript string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and parentheses with escaped versions to ensure that the resulting string can be safely included in a PostScript string literal.</remarks>
	public static string EscapePostScript(string? input)
	{
		// In PostScript string literals, the backslash, parentheses, and control characters need to be escaped.
		return string.IsNullOrEmpty(value: input)
			? string.Empty
			: input.Replace(oldValue: "\\", newValue: "\\\\")
				   .Replace(oldValue: "(", newValue: "\\(")
				   .Replace(oldValue: ")", newValue: "\\)");
	}

	/// <summary>Escapes PDF string literal characters.</summary>
	/// <param name="text">The raw input string.</param>
	/// <returns>The escaped string suitable for PDF output.</returns>
	/// <remarks>In PDF string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string. Control characters are escaped using backslash followed by a letter (e.g. \n for newline), while other non-printable characters are escaped using octal escape sequences.</remarks>
	public static string EscapePdf(string? text)
	{
		// In PDF string literals, the backslash, parentheses, and control characters need to be escaped.
		if (string.IsNullOrEmpty(value: text))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(capacity: text.Length);
		foreach (char ch in text)
		{
			// Escape backslash, parentheses, and control characters with a backslash. For other non-printable characters, use octal escape sequences.
			switch (ch)
			{
				case '\\': builder.Append(value: "\\\\"); break;
				case '(': builder.Append(value: "\\("); break;
				case ')': builder.Append(value: "\\)"); break;
				case '\n': builder.Append(value: "\\n"); break;
				case '\r': builder.Append(value: "\\r"); break;
				case '\t': builder.Append(value: "\\t"); break;
				case '\b': builder.Append(value: "\\b"); break;
				case '\f': builder.Append(value: "\\f"); break;
				default:
					if (ch < ' ')
					{
						builder.Append(value: $"\\{(int)ch:000}");
					}
					else
					{
						builder.Append(value: ch);
					}
					break;
			}
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
		// In RTF, the backslash, braces, and control characters need to be escaped. Non-ASCII characters can be represented using Unicode escape sequences.
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		// Use a StringBuilder for efficient string concatenation when escaping characters.
		StringBuilder builder = new(capacity: input.Length);
		foreach (char ch in input)
		{
			// Escape backslash and braces with a backslash. For newlines, use the \par control word. For other non-ASCII characters, use Unicode escape sequences.
			switch (ch)
			{
				case '\\': builder.Append(value: "\\\\"); break;
				case '{': builder.Append(value: "\\{"); break;
				case '}': builder.Append(value: "\\}"); break;
				case '\n': builder.Append(value: "\\par "); break;
				default:
					if (ch > 127)
					{
						builder.Append(value: $"\\u{(int)ch}?");
					}
					else
					{
						builder.Append(value: ch);
					}
					break;
			}
		}
		// Return the fully escaped string.
		return builder.ToString();
	}

	/// <summary>Escapes a CSV field by doubling internal quotes and wrapping in double quotes.</summary>
	/// <param name="field">The raw field value.</param>
	/// <returns>The escaped CSV field suitable for CSV output.</returns>
	/// <remarks>In CSV, fields that contain commas, quotes, or newlines must be enclosed in double quotes, and internal double quotes are escaped by doubling them. This method first checks if the input field is null and treats it as an empty string; then it replaces any internal double quotes with two double quotes to escape them, and finally wraps the entire field in double quotes to ensure it is treated as a single field in the CSV output.</remarks>
	public static string EscapeCsvField(string? field)
	{
		// In CSV, fields that contain commas, quotes, or newlines must be enclosed in double quotes, and internal double quotes are escaped by doubling them.
		string safeField = field ?? string.Empty;
		// First, double any internal double quotes to escape them.
		safeField = safeField.Replace(oldValue: "\"", newValue: "\"\"");
		return $"\"{safeField}\"";
	}

	/// <summary>Escapes a TOML string value.</summary>
	/// <param name="value">The raw value.</param>
	/// <returns>The escaped TOML string value suitable for TOML output.</returns>
	/// <remarks>In TOML, basic string values are enclosed in double quotes, and backslashes and double quotes within the string must be escaped with a backslash. This method checks if the input value is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and double quotes with escaped double quotes to ensure that the resulting string can be safely included as a basic string value in a TOML document.</remarks>
	public static string EscapeToml(string? value)
	{
		// In TOML, basic string values are enclosed in double quotes, and backslashes and double quotes within the string must be escaped with a backslash.
		return string.IsNullOrEmpty(value: value)
			? string.Empty
			: value.Replace(oldValue: "\\", newValue: "\\\\")
				   .Replace(oldValue: "\"", newValue: "\\\"");
	}
}