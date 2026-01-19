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
			checkedListBox.AccessibleDescription = "Box with selectable orbital elements to search";
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
			checkedListBox.Enter += SetStatusBar_Enter;
			checkedListBox.Leave += ClearStatusBar_Leave;
			checkedListBox.MouseEnter += SetStatusBar_Enter;
			checkedListBox.MouseLeave += ClearStatusBar_Leave;
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
			listView.Enter += SetStatusBar_Enter;
			listView.Leave += ClearStatusBar_Leave;
			listView.MouseEnter += SetStatusBar_Enter;
			listView.MouseLeave += ClearStatusBar_Leave;
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
			buttonCancel.Values.DropDownArrowColor = Color.Empty;
			buttonCancel.Values.Image = FatcowIcons16px.fatcow_cancel_16px;
			buttonCancel.Values.Text = "&Cancel";
			buttonCancel.Click += ButtonCancel_Click;
			buttonCancel.Enter += SetStatusBar_Enter;
			buttonCancel.Leave += ClearStatusBar_Leave;
			buttonCancel.MouseEnter += SetStatusBar_Enter;
			buttonCancel.MouseLeave += ClearStatusBar_Leave;
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
			toolTip.SetToolTip(labelEntriesFound, "Shows the found entries");
			labelEntriesFound.Values.Text = "0 entries found";
			labelEntriesFound.Enter += SetStatusBar_Enter;
			labelEntriesFound.Leave += ClearStatusBar_Leave;
			labelEntriesFound.MouseEnter += SetStatusBar_Enter;
			labelEntriesFound.MouseLeave += ClearStatusBar_Leave;
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
			progressBar.MouseEnter += SetStatusBar_Enter;
			progressBar.MouseLeave += ClearStatusBar_Leave;
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
			groupBox.Enter += SetStatusBar_Enter;
			groupBox.Leave += ClearStatusBar_Leave;
			groupBox.MouseEnter += SetStatusBar_Enter;
			groupBox.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonClear
			// 
			buttonClear.AccessibleDescription = "Clears the search box";
			buttonClear.AccessibleName = "Clear the search box";
			buttonClear.AccessibleRole = AccessibleRole.PushButton;
			buttonClear.ButtonStyle = ButtonStyle.Form;
			buttonClear.Location = new Point(300, 5);
			buttonClear.Margin = new Padding(4, 3, 4, 3);
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(68, 25);
			buttonClear.TabIndex = 1;
			toolTip.SetToolTip(buttonClear, "Clear the search box");
			buttonClear.Values.DropDownArrowColor = Color.Empty;
			buttonClear.Values.Image = FatcowIcons16px.fatcow_cross_16px;
			buttonClear.Values.Text = "&Clear";
			buttonClear.Click += ButtonClear_Click;
			buttonClear.Enter += SetStatusBar_Enter;
			buttonClear.Leave += ClearStatusBar_Leave;
			buttonClear.MouseEnter += SetStatusBar_Enter;
			buttonClear.MouseLeave += ClearStatusBar_Leave;
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
			textBox.TextChanged += TextBox_TextChanged;
			textBox.Enter += SetStatusBar_Enter;
			textBox.Leave += ClearStatusBar_Leave;
			textBox.MouseEnter += SetStatusBar_Enter;
			textBox.MouseLeave += ClearStatusBar_Leave;
			// 
			// buttonLoad
			// 
			buttonLoad.AccessibleDescription = "Load the selected result item";
			buttonLoad.AccessibleName = "Load the selected result item";
			buttonLoad.AccessibleRole = AccessibleRole.PushButton;
			buttonLoad.DialogResult = DialogResult.OK;
			buttonLoad.Location = new Point(276, 239);
			buttonLoad.Margin = new Padding(4, 3, 4, 3);
			buttonLoad.Name = "buttonLoad";
			buttonLoad.Size = new Size(105, 29);
			buttonLoad.TabIndex = 6;
			toolTip.SetToolTip(buttonLoad, "Load the selected result item");
			buttonLoad.Values.DropDownArrowColor = Color.Empty;
			buttonLoad.Values.Image = FatcowIcons16px.fatcow_bullet_go_16px;
			buttonLoad.Values.Text = "&Load";
			buttonLoad.Click += ButtonLoad_Click;
			buttonLoad.Enter += SetStatusBar_Enter;
			buttonLoad.Leave += ClearStatusBar_Leave;
			buttonLoad.MouseEnter += SetStatusBar_Enter;
			buttonLoad.MouseLeave += ClearStatusBar_Leave;
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
			toolTip.SetToolTip(buttonUnmarkAll, "Umark all orbital elements");
			buttonUnmarkAll.Values.DropDownArrowColor = Color.Empty;
			buttonUnmarkAll.Values.Text = "&Unmark all";
			buttonUnmarkAll.Click += ButtonUnmarkAll_Click;
			buttonUnmarkAll.Enter += SetStatusBar_Enter;
			buttonUnmarkAll.Leave += ClearStatusBar_Leave;
			buttonUnmarkAll.MouseEnter += SetStatusBar_Enter;
			buttonUnmarkAll.MouseLeave += ClearStatusBar_Leave;
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
			toolTip.SetToolTip(buttonMarkAll, "Mark all orbital elements");
			buttonMarkAll.Values.DropDownArrowColor = Color.Empty;
			buttonMarkAll.Values.Image = FatcowIcons16px.fatcow_asterisk_orange_16px;
			buttonMarkAll.Values.Text = "&Mark all";
			buttonMarkAll.Click += ButtonMarkAll_Click;
			buttonMarkAll.Enter += SetStatusBar_Enter;
			buttonMarkAll.Leave += ClearStatusBar_Leave;
			buttonMarkAll.MouseEnter += SetStatusBar_Enter;
			buttonMarkAll.MouseLeave += ClearStatusBar_Leave;
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
			buttonSearch.Values.DropDownArrowColor = Color.Empty;
			buttonSearch.Values.Image = FatcowIcons16px.fatcow_magnifier_16px;
			buttonSearch.Values.Text = "&Search";
			buttonSearch.Click += ButtonSearch_Click;
			buttonSearch.Enter += SetStatusBar_Enter;
			buttonSearch.Leave += ClearStatusBar_Leave;
			buttonSearch.MouseEnter += SetStatusBar_Enter;
			buttonSearch.MouseLeave += ClearStatusBar_Leave;
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
			// 
			// SearchForm
			// 
			AccessibleDescription = "Dialog to search a word, a keyword or a number";
			AccessibleName = "Search";
			AccessibleRole = AccessibleRole.Dialog;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(387, 525);
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
			FormClosed += SearchForm_FormClosed;
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