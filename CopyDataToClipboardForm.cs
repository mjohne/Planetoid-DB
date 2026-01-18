using Krypton.Toolkit;

using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// A form that allows users to copy data to the clipboard.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class CopyDataToClipboardForm : BaseKryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// The list of data to be copied to the clipboard.
		private List<string> dataToCopy = [];

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CopyDataToClipboardForm"/> class.
		/// </summary>
		public CopyDataToClipboardForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region local methods

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
		/// Sets the data to be copied to the clipboard.
		/// </summary>
		/// <param name="list">The list of data to be copied.</param>
		public void SetDatabase(List<string> list) => dataToCopy = list;

		#endregion

		#region Form event handler

		/// <summary>
		/// Handles the Load event of the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CopyDataToClipboardForm_Load(object sender, EventArgs e)
		{
			// Set the status bar text
			ClearStatusBar();
			// Set the text of the buttons with the data to be copied
			KryptonButton[] buttons =
			[
				buttonIndexNumber, buttonReadableDesignation, buttonEpoch, buttonMeanAnomaly, buttonArgumentOfPerihelion,
				buttonLongitudeOfTheAscendingNode, buttonInclination, buttonOrbitalEccentricity, buttonMeanDailyMotion,
				buttonSemimajorAxis, buttonAbsoluteMagnitude, buttonSlopeParameter, buttonReference, buttonNumberOfOppositions,
				buttonNumberOfObservations, buttonObservationSpan, buttonRmsResidual, buttonComputername, buttonFlags, buttonDateOfLastObservation
			];
			// Set the tag of each button to the corresponding data from the list
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].Tag = dataToCopy.Count > i ? dataToCopy[index: i] : string.Empty;
			}
		}

		/// <summary>
		/// Handles the FormClosed event of the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void CopyDataToClipboardForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter-Handler

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

		#region Leave-Handler

		/// <summary>
		/// Called when the mouse pointer leaves a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click-Handler

		/// <summary>
		/// Handles the Click event of the buttonIndexNumber control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonIndexNumber_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonIndexNumber.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonReadableDesignation control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonReadableDesignation_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonReadableDesignation.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonEpoch control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonEpoch_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonEpoch.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonMeanAnomaly control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonMeanAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMeanAnomaly.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonArgumentOfPerihelion control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonArgumentOfPerihelion_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonArgumentOfPerihelion.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonLongitudeOfTheAscendingNode control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonLongitudeOfTheAscendingNode.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonInclination control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonInclination_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonInclination.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonOrbitalEccentricity control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonOrbitalEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonOrbitalEccentricity.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonMeanDailyMotion control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonMeanDailyMotion_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMeanDailyMotion.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonSemiMajorAxis control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonSemiMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSemimajorAxis.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonAbsoluteMagnitude control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonAbsoluteMagnitude_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonAbsoluteMagnitude.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonSlopeParameter control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonSlopeParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSlopeParameter.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonReference control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonReference_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonReference.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonNumberOfOppositions control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonNumberOfOppositions_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonNumberOfOppositions.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonNumberOfObservations control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonNumberOfObservations_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonNumberOfObservations.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonObservationSpan control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonObservationSpan_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonObservationSpan.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonRmsResidual control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonRmsResidual_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonRmsResidual.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonComputerName control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonComputerName_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonComputername.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonFlags control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonFlags_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonFlags.Tag?.ToString() ?? string.Empty);

		/// <summary>
		/// Handles the Click event of the buttonDateOfLastObservation control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonDateOfLastObservation_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonDateOfLastObservation.Tag?.ToString() ?? string.Empty);

		#endregion
	}
}