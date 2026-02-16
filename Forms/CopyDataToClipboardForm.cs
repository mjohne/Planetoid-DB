// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Forms;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// A form that allows users to copy data to the clipboard.
/// </summary>
/// <remarks>
/// This form provides a user interface for selecting and copying data to the clipboard.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class CopyDataToClipboardForm : BaseKryptonForm
{
	/// <summary>
	/// The list of data to be copied to the clipboard.
	/// </summary>
	private List<string> dataToCopy = [];

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CopyDataToClipboardForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public CopyDataToClipboardForm() =>
		// Initialize the form components
		InitializeComponent();

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

	/// <summary>
	/// Sets the data to be copied to the clipboard.
	/// </summary>
	/// <param name="list">The list of data to be copied.</param>
	/// <remarks>
	/// This method sets the data to be copied to the clipboard.
	/// </remarks>
	public void SetDatabase(List<string> list) => dataToCopy = list;

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the CopyDataToClipboardForm loads.
	/// Clears the status area and initializes button tags with the values provided via <see cref="SetDatabase(List{string})"/>.
	/// Each button's <c>tag</c> will contain the corresponding data item or an empty string if the item is missing.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to initialize the form's UI elements with information from the database.
	/// </remarks>
	private void CopyDataToClipboardForm_Load(object sender, EventArgs e)
	{
		// Set the status bar text
		ClearStatusBar(label: labelInformation);
		// Set the text of the buttons with the data to be copied
		KryptonButton[] buttons =
		[
			buttonIndexNumber, buttonReadableDesignation, buttonEpoch, buttonMeanAnomaly, buttonArgumentOfPerihelion,
			buttonLongitudeOfTheAscendingNode, buttonInclination, buttonOrbitalEccentricity, buttonMeanDailyMotion,
			buttonSemimajorAxis, buttonAbsoluteMagnitude, buttonSlopeParameter, buttonReference, buttonNumberOfOppositions,
			buttonNumberOfObservations, buttonObservationSpan, buttonRmsResidual, buttonComputerName, buttonFlags, buttonDateOfLastObservation
		];
		// Set the tag of each button to the corresponding data from the list
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Tag = dataToCopy.Count > i ? dataToCopy[index: i] : string.Empty;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the buttonIndexNumber control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the index number to the clipboard.
	/// </remarks>
	private void ButtonIndexNumber_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonIndexNumber.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonReadableDesignation control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the readable designation to the clipboard.
	/// </remarks>
	private void ButtonReadableDesignation_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonReadableDesignation.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonEpoch control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the epoch to the clipboard.
	/// </remarks>
	private void ButtonEpoch_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonEpoch.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonMeanAnomaly control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the mean anomaly to the clipboard.
	/// </remarks>
	private void ButtonMeanAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMeanAnomaly.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonArgumentOfPerihelion control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the argument of perihelion to the clipboard.
	/// </remarks>
	private void ButtonArgumentOfPerihelion_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonArgumentOfPerihelion.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonLongitudeOfTheAscendingNode control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the longitude of the ascending node to the clipboard.
	/// </remarks>
	private void ButtonLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonLongitudeOfTheAscendingNode.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonInclination control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the inclination to the clipboard.
	/// </remarks>
	private void ButtonInclination_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonInclination.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonOrbitalEccentricity control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the orbital eccentricity to the clipboard.
	/// </remarks>
	private void ButtonOrbitalEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonOrbitalEccentricity.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonMeanDailyMotion control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the mean daily motion to the clipboard.
	/// </remarks>
	private void ButtonMeanDailyMotion_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMeanDailyMotion.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonSemiMajorAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the semi-major axis to the clipboard.
	/// </remarks>
	private void ButtonSemiMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSemimajorAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonAbsoluteMagnitude control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the absolute magnitude to the clipboard.
	/// </remarks>
	private void ButtonAbsoluteMagnitude_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonAbsoluteMagnitude.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonSlopeParameter control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the slope parameter to the clipboard.
	/// </remarks>
	private void ButtonSlopeParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSlopeParameter.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonReference control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the reference to the clipboard.
	/// </remarks>
	private void ButtonReference_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonReference.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonNumberOfOppositions control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the number of oppositions to the clipboard.
	/// </remarks>
	private void ButtonNumberOfOppositions_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonNumberOfOppositions.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonNumberOfObservations control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the number of observations to the clipboard.
	/// </remarks>
	private void ButtonNumberOfObservations_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonNumberOfObservations.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonObservationSpan control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the observation span to the clipboard.
	/// </remarks>
	private void ButtonObservationSpan_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonObservationSpan.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonRmsResidual control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the RMS residual to the clipboard.
	/// </remarks>
	private void ButtonRmsResidual_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonRmsResidual.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonComputerName control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the computer name to the clipboard.
	/// </remarks>
	private void ButtonComputerName_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonComputerName.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonFlags control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the flags to the clipboard.
	/// </remarks>
	private void ButtonFlags_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonFlags.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonDateOfLastObservation control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the date of the last observation to the clipboard.
	/// </remarks>
	private void ButtonDateOfLastObservation_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonDateOfLastObservation.Tag?.ToString() ?? string.Empty);

	#endregion
}