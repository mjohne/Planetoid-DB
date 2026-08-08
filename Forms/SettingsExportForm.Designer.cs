// Designer file for SettingsExportForm.

using Krypton.Toolkit;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Represents a dialog that exports program settings to CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged export buttons placed on a
/// <see cref="KryptonPanel"/>. A <see cref="KryptonStatusStrip"/> shows context help at the bottom.
/// This file is maintained by hand because no WinForms designer session is available in this
/// environment; it follows the same hand-written style used by <c>SettingsForm.Designer.cs</c>.</remarks>
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

		kryptonManager = new KryptonManager(components);
		kryptonPanel = new KryptonPanel();
		tableLayoutPanelButtons = new TableLayoutPanel();
		buttonExportCsv = new KryptonButton();
		buttonExportIni = new KryptonButton();
		buttonExportXml = new KryptonButton();
		buttonExportJson = new KryptonButton();
		buttonExportYaml = new KryptonButton();
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
		tableLayoutPanelButtons.Size = new Size(536, 52);
		tableLayoutPanelButtons.TabIndex = 0;

		// ===========================
		// buttonExportCsv
		// ===========================
		buttonExportCsv.AccessibleDescription = "Export settings as CSV";
		buttonExportCsv.AccessibleName = "Export as CSV";
		buttonExportCsv.AccessibleRole = AccessibleRole.PushButton;
		buttonExportCsv.Dock = DockStyle.Fill;
		buttonExportCsv.Location = new Point(3, 3);
		buttonExportCsv.Name = "buttonExportCsv";
		buttonExportCsv.Size = new Size(101, 46);
		buttonExportCsv.TabIndex = 0;
		buttonExportCsv.Values.Text = "CSV";
		buttonExportCsv.Click += ButtonExportCsv_Click;
		buttonExportCsv.MouseEnter += Control_Enter;
		buttonExportCsv.MouseLeave += Control_Leave;

		// ===========================
		// buttonExportIni
		// ===========================
		buttonExportIni.AccessibleDescription = "Export settings as INI";
		buttonExportIni.AccessibleName = "Export as INI";
		buttonExportIni.AccessibleRole = AccessibleRole.PushButton;
		buttonExportIni.Dock = DockStyle.Fill;
		buttonExportIni.Location = new Point(110, 3);
		buttonExportIni.Name = "buttonExportIni";
		buttonExportIni.Size = new Size(101, 46);
		buttonExportIni.TabIndex = 1;
		buttonExportIni.Values.Text = "INI";
		buttonExportIni.Click += ButtonExportIni_Click;
		buttonExportIni.MouseEnter += Control_Enter;
		buttonExportIni.MouseLeave += Control_Leave;

		// ===========================
		// buttonExportXml
		// ===========================
		buttonExportXml.AccessibleDescription = "Export settings as XML";
		buttonExportXml.AccessibleName = "Export as XML";
		buttonExportXml.AccessibleRole = AccessibleRole.PushButton;
		buttonExportXml.Dock = DockStyle.Fill;
		buttonExportXml.Location = new Point(217, 3);
		buttonExportXml.Name = "buttonExportXml";
		buttonExportXml.Size = new Size(101, 46);
		buttonExportXml.TabIndex = 2;
		buttonExportXml.Values.Text = "XML";
		buttonExportXml.Click += ButtonExportXml_Click;
		buttonExportXml.MouseEnter += Control_Enter;
		buttonExportXml.MouseLeave += Control_Leave;

		// ===========================
		// buttonExportJson
		// ===========================
		buttonExportJson.AccessibleDescription = "Export settings as JSON";
		buttonExportJson.AccessibleName = "Export as JSON";
		buttonExportJson.AccessibleRole = AccessibleRole.PushButton;
		buttonExportJson.Dock = DockStyle.Fill;
		buttonExportJson.Location = new Point(324, 3);
		buttonExportJson.Name = "buttonExportJson";
		buttonExportJson.Size = new Size(101, 46);
		buttonExportJson.TabIndex = 3;
		buttonExportJson.Values.Text = "JSON";
		buttonExportJson.Click += ButtonExportJson_Click;
		buttonExportJson.MouseEnter += Control_Enter;
		buttonExportJson.MouseLeave += Control_Leave;

		// ===========================
		// buttonExportYaml
		// ===========================
		buttonExportYaml.AccessibleDescription = "Export settings as YAML";
		buttonExportYaml.AccessibleName = "Export as YAML";
		buttonExportYaml.AccessibleRole = AccessibleRole.PushButton;
		buttonExportYaml.Dock = DockStyle.Fill;
		buttonExportYaml.Location = new Point(431, 3);
		buttonExportYaml.Name = "buttonExportYaml";
		buttonExportYaml.Size = new Size(102, 46);
		buttonExportYaml.TabIndex = 4;
		buttonExportYaml.Values.Text = "YAML";
		buttonExportYaml.Click += ButtonExportYaml_Click;
		buttonExportYaml.MouseEnter += Control_Enter;
		buttonExportYaml.MouseLeave += Control_Leave;

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
		// SettingsExportForm
		// ===========================
		AccessibleDescription = "Export program settings to CSV, INI, XML, JSON or YAML";
		AccessibleName = "Settings Export";
		AccessibleRole = AccessibleRole.Dialog;
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
		Name = "SettingsExportForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Export Settings";
		Load += SettingsExportForm_Load;

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

	/// <summary>The main panel that hosts all export buttons.</summary>
	private KryptonPanel kryptonPanel;

	/// <summary>The table layout panel that arranges the five export buttons horizontally.</summary>
	private TableLayoutPanel tableLayoutPanelButtons;

	/// <summary>The button that exports settings as CSV.</summary>
	private KryptonButton buttonExportCsv;

	/// <summary>The button that exports settings as INI.</summary>
	private KryptonButton buttonExportIni;

	/// <summary>The button that exports settings as XML.</summary>
	private KryptonButton buttonExportXml;

	/// <summary>The button that exports settings as JSON.</summary>
	private KryptonButton buttonExportJson;

	/// <summary>The button that exports settings as YAML.</summary>
	private KryptonButton buttonExportYaml;

	/// <summary>The status strip shown at the bottom of the form.</summary>
	private KryptonStatusStrip kryptonStatusStrip;

	/// <summary>The status label used to display context help text.</summary>
	private ToolStripStatusLabel labelInformation;
}
