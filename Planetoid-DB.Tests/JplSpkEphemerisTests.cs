/*
 * File:        JplSpkEphemerisTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Tests the JPL SPK (DE440/DE441) reader with a synthetic file.
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
using System.Text;

using Planetoid_DB.Services;

namespace Planetoid_DB.Tests;

/// <summary>Tests the JPL SPK (DE440/DE441) reader with a synthetic file.</summary>
public sealed class JplSpkEphemerisTests : IDisposable
{
	/// <summary>Half the covered interval in seconds.</summary>
	private const double Radius = 1.0e6;

	/// <summary>The temporary file path.</summary>
	private readonly string path = Path.Combine(path1: Path.GetTempPath(), path2: $"planetoid-db-test-{Guid.NewGuid():N}.bsp");

	/// <summary>Deletes the temporary file.</summary>
	public void Dispose()
	{
		if (File.Exists(path: path))
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Writes a little-endian DAF/SPK file with type 2 segments centred on J2000.</summary>
	/// <param name="segments">Target, center and the three Chebyshev coefficients per axis (x0, x1, x2, y0, …).</param>
	private void WriteSpk(params (int Target, int Center, double[] Coefficients)[] segments)
	{
		const int coefficientsPerAxis = 3;
		const int recordSize = 2 + (3 * coefficientsPerAxis);
		byte[] file = new byte[3 * 1024];
		Encoding.ASCII.GetBytes(s: "DAF/SPK ").CopyTo(array: file, index: 0);
		BinaryPrimitives.WriteInt32LittleEndian(destination: file.AsSpan(start: 8), value: 2);
		BinaryPrimitives.WriteInt32LittleEndian(destination: file.AsSpan(start: 12), value: 6);
		BinaryPrimitives.WriteInt32LittleEndian(destination: file.AsSpan(start: 76), value: 2);
		BinaryPrimitives.WriteInt32LittleEndian(destination: file.AsSpan(start: 80), value: 2);
		Encoding.ASCII.GetBytes(s: "LTL-IEEE").CopyTo(array: file, index: 88);
		// Summary record (record 2)
		Span<byte> summary = file.AsSpan(start: 1024, length: 1024);
		BinaryPrimitives.WriteDoubleLittleEndian(destination: summary, value: 0.0);
		BinaryPrimitives.WriteDoubleLittleEndian(destination: summary[8..], value: 0.0);
		BinaryPrimitives.WriteDoubleLittleEndian(destination: summary[16..], value: segments.Length);
		List<double> data = [];
		int address = (3 * 128) + 1;
		for (int i = 0; i < segments.Length; i++)
		{
			int begin = address + data.Count;
			data.AddRange(collection: [0.0, Radius, .. segments[i].Coefficients]);
			data.AddRange(collection: [-Radius, 2.0 * Radius, recordSize, 1.0]);
			int end = address + data.Count - 1;
			Span<byte> s = summary[(24 + (i * 40))..];
			BinaryPrimitives.WriteDoubleLittleEndian(destination: s, value: -Radius);
			BinaryPrimitives.WriteDoubleLittleEndian(destination: s[8..], value: Radius);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[16..], value: segments[i].Target);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[20..], value: segments[i].Center);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[24..], value: 1);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[28..], value: 2);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[32..], value: begin);
			BinaryPrimitives.WriteInt32LittleEndian(destination: s[36..], value: end);
		}
		using FileStream stream = File.Create(path: path);
		stream.Write(buffer: file);
		Span<byte> buffer = stackalloc byte[8];
		foreach (double value in data)
		{
			BinaryPrimitives.WriteDoubleLittleEndian(destination: buffer, value: value);
			stream.Write(buffer: buffer);
		}
	}

	/// <summary>Positions are evaluated from the Chebyshev coefficients and chained via the barycenter.</summary>
	[Fact]
	public void GetHeliocentricPosition_EvaluatesChebyshevChain()
	{
		const double au = AstronomicalConstants.AstronomicalUnitKm;
		WriteSpk(
			(10, 0, [0.01 * au, 0, 0, 0, 0, 0, 0, 0, 0]),
			(3, 0, [au, 0.1 * au, 0, 0, 0.2 * au, 0, 0, 0, 0.05 * au]),
			(399, 3, [1000.0, 0, 0, 0, 0, 0, 0, 0, 0]));
		using JplSpkEphemeris ephemeris = new(path);
		// s = 0.5 → T1 = 0.5, T2 = 2·0.25 − 1 = −0.5
		double jd = AstronomicalConstants.JulianDateJ2000 + (0.5 * Radius / AstronomicalConstants.SecondsPerDay);
		Vector3D emb = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: jd);
		Assert.Equal(expected: 1.05 - 0.01, actual: emb.X, precision: 9);
		Assert.Equal(expected: 0.1, actual: emb.Y, precision: 9);
		Assert.Equal(expected: -0.025, actual: emb.Z, precision: 9);
		Vector3D earth = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Earth, julianDateTdb: jd);
		Assert.Equal(expected: emb.X + (1000.0 / au), actual: earth.X, precision: 9);
		Assert.Equal(expected: AstronomicalConstants.JulianDateJ2000 - (Radius / AstronomicalConstants.SecondsPerDay), actual: ephemeris.GetCoverage().FirstJulianDateTdb, precision: 9);
	}

	/// <summary>Times outside the coverage and missing bodies are rejected.</summary>
	[Fact]
	public void GetHeliocentricPosition_OutsideCoverage_Throws()
	{
		WriteSpk((10, 0, new double[9]), (3, 0, new double[9]));
		using JplSpkEphemeris ephemeris = new(path);
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => ephemeris.GetHeliocentricPosition(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: AstronomicalConstants.JulianDateJ2000 + 100.0));
		_ = Assert.Throws<ArgumentOutOfRangeException>(testCode: () => ephemeris.GetHeliocentricPosition(body: SolarSystemBody.Mars, julianDateTdb: AstronomicalConstants.JulianDateJ2000));
	}

	/// <summary>The later segment is selected at an exact shared segment boundary.</summary>
	[Fact]
	public void GetHeliocentricPosition_AtSegmentBoundary_LaterSegmentTakesPrecedence()
	{
		double au = AstronomicalConstants.AstronomicalUnitKm;
		WriteSpk((10, 0, new double[9]), (3, 0, [au, 0, 0, 0, 0, 0, 0, 0, 0]), (3, 0, [2.0 * au, 0, 0, 0, 0, 0, 0, 0, 0]));
		using JplSpkEphemeris ephemeris = new(path);
		double boundary = Math.BitDecrement(x: AstronomicalConstants.JulianDateJ2000 + (Radius / AstronomicalConstants.SecondsPerDay));
		Vector3D position = ephemeris.GetHeliocentricPosition(body: SolarSystemBody.EarthMoonBarycenter, julianDateTdb: boundary);
		Assert.Equal(expected: 2.0, actual: position.X, precision: 9);
	}

	/// <summary>Files that are not SPK files are rejected.</summary>
	[Fact]
	public void Constructor_NotAnSpkFile_Throws()
	{
		File.WriteAllBytes(path: path, bytes: new byte[2048]);
		_ = Assert.Throws<InvalidDataException>(testCode: () => new JplSpkEphemeris(path));
		_ = Assert.Throws<ArgumentException>(testCode: () => new JplSpkEphemeris());
	}

	/// <summary>Malformed summary record counts are rejected as invalid data.</summary>
	/// <param name="summaryCount">The malformed summary count.</param>
	[Theory]
	[InlineData(-1.0)]
	[InlineData(1.5)]
	[InlineData(26.0)]
	[InlineData(double.NaN)]
	public void Constructor_InvalidSummaryCount_Throws(double summaryCount)
	{
		WriteSpk((10, 0, new double[9]));
		using (FileStream stream = new(path, FileMode.Open, FileAccess.Write, FileShare.None))
		{
			stream.Position = 1024 + 16;
			Span<byte> bytes = stackalloc byte[sizeof(double)];
			BinaryPrimitives.WriteDoubleLittleEndian(destination: bytes, value: summaryCount);
			stream.Write(buffer: bytes);
		}
		_ = Assert.Throws<InvalidDataException>(testCode: () => new JplSpkEphemeris(path));
	}
}
