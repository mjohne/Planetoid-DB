/*
 * File:        JplSpkEphemeris.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Services
 * Description: Reads JPL planetary ephemerides (DE440, DE441, ...) from binary SPK files.
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

namespace Planetoid_DB.Services;

/// <summary>Reads JPL planetary ephemerides (DE440, DE441, ...) from binary SPK files.</summary>
/// <remarks>
/// Supports little-endian DAF/SPK files with type 2 (Chebyshev position) segments, which is the format of the JPL DE440 and DE441 kernels
/// (e.g. <c>de440.bsp</c>, <c>de440s.bsp</c>, <c>de441_part-1.bsp</c>, <c>de441_part-2.bsp</c>). Several files may be combined, e.g. both DE441 parts.
/// Positions are returned in AU in the ICRF/J2000.0 frame.
/// </remarks>
internal sealed class JplSpkEphemeris : IPlanetaryEphemeris, IDisposable
{
	/// <summary>Size of a DAF record in bytes.</summary>
	private const int RecordLength = 1024;

	/// <summary>The loaded segments.</summary>
	private readonly List<Segment> segments = [];

	/// <summary>The opened file streams.</summary>
	private readonly List<FileStream> streams = [];

	/// <summary>Synchronization object for stream access and record caching.</summary>
	private readonly Lock syncRoot = new();

	/// <summary>Initializes a new instance of the <see cref="JplSpkEphemeris"/> class.</summary>
	/// <param name="filePaths">The paths of one or more SPK files.</param>
	/// <exception cref="ArgumentException">Thrown when no file is given.</exception>
	/// <exception cref="InvalidDataException">Thrown when a file is not a supported SPK file.</exception>
	public JplSpkEphemeris(params string[] filePaths)
	{
		if (filePaths is not { Length: > 0 })
		{
			throw new ArgumentException(message: "At least one SPK file is required.", paramName: nameof(filePaths));
		}
		try
		{
			foreach (string path in filePaths)
			{
				FileStream stream = new(path: path, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 4096, options: FileOptions.RandomAccess);
				streams.Add(item: stream);
				ReadSegments(stream: stream);
			}
		}
		catch
		{
			Dispose();
			throw;
		}
		if (segments.Count == 0)
		{
			Dispose();
			throw new InvalidDataException(message: "The SPK file(s) do not contain any supported (type 2) segments.");
		}
		Name = "JPL " + string.Join(separator: ", ", values: filePaths.Select(selector: Path.GetFileName));
	}

	/// <inheritdoc/>
	public string Name { get; }

	/// <summary>Gets the time range (TDB Julian dates) covered by the Earth position chain.</summary>
	/// <returns>The first and last Julian date (TDB).</returns>
	public (double FirstJulianDateTdb, double LastJulianDateTdb) GetCoverage()
	{
		Segment[] earth = [.. segments.Where(predicate: s => s.Target == (int)SolarSystemBody.EarthMoonBarycenter)];
		return earth.Length > 0
			? (EtToJulianDate(et: earth.Min(selector: s => s.StartEt)), EtToJulianDate(et: earth.Max(selector: s => s.EndEt)))
			: (double.NaN, double.NaN);
	}

	/// <inheritdoc/>
	public Vector3D GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		double et = (julianDateTdb - AstronomicalConstants.JulianDateJ2000) * AstronomicalConstants.SecondsPerDay;
		lock (syncRoot)
		{
			Vector3D bodyKm = GetBarycentricPositionKm(target: (int)body, et: et);
			Vector3D sunKm = GetBarycentricPositionKm(target: (int)SolarSystemBody.Sun, et: et);
			return (bodyKm - sunKm) / AstronomicalConstants.AstronomicalUnitKm;
		}
	}

	/// <summary>Releases the file handles.</summary>
	public void Dispose()
	{
		foreach (FileStream stream in streams)
		{
			stream.Dispose();
		}
		streams.Clear();
	}

	/// <summary>Converts seconds past J2000 (TDB) into a Julian date.</summary>
	/// <param name="et">The ephemeris time in seconds past J2000.</param>
	/// <returns>The Julian date (TDB).</returns>
	private static double EtToJulianDate(double et) => AstronomicalConstants.JulianDateJ2000 + (et / AstronomicalConstants.SecondsPerDay);

	/// <summary>Recursively resolves the position of a target relative to the solar system barycenter.</summary>
	/// <param name="target">The NAIF ID of the target.</param>
	/// <param name="et">The ephemeris time in seconds past J2000 (TDB).</param>
	/// <returns>The barycentric position in km.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the target or time is not covered.</exception>
	private Vector3D GetBarycentricPositionKm(int target, double et)
	{
		Vector3D result = Vector3D.Zero;
		int current = target;
		while (current != 0)
		{
			Segment segment = FindSegment(target: current, et: et);
			result += segment.Evaluate(et: et);
			current = segment.Center;
		}
		return result;
	}

	/// <summary>Finds the segment containing a target at a given time.</summary>
	/// <param name="target">The NAIF ID of the target.</param>
	/// <param name="et">The ephemeris time in seconds past J2000 (TDB).</param>
	/// <returns>The segment.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when no segment covers the target at that time.</exception>
	private Segment FindSegment(int target, double et)
	{
		// Later segments take precedence, as defined by the SPK specification
		for (int i = segments.Count - 1; i >= 0; i--)
		{
			Segment s = segments[i];
			if (s.Target == target && et >= s.StartEt && et <= s.EndEt)
			{
				return s;
			}
		}
		throw new ArgumentOutOfRangeException(paramName: nameof(et), actualValue: EtToJulianDate(et: et), message: $"The SPK file(s) do not cover NAIF body {target} at this time (JD TDB).");
	}

	/// <summary>Reads the segment summaries of a DAF/SPK file.</summary>
	/// <param name="stream">The file stream.</param>
	/// <exception cref="InvalidDataException">Thrown when the file format is not supported.</exception>
	private void ReadSegments(FileStream stream)
	{
		byte[] fileRecord = ReadBytes(stream: stream, offset: 0, count: RecordLength);
		string idWord = Encoding.ASCII.GetString(bytes: fileRecord, index: 0, count: 8);
		if (!idWord.StartsWith(value: "DAF/SPK", comparisonType: StringComparison.Ordinal))
		{
			throw new InvalidDataException(message: "The file is not a DAF/SPK file.");
		}
		string format = Encoding.ASCII.GetString(bytes: fileRecord, index: 88, count: 8);
		if (format != "LTL-IEEE")
		{
			throw new InvalidDataException(message: $"Unsupported binary format '{format.Trim()}'; only little-endian (LTL-IEEE) SPK files are supported.");
		}
		int nd = BinaryPrimitives.ReadInt32LittleEndian(source: fileRecord.AsSpan(start: 8));
		int ni = BinaryPrimitives.ReadInt32LittleEndian(source: fileRecord.AsSpan(start: 12));
		int forward = BinaryPrimitives.ReadInt32LittleEndian(source: fileRecord.AsSpan(start: 76));
		if (nd != 2 || ni != 6)
		{
			throw new InvalidDataException(message: "The DAF file does not have the SPK summary layout (ND = 2, NI = 6).");
		}
		int summaryDoubles = nd + ((ni + 1) / 2);
		int record = forward;
		HashSet<int> visited = [];
		while (record > 0 && visited.Add(item: record))
		{
			byte[] summaryRecord = ReadBytes(stream: stream, offset: (long)(record - 1) * RecordLength, count: RecordLength);
			int next = (int)BitConverter.ToDouble(value: summaryRecord, startIndex: 0);
			double summaryCount = BitConverter.ToDouble(value: summaryRecord, startIndex: 16);
			int maximumSummaryCount = (RecordLength - 24) / (summaryDoubles * sizeof(double));
			if (!double.IsFinite(d: summaryCount) || summaryCount < 0 || summaryCount > maximumSummaryCount || summaryCount != Math.Truncate(d: summaryCount))
			{
				throw new InvalidDataException(message: $"The SPK summary record has an invalid summary count ({summaryCount}).");
			}
			int count = (int)summaryCount;
			for (int i = 0; i < count; i++)
			{
				int offset = 24 + (i * summaryDoubles * 8);
				double startEt = BitConverter.ToDouble(value: summaryRecord, startIndex: offset);
				double endEt = BitConverter.ToDouble(value: summaryRecord, startIndex: offset + 8);
				int target = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 16);
				int center = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 20);
				int frame = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 24);
				int type = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 28);
				int beginAddress = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 32);
				int endAddress = BitConverter.ToInt32(value: summaryRecord, startIndex: offset + 36);
				// Only type 2 segments in the J2000/ICRF frame (frame ID 1) are used
				if (type == 2 && frame == 1)
				{
					segments.Add(item: new Segment(stream: stream, syncRoot: syncRoot, target: target, center: center, startEt: startEt, endEt: endEt, beginAddress: beginAddress, endAddress: endAddress));
				}
			}
			record = next;
		}
	}

	/// <summary>Reads a block of bytes from a stream.</summary>
	/// <param name="stream">The stream.</param>
	/// <param name="offset">The byte offset.</param>
	/// <param name="count">The number of bytes.</param>
	/// <returns>The bytes read.</returns>
	/// <exception cref="InvalidDataException">Thrown when the file is truncated.</exception>
	private static byte[] ReadBytes(FileStream stream, long offset, int count)
	{
		byte[] buffer = new byte[count];
		_ = stream.Seek(offset: offset, origin: SeekOrigin.Begin);
		stream.ReadExactly(buffer: buffer, offset: 0, count: count);
		return buffer;
	}

	/// <summary>Represents a type 2 (Chebyshev position) SPK segment.</summary>
	private sealed class Segment
	{
		/// <summary>The file stream containing the segment.</summary>
		private readonly FileStream stream;

		/// <summary>Synchronization object shared with the owning ephemeris.</summary>
		private readonly Lock syncRoot;

		/// <summary>The 1-based double-word address of the first segment element.</summary>
		private readonly int beginAddress;

		/// <summary>The initial epoch of the first record (seconds past J2000).</summary>
		private readonly double initialEt;

		/// <summary>The length of each record interval in seconds.</summary>
		private readonly double intervalLength;

		/// <summary>The number of doubles per record.</summary>
		private readonly int recordSize;

		/// <summary>The number of records.</summary>
		private readonly int recordCount;

		/// <summary>The index of the cached record, or -1.</summary>
		private int cachedIndex = -1;

		/// <summary>The cached record coefficients.</summary>
		private double[] cachedRecord = [];

		/// <summary>Initializes a new instance of the <see cref="Segment"/> class.</summary>
		/// <param name="stream">The file stream.</param>
		/// <param name="syncRoot">The synchronization object.</param>
		/// <param name="target">The NAIF ID of the target.</param>
		/// <param name="center">The NAIF ID of the center.</param>
		/// <param name="startEt">The segment start time (seconds past J2000).</param>
		/// <param name="endEt">The segment end time (seconds past J2000).</param>
		/// <param name="beginAddress">The 1-based start address.</param>
		/// <param name="endAddress">The 1-based end address.</param>
		public Segment(FileStream stream, Lock syncRoot, int target, int center, double startEt, double endEt, int beginAddress, int endAddress)
		{
			this.stream = stream;
			this.syncRoot = syncRoot;
			Target = target;
			Center = center;
			StartEt = startEt;
			EndEt = endEt;
			this.beginAddress = beginAddress;
			double[] directory = ReadDoubles(address: endAddress - 3, count: 4);
			initialEt = directory[0];
			intervalLength = directory[1];
			recordSize = (int)directory[2];
			recordCount = (int)directory[3];
			if (intervalLength <= 0 || recordSize < 5 || (recordSize - 2) % 3 != 0 || recordCount <= 0)
			{
				throw new InvalidDataException(message: $"Invalid type 2 segment directory for NAIF body {target}.");
			}
		}

		/// <summary>Gets the NAIF ID of the target.</summary>
		public int Target { get; }

		/// <summary>Gets the NAIF ID of the center.</summary>
		public int Center { get; }

		/// <summary>Gets the segment start time (seconds past J2000, TDB).</summary>
		public double StartEt { get; }

		/// <summary>Gets the segment end time (seconds past J2000, TDB).</summary>
		public double EndEt { get; }

		/// <summary>Evaluates the Chebyshev polynomials of the segment.</summary>
		/// <param name="et">The ephemeris time in seconds past J2000 (TDB).</param>
		/// <returns>The position of the target relative to its center in km.</returns>
		public Vector3D Evaluate(double et)
		{
			int index = Math.Clamp(value: (int)Math.Floor(d: (et - initialEt) / intervalLength), min: 0, max: recordCount - 1);
			lock (syncRoot)
			{
				if (index != cachedIndex)
				{
					cachedRecord = ReadDoubles(address: beginAddress + (index * recordSize), count: recordSize);
					cachedIndex = index;
				}
			}
			double[] rec = cachedRecord;
			double s = (et - rec[0]) / rec[1];
			int n = (recordSize - 2) / 3;
			return new Vector3D(X: Chebyshev(coefficients: rec, offset: 2, count: n, s: s), Y: Chebyshev(coefficients: rec, offset: 2 + n, count: n, s: s), Z: Chebyshev(coefficients: rec, offset: 2 + (2 * n), count: n, s: s));
		}

		/// <summary>Evaluates a Chebyshev series.</summary>
		/// <param name="coefficients">The coefficient array.</param>
		/// <param name="offset">The index of the first coefficient.</param>
		/// <param name="count">The number of coefficients.</param>
		/// <param name="s">The normalized time in [−1, 1].</param>
		/// <returns>The value of the series.</returns>
		private static double Chebyshev(double[] coefficients, int offset, int count, double s)
		{
			double t0 = 1.0, t1 = s, sum = coefficients[offset] + (count > 1 ? coefficients[offset + 1] * s : 0.0);
			for (int k = 2; k < count; k++)
			{
				double t2 = (2.0 * s * t1) - t0;
				sum += coefficients[offset + k] * t2;
				t0 = t1;
				t1 = t2;
			}
			return sum;
		}

		/// <summary>Reads consecutive little-endian doubles from the file.</summary>
		/// <param name="address">The 1-based double-word address.</param>
		/// <param name="count">The number of doubles.</param>
		/// <returns>The values read.</returns>
		private double[] ReadDoubles(int address, int count)
		{
			byte[] bytes = ReadBytes(stream: stream, offset: (long)(address - 1) * 8, count: count * 8);
			double[] values = new double[count];
			for (int i = 0; i < count; i++)
			{
				values[i] = BinaryPrimitives.ReadDoubleLittleEndian(source: bytes.AsSpan(start: i * 8));
			}
			return values;
		}
	}
}
