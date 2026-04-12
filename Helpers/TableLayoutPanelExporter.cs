// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using System.Data.SQLite;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods for saving the contents of a <see cref="TableLayoutPanel"/> to various file formats.</summary>
/// <remarks>Each method accepts the source <see cref="TableLayoutPanel"/>, a document title used in the file content,
/// and the full file-system path of the output file. Column headers are read from the controls in the first row (row 0);
/// row data is read from controls in subsequent rows. Compressed file formats
/// (DOCX, ODT, ODS, XLSX, EPUB) are written as proper ZIP archives rather than flat XML files.
/// SQLite export requires System.Data.SQLite; CHM export requires Microsoft HTML Help Workshop (hhc.exe).</remarks>
public static class TableLayoutPanelExporter
{
	/// <summary>NLog logger for logging messages and errors.</summary>
	/// <remarks>Using NLog allows for flexible logging configuration and supports various log targets such as files, console, etc. The logger is initialized for the current class to capture context in log messages.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Reusable JSON serializer options for efficient serialization.</summary>
	/// <remarks>Creating a static instance of JsonSerializerOptions with WriteIndented set to true allows for consistent formatting of JSON output across all methods that serialize to JSON, while avoiding the overhead of creating new options instances for each serialization operation.</remarks>
	private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

	#region helpers

	/// <summary>Returns the column header texts of the given <see cref="TableLayoutPanel"/> (read from row 0).</summary>
	/// <param name="tableLayoutPanel">The table layout panel whose first row contains headers.</param>
	/// <returns>An array of header strings in column order.</returns>
	/// <remarks>Reads the text of the control at each column position in row 0. If a cell in row 0 has no control, an empty string is used as the header for that column.</remarks>
	private static string[] GetHeaders(TableLayoutPanel tableLayoutPanel)
	{
		string[] headers = new string[tableLayoutPanel.ColumnCount];
		for (int c = 0; c < tableLayoutPanel.ColumnCount; c++)
		{
			headers[c] = tableLayoutPanel.GetControlFromPosition(column: c, row: 0)?.Text ?? string.Empty;
		}
		return headers;
	}

	/// <summary>Enumerates each data row of the <see cref="TableLayoutPanel"/> as an array of cell strings.</summary>
	/// <param name="tableLayoutPanel">The table layout panel to read from. Row 0 is treated as the header row and is skipped.</param>
	/// <returns>An enumerable of string arrays, one per data row.</returns>
	/// <remarks>Reads the text of the control at each cell position. Cells with no control yield an empty string.</remarks>
	private static IEnumerable<string[]> GetRows(TableLayoutPanel tableLayoutPanel)
	{
		// Row 0 is the header row; data rows start at index 1.
		for (int r = 1; r < tableLayoutPanel.RowCount; r++)
		{
			// For each data row, read the text of the control at each column position.
			string[] row = new string[tableLayoutPanel.ColumnCount];
			for (int c = 0; c < tableLayoutPanel.ColumnCount; c++)
			{
				// GetControlFromPosition returns the control at the given cell, or null if no control is present.
				row[c] = tableLayoutPanel.GetControlFromPosition(column: c, row: r)?.Text ?? string.Empty;
			}
			// Yield the row array to the caller.
			yield return row;
		}
	}

	/// <summary>Escapes LaTeX special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for LaTeX output.</returns>
	/// <remarks>LaTeX special characters that need escaping include: \ { } % $ amp # _ ^ ~. This method iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string.</remarks>
	private static string EscapeLatex(string? input) => ExportEscapeHelper.EscapeLatex(input);

	/// <summary>Escapes Markdown table cell characters.</summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The escaped string suitable for Markdown table output.</returns>
	/// <remarks>In Markdown tables, the pipe character '|' is used as a column separator, so it must be escaped if it appears in cell content. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces all occurrences of '|' with '\|', which is the standard way to escape a pipe character in Markdown.</remarks>
	private static string EscapeMarkdownCell(string? value) => ExportEscapeHelper.EscapeMarkdownCell(value);

	/// <summary>Escapes PostScript string literal characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for PostScript output.</returns>
	/// <remarks>In PostScript string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and parentheses with escaped versions to ensure that the resulting string can be safely included in a PostScript string literal.</remarks>
	private static string EscapePostScript(string? input) => ExportEscapeHelper.EscapePostScript(input);

	/// <summary>Escapes PDF string literal characters.</summary>
	/// <param name="text">The raw input string.</param>
	/// <returns>The escaped string suitable for PDF output.</returns>
	/// <remarks>In PDF string literals, the backslash, parentheses, and control characters need to be escaped. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string. Control characters are escaped using backslash followed by a letter (e.g. \n for newline), while other non-printable characters are escaped using octal escape sequences.</remarks>
	private static string EscapePdf(string? text) => ExportEscapeHelper.EscapePdf(text);

	/// <summary>Escapes RTF special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for RTF output.</returns>
	/// <remarks>In RTF, the backslash, braces, and control characters need to be escaped. Non-ASCII characters can be represented using Unicode escape sequences. This method checks if the input string is null or empty and returns an empty string in that case; otherwise, it iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string. Backslashes and braces are escaped with a preceding backslash, newlines are replaced with the \par control word, and non-ASCII characters are represented using \uN? where N is the Unicode code point of the character.</remarks>
	private static string EscapeRtf(string? input) => ExportEscapeHelper.EscapeRtf(input);

	/// <summary>Escapes a CSV field by doubling internal quotes and wrapping in double quotes.</summary>
	/// <param name="field">The raw field value.</param>
	/// <returns>The escaped CSV field suitable for CSV output.</returns>
	/// <remarks>In CSV, fields that contain commas, quotes, or newlines must be enclosed in double quotes, and internal double quotes are escaped by doubling them. This method first checks if the input field is null and treats it as an empty string; then it replaces any internal double quotes with two double quotes to escape them, and finally wraps the entire field in double quotes to ensure it is treated as a single field in the CSV output.</remarks>
	private static string EscapeCsvField(string? field) => ExportEscapeHelper.EscapeCsvField(field);

	/// <summary>Escapes a TOML string value.</summary>
	/// <param name="value">The raw value.</param>
	/// <returns>The escaped TOML string value suitable for TOML output.</returns>
	/// <remarks>In TOML, basic string values are enclosed in double quotes, and backslashes and double quotes within the string must be escaped with a backslash. This method checks if the input value is null or empty and returns an empty string in that case; otherwise, it replaces backslashes with double backslashes and double quotes with escaped double quotes to ensure that the resulting string can be safely included as a basic string value in a TOML document.</remarks>
	private static string EscapeToml(string? value) => ExportEscapeHelper.EscapeToml(value);

	/// <summary>Shows a success message after a file has been saved.</summary>
	/// <remarks>Logs the successful save operation at the Info level and displays a message box to the user.</remarks>
	private static void ShowSuccess()
	{
		// Log the successful save operation at the Info level.
		_ = MessageBox.Show(
			text: I18nStrings.FileSavedSuccessfully,
			caption: I18nStrings.InformationCaption,
			buttons: MessageBoxButtons.OK,
			icon: MessageBoxIcon.Information);
	}

	/// <summary>Logs and shows an error that occurred while saving a file.</summary>
	/// <param name="ex">The exception.</param>
	/// <param name="format">A label identifying the file format (e.g. "Text").</param>
	/// <param name="filePath">The target file path.</param>
	/// <remarks>Logs the error with details about the format and file path, and displays an error message box to the user with the exception message.</remarks>
	private static void ShowError(Exception ex, string format, string filePath)
	{
		// Log the error with details about the format and file path.
		logger.Error(exception: ex, message: $"Error saving as {format} to '{{FilePath}}'.", args: filePath);
		_ = MessageBox.Show(
			text: $"Error saving as {format}: {ex.Message}",
			caption: I18nStrings.ErrorCaption,
			buttons: MessageBoxButtons.OK,
			icon: MessageBoxIcon.Error);
	}

	#endregion

