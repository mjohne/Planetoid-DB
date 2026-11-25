namespace Planetoid_DB.Forms;

partial class SingleAsteroidForm
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

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
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleAsteroidForm));
		toolStripContainer = new ToolStripContainer();
		statusStrip = new Krypton.Toolkit.KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		panelMain = new Krypton.Toolkit.KryptonPanel();
		toolStripMain = new Krypton.Toolkit.KryptonToolStrip();
		tableLayoutPanelMpcorbData = new Krypton.Toolkit.KryptonTableLayoutPanel();
		labelMpcorbIndexData = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbIndex = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbReadableDesignation = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbEpoch = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbMeanAnomalyAtTheEpoch = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbArgumentOfThePerihelion = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbLongitudeOfTheAscendingNode = new Krypton.Toolkit.KryptonLabel();
		labelInclinationToTheEcliptic = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbOrbitalEccentricity = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbMeanDailyMotion = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbSemiMajorAxis = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbAbsoluteMagnitude = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbSlopeParameter = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbReference = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbNumberOfOppositions = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbNumberOfObservations = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbObservationSpan = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbRmsResidual = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbComputerName = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbFlags = new Krypton.Toolkit.KryptonLabel();
		labelMpcorbDateLastObservation = new Krypton.Toolkit.KryptonLabel();
		textBoxMpcorbReadableDesignation = new Krypton.Toolkit.KryptonTextBox();
		dateTimePickerMpcorbEpoch = new Krypton.Toolkit.KryptonDateTimePicker();
		numericUpDownMpcorbMeanAnomalyAtTheEpoch = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbArgumentOfThePerihelion = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbLongitudeOfTheAscendingNode = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownInclinationToTheEcliptic = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbOrbitalEccentricity = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbMeanDailyMotion = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbSemiMajorAxis = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbAbsoluteMagnitude = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbSlopeParameter = new Krypton.Toolkit.KryptonNumericUpDown();
		textBoxMpcorbReference = new Krypton.Toolkit.KryptonTextBox();
		numericUpDownMpcorbNumberOfOppositions = new Krypton.Toolkit.KryptonNumericUpDown();
		numericUpDownMpcorbNumberOfObservations = new Krypton.Toolkit.KryptonNumericUpDown();
		textBoxMpcorbObservationSpan = new Krypton.Toolkit.KryptonTextBox();
		numericUpDownMpcorbRmsResidual = new Krypton.Toolkit.KryptonNumericUpDown();
		textBoxMpcorbComputerName = new Krypton.Toolkit.KryptonTextBox();
		textBoxMpcorbFlags = new Krypton.Toolkit.KryptonTextBox();
		textBoxMpcorbDateLastObservation = new Krypton.Toolkit.KryptonTextBox();
		dropDownButtonSaveList = new ToolStripDropDownButton();
		kryptonManager = new Krypton.Toolkit.KryptonManager(components);
		contextMenuShowControlsInStatusBar = new ContextMenuStrip(components);
		menuItemShowInformationLabel = new ToolStripMenuItem();
		contextMenuSaveToFile = new ContextMenuStrip(components);
		menuItemTextFiles = new ToolStripMenuItem();
		menuItemSaveAsText = new ToolStripMenuItem();
		menuItemSaveAsLatex = new ToolStripMenuItem();
		menuItemSaveAsMarkdown = new ToolStripMenuItem();
		menuItemSaveAsAsciiDoc = new ToolStripMenuItem();
		menuItemSaveAsReStructuredText = new ToolStripMenuItem();
		menuItemSaveAsTextile = new ToolStripMenuItem();
		menuItemSaveAsTypst = new ToolStripMenuItem();
		menuItemWriterDocuments = new ToolStripMenuItem();
		menuItemSaveAsWord = new ToolStripMenuItem();
		menuItemSaveAsOdt = new ToolStripMenuItem();
		menuItemSaveAsRtf = new ToolStripMenuItem();
		menuItemSaveAsAbiword = new ToolStripMenuItem();
		menuItemSaveAsWps = new ToolStripMenuItem();
		menuItemSpreadsheetDocuments = new ToolStripMenuItem();
		menuItemSaveAsExcel = new ToolStripMenuItem();
		menuItemSaveAsOds = new ToolStripMenuItem();
		menuItemSaveAsCsv = new ToolStripMenuItem();
		menuItemSaveAsTsv = new ToolStripMenuItem();
		menuItemSaveAsPsv = new ToolStripMenuItem();
		menuItemSaveAsEt = new ToolStripMenuItem();
		menuItemXmlDocuments = new ToolStripMenuItem();
		menuItemSaveAsHtml = new ToolStripMenuItem();
		menuItemSaveAsXml = new ToolStripMenuItem();
		menuItemSaveAsDocBook = new ToolStripMenuItem();
		menuItemConfigurationFiles = new ToolStripMenuItem();
		menuItemSaveAsJson = new ToolStripMenuItem();
		menuItemSaveAsYaml = new ToolStripMenuItem();
		menuItemSaveAsToml = new ToolStripMenuItem();
		menuItemDatabaseScripts = new ToolStripMenuItem();
		menuItemSaveAsSql = new ToolStripMenuItem();
		menuItemSaveAsSqlite = new ToolStripMenuItem();
		menuItemPortableDocuments = new ToolStripMenuItem();
		menuItemSaveAsPdf = new ToolStripMenuItem();
		menuItemSaveAsPostScript = new ToolStripMenuItem();
		menuItemSaveAsEpub = new ToolStripMenuItem();
		menuItemSaveAsMobi = new ToolStripMenuItem();
		menuItemSaveAsXps = new ToolStripMenuItem();
		menuItemSaveAsFictionBook2 = new ToolStripMenuItem();
		menuItemSaveAsChm = new ToolStripMenuItem();
		buttonApply = new ToolStripButton();
		buttonCancel = new ToolStripButton();
		separator1 = new ToolStripSeparator();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		statusStrip.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)panelMain).BeginInit();
		panelMain.SuspendLayout();
		toolStripMain.SuspendLayout();
		tableLayoutPanelMpcorbData.SuspendLayout();
		contextMenuShowControlsInStatusBar.SuspendLayout();
		contextMenuSaveToFile.SuspendLayout();
		SuspendLayout();
		// 
		// toolStripContainer
		// 
		toolStripContainer.AccessibleDescription = "Groups the data";
		toolStripContainer.AccessibleName = "Panel";
		toolStripContainer.AccessibleRole = AccessibleRole.Pane;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.BottomToolStripPanel.AccessibleName = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.ContentPanel.AccessibleName = "Content panel";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.ContentPanel.Controls.Add(panelMain);
		toolStripContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
		toolStripContainer.ContentPanel.Size = new Size(739, 250);
		toolStripContainer.Dock = DockStyle.Fill;
		// 
		// toolStripContainer.LeftToolStripPanel
		// 
		toolStripContainer.LeftToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.LeftToolStripPanel.AccessibleName = "Left panel";
		toolStripContainer.LeftToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		// 
		// toolStripContainer.RightToolStripPanel
		// 
		toolStripContainer.RightToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.RightToolStripPanel.AccessibleName = "Right panel";
		toolStripContainer.RightToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Size = new Size(739, 297);
		toolStripContainer.TabIndex = 2;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.TopToolStripPanel.AccessibleName = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStripMain);
		// 
		// statusStrip
		// 
		statusStrip.AccessibleDescription = "Shows some information";
		statusStrip.AccessibleName = "Status bar with some information";
		statusStrip.AccessibleRole = AccessibleRole.StatusBar;
		statusStrip.AllowClickThrough = true;
		statusStrip.AllowItemReorder = true;
		statusStrip.ContextMenuStrip = contextMenuShowControlsInStatusBar;
		statusStrip.Dock = DockStyle.None;
		statusStrip.Font = new Font("Segoe UI", 9F);
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		statusStrip.Location = new Point(0, 0);
		statusStrip.Name = "statusStrip";
		statusStrip.ProgressBars = null;
		statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		statusStrip.ShowItemToolTips = true;
		statusStrip.Size = new Size(739, 22);
		statusStrip.TabIndex = 0;
		statusStrip.TabStop = true;
		statusStrip.Text = "Status bar";
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows some information";
		labelInformation.AccessibleName = "Some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		// 
		// panelMain
		// 
		panelMain.AccessibleDescription = "Groups the observatory code data";
		panelMain.AccessibleName = "Panel";
		panelMain.AccessibleRole = AccessibleRole.Pane;
		panelMain.AutoScroll = true;
		panelMain.Controls.Add(tableLayoutPanelMpcorbData);
		panelMain.Dock = DockStyle.Fill;
		panelMain.Location = new Point(0, 0);
		panelMain.Name = "panelMain";
		panelMain.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.FormMain;
		panelMain.Size = new Size(739, 250);
		panelMain.TabIndex = 0;
		panelMain.TabStop = true;
		panelMain.Text = "Mainpanel";
		// 
		// toolStripMain
		// 
		toolStripMain.AccessibleDescription = "Toolbar for the observatory codes form";
		toolStripMain.AccessibleName = "Toolbar";
		toolStripMain.AccessibleRole = AccessibleRole.ToolBar;
		toolStripMain.AllowClickThrough = true;
		toolStripMain.AllowItemReorder = true;
		toolStripMain.Dock = DockStyle.None;
		toolStripMain.Font = new Font("Segoe UI", 9F);
		toolStripMain.Items.AddRange(new ToolStripItem[] { buttonApply, buttonCancel, separator1, dropDownButtonSaveList });
		toolStripMain.Location = new Point(0, 0);
		toolStripMain.Name = "toolStripMain";
		toolStripMain.Size = new Size(739, 25);
		toolStripMain.Stretch = true;
		toolStripMain.TabIndex = 0;
		toolStripMain.TabStop = true;
		toolStripMain.Text = "Toolbar";
		// 
		// tableLayoutPanelMpcorbData
		// 
		tableLayoutPanelMpcorbData.AccessibleDescription = "Groups the MPCORB.DAT data";
		tableLayoutPanelMpcorbData.AccessibleName = "MPCORB.DAT table panel";
		tableLayoutPanelMpcorbData.AccessibleRole = AccessibleRole.Pane;
		tableLayoutPanelMpcorbData.AutoScroll = true;
		tableLayoutPanelMpcorbData.ColumnCount = 4;
		tableLayoutPanelMpcorbData.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanelMpcorbData.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanelMpcorbData.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanelMpcorbData.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanelMpcorbData.ContextMenuStrip = contextMenuSaveToFile;
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbIndexData, 1, 0);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbIndex, 0, 0);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbReadableDesignation, 0, 1);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbEpoch, 0, 2);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbMeanAnomalyAtTheEpoch, 0, 3);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbArgumentOfThePerihelion, 0, 4);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbLongitudeOfTheAscendingNode, 0, 5);
		tableLayoutPanelMpcorbData.Controls.Add(labelInclinationToTheEcliptic, 0, 6);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbOrbitalEccentricity, 0, 7);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbMeanDailyMotion, 0, 8);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbSemiMajorAxis, 0, 9);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbAbsoluteMagnitude, 0, 10);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbSlopeParameter, 2, 1);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbReference, 2, 2);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbNumberOfOppositions, 2, 3);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbNumberOfObservations, 2, 4);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbObservationSpan, 2, 5);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbRmsResidual, 2, 6);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbComputerName, 2, 7);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbFlags, 2, 8);
		tableLayoutPanelMpcorbData.Controls.Add(labelMpcorbDateLastObservation, 2, 9);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbReadableDesignation, 1, 1);
		tableLayoutPanelMpcorbData.Controls.Add(dateTimePickerMpcorbEpoch, 1, 2);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbMeanAnomalyAtTheEpoch, 1, 3);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbArgumentOfThePerihelion, 1, 4);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbLongitudeOfTheAscendingNode, 1, 5);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownInclinationToTheEcliptic, 1, 6);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbOrbitalEccentricity, 1, 7);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbMeanDailyMotion, 1, 8);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbSemiMajorAxis, 1, 9);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbAbsoluteMagnitude, 1, 10);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbSlopeParameter, 3, 1);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbReference, 3, 2);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbNumberOfOppositions, 3, 3);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbNumberOfObservations, 3, 4);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbObservationSpan, 3, 5);
		tableLayoutPanelMpcorbData.Controls.Add(numericUpDownMpcorbRmsResidual, 3, 6);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbComputerName, 3, 7);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbFlags, 3, 8);
		tableLayoutPanelMpcorbData.Controls.Add(textBoxMpcorbDateLastObservation, 3, 9);
		tableLayoutPanelMpcorbData.Dock = DockStyle.Fill;
		tableLayoutPanelMpcorbData.Location = new Point(0, 0);
		tableLayoutPanelMpcorbData.Name = "tableLayoutPanelMpcorbData";
		tableLayoutPanelMpcorbData.RowCount = 11;
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.RowStyles.Add(new RowStyle());
		tableLayoutPanelMpcorbData.Size = new Size(739, 250);
		tableLayoutPanelMpcorbData.TabIndex = 2;
		tableLayoutPanelMpcorbData.TabStop = true;
		// 
		// labelMpcorbIndexData
		// 
		labelMpcorbIndexData.AccessibleDescription = "Shows the value of \"Index No.\"";
		labelMpcorbIndexData.AccessibleName = "Value of \"Index No.\"";
		labelMpcorbIndexData.AccessibleRole = AccessibleRole.StatusBar;
		labelMpcorbIndexData.Dock = DockStyle.Fill;
		labelMpcorbIndexData.Location = new Point(264, 0);
		labelMpcorbIndexData.Margin = new Padding(0);
		labelMpcorbIndexData.Name = "labelMpcorbIndexData";
		labelMpcorbIndexData.Size = new Size(120, 20);
		labelMpcorbIndexData.TabIndex = 1;
		labelMpcorbIndexData.ToolTipValues.Description = "Shows the information of \"Index No.\".\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelMpcorbIndexData.ToolTipValues.EnableToolTips = true;
		labelMpcorbIndexData.ToolTipValues.Heading = "Index No.";
		labelMpcorbIndexData.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbIndexData.Values.Text = "..................";
		// 
		// labelMpcorbIndex
		// 
		labelMpcorbIndex.AccessibleDescription = "Shows the index No.";
		labelMpcorbIndex.AccessibleName = "Index No.";
		labelMpcorbIndex.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbIndex.Dock = DockStyle.Fill;
		labelMpcorbIndex.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
		labelMpcorbIndex.Location = new Point(0, 0);
		labelMpcorbIndex.Margin = new Padding(0);
		labelMpcorbIndex.Name = "labelMpcorbIndex";
		labelMpcorbIndex.Size = new Size(264, 20);
		labelMpcorbIndex.TabIndex = 0;
		labelMpcorbIndex.Tag = "0";
		labelMpcorbIndex.ToolTipValues.Description = "Shows the information of \"Index No.\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbIndex.ToolTipValues.EnableToolTips = true;
		labelMpcorbIndex.ToolTipValues.Heading = "Index No.";
		labelMpcorbIndex.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbIndex.Values.Text = "Index No.";
		// 
		// labelMpcorbReadableDesignation
		// 
		labelMpcorbReadableDesignation.AccessibleDescription = "Shows the readable designation";
		labelMpcorbReadableDesignation.AccessibleName = "Readable designation";
		labelMpcorbReadableDesignation.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbReadableDesignation.Dock = DockStyle.Fill;
		labelMpcorbReadableDesignation.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbReadableDesignation.Location = new Point(0, 20);
		labelMpcorbReadableDesignation.Margin = new Padding(0);
		labelMpcorbReadableDesignation.Name = "labelMpcorbReadableDesignation";
		labelMpcorbReadableDesignation.Size = new Size(264, 23);
		labelMpcorbReadableDesignation.TabIndex = 2;
		labelMpcorbReadableDesignation.Tag = "1";
		labelMpcorbReadableDesignation.ToolTipValues.Description = "Shows the information of \"Readable designation\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbReadableDesignation.ToolTipValues.EnableToolTips = true;
		labelMpcorbReadableDesignation.ToolTipValues.Heading = "Readable designation";
		labelMpcorbReadableDesignation.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbReadableDesignation.Values.Text = "Readable designation";
		// 
		// labelMpcorbEpoch
		// 
		labelMpcorbEpoch.AccessibleDescription = "Shows the epoch (in packed form, .0 TT)";
		labelMpcorbEpoch.AccessibleName = "Epoch (in packed form, .0 TT)";
		labelMpcorbEpoch.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbEpoch.Dock = DockStyle.Fill;
		labelMpcorbEpoch.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbEpoch.Location = new Point(0, 43);
		labelMpcorbEpoch.Margin = new Padding(0);
		labelMpcorbEpoch.Name = "labelMpcorbEpoch";
		labelMpcorbEpoch.Size = new Size(264, 23);
		labelMpcorbEpoch.TabIndex = 4;
		labelMpcorbEpoch.Tag = "2";
		labelMpcorbEpoch.ToolTipValues.Description = "Shows the information of \"Epoch (in packed form, .0 TT)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbEpoch.ToolTipValues.EnableToolTips = true;
		labelMpcorbEpoch.ToolTipValues.Heading = "Epoch (in packed form, .0 TT)";
		labelMpcorbEpoch.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbEpoch.Values.Text = "Epoch (in packed form, .0 TT)";
		// 
		// labelMpcorbMeanAnomalyAtTheEpoch
		// 
		labelMpcorbMeanAnomalyAtTheEpoch.AccessibleDescription = "Shows the mean anomaly at the epoch (degrees)";
		labelMpcorbMeanAnomalyAtTheEpoch.AccessibleName = "Mean anomaly at the epoch (degrees)";
		labelMpcorbMeanAnomalyAtTheEpoch.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbMeanAnomalyAtTheEpoch.Dock = DockStyle.Fill;
		labelMpcorbMeanAnomalyAtTheEpoch.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbMeanAnomalyAtTheEpoch.Location = new Point(0, 66);
		labelMpcorbMeanAnomalyAtTheEpoch.Margin = new Padding(0);
		labelMpcorbMeanAnomalyAtTheEpoch.Name = "labelMpcorbMeanAnomalyAtTheEpoch";
		labelMpcorbMeanAnomalyAtTheEpoch.Size = new Size(264, 22);
		labelMpcorbMeanAnomalyAtTheEpoch.TabIndex = 6;
		labelMpcorbMeanAnomalyAtTheEpoch.Tag = "3";
		labelMpcorbMeanAnomalyAtTheEpoch.ToolTipValues.Description = "Shows the information of \"Mean anomaly at the epoch, in degrees\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbMeanAnomalyAtTheEpoch.ToolTipValues.EnableToolTips = true;
		labelMpcorbMeanAnomalyAtTheEpoch.ToolTipValues.Heading = "Mean anomaly at the epoch (degrees)";
		labelMpcorbMeanAnomalyAtTheEpoch.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbMeanAnomalyAtTheEpoch.Values.ExtraText = "°";
		labelMpcorbMeanAnomalyAtTheEpoch.Values.Text = "Mean anomaly at the epoch";
		// 
		// labelMpcorbArgumentOfThePerihelion
		// 
		labelMpcorbArgumentOfThePerihelion.AccessibleDescription = "Shows the argument of the perihelion, J2000.0 (degrees)";
		labelMpcorbArgumentOfThePerihelion.AccessibleName = "Argument of the perihelion, J2000.0 (degrees)";
		labelMpcorbArgumentOfThePerihelion.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbArgumentOfThePerihelion.Dock = DockStyle.Fill;
		labelMpcorbArgumentOfThePerihelion.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbArgumentOfThePerihelion.Location = new Point(0, 88);
		labelMpcorbArgumentOfThePerihelion.Margin = new Padding(0);
		labelMpcorbArgumentOfThePerihelion.Name = "labelMpcorbArgumentOfThePerihelion";
		labelMpcorbArgumentOfThePerihelion.Size = new Size(264, 22);
		labelMpcorbArgumentOfThePerihelion.TabIndex = 8;
		labelMpcorbArgumentOfThePerihelion.Tag = "4";
		labelMpcorbArgumentOfThePerihelion.ToolTipValues.Description = "Shows the information of \"Argument of the perihelion, J2000.0 (degrees)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbArgumentOfThePerihelion.ToolTipValues.EnableToolTips = true;
		labelMpcorbArgumentOfThePerihelion.ToolTipValues.Heading = "Argument of the perihelion, J2000.0 (degrees)";
		labelMpcorbArgumentOfThePerihelion.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbArgumentOfThePerihelion.Values.ExtraText = "°";
		labelMpcorbArgumentOfThePerihelion.Values.Text = "Argument of the perihelion, J2000.0";
		// 
		// labelMpcorbLongitudeOfTheAscendingNode
		// 
		labelMpcorbLongitudeOfTheAscendingNode.AccessibleDescription = "Shows the longitude of the ascending node, J2000.0 (degrees)";
		labelMpcorbLongitudeOfTheAscendingNode.AccessibleName = "Longitude of the ascending node, J2000.0 (degrees)";
		labelMpcorbLongitudeOfTheAscendingNode.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbLongitudeOfTheAscendingNode.Dock = DockStyle.Fill;
		labelMpcorbLongitudeOfTheAscendingNode.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbLongitudeOfTheAscendingNode.Location = new Point(0, 110);
		labelMpcorbLongitudeOfTheAscendingNode.Margin = new Padding(0);
		labelMpcorbLongitudeOfTheAscendingNode.Name = "labelMpcorbLongitudeOfTheAscendingNode";
		labelMpcorbLongitudeOfTheAscendingNode.Size = new Size(264, 23);
		labelMpcorbLongitudeOfTheAscendingNode.TabIndex = 10;
		labelMpcorbLongitudeOfTheAscendingNode.Tag = "5";
		labelMpcorbLongitudeOfTheAscendingNode.ToolTipValues.Description = "Shows the information of \"Longitude of the ascending node, J2000.0 (degrees)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbLongitudeOfTheAscendingNode.ToolTipValues.EnableToolTips = true;
		labelMpcorbLongitudeOfTheAscendingNode.ToolTipValues.Heading = "Longitude of the ascending node, J2000.0 (degrees)";
		labelMpcorbLongitudeOfTheAscendingNode.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbLongitudeOfTheAscendingNode.Values.ExtraText = "°";
		labelMpcorbLongitudeOfTheAscendingNode.Values.Text = "Longitude of the ascending node, J2000.0";
		// 
		// labelInclinationToTheEcliptic
		// 
		labelInclinationToTheEcliptic.AccessibleDescription = "Shows the inclination to the ecliptic, J2000.0 (degrees)";
		labelInclinationToTheEcliptic.AccessibleName = "Inclination to the ecliptic, J2000.0 (degrees)";
		labelInclinationToTheEcliptic.AccessibleRole = AccessibleRole.StaticText;
		labelInclinationToTheEcliptic.Dock = DockStyle.Fill;
		labelInclinationToTheEcliptic.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelInclinationToTheEcliptic.Location = new Point(0, 133);
		labelInclinationToTheEcliptic.Margin = new Padding(0);
		labelInclinationToTheEcliptic.Name = "labelInclinationToTheEcliptic";
		labelInclinationToTheEcliptic.Size = new Size(264, 22);
		labelInclinationToTheEcliptic.TabIndex = 12;
		labelInclinationToTheEcliptic.Tag = "6";
		labelInclinationToTheEcliptic.ToolTipValues.Description = "Shows the information of \"Inclination to the ecliptic, J2000.0 (degrees)\".\r\nDouble-click or right-click to open the terminology.";
		labelInclinationToTheEcliptic.ToolTipValues.EnableToolTips = true;
		labelInclinationToTheEcliptic.ToolTipValues.Heading = "Inclination to the ecliptic, J2000.0 (degrees)";
		labelInclinationToTheEcliptic.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelInclinationToTheEcliptic.Values.ExtraText = "°";
		labelInclinationToTheEcliptic.Values.Text = "Inclination to the ecliptic, J2000.0";
		// 
		// labelMpcorbOrbitalEccentricity
		// 
		labelMpcorbOrbitalEccentricity.AccessibleDescription = "Shows the orbital eccentricity";
		labelMpcorbOrbitalEccentricity.AccessibleName = "Orbital eccentricity";
		labelMpcorbOrbitalEccentricity.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbOrbitalEccentricity.Dock = DockStyle.Fill;
		labelMpcorbOrbitalEccentricity.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbOrbitalEccentricity.Location = new Point(0, 155);
		labelMpcorbOrbitalEccentricity.Margin = new Padding(0);
		labelMpcorbOrbitalEccentricity.Name = "labelMpcorbOrbitalEccentricity";
		labelMpcorbOrbitalEccentricity.Size = new Size(264, 23);
		labelMpcorbOrbitalEccentricity.TabIndex = 14;
		labelMpcorbOrbitalEccentricity.Tag = "7";
		labelMpcorbOrbitalEccentricity.ToolTipValues.Description = "Shows the information of \"Orbital eccentricity\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbOrbitalEccentricity.ToolTipValues.EnableToolTips = true;
		labelMpcorbOrbitalEccentricity.ToolTipValues.Heading = "Orbital eccentricity";
		labelMpcorbOrbitalEccentricity.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbOrbitalEccentricity.Values.Text = "Orbital eccentricity";
		// 
		// labelMpcorbMeanDailyMotion
		// 
		labelMpcorbMeanDailyMotion.AccessibleDescription = "Shows the mean daily motion (degrees per day)";
		labelMpcorbMeanDailyMotion.AccessibleName = "Mean daily motion (degrees per day)";
		labelMpcorbMeanDailyMotion.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbMeanDailyMotion.Dock = DockStyle.Fill;
		labelMpcorbMeanDailyMotion.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbMeanDailyMotion.Location = new Point(0, 178);
		labelMpcorbMeanDailyMotion.Margin = new Padding(0);
		labelMpcorbMeanDailyMotion.Name = "labelMpcorbMeanDailyMotion";
		labelMpcorbMeanDailyMotion.Size = new Size(264, 23);
		labelMpcorbMeanDailyMotion.TabIndex = 16;
		labelMpcorbMeanDailyMotion.Tag = "8";
		labelMpcorbMeanDailyMotion.ToolTipValues.Description = "Shows the information of \"Mean daily motion (degrees per day)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbMeanDailyMotion.ToolTipValues.EnableToolTips = true;
		labelMpcorbMeanDailyMotion.ToolTipValues.Heading = "Mean daily motion (degrees per day)";
		labelMpcorbMeanDailyMotion.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbMeanDailyMotion.Values.ExtraText = "°/day";
		labelMpcorbMeanDailyMotion.Values.Text = "Mean daily motion";
		// 
		// labelMpcorbSemiMajorAxis
		// 
		labelMpcorbSemiMajorAxis.AccessibleDescription = "Shows the semi-major axis (AU)";
		labelMpcorbSemiMajorAxis.AccessibleName = "Semi-major axis (AU)";
		labelMpcorbSemiMajorAxis.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbSemiMajorAxis.Dock = DockStyle.Fill;
		labelMpcorbSemiMajorAxis.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbSemiMajorAxis.Location = new Point(0, 201);
		labelMpcorbSemiMajorAxis.Margin = new Padding(0);
		labelMpcorbSemiMajorAxis.Name = "labelMpcorbSemiMajorAxis";
		labelMpcorbSemiMajorAxis.Size = new Size(264, 23);
		labelMpcorbSemiMajorAxis.TabIndex = 18;
		labelMpcorbSemiMajorAxis.Tag = "9";
		labelMpcorbSemiMajorAxis.ToolTipValues.Description = "Shows the information of \"Semi-major axis (AU)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbSemiMajorAxis.ToolTipValues.EnableToolTips = true;
		labelMpcorbSemiMajorAxis.ToolTipValues.Heading = "Semi-major axis (AU)";
		labelMpcorbSemiMajorAxis.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbSemiMajorAxis.Values.ExtraText = "AU";
		labelMpcorbSemiMajorAxis.Values.Text = "Semi-major axis";
		// 
		// labelMpcorbAbsoluteMagnitude
		// 
		labelMpcorbAbsoluteMagnitude.AccessibleDescription = "Shows the absolute magnitude, H";
		labelMpcorbAbsoluteMagnitude.AccessibleName = "Absolute magnitude, H";
		labelMpcorbAbsoluteMagnitude.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbAbsoluteMagnitude.Dock = DockStyle.Top;
		labelMpcorbAbsoluteMagnitude.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbAbsoluteMagnitude.Location = new Point(0, 224);
		labelMpcorbAbsoluteMagnitude.Margin = new Padding(0);
		labelMpcorbAbsoluteMagnitude.Name = "labelMpcorbAbsoluteMagnitude";
		labelMpcorbAbsoluteMagnitude.Size = new Size(264, 20);
		labelMpcorbAbsoluteMagnitude.TabIndex = 20;
		labelMpcorbAbsoluteMagnitude.Tag = "10";
		labelMpcorbAbsoluteMagnitude.ToolTipValues.Description = "Shows the information of \"Absolute magnitude, H\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbAbsoluteMagnitude.ToolTipValues.EnableToolTips = true;
		labelMpcorbAbsoluteMagnitude.ToolTipValues.Heading = "Absolute magnitude, H";
		labelMpcorbAbsoluteMagnitude.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbAbsoluteMagnitude.Values.ExtraText = "mag";
		labelMpcorbAbsoluteMagnitude.Values.Text = "Absolute magnitude, H";
		// 
		// labelMpcorbSlopeParameter
		// 
		labelMpcorbSlopeParameter.AccessibleDescription = "Shows the slope parameter, G";
		labelMpcorbSlopeParameter.AccessibleName = "Slope parameter, G";
		labelMpcorbSlopeParameter.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbSlopeParameter.Dock = DockStyle.Fill;
		labelMpcorbSlopeParameter.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
		labelMpcorbSlopeParameter.Location = new Point(384, 20);
		labelMpcorbSlopeParameter.Margin = new Padding(0);
		labelMpcorbSlopeParameter.Name = "labelMpcorbSlopeParameter";
		labelMpcorbSlopeParameter.Size = new Size(222, 23);
		labelMpcorbSlopeParameter.TabIndex = 22;
		labelMpcorbSlopeParameter.Tag = "11";
		labelMpcorbSlopeParameter.ToolTipValues.Description = "Shows the information of \"Slope parameter, G\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbSlopeParameter.ToolTipValues.EnableToolTips = true;
		labelMpcorbSlopeParameter.ToolTipValues.Heading = "Slope parameter, G";
		labelMpcorbSlopeParameter.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbSlopeParameter.Values.Text = "Slope parameter, G";
		// 
		// labelMpcorbReference
		// 
		labelMpcorbReference.AccessibleDescription = "Shows the reference";
		labelMpcorbReference.AccessibleName = "Reference";
		labelMpcorbReference.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbReference.Dock = DockStyle.Fill;
		labelMpcorbReference.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbReference.Location = new Point(384, 43);
		labelMpcorbReference.Margin = new Padding(0);
		labelMpcorbReference.Name = "labelMpcorbReference";
		labelMpcorbReference.Size = new Size(222, 23);
		labelMpcorbReference.TabIndex = 24;
		labelMpcorbReference.Tag = "12";
		labelMpcorbReference.ToolTipValues.Description = "Shows the information of \"Reference\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbReference.ToolTipValues.EnableToolTips = true;
		labelMpcorbReference.ToolTipValues.Heading = "Reference";
		labelMpcorbReference.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbReference.Values.Text = "Reference";
		// 
		// labelMpcorbNumberOfOppositions
		// 
		labelMpcorbNumberOfOppositions.AccessibleDescription = "Shows the number of oppositions";
		labelMpcorbNumberOfOppositions.AccessibleName = "Number of oppositions";
		labelMpcorbNumberOfOppositions.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbNumberOfOppositions.Dock = DockStyle.Fill;
		labelMpcorbNumberOfOppositions.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbNumberOfOppositions.Location = new Point(384, 66);
		labelMpcorbNumberOfOppositions.Margin = new Padding(0);
		labelMpcorbNumberOfOppositions.Name = "labelMpcorbNumberOfOppositions";
		labelMpcorbNumberOfOppositions.Size = new Size(222, 22);
		labelMpcorbNumberOfOppositions.TabIndex = 26;
		labelMpcorbNumberOfOppositions.Tag = "13";
		labelMpcorbNumberOfOppositions.ToolTipValues.Description = "Shows the information of \"Number of oppositions\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbNumberOfOppositions.ToolTipValues.EnableToolTips = true;
		labelMpcorbNumberOfOppositions.ToolTipValues.Heading = "Number of oppositions";
		labelMpcorbNumberOfOppositions.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbNumberOfOppositions.Values.Text = "Number of oppositions";
		// 
		// labelMpcorbNumberOfObservations
		// 
		labelMpcorbNumberOfObservations.AccessibleDescription = "Shows the number of observations";
		labelMpcorbNumberOfObservations.AccessibleName = "Number of observations";
		labelMpcorbNumberOfObservations.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbNumberOfObservations.Dock = DockStyle.Fill;
		labelMpcorbNumberOfObservations.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbNumberOfObservations.Location = new Point(384, 88);
		labelMpcorbNumberOfObservations.Margin = new Padding(0);
		labelMpcorbNumberOfObservations.Name = "labelMpcorbNumberOfObservations";
		labelMpcorbNumberOfObservations.Size = new Size(222, 22);
		labelMpcorbNumberOfObservations.TabIndex = 28;
		labelMpcorbNumberOfObservations.Tag = "14";
		labelMpcorbNumberOfObservations.ToolTipValues.Description = "Shows the information of \"Number of observations\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbNumberOfObservations.ToolTipValues.EnableToolTips = true;
		labelMpcorbNumberOfObservations.ToolTipValues.Heading = "Number of observations";
		labelMpcorbNumberOfObservations.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbNumberOfObservations.Values.Text = "Number of observations";
		// 
		// labelMpcorbObservationSpan
		// 
		labelMpcorbObservationSpan.AccessibleDescription = "Shows the observation span";
		labelMpcorbObservationSpan.AccessibleName = "Observation span";
		labelMpcorbObservationSpan.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbObservationSpan.Dock = DockStyle.Fill;
		labelMpcorbObservationSpan.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbObservationSpan.Location = new Point(384, 110);
		labelMpcorbObservationSpan.Margin = new Padding(0);
		labelMpcorbObservationSpan.Name = "labelMpcorbObservationSpan";
		labelMpcorbObservationSpan.Size = new Size(222, 23);
		labelMpcorbObservationSpan.TabIndex = 30;
		labelMpcorbObservationSpan.Tag = "15";
		labelMpcorbObservationSpan.ToolTipValues.Description = "Shows the information of \"Observation span\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbObservationSpan.ToolTipValues.EnableToolTips = true;
		labelMpcorbObservationSpan.ToolTipValues.Heading = "Observation span";
		labelMpcorbObservationSpan.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbObservationSpan.Values.Text = "Observation span";
		// 
		// labelMpcorbRmsResidual
		// 
		labelMpcorbRmsResidual.AccessibleDescription = "Shows the r.m.s. residual (arcseconds)";
		labelMpcorbRmsResidual.AccessibleName = "r.m.s. residual (arcseconds)";
		labelMpcorbRmsResidual.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbRmsResidual.Dock = DockStyle.Fill;
		labelMpcorbRmsResidual.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbRmsResidual.Location = new Point(384, 133);
		labelMpcorbRmsResidual.Margin = new Padding(0);
		labelMpcorbRmsResidual.Name = "labelMpcorbRmsResidual";
		labelMpcorbRmsResidual.Size = new Size(222, 22);
		labelMpcorbRmsResidual.TabIndex = 32;
		labelMpcorbRmsResidual.Tag = "16";
		labelMpcorbRmsResidual.ToolTipValues.Description = "Shows the information of \"r.m.s residual (arcseconds)\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbRmsResidual.ToolTipValues.EnableToolTips = true;
		labelMpcorbRmsResidual.ToolTipValues.Heading = "r.m.s. residual (arcseconds)";
		labelMpcorbRmsResidual.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbRmsResidual.Values.ExtraText = "\"";
		labelMpcorbRmsResidual.Values.Text = "r.m.s. residual";
		// 
		// labelMpcorbComputerName
		// 
		labelMpcorbComputerName.AccessibleDescription = "Shows the computer name";
		labelMpcorbComputerName.AccessibleName = "Computer name";
		labelMpcorbComputerName.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbComputerName.Dock = DockStyle.Fill;
		labelMpcorbComputerName.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbComputerName.Location = new Point(384, 155);
		labelMpcorbComputerName.Margin = new Padding(0);
		labelMpcorbComputerName.Name = "labelMpcorbComputerName";
		labelMpcorbComputerName.Size = new Size(222, 23);
		labelMpcorbComputerName.TabIndex = 34;
		labelMpcorbComputerName.Tag = "17";
		labelMpcorbComputerName.ToolTipValues.Description = "Shows the information of \"Computer name\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbComputerName.ToolTipValues.EnableToolTips = true;
		labelMpcorbComputerName.ToolTipValues.Heading = "Computer name";
		labelMpcorbComputerName.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbComputerName.Values.Text = "Computer name";
		// 
		// labelMpcorbFlags
		// 
		labelMpcorbFlags.AccessibleDescription = "Shows the 4-hexdigit flags";
		labelMpcorbFlags.AccessibleName = "4-hexdigit flags";
		labelMpcorbFlags.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbFlags.Dock = DockStyle.Fill;
		labelMpcorbFlags.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbFlags.Location = new Point(384, 178);
		labelMpcorbFlags.Margin = new Padding(0);
		labelMpcorbFlags.Name = "labelMpcorbFlags";
		labelMpcorbFlags.Size = new Size(222, 23);
		labelMpcorbFlags.TabIndex = 36;
		labelMpcorbFlags.Tag = "18";
		labelMpcorbFlags.ToolTipValues.Description = "Shows the information of \"4-hexdigit flags\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbFlags.ToolTipValues.EnableToolTips = true;
		labelMpcorbFlags.ToolTipValues.Heading = "4-hexdigit flags";
		labelMpcorbFlags.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbFlags.Values.Text = "4-hexdigit flags";
		// 
		// labelMpcorbDateLastObservation
		// 
		labelMpcorbDateLastObservation.AccessibleDescription = "Shows the date of last observation (YYYYMMDD)";
		labelMpcorbDateLastObservation.AccessibleName = "Date of last observation (YYYYMMDD)";
		labelMpcorbDateLastObservation.AccessibleRole = AccessibleRole.StaticText;
		labelMpcorbDateLastObservation.Dock = DockStyle.Fill;
		labelMpcorbDateLastObservation.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
		labelMpcorbDateLastObservation.Location = new Point(384, 201);
		labelMpcorbDateLastObservation.Margin = new Padding(0);
		labelMpcorbDateLastObservation.Name = "labelMpcorbDateLastObservation";
		labelMpcorbDateLastObservation.Size = new Size(222, 23);
		labelMpcorbDateLastObservation.TabIndex = 38;
		labelMpcorbDateLastObservation.Tag = "19";
		labelMpcorbDateLastObservation.ToolTipValues.Description = "Shows the information of \"Date of last observation\".\r\nDouble-click or right-click to open the terminology.";
		labelMpcorbDateLastObservation.ToolTipValues.EnableToolTips = true;
		labelMpcorbDateLastObservation.ToolTipValues.Heading = "Date of last observation (YYYYMMDD)";
		labelMpcorbDateLastObservation.ToolTipValues.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		labelMpcorbDateLastObservation.Values.ExtraText = "YYYYMMDD";
		labelMpcorbDateLastObservation.Values.Text = "Date of last observation";
		// 
		// textBoxMpcorbReadableDesignation
		// 
		textBoxMpcorbReadableDesignation.Dock = DockStyle.Fill;
		textBoxMpcorbReadableDesignation.Location = new Point(264, 20);
		textBoxMpcorbReadableDesignation.Margin = new Padding(0);
		textBoxMpcorbReadableDesignation.Name = "textBoxMpcorbReadableDesignation";
		textBoxMpcorbReadableDesignation.Size = new Size(120, 23);
		textBoxMpcorbReadableDesignation.TabIndex = 40;
		// 
		// dateTimePickerMpcorbEpoch
		// 
		dateTimePickerMpcorbEpoch.CalendarShowWeekNumbers = true;
		dateTimePickerMpcorbEpoch.Dock = DockStyle.Top;
		dateTimePickerMpcorbEpoch.Format = DateTimePickerFormat.Short;
		dateTimePickerMpcorbEpoch.Location = new Point(264, 43);
		dateTimePickerMpcorbEpoch.Margin = new Padding(0);
		dateTimePickerMpcorbEpoch.Name = "dateTimePickerMpcorbEpoch";
		dateTimePickerMpcorbEpoch.ShowUpDown = true;
		dateTimePickerMpcorbEpoch.Size = new Size(120, 21);
		dateTimePickerMpcorbEpoch.TabIndex = 41;
		// 
		// numericUpDownMpcorbMeanAnomalyAtTheEpoch
		// 
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Dock = DockStyle.Top;
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Location = new Point(264, 66);
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Margin = new Padding(0);
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Name = "numericUpDownMpcorbMeanAnomalyAtTheEpoch";
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Size = new Size(120, 22);
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.TabIndex = 42;
		numericUpDownMpcorbMeanAnomalyAtTheEpoch.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbArgumentOfThePerihelion
		// 
		numericUpDownMpcorbArgumentOfThePerihelion.Dock = DockStyle.Top;
		numericUpDownMpcorbArgumentOfThePerihelion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbArgumentOfThePerihelion.Location = new Point(264, 88);
		numericUpDownMpcorbArgumentOfThePerihelion.Margin = new Padding(0);
		numericUpDownMpcorbArgumentOfThePerihelion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbArgumentOfThePerihelion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbArgumentOfThePerihelion.Name = "numericUpDownMpcorbArgumentOfThePerihelion";
		numericUpDownMpcorbArgumentOfThePerihelion.Size = new Size(120, 22);
		numericUpDownMpcorbArgumentOfThePerihelion.TabIndex = 43;
		numericUpDownMpcorbArgumentOfThePerihelion.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbLongitudeOfTheAscendingNode
		// 
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Dock = DockStyle.Top;
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Location = new Point(264, 110);
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Margin = new Padding(0);
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Name = "numericUpDownMpcorbLongitudeOfTheAscendingNode";
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Size = new Size(120, 22);
		numericUpDownMpcorbLongitudeOfTheAscendingNode.TabIndex = 44;
		numericUpDownMpcorbLongitudeOfTheAscendingNode.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownInclinationToTheEcliptic
		// 
		numericUpDownInclinationToTheEcliptic.Dock = DockStyle.Top;
		numericUpDownInclinationToTheEcliptic.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownInclinationToTheEcliptic.Location = new Point(264, 133);
		numericUpDownInclinationToTheEcliptic.Margin = new Padding(0);
		numericUpDownInclinationToTheEcliptic.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownInclinationToTheEcliptic.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownInclinationToTheEcliptic.Name = "numericUpDownInclinationToTheEcliptic";
		numericUpDownInclinationToTheEcliptic.Size = new Size(120, 22);
		numericUpDownInclinationToTheEcliptic.TabIndex = 45;
		numericUpDownInclinationToTheEcliptic.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbOrbitalEccentricity
		// 
		numericUpDownMpcorbOrbitalEccentricity.Dock = DockStyle.Top;
		numericUpDownMpcorbOrbitalEccentricity.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbOrbitalEccentricity.Location = new Point(264, 155);
		numericUpDownMpcorbOrbitalEccentricity.Margin = new Padding(0);
		numericUpDownMpcorbOrbitalEccentricity.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbOrbitalEccentricity.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbOrbitalEccentricity.Name = "numericUpDownMpcorbOrbitalEccentricity";
		numericUpDownMpcorbOrbitalEccentricity.Size = new Size(120, 22);
		numericUpDownMpcorbOrbitalEccentricity.TabIndex = 46;
		numericUpDownMpcorbOrbitalEccentricity.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbMeanDailyMotion
		// 
		numericUpDownMpcorbMeanDailyMotion.Dock = DockStyle.Top;
		numericUpDownMpcorbMeanDailyMotion.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbMeanDailyMotion.Location = new Point(264, 178);
		numericUpDownMpcorbMeanDailyMotion.Margin = new Padding(0);
		numericUpDownMpcorbMeanDailyMotion.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbMeanDailyMotion.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbMeanDailyMotion.Name = "numericUpDownMpcorbMeanDailyMotion";
		numericUpDownMpcorbMeanDailyMotion.Size = new Size(120, 22);
		numericUpDownMpcorbMeanDailyMotion.TabIndex = 47;
		numericUpDownMpcorbMeanDailyMotion.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbSemiMajorAxis
		// 
		numericUpDownMpcorbSemiMajorAxis.Dock = DockStyle.Top;
		numericUpDownMpcorbSemiMajorAxis.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbSemiMajorAxis.Location = new Point(264, 201);
		numericUpDownMpcorbSemiMajorAxis.Margin = new Padding(0);
		numericUpDownMpcorbSemiMajorAxis.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbSemiMajorAxis.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbSemiMajorAxis.Name = "numericUpDownMpcorbSemiMajorAxis";
		numericUpDownMpcorbSemiMajorAxis.Size = new Size(120, 22);
		numericUpDownMpcorbSemiMajorAxis.TabIndex = 48;
		numericUpDownMpcorbSemiMajorAxis.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbAbsoluteMagnitude
		// 
		numericUpDownMpcorbAbsoluteMagnitude.Dock = DockStyle.Top;
		numericUpDownMpcorbAbsoluteMagnitude.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbAbsoluteMagnitude.Location = new Point(264, 224);
		numericUpDownMpcorbAbsoluteMagnitude.Margin = new Padding(0);
		numericUpDownMpcorbAbsoluteMagnitude.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbAbsoluteMagnitude.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbAbsoluteMagnitude.Name = "numericUpDownMpcorbAbsoluteMagnitude";
		numericUpDownMpcorbAbsoluteMagnitude.Size = new Size(120, 22);
		numericUpDownMpcorbAbsoluteMagnitude.TabIndex = 49;
		numericUpDownMpcorbAbsoluteMagnitude.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbSlopeParameter
		// 
		numericUpDownMpcorbSlopeParameter.Dock = DockStyle.Top;
		numericUpDownMpcorbSlopeParameter.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbSlopeParameter.Location = new Point(606, 20);
		numericUpDownMpcorbSlopeParameter.Margin = new Padding(0);
		numericUpDownMpcorbSlopeParameter.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbSlopeParameter.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbSlopeParameter.Name = "numericUpDownMpcorbSlopeParameter";
		numericUpDownMpcorbSlopeParameter.Size = new Size(133, 22);
		numericUpDownMpcorbSlopeParameter.TabIndex = 50;
		numericUpDownMpcorbSlopeParameter.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// textBoxMpcorbReference
		// 
		textBoxMpcorbReference.Dock = DockStyle.Top;
		textBoxMpcorbReference.Location = new Point(606, 43);
		textBoxMpcorbReference.Margin = new Padding(0);
		textBoxMpcorbReference.Name = "textBoxMpcorbReference";
		textBoxMpcorbReference.Size = new Size(133, 23);
		textBoxMpcorbReference.TabIndex = 51;
		// 
		// numericUpDownMpcorbNumberOfOppositions
		// 
		numericUpDownMpcorbNumberOfOppositions.Dock = DockStyle.Top;
		numericUpDownMpcorbNumberOfOppositions.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbNumberOfOppositions.Location = new Point(606, 66);
		numericUpDownMpcorbNumberOfOppositions.Margin = new Padding(0);
		numericUpDownMpcorbNumberOfOppositions.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbNumberOfOppositions.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbNumberOfOppositions.Name = "numericUpDownMpcorbNumberOfOppositions";
		numericUpDownMpcorbNumberOfOppositions.Size = new Size(133, 22);
		numericUpDownMpcorbNumberOfOppositions.TabIndex = 52;
		numericUpDownMpcorbNumberOfOppositions.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// numericUpDownMpcorbNumberOfObservations
		// 
		numericUpDownMpcorbNumberOfObservations.Dock = DockStyle.Top;
		numericUpDownMpcorbNumberOfObservations.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbNumberOfObservations.Location = new Point(606, 88);
		numericUpDownMpcorbNumberOfObservations.Margin = new Padding(0);
		numericUpDownMpcorbNumberOfObservations.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbNumberOfObservations.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbNumberOfObservations.Name = "numericUpDownMpcorbNumberOfObservations";
		numericUpDownMpcorbNumberOfObservations.Size = new Size(133, 22);
		numericUpDownMpcorbNumberOfObservations.TabIndex = 53;
		numericUpDownMpcorbNumberOfObservations.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// textBoxMpcorbObservationSpan
		// 
		textBoxMpcorbObservationSpan.Dock = DockStyle.Top;
		textBoxMpcorbObservationSpan.Location = new Point(606, 110);
		textBoxMpcorbObservationSpan.Margin = new Padding(0);
		textBoxMpcorbObservationSpan.Name = "textBoxMpcorbObservationSpan";
		textBoxMpcorbObservationSpan.Size = new Size(133, 23);
		textBoxMpcorbObservationSpan.TabIndex = 54;
		// 
		// numericUpDownMpcorbRmsResidual
		// 
		numericUpDownMpcorbRmsResidual.Dock = DockStyle.Top;
		numericUpDownMpcorbRmsResidual.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMpcorbRmsResidual.Location = new Point(606, 133);
		numericUpDownMpcorbRmsResidual.Margin = new Padding(0);
		numericUpDownMpcorbRmsResidual.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMpcorbRmsResidual.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMpcorbRmsResidual.Name = "numericUpDownMpcorbRmsResidual";
		numericUpDownMpcorbRmsResidual.Size = new Size(133, 22);
		numericUpDownMpcorbRmsResidual.TabIndex = 55;
		numericUpDownMpcorbRmsResidual.Value = new decimal(new int[] { 0, 0, 0, 0 });
		// 
		// textBoxMpcorbComputerName
		// 
		textBoxMpcorbComputerName.Dock = DockStyle.Top;
		textBoxMpcorbComputerName.Location = new Point(606, 155);
		textBoxMpcorbComputerName.Margin = new Padding(0);
		textBoxMpcorbComputerName.Name = "textBoxMpcorbComputerName";
		textBoxMpcorbComputerName.Size = new Size(133, 23);
		textBoxMpcorbComputerName.TabIndex = 56;
		// 
		// textBoxMpcorbFlags
		// 
		textBoxMpcorbFlags.Dock = DockStyle.Top;
		textBoxMpcorbFlags.Location = new Point(606, 178);
		textBoxMpcorbFlags.Margin = new Padding(0);
		textBoxMpcorbFlags.MaxLength = 4;
		textBoxMpcorbFlags.Name = "textBoxMpcorbFlags";
		textBoxMpcorbFlags.Size = new Size(133, 23);
		textBoxMpcorbFlags.TabIndex = 57;
		// 
		// textBoxMpcorbDateLastObservation
		// 
		textBoxMpcorbDateLastObservation.Dock = DockStyle.Top;
		textBoxMpcorbDateLastObservation.Location = new Point(606, 201);
		textBoxMpcorbDateLastObservation.Margin = new Padding(0);
		textBoxMpcorbDateLastObservation.MaxLength = 8;
		textBoxMpcorbDateLastObservation.Name = "textBoxMpcorbDateLastObservation";
		textBoxMpcorbDateLastObservation.Size = new Size(133, 23);
		textBoxMpcorbDateLastObservation.TabIndex = 58;
		// 
		// dropDownButtonSaveList
		// 
		dropDownButtonSaveList.AccessibleDescription = "Saves the list as file";
		dropDownButtonSaveList.AccessibleName = "Save list";
		dropDownButtonSaveList.AccessibleRole = AccessibleRole.ButtonDropDown;
		dropDownButtonSaveList.DropDown = contextMenuSaveToFile;
		dropDownButtonSaveList.Image = Resources.FatcowIcons16px.fatcow_diskette_16px;
		dropDownButtonSaveList.ImageTransparentColor = Color.Magenta;
		dropDownButtonSaveList.Name = "dropDownButtonSaveList";
		dropDownButtonSaveList.Size = new Size(78, 22);
		dropDownButtonSaveList.Text = "&Save list";
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// contextMenuShowControlsInStatusBar
		// 
		contextMenuShowControlsInStatusBar.AccessibleDescription = "Shows the context menu for the visibility of controls in the status bar";
		contextMenuShowControlsInStatusBar.AccessibleName = "Context menu for the visibility of controls in the status bar";
		contextMenuShowControlsInStatusBar.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuShowControlsInStatusBar.AllowClickThrough = true;
		contextMenuShowControlsInStatusBar.Font = new Font("Segoe UI", 9F);
		contextMenuShowControlsInStatusBar.Items.AddRange(new ToolStripItem[] { menuItemShowInformationLabel });
		contextMenuShowControlsInStatusBar.Name = "contextMenuStrip";
		contextMenuShowControlsInStatusBar.Size = new Size(198, 26);
		contextMenuShowControlsInStatusBar.TabStop = true;
		contextMenuShowControlsInStatusBar.Text = "Show controls in status bar";
		// 
		// menuItemShowInformationLabel
		// 
		menuItemShowInformationLabel.AccessibleDescription = "Determines whether the information label is visible or hidden";
		menuItemShowInformationLabel.AccessibleName = "Information label";
		menuItemShowInformationLabel.AccessibleRole = AccessibleRole.MenuItem;
		menuItemShowInformationLabel.AutoToolTip = true;
		menuItemShowInformationLabel.Checked = true;
		menuItemShowInformationLabel.CheckOnClick = true;
		menuItemShowInformationLabel.CheckState = CheckState.Checked;
		menuItemShowInformationLabel.Name = "menuItemShowInformationLabel";
		menuItemShowInformationLabel.ShortcutKeyDisplayString = "";
		menuItemShowInformationLabel.Size = new Size(197, 22);
		menuItemShowInformationLabel.Text = "Show &information label";
		// 
		// contextMenuSaveToFile
		// 
		contextMenuSaveToFile.AccessibleDescription = "Save the list as file";
		contextMenuSaveToFile.AccessibleName = "Save list";
		contextMenuSaveToFile.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuSaveToFile.AllowClickThrough = true;
		contextMenuSaveToFile.Font = new Font("Segoe UI", 9F);
		contextMenuSaveToFile.Items.AddRange(new ToolStripItem[] { menuItemTextFiles, menuItemWriterDocuments, menuItemSpreadsheetDocuments, menuItemXmlDocuments, menuItemConfigurationFiles, menuItemDatabaseScripts, menuItemPortableDocuments });
		contextMenuSaveToFile.Name = "contextMenuSaveList";
		contextMenuSaveToFile.Size = new Size(202, 158);
		contextMenuSaveToFile.TabStop = true;
		contextMenuSaveToFile.Text = "&Save list";
		// 
		// menuItemTextFiles
		// 
		menuItemTextFiles.AccessibleDescription = "Saves the list as text file";
		menuItemTextFiles.AccessibleName = "Save as text file";
		menuItemTextFiles.AccessibleRole = AccessibleRole.MenuItem;
		menuItemTextFiles.AutoToolTip = true;
		menuItemTextFiles.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsText, menuItemSaveAsLatex, menuItemSaveAsMarkdown, menuItemSaveAsAsciiDoc, menuItemSaveAsReStructuredText, menuItemSaveAsTextile, menuItemSaveAsTypst });
		menuItemTextFiles.Image = Resources.FatcowIcons16px.fatcow_file_extension_txt_16px;
		menuItemTextFiles.Name = "menuItemTextFiles";
		menuItemTextFiles.Size = new Size(201, 22);
		menuItemTextFiles.Text = "&Text files";
		// 
		// menuItemSaveAsText
		// 
		menuItemSaveAsText.AccessibleDescription = "Saves the list as text file";
		menuItemSaveAsText.AccessibleName = "Save as text";
		menuItemSaveAsText.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsText.AutoToolTip = true;
		menuItemSaveAsText.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsText.Name = "menuItemSaveAsText";
		menuItemSaveAsText.Size = new Size(201, 22);
		menuItemSaveAsText.Text = "Save as &text";
		// 
		// menuItemSaveAsLatex
		// 
		menuItemSaveAsLatex.AccessibleDescription = "Saves the list as Latex file";
		menuItemSaveAsLatex.AccessibleName = "Save as Latex";
		menuItemSaveAsLatex.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsLatex.AutoToolTip = true;
		menuItemSaveAsLatex.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsLatex.Name = "menuItemSaveAsLatex";
		menuItemSaveAsLatex.Size = new Size(201, 22);
		menuItemSaveAsLatex.Text = "Save as &Latex";
		// 
		// menuItemSaveAsMarkdown
		// 
		menuItemSaveAsMarkdown.AccessibleDescription = "Saves the list as Markdown file";
		menuItemSaveAsMarkdown.AccessibleName = "Save as Markdown";
		menuItemSaveAsMarkdown.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsMarkdown.AutoToolTip = true;
		menuItemSaveAsMarkdown.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsMarkdown.Name = "menuItemSaveAsMarkdown";
		menuItemSaveAsMarkdown.Size = new Size(201, 22);
		menuItemSaveAsMarkdown.Text = "Save as &Markdown";
		// 
		// menuItemSaveAsAsciiDoc
		// 
		menuItemSaveAsAsciiDoc.AccessibleDescription = "Saves the list as AsciiDoc file";
		menuItemSaveAsAsciiDoc.AccessibleName = "Save as AsciiDoc";
		menuItemSaveAsAsciiDoc.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsAsciiDoc.AutoToolTip = true;
		menuItemSaveAsAsciiDoc.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsAsciiDoc.Name = "menuItemSaveAsAsciiDoc";
		menuItemSaveAsAsciiDoc.Size = new Size(201, 22);
		menuItemSaveAsAsciiDoc.Text = "Save as &AsciiDoc";
		// 
		// menuItemSaveAsReStructuredText
		// 
		menuItemSaveAsReStructuredText.AccessibleDescription = "Saves the list as reStructuredText file";
		menuItemSaveAsReStructuredText.AccessibleName = "Save as reStructuredText";
		menuItemSaveAsReStructuredText.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsReStructuredText.AutoToolTip = true;
		menuItemSaveAsReStructuredText.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsReStructuredText.Name = "menuItemSaveAsReStructuredText";
		menuItemSaveAsReStructuredText.Size = new Size(201, 22);
		menuItemSaveAsReStructuredText.Text = "Save as &reStructuredText";
		// 
		// menuItemSaveAsTextile
		// 
		menuItemSaveAsTextile.AccessibleDescription = "Saves the list as Textile file";
		menuItemSaveAsTextile.AccessibleName = "Save as Textile";
		menuItemSaveAsTextile.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsTextile.AutoToolTip = true;
		menuItemSaveAsTextile.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsTextile.Name = "menuItemSaveAsTextile";
		menuItemSaveAsTextile.Size = new Size(201, 22);
		menuItemSaveAsTextile.Text = "Save as Te&xtile";
		// 
		// menuItemSaveAsTypst
		// 
		menuItemSaveAsTypst.AccessibleDescription = "Saves the list as Typst file";
		menuItemSaveAsTypst.AccessibleName = "Save as Typst";
		menuItemSaveAsTypst.AutoToolTip = true;
		menuItemSaveAsTypst.Image = Resources.FatcowIcons16px.fatcow_page_white_text_16px;
		menuItemSaveAsTypst.Name = "menuItemSaveAsTypst";
		menuItemSaveAsTypst.Size = new Size(201, 22);
		menuItemSaveAsTypst.Text = "Save as T&ypst";
		// 
		// menuItemWriterDocuments
		// 
		menuItemWriterDocuments.AccessibleDescription = "Saves the list as writer document";
		menuItemWriterDocuments.AccessibleName = "Save as writer document";
		menuItemWriterDocuments.AccessibleRole = AccessibleRole.MenuItem;
		menuItemWriterDocuments.AutoToolTip = true;
		menuItemWriterDocuments.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsWord, menuItemSaveAsOdt, menuItemSaveAsRtf, menuItemSaveAsAbiword, menuItemSaveAsWps });
		menuItemWriterDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_doc_16px;
		menuItemWriterDocuments.Name = "menuItemWriterDocuments";
		menuItemWriterDocuments.Size = new Size(201, 22);
		menuItemWriterDocuments.Text = "&Writer documents";
		// 
		// menuItemSaveAsWord
		// 
		menuItemSaveAsWord.AccessibleDescription = "Saves the list as Word file";
		menuItemSaveAsWord.AccessibleName = "Save as Word";
		menuItemSaveAsWord.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsWord.AutoToolTip = true;
		menuItemSaveAsWord.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		menuItemSaveAsWord.Name = "menuItemSaveAsWord";
		menuItemSaveAsWord.Size = new Size(257, 22);
		menuItemSaveAsWord.Text = "Save as &Word Text (DOCX)";
		// 
		// menuItemSaveAsOdt
		// 
		menuItemSaveAsOdt.AccessibleDescription = "Saves the list as ODT file";
		menuItemSaveAsOdt.AccessibleName = "Save as ODT";
		menuItemSaveAsOdt.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsOdt.AutoToolTip = true;
		menuItemSaveAsOdt.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		menuItemSaveAsOdt.Name = "menuItemSaveAsOdt";
		menuItemSaveAsOdt.Size = new Size(257, 22);
		menuItemSaveAsOdt.Text = "Save as &OpenDocument Text (ODT)";
		// 
		// menuItemSaveAsRtf
		// 
		menuItemSaveAsRtf.AccessibleDescription = "Saves the list as RTF file";
		menuItemSaveAsRtf.AccessibleName = "Save as RTF";
		menuItemSaveAsRtf.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsRtf.AutoToolTip = true;
		menuItemSaveAsRtf.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		menuItemSaveAsRtf.Name = "menuItemSaveAsRtf";
		menuItemSaveAsRtf.Size = new Size(257, 22);
		menuItemSaveAsRtf.Text = "Save as &Rich Text Format (RTF)";
		// 
		// menuItemSaveAsAbiword
		// 
		menuItemSaveAsAbiword.AccessibleDescription = "Saves the list as Abiword file";
		menuItemSaveAsAbiword.AccessibleName = "Save as Abiword";
		menuItemSaveAsAbiword.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsAbiword.AutoToolTip = true;
		menuItemSaveAsAbiword.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		menuItemSaveAsAbiword.Name = "menuItemSaveAsAbiword";
		menuItemSaveAsAbiword.Size = new Size(257, 22);
		menuItemSaveAsAbiword.Text = "Save as &Abiword file (ABW)";
		// 
		// menuItemSaveAsWps
		// 
		menuItemSaveAsWps.AccessibleDescription = "Saves the list as WPS file";
		menuItemSaveAsWps.AccessibleName = "Save as WPS";
		menuItemSaveAsWps.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsWps.AutoToolTip = true;
		menuItemSaveAsWps.Image = Resources.FatcowIcons16px.fatcow_page_white_word_16px;
		menuItemSaveAsWps.Name = "menuItemSaveAsWps";
		menuItemSaveAsWps.Size = new Size(257, 22);
		menuItemSaveAsWps.Text = "Save as W&PS Office Writer (WPS)";
		// 
		// menuItemSpreadsheetDocuments
		// 
		menuItemSpreadsheetDocuments.AccessibleDescription = "Saves the list as spreadsheet document";
		menuItemSpreadsheetDocuments.AccessibleName = "Save as spreadsheet document";
		menuItemSpreadsheetDocuments.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSpreadsheetDocuments.AutoToolTip = true;
		menuItemSpreadsheetDocuments.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsExcel, menuItemSaveAsOds, menuItemSaveAsCsv, menuItemSaveAsTsv, menuItemSaveAsPsv, menuItemSaveAsEt });
		menuItemSpreadsheetDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_xls_16px;
		menuItemSpreadsheetDocuments.Name = "menuItemSpreadsheetDocuments";
		menuItemSpreadsheetDocuments.Size = new Size(201, 22);
		menuItemSpreadsheetDocuments.Text = "&Spreadsheet documents";
		// 
		// menuItemSaveAsExcel
		// 
		menuItemSaveAsExcel.AccessibleDescription = "Saves the list as Excel file";
		menuItemSaveAsExcel.AccessibleName = "Save as Excel";
		menuItemSaveAsExcel.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsExcel.AutoToolTip = true;
		menuItemSaveAsExcel.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsExcel.Name = "menuItemSaveAsExcel";
		menuItemSaveAsExcel.Size = new Size(301, 22);
		menuItemSaveAsExcel.Text = "Save as &Excel Spreadsheet (XLSX)";
		// 
		// menuItemSaveAsOds
		// 
		menuItemSaveAsOds.AccessibleDescription = "Saves the list as ODS file";
		menuItemSaveAsOds.AccessibleName = "Save as ODS";
		menuItemSaveAsOds.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsOds.AutoToolTip = true;
		menuItemSaveAsOds.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsOds.Name = "menuItemSaveAsOds";
		menuItemSaveAsOds.Size = new Size(301, 22);
		menuItemSaveAsOds.Text = "Save as &OpenDocument Spreadsheet (ODS)";
		// 
		// menuItemSaveAsCsv
		// 
		menuItemSaveAsCsv.AccessibleDescription = "Saves the list as CSV file";
		menuItemSaveAsCsv.AccessibleName = "Save as CSV";
		menuItemSaveAsCsv.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsCsv.AutoToolTip = true;
		menuItemSaveAsCsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsCsv.Name = "menuItemSaveAsCsv";
		menuItemSaveAsCsv.Size = new Size(301, 22);
		menuItemSaveAsCsv.Text = "Save as &Comma separated value (CSV)";
		// 
		// menuItemSaveAsTsv
		// 
		menuItemSaveAsTsv.AccessibleDescription = "Saves the list as TSV file";
		menuItemSaveAsTsv.AccessibleName = "Save as TSV";
		menuItemSaveAsTsv.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsTsv.AutoToolTip = true;
		menuItemSaveAsTsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsTsv.Name = "menuItemSaveAsTsv";
		menuItemSaveAsTsv.Size = new Size(301, 22);
		menuItemSaveAsTsv.Text = "Save as &Tabulator separated value (TSV)";
		// 
		// menuItemSaveAsPsv
		// 
		menuItemSaveAsPsv.AccessibleDescription = "Saves the list as PSV file";
		menuItemSaveAsPsv.AccessibleName = "Save as PSV";
		menuItemSaveAsPsv.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsPsv.AutoToolTip = true;
		menuItemSaveAsPsv.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsPsv.Name = "menuItemSaveAsPsv";
		menuItemSaveAsPsv.Size = new Size(301, 22);
		menuItemSaveAsPsv.Text = "Save as &Pipe separated value (PSV)";
		// 
		// menuItemSaveAsEt
		// 
		menuItemSaveAsEt.AccessibleDescription = "Saves the list as ET";
		menuItemSaveAsEt.AccessibleName = "Save as ET";
		menuItemSaveAsEt.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsEt.AutoToolTip = true;
		menuItemSaveAsEt.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		menuItemSaveAsEt.Name = "menuItemSaveAsEt";
		menuItemSaveAsEt.Size = new Size(301, 22);
		menuItemSaveAsEt.Text = "Save as &WPS Office Spreadsheet (ET)";
		// 
		// menuItemXmlDocuments
		// 
		menuItemXmlDocuments.AccessibleDescription = "Saves the list as XML documents";
		menuItemXmlDocuments.AccessibleName = "Save as XML documents";
		menuItemXmlDocuments.AccessibleRole = AccessibleRole.MenuItem;
		menuItemXmlDocuments.AutoToolTip = true;
		menuItemXmlDocuments.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsHtml, menuItemSaveAsXml, menuItemSaveAsDocBook });
		menuItemXmlDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_bin_16px;
		menuItemXmlDocuments.Name = "menuItemXmlDocuments";
		menuItemXmlDocuments.Size = new Size(201, 22);
		menuItemXmlDocuments.Text = "&XML documents";
		// 
		// menuItemSaveAsHtml
		// 
		menuItemSaveAsHtml.AccessibleDescription = "Saves the list as HTML file";
		menuItemSaveAsHtml.AccessibleName = "Save as HTML";
		menuItemSaveAsHtml.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsHtml.AutoToolTip = true;
		menuItemSaveAsHtml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		menuItemSaveAsHtml.Name = "menuItemSaveAsHtml";
		menuItemSaveAsHtml.Size = new Size(163, 22);
		menuItemSaveAsHtml.Text = "Save as &HTML";
		// 
		// menuItemSaveAsXml
		// 
		menuItemSaveAsXml.AccessibleDescription = "Saves the list as XML file";
		menuItemSaveAsXml.AccessibleName = "Save as XML";
		menuItemSaveAsXml.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsXml.AutoToolTip = true;
		menuItemSaveAsXml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		menuItemSaveAsXml.Name = "menuItemSaveAsXml";
		menuItemSaveAsXml.Size = new Size(163, 22);
		menuItemSaveAsXml.Text = "Save as &XML";
		// 
		// menuItemSaveAsDocBook
		// 
		menuItemSaveAsDocBook.AccessibleDescription = "Saves the list as DocBook file";
		menuItemSaveAsDocBook.AccessibleName = "Save as DocBook";
		menuItemSaveAsDocBook.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsDocBook.AutoToolTip = true;
		menuItemSaveAsDocBook.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		menuItemSaveAsDocBook.Name = "menuItemSaveAsDocBook";
		menuItemSaveAsDocBook.Size = new Size(163, 22);
		menuItemSaveAsDocBook.Text = "Save as &DocBook";
		// 
		// menuItemConfigurationFiles
		// 
		menuItemConfigurationFiles.AccessibleDescription = "Saves the list as configuration file";
		menuItemConfigurationFiles.AccessibleName = "Save as configuration file";
		menuItemConfigurationFiles.AccessibleRole = AccessibleRole.MenuItem;
		menuItemConfigurationFiles.AutoToolTip = true;
		menuItemConfigurationFiles.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsJson, menuItemSaveAsYaml, menuItemSaveAsToml });
		menuItemConfigurationFiles.Image = Resources.FatcowIcons16px.fatcow_file_extension_bat_16px;
		menuItemConfigurationFiles.Name = "menuItemConfigurationFiles";
		menuItemConfigurationFiles.Size = new Size(201, 22);
		menuItemConfigurationFiles.Text = "&Configuration files";
		// 
		// menuItemSaveAsJson
		// 
		menuItemSaveAsJson.AccessibleDescription = "Saves the list as JSON file";
		menuItemSaveAsJson.AccessibleName = "Save as JSON";
		menuItemSaveAsJson.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsJson.AutoToolTip = true;
		menuItemSaveAsJson.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		menuItemSaveAsJson.Name = "menuItemSaveAsJson";
		menuItemSaveAsJson.Size = new Size(180, 22);
		menuItemSaveAsJson.Text = "Save as &JSON";
		// 
		// menuItemSaveAsYaml
		// 
		menuItemSaveAsYaml.AccessibleDescription = "Saves the list as YAML file";
		menuItemSaveAsYaml.AccessibleName = "Save as YAML";
		menuItemSaveAsYaml.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsYaml.AutoToolTip = true;
		menuItemSaveAsYaml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		menuItemSaveAsYaml.Name = "menuItemSaveAsYaml";
		menuItemSaveAsYaml.Size = new Size(180, 22);
		menuItemSaveAsYaml.Text = "Save as &YAML";
		// 
		// menuItemSaveAsToml
		// 
		menuItemSaveAsToml.AccessibleDescription = "Saves the list as TOML file";
		menuItemSaveAsToml.AccessibleName = "Save as TOML";
		menuItemSaveAsToml.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsToml.AutoToolTip = true;
		menuItemSaveAsToml.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		menuItemSaveAsToml.Name = "menuItemSaveAsToml";
		menuItemSaveAsToml.Size = new Size(180, 22);
		menuItemSaveAsToml.Text = "Save as &TOML";
		// 
		// menuItemDatabaseScripts
		// 
		menuItemDatabaseScripts.AccessibleDescription = "Saves the list as database script";
		menuItemDatabaseScripts.AccessibleName = "Save as database script";
		menuItemDatabaseScripts.AccessibleRole = AccessibleRole.MenuItem;
		menuItemDatabaseScripts.AutoToolTip = true;
		menuItemDatabaseScripts.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsSql, menuItemSaveAsSqlite });
		menuItemDatabaseScripts.Image = Resources.FatcowIcons16px.fatcow_file_extension_ptb_16px;
		menuItemDatabaseScripts.Name = "menuItemDatabaseScripts";
		menuItemDatabaseScripts.Size = new Size(201, 22);
		menuItemDatabaseScripts.Text = "&Database scripts";
		// 
		// menuItemSaveAsSql
		// 
		menuItemSaveAsSql.AccessibleDescription = "Saves the list as SQL script";
		menuItemSaveAsSql.AccessibleName = "Save as SQL";
		menuItemSaveAsSql.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsSql.AutoToolTip = true;
		menuItemSaveAsSql.Image = Resources.FatcowIcons16px.fatcow_page_white_database_16px;
		menuItemSaveAsSql.Name = "menuItemSaveAsSql";
		menuItemSaveAsSql.Size = new Size(168, 22);
		menuItemSaveAsSql.Text = "Save as &SQL script";
		// 
		// menuItemSaveAsSqlite
		// 
		menuItemSaveAsSqlite.AccessibleDescription = "Saves the list as SQLite file";
		menuItemSaveAsSqlite.AccessibleName = "Save as SQLite";
		menuItemSaveAsSqlite.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsSqlite.AutoToolTip = true;
		menuItemSaveAsSqlite.Image = Resources.FatcowIcons16px.fatcow_page_white_database_16px;
		menuItemSaveAsSqlite.Name = "menuItemSaveAsSqlite";
		menuItemSaveAsSqlite.Size = new Size(168, 22);
		menuItemSaveAsSqlite.Text = "Save as SQ&Lite";
		// 
		// menuItemPortableDocuments
		// 
		menuItemPortableDocuments.AccessibleDescription = "Saves the list as portable document";
		menuItemPortableDocuments.AccessibleName = "Save as portable document";
		menuItemPortableDocuments.AccessibleRole = AccessibleRole.MenuItem;
		menuItemPortableDocuments.AutoToolTip = true;
		menuItemPortableDocuments.DropDownItems.AddRange(new ToolStripItem[] { menuItemSaveAsPdf, menuItemSaveAsPostScript, menuItemSaveAsEpub, menuItemSaveAsMobi, menuItemSaveAsXps, menuItemSaveAsFictionBook2, menuItemSaveAsChm });
		menuItemPortableDocuments.Image = Resources.FatcowIcons16px.fatcow_file_extension_pdf_16px;
		menuItemPortableDocuments.Name = "menuItemPortableDocuments";
		menuItemPortableDocuments.Size = new Size(201, 22);
		menuItemPortableDocuments.Text = "&Portable documents";
		// 
		// menuItemSaveAsPdf
		// 
		menuItemSaveAsPdf.AccessibleDescription = "Saves the list as PDF file";
		menuItemSaveAsPdf.AccessibleName = "Save as PDF";
		menuItemSaveAsPdf.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsPdf.AutoToolTip = true;
		menuItemSaveAsPdf.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsPdf.Name = "menuItemSaveAsPdf";
		menuItemSaveAsPdf.Size = new Size(214, 22);
		menuItemSaveAsPdf.Text = "Save as &PDF";
		// 
		// menuItemSaveAsPostScript
		// 
		menuItemSaveAsPostScript.AccessibleDescription = "Saves the list as PostScript file";
		menuItemSaveAsPostScript.AccessibleName = "Save as PostScript";
		menuItemSaveAsPostScript.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsPostScript.AutoToolTip = true;
		menuItemSaveAsPostScript.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsPostScript.Name = "menuItemSaveAsPostScript";
		menuItemSaveAsPostScript.Size = new Size(214, 22);
		menuItemSaveAsPostScript.Text = "Save as Post&Script (PS)";
		// 
		// menuItemSaveAsEpub
		// 
		menuItemSaveAsEpub.AccessibleDescription = "Saves the list as EPUB file";
		menuItemSaveAsEpub.AccessibleName = "Save as EPUB";
		menuItemSaveAsEpub.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsEpub.AutoToolTip = true;
		menuItemSaveAsEpub.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsEpub.Name = "menuItemSaveAsEpub";
		menuItemSaveAsEpub.Size = new Size(214, 22);
		menuItemSaveAsEpub.Text = "Save as &EPUB";
		// 
		// menuItemSaveAsMobi
		// 
		menuItemSaveAsMobi.AccessibleDescription = "Saves the list as MOBI file";
		menuItemSaveAsMobi.AccessibleName = "Save as MOBI";
		menuItemSaveAsMobi.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsMobi.AutoToolTip = true;
		menuItemSaveAsMobi.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsMobi.Name = "menuItemSaveAsMobi";
		menuItemSaveAsMobi.Size = new Size(214, 22);
		menuItemSaveAsMobi.Text = "Save as &MOBI";
		// 
		// menuItemSaveAsXps
		// 
		menuItemSaveAsXps.AccessibleDescription = "Saves the list as XPS file";
		menuItemSaveAsXps.AccessibleName = "Save as XPS";
		menuItemSaveAsXps.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsXps.AutoToolTip = true;
		menuItemSaveAsXps.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsXps.Name = "menuItemSaveAsXps";
		menuItemSaveAsXps.Size = new Size(214, 22);
		menuItemSaveAsXps.Text = "Save as &XPS";
		// 
		// menuItemSaveAsFictionBook2
		// 
		menuItemSaveAsFictionBook2.AccessibleDescription = "Saves the list as FictionBook2 file";
		menuItemSaveAsFictionBook2.AccessibleName = "Save as FB2";
		menuItemSaveAsFictionBook2.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsFictionBook2.AutoToolTip = true;
		menuItemSaveAsFictionBook2.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsFictionBook2.Name = "menuItemSaveAsFictionBook2";
		menuItemSaveAsFictionBook2.Size = new Size(214, 22);
		menuItemSaveAsFictionBook2.Text = "Save as &FictionBook2 (FB2)";
		// 
		// menuItemSaveAsChm
		// 
		menuItemSaveAsChm.AccessibleDescription = "Saves the list as CHM file";
		menuItemSaveAsChm.AccessibleName = "Save as CHM";
		menuItemSaveAsChm.AccessibleRole = AccessibleRole.MenuItem;
		menuItemSaveAsChm.AutoToolTip = true;
		menuItemSaveAsChm.Image = Resources.FatcowIcons16px.fatcow_page_white_acrobat_16px;
		menuItemSaveAsChm.Name = "menuItemSaveAsChm";
		menuItemSaveAsChm.Size = new Size(214, 22);
		menuItemSaveAsChm.Text = "Save as &CHM";
		// 
		// buttonApply
		// 
		buttonApply.AccessibleDescription = "Closes the window with committing the coordinates.";
		buttonApply.AccessibleName = "Commit the coordinates";
		buttonApply.AccessibleRole = AccessibleRole.PushButton;
		buttonApply.Image = Resources.FatcowIcons16px.fatcow_tick_octagon_16px;
		buttonApply.ImageTransparentColor = Color.Magenta;
		buttonApply.Name = "buttonApply";
		buttonApply.Size = new Size(58, 22);
		buttonApply.Text = "&Apply";
		// 
		// buttonCancel
		// 
		buttonCancel.AccessibleDescription = I18nStrings.WorldMapCancelAccessibleDescription;
		buttonCancel.AccessibleName = "Cancel";
		buttonCancel.AccessibleRole = AccessibleRole.PushButton;
		buttonCancel.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Size = new Size(63, 22);
		buttonCancel.Text = "&Cancel";
		// 
		// separator1
		// 
		separator1.AccessibleDescription = "Just a separator";
		separator1.AccessibleName = "Just a separator";
		separator1.AccessibleRole = AccessibleRole.Separator;
		separator1.Name = "separator1";
		separator1.Size = new Size(6, 25);
		// 
		// SingleAsteroidForm
		// 
		AccessibleRole = AccessibleRole.Window;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		AutoScroll = true;
		ClientSize = new Size(739, 297);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "SingleAsteroidForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Create or edit a single asteroid";
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		statusStrip.ResumeLayout(false);
		statusStrip.PerformLayout();
		((System.ComponentModel.ISupportInitialize)panelMain).EndInit();
		panelMain.ResumeLayout(false);
		toolStripMain.ResumeLayout(false);
		toolStripMain.PerformLayout();
		tableLayoutPanelMpcorbData.ResumeLayout(false);
		tableLayoutPanelMpcorbData.PerformLayout();
		contextMenuShowControlsInStatusBar.ResumeLayout(false);
		contextMenuSaveToFile.ResumeLayout(false);
		ResumeLayout(false);
	}

	#endregion

	private ToolStripContainer toolStripContainer;
	private Krypton.Toolkit.KryptonStatusStrip statusStrip;
	private ToolStripStatusLabel labelInformation;
	private Krypton.Toolkit.KryptonPanel panelMain;
	private Krypton.Toolkit.KryptonToolStrip toolStripMain;
	private Krypton.Toolkit.KryptonTableLayoutPanel tableLayoutPanelMpcorbData;
	private Krypton.Toolkit.KryptonLabel labelMpcorbIndexData;
	private Krypton.Toolkit.KryptonLabel labelMpcorbIndex;
	private Krypton.Toolkit.KryptonLabel labelMpcorbReadableDesignation;
	private Krypton.Toolkit.KryptonLabel labelMpcorbEpoch;
	private Krypton.Toolkit.KryptonLabel labelMpcorbMeanAnomalyAtTheEpoch;
	private Krypton.Toolkit.KryptonLabel labelMpcorbArgumentOfThePerihelion;
	private Krypton.Toolkit.KryptonLabel labelMpcorbLongitudeOfTheAscendingNode;
	private Krypton.Toolkit.KryptonLabel labelInclinationToTheEcliptic;
	private Krypton.Toolkit.KryptonLabel labelMpcorbOrbitalEccentricity;
	private Krypton.Toolkit.KryptonLabel labelMpcorbMeanDailyMotion;
	private Krypton.Toolkit.KryptonLabel labelMpcorbSemiMajorAxis;
	private Krypton.Toolkit.KryptonLabel labelMpcorbAbsoluteMagnitude;
	private Krypton.Toolkit.KryptonLabel labelMpcorbSlopeParameter;
	private Krypton.Toolkit.KryptonLabel labelMpcorbReference;
	private Krypton.Toolkit.KryptonLabel labelMpcorbNumberOfOppositions;
	private Krypton.Toolkit.KryptonLabel labelMpcorbNumberOfObservations;
	private Krypton.Toolkit.KryptonLabel labelMpcorbObservationSpan;
	private Krypton.Toolkit.KryptonLabel labelMpcorbRmsResidual;
	private Krypton.Toolkit.KryptonLabel labelMpcorbComputerName;
	private Krypton.Toolkit.KryptonLabel labelMpcorbFlags;
	private Krypton.Toolkit.KryptonLabel labelMpcorbDateLastObservation;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbReadableDesignation;
	private Krypton.Toolkit.KryptonDateTimePicker dateTimePickerMpcorbEpoch;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbMeanAnomalyAtTheEpoch;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbArgumentOfThePerihelion;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbLongitudeOfTheAscendingNode;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownInclinationToTheEcliptic;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbOrbitalEccentricity;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbMeanDailyMotion;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbSemiMajorAxis;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbAbsoluteMagnitude;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbSlopeParameter;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbReference;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbNumberOfOppositions;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbNumberOfObservations;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbObservationSpan;
	private Krypton.Toolkit.KryptonNumericUpDown numericUpDownMpcorbRmsResidual;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbComputerName;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbFlags;
	private Krypton.Toolkit.KryptonTextBox textBoxMpcorbDateLastObservation;
	private ToolStripDropDownButton dropDownButtonSaveList;
	private ContextMenuStrip contextMenuShowControlsInStatusBar;
	private ToolStripMenuItem menuItemShowInformationLabel;
	private ContextMenuStrip contextMenuSaveToFile;
	private ToolStripMenuItem menuItemTextFiles;
	private ToolStripMenuItem menuItemSaveAsText;
	private ToolStripMenuItem menuItemSaveAsLatex;
	private ToolStripMenuItem menuItemSaveAsMarkdown;
	private ToolStripMenuItem menuItemSaveAsAsciiDoc;
	private ToolStripMenuItem menuItemSaveAsReStructuredText;
	private ToolStripMenuItem menuItemSaveAsTextile;
	private ToolStripMenuItem menuItemSaveAsTypst;
	private ToolStripMenuItem menuItemWriterDocuments;
	private ToolStripMenuItem menuItemSaveAsWord;
	private ToolStripMenuItem menuItemSaveAsOdt;
	private ToolStripMenuItem menuItemSaveAsRtf;
	private ToolStripMenuItem menuItemSaveAsAbiword;
	private ToolStripMenuItem menuItemSaveAsWps;
	private ToolStripMenuItem menuItemSpreadsheetDocuments;
	private ToolStripMenuItem menuItemSaveAsExcel;
	private ToolStripMenuItem menuItemSaveAsOds;
	private ToolStripMenuItem menuItemSaveAsCsv;
	private ToolStripMenuItem menuItemSaveAsTsv;
	private ToolStripMenuItem menuItemSaveAsPsv;
	private ToolStripMenuItem menuItemSaveAsEt;
	private ToolStripMenuItem menuItemXmlDocuments;
	private ToolStripMenuItem menuItemSaveAsHtml;
	private ToolStripMenuItem menuItemSaveAsXml;
	private ToolStripMenuItem menuItemSaveAsDocBook;
	private ToolStripMenuItem menuItemConfigurationFiles;
	private ToolStripMenuItem menuItemSaveAsJson;
	private ToolStripMenuItem menuItemSaveAsYaml;
	private ToolStripMenuItem menuItemSaveAsToml;
	private ToolStripMenuItem menuItemDatabaseScripts;
	private ToolStripMenuItem menuItemSaveAsSql;
	private ToolStripMenuItem menuItemSaveAsSqlite;
	private ToolStripMenuItem menuItemPortableDocuments;
	private ToolStripMenuItem menuItemSaveAsPdf;
	private ToolStripMenuItem menuItemSaveAsPostScript;
	private ToolStripMenuItem menuItemSaveAsEpub;
	private ToolStripMenuItem menuItemSaveAsMobi;
	private ToolStripMenuItem menuItemSaveAsXps;
	private ToolStripMenuItem menuItemSaveAsFictionBook2;
	private ToolStripMenuItem menuItemSaveAsChm;
	private Krypton.Toolkit.KryptonManager kryptonManager;
	private ToolStripButton buttonApply;
	private ToolStripButton buttonCancel;
	private ToolStripSeparator separator1;
}