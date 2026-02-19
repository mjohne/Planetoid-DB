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
			labelDirectory = new KryptonLabel();
			labelSize = new KryptonLabel();
			labelDateCreated = new KryptonLabel();
			labelDateAccessed = new KryptonLabel();
			labelDateWrited = new KryptonLabel();
			labelAttributes = new KryptonLabel();
			labelNameValue = new KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
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
			kryptonManager = new KryptonManager(components);
			contextMenuCopyToClipboard.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			statusStrip.SuspendLayout();
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
			labelName.ToolTipValues.Description = "Shows the name of the database";
			labelName.ToolTipValues.EnableToolTips = true;
			labelName.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelName.ToolTipValues.Heading = "Name";
			labelName.Values.Text = "Name";
			labelName.DoubleClick += CopyToClipboard_DoubleClick;
			labelName.Enter += Control_Enter;
			labelName.Leave += Control_Leave;
			labelName.MouseEnter += Control_Enter;
			labelName.MouseLeave += Control_Leave;
			// 
			// labelDirectory
			// 
			labelDirectory.AccessibleDescription = "Shows the directory of the database";
			labelDirectory.AccessibleName = "Directory";
			labelDirectory.AccessibleRole = AccessibleRole.Text;
			labelDirectory.Dock = DockStyle.Fill;
			labelDirectory.LabelStyle = LabelStyle.BoldPanel;
			labelDirectory.Location = new Point(4, 29);
			labelDirectory.Margin = new Padding(4, 3, 4, 3);
			labelDirectory.Name = "labelDirectory";
			labelDirectory.Size = new Size(102, 20);
			labelDirectory.TabIndex = 2;
			labelDirectory.ToolTipValues.Description = "Shows the directory of the database";
			labelDirectory.ToolTipValues.EnableToolTips = true;
			labelDirectory.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDirectory.ToolTipValues.Heading = "Directory";
			labelDirectory.Values.Text = "Directory";
			labelDirectory.DoubleClick += CopyToClipboard_DoubleClick;
			labelDirectory.Enter += Control_Enter;
			labelDirectory.Leave += Control_Leave;
			labelDirectory.MouseEnter += Control_Enter;
			labelDirectory.MouseLeave += Control_Leave;
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
			labelSize.ToolTipValues.Description = "Shows the size of the database";
			labelSize.ToolTipValues.EnableToolTips = true;
			labelSize.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSize.ToolTipValues.Heading = "Size";
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
			labelDateCreated.ToolTipValues.Description = "Shows the creation date of the database";
			labelDateCreated.ToolTipValues.EnableToolTips = true;
			labelDateCreated.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateCreated.ToolTipValues.Heading = "Creation date";
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
			labelDateAccessed.ToolTipValues.Description = "Shows the last access date of the database";
			labelDateAccessed.ToolTipValues.EnableToolTips = true;
			labelDateAccessed.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateAccessed.ToolTipValues.Heading = "Last access date";
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
			labelDateWrited.ToolTipValues.Description = "Shows the last write date of the database";
			labelDateWrited.ToolTipValues.EnableToolTips = true;
			labelDateWrited.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateWrited.ToolTipValues.Heading = "Last write date";
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
			labelAttributes.ToolTipValues.Description = "Shows the attributes of the database";
			labelAttributes.ToolTipValues.EnableToolTips = true;
			labelAttributes.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAttributes.ToolTipValues.Heading = "Attributes";
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
			labelNameValue.Size = new Size(252, 20);
			labelNameValue.TabIndex = 1;
			labelNameValue.ToolTipValues.Description = "Shows the name of the database";
			labelNameValue.ToolTipValues.EnableToolTips = true;
			labelNameValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelNameValue.ToolTipValues.Heading = "Name value";
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
			// labelDirectoryValue
			// 
			labelDirectoryValue.AccessibleDescription = "Shows the directory of the database";
			labelDirectoryValue.AccessibleName = "Directory value";
			labelDirectoryValue.AccessibleRole = AccessibleRole.Text;
			labelDirectoryValue.ContextMenuStrip = contextMenuCopyToClipboard;
			labelDirectoryValue.Dock = DockStyle.Fill;
			labelDirectoryValue.Location = new Point(114, 29);
			labelDirectoryValue.Margin = new Padding(4, 3, 4, 3);
			labelDirectoryValue.Name = "labelDirectoryValue";
			labelDirectoryValue.Size = new Size(252, 20);
			labelDirectoryValue.TabIndex = 3;
			labelDirectoryValue.ToolTipValues.Description = "Shows the directory of the database";
			labelDirectoryValue.ToolTipValues.EnableToolTips = true;
			labelDirectoryValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDirectoryValue.ToolTipValues.Heading = "Directory value";
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
			labelSizeValue.Size = new Size(252, 20);
			labelSizeValue.TabIndex = 5;
			labelSizeValue.ToolTipValues.Description = "Shows the size of the database";
			labelSizeValue.ToolTipValues.EnableToolTips = true;
			labelSizeValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSizeValue.ToolTipValues.Heading = "Size value";
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
			labelDateCreatedValue.Size = new Size(252, 20);
			labelDateCreatedValue.TabIndex = 7;
			labelDateCreatedValue.ToolTipValues.Description = "Shows the creation date of the database";
			labelDateCreatedValue.ToolTipValues.EnableToolTips = true;
			labelDateCreatedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateCreatedValue.ToolTipValues.Heading = "Creation date value";
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
			labelDateAccessedValue.Size = new Size(252, 20);
			labelDateAccessedValue.TabIndex = 9;
			labelDateAccessedValue.ToolTipValues.Description = "Shows the last access date of the database";
			labelDateAccessedValue.ToolTipValues.EnableToolTips = true;
			labelDateAccessedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateAccessedValue.ToolTipValues.Heading = "Last access date value";
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
			labelDateWritedValue.Size = new Size(252, 20);
			labelDateWritedValue.TabIndex = 11;
			labelDateWritedValue.ToolTipValues.Description = "Shows the last write date of the database";
			labelDateWritedValue.ToolTipValues.EnableToolTips = true;
			labelDateWritedValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDateWritedValue.ToolTipValues.Heading = "Last write date value";
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
			tableLayoutPanel.Controls.Add(labelDirectory, 0, 1);
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
			tableLayoutPanel.Size = new Size(370, 187);
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
			labelAttributesValue.Size = new Size(252, 25);
			labelAttributesValue.TabIndex = 13;
			labelAttributesValue.ToolTipValues.Description = "Shows the attributes of the database";
			labelAttributesValue.ToolTipValues.EnableToolTips = true;
			labelAttributesValue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAttributesValue.ToolTipValues.Heading = "Attributes value";
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
			toolStripContainer.ContentPanel.Size = new Size(370, 187);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(370, 209);
			toolStripContainer.TabIndex = 3;
			toolStripContainer.Text = "toolStripContainer";
			toolStripContainer.TopToolStripPanelVisible = false;
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
			statusStrip.Size = new Size(370, 22);
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
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// DatabaseInformationForm
			// 
			AccessibleDescription = "Shows the informations about the MPCORB.DAT database";
			AccessibleName = "Database Information";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(370, 209);
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
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);

		}

		#endregion
		private KryptonTableLayoutPanel tableLayoutPanel;
    private KryptonLabel labelName;
    private KryptonLabel labelDirectory;
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
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
	}
}