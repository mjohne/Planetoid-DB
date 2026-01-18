using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// A form that displays application information.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class AppInfoForm : BaseKryptonForm
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog logger instance

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
		/// </summary>
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
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the status bar text and enables the information label when text is provided.
		/// </summary>
		/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
		/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
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

		#endregion

		#region form event handlers

		/// <summary>
		/// Fired when the form loads.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void AppInfoForm_Load(object sender, EventArgs e)
		{
			labelTitle.Text = AssemblyInfo.AssemblyProduct;
			labelVersion.Text = string.Format(format: I10nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
			labelDescription.Text = AssemblyInfo.AssemblyDescription;
			labelCopyright.Text = AssemblyInfo.AssemblyCopyright;
			ClearStatusBar();
		}

		/// <summary>
		/// Fired when the form closes.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void AppInfoForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handlers

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

		#region Leave event handlers

		/// <summary>
		/// Called when the mouse pointer leaves a control or the control loses focus.
		/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click event handlers

		/// <summary>
		/// Called when the link to open the website is clicked.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
		/// Called when the link to send an email is clicked.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
		/// Called when a control is double-clicked to copy its text to the clipboard.
		/// If the sender is a <see cref="Control"/>, the control's <see cref="Control.Text"/> is copied.
		/// </summary>
		/// <param name="sender">Event source (expected to be a <see cref="Control"/>).</param>
		/// <param name="e">Event arguments.</param>
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