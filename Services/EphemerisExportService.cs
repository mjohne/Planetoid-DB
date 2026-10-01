/*
 * File:        EphemerisExportService.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Formats and exports ephemeris entries using the invariant culture.
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

/// <summary>Formats and exports ephemeris entries using the invariant culture.</summary>
internal static class EphemerisExportService
{
	/// <summary>The CSV header with explicit units and reference frames.</summary>
	public const string CsvHeader = "Time (UTC; ISO 8601),RA (h; ICRF/J2000),RA (hms),Dec (deg; ICRF/J2000),Dec (dms),Azimuth (deg; N=0 E=90),Altitude (deg),Delta (AU),r (AU),Phase angle (deg),Elongation (deg),V (mag),Sun altitude (deg),Moon distance (deg),Visible";

	/// <summary>Formats a right ascension as sexagesimal hours (<c>HH MM SS.SS</c>).</summary>
	/// <param name="hours">The right ascension in hours.</param>
	/// <returns>The formatted value.</returns>
	public static string FormatRightAscension(double hours)
	{
		double normalized = CoordinateTransformationService.NormalizeDegrees(degrees: hours * 15.0) / 15.0;
		long hundredths = (long)Math.Round(a: normalized * 360000.0) % (24L * 360000L);
		return string.Create(provider: CultureInfo.InvariantCulture, handler: $"{hundredths / 360000:00} {hundredths / 6000 % 60:00} {hundredths % 6000 / 100.0:00.00}");
	}

	/// <summary>Formats a declination as sexagesimal degrees (<c>±DD MM SS.S</c>).</summary>
	/// <param name="degrees">The declination in degrees.</param>
	/// <returns>The formatted value with an explicit sign.</returns>
	public static string FormatDeclination(double degrees)
	{
		char sign = degrees < 0 ? '-' : '+';
		long tenths = (long)Math.Round(a: Math.Abs(value: degrees) * 36000.0);
		return string.Create(provider: CultureInfo.InvariantCulture, handler: $"{sign}{tenths / 36000:00} {tenths / 600 % 60:00} {tenths % 600 / 10.0:00.0}");
	}

	/// <summary>Writes ephemeris entries as CSV (comma separated, invariant culture).</summary>
	/// <param name="entries">The entries.</param>
	/// <param name="writer">The target writer.</param>
	/// <param name="cancellationToken">A token to cancel the export.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public static async Task WriteCsvAsync(IEnumerable<EphemerisEntry> entries, TextWriter writer, CancellationToken cancellationToken = default)
	{
		await writer.WriteLineAsync(buffer: CsvHeader.AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		foreach (EphemerisEntry entry in entries)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await writer.WriteLineAsync(buffer: FormatCsvLine(entry: entry).AsMemory(), cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		await writer.FlushAsync(cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	/// <summary>Exports ephemeris entries to a UTF-8 CSV file.</summary>
	/// <param name="entries">The entries.</param>
	/// <param name="filePath">The target file path.</param>
	/// <param name="cancellationToken">A token to cancel the export.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public static async Task ExportCsvAsync(IEnumerable<EphemerisEntry> entries, string filePath, CancellationToken cancellationToken = default)
	{
		StreamWriter writer = new(path: filePath, append: false, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		await using (writer.ConfigureAwait(continueOnCapturedContext: false))
		{
			await WriteCsvAsync(entries: entries, writer: writer, cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	/// <summary>Formats one entry as a CSV line.</summary>
	/// <param name="entry">The entry.</param>
	/// <returns>The CSV line.</returns>
	public static string FormatCsvLine(EphemerisEntry entry)
	{
		CultureInfo c = CultureInfo.InvariantCulture;
		return string.Join(separator: ',',
			entry.Time.UtcDateTime.ToString(format: "yyyy-MM-ddTHH:mm:ssZ", provider: c),
			entry.RightAscensionHours.ToString(format: "F6", provider: c),
			FormatRightAscension(hours: entry.RightAscensionHours),
			entry.DeclinationDegrees.ToString(format: "F5", provider: c),
			FormatDeclination(degrees: entry.DeclinationDegrees),
			entry.AzimuthDegrees.ToString(format: "F3", provider: c),
			entry.AltitudeDegrees.ToString(format: "F3", provider: c),
			entry.DistanceAu.ToString(format: "F8", provider: c),
			entry.HeliocentricDistanceAu.ToString(format: "F8", provider: c),
			entry.PhaseAngleDegrees.ToString(format: "F2", provider: c),
			entry.SolarElongationDegrees.ToString(format: "F2", provider: c),
			double.IsNaN(d: entry.ApparentMagnitude) ? string.Empty : entry.ApparentMagnitude.ToString(format: "F2", provider: c),
			entry.SunAltitudeDegrees.ToString(format: "F2", provider: c),
			entry.MoonSeparationDegrees.ToString(format: "F2", provider: c),
			entry.IsVisible ? "true" : "false");
	}
}
