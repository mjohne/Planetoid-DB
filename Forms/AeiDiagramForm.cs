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

/// <summary>Displays a 3D point diagram of semi-major axis (a), eccentricity (e), and inclination (i) for all known planetoids.</summary>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public sealed class AeiDiagramForm : BaseKryptonForm
{
/// <summary>NLog logger instance.</summary>
private static readonly Logger logger = LogManager.GetCurrentClassLogger();

/// <summary>Gets the status label used for displaying information in the status bar.</summary>
protected override ToolStripStatusLabel? StatusLabel => labelInformation;

/// <summary>Stores the source MPCORB records passed in by the main form.</summary>
private readonly IReadOnlyList<string> _planetoids;

/// <summary>Synchronization object for point data exchange between worker and UI thread.</summary>
private readonly object _pointsSync = new();

/// <summary>Stores the currently collected raw (a,e,i) points.</summary>
private List<AeiPoint> _rawPoints = [];

/// <summary>Stores points transformed for OpenGL rendering.</summary>
private List<(float X, float Y, float Z)> _renderPoints = [];

/// <summary>Stores the currently running cancellation token source.</summary>
private CancellationTokenSource? _cancellationTokenSource;

/// <summary>Controls pause and resume state for the worker loop.</summary>
private readonly ManualResetEventSlim _pauseGate = new(initialState: true);

/// <summary>Indicates whether the generation workflow is currently paused.</summary>
private bool _isPaused;

/// <summary>Stores the number of points excluded by the active logarithmic scale.</summary>
private int _excludedByLogScale;

/// <summary>Stores the current X axis scaling factor.</summary>
private float _xAxisScale = 1f;

/// <summary>Stores the current Y axis scaling factor.</summary>
private float _yAxisScale = 1f;

/// <summary>Stores the current Z axis scaling factor.</summary>
private float _zAxisScale = 1f;

/// <summary>Whether the OpenGL context is initialized and ready for rendering.</summary>
private bool _glReady;

/// <summary>The embedded OpenTK GLControl that provides the OpenGL rendering surface.</summary>
private GLControl _glControl = null!;

private readonly ToolStripContainer toolStripContainer;
private readonly KryptonToolStrip kryptonToolStripProgress;
private readonly ToolStripLabel toolStripLabelProgress;
private readonly KryptonProgressBarToolStripItem kryptonProgressBar;
private readonly KryptonToolStrip kryptonToolStripWorkflow;
private readonly ToolStripButton toolStripButtonStartPause;
private readonly ToolStripButton toolStripButtonCancel;
private readonly ToolStripLabel toolStripLabelLiveDisplay;
private readonly ToolStripButton toolStripButtonLiveDisplay;
private readonly ToolStripButton toolStripButtonLogScale;
private readonly KryptonToolStrip kryptonToolStripAxisScale;
private readonly ToolStripLabel toolStripLabelScaleX;
private readonly ToolStripTextBox toolStripTextBoxScaleX;
private readonly ToolStripLabel toolStripLabelScaleY;
private readonly ToolStripTextBox toolStripTextBoxScaleY;
private readonly ToolStripLabel toolStripLabelScaleZ;
private readonly ToolStripTextBox toolStripTextBoxScaleZ;
private readonly KryptonPanel kryptonPanelMain;
private readonly Panel panelGl;
private readonly KryptonStatusStrip kryptonStatusStrip;
private readonly ToolStripStatusLabel labelInformation;
private readonly KryptonManager kryptonManager;

/// <summary>Represents one parsed (a,e,i) point.</summary>
/// <param name="SemiMajorAxis">Semi-major axis in AU.</param>
/// <param name="Eccentricity">Eccentricity (unitless).</param>
/// <param name="Inclination">Inclination in degrees.</param>
private readonly record struct AeiPoint(double SemiMajorAxis, double Eccentricity, double Inclination);

/// <summary>Initializes a new instance of the <see cref="AeiDiagramForm"/> class.</summary>
/// <param name="planetoids">The planetoid string records to process from the database.</param>
public AeiDiagramForm(IReadOnlyList<string> planetoids)
{
_planetoids = planetoids;

toolStripContainer = new ToolStripContainer();
kryptonToolStripProgress = new KryptonToolStrip();
toolStripLabelProgress = new ToolStripLabel();
kryptonProgressBar = new KryptonProgressBarToolStripItem();
kryptonToolStripWorkflow = new KryptonToolStrip();
toolStripButtonStartPause = new ToolStripButton();
toolStripButtonCancel = new ToolStripButton();
toolStripLabelLiveDisplay = new ToolStripLabel();
toolStripButtonLiveDisplay = new ToolStripButton();
toolStripButtonLogScale = new ToolStripButton();
kryptonToolStripAxisScale = new KryptonToolStrip();
toolStripLabelScaleX = new ToolStripLabel();
toolStripTextBoxScaleX = new ToolStripTextBox();
toolStripLabelScaleY = new ToolStripLabel();
toolStripTextBoxScaleY = new ToolStripTextBox();
toolStripLabelScaleZ = new ToolStripLabel();
toolStripTextBoxScaleZ = new ToolStripTextBox();
kryptonPanelMain = new KryptonPanel();
panelGl = new Panel();
kryptonStatusStrip = new KryptonStatusStrip();
labelInformation = new ToolStripStatusLabel();
kryptonManager = new KryptonManager();

InitializeComponent();
UpdateProgress(percent: 0);
UpdateRunningState(isRunning: false);
logger.Info(message: "AeiDiagramForm initialized with {0} planetoids.", argument: _planetoids.Count);
}

/// <summary>Releases all resources used by the <see cref="AeiDiagramForm"/>.</summary>
/// <param name="disposing">True if managed resources should be disposed; otherwise false.</param>
protected override void Dispose(bool disposing)
{
if (disposing)
{
_cancellationTokenSource?.Cancel();
_cancellationTokenSource?.Dispose();
_pauseGate.Dispose();
if (_glControl is not null)
{
_glControl.Dispose();
}
kryptonManager.Dispose();
}

base.Dispose(disposing);
}

/// <summary>Returns a short debugger display string for this instance.</summary>
/// <returns>A string representation of the current instance for use in the debugger.</returns>
private string GetDebuggerDisplay() => ToString();

/// <summary>Initializes the controls and layout of the form.</summary>
private void InitializeComponent()
{
SuspendLayout();

toolStripContainer.Dock = DockStyle.Fill;
toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
toolStripContainer.ContentPanel.Size = new Size(980, 620);
toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripAxisScale);
toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripWorkflow);
toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripProgress);
toolStripContainer.Enter += Control_Enter;
toolStripContainer.Leave += Control_Leave;
toolStripContainer.MouseEnter += Control_Enter;
toolStripContainer.MouseLeave += Control_Leave;

