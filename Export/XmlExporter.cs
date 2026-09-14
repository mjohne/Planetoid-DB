/*
 * File:        XmlExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Export
 * Description: Exports database information to a XML file.
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
using System.Xml;

namespace Planetoid_DB.Export;

/// <summary>Represents an XML exporter for exporting database information to an XML file.</summary>
/// <remarks>This class implements the IOrbitDataExporter interface and provides functionality to export database information to an XML file format.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal class XmlExporter : IOrbitDataExporter
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the file extension for XML exports.</summary>
	/// <remarks>This property provides the extension used when exporting database information to XML files.</remarks>
	public string Extension => "xml";

	/// <summary>Gets the file filter string for the save file dialog.</summary>
	/// <remarks>This property provides the filter string used in the save file dialog to specify the types of files that can be saved.</remarks>
	public string Filter => "XML files (*.xml)|*.xml|All files (*.*)|*.*";

	/// <summary>Gets the title for the save file dialog.</summary>
	/// <remarks>This property provides the title text displayed in the save file dialog.</remarks>
	public string Title => "Save database information as XML";

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Exports the selected data to an XML file.</summary>
	/// <param name="filePath">The path of the file to export to.</param>
	/// <param name="exportTitle">The title of the export.</param>
	/// <param name="selectedData">The data to be exported.</param>
	/// <remarks>This method exports the selected data to an XML file at the specified file path.</remarks>
	public void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData)
	{
		// Log the export operation
		logger.Info(message: $"Exporting data to XML file: {filePath}");
		XmlWriterSettings settings = new()
		{
			Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
			Indent = true
		};

		using XmlWriter writer = XmlWriter.Create(outputFileName: filePath, settings: settings);
		writer.WriteStartDocument();
		writer.WriteStartElement(localName: "MinorPlanet", ns: "https://github.com/mjohne/Planetoid-DB");
		writer.WriteElementString(localName: "Title", value: exportTitle);
		foreach (KeyValuePair<string, string> kvp in selectedData)
		{
			writer.WriteStartElement(localName: "Field");
			writer.WriteAttributeString(localName: "name", value: kvp.Key);
			writer.WriteAttributeString(localName: "value", value: kvp.Value);
			writer.WriteEndElement();
		}

		writer.WriteEndElement();
		writer.WriteEndDocument();
		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to XML file: {filePath}");
	}
}