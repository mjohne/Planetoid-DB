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
/// Represents the splash screen form of the application.
/// </summary>
/// <remarks>
/// This form is displayed while the application is loading.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class SplashScreenForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used throughout the application to log important events and errors.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the control that is currently selected for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="SplashScreenForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public SplashScreenForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is called to obtain a string representation of the current instance.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the splash screen progress bar value.
	/// </summary>
	/// <param name="value">The value to set on the progress bar. Must be between <c>progressBarSplash.Minimum</c> and <c>progressBarSplash.Maximum</c>.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the valid range of the progress bar.</exception>
	/// <remarks>
	/// This method is called to set the value of the splash screen progress bar.
	/// </remarks>
	public void SetProgressbar(int value)
	{
		// Validate the value
		// Check if the value is within the range of the progress bar
		// If the value is less than the minimum or greater than the maximum, throw an exception
		if (value < progressBarSplash.Minimum || value > progressBarSplash.Maximum)
		{
			// Log the error and throw an exception
			logger.Error(message: $"Value {value} is out of range for the progress bar. Minimum: {progressBarSplash.Minimum}, Maximum: {progressBarSplash.Maximum}");
			// Throw an exception indicating that the value is out of range
			throw new ArgumentOutOfRangeException(paramName: nameof(value), message: I18nStrings.IndexOutOfRange);
		}
		// Set the value of the progress bar
		progressBarSplash.Value = value;
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the splash screen form loads.
	/// Sets the product title and the formatted version text on the form's labels.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the splash screen form loads.
	/// </remarks>
	private void SplashScreenForm_Load(object sender, EventArgs e)
	{
		// Set the title label text to the product name
		labelTitle.Text = AssemblyInfo.AssemblyProduct;
		// Set the version label text to the assembly version
		labelVersion.Text = string.Format(format: I18nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
	}
	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the mouse button is pressed over a control.
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

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/> or
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when a control is double-clicked.
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
				logger.Error(exception: ex, message: "Failed to copy text to the clipboard.");
				throw new InvalidOperationException(message: "Failed to copy text to the clipboard.", innerException: ex);
			}
		}
	}

	#endregion
}