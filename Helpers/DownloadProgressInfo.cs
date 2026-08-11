/*
 * File:        DownloadProgressInfo.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
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

namespace Planetoid_DB;

/// <summary>Represents the progress of a download operation, including the current number of bytes downloaded, the total number of bytes to be downloaded, the download speed in bytes per second, the elapsed time since the download started, and the estimated time remaining for the download to complete.</summary>
/// <param name="CurrentBytes">The current number of bytes downloaded.</param>
/// <param name="TotalBytes">The total number of bytes to be downloaded.</param>
/// <param name="BytesPerSecond">The download speed in bytes per second.</param>
/// <param name="Elapsed">The elapsed time since the download started.</param>
/// <param name="Estimated">The estimated time remaining for the download to complete.</param>
/// <remarks>This record struct is used to report the progress of a download operation in a type-safe manner.</remarks>
public record struct DownloadProgressInfo(long CurrentBytes, long TotalBytes, double BytesPerSecond, TimeSpan Elapsed, TimeSpan Estimated);
