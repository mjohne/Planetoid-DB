/*
 * File:        JplDevelopmentEphemerisTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for JPL development ephemeris binary reading.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Buffers.Binary;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for parsing and evaluating JPL development ephemeris files.</summary>
public sealed class JplDevelopmentEphemerisTests
{
	/// <summary>Verifies non-finite floating-point header values are rejected.</summary>
	[Theory]
	[InlineData(2652, "NaN")]
	[InlineData(2660, "NaN")]
	[InlineData(2668, "NaN")]
	[InlineData(2680, "NaN")]
	[InlineData(2688, "NaN")]
	[InlineData(2652, "Infinity")]
	[InlineData(2660, "Infinity")]
	[InlineData(2668, "Infinity")]
	[InlineData(2680, "Infinity")]
	[InlineData(2688, "Infinity")]
	[InlineData(2652, "-Infinity")]
	[InlineData(2660, "-Infinity")]
	[InlineData(2668, "-Infinity")]
	[InlineData(2680, "-Infinity")]
	[InlineData(2688, "-Infinity")]
	public void RejectsNonFiniteHeaderValues(int offset, string nonFiniteKind)
	{
		byte[] file = CreateFixture();
		double nonFiniteValue = nonFiniteKind switch
		{
			"NaN" => double.NaN,
			"Infinity" => double.PositiveInfinity,
			_ => double.NegativeInfinity
		};
		WriteDouble(file, offset: offset, value: nonFiniteValue);

		string filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.440");
		try
		{
			File.WriteAllBytes(path: filePath, bytes: file);
			_ = Assert.Throws<InvalidDataException>(testCode: () => new JplDevelopmentEphemeris(filePath: filePath));
		}
		finally
		{
			File.Delete(path: filePath);
		}
	}

	/// <summary>Verifies an extended constant table, record sizing and Earth/Sun position evaluation.</summary>
	[Fact]
	public void ReadsExtendedHeaderAndEvaluatesEarthPosition()
	{
		byte[] file = CreateFixture();
		const double startJulianDate = 2451545.0;
		const double kilometersPerAu = 149597870.7;
		WriteDouble(file, offset: 2652, value: startJulianDate);
		WriteDouble(file, offset: 2660, value: startJulianDate + 1.0);
		WriteDouble(file, offset: 2668, value: 1.0);
		WriteInt32(file, offset: 2676, value: 401);
		WriteDouble(file, offset: 2680, value: kilometersPerAu);
		WriteDouble(file, offset: 2688, value: 81.300569);

		const int recordBytes = 1110 * sizeof(double);
		const int dataRecordOffset = 2 * recordBytes;
		WriteDouble(file, offset: dataRecordOffset, value: startJulianDate);
		WriteDouble(file, offset: dataRecordOffset + sizeof(double), value: startJulianDate + 1.0);
		WriteDouble(file, offset: dataRecordOffset + (8 * sizeof(double)), value: kilometersPerAu);
		WriteDouble(file, offset: dataRecordOffset + (29 * sizeof(double)), value: kilometersPerAu * 0.1);

		string filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.440");
		try
		{
			File.WriteAllBytes(path: filePath, bytes: file);
			using JplDevelopmentEphemeris ephemeris = new(filePath: filePath);

			Vector3d earth = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: startJulianDate);

			Assert.Equal(expected: "JPL DE440", actual: ephemeris.Name);
			Assert.Equal(expected: 1.0 - (0.1 / (1.0 + 81.300569)), actual: earth.X, precision: 12);
			Assert.Equal(expected: 0.0, actual: earth.Y, precision: 12);
			Assert.Equal(expected: 0.0, actual: earth.Z, precision: 12);
		}
		finally
		{
			File.Delete(path: filePath);
		}
	}

	/// <summary>Creates a synthetic DE440 file with an extended constant table and valid header.</summary>
	/// <returns>The file bytes.</returns>
	private static byte[] CreateFixture()
	{
		const int recordBytes = 1110 * sizeof(double);
		byte[] file = new byte[3 * recordBytes];

		WriteDouble(file, offset: 2652, value: 2451545.0);
		WriteDouble(file, offset: 2660, value: 2451546.0);
		WriteDouble(file, offset: 2668, value: 1.0);
		WriteInt32(file, offset: 2676, value: 401);
		WriteDouble(file, offset: 2680, value: 149597870.7);
		WriteDouble(file, offset: 2688, value: 81.300569);
		for (int i = 0; i < 12; i++)
		{
			int pointerOffset = 2696 + (i * 12);
			WriteInt32(file, offset: pointerOffset, value: 3 + (i * 3));
			WriteInt32(file, offset: pointerOffset + 4, value: 1);
			WriteInt32(file, offset: pointerOffset + 8, value: 1);
		}
		WriteInt32(file, offset: 2840, value: 440);

		int extendedPointerOffset = 2856 + 6;
		WriteInt32(file, offset: extendedPointerOffset, value: 211);
		WriteInt32(file, offset: extendedPointerOffset + 4, value: 100);
		WriteInt32(file, offset: extendedPointerOffset + 8, value: 3);

		return file;
	}

	/// <summary>Writes a little-endian integer into a binary fixture.</summary>
	/// <param name="buffer">The destination bytes.</param>
	/// <param name="offset">The destination offset.</param>
	/// <param name="value">The integer value.</param>
	private static void WriteInt32(byte[] buffer, int offset, int value) =>
		BinaryPrimitives.WriteInt32LittleEndian(destination: buffer.AsSpan(start: offset), value: value);

	/// <summary>Writes a little-endian double into a binary fixture.</summary>
	/// <param name="buffer">The destination bytes.</param>
	/// <param name="offset">The destination offset.</param>
	/// <param name="value">The double value.</param>
	private static void WriteDouble(byte[] buffer, int offset, double value) =>
		BinaryPrimitives.WriteDoubleLittleEndian(destination: buffer.AsSpan(start: offset), value: value);
}
