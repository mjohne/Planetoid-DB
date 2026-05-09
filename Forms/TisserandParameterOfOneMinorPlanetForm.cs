// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form for displaying the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>This form computes and presents the Tisserand parameter values for a minor planet using the standard three-body formula. The results are shown in a two-column table layout: planet name in the first column, Tisserand parameter value in the second column.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class TisserandParameterOfOneMinorPlanetForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Semi-major axis of the minor planet in AU.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double semiMajorAxis;

	/// <summary>Orbital eccentricity of the minor planet.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double eccentricity;

	/// <summary>Orbital inclination of the minor planet in degrees.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double inclinationDeg;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="TisserandParameterOfOneMinorPlanetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public TisserandParameterOfOneMinorPlanetForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name with timestamp
		dialog.FileName = $"Tisserand-Parameters_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Sets the orbital elements of the minor planet used for computing Tisserand parameter values.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="inclinationDeg">The inclination to the ecliptic in degrees.</param>
	/// <remarks>Call this method before showing the form so that the Tisserand parameter data is available on load.</remarks>
	public void SetOrbitalElements(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg)
	{
		this.semiMajorAxis = semiMajorAxis;
		this.eccentricity = eccentricity;
		this.inclinationDeg = inclinationDeg;
	}

	/// <summary>Displays a save dialog and exports the table layout panel contents using the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
	{
		// Create and configure the save file dialog
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog; return if the user cancels
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// Perform the export with a wait cursor
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(tableLayoutPanel, "Tisserand parameters of a minor planet", saveFileDialog.FileName);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// Reset the cursor in the finally block
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event. Clears the status bar, computes Tisserand parameter values for all eight planets, and populates the table.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Tisserand parameter values are calculated using <see cref="TisserandParameterCalculator.CalculateTisserandParameters"/> and displayed in the second column of the <see cref="tableLayoutPanel"/>.</remarks>
	private void TisserandParameterOfOneMinorPlanetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Calculate Tisserand parameters for all 8 planets
			List<TisserandParameterCalculator.TisserandResult> results = TisserandParameterCalculator.CalculateTisserandParameters(
				semiMajorAxis: semiMajorAxis,
				eccentricity: eccentricity,
				inclinationDeg: inclinationDeg);
			// Populate the data labels (one per planet row, index 0 = Mercury … 7 = Neptune)
			if (results.Count >= 8)
			{
				labelMercuryData.Text = results[index: 0].TisserandValue.ToString(format: "F6");
				labelVenusData.Text = results[index: 1].TisserandValue.ToString(format: "F6");
				labelEarthData.Text = results[index: 2].TisserandValue.ToString(format: "F6");
				labelMarsData.Text = results[index: 3].TisserandValue.ToString(format: "F6");
				labelJupiterData.Text = results[index: 4].TisserandValue.ToString(format: "F6");
				labelSaturnData.Text = results[index: 5].TisserandValue.ToString(format: "F6");
				labelUranusData.Text = results[index: 6].TisserandValue.ToString(format: "F6");
				labelNeptuneData.Text = results[index: 7].TisserandValue.ToString(format: "F6");
			}
		}
		// Log any exceptions that occur during the calculation and show an error message to the user
		catch (Exception ex)
		{
			logger.Error(message: $"Error computing Tisserand parameter values: {ex}");
			ShowErrorMessage(message: $"Error computing Tisserand parameter values: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a text file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a LaTeX file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a Markdown file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an AsciiDoc file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a ReStructuredText file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a Textile file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a Word document with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an OpenDocument Text file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an RTF file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an Abiword file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a WPS file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an Excel file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an ODS file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a CSV file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a TSV file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a PSV file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an ET file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an HTML file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an XML file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a DocBook file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.dbk)|*.dbk|All Files (*.*)|*.*", defaultExt: "dbk", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a JSON file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a YAML file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a TOML file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a SQL file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite database file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>				   
	/// <remarks>Invokes the export action for saving as a SQLite database file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a PDF file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a PostScript file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an EPUB file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a MOBI file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as an XPS file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a FictionBook2 file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>Invokes the export action for saving as a CHM file with the appropriate filter and dialog title.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "CHM Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Mercury to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Mercury from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToMercury_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMercuryData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Venus to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Venus from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToVenus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelVenusData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Earth to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Earth from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToEarth_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEarthData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Mars to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Mars from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToMars_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMarsData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Jupiter to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Jupiter from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToJupiter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelJupiterData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Saturn to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Saturn from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToSaturn_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSaturnData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Uranus to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Uranus from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToUranus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelUranusData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Neptune to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Neptune from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToNeptune_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNeptuneData.Text);

	#endregion
}