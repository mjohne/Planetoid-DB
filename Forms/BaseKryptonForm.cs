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
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This control is used for clipboard operations such as copy and paste.
	/// </remarks>
	protected Control? currentControl;

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
		_ = MessageBox.Show(text: message, caption: I18nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

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
			_ = MessageBox.Show(text: I18nStrings.CopiedToClipboard, caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
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

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// Derived classes should override this property to provide the specific label.
	/// </summary>
	protected virtual ToolStripStatusLabel? StatusLabel => null;

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer enters a control or the control receives focus.
	/// </remarks>
	protected void Control_Enter(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Check if the status label is null
		if (StatusLabel is null)
		{
			return;
		}
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
			SetStatusBar(label: StatusLabel, text: description);
		}
	}

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	protected void Control_Leave(object sender, EventArgs e)
	{
		// Check if the status label is not null
		if (StatusLabel != null)
		{
			// Clear the status bar text
			ClearStatusBar(label: StatusLabel);
		}
	}

	/// <summary>
	/// Handles double-click events on controls and copies their text to the clipboard.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method extracts text from the sender control or uses the current control's text for ToolStripItems,
	/// then copies it to the clipboard using the <see cref="CopyToClipboard"/> method.
	/// </remarks>
	protected void CopyToClipboard_DoubleClick(object sender, EventArgs e)
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
			// Copy the text to the clipboard using the base method
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


}