kryptonToolStripProgress.Items.AddRange([toolStripLabelProgress, kryptonProgressBar]);
kryptonToolStripProgress.Stretch = true;
kryptonToolStripProgress.Dock = DockStyle.None;
kryptonToolStripProgress.Size = new Size(980, 25);
kryptonToolStripProgress.Text = "Progress toolbar";
kryptonToolStripProgress.Enter += Control_Enter;
kryptonToolStripProgress.Leave += Control_Leave;
kryptonToolStripProgress.MouseEnter += Control_Enter;
kryptonToolStripProgress.MouseLeave += Control_Leave;

toolStripLabelProgress.Text = "Progress";
toolStripLabelProgress.MouseEnter += Control_Enter;
toolStripLabelProgress.MouseLeave += Control_Leave;

kryptonProgressBar.AutoSize = false;
kryptonProgressBar.Margin = new Padding(5, 2, 1, 2);
kryptonProgressBar.Size = new Size(830, 19);
kryptonProgressBar.Values.Text = "0%";
kryptonProgressBar.MouseEnter += Control_Enter;
kryptonProgressBar.MouseLeave += Control_Leave;

kryptonToolStripWorkflow.Items.AddRange([
toolStripButtonStartPause,
toolStripButtonCancel,
new ToolStripSeparator(),
toolStripLabelLiveDisplay,
toolStripButtonLiveDisplay,
new ToolStripSeparator(),
toolStripButtonLogScale
]);
kryptonToolStripWorkflow.Stretch = true;
kryptonToolStripWorkflow.Dock = DockStyle.None;
kryptonToolStripWorkflow.Size = new Size(980, 25);
kryptonToolStripWorkflow.Text = "Workflow toolbar";
kryptonToolStripWorkflow.Enter += Control_Enter;
kryptonToolStripWorkflow.Leave += Control_Leave;
kryptonToolStripWorkflow.MouseEnter += Control_Enter;
kryptonToolStripWorkflow.MouseLeave += Control_Leave;

toolStripButtonStartPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
toolStripButtonStartPause.Text = "&Start";
toolStripButtonStartPause.Click += ToolStripButtonStartPause_Click;
toolStripButtonStartPause.MouseEnter += Control_Enter;
toolStripButtonStartPause.MouseLeave += Control_Leave;

toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
toolStripButtonCancel.Text = "C&ancel";
toolStripButtonCancel.Click += ToolStripButtonCancel_Click;
toolStripButtonCancel.MouseEnter += Control_Enter;
toolStripButtonCancel.MouseLeave += Control_Leave;

toolStripLabelLiveDisplay.Text = "Live display";
toolStripLabelLiveDisplay.MouseEnter += Control_Enter;
toolStripLabelLiveDisplay.MouseLeave += Control_Leave;

toolStripButtonLiveDisplay.CheckOnClick = true;
toolStripButtonLiveDisplay.Checked = true;
toolStripButtonLiveDisplay.DisplayStyle = ToolStripItemDisplayStyle.Text;
toolStripButtonLiveDisplay.Text = "On";
toolStripButtonLiveDisplay.CheckedChanged += ToolStripButtonLiveDisplay_CheckedChanged;
toolStripButtonLiveDisplay.MouseEnter += Control_Enter;
toolStripButtonLiveDisplay.MouseLeave += Control_Leave;

toolStripButtonLogScale.CheckOnClick = true;
toolStripButtonLogScale.DisplayStyle = ToolStripItemDisplayStyle.Text;
toolStripButtonLogScale.Text = "Log scale";
toolStripButtonLogScale.CheckedChanged += ToolStripButtonLogScale_CheckedChanged;
toolStripButtonLogScale.MouseEnter += Control_Enter;
toolStripButtonLogScale.MouseLeave += Control_Leave;

kryptonToolStripAxisScale.Items.AddRange([
toolStripLabelScaleX,
toolStripTextBoxScaleX,
toolStripLabelScaleY,
toolStripTextBoxScaleY,
toolStripLabelScaleZ,
toolStripTextBoxScaleZ
]);
kryptonToolStripAxisScale.Stretch = true;
kryptonToolStripAxisScale.Dock = DockStyle.None;
kryptonToolStripAxisScale.Size = new Size(980, 25);
kryptonToolStripAxisScale.Text = "Axis scale toolbar";
kryptonToolStripAxisScale.Enter += Control_Enter;
kryptonToolStripAxisScale.Leave += Control_Leave;
kryptonToolStripAxisScale.MouseEnter += Control_Enter;
kryptonToolStripAxisScale.MouseLeave += Control_Leave;

toolStripLabelScaleX.Text = "Scale X (a)";
toolStripLabelScaleY.Text = "Scale Y (e)";
toolStripLabelScaleZ.Text = "Scale Z (i)";

toolStripTextBoxScaleX.Text = "1.0";
toolStripTextBoxScaleY.Text = "1.0";
toolStripTextBoxScaleZ.Text = "1.0";
toolStripTextBoxScaleX.AutoSize = false;
toolStripTextBoxScaleY.AutoSize = false;
toolStripTextBoxScaleZ.AutoSize = false;
toolStripTextBoxScaleX.Width = 70;
toolStripTextBoxScaleY.Width = 70;
toolStripTextBoxScaleZ.Width = 70;
toolStripTextBoxScaleX.TextChanged += ToolStripTextBoxScaleX_TextChanged;
toolStripTextBoxScaleY.TextChanged += ToolStripTextBoxScaleY_TextChanged;
toolStripTextBoxScaleZ.TextChanged += ToolStripTextBoxScaleZ_TextChanged;

