/*
 * File:        EphemerisServiceTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests the ephemeris pipeline, orbit propagation, visibility and export.
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

using Planetoid_DB.Services;

namespace Planetoid_DB.Tests;

/// <summary>Tests the ephemeris pipeline, orbit propagation, visibility and export.</summary>
public sealed class EphemerisServiceTests
{
	/// <summary>Calculates one entry for Ceres.</summary>
	/// <param name="times">The UTC instants.</param>
	/// <param name="observer">The observer, or Greenwich.</param>
	/// <param name="criteria">The criteria, or the defaults.</param>
	/// <param name="model">The propagation model.</param>
	/// <returns>The entries.</returns>
	private static IReadOnlyList<EphemerisEntry> Calculate(IReadOnlyList<DateTimeOffset> times, ObserverLocation? observer = null, VisibilityCriteria? criteria = null, PropagationModel model = PropagationModel.TwoBody)
		=> new EphemerisService().Calculate(request: new EphemerisRequest(Elements: TestData.Ceres, Times: times, Observer: observer ?? TestData.Greenwich, Criteria: criteria ?? new VisibilityCriteria(), Model: model));

	/// <summary>The Sun culminates at about 90° − φ + 23.44° on the June solstice and is below the horizon at midnight.</summary>
	[Fact]
	public void Calculate_SunAltitudeAtSolstice()
	{
		IReadOnlyList<EphemerisEntry> entries = Calculate(times: [TestData.Utc(text: "2025-06-21T12:02:00Z"), TestData.Utc(text: "2025-06-21T00:00:00Z")]);
		Assert.InRange(actual: entries[1].SunAltitudeDegrees, low: 61.5, high: 62.5);
		Assert.InRange(actual: entries[0].SunAltitudeDegrees, low: -16.5, high: -14.0);
	}

	/// <summary>Entries are sorted, in UTC, and all values are in their documented ranges.</summary>
	[Fact]
	public void Calculate_EntriesAreSortedUtcAndInRange()
	{
		DateTimeOffset local = new(year: 2025, month: 9, day: 1, hour: 2, minute: 0, second: 0, offset: TimeSpan.FromHours(value: 2));
		IReadOnlyList<EphemerisEntry> entries = Calculate(times: [TestData.Utc(text: "2025-09-01T03:00:00Z"), local, TestData.Utc(text: "2025-09-01T01:00:00Z")]);
		Assert.Equal(expected: [TestData.Utc(text: "2025-09-01T00:00:00Z"), TestData.Utc(text: "2025-09-01T01:00:00Z"), TestData.Utc(text: "2025-09-01T03:00:00Z")], actual: entries.Select(selector: static e => e.Time));
		Assert.All(collection: entries, action: static e =>
		{
			Assert.Equal(expected: TimeSpan.Zero, actual: e.Time.Offset);
			Assert.InRange(actual: e.RightAscensionHours, low: 0.0, high: 23.99999999);
			Assert.InRange(actual: e.DeclinationDegrees, low: -90.0, high: 90.0);
			Assert.InRange(actual: e.AzimuthDegrees, low: 0.0, high: 359.99999999);
			Assert.InRange(actual: e.AltitudeDegrees, low: -90.0, high: 90.0);
			Assert.InRange(actual: e.HeliocentricDistanceAu, low: 2.5, high: 3.0);
			Assert.InRange(actual: e.DistanceAu, low: 1.5, high: 4.0);
			Assert.InRange(actual: e.ApparentMagnitude, low: 6.0, high: 10.0);
		});
	}

	/// <summary>The position changes smoothly across midnight (date change).</summary>
	[Fact]
	public void Calculate_AcrossMidnight_IsContinuous()
	{
		IReadOnlyList<DateTimeOffset> times = EphemerisRequest.CreateTimeSeries(start: TestData.Utc(text: "2025-12-31T22:00:00Z"), end: TestData.Utc(text: "2026-01-01T02:00:00Z"), step: TimeSpan.FromMinutes(value: 10));
		IReadOnlyList<EphemerisEntry> entries = Calculate(times: times);
		for (int i = 1; i < entries.Count; i++)
		{
			double deltaRa = Math.Abs(value: entries[i].RightAscensionHours - entries[i - 1].RightAscensionHours);
			Assert.True(condition: Math.Min(val1: deltaRa, val2: 24.0 - deltaRa) < 0.01, userMessage: $"RA jump at {entries[i].Time:O}");
			Assert.True(condition: Math.Abs(value: entries[i].DeclinationDegrees - entries[i - 1].DeclinationDegrees) < 0.01, userMessage: $"Dec jump at {entries[i].Time:O}");
			Assert.True(condition: Math.Abs(value: entries[i].AltitudeDegrees - entries[i - 1].AltitudeDegrees) < 3.0, userMessage: $"Altitude jump at {entries[i].Time:O}");
		}
	}

	/// <summary>Over a full day the object is below the horizon at some time and is then never reported as visible.</summary>
	[Fact]
	public void Calculate_BelowHorizon_IsNeverVisible()
	{
		IReadOnlyList<DateTimeOffset> times = EphemerisRequest.CreateTimeSeries(start: TestData.Utc(text: "2025-10-02T12:00:00Z"), end: TestData.Utc(text: "2025-10-03T12:00:00Z"), step: TimeSpan.FromMinutes(value: 20));
		VisibilityCriteria criteria = new(MinimumObjectAltitudeDegrees: 0.0, MaximumSunAltitudeDegrees: 90.0);
		IReadOnlyList<EphemerisEntry> entries = Calculate(times: times, criteria: criteria);
		Assert.Contains(collection: entries, filter: static e => e.AltitudeDegrees < 0.0);
		Assert.Contains(collection: entries, filter: static e => e.AltitudeDegrees > 0.0);
		Assert.All(collection: entries, action: e => Assert.Equal(expected: e.AltitudeDegrees > 0.0, actual: e.IsVisible));
	}

	/// <summary>From the far south an object of northern declination never rises, whereas the RA/Dec is unchanged.</summary>
	[Fact]
	public void Calculate_ObserverHemisphere_ChangesOnlyHorizontalCoordinates()
	{
		DateTimeOffset t = TestData.Utc(text: "2025-10-02T23:00:00Z");
		EphemerisEntry north = Calculate(times: [t])[0];
		EphemerisEntry south = Calculate(times: [t], observer: new ObserverLocation(LatitudeDegrees: -89.0, LongitudeDegrees: 0.0))[0];
		Assert.Equal(expected: north.RightAscensionHours, actual: south.RightAscensionHours, precision: 3);
		Assert.Equal(expected: north.DeclinationDegrees, actual: south.DeclinationDegrees, precision: 2);
		Assert.Equal(expected: north.DeclinationDegrees < 0.0, actual: south.AltitudeDegrees > 0.0);
	}

	/// <summary>The perturbed model agrees with the two-body model near the epoch.</summary>
	[Fact]
	public void Calculate_PerturbedModel_CloseToTwoBodyNearEpoch()
	{
		DateTimeOffset[] times = [TestData.Utc(text: "2025-05-20T00:00:00Z")];
		EphemerisEntry twoBody = Calculate(times: times, model: PropagationModel.TwoBody)[0];
		EphemerisEntry perturbed = Calculate(times: times, model: PropagationModel.Perturbed)[0];
		Assert.True(condition: Math.Abs(value: twoBody.RightAscensionHours - perturbed.RightAscensionHours) * 15.0 < 0.05);
		Assert.True(condition: Math.Abs(value: twoBody.DeclinationDegrees - perturbed.DeclinationDegrees) < 0.05);
	}

	/// <summary>A cancelled calculation throws <see cref="OperationCanceledException"/>.</summary>
	/// <returns>A task representing the asynchronous test.</returns>
	[Fact]
	public async Task CalculateAsync_Cancelled_Throws()
	{
		using CancellationTokenSource cts = new();
		await cts.CancelAsync();
		EphemerisRequest request = new(Elements: TestData.Ceres, Times: [TestData.Utc(text: "2025-05-20T00:00:00Z")], Observer: TestData.Greenwich, Criteria: new VisibilityCriteria());
		_ = await Assert.ThrowsAnyAsync<OperationCanceledException>(testCode: () => new EphemerisService().CalculateAsync(request: request, cancellationToken: cts.Token));
	}

	/// <summary>An invalid observer location is rejected.</summary>
	[Fact]
	public void Calculate_InvalidObserver_Throws()
		=> Assert.Throws<ArgumentOutOfRangeException>(testCode: () => Calculate(times: [TestData.Utc(text: "2025-05-20T00:00:00Z")], observer: new ObserverLocation(LatitudeDegrees: 95.0, LongitudeDegrees: 0.0)));

	/// <summary>Kepler's equation is solved accurately, including high eccentricities.</summary>
	/// <param name="meanAnomaly">The mean anomaly in radians.</param>
	/// <param name="eccentricity">The eccentricity.</param>
	[Theory]
	[InlineData(0.1, 0.0)]
	[InlineData(1.0, 0.5)]
	[InlineData(3.0, 0.9)]
	[InlineData(0.01, 0.99)]
	[InlineData(-2.0, 0.3)]
	public void SolveKepler_SatisfiesEquation(double meanAnomaly, double eccentricity)
	{
		double e = OrbitPropagationService.SolveKepler(meanAnomalyRadians: meanAnomaly, eccentricity: eccentricity);
		double residual = CoordinateTransformationService.NormalizeRadians(radians: e - (eccentricity * Math.Sin(a: e)) - meanAnomaly);
		Assert.True(condition: Math.Min(val1: residual, val2: (2.0 * Math.PI) - residual) < 1e-12);
	}

	/// <summary>Two-body propagation over one orbital period returns to the start.</summary>
	[Fact]
	public void PropagateTwoBody_OnePeriod_ReturnsToStart()
	{
		StateVector start = OrbitPropagationService.GetStateAtEpoch(elements: TestData.Ceres);
		double period = 2.0 * Math.PI * Math.Sqrt(d: Math.Pow(x: TestData.Ceres.SemiMajorAxisAu, y: 3) / AstronomicalConstants.GmSun);
		StateVector end = OrbitPropagationService.PropagateTwoBody(state: start, julianDateTdb: start.JulianDateTdb + period);
		Assert.True(condition: (end.Position - start.Position).Length < 1e-8);
	}

	/// <summary>The H-G magnitude reduces to H at r = Δ = 1 AU and zero phase.</summary>
	[Fact]
	public void CalculateApparentMagnitude_Reference()
	{
		Assert.Equal(expected: 5.0, actual: VisibilityCalculator.CalculateApparentMagnitude(absoluteMagnitude: 5.0, slopeParameter: 0.15, heliocentricDistanceAu: 1.0, observerDistanceAu: 1.0, phaseAngleDegrees: 0.0), precision: 9);
		Assert.True(condition: double.IsNaN(d: VisibilityCalculator.CalculateApparentMagnitude(absoluteMagnitude: double.NaN, slopeParameter: 0.15, heliocentricDistanceAu: 2.0, observerDistanceAu: 1.0, phaseAngleDegrees: 10.0)));
	}

	/// <summary>Each visibility criterion can individually make the object invisible.</summary>
	[Fact]
	public void IsVisible_EachCriterion()
	{
		VisibilityCriteria criteria = new(MinimumObjectAltitudeDegrees: 20.0, MaximumSunAltitudeDegrees: -12.0, LimitingMagnitude: 15.0, MinimumMoonSeparationDegrees: 10.0);
		Assert.True(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: 30.0, sunAltitudeDegrees: -20.0, apparentMagnitude: 12.0, moonSeparationDegrees: 40.0, criteria: criteria));
		Assert.False(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: -5.0, sunAltitudeDegrees: -20.0, apparentMagnitude: 12.0, moonSeparationDegrees: 40.0, criteria: criteria));
		Assert.False(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: 30.0, sunAltitudeDegrees: -6.0, apparentMagnitude: 12.0, moonSeparationDegrees: 40.0, criteria: criteria));
		Assert.False(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: 30.0, sunAltitudeDegrees: -20.0, apparentMagnitude: 16.0, moonSeparationDegrees: 40.0, criteria: criteria));
		Assert.False(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: 30.0, sunAltitudeDegrees: -20.0, apparentMagnitude: 12.0, moonSeparationDegrees: 5.0, criteria: criteria));
		Assert.False(condition: VisibilityCalculator.IsVisible(objectAltitudeDegrees: 30.0, sunAltitudeDegrees: -20.0, apparentMagnitude: double.NaN, moonSeparationDegrees: 40.0, criteria: criteria));
	}

	/// <summary>The CSV export uses the invariant culture regardless of the current culture.</summary>
	/// <returns>A task representing the asynchronous test.</returns>
	[Fact]
	public async Task WriteCsvAsync_UsesInvariantCulture()
	{
		CultureInfo previous = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name: "de-DE");
			EphemerisEntry entry = new(Time: TestData.Utc(text: "2025-01-01T00:30:00Z"), RightAscensionHours: 1.5, DeclinationDegrees: -0.25, AzimuthDegrees: 123.4567, AltitudeDegrees: -1.5,
				DistanceAu: 1.25, ApparentMagnitude: double.NaN, IsVisible: false, HeliocentricDistanceAu: 2.0, PhaseAngleDegrees: 10.0, SolarElongationDegrees: 150.0, SunAltitudeDegrees: -30.0, MoonSeparationDegrees: 45.0);
			using StringWriter writer = new(formatProvider: CultureInfo.InvariantCulture);
			await EphemerisExportService.WriteCsvAsync(entries: [entry], writer: writer);
			string[] lines = writer.ToString().Split(separator: Environment.NewLine, options: StringSplitOptions.RemoveEmptyEntries);
			Assert.Equal(expected: 2, actual: lines.Length);
			string[] fields = lines[1].Split(separator: ',');
			Assert.Equal(expected: lines[0].Split(separator: ',').Length, actual: fields.Length);
			Assert.Equal(expected: "2025-01-01T00:30:00Z", actual: fields[0]);
			Assert.Equal(expected: "1.500000", actual: fields[1]);
			Assert.Equal(expected: "-0.25000", actual: fields[3]);
			Assert.Equal(expected: "-00 15 00.0", actual: fields[4]);
			Assert.Equal(expected: "123.457", actual: fields[5]);
			Assert.Equal(expected: string.Empty, actual: fields[11]);
			Assert.Equal(expected: "false", actual: fields[^1]);
		}
		finally
		{
			CultureInfo.CurrentCulture = previous;
		}
	}
}
