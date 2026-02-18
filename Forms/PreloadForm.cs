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
using System.Reflection;

namespace Planetoid_DB;

/// <summary>
/// A form that gets the file MPCORB.DAT.
/// </summary>
/// <remarks>
/// This form is responsible for preloading the necessary data files for the application.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class PreloadForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for logging events and errors.
	/// </summary>
	/// <remarks>
	/// This logger is used to log events and errors that occur within the form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="PreloadForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public PreloadForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a custom display string for the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Extracts an embedded resource from the assembly and writes it to a specified output directory.
	/// </summary>
	/// <param name="nameSpace">The namespace where the resource is located.</param>
	/// <param name="outDir">The output directory where the resource will be written.</param>
	/// <param name="internFilePath">The internal file path within the namespace (optional).</param>
	/// <param name="resourceName">The name of the resource to extract.</param>
	/// <exception cref="FileNotFoundException">Thrown if the specified resource is not found in the assembly.</exception>
	/// <remarks>
	/// This method extracts an embedded resource from the assembly and writes it to the specified output directory.
	/// </remarks>
	private static void ExtractResource(string nameSpace, string outDir, string internFilePath, string resourceName)
	{
		// Get the assembly and the resource path
		Assembly assembly = Assembly.GetExecutingAssembly();
		// Construct the resource path
		string resourcePath = $"{nameSpace}.{(string.IsNullOrEmpty(value: internFilePath) ? "" : internFilePath + ".")}{resourceName}";
		// Open the resource stream and read the bytes
		using Stream s = assembly.GetManifestResourceStream(name: resourcePath) ?? throw new FileNotFoundException(message: $"Resource '{resourcePath}' not found in assembly.");
		// Create the output file stream
		using BinaryReader r = new(input: s);
		// Create the output file stream and write the bytes to it
		using FileStream fs = new(path: Path.Combine(path1: outDir, path2: resourceName), mode: FileMode.Create);
		// Ensure the file stream is writable
		using BinaryWriter w = new(output: fs);
		// Read the bytes from the resource stream and write them to the output file
		w.Write(buffer: r.ReadBytes(count: (int)s.Length));
	}

	/// <summary>
	/// Gets the file path of the MPCORB.DAT file.
	/// </summary>
	/// <remarks>
	/// This property is used to get the file path of the MPCORB.DAT file.
	/// </remarks>
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Hidden)]
	public string MpcOrbDatFilePath { get; set; } = string.Empty;

	#endregion

	#region form event handler

	/// <summary>
	/// Fired when the preload form has finished loading.
	/// Clears the status area so no message is shown on startup.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the preload form has finished loading.
	/// </remarks>
	private void PreloadForm_Load(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the Open Local File command link.
	/// Shows an <see cref="OpenFileDialog"/> to let the user select a local MPCORB.DAT file and,
	/// if a file is selected, stores its path in <see cref="MpcOrbDatFilePath"/> and closes the dialog with <see cref="DialogResult.OK"/>.
	/// </summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the Open Local File command link is clicked.
	/// </remarks>
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

	/// <summary>
	/// Handles the click event for downloading the MPCORB.DAT file.
	/// Checks network availability and opens the download dialog; if the download completes successfully,
	/// sets <see cref="MpcOrbDatFilePath"/> to the downloaded file and closes the form with <see cref="DialogResult.OK"/>.
	/// </summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the Download MPCORB.DAT command link is clicked.
	/// </remarks>
	private void KryptonCommandLinkButtonDownloadMprcorbDat_Click(object sender, EventArgs e)
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log the error and show an error message if there is no internet connection
			logger.Error(message: "No internet connection");
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Open the download form for MPCORB.DAT
			using DownloadMpcorbDatForm formDownloaderForMpcorbDat = new();
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

	/// <summary>
	/// Handles the click event for loading internal demo data.
	/// Extracts an embedded demo data file to the application's working directory,
	/// sets <see cref="MpcOrbDatFilePath"/> to the extracted filename and closes the dialog with <see cref="DialogResult.OK"/>.
	/// </summary>
	/// <param name="sender">Event source (the command link button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the Load Internal Demo Data command link is clicked.
	/// </remarks>
	private void KryptonCommandLinkButtonLoadInternalDemoData_Click(object sender, EventArgs e)
	{
		// Extract the demo data file from the embedded resources
		ExtractResource(nameSpace: "Planetoid_DB", outDir: "", internFilePath: "Resources", resourceName: "demoset-10000.txt");
		// Set the file path to the extracted demo data file
		_ = MpcOrbDatFilePath = "demoset-10000.txt";
		// Set the dialog result to OK
		DialogResult = DialogResult.OK;
	}

	#endregion
}