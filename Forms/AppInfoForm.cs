// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// A form that displays application information.
/// </summary>
/// <remarks>
/// This form is used to present information about the application, such as its version,
/// description, and copyright details.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AppInfoForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors for the class.
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
	/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public AppInfoForm()
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
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the application info form loads.
	/// Populates UI labels with product, version and description information from the assembly
	/// and clears the status area.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to initialize the form's UI elements with information from the assembly.
	/// </remarks>
	private void AppInfoForm_Load(object sender, EventArgs e)
	{
		labelTitle.Text = AssemblyInfo.AssemblyProduct;
		labelVersion.Text = string.Format(format: I18nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
		labelDescription.Text = AssemblyInfo.AssemblyDescription;
		labelCopyright.Text = AssemblyInfo.AssemblyCopyright;
		ClearStatusBar(label: labelInformation);
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Called when the website link is clicked.
	/// Attempts to open the application's website in the user's default browser and logs any error.
	/// </summary>
	/// <param name="sender">Event source (the link label).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to open the application's website in the user's default browser.
	/// </remarks>
	private async void LinkLabelWebsite_Clicked(object sender, EventArgs e)
	{
		try
		{
			// Open the website in the default browser
			using Process process = new();
			process.StartInfo = new ProcessStartInfo(fileName: "https://planetoid-db.de") { UseShellExecute = true };
			// Start the process asynchronously
			_ = await Task.Run(function: process.Start);
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: ex.Message);
			// Show an error message if the website cannot be opened
			ShowErrorMessage(message: $"Error opening the website: {ex.Message}");
		}
	}

	/// <summary>
	/// Called when the email link is clicked.
	/// Attempts to open the user's default mail client with a new message addressed to the application's support email.
	/// Any error during the operation is logged and shown to the user.
	/// </summary>
	/// <param name="sender">Event source (the link label).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to open the user's default mail client with a new message addressed to the application's support email.
	/// </remarks>
	private async void LinkLabelEmail_Clicked(object sender, EventArgs e)
	{
		try
		{
			// Open the default email client with a new message to the specified email address
			using Process process = new();
			process.StartInfo = new ProcessStartInfo(fileName: "mailto:info@planetoid-db.de") { UseShellExecute = true };
			// Start the process asynchronously
			_ = await Task.Run(function: process.Start);
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: ex.Message);
			// Show an error message if the email client cannot be opened
			ShowErrorMessage(message: $"Error opening the website: {ex.Message}");
		}
	}

	#endregion
}