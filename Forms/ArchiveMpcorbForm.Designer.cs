using System.ComponentModel;
using Krypton.Toolkit;
using Planetoid_DB.Resources;

namespace Planetoid_DB
{
    partial class ArchiveMpcorbForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ArchiveMpcorbForm));
			kryptonTextBoxSource = new KryptonTextBox();
			kryptonButtonBrowseSource = new KryptonButton();
			statusStrip = new KryptonStatusStrip();
			labelStatus = new ToolStripStatusLabel();
			toolStripContainer = new ToolStripContainer();
			kryptonPanel = new KryptonPanel();
			groupBoxTarget = new GroupBox();
			kryptonLabelTarget = new KryptonLabel();
			kryptonButtonBrowseTarget = new KryptonButton();
			kryptonTextBoxTarget = new KryptonTextBox();
			groupBoxSource = new GroupBox();
			kryptonLabelSource = new KryptonLabel();
			toolStripIcons = new KryptonToolStrip();
			toolStripButtonArchive = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			toolStripDropDownButtonFormat = new ToolStripDropDownButton();
			toolStripMenuItemFormatZip = new ToolStripMenuItem();
			toolStripMenuItemFormatGzip = new ToolStripMenuItem();
			toolStripMenuItemFormatBrotli = new ToolStripMenuItem();
			toolStripDropDownButtonCompression = new ToolStripDropDownButton();
			toolStripMenuItemCompressionOptimal = new ToolStripMenuItem();
			toolStripMenuItemCompressionFastest = new ToolStripMenuItem();
			toolStripMenuItemCompressionNo = new ToolStripMenuItem();
			toolStripMenuItemCompressionSmallestSize = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			toolStripLabelProgress = new ToolStripLabel();
			kryptonProgressBarToolStripItemCompression = new KryptonProgressBarToolStripItem();
			statusStrip.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			((ISupportInitialize)kryptonPanel).BeginInit();
			kryptonPanel.SuspendLayout();
			groupBoxTarget.SuspendLayout();
			groupBoxSource.SuspendLayout();
			toolStripIcons.SuspendLayout();
			SuspendLayout();
			// 
			// kryptonTextBoxSource
			// 
			kryptonTextBoxSource.AccessibleDescription = "Inputs the full path name of the MPCORB.DAT file as source";
			kryptonTextBoxSource.AccessibleName = "MPCORB full path";
			kryptonTextBoxSource.AccessibleRole = AccessibleRole.Text;
			kryptonTextBoxSource.Location = new Point(94, 22);
			kryptonTextBoxSource.Margin = new Padding(4, 3, 4, 3);
			kryptonTextBoxSource.Name = "kryptonTextBoxSource";
			kryptonTextBoxSource.Size = new Size(432, 23);
			kryptonTextBoxSource.TabIndex = 1;
			kryptonTextBoxSource.ToolTipValues.Description = "Inputs the full path name of the MPCORB.DAT file as source";
			kryptonTextBoxSource.ToolTipValues.EnableToolTips = true;
			kryptonTextBoxSource.ToolTipValues.Heading = "MPCORB full path";
			kryptonTextBoxSource.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonTextBoxSource.Enter += Control_Enter;
			kryptonTextBoxSource.Leave += Control_Leave;
			kryptonTextBoxSource.MouseEnter += Control_Enter;
			kryptonTextBoxSource.MouseLeave += Control_Leave;
			// 
			// kryptonButtonBrowseSource
			// 
			kryptonButtonBrowseSource.AccessibleDescription = "Browses the full path name of the MPCORB.DAT file as source";
			kryptonButtonBrowseSource.AccessibleName = "Browse the MPCORB full path";
			kryptonButtonBrowseSource.AccessibleRole = AccessibleRole.PushButton;
			kryptonButtonBrowseSource.Location = new Point(534, 22);
			kryptonButtonBrowseSource.Margin = new Padding(4, 3, 4, 3);
			kryptonButtonBrowseSource.Name = "kryptonButtonBrowseSource";
			kryptonButtonBrowseSource.Size = new Size(89, 23);
			kryptonButtonBrowseSource.TabIndex = 2;
			kryptonButtonBrowseSource.ToolTipValues.Description = "Browses the full path name of the MPCORB.DAT file as source";
			kryptonButtonBrowseSource.ToolTipValues.EnableToolTips = true;
			kryptonButtonBrowseSource.ToolTipValues.Heading = "Browse the MPCORB full path";
			kryptonButtonBrowseSource.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonButtonBrowseSource.Values.DropDownArrowColor = Color.Empty;
			kryptonButtonBrowseSource.Values.Image = FatcowIcons16px.fatcow_folder_16px;
			kryptonButtonBrowseSource.Values.Text = "Browse (&1)";
			kryptonButtonBrowseSource.Click += KryptonButtonBrowseSource_Click;
			kryptonButtonBrowseSource.Enter += Control_Enter;
			kryptonButtonBrowseSource.Leave += Control_Leave;
			kryptonButtonBrowseSource.MouseEnter += Control_Enter;
			kryptonButtonBrowseSource.MouseLeave += Control_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar with some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.AllowClickThrough = true;
			statusStrip.AllowItemReorder = true;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelStatus });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.ShowItemToolTips = true;
			statusStrip.Size = new Size(657, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 0;
			statusStrip.TabStop = true;
			statusStrip.Text = "statusStrip";
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
			labelStatus.Name = "labelStatus";
			labelStatus.Size = new Size(144, 17);
			labelStatus.Text = "some information here";
			labelStatus.MouseEnter += Control_Enter;
			labelStatus.MouseLeave += Control_Leave;
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
			toolStripContainer.ContentPanel.Controls.Add(kryptonPanel);
			toolStripContainer.ContentPanel.Size = new Size(657, 141);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(657, 188);
			toolStripContainer.TabIndex = 5;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(toolStripIcons);
			// 
			// kryptonPanel
			// 
			kryptonPanel.AccessibleDescription = "Groups the data";
			kryptonPanel.AccessibleName = "Panel";
			kryptonPanel.AccessibleRole = AccessibleRole.Pane;
			kryptonPanel.Controls.Add(groupBoxTarget);
			kryptonPanel.Controls.Add(groupBoxSource);
			kryptonPanel.Dock = DockStyle.Fill;
			kryptonPanel.Location = new Point(0, 0);
			kryptonPanel.Name = "kryptonPanel";
			kryptonPanel.Size = new Size(657, 141);
			kryptonPanel.TabIndex = 0;
			kryptonPanel.TabStop = true;
			// 
			// groupBoxTarget
			// 
			groupBoxTarget.AccessibleDescription = "Groups the target";
			groupBoxTarget.AccessibleName = "Group the target";
			groupBoxTarget.AccessibleRole = AccessibleRole.Grouping;
			groupBoxTarget.BackColor = Color.Transparent;
			groupBoxTarget.Controls.Add(kryptonLabelTarget);
			groupBoxTarget.Controls.Add(kryptonButtonBrowseTarget);
			groupBoxTarget.Controls.Add(kryptonTextBoxTarget);
			groupBoxTarget.Location = new Point(12, 69);
			groupBoxTarget.Name = "groupBoxTarget";
			groupBoxTarget.Size = new Size(634, 60);
			groupBoxTarget.TabIndex = 1;
			groupBoxTarget.TabStop = false;
			groupBoxTarget.Text = "Target";
			// 
			// kryptonLabelTarget
			// 
			kryptonLabelTarget.AccessibleDescription = "Shows the full path name of the MPCORB.DAT file as target";
			kryptonLabelTarget.AccessibleName = "MPCORB.DAT";
			kryptonLabelTarget.AccessibleRole = AccessibleRole.StaticText;
			kryptonLabelTarget.Location = new Point(7, 22);
			kryptonLabelTarget.Margin = new Padding(4, 3, 4, 3);
			kryptonLabelTarget.Name = "kryptonLabelTarget";
			kryptonLabelTarget.Size = new Size(89, 23);
			kryptonLabelTarget.TabIndex = 0;
			kryptonLabelTarget.ToolTipValues.Description = "Shows the full path name of the MPCORB.DAT file as target";
			kryptonLabelTarget.ToolTipValues.EnableToolTips = true;
			kryptonLabelTarget.ToolTipValues.Heading = "MPCORB.DAT";
			kryptonLabelTarget.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonLabelTarget.Values.Text = "MPCORB.DAT:";
			// 
			// kryptonButtonBrowseTarget
			// 
			kryptonButtonBrowseTarget.AccessibleDescription = "Browses the full path name of the MPCORB.DAT file as target";
			kryptonButtonBrowseTarget.AccessibleName = "Browse the MPCORB full path";
			kryptonButtonBrowseTarget.AccessibleRole = AccessibleRole.PushButton;
			kryptonButtonBrowseTarget.Location = new Point(534, 22);
			kryptonButtonBrowseTarget.Margin = new Padding(4, 3, 4, 3);
			kryptonButtonBrowseTarget.Name = "kryptonButtonBrowseTarget";
			kryptonButtonBrowseTarget.Size = new Size(89, 23);
			kryptonButtonBrowseTarget.TabIndex = 2;
			kryptonButtonBrowseTarget.ToolTipValues.Description = "Browses the full path name of the MPCORB.DAT file as target";
			kryptonButtonBrowseTarget.ToolTipValues.EnableToolTips = true;
			kryptonButtonBrowseTarget.ToolTipValues.Heading = "Browse the MPCORB full path";
			kryptonButtonBrowseTarget.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonButtonBrowseTarget.Values.DropDownArrowColor = Color.Empty;
			kryptonButtonBrowseTarget.Values.Image = FatcowIcons16px.fatcow_folder_16px;
			kryptonButtonBrowseTarget.Values.Text = "Browse (&2)";
			kryptonButtonBrowseTarget.Click += KryptonButtonBrowseTarget_Click;
			kryptonButtonBrowseTarget.Enter += Control_Enter;
			kryptonButtonBrowseTarget.Leave += Control_Leave;
			kryptonButtonBrowseTarget.MouseEnter += Control_Enter;
			kryptonButtonBrowseTarget.MouseLeave += Control_Leave;
			// 
			// kryptonTextBoxTarget
			// 
			kryptonTextBoxTarget.AccessibleDescription = "Inputs the full path name of the MPCORB.DAT file as target";
			kryptonTextBoxTarget.AccessibleName = "MPCORB full path";
			kryptonTextBoxTarget.AccessibleRole = AccessibleRole.Text;
			kryptonTextBoxTarget.Location = new Point(94, 22);
			kryptonTextBoxTarget.Margin = new Padding(4, 3, 4, 3);
			kryptonTextBoxTarget.Name = "kryptonTextBoxTarget";
			kryptonTextBoxTarget.Size = new Size(432, 23);
			kryptonTextBoxTarget.TabIndex = 1;
			kryptonTextBoxTarget.ToolTipValues.Description = "Inputs the full path name of the MPCORB.DAT file as target";
			kryptonTextBoxTarget.ToolTipValues.EnableToolTips = true;
			kryptonTextBoxTarget.ToolTipValues.Heading = "MPCORB full path";
			kryptonTextBoxTarget.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonTextBoxTarget.Enter += Control_Enter;
			kryptonTextBoxTarget.Leave += Control_Leave;
			kryptonTextBoxTarget.MouseEnter += Control_Enter;
			kryptonTextBoxTarget.MouseLeave += Control_Leave;
			// 
			// groupBoxSource
			// 
			groupBoxSource.AccessibleDescription = "Groups the source";
			groupBoxSource.AccessibleName = "Group the source";
			groupBoxSource.AccessibleRole = AccessibleRole.Grouping;
			groupBoxSource.BackColor = Color.Transparent;
			groupBoxSource.Controls.Add(kryptonLabelSource);
			groupBoxSource.Controls.Add(kryptonButtonBrowseSource);
			groupBoxSource.Controls.Add(kryptonTextBoxSource);
			groupBoxSource.Location = new Point(12, 3);
			groupBoxSource.Name = "groupBoxSource";
			groupBoxSource.Size = new Size(634, 60);
			groupBoxSource.TabIndex = 0;
			groupBoxSource.TabStop = false;
			groupBoxSource.Text = "Source";
			// 
			// kryptonLabelSource
			// 
			kryptonLabelSource.AccessibleDescription = "Shows the full path name of the MPCORB.DAT file as source";
			kryptonLabelSource.AccessibleName = "MPCORB.DAT";
			kryptonLabelSource.AccessibleRole = AccessibleRole.StaticText;
			kryptonLabelSource.Location = new Point(7, 22);
			kryptonLabelSource.Margin = new Padding(4, 3, 4, 3);
			kryptonLabelSource.Name = "kryptonLabelSource";
			kryptonLabelSource.Size = new Size(89, 23);
			kryptonLabelSource.TabIndex = 0;
			kryptonLabelSource.ToolTipValues.Description = "Shows the full path name of the MPCORB.DAT file as source";
			kryptonLabelSource.ToolTipValues.EnableToolTips = true;
			kryptonLabelSource.ToolTipValues.Heading = "MPCORB.DAT";
			kryptonLabelSource.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			kryptonLabelSource.Values.Text = "MPCORB.DAT:";
			kryptonLabelSource.Enter += Control_Enter;
			kryptonLabelSource.Leave += Control_Leave;
			kryptonLabelSource.MouseEnter += Control_Enter;
			kryptonLabelSource.MouseLeave += Control_Leave;
			// 
			// toolStripIcons
			// 
			toolStripIcons.AccessibleDescription = "Toolbar of archiving and setting";
			toolStripIcons.AccessibleName = "Toolbar of archiving and setting";
			toolStripIcons.AccessibleRole = AccessibleRole.ToolTip;
			toolStripIcons.AllowClickThrough = true;
			toolStripIcons.AllowItemReorder = true;
			toolStripIcons.Dock = DockStyle.None;
			toolStripIcons.Font = new Font("Segoe UI", 9F);
			toolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonArchive, toolStripSeparator1, toolStripDropDownButtonFormat, toolStripDropDownButtonCompression, toolStripSeparator2, toolStripLabelProgress, kryptonProgressBarToolStripItemCompression });
			toolStripIcons.Location = new Point(0, 0);
			toolStripIcons.Name = "toolStripIcons";
			toolStripIcons.Size = new Size(657, 25);
			toolStripIcons.Stretch = true;
			toolStripIcons.TabIndex = 0;
			toolStripIcons.TabStop = true;
			toolStripIcons.Text = "Toolbar of archiving and setting";
			toolStripIcons.Enter += Control_Enter;
			toolStripIcons.Leave += Control_Leave;
			toolStripIcons.MouseEnter += Control_Enter;
			toolStripIcons.MouseLeave += Control_Leave;
			// 
			// toolStripButtonArchive
			// 
			toolStripButtonArchive.AccessibleDescription = "Archives the file MPCORB.DAT";
			toolStripButtonArchive.AccessibleName = "Archive";
			toolStripButtonArchive.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonArchive.Image = FatcowIcons16px.fatcow_package_16px;
			toolStripButtonArchive.ImageTransparentColor = Color.Magenta;
			toolStripButtonArchive.Name = "toolStripButtonArchive";
			toolStripButtonArchive.Size = new Size(67, 22);
			toolStripButtonArchive.Text = "&Archive";
			toolStripButtonArchive.Click += ToolStripButtonArchive_Click;
			toolStripButtonArchive.MouseEnter += Control_Enter;
			toolStripButtonArchive.MouseLeave += Control_Leave;
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
			// toolStripDropDownButtonFormat
			// 
			toolStripDropDownButtonFormat.AccessibleDescription = "Shows the format of the compression";
			toolStripDropDownButtonFormat.AccessibleName = "Format";
			toolStripDropDownButtonFormat.AccessibleRole = AccessibleRole.PushButton;
			toolStripDropDownButtonFormat.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemFormatZip, toolStripMenuItemFormatGzip, toolStripMenuItemFormatBrotli });
			toolStripDropDownButtonFormat.Image = FatcowIcons16px.fatcow_file_extension_zip_16px;
			toolStripDropDownButtonFormat.ImageTransparentColor = Color.Magenta;
			toolStripDropDownButtonFormat.Name = "toolStripDropDownButtonFormat";
			toolStripDropDownButtonFormat.Size = new Size(74, 22);
			toolStripDropDownButtonFormat.Text = "&Format";
			toolStripDropDownButtonFormat.MouseEnter += Control_Enter;
			toolStripDropDownButtonFormat.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemFormatZip
			// 
			toolStripMenuItemFormatZip.AccessibleDescription = "Shows the format \"Zip\" as menu item";
			toolStripMenuItemFormatZip.AccessibleName = "Format: Zip";
			toolStripMenuItemFormatZip.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemFormatZip.AutoToolTip = true;
			toolStripMenuItemFormatZip.Checked = true;
			toolStripMenuItemFormatZip.CheckOnClick = true;
			toolStripMenuItemFormatZip.CheckState = CheckState.Checked;
			toolStripMenuItemFormatZip.Name = "toolStripMenuItemFormatZip";
			toolStripMenuItemFormatZip.ShortcutKeyDisplayString = "Strg+Z";
			toolStripMenuItemFormatZip.ShortcutKeys = Keys.Control | Keys.Z;
			toolStripMenuItemFormatZip.Size = new Size(145, 22);
			toolStripMenuItemFormatZip.Text = "&Zip";
			toolStripMenuItemFormatZip.Click += ToolStripMenuItemFormatZip_Click;
			toolStripMenuItemFormatZip.MouseEnter += Control_Enter;
			toolStripMenuItemFormatZip.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemFormatGzip
			// 
			toolStripMenuItemFormatGzip.AccessibleDescription = "Shows the format \"GZip\" as menu item";
			toolStripMenuItemFormatGzip.AccessibleName = "Format: GZip";
			toolStripMenuItemFormatGzip.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemFormatGzip.AutoToolTip = true;
			toolStripMenuItemFormatGzip.CheckOnClick = true;
			toolStripMenuItemFormatGzip.Name = "toolStripMenuItemFormatGzip";
			toolStripMenuItemFormatGzip.ShortcutKeyDisplayString = "Strg+G";
			toolStripMenuItemFormatGzip.ShortcutKeys = Keys.Control | Keys.G;
			toolStripMenuItemFormatGzip.Size = new Size(145, 22);
			toolStripMenuItemFormatGzip.Text = "&GZip";
			toolStripMenuItemFormatGzip.Click += ToolStripMenuItemFormatGzip_Click;
			toolStripMenuItemFormatGzip.MouseEnter += Control_Enter;
			toolStripMenuItemFormatGzip.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemFormatBrotli
			// 
			toolStripMenuItemFormatBrotli.AccessibleDescription = "Shows the format \"Brotli\" as menu item";
			toolStripMenuItemFormatBrotli.AccessibleName = "Format: Brotli";
			toolStripMenuItemFormatBrotli.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemFormatBrotli.AutoToolTip = true;
			toolStripMenuItemFormatBrotli.CheckOnClick = true;
			toolStripMenuItemFormatBrotli.Name = "toolStripMenuItemFormatBrotli";
			toolStripMenuItemFormatBrotli.ShortcutKeyDisplayString = "Strg+B";
			toolStripMenuItemFormatBrotli.ShortcutKeys = Keys.Control | Keys.B;
			toolStripMenuItemFormatBrotli.Size = new Size(145, 22);
			toolStripMenuItemFormatBrotli.Text = "&Brotli";
			toolStripMenuItemFormatBrotli.Click += ToolStripMenuItemFormatBrotli_Click;
			toolStripMenuItemFormatBrotli.MouseEnter += Control_Enter;
			toolStripMenuItemFormatBrotli.MouseLeave += Control_Leave;
			// 
			// toolStripDropDownButtonCompression
			// 
			toolStripDropDownButtonCompression.AccessibleDescription = "Shows the compresson methods";
			toolStripDropDownButtonCompression.AccessibleName = "Compression";
			toolStripDropDownButtonCompression.AccessibleRole = AccessibleRole.PushButton;
			toolStripDropDownButtonCompression.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemCompressionOptimal, toolStripMenuItemCompressionFastest, toolStripMenuItemCompressionNo, toolStripMenuItemCompressionSmallestSize });
			toolStripDropDownButtonCompression.Image = FatcowIcons16px.fatcow_gear_in_16px;
			toolStripDropDownButtonCompression.ImageTransparentColor = Color.Magenta;
			toolStripDropDownButtonCompression.Name = "toolStripDropDownButtonCompression";
			toolStripDropDownButtonCompression.Size = new Size(106, 22);
			toolStripDropDownButtonCompression.Text = "&Compression";
			toolStripDropDownButtonCompression.MouseEnter += Control_Enter;
			toolStripDropDownButtonCompression.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemCompressionOptimal
			// 
			toolStripMenuItemCompressionOptimal.AccessibleDescription = "Shows the method \"Optimal\" as menu item";
			toolStripMenuItemCompressionOptimal.AccessibleName = "Compression: optimal";
			toolStripMenuItemCompressionOptimal.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemCompressionOptimal.AutoToolTip = true;
			toolStripMenuItemCompressionOptimal.Checked = true;
			toolStripMenuItemCompressionOptimal.CheckOnClick = true;
			toolStripMenuItemCompressionOptimal.CheckState = CheckState.Checked;
			toolStripMenuItemCompressionOptimal.Name = "toolStripMenuItemCompressionOptimal";
			toolStripMenuItemCompressionOptimal.ShortcutKeyDisplayString = "Strg+O";
			toolStripMenuItemCompressionOptimal.ShortcutKeys = Keys.Control | Keys.O;
			toolStripMenuItemCompressionOptimal.Size = new Size(206, 22);
			toolStripMenuItemCompressionOptimal.Text = "&Optimal";
			toolStripMenuItemCompressionOptimal.Click += ToolStripMenuItemCompressionOptimal_Click;
			toolStripMenuItemCompressionOptimal.MouseEnter += Control_Enter;
			toolStripMenuItemCompressionOptimal.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemCompressionFastest
			// 
			toolStripMenuItemCompressionFastest.AccessibleDescription = "Shows the method \"Fastest\" as menu item";
			toolStripMenuItemCompressionFastest.AccessibleName = "Compression: fastest";
			toolStripMenuItemCompressionFastest.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemCompressionFastest.AutoToolTip = true;
			toolStripMenuItemCompressionFastest.CheckOnClick = true;
			toolStripMenuItemCompressionFastest.Name = "toolStripMenuItemCompressionFastest";
			toolStripMenuItemCompressionFastest.ShortcutKeyDisplayString = "Strg+F";
			toolStripMenuItemCompressionFastest.ShortcutKeys = Keys.Control | Keys.F;
			toolStripMenuItemCompressionFastest.Size = new Size(206, 22);
			toolStripMenuItemCompressionFastest.Text = "&Fastest";
			toolStripMenuItemCompressionFastest.Click += ToolStripMenuItemCompressionFastest_Click;
			toolStripMenuItemCompressionFastest.MouseEnter += Control_Enter;
			toolStripMenuItemCompressionFastest.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemCompressionNo
			// 
			toolStripMenuItemCompressionNo.AccessibleDescription = "Shows the method \"No compression\" as menu item";
			toolStripMenuItemCompressionNo.AccessibleName = "Compression: no compression";
			toolStripMenuItemCompressionNo.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemCompressionNo.AutoToolTip = true;
			toolStripMenuItemCompressionNo.CheckOnClick = true;
			toolStripMenuItemCompressionNo.Name = "toolStripMenuItemCompressionNo";
			toolStripMenuItemCompressionNo.ShortcutKeyDisplayString = "Strg+N";
			toolStripMenuItemCompressionNo.ShortcutKeys = Keys.Control | Keys.N;
			toolStripMenuItemCompressionNo.Size = new Size(206, 22);
			toolStripMenuItemCompressionNo.Text = "&No compression";
			toolStripMenuItemCompressionNo.Click += ToolStripMenuItemCompressionNo_Click;
			toolStripMenuItemCompressionNo.MouseEnter += Control_Enter;
			toolStripMenuItemCompressionNo.MouseLeave += Control_Leave;
			// 
			// toolStripMenuItemCompressionSmallestSize
			// 
			toolStripMenuItemCompressionSmallestSize.AccessibleDescription = "Shows the method \"Smallest size\" as menu item";
			toolStripMenuItemCompressionSmallestSize.AccessibleName = "Compression: smalllest size";
			toolStripMenuItemCompressionSmallestSize.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemCompressionSmallestSize.AutoToolTip = true;
			toolStripMenuItemCompressionSmallestSize.CheckOnClick = true;
			toolStripMenuItemCompressionSmallestSize.Name = "toolStripMenuItemCompressionSmallestSize";
			toolStripMenuItemCompressionSmallestSize.ShortcutKeyDisplayString = "Strg+S";
			toolStripMenuItemCompressionSmallestSize.ShortcutKeys = Keys.Control | Keys.S;
			toolStripMenuItemCompressionSmallestSize.Size = new Size(206, 22);
			toolStripMenuItemCompressionSmallestSize.Text = "&Smallest size";
			toolStripMenuItemCompressionSmallestSize.Click += ToolStripMenuItemCompressionSmallestSize_Click;
			toolStripMenuItemCompressionSmallestSize.MouseEnter += Control_Enter;
			toolStripMenuItemCompressionSmallestSize.MouseLeave += Control_Leave;
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
			// toolStripLabelProgress
			// 
			toolStripLabelProgress.AccessibleDescription = "Shows the progress of archiving";
			toolStripLabelProgress.AccessibleName = "Progress";
			toolStripLabelProgress.AccessibleRole = AccessibleRole.StatusBar;
			toolStripLabelProgress.AutoToolTip = true;
			toolStripLabelProgress.Name = "toolStripLabelProgress";
			toolStripLabelProgress.Size = new Size(52, 22);
			toolStripLabelProgress.Text = "Progress";
			toolStripLabelProgress.MouseEnter += Control_Enter;
			toolStripLabelProgress.MouseLeave += Control_Leave;
			// 
			// kryptonProgressBarToolStripItemCompression
			// 
			kryptonProgressBarToolStripItemCompression.AccessibleDescription = "Shows the state of the compression as progress";
			kryptonProgressBarToolStripItemCompression.AccessibleName = "Compression progress";
			kryptonProgressBarToolStripItemCompression.AccessibleRole = AccessibleRole.ProgressBar;
			kryptonProgressBarToolStripItemCompression.AutoToolTip = true;
			kryptonProgressBarToolStripItemCompression.Name = "kryptonProgressBarToolStripItemCompression";
			kryptonProgressBarToolStripItemCompression.Size = new Size(300, 22);
			kryptonProgressBarToolStripItemCompression.StateCommon.Back.Color1 = Color.Green;
			kryptonProgressBarToolStripItemCompression.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
			kryptonProgressBarToolStripItemCompression.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
			kryptonProgressBarToolStripItemCompression.Values.Text = "";
			kryptonProgressBarToolStripItemCompression.MouseEnter += Control_Enter;
			kryptonProgressBarToolStripItemCompression.MouseLeave += Control_Leave;
			// 
			// ArchiveMpcorbForm
			// 
			AccessibleDescription = "Archives the file MPCORB.DAT";
			AccessibleName = "Archive MPCORB.DAT";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(657, 188);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ArchiveMpcorbForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Archive MPCORB.DAT";
			Load += ArchiveMpcorbForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			((ISupportInitialize)kryptonPanel).EndInit();
			kryptonPanel.ResumeLayout(false);
			groupBoxTarget.ResumeLayout(false);
			groupBoxTarget.PerformLayout();
			groupBoxSource.ResumeLayout(false);
			groupBoxSource.PerformLayout();
			toolStripIcons.ResumeLayout(false);
			toolStripIcons.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
		private KryptonTextBox kryptonTextBoxSource;
        private KryptonButton kryptonButtonBrowseSource;
        
        private KryptonStatusStrip statusStrip;
        private ToolStripStatusLabel labelStatus;
		private ToolStripContainer toolStripContainer;
		private KryptonToolStrip toolStripIcons;
		private KryptonPanel kryptonPanel;
		private ToolStripButton toolStripButtonArchive;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripDropDownButton toolStripDropDownButtonFormat;
		private ToolStripDropDownButton toolStripDropDownButtonCompression;
		private ToolStripMenuItem toolStripMenuItemFormatZip;
		private ToolStripMenuItem toolStripMenuItemFormatGzip;
		private ToolStripMenuItem toolStripMenuItemFormatBrotli;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem toolStripMenuItemCompressionOptimal;
		private ToolStripMenuItem toolStripMenuItemCompressionFastest;
		private ToolStripMenuItem toolStripMenuItemCompressionNo;
		private ToolStripMenuItem toolStripMenuItemCompressionSmallestSize;
		private KryptonProgressBarToolStripItem kryptonProgressBarToolStripItemCompression;
		private GroupBox groupBoxSource;
		private KryptonLabel kryptonLabelSource;
		private GroupBox groupBoxTarget;
		private KryptonLabel kryptonLabelTarget;
		private KryptonButton kryptonButtonBrowseTarget;
		private KryptonTextBox kryptonTextBoxTarget;
		private ToolStripLabel toolStripLabelProgress;
	}
}
