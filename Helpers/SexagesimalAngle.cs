/*
 * File:        DateFormatterExtensions.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods to format date strings in "YYYYMMDD" format.
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
using System.Numerics;

namespace Planetoid_DB.Helpers;

/// <summary>Represents an angle as degrees, minutes, and seconds.</summary>
/// <typeparam name="T">The floating-point type used for the seconds component.</typeparam>
/// <param name="Degrees">The degrees component of the angle.</param>
/// <param name="Minutes">The minutes component of the angle.</param>
/// <param name="Seconds">The seconds component of the angle.</param>
/// <remarks>This record struct is immutable and provides a convenient way to represent angles in a sexagesimal format.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal record struct SexagesimalAngle<T>(int Degrees, int Minutes, T Seconds) where T : IFloatingPoint<T>
{
	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private readonly string DebuggerDisplay => ToString();
}
