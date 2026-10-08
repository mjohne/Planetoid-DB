/*
 * File:        Matrix3d.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents a 3×3 rotation matrix used for coordinate frame transformations.
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

/// <summary>Represents a 3×3 matrix used for coordinate frame rotations.</summary>
/// <param name="M11">Row 1, column 1.</param>
/// <param name="M12">Row 1, column 2.</param>
/// <param name="M13">Row 1, column 3.</param>
/// <param name="M21">Row 2, column 1.</param>
/// <param name="M22">Row 2, column 2.</param>
/// <param name="M23">Row 2, column 3.</param>
/// <param name="M31">Row 3, column 1.</param>
/// <param name="M32">Row 3, column 2.</param>
/// <param name="M33">Row 3, column 3.</param>
/// <remarks>The rotation helpers follow the frame-rotation convention R1/R2/R3 of the Explanatory Supplement to the Astronomical Almanac.</remarks>
internal readonly record struct Matrix3d(double M11, double M12, double M13, double M21, double M22, double M23, double M31, double M32, double M33)
{
	/// <summary>Gets the identity matrix.</summary>
	public static Matrix3d Identity => new(M11: 1, M12: 0, M13: 0, M21: 0, M22: 1, M23: 0, M31: 0, M32: 0, M33: 1);

	/// <summary>Creates a frame rotation about the X axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix R1(angle).</returns>
	public static Matrix3d RotationX(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3d(M11: 1, M12: 0, M13: 0, M21: 0, M22: c, M23: s, M31: 0, M32: -s, M33: c);
	}

	/// <summary>Creates a frame rotation about the Y axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix R2(angle).</returns>
	public static Matrix3d RotationY(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3d(M11: c, M12: 0, M13: -s, M21: 0, M22: 1, M23: 0, M31: s, M32: 0, M33: c);
	}

	/// <summary>Creates a frame rotation about the Z axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix R3(angle).</returns>
	public static Matrix3d RotationZ(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3d(M11: c, M12: s, M13: 0, M21: -s, M22: c, M23: 0, M31: 0, M32: 0, M33: 1);
	}

	/// <summary>Multiplies two matrices.</summary>
	/// <param name="a">The left matrix.</param>
	/// <param name="b">The right matrix.</param>
	/// <returns>The product <paramref name="a"/> · <paramref name="b"/>.</returns>
	public static Matrix3d operator *(Matrix3d a, Matrix3d b) => new(
		M11: (a.M11 * b.M11) + (a.M12 * b.M21) + (a.M13 * b.M31),
		M12: (a.M11 * b.M12) + (a.M12 * b.M22) + (a.M13 * b.M32),
		M13: (a.M11 * b.M13) + (a.M12 * b.M23) + (a.M13 * b.M33),
		M21: (a.M21 * b.M11) + (a.M22 * b.M21) + (a.M23 * b.M31),
		M22: (a.M21 * b.M12) + (a.M22 * b.M22) + (a.M23 * b.M32),
		M23: (a.M21 * b.M13) + (a.M22 * b.M23) + (a.M23 * b.M33),
		M31: (a.M31 * b.M11) + (a.M32 * b.M21) + (a.M33 * b.M31),
		M32: (a.M31 * b.M12) + (a.M32 * b.M22) + (a.M33 * b.M32),
		M33: (a.M31 * b.M13) + (a.M32 * b.M23) + (a.M33 * b.M33));

	/// <summary>Applies the matrix to a vector.</summary>
	/// <param name="m">The matrix.</param>
	/// <param name="v">The vector.</param>
	/// <returns>The transformed vector.</returns>
	public static Vector3d operator *(Matrix3d m, Vector3d v) => new(
		X: (m.M11 * v.X) + (m.M12 * v.Y) + (m.M13 * v.Z),
		Y: (m.M21 * v.X) + (m.M22 * v.Y) + (m.M23 * v.Z),
		Z: (m.M31 * v.X) + (m.M32 * v.Y) + (m.M33 * v.Z));

	/// <summary>Returns the transposed matrix (the inverse for a rotation matrix).</summary>
	/// <returns>The transposed matrix.</returns>
	public Matrix3d Transpose() => new(M11: M11, M12: M21, M13: M31, M21: M12, M22: M22, M23: M32, M31: M13, M32: M23, M33: M33);
}
