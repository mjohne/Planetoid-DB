using System.ComponentModel;

using Krypton.Toolkit;
using Krypton.Toolkit.Suite.Extended.Navigator;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
    partial class SettingsForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingsForm));
			tabControlSettings = new TabControl();
			tabPageGeneral = new TabPage();
			checkBox10 = new KryptonCheckBox();
			checkBox9 = new KryptonCheckBox();
			checkBox8 = new KryptonCheckBox();
			tabPageNavigator = new TabPage();
			radioButton9 = new KryptonRadioButton();
			numericUpDown1 = new NumericUpDown();
			radioButton4 = new KryptonRadioButton();
			radioButton3 = new KryptonRadioButton();
			radioButton2 = new KryptonRadioButton();
			radioButton1 = new KryptonRadioButton();
			tabPageUpdate = new TabPage();
			groupBox1 = new KryptonGroupBox();
			groupBox2 = new KryptonGroupBox();
			tabPageLookAndFeel = new TabPage();
			kryptonRadioButton1 = new KryptonRadioButton();
			groupBox4 = new KryptonGroupBox();
			checkBox6 = new KryptonCheckBox();
			checkBox4 = new KryptonCheckBox();
			checkBox5 = new KryptonCheckBox();
			checkBox1 = new KryptonCheckBox();
			checkBox2 = new KryptonCheckBox();
			checkBox3 = new KryptonCheckBox();
			radioButton10 = new KryptonRadioButton();
			radioButton11 = new KryptonRadioButton();
			radioButton12 = new KryptonRadioButton();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripButtonSave = new ToolStripButton();
			toolStripButtonCancel = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			toolStripButtonLoadDefaultSettings = new ToolStripButton();
			toolStripContainerSettings = new ToolStripContainer();
			panel = new KryptonPanel();
			kryptonStatusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			tabControlSettings.SuspendLayout();
			tabPageGeneral.SuspendLayout();
			tabPageNavigator.SuspendLayout();
			((ISupportInitialize)numericUpDown1).BeginInit();
			tabPageUpdate.SuspendLayout();
			((ISupportInitialize)groupBox1).BeginInit();
			((ISupportInitialize)groupBox1.Panel).BeginInit();
			((ISupportInitialize)groupBox2).BeginInit();
			((ISupportInitialize)groupBox2.Panel).BeginInit();
			tabPageLookAndFeel.SuspendLayout();
			((ISupportInitialize)groupBox4).BeginInit();
			((ISupportInitialize)groupBox4.Panel).BeginInit();
			kryptonToolStripIcons.SuspendLayout();
			toolStripContainerSettings.ContentPanel.SuspendLayout();
			toolStripContainerSettings.TopToolStripPanel.SuspendLayout();
			toolStripContainerSettings.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			kryptonStatusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// tabControlSettings
			// 
			tabControlSettings.Controls.Add(tabPageGeneral);
			tabControlSettings.Controls.Add(tabPageNavigator);
			tabControlSettings.Controls.Add(tabPageUpdate);
			tabControlSettings.Controls.Add(tabPageLookAndFeel);
			tabControlSettings.Dock = DockStyle.Fill;
			tabControlSettings.Location = new Point(0, 0);
			tabControlSettings.Margin = new Padding(4, 3, 4, 3);
			tabControlSettings.Name = "tabControlSettings";
			tabControlSettings.SelectedIndex = 0;
			tabControlSettings.ShowToolTips = true;
			tabControlSettings.Size = new Size(503, 263);
			tabControlSettings.TabIndex = 0;
			// 
			// tabPageGeneral
			// 
			tabPageGeneral.Controls.Add(checkBox10);
			tabPageGeneral.Controls.Add(checkBox9);
			tabPageGeneral.Controls.Add(checkBox8);
			tabPageGeneral.Location = new Point(4, 24);
			tabPageGeneral.Margin = new Padding(4, 3, 4, 3);
			tabPageGeneral.Name = "tabPageGeneral";
			tabPageGeneral.Padding = new Padding(4, 3, 4, 3);
			tabPageGeneral.Size = new Size(495, 235);
			tabPageGeneral.TabIndex = 3;
			tabPageGeneral.Text = "General";
			tabPageGeneral.UseVisualStyleBackColor = true;
			// 
			// checkBox10
			// 
			checkBox10.Location = new Point(9, 65);
			checkBox10.Margin = new Padding(4, 3, 4, 3);
			checkBox10.Name = "checkBox10";
			checkBox10.Size = new Size(215, 23);
			checkBox10.TabIndex = 2;
			checkBox10.ToolTipValues.EnableToolTips = true;
			checkBox10.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkBox10.Values.Text = "Enable linking to terminology";
			// 
			// checkBox9
			// 
			checkBox9.Location = new Point(9, 36);
			checkBox9.Margin = new Padding(4, 3, 4, 3);
			checkBox9.Name = "checkBox9";
			checkBox9.Size = new Size(335, 23);
			checkBox9.TabIndex = 1;
			checkBox9.ToolTipValues.EnableToolTips = true;
			checkBox9.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkBox9.Values.Text = "Enabled copying to clipboard by double-clicking";
			// 
			// checkBox8
			// 
			checkBox8.Location = new Point(9, 7);
			checkBox8.Margin = new Padding(4, 3, 4, 3);
			checkBox8.Name = "checkBox8";
			checkBox8.Size = new Size(146, 23);
			checkBox8.TabIndex = 0;
			checkBox8.ToolTipValues.EnableToolTips = true;
			checkBox8.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkBox8.Values.Text = "Stay always on top";
			// 
			// tabPageNavigator
			// 
			tabPageNavigator.Controls.Add(radioButton9);
			tabPageNavigator.Controls.Add(numericUpDown1);
			tabPageNavigator.Controls.Add(radioButton4);
			tabPageNavigator.Controls.Add(radioButton3);
			tabPageNavigator.Controls.Add(radioButton2);
			tabPageNavigator.Controls.Add(radioButton1);
			tabPageNavigator.Location = new Point(4, 24);
			tabPageNavigator.Margin = new Padding(4, 3, 4, 3);
			tabPageNavigator.Name = "tabPageNavigator";
			tabPageNavigator.Padding = new Padding(4, 3, 4, 3);
			tabPageNavigator.Size = new Size(495, 235);
			tabPageNavigator.TabIndex = 0;
			tabPageNavigator.Text = "Navigator";
			tabPageNavigator.UseVisualStyleBackColor = true;
			// 
			// radioButton9
			// 
			radioButton9.Location = new Point(9, 7);
			radioButton9.Margin = new Padding(4, 3, 4, 3);
			radioButton9.Name = "radioButton9";
			radioButton9.Size = new Size(204, 23);
			radioButton9.TabIndex = 5;
			radioButton9.ToolTipValues.EnableToolTips = true;
			radioButton9.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			radioButton9.Values.Text = "Start with the last used item";
			// 
			// numericUpDown1
			// 
			numericUpDown1.Location = new Point(33, 151);
			numericUpDown1.Margin = new Padding(4, 3, 4, 3);
			numericUpDown1.Name = "numericUpDown1";
			numericUpDown1.Size = new Size(118, 23);
			numericUpDown1.TabIndex = 4;
			// 
			// radioButton4
			// 
			radioButton4.Location = new Point(9, 122);
			radioButton4.Margin = new Padding(4, 3, 4, 3);
			radioButton4.Name = "radioButton4";
			radioButton4.Size = new Size(182, 23);
			radioButton4.TabIndex = 3;
			radioButton4.ToolTipValues.EnableToolTips = true;
			radioButton4.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			radioButton4.Values.Text = "Start with a specific item";
			// 
			// radioButton3
			// 
			radioButton3.Location = new Point(9, 93);
			radioButton3.Margin = new Padding(4, 3, 4, 3);
			radioButton3.Name = "radioButton3";
			radioButton3.Size = new Size(184, 23);
			radioButton3.TabIndex = 2;
			radioButton3.ToolTipValues.EnableToolTips = true;
			radioButton3.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			radioButton3.Values.Text = "Start with a random item";
			// 
			// radioButton2
			// 
			radioButton2.Location = new Point(9, 65);
			radioButton2.Margin = new Padding(4, 3, 4, 3);
			radioButton2.Name = "radioButton2";
			radioButton2.Size = new Size(169, 23);
			radioButton2.TabIndex = 1;
			radioButton2.ToolTipValues.EnableToolTips = true;
			radioButton2.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			radioButton2.Values.Text = "Start with the last item";
			// 
			// radioButton1
			// 
			radioButton1.Location = new Point(9, 36);
			radioButton1.Margin = new Padding(4, 3, 4, 3);
			radioButton1.Name = "radioButton1";
			radioButton1.Size = new Size(172, 23);
			radioButton1.TabIndex = 0;
			radioButton1.ToolTipValues.EnableToolTips = true;
			radioButton1.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			radioButton1.Values.Text = "Start with the first item";
			// 
			// tabPageUpdate
			// 
			tabPageUpdate.Controls.Add(groupBox1);
			tabPageUpdate.Controls.Add(groupBox2);
			tabPageUpdate.Location = new Point(4, 24);
			tabPageUpdate.Margin = new Padding(4, 3, 4, 3);
			tabPageUpdate.Name = "tabPageUpdate";
			tabPageUpdate.Padding = new Padding(4, 3, 4, 3);
			tabPageUpdate.Size = new Size(495, 235);
			tabPageUpdate.TabIndex = 1;
			tabPageUpdate.Text = "MPCORB.DAT Update";
			tabPageUpdate.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			groupBox1.Location = new Point(9, 7);
			groupBox1.Margin = new Padding(4, 3, 4, 3);
			groupBox1.Size = new Size(475, 115);
			groupBox1.TabIndex = 6;
			groupBox1.Values.Heading = "Startup update";
			// 
			// groupBox2
			// 
			groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			groupBox2.Location = new Point(9, 129);
			groupBox2.Margin = new Padding(4, 3, 4, 3);
			groupBox2.Size = new Size(475, 117);
			groupBox2.TabIndex = 7;
			groupBox2.Values.Heading = "Background update";
			// 
			// tabPageLookAndFeel
			// 
			tabPageLookAndFeel.Controls.Add(kryptonRadioButton1);
			tabPageLookAndFeel.Controls.Add(groupBox4);
			tabPageLookAndFeel.Location = new Point(4, 24);
			tabPageLookAndFeel.Margin = new Padding(4, 3, 4, 3);
			tabPageLookAndFeel.Name = "tabPageLookAndFeel";
			tabPageLookAndFeel.Padding = new Padding(4, 3, 4, 3);
			tabPageLookAndFeel.Size = new Size(495, 235);
			tabPageLookAndFeel.TabIndex = 2;
			tabPageLookAndFeel.Text = "Look and feel";
			tabPageLookAndFeel.UseVisualStyleBackColor = true;
			// 
			// kryptonRadioButton1
			// 
			kryptonRadioButton1.Location = new Point(324, 7);
			kryptonRadioButton1.Margin = new Padding(4, 3, 4, 3);
			kryptonRadioButton1.Name = "kryptonRadioButton1";
			kryptonRadioButton1.Size = new Size(50, 23);
			kryptonRadioButton1.TabIndex = 6;
			kryptonRadioButton1.Values.Text = "test";
			// 
			// groupBox4
			// 
			groupBox4.Location = new Point(390, 179);
			groupBox4.Margin = new Padding(4, 3, 4, 3);
			groupBox4.Size = new Size(153, 158);
			groupBox4.TabIndex = 3;
			groupBox4.Values.Heading = "Icons";
			// 
			// checkBox6
			// 
			checkBox6.Location = new Point(6, 72);
			checkBox6.Name = "checkBox6";
			checkBox6.Size = new Size(262, 19);
			checkBox6.TabIndex = 5;
			checkBox6.Values.Text = "Ask for restart after the MPCORB.DAT update";
			// 
			// checkBox4
			// 
			checkBox4.Location = new Point(6, 22);
			checkBox4.Name = "checkBox4";
			checkBox4.Size = new Size(337, 19);
			checkBox4.TabIndex = 3;
			checkBox4.Values.Text = "Check the MPCORB.DAT update every hour in background ";
			// 
			// checkBox5
			// 
			checkBox5.Location = new Point(6, 47);
			checkBox5.Name = "checkBox5";
			checkBox5.Size = new Size(371, 19);
			checkBox5.TabIndex = 4;
			checkBox5.Values.Text = "Download automatically the MPCORB.DAT update in background";
			// 
			// checkBox1
			// 
			checkBox1.Location = new Point(6, 22);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(252, 19);
			checkBox1.TabIndex = 0;
			checkBox1.Values.Text = "Check the MPCORB.DAT update on startup";
			// 
			// checkBox2
			// 
			checkBox2.Location = new Point(6, 47);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(348, 19);
			checkBox2.TabIndex = 1;
			checkBox2.Values.Text = "Download automatically the MPCORB.DAT update on startup";
			// 
			// checkBox3
			// 
			checkBox3.Location = new Point(6, 72);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(262, 19);
			checkBox3.TabIndex = 2;
			checkBox3.Values.Text = "Ask for restart after the MPCORB.DAT update";
			// 
			// radioButton10
			// 
			radioButton10.Location = new Point(1, 1);
			radioButton10.Name = "radioButton10";
			radioButton10.Size = new Size(89, 19);
			radioButton10.TabIndex = 3;
			radioButton10.Values.Text = "Fugue icons";
			// 
			// radioButton11
			// 
			radioButton11.Location = new Point(6, 47);
			radioButton11.Name = "radioButton11";
			radioButton11.Size = new Size(96, 19);
			radioButton11.TabIndex = 2;
			radioButton11.Values.Text = "FatCow icons";
			// 
			// radioButton12
			// 
			radioButton12.Location = new Point(6, 22);
			radioButton12.Name = "radioButton12";
			radioButton12.Size = new Size(74, 19);
			radioButton12.TabIndex = 1;
			radioButton12.Values.Text = "Silk icons";
			// 
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.AccessibleDescription = "Toolbar of main functions";
			kryptonToolStripIcons.AccessibleName = "Toolbar of main functions";
			kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonSave, toolStripButtonCancel, toolStripSeparator1, toolStripButtonLoadDefaultSettings });
			kryptonToolStripIcons.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(503, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.TabStop = true;
			kryptonToolStripIcons.Text = "Toolbar of main functions";
			// 
			// toolStripButtonSave
			// 
			toolStripButtonSave.AccessibleDescription = "Saves the settings";
			toolStripButtonSave.AccessibleName = "Save";
			toolStripButtonSave.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonSave.DoubleClickEnabled = true;
			toolStripButtonSave.Image = FatcowIcons16px.fatcow_diskette_16px;
			toolStripButtonSave.ImageTransparentColor = Color.Magenta;
			toolStripButtonSave.Name = "toolStripButtonSave";
			toolStripButtonSave.Size = new Size(51, 22);
			toolStripButtonSave.Text = "Save";
			toolStripButtonSave.ToolTipText = "Save the settings";
			// 
			// toolStripButtonCancel
			// 
			toolStripButtonCancel.AccessibleDescription = "Cancels the settings";
			toolStripButtonCancel.AccessibleName = "Cancel";
			toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonCancel.DoubleClickEnabled = true;
			toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
			toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
			toolStripButtonCancel.Name = "toolStripButtonCancel";
			toolStripButtonCancel.Size = new Size(63, 22);
			toolStripButtonCancel.Text = "Cancel";
			toolStripButtonCancel.ToolTipText = "Cancel the settings";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 25);
			// 
			// toolStripButtonLoadDefaultSettings
			// 
			toolStripButtonLoadDefaultSettings.AccessibleDescription = "Loads the default settings";
			toolStripButtonLoadDefaultSettings.AccessibleName = "Default settings";
			toolStripButtonLoadDefaultSettings.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonLoadDefaultSettings.DoubleClickEnabled = true;
			toolStripButtonLoadDefaultSettings.Image = FatcowIcons16px.fatcow_cog_16px;
			toolStripButtonLoadDefaultSettings.ImageTransparentColor = Color.Magenta;
			toolStripButtonLoadDefaultSettings.Name = "toolStripButtonLoadDefaultSettings";
			toolStripButtonLoadDefaultSettings.Size = new Size(109, 22);
			toolStripButtonLoadDefaultSettings.Text = "Default settings";
			toolStripButtonLoadDefaultSettings.ToolTipText = "Load default settings";
			// 
			// toolStripContainerSettings
			// 
			// 
			// toolStripContainerSettings.ContentPanel
			// 
			toolStripContainerSettings.ContentPanel.Controls.Add(panel);
			toolStripContainerSettings.ContentPanel.Margin = new Padding(4, 3, 4, 3);
			toolStripContainerSettings.ContentPanel.Size = new Size(503, 263);
			toolStripContainerSettings.Dock = DockStyle.Fill;
			toolStripContainerSettings.Location = new Point(0, 0);
			toolStripContainerSettings.Margin = new Padding(4, 3, 4, 3);
			toolStripContainerSettings.Name = "toolStripContainerSettings";
			toolStripContainerSettings.Size = new Size(503, 288);
			toolStripContainerSettings.TabIndex = 7;
			toolStripContainerSettings.Text = "Settings";
			// 
			// toolStripContainerSettings.TopToolStripPanel
			// 
			toolStripContainerSettings.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
			// 
			// panel
			// 
			panel.Controls.Add(tabControlSettings);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(503, 263);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// kryptonStatusStrip
			// 
			kryptonStatusStrip.AccessibleDescription = "Shows some information";
			kryptonStatusStrip.AccessibleName = "Status bar of some information";
			kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
			kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
			kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			kryptonStatusStrip.Location = new Point(0, 266);
			kryptonStatusStrip.Name = "kryptonStatusStrip";
			kryptonStatusStrip.Padding = new Padding(1, 0, 16, 0);
			kryptonStatusStrip.ProgressBars = null;
			kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			kryptonStatusStrip.Size = new Size(503, 22);
			kryptonStatusStrip.SizingGrip = false;
			kryptonStatusStrip.TabIndex = 8;
			kryptonStatusStrip.Text = "status bar";
			// 
			// labelInformation
			// 
			labelInformation.AccessibleDescription = "Show some information";
			labelInformation.AccessibleName = "Show some information";
			labelInformation.AccessibleRole = AccessibleRole.StaticText;
			labelInformation.AutoToolTip = true;
			labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelInformation.Margin = new Padding(5, 3, 0, 2);
			labelInformation.Name = "labelInformation";
			labelInformation.Size = new Size(144, 17);
			labelInformation.Text = "some information here";
			labelInformation.ToolTipText = "Show some information";
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// SettingsForm
			// 
			AccessibleDescription = "Show the settings";
			AccessibleName = "Settings";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(503, 288);
			ControlBox = false;
			Controls.Add(kryptonStatusStrip);
			Controls.Add(toolStripContainerSettings);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "SettingsForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Settings";
			Load += SettingsForm_Load;
			tabControlSettings.ResumeLayout(false);
			tabPageGeneral.ResumeLayout(false);
			tabPageGeneral.PerformLayout();
			tabPageNavigator.ResumeLayout(false);
			tabPageNavigator.PerformLayout();
			((ISupportInitialize)numericUpDown1).EndInit();
			tabPageUpdate.ResumeLayout(false);
			((ISupportInitialize)groupBox1.Panel).EndInit();
			((ISupportInitialize)groupBox1).EndInit();
			((ISupportInitialize)groupBox2.Panel).EndInit();
			((ISupportInitialize)groupBox2).EndInit();
			tabPageLookAndFeel.ResumeLayout(false);
			tabPageLookAndFeel.PerformLayout();
			((ISupportInitialize)groupBox4.Panel).EndInit();
			((ISupportInitialize)groupBox4).EndInit();
			kryptonToolStripIcons.ResumeLayout(false);
			kryptonToolStripIcons.PerformLayout();
			toolStripContainerSettings.ContentPanel.ResumeLayout(false);
			toolStripContainerSettings.TopToolStripPanel.ResumeLayout(false);
			toolStripContainerSettings.TopToolStripPanel.PerformLayout();
			toolStripContainerSettings.ResumeLayout(false);
			toolStripContainerSettings.PerformLayout();
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			kryptonStatusStrip.ResumeLayout(false);
			kryptonStatusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion
		private TabControl tabControlSettings;
        private TabPage tabPageNavigator;
        private TabPage tabPageUpdate;
        private KryptonCheckBox checkBox1;
        private KryptonCheckBox checkBox3;
        private KryptonCheckBox checkBox2;
        private KryptonRadioButton radioButton4;
        private KryptonRadioButton radioButton3;
        private KryptonRadioButton radioButton2;
        private KryptonRadioButton radioButton1;
        private NumericUpDown numericUpDown1;
        private KryptonCheckBox checkBox5;
        private KryptonCheckBox checkBox4;
        private KryptonGroupBox groupBox2;
        private KryptonGroupBox groupBox1;
        private KryptonCheckBox checkBox6;
        private TabPage tabPageLookAndFeel;
        private KryptonToolStrip kryptonToolStripIcons;
        private ToolStripButton toolStripButtonSave;
        private ToolStripButton toolStripButtonCancel;
        private ToolStripButton toolStripButtonLoadDefaultSettings;
        private ToolStripContainer toolStripContainerSettings;
        private ToolStripSeparator toolStripSeparator1;
        private KryptonGroupBox groupBox4;
        private KryptonRadioButton radioButton10;
        private KryptonRadioButton radioButton11;
        private KryptonRadioButton radioButton12;
        private TabPage tabPageGeneral;
        private KryptonCheckBox checkBox10;
        private KryptonCheckBox checkBox9;
        private KryptonCheckBox checkBox8;
        private KryptonRadioButton radioButton9;
        private KryptonPanel panel;
		private KryptonStatusStrip kryptonStatusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
		private KryptonRadioButton kryptonRadioButton1;
	}
}