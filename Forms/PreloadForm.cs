using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Planetoid_DB
{
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
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog logger instance

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
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
			using FileStream fs = new(path: Path.Combine(path1: outDir, path2: resourceName), mode: FileMode.OpenOrCreate);
			// Ensure the file stream is writable
			using BinaryWriter w = new(output: fs);
			// Read the bytes from the resource stream and write them to the output file
			w.Write(buffer: r.ReadBytes(count: (int)s.Length));
		}

		/// <summary>
		/// Sets the status bar text and enables the information label when text is provided.
		/// </summary>
		/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
		/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
		/// <remarks>
		/// This method is used to set the status bar text and enable the information label.
		/// </remarks>
		private void SetStatusBar(string text, string additionalInfo = "")
		{
			// Check if the text is not null or whitespace
			if (string.IsNullOrWhiteSpace(value: text))
			{
				return;
			}
			// Set the status bar text and enable it
			labelInformation.Enabled = true;
			labelInformation.Text = string.IsNullOrWhiteSpace(value: additionalInfo) ? text : $"{text} - {additionalInfo}";
		}

		/// <summary>
		/// Clears the status bar text and disables the information label.
		/// </summary>
		/// <remarks>
		/// Resets the UI state of the status area so that no message is shown.
		/// Use when there is no status to display or when leaving a control.
		/// </remarks>
		private void ClearStatusBar()
		{
			// Clear the status bar text and disable it
			labelInformation.Enabled = false;
			labelInformation.Text = string.Empty;
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
		private void PreloadForm_Load(object sender, EventArgs e) => ClearStatusBar();

		/// <summary>
		/// Fired when the preload form is closed.
		/// Disposes managed resources associated with the form.
		/// </summary>
		/// <param name="sender">Event source (the form).</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method is called when the preload form is closed.
		/// </remarks>
		private void PreloadForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handlers

		/// <summary>
		/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
		/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
		/// <param name="e">Event arguments.</param>
		/// <remarks>
		/// This method is called when a control is focused.
		/// </remarks>
		private void SetStatusBar_Enter(object sender, EventArgs e)
		{
			// Set the status bar text based on the sender's accessible description
			switch (sender)
			{
				// If the sender is a control with an accessible description, set the status bar text
				// If the sender is a ToolStripItem with an accessible description, set the status bar text
				case Control { AccessibleDescription: not null } control:
					SetStatusBar(text: control.AccessibleDescription);
					break;
				case ToolStripItem { AccessibleDescription: not null } item:
					SetStatusBar(text: item.AccessibleDescription);
					break;
			}
		}

		#endregion

		#region Leave event handlers

		/// <summary>
		/// Called when the mouse pointer leaves a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method is called when the mouse pointer leaves a control.
		/// </remarks>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

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
				Logger.Error(message: "No internet connection");
				ShowErrorMessage(message: I10nStrings.NoInternetConnectionText);
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

		#region DoubleClick event handler

		/// <summary>
		/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>,
		/// its <see cref="Control.Text"/> value is copied to the clipboard using the shared helper.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/>.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method is called when a control is double-clicked.
		/// </remarks>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
		}

		#endregion
	}
}
