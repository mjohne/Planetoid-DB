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
	private readonly Font _overlayFont = new("Segoe UI", 9f, FontStyle.Bold);

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

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_cts?.Dispose();
			_pauseGate.Dispose();
			_glControl.Dispose();
			_overlayFont.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		SuspendLayout();
		ClientSize = new Size(980, 680);
		ControlBox = false;
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		MaximizeBox = false;
		MinimizeBox = false;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "a,e,i Diagram (3D)";
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
		_labelInformation.AccessibleName = "Orbital diagram status information";
		_labelInformation.AccessibleDescription = "Shows point count, exclusions, and axis details.";
		_labelInformation.AccessibleRole = AccessibleRole.StaticText;
		_labelInformation.MouseEnter += Control_Enter;
		_labelInformation.MouseLeave += Control_Leave;

		_buttonStartPause.Text = "&Start";
		_buttonStartPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
		_buttonStartPause.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonStartPause.AccessibleName = "Start or pause generation";
		_buttonStartPause.AccessibleDescription = "Starts, pauses, or resumes point generation.";
		_buttonStartPause.AccessibleRole = AccessibleRole.PushButton;
		_buttonStartPause.MouseEnter += Control_Enter;
		_buttonStartPause.MouseLeave += Control_Leave;
		_buttonStartPause.Click += ToolStripButtonStartPause_Click;
		_buttonCancel.Text = "&Cancel";
		_buttonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
		_buttonCancel.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonCancel.AccessibleName = "Cancel generation";
		_buttonCancel.AccessibleDescription = "Cancels ongoing point generation.";
		_buttonCancel.AccessibleRole = AccessibleRole.PushButton;
		_buttonCancel.MouseEnter += Control_Enter;
		_buttonCancel.MouseLeave += Control_Leave;
		_buttonCancel.Click += ToolStripButtonCancel_Click;
		_buttonLive.CheckOnClick = true;
		_buttonLive.Checked = true;
		_buttonLive.Text = "On";
		_buttonLive.Image = FatcowIcons16px.fatcow_tick_circle_frame_16px;
		_buttonLive.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonLive.AccessibleName = "Live updates";
		_buttonLive.AccessibleDescription = "Toggles live rendering updates during generation.";
		_buttonLive.AccessibleRole = AccessibleRole.CheckButton;
		_buttonLive.MouseEnter += Control_Enter;
		_buttonLive.MouseLeave += Control_Leave;
		_buttonLive.CheckedChanged += ToolStripButtonLive_CheckedChanged;
		_buttonLog.CheckOnClick = true;
		_buttonLog.Text = "&Log scale";
		_buttonLog.Image = FatcowIcons16px.fatcow_chart_curve_16px;
		_buttonLog.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
		_buttonLog.AccessibleName = "Log scale";
		_buttonLog.AccessibleDescription = "Toggles logarithmic axis scaling.";
		_buttonLog.AccessibleRole = AccessibleRole.CheckButton;
		_buttonLog.MouseEnter += Control_Enter;
		_buttonLog.MouseLeave += Control_Leave;
		_buttonLog.CheckedChanged += ToolStripButtonLog_CheckedChanged;

		_toolStripMain.Dock = DockStyle.None;
		_toolStripMain.Location = new Point(0, 0);
		_toolStripMain.Stretch = true;
		_toolStripMain.AccessibleName = "Main controls";
		_toolStripMain.AccessibleDescription = "Controls for generation, cancellation, and display options.";
		_toolStripMain.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripMain.Enter += Control_Enter;
		_toolStripMain.Leave += Control_Leave;
		_toolStripMain.MouseEnter += Control_Enter;
		_toolStripMain.MouseLeave += Control_Leave;
		_toolStripMain.Items.AddRange([_buttonStartPause, _buttonCancel, new ToolStripSeparator(), new ToolStripLabel("Live"), _buttonLive, new ToolStripSeparator(), _buttonLog]);

		ConfigureScaleControl(_scaleX, 1.0m);
		ConfigureScaleControl(_scaleY, 1.0m);
		ConfigureScaleControl(_scaleZ, 1.0m);
		_toolStripScaling.Dock = DockStyle.None;
		_toolStripScaling.Location = new Point(0, 25);
		_toolStripScaling.Stretch = true;
		_toolStripScaling.AccessibleName = "Axis scaling controls";
		_toolStripScaling.AccessibleDescription = "Sets scaling factors for a, e, and i axes.";
		_toolStripScaling.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripScaling.Enter += Control_Enter;
		_toolStripScaling.Leave += Control_Leave;
		_toolStripScaling.MouseEnter += Control_Enter;
		_toolStripScaling.MouseLeave += Control_Leave;
		_toolStripScaling.Items.AddRange([new ToolStripLabel("Scale"), new ToolStripSeparator(), new ToolStripLabel("X(a)"), _scaleX, new ToolStripSeparator(), new ToolStripLabel("Y(e)"), _scaleY, new ToolStripSeparator(), new ToolStripLabel("Z(i)"), _scaleZ]);

		_progressBar.AutoSize = false;
		_progressBar.Size = new Size(760, 19);
		_progressBar.Values.Text = "0%";
		_progressBar.AccessibleName = "Generation progress";
		_progressBar.AccessibleDescription = "Shows generation progress percentage.";
		_progressBar.AccessibleRole = AccessibleRole.ProgressBar;
		_progressBar.MouseEnter += Control_Enter;
		_progressBar.MouseLeave += Control_Leave;
		_toolStripProgress.Dock = DockStyle.None;
		_toolStripProgress.Location = new Point(0, 50);
		_toolStripProgress.Stretch = true;
		_toolStripProgress.AccessibleName = "Progress display";
		_toolStripProgress.AccessibleDescription = "Displays generation progress.";
		_toolStripProgress.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripProgress.Enter += Control_Enter;
		_toolStripProgress.Leave += Control_Leave;
		_toolStripProgress.MouseEnter += Control_Enter;
		_toolStripProgress.MouseLeave += Control_Leave;
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
		control.AccessibleName = "Axis scale value";
		control.AccessibleDescription = "Sets axis scaling multiplier.";
		control.AccessibleRole = AccessibleRole.SpinButton;
		control.Enter += Control_Enter;
		control.Leave += Control_Leave;
		control.MouseEnter += Control_Enter;
		control.MouseLeave += Control_Leave;
		control.ValueChanged += ToolStripNumericScale_ValueChanged;
	}

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
		int lastLiveCount = 0;
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
				List<AeiPoint> batch = points.GetRange(lastLiveCount, points.Count - lastLiveCount);
				lastLiveCount = points.Count;
				liveResults.Report(batch);
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
			minA = Math.Min(minA, a);
			minE = Math.Min(minE, e);
			minI = Math.Min(minI, inc);
			maxA = Math.Max(maxA, a);
			maxE = Math.Max(maxE, e);
			maxI = Math.Max(maxI, inc);
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

		double rangeA = Math.Max(1E-9, maxA - minA);
		double rangeE = Math.Max(1E-9, maxE - minE);
		double rangeI = Math.Max(1E-9, maxI - minI);
		const float axisBase = 10f;
		List<RenderPoint> renderPoints = new(capacity: validCount);
		for (int i = 0; i < _rawPoints.Count; i++)
		{
			double a = _rawPoints[i].A;
			double e = _rawPoints[i].E;
			double inc = _rawPoints[i].I;
			if (log)
			{
				if (a <= 0 || e <= 0 || inc <= 0)
				{
					continue;
				}
				a = Math.Log10(a);
				e = Math.Log10(e);
				inc = Math.Log10(inc);
			}
			renderPoints.Add(new RenderPoint((float)((a - minA) / rangeA) * axisBase * sx, (float)((e - minE) / rangeE) * axisBase * sy, (float)((inc - minI) / rangeI) * axisBase * sz));
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

	private void RenderScene(Graphics? overlayGraphics)
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

		GL.PointSize(1f);
		GL.Color3(1f, .8f, .2f);
		GL.Begin(PrimitiveType.Points);
		for (int i = 0; i < _renderPoints.Count; i++)
		{
			GL.Vertex3(_renderPoints[i].X, _renderPoints[i].Y, _renderPoints[i].Z);
		}
		GL.End();
		_glControl.SwapBuffers();

		overlayGraphics?.DrawString("X-axis: semi-major axis a [AU]", _overlayFont, Brushes.IndianRed, 8, 8);
		overlayGraphics?.DrawString("Y-axis: eccentricity e [-]", _overlayFont, Brushes.LightGreen, 8, 26);
		overlayGraphics?.DrawString("Z-axis: inclination i [°]", _overlayFont, Brushes.LightSkyBlue, 8, 44);
		overlayGraphics?.DrawString("Rotate: left mouse | Pan: right mouse | Zoom: wheel", _overlayFont, Brushes.WhiteSmoke, 8, 64);
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
				Progress<List<AeiPoint>> live = new(batch => { _rawPoints.AddRange(batch); RebuildRenderPointsAndInvalidate(); });
				List<AeiPoint> final = await Task.Run(() => BuildPointData(_buttonLive.Checked, progress, live, _cts.Token), _cts.Token);
				_rawPoints = final;
				RebuildRenderPointsAndInvalidate();
				UpdateProgress(100);
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
					UpdateRunningState(false);
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
	private void GlControl_Paint(object? sender, PaintEventArgs e) => RenderScene(e.Graphics);

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
