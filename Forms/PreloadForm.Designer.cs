using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class PreloadForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(PreloadForm));
			kryptonCommandLinkButtonExit = new KryptonCommandLinkButton();
			kryptonCommandLinkButtonLoadInternalDemoData = new KryptonCommandLinkButton();
			kryptonCommandLinkButtonDownloadMprcorbDat = new KryptonCommandLinkButton();
			kryptonCommandLinkButtonOpenLocalFile = new KryptonCommandLinkButton();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			panel = new KryptonPanel();
			openFileDialog = new OpenFileDialog();
			kryptonManager = new KryptonManager(components);
			statusStrip.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			SuspendLayout();
			// 
			// kryptonCommandLinkButtonExit
			// 
			kryptonCommandLinkButtonExit.AccessibleDescription = "Cancels And quits the application";
			kryptonCommandLinkButtonExit.AccessibleName = "Quit the application";
			kryptonCommandLinkButtonExit.AccessibleRole = AccessibleRole.PushButton;
			kryptonCommandLinkButtonExit.CommandLinkTextValues.Description = "Cancel and quit the application";
			kryptonCommandLinkButtonExit.CommandLinkTextValues.Heading = "Quit the application";
			kryptonCommandLinkButtonExit.DialogResult = DialogResult.Cancel;
			kryptonCommandLinkButtonExit.Location = new Point(0, 210);
			kryptonCommandLinkButtonExit.Name = "kryptonCommandLinkButtonExit";
			kryptonCommandLinkButtonExit.OverrideFocus.Border.Draw = InheritBool.True;
			kryptonCommandLinkButtonExit.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
			kryptonCommandLinkButtonExit.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
			kryptonCommandLinkButtonExit.Size = new Size(334, 60);
			kryptonCommandLinkButtonExit.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonExit.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
			kryptonCommandLinkButtonExit.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonExit.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
			kryptonCommandLinkButtonExit.TabIndex = 3;
			kryptonCommandLinkButtonExit.ToolTipValues.Description = "Cancels And quits the application";
			kryptonCommandLinkButtonExit.ToolTipValues.EnableToolTips = true;
			kryptonCommandLinkButtonExit.ToolTipValues.Heading = "Quit the application";
			kryptonCommandLinkButtonExit.Enter += Control_Enter;
			kryptonCommandLinkButtonExit.Leave += Control_Leave;
			kryptonCommandLinkButtonExit.MouseEnter += Control_Enter;
			kryptonCommandLinkButtonExit.MouseLeave += Control_Leave;
			// 
			// kryptonCommandLinkButtonLoadInternalDemoData
			// 
			kryptonCommandLinkButtonLoadInternalDemoData.AccessibleDescription = "Loads internal demo data";
			kryptonCommandLinkButtonLoadInternalDemoData.AccessibleName = "Load internal demo data";
			kryptonCommandLinkButtonLoadInternalDemoData.AccessibleRole = AccessibleRole.PushButton;
			kryptonCommandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Description = "The internal demo data contains 10'000 minor planets";
			kryptonCommandLinkButtonLoadInternalDemoData.CommandLinkTextValues.Heading = "Load internal demo data";
			kryptonCommandLinkButtonLoadInternalDemoData.Location = new Point(0, 144);
			kryptonCommandLinkButtonLoadInternalDemoData.Name = "kryptonCommandLinkButtonLoadInternalDemoData";
			kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.Draw = InheritBool.True;
			kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
			kryptonCommandLinkButtonLoadInternalDemoData.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
			kryptonCommandLinkButtonLoadInternalDemoData.Size = new Size(334, 60);
			kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
			kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonLoadInternalDemoData.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
			kryptonCommandLinkButtonLoadInternalDemoData.TabIndex = 2;
			kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.Description = "Loads internal demo data";
			kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.EnableToolTips = true;
			kryptonCommandLinkButtonLoadInternalDemoData.ToolTipValues.Heading = "Load internal demo data";
			kryptonCommandLinkButtonLoadInternalDemoData.Click += KryptonCommandLinkButtonLoadInternalDemoData_Click;
			kryptonCommandLinkButtonLoadInternalDemoData.Enter += Control_Enter;
			kryptonCommandLinkButtonLoadInternalDemoData.Leave += Control_Leave;
			kryptonCommandLinkButtonLoadInternalDemoData.MouseEnter += Control_Enter;
			kryptonCommandLinkButtonLoadInternalDemoData.MouseLeave += Control_Leave;
			// 
			// kryptonCommandLinkButtonDownloadMprcorbDat
			// 
			kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleDescription = "Downloads MPCORB.DAT from the IAU Minor Planet Center";
			kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleName = "Download MPCORB.DAT";
			kryptonCommandLinkButtonDownloadMprcorbDat.AccessibleRole = AccessibleRole.PushButton;
			kryptonCommandLinkButtonDownloadMprcorbDat.CommandLinkTextValues.Description = "Download MPCORB.DAT from the IAU Minor Planet Center";
			kryptonCommandLinkButtonDownloadMprcorbDat.CommandLinkTextValues.Heading = "Download MPCORB.DAT";
			kryptonCommandLinkButtonDownloadMprcorbDat.Location = new Point(0, 78);
			kryptonCommandLinkButtonDownloadMprcorbDat.Name = "kryptonCommandLinkButtonDownloadMprcorbDat";
			kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.Draw = InheritBool.True;
			kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
			kryptonCommandLinkButtonDownloadMprcorbDat.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
			kryptonCommandLinkButtonDownloadMprcorbDat.Size = new Size(334, 60);
			kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
			kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonDownloadMprcorbDat.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
			kryptonCommandLinkButtonDownloadMprcorbDat.TabIndex = 1;
			kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.Description = "Downloads MPCORB.DAT from the IAU Minor Planet Center";
			kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.EnableToolTips = true;
			kryptonCommandLinkButtonDownloadMprcorbDat.ToolTipValues.Heading = "Download MPCORB.DAT";
			kryptonCommandLinkButtonDownloadMprcorbDat.Click += KryptonCommandLinkButtonDownloadMprcorbDat_Click;
			kryptonCommandLinkButtonDownloadMprcorbDat.Enter += Control_Enter;
			kryptonCommandLinkButtonDownloadMprcorbDat.Leave += Control_Leave;
			kryptonCommandLinkButtonDownloadMprcorbDat.MouseEnter += Control_Enter;
			kryptonCommandLinkButtonDownloadMprcorbDat.MouseLeave += Control_Leave;
			// 
			// kryptonCommandLinkButtonOpenLocalFile
			// 
			kryptonCommandLinkButtonOpenLocalFile.AccessibleDescription = "Opens a local MPCORB.DAT file from hard drive";
			kryptonCommandLinkButtonOpenLocalFile.AccessibleName = "Open a local MPCORB.DAT file";
			kryptonCommandLinkButtonOpenLocalFile.AccessibleRole = AccessibleRole.PushButton;
			kryptonCommandLinkButtonOpenLocalFile.CommandLinkTextValues.Description = "Open a local MPCORB.DAT file from hard drive";
			kryptonCommandLinkButtonOpenLocalFile.CommandLinkTextValues.Heading = "Open a local MPCORB.DAT file";
			kryptonCommandLinkButtonOpenLocalFile.Location = new Point(0, 12);
			kryptonCommandLinkButtonOpenLocalFile.Name = "kryptonCommandLinkButtonOpenLocalFile";
			kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.Draw = InheritBool.True;
			kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.DrawBorders = PaletteDrawBorders.Top | PaletteDrawBorders.Bottom | PaletteDrawBorders.Left | PaletteDrawBorders.Right;
			kryptonCommandLinkButtonOpenLocalFile.OverrideFocus.Border.GraphicsHint = PaletteGraphicsHint.AntiAlias;
			kryptonCommandLinkButtonOpenLocalFile.Size = new Size(334, 60);
			kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.LongText.TextV = PaletteRelativeAlign.Far;
			kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Near;
			kryptonCommandLinkButtonOpenLocalFile.StateCommon.Content.ShortText.TextV = PaletteRelativeAlign.Center;
			kryptonCommandLinkButtonOpenLocalFile.TabIndex = 0;
			kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.Description = "Opens a local MPCORB.DAT file from hard drive";
			kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.EnableToolTips = true;
			kryptonCommandLinkButtonOpenLocalFile.ToolTipValues.Heading = "Open a local MPCORB.DAT file";
			kryptonCommandLinkButtonOpenLocalFile.Click += KryptonCommandLinkButtonOpenLocalFile_Click;
			kryptonCommandLinkButtonOpenLocalFile.Enter += Control_Enter;
			kryptonCommandLinkButtonOpenLocalFile.Leave += Control_Leave;
			kryptonCommandLinkButtonOpenLocalFile.MouseEnter += Control_Enter;
			kryptonCommandLinkButtonOpenLocalFile.MouseLeave += Control_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 278);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(334, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 1;
			statusStrip.Text = "status bar";
			// 
			// labelInformation
			// 
			labelInformation.AccessibleDescription = "Shows some information";
			labelInformation.AccessibleName = "Shows some information";
			labelInformation.AccessibleRole = AccessibleRole.StaticText;
			labelInformation.AutoToolTip = true;
			labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelInformation.Margin = new Padding(5, 3, 0, 2);
			labelInformation.Name = "labelInformation";
			labelInformation.Size = new Size(144, 17);
			labelInformation.Text = "some information here";
			labelInformation.ToolTipText = "Shows some information";
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(kryptonCommandLinkButtonExit);
			panel.Controls.Add(kryptonCommandLinkButtonLoadInternalDemoData);
			panel.Controls.Add(kryptonCommandLinkButtonDownloadMprcorbDat);
			panel.Controls.Add(kryptonCommandLinkButtonOpenLocalFile);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(334, 278);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// openFileDialog
			// 
			openFileDialog.DefaultExt = "dat";
			openFileDialog.FileName = "mpcorb.dat";
			openFileDialog.Filter = "DAT files|*.dat|all files|*.*";
			openFileDialog.Title = "Open MPCORB.DAT";
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// PreloadForm
			// 
			AccessibleDescription = "Handles of the file MPCORB.DAT ist missing";
			AccessibleName = "Preloader";
			AccessibleRole = AccessibleRole.Window;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = kryptonCommandLinkButtonExit;
			ClientSize = new Size(334, 300);
			ControlBox = false;
			Controls.Add(panel);
			Controls.Add(statusStrip);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "PreloadForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Planetoid-DB Preloader";
			Load += PreloadForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonPanel panel;
		private KryptonCommandLinkButton kryptonCommandLinkButtonOpenLocalFile;
		private KryptonCommandLinkButton kryptonCommandLinkButtonExit;
		private KryptonCommandLinkButton kryptonCommandLinkButtonLoadInternalDemoData;
		private KryptonCommandLinkButton kryptonCommandLinkButtonDownloadMprcorbDat;
		private OpenFileDialog openFileDialog;
		private KryptonManager kryptonManager;
	}
}