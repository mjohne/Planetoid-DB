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
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(PreloadForm));
		kryptonCommandLinkButtonExit = new KryptonCommandLinkButton();
		kryptonCommandLinkButtonLoadInternalDemoData = new KryptonCommandLinkButton();
		kryptonCommandLinkButtonDownloadMprcorbDat = new KryptonCommandLinkButton();
		kryptonCommandLinkButtonOpenLocalFile = new KryptonCommandLinkButton();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanelMain = new KryptonPanel();
		openFileDialog = new OpenFileDialog();
		kryptonManager = new KryptonManager(components);
		kryptonCommandLinkButtonAppInfo = new KryptonCommandLinkButton();
		kryptonStatusStrip.SuspendLayout();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonCommandLinkButtonExit
		// 
		kryptonCommandLinkButtonExit.AccessibleDescription = "Cancels and quits the application";
		kryptonCommandLinkButtonExit.AccessibleName = "Quit the application";
		kryptonCommandLinkButtonExit.AccessibleRole = AccessibleRole.PushButton;
		kryptonCommandLinkButtonExit.CommandLinkTextValues.Description = "Cancel and quit the application";
		kryptonCommandLinkButtonExit.CommandLinkTextValues.Heading = "Quit the application";
		kryptonCommandLinkButtonExit.DialogResult = DialogResult.Cancel;
		kryptonCommandLinkButtonExit.Location = new Point(0, 276);
		kryptonCommandLinkButtonExit.Name = "kryptonCommandLinkButtonExit";
		kryptonCommandLinkButtonExit.OverrideFocus.Border.Draw = InheritBool.True;
		kryptonCommandLinkButtonExit.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		kryptonCommandLinkButtonExit.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		kryptonCommandLinkButtonExit.Size = new Size(356, 60);
		kryptonCommandLinkButtonExit.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonExit.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		kryptonCommandLinkButtonExit.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonExit.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		kryptonCommandLinkButtonExit.TabIndex = 4;
		kryptonCommandLinkButtonExit.ToolTipValues.Description = "Cancels and quits the application.";
		kryptonCommandLinkButtonExit.ToolTipValues.EnableToolTips = true;
		kryptonCommandLinkButtonExit.ToolTipValues.Heading = "Quit the application";
		kryptonCommandLinkButtonExit.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		kryptonCommandLinkButtonExit.Enter += Control_Enter;
		kryptonCommandLinkButtonExit.Leave += Control_Leave;
		kryptonCommandLinkButtonExit.MouseEnter += Control_Enter;
		kryptonCommandLinkButtonExit.MouseLeave += Control_Leave;
		// 
		// kryptonCommandLinkButtonLoadInternalDemoData
		// 
		kryptonCommandLinkButtonLoadInternalDemoData.AccessibleDescription = "Loads internal demo data";
		kryptonCommandLinkButtonLoadInternalDemoData.AccessibleName = "Load internal demo data";
		kryptonCommandLinkButtonLoadInternalDemoData.AccessibleRole = AccessibleRole.PushButton;
		kryptonCommandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Description = "The internal demo data contains 10'000 minor planets";
		kryptonCommandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Heading = "Load internal demo data";
		kryptonCommandLinkButtonLoadInternalDemoData.Location = new Point(0, 144);
		kryptonCommandLinkButtonLoadInternalDemoData.Name = "kryptonCommandLinkButtonLoadInternalDemoData";
		kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.Draw = InheritBool.True;
		kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		kryptonCommandLinkButtonLoadInternalDemoData.Size = new Size(356, 60);
		kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		kryptonCommandLinkButtonLoadInternalDemoData.TabIndex = 2;
		kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.Description = "Loads the internal demo data.";
		kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.EnableToolTips = true;
		kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.Heading = "Load internal demo data";
		kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		kryptonCommandLinkButtonLoadInternalDemoData.Click += KryptonCommandLinkButtonLoadInternalDemoData_Click;
		kryptonCommandLinkButtonLoadInternalDemoData.Enter += Control_Enter;
		kryptonCommandLinkButtonLoadInternalDemoData.Leave += Control_Leave;
		kryptonCommandLinkButtonLoadInternalDemoData.MouseEnter += Control_Enter;
		kryptonCommandLinkButtonLoadInternalDemoData.MouseLeave += Control_Leave;
		// 
		// kryptonCommandLinkButtonDownloadMprcorbDat
		// 
		kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleDescription = "Downloads MPCORB.DAT from the IAU Minor Planet Center";
		kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleName = "Download MPCORB.DAT";
		kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleRole = AccessibleRole.PushButton;
		kryptonCommandLinkButtonDownloadMprcorbDat.CommandLinkTextValues.Description = "Download MPCORB.DAT from the IAU Minor Planet Center";
		kryptonCommandLinkButtonDownloadMprcorbDat.CommandLinkTextValues.Heading = "Download MPCORB.DAT";
		kryptonCommandLinkButtonDownloadMprcorbDat.Location = new Point(0, 12);
		kryptonCommandLinkButtonDownloadMprcorbDat.Name = "kryptonCommandLinkButtonDownloadMprcorbDat";
		kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.Draw = InheritBool.True;
		kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		kryptonCommandLinkButtonDownloadMprcorbDat.Size = new Size(356, 60);
		kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		kryptonCommandLinkButtonDownloadMprcorbDat.TabIndex = 0;
		kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.Description = "Downloads MPCORB.DAT from the IAU Minor Planet Center.";
		kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.EnableToolTips = true;
		kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.Heading = "Download MPCORB.DAT";
		kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		kryptonCommandLinkButtonDownloadMprcorbDat.Click += KryptonCommandLinkButtonDownloadMpcorbDat_Click;
		kryptonCommandLinkButtonDownloadMprcorbDat.Enter += Control_Enter;
		kryptonCommandLinkButtonDownloadMprcorbDat.Leave += Control_Leave;
		kryptonCommandLinkButtonDownloadMprcorbDat.MouseEnter += Control_Enter;
		kryptonCommandLinkButtonDownloadMprcorbDat.MouseLeave += Control_Leave;
		// 
		// kryptonCommandLinkButtonOpenLocalFile
		// 
		kryptonCommandLinkButtonOpenLocalFile.AccessibleDescription = "Opens a local MPCORB.DAT file from hard drive";
		kryptonCommandLinkButtonOpenLocalFile.AccessibleName = "Open a local MPCORB.DAT file";
		kryptonCommandLinkButtonOpenLocalFile.AccessibleRole = AccessibleRole.PushButton;
		kryptonCommandLinkButtonOpenLocalFile.CommandLinkTextValues.Description = "Open a local MPCORB.DAT file from hard drive";
		kryptonCommandLinkButtonOpenLocalFile.CommandLinkTextValues.Heading = "Open a local MPCORB.DAT file";
		kryptonCommandLinkButtonOpenLocalFile.Location = new Point(0, 78);
		kryptonCommandLinkButtonOpenLocalFile.Name = "kryptonCommandLinkButtonOpenLocalFile";
		kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.Draw = InheritBool.True;
		kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		kryptonCommandLinkButtonOpenLocalFile.Size = new Size(356, 60);
		kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		kryptonCommandLinkButtonOpenLocalFile.TabIndex = 1;
		kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.Description = "Opens a local MPCORB.DAT file from hard drive.";
		kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.EnableToolTips = true;
		kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.Heading = "Open a local MPCORB.DAT file";
		kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		kryptonCommandLinkButtonOpenLocalFile.Click += KryptonCommandLinkButtonOpenLocalFile_Click;
		kryptonCommandLinkButtonOpenLocalFile.Enter += Control_Enter;
		kryptonCommandLinkButtonOpenLocalFile.Leave += Control_Leave;
		kryptonCommandLinkButtonOpenLocalFile.MouseEnter += Control_Enter;
		kryptonCommandLinkButtonOpenLocalFile.MouseLeave += Control_Leave;
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
		kryptonStatusStrip.Location = new Point(0, 340);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(356, 22);
		kryptonStatusStrip.TabIndex = 1;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
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
		// kryptonPanelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Groups the data";
		kryptonPanelMain.AccessibleName = "Panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(kryptonCommandLinkButtonAppInfo);
		kryptonPanelMain.Controls.Add(kryptonCommandLinkButtonExit);
		kryptonPanelMain.Controls.Add(kryptonCommandLinkButtonLoadInternalDemoData);
		kryptonPanelMain.Controls.Add(kryptonCommandLinkButtonDownloadMprcorbDat);
		kryptonPanelMain.Controls.Add(kryptonCommandLinkButtonOpenLocalFile);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(356, 340);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
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
		// kryptonCommandLinkButtonAppInfo
		// 
		kryptonCommandLinkButtonAppInfo.AccessibleDescription = "Shows more information about the application";
		kryptonCommandLinkButtonAppInfo.AccessibleName = "More information about the application";
		kryptonCommandLinkButtonAppInfo.AccessibleRole = AccessibleRole.PushButton;
		kryptonCommandLinkButtonAppInfo.CommandLinkTextValues.Description = "Show more information about the application";
		kryptonCommandLinkButtonAppInfo.CommandLinkTextValues.Heading = "Application information";
		kryptonCommandLinkButtonAppInfo.Location = new Point(0, 210);
		kryptonCommandLinkButtonAppInfo.Name = "kryptonCommandLinkButtonAppInfo";
		kryptonCommandLinkButtonAppInfo.OverrideFocus.Border.Draw = InheritBool.True;
		kryptonCommandLinkButtonAppInfo.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
		kryptonCommandLinkButtonAppInfo.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
		kryptonCommandLinkButtonAppInfo.Size = new Size(356, 60);
		kryptonCommandLinkButtonAppInfo.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonAppInfo.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
		kryptonCommandLinkButtonAppInfo.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
		kryptonCommandLinkButtonAppInfo.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
		kryptonCommandLinkButtonAppInfo.TabIndex = 3;
		kryptonCommandLinkButtonAppInfo.ToolTipValues.Description = "Shows more information about the application";
		kryptonCommandLinkButtonAppInfo.ToolTipValues.EnableToolTips = true;
		kryptonCommandLinkButtonAppInfo.ToolTipValues.Heading = "Application information";
		kryptonCommandLinkButtonAppInfo.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		kryptonCommandLinkButtonAppInfo.Click += KryptonCommandLinkButtonAppInfo_Click;
		kryptonCommandLinkButtonAppInfo.Enter += Control_Enter;
		kryptonCommandLinkButtonAppInfo.Leave += Control_Leave;
		kryptonCommandLinkButtonAppInfo.MouseEnter += Control_Enter;
		kryptonCommandLinkButtonAppInfo.MouseLeave += Control_Leave;
		// 
		// PreloadForm
		// 
		AccessibleDescription = "Indicates that the MPCORB.DAT file is missing";
		AccessibleName = "Preloader";
		AccessibleRole = AccessibleRole.Window;
		AllowDrop = true;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = kryptonCommandLinkButtonExit;
		ClientSize = new Size(356, 362);
		Controls.Add(kryptonPanelMain);
		Controls.Add(kryptonStatusStrip);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "PreloadForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Planetoid-DB Preloader";
		Load += PreloadForm_Load;
		DragDrop += PreloadForm_DragDrop;
		DragEnter += PreloadForm_DragEnter;
		DragLeave += PreloadForm_DragLeave;
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	/// <summary>Required designer variable for the KryptonStatusStrip component that displays the status strip.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonStatusStrip component that displays the status strip.</remarks>
	private KryptonStatusStrip kryptonStatusStrip;

	/// <summary>Required designer variable for the ToolStripStatusLabel component that displays information in the status strip.</summary>
	/// <remarks>This variable is used by the designer to manage the ToolStripStatusLabel component that displays information in the status strip.</remarks>
	private ToolStripStatusLabel labelInformation;

	/// <summary>Required designer variable for the KryptonPanel component that serves as the main container for the form's controls.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonPanel component that serves as the main container for the form's controls.</remarks>
	private KryptonPanel kryptonPanelMain;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that opens a local file.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that opens a local file.</remarks>
	private KryptonCommandLinkButton kryptonCommandLinkButtonOpenLocalFile;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that exits the application.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that exits the application.</remarks>
	private KryptonCommandLinkButton kryptonCommandLinkButtonExit;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that loads internal demo data.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that loads internal demo data.</remarks>
	private KryptonCommandLinkButton kryptonCommandLinkButtonLoadInternalDemoData;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that downloads the MPCORB.DAT file.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that downloads the MPCORB.DAT file.</remarks>
	private KryptonCommandLinkButton kryptonCommandLinkButtonDownloadMprcorbDat;
	
	/// <summary>Required designer variable for the OpenFileDialog component.</summary>
	/// <remarks>This variable is used by the designer to manage the OpenFileDialog component.</remarks>
	private OpenFileDialog openFileDialog;

	/// <summary>Required designer variable for the KryptonManager component.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonManager component.</remarks>
	private KryptonManager kryptonManager;

	/// <summary>Required designer variable for the KryptonCommandLinkButton component that shows application information.</summary>
	/// <remarks>This variable is used by the designer to manage the KryptonCommandLinkButton component that shows application information.</remarks>
	private KryptonCommandLinkButton kryptonCommandLinkButtonAppInfo;
}