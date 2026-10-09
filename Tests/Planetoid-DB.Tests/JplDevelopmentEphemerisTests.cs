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
using System.Diagnostics;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for parsing and evaluating JPL development ephemeris files.</summary>
/// <remarks>These tests create synthetic DE440 files with valid and invalid headers to verify the reader's behavior.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class JplDevelopmentEphemerisTests
{
	/// <summary>Verifies non-finite floating-point header values are rejected.</summary>
	/// <remarks>This test creates a synthetic DE440 file with non-finite values in the header and verifies that the reader throws an <see cref="InvalidDataException"/>.</remarks>
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
		// Create a synthetic DE440 file with a non-finite value in the header at the specified offset.
		byte[] file = CreateFixture();
		// Write the non-finite value (NaN, Infinity, or -Infinity) into the header at the specified offset.
		double nonFiniteValue = nonFiniteKind switch
		{
			"NaN" => double.NaN,
			"Infinity" => double.PositiveInfinity,
			_ => double.NegativeInfinity
		};
		// Write the non-finite value into the header at the specified offset.
		WriteDouble(file, offset: offset, value: nonFiniteValue);
		// Write the synthetic DE440 file to a temporary file and verify that the reader throws an InvalidDataException.
		string filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.440");
		// Write the synthetic DE440 file to a temporary file and verify that the reader throws an InvalidDataException.
		try
		{
			// Write the synthetic DE440 file to a temporary file.
			File.WriteAllBytes(path: filePath, bytes: file);
			// Verify that the JPL development ephemeris reader throws an InvalidDataException when reading the file with a non-finite header value.
			_ = Assert.Throws<InvalidDataException>(testCode: () => new JplDevelopmentEphemeris(filePath: filePath));
		}
		// Clean up the temporary file after the test.
		finally
		{
			// Delete the temporary file to avoid leaving test artifacts.
			File.Delete(path: filePath);
		}
	}

	/// <summary>Verifies an extended constant table, record sizing and Earth/Sun position evaluation.</summary>
	/// <remarks>This test creates a synthetic DE440 file with an extended constant table and valid header, and verifies that the reader can parse it and evaluate the Earth position correctly.</remarks>
	[Fact]
	public void ReadsExtendedHeaderAndEvaluatesEarthPosition()
	{
		// Create a synthetic DE440 file with an extended constant table and valid header.
		byte[] file = CreateFixture();
		// The start Julian date is 2451545.0 (J2000 epoch), and the end Julian date is one day later.
		const double startJulianDate = 2451545.0;
		// The DE440 header contains the AU in kilometers and the Earth/Moon mass ratio, which we set to known values for testing.
		const double kilometersPerAu = 149597870.7;
		// The Earth/Moon mass ratio is approximately 81.300569, which we write to the header.
		WriteDouble(file, offset: 2652, value: startJulianDate);
		// The end Julian date is one day later than the start date.
		WriteDouble(file, offset: 2660, value: startJulianDate + 1.0);
		// The time step is 1.0 day.
		WriteDouble(file, offset: 2668, value: 1.0);
		// The number of constants in the extended constant table is 401, which we write to the header.
		WriteInt32(file, offset: 2676, value: 401);
		// The DE440 header contains the AU in kilometers and the Earth/Moon mass ratio, which we set to known values for testing.
		WriteDouble(file, offset: 2680, value: kilometersPerAu);
		// The Earth/Moon mass ratio is approximately 81.300569, which we write to the header.
		WriteDouble(file, offset: 2688, value: 81.300569);
		// The DE440 header contains 12 pointers to the data records, which we set to point to the correct offsets for each record.
		const int recordBytes = 1110 * sizeof(double);
		// The first pointer is 3 + (i * 3), which we write to the header.
		const int dataRecordOffset = 2 * recordBytes;
		// The second pointer is 1, which we write to the header.
		WriteDouble(file, offset: dataRecordOffset, value: startJulianDate);
		// The third pointer is 1, which we write to the header.
		WriteDouble(file, offset: dataRecordOffset + sizeof(double), value: startJulianDate + 1.0);
		// The X coefficient for the Earth–Moon barycenter is set to one AU in kilometers.
		WriteDouble(file, offset: dataRecordOffset + (8 * sizeof(double)), value: kilometersPerAu);
		// The X coefficient for the geocentric Moon is set to 0.1 AU in kilometers.
		WriteDouble(file, offset: dataRecordOffset + (29 * sizeof(double)), value: kilometersPerAu * 0.1);
		string filePath = Path.Combine(path1: Path.GetTempPath(), path2: $"{Guid.NewGuid():N}.440");
		// Write the synthetic DE440 file to a temporary file and test the JPL development ephemeris reader.
		try
		{
			// Write the synthetic DE440 file to a temporary file.
			File.WriteAllBytes(path: filePath, bytes: file);
			// Create a JPL development ephemeris reader and evaluate the Earth position at the start Julian date.
			using JplDevelopmentEphemeris ephemeris = new(filePath: filePath);
			// Evaluate the Earth position at the start Julian date.
			Vector3d earth = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: startJulianDate);
			// Verify that the ephemeris name and Earth position are as expected.
			Assert.Equal(expected: "JPL DE440", actual: ephemeris.Name);
			Assert.Equal(expected: 1.0 - (0.1 / (1.0 + 81.300569)), actual: earth.X, precision: 12);
			Assert.Equal(expected: 0.0, actual: earth.Y, precision: 12);
			Assert.Equal(expected: 0.0, actual: earth.Z, precision: 12);
		}
		// Clean up the temporary file after the test.
		finally
		{
			// Delete the temporary file to avoid leaving test artifacts.
			File.Delete(path: filePath);
		}
	}

	/// <summary>Creates a synthetic DE440 file with an extended constant table and valid header.</summary>
	/// <returns>The file bytes.</returns>
	/// <remarks>This fixture is used to test the JPL development ephemeris reader without requiring a full DE440 file.</remarks>
	private static byte[] CreateFixture()
	{
		// The DE440 header is 1110 doubles (8880 bytes) long, and we create three records to ensure the reader can handle multiple records.
		const int recordBytes = 1110 * sizeof(double);
		// The file will contain three records, so the total size is 3 * recordBytes.
		byte[] file = new byte[3 * recordBytes];
		// Write the header values for the extended constant table and other required fields.
		WriteDouble(file, offset: 2652, value: 2451545.0);
		// The end Julian date is one day later than the start date.
		WriteDouble(file, offset: 2660, value: 2451546.0);
		// The time step is 1.0 day.
		WriteDouble(file, offset: 2668, value: 1.0);
		// The number of constants in the extended constant table is 401, which we write to the header.
		WriteInt32(file, offset: 2676, value: 401);
		// The DE440 header contains the AU in kilometers and the Earth/Moon mass ratio, which we set to known values for testing.
		WriteDouble(file, offset: 2680, value: 149597870.7);
		// The Earth/Moon mass ratio is approximately 81.300569, which we write to the header.
		WriteDouble(file, offset: 2688, value: 81.300569);
		for (int i = 0; i < 12; i++)
		{
			// The DE440 header contains 12 pointers to the data records, which we set to point to the correct offsets for each record.
			int pointerOffset = 2696 + (i * 12);
			// The first pointer is 3 + (i * 3), which we write to the header.
			WriteInt32(file, offset: pointerOffset, value: 3 + (i * 3));
			// The second pointer is 1, which we write to the header.
			WriteInt32(file, offset: pointerOffset + 4, value: 1);
			// The third pointer is 1, which we write to the header.
			WriteInt32(file, offset: pointerOffset + 8, value: 1);
		}
		// The record size is 440 doubles, which we write to the header.
		WriteInt32(file, offset: 2840, value: 440);
		// The extended constant table starts at offset 2856, and we write three constants for testing.
		int extendedPointerOffset = 2856 + 6;
		// The first constant is 211, the second is 100, and the third is 3, which we write to the extended constant table.
		WriteInt32(file, offset: extendedPointerOffset, value: 211);
		// The second constant is 100, which we write to the extended constant table.
		WriteInt32(file, offset: extendedPointerOffset + 4, value: 100);
		// The third constant is 3, which we write to the extended constant table.
		WriteInt32(file, offset: extendedPointerOffset + 8, value: 3);
		return file;
	}

	/// <summary>Writes a little-endian integer into a binary fixture.</summary>
	/// <param name="buffer">The destination bytes.</param>
	/// <param name="offset">The destination offset.</param>
	/// <param name="value">The integer value.</param>
	/// <remarks>BinaryPrimitives.WriteInt32LittleEndian is available in .NET 6 and later.</remarks>
	private static void WriteInt32(byte[] buffer, int offset, int value)
	{
		// Use BinaryPrimitives to write the integer in little-endian format
		BinaryPrimitives.WriteInt32LittleEndian(destination: buffer.AsSpan(start: offset), value: value);
	}

	/// <summary>Writes a little-endian double into a binary fixture.</summary>
	/// <param name="buffer">The destination bytes.</param>
	/// <param name="offset">The destination offset.</param>
	/// <param name="value">The double value.</param>
	/// <remarks>BinaryPrimitives.WriteDoubleLittleEndian is available in .NET 6 and later.</remarks>
	private static void WriteDouble(byte[] buffer, int offset, double value)
	{
		// Use BinaryPrimitives to write the double in little-endian format
		BinaryPrimitives.WriteDoubleLittleEndian(destination: buffer.AsSpan(start: offset), value: value);
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
