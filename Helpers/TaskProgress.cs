// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Runtime.InteropServices;

namespace Planetoid_DB.Helpers;

/// <summary>Controls the progress bar of the program icon in the Windows taskbar.</summary>
/// <remarks>This class provides methods to interact with the Windows taskbar to display progress.</remarks>
public static class TaskbarProgress
{
	private static readonly Lock _taskbarInstanceLock = new();

	/// <summary>Lazily evaluates and caches whether the current OS supports the taskbar progress API.</summary>
	/// <remarks>Uses <see cref="Lazy{T}"/> with thread-safe initialization to perform the OS version check exactly once,
	/// eliminating the need for manual double-checked locking and ensuring correct memory visibility across threads.</remarks>
	private static readonly Lazy<bool> _isTaskbarSupported = new(
		valueFactory: static () => Environment.OSVersion.Version >= new Version(major: 6, minor: 1),
		isThreadSafe: true);

	/// <summary>Gets the instance of the taskbar interface used to manage taskbar features such as progress indicators and
	/// thumbnail previews.</summary>
	/// <remarks>This property initializes the taskbar instance only if it has not been created yet, ensuring thread safety.
	/// It requires Windows 7 or later for proper functionality, and explicit initialization is necessary for Windows 10.
	/// On unsupported OS versions, the support state is cached to avoid repeated locking and version checks.</remarks>
	// Lazy Initialization (Thread-safe), the instance is only created when it is actually needed.
	private static ITaskbarList3? TaskbarInstance
	{
		get
		{
			// On unsupported OS versions, short-circuit before acquiring the lock.
			if (!_isTaskbarSupported.Value)
			{
				return null;
			}
			if (field == null)
			{
				lock (_taskbarInstanceLock)
				{
					if (field == null)
					{
						field = new TaskbarInstance() as ITaskbarList3;
						field?.HrInit(); // IMPORTANT: Windows 10 often requires this explicit initialization
					}
				}
			}
			return field;
		}
	}

	/// <summary>Sets the state of the taskbar progress bar (e.g., Normal, Paused, Error).</summary>
	/// <param name="windowHandle">The handle of the window whose taskbar progress state is being set.</param>
	/// <param name="state">The state to set for the taskbar progress bar.</param>
	/// <remarks>This method requires Windows 7 or later for proper functionality.</remarks>
	public static void SetState(IntPtr windowHandle, TaskbarProgressState state)
	{
		// Prevent calls before the window has a valid handle
		if (windowHandle == IntPtr.Zero)
		{
			return;
		}

		TaskbarInstance?.SetProgressState(hwnd: windowHandle, tbpFlags: state);
	}

	/// <summary>Sets the current value of the taskbar progress bar.</summary>
	/// <param name="windowHandle">The handle of the window whose taskbar progress value is being set.</param>
	/// <param name="progressValue">The current progress value.</param>
	/// <param name="progressMax">The maximum progress value.</param>
	/// <remarks>This method requires Windows 7 or later for proper functionality.</remarks>
	public static void SetValue(IntPtr windowHandle, ulong progressValue, ulong progressMax)
	{
		if (windowHandle == IntPtr.Zero)
		{
			return;
		}

		TaskbarInstance?.SetProgressValue(hwnd: windowHandle, ullCompleted: progressValue, ullTotal: progressMax);
	}
}

/// <summary>Defines the possible states of the taskbar progress bar.</summary>
/// <remarks>This enumeration is used to specify the state of the taskbar progress bar.</remarks>
public enum TaskbarProgressState
{
	/// <summary>Represents a state indicating that no progress has been made.</summary>
	/// <remarks>This state is used when there is no progress to report.</remarks>
	NoProgress = 0,
	/// <summary>Represents a state indicating that the progress is indeterminate.</summary>
	/// <remarks>This state is used when the progress is unknown or cannot be determined.</remarks>
	Indeterminate = 0x1,
	/// <summary>Represents a state indicating that the progress is normal.</summary>
	/// <remarks>This state is used when the progress is being tracked normally.</remarks>
	Normal = 0x2,
	/// <summary>Represents a state indicating that an error has occurred.</summary>
	/// <remarks>This state is used when an error has occurred during the progress.</remarks>
	Error = 0x4,
	/// <summary>Represents a state indicating that the progress is paused.</summary>
	/// <remarks>This state is used when the progress is paused.</remarks>
	Paused = 0x8
}

#region COM Interop (Native Windows API)

[ComImport]
[Guid(guid: "ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
[InterfaceType(interfaceType: ComInterfaceType.InterfaceIsIUnknown)]
internal partial interface ITaskbarList3
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

[ComImport()]
[Guid(guid: "56FDF344-FD6D-11d0-958A-006097C9A090")]
[ClassInterface(classInterfaceType: ClassInterfaceType.None)]
internal class TaskbarInstance { }

#endregion
