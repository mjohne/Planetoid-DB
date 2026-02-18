using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class RecordsMainForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RecordsMainForm));
			panel = new KryptonPanel();
			groupBoxExport = new KryptonGroupBox();
			buttonExportAsJson = new KryptonButton();
			buttonExportAsXml = new KryptonButton();
			buttonExportAsTxt = new KryptonButton();
			buttonExportAsHtml = new KryptonButton();
			tableLayoutPanel = new KryptonTableLayoutPanel();
			buttonGoto09 = new KryptonButton();
			buttonGoto08 = new KryptonButton();
			buttonGoto07 = new KryptonButton();
			buttonGoto06 = new KryptonButton();
			buttonGoto05 = new KryptonButton();
			buttonGoto04 = new KryptonButton();
			buttonGoto03 = new KryptonButton();
			buttonGoto02 = new KryptonButton();
			labelPlace10 = new KryptonLabel();
			labelPlace05 = new KryptonLabel();
			labelPlace04 = new KryptonLabel();
			labelPlace03 = new KryptonLabel();
			labelPlace01 = new KryptonLabel();
			labelPlace02 = new KryptonLabel();
			labelPlace06 = new KryptonLabel();
			labelPlace07 = new KryptonLabel();
			labelPlace08 = new KryptonLabel();
			labelPlace09 = new KryptonLabel();
			buttonGoto01 = new KryptonButton();
			labelReadableDesignationHeader = new KryptonLabel();
			labelValueHeader = new KryptonLabel();
			labelReadableDesignation01 = new KryptonLabel();
			labelReadableDesignation02 = new KryptonLabel();
			labelReadableDesignation03 = new KryptonLabel();
			labelReadableDesignation04 = new KryptonLabel();
			labelReadableDesignation05 = new KryptonLabel();
			labelReadableDesignation06 = new KryptonLabel();
			labelReadableDesignation07 = new KryptonLabel();
			labelReadableDesignation08 = new KryptonLabel();
			labelReadableDesignation09 = new KryptonLabel();
			labelReadableDesignation10 = new KryptonLabel();
			labelValue01 = new KryptonLabel();
			labelValue02 = new KryptonLabel();
			labelValue03 = new KryptonLabel();
			labelValue04 = new KryptonLabel();
			labelValue05 = new KryptonLabel();
			labelValue06 = new KryptonLabel();
			labelValue07 = new KryptonLabel();
			labelValue08 = new KryptonLabel();
			labelValue09 = new KryptonLabel();
			labelValue10 = new KryptonLabel();
			buttonGoto10 = new KryptonButton();
			buttonStart = new KryptonButton();
			labelPercent = new KryptonLabel();
			progressBar = new KryptonProgressBar();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			((ISupportInitialize)groupBoxExport).BeginInit();
			((ISupportInitialize)groupBoxExport.Panel).BeginInit();
			groupBoxExport.Panel.SuspendLayout();
			tableLayoutPanel.SuspendLayout();
			statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// panel
			// 
			panel.AccessibleDescription = "pane";
			panel.AccessibleName = "pane";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(groupBoxExport);
			panel.Controls.Add(tableLayoutPanel);
			panel.Controls.Add(buttonStart);
			panel.Controls.Add(labelPercent);
			panel.Controls.Add(progressBar);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(517, 532);
			panel.TabIndex = 0;
			panel.TabStop = true;
			// 
			// groupBoxExport
			// 
			groupBoxExport.AccessibleDescription = "Groups the export buttons";
			groupBoxExport.AccessibleName = "Groups the export buttons";
			groupBoxExport.AccessibleRole = AccessibleRole.Grouping;
			groupBoxExport.Location = new Point(14, 407);
			groupBoxExport.Margin = new Padding(4, 3, 4, 3);
			// 
			// 
			// 
			groupBoxExport.Panel.AccessibleDescription = "Groups the export buttons";
			groupBoxExport.Panel.AccessibleName = "Groups the export buttons";
			groupBoxExport.Panel.AccessibleRole = AccessibleRole.Grouping;
			groupBoxExport.Panel.AutoScroll = true;
			groupBoxExport.Panel.Controls.Add(buttonExportAsJson);
			groupBoxExport.Panel.Controls.Add(buttonExportAsXml);
			groupBoxExport.Panel.Controls.Add(buttonExportAsTxt);
			groupBoxExport.Panel.Controls.Add(buttonExportAsHtml);
			groupBoxExport.Size = new Size(488, 89);
			groupBoxExport.TabIndex = 4;
			groupBoxExport.Values.Heading = "Export list";
			groupBoxExport.Values.Image = FatcowIcons16px.fatcow_page_save_16px;
			groupBoxExport.Enter += Control_Enter;
			groupBoxExport.Leave += Control_Leave;
			groupBoxExport.MouseEnter += Control_Enter;
			groupBoxExport.MouseLeave += Control_Leave;
			// 
			// buttonExportAsJson
			// 
			buttonExportAsJson.AccessibleDescription = "Exports the data sheet as JSON file";
			buttonExportAsJson.AccessibleName = "Export as JSON";
			buttonExportAsJson.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsJson.Location = new Point(357, 7);
			buttonExportAsJson.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsJson.Name = "buttonExportAsJson";
			buttonExportAsJson.Size = new Size(113, 46);
			buttonExportAsJson.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsJson.TabIndex = 3;
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
			// buttonExportAsXml
			// 
			buttonExportAsXml.AccessibleDescription = "Exports the data sheet as XML file";
			buttonExportAsXml.AccessibleName = "Export as XML";
			buttonExportAsXml.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsXml.Location = new Point(239, 7);
			buttonExportAsXml.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsXml.Name = "buttonExportAsXml";
			buttonExportAsXml.Size = new Size(111, 46);
			buttonExportAsXml.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsXml.TabIndex = 2;
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
			// buttonExportAsTxt
			// 
			buttonExportAsTxt.AccessibleDescription = "Exports the data sheet as text file";
			buttonExportAsTxt.AccessibleName = "Export as TXT";
			buttonExportAsTxt.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsTxt.Location = new Point(4, 7);
			buttonExportAsTxt.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsTxt.Name = "buttonExportAsTxt";
			buttonExportAsTxt.Size = new Size(111, 46);
			buttonExportAsTxt.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsTxt.TabIndex = 0;
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
			// buttonExportAsHtml
			// 
			buttonExportAsHtml.AccessibleDescription = "Exports the data sheet as HTML file";
			buttonExportAsHtml.AccessibleName = "Export as HTML";
			buttonExportAsHtml.AccessibleRole = AccessibleRole.PushButton;
			buttonExportAsHtml.Location = new Point(121, 7);
			buttonExportAsHtml.Margin = new Padding(4, 3, 4, 3);
			buttonExportAsHtml.Name = "buttonExportAsHtml";
			buttonExportAsHtml.Size = new Size(111, 46);
			buttonExportAsHtml.StateCommon.Content.Image.ImageV = PaletteRelativeAlign.Near;
			buttonExportAsHtml.TabIndex = 1;
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
			// tableLayoutPanel
			// 
			tableLayoutPanel.AccessibleDescription = "Groups the data";
			tableLayoutPanel.AccessibleName = "Table panel";
			tableLayoutPanel.AccessibleRole = AccessibleRole.Grouping;
			tableLayoutPanel.ColumnCount = 4;
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 233F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel.Controls.Add(buttonGoto09, 3, 9);
			tableLayoutPanel.Controls.Add(buttonGoto08, 3, 8);
			tableLayoutPanel.Controls.Add(buttonGoto07, 3, 7);
			tableLayoutPanel.Controls.Add(buttonGoto06, 3, 6);
			tableLayoutPanel.Controls.Add(buttonGoto05, 3, 5);
			tableLayoutPanel.Controls.Add(buttonGoto04, 3, 4);
			tableLayoutPanel.Controls.Add(buttonGoto03, 3, 3);
			tableLayoutPanel.Controls.Add(buttonGoto02, 3, 2);
			tableLayoutPanel.Controls.Add(labelPlace10, 0, 10);
			tableLayoutPanel.Controls.Add(labelPlace05, 0, 5);
			tableLayoutPanel.Controls.Add(labelPlace04, 0, 4);
			tableLayoutPanel.Controls.Add(labelPlace03, 0, 3);
			tableLayoutPanel.Controls.Add(labelPlace01, 0, 1);
			tableLayoutPanel.Controls.Add(labelPlace02, 0, 2);
			tableLayoutPanel.Controls.Add(labelPlace06, 0, 6);
			tableLayoutPanel.Controls.Add(labelPlace07, 0, 7);
			tableLayoutPanel.Controls.Add(labelPlace08, 0, 8);
			tableLayoutPanel.Controls.Add(labelPlace09, 0, 9);
			tableLayoutPanel.Controls.Add(buttonGoto01, 3, 1);
			tableLayoutPanel.Controls.Add(labelReadableDesignationHeader, 1, 0);
			tableLayoutPanel.Controls.Add(labelValueHeader, 2, 0);
			tableLayoutPanel.Controls.Add(labelReadableDesignation01, 1, 1);
			tableLayoutPanel.Controls.Add(labelReadableDesignation02, 1, 2);
			tableLayoutPanel.Controls.Add(labelReadableDesignation03, 1, 3);
			tableLayoutPanel.Controls.Add(labelReadableDesignation04, 1, 4);
			tableLayoutPanel.Controls.Add(labelReadableDesignation05, 1, 5);
			tableLayoutPanel.Controls.Add(labelReadableDesignation06, 1, 6);
			tableLayoutPanel.Controls.Add(labelReadableDesignation07, 1, 7);
			tableLayoutPanel.Controls.Add(labelReadableDesignation08, 1, 8);
			tableLayoutPanel.Controls.Add(labelReadableDesignation09, 1, 9);
			tableLayoutPanel.Controls.Add(labelReadableDesignation10, 1, 10);
			tableLayoutPanel.Controls.Add(labelValue01, 2, 1);
			tableLayoutPanel.Controls.Add(labelValue02, 2, 2);
			tableLayoutPanel.Controls.Add(labelValue03, 2, 3);
			tableLayoutPanel.Controls.Add(labelValue04, 2, 4);
			tableLayoutPanel.Controls.Add(labelValue05, 2, 5);
			tableLayoutPanel.Controls.Add(labelValue06, 2, 6);
			tableLayoutPanel.Controls.Add(labelValue07, 2, 7);
			tableLayoutPanel.Controls.Add(labelValue08, 2, 8);
			tableLayoutPanel.Controls.Add(labelValue09, 2, 9);
			tableLayoutPanel.Controls.Add(labelValue10, 2, 10);
			tableLayoutPanel.Controls.Add(buttonGoto10, 3, 10);
			tableLayoutPanel.Location = new Point(14, 50);
			tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.RowCount = 11;
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.RowStyles.Add(new RowStyle());
			tableLayoutPanel.Size = new Size(489, 351);
			tableLayoutPanel.TabIndex = 3;
			// 
			// buttonGoto09
			// 
			buttonGoto09.AccessibleDescription = "Goes to the element of the place no. 9";
			buttonGoto09.AccessibleName = "Go to the element of the place no. 9";
			buttonGoto09.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto09.ButtonStyle = ButtonStyle.Form;
			buttonGoto09.Location = new Point(415, 277);
			buttonGoto09.Margin = new Padding(4, 3, 4, 3);
			buttonGoto09.Name = "buttonGoto09";
			buttonGoto09.Size = new Size(70, 23);
			buttonGoto09.TabIndex = 37;
			buttonGoto09.ToolTipValues.Description = "Goes to the element of the place no. 9 ";
			buttonGoto09.ToolTipValues.EnableToolTips = true;
			buttonGoto09.ToolTipValues.Heading = "Go to the element of the place no. 9 ";
			buttonGoto09.Values.DropDownArrowColor = Color.Empty;
			buttonGoto09.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto09.Values.Text = "Goto";
			buttonGoto09.Click += ButtonGoto09_Click;
			buttonGoto09.Enter += Control_Enter;
			buttonGoto09.Leave += Control_Leave;
			buttonGoto09.MouseEnter += Control_Enter;
			buttonGoto09.MouseLeave += Control_Leave;
			// 
			// buttonGoto08
			// 
			buttonGoto08.AccessibleDescription = "Goes to the element of the place no. 8";
			buttonGoto08.AccessibleName = "Go to the element of the place no. 8";
			buttonGoto08.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto08.ButtonStyle = ButtonStyle.Form;
			buttonGoto08.Location = new Point(415, 248);
			buttonGoto08.Margin = new Padding(4, 3, 4, 3);
			buttonGoto08.Name = "buttonGoto08";
			buttonGoto08.Size = new Size(70, 23);
			buttonGoto08.TabIndex = 33;
			buttonGoto08.ToolTipValues.Description = "Goes to the element of the place no. 8";
			buttonGoto08.ToolTipValues.EnableToolTips = true;
			buttonGoto08.ToolTipValues.Heading = "Go to the element of the place no. 8";
			buttonGoto08.Values.DropDownArrowColor = Color.Empty;
			buttonGoto08.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto08.Values.Text = "Goto";
			buttonGoto08.Click += ButtonGoto08_Click;
			buttonGoto08.Enter += Control_Enter;
			buttonGoto08.Leave += Control_Leave;
			buttonGoto08.MouseEnter += Control_Enter;
			buttonGoto08.MouseLeave += Control_Leave;
			// 
			// buttonGoto07
			// 
			buttonGoto07.AccessibleDescription = "Goes to the element of the place no. 7";
			buttonGoto07.AccessibleName = "Go to the element of the place no. 7";
			buttonGoto07.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto07.ButtonStyle = ButtonStyle.Form;
			buttonGoto07.Location = new Point(415, 219);
			buttonGoto07.Margin = new Padding(4, 3, 4, 3);
			buttonGoto07.Name = "buttonGoto07";
			buttonGoto07.Size = new Size(70, 23);
			buttonGoto07.TabIndex = 29;
			buttonGoto07.ToolTipValues.Description = "Goes to the element of the place no. 7";
			buttonGoto07.ToolTipValues.EnableToolTips = true;
			buttonGoto07.ToolTipValues.Heading = "Go to the element of the place no. 7";
			buttonGoto07.Values.DropDownArrowColor = Color.Empty;
			buttonGoto07.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto07.Values.Text = "Goto";
			buttonGoto07.Click += ButtonGoto07_Click;
			buttonGoto07.Enter += Control_Enter;
			buttonGoto07.Leave += Control_Leave;
			buttonGoto07.MouseEnter += Control_Enter;
			buttonGoto07.MouseLeave += Control_Leave;
			// 
			// buttonGoto06
			// 
			buttonGoto06.AccessibleDescription = "Goes to the element of the place no. 6";
			buttonGoto06.AccessibleName = "Go to the element of the place no. 6";
			buttonGoto06.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto06.ButtonStyle = ButtonStyle.Form;
			buttonGoto06.Location = new Point(415, 190);
			buttonGoto06.Margin = new Padding(4, 3, 4, 3);
			buttonGoto06.Name = "buttonGoto06";
			buttonGoto06.Size = new Size(70, 23);
			buttonGoto06.TabIndex = 25;
			buttonGoto06.ToolTipValues.Description = "Goes to the element of the place no. 6";
			buttonGoto06.ToolTipValues.EnableToolTips = true;
			buttonGoto06.ToolTipValues.Heading = "Go to the element of the place no. 6";
			buttonGoto06.Values.DropDownArrowColor = Color.Empty;
			buttonGoto06.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto06.Values.Text = "Goto";
			buttonGoto06.Click += ButtonGoto06_Click;
			buttonGoto06.Enter += Control_Enter;
			buttonGoto06.Leave += Control_Leave;
			buttonGoto06.MouseEnter += Control_Enter;
			buttonGoto06.MouseLeave += Control_Leave;
			// 
			// buttonGoto05
			// 
			buttonGoto05.AccessibleDescription = "Goes to the element of the place no. 5";
			buttonGoto05.AccessibleName = "Go to the element of the place no. 5";
			buttonGoto05.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto05.ButtonStyle = ButtonStyle.Form;
			buttonGoto05.Location = new Point(415, 161);
			buttonGoto05.Margin = new Padding(4, 3, 4, 3);
			buttonGoto05.Name = "buttonGoto05";
			buttonGoto05.Size = new Size(70, 23);
			buttonGoto05.TabIndex = 21;
			buttonGoto05.ToolTipValues.Description = "Goes to the element of the place no. 5";
			buttonGoto05.ToolTipValues.EnableToolTips = true;
			buttonGoto05.ToolTipValues.Heading = "Go to the element of the place no. 5";
			buttonGoto05.Values.DropDownArrowColor = Color.Empty;
			buttonGoto05.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto05.Values.Text = "Goto";
			buttonGoto05.Click += ButtonGoto05_Click;
			buttonGoto05.Enter += Control_Enter;
			buttonGoto05.Leave += Control_Leave;
			buttonGoto05.MouseEnter += Control_Enter;
			buttonGoto05.MouseLeave += Control_Leave;
			// 
			// buttonGoto04
			// 
			buttonGoto04.AccessibleDescription = "Goes to the element of the place no. 4";
			buttonGoto04.AccessibleName = "Go to the element of the place no. 4";
			buttonGoto04.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto04.ButtonStyle = ButtonStyle.Form;
			buttonGoto04.Location = new Point(415, 132);
			buttonGoto04.Margin = new Padding(4, 3, 4, 3);
			buttonGoto04.Name = "buttonGoto04";
			buttonGoto04.Size = new Size(70, 23);
			buttonGoto04.TabIndex = 17;
			buttonGoto04.ToolTipValues.Description = "Goes to the element of the place no. 4";
			buttonGoto04.ToolTipValues.EnableToolTips = true;
			buttonGoto04.ToolTipValues.Heading = "Go to the element of the place no. 4";
			buttonGoto04.Values.DropDownArrowColor = Color.Empty;
			buttonGoto04.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto04.Values.Text = "Goto";
			buttonGoto04.Click += ButtonGoto04_Click;
			buttonGoto04.Enter += Control_Enter;
			buttonGoto04.Leave += Control_Leave;
			buttonGoto04.MouseEnter += Control_Enter;
			buttonGoto04.MouseLeave += Control_Leave;
			// 
			// buttonGoto03
			// 
			buttonGoto03.AccessibleDescription = "Goes to the element of the place no. 3";
			buttonGoto03.AccessibleName = "Go to the element of the place no. 3";
			buttonGoto03.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto03.ButtonStyle = ButtonStyle.Form;
			buttonGoto03.Location = new Point(415, 103);
			buttonGoto03.Margin = new Padding(4, 3, 4, 3);
			buttonGoto03.Name = "buttonGoto03";
			buttonGoto03.Size = new Size(70, 23);
			buttonGoto03.TabIndex = 13;
			buttonGoto03.ToolTipValues.Description = "Goes to the element of the place no. 3";
			buttonGoto03.ToolTipValues.EnableToolTips = true;
			buttonGoto03.ToolTipValues.Heading = "Go to the element of the place no. 3";
			buttonGoto03.Values.DropDownArrowColor = Color.Empty;
			buttonGoto03.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto03.Values.Text = "Goto";
			buttonGoto03.Click += ButtonGoto03_Click;
			buttonGoto03.Enter += Control_Enter;
			buttonGoto03.Leave += Control_Leave;
			buttonGoto03.MouseEnter += Control_Enter;
			buttonGoto03.MouseLeave += Control_Leave;
			// 
			// buttonGoto02
			// 
			buttonGoto02.AccessibleDescription = "Goes to the element of the place no. 2";
			buttonGoto02.AccessibleName = "Go to the element of the place no. 2";
			buttonGoto02.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto02.ButtonStyle = ButtonStyle.Form;
			buttonGoto02.Location = new Point(415, 74);
			buttonGoto02.Margin = new Padding(4, 3, 4, 3);
			buttonGoto02.Name = "buttonGoto02";
			buttonGoto02.Size = new Size(70, 23);
			buttonGoto02.TabIndex = 9;
			buttonGoto02.ToolTipValues.Description = "Goes to the element of the place no. 2";
			buttonGoto02.ToolTipValues.EnableToolTips = true;
			buttonGoto02.ToolTipValues.Heading = "HeadingGo to the element of the place no. 2";
			buttonGoto02.Values.DropDownArrowColor = Color.Empty;
			buttonGoto02.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto02.Values.Text = "Goto";
			buttonGoto02.Click += ButtonGoto02_Click;
			buttonGoto02.Enter += Control_Enter;
			buttonGoto02.Leave += Control_Leave;
			buttonGoto02.MouseEnter += Control_Enter;
			buttonGoto02.MouseLeave += Control_Leave;
			// 
			// labelPlace10
			// 
			labelPlace10.AccessibleDescription = "Shows the record place no. 10";
			labelPlace10.AccessibleName = "Record place no. 10";
			labelPlace10.AccessibleRole = AccessibleRole.StaticText;
			labelPlace10.Dock = DockStyle.Fill;
			labelPlace10.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace10.Location = new Point(4, 306);
			labelPlace10.Margin = new Padding(4, 3, 4, 3);
			labelPlace10.Name = "labelPlace10";
			labelPlace10.Size = new Size(24, 42);
			labelPlace10.TabIndex = 38;
			labelPlace10.ToolTipValues.EnableToolTips = true;
			labelPlace10.Values.Text = "10";
			labelPlace10.Enter += Control_Enter;
			labelPlace10.Leave += Control_Leave;
			labelPlace10.MouseEnter += Control_Enter;
			labelPlace10.MouseLeave += Control_Leave;
			// 
			// labelPlace05
			// 
			labelPlace05.AccessibleDescription = "Shows the record place no. 5";
			labelPlace05.AccessibleName = "Record place no. 5";
			labelPlace05.AccessibleRole = AccessibleRole.StaticText;
			labelPlace05.Dock = DockStyle.Fill;
			labelPlace05.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace05.Location = new Point(4, 161);
			labelPlace05.Margin = new Padding(4, 3, 4, 3);
			labelPlace05.Name = "labelPlace05";
			labelPlace05.Size = new Size(24, 23);
			labelPlace05.TabIndex = 18;
			labelPlace05.ToolTipValues.Description = "Shows the record place no. 5";
			labelPlace05.ToolTipValues.EnableToolTips = true;
			labelPlace05.ToolTipValues.Heading = "Record place no. 5";
			labelPlace05.Values.Text = "5";
			labelPlace05.Enter += Control_Enter;
			labelPlace05.Leave += Control_Leave;
			labelPlace05.MouseEnter += Control_Enter;
			labelPlace05.MouseLeave += Control_Leave;
			// 
			// labelPlace04
			// 
			labelPlace04.AccessibleDescription = "Shows the record place no. 4";
			labelPlace04.AccessibleName = "Record place no. 4";
			labelPlace04.AccessibleRole = AccessibleRole.StaticText;
			labelPlace04.Dock = DockStyle.Fill;
			labelPlace04.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace04.Location = new Point(4, 132);
			labelPlace04.Margin = new Padding(4, 3, 4, 3);
			labelPlace04.Name = "labelPlace04";
			labelPlace04.Size = new Size(24, 23);
			labelPlace04.TabIndex = 14;
			labelPlace04.ToolTipValues.Description = "Shows the record place no. 4";
			labelPlace04.ToolTipValues.EnableToolTips = true;
			labelPlace04.ToolTipValues.Heading = "Record place no. 4";
			labelPlace04.Values.Text = "4";
			labelPlace04.Enter += Control_Enter;
			labelPlace04.Leave += Control_Leave;
			labelPlace04.MouseEnter += Control_Enter;
			labelPlace04.MouseLeave += Control_Leave;
			// 
			// labelPlace03
			// 
			labelPlace03.AccessibleDescription = "Shows the record place no. 3";
			labelPlace03.AccessibleName = "Record place no. 3";
			labelPlace03.AccessibleRole = AccessibleRole.StaticText;
			labelPlace03.Dock = DockStyle.Fill;
			labelPlace03.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace03.Location = new Point(4, 103);
			labelPlace03.Margin = new Padding(4, 3, 4, 3);
			labelPlace03.Name = "labelPlace03";
			labelPlace03.Size = new Size(24, 23);
			labelPlace03.TabIndex = 10;
			labelPlace03.ToolTipValues.Description = "Shows the record place no. 3";
			labelPlace03.ToolTipValues.EnableToolTips = true;
			labelPlace03.ToolTipValues.Heading = "Record place no. 3";
			labelPlace03.Values.Text = "3";
			labelPlace03.Enter += Control_Enter;
			labelPlace03.Leave += Control_Leave;
			labelPlace03.MouseEnter += Control_Enter;
			labelPlace03.MouseLeave += Control_Leave;
			// 
			// labelPlace01
			// 
			labelPlace01.AccessibleDescription = "Shows the record place no.";
			labelPlace01.AccessibleName = "Record place no.";
			labelPlace01.AccessibleRole = AccessibleRole.StaticText;
			labelPlace01.Dock = DockStyle.Fill;
			labelPlace01.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace01.Location = new Point(4, 45);
			labelPlace01.Margin = new Padding(4, 3, 4, 3);
			labelPlace01.Name = "labelPlace01";
			labelPlace01.Size = new Size(24, 23);
			labelPlace01.TabIndex = 2;
			labelPlace01.ToolTipValues.Description = "Shows the record place no.";
			labelPlace01.ToolTipValues.EnableToolTips = true;
			labelPlace01.ToolTipValues.Heading = "Record place no.";
			labelPlace01.Values.Text = "1";
			labelPlace01.Enter += Control_Enter;
			labelPlace01.Leave += Control_Leave;
			labelPlace01.MouseEnter += Control_Enter;
			labelPlace01.MouseLeave += Control_Leave;
			// 
			// labelPlace02
			// 
			labelPlace02.AccessibleDescription = "Shows the record place no. 2";
			labelPlace02.AccessibleName = "Record place no. 2";
			labelPlace02.AccessibleRole = AccessibleRole.StaticText;
			labelPlace02.Dock = DockStyle.Fill;
			labelPlace02.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace02.Location = new Point(4, 74);
			labelPlace02.Margin = new Padding(4, 3, 4, 3);
			labelPlace02.Name = "labelPlace02";
			labelPlace02.Size = new Size(24, 23);
			labelPlace02.TabIndex = 6;
			labelPlace02.ToolTipValues.Description = "Shows the record place no. 2";
			labelPlace02.ToolTipValues.EnableToolTips = true;
			labelPlace02.ToolTipValues.Heading = "Record place no. 2";
			labelPlace02.Values.Text = "2";
			labelPlace02.Enter += Control_Enter;
			labelPlace02.Leave += Control_Leave;
			labelPlace02.MouseEnter += Control_Enter;
			labelPlace02.MouseLeave += Control_Leave;
			// 
			// labelPlace06
			// 
			labelPlace06.AccessibleDescription = "Shows the record place no. 6";
			labelPlace06.AccessibleName = "Record place no. 6";
			labelPlace06.AccessibleRole = AccessibleRole.StaticText;
			labelPlace06.Dock = DockStyle.Fill;
			labelPlace06.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace06.Location = new Point(4, 190);
			labelPlace06.Margin = new Padding(4, 3, 4, 3);
			labelPlace06.Name = "labelPlace06";
			labelPlace06.Size = new Size(24, 23);
			labelPlace06.TabIndex = 22;
			labelPlace06.ToolTipValues.Description = "Shows the record place no. 6";
			labelPlace06.ToolTipValues.EnableToolTips = true;
			labelPlace06.ToolTipValues.Heading = "Record place no. 6";
			labelPlace06.Values.Text = "6";
			labelPlace06.Enter += Control_Enter;
			labelPlace06.Leave += Control_Leave;
			labelPlace06.MouseEnter += Control_Enter;
			labelPlace06.MouseLeave += Control_Leave;
			// 
			// labelPlace07
			// 
			labelPlace07.AccessibleDescription = "Shows the record place no. 7";
			labelPlace07.AccessibleName = "Record place no. 7";
			labelPlace07.AccessibleRole = AccessibleRole.StaticText;
			labelPlace07.Dock = DockStyle.Fill;
			labelPlace07.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace07.Location = new Point(4, 219);
			labelPlace07.Margin = new Padding(4, 3, 4, 3);
			labelPlace07.Name = "labelPlace07";
			labelPlace07.Size = new Size(24, 23);
			labelPlace07.TabIndex = 26;
			labelPlace07.ToolTipValues.Description = "Shows the record place no. 7";
			labelPlace07.ToolTipValues.EnableToolTips = true;
			labelPlace07.ToolTipValues.Heading = "Record place no. 7";
			labelPlace07.Values.Text = "7";
			labelPlace07.Enter += Control_Enter;
			labelPlace07.Leave += Control_Leave;
			labelPlace07.MouseEnter += Control_Enter;
			labelPlace07.MouseLeave += Control_Leave;
			// 
			// labelPlace08
			// 
			labelPlace08.AccessibleDescription = "Shows the record place no. 8";
			labelPlace08.AccessibleName = "Record place no. 8";
			labelPlace08.AccessibleRole = AccessibleRole.StaticText;
			labelPlace08.Dock = DockStyle.Fill;
			labelPlace08.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace08.Location = new Point(4, 248);
			labelPlace08.Margin = new Padding(4, 3, 4, 3);
			labelPlace08.Name = "labelPlace08";
			labelPlace08.Size = new Size(24, 23);
			labelPlace08.TabIndex = 30;
			labelPlace08.ToolTipValues.Description = "Shows the record place no. 8";
			labelPlace08.ToolTipValues.EnableToolTips = true;
			labelPlace08.ToolTipValues.Heading = "Record place no. 8";
			labelPlace08.Values.Text = "8";
			labelPlace08.Enter += Control_Enter;
			labelPlace08.Leave += Control_Leave;
			labelPlace08.MouseEnter += Control_Enter;
			labelPlace08.MouseLeave += Control_Leave;
			// 
			// labelPlace09
			// 
			labelPlace09.AccessibleDescription = "Shows the record place no. 9";
			labelPlace09.AccessibleName = "Record place no. 9";
			labelPlace09.AccessibleRole = AccessibleRole.StaticText;
			labelPlace09.Dock = DockStyle.Fill;
			labelPlace09.LabelStyle = LabelStyle.ItalicPanel;
			labelPlace09.Location = new Point(4, 277);
			labelPlace09.Margin = new Padding(4, 3, 4, 3);
			labelPlace09.Name = "labelPlace09";
			labelPlace09.Size = new Size(24, 23);
			labelPlace09.TabIndex = 34;
			labelPlace09.ToolTipValues.Description = "Shows the record place no. 9";
			labelPlace09.ToolTipValues.EnableToolTips = true;
			labelPlace09.ToolTipValues.Heading = "Record place no. 9";
			labelPlace09.Values.Text = "9";
			labelPlace09.Enter += Control_Enter;
			labelPlace09.Leave += Control_Leave;
			labelPlace09.MouseEnter += Control_Enter;
			labelPlace09.MouseLeave += Control_Leave;
			// 
			// buttonGoto01
			// 
			buttonGoto01.AccessibleDescription = "Goes to the element of the place no. 1";
			buttonGoto01.AccessibleName = "Go to the element of the place no. 1";
			buttonGoto01.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto01.ButtonStyle = ButtonStyle.Form;
			buttonGoto01.Location = new Point(415, 45);
			buttonGoto01.Margin = new Padding(4, 3, 4, 3);
			buttonGoto01.Name = "buttonGoto01";
			buttonGoto01.Size = new Size(70, 23);
			buttonGoto01.TabIndex = 5;
			buttonGoto01.ToolTipValues.Description = "Goes to the element of the place no. 1";
			buttonGoto01.ToolTipValues.EnableToolTips = true;
			buttonGoto01.ToolTipValues.Heading = "Go to the element of the place no. 1";
			buttonGoto01.Values.DropDownArrowColor = Color.Empty;
			buttonGoto01.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto01.Values.Text = "Goto";
			buttonGoto01.Click += ButtonGoto01_Click;
			buttonGoto01.Enter += Control_Enter;
			buttonGoto01.Leave += Control_Leave;
			buttonGoto01.MouseEnter += Control_Enter;
			buttonGoto01.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignationHeader
			// 
			labelReadableDesignationHeader.AccessibleDescription = "Shows the readable designation header";
			labelReadableDesignationHeader.AccessibleName = "Readable designation header";
			labelReadableDesignationHeader.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignationHeader.Dock = DockStyle.Fill;
			labelReadableDesignationHeader.LabelStyle = LabelStyle.BoldPanel;
			labelReadableDesignationHeader.Location = new Point(36, 3);
			labelReadableDesignationHeader.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignationHeader.Name = "labelReadableDesignationHeader";
			labelReadableDesignationHeader.Size = new Size(138, 36);
			labelReadableDesignationHeader.TabIndex = 0;
			labelReadableDesignationHeader.ToolTipValues.Description = "Shows the readable designation header";
			labelReadableDesignationHeader.ToolTipValues.EnableToolTips = true;
			labelReadableDesignationHeader.ToolTipValues.Heading = "Readable designation header";
			labelReadableDesignationHeader.Values.Text = "Readable \r\ndesignation";
			labelReadableDesignationHeader.Enter += Control_Enter;
			labelReadableDesignationHeader.Leave += Control_Leave;
			labelReadableDesignationHeader.MouseEnter += Control_Enter;
			labelReadableDesignationHeader.MouseLeave += Control_Leave;
			// 
			// labelValueHeader
			// 
			labelValueHeader.AccessibleDescription = "Shows the value header";
			labelValueHeader.AccessibleName = "Value header";
			labelValueHeader.AccessibleRole = AccessibleRole.StaticText;
			labelValueHeader.Dock = DockStyle.Fill;
			labelValueHeader.LabelStyle = LabelStyle.BoldPanel;
			labelValueHeader.Location = new Point(182, 3);
			labelValueHeader.Margin = new Padding(4, 3, 4, 3);
			labelValueHeader.Name = "labelValueHeader";
			labelValueHeader.Size = new Size(225, 36);
			labelValueHeader.TabIndex = 1;
			labelValueHeader.ToolTipValues.Description = "Shows the value header";
			labelValueHeader.ToolTipValues.EnableToolTips = true;
			labelValueHeader.ToolTipValues.Heading = "Value header";
			labelValueHeader.Values.Text = "Value";
			labelValueHeader.Enter += Control_Enter;
			labelValueHeader.Leave += Control_Leave;
			labelValueHeader.MouseEnter += Control_Enter;
			labelValueHeader.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation01
			// 
			labelReadableDesignation01.AccessibleDescription = "Shows the readable designation no. 1";
			labelReadableDesignation01.AccessibleName = "Readable designation no. 1";
			labelReadableDesignation01.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation01.Dock = DockStyle.Fill;
			labelReadableDesignation01.Location = new Point(36, 45);
			labelReadableDesignation01.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation01.Name = "labelReadableDesignation01";
			labelReadableDesignation01.Size = new Size(138, 23);
			labelReadableDesignation01.TabIndex = 3;
			labelReadableDesignation01.ToolTipValues.Description = "Shows the readable designation no. 1";
			labelReadableDesignation01.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation01.ToolTipValues.Heading = "Readable designation no. 1";
			labelReadableDesignation01.Values.Text = "designation 01";
			labelReadableDesignation01.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation01.Enter += Control_Enter;
			labelReadableDesignation01.Leave += Control_Leave;
			labelReadableDesignation01.MouseEnter += Control_Enter;
			labelReadableDesignation01.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation02
			// 
			labelReadableDesignation02.AccessibleDescription = "Shows the readable designation no. 2";
			labelReadableDesignation02.AccessibleName = "Readable designation no. 2";
			labelReadableDesignation02.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation02.Dock = DockStyle.Fill;
			labelReadableDesignation02.Location = new Point(36, 74);
			labelReadableDesignation02.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation02.Name = "labelReadableDesignation02";
			labelReadableDesignation02.Size = new Size(138, 23);
			labelReadableDesignation02.TabIndex = 7;
			labelReadableDesignation02.ToolTipValues.Description = "Shows the readable designation no. 2";
			labelReadableDesignation02.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation02.ToolTipValues.Heading = "Readable designation no. 2";
			labelReadableDesignation02.Values.Text = "designation 02";
			labelReadableDesignation02.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation02.Enter += Control_Enter;
			labelReadableDesignation02.Leave += Control_Leave;
			labelReadableDesignation02.MouseEnter += Control_Enter;
			labelReadableDesignation02.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation03
			// 
			labelReadableDesignation03.AccessibleDescription = "Shows the readable designation no. 3";
			labelReadableDesignation03.AccessibleName = "Readable designation no. 3";
			labelReadableDesignation03.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation03.Dock = DockStyle.Fill;
			labelReadableDesignation03.Location = new Point(36, 103);
			labelReadableDesignation03.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation03.Name = "labelReadableDesignation03";
			labelReadableDesignation03.Size = new Size(138, 23);
			labelReadableDesignation03.TabIndex = 11;
			labelReadableDesignation03.ToolTipValues.Description = "Shows the readable designation no. 3";
			labelReadableDesignation03.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation03.ToolTipValues.Heading = "Readable designation no. 3";
			labelReadableDesignation03.Values.Text = "designation 03";
			labelReadableDesignation03.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation03.Enter += Control_Enter;
			labelReadableDesignation03.Leave += Control_Leave;
			labelReadableDesignation03.MouseEnter += Control_Enter;
			labelReadableDesignation03.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation04
			// 
			labelReadableDesignation04.AccessibleDescription = "Shows the readable designation no. 4";
			labelReadableDesignation04.AccessibleName = "Readable designation no. 4";
			labelReadableDesignation04.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation04.Dock = DockStyle.Fill;
			labelReadableDesignation04.Location = new Point(36, 132);
			labelReadableDesignation04.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation04.Name = "labelReadableDesignation04";
			labelReadableDesignation04.Size = new Size(138, 23);
			labelReadableDesignation04.TabIndex = 15;
			labelReadableDesignation04.ToolTipValues.Description = "Shows the readable designation no. 4";
			labelReadableDesignation04.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation04.ToolTipValues.Heading = "Readable designation no. 4";
			labelReadableDesignation04.Values.Text = "designation 04";
			labelReadableDesignation04.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation04.Enter += Control_Enter;
			labelReadableDesignation04.Leave += Control_Leave;
			labelReadableDesignation04.MouseEnter += Control_Enter;
			labelReadableDesignation04.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation05
			// 
			labelReadableDesignation05.AccessibleDescription = "Shows the readable designation no. 5";
			labelReadableDesignation05.AccessibleName = "Readable designation no. 5";
			labelReadableDesignation05.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation05.Dock = DockStyle.Fill;
			labelReadableDesignation05.Location = new Point(36, 161);
			labelReadableDesignation05.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation05.Name = "labelReadableDesignation05";
			labelReadableDesignation05.Size = new Size(138, 23);
			labelReadableDesignation05.TabIndex = 19;
			labelReadableDesignation05.ToolTipValues.Description = "Shows the readable designation no. 5";
			labelReadableDesignation05.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation05.ToolTipValues.Heading = "Readable designation no. 5";
			labelReadableDesignation05.Values.Text = "designation 05";
			labelReadableDesignation05.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation05.Enter += Control_Enter;
			labelReadableDesignation05.Leave += Control_Leave;
			labelReadableDesignation05.MouseEnter += Control_Enter;
			labelReadableDesignation05.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation06
			// 
			labelReadableDesignation06.AccessibleDescription = "Shows the readable designation no. 6";
			labelReadableDesignation06.AccessibleName = "Readable designation no. 6";
			labelReadableDesignation06.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation06.Dock = DockStyle.Fill;
			labelReadableDesignation06.Location = new Point(36, 190);
			labelReadableDesignation06.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation06.Name = "labelReadableDesignation06";
			labelReadableDesignation06.Size = new Size(138, 23);
			labelReadableDesignation06.TabIndex = 23;
			labelReadableDesignation06.ToolTipValues.Description = "Shows the readable designation no. 6";
			labelReadableDesignation06.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation06.ToolTipValues.Heading = "Readable designation no. 6";
			labelReadableDesignation06.Values.Text = "designation 06";
			labelReadableDesignation06.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation06.Enter += Control_Enter;
			labelReadableDesignation06.Leave += Control_Leave;
			labelReadableDesignation06.MouseEnter += Control_Enter;
			labelReadableDesignation06.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation07
			// 
			labelReadableDesignation07.AccessibleDescription = "Shows the readable designation no. 7";
			labelReadableDesignation07.AccessibleName = "Readable designation no. 7";
			labelReadableDesignation07.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation07.Dock = DockStyle.Fill;
			labelReadableDesignation07.Location = new Point(36, 219);
			labelReadableDesignation07.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation07.Name = "labelReadableDesignation07";
			labelReadableDesignation07.Size = new Size(138, 23);
			labelReadableDesignation07.TabIndex = 27;
			labelReadableDesignation07.ToolTipValues.Description = "Shows the readable designation no. 7";
			labelReadableDesignation07.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation07.ToolTipValues.Heading = "Readable designation no. 7";
			labelReadableDesignation07.Values.Text = "designation 07";
			labelReadableDesignation07.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation07.Enter += Control_Enter;
			labelReadableDesignation07.Leave += Control_Leave;
			labelReadableDesignation07.MouseEnter += Control_Enter;
			labelReadableDesignation07.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation08
			// 
			labelReadableDesignation08.AccessibleDescription = "Shows the readable designation no. 8";
			labelReadableDesignation08.AccessibleName = "Readable designation no. 8";
			labelReadableDesignation08.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation08.Dock = DockStyle.Fill;
			labelReadableDesignation08.Location = new Point(36, 248);
			labelReadableDesignation08.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation08.Name = "labelReadableDesignation08";
			labelReadableDesignation08.Size = new Size(138, 23);
			labelReadableDesignation08.TabIndex = 31;
			labelReadableDesignation08.ToolTipValues.Description = "Shows the readable designation no. 8";
			labelReadableDesignation08.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation08.ToolTipValues.Heading = "Readable designation no. 8";
			labelReadableDesignation08.Values.Text = "designation 08";
			labelReadableDesignation08.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation08.Enter += Control_Enter;
			labelReadableDesignation08.Leave += Control_Leave;
			labelReadableDesignation08.MouseEnter += Control_Enter;
			labelReadableDesignation08.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation09
			// 
			labelReadableDesignation09.AccessibleDescription = "Shows the readable designation no. 9";
			labelReadableDesignation09.AccessibleName = "Readable designation no. 9";
			labelReadableDesignation09.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation09.Dock = DockStyle.Fill;
			labelReadableDesignation09.Location = new Point(36, 277);
			labelReadableDesignation09.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation09.Name = "labelReadableDesignation09";
			labelReadableDesignation09.Size = new Size(138, 23);
			labelReadableDesignation09.TabIndex = 35;
			labelReadableDesignation09.ToolTipValues.Description = "Shows the readable designation no. 9";
			labelReadableDesignation09.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation09.ToolTipValues.Heading = "Readable designation no. 9";
			labelReadableDesignation09.Values.Text = "designation 09";
			labelReadableDesignation09.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation09.Enter += Control_Enter;
			labelReadableDesignation09.Leave += Control_Leave;
			labelReadableDesignation09.MouseEnter += Control_Enter;
			labelReadableDesignation09.MouseLeave += Control_Leave;
			// 
			// labelReadableDesignation10
			// 
			labelReadableDesignation10.AccessibleDescription = "Shows the readable designation no. 10";
			labelReadableDesignation10.AccessibleName = "Readable designation no. 10";
			labelReadableDesignation10.AccessibleRole = AccessibleRole.StaticText;
			labelReadableDesignation10.Dock = DockStyle.Fill;
			labelReadableDesignation10.Location = new Point(36, 306);
			labelReadableDesignation10.Margin = new Padding(4, 3, 4, 3);
			labelReadableDesignation10.Name = "labelReadableDesignation10";
			labelReadableDesignation10.Size = new Size(138, 42);
			labelReadableDesignation10.TabIndex = 39;
			labelReadableDesignation10.ToolTipValues.Description = "Shows the readable designation no. 10";
			labelReadableDesignation10.ToolTipValues.EnableToolTips = true;
			labelReadableDesignation10.ToolTipValues.Heading = "Readable designation no. 10";
			labelReadableDesignation10.Values.Text = "designation 10";
			labelReadableDesignation10.DoubleClick += CopyToClipboard_DoubleClick;
			labelReadableDesignation10.Enter += Control_Enter;
			labelReadableDesignation10.Leave += Control_Leave;
			labelReadableDesignation10.MouseEnter += Control_Enter;
			labelReadableDesignation10.MouseLeave += Control_Leave;
			// 
			// labelValue01
			// 
			labelValue01.AccessibleDescription = "Shows the value no. 1";
			labelValue01.AccessibleName = "Value no. 1";
			labelValue01.AccessibleRole = AccessibleRole.StaticText;
			labelValue01.Dock = DockStyle.Fill;
			labelValue01.Location = new Point(182, 45);
			labelValue01.Margin = new Padding(4, 3, 4, 3);
			labelValue01.Name = "labelValue01";
			labelValue01.Size = new Size(225, 23);
			labelValue01.TabIndex = 4;
			labelValue01.ToolTipValues.Description = "Shows the value no. 1";
			labelValue01.ToolTipValues.EnableToolTips = true;
			labelValue01.ToolTipValues.Heading = "Value no. 1";
			labelValue01.Values.Text = "value 01";
			labelValue01.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue01.Enter += Control_Enter;
			labelValue01.Leave += Control_Leave;
			labelValue01.MouseEnter += Control_Enter;
			labelValue01.MouseLeave += Control_Leave;
			// 
			// labelValue02
			// 
			labelValue02.AccessibleDescription = "Shows the value no. 2";
			labelValue02.AccessibleName = "Value no. 2";
			labelValue02.AccessibleRole = AccessibleRole.StaticText;
			labelValue02.Dock = DockStyle.Fill;
			labelValue02.Location = new Point(182, 74);
			labelValue02.Margin = new Padding(4, 3, 4, 3);
			labelValue02.Name = "labelValue02";
			labelValue02.Size = new Size(225, 23);
			labelValue02.TabIndex = 8;
			labelValue02.ToolTipValues.Description = "Shows the value no. 2";
			labelValue02.ToolTipValues.EnableToolTips = true;
			labelValue02.ToolTipValues.Heading = "Value no. 2";
			labelValue02.Values.Text = "value 02";
			labelValue02.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue02.Enter += Control_Enter;
			labelValue02.Leave += Control_Leave;
			labelValue02.MouseEnter += Control_Enter;
			labelValue02.MouseLeave += Control_Leave;
			// 
			// labelValue03
			// 
			labelValue03.AccessibleDescription = "Shows the value no. 3";
			labelValue03.AccessibleName = "Value no. 3";
			labelValue03.AccessibleRole = AccessibleRole.StaticText;
			labelValue03.Dock = DockStyle.Fill;
			labelValue03.Location = new Point(182, 103);
			labelValue03.Margin = new Padding(4, 3, 4, 3);
			labelValue03.Name = "labelValue03";
			labelValue03.Size = new Size(225, 23);
			labelValue03.TabIndex = 12;
			labelValue03.ToolTipValues.Description = "Shows the value no. 3";
			labelValue03.ToolTipValues.EnableToolTips = true;
			labelValue03.ToolTipValues.Heading = "Value no. 3";
			labelValue03.Values.Text = "value 03";
			labelValue03.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue03.Enter += Control_Enter;
			labelValue03.Leave += Control_Leave;
			labelValue03.MouseEnter += Control_Enter;
			labelValue03.MouseLeave += Control_Leave;
			// 
			// labelValue04
			// 
			labelValue04.AccessibleDescription = "Shows the value no. 4";
			labelValue04.AccessibleName = "Value no. 4";
			labelValue04.AccessibleRole = AccessibleRole.StaticText;
			labelValue04.Dock = DockStyle.Fill;
			labelValue04.Location = new Point(182, 132);
			labelValue04.Margin = new Padding(4, 3, 4, 3);
			labelValue04.Name = "labelValue04";
			labelValue04.Size = new Size(225, 23);
			labelValue04.TabIndex = 16;
			labelValue04.ToolTipValues.Description = "Shows the value no. 4";
			labelValue04.ToolTipValues.EnableToolTips = true;
			labelValue04.ToolTipValues.Heading = "Value no. 4";
			labelValue04.Values.Text = "value 04";
			labelValue04.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue04.Enter += Control_Enter;
			labelValue04.Leave += Control_Leave;
			labelValue04.MouseEnter += Control_Enter;
			labelValue04.MouseLeave += Control_Leave;
			// 
			// labelValue05
			// 
			labelValue05.AccessibleDescription = "Shows the value no. 5";
			labelValue05.AccessibleName = "Value no. 5";
			labelValue05.AccessibleRole = AccessibleRole.StaticText;
			labelValue05.Dock = DockStyle.Fill;
			labelValue05.Location = new Point(182, 161);
			labelValue05.Margin = new Padding(4, 3, 4, 3);
			labelValue05.Name = "labelValue05";
			labelValue05.Size = new Size(225, 23);
			labelValue05.TabIndex = 20;
			labelValue05.ToolTipValues.Description = "Shows the value no. 5";
			labelValue05.ToolTipValues.EnableToolTips = true;
			labelValue05.ToolTipValues.Heading = "Value no. 5";
			labelValue05.Values.Text = "value 05";
			labelValue05.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue05.Enter += Control_Enter;
			labelValue05.Leave += Control_Leave;
			labelValue05.MouseEnter += Control_Enter;
			labelValue05.MouseLeave += Control_Leave;
			// 
			// labelValue06
			// 
			labelValue06.AccessibleDescription = "Shows the value no. 6";
			labelValue06.AccessibleName = "Value no. 6";
			labelValue06.AccessibleRole = AccessibleRole.StaticText;
			labelValue06.Dock = DockStyle.Fill;
			labelValue06.Location = new Point(182, 190);
			labelValue06.Margin = new Padding(4, 3, 4, 3);
			labelValue06.Name = "labelValue06";
			labelValue06.Size = new Size(225, 23);
			labelValue06.TabIndex = 24;
			labelValue06.ToolTipValues.Description = "Shows the value no. 6";
			labelValue06.ToolTipValues.EnableToolTips = true;
			labelValue06.ToolTipValues.Heading = "Value no. 6";
			labelValue06.Values.Text = "value 06";
			labelValue06.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue06.Enter += Control_Enter;
			labelValue06.Leave += Control_Leave;
			labelValue06.MouseEnter += Control_Enter;
			labelValue06.MouseLeave += Control_Leave;
			// 
			// labelValue07
			// 
			labelValue07.AccessibleDescription = "Shows the value no. 7";
			labelValue07.AccessibleName = "Value no. 7";
			labelValue07.AccessibleRole = AccessibleRole.StaticText;
			labelValue07.Dock = DockStyle.Fill;
			labelValue07.Location = new Point(182, 219);
			labelValue07.Margin = new Padding(4, 3, 4, 3);
			labelValue07.Name = "labelValue07";
			labelValue07.Size = new Size(225, 23);
			labelValue07.TabIndex = 28;
			labelValue07.ToolTipValues.Description = "Shows the value no. 7";
			labelValue07.ToolTipValues.EnableToolTips = true;
			labelValue07.ToolTipValues.Heading = "Value no. 7";
			labelValue07.Values.Text = "value 07";
			labelValue07.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue07.Enter += Control_Enter;
			labelValue07.Leave += Control_Leave;
			labelValue07.MouseEnter += Control_Enter;
			labelValue07.MouseLeave += Control_Leave;
			// 
			// labelValue08
			// 
			labelValue08.AccessibleDescription = "Shows the value no. 8";
			labelValue08.AccessibleName = "Value no. 8";
			labelValue08.AccessibleRole = AccessibleRole.StaticText;
			labelValue08.Dock = DockStyle.Fill;
			labelValue08.Location = new Point(182, 248);
			labelValue08.Margin = new Padding(4, 3, 4, 3);
			labelValue08.Name = "labelValue08";
			labelValue08.Size = new Size(225, 23);
			labelValue08.TabIndex = 32;
			labelValue08.ToolTipValues.Description = "Shows the value no. 8";
			labelValue08.ToolTipValues.EnableToolTips = true;
			labelValue08.ToolTipValues.Heading = "Value no. 8";
			labelValue08.Values.Text = "value 08";
			labelValue08.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue08.Enter += Control_Enter;
			labelValue08.Leave += Control_Leave;
			labelValue08.MouseEnter += Control_Enter;
			labelValue08.MouseLeave += Control_Leave;
			// 
			// labelValue09
			// 
			labelValue09.AccessibleDescription = "Shows the value no. 9";
			labelValue09.AccessibleName = "Value no. 9";
			labelValue09.AccessibleRole = AccessibleRole.StaticText;
			labelValue09.Dock = DockStyle.Fill;
			labelValue09.Location = new Point(182, 277);
			labelValue09.Margin = new Padding(4, 3, 4, 3);
			labelValue09.Name = "labelValue09";
			labelValue09.Size = new Size(225, 23);
			labelValue09.TabIndex = 36;
			labelValue09.ToolTipValues.Description = "Shows the value no. 9";
			labelValue09.ToolTipValues.EnableToolTips = true;
			labelValue09.ToolTipValues.Heading = "Value no. 9";
			labelValue09.Values.Text = "value 09";
			labelValue09.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue09.Enter += Control_Enter;
			labelValue09.Leave += Control_Leave;
			labelValue09.MouseEnter += Control_Enter;
			labelValue09.MouseLeave += Control_Leave;
			// 
			// labelValue10
			// 
			labelValue10.AccessibleDescription = "Shows the value no. 10";
			labelValue10.AccessibleName = "Value no. 10";
			labelValue10.AccessibleRole = AccessibleRole.StaticText;
			labelValue10.Dock = DockStyle.Fill;
			labelValue10.Location = new Point(182, 306);
			labelValue10.Margin = new Padding(4, 3, 4, 3);
			labelValue10.Name = "labelValue10";
			labelValue10.Size = new Size(225, 42);
			labelValue10.TabIndex = 40;
			labelValue10.ToolTipValues.Description = "Shows the value no. 10";
			labelValue10.ToolTipValues.EnableToolTips = true;
			labelValue10.ToolTipValues.Heading = "Value no. 10";
			labelValue10.Values.Text = "value 10";
			labelValue10.DoubleClick += CopyToClipboard_DoubleClick;
			labelValue10.Enter += Control_Enter;
			labelValue10.Leave += Control_Leave;
			labelValue10.MouseEnter += Control_Enter;
			labelValue10.MouseLeave += Control_Leave;
			// 
			// buttonGoto10
			// 
			buttonGoto10.AccessibleDescription = "Goes to the element of the place no. 10";
			buttonGoto10.AccessibleName = "Go to the element of the place no. 10";
			buttonGoto10.AccessibleRole = AccessibleRole.PushButton;
			buttonGoto10.ButtonStyle = ButtonStyle.Form;
			buttonGoto10.Location = new Point(415, 306);
			buttonGoto10.Margin = new Padding(4, 3, 4, 3);
			buttonGoto10.Name = "buttonGoto10";
			buttonGoto10.Size = new Size(70, 23);
			buttonGoto10.TabIndex = 41;
			buttonGoto10.ToolTipValues.Description = "Goes to the element of the place no. 10";
			buttonGoto10.ToolTipValues.EnableToolTips = true;
			buttonGoto10.ToolTipValues.Heading = "Go to the element of the place no. 10";
			buttonGoto10.Values.DropDownArrowColor = Color.Empty;
			buttonGoto10.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonGoto10.Values.Text = "Goto";
			buttonGoto10.Click += ButtonGoto10_Click;
			buttonGoto10.Enter += Control_Enter;
			buttonGoto10.Leave += Control_Leave;
			buttonGoto10.MouseEnter += Control_Enter;
			buttonGoto10.MouseLeave += Control_Leave;
			// 
			// buttonStart
			// 
			buttonStart.AccessibleDescription = "Detects the records";
			buttonStart.AccessibleName = "Detect the records";
			buttonStart.AccessibleRole = AccessibleRole.PushButton;
			buttonStart.Location = new Point(14, 14);
			buttonStart.Margin = new Padding(4, 3, 4, 3);
			buttonStart.Name = "buttonStart";
			buttonStart.Size = new Size(85, 29);
			buttonStart.TabIndex = 0;
			buttonStart.ToolTipValues.Description = "Detects the records";
			buttonStart.ToolTipValues.EnableToolTips = true;
			buttonStart.ToolTipValues.Heading = "Start";
			buttonStart.Values.DropDownArrowColor = Color.Empty;
			buttonStart.Values.Image = FatcowIcons16px.fatcow_resultset_next_16px;
			buttonStart.Values.Text = "&Start";
			buttonStart.Click += ButtonStart_Click;
			buttonStart.Enter += Control_Enter;
			buttonStart.Leave += Control_Leave;
			buttonStart.MouseEnter += Control_Enter;
			buttonStart.MouseLeave += Control_Leave;
			// 
			// labelPercent
			// 
			labelPercent.AccessibleDescription = "Show the percent status of the record detection";
			labelPercent.AccessibleName = "Percent status of the record detection";
			labelPercent.AccessibleRole = AccessibleRole.StaticText;
			labelPercent.Location = new Point(458, 20);
			labelPercent.Margin = new Padding(4, 3, 4, 3);
			labelPercent.Name = "labelPercent";
			labelPercent.Size = new Size(44, 20);
			labelPercent.TabIndex = 2;
			labelPercent.ToolTipValues.Description = "Show the percent status of the record detection";
			labelPercent.ToolTipValues.EnableToolTips = true;
			labelPercent.ToolTipValues.Heading = "Percent status of the record detection";
			labelPercent.Values.Text = "100 %";
			labelPercent.Enter += Control_Enter;
			labelPercent.Leave += Control_Leave;
			labelPercent.MouseEnter += Control_Enter;
			labelPercent.MouseLeave += Control_Leave;
			// 
			// progressBar
			// 
			progressBar.AccessibleDescription = "Shows the progress status of the record detection";
			progressBar.AccessibleName = "Progress bar";
			progressBar.AccessibleRole = AccessibleRole.ProgressBar;
			progressBar.Location = new Point(106, 14);
			progressBar.Margin = new Padding(4, 3, 4, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(345, 29);
			progressBar.TabIndex = 1;
			progressBar.Text = "Label";
			progressBar.TextBackdropColor = Color.Empty;
			progressBar.TextShadowColor = Color.Empty;
			progressBar.MouseEnter += Control_Enter;
			progressBar.MouseLeave += Control_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 510);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(517, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 2;
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
			labelInformation.ToolTipText = "Show some information";
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// RecordsMainForm
			// 
			AccessibleDescription = "Shows the top ten records";
			AccessibleName = "Top ten records";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(517, 532);
			ControlBox = false;
			Controls.Add(statusStrip);
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "RecordsMainForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Top ten records";
			Load += RecordsMainForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			((ISupportInitialize)groupBoxExport.Panel).EndInit();
			groupBoxExport.Panel.ResumeLayout(false);
			((ISupportInitialize)groupBoxExport).EndInit();
			tableLayoutPanel.ResumeLayout(false);
			tableLayoutPanel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion
		private KryptonPanel panel;
		private KryptonLabel labelPercent;
		private KryptonProgressBar progressBar;
		private KryptonButton buttonStart;
		private KryptonTableLayoutPanel tableLayoutPanel;
		private KryptonLabel labelPlace10;
		private KryptonLabel labelPlace05;
		private KryptonLabel labelPlace04;
		private KryptonLabel labelPlace03;
		private KryptonLabel labelPlace01;
		private KryptonLabel labelPlace02;
		private KryptonLabel labelPlace06;
		private KryptonLabel labelPlace07;
		private KryptonLabel labelPlace08;
		private KryptonLabel labelPlace09;
		private KryptonButton buttonGoto03;
		private KryptonButton buttonGoto02;
		private KryptonButton buttonGoto01;
		private KryptonButton buttonGoto10;
		private KryptonButton buttonGoto09;
		private KryptonButton buttonGoto08;
		private KryptonButton buttonGoto07;
		private KryptonButton buttonGoto06;
		private KryptonButton buttonGoto05;
		private KryptonButton buttonGoto04;
		private KryptonLabel labelReadableDesignationHeader;
		private KryptonLabel labelValueHeader;
		private KryptonLabel labelReadableDesignation01;
		private KryptonLabel labelReadableDesignation02;
		private KryptonLabel labelReadableDesignation03;
		private KryptonLabel labelReadableDesignation04;
		private KryptonLabel labelReadableDesignation05;
		private KryptonLabel labelReadableDesignation06;
		private KryptonLabel labelReadableDesignation07;
		private KryptonLabel labelReadableDesignation08;
		private KryptonLabel labelReadableDesignation09;
		private KryptonLabel labelReadableDesignation10;
		private KryptonLabel labelValue01;
		private KryptonLabel labelValue02;
		private KryptonLabel labelValue03;
		private KryptonLabel labelValue04;
		private KryptonLabel labelValue05;
		private KryptonLabel labelValue06;
		private KryptonLabel labelValue07;
		private KryptonLabel labelValue08;
		private KryptonLabel labelValue09;
		private KryptonLabel labelValue10;
		private KryptonGroupBox groupBoxExport;
		private KryptonButton buttonExportAsJson;
		private KryptonButton buttonExportAsTxt;
		private KryptonButton buttonExportAsXml;
		private KryptonButton buttonExportAsHtml;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private KryptonManager kryptonManager;
	}
}