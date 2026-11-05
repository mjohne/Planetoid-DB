/*
 * File:        DistanceUnit.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Defines the supported units of distance.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Helpers;

/// <summary>Provides supported distance units</summary>
/// <remarks>This enum is used to specify the unit of distance for conversions and calculations.</remarks>
internal enum DistanceUnit
{
	/// <summary>Represents a meter.</summary>
	/// <remarks>This is the base unit of distance in the metric system.</remarks>
	Meter,
	/// <summary>Represents a kilometer, a unit of length equal to 1,000 meters.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	Kilometer,
	/// <summary>Represents a land mile, a unit of length equal to 1,609.344 meters.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	Landmile,
	/// <summary>Represents a nautical mile, a unit of length equal to 1,852 meters.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>	
	NauticalMile,
	/// <summary>Represents a light millisecond, a unit of distance equal to the distance light travels in 0.001 seconds.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LightMillisecond,
	/// <summary>Represents a light second, a unit of distance equal to the distance light travels in 1 second.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LightSecond,
	/// <summary>Represents a light minute, a unit of distance equal to the distance light travels in 60 seconds.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LightMinute,
	/// <summary>Represents a light hour, a unit of distance equal to the distance light travels in 3,600 seconds.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LightHour,
	/// <summary>Represents a light day, a unit of distance equal to the distance light travels in 86,400 seconds.</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LightDay,
	/// <summary>Represents a mercurial unit, a unit of distance equal to the semi-major axis of Mercury's orbit (~0.387 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	MercurialUnit,
	/// <summary>Represents a venusian unit, a unit of distance equal to the semi-major axis of Venus's orbit (~0.723 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	VenusianUnit,
	/// <summary>Represents an astronomical unit, a unit of distance equal to the semi-major axis of Earth's orbit (1 AU = 149'597'870'700 m).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	AstronomicalUnit,
	/// <summary>Represents a lunar unit, a unit of distance equal to the average distance between Earth and Moon (~384'400 km).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	LunarUnit,
	/// <summary>Represents a martian unit, a unit of distance equal to the semi-major axis of Mars's orbit (~1.523 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	MartianUnit,
	/// <summary>Represents a jovian unit, a unit of distance equal to the semi-major axis of Jupiter's orbit (~5.204 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	JovianUnit,
	/// <summary>Represents a saturnian unit, a unit of distance equal to the semi-major axis of Saturn's orbit (~9.582 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	SaturnianUnit,
	/// <summary>Represents a uranian unit, a unit of distance equal to the semi-major axis of Uranus's orbit (~19.201 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	UranianUnit,
	/// <summary>Represents a neptunian unit, a unit of distance equal to the semi-major axis of Neptune's orbit (~30.047 AU).</summary>
	/// <remarks>This unit is commonly used for measuring longer distances.</remarks>
	NeptunianUnit
}
