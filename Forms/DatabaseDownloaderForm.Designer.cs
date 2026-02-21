using Planetoid_DB.Resources;

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
			toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			labelSizeText = new Krypton.Toolkit.KryptonLabel();
			labelDateValue = new Krypton.Toolkit.KryptonLabel();
			labelSourceValue = new Krypton.Toolkit.KryptonLabel();
			labelDateText = new Krypton.Toolkit.KryptonLabel();
			labelSourceText = new Krypton.Toolkit.KryptonLabel();
			labelStatusValue = new Krypton.Toolkit.KryptonLabel();
			labelDownloadSpeed = new Krypton.Toolkit.KryptonLabel();
			labelDownloadSpeedValue = new Krypton.Toolkit.KryptonLabel();
			labelTimeValue = new Krypton.Toolkit.KryptonLabel();
			labelTime = new Krypton.Toolkit.KryptonLabel();
			statusStrip = new Krypton.Toolkit.KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			panel = new Krypton.Toolkit.KryptonPanel();
			toolStripContainer = new ToolStripContainer();
			kryptonToolStripIcons = new Krypton.Toolkit.KryptonToolStrip();
			toolStripButtonDownload = new ToolStripButton();
			toolStripButtonCancel = new ToolStripButton();
			toolStripSeparator = new ToolStripSeparator();
			kryptonProgressBarDownload = new Krypton.Toolkit.KryptonProgressBarToolStripItem();
			tableLayoutPanel.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			kryptonToolStripIcons.SuspendLayout();
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
			tableLayoutPanel.Controls.Add(labelDownloadSpeed, 0, 6);
			tableLayoutPanel.Controls.Add(labelDownloadSpeedValue, 1, 6);
			tableLayoutPanel.Controls.Add(labelTimeValue, 1, 5);
			tableLayoutPanel.Controls.Add(labelTime, 0, 5);
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.Location = new Point(0, 0);
			tableLayoutPanel.Margin = new Padding(0);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 7;
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.Size = new Size(530, 166);
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
			labelStatusText.Size = new Size(105, 20);
			labelStatusText.TabIndex = 0;
			labelStatusText.ToolTipValues.Description = "Status of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelStatusText.ToolTipValues.EnableToolTips = true;
			labelStatusText.ToolTipValues.Heading = "Status";
			labelStatusText.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelSizeValue.Location = new Point(114, 81);
			labelSizeValue.Name = "labelSizeValue";
			labelSizeValue.Size = new Size(414, 22);
			labelSizeValue.TabIndex = 7;
			labelSizeValue.ToolTipValues.Description = "Shows the file size of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSizeValue.ToolTipValues.EnableToolTips = true;
			labelSizeValue.ToolTipValues.Heading = "Size of the dowload file";
			labelSizeValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			contextMenuCopyToClipboard.AccessibleDescription = "Shows the context menu for copying information to the clipboard";
			contextMenuCopyToClipboard.AccessibleName = "Context menu for copying database information to the clipboard";
			contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuCopyToClipboard.AllowClickThrough = true;
			contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCopyToClipboard });
			contextMenuCopyToClipboard.Name = "contextMenuStrip";
			contextMenuCopyToClipboard.Size = new Size(214, 26);
			contextMenuCopyToClipboard.TabStop = true;
			contextMenuCopyToClipboard.Text = "Copy to clipboard";
			contextMenuCopyToClipboard.MouseEnter += Control_Enter;
			contextMenuCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemCopyToClipboard
			// 
			toolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
			toolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
			toolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemCopyToClipboard.AutoToolTip = true;
			toolStripMenuItemCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			toolStripMenuItemCopyToClipboard.Name = "toolStripMenuItemCopyToClipboard";
			toolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			toolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			toolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
			toolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
			toolStripMenuItemCopyToClipboard.Click += CopyToClipboard_DoubleClick;
			toolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
			toolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// labelSizeText
			// 
			labelSizeText.AccessibleDescription = "Shows the file size of the download";
			labelSizeText.AccessibleName = "Size";
			labelSizeText.AccessibleRole = AccessibleRole.Text;
			labelSizeText.Dock = DockStyle.Fill;
			labelSizeText.Location = new Point(3, 81);
			labelSizeText.Name = "labelSizeText";
			labelSizeText.Size = new Size(105, 22);
			labelSizeText.TabIndex = 6;
			labelSizeText.ToolTipValues.Description = "Shows the file size of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSizeText.ToolTipValues.EnableToolTips = true;
			labelSizeText.ToolTipValues.Heading = "Size";
			labelSizeText.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelDateValue.Location = new Point(114, 29);
			labelDateValue.Name = "labelDateValue";
			labelDateValue.Size = new Size(414, 20);
			labelDateValue.TabIndex = 3;
			labelDateValue.ToolTipValues.Description = "Shows the last modified date of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateValue.ToolTipValues.EnableToolTips = true;
			labelDateValue.ToolTipValues.Heading = "Date of the download file";
			labelDateValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelSourceValue.Location = new Point(114, 55);
			labelSourceValue.Name = "labelSourceValue";
			labelSourceValue.Size = new Size(414, 20);
			labelSourceValue.TabIndex = 5;
			labelSourceValue.ToolTipValues.Description = "Shows the download source.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSourceValue.ToolTipValues.EnableToolTips = true;
			labelSourceValue.ToolTipValues.Heading = "Source of the download";
			labelSourceValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelDateText.Size = new Size(105, 20);
			labelDateText.TabIndex = 2;
			labelDateText.ToolTipValues.Description = "Date of the download file.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateText.ToolTipValues.EnableToolTips = true;
			labelDateText.ToolTipValues.Heading = "Date";
			labelDateText.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelSourceText.Size = new Size(105, 20);
			labelSourceText.TabIndex = 4;
			labelSourceText.ToolTipValues.Description = "Shows the download source.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSourceText.ToolTipValues.EnableToolTips = true;
			labelSourceText.ToolTipValues.Heading = "Source";
			labelSourceText.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelStatusValue.Location = new Point(114, 3);
			labelStatusValue.Name = "labelStatusValue";
			labelStatusValue.Size = new Size(414, 20);
			labelStatusValue.TabIndex = 1;
			labelStatusValue.ToolTipValues.Description = "Shows the status of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelStatusValue.ToolTipValues.EnableToolTips = true;
			labelStatusValue.ToolTipValues.Heading = "Status of the download";
			labelStatusValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelStatusValue.Values.Text = "...";
			labelStatusValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelStatusValue.Enter += Control_Enter;
			labelStatusValue.Leave += Control_Leave;
			labelStatusValue.MouseDown += Control_MouseDown;
			labelStatusValue.MouseEnter += Control_Enter;
			labelStatusValue.MouseLeave += Control_Leave;
			// 
			// labelDownloadSpeed
			// 
			labelDownloadSpeed.AccessibleDescription = "Shows the download speed";
			labelDownloadSpeed.AccessibleName = "Download speed";
			labelDownloadSpeed.AccessibleRole = AccessibleRole.Text;
			labelDownloadSpeed.Dock = DockStyle.Fill;
			labelDownloadSpeed.Location = new Point(3, 140);
			labelDownloadSpeed.Name = "labelDownloadSpeed";
			labelDownloadSpeed.Size = new Size(105, 24);
			labelDownloadSpeed.TabIndex = 10;
			labelDownloadSpeed.ToolTipValues.Description = "Shows the download speed.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDownloadSpeed.ToolTipValues.EnableToolTips = true;
			labelDownloadSpeed.ToolTipValues.Heading = "Download speed";
			labelDownloadSpeed.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDownloadSpeed.Values.Text = "Download speed:";
			labelDownloadSpeed.DoubleClick += CopyToClipboard_DoubleClick;
			labelDownloadSpeed.Enter += Control_Enter;
			labelDownloadSpeed.Leave += Control_Leave;
			labelDownloadSpeed.MouseEnter += Control_Enter;
			labelDownloadSpeed.MouseLeave += Control_Leave;
			// 
			// labelDownloadSpeedValue
			// 
			labelDownloadSpeedValue.AccessibleDescription = "Shows the download speed";
			labelDownloadSpeedValue.AccessibleName = "Download speed value";
			labelDownloadSpeedValue.AccessibleRole = AccessibleRole.Text;
			labelDownloadSpeedValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDownloadSpeedValue.Dock = DockStyle.Fill;
			labelDownloadSpeedValue.Location = new Point(114, 140);
			labelDownloadSpeedValue.Name = "labelDownloadSpeedValue";
			labelDownloadSpeedValue.Size = new Size(414, 24);
			labelDownloadSpeedValue.TabIndex = 11;
			labelDownloadSpeedValue.ToolTipValues.Description = "Shows the download speed.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDownloadSpeedValue.ToolTipValues.EnableToolTips = true;
			labelDownloadSpeedValue.ToolTipValues.Heading = "Download speed value";
			labelDownloadSpeedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDownloadSpeedValue.Values.Text = "...";
			labelDownloadSpeedValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDownloadSpeedValue.Enter += Control_Enter;
			labelDownloadSpeedValue.Leave += Control_Leave;
			labelDownloadSpeedValue.MouseDown += Control_MouseDown;
			labelDownloadSpeedValue.MouseEnter += Control_Enter;
			labelDownloadSpeedValue.MouseLeave += Control_Leave;
			// 
			// labelTimeValue
			// 
			labelTimeValue.AccessibleDescription = "Shows the elapsed time and estimated time of the download";
			labelTimeValue.AccessibleName = "Time value";
			labelTimeValue.AccessibleRole = AccessibleRole.Text;
			labelTimeValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelTimeValue.Dock = DockStyle.Fill;
			labelTimeValue.Location = new Point(114, 109);
			labelTimeValue.Name = "labelTimeValue";
			labelTimeValue.Size = new Size(414, 25);
			labelTimeValue.TabIndex = 9;
			labelTimeValue.ToolTipValues.Description = "Shows the elapsed time and estimated time of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelTimeValue.ToolTipValues.EnableToolTips = true;
			labelTimeValue.ToolTipValues.Heading = "Time value";
			labelTimeValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelTimeValue.Values.Text = "...";
			labelTimeValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelTimeValue.Enter += Control_Enter;
			labelTimeValue.Leave += Control_Leave;
			labelTimeValue.MouseDown += Control_MouseDown;
			labelTimeValue.MouseEnter += Control_Enter;
			labelTimeValue.MouseLeave += Control_Leave;
			// 
			// labelTime
			// 
			labelTime.AccessibleDescription = "Shows the elapsed time and estimated time of the download";
			labelTime.AccessibleName = "Time";
			labelTime.AccessibleRole = AccessibleRole.Text;
			labelTime.Dock = DockStyle.Fill;
			labelTime.Location = new Point(3, 109);
			labelTime.Name = "labelTime";
			labelTime.Size = new Size(105, 25);
			labelTime.TabIndex = 8;
			labelTime.ToolTipValues.Description = "Shows the elapsed time and estimated time of the download.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelTime.ToolTipValues.EnableToolTips = true;
			labelTime.ToolTipValues.Heading = "Time";
			labelTime.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelTime.Values.Text = "Time (estimated):";
			labelTime.DoubleClick += CopyToClipboard_DoubleClick;
			labelTime.Enter += Control_Enter;
			labelTime.Leave += Control_Leave;
			labelTime.MouseEnter += Control_Enter;
			labelTime.MouseLeave += Control_Leave;
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
			statusStrip.Size = new Size(530, 22);
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
			labelInformation.ToolTipText = "Shows some information";
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(tableLayoutPanel);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Name = "panel";
			panel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.FormMain;
			panel.Size = new Size(530, 166);
			panel.TabIndex = 9;
			panel.TabStop = true;
			// 
			// toolStripContainer
			// 
			toolStripContainer.AccessibleDescription = "Container";
			toolStripContainer.AccessibleName = "Container";
			toolStripContainer.AccessibleRole = AccessibleRole.Pane;
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(panel);
			toolStripContainer.ContentPanel.Size = new Size(530, 166);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(530, 213);
			toolStripContainer.TabIndex = 11;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
			// 
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonDownload, toolStripButtonCancel, toolStripSeparator, kryptonProgressBarDownload });
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(530, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			// 
			// toolStripButtonDownload
			// 
			toolStripButtonDownload.AccessibleDescription = "Downloads the selected file";
			toolStripButtonDownload.AccessibleName = "Download";
			toolStripButtonDownload.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonDownload.Image = FatcowIcons16px.fatcow_package_go_16px;
			toolStripButtonDownload.ImageTransparentColor = Color.Magenta;
			toolStripButtonDownload.Name = "toolStripButtonDownload";
			toolStripButtonDownload.Size = new Size(81, 22);
			toolStripButtonDownload.Text = "&Download";
			toolStripButtonDownload.Click += ButtonDownload_Click;
			toolStripButtonDownload.MouseEnter += Control_Enter;
			toolStripButtonDownload.MouseLeave += Control_Leave;
			// 
			// toolStripButtonCancel
			// 
			toolStripButtonCancel.AccessibleDescription = "Cancels the download";
			toolStripButtonCancel.AccessibleName = "Cancel download";
			toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
			toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
			toolStripButtonCancel.Name = "toolStripButtonCancel";
			toolStripButtonCancel.Size = new Size(119, 22);
			toolStripButtonCancel.Text = "&Cancel download";
			toolStripButtonCancel.Click += ButtonCancel_Click;
			toolStripButtonCancel.MouseEnter += Control_Enter;
			toolStripButtonCancel.MouseLeave += Control_Leave;
			// 
			// toolStripSeparator
			// 
			toolStripSeparator.AccessibleDescription = "Just a separator";
			toolStripSeparator.AccessibleName = "Just a separator";
			toolStripSeparator.AccessibleRole = AccessibleRole.Separator;
			toolStripSeparator.Name = "toolStripSeparator";
			toolStripSeparator.Size = new Size(6, 25);
			// 
			// kryptonProgressBarDownload
			// 
			kryptonProgressBarDownload.AccessibleDescription = "Shows the progress of the download";
			kryptonProgressBarDownload.AccessibleName = "ProgressBar";
			kryptonProgressBarDownload.Name = "kryptonProgressBarDownload";
			kryptonProgressBarDownload.Size = new Size(290, 22);
			kryptonProgressBarDownload.StateCommon.Back.Color1 = Color.Green;
			kryptonProgressBarDownload.StateDisabled.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
			kryptonProgressBarDownload.StateNormal.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
			kryptonProgressBarDownload.Values.Text = "";
			// 
			// DatabaseDownloaderForm
			// 
			AccessibleDescription = "Downloads the files";
			AccessibleName = "Download files";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(530, 213);
			Controls.Add(toolStripContainer);
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
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			kryptonToolStripIcons.ResumeLayout(false);
			kryptonToolStripIcons.PerformLayout();
			ResumeLayout(false);
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
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
		private ToolStripContainer toolStripContainer;
		private Krypton.Toolkit.KryptonToolStrip kryptonToolStripIcons;
		private ToolStripButton toolStripButtonDownload;
		private ToolStripButton toolStripButtonCancel;
		private ToolStripSeparator toolStripSeparator;
		private Krypton.Toolkit.KryptonProgressBarToolStripItem kryptonProgressBarDownload;
		private Krypton.Toolkit.KryptonLabel labelDownloadSpeed;
		private Krypton.Toolkit.KryptonLabel labelDownloadSpeedValue;
		private Krypton.Toolkit.KryptonLabel labelTimeValue;
		private Krypton.Toolkit.KryptonLabel labelTime;
	}
}
