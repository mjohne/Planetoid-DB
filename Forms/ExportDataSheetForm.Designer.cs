using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class ExportDataSheetForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ExportDataSheetForm));
			statusStrip = new KryptonStatusStrip();
			labelStatus = new ToolStripStatusLabel();
			panel = new KryptonPanel();
			checkedListBoxOrbitalElements = new KryptonCheckedListBox();
			kryptonManager = new KryptonManager(components);
			toolStripContainer = new ToolStripContainer();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripDropDownButtonExport = new ToolStripDropDownButton();
			contextMenuExport = new ContextMenuStrip(components);
			toolStripMenuItemExportAsText = new ToolStripMenuItem();
			toolStripMenuItemExportAsLatex = new ToolStripMenuItem();
			toolStripMenuItemExportAsMarkdown = new ToolStripMenuItem();
			toolStripMenuItemExportAsWord = new ToolStripMenuItem();
			toolStripMenuItemExportAsOdt = new ToolStripMenuItem();
			toolStripMenuItemExportAsRtf = new ToolStripMenuItem();
			toolStripMenuItemExportAsExcel = new ToolStripMenuItem();
			toolStripMenuItemExportAsOds = new ToolStripMenuItem();
			toolStripMenuItemExportAsCsv = new ToolStripMenuItem();
			toolStripMenuItemExportAsTsv = new ToolStripMenuItem();
			toolStripMenuItemExportAsPsv = new ToolStripMenuItem();
			toolStripMenuItemExportAsHtml = new ToolStripMenuItem();
			toolStripMenuItemExportAsXml = new ToolStripMenuItem();
			toolStripMenuItemExportAsJson = new ToolStripMenuItem();
			toolStripMenuItemExportAsYaml = new ToolStripMenuItem();
			toolStripMenuItemExportAsSql = new ToolStripMenuItem();
			toolStripMenuItemExportAsPdf = new ToolStripMenuItem();
			toolStripMenuItemExportAsPostScript = new ToolStripMenuItem();
			toolStripMenuItemExportAsEpub = new ToolStripMenuItem();
			toolStripMenuItemExportAsMobi = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			toolStripButtonMarkAll = new ToolStripButton();
			toolStripButtonUnmarkAll = new ToolStripButton();
			statusStrip.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			kryptonToolStripIcons.SuspendLayout();
			contextMenuExport.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelStatus });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(284, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 0;
			statusStrip.Text = "status bar";
			statusStrip.Enter += Control_Enter;
			statusStrip.Leave += Control_Leave;
			statusStrip.MouseEnter += Control_Enter;
			statusStrip.MouseLeave += Control_Leave;
			// 
			// labelStatus
			// 
			labelStatus.AccessibleDescription = "Shows some information";
			labelStatus.AccessibleName = "Show some information";
			labelStatus.AccessibleRole = AccessibleRole.StaticText;
			labelStatus.AutoToolTip = true;
			labelStatus.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelStatus.Margin = new Padding(5, 3, 0, 2);
			labelStatus.Name = "labelStatus";
			labelStatus.Size = new Size(144, 17);
			labelStatus.Text = "some information here";
			labelStatus.ToolTipText = "Shows some information";
			labelStatus.MouseEnter += Control_Enter;
			labelStatus.MouseLeave += Control_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "Pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(checkedListBoxOrbitalElements);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(284, 299);
			panel.TabIndex = 0;
			panel.TabStop = true;				 
			panel.Enter += Control_Enter;
			panel.Leave += Control_Leave;
			panel.MouseEnter += Control_Enter;
			panel.MouseLeave += Control_Leave;
			// 
			// checkedListBoxOrbitalElements
			// 
			checkedListBoxOrbitalElements.AccessibleDescription = "Checks some orbital elements to print on a data sheet";
			checkedListBoxOrbitalElements.AccessibleName = "Check orbital elements";
			checkedListBoxOrbitalElements.AccessibleRole = AccessibleRole.List;
			checkedListBoxOrbitalElements.BackStyle = PaletteBackStyle.InputControlRibbon;
			checkedListBoxOrbitalElements.CheckOnClick = true;
			checkedListBoxOrbitalElements.Dock = DockStyle.Fill;
			checkedListBoxOrbitalElements.FormattingEnabled = true;
			checkedListBoxOrbitalElements.HorizontalScrollbar = true;
			checkedListBoxOrbitalElements.Items.AddRange(new object[] { "Index No.", "Readable designation", "Epoch (in packed form, .0 TT)", "Mean anomaly at the epoch (degrees)", "Argument of perihelion, J2000.0 (degrees)", "Longitude of the ascending node, J2000.0", "Inclination to the ecliptic, J2000.0 (degrees)", "Orbital eccentricity", "Mean daily motion (degrees per day)", "Semimajor axis (AU)", "Absolute magnitude, H (mag)", "Slope parameter, G", "Reference", "Number of oppositions", "Number of observations", "Observation span", "r.m.s. residual (arcseconds)", "Computer name", "4-hexdigit flags", "Date of last observation (YYYMMDD)", "Linear eccentricity (AU)", "Semi-minor axis (AU)", "Major axis (AU)", "Minor axis (AU)", "Eccentric anomaly (degrees)", "True anomaly (degrees)", "Perihelion distance (AU)", "Aphelion distance (AU)", "Longitude of Descending node (degrees)", "Argument of aphelion (degrees)", "Focal parameter (AU)", "Semi-latus rectum (AU)", "Latus rectum (AU)", "Orbital period (years)", "Orbital area (AU²)", "Orbital perimeter (AU)", "Semi-mean axis (AU)", "Mean axis (AU)", "Standard gravitational parameter (AU³/a²)" });
			checkedListBoxOrbitalElements.Location = new Point(0, 0);
			checkedListBoxOrbitalElements.Margin = new Padding(4, 3, 4, 3);
			checkedListBoxOrbitalElements.Name = "checkedListBoxOrbitalElements";
			checkedListBoxOrbitalElements.Size = new Size(284, 299);
			checkedListBoxOrbitalElements.TabIndex = 0;
			checkedListBoxOrbitalElements.ToolTipValues.Description = "Checks some orbital elements to print on a data sheet.";
			checkedListBoxOrbitalElements.ToolTipValues.EnableToolTips = true;
			checkedListBoxOrbitalElements.ToolTipValues.Heading = "Check orbital elements";
			checkedListBoxOrbitalElements.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkedListBoxOrbitalElements.SelectedIndexChanged += CheckedListBoxOrbitalElements_SelectedIndexChanged;
			checkedListBoxOrbitalElements.Enter += Control_Enter;
			checkedListBoxOrbitalElements.Leave += Control_Leave;
			checkedListBoxOrbitalElements.MouseEnter += Control_Enter;
			checkedListBoxOrbitalElements.MouseLeave += Control_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// toolStripContainer
			// 
			toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
			toolStripContainer.AccessibleName = "Container to arrange the toolbars";
			toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
			toolStripContainer.Enter += Control_Enter;
			toolStripContainer.Leave += Control_Leave;
			toolStripContainer.MouseEnter += Control_Enter;
			toolStripContainer.MouseLeave += Control_Leave;

			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(panel);
			toolStripContainer.ContentPanel.Size = new Size(284, 299);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(284, 346);
			toolStripContainer.TabIndex = 5;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
			// 
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.AccessibleDescription = "Toolbar of exporting orbital elements to file";
			kryptonToolStripIcons.AccessibleName = "Toolbar of exporting orbital elements to file";
			kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripDropDownButtonExport, toolStripSeparator1, toolStripButtonMarkAll, toolStripButtonUnmarkAll });
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(284, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.TabStop = true;	
			kryptonToolStripIcons.Enter += Control_Enter;
			kryptonToolStripIcons.Leave += Control_Leave;
			kryptonToolStripIcons.MouseEnter += Control_Enter;
			kryptonToolStripIcons.MouseLeave += Control_Leave;
			// 
			// toolStripDropDownButtonExport
			// 
			toolStripDropDownButtonExport.AccessibleDescription = "Exports the data entry";
			toolStripDropDownButtonExport.AccessibleName = "Export";
			toolStripDropDownButtonExport.AccessibleRole = AccessibleRole.PushButton;
			toolStripDropDownButtonExport.DropDown = contextMenuExport;
			toolStripDropDownButtonExport.Image = FatcowIcons16px.fatcow_document_export_16px;
			toolStripDropDownButtonExport.ImageTransparentColor = Color.Magenta;
			toolStripDropDownButtonExport.Name = "toolStripDropDownButtonExport";
			toolStripDropDownButtonExport.Size = new Size(70, 22);
			toolStripDropDownButtonExport.Text = "&Export";
			toolStripDropDownButtonExport.MouseEnter += Control_Enter;
			toolStripDropDownButtonExport.MouseLeave += Control_Leave;
			// 
			// contextMenuExport
			// 
			contextMenuExport.AccessibleDescription = "Shows the context menu for exporting the data sheet";
			contextMenuExport.AccessibleName = "Context menu for exporting the data sheet";
			contextMenuExport.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuExport.Font = new Font("Segoe UI", 9F);
			contextMenuExport.Items.AddRange(new ToolStripItem[] { toolStripMenuItemExportAsText, toolStripMenuItemExportAsLatex, toolStripMenuItemExportAsMarkdown, toolStripMenuItemExportAsWord, toolStripMenuItemExportAsOdt, toolStripMenuItemExportAsRtf, toolStripMenuItemExportAsExcel, toolStripMenuItemExportAsOds, toolStripMenuItemExportAsCsv, toolStripMenuItemExportAsTsv, toolStripMenuItemExportAsPsv, toolStripMenuItemExportAsHtml, toolStripMenuItemExportAsXml, toolStripMenuItemExportAsJson, toolStripMenuItemExportAsYaml, toolStripMenuItemExportAsSql, toolStripMenuItemExportAsPdf, toolStripMenuItemExportAsPostScript, toolStripMenuItemExportAsEpub, toolStripMenuItemExportAsMobi });
			contextMenuExport.Name = "contextMenuSaveToFile";
			contextMenuExport.Size = new Size(226, 444);
			contextMenuExport.TabStop = true;
			contextMenuExport.Text = "Export";	 
			contextMenuExport.MouseEnter += Control_Enter;
			contextMenuExport.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsText
			// 
			toolStripMenuItemExportAsText.AccessibleDescription = "Exports the information as text file";
			toolStripMenuItemExportAsText.AccessibleName = "Export as text";
			toolStripMenuItemExportAsText.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsText.AutoToolTip = true;
			toolStripMenuItemExportAsText.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemExportAsText.Name = "toolStripMenuItemExportAsText";
			toolStripMenuItemExportAsText.ShortcutKeyDisplayString = "Strg+X";
			toolStripMenuItemExportAsText.ShortcutKeys = Keys.Control | Keys.X;
			toolStripMenuItemExportAsText.Size = new Size(225, 22);
			toolStripMenuItemExportAsText.Text = "Export as te&xt";
			toolStripMenuItemExportAsText.Click += ToolStripMenuItemExportAsText_Click;
			toolStripMenuItemExportAsText.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsText.MouseLeave += Control_Leave;

			// 
			// toolStripMenuItemExportAsLatex
			// 
			toolStripMenuItemExportAsLatex.AccessibleDescription = "Exports the information as Latex file";
			toolStripMenuItemExportAsLatex.AccessibleName = "Export as Latex";
			toolStripMenuItemExportAsLatex.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsLatex.AutoToolTip = true;
			toolStripMenuItemExportAsLatex.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemExportAsLatex.Name = "toolStripMenuItemExportAsLatex";
			toolStripMenuItemExportAsLatex.ShortcutKeyDisplayString = "Strg+E";
			toolStripMenuItemExportAsLatex.ShortcutKeys = Keys.Control | Keys.E;
			toolStripMenuItemExportAsLatex.Size = new Size(225, 22);
			toolStripMenuItemExportAsLatex.Text = "Export as Lat&ex";
			toolStripMenuItemExportAsLatex.Click += ToolStripMenuItemExportAsLatex_Click;
			toolStripMenuItemExportAsLatex.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsLatex.MouseLeave += Control_Leave;

			// 
			// toolStripMenuItemExportAsMarkdown
			// 
			toolStripMenuItemExportAsMarkdown.AccessibleDescription = "Exports the information as Markdown file";
			toolStripMenuItemExportAsMarkdown.AccessibleName = "Export as Markdown";
			toolStripMenuItemExportAsMarkdown.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsMarkdown.AutoToolTip = true;
			toolStripMenuItemExportAsMarkdown.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemExportAsMarkdown.Name = "toolStripMenuItemExportAsMarkdown";
			toolStripMenuItemExportAsMarkdown.ShortcutKeyDisplayString = "Strg+K";
			toolStripMenuItemExportAsMarkdown.ShortcutKeys = Keys.Control | Keys.K;
			toolStripMenuItemExportAsMarkdown.Size = new Size(225, 22);
			toolStripMenuItemExportAsMarkdown.Text = "Export as Mar&kdown";
			toolStripMenuItemExportAsMarkdown.Click += ToolStripMenuItemExportAsMarkdown_Click;
			toolStripMenuItemExportAsMarkdown.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsMarkdown.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsWord
			// 
			toolStripMenuItemExportAsWord.AccessibleDescription = "Exports the information as Word file";
			toolStripMenuItemExportAsWord.AccessibleName = "Export as Word";
			toolStripMenuItemExportAsWord.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsWord.AutoToolTip = true;
			toolStripMenuItemExportAsWord.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemExportAsWord.Name = "toolStripMenuItemExportAsWord";
			toolStripMenuItemExportAsWord.ShortcutKeyDisplayString = "Strg+W";
			toolStripMenuItemExportAsWord.ShortcutKeys = Keys.Control | Keys.W;
			toolStripMenuItemExportAsWord.Size = new Size(225, 22);
			toolStripMenuItemExportAsWord.Text = "Export as &Word";
			toolStripMenuItemExportAsWord.Click += ToolStripMenuItemExportAsWord_Click;
			toolStripMenuItemExportAsWord.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsWord.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsOdt
			// 
			toolStripMenuItemExportAsOdt.AccessibleDescription = "Exports the information as ODT file";
			toolStripMenuItemExportAsOdt.AccessibleName = "Export as ODT";
			toolStripMenuItemExportAsOdt.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsOdt.AutoToolTip = true;
			toolStripMenuItemExportAsOdt.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemExportAsOdt.Name = "toolStripMenuItemExportAsOdt";
			toolStripMenuItemExportAsOdt.ShortcutKeyDisplayString = "Strg+D";
			toolStripMenuItemExportAsOdt.ShortcutKeys = Keys.Control | Keys.D;
			toolStripMenuItemExportAsOdt.Size = new Size(225, 22);
			toolStripMenuItemExportAsOdt.Text = "Export as O&DT";
			toolStripMenuItemExportAsOdt.Click += ToolStripMenuItemExportAsOdt_Click;
			toolStripMenuItemExportAsOdt.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsOdt.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsRtf
			// 
			toolStripMenuItemExportAsRtf.AccessibleDescription = "Exports the information as RTF file";
			toolStripMenuItemExportAsRtf.AccessibleName = "Export as RTF";
			toolStripMenuItemExportAsRtf.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsRtf.AutoToolTip = true;
			toolStripMenuItemExportAsRtf.Image = FatcowIcons16px.fatcow_page_white_word_16px;
			toolStripMenuItemExportAsRtf.Name = "toolStripMenuItemExportAsRtf";
			toolStripMenuItemExportAsRtf.ShortcutKeyDisplayString = "Strg+R";
			toolStripMenuItemExportAsRtf.ShortcutKeys = Keys.Control | Keys.R;
			toolStripMenuItemExportAsRtf.Size = new Size(225, 22);
			toolStripMenuItemExportAsRtf.Text = "Export as &RTF";
			toolStripMenuItemExportAsRtf.Click += ToolStripMenuItemExportAsRtf_Click;
			toolStripMenuItemExportAsRtf.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsRtf.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsExcel
			// 
			toolStripMenuItemExportAsExcel.AccessibleDescription = "Exports the information as Excel file";
			toolStripMenuItemExportAsExcel.AccessibleName = "Export as Excel";
			toolStripMenuItemExportAsExcel.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsExcel.AutoToolTip = true;
			toolStripMenuItemExportAsExcel.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemExportAsExcel.Name = "toolStripMenuItemExportAsExcel";
			toolStripMenuItemExportAsExcel.ShortcutKeyDisplayString = "Strg+L";
			toolStripMenuItemExportAsExcel.ShortcutKeys = Keys.Control | Keys.L;
			toolStripMenuItemExportAsExcel.Size = new Size(225, 22);
			toolStripMenuItemExportAsExcel.Text = "Export as Exce&l";
			toolStripMenuItemExportAsExcel.Click += ToolStripMenuItemExportAsExcel_Click;
			toolStripMenuItemExportAsExcel.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsExcel.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsOds
			// 
			toolStripMenuItemExportAsOds.AccessibleDescription = "Exports the information as ODS file";
			toolStripMenuItemExportAsOds.AccessibleName = "Export as ODS";
			toolStripMenuItemExportAsOds.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsOds.AutoToolTip = true;
			toolStripMenuItemExportAsOds.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemExportAsOds.Name = "toolStripMenuItemExportAsOds";
			toolStripMenuItemExportAsOds.ShortcutKeyDisplayString = "Strg+S";
			toolStripMenuItemExportAsOds.ShortcutKeys = Keys.Control | Keys.S;
			toolStripMenuItemExportAsOds.Size = new Size(225, 22);
			toolStripMenuItemExportAsOds.Text = "Export as OD&S";
			toolStripMenuItemExportAsOds.Click += ToolStripMenuItemExportAsOds_Click;
			toolStripMenuItemExportAsOds.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsOds.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsCsv
			// 
			toolStripMenuItemExportAsCsv.AccessibleDescription = "Exports the information as CSV file";
			toolStripMenuItemExportAsCsv.AccessibleName = "Export as CSV";
			toolStripMenuItemExportAsCsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsCsv.AutoToolTip = true;
			toolStripMenuItemExportAsCsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemExportAsCsv.Name = "toolStripMenuItemExportAsCsv";
			toolStripMenuItemExportAsCsv.ShortcutKeyDisplayString = "Strg+C";
			toolStripMenuItemExportAsCsv.ShortcutKeys = Keys.Control | Keys.C;
			toolStripMenuItemExportAsCsv.Size = new Size(225, 22);
			toolStripMenuItemExportAsCsv.Text = "Export as &CSV";
			toolStripMenuItemExportAsCsv.Click += ToolStripMenuItemExportAsCsv_Click;
			toolStripMenuItemExportAsCsv.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsCsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsTsv
			// 
			toolStripMenuItemExportAsTsv.AccessibleDescription = "Exports the information as TSV file";
			toolStripMenuItemExportAsTsv.AccessibleName = "Export as TSV";
			toolStripMenuItemExportAsTsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsTsv.AutoToolTip = true;
			toolStripMenuItemExportAsTsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemExportAsTsv.Name = "toolStripMenuItemExportAsTsv";
			toolStripMenuItemExportAsTsv.ShortcutKeyDisplayString = "Strg+T";
			toolStripMenuItemExportAsTsv.ShortcutKeys = Keys.Control | Keys.T;
			toolStripMenuItemExportAsTsv.Size = new Size(225, 22);
			toolStripMenuItemExportAsTsv.Text = "Export as &TSV";
			toolStripMenuItemExportAsTsv.Click += ToolStripMenuItemExportAsTsv_Click;
			toolStripMenuItemExportAsTsv.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsTsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsPsv
			// 
			toolStripMenuItemExportAsPsv.AccessibleDescription = "Exports the information as PSV file";
			toolStripMenuItemExportAsPsv.AccessibleName = "Export as PSV";
			toolStripMenuItemExportAsPsv.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsPsv.AutoToolTip = true;
			toolStripMenuItemExportAsPsv.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
			toolStripMenuItemExportAsPsv.Name = "toolStripMenuItemExportAsPsv";
			toolStripMenuItemExportAsPsv.ShortcutKeyDisplayString = "Strg+V";
			toolStripMenuItemExportAsPsv.ShortcutKeys = Keys.Control | Keys.V;
			toolStripMenuItemExportAsPsv.Size = new Size(225, 22);
			toolStripMenuItemExportAsPsv.Text = "Export as PS&V";
			toolStripMenuItemExportAsPsv.Click += ToolStripMenuItemExportAsPsv_Click;
			toolStripMenuItemExportAsPsv.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsPsv.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsHtml
			// 
			toolStripMenuItemExportAsHtml.AccessibleDescription = "Exports the information as HTML file";
			toolStripMenuItemExportAsHtml.AccessibleName = "Export as HTML";
			toolStripMenuItemExportAsHtml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsHtml.AutoToolTip = true;
			toolStripMenuItemExportAsHtml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemExportAsHtml.Name = "toolStripMenuItemExportAsHtml";
			toolStripMenuItemExportAsHtml.ShortcutKeyDisplayString = "Strg+H";
			toolStripMenuItemExportAsHtml.ShortcutKeys = Keys.Control | Keys.H;
			toolStripMenuItemExportAsHtml.Size = new Size(225, 22);
			toolStripMenuItemExportAsHtml.Text = "Export as &HTML";
			toolStripMenuItemExportAsHtml.Click += ToolStripMenuItemExportAsHtml_Click;
			toolStripMenuItemExportAsHtml.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsHtml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsXml
			// 
			toolStripMenuItemExportAsXml.AccessibleDescription = "Exports the information as XML file";
			toolStripMenuItemExportAsXml.AccessibleName = "Export as XML";
			toolStripMenuItemExportAsXml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsXml.AutoToolTip = true;
			toolStripMenuItemExportAsXml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			toolStripMenuItemExportAsXml.Name = "toolStripMenuItemExportAsXml";
			toolStripMenuItemExportAsXml.ShortcutKeyDisplayString = "Strg+M";
			toolStripMenuItemExportAsXml.ShortcutKeys = Keys.Control | Keys.M;
			toolStripMenuItemExportAsXml.Size = new Size(225, 22);
			toolStripMenuItemExportAsXml.Text = "Export as X&ML";
			toolStripMenuItemExportAsXml.Click += ToolStripMenuItemExportAsXml_Click;
			toolStripMenuItemExportAsXml.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsXml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsJson
			// 
			toolStripMenuItemExportAsJson.AccessibleDescription = "Exports the information as JSON file";
			toolStripMenuItemExportAsJson.AccessibleName = "Export as JSON";
			toolStripMenuItemExportAsJson.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsJson.AutoToolTip = true;
			toolStripMenuItemExportAsJson.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemExportAsJson.Name = "toolStripMenuItemExportAsJson";
			toolStripMenuItemExportAsJson.ShortcutKeyDisplayString = "Strg+J";
			toolStripMenuItemExportAsJson.ShortcutKeys = Keys.Control | Keys.J;
			toolStripMenuItemExportAsJson.Size = new Size(225, 22);
			toolStripMenuItemExportAsJson.Text = "Export as &JSON";
			toolStripMenuItemExportAsJson.Click += ToolStripMenuItemExportAsJson_Click;
			toolStripMenuItemExportAsJson.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsJson.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsYaml
			// 
			toolStripMenuItemExportAsYaml.AccessibleDescription = "Exports the information as YAML file";
			toolStripMenuItemExportAsYaml.AccessibleName = "Export as YAML";
			toolStripMenuItemExportAsYaml.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsYaml.AutoToolTip = true;
			toolStripMenuItemExportAsYaml.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
			toolStripMenuItemExportAsYaml.Name = "toolStripMenuItemExportAsYaml";
			toolStripMenuItemExportAsYaml.ShortcutKeyDisplayString = "Strg+Y";
			toolStripMenuItemExportAsYaml.ShortcutKeys = Keys.Control | Keys.Y;
			toolStripMenuItemExportAsYaml.Size = new Size(225, 22);
			toolStripMenuItemExportAsYaml.Text = "Export as &YAML";
			toolStripMenuItemExportAsYaml.Click += ToolStripMenuItemExportAsYaml_Click;
			toolStripMenuItemExportAsYaml.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsYaml.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsSql
			// 
			toolStripMenuItemExportAsSql.AccessibleDescription = "Exports the information as SQL script";
			toolStripMenuItemExportAsSql.AccessibleName = "Export as SQL";
			toolStripMenuItemExportAsSql.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsSql.AutoToolTip = true;
			toolStripMenuItemExportAsSql.Image = FatcowIcons16px.fatcow_page_white_database_16px;
			toolStripMenuItemExportAsSql.Name = "toolStripMenuItemExportAsSql";
			toolStripMenuItemExportAsSql.ShortcutKeyDisplayString = "Strg+Q";
			toolStripMenuItemExportAsSql.ShortcutKeys = Keys.Control | Keys.Q;
			toolStripMenuItemExportAsSql.Size = new Size(225, 22);
			toolStripMenuItemExportAsSql.Text = "Export as S&QL";
			toolStripMenuItemExportAsSql.Click += ToolStripMenuItemExportAsSql_Click;
			toolStripMenuItemExportAsSql.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsSql.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsPdf
			// 
			toolStripMenuItemExportAsPdf.AccessibleDescription = "Exports the information as PDF file";
			toolStripMenuItemExportAsPdf.AccessibleName = "Export as PDF";
			toolStripMenuItemExportAsPdf.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsPdf.AutoToolTip = true;
			toolStripMenuItemExportAsPdf.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemExportAsPdf.Name = "toolStripMenuItemExportAsPdf";
			toolStripMenuItemExportAsPdf.ShortcutKeyDisplayString = "Strg+F";
			toolStripMenuItemExportAsPdf.ShortcutKeys = Keys.Control | Keys.F;
			toolStripMenuItemExportAsPdf.Size = new Size(225, 22);
			toolStripMenuItemExportAsPdf.Text = "Export as PD&F";
			toolStripMenuItemExportAsPdf.Click += ToolStripMenuItemExportAsPdf_Click;
			toolStripMenuItemExportAsPdf.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsPdf.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsPostScript
			// 
			toolStripMenuItemExportAsPostScript.AccessibleDescription = "Exports the information as PostScript file";
			toolStripMenuItemExportAsPostScript.AccessibleName = "Export as PS";
			toolStripMenuItemExportAsPostScript.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsPostScript.AutoToolTip = true;
			toolStripMenuItemExportAsPostScript.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemExportAsPostScript.Name = "toolStripMenuItemExportAsPostScript";
			toolStripMenuItemExportAsPostScript.ShortcutKeyDisplayString = "Strg+P";
			toolStripMenuItemExportAsPostScript.ShortcutKeys = Keys.Control | Keys.P;
			toolStripMenuItemExportAsPostScript.Size = new Size(225, 22);
			toolStripMenuItemExportAsPostScript.Text = "Export as &PS";
			toolStripMenuItemExportAsPostScript.Click += ToolStripMenuItemExportAsPostScript_Click;
			toolStripMenuItemExportAsPostScript.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsPostScript.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsEpub
			// 
			toolStripMenuItemExportAsEpub.AccessibleDescription = "Exports the information as EPUB file";
			toolStripMenuItemExportAsEpub.AccessibleName = "Export as EPUB";
			toolStripMenuItemExportAsEpub.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsEpub.AutoToolTip = true;
			toolStripMenuItemExportAsEpub.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemExportAsEpub.Name = "toolStripMenuItemExportAsEpub";
			toolStripMenuItemExportAsEpub.ShortcutKeyDisplayString = "Strg+B";
			toolStripMenuItemExportAsEpub.ShortcutKeys = Keys.Control | Keys.B;
			toolStripMenuItemExportAsEpub.Size = new Size(225, 22);
			toolStripMenuItemExportAsEpub.Text = "Export as EPU&B";
			toolStripMenuItemExportAsEpub.Click += ToolStripMenuItemExportAsEpub_Click;
			toolStripMenuItemExportAsEpub.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsEpub.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemExportAsMobi
			// 
			toolStripMenuItemExportAsMobi.AccessibleDescription = "Exports the information as MOBI file";
			toolStripMenuItemExportAsMobi.AccessibleName = "Export as MOBI";
			toolStripMenuItemExportAsMobi.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemExportAsMobi.AutoToolTip = true;
			toolStripMenuItemExportAsMobi.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
			toolStripMenuItemExportAsMobi.Name = "toolStripMenuItemExportAsMobi";
			toolStripMenuItemExportAsMobi.ShortcutKeyDisplayString = "Strg+I";
			toolStripMenuItemExportAsMobi.ShortcutKeys = Keys.Control | Keys.I;
			toolStripMenuItemExportAsMobi.Size = new Size(225, 22);
			toolStripMenuItemExportAsMobi.Text = "Export as MOB&I";
			toolStripMenuItemExportAsMobi.Click += ToolStripMenuItemExportAsMobi_Click;
			toolStripMenuItemExportAsMobi.MouseEnter += Control_Enter;
			toolStripMenuItemExportAsMobi.MouseLeave += Control_Leave;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.AccessibleDescription = "Just a separator";
			toolStripSeparator1.AccessibleName = "Just a separator";
			toolStripSeparator1.AccessibleRole = AccessibleRole.Separator;
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 25);
			toolStripSeparator1.MouseEnter += Control_Enter;
			toolStripSeparator1.MouseLeave += Control_Leave;
			// 
			// toolStripButtonMarkAll
			// 
			toolStripButtonMarkAll.AccessibleDescription = "Marks all orbital elements in the list";
			toolStripButtonMarkAll.AccessibleName = "Mark all orbital elements";
			toolStripButtonMarkAll.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonMarkAll.Image = FatcowIcons16px.fatcow_check_box_16px;
			toolStripButtonMarkAll.ImageTransparentColor = Color.Magenta;
			toolStripButtonMarkAll.Name = "toolStripButtonMarkAll";
			toolStripButtonMarkAll.Size = new Size(69, 22);
			toolStripButtonMarkAll.Text = "&Mark all";
			toolStripButtonMarkAll.Click += ToolStripButtonMarkAll_Click;
			toolStripButtonMarkAll.MouseEnter += Control_Enter;
			toolStripButtonMarkAll.MouseLeave += Control_Leave;
			// 
			// toolStripButtonUnmarkAll
			// 
			toolStripButtonUnmarkAll.AccessibleDescription = "Unmarks all orbital elements in the list";
			toolStripButtonUnmarkAll.AccessibleName = "Unmark all orbital elements";
			toolStripButtonUnmarkAll.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonUnmarkAll.Image = FatcowIcons16px.fatcow_check_box_uncheck_16px;
			toolStripButtonUnmarkAll.ImageTransparentColor = Color.Magenta;
			toolStripButtonUnmarkAll.Name = "toolStripButtonUnmarkAll";
			toolStripButtonUnmarkAll.Size = new Size(84, 22);
			toolStripButtonUnmarkAll.Text = "&Unmark all";
			toolStripButtonUnmarkAll.Click += ToolStripButtonUnmarkAll_Click;
			toolStripButtonUnmarkAll.MouseEnter += Control_Enter;
			toolStripButtonUnmarkAll.MouseLeave += Control_Leave;
			// 
			// ExportDataSheetForm
			// 
			AccessibleDescription = "Exports data sheet";
			AccessibleName = "Export data sheet";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(284, 346);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ExportDataSheetForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Export data sheet";
			Load += ExportDataSheetForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((ISupportInitialize)panel).EndInit();
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
			contextMenuExport.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelStatus;
		private KryptonPanel panel;
		private KryptonCheckedListBox checkedListBoxOrbitalElements;
		private KryptonManager kryptonManager;
		private ToolStripContainer toolStripContainer;
		private KryptonToolStrip kryptonToolStripIcons;
		private ToolStripDropDownButton toolStripDropDownButtonExport;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton toolStripButtonMarkAll;
		private ToolStripButton toolStripButtonUnmarkAll;
		private ContextMenuStrip contextMenuExport;
		private ToolStripMenuItem toolStripMenuItemExportAsText;
		private ToolStripMenuItem toolStripMenuItemExportAsLatex;
		private ToolStripMenuItem toolStripMenuItemExportAsMarkdown;
		private ToolStripMenuItem toolStripMenuItemExportAsWord;
		private ToolStripMenuItem toolStripMenuItemExportAsOdt;
		private ToolStripMenuItem toolStripMenuItemExportAsRtf;
		private ToolStripMenuItem toolStripMenuItemExportAsExcel;
		private ToolStripMenuItem toolStripMenuItemExportAsOds;
		private ToolStripMenuItem toolStripMenuItemExportAsCsv;
		private ToolStripMenuItem toolStripMenuItemExportAsTsv;
		private ToolStripMenuItem toolStripMenuItemExportAsPsv;
		private ToolStripMenuItem toolStripMenuItemExportAsHtml;
		private ToolStripMenuItem toolStripMenuItemExportAsXml;
		private ToolStripMenuItem toolStripMenuItemExportAsJson;
		private ToolStripMenuItem toolStripMenuItemExportAsYaml;
		private ToolStripMenuItem toolStripMenuItemExportAsSql;
		private ToolStripMenuItem toolStripMenuItemExportAsPdf;
		private ToolStripMenuItem toolStripMenuItemExportAsPostScript;
		private ToolStripMenuItem toolStripMenuItemExportAsEpub;
		private ToolStripMenuItem toolStripMenuItemExportAsMobi;
	}
}