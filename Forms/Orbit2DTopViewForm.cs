// This file contains the implementation of the Orbit2DTopViewForm,
// which displays a 2D orbital plane visualization of a selected
// minor planet relative to the eight solar system planets.
using NLog;

using Planetoid_DB.Helpers;

using ScottPlot;
using ScottPlot.Plottables;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a 2D orbital plane visualization of a selected minor planet relative to the eight solar system planets.</summary>
/// <remarks>The form renders the orbit of the selected planetoid and all eight solar system planets as ellipses in the ecliptic plane using ScottPlot. The Sun is represented as a yellow circle at the focal point. The X- and Y-axes are scaled to the extent of the planetoid's orbit; planet orbits that extend beyond this range are still rendered and can be revealed by zooming out.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class Orbit2DTopViewForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to record any errors that occur during orbit visualization rendering.</remarks>
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

	/// <summary>Argument of perihelion of the selected minor planet in degrees.</summary>
	/// <remarks>Set via the constructor before the form is shown. Used as the rotation angle for the orbit ellipse.</remarks>
	private readonly double _argumentPerihelionDeg;

	/// <summary>Mean orbital elements for the eight solar system planets at J2000.0 (ecliptic reference frame).</summary>
	/// <remarks>These values are sourced from the same dataset used in <see cref="MoidCalculator"/> to ensure consistency. Each entry stores the planet name, semi-major axis in AU, eccentricity, and argument of perihelion in degrees.</remarks>
	private static readonly (string Name, double SemiMajorAxis, double Eccentricity, double ArgumentPerihelionDeg)[] PlanetOrbits =
	[
		("Mercury", 0.38709893, 0.20563069, 29.12478),
		("Venus",   0.72333199, 0.00677323, 54.85229),
		("Earth",   1.00000011, 0.01671022, 114.20783),
		("Mars",    1.52366231, 0.09341233, 286.46230),
		("Jupiter", 5.20336301, 0.04839266, 274.19770),
		("Saturn",  9.53707032, 0.05415060, 338.71690),
		("Uranus",  19.19126393, 0.04716771, 96.73436),
		("Neptune", 30.06896348, 0.00858587, 273.24966),
	];

	/// <summary>Radius of the Sun marker in AU used for the central circle in the diagram.</summary>
	/// <remarks>The actual solar radius is approximately 0.00465 AU. A slightly larger value of 0.02 AU is used here to ensure the marker remains clearly visible at typical diagram scales.</remarks>
	private const double SunRadiusAu = 0.02;

	/// <summary>Line width applied to all orbit ellipses.</summary>
	/// <remarks>A slightly thicker line (2 pixels) is used to make the orbital paths easier to distinguish from the plot background.</remarks>
	private const float OrbitLineWidth = 2f;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="Orbit2DTopViewForm"/> class.</summary>
	/// <param name="planetoidName">The name or designation of the selected minor planet.</param>
	/// <param name="semiMajorAxis">The semi-major axis of the planetoid's orbit in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity of the planetoid.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion of the planetoid in degrees.</param>
	/// <remarks>The supplied orbital elements are used immediately on form load to render the orbital diagram.</remarks>
	public Orbit2DTopViewForm(string planetoidName, double semiMajorAxis, double eccentricity, double argumentPerihelionDeg)
	{
		InitializeComponent();
		// Store the planetoid's orbital parameters for use during rendering.
		_planetoidName = planetoidName;
		_semiMajorAxis = semiMajorAxis;
		_eccentricity = eccentricity;
		_argumentPerihelionDeg = argumentPerihelionDeg;
		// Log the initialization of the form with the provided parameters for debugging purposes.
		logger.Info(
			message: "Orbit2DTopViewForm initialized for planetoid '{0}' (a={1} AU, e={2}, ω={3}°).",
			args: [_planetoidName, _semiMajorAxis.ToString(format: "F6", provider: CultureInfo.InvariantCulture), _eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture), _argumentPerihelionDeg.ToString(format: "F4", provider: CultureInfo.InvariantCulture)]);
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>The method currently returns the same string as <see cref="ToString"/>, but can be customized to include more specific information if needed.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Renders the 2D orbital plane diagram in the ScottPlot control.</summary>
	/// <remarks>The method clears the plot, then adds the Sun marker, the eight planet orbit ellipses, and the planetoid orbit ellipse. Axis limits are set to enclose the planetoid's orbit; planet orbits that extend beyond this range are still drawn and accessible by zooming out. Each orbit is drawn with a transparent fill and a colored border only.</remarks>
	private void RenderOrbitPlot()
	{
		formsPlotOrbits.Plot.Clear();
		// Configure title and axis labels.
		formsPlotOrbits.Plot.Title(text: $"Orbit visualization: {_planetoidName}");
		formsPlotOrbits.Plot.Axes.Bottom.Label.Text = "X (AU)";
		formsPlotOrbits.Plot.Axes.Left.Label.Text = "Y (AU)";
		formsPlotOrbits.Plot.Legend.IsVisible = true;
		formsPlotOrbits.Plot.Legend.Alignment = Alignment.UpperRight;
		// Draw the Sun as a yellow filled circle at the origin.
		// The Sun marker is added as a small circle plottable so it appears on top of the orbit lines.
		Ellipse sun = formsPlotOrbits.Plot.Add.Circle(xCenter: 0.0, yCenter: 0.0, radius: SunRadiusAu);
		sun.FillStyle.Color = Colors.Yellow;
		sun.LineStyle.Color = Colors.Orange;
		sun.LineStyle.Width = 1f;
		sun.LegendText = "Sun";
		// Draw the orbits of the eight solar system planets.
		// Planet orbits use a distinct color palette to keep them visually distinct from the planetoid.
		ScottPlot.Color[] planetColors =
		[
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Mercury – light blue-gray
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Venus – golden
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Earth – blue
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Mars – red-brown
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Jupiter – tan
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Saturn – pale gold
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Uranus – cyan
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Neptune – deep 
			/*
			new ScottPlot.Color(red: 0xB0, green: 0xB0, blue: 0xD0),   // Mercury – light blue-gray
			new ScottPlot.Color(red: 0xFF, green: 0xD7, blue: 0x00),   // Venus – golden
			new ScottPlot.Color(red: 0x40, green: 0x80, blue: 0xFF),   // Earth – blue
			new ScottPlot.Color(red: 0xCC, green: 0x44, blue: 0x22),   // Mars – red-brown
			new ScottPlot.Color(red: 0xE8, green: 0xBE, blue: 0x78),   // Jupiter – tan
			new ScottPlot.Color(red: 0xD4, green: 0xC0, blue: 0x70),   // Saturn – pale gold
			new ScottPlot.Color(red: 0x80, green: 0xE0, blue: 0xE8),   // Uranus – cyan
			new ScottPlot.Color(red: 0x30, green: 0x50, blue: 0xCC),   // Neptune – deep 
			 */
		];
		for (int i = 0; i < PlanetOrbits.Length; i++)
		{
			(string? name, double a, double e, double omega) = PlanetOrbits[i];
			AddOrbitEllipse(
				name: name,
				semiMajorAxis: a,
				eccentricity: e,
				argumentPerihelionDeg: omega,
				lineColor: planetColors[i],
				lineWidth: OrbitLineWidth);
		}
		// Draw the planetoid orbit in a distinct orange color so it stands out from the planet orbits.
		AddOrbitEllipse(
			name: _planetoidName,
			semiMajorAxis: _semiMajorAxis,
			eccentricity: _eccentricity,
			argumentPerihelionDeg: _argumentPerihelionDeg,
			lineColor: Colors.OrangeRed,
			lineWidth: OrbitLineWidth + 1f);
		// Scale the visible axes to the planetoid's orbit extent.
		// The aphelion distance (maximum distance from the Sun) defines the required axis range.
		// Planet orbits that extend beyond this limit remain drawn and can be explored by zooming out.
		double aphelion = _semiMajorAxis * (1.0 + _eccentricity);
		double axisMargin = aphelion * 0.15;
		double axisLimit = aphelion + axisMargin;
		formsPlotOrbits.Plot.Axes.SetLimits(left: -axisLimit, right: axisLimit, bottom: -axisLimit, top: axisLimit);
		// Ensure both axes use the same scale (1 AU on X equals 1 AU on Y) so orbits appear as true ellipses.
		formsPlotOrbits.Plot.Axes.SquareUnits();
		formsPlotOrbits.Refresh();
		// Update the status bar with key parameters.
		labelInformation.Text =
			$"{_planetoidName} — a = {_semiMajorAxis.ToString(format: "F4", provider: CultureInfo.InvariantCulture)} AU, " +
			$"e = {_eccentricity.ToString(format: "F6", provider: CultureInfo.InvariantCulture)}, " +
			$"ω = {_argumentPerihelionDeg.ToString(format: "F2", provider: CultureInfo.InvariantCulture)}°";
	}

	/// <summary>Adds a single orbit ellipse to the plot, correctly positioned with the Sun at the focal point.</summary>
	/// <param name="name">The legend label for this orbit.</param>
	/// <param name="semiMajorAxis">The semi-major axis of the orbit in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion in degrees (rotation angle, 0° = east).</param>
	/// <param name="lineColor">The color used to draw the orbit ellipse border.</param>
	/// <param name="lineWidth">The width of the orbit ellipse border line in pixels.</param>
	/// <remarks><para>In the Keplerian two-body problem the Sun occupies one focus of the ellipse. To place the Sun at the origin the ellipse center must be displaced from the Sun by the linear eccentricity <c>c = a·e</c> in the direction opposite to perihelion.</para>
	/// <para>With the perihelion direction at angle <c>ω</c> from east, the center offset is: <c>cx = −a·e·cos(ω)</c>, <c>cy = −a·e·sin(ω)</c>.</para>
	/// <para>ScottPlot's <c>Ellipse.Rotation</c> property takes a <see cref="ScottPlot.Angle"/> value measured counter-clockwise from the positive X-axis (east), which matches the convention used here for the argument of perihelion.</para>
	/// <para>The ellipse fill is left transparent; only the border is colored so orbit lines remain visually distinct and do not occlude one another.</para></remarks>
	private void AddOrbitEllipse(
		string name,
		double semiMajorAxis,
		double eccentricity,
		double argumentPerihelionDeg,
		ScottPlot.Color lineColor,
		float lineWidth)
	{
		// Guard against degenerate or invalid orbital parameters.
		if (semiMajorAxis <= 0.0 || eccentricity < 0.0 || eccentricity >= 1.0)
		{
			logger.Warn(message: "Skipping orbit ellipse for '{0}': invalid parameters (a={1}, e={2}).", args: [name, semiMajorAxis, eccentricity]);
			return;
		}
		double b = semiMajorAxis * Math.Sqrt(d: 1.0 - (eccentricity * eccentricity));
		double c = semiMajorAxis * eccentricity;
		double omegaRad = argumentPerihelionDeg * Math.PI / 180.0;
		// Center of the ellipse displaced from the Sun (at origin) such that the Sun lies at the focus.
		double cx = -c * Math.Cos(d: omegaRad);
		double cy = -c * Math.Sin(a: omegaRad);
		Ellipse ellipse = formsPlotOrbits.Plot.Add.Ellipse(xCenter: cx, yCenter: cy, radiusX: semiMajorAxis, radiusY: b);
		// Rotation in degrees, counter-clockwise from east (positive X-axis).
		ellipse.Rotation = Angle.FromDegrees(argumentPerihelionDeg);
		// Transparent fill: only the orbit border is drawn.
		ellipse.FillStyle.Color = Colors.Transparent;
		ellipse.LineStyle.Color = lineColor;
		ellipse.LineStyle.Width = lineWidth;
		ellipse.LegendText = name;
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the OrbitsForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and renders the orbital diagram when the form is first shown.</remarks>
	private void OrbitsForm_Load(object? sender, EventArgs e)
	{
		// Clear the status bar before rendering to ensure any previous messages are removed.
		ClearStatusBar(label: labelInformation);
		// Render the orbital diagram, with error handling to catch and log any exceptions that may occur during the rendering process.
		try
		{
			// The rendering process involves calculating the positions and shapes of the orbit ellipses based on the provided orbital parameters, and then drawing them on the ScottPlot control.
			RenderOrbitPlot();
		}
		catch (Exception ex)
		{
			// Log the error with details about the planetoid and the exception message for troubleshooting.
			logger.Error(message: $"Failed to render orbit plot for '{_planetoidName}': {ex}");
			ShowErrorMessage(message: $"An error occurred while rendering the orbital diagram: {ex.Message}");
		}
	}

	#endregion
}