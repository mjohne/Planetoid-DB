/*
 * File:        VcalExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a vCalender file.
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

/// <summary>Represents a text exporter for exporting database information to a vCalendar file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a vCalendar file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class VcalExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the VcalExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the VcalExporter class.</remarks>
	public string Extension => "vcal";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "vCalendar files (*.vcal)|*.vcal|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as vCalendar";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString() ?? string.Empty;

	/// <summary>Exports the selected data to a text file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to a text file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to vCalendar file: {filePath}");
		// Create a StringBuilder to build the content of the vCalendar file
		StringBuilder sb = new();
		// Add iCalendar headers and event details
		_ = sb.AppendLine(value: "BEGIN:VCALENDAR");
		_ = sb.AppendLine(value: "VERSION:1.0");
		_ = sb.AppendLine(value: "PRODID:-//Planetoid-DB//Orbit Data Export//EN");
		_ = sb.AppendLine(value: "BEGIN:VEVENT");
		// Add a unique identifier (UID) and timestamp for the event
		_ = sb.AppendLine(value: $"UID:{Guid.NewGuid()}@planetoid-db.de");
		_ = sb.AppendLine(value: $"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
		_ = sb.AppendLine(value: $"SUMMARY:Observation/Data for {exportTitle}");
		// Add the description with key-value pairs from the selected data
		string description = string.Join(separator: "\n", values: selectedData.Select(selector: static x => $"{x.Key}: {x.Value}"));
		_ = sb.AppendLine(value: $"DESCRIPTION:{description}");
		// Add the end of the event and calendar
		_ = sb.AppendLine(value: "END:VEVENT");
		_ = sb.AppendLine(value: "END:VCALENDAR");
		// Write the content of the StringBuilder to the specified file path
		File.WriteAllText(path: filePath, contents: sb.ToString());
		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to vCalendar file: {filePath}");
	}
}