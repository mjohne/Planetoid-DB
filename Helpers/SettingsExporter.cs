/*
 * File:        SettingsExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides static methods to export all application settings (user-scoped and application-scoped) from Settings to CSV, INI, XML, JSON, and YAML files.
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

using Planetoid_DB.Properties;

using System.Configuration;
using System.Text;
using System.Xml;

namespace Planetoid_DB;

/// <summary>Provides static methods to export all application settings (user-scoped and application-scoped)
 from <see cref="Settings"/> to CSV, INI, XML, JSON, and YAML files.</summary>
/// <remarks>Each setting is exported with its name, data type, scope (User/Application), and current value.
 Settings are discovered at run time via the <see cref="SettingsBase.Properties"/> collection so that
 any future additions to <c>Settings.settings</c> are picked up automatically.</remarks>
public static class SettingsExporter
{
	/// <summary>NLog logger for the class.</summary>
	/// <remarks>This logger is used to log messages and errors during the export process.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	#region Internal data model

	/// <summary>Represents a single setting entry that will be written to an export file.</summary>
	/// <remarks>This class is used internally to hold the name, type, scope, and value of a setting.</remarks>
	private sealed class SettingEntry
	{
		/// <summary>Gets the name of the setting.</summary>
		/// <remarks>The name corresponds to the property name in <c>Settings.settings</c>.</remarks>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the CLR type name of the setting value.</summary>
		/// <remarks>The type name is obtained from the <see cref="SettingsProperty.PropertyType"/> of the setting.</remarks>
		public string TypeName { get; init; } = string.Empty;

		/// <summary>Gets the scope of the setting ("User" or "Application").</summary>
		/// <remarks>The scope indicates whether the setting is user-scoped or application-scoped.</remarks>
		public string Scope { get; init; } = string.Empty;

		/// <summary>Gets the current value of the setting as a string.</summary>
		/// <remarks>The value is obtained from <c>Settings.Default[settingName]</c> and converted to a string.</remarks>
		public string Value { get; init; } = string.Empty;
	}

	#endregion

	#region Settings collection helper

	/// <summary>Reads all settings from <see cref="Settings.Default"/> and returns them as a list of <see cref="SettingEntry"/> objects.</summary>
	/// <returns>A list of <see cref="SettingEntry"/> instances, one per setting property.</returns>
	/// <remarks>This method collects all settings, determines their scope, type, and value, and returns them in a sorted order.</remarks>
	private static List<SettingEntry> CollectSettings()
	{
		// Read all settings from Settings.Default and return them as a list of SettingEntry objects.
		List<SettingEntry> entries = [];
		// Get the default settings instance
		Settings settings = Settings.Default;
		// Iterate over all properties in the settings
		foreach (SettingsProperty prop in settings.Properties)
		{
			// Determine the scope of the setting (User or Application)
			string scope = prop.Attributes[key: typeof(UserScopedSettingAttribute)] is UserScopedSettingAttribute
				? "User"
				: "Application";
			// Read the value of the setting, handling any exceptions that may occur
			string value;
			// Use a try-catch block to safely read the value of the setting
			try
			{
				value = settings[propertyName: prop.Name]?.ToString() ?? string.Empty;
			}
			// If an exception occurs (e.g., due to a missing or invalid setting), log a warning and use an empty string as the value
			catch (Exception ex)
			{
				logger.Warn(exception: ex, message: $"Could not read value for setting '{prop.Name}'.");
				value = string.Empty;
			}
			// Add the setting entry to the list
			entries.Add(item: new SettingEntry
			{
				Name = prop.Name,
				TypeName = prop.PropertyType.Name,
				Scope = scope,
				Value = value,
			});
		}
		// Sort the entries by scope and then by name for consistent output
		return [.. entries.OrderBy(keySelector: static e => { ArgumentNullException.ThrowIfNull(argument: e); return e.Scope; }).ThenBy(keySelector: static e => e.Name)];
	}

	#endregion

	#region CSV Export

	/// <summary>Exports all application settings to a CSV file.</summary>
	/// <param name="filePath">The full path of the target CSV file.</param>
	/// <remarks>The CSV file will have the columns Name, Type, Scope, and Value. All field values are RFC 4180 quoted.</remarks>
	public static void SaveAsCsv(string filePath)
	{
		// Log the export operation
		logger.Info(message: $"Exporting settings as CSV to '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during the export process
		try
		{
			// Collect all settings into a list of SettingEntry objects
			List<SettingEntry> entries = CollectSettings();
			// Use a StringBuilder to build the CSV content
			StringBuilder sb = new();
			// Write the CSV header line
			_ = sb.AppendLine(value: "\"Name\",\"Type\",\"Scope\",\"Value\"");
			// Write each setting entry as a CSV line, escaping fields as necessary
			foreach (SettingEntry e in entries)
			{
				// Escape each field for CSV and append the line to the StringBuilder
				_ = sb.AppendLine(value:
					$"{ExportEscapeHelper.EscapeCsvField(input: e.Name)}," +
					$"{ExportEscapeHelper.EscapeCsvField(input: e.TypeName)}," +
					$"{ExportEscapeHelper.EscapeCsvField(input: e.Scope)}," +
					$"{ExportEscapeHelper.EscapeCsvField(input: e.Value)}");
			}
			// Write the CSV content to the specified file path using UTF-8 encoding
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			// Show a success message to the user
			ExportFeedbackHelper.ShowSuccess();
		}
		// Catch any exceptions that occur during the export process and show an error message
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "CSV", filePath: filePath);
		}
	}

	#endregion

	#region INI Export

	/// <summary>Exports all application settings to an INI file.</summary>
	/// <param name="filePath">The full path of the target INI file.</param>
	/// <remarks>Settings are grouped into [User] and [Application] sections. Each line has the form <c>Name=Value</c>. A comment line above each entry records the CLR type of the value.</remarks>
	public static void SaveAsIni(string filePath)
	{
		// Log the export operation
		logger.Info(message: $"Exporting settings as INI to '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during the export process
		try
		{
			// Collect all settings into a list of SettingEntry objects
			List<SettingEntry> entries = CollectSettings();
			// Use a StringBuilder to build the INI content
			StringBuilder sb = new();
			// Track the current scope to group settings into sections
			string? currentScope = null;
			// Write each setting entry to the INI content, grouping by scope
			foreach (SettingEntry e in entries)
			{
				// If the scope has changed, write a new section header
				if (e.Scope != currentScope)
				{
					// If this is not the first section, add a blank line before the new section
					if (currentScope != null)
					{
						_ = sb.AppendLine();
					}
					// Write the section header for the new scope
					_ = sb.AppendLine(value: $"[{e.Scope}]");
					currentScope = e.Scope;
				}
				// Write a comment line with the type of the setting
				_ = sb.AppendLine(value: $"; Type: {e.TypeName}");
				_ = sb.AppendLine(value: $"{e.Name}={e.Value}");
			}
			// Write the INI content to the specified file path using UTF-8 encoding
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			// Show a success message to the user
			ExportFeedbackHelper.ShowSuccess();
		}
		// Catch any exceptions that occur during the export process and show an error message
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "INI", filePath: filePath);
		}
	}

	#endregion

	#region XML Export

	/// <summary>Exports all application settings to an XML file.</summary>
	/// <param name="filePath">The full path of the target XML file.</param>
	/// <remarks>The root element is <c>&lt;Settings&gt;</c>. Each child element is <c>&lt;Setting name="…" type="…" scope="…"&gt;value&lt;/Setting&gt;</c>.</remarks>
	public static void SaveAsXml(string filePath)
	{
		// Log the export operation
		logger.Info(message: $"Exporting settings as XML to '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during the export process
		try
		{
			// Collect all settings into a list of SettingEntry objects
			List<SettingEntry> entries = CollectSettings();
			// Configure XML writer settings for indentation and UTF-8 encoding
			XmlWriterSettings xmlSettings = new()
			{
				Indent = true,
				IndentChars = "  ",
				Encoding = Encoding.UTF8,
			};
			// Create an XmlWriter to write the XML content to the specified file path
			using XmlWriter writer = XmlWriter.Create(outputFileName: filePath, settings: xmlSettings);
			// Write the XML declaration and root element
			writer.WriteStartDocument();
			// Write the root <Settings> element
			writer.WriteStartElement(localName: "Settings");
			// Write each setting entry as a <Setting> element with attributes for name, type, and scope, and the value as the inner text
			foreach (SettingEntry e in entries)
			{
				writer.WriteStartElement(localName: "Setting");
				writer.WriteAttributeString(localName: "name", value: e.Name);
				writer.WriteAttributeString(localName: "type", value: e.TypeName);
				writer.WriteAttributeString(localName: "scope", value: e.Scope);
				writer.WriteString(text: e.Value);
				writer.WriteEndElement();
			}
			// Close the root <Settings> element and end the document
			writer.WriteEndElement();
			writer.WriteEndDocument();
			// Show a success message to the user
			ExportFeedbackHelper.ShowSuccess();
		}
		// Catch any exceptions that occur during the export process and show an error message
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "XML", filePath: filePath);
		}
	}

	#endregion

	#region JSON Export

	/// <summary>Exports all application settings to a JSON file.</summary>
	/// <param name="filePath">The full path of the target JSON file.</param>
	/// <remarks>The file contains a JSON array of objects, each with
	/// <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> fields.</remarks>
	public static void SaveAsJson(string filePath)
	{
		// Log the export operation
		logger.Info(message: $"Exporting settings as JSON to '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during the export process
		try
		{
			// Collect all settings into a list of SettingEntry objects
			List<SettingEntry> entries = CollectSettings();
			// Use a StringBuilder to build the JSON content
			StringBuilder sb = new();
			// Write the opening bracket for the JSON array
			_ = sb.AppendLine(value: "[");
			// Write each setting entry as a JSON object, properly escaped and formatted
			for (int i = 0; i < entries.Count; i++)
			{
				// Get the current setting entry
				SettingEntry e = entries[index: i];
				// Determine whether to add a comma after the object (not after the last one)
				string comma = i < entries.Count - 1 ? "," : string.Empty;
				_ = sb.AppendLine(value: "  {");
				_ = sb.AppendLine(value: $"    \"name\": {JsonEscape(value: e.Name)},");
				_ = sb.AppendLine(value: $"    \"type\": {JsonEscape(value: e.TypeName)},");
				_ = sb.AppendLine(value: $"    \"scope\": {JsonEscape(value: e.Scope)},");
				_ = sb.AppendLine(value: $"    \"value\": {JsonEscape(value: e.Value)}");
				_ = sb.AppendLine(value: $"  }}{comma}");
			}
			// Write the closing bracket for the JSON array
			_ = sb.AppendLine(value: "]");
			// Write the JSON content to the specified file path using UTF-8 encoding
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			// Show a success message to the user
			ExportFeedbackHelper.ShowSuccess();
		}
		// Catch any exceptions that occur during the export process and show an error message
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "JSON", filePath: filePath);
		}
	}

	/// <summary>Returns a JSON-encoded string literal (including surrounding double quotes).</summary>
	/// <param name="value">The raw string value to encode.</param>
	/// <returns>A JSON string literal.</returns>
	/// <remarks>This method escapes special characters according to the JSON specification (RFC 8259).</remarks>
	private static string JsonEscape(string value)
	{
		// Use a StringBuilder to construct the escaped JSON string literal
		StringBuilder sb = new(capacity: value.Length + 4);
		// Append the opening double quote
		_ = sb.Append('"');
		// Iterate over each character in the input string and escape as necessary
		foreach (char ch in value)
		{
			_ = ch switch
			{
				'"' => sb.Append(value: "\\\""),
				'\\' => sb.Append(value: "\\\\"),
				'\n' => sb.Append(value: "\\n"),
				'\r' => sb.Append(value: "\\r"),
				'\t' => sb.Append(value: "\\t"),
				'\b' => sb.Append(value: "\\b"),
				'\f' => sb.Append(value: "\\f"),
				_ => ch < ' ' ? sb.Append(value: $"\\u{(int)ch:x4}") : sb.Append(value: ch),
			};
		}
		// Append the closing double quote
		_ = sb.Append(value: '"');
		// Return the constructed JSON string literal
		return sb.ToString();
	}

	#endregion

	#region YAML Export

	/// <summary>Exports all application settings to a YAML file.</summary>
	/// <param name="filePath">The full path of the target YAML file.</param>
	/// <remarks>The file contains a YAML sequence. Each sequence item is a mapping with the keys <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c>. String values that require quoting are single-quoted with internal single quotes doubled.</remarks>
	public static void SaveAsYaml(string filePath)
	{
		// Log the export operation
		logger.Info(message: $"Exporting settings as YAML to '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during the export process
		try
		{
			// Collect all settings into a list of SettingEntry objects
			List<SettingEntry> entries = CollectSettings();
			// Use a StringBuilder to build the YAML content
			StringBuilder sb = new();
			// Write the YAML document start marker
			_ = sb.AppendLine(value: "---");
			// Write each setting entry as a YAML mapping item
			foreach (SettingEntry e in entries)
			{
				_ = sb.AppendLine(value: $"- name: {YamlScalar(value: e.Name)}");
				_ = sb.AppendLine(value: $"  type: {YamlScalar(value: e.TypeName)}");
				_ = sb.AppendLine(value: $"  scope: {YamlScalar(value: e.Scope)}");
				_ = sb.AppendLine(value: $"  value: {YamlScalar(value: e.Value)}");
			}
			// Write the YAML content to the specified file path using UTF-8 encoding
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			// Show a success message to the user
			ExportFeedbackHelper.ShowSuccess();
		}
		// Catch any exceptions that occur during the export process and show an error message
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "YAML", filePath: filePath);
		}
	}

	/// <summary>Returns a YAML scalar representation of <paramref name="value"/>.</summary>
	/// <param name="value">The raw string value.</param>
	/// <returns>A plain scalar when the value is safe, otherwise a single-quoted scalar.</returns>
	/// <remarks>This method ensures that the returned scalar is valid YAML. It uses single quotes for values that contain special characters or could be misinterpreted by a YAML parser.</remarks>
	private static string YamlScalar(string value)
	{
		// Return a plain scalar if the value is safe, otherwise return a single-quoted scalar.
		if (string.IsNullOrEmpty(value: value))
		{
			return "''";
		}
		// Use single-quoted style for any value that contains characters that could be misinterpreted by a YAML parser (colon, hash, special indicators, etc.).
		bool needsQuoting = value.Any(predicate: static c => c is ':' or '#' or '\'' or '"' or '\\' or '\n' or '\r' or '\t' or '&' or '*' or '!' or '|' or '>' or '{' or '}' or '[' or ']' or ',' or '?');
		// Also quote if the value starts with a special character that could be misinterpreted (e.g., '-', '.', '@', '`').
		if (!needsQuoting && (value[index: 0] is '-' or '.' or '@' or '`'))
		{
			needsQuoting = true;
		}
		// Also quote if the value is a YAML boolean or null literal (true, false, null, ~) to avoid misinterpretation.
		if (!needsQuoting)
		{
			return value;
		}
		// Single-quoted YAML: internal single quotes are doubled.
		return $"'{value.Replace(oldValue: "'", newValue: "''")}'";
	}

	#endregion
}
