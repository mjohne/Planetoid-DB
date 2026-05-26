// This file contains the implementation of the AEI3DDiagramForm,
// which displays a 3D diagram of all planetoid orbital elements:
// X-axis: Semi-major axis (a), Y-axis: Eccentricity (e), Z-axis: Inclination (i)
using NLog;

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a 3D diagram of orbital elements (a, e, i) for all known planetoids.</summary>
/// <remarks>
/// <para>The form renders a 3D scatter plot with axes representing:</para>
/// <list type="bullet">
/// <item><description>X-axis: Semi-major axis (a) in AU</description></item>
/// <item><description>Y-axis: Eccentricity (e), dimensionless (0-1)</description></item>
/// <item><description>Z-axis: Inclination (i) in degrees (0-180)</description></item>
/// </list>
/// <para>Interaction: left-drag to rotate the view, right-drag to pan, scroll wheel to zoom in/out.</para>
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AEI3DDiagramForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>List of raw planetoid database entries.</summary>
	private readonly List<string> _planetoidsDatabase;

	/// <summary>Parsed orbital elements (a, e, i) for rendering.</summary>
	private readonly List<(float a, float e, float i)> _orbitalElements = [];

	// ---- Camera state ----

	/// <summary>Horizontal rotation angle of the camera in degrees.</summary>
	private float _yaw = 25f;

	/// <summary>Vertical rotation angle of the camera in degrees.</summary>
	private float _pitch = 20f;

	/// <summary>Camera distance from the scene origin (zoom level).</summary>
	private float _zoom = 10f;

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

	/// <summary>The OpenGL rendering control.</summary>
	private GLControl? _glControl;

	// ---- Processing state ----

	/// <summary>Whether the processing operation is running.</summary>
	private bool _isProcessing;

	/// <summary>Whether the processing operation has been cancelled.</summary>
	private bool _isCancelled;

	/// <summary>Whether live updates are enabled during processing.</summary>
	private bool _liveUpdate = true;

	/// <summary>Whether logarithmic scaling is enabled for axes.</summary>
	private bool _logarithmicScale;

	/// <summary>Current progress percentage (0-100).</summary>
	private int _progressPercent;

	// ---- Axis scaling factors ----

	/// <summary>X-axis (semi-major axis) scale factor.</summary>
	private float _scaleX = 1.0f;

	/// <summary>Y-axis (eccentricity) scale factor.</summary>
	private float _scaleY = 1.0f;

	/// <summary>Z-axis (inclination) scale factor.</summary>
	private float _scaleZ = 1.0f;

	/// <summary>Initializes a new instance of the <see cref="AEI3DDiagramForm"/> class.</summary>
	/// <param name="planetoidsDatabase">The list of raw planetoid database entries.</param>
	public AEI3DDiagramForm(List<string> planetoidsDatabase)
	{
		InitializeComponent();
		_planetoidsDatabase = planetoidsDatabase;

		// Set form title
		Text = "a,e,i-Diagram";

		// Configure form properties
		StartPosition = FormStartPosition.CenterScreen;
		Size = new Size(width: 1024, height: 768);
		MinimumSize = new Size(width: 800, height: 600);
	}

	/// <summary>Handles the Load event of the form.</summary>
	private void AEI3DDiagramForm_Load(object sender, EventArgs e)
	{
		logger.Info(message: "AEI3DDiagramForm loaded");
		CreateGlControl();
	}

	/// <summary>Handles the FormClosing event of the form.</summary>
	private void AEI3DDiagramForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		logger.Info(message: "AEI3DDiagramForm closing");
		_glControl?.Dispose();
	}

	/// <summary>Creates and initializes the OpenGL rendering control.</summary>
	private void CreateGlControl()
	{
		// Create GLControlSettings with OpenGL 2.1 compatibility profile (immediate mode)
		GLControlSettings settings = new()
		{
			API = ContextAPI.OpenGL,
			Profile = ContextProfile.Any,
			APIVersion = new Version(major: 2, minor: 1),
		};

		// Create the GLControl with the specified settings
		_glControl = new GLControl(settings)
		{
			Dock = DockStyle.Fill
		};

		// Subscribe to events
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.MouseDown += GlControl_MouseDown;
		_glControl.MouseUp += GlControl_MouseUp;
		_glControl.MouseMove += GlControl_MouseMove;
		_glControl.MouseWheel += GlControl_MouseWheel;

		// Add the control to the panel
		panelGlControl.Controls.Add(_glControl);

		// Mark OpenGL as ready
		_glReady = true;

		logger.Info(message: "GLControl created and initialized");
	}

	/// <summary>Handles the Paint event of the GLControl.</summary>
	private void GlControl_Paint(object? sender, EventArgs e)
	{
		if (!_glReady || _glControl == null)
		{
			return;
		}

		_glControl.MakeCurrent();
		RenderScene();
		_glControl.SwapBuffers();
	}

	/// <summary>Renders the 3D scene.</summary>
	private void RenderScene()
	{
		// Clear the color and depth buffers
		GL.Clear(mask: ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		// Set up the projection matrix
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		float aspect = _glControl != null && _glControl.Height > 0 ? (float)_glControl.Width / _glControl.Height : 1.0f;
		GL.Ortho(left: -_zoom * aspect, right: _zoom * aspect, bottom: -_zoom, top: _zoom, zNear: -100, zFar: 100);

		// Set up the modelview matrix
		GL.MatrixMode(mode: MatrixMode.Modelview);
		GL.LoadIdentity();

		// Apply camera transformations
		GL.Translate(x: _panX, y: _panY, z: 0);
		GL.Rotate(angle: _pitch, x: 1, y: 0, z: 0);
		GL.Rotate(angle: _yaw, x: 0, y: 1, z: 0);

		// Draw the axes
		DrawAxes();

		// Draw the orbital element points
		DrawOrbitalElements();
	}

	/// <summary>Draws the coordinate axes.</summary>
	private void DrawAxes()
	{
		GL.Begin(mode: PrimitiveType.Lines);

		// X-axis (red) - Semi-major axis
		GL.Color3(red: 1.0f, green: 0.0f, blue: 0.0f);
		GL.Vertex3(x: 0, y: 0, z: 0);
		GL.Vertex3(x: 5, y: 0, z: 0);

		// Y-axis (green) - Eccentricity
		GL.Color3(red: 0.0f, green: 1.0f, blue: 0.0f);
		GL.Vertex3(x: 0, y: 0, z: 0);
		GL.Vertex3(x: 0, y: 5, z: 0);

		// Z-axis (blue) - Inclination
		GL.Color3(red: 0.0f, green: 0.0f, blue: 1.0f);
		GL.Vertex3(x: 0, y: 0, z: 0);
		GL.Vertex3(x: 0, y: 0, z: 5);

		GL.End();
	}

	/// <summary>Draws the orbital element points.</summary>
	private void DrawOrbitalElements()
	{
		if (_orbitalElements.Count == 0)
		{
			return;
		}

		GL.PointSize(size: 2.0f);
		GL.Begin(mode: PrimitiveType.Points);
		GL.Color3(red: 1.0f, green: 1.0f, blue: 1.0f);

		foreach ((float a, float e, float i) in _orbitalElements)
		{
			// Apply scaling factors
			float x = ApplyScaling(value: a, isLogarithmic: _logarithmicScale) * _scaleX;
			float y = ApplyScaling(value: e, isLogarithmic: false) * _scaleY;
			float z = ApplyScaling(value: i, isLogarithmic: false) * _scaleZ;

			GL.Vertex3(x: x, y: y, z: z);
		}

		GL.End();
	}

	/// <summary>Applies scaling to a value (linear or logarithmic).</summary>
	/// <param name="value">The value to scale.</param>
	/// <param name="isLogarithmic">Whether to apply logarithmic scaling.</param>
	/// <returns>The scaled value.</returns>
	private static float ApplyScaling(float value, bool isLogarithmic)
	{
		if (isLogarithmic && value > 0)
		{
			return MathF.Log10(x: value);
		}
		return value;
	}

	/// <summary>Handles the Resize event of the GLControl.</summary>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		if (!_glReady || _glControl == null)
		{
			return;
		}

		_glControl.MakeCurrent();
		GL.Viewport(x: 0, y: 0, width: _glControl.Width, height: _glControl.Height);
		_glControl.Invalidate();
	}

	/// <summary>Handles the MouseDown event of the GLControl.</summary>
	private void GlControl_MouseDown(object? sender, MouseEventArgs e)
	{
		_lastMousePos = e.Location;
		if (e.Button == MouseButtons.Left)
		{
			_leftDown = true;
		}
		else if (e.Button == MouseButtons.Right)
		{
			_rightDown = true;
		}
	}

	/// <summary>Handles the MouseUp event of the GLControl.</summary>
	private void GlControl_MouseUp(object? sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			_leftDown = false;
		}
		else if (e.Button == MouseButtons.Right)
		{
			_rightDown = false;
		}
	}

	/// <summary>Handles the MouseMove event of the GLControl.</summary>
	private void GlControl_MouseMove(object? sender, MouseEventArgs e)
	{
		int dx = e.X - _lastMousePos.X;
		int dy = e.Y - _lastMousePos.Y;

		if (_leftDown)
		{
			// Rotate view
			_yaw += dx * 0.5f;
			_pitch += dy * 0.5f;
			_glControl?.Invalidate();
		}
		else if (_rightDown)
		{
			// Pan view
			_panX += dx * 0.02f;
			_panY -= dy * 0.02f;
			_glControl?.Invalidate();
		}

		_lastMousePos = e.Location;
	}

	/// <summary>Handles the MouseWheel event of the GLControl.</summary>
	private void GlControl_MouseWheel(object? sender, MouseEventArgs e)
	{
		// Zoom in/out
		_zoom -= e.Delta * 0.001f;
		_zoom = Math.Max(val1: 0.1f, val2: Math.Min(val1: _zoom, val2: 100f));
		_glControl?.Invalidate();
	}

	/// <summary>Handles the Click event of the Start/Pause button.</summary>
	private async void ButtonStartPause_Click(object sender, EventArgs e)
	{
		if (_isProcessing)
		{
			// Pause
			_isProcessing = false;
			toolStripButtonStartPause.Text = "Start";
			logger.Info(message: "Processing paused");
		}
		else
		{
			// Start or resume
			_isProcessing = true;
			_isCancelled = false;
			toolStripButtonStartPause.Text = "Pause";
			logger.Info(message: "Processing started");

			await ProcessPlanetoidData();
		}
	}

	/// <summary>Handles the Click event of the Cancel button.</summary>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		_isCancelled = true;
		_isProcessing = false;
		toolStripButtonStartPause.Text = "Start";
		logger.Info(message: "Processing cancelled");
	}

	/// <summary>Handles the CheckedChanged event of the Live Update checkbox.</summary>
	private void CheckBoxLiveUpdate_CheckedChanged(object sender, EventArgs e)
	{
		_liveUpdate = toolStripButtonLiveUpdate.Checked;
		logger.Info(message: $"Live update: {_liveUpdate}");
	}

	/// <summary>Handles the CheckedChanged event of the Logarithmic Scale checkbox.</summary>
	private void CheckBoxLogarithmicScale_CheckedChanged(object sender, EventArgs e)
	{
		_logarithmicScale = toolStripButtonLogarithmicScale.Checked;
		_glControl?.Invalidate();
		logger.Info(message: $"Logarithmic scale: {_logarithmicScale}");
	}

	/// <summary>Processes the planetoid data and populates the orbital elements list.</summary>
	private async Task ProcessPlanetoidData()
	{
		_orbitalElements.Clear();
		_progressPercent = 0;

		await Task.Run(action: () =>
		{
			int total = _planetoidsDatabase.Count;
			int processed = 0;

			IFormatProvider provider = CultureInfo.InvariantCulture;

			foreach (string rawLine in _planetoidsDatabase)
			{
				if (_isCancelled || !_isProcessing)
				{
					break;
				}

				// Parse the orbital elements using PlanetoidRecord
				PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: rawLine);

				// Parse semi-major axis (a), eccentricity (e), and inclination (i)
				if (double.TryParse(s: record.SemiMajorAxis, style: NumberStyles.Any, provider: provider, result: out double a) &&
					double.TryParse(s: record.OrbEcc, style: NumberStyles.Any, provider: provider, result: out double e) &&
					double.TryParse(s: record.Incl, style: NumberStyles.Any, provider: provider, result: out double i))
				{
					// Add to the list
					_orbitalElements.Add(item: ((float)a, (float)e, (float)i));
				}

				processed++;
				int newPercent = (int)(processed * 100.0 / total);
				if (newPercent != _progressPercent)
				{
					_progressPercent = newPercent;

					// Update UI on the main thread
					Invoke(() =>
					{
						toolStripProgressBar.Value = _progressPercent;
						toolStripLabelProgress.Text = $"{_progressPercent}%";

						if (_liveUpdate)
						{
							_glControl?.Invalidate();
						}
					});
				}
			}
		});

		// Final update
		_isProcessing = false;
		toolStripButtonStartPause.Text = "Start";
		_glControl?.Invalidate();

		logger.Info(message: $"Processing complete. {_orbitalElements.Count} orbital elements loaded.");
		SetStatusBar(label: labelInformation, text: $"Processing complete. {_orbitalElements.Count} planetoids displayed.");
	}

	/// <summary>Handles the ValueChanged event of the X-axis scale numeric up-down.</summary>
	private void NumericUpDownScaleX_ValueChanged(object sender, EventArgs e)
	{
		_scaleX = (float)toolStripNumericUpDownScaleX.Value;
		_glControl?.Invalidate();
	}

	/// <summary>Handles the ValueChanged event of the Y-axis scale numeric up-down.</summary>
	private void NumericUpDownScaleY_ValueChanged(object sender, EventArgs e)
	{
		_scaleY = (float)toolStripNumericUpDownScaleY.Value;
		_glControl?.Invalidate();
	}

	/// <summary>Handles the ValueChanged event of the Z-axis scale numeric up-down.</summary>
	private void NumericUpDownScaleZ_ValueChanged(object sender, EventArgs e)
	{
		_scaleZ = (float)toolStripNumericUpDownScaleZ.Value;
		_glControl?.Invalidate();
	}

	/// <summary>Gets the debugger display string.</summary>
	private string GetDebuggerDisplay() => ToString() ?? nameof(AEI3DDiagramForm);
}
