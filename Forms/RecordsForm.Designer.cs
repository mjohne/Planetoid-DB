using System.ComponentModel;

using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class RecordsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsForm));
			panel = new KryptonPanel();
			groupBoxRecordType = new KryptonGroupBox();
			checkButtonMax = new KryptonCheckButton();
			checkButtonMin = new KryptonCheckButton();
			tableLayoutPanel = new KryptonTableLayoutPanel();
			labelElementHeader = new KryptonLabel();
			labelDesignationHeader = new KryptonLabel();
			labelValueHeader = new KryptonLabel();
			labelElement01 = new KryptonLabel();
			labelElement02 = new KryptonLabel();
			labelElement03 = new KryptonLabel();
			labelElement04 = new KryptonLabel();
			labelElement05 = new KryptonLabel();
			labelElement06 = new KryptonLabel();
			labelElement07 = new KryptonLabel();
			labelElement08 = new KryptonLabel();
			labelElement09 = new KryptonLabel();
			labelElement10 = new KryptonLabel();
			labelElement11 = new KryptonLabel();
			labelElement12 = new KryptonLabel();
			labelElement13 = new KryptonLabel();
			labelDesignation01 = new KryptonLabel();
			labelDesignation02 = new KryptonLabel();
			labelDesignation03 = new KryptonLabel();
			labelDesignation04 = new KryptonLabel();
			labelDesignation05 = new KryptonLabel();
			labelDesignation06 = new KryptonLabel();
			labelDesignation07 = new KryptonLabel();
			labelDesignation08 = new KryptonLabel();
			labelDesignation09 = new KryptonLabel();
			labelDesignation10 = new KryptonLabel();
			labelDesignation11 = new KryptonLabel();
			labelDesignation12 = new KryptonLabel();
			labelDesignation13 = new KryptonLabel();
			labelValue01 = new KryptonLabel();
			labelValue02 = new KryptonLabel();
			labelValue03 = new KryptonLabel();
			labelValue04 = new KryptonLabel();
			labelValue05 = new KryptonLabel();
			labelValue06 = new KryptonLabel();
			labelValue07 = new KryptonLabel();
			labelValue08 = new KryptonLabel();
			labelValue09 = new KryptonLabel();
			labelValue10 = new KryptonLabel();
			labelValue11 = new KryptonLabel();
			labelValue12 = new KryptonLabel();
			labelValue13 = new KryptonLabel();
			buttonStart = new KryptonButton();
			buttonCancel = new KryptonButton();
			labelPercent = new KryptonLabel();
			progressBar = new KryptonProgressBar();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			backgroundWorker = new BackgroundWorker();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			((ISupportInitialize)groupBoxRecordType).BeginInit();
			((ISupportInitialize)groupBoxRecordType.Panel).BeginInit();
			groupBoxRecordType.Panel.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			//
			// panel
			//
			panel.AccessibleDescription = "Main panel";
			panel.AccessibleName = "Main panel";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(groupBoxRecordType);
			panel.Controls.Add(tableLayoutPanel);
			panel.Controls.Add(buttonStart);
			panel.Controls.Add(buttonCancel);
			panel.Controls.Add(labelPercent);
			panel.Controls.Add(progressBar);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(700, 560);
			panel.TabIndex = 0;
			panel.TabStop = true;
			//
			// groupBoxRecordType
			//
			groupBoxRecordType.AccessibleDescription = "Groups the record type buttons";
			groupBoxRecordType.AccessibleName = "Record type";
			groupBoxRecordType.AccessibleRole = AccessibleRole.Grouping;
			groupBoxRecordType.Location = new Point(14, 10);
			groupBoxRecordType.Margin = new Padding(4, 3, 4, 3);
			//
			//
			//
			groupBoxRecordType.Panel.AccessibleDescription = "Groups the record type buttons";
			groupBoxRecordType.Panel.AccessibleName = "Record type";
			groupBoxRecordType.Panel.AccessibleRole = AccessibleRole.Grouping;
			groupBoxRecordType.Panel.Controls.Add(checkButtonMax);
			groupBoxRecordType.Panel.Controls.Add(checkButtonMin);
			groupBoxRecordType.Size = new Size(220, 60);
			groupBoxRecordType.TabIndex = 0;
			groupBoxRecordType.Values.Heading = "Record type";
			groupBoxRecordType.Values.Image = FatcowIcons16px.fatcow_award_star_gold_blue_16px;
			groupBoxRecordType.Enter += Control_Enter;
			groupBoxRecordType.Leave += Control_Leave;
			groupBoxRecordType.MouseEnter += Control_Enter;
			groupBoxRecordType.MouseLeave += Control_Leave;
			//
			// checkButtonMax
			//
			checkButtonMax.AccessibleDescription = "Selects the maximum record type";
			checkButtonMax.AccessibleName = "Maximum";
			checkButtonMax.AccessibleRole = AccessibleRole.PushButton;
			checkButtonMax.ButtonStyle = ButtonStyle.Form;
			checkButtonMax.Checked = true;
			checkButtonMax.Location = new Point(4, 7);
			checkButtonMax.Margin = new Padding(4, 3, 4, 3);
			checkButtonMax.Name = "checkButtonMax";
			checkButtonMax.Size = new Size(100, 29);
			checkButtonMax.TabIndex = 0;
			checkButtonMax.ToolTipValues.Description = "Selects the maximum record type";
			checkButtonMax.ToolTipValues.EnableToolTips = true;
			checkButtonMax.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkButtonMax.ToolTipValues.Heading = "Maximum";
			checkButtonMax.Values.DropDownArrowColor = Color.Empty;
			checkButtonMax.Values.Text = "&Maximum";
			checkButtonMax.Click += CheckButtonMax_Click;
			checkButtonMax.Enter += Control_Enter;
			checkButtonMax.Leave += Control_Leave;
			checkButtonMax.MouseEnter += Control_Enter;
			checkButtonMax.MouseLeave += Control_Leave;
			//
			// checkButtonMin
			//
			checkButtonMin.AccessibleDescription = "Selects the minimum record type";
			checkButtonMin.AccessibleName = "Minimum";
			checkButtonMin.AccessibleRole = AccessibleRole.PushButton;
			checkButtonMin.ButtonStyle = ButtonStyle.Form;
			checkButtonMin.Location = new Point(112, 7);
			checkButtonMin.Margin = new Padding(4, 3, 4, 3);
			checkButtonMin.Name = "checkButtonMin";
			checkButtonMin.Size = new Size(100, 29);
			checkButtonMin.TabIndex = 1;
			checkButtonMin.ToolTipValues.Description = "Selects the minimum record type";
			checkButtonMin.ToolTipValues.EnableToolTips = true;
			checkButtonMin.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkButtonMin.ToolTipValues.Heading = "Minimum";
			checkButtonMin.Values.DropDownArrowColor = Color.Empty;
			checkButtonMin.Values.Text = "Mi&nimum";
			checkButtonMin.Click += CheckButtonMin_Click;
			checkButtonMin.Enter += Control_Enter;
			checkButtonMin.Leave += Control_Leave;
			checkButtonMin.MouseEnter += Control_Enter;
			checkButtonMin.MouseLeave += Control_Leave;
			//
			// tableLayoutPanel
			//
			tableLayoutPanel.AccessibleDescription = "Table showing record data per orbital element";
			tableLayoutPanel.AccessibleName = "Records table";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
			tableLayoutPanel.ColumnCount = 3;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel.Controls.Add(labelElementHeader, 0, 0);
			tableLayoutPanel.Controls.Add(labelDesignationHeader, 1, 0);
			tableLayoutPanel.Controls.Add(labelValueHeader, 2, 0);
			tableLayoutPanel.Controls.Add(labelElement01, 0, 1);
			tableLayoutPanel.Controls.Add(labelDesignation01, 1, 1);
			tableLayoutPanel.Controls.Add(labelValue01, 2, 1);
			tableLayoutPanel.Controls.Add(labelElement02, 0, 2);
			tableLayoutPanel.Controls.Add(labelDesignation02, 1, 2);
			tableLayoutPanel.Controls.Add(labelValue02, 2, 2);
			tableLayoutPanel.Controls.Add(labelElement03, 0, 3);
			tableLayoutPanel.Controls.Add(labelDesignation03, 1, 3);
			tableLayoutPanel.Controls.Add(labelValue03, 2, 3);
			tableLayoutPanel.Controls.Add(labelElement04, 0, 4);
			tableLayoutPanel.Controls.Add(labelDesignation04, 1, 4);
			tableLayoutPanel.Controls.Add(labelValue04, 2, 4);
			tableLayoutPanel.Controls.Add(labelElement05, 0, 5);
			tableLayoutPanel.Controls.Add(labelDesignation05, 1, 5);
			tableLayoutPanel.Controls.Add(labelValue05, 2, 5);
			tableLayoutPanel.Controls.Add(labelElement06, 0, 6);
			tableLayoutPanel.Controls.Add(labelDesignation06, 1, 6);
			tableLayoutPanel.Controls.Add(labelValue06, 2, 6);
			tableLayoutPanel.Controls.Add(labelElement07, 0, 7);
			tableLayoutPanel.Controls.Add(labelDesignation07, 1, 7);
			tableLayoutPanel.Controls.Add(labelValue07, 2, 7);
			tableLayoutPanel.Controls.Add(labelElement08, 0, 8);
			tableLayoutPanel.Controls.Add(labelDesignation08, 1, 8);
			tableLayoutPanel.Controls.Add(labelValue08, 2, 8);
			tableLayoutPanel.Controls.Add(labelElement09, 0, 9);
			tableLayoutPanel.Controls.Add(labelDesignation09, 1, 9);
			tableLayoutPanel.Controls.Add(labelValue09, 2, 9);
			tableLayoutPanel.Controls.Add(labelElement10, 0, 10);
			tableLayoutPanel.Controls.Add(labelDesignation10, 1, 10);
			tableLayoutPanel.Controls.Add(labelValue10, 2, 10);
			tableLayoutPanel.Controls.Add(labelElement11, 0, 11);
			tableLayoutPanel.Controls.Add(labelDesignation11, 1, 11);
			tableLayoutPanel.Controls.Add(labelValue11, 2, 11);
			tableLayoutPanel.Controls.Add(labelElement12, 0, 12);
			tableLayoutPanel.Controls.Add(labelDesignation12, 1, 12);
			tableLayoutPanel.Controls.Add(labelValue12, 2, 12);
			tableLayoutPanel.Controls.Add(labelElement13, 0, 13);
			tableLayoutPanel.Controls.Add(labelDesignation13, 1, 13);
			tableLayoutPanel.Controls.Add(labelValue13, 2, 13);
			tableLayoutPanel.Location = new Point(14, 80);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.RowCount = 14;
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
			tableLayoutPanel.Size = new Size(670, 406);
			tableLayoutPanel.TabIndex = 1;
			//
			// labelElementHeader
			//
			labelElementHeader.AccessibleDescription = "Column header for orbital element names";
			labelElementHeader.AccessibleName = "Orbital element header";
			labelElementHeader.AccessibleRole = AccessibleRole.StaticText;
			labelElementHeader.Dock = DockStyle.Fill;
			labelElementHeader.LabelStyle = LabelStyle.BoldPanel;
			labelElementHeader.Location = new Point(4, 3);
			labelElementHeader.Margin = new Padding(4, 3, 4, 3);
			labelElementHeader.Name = "labelElementHeader";
			labelElementHeader.Size = new Size(212, 23);
			labelElementHeader.TabIndex = 0;
			labelElementHeader.Values.Text = "Orbital element";
			labelElementHeader.Enter += Control_Enter;
			labelElementHeader.Leave += Control_Leave;
			labelElementHeader.MouseEnter += Control_Enter;
			labelElementHeader.MouseLeave += Control_Leave;
			//
			// labelDesignationHeader
			//
			labelDesignationHeader.AccessibleDescription = "Column header for readable designations";
			labelDesignationHeader.AccessibleName = "Readable designation header";
			labelDesignationHeader.AccessibleRole = AccessibleRole.StaticText;
			labelDesignationHeader.Dock = DockStyle.Fill;
			labelDesignationHeader.LabelStyle = LabelStyle.BoldPanel;
			labelDesignationHeader.Location = new Point(224, 3);
			labelDesignationHeader.Margin = new Padding(4, 3, 4, 3);
			labelDesignationHeader.Name = "labelDesignationHeader";
			labelDesignationHeader.Size = new Size(272, 23);
			labelDesignationHeader.TabIndex = 1;
			labelDesignationHeader.Values.Text = "Readable designation";
			labelDesignationHeader.Enter += Control_Enter;
			labelDesignationHeader.Leave += Control_Leave;
			labelDesignationHeader.MouseEnter += Control_Enter;
			labelDesignationHeader.MouseLeave += Control_Leave;
			//
			// labelValueHeader
			//
			labelValueHeader.AccessibleDescription = "Column header for record values";
			labelValueHeader.AccessibleName = "Value header";
			labelValueHeader.AccessibleRole = AccessibleRole.StaticText;
			labelValueHeader.Dock = DockStyle.Fill;
			labelValueHeader.LabelStyle = LabelStyle.BoldPanel;
			labelValueHeader.Location = new Point(504, 3);
			labelValueHeader.Margin = new Padding(4, 3, 4, 3);
			labelValueHeader.Name = "labelValueHeader";
			labelValueHeader.Size = new Size(162, 23);
			labelValueHeader.TabIndex = 2;
			labelValueHeader.Values.Text = "Value";
			labelValueHeader.Enter += Control_Enter;
			labelValueHeader.Leave += Control_Leave;
			labelValueHeader.MouseEnter += Control_Enter;
			labelValueHeader.MouseLeave += Control_Leave;
			//
			// labelElement01
			//
			labelElement01.AccessibleDescription = "Orbital element: Mean Anomaly at the Epoch";
			labelElement01.AccessibleName = "Mean Anomaly at the Epoch";
			labelElement01.AccessibleRole = AccessibleRole.StaticText;
			labelElement01.Dock = DockStyle.Fill;
			labelElement01.Location = new Point(4, 32);
			labelElement01.Margin = new Padding(4, 3, 4, 3);
			labelElement01.Name = "labelElement01";
			labelElement01.Size = new Size(212, 23);
			labelElement01.TabIndex = 3;
			labelElement01.ToolTipValues.Description = "Mean Anomaly at the Epoch";
			labelElement01.ToolTipValues.EnableToolTips = true;
			labelElement01.ToolTipValues.Heading = "Mean Anomaly at the Epoch";
			labelElement01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement01.Values.Text = "Mean Anomaly at the Epoch";
			labelElement01.Enter += Control_Enter;
			labelElement01.Leave += Control_Leave;
			labelElement01.MouseEnter += Control_Enter;
			labelElement01.MouseLeave += Control_Leave;
			//
			// labelElement02
			//
			labelElement02.AccessibleDescription = "Orbital element: Argument of Perihelion";
			labelElement02.AccessibleName = "Argument of Perihelion";
			labelElement02.AccessibleRole = AccessibleRole.StaticText;
			labelElement02.Dock = DockStyle.Fill;
			labelElement02.Location = new Point(4, 61);
			labelElement02.Margin = new Padding(4, 3, 4, 3);
			labelElement02.Name = "labelElement02";
			labelElement02.Size = new Size(212, 23);
			labelElement02.TabIndex = 6;
			labelElement02.ToolTipValues.Description = "Argument of Perihelion";
			labelElement02.ToolTipValues.EnableToolTips = true;
			labelElement02.ToolTipValues.Heading = "Argument of Perihelion";
			labelElement02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement02.Values.Text = "Argument of Perihelion";
			labelElement02.Enter += Control_Enter;
			labelElement02.Leave += Control_Leave;
			labelElement02.MouseEnter += Control_Enter;
			labelElement02.MouseLeave += Control_Leave;
			//
			// labelElement03
			//
			labelElement03.AccessibleDescription = "Orbital element: Longitude of the Ascending Node";
			labelElement03.AccessibleName = "Longitude of the Ascending Node";
			labelElement03.AccessibleRole = AccessibleRole.StaticText;
			labelElement03.Dock = DockStyle.Fill;
			labelElement03.Location = new Point(4, 90);
			labelElement03.Margin = new Padding(4, 3, 4, 3);
			labelElement03.Name = "labelElement03";
			labelElement03.Size = new Size(212, 23);
			labelElement03.TabIndex = 9;
			labelElement03.ToolTipValues.Description = "Longitude of the Ascending Node";
			labelElement03.ToolTipValues.EnableToolTips = true;
			labelElement03.ToolTipValues.Heading = "Longitude of the Ascending Node";
			labelElement03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement03.Values.Text = "Longitude of the Ascending Node";
			labelElement03.Enter += Control_Enter;
			labelElement03.Leave += Control_Leave;
			labelElement03.MouseEnter += Control_Enter;
			labelElement03.MouseLeave += Control_Leave;
			//
			// labelElement04
			//
			labelElement04.AccessibleDescription = "Orbital element: Inclination to the Ecliptic";
			labelElement04.AccessibleName = "Inclination to the Ecliptic";
			labelElement04.AccessibleRole = AccessibleRole.StaticText;
			labelElement04.Dock = DockStyle.Fill;
			labelElement04.Location = new Point(4, 119);
			labelElement04.Margin = new Padding(4, 3, 4, 3);
			labelElement04.Name = "labelElement04";
			labelElement04.Size = new Size(212, 23);
			labelElement04.TabIndex = 12;
			labelElement04.ToolTipValues.Description = "Inclination to the Ecliptic";
			labelElement04.ToolTipValues.EnableToolTips = true;
			labelElement04.ToolTipValues.Heading = "Inclination to the Ecliptic";
			labelElement04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement04.Values.Text = "Inclination to the Ecliptic";
			labelElement04.Enter += Control_Enter;
			labelElement04.Leave += Control_Leave;
			labelElement04.MouseEnter += Control_Enter;
			labelElement04.MouseLeave += Control_Leave;
			//
			// labelElement05
			//
			labelElement05.AccessibleDescription = "Orbital element: Orbital Eccentricity";
			labelElement05.AccessibleName = "Orbital Eccentricity";
			labelElement05.AccessibleRole = AccessibleRole.StaticText;
			labelElement05.Dock = DockStyle.Fill;
			labelElement05.Location = new Point(4, 148);
			labelElement05.Margin = new Padding(4, 3, 4, 3);
			labelElement05.Name = "labelElement05";
			labelElement05.Size = new Size(212, 23);
			labelElement05.TabIndex = 15;
			labelElement05.ToolTipValues.Description = "Orbital Eccentricity";
			labelElement05.ToolTipValues.EnableToolTips = true;
			labelElement05.ToolTipValues.Heading = "Orbital Eccentricity";
			labelElement05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement05.Values.Text = "Orbital Eccentricity";
			labelElement05.Enter += Control_Enter;
			labelElement05.Leave += Control_Leave;
			labelElement05.MouseEnter += Control_Enter;
			labelElement05.MouseLeave += Control_Leave;
			//
			// labelElement06
			//
			labelElement06.AccessibleDescription = "Orbital element: Mean Daily Motion";
			labelElement06.AccessibleName = "Mean Daily Motion";
			labelElement06.AccessibleRole = AccessibleRole.StaticText;
			labelElement06.Dock = DockStyle.Fill;
			labelElement06.Location = new Point(4, 177);
			labelElement06.Margin = new Padding(4, 3, 4, 3);
			labelElement06.Name = "labelElement06";
			labelElement06.Size = new Size(212, 23);
			labelElement06.TabIndex = 18;
			labelElement06.ToolTipValues.Description = "Mean Daily Motion";
			labelElement06.ToolTipValues.EnableToolTips = true;
			labelElement06.ToolTipValues.Heading = "Mean Daily Motion";
			labelElement06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement06.Values.Text = "Mean Daily Motion";
			labelElement06.Enter += Control_Enter;
			labelElement06.Leave += Control_Leave;
			labelElement06.MouseEnter += Control_Enter;
			labelElement06.MouseLeave += Control_Leave;
			//
			// labelElement07
			//
			labelElement07.AccessibleDescription = "Orbital element: Semi-Major Axis";
			labelElement07.AccessibleName = "Semi-Major Axis";
			labelElement07.AccessibleRole = AccessibleRole.StaticText;
			labelElement07.Dock = DockStyle.Fill;
			labelElement07.Location = new Point(4, 206);
			labelElement07.Margin = new Padding(4, 3, 4, 3);
			labelElement07.Name = "labelElement07";
			labelElement07.Size = new Size(212, 23);
			labelElement07.TabIndex = 21;
			labelElement07.ToolTipValues.Description = "Semi-Major Axis";
			labelElement07.ToolTipValues.EnableToolTips = true;
			labelElement07.ToolTipValues.Heading = "Semi-Major Axis";
			labelElement07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement07.Values.Text = "Semi-Major Axis";
			labelElement07.Enter += Control_Enter;
			labelElement07.Leave += Control_Leave;
			labelElement07.MouseEnter += Control_Enter;
			labelElement07.MouseLeave += Control_Leave;
			//
			// labelElement08
			//
			labelElement08.AccessibleDescription = "Orbital element: Absolute Magnitude (H)";
			labelElement08.AccessibleName = "Absolute Magnitude";
			labelElement08.AccessibleRole = AccessibleRole.StaticText;
			labelElement08.Dock = DockStyle.Fill;
			labelElement08.Location = new Point(4, 235);
			labelElement08.Margin = new Padding(4, 3, 4, 3);
			labelElement08.Name = "labelElement08";
			labelElement08.Size = new Size(212, 23);
			labelElement08.TabIndex = 24;
			labelElement08.ToolTipValues.Description = "Absolute Magnitude (H)";
			labelElement08.ToolTipValues.EnableToolTips = true;
			labelElement08.ToolTipValues.Heading = "Absolute Magnitude (H)";
			labelElement08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement08.Values.Text = "Absolute Magnitude (H)";
			labelElement08.Enter += Control_Enter;
			labelElement08.Leave += Control_Leave;
			labelElement08.MouseEnter += Control_Enter;
			labelElement08.MouseLeave += Control_Leave;
			//
			// labelElement09
			//
			labelElement09.AccessibleDescription = "Orbital element: Slope Parameter (G)";
			labelElement09.AccessibleName = "Slope Parameter";
			labelElement09.AccessibleRole = AccessibleRole.StaticText;
			labelElement09.Dock = DockStyle.Fill;
			labelElement09.Location = new Point(4, 264);
			labelElement09.Margin = new Padding(4, 3, 4, 3);
			labelElement09.Name = "labelElement09";
			labelElement09.Size = new Size(212, 23);
			labelElement09.TabIndex = 27;
			labelElement09.ToolTipValues.Description = "Slope Parameter (G)";
			labelElement09.ToolTipValues.EnableToolTips = true;
			labelElement09.ToolTipValues.Heading = "Slope Parameter (G)";
			labelElement09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement09.Values.Text = "Slope Parameter (G)";
			labelElement09.Enter += Control_Enter;
			labelElement09.Leave += Control_Leave;
			labelElement09.MouseEnter += Control_Enter;
			labelElement09.MouseLeave += Control_Leave;
			//
			// labelElement10
			//
			labelElement10.AccessibleDescription = "Orbital element: Number of Oppositions";
			labelElement10.AccessibleName = "Number of Oppositions";
			labelElement10.AccessibleRole = AccessibleRole.StaticText;
			labelElement10.Dock = DockStyle.Fill;
			labelElement10.Location = new Point(4, 293);
			labelElement10.Margin = new Padding(4, 3, 4, 3);
			labelElement10.Name = "labelElement10";
			labelElement10.Size = new Size(212, 23);
			labelElement10.TabIndex = 30;
			labelElement10.ToolTipValues.Description = "Number of Oppositions";
			labelElement10.ToolTipValues.EnableToolTips = true;
			labelElement10.ToolTipValues.Heading = "Number of Oppositions";
			labelElement10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement10.Values.Text = "Number of Oppositions";
			labelElement10.Enter += Control_Enter;
			labelElement10.Leave += Control_Leave;
			labelElement10.MouseEnter += Control_Enter;
			labelElement10.MouseLeave += Control_Leave;
			//
			// labelElement11
			//
			labelElement11.AccessibleDescription = "Orbital element: Number of Observations";
			labelElement11.AccessibleName = "Number of Observations";
			labelElement11.AccessibleRole = AccessibleRole.StaticText;
			labelElement11.Dock = DockStyle.Fill;
			labelElement11.Location = new Point(4, 322);
			labelElement11.Margin = new Padding(4, 3, 4, 3);
			labelElement11.Name = "labelElement11";
			labelElement11.Size = new Size(212, 23);
			labelElement11.TabIndex = 33;
			labelElement11.ToolTipValues.Description = "Number of Observations";
			labelElement11.ToolTipValues.EnableToolTips = true;
			labelElement11.ToolTipValues.Heading = "Number of Observations";
			labelElement11.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement11.Values.Text = "Number of Observations";
			labelElement11.Enter += Control_Enter;
			labelElement11.Leave += Control_Leave;
			labelElement11.MouseEnter += Control_Enter;
			labelElement11.MouseLeave += Control_Leave;
			//
			// labelElement12
			//
			labelElement12.AccessibleDescription = "Orbital element: Observation Span";
			labelElement12.AccessibleName = "Observation Span";
			labelElement12.AccessibleRole = AccessibleRole.StaticText;
			labelElement12.Dock = DockStyle.Fill;
			labelElement12.Location = new Point(4, 351);
			labelElement12.Margin = new Padding(4, 3, 4, 3);
			labelElement12.Name = "labelElement12";
			labelElement12.Size = new Size(212, 23);
			labelElement12.TabIndex = 36;
			labelElement12.ToolTipValues.Description = "Observation Span";
			labelElement12.ToolTipValues.EnableToolTips = true;
			labelElement12.ToolTipValues.Heading = "Observation Span";
			labelElement12.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement12.Values.Text = "Observation Span";
			labelElement12.Enter += Control_Enter;
			labelElement12.Leave += Control_Leave;
			labelElement12.MouseEnter += Control_Enter;
			labelElement12.MouseLeave += Control_Leave;
			//
			// labelElement13
			//
			labelElement13.AccessibleDescription = "Orbital element: RMS Residual";
			labelElement13.AccessibleName = "RMS Residual";
			labelElement13.AccessibleRole = AccessibleRole.StaticText;
			labelElement13.Dock = DockStyle.Fill;
			labelElement13.Location = new Point(4, 380);
			labelElement13.Margin = new Padding(4, 3, 4, 3);
			labelElement13.Name = "labelElement13";
			labelElement13.Size = new Size(212, 23);
			labelElement13.TabIndex = 39;
			labelElement13.ToolTipValues.Description = "RMS Residual";
			labelElement13.ToolTipValues.EnableToolTips = true;
			labelElement13.ToolTipValues.Heading = "RMS Residual";
			labelElement13.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelElement13.Values.Text = "RMS Residual";
			labelElement13.Enter += Control_Enter;
			labelElement13.Leave += Control_Leave;
			labelElement13.MouseEnter += Control_Enter;
			labelElement13.MouseLeave += Control_Leave;
			//
			// labelDesignation01
			//
			labelDesignation01.AccessibleDescription = "Shows the readable designation for Mean Anomaly record";
			labelDesignation01.AccessibleName = "Readable designation for Mean Anomaly";
			labelDesignation01.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation01.Dock = DockStyle.Fill;
			labelDesignation01.Location = new Point(224, 32);
			labelDesignation01.Margin = new Padding(4, 3, 4, 3);
			labelDesignation01.Name = "labelDesignation01";
			labelDesignation01.Size = new Size(272, 23);
			labelDesignation01.TabIndex = 4;
			labelDesignation01.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Mean Anomaly at the Epoch.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation01.ToolTipValues.EnableToolTips = true;
			labelDesignation01.ToolTipValues.Heading = "Readable designation for Mean Anomaly";
			labelDesignation01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation01.Values.Text = "-";
			labelDesignation01.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation01.Enter += Control_Enter;
			labelDesignation01.Leave += Control_Leave;
			labelDesignation01.MouseEnter += Control_Enter;
			labelDesignation01.MouseLeave += Control_Leave;
			//
			// labelDesignation02
			//
			labelDesignation02.AccessibleDescription = "Shows the readable designation for Argument of Perihelion record";
			labelDesignation02.AccessibleName = "Readable designation for Argument of Perihelion";
			labelDesignation02.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation02.Dock = DockStyle.Fill;
			labelDesignation02.Location = new Point(224, 61);
			labelDesignation02.Margin = new Padding(4, 3, 4, 3);
			labelDesignation02.Name = "labelDesignation02";
			labelDesignation02.Size = new Size(272, 23);
			labelDesignation02.TabIndex = 7;
			labelDesignation02.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Argument of Perihelion.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation02.ToolTipValues.EnableToolTips = true;
			labelDesignation02.ToolTipValues.Heading = "Readable designation for Argument of Perihelion";
			labelDesignation02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation02.Values.Text = "-";
			labelDesignation02.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation02.Enter += Control_Enter;
			labelDesignation02.Leave += Control_Leave;
			labelDesignation02.MouseEnter += Control_Enter;
			labelDesignation02.MouseLeave += Control_Leave;
			//
			// labelDesignation03
			//
			labelDesignation03.AccessibleDescription = "Shows the readable designation for Longitude of the Ascending Node record";
			labelDesignation03.AccessibleName = "Readable designation for Longitude of the Ascending Node";
			labelDesignation03.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation03.Dock = DockStyle.Fill;
			labelDesignation03.Location = new Point(224, 90);
			labelDesignation03.Margin = new Padding(4, 3, 4, 3);
			labelDesignation03.Name = "labelDesignation03";
			labelDesignation03.Size = new Size(272, 23);
			labelDesignation03.TabIndex = 10;
			labelDesignation03.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Longitude of the Ascending Node.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation03.ToolTipValues.EnableToolTips = true;
			labelDesignation03.ToolTipValues.Heading = "Readable designation for Longitude of the Ascending Node";
			labelDesignation03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation03.Values.Text = "-";
			labelDesignation03.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation03.Enter += Control_Enter;
			labelDesignation03.Leave += Control_Leave;
			labelDesignation03.MouseEnter += Control_Enter;
			labelDesignation03.MouseLeave += Control_Leave;
			//
			// labelDesignation04
			//
			labelDesignation04.AccessibleDescription = "Shows the readable designation for Inclination to the Ecliptic record";
			labelDesignation04.AccessibleName = "Readable designation for Inclination to the Ecliptic";
			labelDesignation04.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation04.Dock = DockStyle.Fill;
			labelDesignation04.Location = new Point(224, 119);
			labelDesignation04.Margin = new Padding(4, 3, 4, 3);
			labelDesignation04.Name = "labelDesignation04";
			labelDesignation04.Size = new Size(272, 23);
			labelDesignation04.TabIndex = 13;
			labelDesignation04.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Inclination to the Ecliptic.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation04.ToolTipValues.EnableToolTips = true;
			labelDesignation04.ToolTipValues.Heading = "Readable designation for Inclination to the Ecliptic";
			labelDesignation04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation04.Values.Text = "-";
			labelDesignation04.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation04.Enter += Control_Enter;
			labelDesignation04.Leave += Control_Leave;
			labelDesignation04.MouseEnter += Control_Enter;
			labelDesignation04.MouseLeave += Control_Leave;
			//
			// labelDesignation05
			//
			labelDesignation05.AccessibleDescription = "Shows the readable designation for Orbital Eccentricity record";
			labelDesignation05.AccessibleName = "Readable designation for Orbital Eccentricity";
			labelDesignation05.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation05.Dock = DockStyle.Fill;
			labelDesignation05.Location = new Point(224, 148);
			labelDesignation05.Margin = new Padding(4, 3, 4, 3);
			labelDesignation05.Name = "labelDesignation05";
			labelDesignation05.Size = new Size(272, 23);
			labelDesignation05.TabIndex = 16;
			labelDesignation05.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Orbital Eccentricity.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation05.ToolTipValues.EnableToolTips = true;
			labelDesignation05.ToolTipValues.Heading = "Readable designation for Orbital Eccentricity";
			labelDesignation05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation05.Values.Text = "-";
			labelDesignation05.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation05.Enter += Control_Enter;
			labelDesignation05.Leave += Control_Leave;
			labelDesignation05.MouseEnter += Control_Enter;
			labelDesignation05.MouseLeave += Control_Leave;
			//
			// labelDesignation06
			//
			labelDesignation06.AccessibleDescription = "Shows the readable designation for Mean Daily Motion record";
			labelDesignation06.AccessibleName = "Readable designation for Mean Daily Motion";
			labelDesignation06.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation06.Dock = DockStyle.Fill;
			labelDesignation06.Location = new Point(224, 177);
			labelDesignation06.Margin = new Padding(4, 3, 4, 3);
			labelDesignation06.Name = "labelDesignation06";
			labelDesignation06.Size = new Size(272, 23);
			labelDesignation06.TabIndex = 19;
			labelDesignation06.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Mean Daily Motion.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation06.ToolTipValues.EnableToolTips = true;
			labelDesignation06.ToolTipValues.Heading = "Readable designation for Mean Daily Motion";
			labelDesignation06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation06.Values.Text = "-";
			labelDesignation06.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation06.Enter += Control_Enter;
			labelDesignation06.Leave += Control_Leave;
			labelDesignation06.MouseEnter += Control_Enter;
			labelDesignation06.MouseLeave += Control_Leave;
			//
			// labelDesignation07
			//
			labelDesignation07.AccessibleDescription = "Shows the readable designation for Semi-Major Axis record";
			labelDesignation07.AccessibleName = "Readable designation for Semi-Major Axis";
			labelDesignation07.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation07.Dock = DockStyle.Fill;
			labelDesignation07.Location = new Point(224, 206);
			labelDesignation07.Margin = new Padding(4, 3, 4, 3);
			labelDesignation07.Name = "labelDesignation07";
			labelDesignation07.Size = new Size(272, 23);
			labelDesignation07.TabIndex = 22;
			labelDesignation07.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Semi-Major Axis.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation07.ToolTipValues.EnableToolTips = true;
			labelDesignation07.ToolTipValues.Heading = "Readable designation for Semi-Major Axis";
			labelDesignation07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation07.Values.Text = "-";
			labelDesignation07.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation07.Enter += Control_Enter;
			labelDesignation07.Leave += Control_Leave;
			labelDesignation07.MouseEnter += Control_Enter;
			labelDesignation07.MouseLeave += Control_Leave;
			//
			// labelDesignation08
			//
			labelDesignation08.AccessibleDescription = "Shows the readable designation for Absolute Magnitude record";
			labelDesignation08.AccessibleName = "Readable designation for Absolute Magnitude";
			labelDesignation08.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation08.Dock = DockStyle.Fill;
			labelDesignation08.Location = new Point(224, 235);
			labelDesignation08.Margin = new Padding(4, 3, 4, 3);
			labelDesignation08.Name = "labelDesignation08";
			labelDesignation08.Size = new Size(272, 23);
			labelDesignation08.TabIndex = 25;
			labelDesignation08.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Absolute Magnitude.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation08.ToolTipValues.EnableToolTips = true;
			labelDesignation08.ToolTipValues.Heading = "Readable designation for Absolute Magnitude";
			labelDesignation08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation08.Values.Text = "-";
			labelDesignation08.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation08.Enter += Control_Enter;
			labelDesignation08.Leave += Control_Leave;
			labelDesignation08.MouseEnter += Control_Enter;
			labelDesignation08.MouseLeave += Control_Leave;
			//
			// labelDesignation09
			//
			labelDesignation09.AccessibleDescription = "Shows the readable designation for Slope Parameter record";
			labelDesignation09.AccessibleName = "Readable designation for Slope Parameter";
			labelDesignation09.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation09.Dock = DockStyle.Fill;
			labelDesignation09.Location = new Point(224, 264);
			labelDesignation09.Margin = new Padding(4, 3, 4, 3);
			labelDesignation09.Name = "labelDesignation09";
			labelDesignation09.Size = new Size(272, 23);
			labelDesignation09.TabIndex = 28;
			labelDesignation09.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Slope Parameter.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation09.ToolTipValues.EnableToolTips = true;
			labelDesignation09.ToolTipValues.Heading = "Readable designation for Slope Parameter";
			labelDesignation09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation09.Values.Text = "-";
			labelDesignation09.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation09.Enter += Control_Enter;
			labelDesignation09.Leave += Control_Leave;
			labelDesignation09.MouseEnter += Control_Enter;
			labelDesignation09.MouseLeave += Control_Leave;
			//
			// labelDesignation10
			//
			labelDesignation10.AccessibleDescription = "Shows the readable designation for Number of Oppositions record";
			labelDesignation10.AccessibleName = "Readable designation for Number of Oppositions";
			labelDesignation10.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation10.Dock = DockStyle.Fill;
			labelDesignation10.Location = new Point(224, 293);
			labelDesignation10.Margin = new Padding(4, 3, 4, 3);
			labelDesignation10.Name = "labelDesignation10";
			labelDesignation10.Size = new Size(272, 23);
			labelDesignation10.TabIndex = 31;
			labelDesignation10.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Number of Oppositions.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation10.ToolTipValues.EnableToolTips = true;
			labelDesignation10.ToolTipValues.Heading = "Readable designation for Number of Oppositions";
			labelDesignation10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation10.Values.Text = "-";
			labelDesignation10.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation10.Enter += Control_Enter;
			labelDesignation10.Leave += Control_Leave;
			labelDesignation10.MouseEnter += Control_Enter;
			labelDesignation10.MouseLeave += Control_Leave;
			//
			// labelDesignation11
			//
			labelDesignation11.AccessibleDescription = "Shows the readable designation for Number of Observations record";
			labelDesignation11.AccessibleName = "Readable designation for Number of Observations";
			labelDesignation11.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation11.Dock = DockStyle.Fill;
			labelDesignation11.Location = new Point(224, 322);
			labelDesignation11.Margin = new Padding(4, 3, 4, 3);
			labelDesignation11.Name = "labelDesignation11";
			labelDesignation11.Size = new Size(272, 23);
			labelDesignation11.TabIndex = 34;
			labelDesignation11.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Number of Observations.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation11.ToolTipValues.EnableToolTips = true;
			labelDesignation11.ToolTipValues.Heading = "Readable designation for Number of Observations";
			labelDesignation11.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation11.Values.Text = "-";
			labelDesignation11.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation11.Enter += Control_Enter;
			labelDesignation11.Leave += Control_Leave;
			labelDesignation11.MouseEnter += Control_Enter;
			labelDesignation11.MouseLeave += Control_Leave;
			//
			// labelDesignation12
			//
			labelDesignation12.AccessibleDescription = "Shows the readable designation for Observation Span record";
			labelDesignation12.AccessibleName = "Readable designation for Observation Span";
			labelDesignation12.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation12.Dock = DockStyle.Fill;
			labelDesignation12.Location = new Point(224, 351);
			labelDesignation12.Margin = new Padding(4, 3, 4, 3);
			labelDesignation12.Name = "labelDesignation12";
			labelDesignation12.Size = new Size(272, 23);
			labelDesignation12.TabIndex = 37;
			labelDesignation12.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for Observation Span.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation12.ToolTipValues.EnableToolTips = true;
			labelDesignation12.ToolTipValues.Heading = "Readable designation for Observation Span";
			labelDesignation12.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation12.Values.Text = "-";
			labelDesignation12.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation12.Enter += Control_Enter;
			labelDesignation12.Leave += Control_Leave;
			labelDesignation12.MouseEnter += Control_Enter;
			labelDesignation12.MouseLeave += Control_Leave;
			//
			// labelDesignation13
			//
			labelDesignation13.AccessibleDescription = "Shows the readable designation for RMS Residual record";
			labelDesignation13.AccessibleName = "Readable designation for RMS Residual";
			labelDesignation13.AccessibleRole = AccessibleRole.StaticText;
			labelDesignation13.Dock = DockStyle.Fill;
			labelDesignation13.Location = new Point(224, 380);
			labelDesignation13.Margin = new Padding(4, 3, 4, 3);
			labelDesignation13.Name = "labelDesignation13";
			labelDesignation13.Size = new Size(272, 23);
			labelDesignation13.TabIndex = 40;
			labelDesignation13.ToolTipValues.Description = "Shows the readable designation of the asteroid with the record value for RMS Residual.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelDesignation13.ToolTipValues.EnableToolTips = true;
			labelDesignation13.ToolTipValues.Heading = "Readable designation for RMS Residual";
			labelDesignation13.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelDesignation13.Values.Text = "-";
			labelDesignation13.DoubleClick += CopyToClipboard_DoubleClick;
			labelDesignation13.Enter += Control_Enter;
			labelDesignation13.Leave += Control_Leave;
			labelDesignation13.MouseEnter += Control_Enter;
			labelDesignation13.MouseLeave += Control_Leave;
			//
			// labelValue01
			//
			labelValue01.AccessibleDescription = "Shows the record value for Mean Anomaly at the Epoch";
			labelValue01.AccessibleName = "Record value for Mean Anomaly at the Epoch";
			labelValue01.AccessibleRole = AccessibleRole.StaticText;
			labelValue01.Dock = DockStyle.Fill;
			labelValue01.Location = new Point(504, 32);
			labelValue01.Margin = new Padding(4, 3, 4, 3);
			labelValue01.Name = "labelValue01";
			labelValue01.Size = new Size(162, 23);
			labelValue01.TabIndex = 5;
			labelValue01.ToolTipValues.Description = "Shows the record value for Mean Anomaly at the Epoch.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue01.ToolTipValues.EnableToolTips = true;
			labelValue01.ToolTipValues.Heading = "Record value for Mean Anomaly at the Epoch";
			labelValue01.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue01.Values.Text = "-";
			labelValue01.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue01.Enter += Control_Enter;
			labelValue01.Leave += Control_Leave;
			labelValue01.MouseEnter += Control_Enter;
			labelValue01.MouseLeave += Control_Leave;
			//
			// labelValue02
			//
			labelValue02.AccessibleDescription = "Shows the record value for Argument of Perihelion";
			labelValue02.AccessibleName = "Record value for Argument of Perihelion";
			labelValue02.AccessibleRole = AccessibleRole.StaticText;
			labelValue02.Dock = DockStyle.Fill;
			labelValue02.Location = new Point(504, 61);
			labelValue02.Margin = new Padding(4, 3, 4, 3);
			labelValue02.Name = "labelValue02";
			labelValue02.Size = new Size(162, 23);
			labelValue02.TabIndex = 8;
			labelValue02.ToolTipValues.Description = "Shows the record value for Argument of Perihelion.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue02.ToolTipValues.EnableToolTips = true;
			labelValue02.ToolTipValues.Heading = "Record value for Argument of Perihelion";
			labelValue02.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue02.Values.Text = "-";
			labelValue02.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue02.Enter += Control_Enter;
			labelValue02.Leave += Control_Leave;
			labelValue02.MouseEnter += Control_Enter;
			labelValue02.MouseLeave += Control_Leave;
			//
			// labelValue03
			//
			labelValue03.AccessibleDescription = "Shows the record value for Longitude of the Ascending Node";
			labelValue03.AccessibleName = "Record value for Longitude of the Ascending Node";
			labelValue03.AccessibleRole = AccessibleRole.StaticText;
			labelValue03.Dock = DockStyle.Fill;
			labelValue03.Location = new Point(504, 90);
			labelValue03.Margin = new Padding(4, 3, 4, 3);
			labelValue03.Name = "labelValue03";
			labelValue03.Size = new Size(162, 23);
			labelValue03.TabIndex = 11;
			labelValue03.ToolTipValues.Description = "Shows the record value for Longitude of the Ascending Node.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue03.ToolTipValues.EnableToolTips = true;
			labelValue03.ToolTipValues.Heading = "Record value for Longitude of the Ascending Node";
			labelValue03.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue03.Values.Text = "-";
			labelValue03.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue03.Enter += Control_Enter;
			labelValue03.Leave += Control_Leave;
			labelValue03.MouseEnter += Control_Enter;
			labelValue03.MouseLeave += Control_Leave;
			//
			// labelValue04
			//
			labelValue04.AccessibleDescription = "Shows the record value for Inclination to the Ecliptic";
			labelValue04.AccessibleName = "Record value for Inclination to the Ecliptic";
			labelValue04.AccessibleRole = AccessibleRole.StaticText;
			labelValue04.Dock = DockStyle.Fill;
			labelValue04.Location = new Point(504, 119);
			labelValue04.Margin = new Padding(4, 3, 4, 3);
			labelValue04.Name = "labelValue04";
			labelValue04.Size = new Size(162, 23);
			labelValue04.TabIndex = 14;
			labelValue04.ToolTipValues.Description = "Shows the record value for Inclination to the Ecliptic.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue04.ToolTipValues.EnableToolTips = true;
			labelValue04.ToolTipValues.Heading = "Record value for Inclination to the Ecliptic";
			labelValue04.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue04.Values.Text = "-";
			labelValue04.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue04.Enter += Control_Enter;
			labelValue04.Leave += Control_Leave;
			labelValue04.MouseEnter += Control_Enter;
			labelValue04.MouseLeave += Control_Leave;
			//
			// labelValue05
			//
			labelValue05.AccessibleDescription = "Shows the record value for Orbital Eccentricity";
			labelValue05.AccessibleName = "Record value for Orbital Eccentricity";
			labelValue05.AccessibleRole = AccessibleRole.StaticText;
			labelValue05.Dock = DockStyle.Fill;
			labelValue05.Location = new Point(504, 148);
			labelValue05.Margin = new Padding(4, 3, 4, 3);
			labelValue05.Name = "labelValue05";
			labelValue05.Size = new Size(162, 23);
			labelValue05.TabIndex = 17;
			labelValue05.ToolTipValues.Description = "Shows the record value for Orbital Eccentricity.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue05.ToolTipValues.EnableToolTips = true;
			labelValue05.ToolTipValues.Heading = "Record value for Orbital Eccentricity";
			labelValue05.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue05.Values.Text = "-";
			labelValue05.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue05.Enter += Control_Enter;
			labelValue05.Leave += Control_Leave;
			labelValue05.MouseEnter += Control_Enter;
			labelValue05.MouseLeave += Control_Leave;
			//
			// labelValue06
			//
			labelValue06.AccessibleDescription = "Shows the record value for Mean Daily Motion";
			labelValue06.AccessibleName = "Record value for Mean Daily Motion";
			labelValue06.AccessibleRole = AccessibleRole.StaticText;
			labelValue06.Dock = DockStyle.Fill;
			labelValue06.Location = new Point(504, 177);
			labelValue06.Margin = new Padding(4, 3, 4, 3);
			labelValue06.Name = "labelValue06";
			labelValue06.Size = new Size(162, 23);
			labelValue06.TabIndex = 20;
			labelValue06.ToolTipValues.Description = "Shows the record value for Mean Daily Motion.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue06.ToolTipValues.EnableToolTips = true;
			labelValue06.ToolTipValues.Heading = "Record value for Mean Daily Motion";
			labelValue06.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue06.Values.Text = "-";
			labelValue06.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue06.Enter += Control_Enter;
			labelValue06.Leave += Control_Leave;
			labelValue06.MouseEnter += Control_Enter;
			labelValue06.MouseLeave += Control_Leave;
			//
			// labelValue07
			//
			labelValue07.AccessibleDescription = "Shows the record value for Semi-Major Axis";
			labelValue07.AccessibleName = "Record value for Semi-Major Axis";
			labelValue07.AccessibleRole = AccessibleRole.StaticText;
			labelValue07.Dock = DockStyle.Fill;
			labelValue07.Location = new Point(504, 206);
			labelValue07.Margin = new Padding(4, 3, 4, 3);
			labelValue07.Name = "labelValue07";
			labelValue07.Size = new Size(162, 23);
			labelValue07.TabIndex = 23;
			labelValue07.ToolTipValues.Description = "Shows the record value for Semi-Major Axis.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue07.ToolTipValues.EnableToolTips = true;
			labelValue07.ToolTipValues.Heading = "Record value for Semi-Major Axis";
			labelValue07.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue07.Values.Text = "-";
			labelValue07.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue07.Enter += Control_Enter;
			labelValue07.Leave += Control_Leave;
			labelValue07.MouseEnter += Control_Enter;
			labelValue07.MouseLeave += Control_Leave;
			//
			// labelValue08
			//
			labelValue08.AccessibleDescription = "Shows the record value for Absolute Magnitude";
			labelValue08.AccessibleName = "Record value for Absolute Magnitude";
			labelValue08.AccessibleRole = AccessibleRole.StaticText;
			labelValue08.Dock = DockStyle.Fill;
			labelValue08.Location = new Point(504, 235);
			labelValue08.Margin = new Padding(4, 3, 4, 3);
			labelValue08.Name = "labelValue08";
			labelValue08.Size = new Size(162, 23);
			labelValue08.TabIndex = 26;
			labelValue08.ToolTipValues.Description = "Shows the record value for Absolute Magnitude.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue08.ToolTipValues.EnableToolTips = true;
			labelValue08.ToolTipValues.Heading = "Record value for Absolute Magnitude";
			labelValue08.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue08.Values.Text = "-";
			labelValue08.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue08.Enter += Control_Enter;
			labelValue08.Leave += Control_Leave;
			labelValue08.MouseEnter += Control_Enter;
			labelValue08.MouseLeave += Control_Leave;
			//
			// labelValue09
			//
			labelValue09.AccessibleDescription = "Shows the record value for Slope Parameter";
			labelValue09.AccessibleName = "Record value for Slope Parameter";
			labelValue09.AccessibleRole = AccessibleRole.StaticText;
			labelValue09.Dock = DockStyle.Fill;
			labelValue09.Location = new Point(504, 264);
			labelValue09.Margin = new Padding(4, 3, 4, 3);
			labelValue09.Name = "labelValue09";
			labelValue09.Size = new Size(162, 23);
			labelValue09.TabIndex = 29;
			labelValue09.ToolTipValues.Description = "Shows the record value for Slope Parameter.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue09.ToolTipValues.EnableToolTips = true;
			labelValue09.ToolTipValues.Heading = "Record value for Slope Parameter";
			labelValue09.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue09.Values.Text = "-";
			labelValue09.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue09.Enter += Control_Enter;
			labelValue09.Leave += Control_Leave;
			labelValue09.MouseEnter += Control_Enter;
			labelValue09.MouseLeave += Control_Leave;
			//
			// labelValue10
			//
			labelValue10.AccessibleDescription = "Shows the record value for Number of Oppositions";
			labelValue10.AccessibleName = "Record value for Number of Oppositions";
			labelValue10.AccessibleRole = AccessibleRole.StaticText;
			labelValue10.Dock = DockStyle.Fill;
			labelValue10.Location = new Point(504, 293);
			labelValue10.Margin = new Padding(4, 3, 4, 3);
			labelValue10.Name = "labelValue10";
			labelValue10.Size = new Size(162, 23);
			labelValue10.TabIndex = 32;
			labelValue10.ToolTipValues.Description = "Shows the record value for Number of Oppositions.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue10.ToolTipValues.EnableToolTips = true;
			labelValue10.ToolTipValues.Heading = "Record value for Number of Oppositions";
			labelValue10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue10.Values.Text = "-";
			labelValue10.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue10.Enter += Control_Enter;
			labelValue10.Leave += Control_Leave;
			labelValue10.MouseEnter += Control_Enter;
			labelValue10.MouseLeave += Control_Leave;
			//
			// labelValue11
			//
			labelValue11.AccessibleDescription = "Shows the record value for Number of Observations";
			labelValue11.AccessibleName = "Record value for Number of Observations";
			labelValue11.AccessibleRole = AccessibleRole.StaticText;
			labelValue11.Dock = DockStyle.Fill;
			labelValue11.Location = new Point(504, 322);
			labelValue11.Margin = new Padding(4, 3, 4, 3);
			labelValue11.Name = "labelValue11";
			labelValue11.Size = new Size(162, 23);
			labelValue11.TabIndex = 35;
			labelValue11.ToolTipValues.Description = "Shows the record value for Number of Observations.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue11.ToolTipValues.EnableToolTips = true;
			labelValue11.ToolTipValues.Heading = "Record value for Number of Observations";
			labelValue11.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue11.Values.Text = "-";
			labelValue11.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue11.Enter += Control_Enter;
			labelValue11.Leave += Control_Leave;
			labelValue11.MouseEnter += Control_Enter;
			labelValue11.MouseLeave += Control_Leave;
			//
			// labelValue12
			//
			labelValue12.AccessibleDescription = "Shows the record value for Observation Span";
			labelValue12.AccessibleName = "Record value for Observation Span";
			labelValue12.AccessibleRole = AccessibleRole.StaticText;
			labelValue12.Dock = DockStyle.Fill;
			labelValue12.Location = new Point(504, 351);
			labelValue12.Margin = new Padding(4, 3, 4, 3);
			labelValue12.Name = "labelValue12";
			labelValue12.Size = new Size(162, 23);
			labelValue12.TabIndex = 38;
			labelValue12.ToolTipValues.Description = "Shows the record value for Observation Span.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue12.ToolTipValues.EnableToolTips = true;
			labelValue12.ToolTipValues.Heading = "Record value for Observation Span";
			labelValue12.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue12.Values.Text = "-";
			labelValue12.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue12.Enter += Control_Enter;
			labelValue12.Leave += Control_Leave;
			labelValue12.MouseEnter += Control_Enter;
			labelValue12.MouseLeave += Control_Leave;
			//
			// labelValue13
			//
			labelValue13.AccessibleDescription = "Shows the record value for RMS Residual";
			labelValue13.AccessibleName = "Record value for RMS Residual";
			labelValue13.AccessibleRole = AccessibleRole.StaticText;
			labelValue13.Dock = DockStyle.Fill;
			labelValue13.Location = new Point(504, 380);
			labelValue13.Margin = new Padding(4, 3, 4, 3);
			labelValue13.Name = "labelValue13";
			labelValue13.Size = new Size(162, 23);
			labelValue13.TabIndex = 41;
			labelValue13.ToolTipValues.Description = "Shows the record value for RMS Residual.\r\nDouble-click or right-click to copy the information to the clipboard.";
			labelValue13.ToolTipValues.EnableToolTips = true;
			labelValue13.ToolTipValues.Heading = "Record value for RMS Residual";
			labelValue13.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelValue13.Values.Text = "-";
			labelValue13.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue13.Enter += Control_Enter;
			labelValue13.Leave += Control_Leave;
			labelValue13.MouseEnter += Control_Enter;
			labelValue13.MouseLeave += Control_Leave;
			//
			// buttonStart
			//
			buttonStart.AccessibleDescription = "Starts the record detection scan";
			buttonStart.AccessibleName = "Start scan";
			buttonStart.AccessibleRole = AccessibleRole.PushButton;
			buttonStart.Location = new Point(14, 498);
			buttonStart.Margin = new Padding(4, 3, 4, 3);
			buttonStart.Name = "buttonStart";
			buttonStart.Size = new Size(85, 29);
			buttonStart.TabIndex = 2;
			buttonStart.ToolTipValues.Description = "Starts the record detection scan";
			buttonStart.ToolTipValues.EnableToolTips = true;
			buttonStart.ToolTipValues.Heading = "Start";
			buttonStart.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonStart.Values.DropDownArrowColor = Color.Empty;
			buttonStart.Values.Image = FatcowIcons16px.fatcow_resultset_next_16px;
			buttonStart.Values.Text = "&Start";
			buttonStart.Click += ButtonStart_Click;
			buttonStart.Enter += Control_Enter;
			buttonStart.Leave += Control_Leave;
			buttonStart.MouseEnter += Control_Enter;
			buttonStart.MouseLeave += Control_Leave;
			//
			// buttonCancel
			//
			buttonCancel.AccessibleDescription = "Cancels the record detection scan";
			buttonCancel.AccessibleName = "Cancel scan";
			buttonCancel.AccessibleRole = AccessibleRole.PushButton;
			buttonCancel.Enabled = false;
			buttonCancel.Location = new Point(107, 498);
			buttonCancel.Margin = new Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(85, 29);
			buttonCancel.TabIndex = 3;
			buttonCancel.ToolTipValues.Description = "Cancels the record detection scan";
			buttonCancel.ToolTipValues.EnableToolTips = true;
			buttonCancel.ToolTipValues.Heading = "Cancel";
			buttonCancel.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += Control_Enter;
			buttonCancel.Leave += Control_Leave;
			buttonCancel.MouseEnter += Control_Enter;
			buttonCancel.MouseLeave += Control_Leave;
			//
			// labelPercent
			//
			labelPercent.AccessibleDescription = "Shows the percent status of the record detection";
			labelPercent.AccessibleName = "Percent status of the record detection";
			labelPercent.AccessibleRole = AccessibleRole.StaticText;
			labelPercent.Location = new Point(608, 504);
			labelPercent.Margin = new Padding(4, 3, 4, 3);
			labelPercent.Name = "labelPercent";
			labelPercent.Size = new Size(56, 20);
			labelPercent.TabIndex = 5;
			labelPercent.ToolTipValues.Description = "Shows the percent status of the record detection";
			labelPercent.ToolTipValues.EnableToolTips = true;
			labelPercent.ToolTipValues.Heading = "Percent status";
			labelPercent.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			labelPercent.Values.Text = "0 %";
			labelPercent.Enter += Control_Enter;
			labelPercent.Leave += Control_Leave;
			labelPercent.MouseEnter += Control_Enter;
			labelPercent.MouseLeave += Control_Leave;
			//
			// progressBar
			//
			progressBar.AccessibleDescription = "Shows the progress of the record detection";
			progressBar.AccessibleName = "Progress bar";
			progressBar.AccessibleRole = AccessibleRole.ProgressBar;
			progressBar.Enabled = false;
			progressBar.Location = new Point(200, 498);
			progressBar.Margin = new Padding(4, 3, 4, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(400, 29);
			progressBar.TabIndex = 4;
			progressBar.Text = "0 %";
			progressBar.TextBackdropColor = Color.Empty;
			progressBar.TextShadowColor = Color.Empty;
			progressBar.MouseEnter += Control_Enter;
			progressBar.MouseLeave += Control_Leave;
			//
			// statusStrip
			//
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 538);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(700, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 6;
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
			labelInformation.ToolTipText = "Show some information";
			//
			// kryptonManager
			//
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			//
			// backgroundWorker
			//
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			//
			// RecordsForm
			//
			AccessibleDescription = "Shows the record values for all orbital elements";
			AccessibleName = "Record list";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(700, 560);
			Controls.Add(statusStrip);
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "RecordsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Record list";
			Load += RecordsForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			((ISupportInitialize)groupBoxRecordType.Panel).EndInit();
			groupBoxRecordType.Panel.ResumeLayout(false);
			((ISupportInitialize)groupBoxRecordType).EndInit();
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private KryptonPanel panel;
		private KryptonGroupBox groupBoxRecordType;
		private KryptonCheckButton checkButtonMax;
		private KryptonCheckButton checkButtonMin;
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonLabel labelElementHeader;
		private KryptonLabel labelDesignationHeader;
		private KryptonLabel labelValueHeader;
		private KryptonLabel labelElement01;
		private KryptonLabel labelElement02;
		private KryptonLabel labelElement03;
		private KryptonLabel labelElement04;
		private KryptonLabel labelElement05;
		private KryptonLabel labelElement06;
		private KryptonLabel labelElement07;
		private KryptonLabel labelElement08;
		private KryptonLabel labelElement09;
		private KryptonLabel labelElement10;
		private KryptonLabel labelElement11;
		private KryptonLabel labelElement12;
		private KryptonLabel labelElement13;
		private KryptonLabel labelDesignation01;
		private KryptonLabel labelDesignation02;
		private KryptonLabel labelDesignation03;
		private KryptonLabel labelDesignation04;
		private KryptonLabel labelDesignation05;
		private KryptonLabel labelDesignation06;
		private KryptonLabel labelDesignation07;
		private KryptonLabel labelDesignation08;
		private KryptonLabel labelDesignation09;
		private KryptonLabel labelDesignation10;
		private KryptonLabel labelDesignation11;
		private KryptonLabel labelDesignation12;
		private KryptonLabel labelDesignation13;
		private KryptonLabel labelValue01;
		private KryptonLabel labelValue02;
		private KryptonLabel labelValue03;
		private KryptonLabel labelValue04;
		private KryptonLabel labelValue05;
		private KryptonLabel labelValue06;
		private KryptonLabel labelValue07;
		private KryptonLabel labelValue08;
		private KryptonLabel labelValue09;
		private KryptonLabel labelValue10;
		private KryptonLabel labelValue11;
		private KryptonLabel labelValue12;
		private KryptonLabel labelValue13;
		private KryptonButton buttonStart;
		private KryptonButton buttonCancel;
		private KryptonLabel labelPercent;
		private KryptonProgressBar progressBar;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
		private BackgroundWorker backgroundWorker;
	}
}
