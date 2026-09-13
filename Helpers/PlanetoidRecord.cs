/*
 * File:        PlanetoidRecord.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Represents a single planetoid dataset.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics;

namespace Planetoid_DB.Helpers;

/// <summary>Represents a single planetoid dataset.</summary>
/// <param name="Index">index of the planetoid</param>
/// <param name="MagAbs">absolute magnitude of the planetoid</param>
/// <param name="SlopeParam">slope parameter of the planetoid</param>
/// <param name="Epoch">epoch of the planetoid</param>
/// <param name="MeanAnomaly">mean anomaly of the planetoid</param>
/// <param name="ArgPeri">argument of perihelion of the planetoid</param>
/// <param name="LongAscNode">longitude of the ascending node of the planetoid</param>
/// <param name="Incl">inclination of the planetoid</param>
/// <param name="OrbEcc">orbital eccentricity of the planetoid</param>
/// <param name="Motion">mean daily motion of the planetoid</param>
/// <param name="SemiMajorAxis">semi-major axis of the planetoid</param>
/// <param name="Ref">reference of the planetoid</param>
/// <param name="NumberObservation">number of observations of the planetoid</param>
/// <param name="NumberOpposition">number of oppositions of the planetoid</param>
/// <param name="ObsSpan">observation span of the planetoid</param>
/// <param name="RmsResidual">root mean square residual of the planetoid</param>
/// <param name="ComputerName">computer name of the planetoid</param>
/// <param name="Flags">flags of the planetoid</param>
/// <param name="DesignationName">designation name of the planetoid</param>
/// <param name="ObservationLastDate">observation last date of the planetoid</param>
/// <return>The planetoid record.</return>
/// <remarks>This record struct is used to represent a single planetoid dataset.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal readonly record struct PlanetoidRecord(string Index, string MagAbs, string SlopeParam, string Epoch, string MeanAnomaly, string ArgPeri, string LongAscNode, string Incl, string OrbEcc, string Motion, string SemiMajorAxis, string Ref, string NumberObservation, string NumberOpposition, string ObsSpan, string RmsResidual, string ComputerName, string Flags, string DesignationName, string ObservationLastDate)
{
	/// <summary>Parses a raw line (Fixed-Width) into a PlanetoidRecord object.</summary>
	/// <param name="rawLine">The raw line to parse.</param>
	/// <returns>A PlanetoidRecord object.</returns>
	/// <exception cref="ArgumentException">Thrown when the raw line is invalid.</exception>
	/// <remarks>This method expects the raw line to be in a fixed-width format.</remarks>
	public static PlanetoidRecord Parse(string rawLine)
	{
		// Validate input
		if (string.IsNullOrWhiteSpace(value: rawLine) || rawLine.Length < 202)
		{
			// Throw an exception if the raw line is invalid or does not meet the minimum length requirement
			throw new ArgumentException(message: "The raw line is invalid or does not meet the minimum length of 202 characters.", paramName: nameof(rawLine));
		}
		// Use ReadOnlySpan for zero-allocation slicing before calling .ToString()
		ReadOnlySpan<char> span = rawLine.AsSpan();
		// Extract fields based on fixed-width positions
		return new PlanetoidRecord(
			Index: span[..7].Trim().ToString(),
			MagAbs: span.Slice(start: 8, length: 5).Trim().ToString(),
			SlopeParam: span.Slice(start: 14, length: 5).Trim().ToString(),
			Epoch: span.Slice(start: 20, length: 5).Trim().ToString(),
			MeanAnomaly: span.Slice(start: 26, length: 9).Trim().ToString(),
			ArgPeri: span.Slice(start: 37, length: 9).Trim().ToString(),
			LongAscNode: span.Slice(start: 48, length: 9).Trim().ToString(),
			Incl: span.Slice(start: 59, length: 9).Trim().ToString(),
			OrbEcc: span.Slice(start: 70, length: 9).Trim().ToString(),
			Motion: span.Slice(start: 80, length: 11).Trim().ToString(),
			SemiMajorAxis: span.Slice(start: 92, length: 11).Trim().ToString(),
			Ref: span.Slice(start: 107, length: 9).Trim().ToString(),
			NumberObservation: span.Slice(start: 117, length: 5).Trim().ToString(),
			NumberOpposition: span.Slice(start: 123, length: 3).Trim().ToString(),
			ObsSpan: span.Slice(start: 127, length: 9).Trim().ToString(),
			RmsResidual: span.Slice(start: 137, length: 4).Trim().ToString(),
			ComputerName: span.Slice(start: 150, length: 10).Trim().ToString(),
			Flags: span.Slice(start: 161, length: 4).Trim().ToString(),
			DesignationName: span.Slice(start: 166, length: 28).Trim().ToString(),
			ObservationLastDate: span.Slice(start: 194, length: 8).Trim().ToString()
		);

	}

	/// <summary>Returns a string representation of the PlanetoidRecord for debugging purposes.</summary>
	/// <returns>A string representation of the PlanetoidRecord.</returns>
	/// <remarks>This property is used by the DebuggerDisplay attribute to provide a concise view of the record's contents during debugging.</remarks>
	private string DebuggerDisplay => ToString();
}