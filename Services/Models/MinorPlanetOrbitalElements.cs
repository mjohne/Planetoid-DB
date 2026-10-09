/*
 * File:        MinorPlanetOrbitalElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents the osculating orbital elements of a minor planet.
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

namespace Planetoid_DB.Services;

/// <summary>Represents the heliocentric osculating orbital elements of a minor planet (ecliptic and equinox J2000.0, as published in MPCORB).</summary>
/// <param name="Designation">Designation or name of the minor planet.</param>
/// <param name="EpochJulianDateTt">Osculation epoch as Julian date in Terrestrial Time (TT) [d].</param>
/// <param name="MeanAnomalyDegrees">Mean anomaly at the epoch [°].</param>
/// <param name="ArgumentOfPerihelionDegrees">Argument of perihelion ω [°], J2000.0.</param>
/// <param name="LongitudeOfAscendingNodeDegrees">Longitude of the ascending node Ω [°], J2000.0.</param>
/// <param name="InclinationDegrees">Inclination to the ecliptic i [°], J2000.0.</param>
/// <param name="Eccentricity">Orbital eccentricity e (0 ≤ e &lt; 1).</param>
/// <param name="SemiMajorAxisAu">Semi-major axis a [AU].</param>
/// <param name="AbsoluteMagnitude">Absolute magnitude H [mag], or <see cref="double.NaN"/> if unknown.</param>
/// <param name="SlopeParameter">Slope parameter G of the IAU H,G magnitude system.</param>
/// <remarks>The slope parameter G is used in the IAU H,G magnitude system to describe the phase curve of the minor planet, which relates its brightness to the angle between the Sun, the minor planet, and the observer.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed record MinorPlanetOrbitalElements(
	string Designation,
	double EpochJulianDateTt,
	double MeanAnomalyDegrees,
	double ArgumentOfPerihelionDegrees,
	double LongitudeOfAscendingNodeDegrees,
	double InclinationDegrees,
	double Eccentricity,
	double SemiMajorAxisAu,
	double AbsoluteMagnitude,
	double SlopeParameter = 0.15)
{
	/// <summary>Validates the orbital elements.</summary>
	/// <exception cref="ArgumentException">Thrown when the elements are not physically meaningful for an elliptic orbit.</exception>
	/// <remarks>This method checks that the orbital elements are finite and within physically meaningful ranges for an elliptic orbit. It throws an <see cref="ArgumentException"/> if any element is invalid.</remarks>
	public void Validate()
	{
		// Check for non-finite values in the orbital elements
		if (!double.IsFinite(d: EpochJulianDateTt) || !double.IsFinite(d: MeanAnomalyDegrees) || !double.IsFinite(d: ArgumentOfPerihelionDegrees) || !double.IsFinite(d: LongitudeOfAscendingNodeDegrees))
		{
			throw new ArgumentException(message: "The orbital elements contain non-finite values.");
		}
		// Check that the eccentricity is in the range [0, 1) for an elliptic orbit
		if (!double.IsFinite(d: Eccentricity) || Eccentricity is < 0.0 or >= 1.0)
		{
			throw new ArgumentException(message: $"The eccentricity {Eccentricity} is outside the elliptic range [0, 1).");
		}
		// Check that the semi-major axis is positive
		if (!double.IsFinite(d: SemiMajorAxisAu) || SemiMajorAxisAu <= 0.0)
		{
			throw new ArgumentException(message: $"The semi-major axis {SemiMajorAxisAu} AU must be positive.");
		}
		// Check that the inclination is in the range [0°, 180°]
		if (!double.IsFinite(d: InclinationDegrees) || InclinationDegrees is < 0.0 or > 180.0)
		{
			throw new ArgumentException(message: $"The inclination {InclinationDegrees}° is outside the range [0°, 180°].");
		}
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
