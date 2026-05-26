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
public class AEIDiagram3DForm : BaseKryptonForm
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

	private readonly ToolStripContainer _container = new();
	private readonly KryptonStatusStrip _statusStrip = new();
	private readonly ToolStripStatusLabel _labelInformation = new();
	private readonly KryptonToolStrip _toolStripMain = new();
	private readonly KryptonToolStrip _toolStripScaling = new();
	private readonly KryptonToolStrip _toolStripProgress = new();
	private readonly ToolStripButton _buttonStartPause = new();
	private readonly ToolStripButton _buttonCancel = new();
	private readonly ToolStripButton _buttonLive = new();
	private readonly ToolStripButton _buttonLog = new();
	private readonly KryptonProgressBarToolStripItem _progressBar = new();
	private readonly ToolStripNumericUpDown _scaleX = new();
	private readonly ToolStripNumericUpDown _scaleY = new();
	private readonly ToolStripNumericUpDown _scaleZ = new();
	private readonly KryptonPanel _panelMain = new();
	private readonly Panel _panelGl = new();

	private readonly record struct AeiPoint(double A, double E, double I);
	private readonly record struct RenderPoint(float X, float Y, float Z);

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

	private void InitializeComponent()
	{
		SuspendLayout();
		ClientSize = new Size(980, 680);
		ControlBox = false;
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		MaximizeBox = false;
		MinimizeBox = false;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "a,e,i-Diagramm (3D)";
		Load += AEIDiagram3DForm_Load;
		FormClosing += AEIDiagram3DForm_FormClosing;

		_container.Dock = DockStyle.Fill;
		_container.ContentPanel.Controls.Add(_panelMain);
		_container.TopToolStripPanel.Controls.Add(_toolStripMain);
		_container.TopToolStripPanel.Controls.Add(_toolStripScaling);
		_container.TopToolStripPanel.Controls.Add(_toolStripProgress);
		_container.BottomToolStripPanel.Controls.Add(_statusStrip);

		_panelMain.Dock = DockStyle.Fill;
		_panelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		_panelMain.Controls.Add(_panelGl);
		_panelGl.Dock = DockStyle.Fill;
		_panelGl.BackColor = Color.Black;

		_statusStrip.Dock = DockStyle.None;
		_statusStrip.Items.AddRange([_labelInformation]);
		_labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;

		_buttonStartPause.Text = "&Start";
		_buttonStartPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
		_buttonStartPause.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonStartPause.Click += ToolStripButtonStartPause_Click;
		_buttonCancel.Text = "&Cancel";
		_buttonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
		_buttonCancel.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonCancel.Click += ToolStripButtonCancel_Click;
		_buttonLive.CheckOnClick = true;
		_buttonLive.Checked = true;
		_buttonLive.Text = "On";
		_buttonLive.Image = FatcowIcons16px.fatcow_tick_circle_frame_16px;
		_buttonLive.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonLive.CheckedChanged += ToolStripButtonLive_CheckedChanged;
		_buttonLog.CheckOnClick = true;
		_buttonLog.Text = "&Log scale";
		_buttonLog.Image = FatcowIcons16px.fatcow_chart_curve_16px;
		_buttonLog.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonLog.CheckedChanged += ToolStripButtonLog_CheckedChanged;

		_toolStripMain.Dock = DockStyle.None;
		_toolStripMain.Location = new Point(0, 0);
		_toolStripMain.Stretch = true;
		_toolStripMain.Items.AddRange([_buttonStartPause, _buttonCancel, new ToolStripSeparator(), new ToolStripLabel("Live"), _buttonLive, new ToolStripSeparator(), _buttonLog]);

		ConfigureScaleControl(_scaleX, 1.0m);
		ConfigureScaleControl(_scaleY, 1.0m);
		ConfigureScaleControl(_scaleZ, 1.0m);
		_toolStripScaling.Dock = DockStyle.None;
		_toolStripScaling.Location = new Point(0, 25);
		_toolStripScaling.Stretch = true;
		_toolStripScaling.Items.AddRange([new ToolStripLabel("Scale"), new ToolStripSeparator(), new ToolStripLabel("X(a)"), _scaleX, new ToolStripSeparator(), new ToolStripLabel("Y(e)"), _scaleY, new ToolStripSeparator(), new ToolStripLabel("Z(i)"), _scaleZ]);

		_progressBar.AutoSize = false;
		_progressBar.Size = new Size(760, 19);
		_progressBar.Values.Text = "0%";
		_toolStripProgress.Dock = DockStyle.None;
		_toolStripProgress.Location = new Point(0, 50);
		_toolStripProgress.Stretch = true;
		_toolStripProgress.Items.AddRange([new ToolStripLabel("Progress"), _progressBar]);

		Controls.Add(_container);
		ResumeLayout(performLayout: false);
	}

	private void ConfigureScaleControl(ToolStripNumericUpDown control, decimal value)
	{
		control.Minimum = 0.1m;
		control.Maximum = 25m;
		control.Increment = 0.1m;
		control.DecimalPlaces = 1;
		control.Value = value;
		control.AutoSize = false;
		control.Size = new Size(70, 22);
		control.ValueChanged += ToolStripNumericScale_ValueChanged;
	}

	private void CreateGlControl()
	{
		GLControlSettings settings = new() { API = ContextAPI.OpenGL, Profile = ContextProfile.Any, APIVersion = new Version(2, 1) };
		_glControl = new GLControl(glControlSettings: settings) { Dock = DockStyle.Fill };
		_glControl.Paint += GlControl_Paint;
		_glControl.Resize += GlControl_Resize;
		_glControl.MouseDown += GlControl_MouseDown;
		_glControl.MouseUp += GlControl_MouseUp;
		_glControl.MouseMove += GlControl_MouseMove;
		_glControl.MouseWheel += GlControl_MouseWheel;
		_panelGl.Controls.Add(_glControl);
	}

	private static bool TryParseValue(string line, int start, int len, out double value)
	{
		value = default;
		return line.Length >= start + len && double.TryParse(line.Substring(start, len).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
	}

	private static bool TryParseAeiPoint(string line, out AeiPoint point)
	{
		point = default;
		if (!TryParseValue(line, 92, 11, out double a) || !TryParseValue(line, 70, 9, out double e) || !TryParseValue(line, 59, 9, out double i))
		{
			return false;
		}
		if (!double.IsFinite(a) || !double.IsFinite(e) || !double.IsFinite(i) || a < 0 || e < 0 || i < 0)
		{
			return false;
		}
		point = new AeiPoint(a, e, i);
		return true;
	}

	private List<AeiPoint> BuildPointData(bool live, IProgress<int> progress, IProgress<List<AeiPoint>> liveResults, CancellationToken token)
	{
		List<AeiPoint> points = [];
		int total = _planetoids.Count;
		int pInterval = Math.Max(1, total / 100);
		int lInterval = Math.Max(1, total / 25);
		for (int i = 0; i < total; i++)
		{
			token.ThrowIfCancellationRequested();
			_pauseGate.Wait(token);
			if (TryParseAeiPoint(_planetoids[i], out AeiPoint point))
			{
				points.Add(point);
			}
			int processed = i + 1;
			if (processed % pInterval == 0 || processed == total)
			{
				progress.Report(processed * 100 / Math.Max(1, total));
			}
			if (live && (processed % lInterval == 0 || processed == total))
			{
				liveResults.Report([.. points]);
			}
		}
		return points;
	}

	private void UpdateProgress(int percent)
	{
		int value = Math.Clamp(percent, 0, 100);
		_progressBar.Value = value;
		_progressBar.Values.Text = $"{value}%";
		_progressBar.Text = $"{value}%";
		if (IsHandleCreated)
		{
			TaskbarProgress.SetValue(Handle, (ulong)value, 100);
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
		List<(double A, double E, double I)> transformed = [];
		_excludedPoints = 0;
		for (int i = 0; i < _rawPoints.Count; i++)
		{
			double a = _rawPoints[i].A;
			double e = _rawPoints[i].E;
			double inc = _rawPoints[i].I;
			if (log)
			{
				if (a <= 0 || e <= 0 || inc <= 0)
				{
					_excludedPoints++;
					continue;
				}
				a = Math.Log10(a);
				e = Math.Log10(e);
				inc = Math.Log10(inc);
			}
			transformed.Add((a, e, inc));
		}
		double maxA = transformed.Count == 0 ? 1 : Math.Max(1E-9, transformed.Max(static v => v.A));
		double maxE = transformed.Count == 0 ? 1 : Math.Max(1E-9, transformed.Max(static v => v.E));
		double maxI = transformed.Count == 0 ? 1 : Math.Max(1E-9, transformed.Max(static v => v.I));
		const float axisBase = 10f;
		_renderPoints = transformed.Select(v => new RenderPoint((float)(v.A / maxA) * axisBase * sx, (float)(v.E / maxE) * axisBase * sy, (float)(v.I / maxI) * axisBase * sz)).ToList();
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
		GL.Viewport(0, 0, w, h);
		GL.MatrixMode(MatrixMode.Projection);
		GL.LoadIdentity();
		double aspect = (double)w / h;
		double fovY = 45.0 * Math.PI / 180.0;
		double f = 1.0 / Math.Tan(fovY / 2.0);
		double[] p = [f / aspect, 0, 0, 0, 0, f, 0, 0, 0, 0, (1000.0 + 0.1) / (0.1 - 1000.0), -1.0, 0, 0, 2.0 * 1000.0 * 0.1 / (0.1 - 1000.0), 0];
		GL.LoadMatrix(ref p[0]);
		GL.MatrixMode(MatrixMode.Modelview);
	}

	private void RenderScene()
	{
		if (!_glReady)
		{
			return;
		}
		_glControl.MakeCurrent();
		GL.ClearColor(0.04f, 0.04f, 0.08f, 1f);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		GL.Enable(EnableCap.DepthTest);
		SetupProjection();
		GL.LoadIdentity();
		GL.Translate(_panX, _panY, -_zoom);
		GL.Rotate(_pitch, 1f, 0f, 0f);
		GL.Rotate(_yaw, 0f, 1f, 0f);

		float ax = 10f * (float)_scaleX.Value;
		float ay = 10f * (float)_scaleY.Value;
		float az = 10f * (float)_scaleZ.Value;
		GL.LineWidth(2f);
		GL.Begin(PrimitiveType.Lines);
		GL.Color3(1f, .2f, .2f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(ax, 0f, 0f);
		GL.Color3(.2f, 1f, .2f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(0f, ay, 0f);
		GL.Color3(.3f, .6f, 1f); GL.Vertex3(0f, 0f, 0f); GL.Vertex3(0f, 0f, az);
		GL.End();

		GL.PointSize(3.5f);
		GL.Color3(1f, .8f, .2f);
		GL.Begin(PrimitiveType.Points);
		for (int i = 0; i < _renderPoints.Count; i++)
		{
			GL.Vertex3(_renderPoints[i].X, _renderPoints[i].Y, _renderPoints[i].Z);
		}
		GL.End();
		_glControl.SwapBuffers();

		using Graphics g = _glControl.CreateGraphics();
		using Font font = new("Segoe UI", 9f, FontStyle.Bold);
		g.DrawString("X-axis: semi-major axis a [AU]", font, Brushes.IndianRed, 8, 8);
		g.DrawString("Y-axis: eccentricity e [-]", font, Brushes.LightGreen, 8, 26);
		g.DrawString("Z-axis: inclination i [°]", font, Brushes.LightSkyBlue, 8, 44);
		g.DrawString("Rotate: left mouse | Shift: right mouse | Zoom: wheel", font, Brushes.WhiteSmoke, 8, 64);
	}

	/// <summary>Handles the form load and initializes OpenGL state.</summary>
	private void AEIDiagram3DForm_Load(object? sender, EventArgs e)
	{
		try
		{
			_glControl.MakeCurrent();
			GL.Enable(EnableCap.DepthTest);
			_glReady = true;
			SetupProjection();
			_glControl.Invalidate();
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Failed to initialize a,e,i OpenGL context.");
			ShowErrorMessage($"Failed to initialize 3D rendering: {ex.Message}");
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
				_ = KryptonMessageBox.Show("No planetoid data available.", I18nStrings.InformationCaption, KryptonMessageBoxButtons.OK, KryptonMessageBoxIcon.Information);
				return;
			}
			_rawPoints = [];
			_renderPoints = [];
			UpdateProgress(0);
			UpdateStatusLabel();
			_isPaused = false;
			_pauseGate.Set();
			UpdateRunningState(true);
			_buttonLive.Enabled = false;

			_cts = new CancellationTokenSource();
			try
			{
				Progress<int> progress = new(UpdateProgress);
				Progress<List<AeiPoint>> live = new(points => { _rawPoints = points; RebuildRenderPointsAndInvalidate(); });
				List<AeiPoint> final = await Task.Run(() => BuildPointData(_buttonLive.Checked, progress, live, _cts.Token), _cts.Token);
				_rawPoints = final;
				RebuildRenderPointsAndInvalidate();
				UpdateProgress(100);
			}
			catch (OperationCanceledException)
			{
				_labelInformation.Text = "Generation canceled. Press Start to run again.";
			}
			finally
			{
				_cts?.Dispose();
				_cts = null;
				_isPaused = false;
				_pauseGate.Set();
				_buttonLive.Enabled = true;
				UpdateRunningState(false);
			}
			return;
		}

		_isPaused = !_isPaused;
		if (_isPaused) _pauseGate.Reset(); else _pauseGate.Set();
		UpdateRunningState(true);
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
	private void GlControl_Paint(object? sender, PaintEventArgs e) => RenderScene();

	/// <summary>Handles GL resize events and updates the projection.</summary>
	private void GlControl_Resize(object? sender, EventArgs e)
	{
		if (!_glReady) return;
		_glControl.MakeCurrent();
		SetupProjection();
		_glControl.Invalidate();
	}

	/// <summary>Handles GL mouse down events and starts camera interaction.</summary>
	private void GlControl_MouseDown(object? sender, MouseEventArgs e)
	{
		_lastMousePos = e.Location;
		if (e.Button == MouseButtons.Left) _leftDown = true;
		if (e.Button == MouseButtons.Right) _rightDown = true;
	}

	/// <summary>Handles GL mouse up events and ends camera interaction.</summary>
	private void GlControl_MouseUp(object? sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left) _leftDown = false;
		if (e.Button == MouseButtons.Right) _rightDown = false;
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
			_pitch = Math.Clamp(_pitch + (dy * 0.5f), -89f, 89f);
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
		_zoom = Math.Clamp(_zoom - (e.Delta * 0.02f), 2f, 140f);
		_glControl.Invalidate();
	}
}
