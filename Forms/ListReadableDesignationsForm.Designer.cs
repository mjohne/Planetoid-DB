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
			kryptonStatusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			buttonList = new KryptonButton();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			buttonLoad = new KryptonButton();
			labelMinimum = new KryptonLabel();
			numericUpDownMinimum = new KryptonNumericUpDown();
			numericUpDownMaximum = new KryptonNumericUpDown();
			labelMaximum = new KryptonLabel();
			contextMenuSaveList = new ContextMenuStrip(components);
			toolStripMenuItemSaveAsText = new ToolStripMenuItem();
			toolStripMenuItemSaveAsLatex = new ToolStripMenuItem();
			toolStripMenuItemSaveAsMarkdown = new ToolStripMenuItem();
			toolStripMenuItemSaveAsWord = new ToolStripMenuItem();
			toolStripMenuItemSaveAsOdt = new ToolStripMenuItem();
			toolStripMenuItemSaveAsRtf = new ToolStripMenuItem();
			toolStripMenuItemSaveAsExcel = new ToolStripMenuItem();
			toolStripMenuItemSaveAsOds = new ToolStripMenuItem();
			toolStripMenuItemSaveAsCsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsTsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsPsv = new ToolStripMenuItem();
			toolStripMenuItemSaveAsHtml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsXml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsJson = new ToolStripMenuItem();
			toolStripMenuItemSaveAsYaml = new ToolStripMenuItem();
			toolStripMenuItemSaveAsSql = new ToolStripMenuItem();
			toolStripMenuItemSaveAsPdf = new ToolStripMenuItem();
			toolStripMenuItemSaveAsPostScript = new ToolStripMenuItem();
			toolStripMenuItemSaveAsEpub = new ToolStripMenuItem();
			toolStripMenuItemSaveAsMobi = new ToolStripMenuItem();
			dropButtonSaveList = new KryptonDropButton();
			kryptoPanelMain = new KryptonPanel();
			listView = new ListView();
			columnHeaderIndex = new ColumnHeader();
			columnHeaderReadableDesignation = new ColumnHeader();
			kryptonManager = new KryptonManager(components);
			kryptonStatusStrip.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			contextMenuSaveList.SuspendLayout();
			((ISupportInitialize)kryptoPanelMain).BeginInit();
			kryptoPanelMain.SuspendLayout();
			SuspendLayout();
			// 
			// kryptonStatusStrip
			// 
			kryptonStatusStrip.AccessibleDescription = "Shows some information";
			kryptonStatusStrip.AccessibleName = "Status bar with some information";
			kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
			kryptonStatusStrip.AllowClickThrough = true;
			kryptonStatusStrip.AllowItemReorder = true;
			kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
			kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			kryptonStatusStrip.Location = new Point(0, 393);
			kryptonStatusStrip.Name = "kryptonStatusStrip";
			kryptonStatusStrip.ProgressBars = null;
			kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			kryptonStatusStrip.ShowItemToolTips = true;
			kryptonStatusStrip.Size = new Size(312, 22);
			kryptonStatusStrip.SizingGrip = false;
			kryptonStatusStrip.TabIndex = 1;
			kryptonStatusStrip.TabStop = true;
			kryptonStatusStrip.Text = "Status bar";
			// 
			// labelInformation
			// 
			labelInformation.AccessibleDescription = "Shows some information";
			labelInformation.AccessibleName = "Shows some information";
			labelInformation.AccessibleRole = AccessibleRole.StaticText;
			labelInformation.AutoToolTip = true;
			labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
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
			buttonList.ToolTipValues.Description = "Starts the progress and list.";
			buttonList.ToolTipValues.EnableToolTips = true;
			buttonList.ToolTipValues.Heading = "List";
			buttonList.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			contextMenuCopyToClipboard.AccessibleDescription = "Shows the context menu for copying database information to the clipboard";
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
			buttonLoad.ToolTipValues.Description = "Loads the selected planetoid.";
			buttonLoad.ToolTipValues.EnableToolTips = true;
			buttonLoad.ToolTipValues.Heading = "Load";
			buttonLoad.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelMinimum.ToolTipValues.Description = "Shows the minimum.";
			labelMinimum.ToolTipValues.EnableToolTips = true;
			labelMinimum.ToolTipValues.Heading = "Minimum";
			labelMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			numericUpDownMinimum.ToolTipValues.Description = "Shows the minimum value for the list.";
			numericUpDownMinimum.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimum.ToolTipValues.Heading = "Minimum value for the list";
			numericUpDownMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			numericUpDownMaximum.ToolTipValues.Description = "Shows the maximum value for the list.";
			numericUpDownMaximum.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximum.ToolTipValues.Heading = "Maximum value for the list";
			numericUpDownMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			labelMaximum.ToolTipValues.Description = "Shows the maximum.";
			labelMaximum.ToolTipValues.EnableToolTips = true;
			labelMaximum.ToolTipValues.Heading = "Maximum";
			labelMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
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
			contextMenuSaveList.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsText, toolStripMenuItemSaveAsLatex, toolStripMenuItemSaveAsMarkdown, toolStripMenuItemSaveAsWord, toolStripMenuItemSaveAsOdt, toolStripMenuItemSaveAsRtf, toolStripMenuItemSaveAsExcel, toolStripMenuItemSaveAsOds, toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsTsv, toolStripMenuItemSaveAsPsv, toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsJson, toolStripMenuItemSaveAsYaml, toolStripMenuItemSaveAsSql, toolStripMenuItemSaveAsPdf, toolStripMenuItemSaveAsPostScript, toolStripMenuItemSaveAsEpub, toolStripMenuItemSaveAsMobi });
			contextMenuSaveList.Name = "contextMenuStrip1";
			contextMenuSaveList.Size = new Size(216, 444);
			contextMenuSaveList.TabStop = true;
			contextMenuSaveList.Text = "&Save list";
			contextMenuSaveList.MouseEnter += Control_Enter;
			contextMenuSaveList.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsText
			// 
			toolStripMenuItemSaveAsText.AccessibleDescription = "Saves the list as text file";
			toolStripMenuItemSaveAsText.AccessibleName = "Save as text";
			toolStripMenuItemSaveAsText.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsText.AutoToolTip = true;
			toolStripMenuItemSaveAsText.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsText.Name = "toolStripMenuItemSaveAsText";
			toolStripMenuItemSaveAsText.ShortcutKeyDisplayString = "";
			toolStripMenuItemSaveAsText.ShortcutKeys = Keys.Control | Keys.X;
			toolStripMenuItemSaveAsText.Size = new Size(215, 22);
			toolStripMenuItemSaveAsText.Text = "Save as te&xt";
			toolStripMenuItemSaveAsText.Click += ToolStripMenuItemSaveAsText_Click;
			toolStripMenuItemSaveAsText.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsText.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsLatex
			// 
			toolStripMenuItemSaveAsLatex.AccessibleDescription = "Saves the list as Latex file";
			toolStripMenuItemSaveAsLatex.AccessibleName = "Save as Latex";
			toolStripMenuItemSaveAsLatex.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsLatex.AutoToolTip = true;
			toolStripMenuItemSaveAsLatex.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsLatex.Name = "toolStripMenuItemSaveAsLatex";
			toolStripMenuItemSaveAsLatex.ShortcutKeyDisplayString = "Strg+E";
			toolStripMenuItemSaveAsLatex.ShortcutKeys = Keys.Control | Keys.E;
			toolStripMenuItemSaveAsLatex.Size = new Size(215, 22);
			toolStripMenuItemSaveAsLatex.Text = "Save as Lat&ex";
			toolStripMenuItemSaveAsLatex.Click += ToolStripMenuItemSaveAsLatex_Click;
			toolStripMenuItemSaveAsLatex.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsLatex.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsMarkdown
			// 
			toolStripMenuItemSaveAsMarkdown.AccessibleDescription = "Saves the list as Markdown file";
			toolStripMenuItemSaveAsMarkdown.AccessibleName = "Save as Markdown";
			toolStripMenuItemSaveAsMarkdown.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsMarkdown.AutoToolTip = true;
			toolStripMenuItemSaveAsMarkdown.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsMarkdown.Name = "toolStripMenuItemSaveAsMarkdown";
			toolStripMenuItemSaveAsMarkdown.ShortcutKeyDisplayString = "Strg+K";
			toolStripMenuItemSaveAsMarkdown.ShortcutKeys = Keys.Control | Keys.K;
			toolStripMenuItemSaveAsMarkdown.Size = new Size(215, 22);
			toolStripMenuItemSaveAsMarkdown.Text = "Save as Mar&kdown";
			toolStripMenuItemSaveAsMarkdown.Click += ToolStripMenuItemSaveAsMarkdown_Click;
			toolStripMenuItemSaveAsMarkdown.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsMarkdown.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsWord
			// 
			toolStripMenuItemSaveAsWord.AccessibleDescription = "Saves the list as Word file";
			toolStripMenuItemSaveAsWord.AccessibleName = "Save as Word";
			toolStripMenuItemSaveAsWord.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsWord.AutoToolTip = true;
			toolStripMenuItemSaveAsWord.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemSaveAsWord.Name = "toolStripMenuItemSaveAsWord";
			toolStripMenuItemSaveAsWord.ShortcutKeyDisplayString = "Strg+W";
			toolStripMenuItemSaveAsWord.ShortcutKeys = Keys.Control | Keys.W;
			toolStripMenuItemSaveAsWord.Size = new Size(215, 22);
			toolStripMenuItemSaveAsWord.Text = "Save as &Word";
			toolStripMenuItemSaveAsWord.Click += ToolStripMenuItemSaveAsWord_Click;
			toolStripMenuItemSaveAsWord.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsWord.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsOdt
			// 
			toolStripMenuItemSaveAsOdt.AccessibleDescription = "Saves the list as ODT file";
			toolStripMenuItemSaveAsOdt.AccessibleName = "Save as ODT";
			toolStripMenuItemSaveAsOdt.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsOdt.AutoToolTip = true;
			toolStripMenuItemSaveAsOdt.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemSaveAsOdt.Name = "toolStripMenuItemSaveAsOdt";
			toolStripMenuItemSaveAsOdt.ShortcutKeyDisplayString = "Strg+D";
			toolStripMenuItemSaveAsOdt.ShortcutKeys = Keys.Control | Keys.D;
			toolStripMenuItemSaveAsOdt.Size = new Size(215, 22);
			toolStripMenuItemSaveAsOdt.Text = "Save as O&DT";
			toolStripMenuItemSaveAsOdt.Click += ToolStripMenuItemSaveAsOdt_Click;
			toolStripMenuItemSaveAsOdt.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsOdt.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsRtf
			// 
			toolStripMenuItemSaveAsRtf.AccessibleDescription = "Saves the list as RTF file";
			toolStripMenuItemSaveAsRtf.AccessibleName = "Save as RTF";
			toolStripMenuItemSaveAsRtf.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsRtf.AutoToolTip = true;
			toolStripMenuItemSaveAsRtf.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemSaveAsRtf.Name = "toolStripMenuItemSaveAsRtf";
			toolStripMenuItemSaveAsRtf.ShortcutKeyDisplayString = "Strg+R";
			toolStripMenuItemSaveAsRtf.ShortcutKeys = Keys.Control | Keys.R;
			toolStripMenuItemSaveAsRtf.Size = new Size(215, 22);
			toolStripMenuItemSaveAsRtf.Text = "Save as &RTF";
			toolStripMenuItemSaveAsRtf.Click += ToolStripMenuItemSaveAsRtf_Click;
			toolStripMenuItemSaveAsRtf.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsRtf.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsExcel
			// 
			toolStripMenuItemSaveAsExcel.AccessibleDescription = "Saves the list as Excel file";
			toolStripMenuItemSaveAsExcel.AccessibleName = "Save as Excel";
			toolStripMenuItemSaveAsExcel.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsExcel.AutoToolTip = true;
			toolStripMenuItemSaveAsExcel.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsExcel.Name = "toolStripMenuItemSaveAsExcel";
			toolStripMenuItemSaveAsExcel.ShortcutKeyDisplayString = "Strg+L";
			toolStripMenuItemSaveAsExcel.ShortcutKeys = Keys.Control | Keys.L;
			toolStripMenuItemSaveAsExcel.Size = new Size(215, 22);
			toolStripMenuItemSaveAsExcel.Text = "Save as Exce&l";
			toolStripMenuItemSaveAsExcel.Click += ToolStripMenuItemSaveAsExcel_Click;
			toolStripMenuItemSaveAsExcel.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsExcel.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsOds
			// 
			toolStripMenuItemSaveAsOds.AccessibleDescription = "Saves the list as ODS file";
			toolStripMenuItemSaveAsOds.AccessibleName = "Save as ODS";
			toolStripMenuItemSaveAsOds.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsOds.AutoToolTip = true;
			toolStripMenuItemSaveAsOds.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsOds.Name = "toolStripMenuItemSaveAsOds";
			toolStripMenuItemSaveAsOds.ShortcutKeyDisplayString = "Strg+S";
			toolStripMenuItemSaveAsOds.ShortcutKeys = Keys.Control | Keys.S;
			toolStripMenuItemSaveAsOds.Size = new Size(215, 22);
			toolStripMenuItemSaveAsOds.Text = "Save as OD&S";
			toolStripMenuItemSaveAsOds.Click += ToolStripMenuItemSaveAsOds_Click;
			toolStripMenuItemSaveAsOds.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsOds.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsCsv
			// 
			toolStripMenuItemSaveAsCsv.AccessibleDescription = "Saves the list as CSV file";
			toolStripMenuItemSaveAsCsv.AccessibleName = "Save as CSV";
			toolStripMenuItemSaveAsCsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsCsv.AutoToolTip = true;
			toolStripMenuItemSaveAsCsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsCsv.Name = "toolStripMenuItemSaveAsCsv";
			toolStripMenuItemSaveAsCsv.ShortcutKeyDisplayString = "Strg+C";
			toolStripMenuItemSaveAsCsv.ShortcutKeys = Keys.Control | Keys.C;
			toolStripMenuItemSaveAsCsv.Size = new Size(215, 22);
			toolStripMenuItemSaveAsCsv.Text = "Save as &CSV";
			toolStripMenuItemSaveAsCsv.Click += ToolStripMenuItemSaveAsCsv_Click;
			toolStripMenuItemSaveAsCsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsCsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsTsv
			// 
			toolStripMenuItemSaveAsTsv.AccessibleDescription = "Saves the list as TSV file";
			toolStripMenuItemSaveAsTsv.AccessibleName = "Save as TSV";
			toolStripMenuItemSaveAsTsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsTsv.AutoToolTip = true;
			toolStripMenuItemSaveAsTsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsTsv.Name = "toolStripMenuItemSaveAsTsv";
			toolStripMenuItemSaveAsTsv.ShortcutKeyDisplayString = "Strg+T";
			toolStripMenuItemSaveAsTsv.ShortcutKeys = Keys.Control | Keys.T;
			toolStripMenuItemSaveAsTsv.Size = new Size(215, 22);
			toolStripMenuItemSaveAsTsv.Text = "Save as &TSV";
			toolStripMenuItemSaveAsTsv.Click += ToolStripMenuItemSaveAsTsv_Click;
			toolStripMenuItemSaveAsTsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsTsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsPsv
			// 
			toolStripMenuItemSaveAsPsv.AccessibleDescription = "Saves the list as PSV file";
			toolStripMenuItemSaveAsPsv.AccessibleName = "Save as PSV";
			toolStripMenuItemSaveAsPsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsPsv.AutoToolTip = true;
			toolStripMenuItemSaveAsPsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemSaveAsPsv.Name = "toolStripMenuItemSaveAsPsv";
			toolStripMenuItemSaveAsPsv.ShortcutKeyDisplayString = "Strg+V";
			toolStripMenuItemSaveAsPsv.ShortcutKeys = Keys.Control | Keys.V;
			toolStripMenuItemSaveAsPsv.Size = new Size(215, 22);
			toolStripMenuItemSaveAsPsv.Text = "Save as PS&V";
			toolStripMenuItemSaveAsPsv.Click += ToolStripMenuItemSaveAsPsv_Click;
			toolStripMenuItemSaveAsPsv.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsPsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsHtml
			// 
			toolStripMenuItemSaveAsHtml.AccessibleDescription = "Saves the list as HTML file";
			toolStripMenuItemSaveAsHtml.AccessibleName = "Save as HTML";
			toolStripMenuItemSaveAsHtml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsHtml.AutoToolTip = true;
			toolStripMenuItemSaveAsHtml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsHtml.Name = "toolStripMenuItemSaveAsHtml";
			toolStripMenuItemSaveAsHtml.ShortcutKeyDisplayString = "Strg+H";
			toolStripMenuItemSaveAsHtml.ShortcutKeys = Keys.Control | Keys.H;
			toolStripMenuItemSaveAsHtml.Size = new Size(215, 22);
			toolStripMenuItemSaveAsHtml.Text = "Save as &HTML";
			toolStripMenuItemSaveAsHtml.Click += ToolStripMenuItemSaveAsHtml_Click;
			toolStripMenuItemSaveAsHtml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsHtml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsXml
			// 
			toolStripMenuItemSaveAsXml.AccessibleDescription = "Saves the list as XML file";
			toolStripMenuItemSaveAsXml.AccessibleName = "Save as XML";
			toolStripMenuItemSaveAsXml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsXml.AutoToolTip = true;
			toolStripMenuItemSaveAsXml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemSaveAsXml.Name = "toolStripMenuItemSaveAsXml";
			toolStripMenuItemSaveAsXml.ShortcutKeyDisplayString = "Strg+M";
			toolStripMenuItemSaveAsXml.ShortcutKeys = Keys.Control | Keys.M;
			toolStripMenuItemSaveAsXml.Size = new Size(215, 22);
			toolStripMenuItemSaveAsXml.Text = "Save as X&ML";
			toolStripMenuItemSaveAsXml.Click += ToolStripMenuItemSaveAsXml_Click;
			toolStripMenuItemSaveAsXml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsXml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsJson
			// 
			toolStripMenuItemSaveAsJson.AccessibleDescription = "Saves the list as JSON file";
			toolStripMenuItemSaveAsJson.AccessibleName = "Save as JSON";
			toolStripMenuItemSaveAsJson.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsJson.AutoToolTip = true;
			toolStripMenuItemSaveAsJson.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
			toolStripMenuItemSaveAsJson.ShortcutKeyDisplayString = "Strg+J";
			toolStripMenuItemSaveAsJson.ShortcutKeys = Keys.Control | Keys.J;
			toolStripMenuItemSaveAsJson.Size = new Size(215, 22);
			toolStripMenuItemSaveAsJson.Text = "Save as &JSON";
			toolStripMenuItemSaveAsJson.Click += ToolStripMenuItemSaveAsJson_Click;
			toolStripMenuItemSaveAsJson.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsJson.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsYaml
			// 
			toolStripMenuItemSaveAsYaml.AccessibleDescription = "Saves the list as YAML file";
			toolStripMenuItemSaveAsYaml.AccessibleName = "Save as YAML";
			toolStripMenuItemSaveAsYaml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsYaml.AutoToolTip = true;
			toolStripMenuItemSaveAsYaml.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemSaveAsYaml.Name = "toolStripMenuItemSaveAsYaml";
			toolStripMenuItemSaveAsYaml.ShortcutKeyDisplayString = "Strg+Y";
			toolStripMenuItemSaveAsYaml.ShortcutKeys = Keys.Control | Keys.Y;
			toolStripMenuItemSaveAsYaml.Size = new Size(215, 22);
			toolStripMenuItemSaveAsYaml.Text = "Save as &YAML";
			toolStripMenuItemSaveAsYaml.Click += ToolStripMenuItemSaveAsYaml_Click;
			toolStripMenuItemSaveAsYaml.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsYaml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsSql
			// 
			toolStripMenuItemSaveAsSql.AccessibleDescription = "Saves the list as SQL script";
			toolStripMenuItemSaveAsSql.AccessibleName = "Save as SQL";
			toolStripMenuItemSaveAsSql.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsSql.AutoToolTip = true;
			toolStripMenuItemSaveAsSql.Image = FatcowIcons16px.fatcow_page_white_database_16px;
			toolStripMenuItemSaveAsSql.Name = "toolStripMenuItemSaveAsSql";
			toolStripMenuItemSaveAsSql.ShortcutKeyDisplayString = "Strg+Q";
			toolStripMenuItemSaveAsSql.ShortcutKeys = Keys.Control | Keys.Q;
			toolStripMenuItemSaveAsSql.Size = new Size(215, 22);
			toolStripMenuItemSaveAsSql.Text = "Save as S&QL";
			toolStripMenuItemSaveAsSql.Click += ToolStripMenuItemSaveAsSql_Click;
			toolStripMenuItemSaveAsSql.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsSql.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsPdf
			// 
			toolStripMenuItemSaveAsPdf.AccessibleDescription = "Saves the list as PDF file";
			toolStripMenuItemSaveAsPdf.AccessibleName = "Save as PDF";
			toolStripMenuItemSaveAsPdf.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsPdf.AutoToolTip = true;
			toolStripMenuItemSaveAsPdf.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsPdf.Name = "toolStripMenuItemSaveAsPdf";
			toolStripMenuItemSaveAsPdf.ShortcutKeyDisplayString = "Strg+F";
			toolStripMenuItemSaveAsPdf.ShortcutKeys = Keys.Control | Keys.F;
			toolStripMenuItemSaveAsPdf.Size = new Size(215, 22);
			toolStripMenuItemSaveAsPdf.Text = "Save as PD&F";
			toolStripMenuItemSaveAsPdf.Click += ToolStripMenuItemSaveAsPdf_Click;
			toolStripMenuItemSaveAsPdf.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsPdf.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsPostScript
			// 
			toolStripMenuItemSaveAsPostScript.AccessibleDescription = "Saves the list as PostScript file";
			toolStripMenuItemSaveAsPostScript.AccessibleName = "Save as PS";
			toolStripMenuItemSaveAsPostScript.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsPostScript.AutoToolTip = true;
			toolStripMenuItemSaveAsPostScript.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsPostScript.Name = "toolStripMenuItemSaveAsPostScript";
			toolStripMenuItemSaveAsPostScript.ShortcutKeyDisplayString = "Strg+P";
			toolStripMenuItemSaveAsPostScript.ShortcutKeys = Keys.Control | Keys.P;
			toolStripMenuItemSaveAsPostScript.Size = new Size(215, 22);
			toolStripMenuItemSaveAsPostScript.Text = "Save as &PS";
			toolStripMenuItemSaveAsPostScript.Click += ToolStripMenuItemSaveAsPostScript_Click;
			toolStripMenuItemSaveAsPostScript.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsPostScript.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsEpub
			// 
			toolStripMenuItemSaveAsEpub.AccessibleDescription = "Saves the list as EPUB file";
			toolStripMenuItemSaveAsEpub.AccessibleName = "Save as EPUB";
			toolStripMenuItemSaveAsEpub.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsEpub.AutoToolTip = true;
			toolStripMenuItemSaveAsEpub.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsEpub.Name = "toolStripMenuItemSaveAsEpub";
			toolStripMenuItemSaveAsEpub.ShortcutKeyDisplayString = "Strg+B";
			toolStripMenuItemSaveAsEpub.ShortcutKeys = Keys.Control | Keys.B;
			toolStripMenuItemSaveAsEpub.Size = new Size(215, 22);
			toolStripMenuItemSaveAsEpub.Text = "Save as EPU&B";
			toolStripMenuItemSaveAsEpub.Click += ToolStripMenuItemSaveAsEpub_Click;
			toolStripMenuItemSaveAsEpub.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsEpub.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsMobi
			// 
			toolStripMenuItemSaveAsMobi.AccessibleDescription = "Saves the list as MOBI file";
			toolStripMenuItemSaveAsMobi.AccessibleName = "Save as MOBI";
			toolStripMenuItemSaveAsMobi.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsMobi.AutoToolTip = true;
			toolStripMenuItemSaveAsMobi.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemSaveAsMobi.Name = "toolStripMenuItemSaveAsMobi";
			toolStripMenuItemSaveAsMobi.ShortcutKeyDisplayString = "Strg+I";
			toolStripMenuItemSaveAsMobi.ShortcutKeys = Keys.Control | Keys.I;
			toolStripMenuItemSaveAsMobi.Size = new Size(215, 22);
			toolStripMenuItemSaveAsMobi.Text = "Save as MOB&I";
			toolStripMenuItemSaveAsMobi.Click += ToolStripMenuItemSaveAsMobi_Click;
			toolStripMenuItemSaveAsMobi.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsMobi.MouseLeave += Control_Leave;
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
			dropButtonSaveList.ToolTipValues.Description = "Saves the list as file.";
			dropButtonSaveList.ToolTipValues.EnableToolTips = true;
			dropButtonSaveList.ToolTipValues.Heading = "Save list";
			dropButtonSaveList.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			dropButtonSaveList.Values.DropDownArrowColor = Color.Empty;
			dropButtonSaveList.Values.ImageStates.ImageCheckedNormal = null;
			dropButtonSaveList.Values.ImageStates.ImageCheckedPressed = null;
			dropButtonSaveList.Values.ImageStates.ImageCheckedTracking = null;
			dropButtonSaveList.Values.ImageStates.ImageDisabled = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImageNormal = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImagePressed = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.ImageStates.ImageTracking = FatcowIcons16px.fatcow_diskette_16px;
			dropButtonSaveList.Values.Text = "&Save list";
			dropButtonSaveList.Enter += Control_Enter;
			dropButtonSaveList.Leave += Control_Leave;
			dropButtonSaveList.MouseEnter += Control_Enter;
			dropButtonSaveList.MouseLeave += Control_Leave;
			// 
			// kryptoPanelMain
			// 
			kryptoPanelMain.AccessibleDescription = "Groups the data";
			kryptoPanelMain.AccessibleName = "Panel";
			kryptoPanelMain.AccessibleRole = AccessibleRole.Pane;
			kryptoPanelMain.Controls.Add(dropButtonSaveList);
			kryptoPanelMain.Controls.Add(labelMinimum);
			kryptoPanelMain.Controls.Add(numericUpDownMinimum);
			kryptoPanelMain.Controls.Add(numericUpDownMaximum);
			kryptoPanelMain.Controls.Add(labelMaximum);
			kryptoPanelMain.Controls.Add(buttonLoad);
			kryptoPanelMain.Controls.Add(listView);
			kryptoPanelMain.Controls.Add(buttonList);
			kryptoPanelMain.Dock = DockStyle.Fill;
			kryptoPanelMain.Location = new Point(0, 0);
			kryptoPanelMain.Name = "kryptoPanelMain";
			kryptoPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
			kryptoPanelMain.Size = new Size(312, 393);
			kryptoPanelMain.TabIndex = 0;
			kryptoPanelMain.TabStop = true;
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
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// ListReadableDesignationsForm
			// 
			AccessibleDescription = "Lists the readable designations";
			AccessibleName = "List of readable designations";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(312, 415);
			ControlBox = false;
			Controls.Add(kryptoPanelMain);
			Controls.Add(kryptonStatusStrip);
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
			kryptonStatusStrip.ResumeLayout(false);
			kryptonStatusStrip.PerformLayout();
			contextMenuCopyToClipboard.ResumeLayout(false);
			contextMenuSaveList.ResumeLayout(false);
			((ISupportInitialize)kryptoPanelMain).EndInit();
			kryptoPanelMain.ResumeLayout(false);
			kryptoPanelMain.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private KryptonStatusStrip kryptonStatusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonPanel kryptoPanelMain;
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
		private KryptonManager kryptonManager;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
		private ToolStripMenuItem toolStripMenuItemSaveAsSql;
		private ToolStripMenuItem toolStripMenuItemSaveAsMarkdown;
		private ToolStripMenuItem toolStripMenuItemSaveAsYaml;
		private ToolStripMenuItem toolStripMenuItemSaveAsTsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsLatex;
		private ToolStripMenuItem toolStripMenuItemSaveAsPostScript;
		private ToolStripMenuItem toolStripMenuItemSaveAsPdf;
		private ToolStripMenuItem toolStripMenuItemSaveAsPsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsEpub;
		private ToolStripMenuItem toolStripMenuItemSaveAsWord;
		private ToolStripMenuItem toolStripMenuItemSaveAsExcel;
		private ToolStripMenuItem toolStripMenuItemSaveAsOdt;
		private ToolStripMenuItem toolStripMenuItemSaveAsOds;
		private ToolStripMenuItem toolStripMenuItemSaveAsMobi;
		private ToolStripMenuItem toolStripMenuItemSaveAsText;
		private ToolStripMenuItem toolStripMenuItemSaveAsRtf;
	}
}