// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

namespace Planetoid_DB;

/// <summary>
/// Represents a form that provides a user interface for searching within the MPCORB.DAT file.
/// </summary>
/// <remarks>The form enables users to input search criteria, select orbital elements, and view search results. It
/// includes options for full-text search and displays results in a list format. Use this form to facilitate advanced
/// and customizable searches of the MPCORB.DAT dataset.</remarks>
partial class Search2Form
    {
        /// <summary>Required designer variable.</summary>
		/// <remarks>This field stores the components used by the form.</remarks>
        private System.ComponentModel.IContainer components = null;

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
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search2Form));
		kryptonPanelMain = new KryptonPanel();
		splitContainer = new SplitContainer();
		kryptonCheckedListBoxElements = new KryptonCheckedListBox();
		contextMenuStripMark = new ContextMenuStrip(components);
		toolStripMenuItemMarkAll = new ToolStripMenuItem();
		toolStripMenuItemUnmarkAll = new ToolStripMenuItem();
		listViewResults = new ListView();
		columnHeaderIndex = new ColumnHeader();
		columnHeaderDesignation = new ColumnHeader();
		columnHeaderElement = new ColumnHeader();
		columnHeaderValue = new ColumnHeader();
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
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanel = new KryptonPanel();
		kryptonToolStripIcons = new KryptonToolStrip();
		toolStripLabelSearch = new ToolStripLabel();
		toolStripTextBoxSearch = new ToolStripTextBox();
		toolStripButtonFullText = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		toolStripButtonSearch = new ToolStripButton();
		toolStripButtonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripLabelProgress = new ToolStripLabel();
		kryptonProgressBar = new KryptonProgressBarToolStripItem();
		toolStripSeparator3 = new ToolStripSeparator();
		toolStripButtonGoToObject = new ToolStripButton();
		((System.ComponentModel.ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
		splitContainer.Panel1.SuspendLayout();
		splitContainer.Panel2.SuspendLayout();
		splitContainer.SuspendLayout();
		contextMenuStripMark.SuspendLayout();
		contextMenuSaveToFile.SuspendLayout();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)kryptonPanel).BeginInit();
		kryptonPanel.SuspendLayout();
		kryptonToolStripIcons.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonPanelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Groups the data";
		kryptonPanelMain.AccessibleName = "Panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(splitContainer);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.Size = new Size(851, 426);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "Main panel";
		kryptonPanelMain.Enter += Control_Enter;
		kryptonPanelMain.Leave += Control_Leave;
		kryptonPanelMain.MouseEnter += Control_Enter;
		kryptonPanelMain.MouseLeave += Control_Leave;
		// 
		// splitContainer
		// 
		splitContainer.AccessibleDescription = "Splits the content of the dialog";
		splitContainer.AccessibleName = "Content splitter";
		splitContainer.AccessibleRole = AccessibleRole.Grouping;
		splitContainer.Dock = DockStyle.Fill;
		splitContainer.FixedPanel = FixedPanel.Panel1;
		splitContainer.Location = new Point(0, 0);
		splitContainer.Name = "splitContainer";
		// 
		// splitContainer.Panel1
		// 
		splitContainer.Panel1.Controls.Add(kryptonCheckedListBoxElements);
		// 
		// splitContainer.Panel2
		// 
		splitContainer.Panel2.Controls.Add(listViewResults);
		splitContainer.Size = new Size(851, 426);
		splitContainer.SplitterDistance = 207;
		splitContainer.TabIndex = 8;
		splitContainer.Enter += Control_Enter;
		splitContainer.Leave += Control_Leave;
		splitContainer.MouseEnter += Control_Enter;
		splitContainer.MouseLeave += Control_Leave;
		// 
		// kryptonCheckedListBoxElements
		// 
		kryptonCheckedListBoxElements.AccessibleDescription = "Shows the list of selectable elements";
		kryptonCheckedListBoxElements.AccessibleName = "List of selectable elements";
		kryptonCheckedListBoxElements.AccessibleRole = AccessibleRole.List;
		kryptonCheckedListBoxElements.CheckOnClick = true;
		kryptonCheckedListBoxElements.ContextMenuStrip = contextMenuStripMark;
		kryptonCheckedListBoxElements.Dock = DockStyle.Fill;
		kryptonCheckedListBoxElements.HorizontalScrollbar = true;
		kryptonCheckedListBoxElements.Location = new Point(0, 0);
		kryptonCheckedListBoxElements.Name = "kryptonCheckedListBoxElements";
		kryptonCheckedListBoxElements.Size = new Size(207, 426);
		kryptonCheckedListBoxElements.TabIndex = 0;
		kryptonCheckedListBoxElements.ToolTipValues.Description = "Shows the list of selectable elements";
		kryptonCheckedListBoxElements.ToolTipValues.EnableToolTips = true;
		kryptonCheckedListBoxElements.ToolTipValues.Heading = "List of selectable elements";
		kryptonCheckedListBoxElements.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		kryptonCheckedListBoxElements.Enter += Control_Enter;
		kryptonCheckedListBoxElements.Leave += Control_Leave;
		kryptonCheckedListBoxElements.MouseEnter += Control_Enter;
		kryptonCheckedListBoxElements.MouseLeave += Control_Leave;
		// 
		// contextMenuStripMark
		// 
		contextMenuStripMark.AllowClickThrough = true;
		contextMenuStripMark.Font = new Font("Segoe UI", 9F);
		contextMenuStripMark.Items.AddRange(new ToolStripItem[] { toolStripMenuItemMarkAll, toolStripMenuItemUnmarkAll });
		contextMenuStripMark.Name = "contextMenuStripMark";
		contextMenuStripMark.Size = new Size(132, 48);
		contextMenuStripMark.TabStop = true;
		contextMenuStripMark.Enter += Control_Enter;
		contextMenuStripMark.Leave += Control_Leave;
		contextMenuStripMark.MouseEnter += Control_Enter;
		contextMenuStripMark.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemMarkAll
		// 
		toolStripMenuItemMarkAll.AccessibleDescription = "Marks all selectable elements";
		toolStripMenuItemMarkAll.AccessibleName = "Mark all";
		toolStripMenuItemMarkAll.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemMarkAll.AutoToolTip = true;
		toolStripMenuItemMarkAll.Image = Resources.FatcowIcons16px.fatcow_check_box_16px;
		toolStripMenuItemMarkAll.Name = "toolStripMenuItemMarkAll";
		toolStripMenuItemMarkAll.Size = new Size(131, 22);
		toolStripMenuItemMarkAll.Text = "&Mark all";
		toolStripMenuItemMarkAll.Click += ToolStripMenuItemMarkAll_Click;
		toolStripMenuItemMarkAll.MouseEnter += Control_Enter;
		toolStripMenuItemMarkAll.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemUnmarkAll
		// 
		toolStripMenuItemUnmarkAll.AccessibleDescription = "Unmarks all selectable elements";
		toolStripMenuItemUnmarkAll.AccessibleName = "Unmark all";
		toolStripMenuItemUnmarkAll.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemUnmarkAll.AutoToolTip = true;
		toolStripMenuItemUnmarkAll.Image = Resources.FatcowIcons16px.fatcow_check_box_uncheck_16px;
		toolStripMenuItemUnmarkAll.Name = "toolStripMenuItemUnmarkAll";
		toolStripMenuItemUnmarkAll.Size = new Size(131, 22);
		toolStripMenuItemUnmarkAll.Text = "&Unmark all";
		toolStripMenuItemUnmarkAll.Click += ToolStripMenuItemUnmarkAll_Click;
		toolStripMenuItemUnmarkAll.MouseEnter += Control_Enter;
		toolStripMenuItemUnmarkAll.MouseLeave += Control_Leave;
		// 
		// listViewResults
		// 
		listViewResults.AccessibleDescription = "Lists the results";
		listViewResults.AccessibleName = "Results";
		listViewResults.AccessibleRole = AccessibleRole.List;
		listViewResults.AllowColumnReorder = true;
		listViewResults.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderDesignation, columnHeaderElement, columnHeaderValue });
		listViewResults.ContextMenuStrip = contextMenuSaveToFile;
		listViewResults.Dock = DockStyle.Fill;
		listViewResults.FullRowSelect = true;
		listViewResults.GridLines = true;
		listViewResults.Location = new Point(0, 0);
		listViewResults.MultiSelect = false;
		listViewResults.Name = "listViewResults";
		listViewResults.ShowItemToolTips = true;
		listViewResults.Size = new Size(640, 426);
		listViewResults.TabIndex = 0;
		listViewResults.UseCompatibleStateImageBehavior = false;
		listViewResults.View = View.Details;
		listViewResults.VirtualMode = true;
		listViewResults.ColumnClick += ListView_ColumnClick;
		listViewResults.RetrieveVirtualItem += ListViewResults_RetrieveVirtualItem;
		listViewResults.SelectedIndexChanged += ListViewResults_SelectedIndexChanged;
		listViewResults.DoubleClick += ListViewResults_DoubleClick;
		listViewResults.Enter += Control_Enter;
		listViewResults.Leave += Control_Leave;
		listViewResults.MouseEnter += Control_Enter;
		listViewResults.MouseLeave += Control_Leave;
		// 
		// columnHeaderIndex
		// 
		columnHeaderIndex.Text = "Index No.";
		columnHeaderIndex.Width = 90;
		// 
		// columnHeaderDesignation
		// 
		columnHeaderDesignation.Text = "Readable designation";
		columnHeaderDesignation.Width = 150;
		// 
		// columnHeaderElement
		// 
		columnHeaderElement.Text = "Orbital Element";
		columnHeaderElement.Width = 200;
		// 
		// columnHeaderValue
		// 
		columnHeaderValue.Text = "Value";
		columnHeaderValue.Width = 170;
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
		toolStripMenuItemTextFiles.Image = Resources.FatcowIcons16px.fatcow_file_extension_txt_16px;
		toolStripMenuItemTextFiles.Name = "toolStripMenuItemTextFiles";
		toolStripMenuItemTextFiles.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsText.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsText.Name = "toolStripMenuItemSaveAsText";
		toolStripMenuItemSaveAsText.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsLatex.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsLatex.Name = "toolStripMenuItemSaveAsLatex";
		toolStripMenuItemSaveAsLatex.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsMarkdown.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsMarkdown.Name = "toolStripMenuItemSaveAsMarkdown";
		toolStripMenuItemSaveAsMarkdown.ShortcutKeyDisplayString = "";
		toolStripMenuItemSaveAsMarkdown.Size = new Size(201, 22);
		toolStripMenuItemSaveAsMarkdown.Text = "Save as &Markdown";
		toolStripMenuItemSaveAsMarkdown.Click += ToolStripMenuItemSaveAsMarkdown_Click;
		toolStripMenuItemSaveAsMarkdown.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsMarkdown.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsAsciiDoc
		// 
		toolStripMenuItemSaveAsAsciiDoc.AccessibleDescription = "Save list as AsciiDoc file";
		toolStripMenuItemSaveAsAsciiDoc.AccessibleName = "Save as AsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsAsciiDoc.AutoToolTip = true;
		toolStripMenuItemSaveAsAsciiDoc.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsAsciiDoc.Name = "toolStripMenuItemSaveAsAsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.Size = new Size(201, 22);
		toolStripMenuItemSaveAsAsciiDoc.Text = "Save as &AsciiDoc";
		toolStripMenuItemSaveAsAsciiDoc.Click += ToolStripMenuItemSaveAsAsciiDoc_Click;
		toolStripMenuItemSaveAsAsciiDoc.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsAsciiDoc.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsReStructuredText
		// 
		toolStripMenuItemSaveAsReStructuredText.AccessibleDescription = "Save list as reStructuredText file";
		toolStripMenuItemSaveAsReStructuredText.AccessibleName = "Save as reStructuredText";
		toolStripMenuItemSaveAsReStructuredText.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsReStructuredText.AutoToolTip = true;
		toolStripMenuItemSaveAsReStructuredText.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		toolStripMenuItemSaveAsReStructuredText.Name = "toolStripMenuItemSaveAsReStructuredText";
		toolStripMenuItemSaveAsReStructuredText.Size = new Size(201, 22);
		toolStripMenuItemSaveAsReStructuredText.Text = "Save as &reStructuredText";
		toolStripMenuItemSaveAsReStructuredText.Click += ToolStripMenuItemSaveAsReStructuredText_Click;
		toolStripMenuItemSaveAsReStructuredText.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsReStructuredText.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsTextile
		// 
		toolStripMenuItemSaveAsTextile.AccessibleDescription = "Save list as Textile file";
		toolStripMenuItemSaveAsTextile.AccessibleName = "Save as Textile";
		toolStripMenuItemSaveAsTextile.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsTextile.AutoToolTip = true;
		toolStripMenuItemSaveAsTextile.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
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
		toolStripMenuItemWriterDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_doc_16px;
		toolStripMenuItemWriterDocuments.Name = "toolStripMenuItemWriterDocuments";
		toolStripMenuItemWriterDocuments.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsWord.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsWord.Name = "toolStripMenuItemSaveAsWord";
		toolStripMenuItemSaveAsWord.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsOdt.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsOdt.Name = "toolStripMenuItemSaveAsOdt";
		toolStripMenuItemSaveAsOdt.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsRtf.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		toolStripMenuItemSaveAsRtf.Name = "toolStripMenuItemSaveAsRtf";
		toolStripMenuItemSaveAsRtf.ShortcutKeyDisplayString = "";
		toolStripMenuItemSaveAsRtf.Size = new Size(257, 22);
		toolStripMenuItemSaveAsRtf.Text = "Save as &Rich Text Format (RTF)";
		toolStripMenuItemSaveAsRtf.Click += ToolStripMenuItemSaveAsRtf_Click;
		toolStripMenuItemSaveAsRtf.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsRtf.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsAbiword
		// 
		toolStripMenuItemSaveAsAbiword.AccessibleDescription = "Saves the list as Abiword file";
		toolStripMenuItemSaveAsAbiword.AccessibleName = "Save as Abiword file";
		toolStripMenuItemSaveAsAbiword.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsAbiword.AutoToolTip = true;
		toolStripMenuItemSaveAsAbiword.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
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
		toolStripMenuItemSaveAsWps.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
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
		toolStripMenuItemSpreadsheetDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_xls_16px;
		toolStripMenuItemSpreadsheetDocuments.Name = "toolStripMenuItemSpreadsheetDocuments";
		toolStripMenuItemSpreadsheetDocuments.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsExcel.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsExcel.Name = "toolStripMenuItemSaveAsExcel";
		toolStripMenuItemSaveAsExcel.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsOds.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsOds.Name = "toolStripMenuItemSaveAsOds";
		toolStripMenuItemSaveAsOds.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsCsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsCsv.Name = "toolStripMenuItemSaveAsCsv";
		toolStripMenuItemSaveAsCsv.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsTsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsTsv.Name = "toolStripMenuItemSaveAsTsv";
		toolStripMenuItemSaveAsTsv.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsPsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		toolStripMenuItemSaveAsPsv.Name = "toolStripMenuItemSaveAsPsv";
		toolStripMenuItemSaveAsPsv.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsEt.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
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
		toolStripMenuItemXmlDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_bin_16px;
		toolStripMenuItemXmlDocuments.Name = "toolStripMenuItemXmlDocuments";
		toolStripMenuItemXmlDocuments.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsHtml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		toolStripMenuItemSaveAsHtml.Name = "toolStripMenuItemSaveAsHtml";
		toolStripMenuItemSaveAsHtml.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsXml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		toolStripMenuItemSaveAsXml.Name = "toolStripMenuItemSaveAsXml";
		toolStripMenuItemSaveAsXml.ShortcutKeyDisplayString = "";
		toolStripMenuItemSaveAsXml.Size = new Size(163, 22);
		toolStripMenuItemSaveAsXml.Text = "Save as &XML";
		toolStripMenuItemSaveAsXml.Click += ToolStripMenuItemSaveAsXml_Click;
		toolStripMenuItemSaveAsXml.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsXml.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemSaveAsDocBook
		// 
		toolStripMenuItemSaveAsDocBook.AccessibleDescription = "Saves the list as DocBook";
		toolStripMenuItemSaveAsDocBook.AccessibleName = "Save as DocBook";
		toolStripMenuItemSaveAsDocBook.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemSaveAsDocBook.AutoToolTip = true;
		toolStripMenuItemSaveAsDocBook.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
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
		toolStripMenuItemConfigurationFiles.Image = Resources.FatcowIcons16px.fatcow_file_extension_dll_16px;
		toolStripMenuItemConfigurationFiles.Name = "toolStripMenuItemConfigurationFiles";
		toolStripMenuItemConfigurationFiles.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsJson.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		toolStripMenuItemSaveAsJson.Name = "toolStripMenuItemSaveAsJson";
		toolStripMenuItemSaveAsJson.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsYaml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		toolStripMenuItemSaveAsYaml.Name = "toolStripMenuItemSaveAsYaml";
		toolStripMenuItemSaveAsYaml.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsToml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
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
		toolStripMenuItemDatabaseScripts.Image = Resources.FatcowIcons16px.fatcow_file_extension_ptb_16px;
		toolStripMenuItemDatabaseScripts.Name = "toolStripMenuItemDatabaseScripts";
		toolStripMenuItemDatabaseScripts.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsSql.Image = Resources.FatcowIcons16px.fatcow_page_white_database_16px;
		toolStripMenuItemSaveAsSql.Name = "toolStripMenuItemSaveAsSql";
		toolStripMenuItemSaveAsSql.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsSqlite.Image = Resources.FatcowIcons16px.fatcow_page_white_database_16px;
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
		toolStripMenuItemPortableDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_pdf_16px;
		toolStripMenuItemPortableDocuments.Name = "toolStripMenuItemPortableDocuments";
		toolStripMenuItemPortableDocuments.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsPdf.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsPdf.Name = "toolStripMenuItemSaveAsPdf";
		toolStripMenuItemSaveAsPdf.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsPostScript.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsPostScript.Name = "toolStripMenuItemSaveAsPostScript";
		toolStripMenuItemSaveAsPostScript.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsEpub.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsEpub.Name = "toolStripMenuItemSaveAsEpub";
		toolStripMenuItemSaveAsEpub.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsMobi.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsMobi.Name = "toolStripMenuItemSaveAsMobi";
		toolStripMenuItemSaveAsMobi.ShortcutKeyDisplayString = "";
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
		toolStripMenuItemSaveAsXps.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
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
		toolStripMenuItemSaveAsFictionBook2.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
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
		toolStripMenuItemSaveAsChm.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		toolStripMenuItemSaveAsChm.Name = "toolStripMenuItemSaveAsChm";
		toolStripMenuItemSaveAsChm.Size = new Size(214, 22);
		toolStripMenuItemSaveAsChm.Text = "Save as &CHM";
		toolStripMenuItemSaveAsChm.Click += ToolStripMenuItemSaveAsChm_Click;
		toolStripMenuItemSaveAsChm.MouseEnter += Control_Enter;
		toolStripMenuItemSaveAsChm.MouseLeave += Control_Leave;
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
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanel);
		toolStripContainer.ContentPanel.Size = new Size(851, 426);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(851, 473);
		toolStripContainer.TabIndex = 3;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
		toolStripContainer.Enter += Control_Enter;
		toolStripContainer.Leave += Control_Leave;
		toolStripContainer.MouseEnter += Control_Enter;
		toolStripContainer.MouseLeave += Control_Leave;
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
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(851, 22);
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
		labelInformation.AccessibleName = "Show some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;
		// 
		// kryptonPanel
		// 
		kryptonPanel.AccessibleDescription = "Groups the data";
		kryptonPanel.AccessibleName = "Panel";
		kryptonPanel.AccessibleRole = AccessibleRole.Pane;
		kryptonPanel.Controls.Add(kryptonPanelMain);
		kryptonPanel.Dock = DockStyle.Fill;
		kryptonPanel.Location = new Point(0, 0);
		kryptonPanel.Name = "kryptonPanel";
		kryptonPanel.Size = new Size(851, 426);
		kryptonPanel.TabIndex = 0;
		kryptonPanel.TabStop = true;
		kryptonPanel.Text = "Panel";
		kryptonPanel.Enter += Control_Enter;
		kryptonPanel.Leave += Control_Leave;
		kryptonPanel.MouseEnter += Control_Enter;
		kryptonPanel.MouseLeave += Control_Leave;
		// 
		// kryptonToolStripIcons
		// 
		kryptonToolStripIcons.AccessibleDescription = "Toolbar of comparing";
		kryptonToolStripIcons.AccessibleName = "Toolbar of comparing";
		kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStripIcons.AllowClickThrough = true;
		kryptonToolStripIcons.AllowItemReorder = true;
		kryptonToolStripIcons.Dock = DockStyle.None;
		kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
		kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripLabelSearch, toolStripTextBoxSearch, toolStripButtonFullText, toolStripSeparator2, toolStripButtonSearch, toolStripButtonCancel, toolStripSeparator1, toolStripLabelProgress, kryptonProgressBar, toolStripSeparator3, toolStripButtonGoToObject });
		kryptonToolStripIcons.Location = new Point(0, 0);
		kryptonToolStripIcons.Name = "kryptonToolStripIcons";
		kryptonToolStripIcons.Size = new Size(851, 25);
		kryptonToolStripIcons.Stretch = true;
		kryptonToolStripIcons.TabIndex = 0;
		kryptonToolStripIcons.TabStop = true;
		kryptonToolStripIcons.Enter += Control_Enter;
		kryptonToolStripIcons.Leave += Control_Leave;
		kryptonToolStripIcons.MouseEnter += Control_Enter;
		kryptonToolStripIcons.MouseLeave += Control_Leave;
		// 
		// toolStripLabelSearch
		// 
		toolStripLabelSearch.AccessibleDescription = "Indicates the search term";
		toolStripLabelSearch.AccessibleName = "Search term";
		toolStripLabelSearch.AccessibleRole = AccessibleRole.StaticText;
		toolStripLabelSearch.AutoToolTip = true;
		toolStripLabelSearch.Name = "toolStripLabelSearch";
		toolStripLabelSearch.Size = new Size(70, 22);
		toolStripLabelSearch.Text = "Search &term";
		toolStripLabelSearch.MouseEnter += Control_Enter;
		toolStripLabelSearch.MouseLeave += Control_Leave;
		// 
		// toolStripTextBoxSearch
		// 
		toolStripTextBoxSearch.AccessibleDescription = "Contains the search term";
		toolStripTextBoxSearch.AccessibleName = "Search term";
		toolStripTextBoxSearch.AccessibleRole = AccessibleRole.Text;
		toolStripTextBoxSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		toolStripTextBoxSearch.AutoCompleteSource = AutoCompleteSource.AllSystemSources;
		toolStripTextBoxSearch.AutoToolTip = true;
		toolStripTextBoxSearch.Name = "toolStripTextBoxSearch";
		toolStripTextBoxSearch.Size = new Size(150, 25);
		toolStripTextBoxSearch.MouseEnter += Control_Enter;
		toolStripTextBoxSearch.MouseLeave += Control_Leave;
		// 
		// toolStripButtonFullText
		// 
		toolStripButtonFullText.AccessibleDescription = "Checks if only full text is searchable";
		toolStripButtonFullText.AccessibleName = "Full text only";
		toolStripButtonFullText.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonFullText.CheckOnClick = true;
		toolStripButtonFullText.Image = Resources.FatcowIcons16px.fatcow_check_boxes_16px;
		toolStripButtonFullText.ImageTransparentColor = Color.Magenta;
		toolStripButtonFullText.Name = "toolStripButtonFullText";
		toolStripButtonFullText.Size = new Size(95, 22);
		toolStripButtonFullText.Text = "&Full text only";
		toolStripButtonFullText.MouseEnter += Control_Enter;
		toolStripButtonFullText.MouseLeave += Control_Leave;
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
		// toolStripButtonSearch
		// 
		toolStripButtonSearch.AccessibleDescription = "Searches the term";
		toolStripButtonSearch.AccessibleName = "Search";
		toolStripButtonSearch.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonSearch.Image = Resources.FatcowIcons16px.fatcow_zoom_16px;
		toolStripButtonSearch.ImageTransparentColor = Color.Magenta;
		toolStripButtonSearch.Name = "toolStripButtonSearch";
		toolStripButtonSearch.Size = new Size(62, 22);
		toolStripButtonSearch.Text = "&Search";
		toolStripButtonSearch.Click += KryptonButtonSearch_Click;
		toolStripButtonSearch.MouseEnter += Control_Enter;
		toolStripButtonSearch.MouseLeave += Control_Leave;
		// 
		// toolStripButtonCancel
		// 
		toolStripButtonCancel.AccessibleDescription = "Cancels the search";
		toolStripButtonCancel.AccessibleName = "Cancel";
		toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonCancel.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
		toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
		toolStripButtonCancel.Name = "toolStripButtonCancel";
		toolStripButtonCancel.Size = new Size(63, 22);
		toolStripButtonCancel.Text = "C&ancel";
		toolStripButtonCancel.Click += KryptonButtonCancel_Click;
		toolStripButtonCancel.MouseEnter += Control_Enter;
		toolStripButtonCancel.MouseLeave += Control_Leave;
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
		// toolStripLabelProgress
		// 
		toolStripLabelProgress.AccessibleDescription = "Shows the progress of the search";
		toolStripLabelProgress.AccessibleName = "Progress";
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
		kryptonProgressBar.AccessibleDescription = "Shows the progress of the search";
		kryptonProgressBar.AccessibleName = "Comparison progress";
		kryptonProgressBar.AccessibleRole = AccessibleRole.ProgressBar;
		kryptonProgressBar.AutoToolTip = true;
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(220, 22);
		kryptonProgressBar.StateCommon.Back.Color1 = Color.Green;
		kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.Values.Text = "";
		kryptonProgressBar.Enter += Control_Enter;
		kryptonProgressBar.Leave += Control_Leave;
		kryptonProgressBar.MouseEnter += Control_Enter;
		kryptonProgressBar.MouseLeave += Control_Leave;
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
		// toolStripButtonGoToObject
		// 
		toolStripButtonGoToObject.AccessibleDescription = "Goes to the selected planetoid";
		toolStripButtonGoToObject.AccessibleName = "Go to object";
		toolStripButtonGoToObject.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonGoToObject.Enabled = false;
		toolStripButtonGoToObject.Image = Resources.FatcowIcons16px.fatcow_application_go_16px;
		toolStripButtonGoToObject.ImageTransparentColor = Color.Magenta;
		toolStripButtonGoToObject.Name = "toolStripButtonGoToObject";
		toolStripButtonGoToObject.Size = new Size(92, 22);
		toolStripButtonGoToObject.Text = "&Go to object";
		toolStripButtonGoToObject.Click += ToolStripButtonGoToObject_Click;
		toolStripButtonGoToObject.MouseEnter += Control_Enter;
		toolStripButtonGoToObject.MouseLeave += Control_Leave;
		// 
		// Search2Form
		// 
		AccessibleDescription = "Dialog to search a word, a keyword or a number";
		AccessibleName = "Search";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(851, 473);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "Search2Form";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Search in MPCORB.DAT";
		Load += Search2Form_Load;
		((System.ComponentModel.ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		splitContainer.Panel1.ResumeLayout(false);
		splitContainer.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
		splitContainer.ResumeLayout(false);
		contextMenuStripMark.ResumeLayout(false);
		contextMenuSaveToFile.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((System.ComponentModel.ISupportInitialize)kryptonPanel).EndInit();
		kryptonPanel.ResumeLayout(false);
		kryptonToolStripIcons.ResumeLayout(false);
		kryptonToolStripIcons.PerformLayout();
		ResumeLayout(false);

	}

	#endregion

	private Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        private Krypton.Toolkit.KryptonCheckedListBox kryptonCheckedListBoxElements;
        private System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderDesignation;
        private System.Windows.Forms.ColumnHeader columnHeaderElement;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
	private ToolStripContainer toolStripContainer;
	private Krypton.Toolkit.KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private Krypton.Toolkit.KryptonPanel kryptonPanel;
	private Krypton.Toolkit.KryptonToolStrip kryptonToolStripIcons;
	private ToolStripButton toolStripButtonSearch;
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripLabel toolStripLabelProgress;
	private Krypton.Toolkit.KryptonProgressBarToolStripItem kryptonProgressBar;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripLabel toolStripLabelSearch;
	private ToolStripTextBox toolStripTextBoxSearch;
	private ToolStripButton toolStripButtonFullText;
	private SplitContainer splitContainer;
	private ContextMenuStrip contextMenuStripMark;
	private ToolStripMenuItem toolStripMenuItemMarkAll;
	private ToolStripMenuItem toolStripMenuItemUnmarkAll;
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
	private ToolStripSeparator toolStripSeparator3;
	private ToolStripButton toolStripButtonGoToObject;
}
