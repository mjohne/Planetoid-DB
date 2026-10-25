/*
 * File:        RegionCheckerForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Provides information on minor planets located in a specific region of the sky.
 * Remarks:     The user chooses a time range (UTC), a step size, the observer location and visibility criteria. The calculation runs asynchronously via <see cref="EphemerisService"/> and can be canceled; results are shown in a list and can be exported in multiple formats.
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

using NLog;

using Planetoid_DB.Helpers;
using Planetoid_DB.Services;

using System.Diagnostics;
using System.Numerics;

namespace Planetoid_DB.Forms;

/// <summary>Provides information on minor planets located in a specific region of the sky.</summary>
/// <remarks>The user chooses a time range (UTC), a step size, the observer location and visibility criteria. The calculation runs asynchronously via <see cref="EphemerisService"/> and can be canceled; results are shown in a list and can be exported in multiple formats.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class RegionCheckerForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="RegionCheckerForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public RegionCheckerForm()
	{
		// Log the initialization of the form for debugging purposes
		logger.Info(message: "Initializing RegionCheckerForm.");
		// Initialize the form components
		InitializeComponent();
		// Log that the form has been initialized successfully
		logger.Info(message: "RegionCheckerForm initialized successfully.");
	}

	#endregion

	#region helper methods

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	public static bool IsInside<T>(T rightascension, T declination, T minRightascension, T maxRightascension, T minDeclination, T maxDeclination, bool inclusive = true) where T : INumber<T>
	{
		return inclusive
			? rightascension >= minRightascension && rightascension <= maxRightascension && declination >= minDeclination && declination <= maxDeclination
			: rightascension > minRightascension && rightascension < maxRightascension && declination > minDeclination && declination < maxDeclination;
	}

	public static (T swappedA, T swappedB, bool wasChanged) SwapIfGreater<T>(T a, T b) where T : IComparable<T>
	{
		// Returns a value > 0 if 'a' is greater than 'b'
		bool wasChanged = a.CompareTo(other: b) > 0;
		// If 'a' is greater, swap them (b, a). Otherwise, leave them as they are (a, b).
		return wasChanged ? (b, a, true) : (a, b, false);
	}

	private void SwapIfGreaterAtValues(decimal value1, decimal value2, string text, string caption)
	{
		if (string.IsNullOrEmpty(value: text))
		{
			logger.Warn("Text is null or empty.");
			text = "";
		}
		if (string.IsNullOrEmpty(value: caption))
		{
			logger.Warn("Caption is null or empty.");
			caption = "";
		}
		(decimal result1, decimal result2, bool isGreater) = SwapIfGreater(a: value1, b: value2);
		if (isGreater)
		{
			kryptonNumericUpDownRectascensionMinimum.Value = result2;
			kryptonNumericUpDownRectascensionMaximum.Value = result1;
			logger.Info($"Values swapped: {result1} <-> {result2}");
			_ = KryptonMessageBox.Show(text: text, caption: caption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
	}

	private void SwaptIfGreaterAtRectascensionValues()
	{
		SwapIfGreaterAtValues(
			value1: kryptonNumericUpDownDeclinationMinimum.Value,
			value2: kryptonNumericUpDownDeclinationMaximum.Value,
			text: "The two right ascension values were swapped, as the maximum value had been entered first instead of the minimum value.",
			caption: "Values swapped");
	}

	private void SwaptIfGreaterAtDeclinationValues()
	{
		SwapIfGreaterAtValues(
			value1: kryptonNumericUpDownRectascensionMinimum.Value,
			value2: kryptonNumericUpDownRectascensionMaximum.Value,
			text: "The two declination values were swapped, as the maximum value had been entered first instead of the minimum value.",
			caption: "Values swapped");
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void RegionCheckerForm_Load(object sender, EventArgs e)
	{
		// Log the form load event for debugging purposes
		logger.Info(message: "RegionCheckerForm loaded.");
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the information.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>If a calculation is running, it is canceled and the form will close after the checking stops. If no checking is running, the it is disposed.</remarks>
	private void RegionCheckerForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		// Log the form closing event for debugging purposes
		logger.Info(message: "RegionCheckerForm is closing.");
		// Play the open sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region Click event handlers

	private void ToolStripButtonCheck_Click(object sender, EventArgs e)
	{
	}

	private void ToolStripButtonGoToObject_Click(object sender, EventArgs e)
	{
	}

	private void KryptonButtonConversionRectascension_Click(object sender, EventArgs e)
	{
	}

	private void KryptonButtonConversionDeclination_Click(object sender, EventArgs e)
	{
	}

	#endregion

	#region Leave event handlers

	private void KryptonNumericUpDownRectascensionMinimum_Leave(object sender, EventArgs e)
	{
		SwaptIfGreaterAtRectascensionValues();
		Control_Leave(sender: sender, e: e);
	}

	private void KryptonNumericUpDownDeclinationMinimum_Leave(object sender, EventArgs e)
	{
		SwaptIfGreaterAtDeclinationValues();
		Control_Leave(sender: sender, e: e);
	}

	private void KryptonNumericUpDownRectascensionMaximum_Leave(object sender, EventArgs e)
	{
		SwaptIfGreaterAtRectascensionValues();
		Control_Leave(sender: sender, e: e);
	}

	private void KryptonNumericUpDownDeclinationMaximum_Leave(object sender, EventArgs e)
	{
		SwaptIfGreaterAtDeclinationValues();
		Control_Leave(sender: sender, e: e);
	}

	#endregion
}
