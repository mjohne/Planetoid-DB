/*
 * File:        SoundTestForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Displays controls for testing application sounds.
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

/// <summary>Displays controls for testing application sounds.</summary>
/// <remarks>This form is used to display information about the application, including version and copyright details.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public partial class SoundTestForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="SoundTestForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public SoundTestForm()
	{
		// Log that the SoundTestForm is being initialized
		logger.Info(message: "Initializing SoundTestForm...");
		// Initialize the form components
		InitializeComponent();
		// Log that the SoundTestForm has been initialized successfully
		logger.Info(message: "SoundTestForm initialized.");
	}

	#endregion

	#region Helpers

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	#endregion

	#region form event handlers

	/// <summary>Fired when the preload form has finished loading. Clears the status area so no message is shown on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and set up any necessary data.</remarks>
	private void SoundTestForm_Load(object sender, EventArgs e)
	{
		// Log that the SoundTestForm has loaded successfully
		logger.Info(message: "SoundTestForm loaded successfully.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar to ensure no message is displayed on startup
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Logs the closing event and plays the close sound when the license form closes.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data for the form-closing operation.</param>
	/// <remarks>Logs the closing event and plays the close sound to provide closure feedback.</remarks>
	private void SoundTestForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing
		logger.Info(message: "SoundTestForm is closing.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region Click event handlers

	/// <summary>Logs the button activation and plays the start sound.</summary>
	/// <param name="sender">The source of the click event.</param>
	/// <param name="e">The event data associated with the click.</param>
	private void KryptonButtonPlayStartSound_Click(object sender, EventArgs e)
	{
		logger.Info(message: "KryptonButtonPlayStartSound clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayStartSound();
	}

	/// <summary>Logs the button activation and plays the send data sound.</summary>
	/// <param name="sender">The source of the click event.</param>
	/// <param name="e">The event data associated with the click.</param>
	private void KryptonButtonPlaySendDataSound_Click(object sender, EventArgs e)
	{
		logger.Info(message: "KryptonButtonPlaySenDataSound clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the send data sound in the background to indicate that the form has been send data
		AudioPlayback.PlaySendDataSound();
	}

	/// <summary>Logs the button activation and plays the shut down sound.</summary>
	/// <param name="sender">The source of the click event.</param>
	/// <param name="e">The event data associated with the click.</param>
	private void KryptonButtonPlayShutDownSound_Click(object sender, EventArgs e)
	{
		logger.Info(message: "KryptonButtonPlayShutDownSound clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the send data sound in the background to indicate that the form has been shut down
		_ = AudioPlayback.PlayShutDownSound();
	}

	/// <summary>Logs the button activation and plays the open sound.</summary>
	/// <param name="sender">The source of the click event.</param>
	/// <param name="e">The event data associated with the click.</param>
	private void KryptonButtonPlayOpenSound_Click(object sender, EventArgs e)
	{
		logger.Info(message: "KryptonButtonPlayOpenSound clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
	}

	/// <summary>Logs the button activation and plays the close sound.</summary>
	/// <param name="sender">The source of the click event.</param>
	/// <param name="e">The event data associated with the click.</param>
	private void KryptonButtonPlayCloseSound_Click(object sender, EventArgs e)
	{
		logger.Info(message: "KryptonButtonPlayCloseSound clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

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
