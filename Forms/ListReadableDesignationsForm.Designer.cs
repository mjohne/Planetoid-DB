using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class ListReadableDesignationsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ListReadableDesignationsForm));
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			backgroundWorker = new BackgroundWorker();
			toolTip = new ToolTip(components);
			buttonCancel = new KryptonButton();
			progressBar = new KryptonProgressBar();
			buttonList = new KryptonButton();
			labelWarning = new Label();
			buttonLoad = new KryptonButton();
			labelMinimum = new KryptonLabel();
			numericUpDownMinimum = new KryptonNumericUpDown();
			numericUpDownMaximum = new KryptonNumericUpDown();
			labelMaximum = new KryptonLabel();
			contextMenuSaveList = new ContextMenuStrip(components);
			toolStripMenuItemSaveAsCsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsHtml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsXml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsJson = new ToolStripMenuItem();
			dropButtonSaveList = new KryptonDropButton();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCpyToClipboard = new ToolStripMenuItem();
			panel = new KryptonPanel();
			listView = new ListView();
			columnHeaderIndex = new ColumnHeader();
			columnHeaderReadableDesignation = new ColumnHeader();
			saveFileDialogCsv = new SaveFileDialog();
			saveFileDialogJson = new SaveFileDialog();
			saveFileDialogHtml = new SaveFileDialog();
			saveFileDialogXml = new SaveFileDialog();
			kryptonManager = new KryptonManager(components);
			statusStrip.SuspendLayout();
			contextMenuSaveList.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 393);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(312, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 1;
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
			// backgroundWorker
			// 
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// buttonCancel
			// 
			buttonCancel.AccessibleDescription = "Cancels the progress";
			buttonCancel.AccessibleName = "Cancel";
			buttonCancel.AccessibleRole = AccessibleRole.PushButton;
			buttonCancel.Location = new Point(70, 38);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(69, 31);
			buttonCancel.TabIndex = 5;
			toolTip.SetToolTip(buttonCancel, "Cancel the progress");
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += SetStatusBar_Enter;
			buttonCancel.Leave += ClearStatusBar_Leave;
			buttonCancel.MouseEnter += SetStatusBar_Enter;
			buttonCancel.MouseLeave += ClearStatusBar_Leave;
			// 
			// progressBar
			// 
			progressBar.AccessibleDescription = "Shows the progress";
			progressBar.AccessibleName = "Progress";
			progressBar.AccessibleRole = AccessibleRole.ProgressBar;
			progressBar.Location = new Point(12, 75);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(288, 19);
			progressBar.Step = 1;
			progressBar.TabIndex = 8;
			progressBar.TextBackdropColor = Color.Empty;
			progressBar.TextShadowColor = Color.Empty;
			toolTip.SetToolTip(progressBar, "Shows the progress");
			progressBar.Values.Text = "";
			progressBar.MouseEnter += SetStatusBar_Enter;
			progressBar.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonList
			// 
			buttonList.AccessibleDescription = "Starts the progress and list";
			buttonList.AccessibleName = "List";
			buttonList.AccessibleRole = AccessibleRole.PushButton;
			buttonList.Location = new Point(12, 38);
			buttonList.Name = "buttonList";
			buttonList.Size = new Size(52, 31);
			buttonList.TabIndex = 4;
			toolTip.SetToolTip(buttonList, "Start the progress and list");
			buttonList.Values.DropDownArrowColor = Color.Empty;
			buttonList.Values.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			buttonList.Values.Text = "&List";
			buttonList.Click += ButtonList_Click;
			buttonList.Enter += SetStatusBar_Enter;
			buttonList.Leave += ClearStatusBar_Leave;
			buttonList.MouseEnter += SetStatusBar_Enter;
			buttonList.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelWarning
			// 
			labelWarning.AccessibleDescription = "Warning message: Be careful: do not use large ranges between minimum and maximum! This can increase loading time and memory. Use small spans!";
			labelWarning.AccessibleName = "Warning message";
			labelWarning.AccessibleRole = AccessibleRole.Text;
			labelWarning.BackColor = Color.SeaShell;
			labelWarning.BorderStyle = BorderStyle.Fixed3D;
			labelWarning.ContextMenuStrip = contextMenuCopyToClipboard;
			labelWarning.Font = new Font("Segoe UI", 7F);
			labelWarning.Location = new Point(12, 97);
			labelWarning.Name = "labelWarning";
			labelWarning.Size = new Size(288, 35);
			labelWarning.TabIndex = 9;
			labelWarning.Text = "Be careful: This can increase loading time and memory. You can cancel any time.";
			labelWarning.TextAlign = ContentAlignment.MiddleLeft;
			toolTip.SetToolTip(labelWarning, "Be careful: do not use large ranges between minimum and maximum! This can increase loading time and memory. Use small spans!");
			labelWarning.DoubleClick += CopyToClipboard_DoubleClick;
			labelWarning.Enter += SetStatusBar_Enter;
			labelWarning.Leave += ClearStatusBar_Leave;
			labelWarning.MouseDown += Control_MouseDown;
			labelWarning.MouseEnter += SetStatusBar_Enter;
			labelWarning.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonLoad
			// 
			buttonLoad.AccessibleDescription = "Load the selected planetoid";
			buttonLoad.AccessibleName = "Load";
			buttonLoad.AccessibleRole = AccessibleRole.PushButton;
			buttonLoad.DialogResult = DialogResult.OK;
			buttonLoad.Location = new Point(145, 38);
			buttonLoad.Name = "buttonLoad";
			buttonLoad.Size = new Size(56, 31);
			buttonLoad.TabIndex = 6;
			toolTip.SetToolTip(buttonLoad, "Load the selected planetoid");
			buttonLoad.Values.DropDownArrowColor = Color.Empty;
			buttonLoad.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonLoad.Values.Text = "L&oad";
			buttonLoad.Enter += SetStatusBar_Enter;
			buttonLoad.Leave += ClearStatusBar_Leave;
			buttonLoad.MouseEnter += SetStatusBar_Enter;
			buttonLoad.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMinimum
			// 
			labelMinimum.AccessibleDescription = "Shows the minimum";
			labelMinimum.AccessibleName = "Minimum";
			labelMinimum.AccessibleRole = AccessibleRole.Text;
			labelMinimum.Location = new Point(12, 12);
			labelMinimum.Name = "labelMinimum";
			labelMinimum.Size = new Size(66, 20);
			labelMinimum.TabIndex = 0;
			toolTip.SetToolTip(labelMinimum, "Minimum");
			labelMinimum.Values.Text = "M&inimum:";
			labelMinimum.Click += CopyToClipboard_DoubleClick;
			labelMinimum.Enter += SetStatusBar_Enter;
			labelMinimum.Leave += ClearStatusBar_Leave;
			labelMinimum.MouseEnter += SetStatusBar_Enter;
			labelMinimum.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimum
			// 
			numericUpDownMinimum.AccessibleDescription = "Shows the minimum value";
			numericUpDownMinimum.AccessibleName = "Minimum value";
			numericUpDownMinimum.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimum.Location = new Point(84, 10);
			numericUpDownMinimum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimum.Name = "numericUpDownMinimum";
			numericUpDownMinimum.Size = new Size(64, 22);
			numericUpDownMinimum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
			numericUpDownMinimum.TabIndex = 1;
			toolTip.SetToolTip(numericUpDownMinimum, "Minimum value for the list");
			numericUpDownMinimum.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimum.Enter += SetStatusBar_Enter;
			numericUpDownMinimum.Leave += ClearStatusBar_Leave;
			numericUpDownMinimum.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimum.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximum
			// 
			numericUpDownMaximum.AccessibleDescription = "Shows the maximum value";
			numericUpDownMaximum.AccessibleName = "Maximum value";
			numericUpDownMaximum.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximum.Location = new Point(226, 10);
			numericUpDownMaximum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximum.Name = "numericUpDownMaximum";
			numericUpDownMaximum.Size = new Size(64, 22);
			numericUpDownMaximum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
			numericUpDownMaximum.TabIndex = 3;
			toolTip.SetToolTip(numericUpDownMaximum, "Maximum value for the list");
			numericUpDownMaximum.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximum.Enter += SetStatusBar_Enter;
			numericUpDownMaximum.Leave += ClearStatusBar_Leave;
			numericUpDownMaximum.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximum.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMaximum
			// 
			labelMaximum.AccessibleDescription = "Shows the maximum";
			labelMaximum.AccessibleName = "Maximum";
			labelMaximum.AccessibleRole = AccessibleRole.Text;
			labelMaximum.Location = new Point(154, 12);
			labelMaximum.Name = "labelMaximum";
			labelMaximum.Size = new Size(68, 20);
			labelMaximum.TabIndex = 2;
			toolTip.SetToolTip(labelMaximum, "Maximum");
			labelMaximum.Values.Text = "M&aximum:";
			labelMaximum.Click += CopyToClipboard_DoubleClick;
			labelMaximum.Enter += SetStatusBar_Enter;
			labelMaximum.Leave += ClearStatusBar_Leave;
			labelMaximum.MouseEnter += SetStatusBar_Enter;
			labelMaximum.MouseLeave += ClearStatusBar_Leave;
			// 
			// contextMenuSaveList
			// 
			contextMenuSaveList.AccessibleDescription = "Save the list as file";
			contextMenuSaveList.AccessibleName = "Save list";
			contextMenuSaveList.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuSaveList.Font = new Font("Segoe UI", 9F);
			contextMenuSaveList.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsJson });
			contextMenuSaveList.Name = "contextMenuStrip1";
			contextMenuSaveList.Size = new Size(148, 92);
			contextMenuSaveList.TabStop = true;
			contextMenuSaveList.Text = "&Save List";
			toolTip.SetToolTip(contextMenuSaveList, "Save List");
			contextMenuSaveList.MouseEnter += SetStatusBar_Enter;
			contextMenuSaveList.MouseLeave += ClearStatusBar_Leave;
			// 
			// toolStripMenuItemSaveAsCsv
			// 
			toolStripMenuItemSaveAsCsv.AccessibleDescription = "Save the list as CSV file";
			toolStripMenuItemSaveAsCsv.AccessibleName = "Save as CSV";
			toolStripMenuItemSaveAsCsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsCsv.AutoToolTip = true;
			toolStripMenuItemSaveAsCsv.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsCsv.Name = "toolStripMenuItemSaveAsCsv";
			toolStripMenuItemSaveAsCsv.Size = new Size(147, 22);
			toolStripMenuItemSaveAsCsv.Text = "Save as &CSV";
			toolStripMenuItemSaveAsCsv.Click += ToolStripMenuItemSaveAsCsv_Click;
			toolStripMenuItemSaveAsCsv.MouseEnter += SetStatusBar_Enter;
			toolStripMenuItemSaveAsCsv.MouseLeave += ClearStatusBar_Leave;
			// 
			// toolStripMenuItemSaveAsHtml
			// 
			toolStripMenuItemSaveAsHtml.AccessibleDescription = "Save the list as HTML file";
			toolStripMenuItemSaveAsHtml.AccessibleName = "Save as HTML";
			toolStripMenuItemSaveAsHtml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsHtml.AutoToolTip = true;
			toolStripMenuItemSaveAsHtml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsHtml.Name = "toolStripMenuItemSaveAsHtml";
			toolStripMenuItemSaveAsHtml.Size = new Size(147, 22);
			toolStripMenuItemSaveAsHtml.Text = "Save as &HTML";
			toolStripMenuItemSaveAsHtml.Click += ToolStripMenuItemSaveAsHtml_Click;
			toolStripMenuItemSaveAsHtml.MouseEnter += SetStatusBar_Enter;
			toolStripMenuItemSaveAsHtml.MouseLeave += ClearStatusBar_Leave;
			// 
			// toolStripMenuItemSaveAsXml
			// 
			toolStripMenuItemSaveAsXml.AccessibleDescription = "Save the list as XML file";
			toolStripMenuItemSaveAsXml.AccessibleName = "Save as XML";
			toolStripMenuItemSaveAsXml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsXml.AutoToolTip = true;
			toolStripMenuItemSaveAsXml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsXml.Name = "toolStripMenuItemSaveAsXml";
			toolStripMenuItemSaveAsXml.Size = new Size(147, 22);
			toolStripMenuItemSaveAsXml.Text = "Save as &XML";
			toolStripMenuItemSaveAsXml.Click += ToolStripMenuItemSaveAsXml_Click;
			toolStripMenuItemSaveAsXml.MouseEnter += SetStatusBar_Enter;
			toolStripMenuItemSaveAsXml.MouseLeave += ClearStatusBar_Leave;
			// 
			// toolStripMenuItemSaveAsJson
			// 
			toolStripMenuItemSaveAsJson.AccessibleDescription = "Save the list as JSON file";
			toolStripMenuItemSaveAsJson.AccessibleName = "Save as JSON";
			toolStripMenuItemSaveAsJson.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsJson.AutoToolTip = true;
			toolStripMenuItemSaveAsJson.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
			toolStripMenuItemSaveAsJson.Size = new Size(147, 22);
			toolStripMenuItemSaveAsJson.Text = "Save as &JSON";
			toolStripMenuItemSaveAsJson.Click += ToolStripMenuItemSaveAsJson_Click;
			toolStripMenuItemSaveAsJson.MouseEnter += SetStatusBar_Enter;
			toolStripMenuItemSaveAsJson.MouseLeave += ClearStatusBar_Leave;
			// 
			// dropButtonSaveList
			// 
			dropButtonSaveList.AccessibleDescription = "Saves the list as file";
			dropButtonSaveList.AccessibleName = "Save list";
			dropButtonSaveList.AccessibleRole = AccessibleRole.ButtonDropDown;
			dropButtonSaveList.ContextMenuStrip = contextMenuSaveList;
			dropButtonSaveList.Location = new Point(207, 38);
			dropButtonSaveList.Name = "dropButtonSaveList";
			dropButtonSaveList.Size = new Size(93, 31);
			dropButtonSaveList.Splitter = false;
			dropButtonSaveList.TabIndex = 7;
			toolTip.SetToolTip(dropButtonSaveList, "Save List");
			dropButtonSaveList.Values.DropDownArrowColor = Color.Empty;
			dropButtonSaveList.Values.ImageStates.ImageCheckedNormal = null;
			dropButtonSaveList.Values.ImageStates.ImageCheckedPressed = null;
			dropButtonSaveList.Values.ImageStates.ImageCheckedTracking = null;
			dropButtonSaveList.Values.ImageStates.ImageDisabled = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImageNormal = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImagePressed = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImageTracking = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.Text = "&Save List";
			dropButtonSaveList.Enter += SetStatusBar_Enter;
			dropButtonSaveList.Leave += ClearStatusBar_Leave;
			dropButtonSaveList.MouseEnter += SetStatusBar_Enter;
			dropButtonSaveList.MouseLeave += ClearStatusBar_Leave;
			// 
			// contextMenuCopyToClipboard
			// 
			contextMenuCopyToClipboard.AccessibleDescription = "Shows context menu for some options";
			contextMenuCopyToClipboard.AccessibleName = "Some options";
			contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuCopyToClipboard.AllowClickThrough = true;
			contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { ToolStripMenuItemCpyToClipboard });
			contextMenuCopyToClipboard.Name = "contextMenuStrip";
			contextMenuCopyToClipboard.Size = new Size(214, 26);
			contextMenuCopyToClipboard.TabStop = true;
			contextMenuCopyToClipboard.Text = "ContextMenu";
			toolTip.SetToolTip(contextMenuCopyToClipboard, "Context menu for copying to clipboard");
			contextMenuCopyToClipboard.MouseEnter += SetStatusBar_Enter;
			contextMenuCopyToClipboard.MouseLeave += ClearStatusBar_Leave;
			// 
			// ToolStripMenuItemCpyToClipboard
			// 
			ToolStripMenuItemCpyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
			ToolStripMenuItemCpyToClipboard.AccessibleName = "Copy to clipboard";
			ToolStripMenuItemCpyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
			ToolStripMenuItemCpyToClipboard.AutoToolTip = true;
			ToolStripMenuItemCpyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			ToolStripMenuItemCpyToClipboard.Name = "ToolStripMenuItemCpyToClipboard";
			ToolStripMenuItemCpyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			ToolStripMenuItemCpyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			ToolStripMenuItemCpyToClipboard.Size = new Size(213, 22);
			ToolStripMenuItemCpyToClipboard.Text = "&Copy to clipboard";
			ToolStripMenuItemCpyToClipboard.Click += CopyToClipboard_DoubleClick;
			ToolStripMenuItemCpyToClipboard.MouseEnter += SetStatusBar_Enter;
			ToolStripMenuItemCpyToClipboard.MouseLeave += ClearStatusBar_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(dropButtonSaveList);
			panel.Controls.Add(labelMinimum);
			panel.Controls.Add(numericUpDownMinimum);
			panel.Controls.Add(numericUpDownMaximum);
			panel.Controls.Add(labelMaximum);
			panel.Controls.Add(buttonLoad);
			panel.Controls.Add(labelWarning);
			panel.Controls.Add(buttonCancel);
			panel.Controls.Add(listView);
			panel.Controls.Add(progressBar);
			panel.Controls.Add(buttonList);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(312, 393);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// listView
			// 
			listView.AccessibleDescription = "Shows the list with the items";
			listView.AccessibleName = "List";
			listView.AccessibleRole = AccessibleRole.ListItem;
			listView.Activation = ItemActivation.OneClick;
			listView.AllowColumnReorder = true;
			listView.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderReadableDesignation });
			listView.Font = new Font("Segoe UI", 8.5F);
			listView.FullRowSelect = true;
			listView.GridLines = true;
			listView.Location = new Point(12, 135);
			listView.MultiSelect = false;
			listView.Name = "listView";
			listView.ShowItemToolTips = true;
			listView.Size = new Size(288, 255);
			listView.TabIndex = 10;
			listView.UseCompatibleStateImageBehavior = false;
			listView.View = View.Details;
			listView.SelectedIndexChanged += SelectedIndexChanged;
			listView.Enter += SetStatusBar_Enter;
			listView.Leave += ClearStatusBar_Leave;
			listView.MouseEnter += SetStatusBar_Enter;
			listView.MouseLeave += ClearStatusBar_Leave;
			// 
			// columnHeaderIndex
			// 
			columnHeaderIndex.Text = "Index No.";
			columnHeaderIndex.Width = 80;
			// 
			// columnHeaderReadableDesignation
			// 
			columnHeaderReadableDesignation.Text = "Readable designation";
			columnHeaderReadableDesignation.Width = 180;
			// 
			// saveFileDialogCsv
			// 
			saveFileDialogCsv.DefaultExt = "csv";
			saveFileDialogCsv.Filter = "CSV files|*.csv|all files|*.*";
			// 
			// saveFileDialogJson
			// 
			saveFileDialogJson.DefaultExt = "json";
			saveFileDialogJson.Filter = "JSON files|*.json|all files|*.*";
			// 
			// saveFileDialogHtml
			// 
			saveFileDialogHtml.DefaultExt = "html";
			saveFileDialogHtml.Filter = "HTML files|*.html|all files|*.*";
			// 
			// saveFileDialogXml
			// 
			saveFileDialogXml.DefaultExt = "xml";
			saveFileDialogXml.Filter = "XML files|*.xml|all files|*.*";
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// ListReadableDesignationsForm
			// 
			AccessibleDescription = "List readable designations";
			AccessibleName = "List of readable designations";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(312, 415);
			ControlBox = false;
			Controls.Add(panel);
			Controls.Add(statusStrip);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ListReadableDesignationsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "List of readable designations";
			toolTip.SetToolTip(this, "List of readable designations ");
			FormClosed += ListReadableDesignationsForm_FormClosed;
			Load += ListReadableDesignationsForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			contextMenuSaveList.ResumeLayout(false);
			contextMenuCopyToClipboard.ResumeLayout(false);
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private BackgroundWorker backgroundWorker;
		private ToolTip toolTip;
		private KryptonPanel panel;
		private KryptonButton buttonCancel;
		private ListView listView;
		private ColumnHeader columnHeaderIndex;
		private ColumnHeader columnHeaderReadableDesignation;
		private KryptonProgressBar progressBar;
		private KryptonButton buttonList;
		private Label labelWarning;
		private KryptonButton buttonLoad;
		private KryptonLabel labelMinimum;
		private KryptonNumericUpDown numericUpDownMinimum;
		private KryptonNumericUpDown numericUpDownMaximum;
		private KryptonLabel labelMaximum;
		private KryptonDropButton dropButtonSaveList;
		private ContextMenuStrip contextMenuSaveList;
		private ToolStripMenuItem toolStripMenuItemSaveAsCsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsHtml;
		private ToolStripMenuItem toolStripMenuItemSaveAsXml;
		private ToolStripMenuItem toolStripMenuItemSaveAsJson;
		private SaveFileDialog saveFileDialogCsv;
		private SaveFileDialog saveFileDialogJson;
		private SaveFileDialog saveFileDialogHtml;
		private SaveFileDialog saveFileDialogXml;
		private KryptonManager kryptonManager;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem ToolStripMenuItemCpyToClipboard;
	}
}