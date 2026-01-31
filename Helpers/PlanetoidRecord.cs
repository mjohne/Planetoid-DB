namespace Planetoid_DB.Helpers;

/// <summary>
/// Represents a single planetoid dataset.
/// </summary>
/// <param name="Index"></param>
/// <param name="MagAbs"></param>
/// <param name="SlopeParam"></param>
/// <param name="Epoch"></param>
/// <param name="MeanAnomaly"></param>
/// <param name="ArgPeri"></param>
/// <param name="LongAscNode"></param>
/// <param name="Incl"></param>
/// <param name="OrbEcc"></param>
/// <param name="Motion"></param>
/// <param name="SemiMajorAxis"></param>
/// <param name="Ref"></param>
/// <param name="NumberObservation"></param>
/// <param name="NumberOpposition"></param>
/// <param name="ObsSpan"></param>
/// <param name="RmsResidual"></param>
/// <param name="ComputerName"></param>
/// <param name="Flags"></param>
/// <param name="DesignationName"></param>
/// <param name="ObservationLastDate"></param>
/// <remarks>
/// This record struct is used to represent a single planetoid dataset.
/// </remarks>
public readonly record struct PlanetoidRecord(
	string Index, // Gets the index of the planetoid.
	string MagAbs, // Gets the absolute magnitude of the planetoid.
	string SlopeParam, // Gets the slope parameter of the planetoid.
	string Epoch, // Gets the epoch of the planetoid.
	string MeanAnomaly, // Gets the mean anomaly of the planetoid.
	string ArgPeri, // Gets the argument of perihelion of the planetoid.
	string LongAscNode, // Gets the longitude of the ascending node of the planetoid.
	string Incl, // Gets the inclination of the planetoid.
	string OrbEcc, // Gets the orbital eccentricity of the planetoid.
	string Motion, // Gets the mean daily motion of the planetoid.
	string SemiMajorAxis, // Gets the semi-major axis of the planetoid.
	string Ref, // Gets the reference of the planetoid.
	string NumberObservation, // Gets the number of observations of the planetoid.
	string NumberOpposition, // Gets the number of oppositions of the planetoid.
	string ObsSpan, // Gets the observation span of the planetoid.
	string RmsResidual, // Gets the root mean square residual of the planetoid.
	string ComputerName, // Gets the computer name of the planetoid.
	string Flags, // Gets the flags of the planetoid.
	string DesignationName, // Gets the designation name of the planetoid.
	string ObservationLastDate // Gets the observation last date of the planetoid.
)
{
	/// <summary>
	/// Parses a raw line (Fixed-Width) into a PlanetoidRecord object.
	/// </summary>
	/// <param name="rawLine">The raw line to parse.</param>
	/// <returns>A PlanetoidRecord object.</returns>
	/// <exception cref="ArgumentException">Thrown when the raw line is invalid.</exception>
	/// <remarks>
	/// This method expects the raw line to be in a fixed-width format.
	/// </remarks>
	public static PlanetoidRecord Parse(string rawLine)
	{
		// Validate input
		if (string.IsNullOrWhiteSpace(value: rawLine) || rawLine.Length < 202)
		{
			// Return an empty PlanetoidRecord if the input is invalid
			return new PlanetoidRecord();
		}
		// Extract fields based on fixed-width positions
		return new PlanetoidRecord(
			Index: rawLine[..7].Trim(),
			MagAbs: rawLine[8..13].Trim(),
			SlopeParam: rawLine[14..19].Trim(),
			Epoch: rawLine[20..25].Trim(),
			MeanAnomaly: rawLine[26..35].Trim(),
			ArgPeri: rawLine[37..46].Trim(),
			LongAscNode: rawLine[48..57].Trim(),
			Incl: rawLine[59..68].Trim(),
			OrbEcc: rawLine[70..79].Trim(),
			Motion: rawLine[80..91].Trim(),
			SemiMajorAxis: rawLine[92..103].Trim(),
			Ref: rawLine[107..116].Trim(),
			NumberObservation: rawLine[117..122].Trim(),
			NumberOpposition: rawLine[123..126].Trim(),
			ObsSpan: rawLine[127..136].Trim(),
			RmsResidual: rawLine[137..141].Trim(),
			ComputerName: rawLine[150..160].Trim(),
			Flags: rawLine[161..165].Trim(),
			DesignationName: rawLine[166..194].Trim(),
			ObservationLastDate: rawLine[194..202].Trim()
		);
	}
}