	#region Save methods
	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a plain-text file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a heading at the top of the file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The output is a simple tab-delimited text file with the title as the first line, a separator line of dashes, column headers, and then one line per row of data. No special escaping is performed, so tabs in the data may cause misalignment.</remarks>
	public static void SaveAsText(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and write the output file with UTF-8 encoding. The first line is the title, followed by a separator line, then the headers, and then each row of data as tab-delimited values.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the first line, followed by a separator line of dashes, then the column headers, and then each row of data as tab-delimited values.
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '-', count: title.Length));
			writer.WriteLine(value: string.Join(separator: " ", values: headers));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each row of data as tab-delimited values. Note that if any cell contains a tab character, it will cause misalignment in the output since no escaping is performed.
				writer.WriteLine(value: string.Join(separator: " ", values: row));
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Text", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a LaTeX document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title used in the LaTeX caption.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The output is a complete LaTeX document with a table containing the TableLayoutPanel data. Special characters in the data are escaped for LaTeX. The document uses the 'article' class and includes a caption with the provided title.</remarks>
	public static void SaveAsLatex(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and write the output file with UTF-8 encoding. The output is a complete LaTeX document with a table containing the TableLayoutPanel data. Special characters in the data are escaped for LaTeX.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Construct the column specification string for the LaTeX tabular environment, using 'l' (left-aligned) for each column and separating with '|'.
			string colSpec = string.Join(separator: "|", values: Enumerable.Repeat(element: "l", count: headers.Length));
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the LaTeX document preamble, including the document class and input encoding. Then write the table environment with the column specifications, headers, and rows of data. Finally, write the caption using the provided title and end the document.
			writer.WriteLine(value: "\\documentclass{article}");
			writer.WriteLine(value: "\\usepackage[utf8]{inputenc}");
			writer.WriteLine(value: "\\begin{document}");
			writer.WriteLine(value: "\\begin{table}[h!]");
			writer.WriteLine(value: "\\centering");
			writer.WriteLine(value: $"\\begin{{tabular}}{{|{colSpec}|}}");
			writer.WriteLine(value: "\\hline");
			writer.WriteLine(value: string.Join(separator: " & ", values: headers.Select(selector: EscapeLatex)) + " \\\\");
			writer.WriteLine(value: "\\hline");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Escape special characters in each cell for LaTeX and write the row with ' & ' as the column separator and '\\' at the end of the line to indicate a new row in the tabular environment.
				writer.WriteLine(value: string.Join(separator: " & ", values: row.Select(selector: EscapeLatex)) + " \\\\");
			}
			writer.WriteLine(value: "\\hline");
			writer.WriteLine(value: "\\end{tabular}");
			writer.WriteLine(value: $"\\caption{{{EscapeLatex(input: title)}}}");
			writer.WriteLine(value: "\\end{table}");
			writer.WriteLine(value: "\\end{document}");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "LaTeX", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Markdown table.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The output is a Markdown-formatted text file with the title as a level-1 heading, followed by a table containing the TableLayoutPanel data. Special characters in the cell data are escaped to prevent breaking the Markdown syntax.</remarks>
	public static void SaveAsMarkdown(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and write the output file with UTF-8 encoding. The output is a Markdown-formatted text file with the title as a level-1 heading, followed by a table containing the TableLayoutPanel data. Special characters in the cell data are escaped to prevent breaking the Markdown syntax.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a level-1 heading, followed by an empty line, then the Markdown table header row, the separator row, and then each row of data with cells escaped for Markdown.
			writer.WriteLine(value: $"# {title}");
			writer.WriteLine();
			writer.WriteLine(value: "| " + string.Join(separator: " | ", values: headers) + " |");
			writer.WriteLine(value: "| " + string.Join(separator: " | ", values: headers.Select(selector: static _ => ":---")) + " |");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Escape pipe characters in the cell data to prevent breaking the Markdown table syntax, since '|' is used as a column separator. The escaping is done by replacing '|' with '\|', which is the standard way to escape a pipe in Markdown.
				writer.WriteLine(value: "| " + string.Join(separator: " | ", values: row.Select(selector: EscapeMarkdownCell)) + " |");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Markdown", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an AsciiDoc document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as the first-level heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The output is an AsciiDoc-formatted text file with the title as a first-level heading, followed by a table containing the TableLayoutPanel data. The table uses the 'header' option to indicate that the first row contains column headers. Special characters in the cell data are escaped to prevent breaking the AsciiDoc syntax.</remarks>
	public static void SaveAsAsciiDoc(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and write the output file with UTF-8 encoding. The output is an AsciiDoc-formatted text file with the title as a first-level heading, followed by a table containing the TableLayoutPanel data. The table uses the 'header' option to indicate that the first row contains column headers. Special characters in the cell data are escaped to prevent breaking the AsciiDoc syntax.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a first-level heading, followed by an empty line, then the AsciiDoc table with the 'header' option, the column headers, and then each row of data with cells escaped for AsciiDoc.
			writer.WriteLine(value: $"= {title}");
			writer.WriteLine();
			writer.WriteLine(value: "[options=\"header\"]");
			writer.WriteLine(value: "|===");
			writer.WriteLine(value: "|" + string.Join(separator: "|", values: headers));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Escape pipe characters in the cell data to prevent breaking the AsciiDoc table syntax, since '|' is used as a column separator. The escaping is done by replacing '|' with '\|', which is the standard way to escape a pipe in AsciiDoc.
				string[] escaped = [.. row.Select(selector: static v => v.Replace(oldValue: "|", newValue: "\\|"))];
				writer.WriteLine(value: "|" + string.Join(separator: "|", values: escaped));
			}
			writer.WriteLine(value: "|===");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AsciiDoc", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a reStructuredText document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as the main heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The output is a reStructuredText-formatted text file with the title as the main heading, followed by a table containing the TableLayoutPanel data. The table uses grid table syntax with proper alignment and separators. Special characters in the cell data are escaped to prevent breaking the reStructuredText syntax.</remarks>
	public static void SaveAsReStructuredText(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, calculate column widths for proper alignment, and write the output file with UTF-8 encoding. The output is a reStructuredText-formatted text file with the title as the main heading, followed by a grid table containing the TableLayoutPanel data. Special characters in the cell data are escaped to prevent breaking the reStructuredText syntax.
		try
		{
			// Get the column headers and rows from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Convert the rows to a list to allow multiple iterations for calculating column widths and writing the output.
			List<string[]> rows = [.. GetRows(tableLayoutPanel: tableLayoutPanel)];
			// Calculate the maximum width of each column based on the headers and cell data to create a properly aligned grid table.
			int[] widths = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				widths[c] = headers[c].Length + 2;
			}
			// Iterate through each row and update the column widths if any cell in that column is wider than the current maximum.
			foreach (string[] row in rows)
			{
				// Only consider as many cells as there are headers to avoid index out of range errors if some rows have fewer cells than the number of columns.
				for (int c = 0; c < Math.Min(val1: row.Length, val2: headers.Length); c++)
				{
					// Add 2 to the cell length for padding (one space on each side) when calculating the width needed for that column.
					int w = row[c].Length + 2;
					if (w > widths[c])
					{
						widths[c] = w;
					}
				}
			}
			// Construct the separator lines for the grid table based on the calculated column widths.
			string separator = "+" + string.Join(separator: "+", values: widths.Select(selector: w => new string(c: '-', count: w))) + "+";
			// The header separator uses '=' characters instead of '-' to visually distinguish the header row from the data rows.
			string headerSep = "+" + string.Join(separator: "+", values: widths.Select(selector: w => new string(c: '=', count: w))) + "+";
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the main heading, followed by an empty line, then the grid table with proper separators and alignment. The header row is separated from the data rows with a distinct separator line.
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine();
			writer.WriteLine(value: separator);
			string headerRow = $"|{string.Join(separator: "|", values: headers.Select(selector: (h, i) => $" {h.PadRight(totalWidth: widths[i] - 1)}"))}|";
			writer.WriteLine(value: headerRow);
			writer.WriteLine(value: headerSep);
			// Write each data row with proper padding to align with the column widths, and separate rows with the standard separator line.
			foreach (string[] row in rows)
			{
				string dataRow = "|" + string.Join(separator: "|", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					// Escape pipe characters in the cell data to prevent breaking the Textile table syntax, since '|' is used as a column separator. The escaping is done by replacing '|' with '&#124;', which is the HTML entity for the pipe character.
					cell = cell.Replace(oldValue: "|", newValue: "&#124;");
					return $" {cell.PadRight(totalWidth: widths[c] - 1)}";
				})) + "|";
				writer.WriteLine(value: dataRow);
				writer.WriteLine(value: separator);
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "reStructuredText", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Textile document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper Textile document, not a plain text file.</remarks>
	public static void SaveAsTextile(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and write the output file with UTF-8 encoding. The output is a Textile-formatted text file with the title as a level-1 heading, followed by a table containing the TableLayoutPanel data. Special characters in the cell data are escaped to prevent breaking the Textile syntax.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a level-1 heading, followed by an empty line, then the Textile table header row and each row of data with cells escaped for Textile.
			writer.WriteLine(value: $"h1. {title}");
			writer.WriteLine();
			writer.WriteLine(value: "|_. " + string.Join(separator: " |_. ", values: headers) + " |");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Escape pipe characters in the cell data to prevent breaking the Textile table syntax, since '|' is used as a column separator. The escaping is done by replacing '|' with '&#124;', which is the HTML entity for the pipe character.
				string[] escaped = [.. row.Select(selector: static v => v.Replace(oldValue: "|", newValue: "&#124;"))];
				writer.WriteLine(value: "| " + string.Join(separator: " | ", values: escaped) + " |");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Textile", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Microsoft Word document (DOCX).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a styled paragraph at the top.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed DOCX (ZIP/Open XML) archive, not a flat XML file.</remarks>
	public static void SaveAsWord(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a proper compressed DOCX (ZIP/Open XML) archive with the necessary structure and content types. The document contains a table with the TableLayoutPanel data and a styled title paragraph.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream to create the output file, and a ZipArchive to write the DOCX structure. The DOCX format requires specific entries such as [Content_Types].xml, _rels/.rels, and word/document.xml with the appropriate content types and relationships.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// The [Content_Types].xml entry defines the content types for the parts in the package, including the main document part (word/document.xml) and the relationships part (_rels/.rels).
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
				writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
				writer.WriteLine(value: "  <Override PartName=\"/word/document.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\"/>");
				writer.WriteLine(value: "</Types>");
			}
			// The _rels/.rels entry defines the relationship from the package to the main document part (word/document.xml).
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"word/document.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// The word/document.xml entry contains the main content of the Word document, including the title as a styled paragraph and a table with the TableLayoutPanel data. The XML structure follows the Open XML format for WordprocessingML.
			ZipArchiveEntry documentEntry = archive.CreateEntry(entryName: "word/document.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: documentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<w:document xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
				writer.WriteLine(value: "  <w:body>");
				string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
				writer.WriteLine(value: $"    <w:p><w:pPr><w:pStyle w:val=\"Title\"/></w:pPr><w:r><w:t>{safeTitle}</w:t></w:r></w:p>");
				writer.WriteLine(value: "    <w:tbl>");
				writer.WriteLine(value: "      <w:tblPr><w:tblStyle w:val=\"TableGrid\"/><w:tblW w:w=\"0\" w:type=\"auto\"/></w:tblPr>");
				writer.Write(value: "      <w:tr>");
				foreach (string h in headers)
				{
					string safe = System.Net.WebUtility.HtmlEncode(value: h) ?? string.Empty;
					writer.Write(value: $"<w:tc><w:p><w:r><w:t>{safe}</w:t></w:r></w:p></w:tc>");
				}
				writer.WriteLine(value: "</w:tr>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "      <w:tr>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						string safe = System.Net.WebUtility.HtmlEncode(value: cell) ?? string.Empty;
						writer.Write(value: $"<w:tc><w:p><w:r><w:t>{safe}</w:t></w:r></w:p></w:tc>");
					}
					writer.WriteLine(value: "</w:tr>");
				}
				writer.WriteLine(value: "    </w:tbl>");
				writer.WriteLine(value: "  </w:body>");
				writer.WriteLine(value: "</w:document>");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Word", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an OpenDocument Text file (ODT).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed ODT (ZIP) archive, not a flat XML file.</remarks>
	public static void SaveAsOdt(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a proper compressed ODT (ZIP) archive with the necessary structure and content types. The document contains a table with the TableLayoutPanel data and a heading with the title.
		try
		{
			//	Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream to create the output file, and a ZipArchive to write the ODT structure. The ODT format requires specific entries such as mimetype, META-INF/manifest.xml, and content.xml with the appropriate content types and structure. The document contains a table with the TableLayoutPanel data and a heading with the title.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// The mimetype entry must be the first entry in the ZIP archive and must be stored without compression. It specifies the MIME type of the document, which is "application/vnd.oasis.opendocument.text" for ODT files.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.text");
			}
			// The META-INF/manifest.xml entry defines the manifest of the ODT package, listing the files included in the archive and their corresponding MIME types. It is stored with compression.
			ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.text\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: "</manifest:manifest>");
			}
			// The content.xml entry contains the main content of the ODT document, including the title as a heading and a table with the TableLayoutPanel data. The XML structure follows the OpenDocument format for text documents.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\">");
				writer.WriteLine(value: "  <office:body><office:text>");
				string safeTitle = System.Security.SecurityElement.Escape(str: title) ?? string.Empty;
				writer.WriteLine(value: $"    <text:h text:outline-level=\"1\">{safeTitle}</text:h>");
				writer.WriteLine(value: $"    <table:table table:name=\"Data\"><table:table-column table:number-columns-repeated=\"{headers.Length}\"/>");
				writer.Write(value: "    <table:table-header-rows><table:table-row>");
				foreach (string h in headers)
				{
					string safe = System.Security.SecurityElement.Escape(str: h) ?? string.Empty;
					writer.Write(value: $"<table:table-cell><text:p>{safe}</text:p></table:table-cell>");
				}
				writer.WriteLine(value: "</table:table-row></table:table-header-rows>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "    <table:table-row>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						string safe = System.Security.SecurityElement.Escape(str: cell) ?? string.Empty;
						writer.Write(value: $"<table:table-cell><text:p>{safe}</text:p></table:table-cell>");
					}
					writer.WriteLine(value: "</table:table-row>");
				}
				writer.WriteLine(value: "    </table:table>");
				writer.WriteLine(value: "  </office:text></office:body>");
				writer.WriteLine(value: "</office:document-content>");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODT", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Rich Text Format (RTF) file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a bold heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The RTF file is saved using ASCII encoding.</remarks>
	public static void SaveAsRtf(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in Rich Text Format (RTF) with ASCII encoding. The RTF document contains a bold heading for the title and a table with the TableLayoutPanel data. Special characters in the cell data are escaped to prevent breaking the RTF syntax.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in Rich Text Format (RTF) with ASCII encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			// Write the RTF header, define a font table with Arial as the default font, write the title as a bold heading, and then write the table rows with proper RTF syntax. Special characters in the cell data are escaped using a helper method to ensure the RTF document is well-formed.
			writer.WriteLine(value: @"{\rtf1\ansi\deff0");
			writer.WriteLine(value: @"{\fonttbl{\f0 Arial;}}");
			writer.WriteLine(value: @"\f0\fs20");
			writer.WriteLine(value: $@"{{\pard\b\fs24 {EscapeRtf(input: title)}\par\par}}");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				int cumWidth = 0;
				writer.Write(value: @"\trowd\trgaph108\trleft-108");
				// Define the cell boundaries for each column. The width of each cell is set to 1440 twips (1 inch) for simplicity, but this can be adjusted as needed. The cumulative width is used to specify the right boundary of each cell.
				for (int c = 0; c < headers.Length; c++)
				{
					cumWidth += 1440;
					writer.Write(value: $@"\cellx{cumWidth}");
				}
				// Write the cell contents for the current row. Each cell is started with '\pard\intbl', followed by the escaped cell content, and ended with '\cell'. After all cells in the row are written, the row is ended with '\row'.
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.Write(value: @"\pard\intbl ");
					writer.Write(value: EscapeRtf(input: cell));
					writer.Write(value: @"\cell");
				}
				writer.WriteLine(value: @"\row");
			}
			writer.WriteLine(value: "}");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "RTF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an AbiWord document (ABW).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 paragraph.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed ABW (ZIP/XML) archive, not a flat XML file.</remarks>
	public static void SaveAsAbiword(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a proper compressed ABW (ZIP/XML) archive with the necessary structure. The ABW format is based on XML and requires specific elements such as <abiword>, <section>, <p>, and <table> to structure the document content. The document contains a paragraph for the title and a table for the TableLayoutPanel data.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in XML format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The XML structure follows the ABW format, with proper escaping of special characters in the title and cell data to ensure a well-formed XML document.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			writer.WriteLine(value: "<!DOCTYPE abiword PUBLIC \"-//ABISOURCE//DTD AWML 1.0 Strict//EN\" \"http://www.abisource.com/awml.dtd\">");
			writer.WriteLine(value: "<abiword xmlns:awml=\"http://www.abisource.com/awml.dtd\" version=\"1.9.2\" fileformat=\"1.0\" xmlns=\"http://www.abisource.com/awml.dtd\">");
			writer.WriteLine(value: "  <section>");
			string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
			writer.WriteLine(value: $"    <p style=\"Heading 1\">{safeTitle}</p>");
			writer.WriteLine(value: "    <table>");
			int rowIdx = 0;
			for (int c = 0; c < headers.Length; c++)
			{
				// Encode the header text to ensure that any special characters are properly represented in the XML output, preventing issues with rendering or XML structure. The HtmlEncode method converts characters like '<', '>', '&', and '"' into their corresponding XML entities.
				string safeH = System.Net.WebUtility.HtmlEncode(value: headers[c]) ?? string.Empty;
				writer.WriteLine(value: $"      <cell left-attach=\"{c}\" right-attach=\"{c + 1}\" top-attach=\"{rowIdx}\" bottom-attach=\"{rowIdx + 1}\">");
				writer.WriteLine(value: $"        <p>{safeH}</p>");
				writer.WriteLine(value: "      </cell>");
			}
			rowIdx++;
			foreach (string[] dataRow in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the ABW table. Each cell's content is encoded to ensure that special characters do not break the XML structure. The cell elements include attributes to specify their position in the table based on the column index and row index.
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < dataRow.Length ? dataRow[c] : string.Empty;
					string safe = System.Net.WebUtility.HtmlEncode(value: cell) ?? string.Empty;
					writer.WriteLine(value: $"      <cell left-attach=\"{c}\" right-attach=\"{c + 1}\" top-attach=\"{rowIdx}\" bottom-attach=\"{rowIdx + 1}\">");
					writer.WriteLine(value: $"        <p>{safe}</p>");
					writer.WriteLine(value: "      </cell>");
				}
				rowIdx++;
			}
			writer.WriteLine(value: "    </table>");
			writer.WriteLine(value: "  </section>");
			writer.WriteLine(value: "</abiword>");
			//
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AbiWord", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a WPS Writer document (WPS).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>WPS Writer natively supports HTML-based content; the file is saved in HTML format internally.</remarks>
	public static void SaveAsWps(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in HTML format with UTF-8 encoding. The HTML document contains a heading for the title and a table for the TableLayoutPanel data. Special characters in the title and cell data are encoded using HTML entities to ensure a well-formed HTML document that can be opened in WPS Writer.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in HTML format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The HTML structure includes a DOCTYPE declaration, head with meta charset and title, and a body containing an H1 heading for the title and a table for the TableLayoutPanel data. Special characters are encoded using System.Net.WebUtility.HtmlEncode to ensure the HTML document is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "<!DOCTYPE html>");
			writer.WriteLine(value: "<html><head><meta charset=\"utf-8\">");
			writer.WriteLine(value: $"<title>{System.Net.WebUtility.HtmlEncode(value: title)}</title>");
			writer.WriteLine(value: "<style>table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid black;padding:5px;text-align:left}}</style>");
			writer.WriteLine(value: "</head><body>");
			writer.WriteLine(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			writer.Write(value: "<table><tr>");
			foreach (string h in headers)
			{
				// Encode the header text to ensure that any special characters are properly represented in the HTML output, preventing issues with rendering or HTML structure. The HtmlEncode method converts characters like '<', '>', '&', and '"' into their corresponding HTML entities.
				writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			writer.WriteLine(value: "</tr>");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the HTML table. Each cell's content is encoded to ensure that special characters do not break the HTML structure. The table is styled with borders and padding for better readability when opened in WPS Writer.
				writer.Write(value: "<tr>");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.Write(value: $"<td>{System.Net.WebUtility.HtmlEncode(value: cell)}</td>");
				}
				writer.WriteLine(value: "</tr>");
			}
			writer.WriteLine(value: "</table></body></html>");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "WPS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Microsoft Excel workbook (XLSX).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title (used as a comment; the sheet is named "Data").</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed XLSX (ZIP/Open XML) archive.</remarks>
	public static void SaveAsExcel(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a proper compressed XLSX (ZIP/Open XML) archive with the necessary structure and content types. The document contains a single sheet named "Data" with the TableLayoutPanel data. The title is included as a comment in the first cell (A1) for reference.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream to create the output file, and a ZipArchive to write the XLSX structure. The XLSX format requires specific entries such as [Content_Types].xml, _rels/.rels, xl/workbook.xml, xl/_rels/workbook.xml.rels, and xl/worksheets/sheet1.xml with the appropriate content types and relationships. The sheet contains the headers in the first row and the data rows below, with the title included as a comment in cell A1.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// The [Content_Types].xml entry defines the content types for the parts in the package, including the main workbook part (xl/workbook.xml), the worksheet part (xl/worksheets/sheet1.xml), and the relationships part (_rels/.rels).
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
				writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
				writer.WriteLine(value: "  <Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>");
				writer.WriteLine(value: "  <Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>");
				writer.WriteLine(value: "</Types>");
			}
			// The _rels/.rels entry defines the relationship from the package to the main workbook part (xl/workbook.xml).
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// The xl/workbook.xml entry contains the main content of the workbook, including the sheet definitions. The workbook has a single sheet named "Data" with an ID of 1 and a relationship ID of rId1 that points to the worksheet part (xl/worksheets/sheet1.xml).
			ZipArchiveEntry workbookEntry = archive.CreateEntry(entryName: "xl/workbook.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: workbookEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
				writer.WriteLine(value: "  <sheets><sheet name=\"Data\" sheetId=\"1\" r:id=\"rId1\"/></sheets>");
				writer.WriteLine(value: "</workbook>");
			}
			// The xl/_rels/workbook.xml.rels entry defines the relationship from the workbook to the worksheet part (xl/worksheets/sheet1.xml).
			ZipArchiveEntry wbRelsEntry = archive.CreateEntry(entryName: "xl/_rels/workbook.xml.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: wbRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// The xl/worksheets/sheet1.xml entry contains the content of the worksheet, including the column headers in the first row and the data rows below. The title is included as a comment in cell A1 using the <c> element's <comment> child element.
			ZipArchiveEntry sheetEntry = archive.CreateEntry(entryName: "xl/worksheets/sheet1.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: sheetEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
				writer.WriteLine(value: "  <sheetData>");
				writer.Write(value: "    <row>");
				foreach (string h in headers)
				{
					string safe = System.Security.SecurityElement.Escape(str: h) ?? string.Empty;
					writer.Write(value: $"<c t=\"inlineStr\"><is><t>{safe}</t></is></c>");
				}
				writer.WriteLine(value: "</row>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "    <row>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						string safe = System.Security.SecurityElement.Escape(str: cell) ?? string.Empty;
						writer.Write(value: $"<c t=\"inlineStr\"><is><t>{safe}</t></is></c>");
					}
					writer.WriteLine(value: "</row>");
				}
				writer.WriteLine(value: "  </sheetData>");
				writer.WriteLine(value: "</worksheet>");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Excel", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an OpenDocument Spreadsheet (ODS).</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title used as the spreadsheet table name.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed ODS (ZIP) archive, not a flat XML file.</remarks>
	public static void SaveAsOds(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a proper compressed ODS (ZIP) archive with the necessary structure and content types. The ODS format requires specific entries such as mimetype, META-INF/manifest.xml, and content.xml with the appropriate content types and structure. The spreadsheet contains a table with the TableLayoutPanel data, and the table is named using the provided title.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream to create the output file, and a ZipArchive to write the ODS structure. The ODS format requires specific entries such as mimetype, META-INF/manifest.xml, and content.xml with the appropriate content types and structure. The spreadsheet contains a table with the TableLayoutPanel data, and the table is named using the provided title.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// The mimetype entry must be the first entry in the ZIP archive and must be stored without compression. It specifies the MIME type of the document, which is "application/vnd.oasis.opendocument.spreadsheet" for ODS files.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.spreadsheet");
			}
			// The META-INF/manifest.xml entry defines the manifest of the ODS package, listing the files included in the archive and their corresponding MIME types. It is stored with compression.
			ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.spreadsheet\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: "</manifest:manifest>");
			}
			// The content.xml entry contains the main content of the ODS document, including a table with the TableLayoutPanel data. The XML structure follows the OpenDocument format for spreadsheets, with the table named using the provided title.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\">");
				writer.WriteLine(value: "  <office:body><office:spreadsheet>");
				string safeName = System.Security.SecurityElement.Escape(str: title) ?? "Data";
				writer.WriteLine(value: $"    <table:table table:name=\"{safeName}\"><table:table-column table:number-columns-repeated=\"{headers.Length}\"/>");
				writer.Write(value: "    <table:table-row>");
				foreach (string h in headers)
				{
					string safe = System.Security.SecurityElement.Escape(str: h) ?? string.Empty;
					writer.Write(value: $"<table:table-cell office:value-type=\"string\"><text:p>{safe}</text:p></table:table-cell>");
				}
				writer.WriteLine(value: "</table:table-row>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "    <table:table-row>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						string safe = System.Security.SecurityElement.Escape(str: cell) ?? string.Empty;
						writer.Write(value: $"<table:table-cell office:value-type=\"string\"><text:p>{safe}</text:p></table:table-cell>");
					}
					writer.WriteLine(value: "</table:table-row>");
				}
				writer.WriteLine(value: "    </table:table>");
				writer.WriteLine(value: "  </office:spreadsheet></office:body>");
				writer.WriteLine(value: "</office:document-content>");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a CSV file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>Fields containing special characters (commas, quotes, newlines) are properly escaped according to CSV standards.</remarks>
	public static void SaveAsCsv(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in CSV format with UTF-8 encoding. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain special characters such as commas, quotes, or newlines are escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the CSV file is well-formed and can be opened in spreadsheet applications without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in CSV format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain special characters are escaped using a helper method to ensure the CSV file is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: ";", values: headers.Select(selector: EscapeCsvField)));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the CSV file. Each field is processed to escape special characters as needed. The fields are then joined with semicolons to form a single line for each row.
				writer.WriteLine(value: string.Join(separator: ";", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return EscapeCsvField(field: cell);
				})));
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "CSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a TSV file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>Fields containing tabs or newlines are properly escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the TSV file is well-formed.</remarks>
	public static void SaveAsTsv(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in TSV format with UTF-8 encoding. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain tabs or newlines are escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the TSV file is well-formed and can be opened in spreadsheet applications without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in TSV format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain tabs or newlines are escaped using a helper method to ensure the TSV file is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: "\t", values: headers));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the TSV file. Each field is processed to escape tabs and newlines as needed. The fields are then joined with tabs to form a single line for each row.
				writer.WriteLine(value: string.Join(separator: "\t", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c => c < row.Length ? row[c] : string.Empty)));
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a PSV file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>Fields containing pipe characters or newlines are properly escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the PSV file is well-formed.</remarks>
	public static void SaveAsPsv(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in PSV format with UTF-8 encoding. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain pipe characters or newlines are escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the PSV file is well-formed and can be opened in spreadsheet applications without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in PSV format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain pipe characters or newlines are escaped using a helper method to ensure the PSV file is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: "|", values: headers));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the PSV file. Each field is processed to escape pipe characters and newlines as needed. The fields are then joined with pipe characters to form a single line for each row.
				writer.WriteLine(value: string.Join(separator: "|", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c => c < row.Length ? row[c] : string.Empty)));
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a WPS Spreadsheet (ET) file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>ET files use CSV format for WPS Spreadsheet compatibility.</remarks>
	public static void SaveAsEt(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in CSV format with UTF-8 encoding. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain special characters such as commas, quotes, or newlines are escaped by enclosing them in double quotes and doubling any internal double quotes to ensure the CSV file is well-formed and can be opened in WPS Spreadsheet without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in CSV format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The first line contains the column headers, and each subsequent line contains a data row. Fields that contain special characters are escaped using a helper method to ensure the CSV file is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: ";", values: headers.Select(selector: EscapeCsvField)));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the CSV file. Each field is processed to escape special characters as needed. The fields are then joined with semicolons to form a single line for each row.
				writer.WriteLine(value: string.Join(separator: ";", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return EscapeCsvField(field: cell);
				})));
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ET", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an HTML file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written in the &lt;title&gt; element and as an &lt;h1&gt; heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The HTML document contains a heading for the title and a table for the TableLayoutPanel data. Special characters in the title and cell data are encoded using HTML entities to ensure a well-formed HTML document that can be opened in web browsers.</remarks>
	public static void SaveAsHtml(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in HTML format with UTF-8 encoding. The HTML document includes a DOCTYPE declaration, head with meta charset and title, and a body containing an H1 heading for the title and a table for the TableLayoutPanel data. Special characters in the title and cell data are encoded using System.Net.WebUtility.HtmlEncode to ensure that the HTML document is well-formed and can be opened in web browsers without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in HTML format with UTF-8 encoding. The HTML document includes a DOCTYPE declaration, head with meta charset and title, and a body containing an H1 heading for the title and a table for the TableLayoutPanel data. Special characters in the title and cell data are encoded using System.Net.WebUtility.HtmlEncode to ensure that the HTML document is well-formed and can be opened in web browsers without issues.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "<!DOCTYPE html>");
			writer.WriteLine(value: "<html><head><meta charset=\"utf-8\">");
			writer.WriteLine(value: $"<title>{System.Net.WebUtility.HtmlEncode(value: title)}</title>");
			writer.WriteLine(value: "<style>table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid #000;padding:5px;text-align:left}}th{{background-color:#f2f2f2}}</style>");
			writer.WriteLine(value: "</head><body>");
			writer.WriteLine(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			writer.Write(value: "<table><thead><tr>");
			foreach (string h in headers)
			{
				// Write each column header in the HTML table. Each header is encoded to ensure that special characters do not break the HTML structure. The headers are styled with a background color for better readability when opened in web browsers.
				writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			writer.WriteLine(value: "</tr></thead><tbody>");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the HTML table. Each cell is encoded to ensure that special characters do not break the HTML structure. The table uses standard HTML tags to create a well-formed document that can be opened in web browsers.
				writer.Write(value: "<tr>");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.Write(value: $"<td>{System.Net.WebUtility.HtmlEncode(value: cell)}</td>");
				}
				writer.WriteLine(value: "</tr>");
			}
			writer.WriteLine(value: "</tbody></table></body></html>");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "HTML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an XML file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as a <c>title</c> attribute on the root element.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates an XML document with a root element named "data" and a "title" attribute. Each row in the TableLayoutPanel is represented as a "row" element, and each cell is represented as a child element with a name derived from the column header.</remarks>
	public static void SaveAsXml(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in XML format with UTF-8 encoding. The XML document has a root element named "data" with a "title" attribute. Each row in the TableLayoutPanel is represented as a "row" element, and each cell is represented as a child element with a name derived from the column header. Special characters in the headers and cell data are encoded to ensure that the XML document is well-formed and can be opened in XML viewers without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use an XmlWriter to write the output file in XML format with UTF-8 encoding. The XML document has a root element named "data" with a "title" attribute. Each row in the TableLayoutPanel is represented as a "row" element, and each cell is represented as a child element with a name derived from the column header. Special characters in the headers and cell data are encoded to ensure that the XML document is well-formed.
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "data");
			xmlWriter.WriteAttributeString(localName: "title", value: title);
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the XML document. Each cell is encoded to ensure that special characters do not break the XML structure.
				xmlWriter.WriteStartElement(localName: "row");
				for (int c = 0; c < headers.Length; c++)
				{
					// Create a valid XML element name from the column header. If the header is empty, use a default name like "col{index}". The header is encoded to ensure that it can be used as an XML element name without issues.
					string elementName = headers[c].Length > 0
						? XmlConvert.EncodeName(name: headers[c]) ?? $"col{c}"
						: $"col{c}";
					string cell = c < row.Length ? row[c] : string.Empty;
					xmlWriter.WriteElementString(localName: elementName, value: cell);
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "XML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a DocBook XML document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The document title written in the &lt;title&gt; element.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a DocBook XML document with an "article" root element, a "title" element for the document title, and a "section" containing a "table" with the TableLayoutPanel data. The table includes a header row with column headers and subsequent rows for the data. Special characters in the title and cell data are encoded to ensure that the XML document is well-formed and can be processed by DocBook tools without issues.</remarks>
	public static void SaveAsDocBook(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in DocBook XML format with UTF-8 encoding. The XML document has an "article" root element, a "title" element for the document title, and a "section" containing a "table" with the TableLayoutPanel data. The table includes a header row with column headers and subsequent rows for the data. Special characters in the title and cell data are encoded to ensure that the XML document is well-formed and can be processed by DocBook tools without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use an XmlWriter to write the output file in DocBook XML format with UTF-8 encoding. The XML document has an "article" root element, a "title" element for the document title, and a "section" containing a "table" with the TableLayoutPanel data. The table includes a header row with column headers and subsequent rows for the data. Special characters in the title and cell data are encoded to ensure that the XML document is well-formed.
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "article", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteAttributeString(localName: "version", value: "5.0");
			xmlWriter.WriteElementString(localName: "title", value: title);
			xmlWriter.WriteStartElement(localName: "section", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteStartElement(localName: "table", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteAttributeString(localName: "frame", value: "all");
			xmlWriter.WriteElementString(localName: "title", value: title);
			xmlWriter.WriteStartElement(localName: "tgroup", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteAttributeString(localName: "cols", value: headers.Length.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
			for (int c = 0; c < headers.Length; c++)
			{
				// Write the column specifications for the DocBook table. Each column is defined with a "colspec" element, and the "colname" attribute is set to a unique name based on the column index (e.g., "c1", "c2", etc.). This defines the structure of the table and allows for proper formatting when processed by DocBook tools.
				xmlWriter.WriteStartElement(localName: "colspec", ns: "http://docbook.org/ns/docbook");
				xmlWriter.WriteAttributeString(localName: "colname", value: $"c{c + 1}");
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteStartElement(localName: "thead", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteStartElement(localName: "row", ns: "http://docbook.org/ns/docbook");
			foreach (string h in headers)
			{
				// Write each column header in the DocBook table. Each header is encoded to ensure that special characters do not break the XML structure. The headers are placed in the "thead" section of the table to indicate that they are column headers.
				xmlWriter.WriteElementString(localName: "entry", value: h);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement(localName: "tbody", ns: "http://docbook.org/ns/docbook");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the DocBook table. Each cell is encoded to ensure that special characters do not break the XML structure. The rows are placed in the "tbody" section of the table to indicate that they are data rows.
				xmlWriter.WriteStartElement(localName: "row", ns: "http://docbook.org/ns/docbook");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					xmlWriter.WriteElementString(localName: "entry", value: cell);
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "DocBook", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a JSON file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> property at the root of the JSON object.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a JSON document with a root object containing a "title" property and a "rows" property. The "rows" property is an array of objects, where each object represents a row in the TableLayoutPanel and has properties corresponding to the column headers. Special characters in the headers and cell data are properly escaped to ensure that the JSON document is well-formed and can be parsed by JSON parsers without issues.</remarks>
	public static void SaveAsJson(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in JSON format with UTF-8 encoding. The JSON document has a root object containing a "title" property and a "rows" property. The "rows" property is an array of objects, where each object represents a row in the TableLayoutPanel and has properties corresponding to the column headers. Special characters in the headers and cell data are properly escaped by the JsonSerializer to ensure that the JSON document is well-formed.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Create a list of dictionaries to represent the rows of the TableLayoutPanel. Each dictionary represents a single row, with keys corresponding to the column headers and values corresponding to the cell data. The GetRows method is used to retrieve the data rows from the TableLayoutPanel, which reads data from all rows after row 0 (the header row).
			List<Dictionary<string, string>> records = [];
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Create a dictionary for the current row, mapping column headers to cell values. If a row has fewer cells than headers, the missing cells are treated as empty strings. This ensures that each row object in the JSON output has a consistent set of properties corresponding to the column headers.
				Dictionary<string, string> record = [];
				for (int c = 0; c < headers.Length; c++)
				{
					record[key: headers[c]] = c < row.Length ? row[c] : string.Empty;
				}
				records.Add(item: record);
			}
			// Create an anonymous object to represent the root of the JSON document, containing the title and the array of row objects. The JsonSerializer will handle the serialization of this object to a JSON string, including proper escaping of special characters.
			ListViewExporter.NewRecord doc = new(Title: title, Rows: records);
			string json = JsonSerializer.Serialize(value: doc, options: jsonSerializerOptions);
			File.WriteAllText(path: fileName, contents: json);
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "JSON", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a YAML file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> key at the root of the YAML document.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a YAML document with a root object containing a "title" property and a "rows" property. The "rows" property is an array of objects, where each object represents a row in the TableLayoutPanel and has properties corresponding to the column headers. Special characters in the headers and cell data are properly escaped to ensure that the YAML document is well-formed and can be parsed by YAML parsers without issues.</remarks>
	public static void SaveAsYaml(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in YAML format with UTF-8 encoding. The YAML document has a root object containing a "title" property and a "rows" property. The "rows" property is an array of objects, where each object represents a row in the TableLayoutPanel and has properties corresponding to the column headers. Special characters in the headers and cell data are properly escaped to ensure that the YAML document is well-formed.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in YAML format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The YAML document has a root object containing a "title" property and a "rows" property. The "rows" property is an array of objects, where each object represents a row in the TableLayoutPanel and has properties corresponding to the column headers. Special characters in the headers and cell data are escaped by replacing double quotes with escaped double quotes to ensure that the YAML document is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "---");
			writer.WriteLine(value: $"title: \"{title.Replace(oldValue: "\"", newValue: "\\\"")}\"");
			writer.WriteLine(value: $"created_at: \"{DateTime.UtcNow:O}\"");
			writer.WriteLine(value: "rows:");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the YAML document. Each cell is processed to escape double quotes by replacing them with escaped double quotes. The rows are represented as a list of objects under the "rows" key, with each object containing properties corresponding to the column headers and cell values.
				writer.WriteLine(value: "  - item:");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					string safeCell = cell.Replace(oldValue: "\"", newValue: "\\\"");
					string safeKey = headers[c].Replace(oldValue: "\"", newValue: "\\\"");
					writer.WriteLine(value: $"      {safeKey}: \"{safeCell}\"");
				}
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "YAML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a TOML file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> key at the top of the file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a TOML document with a "title" key at the top of the file and a list of tables for each row in the TableLayoutPanel. Each table is represented as a TOML array of tables with keys corresponding to the column headers. Special characters in the headers and cell data are properly escaped to ensure that the TOML document is well-formed and can be parsed by TOML parsers without issues.</remarks>
	public static void SaveAsToml(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in TOML format with UTF-8 encoding. The TOML document has a "title" key at the top of the file and a list of tables for each row in the TableLayoutPanel. Each table is represented as a TOML array of tables with keys corresponding to the column headers. Special characters in the headers and cell data are escaped by replacing double quotes with escaped double quotes to ensure that the TOML document is well-formed.
		try
		{
			//Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a StreamWriter to write the output file in TOML format with UTF-8 encoding. The 'append: false' parameter ensures that the file is overwritten if it already exists. The TOML document has a "title" key at the top of the file and a list of tables for each row in the TableLayoutPanel. Each table is represented as a TOML array of tables with keys corresponding to the column headers. Special characters in the headers and cell data are escaped by replacing double quotes with escaped double quotes to ensure that the TOML document is well-formed.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"title = \"{EscapeToml(value: title)}\"");
			writer.WriteLine(value: $"created_at = {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
			writer.WriteLine();
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the TOML document. Each cell is processed to escape double quotes by replacing them with escaped double quotes. The rows are represented as an array of tables, with each table containing key-value pairs corresponding to the column headers and cell values.
				writer.WriteLine(value: "[[rows]]");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.WriteLine(value: $"{EscapeToml(value: headers[c])} = \"{EscapeToml(value: cell)}\"");
				}
				writer.WriteLine();
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TOML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a SQL INSERT script.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Used as the SQL table name in the CREATE TABLE and INSERT statements.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a SQL script that includes a CREATE TABLE statement to define the table structure based on the column headers, followed by INSERT INTO statements for each row of data. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. Special characters in the data are escaped to ensure that the SQL script is well-formed and can be executed against a SQL database without issues.</remarks>
	public static void SaveAsSql(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file as a SQL script with UTF-8 encoding. The SQL script includes a CREATE TABLE statement to define the table structure based on the column headers, followed by INSERT INTO statements for each row of data. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. Special characters in the data are escaped by doubling single quotes to ensure that the SQL script is well-formed and can be executed against a SQL database without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Create a valid SQL table name from the title by replacing non-alphanumeric characters with underscores. If the resulting table name is empty, use a default name like "Data". This ensures that the CREATE TABLE statement in the SQL script has a valid table name.
			string tableName = new(value: [.. title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_')]);
			if (tableName.Length == 0)
			{
				tableName = "Data";
			}
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"-- Export generated by Planetoid-DB");
			writer.WriteLine(value: $"-- Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			writer.WriteLine();
			string colDefs = string.Join(separator: ",\n    ", values: headers.Select(selector: h => $"[{h}] VARCHAR(255)"));
			writer.WriteLine(value: $"CREATE TABLE IF NOT EXISTS [{tableName}] (");
			writer.WriteLine(value: $"    {colDefs}");
			writer.WriteLine(value: ");");
			writer.WriteLine();
			writer.WriteLine(value: "BEGIN TRANSACTION;");
			string colList = string.Join(separator: ", ", values: headers.Select(selector: h => $"[{h}]"));
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write an INSERT INTO statement for each row of data. Each cell value is escaped by replacing single quotes with two single quotes to ensure that the SQL script is well-formed. The values are enclosed in single quotes to be treated as string literals in the SQL script.
				string values = string.Join(separator: ", ", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return $"'{cell.Replace(oldValue: "'", newValue: "''")}'";
				}));
				writer.WriteLine(value: $"INSERT INTO [{tableName}] ({colList}) VALUES ({values});");
			}
			writer.WriteLine(value: "COMMIT;");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "SQL", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a SQLite database file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Used as the table name inside the SQLite database.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>This method creates a SQLite database file with a single table containing the data from the TableLayoutPanel. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. The method uses parameterized SQL commands to insert the data, which ensures that special characters in the data are properly escaped and that the resulting database is well-formed and can be opened with SQLite tools without issues.</remarks>
	public static void SaveAsSqlite(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and create a SQLite database file with a single table containing the data from the TableLayoutPanel. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. The method uses parameterized SQL commands to insert the data, which ensures that special characters in the data are properly escaped and that the resulting database is well-formed.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Create a valid SQL table name from the title by replacing non-alphanumeric characters with underscores. If the resulting table name is empty, use a default name like "Data". This ensures that the CREATE TABLE statement in the SQLite database has a valid table name.
			string tableName = new(value: [.. title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_')]);
			if (tableName.Length == 0)
			{
				tableName = "Data";
			}
			if (File.Exists(path: fileName))
			{
				File.Delete(path: fileName);
			}
			string connStr = $"Data Source={fileName};Version=3;";
			using SQLiteConnection connection = new(connectionString: connStr);
			connection.Open();
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				string colDefs = string.Join(separator: ", ", values: headers.Select(selector: h => $"[{h}] TEXT"));
				cmd.CommandText = $"CREATE TABLE IF NOT EXISTS [{tableName}] ({colDefs});";
				cmd.ExecuteNonQuery();
			}
			using SQLiteTransaction transaction = connection.BeginTransaction();
			string colNames = string.Join(separator: ", ", values: headers.Select(selector: h => $"[{h}]"));
			string paramNames = string.Join(separator: ", ", values: headers.Select(selector: (_, i) => $"@p{i}"));
			using SQLiteCommand insertCmd = connection.CreateCommand();
			insertCmd.CommandText = $"INSERT INTO [{tableName}] ({colNames}) VALUES ({paramNames});";
			SQLiteParameter[] parameters = new SQLiteParameter[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				// Create parameters for the INSERT command. Each parameter is named "@p{index}" and is of type TEXT. This allows the method to safely insert data containing special characters without risking SQL injection or syntax errors in the resulting SQLite database.
				parameters[c] = insertCmd.Parameters.Add(parameterName: $"@p{c}", parameterType: System.Data.DbType.String);
			}
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Set the parameter values for the current row. If a row has fewer cells than headers, the missing cells are treated as empty strings. This ensures that each INSERT statement has a value for every column defined in the table, and that special characters in the data are properly handled by the parameterized command.
				for (int c = 0; c < headers.Length; c++)
				{
					parameters[c].Value = c < row.Length ? row[c] : string.Empty;
				}
				insertCmd.ExecuteNonQuery();
			}
			transaction.Commit();
			connection.Close();
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "SQLite", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a PDF document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the document heading on each page.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The method generates a PDF document with the specified title and the contents of the TableLayoutPanel. Each page contains a heading with the title and a table with the data.</remarks>
	public static void SaveAsPdf(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in PDF format. The method generates a PDF document with the specified title and the contents of the TableLayoutPanel. Each page contains a heading with the title and a table with the data.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream and StreamWriter to write the output file in PDF format. The PDF document is constructed manually by writing the necessary PDF syntax to define the document structure, pages, and content. The method handles pagination by starting a new page when the content exceeds the page height. The title is written as a heading on each page, and the column headers are repeated on each new page for clarity.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using StreamWriter w = new(stream: fs, encoding: Encoding.ASCII);
			List<long> objectOffsets = [];
			int StartNewObject()
			{
				w.Flush();
				objectOffsets.Add(item: fs.Position);
				int id = objectOffsets.Count;
				w.WriteLine(value: $"{id} 0 obj");
				return id;
			}
			w.WriteLine(value: "%PDF-1.4");
			w.WriteLine(value: "%\xb5\xb5\xb5\xb5");
			const int pageHeight = 842;
			const int startY = 750;
			const int marginY = 50;
			const int lineHeight = 14;
			int usableWidth = 495;
			int colWidth = headers.Length > 0 ? usableWidth / headers.Length : usableWidth;
			int[] colX = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				colX[c] = 50 + (c * colWidth);
			}
			List<int> pageContentObjIds = [];
			int currentY = startY;
			int currentContentObjId = StartNewObject();
			pageContentObjIds.Add(item: currentContentObjId);
			w.WriteLine(value: "<< >> stream");
			w.WriteLine(value: "BT /F1 10 Tf");
			w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 40} Tm ({EscapePdf(text: title)}) Tj");
			for (int c = 0; c < headers.Length; c++)
			{
				// Write the column headers on the PDF page. Each header is positioned based on the calculated column X coordinates and a fixed Y coordinate near the top of the page. The headers are repeated on each new page to maintain context for the data rows.
				w.WriteLine(value: $"1 0 0 1 {colX[c]} {pageHeight - 60} Tm ({EscapePdf(text: headers[c])}) Tj");
			}
			currentY = startY - 30;
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row on the PDF page. Each cell is positioned based on the calculated column X coordinates and the current Y coordinate, which is decremented by a fixed line height for each row. If the current Y coordinate goes below the margin, a new page is started by writing the necessary PDF syntax to end the current content stream and start a new one, along with the title and column headers for the new page.
				if (currentY < marginY)
				{
					w.WriteLine(value: "ET");
					w.WriteLine(value: "endstream");
					w.WriteLine(value: "endobj");
					currentContentObjId = StartNewObject();
					pageContentObjIds.Add(item: currentContentObjId);
					w.WriteLine(value: "<< >> stream");
					w.WriteLine(value: "BT /F1 10 Tf");
					w.WriteLine(value: $"1 0 0 1 50 {pageHeight - 40} Tm ({EscapePdf(text: title)} - Cont.) Tj");
					for (int c = 0; c < headers.Length; c++)
					{
						w.WriteLine(value: $"1 0 0 1 {colX[c]} {pageHeight - 60} Tm ({EscapePdf(text: headers[c])}) Tj");
					}
					currentY = startY - 30;
				}
				for (int c = 0; c < headers.Length; c++)
				{
					// Write each cell in the current row. If a row has fewer cells than headers, the missing cells are treated as empty strings.
					string cell = c < row.Length ? row[c] : string.Empty;
					w.WriteLine(value: $"1 0 0 1 {colX[c]} {currentY} Tm ({EscapePdf(text: cell)}) Tj");
				}
				currentY -= lineHeight;
			}
			w.WriteLine(value: "ET");
			w.WriteLine(value: "endstream");
			w.WriteLine(value: "endobj");
			List<int> pageObjIds = [];
			foreach (int contentId in pageContentObjIds)
			{
				// Write a page object for each content stream. Each page object references the corresponding content stream and the font resource. The parent of each page is set to the pages root object, which will be defined later. The media box is set to A4 size (595x842 points).
				int pageId = StartNewObject();
				pageObjIds.Add(item: pageId);
				int predictedParentId = objectOffsets.Count + (pageContentObjIds.Count - pageObjIds.Count) + 2;
				w.WriteLine(value: "<<");
				w.WriteLine(value: "/Type /Page");
				w.WriteLine(value: $"/Parent {predictedParentId} 0 R");
				w.WriteLine(value: "/MediaBox [0 0 595 842]");
				w.WriteLine(value: $"/Contents {contentId} 0 R");
				w.WriteLine(value: $"/Resources << /Font << /F1 {predictedParentId + 1} 0 R >> >>");
				w.WriteLine(value: ">>");
				w.WriteLine(value: "endobj");
			}
			int pagesRootId = StartNewObject();
			w.WriteLine(value: "<<");
			w.WriteLine(value: "/Type /Pages");
			w.Write(value: "/Kids [");
			foreach (int pid in pageObjIds)
			{
				// Write the Kids array for the pages root object, referencing each page object created earlier. The Kids array contains indirect references to the page objects, which allows the PDF viewer to locate and render each page correctly.
				w.Write(value: $"{pid} 0 R ");
			}
			w.WriteLine(value: "]");
			w.WriteLine(value: $"/Count {pageObjIds.Count}");
			w.WriteLine(value: ">>");
			w.WriteLine(value: "endobj");
			int fontId = StartNewObject();
			w.WriteLine(value: "<<");
			w.WriteLine(value: "/Type /Font");
			w.WriteLine(value: "/Subtype /Type1");
			w.WriteLine(value: "/BaseFont /Helvetica");
			w.WriteLine(value: ">>");
			w.WriteLine(value: "endobj");
			int catalogId = StartNewObject();
			w.WriteLine(value: "<<");
			w.WriteLine(value: "/Type /Catalog");
			w.WriteLine(value: $"/Pages {pagesRootId} 0 R");
			w.WriteLine(value: ">>");
			w.WriteLine(value: "endobj");
			w.Flush();
			long xrefOffset = fs.Position;
			w.WriteLine(value: "xref");
			w.WriteLine(value: $"0 {objectOffsets.Count + 1}");
			w.WriteLine(value: "0000000000 65535 f ");
			foreach (long offset in objectOffsets)
			{
				// Write the byte offset for each object in the xref table. Each offset is formatted as a 10-digit number with leading zeros, followed by "00000 n" to indicate that the object is in use and has a generation number of 0.
				w.WriteLine(value: $"{offset:D10} 00000 n ");
			}
			w.WriteLine(value: "trailer");
			w.WriteLine(value: "<<");
			w.WriteLine(value: $"/Size {objectOffsets.Count + 1}");
			w.WriteLine(value: $"/Root {catalogId} 0 R");
			w.WriteLine(value: ">>");
			w.WriteLine(value: "startxref");
			w.WriteLine(value: xrefOffset);
			w.WriteLine(value: "%%EOF");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PDF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a PostScript file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the page heading on each PostScript page.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The PostScript file will include page headers, column headers, and data rows. If the content exceeds one page, additional pages will be created automatically.</remarks>
	public static void SaveAsPostScript(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in PostScript format. The method generates a PostScript document with page headers containing the title and page number, column headers repeated on each page, and data rows formatted in a simple table layout. Pagination is handled by starting a new page when the content exceeds the page height, ensuring that the output is properly formatted for printing or viewing in a PostScript viewer.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			const int pageHeight = 842;
			const int marginTop = 50;
			const int marginBottom = 50;
			const int startY = pageHeight - marginTop;
			const int lineHeight = 14;
			int usableWidth = 500;
			int colWidth = headers.Length > 0 ? usableWidth / headers.Length : usableWidth;
			int[] colX = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				colX[c] = 50 + (c * colWidth);
			}
			// Declare and initialize the page number and current Y position variables
			int pageNumber = 1;
			int currentY;
			// Local function to write the page header, including the title and column headers. This function is called at the beginning of each new page to ensure that the page header is consistent across all pages. The title is displayed prominently at the top of the page, followed by the column headers positioned below it.
			void WritePageHeader(int pg)
			{
				writer.WriteLine(value: $"%%Page: {pg} {pg}");
				writer.WriteLine(value: "/Helvetica-Bold findfont 12 scalefont setfont");
				writer.WriteLine(value: $"50 {pageHeight - 30} moveto ({EscapePostScript(input: title)} - Page {pg}) show");
				writer.WriteLine(value: "/Helvetica findfont 10 scalefont setfont");
				for (int c = 0; c < headers.Length; c++)
				{
					// Write the column headers on the PostScript page. Each header is positioned based on the calculated column X coordinates and a fixed Y coordinate near the top of the page. The headers are repeated on each new page to maintain context for the data rows.
					writer.WriteLine(value: $"{colX[c]} {pageHeight - 50} moveto ({EscapePostScript(input: headers[c])}) show");
				}
			}
			writer.WriteLine(value: "%!PS-Adobe-3.0");
			writer.WriteLine(value: $"%%Title: {EscapePostScript(input: title)}");
			writer.WriteLine(value: "%%Creator: Planetoid-DB");
			writer.WriteLine(value: "%%Pages: (atend)");
			writer.WriteLine(value: "%%EndComments");
			WritePageHeader(pg: pageNumber);
			currentY = startY - 30;
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row on the PostScript page. Each cell is positioned based on the calculated column X coordinates and the current Y coordinate, which is decremented by a fixed line height for each row. If the current Y coordinate goes below the margin, a new page is started by writing the necessary PostScript syntax to end the current page and start a new one, along with the title and column headers for the new page.
				if (currentY < marginBottom)
				{
					writer.WriteLine(value: "showpage");
					pageNumber++;
					WritePageHeader(pg: pageNumber);
					currentY = startY - 30;
				}
				for (int c = 0; c < headers.Length; c++)
				{
					// Write each cell in the current row. If a row has fewer cells than headers, the missing cells are treated as empty strings.
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.WriteLine(value: $"{colX[c]} {currentY} moveto ({EscapePostScript(input: cell)}) show");
				}
				currentY -= lineHeight;
			}
			writer.WriteLine(value: "showpage");
			writer.WriteLine(value: "%%Trailer");
			writer.WriteLine(value: $"%%Pages: {pageNumber}");
			writer.WriteLine(value: "%%EOF");
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PostScript", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an EPUB file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The EPUB book title used in metadata and content pages.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed EPUB (ZIP) archive conforming to the EPUB 2 specification.</remarks>
	public static void SaveAsEpub(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and create a valid EPUB file as a ZIP archive. The method constructs the necessary EPUB structure, including the mimetype file, META-INF/container.xml, OEBPS/content.opf, OEBPS/toc.ncx, and OEBPS/content.xhtml files. The content.opf file includes metadata with the title and a manifest referencing the content and TOC files. The content.xhtml file contains an HTML representation of the TableLayoutPanel data in a table format. Special characters in the title, headers, and cell data are properly escaped to ensure that the resulting EPUB file is well-formed and can be opened with EPUB readers without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Use a FileStream and ZipArchive to create the EPUB file as a ZIP archive. The method writes the required files for a valid EPUB structure, including the mimetype file (uncompressed), the container.xml file in the META-INF directory, the content.opf file with metadata and manifest, the toc.ncx file for navigation, and the content.xhtml file with the actual content. The content is formatted as an HTML table with the title as a heading and the TableLayoutPanel data as rows in the table.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// The mimetype file must be the first entry in the ZIP archive and must be stored without compression. It specifies the MIME type of the document, which is "application/vnd.oasis.opendocument.text" for ODT files.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/epub+zip");
			}
			// The container.xml file is created in the META-INF directory and specifies the location of the content.opf file, which is the main package file for the EPUB. The container.xml file is required for EPUB readers to locate the content.opf file and access the book's metadata and content.
			ZipArchiveEntry containerEntry = archive.CreateEntry(entryName: "META-INF/container.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: containerEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\"?>");
				writer.WriteLine(value: "<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">");
				writer.WriteLine(value: "  <rootfiles><rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/></rootfiles>");
				writer.WriteLine(value: "</container>");
			}
			string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
			// The content.opf file is created in the OEBPS directory and contains the metadata for the EPUB, including the title, language, identifier, and creator. It also includes a manifest that lists the files included in the EPUB (the TOC and content files) and a spine that defines the reading order of the content.
			ZipArchiveEntry opfEntry = archive.CreateEntry(entryName: "OEBPS/content.opf", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: opfEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<package xmlns=\"http://www.idpf.org/2007/opf\" unique-identifier=\"BookId\" version=\"2.0\">");
				writer.WriteLine(value: "  <metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">");
				writer.WriteLine(value: $"    <dc:title>{safeTitle}</dc:title>");
				writer.WriteLine(value: "    <dc:language>en</dc:language>");
				writer.WriteLine(value: $"    <dc:identifier id=\"BookId\" opf:scheme=\"UUID\">urn:uuid:{Guid.NewGuid()}</dc:identifier>");
				writer.WriteLine(value: "    <dc:creator>Planetoid-DB</dc:creator>");
				writer.WriteLine(value: "  </metadata>");
				writer.WriteLine(value: "  <manifest>");
				writer.WriteLine(value: "    <item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/>");
				writer.WriteLine(value: "    <item id=\"content\" href=\"content.xhtml\" media-type=\"application/xhtml+xml\"/>");
				writer.WriteLine(value: "  </manifest>");
				writer.WriteLine(value: "  <spine toc=\"ncx\"><itemref idref=\"content\"/></spine>");
				writer.WriteLine(value: "</package>");
			}
			// The toc.ncx file is created in the OEBPS directory and defines the navigation structure of the EPUB. It includes a navMap with a single navPoint that references the content.xhtml file. This allows EPUB readers to display a table of contents and navigate to the content page.
			ZipArchiveEntry ncxEntry = archive.CreateEntry(entryName: "OEBPS/toc.ncx", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: ncxEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<ncx xmlns=\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\">");
				writer.WriteLine(value: "  <head><meta name=\"dtb:uid\" content=\"uid\"/><meta name=\"dtb:depth\" content=\"1\"/></head>");
				writer.WriteLine(value: $"  <docTitle><text>{safeTitle}</text></docTitle>");
				writer.WriteLine(value: "  <navMap><navPoint id=\"np1\" playOrder=\"1\"><navLabel><text>Content</text></navLabel><content src=\"content.xhtml\"/></navPoint></navMap>");
				writer.WriteLine(value: "</ncx>");
			}
			// The content.xhtml file is created in the OEBPS directory and contains the actual content of the EPUB. It is formatted as an HTML document with a title, a heading, and a table that includes the column headers and data rows from the TableLayoutPanel. Special characters in the title, headers, and cell data are encoded using HTML encoding to ensure that the resulting XHTML is well-formed and can be rendered correctly by EPUB readers.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "OEBPS/content.xhtml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
				writer.WriteLine(value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">");
				writer.WriteLine(value: $"<head><title>{safeTitle}</title>");
				writer.WriteLine(value: "<style type=\"text/css\">body{{font-family:sans-serif}}table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid #ddd;padding:8px;text-align:left}}th{{background-color:#f2f2f2}}</style>");
				writer.WriteLine(value: "</head><body>");
				writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
				writer.Write(value: "<table><thead><tr>");
				foreach (string h in headers)
				{
					writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
				}
				writer.WriteLine(value: "</tr></thead><tbody>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "<tr>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						writer.Write(value: $"<td>{System.Net.WebUtility.HtmlEncode(value: cell)}</td>");
					}
					writer.WriteLine(value: "</tr>");
				}
				writer.WriteLine(value: "</tbody></table></body></html>");
			}
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "EPUB", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a MOBI file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The book title embedded in the MOBI header and HTML content body.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The method generates a MOBI file with a minimal header and a single HTML content record containing the TableLayoutPanel data formatted as a table. The MOBI file structure is constructed manually, including the necessary header fields and text records. The resulting MOBI file can be opened with compatible e-book readers that support the MOBI format.</remarks>
	public static void SaveAsMobi(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in MOBI format. The method constructs a minimal MOBI file structure, including the necessary header fields and a single HTML content record containing the TableLayoutPanel data formatted as a table. The HTML content is generated by encoding the title, headers, and cell data to ensure that special characters are properly handled. The resulting MOBI file can be opened with compatible e-book readers that support the MOBI format.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Build the HTML content for the MOBI file. The HTML includes a title, a heading with the title, and a table with the column headers and data rows. Special characters in the title, headers, and cell data are encoded using HTML encoding to ensure that the resulting HTML is well-formed and can be rendered correctly by the CHM compiler and viewers.
			StringBuilder html = new();
			html.Append(value: $"<html><head><meta charset=\"UTF-8\"><title>{System.Net.WebUtility.HtmlEncode(value: title)}</title></head><body>");
			html.Append(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			html.Append(value: "<table><tr>");
			foreach (string h in headers)
			{
				// Write the column headers in the HTML table. Each header is encoded using HTML encoding to ensure that special characters are properly handled in the resulting HTML content.
				html.Append(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			html.Append(value: "</tr>");
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row in the HTML table. Each cell is encoded using HTML encoding to ensure that special characters are properly handled. If a row has fewer cells than headers, the missing cells are treated as empty strings.
				html.Append(value: "<tr>");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					html.Append(value: $"<td>{System.Net.WebUtility.HtmlEncode(value: cell)}</td>");
				}
				html.Append(value: "</tr>");
			}
			html.Append(value: "</table></body></html>");
			byte[] bodyData = Encoding.UTF8.GetBytes(s: html.ToString());
			List<byte[]> textRecords = [];
			for (int i = 0; i < bodyData.Length; i += 4096)
			{
				// Split the HTML content into chunks of 4096 bytes to create multiple text records for the MOBI file. Each chunk is stored as a byte array in the textRecords list. This allows the method to handle larger HTML content that may exceed the size limit of a single text record in the MOBI format.
				int len = Math.Min(val1: 4096, val2: bodyData.Length - i);
				byte[] chunk = new byte[len];
				Array.Copy(sourceArray: bodyData, sourceIndex: i, destinationArray: chunk, destinationIndex: 0, length: len);
				textRecords.Add(item: chunk);
			}
			byte[] headerRecord = new byte[256];
			using (MemoryStream ms = new(buffer: headerRecord))
			using (BinaryWriter hw = new(output: ms))
			{
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)1));
				hw.Write(value: (short)0);
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: bodyData.Length));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)textRecords.Count));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)4096));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)0));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)0));
				hw.Write(buffer: Encoding.ASCII.GetBytes(s: "MOBI"));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 232));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 2));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 65001));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 0x12345678));
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 6));
				ms.Seek(offset: 96, loc: SeekOrigin.Begin);
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: textRecords.Count + 1));
				ms.Seek(offset: 100, loc: SeekOrigin.Begin);
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 0));
				ms.Seek(offset: 120, loc: SeekOrigin.Begin);
				hw.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: 6));
			}
			byte[] eofRecord = [0xe9, 0x8e, 0x0d, 0x0a];
			int totalRecords = 1 + textRecords.Count + 1;
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using BinaryWriter w = new(output: fs);
			string dbName = title.Length > 31 ? title[..31] : title;
			byte[] nameBytes = new byte[32];
			Encoding.ASCII.GetBytes(s: dbName).CopyTo(array: nameBytes, index: 0);
			w.Write(buffer: nameBytes);
			w.Write(value: (short)0);
			w.Write(value: (short)0);
			uint secondsSince1904 = (uint)(DateTime.UtcNow - new DateTime(year: 1904, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)).TotalSeconds;
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (int)secondsSince1904));
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (int)secondsSince1904));
			w.Write(value: 0);
			w.Write(value: 0);
			w.Write(value: 0);
			w.Write(value: 0);
			w.Write(buffer: Encoding.ASCII.GetBytes(s: "BOOK"));
			w.Write(buffer: Encoding.ASCII.GetBytes(s: "MOBI"));
			w.Write(value: 0);
			w.Write(value: 0);
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: (short)totalRecords));
			int currentOffset = 78 + (totalRecords * 8) + 2;
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
			w.Write(value: 0);
			currentOffset += headerRecord.Length;
			foreach (byte[] rec in textRecords)
			{
				w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
				w.Write(value: 0);
				currentOffset += rec.Length;
			}
			w.Write(value: System.Net.IPAddress.HostToNetworkOrder(host: currentOffset));
			w.Write(value: 0);
			w.Write(value: (short)0);
			w.Write(buffer: headerRecord);
			foreach (byte[] rec in textRecords)
			{
				w.Write(buffer: rec);
			}
			w.Write(buffer: eofRecord);
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "MOBI", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a FictionBook 2 (FB2) XML document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The book title written in the FB2 metadata and body sections.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The method generates a valid FB2 XML document with the required structure, including the description section with metadata and the body section containing the content formatted as a table. The column headers are written as table headers, and each data row is written as a table row. Special characters in the title, headers, and cell data are properly escaped to ensure that the resulting XML document is well-formed and can be opened with FB2-compatible readers without issues.</remarks>
	public static void SaveAsFictionBook2(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in FictionBook 2 (FB2) XML format. The method constructs a valid FB2 XML document with the required structure, including the description section with metadata (such as title, author, and date) and the body section containing the content formatted as a table. The column headers are written as table headers (<th>), and each data row is written as a table row (<tr>) with cells (<td>). Special characters in the title, headers, and cell data are properly escaped to ensure that the resulting XML document is well-formed and can be opened with FB2-compatible readers without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Define the FictionBook 2 namespace and create an XmlWriter with appropriate settings for indentation and UTF-8 encoding. The XmlWriter is used to write the FB2 XML document, starting with the root element <FictionBook> and including the necessary child elements for the description and body sections. The metadata in the description section includes the title, author, language, and date, while the body section contains a table with the column headers and data rows.
			string fb2Ns = "http://www.gribuser.ru/xml/fictionbook/2.0";
			XmlWriterSettings settings = new() { Indent = true, Encoding = Encoding.UTF8 };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "FictionBook", ns: fb2Ns);
			xmlWriter.WriteAttributeString(prefix: "xmlns", localName: "l", ns: null, value: "http://www.w3.org/1999/xlink");
			xmlWriter.WriteStartElement(localName: "description", ns: fb2Ns);
			xmlWriter.WriteStartElement(localName: "title-info", ns: fb2Ns);
			xmlWriter.WriteElementString(localName: "genre", ns: fb2Ns, value: "reference");
			xmlWriter.WriteStartElement(localName: "author", ns: fb2Ns);
			xmlWriter.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
			xmlWriter.WriteElementString(localName: "last-name", ns: fb2Ns, value: string.Empty);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteElementString(localName: "book-title", ns: fb2Ns, value: title);
			xmlWriter.WriteElementString(localName: "lang", ns: fb2Ns, value: "en");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement(localName: "document-info", ns: fb2Ns);
			xmlWriter.WriteStartElement(localName: "author", ns: fb2Ns);
			xmlWriter.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
			xmlWriter.WriteElementString(localName: "last-name", ns: fb2Ns, value: string.Empty);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteElementString(localName: "program-used", ns: fb2Ns, value: "Planetoid-DB");
			string fb2DateString = DateTime.Now.ToString(format: "yyyy-MM-dd");
			xmlWriter.WriteStartElement(localName: "date", ns: fb2Ns);
			xmlWriter.WriteAttributeString(localName: "value", value: fb2DateString);
			xmlWriter.WriteString(text: fb2DateString);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteElementString(localName: "id", ns: fb2Ns, value: Guid.NewGuid().ToString());
			xmlWriter.WriteElementString(localName: "version", ns: fb2Ns, value: "1.0");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement(localName: "body", ns: fb2Ns);
			xmlWriter.WriteStartElement(localName: "title", ns: fb2Ns);
			xmlWriter.WriteElementString(localName: "p", ns: fb2Ns, value: title);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement(localName: "section", ns: fb2Ns);
			xmlWriter.WriteStartElement(localName: "table", ns: fb2Ns);
			xmlWriter.WriteStartElement(localName: "tr", ns: fb2Ns);
			foreach (string h in headers)
			{
				// Write the column headers as table header elements (<th>) in the FB2 XML document. Each header is written within a <tr> element, and special characters in the headers are properly escaped to ensure that the resulting XML is well-formed.
				xmlWriter.WriteElementString(localName: "th", ns: fb2Ns, value: h);
			}
			xmlWriter.WriteEndElement();
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				// Write each data row as a table row (<tr>) with cells (<td>) in the FB2 XML document. Each cell is written within a <tr> element, and special characters in the cell data are properly escaped. If a row has fewer cells than headers, the missing cells are treated as empty strings.
				xmlWriter.WriteStartElement(localName: "tr", ns: fb2Ns);
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					xmlWriter.WriteElementString(localName: "td", ns: fb2Ns, value: cell);
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			// Show a success message after the file has been saved.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "FictionBook2", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as a Compiled HTML Help (CHM) file.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">The CHM title used in HTML content and the HHP project file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>Requires Microsoft HTML Help Workshop (<c>hhc.exe</c>) to be installed. If it is not found,
	/// an error message is shown and no file is written.</remarks>
	public static void SaveAsChm(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, generate the necessary HTML, HHC, and HHP files in a temporary directory, and invoke the HTML Help Workshop compiler (hhc.exe) to create the CHM file. The method checks for the presence of hhc.exe in the default installation path and shows an error message if it is not found. If hhc.exe is available, it creates a temporary directory to store the intermediate files, generates an index.html file with the TableLayoutPanel data formatted as a table, a toc.hhc file for the table of contents, and a project.hhp file for the project configuration. It then runs hhc.exe with the project file to compile the CHM. If compilation is successful and the CHM file is created, it copies the resulting CHM to the specified output path. Finally, it cleans up the temporary directory and shows a success message or an error message if compilation fails.
		string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
		string hhcPath = Path.Combine(
			path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ProgramFilesX86),
			path2: @"HTML Help Workshop\hhc.exe");
		if (!File.Exists(path: hhcPath))
		{
			_ = MessageBox.Show(
				text: "Microsoft HTML Help Workshop is not installed or not found at the default location. Cannot compile CHM file.",
				caption: I18nStrings.ErrorCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
			return;
		}
		// Create a temporary directory to store the intermediate files needed for CHM compilation. The directory is created in the system's temporary path with a unique name generated using a GUID. After the compilation process, the temporary directory and its contents will be deleted to clean up any intermediate files.
		string tempDir = Path.Combine(path1: Path.GetTempPath(), path2: Guid.NewGuid().ToString());
		Directory.CreateDirectory(path: tempDir);
		// Generate the index.html file with the TableLayoutPanel data formatted as a table, the toc.hhc file for the table of contents, and the project.hhp file for the project configuration. The index.html file includes a title and a table with the column headers and data rows. The toc.hhc file defines a single entry in the table of contents that points to index.html. The project.hhp file specifies the options for CHM compilation, including the title, default topic, and contents file.
		try
		{
			// Define the paths for the intermediate files in the temporary directory.
			string htmlPath = Path.Combine(path1: tempDir, path2: "index.html");
			string hhcFilePath = Path.Combine(path1: tempDir, path2: "toc.hhc");
			string hhpPath = Path.Combine(path1: tempDir, path2: "project.hhp");
			string chmTempPath = Path.Combine(path1: tempDir, path2: "project.chm");
			// Write the index.html file with the TableLayoutPanel data formatted as a table. The HTML includes a title, a heading with the title, and a table with the column headers and data rows. Special characters in the title, headers, and cell data are encoded using HTML encoding to ensure that the resulting HTML is well-formed and can be rendered correctly by the CHM compiler and viewers.
			using (StreamWriter writer = new(path: htmlPath, append: false, encoding: Encoding.UTF8))
			{
				string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
				writer.WriteLine(value: $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>{safeTitle}</title>");
				writer.WriteLine(value: "<style>table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid #000;padding:5px;text-align:left}}th{{background-color:#f2f2f2}}</style></head><body>");
				writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
				writer.Write(value: "<table><tr>");
				foreach (string h in headers)
				{
					writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
				}
				writer.WriteLine(value: "</tr>");
				foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
				{
					writer.Write(value: "<tr>");
					for (int c = 0; c < headers.Length; c++)
					{
						string cell = c < row.Length ? row[c] : string.Empty;
						writer.Write(value: $"<td>{System.Net.WebUtility.HtmlEncode(value: cell)}</td>");
					}
					writer.WriteLine(value: "</tr>");
				}
				writer.WriteLine(value: "</table></body></html>");
			}
			using (StreamWriter writer = new(path: hhcFilePath, append: false, encoding: Encoding.ASCII))
			{
				writer.WriteLine(value: "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">");
				writer.WriteLine(value: "<HTML><HEAD><meta name=\"GENERATOR\" content=\"Planetoid-DB\"></HEAD><BODY>");
				writer.WriteLine(value: "<OBJECT type=\"text/site properties\"><param name=\"ImageType\" value=\"Folder\"></OBJECT>");
				writer.WriteLine(value: "<UL><LI><OBJECT type=\"text/sitemap\">");
				writer.WriteLine(value: $"<param name=\"Name\" value=\"{System.Net.WebUtility.HtmlEncode(value: title)}\">");
				writer.WriteLine(value: "<param name=\"Local\" value=\"index.html\">");
				writer.WriteLine(value: "</OBJECT></UL></BODY></HTML>");
			}
			using (StreamWriter writer = new(path: hhpPath, append: false, encoding: Encoding.ASCII))
			{
				writer.WriteLine(value: "[OPTIONS]");
				writer.WriteLine(value: "Compatibility=1.1 or later");
				writer.WriteLine(value: "Compiled file=project.chm");
				writer.WriteLine(value: "Contents file=toc.hhc");
				writer.WriteLine(value: "Default topic=index.html");
				writer.WriteLine(value: "Display compile progress=No");
				writer.WriteLine(value: "Language=0x409 English (United States)");
				writer.WriteLine(value: $"Title={title}");
				writer.WriteLine(value: string.Empty);
				writer.WriteLine(value: "[FILES]");
				writer.WriteLine(value: "index.html");
			}
			ProcessStartInfo startInfo = new()
			{
				FileName = hhcPath,
				Arguments = $"\"{hhpPath}\"",
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false
			};
			using (Process? process = Process.Start(startInfo: startInfo))
			{
				process?.WaitForExit();
			}
			if (File.Exists(path: chmTempPath))
			{
				File.Copy(sourceFileName: chmTempPath, destFileName: fileName, overwrite: true);
				// Show a success message after the CHM file has been successfully created.
				ShowSuccess();
			}
			else
			{
				// If the CHM file was not created, show an error message to the user.
				_ = MessageBox.Show(
					text: "Failed to compile the CHM file.",
					caption: I18nStrings.ErrorCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Error);
			}
		}
		// Catch any exceptions that occur during the file generation and compilation process, log the error, and show an error message to the user.
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "CHM", filePath: fileName);
		}
		// Finally, clean up the temporary directory used for intermediate files.
		finally
		{
			if (Directory.Exists(path: tempDir))
			{
				Directory.Delete(path: tempDir, recursive: true);
			}
		}
	}

	/// <summary>Saves the contents of <paramref name="tableLayoutPanel"/> as an XML Paper Specification (XPS) document.</summary>
	/// <param name="tableLayoutPanel">The <see cref="TableLayoutPanel"/> containing the data to export.</param>
	/// <param name="title">Written as the page heading on each XPS page.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed XPS (ZIP) archive adhering to the Open Packaging Convention.</remarks>
	public static void SaveAsXps(TableLayoutPanel tableLayoutPanel, string title, string fileName)
	{
		// Get the column headers and rows, and write the output file in XML Paper Specification (XPS) format. The method constructs a valid XPS document as a ZIP archive adhering to the Open Packaging Convention. It creates the necessary entries for content types, relationships, and fixed document sequence, as well as the fixed page containing the TableLayoutPanel data formatted as a table. The column headers are written as text elements, and each data row is written as a series of text elements positioned appropriately on the page. Special characters in the title, headers, and cell data are properly escaped to ensure that the resulting XPS document is well-formed and can be opened with XPS-compatible viewers without issues.
		try
		{
			// Get the column headers from the TableLayoutPanel.
			string[] headers = GetHeaders(tableLayoutPanel: tableLayoutPanel);
			// Create a ZIP archive for the XPS file and write the necessary entries for content types, relationships, fixed document sequence, and fixed page. The content types entry defines the MIME types for the various file extensions used in the XPS package. The relationships entries define the relationships between the different parts of the XPS document, such as the relationship from the root to the fixed document sequence and from the fixed document sequence to the fixed document. The fixed page entry contains the actual content of the page, including the title and a table with the column headers and data rows. The text elements are positioned on the page using appropriate coordinates to create a readable layout.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the [Content_Types].xml entry that defines the MIME types for the file extensions used in the XPS package. This entry is required for the Open Packaging Convention and specifies the content types for relationships, fixed document sequences, fixed documents, fixed pages, and fonts.
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
				writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"fdseq\" ContentType=\"application/vnd.ms-package.xps-fixeddocumentsequence+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"fdoc\" ContentType=\"application/vnd.ms-package.xps-fixeddocument+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"fpage\" ContentType=\"application/vnd.ms-package.xps-fixedpage+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"ttf\" ContentType=\"application/vnd.ms-package.obfuscated-opentype\"/>");
				writer.WriteLine(value: "</Types>");
			}
			// Create the relationships entry at the root of the package that defines the relationship to the fixed document sequence. This entry is required for the Open Packaging Convention and specifies that the fixed document sequence is a required resource for the XPS document.
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"/FixedDocSeq.fdseq\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the relationships entry for the fixed document sequence that defines the relationship to the fixed document. This entry specifies that the fixed document is a required resource for the fixed document sequence.
			ZipArchiveEntry fdseqRelsEntry = archive.CreateEntry(entryName: "_rels/FixedDocSeq.fdseq.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: fdseqRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"../../../Resources/Dummy.ttf\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the fixed document sequence entry that references the fixed document. This entry defines the structure of the XPS document and specifies that the fixed document is part of the fixed document sequence.
			ZipArchiveEntry fdseqEntry = archive.CreateEntry(entryName: "FixedDocSeq.fdseq", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: fdseqEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<FixedDocumentSequence xmlns=\"http://schemas.microsoft.com/xps/2005/06\">");
				writer.WriteLine(value: "  <DocumentReference Source=\"/Documents/1/FixedDoc.fdoc\"/>");
				writer.WriteLine(value: "</FixedDocumentSequence>");
			}
			const int pageHeight = 1056;
			const int startY = 96;
			const int marginB = 960;
			const int lineHeight = 16;
			int usableWidth = 624;
			int colWidth = headers.Length > 0 ? usableWidth / headers.Length : usableWidth;
			int[] colX = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				colX[c] = 96 + (c * colWidth);
			}
			List<string> pageEntries = [];
			int pageNumber = 1;
			int currentY = startY;
			StringBuilder currentPageBuilder = new();
			// Define local functions to start a new page and finish the current page. The StartNewPage function initializes the currentPageBuilder with the XML content for a new fixed page, including the title and column headers. The FinishCurrentPage function finalizes the current page by closing the XML tags, creating a new entry in the ZIP archive for the page, and writing the page content to that entry. It also creates a relationships entry for the page that references the font resource used in the page.
			void StartNewPage()
			{
				currentPageBuilder.Clear();
				currentPageBuilder.AppendLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				currentPageBuilder.AppendLine(value: "<FixedPage Width=\"816\" Height=\"1056\" xmlns=\"http://schemas.microsoft.com/xps/2005/06\" xml:lang=\"en-US\">");
				string safeTitle = System.Security.SecurityElement.Escape(str: title) ?? string.Empty;
				int titleY = Math.Max(val1: 0, val2: currentY - 24);
				currentPageBuilder.AppendLine(value: $"  <Glyphs Fill=\"#FF000000\" FontUri=\"/Resources/Dummy.ttf\" DeviceFontName=\"Arial\" FontRenderingEmSize=\"14\" OriginX=\"96\" OriginY=\"{titleY}\" UnicodeString=\"{safeTitle} - Page {pageNumber}\"/>");
				for (int c = 0; c < headers.Length; c++)
				{
					string safeH = System.Security.SecurityElement.Escape(str: headers[c]) ?? string.Empty;
					currentPageBuilder.AppendLine(value: $"  <Glyphs Fill=\"#FF000000\" FontUri=\"/Resources/Dummy.ttf\" DeviceFontName=\"Arial\" FontRenderingEmSize=\"12\" OriginX=\"{colX[c]}\" OriginY=\"{currentY}\" UnicodeString=\"{safeH}\"/>");
				}
				currentY += lineHeight * 2;
			}
			// The FinishCurrentPage function finalizes the current page by closing the XML tags, creating a new entry in the ZIP archive for the page, and writing the page content to that entry. It also creates a relationships entry for the page that references the font resource used in the page.
			void FinishCurrentPage()
			{
				currentPageBuilder.AppendLine(value: "</FixedPage>");
				string pageName = $"{pageNumber}.fpage";
				string pagePath = $"Documents/1/Pages/{pageName}";
				pageEntries.Add(item: pageName);
				ZipArchiveEntry pageEntry = archive.CreateEntry(entryName: pagePath, compressionLevel: CompressionLevel.Optimal);
				using StreamWriter writer = new(stream: pageEntry.Open(), encoding: Encoding.UTF8);
				writer.Write(value: currentPageBuilder.ToString());
				ZipArchiveEntry pageRelsEntry = archive.CreateEntry(entryName: $"Documents/1/Pages/_rels/{pageName}.rels", compressionLevel: CompressionLevel.Optimal);
				using StreamWriter relsWriter = new(stream: pageRelsEntry.Open(), encoding: Encoding.UTF8);
				relsWriter.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				relsWriter.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				relsWriter.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"../../../Resources/Dummy.ttf\"/>");
				relsWriter.WriteLine(value: "</Relationships>");
			}
			//Iterate through the rows of the TableLayoutPanel and write them to the current page. If the current Y position exceeds the bottom margin, finish the current page and start a new one. Each cell is written as a Glyphs element with appropriate positioning on the page. Special characters in the cell data are escaped to ensure that the XML is well-formed.
			StartNewPage();
			// The GetRows method is called to retrieve the data rows of the TableLayoutPanel (rows 1 onward). Each row is processed to write its cells as Glyphs elements on the current page. If the current Y position exceeds the defined bottom margin, the current page is finalized and a new page is started to continue writing the remaining rows.
			foreach (string[] row in GetRows(tableLayoutPanel: tableLayoutPanel))
			{
				if (currentY > marginB)
				{
					FinishCurrentPage();
					pageNumber++;
					currentY = startY;
					StartNewPage();
				}
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					string safeCell = System.Security.SecurityElement.Escape(str: cell) ?? string.Empty;
					if (!string.IsNullOrEmpty(value: safeCell))
					{
						currentPageBuilder.AppendLine(value: $"  <Glyphs Fill=\"#FF000000\" FontUri=\"/Resources/Dummy.ttf\" DeviceFontName=\"Arial\" FontRenderingEmSize=\"10\" OriginX=\"{colX[c]}\" OriginY=\"{currentY}\" UnicodeString=\"{safeCell}\"/>");
					}
				}
				currentY += lineHeight;
			}
			// Finalize the last page.
			FinishCurrentPage();
			// Create the relationships entry for the fixed document.
			ZipArchiveEntry fdocRelsEntry = archive.CreateEntry(entryName: "Documents/1/_rels/FixedDoc.fdoc.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: fdocRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				for (int p = 1; p <= pageNumber; p++)
				{
					writer.WriteLine(value: $"  <Relationship Id=\"p{p}\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"/Documents/1/Pages/{p}.fpage\"/>");
				}
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the fixed document entry that references the page entries. The fixed document XML includes a PageContent element for each page, referencing the corresponding page entry in the ZIP archive. This structure adheres to the XPS specification and allows XPS viewers to correctly render the document with multiple pages.
			ZipArchiveEntry fdocEntry = archive.CreateEntry(entryName: "Documents/1/FixedDoc.fdoc", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: fdocEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<FixedDocument xmlns=\"http://schemas.microsoft.com/xps/2005/06\">");
				for (int p = 1; p <= pageNumber; p++)
				{
					writer.WriteLine(value: $"  <PageContent Source=\"/Documents/1/Pages/{p}.fpage\"/>");
				}
				writer.WriteLine(value: "</FixedDocument>");
			}
			// Create a dummy font entry to satisfy the requirement for a font resource in the XPS document. The entry is created with no compression and contains a simple string as its content. This allows the Glyphs elements in the fixed pages to reference a valid font resource, ensuring that the XPS document can be opened without errors in XPS viewers.
			ZipArchiveEntry fontEntry = archive.CreateEntry(entryName: "Resources/Dummy.ttf", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: fontEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "DUMMY");
			}
			// Show a success message after the XPS file has been successfully created.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "XPS", filePath: fileName);
		}
	}

	#endregion
}