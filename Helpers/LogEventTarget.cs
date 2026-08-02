// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;
using NLog.Targets;

using Planetoid_DB.Helpers;

namespace Planetoid_DB;

/// <summary>Custom NLog target that forwards every received <see cref="LogEventInfo"/> to <see cref="LogEventStore"/> for later retrieval by the log viewer UI.</summary>
/// <remarks>
/// Register an instance of this target with the NLog configuration before calling
/// <see cref="LogManager.ReconfigExistingLoggers"/>. The target is thread-safe because
/// <see cref="LogEventStore.Add"/> is thread-safe.
/// </remarks>
[Target(name: "LogEventStore")]
public sealed class LogEventTarget : Target
{
	/// <summary>Initializes a new instance of the <see cref="LogEventTarget"/> class with a given target name.</summary>
	/// <param name="name">The NLog target name used to identify this target in the configuration.</param>
	/// <remarks>Passing a <paramref name="name"/> allows multiple targets to coexist while remaining individually addressable.</remarks>
	public LogEventTarget(string name)
	{
		Name = name;
	}

	/// <summary>Receives a single log event from NLog and stores it in <see cref="LogEventStore"/>.</summary>
	/// <param name="logEvent">The log event to store. Must not be <see langword="null"/>.</param>
	/// <remarks>This method is called on the thread that originally issued the log statement; it is safe because <see cref="LogEventStore.Add"/> uses its own lock.</remarks>
	protected override void Write(LogEventInfo logEvent)
	{
		ArgumentNullException.ThrowIfNull(argument: logEvent);
		LogEventStore.Add(logEvent: logEvent);
	}
}
