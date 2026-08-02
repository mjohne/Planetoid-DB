// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Properties;

using System.Text.Json;

namespace Planetoid_DB.Helpers;

/// <summary>Thread-safe in-memory store for NLog <see cref="LogEventInfo"/> instances captured during the application session.</summary>
/// <remarks> <see cref="LogEventTarget"/> writes every received <see cref="LogEventInfo"/> into this store. The <c>LogViewerForm</c> reads from it to populate the list view. All public members are thread-safe.</remarks>
public static class LogEventStore
{
	/// <summary>Lock used to synchronise access to <see cref="_events"/>.</summary>
	/// <remarks>All public methods that access <see cref="_events"/> must acquire the lock in either read or write mode.</remarks>
	private static readonly ReaderWriterLockSlim _lock = new(recursionPolicy: LockRecursionPolicy.NoRecursion);

	/// <summary>Internal list that holds all captured log events.</summary>
	/// <remarks>Access to this list must be synchronised via <see cref="_lock"/>.</remarks>
	private static readonly List<LogEventInfo> _events = [];

	/// <summary>Reusable JSON serializer options for efficient serialization.</summary>
	/// <remarks>Creating a static instance of JsonSerializerOptions with WriteIndented set to true allows for consistent formatting of JSON output across all methods that serialize to JSON, while avoiding the overhead of creating new options instances for each serialization operation.</remarks>
	private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

