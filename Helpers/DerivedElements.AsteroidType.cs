/*
 * File:        DerivedElements.AsteroidType.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods for calculating various orbital elements.
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

internal partial class DerivedElements
{
	/// <summary>Represents standard taxonomic types of asteroids and their average densities.</summary>
	internal enum AsteroidType
	{
		/// <summary>Carbonaceous asteroid (C-type), average density ~1.3 g/cm³.</summary>
		CType,

		/// <summary>Silicaceous asteroid (S-type), average density ~2.7 g/cm³.</summary>
		SType,

		/// <summary>Metallic asteroid (M-type), average density ~4.5 g/cm³.</summary>
		MType,

		/// <summary>Icy body / Trans-Neptunian Object (TNO), average density ~1.0 g/cm³.</summary>
		IcyObject,

		/// <summary>V-type asteroid (basaltic, like 4 Vesta), average density ~3.0 g/cm³.</summary>
		VType
	}
}