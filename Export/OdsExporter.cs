/*
 * File:        OdsExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a ODS file.
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

/// <summary>Represents a ODS exporter for exporting database information to a Word file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a ODS file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class OdsExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the OdsExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the ODsExporter class.</remarks>
	public string Extension => "dos";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "ODS files (*.ods)|*.ods|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as ODS file";

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
	/// <remarks>This method exports the selected data to a ODS file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to ODS file: {filePath}");
		// Create a StringBuilder to build the content of the ODS file
		StringBuilder sb = new();
		_ = sb.AppendLine(value: "<table:table-row><table:table-cell office:value-type=\"string\"><text:p>Element</text:p></table:table-cell><table:table-cell office:value-type=\"string\"><text:p>Value</text:p></table:table-cell></table:table-row>");
		// Append the RTF content to the StringBuilder
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			string elementName = EscapeXml(value: kvp.Key ?? string.Empty);
			string elementValue = EscapeXml(value: kvp.Value);
			// Append the key and value in the format "Key: Value" to the StringBuilder
			_ = sb.AppendLine(value: $"<table:table-row><table:table-cell office:value-type=\"string\"><text:p>{elementName}</text:p></table:table-cell><table:table-cell office:value-type=\"string\"><text:p>{elementValue}</text:p></table:table-cell></table:table-row>");
		}
		// Define the XML content for the content of the ODS file, including the table with the selected orbital elements
		string contentXml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-content
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:table="urn:oasis:names:tc:opendocument:xmlns:table:1.0"
				xmlns:text="urn:oasis:names:tc:opendocument:xmlns:text:1.0"
				office:version="1.2">
				<office:body>
					<office:spreadsheet>
						<table:table table:name="Export">
							{sb}
						</table:table>
					</office:spreadsheet>
				</office:body>
			</office:document-content>
			""";
		// Define the XML content for the styles of the ODS file
		string stylesXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-styles
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:styles/>
			</office:document-styles>
			""";
		// Define the XML content for the meta information of the ODS file
		string metaXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-meta
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:meta="urn:oasis:names:tc:opendocument:xmlns:meta:1.0"
				office:version="1.2">
				<office:meta>
					<meta:generator>Planetoid-DB</meta:generator>
				</office:meta>
			</office:document-meta>
			""";
		// Define the XML content for the settings of the ODS file
		string settingsXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-settings
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:settings/>
			</office:document-settings>
			""";
		// Define the XML content for the manifest of the ODS file
		string manifestXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<manifest:manifest xmlns:manifest="urn:oasis:names:tc:opendocument:xmlns:manifest:1.0" manifest:version="1.2">
				<manifest:file-entry manifest:full-path="/" manifest:media-type="application/vnd.oasis.opendocument.spreadsheet"/>
				<manifest:file-entry manifest:full-path="content.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="styles.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="meta.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="settings.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="META-INF/manifest.xml" manifest:media-type="text/xml"/>
			</manifest:manifest>
			""";
		// Create a new FileStream to write the ODS file content to the specified file
		using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the ODS file as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name, content, and compression level
		void AddEntry(string entryName, string content, CompressionLevel compressionLevel = CompressionLevel.Optimal)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: compressionLevel);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid ODS file
		AddEntry(entryName: "mimetype", content: "application/vnd.oasis.opendocument.spreadsheet", compressionLevel: CompressionLevel.NoCompression);
		AddEntry(entryName: "content.xml", content: contentXml);
		AddEntry(entryName: "styles.xml", content: stylesXml);
		AddEntry(entryName: "meta.xml", content: metaXml);
		AddEntry(entryName: "settings.xml", content: settingsXml);
		AddEntry(entryName: "META-INF/manifest.xml", content: manifestXml);
		// Write the content of the StringBuilder to the specified file path
		//File.WriteAllText(path: filePath, contents: sb.ToString());

		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to ODS file: {filePath}");

	}
}