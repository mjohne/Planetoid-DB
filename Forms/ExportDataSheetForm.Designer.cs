using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class ExportDataSheetForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ExportDataSheetForm));
			buttonExportAsJson = new KryptonButton();
			buttonExportAsTxt = new KryptonButton();
			buttonExportAsXml = new KryptonButton();
			buttonExportAsHtml = new KryptonButton();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			buttonUnmarkAll = new KryptonButton();
			buttonMarkAll = new KryptonButton();
			toolStripContainer = new ToolStripContainer();
			panel = new KryptonPanel();
			checkedListBoxOrbitalElements = new KryptonCheckedListBox();
			saveFileDialogTxt = new SaveFileDialog();
			saveFileDialogHtml = new SaveFileDialog();
			saveFileDialogXml = new SaveFileDialog();
			saveFileDialogJson = new SaveFileDialog();
			kryptonManager = new KryptonManager(components);
			statusStrip.SuspendLayout();
			toolStripContainer.BottomToolStripPanel.SuspendLayout();
			toolStripContainer.ContentPanel.SuspendLayout();
			toolStripContainer.SuspendLayout();
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			SuspendLayout();
			// 
			// buttonExportAsJson
			// 
			buttonExportAsJson.AccessibleDescription = "Exports the data sheet as JSON file";
			buttonExportAsJson.AccessibleName = "Export as JSON";
			buttonExportAsJson.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsJson.Location = new Point(348, 245);
			buttonExportAsJson.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsJson.Name = "buttonExportAsJson";
			buttonExportAsJson.Size = new Size(111, 46);
			buttonExportAsJson.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsJson.TabIndex = 6;
			buttonExportAsJson.ToolTipValues.Description = "Exports the data sheet as JSON file";
			buttonExportAsJson.ToolTipValues.EnableToolTips = true;
			buttonExportAsJson.ToolTipValues.Heading = "Export as JSON";
			buttonExportAsJson.Values.DropDownArrowColor = Color.Empty;
			buttonExportAsJson.Values.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			buttonExportAsJson.Values.Text = "Export as JSON";
			buttonExportAsJson.Click += ButtonExportAsJson_Click;
			buttonExportAsJson.Enter += Control_Enter;
			buttonExportAsJson.Leave += Control_Leave;
			buttonExportAsJson.MouseEnter += Control_Enter;
			buttonExportAsJson.MouseLeave += Control_Leave;
			// 
			// buttonExportAsTxt
			// 
			buttonExportAsTxt.AccessibleDescription = "Exports the data sheet as text file";
			buttonExportAsTxt.AccessibleName = "Export as TXT";
			buttonExportAsTxt.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsTxt.Location = new Point(348, 85);
			buttonExportAsTxt.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsTxt.Name = "buttonExportAsTxt";
			buttonExportAsTxt.Size = new Size(112, 46);
			buttonExportAsTxt.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsTxt.TabIndex = 3;
			buttonExportAsTxt.ToolTipValues.Description = "Exports the data sheet as text file";
			buttonExportAsTxt.ToolTipValues.EnableToolTips = true;
			buttonExportAsTxt.ToolTipValues.Heading = "Export as TXT";
			buttonExportAsTxt.Values.DropDownArrowColor = Color.Empty;
			buttonExportAsTxt.Values.Image = FatcowIcons16px.fatcow_page_white_text_16px;
			buttonExportAsTxt.Values.Text = "Export as TXT";
			buttonExportAsTxt.Click += ButtonExportAsTxt_Click;
			buttonExportAsTxt.Enter += Control_Enter;
			buttonExportAsTxt.Leave += Control_Leave;
			buttonExportAsTxt.MouseEnter += Control_Enter;
			buttonExportAsTxt.MouseLeave += Control_Leave;
			// 
			// buttonExportAsXml
			// 
			buttonExportAsXml.AccessibleDescription = "Exports the data sheet as XML file";
			buttonExportAsXml.AccessibleName = "Export as XML";
			buttonExportAsXml.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsXml.Location = new Point(349, 192);
			buttonExportAsXml.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsXml.Name = "buttonExportAsXml";
			buttonExportAsXml.Size = new Size(111, 46);
			buttonExportAsXml.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsXml.TabIndex = 5;
			buttonExportAsXml.ToolTipValues.Description = "Exports the data sheet as XML file";
			buttonExportAsXml.ToolTipValues.EnableToolTips = true;
			buttonExportAsXml.ToolTipValues.Heading = "Export as XML";
			buttonExportAsXml.Values.DropDownArrowColor = Color.Empty;
			buttonExportAsXml.Values.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			buttonExportAsXml.Values.Text = "Export as XML";
			buttonExportAsXml.Click += ButtonExportAsXml_Click;
			buttonExportAsXml.Enter += Control_Enter;
			buttonExportAsXml.Leave += Control_Leave;
			buttonExportAsXml.MouseEnter += Control_Enter;
			buttonExportAsXml.MouseLeave += Control_Leave;
			// 
			// buttonExportAsHtml
			// 
			buttonExportAsHtml.AccessibleDescription = "Exports the data sheet as HTML file";
			buttonExportAsHtml.AccessibleName = "Export as HTML";
			buttonExportAsHtml.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsHtml.Location = new Point(348, 138);
			buttonExportAsHtml.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsHtml.Name = "buttonExportAsHtml";
			buttonExportAsHtml.Size = new Size(111, 46);
			buttonExportAsHtml.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsHtml.TabIndex = 4;
			buttonExportAsHtml.ToolTipValues.Description = "Exports the data sheet as HTML file";
			buttonExportAsHtml.ToolTipValues.EnableToolTips = true;
			buttonExportAsHtml.ToolTipValues.Heading = "Export as HTML";
			buttonExportAsHtml.Values.DropDownArrowColor = Color.Empty;
			buttonExportAsHtml.Values.Image = FatcowIcons16px.fatcow_page_white_code_16px;
			buttonExportAsHtml.Values.Text = "Export as HTML";
			buttonExportAsHtml.Click += ButtonExportAsHtml_Click;
			buttonExportAsHtml.Enter += Control_Enter;
			buttonExportAsHtml.Leave += Control_Leave;
			buttonExportAsHtml.MouseEnter += Control_Enter;
			buttonExportAsHtml.MouseLeave += Control_Leave;
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
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(474, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 0;
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
			// buttonUnmarkAll
			// 
			buttonUnmarkAll.AccessibleDescription = "Unmarks all orbital elements";
			buttonUnmarkAll.AccessibleName = "Unmark all orbital elements";
			buttonUnmarkAll.AccessibleRole = AccessibleRole.PushButton;
			buttonUnmarkAll.ButtonStyle = ButtonStyle.Form;
			buttonUnmarkAll.Location = new Point(349, 50);
			buttonUnmarkAll.Margin = new Padding(4, 3, 4, 3);
			buttonUnmarkAll.Name = "buttonUnmarkAll";
			buttonUnmarkAll.Size = new Size(110, 29);
			buttonUnmarkAll.TabIndex = 2;
			buttonUnmarkAll.ToolTipValues.Description = "Unmarks all orbital elements";
			buttonUnmarkAll.ToolTipValues.EnableToolTips = true;
			buttonUnmarkAll.ToolTipValues.Heading = "Unmark all orbital elements";
			buttonUnmarkAll.Values.DropDownArrowColor = Color.Empty;
			buttonUnmarkAll.Values.Text = "&Unmark all";
			buttonUnmarkAll.Click += ButtonUnmarkAll_Click;
			buttonUnmarkAll.Enter += Control_Enter;
			buttonUnmarkAll.Leave += Control_Leave;
			buttonUnmarkAll.MouseEnter += Control_Enter;
			buttonUnmarkAll.MouseLeave += Control_Leave;
			// 
			// buttonMarkAll
			// 
			buttonMarkAll.AccessibleDescription = "Marks all orbital elements";
			buttonMarkAll.AccessibleName = "Mark all orbital elements";
			buttonMarkAll.AccessibleRole = AccessibleRole.PushButton;
			buttonMarkAll.ButtonStyle = ButtonStyle.Form;
			buttonMarkAll.Location = new Point(349, 14);
			buttonMarkAll.Margin = new Padding(4, 3, 4, 3);
			buttonMarkAll.Name = "buttonMarkAll";
			buttonMarkAll.Size = new Size(110, 29);
			buttonMarkAll.TabIndex = 1;
			buttonMarkAll.ToolTipValues.Description = "Marks all orbital elements";
			buttonMarkAll.ToolTipValues.EnableToolTips = true;
			buttonMarkAll.ToolTipValues.Heading = "Mark all orbital elements";
			buttonMarkAll.Values.DropDownArrowColor = Color.Empty;
			buttonMarkAll.Values.Image = FatcowIcons16px.fatcow_asterisk_orange_16px;
			buttonMarkAll.Values.Text = "&Mark all";
			buttonMarkAll.Click += ButtonMarkAll_Click;
			buttonMarkAll.Enter += Control_Enter;
			buttonMarkAll.Leave += Control_Leave;
			buttonMarkAll.MouseEnter += Control_Enter;
			buttonMarkAll.MouseLeave += Control_Leave;
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
			toolStripContainer.ContentPanel.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.ContentPanel.Size = new Size(474, 308);
			toolStripContainer.Dock = DockStyle.Fill;
			toolStripContainer.Location = new Point(0, 0);
			toolStripContainer.Margin = new Padding(4, 3, 4, 3);
			toolStripContainer.Name = "toolStripContainer";
			toolStripContainer.Size = new Size(474, 330);
			toolStripContainer.TabIndex = 3;
			toolStripContainer.TopToolStripPanelVisible = false;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Groups the data";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(buttonExportAsJson);
			panel.Controls.Add(buttonUnmarkAll);
			panel.Controls.Add(buttonExportAsXml);
			panel.Controls.Add(buttonExportAsTxt);
			panel.Controls.Add(buttonExportAsHtml);
			panel.Controls.Add(checkedListBoxOrbitalElements);
			panel.Controls.Add(buttonMarkAll);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(474, 308);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// checkedListBoxOrbitalElements
			// 
			checkedListBoxOrbitalElements.AccessibleDescription = "Checks some orbital elements to print on a data sheet";
			checkedListBoxOrbitalElements.AccessibleName = "Check orbital elements";
			checkedListBoxOrbitalElements.AccessibleRole = AccessibleRole.List;
			checkedListBoxOrbitalElements.BackStyle = PaletteBackStyle.InputControlRibbon;
			checkedListBoxOrbitalElements.CheckOnClick = true;
			checkedListBoxOrbitalElements.FormattingEnabled = true;
			checkedListBoxOrbitalElements.HorizontalScrollbar = true;
			checkedListBoxOrbitalElements.Items.AddRange(new object[] { "Index No.", "Readable designation", "Epoch (in packed form, .0 TT)", "Mean anomaly at the epoch (degrees)", "Argument of perihelion, J2000.0 (degrees)", "Longitude of the ascending node, J2000.0", "Inclination to the ecliptic, J2000.0 (degrees)", "Orbital eccentricity", "Mean daily motion (degrees per day)", "Semimajor axis (AU)", "Absolute magnitude, H (mag)", "Slope parameter, G", "Reference", "Number of oppositions", "Number of observations", "Observation span", "r.m.s. residual (arseconds)", "Computer name", "4-hexdigit flags", "Date of last observation (YYYMMDD)", "Linear eccentricity (AU)", "Semi-minor axis (AU)", "Major axis (AU)", "Minor axis (AU)", "Eccenctric anomaly (degrees)", "True anomaly (degrees)", "Perihelion distance (AU)", "Aphelion distance (AU)", "Longitude of Descending node (degrees)", "Argument of aphelion (degrees)", "Focal parameter (AU)", "Semi-latus rectum (AU)", "Latus rectum (AU)", "Orbital period (years)", "Orbital area (AU²)", "Orbital perimeter (AU)", "Semi-mean axis (AU)", "Mean axis (AU)", "Standard gravitational parameter (AU³/a²)" });
			checkedListBoxOrbitalElements.Location = new Point(14, 14);
			checkedListBoxOrbitalElements.Margin = new Padding(4, 3, 4, 3);
			checkedListBoxOrbitalElements.Name = "checkedListBoxOrbitalElements";
			checkedListBoxOrbitalElements.Size = new Size(327, 277);
			checkedListBoxOrbitalElements.TabIndex = 0;
			checkedListBoxOrbitalElements.ToolTipValues.Description = "Checks some orbital elements to print on a data sheet";
			checkedListBoxOrbitalElements.ToolTipValues.EnableToolTips = true;
			checkedListBoxOrbitalElements.ToolTipValues.Heading = "Check orbital elements";
			checkedListBoxOrbitalElements.SelectedIndexChanged += CheckedListBoxOrbitalElements_SelectedIndexChanged;
			checkedListBoxOrbitalElements.Enter += Control_Enter;
			checkedListBoxOrbitalElements.Leave += Control_Leave;
			checkedListBoxOrbitalElements.MouseEnter += Control_Enter;
			checkedListBoxOrbitalElements.MouseLeave += Control_Leave;
			// 
			// saveFileDialogTxt
			// 
			saveFileDialogTxt.DefaultExt = "txt";
			saveFileDialogTxt.Filter = "text files|*.txt|all files|*.*";
			// 
			// saveFileDialogHtml
			// 
			saveFileDialogHtml.DefaultExt = "html";
			saveFileDialogHtml.Filter = "HTML files|*.html|all files|*.*";
			// 
			// saveFileDialogXml
			// 
			saveFileDialogXml.DefaultExt = "xml";
			saveFileDialogXml.Filter = "XML files|*.xml|all files|*.*";
			// 
			// saveFileDialogJson
			// 
			saveFileDialogJson.DefaultExt = "json";
			saveFileDialogJson.Filter = "JSON files|*.json|all files|*.*";
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// ExportDataSheetForm
			// 
			AccessibleDescription = "Exports data sheet";
			AccessibleName = "Export data sheet";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(474, 330);
			ControlBox = false;
			Controls.Add(toolStripContainer);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ExportDataSheetForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Export data sheet";
			Load += ExportDataSheetForm_Load;
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			toolStripContainer.BottomToolStripPanel.PerformLayout();
			toolStripContainer.ContentPanel.ResumeLayout(false);
			toolStripContainer.ResumeLayout(false);
			toolStripContainer.PerformLayout();
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion
		private KryptonButton buttonExportAsHtml;
		private KryptonButton buttonExportAsTxt;
		private KryptonButton buttonExportAsXml;
		private KryptonButton buttonExportAsJson;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private SaveFileDialog saveFileDialogTxt;
		private SaveFileDialog saveFileDialogHtml;
		private SaveFileDialog saveFileDialogXml;
		private SaveFileDialog saveFileDialogJson;
		private ToolStripContainer toolStripContainer;
		private KryptonPanel panel;
		private KryptonCheckedListBox checkedListBoxOrbitalElements;
		private KryptonButton buttonUnmarkAll;
		private KryptonButton buttonMarkAll;
		private KryptonManager kryptonManager;
	}
}