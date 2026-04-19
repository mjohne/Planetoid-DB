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

/// <summary>Provides static methods for saving the contents of a <see cref="TextBox"/> to various file formats.</summary>
/// <remarks>Each method accepts the source <see cref="TextBox"/>, a document title used in the file content, and the full file-system path of the output file. Compressed file formats (DOCX, ODT, ODS, XLSX, EPUB) are written as proper ZIP archives rather than flat XML files. SQLite export requires System.Data.SQLite; CHM export requires Microsoft HTML Help Workshop (hhc.exe).</remarks>
public static partial class TextBoxExporter
{
	/// <summary>NLog logger for logging messages and errors.</summary>
	/// <remarks>Using NLog allows for flexible logging configuration and supports various log targets such as files, console, etc. The logger is initialized for the current class to capture context in log messages.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Reusable JSON serializer options for efficient serialization.</summary>
	/// <remarks>Creating a static instance of JsonSerializerOptions with WriteIndented set to true allows for consistent formatting of JSON output across all methods that serialize to JSON, while avoiding the overhead of creating new options instances for each serialization operation.</remarks>
	private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

	#region helpers

	/// <summary>Escapes LaTeX special characters.</summary>
	/// <param name="input">The raw input string.</param>
	/// <returns>The escaped string suitable for LaTeX output.</returns>
	/// <remarks>LaTeX special characters that need escaping include: \ { } % $ amp # _ ^ ~. This method iterates through each character in the input string and appends either the escaped version or the original character to a StringBuilder, which is then returned as the fully escaped string.</remarks>
	private static string EscapeLatex(string? input) => ExportEscapeHelper.EscapeLatex(input);

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

