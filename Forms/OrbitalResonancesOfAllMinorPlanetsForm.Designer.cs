using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB;

partial class OrbitalResonancesOfAllMinorPlanetsForm
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(OrbitalResonancesOfAllMinorPlanetsForm));
		kryptonPanel = new KryptonPanel();
		btnStart = new KryptonButton();
		btnCancel = new KryptonButton();
		checkBoxMercury = new KryptonCheckBox();
		checkBoxVenus = new KryptonCheckBox();
		checkBoxEarth = new KryptonCheckBox();
		checkBoxMars = new KryptonCheckBox();
		checkBoxJupiter = new KryptonCheckBox();
		checkBoxSaturn = new KryptonCheckBox();
		checkBoxUranus = new KryptonCheckBox();
		checkBoxNeptune = new KryptonCheckBox();
		kryptonProgressBar = new KryptonProgressBar();
		labelProgress = new KryptonLabel();
		listView = new ListView();
		columnHeaderPlanetoid = new ColumnHeader();
		columnHeaderPlanet = new ColumnHeader();
		columnHeaderPlanetPeriod = new ColumnHeader();
		columnHeaderPlanetoidPeriod = new ColumnHeader();
		columnHeaderRatio = new ColumnHeader();
		columnHeaderResonance = new ColumnHeader();
		columnHeaderDeviation = new ColumnHeader();
		columnHeaderIsResonance = new ColumnHeader();
		contextMenuCopyToClipboard = new ContextMenuStrip(components);
		toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
		statusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);
		((ISupportInitialize)kryptonPanel).BeginInit();
		kryptonPanel.SuspendLayout();
		contextMenuCopyToClipboard.SuspendLayout();
		statusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonPanel
		// 
		kryptonPanel.AccessibleDescription = "Groups the data";
		kryptonPanel.AccessibleName = "Group pane";
		kryptonPanel.AccessibleRole = AccessibleRole.Pane;
		kryptonPanel.Controls.Add(btnStart);
		kryptonPanel.Controls.Add(btnCancel);
		kryptonPanel.Controls.Add(checkBoxMercury);
		kryptonPanel.Controls.Add(checkBoxVenus);
		kryptonPanel.Controls.Add(checkBoxEarth);
		kryptonPanel.Controls.Add(checkBoxMars);
		kryptonPanel.Controls.Add(checkBoxJupiter);
		kryptonPanel.Controls.Add(checkBoxSaturn);
		kryptonPanel.Controls.Add(checkBoxUranus);
		kryptonPanel.Controls.Add(checkBoxNeptune);
		kryptonPanel.Controls.Add(kryptonProgressBar);
		kryptonPanel.Controls.Add(labelProgress);
		kryptonPanel.Controls.Add(listView);
		kryptonPanel.Controls.Add(statusStrip);
		kryptonPanel.Dock = DockStyle.Fill;
		kryptonPanel.Location = new Point(0, 0);
		kryptonPanel.Margin = new Padding(4, 3, 4, 3);
		kryptonPanel.Name = "kryptonPanel";
		kryptonPanel.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanel.Size = new Size(904, 620);
		kryptonPanel.TabIndex = 0;
		kryptonPanel.TabStop = true;
		// 
		// btnStart
		// 
		btnStart.AccessibleDescription = "Starts the orbital resonance search for all minor planets";
		btnStart.AccessibleName = "Start search";
		btnStart.AccessibleRole = AccessibleRole.PushButton;
		btnStart.Location = new Point(12, 12);
		btnStart.Name = "btnStart";
		btnStart.Size = new Size(90, 25);
		btnStart.TabIndex = 0;
		btnStart.Values.DropDownArrowColor = Color.Empty;
		btnStart.Values.Text = "&Start Search";
		btnStart.Click += BtnStart_Click;
		btnStart.MouseEnter += Control_Enter;
		btnStart.MouseLeave += Control_Leave;
		// 
		// btnCancel
		// 
		btnCancel.AccessibleDescription = "Cancels the orbital resonance search";
		btnCancel.AccessibleName = "Cancel search";
		btnCancel.AccessibleRole = AccessibleRole.PushButton;
		btnCancel.Enabled = false;
		btnCancel.Location = new Point(108, 12);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new Size(90, 25);
		btnCancel.TabIndex = 1;
		btnCancel.Values.DropDownArrowColor = Color.Empty;
		btnCancel.Values.Text = "&Cancel";
		btnCancel.Click += BtnCancel_Click;
		btnCancel.MouseEnter += Control_Enter;
		btnCancel.MouseLeave += Control_Leave;
		// 
		// checkBoxMercury
		// 
		checkBoxMercury.AccessibleDescription = "Includes Mercury in the resonance search";
		checkBoxMercury.AccessibleName = "Mercury";
		checkBoxMercury.Checked = true;
		checkBoxMercury.CheckState = CheckState.Checked;
		checkBoxMercury.Location = new Point(225, 14);
		checkBoxMercury.Name = "checkBoxMercury";
		checkBoxMercury.Size = new Size(110, 20);
		checkBoxMercury.TabIndex = 2;
		checkBoxMercury.Values.Text = "Mercury";
		checkBoxMercury.MouseEnter += Control_Enter;
		checkBoxMercury.MouseLeave += Control_Leave;
		// 
		// checkBoxVenus
		// 
		checkBoxVenus.AccessibleDescription = "Includes Venus in the resonance search";
		checkBoxVenus.AccessibleName = "Venus";
		checkBoxVenus.Checked = true;
		checkBoxVenus.CheckState = CheckState.Checked;
		checkBoxVenus.Location = new Point(341, 14);
		checkBoxVenus.Name = "checkBoxVenus";
		checkBoxVenus.Size = new Size(90, 20);
		checkBoxVenus.TabIndex = 3;
		checkBoxVenus.Values.Text = "Venus";
		checkBoxVenus.MouseEnter += Control_Enter;
		checkBoxVenus.MouseLeave += Control_Leave;
		// 
		// checkBoxEarth
		// 
		checkBoxEarth.AccessibleDescription = "Includes Earth in the resonance search";
		checkBoxEarth.AccessibleName = "Earth";
		checkBoxEarth.Checked = true;
		checkBoxEarth.CheckState = CheckState.Checked;
		checkBoxEarth.Location = new Point(437, 14);
		checkBoxEarth.Name = "checkBoxEarth";
		checkBoxEarth.Size = new Size(85, 20);
		checkBoxEarth.TabIndex = 4;
		checkBoxEarth.Values.Text = "Earth";
		checkBoxEarth.MouseEnter += Control_Enter;
		checkBoxEarth.MouseLeave += Control_Leave;
		// 
		// checkBoxMars
		// 
		checkBoxMars.AccessibleDescription = "Includes Mars in the resonance search";
		checkBoxMars.AccessibleName = "Mars";
		checkBoxMars.Checked = true;
		checkBoxMars.CheckState = CheckState.Checked;
		checkBoxMars.Location = new Point(528, 14);
		checkBoxMars.Name = "checkBoxMars";
		checkBoxMars.Size = new Size(85, 20);
		checkBoxMars.TabIndex = 5;
		checkBoxMars.Values.Text = "Mars";
		checkBoxMars.MouseEnter += Control_Enter;
		checkBoxMars.MouseLeave += Control_Leave;
		// 
		// checkBoxJupiter
		// 
		checkBoxJupiter.AccessibleDescription = "Includes Jupiter in the resonance search";
		checkBoxJupiter.AccessibleName = "Jupiter";
		checkBoxJupiter.Checked = true;
		checkBoxJupiter.CheckState = CheckState.Checked;
		checkBoxJupiter.Location = new Point(225, 40);
		checkBoxJupiter.Name = "checkBoxJupiter";
		checkBoxJupiter.Size = new Size(110, 20);
		checkBoxJupiter.TabIndex = 6;
		checkBoxJupiter.Values.Text = "Jupiter";
		checkBoxJupiter.MouseEnter += Control_Enter;
		checkBoxJupiter.MouseLeave += Control_Leave;
		// 
		// checkBoxSaturn
		// 
		checkBoxSaturn.AccessibleDescription = "Includes Saturn in the resonance search";
		checkBoxSaturn.AccessibleName = "Saturn";
		checkBoxSaturn.Checked = true;
		checkBoxSaturn.CheckState = CheckState.Checked;
		checkBoxSaturn.Location = new Point(341, 40);
		checkBoxSaturn.Name = "checkBoxSaturn";
		checkBoxSaturn.Size = new Size(90, 20);
		checkBoxSaturn.TabIndex = 7;
		checkBoxSaturn.Values.Text = "Saturn";
		checkBoxSaturn.MouseEnter += Control_Enter;
		checkBoxSaturn.MouseLeave += Control_Leave;
		// 
		// checkBoxUranus
		// 
		checkBoxUranus.AccessibleDescription = "Includes Uranus in the resonance search";
		checkBoxUranus.AccessibleName = "Uranus";
		checkBoxUranus.Checked = true;
		checkBoxUranus.CheckState = CheckState.Checked;
		checkBoxUranus.Location = new Point(437, 40);
		checkBoxUranus.Name = "checkBoxUranus";
		checkBoxUranus.Size = new Size(90, 20);
		checkBoxUranus.TabIndex = 8;
		checkBoxUranus.Values.Text = "Uranus";
		checkBoxUranus.MouseEnter += Control_Enter;
		checkBoxUranus.MouseLeave += Control_Leave;
		// 
		// checkBoxNeptune
		// 
		checkBoxNeptune.AccessibleDescription = "Includes Neptune in the resonance search";
		checkBoxNeptune.AccessibleName = "Neptune";
		checkBoxNeptune.Checked = true;
		checkBoxNeptune.CheckState = CheckState.Checked;
		checkBoxNeptune.Location = new Point(528, 40);
		checkBoxNeptune.Name = "checkBoxNeptune";
		checkBoxNeptune.Size = new Size(100, 20);
		checkBoxNeptune.TabIndex = 9;
		checkBoxNeptune.Values.Text = "Neptune";
		checkBoxNeptune.MouseEnter += Control_Enter;
		checkBoxNeptune.MouseLeave += Control_Leave;
		// 
		// kryptonProgressBar
		// 
		kryptonProgressBar.AccessibleDescription = "Shows the progress of the orbital resonance search";
		kryptonProgressBar.AccessibleName = "Search progress";
		kryptonProgressBar.AccessibleRole = AccessibleRole.ProgressBar;
		kryptonProgressBar.Location = new Point(12, 72);
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(840, 23);
		kryptonProgressBar.TabIndex = 10;
		kryptonProgressBar.Text = "0%";
		kryptonProgressBar.TextBackdropColor = Color.Empty;
		kryptonProgressBar.TextShadowColor = Color.Empty;
		kryptonProgressBar.Values.Text = "0%";
		// 
		// labelProgress
		// 
		labelProgress.AccessibleDescription = "Shows the progress as a percentage";
		labelProgress.AccessibleName = "Progress percentage";
		labelProgress.Location = new Point(858, 73);
		labelProgress.Name = "labelProgress";
		labelProgress.Size = new Size(30, 20);
		labelProgress.TabIndex = 11;
		labelProgress.Values.Text = "0%";
		// 
		// listView
		// 
		listView.AccessibleDescription = "Shows the list of orbital resonances of all planetoids relative to the solar system planets";
		listView.AccessibleName = "Orbital resonances of all minor planets list";
		listView.AccessibleRole = AccessibleRole.List;
		listView.AllowColumnReorder = true;
		listView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		listView.Columns.AddRange(new ColumnHeader[] { columnHeaderPlanetoid, columnHeaderPlanet, columnHeaderPlanetPeriod, columnHeaderPlanetoidPeriod, columnHeaderRatio, columnHeaderResonance, columnHeaderDeviation, columnHeaderIsResonance });
		listView.ContextMenuStrip = contextMenuCopyToClipboard;
		listView.Font = new Font("Segoe UI", 9F);
		listView.FullRowSelect = true;
		listView.GridLines = true;
		listView.Location = new Point(12, 103);
		listView.Margin = new Padding(4, 3, 4, 3);
		listView.Name = "listView";
		listView.ShowItemToolTips = true;
		listView.Size = new Size(880, 493);
		listView.TabIndex = 12;
		listView.UseCompatibleStateImageBehavior = false;
		listView.View = View.Details;
		listView.VirtualMode = true;
		listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		listView.Enter += Control_Enter;
		listView.Leave += Control_Leave;
		listView.MouseEnter += Control_Enter;
		listView.MouseLeave += Control_Leave;
		// 
		// columnHeaderPlanetoid
		// 
		columnHeaderPlanetoid.Text = "Planetoid";
		columnHeaderPlanetoid.Width = 200;
		// 
		// columnHeaderPlanet
		// 
		columnHeaderPlanet.Text = "Planet";
		columnHeaderPlanet.Width = 90;
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
		contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCopyToClipboard });
		contextMenuCopyToClipboard.Name = "contextMenuCopyToClipboard";
		contextMenuCopyToClipboard.Size = new Size(214, 26);
		contextMenuCopyToClipboard.TabStop = true;
		contextMenuCopyToClipboard.Text = "Copy to clipboard";
		contextMenuCopyToClipboard.MouseEnter += Control_Enter;
		contextMenuCopyToClipboard.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemCopyToClipboard
		// 
		toolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the selected row to the clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemCopyToClipboard.AutoToolTip = true;
		toolStripMenuItemCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
		toolStripMenuItemCopyToClipboard.Name = "toolStripMenuItemCopyToClipboard";
		toolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
		toolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
		toolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
		toolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
		toolStripMenuItemCopyToClipboard.Click += ToolStripMenuItemCopyToClipboard_Click;
		toolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
		toolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
		// 
		// statusStrip
		// 
		statusStrip.AccessibleDescription = "Shows some information";
		statusStrip.AccessibleName = "Status bar of some information";
		statusStrip.AccessibleRole = AccessibleRole.StatusBar;
		statusStrip.Font = new Font("Segoe UI", 9F);
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		statusStrip.Location = new Point(0, 598);
		statusStrip.Name = "statusStrip";
		statusStrip.Padding = new Padding(1, 0, 16, 0);
		statusStrip.ProgressBars = null;
		statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		statusStrip.ShowItemToolTips = true;
		statusStrip.Size = new Size(904, 22);
		statusStrip.SizingGrip = false;
		statusStrip.TabIndex = 13;
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
		// OrbitalResonancesOfAllMinorPlanetsForm
		// 
		AccessibleDescription = "Shows orbital resonances of all minor planets relative to the 8 solar system planets";
		AccessibleName = "Orbital resonances of all minor planets";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(904, 620);
		ControlBox = false;
		Controls.Add(kryptonPanel);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		MinimumSize = new Size(920, 620);
		Name = "OrbitalResonancesOfAllMinorPlanetsForm";
		ShowInTaskbar = false;
		StartPosition = FormStartPosition.CenterParent;
		Text = "Orbital resonances of all minor planets";
		FormClosing += OrbitalResonancesOfAllMinorPlanetsForm_FormClosing;
		Load += OrbitalResonancesOfAllMinorPlanetsForm_Load;
		((ISupportInitialize)kryptonPanel).EndInit();
		kryptonPanel.ResumeLayout(false);
		kryptonPanel.PerformLayout();
		contextMenuCopyToClipboard.ResumeLayout(false);
		statusStrip.ResumeLayout(false);
		statusStrip.PerformLayout();
		ResumeLayout(false);
	}

	#endregion

	private KryptonPanel kryptonPanel;
	private KryptonButton btnStart;
	private KryptonButton btnCancel;
	private KryptonCheckBox checkBoxMercury;
	private KryptonCheckBox checkBoxVenus;
	private KryptonCheckBox checkBoxEarth;
	private KryptonCheckBox checkBoxMars;
	private KryptonCheckBox checkBoxJupiter;
	private KryptonCheckBox checkBoxSaturn;
	private KryptonCheckBox checkBoxUranus;
	private KryptonCheckBox checkBoxNeptune;
	private KryptonProgressBar kryptonProgressBar;
	private KryptonLabel labelProgress;
	private ListView listView;
	private ColumnHeader columnHeaderPlanetoid;
	private ColumnHeader columnHeaderPlanet;
	private ColumnHeader columnHeaderPlanetPeriod;
	private ColumnHeader columnHeaderPlanetoidPeriod;
	private ColumnHeader columnHeaderRatio;
	private ColumnHeader columnHeaderResonance;
	private ColumnHeader columnHeaderDeviation;
	private ColumnHeader columnHeaderIsResonance;
	private ContextMenuStrip contextMenuCopyToClipboard;
	private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
	private KryptonStatusStrip statusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
}
