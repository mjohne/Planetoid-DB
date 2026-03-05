using System.ComponentModel;
using Krypton.Toolkit;

namespace Planetoid_DB.Forms;

partial class DatabaseDifferencesForm
{
    private IContainer components = null;

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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(DatabaseDifferencesForm));
		panelMain = new KryptonPanel();
		buttonCancel = new KryptonButton();
		buttonCompare = new KryptonButton();
		groupBoxResults = new KryptonGroupBox();
		listViewResults = new ListView();
		columnHeaderNumber = new ColumnHeader();
		columnHeaderIndex = new ColumnHeader();
		columnHeaderDiff = new ColumnHeader();
		groupBoxProgress = new KryptonGroupBox();
		progressBar = new KryptonProgressBar();
		groupBoxFile2 = new KryptonGroupBox();
		buttonSelectFile2 = new KryptonButton();
		labelFile2 = new KryptonLabel();
		groupBoxFile1 = new KryptonGroupBox();
		buttonSelectFile1 = new KryptonButton();
		labelFile1 = new KryptonLabel();
		statusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		openFileDialog = new OpenFileDialog();
		((ISupportInitialize)panelMain).BeginInit();
		panelMain.SuspendLayout();
		((ISupportInitialize)groupBoxResults).BeginInit();
		((ISupportInitialize)groupBoxResults.Panel).BeginInit();
		groupBoxResults.Panel.SuspendLayout();
		((ISupportInitialize)groupBoxProgress).BeginInit();
		((ISupportInitialize)groupBoxProgress.Panel).BeginInit();
		groupBoxProgress.Panel.SuspendLayout();
		((ISupportInitialize)groupBoxFile2).BeginInit();
		((ISupportInitialize)groupBoxFile2.Panel).BeginInit();
		groupBoxFile2.Panel.SuspendLayout();
		((ISupportInitialize)groupBoxFile1).BeginInit();
		((ISupportInitialize)groupBoxFile1.Panel).BeginInit();
		groupBoxFile1.Panel.SuspendLayout();
		statusStrip.SuspendLayout();
		SuspendLayout();
		// 
		// panelMain
		// 
		panelMain.Controls.Add(buttonCancel);
		panelMain.Controls.Add(buttonCompare);
		panelMain.Controls.Add(groupBoxResults);
		panelMain.Controls.Add(groupBoxProgress);
		panelMain.Controls.Add(groupBoxFile2);
		panelMain.Controls.Add(groupBoxFile1);
		panelMain.Controls.Add(statusStrip);
		panelMain.Dock = DockStyle.Fill;
		panelMain.Location = new Point(0, 0);
		panelMain.Name = "panelMain";
		panelMain.Size = new Size(800, 600);
		panelMain.TabIndex = 0;
		// 
		// buttonCancel
		// 
		buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		buttonCancel.Location = new Point(670, 550);
		buttonCancel.Name = "buttonCancel";
		buttonCancel.Size = new Size(100, 30);
		buttonCancel.TabIndex = 5;
		buttonCancel.Values.DropDownArrowColor = Color.Empty;
		buttonCancel.Values.Text = "Cancel";
		buttonCancel.Click += ButtonCancel_Click;
		// 
		// buttonCompare
		// 
		buttonCompare.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		buttonCompare.Location = new Point(564, 550);
		buttonCompare.Name = "buttonCompare";
		buttonCompare.Size = new Size(100, 30);
		buttonCompare.TabIndex = 4;
		buttonCompare.Values.DropDownArrowColor = Color.Empty;
		buttonCompare.Values.Text = "Compare";
		buttonCompare.Click += ButtonCompare_Click;
		// 
		// groupBoxResults
		// 
		groupBoxResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxResults.Location = new Point(12, 260);
		// 
		// 
		// 
		groupBoxResults.Panel.Controls.Add(listViewResults);
		groupBoxResults.Size = new Size(776, 280);
		groupBoxResults.TabIndex = 3;
		groupBoxResults.Values.Heading = "Results";
		// 
		// listViewResults
		// 
		listViewResults.Columns.AddRange(new ColumnHeader[] { columnHeaderNumber, columnHeaderIndex, columnHeaderDiff });
		listViewResults.Dock = DockStyle.Fill;
		listViewResults.FullRowSelect = true;
		listViewResults.GridLines = true;
		listViewResults.Location = new Point(0, 0);
		listViewResults.Name = "listViewResults";
		listViewResults.Size = new Size(772, 256);
		listViewResults.TabIndex = 0;
		listViewResults.UseCompatibleStateImageBehavior = false;
		listViewResults.View = View.Details;
		// 
		// columnHeaderNumber
		// 
		columnHeaderNumber.Text = "No.";
		// 
		// columnHeaderIndex
		// 
		columnHeaderIndex.Text = "Designation";
		columnHeaderIndex.Width = 150;
		// 
		// columnHeaderDiff
		// 
		columnHeaderDiff.Text = "Difference";
		columnHeaderDiff.Width = 600;
		// 
		// groupBoxProgress
		// 
		groupBoxProgress.Location = new Point(12, 184);
		// 
		// 
		// 
		groupBoxProgress.Panel.Controls.Add(progressBar);
		groupBoxProgress.Size = new Size(776, 70);
		groupBoxProgress.TabIndex = 2;
		groupBoxProgress.Values.Heading = "Progress";
		// 
		// progressBar
		// 
		progressBar.Dock = DockStyle.Fill;
		progressBar.Location = new Point(0, 0);
		progressBar.Name = "progressBar";
		progressBar.Size = new Size(772, 46);
		progressBar.TabIndex = 0;
		progressBar.Text = "0%";
		progressBar.TextBackdropColor = Color.Empty;
		progressBar.TextShadowColor = Color.Empty;
		progressBar.Values.Text = "0%";
		// 
		// groupBoxFile2
		// 
		groupBoxFile2.Location = new Point(12, 98);
		// 
		// 
		// 
		groupBoxFile2.Panel.Controls.Add(buttonSelectFile2);
		groupBoxFile2.Panel.Controls.Add(labelFile2);
		groupBoxFile2.Size = new Size(776, 80);
		groupBoxFile2.TabIndex = 1;
		groupBoxFile2.Values.Heading = "Comparison MPCORB.DAT";
		// 
		// buttonSelectFile2
		// 
		buttonSelectFile2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		buttonSelectFile2.Location = new Point(660, 14);
		buttonSelectFile2.Name = "buttonSelectFile2";
		buttonSelectFile2.Size = new Size(90, 25);
		buttonSelectFile2.TabIndex = 1;
		buttonSelectFile2.Values.DropDownArrowColor = Color.Empty;
		buttonSelectFile2.Values.Text = "Select...";
		buttonSelectFile2.Click += ButtonSelectFile2_Click;
		// 
		// labelFile2
		// 
		labelFile2.Location = new Point(13, 16);
		labelFile2.Name = "labelFile2";
		labelFile2.Size = new Size(100, 20);
		labelFile2.TabIndex = 0;
		labelFile2.Values.Text = "No file selected";
		// 
		// groupBoxFile1
		// 
		groupBoxFile1.Location = new Point(12, 12);
		// 
		// 
		// 
		groupBoxFile1.Panel.Controls.Add(buttonSelectFile1);
		groupBoxFile1.Panel.Controls.Add(labelFile1);
		groupBoxFile1.Size = new Size(776, 80);
		groupBoxFile1.TabIndex = 0;
		groupBoxFile1.Values.Heading = "Reference MPCORB.DAT (Currently Opened)";
		// 
		// buttonSelectFile1
		// 
		buttonSelectFile1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		buttonSelectFile1.Location = new Point(660, 14);
		buttonSelectFile1.Name = "buttonSelectFile1";
		buttonSelectFile1.Size = new Size(90, 25);
		buttonSelectFile1.TabIndex = 1;
		buttonSelectFile1.Values.DropDownArrowColor = Color.Empty;
		buttonSelectFile1.Values.Text = "Select...";
		buttonSelectFile1.Click += ButtonSelectFile1_Click;
		// 
		// labelFile1
		// 
		labelFile1.Location = new Point(13, 16);
		labelFile1.Name = "labelFile1";
		labelFile1.Size = new Size(100, 20);
		labelFile1.TabIndex = 0;
		labelFile1.Values.Text = "No file selected";
		// 
		// statusStrip
		// 
		statusStrip.Font = new Font("Segoe UI", 9F);
		statusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		statusStrip.Location = new Point(0, 578);
		statusStrip.Name = "statusStrip";
		statusStrip.ProgressBars = null;
		statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		statusStrip.Size = new Size(800, 22);
		statusStrip.TabIndex = 6;
		// 
		// labelInformation
		// 
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(123, 17);
		labelInformation.Text = "Ready for comparison";
		// 
		// openFileDialog
		// 
		openFileDialog.Filter = "MPCORB Files (*.DAT)|*.DAT|All Files (*.*)|*.*";
		// 
		// DatabaseDifferencesForm
		// 
		AccessibleDescription = "Compares two MPCORB.DAT files";
		AccessibleName = "Database Differences";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(800, 600);
		ControlBox = false;
		Controls.Add(panelMain);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "DatabaseDifferencesForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Database Differences";
		Load += DatabaseDifferences2Form_Load;
		((ISupportInitialize)panelMain).EndInit();
		panelMain.ResumeLayout(false);
		panelMain.PerformLayout();
		((ISupportInitialize)groupBoxResults.Panel).EndInit();
		groupBoxResults.Panel.ResumeLayout(false);
		((ISupportInitialize)groupBoxResults).EndInit();
		((ISupportInitialize)groupBoxProgress.Panel).EndInit();
		groupBoxProgress.Panel.ResumeLayout(false);
		((ISupportInitialize)groupBoxProgress).EndInit();
		((ISupportInitialize)groupBoxFile2.Panel).EndInit();
		groupBoxFile2.Panel.ResumeLayout(false);
		groupBoxFile2.Panel.PerformLayout();
		((ISupportInitialize)groupBoxFile2).EndInit();
		((ISupportInitialize)groupBoxFile1.Panel).EndInit();
		groupBoxFile1.Panel.ResumeLayout(false);
		groupBoxFile1.Panel.PerformLayout();
		((ISupportInitialize)groupBoxFile1).EndInit();
		statusStrip.ResumeLayout(false);
		statusStrip.PerformLayout();
		ResumeLayout(false);
	}

	private KryptonPanel panelMain;
    private KryptonGroupBox groupBoxFile1;
    private KryptonLabel labelFile1;
    private KryptonButton buttonSelectFile1;
    private KryptonGroupBox groupBoxFile2;
    private KryptonLabel labelFile2;
    private KryptonButton buttonSelectFile2;
    private KryptonGroupBox groupBoxProgress;
    private KryptonProgressBar progressBar;
    private KryptonGroupBox groupBoxResults;
    private ListView listViewResults;
    private ColumnHeader columnHeaderNumber;
    private ColumnHeader columnHeaderIndex;
    private ColumnHeader columnHeaderDiff;
    private KryptonButton buttonCompare;
    private KryptonButton buttonCancel;
    private OpenFileDialog openFileDialog;
    private KryptonStatusStrip statusStrip;
    private ToolStripStatusLabel labelInformation;
}
