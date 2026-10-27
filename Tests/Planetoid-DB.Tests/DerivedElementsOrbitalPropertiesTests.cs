/*
 * File:        DerivedElementsOrbitalPropertiesTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for the orbital properties added to DerivedElements.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for the orbital properties of <see cref="DerivedElements"/>.</summary>
/// <remarks>The reference values were computed independently from the formulas given in the method remarks.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class DerivedElementsOrbitalPropertiesTests
{
	/// <summary>Reference semi-major axis in AU used by the tests.</summary>
	private const double SemiMajorAxis = 2.5;

	/// <summary>Reference numerical eccentricity used by the tests.</summary>
	private const double Eccentricity = 0.3;

	/// <summary>Reference inclination in degrees used by the tests.</summary>
	private const double Inclination = 15.0;

	/// <summary>Reference longitude of the ascending node in degrees used by the tests.</summary>
	private const double LongitudeAscendingNode = 40.0;

	/// <summary>Reference argument of perihelion in degrees used by the tests.</summary>
	private const double ArgumentPerihelion = 60.0;

	/// <summary>Verifies the mean motion in degrees per year and per day.</summary>
	/// <remarks>For a = 1 AU the mean motion is 360°/year or 0.9856076686°/day.</remarks>
	[Fact]
	public void CalculateMeanMotionReturnsDegreesPerYearAndDay()
	{
		Assert.Equal(expected: 360.0, actual: DerivedElements.CalculateMeanMotion(semiMajorAxis: 1.0), precision: 9);
		Assert.Equal(expected: 360.0 / 365.242188, actual: DerivedElements.CalculateMeanDailyMotion(semiMajorAxis: 1.0), precision: 9);
		Assert.Equal(expected: 0.249351251320533, actual: DerivedElements.CalculateMeanDailyMotion(semiMajorAxis: SemiMajorAxis), precision: 9);
		// The yearly and daily mean motions must differ exactly by the sidereal year used by the class.
		Assert.Equal(
			expected: DerivedElements.CalculateMeanMotion(semiMajorAxis: SemiMajorAxis),
			actual: DerivedElements.CalculateMeanDailyMotion(semiMajorAxis: SemiMajorAxis) * 365.242188,
			precision: 9);
	}

	/// <summary>Verifies the Julian date of the perihelion passage.</summary>
	/// <remarks>With M = 90° and T = 3.952847075 years, the perihelion lies 360.985 days before the epoch.</remarks>
	[Fact]
	public void CalculatePerihelionDateReturnsJulianDate()
	{
		const double epochJd = 2460000.5;
		Assert.Equal(expected: 2459639.5556514449, actual: DerivedElements.CalculatePerihelionDate(meanAnomaly: 90.0, semiMajorAxis: SemiMajorAxis, epochJd: epochJd), precision: 6);
		Assert.Equal(expected: 2461083.3330456656, actual: DerivedElements.CalculateNextPerihelionDate(meanAnomaly: 90.0, semiMajorAxis: SemiMajorAxis, epochJd: epochJd), precision: 6);
		// At perihelion (M = 0) the passage date equals the epoch.
		Assert.Equal(expected: epochJd, actual: DerivedElements.CalculatePerihelionDate(meanAnomaly: 0.0, semiMajorAxis: SemiMajorAxis, epochJd: epochJd), precision: 6);
		// The difference between the passage date and the epoch equals the time since perihelion.
		double timeSincePerihelionDays = (epochJd - DerivedElements.CalculatePerihelionDate(meanAnomaly: 90.0, semiMajorAxis: SemiMajorAxis, epochJd: epochJd));
		Assert.Equal(expected: DerivedElements.CalculateTimeSincePerihelion(meanAnomaly: 90.0, semiMajorAxis: SemiMajorAxis) * 365.25, actual: timeSincePerihelionDays, precision: 6);
	}

	/// <summary>Verifies the exact ecliptic coordinates of the perihelion and the aphelion.</summary>
	/// <remarks>The projection accounts for the inclination, unlike the common approximation ϖ = Ω + ω.</remarks>
	[Fact]
	public void CalculatePerihelionAndAphelionPositionReturnsEclipticCoordinates()
	{
		(double longitude, double latitude) = DerivedElements.CalculatePerihelionPosition(
			inclination: Inclination,
			longitudeAscendingNode: LongitudeAscendingNode,
			argumentPerihelion: ArgumentPerihelion);
		Assert.Equal(expected: 99.13252220932563, actual: longitude, precision: 9);
		Assert.Equal(expected: 12.952539642222362, actual: latitude, precision: 9);

		(double aphelionLongitude, double aphelionLatitude) = DerivedElements.CalculateAphelionPosition(
			inclination: Inclination,
			longitudeAscendingNode: LongitudeAscendingNode,
			argumentPerihelion: ArgumentPerihelion);
		Assert.Equal(expected: 279.1325222093256, actual: aphelionLongitude, precision: 9);
		Assert.Equal(expected: -12.952539642222359, actual: aphelionLatitude, precision: 9);

		// For i = 0 the ecliptic longitude reduces to Ω + ω and the latitude vanishes.
		(double flatLongitude, double flatLatitude) = DerivedElements.CalculatePerihelionPosition(
			inclination: 0.0,
			longitudeAscendingNode: LongitudeAscendingNode,
			argumentPerihelion: ArgumentPerihelion);
		Assert.Equal(expected: 100.0, actual: flatLongitude, precision: 9);
		Assert.Equal(expected: 0.0, actual: flatLatitude, precision: 9);
	}

	/// <summary>Verifies the heliocentric distances at the ascending and descending node.</summary>
	/// <remarks>At the nodes cos(ν) = ±cos(ω), so the conic equation yields p/(1 ± e·cos ω).</remarks>
	[Fact]
	public void CalculateNodeDistancesReturnsHeliocentricDistances()
	{
		Assert.Equal(expected: 1.978260869565218, actual: DerivedElements.CalculateAscendingNodeDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, argumentPerihelion: ArgumentPerihelion), precision: 9);
		Assert.Equal(expected: 2.676470588235294, actual: DerivedElements.CalculateDescendingNodeDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, argumentPerihelion: ArgumentPerihelion), precision: 9);
		// For ω = 0 the ascending node coincides with the perihelion and the descending node with the aphelion.
		Assert.Equal(expected: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity), actual: DerivedElements.CalculateAscendingNodeDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, argumentPerihelion: 0.0), precision: 9);
		Assert.Equal(expected: DerivedElements.CalculateAphelionDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity), actual: DerivedElements.CalculateDescendingNodeDistance(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, argumentPerihelion: 0.0), precision: 9);
	}

	/// <summary>Verifies the circular and escape velocities at a given heliocentric distance.</summary>
	/// <remarks>At 1 AU the circular velocity is 2π AU/year and the escape velocity is sqrt(2) times larger.</remarks>
	[Fact]
	public void CalculateCircularAndEscapeVelocities()
	{
		Assert.Equal(expected: 6.283185307179586, actual: DerivedElements.CalculateCircularOrbitalVelocity(heliocentricDistance: 1.0), precision: 9);
		Assert.Equal(expected: 8.885765876316732, actual: DerivedElements.CalculateHeliocentricEscapeVelocity(heliocentricDistance: 1.0), precision: 9);
		Assert.Equal(expected: 4.749641646894903, actual: DerivedElements.CalculateCircularOrbitalVelocity(heliocentricDistance: 1.75), precision: 9);
		Assert.Equal(expected: 6.717007633450856, actual: DerivedElements.CalculateHeliocentricEscapeVelocity(heliocentricDistance: 1.75), precision: 9);
		// The escape velocity is sqrt(2) times the circular velocity at the same distance.
		Assert.Equal(
			expected: Math.Sqrt(d: 2.0) * DerivedElements.CalculateCircularOrbitalVelocity(heliocentricDistance: 1.75),
			actual: DerivedElements.CalculateHeliocentricEscapeVelocity(heliocentricDistance: 1.75),
			precision: 9);
	}

	/// <summary>Verifies the synodic periods with all eight planets.</summary>
	/// <remarks>The Earth value must match the existing single-planet calculation; equal periods yield infinity.</remarks>
	[Fact]
	public void CalculateSynodicPeriodsReturnsOneEntryPerPlanet()
	{
		List<DerivedElements.SynodicPeriod> synodicPeriods = DerivedElements.CalculateSynodicPeriods(semiMajorAxis: SemiMajorAxis);
		Assert.Equal(expected: 8, actual: synodicPeriods.Count);
		Assert.Equal(expected: "Mercury", actual: synodicPeriods[0].PlanetName);
		Assert.Equal(expected: "Neptune", actual: synodicPeriods[7].PlanetName);
		Assert.Equal(expected: 0.256472826216022, actual: synodicPeriods[0].SynodicPeriodYears, precision: 9);
		Assert.Equal(expected: 1.338656210270802, actual: synodicPeriods[2].SynodicPeriodYears, precision: 9);
		Assert.Equal(expected: 3.588179456576375, actual: synodicPeriods[3].SynodicPeriodYears, precision: 9);
		// The Earth entry must equal the established single-planet calculation.
		Assert.Equal(expected: DerivedElements.CalculateSynodicOrbitalPeriod(semiMajorAxis: SemiMajorAxis), actual: synodicPeriods[2].SynodicPeriodYears, precision: 9);
		// A planetoid with a = 1 AU has an infinite synodic period with Earth.
		Assert.True(condition: double.IsPositiveInfinity(d: DerivedElements.CalculateSynodicPeriods(semiMajorAxis: 1.0)[2].SynodicPeriodYears));
	}

	/// <summary>Verifies the equinoctial elements h, k, p and q.</summary>
	/// <remarks>The elements must satisfy h = e·sin(ω+Ω), k = e·cos(ω+Ω), p = tan(i/2)·sin Ω and q = tan(i/2)·cos Ω.</remarks>
	[Fact]
	public void CalculateEquinoctialElements()
	{
		const double longitudeOfPerihelionRad = 100.0 * Math.PI / 180.0;
		Assert.Equal(expected: Eccentricity * Math.Sin(a: longitudeOfPerihelionRad), actual: DerivedElements.CalculateEquinoctialOrbitalElementH(numericalEccentricity: Eccentricity, argumentPerihelion: ArgumentPerihelion, lengthAscendingNode: LongitudeAscendingNode), precision: 9);
		Assert.Equal(expected: Eccentricity * Math.Cos(d: longitudeOfPerihelionRad), actual: DerivedElements.CalculateEquinoctialOrbitalElementK(numericalEccentricity: Eccentricity, argumentPerihelion: ArgumentPerihelion, lengthAscendingNode: LongitudeAscendingNode), precision: 9);

		double halfInclination = Math.Tan(a: Inclination * Math.PI / 180.0 / 2.0);
		Assert.Equal(expected: halfInclination * Math.Sin(a: LongitudeAscendingNode * Math.PI / 180.0), actual: DerivedElements.CalculateEquinoctialOrbitalElementP(orbitalInclination: Inclination, lengthAscendingNode: LongitudeAscendingNode), precision: 9);
		Assert.Equal(expected: halfInclination * Math.Cos(d: LongitudeAscendingNode * Math.PI / 180.0), actual: DerivedElements.CalculateEquinoctialOrbitalElementQ(orbitalInclination: Inclination, lengthAscendingNode: LongitudeAscendingNode), precision: 9);
	}

	/// <summary>Verifies that the velocity components add up to the instantaneous orbital speed.</summary>
	/// <remarks>The components must satisfy v² = v_r² + v_t², and the total speed must match the vis-viva equation at the apsides.</remarks>
	[Fact]
	public void VelocityComponentsAreConsistent()
	{
		const double trueAnomaly = 55.0;
		double radial = DerivedElements.CalculateRadialVelocityComponent(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: trueAnomaly);
		double tangential = DerivedElements.CalculateTangentialVelocityComponent(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: trueAnomaly);
		double total = DerivedElements.CalculateCurrentOrbitalVelocity(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: trueAnomaly);
		Assert.Equal(expected: total * total, actual: (radial * radial) + (tangential * tangential), precision: 9);
		// At the apsides the total speed equals the perihelion and aphelion velocity.
		Assert.Equal(expected: DerivedElements.CalculatePerihelionVelocity(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity), actual: DerivedElements.CalculateCurrentOrbitalVelocity(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: 0.0), precision: 9);
		Assert.Equal(expected: DerivedElements.CalculateAphelionVelocity(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity), actual: DerivedElements.CalculateCurrentOrbitalVelocity(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: 180.0), precision: 9);
		// The radial component vanishes at both apsides.
		Assert.Equal(expected: 0.0, actual: DerivedElements.CalculateRadialVelocityComponent(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: 0.0), precision: 9);
		Assert.Equal(expected: 0.0, actual: DerivedElements.CalculateRadialVelocityComponent(semiMajorAxis: SemiMajorAxis, numericalEccentricity: Eccentricity, trueAnomaly: 180.0), precision: 9);
	}

	/// <summary>Verifies the heliocentric longitude and latitude.</summary>
	/// <remarks>For an argument of latitude of 90° the longitude is Ω + 90° and the latitude equals the inclination.</remarks>
	[Fact]
	public void CalculateHeliocentricCoordinates()
	{
		Assert.Equal(expected: 15.0, actual: DerivedElements.CalculateHeliocentricLatitude(argPeri: ArgumentPerihelion, trueAnom: 30.0, inclination: Inclination), precision: 9);
		Assert.Equal(expected: 130.0, actual: DerivedElements.CalculateHeliocentricLongitude(longitudeAscendingNode: LongitudeAscendingNode, argumentPerihelion: ArgumentPerihelion, trueAnomaly: 30.0, inclination: Inclination), precision: 9);
		Assert.Equal(expected: 130.0, actual: DerivedElements.CalculateTrueLongitude(longitudeAscendingNode: LongitudeAscendingNode, argumentPerihelion: ArgumentPerihelion, trueAnomaly: 30.0), precision: 9);
		Assert.Equal(expected: 2.5 * Math.Sin(a: 15.0 * Math.PI / 180.0), actual: DerivedElements.CalculateMaxEclipticDistance(semiMajorAxis: SemiMajorAxis, inclination: Inclination), precision: 9);
	}

	/// <summary>Verifies that the standard gravitational parameter is the constant 4π² in AU and years.</summary>
	/// <remarks>For heliocentric orbits in AU and Julian years the solar GM equals 4π².</remarks>
	[Fact]
	public void CalculateStandardGravitationalParameterIsConstant()
	{
		Assert.Equal(expected: 4.0 * Math.PI * Math.PI, actual: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: 1.0), precision: 9);
		Assert.Equal(expected: 4.0 * Math.PI * Math.PI, actual: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: 5.0), precision: 9);
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
