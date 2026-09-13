/*
 * File:        BulkObservationsDownloadErrorEntry.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Dialog form that displays detailed bulk-download errors in a list view.
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

namespace Planetoid_DB.Forms;

/// <summary>Represents a single error entry produced during bulk observations download.</summary>
/// <param name="Timestamp">Date and time when the error occurred.</param>
/// <param name="Url">URL that was being requested or processed.</param>
/// <param name="ErrorType">Error type/category.</param>
/// <param name="ErrorDescription">Detailed error explanation.</param>
/// <returns>A string representation of the current object for debugging purposes.</returns>
/// <remarks>Instances of this record are immutable and intended for display purposes only.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}") ]
internal sealed record BulkObservationsDownloadErrorEntry(DateTime Timestamp, string Url, string ErrorType, string ErrorDescription)
{
	/// <summary>Returns a string representation of the current object for debugging purposes.</summary>
	/// <returns>A string representation of the current object.</returns>
	/// <remarks>This property is used by the debugger to display a concise representation of the object.</remarks>
	private string DebuggerDisplay => ToString();
}
