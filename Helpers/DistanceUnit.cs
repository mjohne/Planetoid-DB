namespace Planetoid_DB.Helpers;

/// <summary>Provides supported distance units</summary>
internal enum DistanceUnit
{
	Meter,
	Kilometer,
	Landmile,         // Statute mile ~ 1.609,344 m
	NauticalMile,     // Nautical mile = 1.852 m
	LightMillisecond, // c * 0,001 s
	LightSecond,      // c * 1 s
	LightMinute,      // c * 60 s
	LightHour,        // c * 3600 s
	LightDay,         // c * 86400 s
	MercurialUnit,    // Semi-major axis of Mercury's orbit (~0.387 AU)
	VenusianUnit,     // Semi-major axis of Venus's orbit (~0.723 AU)
	AstronomicalUnit, // Semi-major axis of Earth's orbit (1 AU = 149'597'870'700 m)
	LunarUnit,        // Average distance between Earth and Moon (~384'400 km)
	MartianUnit,       // Semi-major axis of Mars's orbit (~1.523 AU)
	JovianUnit,       // Semi-major axis of Jupiter's orbit (~5.204 AU)
	SaturnianUnit,    // Semi-major axis of Saturn's orbit (~9.582 AU)
	UranianUnit,      // Semi-major axis of Uranus's orbit (~19.201 AU)
	NeptunianUnit     // Semi-major axis of Neptune's orbit (~30.047 AU)
}
