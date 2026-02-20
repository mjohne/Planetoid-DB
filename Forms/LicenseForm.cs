// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Planetoid_DB.Forms;

using System.Reflection;

namespace Planetoid_DB;

/// <summary>
/// A form that displays application information.
/// </summary>
/// <remarks>
/// This form is used to display information about the application, including version and copyright details.
/// </remarks>
public partial class LicenseForm : BaseKryptonForm
{
	/// <summary>
	/// The root namespace for embedded resources.
	/// </summary>
	/// <remarks>
	/// This is used to locate embedded resources within the assembly.
	/// </remarks>
	private const string resourceRootNamespace = "Planetoid_DB";

	/// <summary>
	/// The name of the license resource.
	/// </summary>
	/// <remarks>
	/// This is used to locate the license resource within the assembly.
	/// </remarks>
	private const string licenseResourceName = "LICENSE";

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="LicenseForm"/> class.
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
	/// Asynchronously extracts an embedded resource to a file.
	/// </summary>
	/// <param name="nameSpace">The root namespace.</param>
	/// <param name="destinationPath">The full output path.</param>
	/// <param name="resourceName">The resource name.</param>
	/// <param name="token">Cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// This method is used to extract an embedded resource asynchronously.
	/// </remarks>
	private static async Task ExtractResourceAsync(string nameSpace, string destinationPath, string resourceName, CancellationToken token = default)
	{
		// Get the executing assembly
		Assembly assembly = Assembly.GetExecutingAssembly();
		// Get the resource path
		string resourcePath = $"{nameSpace}.{resourceName}";
		// Try to get the resource stream
		using Stream? resourceStream = assembly.GetManifestResourceStream(name: resourcePath) ?? throw new FileNotFoundException(message: $"Embedded resource '{resourcePath}' not found.");
		// Create the file stream
		// 'useAsync: true' enables internal buffer optimizations for asynchronous writing.
		using FileStream fileStream = new(
			path: destinationPath,
			mode: FileMode.Create,
			access: FileAccess.Write,
			share: FileShare.None,
			bufferSize: 4096,
			useAsync: true);
		// Copy the resource stream to the file stream
		await resourceStream.CopyToAsync(destination: fileStream, cancellationToken: token);
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
	private void LicenseForm_Load(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

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
	private async void KryptonButtonSaveLicense_ClickAsync(object sender, EventArgs e)
	{
		// Create a SaveFileDialog to prompt the user for a file location
		if (saveFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Extract the LICENSE file from the embedded resources and copy it to the selected file location
		try
		{
			await ExtractResourceAsync(nameSpace: resourceRootNamespace, destinationPath: saveFileDialog.FileName, resourceName: licenseResourceName);
			MessageBox.Show(text: "License saved successfully.", caption: "Success", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
		}
		catch (Exception ex)
		{
			MessageBox.Show(text: $"Error saving license: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
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