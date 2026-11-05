/*
 * File:        DistanceConverter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods to convert distances between various units.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

namespace Planetoid_DB.Helpers;

/// <summary>Provides conversions between supported distance units.</summary>
/// <remarks>Conversions use meters as the intermediate unit and include metric, customary, light-time, astronomical, and planetary units.</remarks>
internal static class DistanceConverter
{
	/// <summary>Defines the speed of light in a vacuum (m/s) exactly</summary>
	/// <remarks>The speed of light is defined as exactly 299,792,458 meters per second.</remarks>
	private const decimal SPEED_OF_LIGHT = 299_792_458m;

	/// <summary>Defines the astronomical unit (AU) exactly</summary>
	/// <remarks>The astronomical unit is defined as exactly 149,597,870,700 meters.</remarks>
	private const decimal ASTRONOMICAL_UNIT = 149_597_870_700m;

	/// <summary>Provides the Conversion factors for converting any unit into <see cref="DistanceUnit.Meter" /></summary>
	/// <remarks>The conversion factors are based on the relationship between each unit and the meter.</remarks>
	private static readonly Dictionary<DistanceUnit, decimal> ToMeterFactors = new()
	{
		// Base unit conversions
		{ DistanceUnit.Meter, 1m },                                    // Base unit
		{ DistanceUnit.Kilometer, 1_000m },                            // 1 km = 1,000 m
		{ DistanceUnit.Landmile, 1_609.344m },                         // 1 land mile = 1,609.344 m
		{ DistanceUnit.NauticalMile, 1_852m },                         // 1 nautical mile = 1,852 m
		// light units (c * time in seconds)
		{ DistanceUnit.LightMillisecond, SPEED_OF_LIGHT / 1_000m },    // 1 ms = 0.001 s
		{ DistanceUnit.LightSecond, SPEED_OF_LIGHT },                  // 1 s
		{ DistanceUnit.LightMinute, SPEED_OF_LIGHT * 60m },            // 1 min = 60 s
		{ DistanceUnit.LightHour, SPEED_OF_LIGHT * 3_600m },           // 1 h = 3,600 s
		{ DistanceUnit.LightDay, SPEED_OF_LIGHT * 86_400m },           // 1 d = 86,400 s
		// astronomical & planetary units (semi-mayor axes)
		{ DistanceUnit.AstronomicalUnit, ASTRONOMICAL_UNIT },          // 1 AU = 149,597,870,700 m
		{ DistanceUnit.MercurialUnit, 0.387098m * ASTRONOMICAL_UNIT }, // Semi-major axis of Mercury's orbit (~0.387 AU)
		{ DistanceUnit.VenusianUnit, 0.723332m * ASTRONOMICAL_UNIT},   // Semi-major axis of Venus's orbit (~0.723 AU)
		{ DistanceUnit.LunarUnit, 384_400_000m },                      // Average distance between Earth and Moon (~384,400 km)
		{ DistanceUnit.MartianUnit, 1.52368055m * ASTRONOMICAL_UNIT},  // Semi-major axis of Mars's orbit (~1.523 AU)
		{ DistanceUnit.JovianUnit, 5.2038m * ASTRONOMICAL_UNIT },      // Semi-major axis of Jupiter's orbit (~5.204 AU)
		{ DistanceUnit.SaturnianUnit, 9.5826m * ASTRONOMICAL_UNIT },   // Semi-major axis of Saturn's orbit (~9.582 AU)
		{ DistanceUnit.UranianUnit, 19.2012m * ASTRONOMICAL_UNIT },    // Semi-major axis of Uranus's orbit (~19.201 AU)
		{ DistanceUnit.NeptunianUnit, 30.0471m * ASTRONOMICAL_UNIT },  // Semi-major axis of Neptune's orbit (~30.047 AU)
	};

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Converts a value from one unit of distance to another.</summary>
	/// <param name="value">The value to be converted.</param>
	/// <param name="from">The output unit.</param>
	/// <param name="to">The target unit.</param>
	/// <returns>The converted value.</returns>
	public static decimal Convert(decimal value, DistanceUnit from, DistanceUnit to)
	{
		// If the source and target units are the same, return the original value
		if (from == to)
		{
			// Log a debug message indicating that no conversion is needed
			logger.Debug(message: $"No conversion needed. Source and target units are the same: {from}. Returning original value: {value}.");
			return value;
		}
		// Conversion into meters
		decimal valueInMeters = value * ToMeterFactors[key: from];
		// Conversion from meters to the target unit
		return valueInMeters / ToMeterFactors[key: to];
	}

	/// <summary>Returns the conversion factor from one unit to another.</summary>
	/// <remarks>This method returns the conversion factor that can be used to convert a value from the source unit to the target unit.</remarks>
	public static decimal GetConversionFactor(DistanceUnit from, DistanceUnit to)
	{
		// Use the Convert method to get the conversion factor by converting 1 unit of the source to the target unit
		return Convert(value: 1m, from: from, to: to);
	}
}
