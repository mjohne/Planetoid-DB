using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class DerivedOrbitElementsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(DerivedOrbitElementsForm));
			labelLinearEccentricityDesc = new KryptonLabel();
			contextMenuOpenTerminology = new ContextMenuStrip(components);
			toolStripMenuItemOpenTerminology = new ToolStripMenuItem();
			labelLinearEccentricityData = new KryptonLabel();
			contextMenuCopyToClipboard = new ContextMenuStrip(components);
			ToolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
			labelSemiMinorAxisDesc = new KryptonLabel();
			labelMajorAxisDesc = new KryptonLabel();
			labelMinorAxisDesc = new KryptonLabel();
			labelEccenctricAnomalyDesc = new KryptonLabel();
			labelTrueAnomalyDesc = new KryptonLabel();
			labelPerihelionDistanceDesc = new KryptonLabel();
			LabelAphelionDistanceDesc = new KryptonLabel();
			labelLongitudeOfTheDescendingNodeDesc = new KryptonLabel();
			labelArgumentOfTheAphelionDesc = new KryptonLabel();
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
			contextMenuFullCopyToClipboardDerivedOrbitalElements = new ContextMenuStrip(components);
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
			contextMenuFullCopyToClipboardDerivedOrbitalElements.SuspendLayout();
			SuspendLayout();
			// 
			// labelLinearEccentricityDesc
			// 
			labelLinearEccentricityDesc.AccessibleDescription = "Shows the linear eccentricity (AU)";
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
			labelLinearEccentricityDesc.ToolTipValues.Description = "Shows the linear eccentricity (AU).\r\nDouble-click or right-click to open the terminology.";
			labelLinearEccentricityDesc.ToolTipValues.EnableToolTips = true;
			labelLinearEccentricityDesc.ToolTipValues.Heading = "Linear eccentricity (AU)";
			labelLinearEccentricityDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLinearEccentricityDesc.Values.ExtraText = "AU";
			labelLinearEccentricityDesc.Values.Text = "Linear eccentricity";
			labelLinearEccentricityDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLinearEccentricityDesc.Enter += Control_Enter;
			labelLinearEccentricityDesc.Leave += Control_Leave;
			labelLinearEccentricityDesc.MouseDown += Control_MouseDown;
			labelLinearEccentricityDesc.MouseEnter += Control_Enter;
			labelLinearEccentricityDesc.MouseLeave += Control_Leave;
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
			contextMenuOpenTerminology.MouseEnter += Control_Enter;
			contextMenuOpenTerminology.MouseLeave += Control_Leave;
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
			toolStripMenuItemOpenTerminology.MouseEnter += Control_Enter;
			toolStripMenuItemOpenTerminology.MouseLeave += Control_Leave;
			// 
			// labelLinearEccentricityData
			// 
			labelLinearEccentricityData.AccessibleDescription = "Shows the information of \"Linear eccentricity\"";
			labelLinearEccentricityData.AccessibleName = "Information of \"Linear eccentricity\"";
			labelLinearEccentricityData.AccessibleRole = AccessibleRole.StaticText;
			labelLinearEccentricityData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLinearEccentricityData.Dock = DockStyle.Fill;
			labelLinearEccentricityData.Location = new Point(280, 3);
			labelLinearEccentricityData.Margin = new Padding(4, 3, 4, 3);
			labelLinearEccentricityData.Name = "labelLinearEccentricityData";
			labelLinearEccentricityData.Size = new Size(270, 20);
			labelLinearEccentricityData.TabIndex = 1;
			labelLinearEccentricityData.ToolTipValues.Description = "Shows the information of \"Linear eccentricity\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelLinearEccentricityData.ToolTipValues.EnableToolTips = true;
			labelLinearEccentricityData.ToolTipValues.Heading = "Information of \"Linear eccentricity\"";
			labelLinearEccentricityData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLinearEccentricityData.Values.Text = "..................";
			labelLinearEccentricityData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLinearEccentricityData.Enter += Control_Enter;
			labelLinearEccentricityData.Leave += Control_Leave;
			labelLinearEccentricityData.MouseDown += Control_MouseDown;
			labelLinearEccentricityData.MouseEnter += Control_Enter;
			labelLinearEccentricityData.MouseLeave += Control_Leave;
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
			// labelSemiMinorAxisDesc
			// 
			labelSemiMinorAxisDesc.AccessibleDescription = "Shows the semi-minor axis (AU)";
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
			labelSemiMinorAxisDesc.ToolTipValues.Description = "Shows the semi-minor axis (AU).\r\nDouble-click or right-click to open the terminology.";
			labelSemiMinorAxisDesc.ToolTipValues.EnableToolTips = true;
			labelSemiMinorAxisDesc.ToolTipValues.Heading = "Semi-minor axis (AU)";
			labelSemiMinorAxisDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiMinorAxisDesc.Values.ExtraText = "AU";
			labelSemiMinorAxisDesc.Values.Text = "Semi-minor axis";
			labelSemiMinorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiMinorAxisDesc.Enter += Control_Enter;
			labelSemiMinorAxisDesc.Leave += Control_Leave;
			labelSemiMinorAxisDesc.MouseDown += Control_MouseDown;
			labelSemiMinorAxisDesc.MouseEnter += Control_Enter;
			labelSemiMinorAxisDesc.MouseLeave += Control_Leave;
			// 
			// labelMajorAxisDesc
			// 
			labelMajorAxisDesc.AccessibleDescription = "Shows the major axis (AU)";
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
			labelMajorAxisDesc.ToolTipValues.Description = "Shows the major axis (AU).\r\nDouble-click or right-click to open the terminology.";
			labelMajorAxisDesc.ToolTipValues.EnableToolTips = true;
			labelMajorAxisDesc.ToolTipValues.Heading = "Major axis (AU)";
			labelMajorAxisDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMajorAxisDesc.Values.ExtraText = "AU";
			labelMajorAxisDesc.Values.Text = "Major axis";
			labelMajorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMajorAxisDesc.Enter += Control_Enter;
			labelMajorAxisDesc.Leave += Control_Leave;
			labelMajorAxisDesc.MouseDown += Control_MouseDown;
			labelMajorAxisDesc.MouseEnter += Control_Enter;
			labelMajorAxisDesc.MouseLeave += Control_Leave;
			// 
			// labelMinorAxisDesc
			// 
			labelMinorAxisDesc.AccessibleDescription = "Shows the minor axis (AU)";
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
			labelMinorAxisDesc.ToolTipValues.Description = "Shows the minor axis (AU).\r\nDouble-click or right-click to open the terminology.";
			labelMinorAxisDesc.ToolTipValues.EnableToolTips = true;
			labelMinorAxisDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMinorAxisDesc.Values.ExtraText = "AU";
			labelMinorAxisDesc.Values.Text = "Minor axis";
			labelMinorAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMinorAxisDesc.Enter += Control_Enter;
			labelMinorAxisDesc.Leave += Control_Leave;
			labelMinorAxisDesc.MouseDown += Control_MouseDown;
			labelMinorAxisDesc.MouseEnter += Control_Enter;
			labelMinorAxisDesc.MouseLeave += Control_Leave;
			// 
			// labelEccenctricAnomalyDesc
			// 
			labelEccenctricAnomalyDesc.AccessibleDescription = "Shows the eccentric anomaly (degrees)";
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
			labelEccenctricAnomalyDesc.ToolTipValues.Description = "Shows the eccentric anomaly (degrees).\r\nDouble-click or right-click to open the terminology.";
			labelEccenctricAnomalyDesc.ToolTipValues.EnableToolTips = true;
			labelEccenctricAnomalyDesc.ToolTipValues.Heading = "Eccentric anomaly (degrees)";
			labelEccenctricAnomalyDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelEccenctricAnomalyDesc.Values.ExtraText = "°";
			labelEccenctricAnomalyDesc.Values.Text = "Eccentric anomaly";
			labelEccenctricAnomalyDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelEccenctricAnomalyDesc.Enter += Control_Enter;
			labelEccenctricAnomalyDesc.Leave += Control_Leave;
			labelEccenctricAnomalyDesc.MouseDown += Control_MouseDown;
			labelEccenctricAnomalyDesc.MouseEnter += Control_Enter;
			labelEccenctricAnomalyDesc.MouseLeave += Control_Leave;
			// 
			// labelTrueAnomalyDesc
			// 
			labelTrueAnomalyDesc.AccessibleDescription = "Show sthe true anomaly (degrees)";
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
			labelTrueAnomalyDesc.ToolTipValues.Description = "Shows the true anomaly (degrees).\r\nDouble-click or right-click to open the terminology.";
			labelTrueAnomalyDesc.ToolTipValues.EnableToolTips = true;
			labelTrueAnomalyDesc.ToolTipValues.Heading = "True anomaly (degrees)";
			labelTrueAnomalyDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelTrueAnomalyDesc.Values.ExtraText = "°";
			labelTrueAnomalyDesc.Values.Text = "True anomaly";
			labelTrueAnomalyDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelTrueAnomalyDesc.Enter += Control_Enter;
			labelTrueAnomalyDesc.Leave += Control_Leave;
			labelTrueAnomalyDesc.MouseDown += Control_MouseDown;
			labelTrueAnomalyDesc.MouseEnter += Control_Enter;
			labelTrueAnomalyDesc.MouseLeave += Control_Leave;
			// 
			// labelPerihelionDistanceDesc
			// 
			labelPerihelionDistanceDesc.AccessibleDescription = "Shows the perihelion distance (AU)";
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
			labelPerihelionDistanceDesc.ToolTipValues.Description = "Shows the perihelion distance (AU).\r\nDouble-click or right-click to open the terminology.";
			labelPerihelionDistanceDesc.ToolTipValues.EnableToolTips = true;
			labelPerihelionDistanceDesc.ToolTipValues.Heading = "Perihelion distance (AU)";
			labelPerihelionDistanceDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelPerihelionDistanceDesc.Values.ExtraText = "AU";
			labelPerihelionDistanceDesc.Values.Text = "Perihelion distance";
			labelPerihelionDistanceDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelPerihelionDistanceDesc.Enter += Control_Enter;
			labelPerihelionDistanceDesc.Leave += Control_Leave;
			labelPerihelionDistanceDesc.MouseDown += Control_MouseDown;
			labelPerihelionDistanceDesc.MouseEnter += Control_Enter;
			labelPerihelionDistanceDesc.MouseLeave += Control_Leave;
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
			LabelAphelionDistanceDesc.ToolTipValues.Description = "Shows the aphelion distance (AU).\r\nDouble-click or right-click to open the terminology.";
			LabelAphelionDistanceDesc.ToolTipValues.EnableToolTips = true;
			LabelAphelionDistanceDesc.ToolTipValues.Heading = "Aphelion distance (AU)";
			LabelAphelionDistanceDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			LabelAphelionDistanceDesc.Values.ExtraText = "AU";
			LabelAphelionDistanceDesc.Values.Text = "Aphelion distance";
			LabelAphelionDistanceDesc.DoubleClick += OpenTerminology_DoubleClick;
			LabelAphelionDistanceDesc.Enter += Control_Enter;
			LabelAphelionDistanceDesc.Leave += Control_Leave;
			LabelAphelionDistanceDesc.MouseDown += Control_MouseDown;
			LabelAphelionDistanceDesc.MouseEnter += Control_Enter;
			LabelAphelionDistanceDesc.MouseLeave += Control_Leave;
			// 
			// labelLongitudeOfTheDescendingNodeDesc
			// 
			labelLongitudeOfTheDescendingNodeDesc.AccessibleDescription = "Shows the longitude of the descending node (degrees)";
			labelLongitudeOfTheDescendingNodeDesc.AccessibleName = "Longitude of the descending node (degrees)";
			labelLongitudeOfTheDescendingNodeDesc.AccessibleRole = AccessibleRole.StaticText;
			labelLongitudeOfTheDescendingNodeDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelLongitudeOfTheDescendingNodeDesc.Dock = DockStyle.Fill;
			labelLongitudeOfTheDescendingNodeDesc.LabelStyle = LabelStyle.BoldPanel;
			labelLongitudeOfTheDescendingNodeDesc.Location = new Point(4, 211);
			labelLongitudeOfTheDescendingNodeDesc.Margin = new Padding(4, 3, 4, 3);
			labelLongitudeOfTheDescendingNodeDesc.Name = "labelLongitudeOfTheDescendingNodeDesc";
			labelLongitudeOfTheDescendingNodeDesc.Size = new Size(268, 20);
			labelLongitudeOfTheDescendingNodeDesc.TabIndex = 16;
			labelLongitudeOfTheDescendingNodeDesc.Tag = "28";
			labelLongitudeOfTheDescendingNodeDesc.ToolTipValues.Description = "Shows the longitude of the descending node (degrees).\r\nDouble-click or right-click to open the terminology.";
			labelLongitudeOfTheDescendingNodeDesc.ToolTipValues.EnableToolTips = true;
			labelLongitudeOfTheDescendingNodeDesc.ToolTipValues.Heading = "Longitude of the descending node (degrees)";
			labelLongitudeOfTheDescendingNodeDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLongitudeOfTheDescendingNodeDesc.Values.ExtraText = "°";
			labelLongitudeOfTheDescendingNodeDesc.Values.Text = "Longitude of the descending node";
			labelLongitudeOfTheDescendingNodeDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLongitudeOfTheDescendingNodeDesc.Enter += Control_Enter;
			labelLongitudeOfTheDescendingNodeDesc.Leave += Control_Leave;
			labelLongitudeOfTheDescendingNodeDesc.MouseDown += Control_MouseDown;
			labelLongitudeOfTheDescendingNodeDesc.MouseEnter += Control_Enter;
			labelLongitudeOfTheDescendingNodeDesc.MouseLeave += Control_Leave;
			// 
			// labelArgumentOfTheAphelionDesc
			// 
			labelArgumentOfTheAphelionDesc.AccessibleDescription = "Shows the argument of the aphelion (degrees)";
			labelArgumentOfTheAphelionDesc.AccessibleName = "Argument of the aphelion (degrees)";
			labelArgumentOfTheAphelionDesc.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentOfTheAphelionDesc.ContextMenuStrip = contextMenuOpenTerminology;
			labelArgumentOfTheAphelionDesc.Dock = DockStyle.Fill;
			labelArgumentOfTheAphelionDesc.LabelStyle = LabelStyle.BoldPanel;
			labelArgumentOfTheAphelionDesc.Location = new Point(4, 237);
			labelArgumentOfTheAphelionDesc.Margin = new Padding(4, 3, 4, 3);
			labelArgumentOfTheAphelionDesc.Name = "labelArgumentOfTheAphelionDesc";
			labelArgumentOfTheAphelionDesc.Size = new Size(268, 20);
			labelArgumentOfTheAphelionDesc.TabIndex = 18;
			labelArgumentOfTheAphelionDesc.Tag = "29";
			labelArgumentOfTheAphelionDesc.ToolTipValues.Description = "Shows the argument of the aphelion (degrees).\r\nDouble-click or right-click to open the terminology.";
			labelArgumentOfTheAphelionDesc.ToolTipValues.EnableToolTips = true;
			labelArgumentOfTheAphelionDesc.ToolTipValues.Heading = "Argument of the aphelion (degrees)";
			labelArgumentOfTheAphelionDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelArgumentOfTheAphelionDesc.Values.ExtraText = "°";
			labelArgumentOfTheAphelionDesc.Values.Text = "Argument of the aphelion";
			labelArgumentOfTheAphelionDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelArgumentOfTheAphelionDesc.Enter += Control_Enter;
			labelArgumentOfTheAphelionDesc.Leave += Control_Leave;
			labelArgumentOfTheAphelionDesc.MouseDown += Control_MouseDown;
			labelArgumentOfTheAphelionDesc.MouseEnter += Control_Enter;
			labelArgumentOfTheAphelionDesc.MouseLeave += Control_Leave;
			// 
			// labelFocalParameterDesc
			// 
			labelFocalParameterDesc.AccessibleDescription = "Shows the focal parameter (AU)";
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
			labelFocalParameterDesc.ToolTipValues.Description = "Shows the focal parameter (AU).\r\nDouble-click or right-click to open the terminology.";
			labelFocalParameterDesc.ToolTipValues.EnableToolTips = true;
			labelFocalParameterDesc.ToolTipValues.Heading = "Focal parameter (AU)";
			labelFocalParameterDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelFocalParameterDesc.Values.ExtraText = "AU";
			labelFocalParameterDesc.Values.Text = "Focal parameter";
			labelFocalParameterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelFocalParameterDesc.Enter += Control_Enter;
			labelFocalParameterDesc.Leave += Control_Leave;
			labelFocalParameterDesc.MouseDown += Control_MouseDown;
			labelFocalParameterDesc.MouseEnter += Control_Enter;
			labelFocalParameterDesc.MouseLeave += Control_Leave;
			// 
			// labelSemiLatusRectumDesc
			// 
			labelSemiLatusRectumDesc.AccessibleDescription = "Shows the semi-latus rectum (AU)";
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
			labelSemiLatusRectumDesc.ToolTipValues.Description = "Shows the semi-latus rectum (AU).\r\nDouble-click or right-click to open the terminology.";
			labelSemiLatusRectumDesc.ToolTipValues.EnableToolTips = true;
			labelSemiLatusRectumDesc.ToolTipValues.Heading = "Semi-latus rectum (AU)";
			labelSemiLatusRectumDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiLatusRectumDesc.Values.ExtraText = "AU";
			labelSemiLatusRectumDesc.Values.Text = "Semi-latus rectum";
			labelSemiLatusRectumDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiLatusRectumDesc.Enter += Control_Enter;
			labelSemiLatusRectumDesc.Leave += Control_Leave;
			labelSemiLatusRectumDesc.MouseDown += Control_MouseDown;
			labelSemiLatusRectumDesc.MouseEnter += Control_Enter;
			labelSemiLatusRectumDesc.MouseLeave += Control_Leave;
			// 
			// labelLatusRectumDesc
			// 
			labelLatusRectumDesc.AccessibleDescription = "Shows the latus rectum (AU)";
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
			labelLatusRectumDesc.ToolTipValues.Description = "Shows the semi-latus rectum (AU).\r\nDouble-click or right-click to open the terminology.";
			labelLatusRectumDesc.ToolTipValues.EnableToolTips = true;
			labelLatusRectumDesc.ToolTipValues.Heading = "Semi-latus rectum (AU)";
			labelLatusRectumDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLatusRectumDesc.Values.ExtraText = "AU";
			labelLatusRectumDesc.Values.Text = "Latus rectum";
			labelLatusRectumDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelLatusRectumDesc.Enter += Control_Enter;
			labelLatusRectumDesc.Leave += Control_Leave;
			labelLatusRectumDesc.MouseDown += Control_MouseDown;
			labelLatusRectumDesc.MouseEnter += Control_Enter;
			labelLatusRectumDesc.MouseLeave += Control_Leave;
			// 
			// labelOrbitalPeriodDesc
			// 
			labelOrbitalPeriodDesc.AccessibleDescription = "Shows the orbital period (years)";
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
			labelOrbitalPeriodDesc.ToolTipValues.Description = "Shows the orbital Period (years).\r\nDouble-click or right-click to open the terminology.";
			labelOrbitalPeriodDesc.ToolTipValues.EnableToolTips = true;
			labelOrbitalPeriodDesc.ToolTipValues.Heading = "Orbital Period (years)";
			labelOrbitalPeriodDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalPeriodDesc.Values.ExtraText = "years";
			labelOrbitalPeriodDesc.Values.Text = "Orbital period";
			labelOrbitalPeriodDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalPeriodDesc.Enter += Control_Enter;
			labelOrbitalPeriodDesc.Leave += Control_Leave;
			labelOrbitalPeriodDesc.MouseDown += Control_MouseDown;
			labelOrbitalPeriodDesc.MouseEnter += Control_Enter;
			labelOrbitalPeriodDesc.MouseLeave += Control_Leave;
			// 
			// labelSemiMinorAxisData
			// 
			labelSemiMinorAxisData.AccessibleDescription = "Shows the information of \"Semi-minor axis\"";
			labelSemiMinorAxisData.AccessibleName = "Information of \"Semi-minor axis\"";
			labelSemiMinorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMinorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiMinorAxisData.Dock = DockStyle.Fill;
			labelSemiMinorAxisData.Location = new Point(280, 29);
			labelSemiMinorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelSemiMinorAxisData.Name = "labelSemiMinorAxisData";
			labelSemiMinorAxisData.Size = new Size(270, 20);
			labelSemiMinorAxisData.TabIndex = 3;
			labelSemiMinorAxisData.ToolTipValues.Description = "Shows the information of \"Semi-minor axis\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSemiMinorAxisData.ToolTipValues.EnableToolTips = true;
			labelSemiMinorAxisData.ToolTipValues.Heading = "Information of \"Semi-minor axis\"";
			labelSemiMinorAxisData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiMinorAxisData.Values.Text = "..................";
			labelSemiMinorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiMinorAxisData.Enter += Control_Enter;
			labelSemiMinorAxisData.Leave += Control_Leave;
			labelSemiMinorAxisData.MouseDown += Control_MouseDown;
			labelSemiMinorAxisData.MouseEnter += Control_Enter;
			labelSemiMinorAxisData.MouseLeave += Control_Leave;
			// 
			// labelMajorAxisData
			// 
			labelMajorAxisData.AccessibleDescription = "Shows the information of \"Major axis\"";
			labelMajorAxisData.AccessibleName = "Information of \"Major axis\"";
			labelMajorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMajorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMajorAxisData.Dock = DockStyle.Fill;
			labelMajorAxisData.Location = new Point(280, 55);
			labelMajorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMajorAxisData.Name = "labelMajorAxisData";
			labelMajorAxisData.Size = new Size(270, 20);
			labelMajorAxisData.TabIndex = 5;
			labelMajorAxisData.ToolTipValues.Description = "Shows the information of \"Major axis\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelMajorAxisData.ToolTipValues.EnableToolTips = true;
			labelMajorAxisData.ToolTipValues.Heading = "Information of \"Major axis\"";
			labelMajorAxisData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMajorAxisData.Values.Text = "..................";
			labelMajorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMajorAxisData.Enter += Control_Enter;
			labelMajorAxisData.Leave += Control_Leave;
			labelMajorAxisData.MouseDown += Control_MouseDown;
			labelMajorAxisData.MouseEnter += Control_Enter;
			labelMajorAxisData.MouseLeave += Control_Leave;
			// 
			// labelMinorAxisData
			// 
			labelMinorAxisData.AccessibleDescription = "Shows the information of \"Minor axis\"";
			labelMinorAxisData.AccessibleName = "Information of \"Minor axis\"";
			labelMinorAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMinorAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMinorAxisData.Dock = DockStyle.Fill;
			labelMinorAxisData.Location = new Point(280, 81);
			labelMinorAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMinorAxisData.Name = "labelMinorAxisData";
			labelMinorAxisData.Size = new Size(270, 20);
			labelMinorAxisData.TabIndex = 7;
			labelMinorAxisData.ToolTipValues.Description = "Shows the information of \"Minor axis\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelMinorAxisData.ToolTipValues.EnableToolTips = true;
			labelMinorAxisData.ToolTipValues.Heading = "Information of \"Minor axis\"";
			labelMinorAxisData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMinorAxisData.Values.Text = "..................";
			labelMinorAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMinorAxisData.Enter += Control_Enter;
			labelMinorAxisData.Leave += Control_Leave;
			labelMinorAxisData.MouseDown += Control_MouseDown;
			labelMinorAxisData.MouseEnter += Control_Enter;
			labelMinorAxisData.MouseLeave += Control_Leave;
			// 
			// labelEccentricAnomalyData
			// 
			labelEccentricAnomalyData.AccessibleDescription = "Shows the information of \"Eccentric anomaly\"";
			labelEccentricAnomalyData.AccessibleName = "Information of \"Eccentric anomaly\"";
			labelEccentricAnomalyData.AccessibleRole = AccessibleRole.StaticText;
			labelEccentricAnomalyData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelEccentricAnomalyData.Dock = DockStyle.Fill;
			labelEccentricAnomalyData.Location = new Point(280, 107);
			labelEccentricAnomalyData.Margin = new Padding(4, 3, 4, 3);
			labelEccentricAnomalyData.Name = "labelEccentricAnomalyData";
			labelEccentricAnomalyData.Size = new Size(270, 20);
			labelEccentricAnomalyData.TabIndex = 9;
			labelEccentricAnomalyData.ToolTipValues.Description = "Shows the information of \"Eccentric anomaly\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelEccentricAnomalyData.ToolTipValues.EnableToolTips = true;
			labelEccentricAnomalyData.ToolTipValues.Heading = "Information of \"Eccentric anomaly\"";
			labelEccentricAnomalyData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelEccentricAnomalyData.Values.Text = "..................";
			labelEccentricAnomalyData.DoubleClick += CopyToClipboard_DoubleClick;
			labelEccentricAnomalyData.Enter += Control_Enter;
			labelEccentricAnomalyData.Leave += Control_Leave;
			labelEccentricAnomalyData.MouseDown += Control_MouseDown;
			labelEccentricAnomalyData.MouseEnter += Control_Enter;
			labelEccentricAnomalyData.MouseLeave += Control_Leave;
			// 
			// labelTrueAnomalyData
			// 
			labelTrueAnomalyData.AccessibleDescription = "Shows the information of \"True anomaly\"";
			labelTrueAnomalyData.AccessibleName = "Information of \"True anomaly\"";
			labelTrueAnomalyData.AccessibleRole = AccessibleRole.StaticText;
			labelTrueAnomalyData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelTrueAnomalyData.Dock = DockStyle.Fill;
			labelTrueAnomalyData.Location = new Point(280, 133);
			labelTrueAnomalyData.Margin = new Padding(4, 3, 4, 3);
			labelTrueAnomalyData.Name = "labelTrueAnomalyData";
			labelTrueAnomalyData.Size = new Size(270, 20);
			labelTrueAnomalyData.TabIndex = 11;
			labelTrueAnomalyData.ToolTipValues.Description = "Shows the information of \"True anomaly\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelTrueAnomalyData.ToolTipValues.EnableToolTips = true;
			labelTrueAnomalyData.ToolTipValues.Heading = "Information of \"True anomaly\"";
			labelTrueAnomalyData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelTrueAnomalyData.Values.Text = "..................";
			labelTrueAnomalyData.DoubleClick += CopyToClipboard_DoubleClick;
			labelTrueAnomalyData.Enter += Control_Enter;
			labelTrueAnomalyData.Leave += Control_Leave;
			labelTrueAnomalyData.MouseDown += Control_MouseDown;
			labelTrueAnomalyData.MouseEnter += Control_Enter;
			labelTrueAnomalyData.MouseLeave += Control_Leave;
			// 
			// labelPerihelionDistanceData
			// 
			labelPerihelionDistanceData.AccessibleDescription = "Shows the information of \"Perihelion distance\"";
			labelPerihelionDistanceData.AccessibleName = "Information of \"Perihelion distance\"";
			labelPerihelionDistanceData.AccessibleRole = AccessibleRole.StaticText;
			labelPerihelionDistanceData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelPerihelionDistanceData.Dock = DockStyle.Fill;
			labelPerihelionDistanceData.Location = new Point(280, 159);
			labelPerihelionDistanceData.Margin = new Padding(4, 3, 4, 3);
			labelPerihelionDistanceData.Name = "labelPerihelionDistanceData";
			labelPerihelionDistanceData.Size = new Size(270, 20);
			labelPerihelionDistanceData.TabIndex = 13;
			labelPerihelionDistanceData.ToolTipValues.Description = "Shows the information of \"Perihelion distance\".";
			labelPerihelionDistanceData.ToolTipValues.EnableToolTips = true;
			labelPerihelionDistanceData.ToolTipValues.Heading = "Information of \"Perihelion distance\"";
			labelPerihelionDistanceData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelPerihelionDistanceData.Values.Text = "..................";
			labelPerihelionDistanceData.DoubleClick += CopyToClipboard_DoubleClick;
			labelPerihelionDistanceData.Enter += Control_Enter;
			labelPerihelionDistanceData.Leave += Control_Leave;
			labelPerihelionDistanceData.MouseDown += Control_MouseDown;
			labelPerihelionDistanceData.MouseEnter += Control_Enter;
			labelPerihelionDistanceData.MouseLeave += Control_Leave;
			// 
			// labelAphelionDistanceData
			// 
			labelAphelionDistanceData.AccessibleDescription = "Shows the information of \"Aphelion distance\"";
			labelAphelionDistanceData.AccessibleName = "Information of \"Aphelion distance\"";
			labelAphelionDistanceData.AccessibleRole = AccessibleRole.StaticText;
			labelAphelionDistanceData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelAphelionDistanceData.Dock = DockStyle.Fill;
			labelAphelionDistanceData.Location = new Point(280, 185);
			labelAphelionDistanceData.Margin = new Padding(4, 3, 4, 3);
			labelAphelionDistanceData.Name = "labelAphelionDistanceData";
			labelAphelionDistanceData.Size = new Size(270, 20);
			labelAphelionDistanceData.TabIndex = 15;
			labelAphelionDistanceData.ToolTipValues.Description = "Shows the information of \"Aphelion distance\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelAphelionDistanceData.ToolTipValues.EnableToolTips = true;
			labelAphelionDistanceData.ToolTipValues.Heading = "Information of \"Aphelion distance\"";
			labelAphelionDistanceData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAphelionDistanceData.Values.Text = "..................";
			labelAphelionDistanceData.DoubleClick += CopyToClipboard_DoubleClick;
			labelAphelionDistanceData.Enter += Control_Enter;
			labelAphelionDistanceData.Leave += Control_Leave;
			labelAphelionDistanceData.MouseDown += Control_MouseDown;
			labelAphelionDistanceData.MouseEnter += Control_Enter;
			labelAphelionDistanceData.MouseLeave += Control_Leave;
			// 
			// labelLongitudeDescendingNodeData
			// 
			labelLongitudeDescendingNodeData.AccessibleDescription = "Shows the information of \"Longitude of the descending node\"";
			labelLongitudeDescendingNodeData.AccessibleName = "Information of \"Longitude of the descending node\"";
			labelLongitudeDescendingNodeData.AccessibleRole = AccessibleRole.StaticText;
			labelLongitudeDescendingNodeData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLongitudeDescendingNodeData.Dock = DockStyle.Fill;
			labelLongitudeDescendingNodeData.Location = new Point(280, 211);
			labelLongitudeDescendingNodeData.Margin = new Padding(4, 3, 4, 3);
			labelLongitudeDescendingNodeData.Name = "labelLongitudeDescendingNodeData";
			labelLongitudeDescendingNodeData.Size = new Size(270, 20);
			labelLongitudeDescendingNodeData.TabIndex = 17;
			labelLongitudeDescendingNodeData.ToolTipValues.Description = "Shows the information of \"Longitude of the descending node\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelLongitudeDescendingNodeData.ToolTipValues.EnableToolTips = true;
			labelLongitudeDescendingNodeData.ToolTipValues.Heading = "Information of \"Longitude of the descending node\"";
			labelLongitudeDescendingNodeData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLongitudeDescendingNodeData.Values.Text = "..................";
			labelLongitudeDescendingNodeData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLongitudeDescendingNodeData.Enter += Control_Enter;
			labelLongitudeDescendingNodeData.Leave += Control_Leave;
			labelLongitudeDescendingNodeData.MouseDown += Control_MouseDown;
			labelLongitudeDescendingNodeData.MouseEnter += Control_Enter;
			labelLongitudeDescendingNodeData.MouseLeave += Control_Leave;
			// 
			// labelArgumentAphelionData
			// 
			labelArgumentAphelionData.AccessibleDescription = "Shows the information of \"Argument of the aphelion\"";
			labelArgumentAphelionData.AccessibleName = "Information of \"Argument of the aphelion\"";
			labelArgumentAphelionData.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentAphelionData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelArgumentAphelionData.Dock = DockStyle.Fill;
			labelArgumentAphelionData.Location = new Point(280, 237);
			labelArgumentAphelionData.Margin = new Padding(4, 3, 4, 3);
			labelArgumentAphelionData.Name = "labelArgumentAphelionData";
			labelArgumentAphelionData.Size = new Size(270, 20);
			labelArgumentAphelionData.TabIndex = 19;
			labelArgumentAphelionData.ToolTipValues.Description = "Shows the information of \"Argument of the aphelion\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelArgumentAphelionData.ToolTipValues.EnableToolTips = true;
			labelArgumentAphelionData.ToolTipValues.Heading = "Information of \"Argument of the aphelion\"";
			labelArgumentAphelionData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelArgumentAphelionData.Values.Text = "..................";
			labelArgumentAphelionData.DoubleClick += CopyToClipboard_DoubleClick;
			labelArgumentAphelionData.Enter += Control_Enter;
			labelArgumentAphelionData.Leave += Control_Leave;
			labelArgumentAphelionData.MouseDown += Control_MouseDown;
			labelArgumentAphelionData.MouseEnter += Control_Enter;
			labelArgumentAphelionData.MouseLeave += Control_Leave;
			// 
			// labelFocalParameterData
			// 
			labelFocalParameterData.AccessibleDescription = "Shows the information of \"Focal parameter\"";
			labelFocalParameterData.AccessibleName = "Information of \"Focal parameter\"";
			labelFocalParameterData.AccessibleRole = AccessibleRole.StaticText;
			labelFocalParameterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelFocalParameterData.Dock = DockStyle.Fill;
			labelFocalParameterData.Location = new Point(280, 263);
			labelFocalParameterData.Margin = new Padding(4, 3, 4, 3);
			labelFocalParameterData.Name = "labelFocalParameterData";
			labelFocalParameterData.Size = new Size(270, 20);
			labelFocalParameterData.TabIndex = 21;
			labelFocalParameterData.ToolTipValues.Description = "Shows the information of \"Semi-latus rectum\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelFocalParameterData.ToolTipValues.EnableToolTips = true;
			labelFocalParameterData.ToolTipValues.Heading = "Information of \"Semi-latus rectum\"";
			labelFocalParameterData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelFocalParameterData.Values.Text = "..................";
			labelFocalParameterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelFocalParameterData.Enter += Control_Enter;
			labelFocalParameterData.Leave += Control_Leave;
			labelFocalParameterData.MouseDown += Control_MouseDown;
			labelFocalParameterData.MouseEnter += Control_Enter;
			labelFocalParameterData.MouseLeave += Control_Leave;
			// 
			// labelSemiLatusRectumData
			// 
			labelSemiLatusRectumData.AccessibleDescription = "Shows the information of \"Semi-latus rectum\"";
			labelSemiLatusRectumData.AccessibleName = "Information of \"Semi-latus rectum\"";
			labelSemiLatusRectumData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiLatusRectumData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiLatusRectumData.Dock = DockStyle.Fill;
			labelSemiLatusRectumData.Location = new Point(280, 289);
			labelSemiLatusRectumData.Margin = new Padding(4, 3, 4, 3);
			labelSemiLatusRectumData.Name = "labelSemiLatusRectumData";
			labelSemiLatusRectumData.Size = new Size(270, 20);
			labelSemiLatusRectumData.TabIndex = 23;
			labelSemiLatusRectumData.ToolTipValues.Description = "Shows the information of \"Semi-latus rectum\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSemiLatusRectumData.ToolTipValues.EnableToolTips = true;
			labelSemiLatusRectumData.ToolTipValues.Heading = "Information of \"Semi-latus rectum\"";
			labelSemiLatusRectumData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiLatusRectumData.Values.Text = "..................";
			labelSemiLatusRectumData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiLatusRectumData.Enter += Control_Enter;
			labelSemiLatusRectumData.Leave += Control_Leave;
			labelSemiLatusRectumData.MouseDown += Control_MouseDown;
			labelSemiLatusRectumData.MouseEnter += Control_Enter;
			labelSemiLatusRectumData.MouseLeave += Control_Leave;
			// 
			// labelLatusRectumData
			// 
			labelLatusRectumData.AccessibleDescription = "Shows the information of \"Latus rectum\"";
			labelLatusRectumData.AccessibleName = "Information of \"Latus rectum\"";
			labelLatusRectumData.AccessibleRole = AccessibleRole.StaticText;
			labelLatusRectumData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelLatusRectumData.Dock = DockStyle.Fill;
			labelLatusRectumData.Location = new Point(280, 315);
			labelLatusRectumData.Margin = new Padding(4, 3, 4, 3);
			labelLatusRectumData.Name = "labelLatusRectumData";
			labelLatusRectumData.Size = new Size(270, 20);
			labelLatusRectumData.TabIndex = 25;
			labelLatusRectumData.ToolTipValues.Description = "Shows the information of \"Latus rectum\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelLatusRectumData.ToolTipValues.EnableToolTips = true;
			labelLatusRectumData.ToolTipValues.Heading = "Information of \"Latus rectum\"";
			labelLatusRectumData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLatusRectumData.Values.Text = "..................";
			labelLatusRectumData.DoubleClick += CopyToClipboard_DoubleClick;
			labelLatusRectumData.Enter += Control_Enter;
			labelLatusRectumData.Leave += Control_Leave;
			labelLatusRectumData.MouseDown += Control_MouseDown;
			labelLatusRectumData.MouseEnter += Control_Enter;
			labelLatusRectumData.MouseLeave += Control_Leave;
			// 
			// labelOrbitalPeriodData
			// 
			labelOrbitalPeriodData.AccessibleDescription = "Shows the information of \"Orbital period\"";
			labelOrbitalPeriodData.AccessibleName = "Information of \"Orbital period\"";
			labelOrbitalPeriodData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPeriodData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalPeriodData.Dock = DockStyle.Fill;
			labelOrbitalPeriodData.Location = new Point(280, 341);
			labelOrbitalPeriodData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPeriodData.Name = "labelOrbitalPeriodData";
			labelOrbitalPeriodData.Size = new Size(270, 20);
			labelOrbitalPeriodData.TabIndex = 27;
			labelOrbitalPeriodData.ToolTipValues.Description = "Shows the information of \"Orbital period\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelOrbitalPeriodData.ToolTipValues.EnableToolTips = true;
			labelOrbitalPeriodData.ToolTipValues.Heading = "Information of \"Orbital period\"";
			labelOrbitalPeriodData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalPeriodData.Values.Text = "..................";
			labelOrbitalPeriodData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalPeriodData.Enter += Control_Enter;
			labelOrbitalPeriodData.Leave += Control_Leave;
			labelOrbitalPeriodData.MouseDown += Control_MouseDown;
			labelOrbitalPeriodData.MouseEnter += Control_Enter;
			labelOrbitalPeriodData.MouseLeave += Control_Leave;
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
			labelOrbitalAreaDesc.ToolTipValues.Description = "Shows the orbital area (AU²).\r\nDouble-click or right-click to open the terminology.";
			labelOrbitalAreaDesc.ToolTipValues.EnableToolTips = true;
			labelOrbitalAreaDesc.ToolTipValues.Heading = "Orbital area (AU²)";
			labelOrbitalAreaDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalAreaDesc.Values.ExtraText = "AU²";
			labelOrbitalAreaDesc.Values.Text = "Orbital area";
			labelOrbitalAreaDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalAreaDesc.Enter += Control_Enter;
			labelOrbitalAreaDesc.Leave += Control_Leave;
			labelOrbitalAreaDesc.MouseDown += Control_MouseDown;
			labelOrbitalAreaDesc.MouseEnter += Control_Enter;
			labelOrbitalAreaDesc.MouseLeave += Control_Leave;
			// 
			// labelOrbitalPerimeterDesc
			// 
			labelOrbitalPerimeterDesc.AccessibleDescription = "Shows the orbital perimeter (AU)";
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
			labelOrbitalPerimeterDesc.ToolTipValues.Description = "Shows the orbital perimeter (AU).\r\nDouble-click or right-click to open the terminology.";
			labelOrbitalPerimeterDesc.ToolTipValues.EnableToolTips = true;
			labelOrbitalPerimeterDesc.ToolTipValues.Heading = "Orbital perimeter (AU)";
			labelOrbitalPerimeterDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalPerimeterDesc.Values.ExtraText = "AU";
			labelOrbitalPerimeterDesc.Values.Text = "Orbital perimeter";
			labelOrbitalPerimeterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelOrbitalPerimeterDesc.Enter += Control_Enter;
			labelOrbitalPerimeterDesc.Leave += Control_Leave;
			labelOrbitalPerimeterDesc.MouseDown += Control_MouseDown;
			labelOrbitalPerimeterDesc.MouseEnter += Control_Enter;
			labelOrbitalPerimeterDesc.MouseLeave += Control_Leave;
			// 
			// labelSemiMeanAxisDesc
			// 
			labelSemiMeanAxisDesc.AccessibleDescription = "Shows the semi-mean axis (AU)";
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
			labelSemiMeanAxisDesc.ToolTipValues.Description = "Shows the semi-mean axis (AU).\r\nDouble-click or right-click to open the terminology.";
			labelSemiMeanAxisDesc.ToolTipValues.EnableToolTips = true;
			labelSemiMeanAxisDesc.ToolTipValues.Heading = "Semi-mean axis (AU)";
			labelSemiMeanAxisDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiMeanAxisDesc.Values.ExtraText = "AU";
			labelSemiMeanAxisDesc.Values.Text = "Semi-mean axis";
			labelSemiMeanAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelSemiMeanAxisDesc.Enter += Control_Enter;
			labelSemiMeanAxisDesc.Leave += Control_Leave;
			labelSemiMeanAxisDesc.MouseDown += Control_MouseDown;
			labelSemiMeanAxisDesc.MouseEnter += Control_Enter;
			labelSemiMeanAxisDesc.MouseLeave += Control_Leave;
			// 
			// labelMeanAxisDesc
			// 
			labelMeanAxisDesc.AccessibleDescription = "Shows the mean axis (AU)";
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
			labelMeanAxisDesc.ToolTipValues.Description = "Shows the mean axis (AU).\r\nDouble-click or right-click to open the terminology.";
			labelMeanAxisDesc.ToolTipValues.EnableToolTips = true;
			labelMeanAxisDesc.ToolTipValues.Heading = "Mean axis (AU)";
			labelMeanAxisDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMeanAxisDesc.Values.ExtraText = "AU";
			labelMeanAxisDesc.Values.Text = "Mean axis";
			labelMeanAxisDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelMeanAxisDesc.Enter += Control_Enter;
			labelMeanAxisDesc.Leave += Control_Leave;
			labelMeanAxisDesc.MouseDown += Control_MouseDown;
			labelMeanAxisDesc.MouseEnter += Control_Enter;
			labelMeanAxisDesc.MouseLeave += Control_Leave;
			// 
			// labelOrbitalAreaData
			// 
			labelOrbitalAreaData.AccessibleDescription = "Shows the information of \"Orbital area\"";
			labelOrbitalAreaData.AccessibleName = "Information of \"Orbital area\"";
			labelOrbitalAreaData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalAreaData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalAreaData.Dock = DockStyle.Fill;
			labelOrbitalAreaData.Location = new Point(280, 367);
			labelOrbitalAreaData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalAreaData.Name = "labelOrbitalAreaData";
			labelOrbitalAreaData.Size = new Size(270, 20);
			labelOrbitalAreaData.TabIndex = 29;
			labelOrbitalAreaData.ToolTipValues.Description = "Shows the information of \"Orbital area\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelOrbitalAreaData.ToolTipValues.EnableToolTips = true;
			labelOrbitalAreaData.ToolTipValues.Heading = "Information of \"Orbital area\"";
			labelOrbitalAreaData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalAreaData.Values.Text = "..................";
			labelOrbitalAreaData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalAreaData.Enter += Control_Enter;
			labelOrbitalAreaData.Leave += Control_Leave;
			labelOrbitalAreaData.MouseDown += Control_MouseDown;
			labelOrbitalAreaData.MouseEnter += Control_Enter;
			labelOrbitalAreaData.MouseLeave += Control_Leave;
			// 
			// labelOrbitalPerimeterData
			// 
			labelOrbitalPerimeterData.AccessibleDescription = "Shows the information of \"Orbital perimeter\"";
			labelOrbitalPerimeterData.AccessibleName = "Information of \"Orbital perimeter\"";
			labelOrbitalPerimeterData.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalPerimeterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelOrbitalPerimeterData.Dock = DockStyle.Fill;
			labelOrbitalPerimeterData.Location = new Point(280, 393);
			labelOrbitalPerimeterData.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalPerimeterData.Name = "labelOrbitalPerimeterData";
			labelOrbitalPerimeterData.Size = new Size(270, 20);
			labelOrbitalPerimeterData.TabIndex = 31;
			labelOrbitalPerimeterData.ToolTipValues.Description = "Shows the information of \"Orbital perimeter\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelOrbitalPerimeterData.ToolTipValues.EnableToolTips = true;
			labelOrbitalPerimeterData.ToolTipValues.Heading = "Information of \"Orbital perimeter\"";
			labelOrbitalPerimeterData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalPerimeterData.Values.Text = "..................";
			labelOrbitalPerimeterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelOrbitalPerimeterData.Enter += Control_Enter;
			labelOrbitalPerimeterData.Leave += Control_Leave;
			labelOrbitalPerimeterData.MouseDown += Control_MouseDown;
			labelOrbitalPerimeterData.MouseEnter += Control_Enter;
			labelOrbitalPerimeterData.MouseLeave += Control_Leave;
			// 
			// labelSemiMeanAxisData
			// 
			labelSemiMeanAxisData.AccessibleDescription = "Shows the information of \"Semi-mean axis\"";
			labelSemiMeanAxisData.AccessibleName = "Information of \"Semi-mean axis\"";
			labelSemiMeanAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMeanAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelSemiMeanAxisData.Dock = DockStyle.Fill;
			labelSemiMeanAxisData.Location = new Point(280, 419);
			labelSemiMeanAxisData.Margin = new Padding(4, 3, 4, 3);
			labelSemiMeanAxisData.Name = "labelSemiMeanAxisData";
			labelSemiMeanAxisData.Size = new Size(270, 20);
			labelSemiMeanAxisData.TabIndex = 33;
			labelSemiMeanAxisData.ToolTipValues.Description = "Shows the information of \"Semi-mean axis\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelSemiMeanAxisData.ToolTipValues.EnableToolTips = true;
			labelSemiMeanAxisData.ToolTipValues.Heading = "Information of \"Semi-mean axis\"";
			labelSemiMeanAxisData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiMeanAxisData.Values.Text = "..................";
			labelSemiMeanAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelSemiMeanAxisData.Enter += Control_Enter;
			labelSemiMeanAxisData.Leave += Control_Leave;
			labelSemiMeanAxisData.MouseDown += Control_MouseDown;
			labelSemiMeanAxisData.MouseEnter += Control_Enter;
			labelSemiMeanAxisData.MouseLeave += Control_Leave;
			// 
			// labelStandardGravitationalParameterDesc
			// 
			labelStandardGravitationalParameterDesc.AccessibleDescription = "Shows the standard gravitational parameter (AU³/a²)";
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
			labelStandardGravitationalParameterDesc.ToolTipValues.Description = "Shows the standard gravitational parameter (AU³/a²).\r\nDouble-click or right-click to open the terminology.";
			labelStandardGravitationalParameterDesc.ToolTipValues.EnableToolTips = true;
			labelStandardGravitationalParameterDesc.ToolTipValues.Heading = "Standard gravitational parameter (AU³/a²)";
			labelStandardGravitationalParameterDesc.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelStandardGravitationalParameterDesc.Values.ExtraText = "AU³/a²";
			labelStandardGravitationalParameterDesc.Values.Text = "Standard gravitational parameter";
			labelStandardGravitationalParameterDesc.DoubleClick += OpenTerminology_DoubleClick;
			labelStandardGravitationalParameterDesc.Enter += Control_Enter;
			labelStandardGravitationalParameterDesc.Leave += Control_Leave;
			labelStandardGravitationalParameterDesc.MouseDown += Control_MouseDown;
			labelStandardGravitationalParameterDesc.MouseEnter += Control_Enter;
			labelStandardGravitationalParameterDesc.MouseLeave += Control_Leave;
			// 
			// labelMeanAxisData
			// 
			labelMeanAxisData.AccessibleDescription = "Shows the information of \"Mean axis\"";
			labelMeanAxisData.AccessibleName = "Information of \"Mean axis\"";
			labelMeanAxisData.AccessibleRole = AccessibleRole.StaticText;
			labelMeanAxisData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelMeanAxisData.Dock = DockStyle.Fill;
			labelMeanAxisData.Location = new Point(280, 445);
			labelMeanAxisData.Margin = new Padding(4, 3, 4, 3);
			labelMeanAxisData.Name = "labelMeanAxisData";
			labelMeanAxisData.Size = new Size(270, 20);
			labelMeanAxisData.TabIndex = 35;
			labelMeanAxisData.ToolTipValues.Description = "Shows the information of \"Mean axis\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelMeanAxisData.ToolTipValues.EnableToolTips = true;
			labelMeanAxisData.ToolTipValues.Heading = "Information of \"Mean axis\"";
			labelMeanAxisData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMeanAxisData.Values.Text = "..................";
			labelMeanAxisData.DoubleClick += CopyToClipboard_DoubleClick;
			labelMeanAxisData.Enter += Control_Enter;
			labelMeanAxisData.Leave += Control_Leave;
			labelMeanAxisData.MouseDown += Control_MouseDown;
			labelMeanAxisData.MouseEnter += Control_Enter;
			labelMeanAxisData.MouseLeave += Control_Leave;
			// 
			// labelStandardGravitationalParameterData
			// 
			labelStandardGravitationalParameterData.AccessibleDescription = "Shows the information of \"Standard gravitational parameter\"";
			labelStandardGravitationalParameterData.AccessibleName = "Information of \"Standard gravitational parameter\"";
			labelStandardGravitationalParameterData.AccessibleRole = AccessibleRole.StaticText;
			labelStandardGravitationalParameterData.ContextMenuStrip = contextMenuCopyToClipboard;
			labelStandardGravitationalParameterData.Dock = DockStyle.Fill;
			labelStandardGravitationalParameterData.Location = new Point(280, 471);
			labelStandardGravitationalParameterData.Margin = new Padding(4, 3, 4, 3);
			labelStandardGravitationalParameterData.Name = "labelStandardGravitationalParameterData";
			labelStandardGravitationalParameterData.Size = new Size(270, 21);
			labelStandardGravitationalParameterData.TabIndex = 37;
			labelStandardGravitationalParameterData.ToolTipValues.Description = "Shows the information of \"Standard gravitational parameter\".\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelStandardGravitationalParameterData.ToolTipValues.EnableToolTips = true;
			labelStandardGravitationalParameterData.ToolTipValues.Heading = "Information of \"Standard gravitational parameter\"";
			labelStandardGravitationalParameterData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelStandardGravitationalParameterData.Values.Text = "..................";
			labelStandardGravitationalParameterData.DoubleClick += CopyToClipboard_DoubleClick;
			labelStandardGravitationalParameterData.Enter += Control_Enter;
			labelStandardGravitationalParameterData.Leave += Control_Leave;
			labelStandardGravitationalParameterData.MouseDown += Control_MouseDown;
			labelStandardGravitationalParameterData.MouseEnter += Control_Enter;
			labelStandardGravitationalParameterData.MouseLeave += Control_Leave;
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
			tableLayoutPanel.Controls.Add(labelLongitudeOfTheDescendingNodeDesc, 0, 8);
			tableLayoutPanel.Controls.Add(labelArgumentOfTheAphelionDesc, 0, 9);
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
			toolStripIcons.Enter += Control_Enter;
			toolStripIcons.Leave += Control_Leave;
			toolStripIcons.MouseEnter += Control_Enter;
			toolStripIcons.MouseLeave += Control_Leave;
			// 
			// splitbuttonCopyToClipboard
			// 
			splitbuttonCopyToClipboard.AccessibleDescription = "Copys to clipboard";
			splitbuttonCopyToClipboard.AccessibleName = "Copy to clipboard";
			splitbuttonCopyToClipboard.AccessibleRole = AccessibleRole.SplitButton;
			splitbuttonCopyToClipboard.BackColor = Color.Transparent;
			splitbuttonCopyToClipboard.DropDown = contextMenuFullCopyToClipboardDerivedOrbitalElements;
			splitbuttonCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
			splitbuttonCopyToClipboard.ImageTransparentColor = Color.Magenta;
			splitbuttonCopyToClipboard.Name = "splitbuttonCopyToClipboard";
			splitbuttonCopyToClipboard.Size = new Size(134, 22);
			splitbuttonCopyToClipboard.Text = "&Copy to clipboard";
			splitbuttonCopyToClipboard.ButtonClick += ToolStripButtonCopyToClipboard_Click;
			splitbuttonCopyToClipboard.MouseEnter += Control_Enter;
			splitbuttonCopyToClipboard.MouseLeave += Control_Leave;
			// 
			// contextMenuFullCopyToClipboardDerivedOrbitalElements
			// 
			contextMenuFullCopyToClipboardDerivedOrbitalElements.AccessibleDescription = "Shows the context menu of the derived orbital elements to copy to clipboard";
			contextMenuFullCopyToClipboardDerivedOrbitalElements.AccessibleName = "context menu of the derived orbital elements to copy to clipboard";
			contextMenuFullCopyToClipboardDerivedOrbitalElements.AccessibleRole = AccessibleRole.MenuPopup;
			contextMenuFullCopyToClipboardDerivedOrbitalElements.Font = new Font("Segoe UI", 9F);
			contextMenuFullCopyToClipboardDerivedOrbitalElements.Items.AddRange(new ToolStripItem[] { menuitemCopyToClipboardLinearEccentricity, menuitemCopyToClipboardSemiMinorAxis, menuitemCopyToClipboardMajorAxis, menuitemCopyToClipboardMinorAxis, menuitemCopyToClipboardEccentricAnomaly, menuitemCopyToClipboardTrueAnomaly, menuitemCopyToClipboardPerihelionDistance, menuitemCopyToClipboardAphelionDistance, menuitemCopyToClipboardLongitudeDescendingNode, menuitemCopyToClipboardArgumentAphelion, menuitemCopyToClipboardFocalParameter, menuitemCopyToClipboardSemiLatusRectum, menuitemCopyToClipboardLatusRectum, menuitemCopyToClipboardOrbitalPeriod, menuitemCopyToClipboardOrbitalArea, menuitemCopyToClipboardSemiMeanAxis, menuitemCopyToClipboardMeanAxis, menuitemCopyToClipboardStandardGravitationalParameter });
			contextMenuFullCopyToClipboardDerivedOrbitalElements.Name = "Context menu of copying to clipboard of derived orbital elements";
			contextMenuFullCopyToClipboardDerivedOrbitalElements.OwnerItem = splitbuttonCopyToClipboard;
			contextMenuFullCopyToClipboardDerivedOrbitalElements.Size = new Size(257, 400);
			contextMenuFullCopyToClipboardDerivedOrbitalElements.Text = "Copy to clipboard";
			contextMenuFullCopyToClipboardDerivedOrbitalElements.MouseEnter += Control_Enter;
			contextMenuFullCopyToClipboardDerivedOrbitalElements.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardLinearEccentricity.MouseEnter += Control_Enter;
			menuitemCopyToClipboardLinearEccentricity.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardSemiMinorAxis.MouseEnter += Control_Enter;
			menuitemCopyToClipboardSemiMinorAxis.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardMajorAxis.MouseEnter += Control_Enter;
			menuitemCopyToClipboardMajorAxis.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardMinorAxis.MouseEnter += Control_Enter;
			menuitemCopyToClipboardMinorAxis.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardEccentricAnomaly.MouseEnter += Control_Enter;
			menuitemCopyToClipboardEccentricAnomaly.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardTrueAnomaly.MouseEnter += Control_Enter;
			menuitemCopyToClipboardTrueAnomaly.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardPerihelionDistance.MouseEnter += Control_Enter;
			menuitemCopyToClipboardPerihelionDistance.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardAphelionDistance.MouseEnter += Control_Enter;
			menuitemCopyToClipboardAphelionDistance.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardLongitudeDescendingNode.MouseEnter += Control_Enter;
			menuitemCopyToClipboardLongitudeDescendingNode.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardArgumentAphelion.MouseEnter += Control_Enter;
			menuitemCopyToClipboardArgumentAphelion.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardFocalParameter.MouseEnter += Control_Enter;
			menuitemCopyToClipboardFocalParameter.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardSemiLatusRectum.MouseEnter += Control_Enter;
			menuitemCopyToClipboardSemiLatusRectum.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardLatusRectum.MouseEnter += Control_Enter;
			menuitemCopyToClipboardLatusRectum.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardOrbitalPeriod.MouseEnter += Control_Enter;
			menuitemCopyToClipboardOrbitalPeriod.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardOrbitalArea.MouseEnter += Control_Enter;
			menuitemCopyToClipboardOrbitalArea.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardSemiMeanAxis.MouseEnter += Control_Enter;
			menuitemCopyToClipboardSemiMeanAxis.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardMeanAxis.MouseEnter += Control_Enter;
			menuitemCopyToClipboardMeanAxis.MouseLeave += Control_Leave;
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
			menuitemCopyToClipboardStandardGravitationalParameter.MouseEnter += Control_Enter;
			menuitemCopyToClipboardStandardGravitationalParameter.MouseLeave += Control_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// DerivedOrbitElementsForm
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
			Name = "DerivedOrbitElementsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Derived orbit elements";
			Load += DerivedOrbitElementsForm_Load;
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
			contextMenuFullCopyToClipboardDerivedOrbitalElements.ResumeLayout(false);
			ResumeLayout(false);

		}

		#endregion
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonLabel labelLinearEccentricityData;
		private KryptonLabel labelLinearEccentricityDesc;
		private KryptonLabel labelSemiMinorAxisDesc;
		private KryptonLabel labelMajorAxisDesc;
		private KryptonLabel labelTrueAnomalyDesc;
		private KryptonLabel labelPerihelionDistanceDesc;
		private KryptonLabel LabelAphelionDistanceDesc;
		private KryptonLabel labelLongitudeOfTheDescendingNodeDesc;
		private KryptonLabel labelArgumentOfTheAphelionDesc;
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
		private ToolStripMenuItem ToolStripMenuItemCopyToClipboard;
		private ContextMenuStrip contextMenuOpenTerminology;
		private ToolStripMenuItem toolStripMenuItemOpenTerminology;
		private ToolStrip toolStripIcons;
		private ToolStripSplitButton splitbuttonCopyToClipboard;
		private ContextMenuStrip contextMenuFullCopyToClipboardDerivedOrbitalElements;
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