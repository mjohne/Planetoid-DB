namespace Planetoid_DB
{
    partial class DatabaseDownloaderForm
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
                components.Dispose();
            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseDownloaderForm));
			kryptonManager = new Krypton.Toolkit.KryptonManager(components);
			tableLayoutPanel = new Krypton.Toolkit.KryptonTableLayoutPanel();
			labelStatusText = new Krypton.Toolkit.KryptonLabel();
			labelSizeValue = new Krypton.Toolkit.KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
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
			contextMenuCopyToClipboard.SuspendLayout();
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
			tableLayoutPanel.Enter += Control_Enter;
			tableLayoutPanel.Leave += Control_Leave;
			tableLayoutPanel.MouseEnter += Control_Enter;
			tableLayoutPanel.MouseLeave += Control_Leave;
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
			labelStatusText.ToolTipValues.Description = "Status of the download";
			labelStatusText.ToolTipValues.EnableToolTips = true;
			labelStatusText.ToolTipValues.Heading = "Status";
			labelStatusText.Values.Text = "Status:";
			labelStatusText.DoubleClick += CopyToClipboard_DoubleClick;
			labelStatusText.Enter += Control_Enter;
			labelStatusText.Leave += Control_Leave;
			labelStatusText.MouseEnter += Control_Enter;
			labelStatusText.MouseLeave += Control_Leave;
			// 
			// labelSizeValue
			// 
			labelSizeValue.AccessibleDescription = "Shows the file size of the download";
			labelSizeValue.AccessibleName = "Size of the dowload file";
			labelSizeValue.AccessibleRole = AccessibleRole.Text;
			labelSizeValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSizeValue.Dock = DockStyle.Fill;
			labelSizeValue.Location = new Point(59, 81);
			labelSizeValue.Name = "labelSizeValue";
			labelSizeValue.Size = new Size(414, 22);
			labelSizeValue.TabIndex = 7;
			labelSizeValue.ToolTipValues.Description = "Shows the file size of the download";
			labelSizeValue.ToolTipValues.EnableToolTips = true;
			labelSizeValue.ToolTipValues.Heading = "Size of the dowload file";
			labelSizeValue.Values.Text = "...";
			labelSizeValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelSizeValue.Enter += Control_Enter;
			labelSizeValue.Leave += Control_Leave;
			labelSizeValue.MouseDown += Control_MouseDown;
			labelSizeValue.MouseEnter += Control_Enter;
			labelSizeValue.MouseLeave += Control_Leave;
			// 
			// contextMenuCopyToClipboard
			// 
			contextMenuCopyToClipboard.AccessibleDescription = "Shows context menu for some options";
			contextMenuCopyToClipboard.AccessibleName = "Some options";
			contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuCopyToClipboard.AllowClickThrough = true;
			contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { ToolStripMenuItemCopyToClipboard });
			contextMenuCopyToClipboard.Name = "contextMenuStrip";
			contextMenuCopyToClipboard.Size = new Size(214, 26);
			contextMenuCopyToClipboard.TabStop = true;
			contextMenuCopyToClipboard.Text = "ContextMenu";
			contextMenuCopyToClipboard.MouseEnter += Control_Enter;
			contextMenuCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// ToolStripMenuItemCopyToClipboard
			// 
			ToolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
			ToolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
			ToolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
			ToolStripMenuItemCopyToClipboard.AutoToolTip = true;
			ToolStripMenuItemCopyToClipboard.Image = Resources.FatcowIcons16px.fatcow_page_copy_16px;
			ToolStripMenuItemCopyToClipboard.Name = "ToolStripMenuItemCopyToClipboard";
			ToolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			ToolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			ToolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
			ToolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
			ToolStripMenuItemCopyToClipboard.Click += CopyToClipboard_DoubleClick;
			ToolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
			ToolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
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
			labelSizeText.ToolTipValues.Description = "Shows the file size of the download";
			labelSizeText.ToolTipValues.EnableToolTips = true;
			labelSizeText.ToolTipValues.Heading = "Size";
			labelSizeText.Values.Text = "Size:";
			labelSizeText.DoubleClick += CopyToClipboard_DoubleClick;
			labelSizeText.Enter += Control_Enter;
			labelSizeText.Leave += Control_Leave;
			labelSizeText.MouseEnter += Control_Enter;
			labelSizeText.MouseLeave += Control_Leave;
			// 
			// labelDateValue
			// 
			labelDateValue.AccessibleDescription = "Shows the last modified date of the download file";
			labelDateValue.AccessibleName = "Date of the download file";
			labelDateValue.AccessibleRole = AccessibleRole.Text;
			labelDateValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDateValue.Dock = DockStyle.Fill;
			labelDateValue.Location = new Point(59, 29);
			labelDateValue.Name = "labelDateValue";
			labelDateValue.Size = new Size(414, 20);
			labelDateValue.TabIndex = 3;
			labelDateValue.ToolTipValues.Description = "Shows the last modified date of the download";
			labelDateValue.ToolTipValues.EnableToolTips = true;
			labelDateValue.ToolTipValues.Heading = "Date of the download file";
			labelDateValue.Values.Text = "...";
			labelDateValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateValue.Enter += Control_Enter;
			labelDateValue.Leave += Control_Leave;
			labelDateValue.MouseDown += Control_MouseDown;
			labelDateValue.MouseEnter += Control_Enter;
			labelDateValue.MouseLeave += Control_Leave;
			// 
			// labelSourceValue
			// 
			labelSourceValue.AccessibleDescription = "Shows the download source";
			labelSourceValue.AccessibleName = "Source of the download";
			labelSourceValue.AccessibleRole = AccessibleRole.Text;
			labelSourceValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSourceValue.Dock = DockStyle.Fill;
			labelSourceValue.Location = new Point(59, 55);
			labelSourceValue.Name = "labelSourceValue";
			labelSourceValue.Size = new Size(414, 20);
			labelSourceValue.TabIndex = 5;
			labelSourceValue.ToolTipValues.Description = "Shows the download source";
			labelSourceValue.ToolTipValues.EnableToolTips = true;
			labelSourceValue.ToolTipValues.Heading = "Source of the download";
			labelSourceValue.Values.Text = "...";
			labelSourceValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelSourceValue.Enter += Control_Enter;
			labelSourceValue.Leave += Control_Leave;
			labelSourceValue.MouseDown += Control_MouseDown;
			labelSourceValue.MouseEnter += Control_Enter;
			labelSourceValue.MouseLeave += Control_Leave;
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
			labelDateText.ToolTipValues.Description = "Date of the download file";
			labelDateText.ToolTipValues.EnableToolTips = true;
			labelDateText.ToolTipValues.Heading = "Date";
			labelDateText.Values.Text = "Date:";
			labelDateText.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateText.Enter += Control_Enter;
			labelDateText.Leave += Control_Leave;
			labelDateText.MouseEnter += Control_Enter;
			labelDateText.MouseLeave += Control_Leave;
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
			labelSourceText.ToolTipValues.Description = "Shows the download source";
			labelSourceText.ToolTipValues.EnableToolTips = true;
			labelSourceText.ToolTipValues.Heading = "Source";
			labelSourceText.Values.Text = "Source:";
			labelSourceText.DoubleClick += CopyToClipboard_DoubleClick;
			labelSourceText.Enter += Control_Enter;
			labelSourceText.Leave += Control_Leave;
			labelSourceText.MouseEnter += Control_Enter;
			labelSourceText.MouseLeave += Control_Leave;
			// 
			// labelStatusValue
			// 
			labelStatusValue.AccessibleDescription = "Shows the status of the download";
			labelStatusValue.AccessibleName = "Status of the download";
			labelStatusValue.AccessibleRole = AccessibleRole.Text;
			labelStatusValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelStatusValue.Dock = DockStyle.Fill;
			labelStatusValue.Location = new Point(59, 3);
			labelStatusValue.Name = "labelStatusValue";
			labelStatusValue.Size = new Size(414, 20);
			labelStatusValue.TabIndex = 1;
			labelStatusValue.ToolTipValues.Description = "Shows the status of the download";
			labelStatusValue.ToolTipValues.EnableToolTips = true;
			labelStatusValue.ToolTipValues.Heading = "Status of the download";
			labelStatusValue.Values.Text = "...";
			labelStatusValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelStatusValue.Enter += Control_Enter;
			labelStatusValue.Leave += Control_Leave;
			labelStatusValue.MouseDown += Control_MouseDown;
			labelStatusValue.MouseEnter += Control_Enter;
			labelStatusValue.MouseLeave += Control_Leave;
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
			progressBarDownload.Values.Text = "";
			progressBarDownload.MouseEnter += Control_Enter;
			progressBarDownload.MouseLeave += Control_Leave;
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
			buttonCancel.ToolTipValues.Description = "Cancels the download";
			buttonCancel.ToolTipValues.EnableToolTips = true;
			buttonCancel.ToolTipValues.Heading = "Cancel download";
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel download";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += Control_Enter;
			buttonCancel.Leave += Control_Leave;
			buttonCancel.MouseEnter += Control_Enter;
			buttonCancel.MouseLeave += Control_Leave;
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
			buttonDownload.ToolTipValues.Description = "Downloads the selected file";
			buttonDownload.ToolTipValues.EnableToolTips = true;
			buttonDownload.ToolTipValues.Heading = "Download";
			buttonDownload.Values.DropDownArrowColor = Color.Empty;
			buttonDownload.Values.Image = Resources.FatcowIcons16px.fatcow_package_go_16px;
			buttonDownload.Values.Text = "&Download";
			buttonDownload.Click += ButtonDownload_Click;
			buttonDownload.Enter += Control_Enter;
			buttonDownload.Leave += Control_Leave;
			buttonDownload.MouseEnter += Control_Enter;
			buttonDownload.MouseLeave += Control_Leave;
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
			// DatabaseDownloaderForm
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
			Name = "DatabaseDownloaderForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Downloader";
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			contextMenuCopyToClipboard.ResumeLayout(false);
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		private Krypton.Toolkit.KryptonManager kryptonManager;
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
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
	}
}
