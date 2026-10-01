/*
 * File:        Matrix3D.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Represents an immutable 3x3 rotation matrix.
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

/// <summary>Represents an immutable 3x3 matrix used for frame rotations.</summary>
/// <remarks>Rotation matrices follow the passive convention of the Explanatory Supplement to the Astronomical Almanac (the frame is rotated, not the vector).</remarks>
internal readonly struct Matrix3D
{
	/// <summary>Row-major storage of the nine matrix elements.</summary>
	private readonly double[] m;

	/// <summary>Initializes a new instance of the <see cref="Matrix3D"/> struct.</summary>
	/// <param name="m11">Element row 1, column 1.</param>
	/// <param name="m12">Element row 1, column 2.</param>
	/// <param name="m13">Element row 1, column 3.</param>
	/// <param name="m21">Element row 2, column 1.</param>
	/// <param name="m22">Element row 2, column 2.</param>
	/// <param name="m23">Element row 2, column 3.</param>
	/// <param name="m31">Element row 3, column 1.</param>
	/// <param name="m32">Element row 3, column 2.</param>
	/// <param name="m33">Element row 3, column 3.</param>
	public Matrix3D(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33) => m = [m11, m12, m13, m21, m22, m23, m31, m32, m33];

	/// <summary>Gets the identity matrix.</summary>
	public static Matrix3D Identity => new(m11: 1, m12: 0, m13: 0, m21: 0, m22: 1, m23: 0, m31: 0, m32: 0, m33: 1);

	/// <summary>Creates a passive rotation about the X axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix.</returns>
	public static Matrix3D RotationX(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3D(m11: 1, m12: 0, m13: 0, m21: 0, m22: c, m23: s, m31: 0, m32: -s, m33: c);
	}

	/// <summary>Creates a passive rotation about the Y axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix.</returns>
	public static Matrix3D RotationY(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3D(m11: c, m12: 0, m13: -s, m21: 0, m22: 1, m23: 0, m31: s, m32: 0, m33: c);
	}

	/// <summary>Creates a passive rotation about the Z axis.</summary>
	/// <param name="angleRadians">The rotation angle in radians.</param>
	/// <returns>The rotation matrix.</returns>
	public static Matrix3D RotationZ(double angleRadians)
	{
		(double s, double c) = Math.SinCos(x: angleRadians);
		return new Matrix3D(m11: c, m12: s, m13: 0, m21: -s, m22: c, m23: 0, m31: 0, m32: 0, m33: 1);
	}

	/// <summary>Returns the transposed matrix (the inverse of a rotation matrix).</summary>
	/// <returns>The transposed matrix.</returns>
	public Matrix3D Transpose() => new(m11: m[0], m12: m[3], m13: m[6], m21: m[1], m22: m[4], m23: m[7], m31: m[2], m32: m[5], m33: m[8]);

	/// <summary>Multiplies two matrices.</summary>
	/// <param name="a">The left matrix.</param>
	/// <param name="b">The right matrix.</param>
	/// <returns>The product <c>a · b</c>.</returns>
	public static Matrix3D operator *(Matrix3D a, Matrix3D b)
	{
		double[] r = new double[9];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				r[(i * 3) + j] = (a.m[i * 3] * b.m[j]) + (a.m[(i * 3) + 1] * b.m[3 + j]) + (a.m[(i * 3) + 2] * b.m[6 + j]);
			}
		}
		return new Matrix3D(m11: r[0], m12: r[1], m13: r[2], m21: r[3], m22: r[4], m23: r[5], m31: r[6], m32: r[7], m33: r[8]);
	}

	/// <summary>Multiplies a matrix with a column vector.</summary>
	/// <param name="a">The matrix.</param>
	/// <param name="v">The vector.</param>
	/// <returns>The transformed vector.</returns>
	public static Vector3D operator *(Matrix3D a, Vector3D v) => new(
		X: (a.m[0] * v.X) + (a.m[1] * v.Y) + (a.m[2] * v.Z),
		Y: (a.m[3] * v.X) + (a.m[4] * v.Y) + (a.m[5] * v.Z),
		Z: (a.m[6] * v.X) + (a.m[7] * v.Y) + (a.m[8] * v.Z));
}
