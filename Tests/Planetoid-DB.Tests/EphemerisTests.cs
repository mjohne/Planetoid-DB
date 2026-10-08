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

using System.Globalization;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for <see cref="EphemerisService"/>, the coordinate transformations, visibility and export.</summary>
public sealed class EphemerisTests
{
	/// <summary>The service with the analytical planetary ephemeris.</summary>
	private readonly EphemerisService service = new(planetaryEphemeris: new AnalyticalPlanetaryEphemeris());

	/// <summary>Verifies that a time grid crossing midnight and the turn of the year is continuous and in UTC.</summary>
	[Fact]
	public void TimeGrid_CrossesDateChange()
	{
		DateTimeOffset start = new(year: 2025, month: 12, day: 31, hour: 22, minute: 0, second: 0, offset: TimeSpan.Zero);
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddHours(hours: 4), step: TimeSpan.FromHours(hours: 1));
		Assert.Equal(expected: 5, actual: grid.Count);
		Assert.Equal(expected: new DateTimeOffset(year: 2026, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero), actual: grid[2]);
		Assert.All(collection: grid, action: static t => Assert.Equal(expected: TimeSpan.Zero, actual: t.Offset));
	}

	/// <summary>Verifies the Julian date across the date change (JD starts at noon).</summary>
	[Fact]
	public void JulianDate_DateChange()
	{
		Assert.Equal(expected: 2451545.0, actual: TimeScales.ToJulianDate(dateTime: new DateTime(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, kind: DateTimeKind.Utc)), precision: 9);
		Assert.Equal(expected: 2461041.5, actual: TimeScales.ToJulianDate(dateTime: new DateTime(year: 2026, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)), precision: 9);
	}

	/// <summary>Verifies that the ephemeris across midnight is continuous.</summary>
	[Fact]
	public void Ephemeris_IsContinuousAcrossMidnight()
	{
		DateTimeOffset start = new(year: 2025, month: 12, day: 31, hour: 23, minute: 0, second: 0, offset: TimeSpan.Zero);
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddHours(hours: 2), step: TimeSpan.FromHours(hours: 1));
		IReadOnlyList<EphemerisEntry> result = service.Calculate(elements: TestData.Ceres(), times: grid, observer: TestData.Greenwich);
		Assert.Equal(expected: 3, actual: result.Count);
		for (int i = 1; i < result.Count; i++)
		{
			Assert.InRange(actual: Math.Abs(value: result[i].DeclinationDegrees - result[i - 1].DeclinationDegrees), low: 0.0, high: 0.1);
			Assert.InRange(actual: Math.Abs(value: result[i].DistanceAu - result[i - 1].DistanceAu), low: 0.0, high: 0.01);
		}
	}

	/// <summary>Verifies the plausibility of the ephemeris of Ceres.</summary>
	[Fact]
	public async Task Ephemeris_Ceres_IsPlausible()
	{
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 60), step: TimeSpan.FromDays(days: 10));
		IReadOnlyList<EphemerisEntry> result = await service.CalculateAsync(elements: TestData.Ceres(), times: grid, observer: TestData.Greenwich);
		Assert.All(collection: result, action: static e =>
		{
			Assert.InRange(actual: e.RightAscensionHours, low: 0.0, high: 24.0);
			Assert.InRange(actual: e.DeclinationDegrees, low: -40.0, high: 40.0);
			Assert.InRange(actual: e.AzimuthDegrees, low: 0.0, high: 360.0);
			Assert.InRange(actual: e.DistanceAu, low: 1.5, high: 4.0);
			Assert.InRange(actual: e.HeliocentricDistanceAu, low: 2.5, high: 3.0);
			Assert.InRange(actual: e.ApparentMagnitude, low: 6.0, high: 10.0);
			Assert.Equal(expected: TimeSpan.Zero, actual: e.Time.Offset);
		});
	}

	/// <summary>Verifies that a pre-canceled token cancels the calculation.</summary>
	[Fact]
	public async Task Ephemeris_Cancellation()
	{
		using CancellationTokenSource cts = new();
		await cts.CancelAsync();
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		_ = await Assert.ThrowsAnyAsync<OperationCanceledException>(testCode: () => service.CalculateAsync(elements: TestData.Ceres(), times: [start], observer: TestData.Greenwich, cancellationToken: cts.Token));
	}

	/// <summary>Verifies negative declinations in conversion and formatting.</summary>
	[Fact]
	public void NegativeDeclination()
	{
		(double ra, double dec) = CoordinateTransformationService.ToRightAscensionDeclination(vector: new Vector3d(X: 0.0, Y: Math.Cos(d: -0.5), Z: Math.Sin(a: -0.5)));
		Assert.Equal(expected: 6.0, actual: ra, precision: 9);
		Assert.Equal(expected: -0.5 * 180.0 / Math.PI, actual: dec, precision: 9);
		Assert.Equal(expected: "-00° 30′ 00.0″", actual: CoordinateTransformationService.FormatDeclination(degrees: -0.5));
		Assert.Equal(expected: "-23° 26′ 21.4″", actual: CoordinateTransformationService.FormatDeclination(degrees: -23.439278));
		Assert.Equal(expected: "+00° 00′ 00.0″", actual: CoordinateTransformationService.FormatDeclination(degrees: 0.0));
	}

	/// <summary>Verifies the ephemeris for a southern observer and object below the celestial equator.</summary>
	[Fact]
	public void NegativeDeclination_SouthernObserver()
	{
		(double az, double alt) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 0.0, declinationDegrees: -61.2733, latitudeDegrees: -31.2733);
		Assert.Equal(expected: 60.0, actual: alt, precision: 6);
		Assert.Equal(expected: 180.0, actual: az, precision: 6);
	}

	/// <summary>Verifies that azimuth and angles above 360° (and negative ones) are normalized.</summary>
	[Theory]
	[InlineData(370.0, 10.0)]
	[InlineData(720.0, 0.0)]
	[InlineData(-10.0, 350.0)]
	[InlineData(360.0, 0.0)]
	[InlineData(1085.5, 5.5)]
	public void Azimuth_IsNormalized(double input, double expected)
	{
		Assert.Equal(expected: expected, actual: CoordinateTransformationService.NormalizeDegrees(degrees: input), precision: 9);
	}

	/// <summary>Verifies that the horizontal azimuth is within [0°, 360°) for hour angles beyond 360°.</summary>
	[Fact]
	public void Azimuth_FromLargeHourAngle_IsInRange()
	{
		(double az1, double alt1) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 45.0, declinationDegrees: 20.0, latitudeDegrees: 50.0);
		(double az2, double alt2) = CoordinateTransformationService.EquatorialToHorizontal(hourAngleDegrees: 45.0 + 720.0, declinationDegrees: 20.0, latitudeDegrees: 50.0);
		Assert.InRange(actual: az2, low: 0.0, high: 360.0 - 1e-12);
		Assert.Equal(expected: az1, actual: az2, precision: 9);
		Assert.Equal(expected: alt1, actual: alt2, precision: 9);
		Assert.Equal(expected: 0.0, actual: CoordinateTransformationService.NormalizeHours(hours: 24.0), precision: 12);
	}

	/// <summary>Verifies that an object below the horizon is never visible.</summary>
	[Fact]
	public void BelowHorizon_IsNotVisible()
	{
		VisibilityCriteria lenient = new(MinimumAltitudeDegrees: -90.0, MaximumSunAltitudeDegrees: 90.0);
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: -5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: lenient));
		Assert.True(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: lenient));
		VisibilityCriteria strict = new(MinimumAltitudeDegrees: 10.0, MaximumSunAltitudeDegrees: -18.0, FaintestMagnitude: 7.0, MinimumMoonSeparationDegrees: 30.0);
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 5.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 6.0, moonSeparationDegrees: 90.0, criteria: strict));
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -10.0, apparentMagnitude: 6.0, moonSeparationDegrees: 90.0, criteria: strict));
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 8.0, moonSeparationDegrees: 90.0, criteria: strict));
		Assert.False(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: 6.0, moonSeparationDegrees: 10.0, criteria: strict));
		Assert.True(condition: VisibilityCalculator.IsVisible(altitudeDegrees: 20.0, sunAltitudeDegrees: -30.0, apparentMagnitude: double.NaN, moonSeparationDegrees: 90.0, criteria: strict));
	}

	/// <summary>Verifies that calculated entries below the horizon are never flagged as visible.</summary>
	[Fact]
	public void Ephemeris_BelowHorizonEntries_AreNotVisible()
	{
		DateTimeOffset start = new(year: 2025, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		IReadOnlyList<DateTimeOffset> grid = EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 1), step: TimeSpan.FromHours(hours: 1));
		IReadOnlyList<EphemerisEntry> result = service.Calculate(elements: TestData.Ceres(), times: grid, observer: TestData.Greenwich, criteria: new VisibilityCriteria(MinimumAltitudeDegrees: -90.0, MaximumSunAltitudeDegrees: 90.0));
		Assert.Contains(collection: result, filter: static e => !e.IsAboveHorizon);
		Assert.All(collection: result.Where(predicate: static e => !e.IsAboveHorizon), action: static e => Assert.False(condition: e.IsVisible));
		Assert.All(collection: result.Where(predicate: static e => e.IsAboveHorizon), action: static e => Assert.True(condition: e.IsVisible));
	}

	/// <summary>Verifies that invalid MPCORB records are rejected with an error message.</summary>
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("00001    3.34  0.15 K2555")]
	[InlineData("Header line of MPCORB.DAT ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")]
	public void InvalidMpcorbRecord_IsRejected(string? line)
	{
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: line, elements: out MinorPlanetOrbitalElements? elements, error: out string? error));
		Assert.Null(@object: elements);
		Assert.False(condition: string.IsNullOrWhiteSpace(value: error));
	}

	/// <summary>Verifies that a record with an invalid eccentricity or packed epoch is rejected.</summary>
	[Fact]
	public void InvalidMpcorbRecord_InvalidValues_AreRejected()
	{
		string badEccentricity = TestData.CeresRecord.Remove(startIndex: 70, count: 9).Insert(startIndex: 70, value: "1.5000000");
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: badEccentricity, elements: out _, error: out _));
		string badEpoch = TestData.CeresRecord.Remove(startIndex: 20, count: 5).Insert(startIndex: 20, value: "Z2555");
		Assert.False(condition: MpcorbElementsParser.TryParse(rawLine: badEpoch, elements: out _, error: out _));
		Assert.False(condition: MpcorbElementsParser.TryDecodePackedEpoch(packedEpoch: "K25Z5", julianDateTt: out _));
		Assert.True(condition: MpcorbElementsParser.TryDecodePackedEpoch(packedEpoch: "K2555", julianDateTt: out double jd));
		Assert.Equal(expected: 2460800.5, actual: jd, precision: 9);
	}

	/// <summary>Verifies the conversion of local times to UTC during standard and daylight saving time.</summary>
	[Fact]
	public void TimeZone_StandardAndDaylightSavingTime()
	{
		TimeZoneInfo cet = CreateCentralEuropeanTime();
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 1, day: 15, hour: 21, minute: 0, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 1, day: 15, hour: 22, minute: 0, second: 0), timeZone: cet));
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 7, day: 15, hour: 20, minute: 0, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 7, day: 15, hour: 22, minute: 0, second: 0), timeZone: cet));
	}

	/// <summary>Verifies the handling of the skipped (spring) and repeated (autumn) local hour.</summary>
	[Fact]
	public void TimeZone_DaylightSavingTransitions()
	{
		TimeZoneInfo cet = CreateCentralEuropeanTime();
		// 2025-03-30 02:30 does not exist in CET/CEST; it is interpreted with the offset before the transition (+1 h)
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 3, day: 30, hour: 1, minute: 30, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 3, day: 30, hour: 2, minute: 30, second: 0), timeZone: cet));
		// 2025-10-26 02:30 occurs twice; standard time (+1 h) is used
		Assert.Equal(expected: new DateTimeOffset(year: 2025, month: 10, day: 26, hour: 1, minute: 30, second: 0, offset: TimeSpan.Zero), actual: EphemerisService.LocalToUtc(localTime: new DateTime(year: 2025, month: 10, day: 26, hour: 2, minute: 30, second: 0), timeZone: cet));
	}

	/// <summary>Verifies that the ephemeris does not depend on the offset of the input time.</summary>
	[Fact]
	public void TimeZone_OffsetDoesNotChangeResult()
	{
		DateTimeOffset utc = new(year: 2025, month: 7, day: 1, hour: 22, minute: 0, second: 0, offset: TimeSpan.Zero);
		DateTimeOffset local = utc.ToOffset(offset: TimeSpan.FromHours(hours: 2));
		IReadOnlyList<EphemerisEntry> result = service.Calculate(elements: TestData.Ceres(), times: [utc, local], observer: TestData.Greenwich);
		Assert.Equal(expected: result[0].RightAscensionHours, actual: result[1].RightAscensionHours, precision: 12);
		Assert.Equal(expected: result[0].AltitudeDegrees, actual: result[1].AltitudeDegrees, precision: 12);
		Assert.Equal(expected: TimeSpan.Zero, actual: result[1].Time.Offset);
	}

	/// <summary>Verifies invalid time grids.</summary>
	[Fact]
	public void TimeGrid_InvalidInput_Throws()
	{
		DateTimeOffset start = new(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: 1), step: TimeSpan.Zero));
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddDays(days: -1), step: TimeSpan.FromHours(hours: 1)));
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => EphemerisService.CreateTimeGrid(start: start, end: start.AddYears(years: 100), step: TimeSpan.FromMinutes(minutes: 1)));
	}

	/// <summary>Verifies that the CSV export is culture-independent.</summary>
	[Fact]
	public void CsvExport_IsCultureInvariant()
	{
		CultureInfo previous = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name: "de-DE");
			EphemerisEntry entry = new(Time: new DateTimeOffset(year: 2025, month: 1, day: 1, hour: 23, minute: 0, second: 0, offset: TimeSpan.FromHours(hours: 1)), RightAscensionHours: 12.5, DeclinationDegrees: -10.25, AzimuthDegrees: 180.5, AltitudeDegrees: -1.5, DistanceAu: 1.25, ApparentMagnitude: double.NaN, IsVisible: false);
			string csv = EphemerisExportService.ToCsv(entries: [entry], designation: "(1) Ceres", observer: TestData.Greenwich);
			string[] lines = csv.Split(separator: Environment.NewLine, options: StringSplitOptions.RemoveEmptyEntries);
			Assert.Equal(expected: "# Object: (1) Ceres", actual: lines[0]);
			Assert.Equal(expected: EphemerisExportService.CsvHeader, actual: lines[2]);
			Assert.StartsWith(expectedStartString: "2025-01-01T22:00:00Z,12.500000,12h 30m 00.00s,-10.25000,", actualString: lines[3]);
			Assert.EndsWith(expectedEndString: ",,,,,,no", actualString: lines[3]);
			Assert.Equal(expected: 15, actual: lines[3].Split(separator: ',').Length);
		}
		finally
		{
			CultureInfo.CurrentCulture = previous;
		}
	}

	/// <summary>Creates a Central European time zone with EU daylight saving time rules.</summary>
	/// <returns>The time zone.</returns>
	private static TimeZoneInfo CreateCentralEuropeanTime()
	{
		TimeZoneInfo.TransitionTime start = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 2, minute: 0, second: 0), month: 3, week: 5, dayOfWeek: DayOfWeek.Sunday);
		TimeZoneInfo.TransitionTime end = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay: new DateTime(year: 1, month: 1, day: 1, hour: 3, minute: 0, second: 0), month: 10, week: 5, dayOfWeek: DayOfWeek.Sunday);
		TimeZoneInfo.AdjustmentRule rule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart: DateTime.MinValue.Date, dateEnd: DateTime.MaxValue.Date, daylightDelta: TimeSpan.FromHours(hours: 1), daylightTransitionStart: start, daylightTransitionEnd: end);
		return TimeZoneInfo.CreateCustomTimeZone(id: "Test CET", baseUtcOffset: TimeSpan.FromHours(hours: 1), displayName: "Test CET", standardDisplayName: "CET", daylightDisplayName: "CEST", adjustmentRules: [rule]);
	}
}
