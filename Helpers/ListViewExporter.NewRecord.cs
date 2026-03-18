// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Provides static methods for exporting data from a ListView control.</summary>
/// <remarks>This class is intended to support operations related to exporting ListView data, such as formatting
/// or serializing the contents for external use. All members are static and the class cannot be instantiated.</remarks>
public static partial class ListViewExporter
{
	/// <summary>Represents a record containing a title and a collection of rows, where each row is a dictionary of string key-value
	/// pairs.</summary>
	/// <param name="Title">The title associated with the record. Cannot be null.</param>
	/// <param name="Rows">A list of rows, where each row is represented as a dictionary of string key-value pairs. Cannot be null.</param>
	/// <remarks>This record is used to encapsulate the data structure for exporting ListView contents, allowing for easy serialization and manipulation of the data.</remarks>
	internal record NewRecord(string Title, List<Dictionary<string, string>> Rows);
}
