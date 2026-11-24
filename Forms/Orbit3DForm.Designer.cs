/*
 * File:        Orbit3DForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides the designer-generated user interface for the 3D orbital visualization form.
 * Remarks:     This file contains the Windows Forms designer-generated code for the Orbit3DForm. Do not modify this file manually.
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

/// <summary>Provides the designer-generated user interface for the 3D orbital visualization form.</summary>
/// <remarks>The form contains a <see cref="System.Windows.Forms.Panel"/> that hosts the OpenGL rendering surface and a <see cref="KryptonStatusStrip"/> with an informational status label below.</remarks>
partial class Orbit3DForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Releases all resources used by the <see cref="Orbit3DForm"/>.</summary>
	/// <param name="disposing">True if managed resources should be disposed; otherwise false.</param>
	/// <remarks>This method is called by the runtime to release resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing: disposing);
	}

	/// <summary>Initializes the components of the <see cref="Orbit3DForm"/>.</summary>
	/// <remarks>This method sets up the controls and their properties for the 3D orbit visualization form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(Orbit3DForm));
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanelMain = new KryptonPanel();
		panelGl = new Panel();
		kryptonManager = new KryptonManager(components);
		contextMenuShowControlsInStatusBar = new ContextMenuStrip(components);
		toolStripMenuItemShowInformationLabel = new ToolStripMenuItem();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		contextMenuShowControlsInStatusBar.SuspendLayout();
		SuspendLayout();
		// 
		// toolStripContainer
		// 
		toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
		toolStripContainer.AccessibleName = "Toolbar toolStripContainer";
		toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.AccessibleDescription = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleName = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.AccessibleDescription = "Content panel";
		toolStripContainer.ContentPanel.AccessibleName = "Content panel";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(900, 575);
		toolStripContainer.Dock = DockStyle.Fill;
		// 
		// toolStripContainer.LeftToolStripPanel
		// 
		toolStripContainer.LeftToolStripPanel.AccessibleDescription = "Left panel";
		toolStripContainer.LeftToolStripPanel.AccessibleName = "Left panel";
		toolStripContainer.LeftToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		// 
		// toolStripContainer.RightToolStripPanel
		// 
		toolStripContainer.RightToolStripPanel.AccessibleDescription = "Right panel";
		toolStripContainer.RightToolStripPanel.AccessibleName = "Right panel";
		toolStripContainer.RightToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Size = new Size(900, 622);
		toolStripContainer.TabIndex = 0;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.AccessibleDescription = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleName = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.Enter += Control_Enter;
		toolStripContainer.Leave += Control_Leave;
		toolStripContainer.MouseEnter += Control_Enter;
		toolStripContainer.MouseLeave += Control_Leave;
		// 
		// statusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows orbital element information for the selected planetoid";
		kryptonStatusStrip.AccessibleName = "Status bar with orbital information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.ContextMenuStrip = contextMenuShowControlsInStatusBar;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 0);
		kryptonStatusStrip.Name = "statusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(900, 22);
		kryptonStatusStrip.TabIndex = 0;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		kryptonStatusStrip.Enter += Control_Enter;
		kryptonStatusStrip.Leave += Control_Leave;
		kryptonStatusStrip.MouseEnter += Control_Enter;
		kryptonStatusStrip.MouseLeave += Control_Leave;
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows orbital element information for the selected planetoid";
		labelInformation.AccessibleName = "Orbital element information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Shows orbital element information for the selected planetoid";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;
		// 
		// panelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Contains the 3D orbit visualization";
		kryptonPanelMain.AccessibleName = "Main panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.AutoScroll = true;
		kryptonPanelMain.Controls.Add(panelGl);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "panelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(900, 575);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "Main panel";
		kryptonPanelMain.Enter += Control_Enter;
		kryptonPanelMain.Leave += Control_Leave;
		kryptonPanelMain.MouseEnter += Control_Enter;
		kryptonPanelMain.MouseLeave += Control_Leave;
		// 
		// panelGl
		// 
		panelGl.AccessibleDescription = "OpenGL rendering surface for the 3D orbit visualization";
		panelGl.AccessibleName = "3D orbit rendering surface";
		panelGl.AccessibleRole = AccessibleRole.Client;
		panelGl.AutoScroll = true;
		panelGl.BackColor = Color.Black;
		panelGl.Dock = DockStyle.Fill;
		panelGl.Location = new Point(0, 0);
		panelGl.Name = "panelGl";
		panelGl.Size = new Size(900, 575);
		panelGl.TabIndex = 0;
		panelGl.Enter += Control_Enter;
		panelGl.Leave += Control_Leave;
		panelGl.MouseEnter += Control_Enter;
		panelGl.MouseLeave += Control_Leave;
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
		contextMenuShowControlsInStatusBar.Items.AddRange(new ToolStripItem[] { toolStripMenuItemShowInformationLabel });
		contextMenuShowControlsInStatusBar.Name = "contextMenuStrip";
		contextMenuShowControlsInStatusBar.Size = new Size(198, 48);
		contextMenuShowControlsInStatusBar.TabStop = true;
		contextMenuShowControlsInStatusBar.Text = "Show controls in status bar";
		contextMenuShowControlsInStatusBar.MouseEnter += Control_Enter;
		contextMenuShowControlsInStatusBar.MouseLeave += Control_Leave;
		// 
		// menuItemShowInformationLabel
		// 
		toolStripMenuItemShowInformationLabel.AccessibleDescription = "Determines whether the information label is visible or hidden";
		toolStripMenuItemShowInformationLabel.AccessibleName = "Information label";
		toolStripMenuItemShowInformationLabel.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemShowInformationLabel.AutoToolTip = true;
		toolStripMenuItemShowInformationLabel.Checked = true;
		toolStripMenuItemShowInformationLabel.CheckOnClick = true;
		toolStripMenuItemShowInformationLabel.CheckState = CheckState.Checked;
		toolStripMenuItemShowInformationLabel.Name = "menuItemShowInformationLabel";
		toolStripMenuItemShowInformationLabel.ShortcutKeyDisplayString = "";
		toolStripMenuItemShowInformationLabel.Size = new Size(197, 22);
		toolStripMenuItemShowInformationLabel.Text = "Show &information label";
		toolStripMenuItemShowInformationLabel.Click += ToolStripMenuItemShowInformationLabel_Click;
		toolStripMenuItemShowInformationLabel.MouseEnter += Control_Enter;
		toolStripMenuItemShowInformationLabel.MouseLeave += Control_Leave;
		// 
		// Orbit3DForm
		// 
		AccessibleDescription = "Displays a 3D orbit visualization of the selected minor planet relative to the eight solar system planets";
		AccessibleName = "Orbit 3D visualization form";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(900, 622);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "Orbit3DForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Orbit 3D visualization";
		FormClosing += Orbit3DForm_FormClosing;
		Load += Orbit3DForm_Load;
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		contextMenuShowControlsInStatusBar.ResumeLayout(false);
		ResumeLayout(false);
	}

	private ToolStripContainer toolStripContainer;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonPanel kryptonPanelMain;
	private Panel panelGl;
	private KryptonManager kryptonManager;
	private ContextMenuStrip contextMenuShowControlsInStatusBar;
	private ToolStripMenuItem toolStripMenuItemShowInformationLabel;
}
