/*
 * File:        OrreryForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provides the designer-generated user interface for the orrery (planetary machine) form.
 * Remarks:     This file contains the Windows Forms designer-generated code for the OrreryForm. Do not modify this file manually.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using Planetoid_DB.Helpers;
using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

/// <summary>Provides the designer-generated user interface for the orrery (planetary machine) form.</summary>
/// <remarks>The form contains a <see cref="System.Windows.Forms.Panel"/> that hosts the OpenGL rendering surface, a toolbar with animation controls, and a <see cref="KryptonStatusStrip"/> with an informational status label below.</remarks>
partial class OrreryForm
{
	/// <summary>Required designer variable.</summary>
	/// <remarks>This field stores the components used by the form.</remarks>
	private IContainer components = null;

	/// <summary>Releases all resources used by the <see cref="OrreryForm"/>.</summary>
	/// <param name="disposing">True if managed resources should be disposed; otherwise false.</param>
	/// <remarks>This method is called by the runtime to release resources used by the form.</remarks>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
			_animationTimer?.Dispose();
			_overlayFont?.Dispose();
		}
		base.Dispose(disposing);
	}

	/// <summary>Initializes the components of the <see cref="OrreryForm"/>.</summary>
	/// <remarks>This method sets up the controls and their properties for the orrery form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(OrreryForm));
		toolStripContainer = new ToolStripContainer();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonPanelMain = new KryptonPanel();
		panelGl = new Panel();
		toolStripControls = new KryptonToolStrip();
		buttonPlayPause = new ToolStripButton();
		buttonReset = new ToolStripButton();
		separatorRange = new ToolStripSeparator();
		labelStart = new ToolStripLabel();
		numericStartIndex = new ToolStripNumericUpDown();
		labelEnd = new ToolStripLabel();
		numericEndIndex = new ToolStripNumericUpDown();
		separatorTime = new ToolStripSeparator();
		labelSpeed = new ToolStripLabel();
		trackBarSpeed = new TrackBar();
		hostTrackBarSpeed = new ToolStripControlHost(trackBarSpeed);
		labelDateTime = new ToolStripLabel();
		dateTimePicker = new DateTimePicker();
		hostDateTimePicker = new ToolStripControlHost(dateTimePicker);
		kryptonManager = new KryptonManager(components);
		toolStripContainer.BottomToolStripPanel.SuspendLayout();
		toolStripContainer.ContentPanel.SuspendLayout();
		toolStripContainer.TopToolStripPanel.SuspendLayout();
		toolStripContainer.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		toolStripControls.SuspendLayout();
		((ISupportInitialize)trackBarSpeed).BeginInit();
		SuspendLayout();
		//
		// toolStripContainer
		//
		toolStripContainer.AccessibleDescription = "Container to arrange the toolbars";
		toolStripContainer.AccessibleName = "Toolbar container";
		toolStripContainer.AccessibleRole = AccessibleRole.Grouping;
		//
		// toolStripContainer.BottomToolStripPanel
		//
		toolStripContainer.BottomToolStripPanel.Controls.Add(kryptonStatusStrip);
		//
		// toolStripContainer.ContentPanel
		//
		toolStripContainer.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainer.ContentPanel.Size = new Size(980, 578);
		toolStripContainer.Dock = DockStyle.Fill;
		toolStripContainer.Location = new Point(0, 0);
		toolStripContainer.Name = "toolStripContainer";
		toolStripContainer.Size = new Size(980, 650);
		toolStripContainer.TabIndex = 0;
		toolStripContainer.Text = "toolStripContainer";
		//
		// toolStripContainer.TopToolStripPanel
		//
		toolStripContainer.TopToolStripPanel.Controls.Add(toolStripControls);
		//
		// kryptonStatusStrip
		//
		kryptonStatusStrip.AccessibleDescription = "Shows information for the orrery";
		kryptonStatusStrip.AccessibleName = "Status bar with orrery information";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Dock = DockStyle.None;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 0);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(980, 22);
		kryptonStatusStrip.TabIndex = 0;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";
		//
		// labelInformation
		//
		labelInformation.AccessibleDescription = "Shows information for the orrery";
		labelInformation.AccessibleName = "Orrery information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(144, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Shows information for the orrery";
		labelInformation.MouseEnter += Control_Enter;
		labelInformation.MouseLeave += Control_Leave;
		//
		// kryptonPanelMain
		//
		kryptonPanelMain.AccessibleDescription = "Contains the orrery visualization";
		kryptonPanelMain.AccessibleName = "Main panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(panelGl);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(980, 578);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;
		kryptonPanelMain.Text = "Main panel";
		//
		// panelGl
		//
		panelGl.AccessibleDescription = "OpenGL rendering surface for the orrery visualization";
		panelGl.AccessibleName = "Orrery rendering surface";
		panelGl.AccessibleRole = AccessibleRole.Client;
		panelGl.BackColor = Color.Black;
		panelGl.Dock = DockStyle.Fill;
		panelGl.Location = new Point(0, 0);
		panelGl.Name = "panelGl";
		panelGl.Size = new Size(980, 578);
		panelGl.TabIndex = 0;
		//
		// toolStripControls
		//
		toolStripControls.AccessibleDescription = "Animation and range controls for the orrery";
		toolStripControls.AccessibleName = "Orrery controls";
		toolStripControls.AccessibleRole = AccessibleRole.ToolBar;
		toolStripControls.Dock = DockStyle.None;
		toolStripControls.Font = new Font("Segoe UI", 9F);
		toolStripControls.Items.AddRange(new ToolStripItem[] { buttonPlayPause, buttonReset, separatorRange, labelStart, numericStartIndex, labelEnd, numericEndIndex, separatorTime, labelSpeed, hostTrackBarSpeed, labelDateTime, hostDateTimePicker });
		toolStripControls.Location = new Point(0, 0);
		toolStripControls.Name = "toolStripControls";
		toolStripControls.Size = new Size(980, 48);
		toolStripControls.Stretch = true;
		toolStripControls.TabIndex = 0;
		toolStripControls.Enter += Control_Enter;
		toolStripControls.Leave += Control_Leave;
		toolStripControls.MouseEnter += Control_Enter;
		toolStripControls.MouseLeave += Control_Leave;
		//
		// buttonPlayPause
		//
		buttonPlayPause.AccessibleDescription = "Starts or pauses the orrery animation";
		buttonPlayPause.AccessibleName = "Play or pause";
		buttonPlayPause.AccessibleRole = AccessibleRole.PushButton;
		buttonPlayPause.Image = FatcowIcons16px.fatcow_control_play_blue_16px;
		buttonPlayPause.Name = "buttonPlayPause";
		buttonPlayPause.Size = new Size(51, 45);
		buttonPlayPause.Text = "&Play";
		buttonPlayPause.Click += ButtonPlayPause_Click;
		buttonPlayPause.MouseEnter += Control_Enter;
		buttonPlayPause.MouseLeave += Control_Leave;
		//
		// buttonReset
		//
		buttonReset.AccessibleDescription = "Resets the orrery time to now";
		buttonReset.AccessibleName = "Reset time";
		buttonReset.AccessibleRole = AccessibleRole.PushButton;
		buttonReset.Image = FatcowIcons16px.fatcow_cancel_16px;
		buttonReset.Name = "buttonReset";
		buttonReset.Size = new Size(55, 45);
		buttonReset.Text = "&Reset";
		buttonReset.Click += ButtonReset_Click;
		buttonReset.MouseEnter += Control_Enter;
		buttonReset.MouseLeave += Control_Leave;
		//
		// separatorRange
		//
		separatorRange.Name = "separatorRange";
		separatorRange.Size = new Size(6, 48);
		//
		// labelStart
		//
		labelStart.Name = "labelStart";
		labelStart.Size = new Size(33, 45);
		labelStart.Text = "Start";
		//
		// numericStartIndex
		//
		numericStartIndex.AccessibleDescription = "First planetoid index to include in the orrery";
		numericStartIndex.AccessibleName = "Start index";
		numericStartIndex.AccessibleRole = AccessibleRole.SpinButton;
		numericStartIndex.Name = "numericStartIndex";
		numericStartIndex.Size = new Size(80, 22);
		numericStartIndex.Text = "1";
		//
		// labelEnd
		//
		labelEnd.Name = "labelEnd";
		labelEnd.Size = new Size(27, 45);
		labelEnd.Text = "End";
		//
		// numericEndIndex
		//
		numericEndIndex.AccessibleDescription = "Last planetoid index to include in the orrery";
		numericEndIndex.AccessibleName = "End index";
		numericEndIndex.AccessibleRole = AccessibleRole.SpinButton;
		numericEndIndex.Name = "numericEndIndex";
		numericEndIndex.Size = new Size(80, 22);
		numericEndIndex.Text = "1";
		//
		// separatorTime
		//
		separatorTime.Name = "separatorTime";
		separatorTime.Size = new Size(6, 48);
		//
		// labelSpeed
		//
		labelSpeed.Name = "labelSpeed";
		labelSpeed.Size = new Size(39, 45);
		labelSpeed.Text = "Speed";
		//
		// trackBarSpeed
		//
		trackBarSpeed.AccessibleDescription = "Controls the speed and direction of time flow";
		trackBarSpeed.AccessibleName = "Time speed";
		trackBarSpeed.AccessibleRole = AccessibleRole.Slider;
		trackBarSpeed.LargeChange = 10;
		trackBarSpeed.Maximum = 100;
		trackBarSpeed.Minimum = -100;
		trackBarSpeed.Name = "trackBarSpeed";
		trackBarSpeed.Size = new Size(180, 45);
		trackBarSpeed.TabIndex = 0;
		trackBarSpeed.TickFrequency = 10;
		trackBarSpeed.Value = 0;
		trackBarSpeed.ValueChanged += TrackBarSpeed_ValueChanged;
		//
		// hostTrackBarSpeed
		//
		hostTrackBarSpeed.Name = "hostTrackBarSpeed";
		hostTrackBarSpeed.Size = new Size(180, 45);
		//
		// labelDateTime
		//
		labelDateTime.Name = "labelDateTime";
		labelDateTime.Size = new Size(30, 45);
		labelDateTime.Text = "Time";
		//
		// dateTimePicker
		//
		dateTimePicker.AccessibleDescription = "Sets the date and time of the orrery";
		dateTimePicker.AccessibleName = "Orrery date and time";
		dateTimePicker.AccessibleRole = AccessibleRole.DropList;
		dateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
		dateTimePicker.Format = DateTimePickerFormat.Custom;
		dateTimePicker.Name = "dateTimePicker";
		dateTimePicker.ShowUpDown = true;
		dateTimePicker.Size = new Size(160, 23);
		dateTimePicker.TabIndex = 0;
		dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
		//
		// hostDateTimePicker
		//
		hostDateTimePicker.Name = "hostDateTimePicker";
		hostDateTimePicker.Size = new Size(160, 23);
		//
		// kryptonManager
		//
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		//
		// OrreryForm
		//
		AccessibleDescription = "Displays an animated orrery of all planetoids and the eight solar system planets around the Sun";
		AccessibleName = "Orrery form";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(980, 650);
		Controls.Add(toolStripContainer);
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "OrreryForm";
		SizeGripStyle = SizeGripStyle.Hide;
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Orrery";
		Load += OrreryForm_Load;
		toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
		toolStripContainer.BottomToolStripPanel.PerformLayout();
		toolStripContainer.ContentPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.ResumeLayout(false);
		toolStripContainer.TopToolStripPanel.PerformLayout();
		toolStripContainer.ResumeLayout(false);
		toolStripContainer.PerformLayout();
		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();
		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);
		toolStripControls.ResumeLayout(false);
		toolStripControls.PerformLayout();
		((ISupportInitialize)trackBarSpeed).EndInit();
		ResumeLayout(false);
	}

	private ToolStripContainer toolStripContainer;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonPanel kryptonPanelMain;
	private Panel panelGl;
	private KryptonToolStrip toolStripControls;
	private ToolStripButton buttonPlayPause;
	private ToolStripButton buttonReset;
	private ToolStripSeparator separatorRange;
	private ToolStripLabel labelStart;
	private ToolStripNumericUpDown numericStartIndex;
	private ToolStripLabel labelEnd;
	private ToolStripNumericUpDown numericEndIndex;
	private ToolStripSeparator separatorTime;
	private ToolStripLabel labelSpeed;
	private TrackBar trackBarSpeed;
	private ToolStripControlHost hostTrackBarSpeed;
	private ToolStripLabel labelDateTime;
	private DateTimePicker dateTimePicker;
	private ToolStripControlHost hostDateTimePicker;
	private KryptonManager kryptonManager;
}
