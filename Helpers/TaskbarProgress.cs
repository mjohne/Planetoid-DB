using NLog;

using System.Runtime.InteropServices;

namespace Planetoid_DB.Helpers;

/// <summary>
/// Provides methods to control taskbar progress.
/// </summary>
/// <remarks>
/// This class provides methods to control the taskbar progress indicator.
/// </remarks>
public static class TaskbarProgress
{
	/// <summary>
	/// NLog logger instance for logging application events.
	/// </summary>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	/// <summary>
	/// Defines the different states of the taskbar progress.
	/// </summary>
	/// <remarks>
	/// This enumeration defines the various states of the taskbar progress.
	/// </remarks>
	public enum TaskbarStates
	{
		/// <summary>
		/// No progress.
		/// </summary>
		/// <remarks>
		/// This state indicates that there is no progress to report.
		/// </remarks>
		NoProgress = 0,
		/// <summary>
		/// Indeterminate Progress.
		/// </summary>
		/// <remarks>
		/// This state indicates that the progress is indeterminate.
		/// </remarks>
		Indeterminate = 0x1,
		/// <summary>
		/// Normal progress.
		/// </summary>
		/// <remarks>
		/// This state indicates that the progress is normal.
		/// </remarks>
		Normal = 0x2,
		/// <summary>
		/// Faulty progress.
		/// </summary>
		/// <remarks>
		/// This state indicates that the progress has encountered an error.
		/// </remarks>
		Error = 0x4,
		/// <summary>
		/// Paused progress.
		/// </summary>
		/// <remarks>
		/// This state indicates that the progress is paused.
		/// </remarks>
		Paused = 0x8
	}

	[ComImport]
	[Guid(guid: "ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[InterfaceType(interfaceType: ComInterfaceType.InterfaceIsIUnknown)]
	private interface ITaskbarList3
	{
		// ITaskbarList
		[PreserveSig]
		void HrInit();
		[PreserveSig]
		void AddTab(IntPtr handleWindow);
		[PreserveSig]
		void DeleteTab(IntPtr handleWindow);
		[PreserveSig]
		void ActivateTab(IntPtr handleWindow);
		[PreserveSig]
		void SetActiveAlt(IntPtr handleWindow);

		// ITaskbarList2
		[PreserveSig]
		void MarkFullscreenWindow(IntPtr handleWindow, [MarshalAs(unmanagedType: UnmanagedType.Bool)] bool fFullscreen);

		// ITaskbarList3
		[PreserveSig]
		void SetProgressValue(IntPtr handleWindow, ulong ullCompleted, ulong ullTotal);
		[PreserveSig]
		void SetProgressState(IntPtr handleWindow, TaskbarStates state);
	}

	[Guid(guid: "56FDF344-FD6D-11d0-958A-006097C9A090")]
	[ClassInterface(classInterfaceType: ClassInterfaceType.None)]
	[ComImport]
	private class TaskbarInstance
	{
	}

	private static readonly ITaskbarList3 taskbarInstance = (ITaskbarList3)new TaskbarInstance();
	private static readonly bool taskbarSupported = Environment.OSVersion.Version >= new Version(major: 6, minor: 1);

	/// <summary>
	/// Sets the state of the taskbar progress.
	/// </summary>
	/// <param name="windowHandle">The handle of the window.</param>
	/// <param name="taskbarState">The new state of the taskbar progress.</param>
	/// <remarks>
	/// This method sets the state of the taskbar progress indicator for the specified window.
	/// </remarks>
	public static void SetState(IntPtr windowHandle, TaskbarStates taskbarState)
	{
		if (!taskbarSupported)
		{
			return;
		}

		try
		{
			taskbarInstance.SetProgressState(handleWindow: windowHandle, state: taskbarState);
		}
		catch (Exception ex)
		{
			Logger.Error(exception: ex, message: "Error setting taskbar progress state");
		}
	}

	/// <summary>
	/// Sets the progress value of the taskbar progress.
	/// </summary>
	/// <param name="windowHandle">The handle of the window.</param>
	/// <param name="progressValue">The current progress value.</param>
	/// <param name="progressMax">The maximum progress value.</param>
	/// <remarks>
	/// This method sets the progress value of the taskbar progress indicator for the specified window.
	/// </remarks>
	public static void SetValue(IntPtr windowHandle, double progressValue, double progressMax)
	{
		if (!taskbarSupported)
		{
			return;
		}

		try
		{
			taskbarInstance.SetProgressValue(handleWindow: windowHandle, ullCompleted: (ulong)progressValue, ullTotal: (ulong)progressMax);
		}
		catch (Exception ex)
		{
			Logger.Error(exception: ex, message: "Error setting taskbar progress value");
		}
	}
}