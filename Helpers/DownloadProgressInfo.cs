// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Represents the progress of a download operation, including the current number of bytes downloaded, the total number of bytes to be downloaded, the download speed in bytes per second, the elapsed time since the download started, and the estimated time remaining for the download to complete.</summary>
/// <param name="CurrentBytes">The current number of bytes downloaded.</param>
/// <param name="TotalBytes">The total number of bytes to be downloaded.</param>
/// <param name="BytesPerSecond">The download speed in bytes per second.</param>
/// <param name="Elapsed">The elapsed time since the download started.</param>
/// <param name="Estimated">The estimated time remaining for the download to complete.</param>
/// <remarks>This record struct is used to report the progress of a download operation in a type-safe manner.</remarks>
public record struct DownloadProgressInfo(long CurrentBytes, long TotalBytes, double BytesPerSecond, TimeSpan Elapsed, TimeSpan Estimated);
