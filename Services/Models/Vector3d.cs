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

namespace Planetoid_DB.Services;

/// <summary>Represents a three-dimensional Cartesian vector with double precision.</summary>
/// <param name="X">The X component.</param>
/// <param name="Y">The Y component.</param>
/// <param name="Z">The Z component.</param>
/// <remarks>The unit of the components depends on the context (usually astronomical units [AU] or AU/day).</remarks>
internal readonly record struct Vector3d(double X, double Y, double Z)
{
	/// <summary>Gets the zero vector.</summary>
	public static Vector3d Zero => new(X: 0.0, Y: 0.0, Z: 0.0);

	/// <summary>Gets the Euclidean length of the vector.</summary>
	public double Length => Math.Sqrt(d: (X * X) + (Y * Y) + (Z * Z));

	/// <summary>Adds two vectors.</summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The sum of both vectors.</returns>
	public static Vector3d operator +(Vector3d a, Vector3d b) => new(X: a.X + b.X, Y: a.Y + b.Y, Z: a.Z + b.Z);

	/// <summary>Subtracts two vectors.</summary>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector.</param>
	/// <returns>The difference <paramref name="a"/> − <paramref name="b"/>.</returns>
	public static Vector3d operator -(Vector3d a, Vector3d b) => new(X: a.X - b.X, Y: a.Y - b.Y, Z: a.Z - b.Z);

	/// <summary>Negates a vector.</summary>
	/// <param name="a">The vector.</param>
	/// <returns>The negated vector.</returns>
	public static Vector3d operator -(Vector3d a) => new(X: -a.X, Y: -a.Y, Z: -a.Z);

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3d operator *(Vector3d a, double s) => new(X: a.X * s, Y: a.Y * s, Z: a.Z * s);

	/// <summary>Multiplies a vector by a scalar.</summary>
	/// <param name="s">The scalar.</param>
	/// <param name="a">The vector.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3d operator *(double s, Vector3d a) => a * s;

	/// <summary>Divides a vector by a scalar.</summary>
	/// <param name="a">The vector.</param>
	/// <param name="s">The scalar divisor.</param>
	/// <returns>The scaled vector.</returns>
	public static Vector3d operator /(Vector3d a, double s) => new(X: a.X / s, Y: a.Y / s, Z: a.Z / s);

	/// <summary>Computes the dot product of two vectors.</summary>
	/// <param name="other">The other vector.</param>
	/// <returns>The scalar product.</returns>
	public double Dot(Vector3d other) => (X * other.X) + (Y * other.Y) + (Z * other.Z);

	/// <summary>Returns the unit vector pointing in the same direction.</summary>
	/// <returns>The normalized vector, or <see cref="Zero"/> if the length is zero.</returns>
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
}
