/*
 * File:        OrbitPropagationTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for orbit propagation input validation.
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

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for orbit propagation input validation.</summary>
/// <remarks>These tests verify that the orbit propagation methods correctly handle invalid input by throwing appropriate exceptions.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class OrbitPropagationTests
{
	/// <summary>Verifies two-body propagation rejects non-finite semi-major axes.</summary>
	/// <remarks>Two-body propagation requires a finite semi-major axis to compute the orbital state. Non-finite values (NaN, positive infinity, negative infinity) are invalid and should result in an <see cref="ArgumentOutOfRangeException"/>.</remarks>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void GetTwoBodyStateRejectsNonFiniteSemiMajorAxis(double semiMajorAxisAu)
	{
		// Arrange: Create a valid set of orbital elements for Ceres, but override the semi-major axis with the test value.
		MinorPlanetOrbitalElements elements = TestData.Ceres() with { SemiMajorAxisAu = semiMajorAxisAu };
		// Act & Assert: Verify that calling GetTwoBodyState with the invalid semi-major axis throws an ArgumentOutOfRangeException.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: 2451545.0));
	}

	/// <summary>Verifies two-body propagation rejects non-finite Julian dates.</summary>
	/// <remarks>Two-body propagation requires finite Julian dates for both the epoch and the target date. Non-finite values (NaN, positive infinity, negative infinity) are invalid and should result in an <see cref="ArgumentOutOfRangeException"/>.</remarks>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void GetTwoBodyStateRejectsNonFiniteJulianDates(double julianDate)
	{
		// Arrange: Create a valid set of orbital elements for Ceres.
		MinorPlanetOrbitalElements elements = TestData.Ceres();
		// Act & Assert: Verify that calling GetTwoBodyState with the invalid Julian date throws an ArgumentOutOfRangeException.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: julianDate));
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(
			elements: elements with { EpochJulianDateTt = julianDate },
			julianDateTdb: 2451545.0));
	}

	/// <summary>Verifies two-body propagation rejects invalid orbital elements.</summary>
	/// <remarks>Two-body propagation requires valid orbital elements. Invalid values (NaN, positive infinity, negative values for eccentricity, or inclination outside the range [0, 180]) should result in an <see cref="ArgumentException"/>.</remarks>
	[Fact]
	public void GetTwoBodyStateRejectsInvalidOrbitalElements()
	{
		// Arrange: Create a valid set of orbital elements for Ceres.
		MinorPlanetOrbitalElements elements = TestData.Ceres();
		// Act & Assert: Verify that calling GetTwoBodyState with invalid orbital elements throws an ArgumentException.
		MinorPlanetOrbitalElements[] invalidElements =
		[
			elements with { MeanAnomalyDegrees = double.NaN },
			elements with { ArgumentOfPerihelionDegrees = double.PositiveInfinity },
			elements with { LongitudeOfAscendingNodeDegrees = double.NaN },
			elements with { Eccentricity = double.NaN },
			elements with { Eccentricity = -0.1 },
			elements with { Eccentricity = 1.0 },
			elements with { InclinationDegrees = double.NaN },
			elements with { InclinationDegrees = -0.1 },
			elements with { InclinationDegrees = 180.1 }
		];
		foreach (MinorPlanetOrbitalElements invalidElement in invalidElements)
		{
			_ = Assert.Throws<ArgumentException>(() => OrbitPropagationService.GetTwoBodyState(elements: invalidElement, julianDateTdb: 2451545.0));
		}
	}

	/// <summary>Verifies perturbed propagation rejects non-finite initial and target Julian dates.</summary>
	/// <remarks>Perturbed propagation requires finite initial and target Julian dates. Non-finite values (NaN, positive infinity, negative infinity) are invalid and should result in an <see cref="ArgumentOutOfRangeException"/>.</remarks>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void PropagateRejectsNonFiniteJulianDates(double julianDate)
	{
		// Arrange: Create a valid orbit propagation service and state vector.
		OrbitPropagationService service = new(planetaryEphemeris: new AnalyticalPlanetaryEphemeris());
		// Create a valid state vector with a finite Julian date, position, and velocity.
		StateVector state = new(JulianDateTdb: 2451545.0, Position: new Vector3d(X: 2.0, Y: 0.0, Z: 0.0), Velocity: new Vector3d(X: 0.0, Y: 0.01, Z: 0.0));
		// Act & Assert: Verify that calling Propagate with the invalid Julian date throws an ArgumentOutOfRangeException.
		using CancellationTokenSource cancellation = new();
		// Cancel the token to ensure that the test does not hang if the method does not throw as expected.
		cancellation.Cancel();
		// Test both cases: invalid initial Julian date and invalid target Julian date.
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => service.Propagate(state: state with { JulianDateTdb = julianDate }, targetJulianDateTdb: 2451545.0, cancellationToken: cancellation.Token));
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => service.Propagate(state: state, targetJulianDateTdb: julianDate, cancellationToken: cancellation.Token));
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
