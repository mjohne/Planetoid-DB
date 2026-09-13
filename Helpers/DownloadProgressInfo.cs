/*
 * File:        DownloadProgressInfo.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Represents the progress of a download operation, including the current number of bytes downloaded, the total number of bytes to be downloaded, the download speed in bytes per second, the elapsed time since the download started, and the estimated time remaining for the download to complete.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics;

namespace Planetoid_DB.Helpers;

/// <summary>Represents the progress of a download operation, including the current number of bytes downloaded, the total number of bytes to be downloaded, the download speed in bytes per second, the elapsed time since the download started, and the estimated time remaining for the download to complete.</summary>
/// <param name="CurrentBytes">The current number of bytes downloaded.</param>
/// <param name="TotalBytes">The total number of bytes to be downloaded.</param>
/// <param name="BytesPerSecond">The download speed in bytes per second.</param>
/// <param name="Elapsed">The elapsed time since the download started.</param>
/// <param name="Estimated">The estimated time remaining for the download to complete.</param>
/// <remarks>This record struct is used to report the progress of a download operation in a type-safe manner.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal record struct DownloadProgressInfo(long CurrentBytes, long TotalBytes, double BytesPerSecond, TimeSpan Elapsed, TimeSpan Estimated)
{
	/// <summary>Returns a string representation of the download progress information for debugging purposes. This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise and informative display of the record's properties in the debugger.</summary>
	/// <returns>A string representation of the download progress information.</returns>
	/// <remarks>This property is used by the debugger to display the record's properties in a human-readable format, making it easier to inspect the download progress during debugging sessions.</remarks>
	private readonly string DebuggerDisplay => ToString() ?? string.Empty;
}