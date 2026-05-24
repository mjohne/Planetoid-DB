// This file contains the implementation of the Orbit3DForm,
// which displays a 3D orbital visualization of a selected minor planet
// relative to the eight solar system planets using OpenTK (OpenGL).
using NLog;

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a 3D orbital visualization of a selected minor planet relative to the eight solar system planets.</summary>
/// <remarks>
/// <para>The form renders the orbit of the selected planetoid and all eight solar system planets as 3D ellipses in the
/// ecliptic coordinate frame using OpenTK/OpenGL. The Sun is represented as a yellow point at the origin.</para>
/// <para>Interaction: left-drag to rotate the view, right-drag to pan, scroll wheel to zoom in/out.</para>
/// <para>The part of the planetoid's orbit that lies below the ecliptic plane (ecliptic Z &lt; 0) is highlighted
/// with a semi-transparent violet color and a projected shadow on the ecliptic plane.</para>
/// <para>Current positions of the Sun, planets and the planetoid are computed from the current UTC date/time
/// and the provided Keplerian orbital elements propagated via mean motion.</para>
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class Orbit3DForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	// ---- Orbital elements of the selected planetoid ----

	/// <summary>Name or designation of the selected minor planet.</summary>
	private readonly string _planetoidName;

	/// <summary>Semi-major axis of the planetoid's orbit in AU.</summary>
	private readonly double _semiMajorAxis;

	/// <summary>Orbital eccentricity of the planetoid.</summary>
	private readonly double _eccentricity;

	/// <summary>Orbital inclination of the planetoid in degrees.</summary>
	private readonly double _inclinationDeg;

	/// <summary>Longitude of the ascending node of the planetoid in degrees.</summary>
	private readonly double _longitudeAscendingNodeDeg;

	/// <summary>Argument of perihelion of the planetoid in degrees.</summary>
	private readonly double _argumentPerihelionDeg;

	/// <summary>Mean anomaly of the planetoid at the reference epoch in degrees.</summary>
	private readonly double _meanAnomalyDeg;

	/// <summary>MPCORB packed epoch string (e.g. "K254Q" for 2025-Apr-26).</summary>
	private readonly string _epochMpcorb;

	// ---- Camera state ----

	/// <summary>Horizontal rotation angle of the camera in degrees.</summary>
	private float _yaw = 25f;

	/// <summary>Vertical rotation angle of the camera in degrees.</summary>
	private float _pitch = 20f;

	/// <summary>Camera distance from the scene origin (zoom level) in AU.</summary>
	private float _zoom = 35f;

	/// <summary>Horizontal camera pan offset.</summary>
	private float _panX;

	/// <summary>Vertical camera pan offset.</summary>
	private float _panY;

	/// <summary>Last recorded mouse cursor position for delta computation.</summary>
	private Point _lastMousePos;

	/// <summary>Whether the left mouse button is currently held down.</summary>
	private bool _leftDown;

	/// <summary>Whether the right mouse button is currently held down.</summary>
	private bool _rightDown;

	/// <summary>Whether the OpenGL context is initialized and ready for rendering.</summary>
	private bool _glReady;

	/// <summary>The embedded OpenTK GLControl that provides the OpenGL rendering surface.</summary>
	private GLControl _glControl = null!;

	/// <summary>Number of orbit path segments computed per orbit (higher = smoother ellipse).</summary>
	private const int OrbitSteps = 720;

	/// <summary>Julian Date of the J2000.0 epoch (2000 January 1.5 TT).</summary>
	private const double J2000Jd = 2451545.0;

	/// <summary>Mean daily motion of the Earth in degrees per day (Kepler: 0.9856076686°/day).</summary>
	private const double EarthMeanMotion = 0.9856076686;

	/// <summary>Planet orbital elements at J2000.0 from the Astronomical Almanac / NASA Horizons.
	/// Fields: Name, SemiMajorAxis (AU), Eccentricity, Inclination (°), LongAscNode (°), ArgPeri (°), MeanAnomaly0 (°), OpenGL color.</summary>
	/// <remarks>The mean anomaly column gives the value at the J2000.0 epoch (JD 2451545.0).
	/// The mean motion for each planet is computed from Kepler's third law: n = 0.9856076686 / a^1.5 deg/day.</remarks>
	private static readonly (string Name, double A, double E, double I, double Om, double Peri, double M0, Color Col)[] Planets =
	[
		("Mercury",  0.38709927, 0.20563593,  7.00497902,  48.33076593,  77.45779628, 252.25032350, Color.FromArgb(0xC0, 0xC0, 0xC8)),
		("Venus",    0.72333566, 0.00677672,  3.39467605,  76.67984255, 131.60246718, 181.97909950, Color.FromArgb(0xE8, 0xD0, 0x90)),
		("Earth",    1.00000261, 0.01671123,  0.00001531,   0.0,        102.93768193, 100.46457166, Color.FromArgb(0x40, 0x90, 0xFF)),
		("Mars",     1.52371034, 0.09339410,  1.84969142,  49.55953891, -23.94362959,  -4.55343205, Color.FromArgb(0xE0, 0x60, 0x30)),
		("Jupiter",  5.20288700, 0.04838624,  1.30439695, 100.47390909,  14.72847983,  34.39644051, Color.FromArgb(0xE8, 0xC0, 0x88)),
		("Saturn",   9.53667594, 0.05386179,  2.48599187, 113.66242448,  92.59887831,  49.95424423, Color.FromArgb(0xD8, 0xC8, 0x70)),
		("Uranus",  19.18916464, 0.04725744,  0.77263783,  74.01692503, 170.95427630, 313.23810451, Color.FromArgb(0x80, 0xE0, 0xE8)),
		("Neptune", 30.06992276, 0.00859048,  1.77004347, 131.78422574,  44.96476227, -55.12002969, Color.FromArgb(0x30, 0x50, 0xD0)),
	];

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="Orbit3DForm"/> class.</summary>
	/// <param name="planetoidName">Name or designation of the selected minor planet.</param>
	/// <param name="semiMajorAxis">Semi-major axis of the planetoid's orbit in AU.</param>
	/// <param name="eccentricity">Orbital eccentricity of the planetoid.</param>
	/// <param name="inclinationDeg">Orbital inclination in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">Longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">Argument of perihelion in degrees.</param>
	/// <param name="meanAnomalyDeg">Mean anomaly at the reference epoch in degrees.</param>
	/// <param name="epochMpcorb">MPCORB packed epoch string (e.g. "K254Q").</param>
	/// <remarks>All orbital elements are stored for use during rendering. The OpenGL context is
	/// created programmatically in <see cref="Orbit3DForm_Load"/> after the designer components
	/// have been initialized.</remarks>
	public Orbit3DForm(
		string planetoidName,
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg,
		double meanAnomalyDeg,
		string epochMpcorb)
	{
		InitializeComponent();
		_planetoidName = planetoidName;
		_semiMajorAxis = semiMajorAxis;
		_eccentricity = eccentricity;
		_inclinationDeg = inclinationDeg;
		_longitudeAscendingNodeDeg = longitudeAscendingNodeDeg;
		_argumentPerihelionDeg = argumentPerihelionDeg;
		_meanAnomalyDeg = meanAnomalyDeg;
		_epochMpcorb = epochMpcorb;
		logger.Info(
			message: "Orbit3DForm initialized for '{0}' (a={1} AU, e={2}, i={3}°, Ω={4}°, ω={5}°, M={6}°, epoch={7}).",
			args: [
				planetoidName,
				semiMajorAxis.ToString(format: "F6", provider: CultureInfo.InvariantCulture),
				eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture),
				inclinationDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture),
				longitudeAscendingNodeDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture),
				argumentPerihelionDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture),
				meanAnomalyDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture),
				epochMpcorb]);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Creates and configures the embedded <see cref="GLControl"/> and adds it to the GL panel.</summary>
	/// <remarks>The control is created with an OpenGL compatibility-profile context so that the
	/// immediate-mode GL functions (glBegin/glEnd) used for rendering are available.</remarks>
	private void CreateGlControl()
	{
		var settings = new GLControlSettings
		{
			API = ContextAPI.OpenGL,
			Profile = ContextProfile.Compatability,
			APIVersion = new Version(major: 2, minor: 1),
		};
		_glControl = new GLControl(settings)
		{
			Dock = DockStyle.Fill,
			AccessibleDescription = "OpenGL rendering surface for the 3D orbit diagram",
			AccessibleName = "3D orbit diagram",
			AccessibleRole = AccessibleRole.Client,
		};
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.MouseDown += GlControl_MouseDown;
		_glControl.MouseUp += GlControl_MouseUp;
		_glControl.MouseMove += GlControl_MouseMove;
		_glControl.MouseWheel += GlControl_MouseWheel;
		panelGl.Controls.Add(_glControl);
	}

	// ---- Orbital mechanics ----

	/// <summary>Solves Kepler's equation <c>M = E − e·sin(E)</c> for the eccentric anomaly <c>E</c>
	/// using Newton–Raphson iteration.</summary>
	/// <param name="meanAnomalyRad">Mean anomaly in radians.</param>
	/// <param name="eccentricity">Orbital eccentricity (0 ≤ e &lt; 1).</param>
	/// <returns>Eccentric anomaly in radians.</returns>
	private static double SolveKepler(double meanAnomalyRad, double eccentricity)
	{
		double e = eccentricity;
		double m = meanAnomalyRad;
		double bigE = m; // Initial guess
		for (int i = 0; i < 50; i++)
		{
			double deltaE = (m - bigE + e * Math.Sin(bigE)) / (1.0 - e * Math.Cos(bigE));
			bigE += deltaE;
			if (Math.Abs(deltaE) < 1e-12)
			{
				break;
			}
		}
		return bigE;
	}

	/// <summary>Computes the heliocentric ecliptic Cartesian coordinates (in AU) for a body with
	/// the given Keplerian orbital elements evaluated at the given mean anomaly.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="iDeg">Inclination in degrees.</param>
	/// <param name="omDeg">Longitude of the ascending node in degrees.</param>
	/// <param name="periDeg">Argument of perihelion in degrees.</param>
	/// <param name="mDeg">Mean anomaly in degrees.</param>
	/// <returns>A tuple of (x, y, z) ecliptic coordinates in AU, where the Z axis points toward the
	/// ecliptic north pole (positive = above ecliptic plane).</returns>
	private static (double X, double Y, double Z) OrbElemToEcliptic(
		double a, double e, double iDeg, double omDeg, double periDeg, double mDeg)
	{
		double mRad = mDeg * Math.PI / 180.0;
		double bigE = SolveKepler(meanAnomalyRad: mRad, eccentricity: e);

		// True anomaly ν via atan2
		double nu = 2.0 * Math.Atan2(
			y: Math.Sqrt(1.0 + e) * Math.Sin(bigE / 2.0),
			x: Math.Sqrt(1.0 - e) * Math.Cos(bigE / 2.0));

		// Heliocentric distance
		double r = a * (1.0 - e * Math.Cos(bigE));

		// Position in orbital plane (perifocal frame)
		double xOrbital = r * Math.Cos(nu);
		double yOrbital = r * Math.Sin(nu);

		// Pre-compute trig values for the three Euler angles
		double iRad = iDeg * Math.PI / 180.0;
		double omRad = omDeg * Math.PI / 180.0;
		double periRad = periDeg * Math.PI / 180.0;
		double cosOm = Math.Cos(omRad);
		double sinOm = Math.Sin(omRad);
		double cosI = Math.Cos(iRad);
		double sinI = Math.Sin(iRad);
		double cosPeri = Math.Cos(periRad);
		double sinPeri = Math.Sin(periRad);

		// Rotation matrix: R = Rz(Ω) · Rx(i) · Rz(ω)
		// Applied to (xOrbital, yOrbital, 0)
		double x = (cosOm * cosPeri - sinOm * sinPeri * cosI) * xOrbital
				 + (-cosOm * sinPeri - sinOm * cosPeri * cosI) * yOrbital;
		double y = (sinOm * cosPeri + cosOm * sinPeri * cosI) * xOrbital
				 + (-sinOm * sinPeri + cosOm * cosPeri * cosI) * yOrbital;
		double z = (sinPeri * sinI) * xOrbital
				 + (cosPeri * sinI) * yOrbital;

		return (x, y, z);
	}

	/// <summary>Computes an array of heliocentric ecliptic positions that trace one full orbit of a body.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="iDeg">Inclination in degrees.</param>
	/// <param name="omDeg">Longitude of the ascending node in degrees.</param>
	/// <param name="periDeg">Argument of perihelion in degrees.</param>
	/// <param name="steps">Number of equal-mean-anomaly steps (default <see cref="OrbitSteps"/>).</param>
	/// <returns>An array of <paramref name="steps"/>+1 ecliptic-coordinate tuples that close the orbit.</returns>
	private static (double X, double Y, double Z)[] ComputeOrbitPoints(
		double a, double e, double iDeg, double omDeg, double periDeg, int steps = OrbitSteps)
	{
		var pts = new (double X, double Y, double Z)[steps + 1];
		for (int k = 0; k <= steps; k++)
		{
			double m = k * 360.0 / steps;
			pts[k] = OrbElemToEcliptic(a: a, e: e, iDeg: iDeg, omDeg: omDeg, periDeg: periDeg, mDeg: m);
		}
		return pts;
	}

	/// <summary>Decodes a five-character MPCORB packed epoch string to a Julian Date.</summary>
	/// <param name="packed">Packed epoch string (e.g. "K254Q" = 2025-Apr-26).</param>
	/// <returns>The corresponding Julian Date, or <see cref="J2000Jd"/> if the string is invalid.</returns>
	/// <remarks>
	/// Format: [century-letter][2-digit-year][month-char][day-char].
	/// Century letters: I=1800s, J=1900s, K=2000s.
	/// Month chars: '1'–'9' for Jan–Sep, 'A'=Oct, 'B'=Nov, 'C'=Dec.
	/// Day chars: '1'–'9' for 1–9, 'A'–'V' for 10–31.
	/// </remarks>
	private static double MpcorbEpochToJd(string packed)
	{
		if (packed.Length < 5)
		{
			return J2000Jd;
		}
		int century = packed[0] switch
		{
			'I' => 1800,
			'J' => 1900,
			'K' => 2000,
			_ => 2000,
		};
		if (!int.TryParse(s: packed[1..3], result: out int yearOffset))
		{
			return J2000Jd;
		}
		int year = century + yearOffset;
		int month = packed[3] switch
		{
			>= '1' and <= '9' => packed[3] - '0',
			'A' => 10,
			'B' => 11,
			'C' => 12,
			_ => 1,
		};
		int day = packed[4] switch
		{
			>= '1' and <= '9' => packed[4] - '0',
			>= 'A' and <= 'Z' => packed[4] - 'A' + 10,
			_ => 1,
		};
		// Gregorian calendar → Julian Day Number (integer part) then add 0.5 for JD epoch
		int a = (14 - month) / 12;
		int y = year + 4800 - a;
		int m = month + 12 * a - 3;
		int jdn = day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
		return jdn - 0.5; // JD epoch is at noon; MPCORB epochs are at 0.0 TT
	}

	/// <summary>Computes the current mean anomaly (degrees) of a body given its mean anomaly at a reference epoch.</summary>
	/// <param name="m0Deg">Mean anomaly at the reference epoch in degrees.</param>
	/// <param name="semiMajorAxisAu">Semi-major axis in AU (used to compute mean motion via Kepler's third law).</param>
	/// <param name="epochJd">Julian Date of the reference epoch.</param>
	/// <param name="nowJd">Julian Date of the current time.</param>
	/// <returns>Current mean anomaly in degrees, normalized to [0°, 360°).</returns>
	private static double CurrentMeanAnomaly(double m0Deg, double semiMajorAxisAu, double epochJd, double nowJd)
	{
		// Kepler's 3rd law: n (°/day) = EarthMeanMotion / a^(3/2)
		double n = EarthMeanMotion / Math.Pow(x: semiMajorAxisAu, y: 1.5);
		double m = m0Deg + n * (nowJd - epochJd);
		return ((m % 360.0) + 360.0) % 360.0;
	}

	/// <summary>Converts a <see cref="DateTime"/> value to a Julian Date.</summary>
	/// <param name="dt">The date/time to convert (UTC recommended).</param>
	/// <returns>The Julian Date corresponding to the given date/time.</returns>
	private static double DateTimeToJd(DateTime dt)
	{
		// Meeus algorithm (Astronomical Algorithms, Ch. 7)
		int y = dt.Year;
		int mo = dt.Month;
		double d = dt.Day + (dt.Hour + (dt.Minute + dt.Second / 60.0) / 60.0) / 24.0;
		if (mo <= 2)
		{
			y--;
			mo += 12;
		}
		int a = y / 100;
		int b = 2 - a + a / 4; // Gregorian correction
		return (int)(365.25 * (y + 4716)) + (int)(30.6001 * (mo + 1)) + d + b - 1524.5;
	}

	// ---- OpenGL coordinate mapping ----

	/// <summary>Maps heliocentric ecliptic coordinates (X=vernal equinox, Y=90° east in ecliptic, Z=north pole)
	/// to OpenGL scene coordinates (X=right, Y=up=ecliptic north, Z=toward viewer).</summary>
	/// <param name="ex">Ecliptic X in AU.</param>
	/// <param name="ey">Ecliptic Y in AU.</param>
	/// <param name="ez">Ecliptic Z in AU (positive = above ecliptic plane).</param>
	/// <returns>OpenGL (glX, glY, glZ) floats.</returns>
	private static (float Gx, float Gy, float Gz) EclToGl(double ex, double ey, double ez)
		=> ((float)ex, (float)ez, (float)-ey);

	// ---- OpenGL rendering ----

	/// <summary>Sets up the OpenGL viewport and perspective projection matrix for the current control size.</summary>
	private void SetupProjection()
	{
		int w = _glControl.Width;
		int h = Math.Max(val1: _glControl.Height, val2: 1);
		GL.Viewport(x: 0, y: 0, width: w, height: h);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		// Perspective: fovY = 45°, near = 0.1 AU, far = 2 000 AU
		double aspect = (double)w / h;
		double fovY = 45.0 * Math.PI / 180.0;
		double f = 1.0 / Math.Tan(fovY / 2.0);
		// Column-major OpenGL perspective matrix
		double[] proj =
		[
			f / aspect, 0.0,   0.0,                           0.0,
			0.0,        f,     0.0,                           0.0,
			0.0,        0.0,  (2000.0 + 0.1) / (0.1 - 2000.0), -1.0,
			0.0,        0.0,   2.0 * 2000.0 * 0.1 / (0.1 - 2000.0), 0.0,
		];
		GL.LoadMatrix(ref proj[0]);
		GL.MatrixMode(mode: MatrixMode.Modelview);
	}

	/// <summary>Renders the full 3D scene to the OpenGL surface.</summary>
	/// <remarks>
	/// Draw order:
	/// <list type="number">
	/// <item>Ecliptic plane grid.</item>
	/// <item>Planet orbit lines.</item>
	/// <item>Planetoid orbit: orange above the ecliptic, semi-transparent violet below.</item>
	/// <item>Below-ecliptic shadow projected onto the ecliptic plane.</item>
	/// <item>Sun marker at the origin.</item>
	/// <item>Current planet positions.</item>
	/// <item>Current planetoid position.</item>
	/// </list>
	/// </remarks>
	private void RenderScene()
	{
		if (!_glReady)
		{
			return;
		}
		_glControl.MakeCurrent();
		GL.ClearColor(red: 0.04f, green: 0.04f, blue: 0.10f, alpha: 1f);
		GL.Clear(mask: ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		GL.Enable(cap: EnableCap.DepthTest);
		GL.Enable(cap: EnableCap.Blend);
		GL.BlendFunc(sfactor: BlendingFactor.SrcAlpha, dfactor: BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(cap: EnableCap.LineSmooth);
		GL.Hint(target: HintTarget.LineSmoothHint, mode: HintMode.Nicest);
		GL.Enable(cap: EnableCap.PointSmooth);
		SetupProjection();
		GL.LoadIdentity();
		// Apply camera transform: translate back then rotate
		GL.Translate(x: _panX, y: _panY, z: -_zoom);
		GL.Rotate(angle: _pitch, x: 1f, y: 0f, z: 0f);
		GL.Rotate(angle: _yaw, x: 0f, y: 1f, z: 0f);
		DrawEclipticGrid();
		DrawPlanetOrbits();
		DrawPlanetoidOrbit();
		DrawSun();
		double nowJd = DateTimeToJd(dt: DateTime.UtcNow);
		DrawPlanetCurrentPositions(nowJd: nowJd);
		DrawPlanetoidCurrentPosition(nowJd: nowJd);
		_glControl.SwapBuffers();
	}

	/// <summary>Draws a subtle square grid on the ecliptic plane (OpenGL Y = 0).</summary>
	private static void DrawEclipticGrid()
	{
		const int gridExtent = 40;
		const int gridStep = 5;
		GL.LineWidth(width: 1f);
		GL.Color4(red: 0.15f, green: 0.15f, blue: 0.25f, alpha: 0.8f);
		GL.Begin(mode: PrimitiveType.Lines);
		for (int x = -gridExtent; x <= gridExtent; x += gridStep)
		{
			GL.Vertex3(x: (float)x, y: 0f, z: (float)-gridExtent);
			GL.Vertex3(x: (float)x, y: 0f, z: (float)gridExtent);
		}
		for (int z = -gridExtent; z <= gridExtent; z += gridStep)
		{
			GL.Vertex3(x: (float)-gridExtent, y: 0f, z: (float)z);
			GL.Vertex3(x: (float)gridExtent, y: 0f, z: (float)z);
		}
		GL.End();
	}

	/// <summary>Draws the orbit ellipses of the eight solar system planets.</summary>
	private static void DrawPlanetOrbits()
	{
		GL.LineWidth(width: 1.5f);
		foreach ((string _, double a, double e, double i, double om, double peri, double _, Color col) in Planets)
		{
			var pts = ComputeOrbitPoints(a: a, e: e, iDeg: i, omDeg: om, periDeg: peri);
			// Dim the planet orbit colors slightly
			GL.Color3(red: col.R / 255f * 0.6f, green: col.G / 255f * 0.6f, blue: col.B / 255f * 0.6f);
			GL.Begin(mode: PrimitiveType.LineStrip);
			foreach ((double ex, double ey, double ez) in pts)
			{
				(float gx, float gy, float gz) = EclToGl(ex: ex, ey: ey, ez: ez);
				GL.Vertex3(x: gx, y: gy, z: gz);
			}
			GL.End();
		}
		GL.LineWidth(width: 1f);
	}

	/// <summary>Draws the planetoid orbit, coloring segments differently depending on whether they
	/// are above or below the ecliptic plane. Also draws a semi-transparent shadow on the ecliptic plane
	/// for all below-ecliptic segments.</summary>
	private void DrawPlanetoidOrbit()
	{
		var pts = ComputeOrbitPoints(
			a: _semiMajorAxis,
			e: _eccentricity,
			iDeg: _inclinationDeg,
			omDeg: _longitudeAscendingNodeDeg,
			periDeg: _argumentPerihelionDeg);

		// --- Pass 1: draw orbit line with per-segment color ---
		GL.LineWidth(width: 2.5f);
		GL.Begin(mode: PrimitiveType.Lines);
		for (int k = 0; k < pts.Length - 1; k++)
		{
			(double ex0, double ey0, double ez0) = pts[k];
			(double ex1, double ey1, double ez1) = pts[k + 1];
			// Below ecliptic ↔ ecliptic Z < 0 ↔ OpenGL Y < 0
			bool below0 = ez0 < 0.0;
			bool below1 = ez1 < 0.0;
			if (!below0 && !below1)
			{
				// Above ecliptic: bright orange
				GL.Color4(red: 1.0f, green: 0.60f, blue: 0.10f, alpha: 1.0f);
			}
			else
			{
				// Below ecliptic: semi-transparent violet
				GL.Color4(red: 0.70f, green: 0.30f, blue: 1.0f, alpha: 0.65f);
			}
			(float gx0, float gy0, float gz0) = EclToGl(ex: ex0, ey: ey0, ez: ez0);
			(float gx1, float gy1, float gz1) = EclToGl(ex: ex1, ey: ey1, ez: ez1);
			GL.Vertex3(x: gx0, y: gy0, z: gz0);
			GL.Vertex3(x: gx1, y: gy1, z: gz1);
		}
		GL.End();
		GL.LineWidth(width: 1f);

		// --- Pass 2: semi-transparent shadow on ecliptic plane for below-ecliptic segments ---
		// Render each below-ecliptic orbit segment as a quad connecting the orbit arc to the ecliptic plane.
		// Using GL_QUADS instead of TriangleStrip to avoid artefacts from skipped above-ecliptic vertices.
		GL.Disable(cap: EnableCap.DepthTest);
		GL.Color4(red: 0.55f, green: 0.20f, blue: 0.90f, alpha: 0.18f);
		GL.Begin(mode: PrimitiveType.Quads);
		for (int k = 0; k < pts.Length - 1; k++)
		{
			(double ex0, double ey0, double ez0) = pts[k];
			(double ex1, double ey1, double ez1) = pts[k + 1];
			// Only draw the shadow where at least one endpoint is below the ecliptic
			if (ez0 >= 0.0 && ez1 >= 0.0)
			{
				continue;
			}
			// Clamp the ecliptic-plane projection Y to 0 (shadow lies on the ecliptic)
			(float gx0, float gy0, float gz0) = EclToGl(ex: ex0, ey: ey0, ez: ez0);
			(float gx1, float gy1, float gz1) = EclToGl(ex: ex1, ey: ey1, ez: ez1);
			GL.Vertex3(x: gx0, y: 0f, z: gz0);   // ecliptic projection of pt0
			GL.Vertex3(x: gx1, y: 0f, z: gz1);   // ecliptic projection of pt1
			GL.Vertex3(x: gx1, y: gy1, z: gz1);  // actual orbit pt1
			GL.Vertex3(x: gx0, y: gy0, z: gz0);  // actual orbit pt0
		}
		GL.End();
		GL.Enable(cap: EnableCap.DepthTest);
	}

	/// <summary>Draws the Sun at the origin as a bright yellow point with a faint halo ring.</summary>
	private static void DrawSun()
	{
		// Bright central point
		GL.PointSize(size: 14f);
		GL.Color3(red: 1.0f, green: 1.0f, blue: 0.0f);
		GL.Begin(mode: PrimitiveType.Points);
		GL.Vertex3(x: 0f, y: 0f, z: 0f);
		GL.End();
		// Faint halo in the ecliptic plane
		GL.LineWidth(width: 1f);
		GL.Color4(red: 1.0f, green: 0.9f, blue: 0.4f, alpha: 0.35f);
		GL.Begin(mode: PrimitiveType.LineLoop);
		const double haloRadius = 0.28;
		for (int k = 0; k < 36; k++)
		{
			double angle = k * Math.PI / 18.0;
			GL.Vertex3(x: (float)(haloRadius * Math.Cos(angle)), y: 0f, z: (float)(haloRadius * Math.Sin(angle)));
		}
		GL.End();
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws the current position of each of the eight planets as a colored point marker.</summary>
	/// <param name="nowJd">Current Julian Date.</param>
	private static void DrawPlanetCurrentPositions(double nowJd)
	{
		GL.PointSize(size: 5f);
		foreach ((string _, double a, double e, double i, double om, double peri, double m0, Color col) in Planets)
		{
			double mNow = CurrentMeanAnomaly(m0Deg: m0, semiMajorAxisAu: a, epochJd: J2000Jd, nowJd: nowJd);
			(double ex, double ey, double ez) = OrbElemToEcliptic(a: a, e: e, iDeg: i, omDeg: om, periDeg: peri, mDeg: mNow);
			(float gx, float gy, float gz) = EclToGl(ex: ex, ey: ey, ez: ez);
			GL.Color3(red: col.R / 255f, green: col.G / 255f, blue: col.B / 255f);
			GL.Begin(mode: PrimitiveType.Points);
			GL.Vertex3(x: gx, y: gy, z: gz);
			GL.End();
		}
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws the current position of the selected planetoid as a larger orange-red point marker.</summary>
	/// <param name="nowJd">Current Julian Date.</param>
	private void DrawPlanetoidCurrentPosition(double nowJd)
	{
		double epochJd = MpcorbEpochToJd(packed: _epochMpcorb);
		double mNow = CurrentMeanAnomaly(
			m0Deg: _meanAnomalyDeg,
			semiMajorAxisAu: _semiMajorAxis,
			epochJd: epochJd,
			nowJd: nowJd);
		(double ex, double ey, double ez) = OrbElemToEcliptic(
			a: _semiMajorAxis,
			e: _eccentricity,
			iDeg: _inclinationDeg,
			omDeg: _longitudeAscendingNodeDeg,
			periDeg: _argumentPerihelionDeg,
			mDeg: mNow);
		(float gx, float gy, float gz) = EclToGl(ex: ex, ey: ey, ez: ez);
		GL.PointSize(size: 8f);
		GL.Color3(red: 1.0f, green: 0.35f, blue: 0.05f);
		GL.Begin(mode: PrimitiveType.Points);
		GL.Vertex3(x: gx, y: gy, z: gz);
		GL.End();
		GL.PointSize(size: 1f);
	}

	/// <summary>Updates the status bar label with the current orbital element summary.</summary>
	private void UpdateStatusLabel()
	{
		labelInformation.Text =
			$"3D: {_planetoidName} — " +
			$"a={_semiMajorAxis.ToString(format: "F4", provider: CultureInfo.InvariantCulture)} AU, " +
			$"e={_eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture)}, " +
			$"i={_inclinationDeg.ToString(format: "F2", provider: CultureInfo.InvariantCulture)}°, " +
			$"Ω={_longitudeAscendingNodeDeg.ToString(format: "F2", provider: CultureInfo.InvariantCulture)}°, " +
			$"ω={_argumentPerihelionDeg.ToString(format: "F2", provider: CultureInfo.InvariantCulture)}°, " +
			$"M={_meanAnomalyDeg.ToString(format: "F2", provider: CultureInfo.InvariantCulture)}° | " +
			$"Left-drag: rotate · Right-drag: pan · Scroll: zoom";
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the <see cref="Form.Load"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Creates the <see cref="GLControl"/>, initializes the OpenGL context, and triggers the first render.</remarks>
	private void Orbit3DForm_Load(object? sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		try
		{
			CreateGlControl();
			_glControl.MakeCurrent();
			GL.Enable(cap: EnableCap.DepthTest);
			_glReady = true;
			UpdateStatusLabel();
			_glControl.Invalidate();
		}
		catch (Exception ex)
		{
			logger.Error(message: "Orbit3DForm: failed to initialize OpenGL context for '{0}': {1}", args: [_planetoidName, ex]);
			ShowErrorMessage(message: $"Failed to initialize 3D rendering: {ex.Message}");
		}
	}

	#endregion

	#region GLControl event handlers

	/// <summary>Handles the <see cref="Control.Paint"/> event of the GL control to redraw the scene.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Paint event arguments.</param>
	private void GlControl_Paint(object? sender, PaintEventArgs e) => RenderScene();

	/// <summary>Handles the <see cref="Control.Resize"/> event of the GL control to update the viewport.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		if (!_glReady)
		{
			return;
		}
		_glControl.MakeCurrent();
		SetupProjection();
		_glControl.Invalidate();
	}

	/// <summary>Handles mouse-button-down events on the GL control to begin camera interaction.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	private void GlControl_MouseDown(object? sender, MouseEventArgs e)
	{
		_lastMousePos = e.Location;
		if (e.Button == MouseButtons.Left)
		{
			_leftDown = true;
		}
		if (e.Button == MouseButtons.Right)
		{
			_rightDown = true;
		}
	}

	/// <summary>Handles mouse-button-up events on the GL control to end camera interaction.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	private void GlControl_MouseUp(object? sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			_leftDown = false;
		}
		if (e.Button == MouseButtons.Right)
		{
			_rightDown = false;
		}
	}

	/// <summary>Handles mouse-move events on the GL control to rotate or pan the camera.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	/// <remarks>
	/// Left-drag rotates the scene (yaw and pitch).
	/// Right-drag pans the camera laterally.
	/// </remarks>
	private void GlControl_MouseMove(object? sender, MouseEventArgs e)
	{
		int dx = e.X - _lastMousePos.X;
		int dy = e.Y - _lastMousePos.Y;
		_lastMousePos = e.Location;
		if (_leftDown)
		{
			// Rotate: horizontal mouse → yaw, vertical mouse → pitch
			_yaw += dx * 0.5f;
			_pitch += dy * 0.5f;
			_pitch = Math.Clamp(value: _pitch, min: -89f, max: 89f);
			_glControl.Invalidate();
		}
		else if (_rightDown)
		{
			// Pan: scale pan by current zoom so panning feels consistent at all distances
			_panX += dx * _zoom * 0.001f;
			_panY -= dy * _zoom * 0.001f;
			_glControl.Invalidate();
		}
	}

	/// <summary>Handles scroll-wheel events on the GL control to zoom in or out.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	private void GlControl_MouseWheel(object? sender, MouseEventArgs e)
	{
		_zoom -= e.Delta * 0.02f;
		_zoom = Math.Clamp(value: _zoom, min: 0.5f, max: 200f);
		_glControl.Invalidate();
	}

	#endregion
}
