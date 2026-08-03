// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using System.Runtime.InteropServices;

namespace Planetoid_DB.Helpers;

/// <summary>Controls the progress bar of the program icon in the Windows taskbar.</summary>
/// <remarks>This class provides methods to interact with the Windows taskbar to display progress.</remarks>
public static class TaskbarProgress
{
	/// <summary>Provides a synchronization lock for operations related to the taskbar instance.</summary>
	/// <remarks>Use this lock to ensure thread safety when accessing or modifying shared taskbar-related resources.</remarks>
	private static readonly Lock TaskbarLock = new();

	/// <summary>Holds a cached instance of the ITaskbarList3 COM interface for interacting with the Windows taskbar.</summary>
	/// <remarks>This instance is lazily initialized and reused to minimize COM interop overhead.</remarks>
	private static ITaskbarList3? _taskbarInstance;

	/// <summary>Determines whether the current operating system supports the taskbar progress API (Windows 7 / Server 2008 R2 or later).</summary>
	/// <remarks>This property checks the OS version to ensure that taskbar progress features are available before attempting to use them.</remarks>
	public static bool IsSupported => OperatingSystem.IsWindowsVersionAtLeast(major: 6, minor: 1);

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Retrieves the cached taskbar COM instance or initializes it in a thread-safe manner.</summary>
	/// <remarks>This method ensures that the taskbar instance is created only once and reused for subsequent operations.</remarks>
	private static ITaskbarList3? GetTaskbarInstance()
	{
		// If the taskbar progress API is not supported, return null immediately.
		if (!IsSupported)
		{
			return null;
		}
		// If the cached instance is null, attempt to create it in a thread-safe manner.
		if (_taskbarInstance == null)
		{
			// Lock to ensure that only one thread can initialize the taskbar instance at a time.
			lock (TaskbarLock)
			{
				// Double-check if the instance is still null after acquiring the lock.
				if (_taskbarInstance == null)
				{
					// Attempt to create a new instance of the TaskbarInstance COM object and initialize it.
					try
					{
						// Create a new instance of the TaskbarInstance COM object and cast it to ITaskbarList3.
						ITaskbarList3? instance = new TaskbarInstance() as ITaskbarList3;
						// Call the HrInit method to initialize the taskbar instance.
						instance?.HrInit();
						// Cache the initialized instance for future use.
						_taskbarInstance = instance;
					}
					// Catch specific exceptions that may occur during COM interop and initialization.
					catch (Exception ex) when (ex is COMException or UnauthorizedAccessException or DllNotFoundException)
					{
						// If an exception occurs, log the error and set the cached instance to null to indicate failure.
						_taskbarInstance = null;
						// Log an error message indicating that initializing the taskbar instance failed.
						logger.Error(exception: ex, message: "Error initializing taskbar instance");
					}
				}
			}
		}
		// Return the cached taskbar instance, which may be null if initialization failed.
		return _taskbarInstance;
	}

	/// <summary>Sets the state of the taskbar progress bar (e.g., Normal, Paused, Error).</summary>
	/// <param name="windowHandle">The handle of the target window.</param>
	/// <param name="state">The state to set.</param>
	/// <remarks>This method updates the taskbar progress bar state for the specified window handle, allowing for visual feedback on the taskbar.</remarks>
	public static void SetState(IntPtr windowHandle, TaskbarProgressState state)
	{
		// If the window handle is invalid or the taskbar progress API is not supported, exit early.
		if (windowHandle == IntPtr.Zero || !IsSupported)
		{
			return;
		}
		// Attempt to set the taskbar progress state using the cached taskbar instance.
		try
		{
			// Call the SetProgressState method on the taskbar instance to update the progress bar state.
			GetTaskbarInstance()?.SetProgressState(hwnd: windowHandle, tbpFlags: state);
		}
		// Catch any COM exceptions that may occur during the operation, such as when the Explorer process is restarted.
		catch (COMException)
		{
			// On COM errors (e.g., Explorer restart), discard the invalid instance
			ResetInstance();
			// Log an error message indicating that setting the taskbar progress state failed.
			logger.Error(message: $"Error setting taskbar progress state for window handle: {windowHandle}");
		}
	}

