/*
 * File:        PlanetaryInformationForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that displays orbital and physical properties of the eight planets of the Solar System.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Represents a form that displays orbital and physical properties of the eight planets of the Solar System.</summary>
/// <remarks>The form provides a <see cref="ListView"/> with one column per planet and one row per property. Column headers can be clicked to toggle ascending/descending sorting, and double-clicking a cell copies its value to the Windows clipboard.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class PlanetaryInformationForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages and errors for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column in the ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header to toggle the sort order.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="PlanetaryInformationForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public PlanetaryInformationForm()
	{
		// Initialize the form components
		InitializeComponent();
		kryptonStatusStrip.Dock = DockStyle.Bottom;
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Represents a fixed set of well-known base parameters for a single planet.</summary>
	/// <remarks>Values are current best reference values (NASA/IAU) used to compute all derived orbital and physical properties displayed by the form.</remarks>
	private sealed class PlanetData
	{
		/// <summary>Gets the display name of the planet.</summary>
		public required string Name { get; init; }

		/// <summary>Gets the semi-major axis of the orbit in astronomical units (AU).</summary>
		public required double SemiMajorAxisAu { get; init; }

		/// <summary>Gets the numerical eccentricity of the orbit (dimensionless).</summary>
		public required double Eccentricity { get; init; }

		/// <summary>Gets the inclination of the orbital plane to the ecliptic in degrees.</summary>
		public required double InclinationDeg { get; init; }

		/// <summary>Gets the longitude of the ascending node in degrees.</summary>
		public required double LongitudeAscendingNodeDeg { get; init; }

		/// <summary>Gets the argument of perihelion in degrees.</summary>
		public required double ArgumentPerihelionDeg { get; init; }

		/// <summary>Gets the mean anomaly at the reference epoch in degrees.</summary>
		public required double MeanAnomalyDeg { get; init; }

		/// <summary>Gets the sidereal orbital period in Julian years.</summary>
		public required double SiderealPeriodYears { get; init; }

		/// <summary>Gets the synodic period as seen from Earth in Earth days.</summary>
		public required double SynodicPeriodDays { get; init; }

		/// <summary>Gets the equatorial diameter in kilometres.</summary>
		public required double EquatorialDiameterKm { get; init; }

		/// <summary>Gets the flattening (oblateness) of the planet (dimensionless).</summary>
		public required double Flattening { get; init; }

		/// <summary>Gets the mass in kilograms.</summary>
		public required double MassKg { get; init; }

		/// <summary>Gets the volume in cubic kilometres.</summary>
		public required double VolumeKm3 { get; init; }

		/// <summary>Gets the mean density in kilograms per cubic metre.</summary>
		public required double DensityKgPerM3 { get; init; }

		/// <summary>Gets the equatorial surface gravity in metres per second squared.</summary>
		public required double SurfaceGravityMPerS2 { get; init; }

		/// <summary>Gets the escape velocity in kilometres per second.</summary>
		public required double EscapeVelocityKmPerS { get; init; }

		/// <summary>Gets the sidereal rotation period in Earth days.</summary>
		public required double RotationPeriodDays { get; init; }

		/// <summary>Gets the equatorial rotational velocity in metres per second.</summary>
		public required double EquatorialRotationVelocityMPerS { get; init; }

		/// <summary>Gets the axial tilt (obliquity to the orbit) in degrees.</summary>
		public required double AxialTiltDeg { get; init; }

		/// <summary>Gets the geometric albedo (dimensionless).</summary>
		public required double GeometricAlbedo { get; init; }

		/// <summary>Gets the maximum apparent magnitude as seen from Earth.</summary>
		public required double MaxApparentMagnitude { get; init; }

		/// <summary>Gets the absolute magnitude H.</summary>
		public required double AbsoluteMagnitude { get; init; }
	}

	/// <summary>Astronomical unit expressed in kilometres.</summary>
	/// <remarks>Used to convert distances from AU to km when composing the display values.</remarks>
	private const double AuInKm = 149597870.700;

	/// <summary>Standard gravitational parameter of the Sun in km^3/s^2.</summary>
	/// <remarks>Used to compute perihelion and aphelion velocities via the vis-viva equation.</remarks>
	private const double SunGmKm3PerS2 = 1.32712440018e11;

	/// <summary>Number of seconds in a Julian year.</summary>
	/// <remarks>Used to convert sidereal orbital periods from years to seconds when required.</remarks>
	private const double SecondsPerJulianYear = 365.25 * 86400.0;

	/// <summary>Julian Date of the J2000.0 epoch (2000 January 1.5 TT).</summary>
	/// <remarks>Used as the reference epoch for propagating mean anomaly values to the current date.</remarks>
	private const double J2000Jd = 2451545.0;

	/// <summary>Returns the reference dataset used to populate the ListView.</summary>
	/// <returns>An array containing the base parameters of the eight planets in the canonical order Mercury..Neptune.</returns>
	/// <remarks>Values are taken from the NASA Planetary Fact Sheet and the IAU J2000 mean orbital elements. Angles are in degrees; distances in AU or km; masses in kg; velocities in km/s or m/s as documented on each property.</remarks>
	private static PlanetData[] Planets =>
	[
		new PlanetData
		{
			Name = "Mercury",
			SemiMajorAxisAu = 0.38709893,
			Eccentricity = 0.20563069,
			InclinationDeg = 7.00487,
			LongitudeAscendingNodeDeg = 48.33167,
			ArgumentPerihelionDeg = 29.12478,
			MeanAnomalyDeg = 174.7948,
			SiderealPeriodYears = 0.2408467,
			SynodicPeriodDays = 115.88,
			EquatorialDiameterKm = 4879.4,
			Flattening = 0.0000,
			MassKg = 3.3011e23,
			VolumeKm3 = 6.083e10,
			DensityKgPerM3 = 5427.0,
			SurfaceGravityMPerS2 = 3.70,
			EscapeVelocityKmPerS = 4.25,
			RotationPeriodDays = 58.6462,
			EquatorialRotationVelocityMPerS = 3.026,
			AxialTiltDeg = 0.034,
			GeometricAlbedo = 0.142,
			MaxApparentMagnitude = -2.48,
			AbsoluteMagnitude = -0.613
		},
		new PlanetData
		{
			Name = "Venus",
			SemiMajorAxisAu = 0.72333199,
			Eccentricity = 0.00677323,
			InclinationDeg = 3.39471,
			LongitudeAscendingNodeDeg = 76.68069,
			ArgumentPerihelionDeg = 54.85229,
			MeanAnomalyDeg = 50.4161,
			SiderealPeriodYears = 0.6151973,
			SynodicPeriodDays = 583.92,
			EquatorialDiameterKm = 12103.6,
			Flattening = 0.0000,
			MassKg = 4.8675e24,
			VolumeKm3 = 9.2843e11,
			DensityKgPerM3 = 5243.0,
			SurfaceGravityMPerS2 = 8.87,
			EscapeVelocityKmPerS = 10.36,
			RotationPeriodDays = -243.0187,
			EquatorialRotationVelocityMPerS = 1.81,
			AxialTiltDeg = 177.36,
			GeometricAlbedo = 0.689,
			MaxApparentMagnitude = -4.92,
			AbsoluteMagnitude = -4.384
		},
		new PlanetData
		{
			Name = "Earth",
			SemiMajorAxisAu = 1.00000011,
			Eccentricity = 0.01671022,
			InclinationDeg = 0.00005,
			LongitudeAscendingNodeDeg = -11.26064,
			ArgumentPerihelionDeg = 114.20783,
			MeanAnomalyDeg = 357.5291,
			SiderealPeriodYears = 1.0000174,
			SynodicPeriodDays = double.NaN,
			EquatorialDiameterKm = 12756.2,
			Flattening = 0.0033528,
			MassKg = 5.97237e24,
			VolumeKm3 = 1.08321e12,
			DensityKgPerM3 = 5514.0,
			SurfaceGravityMPerS2 = 9.80665,
			EscapeVelocityKmPerS = 11.186,
			RotationPeriodDays = 0.99726968,
			EquatorialRotationVelocityMPerS = 465.1,
			AxialTiltDeg = 23.4392811,
			GeometricAlbedo = 0.434,
			MaxApparentMagnitude = double.NaN,
			AbsoluteMagnitude = -3.99
		},
		new PlanetData
		{
			Name = "Mars",
			SemiMajorAxisAu = 1.52366231,
			Eccentricity = 0.09341233,
			InclinationDeg = 1.85061,
			LongitudeAscendingNodeDeg = 49.57854,
			ArgumentPerihelionDeg = 286.4623,
			MeanAnomalyDeg = 19.3870,
			SiderealPeriodYears = 1.8808476,
			SynodicPeriodDays = 779.94,
			EquatorialDiameterKm = 6792.4,
			Flattening = 0.00589,
			MassKg = 6.4171e23,
			VolumeKm3 = 1.6318e11,
			DensityKgPerM3 = 3933.5,
			SurfaceGravityMPerS2 = 3.71,
			EscapeVelocityKmPerS = 5.03,
			RotationPeriodDays = 1.02595676,
			EquatorialRotationVelocityMPerS = 241.17,
			AxialTiltDeg = 25.19,
			GeometricAlbedo = 0.170,
			MaxApparentMagnitude = -2.94,
			AbsoluteMagnitude = -1.601
		},
		new PlanetData
		{
			Name = "Jupiter",
			SemiMajorAxisAu = 5.20336301,
			Eccentricity = 0.04839266,
			InclinationDeg = 1.30530,
			LongitudeAscendingNodeDeg = 100.55615,
			ArgumentPerihelionDeg = 275.066,
			MeanAnomalyDeg = 20.020,
			SiderealPeriodYears = 11.862615,
			SynodicPeriodDays = 398.88,
			EquatorialDiameterKm = 142984.0,
			Flattening = 0.06487,
			MassKg = 1.8982e27,
			VolumeKm3 = 1.4313e15,
			DensityKgPerM3 = 1326.0,
			SurfaceGravityMPerS2 = 24.79,
			EscapeVelocityKmPerS = 60.20,
			RotationPeriodDays = 0.41354,
			EquatorialRotationVelocityMPerS = 12572.0,
			AxialTiltDeg = 3.13,
			GeometricAlbedo = 0.538,
			MaxApparentMagnitude = -2.94,
			AbsoluteMagnitude = -9.395
		},
		new PlanetData
		{
			Name = "Saturn",
			SemiMajorAxisAu = 9.53707032,
			Eccentricity = 0.05415060,
			InclinationDeg = 2.48446,
			LongitudeAscendingNodeDeg = 113.71504,
			ArgumentPerihelionDeg = 336.013862,
			MeanAnomalyDeg = 317.020,
			SiderealPeriodYears = 29.447498,
			SynodicPeriodDays = 378.09,
			EquatorialDiameterKm = 120536.0,
			Flattening = 0.09796,
			MassKg = 5.6834e26,
			VolumeKm3 = 8.2713e14,
			DensityKgPerM3 = 687.0,
			SurfaceGravityMPerS2 = 10.44,
			EscapeVelocityKmPerS = 36.09,
			RotationPeriodDays = 0.44401,
			EquatorialRotationVelocityMPerS = 9871.0,
			AxialTiltDeg = 26.73,
			GeometricAlbedo = 0.499,
			MaxApparentMagnitude = -0.55,
			AbsoluteMagnitude = -8.914
		},
		new PlanetData
		{
			Name = "Uranus",
			SemiMajorAxisAu = 19.19126393,
			Eccentricity = 0.04716771,
			InclinationDeg = 0.76986,
			LongitudeAscendingNodeDeg = 74.22988,
			ArgumentPerihelionDeg = 96.541318,
			MeanAnomalyDeg = 142.2386,
			SiderealPeriodYears = 84.016846,
			SynodicPeriodDays = 369.66,
			EquatorialDiameterKm = 51118.0,
			Flattening = 0.02293,
			MassKg = 8.6810e25,
			VolumeKm3 = 6.833e13,
			DensityKgPerM3 = 1270.0,
			SurfaceGravityMPerS2 = 8.87,
			EscapeVelocityKmPerS = 21.38,
			RotationPeriodDays = -0.71833,
			EquatorialRotationVelocityMPerS = 2588.9,
			AxialTiltDeg = 97.77,
			GeometricAlbedo = 0.488,
			MaxApparentMagnitude = 5.38,
			AbsoluteMagnitude = -7.110
		},
		new PlanetData
		{
			Name = "Neptune",
			SemiMajorAxisAu = 30.06896348,
			Eccentricity = 0.00858587,
			InclinationDeg = 1.76917,
			LongitudeAscendingNodeDeg = 131.72169,
			ArgumentPerihelionDeg = 265.646853,
			MeanAnomalyDeg = 256.228,
			SiderealPeriodYears = 164.79132,
			SynodicPeriodDays = 367.49,
			EquatorialDiameterKm = 49528.0,
			Flattening = 0.01708,
			MassKg = 1.02413e26,
			VolumeKm3 = 6.254e13,
			DensityKgPerM3 = 1638.0,
			SurfaceGravityMPerS2 = 11.15,
			EscapeVelocityKmPerS = 23.56,
			RotationPeriodDays = 0.67125,
			EquatorialRotationVelocityMPerS = 2680.4,
			AxialTiltDeg = 28.32,
			GeometricAlbedo = 0.442,
			MaxApparentMagnitude = 7.67,
			AbsoluteMagnitude = -7.00
		}
	];

	/// <summary>Solves Kepler's equation <c>M = E - e·sin(E)</c> for the eccentric anomaly E.</summary>
	/// <param name="meanAnomalyRad">The mean anomaly in radians.</param>
	/// <param name="eccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The eccentric anomaly in radians.</returns>
	/// <remarks>Uses a Newton-Raphson iteration; converges within a handful of steps for the small eccentricities of the major planets.</remarks>
	private static double SolveKepler(double meanAnomalyRad, double eccentricity)
	{
		double e = meanAnomalyRad;
		for (int i = 0; i < 30; i++)
		{
			double delta = (e - (eccentricity * Math.Sin(a: e)) - meanAnomalyRad) / (1.0 - (eccentricity * Math.Cos(d: e)));
			e -= delta;
			if (Math.Abs(value: delta) < 1e-12)
			{
				break;
			}
		}
		return e;
	}

	/// <summary>Computes the true anomaly from the eccentric anomaly and eccentricity.</summary>
	/// <param name="eccentricAnomalyRad">The eccentric anomaly in radians.</param>
	/// <param name="eccentricity">The numerical eccentricity of the orbit.</param>
	/// <returns>The true anomaly in radians in the range <c>[0, 2π)</c>.</returns>
	/// <remarks>Uses the standard half-angle formula; result is normalised to the range <c>[0, 2π)</c>.</remarks>
	private static double TrueAnomalyFromEccentric(double eccentricAnomalyRad, double eccentricity)
	{
		double sqrt = Math.Sqrt(d: (1.0 + eccentricity) / (1.0 - eccentricity));
		double v = 2.0 * Math.Atan2(y: sqrt * Math.Sin(a: eccentricAnomalyRad / 2.0), x: Math.Cos(d: eccentricAnomalyRad / 2.0));
		if (v < 0.0)
		{
			v += 2.0 * Math.PI;
		}
		return v;
	}

	/// <summary>Normalises an angle in degrees to the range <c>[0, 360)</c>.</summary>
	/// <param name="degrees">The input angle in degrees.</param>
	/// <returns>The equivalent angle in the range <c>[0, 360)</c>.</returns>
	/// <remarks>Used when computing derived angular quantities such as the longitude of the descending node.</remarks>
	private static double NormalizeDegrees(double degrees)
	{
		double v = degrees % 360.0;
		if (v < 0.0)
		{
			v += 360.0;
		}
		return v;
	}

	/// <summary>Computes the mean anomaly at the current epoch from a reference epoch value.</summary>
	/// <param name="meanAnomalyAtEpochDeg">Mean anomaly at the reference epoch in degrees.</param>
	/// <param name="siderealPeriodYears">Sidereal orbital period in Julian years.</param>
	/// <param name="epochJd">Julian Date of the reference epoch.</param>
	/// <param name="nowJd">Julian Date at which to evaluate the current mean anomaly.</param>
	/// <returns>The propagated mean anomaly in degrees normalized to the range <c>[0, 360)</c>.</returns>
	/// <remarks>Uses a constant mean motion computed from the sidereal period.</remarks>
	private static double CurrentMeanAnomalyDeg(double meanAnomalyAtEpochDeg, double siderealPeriodYears, double epochJd, double nowJd)
	{
		double meanMotionDegPerDay = 360.0 / (siderealPeriodYears * 365.25);
		return NormalizeDegrees(degrees: meanAnomalyAtEpochDeg + (meanMotionDegPerDay * (nowJd - epochJd)));
	}

	/// <summary>Converts a <see cref="DateTime"/> value to a Julian Date.</summary>
	/// <param name="dateTime">The date/time to convert (UTC recommended).</param>
	/// <returns>The Julian Date corresponding to <paramref name="dateTime"/>.</returns>
	private static double DateTimeToJulianDate(DateTime dateTime)
	{
		int year = dateTime.Year;
		int month = dateTime.Month;
		double day = dateTime.Day + ((dateTime.Hour + ((dateTime.Minute + (dateTime.Second / 60.0)) / 60.0)) / 24.0);
		if (month <= 2)
		{
			year--;
			month += 12;
		}
		int a = year / 100;
		int b = 2 - a + (a / 4);
		return (int)(365.25 * (year + 4716)) + (int)(30.6001 * (month + 1)) + day + b - 1524.5;
	}

	/// <summary>Formats a double value using the current culture with a fixed number of significant digits.</summary>
	/// <param name="value">The value to format.</param>
	/// <param name="format">The .NET numeric format string (for example <c>"G6"</c> or <c>"0.###"</c>).</param>
	/// <returns>The formatted string, or an empty string when <paramref name="value"/> is <see cref="double.NaN"/>.</returns>
	/// <remarks>Uses <see cref="CultureInfo.CurrentCulture"/> so displayed numbers respect the user's locale.</remarks>
	private static string Format(double value, string format = "G6")
	{
		return double.IsNaN(d: value) ? string.Empty : value.ToString(format: format, provider: CultureInfo.CurrentCulture);
	}

	/// <summary>Computes the derived property values for a single planet.</summary>
	/// <param name="p">The planet base parameters.</param>
	/// <param name="nowJd">The current Julian date.</param>
	/// <returns>An array of formatted strings, one per property row, in the same order as <see cref="PropertyLabels"/>.</returns>
	/// <remarks>All derived quantities (semi-minor axis, orbital area, perihelion/aphelion distances and velocities, pole diameter, descending node longitude, argument of aphelion, eccentric and true anomaly) are computed from the base parameters using standard Keplerian and geometric formulas.</remarks>
	private static string[] ComputePlanetValues(PlanetData p, double nowJd)
	{
		// Orbital geometry (in km, computed from AU)
		double aKm = p.SemiMajorAxisAu * AuInKm;
		double bKm = aKm * Math.Sqrt(d: 1.0 - (p.Eccentricity * p.Eccentricity));
		double perihelionKm = aKm * (1.0 - p.Eccentricity);
		double aphelionKm = aKm * (1.0 + p.Eccentricity);
		double semiLatusRectumKm = aKm * (1.0 - (p.Eccentricity * p.Eccentricity));
		double latusRectumKm = 2.0 * semiLatusRectumKm;
		double linearEccentricityKm = aKm * p.Eccentricity;
		double orbitDiameterKm = 2.0 * aKm;
		double orbitAreaKm2 = Math.PI * aKm * bKm;

		// Anomalies at the current epoch (propagated from J2000)
		double currentMeanAnomalyDeg = CurrentMeanAnomalyDeg(
			meanAnomalyAtEpochDeg: p.MeanAnomalyDeg,
			siderealPeriodYears: p.SiderealPeriodYears,
			epochJd: J2000Jd,
			nowJd: nowJd);
		double meanAnomalyRad = currentMeanAnomalyDeg * Math.PI / 180.0;
		double eccentricAnomalyRad = SolveKepler(meanAnomalyRad: meanAnomalyRad, eccentricity: p.Eccentricity);
		double trueAnomalyRad = TrueAnomalyFromEccentric(eccentricAnomalyRad: eccentricAnomalyRad, eccentricity: p.Eccentricity);
		double eccentricAnomalyDeg = eccentricAnomalyRad * 180.0 / Math.PI;
		double trueAnomalyDeg = trueAnomalyRad * 180.0 / Math.PI;

		// Angular derived quantities
		double longitudeDescendingNodeDeg = NormalizeDegrees(degrees: p.LongitudeAscendingNodeDeg + 180.0);
		double argumentAphelionDeg = NormalizeDegrees(degrees: p.ArgumentPerihelionDeg + 180.0);

		// Orbital velocities via vis-viva (km/s)
		double orbitalPerimeterKm = DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: p.SemiMajorAxisAu, numericalEccentricity: p.Eccentricity) * AuInKm;
		double meanOrbitalVelocityKmPerS = orbitalPerimeterKm / (p.SiderealPeriodYears * SecondsPerJulianYear);
		double perihelionVelocityKmPerS = Math.Sqrt(d: SunGmKm3PerS2 * ((2.0 / perihelionKm) - (1.0 / aKm)));
		double aphelionVelocityKmPerS = Math.Sqrt(d: SunGmKm3PerS2 * ((2.0 / aphelionKm) - (1.0 / aKm)));

		// Physical derived
		double polarDiameterKm = p.EquatorialDiameterKm * (1.0 - p.Flattening);

		// Build display rows in the exact order of GetPropertyLabels()
		return
		[
			// Semi-major axis (AU)
			Format(value: p.SemiMajorAxisAu, format: "G6"),
			// Semi-minor axis (AU)
			Format(value: bKm / AuInKm, format: "G6"),
			// Mayor axis (AU)
			Format(value: 2.0 * p.SemiMajorAxisAu, format: "G6"),
			// Minor axis (AU)
			Format(value: 2.0 * bKm / AuInKm, format: "G6"),
			// Perihelion distance (AU)
			Format(value: perihelionKm / AuInKm, format: "G6"),
			// Aphelion distance (AU)
			Format(value: aphelionKm / AuInKm, format: "G6"),
			// Semi-latus rectum (AU)
			Format(value: semiLatusRectumKm / AuInKm, format: "G6"),
			// Latus rectum (AU)
			Format(value: latusRectumKm / AuInKm, format: "G6"),
			// Numeric eccentricity
			Format(value: p.Eccentricity, format: "G6"),
			// Linear ecceentricity (AU)
			Format(value: linearEccentricityKm / AuInKm, format: "G6"),
			// Orbital diameter (AU)
			Format(value: orbitDiameterKm / AuInKm, format: "G6"),
			// Orbital area (km²)
			Format(value: orbitAreaKm2, format: "G6"),
			// Orbital inclination (°)
			Format(value: p.InclinationDeg, format: "G6"),
			// Mean anomaly (°)
			Format(value: currentMeanAnomalyDeg, format: "G6"),
			// Eccentric anomaly (°)
			Format(value: NormalizeDegrees(degrees: eccentricAnomalyDeg), format: "G6"),
			// True anomaly (°)
			Format(value: NormalizeDegrees(degrees: trueAnomalyDeg), format: "G6"),
			// Longitude of the ascending node (°)
			Format(value: NormalizeDegrees(degrees: p.LongitudeAscendingNodeDeg), format: "G6"),
			// Longitude of the descending node (°)
			Format(value: longitudeDescendingNodeDeg, format: "G6"),
			// Argument of perihelion (°)
			Format(value: NormalizeDegrees(degrees: p.ArgumentPerihelionDeg), format: "G6"),
			// Argument of aphelion (°)
			Format(value: argumentAphelionDeg, format: "G6"),
			// Sidereal period (year)
			Format(value: p.SiderealPeriodYears, format: "G6"),
			// synodic period (days)
			Format(value: p.SynodicPeriodDays, format: "G6"),
			// Minimum orbital velocity (km/s)
			Format(value: aphelionVelocityKmPerS, format: "G6"),
			// Mean orbital velocity (km/s)
			Format(value: meanOrbitalVelocityKmPerS, format: "G6"),
			// Ainimum orbital velocity (km/s)
			Format(value: perihelionVelocityKmPerS, format: "G6"),
			// Equatorial diameter (km)
			Format(value: p.EquatorialDiameterKm, format: "G6"),
			// Polar diameter (km)
			Format(value: polarDiameterKm, format: "G6"),
			// Flattening
			Format(value: p.Flattening, format: "G6"),
			// Mass (kg)
			Format(value: p.MassKg, format: "G6"),
			// Volume (km³)
			Format(value: p.VolumeKm3, format: "G6"),
			// Mean density (kg/m³)
			Format(value: p.DensityKgPerM3, format: "G6"),
			// Surface gravity (m/s²)
			Format(value: p.SurfaceGravityMPerS2, format: "G6"),
			// Escape velocity (km/s)
			Format(value: p.EscapeVelocityKmPerS, format: "G6"),
			// Rotation period (days)
			Format(value: p.RotationPeriodDays, format: "G6"),
			// Equatorial rotation velocity (m/s)
			Format(value: p.EquatorialRotationVelocityMPerS, format: "G6"),
			// Axial tilt (°)
			Format(value: p.AxialTiltDeg, format: "G6"),
			// geometric albedo
			Format(value: p.GeometricAlbedo, format: "G6"),
			// Maximal apparent magnitude (mag)
			Format(value: p.MaxApparentMagnitude, format: "G6"),
			// Absolute magnitude (mag)
			Format(value: p.AbsoluteMagnitude, format: "G6")
		];
	}

	/// <summary>Returns the display labels for each property row.</summary>
	/// <returns>An array of localized labels, one per property row, matching the order used by <see cref="ComputePlanetValues"/>.</returns>
	/// <remarks>Labels include the physical unit in parentheses where applicable. The order of entries in this array must match the order of the values returned by <see cref="ComputePlanetValues"/>.</remarks>
	private static string[] PropertyLabels =>
	[
		"Semi-major axis (AU)",
		"Semi-minor axis (AU)",
		"Major axis (AU)",
		"Minor axis (AU)",
		"Perihelion distance (AU)",
		"Aphelion distance (AU)",
		"Semi-latus rectum (AU)",
		"Latus rectum (AU)",
		"Numerical eccentricity",
		"Linear eccentricity (AU)",
		"Orbit diameter (AU)",
		"Orbit area (km²)",
		"Orbital inclination (°)",
		"Mean anomaly (°)",
		"Eccentric anomaly (°)",
		"True anomaly (°)",
		"Longitude of ascending node (°)",
		"Longitude of descending node (°)",
		"Argument of perihelion (°)",
		"Argument of aphelion (°)",
		"Sidereal period (a)",
		"Synodic period (d)",
		"Minimum orbital velocity (km/s)",
		"Mean orbital velocity (km/s)",
		"Maximum orbital velocity (km/s)",
		"Equatorial diameter (km)",
		"Polar diameter (km)",
		"Flattening",
		"Mass (kg)",
		"Volume (km³)",
		"Mean density (kg/m³)",
		"Surface gravity (m/s²)",
		"Escape velocity (km/s)",
		"Rotation period (d)",
		"Equatorial rotation velocity (m/s)",
		"Axial tilt (°)",
		"Geometric albedo",
		"Maximum apparent magnitude (mag)",
		"Absolute magnitude (mag)"
	];

	/// <summary>Populates the ListView with one row per property and one column per planet.</summary>
	/// <remarks>Uses <see cref="ListView.BeginUpdate"/> and <see cref="ListView.EndUpdate"/> to keep the UI responsive while filling in the ~39 rows.</remarks>
	private void LoadPlanetaryInformation()
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Set the cursor to wait while loading data
			Cursor.Current = Cursors.WaitCursor;
			listView.BeginUpdate();
			listView.Items.Clear();
			// Compute all planet columns up-front so we can transpose to per-property rows
			PlanetData[] planets = Planets;
			string[] labels = PropertyLabels;
			string[][] columns = new string[planets.Length][];
			double nowJd = DateTimeToJulianDate(dateTime: DateTime.UtcNow);
			for (int i = 0; i < planets.Length; i++)
			{
				columns[i] = ComputePlanetValues(p: planets[i], nowJd: nowJd);
			}
			// Build one ListViewItem per property row
			List<ListViewItem> items = new(capacity: labels.Length);
			for (int row = 0; row < labels.Length; row++)
			{
				ListViewItem item = new(text: labels[row]);
				for (int col = 0; col < planets.Length; col++)
				{
					_ = item.SubItems.Add(text: columns[col][row]);
				}
				items.Add(item: item);
			}
			listView.Items.AddRange(items: [.. items]);
			// Update the status bar with the number of properties displayed
			SetStatusBar(label: labelInformation, text: $"{listView.Items.Count} planetary properties loaded");
		}
		catch (Exception ex)
		{
			// Log the error and show it to the user
			logger.Error(exception: ex, message: "An error occurred while loading planetary information.");
			ShowErrorMessage(message: $"An error has occurred while loading planetary information: {ex.Message}");
			SetStatusBar(label: labelInformation, text: "Error loading planetary information");
		}
		finally
		{
			listView.EndUpdate();
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Populates the ListView with planetary data on form load.</remarks>
	private void PlanetaryInformationForm_Load(object sender, EventArgs e)
	{
		logger.Info(message: "PlanetaryInformationForm loaded.");
		LoadPlanetaryInformation();
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Toggles the sort order between ascending and descending when the same column is clicked repeatedly, and updates the header text with an arrow indicator.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no items in the ListView, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			logger.Warn(message: "Column click event ignored because the ListView is empty.");
			return;
		}
		// Determine the new sort order based on the clicked column and current sort state
		sortOrder = (e.Column == sortColumn && sortOrder == SortOrder.Ascending)
			? SortOrder.Descending
			: SortOrder.Ascending;
		// Update the sort column to the one that was clicked
		sortColumn = e.Column;
		// Update the column headers to indicate the current sort column and order
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.CurrentCulture) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.CurrentCulture))
			{
				headerText = headerText[2..];
			}
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	/// <summary>Handles the MouseDoubleClick event on the ListView to copy the value of the double-clicked cell to the Windows clipboard.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Uses <see cref="ListView.HitTest(Point)"/> to identify the exact sub-item under the mouse pointer and copies its text via <see cref="BaseKryptonForm.CopyToClipboard(string)"/>.</remarks>
	private void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		// Hit-test to determine which cell (sub-item) was clicked
		ListViewHitTestInfo hit = listView.HitTest(x: e.X, y: e.Y);
		if (hit.Item is null || hit.SubItem is null)
		{
			return;
		}
		string text = hit.SubItem.Text;
		if (string.IsNullOrEmpty(value: text))
		{
			return;
		}
		logger.Info(message: $"Copying planetary information cell to clipboard: {text}");
		CopyToClipboard(text: text);
	}

	#endregion
}
