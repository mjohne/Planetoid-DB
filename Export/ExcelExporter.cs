/*
 * File:        ExcelExporter.cs
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

/// <summary>Represents a Excel exporter for exporting database information to a Word file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a Excel file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class ExcelExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the ExcelExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the ExcelExporter class.</remarks>
	public string Extension => "xlsx";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as Excel file";

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
	/// <remarks>This method exports the selected data to a Excel file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to Excel file: {filePath}");
		// Create a StringBuilder to build the content of the Excel file
		StringBuilder sb = new();
		// Append the selected orbital elements to the rows of the Excel sheet
		int excelRow = 2;
		// Append the RTF content to the StringBuilder
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Mark that there are selected elements to be included in the Excel document
			string elementName = EscapeXml(value: kvp.Key ?? string.Empty);
			string elementValue = EscapeXml(value: kvp.Value);
			// Append the checked item and its corresponding orbit element value as a new row in the Excel sheet XML content
			_ = sb.AppendLine(value: $"<row r=\"{excelRow}\"><c r=\"A{excelRow}\" t=\"inlineStr\"><is><t>{elementName}</t></is></c><c r=\"B{excelRow}\" t=\"inlineStr\"><is><t>{elementValue}</t></is></c></row>");
			excelRow++;
		}
		// Define the XML content for the content types of the Excel document
		string contentTypesXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
				<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
				<Default Extension="xml" ContentType="application/xml"/>
				<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
				<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
			</Types>
			""";
		// Define the XML content for the root relationships of the Excel document
		string rootRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
			</Relationships>
			""";
		// Define the XML content for the workbook of the Excel document, which references the worksheet containing the orbital elements
		string workbookXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
				<sheets>
					<sheet name="Export" sheetId="1" r:id="rId1"/>
				</sheets>
			</workbook>
			""";
		// Define the XML content for the workbook relationships of the Excel document, which defines the relationship to the worksheet containing the orbital elements
		string workbookRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
			</Relationships>
			""";
		// Define the XML content for the worksheet of the Excel document, which contains the rows with the selected orbital elements
		string worksheetXml = $"""
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
				<sheetData>
					{sb}
				</sheetData>
			</worksheet>
			""";
		// Create a new FileStream to write the Excel document content to the specified file
		using FileStream fileStream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the Excel document as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name and content
		void AddEntry(string entryName, string content)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: CompressionLevel.Optimal);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid Excel document
		AddEntry(entryName: "[Content_Types].xml", content: contentTypesXml);
		AddEntry(entryName: "_rels/.rels", content: rootRelsXml);
		AddEntry(entryName: "xl/workbook.xml", content: workbookXml);
		AddEntry(entryName: "xl/_rels/workbook.xml.rels", content: workbookRelsXml);
		AddEntry(entryName: "xl/worksheets/sheet1.xml", content: worksheetXml);
		// Write the content of the StringBuilder to the specified file path
		//File.WriteAllText(path: filePath, contents: sb.ToString());

		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to Excel file: {filePath}");
	}
}