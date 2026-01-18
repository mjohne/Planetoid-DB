using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Planetoid_DB
{
	/// <summary>
	/// A form that displays application information.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class LicenseForm : BaseKryptonForm
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
		/// </summary>
		public LicenseForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region helper methods

		/// <summary>
		/// Returns a short debugger display string for this instance.
		/// </summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the status bar text.
		/// </summary>
		/// <param name="text">The main text to be displayed on the status bar.</param>
		/// <param name="additionalInfo">Additional information to be displayed alongside the main text.</param>
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
		/// Clears the status bar text.
		/// </summary>
		private void ClearStatusBar()
		{
			// Clear the status bar text and disable it
			labelInformation.Enabled = false;
			labelInformation.Text = string.Empty;
		}

		/// <summary>
		/// Extracts an embedded resource from the assembly and writes it to a specified output directory.
		/// </summary>
		/// <param name="nameSpace">The namespace where the resource is located.</param>
		/// <param name="outDir">The output directory where the resource will be written.</param>
		/// <param name="internFilePath">The internal file path within the namespace (optional).</param>
		/// <param name="resourceName">The name of the resource to extract.</param>
		/// <exception cref="FileNotFoundException">Thrown if the specified resource is not found in the assembly.</exception>
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

		#region form event handler

		/// <summary>
		/// Fired when the form loads.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void LicenseForm_Load(object sender, EventArgs e) => ClearStatusBar();

		/// <summary>
		/// Fired when the form closes.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void LicenseForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region enter event handlers

		/// <summary>
		/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
		/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
		/// <param name="e">Event arguments.</param>
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

		#region leave event handlers

		/// <summary>
		/// Called when the mouse pointer leaves a control or the control loses focus.
		/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region click event handlers

		/// <summary>
		/// Saves the license to a file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
			// Set the status bar text to indicate that the file has been saved
			File.Delete(path: Path.Combine(path1: Path.GetDirectoryName(path: fullFileName) ?? string.Empty, path2: "LICENSE"));
		}

		private void KryptonButtonCopyLicenseToClipboard_Click(object sender, EventArgs e) =>
			// Copy the text from the KryptonTextBox to the clipboard
			CopyToClipboard(text: kryptonTextBoxLicense.Text);

		#endregion
	}
}
