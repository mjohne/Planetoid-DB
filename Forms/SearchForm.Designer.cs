using System.ComponentModel;
using Krypton.Toolkit;

using Planetoid_DB.Resources;

namespace Planetoid_DB
{
	partial class SearchForm
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SearchForm));
			checkedListBox = new KryptonCheckedListBox();
			panel = new KryptonPanel();
			listView = new ListView();
			columnHeaderIndex = new ColumnHeader();
			columnHeaderIndexNo = new ColumnHeader();
			columnHeaderProperty = new ColumnHeader();
			columnHeaderValue = new ColumnHeader();
			buttonCancel = new KryptonButton();
			statusStrip = new KryptonStatusStrip();
			labelInformation = new ToolStripStatusLabel();
			labelEntriesFound = new KryptonLabel();
			progressBar = new KryptonProgressBar();
			groupBox = new KryptonGroupBox();
			buttonClear = new KryptonButton();
			textBox = new KryptonTextBox();
			buttonLoad = new KryptonButton();
			buttonUnmarkAll = new KryptonButton();
			buttonMarkAll = new KryptonButton();
			buttonSearch = new KryptonButton();
			toolTip = new ToolTip(components);
			backgroundWorker = new BackgroundWorker();
			kryptonManager = new KryptonManager(components);
			((ISupportInitialize)panel).BeginInit();
			panel.SuspendLayout();
			statusStrip.SuspendLayout();
			((ISupportInitialize)groupBox).BeginInit();
			((ISupportInitialize)groupBox.Panel).BeginInit();
			groupBox.Panel.SuspendLayout();
			SuspendLayout();
			// 
			// checkedListBox
			// 
			checkedListBox.AccessibleDescription = "Show the box with selectable orbital elements to search";
			checkedListBox.AccessibleName = "Box with selectable orbital elements";
			checkedListBox.AccessibleRole = AccessibleRole.CheckButton;
			checkedListBox.BackStyle = PaletteBackStyle.ControlRibbon;
			checkedListBox.CheckOnClick = true;
			checkedListBox.Items.AddRange(new object[] { "Index No.", "Readable designation", "Epoch (in packed form, .0 TT)", "Mean anomaly at the epoch (°)", "Argument of perihelion, J2000.0 (°)", "Longitude of the ascending node, J2000.0 (°)", "Inclination to the ecliptic, J2000.0 (°)", "Orbital eccentricity", "Mean daily motion (°/day)", "Semi-major axis (AU)", "Absolute magnitude, H", "Slope parameter, G", "Reference", "Number of oppositions", "Number of observations", "Observation span", "r.m.s. residual (\")", "Computer name", "4-hexdigit flags", "Date of last observation (YYYMMDD)" });
			checkedListBox.Location = new Point(4, 74);
			checkedListBox.Margin = new Padding(4, 3, 4, 3);
			checkedListBox.Name = "checkedListBox";
			checkedListBox.Size = new Size(266, 194);
			checkedListBox.TabIndex = 1;
			toolTip.SetToolTip(checkedListBox, "Box with selectable orbital elements");
			checkedListBox.ToolTipValues.Description = "Show the box with selectable orbital elements to search";
			checkedListBox.ToolTipValues.EnableToolTips = true;
			checkedListBox.ToolTipValues.Heading = "Box with selectable orbital elements";
			checkedListBox.Enter += Control_Enter;
			checkedListBox.Leave += Control_Leave;
			checkedListBox.MouseEnter += Control_Enter;
			checkedListBox.MouseLeave += Control_Leave;
			// 
			// panel
			// 
			panel.AccessibleDescription = "Panel of the search form";
			panel.AccessibleName = "Panel of the search form";
			panel.AccessibleRole = AccessibleRole.Pane;
			panel.Controls.Add(listView);
			panel.Controls.Add(buttonCancel);
			panel.Controls.Add(statusStrip);
			panel.Controls.Add(labelEntriesFound);
			panel.Controls.Add(progressBar);
			panel.Controls.Add(groupBox);
			panel.Controls.Add(buttonLoad);
			panel.Controls.Add(buttonUnmarkAll);
			panel.Controls.Add(buttonMarkAll);
			panel.Controls.Add(buttonSearch);
			panel.Controls.Add(checkedListBox);
			panel.Dock = DockStyle.Fill;
			panel.Location = new Point(0, 0);
			panel.Margin = new Padding(4, 3, 4, 3);
			panel.Name = "panel";
			panel.PanelBackStyle = PaletteBackStyle.FormMain;
			panel.Size = new Size(387, 525);
			panel.TabIndex = 0;
			// 
			// listView
			// 
			listView.AccessibleDescription = "Shows the search results";
			listView.AccessibleName = "Search results";
			listView.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderIndexNo, columnHeaderProperty, columnHeaderValue });
			listView.FullRowSelect = true;
			listView.GridLines = true;
			listView.Location = new Point(4, 300);
			listView.Name = "listView";
			listView.ShowItemToolTips = true;
			listView.Size = new Size(377, 200);
			listView.TabIndex = 9;
			toolTip.SetToolTip(listView, "Shows the search results");
			listView.UseCompatibleStateImageBehavior = false;
			listView.View = View.Details;
			listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
			listView.Enter += Control_Enter;
			listView.Leave += Control_Leave;
			listView.MouseEnter += Control_Enter;
			listView.MouseLeave += Control_Leave;
			// 
			// columnHeaderIndex
			// 
			columnHeaderIndex.Text = "Index";
			// 
			// columnHeaderIndexNo
			// 
			columnHeaderIndexNo.Text = "Index No.";
			columnHeaderIndexNo.Width = 70;
			// 
			// columnHeaderProperty
			// 
			columnHeaderProperty.Text = "Property";
			columnHeaderProperty.Width = 180;
			// 
			// columnHeaderValue
			// 
			columnHeaderValue.Text = "Value";
			columnHeaderValue.Width = 120;
			// 
			// buttonCancel
			// 
			buttonCancel.AccessibleDescription = "Cancels the search";
			buttonCancel.AccessibleName = "Cancel";
			buttonCancel.AccessibleRole = AccessibleRole.PushButton;
			buttonCancel.Location = new Point(276, 180);
			buttonCancel.Margin = new Padding(4, 3, 4, 3);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new Size(105, 29);
			buttonCancel.TabIndex = 5;
			toolTip.SetToolTip(buttonCancel, "Cancel");
			buttonCancel.ToolTipValues.Description = "Cancels the search";
			buttonCancel.ToolTipValues.EnableToolTips = true;
			buttonCancel.ToolTipValues.Heading = "Cancel";
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += Control_Enter;
			buttonCancel.Leave += Control_Leave;
			buttonCancel.MouseEnter += Control_Enter;
			buttonCancel.MouseLeave += Control_Leave;
			// 
			// statusStrip
			// 
			statusStrip.AccessibleDescription = "Shows some information";
			statusStrip.AccessibleName = "Status bar of some information";
			statusStrip.AccessibleRole = AccessibleRole.StatusBar;
			statusStrip.Font = new Font("Segoe UI", 9F);
			statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
			statusStrip.Location = new Point(0, 503);
			statusStrip.Name = "statusStrip";
			statusStrip.Padding = new Padding(1, 0, 16, 0);
			statusStrip.ProgressBars = null;
			statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
			statusStrip.Size = new Size(387, 22);
			statusStrip.SizingGrip = false;
			statusStrip.TabIndex = 10;
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
			// labelEntriesFound
			// 
			labelEntriesFound.AccessibleDescription = "Shows the found entries";
			labelEntriesFound.AccessibleName = "Found entries";
			labelEntriesFound.AccessibleRole = AccessibleRole.StaticText;
			labelEntriesFound.Location = new Point(255, 274);
			labelEntriesFound.Margin = new Padding(4, 3, 4, 3);
			labelEntriesFound.Name = "labelEntriesFound";
			labelEntriesFound.RightToLeft = RightToLeft.No;
			labelEntriesFound.Size = new Size(93, 20);
			labelEntriesFound.TabIndex = 8;
			toolTip.SetToolTip(labelEntriesFound, "Found entries");
			labelEntriesFound.ToolTipValues.Description = "Shows the found entries";
			labelEntriesFound.ToolTipValues.EnableToolTips = true;
			labelEntriesFound.ToolTipValues.Heading = "Found entries";
			labelEntriesFound.Values.Text = "0 entries found";
			labelEntriesFound.Enter += Control_Enter;
			labelEntriesFound.Leave += Control_Leave;
			labelEntriesFound.MouseEnter += Control_Enter;
			labelEntriesFound.MouseLeave += Control_Leave;
			// 
			// progressBar
			// 
			progressBar.AccessibleDescription = "Shows the progress status of the search";
			progressBar.AccessibleName = "Progress bar";
			progressBar.AccessibleRole = AccessibleRole.ProgressBar;
			progressBar.Location = new Point(4, 274);
			progressBar.Margin = new Padding(4, 3, 4, 3);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(243, 20);
			progressBar.TabIndex = 7;
			progressBar.TextBackdropColor = Color.Empty;
			progressBar.TextShadowColor = Color.Empty;
			toolTip.SetToolTip(progressBar, "Shows the progress status of the search");
			progressBar.Values.Text = "";
			progressBar.MouseEnter += Control_Enter;
			progressBar.MouseLeave += Control_Leave;
			// 
			// groupBox
			// 
			groupBox.AccessibleDescription = "Groups the search element";
			groupBox.AccessibleName = "Group the search element";
			groupBox.AccessibleRole = AccessibleRole.Grouping;
			groupBox.Location = new Point(4, 3);
			groupBox.Margin = new Padding(4, 3, 4, 3);
			// 
			// 
			// 
			groupBox.Panel.AccessibleDescription = "Groups the search element";
			groupBox.Panel.AccessibleName = "Group the search element";
			groupBox.Panel.AccessibleRole = AccessibleRole.Grouping;
			groupBox.Panel.AutoScroll = true;
			groupBox.Panel.Controls.Add(buttonClear);
			groupBox.Panel.Controls.Add(textBox);
			groupBox.Size = new Size(378, 63);
			groupBox.TabIndex = 0;
			toolTip.SetToolTip(groupBox, "Group the search element");
			groupBox.Values.Heading = "Word, number, keyword, ...";
			groupBox.Values.Image = FatcowIcons16px.fatcow_pencil_16px;
			groupBox.Enter += Control_Enter;
			groupBox.Leave += Control_Leave;
			groupBox.MouseEnter += Control_Enter;
			groupBox.MouseLeave += Control_Leave;
			// 
			// buttonClear
			// 
			buttonClear.AccessibleDescription = "Clears the search box";
			buttonClear.AccessibleName = "Clear";
			buttonClear.AccessibleRole = AccessibleRole.PushButton;
			buttonClear.ButtonStyle = ButtonStyle.Form;
			buttonClear.Location = new Point(300, 5);
			buttonClear.Margin = new Padding(4, 3, 4, 3);
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(68, 25);
			buttonClear.TabIndex = 1;
			toolTip.SetToolTip(buttonClear, "Clear the search box");
			buttonClear.ToolTipValues.Description = "Clears the search box";
			buttonClear.ToolTipValues.EnableToolTips = true;
			buttonClear.ToolTipValues.Heading = "Clear";
			buttonClear.Values.DropDownArrowColor = Color.Empty;
			buttonClear.Values.Image = FatcowIcons16px.fatcow_cross_16px;
			buttonClear.Values.Text = "&Clear";
			buttonClear.Click += ButtonClear_Click;
			buttonClear.Enter += Control_Enter;
			buttonClear.Leave += Control_Leave;
			buttonClear.MouseEnter += Control_Enter;
			buttonClear.MouseLeave += Control_Leave;
			// 
			// textBox
			// 
			textBox.AccessibleDescription = "Shows the search box to input some key words";
			textBox.AccessibleName = "Search box";
			textBox.AccessibleRole = AccessibleRole.Text;
			textBox.Location = new Point(5, 5);
			textBox.Margin = new Padding(4, 3, 4, 3);
			textBox.Name = "textBox";
			textBox.Size = new Size(288, 23);
			textBox.TabIndex = 0;
			toolTip.SetToolTip(textBox, "Search box");
			textBox.ToolTipValues.Description = "Shows the search box to input some key words";
			textBox.ToolTipValues.EnableToolTips = true;
			textBox.ToolTipValues.Heading = "Search box";
			textBox.TextChanged += TextBox_TextChanged;
			textBox.Enter += Control_Enter;
			textBox.Leave += Control_Leave;
			textBox.MouseEnter += Control_Enter;
			textBox.MouseLeave += Control_Leave;
			// 
			// buttonLoad
			// 
			buttonLoad.AccessibleDescription = "Loads the selected result item";
			buttonLoad.AccessibleName = "Load the selected result item";
			buttonLoad.AccessibleRole = AccessibleRole.PushButton;
			buttonLoad.DialogResult = DialogResult.OK;
			buttonLoad.Location = new Point(276, 239);
			buttonLoad.Margin = new Padding(4, 3, 4, 3);
			buttonLoad.Name = "buttonLoad";
			buttonLoad.Size = new Size(105, 29);
			buttonLoad.TabIndex = 6;
			toolTip.SetToolTip(buttonLoad, "Load the selected result item");
			buttonLoad.ToolTipValues.Description = "Loads the selected result item";
			buttonLoad.ToolTipValues.EnableToolTips = true;
			buttonLoad.ToolTipValues.Heading = "Load";
			buttonLoad.Values.DropDownArrowColor = Color.Empty;
			buttonLoad.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonLoad.Values.Text = "&Load";
			buttonLoad.Click += ButtonLoad_Click;
			buttonLoad.Enter += Control_Enter;
			buttonLoad.Leave += Control_Leave;
			buttonLoad.MouseEnter += Control_Enter;
			buttonLoad.MouseLeave += Control_Leave;
			// 
			// buttonUnmarkAll
			// 
			buttonUnmarkAll.AccessibleDescription = "Umarks all orbital elements";
			buttonUnmarkAll.AccessibleName = "Umark all orbital elements";
			buttonUnmarkAll.AccessibleRole = AccessibleRole.PushButton;
			buttonUnmarkAll.ButtonStyle = ButtonStyle.Form;
			buttonUnmarkAll.Location = new Point(276, 110);
			buttonUnmarkAll.Margin = new Padding(4, 3, 4, 3);
			buttonUnmarkAll.Name = "buttonUnmarkAll";
			buttonUnmarkAll.Size = new Size(105, 29);
			buttonUnmarkAll.TabIndex = 3;
			toolTip.SetToolTip(buttonUnmarkAll, "Heading");
			buttonUnmarkAll.ToolTipValues.Description = "Umarks all orbital elements";
			buttonUnmarkAll.ToolTipValues.EnableToolTips = true;
			buttonUnmarkAll.ToolTipValues.Heading = "Umark all";
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
			buttonMarkAll.AccessibleDescription = "Marks all orbital elements to search";
			buttonMarkAll.AccessibleName = "Mark all orbital elements";
			buttonMarkAll.AccessibleRole = AccessibleRole.PushButton;
			buttonMarkAll.ButtonStyle = ButtonStyle.Form;
			buttonMarkAll.Location = new Point(276, 74);
			buttonMarkAll.Margin = new Padding(4, 3, 4, 3);
			buttonMarkAll.Name = "buttonMarkAll";
			buttonMarkAll.Size = new Size(105, 29);
			buttonMarkAll.TabIndex = 2;
			toolTip.SetToolTip(buttonMarkAll, "Mark all");
			buttonMarkAll.ToolTipValues.Description = "Marks all orbital elements to search";
			buttonMarkAll.ToolTipValues.EnableToolTips = true;
			buttonMarkAll.ToolTipValues.Heading = "Mark all";
			buttonMarkAll.Values.DropDownArrowColor = Color.Empty;
			buttonMarkAll.Values.Image = FatcowIcons16px.fatcow_asterisk_orange_16px;
			buttonMarkAll.Values.Text = "&Mark all";
			buttonMarkAll.Click += ButtonMarkAll_Click;
			buttonMarkAll.Enter += Control_Enter;
			buttonMarkAll.Leave += Control_Leave;
			buttonMarkAll.MouseEnter += Control_Enter;
			buttonMarkAll.MouseLeave += Control_Leave;
			// 
			// buttonSearch
			// 
			buttonSearch.AccessibleDescription = "Searchs the keyword";
			buttonSearch.AccessibleName = "Search";
			buttonSearch.AccessibleRole = AccessibleRole.PushButton;
			buttonSearch.Location = new Point(276, 145);
			buttonSearch.Margin = new Padding(4, 3, 4, 3);
			buttonSearch.Name = "buttonSearch";
			buttonSearch.Size = new Size(105, 29);
			buttonSearch.TabIndex = 4;
			toolTip.SetToolTip(buttonSearch, "Search");
			buttonSearch.ToolTipValues.Description = "Searchs the keyword";
			buttonSearch.ToolTipValues.EnableToolTips = true;
			buttonSearch.ToolTipValues.Heading = "Search";
			buttonSearch.Values.DropDownArrowColor = Color.Empty;
			buttonSearch.Values.Image = FatcowIcons16px.fatcow_magnifier_16px;
			buttonSearch.Values.Text = "&Search";
			buttonSearch.Click += ButtonSearch_Click;
			buttonSearch.Enter += Control_Enter;
			buttonSearch.Leave += Control_Leave;
			buttonSearch.MouseEnter += Control_Enter;
			buttonSearch.MouseLeave += Control_Leave;
			// 
			// backgroundWorker
			// 
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
			// 
			// kryptonManager
			// 
			kryptonManager.GlobalPaletteMode = PaletteMode.Global;
			kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
			kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";
			// 
			// SearchForm
			// 
			AccessibleDescription = "Dialog to search a word, a keyword or a number";
			AccessibleName = "Search";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(387, 525);
			ControlBox = false;
			Controls.Add(panel);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "SearchForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Search";
			toolTip.SetToolTip(this, "Search");
			Load += SearchForm_Load;
			((ISupportInitialize)panel).EndInit();
			panel.ResumeLayout(false);
			panel.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((ISupportInitialize)groupBox.Panel).EndInit();
			groupBox.Panel.ResumeLayout(false);
			groupBox.Panel.PerformLayout();
			((ISupportInitialize)groupBox).EndInit();
			ResumeLayout(false);
		}

		#endregion
		private KryptonCheckedListBox checkedListBox;
		private KryptonPanel panel;
		private KryptonButton buttonUnmarkAll;
		private KryptonButton buttonMarkAll;
		private KryptonButton buttonSearch;
		private KryptonButton buttonLoad;
		private KryptonGroupBox groupBox;
		private KryptonTextBox textBox;
		private KryptonButton buttonClear;
		private KryptonLabel labelEntriesFound;
		private KryptonProgressBar progressBar;
		private KryptonStatusStrip statusStrip;
		private ToolStripStatusLabel labelInformation;
		private ToolTip toolTip;
		private BackgroundWorker backgroundWorker;
		private KryptonButton buttonCancel;
		private ListView listView;
		private ColumnHeader columnHeaderIndexNo;
		private ColumnHeader columnHeaderProperty;
		private ColumnHeader columnHeaderValue;
		private ColumnHeader columnHeaderIndex;
		private KryptonManager kryptonManager;
	}
}