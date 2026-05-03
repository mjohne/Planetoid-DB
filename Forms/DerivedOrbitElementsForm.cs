// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
	public DerivedOrbitElementsForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Derived-Orbit-Elements_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Displays a save dialog and exports the table layout panel contents using the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title. The dialog allows the user to choose where to save the exported file and what name to give it.
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog. If the user cancels the dialog, the method returns without performing any export action.
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress, and then invoke the specified export action with the text box containing the output, the title for the export, and the selected file name. After the export action is completed, reset the cursor to the default state.
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(tableLayoutPanel, "Derived Orbit Elements", saveFileDialog.FileName);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

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
		_ = formTerminology.ShowDialog();
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

	/// <summary>Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.</summary>
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

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a ReStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word document.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As RTF menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As SQL menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As PostScript menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

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