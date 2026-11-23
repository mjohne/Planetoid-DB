/*
 * File:        AcknowledgementsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Displays acknowledgements and opens related astronomy organization websites.
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

/// <summary>Displays acknowledgements and opens related astronomy organization websites.</summary>
/// <remarks>Provides controls for opening the Minor Planet Center, International Astronomical Union, and Smithsonian Astrophysical Observatory websites.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class AcknowledgementsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the AcknowledgementsForm class.</summary>
	public AcknowledgementsForm()
	{
		// Log that the AcknowledgementsForm is being initialized
		logger.Info(message: "Initializing AcknowledgementsForm...");
		// Initialize the form components
		InitializeComponent();
		// Log that the AcknowledgementsForm has been initialized successfully
		logger.Info(message: "AcknowledgementsForm initialized.");
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
	private void AcknowledgementsForm_Load(object sender, EventArgs e)
	{
		// Log that the form has loaded successfully
		logger.Info(message: "AcknowledgementsForm loaded successfully.");
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
	private void AcknowledgementsForm_Closing(object sender, FormClosingEventArgs e)
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

	#region Click event handlers

	/// <summary>Handles the click event for the KryptonButtonOpenWebsiteMpc. Opens the Minor Planet Center website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Minor Planet Center website.</remarks>
	private void KryptonButtonOpenWebsiteMpc_Click(object sender, EventArgs e)
	{
		string website = "https://minorplanetcenter.net/";
		logger.Info(message: $"Opening Minor Planet Center website: {website}");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		OpenWebsite(fileName: website);
		AudioPlayback.PlaySendDataSound();
	}

	/// <summary>Handles the click event for the KryptonButtonOpenWebsiteIau. Opens the International Astronomical Union website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the International Astronomical Union website.</remarks>
	private void KryptonButtonOpenWebsiteIau_Click(object sender, EventArgs e)
	{
		string website = "https://www.iau.org/";
		logger.Info(message: $"Opening International Astronomical Union website: {website}");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		OpenWebsite(fileName: website);
		AudioPlayback.PlaySendDataSound();
	}

	/// <summary>Handles the click event for the KryptonButtonOpenWebsite. Opens the Smithsonian Astrophysical Observatory website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Smithsonian Astrophysical Observatory website.</remarks>
	private void KryptonButtonOpenWebsiteSao_Click(object sender, EventArgs e)
	{
		string website = "https://www.cfa.harvard.edu/sao/";
		logger.Info(message: $"Opening Smithsonian Astrophysical Observatory website: {website}");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		OpenWebsite(fileName: website);
		AudioPlayback.PlaySendDataSound();
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
