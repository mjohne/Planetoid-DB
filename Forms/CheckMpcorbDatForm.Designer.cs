using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class CheckMpcorbDatForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(CheckMpcorbDatForm));
			toolTip = new KryptonToolTip(components);
			labelUpdateNeeded = new KryptonLabel();
			labelMpcorbDatLocal = new KryptonLabel();
			labelMpcorbDatOnline = new KryptonLabel();
			labelContentLengthText = new KryptonLabel();
			labelModifiedDateText = new KryptonLabel();
			labelContentLengthValueLocal = new KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			labelModifiedDateValueLocal = new KryptonLabel();
			labelContentLengthValueOnline = new KryptonLabel();
			labelModifiedDateValueOnline = new KryptonLabel();
			tableLayoutPanel = new KryptonTableLayoutPanel();
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
			// labelUpdateNeeded
			// 
			labelUpdateNeeded.AccessibleDescription = "Informs if an update is recommended";
			labelUpdateNeeded.AccessibleName = "Update text";
			labelUpdateNeeded.AccessibleRole = AccessibleRole.Text;
			tableLayoutPanel.SetColumnSpan(labelUpdateNeeded, 3);
			labelUpdateNeeded.Dock = DockStyle.Fill;
			labelUpdateNeeded.LabelStyle = LabelStyle.TitleControl;
			labelUpdateNeeded.Location = new Point(4, 81);
			labelUpdateNeeded.Margin = new Padding(4, 3, 4, 3);
			labelUpdateNeeded.Name = "labelUpdateNeeded";
			labelUpdateNeeded.Size = new Size(372, 29);
			labelUpdateNeeded.TabIndex = 8;
			toolTip.SetToolTip(labelUpdateNeeded, "Informs if an update is recommended");
			labelUpdateNeeded.Values.Image = FatcowIcons16px.fatcow_help_16px;
			labelUpdateNeeded.Values.Text = "Update needed?";
			labelUpdateNeeded.DoubleClick += LabelUpdateNeeded_DoubleClick;
			labelUpdateNeeded.Enter += Control_Enter;
			labelUpdateNeeded.Leave += Control_Leave;
			labelUpdateNeeded.MouseEnter += Control_Enter;
			labelUpdateNeeded.MouseLeave += Control_Leave;
			// 
			// labelMpcorbDatLocal
			// 
			labelMpcorbDatLocal.AccessibleDescription = "Information about the local MPCORB.DAT file";
			labelMpcorbDatLocal.AccessibleName = "Local MPCORB.DAT file";
			labelMpcorbDatLocal.AccessibleRole = AccessibleRole.Text;
			labelMpcorbDatLocal.Dock = DockStyle.Fill;
			labelMpcorbDatLocal.LabelStyle = LabelStyle.BoldControl;
			labelMpcorbDatLocal.Location = new Point(113, 3);
			labelMpcorbDatLocal.Margin = new Padding(4, 3, 4, 3);
			labelMpcorbDatLocal.Name = "labelMpcorbDatLocal";
			labelMpcorbDatLocal.Size = new Size(122, 20);
			labelMpcorbDatLocal.TabIndex = 0;
			toolTip.SetToolTip(labelMpcorbDatLocal, "Information about the local MPCORB.DAT file");
			labelMpcorbDatLocal.Values.Text = "MPCORB.DAT local";
			labelMpcorbDatLocal.Enter += Control_Enter;
			labelMpcorbDatLocal.Leave += Control_Leave;
			labelMpcorbDatLocal.MouseEnter += Control_Enter;
			labelMpcorbDatLocal.MouseLeave += Control_Leave;
			// 
			// labelMpcorbDatOnline
			// 
			labelMpcorbDatOnline.AccessibleDescription = "Information about the online MPCORB.DAT file";
			labelMpcorbDatOnline.AccessibleName = "Online MPCORB.DAT file";
			labelMpcorbDatOnline.AccessibleRole = AccessibleRole.Text;
			labelMpcorbDatOnline.Dock = DockStyle.Fill;
			labelMpcorbDatOnline.LabelStyle = LabelStyle.BoldControl;
			labelMpcorbDatOnline.Location = new Point(243, 3);
			labelMpcorbDatOnline.Margin = new Padding(4, 3, 4, 3);
			labelMpcorbDatOnline.Name = "labelMpcorbDatOnline";
			labelMpcorbDatOnline.Size = new Size(133, 20);
			labelMpcorbDatOnline.TabIndex = 1;
			toolTip.SetToolTip(labelMpcorbDatOnline, "Information about the online MPCORB.DAT file");
			labelMpcorbDatOnline.Values.Text = "MPCORB.DAT online";
			labelMpcorbDatOnline.Enter += Control_Enter;
			labelMpcorbDatOnline.Leave += Control_Leave;
			labelMpcorbDatOnline.MouseEnter += Control_Enter;
			labelMpcorbDatOnline.MouseLeave += Control_Leave;
			// 
			// labelContentLengthText
			// 
			labelContentLengthText.AccessibleDescription = "Shows the content length";
			labelContentLengthText.AccessibleName = "Content length";
			labelContentLengthText.AccessibleRole = AccessibleRole.Text;
			labelContentLengthText.LabelStyle = LabelStyle.BoldControl;
			labelContentLengthText.Location = new Point(4, 29);
			labelContentLengthText.Margin = new Padding(4, 3, 4, 3);
			labelContentLengthText.Name = "labelContentLengthText";
			labelContentLengthText.Size = new Size(101, 20);
			labelContentLengthText.TabIndex = 2;
			toolTip.SetToolTip(labelContentLengthText, "Shows the content length");
			labelContentLengthText.Values.Text = "Content length:";
			labelContentLengthText.Enter += Control_Enter;
			labelContentLengthText.Leave += Control_Leave;
			labelContentLengthText.MouseEnter += Control_Enter;
			labelContentLengthText.MouseLeave += Control_Leave;
			// 
			// labelModifiedDateText
			// 
			labelModifiedDateText.AccessibleDescription = "Shows the modified date";
			labelModifiedDateText.AccessibleName = "Modified date";
			labelModifiedDateText.AccessibleRole = AccessibleRole.Text;
			labelModifiedDateText.Dock = DockStyle.Left;
			labelModifiedDateText.LabelStyle = LabelStyle.BoldControl;
			labelModifiedDateText.Location = new Point(4, 55);
			labelModifiedDateText.Margin = new Padding(4, 3, 4, 3);
			labelModifiedDateText.Name = "labelModifiedDateText";
			labelModifiedDateText.Size = new Size(96, 20);
			labelModifiedDateText.TabIndex = 5;
			toolTip.SetToolTip(labelModifiedDateText, "Shows the modified date");
			labelModifiedDateText.Values.Text = "Modified date:";
			labelModifiedDateText.Enter += Control_Enter;
			labelModifiedDateText.Leave += Control_Leave;
			labelModifiedDateText.MouseEnter += Control_Enter;
			labelModifiedDateText.MouseLeave += Control_Leave;
			// 
			// labelContentLengthValueLocal
			// 
			labelContentLengthValueLocal.AccessibleDescription = "Shows the local content length";
			labelContentLengthValueLocal.AccessibleName = "Local content length";
			labelContentLengthValueLocal.AccessibleRole = AccessibleRole.Text;
			labelContentLengthValueLocal.ContextMenuStrip = contextMenuCopyToClipboard;
			labelContentLengthValueLocal.Dock = DockStyle.Fill;
			labelContentLengthValueLocal.Location = new Point(113, 29);
			labelContentLengthValueLocal.Margin = new Padding(4, 3, 4, 3);
			labelContentLengthValueLocal.Name = "labelContentLengthValueLocal";
			labelContentLengthValueLocal.Size = new Size(122, 20);
			labelContentLengthValueLocal.TabIndex = 3;
			toolTip.SetToolTip(labelContentLengthValueLocal, "Shows the local content length");
			labelContentLengthValueLocal.Values.Text = "123456789 bytes";
			labelContentLengthValueLocal.DoubleClick += CopyToClipboard_DoubleClick;
			labelContentLengthValueLocal.Enter += Control_Enter;
			labelContentLengthValueLocal.Leave += Control_Leave;
			labelContentLengthValueLocal.MouseDown += Control_MouseDown;
			labelContentLengthValueLocal.MouseEnter += Control_Enter;
			labelContentLengthValueLocal.MouseLeave += Control_Leave;
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
			toolTip.SetToolTip(contextMenuCopyToClipboard, "Context menu for copying to clipboard");
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
			// labelModifiedDateValueLocal
			// 
			labelModifiedDateValueLocal.AccessibleDescription = "Shows the local modified date";
			labelModifiedDateValueLocal.AccessibleName = "Local modified date";
			labelModifiedDateValueLocal.AccessibleRole = AccessibleRole.Text;
			labelModifiedDateValueLocal.ContextMenuStrip = contextMenuCopyToClipboard;
			labelModifiedDateValueLocal.Dock = DockStyle.Fill;
			labelModifiedDateValueLocal.Location = new Point(113, 55);
			labelModifiedDateValueLocal.Margin = new Padding(4, 3, 4, 3);
			labelModifiedDateValueLocal.Name = "labelModifiedDateValueLocal";
			labelModifiedDateValueLocal.Size = new Size(122, 20);
			labelModifiedDateValueLocal.TabIndex = 6;
			toolTip.SetToolTip(labelModifiedDateValueLocal, "Shows the local modified date");
			labelModifiedDateValueLocal.Values.Text = "00.00.0000 00:00";
			labelModifiedDateValueLocal.DoubleClick += CopyToClipboard_DoubleClick;
			labelModifiedDateValueLocal.Enter += Control_Enter;
			labelModifiedDateValueLocal.Leave += Control_Leave;
			labelModifiedDateValueLocal.MouseDown += Control_MouseDown;
			labelModifiedDateValueLocal.MouseEnter += Control_Enter;
			labelModifiedDateValueLocal.MouseLeave += Control_Leave;
			// 
			// labelContentLengthValueOnline
			// 
			labelContentLengthValueOnline.AccessibleDescription = "Shows the online content length";
			labelContentLengthValueOnline.AccessibleName = "Online content length";
			labelContentLengthValueOnline.AccessibleRole = AccessibleRole.Text;
			labelContentLengthValueOnline.ContextMenuStrip = contextMenuCopyToClipboard;
			labelContentLengthValueOnline.Dock = DockStyle.Fill;
			labelContentLengthValueOnline.Location = new Point(243, 29);
			labelContentLengthValueOnline.Margin = new Padding(4, 3, 4, 3);
			labelContentLengthValueOnline.Name = "labelContentLengthValueOnline";
			labelContentLengthValueOnline.Size = new Size(133, 20);
			labelContentLengthValueOnline.TabIndex = 4;
			toolTip.SetToolTip(labelContentLengthValueOnline, "Shows the online content length");
			labelContentLengthValueOnline.Values.Text = "123456789 bytes";
			labelContentLengthValueOnline.DoubleClick += CopyToClipboard_DoubleClick;
			labelContentLengthValueOnline.Enter += Control_Enter;
			labelContentLengthValueOnline.Leave += Control_Leave;
			labelContentLengthValueOnline.MouseDown += Control_MouseDown;
			labelContentLengthValueOnline.MouseEnter += Control_Enter;
			labelContentLengthValueOnline.MouseLeave += Control_Leave;
			// 
			// labelModifiedDateValueOnline
			// 
			labelModifiedDateValueOnline.AccessibleDescription = "Shows the online modified date";
			labelModifiedDateValueOnline.AccessibleName = "Online Modified date";
			labelModifiedDateValueOnline.AccessibleRole = AccessibleRole.Text;
			labelModifiedDateValueOnline.ContextMenuStrip = contextMenuCopyToClipboard;
			labelModifiedDateValueOnline.Dock = DockStyle.Fill;
			labelModifiedDateValueOnline.Location = new Point(243, 55);
			labelModifiedDateValueOnline.Margin = new Padding(4, 3, 4, 3);
			labelModifiedDateValueOnline.Name = "labelModifiedDateValueOnline";
			labelModifiedDateValueOnline.Size = new Size(133, 20);
			labelModifiedDateValueOnline.TabIndex = 7;
			toolTip.SetToolTip(labelModifiedDateValueOnline, "Shows the online modified date");
			labelModifiedDateValueOnline.Values.Text = "00.00.0000 00:00";
			labelModifiedDateValueOnline.DoubleClick += CopyToClipboard_DoubleClick;
			labelModifiedDateValueOnline.Enter += Control_Enter;
			labelModifiedDateValueOnline.Leave += Control_Leave;
			labelModifiedDateValueOnline.MouseDown += Control_MouseDown;
			labelModifiedDateValueOnline.MouseEnter += Control_Enter;
			labelModifiedDateValueOnline.MouseLeave += Control_Leave;
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Groups the data";
			tableLayoutPanel.AccessibleName = "Group pane";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Pane;
			tableLayoutPanel.ColumnCount = 3;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.Controls.Add(labelMpcorbDatLocal, 1, 0);
			tableLayoutPanel.Controls.Add(labelMpcorbDatOnline, 2, 0);
			tableLayoutPanel.Controls.Add(labelContentLengthText, 0, 1);
			tableLayoutPanel.Controls.Add(labelModifiedDateText, 0, 2);
			tableLayoutPanel.Controls.Add(labelContentLengthValueLocal, 1, 1);
			tableLayoutPanel.Controls.Add(labelModifiedDateValueLocal, 1, 2);
			tableLayoutPanel.Controls.Add(labelContentLengthValueOnline, 2, 1);
			tableLayoutPanel.Controls.Add(labelModifiedDateValueOnline, 2, 2);
			tableLayoutPanel.Controls.Add(labelUpdateNeeded, 0, 3);
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.Location = new Point(0, 0);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 4;
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.Size = new Size(369, 109);
			tableLayoutPanel.TabIndex = 0;
			toolTip.SetToolTip(tableLayoutPanel, "Groups the data");
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
			toolStripContainer.ContentPanel.Size = new Size(369, 109);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(369, 131);
			toolStripContainer.TabIndex = 3;
			toolStripContainer.Text = "toolStripContainer";
			toolTip.SetToolTip(toolStripContainer, "Container to arrange the toolbars");
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
			statusStrip.Size = new Size(369, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 0;
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
			// CheckMpcorbDatForm
			// 
			AccessibleDescription = "Shows the informations about the MPCORB.DAT database local and online";
			AccessibleName = "Check MPCORB.DAT";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(369, 131);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "CheckMpcorbDatForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Check MPCORB.DAT";
			toolTip.SetToolTip(this, "Check MPCORB.DAT");
			Load += CheckMpcorbDatForm_Load;
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

		private KryptonToolTip toolTip;
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonLabel labelUpdateNeeded;
		private KryptonLabel labelMpcorbDatLocal;
		private KryptonLabel labelMpcorbDatOnline;
		private KryptonLabel labelContentLengthText;
		private KryptonLabel labelModifiedDateText;
		private KryptonLabel labelContentLengthValueLocal;
		private KryptonLabel labelModifiedDateValueLocal;
		private KryptonLabel labelContentLengthValueOnline;
		private KryptonLabel labelModifiedDateValueOnline;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonManager kryptonManager;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
	}
}