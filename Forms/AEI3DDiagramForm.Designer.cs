namespace Planetoid_DB.Forms
{
	partial class AEI3DDiagramForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			toolStripContainer = new ToolStripContainer();
			panelGlControl = new Panel();
			kryptonStatusStrip = new Krypton.Toolkit.KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			toolStripMain = new ToolStrip();
			toolStripButtonStartPause = new ToolStripButton();
			toolStripButtonCancel = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			toolStripButtonLiveUpdate = new ToolStripButton();
			toolStripButtonLogarithmicScale = new ToolStripButton();
			toolStripSeparator2 = new ToolStripSeparator();
			toolStripProgressBar = new ToolStripProgressBar();
			toolStripLabelProgress = new ToolStripLabel();
			toolStripScaling = new ToolStrip();
			toolStripLabelScaleX = new ToolStripLabel();
			toolStripNumericUpDownScaleX = new Helpers.ToolStripNumericUpDown();
			toolStripLabelScaleY = new ToolStripLabel();
			toolStripNumericUpDownScaleY = new Helpers.ToolStripNumericUpDown();
			toolStripLabelScaleZ = new ToolStripLabel();
			toolStripNumericUpDownScaleZ = new Helpers.ToolStripNumericUpDown();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			kryptonStatusStrip.SuspendLayout();
			toolStripMain.SuspendLayout();
			toolStripScaling.SuspendLayout();
			SuspendLayout();
			//
			// toolStripContainer
			//
			//
			// toolStripContainer.BottomToolStripPanel
			//
			toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
			//
			// toolStripContainer.ContentPanel
			//
			toolStripContainer.ContentPanel.Controls.Add(panelGlControl);
			toolStripContainer.ContentPanel.Size = new Size(1008, 650);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(1008, 729);
			toolStripContainer.TabIndex = 0;
			//
			// toolStripContainer.TopToolStripPanel
			//
			toolStripContainer.TopToolStripPanel.Controls.Add(toolStripMain);
			toolStripContainer.TopToolStripPanel.Controls.Add(toolStripScaling);
			//
			// panelGlControl
			//
			panelGlControl.BackColor = Color.Black;
			panelGlControl.Dock = DockStyle.Fill;
			panelGlControl.Location = new Point(0, 0);
			panelGlControl.Name = "panelGlControl";
			panelGlControl.Size = new Size(1008, 650);
			panelGlControl.TabIndex = 0;
			//
			// kryptonStatusStrip
			//
			kryptonStatusStrip.Dock = DockStyle.None;
			kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
			kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			kryptonStatusStrip.Location = new Point(0, 0);
			kryptonStatusStrip.Name = "kryptonStatusStrip";
			kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			kryptonStatusStrip.Size = new Size(1008, 22);
			kryptonStatusStrip.TabIndex = 0;
			kryptonStatusStrip.Text = "Status bar";
			//
			// labelInformation
			//
			labelInformation.Name = "labelInformation";
			labelInformation.Size = new Size(993, 17);
			labelInformation.Spring = true;
			labelInformation.Text = "Ready";
			labelInformation.TextAlign = ContentAlignment.MiddleLeft;
			//
			// toolStripMain
			//
			toolStripMain.Dock = DockStyle.None;
			toolStripMain.Font = new Font("Segoe UI", 9F);
			toolStripMain.Items.AddRange(new ToolStripItem[] { toolStripButtonStartPause, toolStripButtonCancel, toolStripSeparator1, toolStripButtonLiveUpdate, toolStripButtonLogarithmicScale, toolStripSeparator2, toolStripProgressBar, toolStripLabelProgress });
			toolStripMain.Location = new Point(3, 0);
			toolStripMain.Name = "toolStripMain";
			toolStripMain.Size = new Size(586, 25);
			toolStripMain.TabIndex = 0;
			//
			// toolStripButtonStartPause
			//
			toolStripButtonStartPause.DisplayStyle = ToolStripItemDisplayStyle.Text;
			toolStripButtonStartPause.Name = "toolStripButtonStartPause";
			toolStripButtonStartPause.Size = new Size(35, 22);
			toolStripButtonStartPause.Text = "Start";
			toolStripButtonStartPause.ToolTipText = "Start or pause the processing";
			toolStripButtonStartPause.Click += ButtonStartPause_Click;
			//
			// toolStripButtonCancel
			//
			toolStripButtonCancel.DisplayStyle = ToolStripItemDisplayStyle.Text;
			toolStripButtonCancel.Name = "toolStripButtonCancel";
			toolStripButtonCancel.Size = new Size(47, 22);
			toolStripButtonCancel.Text = "Cancel";
			toolStripButtonCancel.ToolTipText = "Cancel the processing";
			toolStripButtonCancel.Click += ButtonCancel_Click;
			//
			// toolStripSeparator1
			//
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 25);
			//
			// toolStripButtonLiveUpdate
			//
			toolStripButtonLiveUpdate.CheckOnClick = true;
			toolStripButtonLiveUpdate.Checked = true;
			toolStripButtonLiveUpdate.CheckState = CheckState.Checked;
			toolStripButtonLiveUpdate.DisplayStyle = ToolStripItemDisplayStyle.Text;
			toolStripButtonLiveUpdate.Name = "toolStripButtonLiveUpdate";
			toolStripButtonLiveUpdate.Size = new Size(76, 22);
			toolStripButtonLiveUpdate.Text = "Live Update";
			toolStripButtonLiveUpdate.ToolTipText = "Enable live updates during processing";
			toolStripButtonLiveUpdate.CheckedChanged += CheckBoxLiveUpdate_CheckedChanged;
			//
			// toolStripButtonLogarithmicScale
			//
			toolStripButtonLogarithmicScale.CheckOnClick = true;
			toolStripButtonLogarithmicScale.DisplayStyle = ToolStripItemDisplayStyle.Text;
			toolStripButtonLogarithmicScale.Name = "toolStripButtonLogarithmicScale";
			toolStripButtonLogarithmicScale.Size = new Size(114, 22);
			toolStripButtonLogarithmicScale.Text = "Logarithmic Scale";
			toolStripButtonLogarithmicScale.ToolTipText = "Use logarithmic scale for axes";
			toolStripButtonLogarithmicScale.CheckedChanged += CheckBoxLogarithmicScale_CheckedChanged;
			//
			// toolStripSeparator2
			//
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(6, 25);
			//
			// toolStripProgressBar
			//
			toolStripProgressBar.Name = "toolStripProgressBar";
			toolStripProgressBar.Size = new Size(200, 22);
			//
			// toolStripLabelProgress
			//
			toolStripLabelProgress.Name = "toolStripLabelProgress";
			toolStripLabelProgress.Size = new Size(23, 22);
			toolStripLabelProgress.Text = "0%";
			//
			// toolStripScaling
			//
			toolStripScaling.Dock = DockStyle.None;
			toolStripScaling.Font = new Font("Segoe UI", 9F);
			toolStripScaling.Items.AddRange(new ToolStripItem[] { toolStripLabelScaleX, toolStripNumericUpDownScaleX, toolStripLabelScaleY, toolStripNumericUpDownScaleY, toolStripLabelScaleZ, toolStripNumericUpDownScaleZ });
			toolStripScaling.Location = new Point(3, 25);
			toolStripScaling.Name = "toolStripScaling";
			toolStripScaling.Size = new Size(453, 25);
			toolStripScaling.TabIndex = 1;
			//
			// toolStripLabelScaleX
			//
			toolStripLabelScaleX.Name = "toolStripLabelScaleX";
			toolStripLabelScaleX.Size = new Size(99, 22);
			toolStripLabelScaleX.Text = "X-axis scale (a):";
			//
			// toolStripNumericUpDownScaleX
			//
			toolStripNumericUpDownScaleX.DecimalPlaces = 2;
			toolStripNumericUpDownScaleX.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleX.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			toolStripNumericUpDownScaleX.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleX.Name = "toolStripNumericUpDownScaleX";
			toolStripNumericUpDownScaleX.Size = new Size(50, 25);
			toolStripNumericUpDownScaleX.Text = "1.00";
			toolStripNumericUpDownScaleX.Value = new decimal(new int[] { 1, 0, 0, 0 });
			toolStripNumericUpDownScaleX.ValueChanged += NumericUpDownScaleX_ValueChanged;
			//
			// toolStripLabelScaleY
			//
			toolStripLabelScaleY.Name = "toolStripLabelScaleY";
			toolStripLabelScaleY.Size = new Size(97, 22);
			toolStripLabelScaleY.Text = "Y-axis scale (e):";
			//
			// toolStripNumericUpDownScaleY
			//
			toolStripNumericUpDownScaleY.DecimalPlaces = 2;
			toolStripNumericUpDownScaleY.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleY.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			toolStripNumericUpDownScaleY.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleY.Name = "toolStripNumericUpDownScaleY";
			toolStripNumericUpDownScaleY.Size = new Size(50, 25);
			toolStripNumericUpDownScaleY.Text = "1.00";
			toolStripNumericUpDownScaleY.Value = new decimal(new int[] { 1, 0, 0, 0 });
			toolStripNumericUpDownScaleY.ValueChanged += NumericUpDownScaleY_ValueChanged;
			//
			// toolStripLabelScaleZ
			//
			toolStripLabelScaleZ.Name = "toolStripLabelScaleZ";
			toolStripLabelScaleZ.Size = new Size(93, 22);
			toolStripLabelScaleZ.Text = "Z-axis scale (i):";
			//
			// toolStripNumericUpDownScaleZ
			//
			toolStripNumericUpDownScaleZ.DecimalPlaces = 2;
			toolStripNumericUpDownScaleZ.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleZ.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			toolStripNumericUpDownScaleZ.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
			toolStripNumericUpDownScaleZ.Name = "toolStripNumericUpDownScaleZ";
			toolStripNumericUpDownScaleZ.Size = new Size(50, 25);
			toolStripNumericUpDownScaleZ.Text = "1.00";
			toolStripNumericUpDownScaleZ.Value = new decimal(new int[] { 1, 0, 0, 0 });
			toolStripNumericUpDownScaleZ.ValueChanged += NumericUpDownScaleZ_ValueChanged;
			//
			// AEI3DDiagramForm
			//
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1008, 729);
			Controls.Add(toolStripContainer);
			Name = "AEI3DDiagramForm";
			Text = "a,e,i-Diagram";
			Load += AEI3DDiagramForm_Load;
			FormClosing += AEI3DDiagramForm_FormClosing;
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			kryptonStatusStrip.ResumeLayout(false);
			kryptonStatusStrip.PerformLayout();
			toolStripMain.ResumeLayout(false);
			toolStripMain.PerformLayout();
			toolStripScaling.ResumeLayout(false);
			toolStripScaling.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private ToolStripContainer toolStripContainer;
		private Panel panelGlControl;
		private Krypton.Toolkit.KryptonStatusStrip kryptonStatusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStrip toolStripMain;
		private ToolStripButton toolStripButtonStartPause;
		private ToolStripButton toolStripButtonCancel;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton toolStripButtonLiveUpdate;
		private ToolStripButton toolStripButtonLogarithmicScale;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripProgressBar toolStripProgressBar;
		private ToolStripLabel toolStripLabelProgress;
		private ToolStrip toolStripScaling;
		private ToolStripLabel toolStripLabelScaleX;
		private Helpers.ToolStripNumericUpDown toolStripNumericUpDownScaleX;
		private ToolStripLabel toolStripLabelScaleY;
		private Helpers.ToolStripNumericUpDown toolStripNumericUpDownScaleY;
		private ToolStripLabel toolStripLabelScaleZ;
		private Helpers.ToolStripNumericUpDown toolStripNumericUpDownScaleZ;
	}
}
