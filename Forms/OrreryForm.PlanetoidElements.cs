/*
 * File:        OrreryForm.PlanetoidElements.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Displays an animated orrery (planetary machine) of all planetoids and the eight solar system planets around the Sun.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Displays an animated orrery of all planetoids and the eight solar system planets around the Sun.</summary>
/// <remarks><para>The form renders the orbits of a selected range of planetoids from the MPCORB database together with all eight solar system planets and the Sun as 3D ellipses in the ecliptic coordinate frame using OpenTK/OpenGL.</para>
/// <para>A time-speed slider advances or reverses the simulation clock; the current position of each body is propagated from its Keplerian orbital elements. A date/time control shows and sets the simulated instant.</para>
/// <para>Interaction: left-drag to rotate the view, right-drag to pan, scroll wheel to zoom in/out. Hover over a body to see its name.</para></remarks>
internal partial class OrreryForm
{
	/// <summary>Represents the Keplerian orbital elements of a single planetoid parsed from an MPCORB record.</summary>
	/// <param name="Name">Readable designation of the planetoid.</param>
	/// <param name="A">Semi-major axis in AU.</param>
	/// <param name="E">Eccentricity.</param>
	/// <param name="I">Inclination in degrees.</param>
	/// <param name="Om">Longitude of the ascending node in degrees.</param>
	/// <param name="Peri">Argument of perihelion in degrees.</param>
	/// <param name="M0">Mean anomaly at the epoch in degrees.</param>
	/// <param name="MeanMotion">Mean daily motion in degrees per day, or <see langword="null"/> when the MPCORB field is unavailable.</param>
	/// <param name="EpochJd">Julian Date of the reference epoch.</param>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct PlanetoidElements(string Name, double A, double E, double I, double Om, double Peri, double M0, double? MeanMotion, double EpochJd)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used by the debugger to display the state of the <see cref="OrreryForm"/> instance in a concise format.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
