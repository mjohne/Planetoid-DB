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
			toolTip = new ToolTip(components);
			buttonCancel = new KryptonButton();
			progressBar = new KryptonProgressBar();
			buttonList = new KryptonButton();
			labelWarning = new Label();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCpyToClipboard = new ToolStripMenuItem();
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
			panel = new KryptonPanel();
			listView = new ListView();
			columnHeaderIndex = new ColumnHeader();
			columnHeaderReadableDesignation = new ColumnHeader();
			saveFileDialogCsv = new SaveFileDialog();
			saveFileDialogJson = new SaveFileDialog();
			saveFileDialogHtml = new SaveFileDialog();
			saveFileDialogXml = new SaveFileDialog();
			kryptonManager = new KryptonManager(components);
			saveFileDialogMarkdown = new SaveFileDialog();
			saveFileDialogYaml = new SaveFileDialog();
			saveFileDialogSql = new SaveFileDialog();
			saveFileDialogTsv = new SaveFileDialog();
			saveFileDialogLatex = new SaveFileDialog();
			statusStrip.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			contextMenuSaveList.SuspendLayout();
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
			buttonCancel.Enter += Control_Enter;
			buttonCancel.Leave += Control_Leave;
			buttonCancel.MouseEnter += Control_Enter;
			buttonCancel.MouseLeave += Control_Leave;
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
			progressBar.MouseEnter += Control_Enter;
			progressBar.MouseLeave += Control_Leave;
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
			buttonList.Enter += Control_Enter;
			buttonList.Leave += Control_Leave;
			buttonList.MouseEnter += Control_Enter;
			buttonList.MouseLeave += Control_Leave;
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
			labelWarning.Enter += Control_Enter;
			labelWarning.Leave += Control_Leave;
			labelWarning.MouseDown += Control_MouseDown;
			labelWarning.MouseEnter += Control_Enter;
			labelWarning.MouseLeave += Control_Leave;
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
			contextMenuCopyToClipboard.MouseEnter += Control_Enter;
			contextMenuCopyToClipboard.MouseLeave += Control_Leave;
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
			ToolStripMenuItemCpyToClipboard.MouseEnter += Control_Enter;
			ToolStripMenuItemCpyToClipboard.MouseLeave += Control_Leave;
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
			buttonLoad.Enter += Control_Enter;
			buttonLoad.Leave += Control_Leave;
			buttonLoad.MouseEnter += Control_Enter;
			buttonLoad.MouseLeave += Control_Leave;
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
			labelMinimum.Enter += Control_Enter;
			labelMinimum.Leave += Control_Leave;
			labelMinimum.MouseEnter += Control_Enter;
			labelMinimum.MouseLeave += Control_Leave;
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
			numericUpDownMinimum.Enter += Control_Enter;
			numericUpDownMinimum.Leave += Control_Leave;
			numericUpDownMinimum.MouseEnter += Control_Enter;
			numericUpDownMinimum.MouseLeave += Control_Leave;
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
			numericUpDownMaximum.Enter += Control_Enter;
			numericUpDownMaximum.Leave += Control_Leave;
			numericUpDownMaximum.MouseEnter += Control_Enter;
			numericUpDownMaximum.MouseLeave += Control_Leave;
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
			labelMaximum.Enter += Control_Enter;
			labelMaximum.Leave += Control_Leave;
			labelMaximum.MouseEnter += Control_Enter;
			labelMaximum.MouseLeave += Control_Leave;
			// 
			// contextMenuSaveList
			// 
			contextMenuSaveList.AccessibleDescription = "Save the list as file";
			contextMenuSaveList.AccessibleName = "Save list";
			contextMenuSaveList.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuSaveList.Font = new Font("Segoe UI", 9F);
			contextMenuSaveList.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsJson });
			contextMenuSaveList.Name = "contextMenuStrip1";
			contextMenuSaveList.Size = new Size(193, 92);
			contextMenuSaveList.TabStop = true;
			contextMenuSaveList.Text = "&Save List";
			toolTip.SetToolTip(contextMenuSaveList, "Save List");
			contextMenuSaveList.MouseEnter += Control_Enter;
			contextMenuSaveList.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsCsv
			// 
			toolStripMenuItemSaveAsCsv.AccessibleDescription = "Save the list as CSV file";
			toolStripMenuItemSaveAsCsv.AccessibleName = "Save as CSV";
			toolStripMenuItemSaveAsCsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsCsv.AutoToolTip = true;
			toolStripMenuItemSaveAsCsv.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsCsv.Name = "toolStripMenuItemSaveAsCsv";
			toolStripMenuItemSaveAsCsv.ShortcutKeyDisplayString = "Strg+C";
			toolStripMenuItemSaveAsCsv.ShortcutKeys = Keys.Control | Keys.C;
			toolStripMenuItemSaveAsCsv.Size = new Size(192, 22);
			toolStripMenuItemSaveAsCsv.Text = "Save as &CSV";
			toolStripMenuItemSaveAsCsv.Click += ToolStripMenuItemSaveAsCsv_Click;
			toolStripMenuItemSaveAsCsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsCsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsHtml
			// 
			toolStripMenuItemSaveAsHtml.AccessibleDescription = "Save the list as HTML file";
			toolStripMenuItemSaveAsHtml.AccessibleName = "Save as HTML";
			toolStripMenuItemSaveAsHtml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsHtml.AutoToolTip = true;
			toolStripMenuItemSaveAsHtml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsHtml.Name = "toolStripMenuItemSaveAsHtml";
			toolStripMenuItemSaveAsHtml.ShortcutKeyDisplayString = "Strg+H";
			toolStripMenuItemSaveAsHtml.ShortcutKeys = Keys.Control | Keys.H;
			toolStripMenuItemSaveAsHtml.Size = new Size(192, 22);
			toolStripMenuItemSaveAsHtml.Text = "Save as &HTML";
			toolStripMenuItemSaveAsHtml.Click += ToolStripMenuItemSaveAsHtml_Click;
			toolStripMenuItemSaveAsHtml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsHtml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsXml
			// 
			toolStripMenuItemSaveAsXml.AccessibleDescription = "Save the list as XML file";
			toolStripMenuItemSaveAsXml.AccessibleName = "Save as XML";
			toolStripMenuItemSaveAsXml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsXml.AutoToolTip = true;
			toolStripMenuItemSaveAsXml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsXml.Name = "toolStripMenuItemSaveAsXml";
			toolStripMenuItemSaveAsXml.ShortcutKeyDisplayString = "Strg+X";
			toolStripMenuItemSaveAsXml.ShortcutKeys = Keys.Control | Keys.X;
			toolStripMenuItemSaveAsXml.Size = new Size(192, 22);
			toolStripMenuItemSaveAsXml.Text = "Save as &XML";
			toolStripMenuItemSaveAsXml.Click += ToolStripMenuItemSaveAsXml_Click;
			toolStripMenuItemSaveAsXml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsXml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsJson
			// 
			toolStripMenuItemSaveAsJson.AccessibleDescription = "Save the list as JSON file";
			toolStripMenuItemSaveAsJson.AccessibleName = "Save as JSON";
			toolStripMenuItemSaveAsJson.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsJson.AutoToolTip = true;
			toolStripMenuItemSaveAsJson.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
			toolStripMenuItemSaveAsJson.ShortcutKeyDisplayString = "Strg+J";
			toolStripMenuItemSaveAsJson.ShortcutKeys = Keys.Control | Keys.J;
			toolStripMenuItemSaveAsJson.Size = new Size(192, 22);
			toolStripMenuItemSaveAsJson.Text = "Save as &JSON";
			toolStripMenuItemSaveAsJson.Click += ToolStripMenuItemSaveAsJson_Click;
			toolStripMenuItemSaveAsJson.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsJson.MouseLeave += Control_Leave;
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
			dropButtonSaveList.Enter += Control_Enter;
			dropButtonSaveList.Leave += Control_Leave;
			dropButtonSaveList.MouseEnter += Control_Enter;
			dropButtonSaveList.MouseLeave += Control_Leave;
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
			listView.Enter += Control_Enter;
			listView.Leave += Control_Leave;
			listView.MouseEnter += Control_Enter;
			listView.MouseLeave += Control_Leave;
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
			// saveFileDialogMarkdown
			// 
			saveFileDialogMarkdown.DefaultExt = "md";
			saveFileDialogMarkdown.Filter = "Markdown files|*.md|all files|*.*";
			// 
			// saveFileDialogYaml
			// 
			saveFileDialogYaml.DefaultExt = "yaml";
			saveFileDialogYaml.Filter = "YAML files|*.yaml|all files|*.*";
			// 
			// saveFileDialogSql
			// 
			saveFileDialogSql.DefaultExt = "sql";
			saveFileDialogSql.Filter = "SQL script|*.sql|all files|*.*";
			// 
			// saveFileDialogTsv
			// 
			saveFileDialogTsv.DefaultExt = "tsv";
			saveFileDialogTsv.Filter = "TSV files|*.tsv|all files|*.*";
			// 
			// saveFileDialogLatex
			// 
			saveFileDialogLatex.DefaultExt = "tex";
			saveFileDialogLatex.Filter = "Latex files|*.tex|all files|*.*";
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
			Load += ListReadableDesignationsForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			contextMenuCopyToClipboard.ResumeLayout(false);
			contextMenuSaveList.ResumeLayout(false);
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
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
		private SaveFileDialog saveFileDialogMarkdown;
		private SaveFileDialog saveFileDialogYaml;
		private SaveFileDialog saveFileDialogSql;
		private SaveFileDialog saveFileDialogTsv;
		private SaveFileDialog saveFileDialogLatex;
	}
}