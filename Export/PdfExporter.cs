/*
 * File:        PdfExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a PDF file.
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
using System.Text;

namespace Planetoid_DB.Export;

/// <summary>Represents a PDF exporter for exporting database information to a Word file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a PDF file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class PdfExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the PdfExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the PdfExporter class.</remarks>
	public string Extension => "pdf";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as PDF file";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString() ?? string.Empty;

	private static string EscapePdfText(string value)
	{
		StringBuilder escaped = new();
		// Iterate through each character in the input string and escape special characters as needed
		foreach (char character in value)
		{
			_ = character switch
			{
				'\\' => escaped.Append(value: "\\\\"),
				'(' => escaped.Append(value: "\\("),
				')' => escaped.Append(value: "\\)"),
				'\r' or '\n' => escaped.Append(value: " "),
				_ => escaped.Append(value: character is >= ' ' and <= '~' ? character : '?'),
			};
		}
		// Return the escaped string
		return escaped.ToString();
	}

	/// <summary>Exports the selected data to a text file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to a PDF file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to PDF file: {filePath}");
		// Create a StringBuilder to build the content of the PDF file
		StringBuilder sb = new();
		// Append the PDF content to the StringBuilder using PDF syntax
		_ = sb.AppendLine(value: "BT");
		_ = sb.AppendLine(value: "/F1 12 Tf");
		_ = sb.AppendLine(value: "50 800 Td");
		_ = sb.AppendLine(value: "15 TL");
		// Iterate through the lines of text and append them to the PDF content with proper escaping and formatting
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Append the key and value in the format "Key: Value" to the StringBuilder
			_ = sb.AppendLine(value: $"({EscapePdfText(value: kvp.Key ?? string.Empty)}: {EscapePdfText(value: kvp.Value ?? string.Empty)}) Tj");
			_ = sb.AppendLine(value: "T*");
		}
		// Append the end text operator to the PDF content
		_ = sb.Append(value: "ET");
		// Convert the PDF content to a byte array using ASCII encoding
		Encoding asciiEncoding = Encoding.ASCII;
		byte[] contentBytes = asciiEncoding.GetBytes(s: sb.ToString());
		// Define the PDF objects for the catalog, pages, page, font, and content stream
		string object1 = "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n";
		string object2 = "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n";
		string object3 = "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n";
		string object4 = "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n";
		string object5 = $"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n{sb}\nendstream\nendobj\n";
		// Create a MemoryStream to hold the PDF content and write the PDF header, objects, cross-reference table, trailer, and EOF marker to the stream
		using MemoryStream memoryStream = new();
		// Define a local method to write ASCII-encoded strings to the MemoryStream
		void WriteAscii(string value)
		{
			byte[] bytes = asciiEncoding.GetBytes(s: value);
			memoryStream.Write(buffer: bytes, offset: 0, count: bytes.Length);
		}
		// Write the PDF header to the MemoryStream
		WriteAscii(value: "%PDF-1.4\n");
		// Create a list to hold the byte offsets of the PDF objects for the cross-reference table
		List<long> offsets = [0];
		// Write each PDF object to the MemoryStream and record its byte offset for the cross-reference table
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object1);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object2);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object3);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object4);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object5);
		// Write the cross-reference table to the MemoryStream using the recorded byte offsets of the PDF objects
		long xrefOffset = memoryStream.Position;
		WriteAscii(value: "xref\n");
		WriteAscii(value: "0 6\n");
		WriteAscii(value: "0000000000 65535 f \n");
		// Iterate through the recorded byte offsets of the PDF objects and write each offset to the cross-reference table in the required format
		for (int i = 1; i <= 5; i++)
		{
			_ = offsets[index: i];
			WriteAscii(value: $"{offsets[index: i]:0000000000} 00000 n \n");
		}
		// Write the trailer, startxref, and EOF marker to the MemoryStream to complete the PDF file structure
		WriteAscii(value: "trailer\n");
		WriteAscii(value: "<< /Size 6 /Root 1 0 R >>\n");
		WriteAscii(value: "startxref\n");
		WriteAscii(value: $"{xrefOffset}\n");
		WriteAscii(value: "%%EOF");
		// Write the content of the MemoryStream to the specified file path as a PDF file
		File.WriteAllBytes(path: filePath, bytes: memoryStream.ToArray());
		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to PDF file: {filePath}");
	}
}