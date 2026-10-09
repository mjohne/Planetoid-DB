/*
 * File:        EphemerisTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for the ephemeris calculation.
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
using System.Globalization;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="EphemerisService"/>, the coordinate transformations, visibility and export.</summary>
/// <remarks>These tests use the analytical planetary ephemeris to avoid external files and make the tests self-contained.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class EphemerisTests
{
	/// <summary>The service with the analytical planetary ephemeris.</summary>
	/// <remarks>Using the analytical ephemeris avoids the need for external files and makes the tests self-contained.</remarks>
	private readonly EphemerisService service = new(planetaryEphemeris: new AnalyticalPlanetaryEphemeris());

	/// <summary>Verifies that a time grid crossing midnight and the turn of the year is continuous and in UTC.</summary>
	/// <remarks>Time grids are used for ephemeris calculations and must be continuous and in UTC.</remarks>
	[Fact]
	public void TimeGridCrossesDateChange()
	{
		// Create a time grid from 2025-12-31 22:00 UTC to 2026-01-01 02:00 UTC with 1-hour steps.
		DateTimeOffset start = new(year: 2025, month: 12, day: 31, hour: 22, minute: 0, second: 0, offset: TimeSpan.Zero);
		// The grid should contain 5 points: 22:00, 23:00, 00:00, 01:00, 02:00 UTC.
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddHours(hours: 4), step: TimeSpan.FromHours(hours: 1));
		// Verify the grid has 5 points and the middle point is 2026-01-01 00:00 UTC.
		Assert.Equal(expected: 5, actual: grid.Count);
		// The middle point (index 2) should be 2026-01-01 00:00 UTC.
		Assert.Equal(expected: new DateTimeOffset(year: 2026, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero), actual: grid[2]);
		// All points in the grid should have an offset of zero (UTC).
		Assert.All(collection: grid, action: static t => Assert.Equal(expected: TimeSpan.Zero, actual: t.Offset));
	}

	/// <summary>Verifies the Julian date across the date change (JD starts at noon).</summary>
	/// <remarks>The Julian date starts at noon, so 2000-01-01 12:00 UTC is JD 2451545.0, and 2026-01-01 00:00 UTC is JD 2461041.5.</remarks>
	[Fact]
	public void JulianDateDateChange()
	{
		// 2000-01-01 12:00 UTC is JD 2451545.0
		Assert.Equal(expected: 2451545.0, actual: TimeScales.ToJulianDate(dateTime: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc)), precision: 9);
		// 2026-01-01 00:00 UTC is JD 2461041.5
		Assert.Equal(expected: 2461041.5, actual: TimeScales.ToJulianDate(dateTime: new DateTime(year: 2026, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)), precision: 9);
	}

	/// <summary>Verifies the Espenak–Meeus ΔT polynomial transitions in the historical ranges.</summary>
	/// <remarks>The ΔT polynomial is used to convert between TT and UTC. The values are taken from Espenak &amp; Meeus, "Five Millennium Canon of Solar Eclipses", NASA/TP-2006-214141, 2006.</remarks>
	[Theory]
	[InlineData(1600, 120.0)]
	[InlineData(1700, 8.83)]
	[InlineData(1800, 13.72)]
	[InlineData(1860, 7.62)]
	[InlineData(1900, -2.79)]
	public void DeltaTHistoricalRanges(int year, double expectedSeconds)
	{
		// The expected ΔT values are taken from Espenak &amp; Meeus, "Five Millennium Canon of Solar Eclipses", NASA/TP-2006-214141, 2006.
		DateTime utc = new(year: year, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);
		// The actual ΔT is calculated by TimeScales.TtMinusUtcSeconds, which returns the difference in seconds between TT and UTC.
		Assert.InRange(actual: TimeScales.TtMinusUtcSeconds(utc: utc), low: expectedSeconds - 1.0, high: expectedSeconds + 1.0);
	}

	/// <summary>Verifies that the ephemeris across midnight is continuous.</summary>
	/// <remarks>This test calculates the ephemeris of Ceres across midnight and checks that the declination and distance change smoothly.</remarks>
	[Fact]
	public void EphemerisIsContinuousAcrossMidnight()
	{
		// Create a time grid from 2025-12-31 23:00 UTC to 2026-01-01 01:00 UTC with 1-hour steps.
		DateTimeOffset start = new(year: 2025, month: 12, day: 31, hour: 23, minute: 0, second: 0, offset: TimeSpan.Zero);
		// The grid should contain 3 points: 23:00, 00:00, 01:00 UTC.
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddHours(hours: 2), step: TimeSpan.FromHours(hours: 1));
		// Calculate the ephemeris of Ceres for the time grid and observer at Greenwich.
		IReadOnlyList<EphemerisEntry> result = service.Calculate(elements: TestData.Ceres(), times: grid, observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken);
		// Verify that the result has 3 entries and that the declination and distance change smoothly across midnight.
		Assert.Equal(expected: 3, actual: result.Count);
		// The declination and distance should not change abruptly; the difference between consecutive entries should be small.
		for (int i = 1; i < result.Count; i++)
		{
			// The declination difference should be less than 0.1 degrees, and the distance difference should be less than 0.01 AU.
			Assert.InRange(actual: Math.Abs(value: result[i].DeclinationDegrees - result[i - 1].DeclinationDegrees), low: 0.0, high: 0.1);
			// The distance difference should be less than 0.01 AU.
			Assert.InRange(actual: Math.Abs(value: result[i].DistanceAu - result[i - 1].DistanceAu), low: 0.0, high: 0.01);
		}
	}

	/// <summary>Verifies asynchronous CSV export matches the synchronous formatter and honors cancellation.</summary>
	/// <remarks>This test creates a temporary CSV file, exports a single ephemeris entry asynchronously, and compares the content with the synchronous formatter. It also tests cancellation.</remarks>
	[Fact]
	public async Task CsvExportAsyncMatchesFormatterAndSupportsCancellation()
	{
		// Create a single ephemeris entry with arbitrary values.
		EphemerisEntry entry = new(Time: DateTimeOffset.UnixEpoch, RightAscensionHours: 12.5, DeclinationDegrees: -10.25, AzimuthDegrees: 180.5, AltitudeDegrees: -1.5, DistanceAu: 1.25, ApparentMagnitude: 8.0, IsVisible: false);
		// Create a temporary file path for the CSV export.
		string filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.csv");
		// Ensure the file is deleted after the test, even if an exception occurs.
		try
		{
			// Export the entry to CSV asynchronously and compare the content with the synchronous formatter.
			await EphemerisExportService.ExportCsvAsync(filePath: filePath, entries: [entry], designation: "(1) Ceres", observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken);
			// Read the content of the exported CSV file and compare it with the expected CSV string from the synchronous formatter.
			Assert.Equal(expected: EphemerisExportService.ToCsv(entries: [entry], designation: "(1) Ceres", observer: TestData.Greenwich), actual: await File.ReadAllTextAsync(path: filePath, cancellationToken: TestContext.Current.CancellationToken));
			// Test cancellation by creating a pre-canceled token and calling the export method, expecting an OperationCanceledException.
			using CancellationTokenSource cts = new();
			// Cancel the token before calling the export method.
			await cts.CancelAsync();
			// The export method should throw an OperationCanceledException due to the pre-canceled token.
			_ = await Assert.ThrowsAnyAsync<OperationCanceledException>(testCode: () => EphemerisExportService.ExportCsvAsync(filePath: filePath, entries: [entry], cancellationToken: cts.Token));
		}
		// Finally, delete the temporary file to clean up.
		finally
		{
			// Delete the temporary file if it exists.
			File.Delete(path: filePath);
		}
	}

	/// <summary>Verifies the plausibility of the ephemeris of Ceres.</summary>
	/// <remarks>This test calculates the ephemeris of Ceres over a 60-day period and checks that the right ascension, declination, azimuth, distance, heliocentric distance, apparent magnitude, and time offset are within plausible ranges.</remarks>
	[Fact]
	public async Task EphemerisCeresIsPlausible()
	{
		// Create a time grid from 2025-06-01 00:00 UTC to 2025-07-31 00:00 UTC with 10-day steps.
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		//	The grid should contain 7 points: 2025-06-01, 2025-06-11, 2025-06-21, 2025-07-01, 2025-07-11, 2025-07-21, 2025-07-31 UTC.
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 60), step: TimeSpan.FromDays(days: 10));
		// Calculate the ephemeris of Ceres for the time grid and observer at Greenwich asynchronously.
		IReadOnlyList<EphemerisEntry> result = await service.CalculateAsync(elements: TestData.Ceres(), times: grid, observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken);
		// Verify that the result has 7 entries and that the right ascension, declination, azimuth, distance, heliocentric distance, apparent magnitude, and time offset are within plausible ranges.
		Assert.All(collection: result, action: static e =>
		{
			// Right ascension should be in [0, 24) hours, declination in [-40, 40] degrees, azimuth in [0, 360) degrees, distance in [1.5, 4.0] AU, heliocentric distance in [2.5, 3.0] AU, apparent magnitude in [6.0, 10.0], and time offset should be zero (UTC).
			Assert.InRange(actual: e.RightAscensionHours, low: 0.0, high: 24.0);
			// Declination should be in the range of -40 to 40 degrees for Ceres.
			Assert.InRange(actual: e.DeclinationDegrees, low: -40.0, high: 40.0);
			// Azimuth should be in the range of 0 to 360 degrees.
			Assert.InRange(actual: e.AzimuthDegrees, low: 0.0, high: 360.0);
			// Distance should be in the range of 1.5 to 4.0 AU for Ceres.
			Assert.InRange(actual: e.DistanceAu, low: 1.5, high: 4.0);
			// Heliocentric distance should be in the range of 2.5 to 3.0 AU for Ceres.
			Assert.InRange(actual: e.HeliocentricDistanceAu, low: 2.5, high: 3.0);
			// Apparent magnitude should be in the range of 6.0 to 10.0 for Ceres.
			Assert.InRange(actual: e.ApparentMagnitude, low: 6.0, high: 10.0);
			// Time offset should be zero (UTC).
			Assert.Equal(expected: TimeSpan.Zero, actual: e.Time.Offset);
		});
	}

	/// <summary>Verifies perturbation propagation is independent of request order and starts at the elements' epoch.</summary>
	/// <remarks>This test calculates the ephemeris of Ceres at three different times, both individually and combined, and checks that the results are consistent. It also verifies that the perturbations propagate outward from the elements' epoch.</remarks>
	[Fact]
	public void EphemerisPerturbedRequestsPropagateOutwardFromEpoch()
	{
		// The elements of Ceres have an epoch of 2024-01-01, so the perturbations should propagate outward from that date. The test calculates the ephemeris at three different times: 2000-01-01, 2024-01-01, and 2025-06-01. The results should be consistent whether calculated individually or combined.
		DateTimeOffset[] times =
		[
			new(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero),
			new(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero),
			new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero)
		];
		// Calculate the ephemeris of Ceres for the three times combined.
		IReadOnlyList<EphemerisEntry> combined = service.Calculate(elements: TestData.Ceres(), times: times, observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken);
		// Calculate the ephemeris of Ceres for each time individually and compare with the combined result.
		for (int i = 0; i < times.Length; i++)
		{
			// Calculate the ephemeris of Ceres for the individual time and assert that there is only one entry in the result.
			EphemerisEntry individual = Assert.Single(collection: service.Calculate(elements: TestData.Ceres(), times: [times[i]], observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken));
			// Compare the individual result with the combined result at the same index, allowing for a small precision error.
			Assert.Equal(expected: individual.RightAscensionHours, actual: combined[index: i].RightAscensionHours, precision: 8);
			Assert.Equal(expected: individual.DeclinationDegrees, actual: combined[index: i].DeclinationDegrees, precision: 8);
			Assert.Equal(expected: individual.DistanceAu, actual: combined[index: i].DistanceAu, precision: 8);
		}
	}

	/// <summary>Verifies that a refracted altitude does not change whether the geometric position is above the horizon.</summary>
	/// <remarks>This test creates an ephemeris entry with a refracted altitude above the horizon but a geometric altitude below the horizon, and checks that the IsAboveHorizon property is false.</remarks>
	[Fact]
	public void EphemerisEntryUsesGeometricAltitudeForHorizon()
	{
		// Create an ephemeris entry with a refracted altitude of 0.06 degrees (above the horizon) but a geometric altitude of -0.5 degrees (below the horizon).
		EphemerisEntry entry = new(
			Time: DateTimeOffset.UnixEpoch,
			RightAscensionHours: 0.0,
			DeclinationDegrees: 0.0,
			AzimuthDegrees: 0.0,
			AltitudeDegrees: 0.06,
			DistanceAu: 1.0,
			ApparentMagnitude: 10.0,
			IsVisible: false,
			GeometricAltitudeDegrees: -0.5);
		// The IsAboveHorizon property should be false because the geometric altitude is below the horizon, even though the refracted altitude is above the horizon.
		Assert.False(condition: entry.IsAboveHorizon);
	}

	/// <summary>Verifies that a pre-canceled token cancels the calculation.</summary>
	/// <remarks>This test creates a pre-canceled token and calls the CalculateAsync method, expecting an OperationCanceledException.</remarks>
	[Fact]
	public async Task EphemerisCancellation()
	{
		// Create a pre-canceled token source and call the CalculateAsync method, expecting an OperationCanceledException.
		using CancellationTokenSource cts = new();
		// Cancel the token before calling the CalculateAsync method.
		await cts.CancelAsync();
		// The CalculateAsync method should throw an OperationCanceledException due to the pre-canceled token.
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		// The test expects an OperationCanceledException when calling CalculateAsync with the pre-canceled token.
		_ = await Assert.ThrowsAnyAsync<OperationCanceledException>(testCode: () => service.CalculateAsync(elements: TestData.Ceres(), times: [start], observer: TestData.Greenwich, cancellationToken: cts.Token));
	}

	/// <summary>Verifies negative declinations in conversion and formatting.</summary>
	/// <remarks>This test checks that negative declinations are correctly converted from a vector to right ascension and declination, and that the formatting of declinations produces the expected string representation.</remarks>
	[Fact]
	public void NegativeDeclination()
	{
		// The vector corresponds to a declination of -0.5 radians and a right ascension of 6 hours (90 degrees).
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: new Vector3d(X: 0.0, Y: Math.Cos(d: -0.5), Z: Math.Sin(a: -0.5)));
		// The right ascension should be 6 hours (90 degrees) and the declination should be -0.5 radians converted to degrees.
		Assert.Equal(expected: 6.0, actual: ra, precision: 9);
		// The declination should be -0.5 radians converted to degrees.
		Assert.Equal(expected: -0.5 * 180.0 / Math.PI, actual: dec, precision: 9);
		// The formatting of declinations should produce the expected string representation.
		Assert.Equal(expected: "-00° 30′ 00.0″", actual: CoordinateTransformationService.FormatDeclination(degrees: -0.5));
		// The formatting of declinations should produce the expected string representation for a more negative declination.
		Assert.Equal(expected: "-23° 26′ 21.4″", actual: CoordinateTransformationService.FormatDeclination(degrees: -23.439278));
		// The formatting of declinations should produce the expected string representation for a positive declination.
		Assert.Equal(expected: "+00° 00′ 00.0″", actual: CoordinateTransformationService.FormatDeclination(degrees: 0.0));
	}

	/// <summary>Verifies the ephemeris for a southern observer and object below the celestial equator.</summary>
	/// <remarks>This test checks that the conversion from equatorial to horizontal coordinates for a southern observer and an object with negative declination produces the expected azimuth and altitude.</remarks>
	[Fact]
	public void NegativeDeclinationSouthernObserver()
	{
		// For a southern observer at latitude -31.2733° and an object with declination -61.2733°, the expected azimuth is 180° (south) and the expected altitude is 60°.
		(double az, double alt) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 0.0, declinationDegrees: -61.2733, latitudeDegrees: -31.2733);
		// The altitude should be 60° and the azimuth should be 180°.
		Assert.Equal(expected: 60.0, actual: alt, precision: 6);
		// The azimuth should be 180° (south).
		Assert.Equal(expected: 180.0, actual: az, precision: 6);
	}

	/// <summary>Verifies that azimuth and angles above 360° (and negative ones) are normalized.</summary>
	/// <remarks>This test checks that the NormalizeDegrees method correctly normalizes angles to the range [0°, 360°).</remarks>
	[Theory]
	[InlineData(370.0, 10.0)]
	[InlineData(720.0, 0.0)]
	[InlineData(-10.0, 350.0)]
	[InlineData(360.0, 0.0)]
	[InlineData(1085.5, 5.5)]
	public void AzimuthIsNormalized(double input, double expected)
	{
		// The NormalizeDegrees method should normalize the input angle to the expected angle in the range [0°, 360°).
		Assert.Equal(expected: expected, actual: CoordinateTransformationService.NormalizeDegrees(degrees: input), precision: 9);
	}

	/// <summary>Verifies that the horizontal azimuth is within [0°, 360°) for hour angles beyond 360°.</summary>
	/// <remarks>This test checks that the EquatorialToHorizontal method correctly normalizes the azimuth to the range [0°, 360°) even when the hour angle is beyond 360°.</remarks>
	[Fact]
	public void AzimuthFromLargeHourAngleIsInRange()
	{
		// For a large hour angle, the azimuth should be normalized to the range [0°, 360°).
		(double az1, double alt1) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 45.0, declinationDegrees: 20.0, latitudeDegrees: 50.0);
		// The azimuth should be in the range [0°, 360°) and the altitude should be the same as for the normalized hour angle.
		(double az2, double alt2) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 45.0 + 720.0, declinationDegrees: 20.0, latitudeDegrees: 50.0);
		// The azimuth should be in the range [0°, 360°) and the altitude should be the same as for the normalized hour angle.
		Assert.InRange(actual: az2, low: 0.0, high: 360.0 - 1e-12);
		// The azimuth and altitude should be equal for the normalized and unnormalized hour angles.
		Assert.Equal(expected: az1, actual: az2, precision: 9);
		// The altitude should be equal for the normalized and unnormalized hour angles.
		Assert.Equal(expected: alt1, actual: alt2, precision: 9);
		// The NormalizeHours method should normalize the input hours to the range [0, 24).
		Assert.Equal(expected: 0.0, actual: CoordinateTransformationService.NormalizeHours(hours: 24.0), precision: 12);
	}

	/// <summary>Verifies that an object below the horizon is never visible.</summary>
	/// <remarks>This test checks that the IsVisible method returns false for objects with negative altitude, regardless of other visibility criteria.</remarks>
	[Fact]
	public void BelowHorizonIsNotVisible()
	{
		// Create a lenient visibility criteria that allows any altitude and sun altitude, but requires a minimum moon separation of 0°.
		VisibilityCriteria lenient = new(MinimumAltitudeDegrees: -90.0, MaximumSunAltitudeDegrees: 90.0);
		// An object with negative altitude should not be visible, even if the sun is well below the horizon and the apparent magnitude is bright.
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: -5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: lenient));
		// An object with positive altitude should be visible if the sun is well below the horizon and the apparent magnitude is bright.
		Assert.True(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: lenient));
		// Create a strict visibility criteria that requires a minimum altitude of 10°, a maximum sun altitude of -18°, a faintest magnitude of 7.0, and a minimum moon separation of 30°.
		VisibilityCriteria strict = new(MinimumAltitudeDegrees: 10.0, MaximumSunAltitudeDegrees: -18.0, FaintestMagnitude: 7.0, MinimumMoonSeparationDegrees: 30.0);
		// An object with altitude below the minimum should not be visible.
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 6.0, moonSeparationDegrees: 90.0, criteria: strict));
		// An object with sun altitude above the maximum should not be visible.
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -10.0, apparentMagnitude: 6.0, moonSeparationDegrees: 90.0, criteria: strict));
		// An object with apparent magnitude fainter than the faintest should not be visible.
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: strict));
		// An object with moon separation below the minimum should not be visible.
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 6.0, moonSeparationDegrees: 10.0, criteria: strict));
		// An object with NaN apparent magnitude should be visible because the magnitude criterion is skipped.
		Assert.True(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: double.NaN, moonSeparationDegrees: 90.0, criteria: strict));
	}

	/// <summary>Verifies that calculated entries below the horizon are never flagged as visible.</summary>
	/// <remarks>This test calculates the ephemeris of Ceres over a 1-day period with 10-minute steps and checks that entries below the horizon are never flagged as visible, even if the refracted altitude is above the horizon.</remarks>
	[Fact]
	public void EphemerisBelowHorizonEntriesAreNotVisible()
	{
		// Create a time grid from 2025-06-01 00:00 UTC to 2025-06-02 00:00 UTC with 10-minute steps.
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		// The grid should contain 144 points (24 hours * 6 points per hour).
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 1), step: TimeSpan.FromMinutes(minutes: 10));
		// Calculate the ephemeris of Ceres for the time grid and observer at Greenwich with a visibility criteria that allows any altitude and sun altitude.
		IReadOnlyList<EphemerisEntry> result = service.Calculate(
			elements: TestData.Ceres(),
			times: grid,
			observer: TestData.Greenwich,
			criteria: new VisibilityCriteria(MinimumAltitudeDegrees: -90.0, MaximumSunAltitudeDegrees: 90.0),
			options: new EphemerisOptions(IncludePlanetaryPerturbations: false, ApplyRefraction: true),
			cancellationToken: TestContext.Current.CancellationToken);
		// Verify that there are entries below the horizon and that they are not flagged as visible.
		Assert.Contains(collection: result, filter: static e => !e.IsAboveHorizon);
		// Verify that there are entries with geometric altitude below 0° and refracted altitude above 0°.
		Assert.Contains(collection: result, filter: static e => e.GeometricAltitudeDegrees < 0.0 && e.AltitudeDegrees > 0.0);
		// Verify that all entries below the horizon are not visible and all entries above the horizon are visible.
		Assert.All(collection: result.Where(predicate: static e => !e.IsAboveHorizon), action: static e => Assert.False(condition: e.IsVisible));
		// Verify that all entries above the horizon are visible.
		Assert.All(collection: result.Where(predicate: static e => e.IsAboveHorizon), action: static e => Assert.True(condition: e.IsVisible));
	}

	/// <summary>Verifies that invalid MPCORB records are rejected with an error message.</summary>
	/// <remarks>This test checks that the TryParse method of MpcorbElementsParser returns false and provides an error message for invalid MPCORB records, including null, empty, and malformed lines.</remarks>
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("00001    3.34  0.15 K2555")]
	[InlineData("Header line of MPCORB.DAT ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")]
	public void InvalidMpcorbRecordIsRejected(string? line)
	{
		// The TryParse method should return false and provide an error message for invalid MPCORB records.
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: line, elements: out MinorPlanetOrbitalElements? elements, error: out string? error));
		// The elements should be null and the error message should not be null or whitespace.
		Assert.Null(@object: elements);
		// The error message should not be null or whitespace.
		Assert.False(condition: string.IsNullOrWhiteSpace(value: error));
	}

	/// <summary>Verifies that a record with an invalid eccentricity or packed epoch is rejected.</summary>
	/// <remarks>This test checks that the TryParse method of MpcorbElementsParser returns false for a record with an invalid eccentricity or packed epoch, and that the TryDecodePackedEpoch method returns false for an invalid packed epoch.</remarks>
	[Fact]
	public void InvalidMpcorbRecordInvalidValuesAreRejected()
	{
		// Create a record with an invalid eccentricity (1.5) and check that TryParse returns false.
		string badEccentricity = TestData.CeresRecord.Remove(startIndex: 70, count: 9).Insert(startIndex: 70, value: "1.5000000");
		// The TryParse method should return false for the record with an invalid eccentricity.
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: badEccentricity, elements: out _, error: out _));
		// Create a record with an invalid packed epoch (Z2555) and check that TryParse returns false.
		string badEpoch = TestData.CeresRecord.Remove(startIndex: 20, count: 5).Insert(startIndex: 20, value: "Z2555");
		// The TryParse method should return false for the record with an invalid packed epoch.
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: badEpoch, elements: out _, error: out _));
		// The TryDecodePackedEpoch method should return false for an invalid packed epoch (K25Z5).
		Assert.False(condition: MpcorbElementsParser.TryDecodePackedEpoch(packedEpoch: "K25Z5", julianDateTt: out _));
		// The TryDecodePackedEpoch method should return true for a valid packed epoch (K2555) and produce the expected Julian date.
		Assert.True(condition: MpcorbElementsParser.TryDecodePackedEpoch(packedEpoch: "K2555", julianDateTt: out double jd));
		// The expected Julian date for the packed epoch K2555 is 2460800.5.
		Assert.Equal(expected: 2460800.5, actual: jd, precision: 9);
	}

	/// <summary>Verifies the conversion of local times to UTC during standard and daylight saving time.</summary>
	/// <remarks>This test checks that the LocalToUtc method correctly converts local times to UTC for a time zone with standard and daylight saving time, using Central European Time (CET/CEST) as an example.</remarks>
	[Fact]
	public void TimeZoneStandardAndDaylightSavingTime()
	{
		// Create a time zone for Central European Time (CET/CEST) with standard time offset of +1 hour and daylight saving time offset of +2 hours.
		TimeZoneInfo cet = CreateCentralEuropeanTime();
		// 2025-01-15 22:00 CET is 2025-01-15 21:00 UTC (standard time)
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 1, day: 15, hour: 21, minute: 0, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 1, day: 15, hour: 22, minute: 0, second: 0), timeZone: cet));
		// 2025-07-15 22:00 CEST is 2025-07-15 20:00 UTC (daylight saving time)
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 7, day: 15, hour: 20, minute: 0, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 7, day: 15, hour: 22, minute: 0, second: 0), timeZone: cet));
	}

	/// <summary>Verifies the handling of the skipped (spring) and repeated (autumn) local hour.</summary>
	/// <remarks>This test checks that the LocalToUtc method correctly handles the skipped hour during the spring transition to daylight saving time and the repeated hour during the autumn transition back to standard time, using Central European Time (CET/CEST) as an example.</remarks>
	[Fact]
	public void TimeZoneDaylightSavingTransitions()
	{
		// Create a time zone for Central European Time (CET/CEST) with standard time offset of +1 hour and daylight saving time offset of +2 hours.
		TimeZoneInfo cet = CreateCentralEuropeanTime();
		// 2025-03-30 02:30 does not exist in CET/CEST; it is interpreted with the offset before the transition (+1 h)
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 3, day: 30, hour: 1, minute: 30, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 3, day: 30, hour: 2, minute: 30, second: 0), timeZone: cet));
		// 2025-10-26 02:30 occurs twice; standard time (+1 h) is used
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 10, day: 26, hour: 1, minute: 30, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 10, day: 26, hour: 2, minute: 30, second: 0), timeZone: cet));
	}

	/// <summary>Verifies an ambiguous time with a negative daylight delta uses standard time.</summary>
	/// <remarks>This test checks that the LocalToUtc method correctly uses standard time for an ambiguous time with a negative daylight delta.</remarks>
	[Fact]
	public void TimeZoneNegativeDaylightDeltaUsesStandardTime()
	{
		// Create a time zone with a negative daylight delta.
		TimeZoneInfo zone = CreateNegativeDaylightDeltaTimeZone();
		// 2025-03-30 01:30 is ambiguous; standard time (offset 0) is used
		DateTime localTime = new(year: 2025, month: 3, day: 30, hour: 1, minute: 30, second: 0);
		// The expected UTC time is 2025-03-30 00:30:00Z (standard time).
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 3, day: 30, hour: 0, minute: 30, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: localTime, timeZone: zone));
	}

	/// <summary>Verifies that the ephemeris does not depend on the offset of the input time.</summary>
	/// <remarks>This test checks that the Calculate method produces the same ephemeris entries for the same UTC time and a local time with a different offset, and that the resulting entries have a zero offset.</remarks>
	[Fact]
	public void TimeZoneOffsetDoesNotChangeResult()
	{
		// Create a UTC time and a local time with a +2 hour offset.
		DateTimeOffset utc = new(year: 2025, month: 7, day: 1, hour: 22, minute: 0, second: 0, offset: TimeSpan.Zero);
		// Create a local time with a +2 hour offset.
		DateTimeOffset local = utc.ToOffset(offset: TimeSpan.FromHours(hours: 2));
		// Calculate the ephemeris of Ceres for both times and compare the results.
		IReadOnlyList<EphemerisEntry> result = service.Calculate(elements: TestData.Ceres(), times: [utc, local], observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken);
		// The right ascension and altitude should be equal for both entries, and the time offset of the second entry should be zero (UTC).
		Assert.Equal(expected: result[index: 0].RightAscensionHours, actual: result[index: 1].RightAscensionHours, precision: 12);
		// The altitude should be equal for both entries, and the time offset of the second entry should be zero (UTC).
		Assert.Equal(expected: result[index: 0].AltitudeDegrees, actual: result[index: 1].AltitudeDegrees, precision: 12);
		// The time offset of the second entry should be zero (UTC).
		Assert.Equal(expected: TimeSpan.Zero, actual: result[index: 1].Time.Offset);
	}

	/// <summary>Verifies invalid time grids.</summary>
	/// <remarks>This test checks that the CreateTimeGrid method throws an ArgumentOutOfRangeException for invalid inputs, such as a zero step size, an end time before the start time, or a step size that is too small for a long duration.</remarks>
	[Fact]
	public void TimeGridInvalidInputThrows()
	{
		// Create a start time for the time grid.
		DateTimeOffset start = new(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		// The CreateTimeGrid method should throw an ArgumentOutOfRangeException for a zero step size.
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 1), step: TimeSpan.Zero));
		// The CreateTimeGrid method should throw an ArgumentOutOfRangeException for an end time before the start time.
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: -1), step: TimeSpan.FromHours(hours: 1)));
		// The CreateTimeGrid method should throw an ArgumentOutOfRangeException for a step size that is too small for a long duration (100 years with 1-minute steps).
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddYears(years: 100), step: TimeSpan.FromMinutes(minutes: 1)));
	}

	/// <summary>Verifies that observer longitude follows the documented range.</summary>
	/// <remarks>This test checks that the ObserverLocation.Validate method accepts longitudes within the range [−180°, +180°] and rejects longitudes outside this range.</remarks>
	[Theory]
	[InlineData(-180.0)]
	[InlineData(180.0)]
	public void ObserverLongitudeInRangeIsAccepted(double longitude)
	{
		// The ObserverLocation.Validate method should accept longitudes within the range [−180°, +180°].
		new ObserverLocation(LatitudeDegrees: 0.0, LongitudeDegrees: longitude).Validate();
	}

	/// <summary>Verifies that observer longitude outside [−180°, +180°] is rejected.</summary>
	/// <remarks>This test checks that the ObserverLocation.Validate method throws an ArgumentOutOfRangeException for longitudes outside the range [−180°, +180°].</remarks>
	[Theory]
	[InlineData(-180.01)]
	[InlineData(180.01)]
	[InlineData(360.0)]
	public void ObserverLongitudeOutOfRangeThrows(double longitude)
	{
		// The ObserverLocation.Validate method should throw an ArgumentOutOfRangeException for longitudes outside the range [−180°, +180°].
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => new ObserverLocation(LatitudeDegrees: 0.0, LongitudeDegrees: longitude).Validate());
	}

	/// <summary>Verifies that the CSV export is culture-independent.</summary>
	/// <remarks>This test checks that the ToCsv method produces the same output regardless of the current culture, by setting the culture to "de-DE" and verifying that the CSV fields use a dot as the decimal separator.</remarks>
	[Fact]
	public void CsvExportIsCultureInvariant()
	{
		// Save the current culture and set it to "de-DE" for the test.
		CultureInfo previous = CultureInfo.CurrentCulture;
		// Create an ephemeris entry with specific values and export it to CSV, then verify that the output uses a dot as the decimal separator and is culture-independent.
		try
		{
			// Set the current culture to "de-DE" (German) for the test.
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name: "de-DE");
			// Create an ephemeris entry with specific values, including a time with a +1 hour offset.
			EphemerisEntry entry = new(Time: new DateTimeOffset(year: 2025, month: 1, day: 1, hour: 23, minute: 0, second: 0, offset: TimeSpan.FromHours(hours: 1)), RightAscensionHours: 12.5, DeclinationDegrees: -10.25, AzimuthDegrees: 180.5, AltitudeDegrees: -1.5, DistanceAu: 1.25, ApparentMagnitude: double.NaN, IsVisible: false, AstrometricRightAscensionHours: 7.5, AstrometricDeclinationDegrees: 23.25);
			// Export the entry to CSV and split the output into lines and fields for verification.
			string csv = EphemerisExportService.ToCsv(entries: [entry], designation: "(1) Ceres", observer: TestData.Greenwich);
			// Split the CSV output into lines, removing empty entries.
			string[] lines = csv.Split(separator: Environment.NewLine, options: StringSplitOptions.RemoveEmptyEntries);
			// Split the fourth line (the data line) into fields using a comma as the separator.
			string[] fields = lines[3].Split(separator: ',');
			// Verify that the first line contains the object designation, the third line contains the CSV header, and the fourth line starts and ends with the expected values, including the right ascension, declination, and visibility flag.
			Assert.Equal(expected: "# Object: (1) Ceres", actual: lines[0]);
			// The second line is empty, so we skip it.
			Assert.Equal(expected: EphemerisExportService.CsvHeader, actual: lines[2]);
			// The fourth line should start with the expected values for time, right ascension, declination, and other fields.
			Assert.StartsWith(expectedStartString: "2025-01-01T22:00:00Z,12.500000,12h 30m 00.00s,-10.25000,", actualString: lines[3], StringComparison.Ordinal);
			// The fourth line should end with the visibility flag "no" and the expected number of fields.
			Assert.EndsWith(expectedEndString: ",,,,,,no", actualString: lines[3], StringComparison.Ordinal);
			// Verify that the specific fields in the fourth line match the expected values, including the astrometric right ascension and declination.
			Assert.Equal(expected: "7.500000", actual: fields[5]);
			// The astrometric right ascension should be formatted as "07h 30m 00.00s".
			Assert.Equal(expected: "07h 30m 00.00s", actual: fields[6]);
			// The astrometric declination should be formatted as "+23.25000" and "+23° 15′ 00.0″".
			Assert.Equal(expected: "23.25000", actual: fields[7]);
			// The astrometric declination should be formatted as "+23° 15′ 00.0″".
			Assert.Equal(expected: "+23° 15′ 00.0″", actual: fields[8]);
			// The visibility flag should be "no" for this entry.
			Assert.Equal(expected: 19, actual: fields.Length);
		}
		// Restore the previous culture after the test.
		finally
		{
			// Restore the previous culture to avoid affecting other tests or code.
			CultureInfo.CurrentCulture = previous;
		}
	}

	/// <summary>Creates a Central European time zone with EU daylight saving time rules.</summary>
	/// <returns>The time zone.</returns>
	/// <remarks>This method creates a custom time zone for Central European Time (CET) with standard time offset of +1 hour and daylight saving time offset of +2 hours, following the EU rules for daylight saving time transitions.</remarks>
	private static TimeZoneInfo CreateCentralEuropeanTime()
	{
		// Create the transition times for the start and end of daylight saving time according to EU rules (last Sunday in March and last Sunday in October).
		TimeZoneInfo.TransitionTime start = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 2, minute: 0, second: 0), month: 3, week: 5, dayOfWeek: DayOfWeek.Sunday);
		// Create the transition time for the end of daylight saving time (last Sunday in October).
		TimeZoneInfo.TransitionTime end = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 3, minute: 0, second: 0), month: 10, week: 5, dayOfWeek: DayOfWeek.Sunday);
		// Create an adjustment rule for the daylight saving time transition with a +1 hour delta.
		TimeZoneInfo.AdjustmentRule rule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart: DateTime.MinValue.Date, dateEnd: DateTime.MaxValue.Date, daylightDelta: TimeSpan.FromHours(hours: 1), daylightTransitionStart: start, daylightTransitionEnd: end);
		// Create and return the custom time zone for Central European Time (CET) with the specified adjustment rule.
		return TimeZoneInfo.CreateCustomTimeZone(id: "Test CET", baseUtcOffset: TimeSpan.FromHours(hours: 1), displayName: "Test CET", standardDisplayName: "CET", daylightDisplayName: "CEST", adjustmentRules: [rule]);
	}

	/// <summary>Creates a time zone with a negative daylight-saving offset change.</summary>
	/// <returns>The custom time zone.</returns>
	/// <remarks>This method creates a custom time zone with a base UTC offset of +1 hour and a negative daylight-saving offset change of -1 hour, resulting in a standard time offset of +1 hour and a daylight time offset of 0 hours.</remarks>
	private static TimeZoneInfo CreateNegativeDaylightDeltaTimeZone()
	{
		// Create the transition times for the start and end of daylight saving time with a negative daylight delta.
		TimeZoneInfo.TransitionTime start = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 2, minute: 0, second: 0), month: 3, week: 5, dayOfWeek: DayOfWeek.Sunday);
		// Create the transition time for the end of daylight saving time (last Sunday in October).
		TimeZoneInfo.TransitionTime end = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 3, minute: 0, second: 0), month: 10, week: 5, dayOfWeek: DayOfWeek.Sunday);
		// Create an adjustment rule for the daylight saving time transition with a -1 hour delta.
		TimeZoneInfo.AdjustmentRule rule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart: DateTime.MinValue.Date, dateEnd: DateTime.MaxValue.Date, daylightDelta: TimeSpan.FromHours(hours: -1), daylightTransitionStart: start, daylightTransitionEnd: end);
		// Create and return the custom time zone with a base UTC offset of +1 hour and the specified adjustment rule.
		return TimeZoneInfo.CreateCustomTimeZone(id: "Test Negative DST", baseUtcOffset: TimeSpan.FromHours(hours: 1), displayName: "Test Negative DST", standardDisplayName: "STD", daylightDisplayName: "DST", adjustmentRules: [rule]);
	}

	/// <summary>Verifies that the astrometric J2000 place of Ceres agrees with the MPC ephemeris service (issue #1234).</summary>
	/// <remarks>MPC, 2026-10-08 00:00 UTC: RA = 07h 16m 26.4s, Dec = +23° 19′ 35″ (J2000). The apparent place of date differs mainly by precession.</remarks>
	[Fact]
	public void CeresAstrometricJ2000MatchesMinorPlanetCenter()
	{
		// Create a time for 2026-10-08 00:00 UTC.
		DateTimeOffset time = new(year: 2026, month: 10, day: 8, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		// Calculate the ephemeris entry for Ceres at the specified time and observer location.
		EphemerisEntry entry = service.Calculate(elements: TestData.Ceres(), times: [time], observer: TestData.Greenwich, cancellationToken: TestContext.Current.CancellationToken)[index: 0];
		// The expected astrometric right ascension and declination in J2000 coordinates are 07h 16m 26.4s and +23° 19′ 35″, respectively.
		double expectedRa = 7.0 + (16.0 / 60.0) + (26.4 / 3600.0);
		double expectedDec = 23.0 + (19.0 / 60.0) + (35.0 / 3600.0);
		// The astrometric right ascension and declination should be within 1 arcminute of the expected values.
		double cosDec = Math.Cos(d: expectedDec * Math.PI / 180.0);
		// The difference in right ascension is converted to arcseconds by multiplying by 15 (degrees per hour) and then by 3600 (arcseconds per degree), and adjusted for the cosine of the declination.
		Assert.InRange(actual: Math.Abs(value: entry.AstrometricRightAscensionHours - expectedRa) * 15.0 * cosDec * 3600.0, low: 0.0, high: 60.0);
		// The difference in declination is converted to arcseconds by multiplying by 3600 (arcseconds per degree).
		Assert.InRange(actual: Math.Abs(value: entry.AstrometricDeclinationDegrees - expectedDec) * 3600.0, low: 0.0, high: 60.0);
		// The difference in right ascension between the apparent and astrometric positions should be between 90 and 105 arcseconds.
		Assert.InRange(actual: (entry.RightAscensionHours - entry.AstrometricRightAscensionHours) * 3600.0, low: 90.0, high: 105.0);
		// The difference in declination between the apparent and astrometric positions should be between -3.5 and -2.5 arcminutes.
		Assert.InRange(actual: (entry.DeclinationDegrees - entry.AstrometricDeclinationDegrees) * 60.0, low: -3.5, high: -2.5);
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
