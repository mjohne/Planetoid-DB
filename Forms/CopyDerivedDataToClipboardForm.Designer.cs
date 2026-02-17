using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	/// <summary>
	/// 
	/// </summary>
	partial class CopyDerivedDataToClipboardForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(CopyDerivedDataToClipboardForm));
			buttonLinearEccentricity = new KryptonButton();
			buttonSemiMinorAxis = new KryptonButton();
			buttonMajorAxis = new KryptonButton();
			buttonMinorAxis = new KryptonButton();
			buttonEccentricAnomaly = new KryptonButton();
			buttonTrueAnomaly = new KryptonButton();
			buttonPerihelionDistance = new KryptonButton();
			buttonAphelionDistance = new KryptonButton();
			buttonLongitudeDescendingNode = new KryptonButton();
			buttonArgumentAphelion = new KryptonButton();
			buttonFocalParameter = new KryptonButton();
			buttonSemiLatusRectum = new KryptonButton();
			buttonLatusRectum = new KryptonButton();
			buttonOrbitalPerimeter = new KryptonButton();
			buttonOrbitalArea = new KryptonButton();
			buttonOrbitalPeriod = new KryptonButton();
			buttonStandardGravitationalParameter = new KryptonButton();
			buttonMeanAxis = new KryptonButton();
			buttonSemiMeanAxis = new KryptonButton();
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
			// buttonLinearEccentricity
			// 
			buttonLinearEccentricity.AccessibleDescription = "Copy to clipboard: Linear eccentricity (AU)";
			buttonLinearEccentricity.AccessibleName = "Copy to clipboard: Linear eccentricity (AU)";
			buttonLinearEccentricity.AccessibleRole = AccessibleRole.PushButton;
			buttonLinearEccentricity.ButtonStyle = ButtonStyle.Form;
			buttonLinearEccentricity.Location = new Point(14, 14);
			buttonLinearEccentricity.Margin = new Padding(4, 3, 4, 3);
			buttonLinearEccentricity.Name = "buttonLinearEccentricity";
			buttonLinearEccentricity.Size = new Size(306, 29);
			buttonLinearEccentricity.TabIndex = 0;
			buttonLinearEccentricity.ToolTipValues.Description = "Copy to clipboard: Linear eccentricity (AU)";
			buttonLinearEccentricity.ToolTipValues.EnableToolTips = true;
			buttonLinearEccentricity.ToolTipValues.Heading = "Linear eccentricity (AU)";
			buttonLinearEccentricity.Values.DropDownArrowColor = Color.Empty;
			buttonLinearEccentricity.Values.Text = "Linear eccentricity (AU)";
			buttonLinearEccentricity.Click += ButtonLinearEccentricity_Click;
			buttonLinearEccentricity.Enter += Control_Enter;
			buttonLinearEccentricity.Leave += Control_Leave;
			buttonLinearEccentricity.MouseEnter += Control_Enter;
			buttonLinearEccentricity.MouseLeave += Control_Leave;
			// 
			// buttonSemiMinorAxis
			// 
			buttonSemiMinorAxis.AccessibleDescription = "Copy to clipboard: Semi-minor axis (AU)";
			buttonSemiMinorAxis.AccessibleName = "Copy to clipboard: Semi-minor axis (AU)";
			buttonSemiMinorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonSemiMinorAxis.ButtonStyle = ButtonStyle.Form;
			buttonSemiMinorAxis.Location = new Point(14, 50);
			buttonSemiMinorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonSemiMinorAxis.Name = "buttonSemiMinorAxis";
			buttonSemiMinorAxis.Size = new Size(306, 29);
			buttonSemiMinorAxis.TabIndex = 1;
			buttonSemiMinorAxis.ToolTipValues.Description = "Copy to clipboard: Semi-minor axis (AU)";
			buttonSemiMinorAxis.ToolTipValues.EnableToolTips = true;
			buttonSemiMinorAxis.ToolTipValues.Heading = "Semi-minor axis (AU)";
			buttonSemiMinorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonSemiMinorAxis.Values.Text = "Semi-minor axis (AU)";
			buttonSemiMinorAxis.Click += ButtonSemiMinorAxis_Click;
			buttonSemiMinorAxis.Enter += Control_Enter;
			buttonSemiMinorAxis.Leave += Control_Leave;
			buttonSemiMinorAxis.MouseEnter += Control_Enter;
			buttonSemiMinorAxis.MouseLeave += Control_Leave;
			// 
			// buttonMajorAxis
			// 
			buttonMajorAxis.AccessibleDescription = "Copy to clipboard: Major axis (AU)";
			buttonMajorAxis.AccessibleName = "Copy to clipboard: Major axis (AU)";
			buttonMajorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonMajorAxis.ButtonStyle = ButtonStyle.Form;
			buttonMajorAxis.Location = new Point(14, 85);
			buttonMajorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonMajorAxis.Name = "buttonMajorAxis";
			buttonMajorAxis.Size = new Size(306, 29);
			buttonMajorAxis.TabIndex = 2;
			buttonMajorAxis.ToolTipValues.Description = "Copy to clipboard: Major axis (AU)";
			buttonMajorAxis.ToolTipValues.EnableToolTips = true;
			buttonMajorAxis.ToolTipValues.Heading = "Major axis (AU)";
			buttonMajorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonMajorAxis.Values.Text = "Major axis (AU)";
			buttonMajorAxis.Click += ButtonMajorAxis_Click;
			buttonMajorAxis.Enter += Control_Enter;
			buttonMajorAxis.Leave += Control_Leave;
			buttonMajorAxis.MouseEnter += Control_Enter;
			buttonMajorAxis.MouseLeave += Control_Leave;
			// 
			// buttonMinorAxis
			// 
			buttonMinorAxis.AccessibleDescription = "Copy to clipboard: Minor axis (AU)";
			buttonMinorAxis.AccessibleName = "Copy to clipboard: Minor axis (AU)";
			buttonMinorAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonMinorAxis.ButtonStyle = ButtonStyle.Form;
			buttonMinorAxis.Location = new Point(14, 121);
			buttonMinorAxis.Margin = new Padding(4, 3, 4, 3);
			buttonMinorAxis.Name = "buttonMinorAxis";
			buttonMinorAxis.Size = new Size(306, 29);
			buttonMinorAxis.TabIndex = 3;
			buttonMinorAxis.ToolTipValues.Description = "Copy to clipboard: Minor axis (AU)";
			buttonMinorAxis.ToolTipValues.EnableToolTips = true;
			buttonMinorAxis.ToolTipValues.Heading = "Minor axis (AU)";
			buttonMinorAxis.Values.DropDownArrowColor = Color.Empty;
			buttonMinorAxis.Values.Text = "Minor axis (AU)";
			buttonMinorAxis.Click += ButtonMinorAxis_Click;
			buttonMinorAxis.Enter += Control_Enter;
			buttonMinorAxis.Leave += Control_Leave;
			buttonMinorAxis.MouseEnter += Control_Enter;
			buttonMinorAxis.MouseLeave += Control_Leave;
			// 
			// buttonEccentricAnomaly
			// 
			buttonEccentricAnomaly.AccessibleDescription = "Copy to clipboard: Eccentric anomaly (degrees)";
			buttonEccentricAnomaly.AccessibleName = "Copy to clipboard: Eccentric anomaly (degrees)";
			buttonEccentricAnomaly.AccessibleRole = AccessibleRole.PushButton;
			buttonEccentricAnomaly.ButtonStyle = ButtonStyle.Form;
			buttonEccentricAnomaly.Location = new Point(14, 157);
			buttonEccentricAnomaly.Margin = new Padding(4, 3, 4, 3);
			buttonEccentricAnomaly.Name = "buttonEccentricAnomaly";
			buttonEccentricAnomaly.Size = new Size(306, 29);
			buttonEccentricAnomaly.TabIndex = 4;
			buttonEccentricAnomaly.ToolTipValues.Description = "Copy to clipboard: Eccentric anomaly ((degrees)";
			buttonEccentricAnomaly.ToolTipValues.EnableToolTips = true;
			buttonEccentricAnomaly.ToolTipValues.Heading = "Eccentric anomaly ((degrees)";
			buttonEccentricAnomaly.Values.DropDownArrowColor = Color.Empty;
			buttonEccentricAnomaly.Values.Text = "Eccentric anomaly (°)";
			buttonEccentricAnomaly.Click += ButtonEccentricAnomaly_Click;
			buttonEccentricAnomaly.Enter += Control_Enter;
			buttonEccentricAnomaly.Leave += Control_Leave;
			buttonEccentricAnomaly.MouseEnter += Control_Enter;
			buttonEccentricAnomaly.MouseLeave += Control_Leave;
			// 
			// buttonTrueAnomaly
			// 
			buttonTrueAnomaly.AccessibleDescription = "Copy to clipboard: True anomaly (degrees)";
			buttonTrueAnomaly.AccessibleName = "Copy to clipboard: True anomaly (degrees)";
			buttonTrueAnomaly.AccessibleRole = AccessibleRole.PushButton;
			buttonTrueAnomaly.ButtonStyle = ButtonStyle.Form;
			buttonTrueAnomaly.Location = new Point(14, 193);
			buttonTrueAnomaly.Margin = new Padding(4, 3, 4, 3);
			buttonTrueAnomaly.Name = "buttonTrueAnomaly";
			buttonTrueAnomaly.Size = new Size(306, 29);
			buttonTrueAnomaly.TabIndex = 5;
			buttonTrueAnomaly.ToolTipValues.Description = "Copy to clipboard: True anomaly ((degrees)";
			buttonTrueAnomaly.ToolTipValues.EnableToolTips = true;
			buttonTrueAnomaly.ToolTipValues.Heading = "True anomaly (degrees)";
			buttonTrueAnomaly.Values.DropDownArrowColor = Color.Empty;
			buttonTrueAnomaly.Values.Text = "True anomaly (°)";
			buttonTrueAnomaly.Click += ButtonTrueAnomaly_Click;
			buttonTrueAnomaly.Enter += Control_Enter;
			buttonTrueAnomaly.Leave += Control_Leave;
			buttonTrueAnomaly.MouseEnter += Control_Enter;
			buttonTrueAnomaly.MouseLeave += Control_Leave;
			// 
			// buttonPerihelionDistance
			// 
			buttonPerihelionDistance.AccessibleDescription = "Copy to clipboard: Perihelion distance (AU)";
			buttonPerihelionDistance.AccessibleName = "Copy to clipboard: Perihelion distance (AU)";
			buttonPerihelionDistance.AccessibleRole = AccessibleRole.PushButton;
			buttonPerihelionDistance.ButtonStyle = ButtonStyle.Form;
			buttonPerihelionDistance.Location = new Point(14, 228);
			buttonPerihelionDistance.Margin = new Padding(4, 3, 4, 3);
			buttonPerihelionDistance.Name = "buttonPerihelionDistance";
			buttonPerihelionDistance.Size = new Size(306, 29);
			buttonPerihelionDistance.TabIndex = 6;
			buttonPerihelionDistance.ToolTipValues.Description = "Copy to clipboard: Perihelion distance (AU)";
			buttonPerihelionDistance.ToolTipValues.EnableToolTips = true;
			buttonPerihelionDistance.ToolTipValues.Heading = "Perihelion distance (AU)";
			buttonPerihelionDistance.Values.DropDownArrowColor = Color.Empty;
			buttonPerihelionDistance.Values.Text = "Perihelion distance (AU)";
			buttonPerihelionDistance.Click += ButtonPerihelionDistance_Click;
			buttonPerihelionDistance.Enter += Control_Enter;
			buttonPerihelionDistance.Leave += Control_Leave;
			buttonPerihelionDistance.MouseEnter += Control_Enter;
			buttonPerihelionDistance.MouseLeave += Control_Leave;
			// 
			// buttonAphelionDistance
			// 
			buttonAphelionDistance.AccessibleDescription = "Copy to clipboard: Aphelion distance (AU)";
			buttonAphelionDistance.AccessibleName = "Copy to clipboard: Aphelion distance (AU)";
			buttonAphelionDistance.AccessibleRole = AccessibleRole.PushButton;
			buttonAphelionDistance.ButtonStyle = ButtonStyle.Form;
			buttonAphelionDistance.Location = new Point(14, 264);
			buttonAphelionDistance.Margin = new Padding(4, 3, 4, 3);
			buttonAphelionDistance.Name = "buttonAphelionDistance";
			buttonAphelionDistance.Size = new Size(306, 29);
			buttonAphelionDistance.TabIndex = 7;
			buttonAphelionDistance.ToolTipValues.Description = "Copy to clipboard: Aphelion distance (AU)";
			buttonAphelionDistance.ToolTipValues.EnableToolTips = true;
			buttonAphelionDistance.ToolTipValues.Heading = "Aphelion distance (AU)";
			buttonAphelionDistance.Values.DropDownArrowColor = Color.Empty;
			buttonAphelionDistance.Values.Text = "Aphelion distance (AU)";
			buttonAphelionDistance.Click += ButtonAphelionDistance_Click;
			buttonAphelionDistance.Enter += Control_Enter;
			buttonAphelionDistance.Leave += Control_Leave;
			buttonAphelionDistance.MouseEnter += Control_Enter;
			buttonAphelionDistance.MouseLeave += Control_Leave;
			// 
			// buttonLongitudeDescendingNode
			// 
			buttonLongitudeDescendingNode.AccessibleDescription = "Copy to clipboard: Longitude of the descending node (degrees)";
			buttonLongitudeDescendingNode.AccessibleName = "Copy to clipboard: Longitude of the descending node (degrees)";
			buttonLongitudeDescendingNode.AccessibleRole = AccessibleRole.PushButton;
			buttonLongitudeDescendingNode.ButtonStyle = ButtonStyle.Form;
			buttonLongitudeDescendingNode.Location = new Point(14, 300);
			buttonLongitudeDescendingNode.Margin = new Padding(4, 3, 4, 3);
			buttonLongitudeDescendingNode.Name = "buttonLongitudeDescendingNode";
			buttonLongitudeDescendingNode.Size = new Size(306, 29);
			buttonLongitudeDescendingNode.TabIndex = 8;
			buttonLongitudeDescendingNode.ToolTipValues.Description = "Copy to clipboard: Longitude of the descending node (degrees)";
			buttonLongitudeDescendingNode.ToolTipValues.EnableToolTips = true;
			buttonLongitudeDescendingNode.ToolTipValues.Heading = "Longitude of the descending node (degrees)";
			buttonLongitudeDescendingNode.Values.DropDownArrowColor = Color.Empty;
			buttonLongitudeDescendingNode.Values.Text = "Longitude of the descending node (°)";
			buttonLongitudeDescendingNode.Click += ButtonLongitudeDescendingNode_Click;
			buttonLongitudeDescendingNode.Enter += Control_Enter;
			buttonLongitudeDescendingNode.Leave += Control_Leave;
			buttonLongitudeDescendingNode.MouseEnter += Control_Enter;
			buttonLongitudeDescendingNode.MouseLeave += Control_Leave;
			// 
			// buttonArgumentAphelion
			// 
			buttonArgumentAphelion.AccessibleDescription = "Copy to clipboard: Argument of aphelion (degrees)";
			buttonArgumentAphelion.AccessibleName = "Copy to clipboard: Argument of aphelion (degrees)";
			buttonArgumentAphelion.AccessibleRole = AccessibleRole.PushButton;
			buttonArgumentAphelion.ButtonStyle = ButtonStyle.Form;
			buttonArgumentAphelion.Location = new Point(14, 336);
			buttonArgumentAphelion.Margin = new Padding(4, 3, 4, 3);
			buttonArgumentAphelion.Name = "buttonArgumentAphelion";
			buttonArgumentAphelion.Size = new Size(306, 29);
			buttonArgumentAphelion.TabIndex = 9;
			buttonArgumentAphelion.ToolTipValues.Description = "Copy to clipboard: Argument of aphelion (degrees)";
			buttonArgumentAphelion.ToolTipValues.EnableToolTips = true;
			buttonArgumentAphelion.ToolTipValues.Heading = "Argument of aphelion (degrees)";
			buttonArgumentAphelion.Values.DropDownArrowColor = Color.Empty;
			buttonArgumentAphelion.Values.Text = "Argument of aphelion (°)";
			buttonArgumentAphelion.Click += ButtonArgumentAphelion_Click;
			buttonArgumentAphelion.Enter += Control_Enter;
			buttonArgumentAphelion.Leave += Control_Leave;
			buttonArgumentAphelion.MouseEnter += Control_Enter;
			buttonArgumentAphelion.MouseLeave += Control_Leave;
			// 
			// buttonFocalParameter
			// 
			buttonFocalParameter.AccessibleDescription = "Copy to clipboard: Focal parameter (AU)";
			buttonFocalParameter.AccessibleName = "Copy to clipboard: Focal parameter (AU)";
			buttonFocalParameter.AccessibleRole = AccessibleRole.PushButton;
			buttonFocalParameter.ButtonStyle = ButtonStyle.Form;
			buttonFocalParameter.Location = new Point(327, 14);
			buttonFocalParameter.Margin = new Padding(4, 3, 4, 3);
			buttonFocalParameter.Name = "buttonFocalParameter";
			buttonFocalParameter.Size = new Size(306, 29);
			buttonFocalParameter.TabIndex = 10;
			buttonFocalParameter.ToolTipValues.Description = "Copy to clipboard: Focal parameter (AU)";
			buttonFocalParameter.ToolTipValues.EnableToolTips = true;
			buttonFocalParameter.ToolTipValues.Heading = "Focal parameter (AU)";
			buttonFocalParameter.Values.DropDownArrowColor = Color.Empty;
			buttonFocalParameter.Values.Text = "Focal parameter (AU)";
			buttonFocalParameter.Click += ButtonFocalParameter_Click;
			buttonFocalParameter.Enter += Control_Enter;
			buttonFocalParameter.Leave += Control_Leave;
			buttonFocalParameter.MouseEnter += Control_Enter;
			buttonFocalParameter.MouseLeave += Control_Leave;
			// 
			// buttonSemiLatusRectum
			// 
			buttonSemiLatusRectum.AccessibleDescription = "Copy to clipboard: Semi-latus rectum (AU)";
			buttonSemiLatusRectum.AccessibleName = "Copy to clipboard: Semi-latus rectum (AU)";
			buttonSemiLatusRectum.AccessibleRole = AccessibleRole.PushButton;
			buttonSemiLatusRectum.ButtonStyle = ButtonStyle.Form;
			buttonSemiLatusRectum.Location = new Point(327, 50);
			buttonSemiLatusRectum.Margin = new Padding(4, 3, 4, 3);
			buttonSemiLatusRectum.Name = "buttonSemiLatusRectum";
			buttonSemiLatusRectum.Size = new Size(306, 29);
			buttonSemiLatusRectum.TabIndex = 11;
			buttonSemiLatusRectum.ToolTipValues.Description = "Copy to clipboard: Semi-latus rectum (AU)";
			buttonSemiLatusRectum.ToolTipValues.EnableToolTips = true;
			buttonSemiLatusRectum.ToolTipValues.Heading = "Semi-latus rectum (AU)";
			buttonSemiLatusRectum.Values.DropDownArrowColor = Color.Empty;
			buttonSemiLatusRectum.Values.Text = "Semi-latus rectum (AU)";
			buttonSemiLatusRectum.Click += ButtonSemiLatusRectum_Click;
			buttonSemiLatusRectum.Enter += Control_Enter;
			buttonSemiLatusRectum.Leave += Control_Leave;
			buttonSemiLatusRectum.MouseEnter += Control_Enter;
			buttonSemiLatusRectum.MouseLeave += Control_Leave;
			// 
			// buttonLatusRectum
			// 
			buttonLatusRectum.AccessibleDescription = "Copy to clipboard: Latus rectum (AU)";
			buttonLatusRectum.AccessibleName = "Copy to clipboard: Latus rectum (AU)";
			buttonLatusRectum.AccessibleRole = AccessibleRole.PushButton;
			buttonLatusRectum.ButtonStyle = ButtonStyle.Form;
			buttonLatusRectum.Location = new Point(327, 85);
			buttonLatusRectum.Margin = new Padding(4, 3, 4, 3);
			buttonLatusRectum.Name = "buttonLatusRectum";
			buttonLatusRectum.Size = new Size(306, 29);
			buttonLatusRectum.TabIndex = 12;
			buttonLatusRectum.ToolTipValues.Description = "Copy to clipboard: Latus rectum (AU)";
			buttonLatusRectum.ToolTipValues.EnableToolTips = true;
			buttonLatusRectum.ToolTipValues.Heading = "Latus rectum (AU)";
			buttonLatusRectum.Values.DropDownArrowColor = Color.Empty;
			buttonLatusRectum.Values.Text = "Latus rectum (AU)";
			buttonLatusRectum.Click += ButtonLatusRectum_Click;
			buttonLatusRectum.Enter += Control_Enter;
			buttonLatusRectum.Leave += Control_Leave;
			buttonLatusRectum.MouseEnter += Control_Enter;
			buttonLatusRectum.MouseLeave += Control_Leave;
			// 
			// buttonOrbitalPerimeter
			// 
			buttonOrbitalPerimeter.AccessibleDescription = "Copy to clipboard: Orbital perimeter (AU)";
			buttonOrbitalPerimeter.AccessibleName = "Copy to clipboard: Orbital perimeter (AU)";
			buttonOrbitalPerimeter.AccessibleRole = AccessibleRole.PushButton;
			buttonOrbitalPerimeter.ButtonStyle = ButtonStyle.Form;
			buttonOrbitalPerimeter.Location = new Point(327, 193);
			buttonOrbitalPerimeter.Margin = new Padding(4, 3, 4, 3);
			buttonOrbitalPerimeter.Name = "buttonOrbitalPerimeter";
			buttonOrbitalPerimeter.Size = new Size(306, 29);
			buttonOrbitalPerimeter.TabIndex = 15;
			buttonOrbitalPerimeter.ToolTipValues.Description = "Copy to clipboard: Orbital perimeter (AU)";
			buttonOrbitalPerimeter.ToolTipValues.EnableToolTips = true;
			buttonOrbitalPerimeter.ToolTipValues.Heading = "Orbital perimeter (AU)";
			buttonOrbitalPerimeter.Values.DropDownArrowColor = Color.Empty;
			buttonOrbitalPerimeter.Values.Text = "Orbital perimeter (AU)";
			buttonOrbitalPerimeter.Click += ButtonOrbitalPerimeter_Click;
			buttonOrbitalPerimeter.Enter += Control_Enter;
			buttonOrbitalPerimeter.Leave += Control_Leave;
			buttonOrbitalPerimeter.MouseEnter += Control_Enter;
			buttonOrbitalPerimeter.MouseLeave += Control_Leave;
			// 
			// buttonOrbitalArea
			// 
			buttonOrbitalArea.AccessibleDescription = "Copy to clipboard: Orbital area (AU²)";
			buttonOrbitalArea.AccessibleName = "Copy to clipboard: Orbital area (AU²)";
			buttonOrbitalArea.AccessibleRole = AccessibleRole.PushButton;
			buttonOrbitalArea.ButtonStyle = ButtonStyle.Form;
			buttonOrbitalArea.Location = new Point(327, 157);
			buttonOrbitalArea.Margin = new Padding(4, 3, 4, 3);
			buttonOrbitalArea.Name = "buttonOrbitalArea";
			buttonOrbitalArea.Size = new Size(306, 29);
			buttonOrbitalArea.TabIndex = 14;
			buttonOrbitalArea.ToolTipValues.Description = "Copy to clipboard: Orbital area (AU²)";
			buttonOrbitalArea.ToolTipValues.EnableToolTips = true;
			buttonOrbitalArea.ToolTipValues.Heading = "Orbital area (AU²)";
			buttonOrbitalArea.Values.DropDownArrowColor = Color.Empty;
			buttonOrbitalArea.Values.Text = "Orbital area (AU²)";
			buttonOrbitalArea.Click += ButtonOrbitalArea_Click;
			buttonOrbitalArea.Enter += Control_Enter;
			buttonOrbitalArea.Leave += Control_Leave;
			buttonOrbitalArea.MouseEnter += Control_Enter;
			buttonOrbitalArea.MouseLeave += Control_Leave;
			// 
			// buttonOrbitalPeriod
			// 
			buttonOrbitalPeriod.AccessibleDescription = "Copy to clipboard: Orbital period (years)";
			buttonOrbitalPeriod.AccessibleName = "Copy to clipboard: Orbital period (years)";
			buttonOrbitalPeriod.AccessibleRole = AccessibleRole.PushButton;
			buttonOrbitalPeriod.ButtonStyle = ButtonStyle.Form;
			buttonOrbitalPeriod.Location = new Point(327, 121);
			buttonOrbitalPeriod.Margin = new Padding(4, 3, 4, 3);
			buttonOrbitalPeriod.Name = "buttonOrbitalPeriod";
			buttonOrbitalPeriod.Size = new Size(306, 29);
			buttonOrbitalPeriod.TabIndex = 13;
			buttonOrbitalPeriod.ToolTipValues.Description = "Copy to clipboard: Orbital period (years)";
			buttonOrbitalPeriod.ToolTipValues.EnableToolTips = true;
			buttonOrbitalPeriod.ToolTipValues.Heading = "Orbital period (years)";
			buttonOrbitalPeriod.Values.DropDownArrowColor = Color.Empty;
			buttonOrbitalPeriod.Values.Text = "Orbital period (years)";
			buttonOrbitalPeriod.Click += ButtonOrbitalPeriod_Click;
			buttonOrbitalPeriod.Enter += Control_Enter;
			buttonOrbitalPeriod.Leave += Control_Leave;
			buttonOrbitalPeriod.MouseEnter += Control_Enter;
			buttonOrbitalPeriod.MouseLeave += Control_Leave;
			// 
			// buttonStandardGravitationalParameter
			// 
			buttonStandardGravitationalParameter.AccessibleDescription = "Copy to clipboard: Standard gravitational parameter (AU³/a²)";
			buttonStandardGravitationalParameter.AccessibleName = "Copy to clipboard: Standard gravitational parameter (AU³/a²)";
			buttonStandardGravitationalParameter.AccessibleRole = AccessibleRole.PushButton;
			buttonStandardGravitationalParameter.ButtonStyle = ButtonStyle.Form;
			buttonStandardGravitationalParameter.Location = new Point(327, 300);
			buttonStandardGravitationalParameter.Margin = new Padding(4, 3, 4, 3);
			buttonStandardGravitationalParameter.Name = "buttonStandardGravitationalParameter";
			buttonStandardGravitationalParameter.Size = new Size(306, 29);
			buttonStandardGravitationalParameter.TabIndex = 18;
			buttonStandardGravitationalParameter.ToolTipValues.Description = "Copy to clipboard: Standard gravitational parameter (AU³/a²)";
			buttonStandardGravitationalParameter.ToolTipValues.EnableToolTips = true;
			buttonStandardGravitationalParameter.ToolTipValues.Heading = "Standard gravitational parameter (AU³/a²)";
			buttonStandardGravitationalParameter.Values.DropDownArrowColor = Color.Empty;
			buttonStandardGravitationalParameter.Values.Text = "Standard gravitational parameter (AU³/a²)";
			buttonStandardGravitationalParameter.Click += ButtonStandardGravitationalParameter_Click;
			buttonStandardGravitationalParameter.Enter += Control_Enter;
			buttonStandardGravitationalParameter.Leave += Control_Leave;
			buttonStandardGravitationalParameter.MouseEnter += Control_Enter;
			buttonStandardGravitationalParameter.MouseLeave += Control_Leave;
			// 
			// buttonMeanAxis
			// 
			buttonMeanAxis.AccessibleDescription = "Copy to clipboard: Mean axis (AU)";
			buttonMeanAxis.AccessibleName = "Copy to clipboard: Mean axis (AU)";
			buttonMeanAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonMeanAxis.ButtonStyle = ButtonStyle.Form;
			buttonMeanAxis.Location = new Point(327, 264);
			buttonMeanAxis.Margin = new Padding(4, 3, 4, 3);
			buttonMeanAxis.Name = "buttonMeanAxis";
			buttonMeanAxis.Size = new Size(306, 29);
			buttonMeanAxis.TabIndex = 17;
			buttonMeanAxis.ToolTipValues.Description = "Copy to clipboard: Mean axis (AU)";
			buttonMeanAxis.ToolTipValues.EnableToolTips = true;
			buttonMeanAxis.ToolTipValues.Heading = "Mean axis (AU)";
			buttonMeanAxis.Values.DropDownArrowColor = Color.Empty;
			buttonMeanAxis.Values.Text = "Mean axis (AU)";
			buttonMeanAxis.Click += ButtonMeanAxis_Click;
			buttonMeanAxis.Enter += Control_Enter;
			buttonMeanAxis.Leave += Control_Leave;
			buttonMeanAxis.MouseEnter += Control_Enter;
			buttonMeanAxis.MouseLeave += Control_Leave;
			// 
			// buttonSemiMeanAxis
			// 
			buttonSemiMeanAxis.AccessibleDescription = "Copy to clipboard: Semi-mean axis (AU)";
			buttonSemiMeanAxis.AccessibleName = "Copy to clipboard: Semi-mean axis (AU)";
			buttonSemiMeanAxis.AccessibleRole = AccessibleRole.PushButton;
			buttonSemiMeanAxis.ButtonStyle = ButtonStyle.Form;
			buttonSemiMeanAxis.Location = new Point(327, 228);
			buttonSemiMeanAxis.Margin = new Padding(4, 3, 4, 3);
			buttonSemiMeanAxis.Name = "buttonSemiMeanAxis";
			buttonSemiMeanAxis.Size = new Size(306, 29);
			buttonSemiMeanAxis.TabIndex = 16;
			buttonSemiMeanAxis.ToolTipValues.Description = "Copy to clipboard: Semi-mean axis (AU)";
			buttonSemiMeanAxis.ToolTipValues.EnableToolTips = true;
			buttonSemiMeanAxis.ToolTipValues.Heading = "Semi-mean axis (AU)";
			buttonSemiMeanAxis.Values.DropDownArrowColor = Color.Empty;
			buttonSemiMeanAxis.Values.Text = "Semi-mean axis (AU)";
			buttonSemiMeanAxis.Click += ButtonSemiMeanAxis_Click;
			buttonSemiMeanAxis.Enter += Control_Enter;
			buttonSemiMeanAxis.Leave += Control_Leave;
			buttonSemiMeanAxis.MouseEnter += Control_Enter;
			buttonSemiMeanAxis.MouseLeave += Control_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(buttonStandardGravitationalParameter);
			panel.Controls.Add(buttonMeanAxis);
			panel.Controls.Add(buttonSemiMeanAxis);
			panel.Controls.Add(buttonOrbitalPerimeter);
			panel.Controls.Add(buttonOrbitalArea);
			panel.Controls.Add(buttonOrbitalPeriod);
			panel.Controls.Add(buttonLatusRectum);
			panel.Controls.Add(buttonSemiLatusRectum);
			panel.Controls.Add(buttonFocalParameter);
			panel.Controls.Add(buttonArgumentAphelion);
			panel.Controls.Add(buttonLongitudeDescendingNode);
			panel.Controls.Add(buttonAphelionDistance);
			panel.Controls.Add(buttonPerihelionDistance);
			panel.Controls.Add(buttonEccentricAnomaly);
			panel.Controls.Add(buttonTrueAnomaly);
			panel.Controls.Add(buttonMajorAxis);
			panel.Controls.Add(buttonMinorAxis);
			panel.Controls.Add(buttonLinearEccentricity);
			panel.Controls.Add(buttonSemiMinorAxis);
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
			// CopyDerivedDataToClipboardForm
			// 
			AccessibleDescription = "Copy derived data to clipboard";
			AccessibleName = "Copy derived data to clipboard";
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
			Name = "CopyDerivedDataToClipboardForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Copy derived data to clipboard";
			Load += CopyDerivedDataToClipboardForm_Load;
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

		private KryptonButton buttonLinearEccentricity;
		private KryptonButton buttonSemiMinorAxis;
		private KryptonPanel panel;
		private KryptonButton buttonEccentricAnomaly;
		private KryptonButton buttonTrueAnomaly;
		private KryptonButton buttonMajorAxis;
		private KryptonButton buttonMinorAxis;
		private KryptonButton buttonLatusRectum;
		private KryptonButton buttonSemiLatusRectum;
		private KryptonButton buttonFocalParameter;
		private KryptonButton buttonArgumentAphelion;
		private KryptonButton buttonLongitudeDescendingNode;
		private KryptonButton buttonAphelionDistance;
		private KryptonButton buttonPerihelionDistance;
		private KryptonButton buttonStandardGravitationalParameter;
		private KryptonButton buttonMeanAxis;
		private KryptonButton buttonSemiMeanAxis;
		private KryptonButton buttonOrbitalPerimeter;
		private KryptonButton buttonOrbitalArea;
		private KryptonButton buttonOrbitalPeriod;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolStripContainer toolStripContainer;
		private KryptonManager kryptonManager;
	}
}