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

using Microsoft.Web.WebView2.WinForms;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Designer part of the form that shows an interactive world map for selecting geographic coordinates.</summary>
/// <remarks>This form hosts a toolbar, a WebView2 control and a status bar inside a ToolStripContainer.</remarks>
partial class WorldMapForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	/// <remarks>This method disposes of the resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		// Dispose of the resources used by the form, including any ongoing ephemerides calculation.
		if (disposing && (components != null))
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			components.Dispose();
		}
		// Call the base class Dispose method to ensure proper cleanup.
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify
	/// the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		ComponentResourceManager resources = new ComponentResourceManager(typeof(WorldMapForm));
		toolStripContainer = new ToolStripContainer();
		statusStrip = new StatusStrip();
		labelInformation = new ToolStripStatusLabel();
		webView = new WebView2();
		toolStrip = new ToolStrip();
		buttonApply = new ToolStripButton();
		buttonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		textBoxSearch = new ToolStripTextBox();
		buttonSearch = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		labelCoordinates = new ToolStripLabel();
		toolStripStatusLabel1 = new ToolStripStatusLabel();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		statusStrip.SuspendLayout();
		((ISupportInitialize)webView).BeginInit();
		toolStrip.SuspendLayout();
		SuspendLayout();
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
		toolStripContainer.ContentPanel.Controls.Add(webView);
		toolStripContainer.ContentPanel.Size = new Size(1000, 625);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(1000, 672);
		toolStripContainer.TabIndex = 0;
		// 
		// toolStripContainer.TopToolStripPanel
		// 
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);
		// 
		// statusStrip
		// 
		statusStrip.Dock = DockStyle.None;
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation, toolStripStatusLabel1 });
		statusStrip.Location = new Point(0, 0);
		statusStrip.Name = "statusStrip";
		statusStrip.Size = new Size(1000, 22);
		statusStrip.TabIndex = 0;
		// 
		// labelInformation
		// 
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(0, 17);
		// 
		// webView
		// 
		webView.AccessibleDescription = I18nStrings.WorldMapAccessibleDescription;
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
		// toolStrip
		// 
		toolStrip.Dock = DockStyle.None;
		toolStrip.Font = new Font("Segoe UI", 9F);
		toolStrip.GripStyle = ToolStripGripStyle.Hidden;
		toolStrip.Items.AddRange(new ToolStripItem[] { buttonApply, buttonCancel, toolStripSeparator1, textBoxSearch, buttonSearch, toolStripSeparator2, labelCoordinates });
		toolStrip.Location = new Point(3, 0);
		toolStrip.Name = "toolStrip";
		toolStrip.Size = new Size(512, 25);
		toolStrip.TabIndex = 0;
		// 
		// buttonApply
		// 
		buttonApply.AccessibleDescription = I18nStrings.WorldMapApplyAccessibleDescription;
		buttonApply.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonApply.Enabled = false;
		buttonApply.Name = "buttonApply";
		buttonApply.Size = new Size(27, 22);
		buttonApply.Text = I18nStrings.WorldMapApplyText;
		buttonApply.Click += ButtonApply_Click;
		buttonApply.MouseEnter += Control_Enter;
		buttonApply.MouseLeave += Control_Leave;
		// 
		// buttonCancel
		// 
		buttonCancel.AccessibleDescription = I18nStrings.WorldMapCancelAccessibleDescription;
		buttonCancel.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Size = new Size(47, 22);
		buttonCancel.Text = I18nStrings.WorldMapCancelText;
		buttonCancel.Click += ButtonCancel_Click;
		buttonCancel.MouseEnter += Control_Enter;
		buttonCancel.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator1
		// 
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(6, 25);
		// 
		// textBoxSearch
		// 
		textBoxSearch.AccessibleDescription = I18nStrings.WorldMapSearchQueryAccessibleDescription;
		textBoxSearch.AutoSize = false;
		textBoxSearch.Name = "textBoxSearch";
		textBoxSearch.Size = new Size(260, 25);
		textBoxSearch.KeyDown += TextBoxSearch_KeyDown;
		textBoxSearch.MouseEnter += Control_Enter;
		textBoxSearch.MouseLeave += Control_Leave;
		// 
		// buttonSearch
		// 
		buttonSearch.AccessibleDescription = I18nStrings.WorldMapSearchAccessibleDescription;
		buttonSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonSearch.Name = "buttonSearch";
		buttonSearch.Size = new Size(46, 22);
		buttonSearch.Text = I18nStrings.WorldMapSearchText;
		buttonSearch.Click += ButtonSearch_Click;
		buttonSearch.MouseEnter += Control_Enter;
		buttonSearch.MouseLeave += Control_Leave;
		// 
		// toolStripSeparator2
		// 
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(6, 25);
		// 
		// labelCoordinates
		// 
		labelCoordinates.AccessibleDescription = I18nStrings.WorldMapCoordinatesAccessibleDescription;
		labelCoordinates.Name = "labelCoordinates";
		labelCoordinates.Size = new Size(115, 22);
		labelCoordinates.Text = I18nStrings.WorldMapNoPositionSelected;
		labelCoordinates.MouseEnter += Control_Enter;
		labelCoordinates.MouseLeave += Control_Leave;
		// 
		// toolStripStatusLabel1
		// 
		toolStripStatusLabel1.AccessibleDescription = "Shows some information";
		toolStripStatusLabel1.AccessibleName = "Some information";
		toolStripStatusLabel1.AccessibleRole = AccessibleRole.StaticText;
		toolStripStatusLabel1.AutoToolTip = true;
		toolStripStatusLabel1.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		toolStripStatusLabel1.Size = new Size(144, 17);
		toolStripStatusLabel1.Text = "some information here";
		// 
		// WorldMapForm
		// 
		AccessibleDescription = "Shows the world map";
		AccessibleName = "World map";
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
		Load += WorldMapForm_Load;
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		statusStrip.ResumeLayout(false);
		statusStrip.PerformLayout();
		((ISupportInitialize)webView).EndInit();
		toolStrip.ResumeLayout(false);
		toolStrip.PerformLayout();
		ResumeLayout(false);
	}

	#endregion

	private ToolStripContainer toolStripContainer;
	private StatusStrip statusStrip;
	private ToolStripStatusLabel labelInformation;
	private ToolStrip toolStrip;
	private ToolStripButton buttonApply;
	private ToolStripButton buttonCancel;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripTextBox textBoxSearch;
	private ToolStripButton buttonSearch;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripLabel labelCoordinates;
	private WebView2 webView;
	private ToolStripStatusLabel toolStripStatusLabel1;
}
