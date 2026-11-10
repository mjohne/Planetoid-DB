/*
 * File:        TestData.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Provides shared test data for the ephemeris tests.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB.Tests;

/// <summary>Provides shared test data.</summary>
/// <remarks>This class is internal to the test assembly and is not intended for public use.</remarks>
internal static class TestData
{
	/// <summary>Builds an MPCORB line from fields placed at their fixed column offsets.</summary>
	/// <param name="fields">The pairs of zero-based column offset and right-aligned field text with its width.</param>
	/// <returns>The MPCORB line.</returns>
	/// <remarks>This method is internal to the test assembly and is not intended for public use.</remarks>
	private static string BuildLine(params (int Start, int Width, string Text)[] fields)
	{
		ArgumentNullException.ThrowIfNull(argument: fields);
		// The MPCORB line is 202 characters long, with the last 8 characters reserved for the last-observation date.
		char[] line = new string(c: ' ', count: 202).ToCharArray();
		// The last field (name) is left-aligned, while all other fields are right-aligned.
		foreach ((int start, int width, string text) in fields)
		{
			// Pad the text to the specified width, right-aligned for all fields except the name field (start index 166).
			string padded = text.Length > width ? text : (start == 166 ? text.PadRight(totalWidth: width) : text.PadLeft(totalWidth: width));
			// Copy the padded text into the line at the specified start index.
			padded.CopyTo(sourceIndex: 0, destination: line, destinationIndex: start, count: padded.Length);
		}
		return new string(value: line);
	}

	/// <summary>Gets an MPCORB record of (1) Ceres (epoch 2025-05-05.0 TT).</summary>
	/// <remarks>This property is internal to the test assembly and is not intended for public use.</remarks>
	public static string CeresRecord { get; } = BuildLine((Start: 0, Width: 7, Text: "00001"), (Start: 8, Width: 5, Text: "3.34"), (Start: 14, Width: 5, Text: "0.15"), (Start: 20, Width: 5, Text: "K2555"), (Start: 26, Width: 9, Text: "188.70269"), (Start: 37, Width: 9, Text: "73.27343"), (Start: 48, Width: 9, Text: "80.25221"), (Start: 59, Width: 9, Text: "10.58780"), (Start: 70, Width: 9, Text: "0.0794013"), (Start: 80, Width: 11, Text: "0.21424651"), (Start: 92, Width: 11, Text: "2.7660512"), (Start: 107, Width: 9, Text: "E2024-V47"), (Start: 117, Width: 5, Text: "7330"), (Start: 123, Width: 3, Text: "125"), (Start: 127, Width: 9, Text: "1801-2024"), (Start: 137, Width: 4, Text: "0.80"), (Start: 150, Width: 10, Text: "MPCLINUX"), (Start: 161, Width: 4, Text: "0000"), (Start: 166, Width: 28, Text: "(1) Ceres"), (Start: 194, Width: 8, Text: "20241101"));

	/// <summary>Gets the observer at the Royal Observatory Greenwich.</summary>
	/// <remarks>This property is internal to the test assembly and is not intended for public use.</remarks>
	public static ObserverLocation Greenwich { get; } = new(LatitudeDegrees: 51.4772, LongitudeDegrees: 0.0, ElevationMeters: 46.0);

	/// <summary>Gets an observer in the southern hemisphere (Siding Spring).</summary>
	/// <remarks>This property is internal to the test assembly and is not intended for public use.</remarks>
	public static ObserverLocation SidingSpring { get; } = new(LatitudeDegrees: -31.2733, LongitudeDegrees: 149.0644, ElevationMeters: 1165.0);

	/// <summary>Parses the Ceres record.</summary>
	/// <returns>The orbital elements of Ceres.</returns>
	/// <remarks>This method is internal to the test assembly and is not intended for public use.</remarks>
	public static MinorPlanetOrbitalElements Ceres()
	{
		// Parse the MPCORB record and assert that parsing was successful.
		Assert.True(condition: MpcorbElementsParser.TryParse(rawLine: CeresRecord, elements: out MinorPlanetOrbitalElements? elements, error: out string? error), userMessage: error);
		// A successful parse guarantees that elements is non-null.
		return elements;
	}
}
