/*
 * File:        DerivedElements.OrbitDynamicalClass.cs
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

/// <summary>Provides derived orbital elements and dynamical classifications for minor-planet orbits.</summary>
/// <remarks>Dynamical classifications are based on Tisserand's parameter or, for hyperbolic and interstellar trajectories, the semi-major axis.</remarks>
internal partial class DerivedElements
{
	/// <summary>Represents the dynamical classification of an orbit based on Tisserand's parameter.</summary>
	internal enum OrbitDynamicalClass
	{
		/// <summary>Asteroidal orbit (T_J &gt; 3.0).</summary>
		Asteroidal,

		/// <summary>Jupiter-Family Comet orbit (2.0 &lt;= T_J &lt;= 3.0).</summary>
		JupiterFamilyComet,

		/// <summary>Halley-type comet / Damocloid / Centaur orbit (T_J &lt; 2.0).</summary>
		HalleyTypeCometOrDamocloid,

		/// <summary>Hyperbolic or interstellar trajectory (a &lt; 0).</summary>
		HyperbolicOrInterstellar
	}
}
