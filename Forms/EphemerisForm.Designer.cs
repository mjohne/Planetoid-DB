using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class EphemerisForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

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
			components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(EphemerisForm));
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			toolStripContainer = new ToolStripContainer();
			panel = new KryptonPanel();
			buttonCalculate = new KryptonButton();
			listView = new KryptonListView();
			labelPercent = new KryptonLabel();
			progressBar = new KryptonProgressBar();
			labelEphemeridesStepsInDays = new KryptonLabel();
			numericUpDownStepsInDays = new KryptonNumericUpDown();
			labelEphemeridesEnd = new KryptonLabel();
			dateTimePickerEphemeridesEnd = new KryptonDateTimePicker();
			labelEphemeridesBegin = new KryptonLabel();
			dateTimePickerEphemeridesBegin = new KryptonDateTimePicker();
			toolTip = new ToolTip(components);
			backgroundWorker = new BackgroundWorker();
			kryptonManager = new KryptonManager(components);
			statusStrip.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(406, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 0;
			statusStrip.Text = "status bar";
			// 
			// labelInformation
			// 
			labelInformation.AccessibleDescription = "Shows some information";
			labelInformation.AccessibleName = "Shows some information";
			labelInformation.AccessibleRole = AccessibleRole.StaticText;
			labelInformation.AutoToolTip = true;
			labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelInformation.Margin = new Padding(5, 3, 0, 2);
			labelInformation.Name = "labelInformation";
			labelInformation.Size = new Size(144, 17);
			labelInformation.Text = "some information here";
			labelInformation.ToolTipText = "Show some information";
			// 
			// toolStripContainer
			// 
			toolStripContainer.AccessibleDescription = "Groups the data";
			toolStripContainer.AccessibleName = "panel";
			toolStripContainer.AccessibleRole = AccessibleRole.Pane;
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(panel);
			toolStripContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.ContentPanel.Size = new Size(406, 431);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(406, 453);
			toolStripContainer.TabIndex = 11;
			toolStripContainer.Text = "toolStripContainer";
			toolStripContainer.TopToolStripPanelVisible = false;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "panel";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(buttonCalculate);
			panel.Controls.Add(listView);
			panel.Controls.Add(labelPercent);
			panel.Controls.Add(progressBar);
			panel.Controls.Add(labelEphemeridesStepsInDays);
			panel.Controls.Add(numericUpDownStepsInDays);
			panel.Controls.Add(labelEphemeridesEnd);
			panel.Controls.Add(dateTimePickerEphemeridesEnd);
			panel.Controls.Add(labelEphemeridesBegin);
			panel.Controls.Add(dateTimePickerEphemeridesBegin);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.Size = new Size(406, 431);
			panel.TabIndex = 0;
			// 
			// buttonCalculate
			// 
			buttonCalculate.AccessibleDescription = "Calculates the ephemerides";
			buttonCalculate.AccessibleName = "Calculate the ephemerides";
			buttonCalculate.AccessibleRole = AccessibleRole.PushButton;
			buttonCalculate.Location = new Point(287, 115);
			buttonCalculate.Margin = new Padding(4, 3, 4, 3);
			buttonCalculate.Name = "buttonCalculate";
			buttonCalculate.Size = new Size(105, 29);
			buttonCalculate.TabIndex = 6;
			toolTip.SetToolTip(buttonCalculate, "Calculate the ephemerides");
			buttonCalculate.Values.DropDownArrowColor = Color.Empty;
			buttonCalculate.Values.Image = FatcowIcons16px.fatcow_calculator_16px;
			buttonCalculate.Values.Text = "&Calculate";
			buttonCalculate.Click += ButtonCalculate_Click;
			buttonCalculate.Enter += Control_Enter;
			buttonCalculate.Leave += Control_Leave;
			buttonCalculate.MouseEnter += Control_Enter;
			buttonCalculate.MouseLeave += Control_Leave;
			// 
			// listView
			// 
			listView.AccessibleDescription = "Shows the search results";
			listView.AccessibleName = "Search results";
			listView.AccessibleRole = AccessibleRole.List;
			listView.HideSelection = false;
			listView.Location = new Point(14, 203);
			listView.Margin = new Padding(4, 3, 4, 3);
			listView.MultiSelect = false;
			listView.Name = "listView";
			listView.ShowItemToolTips = true;
			listView.Size = new Size(378, 209);
			listView.TabIndex = 9;
			toolTip.SetToolTip(listView, "Search results");
			listView.Enter += Control_Enter;
			listView.Leave += Control_Leave;
			listView.MouseEnter += Control_Enter;
			listView.MouseLeave += Control_Leave;
			// 
			// labelPercent
			// 
			labelPercent.AccessibleDescription = "Shows the percent status of the search";
			labelPercent.AccessibleName = "Percent status of the search";
			labelPercent.AccessibleRole = AccessibleRole.StaticText;
			labelPercent.Location = new Point(341, 162);
			labelPercent.Margin = new Padding(4, 3, 4, 3);
			labelPercent.Name = "labelPercent";
			labelPercent.Size = new Size(44, 20);
			labelPercent.TabIndex = 8;
			toolTip.SetToolTip(labelPercent, "Shows the progress status of the search");
			labelPercent.Values.Text = "100 %";
			labelPercent.Enter += Control_Enter;
			labelPercent.Leave += Control_Leave;
			labelPercent.MouseEnter += Control_Enter;
			labelPercent.MouseLeave += Control_Leave;
			// 
			// progressBar
			// 
			progressBar.AccessibleDescription = "Shows the progress status of the search";
			progressBar.AccessibleName = "Progress bar";
			progressBar.AccessibleRole = AccessibleRole.ProgressBar;
			progressBar.Location = new Point(14, 165);
			progressBar.Margin = new Padding(4, 3, 4, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(320, 20);
			progressBar.TabIndex = 7;
			progressBar.TextBackdropColor = Color.Empty;
			progressBar.TextShadowColor = Color.Empty;
			toolTip.SetToolTip(progressBar, "Shows the progress status of the search");
			progressBar.Values.Text = "";
			progressBar.MouseEnter += Control_Enter;
			progressBar.MouseLeave += Control_Leave;
			// 
			// labelEphemeridesStepsInDays
			// 
			labelEphemeridesStepsInDays.AccessibleRole = AccessibleRole.StaticText;
			labelEphemeridesStepsInDays.Location = new Point(14, 78);
			labelEphemeridesStepsInDays.Margin = new Padding(4, 3, 4, 3);
			labelEphemeridesStepsInDays.Name = "labelEphemeridesStepsInDays";
			labelEphemeridesStepsInDays.Size = new Size(82, 20);
			labelEphemeridesStepsInDays.TabIndex = 4;
			labelEphemeridesStepsInDays.Values.Text = "&Steps in days";
			labelEphemeridesStepsInDays.Enter += Control_Enter;
			labelEphemeridesStepsInDays.Leave += Control_Leave;
			labelEphemeridesStepsInDays.MouseEnter += Control_Enter;
			labelEphemeridesStepsInDays.MouseLeave += Control_Leave;
			// 
			// numericUpDownStepsInDays
			// 
			numericUpDownStepsInDays.AccessibleDescription = "Choose the steps in days";
			numericUpDownStepsInDays.AccessibleName = "Steps in days";
			numericUpDownStepsInDays.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownStepsInDays.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownStepsInDays.Location = new Point(159, 76);
			numericUpDownStepsInDays.Margin = new Padding(4, 3, 4, 3);
			numericUpDownStepsInDays.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownStepsInDays.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownStepsInDays.Name = "numericUpDownStepsInDays";
			numericUpDownStepsInDays.Size = new Size(92, 22);
			numericUpDownStepsInDays.TabIndex = 5;
			toolTip.SetToolTip(numericUpDownStepsInDays, "Steps in days");
			numericUpDownStepsInDays.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownStepsInDays.Enter += Control_Enter;
			numericUpDownStepsInDays.Leave += Control_Leave;
			numericUpDownStepsInDays.MouseEnter += Control_Enter;
			numericUpDownStepsInDays.MouseLeave += Control_Leave;
			// 
			// labelEphemeridesEnd
			// 
			labelEphemeridesEnd.AccessibleDescription = "Sets the end of the ephemerides";
			labelEphemeridesEnd.AccessibleName = "Ephemerides end";
			labelEphemeridesEnd.AccessibleRole = AccessibleRole.StaticText;
			labelEphemeridesEnd.Location = new Point(14, 45);
			labelEphemeridesEnd.Margin = new Padding(4, 3, 4, 3);
			labelEphemeridesEnd.Name = "labelEphemeridesEnd";
			labelEphemeridesEnd.Size = new Size(108, 20);
			labelEphemeridesEnd.TabIndex = 2;
			toolTip.SetToolTip(labelEphemeridesEnd, "Ephemerides end");
			labelEphemeridesEnd.Values.Text = "Ephemerides &end:";
			labelEphemeridesEnd.Enter += Control_Enter;
			labelEphemeridesEnd.Leave += Control_Leave;
			labelEphemeridesEnd.MouseEnter += Control_Enter;
			labelEphemeridesEnd.MouseLeave += Control_Leave;
			// 
			// dateTimePickerEphemeridesEnd
			// 
			dateTimePickerEphemeridesEnd.AccessibleRole = AccessibleRole.DropList;
			dateTimePickerEphemeridesEnd.Location = new Point(159, 44);
			dateTimePickerEphemeridesEnd.Margin = new Padding(4, 3, 4, 3);
			dateTimePickerEphemeridesEnd.Name = "dateTimePickerEphemeridesEnd";
			dateTimePickerEphemeridesEnd.Size = new Size(173, 21);
			dateTimePickerEphemeridesEnd.TabIndex = 3;
			toolTip.SetToolTip(dateTimePickerEphemeridesEnd, "Date of the ephemerides end");
			dateTimePickerEphemeridesEnd.Enter += Control_Enter;
			dateTimePickerEphemeridesEnd.Leave += Control_Leave;
			dateTimePickerEphemeridesEnd.MouseEnter += Control_Enter;
			dateTimePickerEphemeridesEnd.MouseLeave += Control_Leave;
			// 
			// labelEphemeridesBegin
			// 
			labelEphemeridesBegin.AccessibleDescription = "Sets the begin of the ephemerides";
			labelEphemeridesBegin.AccessibleName = "Ephemerides begin";
			labelEphemeridesBegin.AccessibleRole = AccessibleRole.StaticText;
			labelEphemeridesBegin.Location = new Point(14, 14);
			labelEphemeridesBegin.Margin = new Padding(4, 3, 4, 3);
			labelEphemeridesBegin.Name = "labelEphemeridesBegin";
			labelEphemeridesBegin.Size = new Size(118, 20);
			labelEphemeridesBegin.TabIndex = 0;
			toolTip.SetToolTip(labelEphemeridesBegin, "Ephemerides begin");
			labelEphemeridesBegin.Values.Text = "Ephemerides &begin:";
			labelEphemeridesBegin.Enter += Control_Enter;
			labelEphemeridesBegin.Leave += Control_Leave;
			labelEphemeridesBegin.MouseEnter += Control_Enter;
			labelEphemeridesBegin.MouseLeave += Control_Leave;
			// 
			// dateTimePickerEphemeridesBegin
			// 
			dateTimePickerEphemeridesBegin.AccessibleRole = AccessibleRole.DropList;
			dateTimePickerEphemeridesBegin.Location = new Point(159, 13);
			dateTimePickerEphemeridesBegin.Margin = new Padding(4, 3, 4, 3);
			dateTimePickerEphemeridesBegin.Name = "dateTimePickerEphemeridesBegin";
			dateTimePickerEphemeridesBegin.Size = new Size(173, 21);
			dateTimePickerEphemeridesBegin.TabIndex = 1;
			toolTip.SetToolTip(dateTimePickerEphemeridesBegin, "Date of the ephemerides begin");
			dateTimePickerEphemeridesBegin.Enter += Control_Enter;
			dateTimePickerEphemeridesBegin.Leave += Control_Leave;
			dateTimePickerEphemeridesBegin.MouseEnter += Control_Enter;
			dateTimePickerEphemeridesBegin.MouseLeave += Control_Leave;
			// 
			// backgroundWorker
			// 
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// EphemerisForm
			// 
			AccessibleDescription = "Calculates the ephemerides";
			AccessibleName = "Ephemerides";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(406, 453);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "EphemerisForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Ephemerides";
			Load += EphemerisForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			ResumeLayout(false);

		}

		#endregion

		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonPanel panel;
		private KryptonDateTimePicker dateTimePickerEphemeridesBegin;
		private ToolTip toolTip;
		private BackgroundWorker backgroundWorker;
		private KryptonLabel labelEphemeridesEnd;
		private KryptonDateTimePicker dateTimePickerEphemeridesEnd;
		private KryptonLabel labelEphemeridesBegin;
		private KryptonNumericUpDown numericUpDownStepsInDays;
		private KryptonLabel labelEphemeridesStepsInDays;
		private KryptonLabel labelPercent;
		private KryptonProgressBar progressBar;
		private KryptonButton buttonCalculate;
		private KryptonListView listView;
		private KryptonManager kryptonManager;
	}
}