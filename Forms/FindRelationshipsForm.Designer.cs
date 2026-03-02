using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class FindRelationshipsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FindRelationshipsForm));
			kryptonManager = new KryptonManager(components);
			toolStripContainer = new ToolStripContainer();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripButtonStart = new ToolStripButton();
			toolStripButtonCancel = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			kryptonProgressBar = new KryptonProgressBarToolStripItem();
			splitContainerMain = new SplitContainer();
			panelSettings = new KryptonPanel();
			tableLayoutPanelSettings = new KryptonTableLayoutPanel();
			labelMinRelationships = new KryptonLabel();
			numericUpDownMinRelationships = new KryptonNumericUpDown();
			labelMaxRelationships = new KryptonLabel();
			numericUpDownMaxRelationships = new KryptonNumericUpDown();
			labelTolerance = new KryptonLabel();
			numericUpDownTolerance = new KryptonNumericUpDown();
			labelMinGroupMembers = new KryptonLabel();
			numericUpDownMinGroupMembers = new KryptonNumericUpDown();
			labelElements = new KryptonLabel();
			checkBoxSemiMajorAxis = new KryptonCheckBox();
			checkBoxOrbEcc = new KryptonCheckBox();
			checkBoxIncl = new KryptonCheckBox();
			checkBoxLongAscNode = new KryptonCheckBox();
			checkBoxArgPeri = new KryptonCheckBox();
			checkBoxMeanAnomaly = new KryptonCheckBox();
			checkBoxMagAbs = new KryptonCheckBox();
			checkBoxMotion = new KryptonCheckBox();
			checkBoxSlopeParam = new KryptonCheckBox();
			richTextBoxResults = new RichTextBox();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			statusStrip.SuspendLayout();
			kryptonToolStripIcons.SuspendLayout();
			((ISupportInitialize)splitContainerMain).BeginInit();
			splitContainerMain.Panel1.SuspendLayout();
			splitContainerMain.Panel2.SuspendLayout();
			splitContainerMain.SuspendLayout();
			((ISupportInitialize)panelSettings).BeginInit();
			panelSettings.SuspendLayout();
			tableLayoutPanelSettings.SuspendLayout();
			SuspendLayout();
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
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(splitContainerMain);
			toolStripContainer.ContentPanel.Size = new Size(800, 520);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(800, 570);
			toolStripContainer.TabIndex = 0;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(800, 22);
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
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.AccessibleDescription = "Toolbar for controlling the relationship search";
			kryptonToolStripIcons.AccessibleName = "Search toolbar";
			kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.GripStyle = ToolStripGripStyle.Hidden;
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonStart, toolStripButtonCancel, toolStripSeparator1, kryptonProgressBar });
			kryptonToolStripIcons.Location = new Point(3, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(600, 25);
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.Text = "kryptonToolStrip";
			// 
			// toolStripButtonStart
			// 
			toolStripButtonStart.AccessibleDescription = "Starts the search for orbital element relationships";
			toolStripButtonStart.AccessibleName = "Start search";
			toolStripButtonStart.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonStart.Image = FatcowIcons16px.fatcow_package_go_16px;
			toolStripButtonStart.ImageTransparentColor = Color.Magenta;
			toolStripButtonStart.Name = "toolStripButtonStart";
			toolStripButtonStart.Size = new Size(84, 22);
			toolStripButtonStart.Text = "&Start search";
			toolStripButtonStart.Click += ButtonStart_Click;
			toolStripButtonStart.MouseEnter += Control_Enter;
			toolStripButtonStart.MouseLeave += Control_Leave;
			// 
			// toolStripButtonCancel
			// 
			toolStripButtonCancel.AccessibleDescription = "Cancels the running search";
			toolStripButtonCancel.AccessibleName = "Cancel search";
			toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonCancel.Enabled = false;
			toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
			toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
			toolStripButtonCancel.Name = "toolStripButtonCancel";
			toolStripButtonCancel.Size = new Size(97, 22);
			toolStripButtonCancel.Text = "&Cancel search";
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
			toolStripSeparator1.Size = new Size(6, 25);
			// 
			// kryptonProgressBar
			// 
			kryptonProgressBar.AccessibleDescription = "Shows the progress of the search";
			kryptonProgressBar.AccessibleName = "Search progress bar";
			kryptonProgressBar.Name = "kryptonProgressBar";
			kryptonProgressBar.Size = new Size(290, 22);
			kryptonProgressBar.StateCommon.Back.Color1 = Color.Green;
			kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
			kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
			kryptonProgressBar.Values.Text = "";
			// 
			// splitContainerMain
			// 
			splitContainerMain.AccessibleDescription = "Splits the settings and results areas";
			splitContainerMain.AccessibleName = "Main splitter";
			splitContainerMain.AccessibleRole = AccessibleRole.Grouping;
			splitContainerMain.Dock = DockStyle.Fill;
			splitContainerMain.Location = new Point(0, 0);
			splitContainerMain.Name = "splitContainerMain";
			splitContainerMain.Orientation = Orientation.Horizontal;
			// 
			// splitContainerMain.Panel1
			// 
			splitContainerMain.Panel1.Controls.Add(panelSettings);
			splitContainerMain.Panel1MinSize = 200;
			// 
			// splitContainerMain.Panel2
			// 
			splitContainerMain.Panel2.Controls.Add(richTextBoxResults);
			splitContainerMain.Size = new Size(800, 520);
			splitContainerMain.SplitterDistance = 220;
			splitContainerMain.TabIndex = 0;
			// 
			// panelSettings
			// 
			panelSettings.AccessibleDescription = "Settings panel for the relationship search";
			panelSettings.AccessibleName = "Settings panel";
			panelSettings.AccessibleRole = AccessibleRole.Pane;
			panelSettings.Controls.Add(tableLayoutPanelSettings);
			panelSettings.Dock = DockStyle.Fill;
			panelSettings.Location = new Point(0, 0);
			panelSettings.Name = "panelSettings";
			panelSettings.Size = new Size(800, 220);
			panelSettings.TabIndex = 0;
			// 
			// tableLayoutPanelSettings
			// 
			tableLayoutPanelSettings.AccessibleDescription = "Table layout for the settings";
			tableLayoutPanelSettings.AccessibleName = "Settings table";
			tableLayoutPanelSettings.AccessibleRole = AccessibleRole.Pane;
			tableLayoutPanelSettings.ColumnCount = 4;
			tableLayoutPanelSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
			tableLayoutPanelSettings.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanelSettings.Controls.Add(labelMinRelationships, 0, 0);
			tableLayoutPanelSettings.Controls.Add(numericUpDownMinRelationships, 1, 0);
			tableLayoutPanelSettings.Controls.Add(labelMaxRelationships, 0, 1);
			tableLayoutPanelSettings.Controls.Add(numericUpDownMaxRelationships, 1, 1);
			tableLayoutPanelSettings.Controls.Add(labelTolerance, 0, 2);
			tableLayoutPanelSettings.Controls.Add(numericUpDownTolerance, 1, 2);
			tableLayoutPanelSettings.Controls.Add(labelMinGroupMembers, 0, 3);
			tableLayoutPanelSettings.Controls.Add(numericUpDownMinGroupMembers, 1, 3);
			tableLayoutPanelSettings.Controls.Add(labelElements, 2, 0);
			tableLayoutPanelSettings.Controls.Add(checkBoxSemiMajorAxis, 3, 0);
			tableLayoutPanelSettings.Controls.Add(checkBoxOrbEcc, 3, 1);
			tableLayoutPanelSettings.Controls.Add(checkBoxIncl, 3, 2);
			tableLayoutPanelSettings.Controls.Add(checkBoxLongAscNode, 3, 3);
			tableLayoutPanelSettings.Controls.Add(checkBoxArgPeri, 3, 4);
			tableLayoutPanelSettings.Controls.Add(checkBoxMeanAnomaly, 3, 5);
			tableLayoutPanelSettings.Controls.Add(checkBoxMagAbs, 3, 6);
			tableLayoutPanelSettings.Controls.Add(checkBoxMotion, 3, 7);
			tableLayoutPanelSettings.Controls.Add(checkBoxSlopeParam, 3, 8);
			tableLayoutPanelSettings.Dock = DockStyle.Fill;
			tableLayoutPanelSettings.Location = new Point(0, 0);
			tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
			tableLayoutPanelSettings.PanelBackStyle = PaletteBackStyle.FormMain;
			tableLayoutPanelSettings.RowCount = 9;
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			tableLayoutPanelSettings.Size = new Size(800, 220);
			tableLayoutPanelSettings.TabIndex = 0;
			// 
			// labelMinRelationships
			// 
			labelMinRelationships.AccessibleDescription = "Label for the minimum number of related orbital elements";
			labelMinRelationships.AccessibleName = "Min. relationships label";
			labelMinRelationships.AccessibleRole = AccessibleRole.Text;
			labelMinRelationships.Dock = DockStyle.Fill;
			labelMinRelationships.LabelStyle = LabelStyle.BoldControl;
			labelMinRelationships.Location = new Point(4, 3);
			labelMinRelationships.Margin = new Padding(4, 3, 4, 3);
			labelMinRelationships.Name = "labelMinRelationships";
			labelMinRelationships.Size = new Size(145, 20);
			labelMinRelationships.TabIndex = 0;
			labelMinRelationships.ToolTipValues.Description = "Minimum number of orbital elements that must share common value ranges.";
			labelMinRelationships.ToolTipValues.EnableToolTips = true;
			labelMinRelationships.ToolTipValues.Heading = "Min. relationships";
			labelMinRelationships.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMinRelationships.Values.Text = "Min. relationships (2–7):";
			labelMinRelationships.Enter += Control_Enter;
			labelMinRelationships.Leave += Control_Leave;
			labelMinRelationships.MouseEnter += Control_Enter;
			labelMinRelationships.MouseLeave += Control_Leave;
			// 
			// numericUpDownMinRelationships
			// 
			numericUpDownMinRelationships.AccessibleDescription = "Minimum number of orbital elements that must share a common value range";
			numericUpDownMinRelationships.AccessibleName = "Min. relationships";
			numericUpDownMinRelationships.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinRelationships.Dock = DockStyle.Fill;
			numericUpDownMinRelationships.Location = new Point(157, 3);
			numericUpDownMinRelationships.Maximum = 7;
			numericUpDownMinRelationships.Minimum = 2;
			numericUpDownMinRelationships.Name = "numericUpDownMinRelationships";
			numericUpDownMinRelationships.Size = new Size(74, 20);
			numericUpDownMinRelationships.TabIndex = 1;
			numericUpDownMinRelationships.Value = 2;
			numericUpDownMinRelationships.Enter += Control_Enter;
			numericUpDownMinRelationships.Leave += Control_Leave;
			numericUpDownMinRelationships.MouseEnter += Control_Enter;
			numericUpDownMinRelationships.MouseLeave += Control_Leave;
			// 
			// labelMaxRelationships
			// 
			labelMaxRelationships.AccessibleDescription = "Label for the maximum number of related orbital elements";
			labelMaxRelationships.AccessibleName = "Max. relationships label";
			labelMaxRelationships.AccessibleRole = AccessibleRole.Text;
			labelMaxRelationships.Dock = DockStyle.Fill;
			labelMaxRelationships.LabelStyle = LabelStyle.BoldControl;
			labelMaxRelationships.Location = new Point(4, 29);
			labelMaxRelationships.Margin = new Padding(4, 3, 4, 3);
			labelMaxRelationships.Name = "labelMaxRelationships";
			labelMaxRelationships.Size = new Size(145, 20);
			labelMaxRelationships.TabIndex = 2;
			labelMaxRelationships.ToolTipValues.Description = "Maximum number of orbital elements that may share common value ranges.";
			labelMaxRelationships.ToolTipValues.EnableToolTips = true;
			labelMaxRelationships.ToolTipValues.Heading = "Max. relationships";
			labelMaxRelationships.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMaxRelationships.Values.Text = "Max. relationships (2–7):";
			labelMaxRelationships.Enter += Control_Enter;
			labelMaxRelationships.Leave += Control_Leave;
			labelMaxRelationships.MouseEnter += Control_Enter;
			labelMaxRelationships.MouseLeave += Control_Leave;
			// 
			// numericUpDownMaxRelationships
			// 
			numericUpDownMaxRelationships.AccessibleDescription = "Maximum number of orbital elements that may share a common value range";
			numericUpDownMaxRelationships.AccessibleName = "Max. relationships";
			numericUpDownMaxRelationships.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaxRelationships.Dock = DockStyle.Fill;
			numericUpDownMaxRelationships.Location = new Point(157, 29);
			numericUpDownMaxRelationships.Maximum = 7;
			numericUpDownMaxRelationships.Minimum = 2;
			numericUpDownMaxRelationships.Name = "numericUpDownMaxRelationships";
			numericUpDownMaxRelationships.Size = new Size(74, 20);
			numericUpDownMaxRelationships.TabIndex = 3;
			numericUpDownMaxRelationships.Value = 4;
			numericUpDownMaxRelationships.Enter += Control_Enter;
			numericUpDownMaxRelationships.Leave += Control_Leave;
			numericUpDownMaxRelationships.MouseEnter += Control_Enter;
			numericUpDownMaxRelationships.MouseLeave += Control_Leave;
			// 
			// labelTolerance
			// 
			labelTolerance.AccessibleDescription = "Label for the tolerance (bin width) as a percentage of the value range";
			labelTolerance.AccessibleName = "Tolerance label";
			labelTolerance.AccessibleRole = AccessibleRole.Text;
			labelTolerance.Dock = DockStyle.Fill;
			labelTolerance.LabelStyle = LabelStyle.BoldControl;
			labelTolerance.Location = new Point(4, 55);
			labelTolerance.Margin = new Padding(4, 3, 4, 3);
			labelTolerance.Name = "labelTolerance";
			labelTolerance.Size = new Size(145, 20);
			labelTolerance.TabIndex = 4;
			labelTolerance.ToolTipValues.Description = "Tolerance as a percentage of the value range used to form bins (1–20%).";
			labelTolerance.ToolTipValues.EnableToolTips = true;
			labelTolerance.ToolTipValues.Heading = "Tolerance (%)";
			labelTolerance.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelTolerance.Values.Text = "Tolerance (%):";
			labelTolerance.Enter += Control_Enter;
			labelTolerance.Leave += Control_Leave;
			labelTolerance.MouseEnter += Control_Enter;
			labelTolerance.MouseLeave += Control_Leave;
			// 
			// numericUpDownTolerance
			// 
			numericUpDownTolerance.AccessibleDescription = "Tolerance as a percentage of the value range for grouping orbital elements";
			numericUpDownTolerance.AccessibleName = "Tolerance";
			numericUpDownTolerance.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownTolerance.Dock = DockStyle.Fill;
			numericUpDownTolerance.Location = new Point(157, 55);
			numericUpDownTolerance.Maximum = 20;
			numericUpDownTolerance.Minimum = 1;
			numericUpDownTolerance.Name = "numericUpDownTolerance";
			numericUpDownTolerance.Size = new Size(74, 20);
			numericUpDownTolerance.TabIndex = 5;
			numericUpDownTolerance.Value = 5;
			numericUpDownTolerance.Enter += Control_Enter;
			numericUpDownTolerance.Leave += Control_Leave;
			numericUpDownTolerance.MouseEnter += Control_Enter;
			numericUpDownTolerance.MouseLeave += Control_Leave;
			// 
			// labelMinGroupMembers
			// 
			labelMinGroupMembers.AccessibleDescription = "Label for the minimum number of planetoids in a group";
			labelMinGroupMembers.AccessibleName = "Min. group members label";
			labelMinGroupMembers.AccessibleRole = AccessibleRole.Text;
			labelMinGroupMembers.Dock = DockStyle.Fill;
			labelMinGroupMembers.LabelStyle = LabelStyle.BoldControl;
			labelMinGroupMembers.Location = new Point(4, 81);
			labelMinGroupMembers.Margin = new Padding(4, 3, 4, 3);
			labelMinGroupMembers.Name = "labelMinGroupMembers";
			labelMinGroupMembers.Size = new Size(145, 20);
			labelMinGroupMembers.TabIndex = 6;
			labelMinGroupMembers.ToolTipValues.Description = "Minimum number of planetoids that must belong to a group.";
			labelMinGroupMembers.ToolTipValues.EnableToolTips = true;
			labelMinGroupMembers.ToolTipValues.Heading = "Min. group members";
			labelMinGroupMembers.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMinGroupMembers.Values.Text = "Min. group members:";
			labelMinGroupMembers.Enter += Control_Enter;
			labelMinGroupMembers.Leave += Control_Leave;
			labelMinGroupMembers.MouseEnter += Control_Enter;
			labelMinGroupMembers.MouseLeave += Control_Leave;
			// 
			// numericUpDownMinGroupMembers
			// 
			numericUpDownMinGroupMembers.AccessibleDescription = "Minimum number of planetoids that must be in a group";
			numericUpDownMinGroupMembers.AccessibleName = "Min. group members";
			numericUpDownMinGroupMembers.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinGroupMembers.Dock = DockStyle.Fill;
			numericUpDownMinGroupMembers.Location = new Point(157, 81);
			numericUpDownMinGroupMembers.Maximum = 1000;
			numericUpDownMinGroupMembers.Minimum = 2;
			numericUpDownMinGroupMembers.Name = "numericUpDownMinGroupMembers";
			numericUpDownMinGroupMembers.Size = new Size(74, 20);
			numericUpDownMinGroupMembers.TabIndex = 7;
			numericUpDownMinGroupMembers.Value = 10;
			numericUpDownMinGroupMembers.Enter += Control_Enter;
			numericUpDownMinGroupMembers.Leave += Control_Leave;
			numericUpDownMinGroupMembers.MouseEnter += Control_Enter;
			numericUpDownMinGroupMembers.MouseLeave += Control_Leave;
			// 
			// labelElements
			// 
			labelElements.AccessibleDescription = "Label for the orbital element checkboxes";
			labelElements.AccessibleName = "Orbital elements label";
			labelElements.AccessibleRole = AccessibleRole.Text;
			labelElements.Dock = DockStyle.Fill;
			labelElements.LabelStyle = LabelStyle.BoldControl;
			labelElements.Location = new Point(240, 3);
			labelElements.Margin = new Padding(4, 3, 4, 3);
			labelElements.Name = "labelElements";
			labelElements.Size = new Size(110, 20);
			labelElements.TabIndex = 8;
			tableLayoutPanelSettings.SetRowSpan(labelElements, 2);
			labelElements.ToolTipValues.Description = "Select the orbital elements to include in the relationship search.";
			labelElements.ToolTipValues.EnableToolTips = true;
			labelElements.ToolTipValues.Heading = "Orbital elements";
			labelElements.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElements.Values.Text = "Orbital elements:";
			labelElements.Enter += Control_Enter;
			labelElements.Leave += Control_Leave;
			labelElements.MouseEnter += Control_Enter;
			labelElements.MouseLeave += Control_Leave;
			// 
			// checkBoxSemiMajorAxis
			// 
			checkBoxSemiMajorAxis.AccessibleDescription = "Includes or excludes the semi-major axis in the search";
			checkBoxSemiMajorAxis.AccessibleName = "Semi-major axis";
			checkBoxSemiMajorAxis.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxSemiMajorAxis.Checked = true;
			checkBoxSemiMajorAxis.CheckState = CheckState.Checked;
			checkBoxSemiMajorAxis.Dock = DockStyle.Fill;
			checkBoxSemiMajorAxis.Location = new Point(358, 3);
			checkBoxSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			checkBoxSemiMajorAxis.Name = "checkBoxSemiMajorAxis";
			checkBoxSemiMajorAxis.Size = new Size(120, 20);
			checkBoxSemiMajorAxis.TabIndex = 9;
			checkBoxSemiMajorAxis.Values.Text = "Semi-major axis (a)";
			checkBoxSemiMajorAxis.Enter += Control_Enter;
			checkBoxSemiMajorAxis.Leave += Control_Leave;
			checkBoxSemiMajorAxis.MouseEnter += Control_Enter;
			checkBoxSemiMajorAxis.MouseLeave += Control_Leave;
			// 
			// checkBoxOrbEcc
			// 
			checkBoxOrbEcc.AccessibleDescription = "Includes or excludes the orbital eccentricity in the search";
			checkBoxOrbEcc.AccessibleName = "Orbital eccentricity";
			checkBoxOrbEcc.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxOrbEcc.Checked = true;
			checkBoxOrbEcc.CheckState = CheckState.Checked;
			checkBoxOrbEcc.Dock = DockStyle.Fill;
			checkBoxOrbEcc.Location = new Point(358, 29);
			checkBoxOrbEcc.Margin = new Padding(4, 3, 4, 3);
			checkBoxOrbEcc.Name = "checkBoxOrbEcc";
			checkBoxOrbEcc.Size = new Size(120, 20);
			checkBoxOrbEcc.TabIndex = 10;
			checkBoxOrbEcc.Values.Text = "Eccentricity (e)";
			checkBoxOrbEcc.Enter += Control_Enter;
			checkBoxOrbEcc.Leave += Control_Leave;
			checkBoxOrbEcc.MouseEnter += Control_Enter;
			checkBoxOrbEcc.MouseLeave += Control_Leave;
			// 
			// checkBoxIncl
			// 
			checkBoxIncl.AccessibleDescription = "Includes or excludes the inclination in the search";
			checkBoxIncl.AccessibleName = "Inclination";
			checkBoxIncl.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxIncl.Checked = true;
			checkBoxIncl.CheckState = CheckState.Checked;
			checkBoxIncl.Dock = DockStyle.Fill;
			checkBoxIncl.Location = new Point(358, 55);
			checkBoxIncl.Margin = new Padding(4, 3, 4, 3);
			checkBoxIncl.Name = "checkBoxIncl";
			checkBoxIncl.Size = new Size(120, 20);
			checkBoxIncl.TabIndex = 11;
			checkBoxIncl.Values.Text = "Inclination (i)";
			checkBoxIncl.Enter += Control_Enter;
			checkBoxIncl.Leave += Control_Leave;
			checkBoxIncl.MouseEnter += Control_Enter;
			checkBoxIncl.MouseLeave += Control_Leave;
			// 
			// checkBoxLongAscNode
			// 
			checkBoxLongAscNode.AccessibleDescription = "Includes or excludes the longitude of ascending node in the search";
			checkBoxLongAscNode.AccessibleName = "Longitude of ascending node";
			checkBoxLongAscNode.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxLongAscNode.Checked = false;
			checkBoxLongAscNode.Dock = DockStyle.Fill;
			checkBoxLongAscNode.Location = new Point(358, 81);
			checkBoxLongAscNode.Margin = new Padding(4, 3, 4, 3);
			checkBoxLongAscNode.Name = "checkBoxLongAscNode";
			checkBoxLongAscNode.Size = new Size(120, 20);
			checkBoxLongAscNode.TabIndex = 12;
			checkBoxLongAscNode.Values.Text = "Long. asc. node (Ω)";
			checkBoxLongAscNode.Enter += Control_Enter;
			checkBoxLongAscNode.Leave += Control_Leave;
			checkBoxLongAscNode.MouseEnter += Control_Enter;
			checkBoxLongAscNode.MouseLeave += Control_Leave;
			// 
			// checkBoxArgPeri
			// 
			checkBoxArgPeri.AccessibleDescription = "Includes or excludes the argument of perihelion in the search";
			checkBoxArgPeri.AccessibleName = "Argument of perihelion";
			checkBoxArgPeri.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxArgPeri.Checked = false;
			checkBoxArgPeri.Dock = DockStyle.Fill;
			checkBoxArgPeri.Location = new Point(358, 107);
			checkBoxArgPeri.Margin = new Padding(4, 3, 4, 3);
			checkBoxArgPeri.Name = "checkBoxArgPeri";
			checkBoxArgPeri.Size = new Size(120, 20);
			checkBoxArgPeri.TabIndex = 13;
			checkBoxArgPeri.Values.Text = "Arg. of perihelion (ω)";
			checkBoxArgPeri.Enter += Control_Enter;
			checkBoxArgPeri.Leave += Control_Leave;
			checkBoxArgPeri.MouseEnter += Control_Enter;
			checkBoxArgPeri.MouseLeave += Control_Leave;
			// 
			// checkBoxMeanAnomaly
			// 
			checkBoxMeanAnomaly.AccessibleDescription = "Includes or excludes the mean anomaly in the search";
			checkBoxMeanAnomaly.AccessibleName = "Mean anomaly";
			checkBoxMeanAnomaly.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxMeanAnomaly.Checked = false;
			checkBoxMeanAnomaly.Dock = DockStyle.Fill;
			checkBoxMeanAnomaly.Location = new Point(358, 133);
			checkBoxMeanAnomaly.Margin = new Padding(4, 3, 4, 3);
			checkBoxMeanAnomaly.Name = "checkBoxMeanAnomaly";
			checkBoxMeanAnomaly.Size = new Size(120, 20);
			checkBoxMeanAnomaly.TabIndex = 14;
			checkBoxMeanAnomaly.Values.Text = "Mean anomaly (M)";
			checkBoxMeanAnomaly.Enter += Control_Enter;
			checkBoxMeanAnomaly.Leave += Control_Leave;
			checkBoxMeanAnomaly.MouseEnter += Control_Enter;
			checkBoxMeanAnomaly.MouseLeave += Control_Leave;
			// 
			// checkBoxMagAbs
			// 
			checkBoxMagAbs.AccessibleDescription = "Includes or excludes the absolute magnitude in the search";
			checkBoxMagAbs.AccessibleName = "Absolute magnitude";
			checkBoxMagAbs.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxMagAbs.Checked = false;
			checkBoxMagAbs.Dock = DockStyle.Fill;
			checkBoxMagAbs.Location = new Point(358, 159);
			checkBoxMagAbs.Margin = new Padding(4, 3, 4, 3);
			checkBoxMagAbs.Name = "checkBoxMagAbs";
			checkBoxMagAbs.Size = new Size(120, 20);
			checkBoxMagAbs.TabIndex = 15;
			checkBoxMagAbs.Values.Text = "Abs. magnitude (H)";
			checkBoxMagAbs.Enter += Control_Enter;
			checkBoxMagAbs.Leave += Control_Leave;
			checkBoxMagAbs.MouseEnter += Control_Enter;
			checkBoxMagAbs.MouseLeave += Control_Leave;
			// 
			// checkBoxMotion
			// 
			checkBoxMotion.AccessibleDescription = "Includes or excludes the mean daily motion in the search";
			checkBoxMotion.AccessibleName = "Mean daily motion";
			checkBoxMotion.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxMotion.Checked = false;
			checkBoxMotion.Dock = DockStyle.Fill;
			checkBoxMotion.Location = new Point(358, 185);
			checkBoxMotion.Margin = new Padding(4, 3, 4, 3);
			checkBoxMotion.Name = "checkBoxMotion";
			checkBoxMotion.Size = new Size(120, 20);
			checkBoxMotion.TabIndex = 16;
			checkBoxMotion.Values.Text = "Daily motion (n)";
			checkBoxMotion.Enter += Control_Enter;
			checkBoxMotion.Leave += Control_Leave;
			checkBoxMotion.MouseEnter += Control_Enter;
			checkBoxMotion.MouseLeave += Control_Leave;
			// 
			// checkBoxSlopeParam
			// 
			checkBoxSlopeParam.AccessibleDescription = "Includes or excludes the slope parameter in the search";
			checkBoxSlopeParam.AccessibleName = "Slope parameter";
			checkBoxSlopeParam.AccessibleRole = AccessibleRole.CheckButton;
			checkBoxSlopeParam.Checked = false;
			checkBoxSlopeParam.Dock = DockStyle.Fill;
			checkBoxSlopeParam.Location = new Point(358, 211);
			checkBoxSlopeParam.Margin = new Padding(4, 3, 4, 3);
			checkBoxSlopeParam.Name = "checkBoxSlopeParam";
			checkBoxSlopeParam.Size = new Size(120, 20);
			checkBoxSlopeParam.TabIndex = 17;
			checkBoxSlopeParam.Values.Text = "Slope param. (G)";
			checkBoxSlopeParam.Enter += Control_Enter;
			checkBoxSlopeParam.Leave += Control_Leave;
			checkBoxSlopeParam.MouseEnter += Control_Enter;
			checkBoxSlopeParam.MouseLeave += Control_Leave;
			// 
			// richTextBoxResults
			// 
			richTextBoxResults.AccessibleDescription = "Shows the found orbital element relationship groups";
			richTextBoxResults.AccessibleName = "Results";
			richTextBoxResults.AccessibleRole = AccessibleRole.Text;
			richTextBoxResults.BackColor = SystemColors.Window;
			richTextBoxResults.Dock = DockStyle.Fill;
			richTextBoxResults.Font = new Font("Consolas", 9F);
			richTextBoxResults.Location = new Point(0, 0);
			richTextBoxResults.Name = "richTextBoxResults";
			richTextBoxResults.ReadOnly = true;
			richTextBoxResults.ScrollBars = RichTextBoxScrollBars.Both;
			richTextBoxResults.Size = new Size(800, 296);
			richTextBoxResults.TabIndex = 0;
			richTextBoxResults.Text = "";
			richTextBoxResults.Enter += Control_Enter;
			richTextBoxResults.Leave += Control_Leave;
			richTextBoxResults.MouseEnter += Control_Enter;
			richTextBoxResults.MouseLeave += Control_Leave;
			// 
			// FindRelationshipsForm
			// 
			AccessibleDescription = "Finds groups of planetoids whose orbital elements share common value ranges";
			AccessibleName = "Find relationships and groups";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 570);
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.Sizable;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MinimumSize = new Size(640, 480);
			Name = "FindRelationshipsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Find relationships and groups";
			Load += FindRelationshipsForm_Load;
			FormClosing += FindRelationshipsForm_FormClosing;
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
			((ISupportInitialize)splitContainerMain).EndInit();
			splitContainerMain.Panel1.ResumeLayout(false);
			splitContainerMain.Panel2.ResumeLayout(false);
			splitContainerMain.ResumeLayout(false);
			((ISupportInitialize)panelSettings).EndInit();
			panelSettings.ResumeLayout(false);
			tableLayoutPanelSettings.ResumeLayout(false);
			tableLayoutPanelSettings.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private KryptonManager kryptonManager;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonToolStrip kryptonToolStripIcons;
		private ToolStripButton toolStripButtonStart;
		private ToolStripButton toolStripButtonCancel;
		private ToolStripSeparator toolStripSeparator1;
		private KryptonProgressBarToolStripItem kryptonProgressBar;
		private SplitContainer splitContainerMain;
		private KryptonPanel panelSettings;
		private KryptonTableLayoutPanel tableLayoutPanelSettings;
		private KryptonLabel labelMinRelationships;
		private KryptonNumericUpDown numericUpDownMinRelationships;
		private KryptonLabel labelMaxRelationships;
		private KryptonNumericUpDown numericUpDownMaxRelationships;
		private KryptonLabel labelTolerance;
		private KryptonNumericUpDown numericUpDownTolerance;
		private KryptonLabel labelMinGroupMembers;
		private KryptonNumericUpDown numericUpDownMinGroupMembers;
		private KryptonLabel labelElements;
		private KryptonCheckBox checkBoxSemiMajorAxis;
		private KryptonCheckBox checkBoxOrbEcc;
		private KryptonCheckBox checkBoxIncl;
		private KryptonCheckBox checkBoxLongAscNode;
		private KryptonCheckBox checkBoxArgPeri;
		private KryptonCheckBox checkBoxMeanAnomaly;
		private KryptonCheckBox checkBoxMagAbs;
		private KryptonCheckBox checkBoxMotion;
		private KryptonCheckBox checkBoxSlopeParam;
		private RichTextBox richTextBoxResults;
	}
}
