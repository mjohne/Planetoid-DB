// Imports user-scoped application settings (from various file formats) into the current user profile via Settings.Default.

using NLog;

using Planetoid_DB.Properties;

using System.Configuration;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods to import user-scoped application settings from CSV, INI, XML, JSON, and YAML files
/// into <see cref="Settings.Default"/>.</summary>
/// <remarks>Only user-scoped settings can be written back; application-scoped settings that appear in the file
/// are silently skipped. After a successful import <see cref="Settings.Default"/> is saved automatically.</remarks>
public static class SettingsImporter
{
	/// <summary>NLog logger for the class.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	// -------------------------------------------------------------------------
	// Internal data model
	// -------------------------------------------------------------------------

	/// <summary>Represents a single setting entry parsed from an import file.</summary>
	private sealed class SettingEntry
	{
		/// <summary>Gets the name of the setting.</summary>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the CLR type name of the setting value.</summary>
		public string TypeName { get; init; } = string.Empty;

		/// <summary>Gets the scope of the setting ("User" or "Application").</summary>
		public string Scope { get; init; } = string.Empty;

		/// <summary>Gets the value of the setting as a string.</summary>
		public string Value { get; init; } = string.Empty;
	}

	// -------------------------------------------------------------------------
	// Apply helper
	// -------------------------------------------------------------------------

	/// <summary>Applies the parsed <paramref name="entries"/> to <see cref="Settings.Default"/>,
	/// skipping application-scoped settings or settings not present in the current settings file.</summary>
	/// <param name="entries">The list of setting entries to apply.</param>
	/// <returns>The number of settings that were successfully applied.</returns>
	private static int ApplySettings(IEnumerable<SettingEntry> entries)
	{
		Settings settings = Settings.Default;
		int applied = 0;

		foreach (SettingEntry entry in entries)
		{
			// Only user-scoped settings can be written back.
			if (!string.Equals(a: entry.Scope, b: "User", comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				logger.Debug(message: $"Skipping application-scoped setting '{entry.Name}'.");
				continue;
			}

			// Check that the property actually exists in the current settings.
			SettingsProperty? prop = settings.Properties[entry.Name];
			if (prop is null)
			{
				logger.Warn(message: $"Setting '{entry.Name}' not found in current settings — skipped.");
				continue;
			}

			// Ensure the property is user-scoped (double-check at runtime).
			if (prop.Attributes[typeof(UserScopedSettingAttribute)] is not UserScopedSettingAttribute)
			{
				logger.Warn(message: $"Setting '{entry.Name}' is not user-scoped at runtime — skipped.");
				continue;
			}

			try
			{
				object? converted = ConvertValue(value: entry.Value, typeName: prop.PropertyType.Name, propertyType: prop.PropertyType);
				settings[entry.Name] = converted;
				applied++;
				logger.Debug(message: $"Applied setting '{entry.Name}' = '{entry.Value}'.");
			}
			catch (Exception ex)
			{
				logger.Warn(exception: ex, message: $"Could not apply setting '{entry.Name}': {ex.Message}");
			}
		}

		if (applied > 0)
		{
			settings.Save();
			logger.Info(message: $"Saved {applied} imported setting(s).");
		}

		return applied;
	}

	/// <summary>Converts a string <paramref name="value"/> to the CLR type described by <paramref name="typeName"/>.</summary>
	/// <param name="value">The raw string value.</param>
	/// <param name="typeName">The CLR short type name (e.g. "Boolean", "Int32").</param>
	/// <param name="propertyType">The actual <see cref="Type"/> of the settings property.</param>
	/// <returns>The converted object, or the original string when no specific conversion is available.</returns>
	private static object? ConvertValue(string value, string typeName, Type propertyType)
	{
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

	// -------------------------------------------------------------------------
	// CSV
	// -------------------------------------------------------------------------

	/// <summary>Imports user-scoped application settings from a CSV file.</summary>
	/// <param name="filePath">The full path to the CSV file to import.</param>
	/// <remarks>The CSV file must have the header row <c>Name,Type,Scope,Value</c>.
	/// Fields may optionally be RFC 4180 quoted.</remarks>
	public static void LoadFromCsv(string filePath)
	{
		logger.Info(message: $"Importing settings from CSV file '{filePath}'.");
		try
		{
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			List<SettingEntry> entries = [];

			// Skip header line (index 0).
			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i].Trim();
				if (string.IsNullOrEmpty(value: line))
				{
					continue;
				}

				string[] fields = ParseCsvLine(line: line);
				if (fields.Length < 4)
				{
					logger.Warn(message: $"CSV line {i + 1} has fewer than 4 fields — skipped.");
					continue;
				}

				entries.Add(new SettingEntry
				{
					Name = fields[0],
					TypeName = fields[1],
					Scope = fields[2],
					Value = fields[3],
				});
			}

			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from CSV '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "CSV", filePath: filePath);
		}
	}

