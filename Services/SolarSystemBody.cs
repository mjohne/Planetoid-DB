/*
 * File:        SolarSystemBody.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Enumerates the solar system bodies provided by planetary ephemerides.
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

/// <summary>Enumerates the solar system bodies provided by planetary ephemerides.</summary>
/// <remarks>The numeric values are the NAIF integer IDs used by JPL SPK files (DE440/DE441).</remarks>
internal enum SolarSystemBody
{
	/// <summary>Mercury barycenter.</summary>
	Mercury = 1,
	/// <summary>Venus barycenter.</summary>
	Venus = 2,
	/// <summary>Earth-Moon barycenter.</summary>
	EarthMoonBarycenter = 3,
	/// <summary>Mars barycenter.</summary>
	Mars = 4,
	/// <summary>Jupiter barycenter.</summary>
	Jupiter = 5,
	/// <summary>Saturn barycenter.</summary>
	Saturn = 6,
	/// <summary>Uranus barycenter.</summary>
	Uranus = 7,
	/// <summary>Neptune barycenter.</summary>
	Neptune = 8,
	/// <summary>The Sun.</summary>
	Sun = 10,
	/// <summary>The Moon.</summary>
	Moon = 301,
	/// <summary>The Earth (geocenter).</summary>
	Earth = 399
}
