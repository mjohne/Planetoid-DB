// Designer file for SettingsImportForm.

using Krypton.Toolkit;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a dialog that imports program settings from CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged import buttons placed on a
/// <see cref="KryptonPanel"/>. A <see cref="KryptonStatusStrip"/> shows context help at the bottom.
/// The form also supports drag-and-drop of a settings file to trigger the appropriate import method.</remarks>
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

		kryptonManager = new KryptonManager(components);
		kryptonPanel = new KryptonPanel();
		tableLayoutPanelButtons = new TableLayoutPanel();
		buttonImportCsv = new KryptonButton();
		buttonImportIni = new KryptonButton();
		buttonImportXml = new KryptonButton();
		buttonImportJson = new KryptonButton();
		buttonImportYaml = new KryptonButton();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();

		// --- SuspendLayout / BeginInit ---
		((ISupportInitialize)kryptonPanel).BeginInit();
		kryptonPanel.SuspendLayout();
		tableLayoutPanelButtons.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		SuspendLayout();

		// ===========================
		// kryptonPanel
		// ===========================
		kryptonPanel.Dock = DockStyle.Fill;
		kryptonPanel.Location = new Point(0, 0);
		kryptonPanel.Name = "kryptonPanel";
		kryptonPanel.Padding = new Padding(12);
		kryptonPanel.Size = new Size(560, 100);
		kryptonPanel.TabIndex = 0;
		kryptonPanel.Controls.Add(tableLayoutPanelButtons);

		// ===========================
		// tableLayoutPanelButtons — 5 equally-sized columns, 1 row
		// ===========================
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
		tableLayoutPanelButtons.Size = new Size(536, 52);
		tableLayoutPanelButtons.TabIndex = 0;

		// ===========================
		// buttonImportCsv
		// ===========================
		buttonImportCsv.AccessibleDescription = "Import settings from a CSV file";
		buttonImportCsv.AccessibleName = "Import from CSV";
		buttonImportCsv.AccessibleRole = AccessibleRole.PushButton;
		buttonImportCsv.Dock = DockStyle.Fill;
		buttonImportCsv.Location = new Point(3, 3);
		buttonImportCsv.Name = "buttonImportCsv";
		buttonImportCsv.Size = new Size(101, 46);
		buttonImportCsv.TabIndex = 0;
		buttonImportCsv.Values.Text = "CSV";
		buttonImportCsv.Click += ButtonImportCsv_Click;
		buttonImportCsv.MouseEnter += Control_Enter;
		buttonImportCsv.MouseLeave += Control_Leave;

		// ===========================
		// buttonImportIni
		// ===========================
		buttonImportIni.AccessibleDescription = "Import settings from an INI file";
		buttonImportIni.AccessibleName = "Import from INI";
		buttonImportIni.AccessibleRole = AccessibleRole.PushButton;
		buttonImportIni.Dock = DockStyle.Fill;
		buttonImportIni.Location = new Point(110, 3);
		buttonImportIni.Name = "buttonImportIni";
		buttonImportIni.Size = new Size(101, 46);
		buttonImportIni.TabIndex = 1;
		buttonImportIni.Values.Text = "INI";
		buttonImportIni.Click += ButtonImportIni_Click;
		buttonImportIni.MouseEnter += Control_Enter;
		buttonImportIni.MouseLeave += Control_Leave;

		// ===========================
		// buttonImportXml
		// ===========================
		buttonImportXml.AccessibleDescription = "Import settings from an XML file";
		buttonImportXml.AccessibleName = "Import from XML";
		buttonImportXml.AccessibleRole = AccessibleRole.PushButton;
		buttonImportXml.Dock = DockStyle.Fill;
		buttonImportXml.Location = new Point(217, 3);
		buttonImportXml.Name = "buttonImportXml";
		buttonImportXml.Size = new Size(101, 46);
		buttonImportXml.TabIndex = 2;
		buttonImportXml.Values.Text = "XML";
		buttonImportXml.Click += ButtonImportXml_Click;
		buttonImportXml.MouseEnter += Control_Enter;
		buttonImportXml.MouseLeave += Control_Leave;

		// ===========================
		// buttonImportJson
		// ===========================
		buttonImportJson.AccessibleDescription = "Import settings from a JSON file";
		buttonImportJson.AccessibleName = "Import from JSON";
		buttonImportJson.AccessibleRole = AccessibleRole.PushButton;
		buttonImportJson.Dock = DockStyle.Fill;
		buttonImportJson.Location = new Point(324, 3);
		buttonImportJson.Name = "buttonImportJson";
		buttonImportJson.Size = new Size(101, 46);
		buttonImportJson.TabIndex = 3;
		buttonImportJson.Values.Text = "JSON";
		buttonImportJson.Click += ButtonImportJson_Click;
		buttonImportJson.MouseEnter += Control_Enter;
		buttonImportJson.MouseLeave += Control_Leave;

		// ===========================
		// buttonImportYaml
		// ===========================
		buttonImportYaml.AccessibleDescription = "Import settings from a YAML file";
		buttonImportYaml.AccessibleName = "Import from YAML";
		buttonImportYaml.AccessibleRole = AccessibleRole.PushButton;
		buttonImportYaml.Dock = DockStyle.Fill;
		buttonImportYaml.Location = new Point(431, 3);
		buttonImportYaml.Name = "buttonImportYaml";
		buttonImportYaml.Size = new Size(102, 46);
		buttonImportYaml.TabIndex = 4;
		buttonImportYaml.Values.Text = "YAML";
		buttonImportYaml.Click += ButtonImportYaml_Click;
		buttonImportYaml.MouseEnter += Control_Enter;
		buttonImportYaml.MouseLeave += Control_Leave;

		// ===========================
		// kryptonStatusStrip
		// ===========================
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange([labelInformation]);
		kryptonStatusStrip.Location = new Point(0, 100);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.Size = new Size(560, 22);
		kryptonStatusStrip.TabIndex = 1;

		// ===========================
		// labelInformation
		// ===========================
		labelInformation.AccessibleDescription = "Status information";
		labelInformation.AccessibleName = "Status";
		labelInformation.AccessibleRole = AccessibleRole.StatusBar;
		labelInformation.Enabled = false;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(0, 17);

		// ===========================
		// SettingsImportForm
		// ===========================
		AccessibleDescription = "Import program settings from CSV, INI, XML, JSON or YAML";
		AccessibleName = "Settings Import";
		AccessibleRole = AccessibleRole.Dialog;
		AllowDrop = true;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(560, 122);
		ControlBox = false;
		Controls.Add(kryptonPanel);
		Controls.Add(kryptonStatusStrip);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "SettingsImportForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Import Settings";
		Load += SettingsImportForm_Load;
		DragEnter += SettingsImportForm_DragEnter;
		DragDrop += SettingsImportForm_DragDrop;

		// ===========================
		// RESUME LAYOUT (inner to outer)
		// ===========================
		tableLayoutPanelButtons.ResumeLayout(false);
		kryptonPanel.ResumeLayout(false);
		((ISupportInitialize)kryptonPanel).EndInit();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	/// <summary>The Krypton Manager component that manages the application palette.</summary>
	private KryptonManager kryptonManager;

	/// <summary>The main panel that hosts all import buttons.</summary>
	private KryptonPanel kryptonPanel;

	/// <summary>The table layout panel that arranges the five import buttons horizontally.</summary>
	private TableLayoutPanel tableLayoutPanelButtons;

	/// <summary>The button that imports settings from a CSV file.</summary>
	private KryptonButton buttonImportCsv;

	/// <summary>The button that imports settings from an INI file.</summary>
	private KryptonButton buttonImportIni;

	/// <summary>The button that imports settings from an XML file.</summary>
	private KryptonButton buttonImportXml;

	/// <summary>The button that imports settings from a JSON file.</summary>
	private KryptonButton buttonImportJson;

	/// <summary>The button that imports settings from a YAML file.</summary>
	private KryptonButton buttonImportYaml;

	/// <summary>The status strip shown at the bottom of the form.</summary>
	private KryptonStatusStrip kryptonStatusStrip;

	/// <summary>The status label used to display context help text.</summary>
	private ToolStripStatusLabel labelInformation;
}
