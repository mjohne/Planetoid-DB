using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
    partial class PrintDataSheetForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(PrintDataSheetForm));
			checkedListBoxOrbitalElements = new KryptonCheckedListBox();
			panel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			toolStripContainer = new ToolStripContainer();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripButtonPrintDataSheet = new ToolStripButton();
			toolStripButtonCancelPrint = new ToolStripButton();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.TopToolStripPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			kryptonToolStripIcons.SuspendLayout();
			SuspendLayout();
			// 
			// checkedListBoxOrbitalElements
			// 
			checkedListBoxOrbitalElements.AccessibleDescription = "Checks some orbital elements to print on a data sheet";
			checkedListBoxOrbitalElements.AccessibleName = "Check orbital elements";
			checkedListBoxOrbitalElements.AccessibleRole = AccessibleRole.List;
			checkedListBoxOrbitalElements.BackStyle = PaletteBackStyle.InputControlRibbon;
			checkedListBoxOrbitalElements.CheckOnClick = true;
			checkedListBoxOrbitalElements.Dock = DockStyle.Fill;
			checkedListBoxOrbitalElements.FormattingEnabled = true;
			checkedListBoxOrbitalElements.HorizontalScrollbar = true;
			checkedListBoxOrbitalElements.Items.AddRange(new object[] { "Index No.", "Readable designation", "Epoch (in packed form, .0 TT)", "Mean anomaly at the epoch (degrees)", "Argument of perihelion, J2000.0 (degrees)", "Longitude of the ascending node, J2000.0", "Inclination to the ecliptic, J2000.0 (degrees)", "Orbital eccentricity", "Mean daily motion (degrees per day)", "Semimajor axis (AU)", "Absolute magnitude, H (mag)", "Slope parameter, G", "Reference", "Number of oppositions", "Number of observations", "Observation span", "r.m.s. residual (arcseconds)", "Computer name", "4-hexdigit flags", "Date of last observation (YYYMMDD)", "Linear eccentricity (AU)", "Semi-minor axis (AU)", "Major axis (AU)", "Minor axis (AU)", "Eccentric anomaly (degrees)", "True anomaly (degrees)", "Perihelion distance (AU)", "Aphelion distance (AU)", "Longitude of Descending node (degrees)", "Argument of aphelion (degrees)", "Focal parameter (AU)", "Semi-latus rectum (AU)", "Latus rectum (AU)", "Orbital period (years)", "Orbital area (AU²)", "Orbital perimeter (AU)", "Semi-mean axis (AU)", "Mean axis (AU)", "Standard gravitational parameter (AU³/a²)" });
			checkedListBoxOrbitalElements.Location = new Point(0, 0);
			checkedListBoxOrbitalElements.Margin = new Padding(4, 3, 4, 3);
			checkedListBoxOrbitalElements.Name = "checkedListBoxOrbitalElements";
			checkedListBoxOrbitalElements.Size = new Size(357, 308);
			checkedListBoxOrbitalElements.TabIndex = 0;
			checkedListBoxOrbitalElements.ToolTipValues.Description = "Checks some orbital elements to print on a data sheet.";
			checkedListBoxOrbitalElements.ToolTipValues.EnableToolTips = true;
			checkedListBoxOrbitalElements.ToolTipValues.Heading = "Check orbital elements";
			checkedListBoxOrbitalElements.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
			checkedListBoxOrbitalElements.Enter += Control_Enter;
			checkedListBoxOrbitalElements.Leave += Control_Leave;
			checkedListBoxOrbitalElements.MouseEnter += Control_Enter;
			checkedListBoxOrbitalElements.MouseLeave += Control_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "Pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(checkedListBoxOrbitalElements);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(357, 308);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(357, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 3;
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
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// toolStripContainer
			// 
			toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
			toolStripContainer.AccessibleName = "Container to arrange the toolbars";
			toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			toolStripContainer.BottomToolStripPanel.Controls.Add(statusStrip);
			// 
			// toolStripContainer.ContentPanel
			// 
			toolStripContainer.ContentPanel.Controls.Add(panel);
			toolStripContainer.ContentPanel.Size = new Size(357, 308);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(357, 355);
			toolStripContainer.TabIndex = 2;
			toolStripContainer.Text = "toolStripContainer";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			toolStripContainer.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);
			// 
			// kryptonToolStripIcons
			// 
			kryptonToolStripIcons.AccessibleDescription = "Toolbar of printing values of orbital elements";
			kryptonToolStripIcons.AccessibleName = "Toolbar of printing values of orbital elements";
			kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
			kryptonToolStripIcons.Dock = DockStyle.None;
			kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonPrintDataSheet, toolStripButtonCancelPrint });
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(357, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.TabStop = true;
			kryptonToolStripIcons.Text = "Toolbar of printing values of orbital elements";
			kryptonToolStripIcons.MouseEnter += Control_Enter;
			kryptonToolStripIcons.MouseLeave += Control_Leave;
			// 
			// toolStripButtonPrintDataSheet
			// 
			toolStripButtonPrintDataSheet.AccessibleDescription = "Prints a data sheet with some orbit elements";
			toolStripButtonPrintDataSheet.AccessibleName = "Print data sheet";
			toolStripButtonPrintDataSheet.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonPrintDataSheet.Image = FatcowIcons16px.fatcow_printer_16px;
			toolStripButtonPrintDataSheet.ImageTransparentColor = Color.Magenta;
			toolStripButtonPrintDataSheet.Name = "toolStripButtonPrintDataSheet";
			toolStripButtonPrintDataSheet.Size = new Size(103, 22);
			toolStripButtonPrintDataSheet.Text = "&Print the sheet";
			toolStripButtonPrintDataSheet.Click += ButtonPrintDataSheet_Click;
			// 
			// toolStripButtonCancelPrint
			// 
			toolStripButtonCancelPrint.AccessibleDescription = "Cancels the print";
			toolStripButtonCancelPrint.AccessibleName = "Cancel print";
			toolStripButtonCancelPrint.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonCancelPrint.Image = FatcowIcons16px.fatcow_cancel_16px;
			toolStripButtonCancelPrint.ImageTransparentColor = Color.Magenta;
			toolStripButtonCancelPrint.Name = "toolStripButtonCancelPrint";
			toolStripButtonCancelPrint.Size = new Size(91, 22);
			toolStripButtonCancelPrint.Text = "&Cancel print";
			toolStripButtonCancelPrint.Click += ButtonCancelPrint_Click;
			// 
			// PrintDataSheetForm
			// 
			AccessibleDescription = "Prints a data sheet with some orbit elements";
			AccessibleName = "Print data sheet";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(357, 355);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "PrintDataSheetForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Print data sheet";
			Load += PrintDataSheetForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			toolStripContainer.TopToolStripPanel.PerformLayout();
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			kryptonToolStripIcons.ResumeLayout(false);
			kryptonToolStripIcons.PerformLayout();
			ResumeLayout(false);

		}

		#endregion

		private KryptonCheckedListBox checkedListBoxOrbitalElements;
        private KryptonPanel panel;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
		private ToolStripContainer toolStripContainer;
		private KryptonToolStrip kryptonToolStripIcons;
		private ToolStripButton toolStripButtonPrintDataSheet;
		private ToolStripButton toolStripButtonCancelPrint;
	}
}