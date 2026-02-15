using Krypton.Toolkit;

using NLog;

using System.Diagnostics;

namespace Planetoid_DB.Forms;

/// <summary>
/// Base form providing common behaviours for application forms.
/// Currently: enables <c>KeyPreview</c> and closes the form when the Escape key is pressed.
/// </summary>
/// <remarks>
/// This class serves as a base form for the application, providing common functionality
/// and behaviors that can be shared across derived forms.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class BaseKryptonForm : KryptonForm
{
	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors for the class.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseKryptonForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor sets up the form to receive key events and handle them appropriately.
	/// </remarks>
	protected BaseKryptonForm()
	{
		// Ensure the form receives key events before child controls
		KeyPreview = true;
		KeyDown += BaseKryptonForm_KeyDown;
	}

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation for the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Default KeyDown handler that closes the form when Escape is pressed.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Key event args.</param>
	/// <remarks>
	/// This method is used to handle key down events for the form.
	/// </remarks>
	private void BaseKryptonForm_KeyDown(object? sender, KeyEventArgs e)
	{
		// Close the form when Escape is pressed
		if (e.KeyCode == Keys.Escape && !this.InvokeRequired)
		{
			Close();
		}
	}

	/// <summary>
	/// Displays an error message.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <remarks>
	/// This method is used to display an error message to the user.
	/// </remarks>
	protected static void ShowErrorMessage(string message) =>
		// Show an error message box with the specified message
		_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

	/// <summary>
	/// Copies the specified text to the clipboard and shows a confirmation dialog.
	/// </summary>
	/// <param name="text">The text to copy to the clipboard.</param>
	/// <remarks>
	/// On success an informational dialog is shown. On failure the exception is logged
	/// and an error dialog is displayed. This method is protected so derived forms can use it directly.
	/// </remarks>
	protected static void CopyToClipboard(string text)
	{
		// Try to copy the text to the clipboard
		try
		{
			// Copy the text to the clipboard
			Clipboard.SetText(text: text);
			_ = MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
		}
		// Handle any exceptions that occur during the clipboard operation
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: ex.Message);
			// Show an error message
			ShowErrorMessage(message: $"Error copying to clipboard: {ex.Message}");
		}
	}

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="label">The status label to update.</param>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to set the status bar text and enable the information label.
	/// </remarks>
	protected static void SetStatusBar(ToolStripStatusLabel label, string text, string additionalInfo = "")
	{
		// Check if the label is null or text is null or whitespace
		if (label is null || string.IsNullOrWhiteSpace(value: text))
		{
			return;
		}
		// Set the status bar text and enable it
		label.Enabled = true;
		label.Text = string.IsNullOrWhiteSpace(value: additionalInfo) ? text : $"{text} - {additionalInfo}";
	}

	/// <summary>
	/// Clears the status bar text and disables the information label.
	/// </summary>
	/// <param name="label">The status label to clear.</param>
	/// <remarks>
	/// Resets the UI state of the status area so that no message is shown.
	/// Use when there is no status to display or when leaving a control.
	/// </remarks>
	protected static void ClearStatusBar(ToolStripStatusLabel label)
	{
		// Check if the label is null
		if (label is null)
		{
			return;
		}
		// Clear the status bar text and disable it
		label.Enabled = false;
		label.Text = string.Empty;
	}
}