	/// <summary>Parses a single CSV line, respecting RFC 4180 quoting rules.</summary>
	/// <param name="line">The raw CSV line text.</param>
	/// <returns>An array of field values with surrounding quotes removed and escaped quotes unescaped.</returns>
	private static string[] ParseCsvLine(string line)
	{
		List<string> fields = [];
		int pos = 0;
		while (pos <= line.Length)
		{
			string field;
			if (pos < line.Length && line[pos] == '"')
			{
				// Quoted field
				pos++; // skip opening quote
				StringBuilder sb = new();
				while (pos < line.Length)
				{
					if (line[pos] == '"')
					{
						if (pos + 1 < line.Length && line[pos + 1] == '"')
						{
							// Escaped quote
							_ = sb.Append('"');
							pos += 2;
						}
						else
						{
							pos++; // skip closing quote
							break;
						}
					}
					else
					{
						_ = sb.Append(line[pos]);
						pos++;
					}
				}
				field = sb.ToString();
			}
			else
			{
				// Unquoted field
				int start = pos;
				while (pos < line.Length && line[pos] != ',')
				{
					pos++;
				}
				field = line[start..pos];
			}

			fields.Add(item: field);

			if (pos < line.Length && line[pos] == ',')
			{
				pos++; // skip comma
			}
			else
			{
				break;
			}
		}
		return [.. fields];
	}

	// -------------------------------------------------------------------------
	// INI
	// -------------------------------------------------------------------------

	/// <summary>Imports user-scoped application settings from an INI file.</summary>
	/// <param name="filePath">The full path to the INI file to import.</param>
	/// <remarks>Settings are expected in sections named <c>[User]</c> and <c>[Application]</c>.
	/// Comment lines starting with <c>;</c> may carry the type hint in the form <c>; Type: TypeName</c>.</remarks>
	public static void LoadFromIni(string filePath)
	{
		logger.Info(message: $"Importing settings from INI file '{filePath}'.");
		try
		{
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			List<SettingEntry> entries = [];
			string currentScope = string.Empty;
			string currentTypeName = string.Empty;

			foreach (string rawLine in lines)
			{
				string line = rawLine.Trim();
				if (string.IsNullOrEmpty(value: line))
				{
					continue;
				}

				if (line.StartsWith(value: '[') && line.EndsWith(value: ']'))
				{
					currentScope = line[1..^1].Trim();
					continue;
				}

				if (line.StartsWith(value: ';'))
				{
					// Possible type hint: "; Type: <TypeName>"
					string comment = line[1..].Trim();
					if (comment.StartsWith(value: "Type:", comparisonType: StringComparison.OrdinalIgnoreCase))
					{
						currentTypeName = comment[5..].Trim();
					}
					continue;
				}

				int eqIndex = line.IndexOf(value: '=');
				if (eqIndex <= 0)
				{
					continue;
				}

				string name = line[..eqIndex].Trim();
				string value = line[(eqIndex + 1)..];

				entries.Add(new SettingEntry
				{
					Name = name,
					TypeName = currentTypeName,
					Scope = currentScope,
					Value = value,
				});

				currentTypeName = string.Empty;
			}

			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from INI '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "INI", filePath: filePath);
		}
	}

	// -------------------------------------------------------------------------
	// XML
	// -------------------------------------------------------------------------

	/// <summary>Imports user-scoped application settings from an XML file.</summary>
	/// <param name="filePath">The full path to the XML file to import.</param>
	/// <remarks>The expected XML structure is the one produced by <c>SettingsExporter.SaveAsXml</c>:
	/// a <c>&lt;Settings&gt;</c> root element containing <c>&lt;Setting name="…" type="…" scope="…"&gt;value&lt;/Setting&gt;</c> children.</remarks>
	public static void LoadFromXml(string filePath)
	{
		logger.Info(message: $"Importing settings from XML file '{filePath}'.");
		try
		{
			List<SettingEntry> entries = [];
			XmlDocument doc = new();
			doc.Load(filename: filePath);
			XmlNodeList? settingNodes = doc.SelectNodes(xpath: "/Settings/Setting");
			if (settingNodes is not null)
			{
				foreach (XmlNode node in settingNodes)
				{
					if (node.Attributes is null)
					{
						continue;
					}
					string name = node.Attributes["name"]?.Value ?? string.Empty;
					string typeName = node.Attributes["type"]?.Value ?? string.Empty;
					string scope = node.Attributes["scope"]?.Value ?? string.Empty;
					string value = node.InnerText;

					if (string.IsNullOrEmpty(value: name))
					{
						continue;
					}

					entries.Add(new SettingEntry
					{
						Name = name,
						TypeName = typeName,
						Scope = scope,
						Value = value,
					});
				}
			}

			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from XML '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "XML", filePath: filePath);
		}
	}

