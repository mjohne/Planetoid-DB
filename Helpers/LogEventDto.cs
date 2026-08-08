// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
