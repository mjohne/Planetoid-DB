// Exports application settings (from Settings.settings) to various file formats.

using NLog;

using Planetoid_DB.Properties;

using System.Configuration;
using System.Text;
using System.Xml;

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods to export all application settings (user-scoped and application-scoped)
/// from <see cref="Settings"/> to CSV, INI, XML, JSON, and YAML files.</summary>
/// <remarks>Each setting is exported with its name, data type, scope (User/Application), and current value.
/// Settings are discovered at run time via the <see cref="SettingsBase.Properties"/> collection so that
/// any future additions to <c>Settings.settings</c> are picked up automatically.</remarks>
public static class SettingsExporter
{
	/// <summary>NLog logger for the class.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	// -------------------------------------------------------------------------
	// Internal data model
	// -------------------------------------------------------------------------

	/// <summary>Represents a single setting entry that will be written to an export file.</summary>
	private sealed class SettingEntry
	{
		/// <summary>Gets the name of the setting.</summary>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the CLR type name of the setting value.</summary>
		public string TypeName { get; init; } = string.Empty;

		/// <summary>Gets the scope of the setting ("User" or "Application").</summary>
		public string Scope { get; init; } = string.Empty;

		/// <summary>Gets the current value of the setting as a string.</summary>
		public string Value { get; init; } = string.Empty;
	}

	// -------------------------------------------------------------------------
	// Settings collection helper
	// -------------------------------------------------------------------------

	/// <summary>Reads all settings from <see cref="Settings.Default"/> and returns them as a list of <see cref="SettingEntry"/> objects.</summary>
	/// <returns>A list of <see cref="SettingEntry"/> instances, one per setting property.</returns>
	private static List<SettingEntry> CollectSettings()
	{
		List<SettingEntry> entries = [];
		Settings settings = Settings.Default;

		foreach (SettingsProperty prop in settings.Properties)
		{
			string scope = prop.Attributes[typeof(UserScopedSettingAttribute)] is UserScopedSettingAttribute
				? "User"
				: "Application";

			string value;
			try
			{
				value = settings[prop.Name]?.ToString() ?? string.Empty;
			}
			catch (Exception ex)
			{
				logger.Warn(exception: ex, message: $"Could not read value for setting '{prop.Name}'.");
				value = string.Empty;
			}

			entries.Add(new SettingEntry
			{
				Name = prop.Name,
				TypeName = prop.PropertyType.Name,
				Scope = scope,
				Value = value,
			});
		}

		return [.. entries.OrderBy(e => e.Scope).ThenBy(e => e.Name)];
	}

	// -------------------------------------------------------------------------
	// CSV
	// -------------------------------------------------------------------------

