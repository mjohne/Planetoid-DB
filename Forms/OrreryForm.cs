/*
 * File:        OrreryForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Displays an animated orrery (planetary machine) of all planetoids and the eight solar system planets around the Sun.
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

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using BlendingFactor = OpenTK.Graphics.OpenGL.BlendingFactor;  // Explicit alias to resolve ambiguity
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;  // Explicit alias to resolve ambiguity
using GL = OpenTK.Graphics.OpenGL.GL;  // Explicit alias to resolve ambiguity
using HintMode = OpenTK.Graphics.OpenGL.HintMode;  // Explicit alias to resolve ambiguity
using HintTarget = OpenTK.Graphics.OpenGL.HintTarget;  // Explicit alias to resolve ambiguity
using MatrixMode = OpenTK.Graphics.OpenGL.MatrixMode;  // Explicit alias to resolve ambiguity
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;  // Explicit alias to resolve ambiguity

namespace Planetoid_DB;

/// <summary>Displays an animated orrery of all planetoids and the eight solar system planets around the Sun.</summary>
/// <remarks><para>The form renders the orbits of a selected range of planetoids from the MPCORB database together with all eight solar system planets and the Sun as 3D ellipses in the ecliptic coordinate frame using OpenTK/OpenGL.</para>
/// <para>A time-speed slider advances or reverses the simulation clock; the current position of each body is propagated from its Keplerian orbital elements. A date/time control shows and sets the simulated instant.</para>
/// <para>Interaction: left-drag to rotate the view, right-drag to pan, scroll wheel to zoom in/out. Hover over a body to see its name.</para></remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class OrreryForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used for logging informational messages and debugging output from the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>This property overrides the base class to return the specific <see cref="ToolStripStatusLabel"/> instance used in this form.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	// ---- Constants ----

	/// <summary>Number of orbit path segments computed per orbit (higher = smoother ellipse).</summary>
	/// <remarks>This constant defines the number of segments used to approximate the orbit paths when rendering.</remarks>
	private const int OrbitSteps = 360;

	/// <summary>Julian Date of the J2000.0 epoch (2000 January 1.5 TT).</summary>
	/// <remarks>This constant defines the Julian Date of the J2000.0 epoch, which is used as a reference for orbital calculations.</remarks>
	private const double J2000Jd = 2451545.0;

	/// <summary>Mean daily motion of the Earth in degrees per day (Kepler: 0.9856076686°/day).</summary>
	/// <remarks>This constant defines the mean daily motion of the Earth in degrees per day, used for orbital calculations.</remarks>
	private const double EarthMeanMotion = 0.9856076686;

	/// <summary>Solar gravitational parameter in AU³/day², derived from the JPL DE440 value of GM<sub>Sun</sub>.</summary>
	private const double SolarMuAu3PerDay2 = 0.0002959122082841195;

	/// <summary>Seconds per mean solar day.</summary>
	private const double SecondsPerDay = 86400.0;

	/// <summary>Approximate TT−UTC offset used for the interactive simulation.</summary>
	/// <remarks>The MPCORB epoch is specified in TT. The current TT−UTC offset is used here because future leap seconds are not predictable.</remarks>
	private const double TtMinusUtcSeconds = 69.184;

	/// <summary>Maximum numerical integration step for planetary perturbations.</summary>
	/// <remarks>The actual step is additionally limited to roughly 1/120 of the body's orbital period, with a tighter limit for inner orbits.</remarks>
	private const double MaxPerturbationStepDays = 4.0;

	/// <summary>Maximum number of integration steps accepted for a single propagation request before the request is rejected as pathological.</summary>
	private const int MaxPerturbationSteps = 200000;

	/// <summary>Planetary gravitational parameters in AU³/day², using the JPL DE440 system GM values.</summary>
	private static readonly double[] PlanetMuAu3PerDay2 =
	[
		4.9125001948001294e-11,  // Mercury
		7.2434523326441190e-10,  // Venus
		8.8876924467066020e-10,  // Earth-Moon system
		9.5495488297801950e-11,  // Mars system
		2.8253458252257923e-07,  // Jupiter system
		8.4597059933762900e-08,  // Saturn system
		1.2920265649682404e-08,  // Uranus system
		1.5243573478851052e-08,  // Neptune system
	];

	/// <summary>Maximum number of planetoids whose full orbit path is cached and drawn to keep rendering responsive.</summary>
	/// <remarks>When the selected range exceeds this count, only the current position markers are drawn (no orbit lines) to avoid overwhelming the renderer.</remarks>
	private const int MaxOrbitLines = 2000;

	/// <summary>Maximum number of planetoid markers rendered and considered for hover picking.</summary>
	/// <remarks>This separate cap keeps per-frame position updates and hover detection responsive for very large catalog selections.</remarks>
	private const int MaxRenderedPlanetoids = 2000;

	// ---- Orbital element source ----

	/// <summary>MPCORB column offsets and lengths used for fixed-width parsing.</summary>
	/// <remarks>These values match the offsets used elsewhere in the application for reading the MPCORB.DAT records.</remarks>
	private const int EpochStart = 20, EpochLen = 5;
	private const int MeanAnomalyStart = 26, MeanAnomalyLen = 9;
	private const int ArgPerihelionStart = 37, ArgPerihelionLen = 9;
	private const int LongAscNodeStart = 48, LongAscNodeLen = 9;
	private const int InclinationStart = 59, InclinationLen = 9;
	private const int EccentricityStart = 70, EccentricityLen = 9;
	private const int MeanMotionStart = 80, MeanMotionLen = 11;
	private const int SemiMajorAxisStart = 92, SemiMajorAxisLen = 11;
	private const int DesignationStart = 166, DesignationLen = 28;

	/// <summary>Planet orbital elements at J2000.0 from the Astronomical Almanac / NASA Horizons. Fields: Name, SemiMajorAxis (AU), Eccentricity, Inclination (°), LongAscNode (°), ArgPeri (°), MeanAnomaly0 (°), OpenGL color.</summary>
	/// <remarks>The argument of perihelion and mean anomaly values are derived from the standard longitude-of-perihelion and mean-longitude elements for J2000.0.</remarks>
	private static readonly (string Name, double A, double E, double I, double Om, double Peri, double M0, Color Col)[] Planets =
	[
		(Name: "Mercury",  0.38709927, 0.20563593,  7.00497902,  48.33076593,  29.12703035, 174.79252722, Color.FromArgb(red: 0xC0, green: 0xC0, blue: 0xC8)),
		(Name: "Venus",    0.72333566, 0.00677672,  3.39467605,  76.67984255,  54.92262463,  50.37663232, Color.FromArgb(red: 0xE8, green: 0xD0, blue: 0x90)),
		(Name: "Earth",    1.00000261, 0.01671123,  0.00001531,   0.0,        102.93768193,  -2.47311027, Color.FromArgb(red: 0x40, green: 0x90, blue: 0xFF)),
		(Name: "Mars",     1.52371034, 0.09339410,  1.84969142,  49.55953891, -73.50316850,  19.39019754, Color.FromArgb(red: 0xE0, green: 0x60, blue: 0x30)),
		(Name: "Jupiter",  5.20288700, 0.04838624,  1.30439695, 100.47390909, -85.74542926,  19.66796068, Color.FromArgb(red: 0xE8, green: 0xC0, blue: 0x88)),
		(Name: "Saturn",   9.53667594, 0.05386179,  2.48599187, 113.66242448, -21.06354617, -42.64463408, Color.FromArgb(red: 0xD8, green: 0xC8, blue: 0x70)),
		(Name: "Uranus",  19.18916464, 0.04725744,  0.77263783,  74.01692503,  96.93735127, 142.28382821, Color.FromArgb(red: 0x80, green: 0xE0, blue: 0xE8)),
		(Name: "Neptune", 30.06992276, 0.00859048,  1.77004347, 131.78422574, -86.81946347, -100.08479196, Color.FromArgb(red: 0x30, green: 0x50, blue: 0xD0)),
	];

	/// <summary>The raw MPCORB record lines supplied to the form.</summary>
	/// <remarks>These lines are parsed on load into <see cref="_planetoids"/>.</remarks>
	private readonly IReadOnlyList<string> _sourceLines;

	/// <summary>The parsed planetoid orbital elements for the currently selected index range.</summary>
	/// <remarks>This list is rebuilt whenever the start or end index changes.</remarks>
	private readonly List<PlanetoidElements> _planetoids = [];

	/// <summary>Cached orbit point arrays for each of the eight solar system planets, computed once on load.</summary>
	/// <remarks>Each entry corresponds to the planet at the same index in <see cref="Planets"/>.</remarks>
	private (double X, double Y, double Z)[][]? _cachedPlanetOrbits;

	/// <summary>Cached orbit point arrays for the currently selected planetoids.</summary>
	/// <remarks>Only populated when the selected count does not exceed <see cref="MaxOrbitLines"/>.</remarks>
	private (double X, double Y, double Z)[][]? _cachedPlanetoidOrbits;

	/// <summary>Cached current-position markers for the eight planets at the active simulation time.</summary>
	private RenderedBody[] _cachedPlanetBodies = [];

	/// <summary>Cached current-position markers for the rendered subset of planetoids at the active simulation time.</summary>
	private RenderedBody[] _cachedPlanetoidBodies = [];

	/// <summary>Numerically propagated heliocentric states for the currently rendered planetoids.</summary>	
	private StateVector[] _propagatedPlanetoidStates = [];

	/// <summary>Julian Date (TT) corresponding to <see cref="_propagatedPlanetoidStates"/>.</summary>
	private double _propagatedPlanetoidStatesJdTt = double.NaN;

	/// <summary>Julian Date corresponding to the currently cached marker positions.</summary>
	private double _cachedBodyPositionsJd = double.NaN;

	// ---- Simulation state ----

	/// <summary>The current simulated instant of the orrery (UTC).</summary>
	/// <remarks>This value is advanced by the animation timer according to the time-speed slider and can be set directly with the date/time control.</remarks>
	private DateTime _simulationTime = DateTime.UtcNow;

	/// <summary>Simulated days advanced per animation tick per slider unit.</summary>
	/// <remarks>The effective step is this value multiplied by the current slider value each timer tick.</remarks>
	private const double DaysPerTickPerUnit = 0.5;

	/// <summary>The animation timer that advances the simulation clock.</summary>
	/// <remarks>Ticks roughly 30 times per second while the animation is playing.</remarks>
	private readonly System.Windows.Forms.Timer _animationTimer;

	/// <summary>Whether the animation is currently playing.</summary>
	/// <remarks>Toggled by the play/pause button.</remarks>
	private bool _isPlaying;

	/// <summary>Guard flag to suppress feedback loops when programmatically updating the date/time control.</summary>
	/// <remarks>Set while the simulation clock updates the control so the control's ValueChanged handler does not re-apply the value.</remarks>
	private bool _suppressDateTimeEvent;

	/// <summary>Guard flag preventing range rebuilds before the form has finished loading.</summary>
	/// <remarks>Set to true once the initial load completes so spinner ValueChanged events rebuild the range.</remarks>
	private bool _initialized;

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

	/// <summary>Whether the initial range selection has finished loading and can be rendered.</summary>
	private bool _rangeSelectionReady;

	/// <summary>Monotonic version used to discard outdated background range rebuild results.</summary>
	private int _rangeBuildVersion;

	/// <summary>The embedded OpenTK GLControl that provides the OpenGL rendering surface.</summary>
	/// <remarks>Its lifetime is owned by <c>panelGl.Controls</c>, which disposes child controls when the form is disposed.</remarks>
	[SuppressMessage(category: "Usage", checkId: "CA2213:Disposable fields should be disposed", Justification = "The GLControl is owned and disposed by panelGl.Controls.")]
	private GLControl _glControl = null!;

	/// <summary>Font used to draw the hover tooltip overlay text.</summary>
	/// <remarks>This font is disposed when the form is disposed.</remarks>
	private readonly Font _overlayFont = new(familyName: "Segoe UI", emSize: 9f, style: FontStyle.Bold);

	/// <summary>The name of the body currently under the mouse cursor, or <see langword="null"/> when none.</summary>
	/// <remarks>Displayed as an overlay tooltip near the cursor.</remarks>
	private string? _hoverName;

	/// <summary>The last known mouse cursor position within the GL control, used to place the hover tooltip.</summary>
	/// <remarks>Updated on every mouse-move event.</remarks>
	private Point _hoverPoint;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="OrreryForm"/> class.</summary>
	/// <param name="planetoids">The raw MPCORB record lines to visualize.</param>
	/// <remarks>The orbital elements are parsed and the OpenGL context is created in <see cref="OrreryForm_Load"/> after the designer components have been initialized.</remarks>
	public OrreryForm(IReadOnlyList<string> planetoids)
	{
		_sourceLines = planetoids ?? [];
		InitializeComponent();
		using System.Windows.Forms.Timer _ = _animationTimer = new System.Windows.Forms.Timer(container: components!) { Interval = 33 };
		_animationTimer.Tick += AnimationTimer_Tick;
		logger.Info(message: "OrreryForm initialized with {0} source planetoid records.", args: _sourceLines.Count);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used by the debugger to display the state of the <see cref="OrreryForm"/> instance in a concise format.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Creates and configures the embedded <see cref="GLControl"/> and adds it to the GL panel.</summary>
	/// <remarks>The control is created with an OpenGL compatibility-profile context so that the immediate-mode GL functions used for rendering are available.</remarks>
	private void CreateGlControl()
	{
		GLControlSettings settings = new()
		{
			API = ContextAPI.OpenGL,
			Profile = ContextProfile.Any,
			APIVersion = new Version(major: 2, minor: 1),
		};
		using GLControl _ = _glControl = new GLControl(glControlSettings: settings)
		{
			Dock = DockStyle.Fill,
			AccessibleDescription = "OpenGL rendering surface for the orrery",
			AccessibleName = "Orrery rendering surface",
			AccessibleRole = AccessibleRole.Client,
		};
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.MouseDown += GlControl_MouseDown;
		_glControl.MouseUp += GlControl_MouseUp;
		_glControl.MouseMove += GlControl_MouseMove;
		_glControl.MouseWheel += GlControl_MouseWheel;
		_glControl.MouseLeave += GlControl_MouseLeave;
		_glControl.Enter += Control_Enter;
		_glControl.Leave += Control_Leave;
		_glControl.MouseEnter += Control_Enter;
		panelGl.Controls.Add(value: _glControl);
	}

	// ---- Parsing ----

	/// <summary>Attempts to parse a fixed-width double value from an MPCORB record line.</summary>
	/// <param name="line">The record line.</param>
	/// <param name="start">The zero-based start column.</param>
	/// <param name="len">The field length in characters.</param>
	/// <param name="value">When this method returns, contains the parsed value, or the default value on failure.</param>
	/// <returns><see langword="true"/> if the value was parsed successfully; otherwise <see langword="false"/>.</returns>
	private static bool TryParseValue(string line, int start, int len, out double value)
	{
		value = default;
		return line.Length >= start + len && double.TryParse(s: line.Substring(startIndex: start, length: len).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
	}

	/// <summary>Extracts the readable designation of a planetoid from an MPCORB record line.</summary>
	/// <param name="line">The record line.</param>
	/// <returns>The trimmed readable designation, or the packed number field when the designation column is absent.</returns>
	private static string ParseDesignation(string line)
	{
		if (line.Length >= DesignationStart + DesignationLen)
		{
			string readable = line.Substring(startIndex: DesignationStart, length: DesignationLen).Trim();
			if (readable.Length > 0)
			{
				return readable;
			}
		}
		return line.Length >= 7 ? line[..7].Trim() : line.Trim();
	}

	/// <summary>Attempts to parse a single MPCORB record line into a <see cref="PlanetoidElements"/> value.</summary>
	/// <param name="line">The record line.</param>
	/// <param name="elements">When this method returns, contains the parsed orbital elements on success.</param>
	/// <returns><see langword="true"/> if the line describes a valid bound elliptical orbit; otherwise <see langword="false"/>.</returns>
	private static bool TryParsePlanetoid(string line, out PlanetoidElements elements)
	{
		elements = default;
		if (!TryParseValue(line: line, start: SemiMajorAxisStart, len: SemiMajorAxisLen, value: out double a)
			|| !TryParseValue(line: line, start: EccentricityStart, len: EccentricityLen, value: out double e)
			|| !TryParseValue(line: line, start: InclinationStart, len: InclinationLen, value: out double i)
			|| !TryParseValue(line: line, start: LongAscNodeStart, len: LongAscNodeLen, value: out double om)
			|| !TryParseValue(line: line, start: ArgPerihelionStart, len: ArgPerihelionLen, value: out double peri)
			|| !TryParseValue(line: line, start: MeanAnomalyStart, len: MeanAnomalyLen, value: out double m0))
		{
			return false;
		}
		if (!double.IsFinite(d: a) || !double.IsFinite(d: e) || a <= 0.0 || e < 0.0 || e >= 1.0)
		{
			return false;
		}
		double? meanMotion = TryParseValue(line: line, start: MeanMotionStart, len: MeanMotionLen, value: out double parsedMeanMotion)
			&& double.IsFinite(d: parsedMeanMotion)
			? parsedMeanMotion
			: null;
		string epochPacked = line.Length >= EpochStart + EpochLen ? line.Substring(startIndex: EpochStart, length: EpochLen).Trim() : string.Empty;
		if (!TryMpcorbEpochToJd(packed: epochPacked, julianDate: out double epochJd))
		{
			return false;
		}
		elements = new PlanetoidElements(Name: ParseDesignation(line: line), A: a, E: e, I: i, Om: om, Peri: peri, M0: m0, MeanMotion: meanMotion, EpochJd: epochJd);
		return true;
	}

	/// <summary>Rebuilds <see cref="_planetoids"/> and their cached orbit paths for the currently selected index range.</summary>
	/// <remarks>The start and end index values are one-based and clamped to the available record count. Parsing runs on a background thread so large MPCORB selections do not block the UI thread.</remarks>
	private async Task RebuildRangeAsync()
	{
		int total = _sourceLines.Count;
		if (total == 0)
		{
			_planetoids.Clear();
			_cachedPlanetoidOrbits = null;
			InvalidateBodyCache();
			ClearHoverTarget(redraw: false);
			_rangeSelectionReady = true;
			UpdateStatusLabel();
			return;
		}
		int start = (int)Math.Clamp(value: numericStartIndex.Value, min: 1, max: total);
		int end = (int)Math.Clamp(value: numericEndIndex.Value, min: 1, max: total);
		if (end < start)
		{
			(start, end) = (end, start);
		}
		int rangeBuildVersion = ++_rangeBuildVersion;
		if (!_initialized)
		{
			_rangeSelectionReady = false;
		}
		SetRangeControlsEnabled(isEnabled: false);
		SetStatusBar(label: labelInformation, text: $"Loading planetoids {start:N0}-{end:N0}…");
		try
		{
			RangeBuildResult result = await Task.Run(function: () => BuildRange(start: start, end: end)).ConfigureAwait(continueOnCapturedContext: true);
			if (rangeBuildVersion != _rangeBuildVersion || IsDisposed)
			{
				return;
			}
			_planetoids.Clear();
			_planetoids.AddRange(collection: result.Planetoids);
			_cachedPlanetoidOrbits = result.CachedPlanetoidOrbits;
			_propagatedPlanetoidStates = [];
			_propagatedPlanetoidStatesJdTt = double.NaN;
			InvalidateBodyCache();
			ClearHoverTarget(redraw: false);
			_rangeSelectionReady = true;
			UpdateStatusLabel();
			if (_glReady)
			{
				_glControl.Invalidate();
			}
		}
		finally
		{
			if (rangeBuildVersion == _rangeBuildVersion && !IsDisposed)
			{
				SetRangeControlsEnabled(isEnabled: true);
			}
		}
	}

	/// <summary>Builds the selected planetoid range and any associated orbit caches.</summary>
	/// <param name="start">One-based start index of the selected range.</param>
	/// <param name="end">One-based end index of the selected range.</param>
	/// <returns>The parsed planetoids and any orbit caches for the selected range.</returns>
	private RangeBuildResult BuildRange(int start, int end)
	{
		List<PlanetoidElements> planetoids = new(capacity: end - start + 1);
		for (int idx = start - 1; idx < end; idx++)
		{
			if (TryParsePlanetoid(line: _sourceLines[index: idx], out PlanetoidElements elements))
			{
				planetoids.Add(item: elements);
			}
		}
		(double X, double Y, double Z)[][]? cachedPlanetoidOrbits = null;
		if (planetoids.Count <= MaxOrbitLines)
		{
			cachedPlanetoidOrbits = new (double X, double Y, double Z)[planetoids.Count][];
			for (int idx = 0; idx < planetoids.Count; idx++)
			{
				PlanetoidElements p = planetoids[index: idx];
				cachedPlanetoidOrbits[idx] = ComputeOrbitPoints(a: p.A, e: p.E, iDeg: p.I, omDeg: p.Om, periDeg: p.Peri);
			}
		}
		return new RangeBuildResult(Planetoids: [.. planetoids], CachedPlanetoidOrbits: cachedPlanetoidOrbits);
	}

	/// <summary>Enables or disables the range spinner controls while a background rebuild is active.</summary>
	/// <param name="isEnabled"><see langword="true"/> to enable the controls; otherwise <see langword="false"/>.</param>
	private void SetRangeControlsEnabled(bool isEnabled)
	{
		numericStartIndex.Enabled = isEnabled;
		numericEndIndex.Enabled = isEnabled;
	}

	// ---- Orbital mechanics ----

	/// <summary>Solves Kepler's equation <c>M = E − e·sin(E)</c> for the eccentric anomaly <c>E</c> using a safeguarded Newton iteration.</summary>
	/// <param name="meanAnomalyRad">Mean anomaly in radians.</param>
	/// <param name="eccentricity">Orbital eccentricity (0 ≤ e &lt; 1).</param>
	/// <returns>Eccentric anomaly in radians.</returns>
	private static double SolveKepler(double meanAnomalyRad, double eccentricity)
	{
		double ecc = eccentricity;
		if (!double.IsFinite(d: meanAnomalyRad) || !double.IsFinite(d: ecc) || ecc < 0.0 || ecc >= 1.0)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(eccentricity));
		}

		// Reduce M before evaluating sin/cos. This prevents loss of accuracy when this helper is reused with an unreduced mean anomaly.
		double twoPi = 2.0 * Math.PI;
		double m = ((meanAnomalyRad % twoPi) + twoPi) % twoPi;
		if (m > Math.PI)
		{
			m -= twoPi;
		}

		// M is already a good initial guess for normal asteroid eccentricities. For highly eccentric orbits, π·sign(M) is safer.
		double bigE = ecc < 0.8
			? m
			: Math.CopySign(Math.PI, m == 0.0 ? 1.0 : m);

		for (int iteration = 0; iteration < 50; iteration++)
		{
			double sinE = Math.Sin(a: bigE);
			double cosE = Math.Cos(d: bigE);
			double f = bigE - (ecc * sinE) - m;
			double fp = 1.0 - (ecc * cosE);
			double deltaE = f / fp;
			double nextE = bigE - deltaE;

			if (Math.Abs(value: deltaE) < 1e-13)
			{
				return nextE;
			}

			bigE = nextE;
		}

		return bigE;
	}

	/// <summary>Computes the heliocentric ecliptic Cartesian coordinates (in AU) for a body with the given Keplerian orbital elements evaluated at the given mean anomaly.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="iDeg">Inclination in degrees.</param>
	/// <param name="omDeg">Longitude of the ascending node in degrees.</param>
	/// <param name="periDeg">Argument of perihelion in degrees.</param>
	/// <param name="mDeg">Mean anomaly in degrees.</param>
	/// <returns>A tuple of (x, y, z) ecliptic coordinates in AU, where the Z axis points toward the ecliptic north pole.</returns>
	private static (double X, double Y, double Z) OrbElemToEcliptic(double a, double e, double iDeg, double omDeg, double periDeg, double mDeg)
	{
		double ecc = e;
		double mRad = mDeg * Math.PI / 180.0;
		double bigE = SolveKepler(meanAnomalyRad: mRad, eccentricity: ecc);

		// Direct eccentric-anomaly coordinates avoid the extra true-anomaly conversion and are numerically cleaner near perihelion.
		double cosE = Math.Cos(d: bigE);
		double sinE = Math.Sin(a: bigE);
		double sqrtOneMinusE2 = Math.Sqrt(d: 1.0 - (ecc * ecc));
		double xOrbital = a * (cosE - ecc);
		double yOrbital = a * sqrtOneMinusE2 * sinE;

		double iRad = iDeg * Math.PI / 180.0;
		double omRad = omDeg * Math.PI / 180.0;
		double periRad = periDeg * Math.PI / 180.0;
		double cosOm = Math.Cos(d: omRad);
		double sinOm = Math.Sin(a: omRad);
		double cosI = Math.Cos(d: iRad);
		double sinI = Math.Sin(a: iRad);
		double cosPeri = Math.Cos(d: periRad);
		double sinPeri = Math.Sin(a: periRad);
		double x = (((cosOm * cosPeri) - (sinOm * sinPeri * cosI)) * xOrbital)
				 + (((-cosOm * sinPeri) - (sinOm * cosPeri * cosI)) * yOrbital);
		double y = (((sinOm * cosPeri) + (cosOm * sinPeri * cosI)) * xOrbital)
				 + (((-sinOm * sinPeri) + (cosOm * cosPeri * cosI)) * yOrbital);
		double z = (sinPeri * sinI * xOrbital)
				 + (cosPeri * sinI * yOrbital);
		return (X: x, Y: y, Z: z);
	}

	/// <summary>Converts Keplerian elements at an epoch into a heliocentric Cartesian state vector.</summary>
	private static StateVector OrbElemToStateVector(
		double a, double e, double iDeg, double omDeg, double periDeg, double mDeg)
	{
		double ecc = e;
		double mRad = mDeg * Math.PI / 180.0;
		double bigE = SolveKepler(meanAnomalyRad: mRad, eccentricity: ecc);
		double cosE = Math.Cos(d: bigE);
		double sinE = Math.Sin(a: bigE);
		double sqrtOneMinusE2 = Math.Sqrt(d: 1.0 - (ecc * ecc));
		double r = a * (1.0 - (ecc * cosE));

		// Perifocal position and velocity for the two-body problem.
		double xOrbital = a * (cosE - ecc);
		double yOrbital = a * sqrtOneMinusE2 * sinE;
		double velocityFactor = Math.Sqrt(d: SolarMuAu3PerDay2 * a) / r;
		double vxOrbital = -velocityFactor * sinE;
		double vyOrbital = velocityFactor * sqrtOneMinusE2 * cosE;

		double iRad = iDeg * Math.PI / 180.0;
		double omRad = omDeg * Math.PI / 180.0;
		double periRad = periDeg * Math.PI / 180.0;
		double cosOm = Math.Cos(d: omRad);
		double sinOm = Math.Sin(a: omRad);
		double cosI = Math.Cos(d: iRad);
		double sinI = Math.Sin(a: iRad);
		double cosPeri = Math.Cos(d: periRad);
		double sinPeri = Math.Sin(a: periRad);

		double r11 = (cosOm * cosPeri) - (sinOm * sinPeri * cosI);
		double r12 = (-cosOm * sinPeri) - (sinOm * cosPeri * cosI);
		double r21 = (sinOm * cosPeri) + (cosOm * sinPeri * cosI);
		double r22 = (-sinOm * sinPeri) + (cosOm * cosPeri * cosI);
		double r31 = sinPeri * sinI;
		double r32 = cosPeri * sinI;

		return new StateVector(
			X: (r11 * xOrbital) + (r12 * yOrbital),
			Y: (r21 * xOrbital) + (r22 * yOrbital),
			Z: (r31 * xOrbital) + (r32 * yOrbital),
			Vx: (r11 * vxOrbital) + (r12 * vyOrbital),
			Vy: (r21 * vxOrbital) + (r22 * vyOrbital),
			Vz: (r31 * vxOrbital) + (r32 * vyOrbital));
	}

	/// <summary>Returns a planet's analytic heliocentric state vector at a given Julian date.</summary>
	private static StateVector PlanetPositionAtJd(int planetIndex, double jdTt)
	{
		(string Name, double A, double E, double I, double Om, double Peri, double M0, Color Col) planet = Planets[planetIndex];
		double mNow = CurrentMeanAnomaly(m0Deg: planet.M0, semiMajorAxisAu: planet.A, epochJd: J2000Jd, nowJd: jdTt);
		return OrbElemToStateVector(a: planet.A, e: planet.E, iDeg: planet.I, omDeg: planet.Om, periDeg: planet.Peri, mDeg: mNow);
	}

	/// <summary>Computes heliocentric gravitational acceleration including the indirect term from all eight planets.</summary>
	private static (double Ax, double Ay, double Az) ComputePerturbedAcceleration(StateVector state, double jdTt)
	{
		double r2 = (state.X * state.X) + (state.Y * state.Y) + (state.Z * state.Z);
		double r = Math.Sqrt(d: r2);
		double invR3 = 1.0 / (r2 * r);
		double ax = -SolarMuAu3PerDay2 * state.X * invR3;
		double ay = -SolarMuAu3PerDay2 * state.Y * invR3;
		double az = -SolarMuAu3PerDay2 * state.Z * invR3;

		for (int idx = 0; idx < Planets.Length; idx++)
		{
			StateVector planet = PlanetPositionAtJd(planetIndex: idx, jdTt: jdTt);
			double dx = planet.X - state.X;
			double dy = planet.Y - state.Y;
			double dz = planet.Z - state.Z;
			double delta2 = (dx * dx) + (dy * dy) + (dz * dz);
			double delta = Math.Sqrt(d: delta2);
			if (delta < 1e-7)
			{
				// Point-mass dynamics becomes singular at a collision. This guard prevents a numerical blow-up in the UI.
				delta = 1e-7;
				delta2 = delta * delta;
			}
			double invDelta3 = 1.0 / (delta2 * delta);
			double planetR2 = (planet.X * planet.X) + (planet.Y * planet.Y) + (planet.Z * planet.Z);
			double planetR = Math.Sqrt(d: planetR2);
			double invPlanetR3 = 1.0 / (planetR2 * planetR);
			double mu = PlanetMuAu3PerDay2[idx];
			ax += mu * ((dx * invDelta3) - (planet.X * invPlanetR3));
			ay += mu * ((dy * invDelta3) - (planet.Y * invPlanetR3));
			az += mu * ((dz * invDelta3) - (planet.Z * invPlanetR3));
		}

		return (Ax: ax, Ay: ay, Az: az);
	}

	/// <summary>Performs one fourth-order Runge-Kutta integration step.</summary>
	private static StateVector RungeKutta4Step(StateVector state, double jdTt, double h)
	{
		(double ax1, double ay1, double az1) = ComputePerturbedAcceleration(state: state, jdTt: jdTt);
		StateVector s2 = new(X: state.X + (0.5 * h * state.Vx), Y: state.Y + (0.5 * h * state.Vy), Z: state.Z + (0.5 * h * state.Vz),
			Vx: state.Vx + (0.5 * h * ax1), Vy: state.Vy + (0.5 * h * ay1), Vz: state.Vz + (0.5 * h * az1));
		(double ax2, double ay2, double az2) = ComputePerturbedAcceleration(state: s2, jdTt: jdTt + (0.5 * h));
		StateVector s3 = new(X: state.X + (0.5 * h * s2.Vx), Y: state.Y + (0.5 * h * s2.Vy), Z: state.Z + (0.5 * h * s2.Vz),
			Vx: state.Vx + (0.5 * h * ax2), Vy: state.Vy + (0.5 * h * ay2), Vz: state.Vz + (0.5 * h * az2));
		(double ax3, double ay3, double az3) = ComputePerturbedAcceleration(state: s3, jdTt: jdTt + (0.5 * h));
		StateVector s4 = new(X: state.X + (h * s3.Vx), Y: state.Y + (h * s3.Vy), Z: state.Z + (h * s3.Vz),
			state.Vx + (h * ax3), state.Vy + (h * ay3), state.Vz + (h * az3));
		(double ax4, double ay4, double az4) = ComputePerturbedAcceleration(state: s4, jdTt: jdTt + h);

		return new StateVector(
			X: state.X + (h / 6.0 * (state.Vx + (2.0 * s2.Vx) + (2.0 * s3.Vx) + s4.Vx)),
			Y: state.Y + (h / 6.0 * (state.Vy + (2.0 * s2.Vy) + (2.0 * s3.Vy) + s4.Vy)),
			Z: state.Z + (h / 6.0 * (state.Vz + (2.0 * s2.Vz) + (2.0 * s3.Vz) + s4.Vz)),
			Vx: state.Vx + (h / 6.0 * (ax1 + (2.0 * ax2) + (2.0 * ax3) + ax4)),
			Vy: state.Vy + (h / 6.0 * (ay1 + (2.0 * ay2) + (2.0 * ay3) + ay4)),
			Vz: state.Vz + (h / 6.0 * (az1 + (2.0 * az2) + (2.0 * az3) + az4)));
	}

	/// <summary>Propagates a planetoid state vector through the Sun plus eight-planet gravitational model.</summary>
	private static StateVector PropagatePerturbedState(StateVector initialState, double startJdTt, double endJdTt, double orbitalPeriodDays)
	{
		double totalDays = endJdTt - startJdTt;
		if (Math.Abs(value: totalDays) < 1e-12)
		{
			return initialState;
		}

		double stepDays = Math.Clamp(value: orbitalPeriodDays / 120.0, min: 0.05, max: MaxPerturbationStepDays);
		int stepCount = (int)Math.Ceiling(a: Math.Abs(value: totalDays) / stepDays);
		if (stepCount > MaxPerturbationSteps)
		{
			stepCount = MaxPerturbationSteps;
		}
		double h = totalDays / stepCount;
		StateVector state = initialState;
		double jd = startJdTt;
		for (int step = 0; step < stepCount; step++)
		{
			state = RungeKutta4Step(state: state, jdTt: jd, h: h);
			jd += h;
		}
		return state;
	}

	/// <summary>Computes an array of heliocentric ecliptic positions that trace one full orbit of a body.</summary>
	/// <param name="a">Semi-major axis in AU.</param>
	/// <param name="e">Eccentricity.</param>
	/// <param name="iDeg">Inclination in degrees.</param>
	/// <param name="omDeg">Longitude of the ascending node in degrees.</param>
	/// <param name="periDeg">Argument of perihelion in degrees.</param>
	/// <param name="steps">Number of equal-mean-anomaly steps (default <see cref="OrbitSteps"/>).</param>
	/// <returns>An array of <paramref name="steps"/>+1 ecliptic-coordinate tuples that close the orbit.</returns>
	private static (double X, double Y, double Z)[] ComputeOrbitPoints(double a, double e, double iDeg, double omDeg, double periDeg, int steps = OrbitSteps)
	{
		(double X, double Y, double Z)[] pts = new (double X, double Y, double Z)[steps + 1];
		for (int k = 0; k <= steps; k++)
		{
			double m = k * 360.0 / steps;
			pts[k] = OrbElemToEcliptic(a: a, e: e, iDeg: iDeg, omDeg: omDeg, periDeg: periDeg, mDeg: m);
		}
		return pts;
	}

	/// <summary>Attempts to decode a five-character MPCORB packed epoch string to a Julian Date.</summary>
	/// <param name="packed">Packed epoch string (e.g. "K254Q" = 2025-Apr-26).</param>
	/// <param name="julianDate">When this method returns, contains the corresponding Julian Date when parsing succeeds.</param>
	/// <returns><see langword="true"/> if the packed epoch was valid; otherwise <see langword="false"/>.</returns>
	private static bool TryMpcorbEpochToJd(string packed, out double julianDate)
	{
		julianDate = default;
		if (packed.Length != 5)
		{
			return false;
		}
		int century = packed[index: 0] switch
		{
			'I' => 1800,
			'J' => 1900,
			'K' => 2000,
			_ => 0,
		};
		if (century == 0 || !int.TryParse(s: packed[1..3], result: out int yearOffset))
		{
			return false;
		}
		int year = century + yearOffset;
		int month = packed[index: 3] switch
		{
			>= '1' and <= '9' => packed[index: 3] - '0',
			'A' => 10,
			'B' => 11,
			'C' => 12,
			_ => 0,
		};
		int day = packed[index: 4] switch
		{
			>= '1' and <= '9' => packed[index: 4] - '0',
			>= 'A' and <= 'V' => packed[index: 4] - 'A' + 10,
			_ => 0,
		};
		if (month is < 1 or > 12 || day is < 1 or > 31 || day > DateTime.DaysInMonth(year: year, month: month))
		{
			return false;
		}
		int a = (14 - month) / 12;
		int y = year + 4800 - a;
		int m = month + (12 * a) - 3;
		int jdn = day + (((153 * m) + 2) / 5) + (365 * y) + (y / 4) - (y / 100) + (y / 400) - 32045;
		julianDate = jdn - 0.5;
		return true;
	}

	/// <summary>Computes the current mean anomaly (degrees) for the analytic two-body fallback model.</summary>
	/// <param name="m0Deg">Mean anomaly at the reference epoch in degrees.</param>
	/// <param name="semiMajorAxisAu">Semi-major axis in AU (used to compute mean motion via Kepler's third law when <paramref name="meanMotionDegPerDay"/> is unavailable).</param>
	/// <param name="epochJd">Julian Date of the reference epoch.</param>
	/// <param name="nowJd">Julian Date of the current time.</param>
	/// <param name="meanMotionDegPerDay">Optional mean daily motion in degrees per day.</param>
	/// <returns>Current mean anomaly in degrees, normalized to [0°, 360°).</returns>
	private static double CurrentMeanAnomaly(double m0Deg, double semiMajorAxisAu, double epochJd, double nowJd, double? meanMotionDegPerDay = null)
	{
		double n = meanMotionDegPerDay ?? (EarthMeanMotion / Math.Pow(x: semiMajorAxisAu, y: 1.5));
		double m = m0Deg + (n * (nowJd - epochJd));
		return ((m % 360.0) + 360.0) % 360.0;
	}

	/// <summary>Converts a <see cref="DateTime"/> value to a Julian Date.</summary>
	/// <param name="dt">The date/time to convert (UTC recommended).</param>
	/// <returns>The Julian Date corresponding to the given date/time.</returns>
	private static double DateTimeToJd(DateTime dt)
	{
		int y = dt.Year;
		int mo = dt.Month;
		double d = dt.Day + ((dt.Hour + ((dt.Minute + (dt.Second / 60.0)) / 60.0)) / 24.0);
		if (mo <= 2)
		{
			y--;
			mo += 12;
		}
		int a = y / 100;
		int b = 2 - a + (a / 4);
		return (int)(365.25 * (y + 4716)) + (int)(30.6001 * (mo + 1)) + d + b - 1524.5;
	}

	// ---- OpenGL coordinate mapping ----

	/// <summary>Maps heliocentric ecliptic coordinates to OpenGL scene coordinates.</summary>
	/// <param name="ex">Ecliptic X in AU.</param>
	/// <param name="ey">Ecliptic Y in AU.</param>
	/// <param name="ez">Ecliptic Z in AU (positive = above ecliptic plane).</param>
	/// <returns>OpenGL (glX, glY, glZ) floats.</returns>
	private static (float Gx, float Gy, float Gz) EclToGl(double ex, double ey, double ez)
	{
		return (Gx: (float)ex, Gy: (float)ez, Gz: (float)-ey);
	}

	/// <summary>Invalidates the cached current body positions so they are recomputed for the next render or hover lookup.</summary>
	private void InvalidateBodyCache()
	{
		_cachedPlanetBodies = [];
		_cachedPlanetoidBodies = [];
		_cachedBodyPositionsJd = double.NaN;
	}

	/// <summary>Clears the hover target and optionally redraws the OpenGL control.</summary>
	/// <param name="redraw"><see langword="true"/> to invalidate the control when the hover text changed; otherwise <see langword="false"/>.</param>
	private void ClearHoverTarget(bool redraw = true)
	{
		if (_hoverName is null)
		{
			return;
		}
		_hoverName = null;
		if (redraw && _glReady)
		{
			_glControl.Invalidate();
		}
	}

	/// <summary>Ensures that the current body-position cache matches the requested simulation time.</summary>
	/// <param name="nowJd">Julian Date of the active simulation instant, in UTC.</param>
	private void EnsureBodyCache(double nowJd)
	{
		double nowJdTt = nowJd + (TtMinusUtcSeconds / SecondsPerDay);
		int renderedPlanetoidCount = Math.Min(val1: _planetoids.Count, val2: MaxRenderedPlanetoids);
		if (_cachedPlanetBodies.Length == Planets.Length
			&& _cachedPlanetoidBodies.Length == renderedPlanetoidCount
			&& Math.Abs(value: _cachedBodyPositionsJd - nowJd) < 1e-12)
		{
			return;
		}

		_cachedPlanetBodies = new RenderedBody[Planets.Length];
		for (int idx = 0; idx < Planets.Length; idx++)
		{
			(string name, double a, double e, double i, double om, double peri, double m0, Color color) = Planets[idx];
			double mNow = CurrentMeanAnomaly(m0Deg: m0, semiMajorAxisAu: a, epochJd: J2000Jd, nowJd: nowJdTt);
			(double ex, double ey, double ez) = OrbElemToEcliptic(a: a, e: e, iDeg: i, omDeg: om, periDeg: peri, mDeg: mNow);
			_cachedPlanetBodies[idx] = new RenderedBody(Name: name, Color: color, Ex: ex, Ey: ey, Ez: ez);
		}

		_cachedPlanetoidBodies = new RenderedBody[renderedPlanetoidCount];

		if (_propagatedPlanetoidStates.Length != renderedPlanetoidCount || !double.IsFinite(d: _propagatedPlanetoidStatesJdTt))
		{
			_propagatedPlanetoidStates = new StateVector[renderedPlanetoidCount];
			_propagatedPlanetoidStatesJdTt = double.NaN;
			for (int idx = 0; idx < renderedPlanetoidCount; idx++)
			{
				PlanetoidElements p = _planetoids[index: idx];
				StateVector initial = OrbElemToStateVector(a: p.A, e: p.E, iDeg: p.I, omDeg: p.Om, periDeg: p.Peri, mDeg: p.M0);
				double periodDays = p.MeanMotion.HasValue && p.MeanMotion.Value > 0.0
					? 360.0 / p.MeanMotion.Value
					: 2.0 * Math.PI * Math.Sqrt(d: p.A * p.A * p.A / SolarMuAu3PerDay2);
				_propagatedPlanetoidStates[idx] = PropagatePerturbedState(
					initialState: initial,
					startJdTt: p.EpochJd,
					endJdTt: nowJdTt,
					orbitalPeriodDays: periodDays);
			}
			_propagatedPlanetoidStatesJdTt = nowJdTt;
		}
		else if (Math.Abs(value: _propagatedPlanetoidStatesJdTt - nowJdTt) >= 1e-12)
		{
			double deltaT = nowJdTt - _propagatedPlanetoidStatesJdTt;
			for (int idx = 0; idx < renderedPlanetoidCount; idx++)
			{
				PlanetoidElements p = _planetoids[index: idx];
				double periodDays = p.MeanMotion.HasValue && p.MeanMotion.Value > 0.0
					? 360.0 / p.MeanMotion.Value
					: 2.0 * Math.PI * Math.Sqrt(d: p.A * p.A * p.A / SolarMuAu3PerDay2);
				_propagatedPlanetoidStates[idx] = PropagatePerturbedState(
					initialState: _propagatedPlanetoidStates[idx],
					startJdTt: _propagatedPlanetoidStatesJdTt,
					endJdTt: _propagatedPlanetoidStatesJdTt + deltaT,
					orbitalPeriodDays: periodDays);
			}
			_propagatedPlanetoidStatesJdTt = nowJdTt;
		}

		for (int idx = 0; idx < renderedPlanetoidCount; idx++)
		{
			PlanetoidElements p = _planetoids[index: idx];
			StateVector state = _propagatedPlanetoidStates[idx];
			_cachedPlanetoidBodies[idx] = new RenderedBody(Name: p.Name, Color: Color.Orange, Ex: state.X, Ey: state.Y, Ez: state.Z);
		}
		_cachedBodyPositionsJd = nowJd;
	}

	// ---- OpenGL rendering ----

	/// <summary>Sets up the OpenGL viewport and perspective projection matrix for the current control size.</summary>
	/// <remarks>The projection is a perspective with a 45° vertical field of view and near/far planes at 0.1 AU and 2000 AU.</remarks>
	private void SetupProjection()
	{
		int w = _glControl.Width;
		int h = Math.Max(val1: _glControl.Height, val2: 1);
		GL.Viewport(x: 0, y: 0, width: w, height: h);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		double aspect = (double)w / h;
		double fovY = 45.0 * Math.PI / 180.0;
		double f = 1.0 / Math.Tan(a: fovY / 2.0);
		double[] proj =
		[
			f / aspect, 0.0,   0.0,                           0.0,
			0.0,        f,     0.0,                           0.0,
			0.0,        0.0,  (2000.0 + 0.1) / (0.1 - 2000.0), -1.0,
			0.0,        0.0,   2.0 * 2000.0 * 0.1 / (0.1 - 2000.0), 0.0,
		];
		GL.LoadMatrix(m: ref proj[0]);
		GL.MatrixMode(mode: MatrixMode.Modelview);
	}

	/// <summary>Applies the camera transform (translate then rotate) to the current model-view matrix.</summary>
	/// <remarks>This is used both for rendering and for computing the model-view-projection when projecting a body to screen space for hover detection.</remarks>
	private void ApplyCameraTransform()
	{
		GL.LoadIdentity();
		GL.Translate(x: _panX, y: _panY, z: -_zoom);
		GL.Rotate(angle: _pitch, x: 1f, y: 0f, z: 0f);
		GL.Rotate(angle: _yaw, x: 0f, y: 1f, z: 0f);
	}

	/// <summary>Renders the full 3D scene to the OpenGL surface.</summary>
	/// <param name="overlayGraphics">Optional GDI+ graphics used to draw the hover tooltip overlay.</param>
	private void RenderScene(Graphics? overlayGraphics = null)
	{
		if (!_glReady)
		{
			logger.Error(message: "OpenGL context is not ready; skipping render.");
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
		if (!_rangeSelectionReady)
		{
			_glControl.SwapBuffers();
			return;
		}
		SetupProjection();
		ApplyCameraTransform();
		double nowJd = DateTimeToJd(dt: _simulationTime);
		EnsureBodyCache(nowJd: nowJd);
		DrawEclipticGrid();
		DrawPlanetOrbits(cachedOrbits: _cachedPlanetOrbits!);
		DrawPlanetoidOrbits();
		DrawSun();
		DrawPlanetCurrentPositions();
		DrawPlanetoidCurrentPositions();
		_glControl.SwapBuffers();
		if (overlayGraphics is not null && _hoverName is not null)
		{
			DrawHoverTooltip(g: overlayGraphics);
		}
	}

	/// <summary>Draws a subtle square grid on the ecliptic plane (OpenGL Y = 0).</summary>
	/// <remarks>The grid extends from -40 to +40 AU in both X and Z directions, with lines every 5 AU.</remarks>
	private static void DrawEclipticGrid()
	{
		const int gridExtent = 40;
		const int gridStep = 5;
		GL.LineWidth(width: 1f);
		GL.Color4(red: 0.15f, green: 0.15f, blue: 0.25f, alpha: 0.8f);
		GL.Begin(mode: PrimitiveType.Lines);
		for (int x = -gridExtent; x <= gridExtent; x += gridStep)
		{
			GL.Vertex3(x: x, y: 0f, z: -gridExtent);
			GL.Vertex3(x: x, y: 0f, z: gridExtent);
		}
		for (int z = -gridExtent; z <= gridExtent; z += gridStep)
		{
			GL.Vertex3(x: -gridExtent, y: 0f, z: z);
			GL.Vertex3(x: gridExtent, y: 0f, z: z);
		}
		GL.End();
	}

	/// <summary>Draws the orbit ellipses of the eight solar system planets.</summary>
	/// <param name="cachedOrbits">The cached orbit point arrays for each planet.</param>
	/// <remarks>Each planet's orbit is drawn as a line strip in a distinct color, dimmed slightly so the current-position markers stand out.</remarks>
	private static void DrawPlanetOrbits((double X, double Y, double Z)[][] cachedOrbits)
	{
		GL.LineWidth(width: 1.5f);
		for (int idx = 0; idx < Planets.Length; idx++)
		{
			Color col = Planets[idx].Col;
			(double X, double Y, double Z)[] pts = cachedOrbits[idx];
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

	/// <summary>Draws the orbit ellipses of the currently selected planetoids, if cached.</summary>
	/// <remarks>Orbit lines are drawn in a dim orange color. When more planetoids are selected than <see cref="MaxOrbitLines"/>, orbit lines are skipped and only current positions are drawn.</remarks>
	private void DrawPlanetoidOrbits()
	{
		if (_cachedPlanetoidOrbits is null)
		{
			return;
		}
		GL.LineWidth(width: 1f);
		GL.Color4(red: 1.0f, green: 0.55f, blue: 0.15f, alpha: 0.30f);
		foreach ((double X, double Y, double Z)[] pts in _cachedPlanetoidOrbits)
		{
			GL.Begin(mode: PrimitiveType.LineStrip);
			foreach ((double ex, double ey, double ez) in pts)
			{
				(float gx, float gy, float gz) = EclToGl(ex: ex, ey: ey, ez: ez);
				GL.Vertex3(x: gx, y: gy, z: gz);
			}
			GL.End();
		}
	}

	/// <summary>Draws the Sun at the origin as a bright yellow point with a faint halo ring.</summary>
	private static void DrawSun()
	{
		GL.PointSize(size: 14f);
		GL.Color3(red: 1.0f, green: 1.0f, blue: 0.0f);
		GL.Begin(mode: PrimitiveType.Points);
		GL.Vertex3(x: 0f, y: 0f, z: 0f);
		GL.End();
		GL.LineWidth(width: 1f);
		GL.Color4(red: 1.0f, green: 0.9f, blue: 0.4f, alpha: 0.35f);
		GL.Begin(mode: PrimitiveType.LineLoop);
		const double haloRadius = 0.28;
		for (int k = 0; k < 36; k++)
		{
			double angle = k * Math.PI / 18.0;
			GL.Vertex3(x: (float)(haloRadius * Math.Cos(d: angle)), y: 0f, z: (float)(haloRadius * Math.Sin(a: angle)));
		}
		GL.End();
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws the current position of each of the eight planets as a colored point marker.</summary>
	private void DrawPlanetCurrentPositions()
	{
		GL.PointSize(size: 6f);
		foreach (RenderedBody body in _cachedPlanetBodies)
		{
			(float gx, float gy, float gz) = EclToGl(ex: body.Ex, ey: body.Ey, ez: body.Ez);
			GL.Color3(red: body.Color.R / 255f, green: body.Color.G / 255f, blue: body.Color.B / 255f);
			GL.Begin(mode: PrimitiveType.Points);
			GL.Vertex3(x: gx, y: gy, z: gz);
			GL.End();
		}
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws the current position of each rendered planetoid as a small orange point marker.</summary>
	private void DrawPlanetoidCurrentPositions()
	{
		GL.PointSize(size: 4f);
		GL.Color3(red: 1.0f, green: 0.45f, blue: 0.10f);
		GL.Begin(mode: PrimitiveType.Points);
		foreach (RenderedBody body in _cachedPlanetoidBodies)
		{
			(float gx, float gy, float gz) = EclToGl(ex: body.Ex, ey: body.Ey, ez: body.Ez);
			GL.Vertex3(x: gx, y: gy, z: gz);
		}
		GL.End();
		GL.PointSize(size: 1f);
	}

	/// <summary>Draws the hover tooltip text near the cursor position.</summary>
	/// <param name="g">The GDI+ graphics surface for the GL control.</param>
	/// <remarks>The tooltip is drawn with a semi-transparent dark background for readability over the scene.</remarks>
	private void DrawHoverTooltip(Graphics g)
	{
		if (_hoverName is null)
		{
			return;
		}
		SizeF size = g.MeasureString(text: _hoverName, font: _overlayFont);
		float x = _hoverPoint.X + 12;
		float y = _hoverPoint.Y + 12;
		using SolidBrush back = new(color: Color.FromArgb(alpha: 190, red: 0, green: 0, blue: 0));
		g.FillRectangle(brush: back, x: x - 2, y: y - 1, width: size.Width + 4, height: size.Height + 2);
		using SolidBrush fore = new(color: Color.White);
		g.DrawString(s: _hoverName, font: _overlayFont, brush: fore, x: x, y: y);
	}

	// ---- Hover detection ----

	/// <summary>Projects a heliocentric ecliptic position to GL-control client pixel coordinates using the current camera.</summary>
	/// <param name="ex">Ecliptic X in AU.</param>
	/// <param name="ey">Ecliptic Y in AU.</param>
	/// <param name="ez">Ecliptic Z in AU.</param>
	/// <param name="screen">When this method returns, contains the projected client-pixel position on success.</param>
	/// <returns><see langword="true"/> if the point projects in front of the camera; otherwise <see langword="false"/>.</returns>
	private bool ProjectToScreen(double ex, double ey, double ez, out PointF screen)
	{
		screen = default;
		(float gx, float gy, float gz) = EclToGl(ex: ex, ey: ey, ez: ez);
		float yawRad = _yaw * (float)Math.PI / 180f;
		float pitchRad = _pitch * (float)Math.PI / 180f;
		// Rotate about Y (yaw), then about X (pitch), matching GL.Rotate(pitch) then GL.Rotate(yaw) order applied to the point.
		float cosY = (float)Math.Cos(d: yawRad), sinY = (float)Math.Sin(a: yawRad);
		float x1 = (gx * cosY) + (gz * sinY);
		float z1 = (-gx * sinY) + (gz * cosY);
		float y1 = gy;
		float cosP = (float)Math.Cos(d: pitchRad), sinP = (float)Math.Sin(a: pitchRad);
		float y2 = (y1 * cosP) - (z1 * sinP);
		float z2 = (y1 * sinP) + (z1 * cosP);
		float x2 = x1;
		// Translate by camera
		float camX = x2 + _panX;
		float camY = y2 + _panY;
		float camZ = z2 - _zoom;
		if (camZ >= -0.01f)
		{
			return false;
		}
		int w = _glControl.Width;
		int h = Math.Max(val1: _glControl.Height, val2: 1);
		double aspect = (double)w / h;
		double fovY = 45.0 * Math.PI / 180.0;
		double f = 1.0 / Math.Tan(a: fovY / 2.0);
		double ndcX = f / aspect * camX / -camZ;
		double ndcY = f * camY / -camZ;
		screen = new PointF(x: (float)((ndcX + 1.0) * 0.5 * w), y: (float)((1.0 - ndcY) * 0.5 * h));
		return true;
	}

	/// <summary>Determines the name of the body nearest to the cursor, within a small pixel radius.</summary>
	/// <param name="cursor">The cursor position in GL-control client coordinates.</param>
	/// <returns>The name of the closest body under the cursor, or <see langword="null"/> when none is within the pick radius.</returns>
	private string? PickBodyAt(Point cursor)
	{
		if (!_rangeSelectionReady)
		{
			return null;
		}
		const float pickRadius = 8f;
		float bestDistSq = pickRadius * pickRadius;
		string? best = null;
		double nowJd = DateTimeToJd(dt: _simulationTime);
		EnsureBodyCache(nowJd: nowJd);
		foreach (RenderedBody body in _cachedPlanetBodies)
		{
			if (ProjectToScreen(ex: body.Ex, ey: body.Ey, ez: body.Ez, out PointF s))
			{
				float dsq = ((s.X - cursor.X) * (s.X - cursor.X)) + ((s.Y - cursor.Y) * (s.Y - cursor.Y));
				if (dsq < bestDistSq)
				{
					bestDistSq = dsq;
					best = body.Name;
				}
			}
		}
		foreach (RenderedBody body in _cachedPlanetoidBodies)
		{
			if (ProjectToScreen(ex: body.Ex, ey: body.Ey, ez: body.Ez, out PointF s))
			{
				float dsq = ((s.X - cursor.X) * (s.X - cursor.X)) + ((s.Y - cursor.Y) * (s.Y - cursor.Y));
				if (dsq < bestDistSq)
				{
					bestDistSq = dsq;
					best = body.Name;
				}
			}
		}
		return best;
	}

	/// <summary>Updates the status bar label with the current orrery state.</summary>
	/// <remarks>The status bar displays the number of selected planetoids, the simulated instant, and basic interaction instructions.</remarks>
	private void UpdateStatusLabel()
	{
		SetStatusBar(label: labelInformation, text:
			$"Orrery — {_planetoids.Count} planetoids + 8 planets · " +
			$"{_simulationTime.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.CurrentCulture)} UTC · " +
			$"Left-drag: rotate · Right-drag: pan · Scroll: zoom");
	}

	/// <summary>Synchronizes the date/time control with the current simulation time without re-triggering its change handler.</summary>
	/// <remarks>The picker is set to the local-kind equivalent so the control displays the same wall-clock digits as the simulated UTC instant.</remarks>
	private void SyncDateTimePicker()
	{
		_suppressDateTimeEvent = true;
		try
		{
			DateTime asLocal = DateTime.SpecifyKind(value: _simulationTime, kind: DateTimeKind.Unspecified);
			if (asLocal >= dateTimePicker.MinDate && asLocal <= dateTimePicker.MaxDate)
			{
				dateTimePicker.Value = asLocal;
			}
		}
		finally
		{
			_suppressDateTimeEvent = false;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the <see cref="Form.Load"/> event.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Creates the <see cref="GLControl"/>, initializes the OpenGL context, configures the range spinners, and triggers the first render.</remarks>
	private async void OrreryForm_Load(object? sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		int total = _sourceLines.Count;
		ConfigureIndexControl(control: numericStartIndex, value: total == 0 ? 0 : 1, max: total);
		ConfigureIndexControl(control: numericEndIndex, value: total, max: total);
		try
		{
			CreateGlControl();
			_cachedPlanetOrbits = new (double X, double Y, double Z)[Planets.Length][];
			for (int idx = 0; idx < Planets.Length; idx++)
			{
				(_, double a, double ecc, double i, double om, double peri, _, _) = Planets[idx];
				_cachedPlanetOrbits[idx] = ComputeOrbitPoints(a: a, e: ecc, iDeg: i, omDeg: om, periDeg: peri);
			}
			_glControl.MakeCurrent();
			GL.Enable(cap: EnableCap.DepthTest);
			_glReady = true;
			_simulationTime = DateTime.UtcNow;
			SyncDateTimePicker();
			await RebuildRangeAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (IsDisposed)
			{
				return;
			}
			_initialized = true;
			_glControl.Invalidate();
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "OrreryForm: failed to initialize OpenGL context: {0}", args: ex);
			ShowErrorMessage(message: $"Failed to initialize orrery rendering: {ex.Message}");
		}
	}

	/// <summary>Configures a range spinner control with the given value and one-based maximum.</summary>
	/// <param name="control">The spinner control to configure.</param>
	/// <param name="value">The initial value.</param>
	/// <param name="max">The maximum (record count).</param>
	/// <remarks>The minimum is 1 unless there are no records, in which case both bounds are 0. The ValueChanged handler is wired to rebuild the selected range.</remarks>
	private void ConfigureIndexControl(ToolStripNumericUpDown control, int value, int max)
	{
		control.DecimalPlaces = 0;
		control.Increment = 1m;
		control.Minimum = max == 0 ? 0m : 1m;
		control.Maximum = Math.Max(val1: max, val2: control.Minimum == 0m ? 0 : 1);
		control.Value = Math.Clamp(value: value, min: (int)control.Minimum, max: (int)control.Maximum);
		control.Enter += Control_Enter;
		control.Leave += Control_Leave;
		control.MouseEnter += Control_Enter;
		control.MouseLeave += Control_Leave;
		control.ValueChanged += NumericIndex_ValueChanged;
	}

	#endregion

	#region Control event handlers

	/// <summary>Handles changes to either range spinner by rebuilding the selected planetoid range.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Ignored until the form has finished loading to avoid rebuilding during initialization.</remarks>
	private async void NumericIndex_ValueChanged(object? sender, EventArgs e)
	{
		if (!_initialized)
		{
			return;
		}
		await RebuildRangeAsync().ConfigureAwait(continueOnCapturedContext: true);
	}

	/// <summary>Handles the play/pause button click by toggling the animation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Starts or stops the animation timer and updates the button caption and image accordingly.</remarks>
	private void ButtonPlayPause_Click(object? sender, EventArgs e)
	{
		_isPlaying = !_isPlaying;
		if (_isPlaying)
		{
			_animationTimer.Start();
			buttonPlayPause.Text = "&Pause";
			buttonPlayPause.Image = Resources.FatcowIcons16px.fatcow_control_pause_blue_16px;
		}
		else
		{
			_animationTimer.Stop();
			buttonPlayPause.Text = "&Play";
			buttonPlayPause.Image = Resources.FatcowIcons16px.fatcow_control_play_blue_16px;
		}
	}

	/// <summary>Handles the reset button click by resetting the simulation time to the current instant.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The speed slider is also reset to zero and the animation is paused.</remarks>
	private void ButtonReset_Click(object? sender, EventArgs e)
	{
		_simulationTime = DateTime.UtcNow;
		InvalidateBodyCache();
		ClearHoverTarget(redraw: false);
		trackBarSpeed.Value = 0;
		if (_isPlaying)
		{
			ButtonPlayPause_Click(sender: this, e: EventArgs.Empty);
		}
		SyncDateTimePicker();
		UpdateStatusLabel();
		if (_glReady)
		{
			_glControl.Invalidate();
		}
	}

	/// <summary>Handles changes to the time-speed slider.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>A value of zero pauses time flow while the animation is running; negative values move backward in time.</remarks>
	private void TrackBarSpeed_ValueChanged(object? sender, EventArgs e)
	{
		UpdateStatusLabel();
	}

	/// <summary>Handles changes to the date/time control by setting the simulation instant.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Ignored while the control is being updated programmatically from the simulation clock.</remarks>
	private void DateTimePicker_ValueChanged(object? sender, EventArgs e)
	{
		if (_suppressDateTimeEvent)
		{
			return;
		}
		_simulationTime = DateTime.SpecifyKind(value: dateTimePicker.Value, kind: DateTimeKind.Utc);
		InvalidateBodyCache();
		ClearHoverTarget(redraw: false);
		UpdateStatusLabel();
		if (_glReady)
		{
			_glControl.Invalidate();
		}
	}

	/// <summary>Advances the simulation clock on each animation tick according to the slider speed.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>The simulated instant is advanced by the slider value times <see cref="DaysPerTickPerUnit"/> days per tick and the scene is redrawn.</remarks>
	private void AnimationTimer_Tick(object? sender, EventArgs e)
	{
		int speed = trackBarSpeed.Value;
		if (speed == 0)
		{
			return;
		}
		double deltaDays = speed * DaysPerTickPerUnit;
		try
		{
			_simulationTime = _simulationTime.AddDays(value: deltaDays);
		}
		catch (ArgumentOutOfRangeException)
		{
			// Reached the representable DateTime range; stop the animation gracefully.
			ButtonPlayPause_Click(sender: this, e: EventArgs.Empty);
			return;
		}
		InvalidateBodyCache();
		ClearHoverTarget(redraw: false);
		SyncDateTimePicker();
		UpdateStatusLabel();
		if (_glReady)
		{
			_glControl.Invalidate();
		}
	}

	#endregion

	#region GLControl event handlers

	/// <summary>Handles the <see cref="Control.Paint"/> event of the GL control to redraw the scene.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Paint event arguments.</param>
	/// <remarks>Triggers rendering of the 3D scene, passing the GDI+ graphics for the hover tooltip overlay.</remarks>
	private void GlControl_Paint(object? sender, PaintEventArgs e)
	{
		if (!_glReady)
		{
			logger.Warn(message: "OpenGL context is not ready; skipping paint event.");
			return;
		}
		RenderScene(overlayGraphics: e.Graphics);
	}

	/// <summary>Handles the <see cref="Control.Resize"/> event of the GL control to update the viewport.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Updates the OpenGL viewport and projection matrix whenever the GL control is resized.</remarks>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		if (!_glReady)
		{
			return;
		}
		ClearHoverTarget(redraw: false);
		_glControl.MakeCurrent();
		SetupProjection();
		_glControl.Invalidate();
	}

	/// <summary>Handles mouse-button-down events on the GL control to begin camera interaction.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	/// <remarks>Left-click initiates rotation, while right-click initiates panning.</remarks>
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
	/// <remarks>Releases the rotation or panning mode when the corresponding mouse button is released.</remarks>
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

	/// <summary>Handles mouse-move events on the GL control to rotate or pan the camera and update the hover tooltip.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	/// <remarks>Left-drag rotates the scene, right-drag pans. When no button is held, the body nearest the cursor is detected for the hover tooltip.</remarks>
	private void GlControl_MouseMove(object? sender, MouseEventArgs e)
	{
		int dx = e.X - _lastMousePos.X;
		int dy = e.Y - _lastMousePos.Y;
		_lastMousePos = e.Location;
		_hoverPoint = e.Location;
		if (_leftDown)
		{
			_yaw += dx * 0.5f;
			_pitch += dy * 0.5f;
			_pitch = Math.Clamp(value: _pitch, min: -89f, max: 89f);
			ClearHoverTarget(redraw: false);
			_glControl.Invalidate();
		}
		else if (_rightDown)
		{
			_panX += dx * _zoom * 0.001f;
			_panY -= dy * _zoom * 0.001f;
			ClearHoverTarget(redraw: false);
			_glControl.Invalidate();
		}
		else
		{
			string? previous = _hoverName;
			_hoverName = PickBodyAt(cursor: e.Location);
			if (_hoverName != previous)
			{
				_glControl.Invalidate();
			}
		}
	}

	/// <summary>Handles scroll-wheel events on the GL control to zoom in or out.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Mouse event arguments.</param>
	/// <remarks>Zooms the camera in or out based on the scroll delta, with limits to prevent excessive zooming.</remarks>
	private void GlControl_MouseWheel(object? sender, MouseEventArgs e)
	{
		_zoom -= e.Delta * 0.02f;
		_zoom = Math.Clamp(value: _zoom, min: 0.5f, max: 200f);
		ClearHoverTarget(redraw: false);
		_glControl.Invalidate();
	}

	/// <summary>Clears the hover tooltip when the cursor leaves the GL control.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>Invalidates the control so the tooltip is removed from the display.</remarks>
	private void GlControl_MouseLeave(object? sender, EventArgs e)
	{
		ClearHoverTarget();
	}

	#endregion
}
