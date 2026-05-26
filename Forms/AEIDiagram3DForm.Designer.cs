using Krypton.Toolkit;

using Planetoid_DB.Helpers;

namespace Planetoid_DB.Forms;

partial class AEIDiagram3DForm
{
	private System.ComponentModel.IContainer components = null;

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
			_cts?.Dispose();
			_pauseGate.Dispose();
			_glControl.Dispose();
			_overlayFont.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		_container = new ToolStripContainer();
		_panelMain = new KryptonPanel();
		_panelGl = new Panel();
		_toolStripMain = new KryptonToolStrip();
		_buttonStartPause = new ToolStripButton();
		_buttonCancel = new ToolStripButton();
		_buttonLive = new ToolStripButton();
		_buttonLog = new ToolStripButton();
		_toolStripScaling = new KryptonToolStrip();
		_scaleX = new ToolStripNumericUpDown();
		_scaleY = new ToolStripNumericUpDown();
		_scaleZ = new ToolStripNumericUpDown();
		_toolStripProgress = new KryptonToolStrip();
		_progressBar = new KryptonProgressBarToolStripItem();
		_statusStrip = new KryptonStatusStrip();
		_labelInformation = new ToolStripStatusLabel();
		_container.BottomToolStripPanel.SuspendLayout();
		_container.ContentPanel.SuspendLayout();
		_container.TopToolStripPanel.SuspendLayout();
		_container.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)_panelMain).BeginInit();
		_panelMain.SuspendLayout();
		_toolStripMain.SuspendLayout();
		_toolStripScaling.SuspendLayout();
		_toolStripProgress.SuspendLayout();
		_statusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// _container
		// 
		// 
		// _container.BottomToolStripPanel
		// 
		_container.BottomToolStripPanel.Controls.Add(_statusStrip);
		// 
		// _container.ContentPanel
		// 
		_container.ContentPanel.Controls.Add(_panelMain);
		_container.ContentPanel.Size = new Size(980, 607);
		_container.Dock = DockStyle.Fill;
		_container.Location = new Point(0, 0);
		_container.Name = "_container";
		_container.Size = new Size(980, 680);
		_container.TabIndex = 0;
		// 
		// _container.TopToolStripPanel
		// 
		_container.TopToolStripPanel.Controls.Add(_toolStripMain);
		_container.TopToolStripPanel.Controls.Add(_toolStripScaling);
		_container.TopToolStripPanel.Controls.Add(_toolStripProgress);
		// 
		// _panelMain
		// 
		_panelMain.Controls.Add(_panelGl);
		_panelMain.Dock = DockStyle.Fill;
		_panelMain.Location = new Point(0, 0);
		_panelMain.Name = "_panelMain";
		_panelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		_panelMain.Size = new Size(980, 607);
		_panelMain.TabIndex = 0;
		// 
		// _panelGl
		// 
		_panelGl.BackColor = Color.Black;
		_panelGl.Dock = DockStyle.Fill;
		_panelGl.Location = new Point(0, 0);
		_panelGl.Name = "_panelGl";
		_panelGl.Size = new Size(980, 607);
		_panelGl.TabIndex = 0;
		// 
		// _toolStripMain
		// 
		_toolStripMain.AccessibleDescription = "Controls for generation, cancellation, and display options.";
		_toolStripMain.AccessibleName = "Main controls";
		_toolStripMain.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripMain.Dock = DockStyle.None;
		_toolStripMain.Font = new Font("Segoe UI", 9F);
		_toolStripMain.Items.AddRange(new ToolStripItem[] { _buttonStartPause, _buttonCancel, _buttonLive, _buttonLog });
		_toolStripMain.Location = new Point(3, 0);
		_toolStripMain.Name = "_toolStripMain";
		_toolStripMain.Size = new Size(285, 25);
		_toolStripMain.TabIndex = 0;
		_toolStripMain.Enter += Control_Enter;
		_toolStripMain.Leave += Control_Leave;
		_toolStripMain.MouseEnter += Control_Enter;
		_toolStripMain.MouseLeave += Control_Leave;
		// 
		// _buttonStartPause
		// 
		_buttonStartPause.AccessibleDescription = "Starts, pauses, or resumes point generation.";
		_buttonStartPause.AccessibleName = "Start or pause generation";
		_buttonStartPause.AccessibleRole = AccessibleRole.PushButton;
		_buttonStartPause.Image = Resources.FatcowIcons16px.fatcow_control_play_blue_16px;
		_buttonStartPause.Name = "_buttonStartPause";
		_buttonStartPause.Size = new Size(51, 22);
		_buttonStartPause.Text = "&Start";
		_buttonStartPause.Click += ToolStripButtonStartPause_Click;
		_buttonStartPause.MouseEnter += Control_Enter;
		_buttonStartPause.MouseLeave += Control_Leave;
		// 
		// _buttonCancel
		// 
		_buttonCancel.AccessibleDescription = "Cancels ongoing point generation.";
		_buttonCancel.AccessibleName = "Cancel generation";
		_buttonCancel.AccessibleRole = AccessibleRole.PushButton;
		_buttonCancel.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
		_buttonCancel.Name = "_buttonCancel";
		_buttonCancel.Size = new Size(63, 22);
		_buttonCancel.Text = "&Cancel";
		_buttonCancel.Click += ToolStripButtonCancel_Click;
		_buttonCancel.MouseEnter += Control_Enter;
		_buttonCancel.MouseLeave += Control_Leave;
		// 
		// _buttonLive
		// 
		_buttonLive.AccessibleDescription = "Toggles live rendering updates during generation.";
		_buttonLive.AccessibleName = "Live updates";
		_buttonLive.AccessibleRole = AccessibleRole.CheckButton;
		_buttonLive.Checked = true;
		_buttonLive.CheckOnClick = true;
		_buttonLive.CheckState = CheckState.Checked;
		_buttonLive.Image = Resources.FatcowIcons16px.fatcow_tick_circle_frame_16px;
		_buttonLive.Name = "_buttonLive";
		_buttonLive.Size = new Size(43, 22);
		_buttonLive.Text = "On";
		_buttonLive.CheckedChanged += ToolStripButtonLive_CheckedChanged;
		_buttonLive.MouseEnter += Control_Enter;
		_buttonLive.MouseLeave += Control_Leave;
		// 
		// _buttonLog
		// 
		_buttonLog.AccessibleDescription = "Toggles logarithmic axis scaling.";
		_buttonLog.AccessibleName = "Log scale";
		_buttonLog.AccessibleRole = AccessibleRole.CheckButton;
		_buttonLog.CheckOnClick = true;
		_buttonLog.Image = Resources.FatcowIcons16px.fatcow_chart_curve_16px;
		_buttonLog.Name = "_buttonLog";
		_buttonLog.Size = new Size(76, 22);
		_buttonLog.Text = "&Log scale";
		_buttonLog.CheckedChanged += ToolStripButtonLog_CheckedChanged;
		_buttonLog.MouseEnter += Control_Enter;
		_buttonLog.MouseLeave += Control_Leave;
		// 
		// _toolStripScaling
		// 
		_toolStripScaling.AccessibleDescription = "Sets scaling factors for a, e, and i axes.";
		_toolStripScaling.AccessibleName = "Axis scaling controls";
		_toolStripScaling.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripScaling.Dock = DockStyle.None;
		_toolStripScaling.Font = new Font("Segoe UI", 9F);
		_toolStripScaling.Items.AddRange(new ToolStripItem[] { _scaleX, _scaleY, _scaleZ });
		_toolStripScaling.Location = new Point(288, 0);
		_toolStripScaling.Name = "_toolStripScaling";
		_toolStripScaling.Size = new Size(268, 26);
		_toolStripScaling.TabIndex = 1;
		_toolStripScaling.Enter += Control_Enter;
		_toolStripScaling.Leave += Control_Leave;
		_toolStripScaling.MouseEnter += Control_Enter;
		_toolStripScaling.MouseLeave += Control_Leave;
		// 
		// _scaleX
		// 
		_scaleX.AutoSize = true;
		_scaleX.Name = "_scaleX";
		_scaleX.Size = new Size(41, 23);
		_scaleX.Text = "0";
		// 
		// _scaleY
		// 
		_scaleY.AutoSize = true;
		_scaleY.Name = "_scaleY";
		_scaleY.Size = new Size(41, 23);
		_scaleY.Text = "0";
		// 
		// _scaleZ
		// 
		_scaleZ.AutoSize = true;
		_scaleZ.Name = "_scaleZ";
		_scaleZ.Size = new Size(41, 23);
		_scaleZ.Text = "0";
		// 
		// _toolStripProgress
		// 
		_toolStripProgress.AccessibleDescription = "Displays generation progress.";
		_toolStripProgress.AccessibleName = "Progress display";
		_toolStripProgress.AccessibleRole = AccessibleRole.ToolBar;
		_toolStripProgress.Dock = DockStyle.None;
		_toolStripProgress.Font = new Font("Segoe UI", 9F);
		_toolStripProgress.Items.AddRange(new ToolStripItem[] { _progressBar });
		_toolStripProgress.Location = new Point(0, 26);
		_toolStripProgress.Name = "_toolStripProgress";
		_toolStripProgress.Size = new Size(980, 25);
		_toolStripProgress.Stretch = true;
		_toolStripProgress.TabIndex = 2;
		_toolStripProgress.Enter += Control_Enter;
		_toolStripProgress.Leave += Control_Leave;
		_toolStripProgress.MouseEnter += Control_Enter;
		_toolStripProgress.MouseLeave += Control_Leave;
		// 
		// _progressBar
		// 
		_progressBar.AccessibleDescription = "Shows generation progress percentage.";
		_progressBar.AccessibleName = "Generation progress";
		_progressBar.AccessibleRole = AccessibleRole.ProgressBar;
		_progressBar.AutoSize = false;
		_progressBar.Name = "_progressBar";
		_progressBar.Size = new Size(760, 19);
		_progressBar.Text = "0%";
		_progressBar.Values.Text = "0%";
		_progressBar.MouseEnter += Control_Enter;
		_progressBar.MouseLeave += Control_Leave;
		// 
		// _statusStrip
		// 
		_statusStrip.Dock = DockStyle.None;
		_statusStrip.Font = new Font("Segoe UI", 9F);
		_statusStrip.Items.AddRange(new ToolStripItem[] { _labelInformation });
		_statusStrip.Location = new Point(0, 0);
		_statusStrip.Name = "_statusStrip";
		_statusStrip.ProgressBars = null;
		_statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		_statusStrip.Size = new Size(980, 22);
		_statusStrip.TabIndex = 0;
		// 
		// _labelInformation
		// 
		_labelInformation.AccessibleDescription = "Shows point count, exclusions, and axis details.";
		_labelInformation.AccessibleName = "Orbital diagram status information";
		_labelInformation.AccessibleRole = AccessibleRole.StaticText;
		_labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		_labelInformation.Name = "_labelInformation";
		_labelInformation.Size = new Size(16, 17);
		_labelInformation.MouseEnter += Control_Enter;
		_labelInformation.MouseLeave += Control_Leave;
		// 
		// AEIDiagram3DForm
		// 
		ClientSize = new Size(980, 680);
		ControlBox = false;
		Controls.Add(_container);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "AEIDiagram3DForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "a,e,i Diagram (3D)";
		FormClosing += AEIDiagram3DForm_FormClosing;
		Load += AEIDiagram3DForm_Load;
		_container.BottomToolStripPanel.ResumeLayout(false);
		_container.BottomToolStripPanel.PerformLayout();
		_container.ContentPanel.ResumeLayout(false);
		_container.TopToolStripPanel.ResumeLayout(false);
		_container.TopToolStripPanel.PerformLayout();
		_container.ResumeLayout(false);
		_container.PerformLayout();
		((System.ComponentModel.ISupportInitialize)_panelMain).EndInit();
		_panelMain.ResumeLayout(false);
		_toolStripMain.ResumeLayout(false);
		_toolStripMain.PerformLayout();
		_toolStripScaling.ResumeLayout(false);
		_toolStripScaling.PerformLayout();
		_toolStripProgress.ResumeLayout(false);
		_toolStripProgress.PerformLayout();
		_statusStrip.ResumeLayout(false);
		_statusStrip.PerformLayout();
		ResumeLayout(false);
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

	#region Windows Form Designer generated variables

	private ToolStripContainer _container;
	private KryptonStatusStrip _statusStrip;
	private ToolStripStatusLabel _labelInformation;
	private KryptonToolStrip _toolStripMain;
	private KryptonToolStrip _toolStripScaling;
	private KryptonToolStrip _toolStripProgress;
	private ToolStripButton _buttonStartPause;
	private ToolStripButton _buttonCancel;
	private ToolStripButton _buttonLive;
	private ToolStripButton _buttonLog;
	private KryptonProgressBarToolStripItem _progressBar;
	private ToolStripNumericUpDown _scaleX;
	private ToolStripNumericUpDown _scaleY;
	private ToolStripNumericUpDown _scaleZ;
	private KryptonPanel _panelMain;
	private Panel _panelGl;

	#endregion
}
