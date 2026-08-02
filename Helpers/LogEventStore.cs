// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

namespace Planetoid_DB.Helpers;

/// <summary>Thread-safe in-memory store for NLog <see cref="LogEventInfo"/> instances captured during the application session.</summary>
/// <remarks>
/// <see cref="LogEventTarget"/> writes every received <see cref="LogEventInfo"/> into this store.
/// The <c>LogViewerForm</c> reads from it to populate the list view.
/// All public members are thread-safe.
/// </remarks>
public static class LogEventStore
{
	/// <summary>Lock used to synchronise access to <see cref="_events"/>.</summary>
	private static readonly ReaderWriterLockSlim _lock = new(recursionPolicy: LockRecursionPolicy.NoRecursion);

	/// <summary>Internal list that holds all captured log events.</summary>
	private static readonly List<LogEventInfo> _events = [];

	/// <summary>Adds a log event to the store.</summary>
	/// <param name="logEvent">The <see cref="LogEventInfo"/> to add. Must not be <see langword="null"/>.</param>
	/// <remarks>This method is called by <see cref="LogEventTarget"/> on every log write and is safe to call from multiple threads.</remarks>
	public static void Add(LogEventInfo logEvent)
	{
		ArgumentNullException.ThrowIfNull(argument: logEvent);
		_lock.EnterWriteLock();
		try
		{
			_events.Add(item: logEvent);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Returns a snapshot of all currently stored log events.</summary>
	/// <returns>A new <see cref="List{T}"/> containing a copy of all stored <see cref="LogEventInfo"/> instances in insertion order.</returns>
	/// <remarks>The returned list is an independent copy; subsequent changes to the store do not affect it.</remarks>
	public static List<LogEventInfo> GetSnapshot()
	{
		_lock.EnterReadLock();
		try
		{
			return [.. _events];
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	/// <summary>Removes the log events at the specified indices from the store.</summary>
	/// <param name="indices">A collection of zero-based indices (referring to the current snapshot order) of items to remove.</param>
	/// <remarks>Indices that are out of range are silently ignored. The removal is performed in descending order to preserve index validity during the operation.</remarks>
	public static void RemoveAt(IEnumerable<int> indices)
	{
		ArgumentNullException.ThrowIfNull(argument: indices);
		_lock.EnterWriteLock();
		try
		{
			// Remove in descending order to avoid index shifting
			foreach (int index in indices.OrderByDescending(keySelector: i => i))
			{
				if (index >= 0 && index < _events.Count)
				{
					_events.RemoveAt(index: index);
				}
			}
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Removes all log events from the store.</summary>
	/// <remarks>After this call <see cref="GetSnapshot"/> returns an empty list.</remarks>
	public static void Clear()
	{
		_lock.EnterWriteLock();
		try
		{
			_events.Clear();
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Gets the number of log events currently held in the store.</summary>
	/// <value>The total count of stored log events.</value>
	public static int Count
	{
		get
		{
			_lock.EnterReadLock();
			try
			{
				return _events.Count;
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}
	}
}
