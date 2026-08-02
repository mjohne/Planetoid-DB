// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents the designer-generated partial class for <see cref="LogViewerForm"/>.</summary>
/// <remarks>This file contains the Windows Forms designer-generated code for the form layout.</remarks>
partial class LogViewerForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	/// <remarks>This method is called by the runtime to release resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing: disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method sets up the controls and their properties for the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		kryptonManager = new KryptonManager(components);
		kryptonPanelMain = new KryptonPanel();
		listView = new ListView();
		columnHeaderDateTime = new ColumnHeader();
		columnHeaderLevel = new ColumnHeader();
		columnHeaderExceptionType = new ColumnHeader();
		columnHeaderMessage = new ColumnHeader();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		toolStripContainer = new ToolStripContainer();
		kryptonToolStripMain = new KryptonToolStrip();
		toolStripButtonDeleteSelected = new ToolStripButton();
		toolStripButtonDeleteAll = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripLabelProgress = new ToolStripLabel();
		kryptonProgressBar = new KryptonProgressBarToolStripItem();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonToolStripMain.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// kryptonPanelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Groups the log event data";
		kryptonPanelMain.AccessibleName = "Log events panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(value: listView);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(x: 0, y: 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(width: 900, height: 450);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "Main panel";
		kryptonPanelMain.Enter += Control_Enter;
		kryptonPanelMain.Leave += Control_Leave;
		kryptonPanelMain.MouseEnter += Control_Enter;
		kryptonPanelMain.MouseLeave += Control_Leave;
		// 
		// listView
		// 
		listView.AccessibleDescription = "Shows all stored NLog log events";
		listView.AccessibleName = "Log events list";
		listView.AccessibleRole = AccessibleRole.List;
		listView.AllowColumnReorder = true;
		listView.Columns.AddRange(values: new ColumnHeader[] { columnHeaderDateTime, columnHeaderLevel, columnHeaderExceptionType, columnHeaderMessage });
		listView.Dock = DockStyle.Fill;
		listView.Font = new Font(familyName: "Segoe UI", emSize: 9F);
		listView.FullRowSelect = true;
		listView.GridLines = true;
		listView.HideSelection = false;
		listView.Location = new Point(x: 0, y: 0);
		listView.MultiSelect = true;
		listView.Name = "listView";
		listView.ShowItemToolTips = true;
		listView.Size = new Size(width: 900, height: 450);
		listView.TabIndex = 0;
		listView.UseCompatibleStateImageBehavior = false;
		listView.View = View.Details;
		listView.VirtualMode = true;
		listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
		listView.Enter += Control_Enter;
		listView.Leave += Control_Leave;
		listView.MouseEnter += Control_Enter;
		listView.MouseLeave += Control_Leave;
		// 
		// columnHeaderDateTime
		// 
		columnHeaderDateTime.Text = "Date / Time";
		columnHeaderDateTime.Width = 150;
		// 
		// columnHeaderLevel
		// 
		columnHeaderLevel.Text = "Level";
		columnHeaderLevel.Width = 70;
		// 
		// columnHeaderExceptionType
		// 
		columnHeaderExceptionType.Text = "Exception type";
		columnHeaderExceptionType.Width = 180;
		// 
		// columnHeaderMessage
		// 
		columnHeaderMessage.Text = "Message";
		columnHeaderMessage.Width = 490;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information about the log events";
		kryptonStatusStrip.AccessibleName = "Status bar";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowDrop = true;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font(familyName: "Segoe UI", emSize: 9F);
		kryptonStatusStrip.Items.AddRange(values: new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(x: 0, y: 0);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(width: 900, height: 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 0;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows information about the log events";
		labelInformation.AccessibleName = "Log viewer information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(width: 144, height: 17);
		labelInformation.Text = "Log viewer";
		labelInformation.ToolTipText = "Shows information about the log events";
		// 
		// toolStripContainer
		// 
		// toolStripContainer.BottomToolStripPanel
		toolStripContainer.BottomToolStripPanel.Controls.Add(value: kryptonStatusStrip);
		// toolStripContainer.ContentPanel
		toolStripContainer.ContentPanel.AccessibleDescription = "Log events view";
		toolStripContainer.ContentPanel.AccessibleName = "Content area";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.ContentPanel.Controls.Add(value: kryptonPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(width: 900, height: 450);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(x: 0, y: 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(width: 900, height: 497);
		toolStripContainer.TabIndex = 0;
		toolStripContainer.Text = "toolStripContainer";
		// toolStripContainer.TopToolStripPanel
		toolStripContainer.TopToolStripPanel.Controls.Add(value: kryptonToolStripMain);
		// 
		// kryptonToolStripMain
		// 
		kryptonToolStripMain.AccessibleDescription = "Provides the toolbar buttons";
		kryptonToolStripMain.AccessibleName = "Toolbar";
		kryptonToolStripMain.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStripMain.Dock = DockStyle.None;
		kryptonToolStripMain.Font = new Font(familyName: "Segoe UI", emSize: 9F);
		kryptonToolStripMain.Items.AddRange(values: new ToolStripItem[] { toolStripButtonDeleteSelected, toolStripButtonDeleteAll, toolStripSeparator1, toolStripLabelProgress, kryptonProgressBar });
		kryptonToolStripMain.Location = new Point(x: 0, y: 0);
		kryptonToolStripMain.Name = "kryptonToolStripMain";
		kryptonToolStripMain.Size = new Size(width: 900, height: 25);
		kryptonToolStripMain.Stretch = true;
		kryptonToolStripMain.TabIndex = 0;
		// 
		// toolStripButtonDeleteSelected
		// 
		toolStripButtonDeleteSelected.AccessibleDescription = "Deletes selected log entries from the list";
		toolStripButtonDeleteSelected.AccessibleName = "Delete selected";
		toolStripButtonDeleteSelected.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonDeleteSelected.AutoToolTip = true;
		toolStripButtonDeleteSelected.Enabled = false;
		toolStripButtonDeleteSelected.Image = FatcowIcons16px.fatcow_bin_16px;
		toolStripButtonDeleteSelected.ImageTransparentColor = Color.Magenta;
		toolStripButtonDeleteSelected.Name = "toolStripButtonDeleteSelected";
		toolStripButtonDeleteSelected.Size = new Size(width: 116, height: 22);
		toolStripButtonDeleteSelected.Text = "&Delete selected";
		toolStripButtonDeleteSelected.ToolTipText = "Deletes the selected log entries from the list and from the log store";
		toolStripButtonDeleteSelected.Click += ToolStripButtonDeleteSelected_Click;
		toolStripButtonDeleteSelected.MouseEnter += Control_Enter;
		toolStripButtonDeleteSelected.MouseLeave += Control_Leave;
		// 
		// toolStripButtonDeleteAll
		// 
		toolStripButtonDeleteAll.AccessibleDescription = "Deletes all log entries from the list";
		toolStripButtonDeleteAll.AccessibleName = "Delete all";
		toolStripButtonDeleteAll.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonDeleteAll.AutoToolTip = true;
		toolStripButtonDeleteAll.Enabled = false;
		toolStripButtonDeleteAll.Image = FatcowIcons16px.fatcow_bin_closed_16px;
		toolStripButtonDeleteAll.ImageTransparentColor = Color.Magenta;
		toolStripButtonDeleteAll.Name = "toolStripButtonDeleteAll";
		toolStripButtonDeleteAll.Size = new Size(width: 70, height: 22);
		toolStripButtonDeleteAll.Text = "Delete &all";
		toolStripButtonDeleteAll.ToolTipText = "Deletes all log entries from the list and from the log store";
		toolStripButtonDeleteAll.Click += ToolStripButtonDeleteAll_Click;
		toolStripButtonDeleteAll.MouseEnter += Control_Enter;
		toolStripButtonDeleteAll.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator1
		// 
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(width: 6, height: 25);
		// 
		// toolStripLabelProgress
		// 
		toolStripLabelProgress.AccessibleDescription = "Shows the label for the progress bar";
		toolStripLabelProgress.AccessibleName = "Progress label";
		toolStripLabelProgress.Name = "toolStripLabelProgress";
		toolStripLabelProgress.Size = new Size(width: 55, height: 22);
		toolStripLabelProgress.Text = "&Loading:";
		// 
		// kryptonProgressBar
		// 
		kryptonProgressBar.AccessibleDescription = "Shows the progress of loading log entries";
		kryptonProgressBar.AccessibleName = "Progress bar";
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(width: 200, height: 22);
		kryptonProgressBar.StateCommon.Back.Color1 = Color.SteelBlue;
		kryptonProgressBar.StateDisabled.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.StateNormal.Back.ColorStyle = PaletteColorStyle.OneNote;
		kryptonProgressBar.Values.Text = "";
		// 
		// LogViewerForm
		// 
		AccessibleDescription = "Displays stored NLog log events";
		AccessibleName = "Log Viewer";
		AccessibleRole = AccessibleRole.Window;
		AutoScaleDimensions = new SizeF(dx: 7F, dy: 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(width: 900, height: 497);
		Controls.Add(value: toolStripContainer);
		MinimumSize = new Size(width: 600, height: 300);
		Name = "LogViewerForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Log Viewer";
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(performLayout: false);
		kryptonStatusStrip.ResumeLayout(performLayout: false);
		kryptonStatusStrip.PerformLayout();
		toolStripContainer.BottomToolStripPanel.ResumeLayout(performLayout: false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(performLayout: false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(performLayout: false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(performLayout: false);
		toolStripContainer.PerformLayout();
		kryptonToolStripMain.ResumeLayout(performLayout: false);
		kryptonToolStripMain.PerformLayout();
		ResumeLayout(performLayout: false);
	}

	#endregion

	/// <summary>Required KryptonManager component.</summary>
	private KryptonManager kryptonManager;

	/// <summary>Main panel that hosts the list view.</summary>
	private KryptonPanel kryptonPanelMain;

	/// <summary>ListView that displays log events in virtual mode.</summary>
	private ListView listView;

	/// <summary>Column header for the date and time of the log event.</summary>
	private ColumnHeader columnHeaderDateTime;

	/// <summary>Column header for the log level (Info, Warning, Error, etc.).</summary>
	private ColumnHeader columnHeaderLevel;

	/// <summary>Column header for the exception type, if any.</summary>
	private ColumnHeader columnHeaderExceptionType;

	/// <summary>Column header for the log message text.</summary>
	private ColumnHeader columnHeaderMessage;

	/// <summary>Status strip shown at the bottom of the form.</summary>
	private KryptonStatusStrip kryptonStatusStrip;

	/// <summary>Status label that displays information about the log viewer state.</summary>
	private ToolStripStatusLabel labelInformation;

	/// <summary>Container that hosts the toolbar at the top and the status strip at the bottom.</summary>
	private ToolStripContainer toolStripContainer;

	/// <summary>Main toolbar strip.</summary>
	private KryptonToolStrip kryptonToolStripMain;

	/// <summary>Toolbar button to delete selected log entries.</summary>
	private ToolStripButton toolStripButtonDeleteSelected;

	/// <summary>Toolbar button to delete all log entries.</summary>
	private ToolStripButton toolStripButtonDeleteAll;

	/// <summary>Separator between delete buttons and progress section.</summary>
	private ToolStripSeparator toolStripSeparator1;

	/// <summary>Label for the progress bar.</summary>
	private ToolStripLabel toolStripLabelProgress;

	/// <summary>Progress bar that shows the loading progress.</summary>
	private KryptonProgressBarToolStripItem kryptonProgressBar;
}
