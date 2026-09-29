/*
 * File:        AEIDiagram3DForm.AeiPoint.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: ...
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

/// <summary>Displays a 3D a,e,i diagram for all known planetoids.</summary>
/// <remarks>This form is used to visualize the three-dimensional a, e, i point cloud for all known planetoids, providing interactive camera controls for rotation, zoom, and pan.</remarks>
internal partial class AEIDiagram3DForm
{
	/// <summary>
	///
	/// </summary>
	/// <param name="A"></param>
	/// <param name="E"></param>
	/// <param name="I"></param>
	/// <remarks></remarks>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	private readonly record struct AeiPoint(double A, double E, double I)
	{
		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>The property currently returns the same string as <c>ToString()</c> on this instance, but it can be customized to include more specific information if needed.</remarks>
		private string DebuggerDisplay => ToString();
	}
}
