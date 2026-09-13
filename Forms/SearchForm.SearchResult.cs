/*
 * File:        SearchForm.SearchResult.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that provides advanced search functionality for planetoid records, allowing users to search, filter, and export search results in various formats.
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

namespace Planetoid_DB;

internal partial class SearchForm
{
	/// <summary>Represents the result of a search operation, containing information about the matched item.</summary>
	/// <remarks>This struct is used to store information about each search result found during the search operation. It includes the index of the record, its designation, the specific orbital element that matched the search criteria, and the value of that element. This structured format allows for easy management and display of search results in the user interface.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private struct SearchResult
	{
		/// <summary>Index of the matched record in the database.</summary>
		public string Index;

		/// <summary>Designation of the matched record.</summary>

		public string Designation;
		/// <summary>Name of the orbital element that matched the search criteria.</summary>
		public string Element;

		/// <summary>Value of the orbital element that matched the search criteria.</summary>
		public string Value;

		/// <summary>Returns a string representation of the search result for debugging purposes.</summary>
		/// <returns>A string representation of the search result.</returns>
		/// <remarks>This property is used to provide a custom string representation of the search result for debugging purposes. It formats the index, designation, element, and value into a readable string.</remarks>
		private readonly string DebuggerDisplay => ToString() ?? string.Empty;
	}
}