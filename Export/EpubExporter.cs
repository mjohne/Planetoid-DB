/*
 * File:        EpubExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a EPUB file.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace Planetoid_DB.Export;

/// <summary>Represents a EPUB exporter for exporting database information to a Word file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a EPUB file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class EpubExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the EpubExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the EpubExporter class.</remarks>
	public string Extension => "epub";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "EPUB files (*.epub)|*.epub|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as EPUB file";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString() ?? string.Empty;

	private static string EscapeXml(string value) => value
				.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
				.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
				.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
				.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
				.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);

	/// <summary>Exports the selected data to a text file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to a EPUB file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to EPUB file: {filePath}");
		// Create a StringBuilder to build the content of the EPUB file
		StringBuilder sb = new();
		string title = $"Export for {exportTitle}";
		// Build the body content of the EPUB
		_ = sb.AppendLine(value: $"<h1>{EscapeXml(value: title)}</h1>");
		_ = sb.AppendLine(value: "<ul>");
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Append the key and value in the format "Key: Value" to the StringBuilder
			string elementName = EscapeXml(value: kvp.Key ?? string.Empty);
			string elementValue = EscapeXml(value: kvp.Value ?? string.Empty);
			_ = sb.AppendLine(value: $"<li><strong>{elementName}:</strong> {elementValue}</li>");
		}
		// Append the closing tags for the body content of the EPUB
		_ = sb.AppendLine(value: "</ul>");
		// Define the XML content for the EPUB structure
		string containerXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<container version="1.0" xmlns="urn:oasis:names:tc:opendocument:xmlns:container">
				<rootfiles>
					<rootfile full-path="OEBPS/content.opf" media-type="application/oebps-package+xml"/>
				</rootfiles>
			</container>
			""";
		// Define the XML content for the EPUB package and metadata
		string contentOpf = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<package xmlns="http://www.idpf.org/2007/opf" unique-identifier="bookid" version="3.0">
				<metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
					<dc:identifier id="bookid">urn:uuid:{Guid.NewGuid()}</dc:identifier>
					<dc:title>{EscapeXml(value: title)}</dc:title>
					<dc:language>en</dc:language>
				</metadata>
				<manifest>
					<item id="nav" href="nav.xhtml" media-type="application/xhtml+xml" properties="nav"/>
					<item id="content" href="content.xhtml" media-type="application/xhtml+xml"/>
				</manifest>
				<spine>
					<itemref idref="content"/>
				</spine>
			</package>
			""";
		// Define the XML content for the EPUB navigation document
		string navXhtml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<!DOCTYPE html>
			<html xmlns="http://www.w3.org/1999/xhtml" xmlns:epub="http://www.idpf.org/2007/ops" lang="en" xml:lang="en">
			<head>
				<meta charset="utf-8" />
				<title>Table of Contents</title>
			</head>
			<body>
				<nav epub:type="toc" id="toc">
					<h1>Contents</h1>
					<ol>
						<li><a href="content.xhtml">{EscapeXml(value: title)}</a></li>
					</ol>
				</nav>
			</body>
			</html>
			""";
		// Define the XML content for the EPUB main content document
		string contentXhtml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<!DOCTYPE html>
			<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
			<head>
				<meta charset="utf-8" />
				<title>{EscapeXml(value: title)}</title>
			</head>
			<body>
				{sb}
			</body>
			</html>
			""";
		// Create a new FileStream to write the EPUB content to the specified file
		using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to write the EPUB content in ZIP format
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Define a local function to add an entry to the ZIP archive with the specified name, content, and compression level
		void AddEntry(string entryName, string content, CompressionLevel compressionLevel = CompressionLevel.Optimal)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName, compressionLevel);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary entries to the ZIP archive for the EPUB structure and content
		AddEntry(entryName: "mimetype", content: "application/epub+zip", compressionLevel: CompressionLevel.NoCompression);
		AddEntry(entryName: "META-INF/container.xml", content: containerXml);
		AddEntry(entryName: "OEBPS/content.opf", content: contentOpf);
		AddEntry(entryName: "OEBPS/nav.xhtml", content: navXhtml);
		AddEntry(entryName: "OEBPS/content.xhtml", content: contentXhtml);

		// Write the content of the StringBuilder to the specified file path
		//File.WriteAllText(path: filePath, contents: sb.ToString());

		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to EPUB file: {filePath}");
	}
}