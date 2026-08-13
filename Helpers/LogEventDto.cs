/*
 * File:        LogEventDto.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
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

namespace Planetoid_DB.Helpers;

/// <summary>A serialization-friendly snapshot of a single NLog log event.</summary>
/// <param name="TimeStamp">The timestamp of the log event (as provided by NLog).</param>
/// <param name="Level">The NLog level name (e.g. <c>Info</c>, <c>Error</c>).</param>
/// <param name="ExceptionTypeName">The <see cref="System.Exception"/> type name, or an empty string when no exception was attached.</param>
/// <param name="Message">The fully formatted log message.</param>
/// <remarks>Instances are created from <see cref="NLog.LogEventInfo"/> objects inside <see cref="LogEventStore.SaveAsync"/> and converted back via <see cref="LogEventStore.LoadAsync"/> so that previous sessions are visible in the <c>LogViewerForm</c>.</remarks>
public sealed record LogEventDto(
	DateTime TimeStamp, // Timestamp of the log event
	string Level, // NLog level name (e.g. "Info", "Error")
	string ExceptionTypeName, // System.Exception type name, or empty string when no exception was attached
	string Message // Fully formatted log message
	);
