using Krypton.Toolkit;

using NLog;

using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using Planetoid_DB.Helpers;
using Planetoid_DB.Resources;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Displays a 3D a,e,i diagram for all known planetoids.</summary>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AEIDiagram3DForm : BaseKryptonForm
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for informational text in this form.</summary>
	protected override ToolStripStatusLabel? StatusLabel => _labelInformation;

	private readonly IReadOnlyList<string> _planetoids;
	private List<AeiPoint> _rawPoints = [];
	private List<RenderPoint> _renderPoints = [];
	private CancellationTokenSource? _cts;
	private readonly ManualResetEventSlim _pauseGate = new(initialState: true);
	private bool _isPaused;

	private bool _glReady;
	private GLControl _glControl = null!;
	private float _yaw = 25f;
	private float _pitch = 20f;
	private float _zoom = 28f;
	private float _panX;
	private float _panY;
	private Point _lastMousePos;
	private bool _leftDown;
	private bool _rightDown;
	private int _excludedPoints;

	private readonly record struct AeiPoint(double A, double E, double I);
	private readonly record struct RenderPoint(float X, float Y, float Z);
	private readonly Font _overlayFont = new(familyName: "Segoe UI", emSize: 9f, style: FontStyle.Bold);

	/// <summary>Initializes a new instance of the <see cref="AEIDiagram3DForm"/> class.</summary>
	/// <param name="planetoids">The planetoid records from the database.</param>
	public AEIDiagram3DForm(IReadOnlyList<string> planetoids)
	{
		_planetoids = planetoids;
		InitializeComponent();
		CreateGlControl();
		UpdateProgress(percent: 0);
		UpdateRunningState(isRunning: false);
		UpdateStatusLabel();
	}

	private string GetDebuggerDisplay() => ToString();

	private void CreateGlControl()
	{
		GLControlSettings settings = new() { API = ContextAPI.OpenGL, Profile = ContextProfile.Any, APIVersion = new Version(2, 1) };
		_glControl = new GLControl(glControlSettings: settings)
		{
			Dock = DockStyle.Fill,
			AccessibleName = "Orbital diagram 3D OpenGL canvas",
			AccessibleDescription = "Displays the three-dimensional a, e, i point cloud with camera controls.",
			AccessibleRole = AccessibleRole.Graphic
		};
		_glControl.Enter += Control_Enter;
		_glControl.Leave += Control_Leave;
		_glControl.MouseEnter += Control_Enter;
		_glControl.MouseLeave += Control_Leave;
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.MouseDown += GlControl_MouseDown;
		_glControl.MouseUp += GlControl_MouseUp;
		_glControl.MouseMove += GlControl_MouseMove;
		_glControl.MouseWheel += GlControl_MouseWheel;
		_panelGl.Controls.Add(value: _glControl);
	}

	private static bool TryParseValue(string line, int start, int len, out double value)
	{
		value = default;
		return line.Length >= start + len && double.TryParse(s: line.Substring(startIndex: start, length: len).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
	}

	private static bool TryParseAeiPoint(string line, out AeiPoint point)
	{
		point = default;
		if (!TryParseValue(line: line, start: 92, len: 11, value: out double a) || !TryParseValue(line: line, start: 70, len: 9, value: out double e) || !TryParseValue(line: line, start: 59, len: 9, value: out double i))
		{
			return false;
		}
		if (!double.IsFinite(d: a) || !double.IsFinite(d: e) || !double.IsFinite(d: i) || a < 0 || e < 0 || i < 0)
		{
			return false;
		}
		point = new AeiPoint(A: a, E: e, I: i);
		return true;
	}

	private List<AeiPoint> BuildPointData(bool live, IProgress<int> progress, IProgress<List<AeiPoint>> liveResults, CancellationToken token)
	{
		List<AeiPoint> points = [];
		int total = _planetoids.Count;
		int pInterval = Math.Max(1, total / 100);
		int lInterval = Math.Max(1, total / 25);
		int lastLiveCount = 0;
		for (int i = 0; i < total; i++)
		{
			token.ThrowIfCancellationRequested();
			_pauseGate.Wait(cancellationToken: token);
			if (TryParseAeiPoint(line: _planetoids[index: i], point: out AeiPoint point))
			{
				points.Add(item: point);
			}
			int processed = i + 1;
			if (processed % pInterval == 0 || processed == total)
			{
				progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
			}
			if (live && (processed % lInterval == 0 || processed == total))
			{
				List<AeiPoint> batch = points.GetRange(index: lastLiveCount, count: points.Count - lastLiveCount);
				lastLiveCount = points.Count;
				liveResults.Report(value: batch);
			}
		}
		return points;
	}

	private void UpdateProgress(int percent)
	{
		int value = Math.Clamp(value: percent, min: 0, max: 100);
		_progressBar.Value = value;
		_progressBar.Values.Text = $"{value}%";
		_progressBar.Text = $"{value}%";
		if (IsHandleCreated)
		{
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)value, progressMax: 100);
		}
	}

	private void UpdateRunningState(bool isRunning)
	{
		_buttonCancel.Enabled = isRunning;
		if (!isRunning)
		{
			_buttonStartPause.Text = "&Start";
			_buttonStartPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
		}
		else if (_isPaused)
		{
			_buttonStartPause.Text = "&Resume";
			_buttonStartPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
		}
		else
		{
			_buttonStartPause.Text = "&Pause";
			_buttonStartPause.Image = FatcowIcons16px.fatcow_control_pause_blue_16px;
		}
	}

	private void RebuildRenderPointsAndInvalidate()
	{
		bool log = _buttonLog.Checked;
		float sx = (float)_scaleX.Value;
		float sy = (float)_scaleY.Value;
		float sz = (float)_scaleZ.Value;
		double minA = double.PositiveInfinity;
		double minE = double.PositiveInfinity;
		double minI = double.PositiveInfinity;
		double maxA = double.NegativeInfinity;
		double maxE = double.NegativeInfinity;
		double maxI = double.NegativeInfinity;
		int validCount = 0;
		_excludedPoints = 0;
		for (int i = 0; i < _rawPoints.Count; i++)
		{
			double a = _rawPoints[index: i].A;
			double e = _rawPoints[index: i].E;
			double inc = _rawPoints[index: i].I;
			if (log)
			{
				if (a <= 0 || e <= 0 || inc <= 0)
				{
					_excludedPoints++;
					continue;
				}
				a = Math.Log10(d: a);
				e = Math.Log10(d: e);
				inc = Math.Log10(d: inc);
			}
			minA = Math.Min(val1: minA, val2: a);
			minE = Math.Min(val1: minE, val2: e);
			minI = Math.Min(val1: minI, val2: inc);
			maxA = Math.Max(val1: maxA, val2: a);
			maxE = Math.Max(val1: maxE, val2: e);
			maxI = Math.Max(val1: maxI, val2: inc);
			validCount++;
		}

		if (validCount == 0)
		{
			_renderPoints = [];
			UpdateStatusLabel();
			if (_glReady)
			{
				_glControl.Invalidate();
			}
			return;
		}

		double rangeA = Math.Max(val1: 1E-9, val2: maxA - minA);
		double rangeE = Math.Max(val1: 1E-9, val2: maxE - minE);
		double rangeI = Math.Max(val1: 1E-9, val2: maxI - minI);
		const float axisBase = 10f;
		List<RenderPoint> renderPoints = new(capacity: validCount);
		for (int i = 0; i < _rawPoints.Count; i++)
		{
			double a = _rawPoints[index: i].A;
			double e = _rawPoints[index: i].E;
			double inc = _rawPoints[index: i].I;
			if (log)
			{
				if (a <= 0 || e <= 0 || inc <= 0)
				{
					continue;
				}
				a = Math.Log10(d: a);
				e = Math.Log10(d: e);
				inc = Math.Log10(d: inc);
			}
			renderPoints.Add(item: new RenderPoint(X: (float)((a - minA) / rangeA) * axisBase * sx, Y: (float)((e - minE) / rangeE) * axisBase * sy, Z: (float)((inc - minI) / rangeI) * axisBase * sz));
		}
		_renderPoints = renderPoints;
		UpdateStatusLabel();
		if (_glReady)
		{
			_glControl.Invalidate();
		}
	}

	private void UpdateStatusLabel()
	{
		_labelInformation.Text = $"Points: {_renderPoints.Count:N0} / Raw: {_rawPoints.Count:N0} | Excluded: {_excludedPoints:N0} | Axes: X=a [AU], Y=e [-], Z=i [°]";
	}

	private void SetupProjection()
	{
		int w = _glControl.Width;
		int h = Math.Max(1, _glControl.Height);
		GL.Viewport(x: 0, y: 0, width: w, height: h);
		GL.MatrixMode(mode: MatrixMode.Projection);
		GL.LoadIdentity();
		double aspect = (double)w / h;
		double fovY = 45.0 * Math.PI / 180.0;
		double f = 1.0 / Math.Tan(a: fovY / 2.0);
		double[] p = [f / aspect, 0, 0, 0, 0, f, 0, 0, 0, 0, (1000.0 + 0.1) / (0.1 - 1000.0), -1.0, 0, 0, 2.0 * 1000.0 * 0.1 / (0.1 - 1000.0), 0];
		GL.LoadMatrix(m: ref p[0]);
		GL.MatrixMode(mode: MatrixMode.Modelview);
	}

	private void RenderScene(Graphics? overlayGraphics)
	{
		if (!_glReady)
		{
			return;
		}
		_glControl.MakeCurrent();
		GL.ClearColor(red: 0.04f, green: 0.04f, blue: 0.08f, alpha: 1f);
		GL.Clear(mask: ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		GL.Enable(cap: EnableCap.DepthTest);
		SetupProjection();
		GL.LoadIdentity();
		GL.Translate(x: _panX, y: _panY, z: -_zoom);
		GL.Rotate(angle: _pitch, x: 1f, y: 0f, z: 0f);
		GL.Rotate(angle: _yaw, x: 0f, y: 1f, z: 0f);

		float ax = 10f * (float)_scaleX.Value;
		float ay = 10f * (float)_scaleY.Value;
		float az = 10f * (float)_scaleZ.Value;
		GL.LineWidth(width: 2f);
		GL.Begin(mode: PrimitiveType.Lines);
		GL.Color3(red: 1f, green: .2f, blue: .2f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(ax, 0f, 0f);
		GL.Color3(red: .2f, green: 1f, blue: .2f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(0f, ay, 0f);
		GL.Color3(red: .3f, green: .6f, blue: 1f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(0f, 0f, az);
		GL.End();

		GL.PointSize(size: 1f);
		GL.Color3(red: 1f, green: .8f, blue: .2f);
		GL.Begin(mode: PrimitiveType.Points);
		for (int i = 0; i < _renderPoints.Count; i++)
		{
			GL.Vertex3(x: _renderPoints[index: i].X, y: _renderPoints[index: i].Y, z: _renderPoints[index: i].Z);
		}
		GL.End();
		_glControl.SwapBuffers();

		overlayGraphics?.DrawString(s: "X-axis: semi-major axis a [AU]", font: _overlayFont, brush: Brushes.IndianRed, x: 8, y: 8);
		overlayGraphics?.DrawString(s: "Y-axis: eccentricity e [-]", font: _overlayFont, brush: Brushes.LightGreen, x: 8, y: 26);
		overlayGraphics?.DrawString(s: "Z-axis: inclination i [°]", font: _overlayFont, brush: Brushes.LightSkyBlue, x: 8, y: 44);
		overlayGraphics?.DrawString(s: "Rotate: left mouse | Pan: right mouse | Zoom: wheel", font: _overlayFont, brush: Brushes.WhiteSmoke, x: 8, y: 64);
	}

	/// <summary>Handles the form load and initializes OpenGL state.</summary>
	private void AEIDiagram3DForm_Load(object? sender, EventArgs e)
	{
		try
		{
			_glControl.MakeCurrent();
			GL.Enable(cap: EnableCap.DepthTest);
			_glReady = true;
			SetupProjection();
			_glControl.Invalidate();
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Failed to initialize a,e,i OpenGL context.");
			ShowErrorMessage(message: $"Failed to initialize 3D rendering: {ex.Message}");
		}
	}

	/// <summary>Handles the form closing event and cancels running work.</summary>
	private void AEIDiagram3DForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		_cts?.Cancel();
		_pauseGate.Set();
	}

	/// <summary>Handles start/pause/resume button clicks.</summary>
	private async void ToolStripButtonStartPause_Click(object? sender, EventArgs e)
	{
		if (_cts is null)
		{
			if (_planetoids.Count == 0)
			{
				_ = KryptonMessageBox.Show(owner: this, text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
				return;
			}
			_rawPoints = [];
			_renderPoints = [];
			UpdateProgress(percent: 0);
			UpdateStatusLabel();
			_isPaused = false;
			_pauseGate.Set();
			UpdateRunningState(isRunning: true);
			_buttonLive.Enabled = false;

			_cts = new CancellationTokenSource();
			try
			{
				Progress<int> progress = new(handler: UpdateProgress);
				Progress<List<AeiPoint>> live = new(handler: batch => { _rawPoints.AddRange(collection: batch); RebuildRenderPointsAndInvalidate(); });
				List<AeiPoint> final = await Task.Run(function: () => BuildPointData(live: _buttonLive.Checked, progress: progress, liveResults: live, token: _cts.Token), cancellationToken: _cts.Token);
				_rawPoints = final;
				RebuildRenderPointsAndInvalidate();
				UpdateProgress(percent: 100);
			}
			catch (OperationCanceledException)
			{
				if (!IsDisposed && !Disposing)
				{
					_labelInformation.Text = "Generation canceled. Press Start to run again.";
				}
			}
			finally
			{
				_cts?.Dispose();
				_cts = null;
				_isPaused = false;
				_pauseGate.Set();
				if (!IsDisposed && !Disposing)
				{
					_buttonLive.Enabled = true;
					UpdateRunningState(isRunning: false);
				}
			}
			return;
		}

		_isPaused = !_isPaused;
		if (_isPaused)
		{
			_pauseGate.Reset();
		}
		else
		{
			_pauseGate.Set();
		}
		UpdateRunningState(isRunning: true);
	}

	/// <summary>Handles cancel button clicks and requests cancellation.</summary>
	private void ToolStripButtonCancel_Click(object? sender, EventArgs e)
	{
		_cts?.Cancel();
		_pauseGate.Set();
	}

	/// <summary>Handles live-display toggle changes.</summary>
	private void ToolStripButtonLive_CheckedChanged(object? sender, EventArgs e)
	{
		_buttonLive.Text = _buttonLive.Checked ? "On" : "Off";
		_buttonLive.Image = _buttonLive.Checked ? FatcowIcons16px.fatcow_tick_circle_frame_16px : FatcowIcons16px.fatcow_cancel_16px;
	}

	/// <summary>Handles logarithmic-scale toggle changes and redraws the scene.</summary>
	private void ToolStripButtonLog_CheckedChanged(object? sender, EventArgs e) => RebuildRenderPointsAndInvalidate();

	/// <summary>Handles axis-scale value changes and redraws the scene immediately.</summary>
	private void ToolStripNumericScale_ValueChanged(object? sender, EventArgs e) => RebuildRenderPointsAndInvalidate();

	/// <summary>Handles GL paint events and renders the scene.</summary>
	private void GlControl_Paint(object? sender, PaintEventArgs e) => RenderScene(overlayGraphics: e.Graphics);

	/// <summary>Handles GL resize events and updates the projection.</summary>
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

	/// <summary>Handles GL mouse down events and starts camera interaction.</summary>
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

	/// <summary>Handles GL mouse up events and ends camera interaction.</summary>
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

	/// <summary>Handles GL mouse move events and rotates/shifts the camera.</summary>
	private void GlControl_MouseMove(object? sender, MouseEventArgs e)
	{
		int dx = e.X - _lastMousePos.X;
		int dy = e.Y - _lastMousePos.Y;
		_lastMousePos = e.Location;
		if (_leftDown)
		{
			_yaw += dx * 0.5f;
			_pitch = Math.Clamp(value: _pitch + (dy * 0.5f), min: -89f, max: 89f);
			_glControl.Invalidate();
		}
		else if (_rightDown)
		{
			_panX += dx * _zoom * 0.001f;
			_panY -= dy * _zoom * 0.001f;
			_glControl.Invalidate();
		}
	}

	/// <summary>Handles GL mouse wheel events and zooms the camera.</summary>
	private void GlControl_MouseWheel(object? sender, MouseEventArgs e)
	{
		_zoom = Math.Clamp(value: _zoom - (e.Delta * 0.02f), min: 2f, max: 140f);
		_glControl.Invalidate();
	}
}