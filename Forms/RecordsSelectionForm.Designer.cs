// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>
/// Represents a dialog form that allows users to select, view, and copy information from the top ten records in a
/// database or dataset.
/// </summary>
/// <remarks>The form provides buttons for copying specific record fields to the clipboard and options to sort
/// records in ascending or descending order. It is intended for use as a modal dialog and does not appear in the
/// taskbar. The form is centered on its parent when opened and disables maximize/minimize controls to maintain a fixed
/// layout.</remarks>
partial class RecordsSelectionForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	/// <remarks>This method disposes of the resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify
	/// the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsSelectionForm));
		kryptoPanelMain = new KryptonPanel();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		checkButtonRecordSortDirectionDescending = new KryptonCheckButton();
		checkButtonRecordSortDirectionAscending = new KryptonCheckButton();
		buttonDateOfLastObservation = new KryptonButton();
		buttonComputerName = new KryptonButton();
		buttonRmsResidual = new KryptonButton();
		buttonObservationSpan = new KryptonButton();
		buttonNumberOfObservations = new KryptonButton();
		buttonNumberOfOppositions = new KryptonButton();
		buttonSlopeParameter = new KryptonButton();
		buttonAbsoluteMagnitude = new KryptonButton();
		buttonSemiMajorAxis = new KryptonButton();
		buttonMeanDailyMotion = new KryptonButton();
		buttonOrbitalEccentricity = new KryptonButton();
		buttonInclination = new KryptonButton();
		buttonArgumentOfThePerihelion = new KryptonButton();
		buttonLongitudeOfTheAscendingNode = new KryptonButton();
		buttonMeanAnomaly = new KryptonButton();
		kryptonManager = new KryptonManager(components);
		((ISupportInitialize)kryptoPanelMain).BeginInit();
		kryptoPanelMain.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// kryptoPanelMain
		// 
		kryptoPanelMain.AccessibleDescription = "Groups the data";
		kryptoPanelMain.AccessibleName = "Panel";
		kryptoPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptoPanelMain.Controls.Add(kryptonStatusStrip);
		kryptoPanelMain.Controls.Add(checkButtonRecordSortDirectionDescending);
		kryptoPanelMain.Controls.Add(checkButtonRecordSortDirectionAscending);
		kryptoPanelMain.Controls.Add(buttonDateOfLastObservation);
		kryptoPanelMain.Controls.Add(buttonComputerName);
		kryptoPanelMain.Controls.Add(buttonRmsResidual);
		kryptoPanelMain.Controls.Add(buttonObservationSpan);
		kryptoPanelMain.Controls.Add(buttonNumberOfObservations);
		kryptoPanelMain.Controls.Add(buttonNumberOfOppositions);
		kryptoPanelMain.Controls.Add(buttonSlopeParameter);
		kryptoPanelMain.Controls.Add(buttonAbsoluteMagnitude);
		kryptoPanelMain.Controls.Add(buttonSemiMajorAxis);
		kryptoPanelMain.Controls.Add(buttonMeanDailyMotion);
		kryptoPanelMain.Controls.Add(buttonOrbitalEccentricity);
		kryptoPanelMain.Controls.Add(buttonInclination);
		kryptoPanelMain.Controls.Add(buttonArgumentOfThePerihelion);
		kryptoPanelMain.Controls.Add(buttonLongitudeOfTheAscendingNode);
		kryptoPanelMain.Controls.Add(buttonMeanAnomaly);
		kryptoPanelMain.Dock = DockStyle.Fill;
		kryptoPanelMain.Location = new Point(0, 0);
		kryptoPanelMain.Name = "kryptoPanelMain";
		kryptoPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptoPanelMain.Size = new Size(649, 329);
		kryptoPanelMain.TabIndex = 3;
		kryptoPanelMain.TabStop = true;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar with some information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 307);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.Padding = new Padding(1, 0, 16, 0);
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(649, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 22;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Show some information";
		labelInformation.AccessibleName = "Show some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Show some information";
		// 
		// checkButtonRecordSortDirectionDescending
		// 
		checkButtonRecordSortDirectionDescending.AccessibleDescription = "Selects the descending sort direction";
		checkButtonRecordSortDirectionDescending.AccessibleName = "Sorted descending";
		checkButtonRecordSortDirectionDescending.AccessibleRole = AccessibleRole.PushButton;
		checkButtonRecordSortDirectionDescending.ButtonStyle = ButtonStyle.Form;
		checkButtonRecordSortDirectionDescending.Location = new Point(486, 264);
		checkButtonRecordSortDirectionDescending.Name = "checkButtonRecordSortDirectionDescending";
		checkButtonRecordSortDirectionDescending.Size = new Size(146, 29);
		checkButtonRecordSortDirectionDescending.TabIndex = 21;
		checkButtonRecordSortDirectionDescending.ToolTipValues.Description = "Selects the descending sort direction";
		checkButtonRecordSortDirectionDescending.ToolTipValues.EnableToolTips = true;
		checkButtonRecordSortDirectionDescending.ToolTipValues.Heading = "Sorted descending";
		checkButtonRecordSortDirectionDescending.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkButtonRecordSortDirectionDescending.Values.DropDownArrowColor = Color.Empty;
		checkButtonRecordSortDirectionDescending.Values.Text = "Sorted &descending";
		checkButtonRecordSortDirectionDescending.Click += CheckButtonRecordSortDirectionDescending_Click;
		checkButtonRecordSortDirectionDescending.Enter += Control_Enter;
		checkButtonRecordSortDirectionDescending.Leave += Control_Leave;
		checkButtonRecordSortDirectionDescending.MouseEnter += Control_Enter;
		checkButtonRecordSortDirectionDescending.MouseLeave += Control_Leave;
		// 
		// checkButtonRecordSortDirectionAscending
		// 
		checkButtonRecordSortDirectionAscending.AccessibleDescription = "Selects the ascending sort direction";
		checkButtonRecordSortDirectionAscending.AccessibleName = "Sorted ascending";
		checkButtonRecordSortDirectionAscending.AccessibleRole = AccessibleRole.PushButton;
		checkButtonRecordSortDirectionAscending.ButtonStyle = ButtonStyle.Form;
		checkButtonRecordSortDirectionAscending.Checked = true;
		checkButtonRecordSortDirectionAscending.Location = new Point(327, 264);
		checkButtonRecordSortDirectionAscending.Name = "checkButtonRecordSortDirectionAscending";
		checkButtonRecordSortDirectionAscending.Size = new Size(146, 29);
		checkButtonRecordSortDirectionAscending.TabIndex = 20;
		checkButtonRecordSortDirectionAscending.ToolTipValues.Description = "Selects the ascending sort direction";
		checkButtonRecordSortDirectionAscending.ToolTipValues.EnableToolTips = true;
		checkButtonRecordSortDirectionAscending.ToolTipValues.Heading = "Sorted ascending";
		checkButtonRecordSortDirectionAscending.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkButtonRecordSortDirectionAscending.Values.DropDownArrowColor = Color.Empty;
		checkButtonRecordSortDirectionAscending.Values.Text = "Sorted &ascending";
		checkButtonRecordSortDirectionAscending.Click += CheckButtonRecordSortDirectionAscending_Click;
		checkButtonRecordSortDirectionAscending.Enter += Control_Enter;
		checkButtonRecordSortDirectionAscending.Leave += Control_Leave;
		checkButtonRecordSortDirectionAscending.MouseEnter += Control_Enter;
		checkButtonRecordSortDirectionAscending.MouseLeave += Control_Leave;
		// 
		// buttonDateOfLastObservation
		// 
		buttonDateOfLastObservation.AccessibleDescription = "Copies to clipboard: Date of last observation (YYYMMDD)";
		buttonDateOfLastObservation.AccessibleName = "Copy to clipboard: Date of last observation (YYYMMDD)";
		buttonDateOfLastObservation.AccessibleRole = AccessibleRole.PushButton;
		buttonDateOfLastObservation.ButtonStyle = ButtonStyle.Form;
		buttonDateOfLastObservation.Location = new Point(327, 228);
		buttonDateOfLastObservation.Name = "buttonDateOfLastObservation";
		buttonDateOfLastObservation.Size = new Size(306, 29);
		buttonDateOfLastObservation.TabIndex = 19;
		buttonDateOfLastObservation.ToolTipValues.Description = "Copy to clipboard: Date of last observation (YYYMMDD)";
		buttonDateOfLastObservation.ToolTipValues.EnableToolTips = true;
		buttonDateOfLastObservation.ToolTipValues.Heading = "Date of last observation (YYYMMDD)";
		buttonDateOfLastObservation.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonDateOfLastObservation.Values.DropDownArrowColor = Color.Empty;
		buttonDateOfLastObservation.Values.Text = "Date of last obser&vation (YYYMMDD)";
		buttonDateOfLastObservation.Click += ButtonDateOfLastObservation_Click;
		buttonDateOfLastObservation.Enter += Control_Enter;
		buttonDateOfLastObservation.Leave += Control_Leave;
		buttonDateOfLastObservation.MouseEnter += Control_Enter;
		buttonDateOfLastObservation.MouseLeave += Control_Leave;
		// 
		// buttonComputerName
		// 
		buttonComputerName.AccessibleDescription = "Copies to clipboard: Computer name";
		buttonComputerName.AccessibleName = "Copy to clipboard: Computer name";
		buttonComputerName.AccessibleRole = AccessibleRole.PushButton;
		buttonComputerName.ButtonStyle = ButtonStyle.Form;
		buttonComputerName.Location = new Point(327, 193);
		buttonComputerName.Name = "buttonComputerName";
		buttonComputerName.Size = new Size(306, 29);
		buttonComputerName.TabIndex = 17;
		buttonComputerName.ToolTipValues.Description = "Copy to clipboard: Computer name";
		buttonComputerName.ToolTipValues.EnableToolTips = true;
		buttonComputerName.ToolTipValues.Heading = "Computer name";
		buttonComputerName.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonComputerName.Values.DropDownArrowColor = Color.Empty;
		buttonComputerName.Values.Text = "&Computer name";
		buttonComputerName.Click += ButtonComputerName_Click;
		buttonComputerName.Enter += Control_Enter;
		buttonComputerName.Leave += Control_Leave;
		buttonComputerName.MouseEnter += Control_Enter;
		buttonComputerName.MouseLeave += Control_Leave;
		// 
		// buttonRmsResidual
		// 
		buttonRmsResidual.AccessibleDescription = "Copies to clipboard: r.m.s. residual (\")";
		buttonRmsResidual.AccessibleName = "Copy to clipboard: r.m.s. residual (\")";
		buttonRmsResidual.AccessibleRole = AccessibleRole.PushButton;
		buttonRmsResidual.ButtonStyle = ButtonStyle.Form;
		buttonRmsResidual.Location = new Point(327, 157);
		buttonRmsResidual.Name = "buttonRmsResidual";
		buttonRmsResidual.Size = new Size(306, 29);
		buttonRmsResidual.TabIndex = 16;
		buttonRmsResidual.ToolTipValues.Description = "Copy to clipboard: r.m.s. residual (\")";
		buttonRmsResidual.ToolTipValues.EnableToolTips = true;
		buttonRmsResidual.ToolTipValues.Heading = "r.m.s. residual (\")";
		buttonRmsResidual.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonRmsResidual.Values.DropDownArrowColor = Color.Empty;
		buttonRmsResidual.Values.Text = "r.m.s. &residual (\")";
		buttonRmsResidual.Click += ButtonRmsResidual_Click;
		buttonRmsResidual.Enter += Control_Enter;
		buttonRmsResidual.Leave += Control_Leave;
		buttonRmsResidual.MouseEnter += Control_Enter;
		buttonRmsResidual.MouseLeave += Control_Leave;
		// 
		// buttonObservationSpan
		// 
		buttonObservationSpan.AccessibleDescription = "Copies to clipboard: Observation span";
		buttonObservationSpan.AccessibleName = "Copy to clipboard: Observation span";
		buttonObservationSpan.AccessibleRole = AccessibleRole.PushButton;
		buttonObservationSpan.ButtonStyle = ButtonStyle.Form;
		buttonObservationSpan.Location = new Point(327, 121);
		buttonObservationSpan.Name = "buttonObservationSpan";
		buttonObservationSpan.Size = new Size(306, 29);
		buttonObservationSpan.TabIndex = 15;
		buttonObservationSpan.ToolTipValues.Description = "Copy to clipboard: Observation span";
		buttonObservationSpan.ToolTipValues.EnableToolTips = true;
		buttonObservationSpan.ToolTipValues.Heading = "Observation span";
		buttonObservationSpan.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonObservationSpan.Values.DropDownArrowColor = Color.Empty;
		buttonObservationSpan.Values.Text = "Obser&vation span";
		buttonObservationSpan.Click += ButtonObservationSpan_Click;
		buttonObservationSpan.Enter += Control_Enter;
		buttonObservationSpan.Leave += Control_Leave;
		buttonObservationSpan.MouseEnter += Control_Enter;
		buttonObservationSpan.MouseLeave += Control_Leave;
		// 
		// buttonNumberOfObservations
		// 
		buttonNumberOfObservations.AccessibleDescription = "Copies to clipboard: Number of observations";
		buttonNumberOfObservations.AccessibleName = "Copy to clipboard: Number of observations";
		buttonNumberOfObservations.AccessibleRole = AccessibleRole.PushButton;
		buttonNumberOfObservations.ButtonStyle = ButtonStyle.Form;
		buttonNumberOfObservations.Location = new Point(327, 85);
		buttonNumberOfObservations.Name = "buttonNumberOfObservations";
		buttonNumberOfObservations.Size = new Size(306, 29);
		buttonNumberOfObservations.TabIndex = 14;
		buttonNumberOfObservations.ToolTipValues.Description = "Copy to clipboard: Number of observations";
		buttonNumberOfObservations.ToolTipValues.EnableToolTips = true;
		buttonNumberOfObservations.ToolTipValues.Heading = "Number of observations";
		buttonNumberOfObservations.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonNumberOfObservations.Values.DropDownArrowColor = Color.Empty;
		buttonNumberOfObservations.Values.Text = "Number of o&bservations";
		buttonNumberOfObservations.Click += ButtonNumberOfObservations_Click;
		buttonNumberOfObservations.Enter += Control_Enter;
		buttonNumberOfObservations.Leave += Control_Leave;
		buttonNumberOfObservations.MouseEnter += Control_Enter;
		buttonNumberOfObservations.MouseLeave += Control_Leave;
		// 
		// buttonNumberOfOppositions
		// 
		buttonNumberOfOppositions.AccessibleDescription = "Copies to clipboard: Number of oppositions";
		buttonNumberOfOppositions.AccessibleName = "Copy to clipboard: Number of oppositions";
		buttonNumberOfOppositions.AccessibleRole = AccessibleRole.PushButton;
		buttonNumberOfOppositions.ButtonStyle = ButtonStyle.Form;
		buttonNumberOfOppositions.Location = new Point(327, 50);
		buttonNumberOfOppositions.Name = "buttonNumberOfOppositions";
		buttonNumberOfOppositions.Size = new Size(306, 29);
		buttonNumberOfOppositions.TabIndex = 13;
		buttonNumberOfOppositions.ToolTipValues.Description = "Copy to clipboard: Number of oppositions";
		buttonNumberOfOppositions.ToolTipValues.EnableToolTips = true;
		buttonNumberOfOppositions.ToolTipValues.Heading = "Number of oppositions";
		buttonNumberOfOppositions.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonNumberOfOppositions.Values.DropDownArrowColor = Color.Empty;
		buttonNumberOfOppositions.Values.Text = "Number of &oppositions";
		buttonNumberOfOppositions.Click += ButtonNumberOfOppositions_Click;
		buttonNumberOfOppositions.Enter += Control_Enter;
		buttonNumberOfOppositions.Leave += Control_Leave;
		buttonNumberOfOppositions.MouseEnter += Control_Enter;
		buttonNumberOfOppositions.MouseLeave += Control_Leave;
		// 
		// buttonSlopeParameter
		// 
		buttonSlopeParameter.AccessibleDescription = "Copies to clipboard: Slope parameter, G";
		buttonSlopeParameter.AccessibleName = "Copy to clipboard: Slope parameter, G";
		buttonSlopeParameter.AccessibleRole = AccessibleRole.PushButton;
		buttonSlopeParameter.ButtonStyle = ButtonStyle.Form;
		buttonSlopeParameter.Location = new Point(327, 14);
		buttonSlopeParameter.Name = "buttonSlopeParameter";
		buttonSlopeParameter.Size = new Size(306, 29);
		buttonSlopeParameter.TabIndex = 11;
		buttonSlopeParameter.ToolTipValues.Description = "Copy to clipboard: Slope parameter, G";
		buttonSlopeParameter.ToolTipValues.EnableToolTips = true;
		buttonSlopeParameter.ToolTipValues.Heading = "Slope parameter, G";
		buttonSlopeParameter.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonSlopeParameter.Values.DropDownArrowColor = Color.Empty;
		buttonSlopeParameter.Values.Text = "Slope pa&rameter, G";
		buttonSlopeParameter.Click += ButtonSlopeParameter_Click;
		buttonSlopeParameter.Enter += Control_Enter;
		buttonSlopeParameter.Leave += Control_Leave;
		buttonSlopeParameter.MouseEnter += Control_Enter;
		buttonSlopeParameter.MouseLeave += Control_Leave;
		// 
		// buttonAbsoluteMagnitude
		// 
		buttonAbsoluteMagnitude.AccessibleDescription = "Copies to clipboard: Absolute magnitude, H";
		buttonAbsoluteMagnitude.AccessibleName = "Copy to clipboard: Absolute magnitude, H";
		buttonAbsoluteMagnitude.AccessibleRole = AccessibleRole.PushButton;
		buttonAbsoluteMagnitude.ButtonStyle = ButtonStyle.Form;
		buttonAbsoluteMagnitude.Location = new Point(14, 264);
		buttonAbsoluteMagnitude.Name = "buttonAbsoluteMagnitude";
		buttonAbsoluteMagnitude.Size = new Size(306, 29);
		buttonAbsoluteMagnitude.TabIndex = 10;
		buttonAbsoluteMagnitude.ToolTipValues.Description = "Copy to clipboard: Absolute magnitude, H";
		buttonAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
		buttonAbsoluteMagnitude.ToolTipValues.Heading = "Absolute magnitude, H";
		buttonAbsoluteMagnitude.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonAbsoluteMagnitude.Values.DropDownArrowColor = Color.Empty;
		buttonAbsoluteMagnitude.Values.Text = "Absolute ma&gnitude, H";
		buttonAbsoluteMagnitude.Click += ButtonAbsoluteMagnitude_Click;
		buttonAbsoluteMagnitude.Enter += Control_Enter;
		buttonAbsoluteMagnitude.Leave += Control_Leave;
		buttonAbsoluteMagnitude.MouseEnter += Control_Enter;
		buttonAbsoluteMagnitude.MouseLeave += Control_Leave;
		// 
		// buttonSemiMajorAxis
		// 
		buttonSemiMajorAxis.AccessibleDescription = "Copies to clipboard: Semi-major axis (AU)";
		buttonSemiMajorAxis.AccessibleName = "Copy to clipboard: Semi-major axis (AU)";
		buttonSemiMajorAxis.AccessibleRole = AccessibleRole.PushButton;
		buttonSemiMajorAxis.ButtonStyle = ButtonStyle.Form;
		buttonSemiMajorAxis.Location = new Point(14, 228);
		buttonSemiMajorAxis.Name = "buttonSemiMajorAxis";
		buttonSemiMajorAxis.Size = new Size(306, 29);
		buttonSemiMajorAxis.TabIndex = 9;
		buttonSemiMajorAxis.ToolTipValues.Description = "Copy to clipboard: Semi-major axis (AU)";
		buttonSemiMajorAxis.ToolTipValues.EnableToolTips = true;
		buttonSemiMajorAxis.ToolTipValues.Heading = "Semi-major axis (AU)";
		buttonSemiMajorAxis.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonSemiMajorAxis.Values.DropDownArrowColor = Color.Empty;
		buttonSemiMajorAxis.Values.Text = "&Semi-major axis (AU)";
		buttonSemiMajorAxis.Click += ButtonSemiMajorAxis_Click;
		buttonSemiMajorAxis.Enter += Control_Enter;
		buttonSemiMajorAxis.Leave += Control_Leave;
		buttonSemiMajorAxis.MouseEnter += Control_Enter;
		buttonSemiMajorAxis.MouseLeave += Control_Leave;
		// 
		// buttonMeanDailyMotion
		// 
		buttonMeanDailyMotion.AccessibleDescription = "Copies to clipboard: Mean daily motion (degrees per day)";
		buttonMeanDailyMotion.AccessibleName = "Copy to clipboard: Mean daily motion (degrees per day)";
		buttonMeanDailyMotion.AccessibleRole = AccessibleRole.PushButton;
		buttonMeanDailyMotion.ButtonStyle = ButtonStyle.Form;
		buttonMeanDailyMotion.Location = new Point(14, 193);
		buttonMeanDailyMotion.Name = "buttonMeanDailyMotion";
		buttonMeanDailyMotion.Size = new Size(306, 29);
		buttonMeanDailyMotion.TabIndex = 8;
		buttonMeanDailyMotion.ToolTipValues.Description = "Copy to clipboard: Mean daily motion (degrees per day)";
		buttonMeanDailyMotion.ToolTipValues.EnableToolTips = true;
		buttonMeanDailyMotion.ToolTipValues.Heading = "Mean daily motion (degrees per day)";
		buttonMeanDailyMotion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonMeanDailyMotion.Values.DropDownArrowColor = Color.Empty;
		buttonMeanDailyMotion.Values.Text = "Mean daily &motion (°/day)";
		buttonMeanDailyMotion.Click += ButtonMeanDailyMotion_Click;
		buttonMeanDailyMotion.Enter += Control_Enter;
		buttonMeanDailyMotion.Leave += Control_Leave;
		buttonMeanDailyMotion.MouseEnter += Control_Enter;
		buttonMeanDailyMotion.MouseLeave += Control_Leave;
		// 
		// buttonOrbitalEccentricity
		// 
		buttonOrbitalEccentricity.AccessibleDescription = "Copies to clipboard: Orbital eccentricity";
		buttonOrbitalEccentricity.AccessibleName = "Copy to clipboard: Orbital eccentricity";
		buttonOrbitalEccentricity.AccessibleRole = AccessibleRole.PushButton;
		buttonOrbitalEccentricity.ButtonStyle = ButtonStyle.Form;
		buttonOrbitalEccentricity.Location = new Point(14, 157);
		buttonOrbitalEccentricity.Name = "buttonOrbitalEccentricity";
		buttonOrbitalEccentricity.Size = new Size(306, 29);
		buttonOrbitalEccentricity.TabIndex = 7;
		buttonOrbitalEccentricity.ToolTipValues.Description = "Copy to clipboard: Orbital eccentricity";
		buttonOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
		buttonOrbitalEccentricity.ToolTipValues.Heading = "Orbital eccentricity";
		buttonOrbitalEccentricity.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonOrbitalEccentricity.Values.DropDownArrowColor = Color.Empty;
		buttonOrbitalEccentricity.Values.Text = "Orbital &eccentricity";
		buttonOrbitalEccentricity.Click += ButtonOrbitalEccentricity_Click;
		buttonOrbitalEccentricity.Enter += Control_Enter;
		buttonOrbitalEccentricity.Leave += Control_Leave;
		buttonOrbitalEccentricity.MouseEnter += Control_Enter;
		buttonOrbitalEccentricity.MouseLeave += Control_Leave;
		// 
		// buttonInclination
		// 
		buttonInclination.AccessibleDescription = "Copies to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
		buttonInclination.AccessibleName = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
		buttonInclination.AccessibleRole = AccessibleRole.PushButton;
		buttonInclination.ButtonStyle = ButtonStyle.Form;
		buttonInclination.Location = new Point(14, 121);
		buttonInclination.Name = "buttonInclination";
		buttonInclination.Size = new Size(306, 29);
		buttonInclination.TabIndex = 6;
		buttonInclination.ToolTipValues.Description = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
		buttonInclination.ToolTipValues.EnableToolTips = true;
		buttonInclination.ToolTipValues.Heading = "Inclination to the ecliptic, J2000.0 (degrees)";
		buttonInclination.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonInclination.Values.DropDownArrowColor = Color.Empty;
		buttonInclination.Values.Text = "&Inclination to the ecliptic, J2000.0 (°)";
		buttonInclination.Click += ButtonInclination_Click;
		buttonInclination.Enter += Control_Enter;
		buttonInclination.Leave += Control_Leave;
		buttonInclination.MouseEnter += Control_Enter;
		buttonInclination.MouseLeave += Control_Leave;
		// 
		// buttonArgumentOfThePerihelion
		// 
		buttonArgumentOfThePerihelion.AccessibleDescription = "Copies to clipboard: Argument of the perihelion, J2000.0 (degrees)";
		buttonArgumentOfThePerihelion.AccessibleName = "Copy to clipboard: Argument of the perihelion, J2000.0 (degrees)";
		buttonArgumentOfThePerihelion.AccessibleRole = AccessibleRole.PushButton;
		buttonArgumentOfThePerihelion.ButtonStyle = ButtonStyle.Form;
		buttonArgumentOfThePerihelion.Location = new Point(14, 50);
		buttonArgumentOfThePerihelion.Name = "buttonArgumentOfThePerihelion";
		buttonArgumentOfThePerihelion.Size = new Size(306, 29);
		buttonArgumentOfThePerihelion.TabIndex = 4;
		buttonArgumentOfThePerihelion.ToolTipValues.Description = "Copy to clipboard: Argument of the perihelion, J2000.0 (degrees)";
		buttonArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
		buttonArgumentOfThePerihelion.ToolTipValues.Heading = "Argument of the perihelion, J2000.0 (degrees)";
		buttonArgumentOfThePerihelion.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonArgumentOfThePerihelion.Values.DropDownArrowColor = Color.Empty;
		buttonArgumentOfThePerihelion.Values.Text = "Argument of the peri&helion, J2000.0 (°)";
		buttonArgumentOfThePerihelion.Click += ButtonArgumentOfPerihelion_Click;
		buttonArgumentOfThePerihelion.Enter += Control_Enter;
		buttonArgumentOfThePerihelion.Leave += Control_Leave;
		buttonArgumentOfThePerihelion.MouseEnter += Control_Enter;
		buttonArgumentOfThePerihelion.MouseLeave += Control_Leave;
		// 
		// buttonLongitudeOfTheAscendingNode
		// 
		buttonLongitudeOfTheAscendingNode.AccessibleDescription = "Copies to clipboard: Longitude of the ascending node, J2000.0 (degrees)";
		buttonLongitudeOfTheAscendingNode.AccessibleName = "Copy to clipboard: Longitude of the ascending node, J2000.0 (degrees)";
		buttonLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.PushButton;
		buttonLongitudeOfTheAscendingNode.ButtonStyle = ButtonStyle.Form;
		buttonLongitudeOfTheAscendingNode.Location = new Point(14, 85);
		buttonLongitudeOfTheAscendingNode.Name = "buttonLongitudeOfTheAscendingNode";
		buttonLongitudeOfTheAscendingNode.Size = new Size(306, 29);
		buttonLongitudeOfTheAscendingNode.TabIndex = 5;
		buttonLongitudeOfTheAscendingNode.ToolTipValues.Description = "Copy to clipboard: Longitude of the ascending node, J2000.0 (degrees)";
		buttonLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
		buttonLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Longitude of the ascending node, J2000.0 (degrees)";
		buttonLongitudeOfTheAscendingNode.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonLongitudeOfTheAscendingNode.Values.DropDownArrowColor = Color.Empty;
		buttonLongitudeOfTheAscendingNode.Values.Text = "&Longitude of the ascending node, J2000.0 (°)";
		buttonLongitudeOfTheAscendingNode.Click += ButtonLongitudeOfTheAscendingNode_Click;
		buttonLongitudeOfTheAscendingNode.Enter += Control_Enter;
		buttonLongitudeOfTheAscendingNode.Leave += Control_Leave;
		buttonLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
		buttonLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
		// 
		// buttonMeanAnomaly
		// 
		buttonMeanAnomaly.AccessibleDescription = "Copies to clipboard: Mean anomaly at the epoch (degrees)";
		buttonMeanAnomaly.AccessibleName = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
		buttonMeanAnomaly.AccessibleRole = AccessibleRole.PushButton;
		buttonMeanAnomaly.ButtonStyle = ButtonStyle.Form;
		buttonMeanAnomaly.Location = new Point(14, 14);
		buttonMeanAnomaly.Name = "buttonMeanAnomaly";
		buttonMeanAnomaly.Size = new Size(306, 29);
		buttonMeanAnomaly.TabIndex = 3;
		buttonMeanAnomaly.ToolTipValues.Description = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
		buttonMeanAnomaly.ToolTipValues.EnableToolTips = true;
		buttonMeanAnomaly.ToolTipValues.Heading = "Mean anomaly at the epoch (degrees)";
		buttonMeanAnomaly.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonMeanAnomaly.Values.DropDownArrowColor = Color.Empty;
		buttonMeanAnomaly.Values.Text = "Mean anomal&y at the epoch (°)";
		buttonMeanAnomaly.Click += ButtonMeanAnomaly_Click;
		buttonMeanAnomaly.Enter += Control_Enter;
		buttonMeanAnomaly.Leave += Control_Leave;
		buttonMeanAnomaly.MouseEnter += Control_Enter;
		buttonMeanAnomaly.MouseLeave += Control_Leave;
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// RecordsSelectionForm
		// 
		AccessibleDescription = "Show the top ten records";
		AccessibleName = "Top ten records";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(649, 329);
		ControlBox = false;
		Controls.Add(kryptoPanelMain);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "RecordsSelectionForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Top ten records";
		Load += RecordsSelectionForm_Load;
		((ISupportInitialize)kryptoPanelMain).EndInit();
		kryptoPanelMain.ResumeLayout(false);
		kryptoPanelMain.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		ResumeLayout(false);

	}

	#endregion
	private KryptonPanel kryptoPanelMain;
	private KryptonButton buttonDateOfLastObservation;
	private KryptonButton buttonComputerName;
	private KryptonButton buttonRmsResidual;
	private KryptonButton buttonObservationSpan;
	private KryptonButton buttonNumberOfObservations;
	private KryptonButton buttonNumberOfOppositions;
	private KryptonButton buttonSlopeParameter;
	private KryptonButton buttonAbsoluteMagnitude;
	private KryptonButton buttonSemiMajorAxis;
	private KryptonButton buttonMeanDailyMotion;
	private KryptonButton buttonOrbitalEccentricity;
	private KryptonButton buttonInclination;
	private KryptonButton buttonArgumentOfThePerihelion;
	private KryptonButton buttonLongitudeOfTheAscendingNode;
	private KryptonButton buttonMeanAnomaly;
	private KryptonCheckButton checkButtonRecordSortDirectionAscending;
	private KryptonCheckButton checkButtonRecordSortDirectionDescending;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
}