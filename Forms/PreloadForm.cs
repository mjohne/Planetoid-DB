// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
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

	/// <summary>Extracts resource data (byte array) and writes it to the specified output file path.</summary>
	/// <param name="resourceData">The byte array containing the resource data.</param>
	/// <param name="outputFilePath">The full path where the resource will be written.</param>
	/// <exception cref="ArgumentNullException">Thrown if resourceData is null.</exception>
	/// <remarks>This method writes the provided byte array to the specified file path.</remarks>
	private static void ExtractResource(byte[] resourceData, string outputFilePath)
	{
		// Validate input
		ArgumentNullException.ThrowIfNull(argument: resourceData);
		// Write the resource data to the output file
		File.WriteAllBytes(path: outputFilePath, bytes: resourceData);
	}

	/// <summary>Gets the file path of the MPCORB.DAT file.</summary>
	/// <remarks>This property is used to get the file path of the MPCORB.DAT file.</remarks>
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Hidden)]
	public string MpcOrbDatFilePath { get; set; } = string.Empty;

	#endregion

	#region form event handlers

	/// <summary>Fired when the preload form has finished loading. Clears the status area so no message is shown on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the preload form has finished loading.</remarks>
	private void PreloadForm_Load(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Open Local File command link. Shows an <see cref="OpenFileDialog"/> to let the user select a local MPCORB.DAT file and, if a file is selected, stores its path in <see cref="MpcOrbDatFilePath"/> and closes the dialog with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Open Local File command link is clicked.</remarks>
	private void KryptonCommandLinkButtonOpenLocalFile_Click(object sender, EventArgs e)
	{
		// Create an OpenFileDialog to select a local file
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Set the file path to the selected file
		_ = MpcOrbDatFilePath = openFileDialog.FileName;
		// Set the dialog result to OK
		DialogResult = DialogResult.OK;
	}

	/// <summary>Handles the click event for downloading the MPCORB.DAT file. Checks network availability and opens the download dialog; if the download completes successfully, sets <see cref="MpcOrbDatFilePath"/> to the downloaded file and closes the form with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Download MPCORB.DAT command link is clicked.</remarks>
	private void KryptonCommandLinkButtonDownloadMprcorbDat_Click(object sender, EventArgs e)
	{
		// Check if there is an internet connection available
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log the error and show an error message if there is no internet connection
			logger.Error(message: "No internet connection");
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Resolve and validate the download URL, falling back to the default MPC URL if necessary
			string mpcorbUrl = Settings.Default.systemMpcorbDatGzUrl;
			if (string.IsNullOrWhiteSpace(value: mpcorbUrl) || !Uri.TryCreate(uriString: mpcorbUrl, uriKind: UriKind.Absolute, result: out _))
			{
				// Log a warning and use the default MPC URL as a fallback
				logger.Warn(message: $"systemMpcorbDatGzUrl setting is invalid ('{mpcorbUrl}'). Falling back to default MPC URL.");
				mpcorbUrl = "http://www.minorplanetcenter.org/iau/MPCORB/MPCORB.DAT.gz";
			}
			// Open the download form for MPCORB.DAT
			using DatabaseDownloaderForm formDownloaderForMpcorbDat = new(url: mpcorbUrl);
			// Show the form as a dialog
			if (formDownloaderForMpcorbDat.ShowDialog() == DialogResult.OK)
			{
				// Set the file path to the downloaded MPCORB.DAT file
				_ = MpcOrbDatFilePath = Settings.Default.systemFilenameMpcorb;
				// Set the dialog result to OK
				DialogResult = DialogResult.OK;
			}
		}
	}

	/// <summary>Handles the click event for loading internal demo data. Extracts an embedded demo data file to the application's working directory, sets <see cref="MpcOrbDatFilePath"/> to the extracted filename and closes the dialog with <see cref="DialogResult.OK"/>.</summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Load Internal Demo Data command link is clicked.</remarks>
	private void KryptonCommandLinkButtonLoadInternalDemoData_Click(object sender, EventArgs e)
	{
		// Define the output file name
		string outputFileName = "demoset-10000.txt";
		// Extract the demo data file from the embedded resources using the strongly-typed Resources class
		ExtractResource(resourceData: Properties.Resources.demoset_10000, outputFilePath: outputFileName);
		// Set the file path to the extracted demo data file
		_ = MpcOrbDatFilePath = outputFileName;
		// Set the dialog result to OK
		DialogResult = DialogResult.OK;
	}

	#endregion
}
