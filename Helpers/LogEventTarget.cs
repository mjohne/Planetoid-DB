/*
 * File:        LogEventTarget.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Custom NLog target that forwards every received LogEventInfo to LogEventStore for later retrieval by the log viewer UI.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;
using NLog.Targets;

namespace Planetoid_DB;

/// <summary>Custom NLog target that forwards every received <see cref="LogEventInfo"/> to <see cref="LogEventStore"/> for later retrieval by the log viewer UI.</summary>
/// <remarks>Register an instance of this target with the NLog configuration before calling <see cref="LogManager.ReconfigExistingLoggers()"/>. The target is thread-safe because <see cref="LogEventStore.Add"/> is thread-safe. </remarks>
[Target(name: "LogEventStore")]
public sealed class LogEventTarget : Target
{
	/// <summary>Initializes a new instance of the <see cref="LogEventTarget"/> class with a given target name.</summary>
	/// <param name="name">The NLog target name used to identify this target in the configuration.</param>
	/// <remarks>Passing a <paramref name="name"/> allows multiple targets to coexist while remaining individually addressable.</remarks>
	public LogEventTarget(string name)
	{
		// Set the target name for NLog configuration purposes
		Name = name;
	}

	/// <summary>Receives a single log event from NLog and stores it in <see cref="LogEventStore"/>.</summary>
	/// <param name="logEvent">The log event to store. Must not be <see langword="null"/>.</param>
	/// <remarks>This method is called on the thread that originally issued the log statement; it is safe because <see cref="LogEventStore.Add"/> uses its own lock.</remarks>
	protected override void Write(LogEventInfo logEvent)
	{
		// Validate that the log event is not null
		ArgumentNullException.ThrowIfNull(argument: logEvent);
		// Forward the log event to the LogEventStore for storage
		LogEventStore.Add(logEvent: logEvent);
	}
}
