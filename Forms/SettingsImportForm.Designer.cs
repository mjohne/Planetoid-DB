/*
 * File:        SettingsImportForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a dialog that imports program settings from CSV, INI, XML, JSON, or YAML.
 * Remarks:     This file contains the Windows Forms designer-generated code for the SettingsImportForm. Do not modify this file manually.
 *
 * Autor:       Michael Johne
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

/// <summary>Represents a dialog that imports program settings from CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged import buttons placed on a <see cref="KryptonPanel"/>. A <see cref="KryptonStatusStrip"/> shows context help at the bottom. The form also supports drag-and-drop of a settings file to trigger the appropriate import method.</remarks>
partial class SettingsImportForm
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingsImportForm));
		kryptonManager = new KryptonManager(components);
		kryptonPanel = new KryptonPanel();
		tableLayoutPanelButtons = new KryptonTableLayoutPanel();
		buttonImportCsv = new KryptonButton();
		buttonImportIni = new KryptonButton();
		buttonImportXml = new KryptonButton();
		buttonImportJson = new KryptonButton();
		buttonImportYaml = new KryptonButton();
		kryptonStatusStrip = new KryptonStatusStrip();
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
		tableLayoutPanelButtons.Controls.Add(buttonImportCsv, 0, 0);
		tableLayoutPanelButtons.Controls.Add(buttonImportIni, 1, 0);
		tableLayoutPanelButtons.Controls.Add(buttonImportXml, 2, 0);
		tableLayoutPanelButtons.Controls.Add(buttonImportJson, 3, 0);
		tableLayoutPanelButtons.Controls.Add(buttonImportYaml, 4, 0);
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
		// buttonImportCsv
		// 
		buttonImportCsv.AccessibleDescription = "Import settings from a CSV file";
		buttonImportCsv.AccessibleName = "Import from CSV";
		buttonImportCsv.AccessibleRole = AccessibleRole.PushButton;
		buttonImportCsv.Dock = DockStyle.Fill;
		buttonImportCsv.Location = new Point(3, 3);
		buttonImportCsv.Name = "buttonImportCsv";
		buttonImportCsv.Size = new Size(78, 39);
		buttonImportCsv.TabIndex = 0;
		buttonImportCsv.Values.DropDownArrowColor = Color.Empty;
		buttonImportCsv.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_excel_16px;
		buttonImportCsv.Values.Text = "CSV";
		buttonImportCsv.Click += ButtonImportCsv_Click;
		buttonImportCsv.Enter += Control_Enter;
		buttonImportCsv.Leave += Control_Leave;
		buttonImportCsv.MouseEnter += Control_Enter;
		buttonImportCsv.MouseLeave += Control_Leave;
		// 
		// buttonImportIni
		// 
		buttonImportIni.AccessibleDescription = "Import settings from an INI file";
		buttonImportIni.AccessibleName = "Import from INI";
		buttonImportIni.AccessibleRole = AccessibleRole.PushButton;
		buttonImportIni.Dock = DockStyle.Fill;
		buttonImportIni.Location = new Point(87, 3);
		buttonImportIni.Name = "buttonImportIni";
		buttonImportIni.Size = new Size(78, 39);
		buttonImportIni.TabIndex = 1;
		buttonImportIni.Values.DropDownArrowColor = Color.Empty;
		buttonImportIni.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_gear_16px;
		buttonImportIni.Values.Text = "INI";
		buttonImportIni.Click += ButtonImportIni_Click;
		buttonImportIni.Enter += Control_Enter;
		buttonImportIni.Leave += Control_Leave;
		buttonImportIni.MouseEnter += Control_Enter;
		buttonImportIni.MouseLeave += Control_Leave;
		// 
		// buttonImportXml
		// 
		buttonImportXml.AccessibleDescription = "Import settings from an XML file";
		buttonImportXml.AccessibleName = "Import from XML";
		buttonImportXml.AccessibleRole = AccessibleRole.PushButton;
		buttonImportXml.Dock = DockStyle.Fill;
		buttonImportXml.Location = new Point(171, 3);
		buttonImportXml.Name = "buttonImportXml";
		buttonImportXml.Size = new Size(78, 39);
		buttonImportXml.TabIndex = 2;
		buttonImportXml.Values.DropDownArrowColor = Color.Empty;
		buttonImportXml.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_16px;
		buttonImportXml.Values.Text = "XML";
		buttonImportXml.Click += ButtonImportXml_Click;
		buttonImportXml.Enter += Control_Enter;
		buttonImportXml.Leave += Control_Leave;
		buttonImportXml.MouseEnter += Control_Enter;
		buttonImportXml.MouseLeave += Control_Leave;
		// 
		// buttonImportJson
		// 
		buttonImportJson.AccessibleDescription = "Import settings from a JSON file";
		buttonImportJson.AccessibleName = "Import from JSON";
		buttonImportJson.AccessibleRole = AccessibleRole.PushButton;
		buttonImportJson.Dock = DockStyle.Fill;
		buttonImportJson.Location = new Point(255, 3);
		buttonImportJson.Name = "buttonImportJson";
		buttonImportJson.Size = new Size(78, 39);
		buttonImportJson.TabIndex = 3;
		buttonImportJson.Values.DropDownArrowColor = Color.Empty;
		buttonImportJson.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		buttonImportJson.Values.Text = "JSON";
		buttonImportJson.Click += ButtonImportJson_Click;
		buttonImportJson.Enter += Control_Enter;
		buttonImportJson.Leave += Control_Leave;
		buttonImportJson.MouseEnter += Control_Enter;
		buttonImportJson.MouseLeave += Control_Leave;
		// 
		// buttonImportYaml
		// 
		buttonImportYaml.AccessibleDescription = "Import settings from a YAML file";
		buttonImportYaml.AccessibleName = "Import from YAML";
		buttonImportYaml.AccessibleRole = AccessibleRole.PushButton;
		buttonImportYaml.Dock = DockStyle.Fill;
		buttonImportYaml.Location = new Point(339, 3);
		buttonImportYaml.Name = "buttonImportYaml";
		buttonImportYaml.Size = new Size(78, 39);
		buttonImportYaml.TabIndex = 4;
		buttonImportYaml.Values.DropDownArrowColor = Color.Empty;
		buttonImportYaml.Values.Image = Resources.FatcowIcons16px.fatcow_page_white_code_red_16px;
		buttonImportYaml.Values.Text = "YAML";
		buttonImportYaml.Click += ButtonImportYaml_Click;
		buttonImportYaml.Enter += Control_Enter;
		buttonImportYaml.Leave += Control_Leave;
		buttonImportYaml.MouseEnter += Control_Enter;
		buttonImportYaml.MouseLeave += Control_Leave;
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
		// SettingsImportForm
		// 
		AccessibleDescription = "Import program settings from CSV, INI, XML, JSON or YAML";
		AccessibleName = "Settings Import";
		AccessibleRole = AccessibleRole.Dialog;
		AllowDrop = true;
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
		Name = "SettingsImportForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Import Settings";
		Load += SettingsImportForm_Load;
		DragDrop += SettingsImportForm_DragDrop;
		DragEnter += SettingsImportForm_DragEnter;
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

	/// <summary>The main panel that hosts all import buttons.</summary>
	/// <remarks>This panel is used to group the import buttons and provide padding around them.</remarks>
	private KryptonPanel kryptonPanel;

	/// <summary>The table layout panel that arranges the five import buttons horizontally.</summary>
	/// <remarks>This panel uses a table layout to ensure that the buttons are equally sized and spaced.</remarks>
	private KryptonTableLayoutPanel tableLayoutPanelButtons;

	/// <summary>The button that imports settings from a CSV file.</summary>
	/// <remarks>This button triggers the import of settings from a CSV file when clicked.</remarks>
	private KryptonButton buttonImportCsv;

	/// <summary>The button that imports settings from an INI file.</summary>
	/// <remarks>This button triggers the import of settings from an INI file when clicked.</remarks>
	private KryptonButton buttonImportIni;

	/// <summary>The button that imports settings from an XML file.</summary>
	/// <remarks>This button triggers the import of settings from an XML file when clicked.</remarks>
	private KryptonButton buttonImportXml;

	/// <summary>The button that imports settings from a JSON file.</summary>
	/// <remarks>This button triggers the import of settings from a JSON file when clicked.</remarks>
	private KryptonButton buttonImportJson;

	/// <summary>The button that imports settings from a YAML file.</summary>
	/// <remarks>This button triggers the import of settings from a YAML file when clicked.</remarks>
	private KryptonButton buttonImportYaml;

	/// <summary>The status strip shown at the bottom of the form.</summary>
	///	<remarks>This status strip displays context help and status information to the user.</remarks>
	private Krypton.Toolkit.KryptonStatusStrip kryptonStatusStrip;

	/// <summary>The status label used to display context help text.</summary>
	/// <remarks>This label is updated when the user hovers over the import buttons to provide information about each import option.</remarks>
	private ToolStripStatusLabel labelInformation;
}