	// -------------------------------------------------------------------------
	// JSON
	// -------------------------------------------------------------------------

	/// <summary>Imports user-scoped application settings from a JSON file.</summary>
	/// <param name="filePath">The full path to the JSON file to import.</param>
	/// <remarks>The expected format is the one produced by <c>SettingsExporter.SaveAsJson</c>:
	/// a JSON array of objects with <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> fields.</remarks>
	public static void LoadFromJson(string filePath)
	{
		logger.Info(message: $"Importing settings from JSON file '{filePath}'.");
		try
		{
			string json = File.ReadAllText(path: filePath, encoding: Encoding.UTF8);
			List<SettingEntry> entries = ParseJsonArray(json: json);
			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error importing settings from JSON '{filePath}': {ex.Message}");
			ExportFeedbackHelper.ShowImportError(ex: ex, format: "JSON", filePath: filePath);
		}
	}

	/// <summary>Parses a JSON array of setting objects produced by <c>SettingsExporter.SaveAsJson</c>.</summary>
	/// <param name="json">The raw JSON string.</param>
	/// <returns>A list of <see cref="SettingEntry"/> instances.</returns>
	/// <remarks>This is a simple hand-rolled parser that handles the specific format produced by the exporter.
	/// It does not attempt to handle arbitrary JSON.</remarks>
	private static List<SettingEntry> ParseJsonArray(string json)
	{
		List<SettingEntry> entries = [];
		// Extract each object between { and }
		int pos = 0;
		while (pos < json.Length)
		{
			int start = json.IndexOf(value: '{', startIndex: pos);
			if (start < 0)
			{
				break;
			}
			int end = json.IndexOf(value: '}', startIndex: start);
			if (end < 0)
			{
				break;
			}

			string obj = json[(start + 1)..end];
			Dictionary<string, string> fields = ParseJsonObject(obj: obj);

			_ = fields.TryGetValue(key: "name", value: out string? name);
			_ = fields.TryGetValue(key: "type", value: out string? typeName);
			_ = fields.TryGetValue(key: "scope", value: out string? scope);
			_ = fields.TryGetValue(key: "value", value: out string? value);

			if (!string.IsNullOrEmpty(value: name))
			{
				entries.Add(new SettingEntry
				{
					Name = name ?? string.Empty,
					TypeName = typeName ?? string.Empty,
					Scope = scope ?? string.Empty,
					Value = value ?? string.Empty,
				});
			}

			pos = end + 1;
		}
		return entries;
	}

	/// <summary>Parses a flat JSON object body (the content between braces) into a string dictionary.</summary>
	/// <param name="obj">The raw text between <c>{</c> and <c>}</c>.</param>
	/// <returns>A dictionary of key/value string pairs.</returns>
	private static Dictionary<string, string> ParseJsonObject(string obj)
	{
		Dictionary<string, string> result = new(comparer: StringComparer.OrdinalIgnoreCase);
		int pos = 0;
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

			// Find colon
			int colon = obj.IndexOf(value: ':', startIndex: keyEnd + 1);
			if (colon < 0)
			{
				break;
			}
			pos = colon + 1;

			// Skip whitespace
			while (pos < obj.Length && char.IsWhiteSpace(c: obj[pos]))
			{
				pos++;
			}

			if (pos >= obj.Length)
			{
				break;
			}

			string fieldValue;
			if (obj[pos] == '"')
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
				while (valEnd < obj.Length && obj[valEnd] != ',' && obj[valEnd] != '}')
				{
					valEnd++;
				}
				fieldValue = obj[pos..valEnd].Trim();
				pos = valEnd;
			}

			result[key] = fieldValue;

