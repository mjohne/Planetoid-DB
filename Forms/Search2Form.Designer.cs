namespace Planetoid_DB
{
    partial class Search2Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search2Form));
			kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
			kryptonLabelStatus = new Krypton.Toolkit.KryptonLabel();
			kryptonProgressBar = new Krypton.Toolkit.KryptonProgressBar();
			listViewResults = new ListView();
			columnHeaderIndex = new ColumnHeader();
			columnHeaderDesignation = new ColumnHeader();
			columnHeaderElement = new ColumnHeader();
			columnHeaderValue = new ColumnHeader();
			kryptonButtonCancel = new Krypton.Toolkit.KryptonButton();
			kryptonButtonSearch = new Krypton.Toolkit.KryptonButton();
			kryptonCheckBoxFullText = new Krypton.Toolkit.KryptonCheckBox();
			kryptonCheckedListBoxElements = new Krypton.Toolkit.KryptonCheckedListBox();
			kryptonTextBoxSearch = new Krypton.Toolkit.KryptonTextBox();
			kryptonLabelSearch = new Krypton.Toolkit.KryptonLabel();
			((System.ComponentModel.ISupportInitialize)kryptonPanelMain).BeginInit();
			kryptonPanelMain.SuspendLayout();
			SuspendLayout();
			// 
			// kryptonPanelMain
			// 
			kryptonPanelMain.Controls.Add(kryptonLabelStatus);
			kryptonPanelMain.Controls.Add(kryptonProgressBar);
			kryptonPanelMain.Controls.Add(listViewResults);
			kryptonPanelMain.Controls.Add(kryptonButtonCancel);
			kryptonPanelMain.Controls.Add(kryptonButtonSearch);
			kryptonPanelMain.Controls.Add(kryptonCheckBoxFullText);
			kryptonPanelMain.Controls.Add(kryptonCheckedListBoxElements);
			kryptonPanelMain.Controls.Add(kryptonTextBoxSearch);
			kryptonPanelMain.Controls.Add(kryptonLabelSearch);
			kryptonPanelMain.Dock = DockStyle.Fill;
			kryptonPanelMain.Location = new Point(0, 0);
			kryptonPanelMain.Margin = new Padding(4, 3, 4, 3);
			kryptonPanelMain.Name = "kryptonPanelMain";
			kryptonPanelMain.Size = new Size(933, 519);
			kryptonPanelMain.TabIndex = 0;
			// 
			// kryptonLabelStatus
			// 
			kryptonLabelStatus.Location = new Point(14, 485);
			kryptonLabelStatus.Margin = new Padding(4, 3, 4, 3);
			kryptonLabelStatus.Name = "kryptonLabelStatus";
			kryptonLabelStatus.Size = new Size(54, 23);
			kryptonLabelStatus.TabIndex = 8;
			kryptonLabelStatus.Values.Text = "Status";
			// 
			// kryptonProgressBar
			// 
			kryptonProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			kryptonProgressBar.Location = new Point(14, 448);
			kryptonProgressBar.Margin = new Padding(4, 3, 4, 3);
			kryptonProgressBar.Name = "kryptonProgressBar";
			kryptonProgressBar.Size = new Size(905, 30);
			kryptonProgressBar.TabIndex = 7;
			kryptonProgressBar.Text = "0 %";
			kryptonProgressBar.TextBackdropColor = Color.Empty;
			kryptonProgressBar.TextShadowColor = Color.Empty;
			kryptonProgressBar.Values.Text = "0 %";
			// 
			// listViewResults
			// 
			listViewResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listViewResults.Columns.AddRange(new ColumnHeader[] { columnHeaderIndex, columnHeaderDesignation, columnHeaderElement, columnHeaderValue });
			listViewResults.FullRowSelect = true;
			listViewResults.GridLines = true;
			listViewResults.Location = new Point(378, 47);
			listViewResults.Margin = new Padding(4, 3, 4, 3);
			listViewResults.Name = "listViewResults";
			listViewResults.Size = new Size(541, 393);
			listViewResults.TabIndex = 6;
			listViewResults.UseCompatibleStateImageBehavior = false;
			listViewResults.View = View.Details;
			listViewResults.VirtualMode = true;
			listViewResults.RetrieveVirtualItem += listViewResults_RetrieveVirtualItem;
			// 
			// columnHeaderIndex
			// 
			columnHeaderIndex.Text = "Index No.";
			columnHeaderIndex.Width = 80;
			// 
			// columnHeaderDesignation
			// 
			columnHeaderDesignation.Text = "Readable designation";
			columnHeaderDesignation.Width = 150;
			// 
			// columnHeaderElement
			// 
			columnHeaderElement.Text = "Orbital Element";
			columnHeaderElement.Width = 120;
			// 
			// columnHeaderValue
			// 
			columnHeaderValue.Text = "Value";
			columnHeaderValue.Width = 100;
			// 
			// kryptonButtonCancel
			// 
			kryptonButtonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			kryptonButtonCancel.Location = new Point(154, 411);
			kryptonButtonCancel.Margin = new Padding(4, 3, 4, 3);
			kryptonButtonCancel.Name = "kryptonButtonCancel";
			kryptonButtonCancel.Size = new Size(105, 29);
			kryptonButtonCancel.TabIndex = 5;
			kryptonButtonCancel.Values.DropDownArrowColor = Color.Empty;
			kryptonButtonCancel.Values.Text = "Cancel";
			kryptonButtonCancel.Click += kryptonButtonCancel_Click;
			// 
			// kryptonButtonSearch
			// 
			kryptonButtonSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			kryptonButtonSearch.Location = new Point(14, 412);
			kryptonButtonSearch.Margin = new Padding(4, 3, 4, 3);
			kryptonButtonSearch.Name = "kryptonButtonSearch";
			kryptonButtonSearch.Size = new Size(105, 29);
			kryptonButtonSearch.TabIndex = 4;
			kryptonButtonSearch.Values.DropDownArrowColor = Color.Empty;
			kryptonButtonSearch.Values.Text = "Search";
			kryptonButtonSearch.Click += kryptonButtonSearch_Click;
			// 
			// kryptonCheckBoxFullText
			// 
			kryptonCheckBoxFullText.Location = new Point(14, 382);
			kryptonCheckBoxFullText.Margin = new Padding(4, 3, 4, 3);
			kryptonCheckBoxFullText.Name = "kryptonCheckBoxFullText";
			kryptonCheckBoxFullText.Size = new Size(114, 23);
			kryptonCheckBoxFullText.TabIndex = 3;
			kryptonCheckBoxFullText.Values.Text = "Full Text Only";
			// 
			// kryptonCheckedListBoxElements
			// 
			kryptonCheckedListBoxElements.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			kryptonCheckedListBoxElements.Location = new Point(14, 47);
			kryptonCheckedListBoxElements.Margin = new Padding(4, 3, 4, 3);
			kryptonCheckedListBoxElements.Name = "kryptonCheckedListBoxElements";
			kryptonCheckedListBoxElements.Size = new Size(357, 328);
			kryptonCheckedListBoxElements.TabIndex = 2;
			// 
			// kryptonTextBoxSearch
			// 
			kryptonTextBoxSearch.Location = new Point(130, 14);
			kryptonTextBoxSearch.Margin = new Padding(4, 3, 4, 3);
			kryptonTextBoxSearch.Name = "kryptonTextBoxSearch";
			kryptonTextBoxSearch.Size = new Size(350, 23);
			kryptonTextBoxSearch.TabIndex = 1;
			// 
			// kryptonLabelSearch
			// 
			kryptonLabelSearch.Location = new Point(14, 14);
			kryptonLabelSearch.Margin = new Padding(4, 3, 4, 3);
			kryptonLabelSearch.Name = "kryptonLabelSearch";
			kryptonLabelSearch.Size = new Size(57, 23);
			kryptonLabelSearch.TabIndex = 0;
			kryptonLabelSearch.Values.Text = "Search:";
			// 
			// Search2Form
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(933, 519);
			ControlBox = false;
			Controls.Add(kryptonPanelMain);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(4, 3, 4, 3);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "Search2Form";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Search in MPCORB.DAT";
			Load += Search2Form_Load;
			((System.ComponentModel.ISupportInitialize)kryptonPanelMain).EndInit();
			kryptonPanelMain.ResumeLayout(false);
			kryptonPanelMain.PerformLayout();
			ResumeLayout(false);

		}

		#endregion

		private Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        private Krypton.Toolkit.KryptonLabel kryptonLabelSearch;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearch;
        private Krypton.Toolkit.KryptonCheckedListBox kryptonCheckedListBoxElements;
        private Krypton.Toolkit.KryptonCheckBox kryptonCheckBoxFullText;
        private Krypton.Toolkit.KryptonButton kryptonButtonSearch;
        private Krypton.Toolkit.KryptonButton kryptonButtonCancel;
        private System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderDesignation;
        private System.Windows.Forms.ColumnHeader columnHeaderElement;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private Krypton.Toolkit.KryptonProgressBar kryptonProgressBar;
        private Krypton.Toolkit.KryptonLabel kryptonLabelStatus;
    }
}
