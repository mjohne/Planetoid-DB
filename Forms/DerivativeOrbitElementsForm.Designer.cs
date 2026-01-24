using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class DerivativeOrbitElementsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(DerivativeOrbitElementsForm));
			toolTip = new ToolTip(components);
			labelLinearEccentricityDesc = new KryptonLabel();
			contextMenuOpenTerminology = new ContextMenuStrip(components);
			toolStripMenuItemOpenTerminology = new ToolStripMenuItem();
			labelLinearEccentricityData = new KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCpyToClipboard = new ToolStripMenuItem();
			labelSemiMinorAxisDesc = new KryptonLabel();
			labelMajorAxisDesc = new KryptonLabel();
			labelMinorAxisDesc = new KryptonLabel();
			labelEccenctricAnomalyDesc = new KryptonLabel();
			labelTrueAnomalyDesc = new KryptonLabel();
			labelPerihelionDistanceDesc = new KryptonLabel();
			LabelAphelionDistanceDesc = new KryptonLabel();
			labelLongitudeDescendingNodeDesc = new KryptonLabel();
			labelArgumentAphelionDesc = new KryptonLabel();
			labelFocalParameterDesc = new KryptonLabel();
			labelSemiLatusRectumDesc = new KryptonLabel();
			labelLatusRectumDesc = new KryptonLabel();
			labelOrbitalPeriodDesc = new KryptonLabel();
			labelSemiMinorAxisData = new KryptonLabel();
			labelMajorAxisData = new KryptonLabel();
			labelMinorAxisData = new KryptonLabel();
			labelEccentricAnomalyData = new KryptonLabel();
			labelTrueAnomalyData = new KryptonLabel();
			labelPerihelionDistanceData = new KryptonLabel();
			labelAphelionDistanceData = new KryptonLabel();
			labelLongitudeDescendingNodeData = new KryptonLabel();
			labelArgumentAphelionData = new KryptonLabel();
			labelFocalParameterData = new KryptonLabel();
			labelSemiLatusRectumData = new KryptonLabel();
			labelLatusRectumData = new KryptonLabel();
			labelOrbitalPeriodData = new KryptonLabel();
			labelOrbitalAreaDesc = new KryptonLabel();
			labelOrbitalPerimeterDesc = new KryptonLabel();
			labelSemiMeanAxisDesc = new KryptonLabel();
			labelMeanAxisDesc = new KryptonLabel();
			labelOrbitalAreaData = new KryptonLabel();
			labelOrbitalPerimeterData = new KryptonLabel();
			labelSemiMeanAxisData = new KryptonLabel();
			labelStandardGravitationalParameterDesc = new KryptonLabel();
			labelMeanAxisData = new KryptonLabel();
			labelStandardGravitationalParameterData = new KryptonLabel();
			toolStripContainer = new ToolStripContainer();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			tableLayoutPanel = new KryptonTableLayoutPanel();
			toolStripIcons = new ToolStrip();
			splitbuttonCopyToClipboard = new ToolStripSplitButton();
			contextMenuFullCopyToClipboardDerivatedOrbitalElements = new ContextMenuStrip(components);
			menuitemCopyToClipboardLinearEccentricity = new ToolStripMenuItem();
			menuitemCopyToClipboardSemiMinorAxis = new ToolStripMenuItem();
			menuitemCopyToClipboardMajorAxis = new ToolStripMenuItem();
			menuitemCopyToClipboardMinorAxis = new ToolStripMenuItem();
			menuitemCopyToClipboardEccentricAnomaly = new ToolStripMenuItem();
			menuitemCopyToClipboardTrueAnomaly = new ToolStripMenuItem();
			menuitemCopyToClipboardPerihelionDistance = new ToolStripMenuItem();
			menuitemCopyToClipboardAphelionDistance = new ToolStripMenuItem();
			menuitemCopyToClipboardLongitudeDescendingNode = new ToolStripMenuItem();
			menuitemCopyToClipboardArgumentAphelion = new ToolStripMenuItem();
			menuitemCopyToClipboardFocalParameter = new ToolStripMenuItem();
			menuitemCopyToClipboardSemiLatusRectum = new ToolStripMenuItem();
			menuitemCopyToClipboardLatusRectum = new ToolStripMenuItem();
			menuitemCopyToClipboardOrbitalPeriod = new ToolStripMenuItem();
			menuitemCopyToClipboardOrbitalArea = new ToolStripMenuItem();
			menuitemCopyToClipboardSemiMeanAxis = new ToolStripMenuItem();
			menuitemCopyToClipboardMeanAxis = new ToolStripMenuItem();
			menuitemCopyToClipboardStandardGravitationalParameter = new ToolStripMenuItem();
			kryptonManager = new KryptonManager(components);
			contextMenuOpenTerminology.SuspendLayout();
			contextMenuCopyToClipboard.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			statusStrip.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			toolStripIcons.SuspendLayout();
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.SuspendLayout();
			SuspendLayout();
			// 
			// labelLinearEccentricityDesc
			// 
			labelLinearEccentricityDesc.AccessibleDescription = "Linear eccentricity (AU)";
			labelLinearEccentricityDesc.AccessibleName = "Linear eccentricity (AU)";
			labelLinearEccentricityDesc.AccessibleRole = AccessibleRole.StaticText;
			labelLinearEccentricityDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelLinearEccentricityDesc.Dock = DockStyle.Fill;
			labelLinearEccentricityDesc.LabelStyle = LabelStyle.BoldPanel;
			labelLinearEccentricityDesc.Location = new Point(4, 3);
			labelLinearEccentricityDesc.Margin = new Padding(4, 3, 4, 3);
			labelLinearEccentricityDesc.Name = "labelLinearEccentricityDesc";
			labelLinearEccentricityDesc.Size = new Size(268, 20);
			labelLinearEccentricityDesc.TabIndex = 0;
			labelLinearEccentricityDesc.Tag = "20";
			toolTip.SetToolTip(labelLinearEccentricityDesc, "Linear eccentricity (AU)");
			labelLinearEccentricityDesc.Values.ExtraText = "AU";
			labelLinearEccentricityDesc.Values.Text = "Linear eccentricity";
			labelLinearEccentricityDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLinearEccentricityDesc.Enter += SetStatusBar_Enter;
			labelLinearEccentricityDesc.Leave += ClearStatusBar_Leave;
			labelLinearEccentricityDesc.MouseDown += Control_MouseDown;
			labelLinearEccentricityDesc.MouseEnter += SetStatusBar_Enter;
			labelLinearEccentricityDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// contextMenuOpenTerminology
			// 
			contextMenuOpenTerminology.AccessibleDescription = "Shows context menu for opening the terminology by id";
			contextMenuOpenTerminology.AccessibleName = "Context menu for opening the terminology by id";
			contextMenuOpenTerminology.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuOpenTerminology.AllowClickThrough = true;
			contextMenuOpenTerminology.Font = new Font("Segoe UI", 9F);
			contextMenuOpenTerminology.Items.AddRange(new ToolStripItem[] { toolStripMenuItemOpenTerminology });
			contextMenuOpenTerminology.Name = "contextMenuStrip";
			contextMenuOpenTerminology.Size = new Size(250, 26);
			contextMenuOpenTerminology.TabStop = true;
			contextMenuOpenTerminology.Text = "ContextMenu";
			toolTip.SetToolTip(contextMenuOpenTerminology, "Context menu for opening the terminology by id");
			contextMenuOpenTerminology.MouseEnter += SetStatusBar_Enter;
			contextMenuOpenTerminology.MouseLeave += ClearStatusBar_Leave;
			// 
			// toolStripMenuItemOpenTerminology
			// 
			toolStripMenuItemOpenTerminology.AccessibleDescription = "Opens the term in the terminology";
			toolStripMenuItemOpenTerminology.AccessibleName = "Open in the terminology";
			toolStripMenuItemOpenTerminology.AccessibleRole = AccessibleRole.MenuItem;
			toolStripMenuItemOpenTerminology.AutoToolTip = true;
			toolStripMenuItemOpenTerminology.Image = FatcowIcons16px.fatcow_text_list_bullets_16px;
			toolStripMenuItemOpenTerminology.Name = "toolStripMenuItemOpenTerminology";
			toolStripMenuItemOpenTerminology.ShortcutKeyDisplayString = "Strg+O";
			toolStripMenuItemOpenTerminology.ShortcutKeys = Keys.Control | Keys.O;
			toolStripMenuItemOpenTerminology.Size = new Size(249, 22);
			toolStripMenuItemOpenTerminology.Text = "&Open in the terminology";
			toolStripMenuItemOpenTerminology.Click += OpenTerminology_DoubleClick;
			toolStripMenuItemOpenTerminology.MouseEnter += SetStatusBar_Enter;
			toolStripMenuItemOpenTerminology.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLinearEccentricityData
			// 
			labelLinearEccentricityData.AccessibleDescription = "Shows the information of \"Linear eccentricity\"";
			labelLinearEccentricityData.AccessibleName = "Shows the information of \"Linear eccentricity\"";
			labelLinearEccentricityData.AccessibleRole = AccessibleRole.StaticText;
			labelLinearEccentricityData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLinearEccentricityData.Dock = DockStyle.Fill;
			labelLinearEccentricityData.Location = new Point(280, 3);
			labelLinearEccentricityData.Margin = new Padding(4, 3, 4, 3);
			labelLinearEccentricityData.Name = "labelLinearEccentricityData";
			labelLinearEccentricityData.Size = new Size(270, 20);
			labelLinearEccentricityData.TabIndex = 1;
			toolTip.SetToolTip(labelLinearEccentricityData, "Shows the information of \"Linear eccentricity\"");
			labelLinearEccentricityData.Values.Text = "..................";
			labelLinearEccentricityData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLinearEccentricityData.Enter += SetStatusBar_Enter;
			labelLinearEccentricityData.Leave += ClearStatusBar_Leave;
			labelLinearEccentricityData.MouseDown += Control_MouseDown;
			labelLinearEccentricityData.MouseEnter += SetStatusBar_Enter;
			labelLinearEccentricityData.MouseLeave += ClearStatusBar_Leave;
			// 
			// contextMenuCopyToClipboard
			// 
			contextMenuCopyToClipboard.AccessibleDescription = "Shows context menu for some options";
			contextMenuCopyToClipboard.AccessibleName = "Some options";
			contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuCopyToClipboard.AllowClickThrough = true;
			contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
			contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { ToolStripMenuItemCpyToClipboard });
			contextMenuCopyToClipboard.Name = "contextMenuStrip";
			contextMenuCopyToClipboard.Size = new Size(214, 26);
			contextMenuCopyToClipboard.TabStop = true;
			contextMenuCopyToClipboard.Text = "ContextMenu";
			toolTip.SetToolTip(contextMenuCopyToClipboard, "Context menu for copying to clipboard");
			contextMenuCopyToClipboard.MouseEnter += SetStatusBar_Enter;
			contextMenuCopyToClipboard.MouseLeave += ClearStatusBar_Leave;
			// 
			// ToolStripMenuItemCpyToClipboard
			// 
			ToolStripMenuItemCpyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
			ToolStripMenuItemCpyToClipboard.AccessibleName = "Copy to clipboard";
			ToolStripMenuItemCpyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
			ToolStripMenuItemCpyToClipboard.AutoToolTip = true;
			ToolStripMenuItemCpyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			ToolStripMenuItemCpyToClipboard.Name = "ToolStripMenuItemCpyToClipboard";
			ToolStripMenuItemCpyToClipboard.ShortcutKeyDisplayString = "Strg+C";
			ToolStripMenuItemCpyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
			ToolStripMenuItemCpyToClipboard.Size = new Size(213, 22);
			ToolStripMenuItemCpyToClipboard.Text = "&Copy to clipboard";
			ToolStripMenuItemCpyToClipboard.Click += CopyToClipboard_DoubleClick;
			ToolStripMenuItemCpyToClipboard.MouseEnter += SetStatusBar_Enter;
			ToolStripMenuItemCpyToClipboard.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiMinorAxisDesc
			// 
			labelSemiMinorAxisDesc.AccessibleDescription = "Semi-minor axis (AU)";
			labelSemiMinorAxisDesc.AccessibleName = "Semi-minor axis (AU)";
			labelSemiMinorAxisDesc.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMinorAxisDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelSemiMinorAxisDesc.Dock = DockStyle.Fill;
			labelSemiMinorAxisDesc.LabelStyle = LabelStyle.BoldPanel;
			labelSemiMinorAxisDesc.Location = new Point(4, 29);
			labelSemiMinorAxisDesc.Margin = new Padding(4, 3, 4, 3);
			labelSemiMinorAxisDesc.Name = "labelSemiMinorAxisDesc";
			labelSemiMinorAxisDesc.Size = new Size(268, 20);
			labelSemiMinorAxisDesc.TabIndex = 2;
			labelSemiMinorAxisDesc.Tag = "21";
			toolTip.SetToolTip(labelSemiMinorAxisDesc, "Semi-minor axis (AU)");
			labelSemiMinorAxisDesc.Values.ExtraText = "AU";
			labelSemiMinorAxisDesc.Values.Text = "Semi-minor axis";
			labelSemiMinorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiMinorAxisDesc.Enter += SetStatusBar_Enter;
			labelSemiMinorAxisDesc.Leave += ClearStatusBar_Leave;
			labelSemiMinorAxisDesc.MouseDown += Control_MouseDown;
			labelSemiMinorAxisDesc.MouseEnter += SetStatusBar_Enter;
			labelSemiMinorAxisDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMajorAxisDesc
			// 
			labelMajorAxisDesc.AccessibleDescription = "Major axis (AU)";
			labelMajorAxisDesc.AccessibleName = "Major axis (AU)";
			labelMajorAxisDesc.AccessibleRole = AccessibleRole.StaticText;
			labelMajorAxisDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelMajorAxisDesc.Dock = DockStyle.Fill;
			labelMajorAxisDesc.LabelStyle = LabelStyle.BoldPanel;
			labelMajorAxisDesc.Location = new Point(4, 55);
			labelMajorAxisDesc.Margin = new Padding(4, 3, 4, 3);
			labelMajorAxisDesc.Name = "labelMajorAxisDesc";
			labelMajorAxisDesc.Size = new Size(268, 20);
			labelMajorAxisDesc.TabIndex = 4;
			labelMajorAxisDesc.Tag = "22";
			toolTip.SetToolTip(labelMajorAxisDesc, "Major axis (AU)");
			labelMajorAxisDesc.Values.ExtraText = "AU";
			labelMajorAxisDesc.Values.Text = "Major axis";
			labelMajorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMajorAxisDesc.Enter += SetStatusBar_Enter;
			labelMajorAxisDesc.Leave += ClearStatusBar_Leave;
			labelMajorAxisDesc.MouseDown += Control_MouseDown;
			labelMajorAxisDesc.MouseEnter += SetStatusBar_Enter;
			labelMajorAxisDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMinorAxisDesc
			// 
			labelMinorAxisDesc.AccessibleDescription = "Minor axis (AU)";
			labelMinorAxisDesc.AccessibleName = "Minor axis (AU)";
			labelMinorAxisDesc.AccessibleRole = AccessibleRole.StaticText;
			labelMinorAxisDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelMinorAxisDesc.Dock = DockStyle.Fill;
			labelMinorAxisDesc.LabelStyle = LabelStyle.BoldPanel;
			labelMinorAxisDesc.Location = new Point(4, 81);
			labelMinorAxisDesc.Margin = new Padding(4, 3, 4, 3);
			labelMinorAxisDesc.Name = "labelMinorAxisDesc";
			labelMinorAxisDesc.Size = new Size(268, 20);
			labelMinorAxisDesc.TabIndex = 6;
			labelMinorAxisDesc.Tag = "23";
			toolTip.SetToolTip(labelMinorAxisDesc, "Minor axis (AU)");
			labelMinorAxisDesc.Values.ExtraText = "AU";
			labelMinorAxisDesc.Values.Text = "Minor axis";
			labelMinorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMinorAxisDesc.Enter += SetStatusBar_Enter;
			labelMinorAxisDesc.Leave += ClearStatusBar_Leave;
			labelMinorAxisDesc.MouseDown += Control_MouseDown;
			labelMinorAxisDesc.MouseEnter += SetStatusBar_Enter;
			labelMinorAxisDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelEccenctricAnomalyDesc
			// 
			labelEccenctricAnomalyDesc.AccessibleDescription = "Eccentric anomaly (degrees)";
			labelEccenctricAnomalyDesc.AccessibleName = "Eccentric anomaly (degrees)";
			labelEccenctricAnomalyDesc.AccessibleRole = AccessibleRole.StaticText;
			labelEccenctricAnomalyDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelEccenctricAnomalyDesc.Dock = DockStyle.Fill;
			labelEccenctricAnomalyDesc.LabelStyle = LabelStyle.BoldPanel;
			labelEccenctricAnomalyDesc.Location = new Point(4, 107);
			labelEccenctricAnomalyDesc.Margin = new Padding(4, 3, 4, 3);
			labelEccenctricAnomalyDesc.Name = "labelEccenctricAnomalyDesc";
			labelEccenctricAnomalyDesc.Size = new Size(268, 20);
			labelEccenctricAnomalyDesc.TabIndex = 8;
			labelEccenctricAnomalyDesc.Tag = "24";
			toolTip.SetToolTip(labelEccenctricAnomalyDesc, "Eccentric anomaly (°)");
			labelEccenctricAnomalyDesc.Values.ExtraText = "°";
			labelEccenctricAnomalyDesc.Values.Text = "Eccentric anomaly";
			labelEccenctricAnomalyDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelEccenctricAnomalyDesc.Enter += SetStatusBar_Enter;
			labelEccenctricAnomalyDesc.Leave += ClearStatusBar_Leave;
			labelEccenctricAnomalyDesc.MouseDown += Control_MouseDown;
			labelEccenctricAnomalyDesc.MouseEnter += SetStatusBar_Enter;
			labelEccenctricAnomalyDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelTrueAnomalyDesc
			// 
			labelTrueAnomalyDesc.AccessibleDescription = "True anomaly (degrees)";
			labelTrueAnomalyDesc.AccessibleName = "True anomaly (degrees)";
			labelTrueAnomalyDesc.AccessibleRole = AccessibleRole.StaticText;
			labelTrueAnomalyDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelTrueAnomalyDesc.Dock = DockStyle.Fill;
			labelTrueAnomalyDesc.LabelStyle = LabelStyle.BoldPanel;
			labelTrueAnomalyDesc.Location = new Point(4, 133);
			labelTrueAnomalyDesc.Margin = new Padding(4, 3, 4, 3);
			labelTrueAnomalyDesc.Name = "labelTrueAnomalyDesc";
			labelTrueAnomalyDesc.Size = new Size(268, 20);
			labelTrueAnomalyDesc.TabIndex = 10;
			labelTrueAnomalyDesc.Tag = "25";
			toolTip.SetToolTip(labelTrueAnomalyDesc, "True anomaly (°)");
			labelTrueAnomalyDesc.Values.ExtraText = "°";
			labelTrueAnomalyDesc.Values.Text = "True anomaly";
			labelTrueAnomalyDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelTrueAnomalyDesc.Enter += SetStatusBar_Enter;
			labelTrueAnomalyDesc.Leave += ClearStatusBar_Leave;
			labelTrueAnomalyDesc.MouseDown += Control_MouseDown;
			labelTrueAnomalyDesc.MouseEnter += SetStatusBar_Enter;
			labelTrueAnomalyDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelPerihelionDistanceDesc
			// 
			labelPerihelionDistanceDesc.AccessibleDescription = "Perihelion distance (AU)";
			labelPerihelionDistanceDesc.AccessibleName = "Perihelion distance (AU)";
			labelPerihelionDistanceDesc.AccessibleRole = AccessibleRole.StaticText;
			labelPerihelionDistanceDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelPerihelionDistanceDesc.Dock = DockStyle.Fill;
			labelPerihelionDistanceDesc.LabelStyle = LabelStyle.BoldPanel;
			labelPerihelionDistanceDesc.Location = new Point(4, 159);
			labelPerihelionDistanceDesc.Margin = new Padding(4, 3, 4, 3);
			labelPerihelionDistanceDesc.Name = "labelPerihelionDistanceDesc";
			labelPerihelionDistanceDesc.Size = new Size(268, 20);
			labelPerihelionDistanceDesc.TabIndex = 12;
			labelPerihelionDistanceDesc.Tag = "26";
			toolTip.SetToolTip(labelPerihelionDistanceDesc, "Perihelion distance (AU)");
			labelPerihelionDistanceDesc.Values.ExtraText = "AU";
			labelPerihelionDistanceDesc.Values.Text = "Perihelion distance";
			labelPerihelionDistanceDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelPerihelionDistanceDesc.Enter += SetStatusBar_Enter;
			labelPerihelionDistanceDesc.Leave += ClearStatusBar_Leave;
			labelPerihelionDistanceDesc.MouseDown += Control_MouseDown;
			labelPerihelionDistanceDesc.MouseEnter += SetStatusBar_Enter;
			labelPerihelionDistanceDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// LabelAphelionDistanceDesc
			// 
			LabelAphelionDistanceDesc.AccessibleDescription = "Aphelion distance (AU)";
			LabelAphelionDistanceDesc.AccessibleName = "Aphelion distance (AU)";
			LabelAphelionDistanceDesc.AccessibleRole = AccessibleRole.StaticText;
			LabelAphelionDistanceDesc.ContextMenuStrip = contextMenuOpenTerminology;
			LabelAphelionDistanceDesc.Dock = DockStyle.Fill;
			LabelAphelionDistanceDesc.LabelStyle = LabelStyle.BoldPanel;
			LabelAphelionDistanceDesc.Location = new Point(4, 185);
			LabelAphelionDistanceDesc.Margin = new Padding(4, 3, 4, 3);
			LabelAphelionDistanceDesc.Name = "LabelAphelionDistanceDesc";
			LabelAphelionDistanceDesc.Size = new Size(268, 20);
			LabelAphelionDistanceDesc.TabIndex = 14;
			LabelAphelionDistanceDesc.Tag = "27";
			toolTip.SetToolTip(LabelAphelionDistanceDesc, "Aphelion distance (AU)");
			LabelAphelionDistanceDesc.Values.ExtraText = "AU";
			LabelAphelionDistanceDesc.Values.Text = "Aphelion distance";
			LabelAphelionDistanceDesc.DoubleClick += OpenTerminology_DoubleClick;
			LabelAphelionDistanceDesc.Enter += SetStatusBar_Enter;
			LabelAphelionDistanceDesc.Leave += ClearStatusBar_Leave;
			LabelAphelionDistanceDesc.MouseDown += Control_MouseDown;
			LabelAphelionDistanceDesc.MouseEnter += SetStatusBar_Enter;
			LabelAphelionDistanceDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLongitudeDescendingNodeDesc
			// 
			labelLongitudeDescendingNodeDesc.AccessibleDescription = "Longitude of the descending node (degrees)";
			labelLongitudeDescendingNodeDesc.AccessibleName = "Longitude of the descending node (degrees)";
			labelLongitudeDescendingNodeDesc.AccessibleRole = AccessibleRole.StaticText;
			labelLongitudeDescendingNodeDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelLongitudeDescendingNodeDesc.Dock = DockStyle.Fill;
			labelLongitudeDescendingNodeDesc.LabelStyle = LabelStyle.BoldPanel;
			labelLongitudeDescendingNodeDesc.Location = new Point(4, 211);
			labelLongitudeDescendingNodeDesc.Margin = new Padding(4, 3, 4, 3);
			labelLongitudeDescendingNodeDesc.Name = "labelLongitudeDescendingNodeDesc";
			labelLongitudeDescendingNodeDesc.Size = new Size(268, 20);
			labelLongitudeDescendingNodeDesc.TabIndex = 16;
			labelLongitudeDescendingNodeDesc.Tag = "28";
			toolTip.SetToolTip(labelLongitudeDescendingNodeDesc, "Longitude of the descending node (°)");
			labelLongitudeDescendingNodeDesc.Values.ExtraText = "°";
			labelLongitudeDescendingNodeDesc.Values.Text = "Longitude of the descending node";
			labelLongitudeDescendingNodeDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLongitudeDescendingNodeDesc.Enter += SetStatusBar_Enter;
			labelLongitudeDescendingNodeDesc.Leave += ClearStatusBar_Leave;
			labelLongitudeDescendingNodeDesc.MouseDown += Control_MouseDown;
			labelLongitudeDescendingNodeDesc.MouseEnter += SetStatusBar_Enter;
			labelLongitudeDescendingNodeDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelArgumentAphelionDesc
			// 
			labelArgumentAphelionDesc.AccessibleDescription = "Argument of aphelion (degrees)";
			labelArgumentAphelionDesc.AccessibleName = "Argument of aphelion (degrees)";
			labelArgumentAphelionDesc.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentAphelionDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelArgumentAphelionDesc.Dock = DockStyle.Fill;
			labelArgumentAphelionDesc.LabelStyle = LabelStyle.BoldPanel;
			labelArgumentAphelionDesc.Location = new Point(4, 237);
			labelArgumentAphelionDesc.Margin = new Padding(4, 3, 4, 3);
			labelArgumentAphelionDesc.Name = "labelArgumentAphelionDesc";
			labelArgumentAphelionDesc.Size = new Size(268, 20);
			labelArgumentAphelionDesc.TabIndex = 18;
			labelArgumentAphelionDesc.Tag = "29";
			toolTip.SetToolTip(labelArgumentAphelionDesc, "Argument of aphelion (°)");
			labelArgumentAphelionDesc.Values.ExtraText = "°";
			labelArgumentAphelionDesc.Values.Text = "Argument of aphelion";
			labelArgumentAphelionDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelArgumentAphelionDesc.Enter += SetStatusBar_Enter;
			labelArgumentAphelionDesc.Leave += ClearStatusBar_Leave;
			labelArgumentAphelionDesc.MouseDown += Control_MouseDown;
			labelArgumentAphelionDesc.MouseEnter += SetStatusBar_Enter;
			labelArgumentAphelionDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelFocalParameterDesc
			// 
			labelFocalParameterDesc.AccessibleDescription = "Focal parameter (AU)";
			labelFocalParameterDesc.AccessibleName = "Focal parameter (AU)";
			labelFocalParameterDesc.AccessibleRole = AccessibleRole.StaticText;
			labelFocalParameterDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelFocalParameterDesc.Dock = DockStyle.Fill;
			labelFocalParameterDesc.LabelStyle = LabelStyle.BoldPanel;
			labelFocalParameterDesc.Location = new Point(4, 263);
			labelFocalParameterDesc.Margin = new Padding(4, 3, 4, 3);
			labelFocalParameterDesc.Name = "labelFocalParameterDesc";
			labelFocalParameterDesc.Size = new Size(268, 20);
			labelFocalParameterDesc.TabIndex = 20;
			labelFocalParameterDesc.Tag = "30";
			toolTip.SetToolTip(labelFocalParameterDesc, "Focal parameter (AU)");
			labelFocalParameterDesc.Values.ExtraText = "AU";
			labelFocalParameterDesc.Values.Text = "Focal parameter";
			labelFocalParameterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelFocalParameterDesc.Enter += SetStatusBar_Enter;
			labelFocalParameterDesc.Leave += ClearStatusBar_Leave;
			labelFocalParameterDesc.MouseDown += Control_MouseDown;
			labelFocalParameterDesc.MouseEnter += SetStatusBar_Enter;
			labelFocalParameterDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiLatusRectumDesc
			// 
			labelSemiLatusRectumDesc.AccessibleDescription = "Semi-latus rectum (AU)";
			labelSemiLatusRectumDesc.AccessibleName = "Semi-latus rectum (AU)";
			labelSemiLatusRectumDesc.AccessibleRole = AccessibleRole.StaticText;
			labelSemiLatusRectumDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelSemiLatusRectumDesc.Dock = DockStyle.Fill;
			labelSemiLatusRectumDesc.LabelStyle = LabelStyle.BoldPanel;
			labelSemiLatusRectumDesc.Location = new Point(4, 289);
			labelSemiLatusRectumDesc.Margin = new Padding(4, 3, 4, 3);
			labelSemiLatusRectumDesc.Name = "labelSemiLatusRectumDesc";
			labelSemiLatusRectumDesc.Size = new Size(268, 20);
			labelSemiLatusRectumDesc.TabIndex = 22;
			labelSemiLatusRectumDesc.Tag = "31";
			toolTip.SetToolTip(labelSemiLatusRectumDesc, "Semi-latus rectum (AU)");
			labelSemiLatusRectumDesc.Values.ExtraText = "AU";
			labelSemiLatusRectumDesc.Values.Text = "Semi-latus rectum";
			labelSemiLatusRectumDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiLatusRectumDesc.Enter += SetStatusBar_Enter;
			labelSemiLatusRectumDesc.Leave += ClearStatusBar_Leave;
			labelSemiLatusRectumDesc.MouseDown += Control_MouseDown;
			labelSemiLatusRectumDesc.MouseEnter += SetStatusBar_Enter;
			labelSemiLatusRectumDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLatusRectumDesc
			// 
			labelLatusRectumDesc.AccessibleDescription = "Latus rectum (AU)";
			labelLatusRectumDesc.AccessibleName = "Latus rectum (AU)";
			labelLatusRectumDesc.AccessibleRole = AccessibleRole.StaticText;
			labelLatusRectumDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelLatusRectumDesc.Dock = DockStyle.Fill;
			labelLatusRectumDesc.LabelStyle = LabelStyle.BoldPanel;
			labelLatusRectumDesc.Location = new Point(4, 315);
			labelLatusRectumDesc.Margin = new Padding(4, 3, 4, 3);
			labelLatusRectumDesc.Name = "labelLatusRectumDesc";
			labelLatusRectumDesc.Size = new Size(268, 20);
			labelLatusRectumDesc.TabIndex = 24;
			labelLatusRectumDesc.Tag = "32";
			toolTip.SetToolTip(labelLatusRectumDesc, "Latus rectum (AU)");
			labelLatusRectumDesc.Values.ExtraText = "AU";
			labelLatusRectumDesc.Values.Text = "Latus rectum";
			labelLatusRectumDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLatusRectumDesc.Enter += SetStatusBar_Enter;
			labelLatusRectumDesc.Leave += ClearStatusBar_Leave;
			labelLatusRectumDesc.MouseDown += Control_MouseDown;
			labelLatusRectumDesc.MouseEnter += SetStatusBar_Enter;
			labelLatusRectumDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalPeriodDesc
			// 
			labelOrbitalPeriodDesc.AccessibleDescription = "Orbital period (years)";
			labelOrbitalPeriodDesc.AccessibleName = "Orbital period (years)";
			labelOrbitalPeriodDesc.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPeriodDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelOrbitalPeriodDesc.Dock = DockStyle.Fill;
			labelOrbitalPeriodDesc.LabelStyle = LabelStyle.BoldPanel;
			labelOrbitalPeriodDesc.Location = new Point(4, 341);
			labelOrbitalPeriodDesc.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPeriodDesc.Name = "labelOrbitalPeriodDesc";
			labelOrbitalPeriodDesc.Size = new Size(268, 20);
			labelOrbitalPeriodDesc.TabIndex = 26;
			labelOrbitalPeriodDesc.Tag = "33";
			toolTip.SetToolTip(labelOrbitalPeriodDesc, "Orbital Period (years)");
			labelOrbitalPeriodDesc.Values.ExtraText = "years";
			labelOrbitalPeriodDesc.Values.Text = "Orbital period";
			labelOrbitalPeriodDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalPeriodDesc.Enter += SetStatusBar_Enter;
			labelOrbitalPeriodDesc.Leave += ClearStatusBar_Leave;
			labelOrbitalPeriodDesc.MouseDown += Control_MouseDown;
			labelOrbitalPeriodDesc.MouseEnter += SetStatusBar_Enter;
			labelOrbitalPeriodDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiMinorAxisData
			// 
			labelSemiMinorAxisData.AccessibleDescription = "Shows the information of \"Semi-minor axis\"";
			labelSemiMinorAxisData.AccessibleName = "Shows the information of \"Semi-minor axis\"";
			labelSemiMinorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMinorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiMinorAxisData.Dock = DockStyle.Fill;
			labelSemiMinorAxisData.Location = new Point(280, 29);
			labelSemiMinorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelSemiMinorAxisData.Name = "labelSemiMinorAxisData";
			labelSemiMinorAxisData.Size = new Size(270, 20);
			labelSemiMinorAxisData.TabIndex = 3;
			toolTip.SetToolTip(labelSemiMinorAxisData, "Shows the information of \"Semi-minor axis\"");
			labelSemiMinorAxisData.Values.Text = "..................";
			labelSemiMinorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiMinorAxisData.Enter += SetStatusBar_Enter;
			labelSemiMinorAxisData.Leave += ClearStatusBar_Leave;
			labelSemiMinorAxisData.MouseDown += Control_MouseDown;
			labelSemiMinorAxisData.MouseEnter += SetStatusBar_Enter;
			labelSemiMinorAxisData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMajorAxisData
			// 
			labelMajorAxisData.AccessibleDescription = "Shows the information of \"Major axis\"";
			labelMajorAxisData.AccessibleName = "Shows the information of \"Major axis\"";
			labelMajorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMajorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMajorAxisData.Dock = DockStyle.Fill;
			labelMajorAxisData.Location = new Point(280, 55);
			labelMajorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMajorAxisData.Name = "labelMajorAxisData";
			labelMajorAxisData.Size = new Size(270, 20);
			labelMajorAxisData.TabIndex = 5;
			toolTip.SetToolTip(labelMajorAxisData, "Shows the information of \"Major axis\"");
			labelMajorAxisData.Values.Text = "..................";
			labelMajorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMajorAxisData.Enter += SetStatusBar_Enter;
			labelMajorAxisData.Leave += ClearStatusBar_Leave;
			labelMajorAxisData.MouseDown += Control_MouseDown;
			labelMajorAxisData.MouseEnter += SetStatusBar_Enter;
			labelMajorAxisData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMinorAxisData
			// 
			labelMinorAxisData.AccessibleDescription = "Shows the information of \"Minor axis\"";
			labelMinorAxisData.AccessibleName = "Shows the information of \"Minor axis\"";
			labelMinorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMinorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMinorAxisData.Dock = DockStyle.Fill;
			labelMinorAxisData.Location = new Point(280, 81);
			labelMinorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMinorAxisData.Name = "labelMinorAxisData";
			labelMinorAxisData.Size = new Size(270, 20);
			labelMinorAxisData.TabIndex = 7;
			toolTip.SetToolTip(labelMinorAxisData, "Shows the information of \"Minor axis\"");
			labelMinorAxisData.Values.Text = "..................";
			labelMinorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMinorAxisData.Enter += SetStatusBar_Enter;
			labelMinorAxisData.Leave += ClearStatusBar_Leave;
			labelMinorAxisData.MouseDown += Control_MouseDown;
			labelMinorAxisData.MouseEnter += SetStatusBar_Enter;
			labelMinorAxisData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelEccentricAnomalyData
			// 
			labelEccentricAnomalyData.AccessibleDescription = "Shows the information of \"Eccentric anomaly\"";
			labelEccentricAnomalyData.AccessibleName = "Shows the information of \"Eccentric anomaly\"";
			labelEccentricAnomalyData.AccessibleRole = AccessibleRole.StaticText;
			labelEccentricAnomalyData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelEccentricAnomalyData.Dock = DockStyle.Fill;
			labelEccentricAnomalyData.Location = new Point(280, 107);
			labelEccentricAnomalyData.Margin = new Padding(4, 3, 4, 3);
			labelEccentricAnomalyData.Name = "labelEccentricAnomalyData";
			labelEccentricAnomalyData.Size = new Size(270, 20);
			labelEccentricAnomalyData.TabIndex = 9;
			toolTip.SetToolTip(labelEccentricAnomalyData, "Shows the information of \"Eccentric anomaly\"");
			labelEccentricAnomalyData.Values.Text = "..................";
			labelEccentricAnomalyData.DoubleClick += CopyToClipboard_DoubleClick;
			labelEccentricAnomalyData.Enter += SetStatusBar_Enter;
			labelEccentricAnomalyData.Leave += ClearStatusBar_Leave;
			labelEccentricAnomalyData.MouseDown += Control_MouseDown;
			labelEccentricAnomalyData.MouseEnter += SetStatusBar_Enter;
			labelEccentricAnomalyData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelTrueAnomalyData
			// 
			labelTrueAnomalyData.AccessibleDescription = "Shows the information of \"True anomaly\"";
			labelTrueAnomalyData.AccessibleName = "Shows the information of \"True anomaly\"";
			labelTrueAnomalyData.AccessibleRole = AccessibleRole.StaticText;
			labelTrueAnomalyData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelTrueAnomalyData.Dock = DockStyle.Fill;
			labelTrueAnomalyData.Location = new Point(280, 133);
			labelTrueAnomalyData.Margin = new Padding(4, 3, 4, 3);
			labelTrueAnomalyData.Name = "labelTrueAnomalyData";
			labelTrueAnomalyData.Size = new Size(270, 20);
			labelTrueAnomalyData.TabIndex = 11;
			toolTip.SetToolTip(labelTrueAnomalyData, "Shows the information of \"True anomaly\"");
			labelTrueAnomalyData.Values.Text = "..................";
			labelTrueAnomalyData.DoubleClick += CopyToClipboard_DoubleClick;
			labelTrueAnomalyData.Enter += SetStatusBar_Enter;
			labelTrueAnomalyData.Leave += ClearStatusBar_Leave;
			labelTrueAnomalyData.MouseDown += Control_MouseDown;
			labelTrueAnomalyData.MouseEnter += SetStatusBar_Enter;
			labelTrueAnomalyData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelPerihelionDistanceData
			// 
			labelPerihelionDistanceData.AccessibleDescription = "Shows the information of \"Perihelion distance\"";
			labelPerihelionDistanceData.AccessibleName = "Shows the information of \"Perihelion distance\"";
			labelPerihelionDistanceData.AccessibleRole = AccessibleRole.StaticText;
			labelPerihelionDistanceData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelPerihelionDistanceData.Dock = DockStyle.Fill;
			labelPerihelionDistanceData.Location = new Point(280, 159);
			labelPerihelionDistanceData.Margin = new Padding(4, 3, 4, 3);
			labelPerihelionDistanceData.Name = "labelPerihelionDistanceData";
			labelPerihelionDistanceData.Size = new Size(270, 20);
			labelPerihelionDistanceData.TabIndex = 13;
			toolTip.SetToolTip(labelPerihelionDistanceData, "Shows the information of \"Perihelion distance\"");
			labelPerihelionDistanceData.Values.Text = "..................";
			labelPerihelionDistanceData.DoubleClick += CopyToClipboard_DoubleClick;
			labelPerihelionDistanceData.Enter += SetStatusBar_Enter;
			labelPerihelionDistanceData.Leave += ClearStatusBar_Leave;
			labelPerihelionDistanceData.MouseDown += Control_MouseDown;
			labelPerihelionDistanceData.MouseEnter += SetStatusBar_Enter;
			labelPerihelionDistanceData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelAphelionDistanceData
			// 
			labelAphelionDistanceData.AccessibleDescription = "Shows the information of \"Aphelion distance\"";
			labelAphelionDistanceData.AccessibleName = "Shows the information of \"Aphelion distance\"";
			labelAphelionDistanceData.AccessibleRole = AccessibleRole.StaticText;
			labelAphelionDistanceData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelAphelionDistanceData.Dock = DockStyle.Fill;
			labelAphelionDistanceData.Location = new Point(280, 185);
			labelAphelionDistanceData.Margin = new Padding(4, 3, 4, 3);
			labelAphelionDistanceData.Name = "labelAphelionDistanceData";
			labelAphelionDistanceData.Size = new Size(270, 20);
			labelAphelionDistanceData.TabIndex = 15;
			toolTip.SetToolTip(labelAphelionDistanceData, "Shows the information of \"Aphelion distance\"");
			labelAphelionDistanceData.Values.Text = "..................";
			labelAphelionDistanceData.DoubleClick += CopyToClipboard_DoubleClick;
			labelAphelionDistanceData.Enter += SetStatusBar_Enter;
			labelAphelionDistanceData.Leave += ClearStatusBar_Leave;
			labelAphelionDistanceData.MouseDown += Control_MouseDown;
			labelAphelionDistanceData.MouseEnter += SetStatusBar_Enter;
			labelAphelionDistanceData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLongitudeDescendingNodeData
			// 
			labelLongitudeDescendingNodeData.AccessibleDescription = "Shows the information of \"Longitude of descending node\"";
			labelLongitudeDescendingNodeData.AccessibleName = "Shows the information of \"Longitude of descending node\"";
			labelLongitudeDescendingNodeData.AccessibleRole = AccessibleRole.StaticText;
			labelLongitudeDescendingNodeData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLongitudeDescendingNodeData.Dock = DockStyle.Fill;
			labelLongitudeDescendingNodeData.Location = new Point(280, 211);
			labelLongitudeDescendingNodeData.Margin = new Padding(4, 3, 4, 3);
			labelLongitudeDescendingNodeData.Name = "labelLongitudeDescendingNodeData";
			labelLongitudeDescendingNodeData.Size = new Size(270, 20);
			labelLongitudeDescendingNodeData.TabIndex = 17;
			toolTip.SetToolTip(labelLongitudeDescendingNodeData, "Shows the information of \"Longitude of descending node\"");
			labelLongitudeDescendingNodeData.Values.Text = "..................";
			labelLongitudeDescendingNodeData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLongitudeDescendingNodeData.Enter += SetStatusBar_Enter;
			labelLongitudeDescendingNodeData.Leave += ClearStatusBar_Leave;
			labelLongitudeDescendingNodeData.MouseDown += Control_MouseDown;
			labelLongitudeDescendingNodeData.MouseEnter += SetStatusBar_Enter;
			labelLongitudeDescendingNodeData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelArgumentAphelionData
			// 
			labelArgumentAphelionData.AccessibleDescription = "Shows the information of \"Argument of aphelion\"";
			labelArgumentAphelionData.AccessibleName = "Shows the information of \"Argument of aphelion\"";
			labelArgumentAphelionData.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentAphelionData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelArgumentAphelionData.Dock = DockStyle.Fill;
			labelArgumentAphelionData.Location = new Point(280, 237);
			labelArgumentAphelionData.Margin = new Padding(4, 3, 4, 3);
			labelArgumentAphelionData.Name = "labelArgumentAphelionData";
			labelArgumentAphelionData.Size = new Size(270, 20);
			labelArgumentAphelionData.TabIndex = 19;
			toolTip.SetToolTip(labelArgumentAphelionData, "Shows the information of \"Argument of aphelion\"");
			labelArgumentAphelionData.Values.Text = "..................";
			labelArgumentAphelionData.DoubleClick += CopyToClipboard_DoubleClick;
			labelArgumentAphelionData.Enter += SetStatusBar_Enter;
			labelArgumentAphelionData.Leave += ClearStatusBar_Leave;
			labelArgumentAphelionData.MouseDown += Control_MouseDown;
			labelArgumentAphelionData.MouseEnter += SetStatusBar_Enter;
			labelArgumentAphelionData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelFocalParameterData
			// 
			labelFocalParameterData.AccessibleDescription = "Shows the information of \"Focal parameter\"";
			labelFocalParameterData.AccessibleName = "Shows the information of \"Focal parameter\"";
			labelFocalParameterData.AccessibleRole = AccessibleRole.StaticText;
			labelFocalParameterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelFocalParameterData.Dock = DockStyle.Fill;
			labelFocalParameterData.Location = new Point(280, 263);
			labelFocalParameterData.Margin = new Padding(4, 3, 4, 3);
			labelFocalParameterData.Name = "labelFocalParameterData";
			labelFocalParameterData.Size = new Size(270, 20);
			labelFocalParameterData.TabIndex = 21;
			toolTip.SetToolTip(labelFocalParameterData, "Shows the information of \"Focal parameter\"");
			labelFocalParameterData.Values.Text = "..................";
			labelFocalParameterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelFocalParameterData.Enter += SetStatusBar_Enter;
			labelFocalParameterData.Leave += ClearStatusBar_Leave;
			labelFocalParameterData.MouseDown += Control_MouseDown;
			labelFocalParameterData.MouseEnter += SetStatusBar_Enter;
			labelFocalParameterData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiLatusRectumData
			// 
			labelSemiLatusRectumData.AccessibleDescription = "Shows the information of \"Semi-latus rectum\"";
			labelSemiLatusRectumData.AccessibleName = "Shows the information of \"Semi-latus rectum\"";
			labelSemiLatusRectumData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiLatusRectumData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiLatusRectumData.Dock = DockStyle.Fill;
			labelSemiLatusRectumData.Location = new Point(280, 289);
			labelSemiLatusRectumData.Margin = new Padding(4, 3, 4, 3);
			labelSemiLatusRectumData.Name = "labelSemiLatusRectumData";
			labelSemiLatusRectumData.Size = new Size(270, 20);
			labelSemiLatusRectumData.TabIndex = 23;
			toolTip.SetToolTip(labelSemiLatusRectumData, "Shows the information of \"Semi-latus rectum\"");
			labelSemiLatusRectumData.Values.Text = "..................";
			labelSemiLatusRectumData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiLatusRectumData.Enter += SetStatusBar_Enter;
			labelSemiLatusRectumData.Leave += ClearStatusBar_Leave;
			labelSemiLatusRectumData.MouseDown += Control_MouseDown;
			labelSemiLatusRectumData.MouseEnter += SetStatusBar_Enter;
			labelSemiLatusRectumData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLatusRectumData
			// 
			labelLatusRectumData.AccessibleDescription = "Shows the information of \"Latus rectum\"";
			labelLatusRectumData.AccessibleName = "Shows the information of \"Latus rectum\"";
			labelLatusRectumData.AccessibleRole = AccessibleRole.StaticText;
			labelLatusRectumData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLatusRectumData.Dock = DockStyle.Fill;
			labelLatusRectumData.Location = new Point(280, 315);
			labelLatusRectumData.Margin = new Padding(4, 3, 4, 3);
			labelLatusRectumData.Name = "labelLatusRectumData";
			labelLatusRectumData.Size = new Size(270, 20);
			labelLatusRectumData.TabIndex = 25;
			toolTip.SetToolTip(labelLatusRectumData, "Shows the information of \"Latus rectum\"");
			labelLatusRectumData.Values.Text = "..................";
			labelLatusRectumData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLatusRectumData.Enter += SetStatusBar_Enter;
			labelLatusRectumData.Leave += ClearStatusBar_Leave;
			labelLatusRectumData.MouseDown += Control_MouseDown;
			labelLatusRectumData.MouseEnter += SetStatusBar_Enter;
			labelLatusRectumData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalPeriodData
			// 
			labelOrbitalPeriodData.AccessibleDescription = "Shows the information of \"Period\"";
			labelOrbitalPeriodData.AccessibleName = "Shows the information of \"Period\"";
			labelOrbitalPeriodData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPeriodData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalPeriodData.Dock = DockStyle.Fill;
			labelOrbitalPeriodData.Location = new Point(280, 341);
			labelOrbitalPeriodData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPeriodData.Name = "labelOrbitalPeriodData";
			labelOrbitalPeriodData.Size = new Size(270, 20);
			labelOrbitalPeriodData.TabIndex = 27;
			toolTip.SetToolTip(labelOrbitalPeriodData, "Shows the information of \"Period\"");
			labelOrbitalPeriodData.Values.Text = "..................";
			labelOrbitalPeriodData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalPeriodData.Enter += SetStatusBar_Enter;
			labelOrbitalPeriodData.Leave += ClearStatusBar_Leave;
			labelOrbitalPeriodData.MouseDown += Control_MouseDown;
			labelOrbitalPeriodData.MouseEnter += SetStatusBar_Enter;
			labelOrbitalPeriodData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalAreaDesc
			// 
			labelOrbitalAreaDesc.AccessibleDescription = "Orbital area (AU²)";
			labelOrbitalAreaDesc.AccessibleName = "Orbital area (AU²)";
			labelOrbitalAreaDesc.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalAreaDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelOrbitalAreaDesc.Dock = DockStyle.Fill;
			labelOrbitalAreaDesc.LabelStyle = LabelStyle.BoldPanel;
			labelOrbitalAreaDesc.Location = new Point(4, 367);
			labelOrbitalAreaDesc.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalAreaDesc.Name = "labelOrbitalAreaDesc";
			labelOrbitalAreaDesc.Size = new Size(268, 20);
			labelOrbitalAreaDesc.TabIndex = 28;
			labelOrbitalAreaDesc.Tag = "34";
			toolTip.SetToolTip(labelOrbitalAreaDesc, "Orbital area (AU²)");
			labelOrbitalAreaDesc.Values.ExtraText = "AU²";
			labelOrbitalAreaDesc.Values.Text = "Orbital area";
			labelOrbitalAreaDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalAreaDesc.Enter += SetStatusBar_Enter;
			labelOrbitalAreaDesc.Leave += ClearStatusBar_Leave;
			labelOrbitalAreaDesc.MouseDown += Control_MouseDown;
			labelOrbitalAreaDesc.MouseEnter += SetStatusBar_Enter;
			labelOrbitalAreaDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalPerimeterDesc
			// 
			labelOrbitalPerimeterDesc.AccessibleDescription = "Orbital perimeter (AU)";
			labelOrbitalPerimeterDesc.AccessibleName = "Orbital perimeter (AU)";
			labelOrbitalPerimeterDesc.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPerimeterDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelOrbitalPerimeterDesc.Dock = DockStyle.Fill;
			labelOrbitalPerimeterDesc.LabelStyle = LabelStyle.BoldPanel;
			labelOrbitalPerimeterDesc.Location = new Point(4, 393);
			labelOrbitalPerimeterDesc.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPerimeterDesc.Name = "labelOrbitalPerimeterDesc";
			labelOrbitalPerimeterDesc.Size = new Size(268, 20);
			labelOrbitalPerimeterDesc.TabIndex = 30;
			labelOrbitalPerimeterDesc.Tag = "35";
			toolTip.SetToolTip(labelOrbitalPerimeterDesc, "Orbital perimeter (AU)");
			labelOrbitalPerimeterDesc.Values.ExtraText = "AU";
			labelOrbitalPerimeterDesc.Values.Text = "Orbital perimeter";
			labelOrbitalPerimeterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalPerimeterDesc.Enter += SetStatusBar_Enter;
			labelOrbitalPerimeterDesc.Leave += ClearStatusBar_Leave;
			labelOrbitalPerimeterDesc.MouseDown += Control_MouseDown;
			labelOrbitalPerimeterDesc.MouseEnter += SetStatusBar_Enter;
			labelOrbitalPerimeterDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiMeanAxisDesc
			// 
			labelSemiMeanAxisDesc.AccessibleDescription = "Semi-mean axis (AU)";
			labelSemiMeanAxisDesc.AccessibleName = "Semi-mean axis (AU)";
			labelSemiMeanAxisDesc.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMeanAxisDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelSemiMeanAxisDesc.Dock = DockStyle.Fill;
			labelSemiMeanAxisDesc.LabelStyle = LabelStyle.BoldPanel;
			labelSemiMeanAxisDesc.Location = new Point(4, 419);
			labelSemiMeanAxisDesc.Margin = new Padding(4, 3, 4, 3);
			labelSemiMeanAxisDesc.Name = "labelSemiMeanAxisDesc";
			labelSemiMeanAxisDesc.Size = new Size(268, 20);
			labelSemiMeanAxisDesc.TabIndex = 32;
			labelSemiMeanAxisDesc.Tag = "36";
			toolTip.SetToolTip(labelSemiMeanAxisDesc, "Semi-mean axis (AU)");
			labelSemiMeanAxisDesc.Values.ExtraText = "AU";
			labelSemiMeanAxisDesc.Values.Text = "Semi-mean axis";
			labelSemiMeanAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiMeanAxisDesc.Enter += SetStatusBar_Enter;
			labelSemiMeanAxisDesc.Leave += ClearStatusBar_Leave;
			labelSemiMeanAxisDesc.MouseDown += Control_MouseDown;
			labelSemiMeanAxisDesc.MouseEnter += SetStatusBar_Enter;
			labelSemiMeanAxisDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMeanAxisDesc
			// 
			labelMeanAxisDesc.AccessibleDescription = "Mean axis (AU)";
			labelMeanAxisDesc.AccessibleName = "Mean axis (AU)";
			labelMeanAxisDesc.AccessibleRole = AccessibleRole.StaticText;
			labelMeanAxisDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelMeanAxisDesc.Dock = DockStyle.Fill;
			labelMeanAxisDesc.LabelStyle = LabelStyle.BoldPanel;
			labelMeanAxisDesc.Location = new Point(4, 445);
			labelMeanAxisDesc.Margin = new Padding(4, 3, 4, 3);
			labelMeanAxisDesc.Name = "labelMeanAxisDesc";
			labelMeanAxisDesc.Size = new Size(268, 20);
			labelMeanAxisDesc.TabIndex = 34;
			labelMeanAxisDesc.Tag = "37";
			toolTip.SetToolTip(labelMeanAxisDesc, "Mean axis (AU)");
			labelMeanAxisDesc.Values.ExtraText = "AU";
			labelMeanAxisDesc.Values.Text = "Mean axis";
			labelMeanAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMeanAxisDesc.Enter += SetStatusBar_Enter;
			labelMeanAxisDesc.Leave += ClearStatusBar_Leave;
			labelMeanAxisDesc.MouseDown += Control_MouseDown;
			labelMeanAxisDesc.MouseEnter += SetStatusBar_Enter;
			labelMeanAxisDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalAreaData
			// 
			labelOrbitalAreaData.AccessibleDescription = "Shows the information of \"Orbital area\"";
			labelOrbitalAreaData.AccessibleName = "Shows the information of \"Orbital area\"";
			labelOrbitalAreaData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalAreaData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalAreaData.Dock = DockStyle.Fill;
			labelOrbitalAreaData.Location = new Point(280, 367);
			labelOrbitalAreaData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalAreaData.Name = "labelOrbitalAreaData";
			labelOrbitalAreaData.Size = new Size(270, 20);
			labelOrbitalAreaData.TabIndex = 29;
			toolTip.SetToolTip(labelOrbitalAreaData, "Shows the information of \"Orbital area\"");
			labelOrbitalAreaData.Values.Text = "..................";
			labelOrbitalAreaData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalAreaData.Enter += SetStatusBar_Enter;
			labelOrbitalAreaData.Leave += ClearStatusBar_Leave;
			labelOrbitalAreaData.MouseDown += Control_MouseDown;
			labelOrbitalAreaData.MouseEnter += SetStatusBar_Enter;
			labelOrbitalAreaData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalPerimeterData
			// 
			labelOrbitalPerimeterData.AccessibleDescription = "Shows the information of \"Orbital perimeter\"";
			labelOrbitalPerimeterData.AccessibleName = "Shows the information of \"Orbital perimeter\"";
			labelOrbitalPerimeterData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPerimeterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalPerimeterData.Dock = DockStyle.Fill;
			labelOrbitalPerimeterData.Location = new Point(280, 393);
			labelOrbitalPerimeterData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPerimeterData.Name = "labelOrbitalPerimeterData";
			labelOrbitalPerimeterData.Size = new Size(270, 20);
			labelOrbitalPerimeterData.TabIndex = 31;
			toolTip.SetToolTip(labelOrbitalPerimeterData, "Shows the information of \"Orbital perimeter\"");
			labelOrbitalPerimeterData.Values.Text = "..................";
			labelOrbitalPerimeterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalPerimeterData.Enter += SetStatusBar_Enter;
			labelOrbitalPerimeterData.Leave += ClearStatusBar_Leave;
			labelOrbitalPerimeterData.MouseDown += Control_MouseDown;
			labelOrbitalPerimeterData.MouseEnter += SetStatusBar_Enter;
			labelOrbitalPerimeterData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiMeanAxisData
			// 
			labelSemiMeanAxisData.AccessibleDescription = "Shows the information of \"Semi-mean axis\"";
			labelSemiMeanAxisData.AccessibleName = "Shows the information of \"Semi-mean axis\"";
			labelSemiMeanAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMeanAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiMeanAxisData.Dock = DockStyle.Fill;
			labelSemiMeanAxisData.Location = new Point(280, 419);
			labelSemiMeanAxisData.Margin = new Padding(4, 3, 4, 3);
			labelSemiMeanAxisData.Name = "labelSemiMeanAxisData";
			labelSemiMeanAxisData.Size = new Size(270, 20);
			labelSemiMeanAxisData.TabIndex = 33;
			toolTip.SetToolTip(labelSemiMeanAxisData, "Shows the information of \"Semi-mean axis\"");
			labelSemiMeanAxisData.Values.Text = "..................";
			labelSemiMeanAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiMeanAxisData.Enter += SetStatusBar_Enter;
			labelSemiMeanAxisData.Leave += ClearStatusBar_Leave;
			labelSemiMeanAxisData.MouseDown += Control_MouseDown;
			labelSemiMeanAxisData.MouseEnter += SetStatusBar_Enter;
			labelSemiMeanAxisData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelStandardGravitationalParameterDesc
			// 
			labelStandardGravitationalParameterDesc.AccessibleDescription = "Standard gravitational parameter (AU³/a²)";
			labelStandardGravitationalParameterDesc.AccessibleName = "Standard gravitational parameter (AU³/a²)";
			labelStandardGravitationalParameterDesc.AccessibleRole = AccessibleRole.StaticText;
			labelStandardGravitationalParameterDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelStandardGravitationalParameterDesc.Dock = DockStyle.Fill;
			labelStandardGravitationalParameterDesc.LabelStyle = LabelStyle.BoldPanel;
			labelStandardGravitationalParameterDesc.Location = new Point(4, 471);
			labelStandardGravitationalParameterDesc.Margin = new Padding(4, 3, 4, 3);
			labelStandardGravitationalParameterDesc.Name = "labelStandardGravitationalParameterDesc";
			labelStandardGravitationalParameterDesc.Size = new Size(268, 21);
			labelStandardGravitationalParameterDesc.TabIndex = 36;
			labelStandardGravitationalParameterDesc.Tag = "38";
			toolTip.SetToolTip(labelStandardGravitationalParameterDesc, "Standard gravitational parameter (AU³/a²)");
			labelStandardGravitationalParameterDesc.Values.ExtraText = "AU³/a²";
			labelStandardGravitationalParameterDesc.Values.Text = "Standard gravitational parameter";
			labelStandardGravitationalParameterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelStandardGravitationalParameterDesc.Enter += SetStatusBar_Enter;
			labelStandardGravitationalParameterDesc.Leave += ClearStatusBar_Leave;
			labelStandardGravitationalParameterDesc.MouseDown += Control_MouseDown;
			labelStandardGravitationalParameterDesc.MouseEnter += SetStatusBar_Enter;
			labelStandardGravitationalParameterDesc.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMeanAxisData
			// 
			labelMeanAxisData.AccessibleDescription = "Shows the information of \"Mean axis\"";
			labelMeanAxisData.AccessibleName = "Shows the information of \"Mean axis\"";
			labelMeanAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMeanAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMeanAxisData.Dock = DockStyle.Fill;
			labelMeanAxisData.Location = new Point(280, 445);
			labelMeanAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMeanAxisData.Name = "labelMeanAxisData";
			labelMeanAxisData.Size = new Size(270, 20);
			labelMeanAxisData.TabIndex = 35;
			toolTip.SetToolTip(labelMeanAxisData, "Shows the information of \"Mean axis\"");
			labelMeanAxisData.Values.Text = "..................";
			labelMeanAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMeanAxisData.Enter += SetStatusBar_Enter;
			labelMeanAxisData.Leave += ClearStatusBar_Leave;
			labelMeanAxisData.MouseDown += Control_MouseDown;
			labelMeanAxisData.MouseEnter += SetStatusBar_Enter;
			labelMeanAxisData.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelStandardGravitationalParameterData
			// 
			labelStandardGravitationalParameterData.AccessibleDescription = "Shows the information of \"Standard gravitational parameter\"";
			labelStandardGravitationalParameterData.AccessibleName = "Shows the information of \"Standard gravitational parameter\"";
			labelStandardGravitationalParameterData.AccessibleRole = AccessibleRole.StaticText;
			labelStandardGravitationalParameterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelStandardGravitationalParameterData.Dock = DockStyle.Fill;
			labelStandardGravitationalParameterData.Location = new Point(280, 471);
			labelStandardGravitationalParameterData.Margin = new Padding(4, 3, 4, 3);
			labelStandardGravitationalParameterData.Name = "labelStandardGravitationalParameterData";
			labelStandardGravitationalParameterData.Size = new Size(270, 21);
			labelStandardGravitationalParameterData.TabIndex = 37;
			toolTip.SetToolTip(labelStandardGravitationalParameterData, "Shows the information of \"Standard gravitational parameter\"");
			labelStandardGravitationalParameterData.Values.Text = "..................";
			labelStandardGravitationalParameterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelStandardGravitationalParameterData.Enter += SetStatusBar_Enter;
			labelStandardGravitationalParameterData.Leave += ClearStatusBar_Leave;
			labelStandardGravitationalParameterData.MouseDown += Control_MouseDown;
			labelStandardGravitationalParameterData.MouseEnter += SetStatusBar_Enter;
			labelStandardGravitationalParameterData.MouseLeave += ClearStatusBar_Leave;
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
			toolStripContainer.ContentPanel.Size = new Size(554, 495);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(554, 542);
			toolStripContainer.TabIndex = 3;
			toolStripContainer.Text = "toolStripContainer";
			toolTip.SetToolTip(toolStripContainer, "Container to arrange the toolbars");
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(toolStripIcons);
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
			statusStrip.Size = new Size(554, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 1;
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
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Group the data";
			tableLayoutPanel.AccessibleName = "Table pane";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
			tableLayoutPanel.ColumnCount = 2;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.8194962F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.1805038F));
			tableLayoutPanel.ContextMenuStrip = contextMenuCopyToClipboard;
			tableLayoutPanel.Controls.Add(labelLinearEccentricityData, 1, 0);
			tableLayoutPanel.Controls.Add(labelLinearEccentricityDesc, 0, 0);
			tableLayoutPanel.Controls.Add(labelSemiMinorAxisDesc, 0, 1);
			tableLayoutPanel.Controls.Add(labelMajorAxisDesc, 0, 2);
			tableLayoutPanel.Controls.Add(labelMinorAxisDesc, 0, 3);
			tableLayoutPanel.Controls.Add(labelEccenctricAnomalyDesc, 0, 4);
			tableLayoutPanel.Controls.Add(labelTrueAnomalyDesc, 0, 5);
			tableLayoutPanel.Controls.Add(labelPerihelionDistanceDesc, 0, 6);
			tableLayoutPanel.Controls.Add(LabelAphelionDistanceDesc, 0, 7);
			tableLayoutPanel.Controls.Add(labelLongitudeDescendingNodeDesc, 0, 8);
			tableLayoutPanel.Controls.Add(labelArgumentAphelionDesc, 0, 9);
			tableLayoutPanel.Controls.Add(labelFocalParameterDesc, 0, 10);
			tableLayoutPanel.Controls.Add(labelSemiLatusRectumDesc, 0, 11);
			tableLayoutPanel.Controls.Add(labelLatusRectumDesc, 0, 12);
			tableLayoutPanel.Controls.Add(labelOrbitalPeriodDesc, 0, 13);
			tableLayoutPanel.Controls.Add(labelSemiMinorAxisData, 1, 1);
			tableLayoutPanel.Controls.Add(labelMajorAxisData, 1, 2);
			tableLayoutPanel.Controls.Add(labelMinorAxisData, 1, 3);
			tableLayoutPanel.Controls.Add(labelEccentricAnomalyData, 1, 4);
			tableLayoutPanel.Controls.Add(labelTrueAnomalyData, 1, 5);
			tableLayoutPanel.Controls.Add(labelPerihelionDistanceData, 1, 6);
			tableLayoutPanel.Controls.Add(labelAphelionDistanceData, 1, 7);
			tableLayoutPanel.Controls.Add(labelLongitudeDescendingNodeData, 1, 8);
			tableLayoutPanel.Controls.Add(labelArgumentAphelionData, 1, 9);
			tableLayoutPanel.Controls.Add(labelFocalParameterData, 1, 10);
			tableLayoutPanel.Controls.Add(labelSemiLatusRectumData, 1, 11);
			tableLayoutPanel.Controls.Add(labelLatusRectumData, 1, 12);
			tableLayoutPanel.Controls.Add(labelOrbitalPeriodData, 1, 13);
			tableLayoutPanel.Controls.Add(labelOrbitalAreaDesc, 0, 14);
			tableLayoutPanel.Controls.Add(labelOrbitalPerimeterDesc, 0, 15);
			tableLayoutPanel.Controls.Add(labelSemiMeanAxisDesc, 0, 16);
			tableLayoutPanel.Controls.Add(labelMeanAxisDesc, 0, 17);
			tableLayoutPanel.Controls.Add(labelOrbitalAreaData, 1, 14);
			tableLayoutPanel.Controls.Add(labelOrbitalPerimeterData, 1, 15);
			tableLayoutPanel.Controls.Add(labelSemiMeanAxisData, 1, 16);
			tableLayoutPanel.Controls.Add(labelMeanAxisData, 1, 17);
			tableLayoutPanel.Controls.Add(labelStandardGravitationalParameterData, 1, 18);
			tableLayoutPanel.Controls.Add(labelStandardGravitationalParameterDesc, 0, 18);
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.Location = new Point(0, 0);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 19;
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
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
			tableLayoutPanel.Size = new Size(554, 495);
			tableLayoutPanel.TabIndex = 0;
			tableLayoutPanel.TabStop = true;
			// 
			// toolStripIcons
			// 
			toolStripIcons.AccessibleDescription = "Toolbar of copying, printing and exporting";
			toolStripIcons.AccessibleName = "Toolbar of copying, printing and exporting";
			toolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			toolStripIcons.BackColor = Color.Transparent;
			toolStripIcons.Dock = DockStyle.None;
			toolStripIcons.Font = new Font("Segoe UI", 9F);
			toolStripIcons.Items.AddRange(new ToolStripItem[] { splitbuttonCopyToClipboard });
			toolStripIcons.Location = new Point(0, 0);
			toolStripIcons.Name = "toolStripIcons";
			toolStripIcons.Size = new Size(554, 25);
			toolStripIcons.Stretch = true;
			toolStripIcons.TabIndex = 0;
			toolStripIcons.TabStop = true;
			toolStripIcons.Text = "Toolbar of copying, printing and exporting";
			toolStripIcons.Enter += SetStatusBar_Enter;
			toolStripIcons.Leave += ClearStatusBar_Leave;
			toolStripIcons.MouseEnter += SetStatusBar_Enter;
			toolStripIcons.MouseLeave += ClearStatusBar_Leave;
			// 
			// splitbuttonCopyToClipboard
			// 
			splitbuttonCopyToClipboard.AccessibleDescription = "Copys to clipboard";
			splitbuttonCopyToClipboard.AccessibleName = "Copy to clipboard";
			splitbuttonCopyToClipboard.AccessibleRole = AccessibleRole.SplitButton;
			splitbuttonCopyToClipboard.BackColor = Color.Transparent;
			splitbuttonCopyToClipboard.DisplayStyle = ToolStripItemDisplayStyle.Image;
			splitbuttonCopyToClipboard.DropDown = contextMenuFullCopyToClipboardDerivatedOrbitalElements;
			splitbuttonCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			splitbuttonCopyToClipboard.ImageTransparentColor = Color.Magenta;
			splitbuttonCopyToClipboard.Name = "splitbuttonCopyToClipboard";
			splitbuttonCopyToClipboard.Size = new Size(32, 22);
			splitbuttonCopyToClipboard.Text = "Copy to clipboard";
			splitbuttonCopyToClipboard.ButtonClick += ToolStripButtonCopyToClipboard_Click;
			splitbuttonCopyToClipboard.MouseEnter += SetStatusBar_Enter;
			splitbuttonCopyToClipboard.MouseLeave += ClearStatusBar_Leave;
			// 
			// contextMenuFullCopyToClipboardDerivatedOrbitalElements
			// 
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.AccessibleDescription = "Shows the context menu of the derivated orbital elements to copy to clipboard";
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.AccessibleName = "context menu of the derivated orbital elements to copy to clipboard";
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.Font = new Font("Segoe UI", 9F);
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.Items.AddRange(new ToolStripItem[] { menuitemCopyToClipboardLinearEccentricity, menuitemCopyToClipboardSemiMinorAxis, menuitemCopyToClipboardMajorAxis, menuitemCopyToClipboardMinorAxis, menuitemCopyToClipboardEccentricAnomaly, menuitemCopyToClipboardTrueAnomaly, menuitemCopyToClipboardPerihelionDistance, menuitemCopyToClipboardAphelionDistance, menuitemCopyToClipboardLongitudeDescendingNode, menuitemCopyToClipboardArgumentAphelion, menuitemCopyToClipboardFocalParameter, menuitemCopyToClipboardSemiLatusRectum, menuitemCopyToClipboardLatusRectum, menuitemCopyToClipboardOrbitalPeriod, menuitemCopyToClipboardOrbitalArea, menuitemCopyToClipboardSemiMeanAxis, menuitemCopyToClipboardMeanAxis, menuitemCopyToClipboardStandardGravitationalParameter });
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.Name = resources.GetString("contextMenuFullCopyToClipboardDerivatedOrbitalElements.Name");
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.Size = new Size(257, 400);
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.Text = "Copy to clipboard";
			toolTip.SetToolTip(contextMenuFullCopyToClipboardDerivatedOrbitalElements, "Copy to clipboard");
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.MouseEnter += SetStatusBar_Enter;
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardLinearEccentricity
			// 
			menuitemCopyToClipboardLinearEccentricity.AccessibleDescription = "Copy to clipboard: Linear eccentricity";
			menuitemCopyToClipboardLinearEccentricity.AccessibleName = "Copy to clipboard: Linear eccentricity";
			menuitemCopyToClipboardLinearEccentricity.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardLinearEccentricity.AutoToolTip = true;
			menuitemCopyToClipboardLinearEccentricity.Image = (Image)resources.GetObject("menuitemCopyToClipboardLinearEccentricity.Image");
			menuitemCopyToClipboardLinearEccentricity.Name = "menuitemCopyToClipboardLinearEccentricity";
			menuitemCopyToClipboardLinearEccentricity.Size = new Size(256, 22);
			menuitemCopyToClipboardLinearEccentricity.Text = "Linear eccentricity";
			menuitemCopyToClipboardLinearEccentricity.Click += MenuitemCopyToClipboardLinearEccentricity_Click;
			menuitemCopyToClipboardLinearEccentricity.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardLinearEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardSemiMinorAxis
			// 
			menuitemCopyToClipboardSemiMinorAxis.AccessibleDescription = "Copy to clipboard: Semi minor axis";
			menuitemCopyToClipboardSemiMinorAxis.AccessibleName = "Copy to clipboard: Semi minor axis";
			menuitemCopyToClipboardSemiMinorAxis.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardSemiMinorAxis.AutoToolTip = true;
			menuitemCopyToClipboardSemiMinorAxis.Image = (Image)resources.GetObject("menuitemCopyToClipboardSemiMinorAxis.Image");
			menuitemCopyToClipboardSemiMinorAxis.Name = "menuitemCopyToClipboardSemiMinorAxis";
			menuitemCopyToClipboardSemiMinorAxis.Size = new Size(256, 22);
			menuitemCopyToClipboardSemiMinorAxis.Text = "Semi minor axis";
			menuitemCopyToClipboardSemiMinorAxis.Click += MenuitemCopyToClipboardSemiMinorAxis_Click;
			menuitemCopyToClipboardSemiMinorAxis.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardSemiMinorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardMajorAxis
			// 
			menuitemCopyToClipboardMajorAxis.AccessibleDescription = "Copy to clipboard: Major axis";
			menuitemCopyToClipboardMajorAxis.AccessibleName = "Copy to clipboard: Major axis";
			menuitemCopyToClipboardMajorAxis.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardMajorAxis.AutoToolTip = true;
			menuitemCopyToClipboardMajorAxis.Image = (Image)resources.GetObject("menuitemCopyToClipboardMajorAxis.Image");
			menuitemCopyToClipboardMajorAxis.Name = "menuitemCopyToClipboardMajorAxis";
			menuitemCopyToClipboardMajorAxis.Size = new Size(256, 22);
			menuitemCopyToClipboardMajorAxis.Text = "Major axis";
			menuitemCopyToClipboardMajorAxis.Click += MenuitemCopyToClipboardMajorAxis_Click;
			menuitemCopyToClipboardMajorAxis.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardMinorAxis
			// 
			menuitemCopyToClipboardMinorAxis.AccessibleDescription = "Copy to clipboard: Minor axis";
			menuitemCopyToClipboardMinorAxis.AccessibleName = "Copy to clipboard: Minor axis";
			menuitemCopyToClipboardMinorAxis.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardMinorAxis.AutoToolTip = true;
			menuitemCopyToClipboardMinorAxis.Image = (Image)resources.GetObject("menuitemCopyToClipboardMinorAxis.Image");
			menuitemCopyToClipboardMinorAxis.Name = "menuitemCopyToClipboardMinorAxis";
			menuitemCopyToClipboardMinorAxis.Size = new Size(256, 22);
			menuitemCopyToClipboardMinorAxis.Text = "Minor axis";
			menuitemCopyToClipboardMinorAxis.Click += MenuitemCopyToClipboardMinorAxis_Click;
			menuitemCopyToClipboardMinorAxis.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardMinorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardEccentricAnomaly
			// 
			menuitemCopyToClipboardEccentricAnomaly.AccessibleDescription = "Copy to clipboard: Eccentric anomaly";
			menuitemCopyToClipboardEccentricAnomaly.AccessibleName = "Copy to clipboard: Eccentric anomaly";
			menuitemCopyToClipboardEccentricAnomaly.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardEccentricAnomaly.AutoToolTip = true;
			menuitemCopyToClipboardEccentricAnomaly.Image = (Image)resources.GetObject("menuitemCopyToClipboardEccentricAnomaly.Image");
			menuitemCopyToClipboardEccentricAnomaly.Name = "menuitemCopyToClipboardEccentricAnomaly";
			menuitemCopyToClipboardEccentricAnomaly.Size = new Size(256, 22);
			menuitemCopyToClipboardEccentricAnomaly.Text = "Eccentric anomaly";
			menuitemCopyToClipboardEccentricAnomaly.Click += MenuitemCopyToClipboardEccentricAnomaly_Click;
			menuitemCopyToClipboardEccentricAnomaly.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardEccentricAnomaly.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardTrueAnomaly
			// 
			menuitemCopyToClipboardTrueAnomaly.AccessibleDescription = "Copy to clipboard: True anomaly";
			menuitemCopyToClipboardTrueAnomaly.AccessibleName = "Copy to clipboard: True anomaly";
			menuitemCopyToClipboardTrueAnomaly.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardTrueAnomaly.AutoToolTip = true;
			menuitemCopyToClipboardTrueAnomaly.Image = (Image)resources.GetObject("menuitemCopyToClipboardTrueAnomaly.Image");
			menuitemCopyToClipboardTrueAnomaly.Name = "menuitemCopyToClipboardTrueAnomaly";
			menuitemCopyToClipboardTrueAnomaly.Size = new Size(256, 22);
			menuitemCopyToClipboardTrueAnomaly.Text = "True anomaly";
			menuitemCopyToClipboardTrueAnomaly.Click += MenuitemCopyToClipboardTrueAnomaly_Click;
			menuitemCopyToClipboardTrueAnomaly.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardTrueAnomaly.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardPerihelionDistance
			// 
			menuitemCopyToClipboardPerihelionDistance.AccessibleDescription = "Copy to clipboard: Perihelion distance";
			menuitemCopyToClipboardPerihelionDistance.AccessibleName = "Copy to clipboard: Perihelion distance";
			menuitemCopyToClipboardPerihelionDistance.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardPerihelionDistance.AutoToolTip = true;
			menuitemCopyToClipboardPerihelionDistance.Image = (Image)resources.GetObject("menuitemCopyToClipboardPerihelionDistance.Image");
			menuitemCopyToClipboardPerihelionDistance.Name = "menuitemCopyToClipboardPerihelionDistance";
			menuitemCopyToClipboardPerihelionDistance.Size = new Size(256, 22);
			menuitemCopyToClipboardPerihelionDistance.Text = "Perihelion distance";
			menuitemCopyToClipboardPerihelionDistance.Click += MenuitemCopyToClipboardPerihelionDistance_Click;
			menuitemCopyToClipboardPerihelionDistance.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardPerihelionDistance.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardAphelionDistance
			// 
			menuitemCopyToClipboardAphelionDistance.AccessibleDescription = "Copy to clipboard: Aphelion distance";
			menuitemCopyToClipboardAphelionDistance.AccessibleName = "Copy to clipboard: Aphelion distance";
			menuitemCopyToClipboardAphelionDistance.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardAphelionDistance.AutoToolTip = true;
			menuitemCopyToClipboardAphelionDistance.Image = (Image)resources.GetObject("menuitemCopyToClipboardAphelionDistance.Image");
			menuitemCopyToClipboardAphelionDistance.Name = "menuitemCopyToClipboardAphelionDistance";
			menuitemCopyToClipboardAphelionDistance.Size = new Size(256, 22);
			menuitemCopyToClipboardAphelionDistance.Text = "Aphelion distance";
			menuitemCopyToClipboardAphelionDistance.Click += MenuitemCopyToClipboardAphelionDistance_Click;
			menuitemCopyToClipboardAphelionDistance.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardAphelionDistance.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardLongitudeDescendingNode
			// 
			menuitemCopyToClipboardLongitudeDescendingNode.AccessibleDescription = "Copy to clipboard: Longitude of the descending node";
			menuitemCopyToClipboardLongitudeDescendingNode.AccessibleName = "Copy to clipboard: Longitude of the descending node";
			menuitemCopyToClipboardLongitudeDescendingNode.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardLongitudeDescendingNode.AutoToolTip = true;
			menuitemCopyToClipboardLongitudeDescendingNode.Image = (Image)resources.GetObject("menuitemCopyToClipboardLongitudeDescendingNode.Image");
			menuitemCopyToClipboardLongitudeDescendingNode.Name = "menuitemCopyToClipboardLongitudeDescendingNode";
			menuitemCopyToClipboardLongitudeDescendingNode.Size = new Size(256, 22);
			menuitemCopyToClipboardLongitudeDescendingNode.Text = "Longitude of the descending node";
			menuitemCopyToClipboardLongitudeDescendingNode.Click += MenuitemCopyToClipboardLongitudeDescendingNode_Click;
			menuitemCopyToClipboardLongitudeDescendingNode.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardLongitudeDescendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardArgumentAphelion
			// 
			menuitemCopyToClipboardArgumentAphelion.AccessibleDescription = "Copy to clipboard: Argument of aphelion";
			menuitemCopyToClipboardArgumentAphelion.AccessibleName = "Copy to clipboard: Argument of aphelion";
			menuitemCopyToClipboardArgumentAphelion.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardArgumentAphelion.AutoToolTip = true;
			menuitemCopyToClipboardArgumentAphelion.Image = (Image)resources.GetObject("menuitemCopyToClipboardArgumentAphelion.Image");
			menuitemCopyToClipboardArgumentAphelion.Name = "menuitemCopyToClipboardArgumentAphelion";
			menuitemCopyToClipboardArgumentAphelion.Size = new Size(256, 22);
			menuitemCopyToClipboardArgumentAphelion.Text = "Argument of aphelion";
			menuitemCopyToClipboardArgumentAphelion.Click += MenuitemCopyToClipboardArgumentAphelion_Click;
			menuitemCopyToClipboardArgumentAphelion.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardArgumentAphelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardFocalParameter
			// 
			menuitemCopyToClipboardFocalParameter.AccessibleDescription = "Copy to clipboard: Focal parameter";
			menuitemCopyToClipboardFocalParameter.AccessibleName = "Copy to clipboard: Focal parameter";
			menuitemCopyToClipboardFocalParameter.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardFocalParameter.AutoToolTip = true;
			menuitemCopyToClipboardFocalParameter.Image = (Image)resources.GetObject("menuitemCopyToClipboardFocalParameter.Image");
			menuitemCopyToClipboardFocalParameter.Name = "menuitemCopyToClipboardFocalParameter";
			menuitemCopyToClipboardFocalParameter.Size = new Size(256, 22);
			menuitemCopyToClipboardFocalParameter.Text = "Focal parameter";
			menuitemCopyToClipboardFocalParameter.Click += MenuitemCopyToClipboardFocalParameter_Click;
			menuitemCopyToClipboardFocalParameter.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardFocalParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardSemiLatusRectum
			// 
			menuitemCopyToClipboardSemiLatusRectum.AccessibleDescription = "Copy to clipboard: Semi-latus rectum";
			menuitemCopyToClipboardSemiLatusRectum.AccessibleName = "Copy to clipboard: Semi-latus rectum";
			menuitemCopyToClipboardSemiLatusRectum.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardSemiLatusRectum.AutoToolTip = true;
			menuitemCopyToClipboardSemiLatusRectum.Image = (Image)resources.GetObject("menuitemCopyToClipboardSemiLatusRectum.Image");
			menuitemCopyToClipboardSemiLatusRectum.Name = "menuitemCopyToClipboardSemiLatusRectum";
			menuitemCopyToClipboardSemiLatusRectum.Size = new Size(256, 22);
			menuitemCopyToClipboardSemiLatusRectum.Text = "Semi-latus rectum";
			menuitemCopyToClipboardSemiLatusRectum.Click += MenuitemCopyToClipboardSemiLatusRectum_Click;
			menuitemCopyToClipboardSemiLatusRectum.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardSemiLatusRectum.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardLatusRectum
			// 
			menuitemCopyToClipboardLatusRectum.AccessibleDescription = "Copy to clipboard: Latus rectum";
			menuitemCopyToClipboardLatusRectum.AccessibleName = "Copy to clipboard: Latus rectum";
			menuitemCopyToClipboardLatusRectum.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardLatusRectum.AutoToolTip = true;
			menuitemCopyToClipboardLatusRectum.Image = (Image)resources.GetObject("menuitemCopyToClipboardLatusRectum.Image");
			menuitemCopyToClipboardLatusRectum.Name = "menuitemCopyToClipboardLatusRectum";
			menuitemCopyToClipboardLatusRectum.Size = new Size(256, 22);
			menuitemCopyToClipboardLatusRectum.Text = "Latus rectum";
			menuitemCopyToClipboardLatusRectum.Click += MenuitemCopyToClipboardLatusRectum_Click;
			menuitemCopyToClipboardLatusRectum.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardLatusRectum.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardOrbitalPeriod
			// 
			menuitemCopyToClipboardOrbitalPeriod.AccessibleDescription = "Copy to clipboard: Orbital period";
			menuitemCopyToClipboardOrbitalPeriod.AccessibleName = "Copy to clipboard: Orbital period";
			menuitemCopyToClipboardOrbitalPeriod.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardOrbitalPeriod.AutoToolTip = true;
			menuitemCopyToClipboardOrbitalPeriod.Image = (Image)resources.GetObject("menuitemCopyToClipboardOrbitalPeriod.Image");
			menuitemCopyToClipboardOrbitalPeriod.Name = "menuitemCopyToClipboardOrbitalPeriod";
			menuitemCopyToClipboardOrbitalPeriod.Size = new Size(256, 22);
			menuitemCopyToClipboardOrbitalPeriod.Text = "Orbital period";
			menuitemCopyToClipboardOrbitalPeriod.Click += MenuitemCopyToClipboardOrbitalPeriod_Click;
			menuitemCopyToClipboardOrbitalPeriod.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardOrbitalPeriod.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardOrbitalArea
			// 
			menuitemCopyToClipboardOrbitalArea.AccessibleDescription = "Copy to clipboard: Orbital area";
			menuitemCopyToClipboardOrbitalArea.AccessibleName = "Copy to clipboard: Orbital area";
			menuitemCopyToClipboardOrbitalArea.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardOrbitalArea.AutoToolTip = true;
			menuitemCopyToClipboardOrbitalArea.Image = (Image)resources.GetObject("menuitemCopyToClipboardOrbitalArea.Image");
			menuitemCopyToClipboardOrbitalArea.Name = "menuitemCopyToClipboardOrbitalArea";
			menuitemCopyToClipboardOrbitalArea.Size = new Size(256, 22);
			menuitemCopyToClipboardOrbitalArea.Text = "Orbital area";
			menuitemCopyToClipboardOrbitalArea.Click += MenuitemCopyToClipboardOrbitalArea_Click;
			menuitemCopyToClipboardOrbitalArea.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardOrbitalArea.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardSemiMeanAxis
			// 
			menuitemCopyToClipboardSemiMeanAxis.AccessibleDescription = "Copy to clipboard: Semi-mean axis";
			menuitemCopyToClipboardSemiMeanAxis.AccessibleName = "Copy to clipboard: Semi-mean axis";
			menuitemCopyToClipboardSemiMeanAxis.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardSemiMeanAxis.AutoToolTip = true;
			menuitemCopyToClipboardSemiMeanAxis.Image = (Image)resources.GetObject("menuitemCopyToClipboardSemiMeanAxis.Image");
			menuitemCopyToClipboardSemiMeanAxis.Name = "menuitemCopyToClipboardSemiMeanAxis";
			menuitemCopyToClipboardSemiMeanAxis.Size = new Size(256, 22);
			menuitemCopyToClipboardSemiMeanAxis.Text = "Semi-mean axis";
			menuitemCopyToClipboardSemiMeanAxis.Click += MenuitemCopyToClipboardSemiMeanAxis_Click;
			menuitemCopyToClipboardSemiMeanAxis.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardSemiMeanAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardMeanAxis
			// 
			menuitemCopyToClipboardMeanAxis.AccessibleDescription = "Copy to clipboard: Mean axis";
			menuitemCopyToClipboardMeanAxis.AccessibleName = "Copy to clipboard: Mean axis";
			menuitemCopyToClipboardMeanAxis.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardMeanAxis.AutoToolTip = true;
			menuitemCopyToClipboardMeanAxis.Image = (Image)resources.GetObject("menuitemCopyToClipboardMeanAxis.Image");
			menuitemCopyToClipboardMeanAxis.Name = "menuitemCopyToClipboardMeanAxis";
			menuitemCopyToClipboardMeanAxis.Size = new Size(256, 22);
			menuitemCopyToClipboardMeanAxis.Text = "Mean axis";
			menuitemCopyToClipboardMeanAxis.Click += MenuitemCopyToClipboardMeanAxis_Click;
			menuitemCopyToClipboardMeanAxis.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardMeanAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// menuitemCopyToClipboardStandardGravitationalParameter
			// 
			menuitemCopyToClipboardStandardGravitationalParameter.AccessibleDescription = "Copy to clipboard: Standard gravitational parameter";
			menuitemCopyToClipboardStandardGravitationalParameter.AccessibleName = "Copy to clipboard: Standard gravitational parameter";
			menuitemCopyToClipboardStandardGravitationalParameter.AccessibleRole = AccessibleRole.MenuItem;
			menuitemCopyToClipboardStandardGravitationalParameter.AutoToolTip = true;
			menuitemCopyToClipboardStandardGravitationalParameter.Image = (Image)resources.GetObject("menuitemCopyToClipboardStandardGravitationalParameter.Image");
			menuitemCopyToClipboardStandardGravitationalParameter.Name = "menuitemCopyToClipboardStandardGravitationalParameter";
			menuitemCopyToClipboardStandardGravitationalParameter.Size = new Size(256, 22);
			menuitemCopyToClipboardStandardGravitationalParameter.Text = "Standard gravitational parameter";
			menuitemCopyToClipboardStandardGravitationalParameter.Click += MenuitemCopyToClipboardStandardGravitationalParameter_Click;
			menuitemCopyToClipboardStandardGravitationalParameter.MouseEnter += SetStatusBar_Enter;
			menuitemCopyToClipboardStandardGravitationalParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// DerivativeOrbitElementsForm
			// 
			AccessibleDescription = "Calculates some derivated orbit elements";
			AccessibleName = "Derivated orbit elements";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(554, 542);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "DerivativeOrbitElementsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Derivated orbit elements";
			toolTip.SetToolTip(this, "Derivated orbit elements");
			FormClosed += DerivativeOrbitElementsForm_FormClosed;
			Load += DerivativeOrbitElementsForm_Load;
			contextMenuOpenTerminology.ResumeLayout(false);
			contextMenuCopyToClipboard.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			toolStripIcons.ResumeLayout(false);
			toolStripIcons.PerformLayout();
			contextMenuFullCopyToClipboardDerivatedOrbitalElements.ResumeLayout(false);
			ResumeLayout(false);

		}

		#endregion
		private ToolTip toolTip;
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonLabel labelLinearEccentricityData;
		private KryptonLabel labelLinearEccentricityDesc;
		private KryptonLabel labelSemiMinorAxisDesc;
		private KryptonLabel labelMajorAxisDesc;
		private KryptonLabel labelTrueAnomalyDesc;
		private KryptonLabel labelPerihelionDistanceDesc;
		private KryptonLabel LabelAphelionDistanceDesc;
		private KryptonLabel labelLongitudeDescendingNodeDesc;
		private KryptonLabel labelArgumentAphelionDesc;
		private KryptonLabel labelFocalParameterDesc;
		private KryptonLabel labelSemiLatusRectumDesc;
		private KryptonLabel labelLatusRectumDesc;
		private KryptonLabel labelOrbitalPeriodDesc;
		private KryptonLabel labelMinorAxisDesc;
		private KryptonLabel labelEccenctricAnomalyDesc;
		private KryptonLabel labelOrbitalPeriodData;
		private KryptonLabel labelSemiMinorAxisData;
		private KryptonLabel labelMajorAxisData;
		private KryptonLabel labelMinorAxisData;
		private KryptonLabel labelEccentricAnomalyData;
		private KryptonLabel labelTrueAnomalyData;
		private KryptonLabel labelPerihelionDistanceData;
		private KryptonLabel labelAphelionDistanceData;
		private KryptonLabel labelLongitudeDescendingNodeData;
		private KryptonLabel labelArgumentAphelionData;
		private KryptonLabel labelFocalParameterData;
		private KryptonLabel labelSemiLatusRectumData;
		private KryptonLabel labelLatusRectumData;
		private KryptonLabel labelOrbitalAreaDesc;
		private KryptonLabel labelOrbitalPerimeterDesc;
		private KryptonLabel labelSemiMeanAxisDesc;
		private KryptonLabel labelMeanAxisDesc;
		private KryptonLabel labelOrbitalAreaData;
		private KryptonLabel labelOrbitalPerimeterData;
		private KryptonLabel labelSemiMeanAxisData;
		private KryptonLabel labelMeanAxisData;
		private KryptonLabel labelStandardGravitationalParameterData;
		private KryptonLabel labelStandardGravitationalParameterDesc;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonManager kryptonManager;
		private ContextMenuStrip contextMenuCopyToClipboard;
		private ToolStripMenuItem ToolStripMenuItemCpyToClipboard;
		private ContextMenuStrip contextMenuOpenTerminology;
		private ToolStripMenuItem toolStripMenuItemOpenTerminology;
		private ToolStrip toolStripIcons;
		private ToolStripSplitButton splitbuttonCopyToClipboard;
		private ContextMenuStrip contextMenuFullCopyToClipboardDerivatedOrbitalElements;
		private ToolStripMenuItem menuitemCopyToClipboardLinearEccentricity;
		private ToolStripMenuItem menuitemCopyToClipboardSemiMinorAxis;
		private ToolStripMenuItem menuitemCopyToClipboardMajorAxis;
		private ToolStripMenuItem menuitemCopyToClipboardMinorAxis;
		private ToolStripMenuItem menuitemCopyToClipboardEccentricAnomaly;
		private ToolStripMenuItem menuitemCopyToClipboardTrueAnomaly;
		private ToolStripMenuItem menuitemCopyToClipboardPerihelionDistance;
		private ToolStripMenuItem menuitemCopyToClipboardAphelionDistance;
		private ToolStripMenuItem menuitemCopyToClipboardLongitudeDescendingNode;
		private ToolStripMenuItem menuitemCopyToClipboardArgumentAphelion;
		private ToolStripMenuItem menuitemCopyToClipboardFocalParameter;
		private ToolStripMenuItem menuitemCopyToClipboardSemiLatusRectum;
		private ToolStripMenuItem menuitemCopyToClipboardLatusRectum;
		private ToolStripMenuItem menuitemCopyToClipboardOrbitalPeriod;
		private ToolStripMenuItem menuitemCopyToClipboardOrbitalArea;
		private ToolStripMenuItem menuitemCopyToClipboardSemiMeanAxis;
		private ToolStripMenuItem menuitemCopyToClipboardMeanAxis;
		private ToolStripMenuItem menuitemCopyToClipboardStandardGravitationalParameter;
	}
}