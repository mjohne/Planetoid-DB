/*
 * File:        AstronomicalCoordinateSystem.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Converts positions between common astronomical coordinate systems.
 * Remarks: All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.
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

/// <summary>Identifies a supported astronomical coordinate system.</summary>
/// <remarks>All coordinate arguments and results are in degrees. The equatorial fixed frame is J2000.0/ICRS; date-dependent frames use IAU 1976 precession and the mean obliquity of date. Sidereal time uses IAU 1982 GMST with UTC as an approximation for UT1.</remarks>
internal enum AstronomicalCoordinateSystem
{
	/// <summary>Horizontal azimuth and altitude for the supplied observer and time.</summary>
	/// <remarks>Azimuth is measured from north through east. Altitude is measured from the horizon.</remarks>
	Horizontal,
	/// <summary>Equatorial coordinates in the fixed J2000.0 frame.</summary>
	/// <remarks>Right ascension is measured in degrees from the vernal equinox. Declination is measured in degrees from the celestial equator.</remarks>
	EquatorialJ2000,
	/// <summary>Equatorial coordinates in the mean equator and equinox of date.</summary>
	/// <remarks>Right ascension is measured in degrees from the vernal equinox. Declination is measured in degrees from the celestial equator.</remarks>
	EquatorialOfDate,
	/// <summary>Ecliptic coordinates in the mean ecliptic of date.</summary>
	/// <remarks>Longitude is measured in degrees from the vernal equinox. Latitude is measured in degrees from the ecliptic plane.</remarks>
	Ecliptic,
	/// <summary>Galactic coordinates in the IAU-defined galactic frame.</summary>
	/// <remarks>Longitude is measured in degrees from the galactic center. Latitude is measured in degrees from the galactic plane.</remarks>
	Galactic,
	/// <summary>Supergalactic coordinates in the de Vaucouleurs frame.</summary>
	/// <remarks>Longitude is measured in degrees from the supergalactic center. Latitude is measured in degrees from the supergalactic plane.</remarks>
	Supergalactic
}
