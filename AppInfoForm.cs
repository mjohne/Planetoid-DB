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
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the control that is currently selected
	/// for clipboard operations, such as copying text.
	/// </remarks>
	private Control? currentControl;

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
		labelVersion.Text = string.Format(format: I10nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
		labelDescription.Text = AssemblyInfo.AssemblyDescription;
		labelCopyright.Text = AssemblyInfo.AssemblyCopyright;
		ClearStatusBar(label: labelInformation);
	}

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer enters a control or the control receives focus.
	/// </remarks>
	private void Control_Enter(object sender, EventArgs e)
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
		// If a description is available, set it in the status bar
		if (description != null)
		{
			SetStatusBar(label: labelInformation, text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to store the control that triggered the event for future reference.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
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
			Logger.Error(exception: ex, message: ex.Message);
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
			Logger.Error(exception: ex, message: ex.Message);
			// Show an error message if the email client cannot be opened
			ShowErrorMessage(message: $"Error opening the website: {ex.Message}");
		}
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/> or
	/// a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the text of a control or ToolStrip item to the clipboard.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the text to copy based on the sender type
		string? textToCopy = sender switch
		{
			Control c => c.Text,
			ToolStripItem => currentControl?.Text,
			_ => null
		};
		// Check if the text to copy is not null or empty
		if (!string.IsNullOrEmpty(value: textToCopy))
		{
			// Assuming CopyToClipboard is a helper method in BaseKryptonForm or similar
			// If not, use Clipboard.SetText(textToCopy);
			try
			{
				CopyToClipboard(text: textToCopy);
			}
			// Log any exception that occurs during the clipboard operation
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "Failed to copy text to the clipboard.");
				throw new InvalidOperationException(message: "Failed to copy text to the clipboard.", innerException: ex);
			}
		}
	}

	#endregion

}