// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using System.ComponentModel;
using System.Windows.Forms;

namespace Planetoid_DB;

/// <summary>A form that displays a splash screen during application startup.</summary>
/// <remarks>This form is used to show a splash screen while the application is loading data.</remarks>
partial class SplashScreenForm
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(SplashScreenForm));
		progressBarSplash = new KryptonProgressBar();
		contextMenuStripCopyToClipboard = new ContextMenuStrip(components);
		toolStripMenuItemCopyToClipboard = new ToolStripMenuItem();
		labelVersion = new Label();
		toolTip = new ToolTip(components);
		kryptonManager = new KryptonManager(components);
		contextMenuStripCopyToClipboard.SuspendLayout();
		SuspendLayout();
		// 
		// progressBarSplash
		// 
		progressBarSplash.AccessibleDescription = "Loads the data";
		progressBarSplash.AccessibleName = "Progress Bar";
		progressBarSplash.AccessibleRole = AccessibleRole.ProgressBar;
		progressBarSplash.Dock = DockStyle.Bottom;
		progressBarSplash.Location = new Point(0, 331);
		progressBarSplash.Name = "progressBarSplash";
		progressBarSplash.Size = new Size(493, 23);
		progressBarSplash.Step = 1;
		progressBarSplash.TabIndex = 3;
		progressBarSplash.Text = "Loading data...";
		progressBarSplash.TextBackdropColor = Color.Empty;
		progressBarSplash.TextShadowColor = Color.Empty;
		toolTip.SetToolTip(progressBarSplash, "Loads the data");
		progressBarSplash.Values.Text = "Loading data...";
		// 
		// contextMenuStripCopyToClipboard
		// 
		contextMenuStripCopyToClipboard.AccessibleDescription = "Shows the context menu for copying database information to the clipboard";
		contextMenuStripCopyToClipboard.AccessibleName = "Context menu for copying database information to the clipboard";
		contextMenuStripCopyToClipboard.AccessibleRole = AccessibleRole.MenuPopup;
		contextMenuStripCopyToClipboard.AllowClickThrough = true;
		contextMenuStripCopyToClipboard.Font = new Font("Segoe UI", 9F);
		contextMenuStripCopyToClipboard.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCopyToClipboard });
		contextMenuStripCopyToClipboard.Name = "contextMenuStrip";
		contextMenuStripCopyToClipboard.Size = new Size(214, 48);
		contextMenuStripCopyToClipboard.TabStop = true;
		contextMenuStripCopyToClipboard.Text = "Copy to clipboard";
		toolTip.SetToolTip(contextMenuStripCopyToClipboard, "Context Menu for copying to clipboard");
		// 
		// toolStripMenuItemCopyToClipboard
		// 
		toolStripMenuItemCopyToClipboard.AccessibleDescription = "Copies the text/value to the clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleName = "Copy to clipboard";
		toolStripMenuItemCopyToClipboard.AccessibleRole = AccessibleRole.MenuItem;
		toolStripMenuItemCopyToClipboard.AutoToolTip = true;
		toolStripMenuItemCopyToClipboard.Image = Resources.FatcowIcons16px.fatcow_page_copy_16px;
		toolStripMenuItemCopyToClipboard.Name = "toolStripMenuItemCopyToClipboard";
		toolStripMenuItemCopyToClipboard.ShortcutKeyDisplayString = "Strg+C";
		toolStripMenuItemCopyToClipboard.ShortcutKeys = Keys.Control | Keys.C;
		toolStripMenuItemCopyToClipboard.Size = new Size(213, 22);
		toolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
		toolStripMenuItemCopyToClipboard.Click += CopyToClipboard_DoubleClick;
		// 
		// labelVersion
		// 
		labelVersion.AccessibleDescription = "Shows the version number";
		labelVersion.AccessibleName = "Version";
		labelVersion.AccessibleRole = AccessibleRole.Text;
		labelVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		labelVersion.BackColor = Color.Transparent;
		labelVersion.ContextMenuStrip = contextMenuStripCopyToClipboard;
		labelVersion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
		labelVersion.ForeColor = Color.White;
		labelVersion.Location = new Point(136, 204);
		labelVersion.Margin = new Padding(0, 0, 0, 0);
		labelVersion.Name = "labelVersion";
		labelVersion.Size = new Size(221, 25);
		labelVersion.TabIndex = 1;
		labelVersion.Text = "Version: X.X.X.X";
		labelVersion.TextAlign = ContentAlignment.MiddleCenter;
		toolTip.SetToolTip(labelVersion, "Shows the version number");
		labelVersion.DoubleClick += CopyToClipboard_DoubleClick;
		labelVersion.MouseDown += Control_MouseDown;
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
		ClientSize = new Size(493, 354);
		Controls.Add(labelVersion);
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
		toolTip.SetToolTip(this, "Splash screen");
		TopMost = true;
		Load += SplashScreenForm_Load;
		contextMenuStripCopyToClipboard.ResumeLayout(false);
		ResumeLayout(false);
	}

	#endregion

	private KryptonProgressBar progressBarSplash;
    private Label labelVersion;
	private ToolTip toolTip;
	private KryptonManager kryptonManager;
	private ContextMenuStrip contextMenuStripCopyToClipboard;
	private ToolStripMenuItem toolStripMenuItemCopyToClipboard;
}