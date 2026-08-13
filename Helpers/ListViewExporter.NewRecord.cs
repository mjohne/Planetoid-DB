/*
 * File:        ListViewExporter.NewRecord.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides static methods for exporting data from a ListView control.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods for exporting data from a ListView control.</summary>
/// <remarks>This class is intended to support operations related to exporting ListView data, such as formatting or serializing the contents for external use. All members are static and the class cannot be instantiated.</remarks>
public static partial class ListViewExporter
{
	/// <summary>Represents a record containing a title and a collection of rows, where each row is a dictionary of string key-value pairs.</summary>
	/// <param name="Title">The title associated with the record. Cannot be null.</param>
	/// <param name="Rows">A list of rows, where each row is represented as a dictionary of string key-value pairs. Cannot be null.</param>
	/// <remarks>This record is used to encapsulate the data structure for exporting ListView contents, allowing for easy serialization and manipulation of the data.</remarks>
	internal record NewRecord(string Title, List<Dictionary<string, string>> Rows);
}