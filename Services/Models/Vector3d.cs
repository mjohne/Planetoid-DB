/*
 * File:        Vector3d.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents a three-dimensional Cartesian vector with double precision.
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

namespace Planetoid_DB.Services;

/// <summary>Represents a three-dimensional Cartesian vector with double precision.</summary>
/// <param name="X">The X component.</param>
/// <param name="Y">The Y component.</param>
/// <param name="Z">The Z component.</param>
/// <remarks>The unit of the components depends on the context (usually astronomical units [AU] or AU/day).</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal readonly record struct Vector3d(double X, double Y, double Z)
{
	/// <summary>Gets the zero vector.</summary>
	/// <remarks>The zero vector has all components equal to zero and represents the origin in three-dimensional space.</remarks>
	public static Vector3d Zero => new(X: 0.0, Y: 0.0, Z: 0.0);

	/// <summary>Gets the Euclidean length of the vector.</summary>
	/// <remarks>The length is computed as the square root of the sum of the squares of the components: √(X² + Y² + Z²).</remarks>
	public double Length => Math.Sqrt(d: (X * X) + (Y * Y) + (Z * Z));

	/// <summary>Adds two vectors.</summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The sum of both vectors.</returns>
	/// <remarks>The addition is performed component-wise: (X₁ + X₂, Y₁ + Y₂, Z₁ + Z₂).</remarks>
	public static Vector3d operator +(Vector3d a, Vector3d b)
	{
		return new(X: a.X + b.X, Y: a.Y + b.Y, Z: a.Z + b.Z);
	}

	/// <summary>Subtracts two vectors.</summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The difference <paramref name="a"/> − <paramref name="b"/>.</returns>
	/// <remarks>The subtraction is performed component-wise: (X₁ − X₂, Y₁ − Y₂, Z₁ − Z₂).</remarks>
	public static Vector3d operator -(Vector3d a, Vector3d b)
	{
		return new(X: a.X - b.X, Y: a.Y - b.Y, Z: a.Z - b.Z);
	}

	/// <summary>Negates a vector.</summary>
	/// <param name="a">The vector.</param>
	/// <returns>The negated vector.</returns>
	/// <remarks>The negation is performed component-wise: (−X, −Y, −Z).</remarks>
	public static Vector3d operator -(Vector3d a)
	{
		return new(X: -a.X, Y: -a.Y, Z: -a.Z);
	}

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	/// <remarks>The multiplication is performed component-wise: (X * s, Y * s, Z * s).</remarks>
	public static Vector3d operator *(Vector3d a, double s)
	{
		return new(X: a.X * s, Y: a.Y * s, Z: a.Z * s);
	}

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="s">The scalar.</param>
	/// <param name="a">The vector.</param>
	/// <returns>The scaled vector.</returns>
	/// <remarks>The multiplication is performed component-wise: (X * s, Y * s, Z * s).</remarks>
	public static Vector3d operator *(double s, Vector3d a)
	{
		return a * s;
	}

	/// <summary>Divides a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar divisor.</param>
	/// <returns>The scaled vector.</returns>
	/// <remarks>The division is performed component-wise: (X / s, Y / s, Z / s). Throws <see cref="DivideByZeroException"/> if <paramref name="s"/> is zero.</remarks>
	public static Vector3d operator /(Vector3d a, double s)
	{
		return new(X: a.X / s, Y: a.Y / s, Z: a.Z / s);
	}

	/// <summary>Computes the dot product of two vectors.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The scalar product.</returns>
	/// <remarks>The dot product is computed as X₁·X₂ + Y₁·Y₂ + Z₁·Z₂.</remarks>
	public double Dot(Vector3d other)
	{
		return (X * other.X) + (Y * other.Y) + (Z * other.Z);
	}

	/// <summary>Returns the unit vector pointing in the same direction.</summary>
	/// <returns>The normalized vector, or <see cref="Zero"/> if the length is zero.</returns>
	/// <remarks>The normalization is performed by dividing the vector by its length.</remarks>
	public Vector3d Normalize()
	{
		double length = Length;
		return length > 0.0 ? this / length : Zero;
	}

	/// <summary>Computes the angle between two vectors in degrees.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The angle in degrees in the range [0°, 180°].</returns>
	/// <remarks>Uses the numerically stable atan2 formulation.</remarks>
	public double AngleToDegrees(Vector3d other)
	{
		double cx = (Y * other.Z) - (Z * other.Y);
		double cy = (Z * other.X) - (X * other.Z);
		double cz = (X * other.Y) - (Y * other.X);
		double cross = Math.Sqrt(d: (cx * cx) + (cy * cy) + (cz * cz));
		return Math.Atan2(y: cross, x: Dot(other: other)) * 180.0 / Math.PI;
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