	/// <summary>Adds a log event to the store.</summary>
	/// <param name="logEvent">The <see cref="LogEventInfo"/> to add. Must not be <see langword="null"/>.</param>
	/// <remarks>This method is called by <see cref="LogEventTarget"/> on every log write and is safe to call from multiple threads.</remarks>
	public static void Add(LogEventInfo logEvent)
	{
		// Validate argument
		ArgumentNullException.ThrowIfNull(argument: logEvent);
		// Acquire write lock to ensure exclusive access to the list
		_lock.EnterWriteLock();
		try
		{
			// Append the log event to the end of the list
			_events.Add(item: logEvent);
		}
		// Release the write lock in a finally block to ensure it is always released, even if an exception occurs
		finally
		{
			// Release the write lock to allow other threads to access the list
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Returns a snapshot of all currently stored log events.</summary>
	/// <returns>A new <see cref="List{T}"/> containing a copy of all stored <see cref="LogEventInfo"/> instances in insertion order.</returns>
	/// <remarks>The returned list is an independent copy; subsequent changes to the store do not affect it.</remarks>
	public static List<LogEventInfo> GetSnapshot()
	{
		// Acquire read lock to ensure consistent access to the list
		_lock.EnterReadLock();
		// Create a new list containing a copy of the stored log events
		try
		{
			// Return a new list containing the current log events in insertion order
			return [.. _events];
		}
		// Release the read lock in a finally block to ensure it is always released, even if an exception occurs
		finally
		{
			// Release the read lock to allow other threads to access the list
			_lock.ExitReadLock();
		}
	}

	/// <summary>Removes the log events at the specified indices from the store.</summary>
	/// <param name="indices">A collection of zero-based indices (referring to the current snapshot order) of items to remove.</param>
	/// <remarks>Indices that are out of range are silently ignored. The removal is performed in descending order to preserve index validity during the operation.</remarks>
	public static void RemoveAt(IEnumerable<int> indices)
	{
		// Validate argument
		ArgumentNullException.ThrowIfNull(argument: indices);
		// Acquire write lock to ensure exclusive access to the list during removal
		_lock.EnterWriteLock();
		// Use a try/finally block to ensure the write lock is always released
		try
		{
			// Remove in descending order to avoid index shifting
			foreach (int index in indices.Distinct().OrderByDescending(keySelector: i => i))
			{
				// Only remove if the index is within the valid range of the list
				if (index >= 0 && index < _events.Count)
				{
					_events.RemoveAt(index: index);
				}
			}
		}
		// Release the write lock in the finally block to ensure it is always released, even if an exception occurs
		finally
		{
			// Release the write lock to allow other threads to access the list
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Removes all log events from the store.</summary>
	/// <remarks>After this call <see cref="GetSnapshot"/> returns an empty list.</remarks>
	public static void Clear()
	{
		// Acquire write lock to ensure exclusive access to the list during clearing
		_lock.EnterWriteLock();
		// Use a try/finally block to ensure the write lock is always released
		try
		{
			// Clear the internal list of log events
			_events.Clear();
		}
		// Release the write lock in the finally block to ensure it is always released, even if an exception occurs
		finally
		{
			// Release the write lock to allow other threads to access the list
			_lock.ExitWriteLock();
		}
	}

	/// <summary>Gets the number of log events currently held in the store.</summary>
	/// <value>The total count of stored log events.</value>
	/// <remarks>This property is thread-safe and reflects the current state of the store at the time of access.</remarks>
	public static int Count
	{
		// Acquire read lock to ensure consistent access to the list
		get
		{
			// Use a try/finally block to ensure the read lock is always released
			_lock.EnterReadLock();
			try
			{
				// Return the count of log events currently stored
				return _events.Count;
			}
			// Release the read lock in the finally block to ensure it is always released, even if an exception occurs
			finally
			{
				// Release the read lock to allow other threads to access the list
				_lock.ExitReadLock();
			}
		}
	}

	/// <summary>Gets the full path of the JSON file used to persist log events between sessions.</summary>
	/// <value>Path inside <c>%AppData%\Planetoid-DB\log-events.json</c>.</value>
	/// <remarks>The directory is created automatically when <see cref="SaveAsync"/> is called for the first time.</remarks>
	public static string StoragePath { get; } = Path.Combine(
		path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ApplicationData),
		path2: Settings.Default.userAppDirectory,
		path3: Settings.Default.userLogEventsFilename);

	/// <summary>Serializes all currently stored log events to <see cref="StoragePath"/> as a JSON array.</summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous write operation.</returns>
	/// <remarks> The directory is created automatically if it does not yet exist. Any I/O or serialization exception is silently swallowed so that a failed save never prevents the application from shutting down.</remarks>
	public static async Task SaveAsync()
	{
		// Create a snapshot of the current log events to avoid holding the lock during I/O operations
		List<LogEventInfo> snapshot = GetSnapshot();
		// Convert the snapshot to a list of DTOs suitable for JSON serialization
		List<LogEventDto> dtos = snapshot.ConvertAll(converter: e => new LogEventDto(
			TimeStamp: e.TimeStamp,
			Level: e.Level?.Name ?? string.Empty,
			ExceptionTypeName: e.Exception?.GetType().Name ?? string.Empty,
			Message: e.FormattedMessage ?? string.Empty));
		// Use a try/catch block to handle any exceptions during file operations and serialization
		try
		{
			// Ensure the directory exists before attempting to write the file
			string? directory = Path.GetDirectoryName(path: StoragePath);
			// Create the directory if it does not exist
			if (!string.IsNullOrEmpty(value: directory))
			{
				// Create the directory and ignore the returned DirectoryInfo object
				_ = Directory.CreateDirectory(path: directory);
			}
			// Open a file stream for asynchronous writing to the specified storage path
			await using FileStream fs = new(
				path: StoragePath,
				mode: FileMode.Create,
				access: FileAccess.Write,
				share: FileShare.None,
				bufferSize: 4096,
				useAsync: true);
			// Serialize the list of DTOs to JSON and write it to the file stream
			await JsonSerializer.SerializeAsync(
				utf8Json: fs,
				value: dtos,
				options: jsonSerializerOptions);
		}
		catch (Exception)
		{
			// A failed save must never crash or delay shutdown.
		}
	}

	/// <summary>Deserializes log events persisted by a previous session from <see cref="StoragePath"/> and prepends them to the current store.</summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous read operation.</returns>
	/// <remarks>Events from the file are inserted at the <em>beginning</em> of the store so that they appear before the events of the current session in the <c>LogViewerForm</c>. The file is not deleted after loading; it is overwritten on the next <see cref="SaveAsync"/> call.Any I/O or deserialization exception is silently swallowed.</remarks>
	public static async Task LoadAsync()
	{
		// If the file does not exist, there is nothing to load, so we can return early.
		if (!File.Exists(path: StoragePath))
		{
			return;
		}
		// Use a try/catch block to handle any exceptions during file operations and deserialization
		try
		{
			// Open a file stream for asynchronous reading from the specified storage path
			await using FileStream fs = new(
				path: StoragePath,
				mode: FileMode.Open,
				access: FileAccess.Read,
				share: FileShare.Read,
				bufferSize: 4096,
				useAsync: true);
			// Deserialize the JSON array from the file stream into a list of DTOs
			List<LogEventDto>? dtos = await JsonSerializer.DeserializeAsync<List<LogEventDto>>(utf8Json: fs);
			// If the deserialized list is null or empty, there are no events to restore, so we can return early.
			if (dtos is null || dtos.Count == 0)
			{
				return;
			}
			// Convert the list of DTOs back into LogEventInfo instances
			List<LogEventInfo> restored = dtos.ConvertAll(converter: dto =>
			{
				// Convert the level name string back into a LogLevel instance
				LogLevel level = LogLevel.FromString(levelName: dto.Level);
				// Create a new LogEventInfo instance with the restored properties
				LogEventInfo evt = LogEventInfo.Create(level, "Restored", dto.Message);
				// Set the timestamp of the restored event to match the original event
				evt.TimeStamp = dto.TimeStamp;
				// If the exception type name is not empty, add it to the event properties for reference
				if (!string.IsNullOrEmpty(value: dto.ExceptionTypeName))
				{
					evt.Properties["ExceptionTypeName"] = dto.ExceptionTypeName;
				}
				// Return the restored LogEventInfo instance
				return evt;
			});
			// Acquire write lock to ensure exclusive access to the list while prepending restored events
			_lock.EnterWriteLock();
			// Use a try/finally block to ensure the write lock is always released
			try
			{
				// Prepend so previous-session events appear first.
				_events.InsertRange(index: 0, collection: restored);
			}
			// Release the write lock in the finally block to ensure it is always released, even if an exception occurs
			finally
			{
				// Release the write lock to allow other threads to access the list
				_lock.ExitWriteLock();
			}
		}
		catch (Exception)
		{
			// A corrupt or unreadable file must never prevent the application from starting.
		}
	}
}
