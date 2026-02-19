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
			buttonList = new KryptonButton();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			buttonLoad = new KryptonButton();
			labelMinimum = new KryptonLabel();
			numericUpDownMinimum = new KryptonNumericUpDown();
			numericUpDownMaximum = new KryptonNumericUpDown();
			labelMaximum = new KryptonLabel();
			contextMenuSaveList = new ContextMenuStrip(components);
			toolStripMenuItemSaveAsLatex = new ToolStripMenuItem();
			toolStripMenuItemSaveAsMarkdown = new ToolStripMenuItem();
			toolStripMenuItemSaveAsCsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsTsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsHtml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsXml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsJson = new ToolStripMenuItem();
			toolStripMenuItemSaveAsYaml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsSql = new ToolStripMenuItem();
			toolStripMenuItemSaveAsPdf = new ToolStripMenuItem();
			toolStripMenuItemSaveAsPostScript = new ToolStripMenuItem();
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
			saveFileDialogPostScript = new SaveFileDialog();
			saveFileDialogPdf = new SaveFileDialog();
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
			// buttonList
			// 
			buttonList.AccessibleDescription = "Starts the progress and list";
			buttonList.AccessibleName = "List";
			buttonList.AccessibleRole = AccessibleRole.PushButton;
			buttonList.Location = new Point(12, 38);
			buttonList.Name = "buttonList";
			buttonList.Size = new Size(52, 31);
			buttonList.TabIndex = 4;
			buttonList.ToolTipValues.Description = "Starts the progress and list";
			buttonList.ToolTipValues.EnableToolTips = true;
			buttonList.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonList.ToolTipValues.Heading = "List";
			buttonList.Values.DropDownArrowColor = Color.Empty;
			buttonList.Values.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			buttonList.Values.Text = "&List";
			buttonList.Click += ButtonList_Click;
			buttonList.Enter += Control_Enter;
			buttonList.Leave += Control_Leave;
			buttonList.MouseEnter += Control_Enter;
			buttonList.MouseLeave += Control_Leave;
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
			ToolStripMenuItemCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			ToolStripMenuItemCopyToClipboard.Name = "ToolStripMenuItemCopyToClipboard";
			ToolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			ToolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			ToolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
			ToolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
			ToolStripMenuItemCopyToClipboard.Click += CopyToClipboard_DoubleClick;
			ToolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
			ToolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// buttonLoad
			// 
			buttonLoad.AccessibleDescription = "Loads the selected planetoid";
			buttonLoad.AccessibleName = "Load";
			buttonLoad.AccessibleRole = AccessibleRole.PushButton;
			buttonLoad.DialogResult = DialogResult.OK;
			buttonLoad.Location = new Point(70, 38);
			buttonLoad.Name = "buttonLoad";
			buttonLoad.Size = new Size(56, 31);
			buttonLoad.TabIndex = 6;
			buttonLoad.ToolTipValues.Description = "Loads the selected planetoid";
			buttonLoad.ToolTipValues.EnableToolTips = true;
			buttonLoad.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonLoad.ToolTipValues.Heading = "Load";
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
			labelMinimum.ToolTipValues.Description = "Shows the minimum";
			labelMinimum.ToolTipValues.EnableToolTips = true;
			labelMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMinimum.ToolTipValues.Heading = "Minimum";
			labelMinimum.Values.Text = "M&inimum:";
			labelMinimum.Click += CopyToClipboard_DoubleClick;
			labelMinimum.Enter += Control_Enter;
			labelMinimum.Leave += Control_Leave;
			labelMinimum.MouseEnter += Control_Enter;
			labelMinimum.MouseLeave += Control_Leave;
			// 
			// numericUpDownMinimum
			// 
			numericUpDownMinimum.AccessibleDescription = "Shows the minimum value for the list";
			numericUpDownMinimum.AccessibleName = "Minimum value for the list";
			numericUpDownMinimum.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimum.Location = new Point(84, 10);
			numericUpDownMinimum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimum.Name = "numericUpDownMinimum";
			numericUpDownMinimum.Size = new Size(64, 22);
			numericUpDownMinimum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
			numericUpDownMinimum.TabIndex = 1;
			numericUpDownMinimum.ToolTipValues.Description = "Shows the minimum value for the list";
			numericUpDownMinimum.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimum.ToolTipValues.Heading = "Minimum value for the list";
			numericUpDownMinimum.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimum.Enter += Control_Enter;
			numericUpDownMinimum.Leave += Control_Leave;
			numericUpDownMinimum.MouseEnter += Control_Enter;
			numericUpDownMinimum.MouseLeave += Control_Leave;
			// 
			// numericUpDownMaximum
			// 
			numericUpDownMaximum.AccessibleDescription = "Shows the maximum value for the list";
			numericUpDownMaximum.AccessibleName = "Maximum value for the list";
			numericUpDownMaximum.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximum.Location = new Point(226, 10);
			numericUpDownMaximum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximum.Name = "numericUpDownMaximum";
			numericUpDownMaximum.Size = new Size(64, 22);
			numericUpDownMaximum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
			numericUpDownMaximum.TabIndex = 3;
			numericUpDownMaximum.ToolTipValues.Description = "Shows the maximum value for the list";
			numericUpDownMaximum.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximum.ToolTipValues.Heading = "Maximum value for the list";
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
			labelMaximum.ToolTipValues.Description = "Shows the maximum";
			labelMaximum.ToolTipValues.EnableToolTips = true;
			labelMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMaximum.ToolTipValues.Heading = "Maximum";
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
			contextMenuSaveList.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsLatex, toolStripMenuItemSaveAsMarkdown, toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsTsv, toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsJson, toolStripMenuItemSaveAsYaml, toolStripMenuItemSaveAsSql, toolStripMenuItemSaveAsPdf, toolStripMenuItemSaveAsPostScript });
			contextMenuSaveList.Name = "contextMenuStrip1";
			contextMenuSaveList.Size = new Size(193, 246);
			contextMenuSaveList.TabStop = true;
			contextMenuSaveList.Text = "&Save List";
			contextMenuSaveList.MouseEnter += Control_Enter;
			contextMenuSaveList.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsLatex
			// 
			toolStripMenuItemSaveAsLatex.AccessibleDescription = "Save the list as Latex file";
			toolStripMenuItemSaveAsLatex.AccessibleName = "Save as TEX";
			toolStripMenuItemSaveAsLatex.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsLatex.AutoToolTip = true;
			toolStripMenuItemSaveAsLatex.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsLatex.Name = "toolStripMenuItemSaveAsLatex";
			toolStripMenuItemSaveAsLatex.ShortcutKeyDisplayString = "Strg+E";
			toolStripMenuItemSaveAsLatex.ShortcutKeys = Keys.Control | Keys.E;
			toolStripMenuItemSaveAsLatex.Size = new Size(192, 22);
			toolStripMenuItemSaveAsLatex.Text = "Save as T&EX";
			toolStripMenuItemSaveAsLatex.Click += ToolStripMenuItemSaveAsLatex_Click;
			toolStripMenuItemSaveAsLatex.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsLatex.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsMarkdown
			// 
			toolStripMenuItemSaveAsMarkdown.AccessibleDescription = "Save the list as Markdown document";
			toolStripMenuItemSaveAsMarkdown.AccessibleName = "Save as MD";
			toolStripMenuItemSaveAsMarkdown.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsMarkdown.AutoToolTip = true;
			toolStripMenuItemSaveAsMarkdown.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsMarkdown.Name = "toolStripMenuItemSaveAsMarkdown";
			toolStripMenuItemSaveAsMarkdown.ShortcutKeyDisplayString = "Strg+M";
			toolStripMenuItemSaveAsMarkdown.ShortcutKeys = Keys.Control | Keys.M;
			toolStripMenuItemSaveAsMarkdown.Size = new Size(192, 22);
			toolStripMenuItemSaveAsMarkdown.Text = "Save as &MD";
			toolStripMenuItemSaveAsMarkdown.Click += ToolStripMenuItemSaveAsMarkdown_Click;
			toolStripMenuItemSaveAsMarkdown.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsMarkdown.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsCsv
			// 
			toolStripMenuItemSaveAsCsv.AccessibleDescription = "Save the list as CSV file";
			toolStripMenuItemSaveAsCsv.AccessibleName = "Save as CSV";
			toolStripMenuItemSaveAsCsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsCsv.AutoToolTip = true;
			toolStripMenuItemSaveAsCsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsCsv.Name = "toolStripMenuItemSaveAsCsv";
			toolStripMenuItemSaveAsCsv.ShortcutKeyDisplayString = "Strg+C";
			toolStripMenuItemSaveAsCsv.ShortcutKeys = Keys.Control | Keys.C;
			toolStripMenuItemSaveAsCsv.Size = new Size(192, 22);
			toolStripMenuItemSaveAsCsv.Text = "Save as &CSV";
			toolStripMenuItemSaveAsCsv.Click += ToolStripMenuItemSaveAsCsv_Click;
			toolStripMenuItemSaveAsCsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsCsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsTsv
			// 
			toolStripMenuItemSaveAsTsv.AccessibleDescription = "Save the list as TSV file";
			toolStripMenuItemSaveAsTsv.AccessibleName = "Save as TSV";
			toolStripMenuItemSaveAsTsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsTsv.AutoToolTip = true;
			toolStripMenuItemSaveAsTsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsTsv.Name = "toolStripMenuItemSaveAsTsv";
			toolStripMenuItemSaveAsTsv.ShortcutKeyDisplayString = "Strg+T";
			toolStripMenuItemSaveAsTsv.Size = new Size(192, 22);
			toolStripMenuItemSaveAsTsv.Text = "Save as &TSV";
			toolStripMenuItemSaveAsTsv.Click += ToolStripMenuItemSaveAsTsv_Click;
			toolStripMenuItemSaveAsTsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsTsv.MouseLeave += Control_Leave;
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
			toolStripMenuItemSaveAsJson.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
			toolStripMenuItemSaveAsJson.ShortcutKeyDisplayString = "Strg+J";
			toolStripMenuItemSaveAsJson.ShortcutKeys = Keys.Control | Keys.J;
			toolStripMenuItemSaveAsJson.Size = new Size(192, 22);
			toolStripMenuItemSaveAsJson.Text = "Save as &JSON";
			toolStripMenuItemSaveAsJson.Click += ToolStripMenuItemSaveAsJson_Click;
			toolStripMenuItemSaveAsJson.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsJson.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsYaml
			// 
			toolStripMenuItemSaveAsYaml.AccessibleDescription = "Save the list as YAML file";
			toolStripMenuItemSaveAsYaml.AccessibleName = "Save as YAML";
			toolStripMenuItemSaveAsYaml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsYaml.AutoToolTip = true;
			toolStripMenuItemSaveAsYaml.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemSaveAsYaml.Name = "toolStripMenuItemSaveAsYaml";
			toolStripMenuItemSaveAsYaml.ShortcutKeyDisplayString = "Strg+Y";
			toolStripMenuItemSaveAsYaml.ShortcutKeys = Keys.Control | Keys.Y;
			toolStripMenuItemSaveAsYaml.Size = new Size(192, 22);
			toolStripMenuItemSaveAsYaml.Text = "Save as &YAML";
			toolStripMenuItemSaveAsYaml.Click += ToolStripMenuItemSaveAsYaml_Click;
			toolStripMenuItemSaveAsYaml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsYaml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsSql
			// 
			toolStripMenuItemSaveAsSql.AccessibleDescription = "Save the list as SQL script";
			toolStripMenuItemSaveAsSql.AccessibleName = "Save as SQL";
			toolStripMenuItemSaveAsSql.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsSql.AutoToolTip = true;
			toolStripMenuItemSaveAsSql.Image = FatcowIcons16px.fatcow_page_white_database_16px;
			toolStripMenuItemSaveAsSql.Name = "toolStripMenuItemSaveAsSql";
			toolStripMenuItemSaveAsSql.ShortcutKeyDisplayString = "Strg+S";
			toolStripMenuItemSaveAsSql.ShortcutKeys = Keys.Control | Keys.S;
			toolStripMenuItemSaveAsSql.Size = new Size(192, 22);
			toolStripMenuItemSaveAsSql.Text = "Save as &SQL";
			toolStripMenuItemSaveAsSql.Click += ToolStripMenuItemSaveAsSql_Click;
			toolStripMenuItemSaveAsSql.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsSql.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsPdf
			// 
			toolStripMenuItemSaveAsPdf.AccessibleDescription = "Save the list as PDF";
			toolStripMenuItemSaveAsPdf.AccessibleName = "Save as PDF";
			toolStripMenuItemSaveAsPdf.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsPdf.AutoToolTip = true;
			toolStripMenuItemSaveAsPdf.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsPdf.Name = "toolStripMenuItemSaveAsPdf";
			toolStripMenuItemSaveAsPdf.ShortcutKeyDisplayString = "Strg+F";
			toolStripMenuItemSaveAsPdf.ShortcutKeys = Keys.Control | Keys.F;
			toolStripMenuItemSaveAsPdf.Size = new Size(192, 22);
			toolStripMenuItemSaveAsPdf.Text = "Save as PD&F";
			toolStripMenuItemSaveAsPdf.Click += ToolStripMenuItemSaveAsPdf_Click;
			toolStripMenuItemSaveAsPdf.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsPdf.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsPostScript
			// 
			toolStripMenuItemSaveAsPostScript.AccessibleDescription = "Save the list as PostScript";
			toolStripMenuItemSaveAsPostScript.AccessibleName = "Save as PS";
			toolStripMenuItemSaveAsPostScript.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsPostScript.AutoToolTip = true;
			toolStripMenuItemSaveAsPostScript.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsPostScript.Name = "toolStripMenuItemSaveAsPostScript";
			toolStripMenuItemSaveAsPostScript.ShortcutKeyDisplayString = "Strg+P";
			toolStripMenuItemSaveAsPostScript.ShortcutKeys = Keys.Control | Keys.P;
			toolStripMenuItemSaveAsPostScript.Size = new Size(192, 22);
			toolStripMenuItemSaveAsPostScript.Text = "Save as &PS";
			toolStripMenuItemSaveAsPostScript.Click += ToolStripMenuItemSaveAsPostScript_Click;
			toolStripMenuItemSaveAsPostScript.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsPostScript.MouseLeave += Control_Leave;
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
			dropButtonSaveList.ToolTipValues.Description = "Saves the list as file";
			dropButtonSaveList.ToolTipValues.EnableToolTips = true;
			dropButtonSaveList.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			dropButtonSaveList.ToolTipValues.Heading = "Save List";
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
			panel.Controls.Add(listView);
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
			listView.Location = new Point(12, 75);
			listView.MultiSelect = false;
			listView.Name = "listView";
			listView.ShowItemToolTips = true;
			listView.Size = new Size(288, 315);
			listView.TabIndex = 10;
			listView.UseCompatibleStateImageBehavior = false;
			listView.View = View.Details;
			listView.VirtualMode = true;
			listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
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
			saveFileDialogLatex.Filter = "LaTex files|*.tex|all files|*.*";
			// 
			// saveFileDialogPostScript
			// 
			saveFileDialogPostScript.DefaultExt = "ps";
			saveFileDialogPostScript.Filter = "PostScript files|*.ps|all files|*.*";
			// 
			// saveFileDialogPdf
			// 
			saveFileDialogPdf.DefaultExt = "pdf";
			saveFileDialogPdf.Filter = "PDF files|*.pdf|all files|*.*";
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
			FormClosed += ListReadableDesignationsForm_FormClosed;
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
		private KryptonPanel panel;
		private ListView listView;
		private ColumnHeader columnHeaderIndex;
		private ColumnHeader columnHeaderReadableDesignation;
		private KryptonButton buttonList;
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
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
		private SaveFileDialog saveFileDialogMarkdown;
		private SaveFileDialog saveFileDialogYaml;
		private SaveFileDialog saveFileDialogSql;
		private SaveFileDialog saveFileDialogTsv;
		private SaveFileDialog saveFileDialogLatex;
		private ToolStripMenuItem toolStripMenuItemSaveAsSql;
		private ToolStripMenuItem toolStripMenuItemSaveAsMarkdown;
		private ToolStripMenuItem toolStripMenuItemSaveAsYaml;
		private ToolStripMenuItem toolStripMenuItemSaveAsTsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsLatex;
		private SaveFileDialog saveFileDialogPostScript;
		private ToolStripMenuItem toolStripMenuItemSaveAsPostScript;
		private SaveFileDialog saveFileDialogPdf;
		private ToolStripMenuItem toolStripMenuItemSaveAsPdf;
	}
}