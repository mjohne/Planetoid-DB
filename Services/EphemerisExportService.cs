/*
 * File:        EphemerisExportService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Exports ephemerides in a culture-independent CSV format.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;
using System.Text;

namespace Planetoid_DB.Services;

/// <summary>Exports ephemerides as CSV using <see cref="CultureInfo.InvariantCulture"/> and unit-labeled columns.</summary>
/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
internal static class EphemerisExportService
{
	/// <summary>The CSV header with units.</summary>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	public const string CsvHeader = "Time (UTC, ISO 8601),RA of date (h),RA of date (hms),Dec of date (deg),Dec of date (dms),RA J2000 (h),RA J2000 (hms),Dec J2000 (deg),Dec J2000 (dms),Azimuth (deg),Altitude (deg),Distance (AU),Heliocentric distance (AU),Magnitude (mag),Phase angle (deg),Elongation (deg),Sun altitude (deg),Moon separation (deg),Visible";

	/// <summary>Creates the CSV text of an ephemeris.</summary>
	/// <param name="entries">The ephemeris entries.</param>
	/// <param name="designation">An optional designation written as a comment line.</param>
	/// <param name="observer">An optional observer location written as a comment line.</param>
	/// <returns>The CSV text.</returns>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	public static string ToCsv(IEnumerable<EphemerisEntry> entries, string? designation = null, ObserverLocation? observer = null)
	{
		// Validate arguments
		ArgumentNullException.ThrowIfNull(argument: entries);
		// Create a StringBuilder to build the CSV text
		CultureInfo ic = CultureInfo.InvariantCulture;
		// Append the optional designation and observer location as comment lines, then append the CSV header and each entry formatted as a CSV line
		StringBuilder sb = new();
		// Append the optional designation as a comment line if provided
		if (!string.IsNullOrWhiteSpace(value: designation))
		{
			_ = sb.Append(value: "# Object: ").AppendLine(value: designation.Trim());
		}
		// Append the optional observer location as a comment line if provided
		if (observer is not null)
		{
			_ = sb.AppendLine(value: string.Create(provider: ic, handler: $"# Observer: latitude {observer.LatitudeDegrees:0.######} deg, longitude {observer.LongitudeDegrees:0.######} deg (east positive), elevation {observer.ElevationMeters:0.#} m"));
		}
		// Append the CSV header
		_ = sb.AppendLine(value: CsvHeader);
		// Append each entry formatted as a CSV line
		foreach (EphemerisEntry e in entries)
		{
			_ = sb.AppendLine(value: FormatCsvLine(entry: e, provider: ic));
		}
		// Return the CSV text as a string
		return sb.ToString();
	}

	/// <summary>Writes the CSV text of an ephemeris to a file (UTF-8).</summary>
	/// <param name="filePath">The file path.</param>
	/// <param name="entries">The ephemeris entries.</param>
	/// <param name="designation">An optional designation.</param>
	/// <param name="observer">An optional observer location.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	public static Task ExportCsvAsync(string filePath, IEnumerable<EphemerisEntry> entries, string? designation = null, ObserverLocation? observer = null, CancellationToken cancellationToken = default)
	{
		// Validate arguments
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: filePath);
		ArgumentNullException.ThrowIfNull(argument: entries);
		// Create a task to write the CSV text to the specified file path asynchronously
		return Task.Run(function: async () =>
		{
			// Create a FileStream and StreamWriter to write the CSV text to the file asynchronously
			FileStream stream = new(path: filePath, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None, bufferSize: 4096, options: FileOptions.Asynchronous);
			StreamWriter writer = new(stream: stream, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), bufferSize: 1024, leaveOpen: true);
			// Use ConfigureAwait(false) to avoid capturing the synchronization context
			await using (stream.ConfigureAwait(continueOnCapturedContext: false))
			await using (writer.ConfigureAwait(continueOnCapturedContext: false))
			{
				// Write the optional designation and observer location as comment lines, then write the CSV header and each entry formatted as a CSV line
				if (!string.IsNullOrWhiteSpace(value: designation))
				{
					// Write the designation as a comment line
					await writer.WriteLineAsync(buffer: $"# Object: {designation.Trim()}".AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				// Write the optional observer location as a comment line if provided
				if (observer is not null)
				{
					// Write the observer location as a comment line
					await writer.WriteLineAsync(buffer: string.Create(provider: CultureInfo.InvariantCulture, handler: $"# Observer: latitude {observer.LatitudeDegrees:0.######} deg, longitude {observer.LongitudeDegrees:0.######} deg (east positive), elevation {observer.ElevationMeters:0.#} m").AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				// Write the CSV header
				await writer.WriteLineAsync(buffer: CsvHeader.AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				// Write each entry formatted as a CSV line
				foreach (EphemerisEntry entry in entries)
				{
					cancellationToken.ThrowIfCancellationRequested();
					// Write the formatted CSV line for the entry
					await writer.WriteLineAsync(buffer: FormatCsvLine(entry: entry, provider: CultureInfo.InvariantCulture).AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}, cancellationToken: cancellationToken);
	}

	/// <summary>Formats a number invariantly; <see cref="double.NaN"/> becomes an empty field.</summary>
	/// <param name="value">The value.</param>
	/// <param name="format">The numeric format.</param>
	/// <returns>The formatted text.</returns>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	private static string Format(double value, string format)
	{
		// Validate arguments
		ArgumentNullException.ThrowIfNull(argument: format);
		// Format the value using the specified format if it is finite; otherwise, return an empty string
		return double.IsFinite(d: value) ? value.ToString(format: format, provider: CultureInfo.InvariantCulture) : string.Empty;
	}

	/// <summary>Formats a value with a custom formatter; <see cref="double.NaN"/> becomes an empty field.</summary>
	/// <param name="value">The value.</param>
	/// <param name="formatter">The formatter applied to finite values.</param>
	/// <returns>The formatted text.</returns>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	private static string FormatOrEmpty(double value, Func<double, string> formatter)
	{
		// Validate arguments
		ArgumentNullException.ThrowIfNull(argument: formatter);
		// Format the value using the provided formatter if it is finite; otherwise, return an empty string
		return double.IsFinite(d: value) ? formatter(arg: value) : string.Empty;
	}

	/// <summary>Formats one CSV row.</summary>
	/// <param name="entry">The ephemeris entry.</param>
	/// <param name="provider">The formatting culture.</param>
	/// <returns>The CSV row.</returns>
	/// <remarks>All values are expressed in astronomical units [AU], days [d] and degrees [°] unless otherwise specified.</remarks>
	private static string FormatCsvLine(EphemerisEntry entry, IFormatProvider provider)
	{
		// Validate arguments
		ArgumentNullException.ThrowIfNull(argument: entry);
		ArgumentNullException.ThrowIfNull(argument: provider);
		// Format: Time (UTC, ISO 8601),RA of date (h),RA of date (hms),Dec of date (deg),Dec of date (dms),RA J2000 (h),RA J2000 (hms),Dec J2000 (deg),Dec J2000 (dms),Azimuth (deg),Altitude (deg),Distance (AU),Heliocentric distance (AU),Magnitude (mag),Phase angle (deg),Elongation (deg),Sun altitude (deg),Moon separation (deg),Visible
		return string.Create(provider: provider, handler:
			$"{entry.Time.ToUniversalTime().UtcDateTime:yyyy-MM-ddTHH:mm:ssZ},{entry.RightAscensionHours:0.000000},{CoordinateTransformationService.FormatRightAscension(hours: entry.RightAscensionHours)},{entry.DeclinationDegrees:0.00000},{CoordinateTransformationService.FormatDeclination(degrees: entry.DeclinationDegrees)},{Format(value: entry.AstrometricRightAscensionHours, format: "0.000000")},{FormatOrEmpty(value: entry.AstrometricRightAscensionHours, formatter: CoordinateTransformationService.FormatRightAscension)},{Format(value: entry.AstrometricDeclinationDegrees, format: "0.00000")},{FormatOrEmpty(value: entry.AstrometricDeclinationDegrees, formatter: CoordinateTransformationService.FormatDeclination)},{entry.AzimuthDegrees:0.0000},{entry.AltitudeDegrees:0.0000},{entry.DistanceAu:0.0000000},{entry.HeliocentricDistanceAu:0.0000000},{Format(value: entry.ApparentMagnitude, format: "0.00")},{Format(value: entry.PhaseAngleDegrees, format: "0.00")},{Format(value: entry.ElongationDegrees, format: "0.00")},{Format(value: entry.SunAltitudeDegrees, format: "0.00")},{Format(value: entry.MoonSeparationDegrees, format: "0.00")},{(entry.IsVisible ? "yes" : "no")}");
	}
}
