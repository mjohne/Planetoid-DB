// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a form that displays data in a table format.</summary>
/// <remarks>This form is used to display data from the MPCORB.DAT file in a tabular format, allowing users to specify a range and view the results.</remarks>
partial class TableModeForm
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

	/// <summary>Required method for Designer support - do not modify
	/// the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(TableModeForm));
		listView = new ListView();
		columnHeaderIndex = new ColumnHeader();
		columnHeaderReadableDesignation = new ColumnHeader();
		columnHeaderEpoch = new ColumnHeader();
		columnHeaderMeanAnomaly = new ColumnHeader();
		columnHeaderArgumentPerihelion = new ColumnHeader();
		columnHeaderLongitudeAscendingNode = new ColumnHeader();
		columnHeaderInclination = new ColumnHeader();
		columnHeaderOrbitalEccentricity = new ColumnHeader();
		columnHeaderMeanDailyMotion = new ColumnHeader();
		columnHeaderSemimajorAxis = new ColumnHeader();
		columnHeaderAbsoluteMagnitude = new ColumnHeader();
		columnHeaderSlopeParameter = new ColumnHeader();
		columnHeaderReference = new ColumnHeader();
		columnHeaderNumberOppositions = new ColumnHeader();
		columnHeaderNumberObservations = new ColumnHeader();
		columnHeaderObservationSpan = new ColumnHeader();
		columnHeaderRmsResidual = new ColumnHeader();
		columnHeaderComputerName = new ColumnHeader();
		columnHeaderFlags = new ColumnHeader();
		columnHeaderDateLastObservation = new ColumnHeader();
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
		toolStripMenuItemSaveAsYaml = new ToolStripMenuItem();
		toolStripMenuItemSaveAsToml = new ToolStripMenuItem();
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
		toolStripDropDownButtonSaveToFile = new ToolStripDropDownButton();
		backgroundWorker = new BackgroundWorker();
		kryptoPanelMain = new KryptonPanel();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);
		toolStripContainer = new ToolStripContainer();
		toolStripIcons = new ToolStrip();
		toolStripButtonList = new ToolStripButton();
		toolStripButtonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripLabelMinimum = new ToolStripLabel();
		toolStripNumericUpDownMinimum = new Planetoid_DB.Helpers.ToolStripNumericUpDown();
		toolStripLabelMaximum = new ToolStripLabel();
		toolStripNumericUpDownMaximum = new Planetoid_DB.Helpers.ToolStripNumericUpDown();
		toolStripSeparator2 = new ToolStripSeparator();
		toolStripButtonGoToObject = new ToolStripButton();
		toolStripSeparator4 = new ToolStripSeparator();
		toolStripSeparator3 = new ToolStripSeparator();
		toolStripLabelProgress = new ToolStripLabel();
		kryptonProgressBar = new KryptonProgressBarToolStripItem();
		contextMenuSaveToFile.SuspendLayout();
		((ISupportInitialize)kryptoPanelMain).BeginInit();
		kryptoPanelMain.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		toolStripIcons.SuspendLayout();
		SuspendLayout();
		// 
		// listView
		// 
		listView.AccessibleDescription = "Shows the list with the items";
		listView.AccessibleName = "List";
		listView.AccessibleRole = AccessibleRole.ListItem;
		listView.Activation = ItemActivation.OneClick;
		listView.AllowColumnReorder = true;
		listView.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderReadableDesignation, columnHeaderEpoch, columnHeaderMeanAnomaly, columnHeaderArgumentPerihelion, columnHeaderLongitudeAscendingNode, columnHeaderInclination, columnHeaderOrbitalEccentricity, columnHeaderMeanDailyMotion, columnHeaderSemimajorAxis, columnHeaderAbsoluteMagnitude, columnHeaderSlopeParameter, columnHeaderReference, columnHeaderNumberOppositions, columnHeaderNumberObservations, columnHeaderObservationSpan, columnHeaderRmsResidual, columnHeaderComputerName, columnHeaderFlags, columnHeaderDateLastObservation });
		listView.ContextMenuStrip = contextMenuSaveToFile;
		listView.Dock = DockStyle.Fill;
		listView.Font = new Font("Segoe UI", 8.5F);
		listView.FullRowSelect = true;
		listView.GridLines = true;
		listView.Location = new Point(0, 0);
		listView.MultiSelect = false;
		listView.Name = "listView";
		listView.ShowItemToolTips = true;
		listView.Size = new Size(945, 431);
		listView.TabIndex = 8;
		listView.UseCompatibleStateImageBehavior = false;
		listView.View = View.Details;
		listView.VirtualMode = true;
		listView.ColumnClick += ListView_ColumnClick;
		listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		listView.SelectedIndexChanged += ListViewTableMode_SelectedIndexChanged;
		listView.DoubleClick += ListView_DoubleClick;
		listView.Enter += Control_Enter;
		listView.Leave += Control_Leave;
		listView.MouseEnter += Control_Enter;
		listView.MouseLeave += Control_Leave;
		// 
		// columnHeaderIndex
		// 
		columnHeaderIndex.Text = "Index No.";
		// 
		// columnHeaderReadableDesignation
		// 
		columnHeaderReadableDesignation.Text = "Readable designation";
		columnHeaderReadableDesignation.Width = 120;
		// 
		// columnHeaderEpoch
		// 
		columnHeaderEpoch.Text = "Epoch";
		columnHeaderEpoch.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderMeanAnomaly
		// 
		columnHeaderMeanAnomaly.Text = "Mean anomaly at the epoch, in degrees";
		columnHeaderMeanAnomaly.TextAlign = HorizontalAlignment.Right;
		columnHeaderMeanAnomaly.Width = 70;
		// 
		// columnHeaderArgumentPerihelion
		// 
		columnHeaderArgumentPerihelion.Text = "Argument of perihelion, J2000.0 (degrees)";
		columnHeaderArgumentPerihelion.TextAlign = HorizontalAlignment.Right;
		columnHeaderArgumentPerihelion.Width = 70;
		// 
		// columnHeaderLongitudeAscendingNode
		// 
		columnHeaderLongitudeAscendingNode.Text = "Longitude of the ascending node";
		columnHeaderLongitudeAscendingNode.TextAlign = HorizontalAlignment.Right;
		columnHeaderLongitudeAscendingNode.Width = 70;
		// 
		// columnHeaderInclination
		// 
		columnHeaderInclination.Text = "Inclination";
		columnHeaderInclination.TextAlign = HorizontalAlignment.Right;
		// 
		// columnHeaderOrbitalEccentricity
		// 
		columnHeaderOrbitalEccentricity.Text = "Orbital eccentricity";
		columnHeaderOrbitalEccentricity.TextAlign = HorizontalAlignment.Right;
		columnHeaderOrbitalEccentricity.Width = 70;
		// 
		// columnHeaderMeanDailyMotion
		// 
		columnHeaderMeanDailyMotion.Text = "Mean daily motion";
		columnHeaderMeanDailyMotion.TextAlign = HorizontalAlignment.Right;
		columnHeaderMeanDailyMotion.Width = 70;
		// 
		// columnHeaderSemimajorAxis
		// 
		columnHeaderSemimajorAxis.Text = "Semimajor axis";
		columnHeaderSemimajorAxis.TextAlign = HorizontalAlignment.Right;
		columnHeaderSemimajorAxis.Width = 75;
		// 
		// columnHeaderAbsoluteMagnitude
		// 
		columnHeaderAbsoluteMagnitude.Text = "Absolute magnitude";
		columnHeaderAbsoluteMagnitude.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderSlopeParameter
		// 
		columnHeaderSlopeParameter.Text = "Slope parameter";
		columnHeaderSlopeParameter.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderReference
		// 
		columnHeaderReference.Text = "Reference";
		columnHeaderReference.TextAlign = HorizontalAlignment.Center;
		columnHeaderReference.Width = 80;
		// 
		// columnHeaderNumberOppositions
		// 
		columnHeaderNumberOppositions.Text = "Number of oppositions";
		columnHeaderNumberOppositions.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderNumberObservations
		// 
		columnHeaderNumberObservations.Text = "Number of observations";
		columnHeaderNumberObservations.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderObservationSpan
		// 
		columnHeaderObservationSpan.Text = "Observation span";
		columnHeaderObservationSpan.TextAlign = HorizontalAlignment.Center;
		columnHeaderObservationSpan.Width = 80;
		// 
		// columnHeaderRmsResidual
		// 
		columnHeaderRmsResidual.Text = "r.m.s. residual";
		columnHeaderRmsResidual.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderComputerName
		// 
		columnHeaderComputerName.Text = "Computer name";
		columnHeaderComputerName.TextAlign = HorizontalAlignment.Center;
		columnHeaderComputerName.Width = 80;
		// 
		// columnHeaderFlags
		// 
		columnHeaderFlags.Text = "4-hexdigit flags";
		columnHeaderFlags.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderDateLastObservation
		// 
		columnHeaderDateLastObservation.Text = "Date of last observation";
		columnHeaderDateLastObservation.TextAlign = HorizontalAlignment.Center;
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
		contextMenuSaveToFile.OwnerItem = toolStripDropDownButtonSaveToFile;
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
		toolStripMenuItemConfigurationFiles.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsJson, toolStripMenuItemSaveAsYaml, toolStripMenuItemSaveAsToml });
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
		// toolStripDropDownButtonSaveToFile
		// 
		toolStripDropDownButtonSaveToFile.AccessibleDescription = "Saves information to file";
		toolStripDropDownButtonSaveToFile.AccessibleName = "Save to file";
		toolStripDropDownButtonSaveToFile.AccessibleRole = AccessibleRole.ButtonDropDown;
		toolStripDropDownButtonSaveToFile.DropDown = contextMenuSaveToFile;
		toolStripDropDownButtonSaveToFile.Image = FatcowIcons16px.fatcow_diskette_16px;
		toolStripDropDownButtonSaveToFile.ImageTransparentColor = Color.Magenta;
		toolStripDropDownButtonSaveToFile.Name = "toolStripDropDownButtonSaveToFile";
		toolStripDropDownButtonSaveToFile.Size = new Size(93, 23);
		toolStripDropDownButtonSaveToFile.Text = "&Save to file";
		toolStripDropDownButtonSaveToFile.MouseEnter += Control_Enter;
		toolStripDropDownButtonSaveToFile.MouseLeave += Control_Leave;
		// 
		// backgroundWorker
		// 
		backgroundWorker.WorkerReportsProgress = true;
		backgroundWorker.WorkerSupportsCancellation = true;
		// 
		// kryptoPanelMain
		// 
		kryptoPanelMain.AccessibleDescription = "Groups the data";
		kryptoPanelMain.AccessibleName = "Panel";
		kryptoPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptoPanelMain.Controls.Add(listView);
		kryptoPanelMain.Dock = DockStyle.Fill;
		kryptoPanelMain.Location = new Point(0, 0);
		kryptoPanelMain.Name = "kryptoPanelMain";
		kryptoPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptoPanelMain.Size = new Size(945, 431);
		kryptoPanelMain.TabIndex = 0;
		kryptoPanelMain.TabStop = true;
		kryptoPanelMain.Text = "Main Panel";
		kryptoPanelMain.Enter += Control_Enter;
		kryptoPanelMain.Leave += Control_Leave;
		kryptoPanelMain.MouseEnter += Control_Enter;
		kryptoPanelMain.MouseLeave += Control_Leave;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar of some information";
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
		kryptonStatusStrip.Size = new Size(945, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 9;
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
		labelInformation.ToolTipText = "Shows some information";
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
		toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
		toolStripContainer.AccessibleName = "Container to arrange the toolbars";
		toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.Controls.Add(kryptoPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(945, 431);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(945, 479);
		toolStripContainer.TabIndex = 2;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStripIcons);
		toolStripContainer.Enter += Control_Enter;
		toolStripContainer.Leave += Control_Leave;
		toolStripContainer.MouseEnter += Control_Enter;
		toolStripContainer.MouseLeave += Control_Leave;
		// 
		// toolStripIcons
		// 
		toolStripIcons.AccessibleDescription = "Toolbar of saving";
		toolStripIcons.AccessibleName = "Toolbar of saving";
		toolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
		toolStripIcons.AllowClickThrough = true;
		toolStripIcons.AllowItemReorder = true;
		toolStripIcons.BackColor = Color.Transparent;
		toolStripIcons.Dock = DockStyle.None;
		toolStripIcons.Font = new Font("Segoe UI", 9F);
		toolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonList, toolStripButtonCancel, toolStripSeparator1, toolStripLabelMinimum, toolStripNumericUpDownMinimum, toolStripLabelMaximum, toolStripNumericUpDownMaximum, toolStripSeparator2, toolStripButtonGoToObject, toolStripSeparator4, toolStripDropDownButtonSaveToFile, toolStripSeparator3, toolStripLabelProgress, kryptonProgressBar });
		toolStripIcons.Location = new Point(0, 0);
		toolStripIcons.Name = "toolStripIcons";
		toolStripIcons.Size = new Size(945, 26);
		toolStripIcons.Stretch = true;
		toolStripIcons.TabIndex = 3;
		toolStripIcons.TabStop = true;
		toolStripIcons.Text = "Toolbar of saving";
		toolStripIcons.Enter += Control_Enter;
		toolStripIcons.Leave += Control_Leave;
		toolStripIcons.MouseEnter += Control_Enter;
		toolStripIcons.MouseLeave += Control_Leave;
		// 
		// toolStripButtonList
		// 
		toolStripButtonList.AccessibleDescription = "Starts the progress and list";
		toolStripButtonList.AccessibleName = "List";
		toolStripButtonList.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonList.Image = FatcowIcons16px.fatcow_table_16px;
		toolStripButtonList.ImageTransparentColor = Color.Magenta;
		toolStripButtonList.Name = "toolStripButtonList";
		toolStripButtonList.Size = new Size(45, 23);
		toolStripButtonList.Text = "&List";
		toolStripButtonList.Click += ToolStripButtonList_ClickAsync;
		toolStripButtonList.MouseEnter += Control_Enter;
		toolStripButtonList.MouseLeave += Control_Leave;
		// 
		// toolStripButtonCancel
		// 
		toolStripButtonCancel.AccessibleDescription = "Cancels the progress";
		toolStripButtonCancel.AccessibleName = "Cancel";
		toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
		toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
		toolStripButtonCancel.Name = "toolStripButtonCancel";
		toolStripButtonCancel.Size = new Size(63, 23);
		toolStripButtonCancel.Text = "&Cancel";
		toolStripButtonCancel.Click += ButtonCancel_Click;
		toolStripButtonCancel.MouseEnter += Control_Enter;
		toolStripButtonCancel.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator1
		// 
		toolStripSeparator1.AccessibleDescription = "Just a separator";
		toolStripSeparator1.AccessibleName = "Just a separator";
		toolStripSeparator1.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(6, 26);
		toolStripSeparator1.MouseEnter += Control_Enter;
		toolStripSeparator1.MouseLeave += Control_Leave;
		// 
		// toolStripLabelMinimum
		// 
		toolStripLabelMinimum.AccessibleDescription = "Shows the minimum";
		toolStripLabelMinimum.AccessibleName = "Minimum";
		toolStripLabelMinimum.AutoToolTip = true;
		toolStripLabelMinimum.Name = "toolStripLabelMinimum";
		toolStripLabelMinimum.Size = new Size(60, 23);
		toolStripLabelMinimum.Text = "M&inimum";
		toolStripLabelMinimum.MouseEnter += Control_Enter;
		toolStripLabelMinimum.MouseLeave += Control_Leave;
		// 
		// toolStripNumericUpDownMinimum
		// 
		toolStripNumericUpDownMinimum.AccessibleDescription = "Shows the minimum value for the list";
		toolStripNumericUpDownMinimum.AccessibleName = "Minimum value for the list";
		toolStripNumericUpDownMinimum.AutoSize = true;
		toolStripNumericUpDownMinimum.Name = "toolStripNumericUpDownMinimum";
		toolStripNumericUpDownMinimum.Size = new Size(41, 23);
		toolStripNumericUpDownMinimum.Text = "0";
		toolStripNumericUpDownMinimum.TextAlign = HorizontalAlignment.Center;
		toolStripNumericUpDownMinimum.MouseEnter += Control_Enter;
		toolStripNumericUpDownMinimum.MouseLeave += Control_Leave;
		// 
		// toolStripLabelMaximum
		// 
		toolStripLabelMaximum.AccessibleDescription = "Shows the maximum";
		toolStripLabelMaximum.AccessibleName = "Maximum";
		toolStripLabelMaximum.AutoToolTip = true;
		toolStripLabelMaximum.Name = "toolStripLabelMaximum";
		toolStripLabelMaximum.Size = new Size(62, 23);
		toolStripLabelMaximum.Text = "M&aximum";
		toolStripLabelMaximum.MouseEnter += Control_Enter;
		toolStripLabelMaximum.MouseLeave += Control_Leave;
		// 
		// toolStripNumericUpDownMaximum
		// 
		toolStripNumericUpDownMaximum.AccessibleDescription = "Shows the maximum value for the list";
		toolStripNumericUpDownMaximum.AccessibleName = "Maximum value for the list";
		toolStripNumericUpDownMaximum.AutoSize = true;
		toolStripNumericUpDownMaximum.Name = "toolStripNumericUpDownMaximum";
		toolStripNumericUpDownMaximum.Size = new Size(41, 23);
		toolStripNumericUpDownMaximum.Text = "0";
		toolStripNumericUpDownMaximum.TextAlign = HorizontalAlignment.Center;
		toolStripNumericUpDownMaximum.MouseEnter += Control_Enter;
		toolStripNumericUpDownMaximum.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator2
		// 
		toolStripSeparator2.AccessibleDescription = "Just a separator";
		toolStripSeparator2.AccessibleName = "Just a separator";
		toolStripSeparator2.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(6, 26);
		toolStripSeparator2.MouseEnter += Control_Enter;
		toolStripSeparator2.MouseLeave += Control_Leave;
		// 
		// toolStripButtonGoToObject
		// 
		toolStripButtonGoToObject.AccessibleDescription = "Navigates to the selected planetoid in the main form and closes this form";
		toolStripButtonGoToObject.AccessibleName = "Go to object";
		toolStripButtonGoToObject.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonGoToObject.Enabled = false;
		toolStripButtonGoToObject.Image = FatcowIcons16px.fatcow_application_go_16px;
		toolStripButtonGoToObject.ImageTransparentColor = Color.Magenta;
		toolStripButtonGoToObject.Name = "toolStripButtonGoToObject";
		toolStripButtonGoToObject.Size = new Size(92, 23);
		toolStripButtonGoToObject.Text = "&Go to object";
		toolStripButtonGoToObject.Click += ToolStripButtonGoToObject_Click;
		toolStripButtonGoToObject.MouseEnter += Control_Enter;
		toolStripButtonGoToObject.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator4
		// 
		toolStripSeparator4.AccessibleDescription = "Just a separator";
		toolStripSeparator4.AccessibleName = "Just a separator";
		toolStripSeparator4.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator4.Name = "toolStripSeparator4";
		toolStripSeparator4.Size = new Size(6, 26);
		toolStripSeparator4.MouseEnter += Control_Enter;
		toolStripSeparator4.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator3
		// 
		toolStripSeparator3.AccessibleDescription = "Just a separator";
		toolStripSeparator3.AccessibleName = "Just a separator";
		toolStripSeparator3.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator3.Name = "toolStripSeparator3";
		toolStripSeparator3.Size = new Size(6, 26);
		toolStripSeparator3.MouseEnter += Control_Enter;
		toolStripSeparator3.MouseLeave += Control_Leave;
		// 
		// toolStripLabelProgress
		// 
		toolStripLabelProgress.AccessibleDescription = "Shows the progress of comparing";
		toolStripLabelProgress.AccessibleName = "Progress";
		toolStripLabelProgress.AccessibleRole = AccessibleRole.StaticText;
		toolStripLabelProgress.AutoToolTip = true;
		toolStripLabelProgress.Name = "toolStripLabelProgress";
		toolStripLabelProgress.Size = new Size(52, 23);
		toolStripLabelProgress.Text = "Pro&gress";
		toolStripLabelProgress.MouseEnter += Control_Enter;
		toolStripLabelProgress.MouseLeave += Control_Leave;
		// 
		// kryptonProgressBar
		// 
		kryptonProgressBar.AccessibleDescription = "Shows the progress";
		kryptonProgressBar.AccessibleName = "Progress";
		kryptonProgressBar.AccessibleRole = AccessibleRole.ProgressBar;
		kryptonProgressBar.AutoToolTip = true;
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(300, 23);
		kryptonProgressBar.StateCommon.Back.Color1 = Color.Green;
		kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.Values.Text = "";
		kryptonProgressBar.MouseEnter += Control_Enter;
		kryptonProgressBar.MouseLeave += Control_Leave;
		// 
		// TableModeForm
		// 
		AccessibleDescription = "Lists the MPCORB.DAT into a  table";
		AccessibleName = "Table Mode";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(945, 479);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "TableModeForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Table Mode";
		FormClosing += TableModeForm_FormClosing;
		FormClosed += TableModeForm_FormClosed;
		Load += TableModeForm_Load;
		contextMenuSaveToFile.ResumeLayout(false);
		((ISupportInitialize)kryptoPanelMain).EndInit();
		kryptoPanelMain.ResumeLayout(false);
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		toolStripIcons.ResumeLayout(false);
		toolStripIcons.PerformLayout();
		ResumeLayout(false);
	}

	#endregion
	private ListView listView;
    private ColumnHeader columnHeaderIndex;
    private ColumnHeader columnHeaderReadableDesignation;
    private ColumnHeader columnHeaderEpoch;
    private ColumnHeader columnHeaderMeanAnomaly;
    private ColumnHeader columnHeaderArgumentPerihelion;
    private ColumnHeader columnHeaderLongitudeAscendingNode;
    private ColumnHeader columnHeaderInclination;
    private ColumnHeader columnHeaderOrbitalEccentricity;
    private ColumnHeader columnHeaderMeanDailyMotion;
    private ColumnHeader columnHeaderSemimajorAxis;
    private ColumnHeader columnHeaderAbsoluteMagnitude;
    private ColumnHeader columnHeaderSlopeParameter;
    private ColumnHeader columnHeaderReference;
    private ColumnHeader columnHeaderNumberOppositions;
    private ColumnHeader columnHeaderNumberObservations;
    private ColumnHeader columnHeaderObservationSpan;
    private ColumnHeader columnHeaderRmsResidual;
    private ColumnHeader columnHeaderComputerName;
    private ColumnHeader columnHeaderFlags;
    private ColumnHeader columnHeaderDateLastObservation;
    private BackgroundWorker backgroundWorker;
        private KryptonPanel kryptoPanelMain;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
	private ToolStripContainer toolStripContainer;
	private ToolStrip toolStripIcons;
	private ToolStripDropDownButton toolStripDropDownButtonSaveToFile;
	private ToolStripLabel toolStripLabelMinimum;
	private Helpers.ToolStripNumericUpDown toolStripNumericUpDownMinimum;
	private ToolStripLabel toolStripLabelMaximum;
	private Helpers.ToolStripNumericUpDown toolStripNumericUpDownMaximum;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripButton toolStripButtonList;
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripSeparator toolStripSeparator3;
	private ToolStripSeparator toolStripSeparator4;
	private ToolStripButton toolStripButtonGoToObject;
	private ToolStripLabel toolStripLabelProgress;
	private KryptonProgressBarToolStripItem kryptonProgressBar;
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
	private ToolStripMenuItem toolStripMenuItemSaveAsYaml;
	private ToolStripMenuItem toolStripMenuItemSaveAsToml;
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
}