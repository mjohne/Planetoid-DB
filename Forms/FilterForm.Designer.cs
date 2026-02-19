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
			numericUpDownMaximumArgumentOfThePerihelion = new KryptonNumericUpDown();
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
			numericUpDownMinimumArgumentOfThePerihelion = new KryptonNumericUpDown();
			labelArgumentOfThePerihelion = new KryptonLabel();
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
			buttonReset.ToolTipValues.Description = "Resets the filter settings";
			buttonReset.ToolTipValues.EnableToolTips = true;
			buttonReset.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonReset.ToolTipValues.Heading = "Reset all";
			buttonReset.Values.DropDownArrowColor = Color.Empty;
			buttonReset.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonReset.Values.Text = "&Reset all";
			buttonReset.Click += ButtonReset_Click;
			buttonReset.Enter += Control_Enter;
			buttonReset.Leave += Control_Leave;
			buttonReset.MouseEnter += Control_Enter;
			buttonReset.MouseLeave += Control_Leave;
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
			buttonCancel.ToolTipValues.Description = "Cancels the filter settings";
			buttonCancel.ToolTipValues.EnableToolTips = true;
			buttonCancel.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonCancel.ToolTipValues.Heading = "Cancel";
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += Control_Enter;
			buttonCancel.Leave += Control_Leave;
			buttonCancel.MouseEnter += Control_Enter;
			buttonCancel.MouseLeave += Control_Leave;
			// 
			// buttonApply
			// 
			buttonApply.AccessibleDescription = "Applies the filter settings";
			buttonApply.AccessibleName = "Apply the filter settings";
			buttonApply.AccessibleRole = AccessibleRole.PushButton;
			buttonApply.DialogResult = DialogResult.OK;
			buttonApply.Location = new Point(112, 416);
			buttonApply.Margin = new Padding(4, 3, 4, 3);
			buttonApply.Name = "buttonApply";
			buttonApply.Size = new Size(105, 29);
			buttonApply.TabIndex = 1;
			buttonApply.ToolTipValues.Description = "Applies the filter settings";
			buttonApply.ToolTipValues.EnableToolTips = true;
			buttonApply.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonApply.ToolTipValues.Heading = "Apply";
			buttonApply.Values.DropDownArrowColor = Color.Empty;
			buttonApply.Values.Image = FatcowIcons16px.fatcow_accept_button_16px;
			buttonApply.Values.Text = "&Apply";
			buttonApply.Click += ButtonApply_Click;
			buttonApply.Enter += Control_Enter;
			buttonApply.Leave += Control_Leave;
			buttonApply.MouseEnter += Control_Enter;
			buttonApply.MouseLeave += Control_Leave;
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
			tableLayoutPanel.Controls.Add(numericUpDownMaximumArgumentOfThePerihelion, 2, 3);
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
			tableLayoutPanel.Controls.Add(numericUpDownMinimumArgumentOfThePerihelion, 1, 3);
			tableLayoutPanel.Controls.Add(labelArgumentOfThePerihelion, 0, 3);
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
			labelHeaderReset.ToolTipValues.Description = "Shows the header of the reset buttons";
			labelHeaderReset.ToolTipValues.EnableToolTips = true;
			labelHeaderReset.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelHeaderReset.ToolTipValues.Heading = "Header of the reset buttons";
			labelHeaderReset.Values.Text = "Reset";
			labelHeaderReset.Enter += Control_Enter;
			labelHeaderReset.Leave += Control_Leave;
			labelHeaderReset.MouseEnter += Control_Enter;
			labelHeaderReset.MouseLeave += Control_Leave;
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
			labelHeaderMaximum.ToolTipValues.Description = "Shows the header of the maximum spin buttons";
			labelHeaderMaximum.ToolTipValues.EnableToolTips = true;
			labelHeaderMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelHeaderMaximum.ToolTipValues.Heading = "Header of the maximum spin buttons";
			labelHeaderMaximum.Values.Text = "Maximum";
			labelHeaderMaximum.Enter += Control_Enter;
			labelHeaderMaximum.Leave += Control_Leave;
			labelHeaderMaximum.MouseEnter += Control_Enter;
			labelHeaderMaximum.MouseLeave += Control_Leave;
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
			labelHeaderMinimum.ToolTipValues.Description = "Shows the header of the minimum spin buttons";
			labelHeaderMinimum.ToolTipValues.EnableToolTips = true;
			labelHeaderMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelHeaderMinimum.ToolTipValues.Heading = "Header of the minimum spin buttons";
			labelHeaderMinimum.Values.Text = "Minimum";
			labelHeaderMinimum.Enter += Control_Enter;
			labelHeaderMinimum.Leave += Control_Leave;
			labelHeaderMinimum.MouseEnter += Control_Enter;
			labelHeaderMinimum.MouseLeave += Control_Leave;
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
			buttonResetRmsResidual.ToolTipValues.Description = "Resets the minimum and maximum of ";
			buttonResetRmsResidual.ToolTipValues.EnableToolTips = true;
			buttonResetRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetRmsResidual.ToolTipValues.Heading = "Reset the minimum and maximum of ";
			buttonResetRmsResidual.Values.DropDownArrowColor = Color.Empty;
			buttonResetRmsResidual.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetRmsResidual.Values.Text = "";
			buttonResetRmsResidual.Click += ButtonResetRmsResidual_Click;
			buttonResetRmsResidual.Enter += Control_Enter;
			buttonResetRmsResidual.Leave += Control_Leave;
			buttonResetRmsResidual.MouseEnter += Control_Enter;
			buttonResetRmsResidual.MouseLeave += Control_Leave;
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
			buttonResetNumberOfObservations.ToolTipValues.Description = "Resets the minimum and maximum of number of observations";
			buttonResetNumberOfObservations.ToolTipValues.EnableToolTips = true;
			buttonResetNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetNumberOfObservations.ToolTipValues.Heading = "Reset the minimum and maximum of number of observations";
			buttonResetNumberOfObservations.Values.DropDownArrowColor = Color.Empty;
			buttonResetNumberOfObservations.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetNumberOfObservations.Values.Text = "";
			buttonResetNumberOfObservations.Click += ButtonResetNumberOfObservations_Click;
			buttonResetNumberOfObservations.Enter += Control_Enter;
			buttonResetNumberOfObservations.Leave += Control_Leave;
			buttonResetNumberOfObservations.MouseEnter += Control_Enter;
			buttonResetNumberOfObservations.MouseLeave += Control_Leave;
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
			buttonNumberOfOppositions.ToolTipValues.Description = "Resets the minimum and maximum of number of oppositions";
			buttonNumberOfOppositions.ToolTipValues.EnableToolTips = true;
			buttonNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonNumberOfOppositions.ToolTipValues.Heading = "Reset the minimum and maximum of number of oppositions";
			buttonNumberOfOppositions.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfOppositions.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonNumberOfOppositions.Values.Text = "";
			buttonNumberOfOppositions.Click += ButtonNumberOfOppositions_Click;
			buttonNumberOfOppositions.Enter += Control_Enter;
			buttonNumberOfOppositions.Leave += Control_Leave;
			buttonNumberOfOppositions.MouseEnter += Control_Enter;
			buttonNumberOfOppositions.MouseLeave += Control_Leave;
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
			buttonResetSlopeParameter.ToolTipValues.Description = "Resets the minimum and maximum of slope parameter";
			buttonResetSlopeParameter.ToolTipValues.EnableToolTips = true;
			buttonResetSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetSlopeParameter.ToolTipValues.Heading = "Reset the minimum and maximum of slope parameter";
			buttonResetSlopeParameter.Values.DropDownArrowColor = Color.Empty;
			buttonResetSlopeParameter.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetSlopeParameter.Values.Text = "";
			buttonResetSlopeParameter.Click += ButtonResetSlopeParameter_Click;
			buttonResetSlopeParameter.Enter += Control_Enter;
			buttonResetSlopeParameter.Leave += Control_Leave;
			buttonResetSlopeParameter.MouseEnter += Control_Enter;
			buttonResetSlopeParameter.MouseLeave += Control_Leave;
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
			buttonResetAbsoluteMagnitude.ToolTipValues.Description = "Resets the minimum and maximum of absolute magnitude";
			buttonResetAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
			buttonResetAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetAbsoluteMagnitude.ToolTipValues.Heading = "Reset the minimum and maximum of absolute magnitude";
			buttonResetAbsoluteMagnitude.Values.DropDownArrowColor = Color.Empty;
			buttonResetAbsoluteMagnitude.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetAbsoluteMagnitude.Values.Text = "";
			buttonResetAbsoluteMagnitude.Click += ButtonResetAbsoluteMagnitude_Click;
			buttonResetAbsoluteMagnitude.Enter += Control_Enter;
			buttonResetAbsoluteMagnitude.Leave += Control_Leave;
			buttonResetAbsoluteMagnitude.MouseEnter += Control_Enter;
			buttonResetAbsoluteMagnitude.MouseLeave += Control_Leave;
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
			buttonResetSemiMajorAxis.ToolTipValues.Description = "Resets the minimum and maximum of semi-major axis";
			buttonResetSemiMajorAxis.ToolTipValues.EnableToolTips = true;
			buttonResetSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetSemiMajorAxis.ToolTipValues.Heading = "Reset the minimum and maximum of semi-major axis";
			buttonResetSemiMajorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonResetSemiMajorAxis.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetSemiMajorAxis.Values.Text = "";
			buttonResetSemiMajorAxis.Click += ButtonResetSemiMajorAxis_Click;
			buttonResetSemiMajorAxis.Enter += Control_Enter;
			buttonResetSemiMajorAxis.Leave += Control_Leave;
			buttonResetSemiMajorAxis.MouseEnter += Control_Enter;
			buttonResetSemiMajorAxis.MouseLeave += Control_Leave;
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
			buttonResetMeanDailyMotion.ToolTipValues.Description = "Resets the minimum and maximum of mean daily motion";
			buttonResetMeanDailyMotion.ToolTipValues.EnableToolTips = true;
			buttonResetMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetMeanDailyMotion.ToolTipValues.Heading = "Reset the minimum and maximum of mean daily motion";
			buttonResetMeanDailyMotion.Values.DropDownArrowColor = Color.Empty;
			buttonResetMeanDailyMotion.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetMeanDailyMotion.Values.Text = "";
			buttonResetMeanDailyMotion.Click += ButtonResetMeanDailyMotion_Click;
			buttonResetMeanDailyMotion.Enter += Control_Enter;
			buttonResetMeanDailyMotion.Leave += Control_Leave;
			buttonResetMeanDailyMotion.MouseEnter += Control_Enter;
			buttonResetMeanDailyMotion.MouseLeave += Control_Leave;
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
			buttonResetLongitudeOfTheAscendingNode.ToolTipValues.Description = "Resets the minimum and maximum of longitude of the ascending node";
			buttonResetLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
			buttonResetLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Reset the minimum and maximum of longitude of the ascending node";
			buttonResetLongitudeOfTheAscendingNode.Values.DropDownArrowColor = Color.Empty;
			buttonResetLongitudeOfTheAscendingNode.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetLongitudeOfTheAscendingNode.Values.Text = "";
			buttonResetLongitudeOfTheAscendingNode.Click += ButtonResetLongitudeOfTheAscendingNode_Click;
			buttonResetLongitudeOfTheAscendingNode.Enter += Control_Enter;
			buttonResetLongitudeOfTheAscendingNode.Leave += Control_Leave;
			buttonResetLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
			buttonResetLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
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
			buttonResetArgumentOfPerihelion.ToolTipValues.Description = "Resets the minimum and maximum of argument of perihelion";
			buttonResetArgumentOfPerihelion.ToolTipValues.EnableToolTips = true;
			buttonResetArgumentOfPerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetArgumentOfPerihelion.ToolTipValues.Heading = "Reset the minimum and maximum of argument of perihelion";
			buttonResetArgumentOfPerihelion.Values.DropDownArrowColor = Color.Empty;
			buttonResetArgumentOfPerihelion.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetArgumentOfPerihelion.Values.Text = "";
			buttonResetArgumentOfPerihelion.Click += ButtonResetArgumentOfPerihelion_Click;
			buttonResetArgumentOfPerihelion.Enter += Control_Enter;
			buttonResetArgumentOfPerihelion.Leave += Control_Leave;
			buttonResetArgumentOfPerihelion.MouseEnter += Control_Enter;
			buttonResetArgumentOfPerihelion.MouseLeave += Control_Leave;
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
			buttonResetMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Resets the minimum and maximum of mean anomaly at the epoch";
			buttonResetMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
			buttonResetMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Reset the minimum and maximum of mean anomaly at the epoch";
			buttonResetMeanAnomalyAtTheEpoch.Values.DropDownArrowColor = Color.Empty;
			buttonResetMeanAnomalyAtTheEpoch.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetMeanAnomalyAtTheEpoch.Values.Text = "";
			buttonResetMeanAnomalyAtTheEpoch.Click += ButtonResetMeanAnomalyAtTheEpoch_Click;
			buttonResetMeanAnomalyAtTheEpoch.Enter += Control_Enter;
			buttonResetMeanAnomalyAtTheEpoch.Leave += Control_Leave;
			buttonResetMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
			buttonResetMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
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
			numericUpDownMaximumRmsResidual.ToolTipValues.Description = "Shows the maximum of r.m.s. residual";
			numericUpDownMaximumRmsResidual.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumRmsResidual.ToolTipValues.Heading = "Maximum of r.m.s. residual";
			numericUpDownMaximumRmsResidual.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumRmsResidual.ValueChanged += NumericUpDownMaximumRmsResidual_ValueChanged;
			numericUpDownMaximumRmsResidual.Enter += Control_Enter;
			numericUpDownMaximumRmsResidual.Leave += Control_Leave;
			numericUpDownMaximumRmsResidual.MouseEnter += Control_Enter;
			numericUpDownMaximumRmsResidual.MouseLeave += Control_Leave;
			// 
			// numericUpDownMaximumNumberOfObservations
			// 
			numericUpDownMaximumNumberOfObservations.AccessibleDescription = "Shows the maximum of number of observations ";
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
			numericUpDownMaximumNumberOfObservations.ToolTipValues.Description = "Shows the maximum of number of observations ";
			numericUpDownMaximumNumberOfObservations.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumNumberOfObservations.ToolTipValues.Heading = "Maximum of number of observations";
			numericUpDownMaximumNumberOfObservations.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfObservations.ValueChanged += NumericUpDownMaximumNumberOfObservations_ValueChanged;
			numericUpDownMaximumNumberOfObservations.Enter += Control_Enter;
			numericUpDownMaximumNumberOfObservations.Leave += Control_Leave;
			numericUpDownMaximumNumberOfObservations.MouseEnter += Control_Enter;
			numericUpDownMaximumNumberOfObservations.MouseLeave += Control_Leave;
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
			numericUpDownMaximumNumberOfOppositions.ToolTipValues.Description = "Shows the maximum of number of oppositions";
			numericUpDownMaximumNumberOfOppositions.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumNumberOfOppositions.ToolTipValues.Heading = "Maximum of number of oppositions";
			numericUpDownMaximumNumberOfOppositions.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumNumberOfOppositions.ValueChanged += NumericUpDownMaximumNumberOfOppositions_ValueChanged;
			numericUpDownMaximumNumberOfOppositions.Enter += Control_Enter;
			numericUpDownMaximumNumberOfOppositions.Leave += Control_Leave;
			numericUpDownMaximumNumberOfOppositions.MouseEnter += Control_Enter;
			numericUpDownMaximumNumberOfOppositions.MouseLeave += Control_Leave;
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
			numericUpDownMaximumSlopeParameter.ToolTipValues.Description = "Shows the maximum of slope parameter";
			numericUpDownMaximumSlopeParameter.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumSlopeParameter.ToolTipValues.Heading = "Maximum of slope parameter";
			numericUpDownMaximumSlopeParameter.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSlopeParameter.ValueChanged += NumericUpDownMaximumSlopeParameter_ValueChanged;
			numericUpDownMaximumSlopeParameter.Enter += Control_Enter;
			numericUpDownMaximumSlopeParameter.Leave += Control_Leave;
			numericUpDownMaximumSlopeParameter.MouseEnter += Control_Enter;
			numericUpDownMaximumSlopeParameter.MouseLeave += Control_Leave;
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
			numericUpDownMaximumAbsoluteMagnitude.ToolTipValues.Description = "Shows the maximum of absolute magnitude";
			numericUpDownMaximumAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumAbsoluteMagnitude.ToolTipValues.Heading = "Maximum of absolute magnitude";
			numericUpDownMaximumAbsoluteMagnitude.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumAbsoluteMagnitude.ValueChanged += NumericUpDownMaximumAbsoluteMagnitude_ValueChanged;
			numericUpDownMaximumAbsoluteMagnitude.Enter += Control_Enter;
			numericUpDownMaximumAbsoluteMagnitude.Leave += Control_Leave;
			numericUpDownMaximumAbsoluteMagnitude.MouseEnter += Control_Enter;
			numericUpDownMaximumAbsoluteMagnitude.MouseLeave += Control_Leave;
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
			numericUpDownMaximumSemiMajorAxis.ToolTipValues.Description = "Shows the maximum of semi-major axis";
			numericUpDownMaximumSemiMajorAxis.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumSemiMajorAxis.ToolTipValues.Heading = "Maximum of semi-major axis";
			numericUpDownMaximumSemiMajorAxis.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumSemiMajorAxis.ValueChanged += NumericUpDownMaximumSemiMajorAxis_ValueChanged;
			numericUpDownMaximumSemiMajorAxis.Enter += Control_Enter;
			numericUpDownMaximumSemiMajorAxis.Leave += Control_Leave;
			numericUpDownMaximumSemiMajorAxis.MouseEnter += Control_Enter;
			numericUpDownMaximumSemiMajorAxis.MouseLeave += Control_Leave;
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
			numericUpDownMaximumMeanDailyMotion.ToolTipValues.Description = "Shows the maximum of mean daily motion";
			numericUpDownMaximumMeanDailyMotion.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumMeanDailyMotion.ToolTipValues.Heading = "Maximum of mean daily motion";
			numericUpDownMaximumMeanDailyMotion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanDailyMotion.ValueChanged += NumericUpDownMaximumMeanDailyMotion_ValueChanged;
			numericUpDownMaximumMeanDailyMotion.Enter += Control_Enter;
			numericUpDownMaximumMeanDailyMotion.Leave += Control_Leave;
			numericUpDownMaximumMeanDailyMotion.MouseEnter += Control_Enter;
			numericUpDownMaximumMeanDailyMotion.MouseLeave += Control_Leave;
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
			numericUpDownMaximumOrbitalEccentricity.ToolTipValues.Description = "Shows the maximum of orbital eccentricity";
			numericUpDownMaximumOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumOrbitalEccentricity.ToolTipValues.Heading = "Maximum of orbital eccentricity";
			numericUpDownMaximumOrbitalEccentricity.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumOrbitalEccentricity.ValueChanged += NumericUpDownMaximumOrbitalEccentricity_ValueChanged;
			numericUpDownMaximumOrbitalEccentricity.Enter += Control_Enter;
			numericUpDownMaximumOrbitalEccentricity.Leave += Control_Leave;
			numericUpDownMaximumOrbitalEccentricity.MouseEnter += Control_Enter;
			numericUpDownMaximumOrbitalEccentricity.MouseLeave += Control_Leave;
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
			numericUpDownMaximumInclination.ToolTipValues.Description = "Shows the maximum of inclination to the ecliptic";
			numericUpDownMaximumInclination.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumInclination.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumInclination.ToolTipValues.Heading = "Maximum of inclination to the ecliptic";
			numericUpDownMaximumInclination.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumInclination.ValueChanged += NumericUpDownMaximumInclination_ValueChanged;
			numericUpDownMaximumInclination.Enter += Control_Enter;
			numericUpDownMaximumInclination.Leave += Control_Leave;
			numericUpDownMaximumInclination.MouseEnter += Control_Enter;
			numericUpDownMaximumInclination.MouseLeave += Control_Leave;
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
			numericUpDownMaximumLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the maximum of longitude of the ascending node";
			numericUpDownMaximumLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Maximum of longitude of the ascending node";
			numericUpDownMaximumLongitudeOfTheAscendingNode.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumLongitudeOfTheAscendingNode.ValueChanged += NumericUpDownMaximumLongitudeOfTheAscendingNode_ValueChanged;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Enter += Control_Enter;
			numericUpDownMaximumLongitudeOfTheAscendingNode.Leave += Control_Leave;
			numericUpDownMaximumLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
			numericUpDownMaximumLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
			// 
			// numericUpDownMaximumArgumentOfThePerihelion
			// 
			numericUpDownMaximumArgumentOfThePerihelion.AccessibleDescription = "Shows the maximum of argument of the perihelion";
			numericUpDownMaximumArgumentOfThePerihelion.AccessibleName = "Maximum of argument of the perihelion";
			numericUpDownMaximumArgumentOfThePerihelion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMaximumArgumentOfThePerihelion.AllowDecimals = true;
			numericUpDownMaximumArgumentOfThePerihelion.DecimalPlaces = 8;
			numericUpDownMaximumArgumentOfThePerihelion.Dock = DockStyle.Fill;
			numericUpDownMaximumArgumentOfThePerihelion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMaximumArgumentOfThePerihelion.Location = new Point(378, 57);
			numericUpDownMaximumArgumentOfThePerihelion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMaximumArgumentOfThePerihelion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMaximumArgumentOfThePerihelion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumArgumentOfThePerihelion.Name = "numericUpDownMaximumArgumentOfThePerihelion";
			numericUpDownMaximumArgumentOfThePerihelion.Size = new Size(120, 22);
			numericUpDownMaximumArgumentOfThePerihelion.TabIndex = 10;
			numericUpDownMaximumArgumentOfThePerihelion.ThousandsSeparator = true;
			numericUpDownMaximumArgumentOfThePerihelion.ToolTipValues.Description = "Shows the maximum of argument of the perihelion";
			numericUpDownMaximumArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumArgumentOfThePerihelion.ToolTipValues.Heading = "Maximum of argument of the perihelion";
			numericUpDownMaximumArgumentOfThePerihelion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumArgumentOfThePerihelion.ValueChanged += NumericUpDownMaximumArgumentOfPerihelion_ValueChanged;
			numericUpDownMaximumArgumentOfThePerihelion.Enter += Control_Enter;
			numericUpDownMaximumArgumentOfThePerihelion.Leave += Control_Leave;
			numericUpDownMaximumArgumentOfThePerihelion.MouseEnter += Control_Enter;
			numericUpDownMaximumArgumentOfThePerihelion.MouseLeave += Control_Leave;
			// 
			// numericUpDownMaximumMeanAnomalyAtTheEpoch
			// 
			numericUpDownMaximumMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the maximum of mean anomaly at the epoch";
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
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the maximum of mean anomaly at the epoch";
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Maximum of mean anomaly at the epoch";
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMaximumMeanAnomalyAtTheEpoch.ValueChanged += NumericUpDownMaximumMeanAnomalyAtTheEpoch_ValueChanged;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Enter += Control_Enter;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.Leave += Control_Leave;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
			numericUpDownMaximumMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
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
			numericUpDownMinimumRmsResidual.ToolTipValues.Description = "Shows the minimum of r.m.s. residual";
			numericUpDownMinimumRmsResidual.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumRmsResidual.ToolTipValues.Heading = "Minimum of r.m.s. residual";
			numericUpDownMinimumRmsResidual.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumRmsResidual.ValueChanged += NumericUpDownMinimumRmsResidual_ValueChanged;
			numericUpDownMinimumRmsResidual.Enter += Control_Enter;
			numericUpDownMinimumRmsResidual.Leave += Control_Leave;
			numericUpDownMinimumRmsResidual.MouseEnter += Control_Enter;
			numericUpDownMinimumRmsResidual.MouseLeave += Control_Leave;
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
			numericUpDownMinimumNumberOfObservations.ToolTipValues.Description = "Shows the minimum of number of observations";
			numericUpDownMinimumNumberOfObservations.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumNumberOfObservations.ToolTipValues.Heading = "Minimum of number of observations";
			numericUpDownMinimumNumberOfObservations.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfObservations.ValueChanged += NumericUpDownMinimumNumberOfObservations_ValueChanged;
			numericUpDownMinimumNumberOfObservations.Enter += Control_Enter;
			numericUpDownMinimumNumberOfObservations.Leave += Control_Leave;
			numericUpDownMinimumNumberOfObservations.MouseEnter += Control_Enter;
			numericUpDownMinimumNumberOfObservations.MouseLeave += Control_Leave;
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
			numericUpDownMinimumNumberOfOppositions.ToolTipValues.Description = "Shows the minimum of number of oppositions";
			numericUpDownMinimumNumberOfOppositions.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumNumberOfOppositions.ToolTipValues.Heading = "Minimum of number of oppositions";
			numericUpDownMinimumNumberOfOppositions.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumNumberOfOppositions.ValueChanged += NumericUpDownMinimumNumberOfOppositions_ValueChanged;
			numericUpDownMinimumNumberOfOppositions.Enter += Control_Enter;
			numericUpDownMinimumNumberOfOppositions.Leave += Control_Leave;
			numericUpDownMinimumNumberOfOppositions.MouseEnter += Control_Enter;
			numericUpDownMinimumNumberOfOppositions.MouseLeave += Control_Leave;
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
			numericUpDownMinimumSlopeParameter.ToolTipValues.Description = "Shows the minimum of slope parameter";
			numericUpDownMinimumSlopeParameter.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumSlopeParameter.ToolTipValues.Heading = "Minimum of slope parameter";
			numericUpDownMinimumSlopeParameter.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSlopeParameter.ValueChanged += NumericUpDownMinimumSlopeParameter_ValueChanged;
			numericUpDownMinimumSlopeParameter.Enter += Control_Enter;
			numericUpDownMinimumSlopeParameter.Leave += Control_Leave;
			numericUpDownMinimumSlopeParameter.MouseEnter += Control_Enter;
			numericUpDownMinimumSlopeParameter.MouseLeave += Control_Leave;
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
			numericUpDownMinimumAbsoluteMagnitude.ToolTipValues.Description = "Shows the minimum of absolute magnitude";
			numericUpDownMinimumAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumAbsoluteMagnitude.ToolTipValues.Heading = "Minimum of absolute magnitude";
			numericUpDownMinimumAbsoluteMagnitude.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumAbsoluteMagnitude.ValueChanged += NumericUpDownMinimumAbsoluteMagnitude_ValueChanged;
			numericUpDownMinimumAbsoluteMagnitude.Enter += Control_Enter;
			numericUpDownMinimumAbsoluteMagnitude.Leave += Control_Leave;
			numericUpDownMinimumAbsoluteMagnitude.MouseEnter += Control_Enter;
			numericUpDownMinimumAbsoluteMagnitude.MouseLeave += Control_Leave;
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
			numericUpDownMinimumSemiMajorAxis.ToolTipValues.Description = "Shows the minimum of semi-major axis";
			numericUpDownMinimumSemiMajorAxis.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumSemiMajorAxis.ToolTipValues.Heading = "Minimum of semi-major axis";
			numericUpDownMinimumSemiMajorAxis.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumSemiMajorAxis.ValueChanged += NumericUpDownMinimumSemiMajorAxis_ValueChanged;
			numericUpDownMinimumSemiMajorAxis.Enter += Control_Enter;
			numericUpDownMinimumSemiMajorAxis.Leave += Control_Leave;
			numericUpDownMinimumSemiMajorAxis.MouseEnter += Control_Enter;
			numericUpDownMinimumSemiMajorAxis.MouseLeave += Control_Leave;
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
			numericUpDownMinimumMeanDailyMotion.ToolTipValues.Description = "Shows the minimum of mean daily motion";
			numericUpDownMinimumMeanDailyMotion.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumMeanDailyMotion.ToolTipValues.Heading = "Minimum of mean daily motion";
			numericUpDownMinimumMeanDailyMotion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanDailyMotion.ValueChanged += NumericUpDownMinimumMeanDailyMotion_ValueChanged;
			numericUpDownMinimumMeanDailyMotion.Enter += Control_Enter;
			numericUpDownMinimumMeanDailyMotion.Leave += Control_Leave;
			numericUpDownMinimumMeanDailyMotion.MouseEnter += Control_Enter;
			numericUpDownMinimumMeanDailyMotion.MouseLeave += Control_Leave;
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
			numericUpDownMinimumOrbitalEccentricity.ToolTipValues.Description = "Shows the minimum of orbital eccentricity";
			numericUpDownMinimumOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumOrbitalEccentricity.ToolTipValues.Heading = "Minimum of orbital eccentricity";
			numericUpDownMinimumOrbitalEccentricity.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumOrbitalEccentricity.ValueChanged += NumericUpDownMinimumOrbitalEccentricity_ValueChanged;
			numericUpDownMinimumOrbitalEccentricity.Enter += Control_Enter;
			numericUpDownMinimumOrbitalEccentricity.Leave += Control_Leave;
			numericUpDownMinimumOrbitalEccentricity.MouseEnter += Control_Enter;
			numericUpDownMinimumOrbitalEccentricity.MouseLeave += Control_Leave;
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
			numericUpDownMinimumInclination.ToolTipValues.Description = "Shows the minimum of inclination to the ecliptic";
			numericUpDownMinimumInclination.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumInclination.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumInclination.ToolTipValues.Heading = "Minimum of inclination to the ecliptic";
			numericUpDownMinimumInclination.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumInclination.ValueChanged += NumericUpDownMinimumInclination_ValueChanged;
			numericUpDownMinimumInclination.Enter += Control_Enter;
			numericUpDownMinimumInclination.Leave += Control_Leave;
			numericUpDownMinimumInclination.MouseEnter += Control_Enter;
			numericUpDownMinimumInclination.MouseLeave += Control_Leave;
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
			numericUpDownMinimumLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the minimum of longitude of the ascending node";
			numericUpDownMinimumLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Minimum of longitude of the ascending node";
			numericUpDownMinimumLongitudeOfTheAscendingNode.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumLongitudeOfTheAscendingNode.ValueChanged += NumericUpDownMinimumLongitudeOfTheAscendingNode_ValueChanged;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Enter += Control_Enter;
			numericUpDownMinimumLongitudeOfTheAscendingNode.Leave += Control_Leave;
			numericUpDownMinimumLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
			numericUpDownMinimumLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
			// 
			// numericUpDownMinimumArgumentOfThePerihelion
			// 
			numericUpDownMinimumArgumentOfThePerihelion.AccessibleDescription = "Shows the minimum of argument of the perihelion";
			numericUpDownMinimumArgumentOfThePerihelion.AccessibleName = "Minimum of argument of the perihelion";
			numericUpDownMinimumArgumentOfThePerihelion.AccessibleRole = AccessibleRole.SpinButton;
			numericUpDownMinimumArgumentOfThePerihelion.AllowDecimals = true;
			numericUpDownMinimumArgumentOfThePerihelion.DecimalPlaces = 8;
			numericUpDownMinimumArgumentOfThePerihelion.Dock = DockStyle.Fill;
			numericUpDownMinimumArgumentOfThePerihelion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
			numericUpDownMinimumArgumentOfThePerihelion.Location = new Point(250, 57);
			numericUpDownMinimumArgumentOfThePerihelion.Margin = new Padding(4, 3, 4, 3);
			numericUpDownMinimumArgumentOfThePerihelion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			numericUpDownMinimumArgumentOfThePerihelion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumArgumentOfThePerihelion.Name = "numericUpDownMinimumArgumentOfThePerihelion";
			numericUpDownMinimumArgumentOfThePerihelion.Size = new Size(120, 22);
			numericUpDownMinimumArgumentOfThePerihelion.TabIndex = 9;
			numericUpDownMinimumArgumentOfThePerihelion.ThousandsSeparator = true;
			numericUpDownMinimumArgumentOfThePerihelion.ToolTipValues.Description = "Shows the minimum of argument of the perihelion";
			numericUpDownMinimumArgumentOfThePerihelion.ToolTipValues.Heading = "Minimum of argument of the perihelion";
			numericUpDownMinimumArgumentOfThePerihelion.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumArgumentOfThePerihelion.ValueChanged += NumericUpDownMinimumArgumentOfPerihelion_ValueChanged;
			numericUpDownMinimumArgumentOfThePerihelion.Enter += Control_Enter;
			numericUpDownMinimumArgumentOfThePerihelion.Leave += Control_Leave;
			numericUpDownMinimumArgumentOfThePerihelion.MouseEnter += Control_Enter;
			numericUpDownMinimumArgumentOfThePerihelion.MouseLeave += Control_Leave;
			// 
			// labelArgumentOfThePerihelion
			// 
			labelArgumentOfThePerihelion.AccessibleDescription = "Shows the minimum and maximum of argument of the perihelion";
			labelArgumentOfThePerihelion.AccessibleName = "Argument of the perihelion";
			labelArgumentOfThePerihelion.AccessibleRole = AccessibleRole.StaticText;
			labelArgumentOfThePerihelion.Dock = DockStyle.Fill;
			labelArgumentOfThePerihelion.Location = new Point(4, 57);
			labelArgumentOfThePerihelion.Margin = new Padding(4, 3, 4, 3);
			labelArgumentOfThePerihelion.Name = "labelArgumentOfThePerihelion";
			labelArgumentOfThePerihelion.Size = new Size(238, 25);
			labelArgumentOfThePerihelion.TabIndex = 8;
			labelArgumentOfThePerihelion.ToolTipValues.Description = "Shows the minimum and maximum of argument of the perihelion";
			labelArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
			labelArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelArgumentOfThePerihelion.ToolTipValues.Heading = "Argument of the perihelion, J2000.0";
			labelArgumentOfThePerihelion.Values.Text = "Argument of the perihelion, J2000.0";
			labelArgumentOfThePerihelion.Enter += Control_Enter;
			labelArgumentOfThePerihelion.Leave += Control_Leave;
			labelArgumentOfThePerihelion.MouseEnter += Control_Enter;
			labelArgumentOfThePerihelion.MouseLeave += Control_Leave;
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
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the minimum of mean anomaly at the epoch";
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Minimum of mean anomaly at the epoch";
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Value = new decimal(new int[] { 0, 0, 0, 0 });
			numericUpDownMinimumMeanAnomalyAtTheEpoch.ValueChanged += NumericUpDownMinimumMeanAnomalyAtTheEpoch_ValueChanged;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Enter += Control_Enter;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.Leave += Control_Leave;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
			numericUpDownMinimumMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
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
			labelRmsResidual.ToolTipValues.Description = "Shows the minimum and maximum of r.m.s. residual";
			labelRmsResidual.ToolTipValues.EnableToolTips = true;
			labelRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelRmsResidual.ToolTipValues.Heading = "r.m.s. residual";
			labelRmsResidual.Values.Text = "r.m.s. residual";
			labelRmsResidual.Enter += Control_Enter;
			labelRmsResidual.Leave += Control_Leave;
			labelRmsResidual.MouseEnter += Control_Enter;
			labelRmsResidual.MouseLeave += Control_Leave;
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
			labelLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the minimum and maximum of longitude of the ascending node";
			labelLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
			labelLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Longitude of the ascending node, J2000.0";
			labelLongitudeOfTheAscendingNode.Values.Text = "Longitude of the ascending node, J2000.0";
			labelLongitudeOfTheAscendingNode.Enter += Control_Enter;
			labelLongitudeOfTheAscendingNode.Leave += Control_Leave;
			labelLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
			labelLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
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
			labelNumberOfObservations.ToolTipValues.Description = "Shows the minimum and maximum of number of observations";
			labelNumberOfObservations.ToolTipValues.EnableToolTips = true;
			labelNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelNumberOfObservations.ToolTipValues.Heading = "Number of observations";
			labelNumberOfObservations.Values.Text = "Number of observations";
			labelNumberOfObservations.Enter += Control_Enter;
			labelNumberOfObservations.Leave += Control_Leave;
			labelNumberOfObservations.MouseEnter += Control_Enter;
			labelNumberOfObservations.MouseLeave += Control_Leave;
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
			labelInclination.ToolTipValues.Description = "Shows the minimum and maximum of inclination to the ecliptic";
			labelInclination.ToolTipValues.EnableToolTips = true;
			labelInclination.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelInclination.ToolTipValues.Heading = "Inclination to the ecliptic, J2000.0";
			labelInclination.Values.Text = "Inclination to the ecliptic, J2000.0";
			labelInclination.Enter += Control_Enter;
			labelInclination.Leave += Control_Leave;
			labelInclination.MouseEnter += Control_Enter;
			labelInclination.MouseLeave += Control_Leave;
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
			labelNumberOfOppositions.ToolTipValues.Description = "Shows the minimum and maximum of number of oppositions";
			labelNumberOfOppositions.ToolTipValues.EnableToolTips = true;
			labelNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelNumberOfOppositions.ToolTipValues.Heading = "Number of oppositions";
			labelNumberOfOppositions.Values.Text = "Number of oppositions";
			labelNumberOfOppositions.Enter += Control_Enter;
			labelNumberOfOppositions.Leave += Control_Leave;
			labelNumberOfOppositions.MouseEnter += Control_Enter;
			labelNumberOfOppositions.MouseLeave += Control_Leave;
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
			labelOrbitalEccentricity.ToolTipValues.Description = "Shows the minimum and maximum of orbital eccentricity";
			labelOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
			labelOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelOrbitalEccentricity.ToolTipValues.Heading = "Orbital eccentricity";
			labelOrbitalEccentricity.Values.Text = "Orbital eccentricity";
			labelOrbitalEccentricity.Enter += Control_Enter;
			labelOrbitalEccentricity.Leave += Control_Leave;
			labelOrbitalEccentricity.MouseEnter += Control_Enter;
			labelOrbitalEccentricity.MouseLeave += Control_Leave;
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
			labelSlopeParameter.ToolTipValues.Description = "Shows the minimum and maximum of slope parameter";
			labelSlopeParameter.ToolTipValues.EnableToolTips = true;
			labelSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSlopeParameter.ToolTipValues.Heading = "Slope parameter, G";
			labelSlopeParameter.Values.Text = "Slope parameter, G";
			labelSlopeParameter.Enter += Control_Enter;
			labelSlopeParameter.Leave += Control_Leave;
			labelSlopeParameter.MouseEnter += Control_Enter;
			labelSlopeParameter.MouseLeave += Control_Leave;
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
			labelMeanDailyMotion.ToolTipValues.Description = "Shows the minimum and maximum of mean daily motion";
			labelMeanDailyMotion.ToolTipValues.EnableToolTips = true;
			labelMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMeanDailyMotion.ToolTipValues.Heading = "Mean daily motion";
			labelMeanDailyMotion.Values.Text = "Mean daily motion";
			labelMeanDailyMotion.Enter += Control_Enter;
			labelMeanDailyMotion.Leave += Control_Leave;
			labelMeanDailyMotion.MouseEnter += Control_Enter;
			labelMeanDailyMotion.MouseLeave += Control_Leave;
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
			labelAbsoluteMagnitude.ToolTipValues.Description = "Shows the minimum and maximum of absolute magnitude";
			labelAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
			labelAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelAbsoluteMagnitude.ToolTipValues.Heading = "Absolute magnitude, H";
			labelAbsoluteMagnitude.Values.Text = "Absolute magnitude, H";
			labelAbsoluteMagnitude.Enter += Control_Enter;
			labelAbsoluteMagnitude.Leave += Control_Leave;
			labelAbsoluteMagnitude.MouseEnter += Control_Enter;
			labelAbsoluteMagnitude.MouseLeave += Control_Leave;
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
			labelSemiMajorAxis.ToolTipValues.Description = "Shows the minimum and maximum of semi-major axis";
			labelSemiMajorAxis.ToolTipValues.EnableToolTips = true;
			labelSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelSemiMajorAxis.ToolTipValues.Heading = "Semi-major axis";
			labelSemiMajorAxis.Values.Text = "Semi-major axis";
			labelSemiMajorAxis.Enter += Control_Enter;
			labelSemiMajorAxis.Leave += Control_Leave;
			labelSemiMajorAxis.MouseEnter += Control_Enter;
			labelSemiMajorAxis.MouseLeave += Control_Leave;
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
			labelMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the minimum and maximum of mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
			labelMeanAnomalyAtTheEpoch.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.Values.Text = "Mean anomaly at the epoch";
			labelMeanAnomalyAtTheEpoch.Enter += Control_Enter;
			labelMeanAnomalyAtTheEpoch.Leave += Control_Leave;
			labelMeanAnomalyAtTheEpoch.MouseEnter += Control_Enter;
			labelMeanAnomalyAtTheEpoch.MouseLeave += Control_Leave;
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
			buttonResetInclination.ToolTipValues.Description = "Resets the minimum and maximum of inclination to the ecliptic";
			buttonResetInclination.ToolTipValues.EnableToolTips = true;
			buttonResetInclination.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetInclination.ToolTipValues.Heading = "Reset the minimum and maximum of inclination to the ecliptic";
			buttonResetInclination.Values.DropDownArrowColor = Color.Empty;
			buttonResetInclination.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetInclination.Values.Text = "";
			buttonResetInclination.Click += ButtonResetInclination_Click;
			buttonResetInclination.Enter += Control_Enter;
			buttonResetInclination.Leave += Control_Leave;
			buttonResetInclination.MouseEnter += Control_Enter;
			buttonResetInclination.MouseLeave += Control_Leave;
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
			buttonResetOrbitalEccentricity.ToolTipValues.Description = "Resets the minimum and maximum of orbital eccentricity";
			buttonResetOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
			buttonResetOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonResetOrbitalEccentricity.ToolTipValues.Heading = "Reset the minimum and maximum of orbital eccentricity";
			buttonResetOrbitalEccentricity.Values.DropDownArrowColor = Color.Empty;
			buttonResetOrbitalEccentricity.Values.Image = FatcowIcons16px.fatcow_update_16px;
			buttonResetOrbitalEccentricity.Values.Text = "";
			buttonResetOrbitalEccentricity.Click += ButtonResetOrbitalEccentricity_Click;
			buttonResetOrbitalEccentricity.Enter += Control_Enter;
			buttonResetOrbitalEccentricity.Leave += Control_Leave;
			buttonResetOrbitalEccentricity.MouseEnter += Control_Enter;
			buttonResetOrbitalEccentricity.MouseLeave += Control_Leave;
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
			labelHeaderElement.ToolTipValues.Description = "Shows the header of the orbital elements";
			labelHeaderElement.ToolTipValues.EnableToolTips = true;
			labelHeaderElement.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelHeaderElement.ToolTipValues.Heading = "Header of the orbital elements";
			labelHeaderElement.Values.Text = "Element";
			labelHeaderElement.Enter += Control_Enter;
			labelHeaderElement.Leave += Control_Leave;
			labelHeaderElement.MouseEnter += Control_Enter;
			labelHeaderElement.MouseLeave += Control_Leave;
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
		private KryptonLabel labelArgumentOfThePerihelion;
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
		private KryptonNumericUpDown numericUpDownMinimumArgumentOfThePerihelion;
		private KryptonNumericUpDown numericUpDownMinimumInclination;
		private KryptonNumericUpDown numericUpDownMinimumLongitudeOfTheAscendingNode;
		private KryptonNumericUpDown numericUpDownMaximumAbsoluteMagnitude;
		private KryptonNumericUpDown numericUpDownMaximumSemiMajorAxis;
		private KryptonNumericUpDown numericUpDownMaximumMeanDailyMotion;
		private KryptonNumericUpDown numericUpDownMaximumOrbitalEccentricity;
		private KryptonNumericUpDown numericUpDownMaximumInclination;
		private KryptonNumericUpDown numericUpDownMaximumLongitudeOfTheAscendingNode;
		private KryptonNumericUpDown numericUpDownMaximumArgumentOfThePerihelion;
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
		private KryptonManager kryptonManager;
	}
}