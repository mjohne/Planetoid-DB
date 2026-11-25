/*
 * File:        WorldMapForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Form
 * Description: Designer file of the form that shows an interactive world map for selecting geographic coordinates.
 * Remarks:     This file contains the Windows Forms designer-generated code for the WorldMapForm.
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

using Microsoft.Web.WebView2.WinForms;

using System.ComponentModel;

namespace Planetoid_DB.Form;

/// <summary>Designer part of the form that shows an interactive world map for selecting geographic coordinates.</summary>
/// <remarks>This form hosts a toolbar, a WebView2 control and a status bar inside a ToolStripContainer.</remarks>
partial class WorldMapForm
{
	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(WorldMapForm));
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		contextMenuShowControlsInStatusBar = new ContextMenuStrip(components);
		menuItemShowInformationLabel = new ToolStripMenuItem();
		labelInformation = new ToolStripStatusLabel();
		webView = new WebView2();
		kryptonToolStrip = new KryptonToolStrip();
		buttonCommitCoordinates = new ToolStripButton();
		buttonCancel = new ToolStripButton();
		separator1 = new ToolStripSeparator();
		textBoxSearch = new ToolStripTextBox();
		buttonSearch = new ToolStripButton();
		separator2 = new ToolStripSeparator();
		labelCoordinates = new ToolStripLabel();
		kryptonManager = new KryptonManager(components);
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		contextMenuShowControlsInStatusBar.SuspendLayout();
		((ISupportInitialize)webView).BeginInit();
		kryptonToolStrip.SuspendLayout();
		SuspendLayout();
		// 
		// toolStripContainer
		// 
		toolStripContainer.AccessibleDescription = "Groups the panels";
		toolStripContainer.AccessibleName = "Panel";
		toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.BottomToolStripPanel.AccessibleName = "Bottom panel";
		toolStripContainer.BottomToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.ContentPanel.AccessibleName = "Content panel";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.ContentPanel.Controls.Add(webView);
		toolStripContainer.ContentPanel.Size = new Size(1000, 625);
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
		toolStripContainer.Size = new Size(1000, 672);
		toolStripContainer.TabIndex = 0;
		toolStripContainer.Text = "toolStripContainer";
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.AccessibleDescription = "Just a panel";
		toolStripContainer.TopToolStripPanel.AccessibleName = "Top panel";
		toolStripContainer.TopToolStripPanel.AccessibleRole = AccessibleRole.Pane;
		toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStrip);
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar with some information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.ContextMenuStrip = contextMenuShowControlsInStatusBar;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 0);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(1000, 22);
		kryptonStatusStrip.TabIndex = 0;
		kryptonStatusStrip.TabStop = true;
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
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows some information";
		labelInformation.AccessibleName = "Some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(16, 17);
		// 
		// webView
		// 
		webView.AccessibleDescription = I18nStrings.WorldMapAccessibleDescription;
		webView.AccessibleName = "World map";
		webView.AccessibleRole = AccessibleRole.Document;
		webView.AllowExternalDrop = false;
		webView.CreationProperties = null;
		webView.DefaultBackgroundColor = Color.White;
		webView.Dock = DockStyle.Fill;
		webView.Location = new Point(0, 0);
		webView.Name = "webView";
		webView.Size = new Size(1000, 625);
		webView.TabIndex = 0;
		webView.ZoomFactor = 1D;
		webView.MouseEnter += Control_Enter;
		webView.MouseLeave += Control_Leave;
		// 
		// kryptonToolStrip
		// 
		kryptonToolStrip.AccessibleDescription = "Toolbar of main functions";
		kryptonToolStrip.AccessibleName = "Toolbar of main functions";
		kryptonToolStrip.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStrip.AllowClickThrough = true;
		kryptonToolStrip.AllowItemReorder = true;
		kryptonToolStrip.Dock = DockStyle.None;
		kryptonToolStrip.Font = new Font("Segoe UI", 9F);
		kryptonToolStrip.Items.AddRange(new ToolStripItem[] { buttonCommitCoordinates, buttonCancel, separator1, textBoxSearch, buttonSearch, separator2, labelCoordinates });
		kryptonToolStrip.Location = new Point(0, 0);
		kryptonToolStrip.Name = "kryptonToolStrip";
		kryptonToolStrip.Size = new Size(1000, 25);
		kryptonToolStrip.Stretch = true;
		kryptonToolStrip.TabIndex = 0;
		kryptonToolStrip.TabStop = true;
		kryptonToolStrip.Text = "Toolbar of main functions";
		// 
		// buttonCommitCoordinates
		// 
		buttonCommitCoordinates.AccessibleDescription = I18nStrings.WorldMapApplyAccessibleDescription;
		buttonCommitCoordinates.AccessibleName = "Commit coordinates";
		buttonCommitCoordinates.AccessibleRole = AccessibleRole.PushButton;
		buttonCommitCoordinates.Enabled = false;
		buttonCommitCoordinates.Image = Resources.FatcowIcons16px.fatcow_accept_button_16px;
		buttonCommitCoordinates.Name = "buttonCommitCoordinates";
		buttonCommitCoordinates.Size = new Size(136, 22);
		buttonCommitCoordinates.Text = "C&ommit coordinates";
		buttonCommitCoordinates.Click += ButtonCommitCoordinates_Click;
		buttonCommitCoordinates.MouseEnter += Control_Enter;
		buttonCommitCoordinates.MouseLeave += Control_Leave;
		// 
		// buttonCancel
		// 
		buttonCancel.AccessibleDescription = I18nStrings.WorldMapCancelAccessibleDescription;
		buttonCancel.AccessibleName = "Cancel";
		buttonCancel.AccessibleRole = AccessibleRole.PushButton;
		buttonCancel.Image = Resources.FatcowIcons16px.fatcow_cancel_16px;
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Size = new Size(63, 22);
		buttonCancel.Text = I18nStrings.WorldMapCancelText;
		buttonCancel.Click += ButtonCancel_Click;
		buttonCancel.MouseEnter += Control_Enter;
		buttonCancel.MouseLeave += Control_Leave;
		// 
		// separator1
		// 
		separator1.AccessibleDescription = "Just a separator";
		separator1.AccessibleName = "Separator";
		separator1.AccessibleRole = AccessibleRole.Separator;
		separator1.Name = "separator1";
		separator1.Size = new Size(6, 25);
		// 
		// textBoxSearch
		// 
		textBoxSearch.AccessibleDescription = I18nStrings.WorldMapSearchQueryAccessibleDescription;
		textBoxSearch.AccessibleName = "Search box";
		textBoxSearch.AccessibleRole = AccessibleRole.Text;
		textBoxSearch.AutoSize = false;
		textBoxSearch.AutoToolTip = true;
		textBoxSearch.Name = "textBoxSearch";
		textBoxSearch.Size = new Size(260, 25);
		textBoxSearch.KeyDown += TextBoxSearch_KeyDown;
		textBoxSearch.MouseEnter += Control_Enter;
		textBoxSearch.MouseLeave += Control_Leave;
		// 
		// buttonSearch
		// 
		buttonSearch.AccessibleDescription = I18nStrings.WorldMapSearchAccessibleDescription;
		buttonSearch.AccessibleName = "Search";
		buttonSearch.AccessibleRole = AccessibleRole.PushButton;
		buttonSearch.Image = Resources.FatcowIcons16px.fatcow_find_16px;
		buttonSearch.Name = "buttonSearch";
		buttonSearch.Size = new Size(62, 22);
		buttonSearch.Text = I18nStrings.WorldMapSearchText;
		buttonSearch.Click += ButtonSearch_Click;
		buttonSearch.MouseEnter += Control_Enter;
		buttonSearch.MouseLeave += Control_Leave;
		// 
		// separator2
		// 
		separator2.AccessibleDescription = "Just a separator";
		separator2.AccessibleName = "Separator";
		separator2.AccessibleRole = AccessibleRole.Separator;
		separator2.Name = "separator2";
		separator2.Size = new Size(6, 25);
		// 
		// labelCoordinates
		// 
		labelCoordinates.AccessibleDescription = I18nStrings.WorldMapCoordinatesAccessibleDescription;
		labelCoordinates.AccessibleName = "Coordinates";
		labelCoordinates.AccessibleRole = AccessibleRole.StaticText;
		labelCoordinates.DoubleClickEnabled = true;
		labelCoordinates.Image = Resources.FatcowIcons16px.fatcow_location_pin_16px;
		labelCoordinates.Name = "labelCoordinates";
		labelCoordinates.Size = new Size(131, 22);
		labelCoordinates.Text = I18nStrings.WorldMapNoPositionSelected;
		labelCoordinates.DoubleClick += LabelCoordinates_DoubleClick;
		labelCoordinates.MouseEnter += Control_Enter;
		labelCoordinates.MouseLeave += Control_Leave;
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// WorldMapForm
		// 
		AccessibleDescription = I18nStrings.WorldMapAccessibleDescription;
		AccessibleName = I18nStrings.WorldMapTitle;
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		AutoScroll = true;
		ClientSize = new Size(1000, 672);
		ControlBox = false;
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		MinimumSize = new Size(640, 400);
		Name = "WorldMapForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "World map";
		FormClosing += OnFormClosing;
		Load += WorldMapForm_Load;
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		contextMenuShowControlsInStatusBar.ResumeLayout(false);
		((ISupportInitialize)webView).EndInit();
		kryptonToolStrip.ResumeLayout(false);
		kryptonToolStrip.PerformLayout();
		ResumeLayout(false);
	}

	#endregion


	/// <summary>Provides the context menu used to show controls in the status bar.</summary>
	private ContextMenuStrip contextMenuShowControlsInStatusBar;
	
	/// <summary>The container for components managed by the form.</summary>
	private IContainer components;
	
	/// <summary>Represents the status strip hosted by the form.</summary>
	private KryptonStatusStrip kryptonStatusStrip;
	
	/// <summary>The Krypton tool strip used by the form.</summary>
	private KryptonToolStrip kryptonToolStrip;
	
	/// <summary>Manages Krypton theme settings and visual styles for the form.</summary>
	private KryptonManager kryptonManager;
	
	/// <summary>Gets or sets the button that applies changes.</summary>
	private ToolStripButton buttonCommitCoordinates;
	
	/// <summary>Represents the cancel button in the tool strip.</summary>
	private ToolStripButton buttonCancel;
	
	/// <summary>Search button.</summary>
	private ToolStripButton buttonSearch;
	
	/// <summary>Gets or sets the container that hosts the form's tool strip.</summary>
	private ToolStripContainer toolStripContainer;
	
	/// <summary>Displays the current coordinates in the user interface.</summary>
	private ToolStripLabel labelCoordinates;
	
	/// <summary>Represents the menu item used to show or hide the information label.</summary>
	private ToolStripMenuItem menuItemShowInformationLabel;
	
	/// <summary>A separator control used to visually divide items in a toolbar or menu.</summary>
	private ToolStripSeparator separator1;
	
	/// <summary>Represents the second separator in the tool strip.</summary>
	private ToolStripSeparator separator2;
	
	/// <summary>Gets or sets the status label that displays information in the status strip.</summary>
	private ToolStripStatusLabel labelInformation;
	
	/// <summary>The search text box.</summary>
	private ToolStripTextBox textBoxSearch;
	
	/// <summary>The WebView2 control used to display the world map.</summary>
	private WebView2 webView;
}
