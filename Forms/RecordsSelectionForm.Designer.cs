using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class RecordsSelectionForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsSelectionForm));
			panel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
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
			buttonArgumentOfPerihelion = new KryptonButton();
			buttonLongitudeOfTheAscendingNode = new KryptonButton();
			buttonMeanAnomaly = new KryptonButton();
			toolTip = new ToolTip(components);
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the buttons";
			panel.AccessibleName = "Groups the buttons";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(statusStrip);
			panel.Controls.Add(checkButtonRecordSortDirectionDescending);
			panel.Controls.Add(checkButtonRecordSortDirectionAscending);
			panel.Controls.Add(buttonDateOfLastObservation);
			panel.Controls.Add(buttonComputerName);
			panel.Controls.Add(buttonRmsResidual);
			panel.Controls.Add(buttonObservationSpan);
			panel.Controls.Add(buttonNumberOfObservations);
			panel.Controls.Add(buttonNumberOfOppositions);
			panel.Controls.Add(buttonSlopeParameter);
			panel.Controls.Add(buttonAbsoluteMagnitude);
			panel.Controls.Add(buttonSemiMajorAxis);
			panel.Controls.Add(buttonMeanDailyMotion);
			panel.Controls.Add(buttonOrbitalEccentricity);
			panel.Controls.Add(buttonInclination);
			panel.Controls.Add(buttonArgumentOfPerihelion);
			panel.Controls.Add(buttonLongitudeOfTheAscendingNode);
			panel.Controls.Add(buttonMeanAnomaly);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(649, 329);
			panel.TabIndex = 3;
			panel.TabStop = true;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 307);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(649, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 22;
			statusStrip.Text = "status bar";
			// 
			// labelInformation
			// 
			labelInformation.AccessibleDescription = "Show some information";
			labelInformation.AccessibleName = "Show some information";
			labelInformation.AccessibleRole = AccessibleRole.StaticText;
			labelInformation.AutoToolTip = true;
			labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelInformation.Margin = new Padding(5, 3, 0, 2);
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
			checkButtonRecordSortDirectionDescending.Margin = new Padding(4, 3, 4, 3);
			checkButtonRecordSortDirectionDescending.Name = "checkButtonRecordSortDirectionDescending";
			checkButtonRecordSortDirectionDescending.Size = new Size(146, 29);
			checkButtonRecordSortDirectionDescending.TabIndex = 21;
			toolTip.SetToolTip(checkButtonRecordSortDirectionDescending, "Sorted descending");
			checkButtonRecordSortDirectionDescending.Values.DropDownArrowColor = Color.Empty;
			checkButtonRecordSortDirectionDescending.Values.Text = "Sorted &descending";
			checkButtonRecordSortDirectionDescending.Click += CheckButtonRecordSortDirectionDescending_Click;
			checkButtonRecordSortDirectionDescending.Enter += SetStatusBar_Enter;
			checkButtonRecordSortDirectionDescending.Leave += ClearStatusBar_Leave;
			checkButtonRecordSortDirectionDescending.MouseEnter += SetStatusBar_Enter;
			checkButtonRecordSortDirectionDescending.MouseLeave += ClearStatusBar_Leave;
			// 
			// checkButtonRecordSortDirectionAscending
			// 
			checkButtonRecordSortDirectionAscending.AccessibleDescription = "Selects the ascending sort direction";
			checkButtonRecordSortDirectionAscending.AccessibleName = "Sorted ascending";
			checkButtonRecordSortDirectionAscending.AccessibleRole = AccessibleRole.PushButton;
			checkButtonRecordSortDirectionAscending.ButtonStyle = ButtonStyle.Form;
			checkButtonRecordSortDirectionAscending.Checked = true;
			checkButtonRecordSortDirectionAscending.Location = new Point(327, 264);
			checkButtonRecordSortDirectionAscending.Margin = new Padding(4, 3, 4, 3);
			checkButtonRecordSortDirectionAscending.Name = "checkButtonRecordSortDirectionAscending";
			checkButtonRecordSortDirectionAscending.Size = new Size(146, 29);
			checkButtonRecordSortDirectionAscending.TabIndex = 20;
			toolTip.SetToolTip(checkButtonRecordSortDirectionAscending, "Sorted ascending");
			checkButtonRecordSortDirectionAscending.Values.DropDownArrowColor = Color.Empty;
			checkButtonRecordSortDirectionAscending.Values.Text = "Sorted &ascending";
			checkButtonRecordSortDirectionAscending.Click += CheckButtonRecordSortDirectionAscending_Click;
			checkButtonRecordSortDirectionAscending.Enter += SetStatusBar_Enter;
			checkButtonRecordSortDirectionAscending.Leave += ClearStatusBar_Leave;
			checkButtonRecordSortDirectionAscending.MouseEnter += SetStatusBar_Enter;
			checkButtonRecordSortDirectionAscending.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonDateOfLastObservation
			// 
			buttonDateOfLastObservation.AccessibleDescription = "Copy to clipboard: Date of last observation (YYYMMDD)";
			buttonDateOfLastObservation.AccessibleName = "Copy to clipboard: Date of last observation (YYYMMDD)";
			buttonDateOfLastObservation.AccessibleRole = AccessibleRole.PushButton;
			buttonDateOfLastObservation.ButtonStyle = ButtonStyle.Form;
			buttonDateOfLastObservation.Location = new Point(327, 228);
			buttonDateOfLastObservation.Margin = new Padding(4, 3, 4, 3);
			buttonDateOfLastObservation.Name = "buttonDateOfLastObservation";
			buttonDateOfLastObservation.Size = new Size(306, 29);
			buttonDateOfLastObservation.TabIndex = 19;
			toolTip.SetToolTip(buttonDateOfLastObservation, "Copy to clipboard: Date of last observation (YYYMMDD)");
			buttonDateOfLastObservation.Values.DropDownArrowColor = Color.Empty;
			buttonDateOfLastObservation.Values.Text = "Date of last obser&vation (YYYMMDD)";
			buttonDateOfLastObservation.Click += ButtonDateOfLastObservation_Click;
			buttonDateOfLastObservation.Enter += SetStatusBar_Enter;
			buttonDateOfLastObservation.Leave += ClearStatusBar_Leave;
			buttonDateOfLastObservation.MouseEnter += SetStatusBar_Enter;
			buttonDateOfLastObservation.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonComputerName
			// 
			buttonComputerName.AccessibleDescription = "Copy to clipboard: Computer name";
			buttonComputerName.AccessibleName = "Copy to clipboard: Computer name";
			buttonComputerName.AccessibleRole = AccessibleRole.PushButton;
			buttonComputerName.ButtonStyle = ButtonStyle.Form;
			buttonComputerName.Location = new Point(327, 193);
			buttonComputerName.Margin = new Padding(4, 3, 4, 3);
			buttonComputerName.Name = "buttonComputerName";
			buttonComputerName.Size = new Size(306, 29);
			buttonComputerName.TabIndex = 17;
			toolTip.SetToolTip(buttonComputerName, "Copy to clipboard: Computer name");
			buttonComputerName.Values.DropDownArrowColor = Color.Empty;
			buttonComputerName.Values.Text = "&Computer name";
			buttonComputerName.Click += ButtonComputerName_Click;
			buttonComputerName.Enter += SetStatusBar_Enter;
			buttonComputerName.Leave += ClearStatusBar_Leave;
			buttonComputerName.MouseEnter += SetStatusBar_Enter;
			buttonComputerName.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonRmsResidual
			// 
			buttonRmsResidual.AccessibleDescription = "Copy to clipboard: r.m.s. residual (\")";
			buttonRmsResidual.AccessibleName = "Copy to clipboard: r.m.s. residual (\")";
			buttonRmsResidual.AccessibleRole = AccessibleRole.PushButton;
			buttonRmsResidual.ButtonStyle = ButtonStyle.Form;
			buttonRmsResidual.Location = new Point(327, 157);
			buttonRmsResidual.Margin = new Padding(4, 3, 4, 3);
			buttonRmsResidual.Name = "buttonRmsResidual";
			buttonRmsResidual.Size = new Size(306, 29);
			buttonRmsResidual.TabIndex = 16;
			toolTip.SetToolTip(buttonRmsResidual, "Copy to clipboard: r.m.s. residual (\")");
			buttonRmsResidual.Values.DropDownArrowColor = Color.Empty;
			buttonRmsResidual.Values.Text = "r.m.s. &residual (\")";
			buttonRmsResidual.Click += ButtonRmsResidual_Click;
			buttonRmsResidual.Enter += SetStatusBar_Enter;
			buttonRmsResidual.Leave += ClearStatusBar_Leave;
			buttonRmsResidual.MouseEnter += SetStatusBar_Enter;
			buttonRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonObservationSpan
			// 
			buttonObservationSpan.AccessibleDescription = "Copy to clipboard: Observation span";
			buttonObservationSpan.AccessibleName = "Copy to clipboard: Observation span";
			buttonObservationSpan.AccessibleRole = AccessibleRole.PushButton;
			buttonObservationSpan.ButtonStyle = ButtonStyle.Form;
			buttonObservationSpan.Location = new Point(327, 121);
			buttonObservationSpan.Margin = new Padding(4, 3, 4, 3);
			buttonObservationSpan.Name = "buttonObservationSpan";
			buttonObservationSpan.Size = new Size(306, 29);
			buttonObservationSpan.TabIndex = 15;
			toolTip.SetToolTip(buttonObservationSpan, "Copy to clipboard: Observation span");
			buttonObservationSpan.Values.DropDownArrowColor = Color.Empty;
			buttonObservationSpan.Values.Text = "Obser&vation span";
			buttonObservationSpan.Click += ButtonObservationSpan_Click;
			buttonObservationSpan.Enter += SetStatusBar_Enter;
			buttonObservationSpan.Leave += ClearStatusBar_Leave;
			buttonObservationSpan.MouseEnter += SetStatusBar_Enter;
			buttonObservationSpan.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonNumberOfObservations
			// 
			buttonNumberOfObservations.AccessibleDescription = "Copy to clipboard: Number of observations";
			buttonNumberOfObservations.AccessibleName = "Copy to clipboard: Number of observations";
			buttonNumberOfObservations.AccessibleRole = AccessibleRole.PushButton;
			buttonNumberOfObservations.ButtonStyle = ButtonStyle.Form;
			buttonNumberOfObservations.Location = new Point(327, 85);
			buttonNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			buttonNumberOfObservations.Name = "buttonNumberOfObservations";
			buttonNumberOfObservations.Size = new Size(306, 29);
			buttonNumberOfObservations.TabIndex = 14;
			toolTip.SetToolTip(buttonNumberOfObservations, "Copy to clipboard: Number of observations");
			buttonNumberOfObservations.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfObservations.Values.Text = "Number of o&bservations";
			buttonNumberOfObservations.Click += ButtonNumberOfObservations_Click;
			buttonNumberOfObservations.Enter += SetStatusBar_Enter;
			buttonNumberOfObservations.Leave += ClearStatusBar_Leave;
			buttonNumberOfObservations.MouseEnter += SetStatusBar_Enter;
			buttonNumberOfObservations.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonNumberOfOppositions
			// 
			buttonNumberOfOppositions.AccessibleDescription = "Copy to clipboard: Number of oppositions";
			buttonNumberOfOppositions.AccessibleName = "Copy to clipboard: Number of oppositions";
			buttonNumberOfOppositions.AccessibleRole = AccessibleRole.PushButton;
			buttonNumberOfOppositions.ButtonStyle = ButtonStyle.Form;
			buttonNumberOfOppositions.Location = new Point(327, 50);
			buttonNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			buttonNumberOfOppositions.Name = "buttonNumberOfOppositions";
			buttonNumberOfOppositions.Size = new Size(306, 29);
			buttonNumberOfOppositions.TabIndex = 13;
			toolTip.SetToolTip(buttonNumberOfOppositions, "Copy to clipboard: Number of oppositions");
			buttonNumberOfOppositions.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfOppositions.Values.Text = "Number of &oppositions";
			buttonNumberOfOppositions.Click += ButtonNumberOfOppositions_Click;
			buttonNumberOfOppositions.Enter += SetStatusBar_Enter;
			buttonNumberOfOppositions.Leave += ClearStatusBar_Leave;
			buttonNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			buttonNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonSlopeParameter
			// 
			buttonSlopeParameter.AccessibleDescription = "Copy to clipboard: Slope parameter, G";
			buttonSlopeParameter.AccessibleName = "Copy to clipboard: Slope parameter, G";
			buttonSlopeParameter.AccessibleRole = AccessibleRole.PushButton;
			buttonSlopeParameter.ButtonStyle = ButtonStyle.Form;
			buttonSlopeParameter.Location = new Point(327, 14);
			buttonSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			buttonSlopeParameter.Name = "buttonSlopeParameter";
			buttonSlopeParameter.Size = new Size(306, 29);
			buttonSlopeParameter.TabIndex = 11;
			toolTip.SetToolTip(buttonSlopeParameter, "Copy to clipboard: Slope parameter, G");
			buttonSlopeParameter.Values.DropDownArrowColor = Color.Empty;
			buttonSlopeParameter.Values.Text = "Slope pa&rameter, G";
			buttonSlopeParameter.Click += ButtonSlopeParameter_Click;
			buttonSlopeParameter.Enter += SetStatusBar_Enter;
			buttonSlopeParameter.Leave += ClearStatusBar_Leave;
			buttonSlopeParameter.MouseEnter += SetStatusBar_Enter;
			buttonSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonAbsoluteMagnitude
			// 
			buttonAbsoluteMagnitude.AccessibleDescription = "Copy to clipboard: Absolute magnitude, H";
			buttonAbsoluteMagnitude.AccessibleName = "Copy to clipboard: Absolute magnitude, H";
			buttonAbsoluteMagnitude.AccessibleRole = AccessibleRole.PushButton;
			buttonAbsoluteMagnitude.ButtonStyle = ButtonStyle.Form;
			buttonAbsoluteMagnitude.Location = new Point(14, 264);
			buttonAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			buttonAbsoluteMagnitude.Name = "buttonAbsoluteMagnitude";
			buttonAbsoluteMagnitude.Size = new Size(306, 29);
			buttonAbsoluteMagnitude.TabIndex = 10;
			toolTip.SetToolTip(buttonAbsoluteMagnitude, "Copy to clipboard: Absolute magnitude, H");
			buttonAbsoluteMagnitude.Values.DropDownArrowColor = Color.Empty;
			buttonAbsoluteMagnitude.Values.Text = "Absolute ma&gnitude, H";
			buttonAbsoluteMagnitude.Click += ButtonAbsoluteMagnitude_Click;
			buttonAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			buttonAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			buttonAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			buttonAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonSemiMajorAxis
			// 
			buttonSemiMajorAxis.AccessibleDescription = "Copy to clipboard: Semi-major axis (AU)";
			buttonSemiMajorAxis.AccessibleName = "Copy to clipboard: Semi-major axis (AU)";
			buttonSemiMajorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonSemiMajorAxis.ButtonStyle = ButtonStyle.Form;
			buttonSemiMajorAxis.Location = new Point(14, 228);
			buttonSemiMajorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonSemiMajorAxis.Name = "buttonSemiMajorAxis";
			buttonSemiMajorAxis.Size = new Size(306, 29);
			buttonSemiMajorAxis.TabIndex = 9;
			toolTip.SetToolTip(buttonSemiMajorAxis, "Copy to clipboard: Semi-major axis (AU)");
			buttonSemiMajorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonSemiMajorAxis.Values.Text = "&Semi-major axis (AU)";
			buttonSemiMajorAxis.Click += ButtonSemiMajorAxis_Click;
			buttonSemiMajorAxis.Enter += SetStatusBar_Enter;
			buttonSemiMajorAxis.Leave += ClearStatusBar_Leave;
			buttonSemiMajorAxis.MouseEnter += SetStatusBar_Enter;
			buttonSemiMajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonMeanDailyMotion
			// 
			buttonMeanDailyMotion.AccessibleDescription = "Copy to clipboard: Mean daily motion (degrees per day)";
			buttonMeanDailyMotion.AccessibleName = "Copy to clipboard: Mean daily motion (degrees per day)";
			buttonMeanDailyMotion.AccessibleRole = AccessibleRole.PushButton;
			buttonMeanDailyMotion.ButtonStyle = ButtonStyle.Form;
			buttonMeanDailyMotion.Location = new Point(14, 193);
			buttonMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			buttonMeanDailyMotion.Name = "buttonMeanDailyMotion";
			buttonMeanDailyMotion.Size = new Size(306, 29);
			buttonMeanDailyMotion.TabIndex = 8;
			toolTip.SetToolTip(buttonMeanDailyMotion, "Copy to clipboard: Mean daily motion (degrees per day)");
			buttonMeanDailyMotion.Values.DropDownArrowColor = Color.Empty;
			buttonMeanDailyMotion.Values.Text = "Mean daily &motion (°/day)";
			buttonMeanDailyMotion.Click += ButtonMeanDailyMotion_Click;
			buttonMeanDailyMotion.Enter += SetStatusBar_Enter;
			buttonMeanDailyMotion.Leave += ClearStatusBar_Leave;
			buttonMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			buttonMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonOrbitalEccentricity
			// 
			buttonOrbitalEccentricity.AccessibleDescription = "Copy to clipboard: Orbital eccentricity";
			buttonOrbitalEccentricity.AccessibleName = "Copy to clipboard: Orbital eccentricity";
			buttonOrbitalEccentricity.AccessibleRole = AccessibleRole.PushButton;
			buttonOrbitalEccentricity.ButtonStyle = ButtonStyle.Form;
			buttonOrbitalEccentricity.Location = new Point(14, 157);
			buttonOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			buttonOrbitalEccentricity.Name = "buttonOrbitalEccentricity";
			buttonOrbitalEccentricity.Size = new Size(306, 29);
			buttonOrbitalEccentricity.TabIndex = 7;
			toolTip.SetToolTip(buttonOrbitalEccentricity, "Copy to clipboard: Orbital eccentricity");
			buttonOrbitalEccentricity.Values.DropDownArrowColor = Color.Empty;
			buttonOrbitalEccentricity.Values.Text = "Orbital &eccentricity";
			buttonOrbitalEccentricity.Click += ButtonOrbitalEccentricity_Click;
			buttonOrbitalEccentricity.Enter += SetStatusBar_Enter;
			buttonOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			buttonOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			buttonOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonInclination
			// 
			buttonInclination.AccessibleDescription = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
			buttonInclination.AccessibleName = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
			buttonInclination.AccessibleRole = AccessibleRole.PushButton;
			buttonInclination.ButtonStyle = ButtonStyle.Form;
			buttonInclination.Location = new Point(14, 121);
			buttonInclination.Margin = new Padding(4, 3, 4, 3);
			buttonInclination.Name = "buttonInclination";
			buttonInclination.Size = new Size(306, 29);
			buttonInclination.TabIndex = 6;
			toolTip.SetToolTip(buttonInclination, "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)");
			buttonInclination.Values.DropDownArrowColor = Color.Empty;
			buttonInclination.Values.Text = "&Inclination to the ecliptic, J2000.0 (°)";
			buttonInclination.Click += ButtonInclination_Click;
			buttonInclination.Enter += SetStatusBar_Enter;
			buttonInclination.Leave += ClearStatusBar_Leave;
			buttonInclination.MouseEnter += SetStatusBar_Enter;
			buttonInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonArgumentOfPerihelion
			// 
			buttonArgumentOfPerihelion.AccessibleDescription = "Copy to clipboard: Argument of perihelion, J2000.0 (degrees)";
			buttonArgumentOfPerihelion.AccessibleName = "Copy to clipboard: Argument of perihelion, J2000.0 (degrees)";
			buttonArgumentOfPerihelion.AccessibleRole = AccessibleRole.PushButton;
			buttonArgumentOfPerihelion.ButtonStyle = ButtonStyle.Form;
			buttonArgumentOfPerihelion.Location = new Point(14, 50);
			buttonArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			buttonArgumentOfPerihelion.Name = "buttonArgumentOfPerihelion";
			buttonArgumentOfPerihelion.Size = new Size(306, 29);
			buttonArgumentOfPerihelion.TabIndex = 4;
			toolTip.SetToolTip(buttonArgumentOfPerihelion, "Copy to clipboard: Argument of perihelion, J2000.0 (degrees)");
			buttonArgumentOfPerihelion.Values.DropDownArrowColor = Color.Empty;
			buttonArgumentOfPerihelion.Values.Text = "Argument of peri&helion, J2000.0 (°)";
			buttonArgumentOfPerihelion.Click += ButtonArgumentOfPerihelion_Click;
			buttonArgumentOfPerihelion.Enter += SetStatusBar_Enter;
			buttonArgumentOfPerihelion.Leave += ClearStatusBar_Leave;
			buttonArgumentOfPerihelion.MouseEnter += SetStatusBar_Enter;
			buttonArgumentOfPerihelion.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonLongitudeOfTheAscendingNode
			// 
			buttonLongitudeOfTheAscendingNode.AccessibleDescription = "Copy to clipboard: Longitude of the ascending node, J2000.0 (degrees)";
			buttonLongitudeOfTheAscendingNode.AccessibleName = "Copy to clipboard: Longitude of the ascending node, J2000.0 (degrees)";
			buttonLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.PushButton;
			buttonLongitudeOfTheAscendingNode.ButtonStyle = ButtonStyle.Form;
			buttonLongitudeOfTheAscendingNode.Location = new Point(14, 85);
			buttonLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			buttonLongitudeOfTheAscendingNode.Name = "buttonLongitudeOfTheAscendingNode";
			buttonLongitudeOfTheAscendingNode.Size = new Size(306, 29);
			buttonLongitudeOfTheAscendingNode.TabIndex = 5;
			toolTip.SetToolTip(buttonLongitudeOfTheAscendingNode, "Copy to clipboard: Longitude of the ascending node, J2000.0 (degrees)");
			buttonLongitudeOfTheAscendingNode.Values.DropDownArrowColor = Color.Empty;
			buttonLongitudeOfTheAscendingNode.Values.Text = "&Longitude of the ascending node, J2000.0 (°)";
			buttonLongitudeOfTheAscendingNode.Click += ButtonLongitudeOfTheAscendingNode_Click;
			buttonLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			buttonLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			buttonLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			buttonLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonMeanAnomaly
			// 
			buttonMeanAnomaly.AccessibleDescription = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
			buttonMeanAnomaly.AccessibleName = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
			buttonMeanAnomaly.AccessibleRole = AccessibleRole.PushButton;
			buttonMeanAnomaly.ButtonStyle = ButtonStyle.Form;
			buttonMeanAnomaly.Location = new Point(14, 14);
			buttonMeanAnomaly.Margin = new Padding(4, 3, 4, 3);
			buttonMeanAnomaly.Name = "buttonMeanAnomaly";
			buttonMeanAnomaly.Size = new Size(306, 29);
			buttonMeanAnomaly.TabIndex = 3;
			toolTip.SetToolTip(buttonMeanAnomaly, "Copy to clipboard: Mean anomaly at the epoch (degrees)");
			buttonMeanAnomaly.Values.DropDownArrowColor = Color.Empty;
			buttonMeanAnomaly.Values.Text = "Mean anomal&y at the epoch (°)";
			buttonMeanAnomaly.Click += ButtonMeanAnomaly_Click;
			buttonMeanAnomaly.Enter += SetStatusBar_Enter;
			buttonMeanAnomaly.Leave += ClearStatusBar_Leave;
			buttonMeanAnomaly.MouseEnter += SetStatusBar_Enter;
			buttonMeanAnomaly.MouseLeave += ClearStatusBar_Leave;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			// 
			// RecordsSelectionForm
			// 
			AccessibleDescription = "Show the top ten records";
			AccessibleName = "Top ten records";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(649, 329);
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "RecordsSelectionForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Top ten records";
			toolTip.SetToolTip(this, "Top ten records");
			FormClosed += RecordsSelectionForm_FormClosed;
			Load += RecordsSelectionForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);

		}

		#endregion
		private KryptonPanel panel;
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
		private KryptonButton buttonArgumentOfPerihelion;
		private KryptonButton buttonLongitudeOfTheAscendingNode;
		private KryptonButton buttonMeanAnomaly;
		private KryptonCheckButton checkButtonRecordSortDirectionAscending;
		private KryptonCheckButton checkButtonRecordSortDirectionDescending;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolTip toolTip;
		private KryptonManager kryptonManager;
	}
}