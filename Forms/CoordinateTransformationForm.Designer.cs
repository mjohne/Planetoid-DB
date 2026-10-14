/*
 * File:        CoordinateTransformationForm.Designer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Designer-generated controls for the coordinate conversion form.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

#nullable enable

using System.ComponentModel;

namespace Planetoid_DB.Forms;

/// <summary>Contains the designer-generated controls for the coordinate conversion form.</summary>
partial class CoordinateTransformationForm
{
	private IContainer? components;
	private TableLayoutPanel tableLayoutPanel = null!;
	private Label labelSource = null!;
	private ComboBox comboBoxSource = null!;
	private Label labelTarget = null!;
	private ComboBox comboBoxTarget = null!;
	private Label labelFirstCoordinate = null!;
	private TextBox textBoxFirstCoordinate = null!;
	private Label labelSecondCoordinate = null!;
	private TextBox textBoxSecondCoordinate = null!;
	private Label labelFirstCoordinateType = null!;
	private ComboBox comboBoxFirstCoordinateType = null!;
	private Label labelAngleUnit = null!;
	private ComboBox comboBoxAngleUnit = null!;
	private Label labelDateTimeUtc = null!;
	private DateTimePicker dateTimePickerUtc = null!;
	private Label labelObserverLatitude = null!;
	private TextBox textBoxObserverLatitude = null!;
	private Label labelObserverLongitude = null!;
	private TextBox textBoxObserverLongitude = null!;
	private Label labelResultTitle = null!;
	private Label labelResult = null!;
	private Button buttonConvert = null!;

	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new(typeof(CoordinateTransformationForm));
		tableLayoutPanel = new TableLayoutPanel();
		labelSource = new Label();
		comboBoxSource = new ComboBox();
		labelTarget = new Label();
		comboBoxTarget = new ComboBox();
		labelFirstCoordinate = new Label();
		textBoxFirstCoordinate = new TextBox();
		labelSecondCoordinate = new Label();
		textBoxSecondCoordinate = new TextBox();
		labelFirstCoordinateType = new Label();
		comboBoxFirstCoordinateType = new ComboBox();
		labelAngleUnit = new Label();
		comboBoxAngleUnit = new ComboBox();
		labelDateTimeUtc = new Label();
		dateTimePickerUtc = new DateTimePicker();
		labelObserverLatitude = new Label();
		textBoxObserverLatitude = new TextBox();
		labelObserverLongitude = new Label();
		textBoxObserverLongitude = new TextBox();
		labelResultTitle = new Label();
		labelResult = new Label();
		buttonConvert = new Button();
		SuspendLayout();
		//
		// tableLayoutPanel
		//
		tableLayoutPanel.ColumnCount = 4;
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
		tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
		tableLayoutPanel.Controls.Add(labelSource, 0, 0);
		tableLayoutPanel.Controls.Add(comboBoxSource, 1, 0);
		tableLayoutPanel.Controls.Add(labelTarget, 2, 0);
		tableLayoutPanel.Controls.Add(comboBoxTarget, 3, 0);
		tableLayoutPanel.Controls.Add(labelFirstCoordinate, 0, 1);
		tableLayoutPanel.Controls.Add(textBoxFirstCoordinate, 1, 1);
		tableLayoutPanel.Controls.Add(labelFirstCoordinateType, 2, 1);
		tableLayoutPanel.Controls.Add(comboBoxFirstCoordinateType, 3, 1);
		tableLayoutPanel.Controls.Add(labelSecondCoordinate, 0, 2);
		tableLayoutPanel.Controls.Add(textBoxSecondCoordinate, 1, 2);
		tableLayoutPanel.Controls.Add(labelAngleUnit, 2, 2);
		tableLayoutPanel.Controls.Add(comboBoxAngleUnit, 3, 2);
		tableLayoutPanel.Controls.Add(labelDateTimeUtc, 0, 3);
		tableLayoutPanel.Controls.Add(dateTimePickerUtc, 1, 3);
		tableLayoutPanel.SetColumnSpan(dateTimePickerUtc, 3);
		tableLayoutPanel.Controls.Add(labelObserverLatitude, 0, 4);
		tableLayoutPanel.Controls.Add(textBoxObserverLatitude, 1, 4);
		tableLayoutPanel.Controls.Add(labelObserverLongitude, 2, 4);
		tableLayoutPanel.Controls.Add(textBoxObserverLongitude, 3, 4);
		tableLayoutPanel.Controls.Add(labelResultTitle, 0, 5);
		tableLayoutPanel.SetColumnSpan(labelResultTitle, 4);
		tableLayoutPanel.Controls.Add(labelResult, 0, 6);
		tableLayoutPanel.SetColumnSpan(labelResult, 4);
		tableLayoutPanel.Controls.Add(buttonConvert, 3, 7);
		tableLayoutPanel.SetColumnSpan(buttonConvert, 1);
		tableLayoutPanel.Dock = DockStyle.Fill;
		tableLayoutPanel.Location = new Point(0, 0);
		tableLayoutPanel.Name = "tableLayoutPanel";
		tableLayoutPanel.Padding = new Padding(12);
		tableLayoutPanel.RowCount = 8;
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
		tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
		tableLayoutPanel.Size = new Size(740, 350);
		tableLayoutPanel.TabIndex = 0;
		//
		// labelSource
		//
		labelSource.Anchor = AnchorStyles.Left;
		labelSource.AutoSize = true;
		labelSource.Name = "labelSource";
		labelSource.Text = "Convert from";
		//
		// comboBoxSource
		//
		comboBoxSource.Dock = DockStyle.Fill;
		comboBoxSource.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxSource.Name = "comboBoxSource";
		comboBoxSource.SelectedIndexChanged += CoordinateSelection_Changed;
		//
		// labelTarget
		//
		labelTarget.Anchor = AnchorStyles.Left;
		labelTarget.AutoSize = true;
		labelTarget.Name = "labelTarget";
		labelTarget.Text = "Convert to";
		//
		// comboBoxTarget
		//
		comboBoxTarget.Dock = DockStyle.Fill;
		comboBoxTarget.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxTarget.Name = "comboBoxTarget";
		comboBoxTarget.SelectedIndexChanged += CoordinateSelection_Changed;
		//
		// labelFirstCoordinate
		//
		labelFirstCoordinate.Anchor = AnchorStyles.Left;
		labelFirstCoordinate.AutoSize = true;
		labelFirstCoordinate.Name = "labelFirstCoordinate";
		//
		// textBoxFirstCoordinate
		//
		textBoxFirstCoordinate.Dock = DockStyle.Fill;
		textBoxFirstCoordinate.Name = "textBoxFirstCoordinate";
		//
		// labelFirstCoordinateType
		//
		labelFirstCoordinateType.Anchor = AnchorStyles.Left;
		labelFirstCoordinateType.AutoSize = true;
		labelFirstCoordinateType.Name = "labelFirstCoordinateType";
		labelFirstCoordinateType.Text = "Input as";
		//
		// comboBoxFirstCoordinateType
		//
		comboBoxFirstCoordinateType.Dock = DockStyle.Fill;
		comboBoxFirstCoordinateType.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxFirstCoordinateType.Name = "comboBoxFirstCoordinateType";
		comboBoxFirstCoordinateType.SelectedIndexChanged += CoordinateSelection_Changed;
		//
		// labelSecondCoordinate
		//
		labelSecondCoordinate.Anchor = AnchorStyles.Left;
		labelSecondCoordinate.AutoSize = true;
		labelSecondCoordinate.Name = "labelSecondCoordinate";
		//
		// textBoxSecondCoordinate
		//
		textBoxSecondCoordinate.Dock = DockStyle.Fill;
		textBoxSecondCoordinate.Name = "textBoxSecondCoordinate";
		//
		// labelAngleUnit
		//
		labelAngleUnit.Anchor = AnchorStyles.Left;
		labelAngleUnit.AutoSize = true;
		labelAngleUnit.Name = "labelAngleUnit";
		labelAngleUnit.Text = "RA / HA unit";
		//
		// comboBoxAngleUnit
		//
		comboBoxAngleUnit.Dock = DockStyle.Fill;
		comboBoxAngleUnit.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxAngleUnit.Name = "comboBoxAngleUnit";
		//
		// labelDateTimeUtc
		//
		labelDateTimeUtc.Anchor = AnchorStyles.Left;
		labelDateTimeUtc.AutoSize = true;
		labelDateTimeUtc.Name = "labelDateTimeUtc";
		labelDateTimeUtc.Text = "Date and time (UTC)";
		//
		// dateTimePickerUtc
		//
		dateTimePickerUtc.Dock = DockStyle.Left;
		dateTimePickerUtc.Format = DateTimePickerFormat.Custom;
		dateTimePickerUtc.CustomFormat = "yyyy-MM-dd HH:mm:ss";
		dateTimePickerUtc.Name = "dateTimePickerUtc";
		dateTimePickerUtc.ShowUpDown = true;
		dateTimePickerUtc.Width = 200;
		//
		// labelObserverLatitude
		//
		labelObserverLatitude.Anchor = AnchorStyles.Left;
		labelObserverLatitude.AutoSize = true;
		labelObserverLatitude.Name = "labelObserverLatitude";
		labelObserverLatitude.Text = "Observer latitude (°)";
		//
		// textBoxObserverLatitude
		//
		textBoxObserverLatitude.Dock = DockStyle.Fill;
		textBoxObserverLatitude.Name = "textBoxObserverLatitude";
		textBoxObserverLatitude.Text = "0";
		//
		// labelObserverLongitude
		//
		labelObserverLongitude.Anchor = AnchorStyles.Left;
		labelObserverLongitude.AutoSize = true;
		labelObserverLongitude.Name = "labelObserverLongitude";
		labelObserverLongitude.Text = "Longitude east (°)";
		//
		// textBoxObserverLongitude
		//
		textBoxObserverLongitude.Dock = DockStyle.Fill;
		textBoxObserverLongitude.Name = "textBoxObserverLongitude";
		textBoxObserverLongitude.Text = "0";
		//
		// labelResultTitle
		//
		labelResultTitle.Anchor = AnchorStyles.Left;
		labelResultTitle.AutoSize = true;
		labelResultTitle.Name = "labelResultTitle";
		labelResultTitle.Text = "Result (first and second coordinate)";
		//
		// labelResult
		//
		labelResult.Dock = DockStyle.Fill;
		labelResult.Name = "labelResult";
		labelResult.TextAlign = ContentAlignment.MiddleLeft;
		//
		// buttonConvert
		//
		buttonConvert.Dock = DockStyle.Fill;
		buttonConvert.Name = "buttonConvert";
		buttonConvert.Text = "Convert";
		buttonConvert.UseVisualStyleBackColor = true;
		buttonConvert.Click += ButtonConvert_Click;
		//
		// CoordinateTransformationForm
		//
		resources.ApplyResources(this, "$this");
		AcceptButton = buttonConvert;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(740, 350);
		Controls.Add(tableLayoutPanel);
		MinimumSize = new Size(680, 380);
		Name = "CoordinateTransformationForm";
		StartPosition = FormStartPosition.CenterParent;
		ResumeLayout(performLayout: false);
	}
}
