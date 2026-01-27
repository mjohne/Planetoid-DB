using Planetoid_DB.Forms;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Represents the form for selecting records.
/// </summary>
/// <remarks>
/// This form provides a user interface for selecting records from the database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class RecordsSelectionForm : BaseKryptonForm
{
	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="RecordsSelectionForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public RecordsSelectionForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is called when the debugger displays the object.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is called to update the status bar with the provided text.
	/// </remarks>
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

	/// <summary>
	/// Shows the main records form.
	/// </summary>
	/// <remarks>
	/// This method is called to show the main records form.
	/// </remarks>
	private static void ShowRecordsMain()
	{
		// Create and show the main records form
		using RecordsMainForm formRecordsMain = new();
		// Show the form as a dialog and wait for it to close
		_ = formRecordsMain.ShowDialog();
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the RecordsSelectionForm is loaded.
	/// </remarks>
	private void RecordsSelectionForm_Load(object sender, EventArgs e) => ClearStatusBar();

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
	private void SetStatusBar_Enter(object sender, EventArgs e)
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
			SetStatusBar(text: description);
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
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region Click & ButtonClick event handlers

	/// <summary>
	/// Handles the Click event of the ButtonMeanAnomaly control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonMeanAnomaly control is clicked.
	/// </remarks>
	private void ButtonMeanAnomaly_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonArgumentOfPerihelion control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonArgumentOfPerihelion control is clicked.
	/// </remarks>
	private void ButtonArgumentOfPerihelion_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonLongitudeOfTheAscendingNode control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonLongitudeOfTheAscendingNode control is clicked.
	/// </remarks>
	private void ButtonLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonInclination control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonInclination control is clicked.
	/// </remarks>
	private void ButtonInclination_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonOrbitalEccentricity control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonOrbitalEccentricity control is clicked.
	/// </remarks>
	private void ButtonOrbitalEccentricity_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonMeanDailyMotion control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonMeanDailyMotion control is clicked.
	/// </remarks>
	private void ButtonMeanDailyMotion_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonSemiMajorAxis control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonSemiMajorAxis control is clicked.
	/// </remarks>
	private void ButtonSemiMajorAxis_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonAbsoluteMagnitude control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonAbsoluteMagnitude control is clicked.
	/// </remarks>
	private void ButtonAbsoluteMagnitude_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonSlopeParameter control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonSlopeParameter control is clicked.
	/// </remarks>
	private void ButtonSlopeParameter_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonNumberOfOppositions control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonNumberOfOppositions control is clicked.
	/// </remarks>
	private void ButtonNumberOfOppositions_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonNumberOfObservations control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonNumberOfObservations control is clicked.
	/// </remarks>
	private void ButtonNumberOfObservations_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonObservationSpan control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonObservationSpan control is clicked.
	/// </remarks>
	private void ButtonObservationSpan_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonRmsResidual control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonRmsResidual control is clicked.
	/// </remarks>
	private void ButtonRmsResidual_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonComputerName control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonComputerName control is clicked.
	/// </remarks>
	private void ButtonComputerName_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the ButtonDateOfLastObservation control.
	/// Shows the main records form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the ButtonDateOfLastObservation control is clicked.
	/// </remarks>
	private void ButtonDateOfLastObservation_Click(object sender, EventArgs e) => ShowRecordsMain();

	/// <summary>
	/// Handles the Click event of the CheckButtonRecordSortDirectionAscending control.
	/// Toggles the checked state of the descending sort direction button.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the CheckButtonRecordSortDirectionAscending control is clicked.
	/// </remarks>
	private void CheckButtonRecordSortDirectionAscending_Click(object sender, EventArgs e) => checkButtonRecordSortDirectionDescending.Checked = !checkButtonRecordSortDirectionAscending.Checked;

	/// <summary>
	/// Handles the Click event of the CheckButtonRecordSortDirectionDescending control.
	/// Toggles the checked state of the ascending sort direction button.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the CheckButtonRecordSortDirectionDescending control is clicked.
	/// </remarks>
	private void CheckButtonRecordSortDirectionDescending_Click(object sender, EventArgs e) => checkButtonRecordSortDirectionAscending.Checked = !checkButtonRecordSortDirectionDescending.Checked;

	#endregion
}