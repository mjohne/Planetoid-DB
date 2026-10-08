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
internal static class EphemerisExportService
{
	/// <summary>The CSV header with units.</summary>
	public const string CsvHeader = "Time (UTC, ISO 8601),RA (h),RA (hms),Dec (deg),Dec (dms),Azimuth (deg),Altitude (deg),Distance (AU),Heliocentric distance (AU),Magnitude (mag),Phase angle (deg),Elongation (deg),Sun altitude (deg),Moon separation (deg),Visible";

	/// <summary>Creates the CSV text of an ephemeris.</summary>
	/// <param name="entries">The ephemeris entries.</param>
	/// <param name="designation">An optional designation written as a comment line.</param>
	/// <param name="observer">An optional observer location written as a comment line.</param>
	/// <returns>The CSV text.</returns>
	public static string ToCsv(IEnumerable<EphemerisEntry> entries, string? designation = null, ObserverLocation? observer = null)
	{
		ArgumentNullException.ThrowIfNull(argument: entries);
		CultureInfo ic = CultureInfo.InvariantCulture;
		StringBuilder sb = new();
		if (!string.IsNullOrWhiteSpace(value: designation))
		{
			_ = sb.Append(value: "# Object: ").AppendLine(value: designation.Trim());
		}
		if (observer is not null)
		{
			_ = sb.AppendLine(value: string.Create(provider: ic, handler: $"# Observer: latitude {observer.LatitudeDegrees:0.######} deg, longitude {observer.LongitudeDegrees:0.######} deg (east positive), elevation {observer.ElevationMeters:0.#} m"));
		}
		_ = sb.AppendLine(value: CsvHeader);
		foreach (EphemerisEntry e in entries)
		{
			_ = sb.AppendLine(value: string.Create(provider: ic, handler:
				$"{e.Time.ToUniversalTime().UtcDateTime:yyyy-MM-ddTHH:mm:ssZ},{e.RightAscensionHours:0.000000},{CoordinateTransformationService.FormatRightAscension(hours: e.RightAscensionHours)},{e.DeclinationDegrees:0.00000},{CoordinateTransformationService.FormatDeclination(degrees: e.DeclinationDegrees)},{e.AzimuthDegrees:0.0000},{e.AltitudeDegrees:0.0000},{e.DistanceAu:0.0000000},{e.HeliocentricDistanceAu:0.0000000},{Format(value: e.ApparentMagnitude, format: "0.00")},{Format(value: e.PhaseAngleDegrees, format: "0.00")},{Format(value: e.ElongationDegrees, format: "0.00")},{Format(value: e.SunAltitudeDegrees, format: "0.00")},{Format(value: e.MoonSeparationDegrees, format: "0.00")},{(e.IsVisible ? "yes" : "no")}"));
		}
		return sb.ToString();
	}

	/// <summary>Writes the CSV text of an ephemeris to a file (UTF-8).</summary>
	/// <param name="filePath">The file path.</param>
	/// <param name="entries">The ephemeris entries.</param>
	/// <param name="designation">An optional designation.</param>
	/// <param name="observer">An optional observer location.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public static Task ExportCsvAsync(string filePath, IEnumerable<EphemerisEntry> entries, string? designation = null, ObserverLocation? observer = null, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: filePath);
		return File.WriteAllTextAsync(path: filePath, contents: ToCsv(entries: entries, designation: designation, observer: observer), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken: cancellationToken);
	}

	/// <summary>Formats a number invariantly; <see cref="double.NaN"/> becomes an empty field.</summary>
	/// <param name="value">The value.</param>
	/// <param name="format">The numeric format.</param>
	/// <returns>The formatted text.</returns>
	private static string Format(double value, string format) => double.IsFinite(d: value) ? value.ToString(format: format, provider: CultureInfo.InvariantCulture) : string.Empty;
}
