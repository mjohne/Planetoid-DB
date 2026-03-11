// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a form that displays data in a table format.</summary>
/// <remarks>This form is used to display data from the MPCORB.DAT file in a tabular format, allowing users to specify a range and view the results.</remarks>
partial class TableModeForm
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(TableModeForm));
		progressBar = new KryptonProgressBar();
		labelMinimum = new KryptonLabel();
		numericUpDownMinimum = new KryptonNumericUpDown();
		numericUpDownMaximum = new KryptonNumericUpDown();
		labelMaximum = new KryptonLabel();
		buttonList = new KryptonButton();
		labelWarning = new Label();
		contextMenuCopyToClipboard = new ContextMenuStrip(components);
		toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
		buttonCancel = new KryptonButton();
		listView = new ListView();
		columnHeaderIndex = new ColumnHeader();
		columnHeaderReadableDesignation = new ColumnHeader();
		columnHeaderEpoch = new ColumnHeader();
		columnHeaderMeanAnomaly = new ColumnHeader();
		columnHeaderArgumentPerihelion = new ColumnHeader();
		columnHeaderLongitudeAscendingNode = new ColumnHeader();
		columnHeaderInclination = new ColumnHeader();
		columnHeaderOrbitalEccentricity = new ColumnHeader();
		columnHeaderMeanDailyMotion = new ColumnHeader();
		columnHeaderSemimajorAxis = new ColumnHeader();
		columnHeaderAbsoluteMagnitude = new ColumnHeader();
		columnHeaderSlopeParameter = new ColumnHeader();
		columnHeaderReference = new ColumnHeader();
		columnHeaderNumberOppositions = new ColumnHeader();
		columnHeaderNumberObservations = new ColumnHeader();
		columnHeaderObservationSpan = new ColumnHeader();
		columnHeaderRmsResidual = new ColumnHeader();
		columnHeaderComputerName = new ColumnHeader();
		columnHeaderFlags = new ColumnHeader();
		columnHeaderDateLastObservation = new ColumnHeader();
		backgroundWorker = new BackgroundWorker();
		kryptoPanelMain = new KryptonPanel();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);
		contextMenuCopyToClipboard.SuspendLayout();
		((ISupportInitialize)kryptoPanelMain).BeginInit();
		kryptoPanelMain.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// progressBar
		// 
		progressBar.AccessibleDescription = "Shows the progress";
		progressBar.AccessibleName = "Progress";
		progressBar.AccessibleRole = AccessibleRole.ProgressBar;
		progressBar.Location = new Point(15, 65);
		progressBar.Name = "progressBar";
		progressBar.Size = new Size(840, 22);
		progressBar.Step = 1;
		progressBar.TabIndex = 7;
		progressBar.TextBackdropColor = Color.Empty;
		progressBar.TextShadowColor = Color.Empty;
		progressBar.Values.Text = "";
		progressBar.MouseEnter += Control_Enter;
		progressBar.MouseLeave += Control_Leave;
		// 
		// labelMinimum
		// 
		labelMinimum.AccessibleDescription = "Shows the minimum";
		labelMinimum.AccessibleName = "Minimum";
		labelMinimum.AccessibleRole = AccessibleRole.Text;
		labelMinimum.Location = new Point(15, 24);
		labelMinimum.Name = "labelMinimum";
		labelMinimum.Size = new Size(77, 23);
		labelMinimum.TabIndex = 0;
		labelMinimum.ToolTipValues.Description = "Shows the minimum.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelMinimum.ToolTipValues.EnableToolTips = true;
		labelMinimum.ToolTipValues.Heading = "Minimum";
		labelMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelMinimum.Values.Text = "M&inimum:";
		labelMinimum.DoubleClick += CopyToClipboard_DoubleClick;
		labelMinimum.Enter += Control_Enter;
		labelMinimum.Leave += Control_Leave;
		labelMinimum.MouseEnter += Control_Enter;
		labelMinimum.MouseLeave += Control_Leave;
		// 
		// numericUpDownMinimum
		// 
		numericUpDownMinimum.AccessibleDescription = "Shows the minimum value for the list";
		numericUpDownMinimum.AccessibleName = "Minimum value for the list";
		numericUpDownMinimum.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownMinimum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMinimum.Location = new Point(96, 21);
		numericUpDownMinimum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMinimum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMinimum.Name = "numericUpDownMinimum";
		numericUpDownMinimum.Size = new Size(75, 22);
		numericUpDownMinimum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
		numericUpDownMinimum.TabIndex = 1;
		numericUpDownMinimum.ToolTipValues.Description = "Shows the minimum value for the list.";
		numericUpDownMinimum.ToolTipValues.EnableToolTips = true;
		numericUpDownMinimum.ToolTipValues.Heading = "Minimum value for the list";
		numericUpDownMinimum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		numericUpDownMinimum.Value = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMinimum.Enter += Control_Enter;
		numericUpDownMinimum.Leave += Control_Leave;
		// 
		// numericUpDownMaximum
		// 
		numericUpDownMaximum.AccessibleDescription = "Shows the maximum value for the list";
		numericUpDownMaximum.AccessibleName = "Maximum value for the list";
		numericUpDownMaximum.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownMaximum.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownMaximum.Location = new Point(259, 21);
		numericUpDownMaximum.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numericUpDownMaximum.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMaximum.Name = "numericUpDownMaximum";
		numericUpDownMaximum.Size = new Size(75, 22);
		numericUpDownMaximum.StateCommon.Content.TextH = PaletteRelativeAlign.Center;
		numericUpDownMaximum.TabIndex = 3;
		numericUpDownMaximum.ToolTipValues.Description = "Shows the maximum value for the list.";
		numericUpDownMaximum.ToolTipValues.EnableToolTips = true;
		numericUpDownMaximum.ToolTipValues.Heading = "Maximum value for the list";
		numericUpDownMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		numericUpDownMaximum.Value = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMaximum.Enter += Control_Enter;
		numericUpDownMaximum.Leave += Control_Leave;
		// 
		// labelMaximum
		// 
		labelMaximum.AccessibleDescription = "Shows the maximum";
		labelMaximum.AccessibleName = "Maximum";
		labelMaximum.AccessibleRole = AccessibleRole.Text;
		labelMaximum.Location = new Point(177, 24);
		labelMaximum.Name = "labelMaximum";
		labelMaximum.Size = new Size(79, 23);
		labelMaximum.TabIndex = 2;
		labelMaximum.ToolTipValues.Description = "Shows the maximum.\r\nDouble-click or right-click to copy the information to the clipboard.";
		labelMaximum.ToolTipValues.EnableToolTips = true;
		labelMaximum.ToolTipValues.Heading = "Maximum";
		labelMaximum.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		labelMaximum.Values.Text = "M&aximum:";
		labelMaximum.DoubleClick += CopyToClipboard_DoubleClick;
		labelMaximum.Enter += Control_Enter;
		labelMaximum.Leave += Control_Leave;
		labelMaximum.MouseEnter += Control_Enter;
		labelMaximum.MouseLeave += Control_Leave;
		// 
		// buttonList
		// 
		buttonList.AccessibleDescription = "Starts the progress and list";
		buttonList.AccessibleName = "List";
		buttonList.AccessibleRole = AccessibleRole.PushButton;
		buttonList.Location = new Point(341, 16);
		buttonList.Name = "buttonList";
		buttonList.Size = new Size(61, 36);
		buttonList.TabIndex = 4;
		buttonList.ToolTipValues.Description = "Starts the progress and list.";
		buttonList.ToolTipValues.EnableToolTips = true;
		buttonList.ToolTipValues.Heading = "List";
		buttonList.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		buttonList.Values.DropDownArrowColor = Color.Empty;
		buttonList.Values.Image = FatcowIcons16px.fatcow_table_16px;
		buttonList.Values.Text = "&List";
		buttonList.Click += ButtonList_ClickAsync;
		buttonList.Enter += Control_Enter;
		buttonList.Leave += Control_Leave;
		buttonList.MouseEnter += Control_Enter;
		buttonList.MouseLeave += Control_Leave;
		// 
		// labelWarning
		// 
		labelWarning.AccessibleDescription = "Warning message: Be careful: do not use large ranges between minimum and maximum! This can increase loading time and memory. Use small spans!";
		labelWarning.AccessibleName = "Warning message";
		labelWarning.AccessibleRole = AccessibleRole.Text;
		labelWarning.BackColor = Color.SeaShell;
		labelWarning.BorderStyle = BorderStyle.Fixed3D;
		labelWarning.ContextMenuStrip = contextMenuCopyToClipboard;
		labelWarning.Font = new Font("Segoe UI", 7F);
		labelWarning.Location = new Point(496, 16);
		labelWarning.Margin = new Padding(3);
		labelWarning.Name = "labelWarning";
		labelWarning.Size = new Size(359, 45);
		labelWarning.TabIndex = 6;
		labelWarning.Text = "Be careful: Do not use large ranges between minimum and maximum! This can increase loading time and memory. Use small ranges! You can cancel any time.";
		labelWarning.TextAlign = ContentAlignment.MiddleLeft;
		labelWarning.DoubleClick += CopyToClipboard_DoubleClick;
		labelWarning.Enter += Control_Enter;
		labelWarning.Leave += Control_Leave;
		labelWarning.MouseDown += Control_MouseDown;
		labelWarning.MouseEnter += Control_Enter;
		labelWarning.MouseLeave += Control_Leave;
		// 
		// contextMenuCopyToClipboard
		// 
		contextMenuCopyToClipboard.AccessibleDescription = "Shows context menu for copying to clipboard";
		contextMenuCopyToClipboard.AccessibleName = "Context menu for copying to clipboard";
		contextMenuCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuCopyToClipboard.AllowClickThrough = true;
		contextMenuCopyToClipboard.Font = new Font("Segoe UI", 9F);
		contextMenuCopyToClipboard.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCopyToClipboard });
		contextMenuCopyToClipboard.Name = "contextMenuStrip";
		contextMenuCopyToClipboard.Size = new Size(214, 26);
		contextMenuCopyToClipboard.TabStop = true;
		contextMenuCopyToClipboard.Text = "Context menu for copying to clipboard";
		contextMenuCopyToClipboard.MouseEnter += Control_Enter;
		contextMenuCopyToClipboard.MouseLeave += Control_Leave;
		// 
		// toolStripMenuItemCopyToClipboard
		// 
		toolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemCopyToClipboard.AutoToolTip = true;
		toolStripMenuItemCopyToClipboard.Image = FatcowIcons16px.fatcow_page_copy_16px;
		toolStripMenuItemCopyToClipboard.Name = "toolStripMenuItemCopyToClipboard";
		toolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
		toolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
		toolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
		toolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
		toolStripMenuItemCopyToClipboard.Click += CopyToClipboard_DoubleClick;
		toolStripMenuItemCopyToClipboard.MouseEnter += Control_Enter;
		toolStripMenuItemCopyToClipboard.MouseLeave += Control_Leave;
		// 
		// buttonCancel
		// 
		buttonCancel.AccessibleDescription = "Cancels the progress";
		buttonCancel.AccessibleName = "Cancel";
		buttonCancel.AccessibleRole = AccessibleRole.PushButton;
		buttonCancel.Location = new Point(408, 16);
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Size = new Size(80, 36);
		buttonCancel.TabIndex = 5;
		buttonCancel.ToolTipValues.Description = "Cancels the progress.";
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
		// listView
		// 
		listView.AccessibleDescription = "Shows the list with the items";
		listView.AccessibleName = "List";
		listView.AccessibleRole = AccessibleRole.ListItem;
		listView.Activation = ItemActivation.OneClick;
		listView.AllowColumnReorder = true;
		listView.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderReadableDesignation, columnHeaderEpoch, columnHeaderMeanAnomaly, columnHeaderArgumentPerihelion, columnHeaderLongitudeAscendingNode, columnHeaderInclination, columnHeaderOrbitalEccentricity, columnHeaderMeanDailyMotion, columnHeaderSemimajorAxis, columnHeaderAbsoluteMagnitude, columnHeaderSlopeParameter, columnHeaderReference, columnHeaderNumberOppositions, columnHeaderNumberObservations, columnHeaderObservationSpan, columnHeaderRmsResidual, columnHeaderComputerName, columnHeaderFlags, columnHeaderDateLastObservation });
		listView.Font = new Font("Segoe UI", 8.5F);
		listView.FullRowSelect = true;
		listView.GridLines = true;
		listView.Location = new Point(15, 93);
		listView.MultiSelect = false;
		listView.Name = "listView";
		listView.ShowItemToolTips = true;
		listView.Size = new Size(839, 344);
		listView.TabIndex = 8;
		listView.UseCompatibleStateImageBehavior = false;
		listView.View = View.Details;
		listView.VirtualMode = true;
		listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		listView.SelectedIndexChanged += ListViewTableMode_SelectedIndexChanged;
		listView.Enter += Control_Enter;
		listView.Leave += Control_Leave;
		listView.MouseEnter += Control_Enter;
		listView.MouseLeave += Control_Leave;
		// 
		// columnHeaderIndex
		// 
		columnHeaderIndex.Text = "Index No.";
		// 
		// columnHeaderReadableDesignation
		// 
		columnHeaderReadableDesignation.Text = "Readable designation";
		columnHeaderReadableDesignation.Width = 120;
		// 
		// columnHeaderEpoch
		// 
		columnHeaderEpoch.Text = "Epoch";
		columnHeaderEpoch.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderMeanAnomaly
		// 
		columnHeaderMeanAnomaly.Text = "Mean anomaly at the epoch, in degrees";
		columnHeaderMeanAnomaly.TextAlign = HorizontalAlignment.Right;
		columnHeaderMeanAnomaly.Width = 70;
		// 
		// columnHeaderArgumentPerihelion
		// 
		columnHeaderArgumentPerihelion.Text = "Argument of perihelion, J2000.0 (degrees)";
		columnHeaderArgumentPerihelion.TextAlign = HorizontalAlignment.Right;
		columnHeaderArgumentPerihelion.Width = 70;
		// 
		// columnHeaderLongitudeAscendingNode
		// 
		columnHeaderLongitudeAscendingNode.Text = "Longitude of the ascending node";
		columnHeaderLongitudeAscendingNode.TextAlign = HorizontalAlignment.Right;
		columnHeaderLongitudeAscendingNode.Width = 70;
		// 
		// columnHeaderInclination
		// 
		columnHeaderInclination.Text = "Inclination";
		columnHeaderInclination.TextAlign = HorizontalAlignment.Right;
		// 
		// columnHeaderOrbitalEccentricity
		// 
		columnHeaderOrbitalEccentricity.Text = "Orbital eccentricity";
		columnHeaderOrbitalEccentricity.TextAlign = HorizontalAlignment.Right;
		columnHeaderOrbitalEccentricity.Width = 70;
		// 
		// columnHeaderMeanDailyMotion
		// 
		columnHeaderMeanDailyMotion.Text = "Mean daily motion";
		columnHeaderMeanDailyMotion.TextAlign = HorizontalAlignment.Right;
		columnHeaderMeanDailyMotion.Width = 70;
		// 
		// columnHeaderSemimajorAxis
		// 
		columnHeaderSemimajorAxis.Text = "Semimajor axis";
		columnHeaderSemimajorAxis.TextAlign = HorizontalAlignment.Right;
		columnHeaderSemimajorAxis.Width = 75;
		// 
		// columnHeaderAbsoluteMagnitude
		// 
		columnHeaderAbsoluteMagnitude.Text = "Absolute magnitude";
		columnHeaderAbsoluteMagnitude.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderSlopeParameter
		// 
		columnHeaderSlopeParameter.Text = "Slope parameter";
		columnHeaderSlopeParameter.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderReference
		// 
		columnHeaderReference.Text = "Reference";
		columnHeaderReference.TextAlign = HorizontalAlignment.Center;
		columnHeaderReference.Width = 80;
		// 
		// columnHeaderNumberOppositions
		// 
		columnHeaderNumberOppositions.Text = "Number of oppositions";
		columnHeaderNumberOppositions.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderNumberObservations
		// 
		columnHeaderNumberObservations.Text = "Number of observations";
		columnHeaderNumberObservations.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderObservationSpan
		// 
		columnHeaderObservationSpan.Text = "Observation span";
		columnHeaderObservationSpan.TextAlign = HorizontalAlignment.Center;
		columnHeaderObservationSpan.Width = 80;
		// 
		// columnHeaderRmsResidual
		// 
		columnHeaderRmsResidual.Text = "r.m.s. residual";
		columnHeaderRmsResidual.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderComputerName
		// 
		columnHeaderComputerName.Text = "Computer name";
		columnHeaderComputerName.TextAlign = HorizontalAlignment.Center;
		columnHeaderComputerName.Width = 80;
		// 
		// columnHeaderFlags
		// 
		columnHeaderFlags.Text = "4-hexdigit flags";
		columnHeaderFlags.TextAlign = HorizontalAlignment.Center;
		// 
		// columnHeaderDateLastObservation
		// 
		columnHeaderDateLastObservation.Text = "Date of last observation";
		columnHeaderDateLastObservation.TextAlign = HorizontalAlignment.Center;
		// 
		// backgroundWorker
		// 
		backgroundWorker.WorkerReportsProgress = true;
		backgroundWorker.WorkerSupportsCancellation = true;
		// 
		// kryptoPanelMain
		// 
		kryptoPanelMain.AccessibleDescription = "Groups the data";
		kryptoPanelMain.AccessibleName = "Panel";
		kryptoPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptoPanelMain.Controls.Add(kryptonStatusStrip);
		kryptoPanelMain.Controls.Add(labelMinimum);
		kryptoPanelMain.Controls.Add(buttonCancel);
		kryptoPanelMain.Controls.Add(listView);
		kryptoPanelMain.Controls.Add(labelWarning);
		kryptoPanelMain.Controls.Add(progressBar);
		kryptoPanelMain.Controls.Add(buttonList);
		kryptoPanelMain.Controls.Add(numericUpDownMinimum);
		kryptoPanelMain.Controls.Add(numericUpDownMaximum);
		kryptoPanelMain.Controls.Add(labelMaximum);
		kryptoPanelMain.Dock = DockStyle.Fill;
		kryptoPanelMain.Location = new Point(0, 0);
		kryptoPanelMain.Name = "kryptoPanelMain";
		kryptoPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptoPanelMain.Size = new Size(868, 479);
		kryptoPanelMain.TabIndex = 0;
		kryptoPanelMain.TabStop = true;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar of some information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 457);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.Padding = new Padding(1, 0, 16, 0);
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(868, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 9;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows some information";
		labelInformation.AccessibleName = "Shows some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Shows some information";
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// TableModeForm
		// 
		AccessibleDescription = "Lists the MPCORB.DAT into a  table";
		AccessibleName = "Table Mode";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(868, 479);
		ControlBox = false;
		Controls.Add(kryptoPanelMain);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "TableModeForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Table Mode";
		FormClosing += TableModeForm_FormClosing;
		FormClosed += TableModeForm_FormClosed;
		Load += TableModeForm_Load;
		contextMenuCopyToClipboard.ResumeLayout(false);
		((ISupportInitialize)kryptoPanelMain).EndInit();
		kryptoPanelMain.ResumeLayout(false);
		kryptoPanelMain.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		ResumeLayout(false);
	}

	#endregion
	private ListView listView;
    private ColumnHeader columnHeaderIndex;
    private ColumnHeader columnHeaderReadableDesignation;
    private ColumnHeader columnHeaderEpoch;
    private ColumnHeader columnHeaderMeanAnomaly;
    private ColumnHeader columnHeaderArgumentPerihelion;
    private ColumnHeader columnHeaderLongitudeAscendingNode;
    private ColumnHeader columnHeaderInclination;
    private ColumnHeader columnHeaderOrbitalEccentricity;
    private ColumnHeader columnHeaderMeanDailyMotion;
    private ColumnHeader columnHeaderSemimajorAxis;
    private ColumnHeader columnHeaderAbsoluteMagnitude;
    private ColumnHeader columnHeaderSlopeParameter;
    private ColumnHeader columnHeaderReference;
    private ColumnHeader columnHeaderNumberOppositions;
    private ColumnHeader columnHeaderNumberObservations;
    private ColumnHeader columnHeaderObservationSpan;
    private ColumnHeader columnHeaderRmsResidual;
    private ColumnHeader columnHeaderComputerName;
    private ColumnHeader columnHeaderFlags;
    private ColumnHeader columnHeaderDateLastObservation;
    private KryptonProgressBar progressBar;
    private BackgroundWorker backgroundWorker;
	private KryptonLabel labelMinimum;
	private KryptonNumericUpDown numericUpDownMinimum;
	private KryptonNumericUpDown numericUpDownMaximum;
	private KryptonLabel labelMaximum;
    private KryptonButton buttonList;
    private Label labelWarning;
    private KryptonButton buttonCancel;
        private KryptonPanel kryptoPanelMain;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
	private ContextMenuStrip contextMenuCopyToClipboard;
	private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
}