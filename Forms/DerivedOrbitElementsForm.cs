// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

using static Planetoid_DB.TerminologyForm;

namespace Planetoid_DB;

/// <summary>Form for displaying derived orbit elements.</summary>
/// <remarks>This form provides a user interface for displaying derived orbit elements.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DerivedOrbitElementsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Derived Orbit Elements";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Derived-Orbit-Elements";

	#endregion

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Stores the current tag text of the control.</summary>
	/// <remarks>This field is used to keep track of the current tag text of the control.</remarks>
	private string currentTagText = string.Empty;

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>List of derived orbit elements.</summary>
	/// <remarks>This field is used to store the list of derived orbit elements.</remarks>
	private List<object> derivedOrbitElements = [];

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DerivedOrbitElementsForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public DerivedOrbitElementsForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Apply comprehensive flicker reduction for the TableLayoutPanel
		OptimizeTableLayoutPanelForFlickerReduction();
	}

	/// <summary>Optimizes the TableLayoutPanel to eliminate flickering during label updates.</summary>
	/// <remarks>This method enables double buffering and optimized painting styles on the panel and all child labels.</remarks>
	private void OptimizeTableLayoutPanelForFlickerReduction() => DoubleBufferingHelper.EnableDoubleBuffering(control: tableLayoutPanel, includeChildLabels: true);


	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Tries to parse an integer from the input string.</summary>
	/// <param name="input">The input string to parse.</param>
	/// <param name="value">The parsed integer value if successful.</param>
	/// <param name="errorMessage">An error message if parsing fails.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	/// <remarks>This method is used to parse an integer from the input string.</remarks>
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

	/// <summary>Opens the terminology dialog for a specific derived orbit element. The <paramref name="index"/> selects which terminology entry to show. Values outside the supported range are normalized to the default (index 0).</summary>
	/// <param name="index">Zero-based index selecting the terminology topic (valid range: 0..38).</param>
	/// <remarks>This method is used to open the terminology dialog for a specific derived orbit element.</remarks>
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
		_ = formTerminology.ShowDialog(owner: this);
	}

	/// <summary>Sets the internal list of derived orbit elements used by the form.</summary>
	/// <param name="list">A list of derived orbit element values. The list is stored by reference and will be used to populate the UI when the form loads.</param>
	/// <remarks>This method is used to set the internal list of derived orbit elements.</remarks>
	public void SetDatabase(List<object> list) => derivedOrbitElements = list;

	#endregion

	#region form event handlers

	/// <summary>Fired when the derived orbit elements form is loaded. Clears the status area, validates the provided derived-element data and populates the UI labels with the corresponding values. If the provided data is invalid an error is logged and shown to the user.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the form is loaded.</remarks>
	private void DerivedOrbitElementsForm_Load(object sender, EventArgs e)
	{
		// Set the status bar text
		ClearStatusBar(label: labelInformation);
		if (derivedOrbitElements.Count < 41)
		{
			// Log the error and show an error message
			logger.Error(message: $"Invalid data: Expected at least 41 elements, received {derivedOrbitElements.Count}");
			ShowErrorMessage(message: $"Invalid data: Expected at least 41 elements, received {derivedOrbitElements.Count}");
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
		labelDirectrixData.Text = derivedOrbitElements[index: 19]?.ToString();
		labelPerihelionVelocityData.Text = derivedOrbitElements[index: 20]?.ToString();
		labelAphelionVelocityData.Text = derivedOrbitElements[index: 21]?.ToString();
		labelMeanOrbitalVelocityData.Text = derivedOrbitElements[index: 22]?.ToString();
		labelCurrentOrbitalVelocityData.Text = derivedOrbitElements[index: 23]?.ToString();
		labelRadialVelocityComponentData.Text = derivedOrbitElements[index: 24]?.ToString();
		labelTangentialVelocityComponentData.Text = derivedOrbitElements[index: 25]?.ToString();
		labelSpecificOrbitalEnergyData.Text = derivedOrbitElements[index: 26]?.ToString();
		labelSpecificAngularMomentumData.Text = derivedOrbitElements[index: 27]?.ToString();
		labelVisVivaEnergyData.Text = derivedOrbitElements[index: 28]?.ToString();
		labelLongitudeOfPerihelionData.Text = derivedOrbitElements[index: 29]?.ToString();
		labelMeanLongitudeData.Text = derivedOrbitElements[index: 30]?.ToString();
		labelArgumentOfLatitudeData.Text = derivedOrbitElements[index: 31]?.ToString();
		labelFlightPathAngleData.Text = derivedOrbitElements[index: 32]?.ToString();
		labelTimeSincePerihelionData.Text = derivedOrbitElements[index: 33]?.ToString();
		labelTimeToNextPerihelionData.Text = derivedOrbitElements[index: 34]?.ToString();
		labelTimeSinceAphelionData.Text = derivedOrbitElements[index: 35]?.ToString();
		labelTimeToNextAphelionData.Text = derivedOrbitElements[index: 36]?.ToString();
		labelSynodicPeriodData.Text = derivedOrbitElements[index: 37]?.ToString();
		labelTisserandParameterData.Text = derivedOrbitElements[index: 38]?.ToString();
		labelMeanDistanceFromFocusData.Text = derivedOrbitElements[index: 39]?.ToString();
		labelGeometricAlbedoAdjustedDiameterData.Text = derivedOrbitElements[index: 40]?.ToString();
	}

	#endregion

	#region MouseDown event handlers

	/// <summary>Handles the MouseDown event for controls. Stores the control that triggered the event for future reference.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to store the control that triggered the event for future reference.</remarks>
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

	/// <summary>Handles the click event for the MenuitemCopyToClipboardLinearEccentricity.
	/// Copies the linear eccentricity data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the linear eccentricity data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardLinearEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLinearEccentricityData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardSemiMinorAxis.
	/// Copies the semi-minor axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-minor axis data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardSemiMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMinorAxisData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardMajorAxis.
	/// Copies the major axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the major axis data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMajorAxisData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardMinorAxis.
	/// Copies the minor axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the minor axis data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMinorAxisData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardEccentricAnomaly.
	/// Copies the eccentric anomaly data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the eccentric anomaly data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardEccentricAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEccentricAnomalyData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardTrueAnomaly.
	/// Copies the true anomaly data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the true anomaly data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardTrueAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelTrueAnomalyData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardPerihelionDistance.
	/// Copies the perihelion distance data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the perihelion distance data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardPerihelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelPerihelionDistanceData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardAphelionDistance.
	/// Copies the aphelion distance data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the aphelion distance data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardAphelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAphelionDistanceData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardLongitudeDescendingNode.
	/// Copies the longitude of the descending node data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the longitude of the descending node data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardLongitudeDescendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeDescendingNodeData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardArgumentAphelion.
	/// Copies the argument of aphelion data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the argument of aphelion data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardArgumentAphelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentAphelionData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardFocalParameter.
	/// Copies the focal parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the focal parameter data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardFocalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFocalParameterData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardSemiLatusRectum.
	/// Copies the semi-latus rectum data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-latus rectum data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardSemiLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiLatusRectumData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardLatusRectum.
	/// Copies the latus rectum data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the latus rectum data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLatusRectumData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardOrbitalPeriod.
	/// Copies the orbital period data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital period data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardOrbitalPeriod_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalPeriodData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardOrbitalArea.
	/// Copies the orbital area data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital area data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardOrbitalArea_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalAreaData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardSemiMeanAxis.
	/// Copies the semi-mean axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-mean axis data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardSemiMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMeanAxisData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardMeanAxis.
	/// Copies the mean axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean axis data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAxisData.Text);

	/// <summary>Handles the click event for the MenuitemCopyToClipboardStandardGravitationalParameter.
	/// Copies the standard gravitational parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the standard gravitational parameter data to the clipboard.</remarks>
	private void MenuitemCopyToClipboardStandardGravitationalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelStandardGravitationalParameterData.Text);

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog for the corresponding entry if successful.</remarks>
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