namespace Planetoid_DB.Helpers;

internal static class DistanceConverter
{
	/// <summary>Defines the speed of light in a vacuum (m/s) exactly</summary>
	private const decimal SPEED_OF_LIGHT = 299_792_458m;

	/// <summary>Defines the astronomical unit (AU) exactly</summary>
	private const decimal ASTRONOMICAL_UNIT = 149_597_870_700m;

	/// <summary>Provides the Conversion factors for converting any unit into <see cref="DistanceUnit.Meter" /></summary>
	private static readonly Dictionary<DistanceUnit, decimal> ToMeterFactors = new()
	{
		{ DistanceUnit.Meter, 1m },
		{ DistanceUnit.Kilometer, 1_000m },
		{ DistanceUnit.Landmile, 1_609.344m },
		{ DistanceUnit.NauticalMile, 1_852m },

		// light units (c * time in seconds)
		{ DistanceUnit.LightMillisecond, SPEED_OF_LIGHT / 1_000m },
		{ DistanceUnit.LightSecond, SPEED_OF_LIGHT },
		{ DistanceUnit.LightMinute, SPEED_OF_LIGHT * 60m },
		{ DistanceUnit.LightHour, SPEED_OF_LIGHT * 3_600m },
		{ DistanceUnit.LightDay, SPEED_OF_LIGHT * 86_400m },

		// astronomical & planetary units (semi-mayor axes)
		{ DistanceUnit.AstronomicalUnit, ASTRONOMICAL_UNIT },
		{ DistanceUnit.MercurialUnit, 0.387098m * ASTRONOMICAL_UNIT },
		{ DistanceUnit.VenusianUnit, 0.723332m * ASTRONOMICAL_UNIT},
		{ DistanceUnit.LunarUnit, 384_400_000m },
		{ DistanceUnit.MartianUnit, 1.52368055m * ASTRONOMICAL_UNIT},
		{ DistanceUnit.JovianUnit, 5.2038m * ASTRONOMICAL_UNIT },
		{ DistanceUnit.SaturnianUnit, 9.5826m * ASTRONOMICAL_UNIT },
		{ DistanceUnit.UranianUnit, 19.2012m * ASTRONOMICAL_UNIT },
		{ DistanceUnit.NeptunianUnit, 30.0471m * ASTRONOMICAL_UNIT },
	};

	/// <summary>Converts a value from one unit of distance to another.</summary>
	/// <param name="value">The value to be converted.</param>
	/// <param name="from">The output unit.</param>
	/// <param name="to">The target unit.</param>
	/// <returns>The converted value.</returns>
	public static decimal Convert(decimal value, DistanceUnit from, DistanceUnit to)
	{
		if (from == to)
		{
			return value;
		}

		// Conversion into meters
		decimal valueInMeters = value * ToMeterFactors[key: from];

		// Conversion from meters to the target unit
		return valueInMeters / ToMeterFactors[key: to];
	}

	/// <summary>Returns the conversion factor from one unit to another.</summary>
	public static decimal GetConversionFactor(DistanceUnit from, DistanceUnit to)
	{
		return Convert(value: 1m, from: from, to: to);
	}
}
