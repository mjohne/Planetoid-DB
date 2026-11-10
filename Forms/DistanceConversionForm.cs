/*
 * File:        DistanceConversionForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Converts between earthly, planetary and light distances.
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

/// <summary>Converts between angle and time measure.</summary>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class DistanceConversionForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	// Prevents the infinite loop from being triggered
	private bool _isUpdating;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="DistanceConversionForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public DistanceConversionForm()
	{
		// Log the initialization of the form for debugging purposes
		logger.Info(message: "Initializing DistanceConversionForm...");
		// Initialize the form components
		InitializeComponent();
		// Log that the form has been initialized successfully
		logger.Info(message: "DistanceConversionForm initialized.");
	}

	#endregion

	#region Helpers

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void DistanceConversionForm_Load(object sender, EventArgs e)
	{
		// Log the form load event for debugging purposes
		logger.Info(message: "DistanceConversionForm loaded.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the information.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>If a calculation is running, it is canceled and the form will close after the checking stops. If no checking is running, the it is disposed.</remarks>
	private void DistanceConversionForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log the form closing event for debugging purposes
		logger.Info(message: "DistanceConversionForm is closing.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region ValueChanged event handlers

	/// <summary>Updates the corresponding distance-unit values when the meter value changes.</summary>
	/// <remarks>Prevents recursive updates while synchronizing the converted values.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownMeter_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMeter.Value, from: DistanceUnit.Meter, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance fields when the kilometer value changes.</summary>
	/// <remarks>Prevents recursive updates while synchronizing the converted values.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownKilometer_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.Meter);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownKilometer.Value, from: DistanceUnit.Kilometer, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance values when the land-mile value changes.</summary>
	/// <remarks>Updates are skipped while another distance value is being synchronized.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownLandMile_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLandMile.Value, from: DistanceUnit.Landmile, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance values when the nautical mile value changes.</summary>
	/// <remarks>Prevents recursive updates while synchronizing the other distance controls.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownNauticalMile_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.Landmile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNauticalMile.Value, from: DistanceUnit.NauticalMile, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the other distance-unit values when the light-millisecond value changes.</summary>
	/// <remarks>Recursive updates are ignored while the other distance-unit values are being synchronized.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownLightMillisecond_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMillisecond.Value, from: DistanceUnit.LightMillisecond, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance-unit values when the light-second value changes.</summary>
	/// <remarks>Prevents recursive updates while converting the light-second value to the other supported distance units.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownLightSecond_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightSecond.Value, from: DistanceUnit.LightSecond, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates all related distance unit controls to match the current light-minute value.</summary>
	/// <param name="sender">The event source that raised the value-changed notification.</param>
	/// <param name="e">The event data associated with the value-changed event.</param>
	private void KryptonNumericUpDownLightMinute_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightMinute.Value, from: DistanceUnit.LightMinute, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the equivalent distance values when the light-hour value changes.</summary>
	/// <remarks>Updates are skipped while another synchronized value update is in progress.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownLightHour_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightHour.Value, from: DistanceUnit.LightHour, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance-unit values when the light-day value changes.</summary>
	/// <remarks>Updates are skipped while another value update is in progress to prevent recursive updates.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownLightDay_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.LightHour);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLightDay.Value, from: DistanceUnit.LightDay, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Responds to a Mercurial unit value change by converting it to the other supported distance units and updating the related controls without re-entering the event handler.</summary>
	/// <param name="sender">The source of the value-changed event.</param>
	/// <param name="e">The event data associated with the value change.</param>
	private void KryptonNumericUpDownMercurialUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMercurialUnit.Value, from: DistanceUnit.MercurialUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the other distance-unit values with the selected Venusian unit.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownVenusianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownVenusianUnit.Value, from: DistanceUnit.VenusianUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance-unit values when the terrestrial-unit value changes.</summary>
	/// <remarks>Updates are skipped while another update is in progress to prevent recursive value changes.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownTerrestrialUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownTerrestrialUnit.Value, from: DistanceUnit.AstronomicalUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>
	/// Updates the related distance controls when the lunar unit value changes by converting the current value into each supported unit.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the event.</param>
	private void KryptonNumericUpDownLunarUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownLunarUnit.Value, from: DistanceUnit.LunarUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>
	/// Updates the other distance-unit values when the Martian unit value changes.</summary>	/// <remarks>Prevents recursive updates while synchronizing the distance-unit values.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownMartianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownMartianUnit.Value, from: DistanceUnit.MartianUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the related distance-unit values to match the selected Jovian unit value.</summary>
	/// <param name="sender">The event source that raised the value-change event.</param>
	/// <param name="e">The event data associated with the value-change event.</param>
	private void KryptonNumericUpDownJovianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownJovianUnit.Value, from: DistanceUnit.JovianUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Updates the corresponding distance-unit values when the Saturnian unit value changes.</summary>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownSaturnianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.UranianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownSaturnianUnit.Value, from: DistanceUnit.SaturnianUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the other distance values when the Uranian unit value changes.</summary>
	/// <remarks>Updates are ignored while other distance values are being synchronized.</remarks>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownUranianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownNeptunianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownUranianUnit.Value, from: DistanceUnit.UranianUnit, to: DistanceUnit.NeptunianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	/// <summary>Synchronizes the other distance-unit values when the Neptunian unit value changes.</summary>
	/// <remarks>Updates are skipped while another synchronization is in progress.</remarks>
	/// <param name="sender">The control that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonNumericUpDownNeptunianUnit_ValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (_isUpdating)
		{
			return;
		}
		try
		{
			_isUpdating = true;
			kryptonNumericUpDownMeter.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.Meter);
			kryptonNumericUpDownKilometer.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.Kilometer);
			kryptonNumericUpDownLandMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.Landmile);
			kryptonNumericUpDownNauticalMile.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.NauticalMile);
			kryptonNumericUpDownLightMillisecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LightMillisecond);
			kryptonNumericUpDownLightSecond.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LightSecond);
			kryptonNumericUpDownLightMinute.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LightMinute);
			kryptonNumericUpDownLightHour.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LightHour);
			kryptonNumericUpDownLightDay.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LightDay);
			kryptonNumericUpDownMercurialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.MercurialUnit);
			kryptonNumericUpDownVenusianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.VenusianUnit);
			kryptonNumericUpDownTerrestrialUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.AstronomicalUnit);
			kryptonNumericUpDownLunarUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.LunarUnit);
			kryptonNumericUpDownMartianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.MartianUnit);
			kryptonNumericUpDownJovianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.JovianUnit);
			kryptonNumericUpDownSaturnianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.SaturnianUnit);
			kryptonNumericUpDownUranianUnit.Value = DistanceConverter.Convert(value: kryptonNumericUpDownNeptunianUnit.Value, from: DistanceUnit.NeptunianUnit, to: DistanceUnit.UranianUnit);
		}
		finally
		{
			_isUpdating = false;
		}
	}

	#endregion
}
