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

using System.Diagnostics;

namespace Planetoid_DB.Services;

/// <summary>Represents a geodetic observer location on the Earth (WGS84).</summary>
/// <param name="LatitudeDegrees">Geodetic latitude in degrees [−90°, +90°], north positive.</param>
/// <param name="LongitudeDegrees">Geodetic longitude in degrees [−180°, +180°], east positive.</param>
/// <param name="ElevationMeters">Height above the WGS84 ellipsoid in meters.</param>
/// <remarks>The geodetic coordinates are based on the WGS84 reference ellipsoid, which is the standard for GPS and other geospatial applications. The latitude and longitude are expressed in degrees, with north and east being positive directions. The elevation is measured in meters above the WGS84 ellipsoid, which approximates the shape of the Earth.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed record ObserverLocation(double LatitudeDegrees, double LongitudeDegrees, double ElevationMeters = 0.0)
{
	/// <summary>Validates the location and throws if it is out of range.</summary>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when latitude, longitude or elevation are out of range.</exception>
	/// <remarks>This method checks that the latitude is between −90° and +90°, the longitude is between −180° and +180°, and the elevation is between −500 m and 10000 m. If any of these conditions are not met, an <see cref="ArgumentOutOfRangeException"/> is thrown with a descriptive message.</remarks>
	public void Validate()
	{
		// Validate the latitude, longitude and elevation values to ensure they are within the valid ranges for geodetic coordinates.
		if (!double.IsFinite(d: LatitudeDegrees) || LatitudeDegrees is < -90.0 or > 90.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LatitudeDegrees), actualValue: LatitudeDegrees, message: "Latitude must be between −90° and +90°.");
		}
		// Validate the longitude value to ensure it is within the valid range for geodetic coordinates.
		if (!double.IsFinite(d: LongitudeDegrees) || LongitudeDegrees is < -180.0 or > 180.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(LongitudeDegrees), actualValue: LongitudeDegrees, message: "Longitude must be between −180° and +180°.");
		}
		// Validate the elevation value to ensure it is within the valid range for geodetic coordinates.
		if (!double.IsFinite(d: ElevationMeters) || ElevationMeters is < -500.0 or > 10000.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(ElevationMeters), actualValue: ElevationMeters, message: "Elevation must be between −500 m and 10000 m.");
		}
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
