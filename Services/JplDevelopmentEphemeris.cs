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
using System.Diagnostics;

namespace Planetoid_DB.Services;

/// <summary>Reads JPL Development Ephemeris files in the classic little-endian binary format (e.g. <c>linux_p1550p2650.440</c>, <c>linux_m13000p17000.441</c>).</summary>
/// <remarks>The files can be downloaded from https://ssd.jpl.nasa.gov/ftp/eph/planets/Linux/. Positions are evaluated from the Chebyshev
/// coefficients and returned in the ICRF in astronomical units. DE440 covers 1550–2650, DE441 covers −13200 to +17191.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class JplDevelopmentEphemeris : IPlanetaryEphemerisProvider, IDisposable
{
	/// <summary>Size of the fixed part of the header [bytes].</summary>
	/// <remarks>The fixed header contains metadata about the ephemeris file, including the number of coefficients, the number of sub-intervals, and other information required to interpret the data records.</remarks>
	private const int FixedHeaderSize = 2856;

	/// <summary>Index of the Earth–Moon barycenter in the coefficient pointer table.</summary>
	/// <remarks>The Earth–Moon barycenter is used to compute the heliocentric position of the Earth and Moon separately, based on the Earth/Moon mass ratio.</remarks>
	private const int EmbIndex = 2;

	/// <summary>Index of the Moon (geocentric) in the coefficient pointer table.</summary>
	/// <remarks>The Moon's position is given relative to the Earth (geocentric) and is used to compute the Moon's heliocentric position based on the Earth/Moon mass ratio.</remarks>
	private const int MoonIndex = 9;

	/// <summary>Index of the Sun (barycentric) in the coefficient pointer table.</summary>
	/// <remarks>The Sun's position is given relative to the solar system barycenter and is used to compute the heliocentric positions of other bodies.</remarks>
	private const int SunIndex = 10;

	/// <summary>The underlying file stream.</summary>
	/// <remarks>This stream is used to read the binary ephemeris data from the file.</remarks>
	private readonly FileStream stream;

	/// <summary>Coefficient pointer table: offset (1-based), number of coefficients, number of sub-intervals for each item.</summary>
	/// <remarks>The pointer table is a 12×3 array where each row corresponds to a solar system body or other item, and the columns represent the offset of the coefficients in the data record, the number of coefficients for that item, and the number of sub-intervals within each data block.</remarks>
	private readonly int[][] pointers = new int[12][];

	/// <summary>Number of doubles per data record.</summary>
	/// <remarks>This value represents the total number of double-precision floating-point numbers in each data record of the ephemeris file.</remarks>
	private readonly int coefficientsPerRecord;

	/// <summary>Length of a data block [d].</summary>
	/// <remarks>This value represents the duration of each data block in days.</remarks>
	private readonly double blockLengthDays;

	/// <summary>Kilometers per astronomical unit as stored in the file.</summary>
	/// <remarks>This value represents the number of kilometers in one astronomical unit as stored in the ephemeris file.</remarks>
	private readonly double kilometersPerAu;

	/// <summary>Earth/Moon mass ratio as stored in the file.</summary>
	/// <remarks>This value represents the ratio of the Earth's mass to the Moon's mass as stored in the ephemeris file.</remarks>
	private readonly double earthMoonRatio;

	/// <summary>Synchronization object for the record cache.</summary>
	/// <remarks>This object is used to synchronize access to the cached data record.</remarks>
	private readonly Lock sync = new();

	/// <summary>The cached data record.</summary>
	/// <remarks>This array holds the cached data record for quick access.</remarks>
	private readonly double[] record;

	/// <summary>The index of the cached data record, or −1.</summary>
	/// <remarks>This value indicates the index of the currently cached data record, or −1 if no record is cached.</remarks>
	private long cachedRecordIndex = -1;

	/// <summary>Initializes a new instance of the <see cref="JplDevelopmentEphemeris"/> class.</summary>
	/// <param name="filePath">The path of the binary DE file.</param>
	/// <exception cref="InvalidDataException">Thrown when the file is not a valid little-endian JPL DE binary file.</exception>
	/// <remarks>This constructor opens the specified binary DE file, reads the header, and initializes the ephemeris provider. It validates the header and coefficient layout to ensure that the file is a valid JPL DE binary file.</remarks>
	public JplDevelopmentEphemeris(string filePath)
	{
		// Initialize the coefficient pointer table
		for (int i = 0; i < 12; i++)
		{
			pointers[i] = new int[3];
		}
		// Open the file stream with random access and a small buffer size for efficient reading of the header and data records
		stream = new FileStream(path: filePath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.Read, bufferSize: 1, options: FileOptions.RandomAccess);
		// Read the fixed header and validate the file format
		try
		{
			// Read the fixed header (2856 bytes) and the variable part (up to 1000 constants, 6 bytes each, plus 24 bytes for additional pointers)
			byte[] header = new byte[FixedHeaderSize + (6 * 1000) + 24];
			// Read at least the fixed header; the variable part may not be present in all files
			int read = stream.ReadAtLeast(buffer: header, minimumBytes: header.Length, throwOnEndOfStream: false);
			// Validate that the fixed header was read completely
			if (read < header.Length)
			{
				// If the fixed header is incomplete, throw an exception indicating that the file header is incomplete
				throw new InvalidDataException(message: "The file header is incomplete.");
			}
			// Use a ReadOnlySpan<byte> to avoid copying the header data and to allow efficient slicing
			ReadOnlySpan<byte> h = header;
			// Read the start and end Julian dates, block length, constant count, kilometers per AU, and Earth/Moon mass ratio from the header
			StartJulianDate = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2652..]);
			EndJulianDate = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2660..]);
			blockLengthDays = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2668..]);
			int constantCount = BinaryPrimitives.ReadInt32LittleEndian(source: h[2676..]);
			kilometersPerAu = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2680..]);
			earthMoonRatio = BinaryPrimitives.ReadDoubleLittleEndian(source: h[2688..]);
			// Read the coefficient pointers for the 12 items (Mercury, Venus, Earth-Moon barycenter, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto, Moon, Sun, Nutations)
			for (int i = 0; i < 12; i++)
			{
				// Each item has 3 integers: offset (1-based), number of coefficients, number of sub-intervals
				for (int j = 0; j < 3; j++)
				{
					// Read the pointer values from the header and store them in the pointers array
					pointers[i][j] = BinaryPrimitives.ReadInt32LittleEndian(source: h[(2696 + (((i * 3) + j) * 4))..]);
				}
			}
			// Read the DE number (e.g., 440 for DE440) from the header
			int deNumber = BinaryPrimitives.ReadInt32LittleEndian(source: h[2840..]);
			// Validate the DE number, constant count, and other header values to ensure they are within expected ranges
			if (deNumber is < 100 or > 1000 || constantCount is < 0 or > 1000 ||
				!double.IsFinite(d: StartJulianDate) || !double.IsFinite(d: EndJulianDate) || !double.IsFinite(d: blockLengthDays) ||
				!double.IsFinite(d: kilometersPerAu) || !double.IsFinite(d: earthMoonRatio) ||
				blockLengthDays <= 0.0 || EndJulianDate <= StartJulianDate ||
				kilometersPerAu is < 1.4e8 or > 1.6e8 || earthMoonRatio is < 80.0 or > 82.0)
			{
				// If any of the header values are out of the expected ranges, throw an exception indicating that the file is not a valid little-endian JPL DE binary file
				throw new InvalidDataException(message: "The file is not a valid little-endian JPL DE binary file.");
			}
			// Set the human-readable name of the ephemeris based on the DE number
			Name = $"JPL DE{deNumber.ToString(provider: System.Globalization.CultureInfo.InvariantCulture)}";
			// Determine the record length from the largest coefficient pointer (bodies: 3 components, nutations: 2 components)
			int maxEnd = 0;
			// Loop through the 12 items to find the maximum end index of the coefficients
			for (int i = 0; i < 12; i++)
			{
				// For the Sun (index 11), there are only 2 components (X, Y), while for other bodies there are 3 components (X, Y, Z)
				int components = i == 11 ? 2 : 3;
				// Read the end index of the coefficients for the current item from the header
				int pointerEnd = ReadPointerEnd(h: h, offset: 2696 + (i * 12), components: components);
				// If the Sun's pointer is invalid (0), throw an exception indicating that the coefficient layout is invalid
				if (i <= SunIndex && pointerEnd == 0)
				{
					// If the Sun's pointer is invalid, throw an exception indicating that the coefficient layout is invalid
					throw new InvalidDataException(message: "The coefficient layout of the JPL DE file is invalid.");
				}
				maxEnd = Math.Max(val1: maxEnd, val2: pointerEnd);
			}
			// Lunar librations (third pointer triple stored after the DE number)
			// Read the end index of the nutation coefficients (3 components) from the header and update maxEnd
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: 2844, components: 3));
			// Lunar librations (fourth pointer triple stored after the DE number)
			int extraOffset = FixedHeaderSize + (Math.Max(val1: 0, val2: constantCount - 400) * 6);
			// Read the end index of the fourth pointer triple (3 components) from the header and update maxEnd
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: extraOffset, components: 3));
			// Read the end index of the fourth pointer triple (1 component) from the header and update maxEnd
			maxEnd = Math.Max(val1: maxEnd, val2: ReadPointerEnd(h: h, offset: extraOffset + 12, components: 1));
			// Validate that the maximum end index of the coefficients is within a reasonable range (3 to 100000)
			if (maxEnd is < 3 or > 100000)
			{
				// If the maximum end index is out of range, throw an exception indicating that the coefficient layout of the JPL DE file is invalid
				throw new InvalidDataException(message: "The coefficient layout of the JPL DE file is invalid.");
			}
			// Allocate the record array to hold the coefficients for a single data record
			coefficientsPerRecord = maxEnd;
			record = new double[coefficientsPerRecord];
			// Verify the layout: the first data record must start at the start date of the ephemeris
			ReadRecord(recordIndex: 0);
			// Check that the first record's start date matches the expected StartJulianDate and that the block length is correct
			if (Math.Abs(value: record[0] - StartJulianDate) > 1e-6 || Math.Abs(value: record[1] - record[0] - blockLengthDays) > 1e-6)
			{
				// If the first record's start date or block length is incorrect,
				throw new InvalidDataException(message: "The data records of the JPL DE file could not be located.");
			}
		}
		// If any exception occurs during the initialization, dispose of the stream and rethrow the exception
		catch
		{
			stream.Dispose();
			throw;
		}
	}

	/// <summary>Gets a human-readable name of the ephemeris (e.g. "JPL DE440").</summary>
	/// <remarks>This property returns the name of the ephemeris based on the DE number read from the file header.</remarks>
	public string Name { get; }

	/// <summary>Gets the first Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>This property returns the start Julian date (TDB) read from the file header, indicating the beginning of the ephemeris coverage.</remarks>
	public double StartJulianDate { get; }

	/// <summary>Gets the last Julian date (TDB) covered by the ephemeris.</summary>
	/// <remarks>This property returns the end Julian date (TDB) read from the file header, indicating the end of the ephemeris coverage.</remarks>
	public double EndJulianDate { get; }

	/// <summary>Gets the heliocentric position of the specified body at the given Julian date (TDB).</summary>
	/// <param name="body">The solar system body.</param>
	/// <param name="julianDateTdb">The Julian date (TDB).</param>
	/// <returns>The heliocentric position of the body in astronomical units (AU).</returns>
	/// <remarks>This method computes the heliocentric position of the specified solar system body by evaluating the Chebyshev coefficients for that body and subtracting the Sun's position. The result is returned in astronomical units (AU).</remarks>
	public Vector3d GetHeliocentricPosition(SolarSystemBody body, double julianDateTdb)
	{
		// Lock the sync object to ensure thread-safe access to the cached record and evaluation
		lock (sync)
		{
			// Evaluate the Sun's position at the given Julian date (TDB)
			Vector3d sun = Evaluate(item: SunIndex, julianDateTdb: julianDateTdb);
			// Evaluate the barycentric position of the specified body based on its index in the coefficient pointer table
			Vector3d barycentric = body switch
			{
				// Evaluate the position of the specified solar system body based on its index in the coefficient pointer table
				SolarSystemBody.Mercury => Evaluate(item: 0, julianDateTdb: julianDateTdb),
				SolarSystemBody.Venus => Evaluate(item: 1, julianDateTdb: julianDateTdb),
				SolarSystemBody.EarthMoonBarycenter => Evaluate(item: EmbIndex, julianDateTdb: julianDateTdb),
				SolarSystemBody.Earth => Evaluate(item: EmbIndex, julianDateTdb: julianDateTdb) - (Evaluate(item: MoonIndex, julianDateTdb: julianDateTdb) / (1.0 + earthMoonRatio)),
				SolarSystemBody.Mars => Evaluate(item: 3, julianDateTdb: julianDateTdb),
				SolarSystemBody.Jupiter => Evaluate(item: 4, julianDateTdb: julianDateTdb),
				SolarSystemBody.Saturn => Evaluate(item: 5, julianDateTdb: julianDateTdb),
				SolarSystemBody.Uranus => Evaluate(item: 6, julianDateTdb: julianDateTdb),
				SolarSystemBody.Neptune => Evaluate(item: 7, julianDateTdb: julianDateTdb),
				// If the specified body is not supported, throw an exception indicating that the body is unsupported
				_ => throw new ArgumentOutOfRangeException(paramName: nameof(body), actualValue: body, message: "Unsupported body.")
			};
			// Return the heliocentric position of the body by subtracting the Sun's position and converting from kilometers to astronomical units (AU)
			return (barycentric - sun) / kilometersPerAu;
		}
	}

	/// <summary>Gets the geocentric position of the Moon at the given Julian date (TDB).</summary>
	/// <param name="julianDateTdb">The Julian date (TDB).</param>
	/// <returns>The geocentric position of the Moon in astronomical units (AU).</returns>
	/// <remarks>This method computes the geocentric position of the Moon by evaluating the Chebyshev coefficients for the Moon and converting the result from kilometers to astronomical units (AU).</remarks>	
	public Vector3d GetGeocentricMoonPosition(double julianDateTdb)
	{
		// Lock the sync object to ensure thread-safe access to the cached record and evaluation
		lock (sync)
		{
			// Evaluate the Moon's position at the given Julian date (TDB) and convert from kilometers to astronomical units (AU)
			return Evaluate(item: MoonIndex, julianDateTdb: julianDateTdb) / kilometersPerAu;
		}
	}

	/// <summary>Releases the file handle.</summary>
	/// <remarks>This method disposes of the underlying file stream, releasing the file handle and any associated resources.</remarks>
	public void Dispose()
	{
		// Dispose of the underlying stream
		stream.Dispose();
	}

	/// <summary>Reads an optional pointer triple and returns the end index of its coefficients.</summary>
	/// <param name="h">The header bytes.</param>
	/// <param name="offset">The byte offset of the triple.</param>
	/// <param name="components">The number of components.</param>
	/// <returns>The end index, or 0 if not available.</returns>
	/// <remarks>This method reads a pointer triple (offset, count, sub-intervals) from the header and calculates the end index of the coefficients for the specified number of components. If the pointer is invalid or out of range, it returns 0.</remarks>
	private static int ReadPointerEnd(ReadOnlySpan<byte> h, int offset, int components)
	{
		// Check if the offset is within the bounds of the header span
		if (offset + 12 > h.Length)
		{
			// If the offset is out of bounds, return 0 to indicate that the pointer is not available
			return 0;
		}
		// Read the start index, count of coefficients, and number of sub-intervals from the header
		int start = BinaryPrimitives.ReadInt32LittleEndian(source: h[offset..]);
		int count = BinaryPrimitives.ReadInt32LittleEndian(source: h[(offset + 4)..]);
		int sub = BinaryPrimitives.ReadInt32LittleEndian(source: h[(offset + 8)..]);
		// Validate the pointer values and calculate the end index of the coefficients for the specified number of components
		return start <= 0 || count <= 0 || sub <= 0 || start > 100000 || count > 100 || sub > 100 ? 0 : start - 1 + (count * components * sub);
	}

	/// <summary>Reads a data record into the cache.</summary>
	/// <param name="recordIndex">The zero-based index of the data record.</param>
	/// <remarks>This method reads a data record from the ephemeris file at the specified index and caches it for subsequent evaluations. If the requested record is already cached, it does nothing.</remarks>
	private void ReadRecord(long recordIndex)
	{
		// Check if the requested record index is already cached
		if (recordIndex == cachedRecordIndex)
		{
			// If the record is already cached, return without reading from the file
			return;
		}
		// Calculate the number of bytes in a data record (each coefficient is a double, which is 8 bytes)
		int recordBytes = coefficientsPerRecord * 8;
		// Create a buffer to hold the data record bytes
		byte[] buffer = new byte[recordBytes];
		// Seek to the position of the requested data record in the file (the first two records are reserved for header information)
		stream.Position = (recordIndex + 2) * recordBytes;
		// Read the data record bytes from the file into the buffer
		stream.ReadExactly(buffer: buffer);
		// Convert the bytes in the buffer to double-precision floating-point numbers and store them in the record array
		for (int i = 0; i < coefficientsPerRecord; i++)
		{
			// Read each double value from the buffer using little-endian byte order and store it in the record array
			record[i] = BinaryPrimitives.ReadDoubleLittleEndian(source: buffer.AsSpan(start: i * 8));
		}
		// Update the cached record index to indicate that the requested record is now cached
		cachedRecordIndex = recordIndex;
	}

	/// <summary>Evaluates the Chebyshev series of an item.</summary>
	/// <param name="item">The item index (0 = Mercury … 10 = Sun).</param>
	/// <param name="julianDateTdb">The Julian date (TDB) [d].</param>
	/// <returns>The position [km].</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the date is outside the ephemeris coverage.</exception>
	/// <remarks>This method evaluates the Chebyshev series for the specified item (solar system body) at the given Julian date (TDB). It calculates the appropriate data record and sub-interval, normalizes the time, and computes the position using the Chebyshev coefficients. The result is returned in kilometers.</remarks>
	private Vector3d Evaluate(int item, double julianDateTdb)
	{
		// Check if the requested Julian date is within the coverage of the ephemeris
		if (julianDateTdb < StartJulianDate || julianDateTdb > EndJulianDate)
		{
			// If the date is outside the coverage, throw an exception indicating that the date is out of range
			throw new ArgumentOutOfRangeException(paramName: nameof(julianDateTdb), actualValue: julianDateTdb, message: $"The date is outside the coverage of {Name}.");
		}
		// Calculate the index of the last data record based on the start and end Julian dates and the block length
		long lastRecord = (long)Math.Floor(d: (EndJulianDate - StartJulianDate) / blockLengthDays) - 1;
		long recordIndex = Math.Min(val1: (long)Math.Floor(d: (julianDateTdb - StartJulianDate) / blockLengthDays), val2: Math.Max(val1: 0, val2: lastRecord));
		// Read the data record corresponding to the calculated index
		ReadRecord(recordIndex: recordIndex);
		// Get the offset, count, and number of sub-intervals for the specified item from the pointer table
		int offset = pointers[item][0] - 1;
		int count = pointers[item][1];
		int subIntervals = pointers[item][2];
		// Calculate the length of each sub-interval in days
		double subLength = blockLengthDays / subIntervals;
		// Determine the sub-interval index for the given Julian date
		int sub = Math.Clamp(value: (int)Math.Floor(d: (julianDateTdb - record[0]) / subLength), min: 0, max: subIntervals - 1);
		// Normalize the time to the range [-1, 1] for Chebyshev evaluation
		double tc = (2.0 * (julianDateTdb - (record[0] + (sub * subLength))) / subLength) - 1.0;
		// Calculate the base index of the coefficients for the specified item and sub-interval
		int baseIndex = offset + (sub * count * 3);
		// Evaluate the Chebyshev series for each component (X, Y, Z) using the coefficients and normalized time
		return new Vector3d(
			X: Chebyshev(coefficients: record.AsSpan(start: baseIndex, length: count), x: tc),
			Y: Chebyshev(coefficients: record.AsSpan(start: baseIndex + count, length: count), x: tc),
			Z: Chebyshev(coefficients: record.AsSpan(start: baseIndex + (2 * count), length: count), x: tc));
	}

	/// <summary>Evaluates a Chebyshev series with the Clenshaw recurrence.</summary>
	/// <param name="coefficients">The coefficients.</param>
	/// <param name="x">The normalized argument in [−1, 1].</param>
	/// <returns>The value of the series.</returns>
	/// <remarks>This method evaluates a Chebyshev series using the Clenshaw recurrence formula. The coefficients are provided in a read-only span, and the normalized argument x must be in the range [-1, 1]. The result is the value of the Chebyshev series at the specified x.</remarks>
	internal static double Chebyshev(ReadOnlySpan<double> coefficients, double x)
	{
		// Initialize the recurrence variables for the Clenshaw algorithm
		double b1 = 0.0, b2 = 0.0;
		// Iterate through the coefficients in reverse order to compute the Chebyshev series value
		for (int k = coefficients.Length - 1; k >= 1; k--)
		{
			// Apply the Clenshaw recurrence relation to compute the next value in the series
			double b0 = (2.0 * x * b1) - b2 + coefficients[index: k];
			// Update the recurrence variables for the next iteration
			b2 = b1;
			b1 = b0;
		}
		// Return the final value of the Chebyshev series, which includes the first coefficient and the last computed b1 and b2 values
		return (x * b1) - b2 + coefficients[index: 0];
	}

	/// <summary>Returns the name of the ephemeris.</summary>
	/// <returns>The name.</returns>
	/// <remarks>This method overrides the default ToString() implementation to return the human-readable name of the ephemeris, which is useful for logging and debugging purposes.</remarks>
	public override string ToString()
	{
		return Name;
	}

	/// <summary>Gets a string representation of the instance for debugging purposes.</summary>
	/// <returns>A string representation of the instance.</returns>
	/// <remarks>This property is used by the <see cref="DebuggerDisplayAttribute"/> to provide a concise summary of the instance in the debugger.</remarks>
	private string DebuggerDisplay => ToString();
}
