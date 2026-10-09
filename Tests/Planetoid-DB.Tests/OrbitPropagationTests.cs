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

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for orbit propagation input validation.</summary>
public sealed class OrbitPropagationTests
{
	/// <summary>Verifies two-body propagation rejects non-finite semi-major axes.</summary>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void GetTwoBodyState_RejectsNonFiniteSemiMajorAxis(double semiMajorAxisAu)
	{
		MinorPlanetOrbitalElements elements = TestData.Ceres() with { SemiMajorAxisAu = semiMajorAxisAu };

		Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: 2451545.0));
	}

	/// <summary>Verifies two-body propagation rejects non-finite Julian dates.</summary>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void GetTwoBodyState_RejectsNonFiniteJulianDates(double julianDate)
	{
		MinorPlanetOrbitalElements elements = TestData.Ceres();

		Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(elements: elements, julianDateTdb: julianDate));
		Assert.Throws<ArgumentOutOfRangeException>(() => OrbitPropagationService.GetTwoBodyState(
			elements: elements with { EpochJulianDateTt = julianDate },
			julianDateTdb: 2451545.0));
	}

	/// <summary>Verifies perturbed propagation rejects non-finite initial and target Julian dates.</summary>
	[Theory]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	[InlineData(double.NegativeInfinity)]
	public void Propagate_RejectsNonFiniteJulianDates(double julianDate)
	{
		OrbitPropagationService service = new(planetaryEphemeris: new AnalyticalPlanetaryEphemeris());
		StateVector state = new(JulianDateTdb: 2451545.0, Position: new Vector3d(X: 2.0, Y: 0.0, Z: 0.0), Velocity: new Vector3d(X: 0.0, Y: 0.01, Z: 0.0));
		using CancellationTokenSource cancellation = new();
		cancellation.Cancel();

		Assert.Throws<ArgumentOutOfRangeException>(() => service.Propagate(state: state with { JulianDateTdb = julianDate }, targetJulianDateTdb: 2451545.0, cancellationToken: cancellation.Token));
		Assert.Throws<ArgumentOutOfRangeException>(() => service.Propagate(state: state, targetJulianDateTdb: julianDate, cancellationToken: cancellation.Token));
	}
}
