/*
 * File:        ExportEscapeHelperTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for the export escape helper.
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

using System.Diagnostics;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="ExportEscapeHelper"/>.</summary>
/// <remarks>These tests verify that the various escape methods correctly handle special characters, control characters, and null or empty inputs.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class ExportEscapeHelperTests
{
	/// <summary>Verifies LaTeX special characters are escaped.</summary>
	/// <remarks>The LaTeX escape sequence for a backslash is \textbackslash{}, for a tilde is \~{}, and for a caret is \^{}.</remarks>
	[Fact]
	public void EscapeLatexEscapesSpecialCharacters()
	{
		// The LaTeX escape sequence for a backslash is \textbackslash{}, for a tilde is \~{}, and for a caret is \^{}.
		Assert.Equal(expected: "\\textbackslash{}\\{\\}\\%\\$\\&\\#\\_\\^{}\\~{}a", actual: ExportEscapeHelper.EscapeLatex(input: "\\{}%$&#_^~a"));
	}

	/// <summary>Verifies Markdown and Typst cells escape the pipe character.</summary>
	/// <remarks>In Markdown and Typst, the pipe character is used to separate table cells, so it must be escaped with a backslash.</remarks>
	[Fact]
	public void EscapeMarkdownAndTypstCellEscapePipes()
	{
		// The pipe character is used to separate table cells in Markdown and Typst, so it must be escaped with a backslash.
		Assert.Equal(expected: "a\\|b", actual: ExportEscapeHelper.EscapeMarkdownCell(input: "a|b"));
		Assert.Equal(expected: "a\\|b", actual: ExportEscapeHelper.EscapeTypstCell(input: "a|b"));
	}

	/// <summary>Verifies PostScript string literal characters are escaped.</summary>
	/// <remarks>In PostScript, the backslash and parentheses are special characters in string literals, so they must be escaped with a backslash.</remarks>
	[Fact]
	public void EscapePostScriptEscapesBackslashAndParentheses()
	{
		// In PostScript, the backslash and parentheses are special characters in string literals, so they must be escaped with a backslash.
		Assert.Equal(expected: "\\\\\\(x\\)", actual: ExportEscapeHelper.EscapePostScript(input: "\\(x)"));
	}

	/// <summary>Verifies PDF string literal characters and control characters are escaped.</summary>
	/// <remarks>In PDF, the backslash and parentheses are special characters in string literals, and control characters (ASCII 0-31) must be escaped using octal notation.</remarks>
	[Fact]
	public void EscapePdfEscapesSpecialAndControlCharacters()
	{
		// In PDF, the backslash and parentheses are special characters in string literals, and control characters (ASCII 0-31) must be escaped using octal notation.
		Assert.Equal(expected: "\\(\\)\\\\\\n\\r\\t\\001", actual: ExportEscapeHelper.EscapePdf(input: "()\\\n\r\t\u0001"));
	}

	/// <summary>Verifies RTF syntax, line breaks and non-ASCII characters are escaped.</summary>
	/// <remarks>In RTF, the backslash, braces, and certain control words are special characters, line breaks are represented by \par, and non-ASCII characters are represented using \uN? where N is the Unicode code point.</remarks>
	[Fact]
	public void EscapeRtfEscapesSyntaxLineBreaksAndUnicode()
	{
		// In RTF, the backslash, braces, and certain control words are special characters, line breaks are represented by \par, and non-ASCII characters are represented using \uN? where N is the Unicode code point.
		Assert.Equal(expected: "\\{\\}\\\\a\\par b\\u252?", actual: ExportEscapeHelper.EscapeRtf(input: "{}\\a\r\nb\u00fc"));
	}

	/// <summary>Verifies surrogate pairs are emitted as two signed RTF Unicode escapes.</summary>
	/// <remarks>In RTF, surrogate pairs (characters outside the Basic Multilingual Plane) are represented as two signed 16-bit Unicode escapes.</remarks>
	[Fact]
	public void EscapeRtfEscapesSurrogatePairs()
	{
		// In RTF, surrogate pairs (characters outside the Basic Multilingual Plane) are represented as two signed 16-bit Unicode escapes.
		Assert.Equal(expected: "\\u-10179?\\u-8704?", actual: ExportEscapeHelper.EscapeRtf(input: "\U0001F600"));
	}

	/// <summary>Verifies CSV fields are quoted and inner quotes are doubled.</summary>
	/// <remarks>In CSV, fields containing commas or quotes must be enclosed in double quotes, and inner double quotes must be escaped by doubling them.</remarks>
	[Fact]
	public void EscapeCsvFieldQuotesAndDoublesQuotes()
	{
		// In CSV, fields containing commas or quotes must be enclosed in double quotes, and inner double quotes must be escaped by doubling them.
		Assert.Equal(expected: "\"a\"\"b,c\"", actual: ExportEscapeHelper.EscapeCsvField(input: "a\"b,c"));
		Assert.Equal(expected: "\"\"", actual: ExportEscapeHelper.EscapeCsvField(input: null));
	}

	/// <summary>Verifies TOML strings escape backslashes and quotes.</summary>
	/// <remarks>In TOML, backslashes and double quotes are special characters in string literals and must be escaped with a backslash.</remarks>
	[Fact]
	public void EscapeTomlEscapesBackslashAndQuotes()
	{
		// In TOML, backslashes and double quotes are special characters in string literals and must be escaped with a backslash.
		Assert.Equal(expected: "\\\\\\\"", actual: ExportEscapeHelper.EscapeToml(input: "\\\""));
	}

	/// <summary>Verifies null and empty inputs return an empty string.</summary>
	/// <remarks>If the input is null or an empty string, the escape methods should return an empty string.</remarks>
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void EscapeMethodsReturnEmptyForNullOrEmpty(string? input)
	{
		// All escape methods should return an empty string for null or empty input.
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeLatex(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeMarkdownCell(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapePostScript(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapePdf(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeRtf(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeToml(input: input));
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
