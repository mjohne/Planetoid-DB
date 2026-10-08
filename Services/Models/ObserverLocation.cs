/*
 * File:        ObserverLocation.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents a geodetic observer location on the Earth (WGS84).
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Services;

/// <summary>Represents a geodetic observer location on the Earth (WGS84).</summary>
/// <param name="LatitudeDegrees">Geodetic latitude in degrees [−90°, +90°], north positive.</param>
/// <param name="LongitudeDegrees">Geodetic longitude in degrees [−180°, +180°], east positive.</param>
/// <param name="ElevationMeters">Height above the WGS84 ellipsoid in meters.</param>
internal sealed record ObserverLocation(double LatitudeDegrees, double LongitudeDegrees, double ElevationMeters = 0.0)
{
	/// <summary>Validates the location and throws if it is out of range.</summary>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when latitude, longitude or elevation are out of range.</exception>
	public void Validate()
	{
		if (!double.IsFinite(d: LatitudeDegrees) || LatitudeDegrees is < -90.0 or > 90.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LatitudeDegrees), actualValue: LatitudeDegrees, message: "Latitude must be between −90° and +90°.");
		}
		if (!double.IsFinite(d: LongitudeDegrees) || LongitudeDegrees is < -180.0 or > 360.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LongitudeDegrees), actualValue: LongitudeDegrees, message: "Longitude must be between −180° and +360°.");
		}
		if (!double.IsFinite(d: ElevationMeters) || ElevationMeters is < -500.0 or > 10000.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(ElevationMeters), actualValue: ElevationMeters, message: "Elevation must be between −500 m and 10000 m.");
		}
	}
}
