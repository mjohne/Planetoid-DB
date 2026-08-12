/*
 * File:        SettingsExportForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a dialog that exports program settings to CSV, INI, XML, JSON, or YAML.
 * Remarks:     This file contains the Windows Forms designer-generated code for the SettingsExportForm. Do not modify this file manually.
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

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a dialog that exports program settings to CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged export buttons placed on a <see cref="KryptonPanel"/>. A <see cref="KryptonStatusStrip"/> shows context help at the bottom.</remarks>
partial class SettingsExportForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field is used by the Windows Forms designer to manage components.</remarks>
	private IContainer components = null;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	/// <remarks>This method disposes of the resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support — do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingsExportForm));
		kryptonManager = new KryptonManager(components);
		kryptonPanel = new KryptonPanel();
		tableLayoutPanelButtons = new KryptonTableLayoutPanel();
		buttonExportCsv = new KryptonButton();
		buttonExportIni = new KryptonButton();
		buttonExportXml = new KryptonButton();
		buttonExportJson = new KryptonButton();
		buttonExportYaml = new KryptonButton();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		labelInformation = new ToolStripStatusLabel();
		((ISupportInitialize)kryptonPanel).BeginInit();
		kryptonPanel.SuspendLayout();
		tableLayoutPanelButtons.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// kryptonManager
		// 
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
		// 
		// kryptonPanel
		// 
		kryptonPanel.Controls.Add(tableLayoutPanelButtons);
		kryptonPanel.Dock = DockStyle.Fill;
		kryptonPanel.Location = new Point(0, 0);
		kryptonPanel.Name = "kryptonPanel";
		kryptonPanel.Padding = new Padding(12);
		kryptonPanel.Size = new Size(444, 69);
		kryptonPanel.TabIndex = 0;
		kryptonPanel.Enter += Control_Enter;
		kryptonPanel.Leave += Control_Leave;
		kryptonPanel.MouseEnter += Control_Enter;
		kryptonPanel.MouseLeave += Control_Leave;
		// 
		// tableLayoutPanelButtons
		// 
		tableLayoutPanelButtons.AccessibleDescription = "Groups the data";
		tableLayoutPanelButtons.AccessibleName = "Table panel";
		tableLayoutPanelButtons.AccessibleRole = AccessibleRole.Pane;
		tableLayoutPanelButtons.ColumnCount = 5;
		tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanelButtons.Controls.Add(buttonExportCsv, 0, 0);
		tableLayoutPanelButtons.Controls.Add(buttonExportIni, 1, 0);
		tableLayoutPanelButtons.Controls.Add(buttonExportXml, 2, 0);
		tableLayoutPanelButtons.Controls.Add(buttonExportJson, 3, 0);
		tableLayoutPanelButtons.Controls.Add(buttonExportYaml, 4, 0);
		tableLayoutPanelButtons.Dock = DockStyle.Fill;
		tableLayoutPanelButtons.Location = new Point(12, 12);
		tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
		tableLayoutPanelButtons.RowCount = 1;
		tableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
		tableLayoutPanelButtons.Size = new Size(420, 45);
		tableLayoutPanelButtons.TabIndex = 0;
		tableLayoutPanelButtons.Enter += Control_Enter;
		tableLayoutPanelButtons.Leave += Control_Leave;
		tableLayoutPanelButtons.MouseEnter += Control_Enter;
		tableLayoutPanelButtons.MouseLeave += Control_Leave;
		// 
		// buttonExportCsv
		// 
		buttonExportCsv.AccessibleDescription = "Export settings as CSV";
		buttonExportCsv.AccessibleName = "Export as CSV";
		buttonExportCsv.AccessibleRole = AccessibleRole.PushButton;
		buttonExportCsv.Dock = DockStyle.Fill;
		buttonExportCsv.Location = new Point(3, 3);
		buttonExportCsv.Name = "buttonExportCsv";
		buttonExportCsv.Size = new Size(78, 39);
		buttonExportCsv.TabIndex = 0;
		buttonExportCsv.Values.DropDownArrowColor = Color.Empty;
		buttonExportCsv.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		buttonExportCsv.Values.Text = "CSV";
		buttonExportCsv.Click += ButtonExportCsv_Click;
		buttonExportCsv.Enter += Control_Enter;
		buttonExportCsv.Leave += Control_Leave;
		buttonExportCsv.MouseEnter += Control_Enter;
		buttonExportCsv.MouseLeave += Control_Leave;
		// 
		// buttonExportIni
		// 
		buttonExportIni.AccessibleDescription = "Export settings as INI";
		buttonExportIni.AccessibleName = "Export as INI";
		buttonExportIni.AccessibleRole = AccessibleRole.PushButton;
		buttonExportIni.Dock = DockStyle.Fill;
		buttonExportIni.Location = new Point(87, 3);
		buttonExportIni.Name = "buttonExportIni";
		buttonExportIni.Size = new Size(78, 39);
		buttonExportIni.TabIndex = 1;
		buttonExportIni.Values.DropDownArrowColor = Color.Empty;
		buttonExportIni.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_gear_16px;
		buttonExportIni.Values.Text = "INI";
		buttonExportIni.Click += ButtonExportIni_Click;
		buttonExportIni.Enter += Control_Enter;
		buttonExportIni.Leave += Control_Leave;
		buttonExportIni.MouseEnter += Control_Enter;
		buttonExportIni.MouseLeave += Control_Leave;
		// 
		// buttonExportXml
		// 
		buttonExportXml.AccessibleDescription = "Export settings as XML";
		buttonExportXml.AccessibleName = "Export as XML";
		buttonExportXml.AccessibleRole = AccessibleRole.PushButton;
		buttonExportXml.Dock = DockStyle.Fill;
		buttonExportXml.Location = new Point(171, 3);
		buttonExportXml.Name = "buttonExportXml";
		buttonExportXml.Size = new Size(78, 39);
		buttonExportXml.TabIndex = 2;
		buttonExportXml.Values.DropDownArrowColor = Color.Empty;
		buttonExportXml.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		buttonExportXml.Values.Text = "XML";
		buttonExportXml.Click += ButtonExportXml_Click;
		buttonExportXml.Enter += Control_Enter;
		buttonExportXml.Leave += Control_Leave;
		buttonExportXml.MouseEnter += Control_Enter;
		buttonExportXml.MouseLeave += Control_Leave;
		// 
		// buttonExportJson
		// 
		buttonExportJson.AccessibleDescription = "Export settings as JSON";
		buttonExportJson.AccessibleName = "Export as JSON";
		buttonExportJson.AccessibleRole = AccessibleRole.PushButton;
		buttonExportJson.Dock = DockStyle.Fill;
		buttonExportJson.Location = new Point(255, 3);
		buttonExportJson.Name = "buttonExportJson";
		buttonExportJson.Size = new Size(78, 39);
		buttonExportJson.TabIndex = 3;
		buttonExportJson.Values.DropDownArrowColor = Color.Empty;
		buttonExportJson.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		buttonExportJson.Values.Text = "JSON";
		buttonExportJson.Click += ButtonExportJson_Click;
		buttonExportJson.Enter += Control_Enter;
		buttonExportJson.Leave += Control_Leave;
		buttonExportJson.MouseEnter += Control_Enter;
		buttonExportJson.MouseLeave += Control_Leave;
		// 
		// buttonExportYaml
		// 
		buttonExportYaml.AccessibleDescription = "Export settings as YAML";
		buttonExportYaml.AccessibleName = "Export as YAML";
		buttonExportYaml.AccessibleRole = AccessibleRole.PushButton;
		buttonExportYaml.Dock = DockStyle.Fill;
		buttonExportYaml.Location = new Point(339, 3);
		buttonExportYaml.Name = "buttonExportYaml";
		buttonExportYaml.Size = new Size(78, 39);
		buttonExportYaml.TabIndex = 4;
		buttonExportYaml.Values.DropDownArrowColor = Color.Empty;
		buttonExportYaml.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		buttonExportYaml.Values.Text = "YAML";
		buttonExportYaml.Click += ButtonExportYaml_Click;
		buttonExportYaml.Enter += Control_Enter;
		buttonExportYaml.Leave += Control_Leave;
		buttonExportYaml.MouseEnter += Control_Enter;
		buttonExportYaml.MouseLeave += Control_Leave;
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Shows some information";
		kryptonStatusStrip.AccessibleName = "Status bar with some information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation, labelInformation });
		kryptonStatusStrip.Location = new Point(0, 69);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(444, 22);
		kryptonStatusStrip.TabIndex = 1;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Enter += Control_Enter;
		kryptonStatusStrip.Leave += Control_Leave;
		kryptonStatusStrip.MouseEnter += Control_Enter;
		kryptonStatusStrip.MouseLeave += Control_Leave;
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Shows some information";
		labelInformation.AccessibleName = "Shows some information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = Resources.FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(0, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Shows some information";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;
		// 
		// SettingsExportForm
		// 
		AccessibleDescription = "Export program settings to CSV, INI, XML, JSON or YAML";
		AccessibleName = "Settings Export";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(444, 91);
		ControlBox = false;
		Controls.Add(kryptonPanel);
		Controls.Add(kryptonStatusStrip);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "SettingsExportForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Export Settings";
		Load += SettingsExportForm_Load;
		((ISupportInitialize)kryptonPanel).EndInit();
		kryptonPanel.ResumeLayout(false);
		tableLayoutPanelButtons.ResumeLayout(false);
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	/// <summary>The Krypton Manager component that manages the application palette.</summary>
	/// <remarks>This component is used to apply the Krypton theme to the form and its controls.</remarks>
	private KryptonManager kryptonManager;

	/// <summary>The main panel that hosts all export buttons.</summary>
	/// <remarks>This panel is used to group the export buttons and provide padding around them.</remarks>
	private KryptonPanel kryptonPanel;

	/// <summary>The table layout panel that arranges the five export buttons horizontally.</summary>
	/// <remarks>This panel uses a table layout to ensure that the buttons are equally sized and spaced.</remarks>
	private KryptonTableLayoutPanel tableLayoutPanelButtons;

	/// <summary>The button that exports settings as CSV.</summary>
	/// <remarks>This button triggers the export of settings as a CSV file when clicked.</remarks>
	private KryptonButton buttonExportCsv;

	/// <summary>The button that exports settings as INI.</summary>
	/// <remarks>This button triggers the export of settings as an INI file when clicked.</remarks>
	private KryptonButton buttonExportIni;

	/// <summary>The button that exports settings as XML.</summary>
	/// <remarks>This button triggers the export of settings as an XML file when clicked.</remarks>
	private KryptonButton buttonExportXml;

	/// <summary>The button that exports settings as JSON.</summary>
	/// <remarks>This button triggers the export of settings as a JSON file when clicked.</remarks>
	private KryptonButton buttonExportJson;

	/// <summary>The button that exports settings as YAML.</summary>
	/// <remarks>This button triggers the export of settings as a YAML file when clicked.</remarks>
	private KryptonButton buttonExportYaml;

	/// <summary>The status strip shown at the bottom of the form.</summary>
	/// <remarks>This status strip displays context help and status information to the user.</remarks>
	private KryptonStatusStrip kryptonStatusStrip;

	/// <summary>The status label used to display context help text.</summary>
	/// <remarks>This label is updated when the user hovers over the export buttons to provide information about each export option.</remarks>
	private ToolStripStatusLabel labelInformation;
}