kryptonPanelMain.Dock = DockStyle.Fill;
kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
kryptonPanelMain.Controls.Add(panelGl);
kryptonPanelMain.Enter += Control_Enter;
kryptonPanelMain.Leave += Control_Leave;
kryptonPanelMain.MouseEnter += Control_Enter;
kryptonPanelMain.MouseLeave += Control_Leave;

panelGl.BackColor = Color.Black;
panelGl.Dock = DockStyle.Fill;
panelGl.Enter += Control_Enter;
panelGl.Leave += Control_Leave;
panelGl.MouseEnter += Control_Enter;
panelGl.MouseLeave += Control_Leave;

kryptonStatusStrip.Items.AddRange([labelInformation]);
kryptonStatusStrip.Dock = DockStyle.None;
kryptonStatusStrip.Size = new Size(980, 22);
kryptonStatusStrip.Text = "Status bar";
kryptonStatusStrip.Enter += Control_Enter;
kryptonStatusStrip.Leave += Control_Leave;
kryptonStatusStrip.MouseEnter += Control_Enter;
kryptonStatusStrip.MouseLeave += Control_Leave;

labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
labelInformation.Text = "X = semi-major axis (a), Y = eccentricity (e), Z = inclination (i)";
labelInformation.MouseEnter += Control_Enter;
labelInformation.MouseLeave += Control_Leave;

kryptonManager.GlobalPaletteMode = PaletteMode.Global;

AccessibleName = "a,e,i diagram form";
AccessibleDescription = "Displays all known planetoids in a 3D a-e-i diagram";
AutoScaleDimensions = new SizeF(7F, 15F);
AutoScaleMode = AutoScaleMode.Font;
ClientSize = new Size(980, 717);
ControlBox = false;
FormBorderStyle = FormBorderStyle.SizableToolWindow;
MaximizeBox = false;
MinimizeBox = false;
Name = "AeiDiagramForm";
StartPosition = FormStartPosition.CenterScreen;
Text = "a,e,i diagram (3D)";
Controls.Add(toolStripContainer);
Load += AeiDiagramForm_Load;
FormClosing += AeiDiagramForm_FormClosing;

ResumeLayout(false);
}

/// <summary>Creates and configures the embedded <see cref="GLControl"/> and adds it to the OpenGL host panel.</summary>
private void CreateGlControl()
{
GLControlSettings settings = new()
{
API = ContextAPI.OpenGL,
Profile = ContextProfile.Any,
APIVersion = new Version(major: 2, minor: 1),
};

_glControl = new GLControl(glControlSettings: settings)
{
Dock = DockStyle.Fill,
BackColor = Color.Black,
};

_glControl.Paint += GlControl_Paint;
_glControl.Resize += GlControl_Resize;
panelGl.Controls.Add(value: _glControl);
}

/// <summary>Updates the toolbar state to reflect whether point generation is running.</summary>
/// <param name="isRunning">True while the background task is active; otherwise false.</param>
private void UpdateRunningState(bool isRunning)
{
toolStripButtonStartPause.Image = isRunning
? (_isPaused ? FatcowIcons16px.fatcow_control_play_blue_16px : FatcowIcons16px.fatcow_control_pause_blue_16px)
: FatcowIcons16px.fatcow_control_play_blue_16px;
toolStripButtonStartPause.Text = isRunning
? (_isPaused ? "&Resume" : "&Pause")
: "&Start";
toolStripButtonCancel.Enabled = isRunning;
}

/// <summary>Updates the progress bar value and taskbar progress indicator.</summary>
/// <param name="percent">The progress percentage to display.</param>
private void UpdateProgress(int percent)
{
int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
kryptonProgressBar.Value = clampedPercent;
kryptonProgressBar.Values.Text = $"{clampedPercent}%";
if (IsHandleCreated)
{
TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
}
}

