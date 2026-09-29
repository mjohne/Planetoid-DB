/*
 * File:        TaskbarProgressState.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Controls the progress bar of the program icon in the Windows taskbar.
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

/// <summary>Defines the possible states of the taskbar progress bar.</summary>
/// <remarks>This enumeration represents the different visual states that the taskbar progress bar can display, providing feedback to the user about the progress of an operation.</remarks>
internal enum TaskbarProgressState
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