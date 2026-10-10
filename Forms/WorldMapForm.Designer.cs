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
		toolStripContainer = new ToolStripContainer();
		statusStrip = new StatusStrip();
		labelInformation = new ToolStripStatusLabel();
		toolStrip = new ToolStrip();
		buttonApply = new ToolStripButton();
		buttonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		textBoxSearch = new ToolStripTextBox();
		buttonSearch = new ToolStripButton();
		toolStripSeparator2 = new ToolStripSeparator();
		labelCoordinates = new ToolStripLabel();
		webView = new WebView2();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		statusStrip.SuspendLayout();
		toolStrip.SuspendLayout();
		((ISupportInitialize)webView).BeginInit();
		SuspendLayout();
		//
		// toolStripContainer
		//
		toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
		toolStripContainer.ContentPanel.Controls.Add(webView);
		toolStripContainer.ContentPanel.Size = new Size(1000, 600);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);
		//
		// statusStrip
		//
		statusStrip.Dock = DockStyle.None;
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		statusStrip.Name = "statusStrip";
		//
		// labelInformation
		//
		labelInformation.Name = "labelInformation";
		labelInformation.Text = string.Empty;
		//
		// toolStrip
		//
		toolStrip.Dock = DockStyle.None;
		toolStrip.GripStyle = ToolStripGripStyle.Hidden;
		toolStrip.Items.AddRange(new ToolStripItem[] { buttonApply, buttonCancel, toolStripSeparator1, textBoxSearch, buttonSearch, toolStripSeparator2, labelCoordinates });
		toolStrip.Name = "toolStrip";
		//
		// buttonApply
		//
		buttonApply.AccessibleDescription = "Returns the selected coordinates and closes the window.";
		buttonApply.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonApply.Enabled = false;
		buttonApply.Name = "buttonApply";
		buttonApply.Text = "&OK";
		buttonApply.Click += ButtonApply_Click;
		buttonApply.MouseEnter += Control_Enter;
		buttonApply.MouseLeave += Control_Leave;
		//
		// buttonCancel
		//
		buttonCancel.AccessibleDescription = "Closes the window without returning coordinates.";
		buttonCancel.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Text = "&Cancel";
		buttonCancel.Click += ButtonCancel_Click;
		buttonCancel.MouseEnter += Control_Enter;
		buttonCancel.MouseLeave += Control_Leave;
		//
		// toolStripSeparator1
		//
		toolStripSeparator1.Name = "toolStripSeparator1";
		//
		// textBoxSearch
		//
		textBoxSearch.AccessibleDescription = "Enter a place, street, country or postal code to search for.";
		textBoxSearch.AutoSize = false;
		textBoxSearch.Name = "textBoxSearch";
		textBoxSearch.Size = new Size(260, 25);
		textBoxSearch.KeyDown += TextBoxSearch_KeyDown;
		textBoxSearch.MouseEnter += Control_Enter;
		textBoxSearch.MouseLeave += Control_Leave;
		//
		// buttonSearch
		//
		buttonSearch.AccessibleDescription = "Searches the entered location and sets a marker on the map.";
		buttonSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
		buttonSearch.Name = "buttonSearch";
		buttonSearch.Text = "&Search";
		buttonSearch.Click += ButtonSearch_Click;
		buttonSearch.MouseEnter += Control_Enter;
		buttonSearch.MouseLeave += Control_Leave;
		//
		// toolStripSeparator2
		//
		toolStripSeparator2.Name = "toolStripSeparator2";
		//
		// labelCoordinates
		//
		labelCoordinates.AccessibleDescription = "Shows the currently selected geographic coordinates.";
		labelCoordinates.Name = "labelCoordinates";
		labelCoordinates.Text = "No position selected";
		labelCoordinates.MouseEnter += Control_Enter;
		labelCoordinates.MouseLeave += Control_Leave;
		//
		// webView
		//
		webView.AllowExternalDrop = false;
		webView.CreationProperties = null;
		webView.DefaultBackgroundColor = Color.White;
		webView.Dock = DockStyle.Fill;
		webView.Name = "webView";
		webView.ZoomFactor = 1D;
		webView.AccessibleDescription = "World map: drag to pan, scroll to zoom, click to set a marker.";
		webView.MouseEnter += Control_Enter;
		webView.MouseLeave += Control_Leave;
		//
		// WorldMapForm
		//
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1000, 672);
		Controls.Add(toolStripContainer);
		MinimumSize = new Size(640, 400);
		Name = "WorldMapForm";
		ShowIcon = false;
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
		toolStrip.ResumeLayout(false);
		toolStrip.PerformLayout();
		((ISupportInitialize)webView).EndInit();
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
}
