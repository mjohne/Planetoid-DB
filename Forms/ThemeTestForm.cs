/*
 * File:        ThemeTestForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Provides a form for testing themes.
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

/// <summary>Provides a form for testing themes.</summary>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public partial class ThemeTestForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ThemeTestForm"/> class.</summary>
	public ThemeTestForm()
	{
		// Log the initialization
		logger.Info(message: "Initializing ThemeTestForm...");
		// Initialize the form components
		InitializeComponent();
		// Log the initialization
		logger.Info(message: "ThemeTestForm initialized.");
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a custom display string for the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	#endregion

	#region Form event handlers

	/// <summary>Fired when the preload form has finished loading. Clears the status area so no message is shown on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and set up any necessary data.</remarks>
	private void ThemeTestForm_Load(object sender, EventArgs e)
	{
		// Log that the ThemeTestForm has loaded successfully
		logger.Info(message: "ThemeTestForm loaded successfully.");
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
	private void ThemeTestForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing
		logger.Info(message: "ThemeTestForm is closing.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region SelectedValueChanged event handlers

	/// <summary>Logs the selected theme when a theme is selected.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void KryptonThemeListBox_SelectedValueChanged(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (kryptonThemeListBox.SelectedItem != null)
		{
			string selectedText = kryptonThemeListBox.SelectedItem.ToString() ?? string.Empty;
			logger.Info($"Select theme item: {selectedText}.");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Shows or hides the information label according to the menu item's checked state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemShowInformationLabel_Click(object sender, EventArgs e)
	{
		// Log the action of clicking of ToolStripMenuItemShowInformationLabel
		logger.Info(message: $"ToolStripMenuItemShowInformationLabel clicked.");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		labelInformation.Visible = menuItemShowInformationLabel.Checked;
		// Log the new state of the visibility of the information label
		logger.Info(message: $"Set the visibility of the information label to {labelInformation.Visible}.");
	}

	#endregion
}
