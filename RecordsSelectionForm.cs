using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents the form for selecting records.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class RecordsSelectionForm : BaseKryptonForm
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog logger instance

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="RecordsSelectionForm"/> class.
		/// </summary>
		public RecordsSelectionForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region Local Methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		private static void ShowErrorMessage(string message) =>
			// Show an error message box with the specified message
			_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

		/// <summary>
		/// Copies the specified text to the clipboard and displays a confirmation message.
		/// </summary>
		/// <param name="text">The text to be copied.</param>
		private static void CopyToClipboard(string text)
		{
			try
			{
				// Copy the text to the clipboard
				Clipboard.SetText(text: text);
				_ = MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				// Log the exception and show an error message
				Logger.Error(exception: ex, message: ex.Message);
				// Show an error message
				ShowErrorMessage(message: $"File not found: {ex.Message}");
			}
		}

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
		/// Shows the main records form.
		/// </summary>
		private static void ShowRecordsMain()
		{
			// Create and show the main records form
			using RecordsMainForm formRecordsMain = new();
			// Show the form as a dialog and wait for it to close
			_ = formRecordsMain.ShowDialog();
		}

		#endregion

		#region Form event handler

		/// <summary>
		/// Handles the Load event of the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void RecordsSelectionForm_Load(object sender, EventArgs e)
		{
			ClearStatusBar();
		}

		/// <summary>
		/// Handles the FormClosed event of the RecordsSelectionForm.
		/// Disposes the form when it is closed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void RecordsSelectionForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handler

		/// <summary>
		/// Called when the mouse pointer moves over a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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

		#region Click & ButtonClick event handlers

		/// <summary>
		/// Handles the Click event of the ButtonMeanAnomaly control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonMeanAnomaly_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonArgumentOfPerihelion control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonArgumentOfPerihelion_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonLongitudeOfTheAscendingNode control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonInclination control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonInclination_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonOrbitalEccentricity control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonOrbitalEccentricity_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonMeanDailyMotion control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonMeanDailyMotion_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonSemiMajorAxis control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonSemiMajorAxis_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonAbsoluteMagnitude control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonAbsoluteMagnitude_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonSlopeParameter control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonSlopeParameter_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonNumberOfOppositions control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonNumberOfOppositions_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonNumberOfObservations control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonNumberOfObservations_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonObservationSpan control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonObservationSpan_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonRmsResidual control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonRmsResidual_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonComputerName control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonComputerName_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the ButtonDateOfLastObservation control.
		/// Shows the main records form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonDateOfLastObservation_Click(object sender, EventArgs e) => ShowRecordsMain();

		/// <summary>
		/// Handles the Click event of the CheckButtonRecordSortDirectionAscending control.
		/// Toggles the checked state of the descending sort direction button.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CheckButtonRecordSortDirectionAscending_Click(object sender, EventArgs e) => checkButtonRecordSortDirectionDescending.Checked = !checkButtonRecordSortDirectionAscending.Checked;

		/// <summary>
		/// Handles the Click event of the CheckButtonRecordSortDirectionDescending control.
		/// Toggles the checked state of the ascending sort direction button.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CheckButtonRecordSortDirectionDescending_Click(object sender, EventArgs e) => checkButtonRecordSortDirectionAscending.Checked = !checkButtonRecordSortDirectionDescending.Checked;

		#endregion
	}
}