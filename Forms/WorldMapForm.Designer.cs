/*
 * File:        WorldMapForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
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

namespace Planetoid_DB;

/// <summary>Designer part of the form that shows an interactive world map for selecting geographic coordinates.</summary>
/// <remarks>This form hosts a toolbar, a WebView2 control and a status bar inside a ToolStripContainer.</remarks>
partial class WorldMapForm
{
	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify
	/// the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(WorldMapForm));
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		webView = new WebView2();
		kryptonToolStrip = new KryptonToolStrip();
		buttonApply = new ToolStripButton();
		buttonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		textBoxSearch = new ToolStripTextBox();
		buttonSearch = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		labelCoordinates = new ToolStripLabel();
		kryptonManager = new KryptonManager(components);
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
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
		kryptonToolStrip.AccessibleRole = AccessibleRole.ToolTip;
		kryptonToolStrip.AllowClickThrough = true;
		kryptonToolStrip.AllowItemReorder = true;
		kryptonToolStrip.Dock = DockStyle.None;
		kryptonToolStrip.Font = new Font("Segoe UI", 9F);
		kryptonToolStrip.Items.AddRange(new ToolStripItem[] { buttonApply, buttonCancel, toolStripSeparator1, textBoxSearch, buttonSearch, toolStripSeparator2, labelCoordinates });
		kryptonToolStrip.Location = new Point(0, 0);
		kryptonToolStrip.Name = "kryptonToolStrip";
		kryptonToolStrip.Size = new Size(1000, 25);
		kryptonToolStrip.Stretch = true;
		kryptonToolStrip.TabIndex = 0;
		kryptonToolStrip.TabStop = true;
		kryptonToolStrip.Text = "Toolbar of main functions";
		// 
		// buttonApply
		// 
		buttonApply.AccessibleDescription = I18nStrings.WorldMapApplyAccessibleDescription;
		buttonApply.AccessibleName = "Apply";
		buttonApply.AccessibleRole = AccessibleRole.PushButton;
		buttonApply.Enabled = false;
		buttonApply.Image = Resources.FatcowIcons16px.fatcow_accept_button_16px;
		buttonApply.Name = "buttonApply";
		buttonApply.Size = new Size(43, 22);
		buttonApply.Text = I18nStrings.WorldMapApplyText;
		buttonApply.Click += ButtonApply_Click;
		buttonApply.MouseEnter += Control_Enter;
		buttonApply.MouseLeave += Control_Leave;
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
		// toolStripSeparator1
		// 
		toolStripSeparator1.AccessibleDescription = "Just a separator";
		toolStripSeparator1.AccessibleName = "Separator";
		toolStripSeparator1.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(6, 25);
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
		// toolStripSeparator2
		// 
		toolStripSeparator2.AccessibleDescription = "Just a separator";
		toolStripSeparator2.AccessibleName = "Separator";
		toolStripSeparator2.AccessibleRole = AccessibleRole.Separator;
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(6, 25);
		// 
		// labelCoordinates
		// 
		labelCoordinates.AccessibleDescription = I18nStrings.WorldMapCoordinatesAccessibleDescription;
		labelCoordinates.AccessibleName = "Coordinates";
		labelCoordinates.AccessibleRole = AccessibleRole.PushButton;
		labelCoordinates.Image = Resources.FatcowIcons16px.fatcow_location_pin_16px;
		labelCoordinates.Name = "labelCoordinates";
		labelCoordinates.Size = new Size(131, 22);
		labelCoordinates.Text = I18nStrings.WorldMapNoPositionSelected;
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
		StartPosition = FormStartPosition.CenterParent;
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
		((ISupportInitialize)webView).EndInit();
		kryptonToolStrip.ResumeLayout(false);
		kryptonToolStrip.PerformLayout();
		ResumeLayout(false);
	}

	#endregion

	private ToolStripContainer toolStripContainer;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonToolStrip kryptonToolStrip;
	private ToolStripButton buttonApply;
	private ToolStripButton buttonCancel;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripTextBox textBoxSearch;
	private ToolStripButton buttonSearch;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripLabel labelCoordinates;
	private WebView2 webView;
	private KryptonManager kryptonManager;
	private IContainer components;
}