	/// <summary>Sets the current value of the taskbar progress bar.</summary>
	/// <param name="windowHandle">The handle of the target window.</param>
	/// <param name="progressValue">The current progress value.</param>
	/// <param name="progressMax">The maximum progress value.</param>
	/// <remarks>This method updates the taskbar progress bar value for the specified window handle, allowing for visual feedback on the taskbar.</remarks>
	public static void SetValue(IntPtr windowHandle, ulong progressValue, ulong progressMax)
	{
		// If the window handle is invalid or the taskbar progress API is not supported, exit early.
		if (windowHandle == IntPtr.Zero || !IsSupported)
		{
			return;
		}
		// Attempt to set the taskbar progress value using the cached taskbar instance.
		try
		{
			// Call the SetProgressValue method on the taskbar instance to update the progress bar value.
			GetTaskbarInstance()?.SetProgressValue(hwnd: windowHandle, ullCompleted: progressValue, ullTotal: progressMax);
		}
		// Catch any COM exceptions that may occur during the operation, such as when the Explorer process is restarted.
		catch (COMException)
		{
			// On COM errors, discard the invalid instance
			ResetInstance();
			// Log an error message indicating that setting the taskbar progress value failed.
			logger.Error(message: $"Error setting taskbar progress value for window handle: {windowHandle}");
		}
	}

	/// <summary>Discards the current COM instance and releases its native resources.</summary>
	/// <remarks>This method releases the COM object associated with the taskbar instance, ensuring that native resources are properly cleaned up.</remarks>
	private static void ResetInstance()
	{
		// Lock to ensure that only one thread can reset the taskbar instance at a time.
		lock (TaskbarLock)
		{
			// If the cached taskbar instance is not null, attempt to release it.
			if (_taskbarInstance != null)
			{
				// Release the COM object and handle any exceptions that may occur during the release process.
				try
				{
					// Release the COM object to free native resources.
					Marshal.ReleaseComObject(o: _taskbarInstance);
				}
				catch
				{
					// Ignore if the object has already been released
				}
				finally
				{
					// Set the cached instance to null to indicate that it has been released.
					_taskbarInstance = null;
				}
			}
		}
	}
}

/// <summary>Defines the possible states of the taskbar progress bar.</summary>
/// <remarks>This enumeration represents the different visual states that the taskbar progress bar can display, providing feedback to the user about the progress of an operation.</remarks>
public enum TaskbarProgressState
{
	/// <summary>Disables the taskbar progress bar.</summary>
	/// <remarks>Use this state to hide the progress bar and indicate that no progress is being tracked.</remarks>
	NoProgress = 0x0,

	/// <summary>Displays an indeterminate (marquee) progress.</summary>
	/// <remarks>Use this state to indicate that the progress is ongoing but the exact amount of completion is unknown.</remarks>
	Indeterminate = 0x1,

	/// <summary>Displays a normal green progress bar.</summary>
	/// <remarks>Use this state to indicate that the operation is progressing normally.</remarks>
	Normal = 0x2,

	/// <summary>Displays a red progress bar for errors.</summary>
	/// <remarks>Use this state to indicate that an error has occurred during the operation.</remarks>
	Error = 0x4,

	/// <summary>Displays a yellow progress bar for paused operations.</summary>
	/// <remarks>Use this state to indicate that the operation has been paused.</remarks>
	Paused = 0x8
}

#region COM Interop (Native Windows API)

/// <summary>Represents the COM interface for interacting with the Windows taskbar.</summary>
/// <remarks>This interface provides methods for managing taskbar tabs, progress, and other taskbar-related functionalities.</remarks>
[ComImport] // Indicates that the interface is imported from a COM type library
[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")] // Specifies the GUID of the COM interface
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] // Specifies that the interface uses the IUnknown interface type for COM interop
internal interface ITaskbarList3
{
	// ITaskbarList
	void HrInit();
	void AddTab(IntPtr hwnd);
	void DeleteTab(IntPtr hwnd);
	void ActivateTab(IntPtr hwnd);
	void SetActiveAlt(IntPtr hwnd);
	// ITaskbarList2
	void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(unmanagedType: UnmanagedType.Bool)] bool fFullscreen);
	// ITaskbarList3
	void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
	void SetProgressState(IntPtr hwnd, TaskbarProgressState tbpFlags);
}

[ComImport] // Indicates that the class is imported from a COM type library
[Guid("56FDF344-FD6D-11d0-958A-006097C9A090")] // Specifies the GUID of the COM class
[ClassInterface(ClassInterfaceType.None)] // Specifies that no class interface is generated for the COM class
internal class TaskbarInstance { }

#endregion