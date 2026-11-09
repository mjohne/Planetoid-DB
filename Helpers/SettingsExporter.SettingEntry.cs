/*
 * File:        SettingsExporter.SettingEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
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

using Planetoid_DB.Properties;

using System.Configuration;
using System.Diagnostics;

namespace Planetoid_DB.Helpers;

/// <summary>Exports application settings to a file.</summary>
/// <remarks>Exported entries include each setting's name, CLR type, scope, and current value.</remarks>
internal static partial class SettingsExporter
{
	/// <summary>Represents a single setting entry parsed from an import file.</summary>
	/// <remarks>This class is used internally to hold the name, type, scope, and value of a setting before it is applied to <see cref="Settings.Default"/>.</remarks>
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
	private sealed class SettingEntry
	{
		/// <summary>Gets the name of the setting.</summary>
		/// <remarks>This is the key used to look up the setting in <see cref="Settings.Default"/>.</remarks>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the CLR type name of the setting value.</summary>
		/// <remarks>The type name is obtained from the <see cref="SettingsProperty.PropertyType"/> of the setting.</remarks>
		public string TypeName { get; init; } = string.Empty;

		/// <summary>Gets the scope of the setting ("User" or "Application").</summary>
		/// <remarks>Only user-scoped settings can be applied; application-scoped settings are skipped.</remarks>
		public string Scope { get; init; } = string.Empty;

		/// <summary>Gets the value of the setting as a string.</summary>
		/// <remarks>This value will be converted to the appropriate type based on <see cref="TypeName"/> when applying the setting.</remarks>
		public string Value { get; init; } = string.Empty;

		/// <summary>Returns a string representation of the setting entry for debugging purposes.</summary>
		/// <returns>A string representation of the setting entry.</returns>
		/// <remarks>This method is used to provide a human-readable representation of the setting entry for debugging.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
