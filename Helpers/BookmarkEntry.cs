/*
 * File:        BookmarkEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Represents a single bookmark entry for a planetoid record.
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

/// <summary>Represents a single bookmark entry for a planetoid record.</summary>
/// <remarks>A bookmark stores the date/time it was saved, the zero-based index position of the record in the database, and the readable designation (name) of the planetoid.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class BookmarkEntry
{
	/// <summary>Initializes a new instance of the <see cref="BookmarkEntry"/> class.</summary>
	public BookmarkEntry()
	{
	}
	/// <summary>Gets or sets the date and time when this bookmark was created.</summary>
	/// <remarks>The value is stored in UTC.</remarks>
	public DateTime SavedAt { get; set; }

	/// <summary>Gets or sets the zero-based index position of the planetoid record in the database.</summary>
	/// <remarks>This value is used to quickly locate the planetoid record in the database without needing to search by name or other attributes.</remarks>
	public int Position { get; set; }

	/// <summary>Gets or sets the readable designation (name) of the bookmarked planetoid.</summary>
	/// <remarks>This value is used for display purposes in the user interface.</remarks>
	public string Name { get; set; } = string.Empty;

	/// <summary>Returns a string that represents the current bookmark entry.</summary>
	/// <returns>A string that represents the current bookmark entry.</returns>
	/// <remarks>This property is used by the debugger to display a friendly string representation of the bookmark entry.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