	/// <summary>Exports all application settings to a CSV file.</summary>
	/// <param name="filePath">The full path of the target CSV file.</param>
	/// <remarks>The CSV file will have the columns Name, Type, Scope, and Value.
	/// All field values are RFC 4180 quoted.</remarks>
	public static void SaveAsCsv(string filePath)
	{
		logger.Info(message: $"Exporting settings as CSV to '{filePath}'.");
		try
		{
			List<SettingEntry> entries = CollectSettings();
			StringBuilder sb = new();
			_ = sb.AppendLine(value: "\"Name\",\"Type\",\"Scope\",\"Value\"");
			foreach (SettingEntry e in entries)
			{
				_ = sb.AppendLine(value: $"{ExportEscapeHelper.EscapeCsvField(e.Name)},{ExportEscapeHelper.EscapeCsvField(e.TypeName)},{ExportEscapeHelper.EscapeCsvField(e.Scope)},{ExportEscapeHelper.EscapeCsvField(e.Value)}");
			}
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			ExportFeedbackHelper.ShowSuccess();
		}
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "CSV", filePath: filePath);
		}
	}

	// -------------------------------------------------------------------------
	// INI
	// -------------------------------------------------------------------------

	/// <summary>Exports all application settings to an INI file.</summary>
	/// <param name="filePath">The full path of the target INI file.</param>
	/// <remarks>Settings are grouped into [User] and [Application] sections.
	/// Each line has the form <c>Name=Value</c>. A comment line above each entry
	/// records the CLR type of the value.</remarks>
	public static void SaveAsIni(string filePath)
	{
		logger.Info(message: $"Exporting settings as INI to '{filePath}'.");
		try
		{
			List<SettingEntry> entries = CollectSettings();
			StringBuilder sb = new();
			string? currentScope = null;
			foreach (SettingEntry e in entries)
			{
				if (e.Scope != currentScope)
				{
					if (currentScope != null)
					{
						_ = sb.AppendLine();
					}
					_ = sb.AppendLine(value: $"[{e.Scope}]");
					currentScope = e.Scope;
				}
				_ = sb.AppendLine(value: $"; Type: {e.TypeName}");
				_ = sb.AppendLine(value: $"{e.Name}={e.Value}");
			}
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			ExportFeedbackHelper.ShowSuccess();
		}
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "INI", filePath: filePath);
		}
	}

	// -------------------------------------------------------------------------
	// XML
	// -------------------------------------------------------------------------

	/// <summary>Exports all application settings to an XML file.</summary>
	/// <param name="filePath">The full path of the target XML file.</param>
	/// <remarks>The root element is <c>&lt;Settings&gt;</c>. Each child element is
	/// <c>&lt;Setting name="…" type="…" scope="…"&gt;value&lt;/Setting&gt;</c>.</remarks>
	public static void SaveAsXml(string filePath)
	{
		logger.Info(message: $"Exporting settings as XML to '{filePath}'.");
		try
		{
			List<SettingEntry> entries = CollectSettings();
			XmlWriterSettings xmlSettings = new()
			{
				Indent = true,
				IndentChars = "  ",
				Encoding = Encoding.UTF8,
			};
			using XmlWriter writer = XmlWriter.Create(outputFileName: filePath, settings: xmlSettings);
			writer.WriteStartDocument();
			writer.WriteStartElement(localName: "Settings");
			foreach (SettingEntry e in entries)
			{
				writer.WriteStartElement(localName: "Setting");
				writer.WriteAttributeString(localName: "name", value: e.Name);
				writer.WriteAttributeString(localName: "type", value: e.TypeName);
				writer.WriteAttributeString(localName: "scope", value: e.Scope);
				writer.WriteString(text: e.Value);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndDocument();
			ExportFeedbackHelper.ShowSuccess();
		}
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "XML", filePath: filePath);
		}
	}

	// -------------------------------------------------------------------------
	// JSON
	// -------------------------------------------------------------------------

	/// <summary>Exports all application settings to a JSON file.</summary>
	/// <param name="filePath">The full path of the target JSON file.</param>
	/// <remarks>The file contains a JSON array of objects, each with
	/// <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c> fields.</remarks>
	public static void SaveAsJson(string filePath)
	{
		logger.Info(message: $"Exporting settings as JSON to '{filePath}'.");
		try
		{
			List<SettingEntry> entries = CollectSettings();
			StringBuilder sb = new();
			_ = sb.AppendLine(value: "[");
			for (int i = 0; i < entries.Count; i++)
			{
				SettingEntry e = entries[i];
				string comma = i < entries.Count - 1 ? "," : string.Empty;
				_ = sb.AppendLine(value: "  {");
				_ = sb.AppendLine(value: $"    \"name\": {JsonEscape(e.Name)},");
				_ = sb.AppendLine(value: $"    \"type\": {JsonEscape(e.TypeName)},");
				_ = sb.AppendLine(value: $"    \"scope\": {JsonEscape(e.Scope)},");
				_ = sb.AppendLine(value: $"    \"value\": {JsonEscape(e.Value)}");
				_ = sb.AppendLine(value: $"  }}{comma}");
			}
			_ = sb.AppendLine(value: "]");
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			ExportFeedbackHelper.ShowSuccess();
		}
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "JSON", filePath: filePath);
		}
	}

	/// <summary>Returns a JSON-encoded string literal (including surrounding double quotes).</summary>
	/// <param name="value">The raw string value to encode.</param>
	/// <returns>A JSON string literal.</returns>
	private static string JsonEscape(string value)
	{
		StringBuilder sb = new(capacity: value.Length + 4);
		_ = sb.Append('"');
		foreach (char ch in value)
		{
			_ = ch switch
			{
				'"' => sb.Append("\\\""),
				'\\' => sb.Append("\\\\"),
				'\n' => sb.Append("\\n"),
				'\r' => sb.Append("\\r"),
				'\t' => sb.Append("\\t"),
				'\b' => sb.Append("\\b"),
				'\f' => sb.Append("\\f"),
				_ => ch < ' ' ? sb.Append($"\\u{(int)ch:x4}") : sb.Append(ch),
			};
		}
		_ = sb.Append('"');
		return sb.ToString();
	}

	// -------------------------------------------------------------------------
	// YAML
	// -------------------------------------------------------------------------

	/// <summary>Exports all application settings to a YAML file.</summary>
	/// <param name="filePath">The full path of the target YAML file.</param>
	/// <remarks>The file contains a YAML sequence. Each sequence item is a mapping
	/// with the keys <c>name</c>, <c>type</c>, <c>scope</c>, and <c>value</c>.
	/// String values that require quoting are single-quoted with internal single
	/// quotes doubled.</remarks>
	public static void SaveAsYaml(string filePath)
	{
		logger.Info(message: $"Exporting settings as YAML to '{filePath}'.");
		try
		{
			List<SettingEntry> entries = CollectSettings();
			StringBuilder sb = new();
			_ = sb.AppendLine(value: "---");
			foreach (SettingEntry e in entries)
			{
				_ = sb.AppendLine(value: $"- name: {YamlScalar(e.Name)}");
				_ = sb.AppendLine(value: $"  type: {YamlScalar(e.TypeName)}");
				_ = sb.AppendLine(value: $"  scope: {YamlScalar(e.Scope)}");
				_ = sb.AppendLine(value: $"  value: {YamlScalar(e.Value)}");
			}
			File.WriteAllText(path: filePath, contents: sb.ToString(), encoding: Encoding.UTF8);
			ExportFeedbackHelper.ShowSuccess();
		}
		catch (Exception ex)
		{
			ExportFeedbackHelper.ShowError(ex: ex, format: "YAML", filePath: filePath);
		}
	}

	/// <summary>Returns a YAML scalar representation of <paramref name="value"/>.</summary>
	/// <param name="value">The raw string value.</param>
	/// <returns>A plain scalar when the value is safe, otherwise a single-quoted scalar.</returns>
	private static string YamlScalar(string value)
	{
		if (string.IsNullOrEmpty(value: value))
		{
			return "''";
		}

		// Use single-quoted style for any value that contains characters that could
		// be misinterpreted by a YAML parser (colon, hash, special indicators, etc.).
		bool needsQuoting = value.Any(c => c is ':' or '#' or '\'' or '"' or '\\' or '\n' or '\r' or '\t' or '&' or '*' or '!' or '|' or '>' or '{' or '}' or '[' or ']' or ',' or '?');
		if (!needsQuoting && (value[0] is '-' or '.' or '@' or '`'))
		{
			needsQuoting = true;
		}

		if (!needsQuoting)
		{
			return value;
		}

		// Single-quoted YAML: internal single quotes are doubled.
		return $"'{value.Replace("'", "''")}'";
	}
}
