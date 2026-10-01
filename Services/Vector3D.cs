/*
 * File:        Vector3D.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents an immutable three-dimensional double-precision vector.
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

/// <summary>Represents an immutable three-dimensional double-precision vector.</summary>
/// <param name="X">The X component.</param>
/// <param name="Y">The Y component.</param>
/// <param name="Z">The Z component.</param>
/// <remarks>The unit of the components depends on the context (usually astronomical units or AU/day).</remarks>
internal readonly record struct Vector3D(double X, double Y, double Z)
{
	/// <summary>Gets the zero vector.</summary>
	public static Vector3D Zero => new(X: 0.0, Y: 0.0, Z: 0.0);

	/// <summary>Gets the Euclidean length of the vector.</summary>
	public double Length => Math.Sqrt(d: (X * X) + (Y * Y) + (Z * Z));

	/// <summary>Adds two vectors.</summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The sum of both vectors.</returns>
	public static Vector3D operator +(Vector3D a, Vector3D b) => new(X: a.X + b.X, Y: a.Y + b.Y, Z: a.Z + b.Z);

	/// <summary>Subtracts two vectors.</summary>
	/// <param name="a">The minuend.</param>
	/// <param name="b">The subtrahend.</param>
	/// <returns>The difference of both vectors.</returns>
	public static Vector3D operator -(Vector3D a, Vector3D b) => new(X: a.X - b.X, Y: a.Y - b.Y, Z: a.Z - b.Z);

	/// <summary>Negates a vector.</summary>
	/// <param name="a">The vector.</param>
	/// <returns>The negated vector.</returns>
	public static Vector3D operator -(Vector3D a) => new(X: -a.X, Y: -a.Y, Z: -a.Z);

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3D operator *(Vector3D a, double s) => new(X: a.X * s, Y: a.Y * s, Z: a.Z * s);

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="s">The scalar.</param>
	/// <param name="a">The vector.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3D operator *(double s, Vector3D a) => a * s;

	/// <summary>Divides a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3D operator /(Vector3D a, double s) => new(X: a.X / s, Y: a.Y / s, Z: a.Z / s);

	/// <summary>Calculates the dot product of two vectors.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The dot product.</returns>
	public double Dot(Vector3D other) => (X * other.X) + (Y * other.Y) + (Z * other.Z);

	/// <summary>Calculates the cross product of two vectors.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The cross product.</returns>
	public Vector3D Cross(Vector3D other) => new(
		X: (Y * other.Z) - (Z * other.Y),
		Y: (Z * other.X) - (X * other.Z),
		Z: (X * other.Y) - (Y * other.X));

	/// <summary>Returns the unit vector pointing in the same direction.</summary>
	/// <returns>The normalized vector.</returns>
	public Vector3D Normalize() => this / Length;

	/// <summary>Calculates the angle between two vectors in radians.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The angle in radians in the range [0, π].</returns>
	/// <remarks>Uses atan2 of the cross and dot products, which is numerically stable for small and large angles.</remarks>
	public double AngleTo(Vector3D other) => Math.Atan2(y: Cross(other: other).Length, x: Dot(other: other));
}
