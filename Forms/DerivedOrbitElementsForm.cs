// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;

using static Planetoid_DB.TerminologyForm;

namespace Planetoid_DB;

/// <summary>
/// Form for displaying derived orbit elements.
/// </summary>
/// <remarks>
/// This form provides a user interface for displaying derived orbit elements.
/// </remarks>
public partial class DerivedOrbitElementsForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This field is used to log messages for the form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Stores the planetoids database.
	/// </summary>
	/// <remarks>
	/// This list is used to store the planetoids database entries.
	/// </remarks>
	private readonly List<string> planetoidsDatabase = [];

	/// <summary>
	/// Stores the current tag text of the control.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the current tag text of the control.
	/// </remarks>
	private string currentTagText = string.Empty;

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

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
		formTerminology.SelectedElement = index switch
		{
			0 => TerminologyElement.IndexNumber,
			1 => TerminologyElement.ReadableDesignation,
			2 => TerminologyElement.Epoch,
			3 => TerminologyElement.MeanAnomalyAtTheEpoch,
			4 => TerminologyElement.ArgumentOfThePerihelion,
			5 => TerminologyElement.LongitudeOfTheAscendingNode,
			6 => TerminologyElement.InclinationToTheEcliptic,
			7 => TerminologyElement.OrbitalEccentricity,
			8 => TerminologyElement.MeanDailyMotion,
			9 => TerminologyElement.SemiMajorAxis,
			10 => TerminologyElement.AbsoluteMagnitude,
			11 => TerminologyElement.SlopeParameter,
			12 => TerminologyElement.Reference,
			13 => TerminologyElement.NumberOfOppositions,
			14 => TerminologyElement.NumberOfObservations,
			15 => TerminologyElement.ObservationSpan,
			16 => TerminologyElement.RmsResidual,
			17 => TerminologyElement.ComputerName,
			18 => TerminologyElement.Flags,
			19 => TerminologyElement.DateOfLastObservation,
			20 => TerminologyElement.LinearEccentricity,
			21 => TerminologyElement.SemiMinorAxis,
			22 => TerminologyElement.MajorAxis,
			23 => TerminologyElement.MinorAxis,
			24 => TerminologyElement.EccentricAnomaly,
			25 => TerminologyElement.TrueAnomaly,
			26 => TerminologyElement.PerihelionDistance,
			27 => TerminologyElement.AphelionDistance,
			28 => TerminologyElement.LongitudeOfTheDescendingNode,
			29 => TerminologyElement.ArgumentOfTheAphelion,
			30 => TerminologyElement.FocalParameter,
			31 => TerminologyElement.SemiLatusRectum,
			32 => TerminologyElement.LatusRectum,
			33 => TerminologyElement.OrbitalPeriod,
			34 => TerminologyElement.OrbitalArea,
			35 => TerminologyElement.OrbitalPerimeter,
			36 => TerminologyElement.SemiMeanAxis,
			37 => TerminologyElement.MeanAxis,
			38 => TerminologyElement.StandardGravitationalParameter,
			_ => TerminologyElement.IndexNumber,
		};
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
		// Create a new List to store the data to copy
		// The capacity is set to 0 because we will add items dynamically
		// The items in the List are the labels that contain the data to be copied
		// The labels are accessed using their respective properties

		List<string> dataToCopy = [
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
		];
		// Create a new list to store the non-empty data items
		List<string> dataToCopyList = [.. dataToCopy.Where(predicate: static item => !string.IsNullOrEmpty(value: item))];
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
		ClearStatusBar(label: labelInformation);
		if (derivedOrbitElements.Count < 19)
		{
			// Log the error and show an error message
			logger.Error(message: "Invalid data");
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
	protected override void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
			// Store the current tag text of the control
			currentTagText = control.Tag?.ToString() ?? string.Empty;
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
		logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
	}

	#endregion
}