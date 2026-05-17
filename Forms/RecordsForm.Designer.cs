// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a form that displays and manages orbital element records, allowing users to view record values and associated designations for various orbital elements.</summary>
/// <remarks>The RecordsForm provides an interactive interface for viewing record values across multiple orbital elements, including controls for starting and canceling record detection scans. Progress and status information are displayed to assist users during scanning operations. The form is intended to be used as a modal dialog and is not shown in the taskbar. Accessibility features are included to support assistive technologies.</remarks>
partial class RecordsForm
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsForm));
		kryptonPanelMain = new KryptonPanel();
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
		labelElementHeader = new KryptonLabel();
		contextMenuCopyToClipboard = new ContextMenuStrip(components);
		toolStripMenuItemCopyToClipboardInContextMenu = new ToolStripMenuItem();
		labelDesignationHeader = new KryptonLabel();
		labelValueHeader = new KryptonLabel();
		labelElementMeanAnomalyAtTheEpoch = new KryptonLabel();
		labelDesignationMeanAnomalyAtTheEpoch = new KryptonLabel();
		labelValueMeanAnomalyAtTheEpoch = new KryptonLabel();
		labelElementArgumentOfThePerihelion = new KryptonLabel();
		labelDesignationArgumentOfThePerihelion = new KryptonLabel();
		labelValueArgumentOfThePerihelion = new KryptonLabel();
		labelElementLongitudeOfTheAscendingNodeDesc = new KryptonLabel();
		labelDesignationLongitudeOfTheAscendingNode = new KryptonLabel();
		labelValueLongitudeOfTheAscendingNode = new KryptonLabel();
		labelElementInclinationToTheEcliptic = new KryptonLabel();
		labelDesignationInclinationToTheEcliptic = new KryptonLabel();
		labelValueInclinationToTheEcliptic = new KryptonLabel();
		labelElementOrbitalEccentricity = new KryptonLabel();
		labelDesignationOrbitalEccentricity = new KryptonLabel();
		labelValueOrbitalEccentricity = new KryptonLabel();
		labelElementMeanDailyMotion = new KryptonLabel();
		labelDesignationMeanDailyMotion = new KryptonLabel();
		labelValueMeanDailyMotion = new KryptonLabel();
		labelElementSemiMajorAxis = new KryptonLabel();
		labelDesignationSemiMajorAxis = new KryptonLabel();
		labelValueSemiMajorAxis = new KryptonLabel();
		labelElementAbsoluteMagnitude = new KryptonLabel();
		labelDesignationAbsoluteMagnitude = new KryptonLabel();
		labelValueAbsoluteMagnitude = new KryptonLabel();
		labelElementSlopeParameter = new KryptonLabel();
		labelDesignationSlopeParameter = new KryptonLabel();
		labelValueSlopeParameter = new KryptonLabel();
		labelElementNumberOfOppositions = new KryptonLabel();
		labelDesignationNumberOfOppositions = new KryptonLabel();
		labelValueNumberOfOppositions = new KryptonLabel();
		labelElementNumberOfObservations = new KryptonLabel();
		labelDesignationNumberOfObservations = new KryptonLabel();
		labelValueNumberOfObservations = new KryptonLabel();
		labelElementRmsResidual = new KryptonLabel();
		labelDesignationRmsResidual = new KryptonLabel();
		labelValueRmsResidual = new KryptonLabel();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);
		backgroundWorker = new BackgroundWorker();
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
		kryptonPanelMain.Controls.Add(tableLayoutPanel);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(542, 373);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "Main panel";
		kryptonPanelMain.Enter += Control_Enter;
		kryptonPanelMain.Leave += Control_Leave;
		kryptonPanelMain.MouseEnter += Control_Enter;
		kryptonPanelMain.MouseLeave += Control_Leave;
		// 
		// tableLayoutPanel
		// 
		tableLayoutPanel.AccessibleDescription = "Table showing record data per orbital element";
		tableLayoutPanel.AccessibleName = "Records table";
		tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
		tableLayoutPanel.ColumnCount = 3;
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		tableLayoutPanel.ContextMenuStrip = contextMenuSaveToFile;
		tableLayoutPanel.Controls.Add(labelElementHeader, 0, 0);
		tableLayoutPanel.Controls.Add(labelDesignationHeader, 1, 0);
		tableLayoutPanel.Controls.Add(labelValueHeader, 2, 0);
		tableLayoutPanel.Controls.Add(labelElementMeanAnomalyAtTheEpoch, 0, 1);
		tableLayoutPanel.Controls.Add(labelDesignationMeanAnomalyAtTheEpoch, 1, 1);
		tableLayoutPanel.Controls.Add(labelValueMeanAnomalyAtTheEpoch, 2, 1);
		tableLayoutPanel.Controls.Add(labelElementArgumentOfThePerihelion, 0, 2);
		tableLayoutPanel.Controls.Add(labelDesignationArgumentOfThePerihelion, 1, 2);
		tableLayoutPanel.Controls.Add(labelValueArgumentOfThePerihelion, 2, 2);
		tableLayoutPanel.Controls.Add(labelElementLongitudeOfTheAscendingNodeDesc, 0, 3);
		tableLayoutPanel.Controls.Add(labelDesignationLongitudeOfTheAscendingNode, 1, 3);
		tableLayoutPanel.Controls.Add(labelValueLongitudeOfTheAscendingNode, 2, 3);
		tableLayoutPanel.Controls.Add(labelElementInclinationToTheEcliptic, 0, 4);
		tableLayoutPanel.Controls.Add(labelDesignationInclinationToTheEcliptic, 1, 4);
		tableLayoutPanel.Controls.Add(labelValueInclinationToTheEcliptic, 2, 4);
		tableLayoutPanel.Controls.Add(labelElementOrbitalEccentricity, 0, 5);
		tableLayoutPanel.Controls.Add(labelDesignationOrbitalEccentricity, 1, 5);
		tableLayoutPanel.Controls.Add(labelValueOrbitalEccentricity, 2, 5);
		tableLayoutPanel.Controls.Add(labelElementMeanDailyMotion, 0, 6);
		tableLayoutPanel.Controls.Add(labelDesignationMeanDailyMotion, 1, 6);
		tableLayoutPanel.Controls.Add(labelValueMeanDailyMotion, 2, 6);
		tableLayoutPanel.Controls.Add(labelElementSemiMajorAxis, 0, 7);
		tableLayoutPanel.Controls.Add(labelDesignationSemiMajorAxis, 1, 7);
		tableLayoutPanel.Controls.Add(labelValueSemiMajorAxis, 2, 7);
		tableLayoutPanel.Controls.Add(labelElementAbsoluteMagnitude, 0, 8);
		tableLayoutPanel.Controls.Add(labelDesignationAbsoluteMagnitude, 1, 8);
		tableLayoutPanel.Controls.Add(labelValueAbsoluteMagnitude, 2, 8);
		tableLayoutPanel.Controls.Add(labelElementSlopeParameter, 0, 9);
		tableLayoutPanel.Controls.Add(labelDesignationSlopeParameter, 1, 9);
		tableLayoutPanel.Controls.Add(labelValueSlopeParameter, 2, 9);
		tableLayoutPanel.Controls.Add(labelElementNumberOfOppositions, 0, 10);
		tableLayoutPanel.Controls.Add(labelDesignationNumberOfOppositions, 1, 10);
		tableLayoutPanel.Controls.Add(labelValueNumberOfOppositions, 2, 10);
		tableLayoutPanel.Controls.Add(labelElementNumberOfObservations, 0, 11);
		tableLayoutPanel.Controls.Add(labelDesignationNumberOfObservations, 1, 11);
		tableLayoutPanel.Controls.Add(labelValueNumberOfObservations, 2, 11);
		tableLayoutPanel.Controls.Add(labelElementRmsResidual, 0, 12);
		tableLayoutPanel.Controls.Add(labelDesignationRmsResidual, 1, 12);
		tableLayoutPanel.Controls.Add(labelValueRmsResidual, 2, 12);
		tableLayoutPanel.Dock = DockStyle.Fill;
		tableLayoutPanel.Location = new Point(0, 0);
		tableLayoutPanel.Name = "tableLayoutPanel";
		tableLayoutPanel.RowCount = 13;
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
		tableLayoutPanel.Size = new Size(542, 373);
		tableLayoutPanel.TabIndex = 1;
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
		contextMenuSaveToFile.Size = new Size(202, 158);
		contextMenuSaveToFile.TabStop = true;
		contextMenuSaveToFile.Text = "&Save list";
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
		// labelElementHeader
		// 
		labelElementHeader.AccessibleDescription = "Column header for orbital element names";
		labelElementHeader.AccessibleName = "Orbital element header";
		labelElementHeader.AccessibleRole = AccessibleRole.StaticText;
		labelElementHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementHeader.Dock = DockStyle.Fill;
		labelElementHeader.LabelStyle = LabelStyle.BoldPanel;
		labelElementHeader.Location = new Point(3, 3);
		labelElementHeader.Name = "labelElementHeader";
		labelElementHeader.Size = new Size(214, 23);
		labelElementHeader.TabIndex = 0;
		labelElementHeader.Values.Text = "Orbital element";
		labelElementHeader.Enter += Control_Enter;
		labelElementHeader.Leave += Control_Leave;
		labelElementHeader.MouseDown += Control_MouseDown;
		labelElementHeader.MouseEnter += Control_Enter;
		labelElementHeader.MouseLeave += Control_Leave;
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
		// labelDesignationHeader
		// 
		labelDesignationHeader.AccessibleDescription = "Column header for readable designations";
		labelDesignationHeader.AccessibleName = "Readable designation header";
		labelDesignationHeader.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationHeader.Dock = DockStyle.Fill;
		labelDesignationHeader.LabelStyle = LabelStyle.BoldPanel;
		labelDesignationHeader.Location = new Point(223, 3);
		labelDesignationHeader.Name = "labelDesignationHeader";
		labelDesignationHeader.Size = new Size(182, 23);
		labelDesignationHeader.TabIndex = 1;
		labelDesignationHeader.Values.Text = "Readable designation";
		labelDesignationHeader.Enter += Control_Enter;
		labelDesignationHeader.Leave += Control_Leave;
		labelDesignationHeader.MouseDown += Control_MouseDown;
		labelDesignationHeader.MouseEnter += Control_Enter;
		labelDesignationHeader.MouseLeave += Control_Leave;
		// 
		// labelValueHeader
		// 
		labelValueHeader.AccessibleDescription = "Column header for record values";
		labelValueHeader.AccessibleName = "Value header";
		labelValueHeader.AccessibleRole = AccessibleRole.StaticText;
		labelValueHeader.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueHeader.Dock = DockStyle.Fill;
		labelValueHeader.LabelStyle = LabelStyle.BoldPanel;
		labelValueHeader.Location = new Point(411, 3);
		labelValueHeader.Name = "labelValueHeader";
		labelValueHeader.Size = new Size(128, 23);
		labelValueHeader.TabIndex = 2;
		labelValueHeader.Values.Text = "Value";
		labelValueHeader.Enter += Control_Enter;
		labelValueHeader.Leave += Control_Leave;
		labelValueHeader.MouseDown += Control_MouseDown;
		labelValueHeader.MouseEnter += Control_Enter;
		labelValueHeader.MouseLeave += Control_Leave;
		// 
		// labelElementMeanAnomalyAtTheEpoch
		// 
		labelElementMeanAnomalyAtTheEpoch.AccessibleDescription = "Orbital element: mean anomaly at the epoch";
		labelElementMeanAnomalyAtTheEpoch.AccessibleName = "Mean anomaly at the epoch";
		labelElementMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.StaticText;
		labelElementMeanAnomalyAtTheEpoch.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
		labelElementMeanAnomalyAtTheEpoch.Location = new Point(3, 32);
		labelElementMeanAnomalyAtTheEpoch.Name = "labelElementMeanAnomalyAtTheEpoch";
		labelElementMeanAnomalyAtTheEpoch.Size = new Size(214, 23);
		labelElementMeanAnomalyAtTheEpoch.TabIndex = 3;
		labelElementMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Mean anomaly at the epoch";
		labelElementMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
		labelElementMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Mean anomaly at the epoch";
		labelElementMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementMeanAnomalyAtTheEpoch.Values.Text = "Mean anomaly at the epoch";
		labelElementMeanAnomalyAtTheEpoch.Enter += Control_Enter;
		labelElementMeanAnomalyAtTheEpoch.Leave += Control_Leave;
		labelElementMeanAnomalyAtTheEpoch.MouseDown += Control_MouseDown;
		labelElementMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
		labelElementMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
		// 
		// labelDesignationMeanAnomalyAtTheEpoch
		// 
		labelDesignationMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the readable designation for the mean anomaly record";
		labelDesignationMeanAnomalyAtTheEpoch.AccessibleName = "Readable designation for the mean anomaly";
		labelDesignationMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationMeanAnomalyAtTheEpoch.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
		labelDesignationMeanAnomalyAtTheEpoch.Location = new Point(223, 32);
		labelDesignationMeanAnomalyAtTheEpoch.Name = "labelDesignationMeanAnomalyAtTheEpoch";
		labelDesignationMeanAnomalyAtTheEpoch.Size = new Size(182, 23);
		labelDesignationMeanAnomalyAtTheEpoch.TabIndex = 4;
		labelDesignationMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the mean anomaly at the epoch.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
		labelDesignationMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Readable designation for the mean anomaly";
		labelDesignationMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationMeanAnomalyAtTheEpoch.Values.Text = "-";
		labelDesignationMeanAnomalyAtTheEpoch.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationMeanAnomalyAtTheEpoch.Enter += Control_Enter;
		labelDesignationMeanAnomalyAtTheEpoch.Leave += Control_Leave;
		labelDesignationMeanAnomalyAtTheEpoch.MouseDown += Control_MouseDown;
		labelDesignationMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
		labelDesignationMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
		// 
		// labelValueMeanAnomalyAtTheEpoch
		// 
		labelValueMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the record value for the mean anomaly at the epoch";
		labelValueMeanAnomalyAtTheEpoch.AccessibleName = "Record value for the mean anomaly at the epoch";
		labelValueMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.StaticText;
		labelValueMeanAnomalyAtTheEpoch.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
		labelValueMeanAnomalyAtTheEpoch.Location = new Point(411, 32);
		labelValueMeanAnomalyAtTheEpoch.Name = "labelValueMeanAnomalyAtTheEpoch";
		labelValueMeanAnomalyAtTheEpoch.Size = new Size(128, 23);
		labelValueMeanAnomalyAtTheEpoch.TabIndex = 5;
		labelValueMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the record value for the mean anomaly at the epoch.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
		labelValueMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Record value for the mean anomaly at the epoch";
		labelValueMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueMeanAnomalyAtTheEpoch.Values.Text = "-";
		labelValueMeanAnomalyAtTheEpoch.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueMeanAnomalyAtTheEpoch.Enter += Control_Enter;
		labelValueMeanAnomalyAtTheEpoch.Leave += Control_Leave;
		labelValueMeanAnomalyAtTheEpoch.MouseDown += Control_MouseDown;
		labelValueMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
		labelValueMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
		// 
		// labelElementArgumentOfThePerihelion
		// 
		labelElementArgumentOfThePerihelion.AccessibleDescription = "Orbital element: Argument of the perihelion";
		labelElementArgumentOfThePerihelion.AccessibleName = "Argument of the perihelion";
		labelElementArgumentOfThePerihelion.AccessibleRole = AccessibleRole.StaticText;
		labelElementArgumentOfThePerihelion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementArgumentOfThePerihelion.Dock = DockStyle.Fill;
		labelElementArgumentOfThePerihelion.Location = new Point(3, 61);
		labelElementArgumentOfThePerihelion.Name = "labelElementArgumentOfThePerihelion";
		labelElementArgumentOfThePerihelion.Size = new Size(214, 23);
		labelElementArgumentOfThePerihelion.TabIndex = 6;
		labelElementArgumentOfThePerihelion.ToolTipValues.Description = "Argument of the perihelion";
		labelElementArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
		labelElementArgumentOfThePerihelion.ToolTipValues.Heading = "Argument of the perihelion";
		labelElementArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementArgumentOfThePerihelion.Values.Text = "Argument of the perihelion";
		labelElementArgumentOfThePerihelion.Enter += Control_Enter;
		labelElementArgumentOfThePerihelion.Leave += Control_Leave;
		labelElementArgumentOfThePerihelion.MouseDown += Control_MouseDown;
		labelElementArgumentOfThePerihelion.MouseEnter += Control_Enter;
		labelElementArgumentOfThePerihelion.MouseLeave += Control_Leave;
		// 
		// labelDesignationArgumentOfThePerihelion
		// 
		labelDesignationArgumentOfThePerihelion.AccessibleDescription = "Shows the readable designation for the argument of the perihelion record";
		labelDesignationArgumentOfThePerihelion.AccessibleName = "Readable designation for the argument of the perihelion";
		labelDesignationArgumentOfThePerihelion.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationArgumentOfThePerihelion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationArgumentOfThePerihelion.Dock = DockStyle.Fill;
		labelDesignationArgumentOfThePerihelion.Location = new Point(223, 61);
		labelDesignationArgumentOfThePerihelion.Name = "labelDesignationArgumentOfThePerihelion";
		labelDesignationArgumentOfThePerihelion.Size = new Size(182, 23);
		labelDesignationArgumentOfThePerihelion.TabIndex = 7;
		labelDesignationArgumentOfThePerihelion.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the argument of the perihelion.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
		labelDesignationArgumentOfThePerihelion.ToolTipValues.Heading = "Readable designation for the argument of the perihelion";
		labelDesignationArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationArgumentOfThePerihelion.Values.Text = "-";
		labelDesignationArgumentOfThePerihelion.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationArgumentOfThePerihelion.Enter += Control_Enter;
		labelDesignationArgumentOfThePerihelion.Leave += Control_Leave;
		labelDesignationArgumentOfThePerihelion.MouseDown += Control_MouseDown;
		labelDesignationArgumentOfThePerihelion.MouseEnter += Control_Enter;
		labelDesignationArgumentOfThePerihelion.MouseLeave += Control_Leave;
		// 
		// labelValueArgumentOfThePerihelion
		// 
		labelValueArgumentOfThePerihelion.AccessibleDescription = "Shows the record value for the argument of the perihelion";
		labelValueArgumentOfThePerihelion.AccessibleName = "Record value for the argument of the perihelion";
		labelValueArgumentOfThePerihelion.AccessibleRole = AccessibleRole.StaticText;
		labelValueArgumentOfThePerihelion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueArgumentOfThePerihelion.Dock = DockStyle.Fill;
		labelValueArgumentOfThePerihelion.Location = new Point(411, 61);
		labelValueArgumentOfThePerihelion.Name = "labelValueArgumentOfThePerihelion";
		labelValueArgumentOfThePerihelion.Size = new Size(128, 23);
		labelValueArgumentOfThePerihelion.TabIndex = 8;
		labelValueArgumentOfThePerihelion.ToolTipValues.Description = "Shows the record value for the argument of the perihelion.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
		labelValueArgumentOfThePerihelion.ToolTipValues.Heading = "Record value for the argument of the perihelion";
		labelValueArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueArgumentOfThePerihelion.Values.Text = "-";
		labelValueArgumentOfThePerihelion.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueArgumentOfThePerihelion.Enter += Control_Enter;
		labelValueArgumentOfThePerihelion.Leave += Control_Leave;
		labelValueArgumentOfThePerihelion.MouseDown += Control_MouseDown;
		labelValueArgumentOfThePerihelion.MouseEnter += Control_Enter;
		labelValueArgumentOfThePerihelion.MouseLeave += Control_Leave;
		// 
		// labelElementLongitudeOfTheAscendingNodeDesc
		// 
		labelElementLongitudeOfTheAscendingNodeDesc.AccessibleDescription = "Orbital element: Longitude of the ascending node";
		labelElementLongitudeOfTheAscendingNodeDesc.AccessibleName = "Longitude of the ascending node";
		labelElementLongitudeOfTheAscendingNodeDesc.AccessibleRole = AccessibleRole.StaticText;
		labelElementLongitudeOfTheAscendingNodeDesc.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementLongitudeOfTheAscendingNodeDesc.Dock = DockStyle.Fill;
		labelElementLongitudeOfTheAscendingNodeDesc.Location = new Point(3, 90);
		labelElementLongitudeOfTheAscendingNodeDesc.Name = "labelElementLongitudeOfTheAscendingNodeDesc";
		labelElementLongitudeOfTheAscendingNodeDesc.Size = new Size(214, 23);
		labelElementLongitudeOfTheAscendingNodeDesc.TabIndex = 9;
		labelElementLongitudeOfTheAscendingNodeDesc.ToolTipValues.Description = "Longitude of the ascending node";
		labelElementLongitudeOfTheAscendingNodeDesc.ToolTipValues.EnableToolTips = true;
		labelElementLongitudeOfTheAscendingNodeDesc.ToolTipValues.Heading = "Longitude of the ascending node";
		labelElementLongitudeOfTheAscendingNodeDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementLongitudeOfTheAscendingNodeDesc.Values.Text = "Longitude of the ascending node";
		labelElementLongitudeOfTheAscendingNodeDesc.Enter += Control_Enter;
		labelElementLongitudeOfTheAscendingNodeDesc.Leave += Control_Leave;
		labelElementLongitudeOfTheAscendingNodeDesc.MouseDown += Control_MouseDown;
		labelElementLongitudeOfTheAscendingNodeDesc.MouseEnter += Control_Enter;
		labelElementLongitudeOfTheAscendingNodeDesc.MouseLeave += Control_Leave;
		// 
		// labelDesignationLongitudeOfTheAscendingNode
		// 
		labelDesignationLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the readable designation for Longitude of the ascending node record";
		labelDesignationLongitudeOfTheAscendingNode.AccessibleName = "Readable designation for the longitude of the ascending node";
		labelDesignationLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationLongitudeOfTheAscendingNode.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
		labelDesignationLongitudeOfTheAscendingNode.Location = new Point(223, 90);
		labelDesignationLongitudeOfTheAscendingNode.Name = "labelDesignationLongitudeOfTheAscendingNode";
		labelDesignationLongitudeOfTheAscendingNode.Size = new Size(182, 23);
		labelDesignationLongitudeOfTheAscendingNode.TabIndex = 10;
		labelDesignationLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the longitude of the ascending node.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
		labelDesignationLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Readable designation for the longitude of the ascending node";
		labelDesignationLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationLongitudeOfTheAscendingNode.Values.Text = "-";
		labelDesignationLongitudeOfTheAscendingNode.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationLongitudeOfTheAscendingNode.Enter += Control_Enter;
		labelDesignationLongitudeOfTheAscendingNode.Leave += Control_Leave;
		labelDesignationLongitudeOfTheAscendingNode.MouseDown += Control_MouseDown;
		labelDesignationLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
		labelDesignationLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
		// 
		// labelValueLongitudeOfTheAscendingNode
		// 
		labelValueLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the record value for the longitude of the ascending node";
		labelValueLongitudeOfTheAscendingNode.AccessibleName = "Record value for the longitude of the ascending node";
		labelValueLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.StaticText;
		labelValueLongitudeOfTheAscendingNode.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
		labelValueLongitudeOfTheAscendingNode.Location = new Point(411, 90);
		labelValueLongitudeOfTheAscendingNode.Name = "labelValueLongitudeOfTheAscendingNode";
		labelValueLongitudeOfTheAscendingNode.Size = new Size(128, 23);
		labelValueLongitudeOfTheAscendingNode.TabIndex = 11;
		labelValueLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the record value for the longitude of the ascending node.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
		labelValueLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Record value for the longitude of the ascending node";
		labelValueLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueLongitudeOfTheAscendingNode.Values.Text = "-";
		labelValueLongitudeOfTheAscendingNode.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueLongitudeOfTheAscendingNode.Enter += Control_Enter;
		labelValueLongitudeOfTheAscendingNode.Leave += Control_Leave;
		labelValueLongitudeOfTheAscendingNode.MouseDown += Control_MouseDown;
		labelValueLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
		labelValueLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
		// 
		// labelElementInclinationToTheEcliptic
		// 
		labelElementInclinationToTheEcliptic.AccessibleDescription = "Orbital element: Inclination to the ecliptic";
		labelElementInclinationToTheEcliptic.AccessibleName = "Inclination to the ecliptic";
		labelElementInclinationToTheEcliptic.AccessibleRole = AccessibleRole.StaticText;
		labelElementInclinationToTheEcliptic.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementInclinationToTheEcliptic.Dock = DockStyle.Fill;
		labelElementInclinationToTheEcliptic.Location = new Point(3, 119);
		labelElementInclinationToTheEcliptic.Name = "labelElementInclinationToTheEcliptic";
		labelElementInclinationToTheEcliptic.Size = new Size(214, 23);
		labelElementInclinationToTheEcliptic.TabIndex = 12;
		labelElementInclinationToTheEcliptic.ToolTipValues.Description = "Inclination to the ecliptic";
		labelElementInclinationToTheEcliptic.ToolTipValues.EnableToolTips = true;
		labelElementInclinationToTheEcliptic.ToolTipValues.Heading = "Inclination to the ecliptic";
		labelElementInclinationToTheEcliptic.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementInclinationToTheEcliptic.Values.Text = "Inclination to the ecliptic";
		labelElementInclinationToTheEcliptic.Enter += Control_Enter;
		labelElementInclinationToTheEcliptic.Leave += Control_Leave;
		labelElementInclinationToTheEcliptic.MouseDown += Control_MouseDown;
		labelElementInclinationToTheEcliptic.MouseEnter += Control_Enter;
		labelElementInclinationToTheEcliptic.MouseLeave += Control_Leave;
		// 
		// labelDesignationInclinationToTheEcliptic
		// 
		labelDesignationInclinationToTheEcliptic.AccessibleDescription = "Shows the readable designation for the inclination to the ecliptic record";
		labelDesignationInclinationToTheEcliptic.AccessibleName = "Readable designation for the inclination to the ecliptic";
		labelDesignationInclinationToTheEcliptic.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationInclinationToTheEcliptic.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationInclinationToTheEcliptic.Dock = DockStyle.Fill;
		labelDesignationInclinationToTheEcliptic.Location = new Point(223, 119);
		labelDesignationInclinationToTheEcliptic.Name = "labelDesignationInclinationToTheEcliptic";
		labelDesignationInclinationToTheEcliptic.Size = new Size(182, 23);
		labelDesignationInclinationToTheEcliptic.TabIndex = 13;
		labelDesignationInclinationToTheEcliptic.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the inclination to the ecliptic.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationInclinationToTheEcliptic.ToolTipValues.EnableToolTips = true;
		labelDesignationInclinationToTheEcliptic.ToolTipValues.Heading = "Readable designation for the inclination to the ecliptic";
		labelDesignationInclinationToTheEcliptic.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationInclinationToTheEcliptic.Values.Text = "-";
		labelDesignationInclinationToTheEcliptic.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationInclinationToTheEcliptic.Enter += Control_Enter;
		labelDesignationInclinationToTheEcliptic.Leave += Control_Leave;
		labelDesignationInclinationToTheEcliptic.MouseDown += Control_MouseDown;
		labelDesignationInclinationToTheEcliptic.MouseEnter += Control_Enter;
		labelDesignationInclinationToTheEcliptic.MouseLeave += Control_Leave;
		// 
		// labelValueInclinationToTheEcliptic
		// 
		labelValueInclinationToTheEcliptic.AccessibleDescription = "Shows the record value for the inclination to the ecliptic";
		labelValueInclinationToTheEcliptic.AccessibleName = "Record value for the inclination to the ecliptic";
		labelValueInclinationToTheEcliptic.AccessibleRole = AccessibleRole.StaticText;
		labelValueInclinationToTheEcliptic.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueInclinationToTheEcliptic.Dock = DockStyle.Fill;
		labelValueInclinationToTheEcliptic.Location = new Point(411, 119);
		labelValueInclinationToTheEcliptic.Name = "labelValueInclinationToTheEcliptic";
		labelValueInclinationToTheEcliptic.Size = new Size(128, 23);
		labelValueInclinationToTheEcliptic.TabIndex = 14;
		labelValueInclinationToTheEcliptic.ToolTipValues.Description = "Shows the record value for the inclination to the ecliptic.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueInclinationToTheEcliptic.ToolTipValues.EnableToolTips = true;
		labelValueInclinationToTheEcliptic.ToolTipValues.Heading = "Record value for the inclination to the ecliptic";
		labelValueInclinationToTheEcliptic.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueInclinationToTheEcliptic.Values.Text = "-";
		labelValueInclinationToTheEcliptic.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueInclinationToTheEcliptic.Enter += Control_Enter;
		labelValueInclinationToTheEcliptic.Leave += Control_Leave;
		labelValueInclinationToTheEcliptic.MouseDown += Control_MouseDown;
		labelValueInclinationToTheEcliptic.MouseEnter += Control_Enter;
		labelValueInclinationToTheEcliptic.MouseLeave += Control_Leave;
		// 
		// labelElementOrbitalEccentricity
		// 
		labelElementOrbitalEccentricity.AccessibleDescription = "Orbital element: Orbital eccentricity";
		labelElementOrbitalEccentricity.AccessibleName = "Orbital eccentricity";
		labelElementOrbitalEccentricity.AccessibleRole = AccessibleRole.StaticText;
		labelElementOrbitalEccentricity.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementOrbitalEccentricity.Dock = DockStyle.Fill;
		labelElementOrbitalEccentricity.Location = new Point(3, 148);
		labelElementOrbitalEccentricity.Name = "labelElementOrbitalEccentricity";
		labelElementOrbitalEccentricity.Size = new Size(214, 23);
		labelElementOrbitalEccentricity.TabIndex = 15;
		labelElementOrbitalEccentricity.ToolTipValues.Description = "Orbital eccentricity";
		labelElementOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
		labelElementOrbitalEccentricity.ToolTipValues.Heading = "Orbital eccentricity";
		labelElementOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementOrbitalEccentricity.Values.Text = "Orbital eccentricity";
		labelElementOrbitalEccentricity.Enter += Control_Enter;
		labelElementOrbitalEccentricity.Leave += Control_Leave;
		labelElementOrbitalEccentricity.MouseDown += Control_MouseDown;
		labelElementOrbitalEccentricity.MouseEnter += Control_Enter;
		labelElementOrbitalEccentricity.MouseLeave += Control_Leave;
		// 
		// labelDesignationOrbitalEccentricity
		// 
		labelDesignationOrbitalEccentricity.AccessibleDescription = "Shows the readable designation for orbital eccentricity record";
		labelDesignationOrbitalEccentricity.AccessibleName = "Readable designation for orbital eccentricity";
		labelDesignationOrbitalEccentricity.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationOrbitalEccentricity.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationOrbitalEccentricity.Dock = DockStyle.Fill;
		labelDesignationOrbitalEccentricity.Location = new Point(223, 148);
		labelDesignationOrbitalEccentricity.Name = "labelDesignationOrbitalEccentricity";
		labelDesignationOrbitalEccentricity.Size = new Size(182, 23);
		labelDesignationOrbitalEccentricity.TabIndex = 16;
		labelDesignationOrbitalEccentricity.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the orbital eccentricity.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
		labelDesignationOrbitalEccentricity.ToolTipValues.Heading = "Readable designation for the orbital eccentricity";
		labelDesignationOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationOrbitalEccentricity.Values.Text = "-";
		labelDesignationOrbitalEccentricity.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationOrbitalEccentricity.Enter += Control_Enter;
		labelDesignationOrbitalEccentricity.Leave += Control_Leave;
		labelDesignationOrbitalEccentricity.MouseDown += Control_MouseDown;
		labelDesignationOrbitalEccentricity.MouseEnter += Control_Enter;
		labelDesignationOrbitalEccentricity.MouseLeave += Control_Leave;
		// 
		// labelValueOrbitalEccentricity
		// 
		labelValueOrbitalEccentricity.AccessibleDescription = "Shows the record value for the orbital eccentricity";
		labelValueOrbitalEccentricity.AccessibleName = "Record value for the orbital eccentricity";
		labelValueOrbitalEccentricity.AccessibleRole = AccessibleRole.StaticText;
		labelValueOrbitalEccentricity.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueOrbitalEccentricity.Dock = DockStyle.Fill;
		labelValueOrbitalEccentricity.Location = new Point(411, 148);
		labelValueOrbitalEccentricity.Name = "labelValueOrbitalEccentricity";
		labelValueOrbitalEccentricity.Size = new Size(128, 23);
		labelValueOrbitalEccentricity.TabIndex = 17;
		labelValueOrbitalEccentricity.ToolTipValues.Description = "Shows the record value for the orbital eccentricity.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
		labelValueOrbitalEccentricity.ToolTipValues.Heading = "Record value for the orbital eccentricity";
		labelValueOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueOrbitalEccentricity.Values.Text = "-";
		labelValueOrbitalEccentricity.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueOrbitalEccentricity.Enter += Control_Enter;
		labelValueOrbitalEccentricity.Leave += Control_Leave;
		labelValueOrbitalEccentricity.MouseDown += Control_MouseDown;
		labelValueOrbitalEccentricity.MouseEnter += Control_Enter;
		labelValueOrbitalEccentricity.MouseLeave += Control_Leave;
		// 
		// labelElementMeanDailyMotion
		// 
		labelElementMeanDailyMotion.AccessibleDescription = "Orbital element: Mean daily motion";
		labelElementMeanDailyMotion.AccessibleName = "Mean daily motion";
		labelElementMeanDailyMotion.AccessibleRole = AccessibleRole.StaticText;
		labelElementMeanDailyMotion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementMeanDailyMotion.Dock = DockStyle.Fill;
		labelElementMeanDailyMotion.Location = new Point(3, 177);
		labelElementMeanDailyMotion.Name = "labelElementMeanDailyMotion";
		labelElementMeanDailyMotion.Size = new Size(214, 23);
		labelElementMeanDailyMotion.TabIndex = 18;
		labelElementMeanDailyMotion.ToolTipValues.Description = "Mean daily motion";
		labelElementMeanDailyMotion.ToolTipValues.EnableToolTips = true;
		labelElementMeanDailyMotion.ToolTipValues.Heading = "Mean daily motion";
		labelElementMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementMeanDailyMotion.Values.Text = "Mean daily motion";
		labelElementMeanDailyMotion.Enter += Control_Enter;
		labelElementMeanDailyMotion.Leave += Control_Leave;
		labelElementMeanDailyMotion.MouseDown += Control_MouseDown;
		labelElementMeanDailyMotion.MouseEnter += Control_Enter;
		labelElementMeanDailyMotion.MouseLeave += Control_Leave;
		// 
		// labelDesignationMeanDailyMotion
		// 
		labelDesignationMeanDailyMotion.AccessibleDescription = "Shows the readable designation for the mean daily motion record";
		labelDesignationMeanDailyMotion.AccessibleName = "Readable designation for the mean daily motion";
		labelDesignationMeanDailyMotion.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationMeanDailyMotion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationMeanDailyMotion.Dock = DockStyle.Fill;
		labelDesignationMeanDailyMotion.Location = new Point(223, 177);
		labelDesignationMeanDailyMotion.Name = "labelDesignationMeanDailyMotion";
		labelDesignationMeanDailyMotion.Size = new Size(182, 23);
		labelDesignationMeanDailyMotion.TabIndex = 19;
		labelDesignationMeanDailyMotion.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the mean daily motion.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationMeanDailyMotion.ToolTipValues.EnableToolTips = true;
		labelDesignationMeanDailyMotion.ToolTipValues.Heading = "Readable designation for the mean daily motion";
		labelDesignationMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationMeanDailyMotion.Values.Text = "-";
		labelDesignationMeanDailyMotion.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationMeanDailyMotion.Enter += Control_Enter;
		labelDesignationMeanDailyMotion.Leave += Control_Leave;
		labelDesignationMeanDailyMotion.MouseDown += Control_MouseDown;
		labelDesignationMeanDailyMotion.MouseEnter += Control_Enter;
		labelDesignationMeanDailyMotion.MouseLeave += Control_Leave;
		// 
		// labelValueMeanDailyMotion
		// 
		labelValueMeanDailyMotion.AccessibleDescription = "Shows the record value for mean daily motion";
		labelValueMeanDailyMotion.AccessibleName = "Record value for the mean daily motion";
		labelValueMeanDailyMotion.AccessibleRole = AccessibleRole.StaticText;
		labelValueMeanDailyMotion.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueMeanDailyMotion.Dock = DockStyle.Fill;
		labelValueMeanDailyMotion.Location = new Point(411, 177);
		labelValueMeanDailyMotion.Name = "labelValueMeanDailyMotion";
		labelValueMeanDailyMotion.Size = new Size(128, 23);
		labelValueMeanDailyMotion.TabIndex = 20;
		labelValueMeanDailyMotion.ToolTipValues.Description = "Shows the record value for the mean daily motion.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueMeanDailyMotion.ToolTipValues.EnableToolTips = true;
		labelValueMeanDailyMotion.ToolTipValues.Heading = "Record value for mean daily motion";
		labelValueMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueMeanDailyMotion.Values.Text = "-";
		labelValueMeanDailyMotion.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueMeanDailyMotion.Enter += Control_Enter;
		labelValueMeanDailyMotion.Leave += Control_Leave;
		labelValueMeanDailyMotion.MouseDown += Control_MouseDown;
		labelValueMeanDailyMotion.MouseEnter += Control_Enter;
		labelValueMeanDailyMotion.MouseLeave += Control_Leave;
		// 
		// labelElementSemiMajorAxis
		// 
		labelElementSemiMajorAxis.AccessibleDescription = "Orbital element: Semi-major axis";
		labelElementSemiMajorAxis.AccessibleName = "Semi-major axis";
		labelElementSemiMajorAxis.AccessibleRole = AccessibleRole.StaticText;
		labelElementSemiMajorAxis.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementSemiMajorAxis.Dock = DockStyle.Fill;
		labelElementSemiMajorAxis.Location = new Point(3, 206);
		labelElementSemiMajorAxis.Name = "labelElementSemiMajorAxis";
		labelElementSemiMajorAxis.Size = new Size(214, 23);
		labelElementSemiMajorAxis.TabIndex = 21;
		labelElementSemiMajorAxis.ToolTipValues.Description = "Semi-major axis";
		labelElementSemiMajorAxis.ToolTipValues.EnableToolTips = true;
		labelElementSemiMajorAxis.ToolTipValues.Heading = "Semi-major axis";
		labelElementSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementSemiMajorAxis.Values.Text = "Semi-major axis";
		labelElementSemiMajorAxis.Enter += Control_Enter;
		labelElementSemiMajorAxis.Leave += Control_Leave;
		labelElementSemiMajorAxis.MouseDown += Control_MouseDown;
		labelElementSemiMajorAxis.MouseEnter += Control_Enter;
		labelElementSemiMajorAxis.MouseLeave += Control_Leave;
		// 
		// labelDesignationSemiMajorAxis
		// 
		labelDesignationSemiMajorAxis.AccessibleDescription = "Shows the readable designation for the semi-major axis record";
		labelDesignationSemiMajorAxis.AccessibleName = "Readable designation for the semi-major axis";
		labelDesignationSemiMajorAxis.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationSemiMajorAxis.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationSemiMajorAxis.Dock = DockStyle.Fill;
		labelDesignationSemiMajorAxis.Location = new Point(223, 206);
		labelDesignationSemiMajorAxis.Name = "labelDesignationSemiMajorAxis";
		labelDesignationSemiMajorAxis.Size = new Size(182, 23);
		labelDesignationSemiMajorAxis.TabIndex = 22;
		labelDesignationSemiMajorAxis.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the semi-major axis.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationSemiMajorAxis.ToolTipValues.EnableToolTips = true;
		labelDesignationSemiMajorAxis.ToolTipValues.Heading = "Readable designation for semi-major axis";
		labelDesignationSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationSemiMajorAxis.Values.Text = "-";
		labelDesignationSemiMajorAxis.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationSemiMajorAxis.Enter += Control_Enter;
		labelDesignationSemiMajorAxis.Leave += Control_Leave;
		labelDesignationSemiMajorAxis.MouseDown += Control_MouseDown;
		labelDesignationSemiMajorAxis.MouseEnter += Control_Enter;
		labelDesignationSemiMajorAxis.MouseLeave += Control_Leave;
		// 
		// labelValueSemiMajorAxis
		// 
		labelValueSemiMajorAxis.AccessibleDescription = "Shows the record value for the semi-major axis";
		labelValueSemiMajorAxis.AccessibleName = "Record value for the semi-major axis";
		labelValueSemiMajorAxis.AccessibleRole = AccessibleRole.StaticText;
		labelValueSemiMajorAxis.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueSemiMajorAxis.Dock = DockStyle.Fill;
		labelValueSemiMajorAxis.Location = new Point(411, 206);
		labelValueSemiMajorAxis.Name = "labelValueSemiMajorAxis";
		labelValueSemiMajorAxis.Size = new Size(128, 23);
		labelValueSemiMajorAxis.TabIndex = 23;
		labelValueSemiMajorAxis.ToolTipValues.Description = "Shows the record value for the semi-major axis.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueSemiMajorAxis.ToolTipValues.EnableToolTips = true;
		labelValueSemiMajorAxis.ToolTipValues.Heading = "Record value for the semi-major axis";
		labelValueSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueSemiMajorAxis.Values.Text = "-";
		labelValueSemiMajorAxis.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueSemiMajorAxis.Enter += Control_Enter;
		labelValueSemiMajorAxis.Leave += Control_Leave;
		labelValueSemiMajorAxis.MouseDown += Control_MouseDown;
		labelValueSemiMajorAxis.MouseEnter += Control_Enter;
		labelValueSemiMajorAxis.MouseLeave += Control_Leave;
		// 
		// labelElementAbsoluteMagnitude
		// 
		labelElementAbsoluteMagnitude.AccessibleDescription = "Orbital element: Absolute magnitude (H)";
		labelElementAbsoluteMagnitude.AccessibleName = "Absolute magnitude";
		labelElementAbsoluteMagnitude.AccessibleRole = AccessibleRole.StaticText;
		labelElementAbsoluteMagnitude.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementAbsoluteMagnitude.Dock = DockStyle.Fill;
		labelElementAbsoluteMagnitude.Location = new Point(3, 235);
		labelElementAbsoluteMagnitude.Name = "labelElementAbsoluteMagnitude";
		labelElementAbsoluteMagnitude.Size = new Size(214, 23);
		labelElementAbsoluteMagnitude.TabIndex = 24;
		labelElementAbsoluteMagnitude.ToolTipValues.Description = "Absolute magnitude (H)";
		labelElementAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
		labelElementAbsoluteMagnitude.ToolTipValues.Heading = "Absolute magnitude (H)";
		labelElementAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementAbsoluteMagnitude.Values.Text = "Absolute magnitude (H)";
		labelElementAbsoluteMagnitude.Enter += Control_Enter;
		labelElementAbsoluteMagnitude.Leave += Control_Leave;
		labelElementAbsoluteMagnitude.MouseDown += Control_MouseDown;
		labelElementAbsoluteMagnitude.MouseEnter += Control_Enter;
		labelElementAbsoluteMagnitude.MouseLeave += Control_Leave;
		// 
		// labelDesignationAbsoluteMagnitude
		// 
		labelDesignationAbsoluteMagnitude.AccessibleDescription = "Shows the readable designation for the absolute magnitude record";
		labelDesignationAbsoluteMagnitude.AccessibleName = "Readable designation for the absolute magnitude";
		labelDesignationAbsoluteMagnitude.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationAbsoluteMagnitude.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationAbsoluteMagnitude.Dock = DockStyle.Fill;
		labelDesignationAbsoluteMagnitude.Location = new Point(223, 235);
		labelDesignationAbsoluteMagnitude.Name = "labelDesignationAbsoluteMagnitude";
		labelDesignationAbsoluteMagnitude.Size = new Size(182, 23);
		labelDesignationAbsoluteMagnitude.TabIndex = 25;
		labelDesignationAbsoluteMagnitude.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the absolute magnitude.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
		labelDesignationAbsoluteMagnitude.ToolTipValues.Heading = "Readable designation for the absolute magnitude";
		labelDesignationAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationAbsoluteMagnitude.Values.Text = "-";
		labelDesignationAbsoluteMagnitude.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationAbsoluteMagnitude.Enter += Control_Enter;
		labelDesignationAbsoluteMagnitude.Leave += Control_Leave;
		labelDesignationAbsoluteMagnitude.MouseDown += Control_MouseDown;
		labelDesignationAbsoluteMagnitude.MouseEnter += Control_Enter;
		labelDesignationAbsoluteMagnitude.MouseLeave += Control_Leave;
		// 
		// labelValueAbsoluteMagnitude
		// 
		labelValueAbsoluteMagnitude.AccessibleDescription = "Shows the record value for the absolute magnitude";
		labelValueAbsoluteMagnitude.AccessibleName = "Record value for the absolute magnitude";
		labelValueAbsoluteMagnitude.AccessibleRole = AccessibleRole.StaticText;
		labelValueAbsoluteMagnitude.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueAbsoluteMagnitude.Dock = DockStyle.Fill;
		labelValueAbsoluteMagnitude.Location = new Point(411, 235);
		labelValueAbsoluteMagnitude.Name = "labelValueAbsoluteMagnitude";
		labelValueAbsoluteMagnitude.Size = new Size(128, 23);
		labelValueAbsoluteMagnitude.TabIndex = 26;
		labelValueAbsoluteMagnitude.ToolTipValues.Description = "Shows the record value for the absolute magnitude.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
		labelValueAbsoluteMagnitude.ToolTipValues.Heading = "Record value for the absolute magnitude";
		labelValueAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueAbsoluteMagnitude.Values.Text = "-";
		labelValueAbsoluteMagnitude.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueAbsoluteMagnitude.Enter += Control_Enter;
		labelValueAbsoluteMagnitude.Leave += Control_Leave;
		labelValueAbsoluteMagnitude.MouseDown += Control_MouseDown;
		labelValueAbsoluteMagnitude.MouseEnter += Control_Enter;
		labelValueAbsoluteMagnitude.MouseLeave += Control_Leave;
		// 
		// labelElementSlopeParameter
		// 
		labelElementSlopeParameter.AccessibleDescription = "Orbital element: Slope parameter (G)";
		labelElementSlopeParameter.AccessibleName = "Slope parameter";
		labelElementSlopeParameter.AccessibleRole = AccessibleRole.StaticText;
		labelElementSlopeParameter.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementSlopeParameter.Dock = DockStyle.Fill;
		labelElementSlopeParameter.Location = new Point(3, 264);
		labelElementSlopeParameter.Name = "labelElementSlopeParameter";
		labelElementSlopeParameter.Size = new Size(214, 23);
		labelElementSlopeParameter.TabIndex = 27;
		labelElementSlopeParameter.ToolTipValues.Description = "Slope parameter (G)";
		labelElementSlopeParameter.ToolTipValues.EnableToolTips = true;
		labelElementSlopeParameter.ToolTipValues.Heading = "Slope parameter (G)";
		labelElementSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementSlopeParameter.Values.Text = "Slope parameter (G)";
		labelElementSlopeParameter.Enter += Control_Enter;
		labelElementSlopeParameter.Leave += Control_Leave;
		labelElementSlopeParameter.MouseDown += Control_MouseDown;
		labelElementSlopeParameter.MouseEnter += Control_Enter;
		labelElementSlopeParameter.MouseLeave += Control_Leave;
		// 
		// labelDesignationSlopeParameter
		// 
		labelDesignationSlopeParameter.AccessibleDescription = "Shows the readable designation for the slope parameter record";
		labelDesignationSlopeParameter.AccessibleName = "Readable designation for the slope parameter";
		labelDesignationSlopeParameter.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationSlopeParameter.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationSlopeParameter.Dock = DockStyle.Fill;
		labelDesignationSlopeParameter.Location = new Point(223, 264);
		labelDesignationSlopeParameter.Name = "labelDesignationSlopeParameter";
		labelDesignationSlopeParameter.Size = new Size(182, 23);
		labelDesignationSlopeParameter.TabIndex = 28;
		labelDesignationSlopeParameter.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the slope parameter.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationSlopeParameter.ToolTipValues.EnableToolTips = true;
		labelDesignationSlopeParameter.ToolTipValues.Heading = "Readable designation for the slope parameter";
		labelDesignationSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationSlopeParameter.Values.Text = "-";
		labelDesignationSlopeParameter.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationSlopeParameter.Enter += Control_Enter;
		labelDesignationSlopeParameter.Leave += Control_Leave;
		labelDesignationSlopeParameter.MouseDown += Control_MouseDown;
		labelDesignationSlopeParameter.MouseEnter += Control_Enter;
		labelDesignationSlopeParameter.MouseLeave += Control_Leave;
		// 
		// labelValueSlopeParameter
		// 
		labelValueSlopeParameter.AccessibleDescription = "Shows the record value for the slope parameter";
		labelValueSlopeParameter.AccessibleName = "Record value for the slope parameter";
		labelValueSlopeParameter.AccessibleRole = AccessibleRole.StaticText;
		labelValueSlopeParameter.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueSlopeParameter.Dock = DockStyle.Fill;
		labelValueSlopeParameter.Location = new Point(411, 264);
		labelValueSlopeParameter.Name = "labelValueSlopeParameter";
		labelValueSlopeParameter.Size = new Size(128, 23);
		labelValueSlopeParameter.TabIndex = 29;
		labelValueSlopeParameter.ToolTipValues.Description = "Shows the record value for the slope parameter.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueSlopeParameter.ToolTipValues.EnableToolTips = true;
		labelValueSlopeParameter.ToolTipValues.Heading = "Record value for the slope parameter";
		labelValueSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueSlopeParameter.Values.Text = "-";
		labelValueSlopeParameter.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueSlopeParameter.Enter += Control_Enter;
		labelValueSlopeParameter.Leave += Control_Leave;
		labelValueSlopeParameter.MouseDown += Control_MouseDown;
		labelValueSlopeParameter.MouseEnter += Control_Enter;
		labelValueSlopeParameter.MouseLeave += Control_Leave;
		// 
		// labelElementNumberOfOppositions
		// 
		labelElementNumberOfOppositions.AccessibleDescription = "Orbital element: Number of oppositions";
		labelElementNumberOfOppositions.AccessibleName = "Number of oppositions";
		labelElementNumberOfOppositions.AccessibleRole = AccessibleRole.StaticText;
		labelElementNumberOfOppositions.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementNumberOfOppositions.Dock = DockStyle.Fill;
		labelElementNumberOfOppositions.Location = new Point(3, 293);
		labelElementNumberOfOppositions.Name = "labelElementNumberOfOppositions";
		labelElementNumberOfOppositions.Size = new Size(214, 23);
		labelElementNumberOfOppositions.TabIndex = 30;
		labelElementNumberOfOppositions.ToolTipValues.Description = "Number of oppositions";
		labelElementNumberOfOppositions.ToolTipValues.EnableToolTips = true;
		labelElementNumberOfOppositions.ToolTipValues.Heading = "Number of oppositions";
		labelElementNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementNumberOfOppositions.Values.Text = "Number of oppositions";
		labelElementNumberOfOppositions.Enter += Control_Enter;
		labelElementNumberOfOppositions.Leave += Control_Leave;
		labelElementNumberOfOppositions.MouseDown += Control_MouseDown;
		labelElementNumberOfOppositions.MouseEnter += Control_Enter;
		labelElementNumberOfOppositions.MouseLeave += Control_Leave;
		// 
		// labelDesignationNumberOfOppositions
		// 
		labelDesignationNumberOfOppositions.AccessibleDescription = "Shows the readable designation for the number of oppositions record";
		labelDesignationNumberOfOppositions.AccessibleName = "Readable designation for the number of oppositions";
		labelDesignationNumberOfOppositions.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationNumberOfOppositions.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationNumberOfOppositions.Dock = DockStyle.Fill;
		labelDesignationNumberOfOppositions.Location = new Point(223, 293);
		labelDesignationNumberOfOppositions.Name = "labelDesignationNumberOfOppositions";
		labelDesignationNumberOfOppositions.Size = new Size(182, 23);
		labelDesignationNumberOfOppositions.TabIndex = 31;
		labelDesignationNumberOfOppositions.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the number of oppositions.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationNumberOfOppositions.ToolTipValues.EnableToolTips = true;
		labelDesignationNumberOfOppositions.ToolTipValues.Heading = "Readable designation for the number of oppositions";
		labelDesignationNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationNumberOfOppositions.Values.Text = "-";
		labelDesignationNumberOfOppositions.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationNumberOfOppositions.Enter += Control_Enter;
		labelDesignationNumberOfOppositions.Leave += Control_Leave;
		labelDesignationNumberOfOppositions.MouseDown += Control_MouseDown;
		labelDesignationNumberOfOppositions.MouseEnter += Control_Enter;
		labelDesignationNumberOfOppositions.MouseLeave += Control_Leave;
		// 
		// labelValueNumberOfOppositions
		// 
		labelValueNumberOfOppositions.AccessibleDescription = "Shows the record value for the number of oppositions";
		labelValueNumberOfOppositions.AccessibleName = "Record value for the number of oppositions";
		labelValueNumberOfOppositions.AccessibleRole = AccessibleRole.StaticText;
		labelValueNumberOfOppositions.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueNumberOfOppositions.Dock = DockStyle.Fill;
		labelValueNumberOfOppositions.Location = new Point(411, 293);
		labelValueNumberOfOppositions.Name = "labelValueNumberOfOppositions";
		labelValueNumberOfOppositions.Size = new Size(128, 23);
		labelValueNumberOfOppositions.TabIndex = 32;
		labelValueNumberOfOppositions.ToolTipValues.Description = "Shows the record value for the number of oppositions.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueNumberOfOppositions.ToolTipValues.EnableToolTips = true;
		labelValueNumberOfOppositions.ToolTipValues.Heading = "Record value for the number of oppositions";
		labelValueNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueNumberOfOppositions.Values.Text = "-";
		labelValueNumberOfOppositions.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueNumberOfOppositions.Enter += Control_Enter;
		labelValueNumberOfOppositions.Leave += Control_Leave;
		labelValueNumberOfOppositions.MouseDown += Control_MouseDown;
		labelValueNumberOfOppositions.MouseEnter += Control_Enter;
		labelValueNumberOfOppositions.MouseLeave += Control_Leave;
		// 
		// labelElementNumberOfObservations
		// 
		labelElementNumberOfObservations.AccessibleDescription = "Orbital element: Number of observations";
		labelElementNumberOfObservations.AccessibleName = "Number of observations";
		labelElementNumberOfObservations.AccessibleRole = AccessibleRole.StaticText;
		labelElementNumberOfObservations.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementNumberOfObservations.Dock = DockStyle.Fill;
		labelElementNumberOfObservations.Location = new Point(3, 322);
		labelElementNumberOfObservations.Name = "labelElementNumberOfObservations";
		labelElementNumberOfObservations.Size = new Size(214, 23);
		labelElementNumberOfObservations.TabIndex = 33;
		labelElementNumberOfObservations.ToolTipValues.Description = "Number of observations";
		labelElementNumberOfObservations.ToolTipValues.EnableToolTips = true;
		labelElementNumberOfObservations.ToolTipValues.Heading = "Number of observations";
		labelElementNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementNumberOfObservations.Values.Text = "Number of observations";
		labelElementNumberOfObservations.Enter += Control_Enter;
		labelElementNumberOfObservations.Leave += Control_Leave;
		labelElementNumberOfObservations.MouseDown += Control_MouseDown;
		labelElementNumberOfObservations.MouseEnter += Control_Enter;
		labelElementNumberOfObservations.MouseLeave += Control_Leave;
		// 
		// labelDesignationNumberOfObservations
		// 
		labelDesignationNumberOfObservations.AccessibleDescription = "Shows the readable designation for the number of the observations record";
		labelDesignationNumberOfObservations.AccessibleName = "Readable designation for the number of the observations";
		labelDesignationNumberOfObservations.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationNumberOfObservations.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationNumberOfObservations.Dock = DockStyle.Fill;
		labelDesignationNumberOfObservations.Location = new Point(223, 322);
		labelDesignationNumberOfObservations.Name = "labelDesignationNumberOfObservations";
		labelDesignationNumberOfObservations.Size = new Size(182, 23);
		labelDesignationNumberOfObservations.TabIndex = 34;
		labelDesignationNumberOfObservations.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the number of the observations.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationNumberOfObservations.ToolTipValues.EnableToolTips = true;
		labelDesignationNumberOfObservations.ToolTipValues.Heading = "Readable designation for the number of the observations";
		labelDesignationNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationNumberOfObservations.Values.Text = "-";
		labelDesignationNumberOfObservations.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationNumberOfObservations.Enter += Control_Enter;
		labelDesignationNumberOfObservations.Leave += Control_Leave;
		labelDesignationNumberOfObservations.MouseDown += Control_MouseDown;
		labelDesignationNumberOfObservations.MouseEnter += Control_Enter;
		labelDesignationNumberOfObservations.MouseLeave += Control_Leave;
		// 
		// labelValueNumberOfObservations
		// 
		labelValueNumberOfObservations.AccessibleDescription = "Shows the record value for the number of the observations";
		labelValueNumberOfObservations.AccessibleName = "Record value for the number of the observations";
		labelValueNumberOfObservations.AccessibleRole = AccessibleRole.StaticText;
		labelValueNumberOfObservations.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueNumberOfObservations.Dock = DockStyle.Fill;
		labelValueNumberOfObservations.Location = new Point(411, 322);
		labelValueNumberOfObservations.Name = "labelValueNumberOfObservations";
		labelValueNumberOfObservations.Size = new Size(128, 23);
		labelValueNumberOfObservations.TabIndex = 35;
		labelValueNumberOfObservations.ToolTipValues.Description = "Shows the record value for the number of the observations.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueNumberOfObservations.ToolTipValues.EnableToolTips = true;
		labelValueNumberOfObservations.ToolTipValues.Heading = "Record value for the number of the observations";
		labelValueNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueNumberOfObservations.Values.Text = "-";
		labelValueNumberOfObservations.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueNumberOfObservations.Enter += Control_Enter;
		labelValueNumberOfObservations.Leave += Control_Leave;
		labelValueNumberOfObservations.MouseDown += Control_MouseDown;
		labelValueNumberOfObservations.MouseEnter += Control_Enter;
		labelValueNumberOfObservations.MouseLeave += Control_Leave;
		// 
		// labelElementRmsResidual
		// 
		labelElementRmsResidual.AccessibleDescription = "Orbital element: RMS residual";
		labelElementRmsResidual.AccessibleName = "RMS residual";
		labelElementRmsResidual.AccessibleRole = AccessibleRole.StaticText;
		labelElementRmsResidual.ContextMenuStrip = contextMenuCopyToClipboard;
		labelElementRmsResidual.Dock = DockStyle.Fill;
		labelElementRmsResidual.Location = new Point(3, 351);
		labelElementRmsResidual.Name = "labelElementRmsResidual";
		labelElementRmsResidual.Size = new Size(214, 23);
		labelElementRmsResidual.TabIndex = 39;
		labelElementRmsResidual.ToolTipValues.Description = "RMS residual";
		labelElementRmsResidual.ToolTipValues.EnableToolTips = true;
		labelElementRmsResidual.ToolTipValues.Heading = "RMS residual";
		labelElementRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelElementRmsResidual.Values.Text = "RMS residual";
		labelElementRmsResidual.Enter += Control_Enter;
		labelElementRmsResidual.Leave += Control_Leave;
		labelElementRmsResidual.MouseDown += Control_MouseDown;
		labelElementRmsResidual.MouseEnter += Control_Enter;
		labelElementRmsResidual.MouseLeave += Control_Leave;
		// 
		// labelDesignationRmsResidual
		// 
		labelDesignationRmsResidual.AccessibleDescription = "Shows the readable designation for the RMS residual record";
		labelDesignationRmsResidual.AccessibleName = "Readable designation for the RMS residual";
		labelDesignationRmsResidual.AccessibleRole = AccessibleRole.StaticText;
		labelDesignationRmsResidual.ContextMenuStrip = contextMenuCopyToClipboard;
		labelDesignationRmsResidual.Dock = DockStyle.Fill;
		labelDesignationRmsResidual.Location = new Point(223, 351);
		labelDesignationRmsResidual.Name = "labelDesignationRmsResidual";
		labelDesignationRmsResidual.Size = new Size(182, 23);
		labelDesignationRmsResidual.TabIndex = 40;
		labelDesignationRmsResidual.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for the RMS residual.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelDesignationRmsResidual.ToolTipValues.EnableToolTips = true;
		labelDesignationRmsResidual.ToolTipValues.Heading = "Readable designation for the RMS residual";
		labelDesignationRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelDesignationRmsResidual.Values.Text = "-";
		labelDesignationRmsResidual.DoubleClick += CopyToClipboard_DoubleClick;
		labelDesignationRmsResidual.Enter += Control_Enter;
		labelDesignationRmsResidual.Leave += Control_Leave;
		labelDesignationRmsResidual.MouseDown += Control_MouseDown;
		labelDesignationRmsResidual.MouseEnter += Control_Enter;
		labelDesignationRmsResidual.MouseLeave += Control_Leave;
		// 
		// labelValueRmsResidual
		// 
		labelValueRmsResidual.AccessibleDescription = "Shows the record value for the RMS residual";
		labelValueRmsResidual.AccessibleName = "Record value for the RMS residual";
		labelValueRmsResidual.AccessibleRole = AccessibleRole.StaticText;
		labelValueRmsResidual.ContextMenuStrip = contextMenuCopyToClipboard;
		labelValueRmsResidual.Dock = DockStyle.Fill;
		labelValueRmsResidual.Location = new Point(411, 351);
		labelValueRmsResidual.Name = "labelValueRmsResidual";
		labelValueRmsResidual.Size = new Size(128, 23);
		labelValueRmsResidual.TabIndex = 41;
		labelValueRmsResidual.ToolTipValues.Description = "Shows the record value for the RMS residual.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelValueRmsResidual.ToolTipValues.EnableToolTips = true;
		labelValueRmsResidual.ToolTipValues.Heading = "Record value for the RMS residual";
		labelValueRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelValueRmsResidual.Values.Text = "-";
		labelValueRmsResidual.DoubleClick += CopyToClipboard_DoubleClick;
		labelValueRmsResidual.Enter += Control_Enter;
		labelValueRmsResidual.Leave += Control_Leave;
		labelValueRmsResidual.MouseDown += Control_MouseDown;
		labelValueRmsResidual.MouseEnter += Control_Enter;
		labelValueRmsResidual.MouseLeave += Control_Leave;
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
		kryptonStatusStrip.Size = new Size(542, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 6;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
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
		// backgroundWorker
		// 
		backgroundWorker.WorkerReportsProgress = true;
		backgroundWorker.WorkerSupportsCancellation = true;
		// 
		// toolStripContainer
		// 
		toolStripContainer.AccessibleDescription = "Container";
		toolStripContainer.AccessibleName = "Container";
		toolStripContainer.AccessibleRole = AccessibleRole.Pane;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.AccessibleDescription = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleName = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.AccessibleDescription = "Content panel";
		toolStripContainer.ContentPanel.AccessibleName = "Content panel";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(542, 373);
		toolStripContainer.Dock = DockStyle.Fill;
		// 
		// toolStripContainer.LeftToolStripPanel
		// 
		toolStripContainer.LeftToolStripPanel.AccessibleDescription = "Left panel";
		toolStripContainer.LeftToolStripPanel.AccessibleName = "Left panel";
		toolStripContainer.LeftToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		// 
		// toolStripContainer.RightToolStripPanel
		// 
		toolStripContainer.RightToolStripPanel.AccessibleDescription = "Right panel";
		toolStripContainer.RightToolStripPanel.AccessibleName = "Right panel";
		toolStripContainer.RightToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Size = new Size(542, 445);
		toolStripContainer.TabIndex = 8;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.AccessibleDescription = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleName = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleRole = AccessibleRole.Pane;
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
		kryptonToolStripGenerateList.Size = new Size(542, 25);
		kryptonToolStripGenerateList.Stretch = true;
		kryptonToolStripGenerateList.TabIndex = 1;
		kryptonToolStripGenerateList.TabStop = true;
		kryptonToolStripGenerateList.Text = "Toolbar of generating list";
		kryptonToolStripGenerateList.Enter += Control_Enter;
		kryptonToolStripGenerateList.Leave += Control_Leave;
		kryptonToolStripGenerateList.MouseEnter += Control_Enter;
		kryptonToolStripGenerateList.MouseLeave += Control_Leave;
		// 
		// toolStripButtonStart
		// 
		toolStripButtonStart.AccessibleDescription = "Starts the record detection scan";
		toolStripButtonStart.AccessibleName = "Start scan";
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
		toolStripButtonSortOrderAscending.Click += SetAscendingSortOrder_Click;
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
		toolStripButtonSortOrderDescending.Click += SetDescendingSortOrder_Click;
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
		kryptonToolStripProgress.Size = new Size(542, 25);
		kryptonToolStripProgress.Stretch = true;
		kryptonToolStripProgress.TabIndex = 2;
		kryptonToolStripProgress.TabStop = true;
		kryptonToolStripProgress.Text = "Toolbar of progress";
		kryptonToolStripProgress.Enter += Control_Enter;
		kryptonToolStripProgress.Leave += Control_Leave;
		kryptonToolStripProgress.MouseEnter += Control_Enter;
		kryptonToolStripProgress.MouseLeave += Control_Leave;
		// 
		// toolStripLabelProgress
		// 
		toolStripLabelProgress.AccessibleDescription = "Shows the progress of generating";
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
		kryptonProgressBar.AccessibleDescription = "Shows the progress of generating";
		kryptonProgressBar.AccessibleName = "Loading progress";
		kryptonProgressBar.AccessibleRole = AccessibleRole.ProgressBar;
		kryptonProgressBar.AutoToolTip = true;
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(400, 22);
		kryptonProgressBar.StateCommon.Back.Color1 = Color.Green;
		kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.Values.Text = "";
		kryptonProgressBar.MouseEnter += Control_Enter;
		kryptonProgressBar.MouseLeave += Control_Leave;
		// 
		// RecordsForm
		// 
		AccessibleDescription = "Shows the record values for all orbital elements";
		AccessibleName = "Records";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(542, 445);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "RecordsForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Records";
		Load += RecordsForm_Load;
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
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
	private KryptonTableLayoutPanel tableLayoutPanel;
	private KryptonLabel labelElementHeader;
	private KryptonLabel labelDesignationHeader;
	private KryptonLabel labelValueHeader;
	private KryptonLabel labelElementMeanAnomalyAtTheEpoch;
	private KryptonLabel labelElementArgumentOfThePerihelion;
	private KryptonLabel labelElementLongitudeOfTheAscendingNodeDesc;
	private KryptonLabel labelElementInclinationToTheEcliptic;
	private KryptonLabel labelElementOrbitalEccentricity;
	private KryptonLabel labelElementMeanDailyMotion;
	private KryptonLabel labelElementSemiMajorAxis;
	private KryptonLabel labelElementAbsoluteMagnitude;
	private KryptonLabel labelElementSlopeParameter;
	private KryptonLabel labelElementNumberOfOppositions;
	private KryptonLabel labelElementNumberOfObservations;
	private KryptonLabel labelElementRmsResidual;
	private KryptonLabel labelDesignationMeanAnomalyAtTheEpoch;
	private KryptonLabel labelDesignationArgumentOfThePerihelion;
	private KryptonLabel labelDesignationLongitudeOfTheAscendingNode;
	private KryptonLabel labelDesignationInclinationToTheEcliptic;
	private KryptonLabel labelDesignationOrbitalEccentricity;
	private KryptonLabel labelDesignationMeanDailyMotion;
	private KryptonLabel labelDesignationSemiMajorAxis;
	private KryptonLabel labelDesignationAbsoluteMagnitude;
	private KryptonLabel labelDesignationSlopeParameter;
	private KryptonLabel labelDesignationNumberOfOppositions;
	private KryptonLabel labelDesignationNumberOfObservations;
	private KryptonLabel labelDesignationRmsResidual;
	private KryptonLabel labelValueMeanAnomalyAtTheEpoch;
	private KryptonLabel labelValueArgumentOfThePerihelion;
	private KryptonLabel labelValueLongitudeOfTheAscendingNode;
	private KryptonLabel labelValueInclinationToTheEcliptic;
	private KryptonLabel labelValueOrbitalEccentricity;
	private KryptonLabel labelValueMeanDailyMotion;
	private KryptonLabel labelValueSemiMajorAxis;
	private KryptonLabel labelValueAbsoluteMagnitude;
	private KryptonLabel labelValueSlopeParameter;
	private KryptonLabel labelValueNumberOfOppositions;
	private KryptonLabel labelValueNumberOfObservations;
	private KryptonLabel labelValueRmsResidual;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
	private BackgroundWorker backgroundWorker;
	private ToolStripContainer toolStripContainer;
	private KryptonToolStrip kryptonToolStripGenerateList;
	private ToolStripButton toolStripButtonStart;
	private ToolStripDropDownButton toolStripDropDownButtonSaveList;
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
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripButton toolStripButtonSortOrderAscending;
	private ToolStripButton toolStripButtonSortOrderDescending;
	private ToolStripSeparator toolStripSeparator3;
	private KryptonToolStrip kryptonToolStripProgress;
	private ToolStripLabel toolStripLabelProgress;
	private KryptonProgressBarToolStripItem kryptonProgressBar;
	private ContextMenuStrip contextMenuCopyToClipboard;
	private ToolStripMenuItem toolStripMenuItemCopyToClipboardInContextMenu;
}