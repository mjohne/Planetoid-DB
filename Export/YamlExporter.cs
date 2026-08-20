/*
 * File:        YamlExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a YAML file.
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

/// <summary>Represents a YAML exporter for exporting database information to a YAML file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to a YAML file format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class YamlExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Initializes a new instance of the YamlExporter class.</summary>
	/// <remarks>This constructor initializes a new instance of the YamlExporter class.</remarks>
	public string Extension => "yaml";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as YAML";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString() ?? string.Empty;

	/// <summary>Exports the selected data to a YAML file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to a YAML file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to YAML file: {filePath}");
		// Create a StringBuilder to build the content of the YAML file
		StringBuilder sb = new();
		// Append each key-value pair from the selected data to the StringBuilder
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			// Append the key and value in the format "Key: Value" to the StringBuilder
			_ = sb.AppendLine(handler: $"{kvp.Key}: {kvp.Value}");
		}
		// Write the content of the StringBuilder to the specified file path
		File.WriteAllText(path: filePath, contents: sb.ToString());
	}
}