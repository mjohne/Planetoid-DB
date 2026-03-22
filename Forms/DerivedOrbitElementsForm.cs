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

	/// <summary>Opens the terminology dialog for a specific derived orbit element.
	/// The <paramref name="index"/> selects which terminology entry to show. Values outside the supported range
	/// are normalized to the default (index 0).</summary>
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

	/// <summary>Shows the form to copy data to the clipboard.</summary>
	/// <remarks>This method is used to show the form for copying data to the clipboard.</remarks>
	#endregion

	#region form event handlers

	/// <summary>Fired when the derived orbit elements form is loaded.
	/// Clears the status area, validates the provided derived-element data and populates the UI labels
	/// with the corresponding values. If the provided data is invalid an error is logged and shown to the user.</summary>
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

	/// <summary>Handles the Click event of the Save As Text menu item, allowing the user to export the contents of the table layout
	/// panel to a text file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the method exports the current list view results to the specified text file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As Text menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the text file to save the list view results; if the user confirms the save operation, call the SaveAsText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save as Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsText(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as LaTeX' menu item, allowing the user to export the contents of the table
	/// layout panel to a LaTeX file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the current table layout panel data to a LaTeX file at the specified location.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the LaTeX file to save the list view results; if the user confirms the save operation, call the SaveAsLatex method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save as LaTeX"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsLatex(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as Markdown' menu item, allowing the user to export the current table layout
	/// panel content to a Markdown file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the current table layout panel data is exported as a Markdown file. The export operation will not proceed
	/// if the dialog is canceled or invalid input is provided.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Markdown file to save the list view results; if the user confirms the save operation, call the SaveAsMarkdown method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Markdown Files (*.md)|*.md|All Files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save as Markdown"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsMarkdown(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as AsciiDoc' menu item, allowing the user to export the current list view
	/// results to an AsciiDoc file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout is exported as an AsciiDoc document. The export operation is only performed if the dialog is
	/// successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the AsciiDoc file to save the list view results; if the user confirms the save operation, call the SaveAsAsciiDoc method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*",
			DefaultExt = "adoc",
			Title = "Save as AsciiDoc"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsAsciiDoc(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as ReStructuredText' menu item, allowing the user to export the list view
	/// results to a ReStructuredText (.rst) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the current table layout is exported as a ReStructuredText file. The export includes the list of readable
	/// designations.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the ReStructuredText file to save the list view results; if the user confirms the save operation, call the SaveAsReStructuredText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*",
			DefaultExt = "rst",
			Title = "Save as ReStructuredText"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsReStructuredText(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save As Textile' menu item to export the contents of the table layout panel to a
	/// Textile file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the file name and location for the Textile
	/// export. If the user confirms the operation, the contents of the table layout panel are saved in Textile
	/// format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the textile file to save the list view results; if the user confirms the save operation, call the SaveAsTextile method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*",
			DefaultExt = "textile",
			Title = "Save as Textile"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsTextile(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as Word' menu item to export the contents of the table layout panel to a Word
	/// document.</summary>
	/// <remarks>This method displays a Save File dialog to allow the user to specify the destination for the
	/// exported Word document. If the user confirms the operation, the contents of the table layout panel are saved to the
	/// specified file in .docx format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Word file to save the list view results; if the user confirms the save operation, call the SaveAsWord method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save as Word"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsWord(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as ODT' menu item to export the contents of the table layout panel to an
	/// OpenDocument Text (.odt) file.</summary>
	/// <remarks>Displays a Save File dialog allowing the user to specify the destination file. If the user
	/// confirms, the method exports the current list view results to the specified ODT file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Text file to save the list view results; if the user confirms the save operation, call the SaveAsOdt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save as OpenDocument Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsOdt(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As RTF menu item, allowing the user to export the contents of the table layout
	/// panel to a Rich Text Format (RTF) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the method exports the current list view results to an RTF file. The exported file includes the readable
	/// designations displayed in the table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the Save As RTF menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the RTF file to save the list view results; if the user confirms the save operation, call the SaveAsRtf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save as RTF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsRtf(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save As Abiword' menu item, allowing the user to export the list view results to an
	/// Abiword (.abw) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the Abiword file location and name. If the user
	/// confirms, the current table layout is exported to the specified file in Abiword format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Abiword file to save the list view results; if the user confirms the save operation, call the SaveAsAbiword method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*",
			DefaultExt = "abw",
			Title = "Save as Abiword"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsAbiword(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As WPS menu item, allowing the user to export the current list view results to
	/// a WPS Writer file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the selected WPS file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Writer file to save the list view results; if the user confirms the save operation, call the SaveAsWps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*",
			DefaultExt = "wps",
			Title = "Save as WPS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsWps(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event of the 'Save as Excel' menu item to export the contents of the table layout panel to an
	/// Excel file.</summary>
	/// <remarks>This method displays a Save File dialog to the user for specifying the Excel file location and
	/// name. If the user confirms the operation, the contents of the table layout panel are exported to the specified
	/// Excel file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Excel file to save the list view results; if the user confirms the save operation, call the SaveAsExcel method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save as Excel"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsExcel(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as ODS' menu item, allowing the user to export the contents of the table
	/// layout panel to an OpenDocument Spreadsheet (ODS) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination ODS file. If the user confirms,
	/// the method exports the current table layout panel data to the selected file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Spreadsheet file to save the list view results; if the user confirms the save operation, call the SaveAsOds method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save as ODS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsOds(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as CSV' menu item, allowing the user to export the contents of the table
	/// layout panel to a CSV file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the contents of the table layout panel are exported to the selected CSV file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CSV file to save the list view results; if the user confirms the save operation, call the SaveAsCsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save as CSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsCsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As TSV menu item, allowing the user to export the contents of the table layout
	/// panel to a tab-separated values (TSV) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms the operation, the method exports the current data to a TSV file. The exported file can be used for data
	/// exchange or further processing in spreadsheet applications.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TSV file to save the list view results; if the user confirms the save operation, call the SaveAsTsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save as TSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsTsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as PSV' menu item to export the contents of the table layout panel to a
	/// pipe-separated values (PSV) file.</summary>
	/// <remarks>This method displays a Save File dialog allowing the user to specify the destination file for the
	/// PSV export. If the user confirms the operation, the current data is saved in PSV format. The export includes the
	/// list of readable designations from the table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PSV file to save the list view results; if the user confirms the save operation, call the SaveAsPsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save as PSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsPsv(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event for the 'Save As ET' menu item, allowing the user to export the current list view results
	/// to an ET file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the file location and name. If the user
	/// confirms, the current table layout is exported to the specified ET file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the ET file to save the list view results; if the user confirms the save operation, call the SaveAsEt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "ET Files (*.et)|*.et|All Files (*.*)|*.*",
			DefaultExt = "et",
			Title = "Save as ET"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsEt(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as HTML' menu item to export the contents of the table layout panel to an HTML
	/// file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the destination file. If the user
	/// confirms, the method exports the current table layout panel data to the selected HTML file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the HTML file to save the list view results; if the user confirms the save operation, call the SaveAsHtml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save as HTML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsHtml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as XML' menu item to export the current list view results to an XML file.</summary>
	/// <remarks>This method displays a Save File dialog to the user for specifying the XML file location and name.
	/// If the user confirms the operation, the current table layout panel data is exported as an XML file. The export
	/// includes the list of readable designations.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XML file to save the list view results; if the user confirms the save operation, call the SaveAsXml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as XML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsXml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as DocBook' menu item, allowing the user to export the list view results to a
	/// DocBook XML file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout is exported as a DocBook XML file. The export operation may overwrite an existing file if the
	/// user selects one.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the DocBook file to save the list view results; if the user confirms the save operation, call the SaveAsDocBook method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as DocBook"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsDocBook(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as JSON' menu item, allowing the user to export the current list view results
	/// to a JSON file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current data is exported to the selected JSON file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the JSON file to save the list view results; if the user confirms the save operation, call the SaveAsJson method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save as JSON"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsJson(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);

	}

	/// <summary>Handles the click event for the 'Save as YAML' menu item, allowing the user to export the current table layout
	/// panel data to a YAML file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms the operation, the current data from the table layout panel is exported to the selected YAML
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the YAML file to save the list view results; if the user confirms the save operation, call the SaveAsYaml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save as YAML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsYaml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as TOML' menu item, allowing the user to export the current list view results
	/// to a TOML file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// current table layout panel data is exported to the specified TOML file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TOML file to save the list view results; if the user confirms the save operation, call the SaveAsToml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*",
			DefaultExt = "toml",
			Title = "Save as TOML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsToml(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As SQL menu item to export the current list view results to a SQL file.</summary>
	/// <remarks>This method displays a Save File dialog to allow the user to specify the destination for the SQL
	/// export. If the user confirms the operation, the current contents of the table layout panel are exported as a SQL
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As SQL menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQL file to save the list view results; if the user confirms the save operation, call the SaveAsSql method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save as SQL"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsSql(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as SQLite' menu item, allowing the user to export the current list view
	/// results to a SQLite database file.</summary>
	/// <remarks>Opens a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the selected SQLite file. The export includes the current contents of the associated
	/// table layout panel.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQLite file to save the list view results; if the user confirms the save operation, call the SaveAsSqlite method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*",
			DefaultExt = "sqlite",
			Title = "Save as SQLite"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsSqlite(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event of the 'Save as PDF' menu item to export the contents of the table layout panel to a PDF
	/// file.</summary>
	/// <remarks>Displays a Save File dialog to allow the user to specify the destination for the PDF file. If the
	/// user confirms the operation, the contents of the table layout panel are exported to the specified PDF
	/// file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PDF file to save the list view results; if the user confirms the save operation, call the SaveAsPdf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save as PDF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsPdf(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As PostScript menu item to allow the user to export the list view results as a
	/// PostScript file.</summary>
	/// <remarks>This handler displays a Save File dialog for the user to specify the destination file. If the user
	/// confirms, the current table layout is exported to a PostScript file. The export operation is only performed if the
	/// dialog is successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As PostScript menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PostScript file to save the list view results; if the user confirms the save operation, call the SaveAsPostScript method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save as PostScript"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsPostScript(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as EPUB' menu item, allowing the user to export the current list view results
	/// to an EPUB file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the EPUB file location and name. If the user
	/// confirms the operation, the current table layout is exported as an EPUB file. The export is only performed if the
	/// dialog is successfully prepared and the user completes the save action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the EPUB file to save the list view results; if the user confirms the save operation, call the SaveAsEpub method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save as EPUB"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsEpub(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As MOBI menu item, allowing the user to export the current list view results to
	/// a MOBI file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms, the
	/// method exports the data to the specified MOBI file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the MOBI file to save the list view results; if the user confirms the save operation, call the SaveAsMobi method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save as MOBI"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsMobi(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As XPS menu item, allowing the user to export the current list view results to
	/// an XPS file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination XPS file. If the user confirms,
	/// the current table layout panel content is exported to the specified XPS file.</remarks>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XPS file to save the list view results; if the user confirms the save operation, call the SaveAsXps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*",
			DefaultExt = "xps",
			Title = "Save as XPS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsXps(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save as FictionBook2' menu item, allowing the user to export the current list view
	/// results to a FictionBook2 (.fb2) file.</summary>
	/// <remarks>Displays a Save File dialog for the user to specify the destination file. If the user confirms the
	/// operation, the method exports the data to the specified FictionBook2 file format.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the FictionBook2 file to save the list view results; if the user confirms the save operation, call the SaveAsFictionBook2 method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*",
			DefaultExt = "fb2",
			Title = "Save as FictionBook2"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsFictionBook2(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save as CHM' menu item, allowing the user to export the current list view results
	/// to a Compiled HTML Help (CHM) file.</summary>
	/// <remarks>This method displays a Save File dialog for the user to specify the destination and filename for
	/// the CHM export. If the user confirms the operation, the current table layout is exported to a CHM file at the
	/// specified location.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CHM file to save the list view results; if the user confirms the save operation, call the SaveAsChm method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*",
			DefaultExt = "chm",
			Title = "Save as CHM"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		TableLayoutPanelExporter.SaveAsChm(tableLayoutPanel: tableLayoutPanel, title: "List of readable designations", fileName: saveFileDialog.FileName);
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog
	/// for the corresponding entry if successful.</remarks>
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