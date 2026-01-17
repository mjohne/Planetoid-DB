using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class FilterForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FilterForm));
			panel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			buttonReset = new KryptonButton();
			buttonCancel = new KryptonButton();
			buttonApply = new KryptonButton();
			tableLayoutPanel = new KryptonTableLayoutPanel();
			labelHeaderReset = new KryptonLabel();
			labelHeaderMaximum = new KryptonLabel();
			labelHeaderMinimum = new KryptonLabel();
			buttonResetRmsResidual = new KryptonButton();
			buttonResetNumberOfObservations = new KryptonButton();
			buttonNumberOfOppositions = new KryptonButton();
			buttonResetSlopeParameter = new KryptonButton();
			buttonResetAbsoluteMagnitude = new KryptonButton();
			buttonResetSemiMajorAxis = new KryptonButton();
			buttonResetMeanDailyMotion = new KryptonButton();
			buttonResetLongitudeOfTheAscendingNode = new KryptonButton();
			buttonResetArgumentOfPerihelion = new KryptonButton();
			buttonResetMeanAnomalyAtTheEpoch = new KryptonButton();
			numericUpDownMaximumRmsResidual = new KryptonNumericUpDown();
			numericUpDownMaximumNumberOfObservations = new KryptonNumericUpDown();
			numericUpDownMaximumNumberOfOppositions = new KryptonNumericUpDown();
			numericUpDownMaximumSlopeParameter = new KryptonNumericUpDown();
			numericUpDownMaximumAbsoluteMagnitude = new KryptonNumericUpDown();
			numericUpDownMaximumSemiMajorAxis = new KryptonNumericUpDown();
			numericUpDownMaximumMeanDailyMotion = new KryptonNumericUpDown();
			numericUpDownMaximumOrbitalEccentricity = new KryptonNumericUpDown();
			numericUpDownMaximumInclination = new KryptonNumericUpDown();
			numericUpDownMaximumLongitudeOfTheAscendingNode = new KryptonNumericUpDown();
			numericUpDownMaximumArgumentOfPerihelion = new KryptonNumericUpDown();
			numericUpDownMaximumMeanAnomalyAtTheEpoch = new KryptonNumericUpDown();
			numericUpDownMinimumRmsResidual = new KryptonNumericUpDown();
			numericUpDownMinimumNumberOfObservations = new KryptonNumericUpDown();
			numericUpDownMinimumNumberOfOppositions = new KryptonNumericUpDown();
			numericUpDownMinimumSlopeParameter = new KryptonNumericUpDown();
			numericUpDownMinimumAbsoluteMagnitude = new KryptonNumericUpDown();
			numericUpDownMinimumSemiMajorAxis = new KryptonNumericUpDown();
			numericUpDownMinimumMeanDailyMotion = new KryptonNumericUpDown();
			numericUpDownMinimumOrbitalEccentricity = new KryptonNumericUpDown();
			numericUpDownMinimumInclination = new KryptonNumericUpDown();
			numericUpDownMinimumLongitudeOfTheAscendingNode = new KryptonNumericUpDown();
			numericUpDownMinimumArgumentOfPerihelion = new KryptonNumericUpDown();
			labelArgumentOfPerihelion = new KryptonLabel();
			numericUpDownMinimumMeanAnomalyAtTheEpoch = new KryptonNumericUpDown();
			labelRmsResidual = new KryptonLabel();
			labelLongitudeOfTheAscendingNode = new KryptonLabel();
			labelNumberOfObservations = new KryptonLabel();
			labelInclination = new KryptonLabel();
			labelNumberOfOppositions = new KryptonLabel();
			labelOrbitalEccentricity = new KryptonLabel();
			labelSlopeParameter = new KryptonLabel();
			labelMeanDailyMotion = new KryptonLabel();
			labelAbsoluteMagnitude = new KryptonLabel();
			labelSemiMajorAxis = new KryptonLabel();
			labelMeanAnomalyAtTheEpoch = new KryptonLabel();
			buttonResetInclination = new KryptonButton();
			buttonResetOrbitalEccentricity = new KryptonButton();
			labelHeaderElement = new KryptonLabel();
			toolTip = new ToolTip(components);
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			SuspendLayout();
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(statusStrip);
			panel.Controls.Add(buttonReset);
			panel.Controls.Add(buttonCancel);
			panel.Controls.Add(buttonApply);
			panel.Controls.Add(tableLayoutPanel);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.Size = new Size(607, 477);
			panel.TabIndex = 0;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 455);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(607, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 4;
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
			// buttonReset
			// 
			buttonReset.AccessibleDescription = "Resets the filter settings";
			buttonReset.AccessibleName = "Reset the filter settings";
			buttonReset.AccessibleRole = AccessibleRole.PushButton;
			buttonReset.Location = new Point(336, 416);
			buttonReset.Margin = new Padding(4, 3, 4, 3);
			buttonReset.Name = "buttonReset";
			buttonReset.Size = new Size(105, 29);
			buttonReset.TabIndex = 3;
			toolTip.SetToolTip(buttonReset, "Reset the filter settings");
			buttonReset.Values.DropDownArrowColor = Color.Empty;
			buttonReset.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonReset.Values.Text = "&Reset all";
			buttonReset.Click += ButtonReset_Click;
			buttonReset.Enter += SetStatusBar_Enter;
			buttonReset.Leave += ClearStatusBar_Leave;
			buttonReset.MouseEnter += SetStatusBar_Enter;
			buttonReset.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonCancel
			// 
			buttonCancel.AccessibleDescription = "Cancels the filter settings";
			buttonCancel.AccessibleName = "Cancel the filter settings";
			buttonCancel.AccessibleRole = AccessibleRole.PushButton;
			buttonCancel.DialogResult = DialogResult.Cancel;
			buttonCancel.Location = new Point(224, 416);
			buttonCancel.Margin = new Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(105, 29);
			buttonCancel.TabIndex = 2;
			toolTip.SetToolTip(buttonCancel, "Cancel the filter settings");
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += SetStatusBar_Enter;
			buttonCancel.Leave += ClearStatusBar_Leave;
			buttonCancel.MouseEnter += SetStatusBar_Enter;
			buttonCancel.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonApply
			// 
			buttonApply.AccessibleDescription = "Applys the filter settings";
			buttonApply.AccessibleName = "Apply the filter settings";
			buttonApply.AccessibleRole = AccessibleRole.PushButton;
			buttonApply.DialogResult = DialogResult.OK;
			buttonApply.Location = new Point(112, 416);
			buttonApply.Margin = new Padding(4, 3, 4, 3);
			buttonApply.Name = "buttonApply";
			buttonApply.Size = new Size(105, 29);
			buttonApply.TabIndex = 1;
			toolTip.SetToolTip(buttonApply, "Apply the filter settings");
			buttonApply.Values.DropDownArrowColor = Color.Empty;
			buttonApply.Values.Image = FatcowIcons16px.fatcow_accept_button_16px;
			buttonApply.Values.Text = "&Apply";
			buttonApply.Click += ButtonApply_Click;
			buttonApply.Enter += SetStatusBar_Enter;
			buttonApply.Leave += ClearStatusBar_Leave;
			buttonApply.MouseEnter += SetStatusBar_Enter;
			buttonApply.MouseLeave += ClearStatusBar_Leave;
			// 
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Groups the data";
			tableLayoutPanel.AccessibleName = "pane";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
			tableLayoutPanel.ColumnCount = 4;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.Controls.Add(labelHeaderReset, 3, 0);
			tableLayoutPanel.Controls.Add(labelHeaderMaximum, 2, 0);
			tableLayoutPanel.Controls.Add(labelHeaderMinimum, 1, 0);
			tableLayoutPanel.Controls.Add(buttonResetRmsResidual, 3, 13);
			tableLayoutPanel.Controls.Add(buttonResetNumberOfObservations, 3, 12);
			tableLayoutPanel.Controls.Add(buttonNumberOfOppositions, 3, 11);
			tableLayoutPanel.Controls.Add(buttonResetSlopeParameter, 3, 10);
			tableLayoutPanel.Controls.Add(buttonResetAbsoluteMagnitude, 3, 9);
			tableLayoutPanel.Controls.Add(buttonResetSemiMajorAxis, 3, 8);
			tableLayoutPanel.Controls.Add(buttonResetMeanDailyMotion, 3, 7);
			tableLayoutPanel.Controls.Add(buttonResetLongitudeOfTheAscendingNode, 3, 4);
			tableLayoutPanel.Controls.Add(buttonResetArgumentOfPerihelion, 3, 3);
			tableLayoutPanel.Controls.Add(buttonResetMeanAnomalyAtTheEpoch, 3, 2);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumRmsResidual, 2, 13);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumNumberOfObservations, 2, 12);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumNumberOfOppositions, 2, 11);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumSlopeParameter, 2, 10);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumAbsoluteMagnitude, 2, 9);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumSemiMajorAxis, 2, 8);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumMeanDailyMotion, 2, 7);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumOrbitalEccentricity, 2, 6);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumInclination, 2, 5);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumLongitudeOfTheAscendingNode, 2, 4);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumArgumentOfPerihelion, 2, 3);
			tableLayoutPanel.Controls.Add(numericUpDownMaximumMeanAnomalyAtTheEpoch, 2, 2);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumRmsResidual, 1, 13);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumNumberOfObservations, 1, 12);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumNumberOfOppositions, 1, 11);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumSlopeParameter, 1, 10);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumAbsoluteMagnitude, 1, 9);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumSemiMajorAxis, 1, 8);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumMeanDailyMotion, 1, 7);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumOrbitalEccentricity, 1, 6);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumInclination, 1, 5);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumLongitudeOfTheAscendingNode, 1, 4);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumArgumentOfPerihelion, 1, 3);
			tableLayoutPanel.Controls.Add(labelArgumentOfPerihelion, 0, 3);
			tableLayoutPanel.Controls.Add(numericUpDownMinimumMeanAnomalyAtTheEpoch, 1, 2);
			tableLayoutPanel.Controls.Add(labelRmsResidual, 0, 13);
			tableLayoutPanel.Controls.Add(labelLongitudeOfTheAscendingNode, 0, 4);
			tableLayoutPanel.Controls.Add(labelNumberOfObservations, 0, 12);
			tableLayoutPanel.Controls.Add(labelInclination, 0, 5);
			tableLayoutPanel.Controls.Add(labelNumberOfOppositions, 0, 11);
			tableLayoutPanel.Controls.Add(labelOrbitalEccentricity, 0, 6);
			tableLayoutPanel.Controls.Add(labelSlopeParameter, 0, 10);
			tableLayoutPanel.Controls.Add(labelMeanDailyMotion, 0, 7);
			tableLayoutPanel.Controls.Add(labelAbsoluteMagnitude, 0, 9);
			tableLayoutPanel.Controls.Add(labelSemiMajorAxis, 0, 8);
			tableLayoutPanel.Controls.Add(labelMeanAnomalyAtTheEpoch, 0, 2);
			tableLayoutPanel.Controls.Add(buttonResetInclination, 3, 5);
			tableLayoutPanel.Controls.Add(buttonResetOrbitalEccentricity, 3, 6);
			tableLayoutPanel.Controls.Add(labelHeaderElement, 0, 0);
			tableLayoutPanel.Location = new Point(4, 3);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.PanelBackStyle = PaletteBackStyle.FormMain;
			tableLayoutPanel.RowCount = 14;
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
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
			tableLayoutPanel.Size = new Size(595, 397);
			tableLayoutPanel.TabIndex = 0;
			// 
			// labelHeaderReset
			// 
			labelHeaderReset.AccessibleDescription = "Shows the header of the reset buttons";
			labelHeaderReset.AccessibleName = "Header of the reset buttons";
			labelHeaderReset.AccessibleRole = AccessibleRole.StaticText;
			labelHeaderReset.Dock = DockStyle.Fill;
			labelHeaderReset.LabelStyle = LabelStyle.BoldPanel;
			labelHeaderReset.Location = new Point(506, 3);
			labelHeaderReset.Margin = new Padding(4, 3, 4, 3);
			labelHeaderReset.Name = "labelHeaderReset";
			labelHeaderReset.Size = new Size(85, 17);
			labelHeaderReset.TabIndex = 3;
			toolTip.SetToolTip(labelHeaderReset, "Header of the reset buttons");
			labelHeaderReset.Values.Text = "Reset";
			labelHeaderReset.Enter += SetStatusBar_Enter;
			labelHeaderReset.Leave += ClearStatusBar_Leave;
			labelHeaderReset.MouseEnter += SetStatusBar_Enter;
			labelHeaderReset.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelHeaderMaximum
			// 
			labelHeaderMaximum.AccessibleDescription = "Shows the header of the maximum spin buttons";
			labelHeaderMaximum.AccessibleName = "Header of the maximum spin buttons";
			labelHeaderMaximum.AccessibleRole = AccessibleRole.StaticText;
			labelHeaderMaximum.Dock = DockStyle.Fill;
			labelHeaderMaximum.LabelStyle = LabelStyle.BoldPanel;
			labelHeaderMaximum.Location = new Point(378, 3);
			labelHeaderMaximum.Margin = new Padding(4, 3, 4, 3);
			labelHeaderMaximum.Name = "labelHeaderMaximum";
			labelHeaderMaximum.Size = new Size(120, 17);
			labelHeaderMaximum.TabIndex = 2;
			toolTip.SetToolTip(labelHeaderMaximum, "Header of the maximum spin buttons");
			labelHeaderMaximum.Values.Text = "Maximum";
			labelHeaderMaximum.Enter += SetStatusBar_Enter;
			labelHeaderMaximum.Leave += ClearStatusBar_Leave;
			labelHeaderMaximum.MouseEnter += SetStatusBar_Enter;
			labelHeaderMaximum.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelHeaderMinimum
			// 
			labelHeaderMinimum.AccessibleDescription = "Shows the header of the minimum spin buttons";
			labelHeaderMinimum.AccessibleName = "Header of the minimum spin buttons";
			labelHeaderMinimum.AccessibleRole = AccessibleRole.StaticText;
			labelHeaderMinimum.Dock = DockStyle.Fill;
			labelHeaderMinimum.LabelStyle = LabelStyle.BoldPanel;
			labelHeaderMinimum.Location = new Point(250, 3);
			labelHeaderMinimum.Margin = new Padding(4, 3, 4, 3);
			labelHeaderMinimum.Name = "labelHeaderMinimum";
			labelHeaderMinimum.Size = new Size(120, 17);
			labelHeaderMinimum.TabIndex = 1;
			toolTip.SetToolTip(labelHeaderMinimum, "Header of the minimum spin buttons");
			labelHeaderMinimum.Values.Text = "Minimum";
			labelHeaderMinimum.Enter += SetStatusBar_Enter;
			labelHeaderMinimum.Leave += ClearStatusBar_Leave;
			labelHeaderMinimum.MouseEnter += SetStatusBar_Enter;
			labelHeaderMinimum.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetRmsResidual
			// 
			buttonResetRmsResidual.AccessibleDescription = "Resets the minimum and maximum of ";
			buttonResetRmsResidual.AccessibleName = "Reset the minimum and maximum of ";
			buttonResetRmsResidual.AccessibleRole = AccessibleRole.PushButton;
			buttonResetRmsResidual.ButtonStyle = ButtonStyle.Form;
			buttonResetRmsResidual.Dock = DockStyle.Fill;
			buttonResetRmsResidual.Location = new Point(506, 367);
			buttonResetRmsResidual.Margin = new Padding(4, 3, 4, 3);
			buttonResetRmsResidual.Name = "buttonResetRmsResidual";
			buttonResetRmsResidual.Size = new Size(85, 27);
			buttonResetRmsResidual.TabIndex = 51;
			toolTip.SetToolTip(buttonResetRmsResidual, "Reset the minimum and maximum of ");
			buttonResetRmsResidual.ToolTipValues.EnableToolTips = true;
			buttonResetRmsResidual.Values.DropDownArrowColor = Color.Empty;
			buttonResetRmsResidual.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetRmsResidual.Values.Text = "";
			buttonResetRmsResidual.Click += ButtonResetRmsResidual_Click;
			buttonResetRmsResidual.Enter += SetStatusBar_Enter;
			buttonResetRmsResidual.Leave += ClearStatusBar_Leave;
			buttonResetRmsResidual.MouseEnter += SetStatusBar_Enter;
			buttonResetRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetNumberOfObservations
			// 
			buttonResetNumberOfObservations.AccessibleDescription = "Resets the minimum and maximum of number of observations";
			buttonResetNumberOfObservations.AccessibleName = "Reset the minimum and maximum of number of observations";
			buttonResetNumberOfObservations.AccessibleRole = AccessibleRole.PushButton;
			buttonResetNumberOfObservations.ButtonStyle = ButtonStyle.Form;
			buttonResetNumberOfObservations.Dock = DockStyle.Fill;
			buttonResetNumberOfObservations.Location = new Point(506, 336);
			buttonResetNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			buttonResetNumberOfObservations.Name = "buttonResetNumberOfObservations";
			buttonResetNumberOfObservations.Size = new Size(85, 25);
			buttonResetNumberOfObservations.TabIndex = 47;
			toolTip.SetToolTip(buttonResetNumberOfObservations, "Reset the minimum and maximum of number of observations");
			buttonResetNumberOfObservations.ToolTipValues.EnableToolTips = true;
			buttonResetNumberOfObservations.Values.DropDownArrowColor = Color.Empty;
			buttonResetNumberOfObservations.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetNumberOfObservations.Values.Text = "";
			buttonResetNumberOfObservations.Click += ButtonResetNumberOfObservations_Click;
			buttonResetNumberOfObservations.Enter += SetStatusBar_Enter;
			buttonResetNumberOfObservations.Leave += ClearStatusBar_Leave;
			buttonResetNumberOfObservations.MouseEnter += SetStatusBar_Enter;
			buttonResetNumberOfObservations.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonNumberOfOppositions
			// 
			buttonNumberOfOppositions.AccessibleDescription = "Resets the minimum and maximum of number of oppositions";
			buttonNumberOfOppositions.AccessibleName = "Reset the minimum and maximum of number of oppositions";
			buttonNumberOfOppositions.AccessibleRole = AccessibleRole.PushButton;
			buttonNumberOfOppositions.ButtonStyle = ButtonStyle.Form;
			buttonNumberOfOppositions.Dock = DockStyle.Fill;
			buttonNumberOfOppositions.Location = new Point(506, 305);
			buttonNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			buttonNumberOfOppositions.Name = "buttonNumberOfOppositions";
			buttonNumberOfOppositions.Size = new Size(85, 25);
			buttonNumberOfOppositions.TabIndex = 43;
			toolTip.SetToolTip(buttonNumberOfOppositions, "Reset the minimum and maximum of number of oppositions");
			buttonNumberOfOppositions.ToolTipValues.EnableToolTips = true;
			buttonNumberOfOppositions.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfOppositions.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonNumberOfOppositions.Values.Text = "";
			buttonNumberOfOppositions.Click += ButtonNumberOfOppositions_Click;
			buttonNumberOfOppositions.Enter += SetStatusBar_Enter;
			buttonNumberOfOppositions.Leave += ClearStatusBar_Leave;
			buttonNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			buttonNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetSlopeParameter
			// 
			buttonResetSlopeParameter.AccessibleDescription = "Resets the minimum and maximum of slope parameter";
			buttonResetSlopeParameter.AccessibleName = "Reset the minimum and maximum of slope parameter";
			buttonResetSlopeParameter.AccessibleRole = AccessibleRole.PushButton;
			buttonResetSlopeParameter.ButtonStyle = ButtonStyle.Form;
			buttonResetSlopeParameter.Dock = DockStyle.Fill;
			buttonResetSlopeParameter.Location = new Point(506, 274);
			buttonResetSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			buttonResetSlopeParameter.Name = "buttonResetSlopeParameter";
			buttonResetSlopeParameter.Size = new Size(85, 25);
			buttonResetSlopeParameter.TabIndex = 39;
			toolTip.SetToolTip(buttonResetSlopeParameter, "Reset the minimum and maximum of slope parameter");
			buttonResetSlopeParameter.ToolTipValues.EnableToolTips = true;
			buttonResetSlopeParameter.Values.DropDownArrowColor = Color.Empty;
			buttonResetSlopeParameter.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetSlopeParameter.Values.Text = "";
			buttonResetSlopeParameter.Click += ButtonResetSlopeParameter_Click;
			buttonResetSlopeParameter.Enter += SetStatusBar_Enter;
			buttonResetSlopeParameter.Leave += ClearStatusBar_Leave;
			buttonResetSlopeParameter.MouseEnter += SetStatusBar_Enter;
			buttonResetSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetAbsoluteMagnitude
			// 
			buttonResetAbsoluteMagnitude.AccessibleDescription = "Resets the minimum and maximum of absolute magnitude";
			buttonResetAbsoluteMagnitude.AccessibleName = "Reset the minimum and maximum of absolute magnitude";
			buttonResetAbsoluteMagnitude.AccessibleRole = AccessibleRole.PushButton;
			buttonResetAbsoluteMagnitude.ButtonStyle = ButtonStyle.Form;
			buttonResetAbsoluteMagnitude.Dock = DockStyle.Fill;
			buttonResetAbsoluteMagnitude.Location = new Point(506, 243);
			buttonResetAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			buttonResetAbsoluteMagnitude.Name = "buttonResetAbsoluteMagnitude";
			buttonResetAbsoluteMagnitude.Size = new Size(85, 25);
			buttonResetAbsoluteMagnitude.TabIndex = 35;
			toolTip.SetToolTip(buttonResetAbsoluteMagnitude, "Reset the minimum and maximum of absolute magnitude");
			buttonResetAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
			buttonResetAbsoluteMagnitude.Values.DropDownArrowColor = Color.Empty;
			buttonResetAbsoluteMagnitude.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetAbsoluteMagnitude.Values.Text = "";
			buttonResetAbsoluteMagnitude.Click += ButtonResetAbsoluteMagnitude_Click;
			buttonResetAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			buttonResetAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			buttonResetAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			buttonResetAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetSemiMajorAxis
			// 
			buttonResetSemiMajorAxis.AccessibleDescription = "Resets the minimum and maximum of semi-major axis";
			buttonResetSemiMajorAxis.AccessibleName = "Reset the minimum and maximum of semi-major axis";
			buttonResetSemiMajorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonResetSemiMajorAxis.ButtonStyle = ButtonStyle.Form;
			buttonResetSemiMajorAxis.Dock = DockStyle.Fill;
			buttonResetSemiMajorAxis.Location = new Point(506, 212);
			buttonResetSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonResetSemiMajorAxis.Name = "buttonResetSemiMajorAxis";
			buttonResetSemiMajorAxis.Size = new Size(85, 25);
			buttonResetSemiMajorAxis.TabIndex = 31;
			toolTip.SetToolTip(buttonResetSemiMajorAxis, "Reset the minimum and maximum of semi-major axis");
			buttonResetSemiMajorAxis.ToolTipValues.EnableToolTips = true;
			buttonResetSemiMajorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonResetSemiMajorAxis.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetSemiMajorAxis.Values.Text = "";
			buttonResetSemiMajorAxis.Click += ButtonResetSemiMajorAxis_Click;
			buttonResetSemiMajorAxis.Enter += SetStatusBar_Enter;
			buttonResetSemiMajorAxis.Leave += ClearStatusBar_Leave;
			buttonResetSemiMajorAxis.MouseEnter += SetStatusBar_Enter;
			buttonResetSemiMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetMeanDailyMotion
			// 
			buttonResetMeanDailyMotion.AccessibleDescription = "Resets the minimum and maximum of mean daily motion";
			buttonResetMeanDailyMotion.AccessibleName = "Reset the minimum and maximum of mean daily motion";
			buttonResetMeanDailyMotion.AccessibleRole = AccessibleRole.PushButton;
			buttonResetMeanDailyMotion.ButtonStyle = ButtonStyle.Form;
			buttonResetMeanDailyMotion.Dock = DockStyle.Fill;
			buttonResetMeanDailyMotion.Location = new Point(506, 181);
			buttonResetMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			buttonResetMeanDailyMotion.Name = "buttonResetMeanDailyMotion";
			buttonResetMeanDailyMotion.Size = new Size(85, 25);
			buttonResetMeanDailyMotion.TabIndex = 27;
			toolTip.SetToolTip(buttonResetMeanDailyMotion, "Reset the minimum and maximum of mean daily motion");
			buttonResetMeanDailyMotion.ToolTipValues.EnableToolTips = true;
			buttonResetMeanDailyMotion.Values.DropDownArrowColor = Color.Empty;
			buttonResetMeanDailyMotion.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetMeanDailyMotion.Values.Text = "";
			buttonResetMeanDailyMotion.Click += ButtonResetMeanDailyMotion_Click;
			buttonResetMeanDailyMotion.Enter += SetStatusBar_Enter;
			buttonResetMeanDailyMotion.Leave += ClearStatusBar_Leave;
			buttonResetMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			buttonResetMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetLongitudeOfTheAscendingNode
			// 
			buttonResetLongitudeOfTheAscendingNode.AccessibleDescription = "Resets the minimum and maximum of longitude of the ascending node";
			buttonResetLongitudeOfTheAscendingNode.AccessibleName = "Reset the minimum and maximum of longitude of the ascending node";
			buttonResetLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.PushButton;
			buttonResetLongitudeOfTheAscendingNode.ButtonStyle = ButtonStyle.Form;
			buttonResetLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
			buttonResetLongitudeOfTheAscendingNode.Location = new Point(506, 88);
			buttonResetLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			buttonResetLongitudeOfTheAscendingNode.Name = "buttonResetLongitudeOfTheAscendingNode";
			buttonResetLongitudeOfTheAscendingNode.Size = new Size(85, 25);
			buttonResetLongitudeOfTheAscendingNode.TabIndex = 15;
			toolTip.SetToolTip(buttonResetLongitudeOfTheAscendingNode, "Reset the minimum and maximum of longitude of the ascending node");
			buttonResetLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
			buttonResetLongitudeOfTheAscendingNode.Values.DropDownArrowColor = Color.Empty;
			buttonResetLongitudeOfTheAscendingNode.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetLongitudeOfTheAscendingNode.Values.Text = "";
			buttonResetLongitudeOfTheAscendingNode.Click += ButtonResetLongitudeOfTheAscendingNode_Click;
			buttonResetLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			buttonResetLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			buttonResetLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			buttonResetLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetArgumentOfPerihelion
			// 
			buttonResetArgumentOfPerihelion.AccessibleDescription = "Resets the minimum and maximum of argument of perihelion";
			buttonResetArgumentOfPerihelion.AccessibleName = "Reset the minimum and maximum of argument of perihelion";
			buttonResetArgumentOfPerihelion.AccessibleRole = AccessibleRole.PushButton;
			buttonResetArgumentOfPerihelion.ButtonStyle = ButtonStyle.Form;
			buttonResetArgumentOfPerihelion.Dock = DockStyle.Fill;
			buttonResetArgumentOfPerihelion.Location = new Point(506, 57);
			buttonResetArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			buttonResetArgumentOfPerihelion.Name = "buttonResetArgumentOfPerihelion";
			buttonResetArgumentOfPerihelion.Size = new Size(85, 25);
			buttonResetArgumentOfPerihelion.TabIndex = 11;
			toolTip.SetToolTip(buttonResetArgumentOfPerihelion, "Reset the minimum and maximum of argument of perihelion");
			buttonResetArgumentOfPerihelion.ToolTipValues.EnableToolTips = true;
			buttonResetArgumentOfPerihelion.Values.DropDownArrowColor = Color.Empty;
			buttonResetArgumentOfPerihelion.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetArgumentOfPerihelion.Values.Text = "";
			buttonResetArgumentOfPerihelion.Click += ButtonResetArgumentOfPerihelion_Click;
			buttonResetArgumentOfPerihelion.Enter += SetStatusBar_Enter;
			buttonResetArgumentOfPerihelion.Leave += ClearStatusBar_Leave;
			buttonResetArgumentOfPerihelion.MouseEnter += SetStatusBar_Enter;
			buttonResetArgumentOfPerihelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetMeanAnomalyAtTheEpoch
			// 
			buttonResetMeanAnomalyAtTheEpoch.AccessibleDescription = "Resets the minimum and maximum of mean anomaly at the epoch";
			buttonResetMeanAnomalyAtTheEpoch.AccessibleName = "Reset the minimum and maximum of mean anomaly at the epoch";
			buttonResetMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.PushButton;
			buttonResetMeanAnomalyAtTheEpoch.ButtonStyle = ButtonStyle.Form;
			buttonResetMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
			buttonResetMeanAnomalyAtTheEpoch.Location = new Point(506, 26);
			buttonResetMeanAnomalyAtTheEpoch.Margin = new Padding(4, 3, 4, 3);
			buttonResetMeanAnomalyAtTheEpoch.Name = "buttonResetMeanAnomalyAtTheEpoch";
			buttonResetMeanAnomalyAtTheEpoch.Size = new Size(85, 25);
			buttonResetMeanAnomalyAtTheEpoch.TabIndex = 7;
			toolTip.SetToolTip(buttonResetMeanAnomalyAtTheEpoch, "Reset the minimum and maximum of mean anomaly at the epoch");
			buttonResetMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
			buttonResetMeanAnomalyAtTheEpoch.Values.DropDownArrowColor = Color.Empty;
			buttonResetMeanAnomalyAtTheEpoch.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetMeanAnomalyAtTheEpoch.Values.Text = "";
			buttonResetMeanAnomalyAtTheEpoch.Click += ButtonResetMeanAnomalyAtTheEpoch_Click;
			buttonResetMeanAnomalyAtTheEpoch.Enter += SetStatusBar_Enter;
			buttonResetMeanAnomalyAtTheEpoch.Leave += ClearStatusBar_Leave;
			buttonResetMeanAnomalyAtTheEpoch.MouseEnter += SetStatusBar_Enter;
			buttonResetMeanAnomalyAtTheEpoch.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumRmsResidual
			// 
			numericUpDownMaximumRmsResidual.AccessibleDescription = "Shows the maximum of r.m.s. residual";
			numericUpDownMaximumRmsResidual.AccessibleName = "Maximum of r.m.s. residual";
			numericUpDownMaximumRmsResidual.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumRmsResidual.AllowDecimals = true;
			numericUpDownMaximumRmsResidual.DecimalPlaces = 8;
			numericUpDownMaximumRmsResidual.Dock = DockStyle.Fill;
			numericUpDownMaximumRmsResidual.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumRmsResidual.Location = new Point(378, 367);
			numericUpDownMaximumRmsResidual.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumRmsResidual.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumRmsResidual.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumRmsResidual.Name = "numericUpDownMaximumRmsResidual";
			numericUpDownMaximumRmsResidual.Size = new Size(120, 22);
			numericUpDownMaximumRmsResidual.TabIndex = 50;
			numericUpDownMaximumRmsResidual.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumRmsResidual, "Maximum of r.m.s. residual");
			numericUpDownMaximumRmsResidual.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumRmsResidual.ValueChanged += NumericUpDownMaximumRmsResidual_ValueChanged;
			numericUpDownMaximumRmsResidual.Enter += SetStatusBar_Enter;
			numericUpDownMaximumRmsResidual.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumRmsResidual.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumNumberOfObservations
			// 
			numericUpDownMaximumNumberOfObservations.AccessibleDescription = "Shows the maximum ofnumber of observations ";
			numericUpDownMaximumNumberOfObservations.AccessibleName = "Maximum of number of observations";
			numericUpDownMaximumNumberOfObservations.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumNumberOfObservations.AllowDecimals = true;
			numericUpDownMaximumNumberOfObservations.DecimalPlaces = 8;
			numericUpDownMaximumNumberOfObservations.Dock = DockStyle.Fill;
			numericUpDownMaximumNumberOfObservations.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumNumberOfObservations.Location = new Point(378, 336);
			numericUpDownMaximumNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumNumberOfObservations.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumNumberOfObservations.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfObservations.Name = "numericUpDownMaximumNumberOfObservations";
			numericUpDownMaximumNumberOfObservations.Size = new Size(120, 22);
			numericUpDownMaximumNumberOfObservations.TabIndex = 46;
			numericUpDownMaximumNumberOfObservations.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumNumberOfObservations, "Maximum of number of observations");
			numericUpDownMaximumNumberOfObservations.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfObservations.ValueChanged += NumericUpDownMaximumNumberOfObservations_ValueChanged;
			numericUpDownMaximumNumberOfObservations.Enter += SetStatusBar_Enter;
			numericUpDownMaximumNumberOfObservations.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumNumberOfObservations.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumNumberOfObservations.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumNumberOfOppositions
			// 
			numericUpDownMaximumNumberOfOppositions.AccessibleDescription = "Shows the maximum of number of oppositions";
			numericUpDownMaximumNumberOfOppositions.AccessibleName = "Maximum of number of oppositions";
			numericUpDownMaximumNumberOfOppositions.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumNumberOfOppositions.AllowDecimals = true;
			numericUpDownMaximumNumberOfOppositions.DecimalPlaces = 8;
			numericUpDownMaximumNumberOfOppositions.Dock = DockStyle.Fill;
			numericUpDownMaximumNumberOfOppositions.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumNumberOfOppositions.Location = new Point(378, 305);
			numericUpDownMaximumNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumNumberOfOppositions.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumNumberOfOppositions.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfOppositions.Name = "numericUpDownMaximumNumberOfOppositions";
			numericUpDownMaximumNumberOfOppositions.Size = new Size(120, 22);
			numericUpDownMaximumNumberOfOppositions.TabIndex = 42;
			numericUpDownMaximumNumberOfOppositions.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumNumberOfOppositions, "Maximum of number of oppositions");
			numericUpDownMaximumNumberOfOppositions.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfOppositions.ValueChanged += NumericUpDownMaximumNumberOfOppositions_ValueChanged;
			numericUpDownMaximumNumberOfOppositions.Enter += SetStatusBar_Enter;
			numericUpDownMaximumNumberOfOppositions.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumSlopeParameter
			// 
			numericUpDownMaximumSlopeParameter.AccessibleDescription = "Shows the maximum of slope parameter";
			numericUpDownMaximumSlopeParameter.AccessibleName = "Maximum of slope parameter";
			numericUpDownMaximumSlopeParameter.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumSlopeParameter.AllowDecimals = true;
			numericUpDownMaximumSlopeParameter.DecimalPlaces = 8;
			numericUpDownMaximumSlopeParameter.Dock = DockStyle.Fill;
			numericUpDownMaximumSlopeParameter.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumSlopeParameter.Location = new Point(378, 274);
			numericUpDownMaximumSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumSlopeParameter.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumSlopeParameter.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSlopeParameter.Name = "numericUpDownMaximumSlopeParameter";
			numericUpDownMaximumSlopeParameter.Size = new Size(120, 22);
			numericUpDownMaximumSlopeParameter.TabIndex = 38;
			numericUpDownMaximumSlopeParameter.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumSlopeParameter, "Maximum of slope parameter");
			numericUpDownMaximumSlopeParameter.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSlopeParameter.ValueChanged += NumericUpDownMaximumSlopeParameter_ValueChanged;
			numericUpDownMaximumSlopeParameter.Enter += SetStatusBar_Enter;
			numericUpDownMaximumSlopeParameter.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumSlopeParameter.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumAbsoluteMagnitude
			// 
			numericUpDownMaximumAbsoluteMagnitude.AccessibleDescription = "Shows the maximum of absolute magnitude";
			numericUpDownMaximumAbsoluteMagnitude.AccessibleName = "Maximum of absolute magnitude";
			numericUpDownMaximumAbsoluteMagnitude.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumAbsoluteMagnitude.AllowDecimals = true;
			numericUpDownMaximumAbsoluteMagnitude.DecimalPlaces = 8;
			numericUpDownMaximumAbsoluteMagnitude.Dock = DockStyle.Fill;
			numericUpDownMaximumAbsoluteMagnitude.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumAbsoluteMagnitude.Location = new Point(378, 243);
			numericUpDownMaximumAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumAbsoluteMagnitude.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumAbsoluteMagnitude.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumAbsoluteMagnitude.Name = "numericUpDownMaximumAbsoluteMagnitude";
			numericUpDownMaximumAbsoluteMagnitude.Size = new Size(120, 22);
			numericUpDownMaximumAbsoluteMagnitude.TabIndex = 34;
			numericUpDownMaximumAbsoluteMagnitude.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumAbsoluteMagnitude, "Maximum of absolute magnitude");
			numericUpDownMaximumAbsoluteMagnitude.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumAbsoluteMagnitude.ValueChanged += NumericUpDownMaximumAbsoluteMagnitude_ValueChanged;
			numericUpDownMaximumAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			numericUpDownMaximumAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumSemiMajorAxis
			// 
			numericUpDownMaximumSemiMajorAxis.AccessibleDescription = "Shows the maximum of semi-major axis";
			numericUpDownMaximumSemiMajorAxis.AccessibleName = "Maximum of semi-major axis";
			numericUpDownMaximumSemiMajorAxis.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumSemiMajorAxis.AllowDecimals = true;
			numericUpDownMaximumSemiMajorAxis.DecimalPlaces = 8;
			numericUpDownMaximumSemiMajorAxis.Dock = DockStyle.Fill;
			numericUpDownMaximumSemiMajorAxis.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumSemiMajorAxis.Location = new Point(378, 212);
			numericUpDownMaximumSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumSemiMajorAxis.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumSemiMajorAxis.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSemiMajorAxis.Name = "numericUpDownMaximumSemiMajorAxis";
			numericUpDownMaximumSemiMajorAxis.Size = new Size(120, 22);
			numericUpDownMaximumSemiMajorAxis.TabIndex = 30;
			numericUpDownMaximumSemiMajorAxis.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumSemiMajorAxis, "Maximum of semi-major axis");
			numericUpDownMaximumSemiMajorAxis.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSemiMajorAxis.ValueChanged += NumericUpDownMaximumSemiMajorAxis_ValueChanged;
			numericUpDownMaximumSemiMajorAxis.Enter += SetStatusBar_Enter;
			numericUpDownMaximumSemiMajorAxis.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumSemiMajorAxis.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumSemiMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumMeanDailyMotion
			// 
			numericUpDownMaximumMeanDailyMotion.AccessibleDescription = "Shows the maximum of mean daily motion";
			numericUpDownMaximumMeanDailyMotion.AccessibleName = "Maximum of mean daily motion";
			numericUpDownMaximumMeanDailyMotion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumMeanDailyMotion.AllowDecimals = true;
			numericUpDownMaximumMeanDailyMotion.DecimalPlaces = 8;
			numericUpDownMaximumMeanDailyMotion.Dock = DockStyle.Fill;
			numericUpDownMaximumMeanDailyMotion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumMeanDailyMotion.Location = new Point(378, 181);
			numericUpDownMaximumMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumMeanDailyMotion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumMeanDailyMotion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanDailyMotion.Name = "numericUpDownMaximumMeanDailyMotion";
			numericUpDownMaximumMeanDailyMotion.Size = new Size(120, 22);
			numericUpDownMaximumMeanDailyMotion.TabIndex = 26;
			numericUpDownMaximumMeanDailyMotion.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumMeanDailyMotion, "Maximum of mean daily motion");
			numericUpDownMaximumMeanDailyMotion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanDailyMotion.ValueChanged += NumericUpDownMaximumMeanDailyMotion_ValueChanged;
			numericUpDownMaximumMeanDailyMotion.Enter += SetStatusBar_Enter;
			numericUpDownMaximumMeanDailyMotion.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumOrbitalEccentricity
			// 
			numericUpDownMaximumOrbitalEccentricity.AccessibleDescription = "Shows the maximum of orbital eccentricity";
			numericUpDownMaximumOrbitalEccentricity.AccessibleName = "Maximum of orbital eccentricity";
			numericUpDownMaximumOrbitalEccentricity.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumOrbitalEccentricity.AllowDecimals = true;
			numericUpDownMaximumOrbitalEccentricity.DecimalPlaces = 8;
			numericUpDownMaximumOrbitalEccentricity.Dock = DockStyle.Fill;
			numericUpDownMaximumOrbitalEccentricity.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumOrbitalEccentricity.Location = new Point(378, 150);
			numericUpDownMaximumOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumOrbitalEccentricity.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumOrbitalEccentricity.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumOrbitalEccentricity.Name = "numericUpDownMaximumOrbitalEccentricity";
			numericUpDownMaximumOrbitalEccentricity.Size = new Size(120, 22);
			numericUpDownMaximumOrbitalEccentricity.TabIndex = 22;
			numericUpDownMaximumOrbitalEccentricity.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumOrbitalEccentricity, "Maximum of orbital eccentricity");
			numericUpDownMaximumOrbitalEccentricity.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumOrbitalEccentricity.ValueChanged += NumericUpDownMaximumOrbitalEccentricity_ValueChanged;
			numericUpDownMaximumOrbitalEccentricity.Enter += SetStatusBar_Enter;
			numericUpDownMaximumOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumInclination
			// 
			numericUpDownMaximumInclination.AccessibleDescription = "Shows the maximum of inclination to the ecliptic";
			numericUpDownMaximumInclination.AccessibleName = "Maximum of inclination to the ecliptic";
			numericUpDownMaximumInclination.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumInclination.AllowDecimals = true;
			numericUpDownMaximumInclination.DecimalPlaces = 8;
			numericUpDownMaximumInclination.Dock = DockStyle.Fill;
			numericUpDownMaximumInclination.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumInclination.Location = new Point(378, 119);
			numericUpDownMaximumInclination.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumInclination.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumInclination.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumInclination.Name = "numericUpDownMaximumInclination";
			numericUpDownMaximumInclination.Size = new Size(120, 22);
			numericUpDownMaximumInclination.TabIndex = 18;
			numericUpDownMaximumInclination.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumInclination, "Maximum of inclination to the ecliptic");
			numericUpDownMaximumInclination.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumInclination.ValueChanged += NumericUpDownMaximumInclination_ValueChanged;
			numericUpDownMaximumInclination.Enter += SetStatusBar_Enter;
			numericUpDownMaximumInclination.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumInclination.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumLongitudeOfTheAscendingNode
			// 
			numericUpDownMaximumLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the maximum of longitude of the ascending node";
			numericUpDownMaximumLongitudeOfTheAscendingNode.AccessibleName = "Maximum of longitude of the ascending node";
			numericUpDownMaximumLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumLongitudeOfTheAscendingNode.AllowDecimals = true;
			numericUpDownMaximumLongitudeOfTheAscendingNode.DecimalPlaces = 8;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumLongitudeOfTheAscendingNode.Location = new Point(378, 88);
			numericUpDownMaximumLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumLongitudeOfTheAscendingNode.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumLongitudeOfTheAscendingNode.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumLongitudeOfTheAscendingNode.Name = "numericUpDownMaximumLongitudeOfTheAscendingNode";
			numericUpDownMaximumLongitudeOfTheAscendingNode.Size = new Size(120, 22);
			numericUpDownMaximumLongitudeOfTheAscendingNode.TabIndex = 14;
			numericUpDownMaximumLongitudeOfTheAscendingNode.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumLongitudeOfTheAscendingNode, "Maximum of longitude of the ascending node");
			numericUpDownMaximumLongitudeOfTheAscendingNode.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumLongitudeOfTheAscendingNode.ValueChanged += NumericUpDownMaximumLongitudeOfTheAscendingNode_ValueChanged;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumArgumentOfPerihelion
			// 
			numericUpDownMaximumArgumentOfPerihelion.AccessibleDescription = "Shows the maximum of argument of perihelion";
			numericUpDownMaximumArgumentOfPerihelion.AccessibleName = "Maximum of argument of perihelion";
			numericUpDownMaximumArgumentOfPerihelion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumArgumentOfPerihelion.AllowDecimals = true;
			numericUpDownMaximumArgumentOfPerihelion.DecimalPlaces = 8;
			numericUpDownMaximumArgumentOfPerihelion.Dock = DockStyle.Fill;
			numericUpDownMaximumArgumentOfPerihelion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumArgumentOfPerihelion.Location = new Point(378, 57);
			numericUpDownMaximumArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumArgumentOfPerihelion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumArgumentOfPerihelion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumArgumentOfPerihelion.Name = "numericUpDownMaximumArgumentOfPerihelion";
			numericUpDownMaximumArgumentOfPerihelion.Size = new Size(120, 22);
			numericUpDownMaximumArgumentOfPerihelion.TabIndex = 10;
			numericUpDownMaximumArgumentOfPerihelion.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumArgumentOfPerihelion, "Maximum of argument of perihelion");
			numericUpDownMaximumArgumentOfPerihelion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumArgumentOfPerihelion.ValueChanged += NumericUpDownMaximumArgumentOfPerihelion_ValueChanged;
			numericUpDownMaximumArgumentOfPerihelion.Enter += SetStatusBar_Enter;
			numericUpDownMaximumArgumentOfPerihelion.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumArgumentOfPerihelion.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumArgumentOfPerihelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMaximumMeanAnomalyAtTheEpoch
			// 
			numericUpDownMaximumMeanAnomalyAtTheEpoch.AccessibleDescription = "Show the maximum of mean anomaly at the epoch";
			numericUpDownMaximumMeanAnomalyAtTheEpoch.AccessibleName = "Maximum of mean anomaly at the epoch";
			numericUpDownMaximumMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.AllowDecimals = true;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.DecimalPlaces = 8;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Location = new Point(378, 26);
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Name = "numericUpDownMaximumMeanAnomalyAtTheEpoch";
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Size = new Size(120, 22);
			numericUpDownMaximumMeanAnomalyAtTheEpoch.TabIndex = 6;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMaximumMeanAnomalyAtTheEpoch, "Maximum of mean anomaly at the epoch");
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ValueChanged += NumericUpDownMaximumMeanAnomalyAtTheEpoch_ValueChanged;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Enter += SetStatusBar_Enter;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Leave += ClearStatusBar_Leave;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.MouseEnter += SetStatusBar_Enter;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumRmsResidual
			// 
			numericUpDownMinimumRmsResidual.AccessibleDescription = "Shows the minimum of r.m.s. residual";
			numericUpDownMinimumRmsResidual.AccessibleName = "Minimum of r.m.s. residual";
			numericUpDownMinimumRmsResidual.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumRmsResidual.AllowDecimals = true;
			numericUpDownMinimumRmsResidual.DecimalPlaces = 8;
			numericUpDownMinimumRmsResidual.Dock = DockStyle.Fill;
			numericUpDownMinimumRmsResidual.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumRmsResidual.Location = new Point(250, 367);
			numericUpDownMinimumRmsResidual.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumRmsResidual.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumRmsResidual.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumRmsResidual.Name = "numericUpDownMinimumRmsResidual";
			numericUpDownMinimumRmsResidual.Size = new Size(120, 22);
			numericUpDownMinimumRmsResidual.TabIndex = 49;
			numericUpDownMinimumRmsResidual.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumRmsResidual, "Minimum of r.m.s. residual");
			numericUpDownMinimumRmsResidual.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumRmsResidual.ValueChanged += NumericUpDownMinimumRmsResidual_ValueChanged;
			numericUpDownMinimumRmsResidual.Enter += SetStatusBar_Enter;
			numericUpDownMinimumRmsResidual.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumRmsResidual.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumNumberOfObservations
			// 
			numericUpDownMinimumNumberOfObservations.AccessibleDescription = "Shows the minimum of number of observations";
			numericUpDownMinimumNumberOfObservations.AccessibleName = "Minimum of number of observations";
			numericUpDownMinimumNumberOfObservations.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumNumberOfObservations.AllowDecimals = true;
			numericUpDownMinimumNumberOfObservations.DecimalPlaces = 8;
			numericUpDownMinimumNumberOfObservations.Dock = DockStyle.Fill;
			numericUpDownMinimumNumberOfObservations.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumNumberOfObservations.Location = new Point(250, 336);
			numericUpDownMinimumNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumNumberOfObservations.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumNumberOfObservations.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfObservations.Name = "numericUpDownMinimumNumberOfObservations";
			numericUpDownMinimumNumberOfObservations.Size = new Size(120, 22);
			numericUpDownMinimumNumberOfObservations.TabIndex = 45;
			numericUpDownMinimumNumberOfObservations.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumNumberOfObservations, "Minimum of number of observations");
			numericUpDownMinimumNumberOfObservations.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfObservations.ValueChanged += NumericUpDownMinimumNumberOfObservations_ValueChanged;
			numericUpDownMinimumNumberOfObservations.Enter += SetStatusBar_Enter;
			numericUpDownMinimumNumberOfObservations.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumNumberOfObservations.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumNumberOfObservations.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumNumberOfOppositions
			// 
			numericUpDownMinimumNumberOfOppositions.AccessibleDescription = "Shows the minimum of number of oppositions";
			numericUpDownMinimumNumberOfOppositions.AccessibleName = "Minimum of number of oppositions";
			numericUpDownMinimumNumberOfOppositions.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumNumberOfOppositions.AllowDecimals = true;
			numericUpDownMinimumNumberOfOppositions.DecimalPlaces = 8;
			numericUpDownMinimumNumberOfOppositions.Dock = DockStyle.Fill;
			numericUpDownMinimumNumberOfOppositions.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumNumberOfOppositions.Location = new Point(250, 305);
			numericUpDownMinimumNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumNumberOfOppositions.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumNumberOfOppositions.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfOppositions.Name = "numericUpDownMinimumNumberOfOppositions";
			numericUpDownMinimumNumberOfOppositions.Size = new Size(120, 22);
			numericUpDownMinimumNumberOfOppositions.TabIndex = 41;
			numericUpDownMinimumNumberOfOppositions.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumNumberOfOppositions, "Minimum of number of oppositions");
			numericUpDownMinimumNumberOfOppositions.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfOppositions.ValueChanged += NumericUpDownMinimumNumberOfOppositions_ValueChanged;
			numericUpDownMinimumNumberOfOppositions.Enter += SetStatusBar_Enter;
			numericUpDownMinimumNumberOfOppositions.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumSlopeParameter
			// 
			numericUpDownMinimumSlopeParameter.AccessibleDescription = "Shows the minimum of slope parameter";
			numericUpDownMinimumSlopeParameter.AccessibleName = "Minimum of slope parameter";
			numericUpDownMinimumSlopeParameter.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumSlopeParameter.AllowDecimals = true;
			numericUpDownMinimumSlopeParameter.DecimalPlaces = 8;
			numericUpDownMinimumSlopeParameter.Dock = DockStyle.Fill;
			numericUpDownMinimumSlopeParameter.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumSlopeParameter.Location = new Point(250, 274);
			numericUpDownMinimumSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumSlopeParameter.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumSlopeParameter.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSlopeParameter.Name = "numericUpDownMinimumSlopeParameter";
			numericUpDownMinimumSlopeParameter.Size = new Size(120, 22);
			numericUpDownMinimumSlopeParameter.TabIndex = 37;
			numericUpDownMinimumSlopeParameter.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumSlopeParameter, "Minimum of slope parameter");
			numericUpDownMinimumSlopeParameter.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSlopeParameter.ValueChanged += NumericUpDownMinimumSlopeParameter_ValueChanged;
			numericUpDownMinimumSlopeParameter.Enter += SetStatusBar_Enter;
			numericUpDownMinimumSlopeParameter.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumSlopeParameter.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumAbsoluteMagnitude
			// 
			numericUpDownMinimumAbsoluteMagnitude.AccessibleDescription = "Shows the minimum of absolute magnitude";
			numericUpDownMinimumAbsoluteMagnitude.AccessibleName = "Minimum of absolute magnitude";
			numericUpDownMinimumAbsoluteMagnitude.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumAbsoluteMagnitude.AllowDecimals = true;
			numericUpDownMinimumAbsoluteMagnitude.DecimalPlaces = 8;
			numericUpDownMinimumAbsoluteMagnitude.Dock = DockStyle.Fill;
			numericUpDownMinimumAbsoluteMagnitude.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumAbsoluteMagnitude.Location = new Point(250, 243);
			numericUpDownMinimumAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumAbsoluteMagnitude.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumAbsoluteMagnitude.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumAbsoluteMagnitude.Name = "numericUpDownMinimumAbsoluteMagnitude";
			numericUpDownMinimumAbsoluteMagnitude.Size = new Size(120, 22);
			numericUpDownMinimumAbsoluteMagnitude.TabIndex = 33;
			numericUpDownMinimumAbsoluteMagnitude.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumAbsoluteMagnitude, "Minimum of absolute magnitude");
			numericUpDownMinimumAbsoluteMagnitude.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumAbsoluteMagnitude.ValueChanged += NumericUpDownMinimumAbsoluteMagnitude_ValueChanged;
			numericUpDownMinimumAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			numericUpDownMinimumAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumSemiMajorAxis
			// 
			numericUpDownMinimumSemiMajorAxis.AccessibleDescription = "Shows the minimum of semi-major axis";
			numericUpDownMinimumSemiMajorAxis.AccessibleName = "Minimum of semi-major axis";
			numericUpDownMinimumSemiMajorAxis.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumSemiMajorAxis.AllowDecimals = true;
			numericUpDownMinimumSemiMajorAxis.DecimalPlaces = 8;
			numericUpDownMinimumSemiMajorAxis.Dock = DockStyle.Fill;
			numericUpDownMinimumSemiMajorAxis.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumSemiMajorAxis.Location = new Point(250, 212);
			numericUpDownMinimumSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumSemiMajorAxis.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumSemiMajorAxis.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSemiMajorAxis.Name = "numericUpDownMinimumSemiMajorAxis";
			numericUpDownMinimumSemiMajorAxis.Size = new Size(120, 22);
			numericUpDownMinimumSemiMajorAxis.TabIndex = 29;
			numericUpDownMinimumSemiMajorAxis.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumSemiMajorAxis, "Minimum of semi-major axis");
			numericUpDownMinimumSemiMajorAxis.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSemiMajorAxis.ValueChanged += NumericUpDownMinimumSemiMajorAxis_ValueChanged;
			numericUpDownMinimumSemiMajorAxis.Enter += SetStatusBar_Enter;
			numericUpDownMinimumSemiMajorAxis.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumSemiMajorAxis.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumSemiMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumMeanDailyMotion
			// 
			numericUpDownMinimumMeanDailyMotion.AccessibleDescription = "Shows the minimum of mean daily motion";
			numericUpDownMinimumMeanDailyMotion.AccessibleName = "Minimum of mean daily motion";
			numericUpDownMinimumMeanDailyMotion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumMeanDailyMotion.AllowDecimals = true;
			numericUpDownMinimumMeanDailyMotion.DecimalPlaces = 8;
			numericUpDownMinimumMeanDailyMotion.Dock = DockStyle.Fill;
			numericUpDownMinimumMeanDailyMotion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumMeanDailyMotion.Location = new Point(250, 181);
			numericUpDownMinimumMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumMeanDailyMotion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumMeanDailyMotion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanDailyMotion.Name = "numericUpDownMinimumMeanDailyMotion";
			numericUpDownMinimumMeanDailyMotion.Size = new Size(120, 22);
			numericUpDownMinimumMeanDailyMotion.TabIndex = 25;
			numericUpDownMinimumMeanDailyMotion.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumMeanDailyMotion, "Minimum of mean daily motion");
			numericUpDownMinimumMeanDailyMotion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanDailyMotion.ValueChanged += NumericUpDownMinimumMeanDailyMotion_ValueChanged;
			numericUpDownMinimumMeanDailyMotion.Enter += SetStatusBar_Enter;
			numericUpDownMinimumMeanDailyMotion.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumOrbitalEccentricity
			// 
			numericUpDownMinimumOrbitalEccentricity.AccessibleDescription = "Shows the minimum of orbital eccentricity";
			numericUpDownMinimumOrbitalEccentricity.AccessibleName = "Minimum of orbital eccentricity";
			numericUpDownMinimumOrbitalEccentricity.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumOrbitalEccentricity.AllowDecimals = true;
			numericUpDownMinimumOrbitalEccentricity.DecimalPlaces = 8;
			numericUpDownMinimumOrbitalEccentricity.Dock = DockStyle.Fill;
			numericUpDownMinimumOrbitalEccentricity.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumOrbitalEccentricity.Location = new Point(250, 150);
			numericUpDownMinimumOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumOrbitalEccentricity.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumOrbitalEccentricity.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumOrbitalEccentricity.Name = "numericUpDownMinimumOrbitalEccentricity";
			numericUpDownMinimumOrbitalEccentricity.Size = new Size(120, 22);
			numericUpDownMinimumOrbitalEccentricity.TabIndex = 21;
			numericUpDownMinimumOrbitalEccentricity.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumOrbitalEccentricity, "Minimum of orbital eccentricity");
			numericUpDownMinimumOrbitalEccentricity.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumOrbitalEccentricity.ValueChanged += NumericUpDownMinimumOrbitalEccentricity_ValueChanged;
			numericUpDownMinimumOrbitalEccentricity.Enter += SetStatusBar_Enter;
			numericUpDownMinimumOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumInclination
			// 
			numericUpDownMinimumInclination.AccessibleDescription = "Shows the minimum of inclination to the ecliptic";
			numericUpDownMinimumInclination.AccessibleName = "Minimum of inclination to the ecliptic";
			numericUpDownMinimumInclination.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumInclination.AllowDecimals = true;
			numericUpDownMinimumInclination.DecimalPlaces = 8;
			numericUpDownMinimumInclination.Dock = DockStyle.Fill;
			numericUpDownMinimumInclination.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumInclination.Location = new Point(250, 119);
			numericUpDownMinimumInclination.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumInclination.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumInclination.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumInclination.Name = "numericUpDownMinimumInclination";
			numericUpDownMinimumInclination.Size = new Size(120, 22);
			numericUpDownMinimumInclination.TabIndex = 17;
			numericUpDownMinimumInclination.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumInclination, "Minimum of inclination to the ecliptic");
			numericUpDownMinimumInclination.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumInclination.ValueChanged += NumericUpDownMinimumInclination_ValueChanged;
			numericUpDownMinimumInclination.Enter += SetStatusBar_Enter;
			numericUpDownMinimumInclination.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumInclination.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumLongitudeOfTheAscendingNode
			// 
			numericUpDownMinimumLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the minimum of longitude of the ascending node";
			numericUpDownMinimumLongitudeOfTheAscendingNode.AccessibleName = "Minimum of longitude of the ascending node";
			numericUpDownMinimumLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumLongitudeOfTheAscendingNode.AllowDecimals = true;
			numericUpDownMinimumLongitudeOfTheAscendingNode.DecimalPlaces = 8;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumLongitudeOfTheAscendingNode.Location = new Point(250, 88);
			numericUpDownMinimumLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumLongitudeOfTheAscendingNode.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumLongitudeOfTheAscendingNode.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumLongitudeOfTheAscendingNode.Name = "numericUpDownMinimumLongitudeOfTheAscendingNode";
			numericUpDownMinimumLongitudeOfTheAscendingNode.Size = new Size(120, 22);
			numericUpDownMinimumLongitudeOfTheAscendingNode.TabIndex = 13;
			numericUpDownMinimumLongitudeOfTheAscendingNode.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumLongitudeOfTheAscendingNode, "Minimum of longitude of the ascending node");
			numericUpDownMinimumLongitudeOfTheAscendingNode.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumLongitudeOfTheAscendingNode.ValueChanged += NumericUpDownMinimumLongitudeOfTheAscendingNode_ValueChanged;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumArgumentOfPerihelion
			// 
			numericUpDownMinimumArgumentOfPerihelion.AccessibleDescription = "Shows the minimum of argument of perihelion";
			numericUpDownMinimumArgumentOfPerihelion.AccessibleName = "Minimum of argument of perihelion";
			numericUpDownMinimumArgumentOfPerihelion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumArgumentOfPerihelion.AllowDecimals = true;
			numericUpDownMinimumArgumentOfPerihelion.DecimalPlaces = 8;
			numericUpDownMinimumArgumentOfPerihelion.Dock = DockStyle.Fill;
			numericUpDownMinimumArgumentOfPerihelion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumArgumentOfPerihelion.Location = new Point(250, 57);
			numericUpDownMinimumArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumArgumentOfPerihelion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumArgumentOfPerihelion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumArgumentOfPerihelion.Name = "numericUpDownMinimumArgumentOfPerihelion";
			numericUpDownMinimumArgumentOfPerihelion.Size = new Size(120, 22);
			numericUpDownMinimumArgumentOfPerihelion.TabIndex = 9;
			numericUpDownMinimumArgumentOfPerihelion.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumArgumentOfPerihelion, "Minimum of argument of perihelion");
			numericUpDownMinimumArgumentOfPerihelion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumArgumentOfPerihelion.ValueChanged += NumericUpDownMinimumArgumentOfPerihelion_ValueChanged;
			numericUpDownMinimumArgumentOfPerihelion.Enter += SetStatusBar_Enter;
			numericUpDownMinimumArgumentOfPerihelion.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumArgumentOfPerihelion.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumArgumentOfPerihelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelArgumentOfPerihelion
			// 
			labelArgumentOfPerihelion.AccessibleDescription = "Shows the minimum and maximum of argument of perihelion";
			labelArgumentOfPerihelion.AccessibleName = "Argument of perihelion";
			labelArgumentOfPerihelion.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentOfPerihelion.Dock = DockStyle.Fill;
			labelArgumentOfPerihelion.Location = new Point(4, 57);
			labelArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			labelArgumentOfPerihelion.Name = "labelArgumentOfPerihelion";
			labelArgumentOfPerihelion.Size = new Size(238, 25);
			labelArgumentOfPerihelion.TabIndex = 8;
			toolTip.SetToolTip(labelArgumentOfPerihelion, "Argument of perihelion, J2000.0");
			labelArgumentOfPerihelion.Values.Text = "Argument of perihelion, J2000.0";
			labelArgumentOfPerihelion.Enter += SetStatusBar_Enter;
			labelArgumentOfPerihelion.Leave += ClearStatusBar_Leave;
			labelArgumentOfPerihelion.MouseEnter += SetStatusBar_Enter;
			labelArgumentOfPerihelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// numericUpDownMinimumMeanAnomalyAtTheEpoch
			// 
			numericUpDownMinimumMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the minimum of mean anomaly at the epoch";
			numericUpDownMinimumMeanAnomalyAtTheEpoch.AccessibleName = "Minimum of mean anomaly at the epoch";
			numericUpDownMinimumMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.AllowDecimals = true;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.DecimalPlaces = 8;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Location = new Point(250, 26);
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Name = "numericUpDownMinimumMeanAnomalyAtTheEpoch";
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Size = new Size(120, 22);
			numericUpDownMinimumMeanAnomalyAtTheEpoch.TabIndex = 5;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ThousandsSeparator = true;
			toolTip.SetToolTip(numericUpDownMinimumMeanAnomalyAtTheEpoch, "Minimum of mean anomaly at the epoch");
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ValueChanged += NumericUpDownMinimumMeanAnomalyAtTheEpoch_ValueChanged;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Enter += SetStatusBar_Enter;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Leave += ClearStatusBar_Leave;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.MouseEnter += SetStatusBar_Enter;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelRmsResidual
			// 
			labelRmsResidual.AccessibleDescription = "Shows the minimum and maximum of r.m.s. residual";
			labelRmsResidual.AccessibleName = "r.m.s. residual";
			labelRmsResidual.AccessibleRole = AccessibleRole.StaticText;
			labelRmsResidual.Dock = DockStyle.Fill;
			labelRmsResidual.Location = new Point(4, 367);
			labelRmsResidual.Margin = new Padding(4, 3, 4, 3);
			labelRmsResidual.Name = "labelRmsResidual";
			labelRmsResidual.Size = new Size(238, 27);
			labelRmsResidual.TabIndex = 48;
			toolTip.SetToolTip(labelRmsResidual, "r.m.s. residual");
			labelRmsResidual.Values.Text = "r.m.s. residual";
			labelRmsResidual.Enter += SetStatusBar_Enter;
			labelRmsResidual.Leave += ClearStatusBar_Leave;
			labelRmsResidual.MouseEnter += SetStatusBar_Enter;
			labelRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelLongitudeOfTheAscendingNode
			// 
			labelLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the minimum and maximum of longitude of the ascending node";
			labelLongitudeOfTheAscendingNode.AccessibleName = "Longitude of the ascending node";
			labelLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.StaticText;
			labelLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
			labelLongitudeOfTheAscendingNode.Location = new Point(4, 88);
			labelLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			labelLongitudeOfTheAscendingNode.Name = "labelLongitudeOfTheAscendingNode";
			labelLongitudeOfTheAscendingNode.Size = new Size(238, 25);
			labelLongitudeOfTheAscendingNode.TabIndex = 12;
			toolTip.SetToolTip(labelLongitudeOfTheAscendingNode, "Longitude of the ascending node, J2000.0");
			labelLongitudeOfTheAscendingNode.Values.Text = "Longitude of the ascending node, J2000.0";
			labelLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			labelLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			labelLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			labelLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelNumberOfObservations
			// 
			labelNumberOfObservations.AccessibleDescription = "Shows the minimum and maximum of number of observations";
			labelNumberOfObservations.AccessibleName = "Number of observations";
			labelNumberOfObservations.AccessibleRole = AccessibleRole.StaticText;
			labelNumberOfObservations.Dock = DockStyle.Fill;
			labelNumberOfObservations.Location = new Point(4, 336);
			labelNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			labelNumberOfObservations.Name = "labelNumberOfObservations";
			labelNumberOfObservations.Size = new Size(238, 25);
			labelNumberOfObservations.TabIndex = 44;
			toolTip.SetToolTip(labelNumberOfObservations, "Number of observations");
			labelNumberOfObservations.Values.Text = "Number of observations";
			labelNumberOfObservations.Enter += SetStatusBar_Enter;
			labelNumberOfObservations.Leave += ClearStatusBar_Leave;
			labelNumberOfObservations.MouseEnter += SetStatusBar_Enter;
			labelNumberOfObservations.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelInclination
			// 
			labelInclination.AccessibleDescription = "Shows the minimum and maximum of inclination to the ecliptic";
			labelInclination.AccessibleName = "Inclination to the ecliptic";
			labelInclination.AccessibleRole = AccessibleRole.StaticText;
			labelInclination.Dock = DockStyle.Fill;
			labelInclination.Location = new Point(4, 119);
			labelInclination.Margin = new Padding(4, 3, 4, 3);
			labelInclination.Name = "labelInclination";
			labelInclination.Size = new Size(238, 25);
			labelInclination.TabIndex = 16;
			toolTip.SetToolTip(labelInclination, "Inclination to the ecliptic, J2000.0");
			labelInclination.Values.Text = "Inclination to the ecliptic, J2000.0";
			labelInclination.Enter += SetStatusBar_Enter;
			labelInclination.Leave += ClearStatusBar_Leave;
			labelInclination.MouseEnter += SetStatusBar_Enter;
			labelInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelNumberOfOppositions
			// 
			labelNumberOfOppositions.AccessibleDescription = "Shows the minimum and maximum of number of oppositions";
			labelNumberOfOppositions.AccessibleName = "Number of oppositions";
			labelNumberOfOppositions.AccessibleRole = AccessibleRole.StaticText;
			labelNumberOfOppositions.Dock = DockStyle.Fill;
			labelNumberOfOppositions.Location = new Point(4, 305);
			labelNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			labelNumberOfOppositions.Name = "labelNumberOfOppositions";
			labelNumberOfOppositions.Size = new Size(238, 25);
			labelNumberOfOppositions.TabIndex = 40;
			toolTip.SetToolTip(labelNumberOfOppositions, "Number of oppositions");
			labelNumberOfOppositions.Values.Text = "Number of oppositions";
			labelNumberOfOppositions.Enter += SetStatusBar_Enter;
			labelNumberOfOppositions.Leave += ClearStatusBar_Leave;
			labelNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			labelNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelOrbitalEccentricity
			// 
			labelOrbitalEccentricity.AccessibleDescription = "Shows the minimum and maximum of orbital eccentricity";
			labelOrbitalEccentricity.AccessibleName = "Orbital eccentricity";
			labelOrbitalEccentricity.AccessibleRole = AccessibleRole.StaticText;
			labelOrbitalEccentricity.Dock = DockStyle.Fill;
			labelOrbitalEccentricity.Location = new Point(4, 150);
			labelOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			labelOrbitalEccentricity.Name = "labelOrbitalEccentricity";
			labelOrbitalEccentricity.Size = new Size(238, 25);
			labelOrbitalEccentricity.TabIndex = 20;
			toolTip.SetToolTip(labelOrbitalEccentricity, "Orbital eccentricity");
			labelOrbitalEccentricity.Values.Text = "Orbital eccentricity";
			labelOrbitalEccentricity.Enter += SetStatusBar_Enter;
			labelOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			labelOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			labelOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSlopeParameter
			// 
			labelSlopeParameter.AccessibleDescription = "Shows the minimum and maximum of slope parameter";
			labelSlopeParameter.AccessibleName = "Slope parameter";
			labelSlopeParameter.AccessibleRole = AccessibleRole.StaticText;
			labelSlopeParameter.Dock = DockStyle.Fill;
			labelSlopeParameter.Location = new Point(4, 274);
			labelSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			labelSlopeParameter.Name = "labelSlopeParameter";
			labelSlopeParameter.Size = new Size(238, 25);
			labelSlopeParameter.TabIndex = 36;
			toolTip.SetToolTip(labelSlopeParameter, "Slope parameter, G");
			labelSlopeParameter.Values.Text = "Slope parameter, G";
			labelSlopeParameter.Enter += SetStatusBar_Enter;
			labelSlopeParameter.Leave += ClearStatusBar_Leave;
			labelSlopeParameter.MouseEnter += SetStatusBar_Enter;
			labelSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMeanDailyMotion
			// 
			labelMeanDailyMotion.AccessibleDescription = "Shows the minimum and maximum of mean daily motion";
			labelMeanDailyMotion.AccessibleName = "Mean daily motion";
			labelMeanDailyMotion.AccessibleRole = AccessibleRole.StaticText;
			labelMeanDailyMotion.Dock = DockStyle.Fill;
			labelMeanDailyMotion.Location = new Point(4, 181);
			labelMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			labelMeanDailyMotion.Name = "labelMeanDailyMotion";
			labelMeanDailyMotion.Size = new Size(238, 25);
			labelMeanDailyMotion.TabIndex = 24;
			toolTip.SetToolTip(labelMeanDailyMotion, "Mean daily motion");
			labelMeanDailyMotion.Values.Text = "Mean daily motion";
			labelMeanDailyMotion.Enter += SetStatusBar_Enter;
			labelMeanDailyMotion.Leave += ClearStatusBar_Leave;
			labelMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			labelMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelAbsoluteMagnitude
			// 
			labelAbsoluteMagnitude.AccessibleDescription = "Shows the minimum and maximum of absolute magnitude";
			labelAbsoluteMagnitude.AccessibleName = "Absolute magnitude";
			labelAbsoluteMagnitude.AccessibleRole = AccessibleRole.StaticText;
			labelAbsoluteMagnitude.Dock = DockStyle.Fill;
			labelAbsoluteMagnitude.Location = new Point(4, 243);
			labelAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			labelAbsoluteMagnitude.Name = "labelAbsoluteMagnitude";
			labelAbsoluteMagnitude.Size = new Size(238, 25);
			labelAbsoluteMagnitude.TabIndex = 32;
			toolTip.SetToolTip(labelAbsoluteMagnitude, "Absolute magnitude, H");
			labelAbsoluteMagnitude.Values.Text = "Absolute magnitude, H";
			labelAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			labelAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			labelAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			labelAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelSemiMajorAxis
			// 
			labelSemiMajorAxis.AccessibleDescription = "Shows the minimum and maximum of semi-major axis";
			labelSemiMajorAxis.AccessibleName = "Semi-major axis";
			labelSemiMajorAxis.AccessibleRole = AccessibleRole.StaticText;
			labelSemiMajorAxis.Dock = DockStyle.Fill;
			labelSemiMajorAxis.Location = new Point(4, 212);
			labelSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			labelSemiMajorAxis.Name = "labelSemiMajorAxis";
			labelSemiMajorAxis.Size = new Size(238, 25);
			labelSemiMajorAxis.TabIndex = 28;
			toolTip.SetToolTip(labelSemiMajorAxis, "Semi-major axis");
			labelSemiMajorAxis.Values.Text = "Semi-major axis";
			labelSemiMajorAxis.Enter += SetStatusBar_Enter;
			labelSemiMajorAxis.Leave += ClearStatusBar_Leave;
			labelSemiMajorAxis.MouseEnter += SetStatusBar_Enter;
			labelSemiMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelMeanAnomalyAtTheEpoch
			// 
			labelMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the minimum and maximum of mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.AccessibleName = "Mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.StaticText;
			labelMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
			labelMeanAnomalyAtTheEpoch.Location = new Point(4, 26);
			labelMeanAnomalyAtTheEpoch.Margin = new Padding(4, 3, 4, 3);
			labelMeanAnomalyAtTheEpoch.Name = "labelMeanAnomalyAtTheEpoch";
			labelMeanAnomalyAtTheEpoch.Size = new Size(238, 25);
			labelMeanAnomalyAtTheEpoch.TabIndex = 4;
			toolTip.SetToolTip(labelMeanAnomalyAtTheEpoch, "Mean anomaly at the epoch");
			labelMeanAnomalyAtTheEpoch.Values.Text = "Mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.Enter += SetStatusBar_Enter;
			labelMeanAnomalyAtTheEpoch.Leave += ClearStatusBar_Leave;
			labelMeanAnomalyAtTheEpoch.MouseEnter += SetStatusBar_Enter;
			labelMeanAnomalyAtTheEpoch.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetInclination
			// 
			buttonResetInclination.AccessibleDescription = "Resets the minimum and maximum of inclination to the ecliptic";
			buttonResetInclination.AccessibleName = "Reset the minimum and maximum of inclination to the ecliptic";
			buttonResetInclination.AccessibleRole = AccessibleRole.PushButton;
			buttonResetInclination.ButtonStyle = ButtonStyle.Form;
			buttonResetInclination.Dock = DockStyle.Fill;
			buttonResetInclination.Location = new Point(506, 119);
			buttonResetInclination.Margin = new Padding(4, 3, 4, 3);
			buttonResetInclination.Name = "buttonResetInclination";
			buttonResetInclination.Size = new Size(85, 25);
			buttonResetInclination.TabIndex = 19;
			toolTip.SetToolTip(buttonResetInclination, "Reset the minimum and maximum of inclination to the ecliptic");
			buttonResetInclination.ToolTipValues.EnableToolTips = true;
			buttonResetInclination.Values.DropDownArrowColor = Color.Empty;
			buttonResetInclination.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetInclination.Values.Text = "";
			buttonResetInclination.Click += ButtonResetInclination_Click;
			buttonResetInclination.Enter += SetStatusBar_Enter;
			buttonResetInclination.Leave += ClearStatusBar_Leave;
			buttonResetInclination.MouseEnter += SetStatusBar_Enter;
			buttonResetInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonResetOrbitalEccentricity
			// 
			buttonResetOrbitalEccentricity.AccessibleDescription = "Resets the minimum and maximum of orbital eccentricity";
			buttonResetOrbitalEccentricity.AccessibleName = "Reset the minimum and maximum of orbital eccentricity";
			buttonResetOrbitalEccentricity.AccessibleRole = AccessibleRole.PushButton;
			buttonResetOrbitalEccentricity.ButtonStyle = ButtonStyle.Form;
			buttonResetOrbitalEccentricity.Dock = DockStyle.Fill;
			buttonResetOrbitalEccentricity.Location = new Point(506, 150);
			buttonResetOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			buttonResetOrbitalEccentricity.Name = "buttonResetOrbitalEccentricity";
			buttonResetOrbitalEccentricity.Size = new Size(85, 25);
			buttonResetOrbitalEccentricity.TabIndex = 23;
			toolTip.SetToolTip(buttonResetOrbitalEccentricity, "Reset the minimum and maximum of orbital eccentricity");
			buttonResetOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
			buttonResetOrbitalEccentricity.Values.DropDownArrowColor = Color.Empty;
			buttonResetOrbitalEccentricity.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetOrbitalEccentricity.Values.Text = "";
			buttonResetOrbitalEccentricity.Click += ButtonResetOrbitalEccentricity_Click;
			buttonResetOrbitalEccentricity.Enter += SetStatusBar_Enter;
			buttonResetOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			buttonResetOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			buttonResetOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// labelHeaderElement
			// 
			labelHeaderElement.AccessibleDescription = "Shows the header of the orbital elements";
			labelHeaderElement.AccessibleName = "Header of the orbital elements";
			labelHeaderElement.AccessibleRole = AccessibleRole.StaticText;
			labelHeaderElement.Dock = DockStyle.Fill;
			labelHeaderElement.LabelStyle = LabelStyle.BoldPanel;
			labelHeaderElement.Location = new Point(4, 3);
			labelHeaderElement.Margin = new Padding(4, 3, 4, 3);
			labelHeaderElement.Name = "labelHeaderElement";
			labelHeaderElement.Size = new Size(238, 17);
			labelHeaderElement.TabIndex = 0;
			toolTip.SetToolTip(labelHeaderElement, "Header of the orbital elements");
			labelHeaderElement.Values.Text = "Element";
			labelHeaderElement.Enter += SetStatusBar_Enter;
			labelHeaderElement.Leave += ClearStatusBar_Leave;
			labelHeaderElement.MouseEnter += SetStatusBar_Enter;
			labelHeaderElement.MouseLeave += ClearStatusBar_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// FilterForm
			// 
			AccessibleDescription = "Shows the filter";
			AccessibleName = "Filter";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(607, 477);
			ControlBox = false;
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FilterForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Filter";
			toolTip.SetToolTip(this, "Filter");
			FormClosed += FilterForm_FormClosed;
			Load += FilterForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			ResumeLayout(false);

		}

		#endregion
		private KryptonPanel panel;
		private KryptonLabel labelMeanAnomalyAtTheEpoch;
		private KryptonLabel labelArgumentOfPerihelion;
		private KryptonLabel labelNumberOfOppositions;
		private KryptonLabel labelSlopeParameter;
		private KryptonLabel labelAbsoluteMagnitude;
		private KryptonLabel labelSemiMajorAxis;
		private KryptonLabel labelMeanDailyMotion;
		private KryptonLabel labelOrbitalEccentricity;
		private KryptonLabel labelInclination;
		private KryptonLabel labelLongitudeOfTheAscendingNode;
		private KryptonLabel labelRmsResidual;
		private KryptonLabel labelNumberOfObservations;
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonNumericUpDown numericUpDownMinimumMeanAnomalyAtTheEpoch;
		private KryptonNumericUpDown numericUpDownMinimumArgumentOfPerihelion;
		private KryptonNumericUpDown numericUpDownMinimumInclination;
		private KryptonNumericUpDown numericUpDownMinimumLongitudeOfTheAscendingNode;
		private KryptonNumericUpDown numericUpDownMaximumAbsoluteMagnitude;
		private KryptonNumericUpDown numericUpDownMaximumSemiMajorAxis;
		private KryptonNumericUpDown numericUpDownMaximumMeanDailyMotion;
		private KryptonNumericUpDown numericUpDownMaximumOrbitalEccentricity;
		private KryptonNumericUpDown numericUpDownMaximumInclination;
		private KryptonNumericUpDown numericUpDownMaximumLongitudeOfTheAscendingNode;
		private KryptonNumericUpDown numericUpDownMaximumArgumentOfPerihelion;
		private KryptonNumericUpDown numericUpDownMaximumMeanAnomalyAtTheEpoch;
		private KryptonNumericUpDown numericUpDownMinimumRmsResidual;
		private KryptonNumericUpDown numericUpDownMinimumNumberOfObservations;
		private KryptonNumericUpDown numericUpDownMinimumNumberOfOppositions;
		private KryptonNumericUpDown numericUpDownMinimumSlopeParameter;
		private KryptonNumericUpDown numericUpDownMinimumAbsoluteMagnitude;
		private KryptonNumericUpDown numericUpDownMinimumSemiMajorAxis;
		private KryptonNumericUpDown numericUpDownMinimumMeanDailyMotion;
		private KryptonNumericUpDown numericUpDownMinimumOrbitalEccentricity;
		private KryptonNumericUpDown numericUpDownMaximumRmsResidual;
		private KryptonNumericUpDown numericUpDownMaximumNumberOfObservations;
		private KryptonNumericUpDown numericUpDownMaximumNumberOfOppositions;
		private KryptonNumericUpDown numericUpDownMaximumSlopeParameter;
		private KryptonButton buttonResetMeanAnomalyAtTheEpoch;
		private KryptonButton buttonResetRmsResidual;
		private KryptonButton buttonResetNumberOfObservations;
		private KryptonButton buttonNumberOfOppositions;
		private KryptonButton buttonResetSlopeParameter;
		private KryptonButton buttonResetAbsoluteMagnitude;
		private KryptonButton buttonResetSemiMajorAxis;
		private KryptonButton buttonResetMeanDailyMotion;
		private KryptonButton buttonResetLongitudeOfTheAscendingNode;
		private KryptonButton buttonResetArgumentOfPerihelion;
		private KryptonButton buttonResetInclination;
		private KryptonButton buttonResetOrbitalEccentricity;
		private KryptonLabel labelHeaderReset;
		private KryptonLabel labelHeaderMaximum;
		private KryptonLabel labelHeaderMinimum;
		private KryptonLabel labelHeaderElement;
		private KryptonButton buttonApply;
		private KryptonButton buttonReset;
		private KryptonButton buttonCancel;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolTip toolTip;
		private KryptonManager kryptonManager;
	}
}