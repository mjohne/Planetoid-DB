namespace Planetoid_DB.Forms
{
	partial class ThemeTestForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeTestForm));
			kryptonThemeComboBox = new Krypton.Toolkit.KryptonThemeComboBox();
			((System.ComponentModel.ISupportInitialize)kryptonThemeComboBox).BeginInit();
			SuspendLayout();
			// 
			// kryptonThemeComboBox
			// 
			kryptonThemeComboBox.AccessibleDescription = "Lists the themes in a combo box";
			kryptonThemeComboBox.AccessibleName = "Theme combo box";
			kryptonThemeComboBox.AccessibleRole = AccessibleRole.ComboBox;
			kryptonThemeComboBox.AllowButtonSpecToolTipPriority = true;
			kryptonThemeComboBox.AllowButtonSpecToolTips = true;
			kryptonThemeComboBox.Location = new Point(12, 12);
			kryptonThemeComboBox.Name = "kryptonThemeComboBox";
			kryptonThemeComboBox.Size = new Size(260, 22);
			kryptonThemeComboBox.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
			kryptonThemeComboBox.TabIndex = 0;
			kryptonThemeComboBox.ToolTipValues.Description = "Lists the themes in a combo box";
			kryptonThemeComboBox.ToolTipValues.EnableToolTips = true;
			kryptonThemeComboBox.ToolTipValues.Heading = "Theme combo box";
			// 
			// ThemeTestForm
			// 
			AccessibleDescription = "Tests the themes";
			AccessibleName = "Test theme form";
			AccessibleRole = AccessibleRole.Window;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(284, 59);
			ControlBox = false;
			Controls.Add(kryptonThemeComboBox);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "ThemeTestForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "ThemeTestForm";
			((System.ComponentModel.ISupportInitialize)kryptonThemeComboBox).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Krypton.Toolkit.KryptonThemeComboBox kryptonThemeComboBox;
	}
}