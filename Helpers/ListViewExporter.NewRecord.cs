/*
 * File:        ListViewExporter.NewRecord.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
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

using System.Diagnostics;

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods for exporting data from a ListView control.</summary>
/// <remarks>This class is intended to support operations related to exporting ListView data, such as formatting or serializing the contents for external use. All members are static and the class cannot be instantiated.</remarks>
internal static partial class ListViewExporter
{
	/// <summary>Represents a record containing a title and a collection of rows, where each row is a dictionary of string key-value pairs.</summary>
	/// <param name="Title">The title associated with the record. Cannot be null.</param>
	/// <param name="Rows">A list of rows, where each row is represented as a dictionary of string key-value pairs. Cannot be null.</param>
	/// <remarks>This record is used to encapsulate the data structure for exporting ListView contents, allowing for easy serialization and manipulation of the data.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	internal record NewRecord(string Title, List<Dictionary<string, string>> Rows)
	{
		/// <summary>Returns a string representation of the NewRecord instance for debugging purposes. This property is used by the DebuggerDisplay attribute to provide a concise and informative display of the record's properties in the debugger.</summary>
		/// <remarks>This property is used by the debugger to display the state of the object in a human-readable format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}