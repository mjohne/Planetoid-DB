using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Main form for managing records in the Planetoid database.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class RecordsMainForm : BaseKryptonForm
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="RecordsMainForm"/> class.
		/// </summary>
		public RecordsMainForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region Local methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
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

		#endregion

		#region Form event handler

		/// <summary>
		/// Handles the Load event of the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void RecordsMainForm_Load(object sender, EventArgs e)
		{
			ClearStatusBar();
		}

		/// <summary>
		/// Handles the FormClosed event of the RecordsMainForm.
		/// Disposes the form when it is closed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void RecordsMainForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handler

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

		#region Leave event handler

		/// <summary>
		/// Called when the mouse pointer leaves a control or the control loses focus.
		/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click event handler

		/// <summary>
		/// Handles the Click event of the ButtonStart control.
		/// Starts the main process.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonStart_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsTxt control.
		/// Exports data as a TXT file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsTxt_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsHtml control.
		/// Exports data as an HTML file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsHtml_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsXml control.
		/// Exports data as an XML file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsXml_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsJson control.
		/// Exports data as a JSON file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsJson_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto01 control.
		/// Navigates to the first record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto01_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto02 control.
		/// Navigates to the second record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto02_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto03 control.
		/// Navigates to the third record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto03_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto04 control.
		/// Navigates to the fourth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto04_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto05 control.
		/// Navigates to the fifth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto05_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto06 control.
		/// Navigates to the sixth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto06_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto07 control.
		/// Navigates to the seventh record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto07_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto08 control.
		/// Navigates to the eighth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto08_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto09 control.
		/// Navigates to the ninth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto09_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the Click event of the ButtonGoto10 control.
		/// Navigates to the tenth record.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonGoto10_Click(object sender, EventArgs e)
		{
		}

		#endregion

		#region DoubleClick event handler

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
