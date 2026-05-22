// This file contains the designer-generated code for the HistogramForm.

using Krypton.Toolkit;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Partial class containing the designer-generated code for HistogramForm.</summary>
partial class HistogramForm
{
	/// <summary>Required designer variable.</summary>
	private IContainer components = null;

	/// <summary>Releases all resources used by the HistogramForm.</summary>
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(HistogramForm));

		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanel = new KryptonPanel();
		splitContainer = new KryptonSplitContainer();
		panelChart = new KryptonPanel();
		listView = new ListView();
		columnHeaderRange = new ColumnHeader();
		columnHeaderCount = new ColumnHeader();
		toolStrip = new ToolStrip();
		toolStripButtonStart = new ToolStripButton();
		toolStripButtonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripLabel1 = new ToolStripLabel();
		comboBoxProperty = new ToolStripComboBox();
		toolStripLabel2 = new ToolStripLabel();
		comboBoxBinSize = new ToolStripComboBox();
		toolStripSeparator2 = new ToolStripSeparator();
		checkBoxLiveUpdate = new ToolStripButton();
		toolStripSeparator3 = new ToolStripSeparator();
		kryptonProgressBar = new KryptonProgressBarToolStripItem();

		toolStripContainer.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		((ISupportInitialize)kryptonPanel).BeginInit();
		kryptonPanel.SuspendLayout();
		((ISupportInitialize)splitContainer).BeginInit();
		((ISupportInitialize)splitContainer.Panel1).BeginInit();
		splitContainer.Panel1.SuspendLayout();
		((ISupportInitialize)splitContainer.Panel2).BeginInit();
		splitContainer.Panel2.SuspendLayout();
		splitContainer.SuspendLayout();
		((ISupportInitialize)panelChart).BeginInit();
		toolStrip.SuspendLayout();
		SuspendLayout();

		// toolStripContainer
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanel);
		toolStripContainer.ContentPanel.Size = new Size(1000, 550);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);

		// kryptonStatusStrip
		kryptonStatusStrip.Dock = DockStyle.Bottom;
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 575);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.Size = new Size(1000, 25);

		// labelInformation
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(985, 20);
		labelInformation.Spring = true;
		labelInformation.TextAlign = ContentAlignment.MiddleLeft;

		// kryptonPanel
		kryptonPanel.Controls.Add(splitContainer);
		kryptonPanel.Dock = DockStyle.Fill;
		kryptonPanel.Location = new Point(0, 0);
		kryptonPanel.Name = "kryptonPanel";
		kryptonPanel.Size = new Size(1000, 550);

		// splitContainer
		splitContainer.Cursor = Cursors.Default;
		splitContainer.Dock = DockStyle.Fill;
		splitContainer.Location = new Point(0, 0);
		splitContainer.Name = "splitContainer";
		splitContainer.Orientation = Orientation.Horizontal;
		splitContainer.Panel1.Controls.Add(panelChart);
		splitContainer.Panel2.Controls.Add(listView);
		splitContainer.Size = new Size(1000, 550);
		splitContainer.SplitterDistance = 350;

		// panelChart
		panelChart.Dock = DockStyle.Fill;
		panelChart.Location = new Point(0, 0);
		panelChart.Name = "panelChart";
		panelChart.Size = new Size(1000, 350);

		// listView
		listView.Columns.AddRange(new ColumnHeader[] { columnHeaderRange, columnHeaderCount });
		listView.Dock = DockStyle.Fill;
		listView.FullRowSelect = true;
		listView.GridLines = true;
		listView.Location = new Point(0, 0);
		listView.Name = "listView";
		listView.Size = new Size(1000, 195);
		listView.TabIndex = 0;
		listView.UseCompatibleStateImageBehavior = false;
		listView.View = View.Details;
		listView.ColumnClick += ListView_ColumnClick;

		// columnHeaderRange
		columnHeaderRange.Tag = "Range";
		columnHeaderRange.Text = "Range";
		columnHeaderRange.Width = 200;

		// columnHeaderCount
		columnHeaderCount.Tag = "Count";
		columnHeaderCount.Text = "Count";
		columnHeaderCount.Width = 150;

		// toolStrip
		toolStrip.Dock = DockStyle.None;
		toolStrip.GripStyle = ToolStripGripStyle.Hidden;
		toolStrip.Items.AddRange(new ToolStripItem[] {
			toolStripButtonStart,
			toolStripButtonCancel,
			toolStripSeparator1,
			toolStripLabel1,
			comboBoxProperty,
			toolStripLabel2,
			comboBoxBinSize,
			toolStripSeparator2,
			checkBoxLiveUpdate,
			toolStripSeparator3,
			kryptonProgressBar
		});
		toolStrip.Location = new Point(0, 0);
		toolStrip.Name = "toolStrip";
		toolStrip.Size = new Size(1000, 25);

		// toolStripButtonStart
		toolStripButtonStart.DisplayStyle = ToolStripItemDisplayStyle.Image;
		toolStripButtonStart.Image = Properties.FugueIcons16px.control;
		toolStripButtonStart.Name = "toolStripButtonStart";
		toolStripButtonStart.Size = new Size(23, 22);
		toolStripButtonStart.Text = "Start";
		toolStripButtonStart.AccessibleDescription = "Start the histogram calculation";
		toolStripButtonStart.AccessibleName = "Start";
		toolStripButtonStart.Click += ToolStripButtonStart_Click;

		// toolStripButtonCancel
		toolStripButtonCancel.DisplayStyle = ToolStripItemDisplayStyle.Image;
		toolStripButtonCancel.Enabled = false;
		toolStripButtonCancel.Image = Properties.FugueIcons16px.control_stop_square;
		toolStripButtonCancel.Name = "toolStripButtonCancel";
		toolStripButtonCancel.Size = new Size(23, 22);
		toolStripButtonCancel.Text = "Cancel";
		toolStripButtonCancel.AccessibleDescription = "Cancel the histogram calculation";
		toolStripButtonCancel.AccessibleName = "Cancel";
		toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

		// toolStripSeparator1
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(6, 25);

		// toolStripLabel1
		toolStripLabel1.Name = "toolStripLabel1";
		toolStripLabel1.Size = new Size(55, 22);
		toolStripLabel1.Text = "Property:";

		// comboBoxProperty
		comboBoxProperty.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxProperty.Name = "comboBoxProperty";
		comboBoxProperty.Size = new Size(250, 25);
		comboBoxProperty.AccessibleDescription = "Select the property to histogram";
		comboBoxProperty.AccessibleName = "Property";

		// toolStripLabel2
		toolStripLabel2.Name = "toolStripLabel2";
		toolStripLabel2.Size = new Size(52, 22);
		toolStripLabel2.Text = "Bin Size:";

		// comboBoxBinSize
		comboBoxBinSize.Name = "comboBoxBinSize";
		comboBoxBinSize.Size = new Size(100, 25);
		comboBoxBinSize.AccessibleDescription = "Enter or select the bin size";
		comboBoxBinSize.AccessibleName = "Bin Size";

		// toolStripSeparator2
		toolStripSeparator2.Name = "toolStripSeparator2";
		toolStripSeparator2.Size = new Size(6, 25);

		// checkBoxLiveUpdate
		checkBoxLiveUpdate.CheckOnClick = true;
		checkBoxLiveUpdate.DisplayStyle = ToolStripItemDisplayStyle.Text;
		checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
		checkBoxLiveUpdate.Size = new Size(75, 22);
		checkBoxLiveUpdate.Text = "Live Update";
		checkBoxLiveUpdate.AccessibleDescription = "Enable live update during calculation";
		checkBoxLiveUpdate.AccessibleName = "Live Update";

		// toolStripSeparator3
		toolStripSeparator3.Name = "toolStripSeparator3";
		toolStripSeparator3.Size = new Size(6, 25);

		// kryptonProgressBar
		kryptonProgressBar.Name = "kryptonProgressBar";
		kryptonProgressBar.Size = new Size(200, 22);
		kryptonProgressBar.Maximum = 100;
		kryptonProgressBar.Minimum = 0;
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.DisplayStyle = KryptonProgressBarToolStripItem.ProgressBarDisplayStyle.Text;

		// HistogramForm
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1000, 600);
		Controls.Add(toolStripContainer);
		Controls.Add(kryptonStatusStrip);
		Name = "HistogramForm";
		Text = "Histogram of Orbital Elements";

		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((ISupportInitialize)kryptonPanel).EndInit();
		kryptonPanel.ResumeLayout(false);
		((ISupportInitialize)splitContainer.Panel1).EndInit();
		splitContainer.Panel1.ResumeLayout(false);
		((ISupportInitialize)splitContainer.Panel2).EndInit();
		splitContainer.Panel2.ResumeLayout(false);
		((ISupportInitialize)splitContainer).EndInit();
		splitContainer.ResumeLayout(false);
		((ISupportInitialize)panelChart).EndInit();
		toolStrip.ResumeLayout(false);
		toolStrip.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private ToolStripContainer toolStripContainer;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonPanel kryptonPanel;
	private KryptonSplitContainer splitContainer;
	private KryptonPanel panelChart;
	private ListView listView;
	private ColumnHeader columnHeaderRange;
	private ColumnHeader columnHeaderCount;
	private ToolStrip toolStrip;
	private ToolStripButton toolStripButtonStart;
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripLabel toolStripLabel1;
	private ToolStripComboBox comboBoxProperty;
	private ToolStripLabel toolStripLabel2;
	private ToolStripComboBox comboBoxBinSize;
	private ToolStripSeparator toolStripSeparator2;
	private ToolStripButton checkBoxLiveUpdate;
	private ToolStripSeparator toolStripSeparator3;
	private KryptonProgressBarToolStripItem kryptonProgressBar;
}