/// <summary>Transforms raw points into normalized render coordinates for the OpenGL view.</summary>
private void RebuildRenderPoints()
{
List<AeiPoint> source;
lock (_pointsSync)
{
source = [.. _rawPoints];
}

List<(double X, double Y, double Z)> values = new(capacity: source.Count);
int excluded = 0;
bool logScale = toolStripButtonLogScale.Checked;

for (int i = 0; i < source.Count; i++)
{
double x = source[i].SemiMajorAxis;
double y = source[i].Eccentricity;
double z = source[i].Inclination;
if (logScale)
{
if (x <= 0 || y <= 0 || z <= 0)
{
excluded++;
continue;
}

x = Math.Log10(d: x);
y = Math.Log10(d: y);
z = Math.Log10(d: z);
}

if (double.IsFinite(d: x) && double.IsFinite(d: y) && double.IsFinite(d: z))
{
values.Add(item: (x, y, z));
}
}

if (values.Count == 0)
{
lock (_pointsSync)
{
_renderPoints = [];
_excludedByLogScale = excluded;
}
return;
}

double minX = values.Min(selector: p => p.X);
double maxX = values.Max(selector: p => p.X);
double minY = values.Min(selector: p => p.Y);
double maxY = values.Max(selector: p => p.Y);
double minZ = values.Min(selector: p => p.Z);
double maxZ = values.Max(selector: p => p.Z);

double rangeX = Math.Max(val1: 1e-12, val2: maxX - minX);
double rangeY = Math.Max(val1: 1e-12, val2: maxY - minY);
double rangeZ = Math.Max(val1: 1e-12, val2: maxZ - minZ);

List<(float X, float Y, float Z)> transformed = new(capacity: values.Count);
for (int i = 0; i < values.Count; i++)
{
transformed.Add(item: (
X: (float)((values[i].X - minX) / rangeX * _xAxisScale),
Y: (float)((values[i].Y - minY) / rangeY * _yAxisScale),
Z: (float)((values[i].Z - minZ) / rangeZ * _zAxisScale)));
}

lock (_pointsSync)
{
_renderPoints = transformed;
_excludedByLogScale = excluded;
}
}

/// <summary>Builds all (a,e,i) points on a background thread.</summary>
/// <param name="enableLiveDisplay">True to publish intermediate results while collecting; otherwise false.</param>
/// <param name="progress">Receives percentage updates for the progress bar.</param>
/// <param name="liveResults">Receives intermediate snapshots for live display.</param>
/// <param name="cancellationToken">The token used to cancel the operation.</param>
/// <returns>A list of final parsed points.</returns>
private List<AeiPoint> BuildAeiPoints(
bool enableLiveDisplay,
IProgress<int> progress,
IProgress<List<AeiPoint>> liveResults,
CancellationToken cancellationToken)
{
List<AeiPoint> points = [];
int total = _planetoids.Count;
int progressInterval = Math.Max(val1: 1, val2: total / 100);
int liveInterval = Math.Max(val1: 1, val2: total / 30);

for (int i = 0; i < total; i++)
{
cancellationToken.ThrowIfCancellationRequested();
_pauseGate.Wait(cancellationToken: cancellationToken);

string line = _planetoids[index: i];
if (TryParseSemiMajorAxis(line: line, value: out double semiMajorAxis)
&& TryParseEccentricity(line: line, value: out double eccentricity)
&& TryParseInclination(line: line, value: out double inclination)
&& double.IsFinite(d: semiMajorAxis)
&& double.IsFinite(d: eccentricity)
&& double.IsFinite(d: inclination)
&& semiMajorAxis >= 0
&& eccentricity >= 0
&& inclination >= 0)
{
points.Add(item: new AeiPoint(SemiMajorAxis: semiMajorAxis, Eccentricity: eccentricity, Inclination: inclination));
}

int processed = i + 1;
if (processed % progressInterval == 0 || processed == total)
{
progress.Report(value: processed * 100 / Math.Max(val1: 1, val2: total));
}

if (enableLiveDisplay && (processed % liveInterval == 0 || processed == total))
{
liveResults.Report(value: CreateLivePreviewSnapshot(points: points));
}
}

return points;
}

