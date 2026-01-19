namespace Planetoid_DB
{
    partial class FileDownloaderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDownloaderForm));
			kryptonManager = new Krypton.Toolkit.KryptonManager(components);
			toolTip = new ToolTip(components);
			tableLayoutPanel = new Krypton.Toolkit.KryptonTableLayoutPanel();
			labelStatusText = new Krypton.Toolkit.KryptonLabel();
			labelSizeValue = new Krypton.Toolkit.KryptonLabel();
			labelSizeText = new Krypton.Toolkit.KryptonLabel();
			labelDateValue = new Krypton.Toolkit.KryptonLabel();
			labelSourceValue = new Krypton.Toolkit.KryptonLabel();
			labelDateText = new Krypton.Toolkit.KryptonLabel();
			labelSourceText = new Krypton.Toolkit.KryptonLabel();
			labelStatusValue = new Krypton.Toolkit.KryptonLabel();
			progressBarDownload = new Krypton.Toolkit.KryptonProgressBar();
			buttonCancel = new Krypton.Toolkit.KryptonButton();
			buttonDownload = new Krypton.Toolkit.KryptonButton();
			statusStrip = new Krypton.Toolkit.KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			panel = new Krypton.Toolkit.KryptonPanel();
			tableLayoutPanel.SuspendLayout();
			statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			SuspendLayout();
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Groups the data";
			tableLayoutPanel.AccessibleName = "Information";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Pane;
			tableLayoutPanel.ColumnCount = 2;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.Controls.Add(labelStatusText, 0, 0);
			tableLayoutPanel.Controls.Add(labelSizeValue, 1, 3);
			tableLayoutPanel.Controls.Add(labelSizeText, 0, 3);
			tableLayoutPanel.Controls.Add(labelDateValue, 1, 1);
			tableLayoutPanel.Controls.Add(labelSourceValue, 1, 2);
			tableLayoutPanel.Controls.Add(labelDateText, 0, 1);
			tableLayoutPanel.Controls.Add(labelSourceText, 0, 2);
			tableLayoutPanel.Controls.Add(labelStatusValue, 1, 0);
			tableLayoutPanel.Dock = DockStyle.Top;
			tableLayoutPanel.Location = new Point(0, 0);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 4;
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.Size = new Size(476, 106);
			tableLayoutPanel.TabIndex = 0;
			toolTip.SetToolTip(tableLayoutPanel, "Groups the data");
			tableLayoutPanel.Enter += SetStatusBar_Enter;
			tableLayoutPanel.Leave += ClearStatusBar_Leave;
			tableLayoutPanel.MouseEnter += SetStatusBar_Enter;
			tableLayoutPanel.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelStatusText
			// 
			labelStatusText.AccessibleDescription = "Status of the download";
			labelStatusText.AccessibleName = "Status";
			labelStatusText.AccessibleRole = AccessibleRole.Text;
			labelStatusText.Dock = DockStyle.Fill;
			labelStatusText.Location = new Point(3, 3);
			labelStatusText.Name = "labelStatusText";
			labelStatusText.Size = new Size(50, 20);
			labelStatusText.TabIndex = 0;
			toolTip.SetToolTip(labelStatusText, "Status");
			labelStatusText.Values.Text = "Status:";
			labelStatusText.DoubleClick += CopyToClipboard_DoubleClick;
			labelStatusText.Enter += SetStatusBar_Enter;
			labelStatusText.Leave += ClearStatusBar_Leave;
			labelStatusText.MouseEnter += SetStatusBar_Enter;
			labelStatusText.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSizeValue
			// 
			labelSizeValue.AccessibleDescription = "Shows the file size of the download";
			labelSizeValue.AccessibleName = "Size of the dowload file";
			labelSizeValue.AccessibleRole = AccessibleRole.Text;
			labelSizeValue.Dock = DockStyle.Fill;
			labelSizeValue.Location = new Point(59, 81);
			labelSizeValue.Name = "labelSizeValue";
			labelSizeValue.Size = new Size(414, 22);
			labelSizeValue.TabIndex = 7;
			toolTip.SetToolTip(labelSizeValue, "Shows the file size of the download");
			labelSizeValue.Values.Text = "...";
			labelSizeValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelSizeValue.Enter += SetStatusBar_Enter;
			labelSizeValue.Leave += ClearStatusBar_Leave;
			labelSizeValue.MouseEnter += SetStatusBar_Enter;
			labelSizeValue.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSizeText
			// 
			labelSizeText.AccessibleDescription = "Shows the file size of the download";
			labelSizeText.AccessibleName = "Size";
			labelSizeText.AccessibleRole = AccessibleRole.Text;
			labelSizeText.Dock = DockStyle.Fill;
			labelSizeText.Location = new Point(3, 81);
			labelSizeText.Name = "labelSizeText";
			labelSizeText.Size = new Size(50, 22);
			labelSizeText.TabIndex = 6;
			toolTip.SetToolTip(labelSizeText, "Size");
			labelSizeText.Values.Text = "Size:";
			labelSizeText.DoubleClick += CopyToClipboard_DoubleClick;
			labelSizeText.Enter += SetStatusBar_Enter;
			labelSizeText.Leave += ClearStatusBar_Leave;
			labelSizeText.MouseEnter += SetStatusBar_Enter;
			labelSizeText.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelDateValue
			// 
			labelDateValue.AccessibleDescription = "Shows the last modified date of the download file";
			labelDateValue.AccessibleName = "Date of the download file";
			labelDateValue.AccessibleRole = AccessibleRole.Text;
			labelDateValue.Dock = DockStyle.Fill;
			labelDateValue.Location = new Point(59, 29);
			labelDateValue.Name = "labelDateValue";
			labelDateValue.Size = new Size(414, 20);
			labelDateValue.TabIndex = 3;
			toolTip.SetToolTip(labelDateValue, "Shows the last modified date of the download");
			labelDateValue.Values.Text = "...";
			labelDateValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateValue.Enter += SetStatusBar_Enter;
			labelDateValue.Leave += ClearStatusBar_Leave;
			labelDateValue.MouseEnter += SetStatusBar_Enter;
			labelDateValue.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSourceValue
			// 
			labelSourceValue.AccessibleDescription = "Shows the download source";
			labelSourceValue.AccessibleName = "Source of the download";
			labelSourceValue.AccessibleRole = AccessibleRole.Text;
			labelSourceValue.Dock = DockStyle.Fill;
			labelSourceValue.Location = new Point(59, 55);
			labelSourceValue.Name = "labelSourceValue";
			labelSourceValue.Size = new Size(414, 20);
			labelSourceValue.TabIndex = 5;
			toolTip.SetToolTip(labelSourceValue, "Shows the download source");
			labelSourceValue.Values.Text = "...";
			labelSourceValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelSourceValue.Enter += SetStatusBar_Enter;
			labelSourceValue.Leave += ClearStatusBar_Leave;
			labelSourceValue.MouseEnter += SetStatusBar_Enter;
			labelSourceValue.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelDateText
			// 
			labelDateText.AccessibleDescription = "Date of the download file";
			labelDateText.AccessibleName = "Date";
			labelDateText.AccessibleRole = AccessibleRole.Text;
			labelDateText.Dock = DockStyle.Fill;
			labelDateText.Location = new Point(3, 29);
			labelDateText.Name = "labelDateText";
			labelDateText.Size = new Size(50, 20);
			labelDateText.TabIndex = 2;
			toolTip.SetToolTip(labelDateText, "Date");
			labelDateText.Values.Text = "Date:";
			labelDateText.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateText.Enter += SetStatusBar_Enter;
			labelDateText.Leave += ClearStatusBar_Leave;
			labelDateText.MouseEnter += SetStatusBar_Enter;
			labelDateText.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSourceText
			// 
			labelSourceText.AccessibleDescription = "Shows the download source";
			labelSourceText.AccessibleName = "Source";
			labelSourceText.AccessibleRole = AccessibleRole.Text;
			labelSourceText.Dock = DockStyle.Fill;
			labelSourceText.Location = new Point(3, 55);
			labelSourceText.Name = "labelSourceText";
			labelSourceText.Size = new Size(50, 20);
			labelSourceText.TabIndex = 4;
			toolTip.SetToolTip(labelSourceText, "Source");
			labelSourceText.Values.Text = "Source:";
			labelSourceText.DoubleClick += CopyToClipboard_DoubleClick;
			labelSourceText.Enter += SetStatusBar_Enter;
			labelSourceText.Leave += ClearStatusBar_Leave;
			labelSourceText.MouseEnter += SetStatusBar_Enter;
			labelSourceText.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelStatusValue
			// 
			labelStatusValue.AccessibleDescription = "Shows the status of the download";
			labelStatusValue.AccessibleName = "Status of the download";
			labelStatusValue.AccessibleRole = AccessibleRole.Text;
			labelStatusValue.Dock = DockStyle.Fill;
			labelStatusValue.Location = new Point(59, 3);
			labelStatusValue.Name = "labelStatusValue";
			labelStatusValue.Size = new Size(414, 20);
			labelStatusValue.TabIndex = 1;
			toolTip.SetToolTip(labelStatusValue, "Shows the status of the download");
			labelStatusValue.Values.Text = "...";
			labelStatusValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelStatusValue.Enter += SetStatusBar_Enter;
			labelStatusValue.Leave += ClearStatusBar_Leave;
			labelStatusValue.MouseEnter += SetStatusBar_Enter;
			labelStatusValue.MouseLeave += ClearStatusBar_Leave;
			// 
			// progressBarDownload
			// 
			progressBarDownload.AccessibleDescription = "Shows the progress of the download";
			progressBarDownload.AccessibleName = "Progress of the download";
			progressBarDownload.AccessibleRole = AccessibleRole.ProgressBar;
			progressBarDownload.Location = new Point(12, 112);
			progressBarDownload.Name = "progressBarDownload";
			progressBarDownload.Size = new Size(452, 14);
			progressBarDownload.TabIndex = 1;
			progressBarDownload.TextBackdropColor = Color.Empty;
			progressBarDownload.TextShadowColor = Color.Empty;
			toolTip.SetToolTip(progressBarDownload, "Shows the progress of the download");
			progressBarDownload.Values.Text = "";
			progressBarDownload.MouseEnter += SetStatusBar_Enter;
			progressBarDownload.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonCancel
			// 
			buttonCancel.AccessibleDescription = "Cancels the download";
			buttonCancel.AccessibleName = "Cancel download";
			buttonCancel.AccessibleRole = AccessibleRole.PushButton;
			buttonCancel.Location = new Point(241, 144);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(128, 36);
			buttonCancel.TabIndex = 3;
			toolTip.SetToolTip(buttonCancel, "Cancel download");
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel download";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += SetStatusBar_Enter;
			buttonCancel.Leave += ClearStatusBar_Leave;
			buttonCancel.MouseEnter += SetStatusBar_Enter;
			buttonCancel.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonDownload
			// 
			buttonDownload.AccessibleDescription = "Downloads the selected file";
			buttonDownload.AccessibleName = "Download";
			buttonDownload.AccessibleRole = AccessibleRole.PushButton;
			buttonDownload.Location = new Point(107, 144);
			buttonDownload.Name = "buttonDownload";
			buttonDownload.Size = new Size(128, 36);
			buttonDownload.TabIndex = 2;
			toolTip.SetToolTip(buttonDownload, "Download");
			buttonDownload.Values.DropDownArrowColor = Color.Empty;
			buttonDownload.Values.Image = Resources.FatcowIcons16px.fatcow_package_go_16px;
			buttonDownload.Values.Text = "&Download";
			buttonDownload.Click += ButtonDownload_Click;
			buttonDownload.Enter += SetStatusBar_Enter;
			buttonDownload.Leave += ClearStatusBar_Leave;
			buttonDownload.MouseEnter += SetStatusBar_Enter;
			buttonDownload.MouseLeave += ClearStatusBar_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Showss some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 196);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(476, 22);
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
			labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
			labelInformation.Margin = new Padding(5, 3, 0, 2);
			labelInformation.Name = "labelInformation";
			labelInformation.Size = new Size(144, 17);
			labelInformation.Text = "some information here";
			labelInformation.ToolTipText = "Shows some information";
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(tableLayoutPanel);
			panel.Controls.Add(progressBarDownload);
			panel.Controls.Add(buttonCancel);
			panel.Controls.Add(buttonDownload);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Name = "panel";
			panel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.FormMain;
			panel.Size = new Size(476, 218);
			panel.TabIndex = 9;
			panel.TabStop = true;
			// 
			// FileDownloaderForm
			// 
			AccessibleDescription = "Downloads the files";
			AccessibleName = "Download files";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(476, 218);
			Controls.Add(statusStrip);
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FileDownloaderForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Downloader";
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		private Krypton.Toolkit.KryptonManager kryptonManager;
		private ToolTip toolTip;
		private Krypton.Toolkit.KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private Krypton.Toolkit.KryptonPanel panel;
		private Krypton.Toolkit.KryptonTableLayoutPanel tableLayoutPanel;
		private Krypton.Toolkit.KryptonLabel labelStatusText;
		private Krypton.Toolkit.KryptonLabel labelSizeValue;
		private Krypton.Toolkit.KryptonLabel labelSizeText;
		private Krypton.Toolkit.KryptonLabel labelDateValue;
		private Krypton.Toolkit.KryptonLabel labelSourceValue;
		private Krypton.Toolkit.KryptonLabel labelDateText;
		private Krypton.Toolkit.KryptonLabel labelSourceText;
		private Krypton.Toolkit.KryptonLabel labelStatusValue;
		private Krypton.Toolkit.KryptonProgressBar progressBarDownload;
		private Krypton.Toolkit.KryptonButton buttonCancel;
		private Krypton.Toolkit.KryptonButton buttonDownload;
	}
}
