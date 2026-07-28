namespace Planetoid_DB.Forms;

using Krypton.Toolkit;

using System.ComponentModel;

partial class AsteroidGameForm
{
	/// <summary>Required designer variable.</summary>
	private IContainer components = null;

	/// <summary>Releases all resources used by the <see cref="AsteroidGameForm"/>.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>Initializes the components of the form.</summary>
	private void InitializeComponent()
	{
		ComponentResourceManager resources = new ComponentResourceManager(typeof(AsteroidGameForm));
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new StatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanelMain = new KryptonPanel();
		panelGl = new Panel();
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		SuspendLayout();
		// 
		// toolStripContainer
		// 
		toolStripContainer.AccessibleDescription = "ToolStrip container for the Asteroid game";
		toolStripContainer.AccessibleName = "ToolStrip container";
		toolStripContainer.AccessibleRole = AccessibleRole.Client;
		// 
		// toolStripContainer.BottomToolStripPanel
		// 
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		// 
		// toolStripContainer.ContentPanel
		// 
		toolStripContainer.ContentPanel.AccessibleDescription = "Content panel for the Asteroid game";
		toolStripContainer.ContentPanel.AccessibleName = "Content panel";
		toolStripContainer.ContentPanel.AccessibleRole = AccessibleRole.Client;
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainer.ContentPanel.Margin = new Padding(3, 2, 3, 2);
		toolStripContainer.ContentPanel.Size = new Size(982, 506);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Margin = new Padding(3, 2, 3, 2);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(982, 553);
		toolStripContainer.TabIndex = 0;
		toolStripContainer.Text = "ToolStrip container";
		// 
		// kryptonStatusStrip
		// 
		kryptonStatusStrip.AccessibleDescription = "Status strip for the Asteroid game";
		kryptonStatusStrip.AccessibleName = "Status strip";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.ImageScalingSize = new Size(20, 20);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 0);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.Size = new Size(982, 22);
		kryptonStatusStrip.TabIndex = 0;
		// 
		// labelInformation
		// 
		labelInformation.AccessibleDescription = "Game information and status";
		labelInformation.AccessibleName = "Information label";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(967, 17);
		labelInformation.Spring = true;
		labelInformation.Text = "Arrow keys: move, Space: shoot, Enter: start";
		labelInformation.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// kryptonPanelMain
		// 
		kryptonPanelMain.AccessibleDescription = "Main panel for the Asteroid game";
		kryptonPanelMain.AccessibleName = "Main panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Client;
		kryptonPanelMain.Controls.Add(panelGl);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Margin = new Padding(3, 2, 3, 2);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.Size = new Size(982, 506);
		kryptonPanelMain.TabIndex = 0;
		// 
		// panelGl
		// 
		panelGl.AccessibleDescription = "Container panel for OpenGL rendering surface";
		panelGl.AccessibleName = "GL panel";
		panelGl.AccessibleRole = AccessibleRole.Client;
		panelGl.BackColor = Color.Black;
		panelGl.Dock = DockStyle.Fill;
		panelGl.Location = new Point(0, 0);
		panelGl.Margin = new Padding(3, 2, 3, 2);
		panelGl.Name = "panelGl";
		panelGl.Size = new Size(982, 506);
		panelGl.TabIndex = 0;
		// 
		// AsteroidGameForm
		// 
		AccessibleDescription = "Asteroids arcade game window";
		AccessibleName = "Asteroid game";
		AccessibleRole = AccessibleRole.Window;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(982, 553);
		Controls.Add(toolStripContainer);
		Icon = (Icon)resources.GetObject("$this.Icon");
		KeyPreview = true;
		Margin = new Padding(3, 2, 3, 2);
		MinimumSize = new Size(640, 480);
		Name = "AsteroidGameForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Asteroids Game";
		FormClosing += AsteroidGameForm_FormClosing;
		Load += AsteroidGameForm_Load;
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		ResumeLayout(false);
	}

	#endregion

	private ToolStripContainer toolStripContainer;
	private KryptonPanel kryptonPanelMain;
	private Panel panelGl;
	private StatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
}