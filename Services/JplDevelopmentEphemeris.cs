/*
 * File:        JplDevelopmentEphemeris.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Reads JPL Development Ephemeris binary files (e.g. DE440, DE441).
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

namespace Planetoid_DB.Services;

/// <summary>Reads JPL Development Ephemeris files in the classic little-endian binary format (e.g. <c>linux_p1550p2650.440</c>, <c>linux_m13000p17000.441</c>).</summary>
/// <remarks>
/// The files can be downloaded from https://ssd.jpl.nasa.gov/ftp/eph/planets/Linux/. Positions are evaluated from the Chebyshev
/// coefficients and returned in the ICRF in astronomical units. DE440 covers 1550–2650, DE441 covers −13200 to +17191.
/// </remarks>
internal sealed class JplDevelopmentEphemeris : IPlanetaryEphemerisProvider, IDisposable
{
	/// <summary>Size of the fixed part of the header [bytes].</summary>
	private const int FixedHeaderSize = 2856;

	/// <summary>Index of the Earth–Moon barycenter in the coefficient pointer table.</summary>
	private const int EmbIndex = 2;

	/// <summary>Index of the Moon (geocentric) in the coefficient pointer table.</summary>
	private const int MoonIndex = 9;

	/// <summary>Index of the Sun (barycentric) in the coefficient pointer table.</summary>
	private const int SunIndex = 10;

	/// <summary>The underlying file stream.</summary>
	private readonly FileStream stream;

	/// <summary>Coefficient pointer table: offset (1-based), number of coefficients, number of sub-intervals for each item.</summary>
	private readonly int[,] pointers = new int[12, 3];

	/// <summary>Number of doubles per data record.</summary>
	private readonly int coefficientsPerRecord;

	/// <summary>Length of a data block [d].</summary>
	private readonly double blockLengthDays;

	/// <summary>Kilometers per astronomical unit as stored in the file.</summary>
	private readonly double kilometersPerAu;

	/// <summary>Earth/Moon mass ratio as stored in the file.</summary>
	private readonly double earthMoonRatio;

	/// <summary>Synchronization object for the record cache.</summary>
	private readonly Lock sync = new();

	/// <summary>The cached data record.</summary>
	private readonly double[] record;

	/// <summary>The index of the cached data record, or −1.</summary>
	private long cachedRecordIndex = -1;

	/// <summary>Initializes a new instance of the <see cref="JplDevelopmentEphemeris"/> class.</summary>
	/// <param name="filePath">The path of the binary DE file.</param>
	/// <exception cref="InvalidDataException">Thrown when the file is not a valid little-endian JPL DE binary file.</exception>
	public JplDevelopmentEphemeris(string filePath)
	{
		stream = new FileStream(path: filePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 1, options: FileOptions.RandomAccess);
		try
		{
			byte[] header = new byte[FixedHeaderSize + (6 * 1000) + 24];
			int read = stream.ReadAtLeast(buffer: header, minimumBytes: FixedHeaderSize, throwOnEndOfStream: false);
			if (read < FixedHeaderSize)
			{
				throw new InvalidDataException(message: "The file is too short to be a JPL DE binary file.");
			}
			Span<byte> h = header.AsSpan(start: 0, length: read);
			StartJulianDate = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2652..]);
			EndJulianDate = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2660..]);
			blockLengthDays = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2668..]);
			int constantCount = BinaryPrimitives.ReadInt32LittleEndian(source: h[2676..]);
			kilometersPerAu = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2680..]);
			earthMoonRatio = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2688..]);
			for (int i = 0; i < 12; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					pointers[i, j] = BinaryPrimitives.ReadInt32LittleEndian(source: h[(2696 + (((i * 3) + j) * 4))..]);
				}
			}
			int deNumber = BinaryPrimitives.ReadInt32LittleEndian(source: h[2840..]);
			if (deNumber is < 100 or > 1000 || constantCount is < 0 or > 1000 || blockLengthDays <= 0.0 || EndJulianDate <= StartJulianDate ||
				kilometersPerAu is < 1.4e8 or > 1.6e8 || earthMoonRatio is < 80.0 or > 82.0)
			{
				throw new InvalidDataException(message: "The file is not a valid little-endian JPL DE binary file.");
			}
			Name = $"JPL DE{deNumber.ToString(provider: System.Globalization.CultureInfo.InvariantCulture)}";
			// Determine the record length from the largest coefficient pointer (bodies: 3 components, nutations: 2 components)
			int maxEnd = 0;
			for (int i = 0; i < 12; i++)
			{
				int components = i == 11 ? 2 : 3;
				maxEnd = Math.Max(val1: maxEnd, val2: pointers[i, 0] - 1 + (pointers[i, 1] * components * pointers[i, 2]));
			}
			// Lunar librations (third pointer triple stored after the DE number)
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: 2844, components: 3));
			int extraOffset = FixedHeaderSize + (Math.Max(val1: 0, val2: constantCount - 400) * 6);
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: extraOffset, components: 3));
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: extraOffset + 12, components: 1));
			if (maxEnd is < 3 or > 100000)
			{
				throw new InvalidDataException(message: "The coefficient layout of the JPL DE file is invalid.");
			}
			coefficientsPerRecord = maxEnd;
			record = new double[coefficientsPerRecord];
			// Verify the layout: the first data record must start at the start date of the ephemeris
			ReadRecord(recordIndex: 0);
			if (Math.Abs(value: record[0] - StartJulianDate) > 1e-6 || Math.Abs(value: record[1] - record[0] - blockLengthDays) > 1e-6)
			{
				throw new InvalidDataException(message: "The data records of the JPL DE file could not be located.");
			}
		}
		catch
		{
			stream.Dispose();
			throw;
		}
	}

	/// <inheritdoc/>
	public string Name { get; }

	/// <inheritdoc/>
	public double StartJulianDate { get; }

	/// <inheritdoc/>
	public double EndJulianDate { get; }

	/// <inheritdoc/>
	public Vector3d GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		lock (sync)
		{
			Vector3d sun = Evaluate(item: SunIndex, julianDateTdb: julianDateTdb);
			Vector3d barycentric = body switch
			{
				SolarSystemBody.Mercury => Evaluate(item: 0, julianDateTdb: julianDateTdb),
				SolarSystemBody.Venus => Evaluate(item: 1, julianDateTdb: julianDateTdb),
				SolarSystemBody.EarthMoonBarycenter => Evaluate(item: EmbIndex, julianDateTdb: julianDateTdb),
				SolarSystemBody.Earth => Evaluate(item: EmbIndex, julianDateTdb: julianDateTdb) - (Evaluate(item: MoonIndex, julianDateTdb: julianDateTdb) / (1.0 + earthMoonRatio)),
				SolarSystemBody.Mars => Evaluate(item: 3, julianDateTdb: julianDateTdb),
				SolarSystemBody.Jupiter => Evaluate(item: 4, julianDateTdb: julianDateTdb),
				SolarSystemBody.Saturn => Evaluate(item: 5, julianDateTdb: julianDateTdb),
				SolarSystemBody.Uranus => Evaluate(item: 6, julianDateTdb: julianDateTdb),
				SolarSystemBody.Neptune => Evaluate(item: 7, julianDateTdb: julianDateTdb),
				_ => throw new ArgumentOutOfRangeException(paramName: nameof(body), actualValue: body, message: "Unsupported body.")
			};
			return (barycentric - sun) / kilometersPerAu;
		}
	}

	/// <inheritdoc/>
	public Vector3d GetGeocentricMoonPosition(double julianDateTdb)
	{
		lock (sync)
		{
			return Evaluate(item: MoonIndex, julianDateTdb: julianDateTdb) / kilometersPerAu;
		}
	}

	/// <summary>Releases the file handle.</summary>
	public void Dispose() => stream.Dispose();

	/// <summary>Reads an optional pointer triple and returns the end index of its coefficients.</summary>
	/// <param name="h">The header bytes.</param>
	/// <param name="offset">The byte offset of the triple.</param>
	/// <param name="components">The number of components.</param>
	/// <returns>The end index, or 0 if not available.</returns>
	private static int ReadPointerEnd(ReadOnlySpan<byte> h, int offset, int components)
	{
		if (offset + 12 > h.Length)
		{
			return 0;
		}
		int start = BinaryPrimitives.ReadInt32LittleEndian(source: h[offset..]);
		int count = BinaryPrimitives.ReadInt32LittleEndian(source: h[(offset + 4)..]);
		int sub = BinaryPrimitives.ReadInt32LittleEndian(source: h[(offset + 8)..]);
		return start <= 0 || count <= 0 || sub <= 0 || start > 100000 || count > 100 || sub > 100 ? 0 : start - 1 + (count * components * sub);
	}

	/// <summary>Reads a data record into the cache.</summary>
	/// <param name="recordIndex">The zero-based index of the data record.</param>
	private void ReadRecord(long recordIndex)
	{
		if (recordIndex == cachedRecordIndex)
		{
			return;
		}
		int recordBytes = coefficientsPerRecord * 8;
		byte[] buffer = new byte[recordBytes];
		stream.Position = (recordIndex + 2) * recordBytes;
		stream.ReadExactly(buffer: buffer);
		for (int i = 0; i < coefficientsPerRecord; i++)
		{
			record[i] = BinaryPrimitives.ReadDoubleLittleEndian(source: buffer.AsSpan(start: i * 8));
		}
		cachedRecordIndex = recordIndex;
	}

	/// <summary>Evaluates the Chebyshev series of an item.</summary>
	/// <param name="item">The item index (0 = Mercury … 10 = Sun).</param>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The position [km].</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the date is outside the ephemeris coverage.</exception>
	private Vector3d Evaluate(int item, double julianDateTdb)
	{
		if (julianDateTdb < StartJulianDate || julianDateTdb > EndJulianDate)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(julianDateTdb), actualValue: julianDateTdb, message: $"The date is outside the coverage of {Name}.");
		}
		long lastRecord = (long)Math.Floor(d: (EndJulianDate - StartJulianDate) / blockLengthDays) - 1;
		long recordIndex = Math.Min(val1: (long)Math.Floor(d: (julianDateTdb - StartJulianDate) / blockLengthDays), val2: Math.Max(val1: 0, val2: lastRecord));
		ReadRecord(recordIndex: recordIndex);
		int offset = pointers[item, 0] - 1;
		int count = pointers[item, 1];
		int subIntervals = pointers[item, 2];
		double subLength = blockLengthDays / subIntervals;
		int sub = Math.Clamp(value: (int)Math.Floor(d: (julianDateTdb - record[0]) / subLength), min: 0, max: subIntervals - 1);
		double tc = (2.0 * (julianDateTdb - (record[0] + (sub * subLength))) / subLength) - 1.0;
		int baseIndex = offset + (sub * count * 3);
		return new Vector3d(
			X: Chebyshev(coefficients: record.AsSpan(start: baseIndex, length: count), x: tc),
			Y: Chebyshev(coefficients: record.AsSpan(start: baseIndex + count, length: count), x: tc),
			Z: Chebyshev(coefficients: record.AsSpan(start: baseIndex + (2 * count), length: count), x: tc));
	}

	/// <summary>Evaluates a Chebyshev series with the Clenshaw recurrence.</summary>
	/// <param name="coefficients">The coefficients.</param>
	/// <param name="x">The normalized argument in [−1, 1].</param>
	/// <returns>The value of the series.</returns>
	internal static double Chebyshev(ReadOnlySpan<double> coefficients, double x)
	{
		double b1 = 0.0, b2 = 0.0;
		for (int k = coefficients.Length - 1; k >= 1; k--)
		{
			double b0 = (2.0 * x * b1) - b2 + coefficients[k];
			b2 = b1;
			b1 = b0;
		}
		return (x * b1) - b2 + coefficients[0];
	}

	/// <summary>Returns the name of the ephemeris.</summary>
	/// <returns>The name.</returns>
	public override string ToString() => Name;
}
