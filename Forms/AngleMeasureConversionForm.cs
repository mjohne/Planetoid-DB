/*
 * File:        AngleMeasureConversionForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Converts between angle and time measure.
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
internal partial class AngleMeasureConversionForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	#region Construtor

	/// <summary>Initializes a new instance of the <see cref="AngleMeasureConversionForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public AngleMeasureConversionForm()
	{
		// Log the initialization of the form for debugging purposes
		logger.Info(message: "Initializing AngleMeasureConversionForm...");
		// Initialize the form components
		InitializeComponent();
		// Log that the form has been initialized successfully
		logger.Info(message: "AngleMeasureConversionForm initialized successfully.");
	}

	#endregion

	#region helper methodes

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void AngleMeasureConversionForm_Load(object sender, EventArgs e)
	{
		// Log the form load event for debugging purposes
		logger.Info(message: "AngleMeasureConversionForm loaded.");
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the information.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>If a calculation is running, it is canceled and the form will close after the checking stops. If no checking is running, the it is disposed.</remarks>
	private void AngleMeasureConversionForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log the form closing event for debugging purposes
		logger.Info(message: "AngleMeasureConversionForm is closing.");
		// Play the open sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region Leave as calculate event handlers

	private void RectascensionFromTimeMeasureSexagesimal_Calculate(object sender, EventArgs e)
	{
		SexagesimalAngle<double> sexagesimalAngle = new(
			Degrees: (int)kryptonNumericUpDownRectascensionAngleMeasureSexagesimalDegree.Value,
			Minutes: (int)kryptonNumericUpDownRectascensionAngleMeasureSexagesimalMinute.Value,
			Seconds: (double)kryptonNumericUpDownRectascensionAngleMeasureSexagesimalSecond.Value);

		SexagesimalAngle<decimal> sexagesimalAngleDecimal = new(
			Degrees: (int)kryptonNumericUpDownRectascensionAngleMeasureSexagesimalDegree.Value,
			Minutes: (int)kryptonNumericUpDownRectascensionAngleMeasureSexagesimalMinute.Value,
			Seconds: kryptonNumericUpDownRectascensionAngleMeasureSexagesimalSecond.Value);

		kryptonNumericUpDownRectascensionAngleMeasureDecimalDegree.Value = MeasureConverter.SexagesimalAngleToDecimalAngle(angle: sexagesimalAngleDecimal);

		SexagesimalTime<double> sexagesimalTime = MeasureConverter.SexagesimalAngleToSexagesimalTime(angle: sexagesimalAngle);

		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value = sexagesimalTime.Hours;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value = sexagesimalTime.Minutes;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value = (decimal)sexagesimalTime.Seconds;

		kryptonNumericUpDownRectascensionTimeMeasureDecimalHour.Value = MeasureConverter.SexagesimalAngleToDecimalTime(angle: sexagesimalAngleDecimal);
	}

	private void RectascensionFromAngleMeasureSexagesimal_Calculate(object sender, EventArgs e)
	{
		kryptonNumericUpDownRectascensionTimeMeasureDecimalHour.Value = MeasureConverter.SexagesimalTimeToDecimalTime(time: new SexagesimalTime<decimal>(
					Hours: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value,
					Minutes: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value,
					Seconds: kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value));

		SexagesimalTime<double> sexagesimalTime = new(
			Hours: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value,
			Minutes: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value,
			Seconds: (double)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value);

		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalDegree.Value = MeasureConverter.SexagesimalTimeToSexagesimalAngle(time: sexagesimalTime).Degrees;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalMinute.Value = MeasureConverter.SexagesimalTimeToSexagesimalAngle(time: sexagesimalTime).Minutes;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalSecond.Value = (decimal)MeasureConverter.SexagesimalTimeToSexagesimalAngle(time: sexagesimalTime).Seconds;

		kryptonNumericUpDownRectascensionAngleMeasureDecimalDegree.Value = MeasureConverter.SexagesimalTimeToDecimalAngle(time: new SexagesimalTime<decimal>(
			Hours: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value,
			Minutes: (int)kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value,
			Seconds: kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value));
	}

	private void RectascensionFromTimeMeasureDecimal_Calculate(object sender, EventArgs e)
	{
		decimal decimalHours = kryptonNumericUpDownRectascensionTimeMeasureDecimalHour.Value;

		SexagesimalTime<decimal> sexagesimalTime = MeasureConverter.DecimalTimeToSexagesimalTime(decimalHours: decimalHours);

		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value = sexagesimalTime.Hours;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value = sexagesimalTime.Minutes;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value = sexagesimalTime.Seconds;

		SexagesimalAngle<double> sexagesimalAngle = MeasureConverter.DecimalTimeToSexagesimalAngle(decimalHours: (double)decimalHours);

		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalDegree.Value = sexagesimalAngle.Degrees;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalMinute.Value = sexagesimalAngle.Minutes;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalSecond.Value = (decimal)sexagesimalAngle.Seconds;

		kryptonNumericUpDownRectascensionAngleMeasureDecimalDegree.Value = MeasureConverter.DecimalTimeToDecimalAngle(decimalHours: decimalHours);
	}

	private void RectascensionFromAngleMeasureDecimal_Calculate(object sender, EventArgs e)
	{
		decimal decimalDegrees = kryptonNumericUpDownRectascensionAngleMeasureDecimalDegree.Value;

		SexagesimalAngle<decimal> sexagesimalAngle = MeasureConverter.DecimalAngleToSexagesimalAngle(decimalDegrees: decimalDegrees);

		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalDegree.Value = sexagesimalAngle.Degrees;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalMinute.Value = sexagesimalAngle.Minutes;
		kryptonNumericUpDownRectascensionAngleMeasureSexagesimalSecond.Value = sexagesimalAngle.Seconds;

		kryptonNumericUpDownRectascensionTimeMeasureDecimalHour.Value = MeasureConverter.DecimalAngleToDecimalTime(decimalDegrees: decimalDegrees);

		SexagesimalTime<double> sexagesimalTime = MeasureConverter.DecimalAngleToSexagesimalTime(decimalDegrees: (double)decimalDegrees);

		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalHour.Value = sexagesimalTime.Hours;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalMinute.Value = sexagesimalTime.Minutes;
		kryptonNumericUpDownRectascensionTimeMeasureSexagesimalSecond.Value = (decimal)sexagesimalTime.Seconds;
	}

	private void DeclinationFromAngleMeasureSexagesimal_Calculate(object sender, EventArgs e)
	{
		SexagesimalAngle<decimal> sexagesimalAngle = new(
			Degrees: (int)kryptonNumericUpDownDeclinationAngleMeasureSexagesimalDegree.Value,
			Minutes: (int)kryptonNumericUpDownDeclinationAngleMeasureSexagesimalArcMinute.Value,
			Seconds: kryptonNumericUpDownDeclinationAngleMeasureSexagesimalArcSecond.Value);

		kryptonNumericUpDownDeclinationAngleMeasureDecimalDegree.Value = MeasureConverter.SexagesimalAngleToDecimalAngle(angle: sexagesimalAngle);
	}

	private void DeclinationFromAngleMeasureDecimal_Calculate(object sender, EventArgs e)
	{
		decimal decimalDegrees = kryptonNumericUpDownDeclinationAngleMeasureDecimalDegree.Value;

		SexagesimalAngle<decimal> sexagesimalAngle = MeasureConverter.DecimalAngleToSexagesimalAngle(decimalDegrees: decimalDegrees);

		kryptonNumericUpDownDeclinationAngleMeasureSexagesimalDegree.Value = sexagesimalAngle.Degrees;
		kryptonNumericUpDownDeclinationAngleMeasureSexagesimalArcMinute.Value = sexagesimalAngle.Minutes;
		kryptonNumericUpDownDeclinationAngleMeasureSexagesimalArcSecond.Value = sexagesimalAngle.Seconds;
	}

	#endregion

}
