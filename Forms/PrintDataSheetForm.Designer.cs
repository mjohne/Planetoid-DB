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
			kryptonPanel = new KryptonPanel();
			statusStrip = new KryptonStatusStrip();
			labelStatus = new ToolStripStatusLabel();
			toolStripProgressBarPrinting = new ToolStripProgressBar();
			kryptonManager = new KryptonManager(components);
			toolStripContainer = new ToolStripContainer();
			kryptonToolStripIcons = new KryptonToolStrip();
			toolStripButtonPrint = new ToolStripButton();
			toolStripButtonCancelPrint = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			toolStripButtonPrintPreview = new ToolStripButton();
			toolStripButtonPrintSetup = new ToolStripButton();
			toolStripSeparator = new ToolStripSeparator();
			toolStripButtonMarkAll = new ToolStripButton();
			toolStripButtonUnmarkAll = new ToolStripButton();
			((ISupportInitialize)kryptonPanel).BeginInit();
			kryptonPanel.SuspendLayout();
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
			checkedListBoxOrbitalElements.Size = new Size(313, 308);
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
			kryptonPanel.AccessibleDescription = "Groups the data";
			kryptonPanel.AccessibleName = "Pane";
			kryptonPanel.AccessibleRole = AccessibleRole.Pane;
			kryptonPanel.Controls.Add(checkedListBoxOrbitalElements);
			kryptonPanel.Dock = DockStyle.Fill;
			kryptonPanel.Location = new Point(0, 0);
			kryptonPanel.Margin = new Padding(4, 3, 4, 3);
			kryptonPanel.Name = "panel";
			kryptonPanel.PanelBackStyle = PaletteBackStyle.FormMain;
			kryptonPanel.Size = new Size(313, 308);
			kryptonPanel.TabIndex = 0;
			kryptonPanel.TabStop = true;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Dock = DockStyle.None;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelStatus, toolStripProgressBarPrinting });
			statusStrip.Location = new Point(0, 0);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(313, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 3;
			statusStrip.Text = "status bar";
			statusStrip.MouseEnter += Control_Enter;
			statusStrip.MouseLeave += Control_Leave;
			// 
			// labelInformation
			// 
			labelStatus.AccessibleDescription = "Shows some information";
			labelStatus.AccessibleName = "Shows some information";
			labelStatus.AccessibleRole = AccessibleRole.StaticText;
			labelStatus.AutoToolTip = true;
			labelStatus.Image = FatcowIcons16px.fatcow_lightbulb_16px;
			labelStatus.Margin = new Padding(5, 3, 0, 2);
			labelStatus.Name = "labelInformation";
			labelStatus.Size = new Size(144, 17);
			labelStatus.Text = "some information here";
			labelStatus.ToolTipText = "Shows some information";
			labelStatus.MouseEnter += Control_Enter;
			labelStatus.MouseLeave += Control_Leave;
			// 
			// kryptonProgressBar
			// 
			toolStripProgressBarPrinting.AccessibleDescription = "Shows the progress bar of the printing progress";
			toolStripProgressBarPrinting.AccessibleName = "Printing progress";
			toolStripProgressBarPrinting.AccessibleRole = AccessibleRole.ProgressBar;
			toolStripProgressBarPrinting.AutoToolTip = true;
			toolStripProgressBarPrinting.Name = "kryptonProgressBar";
			toolStripProgressBarPrinting.Size = new Size(150, 16);
			toolStripProgressBarPrinting.Style = ProgressBarStyle.Continuous;
			toolStripProgressBarPrinting.ToolTipText = "Printing progress";
			toolStripProgressBarPrinting.MouseEnter += Control_Enter;
			toolStripProgressBarPrinting.MouseLeave += Control_Leave;
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
			toolStripContainer.ContentPanel.Controls.Add(kryptonPanel);
			toolStripContainer.ContentPanel.Size = new Size(313, 308);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(313, 355);
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
			kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonPrint, toolStripButtonCancelPrint, toolStripSeparator1, toolStripButtonPrintPreview, toolStripButtonPrintSetup, toolStripSeparator, toolStripButtonMarkAll, toolStripButtonUnmarkAll });
			kryptonToolStripIcons.Location = new Point(0, 0);
			kryptonToolStripIcons.Name = "kryptonToolStripIcons";
			kryptonToolStripIcons.Size = new Size(313, 25);
			kryptonToolStripIcons.Stretch = true;
			kryptonToolStripIcons.TabIndex = 0;
			kryptonToolStripIcons.TabStop = true;
			kryptonToolStripIcons.Text = "Toolbar of printing values of orbital elements";
			kryptonToolStripIcons.Enter += Control_Enter;
			kryptonToolStripIcons.Leave += Control_Leave;
			kryptonToolStripIcons.MouseEnter += Control_Enter;
			kryptonToolStripIcons.MouseLeave += Control_Leave;
			// 
			// toolStripButtonPrint
			// 
			toolStripButtonPrint.AccessibleDescription = "Prints a data sheet with some orbit elements";
			toolStripButtonPrint.AccessibleName = "Print data sheet";
			toolStripButtonPrint.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonPrint.Image = FatcowIcons16px.fatcow_printer_16px;
			toolStripButtonPrint.ImageTransparentColor = Color.Magenta;
			toolStripButtonPrint.Name = "toolStripButtonPrint";
			toolStripButtonPrint.Size = new Size(52, 22);
			toolStripButtonPrint.Text = "&Print";
			toolStripButtonPrint.Click += ButtonPrintDataSheet_Click;
			toolStripButtonPrint.MouseEnter += Control_Enter;
			toolStripButtonPrint.MouseLeave += Control_Leave;
			// 
			// toolStripButtonCancelPrint
			// 
			toolStripButtonCancelPrint.AccessibleDescription = "Cancels the printing";
			toolStripButtonCancelPrint.AccessibleName = "Cancel print";
			toolStripButtonCancelPrint.AccessibleRole = AccessibleRole.Text;
			toolStripButtonCancelPrint.Image = FatcowIcons16px.fatcow_cancel_16px;
			toolStripButtonCancelPrint.ImageTransparentColor = Color.Magenta;
			toolStripButtonCancelPrint.Name = "toolStripButtonCancelPrint";
			toolStripButtonCancelPrint.Size = new Size(91, 22);
			toolStripButtonCancelPrint.Text = "&Cancel print";
			toolStripButtonCancelPrint.Click += ToolStripButtonCancelPrint_Click;
			toolStripButtonCancelPrint.MouseEnter += Control_Enter;
			toolStripButtonCancelPrint.MouseLeave += Control_Leave;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 25);
			// 
			// toolStripButtonPrintPreview
			// 
			toolStripButtonPrintPreview.AccessibleDescription = "Shows the print preview";
			toolStripButtonPrintPreview.AccessibleName = "Print preview";
			toolStripButtonPrintPreview.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonPrintPreview.Image = FatcowIcons16px.fatcow_page_white_magnify_16px;
			toolStripButtonPrintPreview.ImageTransparentColor = Color.Magenta;
			toolStripButtonPrintPreview.Name = "toolStripButtonPrintPreview";
			toolStripButtonPrintPreview.Size = new Size(68, 22);
			toolStripButtonPrintPreview.Text = "Pre&view";
			toolStripButtonPrintPreview.Click += ToolStripButtonPrintPreview_Click;
			toolStripButtonPrintPreview.MouseEnter += Control_Enter;
			toolStripButtonPrintPreview.MouseLeave += Control_Leave;
			// 
			// toolStripButtonPrintSetup
			// 
			toolStripButtonPrintSetup.AccessibleDescription = "Shows the printer setup";
			toolStripButtonPrintSetup.AccessibleName = "Printer setup";
			toolStripButtonPrintSetup.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonPrintSetup.Image = FatcowIcons16px.fatcow_wrench_orange_16px;
			toolStripButtonPrintSetup.ImageTransparentColor = Color.Magenta;
			toolStripButtonPrintSetup.Name = "toolStripButtonPrintSetup";
			toolStripButtonPrintSetup.Size = new Size(57, 22);
			toolStripButtonPrintSetup.Text = "&Setup";
			toolStripButtonPrintSetup.Click += ToolStripButtonPageSetup_Click;
			toolStripButtonPrintSetup.MouseEnter += Control_Enter;
			toolStripButtonPrintSetup.MouseLeave += Control_Leave;
			// 
			// toolStripSeparator
			// 
			toolStripSeparator.AccessibleDescription = "Just a separator";
			toolStripSeparator.AccessibleName = "Just a separator";
			toolStripSeparator.AccessibleRole = AccessibleRole.Separator;
			toolStripSeparator.Name = "toolStripSeparator";
			toolStripSeparator.Size = new Size(6, 25);
			// 
			// toolStripButtonMarkAll
			// 
			toolStripButtonMarkAll.AccessibleDescription = "Marks all orbital elements in the list";
			toolStripButtonMarkAll.AccessibleName = "Mark all orbital elements";
			toolStripButtonMarkAll.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonMarkAll.Image = FatcowIcons16px.fatcow_check_box_16px;
			toolStripButtonMarkAll.ImageTransparentColor = Color.Magenta;
			toolStripButtonMarkAll.Name = "toolStripButtonMarkAll";
			toolStripButtonMarkAll.Size = new Size(69, 20);
			toolStripButtonMarkAll.Text = "&Mark all";
			toolStripButtonMarkAll.Click += ToolStripButtonMarkAll_Click;
			toolStripButtonMarkAll.MouseEnter += Control_Enter;
			toolStripButtonMarkAll.MouseLeave += Control_Leave;
			// 
			// toolStripButtonUnmarkAll
			// 
			toolStripButtonUnmarkAll.AccessibleDescription = "Unmarks all orbital elements in the list";
			toolStripButtonUnmarkAll.AccessibleName = "Unmark all orbital elements";
			toolStripButtonUnmarkAll.AccessibleRole = AccessibleRole.PushButton;
			toolStripButtonUnmarkAll.Image = FatcowIcons16px.fatcow_check_box_uncheck_16px;
			toolStripButtonUnmarkAll.ImageTransparentColor = Color.Magenta;
			toolStripButtonUnmarkAll.Name = "toolStripButtonUnmarkAll";
			toolStripButtonUnmarkAll.Size = new Size(84, 20);
			toolStripButtonUnmarkAll.Text = "&Unmark all";
			toolStripButtonUnmarkAll.Click += ToolStripButtonUnmarkAll_Click;
			toolStripButtonUnmarkAll.MouseEnter += Control_Enter;
			toolStripButtonUnmarkAll.MouseLeave += Control_Leave;
			// 
			// PrintDataSheetForm
			// 
			AccessibleDescription = "Prints a data sheet with some orbit elements";
			AccessibleName = "Print data sheet";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(313, 355);
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
			((ISupportInitialize)kryptonPanel).EndInit();
			kryptonPanel.ResumeLayout(false);
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
        private KryptonPanel kryptonPanel;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelStatus;
		private ToolStripProgressBar toolStripProgressBarPrinting;
		private KryptonManager kryptonManager;
		private ToolStripContainer toolStripContainer;
		private KryptonToolStrip kryptonToolStripIcons;
		private ToolStripButton toolStripButtonPrint;
		private ToolStripSeparator toolStripSeparator;
		private ToolStripButton toolStripButtonMarkAll;
		private ToolStripButton toolStripButtonUnmarkAll;
		private ToolStripButton toolStripButtonPrintPreview;
		private ToolStripButton toolStripButtonPrintSetup;
		private ToolStripButton toolStripButtonCancelPrint;
		private ToolStripSeparator toolStripSeparator1;
	}
}