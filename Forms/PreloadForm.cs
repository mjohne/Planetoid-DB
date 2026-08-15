/*
 * File:        PreloadForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: A form that gets the file MPCORB.DAT.
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

using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>A form that gets the file MPCORB.DAT.</summary>
/// <remarks>This form is responsible for preloading the necessary data files for the application.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class PreloadForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for logging events and errors.</summary>
	/// <remarks>This logger is used to log events and errors that occur within the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the file path of the MPCORB.DAT file.</summary>
	/// <remarks>This property is used to get the file path of the MPCORB.DAT file.</remarks>
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Hidden)]
	public string MpcOrbDatFilePath { get; set; } = string.Empty;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="PreloadForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public PreloadForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom display string for the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Safely extracts resource data bytes to the specified output file path.</summary>
	/// <param name="resourceData">The byte array containing the resource data.</param>
	/// <param name="outputFilePath">The full path where the resource will be written.</param>
	/// <exception cref="ArgumentNullException">Thrown if resourceData is null.</exception>
	/// <remarks>This method writes the provided byte array to the specified file path.</remarks>
	private static bool TryExtractResource(byte[] resourceData, string outputFilePath)
	{
		// Validate input
		ArgumentNullException.ThrowIfNull(argument: resourceData);
		try
		{
			// Write the resource data to the specified output file path
			File.WriteAllBytes(path: outputFilePath, bytes: resourceData);
			return true;
		}
		// Catch specific exceptions related to file I/O and access permissions
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			// Log the error and return false if an exception occurs during file writing
			logger.Error(exception: ex, message: $"Failed to extract resource file to '{outputFilePath}'.");
			return false;
		}

	}

	/// <summary>Opens a file dialog to allow the user to select a local MPCORB.DAT file. If a file is selected, sets the <see cref="MpcOrbDatFilePath"/> property to the selected file path and closes the form with <see cref="DialogResult.OK"/>.</summary>
	/// <remarks>This method is called when the user chooses to open a local file.</remarks>
	private void OpenLocalFile()
	{
		// Create an OpenFileDialog to select a local file
		if (openFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			logger.Warn(message: "User canceled the Open Local File dialog.");
			return;
		}
		logger.Info(message: $"User selected local file '{openFileDialog.FileName}'");
		// Set the file path to the selected file
		_ = MpcOrbDatFilePath = openFileDialog.FileName;
		// Set the dialog result to OK
		DialogResult = DialogResult.OK;
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the preload form has finished loading. Clears the status area so no message is shown on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the preload form has finished loading.</remarks>
	private void PreloadForm_Load(object sender, EventArgs e)
	{
		// Log that the preload form has loaded successfully
		logger.Info(message: "PreloadForm loaded successfully.");
		ClearStatusBar(label: labelInformation);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Open Local File command link. Shows an <see cref="OpenFileDialog"/> to let the user select a local MPCORB.DAT file and, if a file is selected, stores its path in <see cref="MpcOrbDatFilePath"/> and closes the dialog with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Open Local File command link is clicked.</remarks>
	private void KryptonCommandLinkButtonOpenLocalFile_Click(object sender, EventArgs e)
	{
		logger.Info(message: "User clicked 'Open Local File' command link.");
		OpenLocalFile();
	}

	/// <summary>Handles the click event for downloading the MPCORB.DAT file. Checks network availability and opens the download dialog; if the download completes successfully, sets <see cref="MpcOrbDatFilePath"/> to the downloaded file and closes the form with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Download MPCORB.DAT command link is clicked.</remarks>
	private void KryptonCommandLinkButtonDownloadMpcorbDat_Click(object sender, EventArgs e)
	{
		logger.Info(message: "User clicked 'Download MPCORB.DAT' command link.");
		// Check if there is an internet connection available
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log the error and show an error message if there is no internet connection
			logger.Error(message: "No internet connection");
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		// Resolve and validate the download URL, falling back to the default MPC URL if necessary
		string mpcorbUrl = Settings.Default.systemMpcorbDatGzUrl;
		if (string.IsNullOrWhiteSpace(value: mpcorbUrl) || !Uri.TryCreate(uriString: mpcorbUrl, uriKind: UriKind.Absolute, result: out Uri? parsedUri) || parsedUri.Scheme != Uri.UriSchemeHttps)
		{
			// Log a warning and use the default MPC URL as a fallback
			logger.Warn(message: $"systemMpcorbDatGzUrl setting is invalid ('{mpcorbUrl}'). Falling back to default MPC URL.");
			mpcorbUrl = "https://www.minorplanetcenter.org/iau/MPCORB/MPCORB.DAT.gz";
		}
		logger.Info(message: $"Using MPCORB.DAT download URL: '{mpcorbUrl}'");
		// Open the download form for MPCORB.DAT
		using DatabaseDownloaderForm formDownloaderForMpcorbDat = new(url: mpcorbUrl);
		// Show the form as a dialog
		if (formDownloaderForMpcorbDat.ShowDialog(owner: this) == DialogResult.OK)
		{
			// Set the file path to the downloaded MPCORB.DAT file
			_ = MpcOrbDatFilePath = Settings.Default.systemFilenameMpcorbDat;
			// Set the dialog result to OK
			DialogResult = DialogResult.OK;
		}
	}

	/// <summary>Handles the click event for loading internal demo data. Extracts an embedded demo data file to the application's working directory, sets <see cref="MpcOrbDatFilePath"/> to the extracted filename and closes the dialog with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Load Internal Demo Data command link is clicked.</remarks>
	private void KryptonCommandLinkButtonLoadInternalDemoData_Click(object sender, EventArgs e)
	{
		logger.Info(message: "User clicked 'Load Internal Demo Data' command link.");
		// Define the output file name
		string outputFileName = "demoset-10000.txt";
		// Combine the application's startup path with the output file name to get the full output path
		string outputPath = Path.Combine(path1: Application.StartupPath, path2: outputFileName);
		// Attempt to extract the embedded demo data resource to the output path
		if (TryExtractResource(resourceData: Properties.Resources.demoset_10000, outputFilePath: outputPath))
		{
			logger.Info(message: $"Successfully extracted demo data to '{outputPath}'");
			// Set the file path to the extracted demo data file
			_ = MpcOrbDatFilePath = outputPath;
			DialogResult = DialogResult.OK;
			// Set the dialog result to OK
		}
		// If extraction fails, log an error and show an error message
		else
		{
			ShowErrorMessage(message: $"Failed to extract demo data to '{outputPath}'.");
			logger.Error(message: $"Failed to extract demo data to '{outputPath}'.");
		}
	}

	#endregion

	#region Drag-and-drop event handlers

	/// <summary>Handles the DragEnter event. Updates the status label and sets the drag effect based on the data being dragged.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="DragEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when a drag operation enters the form area.</remarks>
	private void PreloadForm_DragEnter(object sender, DragEventArgs e)
	{
		// Log that the DragEnter event has been triggered
		logger.Info(message: "DragEnter event triggered.");
		// Update the status label to inform the user about the supported file type
		labelInformation.Text = "Drag and drop only MPCORB.DAT files are supported.";
		// Check if the data being dragged is null and log a warning if it is
		if (e.Data is null)
		{
			logger.Warn(message: "DragEventArgs.Data is null.");
			return;
		}
		// Set the drag effect to Copy if the data being dragged is a file drop, otherwise set it to None
		e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
	}

	/// <summary>Handles the DragLeave event. Clears the status label when the drag operation leaves the form.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the drag operation leaves the form area.</remarks>
	private void PreloadForm_DragLeave(object sender, EventArgs e)
	{
		// Log that the DragLeave event has been triggered
		logger.Info(message: "DragLeave event triggered.");
		// Clear the status label to remove any drag-and-drop information
		labelInformation.Text = string.Empty;
	}

	/// <summary>Handles the DragDrop event. Validates that a file was dropped and sets <see cref="MpcOrbDatFilePath"/> to the first dropped file, then closes the form with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="DragEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when a file is dropped onto the preload form.</remarks>
	private void PreloadForm_DragDrop(object sender, DragEventArgs e)
	{
		// Log that the DragDrop event has been triggered
		logger.Info(message: "DragDrop event triggered.");
		// Clear the status label to remove any drag-and-drop information
		labelInformation.Text = string.Empty;
		// Check if the data being dropped is null and log a warning if it is
		if (e.Data is null)
		{
			logger.Warn(message: "DragEventArgs.Data is null.");
			return;
		}
		// Check if the data being dropped is a file drop; if not, log a warning and return
		if (!e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			logger.Warn(message: "No file drop detected.");
			return;
		}
		// Log that a file drop has been detected
		logger.Info(message: "File drop detected.");
		// Attempt to retrieve the dropped file paths; if unsuccessful, log a warning and return
		if (e.Data.GetData(DataFormats.FileDrop) is not string[] { Length: > 0 } files)
		{
			logger.Warn(message: "Could not retrieve dropped file paths.");
			return;
		}
		// Log the first dropped file path and set the MpcOrbDatFilePath property to it
		logger.Info(message: $"User dropped file '{files[0]}'.");
		_ = MpcOrbDatFilePath = files[0];
		DialogResult = DialogResult.OK;
	}

	#endregion
}
