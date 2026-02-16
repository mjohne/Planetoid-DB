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
			toolTip = new KryptonToolTip(components);
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
			checkButtonRecordSortDirectionAscending.Margin = new Padding(4, 3, 4, 3);
			checkButtonRecordSortDirectionAscending.Name = "checkButtonRecordSortDirectionAscending";
			checkButtonRecordSortDirectionAscending.Size = new Size(146, 29);
			checkButtonRecordSortDirectionAscending.TabIndex = 20;
			toolTip.SetToolTip(checkButtonRecordSortDirectionAscending, "Sorted ascending");
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
			buttonDateOfLastObservation.Enter += Control_Enter;
			buttonDateOfLastObservation.Leave += Control_Leave;
			buttonDateOfLastObservation.MouseEnter += Control_Enter;
			buttonDateOfLastObservation.MouseLeave += Control_Leave;
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
			buttonComputerName.Enter += Control_Enter;
			buttonComputerName.Leave += Control_Leave;
			buttonComputerName.MouseEnter += Control_Enter;
			buttonComputerName.MouseLeave += Control_Leave;
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
			buttonRmsResidual.Enter += Control_Enter;
			buttonRmsResidual.Leave += Control_Leave;
			buttonRmsResidual.MouseEnter += Control_Enter;
			buttonRmsResidual.MouseLeave += Control_Leave;
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
			buttonObservationSpan.Enter += Control_Enter;
			buttonObservationSpan.Leave += Control_Leave;
			buttonObservationSpan.MouseEnter += Control_Enter;
			buttonObservationSpan.MouseLeave += Control_Leave;
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
			buttonNumberOfObservations.Enter += Control_Enter;
			buttonNumberOfObservations.Leave += Control_Leave;
			buttonNumberOfObservations.MouseEnter += Control_Enter;
			buttonNumberOfObservations.MouseLeave += Control_Leave;
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
			buttonNumberOfOppositions.Enter += Control_Enter;
			buttonNumberOfOppositions.Leave += Control_Leave;
			buttonNumberOfOppositions.MouseEnter += Control_Enter;
			buttonNumberOfOppositions.MouseLeave += Control_Leave;
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
			buttonSlopeParameter.Enter += Control_Enter;
			buttonSlopeParameter.Leave += Control_Leave;
			buttonSlopeParameter.MouseEnter += Control_Enter;
			buttonSlopeParameter.MouseLeave += Control_Leave;
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
			buttonAbsoluteMagnitude.Enter += Control_Enter;
			buttonAbsoluteMagnitude.Leave += Control_Leave;
			buttonAbsoluteMagnitude.MouseEnter += Control_Enter;
			buttonAbsoluteMagnitude.MouseLeave += Control_Leave;
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
			buttonSemiMajorAxis.Enter += Control_Enter;
			buttonSemiMajorAxis.Leave += Control_Leave;
			buttonSemiMajorAxis.MouseEnter += Control_Enter;
			buttonSemiMajorAxis.MouseLeave += Control_Leave;
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
			buttonMeanDailyMotion.Enter += Control_Enter;
			buttonMeanDailyMotion.Leave += Control_Leave;
			buttonMeanDailyMotion.MouseEnter += Control_Enter;
			buttonMeanDailyMotion.MouseLeave += Control_Leave;
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
			buttonOrbitalEccentricity.Enter += Control_Enter;
			buttonOrbitalEccentricity.Leave += Control_Leave;
			buttonOrbitalEccentricity.MouseEnter += Control_Enter;
			buttonOrbitalEccentricity.MouseLeave += Control_Leave;
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
			buttonInclination.Enter += Control_Enter;
			buttonInclination.Leave += Control_Leave;
			buttonInclination.MouseEnter += Control_Enter;
			buttonInclination.MouseLeave += Control_Leave;
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
			buttonArgumentOfPerihelion.Enter += Control_Enter;
			buttonArgumentOfPerihelion.Leave += Control_Leave;
			buttonArgumentOfPerihelion.MouseEnter += Control_Enter;
			buttonArgumentOfPerihelion.MouseLeave += Control_Leave;
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
			buttonLongitudeOfTheAscendingNode.Enter += Control_Enter;
			buttonLongitudeOfTheAscendingNode.Leave += Control_Leave;
			buttonLongitudeOfTheAscendingNode.MouseEnter += Control_Enter;
			buttonLongitudeOfTheAscendingNode.MouseLeave += Control_Leave;
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
		private KryptonToolTip toolTip;
		private KryptonManager kryptonManager;
	}
}