/// <summary>Creates a bounded snapshot for live updates without copying all collected points.</summary>
/// <param name="points">The currently accumulated points.</param>
/// <returns>A sampled snapshot suitable for intermediate rendering.</returns>
private static List<AeiPoint> CreateLivePreviewSnapshot(List<AeiPoint> points)
{
const int maxPreviewPoints = 30_000;
if (points.Count <= maxPreviewPoints)
{
return [.. points];
}

int step = (int)Math.Ceiling(a: (double)points.Count / maxPreviewPoints);
List<AeiPoint> preview = new(capacity: maxPreviewPoints);
for (int i = 0; i < points.Count; i += step)
{
preview.Add(item: points[i]);
}

return preview;
}

/// <summary>Attempts to parse a floating-point slice from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="startIndex">The inclusive start index of the numeric field.</param>
/// <param name="length">The field length.</param>
/// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseValue(string line, int startIndex, int length, out double value)
{
value = default;
return line.Length >= startIndex + length && double.TryParse(
s: line.Substring(startIndex: startIndex, length: length).Trim(),
style: NumberStyles.Float,
provider: CultureInfo.InvariantCulture,
result: out value);
}

/// <summary>Attempts to parse the semi-major axis from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed semi-major axis if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseSemiMajorAxis(string line, out double value) => TryParseValue(line: line, startIndex: 92, length: 11, value: out value);

/// <summary>Attempts to parse the orbital eccentricity from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed eccentricity if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseEccentricity(string line, out double value) => TryParseValue(line: line, startIndex: 70, length: 9, value: out value);

/// <summary>Attempts to parse the inclination from a raw MPCORB record.</summary>
/// <param name="line">The raw MPCORB line.</param>
/// <param name="value">When this method returns, contains the parsed inclination if parsing succeeded.</param>
/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
private static bool TryParseInclination(string line, out double value) => TryParseValue(line: line, startIndex: 59, length: 9, value: out value);

/// <summary>Handles the <see cref="Form.Load"/> event.</summary>
/// <param name="sender">The event source.</param>
/// <param name="e">Event arguments.</param>
private void AeiDiagramForm_Load(object? sender, EventArgs e)
{
try
{
CreateGlControl();
_glControl.MakeCurrent();
GL.ClearColor(0f, 0f, 0f, 1f);
GL.Enable(cap: EnableCap.DepthTest);
GL.Enable(cap: EnableCap.PointSmooth);
_glReady = true;
_glControl.Invalidate();
}
catch (Exception ex)
{
logger.Error(exception: ex, message: ex.Message);
ShowErrorMessage(message: $"Failed to initialize 3D rendering: {ex.Message}");
}
}

/// <summary>Handles the <see cref="Form.FormClosing"/> event.</summary>
/// <param name="sender">The event source.</param>
/// <param name="e">The event data associated with form closing.</param>
private void AeiDiagramForm_FormClosing(object? sender, FormClosingEventArgs e) => _cancellationTokenSource?.Cancel();

/// <summary>Handles the <see cref="Control.Resize"/> event of the GL control.</summary>
/// <param name="sender">The event source.</param>
/// <param name="e">Event arguments.</param>
private void GlControl_Resize(object? sender, EventArgs e)
{
if (_glReady)
{
_glControl.Invalidate();
}
}

