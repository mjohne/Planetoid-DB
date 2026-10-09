/*
 * File:        SolarSystemBody.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Enumerates the major solar system bodies used by the planetary ephemeris providers.
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

/// <summary>Enumerates the major solar system bodies used by the planetary ephemeris providers.</summary>
internal enum SolarSystemBody
{
	/// <summary>The planet Mercury.</summary>
	Mercury,
	/// <summary>The planet Venus.</summary>
	Venus,
	/// <summary>The Earth (geocenter).</summary>
	Earth,
	/// <summary>The Earth–Moon barycenter.</summary>
	EarthMoonBarycenter,
	/// <summary>The planet Mars (system barycenter).</summary>
	Mars,
	/// <summary>The planet Jupiter (system barycenter).</summary>
	Jupiter,
	/// <summary>The planet Saturn (system barycenter).</summary>
	Saturn,
	/// <summary>The planet Uranus (system barycenter).</summary>
	Uranus,
	/// <summary>The planet Neptune (system barycenter).</summary>
	Neptune
}
