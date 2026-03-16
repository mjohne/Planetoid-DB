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

/// <summary>Provides static methods for saving the contents of a <see cref="ListView"/> to various file formats.</summary>
/// <remarks>Each method accepts the source <see cref="ListView"/>, a document title used in the file content,
/// and the full file-system path of the output file. Column headers are read from the ListView column collection;
/// row data is read from either the normal items collection or (in virtual mode) by requesting items via the
/// <see cref="ListView.RetrieveVirtualItem"/> event handler of the owning form. Compressed file formats
/// (DOCX, ODT, ODS, XLSX, EPUB) are written as proper ZIP archives rather than flat XML files.
/// SQLite export requires System.Data.SQLite; CHM export requires Microsoft HTML Help Workshop (hhc.exe).</remarks>
public static class ListViewExporter
{
	/// <summary>NLog logger for logging messages and errors.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	#region Private helpers

	/// <summary>Returns the column header texts of the given <see cref="ListView"/>.</summary>
	/// <param name="listView">The list view whose columns to read.</param>
	/// <returns>An array of header strings in column order.</returns>
	private static string[] GetHeaders(ListView listView)
	{
		string[] headers = new string[listView.Columns.Count];
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			headers[i] = listView.Columns[index: i].Text;
		}
		return headers;
	}

	/// <summary>Enumerates each row of the <see cref="ListView"/> as an array of cell strings.</summary>
	/// <param name="listView">The list view to read from.</param>
	/// <returns>An enumerable of string arrays, one per row.</returns>
	/// <remarks>Works for both normal and virtual-mode list views. In virtual mode the
	/// <see cref="ListView.RetrieveVirtualItem"/> event of the owning form is triggered for each item.</remarks>
	private static IEnumerable<string[]> GetRows(ListView listView)
	{
		int count = listView.VirtualMode ? listView.VirtualListSize : listView.Items.Count;
		for (int i = 0; i < count; i++)
		{
			ListViewItem item = listView.Items[index: i];
			string[] row = new string[item.SubItems.Count];
			for (int j = 0; j < item.SubItems.Count; j++)
			{
				row[j] = item.SubItems[index: j].Text;
			}
			yield return row;
		}
	}

	/// <summary>Escapes LaTeX special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for LaTeX output.</returns>
	private static string EscapeLatex(string? input)
	{
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		StringBuilder builder = new(capacity: input.Length);
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
		return builder.ToString();
	}

	/// <summary>Escapes Markdown table cell characters.</summary>
	/// <param name="value">The raw cell value.</param>
	/// <returns>The escaped string.</returns>
	private static string EscapeMarkdownCell(string? value)
	{
		return string.IsNullOrEmpty(value: value) ? string.Empty : value.Replace(oldValue: "|", newValue: "\\|");
	}

	/// <summary>Escapes PostScript string literal characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string.</returns>
	private static string EscapePostScript(string? input)
	{
		return string.IsNullOrEmpty(value: input)
			? string.Empty
			: input.Replace(oldValue: "\\", newValue: "\\\\")
				   .Replace(oldValue: "(", newValue: "\\(")
				   .Replace(oldValue: ")", newValue: "\\)");
	}

	/// <summary>Escapes PDF string literal characters.</summary>
	/// <param name="text">The raw input string.</param>
	/// <returns>The escaped string.</returns>
	private static string EscapePdf(string? text)
	{
		if (string.IsNullOrEmpty(value: text))
		{
			return string.Empty;
		}
		StringBuilder builder = new(capacity: text.Length);
		foreach (char ch in text)
		{
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
		return builder.ToString();
	}

	/// <summary>Escapes RTF special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string.</returns>
	private static string EscapeRtf(string? input)
	{
		if (string.IsNullOrEmpty(value: input))
		{
			return string.Empty;
		}
		StringBuilder builder = new(capacity: input.Length);
		foreach (char ch in input)
		{
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
		return builder.ToString();
	}

	/// <summary>Escapes a CSV field by doubling internal quotes and wrapping in double quotes.</summary>
	/// <param name="field">The raw field value.</param>
	/// <returns>The escaped CSV field.</returns>
	private static string EscapeCsvField(string? field)
	{
		string safeField = field ?? string.Empty;
		safeField = safeField.Replace(oldValue: "\"", newValue: "\"\"");
		return $"\"{safeField}\"";
	}

	/// <summary>Escapes a TOML string value.</summary>
	/// <param name="value">The raw value.</param>
	/// <returns>The escaped TOML string value.</returns>
	private static string EscapeToml(string? value)
	{
		return string.IsNullOrEmpty(value: value)
			? string.Empty
			: value.Replace(oldValue: "\\", newValue: "\\\\")
				   .Replace(oldValue: "\"", newValue: "\\\"");
	}

	/// <summary>Shows a success message after a file has been saved.</summary>
	private static void ShowSuccess()
	{
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
	private static void ShowError(Exception ex, string format, string filePath)
	{
		logger.Error(exception: ex, message: $"Error saving as {format} to '{{FilePath}}'.", args: filePath);
		_ = MessageBox.Show(
			text: $"Error saving as {format}: {ex.Message}",
			caption: I18nStrings.ErrorCaption,
			buttons: MessageBoxButtons.OK,
			icon: MessageBoxIcon.Error);
	}

	#endregion

	#region Save methods
	/// <summary>Saves the contents of <paramref name="listView"/> as a plain-text file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a heading at the top of the file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsText(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '-', count: title.Length));
			writer.WriteLine(value: string.Join(separator: "\t", values: headers));
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: "\t", values: row));
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Text", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a LaTeX document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title used in the LaTeX caption.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsLatex(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			string colSpec = string.Join(separator: "|", values: Enumerable.Repeat(element: "l", count: headers.Length));
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "\\documentclass{article}");
			writer.WriteLine(value: "\\usepackage[utf8]{inputenc}");
			writer.WriteLine(value: "\\begin{document}");
			writer.WriteLine(value: "\\begin{table}[h!]");
			writer.WriteLine(value: "\\centering");
			writer.WriteLine(value: $"\\begin{{tabular}}{{|{colSpec}|}}");
			writer.WriteLine(value: "\\hline");
			writer.WriteLine(value: string.Join(separator: " & ", values: headers.Select(selector: EscapeLatex)) + " \\\\");
			writer.WriteLine(value: "\\hline");
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: " & ", values: row.Select(selector: EscapeLatex)) + " \\\\");
			}
			writer.WriteLine(value: "\\hline");
			writer.WriteLine(value: "\\end{tabular}");
			writer.WriteLine(value: $"\\caption{{{EscapeLatex(input: title)}}}");
			writer.WriteLine(value: "\\end{table}");
			writer.WriteLine(value: "\\end{document}");
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "LaTeX", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Markdown table.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsMarkdown(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"# {title}");
			writer.WriteLine();
			writer.WriteLine(value: "| " + string.Join(separator: " | ", values: headers) + " |");
			writer.WriteLine(value: "| " + string.Join(separator: " | ", values: headers.Select(selector: static _ => ":---")) + " |");
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: "| " + string.Join(separator: " | ", values: row.Select(selector: EscapeMarkdownCell)) + " |");
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Markdown", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an AsciiDoc document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as the first-level heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsAsciiDoc(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"= {title}");
			writer.WriteLine();
			writer.WriteLine(value: "[options=\"header\"]");
			writer.WriteLine(value: "|===");
			writer.WriteLine(value: "|" + string.Join(separator: "|", values: headers));
			foreach (string[] row in GetRows(listView: listView))
			{
				string[] escaped = row.Select(selector: static v => v.Replace(oldValue: "|", newValue: "\\|")).ToArray();
				writer.WriteLine(value: "|" + string.Join(separator: "|", values: escaped));
			}
			writer.WriteLine(value: "|===");
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AsciiDoc", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a reStructuredText document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as the main heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsReStructuredText(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			List<string[]> rows = GetRows(listView: listView).ToList();
			int[] widths = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				widths[c] = headers[c].Length + 2;
			}
			foreach (string[] row in rows)
			{
				for (int c = 0; c < Math.Min(val1: row.Length, val2: headers.Length); c++)
				{
					int w = row[c].Length + 2;
					if (w > widths[c])
					{
						widths[c] = w;
					}
				}
			}
			string separator = "+" + string.Join(separator: "+", values: widths.Select(selector: w => new string(c: '-', count: w))) + "+";
			string headerSep = "+" + string.Join(separator: "+", values: widths.Select(selector: w => new string(c: '=', count: w))) + "+";
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine();
			writer.WriteLine(value: separator);
			string headerRow = "|" + string.Join(separator: "|", values: headers.Select(selector: (h, i) => $" {h.PadRight(totalWidth: widths[i] - 1)}")) + "|";
			writer.WriteLine(value: headerRow);
			writer.WriteLine(value: headerSep);
			foreach (string[] row in rows)
			{
				string dataRow = "|" + string.Join(separator: "|", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return $" {cell.PadRight(totalWidth: widths[c] - 1)}";
				})) + "|";
				writer.WriteLine(value: dataRow);
				writer.WriteLine(value: separator);
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "reStructuredText", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Textile document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsTextile(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"h1. {title}");
			writer.WriteLine();
			writer.WriteLine(value: "|_. " + string.Join(separator: " |_. ", values: headers) + " |");
			foreach (string[] row in GetRows(listView: listView))
			{
				string[] escaped = row.Select(selector: static v => v.Replace(oldValue: "|", newValue: "&#124;")).ToArray();
				writer.WriteLine(value: "| " + string.Join(separator: " | ", values: escaped) + " |");
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Textile", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Microsoft Word document (DOCX).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a styled paragraph at the top.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed DOCX (ZIP/Open XML) archive, not a flat XML file.</remarks>
	public static void SaveAsWord(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
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
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"word/document.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
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
				foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Word", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an OpenDocument Text file (ODT).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed ODT (ZIP) archive, not a flat XML file.</remarks>
	public static void SaveAsOdt(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.text");
			}
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
				foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODT", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Rich Text Format (RTF) file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a bold heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsRtf(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			writer.WriteLine(value: @"{\rtf1\ansi\deff0");
			writer.WriteLine(value: @"{\fonttbl{\f0 Arial;}}");
			writer.WriteLine(value: @"\f0\fs20");
			writer.WriteLine(value: $@"{{\pard\b\fs24 {EscapeRtf(input: title)}\par\par}}");
			foreach (string[] row in GetRows(listView: listView))
			{
				int cumWidth = 0;
				writer.Write(value: @"\trowd\trgaph108\trleft-108");
				for (int c = 0; c < headers.Length; c++)
				{
					cumWidth += 1440;
					writer.Write(value: $@"\cellx{cumWidth}");
				}
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "RTF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an AbiWord document (ABW).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 paragraph.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsAbiword(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
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
				string safeH = System.Net.WebUtility.HtmlEncode(value: headers[c]) ?? string.Empty;
				writer.WriteLine(value: $"      <cell left-attach=\"{c}\" right-attach=\"{c + 1}\" top-attach=\"{rowIdx}\" bottom-attach=\"{rowIdx + 1}\">");
				writer.WriteLine(value: $"        <p>{safeH}</p>");
				writer.WriteLine(value: "      </cell>");
			}
			rowIdx++;
			foreach (string[] dataRow in GetRows(listView: listView))
			{
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AbiWord", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a WPS Writer document (WPS).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written as a level-1 heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>WPS Writer natively supports HTML-based content; the file is saved in HTML format internally.</remarks>
	public static void SaveAsWps(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
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
				writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			writer.WriteLine(value: "</tr>");
			foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "WPS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Microsoft Excel workbook (XLSX).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title (used as a comment; the sheet is named "Data").</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed XLSX (ZIP/Open XML) archive.</remarks>
	public static void SaveAsExcel(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
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
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			ZipArchiveEntry workbookEntry = archive.CreateEntry(entryName: "xl/workbook.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: workbookEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
				writer.WriteLine(value: "  <sheets><sheet name=\"Data\" sheetId=\"1\" r:id=\"rId1\"/></sheets>");
				writer.WriteLine(value: "</workbook>");
			}
			ZipArchiveEntry wbRelsEntry = archive.CreateEntry(entryName: "xl/_rels/workbook.xml.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: wbRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
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
				foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Excel", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an OpenDocument Spreadsheet (ODS).</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title used as the spreadsheet table name.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed ODS (ZIP) archive, not a flat XML file.</remarks>
	public static void SaveAsOds(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.spreadsheet");
			}
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
				foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a CSV file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsCsv(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: ",", values: headers.Select(selector: EscapeCsvField)));
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: ",", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return EscapeCsvField(field: cell);
				})));
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "CSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a TSV file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsTsv(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: "\t", values: headers));
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: "\t", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c => c < row.Length ? row[c] : string.Empty)));
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a PSV file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsPsv(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: "|", values: headers));
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: "|", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c => c < row.Length ? row[c] : string.Empty)));
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a WPS Spreadsheet (ET) file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Not written to the file body; reserved for future use.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>ET files use CSV format for WPS Spreadsheet compatibility.</remarks>
	public static void SaveAsEt(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: string.Join(separator: ",", values: headers.Select(selector: EscapeCsvField)));
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: string.Join(separator: ",", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return EscapeCsvField(field: cell);
				})));
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ET", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an HTML file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written in the &lt;title&gt; element and as an &lt;h1&gt; heading.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsHtml(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
			writer.WriteLine(value: "<!DOCTYPE html>");
			writer.WriteLine(value: $"<html lang=\"en\"><head><meta charset=\"utf-8\"><title>{safeTitle}</title>");
			writer.WriteLine(value: "<style>body{{font-family:sans-serif}}table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid #ccc;padding:6px;text-align:left}}th{{background:#f2f2f2}}</style>");
			writer.WriteLine(value: "</head><body>");
			writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
			writer.Write(value: "<table><thead><tr>");
			foreach (string h in headers)
			{
				writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			writer.WriteLine(value: "</tr></thead><tbody>");
			foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "HTML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an XML file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as a <c>title</c> attribute on the root element.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsXml(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "data");
			xmlWriter.WriteAttributeString(localName: "title", value: title);
			foreach (string[] row in GetRows(listView: listView))
			{
				xmlWriter.WriteStartElement(localName: "row");
				for (int c = 0; c < headers.Length; c++)
				{
					string elementName = headers[c].Length > 0
						? System.Xml.XmlConvert.EncodeName(name: headers[c]) ?? $"col{c}"
						: $"col{c}";
					string cell = c < row.Length ? row[c] : string.Empty;
					xmlWriter.WriteElementString(localName: elementName, value: cell);
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "XML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a DocBook XML document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The document title written in the &lt;title&gt; element.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsDocBook(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "article", ns: "http://docbook.org/ns/docbook");
			xmlWriter.WriteAttributeString(localName: "version", value: "5.0");
			xmlWriter.WriteElementString(localName: "title", value: title);
			xmlWriter.WriteStartElement(localName: "section");
			xmlWriter.WriteStartElement(localName: "table");
			xmlWriter.WriteAttributeString(localName: "frame", value: "all");
			xmlWriter.WriteElementString(localName: "title", value: title);
			xmlWriter.WriteStartElement(localName: "tgroup");
			xmlWriter.WriteAttributeString(localName: "cols", value: headers.Length.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
			for (int c = 0; c < headers.Length; c++)
			{
				xmlWriter.WriteStartElement(localName: "colspec");
				xmlWriter.WriteAttributeString(localName: "colname", value: $"c{c + 1}");
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteStartElement(localName: "thead");
			xmlWriter.WriteStartElement(localName: "row");
			foreach (string h in headers)
			{
				xmlWriter.WriteElementString(localName: "entry", value: h);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement(localName: "tbody");
			foreach (string[] row in GetRows(listView: listView))
			{
				xmlWriter.WriteStartElement(localName: "row");
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "DocBook", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a JSON file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> property at the root of the JSON object.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsJson(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			List<Dictionary<string, string>> records = [];
			foreach (string[] row in GetRows(listView: listView))
			{
				Dictionary<string, string> record = [];
				for (int c = 0; c < headers.Length; c++)
				{
					record[key: headers[c]] = c < row.Length ? row[c] : string.Empty;
				}
				records.Add(item: record);
			}
			var doc = new { title, rows = records };
			string json = JsonSerializer.Serialize(value: doc, options: new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(path: fileName, contents: json);
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "JSON", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a YAML file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> key at the root of the YAML document.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsYaml(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "---");
			writer.WriteLine(value: $"title: \"{title.Replace(oldValue: "\"", newValue: "\\\"")}\"");
			writer.WriteLine(value: $"created_at: \"{DateTime.UtcNow:O}\"");
			writer.WriteLine(value: "rows:");
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: "  - item:");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					string safeCell = cell.Replace(oldValue: "\"", newValue: "\\\"");
					string safeKey = headers[c].Replace(oldValue: "\"", newValue: "\\\"");
					writer.WriteLine(value: $"      {safeKey}: \"{safeCell}\"");
				}
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "YAML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a TOML file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as the value of a <c>title</c> key at the top of the file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsToml(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"title = \"{EscapeToml(value: title)}\"");
			writer.WriteLine(value: $"created_at = {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
			writer.WriteLine();
			foreach (string[] row in GetRows(listView: listView))
			{
				writer.WriteLine(value: "[[rows]]");
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.WriteLine(value: $"{EscapeToml(value: headers[c])} = \"{EscapeToml(value: cell)}\"");
				}
				writer.WriteLine();
			}
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TOML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a SQL INSERT script.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Used as the SQL table name in the CREATE TABLE and INSERT statements.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsSql(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			string tableName = new(value: title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_').ToArray());
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
			foreach (string[] row in GetRows(listView: listView))
			{
				string values = string.Join(separator: ", ", values: Enumerable.Range(start: 0, count: headers.Length).Select(selector: c =>
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					return $"'{cell.Replace(oldValue: "'", newValue: "''")}'";	
				}));
				writer.WriteLine(value: $"INSERT INTO [{tableName}] ({colList}) VALUES ({values});");
			}
			writer.WriteLine(value: "COMMIT;");
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "SQL", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a SQLite database file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Used as the table name inside the SQLite database.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsSqlite(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			string tableName = new(value: title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_').ToArray());
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
				parameters[c] = insertCmd.Parameters.Add(parameterName: $"@p{c}", parameterType: System.Data.DbType.String);
			}
			foreach (string[] row in GetRows(listView: listView))
			{
				for (int c = 0; c < headers.Length; c++)
				{
					parameters[c].Value = c < row.Length ? row[c] : string.Empty;
				}
				insertCmd.ExecuteNonQuery();
			}
			transaction.Commit();
			connection.Close();
			ShowSuccess();
		}
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "SQLite", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a PDF document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as the document heading on each page.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsPdf(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
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
				w.WriteLine(value: $"1 0 0 1 {colX[c]} {pageHeight - 60} Tm ({EscapePdf(text: headers[c])}) Tj");
			}
			currentY = startY - 30;
			foreach (string[] row in GetRows(listView: listView))
			{
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PDF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a PostScript file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">Written as the page heading on each PostScript page.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsPostScript(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			const int pageHeight = 842;
			const int marginTop = 50;
			const int marginBottom = 50;
			const int startY = pageHeight - marginTop;
			const int lineHeight = 14;
			int currentY = startY;
			int pageNumber = 1;
			int usableWidth = 500;
			int colWidth = headers.Length > 0 ? usableWidth / headers.Length : usableWidth;
			int[] colX = new int[headers.Length];
			for (int c = 0; c < headers.Length; c++)
			{
				colX[c] = 50 + (c * colWidth);
			}
			void WritePageHeader(int pg)
			{
				writer.WriteLine(value: $"%%Page: {pg} {pg}");
				writer.WriteLine(value: "/Helvetica-Bold findfont 12 scalefont setfont");
				writer.WriteLine(value: $"50 {pageHeight - 30} moveto ({EscapePostScript(input: title)} - Page {pg}) show");
				writer.WriteLine(value: "/Helvetica findfont 10 scalefont setfont");
				for (int c = 0; c < headers.Length; c++)
				{
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
			foreach (string[] row in GetRows(listView: listView))
			{
				if (currentY < marginBottom)
				{
					writer.WriteLine(value: "showpage");
					pageNumber++;
					WritePageHeader(pg: pageNumber);
					currentY = startY - 30;
				}
				for (int c = 0; c < headers.Length; c++)
				{
					string cell = c < row.Length ? row[c] : string.Empty;
					writer.WriteLine(value: $"{colX[c]} {currentY} moveto ({EscapePostScript(input: cell)}) show");
				}
				currentY -= lineHeight;
			}
			writer.WriteLine(value: "showpage");
			writer.WriteLine(value: "%%Trailer");
			writer.WriteLine(value: $"%%Pages: {pageNumber}");
			writer.WriteLine(value: "%%EOF");
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PostScript", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as an EPUB file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The EPUB book title used in metadata and content pages.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>The file is a proper compressed EPUB (ZIP) archive conforming to the EPUB 2 specification.</remarks>
	public static void SaveAsEpub(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/epub+zip");
			}
			ZipArchiveEntry containerEntry = archive.CreateEntry(entryName: "META-INF/container.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: containerEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\"?>");
				writer.WriteLine(value: "<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">");
				writer.WriteLine(value: "  <rootfiles><rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/></rootfiles>");
				writer.WriteLine(value: "</container>");
			}
			string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
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
				foreach (string[] row in GetRows(listView: listView))
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "EPUB", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a MOBI file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The book title embedded in the MOBI header and HTML content body.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsMobi(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
			StringBuilder html = new();
			html.Append(value: $"<html><head><meta charset=\"UTF-8\"><title>{System.Net.WebUtility.HtmlEncode(value: title)}</title></head><body>");
			html.Append(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			html.Append(value: "<table><tr>");
			foreach (string h in headers)
			{
				html.Append(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
			}
			html.Append(value: "</tr>");
			foreach (string[] row in GetRows(listView: listView))
			{
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "MOBI", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a FictionBook 2 (FB2) XML document.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The book title written in the FB2 metadata and body sections.</param>
	/// <param name="fileName">The full path of the output file.</param>
	public static void SaveAsFictionBook2(ListView listView, string title, string fileName)
	{
		try
		{
			string[] headers = GetHeaders(listView: listView);
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
				xmlWriter.WriteElementString(localName: "th", ns: fb2Ns, value: h);
			}
			xmlWriter.WriteEndElement();
			foreach (string[] row in GetRows(listView: listView))
			{
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
			ShowSuccess();
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "FictionBook2", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="listView"/> as a Compiled HTML Help (CHM) file.</summary>
	/// <param name="listView">The <see cref="ListView"/> containing the data to export.</param>
	/// <param name="title">The CHM title used in HTML content and the HHP project file.</param>
	/// <param name="fileName">The full path of the output file.</param>
	/// <remarks>Requires Microsoft HTML Help Workshop (<c>hhc.exe</c>) to be installed. If it is not found,
	/// an error message is shown and no file is written.</remarks>
	public static void SaveAsChm(ListView listView, string title, string fileName)
	{
		string[] headers = GetHeaders(listView: listView);
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
		string tempDir = Path.Combine(path1: Path.GetTempPath(), path2: Guid.NewGuid().ToString());
		Directory.CreateDirectory(path: tempDir);
		try
		{
			string htmlPath = Path.Combine(path1: tempDir, path2: "index.html");
			string hhcFilePath = Path.Combine(path1: tempDir, path2: "toc.hhc");
			string hhpPath = Path.Combine(path1: tempDir, path2: "project.hhp");
			string chmTempPath = Path.Combine(path1: tempDir, path2: "project.chm");
			using (StreamWriter writer = new(path: htmlPath, append: false, encoding: Encoding.UTF8))
			{
				string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
				writer.WriteLine(value: $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>{safeTitle}</title>");
				writer.WriteLine(value: "<style>table{{border-collapse:collapse;width:100%}}th,td{{border:1px solid #000;padding:5px;text-align:left}}</style></head><body>");
				writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
				writer.Write(value: "<table><tr>");
				foreach (string h in headers)
				{
					writer.Write(value: $"<th>{System.Net.WebUtility.HtmlEncode(value: h)}</th>");
				}
				writer.WriteLine(value: "</tr>");
				foreach (string[] row in GetRows(listView: listView))
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
				ShowSuccess();
			}
			else
			{
				_ = MessageBox.Show(
					text: "Failed to compile the CHM file.",
					caption: I18nStrings.ErrorCaption,
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Error);
			}
		}
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "CHM", filePath: fileName);
		}
		finally
		{
			if (Directory.Exists(path: tempDir))
			{
				Directory.Delete(path: tempDir, recursive: true);
			}
		}
	}

	#endregion
}
