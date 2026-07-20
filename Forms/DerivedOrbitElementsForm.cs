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

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>List of derived orbit elements.</summary>
	/// <remarks>This field is used to store the list of derived orbit elements.</remarks>
	private List<object> derivedOrbitElements = [];

	/// <summary>Array of labels corresponding to the derived orbit elements.</summary>
	/// <remarks>This array is used to store references to the labels that display the derived orbit elements.</remarks>
	private readonly KryptonLabel[] orbitDataLabels;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DerivedOrbitElementsForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public DerivedOrbitElementsForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Enable double buffering on the table layout panel and its child labels to reduce flickering during updates
		DoubleBufferingHelper.EnableDoubleBuffering(control: tableLayoutPanel, includeChildLabels: true);
		// Map labels in the exact expected order of the incoming database list
		orbitDataLabels =
		[
			labelLinearEccentricityData, labelSemiMinorAxisData, labelMajorAxisData,
			labelMinorAxisData, labelEccentricAnomalyData, labelTrueAnomalyData,
			labelPerihelionDistanceData, labelAphelionDistanceData, labelLongitudeDescendingNodeData,
			labelArgumentAphelionData, labelFocalParameterData, labelSemiLatusRectumData,
			labelLatusRectumData, labelOrbitalPeriodData, labelOrbitalAreaData,
			labelOrbitalPerimeterData, labelSemiMeanAxisData, labelMeanAxisData,
			labelStandardGravitationalParameterData, labelDirectrixData, labelPerihelionVelocityData,
			labelAphelionVelocityData, labelMeanOrbitalVelocityData, labelCurrentOrbitalVelocityData,
			labelRadialVelocityComponentData, labelTangentialVelocityComponentData, labelSpecificOrbitalEnergyData,
			labelSpecificAngularMomentumData, labelVisVivaEnergyData, labelLongitudeOfPerihelionData,
			labelMeanLongitudeData, labelArgumentOfLatitudeData, labelFlightPathAngleData,
			labelTimeSincePerihelionData, labelTimeToNextPerihelionData, labelTimeSinceAphelionData,
			labelTimeToNextAphelionData, labelSynodicPeriodData, labelTisserandParameterData,
			labelMeanDistanceFromFocusData, labelGeometricAlbedoAdjustedDiameterData
		];
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the internal list of derived orbit elements used by the form. The provided list is stored by reference and will be used to populate the UI when the form loads.</summary>
	/// <param name="list">The list of derived orbit elements to be used by the form.</param>
	/// <remarks>This method is used to set the internal list of derived orbit elements for the form.</remarks>
	public void SetDatabase(List<object> list) => derivedOrbitElements = list;

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
		// Direct cast replaces the 40-line switch statement, assuming the enum values correspond to the indices.
		// If out of range, fallback to 0 (IndexNumber).
		TerminologyElement element = Enum.IsDefined(enumType: typeof(TerminologyElement), value: (int)index)
			? (TerminologyElement)index
			: TerminologyElement.IndexNumber;
		// Create and show the terminology form
		using TerminologyForm formTerminology = new()
		{
			SelectedElement = element,
			TopMost = TopMost
		};
		// Show the form as a dialog
		_ = formTerminology.ShowDialog(owner: this);
	}

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
		// Validate the provided derived orbit elements data
		if (derivedOrbitElements.Count < orbitDataLabels.Length)
		{
			string errorMsg = $"Invalid data: Expected at least {orbitDataLabels.Length} elements, received {derivedOrbitElements.Count}";
			logger.Error(message: errorMsg);
			ShowErrorMessage(message: errorMsg);
			return;
		}
		// Dynamically assign values to reduce code duplication
		for (int i = 0; i < orbitDataLabels.Length; i++)
		{
			orbitDataLabels[i].Text = derivedOrbitElements[index: i]?.ToString() ?? string.Empty;
		}
	}

	#endregion

	#region MouseDown event handlers

	/// <summary>Handles the MouseDown event for controls. Stores the control that triggered the event for future reference.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to store the control that triggered the event for future reference.</remarks>
	protected override void Control_MouseDown(object sender, MouseEventArgs e)
	{
		if (sender is ToolStripMenuItem menuItem && menuItem.Tag is Label targetLabel)
		{
			CopyToClipboard(text: targetLabel.Text);
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for the CopyToClipboardLinearEccentricity.
	/// Copies the linear eccentricity data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the linear eccentricity data to the clipboard.</remarks>
	private void CopyToClipboardLinearEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLinearEccentricityData.Text);

	/// <summary>Handles the click event for the CopyToClipboardSemiMinorAxis.
	/// Copies the semi-minor axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-minor axis data to the clipboard.</remarks>
	private void CopyToClipboardSemiMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMinorAxisData.Text);

	/// <summary>Handles the click event for the CopyToClipboardMajorAxis.
	/// Copies the major axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the major axis data to the clipboard.</remarks>
	private void CopyToClipboardMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMajorAxisData.Text);

	/// <summary>Handles the click event for the CopyToClipboardMinorAxis.
	/// Copies the minor axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the minor axis data to the clipboard.</remarks>
	private void CopyToClipboardMinorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMinorAxisData.Text);

	/// <summary>Handles the click event for the CopyToClipboardEccentricAnomaly.
	/// Copies the eccentric anomaly data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the eccentric anomaly data to the clipboard.</remarks>
	private void CopyToClipboardEccentricAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEccentricAnomalyData.Text);

	/// <summary>Handles the click event for the CopyToClipboardTrueAnomaly.
	/// Copies the true anomaly data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the true anomaly data to the clipboard.</remarks>
	private void CopyToClipboardTrueAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelTrueAnomalyData.Text);

	/// <summary>Handles the click event for the CopyToClipboardPerihelionDistance.
	/// Copies the perihelion distance data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the perihelion distance data to the clipboard.</remarks>
	private void CopyToClipboardPerihelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelPerihelionDistanceData.Text);

	/// <summary>Handles the click event for the CopyToClipboardAphelionDistance.
	/// Copies the aphelion distance data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the aphelion distance data to the clipboard.</remarks>
	private void CopyToClipboardAphelionDistance_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAphelionDistanceData.Text);

	/// <summary>Handles the click event for the CopyToClipboardLongitudeDescendingNode.
	/// Copies the longitude of the descending node data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the longitude of the descending node data to the clipboard.</remarks>
	private void CopyToClipboardLongitudeDescendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeDescendingNodeData.Text);

	/// <summary>Handles the click event for the CopyToClipboardArgumentAphelion.
	/// Copies the argument of aphelion data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the argument of aphelion data to the clipboard.</remarks>
	private void CopyToClipboardArgumentAphelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentAphelionData.Text);

	/// <summary>Handles the click event for the CopyToClipboardFocalParameter.
	/// Copies the focal parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the focal parameter data to the clipboard.</remarks>
	private void CopyToClipboardFocalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFocalParameterData.Text);

	/// <summary>Handles the click event for the CopyToClipboardSemiLatusRectum.
	/// Copies the semi-latus rectum data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-latus rectum data to the clipboard.</remarks>
	private void CopyToClipboardSemiLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiLatusRectumData.Text);

	/// <summary>Handles the click event for the CopyToClipboardLatusRectum.
	/// Copies the latus rectum data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the latus rectum data to the clipboard.</remarks>
	private void CopyToClipboardLatusRectum_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLatusRectumData.Text);

	/// <summary>Handles the click event for the CopyToClipboardOrbitalPeriod.
	/// Copies the orbital period data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital period data to the clipboard.</remarks>
	private void CopyToClipboardOrbitalPeriod_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalPeriodData.Text);

	/// <summary>Handles the click event for the CopyToClipboardOrbitalArea.
	/// Copies the orbital area data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital area data to the clipboard.</remarks>
	private void CopyToClipboardOrbitalArea_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalAreaData.Text);

	/// <summary>Handles the click event for the CopyToClipboardSemiMeanAxis.
	/// Copies the semi-mean axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-mean axis data to the clipboard.</remarks>
	private void CopyToClipboardSemiMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMeanAxisData.Text);

	/// <summary>Handles the click event for the CopyToClipboardMeanAxis.
	/// Copies the mean axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean axis data to the clipboard.</remarks>
	private void CopyToClipboardMeanAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAxisData.Text);

	/// <summary>Handles the click event for the CopyToClipboardStandardGravitationalParameter.
	/// Copies the standard gravitational parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the standard gravitational parameter data to the clipboard.</remarks>
	private void CopyToClipboardStandardGravitationalParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelStandardGravitationalParameterData.Text);

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog for the corresponding entry if successful.</remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Safe casting from sender directly to evaluate the exact control being interacted with
		if (sender is Control control && control.Tag != null)
		{
			// Retrieve the tag text, ensuring it's not null
			string tagText = control.Tag.ToString() ?? string.Empty;
			// Attempt to parse the tag text as an integer
			if (TryParseInt(input: tagText, value: out int index, errorMessage: out string errorMessage))
			{
				// Open the terminology dialog for the parsed index
				OpenTerminology(index: (uint)index);
				return;
			}
			// Log the error and show an error message
			logger.Error(message: $"Failed to parse index from tag text '{tagText}': {errorMessage}");
			ShowErrorMessage(message: $"Failed to parse index from tag text '{tagText}': {errorMessage}");
		}
	}

	#endregion
}