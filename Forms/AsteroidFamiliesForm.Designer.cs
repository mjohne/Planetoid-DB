using System.ComponentModel;
using Krypton.Toolkit;

namespace Planetoid_DB.Forms;

partial class AsteroidFamiliesForm
{
	private IContainer components = null;

	/// <summary>
	/// Cleans up any resources being used.
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

	private void InitializeComponent()
	{
		kryptonPanel1 = new KryptonPanel();
		btnStart = new KryptonButton();
		btnCancel = new KryptonButton();
		btnSaveSelected = new KryptonButton();
		btnSaveAll = new KryptonButton();
		lblTolA = new KryptonLabel();
		numTolA = new KryptonNumericUpDown();
		lblTolE = new KryptonLabel();
		numTolE = new KryptonNumericUpDown();
		lblTolI = new KryptonLabel();
		numTolI = new KryptonNumericUpDown();
		lblMinMembers = new KryptonLabel();
		numMinMembers = new KryptonNumericUpDown();
		progressBar = new KryptonProgressBar();
		lblProgress = new KryptonLabel();
		splitContainer = new KryptonSplitContainer();
		treeViewFamilies = new TreeView();
		listViewMembers = new ListView();
		colIndex = new ColumnHeader();
		colName = new ColumnHeader();
		colSemiMajorAxis = new ColumnHeader();
		colEccentricity = new ColumnHeader();
		colInclination = new ColumnHeader();
		colMeanAnomaly = new ColumnHeader();
		colArgPeri = new ColumnHeader();
		colLongAscNode = new ColumnHeader();
		((ISupportInitialize)kryptonPanel1).BeginInit();
		kryptonPanel1.SuspendLayout();
		((ISupportInitialize)splitContainer).BeginInit();
		(splitContainer.Panel1).BeginInit();
		splitContainer.Panel1.SuspendLayout();
		(splitContainer.Panel2).BeginInit();
		splitContainer.Panel2.SuspendLayout();
		SuspendLayout();
		//
		// kryptonPanel1
		//
		kryptonPanel1.Controls.Add(btnStart);
		kryptonPanel1.Controls.Add(btnCancel);
		kryptonPanel1.Controls.Add(btnSaveSelected);
		kryptonPanel1.Controls.Add(btnSaveAll);
		kryptonPanel1.Controls.Add(lblTolA);
		kryptonPanel1.Controls.Add(numTolA);
		kryptonPanel1.Controls.Add(lblTolE);
		kryptonPanel1.Controls.Add(numTolE);
		kryptonPanel1.Controls.Add(lblTolI);
		kryptonPanel1.Controls.Add(numTolI);
		kryptonPanel1.Controls.Add(lblMinMembers);
		kryptonPanel1.Controls.Add(numMinMembers);
		kryptonPanel1.Controls.Add(progressBar);
		kryptonPanel1.Controls.Add(lblProgress);
		kryptonPanel1.Controls.Add(splitContainer);
		kryptonPanel1.Dock = DockStyle.Fill;
		kryptonPanel1.Location = new Point(0, 0);
		kryptonPanel1.Name = "kryptonPanel1";
		kryptonPanel1.Size = new Size(1100, 650);
		kryptonPanel1.TabIndex = 0;
		//
		// btnStart
		//
		btnStart.Location = new Point(12, 12);
		btnStart.Name = "btnStart";
		btnStart.Size = new Size(90, 25);
		btnStart.TabIndex = 0;
		btnStart.Values.DropDownArrowColor = Color.Empty;
		btnStart.Values.Text = "&Start Search";
		btnStart.Click += BtnStart_Click;
		//
		// btnCancel
		//
		btnCancel.Enabled = false;
		btnCancel.Location = new Point(108, 12);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new Size(90, 25);
		btnCancel.TabIndex = 1;
		btnCancel.Values.DropDownArrowColor = Color.Empty;
		btnCancel.Values.Text = "&Cancel";
		btnCancel.Click += BtnCancel_Click;
		//
		// lblTolA
		//
		lblTolA.Location = new Point(210, 15);
		lblTolA.Name = "lblTolA";
		lblTolA.Size = new Size(55, 20);
		lblTolA.TabIndex = 2;
		lblTolA.Values.Text = "Tol. a:";
		//
		// numTolA
		//
		numTolA.DecimalPlaces = 2;
		numTolA.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
		numTolA.Location = new Point(270, 14);
		numTolA.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
		numTolA.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
		numTolA.Name = "numTolA";
		numTolA.Size = new Size(65, 22);
		numTolA.TabIndex = 3;
		numTolA.Value = new decimal(new int[] { 5, 0, 0, 131072 });
		//
		// lblTolE
		//
		lblTolE.Location = new Point(345, 15);
		lblTolE.Name = "lblTolE";
		lblTolE.Size = new Size(50, 20);
		lblTolE.TabIndex = 4;
		lblTolE.Values.Text = "Tol. e:";
		//
		// numTolE
		//
		numTolE.DecimalPlaces = 2;
		numTolE.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
		numTolE.Location = new Point(400, 14);
		numTolE.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
		numTolE.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
		numTolE.Name = "numTolE";
		numTolE.Size = new Size(65, 22);
		numTolE.TabIndex = 5;
		numTolE.Value = new decimal(new int[] { 2, 0, 0, 131072 });
		//
		// lblTolI
		//
		lblTolI.Location = new Point(475, 15);
		lblTolI.Name = "lblTolI";
		lblTolI.Size = new Size(50, 20);
		lblTolI.TabIndex = 6;
		lblTolI.Values.Text = "Tol. i:";
		//
		// numTolI
		//
		numTolI.DecimalPlaces = 1;
		numTolI.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
		numTolI.Location = new Point(530, 14);
		numTolI.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
		numTolI.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
		numTolI.Name = "numTolI";
		numTolI.Size = new Size(65, 22);
		numTolI.TabIndex = 7;
		numTolI.Value = new decimal(new int[] { 20, 0, 0, 65536 });
		//
		// lblMinMembers
		//
		lblMinMembers.Location = new Point(605, 15);
		lblMinMembers.Name = "lblMinMembers";
		lblMinMembers.Size = new Size(82, 20);
		lblMinMembers.TabIndex = 8;
		lblMinMembers.Values.Text = "Min. Members:";
		//
		// numMinMembers
		//
		numMinMembers.Increment = new decimal(new int[] { 1, 0, 0, 0 });
		numMinMembers.Location = new Point(695, 14);
		numMinMembers.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
		numMinMembers.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
		numMinMembers.Name = "numMinMembers";
		numMinMembers.Size = new Size(60, 22);
		numMinMembers.TabIndex = 9;
		numMinMembers.Value = new decimal(new int[] { 3, 0, 0, 0 });
		//
		// btnSaveSelected
		//
		btnSaveSelected.Enabled = false;
		btnSaveSelected.Location = new Point(770, 12);
		btnSaveSelected.Name = "btnSaveSelected";
		btnSaveSelected.Size = new Size(130, 25);
		btnSaveSelected.TabIndex = 10;
		btnSaveSelected.Values.DropDownArrowColor = Color.Empty;
		btnSaveSelected.Values.Text = "Save &Selected";
		btnSaveSelected.Click += BtnSaveSelected_Click;
		//
		// btnSaveAll
		//
		btnSaveAll.Enabled = false;
		btnSaveAll.Location = new Point(908, 12);
		btnSaveAll.Name = "btnSaveAll";
		btnSaveAll.Size = new Size(90, 25);
		btnSaveAll.TabIndex = 11;
		btnSaveAll.Values.DropDownArrowColor = Color.Empty;
		btnSaveAll.Values.Text = "Save &All";
		btnSaveAll.Click += BtnSaveAll_Click;
		//
		// progressBar
		//
		progressBar.Location = new Point(12, 50);
		progressBar.Name = "progressBar";
		progressBar.Size = new Size(1040, 23);
		progressBar.TabIndex = 12;
		progressBar.Text = string.Empty;
		progressBar.TextBackdropColor = Color.Empty;
		progressBar.TextShadowColor = Color.Empty;
		//
		// lblProgress
		//
		lblProgress.Location = new Point(1058, 51);
		lblProgress.Name = "lblProgress";
		lblProgress.Size = new Size(30, 20);
		lblProgress.TabIndex = 13;
		lblProgress.Values.Text = "0%";
		//
		// splitContainer
		//
		splitContainer.AccessibleDescription = "Splits the view between the asteroid families tree and the member list";
		splitContainer.AccessibleName = "Asteroid families split view";
		splitContainer.AccessibleRole = AccessibleRole.Pane;
		splitContainer.Location = new Point(12, 84);
		splitContainer.Name = "splitContainer";
		splitContainer.Panel1.Controls.Add(treeViewFamilies);
		splitContainer.Panel2.Controls.Add(listViewMembers);
		splitContainer.Size = new Size(1076, 554);
		splitContainer.SplitterDistance = 270;
		splitContainer.TabIndex = 14;
		//
		// treeViewFamilies
		//
		treeViewFamilies.Dock = DockStyle.Fill;
		treeViewFamilies.Location = new Point(0, 0);
		treeViewFamilies.Name = "treeViewFamilies";
		treeViewFamilies.Size = new Size(270, 554);
		treeViewFamilies.TabIndex = 0;
		treeViewFamilies.AfterSelect += TreeViewFamilies_AfterSelect;
		//
		// listViewMembers
		//
		listViewMembers.Columns.AddRange(new ColumnHeader[] { colIndex, colName, colSemiMajorAxis, colEccentricity, colInclination, colMeanAnomaly, colArgPeri, colLongAscNode });
		listViewMembers.Dock = DockStyle.Fill;
		listViewMembers.FullRowSelect = true;
		listViewMembers.GridLines = true;
		listViewMembers.Location = new Point(0, 0);
		listViewMembers.Name = "listViewMembers";
		listViewMembers.Size = new Size(800, 554);
		listViewMembers.TabIndex = 0;
		listViewMembers.UseCompatibleStateImageBehavior = false;
		listViewMembers.View = View.Details;
		listViewMembers.VirtualMode = true;
		listViewMembers.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
		//
		// colIndex
		//
		colIndex.Text = "Index";
		colIndex.Width = 80;
		//
		// colName
		//
		colName.Text = "Name";
		colName.Width = 200;
		//
		// colSemiMajorAxis
		//
		colSemiMajorAxis.Text = "Semi-Major Axis (AU)";
		colSemiMajorAxis.Width = 130;
		//
		// colEccentricity
		//
		colEccentricity.Text = "Eccentricity";
		colEccentricity.Width = 90;
		//
		// colInclination
		//
		colInclination.Text = "Inclination (°)";
		colInclination.Width = 100;
		//
		// colMeanAnomaly
		//
		colMeanAnomaly.Text = "Mean Anomaly (°)";
		colMeanAnomaly.Width = 115;
		//
		// colArgPeri
		//
		colArgPeri.Text = "Arg. Perihelion (°)";
		colArgPeri.Width = 120;
		//
		// colLongAscNode
		//
		colLongAscNode.Text = "Long. Asc. Node (°)";
		colLongAscNode.Width = 120;
		//
		// AsteroidFamiliesForm
		//
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1100, 650);
		Controls.Add(kryptonPanel1);
		MinimumSize = new Size(800, 500);
		Name = "AsteroidFamiliesForm";
		Text = "Asteroid Families Detection";
		FormClosing += AsteroidFamiliesForm_FormClosing;
		((ISupportInitialize)kryptonPanel1).EndInit();
		kryptonPanel1.ResumeLayout(false);
		(splitContainer.Panel1).EndInit();
		splitContainer.Panel1.ResumeLayout(false);
		(splitContainer.Panel2).EndInit();
		splitContainer.Panel2.ResumeLayout(false);
		((ISupportInitialize)splitContainer).EndInit();
		ResumeLayout(false);
	}

	private KryptonPanel kryptonPanel1;
	private KryptonButton btnStart;
	private KryptonButton btnCancel;
	private KryptonButton btnSaveSelected;
	private KryptonButton btnSaveAll;
	private KryptonLabel lblTolA;
	private KryptonNumericUpDown numTolA;
	private KryptonLabel lblTolE;
	private KryptonNumericUpDown numTolE;
	private KryptonLabel lblTolI;
	private KryptonNumericUpDown numTolI;
	private KryptonLabel lblMinMembers;
	private KryptonNumericUpDown numMinMembers;
	private KryptonProgressBar progressBar;
	private KryptonLabel lblProgress;
	private KryptonSplitContainer splitContainer;
	private TreeView treeViewFamilies;
	private ListView listViewMembers;
	private ColumnHeader colIndex;
	private ColumnHeader colName;
	private ColumnHeader colSemiMajorAxis;
	private ColumnHeader colEccentricity;
	private ColumnHeader colInclination;
	private ColumnHeader colMeanAnomaly;
	private ColumnHeader colArgPeri;
	private ColumnHeader colLongAscNode;
}
