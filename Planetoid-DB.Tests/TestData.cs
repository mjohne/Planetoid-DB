/*
 * File:        TestData.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Provides shared test data such as MPCORB records.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;

using Planetoid_DB.Services;

namespace Planetoid_DB.Tests;

/// <summary>Provides shared test data such as MPCORB records.</summary>
internal static class TestData
{
	/// <summary>The Greenwich observatory.</summary>
	public static readonly ObserverLocation Greenwich = new(LatitudeDegrees: 51.4769, LongitudeDegrees: 0.0, HeightMeters: 46.0);

	/// <summary>Builds a fixed-width MPCORB.DAT record.</summary>
	/// <param name="designation">The packed designation (columns 1–7).</param>
	/// <param name="h">The absolute magnitude field.</param>
	/// <param name="g">The slope parameter field.</param>
	/// <param name="epoch">The packed epoch.</param>
	/// <param name="meanAnomaly">The mean anomaly field.</param>
	/// <param name="argumentOfPerihelion">The argument of perihelion field.</param>
	/// <param name="node">The longitude of the ascending node field.</param>
	/// <param name="inclination">The inclination field.</param>
	/// <param name="eccentricity">The eccentricity field.</param>
	/// <param name="semiMajorAxis">The semi-major axis field.</param>
	/// <param name="readableName">The readable designation (columns 167–194).</param>
	/// <returns>The record.</returns>
	public static string BuildRecord(
		string designation = "00001",
		string h = "3.33",
		string g = "0.15",
		string epoch = "K2555",
		string meanAnomaly = "188.70269",
		string argumentOfPerihelion = "73.27343",
		string node = "80.25221",
		string inclination = "10.58780",
		string eccentricity = "0.0795434",
		string semiMajorAxis = "2.7660512",
		string readableName = "(1) Ceres")
	{
		char[] line = new string(c: ' ', count: 202).ToCharArray();
		void Put(int start, int length, string value, bool left = false)
		{
			string field = left ? value.PadRight(totalWidth: length) : value.PadLeft(totalWidth: length);
			field.CopyTo(sourceIndex: 0, destination: line, destinationIndex: start, count: length);
		}
		Put(start: 0, length: 7, value: designation, left: true);
		Put(start: 8, length: 5, value: h);
		Put(start: 14, length: 5, value: g);
		Put(start: 20, length: 5, value: epoch);
		Put(start: 26, length: 9, value: meanAnomaly);
		Put(start: 37, length: 9, value: argumentOfPerihelion);
		Put(start: 48, length: 9, value: node);
		Put(start: 59, length: 9, value: inclination);
		Put(start: 70, length: 9, value: eccentricity);
		Put(start: 80, length: 11, value: "0.21424651");
		Put(start: 92, length: 11, value: semiMajorAxis);
		Put(start: 166, length: 28, value: readableName, left: true);
		return new string(value: line);
	}

	/// <summary>Gets the parsed elements of Ceres.</summary>
	public static MinorPlanetElements Ceres => MinorPlanetElements.Parse(record: BuildRecord());

	/// <summary>Creates a UTC instant.</summary>
	/// <param name="text">The ISO 8601 text.</param>
	/// <returns>The instant.</returns>
	public static DateTimeOffset Utc(string text) => DateTimeOffset.Parse(input: text, formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
}
