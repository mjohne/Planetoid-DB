using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class OrbitalResonanceForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(OrbitalResonanceForm));
			panel = new KryptonPanel();
			listView = new ListView();
			columnHeaderPlanet = new ColumnHeader();
			columnHeaderPlanetPeriod = new ColumnHeader();
			columnHeaderPlanetoidPeriod = new ColumnHeader();
			columnHeaderRatio = new ColumnHeader();
			columnHeaderResonance = new ColumnHeader();
			columnHeaderDeviation = new ColumnHeader();
			columnHeaderIsResonance = new ColumnHeader();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "Group pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(listView);
			panel.Controls.Add(statusStrip);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(729, 227);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// listView
			// 
			listView.AccessibleDescription = "Shows the list of orbital resonances relative to the solar system planets";
			listView.AccessibleName = "Orbital resonances list";
			listView.AccessibleRole = AccessibleRole.List;
			listView.AllowColumnReorder = true;
			listView.Columns.AddRange(new ColumnHeader[] { columnHeaderPlanet, columnHeaderPlanetPeriod, columnHeaderPlanetoidPeriod, columnHeaderRatio, columnHeaderResonance, columnHeaderDeviation, columnHeaderIsResonance });
			listView.ContextMenuStrip = contextMenuCopyToClipboard;
			listView.Dock = DockStyle.Fill;
			listView.Font = new Font("Segoe UI", 9F);
			listView.FullRowSelect = true;
			listView.GridLines = true;
			listView.Location = new Point(0, 0);
			listView.Margin = new Padding(4, 3, 4, 3);
			listView.Name = "listView";
			listView.ShowItemToolTips = true;
			listView.Size = new Size(729, 205);
			listView.TabIndex = 0;
			listView.UseCompatibleStateImageBehavior = false;
			listView.View = View.Details;
			listView.Enter += Control_Enter;
			listView.Leave += Control_Leave;
			listView.MouseEnter += Control_Enter;
			listView.MouseLeave += Control_Leave;
			// 
			// columnHeaderPlanet
			// 
			columnHeaderPlanet.Text = "Planet";
			columnHeaderPlanet.Width = 120;
			// 
			// columnHeaderPlanetPeriod
			// 
			columnHeaderPlanetPeriod.Text = "Planet Period (yr)";
			columnHeaderPlanetPeriod.TextAlign = HorizontalAlignment.Right;
			columnHeaderPlanetPeriod.Width = 120;
			// 
			// columnHeaderPlanetoidPeriod
			// 
			columnHeaderPlanetoidPeriod.Text = "Planetoid Period (yr)";
			columnHeaderPlanetoidPeriod.TextAlign = HorizontalAlignment.Right;
			columnHeaderPlanetoidPeriod.Width = 130;
			// 
			// columnHeaderRatio
			// 
			columnHeaderRatio.Text = "Ratio";
			columnHeaderRatio.TextAlign = HorizontalAlignment.Right;
			columnHeaderRatio.Width = 80;
			// 
			// columnHeaderResonance
			// 
			columnHeaderResonance.Text = "Resonance";
			columnHeaderResonance.TextAlign = HorizontalAlignment.Center;
			columnHeaderResonance.Width = 90;
			// 
			// columnHeaderDeviation
			// 
			columnHeaderDeviation.Text = "Deviation (%)";
			columnHeaderDeviation.TextAlign = HorizontalAlignment.Right;
			columnHeaderDeviation.Width = 90;
			// 
			// columnHeaderIsResonance
			// 
			columnHeaderIsResonance.Text = "Is Resonance";
			columnHeaderIsResonance.TextAlign = HorizontalAlignment.Center;
			columnHeaderIsResonance.Width = 90;
			// 
			// contextMenuCopyToClipboard
			// 
			contextMenuCopyToClipboard.AccessibleDescription = "Shows the context menu for copying information to the clipboard";
			contextMenuCopyToClipboard.AccessibleName = "Context menu for copying information to the clipboard";
			contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuCopyToClipboard.AllowClickThrough = true;
			contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { ToolStripMenuItemCopyToClipboard });
			contextMenuCopyToClipboard.Name = "contextMenuStrip";
			contextMenuCopyToClipboard.Size = new Size(214, 26);
			contextMenuCopyToClipboard.TabStop = true;
			contextMenuCopyToClipboard.Text = "Copy to clipboard";
			contextMenuCopyToClipboard.MouseEnter += Control_Enter;
			contextMenuCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// ToolStripMenuItemCopyToClipboard
			// 
			ToolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the selected row to the clipboard";
			ToolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
			ToolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
			ToolStripMenuItemCopyToClipboard.AutoToolTip = true;
			ToolStripMenuItemCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			ToolStripMenuItemCopyToClipboard.Name = "ToolStripMenuItemCopyToClipboard";
			ToolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			ToolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			ToolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
			ToolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
			ToolStripMenuItemCopyToClipboard.Click += ToolStripMenuItemCopyToClipboard_Click;
			ToolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
			ToolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 205);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.ShowItemToolTips = true;
			statusStrip.Size = new Size(729, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 1;
			statusStrip.TabStop = true;
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
			// OrbitalResonanceForm
			// 
			AccessibleDescription = "Shows orbital resonances of the planetoid relative to the 8 planets";
			AccessibleName = "Orbital resonances";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(729, 227);
			ControlBox = false;
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "OrbitalResonanceForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Orbital resonances";
			Load += OrbitalResonanceForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			contextMenuCopyToClipboard.ResumeLayout(false);
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private KryptonPanel panel;
		private ListView listView;
		private ColumnHeader columnHeaderPlanet;
		private ColumnHeader columnHeaderPlanetPeriod;
		private ColumnHeader columnHeaderPlanetoidPeriod;
		private ColumnHeader columnHeaderRatio;
		private ColumnHeader columnHeaderResonance;
		private ColumnHeader columnHeaderDeviation;
		private ColumnHeader columnHeaderIsResonance;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
	}
}
