/*
 * File:        StateVector.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents a heliocentric state vector in the ICRF/J2000 equatorial frame.
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

/// <summary>Represents a heliocentric state vector in the ICRF/J2000 equatorial frame.</summary>
/// <param name="JulianDateTdb">Julian date in Barycentric Dynamical Time (TDB) [d].</param>
/// <param name="Position">Heliocentric position [AU].</param>
/// <param name="Velocity">Heliocentric velocity [AU/d].</param>
internal readonly record struct StateVector(double JulianDateTdb, Vector3d Position, Vector3d Velocity);
