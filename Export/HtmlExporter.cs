/*
 * File:        HtmlExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a HTML file.
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

/// <summary>Represents a HTML exporter for exporting database information to a HTML file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a HTML file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class HtmlExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the HtmlExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the HtmlExporter class.</remarks>
	public string Extension => "html";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "HTML files (*.docx)|*.html|All files (*.*)|*.*";
	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as HTML";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString() ?? string.Empty;

	/// <summary>Exports the selected data to a HTML file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to a HTML file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to HTML file: {filePath}");
		// Create a StringBuilder to build the content of the HTML file
		StringBuilder sb = new();
        // Append the HTML content to the StringBuilder
		_ = sb.AppendLine(value: "<!DOCTYPE html>");
		_ = sb.AppendLine(value: "<html lang=\"en\">");
		_ = sb.AppendLine(value: "\t<head>");
		_ = sb.AppendLine(value: "\t\t<meta charset=\"utf-8\">");
		_ = sb.AppendLine(value: "\t\t<meta name=\"description\" content=\"\">");
		_ = sb.AppendLine(value: "\t\t<meta name=\"keywords\" content=\"\">");
		_ = sb.AppendLine(value: "\t\t<meta name=\"generator\" content=\"Planetoid-DB\">");
		_ = sb.AppendLine(handler: $"\t\t<title>{exportTitle}</title>");
		_ = sb.AppendLine(value: "\t\t<style>");
		_ = sb.AppendLine(value: "\t\t\t* {font-family: sans-serif;}");
		_ = sb.AppendLine(value: "\t\t\t.italic {font-style: italic;}");
		_ = sb.AppendLine(value: "\t\t\t.bold {font-weight: bold;}");
		_ = sb.AppendLine(value: "\t\t\t.sup {vertical-align: super; font-size: smaller;}");
		_ = sb.AppendLine(value: "\t\t\t.sub {vertical-align: sub; font-size: smaller;}");
		_ = sb.AppendLine(value: "\t\t\t.block {width:350px; display: inline-block;}");
		_ = sb.AppendLine(value: "\t\t</style>");
		_ = sb.AppendLine(value: "\t</head>");
		_ = sb.AppendLine(value: "\t<body>");
		_ = sb.AppendLine(value: "\t\t<p>");
		// Append each key-value pair from the selected data to the StringBuilder
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Append the key and value in the format "Key: Value" to the StringBuilder
            _ = sb.AppendLine(handler: $"\t\t\t<span class=\"bold block\" xml:id=\"element-id-{kvp.Key}\">{kvp.Key}:</span> <span xml:id=\"value-id-{kvp.Key}\">{kvp.Value}</span><br />");
		}
        // Append the closing tags for the HTML content
		_ = sb.AppendLine(value: "\t\t</p>");
		_ = sb.AppendLine(value: "\t</body>");
		_ = sb.Append(value: "</html>");
		// Write the content of the StringBuilder to the specified file path
		File.WriteAllText(path: filePath, contents: sb.ToString());
	}
}