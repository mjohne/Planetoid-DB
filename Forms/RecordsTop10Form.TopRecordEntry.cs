/*
 * File:        RecordsTop10Form.TopRecordEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Main form for managing records in the Planetoid database.
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

/// <summary>Represents the main form for managing records in the Planetoid database, specifically handling the top-ten entries of planetoids based on their numeric values.</summary>
/// <remarks>This partial class contains the definition for the TopRecordEntry record struct used to represent individual top-ten entries.</remarks>
internal partial class RecordsTop10Form
{
	/// <summary>Holds one top-ten entry containing designation, numeric value, and display value.</summary>
	/// <param name="Designation">Readable designation of the planetoid.</param>
	/// <param name="StringValue">Original string value as shown from source data.</param>
	/// <param name="NumericValue">Numeric value used for ranking and comparison.</param>
	/// <remarks>This record struct is used to represent a single entry in the top-ten list of planetoids, encapsulating the designation, string representation, and numeric value for sorting and display purposes.</remarks>
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct TopRecordEntry(string Designation, string StringValue, double NumericValue)
	{
		/// <summary>Gets a string representation of the top record entry for debugging purposes.</summary>
		/// <remarks>This property is used by the DebuggerDisplay attribute to show a concise representation of the record in the debugger.</remarks>
		private string DebuggerDisplay => ToString();
	}
}