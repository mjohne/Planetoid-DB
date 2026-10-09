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

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="ExportEscapeHelper"/>.</summary>
public sealed class ExportEscapeHelperTests
{
	/// <summary>Verifies LaTeX special characters are escaped.</summary>
	[Fact]
	public void EscapeLatex_EscapesSpecialCharacters()
	{
		Assert.Equal(expected: "\\textbackslash{}\\{\\}\\%\\$\\&\\#\\_\\^{}\\~{}a", actual: ExportEscapeHelper.EscapeLatex(input: "\\{}%$&#_^~a"));
	}

	/// <summary>Verifies Markdown and Typst cells escape the pipe character.</summary>
	[Fact]
	public void EscapeMarkdownAndTypstCell_EscapePipes()
	{
		Assert.Equal(expected: "a\\|b", actual: ExportEscapeHelper.EscapeMarkdownCell(input: "a|b"));
		Assert.Equal(expected: "a\\|b", actual: ExportEscapeHelper.EscapeTypstCell(input: "a|b"));
	}

	/// <summary>Verifies PostScript string literal characters are escaped.</summary>
	[Fact]
	public void EscapePostScript_EscapesBackslashAndParentheses()
	{
		Assert.Equal(expected: "\\\\\\(x\\)", actual: ExportEscapeHelper.EscapePostScript(input: "\\(x)"));
	}

	/// <summary>Verifies PDF string literal characters and control characters are escaped.</summary>
	[Fact]
	public void EscapePdf_EscapesSpecialAndControlCharacters()
	{
		Assert.Equal(expected: "\\(\\)\\\\\\n\\r\\t\\001", actual: ExportEscapeHelper.EscapePdf(input: "()\\\n\r\t\u0001"));
	}

	/// <summary>Verifies RTF syntax, line breaks and non-ASCII characters are escaped.</summary>
	[Fact]
	public void EscapeRtf_EscapesSyntaxLineBreaksAndUnicode()
	{
		Assert.Equal(expected: "\\{\\}\\\\a\\par b\\u252?", actual: ExportEscapeHelper.EscapeRtf(input: "{}\\a\r\nb\u00fc"));
	}

	/// <summary>Verifies surrogate pairs are emitted as two signed RTF Unicode escapes.</summary>
	[Fact]
	public void EscapeRtf_EscapesSurrogatePairs()
	{
		Assert.Equal(expected: "\\u-10179?\\u-8704?", actual: ExportEscapeHelper.EscapeRtf(input: "\U0001F600"));
	}

	/// <summary>Verifies CSV fields are quoted and inner quotes are doubled.</summary>
	[Fact]
	public void EscapeCsvField_QuotesAndDoublesQuotes()
	{
		Assert.Equal(expected: "\"a\"\"b,c\"", actual: ExportEscapeHelper.EscapeCsvField(input: "a\"b,c"));
		Assert.Equal(expected: "\"\"", actual: ExportEscapeHelper.EscapeCsvField(input: null));
	}

	/// <summary>Verifies TOML strings escape backslashes and quotes.</summary>
	[Fact]
	public void EscapeToml_EscapesBackslashAndQuotes()
	{
		Assert.Equal(expected: "\\\\\\\"", actual: ExportEscapeHelper.EscapeToml(input: "\\\""));
	}

	/// <summary>Verifies null and empty inputs return an empty string.</summary>
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void EscapeMethods_ReturnEmptyForNullOrEmpty(string? input)
	{
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeLatex(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeMarkdownCell(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapePostScript(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapePdf(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeRtf(input: input));
		Assert.Equal(expected: string.Empty, actual: ExportEscapeHelper.EscapeToml(input: input));
	}
}