	/// <summary>Saves the content of the specified TextBox to a text file, including a title and a separator line.</summary>
	/// <remarks>The method writes the title, a separator line, and the TextBox content to the file using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox whose text content will be saved to the file. Cannot be null.</param>
	/// <param name="title">The title to write as the first line of the file. The length of this string determines the length of the separator
	/// line.</param>
	/// <param name="fileName">The full path and name of the file to which the content will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsText(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title, a separator line, and the TextBox content to the file with UTF-8 encoding.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title, a separator line of dashes matching the title length, and the TextBox content to the file.
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '-', count: title.Length));
			writer.WriteLine(value: textBox.Text);
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Text", filePath: fileName);
		}
	}

	/// <summary>Exports the contents of the specified text box to a LaTeX document and saves it to the specified file.</summary>
	/// <remarks>The method creates a basic LaTeX document using the UTF-8 encoding. Each line from the text box is escaped for LaTeX and written as a new line in the document. If an I/O or access error occurs during saving, an error message is displayed.</remarks>
	/// <param name="textBox">The text box whose lines will be written to the LaTeX document. Cannot be null.</param>
	/// <param name="title">The title to use for the LaTeX document. This value will appear as the document's title.</param>
	/// <param name="fileName">The full path and file name where the LaTeX document will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsLatex(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to create a LaTeX document with the specified title and content from the text box. Each line is escaped for LaTeX and written as a new line in the document.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "\\documentclass{article}");
			writer.WriteLine(value: "\\usepackage[utf8]{inputenc}");
			writer.WriteLine(value: "\\begin{document}");
			writer.WriteLine(value: $"\\title{{{EscapeLatex(input: title)}}}");
			writer.WriteLine(value: "\\maketitle");
			// Write each line from the text box to the LaTeX document, escaping special characters and adding a line break after each line.
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: EscapeLatex(input: line) + "\\\\");
			}
			writer.WriteLine(value: "\\end{document}");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "LaTeX", filePath: fileName);
		}
	}

	/// <summary>Saves the content of the specified text box as a Markdown file with the given title.</summary>
	/// <remarks>If the file cannot be written due to I/O errors or insufficient permissions, an error is displayed to the user. The method overwrites the file if it already exists.</remarks>
	/// <param name="textBox">The text box whose content will be saved to the Markdown file. Cannot be null.</param>
	/// <param name="title">The title to be used as the first-level heading in the Markdown file. If empty, the file will start with an empty heading.</param>
	/// <param name="fileName">The full path and name of the file to which the Markdown content will be saved. Must be a valid file path.</param>
	public static void SaveAsMarkdown(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title as a Markdown heading and the content of the text box to the specified file. The file is saved with UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a Markdown heading, followed by the content of the text box.
			writer.WriteLine(value: $"# {title}");
			writer.WriteLine();
			writer.WriteLine(value: textBox.Text);
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Markdown", filePath: fileName);
		}
	}

	/// <summary>Saves the content of the specified text box to a file in AsciiDoc format using the provided title.</summary>
	/// <remarks>The method writes the title as an AsciiDoc heading, followed by the text box content. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose content will be saved to the AsciiDoc file. Cannot be null.</param>
	/// <param name="title">The title to be used as the AsciiDoc document heading. This appears as the first line in the output file.</param>
	/// <param name="fileName">The full path and name of the file to which the AsciiDoc content will be written. If the file exists, it will be overwritten.</param>
	public static void SaveAsAsciiDoc(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title as an AsciiDoc heading and the content of the text box to the specified file. The file is saved with UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as an AsciiDoc heading, followed by the content of the text box.
			writer.WriteLine(value: $"= {title}");
			writer.WriteLine();
			writer.WriteLine(value: textBox.Text);
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AsciiDoc", filePath: fileName);
		}
	}

	/// <summary>Saves the text content of the specified TextBox to a file in reStructuredText format, using the provided title as a heading.</summary>
	/// <remarks>The method writes the title as a reStructuredText heading, followed by the content of the text box to the specified file. The file is saved with UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose text content will be saved to the file. Cannot be null.</param>
	/// <param name="title">The title to use as the heading in the reStructuredText file. The length of the title determines the underline formatting.</param>
	/// <param name="fileName">The full path and name of the file to which the content will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsReStructuredText(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title as a reStructuredText heading and the content of the text box to the specified file. The file is saved with UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a reStructuredText heading, followed by the content of the text box.
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine(value: title);
			writer.WriteLine(value: new string(c: '=', count: title.Length));
			writer.WriteLine();
			writer.WriteLine(value: textBox.Text);
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "reStructuredText", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified text box to a file in Textile markup format, using the provided title as a
	/// heading.</summary>
	/// <remarks>Each line from the text box is written as a separate paragraph in the output file. The method writes the title as a level-one heading at the top of the file. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose lines are to be saved as Textile-formatted paragraphs. Cannot be null.</param>
	/// <param name="title">The title to use as the main heading in the Textile file. This will be written as a level-one heading.</param>
	/// <param name="fileName">The full path and name of the file to which the Textile content will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsTextile(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title as a Textile heading and each line of the text box as a separate paragraph in the specified file. The file is saved with UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as a Textile level-one heading, followed by each line of the text box as a separate paragraph.
			writer.WriteLine(value: $"h1. {title}");
			writer.WriteLine();
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: $"p. {line}");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Textile", filePath: fileName);
		}
	}

	/// <summary>
	/// Saves the contents of the specified text box as a Microsoft Word document in the .docx format at the given file path, using the provided title as the document heading.</summary>
	/// <remarks>If a file already exists at the specified path, it will be overwritten. The method creates a minimal .docx file containing the title and each line of the text box as a separate paragraph. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose lines will be exported to the Word document. Cannot be null.</param>
	/// <param name="title">The title to use as the heading of the Word document. If null, an empty title is used.</param>
	/// <param name="fileName">The full file path where the Word document will be created. Must be a valid path and the application must have write access.</param>
	public static void SaveAsWord(TextBox textBox, string title, string fileName)
	{
		// Use a ZipArchive to create a .docx file, which is essentially a ZIP archive containing XML files. The method creates the necessary structure for a minimal Word document, including the content types, relationships, and main document XML. Each line from the text box is added as a separate paragraph in the document body. If an I/O or access error occurs during file creation, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream and finalize the ZIP archive.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the necessary entries in the ZIP archive for a minimal .docx file structure.
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			// Write the content types XML, which defines the MIME types for the parts of the .docx file. This is required for Word to recognize the structure of the document.
			using (StreamWriter writer = new(stream: contentTypesEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
				writer.WriteLine(value: "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
				writer.WriteLine(value: "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
				writer.WriteLine(value: "  <Override PartName=\"/word/document.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\"/>");
				writer.WriteLine(value: "</Types>");
			}
			// Create the relationships entry, which defines the relationship between the main document part and the package. This is required for Word to locate the main document XML.
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			// Write the relationships XML, which specifies that the main document part (document.xml) is related to the package with a specific relationship ID. This allows Word to find and load the main document content when opening the .docx file.
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"word/document.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the main document XML entry, which contains the actual content of the Word document. This includes the title as a heading and each line from the text box as a separate paragraph.
			ZipArchiveEntry documentEntry = archive.CreateEntry(entryName: "word/document.xml", compressionLevel: CompressionLevel.Optimal);
			// Write the main document XML, which defines the structure and content of the Word document. The title is added as a heading, and each line from the text box is added as a separate paragraph. The XML namespaces and structure follow the OpenXML standard for Word documents.
			using (StreamWriter writer = new(stream: documentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<w:document xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
				writer.WriteLine(value: "  <w:body>");
				string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
				writer.WriteLine(value: $"    <w:p><w:pPr><w:pStyle w:val=\"Title\"/></w:pPr><w:r><w:t>{safeTitle}</w:t></w:r></w:p>");
				foreach (string line in textBox.Lines)
				{
					string safe = System.Net.WebUtility.HtmlEncode(value: line) ?? string.Empty;
					writer.WriteLine(value: $"    <w:p><w:r><w:t>{safe}</w:t></w:r></w:p>");
				}
				writer.WriteLine(value: "  </w:body>");
				writer.WriteLine(value: "</w:document>");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Word", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as an OpenDocument Text (ODT) file with the given title and file name.</summary>
	/// <remarks>The method creates a minimal ODT file structure, including the required mimetype, manifest, and content files. The text from the TextBox is written as paragraphs, and the title is used as a heading. If the file cannot be created due to I/O or permission issues, an error is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose text content will be exported to the ODT file.</param>
	/// <param name="title">The title to be used as the main heading in the generated ODT document. This value is inserted as a heading in the document.</param>
	/// <param name="fileName">The full path and file name where the ODT file will be created. If a file with the same name exists, it will be overwritten.</param>
	public static void SaveAsOdt(TextBox textBox, string title, string fileName)
	{
		// Use a ZipArchive to create an ODT file, which is essentially a ZIP archive containing specific XML files. The method creates the necessary structure for a minimal ODT file, including the mimetype, manifest, and content XML. Each line from the TextBox is added as a separate paragraph in the content XML. If an I/O or access error occurs during file creation, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream and finalize the ZIP archive.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the mimetype entry, which must be the first entry in the ODT file and must be stored without compression. This entry specifies the MIME type of the document, which is required for ODT files.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			// Write the MIME type for an ODT file to the mimetype entry. This is required for ODT files and must be exactly "application/vnd.oasis.opendocument.text".
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.text");
			}
			// Create the manifest entry, which defines the files included in the ODT package and their MIME types. This is required for ODT files to specify the structure of the document.
			ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
			// Write the manifest XML, which lists the files included in the ODT package and their corresponding MIME types. This is necessary for ODT files to define the structure and content of the document.
			using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.text\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: "</manifest:manifest>");
			}
			// Create the content entry, which contains the actual content of the ODT document. This includes the title as a heading and each line from the TextBox as a separate paragraph. The XML structure follows the OpenDocument standard for text documents.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
			// Write the content XML, which defines the structure and content of the ODT document. The title is added as a heading, and each line from the TextBox is added as a separate paragraph. The XML namespaces and structure follow the OpenDocument standard for text documents.
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" office:version=\"1.2\">");
				writer.WriteLine(value: "  <office:body><office:text>");
				string safeTitle = System.Security.SecurityElement.Escape(str: title) ?? string.Empty;
				writer.WriteLine(value: $"    <text:h text:outline-level=\"1\">{safeTitle}</text:h>");
				foreach (string line in textBox.Lines)
				{
					string safe = System.Security.SecurityElement.Escape(str: line) ?? string.Empty;
					writer.WriteLine(value: $"    <text:p>{safe}</text:p>");
				}
				writer.WriteLine(value: "  </office:text></office:body>");
				writer.WriteLine(value: "</office:document-content>");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODT", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified text box as a Microsoft Excel (XLSX) file at the given file path, using the provided title as the sheet name.</summary>
	/// <remarks>The method creates a minimal Excel XLSX file containing the title as the sheet name and each line of the text box as a separate row. If an I/O or access error occurs during saving, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose lines will be exported to the Excel file. Cannot be null.</param>
	/// <param name="title">The title to use as the sheet name in the Excel file. If null or empty, a default name is used.</param>
	/// <param name="fileName">The full file path where the Excel file will be created. Must be a valid path and the application must have write access.</param>
	public static void SaveAsXlsx(TextBox textBox, string title, string fileName)
	{
		// Use a ZipArchive to create an Excel XLSX file, which is essentially a ZIP archive containing specific XML files. The method creates the necessary structure for a minimal XLSX file, including the content types, relationships, and worksheet XML. Each line from the text box is added as a separate row in the worksheet. If an I/O or access error occurs during file creation, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream and finalize the ZIP archive.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the necessary entries in the ZIP archive for a minimal XLSX file structure.
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			// Write the content types XML, which defines the MIME types for the parts of the XLSX file. This is required for Excel to recognize the structure of the document.
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
			// Create the relationships entry, which defines the relationship between the main workbook part and the package. This is required for Excel to locate the main workbook XML when opening the .xlsx file.
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			// Write the relationships XML, which specifies that the main workbook part (workbook.xml) is related to the package with a specific relationship ID. This allows Excel to find and load the main workbook content when opening the .xlsx file.
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the main workbook XML entry, which contains the structure of the Excel workbook. This includes a reference to the worksheet that will contain the data. The XML structure follows the OpenXML standard for Excel workbooks.
			ZipArchiveEntry workbookEntry = archive.CreateEntry(entryName: "xl/workbook.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: workbookEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
				writer.WriteLine(value: "  <sheets><sheet name=\"Data\" sheetId=\"1\" r:id=\"rId1\"/></sheets>");
				writer.WriteLine(value: "</workbook>");
			}
			// Create the workbook relationships entry, which defines the relationship between the workbook and the worksheet. This is required for Excel to locate the worksheet XML when opening the .xlsx file.
			ZipArchiveEntry wbRelsEntry = archive.CreateEntry(entryName: "xl/_rels/workbook.xml.rels", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: wbRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the worksheet XML entry, which contains the actual data for the Excel sheet. Each line from the TextBox is added as a separate row in the worksheet. The title is added as the first row. The XML structure follows the OpenXML standard for Excel worksheets.
			ZipArchiveEntry sheetEntry = archive.CreateEntry(entryName: "xl/worksheets/sheet1.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: sheetEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
				writer.WriteLine(value: "<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
				writer.WriteLine(value: "  <sheetData>");
				writer.WriteLine(value: $"    <row><c t=\"inlineStr\"><is><t>{System.Security.SecurityElement.Escape(str: title)}</t></is></c></row>");
				foreach (string line in textBox.Lines)
				{
					writer.WriteLine(value: $"    <row><c t=\"inlineStr\"><is><t>{System.Security.SecurityElement.Escape(str: line)}</t></is></c></row>");
				}
				writer.WriteLine(value: "  </sheetData>");
				writer.WriteLine(value: "</worksheet>");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "Excel", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as an OpenDocument Spreadsheet (ODS) file at the given file path.</summary>
	/// <remarks>The method creates a minimal ODS file containing a single table with one column, where each row corresponds to a line from the TextBox. The method displays a success or error message upon completion. The file is created using UTF-8 encoding for XML content and ASCII encoding for the mimetype entry.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported to the ODS file. Each line is written as a separate row in the spreadsheet.</param>
	/// <param name="title">The title to use for the spreadsheet table within the ODS file. This value is used as the table name.</param>
	/// <param name="fileName">The full file path where the ODS file will be created. If a file with the same name exists, it will be overwritten.</param>
	public static void SaveAsOds(TextBox textBox, string title, string fileName)
	{
		// Use a ZipArchive to create an ODS file, which is essentially a ZIP archive containing specific XML files. The method creates the necessary structure for a minimal ODS file, including the mimetype, manifest, and content XML. Each line from the TextBox is added as a separate row in the spreadsheet. If an I/O or access error occurs during file creation, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream and finalize the ZIP archive.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the mimetype entry, which must be the first entry in the ODS file and must be stored without compression. This entry specifies the MIME type of the document, which is required for ODS files.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			// Write the MIME type for an ODS file to the mimetype entry. This is required for ODS files and must be exactly "application/vnd.oasis.opendocument.spreadsheet".
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/vnd.oasis.opendocument.spreadsheet");
			}
			// Create the manifest entry, which defines the files included in the ODS package and their MIME types. This is required for ODS files to specify the structure of the document.
			ZipArchiveEntry manifestEntry = archive.CreateEntry(entryName: "META-INF/manifest.xml", compressionLevel: CompressionLevel.Optimal);
			// Write the manifest XML, which lists the files included in the ODS package and their corresponding MIME types. This is necessary for ODS files to define the structure and content of the document.
			using (StreamWriter writer = new(stream: manifestEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<manifest:manifest xmlns:manifest=\"urn:oasis:names:tc:opendocument:xmlns:manifest:1.0\" manifest:version=\"1.2\">");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"/\" manifest:media-type=\"application/vnd.oasis.opendocument.spreadsheet\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"content.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: " <manifest:file-entry manifest:full-path=\"META-INF/manifest.xml\" manifest:media-type=\"text/xml\"/>");
				writer.WriteLine(value: "</manifest:manifest>");
			}
			// Create the content entry, which contains the actual content of the ODS document. This includes the title as a heading and each line from the TextBox as a separate row in a table. The XML structure follows the OpenDocument standard for spreadsheets.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "content.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<office:document-content xmlns:office=\"urn:oasis:names:tc:opendocument:xmlns:office:1.0\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\" xmlns:table=\"urn:oasis:names:tc:opendocument:xmlns:table:1.0\" office:version=\"1.2\">");
				writer.WriteLine(value: "  <office:body><office:spreadsheet>");
				string safeName = System.Security.SecurityElement.Escape(str: title) ?? "Data";
				writer.WriteLine(value: $"    <table:table table:name=\"{safeName}\"><table:table-column table:number-columns-repeated=\"1\"/>");
				foreach (string line in textBox.Lines)
				{
					writer.WriteLine(value: $"    <table:table-row><table:table-cell office:value-type=\"string\"><text:p>{System.Security.SecurityElement.Escape(str: line)}</text:p></table:table-cell></table:table-row>");
				}
				writer.WriteLine(value: "    </table:table>");
				writer.WriteLine(value: "  </office:spreadsheet></office:body>");
				writer.WriteLine(value: "</office:document-content>");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ODS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a Rich Text Format (RTF) file with the given title.</summary>
	/// <remarks>The method writes the TextBox content as plain text in RTF format using ASCII encoding. If the file cannot be written due to I/O or permission issues, an error is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose text content will be saved to the RTF file.</param>
	/// <param name="title">The title to include at the beginning of the RTF document. This appears as a bold heading in the file.</param>
	/// <param name="fileName">The full path and file name where the RTF file will be created or overwritten.</param>
	public static void SaveAsRtf(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the content of the TextBox to an RTF file. The method constructs a basic RTF document structure, including the title as a bold heading and each line of the TextBox as a separate paragraph. The file is saved using ASCII encoding. If an I/O or access error occurs during saving, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			// Write the RTF header and define a font table with Arial as the default font. Then write the title as a bold heading and each line from the TextBox as a separate paragraph. Finally, close the RTF document structure.
			writer.WriteLine(value: @"{\rtf1\ansi\deff0");
			writer.WriteLine(value: @"{\fonttbl{\f0 Arial;}}");
			writer.WriteLine(value: @"\f0\fs20");
			writer.WriteLine(value: $@"{{\pard\b\fs24 {EscapeRtf(input: title)}\par\par}}");
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: $@"{{\pard {EscapeRtf(input: line)}\par}}");
			}
			writer.WriteLine(value: "}");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "RTF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified text box to a file in ABW (AbiWord) format using the provided title.</summary>
	/// <remarks>The method writes a minimal AbiWord XML document containing the title as a first-level heading and the text box content as paragraphs. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose content will be saved to the ABW file. Cannot be null.</param>
	/// <param name="title">The title to be used as the first-level heading in the ABW file. If empty, the file will start with an empty heading.</param>
	/// <param name="fileName">The full path and name of the file to which the ABW content will be saved. Must be a valid file path.</param>
	public static void SaveAsAbiword(TextBox textBox, string title, string fileName)
	{
		// Write a minimal AbiWord XML document so the generated .abw file matches the method name and caller expectations.
		try
		{
			XmlWriterSettings settings = new()
			{
				Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
				Indent = true,
				NewLineHandling = NewLineHandling.Entitize
			};

			using XmlWriter writer = XmlWriter.Create(outputFileName: fileName, settings: settings);
			writer.WriteStartDocument();
			writer.WriteStartElement(localName: "abiword");
			writer.WriteAttributeString(localName: "template", value: "normal.awt");
			writer.WriteAttributeString(localName: "xml:space", value: "preserve");
			writer.WriteAttributeString(localName: "xmlns", value: "http://www.abisource.com/awml.dtd");
			writer.WriteAttributeString(localName: "version", value: "1.0");

			writer.WriteStartElement(localName: "metadata");
			// The "m" element is the AbiWord metadata key-value element; here it stores the document title.
			writer.WriteElementString(localName: "m", ns: null, value: title);
			writer.WriteEndElement();

			writer.WriteStartElement(localName: "section");

			writer.WriteStartElement(localName: "p");
			writer.WriteAttributeString(localName: "style", value: "Heading 1");
			writer.WriteString(text: title);
			writer.WriteEndElement();

			string[] lines = textBox.Text.Replace(oldValue: "\r\n", newValue: "\n", comparisonType: StringComparison.Ordinal).Split('\n');
			foreach (string line in lines)
			{
				writer.WriteStartElement(localName: "p");
				writer.WriteString(text: line);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "AbiWord", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a WPS-compatible HTML file with the given title.</summary>
	/// <remarks>The method encodes all text to ensure valid HTML output. If an I/O or access error occurs during
	/// saving, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox whose lines will be exported to the WPS file. Cannot be null.</param>
	/// <param name="title">The title to use for the HTML document. This value is used in the document's <c>title</c> element and as a heading.</param>
	/// <param name="fileName">The full path and file name where the WPS file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsWps(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the content of the TextBox to an HTML file compatible with WPS Office. The method constructs a basic HTML document structure, including the title in the <c>title</c> element and as a heading in the body. Each line from the TextBox is added as a separate paragraph. All text is HTML-encoded to ensure valid output. The file is saved using UTF-8 encoding. If an I/O or access error occurs during saving, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the HTML document structure, including the title and each line from the TextBox as a separate paragraph. All text is HTML-encoded to ensure valid output.
			writer.WriteLine(value: "<!DOCTYPE html>");
			writer.WriteLine(value: "<html><head><meta charset=\"utf-8\">");
			writer.WriteLine(value: $"<title>{System.Net.WebUtility.HtmlEncode(value: title)}</title>");
			writer.WriteLine(value: "</head><body>");
			writer.WriteLine(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: $"<p>{System.Net.WebUtility.HtmlEncode(value: line)}</p>");
			}
			writer.WriteLine(value: "</body></html>");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "WPS", filePath: fileName);
		}
	}

	/// <summary>Exports the contents of the specified TextBox to a new Excel file in .xlsx format, using the provided title and file name.</summary>
	/// <remarks>This method delegates to <see cref="SaveAsXlsx"/> to avoid duplicating XLSX-generation logic.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be written as rows in the generated Excel worksheet. Cannot be null.</param>
	/// <param name="title">The title to be written as the first row in the Excel worksheet. This value appears as the header of the exported data.</param>
	/// <param name="fileName">The full path and file name for the Excel file to create. If a file with the same name exists, it will be overwritten.</param>
	public static void SaveAsExcel(TextBox textBox, string title, string fileName) =>
		SaveAsXlsx(textBox: textBox, title: title, fileName: fileName);

	/// <summary>Saves the contents of the specified TextBox as a PDF document.</summary>
	/// <remarks>The method creates a valid PDF document using a consistent object numbering scheme (Catalog, Pages, Font, then Page/Content pairs). A proper cross-reference table with computed byte offsets is appended before the trailer so that PDF readers can locate each object. If an I/O or access error occurs during saving, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose contents are to be saved. Cannot be null.</param>
	/// <param name="title">The title to use for the PDF document. Cannot be null or empty.</param>
	/// <param name="fileName">The name of the file to which the PDF document will be saved. Cannot be null or empty.</param>
	public static void SaveAsPdf(TextBox textBox, string title, string fileName)
	{
		// Write a valid PDF using a consistent object numbering scheme and a proper cross-reference table with real byte offsets.
		try
		{
			// Define constants for page dimensions and layout. The page height is set to 842 points (A4 size), with a starting Y position of 750 points for the first line of text. A margin of 50 points is maintained at the bottom, and each line of text is spaced by 14 points.
			const int pageHeight = 842;
			const int startY = 750;
			const int marginY = 50;
			const int lineHeight = 14;

			// Object IDs: 1=Catalog, 2=Pages, 3=Font, then per-page pairs: 4+2*i=Page, 5+2*i=Content.
			const int catalogObjectId = 1;
			const int pagesObjectId = 2;
			const int fontObjectId = 3;
			const int firstPageObjectId = 4;

			// Pre-calculate how many lines fit on a page and the total page count.
			// 30 is a top-of-page offset reserved below the title line so body text starts with consistent spacing.
			int linesPerPage = Math.Max(val1: 1, val2: (startY - 30 - marginY) / lineHeight);
			int pageCount = Math.Max(val1: 1, val2: (textBox.Lines.Length + linesPerPage - 1) / linesPerPage);
			int totalObjects = 3 + (pageCount * 2);

			// Track the byte offset at which each object starts so the xref table is accurate.
			System.Collections.Generic.Dictionary<int, long> objectOffsets = new();

			// Use a FileStream so that BaseStream.Position accurately reflects bytes written to disk.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using StreamWriter writer = new(stream: fs, encoding: Encoding.ASCII, bufferSize: 4096, leaveOpen: false)
			{
				NewLine = "\n"
			};

			// Flush the StreamWriter buffer and record the current stream position as the start of objectId.
			void BeginObject(int objectId)
			{
				writer.Flush();
				objectOffsets[objectId] = fs.Position;
				writer.WriteLine(value: $"{objectId} 0 obj");
			}

			// Write the PDF header.
			writer.WriteLine(value: "%PDF-1.4");
			writer.WriteLine(value: "%\xb5\xb5\xb5\xb5");

			// Object 1: Catalog — references the Pages object.
			BeginObject(catalogObjectId);
			writer.WriteLine(value: "<<");
			writer.WriteLine(value: $"/Type /Catalog /Pages {pagesObjectId} 0 R");
			writer.WriteLine(value: ">>");
			writer.WriteLine(value: "endobj");

			// Object 2: Pages — lists all page objects and the total page count.
			BeginObject(pagesObjectId);
			writer.WriteLine(value: "<<");
			writer.Write(value: $"/Type /Pages /Count {pageCount} /Kids [");
			for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
			{
				if (pageIndex > 0)
				{
					writer.Write(value: " ");
				}

				writer.Write(value: $"{firstPageObjectId + (pageIndex * 2)} 0 R");
			}

			writer.WriteLine(value: "]");
			writer.WriteLine(value: ">>");
			writer.WriteLine(value: "endobj");

			// Object 3: Font — a standard Type1 Helvetica font shared by all pages.
			BeginObject(fontObjectId);
			writer.WriteLine(value: "<<");
			writer.WriteLine(value: "/Type /Font");
			writer.WriteLine(value: "/Subtype /Type1");
			writer.WriteLine(value: "/BaseFont /Helvetica");
			writer.WriteLine(value: ">>");
			writer.WriteLine(value: "endobj");

			// Write one Page object and one Content stream object for each page.
			for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
			{
				int pageObjectId = firstPageObjectId + (pageIndex * 2);
				int contentObjectId = pageObjectId + 1;

				// Page object references the shared font and its own content stream.
				BeginObject(pageObjectId);
				writer.WriteLine(value: "<<");
				writer.WriteLine(value: "/Type /Page");
				writer.WriteLine(value: $"/Parent {pagesObjectId} 0 R");
				writer.WriteLine(value: "/MediaBox [0 0 595 842]");
				writer.WriteLine(value: $"/Contents {contentObjectId} 0 R");
				writer.WriteLine(value: $"/Resources << /Font << /F1 {fontObjectId} 0 R >> >>");
				writer.WriteLine(value: ">>");
				writer.WriteLine(value: "endobj");

				// Build the page content stream in a MemoryStream so the /Length value is known before writing.
				StringBuilder sb = new();
				sb.AppendLine(value: "BT /F1 10 Tf");
				if (pageIndex == 0)
				{
					sb.AppendLine(value: $"1 0 0 1 50 {pageHeight - 40} Tm ({EscapePdf(text: title)}) Tj");
				}

				// Start the body text 30 points below the top content margin to leave space after the title line.
				int currentY = startY - 30;
				int startLineIndex = pageIndex * linesPerPage;
				int endLineIndex = Math.Min(val1: startLineIndex + linesPerPage, val2: textBox.Lines.Length);
				for (int lineIndex = startLineIndex; lineIndex < endLineIndex; lineIndex++)
				{
					sb.AppendLine(value: $"1 0 0 1 50 {currentY} Tm ({EscapePdf(text: textBox.Lines[lineIndex])}) Tj");
					currentY -= lineHeight;
				}

				sb.AppendLine(value: "ET");

				// Encode the content to ASCII bytes so the byte length is exact.
				using MemoryStream ms = new();
				using (StreamWriter sw = new(stream: ms, encoding: Encoding.ASCII, bufferSize: 1024, leaveOpen: true) { NewLine = "\n" })
				{
					sw.Write(value: sb.ToString());
					sw.Flush();
				}

				// Content stream object — /Length must match the exact byte count of the stream body.
				BeginObject(contentObjectId);
				writer.WriteLine(value: "<<");
				writer.WriteLine(value: $"/Length {ms.Length}");
				writer.WriteLine(value: ">>");
				writer.WriteLine(value: "stream");
				writer.Flush();
				ms.Position = 0;
				ms.CopyTo(destination: fs);
				writer.WriteLine();
				writer.WriteLine(value: "endstream");
				writer.WriteLine(value: "endobj");
			}

			// Flush pending bytes and record the byte offset where the xref section begins.
			writer.Flush();
			long xrefOffset = fs.Position;

			// Write the cross-reference table. Each entry must be exactly 20 bytes (10-digit offset, space, 5-digit generation, space, keyword, space, newline).
			int xrefSize = totalObjects + 1;
			writer.WriteLine(value: "xref");
			writer.WriteLine(value: $"0 {xrefSize}");
			writer.WriteLine(value: "0000000000 65535 f ");
			for (int objectId = 1; objectId <= totalObjects; objectId++)
			{
				if (objectOffsets.TryGetValue(key: objectId, value: out long objectOffset))
				{
					writer.WriteLine(value: $"{objectOffset:0000000000} 00000 n ");
				}
				else
				{
					writer.WriteLine(value: "0000000000 00000 f ");
				}
			}

			// Write the trailer with the Root reference and the numeric startxref offset.
			writer.WriteLine(value: "trailer");
			writer.WriteLine(value: "<<");
			writer.WriteLine(value: $"/Size {xrefSize}");
			writer.WriteLine(value: "/Root 1 0 R");
			writer.WriteLine(value: ">>");
			writer.WriteLine(value: "startxref");
			writer.WriteLine(value: xrefOffset.ToString());
			writer.WriteLine(value: "%%EOF");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PDF", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="textBox"/> as a FictionBook 2 (FB2) XML document.</summary>
	/// <remarks>The method creates a FictionBook 2 (FB2) XML document with each line from the TextBox. If an I/O or access error occurs during saving, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose contents are to be saved. Cannot be null.</param>
	/// <param name="title">The title to use for the FB2 document. Cannot be null or empty.</param>
	/// <param name="fileName">The name of the file to which the FB2 document will be saved. Cannot be null or empty.</param>
	public static void SaveAsFictionBook2(TextBox textBox, string title, string fileName)
	{
		// Use the System.Xml library to create an XML document in the FictionBook 2 (FB2) format. The method constructs the necessary FB2 structure, including the description, title-info, document-info, and body sections. Each line from the TextBox is added as a paragraph in the body section of the FB2 document. The XML is written with UTF-8 encoding and proper indentation for readability. If an I/O or access error occurs during saving, an error message is displayed to the user.
		try
		{
			// The 'using' statement ensures that the XmlWriter is properly disposed after use, which will flush and close the underlying file stream. The XmlWriter is created with settings that specify UTF-8 encoding and indentation for readability. The XML document is constructed according to the FB2 format specifications, with the appropriate namespaces and structure.
			string fb2Ns = "http://www.gribuser.ru/xml/fictionbook/2.0";
			// The XmlWriter is used to write the XML content for the FB2 document, including the description, title-info, document-info, and body sections. Each line from the TextBox is added as a paragraph in the body section of the FB2 document. The XML is written with UTF-8 encoding and proper indentation for readability, ensuring that the resulting FB2 file is well-formed and adheres to the FB2 specifications.
			XmlWriterSettings settings = new() { Indent = true, Encoding = Encoding.UTF8 };
			// Write the XML content for the FB2 document, including the necessary elements and attributes. The description section includes the title-info and document-info elements, which contain metadata about the book. The body section includes a title and a section that contains paragraphs for each line from the TextBox. The XML is structured according to the FB2 format specifications, ensuring compatibility with FB2 readers.
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			// Start the XML document and write the root element with the appropriate namespace. The FictionBook element is the root of the FB2 document, and it includes the namespace declaration for the FB2 format. This ensures that the XML document is recognized as a valid FB2 file by compatible readers.
			xmlWriter.WriteStartDocument();
			// Write the root element for the FB2 document, including the namespace declaration.
			xmlWriter.WriteStartElement(localName: "FictionBook", ns: fb2Ns);
			xmlWriter.WriteAttributeString(prefix: "xmlns", localName: "l", ns: null, value: "http://www.w3.org/1999/xlink");
			// Write the description section of the FB2 document, which includes the title-info and document-info elements. The title-info element contains metadata about the book, such as the genre, author, book title, and language. The document-info element contains metadata about the document itself, such as the author, program used to create it, creation date, unique identifier, and version. This information is important for readers to understand the origin and details of the FB2 document.
			xmlWriter.WriteStartElement(localName: "description", ns: fb2Ns);
			// Write the title-info element, which contains metadata about the book. This includes the genre, author information, book title, and language. The genre is set to "reference", and the author is specified with a first name of "Planetoid-DB" and an empty last name. The book title is set to the provided title parameter, and the language is set to English ("en"). This information is essential for FB2 readers to display the correct metadata for the book.
			xmlWriter.WriteStartElement(localName: "title-info", ns: fb2Ns);
			// The genre element is set to "reference", which indicates the type of content in the FB2 document. This is a required element in the title-info section of the FB2 format, and it helps readers categorize and display the book appropriately based on its genre.
			xmlWriter.WriteElementString(localName: "genre", ns: fb2Ns, value: "reference");
			// Write the author element, which contains the first name and last name of the author. In this case, the first name is set to "Planetoid-DB" and the last name is left empty. This information is included in the title-info section of the FB2 format and allows readers to identify the author of the book.
			xmlWriter.WriteStartElement(localName: "author", ns: fb2Ns);
			// The first-name element is set to "Planetoid-DB", which identifies the author of the book. The last-name element is left empty, as there is no last name provided. This information is included in the title-info section of the FB2 document and is important for readers to identify the author of the book.
			xmlWriter.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
			// The last-name element is left empty, as there is no last name provided for the author. This element is included in the title-info section of the FB2 document, and while it is required by the FB2 format, it can be left empty if there is no last name to provide.
			xmlWriter.WriteElementString(localName: "last-name", ns: fb2Ns, value: string.Empty);
			// End the author element after writing the first-name and last-name elements.
			xmlWriter.WriteEndElement();
			// The book-title element is set to the provided title parameter, which specifies the title of the book in the FB2 document. This element is included in the title-info section and is essential for readers to identify the title of the book when viewing the FB2 document.
			xmlWriter.WriteElementString(localName: "book-title", ns: fb2Ns, value: title);
			// The lang element is set to "en", which indicates that the language of the book is English. This element is included in the title-info section of the FB2 document and helps readers display the correct language information for the book.
			xmlWriter.WriteElementString(localName: "lang", ns: fb2Ns, value: "en");
			// End the title-info element after writing the genre, author, book-title, and lang elements.
			xmlWriter.WriteEndElement();
			// Write the document-info element, which contains metadata about the document itself. This includes the author of the document, the program used to create it, the creation date, a unique identifier, and the version of the document. This information is important for readers to understand the origin and details of the FB2 document.
			xmlWriter.WriteStartElement(localName: "document-info", ns: fb2Ns);
			// Write the author element within the document-info section, which contains the first name and last name of the author of the document. In this case, the first name is set to "Planetoid-DB" and the last name is left empty. This information is included in the document-info section of the FB2 document and provides details about the creator of the document.
			xmlWriter.WriteStartElement(localName: "author", ns: fb2Ns);
			// The first-name element is set to "Planetoid-DB", which identifies the author of the document. The last-name element is left empty, as there is no last name provided. This information is included in the document-info section of the FB2 document and provides details about the creator of the document.
			xmlWriter.WriteElementString(localName: "first-name", ns: fb2Ns, value: "Planetoid-DB");
			// The last-name element is left empty, as there is no last name provided for the author of the document. This element is included in the document-info section of the FB2 document, and while it is required by the FB2 format, it can be left empty if there is no last name to provide.
			xmlWriter.WriteElementString(localName: "last-name", ns: fb2Ns, value: string.Empty);
			// End the author element after writing the first-name and last-name elements.
			xmlWriter.WriteEndElement();
			// The program-used element is set to "Planetoid-DB", which indicates the program that was used to create the FB2 document. This element is included in the document-info section and provides information about the software used to generate the document.
			xmlWriter.WriteElementString(localName: "program-used", ns: fb2Ns, value: "Planetoid-DB");
			// The date element is set to the current date, which indicates when the FB2 document was created. This element is included in the document-info section and provides information about the creation date of the document.
			string fb2DateString = DateTime.Now.ToString(format: "yyyy-MM-dd");
			// The date element includes a value attribute that contains the date in the format "yyyy-MM-dd". The text content of the date element also contains the same date string. This provides both a machine-readable value and a human-readable representation of the creation date in the FB2 document.
			xmlWriter.WriteStartElement(localName: "date", ns: fb2Ns);
			// The value attribute of the date element is set to the current date in the format "yyyy-MM-dd". This provides a machine-readable representation of the creation date in the FB2 document, which can be used by readers to display or sort documents based on their creation dates.
			xmlWriter.WriteAttributeString(localName: "value", value: fb2DateString);
			// The text content of the date element is also set to the current date string, providing a human-readable representation of the creation date in the FB2 document. This allows readers to easily see the creation date when viewing the document.
			xmlWriter.WriteString(text: fb2DateString);
			// End the date element after writing the value attribute and text content.
			xmlWriter.WriteEndElement();
			// The id element is set to a new GUID, which provides a unique identifier for the FB2 document. This element is included in the document-info section and allows readers to uniquely identify the document, which can be useful for cataloging or referencing purposes.
			xmlWriter.WriteElementString(localName: "id", ns: fb2Ns, value: Guid.NewGuid().ToString());
			// The version element is set to "1.0", which indicates the version of the FB2 document. This element is included in the document-info section and provides information about the version of the document format being used.
			xmlWriter.WriteElementString(localName: "version", ns: fb2Ns, value: "1.0");
			// End the document-info element after writing the author, program-used, date, id, and version elements.
			xmlWriter.WriteEndElement();
			// End the description element after writing the title-info and document-info sections.
			xmlWriter.WriteEndElement();
			// Write the body section of the FB2 document, which contains the main content. The body includes a title and a section that contains paragraphs for each line from the TextBox. The title is added as a paragraph within the title element, and each line from the TextBox is added as a paragraph within the section element. This structure adheres to the FB2 format specifications and allows readers to display the content correctly.
			xmlWriter.WriteStartElement(localName: "body", ns: fb2Ns);
			// The title element contains a paragraph with the title of the book. This is included in the body section of the FB2 document and provides a clear title for the content when viewed on an FB2 reader.
			xmlWriter.WriteStartElement(localName: "title", ns: fb2Ns);
			// The paragraph element within the title contains the title of the book, which is set to the provided title parameter. This is included in the title element of the body section and provides a clear title for the content when viewed on an FB2 reader.
			xmlWriter.WriteElementString(localName: "p", ns: fb2Ns, value: title);
			// End the title element after writing the paragraph with the book title.
			xmlWriter.WriteEndElement();
			// The section element contains paragraphs for each line from the TextBox. Each line is added as a separate paragraph within the section, allowing for proper formatting and display of the content in the FB2 document. This structure adheres to the FB2 format specifications and ensures that the content is displayed correctly when viewed on an FB2 reader.
			xmlWriter.WriteStartElement(localName: "section", ns: fb2Ns);
			// Write each line from the TextBox as a paragraph in the section element of the FB2 document. Each line is added as a separate paragraph, allowing for proper formatting and display of the content in the FB2 document. This structure adheres to the FB2 format specifications and ensures that the content is displayed correctly when viewed on an FB2 reader.
			foreach (string line in textBox.Lines)
			{
				xmlWriter.WriteElementString(localName: "p", ns: fb2Ns, value: line);
			}
			// End the section element after writing all paragraphs for the lines from the TextBox.
			xmlWriter.WriteEndElement();
			// End the body element after writing the title and section elements.
			xmlWriter.WriteEndElement();
			// End the root FictionBook element after writing the description and body sections.
			xmlWriter.WriteEndElement();
			// End the XML document after writing all content for the FB2 file.
			xmlWriter.WriteEndDocument();
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "FictionBook2", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of <paramref name="textBox"/> as a Compiled HTML Help (CHM) file.</summary>
	/// <remarks>The method uses the Microsoft HTML Help Workshop (hhc.exe) to compile a CHM file. It creates temporary files for the HTML content, the table of contents (HHC), and the project file (HHP). The HTML content is generated from the lines in the TextBox, and the HHC and HHP files are created with the necessary structure for compiling a CHM file. The method then invokes hhc.exe with the project file to compile the CHM. If the compilation is successful, the resulting CHM file is copied to the specified location. If the compilation fails or if an I/O error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose contents are to be saved. Cannot be null.</param>
	/// <param name="title">The title to use for the save dialog or the saved file. Cannot be null or empty.</param>
	/// <param name="fileName">The name of the file to which the contents will be saved. Cannot be null or empty.</param>
	public static void SaveAsChm(TextBox textBox, string title, string fileName)
	{
		// The method uses the Microsoft HTML Help Workshop (hhc.exe) to compile a CHM file. It creates temporary files for the HTML content, the table of contents (HHC), and the project file (HHP). The HTML content is generated from the lines in the TextBox, and the HHC and HHP files are created with the necessary structure for compiling a CHM file. The method then invokes hhc.exe with the project file to compile the CHM. If the compilation is successful, the resulting CHM file is copied to the specified location. If the compilation fails or if an I/O error occurs, an error message is displayed to the user.
		string hhcPath = Path.Combine(
			path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ProgramFilesX86),
			path2: @"HTML Help Workshop\hhc.exe");
		// Check if the hhc.exe file exists at the expected location. If it does not exist, show an error message to the user indicating that Microsoft HTML Help Workshop is not installed or not found, and return from the method without attempting to compile the CHM file.
		if (!File.Exists(path: hhcPath))
		{
			_ = MessageBox.Show(
				text: "Microsoft HTML Help Workshop is not installed or not found at the default location. Cannot compile CHM file.",
				caption: I18nStrings.ErrorCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
			return;
		}
		// Create a temporary directory to store the HTML, HHC, and HHP files needed for compiling the CHM. The directory is created in the system's temporary folder with a unique name generated using a GUID. This ensures that the temporary files do not conflict with any existing files and can be safely cleaned up after the compilation process.
		string tempDir = Path.Combine(path1: Path.GetTempPath(), path2: Guid.NewGuid().ToString());
		// Create the temporary directory.
		Directory.CreateDirectory(path: tempDir);
		// The method uses a try-finally block to ensure that the temporary directory is deleted after the compilation process, regardless of whether it succeeds or fails. The try block contains the code for generating the HTML, HHC, and HHP files, as well as invoking hhc.exe to compile the CHM. The finally block checks if the temporary directory exists and deletes it recursively to clean up any temporary files created during the process.
		try
		{
			// Define the paths for the temporary HTML file, HHC file, HHP file, and the resulting CHM file within the temporary directory. These files are necessary for compiling the CHM using hhc.exe. The HTML file contains the content generated from the TextBox, while the HHC and HHP files define the structure and project settings for the CHM compilation.
			string htmlPath = Path.Combine(path1: tempDir, path2: "index.html");
			string hhcFilePath = Path.Combine(path1: tempDir, path2: "toc.hhc");
			string hhpPath = Path.Combine(path1: tempDir, path2: "project.hhp");
			string chmTempPath = Path.Combine(path1: tempDir, path2: "project.chm");
			// Generate the HTML content for the CHM file based on the lines in the TextBox. The HTML is structured with a simple layout, including a title and paragraphs for each line from the TextBox. The content is encoded in UTF-8 to ensure proper character representation. This HTML file will be used as the main topic for the CHM file when compiled.
			using (StreamWriter writer = new(path: htmlPath, append: false, encoding: Encoding.UTF8))
			{
				string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
				writer.WriteLine(value: $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>{safeTitle}</title></head><body>");
				writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
				foreach (string line in textBox.Lines)
				{
					writer.WriteLine(value: $"<p>{System.Net.WebUtility.HtmlEncode(value: line)}</p>");
				}
				writer.WriteLine(value: "</body></html>");
			}
			// Generate the HHC (table of contents) file for the CHM compilation. The HHC file defines the structure of the table of contents for the CHM file, including the title and the link to the main topic (index.html). The content is encoded in ASCII, as required by the CHM format. This HHC file will be referenced in the HHP project file during compilation.
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
			// Generate the HHP (project) file for the CHM compilation. The HHP file defines the project settings for compiling the CHM file, including the title, language, default topic, and the list of files to include in the compilation. The content is encoded in ASCII, as required by the CHM format. This HHP file will be used as the input for hhc.exe to compile the CHM file.
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
			// Invoke hhc.exe with the project file to compile the CHM. The ProcessStartInfo is configured to run hhc.exe with the appropriate arguments, and the process is executed without creating a window. The method waits for the compilation process to complete before proceeding. If the compilation is successful, the resulting CHM file is copied to the specified location. If the compilation fails or if an I/O error occurs, an error message is displayed to the user.
			ProcessStartInfo startInfo = new()
			{
				FileName = hhcPath,
				Arguments = $"\"{hhpPath}\"",
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false
			};
			// Start the compilation process and wait for it to complete.
			using (Process? process = Process.Start(startInfo: startInfo))
			{
				process?.WaitForExit();
			}
			// After the compilation process completes, check if the resulting CHM file exists in the temporary directory. If it exists, copy it to the specified location. If it does not exist, show an error message to the user indicating that the compilation failed.
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
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "CHM", filePath: fileName);
		}
		// The finally block ensures that the temporary directory is deleted after the compilation process, regardless of whether it succeeds or fails. This is important for cleaning up any temporary files created during the process and preventing clutter in the system's temporary folder.
		finally
		{
			if (Directory.Exists(path: tempDir))
			{
				Directory.Delete(path: tempDir, recursive: true);
			}
		}
	}

	/// <summary>Saves the contents of <paramref name="textBox"/> as an XML Paper Specification (XPS) document.</summary>
	/// <remarks>The method creates an XPS document with each line from the TextBox. If an I/O or access error occurs during saving, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The text box whose contents are to be saved. Cannot be null.</param>
	/// <param name="title">The title to use for the XPS document. Cannot be null or empty.</param>
	/// <param name="fileName">The name of the file to which the XPS document will be saved. Cannot be null or empty.</param>
	public static void SaveAsXps(TextBox textBox, string title, string fileName)
	{
		// The method uses the System.IO.Compression library to create a ZIP archive in the XPS format. It constructs the necessary structure for an XPS document, including the [Content_Types].xml file, the _rels/.rels file, the _rels/FixedDocSeq.fdseq.rels file, and the FixedDocSeq.fdseq file. Each line from the TextBox is added as a separate page in the XPS document, with a simple layout that includes the title and page number. The resulting ZIP archive is saved with a .xps extension. If an I/O or access error occurs during this process, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream. The ZipArchive is created in Create mode, which allows for adding entries to the archive. The method constructs the necessary files and structure for an XPS document, including the content types, relationships, and fixed document sequence. Each line from the TextBox is added as a separate page in the XPS document, with a simple layout that includes the title and page number.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the [Content_Types].xml entry, which defines the content types for the XPS document. This file is required by the XPS format and specifies the content types for the relationships, fixed document sequence, fixed documents, fixed pages, and fonts used in the XPS document. The content types are defined according to the Open Packaging Conventions (OPC) specifications, which are used by XPS to structure the document.
			ZipArchiveEntry contentTypesEntry = archive.CreateEntry(entryName: "[Content_Types].xml", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the [Content_Types].xml file, which defines the content types for the XPS document. The XML is structured according to the OPC specifications, with a root Types element that contains Default elements for each content type used in the XPS document. This file is essential for XPS readers to understand how to handle the different parts of the document based on their content types.
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
			// Create the _rels/.rels entry, which defines the relationships for the XPS document. This file is required by the XPS format and specifies the relationship between the root of the package and the fixed document sequence. The relationship is defined according to the OPC specifications, which are used by XPS to structure the document.
			ZipArchiveEntry relsEntry = archive.CreateEntry(entryName: "_rels/.rels", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the _rels/.rels file, which defines the relationships for the XPS document. The XML is structured according to the OPC specifications, with a root Relationships element that contains a Relationship element defining the relationship between the root of the package and the fixed document sequence. This file is essential for XPS readers to understand how to navigate the structure of the document based on its relationships.
			using (StreamWriter writer = new(stream: relsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/fixedrepresentation\" Target=\"/FixedDocSeq.fdseq\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the _rels/FixedDocSeq.fdseq.rels entry, which defines the relationships for the fixed document sequence. This file is required by the XPS format and specifies the relationship between the fixed document sequence and the fixed document. The relationship is defined according to the OPC specifications, which are used by XPS to structure the document.
			ZipArchiveEntry fdseqRelsEntry = archive.CreateEntry(entryName: "_rels/FixedDocSeq.fdseq.rels", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the _rels/FixedDocSeq.fdseq.rels file, which defines the relationships for the fixed document sequence. The XML is structured according to the OPC specifications, with a root Relationships element that contains a Relationship element defining the relationship between the fixed document sequence and the fixed document. This file is essential for XPS readers to understand how to navigate from the fixed document sequence to the fixed document based on the defined relationships.
			using (StreamWriter writer = new(stream: fdseqRelsEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
				writer.WriteLine(value: "  <Relationship Id=\"rId1\" Type=\"http://schemas.microsoft.com/xps/2005/06/required-resource\" Target=\"../../../Resources/Dummy.ttf\"/>");
				writer.WriteLine(value: "</Relationships>");
			}
			// Create the FixedDocSeq.fdseq entry, which defines the fixed document sequence for the XPS document. This file is required by the XPS format and specifies the sequence of fixed documents in the XPS document. The fixed document sequence is defined according to the XPS specifications, which are used to structure the document.
			ZipArchiveEntry fdseqEntry = archive.CreateEntry(entryName: "FixedDocSeq.fdseq", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the FixedDocSeq.fdseq file, which defines the fixed document sequence for the XPS document. The XML is structured according to the XPS specifications, with a root FixedDocumentSequence element that contains a DocumentReference element referencing the fixed document. This file is essential for XPS readers to understand the sequence of fixed documents in the document and how to navigate to them based on the references.
			using (StreamWriter writer = new(stream: fdseqEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine(value: "<FixedDocumentSequence xmlns=\"http://schemas.microsoft.com/xps/2005/06\">");
				writer.WriteLine(value: "  <DocumentReference Source=\"/Documents/1/FixedDoc.fdoc\"/>");
				writer.WriteLine(value: "</FixedDocumentSequence>");
			}
			// Define constants for page dimensions, margins, and line height. These values are used to layout the content on each page of the XPS document. The page width is set to 816 units, the page height is set to 1056 units, the starting Y position for content is set to 96 units, the bottom margin is set to 960 units, and the line height is set to 16 units. These values can be adjusted as needed to fit the desired layout and formatting for the XPS document.
			const int pageHeight = 1056;
			const int startY = 96;
			const int marginB = 960;
			const int lineHeight = 16;
			int currentY = startY;
			// Create a list to keep track of the page entries that will be added to the XPS document. This list is used to generate the relationships for the fixed document sequence and to reference the pages in the fixed document. Each page entry corresponds to a page in the XPS document, and the list allows for easy management of these entries as they are created.
			List<string> pageEntries = [];
			int pageNumber = 1;
			StringBuilder currentPageBuilder = new();
			// Define a local function to start a new page in the XPS document. This function initializes the currentPageBuilder with the necessary XML structure for a fixed page, including the title and page number. The title is displayed at the top of the page, and the current Y position is updated to account for the space taken by the title. This function is called whenever a new page needs to be started, such as when the content exceeds the bottom margin.
			void StartNewPage()
			{
				currentPageBuilder.Clear();
				currentPageBuilder.AppendLine(value: "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				currentPageBuilder.AppendLine(value: "<FixedPage Width=\"816\" Height=\"1056\" xmlns=\"http://schemas.microsoft.com/xps/2005/06\" xml:lang=\"en-US\">");
				string safeTitle = System.Security.SecurityElement.Escape(str: title) ?? string.Empty;
				int titleY = Math.Max(val1: 0, val2: currentY - 24);
				currentPageBuilder.AppendLine(value: $"  <Glyphs Fill=\"#FF000000\" FontUri=\"/Resources/Dummy.ttf\" DeviceFontName=\"Arial\" FontRenderingEmSize=\"14\" OriginX=\"96\" OriginY=\"{titleY}\" UnicodeString=\"{safeTitle} - Page {pageNumber}\"/>");
				currentY += lineHeight * 2;
			}
			// Define a local function to finish the current page and add it to the XPS document. This function appends the closing tag for the fixed page, creates a new entry in the ZIP archive for the page, and writes the XML content for the page. It also creates a relationships entry for the page that references a dummy font resource. This function is called whenever a page is completed, such as when the content exceeds the bottom margin or when all lines have been processed.
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
			// Start a new page after finishing the current one.
			StartNewPage();
			// Write each line from the TextBox to the current page. If the current Y position exceeds the bottom margin, finish the current page and start a new one. Each line is added as a Glyphs element in the fixed page XML, with the appropriate font and positioning. The current Y position is updated after adding each line to ensure proper spacing between lines on the page.
			foreach (string line in textBox.Lines)
			{
				if (currentY > marginB)
				{
					FinishCurrentPage();
					pageNumber++;
					currentY = startY;
					StartNewPage();
				}
				string safeCell = System.Security.SecurityElement.Escape(str: line) ?? string.Empty;
				if (!string.IsNullOrEmpty(value: safeCell))
				{
					currentPageBuilder.AppendLine(value: $"  <Glyphs Fill=\"#FF000000\" FontUri=\"/Resources/Dummy.ttf\" DeviceFontName=\"Arial\" FontRenderingEmSize=\"12\" OriginX=\"96\" OriginY=\"{currentY}\" UnicodeString=\"{safeCell}\"/>");
				}
				currentY += lineHeight;
			}
			// After processing all lines, finish the current page to ensure that any remaining content is added to the XPS document. This is important to include the last page of content, which may not have been finished if the loop ended without exceeding the bottom margin.
			FinishCurrentPage();
			// Create the relationships entry for the fixed document, which references all the pages in the document. This file is required by the XPS format and specifies the relationships between the fixed document and its pages. The relationships are defined according to the OPC specifications, which are used by XPS to structure the document.
			ZipArchiveEntry fdocRelsEntry = archive.CreateEntry(entryName: "Documents/1/_rels/FixedDoc.fdoc.rels", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the relationships of the fixed document, which references all the pages in the document. The XML is structured according to the OPC specifications, with a root Relationships element that contains a Relationship element for each page in the document. This file is essential for XPS readers to understand how to navigate from the fixed document to its pages based on the defined relationships.
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
			// Create the fixed document entry, which references all the pages in the document. This file is required by the XPS format and specifies the content of the fixed document, including references to its pages. The fixed document is defined according to the XPS specifications, which are used to structure the document.
			ZipArchiveEntry fdocEntry = archive.CreateEntry(entryName: "Documents/1/FixedDoc.fdoc", compressionLevel: CompressionLevel.Optimal);
			// Write the XML content for the fixed document, which references all the pages in the document. The XML is structured according to the XPS specifications, with a root FixedDocument element that contains PageContent elements for each page in the document. This file is essential for XPS readers to understand how to display the content of the document based on its pages.
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
			// Create a dummy font entry, which is required by the XPS format for rendering text. This entry references a dummy TrueType font file, which is included in the Resources folder of the XPS document. The font file is obfuscated according to the XPS specifications, and it allows XPS readers to render the text content of the document using a standard font.
			ZipArchiveEntry fontEntry = archive.CreateEntry(entryName: "Resources/Dummy.ttf", compressionLevel: CompressionLevel.NoCompression);
			// Write the dummy font data to the font entry. In this example, we are writing a simple placeholder string "DUMMY" to the font file. In a real implementation, you would include an actual obfuscated TrueType font file that meets the requirements of the XPS format. The content is encoded in ASCII, as required by the XPS specifications for obfuscated fonts.
			using (StreamWriter writer = new(stream: fontEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "DUMMY");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "XPS", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a CSV file with each line as a separate row.</summary>
	/// <remarks>Each line from the TextBox is written as a single CSV field enclosed in double quotes. The title is written as the first row. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as CSV rows. Cannot be null.</param>
	/// <param name="title">The title to write as the first row in the CSV file.</param>
	/// <param name="fileName">The full path and file name where the CSV file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsCsv(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title and each line from the TextBox as CSV rows with UTF-8 encoding. Each field is escaped using the EscapeCsvField helper method to ensure the CSV file is well-formed and can be opened in spreadsheet applications without issues.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the first row, followed by each line from the TextBox as a separate row in the CSV file. Fields are escaped to handle special characters.
			writer.WriteLine(value: EscapeCsvField(field: title));
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: EscapeCsvField(field: line));
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "CSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a TSV (Tab-Separated Values) file with each line as a separate row.</summary>
	/// <remarks>Each line from the TextBox is written as a separate row in the TSV file. The title is written as the first row. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as TSV rows. Cannot be null.</param>
	/// <param name="title">The title to write as the first row in the TSV file.</param>
	/// <param name="fileName">The full path and file name where the TSV file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsTsv(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title and each line from the TextBox as TSV rows with UTF-8 encoding.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the first row, followed by each line from the TextBox as a separate row in the TSV file.
			writer.WriteLine(value: title);
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: line);
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a PSV (Pipe-Separated Values) file with each line as a separate row.</summary>
	/// <remarks>Each line from the TextBox is written as a separate row in the PSV file. The title is written as the first row. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as PSV rows. Cannot be null.</param>
	/// <param name="title">The title to write as the first row in the PSV file.</param>
	/// <param name="fileName">The full path and file name where the PSV file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsPsv(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title and each line from the TextBox as PSV rows with UTF-8 encoding.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the first row, followed by each line from the TextBox as a separate row in the PSV file.
			writer.WriteLine(value: title);
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: line);
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PSV", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a WPS Spreadsheet (ET) file using CSV format.</summary>
	/// <remarks>ET files use CSV format for WPS Spreadsheet compatibility. Each line from the TextBox is written as a single escaped CSV field. The title is written as the first row. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as ET rows. Cannot be null.</param>
	/// <param name="title">The title to write as the first row in the ET file.</param>
	/// <param name="fileName">The full path and file name where the ET file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsEt(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the title and each line from the TextBox as CSV rows with UTF-8 encoding. Fields that contain special characters are escaped using the EscapeCsvField helper method to ensure the file is well-formed and can be opened in WPS Spreadsheet without issues.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the title as the first row, followed by each line from the TextBox as a separate row in the CSV file. Fields are escaped to handle special characters.
			writer.WriteLine(value: EscapeCsvField(field: title));
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: EscapeCsvField(field: line));
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "ET", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as an HTML file with the given title.</summary>
	/// <remarks>The method creates an HTML document with a heading for the title and the TextBox content as paragraphs. Special characters in the title and text are encoded using HTML entities to ensure a well-formed HTML document. The file is saved using UTF-8 encoding. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as HTML paragraphs. Cannot be null.</param>
	/// <param name="title">The title to use for the HTML document heading and title element.</param>
	/// <param name="fileName">The full path and file name where the HTML file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsHtml(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the content of the TextBox as an HTML document with UTF-8 encoding. The HTML document includes a DOCTYPE declaration, head with meta charset, title, and basic styling, and a body containing an H1 heading for the title and paragraphs for each line from the TextBox. Special characters are encoded using System.Net.WebUtility.HtmlEncode to ensure the HTML document is well-formed.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			// Write the HTML document structure, including the title and each line from the TextBox as a separate paragraph. All text is HTML-encoded to ensure valid output.
			writer.WriteLine(value: "<!DOCTYPE html>");
			writer.WriteLine(value: "<html><head><meta charset=\"utf-8\">");
			writer.WriteLine(value: $"<title>{System.Net.WebUtility.HtmlEncode(value: title)}</title>");
			writer.WriteLine(value: "<style>body{font-family:sans-serif}p{margin:0.2em 0}</style>");
			writer.WriteLine(value: "</head><body>");
			writer.WriteLine(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: $"<p>{System.Net.WebUtility.HtmlEncode(value: line)}</p>");
			}
			writer.WriteLine(value: "</body></html>");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "HTML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as an XML file with the given title.</summary>
	/// <remarks>The method creates an XML document with a root element named "data" and a "title" attribute. Each line from the TextBox is represented as a "line" element. Special characters in the title and line data are properly escaped to ensure the XML document is well-formed. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as XML elements. Cannot be null.</param>
	/// <param name="title">The title written as a "title" attribute on the root element.</param>
	/// <param name="fileName">The full path and file name where the XML file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsXml(TextBox textBox, string title, string fileName)
	{
		// Use an XmlWriter to write the output file in XML format with UTF-8 encoding. The XML document has a root element named "data" with a "title" attribute. Each line from the TextBox is represented as a "line" element. Special characters in the title and line data are properly escaped by the XmlWriter to ensure the XML document is well-formed.
		try
		{
			// Configure the XmlWriter to produce indented output for readability.
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "data");
			xmlWriter.WriteAttributeString(localName: "title", value: title);
			// Write each line from the TextBox as a separate "line" element in the XML document.
			foreach (string line in textBox.Lines)
			{
				xmlWriter.WriteElementString(localName: "line", value: line);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "XML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a DocBook XML document.</summary>
	/// <remarks>The method creates a DocBook XML document with an "article" root element, a "title" element for the document title, and a "section" containing "para" elements for each line from the TextBox. Special characters in the title and line data are encoded to ensure the XML document is well-formed and can be processed by DocBook tools. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as DocBook paragraphs. Cannot be null.</param>
	/// <param name="title">The document title written in the &lt;title&gt; element.</param>
	/// <param name="fileName">The full path and file name where the DocBook XML file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsDocBook(TextBox textBox, string title, string fileName)
	{
		// Use an XmlWriter to write the output file in DocBook XML format with UTF-8 encoding. The XML document has an "article" root element with version "5.0", a "title" element for the document title, and a "section" containing "para" elements for each line from the TextBox.
		try
		{
			string docbookNs = "http://docbook.org/ns/docbook";
			// Configure the XmlWriter to produce indented output for readability.
			XmlWriterSettings settings = new() { Indent = true };
			using XmlWriter xmlWriter = XmlWriter.Create(outputFileName: fileName, settings: settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement(localName: "article", ns: docbookNs);
			xmlWriter.WriteAttributeString(localName: "version", value: "5.0");
			xmlWriter.WriteElementString(localName: "title", ns: docbookNs, value: title);
			// Write a section element containing each line from the TextBox as a "para" element.
			xmlWriter.WriteStartElement(localName: "section", ns: docbookNs);
			xmlWriter.WriteElementString(localName: "title", ns: docbookNs, value: title);
			foreach (string line in textBox.Lines)
			{
				xmlWriter.WriteElementString(localName: "para", ns: docbookNs, value: line);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "DocBook", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a JSON file with the given title.</summary>
	/// <remarks>The method creates a JSON document with a root object containing a "title" property and a "lines" array property. Each element in the "lines" array corresponds to a line from the TextBox. Special characters in the title and line data are properly escaped by the JsonSerializer to ensure the JSON document is well-formed. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as JSON array elements. Cannot be null.</param>
	/// <param name="title">The title written as the value of the "title" property at the root of the JSON object.</param>
	/// <param name="fileName">The full path and file name where the JSON file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsJson(TextBox textBox, string title, string fileName)
	{
		// Create a JSON document with a root object containing a "title" property and a "lines" array property. The JsonSerializer handles proper escaping of special characters to ensure the JSON document is well-formed.
		try
		{
			// Create an anonymous object to represent the JSON document structure, with the title and the lines from the TextBox.
			var doc = new { title, lines = textBox.Lines };
			string json = JsonSerializer.Serialize(value: doc, options: jsonSerializerOptions);
			File.WriteAllText(path: fileName, contents: json);
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "JSON", filePath: fileName);
		}
	}

	/// <summary>Escapes a value for use as a YAML single-quoted scalar.</summary>
	/// <param name="value">The value to escape.</param>
	/// <returns>A YAML single-quoted scalar that preserves backslashes and escapes embedded single quotes.</returns>
	private static string EscapeYamlSingleQuotedScalar(string? value)
	{
		return $"'{(value ?? string.Empty).Replace(oldValue: "'", newValue: "''")}'";
	}

	/// <summary>Saves the contents of the specified TextBox as a YAML file with the given title.</summary>
	/// <remarks>The method creates a YAML document with a "title" key and a "lines" sequence. Each element in the "lines" sequence corresponds to a line from the TextBox. Special characters in the title and line data are escaped by replacing double quotes with escaped double quotes to ensure the YAML document is well-formed. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as YAML sequence entries. Cannot be null.</param>
	/// <param name="title">The title written as the value of the "title" key at the root of the YAML document.</param>
	/// <param name="fileName">The full path and file name where the YAML file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsYaml(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the output file in YAML format with UTF-8 encoding. The YAML document has a "title" key and a "lines" sequence containing each line from the TextBox as a separate entry. Text values are emitted as YAML single-quoted scalars so backslashes are preserved literally and embedded single quotes are escaped safely.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "---");
			writer.WriteLine(value: $"title: {EscapeYamlSingleQuotedScalar(value: title)}");
			writer.WriteLine(value: $"created_at: {EscapeYamlSingleQuotedScalar(value: $"{DateTime.UtcNow:O}")}");
			writer.WriteLine(value: "lines:");
			// Write each line from the TextBox as a YAML sequence entry using YAML single-quoted scalars to preserve the original text content.
			foreach (string line in textBox.Lines)
			{
				writer.WriteLine(value: $"  - {EscapeYamlSingleQuotedScalar(value: line)}");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "YAML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a TOML file with the given title.</summary>
	/// <remarks>The method creates a TOML document with a "title" key and a "lines" array containing each line from the TextBox. Special characters in the title and line data are escaped using the EscapeToml helper method to ensure the TOML document is well-formed. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as TOML array entries. Cannot be null.</param>
	/// <param name="title">The title written as the value of the "title" key at the top of the TOML file.</param>
	/// <param name="fileName">The full path and file name where the TOML file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsToml(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the output file in TOML format with UTF-8 encoding. The TOML document has a "title" key and a "lines" array containing each line from the TextBox. Special characters are escaped using the EscapeToml helper method.
		try
		{
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: $"title = \"{EscapeToml(value: title)}\"");
			writer.WriteLine(value: $"created_at = {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
			writer.WriteLine();
			writer.Write(value: "lines = [");
			// Write each line from the TextBox as a TOML array entry, escaping special characters using the EscapeToml helper method.
			for (int i = 0; i < textBox.Lines.Length; i++)
			{
				if (i > 0)
				{
					writer.Write(value: ", ");
				}
				writer.Write(value: $"\"{EscapeToml(value: textBox.Lines[i])}\"");
			}
			writer.WriteLine(value: "]");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "TOML", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a SQL INSERT script.</summary>
	/// <remarks>The method creates a SQL script that includes a CREATE TABLE statement with a single TEXT column and INSERT INTO statements for each line from the TextBox. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. Special characters in the data are escaped by doubling single quotes to ensure the SQL script is well-formed. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as SQL INSERT rows. Cannot be null.</param>
	/// <param name="title">The title used as the table name in the SQL script. Non-alphanumeric characters are replaced by underscores.</param>
	/// <param name="fileName">The full path and file name where the SQL file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsSql(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the SQL script with UTF-8 encoding. The script includes a CREATE TABLE statement with a single TEXT column and INSERT INTO statements for each line from the TextBox. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. Special characters in the data are escaped by doubling single quotes.
		try
		{
			// Create a valid SQL table name from the title by replacing non-alphanumeric characters with underscores. If the resulting table name is empty, use a default name "Data".
			string tableName = new(value: [.. title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_')]);
			if (tableName.Length == 0)
			{
				tableName = "Data";
			}
			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.UTF8);
			writer.WriteLine(value: "-- Export generated by Planetoid-DB");
			writer.WriteLine(value: $"-- Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			writer.WriteLine();
			writer.WriteLine(value: $"CREATE TABLE IF NOT EXISTS [{tableName}] (");
			writer.WriteLine(value: "    [Content] TEXT");
			writer.WriteLine(value: ");");
			writer.WriteLine();
			writer.WriteLine(value: "BEGIN TRANSACTION;");
			// Write an INSERT INTO statement for each line from the TextBox. Single quotes in the data are escaped by doubling them.
			foreach (string line in textBox.Lines)
			{
				string escaped = line.Replace(oldValue: "'", newValue: "''");
				writer.WriteLine(value: $"INSERT INTO [{tableName}] ([Content]) VALUES ('{escaped}');");
			}
			writer.WriteLine(value: "COMMIT;");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "SQL", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a SQLite database file.</summary>
	/// <remarks>The method creates a SQLite database file with a single table containing a TEXT column for each line from the TextBox. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. The method uses parameterized SQL commands to insert the data, which ensures that special characters in the data are properly escaped. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as SQLite database rows. Cannot be null.</param>
	/// <param name="title">The title used as the table name in the SQLite database. Non-alphanumeric characters are replaced by underscores.</param>
	/// <param name="fileName">The full path and file name where the SQLite database will be created. If a file with the same name exists, it will be overwritten.</param>
	public static void SaveAsSqlite(TextBox textBox, string title, string fileName)
	{
		// Create a SQLite database file with a single table containing a TEXT column for each line from the TextBox. The table name is derived from the title parameter, with non-alphanumeric characters replaced by underscores. Parameterized SQL commands are used to safely insert the data.
		try
		{
			// Create a valid SQL table name from the title by replacing non-alphanumeric characters with underscores. If the resulting table name is empty, use a default name "Data".
			string tableName = new(value: [.. title.Select(selector: static c => char.IsLetterOrDigit(c: c) ? c : '_')]);
			if (tableName.Length == 0)
			{
				tableName = "Data";
			}
			// Delete any existing file to ensure a clean database creation.
			if (File.Exists(path: fileName))
			{
				File.Delete(path: fileName);
			}
			string connStr = $"Data Source={fileName};Version=3;";
			using SQLiteConnection connection = new(connectionString: connStr);
			connection.Open();
			// Create the table with a single TEXT column named "Content".
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = $"CREATE TABLE IF NOT EXISTS [{tableName}] ([Content] TEXT);";
				cmd.ExecuteNonQuery();
			}
			// Use a transaction for efficient batch insertion of all lines.
			using SQLiteTransaction transaction = connection.BeginTransaction();
			using SQLiteCommand insertCmd = connection.CreateCommand();
			insertCmd.CommandText = $"INSERT INTO [{tableName}] ([Content]) VALUES (@p0);";
			SQLiteParameter parameter = insertCmd.Parameters.Add(parameterName: "@p0", parameterType: System.Data.DbType.String);
			// Insert each line from the TextBox as a row in the SQLite table.
			foreach (string line in textBox.Lines)
			{
				parameter.Value = line;
				insertCmd.ExecuteNonQuery();
			}
			transaction.Commit();
			connection.Close();
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch any exceptions during SQLite operations, log the error, and show an error message to the user.
		catch (Exception ex)
		{
			ShowError(ex: ex, format: "SQLite", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a PostScript file.</summary>
	/// <remarks>The method generates a PostScript document with page headers containing the title and page number, and lines of text from the TextBox. Pagination is handled by starting a new page when the content exceeds the page height. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported to the PostScript file. Cannot be null.</param>
	/// <param name="title">The title to use as the page heading in the PostScript document.</param>
	/// <param name="fileName">The full path and file name where the PostScript file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsPostScript(TextBox textBox, string title, string fileName)
	{
		// Use a StreamWriter to write the content of the TextBox as a PostScript document with ASCII encoding. The method constructs a multi-page PostScript document with page headers containing the title and page number. Pagination is handled by starting a new page when the content exceeds the page height.
		try
		{
			// Define constants for page dimensions and layout.
			const int pageHeight = 842;
			const int startY = 750;
			const int marginY = 50;
			const int lineHeight = 14;

			// The 'using' statement ensures that the StreamWriter is properly disposed after use, which will flush and close the underlying file stream.
			using StreamWriter writer = new(path: fileName, append: false, encoding: Encoding.ASCII);
			// Write the PostScript header, including the document title and page setup.
			writer.WriteLine(value: "%!PS-Adobe-3.0");
			writer.WriteLine(value: $"%%Title: {EscapePostScript(input: title)}");
			writer.WriteLine(value: "%%Creator: Planetoid-DB");
			writer.WriteLine(value: $"%%CreationDate: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			writer.WriteLine(value: "%%EndComments");
			writer.WriteLine();

			int currentY = startY;
			int pageNumber = 1;

			// Start the first page.
			writer.WriteLine(value: $"%%Page: {pageNumber} {pageNumber}");
			writer.WriteLine(value: "/Helvetica findfont 14 scalefont setfont");
			writer.WriteLine(value: $"50 {pageHeight - 40} moveto ({EscapePostScript(input: title)} - Page {pageNumber}) show");
			writer.WriteLine(value: "/Helvetica findfont 10 scalefont setfont");

			// Write each line from the TextBox to the PostScript document. If the current Y position exceeds the bottom margin, finish the current page and start a new one.
			foreach (string line in textBox.Lines)
			{
				if (currentY < marginY)
				{
					// End the current page and start a new one.
					writer.WriteLine(value: "showpage");
					pageNumber++;
					currentY = startY;
					writer.WriteLine(value: $"%%Page: {pageNumber} {pageNumber}");
					writer.WriteLine(value: "/Helvetica findfont 14 scalefont setfont");
					writer.WriteLine(value: $"50 {pageHeight - 40} moveto ({EscapePostScript(input: title)} - Page {pageNumber}) show");
					writer.WriteLine(value: "/Helvetica findfont 10 scalefont setfont");
				}
				writer.WriteLine(value: $"50 {currentY} moveto ({EscapePostScript(input: line)}) show");
				currentY -= lineHeight;
			}
			// End the last page and write the PostScript trailer.
			writer.WriteLine(value: "showpage");
			writer.WriteLine(value: "%%EOF");
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "PostScript", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as an EPUB file.</summary>
	/// <remarks>The file is a proper compressed EPUB (ZIP) archive conforming to the EPUB 2 specification. The method creates the necessary structure, including the mimetype, container.xml, content.opf, toc.ncx, and content.xhtml files. Each line from the TextBox is written as a paragraph in the XHTML content. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be exported as EPUB content paragraphs. Cannot be null.</param>
	/// <param name="title">The EPUB book title used in metadata and content pages.</param>
	/// <param name="fileName">The full path and file name where the EPUB file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsEpub(TextBox textBox, string title, string fileName)
	{
		// Use a ZipArchive to create an EPUB file, which is essentially a ZIP archive containing specific XML and XHTML files. The method creates the necessary structure for a valid EPUB 2 document, including the mimetype, META-INF/container.xml, OEBPS/content.opf, OEBPS/toc.ncx, and OEBPS/content.xhtml files. Each line from the TextBox is added as a paragraph in the XHTML content. If an I/O or access error occurs, an error message is displayed to the user.
		try
		{
			// The 'using' statements ensure that the FileStream and ZipArchive are properly disposed after use, which will flush and close the underlying file stream and finalize the ZIP archive.
			using FileStream fs = new(path: fileName, mode: FileMode.Create);
			using ZipArchive archive = new(stream: fs, mode: ZipArchiveMode.Create);
			// Create the mimetype entry, which must be the first entry in the EPUB file and must be stored without compression.
			ZipArchiveEntry mimetypeEntry = archive.CreateEntry(entryName: "mimetype", compressionLevel: CompressionLevel.NoCompression);
			using (StreamWriter writer = new(stream: mimetypeEntry.Open(), encoding: Encoding.ASCII))
			{
				writer.Write(value: "application/epub+zip");
			}
			// Create the container.xml file in the META-INF directory, which specifies the location of the content.opf file.
			ZipArchiveEntry containerEntry = archive.CreateEntry(entryName: "META-INF/container.xml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: containerEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">");
				writer.WriteLine(value: "  <rootfiles><rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/></rootfiles>");
				writer.WriteLine(value: "</container>");
			}
			string safeTitle = System.Net.WebUtility.HtmlEncode(value: title) ?? string.Empty;
			// Create the content.opf file in the OEBPS directory with the metadata for the EPUB.
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
			// Create the toc.ncx file in the OEBPS directory for navigation.
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
			// Create the content.xhtml file in the OEBPS directory with the actual content.
			ZipArchiveEntry contentEntry = archive.CreateEntry(entryName: "OEBPS/content.xhtml", compressionLevel: CompressionLevel.Optimal);
			using (StreamWriter writer = new(stream: contentEntry.Open(), encoding: Encoding.UTF8))
			{
				writer.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				writer.WriteLine(value: "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
				writer.WriteLine(value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">");
				writer.WriteLine(value: $"<head><title>{safeTitle}</title>");
				writer.WriteLine(value: "<style type=\"text/css\">body{{font-family:sans-serif}}p{{margin:0.2em 0}}</style>");
				writer.WriteLine(value: "</head><body>");
				writer.WriteLine(value: $"<h1>{safeTitle}</h1>");
				// Write each line from the TextBox as a paragraph in the XHTML content. All text is HTML-encoded to ensure valid XHTML output.
				foreach (string line in textBox.Lines)
				{
					writer.WriteLine(value: $"<p>{System.Net.WebUtility.HtmlEncode(value: line)}</p>");
				}
				writer.WriteLine(value: "</body></html>");
			}
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "EPUB", filePath: fileName);
		}
	}

	/// <summary>Saves the contents of the specified TextBox as a MOBI file.</summary>
	/// <remarks>The method generates a MOBI file with a minimal header and a single HTML content record containing the TextBox data formatted as paragraphs. The MOBI file structure is constructed manually, including the necessary header fields and text records. The resulting MOBI file can be opened with compatible e-book readers that support the MOBI format. If an I/O or access error occurs, an error message is displayed to the user.</remarks>
	/// <param name="textBox">The TextBox control whose lines will be written as HTML paragraphs in the MOBI content. Cannot be null.</param>
	/// <param name="title">The book title embedded in the MOBI header and HTML content body.</param>
	/// <param name="fileName">The full path and file name where the MOBI file will be saved. If the file exists, it will be overwritten.</param>
	public static void SaveAsMobi(TextBox textBox, string title, string fileName)
	{
		// Build the HTML content for the MOBI file, construct the MOBI file structure with the necessary header fields and text records, and write the resulting binary data to the specified file. The HTML content is generated by encoding the title and each line from the TextBox as HTML paragraphs.
		try
		{
			// Build the HTML content for the MOBI file. The HTML includes a title, a heading with the title, and paragraphs for each line from the TextBox. Special characters are HTML-encoded to ensure valid output.
			StringBuilder html = new();
			html.Append(value: $"<html><head><meta charset=\"UTF-8\"><title>{System.Net.WebUtility.HtmlEncode(value: title)}</title></head><body>");
			html.Append(value: $"<h1>{System.Net.WebUtility.HtmlEncode(value: title)}</h1>");
			foreach (string line in textBox.Lines)
			{
				html.Append(value: $"<p>{System.Net.WebUtility.HtmlEncode(value: line)}</p>");
			}
			html.Append(value: "</body></html>");
			byte[] bodyData = Encoding.UTF8.GetBytes(s: html.ToString());
			// Split the HTML content into chunks of 4096 bytes to create multiple text records for the MOBI file.
			List<byte[]> textRecords = [];
			for (int i = 0; i < bodyData.Length; i += 4096)
			{
				int len = Math.Min(val1: 4096, val2: bodyData.Length - i);
				byte[] chunk = new byte[len];
				Array.Copy(sourceArray: bodyData, sourceIndex: i, destinationArray: chunk, destinationIndex: 0, length: len);
				textRecords.Add(item: chunk);
			}
			// Build the MOBI header record with the necessary fields for the MOBI file structure.
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
			// Write the MOBI file structure, including the PDB header, record offsets, header record, text records, and EOF record.
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
			// If the save operation completes successfully, show a success message to the user.
			ShowSuccess();
		}
		// Catch IO-related exceptions such as IOException and UnauthorizedAccessException, log the error, and show an error message to the user.
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			ShowError(ex: ex, format: "MOBI", filePath: fileName);
		}
	}

	#endregion
}