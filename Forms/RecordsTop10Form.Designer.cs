// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents the main form for displaying and managing the top ten records.</summary>
/// <remarks>This form provides functionality to detect records and export them in various formats such as JSON, XML, TXT, and HTML. It includes a progress bar to indicate the status of record detection and a status strip for displaying additional information.</remarks>
partial class RecordsTop10Form
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	/// <remarks>This method disposes of the resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsTop10Form));
		kryptonPanelMain = new KryptonPanel();
		splitContainerMain = new SplitContainer();
		kryptonPanelSplitterContainerLeft = new KryptonPanel();
		listBox = new KryptonListBox();
		kryptonPanelSplitterContainerRight = new KryptonPanel();
		tableLayoutPanel = new KryptonTableLayoutPanel();
		contextMenuSaveToFile = new ContextMenuStrip(components);
		toolStripMenuItemTextFiles = new ToolStripMenuItem();
		toolStripMenuItemSaveAsText = new ToolStripMenuItem();
		toolStripMenuItemSaveAsLatex = new ToolStripMenuItem();
		toolStripMenuItemSaveAsMarkdown = new ToolStripMenuItem();
		toolStripMenuItemSaveAsAsciiDoc = new ToolStripMenuItem();
		toolStripMenuItemSaveAsReStructuredText = new ToolStripMenuItem();
		toolStripMenuItemSaveAsTextile = new ToolStripMenuItem();
		toolStripMenuItemWriterDocuments = new ToolStripMenuItem();
		toolStripMenuItemSaveAsWord = new ToolStripMenuItem();
		toolStripMenuItemSaveAsOdt = new ToolStripMenuItem();
		toolStripMenuItemSaveAsRtf = new ToolStripMenuItem();
		toolStripMenuItemSaveAsAbiword = new ToolStripMenuItem();
		toolStripMenuItemSaveAsWps = new ToolStripMenuItem();
		toolStripMenuItemSpreadsheetDocuments = new ToolStripMenuItem();
		toolStripMenuItemSaveAsExcel = new ToolStripMenuItem();
		toolStripMenuItemSaveAsOds = new ToolStripMenuItem();
		toolStripMenuItemSaveAsCsv = new ToolStripMenuItem();
		toolStripMenuItemSaveAsTsv = new ToolStripMenuItem();
		toolStripMenuItemSaveAsPsv = new ToolStripMenuItem();
		toolStripMenuItemSaveAsEt = new ToolStripMenuItem();
		toolStripMenuItemXmlDocuments = new ToolStripMenuItem();
		toolStripMenuItemSaveAsHtml = new ToolStripMenuItem();
		toolStripMenuItemSaveAsXml = new ToolStripMenuItem();
		toolStripMenuItemSaveAsDocBook = new ToolStripMenuItem();
		toolStripMenuItemConfigurationFiles = new ToolStripMenuItem();
		toolStripMenuItemSaveAsJson = new ToolStripMenuItem();
		toolStripMenuItemSaveAsToml = new ToolStripMenuItem();
		toolStripMenuItemSaveAsYaml = new ToolStripMenuItem();
		toolStripMenuItemDatabaseScripts = new ToolStripMenuItem();
		toolStripMenuItemSaveAsSql = new ToolStripMenuItem();
		toolStripMenuItemSaveAsSqlite = new ToolStripMenuItem();
		toolStripMenuItemPortableDocuments = new ToolStripMenuItem();
		toolStripMenuItemSaveAsPdf = new ToolStripMenuItem();
		toolStripMenuItemSaveAsPostScript = new ToolStripMenuItem();
		toolStripMenuItemSaveAsEpub = new ToolStripMenuItem();
		toolStripMenuItemSaveAsMobi = new ToolStripMenuItem();
		toolStripMenuItemSaveAsXps = new ToolStripMenuItem();
		toolStripMenuItemSaveAsFictionBook2 = new ToolStripMenuItem();
		toolStripMenuItemSaveAsChm = new ToolStripMenuItem();
		toolStripDropDownButtonSaveList = new ToolStripDropDownButton();
		labelGoToObjectHeader = new KryptonLabel();
		contextMenuCopyToClipboard = new ContextMenuStrip(components);
		toolStripMenuItemCopyToClipboardInContextMenu = new ToolStripMenuItem();
		labelPlaceHeader = new KryptonLabel();
		buttonGoto09 = new KryptonButton();
		buttonGoto08 = new KryptonButton();
		buttonGoto07 = new KryptonButton();
		buttonGoto06 = new KryptonButton();
		buttonGoto05 = new KryptonButton();
		buttonGoto04 = new KryptonButton();
		buttonGoto03 = new KryptonButton();
		buttonGoto02 = new KryptonButton();
		labelPlace10 = new KryptonLabel();
		labelPlace05 = new KryptonLabel();
		labelPlace04 = new KryptonLabel();
		labelPlace03 = new KryptonLabel();
		labelPlace01 = new KryptonLabel();
		labelPlace02 = new KryptonLabel();
		labelPlace06 = new KryptonLabel();
		labelPlace07 = new KryptonLabel();
		labelPlace08 = new KryptonLabel();
		labelPlace09 = new KryptonLabel();
		buttonGoto01 = new KryptonButton();
		labelReadableDesignationHeader = new KryptonLabel();
		labelValueHeader = new KryptonLabel();
		labelReadableDesignation01 = new KryptonLabel();
		labelReadableDesignation02 = new KryptonLabel();
		labelReadableDesignation03 = new KryptonLabel();
		labelReadableDesignation04 = new KryptonLabel();
		labelReadableDesignation05 = new KryptonLabel();
		labelReadableDesignation06 = new KryptonLabel();
		labelReadableDesignation07 = new KryptonLabel();
		labelReadableDesignation08 = new KryptonLabel();
		labelReadableDesignation09 = new KryptonLabel();
		labelReadableDesignation10 = new KryptonLabel();
		labelValue01 = new KryptonLabel();
		labelValue02 = new KryptonLabel();
		labelValue03 = new KryptonLabel();
		labelValue04 = new KryptonLabel();
		labelValue05 = new KryptonLabel();
		labelValue06 = new KryptonLabel();
		labelValue07 = new KryptonLabel();
		labelValue08 = new KryptonLabel();
		labelValue09 = new KryptonLabel();
		labelValue10 = new KryptonLabel();
		buttonGoto10 = new KryptonButton();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);
		toolStripContainer = new ToolStripContainer();
		kryptonToolStripGenerateList = new KryptonToolStrip();
		toolStripButtonStart = new ToolStripButton();
		toolStripButtonCancel = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		toolStripButtonSortOrderAscending = new ToolStripButton();
		toolStripButtonSortOrderDescending = new ToolStripButton();
		toolStripSeparator3 = new ToolStripSeparator();
		kryptonToolStripProgress = new KryptonToolStrip();
		toolStripLabelProgress = new ToolStripLabel();
		kryptonProgressBar = new KryptonProgressBarToolStripItem();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		((ISupportInitialize)splitContainerMain).BeginInit();
		splitContainerMain.Panel1.SuspendLayout();
		splitContainerMain.Panel2.SuspendLayout();
		splitContainerMain.SuspendLayout();
		((ISupportInitialize)kryptonPanelSplitterContainerLeft).BeginInit();
		kryptonPanelSplitterContainerLeft.SuspendLayout();
		((ISupportInitialize)kryptonPanelSplitterContainerRight).BeginInit();
		kryptonPanelSplitterContainerRight.SuspendLayout();
		tableLayoutPanel.SuspendLayout();
		contextMenuSaveToFile.SuspendLayout();
		contextMenuCopyToClipboard.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonToolStripGenerateList.SuspendLayout();
		kryptonToolStripProgress.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonPanelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Groups the data";
		kryptonPanelMain.AccessibleName = "Panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(splitContainerMain);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(1054, 341);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "kryptonPanelMain";
		kryptonPanelMain.Enter += Control_Enter;
		kryptonPanelMain.Leave += Control_Leave;
		kryptonPanelMain.MouseEnter += Control_Enter;
		kryptonPanelMain.MouseLeave += Control_Leave;
		// 
		// splitContainerMain
		// 
		splitContainerMain.AccessibleDescription = "Splits the main content";
		splitContainerMain.AccessibleName = "Split container";
		splitContainerMain.AccessibleRole = AccessibleRole.Grouping;
		splitContainerMain.Dock = DockStyle.Fill;
		splitContainerMain.Location = new Point(0, 0);
		splitContainerMain.Name = "splitContainerMain";
		// 
		// splitContainerMain.Panel1
		// 
		splitContainerMain.Panel1.AccessibleDescription = "Split container panel 1";
		splitContainerMain.Panel1.AccessibleName = "Split container panel 1";
		splitContainerMain.Panel1.AccessibleRole = AccessibleRole.Grouping;
		splitContainerMain.Panel1.Controls.Add(kryptonPanelSplitterContainerLeft);
		// 
		// splitContainerMain.Panel2
		// 
		splitContainerMain.Panel2.AccessibleDescription = "Split container panel 2";
		splitContainerMain.Panel2.AccessibleName = "Split container panel 2";
		splitContainerMain.Panel2.AccessibleRole = AccessibleRole.Grouping;
		splitContainerMain.Panel2.Controls.Add(kryptonPanelSplitterContainerRight);
		splitContainerMain.Size = new Size(1054, 341);
		splitContainerMain.SplitterDistance = 348;
		splitContainerMain.TabIndex = 0;
		splitContainerMain.Enter += Control_Enter;
		splitContainerMain.Leave += Control_Leave;
		splitContainerMain.MouseEnter += Control_Enter;
		splitContainerMain.MouseLeave += Control_Leave;
		// 
		// kryptonPanelSplitterContainerLeft
		// 
		kryptonPanelSplitterContainerLeft.AccessibleDescription = "Groups the data";
		kryptonPanelSplitterContainerLeft.AccessibleName = "Panel";
		kryptonPanelSplitterContainerLeft.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelSplitterContainerLeft.Controls.Add(listBox);
		kryptonPanelSplitterContainerLeft.Dock = DockStyle.Fill;
		kryptonPanelSplitterContainerLeft.Location = new Point(0, 0);
		kryptonPanelSplitterContainerLeft.Name = "kryptonPanelSplitterContainerLeft";
		kryptonPanelSplitterContainerLeft.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelSplitterContainerLeft.Size = new Size(348, 341);
		kryptonPanelSplitterContainerLeft.TabIndex = 2;
		kryptonPanelSplitterContainerLeft.TabStop = true;
		kryptonPanelSplitterContainerLeft.Text = "kryptonPanelSplitterContainerLeft";
		kryptonPanelSplitterContainerLeft.Enter += Control_Enter;
		kryptonPanelSplitterContainerLeft.Leave += Control_Leave;
		kryptonPanelSplitterContainerLeft.MouseEnter += Control_Enter;
		kryptonPanelSplitterContainerLeft.MouseLeave += Control_Leave;
		// 
		// listBox
		// 
		listBox.AccessibleDescription = "Lists the terms that can be looked up";
		listBox.AccessibleName = "Terms list";
		listBox.AccessibleRole = AccessibleRole.List;
		listBox.BackStyle = PaletteBackStyle.PanelClient;
		listBox.Dock = DockStyle.Fill;
		listBox.Items.AddRange(new object[] { "Mean anomaly at the epoch", "Argument of the perihelion, J2000.0", "Longitude of the ascending node, J2000.0", "Inclination to the ecliptic, J2000.0", "Orbital eccentricity", "Mean daily motion", "Semi-major axis", "Absolute magnitude, H", "Slope parameter, G", "Number of oppositions", "Number of observations", "r.m.s. residual" });
		listBox.Location = new Point(0, 0);
		listBox.Name = "listBox";
		listBox.Size = new Size(348, 341);
		listBox.TabIndex = 0;
		listBox.ToolTipValues.Description = "Lists the terms that can be looked up.\r\nDouble-click or right-click to copy the information to the clipboard.";
		listBox.ToolTipValues.EnableToolTips = true;
		listBox.ToolTipValues.Heading = "Terms list";
		listBox.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		listBox.Enter += Control_Enter;
		listBox.Leave += Control_Leave;
		listBox.MouseEnter += Control_Enter;
		listBox.MouseLeave += Control_Leave;
		// 
		// kryptonPanelSplitterContainerRight
		// 
		kryptonPanelSplitterContainerRight.AccessibleDescription = "Groups the data";
		kryptonPanelSplitterContainerRight.AccessibleName = "Panel";
		kryptonPanelSplitterContainerRight.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelSplitterContainerRight.Controls.Add(tableLayoutPanel);
		kryptonPanelSplitterContainerRight.Dock = DockStyle.Fill;
		kryptonPanelSplitterContainerRight.Location = new Point(0, 0);
		kryptonPanelSplitterContainerRight.Name = "kryptonPanelSplitterContainerRight";
		kryptonPanelSplitterContainerRight.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelSplitterContainerRight.Size = new Size(702, 341);
		kryptonPanelSplitterContainerRight.TabIndex = 1;
		kryptonPanelSplitterContainerRight.TabStop = true;
		kryptonPanelSplitterContainerRight.Text = "kryptonPanelSplitterContainerRight";
		kryptonPanelSplitterContainerRight.Enter += Control_Enter;
		kryptonPanelSplitterContainerRight.Leave += Control_Leave;
		kryptonPanelSplitterContainerRight.MouseEnter += Control_Enter;
		kryptonPanelSplitterContainerRight.MouseLeave += Control_Leave;
		// 
		// tableLayoutPanel
		// 
		tableLayoutPanel.AccessibleDescription = "Groups the data";
		tableLayoutPanel.AccessibleName = "Table panel";
		tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
		tableLayoutPanel.ColumnCount = 4;
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 233F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel.ContextMenuStrip = contextMenuSaveToFile;
		tableLayoutPanel.Controls.Add(labelGoToObjectHeader, 3, 0);
		tableLayoutPanel.Controls.Add(labelPlaceHeader, 0, 0);
		tableLayoutPanel.Controls.Add(buttonGoto09, 3, 9);
		tableLayoutPanel.Controls.Add(buttonGoto08, 3, 8);
		tableLayoutPanel.Controls.Add(buttonGoto07, 3, 7);
		tableLayoutPanel.Controls.Add(buttonGoto06, 3, 6);
		tableLayoutPanel.Controls.Add(buttonGoto05, 3, 5);
		tableLayoutPanel.Controls.Add(buttonGoto04, 3, 4);
		tableLayoutPanel.Controls.Add(buttonGoto03, 3, 3);
		tableLayoutPanel.Controls.Add(buttonGoto02, 3, 2);
		tableLayoutPanel.Controls.Add(labelPlace10, 0, 10);
		tableLayoutPanel.Controls.Add(labelPlace05, 0, 5);
		tableLayoutPanel.Controls.Add(labelPlace04, 0, 4);
		tableLayoutPanel.Controls.Add(labelPlace03, 0, 3);
		tableLayoutPanel.Controls.Add(labelPlace01, 0, 1);
		tableLayoutPanel.Controls.Add(labelPlace02, 0, 2);
		tableLayoutPanel.Controls.Add(labelPlace06, 0, 6);
		tableLayoutPanel.Controls.Add(labelPlace07, 0, 7);
		tableLayoutPanel.Controls.Add(labelPlace08, 0, 8);
		tableLayoutPanel.Controls.Add(labelPlace09, 0, 9);
		tableLayoutPanel.Controls.Add(buttonGoto01, 3, 1);
		tableLayoutPanel.Controls.Add(labelReadableDesignationHeader, 1, 0);
		tableLayoutPanel.Controls.Add(labelValueHeader, 2, 0);
		tableLayoutPanel.Controls.Add(labelReadableDesignation01, 1, 1);
		tableLayoutPanel.Controls.Add(labelReadableDesignation02, 1, 2);
		tableLayoutPanel.Controls.Add(labelReadableDesignation03, 1, 3);
		tableLayoutPanel.Controls.Add(labelReadableDesignation04, 1, 4);
		tableLayoutPanel.Controls.Add(labelReadableDesignation05, 1, 5);
		tableLayoutPanel.Controls.Add(labelReadableDesignation06, 1, 6);
		tableLayoutPanel.Controls.Add(labelReadableDesignation07, 1, 7);
		tableLayoutPanel.Controls.Add(labelReadableDesignation08, 1, 8);
		tableLayoutPanel.Controls.Add(labelReadableDesignation09, 1, 9);
		tableLayoutPanel.Controls.Add(labelReadableDesignation10, 1, 10);
		tableLayoutPanel.Controls.Add(labelValue01, 2, 1);
		tableLayoutPanel.Controls.Add(labelValue02, 2, 2);
		tableLayoutPanel.Controls.Add(labelValue03, 2, 3);
		tableLayoutPanel.Controls.Add(labelValue04, 2, 4);
		tableLayoutPanel.Controls.Add(labelValue05, 2, 5);
		tableLayoutPanel.Controls.Add(labelValue06, 2, 6);
		tableLayoutPanel.Controls.Add(labelValue07, 2, 7);
		tableLayoutPanel.Controls.Add(labelValue08, 2, 8);
		tableLayoutPanel.Controls.Add(labelValue09, 2, 9);
		tableLayoutPanel.Controls.Add(labelValue10, 2, 10);
		tableLayoutPanel.Controls.Add(buttonGoto10, 3, 10);
		tableLayoutPanel.Dock = DockStyle.Fill;
		tableLayoutPanel.Location = new Point(0, 0);
		tableLayoutPanel.Name = "tableLayoutPanel";
		tableLayoutPanel.RowCount = 11;
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.RowStyles.Add(new RowStyle());
		tableLayoutPanel.Size = new Size(702, 341);
		tableLayoutPanel.TabIndex = 0;
		tableLayoutPanel.Enter += Control_Enter;
		tableLayoutPanel.Leave += Control_Leave;
		tableLayoutPanel.MouseEnter += Control_Enter;
		tableLayoutPanel.MouseLeave += Control_Leave;
		// 
		// contextMenuSaveToFile
		// 
		contextMenuSaveToFile.AccessibleDescription = "Save the list as file";
		contextMenuSaveToFile.AccessibleName = "Save list";
		contextMenuSaveToFile.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuSaveToFile.AllowClickThrough = true;
		contextMenuSaveToFile.Font = new Font("Segoe UI", 9F);
		contextMenuSaveToFile.Items.AddRange(new ToolStripItem[] { toolStripMenuItemTextFiles, toolStripMenuItemWriterDocuments, toolStripMenuItemSpreadsheetDocuments, toolStripMenuItemXmlDocuments, toolStripMenuItemConfigurationFiles, toolStripMenuItemDatabaseScripts, toolStripMenuItemPortableDocuments });
		contextMenuSaveToFile.Name = "contextMenuSaveList";
		contextMenuSaveToFile.OwnerItem = toolStripDropDownButtonSaveList;
		contextMenuSaveToFile.Size = new Size(202, 158);
		contextMenuSaveToFile.TabStop = true;
		contextMenuSaveToFile.Text = "&Save list";
		contextMenuSaveToFile.Enter += Control_Enter;
		contextMenuSaveToFile.Leave += Control_Leave;
		contextMenuSaveToFile.MouseEnter += Control_Enter;
		contextMenuSaveToFile.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemTextFiles
		// 
		toolStripMenuItemTextFiles.AccessibleDescription = "Saves the list as text file";
		toolStripMenuItemTextFiles.AccessibleName = "Save as text file";
		toolStripMenuItemTextFiles.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemTextFiles.AutoToolTip = true;
		toolStripMenuItemTextFiles.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsText, toolStripMenuItemSaveAsLatex, toolStripMenuItemSaveAsMarkdown, toolStripMenuItemSaveAsAsciiDoc, toolStripMenuItemSaveAsReStructuredText, toolStripMenuItemSaveAsTextile });
		toolStripMenuItemTextFiles.Image = FatcowIcons16px.fatcow_file_extension_txt_16px;
		toolStripMenuItemTextFiles.Name = "toolStripMenuItemTextFiles";
		toolStripMenuItemTextFiles.Size = new Size(201, 22);
		toolStripMenuItemTextFiles.Text = "&Text files";
		toolStripMenuItemTextFiles.MouseEnter += Control_Enter;
		toolStripMenuItemTextFiles.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsText
		// 
		toolStripMenuItemSaveAsText.AccessibleDescription = "Saves the list as text file";
		toolStripMenuItemSaveAsText.AccessibleName = "Save as text";
		toolStripMenuItemSaveAsText.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsText.AutoToolTip = true;
		toolStripMenuItemSaveAsText.Image = FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsText.Name = "toolStripMenuItemSaveAsText";
		toolStripMenuItemSaveAsText.Size = new Size(201, 22);
		toolStripMenuItemSaveAsText.Text = "Save as &text";
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
		toolStripMenuItemSaveAsLatex.Size = new Size(201, 22);
		toolStripMenuItemSaveAsLatex.Text = "Save as &Latex";
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
		toolStripMenuItemSaveAsMarkdown.Size = new Size(201, 22);
		toolStripMenuItemSaveAsMarkdown.Text = "Save as &Markdown";
		toolStripMenuItemSaveAsMarkdown.Click += ToolStripMenuItemSaveAsMarkdown_Click;
		toolStripMenuItemSaveAsMarkdown.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsMarkdown.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsAsciiDoc
		// 
		toolStripMenuItemSaveAsAsciiDoc.AccessibleDescription = "Saves the list as AsciiDoc file";
		toolStripMenuItemSaveAsAsciiDoc.AccessibleName = "Save as AsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsAsciiDoc.AutoToolTip = true;
		toolStripMenuItemSaveAsAsciiDoc.Image = FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsAsciiDoc.Name = "toolStripMenuItemSaveAsAsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.Size = new Size(201, 22);
		toolStripMenuItemSaveAsAsciiDoc.Text = "Save as &AsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.Click += ToolStripMenuItemSaveAsAsciiDoc_Click;
		toolStripMenuItemSaveAsAsciiDoc.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsAsciiDoc.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsReStructuredText
		// 
		toolStripMenuItemSaveAsReStructuredText.AccessibleDescription = "Saves the list as reStructuredText file";
		toolStripMenuItemSaveAsReStructuredText.AccessibleName = "Save as reStructuredText";
		toolStripMenuItemSaveAsReStructuredText.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsReStructuredText.AutoToolTip = true;
		toolStripMenuItemSaveAsReStructuredText.Image = FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsReStructuredText.Name = "toolStripMenuItemSaveAsReStructuredText";
		toolStripMenuItemSaveAsReStructuredText.Size = new Size(201, 22);
		toolStripMenuItemSaveAsReStructuredText.Text = "Save as &reStructuredText";
		toolStripMenuItemSaveAsReStructuredText.Click += ToolStripMenuItemSaveAsReStructuredText_Click;
		toolStripMenuItemSaveAsReStructuredText.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsReStructuredText.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsTextile
		// 
		toolStripMenuItemSaveAsTextile.AccessibleDescription = "Saves the list as Textile file";
		toolStripMenuItemSaveAsTextile.AccessibleName = "Save as Textile";
		toolStripMenuItemSaveAsTextile.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsTextile.AutoToolTip = true;
		toolStripMenuItemSaveAsTextile.Image = FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsTextile.Name = "toolStripMenuItemSaveAsTextile";
		toolStripMenuItemSaveAsTextile.Size = new Size(201, 22);
		toolStripMenuItemSaveAsTextile.Text = "Save as Te&xtile";
		toolStripMenuItemSaveAsTextile.Click += ToolStripMenuItemSaveAsTextile_Click;
		toolStripMenuItemSaveAsTextile.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsTextile.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemWriterDocuments
		// 
		toolStripMenuItemWriterDocuments.AccessibleDescription = "Saves the list as writer document";
		toolStripMenuItemWriterDocuments.AccessibleName = "Save as writer document";
		toolStripMenuItemWriterDocuments.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemWriterDocuments.AutoToolTip = true;
		toolStripMenuItemWriterDocuments.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsWord, toolStripMenuItemSaveAsOdt, toolStripMenuItemSaveAsRtf, toolStripMenuItemSaveAsAbiword, toolStripMenuItemSaveAsWps });
		toolStripMenuItemWriterDocuments.Image = FatcowIcons16px.fatcow_file_extension_doc_16px;
		toolStripMenuItemWriterDocuments.Name = "toolStripMenuItemWriterDocuments";
		toolStripMenuItemWriterDocuments.Size = new Size(201, 22);
		toolStripMenuItemWriterDocuments.Text = "&Writer documents";
		toolStripMenuItemWriterDocuments.MouseEnter += Control_Enter;
		toolStripMenuItemWriterDocuments.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsWord
		// 
		toolStripMenuItemSaveAsWord.AccessibleDescription = "Saves the list as Word file";
		toolStripMenuItemSaveAsWord.AccessibleName = "Save as Word";
		toolStripMenuItemSaveAsWord.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsWord.AutoToolTip = true;
		toolStripMenuItemSaveAsWord.Image = FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsWord.Name = "toolStripMenuItemSaveAsWord";
		toolStripMenuItemSaveAsWord.Size = new Size(257, 22);
		toolStripMenuItemSaveAsWord.Text = "Save as &Word Text (DOCX)";
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
		toolStripMenuItemSaveAsOdt.Size = new Size(257, 22);
		toolStripMenuItemSaveAsOdt.Text = "Save as &OpenDocument Text (ODT)";
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
		toolStripMenuItemSaveAsRtf.Size = new Size(257, 22);
		toolStripMenuItemSaveAsRtf.Text = "Save as &Rich Text Format (RTF)";
		toolStripMenuItemSaveAsRtf.Click += ToolStripMenuItemSaveAsRtf_Click;
		toolStripMenuItemSaveAsRtf.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsRtf.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsAbiword
		// 
		toolStripMenuItemSaveAsAbiword.AccessibleDescription = "Saves the list as Abiword file";
		toolStripMenuItemSaveAsAbiword.AccessibleName = "Save as Abiword";
		toolStripMenuItemSaveAsAbiword.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsAbiword.AutoToolTip = true;
		toolStripMenuItemSaveAsAbiword.Image = FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsAbiword.Name = "toolStripMenuItemSaveAsAbiword";
		toolStripMenuItemSaveAsAbiword.Size = new Size(257, 22);
		toolStripMenuItemSaveAsAbiword.Text = "Save as &Abiword file (ABW)";
		toolStripMenuItemSaveAsAbiword.Click += ToolStripMenuItemSaveAsAbiword_Click;
		toolStripMenuItemSaveAsAbiword.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsAbiword.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsWps
		// 
		toolStripMenuItemSaveAsWps.AccessibleDescription = "Saves the list as WPS file";
		toolStripMenuItemSaveAsWps.AccessibleName = "Save as WPS";
		toolStripMenuItemSaveAsWps.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsWps.AutoToolTip = true;
		toolStripMenuItemSaveAsWps.Image = FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsWps.Name = "toolStripMenuItemSaveAsWps";
		toolStripMenuItemSaveAsWps.Size = new Size(257, 22);
		toolStripMenuItemSaveAsWps.Text = "Save as W&PS Office Writer (WPS)";
		toolStripMenuItemSaveAsWps.Click += ToolStripMenuItemSaveAsWps_Click;
		toolStripMenuItemSaveAsWps.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsWps.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSpreadsheetDocuments
		// 
		toolStripMenuItemSpreadsheetDocuments.AccessibleDescription = "Saves the list as spreadsheet document";
		toolStripMenuItemSpreadsheetDocuments.AccessibleName = "Save as spreadsheet document";
		toolStripMenuItemSpreadsheetDocuments.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSpreadsheetDocuments.AutoToolTip = true;
		toolStripMenuItemSpreadsheetDocuments.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsExcel, toolStripMenuItemSaveAsOds, toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsTsv, toolStripMenuItemSaveAsPsv, toolStripMenuItemSaveAsEt });
		toolStripMenuItemSpreadsheetDocuments.Image = FatcowIcons16px.fatcow_file_extension_xls_16px;
		toolStripMenuItemSpreadsheetDocuments.Name = "toolStripMenuItemSpreadsheetDocuments";
		toolStripMenuItemSpreadsheetDocuments.Size = new Size(201, 22);
		toolStripMenuItemSpreadsheetDocuments.Text = "&Spreadsheet documents";
		toolStripMenuItemSpreadsheetDocuments.MouseEnter += Control_Enter;
		toolStripMenuItemSpreadsheetDocuments.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsExcel
		// 
		toolStripMenuItemSaveAsExcel.AccessibleDescription = "Saves the list as Excel file";
		toolStripMenuItemSaveAsExcel.AccessibleName = "Save as Excel";
		toolStripMenuItemSaveAsExcel.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsExcel.AutoToolTip = true;
		toolStripMenuItemSaveAsExcel.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsExcel.Name = "toolStripMenuItemSaveAsExcel";
		toolStripMenuItemSaveAsExcel.Size = new Size(301, 22);
		toolStripMenuItemSaveAsExcel.Text = "Save as &Excel Spreadsheet (XLSX)";
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
		toolStripMenuItemSaveAsOds.Size = new Size(301, 22);
		toolStripMenuItemSaveAsOds.Text = "Save as &OpenDocument Spreadsheet (ODS)";
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
		toolStripMenuItemSaveAsCsv.Size = new Size(301, 22);
		toolStripMenuItemSaveAsCsv.Text = "Save as &Comma separated value (CSV)";
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
		toolStripMenuItemSaveAsTsv.Size = new Size(301, 22);
		toolStripMenuItemSaveAsTsv.Text = "Save as &Tabulator separated value (TSV)";
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
		toolStripMenuItemSaveAsPsv.Size = new Size(301, 22);
		toolStripMenuItemSaveAsPsv.Text = "Save as &Pipe separated value (PSV)";
		toolStripMenuItemSaveAsPsv.Click += ToolStripMenuItemSaveAsPsv_Click;
		toolStripMenuItemSaveAsPsv.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsPsv.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsEt
		// 
		toolStripMenuItemSaveAsEt.AccessibleDescription = "Saves the list as ET";
		toolStripMenuItemSaveAsEt.AccessibleName = "Save as ET";
		toolStripMenuItemSaveAsEt.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsEt.AutoToolTip = true;
		toolStripMenuItemSaveAsEt.Image = FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsEt.Name = "toolStripMenuItemSaveAsEt";
		toolStripMenuItemSaveAsEt.Size = new Size(301, 22);
		toolStripMenuItemSaveAsEt.Text = "Save as &WPS Office Spreadsheet (ET)";
		toolStripMenuItemSaveAsEt.Click += ToolStripMenuItemSaveAsEt_Click;
		toolStripMenuItemSaveAsEt.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsEt.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemXmlDocuments
		// 
		toolStripMenuItemXmlDocuments.AccessibleDescription = "Saves the list as XML documents";
		toolStripMenuItemXmlDocuments.AccessibleName = "Save as XML documents";
		toolStripMenuItemXmlDocuments.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemXmlDocuments.AutoToolTip = true;
		toolStripMenuItemXmlDocuments.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsDocBook });
		toolStripMenuItemXmlDocuments.Image = FatcowIcons16px.fatcow_file_extension_bin_16px;
		toolStripMenuItemXmlDocuments.Name = "toolStripMenuItemXmlDocuments";
		toolStripMenuItemXmlDocuments.Size = new Size(201, 22);
		toolStripMenuItemXmlDocuments.Text = "&XML documents";
		toolStripMenuItemXmlDocuments.MouseEnter += Control_Enter;
		toolStripMenuItemXmlDocuments.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsHtml
		// 
		toolStripMenuItemSaveAsHtml.AccessibleDescription = "Saves the list as HTML file";
		toolStripMenuItemSaveAsHtml.AccessibleName = "Save as HTML";
		toolStripMenuItemSaveAsHtml.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsHtml.AutoToolTip = true;
		toolStripMenuItemSaveAsHtml.Image = FatcowIcons16px.fatcow_page_white_code_16px;
		toolStripMenuItemSaveAsHtml.Name = "toolStripMenuItemSaveAsHtml";
		toolStripMenuItemSaveAsHtml.Size = new Size(163, 22);
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
		toolStripMenuItemSaveAsXml.Size = new Size(163, 22);
		toolStripMenuItemSaveAsXml.Text = "Save as &XML";
		toolStripMenuItemSaveAsXml.Click += ToolStripMenuItemSaveAsXml_Click;
		toolStripMenuItemSaveAsXml.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsXml.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsDocBook
		// 
		toolStripMenuItemSaveAsDocBook.AccessibleDescription = "Saves the list as DocBook file";
		toolStripMenuItemSaveAsDocBook.AccessibleName = "Save as DocBook";
		toolStripMenuItemSaveAsDocBook.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsDocBook.AutoToolTip = true;
		toolStripMenuItemSaveAsDocBook.Image = FatcowIcons16px.fatcow_page_white_code_16px;
		toolStripMenuItemSaveAsDocBook.Name = "toolStripMenuItemSaveAsDocBook";
		toolStripMenuItemSaveAsDocBook.Size = new Size(163, 22);
		toolStripMenuItemSaveAsDocBook.Text = "Save as &DocBook";
		toolStripMenuItemSaveAsDocBook.Click += ToolStripMenuItemSaveAsDocBook_Click;
		toolStripMenuItemSaveAsDocBook.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsDocBook.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemConfigurationFiles
		// 
		toolStripMenuItemConfigurationFiles.AccessibleDescription = "Saves the list as configuration file";
		toolStripMenuItemConfigurationFiles.AccessibleName = "Save as configuration file";
		toolStripMenuItemConfigurationFiles.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemConfigurationFiles.AutoToolTip = true;
		toolStripMenuItemConfigurationFiles.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsJson, toolStripMenuItemSaveAsToml, toolStripMenuItemSaveAsYaml });
		toolStripMenuItemConfigurationFiles.Image = FatcowIcons16px.fatcow_file_extension_bat_16px;
		toolStripMenuItemConfigurationFiles.Name = "toolStripMenuItemConfigurationFiles";
		toolStripMenuItemConfigurationFiles.Size = new Size(201, 22);
		toolStripMenuItemConfigurationFiles.Text = "&Configuration files";
		toolStripMenuItemConfigurationFiles.MouseEnter += Control_Enter;
		toolStripMenuItemConfigurationFiles.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsJson
		// 
		toolStripMenuItemSaveAsJson.AccessibleDescription = "Saves the list as JSON file";
		toolStripMenuItemSaveAsJson.AccessibleName = "Save as JSON";
		toolStripMenuItemSaveAsJson.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsJson.AutoToolTip = true;
		toolStripMenuItemSaveAsJson.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
		toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
		toolStripMenuItemSaveAsJson.Size = new Size(146, 22);
		toolStripMenuItemSaveAsJson.Text = "Save as &JSON";
		toolStripMenuItemSaveAsJson.Click += ToolStripMenuItemSaveAsJson_Click;
		toolStripMenuItemSaveAsJson.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsJson.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsToml
		// 
		toolStripMenuItemSaveAsToml.AccessibleDescription = "Saves the list as TOML file";
		toolStripMenuItemSaveAsToml.AccessibleName = "Save as TOML";
		toolStripMenuItemSaveAsToml.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsToml.AutoToolTip = true;
		toolStripMenuItemSaveAsToml.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
		toolStripMenuItemSaveAsToml.Name = "toolStripMenuItemSaveAsToml";
		toolStripMenuItemSaveAsToml.Size = new Size(146, 22);
		toolStripMenuItemSaveAsToml.Text = "Save as &TOML";
		toolStripMenuItemSaveAsToml.Click += ToolStripMenuItemSaveAsToml_Click;
		toolStripMenuItemSaveAsToml.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsToml.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsYaml
		// 
		toolStripMenuItemSaveAsYaml.AccessibleDescription = "Saves the list as YAML file";
		toolStripMenuItemSaveAsYaml.AccessibleName = "Save as YAML";
		toolStripMenuItemSaveAsYaml.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsYaml.AutoToolTip = true;
		toolStripMenuItemSaveAsYaml.Image = FatcowIcons16px.fatcow_page_white_code_red_16px;
		toolStripMenuItemSaveAsYaml.Name = "toolStripMenuItemSaveAsYaml";
		toolStripMenuItemSaveAsYaml.Size = new Size(146, 22);
		toolStripMenuItemSaveAsYaml.Text = "Save as &YAML";
		toolStripMenuItemSaveAsYaml.Click += ToolStripMenuItemSaveAsYaml_Click;
		toolStripMenuItemSaveAsYaml.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsYaml.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemDatabaseScripts
		// 
		toolStripMenuItemDatabaseScripts.AccessibleDescription = "Saves the list as database script";
		toolStripMenuItemDatabaseScripts.AccessibleName = "Save as database script";
		toolStripMenuItemDatabaseScripts.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemDatabaseScripts.AutoToolTip = true;
		toolStripMenuItemDatabaseScripts.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsSql, toolStripMenuItemSaveAsSqlite });
		toolStripMenuItemDatabaseScripts.Image = FatcowIcons16px.fatcow_file_extension_ptb_16px;
		toolStripMenuItemDatabaseScripts.Name = "toolStripMenuItemDatabaseScripts";
		toolStripMenuItemDatabaseScripts.Size = new Size(201, 22);
		toolStripMenuItemDatabaseScripts.Text = "&Database scripts";
		toolStripMenuItemDatabaseScripts.MouseEnter += Control_Enter;
		toolStripMenuItemDatabaseScripts.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsSql
		// 
		toolStripMenuItemSaveAsSql.AccessibleDescription = "Saves the list as SQL script";
		toolStripMenuItemSaveAsSql.AccessibleName = "Save as SQL";
		toolStripMenuItemSaveAsSql.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsSql.AutoToolTip = true;
		toolStripMenuItemSaveAsSql.Image = FatcowIcons16px.fatcow_page_white_database_16px;
		toolStripMenuItemSaveAsSql.Name = "toolStripMenuItemSaveAsSql";
		toolStripMenuItemSaveAsSql.Size = new Size(168, 22);
		toolStripMenuItemSaveAsSql.Text = "Save as &SQL script";
		toolStripMenuItemSaveAsSql.Click += ToolStripMenuItemSaveAsSql_Click;
		toolStripMenuItemSaveAsSql.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsSql.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsSqlite
		// 
		toolStripMenuItemSaveAsSqlite.AccessibleDescription = "Saves the list as SQLite file";
		toolStripMenuItemSaveAsSqlite.AccessibleName = "Save as SQLite";
		toolStripMenuItemSaveAsSqlite.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsSqlite.AutoToolTip = true;
		toolStripMenuItemSaveAsSqlite.Image = FatcowIcons16px.fatcow_page_white_database_16px;
		toolStripMenuItemSaveAsSqlite.Name = "toolStripMenuItemSaveAsSqlite";
		toolStripMenuItemSaveAsSqlite.Size = new Size(168, 22);
		toolStripMenuItemSaveAsSqlite.Text = "Save as SQ&Lite";
		toolStripMenuItemSaveAsSqlite.Click += ToolStripMenuItemSaveAsSqlite_Click;
		toolStripMenuItemSaveAsSqlite.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsSqlite.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemPortableDocuments
		// 
		toolStripMenuItemPortableDocuments.AccessibleDescription = "Saves the list as portable document";
		toolStripMenuItemPortableDocuments.AccessibleName = "Save as portable document";
		toolStripMenuItemPortableDocuments.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemPortableDocuments.AutoToolTip = true;
		toolStripMenuItemPortableDocuments.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsPdf, toolStripMenuItemSaveAsPostScript, toolStripMenuItemSaveAsEpub, toolStripMenuItemSaveAsMobi, toolStripMenuItemSaveAsXps, toolStripMenuItemSaveAsFictionBook2, toolStripMenuItemSaveAsChm });
		toolStripMenuItemPortableDocuments.Image = FatcowIcons16px.fatcow_file_extension_pdf_16px;
		toolStripMenuItemPortableDocuments.Name = "toolStripMenuItemPortableDocuments";
		toolStripMenuItemPortableDocuments.Size = new Size(201, 22);
		toolStripMenuItemPortableDocuments.Text = "&Portable documents";
		toolStripMenuItemPortableDocuments.MouseEnter += Control_Enter;
		toolStripMenuItemPortableDocuments.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsPdf
		// 
		toolStripMenuItemSaveAsPdf.AccessibleDescription = "Saves the list as PDF file";
		toolStripMenuItemSaveAsPdf.AccessibleName = "Save as PDF";
		toolStripMenuItemSaveAsPdf.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsPdf.AutoToolTip = true;
		toolStripMenuItemSaveAsPdf.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsPdf.Name = "toolStripMenuItemSaveAsPdf";
		toolStripMenuItemSaveAsPdf.Size = new Size(214, 22);
		toolStripMenuItemSaveAsPdf.Text = "Save as &PDF";
		toolStripMenuItemSaveAsPdf.Click += ToolStripMenuItemSaveAsPdf_Click;
		toolStripMenuItemSaveAsPdf.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsPdf.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsPostScript
		// 
		toolStripMenuItemSaveAsPostScript.AccessibleDescription = "Saves the list as PostScript file";
		toolStripMenuItemSaveAsPostScript.AccessibleName = "Save as PostScript";
		toolStripMenuItemSaveAsPostScript.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsPostScript.AutoToolTip = true;
		toolStripMenuItemSaveAsPostScript.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsPostScript.Name = "toolStripMenuItemSaveAsPostScript";
		toolStripMenuItemSaveAsPostScript.Size = new Size(214, 22);
		toolStripMenuItemSaveAsPostScript.Text = "Save as Post&Script (PS)";
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
		toolStripMenuItemSaveAsEpub.Size = new Size(214, 22);
		toolStripMenuItemSaveAsEpub.Text = "Save as &EPUB";
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
		toolStripMenuItemSaveAsMobi.Size = new Size(214, 22);
		toolStripMenuItemSaveAsMobi.Text = "Save as &MOBI";
		toolStripMenuItemSaveAsMobi.Click += ToolStripMenuItemSaveAsMobi_Click;
		toolStripMenuItemSaveAsMobi.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsMobi.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsXps
		// 
		toolStripMenuItemSaveAsXps.AccessibleDescription = "Saves the list as XPS file";
		toolStripMenuItemSaveAsXps.AccessibleName = "Save as XPS";
		toolStripMenuItemSaveAsXps.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsXps.AutoToolTip = true;
		toolStripMenuItemSaveAsXps.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsXps.Name = "toolStripMenuItemSaveAsXps";
		toolStripMenuItemSaveAsXps.Size = new Size(214, 22);
		toolStripMenuItemSaveAsXps.Text = "Save as &XPS";
		toolStripMenuItemSaveAsXps.Click += ToolStripMenuItemSaveAsXps_Click;
		toolStripMenuItemSaveAsXps.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsXps.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsFictionBook2
		// 
		toolStripMenuItemSaveAsFictionBook2.AccessibleDescription = "Saves the list as FictionBook2 file";
		toolStripMenuItemSaveAsFictionBook2.AccessibleName = "Save as FB2";
		toolStripMenuItemSaveAsFictionBook2.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsFictionBook2.AutoToolTip = true;
		toolStripMenuItemSaveAsFictionBook2.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsFictionBook2.Name = "toolStripMenuItemSaveAsFictionBook2";
		toolStripMenuItemSaveAsFictionBook2.Size = new Size(214, 22);
		toolStripMenuItemSaveAsFictionBook2.Text = "Save as &FictionBook2 (FB2)";
		toolStripMenuItemSaveAsFictionBook2.Click += ToolStripMenuItemSaveAsFictionBook2_Click;
		toolStripMenuItemSaveAsFictionBook2.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsFictionBook2.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsChm
		// 
		toolStripMenuItemSaveAsChm.AccessibleDescription = "Saves the list as CHM file";
		toolStripMenuItemSaveAsChm.AccessibleName = "Save as CHM";
		toolStripMenuItemSaveAsChm.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsChm.AutoToolTip = true;
		toolStripMenuItemSaveAsChm.Image = FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsChm.Name = "toolStripMenuItemSaveAsChm";
		toolStripMenuItemSaveAsChm.Size = new Size(214, 22);
		toolStripMenuItemSaveAsChm.Text = "Save as &CHM";
		toolStripMenuItemSaveAsChm.Click += ToolStripMenuItemSaveAsChm_Click;
		toolStripMenuItemSaveAsChm.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsChm.MouseLeave += Control_Leave;
		// 
		// toolStripDropDownButtonSaveList
		// 
		toolStripDropDownButtonSaveList.AccessibleDescription = "Saves the list as file";
		toolStripDropDownButtonSaveList.AccessibleName = "Save list";
		toolStripDropDownButtonSaveList.AccessibleRole = AccessibleRole.ButtonDropDown;
		toolStripDropDownButtonSaveList.DropDown = contextMenuSaveToFile;
		toolStripDropDownButtonSaveList.Image = FatcowIcons16px.fatcow_diskette_16px;
		toolStripDropDownButtonSaveList.ImageTransparentColor = Color.Magenta;
		toolStripDropDownButtonSaveList.Name = "toolStripDropDownButtonSaveList";
		toolStripDropDownButtonSaveList.Size = new Size(78, 22);
		toolStripDropDownButtonSaveList.Text = "&Save list";
		toolStripDropDownButtonSaveList.MouseEnter += Control_Enter;
		toolStripDropDownButtonSaveList.MouseLeave += Control_Leave;
		// 
		// labelGoToObjectHeader
		// 
		labelGoToObjectHeader.AccessibleDescription = "Shows the go to object header";
		labelGoToObjectHeader.AccessibleName = "Go to object header";
		labelGoToObjectHeader.AccessibleRole = AccessibleRole.StaticText;
		labelGoToObjectHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelGoToObjectHeader.Dock = DockStyle.Fill;
		labelGoToObjectHeader.LabelStyle = LabelStyle.BoldPanel;
		labelGoToObjectHeader.Location = new Point(615, 3);
		labelGoToObjectHeader.Name = "labelGoToObjectHeader";
		labelGoToObjectHeader.Size = new Size(140, 36);
		labelGoToObjectHeader.TabIndex = 3;
		labelGoToObjectHeader.ToolTipValues.Description = "Shows the go to object header.";
		labelGoToObjectHeader.ToolTipValues.EnableToolTips = true;
		labelGoToObjectHeader.ToolTipValues.Heading = "Go to object header";
		labelGoToObjectHeader.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelGoToObjectHeader.Values.Text = "Go to object";
		labelGoToObjectHeader.Enter += Control_Enter;
		labelGoToObjectHeader.Leave += Control_Leave;
		labelGoToObjectHeader.MouseDown += Control_MouseDown;
		labelGoToObjectHeader.MouseEnter += Control_Enter;
		labelGoToObjectHeader.MouseLeave += Control_Leave;
		// 
		// contextMenuCopyToClipboard
		// 
		contextMenuCopyToClipboard.AccessibleDescription = "Shows the context menu for copying database information to the clipboard";
		contextMenuCopyToClipboard.AccessibleName = "Context menu for copying database information to the clipboard";
		contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuCopyToClipboard.AllowClickThrough = true;
		contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
		contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCopyToClipboardInContextMenu });
		contextMenuCopyToClipboard.Name = "contextMenuStrip";
		contextMenuCopyToClipboard.Size = new Size(212, 26);
		contextMenuCopyToClipboard.TabStop = true;
		contextMenuCopyToClipboard.Text = "Copy to clipboard";
		contextMenuCopyToClipboard.Enter += Control_Enter;
		contextMenuCopyToClipboard.Leave += Control_Leave;
		contextMenuCopyToClipboard.MouseEnter += Control_Enter;
		contextMenuCopyToClipboard.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemCopyToClipboardInContextMenu
		// 
		toolStripMenuItemCopyToClipboardInContextMenu.AccessibleDescription = "Copies the text/value to the clipboard";
		toolStripMenuItemCopyToClipboardInContextMenu.AccessibleName = "Copy to clipboard";
		toolStripMenuItemCopyToClipboardInContextMenu.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemCopyToClipboardInContextMenu.AutoToolTip = true;
		toolStripMenuItemCopyToClipboardInContextMenu.Image = FatcowIcons16px.fatcow_page_copy_16px;
		toolStripMenuItemCopyToClipboardInContextMenu.Name = "toolStripMenuItemCopyToClipboardInContextMenu";
		toolStripMenuItemCopyToClipboardInContextMenu.ShortcutKeyDisplayString = "Ctrl+C";
		toolStripMenuItemCopyToClipboardInContextMenu.ShortcutKeys = Keys.Control | Keys.C;
		toolStripMenuItemCopyToClipboardInContextMenu.Size = new Size(211, 22);
		toolStripMenuItemCopyToClipboardInContextMenu.Text = "&Copy to clipboard";
		toolStripMenuItemCopyToClipboardInContextMenu.Click += CopyToClipboard_DoubleClick;
		toolStripMenuItemCopyToClipboardInContextMenu.MouseEnter += Control_Enter;
		toolStripMenuItemCopyToClipboardInContextMenu.MouseLeave += Control_Leave;
		// 
		// labelPlaceHeader
		// 
		labelPlaceHeader.AccessibleDescription = "Shows the place header";
		labelPlaceHeader.AccessibleName = "Place header";
		labelPlaceHeader.AccessibleRole = AccessibleRole.StaticText;
		labelPlaceHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlaceHeader.Dock = DockStyle.Fill;
		labelPlaceHeader.LabelStyle = LabelStyle.BoldPanel;
		labelPlaceHeader.Location = new Point(3, 3);
		labelPlaceHeader.Name = "labelPlaceHeader";
		labelPlaceHeader.Size = new Size(227, 36);
		labelPlaceHeader.TabIndex = 0;
		labelPlaceHeader.ToolTipValues.Description = "Shows the place header.";
		labelPlaceHeader.ToolTipValues.EnableToolTips = true;
		labelPlaceHeader.ToolTipValues.Heading = "Place header";
		labelPlaceHeader.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlaceHeader.Values.Text = "Place";
		labelPlaceHeader.Enter += Control_Enter;
		labelPlaceHeader.Leave += Control_Leave;
		labelPlaceHeader.MouseDown += Control_MouseDown;
		labelPlaceHeader.MouseEnter += Control_Enter;
		labelPlaceHeader.MouseLeave += Control_Leave;
		// 
		// buttonGoto09
		// 
		buttonGoto09.AccessibleDescription = "Goes to the element of the place no. 9";
		buttonGoto09.AccessibleName = "Go to the element of the place no. 9";
		buttonGoto09.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto09.ButtonStyle = ButtonStyle.Form;
		buttonGoto09.Dock = DockStyle.Fill;
		buttonGoto09.Location = new Point(615, 277);
		buttonGoto09.Name = "buttonGoto09";
		buttonGoto09.Size = new Size(140, 23);
		buttonGoto09.TabIndex = 39;
		buttonGoto09.ToolTipValues.Description = "Goes to the element of the place no. 9.";
		buttonGoto09.ToolTipValues.EnableToolTips = true;
		buttonGoto09.ToolTipValues.Heading = "Go to the element of the place no. 9";
		buttonGoto09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto09.Values.DropDownArrowColor = Color.Empty;
		buttonGoto09.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto09.Values.Text = "Goto";
		buttonGoto09.Click += Goto09_Click;
		buttonGoto09.Enter += Control_Enter;
		buttonGoto09.Leave += Control_Leave;
		buttonGoto09.MouseEnter += Control_Enter;
		buttonGoto09.MouseLeave += Control_Leave;
		// 
		// buttonGoto08
		// 
		buttonGoto08.AccessibleDescription = "Goes to the element of the place no. 8";
		buttonGoto08.AccessibleName = "Go to the element of the place no. 8";
		buttonGoto08.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto08.ButtonStyle = ButtonStyle.Form;
		buttonGoto08.Dock = DockStyle.Fill;
		buttonGoto08.Location = new Point(615, 248);
		buttonGoto08.Name = "buttonGoto08";
		buttonGoto08.Size = new Size(140, 23);
		buttonGoto08.TabIndex = 35;
		buttonGoto08.ToolTipValues.Description = "Goes to the element of the place no. 8.";
		buttonGoto08.ToolTipValues.EnableToolTips = true;
		buttonGoto08.ToolTipValues.Heading = "Go to the element of the place no. 8";
		buttonGoto08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto08.Values.DropDownArrowColor = Color.Empty;
		buttonGoto08.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto08.Values.Text = "Goto";
		buttonGoto08.Click += Goto08_Click;
		buttonGoto08.Enter += Control_Enter;
		buttonGoto08.Leave += Control_Leave;
		buttonGoto08.MouseEnter += Control_Enter;
		buttonGoto08.MouseLeave += Control_Leave;
		// 
		// buttonGoto07
		// 
		buttonGoto07.AccessibleDescription = "Goes to the element of the place no. 7";
		buttonGoto07.AccessibleName = "Go to the element of the place no. 7";
		buttonGoto07.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto07.ButtonStyle = ButtonStyle.Form;
		buttonGoto07.Dock = DockStyle.Fill;
		buttonGoto07.Location = new Point(615, 219);
		buttonGoto07.Name = "buttonGoto07";
		buttonGoto07.Size = new Size(140, 23);
		buttonGoto07.TabIndex = 31;
		buttonGoto07.ToolTipValues.Description = "Goes to the element of the place no. 7.";
		buttonGoto07.ToolTipValues.EnableToolTips = true;
		buttonGoto07.ToolTipValues.Heading = "Go to the element of the place no. 7";
		buttonGoto07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto07.Values.DropDownArrowColor = Color.Empty;
		buttonGoto07.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto07.Values.Text = "Goto";
		buttonGoto07.Click += Goto07_Click;
		buttonGoto07.Enter += Control_Enter;
		buttonGoto07.Leave += Control_Leave;
		buttonGoto07.MouseEnter += Control_Enter;
		buttonGoto07.MouseLeave += Control_Leave;
		// 
		// buttonGoto06
		// 
		buttonGoto06.AccessibleDescription = "Goes to the element of the place no. 6";
		buttonGoto06.AccessibleName = "Go to the element of the place no. 6";
		buttonGoto06.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto06.ButtonStyle = ButtonStyle.Form;
		buttonGoto06.Dock = DockStyle.Fill;
		buttonGoto06.Location = new Point(615, 190);
		buttonGoto06.Name = "buttonGoto06";
		buttonGoto06.Size = new Size(140, 23);
		buttonGoto06.TabIndex = 27;
		buttonGoto06.ToolTipValues.Description = "Goes to the element of the place no. 6.";
		buttonGoto06.ToolTipValues.EnableToolTips = true;
		buttonGoto06.ToolTipValues.Heading = "Go to the element of the place no. 6";
		buttonGoto06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto06.Values.DropDownArrowColor = Color.Empty;
		buttonGoto06.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto06.Values.Text = "Goto";
		buttonGoto06.Click += Goto06_Click;
		buttonGoto06.Enter += Control_Enter;
		buttonGoto06.Leave += Control_Leave;
		buttonGoto06.MouseEnter += Control_Enter;
		buttonGoto06.MouseLeave += Control_Leave;
		// 
		// buttonGoto05
		// 
		buttonGoto05.AccessibleDescription = "Goes to the element of the place no. 5";
		buttonGoto05.AccessibleName = "Go to the element of the place no. 5";
		buttonGoto05.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto05.ButtonStyle = ButtonStyle.Form;
		buttonGoto05.Dock = DockStyle.Fill;
		buttonGoto05.Location = new Point(615, 161);
		buttonGoto05.Name = "buttonGoto05";
		buttonGoto05.Size = new Size(140, 23);
		buttonGoto05.TabIndex = 23;
		buttonGoto05.ToolTipValues.Description = "Goes to the element of the place no. 5.";
		buttonGoto05.ToolTipValues.EnableToolTips = true;
		buttonGoto05.ToolTipValues.Heading = "Go to the element of the place no. 5";
		buttonGoto05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto05.Values.DropDownArrowColor = Color.Empty;
		buttonGoto05.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto05.Values.Text = "Goto";
		buttonGoto05.Click += Goto05_Click;
		buttonGoto05.Enter += Control_Enter;
		buttonGoto05.Leave += Control_Leave;
		buttonGoto05.MouseEnter += Control_Enter;
		buttonGoto05.MouseLeave += Control_Leave;
		// 
		// buttonGoto04
		// 
		buttonGoto04.AccessibleDescription = "Goes to the element of the place no. 4";
		buttonGoto04.AccessibleName = "Go to the element of the place no. 4";
		buttonGoto04.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto04.ButtonStyle = ButtonStyle.Form;
		buttonGoto04.Dock = DockStyle.Fill;
		buttonGoto04.Location = new Point(615, 132);
		buttonGoto04.Name = "buttonGoto04";
		buttonGoto04.Size = new Size(140, 23);
		buttonGoto04.TabIndex = 19;
		buttonGoto04.ToolTipValues.Description = "Goes to the element of the place no. 4.";
		buttonGoto04.ToolTipValues.EnableToolTips = true;
		buttonGoto04.ToolTipValues.Heading = "Go to the element of the place no. 4";
		buttonGoto04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto04.Values.DropDownArrowColor = Color.Empty;
		buttonGoto04.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto04.Values.Text = "Goto";
		buttonGoto04.Click += Goto04_Click;
		buttonGoto04.Enter += Control_Enter;
		buttonGoto04.Leave += Control_Leave;
		buttonGoto04.MouseEnter += Control_Enter;
		buttonGoto04.MouseLeave += Control_Leave;
		// 
		// buttonGoto03
		// 
		buttonGoto03.AccessibleDescription = "Goes to the element of the place no. 3";
		buttonGoto03.AccessibleName = "Go to the element of the place no. 3";
		buttonGoto03.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto03.ButtonStyle = ButtonStyle.Form;
		buttonGoto03.Dock = DockStyle.Fill;
		buttonGoto03.Location = new Point(615, 103);
		buttonGoto03.Name = "buttonGoto03";
		buttonGoto03.Size = new Size(140, 23);
		buttonGoto03.TabIndex = 15;
		buttonGoto03.ToolTipValues.Description = "Goes to the element of the place no. 3.";
		buttonGoto03.ToolTipValues.EnableToolTips = true;
		buttonGoto03.ToolTipValues.Heading = "Go to the element of the place no. 3";
		buttonGoto03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto03.Values.DropDownArrowColor = Color.Empty;
		buttonGoto03.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto03.Values.Text = "Goto";
		buttonGoto03.Click += Goto03_Click;
		buttonGoto03.Enter += Control_Enter;
		buttonGoto03.Leave += Control_Leave;
		buttonGoto03.MouseEnter += Control_Enter;
		buttonGoto03.MouseLeave += Control_Leave;
		// 
		// buttonGoto02
		// 
		buttonGoto02.AccessibleDescription = "Goes to the element of the place no. 2";
		buttonGoto02.AccessibleName = "Go to the element of the place no. 2";
		buttonGoto02.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto02.ButtonStyle = ButtonStyle.Form;
		buttonGoto02.Dock = DockStyle.Fill;
		buttonGoto02.Location = new Point(615, 74);
		buttonGoto02.Name = "buttonGoto02";
		buttonGoto02.Size = new Size(140, 23);
		buttonGoto02.TabIndex = 11;
		buttonGoto02.ToolTipValues.Description = "Goes to the element of the place no. 2.";
		buttonGoto02.ToolTipValues.EnableToolTips = true;
		buttonGoto02.ToolTipValues.Heading = "Go to the element of the place no. 2";
		buttonGoto02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto02.Values.DropDownArrowColor = Color.Empty;
		buttonGoto02.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto02.Values.Text = "Goto";
		buttonGoto02.Click += Goto02_Click;
		buttonGoto02.Enter += Control_Enter;
		buttonGoto02.Leave += Control_Leave;
		buttonGoto02.MouseEnter += Control_Enter;
		buttonGoto02.MouseLeave += Control_Leave;
		// 
		// labelPlace10
		// 
		labelPlace10.AccessibleDescription = "Shows the record place no. 10";
		labelPlace10.AccessibleName = "Record place no. 10";
		labelPlace10.AccessibleRole = AccessibleRole.StaticText;
		labelPlace10.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace10.Dock = DockStyle.Fill;
		labelPlace10.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace10.Location = new Point(3, 306);
		labelPlace10.Name = "labelPlace10";
		labelPlace10.Size = new Size(227, 42);
		labelPlace10.TabIndex = 40;
		labelPlace10.ToolTipValues.Description = "Shows the record place no. 10.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace10.ToolTipValues.EnableToolTips = true;
		labelPlace10.ToolTipValues.Heading = "Record place no. 10";
		labelPlace10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace10.Values.Text = "10";
		labelPlace10.Enter += Control_Enter;
		labelPlace10.Leave += Control_Leave;
		labelPlace10.MouseDown += Control_MouseDown;
		labelPlace10.MouseEnter += Control_Enter;
		labelPlace10.MouseLeave += Control_Leave;
		// 
		// labelPlace05
		// 
		labelPlace05.AccessibleDescription = "Shows the record place no. 5";
		labelPlace05.AccessibleName = "Record place no. 5";
		labelPlace05.AccessibleRole = AccessibleRole.StaticText;
		labelPlace05.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace05.Dock = DockStyle.Fill;
		labelPlace05.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace05.Location = new Point(3, 161);
		labelPlace05.Name = "labelPlace05";
		labelPlace05.Size = new Size(227, 23);
		labelPlace05.TabIndex = 20;
		labelPlace05.ToolTipValues.Description = "Shows the record place no. 5.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace05.ToolTipValues.EnableToolTips = true;
		labelPlace05.ToolTipValues.Heading = "Record place no. 5";
		labelPlace05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace05.Values.Text = "5";
		labelPlace05.Enter += Control_Enter;
		labelPlace05.Leave += Control_Leave;
		labelPlace05.MouseDown += Control_MouseDown;
		labelPlace05.MouseEnter += Control_Enter;
		labelPlace05.MouseLeave += Control_Leave;
		// 
		// labelPlace04
		// 
		labelPlace04.AccessibleDescription = "Shows the record place no. 4";
		labelPlace04.AccessibleName = "Record place no. 4";
		labelPlace04.AccessibleRole = AccessibleRole.StaticText;
		labelPlace04.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace04.Dock = DockStyle.Fill;
		labelPlace04.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace04.Location = new Point(3, 132);
		labelPlace04.Name = "labelPlace04";
		labelPlace04.Size = new Size(227, 23);
		labelPlace04.TabIndex = 16;
		labelPlace04.ToolTipValues.Description = "Shows the record place no. 4.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace04.ToolTipValues.EnableToolTips = true;
		labelPlace04.ToolTipValues.Heading = "Record place no. 4";
		labelPlace04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace04.Values.Text = "4";
		labelPlace04.Enter += Control_Enter;
		labelPlace04.Leave += Control_Leave;
		labelPlace04.MouseDown += Control_MouseDown;
		labelPlace04.MouseEnter += Control_Enter;
		labelPlace04.MouseLeave += Control_Leave;
		// 
		// labelPlace03
		// 
		labelPlace03.AccessibleDescription = "Shows the record place no. 3";
		labelPlace03.AccessibleName = "Record place no. 3";
		labelPlace03.AccessibleRole = AccessibleRole.StaticText;
		labelPlace03.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace03.Dock = DockStyle.Fill;
		labelPlace03.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace03.Location = new Point(3, 103);
		labelPlace03.Name = "labelPlace03";
		labelPlace03.Size = new Size(227, 23);
		labelPlace03.TabIndex = 12;
		labelPlace03.ToolTipValues.Description = "Shows the record place no. 3.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace03.ToolTipValues.EnableToolTips = true;
		labelPlace03.ToolTipValues.Heading = "Record place no. 3";
		labelPlace03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace03.Values.Text = "3";
		labelPlace03.Enter += Control_Enter;
		labelPlace03.Leave += Control_Leave;
		labelPlace03.MouseDown += Control_MouseDown;
		labelPlace03.MouseEnter += Control_Enter;
		labelPlace03.MouseLeave += Control_Leave;
		// 
		// labelPlace01
		// 
		labelPlace01.AccessibleDescription = "Shows the record place no.";
		labelPlace01.AccessibleName = "Record place no.";
		labelPlace01.AccessibleRole = AccessibleRole.StaticText;
		labelPlace01.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace01.Dock = DockStyle.Fill;
		labelPlace01.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace01.Location = new Point(3, 45);
		labelPlace01.Name = "labelPlace01";
		labelPlace01.Size = new Size(227, 23);
		labelPlace01.TabIndex = 4;
		labelPlace01.ToolTipValues.Description = "Shows the record place no 1.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace01.ToolTipValues.EnableToolTips = true;
		labelPlace01.ToolTipValues.Heading = "Record place no.";
		labelPlace01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace01.Values.Text = "1";
		labelPlace01.Enter += Control_Enter;
		labelPlace01.Leave += Control_Leave;
		labelPlace01.MouseDown += Control_MouseDown;
		labelPlace01.MouseEnter += Control_Enter;
		labelPlace01.MouseLeave += Control_Leave;
		// 
		// labelPlace02
		// 
		labelPlace02.AccessibleDescription = "Shows the record place no. 2";
		labelPlace02.AccessibleName = "Record place no. 2";
		labelPlace02.AccessibleRole = AccessibleRole.StaticText;
		labelPlace02.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace02.Dock = DockStyle.Fill;
		labelPlace02.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace02.Location = new Point(3, 74);
		labelPlace02.Name = "labelPlace02";
		labelPlace02.Size = new Size(227, 23);
		labelPlace02.TabIndex = 8;
		labelPlace02.ToolTipValues.Description = "Shows the record place no. 2.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace02.ToolTipValues.EnableToolTips = true;
		labelPlace02.ToolTipValues.Heading = "Record place no. 2";
		labelPlace02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace02.Values.Text = "2";
		labelPlace02.Enter += Control_Enter;
		labelPlace02.Leave += Control_Leave;
		labelPlace02.MouseDown += Control_MouseDown;
		labelPlace02.MouseEnter += Control_Enter;
		labelPlace02.MouseLeave += Control_Leave;
		// 
		// labelPlace06
		// 
		labelPlace06.AccessibleDescription = "Shows the record place no. 6";
		labelPlace06.AccessibleName = "Record place no. 6";
		labelPlace06.AccessibleRole = AccessibleRole.StaticText;
		labelPlace06.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace06.Dock = DockStyle.Fill;
		labelPlace06.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace06.Location = new Point(3, 190);
		labelPlace06.Name = "labelPlace06";
		labelPlace06.Size = new Size(227, 23);
		labelPlace06.TabIndex = 24;
		labelPlace06.ToolTipValues.Description = "Shows the record place no. 6.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace06.ToolTipValues.EnableToolTips = true;
		labelPlace06.ToolTipValues.Heading = "Record place no. 6";
		labelPlace06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace06.Values.Text = "6";
		labelPlace06.Enter += Control_Enter;
		labelPlace06.Leave += Control_Leave;
		labelPlace06.MouseDown += Control_MouseDown;
		labelPlace06.MouseEnter += Control_Enter;
		labelPlace06.MouseLeave += Control_Leave;
		// 
		// labelPlace07
		// 
		labelPlace07.AccessibleDescription = "Shows the record place no. 7";
		labelPlace07.AccessibleName = "Record place no. 7";
		labelPlace07.AccessibleRole = AccessibleRole.StaticText;
		labelPlace07.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace07.Dock = DockStyle.Fill;
		labelPlace07.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace07.Location = new Point(3, 219);
		labelPlace07.Name = "labelPlace07";
		labelPlace07.Size = new Size(227, 23);
		labelPlace07.TabIndex = 28;
		labelPlace07.ToolTipValues.Description = "Shows the record place no. 7.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace07.ToolTipValues.EnableToolTips = true;
		labelPlace07.ToolTipValues.Heading = "Record place no. 7";
		labelPlace07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace07.Values.Text = "7";
		labelPlace07.Enter += Control_Enter;
		labelPlace07.Leave += Control_Leave;
		labelPlace07.MouseDown += Control_MouseDown;
		labelPlace07.MouseEnter += Control_Enter;
		labelPlace07.MouseLeave += Control_Leave;
		// 
		// labelPlace08
		// 
		labelPlace08.AccessibleDescription = "Shows the record place no. 8";
		labelPlace08.AccessibleName = "Record place no. 8";
		labelPlace08.AccessibleRole = AccessibleRole.StaticText;
		labelPlace08.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace08.Dock = DockStyle.Fill;
		labelPlace08.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace08.Location = new Point(3, 248);
		labelPlace08.Name = "labelPlace08";
		labelPlace08.Size = new Size(227, 23);
		labelPlace08.TabIndex = 32;
		labelPlace08.ToolTipValues.Description = "Shows the record place no. 8.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace08.ToolTipValues.EnableToolTips = true;
		labelPlace08.ToolTipValues.Heading = "Record place no. 8";
		labelPlace08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace08.Values.Text = "8";
		labelPlace08.Enter += Control_Enter;
		labelPlace08.Leave += Control_Leave;
		labelPlace08.MouseDown += Control_MouseDown;
		labelPlace08.MouseEnter += Control_Enter;
		labelPlace08.MouseLeave += Control_Leave;
		// 
		// labelPlace09
		// 
		labelPlace09.AccessibleDescription = "Shows the record place no. 9";
		labelPlace09.AccessibleName = "Record place no. 9";
		labelPlace09.AccessibleRole = AccessibleRole.StaticText;
		labelPlace09.ContextMenuStrip = contextMenuCopyToClipboard;
		labelPlace09.Dock = DockStyle.Fill;
		labelPlace09.LabelStyle = LabelStyle.ItalicPanel;
		labelPlace09.Location = new Point(3, 277);
		labelPlace09.Name = "labelPlace09";
		labelPlace09.Size = new Size(227, 23);
		labelPlace09.TabIndex = 36;
		labelPlace09.ToolTipValues.Description = "Shows the record place no. 9.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelPlace09.ToolTipValues.EnableToolTips = true;
		labelPlace09.ToolTipValues.Heading = "Record place no. 9";
		labelPlace09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelPlace09.Values.Text = "9";
		labelPlace09.Enter += Control_Enter;
		labelPlace09.Leave += Control_Leave;
		labelPlace09.MouseDown += Control_MouseDown;
		labelPlace09.MouseEnter += Control_Enter;
		labelPlace09.MouseLeave += Control_Leave;
		// 
		// buttonGoto01
		// 
		buttonGoto01.AccessibleDescription = "Goes to the element of the place no. 1";
		buttonGoto01.AccessibleName = "Go to the element of the place no. 1";
		buttonGoto01.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto01.ButtonStyle = ButtonStyle.Form;
		buttonGoto01.Dock = DockStyle.Fill;
		buttonGoto01.Location = new Point(615, 45);
		buttonGoto01.Name = "buttonGoto01";
		buttonGoto01.Size = new Size(140, 23);
		buttonGoto01.TabIndex = 7;
		buttonGoto01.ToolTipValues.Description = "Goes to the element of the place no. 1.";
		buttonGoto01.ToolTipValues.EnableToolTips = true;
		buttonGoto01.ToolTipValues.Heading = "Go to the element of the place no. 1";
		buttonGoto01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto01.Values.DropDownArrowColor = Color.Empty;
		buttonGoto01.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto01.Values.Text = "Goto";
		buttonGoto01.Click += Goto01_Click;
		buttonGoto01.Enter += Control_Enter;
		buttonGoto01.Leave += Control_Leave;
		buttonGoto01.MouseEnter += Control_Enter;
		buttonGoto01.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignationHeader
		// 
		labelReadableDesignationHeader.AccessibleDescription = "Shows the readable designation header";
		labelReadableDesignationHeader.AccessibleName = "Readable designation header";
		labelReadableDesignationHeader.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignationHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignationHeader.Dock = DockStyle.Fill;
		labelReadableDesignationHeader.LabelStyle = LabelStyle.BoldPanel;
		labelReadableDesignationHeader.Location = new Point(236, 3);
		labelReadableDesignationHeader.Name = "labelReadableDesignationHeader";
		labelReadableDesignationHeader.Size = new Size(140, 36);
		labelReadableDesignationHeader.TabIndex = 1;
		labelReadableDesignationHeader.ToolTipValues.Description = "Shows the readable designation header.";
		labelReadableDesignationHeader.ToolTipValues.EnableToolTips = true;
		labelReadableDesignationHeader.ToolTipValues.Heading = "Readable designation header";
		labelReadableDesignationHeader.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignationHeader.Values.Text = "Readable \r\ndesignation";
		labelReadableDesignationHeader.Enter += Control_Enter;
		labelReadableDesignationHeader.Leave += Control_Leave;
		labelReadableDesignationHeader.MouseDown += Control_MouseDown;
		labelReadableDesignationHeader.MouseEnter += Control_Enter;
		labelReadableDesignationHeader.MouseLeave += Control_Leave;
		// 
		// labelValueHeader
		// 
		labelValueHeader.AccessibleDescription = "Shows the value header";
		labelValueHeader.AccessibleName = "Value header";
		labelValueHeader.AccessibleRole = AccessibleRole.StaticText;
		labelValueHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueHeader.Dock = DockStyle.Fill;
		labelValueHeader.LabelStyle = LabelStyle.BoldPanel;
		labelValueHeader.Location = new Point(382, 3);
		labelValueHeader.Name = "labelValueHeader";
		labelValueHeader.Size = new Size(227, 36);
		labelValueHeader.TabIndex = 2;
		labelValueHeader.ToolTipValues.Description = "Shows the value header.";
		labelValueHeader.ToolTipValues.EnableToolTips = true;
		labelValueHeader.ToolTipValues.Heading = "Value header";
		labelValueHeader.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueHeader.Values.Text = "Value";
		labelValueHeader.Enter += Control_Enter;
		labelValueHeader.Leave += Control_Leave;
		labelValueHeader.MouseDown += Control_MouseDown;
		labelValueHeader.MouseEnter += Control_Enter;
		labelValueHeader.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation01
		// 
		labelReadableDesignation01.AccessibleDescription = "Shows the readable designation no. 1";
		labelReadableDesignation01.AccessibleName = "Readable designation no. 1";
		labelReadableDesignation01.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation01.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation01.Dock = DockStyle.Fill;
		labelReadableDesignation01.Location = new Point(236, 45);
		labelReadableDesignation01.Name = "labelReadableDesignation01";
		labelReadableDesignation01.Size = new Size(140, 23);
		labelReadableDesignation01.TabIndex = 5;
		labelReadableDesignation01.ToolTipValues.Description = "Shows the readable designation of place no. 1.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation01.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation01.ToolTipValues.Heading = "Readable designation no. 1";
		labelReadableDesignation01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation01.Values.Text = "-";
		labelReadableDesignation01.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation01.Enter += Control_Enter;
		labelReadableDesignation01.Leave += Control_Leave;
		labelReadableDesignation01.MouseDown += Control_MouseDown;
		labelReadableDesignation01.MouseEnter += Control_Enter;
		labelReadableDesignation01.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation02
		// 
		labelReadableDesignation02.AccessibleDescription = "Shows the readable designation no. 2";
		labelReadableDesignation02.AccessibleName = "Readable designation no. 2";
		labelReadableDesignation02.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation02.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation02.Dock = DockStyle.Fill;
		labelReadableDesignation02.Location = new Point(236, 74);
		labelReadableDesignation02.Name = "labelReadableDesignation02";
		labelReadableDesignation02.Size = new Size(140, 23);
		labelReadableDesignation02.TabIndex = 9;
		labelReadableDesignation02.ToolTipValues.Description = "Shows the readable designation of place no. 2.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation02.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation02.ToolTipValues.Heading = "Readable designation no. 2";
		labelReadableDesignation02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation02.Values.Text = "-";
		labelReadableDesignation02.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation02.Enter += Control_Enter;
		labelReadableDesignation02.Leave += Control_Leave;
		labelReadableDesignation02.MouseDown += Control_MouseDown;
		labelReadableDesignation02.MouseEnter += Control_Enter;
		labelReadableDesignation02.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation03
		// 
		labelReadableDesignation03.AccessibleDescription = "Shows the readable designation no. 3";
		labelReadableDesignation03.AccessibleName = "Readable designation no. 3";
		labelReadableDesignation03.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation03.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation03.Dock = DockStyle.Fill;
		labelReadableDesignation03.Location = new Point(236, 103);
		labelReadableDesignation03.Name = "labelReadableDesignation03";
		labelReadableDesignation03.Size = new Size(140, 23);
		labelReadableDesignation03.TabIndex = 13;
		labelReadableDesignation03.ToolTipValues.Description = "Shows the readable designation of place no. 3.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation03.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation03.ToolTipValues.Heading = "Readable designation no. 3";
		labelReadableDesignation03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation03.Values.Text = "-";
		labelReadableDesignation03.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation03.Enter += Control_Enter;
		labelReadableDesignation03.Leave += Control_Leave;
		labelReadableDesignation03.MouseDown += Control_MouseDown;
		labelReadableDesignation03.MouseEnter += Control_Enter;
		labelReadableDesignation03.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation04
		// 
		labelReadableDesignation04.AccessibleDescription = "Shows the readable designation no. 4";
		labelReadableDesignation04.AccessibleName = "Readable designation no. 4";
		labelReadableDesignation04.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation04.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation04.Dock = DockStyle.Fill;
		labelReadableDesignation04.Location = new Point(236, 132);
		labelReadableDesignation04.Name = "labelReadableDesignation04";
		labelReadableDesignation04.Size = new Size(140, 23);
		labelReadableDesignation04.TabIndex = 17;
		labelReadableDesignation04.ToolTipValues.Description = "Shows the readable designation of place no. 4.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation04.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation04.ToolTipValues.Heading = "Readable designation no. 4";
		labelReadableDesignation04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation04.Values.Text = "-";
		labelReadableDesignation04.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation04.Enter += Control_Enter;
		labelReadableDesignation04.Leave += Control_Leave;
		labelReadableDesignation04.MouseDown += Control_MouseDown;
		labelReadableDesignation04.MouseEnter += Control_Enter;
		labelReadableDesignation04.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation05
		// 
		labelReadableDesignation05.AccessibleDescription = "Shows the readable designation no. 5";
		labelReadableDesignation05.AccessibleName = "Readable designation no. 5";
		labelReadableDesignation05.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation05.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation05.Dock = DockStyle.Fill;
		labelReadableDesignation05.Location = new Point(236, 161);
		labelReadableDesignation05.Name = "labelReadableDesignation05";
		labelReadableDesignation05.Size = new Size(140, 23);
		labelReadableDesignation05.TabIndex = 21;
		labelReadableDesignation05.ToolTipValues.Description = "Shows the readable designation of place no. 5.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation05.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation05.ToolTipValues.Heading = "Readable designation no. 5";
		labelReadableDesignation05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation05.Values.Text = "-";
		labelReadableDesignation05.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation05.Enter += Control_Enter;
		labelReadableDesignation05.Leave += Control_Leave;
		labelReadableDesignation05.MouseDown += Control_MouseDown;
		labelReadableDesignation05.MouseEnter += Control_Enter;
		labelReadableDesignation05.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation06
		// 
		labelReadableDesignation06.AccessibleDescription = "Shows the readable designation no. 6";
		labelReadableDesignation06.AccessibleName = "Readable designation no. 6";
		labelReadableDesignation06.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation06.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation06.Dock = DockStyle.Fill;
		labelReadableDesignation06.Location = new Point(236, 190);
		labelReadableDesignation06.Name = "labelReadableDesignation06";
		labelReadableDesignation06.Size = new Size(140, 23);
		labelReadableDesignation06.TabIndex = 25;
		labelReadableDesignation06.ToolTipValues.Description = "Shows the readable designation of place no. 6.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation06.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation06.ToolTipValues.Heading = "Readable designation no. 6";
		labelReadableDesignation06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation06.Values.Text = "-";
		labelReadableDesignation06.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation06.Enter += Control_Enter;
		labelReadableDesignation06.Leave += Control_Leave;
		labelReadableDesignation06.MouseDown += Control_MouseDown;
		labelReadableDesignation06.MouseEnter += Control_Enter;
		labelReadableDesignation06.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation07
		// 
		labelReadableDesignation07.AccessibleDescription = "Shows the readable designation no. 7";
		labelReadableDesignation07.AccessibleName = "Readable designation no. 7";
		labelReadableDesignation07.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation07.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation07.Dock = DockStyle.Fill;
		labelReadableDesignation07.Location = new Point(236, 219);
		labelReadableDesignation07.Name = "labelReadableDesignation07";
		labelReadableDesignation07.Size = new Size(140, 23);
		labelReadableDesignation07.TabIndex = 29;
		labelReadableDesignation07.ToolTipValues.Description = "Shows the readable designation of place no. 7.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation07.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation07.ToolTipValues.Heading = "Readable designation no. 7";
		labelReadableDesignation07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation07.Values.Text = "-";
		labelReadableDesignation07.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation07.Enter += Control_Enter;
		labelReadableDesignation07.Leave += Control_Leave;
		labelReadableDesignation07.MouseDown += Control_MouseDown;
		labelReadableDesignation07.MouseEnter += Control_Enter;
		labelReadableDesignation07.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation08
		// 
		labelReadableDesignation08.AccessibleDescription = "Shows the readable designation no. 8";
		labelReadableDesignation08.AccessibleName = "Readable designation no. 8";
		labelReadableDesignation08.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation08.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation08.Dock = DockStyle.Fill;
		labelReadableDesignation08.Location = new Point(236, 248);
		labelReadableDesignation08.Name = "labelReadableDesignation08";
		labelReadableDesignation08.Size = new Size(140, 23);
		labelReadableDesignation08.TabIndex = 33;
		labelReadableDesignation08.ToolTipValues.Description = "Shows the readable designation of place no. 8.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation08.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation08.ToolTipValues.Heading = "Readable designation no. 8";
		labelReadableDesignation08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation08.Values.Text = "-";
		labelReadableDesignation08.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation08.Enter += Control_Enter;
		labelReadableDesignation08.Leave += Control_Leave;
		labelReadableDesignation08.MouseDown += Control_MouseDown;
		labelReadableDesignation08.MouseEnter += Control_Enter;
		labelReadableDesignation08.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation09
		// 
		labelReadableDesignation09.AccessibleDescription = "Shows the readable designation no. 9";
		labelReadableDesignation09.AccessibleName = "Readable designation no. 9";
		labelReadableDesignation09.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation09.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation09.Dock = DockStyle.Fill;
		labelReadableDesignation09.Location = new Point(236, 277);
		labelReadableDesignation09.Name = "labelReadableDesignation09";
		labelReadableDesignation09.Size = new Size(140, 23);
		labelReadableDesignation09.TabIndex = 37;
		labelReadableDesignation09.ToolTipValues.Description = "Shows the readable designation of place no. 9.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation09.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation09.ToolTipValues.Heading = "Readable designation no. 9";
		labelReadableDesignation09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation09.Values.Text = "-";
		labelReadableDesignation09.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation09.Enter += Control_Enter;
		labelReadableDesignation09.Leave += Control_Leave;
		labelReadableDesignation09.MouseDown += Control_MouseDown;
		labelReadableDesignation09.MouseEnter += Control_Enter;
		labelReadableDesignation09.MouseLeave += Control_Leave;
		// 
		// labelReadableDesignation10
		// 
		labelReadableDesignation10.AccessibleDescription = "Shows the readable designation no. 10";
		labelReadableDesignation10.AccessibleName = "Readable designation no. 10";
		labelReadableDesignation10.AccessibleRole = AccessibleRole.StaticText;
		labelReadableDesignation10.ContextMenuStrip = contextMenuCopyToClipboard;
		labelReadableDesignation10.Dock = DockStyle.Fill;
		labelReadableDesignation10.Location = new Point(236, 306);
		labelReadableDesignation10.Name = "labelReadableDesignation10";
		labelReadableDesignation10.Size = new Size(140, 42);
		labelReadableDesignation10.TabIndex = 41;
		labelReadableDesignation10.ToolTipValues.Description = "Shows the readable designation of place no. 10.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelReadableDesignation10.ToolTipValues.EnableToolTips = true;
		labelReadableDesignation10.ToolTipValues.Heading = "Readable designation no. 10";
		labelReadableDesignation10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelReadableDesignation10.Values.Text = "-";
		labelReadableDesignation10.DoubleClick += CopyToClipboard_DoubleClick;
		labelReadableDesignation10.Enter += Control_Enter;
		labelReadableDesignation10.Leave += Control_Leave;
		labelReadableDesignation10.MouseDown += Control_MouseDown;
		labelReadableDesignation10.MouseEnter += Control_Enter;
		labelReadableDesignation10.MouseLeave += Control_Leave;
		// 
		// labelValue01
		// 
		labelValue01.AccessibleDescription = "Shows the value no. 1";
		labelValue01.AccessibleName = "Value no. 1";
		labelValue01.AccessibleRole = AccessibleRole.StaticText;
		labelValue01.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue01.Dock = DockStyle.Fill;
		labelValue01.Location = new Point(382, 45);
		labelValue01.Name = "labelValue01";
		labelValue01.Size = new Size(227, 23);
		labelValue01.TabIndex = 6;
		labelValue01.ToolTipValues.Description = "Shows the value of place  no. 1.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue01.ToolTipValues.EnableToolTips = true;
		labelValue01.ToolTipValues.Heading = "Value no. 1";
		labelValue01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue01.Values.Text = "-";
		labelValue01.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue01.Enter += Control_Enter;
		labelValue01.Leave += Control_Leave;
		labelValue01.MouseDown += Control_MouseDown;
		labelValue01.MouseEnter += Control_Enter;
		labelValue01.MouseLeave += Control_Leave;
		// 
		// labelValue02
		// 
		labelValue02.AccessibleDescription = "Shows the value no. 2";
		labelValue02.AccessibleName = "Value no. 2";
		labelValue02.AccessibleRole = AccessibleRole.StaticText;
		labelValue02.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue02.Dock = DockStyle.Fill;
		labelValue02.Location = new Point(382, 74);
		labelValue02.Name = "labelValue02";
		labelValue02.Size = new Size(227, 23);
		labelValue02.TabIndex = 10;
		labelValue02.ToolTipValues.Description = "Shows the value of place no. 2.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue02.ToolTipValues.EnableToolTips = true;
		labelValue02.ToolTipValues.Heading = "Value no. 2";
		labelValue02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue02.Values.Text = "-";
		labelValue02.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue02.Enter += Control_Enter;
		labelValue02.Leave += Control_Leave;
		labelValue02.MouseDown += Control_MouseDown;
		labelValue02.MouseEnter += Control_Enter;
		labelValue02.MouseLeave += Control_Leave;
		// 
		// labelValue03
		// 
		labelValue03.AccessibleDescription = "Shows the value no. 3";
		labelValue03.AccessibleName = "Value no. 3";
		labelValue03.AccessibleRole = AccessibleRole.StaticText;
		labelValue03.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue03.Dock = DockStyle.Fill;
		labelValue03.Location = new Point(382, 103);
		labelValue03.Name = "labelValue03";
		labelValue03.Size = new Size(227, 23);
		labelValue03.TabIndex = 14;
		labelValue03.ToolTipValues.Description = "Shows the value of place no. 3.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue03.ToolTipValues.EnableToolTips = true;
		labelValue03.ToolTipValues.Heading = "Value no. 3";
		labelValue03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue03.Values.Text = "-";
		labelValue03.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue03.Enter += Control_Enter;
		labelValue03.Leave += Control_Leave;
		labelValue03.MouseDown += Control_MouseDown;
		labelValue03.MouseEnter += Control_Enter;
		labelValue03.MouseLeave += Control_Leave;
		// 
		// labelValue04
		// 
		labelValue04.AccessibleDescription = "Shows the value no. 4";
		labelValue04.AccessibleName = "Value no. 4";
		labelValue04.AccessibleRole = AccessibleRole.StaticText;
		labelValue04.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue04.Dock = DockStyle.Fill;
		labelValue04.Location = new Point(382, 132);
		labelValue04.Name = "labelValue04";
		labelValue04.Size = new Size(227, 23);
		labelValue04.TabIndex = 18;
		labelValue04.ToolTipValues.Description = "Shows the value of place no. 4.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue04.ToolTipValues.EnableToolTips = true;
		labelValue04.ToolTipValues.Heading = "Value no. 4";
		labelValue04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue04.Values.Text = "-";
		labelValue04.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue04.Enter += Control_Enter;
		labelValue04.Leave += Control_Leave;
		labelValue04.MouseDown += Control_MouseDown;
		labelValue04.MouseEnter += Control_Enter;
		labelValue04.MouseLeave += Control_Leave;
		// 
		// labelValue05
		// 
		labelValue05.AccessibleDescription = "Shows the value no. 5";
		labelValue05.AccessibleName = "Value no. 5";
		labelValue05.AccessibleRole = AccessibleRole.StaticText;
		labelValue05.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue05.Dock = DockStyle.Fill;
		labelValue05.Location = new Point(382, 161);
		labelValue05.Name = "labelValue05";
		labelValue05.Size = new Size(227, 23);
		labelValue05.TabIndex = 22;
		labelValue05.ToolTipValues.Description = "Shows the value of place no. 5.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue05.ToolTipValues.EnableToolTips = true;
		labelValue05.ToolTipValues.Heading = "Value no. 5";
		labelValue05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue05.Values.Text = "-";
		labelValue05.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue05.Enter += Control_Enter;
		labelValue05.Leave += Control_Leave;
		labelValue05.MouseDown += Control_MouseDown;
		labelValue05.MouseEnter += Control_Enter;
		labelValue05.MouseLeave += Control_Leave;
		// 
		// labelValue06
		// 
		labelValue06.AccessibleDescription = "Shows the value no. 6";
		labelValue06.AccessibleName = "Value no. 6";
		labelValue06.AccessibleRole = AccessibleRole.StaticText;
		labelValue06.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue06.Dock = DockStyle.Fill;
		labelValue06.Location = new Point(382, 190);
		labelValue06.Name = "labelValue06";
		labelValue06.Size = new Size(227, 23);
		labelValue06.TabIndex = 26;
		labelValue06.ToolTipValues.Description = "Shows the value of place no. 6.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue06.ToolTipValues.EnableToolTips = true;
		labelValue06.ToolTipValues.Heading = "Value no. 6";
		labelValue06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue06.Values.Text = "-";
		labelValue06.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue06.Enter += Control_Enter;
		labelValue06.Leave += Control_Leave;
		labelValue06.MouseDown += Control_MouseDown;
		labelValue06.MouseEnter += Control_Enter;
		labelValue06.MouseLeave += Control_Leave;
		// 
		// labelValue07
		// 
		labelValue07.AccessibleDescription = "Shows the value no. 7";
		labelValue07.AccessibleName = "Value no. 7";
		labelValue07.AccessibleRole = AccessibleRole.StaticText;
		labelValue07.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue07.Dock = DockStyle.Fill;
		labelValue07.Location = new Point(382, 219);
		labelValue07.Name = "labelValue07";
		labelValue07.Size = new Size(227, 23);
		labelValue07.TabIndex = 30;
		labelValue07.ToolTipValues.Description = "Shows the value of place no. 7.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue07.ToolTipValues.EnableToolTips = true;
		labelValue07.ToolTipValues.Heading = "Value no. 7";
		labelValue07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue07.Values.Text = "-";
		labelValue07.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue07.Enter += Control_Enter;
		labelValue07.Leave += Control_Leave;
		labelValue07.MouseDown += Control_MouseDown;
		labelValue07.MouseEnter += Control_Enter;
		labelValue07.MouseLeave += Control_Leave;
		// 
		// labelValue08
		// 
		labelValue08.AccessibleDescription = "Shows the value no. 8";
		labelValue08.AccessibleName = "Value no. 8";
		labelValue08.AccessibleRole = AccessibleRole.StaticText;
		labelValue08.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue08.Dock = DockStyle.Fill;
		labelValue08.Location = new Point(382, 248);
		labelValue08.Name = "labelValue08";
		labelValue08.Size = new Size(227, 23);
		labelValue08.TabIndex = 34;
		labelValue08.ToolTipValues.Description = "Shows the value of place no. 8.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue08.ToolTipValues.EnableToolTips = true;
		labelValue08.ToolTipValues.Heading = "Value no. 8";
		labelValue08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue08.Values.Text = "-";
		labelValue08.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue08.Enter += Control_Enter;
		labelValue08.Leave += Control_Leave;
		labelValue08.MouseDown += Control_MouseDown;
		labelValue08.MouseEnter += Control_Enter;
		labelValue08.MouseLeave += Control_Leave;
		// 
		// labelValue09
		// 
		labelValue09.AccessibleDescription = "Shows the value no. 9";
		labelValue09.AccessibleName = "Value no. 9";
		labelValue09.AccessibleRole = AccessibleRole.StaticText;
		labelValue09.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue09.Dock = DockStyle.Fill;
		labelValue09.Location = new Point(382, 277);
		labelValue09.Name = "labelValue09";
		labelValue09.Size = new Size(227, 23);
		labelValue09.TabIndex = 38;
		labelValue09.ToolTipValues.Description = "Shows the value of place no. 9.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue09.ToolTipValues.EnableToolTips = true;
		labelValue09.ToolTipValues.Heading = "Value no. 9";
		labelValue09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue09.Values.Text = "-";
		labelValue09.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue09.Enter += Control_Enter;
		labelValue09.Leave += Control_Leave;
		labelValue09.MouseDown += Control_MouseDown;
		labelValue09.MouseEnter += Control_Enter;
		labelValue09.MouseLeave += Control_Leave;
		// 
		// labelValue10
		// 
		labelValue10.AccessibleDescription = "Shows the value no. 10";
		labelValue10.AccessibleName = "Value no. 10";
		labelValue10.AccessibleRole = AccessibleRole.StaticText;
		labelValue10.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValue10.Dock = DockStyle.Fill;
		labelValue10.Location = new Point(382, 306);
		labelValue10.Name = "labelValue10";
		labelValue10.Size = new Size(227, 42);
		labelValue10.TabIndex = 42;
		labelValue10.ToolTipValues.Description = "Shows the value of place no. 10.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValue10.ToolTipValues.EnableToolTips = true;
		labelValue10.ToolTipValues.Heading = "Value no. 10";
		labelValue10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValue10.Values.Text = "-";
		labelValue10.DoubleClick += CopyToClipboard_DoubleClick;
		labelValue10.Enter += Control_Enter;
		labelValue10.Leave += Control_Leave;
		labelValue10.MouseDown += Control_MouseDown;
		labelValue10.MouseEnter += Control_Enter;
		labelValue10.MouseLeave += Control_Leave;
		// 
		// buttonGoto10
		// 
		buttonGoto10.AccessibleDescription = "Goes to the element of the place no. 10";
		buttonGoto10.AccessibleName = "Go to the element of the place no. 10";
		buttonGoto10.AccessibleRole = AccessibleRole.PushButton;
		buttonGoto10.ButtonStyle = ButtonStyle.Form;
		buttonGoto10.Dock = DockStyle.Fill;
		buttonGoto10.Location = new Point(615, 306);
		buttonGoto10.Name = "buttonGoto10";
		buttonGoto10.Size = new Size(140, 42);
		buttonGoto10.TabIndex = 43;
		buttonGoto10.ToolTipValues.Description = "Goes to the element of the place no. 10.";
		buttonGoto10.ToolTipValues.EnableToolTips = true;
		buttonGoto10.ToolTipValues.Heading = "Go to the element of the place no. 10";
		buttonGoto10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonGoto10.Values.DropDownArrowColor = Color.Empty;
		buttonGoto10.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
		buttonGoto10.Values.Text = "Goto";
		buttonGoto10.Click += Goto10_Click;
		buttonGoto10.Enter += Control_Enter;
		buttonGoto10.Leave += Control_Leave;
		buttonGoto10.MouseEnter += Control_Enter;
		buttonGoto10.MouseLeave += Control_Leave;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar with some information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 0);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.Padding = new Padding(1, 0, 16, 0);
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(1054, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 0;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		kryptonStatusStrip.Enter += Control_Enter;
		kryptonStatusStrip.Leave += Control_Leave;
		kryptonStatusStrip.MouseEnter += Control_Enter;
		kryptonStatusStrip.MouseLeave += Control_Leave;
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
		labelInformation.ToolTipText = "Show some information";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
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
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(1054, 341);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(1054, 413);
		toolStripContainer.TabIndex = 4;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripGenerateList);
		toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripProgress);
		toolStripContainer.Enter += Control_Enter;
		toolStripContainer.Leave += Control_Leave;
		toolStripContainer.MouseEnter += Control_Enter;
		toolStripContainer.MouseLeave += Control_Leave;
		// 
		// kryptonToolStripGenerateList
		// 
		kryptonToolStripGenerateList.AccessibleDescription = "Toolbar of generating list";
		kryptonToolStripGenerateList.AccessibleName = "Toolbar of generating list";
		kryptonToolStripGenerateList.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStripGenerateList.AllowClickThrough = true;
		kryptonToolStripGenerateList.AllowItemReorder = true;
		kryptonToolStripGenerateList.Dock = DockStyle.None;
		kryptonToolStripGenerateList.Font = new Font("Segoe UI", 9F);
		kryptonToolStripGenerateList.Items.AddRange(new ToolStripItem[] { toolStripButtonStart, toolStripButtonCancel, toolStripSeparator2, toolStripButtonSortOrderAscending, toolStripButtonSortOrderDescending, toolStripSeparator3, toolStripDropDownButtonSaveList });
		kryptonToolStripGenerateList.Location = new Point(0, 0);
		kryptonToolStripGenerateList.Name = "kryptonToolStripGenerateList";
		kryptonToolStripGenerateList.Size = new Size(1054, 25);
		kryptonToolStripGenerateList.Stretch = true;
		kryptonToolStripGenerateList.TabIndex = 0;
		kryptonToolStripGenerateList.TabStop = true;
		kryptonToolStripGenerateList.Text = "Toolbar of generating list";
		kryptonToolStripGenerateList.Enter += Control_Enter;
		kryptonToolStripGenerateList.Leave += Control_Leave;
		kryptonToolStripGenerateList.MouseEnter += Control_Enter;
		kryptonToolStripGenerateList.MouseLeave += Control_Leave;
		// 
		// toolStripButtonStart
		// 
		toolStripButtonStart.AccessibleDescription = "Detects the records";
		toolStripButtonStart.AccessibleName = "Detect the records";
		toolStripButtonStart.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonStart.Image = FatcowIcons16px.fatcow_resultset_next_16px;
		toolStripButtonStart.ImageTransparentColor = Color.Magenta;
		toolStripButtonStart.Name = "toolStripButtonStart";
		toolStripButtonStart.Size = new Size(51, 22);
		toolStripButtonStart.Text = "&Start";
		toolStripButtonStart.Click += Start_Click;
		toolStripButtonStart.MouseEnter += Control_Enter;
		toolStripButtonStart.MouseLeave += Control_Leave;
		// 
		// toolStripButtonCancel
		// 
		toolStripButtonCancel.AccessibleDescription = "Cancels the record detection scan";
		toolStripButtonCancel.AccessibleName = "Cancel scan";
		toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
		toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
		toolStripButtonCancel.Name = "toolStripButtonCancel";
		toolStripButtonCancel.Size = new Size(63, 22);
		toolStripButtonCancel.Text = "&Cancel";
		toolStripButtonCancel.Click += Cancel_Click;
		toolStripButtonCancel.MouseEnter += Control_Enter;
		toolStripButtonCancel.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator2
		// 
		toolStripSeparator2.AccessibleDescription = "Just a separator";
		toolStripSeparator2.AccessibleName = "Just a separator";
		toolStripSeparator2.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(6, 25);
		toolStripSeparator2.MouseEnter += Control_Enter;
		toolStripSeparator2.MouseLeave += Control_Leave;
		// 
		// toolStripButtonSortOrderAscending
		// 
		toolStripButtonSortOrderAscending.AccessibleDescription = "Selects the ascending sort order";
		toolStripButtonSortOrderAscending.AccessibleName = "Ascending sort order";
		toolStripButtonSortOrderAscending.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonSortOrderAscending.Checked = true;
		toolStripButtonSortOrderAscending.CheckOnClick = true;
		toolStripButtonSortOrderAscending.CheckState = CheckState.Checked;
		toolStripButtonSortOrderAscending.Image = FatcowIcons16px.fatcow_sort_ascending_16px;
		toolStripButtonSortOrderAscending.ImageTransparentColor = Color.Magenta;
		toolStripButtonSortOrderAscending.Name = "toolStripButtonSortOrderAscending";
		toolStripButtonSortOrderAscending.Size = new Size(83, 22);
		toolStripButtonSortOrderAscending.Text = "&Ascending";
		toolStripButtonSortOrderAscending.MouseEnter += Control_Enter;
		toolStripButtonSortOrderAscending.MouseLeave += Control_Leave;
		// 
		// toolStripButtonSortOrderDescending
		// 
		toolStripButtonSortOrderDescending.AccessibleDescription = "Selects the descending sort order";
		toolStripButtonSortOrderDescending.AccessibleName = "Descending sort order";
		toolStripButtonSortOrderDescending.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonSortOrderDescending.CheckOnClick = true;
		toolStripButtonSortOrderDescending.Image = FatcowIcons16px.fatcow_sort_descending_16px;
		toolStripButtonSortOrderDescending.ImageTransparentColor = Color.Magenta;
		toolStripButtonSortOrderDescending.Name = "toolStripButtonSortOrderDescending";
		toolStripButtonSortOrderDescending.Size = new Size(89, 22);
		toolStripButtonSortOrderDescending.Text = "&Descending";
		toolStripButtonSortOrderDescending.MouseEnter += Control_Enter;
		toolStripButtonSortOrderDescending.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator3
		// 
		toolStripSeparator3.AccessibleDescription = "Just a separator";
		toolStripSeparator3.AccessibleName = "Just a separator";
		toolStripSeparator3.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator3.Name = "toolStripSeparator3";
		toolStripSeparator3.Size = new Size(6, 25);
		toolStripSeparator3.MouseEnter += Control_Enter;
		toolStripSeparator3.MouseLeave += Control_Leave;
		// 
		// kryptonToolStripProgress
		// 
		kryptonToolStripProgress.AccessibleDescription = "Toolbar of progress";
		kryptonToolStripProgress.AccessibleName = "Toolbar of progress";
		kryptonToolStripProgress.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStripProgress.AllowClickThrough = true;
		kryptonToolStripProgress.AllowItemReorder = true;
		kryptonToolStripProgress.Dock = DockStyle.None;
		kryptonToolStripProgress.Font = new Font("Segoe UI", 9F);
		kryptonToolStripProgress.Items.AddRange(new ToolStripItem[] { toolStripLabelProgress, kryptonProgressBar });
		kryptonToolStripProgress.Location = new Point(0, 25);
		kryptonToolStripProgress.Name = "kryptonToolStripProgress";
		kryptonToolStripProgress.Size = new Size(1054, 25);
		kryptonToolStripProgress.Stretch = true;
		kryptonToolStripProgress.TabIndex = 1;
		kryptonToolStripProgress.TabStop = true;
		kryptonToolStripProgress.Text = "Toolbar of progress";
		kryptonToolStripProgress.Enter += Control_Enter;
		kryptonToolStripProgress.Leave += Control_Leave;
		kryptonToolStripProgress.MouseEnter += Control_Enter;
		kryptonToolStripProgress.MouseLeave += Control_Leave;
		// 
		// toolStripLabelProgress
		// 
		toolStripLabelProgress.AccessibleDescription = "Shows the progress of of the record detection";
		toolStripLabelProgress.AccessibleName = "Loading progress";
		toolStripLabelProgress.AccessibleRole = AccessibleRole.StaticText;
		toolStripLabelProgress.AutoToolTip = true;
		toolStripLabelProgress.Name = "toolStripLabelProgress";
		toolStripLabelProgress.Size = new Size(52, 22);
		toolStripLabelProgress.Text = "Pro&gress";
		toolStripLabelProgress.MouseEnter += Control_Enter;
		toolStripLabelProgress.MouseLeave += Control_Leave;
		// 
		// kryptonProgressBar
		// 
		kryptonProgressBar.AccessibleDescription = "Shows the progress status of the record detection";
		kryptonProgressBar.AccessibleName = "Loading progress";
		kryptonProgressBar.AccessibleRole = AccessibleRole.ProgressBar;
		kryptonProgressBar.AutoToolTip = true;
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(400, 22);
		kryptonProgressBar.StateCommon.Back.Color1 = Color.Green;
		kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.Values.Text = "";
		kryptonProgressBar.Enter += Control_Enter;
		kryptonProgressBar.Leave += Control_Leave;
		kryptonProgressBar.MouseEnter += Control_Enter;
		kryptonProgressBar.MouseLeave += Control_Leave;
		// 
		// RecordsTop10Form
		// 
		AccessibleDescription = "Shows the top ten records";
		AccessibleName = "Top ten records";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1054, 413);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "RecordsTop10Form";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Top ten records";
		Load += RecordsMainForm_Load;
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		splitContainerMain.Panel1.ResumeLayout(false);
		splitContainerMain.Panel2.ResumeLayout(false);
		((ISupportInitialize)splitContainerMain).EndInit();
		splitContainerMain.ResumeLayout(false);
		((ISupportInitialize)kryptonPanelSplitterContainerLeft).EndInit();
		kryptonPanelSplitterContainerLeft.ResumeLayout(false);
		((ISupportInitialize)kryptonPanelSplitterContainerRight).EndInit();
		kryptonPanelSplitterContainerRight.ResumeLayout(false);
		tableLayoutPanel.ResumeLayout(false);
		tableLayoutPanel.PerformLayout();
		contextMenuSaveToFile.ResumeLayout(false);
		contextMenuCopyToClipboard.ResumeLayout(false);
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonToolStripGenerateList.ResumeLayout(false);
		kryptonToolStripGenerateList.PerformLayout();
		kryptonToolStripProgress.ResumeLayout(false);
		kryptonToolStripProgress.PerformLayout();
		ResumeLayout(false);

	}

	#endregion
	private KryptonPanel kryptonPanelMain;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
	private ToolStripContainer toolStripContainer;
	private ContextMenuStrip contextMenuSaveToFile;
	private ToolStripMenuItem toolStripMenuItemTextFiles;
	private ToolStripMenuItem toolStripMenuItemSaveAsText;
	private ToolStripMenuItem toolStripMenuItemSaveAsLatex;
	private ToolStripMenuItem toolStripMenuItemSaveAsMarkdown;
	private ToolStripMenuItem toolStripMenuItemSaveAsAsciiDoc;
	private ToolStripMenuItem toolStripMenuItemSaveAsReStructuredText;
	private ToolStripMenuItem toolStripMenuItemSaveAsTextile;
	private ToolStripMenuItem toolStripMenuItemWriterDocuments;
	private ToolStripMenuItem toolStripMenuItemSaveAsWord;
	private ToolStripMenuItem toolStripMenuItemSaveAsOdt;
	private ToolStripMenuItem toolStripMenuItemSaveAsRtf;
	private ToolStripMenuItem toolStripMenuItemSaveAsAbiword;
	private ToolStripMenuItem toolStripMenuItemSaveAsWps;
	private ToolStripMenuItem toolStripMenuItemSpreadsheetDocuments;
	private ToolStripMenuItem toolStripMenuItemSaveAsExcel;
	private ToolStripMenuItem toolStripMenuItemSaveAsOds;
	private ToolStripMenuItem toolStripMenuItemSaveAsCsv;
	private ToolStripMenuItem toolStripMenuItemSaveAsTsv;
	private ToolStripMenuItem toolStripMenuItemSaveAsPsv;
	private ToolStripMenuItem toolStripMenuItemSaveAsEt;
	private ToolStripMenuItem toolStripMenuItemXmlDocuments;
	private ToolStripMenuItem toolStripMenuItemSaveAsHtml;
	private ToolStripMenuItem toolStripMenuItemSaveAsXml;
	private ToolStripMenuItem toolStripMenuItemSaveAsDocBook;
	private ToolStripMenuItem toolStripMenuItemConfigurationFiles;
	private ToolStripMenuItem toolStripMenuItemSaveAsJson;
	private ToolStripMenuItem toolStripMenuItemSaveAsToml;
	private ToolStripMenuItem toolStripMenuItemSaveAsYaml;
	private ToolStripMenuItem toolStripMenuItemDatabaseScripts;
	private ToolStripMenuItem toolStripMenuItemSaveAsSql;
	private ToolStripMenuItem toolStripMenuItemSaveAsSqlite;
	private ToolStripMenuItem toolStripMenuItemPortableDocuments;
	private ToolStripMenuItem toolStripMenuItemSaveAsPdf;
	private ToolStripMenuItem toolStripMenuItemSaveAsPostScript;
	private ToolStripMenuItem toolStripMenuItemSaveAsEpub;
	private ToolStripMenuItem toolStripMenuItemSaveAsMobi;
	private ToolStripMenuItem toolStripMenuItemSaveAsXps;
	private ToolStripMenuItem toolStripMenuItemSaveAsFictionBook2;
	private ToolStripMenuItem toolStripMenuItemSaveAsChm;
	private ContextMenuStrip contextMenuCopyToClipboard;
	private ToolStripMenuItem toolStripMenuItemCopyToClipboardInContextMenu;
	private KryptonToolStrip kryptonToolStripGenerateList;
	private ToolStripButton toolStripButtonStart;
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripButton toolStripButtonSortOrderAscending;
	private ToolStripButton toolStripButtonSortOrderDescending;
	private ToolStripSeparator toolStripSeparator3;
	private ToolStripDropDownButton toolStripDropDownButtonSaveList;
	private KryptonToolStrip kryptonToolStripProgress;
	private ToolStripLabel toolStripLabelProgress;
	private KryptonProgressBarToolStripItem kryptonProgressBar;
	private SplitContainer splitContainerMain;
	private KryptonPanel kryptonPanelSplitterContainerLeft;
	private KryptonPanel kryptonPanelSplitterContainerRight;
	private KryptonListBox listBox;
	private KryptonTableLayoutPanel tableLayoutPanel;
	private KryptonLabel labelGoToObjectHeader;
	private KryptonLabel labelPlaceHeader;
	private KryptonButton buttonGoto09;
	private KryptonButton buttonGoto08;
	private KryptonButton buttonGoto07;
	private KryptonButton buttonGoto06;
	private KryptonButton buttonGoto05;
	private KryptonButton buttonGoto04;
	private KryptonButton buttonGoto03;
	private KryptonButton buttonGoto02;
	private KryptonLabel labelPlace10;
	private KryptonLabel labelPlace05;
	private KryptonLabel labelPlace04;
	private KryptonLabel labelPlace03;
	private KryptonLabel labelPlace01;
	private KryptonLabel labelPlace02;
	private KryptonLabel labelPlace06;
	private KryptonLabel labelPlace07;
	private KryptonLabel labelPlace08;
	private KryptonLabel labelPlace09;
	private KryptonButton buttonGoto01;
	private KryptonLabel labelReadableDesignationHeader;
	private KryptonLabel labelValueHeader;
	private KryptonLabel labelReadableDesignation01;
	private KryptonLabel labelReadableDesignation02;
	private KryptonLabel labelReadableDesignation03;
	private KryptonLabel labelReadableDesignation04;
	private KryptonLabel labelReadableDesignation05;
	private KryptonLabel labelReadableDesignation06;
	private KryptonLabel labelReadableDesignation07;
	private KryptonLabel labelReadableDesignation08;
	private KryptonLabel labelReadableDesignation09;
	private KryptonLabel labelReadableDesignation10;
	private KryptonLabel labelValue01;
	private KryptonLabel labelValue02;
	private KryptonLabel labelValue03;
	private KryptonLabel labelValue04;
	private KryptonLabel labelValue05;
	private KryptonLabel labelValue06;
	private KryptonLabel labelValue07;
	private KryptonLabel labelValue08;
	private KryptonLabel labelValue09;
	private KryptonLabel labelValue10;
	private KryptonButton buttonGoto10;
}