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
internal static class TestData
{
	/// <summary>Builds an MPCORB line from fields placed at their fixed column offsets.</summary>
	/// <param name="fields">The pairs of zero-based column offset and right-aligned field text with its width.</param>
	/// <returns>The MPCORB line.</returns>
	private static string BuildLine(params (int Start, int Width, string Text)[] fields)
	{
		char[] line = new string(c: ' ', count: 202).ToCharArray();
		foreach ((int start, int width, string text) in fields)
		{
			string padded = text.Length > width ? text : (start == 166 ? text.PadRight(totalWidth: width) : text.PadLeft(totalWidth: width));
			padded.CopyTo(sourceIndex: 0, destination: line, destinationIndex: start, count: padded.Length);
		}
		return new string(value: line);
	}

	/// <summary>Gets an MPCORB record of (1) Ceres (epoch 2025-05-05.0 TT).</summary>
	public static string CeresRecord { get; } = BuildLine(
		(0, 7, "00001"), (8, 5, "3.34"), (14, 5, "0.15"), (20, 5, "K2555"),
		(26, 9, "188.70269"), (37, 9, "73.27343"), (48, 9, "80.25221"), (59, 9, "10.58780"),
		(70, 9, "0.0794013"), (80, 11, "0.21424651"), (92, 11, "2.7660512"),
		(107, 9, "E2024-V47"), (117, 5, "7330"), (123, 3, "125"), (127, 9, "1801-2024"), (137, 4, "0.80"),
		(150, 10, "MPCLINUX"), (161, 4, "0000"), (166, 28, "(1) Ceres"), (194, 8, "20241101"));

	/// <summary>Gets the observer at the Royal Observatory Greenwich.</summary>
	public static ObserverLocation Greenwich { get; } = new(LatitudeDegrees: 51.4772, LongitudeDegrees: 0.0, ElevationMeters: 46.0);

	/// <summary>Gets an observer in the southern hemisphere (Siding Spring).</summary>
	public static ObserverLocation SidingSpring { get; } = new(LatitudeDegrees: -31.2733, LongitudeDegrees: 149.0644, ElevationMeters: 1165.0);

	/// <summary>Parses the Ceres record.</summary>
	/// <returns>The orbital elements of Ceres.</returns>
	public static MinorPlanetOrbitalElements Ceres()
	{
		Assert.True(condition: MpcorbElementsParser.TryParse(rawLine: CeresRecord, elements: out MinorPlanetOrbitalElements? elements, error: out string? error), userMessage: error);
		return elements;
	}
}
