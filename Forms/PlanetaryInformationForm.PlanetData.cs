/*
 * File:        PlanetaryInformationForm.PlanetData.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that displays orbital and physical properties of the eight planets of the Solar System.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Provides a user interface for viewing and analyzing orbital and physical data for a planet.</summary>
/// <remarks>The form displays reference planetary parameters and derived values used to compare and study the characteristics of a selected world.</remarks>
internal partial class PlanetaryInformationForm
{
	/// <summary>Represents a fixed set of well-known base parameters for a single planet.</summary>
	/// <param name="Name">Gets the display name of the planet.</param>
	/// <param name="SemiMajorAxisAu">Gets the semi-major axis of the orbit in astronomical units (AU).</param>
	/// <param name="Eccentricity">Gets the numerical eccentricity of the orbit (dimensionless).</param>
	/// <param name="InclinationDeg">Gets the inclination of the orbital plane to the ecliptic in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">Gets the longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">Gets the argument of perihelion in degrees.</param>
	/// <param name="MeanAnomalyDeg">Gets the mean anomaly at the reference epoch in degrees.</param>
	/// <param name="SiderealPeriodYears">Gets the sidereal orbital period in Julian years.</param>
	/// <param name="SynodicPeriodDays">Gets the synodic period as seen from Earth in Earth days.</param>
	/// <param name="EquatorialDiameterKm">Gets the equatorial diameter in kilometres.</param>
	/// <param name="Flattening">Gets the flattening (oblateness) of the planet (dimensionless).</param>
	/// <param name="MassKg">Gets the mass in kilograms.</param>
	/// <param name="VolumeKm3">Gets the volume in cubic kilometres.</param>
	/// <param name="DensityKgPerM3">Gets the mean density in kilograms per cubic metre.</param>
	/// <param name="SurfaceGravityMPerS2">Gets the equatorial surface gravity in metres per second squared.</param>
	/// <param name="EscapeVelocityKmPerS">Gets the escape velocity in kilometres per second.</param>
	/// <param name="RotationPeriodDays">Gets the sidereal rotation period in Earth days.</param>
	/// <param name="EquatorialRotationVelocityMPerS">Gets the equatorial rotational velocity in metres per second.</param>
	/// <param name="AxialTiltDeg">Gets the axial tilt (obliquity to the orbit) in degrees.</param>
	/// <param name="GeometricAlbedo">Gets the geometric albedo (dimensionless).</param>
	/// <param name="MaxApparentMagnitude">Gets the maximum apparent magnitude as seen from Earth.</param>
	/// <param name="AbsoluteMagnitude">Gets the absolute magnitude H.</param>
	/// <remarks>Values are current best reference values (NASA/IAU) used to compute all derived orbital and physical properties displayed by the form.</remarks>
	private record PlanetData(string Name, double SemiMajorAxisAu, double Eccentricity, double InclinationDeg, double LongitudeAscendingNodeDeg, double ArgumentPerihelionDeg, double MeanAnomalyDeg, double SiderealPeriodYears, double SynodicPeriodDays, double EquatorialDiameterKm, double Flattening, double MassKg, double VolumeKm3, double DensityKgPerM3, double SurfaceGravityMPerS2, double EscapeVelocityKmPerS, double RotationPeriodDays, double EquatorialRotationVelocityMPerS, double AxialTiltDeg, double GeometricAlbedo, double MaxApparentMagnitude, double AbsoluteMagnitude);
}
