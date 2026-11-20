/*
 * File:        GeometricAlbedoAdjustedDiameterForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Displays and adjusts absolute magnitude and geometric albedo values to calculate and show the corresponding asteroid diameter in kilometers.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB.Forms;

/// <summary>Displays and adjusts absolute magnitude and geometric albedo values to calculate and show the corresponding asteroid diameter in kilometers.</summary>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class GeometricAlbedoAdjustedDiameterForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => kryptonTableLayoutPanelAlbedoTable;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Albedo Table";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Albedo-Table";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Indicates whether a UI update is currently in progress.</summary>
	private bool _isUpdating = false;

	/// <summary>Stores the calculated adjusted diameter.</summary>
	private double diameter = 0;

	/// <summary>The default albedo value used for geometric albedo calculations.</summary>
	private readonly double defaultAlbedo = 0.14;

	#region Constructor

	/// <summary>Initializes a geometric-albedo-adjusted diameter form for the specified absolute magnitude.</summary>
	/// <param name="mag">The absolute magnitude used to initialize the form.</param>
	public GeometricAlbedoAdjustedDiameterForm(double mag)
	{
		// Log that the GeometricAlbedoAdjustedDiameterForm is being initialized
		logger.Info(message: "Initializing GeometricAlbedoAdjustedDiameterForm...");
		// Initialize the form components
		InitializeComponent();
		kryptonNumericUpDownAbsoluteMagnitude.Value = (decimal)mag;
		kryptonNumericUpDownGeometricAlbedo.Value = (decimal)defaultAlbedo;
		// Log that the GeometricAlbedoAdjustedDiameterForm has been initialized successfully
		logger.Info(message: "GeometricAlbedoAdjustedDiameterForm initialized.");
	}

	#endregion

	#region helper methods

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	#endregion

	#region Form event handlers

	/// <summary>Handles initialization when the geometric albedo-adjusted diameter form loads.</summary>
	/// <param name="sender">The object that raised the load event.</param>
	/// <param name="e">The event data associated with the load event.</param>
	private void GeometricAlbedoAdjustedDiameterForm_Load(object sender, EventArgs e)
	{
		// Log that the form has loaded successfully
		logger.Info(message: "GeometricAlbedoAdjustedDiameterForm loaded successfully.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar to ensure no message is displayed on startup
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles closing of the geometric-albedo-adjusted diameter form.</summary>
	/// <remarks>Logs the closing action and plays the close sound.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The form-closing event data.</param>
	private void GeometricAlbedoAdjustedDiameterForm_Closing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing
		logger.Info(message: "GeometricAlbedoAdjustedDiameterForm is closing.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region #ValueChanged even handlers

	/// <summary>Synchronizes the absolute-magnitude track bar with the numeric input and updates the geometric-albedo-adjusted diameter.</summary>
	/// <param name="sender">The source of the value-changed event.</param>
	/// <param name="e">The event data associated with the value-changed event.</param>
	private void KryptonTrackBarAbsoluteMagnitude_ValueChanged(object sender, EventArgs e)
	{
		// Log the action of changing value of KryptonTrackBarAbsoluteMagnitude
		logger.Info(message: "Changing value of KryptonTrackBarAbsoluteMagnitude...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		_isUpdating = true;
		try
		{
			decimal val = kryptonTrackBarAbsoluteMagnitude.Value / 10m;
			if (kryptonNumericUpDownAbsoluteMagnitude.Value != val)
			{
				kryptonNumericUpDownAbsoluteMagnitude.Value = Math.Clamp(value: val, min: kryptonNumericUpDownAbsoluteMagnitude.Minimum, max: kryptonNumericUpDownAbsoluteMagnitude.Maximum);
			}
			diameter = DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: (double)kryptonNumericUpDownAbsoluteMagnitude.Value, geometricAlbedo: (double)kryptonNumericUpDownGeometricAlbedo.Value);
			kryptonLabelGeometricAlbedoAdjustedDiameter.Text = $"The geometric albedo-adjusted diameter is {diameter:N5} km.";
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the absolute-magnitude controls and recalculates the geometric-albedo-adjusted diameter.</summary>
	/// <param name="sender">The source of the value-change event.</param>
	/// <param name="e">The event data associated with the value change.</param>
	private void KryptonNumericUpDownAbsoluteMagnitude_ValueChanged(object sender, EventArgs e)
	{
		// Log the action of changing value of KryptonNumericUpDownAbsoluteMagnitude
		logger.Info(message: "Changing value of KryptonNumericUpDownAbsoluteMagnitude...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		_isUpdating = true;
		try
		{
			int val = (int)(kryptonNumericUpDownAbsoluteMagnitude.Value * 10m);
			if (kryptonTrackBarAbsoluteMagnitude.Value != val)
			{
				kryptonTrackBarAbsoluteMagnitude.Value = Math.Clamp(value: val, min: kryptonTrackBarAbsoluteMagnitude.Minimum, max: kryptonTrackBarAbsoluteMagnitude.Maximum);
			}
			diameter = DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: (double)kryptonNumericUpDownAbsoluteMagnitude.Value, geometricAlbedo: (double)kryptonNumericUpDownGeometricAlbedo.Value);
			kryptonLabelGeometricAlbedoAdjustedDiameter.Text = $"The geometric albedo-adjusted diameter is {diameter:N5} km.";
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the geometric albedo controls and updates the albedo-adjusted diameter.</summary>
	/// <remarks>Updates are skipped while another control update is in progress.</remarks>
	/// <param name="sender">The source of the value-changed event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonTrackBarGeometricAlbedo_ValueChanged(object sender, EventArgs e)
	{
		// Log the action of changing value of KryptonTrackBarGeometricAlbedo
		logger.Info(message: "Changing value of KryptonTrackBarGeometricAlbedo...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		_isUpdating = true;
		try
		{
			decimal val = kryptonTrackBarGeometricAlbedo.Value / 1000m;
			if (kryptonNumericUpDownGeometricAlbedo.Value != val)
			{
				kryptonNumericUpDownGeometricAlbedo.Value = Math.Clamp(value: val, min: kryptonNumericUpDownGeometricAlbedo.Minimum, max: kryptonNumericUpDownGeometricAlbedo.Maximum);
			}
			diameter = DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: (double)kryptonNumericUpDownAbsoluteMagnitude.Value, geometricAlbedo: (double)kryptonNumericUpDownGeometricAlbedo.Value);
			kryptonLabelGeometricAlbedoAdjustedDiameter.Text = $"The geometric albedo-adjusted diameter is {diameter:N5} km.";
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the geometric albedo track bar, recalculates the adjusted diameter, and updates its display.</summary>
	/// <param name="sender">The source of the value-changed event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownGeometricAlbedo_ValueChanged(object sender, EventArgs e)
	{
		// Log the action of changing value of KryptonNumericUpDownGeometricAlbedo
		logger.Info(message: "Changing value of KryptonNumericUpDownGeometricAlbedo...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		_isUpdating = true;
		try
		{
			int val = (int)(kryptonNumericUpDownGeometricAlbedo.Value * 1000m);
			if (kryptonTrackBarGeometricAlbedo.Value != val)
			{
				kryptonTrackBarGeometricAlbedo.Value = Math.Clamp(value: val, min: kryptonTrackBarGeometricAlbedo.Minimum, max: kryptonTrackBarGeometricAlbedo.Maximum);
			}
			diameter = DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: (double)kryptonNumericUpDownAbsoluteMagnitude.Value, geometricAlbedo: (double)kryptonNumericUpDownGeometricAlbedo.Value);
			kryptonLabelGeometricAlbedoAdjustedDiameter.Text = $"The geometric albedo-adjusted diameter is {diameter:N5} km.";
		}
		finally
		{
			_isUpdating = false;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Shows or hides the information label according to the menu item's checked state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void ToolStripMenuItemShowInformationLabel_Click(object sender, EventArgs e)
	{
		// Log the action of clicking of ToolStripMenuItemShowInformationLabel
		logger.Info(message: $"ToolStripMenuItemShowInformationLabel clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		labelInformation.Visible = toolStripMenuItemShowInformationLabel.Checked;
		// Log the new state of the visibility of the information label
		logger.Info(message: $"Set the visibility of the information label to {labelInformation.Visible}.");
	}

	#endregion
}
