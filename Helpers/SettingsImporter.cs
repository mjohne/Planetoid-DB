/*
 * File:        SettingsImporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides static methods to import user-scoped application settings from CSV, INI, XML, JSON, and YAML files into Settings.Default.
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
using System.Globalization;
using System.Text;
using System.Xml;

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods to import user-scoped application settings from CSV, INI, XML, JSON, and YAML files into <see cref="Settings.Default"/>.</summary>
/// <remarks>Only user-scoped settings can be written back; application-scoped settings that appear in the file are silently skipped. After a successful import <see cref="Settings.Default"/> is saved automatically.</remarks>
public static class SettingsImporter
{
	/// <summary>NLog logger for the class.</summary>
	/// <remarks>This logger is used to log messages and errors during the import process.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	#region Internal data model

	/// <summary>Represents a single setting entry parsed from an import file.</summary>
	/// <remarks>This class is used internally to hold the name, type, scope, and value of a setting before it is applied to <see cref="Settings.Default"/>.</remarks>
	private sealed class SettingEntry
	{
		/// <summary>Gets the name of the setting.</summary>
		/// <remarks>This is the key used to look up the setting in <see cref="Settings.Default"/>.</remarks>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the CLR type name of the setting value.</summary>
		/// <remarks>This is used to convert the string value to the appropriate type when applying the setting.</remarks>
		public string TypeName { get; init; } = string.Empty;

		/// <summary>Gets the scope of the setting ("User" or "Application").</summary>
		/// <remarks>Only user-scoped settings can be applied; application-scoped settings are skipped.</remarks>
		public string Scope { get; init; } = string.Empty;

		/// <summary>Gets the value of the setting as a string.</summary>
		/// <remarks>This value will be converted to the appropriate type based on <see cref="TypeName"/> when applying the setting.</remarks>
		public string Value { get; init; } = string.Empty;
	}

	#endregion

	#region Apply helper

	/// <summary>Applies the parsed <paramref name="entries"/> to <see cref="Settings.Default"/>, skipping application-scoped settings or settings not present in the current settings file.</summary>
	/// <param name="entries">The list of setting entries to apply.</param>
	/// <returns>The number of settings that were successfully applied.</returns>
	/// <remarks>This method iterates over the provided entries, checks their scope and existence in the current settings, converts their values to the appropriate type, and saves the settings if any were applied.</remarks>
	private static int ApplySettings(IEnumerable<SettingEntry> entries)
	{
		// Get the current settings instance.
		Settings settings = Settings.Default;
		// Track how many settings were successfully applied.
		int applied = 0;
		// Iterate over each entry and attempt to apply it.
		foreach (SettingEntry entry in entries)
		{
			// Only user-scoped settings can be written back.
			if (!string.Equals(a: entry.Scope, b: "User", comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// Log a debug message and skip application-scoped settings.
				logger.Debug(message: $"Skipping application-scoped setting '{entry.Name}'.");
				continue;
			}
			// Check that the property actually exists in the current settings.
			SettingsProperty? prop = settings.Properties[entry.Name];
			// If the property is not found, log a warning and skip it.
			if (prop is null)
			{
				// Log a warning that the setting was not found in the current settings.
				logger.Warn(message: $"Setting '{entry.Name}' not found in current settings — skipped.");
				continue;
			}
			// Ensure the property is user-scoped (double-check at runtime).
			if (prop.Attributes[key: typeof(UserScopedSettingAttribute)] is not UserScopedSettingAttribute)
			{
				// Log a warning that the setting is not user-scoped and skip it.
				logger.Warn(message: $"Setting '{entry.Name}' is not user-scoped at runtime — skipped.");
				continue;
			}
			// Attempt to convert the string value to the appropriate type and apply it.
			try
			{
				// Convert the string value to the CLR type of the property.
				object? converted = ConvertValue(value: entry.Value, typeName: prop.PropertyType.Name, propertyType: prop.PropertyType);
				// Assign the converted value to the settings property.
				settings[entry.Name] = converted;
				// Increment the count of successfully applied settings.
				applied++;
				// Log a debug message indicating the setting was applied.
				logger.Debug(message: $"Applied setting '{entry.Name}' = '{entry.Value}'.");
			}
			// Catch any exceptions that occur during conversion or assignment and log a warning.
			catch (Exception ex)
			{
				// Log a warning that the setting could not be applied.
				logger.Warn(exception: ex, message: $"Could not apply setting '{entry.Name}': {ex.Message}");
				ExportFeedbackHelper.ShowErrorMessage(message: $"Could not apply setting '{entry.Name}': {ex.Message}");
			}
		}
		// If any settings were applied, save the settings to persist the changes.
		if (applied > 0)
		{
			// Save the settings to persist the changes made to user-scoped settings.
			settings.Save();
			// Log an info message indicating how many settings were saved.
			logger.Info(message: $"Saved {applied} imported setting(s).");
		}
		// Return the count of successfully applied settings.
		return applied;
	}

	/// <summary>Converts a string <paramref name="value"/> to the CLR type described by <paramref name="typeName"/>.</summary>
	/// <param name="value">The raw string value.</param>
	/// <param name="typeName">The CLR short type name (e.g. "Boolean", "Int32").</param>
	/// <param name="propertyType">The actual <see cref="Type"/> of the settings property.</param>
	/// <returns>The converted object, or <see langword="null"/> if <see cref="Convert.ChangeType(object, Type, IFormatProvider)"/> returns <see langword="null"/>.</returns>
	/// <remarks>All numeric conversions use <see cref="CultureInfo.InvariantCulture"/>. Unsupported types fall back to <see cref="Convert.ChangeType(object, Type, IFormatProvider)"/>. Throws if the input string is not in a format compatible with the target type.</remarks>
	private static object? ConvertValue(string value, string typeName, Type propertyType)
	{
		// Use a switch expression to convert the string value to the appropriate CLR type based on the type name.
		return typeName switch
		{
			"Boolean" => bool.Parse(value: value),
			"Int32" => int.Parse(s: value, provider: CultureInfo.InvariantCulture),
			"Int64" => long.Parse(s: value, provider: CultureInfo.InvariantCulture),
			"Double" => double.Parse(s: value, provider: CultureInfo.InvariantCulture),
			"Single" => float.Parse(s: value, provider: CultureInfo.InvariantCulture),
			"Decimal" => decimal.Parse(s: value, provider: CultureInfo.InvariantCulture),
			"String" => value,
			_ => Convert.ChangeType(value: value, conversionType: propertyType, provider: CultureInfo.InvariantCulture),
		};
	}

	#endregion

	#region CSV import

	/// <summary>Imports user-scoped application settings from a CSV file.</summary>
	/// <param name="filePath">The full path to the CSV file to import.</param>
	/// <remarks>The CSV file must have the header row <c>Name,Type,Scope,Value</c>. Fields may optionally be RFC 4180 quoted.</remarks>
	public static void LoadFromCsv(string filePath)
	{
		// Log an info message indicating the start of the CSV import process.
		logger.Info(message: $"Importing settings from CSV file '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during file reading or parsing.
		try
		{
			// Read all lines from the CSV file using UTF-8 encoding.
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			// Initialize a list to hold the parsed setting entries.
			List<SettingEntry> entries = [];
			// Skip header line (index 0).
			for (int i = 1; i < lines.Length; i++)
			{
				// Trim whitespace from the line and check if it's empty.
				string line = lines[i].Trim();
				// If the line is empty, skip it and continue to the next iteration.
				if (string.IsNullOrEmpty(value: line))
				{
					continue;
				}
				// Parse the CSV line into fields, respecting RFC 4180 quoting rules.
				string[] fields = ParseCsvLine(line: line);
				// Check if the parsed line has at least 4 fields (Name, Type, Scope, Value).
				if (fields.Length < 4)
				{
					// Log a warning if the CSV line has fewer than 4 fields and skip it.
					logger.Warn(message: $"CSV line {i + 1} has fewer than 4 fields — skipped.");
					continue;
				}
				// Create a new SettingEntry object with the parsed fields and add it to the entries list.
				entries.Add(new SettingEntry
				{
					Name = fields[0],
					TypeName = fields[1],
					Scope = fields[2],
					Value = fields[3],
				});
			}
			// Apply the parsed settings entries to Settings.Default and get the count of successfully applied settings.
			int applied = ApplySettings(entries: entries);
			// Show a success message indicating how many settings were imported and applied.
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		// Catch any exceptions that occur during the import process and log an error message.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from CSV '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "CSV", filePath: filePath);
		}
	}

	/// <summary>Parses a single CSV line, respecting RFC 4180 quoting rules.</summary>
	/// <param name="line">The raw CSV line text.</param>
	/// <returns>An array of field values with surrounding quotes removed and escaped quotes unescaped.</returns>
	/// <remarks>This method handles quoted fields, escaped quotes (""), and unquoted fields. It does not handle multiline fields.</remarks>
	private static string[] ParseCsvLine(string line)
	{
		// Initialize a list to hold the parsed fields.
		List<string> fields = [];
		// Initialize the position index to start parsing from the beginning of the line.
		int pos = 0;
		// Loop until the end of the line is reached.
		while (pos <= line.Length)
		{
			// Declare a variable to hold the current field value.
			string field;
			// Check if the current position is within the line and if the character is a double quote, indicating a quoted field.
			if (pos < line.Length && line[index: pos] == '"')
			{
				// Quoted field and skip opening quote
				pos++;
				// Use a StringBuilder to efficiently build the field value while handling escaped quotes.
				StringBuilder sb = new();
				// Loop until the closing quote is found or the end of the line is reached.
				while (pos < line.Length)
				{
					// Check if the current character is a double quote.
					if (line[index: pos] == '"')
					{
						// Check for escaped quote (""), which is represented by two consecutive double quotes.
						if (pos + 1 < line.Length && line[index: pos + 1] == '"')
						{
							// Escaped quote
							_ = sb.Append(value: '"');
							pos += 2;
						}
						// Closing quote found, break out of the loop.
						else
						{
							pos++; // skip closing quote
							break;
						}
					}
					// If the current character is not a double quote, append it to the StringBuilder and move to the next character.
					else
					{
						_ = sb.Append(value: line[index: pos]);
						pos++;
					}
				}
				// Assign the built string to the field variable.
				field = sb.ToString();
			}
			// If the current character is not a double quote, it is an unquoted field.
			else
			{
				// Unquoted field
				int start = pos;
				// Loop until a comma is found or the end of the line is reached to extract the unquoted field.
				while (pos < line.Length && line[index: pos] != ',')
				{
					pos++;
				}
				// Assign the substring representing the unquoted field to the field variable.
				field = line[start..pos];
			}
			// Add the parsed field to the list of fields.
			fields.Add(item: field);
			// Check if the current position is within the line and if the character is a comma, indicating the end of the current field.
			if (pos < line.Length && line[index: pos] == ',')
			{
				// skip comma
				pos++;
			}
			// If the end of the line is reached, break out of the loop.
			else
			{
				break;
			}
		}
		// Return the list of parsed fields as an array.
		return [.. fields];
	}

	#endregion

	#region INI import

	/// <summary>Imports user-scoped application settings from an INI file.</summary>
	/// <param name="filePath">The full path to the INI file to import.</param>
	/// <remarks>Settings are expected in sections named <c>[User]</c> and <c>[Application]</c>. Comment lines starting with <c>;</c> may carry the type hint in the form <c>; Type: TypeName</c>.</remarks>
	public static void LoadFromIni(string filePath)
	{
		// Log an info message indicating the start of the INI import process.
		logger.Info(message: $"Importing settings from INI file '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during file reading or parsing.
		try
		{
			// Read all lines from the INI file using UTF-8 encoding.
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			// Initialize a list to hold the parsed setting entries.
			List<SettingEntry> entries = [];
			// Initialize variables to track the current section (scope) and type hint while parsing the INI file.
			string currentScope = string.Empty;
			string currentTypeName = string.Empty;
			// Iterate over each line in the INI file.
			foreach (string rawLine in lines)
			{
				// Trim whitespace from the line and check if it's empty.
				string line = rawLine.Trim();
				// If the line is empty, skip it and continue to the next iteration.
				if (string.IsNullOrEmpty(value: line))
				{
					continue;
				}
				// Check if the line is a section header (e.g., [User] or [Application]).
				if (line.StartsWith(value: '[') && line.EndsWith(value: ']'))
				{
					currentScope = line[1..^1].Trim();
					continue;
				}
				// Check if the line is a comment line starting with a semicolon.
				if (line.StartsWith(value: ';'))
				{
					// Possible type hint: "; Type: <TypeName>"
					string comment = line[1..].Trim();
					// Check if the comment line starts with "Type:" (case-insensitive) to extract the type hint.
					if (comment.StartsWith(value: "Type:", comparisonType: StringComparison.OrdinalIgnoreCase))
					{
						currentTypeName = comment[5..].Trim();
					}
					continue;
				}
				// Check if the line contains an equals sign, indicating a key-value pair.
				int eqIndex = line.IndexOf(value: '=');
				// If there is no equals sign or it is at the start of the line, skip this line.
				if (eqIndex <= 0)
				{
					continue;
				}
				// Extract the name and value from the line, trimming whitespace.
				string name = line[..eqIndex].Trim();
				string value = line[(eqIndex + 1)..];
				// If the name is empty, skip this line.
				entries.Add(new SettingEntry
				{
					Name = name,
					TypeName = currentTypeName,
					Scope = currentScope,
					Value = value,
				});
				// Reset the type hint after using it for this entry.
				currentTypeName = string.Empty;
			}
			// Apply the parsed settings entries to Settings.Default and get the count of successfully applied settings.
			int applied = ApplySettings(entries: entries);
			// Show a success message indicating how many settings were imported and applied.
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		// Catch any exceptions that occur during the import process and log an error message.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from INI '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "INI", filePath: filePath);
		}
	}

	#endregion

	#region XML import

	/// <summary>Imports user-scoped application settings from an XML file.</summary>
	/// <param name="filePath">The full path to the XML file to import.</param>
	/// <remarks>The expected XML structure is the one produced by <c>SettingsExporter.SaveAsXml</c>: a <c>&lt;Settings&gt;</c> root element containing <c>&lt;Setting name="…" type="…" scope="…"&gt;value&lt;/Setting&gt;</c> children.</remarks>
	public static void LoadFromXml(string filePath)
	{
		// Log an info message indicating the start of the XML import process.
		logger.Info(message: $"Importing settings from XML file '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during file reading or parsing.
		try
		{
			// Initialize a list to hold the parsed setting entries.
			List<SettingEntry> entries = [];
			// Load the XML document from the specified file path.
			XmlDocument doc = new();
			// Load the XML document from the specified file path.
			doc.Load(filename: filePath);
			// Select all <Setting> nodes under the <Settings> root element.
			XmlNodeList? settingNodes = doc.SelectNodes(xpath: "/Settings/Setting");
			// If any <Setting> nodes are found, iterate over them to extract their attributes and inner text.
			if (settingNodes is not null)
			{
				// Iterate over each <Setting> node to extract its attributes and inner text.
				foreach (XmlNode node in settingNodes)
				{
					// If the node has no attributes, skip it and continue to the next node.
					if (node.Attributes is null)
					{
						continue;
					}
					// Extract the "name", "type", and "scope" attributes, using empty strings as defaults if they are missing.
					string name = node.Attributes["name"]?.Value ?? string.Empty;
					string typeName = node.Attributes["type"]?.Value ?? string.Empty;
					string scope = node.Attributes["scope"]?.Value ?? string.Empty;
					string value = node.InnerText;
					// If the name is empty, skip this setting and continue to the next node.
					if (string.IsNullOrEmpty(value: name))
					{
						continue;
					}
					// Create a new SettingEntry object with the extracted attributes and inner text, and add it to the entries list.
					entries.Add(new SettingEntry
					{
						Name = name,
						TypeName = typeName,
						Scope = scope,
						Value = value,
					});
				}
			}
			// Apply the parsed settings entries to Settings.Default and get the count of successfully applied settings.
			int applied = ApplySettings(entries: entries);
			// Show a success message indicating how many settings were imported and applied.
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		// Catch any exceptions that occur during the import process and log an error message.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from XML '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "XML", filePath: filePath);
		}
	}

	#endregion

	#region JSON import

	/// <summary>Imports user-scoped application settings from a JSON file.</summary>
	/// <param name="filePath">The full path to the JSON file to import.</param>
	/// <remarks>The expected format is the one produced by <c>SettingsExporter.SaveAsJson</c>: a JSON array of objects with <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> fields.</remarks>
	public static void LoadFromJson(string filePath)
	{
		// Log an info message indicating the start of the JSON import process.
		logger.Info(message: $"Importing settings from JSON file '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during file reading or parsing.
		try
		{
			// Read the entire JSON file content as a string using UTF-8 encoding.
			string json = File.ReadAllText(path: filePath, encoding: Encoding.UTF8);
			// Parse the JSON string into a list of SettingEntry objects using a custom parser.
			List<SettingEntry> entries = ParseJsonArray(json: json);
			// Apply the parsed settings entries to Settings.Default and get the count of successfully applied settings.
			int applied = ApplySettings(entries: entries);
			// Show a success message indicating how many settings were imported and applied.
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		// Catch any exceptions that occur during the import process and log an error message.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from JSON '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "JSON", filePath: filePath);
		}
	}

	/// <summary>Parses a JSON array of setting objects produced by <c>SettingsExporter.SaveAsJson</c>.</summary>
	/// <param name="json">The raw JSON string.</param>
	/// <returns>A list of <see cref="SettingEntry"/> instances.</returns>
	/// <remarks>This is a simple hand-rolled parser that handles the specific format produced by the exporter. It does not attempt to handle arbitrary JSON.</remarks>
	private static List<SettingEntry> ParseJsonArray(string json)
	{
		// Remove whitespace and newlines for easier parsing.
		List<SettingEntry> entries = [];
		// Extract each object between { and }
		int pos = 0;
		// Loop through the JSON string to find and parse each object in the array.
		while (pos < json.Length)
		{
			// Find the next opening brace for an object.
			int start = json.IndexOf(value: '{', startIndex: pos);
			// If no opening brace is found, break out of the loop.
			if (start < 0)
			{
				logger.Warn(message: $"No opening brace found in JSON at position {pos} — stopping parse.");
				break;
			}
			// Find the matching closing brace for the object, taking into account nested braces and strings.
			int end = -1;
			bool inString = false;
			bool escape = false;
			int depth = 0;
			// Loop through the JSON string starting from the opening brace to find the corresponding closing brace.
			for (int i = start; i < json.Length; i++)
			{
				// Get the current character in the JSON string.
				char ch = json[i];
				// Handle string literals and escape sequences to avoid misinterpreting braces inside strings.
				if (inString)
				{
					// Handle escape sequences inside strings to avoid misinterpreting escaped quotes.
					if (escape)
					{
						escape = false;
						continue;
					}
					// Handle escape sequences inside strings to avoid misinterpreting escaped quotes.
					if (ch == '\\')
					{
						escape = true;
						continue;
					}
					// If a closing quote is found, exit the string context.
					if (ch == '"')
					{
						inString = false;
					}
					// Continue to the next character without processing braces inside strings.
					continue;
				}
				// If an opening quote is found, enter the string context.
				if (ch == '"')
				{
					inString = true;
					continue;
				}
				// Track the depth of nested braces to find the matching closing brace for the current object.
				if (ch == '{')
				{
					depth++;
					continue;
				}
				// If a closing brace is found, decrement the depth and check if it matches the opening brace.
				if (ch == '}')
				{
					depth--;
					if (depth == 0)
					{
						end = i;
						break;
					}
				}
			}
			// If no matching closing brace is found, log a warning and break out of the loop.
			if (end < 0)
			{
				logger.Warn(message: $"No matching closing brace found in JSON at position {start} — stopping parse.");
				break;
			}
			// Extract the substring representing the JSON object and parse it into a dictionary of fields.
			string obj = json[(start + 1)..end];
			Dictionary<string, string> fields = ParseJsonObject(obj: obj);
			// Extract the individual fields from the dictionary, using TryGetValue to handle missing keys gracefully.
			_ = fields.TryGetValue(key: "name", value: out string? name);
			_ = fields.TryGetValue(key: "type", value: out string? typeName);
			_ = fields.TryGetValue(key: "scope", value: out string? scope);
			_ = fields.TryGetValue(key: "value", value: out string? value);
			// Only add the entry if the name is not null or empty, as it is required for identifying the setting.
			if (!string.IsNullOrEmpty(value: name))
			{
				// Create a new SettingEntry object with the extracted fields and add it to the entries list.
				entries.Add(item: new SettingEntry
				{
					Name = name ?? string.Empty,
					TypeName = typeName ?? string.Empty,
					Scope = scope ?? string.Empty,
					Value = value ?? string.Empty,
				});
			}
			// Move the position index to the character after the closing brace to continue parsing the next object.
			pos = end + 1;
		}
		// Return the list of parsed SettingEntry objects.
		return entries;
	}

	/// <summary>Parses a flat JSON object body (the content between braces) into a string dictionary.</summary>
	/// <param name="obj">The raw text between <c>{</c> and <c>}</c>.</param>
	/// <returns>A dictionary of key/value string pairs.</returns>
	/// <remarks>This is a simple hand-rolled parser that handles the specific format produced by the exporter. It does not attempt to handle arbitrary JSON.</remarks>
	private static Dictionary<string, string> ParseJsonObject(string obj)
	{
		// Use a case-insensitive dictionary to store the parsed key/value pairs.
		Dictionary<string, string> result = new(comparer: StringComparer.OrdinalIgnoreCase);
		// Initialize the position index to start parsing from the beginning of the object string.
		int pos = 0;
		// Loop through the object string to find and parse each key/value pair.
		while (pos < obj.Length)
		{
			// Find opening quote for key
			int keyStart = obj.IndexOf(value: '"', startIndex: pos);
			if (keyStart < 0)
			{
				break;
			}
			int keyEnd = FindClosingQuote(text: obj, start: keyStart + 1);
			if (keyEnd < 0)
			{
				break;
			}
			string key = JsonUnescape(value: obj[(keyStart + 1)..keyEnd]);
			// Find colon after key
			int colon = obj.IndexOf(value: ':', startIndex: keyEnd + 1);
			if (colon < 0)
			{
				break;
			}
			pos = colon + 1;
			// Skip whitespace after colon
			while (pos < obj.Length && char.IsWhiteSpace(c: obj[index: pos]))
			{
				pos++;
			}

			if (pos >= obj.Length)
			{
				break;
			}
			// Determine if the value is a string (starts with a quote) or a non-string (number, boolean, null)
			string fieldValue;
			if (obj[index: pos] == '"')
			{
				int valEnd = FindClosingQuote(text: obj, start: pos + 1);
				if (valEnd < 0)
				{
					break;
				}
				fieldValue = JsonUnescape(value: obj[(pos + 1)..valEnd]);
				pos = valEnd + 1;
			}
			else
			{
				// Non-string value (number, boolean, null)
				int valEnd = pos;
				while (valEnd < obj.Length && obj[index: valEnd] != ',' && obj[index: valEnd] != '}')
				{
					valEnd++;
				}
				fieldValue = obj[pos..valEnd].Trim();
				pos = valEnd;
			}
			result[key] = fieldValue;
			// Skip comma
			while (pos < obj.Length && (obj[index: pos] == ',' || char.IsWhiteSpace(c: obj[index: pos])))
			{
				pos++;
			}
		}
		// Return the dictionary of parsed key/value pairs.
		return result;
	}

	/// <summary>Finds the index of the closing double-quote character, correctly skipping escaped quotes.</summary>
	/// <param name="text">The text to search in.</param>
	/// <param name="start">The position immediately after the opening quote.</param>
	/// <returns>The index of the closing quote, or <c>-1</c> if not found.</returns>
	/// <remarks>This method handles escaped quotes (\"), ensuring that the search for the closing quote does not terminate prematurely.</remarks>
	private static int FindClosingQuote(string text, int start)
	{
		// Start searching from the specified position, looking for the closing quote while skipping escaped quotes.
		int i = start;
		// Loop through the text until the end is reached.
		while (i < text.Length)
		{
			// If an escape character is found, skip the next character to avoid misinterpreting escaped quotes.
			if (text[index: i] == '\\')
			{
				// skip escaped character
				i += 2;
				continue;
			}
			// If a closing quote is found, return its index.
			if (text[index: i] == '"')
			{
				return i;
			}
			i++;
		}
		// If no closing quote is found, return -1 to indicate failure.
		return -1;
	}

	/// <summary>Unescapes a JSON string value (removes backslash escapes).</summary>
	/// <param name="value">The raw JSON string content (without surrounding quotes).</param>
	/// <returns>The unescaped string.</returns>
	/// <remarks>This method handles common JSON escape sequences such as \n, \r, \t, \\, \", and Unicode escapes (\uXXXX).</remarks>
	private static string JsonUnescape(string value)
	{
		// If the string does not contain any backslashes, return it as-is for efficiency.
		if (!value.Contains(value: '\\'))
		{
			return value;
		}
		// Use a StringBuilder to efficiently build the unescaped string, initializing it with the capacity of the input string length.
		StringBuilder sb = new(capacity: value.Length);
		int i = 0;
		// Loop through the input string, processing each character and handling escape sequences as needed.
		while (i < value.Length)
		{
			// If an escape character is found, handle the escape sequence.
			if (value[index: i] == '\\' && i + 1 < value.Length)
			{
				// Handle escape sequence based on the character following the backslash.
				_ = value[index: i + 1] switch
				{
					'"' => sb.Append(value: '"'),
					'\\' => sb.Append(value: '\\'),
					'n' => sb.Append(value: '\n'),
					'r' => sb.Append(value: '\r'),
					't' => sb.Append(value: '\t'),
					'b' => sb.Append(value: '\b'),
					'f' => sb.Append(value: '\f'),
					'u' when i + 5 < value.Length => sb.Append(value: (char)Convert.ToInt32(value: value.Substring(startIndex: i + 2, length: 4), fromBase: 16)),
					_ => sb.Append(value: value[index: i + 1]),
				};
				// Move the index forward by the appropriate number of characters based on the escape sequence length.
				i += value[index: i + 1] == 'u' ? 6 : 2;
			}
			// If no escape character is found, append the current character to the StringBuilder and move to the next character.
			else
			{
				_ = sb.Append(value: value[index: i]);
				i++;
			}
		}
		// Return the unescaped string built in the StringBuilder.
		return sb.ToString();
	}

	#endregion

	#region YAML import

	/// <summary>Imports user-scoped application settings from a YAML file.</summary>
	/// <param name="filePath">The full path to the YAML file to import.</param>
	/// <remarks>The expected format is the one produced by <c>SettingsExporter.SaveAsYaml</c>: a YAML sequence where each item is a mapping with <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> keys.</remarks>
	public static void LoadFromYaml(string filePath)
	{
		// Log an info message indicating the start of the YAML import process.
		logger.Info(message: $"Importing settings from YAML file '{filePath}'.");
		// Use a try-catch block to handle any exceptions that may occur during file reading or parsing.
		try
		{
			// Read all lines from the YAML file using UTF-8 encoding.
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			// Initialize a list to hold the parsed setting entries.
			List<SettingEntry> entries = [];
			// Initialize variables to track the current entry's name, type, scope, and value while parsing the YAML file.
			string name = string.Empty;
			string typeName = string.Empty;
			string scope = string.Empty;
			string value = string.Empty;
			bool inEntry = false;
			// Iterate over each line in the YAML file to parse the settings entries.
			foreach (string rawLine in lines)
			{
				// Trim whitespace from the line and check if it's empty or a separator (---).
				string line = rawLine.TrimEnd();
				if (string.IsNullOrWhiteSpace(value: line) || line == "---")
				{
					continue;
				}
				// Check if the line starts a new entry with "- name:".
				if (line.StartsWith(value: "- name:"))
				{
					// Save previous entry if complete
					if (inEntry && !string.IsNullOrEmpty(value: name))
					{
						entries.Add(item: new SettingEntry { Name = name, TypeName = typeName, Scope = scope, Value = value });
					}
					// Start new entry
					name = YamlUnquote(ParseYamlValue(line: line, key: "name"));
					typeName = string.Empty;
					scope = string.Empty;
					value = string.Empty;
					inEntry = true;
				}
				// Check if the line contains the "type:" key for the current entry.
				else if (inEntry && line.StartsWith(value: "  type:"))
				{
					typeName = YamlUnquote(scalar: ParseYamlValue(line: line, key: "type"));
				}
				// Check if the line contains the "scope:" key for the current entry.
				else if (inEntry && line.StartsWith(value: "  scope:"))
				{
					scope = YamlUnquote(scalar: ParseYamlValue(line: line, key: "scope"));
				}
				// Check if the line contains the "value:" key for the current entry.
				else if (inEntry && line.StartsWith(value: "  value:"))
				{
					value = YamlUnquote(scalar: ParseYamlValue(line: line, key: "value"));
				}
			}
			// Flush last entry
			if (inEntry && !string.IsNullOrEmpty(value: name))
			{
				entries.Add(item: new SettingEntry { Name = name, TypeName = typeName, Scope = scope, Value = value });
			}
			// Apply the parsed settings entries to Settings.Default and get the count of successfully applied settings.
			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		// Catch any exceptions that occur during the import process and log an error message.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from YAML '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "YAML", filePath: filePath);
		}
	}

	/// <summary>Extracts the scalar value after the colon for a YAML key on a single line.</summary>
	/// <param name="line">The YAML line, e.g. <c>- name: FooBar</c>.</param>
	/// <param name="key">The key to look for, e.g. <c>"name"</c>.</param>
	/// <returns>The trimmed scalar value, or an empty string if the key is not found.</returns>
	private static string ParseYamlValue(string line, string key)
	{
		// Handles both "- name: value" and "  name: value"
		// Construct the search string for the key followed by a colon.
		string search = $"{key}:";
		// Find the index of the search string in the line using ordinal comparison.
		int idx = line.IndexOf(value: search, comparisonType: StringComparison.Ordinal);
		// If the key is not found, return an empty string; otherwise, extract the substring after the colon and trim whitespace.
		return idx < 0 ? string.Empty : line[(idx + search.Length)..].Trim();
	}

	/// <summary>Removes YAML single-quote wrapping and unescapes doubled single quotes.</summary>
	/// <param name="scalar">A YAML scalar that may or may not be single-quoted.</param>
	/// <returns>The plain string value.</returns>
	private static string YamlUnquote(string scalar)
	{
		// If the scalar is single-quoted, remove the quotes and unescape doubled single quotes.
		if (scalar.Length >= 2 && scalar[0] == '\'' && scalar[^1] == '\'')
		{
			// Single-quoted scalar — unescape '' → '
			return scalar[1..^1].Replace(oldValue: "''", newValue: "'");
		}
		// Otherwise, return the scalar as-is.
		return scalar;
	}

	#endregion
}
