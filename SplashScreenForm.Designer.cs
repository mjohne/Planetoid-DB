using System.ComponentModel;
using Krypton.Toolkit;

namespace Planetoid_DB
{
  partial class SplashScreenForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SplashScreenForm));
			progressBarSplash = new KryptonProgressBar();
			labelTitle = new Label();
			labelVersion = new Label();
			toolTip = new ToolTip(components);
			kryptonManager = new KryptonManager(components);
			SuspendLayout();
			// 
			// progressBarSplash
			// 
			progressBarSplash.AccessibleDescription = "Loads the data";
			progressBarSplash.AccessibleName = "Progress Bar";
			progressBarSplash.AccessibleRole = AccessibleRole.ProgressBar;
			progressBarSplash.Dock = DockStyle.Bottom;
			progressBarSplash.Location = new Point(0, 337);
			progressBarSplash.Margin = new Padding(4, 3, 4, 3);
			progressBarSplash.Name = "progressBarSplash";
			progressBarSplash.Size = new Size(481, 23);
			progressBarSplash.Step = 1;
			progressBarSplash.TabIndex = 3;
			progressBarSplash.Text = "Loading data...";
			progressBarSplash.TextBackdropColor = Color.Empty;
			progressBarSplash.TextShadowColor = Color.Empty;
			toolTip.SetToolTip(progressBarSplash, "Loads the data");
			progressBarSplash.Values.Text = "Loading data...";
			// 
			// labelTitle
			// 
			labelTitle.AccessibleDescription = "Shows the applicatiopn name";
			labelTitle.AccessibleName = "Application name";
			labelTitle.AccessibleRole = AccessibleRole.Text;
			labelTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			labelTitle.AutoSize = true;
			labelTitle.BackColor = Color.Transparent;
			labelTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
			labelTitle.ForeColor = Color.LightCyan;
			labelTitle.Location = new Point(223, 25);
			labelTitle.Margin = new Padding(4, 0, 4, 0);
			labelTitle.Name = "labelTitle";
			labelTitle.Size = new Size(221, 45);
			labelTitle.TabIndex = 0;
			labelTitle.Text = "Planetoid-DB";
			toolTip.SetToolTip(labelTitle, "Application name");
			labelTitle.DoubleClick += CopyToClipboard_DoubleClick;
			// 
			// labelVersion
			// 
			labelVersion.AccessibleDescription = "Shows the version number";
			labelVersion.AccessibleName = "Version";
			labelVersion.AccessibleRole = AccessibleRole.Text;
			labelVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			labelVersion.BackColor = Color.Transparent;
			labelVersion.Font = new Font("Segoe UI", 8.5F);
			labelVersion.ForeColor = Color.White;
			labelVersion.Location = new Point(223, 77);
			labelVersion.Margin = new Padding(4, 0, 4, 0);
			labelVersion.Name = "labelVersion";
			labelVersion.Size = new Size(258, 25);
			labelVersion.TabIndex = 1;
			labelVersion.Text = "Version: X.X.X.X";
			labelVersion.TextAlign = ContentAlignment.MiddleCenter;
			toolTip.SetToolTip(labelVersion, "Version number");
			labelVersion.DoubleClick += CopyToClipboard_DoubleClick;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// SplashScreenForm
			// 
			AccessibleDescription = "Shows the splash screen and the loading progress of the data";
			AccessibleName = "splash screen";
			AccessibleRole = AccessibleRole.Window;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
			ClientSize = new Size(481, 360);
			Controls.Add(labelVersion);
			Controls.Add(labelTitle);
			Controls.Add(progressBarSplash);
			Cursor = Cursors.AppStarting;
			FormBorderStyle = FormBorderStyle.Fixed3D;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "SplashScreenForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Splash Screen";
			toolTip.SetToolTip(this, "splash screen");
			TopMost = true;
			FormClosed += SplashScreenForm_FormClosed;
			Load += SplashScreenForm_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private KryptonProgressBar progressBarSplash;
    private Label labelTitle;
    private Label labelVersion;
		private ToolTip toolTip;
		private KryptonManager kryptonManager;
	}
}