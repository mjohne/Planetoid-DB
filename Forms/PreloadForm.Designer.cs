/*
 * File:        PreloadForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that enables users to preload the MPCORB.DAT file by providing options to load internal demo data, download the file, or open a local file.
 * Remarks:     This file contains the Windows Forms designer-generated code for the PreloadForm. Do not modify this file manually.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a form that enables users to preload the MPCORB.DAT file by providing options to load internal demo data, download the file, or open a local file.</summary>
/// <remarks>The PreloadForm facilitates the initial setup required for working with minor planet data in Planetoid-DB. It includes a status bar for user feedback and organizes available actions in a user-friendly interface. Use this form to ensure the necessary data is available before proceeding with further operations.</remarks>
partial class PreloadForm
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
		base.Dispose(disposing: disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(PreloadForm));
		commandLinkButtonExit = new KryptonCommandLinkButton();
		commandLinkButtonLoadInternalDemoData = new KryptonCommandLinkButton();
		commandLinkButtonDownloadMpcorbDat = new KryptonCommandLinkButton();
		commandLinkButtonOpenLocalFile = new KryptonCommandLinkButton();
		commandLinkButtonAppInfo = new KryptonCommandLinkButton();
		statusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		panelMain = new KryptonPanel();
		openFileDialog = new OpenFileDialog();
		kryptonManager = new KryptonManager(components);
		contextMenuShowControlsInStatusBar = new ContextMenuStrip(components);
		menuItemShowInformationLabel = new ToolStripMenuItem();
		statusStrip.SuspendLayout();
		((ISupportInitialize)panelMain).BeginInit();
		panelMain.SuspendLayout();
		contextMenuShowControlsInStatusBar.SuspendLayout();
		SuspendLayout();
		// 
		// commandLinkButtonExit
		// 
		commandLinkButtonExit.AccessibleDescription = "Cancels and quits the application";
		commandLinkButtonExit.AccessibleName = "Quit the application";
		commandLinkButtonExit.AccessibleRole = AccessibleRole.PushButton;
		commandLinkButtonExit.CommandLinkTextValues.Description = "Cancel and quit the application";
		commandLinkButtonExit.CommandLinkTextValues.Heading = "Quit the application";
		commandLinkButtonExit.DialogResult = DialogResult.Cancel;
		commandLinkButtonExit.Location = new Point(0, 276);
		commandLinkButtonExit.Name = "commandLinkButtonExit";
		commandLinkButtonExit.OverrideFocus.Border.Draw = InheritBool.True;
		commandLinkButtonExit.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		commandLinkButtonExit.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		commandLinkButtonExit.Size = new Size(356, 60);
		commandLinkButtonExit.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonExit.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		commandLinkButtonExit.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonExit.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		commandLinkButtonExit.TabIndex = 4;
		commandLinkButtonExit.ToolTipValues.Description = "Cancels and quits the application.";
		commandLinkButtonExit.ToolTipValues.EnableToolTips = true;
		commandLinkButtonExit.ToolTipValues.Heading = "Quit the application";
		commandLinkButtonExit.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		commandLinkButtonExit.Enter += Control_Enter;
		commandLinkButtonExit.Leave += Control_Leave;
		commandLinkButtonExit.MouseEnter += Control_Enter;
		commandLinkButtonExit.MouseLeave += Control_Leave;
		// 
		// commandLinkButtonLoadInternalDemoData
		// 
		commandLinkButtonLoadInternalDemoData.AccessibleDescription = "Loads internal demo data";
		commandLinkButtonLoadInternalDemoData.AccessibleName = "Load internal demo data";
		commandLinkButtonLoadInternalDemoData.AccessibleRole = AccessibleRole.PushButton;
		commandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Description = "The internal demo data contains 10'000 minor planets";
		commandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Heading = "Load internal demo data";
		commandLinkButtonLoadInternalDemoData.Location = new Point(0, 144);
		commandLinkButtonLoadInternalDemoData.Name = "commandLinkButtonLoadInternalDemoData";
		commandLinkButtonLoadInternalDemoData.OverrideFocus.Border.Draw = InheritBool.True;
		commandLinkButtonLoadInternalDemoData.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		commandLinkButtonLoadInternalDemoData.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		commandLinkButtonLoadInternalDemoData.Size = new Size(356, 60);
		commandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		commandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		commandLinkButtonLoadInternalDemoData.TabIndex = 2;
		commandLinkButtonLoadInternalDemoData.ToolTipValues.Description = "Loads the internal demo data.";
		commandLinkButtonLoadInternalDemoData.ToolTipValues.EnableToolTips = true;
		commandLinkButtonLoadInternalDemoData.ToolTipValues.Heading = "Load internal demo data";
		commandLinkButtonLoadInternalDemoData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		commandLinkButtonLoadInternalDemoData.Click += CommandLinkButtonLoadInternalDemoData_Click;
		commandLinkButtonLoadInternalDemoData.Enter += Control_Enter;
		commandLinkButtonLoadInternalDemoData.Leave += Control_Leave;
		commandLinkButtonLoadInternalDemoData.MouseEnter += Control_Enter;
		commandLinkButtonLoadInternalDemoData.MouseLeave += Control_Leave;
		// 
		// commandLinkButtonDownloadMpcorbDat
		// 
		commandLinkButtonDownloadMpcorbDat.AccessibleDescription = "Downloads MPCORB.DAT from the IAU Minor Planet Center";
		commandLinkButtonDownloadMpcorbDat.AccessibleName = "Download MPCORB.DAT";
		commandLinkButtonDownloadMpcorbDat.AccessibleRole = AccessibleRole.PushButton;
		commandLinkButtonDownloadMpcorbDat.CommandLinkTextValues.Description = "Download MPCORB.DAT from the IAU Minor Planet Center";
		commandLinkButtonDownloadMpcorbDat.CommandLinkTextValues.Heading = "Download MPCORB.DAT";
		commandLinkButtonDownloadMpcorbDat.Location = new Point(0, 12);
		commandLinkButtonDownloadMpcorbDat.Name = "commandLinkButtonDownloadMpcorbDat";
		commandLinkButtonDownloadMpcorbDat.OverrideFocus.Border.Draw = InheritBool.True;
		commandLinkButtonDownloadMpcorbDat.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		commandLinkButtonDownloadMpcorbDat.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		commandLinkButtonDownloadMpcorbDat.Size = new Size(356, 60);
		commandLinkButtonDownloadMpcorbDat.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonDownloadMpcorbDat.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		commandLinkButtonDownloadMpcorbDat.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonDownloadMpcorbDat.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		commandLinkButtonDownloadMpcorbDat.TabIndex = 0;
		commandLinkButtonDownloadMpcorbDat.ToolTipValues.Description = "Downloads MPCORB.DAT from the IAU Minor Planet Center.";
		commandLinkButtonDownloadMpcorbDat.ToolTipValues.EnableToolTips = true;
		commandLinkButtonDownloadMpcorbDat.ToolTipValues.Heading = "Download MPCORB.DAT";
		commandLinkButtonDownloadMpcorbDat.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		commandLinkButtonDownloadMpcorbDat.Click += CommandLinkButtonDownloadMpcorbDat_Click;
		commandLinkButtonDownloadMpcorbDat.Enter += Control_Enter;
		commandLinkButtonDownloadMpcorbDat.Leave += Control_Leave;
		commandLinkButtonDownloadMpcorbDat.MouseEnter += Control_Enter;
		commandLinkButtonDownloadMpcorbDat.MouseLeave += Control_Leave;
		// 
		// commandLinkButtonOpenLocalFile
		// 
		commandLinkButtonOpenLocalFile.AccessibleDescription = "Opens a local MPCORB.DAT file from hard drive";
		commandLinkButtonOpenLocalFile.AccessibleName = "Open a local MPCORB.DAT file";
		commandLinkButtonOpenLocalFile.AccessibleRole = AccessibleRole.PushButton;
		commandLinkButtonOpenLocalFile.CommandLinkTextValues.Description = "Open a local MPCORB.DAT file from hard drive";
		commandLinkButtonOpenLocalFile.CommandLinkTextValues.Heading = "Open a local MPCORB.DAT file";
		commandLinkButtonOpenLocalFile.Location = new Point(0, 78);
		commandLinkButtonOpenLocalFile.Name = "commandLinkButtonOpenLocalFile";
		commandLinkButtonOpenLocalFile.OverrideFocus.Border.Draw = InheritBool.True;
		commandLinkButtonOpenLocalFile.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		commandLinkButtonOpenLocalFile.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		commandLinkButtonOpenLocalFile.Size = new Size(356, 60);
		commandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		commandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		commandLinkButtonOpenLocalFile.TabIndex = 1;
		commandLinkButtonOpenLocalFile.ToolTipValues.Description = "Opens a local MPCORB.DAT file from hard drive.";
		commandLinkButtonOpenLocalFile.ToolTipValues.EnableToolTips = true;
		commandLinkButtonOpenLocalFile.ToolTipValues.Heading = "Open a local MPCORB.DAT file";
		commandLinkButtonOpenLocalFile.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		commandLinkButtonOpenLocalFile.Click += CommandLinkButtonOpenLocalFile_Click;
		commandLinkButtonOpenLocalFile.Enter += Control_Enter;
		commandLinkButtonOpenLocalFile.Leave += Control_Leave;
		commandLinkButtonOpenLocalFile.MouseEnter += Control_Enter;
		commandLinkButtonOpenLocalFile.MouseLeave += Control_Leave;
		// 
		// statusStrip
		// 
		statusStrip.AccessibleDescription = "Shows some information";
		statusStrip.AccessibleName = "Status bar with some information";
		statusStrip.AccessibleRole = AccessibleRole.StatusBar;
		statusStrip.AllowClickThrough = true;
		statusStrip.AllowItemReorder = true;
		statusStrip.ContextMenuStrip = contextMenuShowControlsInStatusBar;
		statusStrip.Font = new Font("Segoe UI", 9F);
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		statusStrip.Location = new Point(0, 340);
		statusStrip.Name = "statusStrip";
		statusStrip.ProgressBars = null;
		statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		statusStrip.ShowItemToolTips = true;
		statusStrip.Size = new Size(356, 22);
		statusStrip.TabIndex = 1;
		statusStrip.TabStop = true;
		statusStrip.Text = "Status bar";
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows some information";
		labelInformation.AccessibleName = "Some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		// 
		// panelMain
		// 
		panelMain.AccessibleDescription = "Groups the data";
		panelMain.AccessibleName = "Panel";
		panelMain.AccessibleRole = AccessibleRole.Pane;
		panelMain.AutoScroll = true;
		panelMain.Controls.Add(commandLinkButtonAppInfo);
		panelMain.Controls.Add(commandLinkButtonExit);
		panelMain.Controls.Add(commandLinkButtonLoadInternalDemoData);
		panelMain.Controls.Add(commandLinkButtonDownloadMpcorbDat);
		panelMain.Controls.Add(commandLinkButtonOpenLocalFile);
		panelMain.Dock = DockStyle.Fill;
		panelMain.Location = new Point(0, 0);
		panelMain.Name = "panelMain";
		panelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		panelMain.Size = new Size(356, 340);
		panelMain.TabIndex = 0;
		panelMain.TabStop = true;
		// 
		// commandLinkButtonAppInfo
		// 
		commandLinkButtonAppInfo.AccessibleDescription = "Shows more information about the application";
		commandLinkButtonAppInfo.AccessibleName = "More information about the application";
		commandLinkButtonAppInfo.AccessibleRole = AccessibleRole.PushButton;
		commandLinkButtonAppInfo.CommandLinkTextValues.Description = "Show more information about the application";
		commandLinkButtonAppInfo.CommandLinkTextValues.Heading = "Application information";
		commandLinkButtonAppInfo.Location = new Point(0, 210);
		commandLinkButtonAppInfo.Name = "commandLinkButtonAppInfo";
		commandLinkButtonAppInfo.OverrideFocus.Border.Draw = InheritBool.True;
		commandLinkButtonAppInfo.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		commandLinkButtonAppInfo.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		commandLinkButtonAppInfo.Size = new Size(356, 60);
		commandLinkButtonAppInfo.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonAppInfo.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		commandLinkButtonAppInfo.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		commandLinkButtonAppInfo.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		commandLinkButtonAppInfo.TabIndex = 3;
		commandLinkButtonAppInfo.ToolTipValues.Description = "Shows more information about the application";
		commandLinkButtonAppInfo.ToolTipValues.EnableToolTips = true;
		commandLinkButtonAppInfo.ToolTipValues.Heading = "Application information";
		commandLinkButtonAppInfo.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		commandLinkButtonAppInfo.Click += CommandLinkButtonAppInfo_Click;
		commandLinkButtonAppInfo.Enter += Control_Enter;
		commandLinkButtonAppInfo.Leave += Control_Leave;
		commandLinkButtonAppInfo.MouseEnter += Control_Enter;
		commandLinkButtonAppInfo.MouseLeave += Control_Leave;
		// 
		// openFileDialog
		// 
		openFileDialog.DefaultExt = "dat";
		openFileDialog.FileName = "mpcorb.dat";
		openFileDialog.Filter = "DAT files|*.dat|all files|*.*";
		openFileDialog.Title = "Open MPCORB.DAT";
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
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
		contextMenuShowControlsInStatusBar.Size = new Size(198, 48);
		contextMenuShowControlsInStatusBar.TabStop = true;
		contextMenuShowControlsInStatusBar.Text = "Show controls in status bar";
		contextMenuShowControlsInStatusBar.MouseEnter += Control_Enter;
		contextMenuShowControlsInStatusBar.MouseLeave += Control_Leave;
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
		menuItemShowInformationLabel.Click += MenuItemShowInformationLabel_Click;
		menuItemShowInformationLabel.MouseEnter += Control_Enter;
		menuItemShowInformationLabel.MouseLeave += Control_Leave;
		// 
		// PreloadForm
		// 
		AccessibleDescription = "Indicates that the MPCORB.DAT file is missing";
		AccessibleName = "Preloader";
		AccessibleRole = AccessibleRole.Window;
		AllowDrop = true;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = commandLinkButtonExit;
		ClientSize = new Size(356, 362);
		Controls.Add(panelMain);
		Controls.Add(statusStrip);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "PreloadForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Planetoid-DB Preloader";
		FormClosing += PreloadForm_FormClosing;
		Load += PreloadForm_Load;
		DragDrop += PreloadForm_DragDrop;
		DragEnter += PreloadForm_DragEnter;
		DragLeave += PreloadForm_DragLeave;
		statusStrip.ResumeLayout(false);
		statusStrip.PerformLayout();
		((ISupportInitialize)panelMain).EndInit();
		panelMain.ResumeLayout(false);
		contextMenuShowControlsInStatusBar.ResumeLayout(false);
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	/// <summary>Required designer variable for the KryptonStatusStrip component that displays the status strip.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonStatusStrip component that displays the status strip.</remarks>
	private KryptonStatusStrip statusStrip;

	/// <summary>Required designer variable for the ToolStripStatusLabel component that displays information in the status strip.</summary>
	/// <remarks>This variable is used by the designer to manage the ToolStripStatusLabel component that displays information in the status strip.</remarks>
	private ToolStripStatusLabel labelInformation;

	/// <summary>Required designer variable for the KryptonPanel component that serves as the main toolStripContainer for the form's controls.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonPanel component that serves as the main toolStripContainer for the form's controls.</remarks>
	private KryptonPanel panelMain;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that opens a local file.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that opens a local file.</remarks>
	private KryptonCommandLinkButton commandLinkButtonOpenLocalFile;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that exits the application.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that exits the application.</remarks>
	private KryptonCommandLinkButton commandLinkButtonExit;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that loads internal demo data.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that loads internal demo data.</remarks>
	private KryptonCommandLinkButton commandLinkButtonLoadInternalDemoData;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that downloads the MPCORB.DAT file.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that downloads the MPCORB.DAT file.</remarks>
	private KryptonCommandLinkButton commandLinkButtonDownloadMpcorbDat;
	
	/// <summary>Required designer variable for the OpenFileDialog component.</summary>
	/// <remarks>This variable is used by the designer to manage the OpenFileDialog component.</remarks>
	private OpenFileDialog openFileDialog;

	/// <summary>Required designer variable for the KryptonManager component.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonManager component.</remarks>
	private KryptonManager kryptonManager;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that shows application information.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that shows application information.</remarks>
	private KryptonCommandLinkButton commandLinkButtonAppInfo;
	
	/// <summary>Gets or sets the context menu used to show controls in the status bar.</summary>
	private ContextMenuStrip contextMenuShowControlsInStatusBar;

	/// <summary>Menu item that controls display of the information label.</summary>
	private ToolStripMenuItem menuItemShowInformationLabel;
}
