using NLog;

using Planetoid_DB.Forms;

using System.Collections;
using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Form for displaying derived orbit elements.
/// </summary>
/// <remarks>
/// This form provides a user interface for displaying derived orbit elements.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DerivedOrbitElementsForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This field is used to log messages for the form.
	/// </remarks>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Stores the planetoids database.
	/// </summary>
	/// <remarks>
	/// This ArrayList is used to store the planetoids database entries.
	/// </remarks>
	private readonly ArrayList planetoidsDatabase = [];

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the control that is currently selected for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	/// <summary>
	/// Stores the current tag text of the control.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the current tag text of the control.
	/// </remarks>
	private readonly string currentTagText = string.Empty;

	/// <summary>
	/// List of derived orbit elements.
	/// </summary>
	/// <remarks>
	/// This field is used to store the list of derived orbit elements.
	/// </remarks>
	private List<object> derivedOrbitElements = [];

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="DerivedOrbitElementsForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public DerivedOrbitElementsForm() =>
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
	/// This method is used to update the status bar with the provided text.
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
	/// Tries to parse an integer from the input string.
	/// </summary>
	/// <param name="input">The input string to parse.</param>
	/// <param name="value">The parsed integer value if successful.</param>
	/// <param name="errorMessage">An error message if parsing fails.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	/// <remarks>
	/// This method is used to parse an integer from the input string.
	/// </remarks>
	public static bool TryParseInt(string input, out int value, out string errorMessage)
	{
		// Initialize output parameters
		value = 0;
		errorMessage = string.Empty;
		// Check if the input is null or whitespace
		if (string.IsNullOrWhiteSpace(value: input))
		{
			// Set the error message and return false
			errorMessage = "The entered text is empty or consists only of spaces.";
			return false;
		}
		// Try to parse the integer
		// If parsing fails, set the error message
		if (!int.TryParse(s: input, result: out value))
		{
			// Set the error message and return false
			errorMessage = $"The value \"{input}\" is not a valid integer.";
			return false;
		}
		// Parsing was successful
		return true;
	}

	/// <summary>
	/// Opens the terminology dialog for a specific derived orbit element.
	/// The <paramref name="index"/> selects which terminology entry to show. Values outside the supported range
	/// are normalized to the default (index 0).
	/// </summary>
	/// <param name="index">Zero-based index selecting the terminology topic (valid range: 0..38).</param>
	/// <remarks>
	/// This method is used to open the terminology dialog for a specific derived orbit element.
	/// </remarks>
	private void OpenTerminology(uint index)
	{
		// Check if the index is valid
		// If the index is out of range, set it to 0
		if (index > 38)
		{
			index = 0;
		}
		// Create a new instance of the TerminologyForm and set the active terminology based on the index
		using TerminologyForm formTerminology = new();
		// Set the active terminology based on the index
		switch (index)
		{
			// Set the active terminology based on the index
			// Each case corresponds to a specific terminology
			// and calls the appropriate method in the TerminologyForm
			case 0: formTerminology.SetIndexNumberActive(); break;
			case 1: formTerminology.SetReadableDesignationActive(); break;
			case 2: formTerminology.SetEpochActive(); break;
			case 3: formTerminology.SetMeanAnomalyAtTheEpochActive(); break;
			case 4: formTerminology.SetArgumentOfPerihelionActive(); break;
			case 5: formTerminology.SetLongitudeOfTheAscendingNodeActive(); break;
			case 6: formTerminology.SetInclinationToTheEclipticActive(); break;
			case 7: formTerminology.SetOrbitalEccentricityActive(); break;
			case 8: formTerminology.SetMeanDailyMotionActive(); break;
			case 9: formTerminology.SetSemiMajorAxisActive(); break;
			case 10: formTerminology.SetAbsoluteMagnitudeActive(); break;
			case 11: formTerminology.SetSlopeParamActive(); break;
			case 12: formTerminology.SetReferenceActive(); break;
			case 13: formTerminology.SetNumberOfOppositionsActive(); break;
			case 14: formTerminology.SetNumberOfObservationsActive(); break;
			case 15: formTerminology.SetObservationSpanActive(); break;
			case 16: formTerminology.SetRmsResidualActive(); break;
			case 17: formTerminology.SetComputerNameActive(); break;
			case 18: formTerminology.SetFlagsActive(); break;
			case 19: formTerminology.SetDateOfTheLastObservationActive(); break;
			case 20: formTerminology.SetLinearEccentricityActive(); break;
			case 21: formTerminology.SetSemiMinorAxisActive(); break;
			case 22: formTerminology.SetMajorAxisActive(); break;
			case 23: formTerminology.SetMinorAxisActive(); break;
			case 24: formTerminology.SetEccentricAnomalyActive(); break;
			case 25: formTerminology.SetTrueAnomalyActive(); break;
			case 26: formTerminology.SetPerihelionDistanceActive(); break;
			case 27: formTerminology.SetAphelionDistanceActive(); break;
			case 28: formTerminology.SetLongitudeOfTheDescendingNodeActive(); break;
			case 29: formTerminology.SetArgumentOfTheAphelionActive(); break;
			case 30: formTerminology.SetFocalParameterActive(); break;
			case 31: formTerminology.SetSemiLatusRectumActive(); break;
			case 32: formTerminology.SetLatusRectumActive(); break;
			case 33: formTerminology.SetOrbitalPeriodActive(); break;
			case 34: formTerminology.SetOrbitalAreaActive(); break;
			case 35: formTerminology.SetOrbitalPerimeterActive(); break;
			case 36: formTerminology.SetSemiMeanAxisActive(); break;
			case 37: formTerminology.SetMeanAxisActive(); break;
			case 38: formTerminology.SetStandardGravitationalParameterActive(); break;
			// Default case to handle unexpected values
			default: formTerminology.SetIndexNumberActive(); break;
		}
		// Set the form to be topmost if the main form is topmost
		formTerminology.TopMost = TopMost;
		// Show the terminology form as a dialog
		_ = formTerminology.ShowDialog();
	}

	/// <summary>
	/// Sets the internal list of derived orbit elements used by the form.
	/// </summary>
	/// <param name="list">A list of derived orbit element values. The list is stored by reference and will be used to populate the UI when the form loads.</param>
	/// <remarks>
	/// This method is used to set the internal list of derived orbit elements.
	/// </remarks>
	public void SetDatabase(List<object> list) => derivedOrbitElements = list;

	/// <summary>
	/// Shows the form to copy data to the clipboard.
	/// </summary>
	/// <remarks>
	/// This method is used to show the form for copying data to the clipboard.
	/// </remarks>
	private void ShowCopyDataToClipboard()
	{
		// Create a new ArrayList to store the data to copy
		// The capacity is set to 0 because we will add items dynamically
		// The items in the ArrayList are the labels that contain the data to be copied
		// The labels are accessed using their respective properties

		ArrayList dataToCopy = new(capacity: 0)
		{
			labelLinearEccentricityData.Text,
			labelSemiMinorAxisData.Text,
			labelMajorAxisData.Text,
			labelMinorAxisData.Text,
			labelEccentricAnomalyData.Text,
			labelTrueAnomalyData.Text,
			labelPerihelionDistanceData.Text,
			labelAphelionDistanceData.Text,
			labelLongitudeDescendingNodeData.Text,
			labelArgumentAphelionData.Text,
			labelFocalParameterData.Text,
			labelSemiLatusRectumData.Text,
			labelLatusRectumData.Text,
			labelOrbitalPeriodData.Text,
			labelOrbitalAreaData.Text,
			labelOrbitalPerimeterData.Text,
			labelSemiMeanAxisData.Text,
			labelMeanAxisData.Text,
			labelStandardGravitationalParameterData.Text
		};
		// Create a new list to store the data to copy
		List<string> dataToCopyList = [];
		dataToCopyList.AddRange(collection: dataToCopy.OfType<object>().Select(selector: static item => item.ToString()).Where(predicate: static itemString => !string.IsNullOrEmpty(value: itemString))!);
		// Iterate through each item in the dataToCopy array
		// Create a new instance of the CopyDataToClipboardForm
		using CopyDerivedDataToClipboardForm formCopyDerivedDataToClipboard = new();
		// Set the TopMost property to true to keep the form on top of other windows
		formCopyDerivedDataToClipboard.TopMost = TopMost;
		// Fill the form with the data to copy
		formCopyDerivedDataToClipboard.SetDatabase(list: dataToCopyList);
		// Show the copy data to clipboard form as a modal dialog
		_ = formCopyDerivedDataToClipboard.ShowDialog();
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the derived orbit elements form is loaded.
	/// Clears the status area, validates the provided derived-element data and populates the UI labels
	/// with the corresponding values. If the provided data is invalid an error is logged and shown to the user.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form is loaded.
	/// </remarks>
	private void DerivedOrbitElementsForm_Load(object sender, EventArgs e)
	{
		// Set the status bar text
		ClearStatusBar();
		if (derivedOrbitElements.Count < 19)
		{
			// Log the error and show an error message
			Logger.Error(message: "Invalid data");
			ShowErrorMessage(message: "Invalid data");
			return;
		}
		// Set the text of the labels with the orbit elements
		labelLinearEccentricityData.Text = derivedOrbitElements[index: 0]?.ToString();
		labelSemiMinorAxisData.Text = derivedOrbitElements[index: 1]?.ToString();
		labelMajorAxisData.Text = derivedOrbitElements[index: 2]?.ToString();
		labelMinorAxisData.Text = derivedOrbitElements[index: 3]?.ToString();
		labelEccentricAnomalyData.Text = derivedOrbitElements[index: 4]?.ToString();
		labelTrueAnomalyData.Text = derivedOrbitElements[index: 5]?.ToString();
		labelPerihelionDistanceData.Text = derivedOrbitElements[index: 6]?.ToString();
		labelAphelionDistanceData.Text = derivedOrbitElements[index: 7]?.ToString();
		labelLongitudeDescendingNodeData.Text = derivedOrbitElements[index: 8]?.ToString();
		labelArgumentAphelionData.Text = derivedOrbitElements[index: 9]?.ToString();
		labelFocalParameterData.Text = derivedOrbitElements[index: 10]?.ToString();
		labelSemiLatusRectumData.Text = derivedOrbitElements[index: 11]?.ToString();
		labelLatusRectumData.Text = derivedOrbitElements[index: 12]?.ToString();
		labelOrbitalPeriodData.Text = derivedOrbitElements[index: 13]?.ToString();
		labelOrbitalAreaData.Text = derivedOrbitElements[index: 14]?.ToString();
		labelOrbitalPerimeterData.Text = derivedOrbitElements[index: 15]?.ToString();
		labelSemiMeanAxisData.Text = derivedOrbitElements[index: 16]?.ToString();
		labelMeanAxisData.Text = derivedOrbitElements[index: 17]?.ToString();
		labelStandardGravitationalParameterData.Text = derivedOrbitElements[index: 18]?.ToString();
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
	/// This method is called when the mouse pointer enters a control or the control receives focus.
	/// </remarks>
	private void Control_Enter(object sender, EventArgs e)
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
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to store the control that triggered the event for future reference.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the click event for the ToolStripButtonCopyToClipboard.
	/// Shows the form to copy data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to show the form to copy data to the clipboard.
	/// </remarks>
	private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e) => ShowCopyDataToClipboard();

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardLinearEccentricity.
	/// Copies the linear eccentricity data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the linear eccentricity data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardLinearEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLinearEccentricityData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardSemiMinorAxis.
	/// Copies the semi-minor axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the semi-minor axis data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardSemiMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMinorAxisData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardMajorAxis.
	/// Copies the major axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the major axis data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMajorAxisData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardMinorAxis.
	/// Copies the minor axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the minor axis data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMinorAxisData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardEccentricAnomaly.
	/// Copies the eccentric anomaly data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the eccentric anomaly data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardEccentricAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEccentricAnomalyData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardTrueAnomaly.
	/// Copies the true anomaly data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the true anomaly data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardTrueAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelTrueAnomalyData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardPerihelionDistance.
	/// Copies the perihelion distance data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the perihelion distance data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardPerihelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelPerihelionDistanceData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardAphelionDistance.
	/// Copies the aphelion distance data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the aphelion distance data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardAphelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAphelionDistanceData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardLongitudeDescendingNode.
	/// Copies the longitude of the descending node data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the longitude of the descending node data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardLongitudeDescendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeDescendingNodeData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardArgumentAphelion.
	/// Copies the argument of aphelion data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the argument of aphelion data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardArgumentAphelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentAphelionData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardFocalParameter.
	/// Copies the focal parameter data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the focal parameter data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardFocalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFocalParameterData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardSemiLatusRectum.
	/// Copies the semi-latus rectum data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the semi-latus rectum data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardSemiLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiLatusRectumData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardLatusRectum.
	/// Copies the latus rectum data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the latus rectum data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLatusRectumData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardOrbitalPeriod.
	/// Copies the orbital period data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the orbital period data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardOrbitalPeriod_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalPeriodData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardOrbitalArea.
	/// Copies the orbital area data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the orbital area data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardOrbitalArea_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalAreaData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardSemiMeanAxis.
	/// Copies the semi-mean axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the semi-mean axis data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardSemiMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMeanAxisData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardMeanAxis.
	/// Copies the mean axis data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the mean axis data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAxisData.Text);

	/// <summary>
	/// Handles the click event for the MenuitemCopyToClipboardStandardGravitationalParameter.
	/// Copies the standard gravitational parameter data to the clipboard.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to copy the standard gravitational parameter data to the clipboard.
	/// </remarks>
	private void MenuitemCopyToClipboardStandardGravitationalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelStandardGravitationalParameterData.Text);

	#endregion

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// If the <paramref name="sender"/> is a <see cref="Control"/>, its <see cref="Control.Text"/> value is copied to the clipboard.
	/// If the <paramref name="sender"/> is a <see cref="ToolStripItem"/>, its <see cref="ToolStripItem.Text"/> value is copied to the clipboard.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
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
			// Try to set the clipboard text
			try { CopyToClipboard(text: textToCopy); }
			catch
			{ // Throw an exception
				throw new ArgumentException(message: "Unsupported sender type", paramName: nameof(sender));
			}
		}
	}

	/// <summary>
	/// Handles double-click events on the control to open the terminology dialog.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method attempts to parse the current tag text as an integer and opens the terminology dialog
	/// for the corresponding entry if successful.
	/// </remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Try to parse the index from the current tag text
		// If successful, open the terminology dialog for that index
		// If parsing fails, log an error and show an error message
		if (TryParseInt(input: currentTagText, value: out int index, errorMessage: out string errorMessage))
		{
			// Open the terminology dialog for the parsed index
			OpenTerminology(index: (uint)index);
			return;
		}
		// Log the error and show an error message
		Logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
	}

	#endregion
}