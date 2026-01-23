using Krypton.Toolkit;

using NLog;

using System.Diagnostics;

namespace Planetoid_DB.Forms
{
	/// <summary>
	/// Base form providing common behaviours for application forms.
	/// Currently: enables <c>KeyPreview</c> and closes the form when the Escape key is pressed.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public class BaseKryptonForm : KryptonForm
	{
		/// <summary>
		/// NLog logger instance for the class.
		/// </summary>
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseKryptonForm"/> class.
		/// </summary>
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
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Default KeyDown handler that closes the form when Escape is pressed.
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Key event args.</param>
		private void BaseKryptonForm_KeyDown(object? sender, KeyEventArgs e)
		{
			// Close the form when Escape is pressed
			if (e.KeyCode == Keys.Escape && this.InvokeRequired == false)
			{
				Close();
			}
		}

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
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
				Logger.Error(exception: ex, message: ex.Message);
				// Show an error message
				ShowErrorMessage(message: $"Error copying to clipboard: {ex.Message}");
			}
		}
	}
}