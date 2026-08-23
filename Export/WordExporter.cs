/*
 * File:        WordExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a Word file.
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

/// <summary>Represents a Word exporter for exporting database information to a Word file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a Word file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class WordExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the WordExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the WordExporter class.</remarks>
	public string Extension => "docx";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "Word files (*.docx)|*.docx|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as Word file";

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
	/// <remarks>This method exports the selected data to a Word file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to Word file: {filePath}");
		// Create a StringBuilder to build the content of the Word file
		StringBuilder sb = new();
		// Append the Word content to the StringBuilder
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Append the key and value in the format "Key: Value" to the StringBuilder
			_ = sb.Append(value: "<w:p><w:r><w:t xml:space=\"preserve\">");
			_ = sb.Append(value: $"{EscapeXml(value: kvp.Key)}: {EscapeXml(value: kvp.Value)}");
			_ = sb.Append(value: "</w:t></w:r></w:p>");
		}
		// Define the XML content for the content types of the Word document
		string contentTypesXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
				<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
				<Default Extension="xml" ContentType="application/xml"/>
				<Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
			</Types>
			""";
		// Define the XML content for the root relationships of the Word document
		string rootRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
			</Relationships>
			""";
		// Define the XML content for the main document of the Word document, including the paragraphs with the orbital elements
		string documentXml = $"""
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<w:document xmlns:wpc="http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas"
				xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
				xmlns:o="urn:schemas-microsoft-com:office:office"
				xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"
				xmlns:m="http://schemas.openxmlformats.org/officeDocument/2006/math"
				xmlns:v="urn:schemas-microsoft-com:vml"
				xmlns:wp14="http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing"
				xmlns:wp="http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"
				xmlns:w10="urn:schemas-microsoft-com:office:word"
				xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"
				xmlns:w14="http://schemas.microsoft.com/office/word/2010/wordml"
				xmlns:wpg="http://schemas.microsoft.com/office/word/2010/wordprocessingGroup"
				xmlns:wpi="http://schemas.microsoft.com/office/word/2010/wordprocessingInk"
				xmlns:wne="http://schemas.microsoft.com/office/word/2006/wordml"
				xmlns:wps="http://schemas.microsoft.com/office/word/2010/wordprocessingShape"
				mc:Ignorable="w14 wp14">
				<w:body>
					{sb}
					<w:sectPr>
						<w:pgSz w:w="11906" w:h="16838"/>
						<w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440" w:header="708" w:footer="708" w:gutter="0"/>
						<w:cols w:space="708"/>
						<w:docGrid w:linePitch="360"/>
					</w:sectPr>
				</w:body>
			</w:document>
			""";
		// Create a new FileStream to write the Word document content to the specified file
		using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the Word document as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name and content
		void AddEntry(string entryName, string content)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: CompressionLevel.Optimal);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid Word document
		AddEntry(entryName: "[Content_Types].xml", content: contentTypesXml);
		AddEntry(entryName: "_rels/.rels", content: rootRelsXml);
		AddEntry(entryName: "word/document.xml", content: documentXml);

		// Write the content of the StringBuilder to the specified file path
		//File.WriteAllText(path: filePath, contents: sb.ToString());

		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to Word file: {filePath}");
	}
}