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
			toolTip = new ToolTip(components);
			buttonPrintDataSheet = new KryptonButton();
			buttonCancelPrint = new KryptonButton();
			panel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// checkedListBoxOrbitalElements
			// 
			checkedListBoxOrbitalElements.AccessibleDescription = "Checks some orbital elements to print on a data sheet";
			checkedListBoxOrbitalElements.AccessibleName = "Checks orbital elements";
			checkedListBoxOrbitalElements.AccessibleRole = AccessibleRole.List;
			checkedListBoxOrbitalElements.BackStyle = PaletteBackStyle.InputControlRibbon;
			checkedListBoxOrbitalElements.CheckOnClick = true;
			checkedListBoxOrbitalElements.FormattingEnabled = true;
			checkedListBoxOrbitalElements.HorizontalScrollbar = true;
			checkedListBoxOrbitalElements.Items.AddRange(new object[] { "Index No.", "Readable designation", "Epoch (in packed form, .0 TT)", "Mean anomaly at the epoch, in degrees", "Argument of perihelion, J2000.0 (degrees)", "Longitude of the ascending node, J2000.0", "Inclination to the ecliptic, J2000.0 (degrees)", "Orbital eccentricity", "Mean daily motion (degrees per day)", "Semimajor axis (AU)", "Absolute magnitude, H", "Slope parameter, G", "Reference", "Number of oppositions", "Number of observations", "Observation span", "r.m.s. residual (\")", "Computer name", "4-hexdigit flags", "Date of last observation", "Linear eccentricity", "Semi-minor axis", "Major axis", "Minor axis", "Eccenctric anomaly", "True anomaly", "Perihelion distance", "Aphelion distance", "Longitude of Descending node", "Argument of aphelion", "Focal parameter", "Semi-latus rectum", "Latus rectum", "Period", "Orbital area", "Orbital perimeter", "Semi-mean axis", "Mean axis", "Standard gravitational parameter" });
			checkedListBoxOrbitalElements.Location = new Point(12, 12);
			checkedListBoxOrbitalElements.Name = "checkedListBoxOrbitalElements";
			checkedListBoxOrbitalElements.Size = new Size(280, 220);
			checkedListBoxOrbitalElements.TabIndex = 0;
			toolTip.SetToolTip(checkedListBoxOrbitalElements, "Checks some orbital elements to print on a data sheet");
			checkedListBoxOrbitalElements.Enter += SetStatusBar_Enter;
			checkedListBoxOrbitalElements.Leave += ClearStatusBar_Leave;
			checkedListBoxOrbitalElements.MouseEnter += SetStatusBar_Enter;
			checkedListBoxOrbitalElements.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonPrintDataSheet
			// 
			buttonPrintDataSheet.AccessibleDescription = "Prints a data sheet with some orbit elements";
			buttonPrintDataSheet.AccessibleName = "Print data sheet";
			buttonPrintDataSheet.AccessibleRole = AccessibleRole.PushButton;
			buttonPrintDataSheet.Location = new Point(12, 238);
			buttonPrintDataSheet.Name = "buttonPrintDataSheet";
			buttonPrintDataSheet.Size = new Size(132, 36);
			buttonPrintDataSheet.TabIndex = 1;
			toolTip.SetToolTip(buttonPrintDataSheet, "Print a data sheet with some orbit elements");
			buttonPrintDataSheet.Values.DropDownArrowColor = Color.Empty;
			buttonPrintDataSheet.Values.Image = FatcowIcons16px.fatcow_printer_16px;
			buttonPrintDataSheet.Values.Text = "&Print the sheet";
			buttonPrintDataSheet.Click += ButtonPrintDataSheet_Click;
			buttonPrintDataSheet.Enter += SetStatusBar_Enter;
			buttonPrintDataSheet.Leave += ClearStatusBar_Leave;
			buttonPrintDataSheet.MouseEnter += SetStatusBar_Enter;
			buttonPrintDataSheet.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonCancelPrint
			// 
			buttonCancelPrint.AccessibleDescription = "Cancels the print";
			buttonCancelPrint.AccessibleName = "Cancel print";
			buttonCancelPrint.AccessibleRole = AccessibleRole.PushButton;
			buttonCancelPrint.Location = new Point(164, 238);
			buttonCancelPrint.Name = "buttonCancelPrint";
			buttonCancelPrint.Size = new Size(128, 36);
			buttonCancelPrint.TabIndex = 2;
			toolTip.SetToolTip(buttonCancelPrint, "Cancel the print");
			buttonCancelPrint.Values.DropDownArrowColor = Color.Empty;
			buttonCancelPrint.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancelPrint.Values.Text = "&Cancel print";
			buttonCancelPrint.Click += ButtonCancelPrint_Click;
			buttonCancelPrint.Enter += SetStatusBar_Enter;
			buttonCancelPrint.Leave += ClearStatusBar_Leave;
			buttonCancelPrint.MouseEnter += SetStatusBar_Enter;
			buttonCancelPrint.MouseLeave += ClearStatusBar_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(statusStrip);
			panel.Controls.Add(checkedListBoxOrbitalElements);
			panel.Controls.Add(buttonPrintDataSheet);
			panel.Controls.Add(buttonCancelPrint);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(306, 308);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 286);
			statusStrip.Name = "statusStrip";
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(306, 22);
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
			// 
			// PrintDataSheetForm
			// 
			AccessibleDescription = "Prints a data sheet with some orbit elements";
			AccessibleName = "Print data sheet";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(306, 308);
			Controls.Add(panel);
			Font = new Font("Segoe UI", 8.5F);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "PrintDataSheetForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Print data sheet";
			toolTip.SetToolTip(this, "Print data sheet");
			FormClosed += PrintDataSheetForm_FormClosed;
			Load += PrintDataSheetForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);

		}

		#endregion

		private KryptonCheckedListBox checkedListBoxOrbitalElements;
        private ToolTip toolTip;
        private KryptonButton buttonPrintDataSheet;
        private KryptonButton buttonCancelPrint;
        private KryptonPanel panel;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
	}
}