/// <summary>Handles the <see cref="Control.Paint"/> event of the GL control to redraw the scene.</summary>
/// <param name="sender">The event source.</param>
/// <param name="e">Paint event arguments.</param>
private void GlControl_Paint(object? sender, PaintEventArgs e)
{
if (!_glReady)
{
return;
}

if (_glControl.ClientSize.Width <= 0 || _glControl.ClientSize.Height <= 0)
{
return;
}

GL.Viewport(x: 0, y: 0, width: _glControl.ClientSize.Width, height: _glControl.ClientSize.Height);
GL.Clear(mask: ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

GL.MatrixMode(mode: MatrixMode.Projection);
GL.LoadIdentity();
double aspect = (double)_glControl.ClientSize.Width / Math.Max(val1: 1, val2: _glControl.ClientSize.Height);
SetPerspective(fieldOfViewDeg: 45, aspect: aspect, near: 0.1, far: 100.0);

GL.MatrixMode(mode: MatrixMode.Modelview);
GL.LoadIdentity();
GL.Translate(x: -_xAxisScale * 0.5f, y: -_yAxisScale * 0.5f, z: -3.5f - (_zAxisScale * 0.5f));
GL.Rotate(angle: 25f, x: 1f, y: 0f, z: 0f);
GL.Rotate(angle: -35f, x: 0f, y: 1f, z: 0f);

DrawAxes();
DrawPoints();

_glControl.SwapBuffers();
}

/// <summary>Configures a perspective projection matrix using the classic OpenGL frustum API.</summary>
/// <param name="fieldOfViewDeg">Vertical field of view in degrees.</param>
/// <param name="aspect">Viewport aspect ratio (width/height).</param>
/// <param name="near">Near clipping plane distance.</param>
/// <param name="far">Far clipping plane distance.</param>
private static void SetPerspective(double fieldOfViewDeg, double aspect, double near, double far)
{
double top = Math.Tan(a: fieldOfViewDeg * Math.PI / 360.0) * near;
double bottom = -top;
double right = top * aspect;
double left = -right;
GL.Frustum(left: left, right: right, bottom: bottom, top: top, zNear: near, zFar: far);
}

/// <summary>Draws the three diagram axes with color coding: X=a, Y=e, Z=i.</summary>
private void DrawAxes()
{
GL.LineWidth(width: 2f);
GL.Begin(mode: PrimitiveType.Lines);

GL.Color3(red: 1f, green: 0.25f, blue: 0.25f);
GL.Vertex3(x: 0f, y: 0f, z: 0f);
GL.Vertex3(x: _xAxisScale, y: 0f, z: 0f);

GL.Color3(red: 0.35f, green: 1f, blue: 0.35f);
GL.Vertex3(x: 0f, y: 0f, z: 0f);
GL.Vertex3(x: 0f, y: _yAxisScale, z: 0f);

GL.Color3(red: 0.35f, green: 0.55f, blue: 1f);
GL.Vertex3(x: 0f, y: 0f, z: 0f);
GL.Vertex3(x: 0f, y: 0f, z: _zAxisScale);

GL.End();
}

/// <summary>Draws all transformed point markers.</summary>
private void DrawPoints()
{
List<(float X, float Y, float Z)> points;
lock (_pointsSync)
{
points = [.. _renderPoints];
}

GL.PointSize(size: 3f);
GL.Color3(red: 1f, green: 0.86f, blue: 0.25f);
GL.Begin(mode: PrimitiveType.Points);
for (int i = 0; i < points.Count; i++)
{
GL.Vertex3(x: points[i].X, y: points[i].Y, z: points[i].Z);
}
GL.End();
}

/// <summary>Handles the Click event of the start/pause button.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the click.</param>
private async void ToolStripButtonStartPause_Click(object? sender, EventArgs e)
{
if (_cancellationTokenSource is not null)
{
_isPaused = !_isPaused;
if (_isPaused)
{
_pauseGate.Reset();
labelInformation.Text = "Generation paused.";
}
else
{
_pauseGate.Set();
labelInformation.Text = "Generation resumed.";
}
UpdateRunningState(isRunning: true);
return;
}

if (_planetoids.Count == 0)
{
_ = KryptonMessageBox.Show(text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
return;
}

_isPaused = false;
_pauseGate.Set();
UpdateRunningState(isRunning: true);
UpdateProgress(percent: 0);
ClearStatusBar(label: labelInformation);

lock (_pointsSync)
{
_rawPoints = [];
}
RebuildRenderPoints();
_glControl?.Invalidate();

bool enableLiveDisplay = toolStripButtonLiveDisplay.Checked;
_cancellationTokenSource = new CancellationTokenSource();

try
{
Progress<int> progress = new(handler: UpdateProgress);
Progress<List<AeiPoint>> liveResults = new(handler: results =>
{
lock (_pointsSync)
{
_rawPoints = results;
}
RebuildRenderPoints();
_glControl?.Invalidate();
});

List<AeiPoint> finalResults = await Task.Run(
function: () => BuildAeiPoints(
enableLiveDisplay: enableLiveDisplay,
progress: progress,
liveResults: liveResults,
cancellationToken: _cancellationTokenSource.Token),
cancellationToken: _cancellationTokenSource.Token);

lock (_pointsSync)
{
_rawPoints = finalResults;
}
RebuildRenderPoints();
_glControl?.Invalidate();

labelInformation.Text = toolStripButtonLogScale.Checked && _excludedByLogScale > 0
? $"a,e,i diagram created with {_renderPoints.Count:N0} points ({_excludedByLogScale:N0} excluded for log scale)."
: $"a,e,i diagram created with {_renderPoints.Count:N0} points.";
}
catch (OperationCanceledException)
{
logger.Info(message: "a,e,i diagram generation was canceled by the user.");
labelInformation.Text = "a,e,i diagram generation canceled.";
}
catch (Exception ex)
{
logger.Error(exception: ex, message: ex.Message);
ShowErrorMessage(message: $"An error has occurred during a,e,i diagram creation: {ex.Message}");
}
finally
{
_cancellationTokenSource?.Dispose();
_cancellationTokenSource = null;
_isPaused = false;
_pauseGate.Set();
UpdateRunningState(isRunning: false);
}
}

/// <summary>Handles the Click event of the cancel button.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the click.</param>
private void ToolStripButtonCancel_Click(object? sender, EventArgs e)
{
_pauseGate.Set();
_cancellationTokenSource?.Cancel();
}

/// <summary>Handles the CheckedChanged event of the live-display toggle button.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the check-state change.</param>
private void ToolStripButtonLiveDisplay_CheckedChanged(object? sender, EventArgs e) =>
toolStripButtonLiveDisplay.Text = toolStripButtonLiveDisplay.Checked ? "On" : "Off";

/// <summary>Handles the CheckedChanged event of the log-scale toggle button.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the check-state change.</param>
private void ToolStripButtonLogScale_CheckedChanged(object? sender, EventArgs e)
{
RebuildRenderPoints();
_glControl?.Invalidate();
}

/// <summary>Handles the TextChanged event of the X-axis scale textbox.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the text change.</param>
private void ToolStripTextBoxScaleX_TextChanged(object? sender, EventArgs e) =>
TryApplyAxisScale(text: toolStripTextBoxScaleX.Text, assign: value => _xAxisScale = value);

/// <summary>Handles the TextChanged event of the Y-axis scale textbox.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the text change.</param>
private void ToolStripTextBoxScaleY_TextChanged(object? sender, EventArgs e) =>
TryApplyAxisScale(text: toolStripTextBoxScaleY.Text, assign: value => _yAxisScale = value);

/// <summary>Handles the TextChanged event of the Z-axis scale textbox.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data associated with the text change.</param>
private void ToolStripTextBoxScaleZ_TextChanged(object? sender, EventArgs e) =>
TryApplyAxisScale(text: toolStripTextBoxScaleZ.Text, assign: value => _zAxisScale = value);

/// <summary>Validates and applies one axis scale value, then redraws the diagram immediately.</summary>
/// <param name="text">User-entered numeric text.</param>
/// <param name="assign">Setter callback for the selected axis field.</param>
private void TryApplyAxisScale(string text, Action<float> assign)
{
if (!float.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out float value)
|| !float.IsFinite(f: value)
|| value <= 0f)
{
return;
}

assign(value);
RebuildRenderPoints();
_glControl?.Invalidate();
}
}
