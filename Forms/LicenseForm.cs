using Planetoid_DB.Forms;

using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Planetoid_DB;

/// <summary>
/// A form that displays application information.
/// </summary>
/// <remarks>
/// This form is used to display information about the application, including version and copyright details.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class LicenseForm : BaseKryptonForm
{
	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public LicenseForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

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
	/// Extracts an embedded resource from the executing assembly and writes it to the specified output directory.
	/// </summary>
	/// <param name="nameSpace">The root namespace where the resource is located.</param>
	/// <param name="outDir">The output directory where the resource will be written. Directory will be created if it does not exist.</param>
	/// <param name="internFilePath">Optional internal path within the namespace (e.g. "Resources").</param>
	/// <param name="resourceName">The name of the resource to extract (including extension).</param>
	/// <exception cref="FileNotFoundException">Thrown when the specified resource is not found in the assembly.</exception>
	/// <remarks>
	/// This method is used to extract an embedded resource from the assembly and write it to the specified output directory.
	/// </remarks>
	private static void ExtractResource(string nameSpace, string outDir, string internFilePath, string resourceName)
	{
		// Get the assembly and the resource path
		Assembly assembly = Assembly.GetExecutingAssembly();
		// Construct the resource path
		string resourcePath = $"{nameSpace}.{(string.IsNullOrEmpty(value: internFilePath) ? "" : internFilePath + ".")}{resourceName}";
		// Check if the output directory exists, if not create it
		if (!Directory.Exists(path: outDir))
		{
			_ = Directory.CreateDirectory(path: outDir);
		}
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

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the preload form has finished loading.
	/// Clears the status area so no message is shown on startup.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form and set up any necessary data.
	/// </remarks>
	private void LicenseForm_Load(object sender, EventArgs e) => ClearStatusBar();

	/// <summary>
	/// Fired when the license form is closed.
	/// Disposes managed resources associated with the form.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to release any resources held by the form.
	/// </remarks>
	private void LicenseForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is used to set the status bar text when a control or ToolStrip item is focused.
	/// </remarks>
	private void SetStatusBar_Enter(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the accessible description based on the sender type
		string? description = sender switch
		{
			Control c => c.AccessibleDescription,
			ToolStripItem t => t.AccessibleDescription,
			_ => null
		};
		// If we have a description, set it in the status bar
		if (description != null)
		{
			SetStatusBar(text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is used to clear the status bar text when the mouse leaves a control or the control loses focus.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Save License button click.
	/// Prompts the user for a destination via <see cref="SaveFileDialog"/>, extracts the embedded LICENSE resource
	/// to a temporary file and copies it to the selected destination.
	/// </summary>
	/// <param name="sender">Event source (the save button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <exception cref="FileNotFoundException">Thrown when the embedded LICENSE resource cannot be found.</exception>
	/// <exception cref="IOException">Propagated when file copy or delete operations fail.</exception>
	/// <remarks>
	/// This method is used to save the LICENSE file to a user-specified location.
	/// </remarks>
	private void KryptonButtonSaveLicense_Click(object sender, EventArgs e)
	{
		// Create a SaveFileDialog to prompt the user for a file location
		if (saveFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Get the selected file name
		string fullFileName = saveFileDialog.FileName;
		// Extract the LICENSE file from the embedded resources and copy it to the selected file location
		ExtractResource(nameSpace: "Planetoid_DB", outDir: Path.GetDirectoryName(path: fullFileName) ?? string.Empty, internFilePath: "", resourceName: "LICENSE");
		// Copy the LICENSE file to the selected file location
		File.Copy(sourceFileName: Path.Combine(path1: Path.GetDirectoryName(path: fullFileName) ?? string.Empty, path2: "LICENSE"), destFileName: fullFileName, overwrite: true);
		// Remove the temporary extracted LICENSE file
		File.Delete(path: Path.Combine(path1: Path.GetDirectoryName(path: fullFileName) ?? string.Empty, path2: "LICENSE"));
	}

	/// <summary>
	/// Handles the Copy License to Clipboard button click.
	/// Copies the contents of <c>kryptonTextBoxLicense.Text</c> to the clipboard.
	/// </summary>
	/// <param name="sender">Event source (the copy button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the license text to the clipboard.
	/// </remarks>
	private void KryptonButtonCopyLicenseToClipboard_Click(object sender, EventArgs e) =>
		// Copy the text from the KryptonTextBox to the clipboard
		CopyToClipboard(text: kryptonTextBoxLicense.Text);

	#endregion
}