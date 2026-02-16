// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB;

public partial class TerminologyForm
{
	/// <summary>
	/// Enumeration of terminology elements used in the application.
	/// </summary>
	/// <remarks>
	/// This enumeration defines the various terminology elements that can be selected.
	/// </remarks>
	public enum TerminologyElement
	{
		/// <summary>
		/// Index number of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the index number of the element.
		/// </remarks>
		IndexNumber,
		/// <summary>
		/// Readable designation of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the readable designation of the element.
		/// </remarks>
		ReadableDesignation,
		/// <summary>
		/// Epoch of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the epoch of the element.
		/// </remarks>
		Epoch,
		/// <summary>
		/// Mean anomaly at the epoch of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the mean anomaly at the epoch of the element.
		/// </remarks>
		MeanAnomalyAtTheEpoch,
		/// <summary>
		/// Argument of perihelion of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the argument of perihelion of the element.
		/// </remarks>
		ArgumentOfThePerihelion,
		/// <summary>
		/// Longitude of the ascending node of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the longitude of the ascending node of the element.
		/// </remarks>
		LongitudeOfTheAscendingNode,
		/// <summary>
		/// Inclination to the ecliptic of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the inclination to the ecliptic of the element.
		/// </remarks>
		InclinationToTheEcliptic,
		/// <summary>
		/// Orbital eccentricity of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the orbital eccentricity of the element.
		/// </remarks>
		OrbitalEccentricity,
		/// <summary>
		/// Mean daily motion of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the mean daily motion of the element.
		/// </remarks>
		MeanDailyMotion,
		/// <summary>
		/// Semi-major axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the semi-major axis of the element.
		/// </remarks>
		SemiMajorAxis,
		/// <summary>
		/// Absolute magnitude of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the absolute magnitude of the element.
		/// </remarks>
		AbsoluteMagnitude,
		/// <summary>
		/// Slope parameter of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the slope parameter of the element.
		/// </remarks>
		SlopeParameter,
		/// <summary>
		/// Reference of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the reference of the element.
		/// </remarks>
		Reference,
		/// <summary>
		/// Number of oppositions of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the number of oppositions of the element.
		/// </remarks>
		NumberOfOppositions,
		/// <summary>
		/// Number of observations of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the number of observations of the element.
		/// </remarks>
		NumberOfObservations,
		/// <summary>
		/// Observation span of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the observation span of the element.
		/// </remarks>
		ObservationSpan,
		/// <summary>
		/// RMS residual of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the RMS residual of the element.
		/// </remarks>
		RmsResidual,
		/// <summary>
		/// Computer name of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the computer name of the element.
		/// </remarks>
		ComputerName,
		/// <summary>
		/// Flags of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the flags of the element.
		/// </remarks>
		Flags,
		/// <summary>
		/// Date of last observation of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the date of last observation of the element.
		/// </remarks>
		DateOfLastObservation,
		/// <summary>
		/// Linear eccentricity of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the linear eccentricity of the element.
		/// </remarks>
		LinearEccentricity,
		/// <summary>
		/// Semi-minor axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the semi-minor axis of the element.
		/// </remarks>
		SemiMinorAxis,
		/// <summary>
		/// Major axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the major axis of the element.
		/// </remarks>
		MajorAxis,
		/// <summary>
		/// Minor axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the minor axis of the element.
		/// </remarks>
		MinorAxis,
		/// <summary>
		/// Eccentric anomaly of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the eccentric anomaly of the element.
		/// </remarks>
		EccentricAnomaly,
		/// <summary>
		/// True anomaly of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the true anomaly of the element.
		/// </remarks>
		TrueAnomaly,
		/// <summary>
		/// Perihelion distance of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the perihelion distance of the element.
		/// </remarks>
		PerihelionDistance,
		/// <summary>
		/// Aphelion distance of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the aphelion distance of the element.
		/// </remarks>
		AphelionDistance,
		/// <summary>
		/// Longitude of the descending node of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the longitude of the descending node of the element.
		/// </remarks>
		LongitudeOfTheDescendingNode,
		/// <summary>
		/// Argument of aphelion of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the argument of aphelion of the element.
		/// </remarks>
		ArgumentOfTheAphelion,
		/// <summary>
		/// Focal parameter of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the focal parameter of the element.
		/// </remarks>
		FocalParameter,
		/// <summary>
		/// Semi-latus rectum of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the semi-latus rectum of the element.
		/// </remarks>
		SemiLatusRectum,
		/// <summary>
		/// Latus rectum of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the latus rectum of the element.
		/// </remarks>
		LatusRectum,
		/// <summary>
		/// Orbital period of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the orbital period of the element.
		/// </remarks>
		OrbitalPeriod,
		/// <summary>
		/// Orbital area of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the orbital area of the element.
		/// </remarks>
		OrbitalArea,
		/// <summary>
		/// Orbital perimeter of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the orbital perimeter of the element.
		/// </remarks>
		OrbitalPerimeter,
		/// <summary>
		/// Semi-mean axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the semi-mean axis of the element.
		/// </remarks>
		SemiMeanAxis,
		/// <summary>
		/// Mean axis of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the mean axis of the element.
		/// </remarks>
		MeanAxis,
		/// <summary>
		/// Standard gravitational parameter of the element.
		/// </summary>
		/// <remarks>
		/// This field stores the standard gravitational parameter of the element.
		/// </remarks>
		StandardGravitationalParameter
	}
}