			// Skip comma
			while (pos < obj.Length && (obj[pos] == ',' || char.IsWhiteSpace(c: obj[pos])))
			{
				pos++;
			}
		}
		return result;
	}

	/// <summary>Finds the index of the closing double-quote character, correctly skipping escaped quotes.</summary>
	/// <param name="text">The text to search in.</param>
	/// <param name="start">The position immediately after the opening quote.</param>
	/// <returns>The index of the closing quote, or <c>-1</c> if not found.</returns>
	private static int FindClosingQuote(string text, int start)
	{
		int i = start;
		while (i < text.Length)
		{
			if (text[i] == '\\')
			{
				i += 2; // skip escaped character
				continue;
			}
			if (text[i] == '"')
			{
				return i;
			}
			i++;
		}
		return -1;
	}

	/// <summary>Unescapes a JSON string value (removes backslash escapes).</summary>
	/// <param name="value">The raw JSON string content (without surrounding quotes).</param>
	/// <returns>The unescaped string.</returns>
	private static string JsonUnescape(string value)
	{
		if (!value.Contains(value: '\\'))
		{
			return value;
		}

		StringBuilder sb = new(capacity: value.Length);
		int i = 0;
		while (i < value.Length)
		{
			if (value[i] == '\\' && i + 1 < value.Length)
			{
				_ = value[i + 1] switch
				{
					'"' => sb.Append('"'),
					'\\' => sb.Append('\\'),
					'n' => sb.Append('\n'),
					'r' => sb.Append('\r'),
					't' => sb.Append('\t'),
					'b' => sb.Append('\b'),
					'f' => sb.Append('\f'),
					'u' when i + 5 < value.Length => sb.Append(value: (char)Convert.ToInt32(value: value.Substring(startIndex: i + 2, length: 4), fromBase: 16)),
					_ => sb.Append(value[i + 1]),
				};
				i += value[i + 1] == 'u' ? 6 : 2;
			}
			else
			{
				_ = sb.Append(value[i]);
				i++;
			}
		}
		return sb.ToString();
	}

	// -------------------------------------------------------------------------
	// YAML
	// -------------------------------------------------------------------------

	/// <summary>Imports user-scoped application settings from a YAML file.</summary>
	/// <param name="filePath">The full path to the YAML file to import.</param>
	/// <remarks>The expected format is the one produced by <c>SettingsExporter.SaveAsYaml</c>:
	/// a YAML sequence where each item is a mapping with <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> keys.</remarks>
	public static void LoadFromYaml(string filePath)
	{
		logger.Info(message: $"Importing settings from YAML file '{filePath}'.");
		try
		{
			string[] lines = File.ReadAllLines(path: filePath, encoding: Encoding.UTF8);
			List<SettingEntry> entries = [];

			string name = string.Empty;
			string typeName = string.Empty;
			string scope = string.Empty;
			string value = string.Empty;
			bool inEntry = false;

			foreach (string rawLine in lines)
			{
				string line = rawLine.TrimEnd();

				if (string.IsNullOrWhiteSpace(value: line) || line == "---")
				{
					continue;
				}

				if (line.StartsWith(value: "- name:"))
				{
					// Save previous entry if complete
					if (inEntry && !string.IsNullOrEmpty(value: name))
					{
						entries.Add(new SettingEntry { Name = name, TypeName = typeName, Scope = scope, Value = value });
					}
					// Start new entry
					name = YamlUnquote(ParseYamlValue(line: line, key: "name"));
					typeName = string.Empty;
					scope = string.Empty;
					value = string.Empty;
					inEntry = true;
				}
				else if (inEntry && line.StartsWith(value: "  type:"))
				{
					typeName = YamlUnquote(ParseYamlValue(line: line, key: "type"));
				}
				else if (inEntry && line.StartsWith(value: "  scope:"))
				{
					scope = YamlUnquote(ParseYamlValue(line: line, key: "scope"));
				}
				else if (inEntry && line.StartsWith(value: "  value:"))
				{
					value = YamlUnquote(ParseYamlValue(line: line, key: "value"));
				}
			}

			// Flush last entry
			if (inEntry && !string.IsNullOrEmpty(value: name))
			{
				entries.Add(new SettingEntry { Name = name, TypeName = typeName, Scope = scope, Value = value });
			}

			int applied = ApplySettings(entries: entries);
			ExportFeedbackHelper.ShowImportSuccess(count: applied);
		}
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
		string search = $"{key}:";
		int idx = line.IndexOf(value: search, comparisonType: StringComparison.Ordinal);
		if (idx < 0)
		{
			return string.Empty;
		}
		return line[(idx + search.Length)..].Trim();
	}

	/// <summary>Removes YAML single-quote wrapping and unescapes doubled single quotes.</summary>
	/// <param name="scalar">A YAML scalar that may or may not be single-quoted.</param>
	/// <returns>The plain string value.</returns>
	private static string YamlUnquote(string scalar)
	{
		if (scalar.Length >= 2 && scalar[0] == '\'' && scalar[^1] == '\'')
		{
			// Single-quoted scalar — unescape '' → '
			return scalar[1..^1].Replace(oldValue: "''", newValue: "'");
		}
		return scalar;
	}
}
