using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
  partial class DatabaseInformationForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(DatabaseInformationForm));
			labelName = new KryptonLabel();
			labelPath = new KryptonLabel();
			labelSize = new KryptonLabel();
			labelDateCreated = new KryptonLabel();
			labelDateAccessed = new KryptonLabel();
			labelDateWrited = new KryptonLabel();
			labelAttributes = new KryptonLabel();
			labelNameValue = new KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			labelDirectoryValue = new KryptonLabel();
			labelSizeValue = new KryptonLabel();
			labelDateCreatedValue = new KryptonLabel();
			labelDateAccessedValue = new KryptonLabel();
			labelDateWritedValue = new KryptonLabel();
			tableLayoutPanel = new KryptonTableLayoutPanel();
			labelAttributesValue = new KryptonLabel();
			toolStripContainer = new ToolStripContainer();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripButtonSaveToFile = new ToolStripButton();
			toolStripButtonCopyToClipboard = new ToolStripButton();
			kryptonManager = new KryptonManager(components);
			contextMenuFullCopyToClipboard = new ContextMenuStrip(components);
			menuitemCopyToClipboardName = new ToolStripMenuItem();
			menuitemCopyToClipboardPath = new ToolStripMenuItem();
			menuitemCopyToClipboardSize = new ToolStripMenuItem();
			menuitemCopyToClipboardCreationDate = new ToolStripMenuItem();
			menuitemCopyToClipboardLastAccessDate = new ToolStripMenuItem();
			menuitemCopyToClipboardLastWriteDate = new ToolStripMenuItem();
			menuitemCopyToClipboardAttributes = new ToolStripMenuItem();
			contextMenuSaveToFile = new ContextMenuStrip(components);
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
			contextMenuCopyToClipboard.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			statusStrip.SuspendLayout();
			kryptonToolStripIcons.SuspendLayout();
			contextMenuFullCopyToClipboard.SuspendLayout();
			contextMenuSaveToFile.SuspendLayout();
			SuspendLayout();
			// 
			// labelName
			// 
			labelName.AccessibleDescription = "Shows the name of the database";
			labelName.AccessibleName = "Name";
			labelName.AccessibleRole = AccessibleRole.Text;
			labelName.Dock = DockStyle.Fill;
			labelName.LabelStyle = LabelStyle.BoldPanel;
			labelName.Location = new Point(4, 3);
			labelName.Margin = new Padding(4, 3, 4, 3);
			labelName.Name = "labelName";
			labelName.Size = new Size(102, 20);
			labelName.TabIndex = 0;
			labelName.ToolTipValues.Description = "Shows the name of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelName.ToolTipValues.EnableToolTips = true;
			labelName.ToolTipValues.Heading = "Name value";
			labelName.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelName.Values.Text = "Name";
			labelName.DoubleClick += CopyToClipboard_DoubleClick;
			labelName.Enter += Control_Enter;
			labelName.Leave += Control_Leave;
			labelName.MouseEnter += Control_Enter;
			labelName.MouseLeave += Control_Leave;
			// 
			// labelPath
			// 
			labelPath.AccessibleDescription = "Shows the path of the database";
			labelPath.AccessibleName = "Path";
			labelPath.AccessibleRole = AccessibleRole.Text;
			labelPath.Dock = DockStyle.Fill;
			labelPath.LabelStyle = LabelStyle.BoldPanel;
			labelPath.Location = new Point(4, 29);
			labelPath.Margin = new Padding(4, 3, 4, 3);
			labelPath.Name = "labelPath";
			labelPath.Size = new Size(102, 20);
			labelPath.TabIndex = 2;
			labelPath.ToolTipValues.Description = "Shows the path of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelPath.ToolTipValues.EnableToolTips = true;
			labelPath.ToolTipValues.Heading = "Path";
			labelPath.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelPath.Values.Text = "Path";
			labelPath.DoubleClick += CopyToClipboard_DoubleClick;
			labelPath.Enter += Control_Enter;
			labelPath.Leave += Control_Leave;
			labelPath.MouseEnter += Control_Enter;
			labelPath.MouseLeave += Control_Leave;
			// 
			// labelSize
			// 
			labelSize.AccessibleDescription = "Shows the size of the database";
			labelSize.AccessibleName = "Size";
			labelSize.AccessibleRole = AccessibleRole.Text;
			labelSize.Dock = DockStyle.Fill;
			labelSize.LabelStyle = LabelStyle.BoldPanel;
			labelSize.Location = new Point(4, 55);
			labelSize.Margin = new Padding(4, 3, 4, 3);
			labelSize.Name = "labelSize";
			labelSize.Size = new Size(102, 20);
			labelSize.TabIndex = 4;
			labelSize.ToolTipValues.Description = "Shows the size of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSize.ToolTipValues.EnableToolTips = true;
			labelSize.ToolTipValues.Heading = "Size";
			labelSize.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSize.Values.Text = "Size";
			labelSize.DoubleClick += CopyToClipboard_DoubleClick;
			labelSize.Enter += Control_Enter;
			labelSize.Leave += Control_Leave;
			labelSize.MouseEnter += Control_Enter;
			labelSize.MouseLeave += Control_Leave;
			// 
			// labelDateCreated
			// 
			labelDateCreated.AccessibleDescription = "Shows the creation date of the database";
			labelDateCreated.AccessibleName = "Creation date";
			labelDateCreated.AccessibleRole = AccessibleRole.Text;
			labelDateCreated.Dock = DockStyle.Fill;
			labelDateCreated.LabelStyle = LabelStyle.BoldPanel;
			labelDateCreated.Location = new Point(4, 81);
			labelDateCreated.Margin = new Padding(4, 3, 4, 3);
			labelDateCreated.Name = "labelDateCreated";
			labelDateCreated.Size = new Size(102, 20);
			labelDateCreated.TabIndex = 6;
			labelDateCreated.ToolTipValues.Description = "Shows the creation date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateCreated.ToolTipValues.EnableToolTips = true;
			labelDateCreated.ToolTipValues.Heading = "Creation date";
			labelDateCreated.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateCreated.Values.Text = "Creation date";
			labelDateCreated.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateCreated.Enter += Control_Enter;
			labelDateCreated.Leave += Control_Leave;
			labelDateCreated.MouseEnter += Control_Enter;
			labelDateCreated.MouseLeave += Control_Leave;
			// 
			// labelDateAccessed
			// 
			labelDateAccessed.AccessibleDescription = "Shows the last access date of the database";
			labelDateAccessed.AccessibleName = "Last access date";
			labelDateAccessed.AccessibleRole = AccessibleRole.Text;
			labelDateAccessed.Dock = DockStyle.Fill;
			labelDateAccessed.LabelStyle = LabelStyle.BoldPanel;
			labelDateAccessed.Location = new Point(4, 107);
			labelDateAccessed.Margin = new Padding(4, 3, 4, 3);
			labelDateAccessed.Name = "labelDateAccessed";
			labelDateAccessed.Size = new Size(102, 20);
			labelDateAccessed.TabIndex = 8;
			labelDateAccessed.ToolTipValues.Description = "Shows the last access date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateAccessed.ToolTipValues.EnableToolTips = true;
			labelDateAccessed.ToolTipValues.Heading = "Last access date";
			labelDateAccessed.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateAccessed.Values.Text = "Last access date";
			labelDateAccessed.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateAccessed.Enter += Control_Enter;
			labelDateAccessed.Leave += Control_Leave;
			labelDateAccessed.MouseEnter += Control_Enter;
			labelDateAccessed.MouseLeave += Control_Leave;
			// 
			// labelDateWrited
			// 
			labelDateWrited.AccessibleDescription = "Shows the last write date of the database";
			labelDateWrited.AccessibleName = "Last write date";
			labelDateWrited.AccessibleRole = AccessibleRole.Text;
			labelDateWrited.Dock = DockStyle.Fill;
			labelDateWrited.LabelStyle = LabelStyle.BoldPanel;
			labelDateWrited.Location = new Point(4, 133);
			labelDateWrited.Margin = new Padding(4, 3, 4, 3);
			labelDateWrited.Name = "labelDateWrited";
			labelDateWrited.Size = new Size(102, 20);
			labelDateWrited.TabIndex = 10;
			labelDateWrited.ToolTipValues.Description = "Shows the last write date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateWrited.ToolTipValues.EnableToolTips = true;
			labelDateWrited.ToolTipValues.Heading = "Last write date";
			labelDateWrited.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateWrited.Values.Text = "Last write date";
			labelDateWrited.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateWrited.Enter += Control_Enter;
			labelDateWrited.Leave += Control_Leave;
			labelDateWrited.MouseEnter += Control_Enter;
			labelDateWrited.MouseLeave += Control_Leave;
			// 
			// labelAttributes
			// 
			labelAttributes.AccessibleDescription = "Shows the attributes of the database";
			labelAttributes.AccessibleName = "Attributes";
			labelAttributes.AccessibleRole = AccessibleRole.Text;
			labelAttributes.Dock = DockStyle.Fill;
			labelAttributes.LabelStyle = LabelStyle.BoldPanel;
			labelAttributes.Location = new Point(4, 159);
			labelAttributes.Margin = new Padding(4, 3, 4, 3);
			labelAttributes.Name = "labelAttributes";
			labelAttributes.Size = new Size(102, 25);
			labelAttributes.TabIndex = 12;
			labelAttributes.ToolTipValues.Description = "Shows the attributes of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelAttributes.ToolTipValues.EnableToolTips = true;
			labelAttributes.ToolTipValues.Heading = "Attributes";
			labelAttributes.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAttributes.Values.Text = "Attributes";
			labelAttributes.DoubleClick += CopyToClipboard_DoubleClick;
			labelAttributes.Enter += Control_Enter;
			labelAttributes.Leave += Control_Leave;
			labelAttributes.MouseEnter += Control_Enter;
			labelAttributes.MouseLeave += Control_Leave;
			// 
			// labelNameValue
			// 
			labelNameValue.AccessibleDescription = "Shows the name of the database";
			labelNameValue.AccessibleName = "Name value";
			labelNameValue.AccessibleRole = AccessibleRole.Text;
			labelNameValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelNameValue.Dock = DockStyle.Fill;
			labelNameValue.Location = new Point(114, 3);
			labelNameValue.Margin = new Padding(4, 3, 4, 3);
			labelNameValue.Name = "labelNameValue";
			labelNameValue.Size = new Size(290, 20);
			labelNameValue.TabIndex = 1;
			labelNameValue.ToolTipValues.Description = "Shows the name of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelNameValue.ToolTipValues.EnableToolTips = true;
			labelNameValue.ToolTipValues.Heading = "Name value";
			labelNameValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelNameValue.Values.Text = "..........";
			labelNameValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelNameValue.Enter += Control_Enter;
			labelNameValue.Leave += Control_Leave;
			labelNameValue.MouseDown += Control_MouseDown;
			labelNameValue.MouseEnter += Control_Enter;
			labelNameValue.MouseLeave += Control_Leave;
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
			// labelDirectoryValue
			// 
			labelDirectoryValue.AccessibleDescription = "Shows the path of the database";
			labelDirectoryValue.AccessibleName = "Path value";
			labelDirectoryValue.AccessibleRole = AccessibleRole.Text;
			labelDirectoryValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDirectoryValue.Dock = DockStyle.Fill;
			labelDirectoryValue.Location = new Point(114, 29);
			labelDirectoryValue.Margin = new Padding(4, 3, 4, 3);
			labelDirectoryValue.Name = "labelDirectoryValue";
			labelDirectoryValue.Size = new Size(290, 20);
			labelDirectoryValue.TabIndex = 3;
			labelDirectoryValue.ToolTipValues.Description = "Shows the path of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDirectoryValue.ToolTipValues.EnableToolTips = true;
			labelDirectoryValue.ToolTipValues.Heading = "Path value";
			labelDirectoryValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDirectoryValue.Values.Text = "..........";
			labelDirectoryValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDirectoryValue.Enter += Control_Enter;
			labelDirectoryValue.Leave += Control_Leave;
			labelDirectoryValue.MouseDown += Control_MouseDown;
			labelDirectoryValue.MouseEnter += Control_Enter;
			labelDirectoryValue.MouseLeave += Control_Leave;
			// 
			// labelSizeValue
			// 
			labelSizeValue.AccessibleDescription = "Shows the size of the database";
			labelSizeValue.AccessibleName = "Size value";
			labelSizeValue.AccessibleRole = AccessibleRole.Text;
			labelSizeValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSizeValue.Dock = DockStyle.Fill;
			labelSizeValue.Location = new Point(114, 55);
			labelSizeValue.Margin = new Padding(4, 3, 4, 3);
			labelSizeValue.Name = "labelSizeValue";
			labelSizeValue.Size = new Size(290, 20);
			labelSizeValue.TabIndex = 5;
			labelSizeValue.ToolTipValues.Description = "Shows the size of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSizeValue.ToolTipValues.EnableToolTips = true;
			labelSizeValue.ToolTipValues.Heading = "Size value";
			labelSizeValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSizeValue.Values.Text = "..........";
			labelSizeValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelSizeValue.Enter += Control_Enter;
			labelSizeValue.Leave += Control_Leave;
			labelSizeValue.MouseDown += Control_MouseDown;
			labelSizeValue.MouseEnter += Control_Enter;
			labelSizeValue.MouseLeave += Control_Leave;
			// 
			// labelDateCreatedValue
			// 
			labelDateCreatedValue.AccessibleDescription = "Shows the creation date of the database";
			labelDateCreatedValue.AccessibleName = "Creation date value";
			labelDateCreatedValue.AccessibleRole = AccessibleRole.Text;
			labelDateCreatedValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDateCreatedValue.Dock = DockStyle.Fill;
			labelDateCreatedValue.Location = new Point(114, 81);
			labelDateCreatedValue.Margin = new Padding(4, 3, 4, 3);
			labelDateCreatedValue.Name = "labelDateCreatedValue";
			labelDateCreatedValue.Size = new Size(290, 20);
			labelDateCreatedValue.TabIndex = 7;
			labelDateCreatedValue.ToolTipValues.Description = "Shows the creation date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateCreatedValue.ToolTipValues.EnableToolTips = true;
			labelDateCreatedValue.ToolTipValues.Heading = "Creation date value";
			labelDateCreatedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateCreatedValue.Values.Text = "..........";
			labelDateCreatedValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateCreatedValue.Enter += Control_Enter;
			labelDateCreatedValue.Leave += Control_Leave;
			labelDateCreatedValue.MouseDown += Control_MouseDown;
			labelDateCreatedValue.MouseEnter += Control_Enter;
			labelDateCreatedValue.MouseLeave += Control_Leave;
			// 
			// labelDateAccessedValue
			// 
			labelDateAccessedValue.AccessibleDescription = "Shows the last access date of the database";
			labelDateAccessedValue.AccessibleName = "Last access date value";
			labelDateAccessedValue.AccessibleRole = AccessibleRole.Text;
			labelDateAccessedValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDateAccessedValue.Dock = DockStyle.Fill;
			labelDateAccessedValue.Location = new Point(114, 107);
			labelDateAccessedValue.Margin = new Padding(4, 3, 4, 3);
			labelDateAccessedValue.Name = "labelDateAccessedValue";
			labelDateAccessedValue.Size = new Size(290, 20);
			labelDateAccessedValue.TabIndex = 9;
			labelDateAccessedValue.ToolTipValues.Description = "Shows the last access date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateAccessedValue.ToolTipValues.EnableToolTips = true;
			labelDateAccessedValue.ToolTipValues.Heading = "Last access date value";
			labelDateAccessedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateAccessedValue.Values.Text = "..........";
			labelDateAccessedValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateAccessedValue.Enter += Control_Enter;
			labelDateAccessedValue.Leave += Control_Leave;
			labelDateAccessedValue.MouseDown += Control_MouseDown;
			labelDateAccessedValue.MouseEnter += Control_Enter;
			labelDateAccessedValue.MouseLeave += Control_Leave;
			// 
			// labelDateWritedValue
			// 
			labelDateWritedValue.AccessibleDescription = "Shows the last write date of the database";
			labelDateWritedValue.AccessibleName = "Last write date value";
			labelDateWritedValue.AccessibleRole = AccessibleRole.Text;
			labelDateWritedValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDateWritedValue.Dock = DockStyle.Fill;
			labelDateWritedValue.Location = new Point(114, 133);
			labelDateWritedValue.Margin = new Padding(4, 3, 4, 3);
			labelDateWritedValue.Name = "labelDateWritedValue";
			labelDateWritedValue.Size = new Size(290, 20);
			labelDateWritedValue.TabIndex = 11;
			labelDateWritedValue.ToolTipValues.Description = "Shows the last write date of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDateWritedValue.ToolTipValues.EnableToolTips = true;
			labelDateWritedValue.ToolTipValues.Heading = "Last write date value";
			labelDateWritedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateWritedValue.Values.Text = "..........";
			labelDateWritedValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelDateWritedValue.Enter += Control_Enter;
			labelDateWritedValue.Leave += Control_Leave;
			labelDateWritedValue.MouseDown += Control_MouseDown;
			labelDateWritedValue.MouseEnter += Control_Enter;
			labelDateWritedValue.MouseLeave += Control_Leave;
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Groups the information";
			tableLayoutPanel.AccessibleName = "Information";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
			tableLayoutPanel.AutoScroll = true;
			tableLayoutPanel.ColumnCount = 2;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel.Controls.Add(labelName, 0, 0);
			tableLayoutPanel.Controls.Add(labelPath, 0, 1);
			tableLayoutPanel.Controls.Add(labelSize, 0, 2);
			tableLayoutPanel.Controls.Add(labelDateCreated, 0, 3);
			tableLayoutPanel.Controls.Add(labelDateAccessed, 0, 4);
			tableLayoutPanel.Controls.Add(labelDateWrited, 0, 5);
			tableLayoutPanel.Controls.Add(labelAttributes, 0, 6);
			tableLayoutPanel.Controls.Add(labelNameValue, 1, 0);
			tableLayoutPanel.Controls.Add(labelDirectoryValue, 1, 1);
			tableLayoutPanel.Controls.Add(labelSizeValue, 1, 2);
			tableLayoutPanel.Controls.Add(labelDateCreatedValue, 1, 3);
			tableLayoutPanel.Controls.Add(labelDateAccessedValue, 1, 4);
			tableLayoutPanel.Controls.Add(labelDateWritedValue, 1, 5);
			tableLayoutPanel.Controls.Add(labelAttributesValue, 1, 6);
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.Location = new Point(0, 0);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 7;
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.Size = new Size(408, 187);
			tableLayoutPanel.TabIndex = 0;
			tableLayoutPanel.TabStop = true;
			// 
			// labelAttributesValue
			// 
			labelAttributesValue.AccessibleDescription = "Shows the attributes of the database";
			labelAttributesValue.AccessibleName = "Attributes value";
			labelAttributesValue.AccessibleRole = AccessibleRole.Text;
			labelAttributesValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelAttributesValue.Dock = DockStyle.Fill;
			labelAttributesValue.Location = new Point(114, 159);
			labelAttributesValue.Margin = new Padding(4, 3, 4, 3);
			labelAttributesValue.Name = "labelAttributesValue";
			labelAttributesValue.Size = new Size(290, 25);
			labelAttributesValue.TabIndex = 13;
			labelAttributesValue.ToolTipValues.Description = "Shows the attributes of the database.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelAttributesValue.ToolTipValues.EnableToolTips = true;
			labelAttributesValue.ToolTipValues.Heading = "Attributes value";
			labelAttributesValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAttributesValue.Values.Text = "..........";
			labelAttributesValue.DoubleClick += CopyToClipboard_DoubleClick;
			labelAttributesValue.Enter += Control_Enter;
			labelAttributesValue.Leave += Control_Leave;
			labelAttributesValue.MouseDown += Control_MouseDown;
			labelAttributesValue.MouseEnter += Control_Enter;
			labelAttributesValue.MouseLeave += Control_Leave;
			// 
			// toolStripContainer
			// 
			toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
			toolStripContainer.AccessibleName = "Container to arrange the toolbars";
			toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(tableLayoutPanel);
			toolStripContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.ContentPanel.Size = new Size(408, 187);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(408, 234);
			toolStripContainer.TabIndex = 3;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
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
			statusStrip.Size = new Size(408, 22);
			statusStrip.TabIndex = 2;
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
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.AccessibleDescription = "Toolbar of copying and saving information";
			kryptonToolStripIcons.AccessibleName = "Toolbar of copying and saving information";
			kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonSaveToFile, toolStripButtonCopyToClipboard });
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(408, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.TabStop = true;
			kryptonToolStripIcons.Text = "Toolbar of copying and saving information";
			kryptonToolStripIcons.MouseEnter += Control_Enter;
			kryptonToolStripIcons.MouseLeave += Control_Leave;
			// 
			// toolStripButtonSaveToFile
			// 
			toolStripButtonSaveToFile.AccessibleDescription = "Saves information to file";
			toolStripButtonSaveToFile.AccessibleName = "Save to file";
			toolStripButtonSaveToFile.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonSaveToFile.Image = FatcowIcons16px.fatcow_diskette_16px;
			toolStripButtonSaveToFile.ImageTransparentColor = Color.Magenta;
			toolStripButtonSaveToFile.Name = "toolStripButtonSaveToFile";
			toolStripButtonSaveToFile.Size = new Size(84, 22);
			toolStripButtonSaveToFile.Text = "&Save to file";
			toolStripButtonSaveToFile.Click += ToolStripButtonSaveToFile_Click;
			toolStripButtonSaveToFile.MouseEnter += Control_Enter;
			toolStripButtonSaveToFile.MouseLeave += Control_Leave;
			// 
			// toolStripButtonCopyToClipboard
			// 
			toolStripButtonCopyToClipboard.AccessibleDescription = "Copies information to clipboard";
			toolStripButtonCopyToClipboard.AccessibleName = "Copy to clipboard";
			toolStripButtonCopyToClipboard.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonCopyToClipboard.Image = FugueIcons16px.fugue_blue_document_copy_16px;
			toolStripButtonCopyToClipboard.ImageTransparentColor = Color.Magenta;
			toolStripButtonCopyToClipboard.Name = "toolStripButtonCopyToClipboard";
			toolStripButtonCopyToClipboard.Size = new Size(122, 22);
			toolStripButtonCopyToClipboard.Text = "&Copy to clipboard";
			toolStripButtonCopyToClipboard.Click += ToolStripButtonCopyToClipboard_Click;
			toolStripButtonCopyToClipboard.MouseEnter += Control_Enter;
			toolStripButtonCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// contextMenuFullCopyToClipboard
			// 
			contextMenuFullCopyToClipboard.AccessibleDescription = "Shows the context menu for copying database information to the clipboard";
			contextMenuFullCopyToClipboard.AccessibleName = "Context menu for copying database information to the clipboard";
			contextMenuFullCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuFullCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuFullCopyToClipboard.Items.AddRange(new ToolStripItem[] { menuitemCopyToClipboardName, menuitemCopyToClipboardPath, menuitemCopyToClipboardSize, menuitemCopyToClipboardCreationDate, menuitemCopyToClipboardLastAccessDate, menuitemCopyToClipboardLastWriteDate, menuitemCopyToClipboardAttributes });
			contextMenuFullCopyToClipboard.Name = "Context menu for copying database information to the clipboard";
			contextMenuFullCopyToClipboard.Size = new Size(159, 158);
			contextMenuFullCopyToClipboard.Text = "Copy to clipboard";
			contextMenuFullCopyToClipboard.MouseEnter += Control_Enter;
			contextMenuFullCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardName
			// 
			menuitemCopyToClipboardName.AccessibleDescription = "Copies to clipboard: Name";
			menuitemCopyToClipboardName.AccessibleName = "Copy to clipboard: Name";
			menuitemCopyToClipboardName.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardName.AutoToolTip = true;
			menuitemCopyToClipboardName.Image = (Image)resources.GetObject("menuitemCopyToClipboardName.Image");
			menuitemCopyToClipboardName.Name = "menuitemCopyToClipboardName";
			menuitemCopyToClipboardName.Size = new Size(158, 22);
			menuitemCopyToClipboardName.Text = "Name";
			menuitemCopyToClipboardName.Click += MenuitemCopyToClipboardName_Click;
			menuitemCopyToClipboardName.MouseEnter += Control_Enter;
			menuitemCopyToClipboardName.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardPath
			// 
			menuitemCopyToClipboardPath.AccessibleDescription = "Copies to clipboard: Path";
			menuitemCopyToClipboardPath.AccessibleName = "Copy to clipboard: Path";
			menuitemCopyToClipboardPath.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardPath.AutoToolTip = true;
			menuitemCopyToClipboardPath.Image = (Image)resources.GetObject("menuitemCopyToClipboardPath.Image");
			menuitemCopyToClipboardPath.Name = "menuitemCopyToClipboardPath";
			menuitemCopyToClipboardPath.Size = new Size(158, 22);
			menuitemCopyToClipboardPath.Text = "Path";
			menuitemCopyToClipboardPath.Click += MenuitemCopyToClipboardPath_Click;
			menuitemCopyToClipboardPath.MouseEnter += Control_Enter;
			menuitemCopyToClipboardPath.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardSize
			// 
			menuitemCopyToClipboardSize.AccessibleDescription = "Copies to clipboard: Size";
			menuitemCopyToClipboardSize.AccessibleName = "Copy to clipboard: Size";
			menuitemCopyToClipboardSize.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardSize.AutoToolTip = true;
			menuitemCopyToClipboardSize.Image = (Image)resources.GetObject("menuitemCopyToClipboardSize.Image");
			menuitemCopyToClipboardSize.Name = "menuitemCopyToClipboardSize";
			menuitemCopyToClipboardSize.Size = new Size(158, 22);
			menuitemCopyToClipboardSize.Text = "Size";
			menuitemCopyToClipboardSize.Click += MenuitemCopyToClipboardSize_Click;
			menuitemCopyToClipboardSize.MouseEnter += Control_Enter;
			menuitemCopyToClipboardSize.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardCreationDate
			// 
			menuitemCopyToClipboardCreationDate.AccessibleDescription = "Copies to clipboard: Creation date";
			menuitemCopyToClipboardCreationDate.AccessibleName = "Copy to clipboard: Creation date";
			menuitemCopyToClipboardCreationDate.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardCreationDate.AutoToolTip = true;
			menuitemCopyToClipboardCreationDate.Image = (Image)resources.GetObject("menuitemCopyToClipboardCreationDate.Image");
			menuitemCopyToClipboardCreationDate.Name = "menuitemCopyToClipboardCreationDate";
			menuitemCopyToClipboardCreationDate.Size = new Size(158, 22);
			menuitemCopyToClipboardCreationDate.Text = "Creation date";
			menuitemCopyToClipboardCreationDate.Click += MenuitemCopyToClipboardCreationDate_Click;
			menuitemCopyToClipboardCreationDate.MouseEnter += Control_Enter;
			menuitemCopyToClipboardCreationDate.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardLastAccessDate
			// 
			menuitemCopyToClipboardLastAccessDate.AccessibleDescription = "Copies to clipboard: Last access date";
			menuitemCopyToClipboardLastAccessDate.AccessibleName = "Copy to clipboard: Last access date";
			menuitemCopyToClipboardLastAccessDate.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardLastAccessDate.AutoToolTip = true;
			menuitemCopyToClipboardLastAccessDate.Image = (Image)resources.GetObject("menuitemCopyToClipboardLastAccessDate.Image");
			menuitemCopyToClipboardLastAccessDate.Name = "menuitemCopyToClipboardLastAccessDate";
			menuitemCopyToClipboardLastAccessDate.Size = new Size(158, 22);
			menuitemCopyToClipboardLastAccessDate.Text = "Last access date";
			menuitemCopyToClipboardLastAccessDate.Click += MenuitemCopyToClipboardLastAccessDate_Click;
			menuitemCopyToClipboardLastAccessDate.MouseEnter += Control_Enter;
			menuitemCopyToClipboardLastAccessDate.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardLastWriteDate
			// 
			menuitemCopyToClipboardLastWriteDate.AccessibleDescription = "Copies to clipboard: Last write date";
			menuitemCopyToClipboardLastWriteDate.AccessibleName = "Copy to clipboard: Last write date";
			menuitemCopyToClipboardLastWriteDate.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardLastWriteDate.AutoToolTip = true;
			menuitemCopyToClipboardLastWriteDate.Image = (Image)resources.GetObject("menuitemCopyToClipboardLastWriteDate.Image");
			menuitemCopyToClipboardLastWriteDate.Name = "menuitemCopyToClipboardLastWriteDate";
			menuitemCopyToClipboardLastWriteDate.Size = new Size(158, 22);
			menuitemCopyToClipboardLastWriteDate.Text = "Last write date";
			menuitemCopyToClipboardLastWriteDate.Click += MenuitemCopyToClipboardLastWriteDate_Click;
			menuitemCopyToClipboardLastWriteDate.MouseEnter += Control_Enter;
			menuitemCopyToClipboardLastWriteDate.MouseLeave += Control_Leave;
			// 
			// menuitemCopyToClipboardAttributes
			// 
			menuitemCopyToClipboardAttributes.AccessibleDescription = "Copies to clipboard: Attributes";
			menuitemCopyToClipboardAttributes.AccessibleName = "Copy to clipboard: Attributes";
			menuitemCopyToClipboardAttributes.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardAttributes.AutoToolTip = true;
			menuitemCopyToClipboardAttributes.Image = (Image)resources.GetObject("menuitemCopyToClipboardAttributes.Image");
			menuitemCopyToClipboardAttributes.Name = "menuitemCopyToClipboardAttributes";
			menuitemCopyToClipboardAttributes.Size = new Size(158, 22);
			menuitemCopyToClipboardAttributes.Text = "Attributes";
			menuitemCopyToClipboardAttributes.Click += MenuitemCopyToClipboardAttributes_Click;
			menuitemCopyToClipboardAttributes.MouseEnter += Control_Enter;
			menuitemCopyToClipboardAttributes.MouseLeave += Control_Leave;
			// 
			// contextMenuSaveToFile
			// 
			contextMenuSaveToFile.AccessibleDescription = "Saves the information to file";
			contextMenuSaveToFile.AccessibleName = "Save to file";
			contextMenuSaveToFile.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuSaveToFile.Font = new Font("Segoe UI", 9F);
			contextMenuSaveToFile.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSaveAsText, toolStripMenuItemSaveAsLatex, toolStripMenuItemSaveAsMarkdown, toolStripMenuItemSaveAsWord, toolStripMenuItemSaveAsOdt, toolStripMenuItemSaveAsRtf, toolStripMenuItemSaveAsExcel, toolStripMenuItemSaveAsOds, toolStripMenuItemSaveAsCsv, toolStripMenuItemSaveAsTsv, toolStripMenuItemSaveAsPsv, toolStripMenuItemSaveAsHtml, toolStripMenuItemSaveAsXml, toolStripMenuItemSaveAsJson, toolStripMenuItemSaveAsYaml, toolStripMenuItemSaveAsSql, toolStripMenuItemSaveAsPdf, toolStripMenuItemSaveAsPostScript, toolStripMenuItemSaveAsEpub, toolStripMenuItemSaveAsMobi });
			contextMenuSaveToFile.Name = "contextMenuSaveToFile";
			contextMenuSaveToFile.Size = new Size(216, 444);
			contextMenuSaveToFile.TabStop = true;
			contextMenuSaveToFile.Text = "&Save to file";
			contextMenuSaveToFile.MouseEnter += Control_Enter;
			contextMenuSaveToFile.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsText
			// 
			toolStripMenuItemSaveAsText.AccessibleDescription = "Saves the information as text file";
			toolStripMenuItemSaveAsText.AccessibleName = "Save as text";
			toolStripMenuItemSaveAsText.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemSaveAsText.AutoToolTip = true;
			toolStripMenuItemSaveAsText.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			toolStripMenuItemSaveAsText.Name = "toolStripMenuItemSaveAsText";
			toolStripMenuItemSaveAsText.ShortcutKeyDisplayString = "Strg+X";
			toolStripMenuItemSaveAsText.ShortcutKeys = Keys.Control | Keys.X;
			toolStripMenuItemSaveAsText.Size = new Size(215, 22);
			toolStripMenuItemSaveAsText.Text = "Save as te&xt";
			toolStripMenuItemSaveAsText.Click += ToolStripMenuItemSaveAsText_Click;
			toolStripMenuItemSaveAsText.MouseEnter += Control_Enter;
			toolStripMenuItemSaveAsText.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemSaveAsLatex
			// 
			toolStripMenuItemSaveAsLatex.AccessibleDescription = "Saves the information as Latex file";
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
			toolStripMenuItemSaveAsMarkdown.AccessibleDescription = "Saves the information as Markdown file";
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
			toolStripMenuItemSaveAsWord.AccessibleDescription = "Saves the information as Word file";
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
			toolStripMenuItemSaveAsOdt.AccessibleDescription = "Saves the information as ODT file";
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
			toolStripMenuItemSaveAsRtf.AccessibleDescription = "Saves the information as RTF file";
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
			// 
			// toolStripMenuItemSaveAsExcel
			// 
			toolStripMenuItemSaveAsExcel.AccessibleDescription = "Saves the information as Excel file";
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
			toolStripMenuItemSaveAsOds.AccessibleDescription = "Saves the information as ODS file";
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
			toolStripMenuItemSaveAsCsv.AccessibleDescription = "Saves the information as CSV file";
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
			toolStripMenuItemSaveAsTsv.AccessibleDescription = "Saves the information as TSV file";
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
			toolStripMenuItemSaveAsPsv.AccessibleDescription = "Saves the information as PSV file";
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
			toolStripMenuItemSaveAsHtml.AccessibleDescription = "Saves the information as HTML file";
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
			toolStripMenuItemSaveAsXml.AccessibleDescription = "Saves the information as XML file";
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
			toolStripMenuItemSaveAsJson.AccessibleDescription = "Saves the information as JSON file";
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
			toolStripMenuItemSaveAsYaml.AccessibleDescription = "Saves the information as YAML file";
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
			toolStripMenuItemSaveAsSql.AccessibleDescription = "Saves the information as SQL script";
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
			toolStripMenuItemSaveAsPdf.AccessibleDescription = "Saves the information as PDF file";
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
			toolStripMenuItemSaveAsPostScript.AccessibleDescription = "Saves the information as PostScript file";
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
			toolStripMenuItemSaveAsEpub.AccessibleDescription = "Saves the information as EPUB file";
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
			toolStripMenuItemSaveAsMobi.AccessibleDescription = "Saves the information as MOBI file";
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
			// DatabaseInformationForm
			// 
			AccessibleDescription = "Shows the informations about the MPCORB.DAT database";
			AccessibleName = "Database Information";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(408, 234);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "DatabaseInformationForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Database information";
			Load += DatabaseInformationForm_Load;
			contextMenuCopyToClipboard.ResumeLayout(false);
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			kryptonToolStripIcons.ResumeLayout(false);
			kryptonToolStripIcons.PerformLayout();
			contextMenuFullCopyToClipboard.ResumeLayout(false);
			contextMenuSaveToFile.ResumeLayout(false);
			ResumeLayout(false);

		}

		#endregion
		private KryptonTableLayoutPanel tableLayoutPanel;
    private KryptonLabel labelName;
    private KryptonLabel labelPath;
    private KryptonLabel labelSize;
    private KryptonLabel labelDateCreated;
    private KryptonLabel labelDateAccessed;
    private KryptonLabel labelDateWrited;
    private KryptonLabel labelAttributes;
    private KryptonLabel labelNameValue;
    private KryptonLabel labelDirectoryValue;
    private KryptonLabel labelSizeValue;
    private KryptonLabel labelDateCreatedValue;
    private KryptonLabel labelDateAccessedValue;
    private KryptonLabel labelDateWritedValue;
    private KryptonLabel labelAttributesValue;
	private KryptonStatusStrip statusStrip;
	private ToolStripStatusLabel labelInformation;
	private ToolStripContainer toolStripContainer;
		private KryptonManager kryptonManager;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
		private KryptonToolStrip kryptonToolStripIcons;
		private ToolStripButton toolStripButtonCopyToClipboard;
		private ContextMenuStrip contextMenuFullCopyToClipboard;
		private ToolStripMenuItem menuitemCopyToClipboardName;
		private ToolStripMenuItem menuitemCopyToClipboardPath;
		private ToolStripMenuItem menuitemCopyToClipboardSize;
		private ToolStripMenuItem menuitemCopyToClipboardCreationDate;
		private ToolStripMenuItem menuitemCopyToClipboardLastAccessDate;
		private ToolStripMenuItem menuitemCopyToClipboardLastWriteDate;
		private ToolStripMenuItem menuitemCopyToClipboardAttributes;
		private ToolStripButton toolStripButtonSaveToFile;
		private ContextMenuStrip contextMenuSaveToFile;
		private ToolStripMenuItem toolStripMenuItemSaveAsLatex;
		private ToolStripMenuItem toolStripMenuItemSaveAsMarkdown;
		private ToolStripMenuItem toolStripMenuItemSaveAsWord;
		private ToolStripMenuItem toolStripMenuItemSaveAsOdt;
		private ToolStripMenuItem toolStripMenuItemSaveAsExcel;
		private ToolStripMenuItem toolStripMenuItemSaveAsOds;
		private ToolStripMenuItem toolStripMenuItemSaveAsCsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsTsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsPsv;
		private ToolStripMenuItem toolStripMenuItemSaveAsHtml;
		private ToolStripMenuItem toolStripMenuItemSaveAsXml;
		private ToolStripMenuItem toolStripMenuItemSaveAsJson;
		private ToolStripMenuItem toolStripMenuItemSaveAsYaml;
		private ToolStripMenuItem toolStripMenuItemSaveAsSql;
		private ToolStripMenuItem toolStripMenuItemSaveAsPdf;
		private ToolStripMenuItem toolStripMenuItemSaveAsEpub;
		private ToolStripMenuItem toolStripMenuItemSaveAsPostScript;
		private ToolStripMenuItem toolStripMenuItemSaveAsMobi;
		private ToolStripMenuItem toolStripMenuItemSaveAsText;
		private ToolStripMenuItem toolStripMenuItemSaveAsRtf;
	}
}