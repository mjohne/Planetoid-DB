// This file contains the implementation of the Orbits2DSideViewForm,
// which displays a 2D side-view orbit visualization of a selected
// minor planet relative to the eight solar system planets.
// Each orbit is drawn as a line through the origin (Sun), where the
// inclination angle determines the slope and the semi-major axis with
// eccentricity determines the perihelion/aphelion arm lengths.
using NLog;

using Planetoid_DB.Helpers;

using ScottPlot;
using ScottPlot.Plottables;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a 2D side-view orbit visualization of a selected minor planet relative to the eight solar system planets.</summary>
/// <remarks>The form renders each orbit as a straight line through the origin (Sun) where the slope of the line corresponds to the orbital inclination. The perihelion arm (length = a·(1–e)) is drawn above the ecliptic plane and the aphelion arm (length = a·(1+e)) below it. The X- and Y-axes are scaled to the extent of the planetoid's orbit; planet orbits that extend beyond this range are still rendered and can be revealed by zooming out.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class Orbit2DSideViewForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger records events and errors that occur during orbit visualization rendering.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Name or designation of the selected minor planet.</summary>
	/// <remarks>Displayed in the plot title to identify which planetoid is being visualized.</remarks>
	private readonly string _planetoidName;

	/// <summary>Semi-major axis of the selected minor planet's orbit in AU.</summary>
	/// <remarks>Set via the constructor before the form is shown.</remarks>
	private readonly double _semiMajorAxis;

	/// <summary>Orbital eccentricity of the selected minor planet.</summary>
	/// <remarks>Set via the constructor before the form is shown.</remarks>
	private readonly double _eccentricity;

	/// <summary>Orbital inclination of the selected minor planet in degrees.</summary>
	/// <remarks>Determines the slope of the orbit line in the side-view diagram. 0° is along the positive X-axis; 90° points straight up the positive Y-axis, continuing counterclockwise.</remarks>
	private readonly double _inclinationDeg;

	/// <summary>Mean orbital elements for the eight solar system planets at J2000.0 (ecliptic reference frame).</summary>
	/// <remarks>These values are sourced from the same dataset used in <see cref="MoidCalculator"/> to ensure consistency. Each entry stores the planet name, semi-major axis in AU, eccentricity, and inclination in degrees.</remarks>
	private static readonly (string Name, double SemiMajorAxis, double Eccentricity, double InclinationDeg)[] PlanetOrbits =
	[
		("Mercury", 0.38709893, 0.20563069, 7.00487),
		("Venus",   0.72333199, 0.00677323, 3.39471),
		("Earth",   1.00000011, 0.01671022, 0.00005),
		("Mars",    1.52366231, 0.09341233, 1.85061),
		("Jupiter", 5.20336301, 0.04839266, 1.30530),
		("Saturn",  9.53707032, 0.05415060, 2.48446),
		("Uranus",  19.19126393, 0.04716771, 0.76986),
		("Neptune", 30.06896348, 0.00858587, 1.76917),
	];

	/// <summary>Colors used to draw each planet's orbit line, in Mercury–Neptune order.</summary>
	/// <remarks>Colors are chosen to be visually distinct from one another and from the planetoid (drawn in orange-red).</remarks>
	private static readonly ScottPlot.Color[] PlanetColors =
	[
		new ScottPlot.Color(red: 0xB0, green: 0xB0, blue: 0xD0),   // Mercury – light blue-gray
		new ScottPlot.Color(red: 0xFF, green: 0xD7, blue: 0x00),   // Venus – golden
		new ScottPlot.Color(red: 0x40, green: 0x80, blue: 0xFF),   // Earth – blue
		new ScottPlot.Color(red: 0xCC, green: 0x44, blue: 0x22),   // Mars – red-brown
		new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Jupiter – tan
		new ScottPlot.Color(red: 0xD4, green: 0xC0, blue: 0x70),   // Saturn – pale gold
		new ScottPlot.Color(red: 0x80, green: 0xE0, blue: 0xE8),   // Uranus – cyan
		new ScottPlot.Color(red: 0x30, green: 0x50, blue: 0xCC),   // Neptune – deep blue
	];

	/// <summary>Radius of the Sun marker in AU used for the central circle in the diagram.</summary>
	/// <remarks>The actual solar radius is approximately 0.00465 AU. A slightly larger value of 0.02 AU is used here to ensure the marker remains clearly visible at typical diagram scales.</remarks>
	private const double SunRadiusAu = 0.02;

	/// <summary>Line width applied to planet orbit lines.</summary>
	/// <remarks>A slightly thicker line (2 pixels) is used to make the orbital paths easier to distinguish from the plot background.</remarks>
	private const float OrbitLineWidth = 2f;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="Orbit2DSideViewForm"/> class.</summary>
	/// <param name="planetoidName">The name or designation of the selected minor planet.</param>
	/// <param name="semiMajorAxis">The semi-major axis of the planetoid's orbit in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity of the planetoid.</param>
	/// <param name="inclinationDeg">The orbital inclination of the planetoid in degrees.</param>
	/// <remarks>The supplied orbital elements are used immediately on form load to render the side-view orbital diagram.</remarks>
	public Orbit2DSideViewForm(string planetoidName, double semiMajorAxis, double eccentricity, double inclinationDeg)
	{
		InitializeComponent();
		// Store the planetoid's orbital parameters for use during rendering.
		_planetoidName = planetoidName;
		_semiMajorAxis = semiMajorAxis;
		_eccentricity = eccentricity;
		_inclinationDeg = inclinationDeg;
		// Log the initialization of the form with the provided parameters for debugging purposes.
		logger.Info(
			message: "Orbit2DSideViewForm initialized for planetoid '{0}' (a={1} AU, e={2}, i={3}°).",
			args: [_planetoidName, _semiMajorAxis.ToString(format: "F6", provider: CultureInfo.InvariantCulture), _eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture), _inclinationDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture)]);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>The method currently returns the same string as <c>ToString()</c> on this instance, but can be customized to include more specific information if needed.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Renders the 2D side-view orbital diagram in the ScottPlot control.</summary>
	/// <remarks><para>Each orbit is represented as a straight line through the origin (Sun). The perihelion arm of the line points in the direction of the orbital inclination (0° = positive X-axis, 90° = positive Y-axis, counterclockwise) and has length equal to the perihelion distance a·(1–e). The aphelion arm points in the opposite direction and has length a·(1+e).</para>
	/// <para>The axis limits are set to the planetoid's aphelion distance plus a small margin. Planet orbits that extend beyond this range are still drawn and can be accessed by zooming out. Both axes are scaled equally (SquareUnits) so that inclination angles appear geometrically correct.</para></remarks>
	private void RenderOrbitPlot()
	{
		formsPlotOrbits.Plot.Clear();
		// Configure title and axis labels.
		formsPlotOrbits.Plot.Title(text: $"Side view orbit: {_planetoidName}");
		formsPlotOrbits.Plot.Axes.Bottom.Label.Text = "Projected orbital distance (AU)";
		formsPlotOrbits.Plot.Axes.Left.Label.Text = "Inclination height (AU)";
		formsPlotOrbits.Plot.Legend.IsVisible = true;
		formsPlotOrbits.Plot.Legend.Alignment = Alignment.UpperRight;
		// Draw planet orbit lines first so the planetoid's line appears on top.
		for (int idx = 0; idx < PlanetOrbits.Length; idx++)
		{
			(string name, double a, double e, double inclinDeg) = PlanetOrbits[idx];
			AddOrbitLine(
				name: name,
				semiMajorAxis: a,
				eccentricity: e,
				inclinationDeg: inclinDeg,
				lineColor: PlanetColors[idx],
				lineWidth: OrbitLineWidth);
		}
		// Draw the planetoid's orbit line in a distinct orange-red so it stands out.
		AddOrbitLine(
			name: _planetoidName,
			semiMajorAxis: _semiMajorAxis,
			eccentricity: _eccentricity,
			inclinationDeg: _inclinationDeg,
			lineColor: Colors.OrangeRed,
			lineWidth: OrbitLineWidth + 1f);
		// Draw the Sun as a yellow filled circle at the origin.
		// It is added after the orbit lines so it is rendered on top.
		Ellipse sun = formsPlotOrbits.Plot.Add.Circle(xCenter: 0.0, yCenter: 0.0, radius: SunRadiusAu);
		sun.FillStyle.Color = Colors.Yellow;
		sun.LineStyle.Color = Colors.Orange;
		sun.LineStyle.Width = 1f;
		sun.LegendText = "Sun";
		// Scale the visible axes to the planetoid's orbit extent.
		// The aphelion distance (maximum distance from the Sun) defines the required axis range.
		// Planet orbits that extend beyond this limit remain drawn and can be explored by zooming out.
		double axisLimit;
		if (_semiMajorAxis > 0.0 && _eccentricity >= 0.0 && _eccentricity < 1.0)
		{
			double aphelion = _semiMajorAxis * (1.0 + _eccentricity);
			double axisMargin = aphelion * 0.15;
			axisLimit = aphelion + axisMargin;
		}
		else
		{
			logger.Warn(message: "Invalid planetoid parameters for axis scaling (a={0}, e={1}); using fallback axis limit.", args: [_semiMajorAxis, _eccentricity]);
			axisLimit = 1.0;
		}
		formsPlotOrbits.Plot.Axes.SetLimits(left: -axisLimit, right: axisLimit, bottom: -axisLimit, top: axisLimit);
		// Ensure both axes use the same scale (1 AU on X equals 1 AU on Y) so inclination angles appear geometrically correct.
		formsPlotOrbits.Plot.Axes.SquareUnits();
		formsPlotOrbits.Refresh();
		// Update the status bar with the key orbital parameters.
		labelInformation.Text =
			$"{_planetoidName} — a = {_semiMajorAxis.ToString(format: "F4", provider: CultureInfo.InvariantCulture)} AU, " +
			$"e = {_eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture)}, " +
			$"i = {_inclinationDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture)}°";
	}

	/// <summary>Adds a single orbit line to the side-view plot, centered at the Sun (origin).</summary>
	/// <param name="name">The legend label for this orbit line.</param>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="inclinationDeg">The orbital inclination in degrees. 0° places the perihelion end on the positive X-axis; 90° places it on the positive Y-axis; angles increase counterclockwise.</param>
	/// <param name="lineColor">The color used to draw the orbit line.</param>
	/// <param name="lineWidth">The width of the orbit line in pixels.</param>
	/// <remarks><para>The orbit is drawn as a straight line passing through the origin (Sun) from the aphelion endpoint to the perihelion endpoint:</para>
	/// <list type="bullet">
	/// <item><description>Perihelion endpoint: distance a·(1–e) from Sun, at angle <paramref name="inclinationDeg"/> CCW from +X-axis → (a·(1–e)·cos(i), a·(1–e)·sin(i)).</description></item>
	/// <item><description>Aphelion endpoint: distance a·(1+e) from Sun, at angle <paramref name="inclinationDeg"/>+180° → (–a·(1+e)·cos(i), –a·(1+e)·sin(i)).</description></item>
	/// </list>
	/// <para>This results in the perihelion lying in the upper (positive Y) half-plane for inclinations between 0° and 180°, and the aphelion in the lower half-plane, consistent with the side-view convention.</para></remarks>
	private void AddOrbitLine(
		string name,
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		ScottPlot.Color lineColor,
		float lineWidth)
	{
		// Guard against degenerate or invalid orbital parameters.
		if (semiMajorAxis <= 0.0 || eccentricity < 0.0 || eccentricity >= 1.0)
		{
			logger.Warn(message: "Skipping orbit line for '{0}': invalid parameters (a={1}, e={2}).", args: [name, semiMajorAxis, eccentricity]);
			return;
		}
		double inclRad = inclinationDeg * Math.PI / 180.0;
		double cosI = Math.Cos(d: inclRad);
		double sinI = Math.Sin(a: inclRad);
		double perihelionDist = semiMajorAxis * (1.0 - eccentricity);
		double aphelionDist = semiMajorAxis * (1.0 + eccentricity);
		// Perihelion endpoint: above the ecliptic plane for positive inclinations.
		double xPerihelion = perihelionDist * cosI;
		double yPerihelion = perihelionDist * sinI;
		// Aphelion endpoint: below the ecliptic plane (opposite direction from perihelion).
		double xAphelion = -aphelionDist * cosI;
		double yAphelion = -aphelionDist * sinI;
		// Draw a line from aphelion to perihelion through the origin (Sun).
		LinePlot line = formsPlotOrbits.Plot.Add.Line(
			x1: xAphelion,
			y1: yAphelion,
			x2: xPerihelion,
			y2: yPerihelion);
		line.Color = lineColor;
		line.LineWidth = lineWidth;
		line.LegendText = name;
		// Add small markers at perihelion and aphelion to aid visual identification.
		line.MarkerSize = 4f;
		line.MarkerShape = MarkerShape.FilledCircle;
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the Orbits2DSideViewForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and renders the orbital diagram when the form is first shown.</remarks>
	private void Orbits2DSideViewForm_Load(object? sender, EventArgs e)
	{
		// Clear the status bar before rendering to ensure any previous messages are removed.
		ClearStatusBar(label: labelInformation);
		// Render the orbital diagram, with error handling to catch and log any exceptions.
		try
		{
			RenderOrbitPlot();
		}
		catch (Exception ex)
		{
			logger.Error(message: $"Failed to render side-view orbit plot for '{_planetoidName}': {ex}");
			ShowErrorMessage(message: $"An error occurred while rendering the orbital diagram: {ex.Message}");
		}
	}

	#endregion
}