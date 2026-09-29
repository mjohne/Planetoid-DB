/*
 * File:        RecordsForm.RecordProgressUpdate.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents the form that scans all orbital elements for maximum or minimum record values.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.ComponentModel;
using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents the form that scans all orbital elements for maximum or minimum record values.</summary>
/// <remarks>This form displays the record holder (asteroid with the highest or lowest value) for each orbital element in the database. The user can choose between maximum and minimum records, start and cancel the scan at any time.</remarks>
internal partial class RecordsForm
{
	/// <summary>Holds a progress update for a single orbital element record, used to marshal label updates from the background thread to the UI thread via <see cref="BackgroundWorker.ReportProgress(int, object)"/>.</summary>
	/// <param name="ElementIndex">Zero-based index of the orbital element.</param>
	/// <param name="Value">The new record value (as double for comparison).</param>
	/// <param name="StringValue">The original string value from the database to display in the UI.</param>
	/// <param name="Designation">The readable designation of the record-holder asteroid.</param>
	/// <remarks>This struct is used to pass progress updates from the background worker to the UI thread.</remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct RecordProgressUpdate(int ElementIndex, double Value, string StringValue, string Designation)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}