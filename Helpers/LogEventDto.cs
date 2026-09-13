/*
 * File:        LogEventDto.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: A serialization-friendly snapshot of a single NLog log event.
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

/// <summary>A serialization-friendly snapshot of a single NLog log event.</summary>
/// <param name="TimeStamp">The timestamp of the log event (as provided by NLog).</param>
/// <param name="Level">The NLog level name (e.g. <c>Info</c>, <c>Error</c>).</param>
/// <param name="ExceptionTypeName">The <see cref="Exception"/> type name, or an empty string when no exception was attached.</param>
/// <param name="Message">The fully formatted log message.</param>
/// <remarks>Instances are created from <see cref="NLog.LogEventInfo"/> objects inside <see cref="LogEventStore.SaveAsync"/> and converted back via <see cref="LogEventStore.LoadAsync"/> so that previous sessions are visible in the <c>LogViewerForm</c>.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed record LogEventDto(DateTime TimeStamp, string Level, string ExceptionTypeName, string Message)
{
	/// <summary>Returns a string representation of the current object for debugging purposes.</summary>
	/// <returns>A string representation of the current object.</returns>
	/// <remarks>This property is used by the debugger to display the state of the object in a human-readable format.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}