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
public partial class CopyDerivedDataToClipboardForm : BaseKryptonForm
{
	/// <summary>
	/// The list of data to be copied to the clipboard.
	/// </summary>
	private List<string> dataToCopy = [];

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="CopyDerivedDataToClipboardForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public CopyDerivedDataToClipboardForm() =>
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
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method updates the status bar with the provided text and additional information.
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
	/// Fired when the CopyDerivedDataToClipboardForm loads.
	/// Clears the status area and initializes button tags with the values provided via <see cref="SetDatabase(List{string})"/>.
	/// Each button's <c>tag</c> will contain the corresponding data item or an empty string if the item is missing.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to initialize the form's UI elements with information from the database.
	/// </remarks>
	private void CopyDerivedDataToClipboardForm_Load(object sender, EventArgs e)
	{
		// Set the status bar text
		ClearStatusBar();
		// Set the text of the buttons with the data to be copied
		KryptonButton[] buttons =
		[
			buttonLinearEccentricity, buttonSemiMinorAxis, buttonMajorAxis, buttonMinorAxis, buttonEccentricAnomaly,
			buttonTrueAnomaly, buttonPerihelionDistance, buttonAphelionDistance, buttonLongitudeDescendingNode,
			buttonArgumentAphelion, buttonFocalParameter, buttonSemiLatusRectum, buttonLatusRectum, buttonOrbitalPeriod,
			buttonOrbitalArea, buttonOrbitalPerimeter, buttonSemiMeanAxis, buttonMeanAxis, buttonStandardGravitationalParameter
		];
		// Set the tag of each button to the corresponding data from the list
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Tag = dataToCopy.Count > i ? dataToCopy[index: i] : string.Empty;
		}
	}

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This event is used to update the status bar when the mouse enters a control.
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
	/// This event is used to clear the status bar when the mouse leaves a control.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the buttonLinearEccentricity control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the linear eccentricity to the clipboard.
	/// </remarks>
	private void ButtonLinearEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonLinearEccentricity.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonSemiMinorAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the semi-minor axis to the clipboard.
	/// </remarks>
	private void ButtonSemiMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSemiMinorAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonMajorAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the major axis to the clipboard.
	/// </remarks>
	private void ButtonMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMajorAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonMinorAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the minor axis to the clipboard.
	/// </remarks>
	private void ButtonMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMinorAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonEccentricAnomaly control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the eccentric anomaly to the clipboard.
	/// </remarks>
	private void ButtonEccentricAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonEccentricAnomaly.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonTrueAnomaly control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the true anomaly to the clipboard.
	/// </remarks>
	private void ButtonTrueAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonTrueAnomaly.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonPerihelionDistance control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the perihelion distance to the clipboard.
	/// </remarks>
	private void ButtonPerihelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonPerihelionDistance.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonAphelionDistance control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the aphelion distance to the clipboard.
	/// </remarks>
	private void ButtonAphelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonAphelionDistance.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonLongitudeDescendingNode control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the longitude of the descending node to the clipboard.
	/// </remarks>
	private void ButtonLongitudeDescendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonLongitudeDescendingNode.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonArgumentAphelion control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the argument of perihelion to the clipboard.
	/// </remarks>
	private void ButtonArgumentAphelion_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonArgumentAphelion.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonFocalParameter control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the focal parameter to the clipboard.
	/// </remarks>
	private void ButtonFocalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonFocalParameter.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonSemiLatusRectum control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the semi-latus rectum to the clipboard.
	/// </remarks>
	private void ButtonSemiLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSemiLatusRectum.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonLatusRectum control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the latus rectum to the clipboard.
	/// </remarks>
	private void ButtonLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonLatusRectum.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonOrbitalPeriod control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the orbital period to the clipboard.
	/// </remarks>
	private void ButtonOrbitalPeriod_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonOrbitalPeriod.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonOrbitalArea control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the orbital area to the clipboard.
	/// </remarks>
	private void ButtonOrbitalArea_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonOrbitalArea.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonOrbitalPerimeter control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the orbital perimeter to the clipboard.
	/// </remarks>
	private void ButtonOrbitalPerimeter_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonOrbitalPerimeter.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonSemiMeanAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the semi-mean axis to the clipboard.
	/// </remarks>
	private void ButtonSemiMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonSemiMeanAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonMeanAxis control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the mean axis to the clipboard.
	/// </remarks>
	private void ButtonMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonMeanAxis.Tag?.ToString() ?? string.Empty);

	/// <summary>
	/// Handles the Click event of the buttonStandardGravitationalParameter control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is used to copy the standard gravitational parameter to the clipboard.
	/// </remarks>
	private void ButtonStandardGravitationalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: buttonStandardGravitationalParameter.Tag?.ToString() ?? string.Empty);

	#endregion
}