using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	/// <summary>
	/// 
	/// </summary>
	partial class CopyDataToClipboardForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(CopyDataToClipboardForm));
			buttonIndexNumber = new KryptonButton();
			toolTip = new ToolTip(components);
			buttonReadableDesignation = new KryptonButton();
			buttonEpoch = new KryptonButton();
			buttonMeanAnomaly = new KryptonButton();
			buttonArgumentOfPerihelion = new KryptonButton();
			buttonLongitudeOfTheAscendingNode = new KryptonButton();
			buttonInclination = new KryptonButton();
			buttonOrbitalEccentricity = new KryptonButton();
			buttonMeanDailyMotion = new KryptonButton();
			buttonSemimajorAxis = new KryptonButton();
			buttonAbsoluteMagnitude = new KryptonButton();
			buttonSlopeParameter = new KryptonButton();
			buttonReference = new KryptonButton();
			buttonObservationSpan = new KryptonButton();
			buttonNumberOfObservations = new KryptonButton();
			buttonNumberOfOppositions = new KryptonButton();
			buttonFlags = new KryptonButton();
			buttonComputerName = new KryptonButton();
			buttonRmsResidual = new KryptonButton();
			buttonDateOfLastObservation = new KryptonButton();
			panel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			toolStripContainer = new ToolStripContainer();
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			SuspendLayout();
			// 
			// buttonIndexNumber
			// 
			buttonIndexNumber.AccessibleDescription = "Copy to clipboard: Index No.";
			buttonIndexNumber.AccessibleName = "Copy to clipboard: Index No.";
			buttonIndexNumber.AccessibleRole = AccessibleRole.PushButton;
			buttonIndexNumber.ButtonStyle = ButtonStyle.Form;
			buttonIndexNumber.Location = new Point(14, 14);
			buttonIndexNumber.Margin = new Padding(4, 3, 4, 3);
			buttonIndexNumber.Name = "buttonIndexNumber";
			buttonIndexNumber.Size = new Size(306, 29);
			buttonIndexNumber.TabIndex = 0;
			toolTip.SetToolTip(buttonIndexNumber, "Index No.");
			buttonIndexNumber.Values.DropDownArrowColor = Color.Empty;
			buttonIndexNumber.Values.Text = "Index No.";
			buttonIndexNumber.Click += ButtonIndexNumber_Click;
			buttonIndexNumber.Enter += SetStatusBar_Enter;
			buttonIndexNumber.Leave += ClearStatusBar_Leave;
			buttonIndexNumber.MouseEnter += SetStatusBar_Enter;
			buttonIndexNumber.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonReadableDesignation
			// 
			buttonReadableDesignation.AccessibleDescription = "Copy to clipboard: Readable designation";
			buttonReadableDesignation.AccessibleName = "Copy to clipboard: Readable designation";
			buttonReadableDesignation.AccessibleRole = AccessibleRole.PushButton;
			buttonReadableDesignation.ButtonStyle = ButtonStyle.Form;
			buttonReadableDesignation.Location = new Point(14, 50);
			buttonReadableDesignation.Margin = new Padding(4, 3, 4, 3);
			buttonReadableDesignation.Name = "buttonReadableDesignation";
			buttonReadableDesignation.Size = new Size(306, 29);
			buttonReadableDesignation.TabIndex = 1;
			toolTip.SetToolTip(buttonReadableDesignation, "Readable designation");
			buttonReadableDesignation.Values.DropDownArrowColor = Color.Empty;
			buttonReadableDesignation.Values.Text = "Readable designation";
			buttonReadableDesignation.Click += ButtonReadableDesignation_Click;
			buttonReadableDesignation.Enter += SetStatusBar_Enter;
			buttonReadableDesignation.Leave += ClearStatusBar_Leave;
			buttonReadableDesignation.MouseEnter += SetStatusBar_Enter;
			buttonReadableDesignation.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonEpoch
			// 
			buttonEpoch.AccessibleDescription = "Copy to clipboard: Epoch (in packed form, .0 TT)";
			buttonEpoch.AccessibleName = "Copy to clipboard: Epoch (in packed form, .0 TT)";
			buttonEpoch.AccessibleRole = AccessibleRole.PushButton;
			buttonEpoch.ButtonStyle = ButtonStyle.Form;
			buttonEpoch.Location = new Point(14, 85);
			buttonEpoch.Margin = new Padding(4, 3, 4, 3);
			buttonEpoch.Name = "buttonEpoch";
			buttonEpoch.Size = new Size(306, 29);
			buttonEpoch.TabIndex = 2;
			toolTip.SetToolTip(buttonEpoch, "Epoch (in packed form, .0 TT)");
			buttonEpoch.Values.DropDownArrowColor = Color.Empty;
			buttonEpoch.Values.Text = "Epoch (in packed form, .0 TT)";
			buttonEpoch.Click += ButtonEpoch_Click;
			buttonEpoch.Enter += SetStatusBar_Enter;
			buttonEpoch.Leave += ClearStatusBar_Leave;
			buttonEpoch.MouseEnter += SetStatusBar_Enter;
			buttonEpoch.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonMeanAnomaly
			// 
			buttonMeanAnomaly.AccessibleDescription = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
			buttonMeanAnomaly.AccessibleName = "Copy to clipboard: Mean anomaly at the epoch (degrees)";
			buttonMeanAnomaly.AccessibleRole = AccessibleRole.PushButton;
			buttonMeanAnomaly.ButtonStyle = ButtonStyle.Form;
			buttonMeanAnomaly.Location = new Point(14, 121);
			buttonMeanAnomaly.Margin = new Padding(4, 3, 4, 3);
			buttonMeanAnomaly.Name = "buttonMeanAnomaly";
			buttonMeanAnomaly.Size = new Size(306, 29);
			buttonMeanAnomaly.TabIndex = 3;
			toolTip.SetToolTip(buttonMeanAnomaly, "Mean anomaly at the epoch (degrees)");
			buttonMeanAnomaly.Values.DropDownArrowColor = Color.Empty;
			buttonMeanAnomaly.Values.Text = "Mean anomaly at the epoch (°)";
			buttonMeanAnomaly.Click += ButtonMeanAnomaly_Click;
			buttonMeanAnomaly.Enter += SetStatusBar_Enter;
			buttonMeanAnomaly.Leave += ClearStatusBar_Leave;
			buttonMeanAnomaly.MouseEnter += SetStatusBar_Enter;
			buttonMeanAnomaly.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonArgumentOfPerihelion
			// 
			buttonArgumentOfPerihelion.AccessibleDescription = "Copy to clipboard: Argument of perihelion, J2000.0 (degrees)";
			buttonArgumentOfPerihelion.AccessibleName = "Copy to clipboard: Argument of perihelion, J2000.0 (degrees)";
			buttonArgumentOfPerihelion.AccessibleRole = AccessibleRole.PushButton;
			buttonArgumentOfPerihelion.ButtonStyle = ButtonStyle.Form;
			buttonArgumentOfPerihelion.Location = new Point(14, 157);
			buttonArgumentOfPerihelion.Margin = new Padding(4, 3, 4, 3);
			buttonArgumentOfPerihelion.Name = "buttonArgumentOfPerihelion";
			buttonArgumentOfPerihelion.Size = new Size(306, 29);
			buttonArgumentOfPerihelion.TabIndex = 4;
			toolTip.SetToolTip(buttonArgumentOfPerihelion, "Argument of perihelion, J2000.0 (degrees)");
			buttonArgumentOfPerihelion.Values.DropDownArrowColor = Color.Empty;
			buttonArgumentOfPerihelion.Values.Text = "Argument of perihelion, J2000.0 (°)";
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
			buttonLongitudeOfTheAscendingNode.Location = new Point(14, 193);
			buttonLongitudeOfTheAscendingNode.Margin = new Padding(4, 3, 4, 3);
			buttonLongitudeOfTheAscendingNode.Name = "buttonLongitudeOfTheAscendingNode";
			buttonLongitudeOfTheAscendingNode.Size = new Size(306, 29);
			buttonLongitudeOfTheAscendingNode.TabIndex = 5;
			toolTip.SetToolTip(buttonLongitudeOfTheAscendingNode, "Longitude of the ascending node, J2000.0 (degrees)");
			buttonLongitudeOfTheAscendingNode.Values.DropDownArrowColor = Color.Empty;
			buttonLongitudeOfTheAscendingNode.Values.Text = "Longitude of the ascending node, J2000.0 (°)";
			buttonLongitudeOfTheAscendingNode.Click += ButtonLongitudeOfTheAscendingNode_Click;
			buttonLongitudeOfTheAscendingNode.Enter += SetStatusBar_Enter;
			buttonLongitudeOfTheAscendingNode.Leave += ClearStatusBar_Leave;
			buttonLongitudeOfTheAscendingNode.MouseEnter += SetStatusBar_Enter;
			buttonLongitudeOfTheAscendingNode.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonInclination
			// 
			buttonInclination.AccessibleDescription = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
			buttonInclination.AccessibleName = "Copy to clipboard: Inclination to the ecliptic, J2000.0 (degrees)";
			buttonInclination.AccessibleRole = AccessibleRole.PushButton;
			buttonInclination.ButtonStyle = ButtonStyle.Form;
			buttonInclination.Location = new Point(14, 228);
			buttonInclination.Margin = new Padding(4, 3, 4, 3);
			buttonInclination.Name = "buttonInclination";
			buttonInclination.Size = new Size(306, 29);
			buttonInclination.TabIndex = 6;
			toolTip.SetToolTip(buttonInclination, "Inclination to the ecliptic, J2000.0 (degrees)");
			buttonInclination.Values.DropDownArrowColor = Color.Empty;
			buttonInclination.Values.Text = "Inclination to the ecliptic, J2000.0 (°)";
			buttonInclination.Click += ButtonInclination_Click;
			buttonInclination.Enter += SetStatusBar_Enter;
			buttonInclination.Leave += ClearStatusBar_Leave;
			buttonInclination.MouseEnter += SetStatusBar_Enter;
			buttonInclination.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonOrbitalEccentricity
			// 
			buttonOrbitalEccentricity.AccessibleDescription = "Copy to clipboard: Orbital eccentricity";
			buttonOrbitalEccentricity.AccessibleName = "Copy to clipboard: Orbital eccentricity";
			buttonOrbitalEccentricity.AccessibleRole = AccessibleRole.PushButton;
			buttonOrbitalEccentricity.ButtonStyle = ButtonStyle.Form;
			buttonOrbitalEccentricity.Location = new Point(14, 264);
			buttonOrbitalEccentricity.Margin = new Padding(4, 3, 4, 3);
			buttonOrbitalEccentricity.Name = "buttonOrbitalEccentricity";
			buttonOrbitalEccentricity.Size = new Size(306, 29);
			buttonOrbitalEccentricity.TabIndex = 7;
			toolTip.SetToolTip(buttonOrbitalEccentricity, "Orbital eccentricity");
			buttonOrbitalEccentricity.Values.DropDownArrowColor = Color.Empty;
			buttonOrbitalEccentricity.Values.Text = "Orbital eccentricity";
			buttonOrbitalEccentricity.Click += ButtonOrbitalEccentricity_Click;
			buttonOrbitalEccentricity.Enter += SetStatusBar_Enter;
			buttonOrbitalEccentricity.Leave += ClearStatusBar_Leave;
			buttonOrbitalEccentricity.MouseEnter += SetStatusBar_Enter;
			buttonOrbitalEccentricity.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonMeanDailyMotion
			// 
			buttonMeanDailyMotion.AccessibleDescription = "Copy to clipboard: Mean daily motion (degrees per day)";
			buttonMeanDailyMotion.AccessibleName = "Copy to clipboard: Mean daily motion (degrees per day)";
			buttonMeanDailyMotion.AccessibleRole = AccessibleRole.PushButton;
			buttonMeanDailyMotion.ButtonStyle = ButtonStyle.Form;
			buttonMeanDailyMotion.Location = new Point(14, 300);
			buttonMeanDailyMotion.Margin = new Padding(4, 3, 4, 3);
			buttonMeanDailyMotion.Name = "buttonMeanDailyMotion";
			buttonMeanDailyMotion.Size = new Size(306, 29);
			buttonMeanDailyMotion.TabIndex = 8;
			toolTip.SetToolTip(buttonMeanDailyMotion, "Mean daily motion (degrees per day)");
			buttonMeanDailyMotion.Values.DropDownArrowColor = Color.Empty;
			buttonMeanDailyMotion.Values.Text = "Mean daily motion (°/day)";
			buttonMeanDailyMotion.Click += ButtonMeanDailyMotion_Click;
			buttonMeanDailyMotion.Enter += SetStatusBar_Enter;
			buttonMeanDailyMotion.Leave += ClearStatusBar_Leave;
			buttonMeanDailyMotion.MouseEnter += SetStatusBar_Enter;
			buttonMeanDailyMotion.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonSemimajorAxis
			// 
			buttonSemimajorAxis.AccessibleDescription = "Copy to clipboard: Semi-major axis (AU)";
			buttonSemimajorAxis.AccessibleName = "Copy to clipboard: Semi-major axis (AU)";
			buttonSemimajorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonSemimajorAxis.ButtonStyle = ButtonStyle.Form;
			buttonSemimajorAxis.Location = new Point(14, 336);
			buttonSemimajorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonSemimajorAxis.Name = "buttonSemimajorAxis";
			buttonSemimajorAxis.Size = new Size(306, 29);
			buttonSemimajorAxis.TabIndex = 9;
			toolTip.SetToolTip(buttonSemimajorAxis, "Semi-major axis (AU)");
			buttonSemimajorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonSemimajorAxis.Values.Text = "Semi-major axis (AU)";
			buttonSemimajorAxis.Click += ButtonSemiMajorAxis_Click;
			buttonSemimajorAxis.Enter += SetStatusBar_Enter;
			buttonSemimajorAxis.Leave += ClearStatusBar_Leave;
			buttonSemimajorAxis.MouseEnter += SetStatusBar_Enter;
			buttonSemimajorAxis.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonAbsoluteMagnitude
			// 
			buttonAbsoluteMagnitude.AccessibleDescription = "Copy to clipboard: Absolute magnitude, H";
			buttonAbsoluteMagnitude.AccessibleName = "Copy to clipboard: Absolute magnitude, H";
			buttonAbsoluteMagnitude.AccessibleRole = AccessibleRole.PushButton;
			buttonAbsoluteMagnitude.ButtonStyle = ButtonStyle.Form;
			buttonAbsoluteMagnitude.Location = new Point(327, 14);
			buttonAbsoluteMagnitude.Margin = new Padding(4, 3, 4, 3);
			buttonAbsoluteMagnitude.Name = "buttonAbsoluteMagnitude";
			buttonAbsoluteMagnitude.Size = new Size(306, 29);
			buttonAbsoluteMagnitude.TabIndex = 10;
			toolTip.SetToolTip(buttonAbsoluteMagnitude, "Absolute magnitude, H");
			buttonAbsoluteMagnitude.Values.DropDownArrowColor = Color.Empty;
			buttonAbsoluteMagnitude.Values.Text = "Absolute magnitude, H";
			buttonAbsoluteMagnitude.Click += ButtonAbsoluteMagnitude_Click;
			buttonAbsoluteMagnitude.Enter += SetStatusBar_Enter;
			buttonAbsoluteMagnitude.Leave += ClearStatusBar_Leave;
			buttonAbsoluteMagnitude.MouseEnter += SetStatusBar_Enter;
			buttonAbsoluteMagnitude.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonSlopeParameter
			// 
			buttonSlopeParameter.AccessibleDescription = "Copy to clipboard: Slope parameter, G";
			buttonSlopeParameter.AccessibleName = "Copy to clipboard: Slope parameter, G";
			buttonSlopeParameter.AccessibleRole = AccessibleRole.PushButton;
			buttonSlopeParameter.ButtonStyle = ButtonStyle.Form;
			buttonSlopeParameter.Location = new Point(327, 50);
			buttonSlopeParameter.Margin = new Padding(4, 3, 4, 3);
			buttonSlopeParameter.Name = "buttonSlopeParameter";
			buttonSlopeParameter.Size = new Size(306, 29);
			buttonSlopeParameter.TabIndex = 11;
			toolTip.SetToolTip(buttonSlopeParameter, "Slope parameter, G");
			buttonSlopeParameter.Values.DropDownArrowColor = Color.Empty;
			buttonSlopeParameter.Values.Text = "Slope parameter, G";
			buttonSlopeParameter.Click += ButtonSlopeParameter_Click;
			buttonSlopeParameter.Enter += SetStatusBar_Enter;
			buttonSlopeParameter.Leave += ClearStatusBar_Leave;
			buttonSlopeParameter.MouseEnter += SetStatusBar_Enter;
			buttonSlopeParameter.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonReference
			// 
			buttonReference.AccessibleDescription = "Copy to clipboard: Reference";
			buttonReference.AccessibleName = "Copy to clipboard: Reference";
			buttonReference.AccessibleRole = AccessibleRole.PushButton;
			buttonReference.ButtonStyle = ButtonStyle.Form;
			buttonReference.Location = new Point(327, 85);
			buttonReference.Margin = new Padding(4, 3, 4, 3);
			buttonReference.Name = "buttonReference";
			buttonReference.Size = new Size(306, 29);
			buttonReference.TabIndex = 12;
			toolTip.SetToolTip(buttonReference, "Reference");
			buttonReference.Values.DropDownArrowColor = Color.Empty;
			buttonReference.Values.Text = "Reference";
			buttonReference.Click += ButtonReference_Click;
			buttonReference.Enter += SetStatusBar_Enter;
			buttonReference.Leave += ClearStatusBar_Leave;
			buttonReference.MouseEnter += SetStatusBar_Enter;
			buttonReference.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonObservationSpan
			// 
			buttonObservationSpan.AccessibleDescription = "Copy to clipboard: Observation span";
			buttonObservationSpan.AccessibleName = "Copy to clipboard: Observation span";
			buttonObservationSpan.AccessibleRole = AccessibleRole.PushButton;
			buttonObservationSpan.ButtonStyle = ButtonStyle.Form;
			buttonObservationSpan.Location = new Point(327, 193);
			buttonObservationSpan.Margin = new Padding(4, 3, 4, 3);
			buttonObservationSpan.Name = "buttonObservationSpan";
			buttonObservationSpan.Size = new Size(306, 29);
			buttonObservationSpan.TabIndex = 15;
			toolTip.SetToolTip(buttonObservationSpan, "Observation span");
			buttonObservationSpan.Values.DropDownArrowColor = Color.Empty;
			buttonObservationSpan.Values.Text = "Observation span";
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
			buttonNumberOfObservations.Location = new Point(327, 157);
			buttonNumberOfObservations.Margin = new Padding(4, 3, 4, 3);
			buttonNumberOfObservations.Name = "buttonNumberOfObservations";
			buttonNumberOfObservations.Size = new Size(306, 29);
			buttonNumberOfObservations.TabIndex = 14;
			toolTip.SetToolTip(buttonNumberOfObservations, "Number of observations");
			buttonNumberOfObservations.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfObservations.Values.Text = "Number of observations";
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
			buttonNumberOfOppositions.Location = new Point(327, 121);
			buttonNumberOfOppositions.Margin = new Padding(4, 3, 4, 3);
			buttonNumberOfOppositions.Name = "buttonNumberOfOppositions";
			buttonNumberOfOppositions.Size = new Size(306, 29);
			buttonNumberOfOppositions.TabIndex = 13;
			toolTip.SetToolTip(buttonNumberOfOppositions, "Number of oppositions");
			buttonNumberOfOppositions.Values.DropDownArrowColor = Color.Empty;
			buttonNumberOfOppositions.Values.Text = "Number of oppositions";
			buttonNumberOfOppositions.Click += ButtonNumberOfOppositions_Click;
			buttonNumberOfOppositions.Enter += SetStatusBar_Enter;
			buttonNumberOfOppositions.Leave += ClearStatusBar_Leave;
			buttonNumberOfOppositions.MouseEnter += SetStatusBar_Enter;
			buttonNumberOfOppositions.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonFlags
			// 
			buttonFlags.AccessibleDescription = "Copy to clipboard: 4-hexdigit flags";
			buttonFlags.AccessibleName = "Copy to clipboard: 4-hexdigit flags";
			buttonFlags.AccessibleRole = AccessibleRole.PushButton;
			buttonFlags.ButtonStyle = ButtonStyle.Form;
			buttonFlags.Location = new Point(327, 300);
			buttonFlags.Margin = new Padding(4, 3, 4, 3);
			buttonFlags.Name = "buttonFlags";
			buttonFlags.Size = new Size(306, 29);
			buttonFlags.TabIndex = 18;
			toolTip.SetToolTip(buttonFlags, "4-hexdigit flags");
			buttonFlags.Values.DropDownArrowColor = Color.Empty;
			buttonFlags.Values.Text = "4-hexdigit flags";
			buttonFlags.Click += ButtonFlags_Click;
			buttonFlags.Enter += SetStatusBar_Enter;
			buttonFlags.Leave += ClearStatusBar_Leave;
			buttonFlags.MouseEnter += SetStatusBar_Enter;
			buttonFlags.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonComputerName
			// 
			buttonComputerName.AccessibleDescription = "Copy to clipboard: Computer name";
			buttonComputerName.AccessibleName = "Copy to clipboard: Computer name";
			buttonComputerName.AccessibleRole = AccessibleRole.PushButton;
			buttonComputerName.ButtonStyle = ButtonStyle.Form;
			buttonComputerName.Location = new Point(327, 264);
			buttonComputerName.Margin = new Padding(4, 3, 4, 3);
			buttonComputerName.Name = "buttonComputerName";
			buttonComputerName.Size = new Size(306, 29);
			buttonComputerName.TabIndex = 17;
			toolTip.SetToolTip(buttonComputerName, "Computer name");
			buttonComputerName.Values.DropDownArrowColor = Color.Empty;
			buttonComputerName.Values.Text = "Computer name";
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
			buttonRmsResidual.Location = new Point(327, 228);
			buttonRmsResidual.Margin = new Padding(4, 3, 4, 3);
			buttonRmsResidual.Name = "buttonRmsResidual";
			buttonRmsResidual.Size = new Size(306, 29);
			buttonRmsResidual.TabIndex = 16;
			toolTip.SetToolTip(buttonRmsResidual, "r.m.s. residual (\")");
			buttonRmsResidual.Values.DropDownArrowColor = Color.Empty;
			buttonRmsResidual.Values.Text = "r.m.s. residual (\")";
			buttonRmsResidual.Click += ButtonRmsResidual_Click;
			buttonRmsResidual.Enter += SetStatusBar_Enter;
			buttonRmsResidual.Leave += ClearStatusBar_Leave;
			buttonRmsResidual.MouseEnter += SetStatusBar_Enter;
			buttonRmsResidual.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonDateOfLastObservation
			// 
			buttonDateOfLastObservation.AccessibleDescription = "Copy to clipboard: Date of last observation (YYYMMDD)";
			buttonDateOfLastObservation.AccessibleName = "Copy to clipboard: Date of last observation (YYYMMDD)";
			buttonDateOfLastObservation.AccessibleRole = AccessibleRole.PushButton;
			buttonDateOfLastObservation.ButtonStyle = ButtonStyle.Form;
			buttonDateOfLastObservation.Location = new Point(327, 336);
			buttonDateOfLastObservation.Margin = new Padding(4, 3, 4, 3);
			buttonDateOfLastObservation.Name = "buttonDateOfLastObservation";
			buttonDateOfLastObservation.Size = new Size(306, 29);
			buttonDateOfLastObservation.TabIndex = 19;
			toolTip.SetToolTip(buttonDateOfLastObservation, "Date of last observation (YYYMMDD)");
			buttonDateOfLastObservation.Values.DropDownArrowColor = Color.Empty;
			buttonDateOfLastObservation.Values.Text = "Date of last observation (YYYMMDD)";
			buttonDateOfLastObservation.Click += ButtonDateOfLastObservation_Click;
			buttonDateOfLastObservation.Enter += SetStatusBar_Enter;
			buttonDateOfLastObservation.Leave += ClearStatusBar_Leave;
			buttonDateOfLastObservation.MouseEnter += SetStatusBar_Enter;
			buttonDateOfLastObservation.MouseLeave += ClearStatusBar_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(buttonDateOfLastObservation);
			panel.Controls.Add(buttonFlags);
			panel.Controls.Add(buttonComputerName);
			panel.Controls.Add(buttonRmsResidual);
			panel.Controls.Add(buttonObservationSpan);
			panel.Controls.Add(buttonNumberOfObservations);
			panel.Controls.Add(buttonNumberOfOppositions);
			panel.Controls.Add(buttonReference);
			panel.Controls.Add(buttonSlopeParameter);
			panel.Controls.Add(buttonAbsoluteMagnitude);
			panel.Controls.Add(buttonSemimajorAxis);
			panel.Controls.Add(buttonMeanDailyMotion);
			panel.Controls.Add(buttonOrbitalEccentricity);
			panel.Controls.Add(buttonInclination);
			panel.Controls.Add(buttonArgumentOfPerihelion);
			panel.Controls.Add(buttonLongitudeOfTheAscendingNode);
			panel.Controls.Add(buttonEpoch);
			panel.Controls.Add(buttonMeanAnomaly);
			panel.Controls.Add(buttonIndexNumber);
			panel.Controls.Add(buttonReadableDesignation);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(644, 377);
			panel.TabIndex = 0;
			panel.TabStop = true;
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
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(644, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 20;
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
			// toolStripContainer
			// 
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(panel);
			toolStripContainer.ContentPanel.Size = new Size(644, 377);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(644, 399);
			toolStripContainer.TabIndex = 1;
			toolStripContainer.TopToolStripPanelVisible = false;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// CopyDataToClipboardForm
			// 
			AccessibleDescription = "Copy data to clipboard";
			AccessibleName = "Copy data to clipboard";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(644, 399);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "CopyDataToClipboardForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Copy data to clipboard";
			toolTip.SetToolTip(this, "Copy data to clipboard");
			Load += CopyDataToClipboardForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private KryptonButton buttonIndexNumber;
		private ToolTip toolTip;
		private KryptonButton buttonReadableDesignation;
		private KryptonPanel panel;
		private KryptonButton buttonArgumentOfPerihelion;
		private KryptonButton buttonLongitudeOfTheAscendingNode;
		private KryptonButton buttonEpoch;
		private KryptonButton buttonMeanAnomaly;
		private KryptonButton buttonReference;
		private KryptonButton buttonSlopeParameter;
		private KryptonButton buttonAbsoluteMagnitude;
		private KryptonButton buttonSemimajorAxis;
		private KryptonButton buttonMeanDailyMotion;
		private KryptonButton buttonOrbitalEccentricity;
		private KryptonButton buttonInclination;
		private KryptonButton buttonDateOfLastObservation;
		private KryptonButton buttonFlags;
		private KryptonButton buttonComputerName;
		private KryptonButton buttonRmsResidual;
		private KryptonButton buttonObservationSpan;
		private KryptonButton buttonNumberOfObservations;
		private KryptonButton buttonNumberOfOppositions;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonManager kryptonManager;
	}
}