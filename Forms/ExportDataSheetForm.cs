/*
 * File:        ExportDataSheetForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for exporting data sheets with various formats.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Export;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Text;

namespace Planetoid_DB;

/// <summary>Form for exporting data sheets with various formats.</summary>
/// <remarks>This form allows users to select orbital elements and export them in different formats.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ExportDataSheetForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>List of orbit elements to be exported</summary>
	/// <remarks>This list contains the names of the orbital elements that the user has selected for export.</remarks>
	private List<string> orbitElements = [];

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="ExportDataSheetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public ExportDataSheetForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the internal list of orbit elements that will be used for export operations.</summary>
	/// <param name="list">A list of orbit element values (strings). The list is stored by reference.</param>
	/// <remarks>This method is used to set the internal list of orbit elements that will be used for export operations.</remarks>
	public void SetDatabase(List<string> list) => orbitElements = list;


	/// <summary>Updates the enabled state of the export button based on the number of checked items in the orbital elements checklist.</summary>
	/// <remarks>This method is used to enable or disable the export button based on whether any items are checked in the orbital elements checklist.</remarks>
	private void UpdateExportButtonState() => toolStripDropDownButtonExport.Enabled = checkedListBoxOrbitalElements.CheckedItems.Count > 0;

	/// <summary>Checks or unchecks all items in the orbital elements checklist and toggles export buttons.</summary>
	/// <param name="check">If true, all items are checked; if false, all items are unchecked.</param>
	/// <remarks>This method is used to check or uncheck all items in the orbital elements checklist and toggle the export buttons accordingly.</remarks>
	private void CheckIt(bool check)
	{
		// Check or uncheck all items in the checked list box
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check or uncheck the item at index i
			checkedListBoxOrbitalElements.SetItemChecked(index: i, value: check);
		}
		//toolStripDropDownButtonExport.Enabled = !IsAllUnmarked();
		UpdateExportButtonState();
	}

	/// <summary>Checks all items in the orbital elements checklist.</summary>
	/// <remarks>This method is used to mark all items in the orbital elements checklist.</remarks>
	private void MarkAll() => CheckIt(check: true);

	/// <summary>Unchecks all items in the orbital elements checklist.</summary>
	/// <remarks>This method is used to unmark all items in the orbital elements checklist.</remarks>
	private void UnmarkAll() => CheckIt(check: false);

	/// <summary>Executes the export operation using the specified exporter.</summary>
	/// <param name="exporter">The exporter to use for the export operation.</param>
	/// <remarks>This method checks for the presence of required orbit elements, prompts the user for a file path, and performs the export operation using the provided exporter.</remarks>
	private void ExecuteExport(IOrbitDataExporter exporter)
	{
		// Log the start of the export operation
		logger.Info(message: $"Starting export operation using {exporter.Extension} exporter.");
		// Check if the exporter is null and throw an exception if it is
		ArgumentNullException.ThrowIfNull(argument: exporter);
		// Check if the orbit elements list is null or does not contain enough elements for export
		if (orbitElements == null || orbitElements.Count < 2)
		{
			// Log an error message indicating that the orbit elements list is null or does not contain enough elements for export
			logger.Error(message: "Orbit elements list is null or does not contain enough elements for export.");
			ShowErrorMessage("Orbit elements list is null or does not contain enough elements for export.");
			return;
		}
		// Use the first element of the orbit elements list as the default file name for the export
		string defaultFileName = orbitElements[index: 0];
		// Create an export title using the first two elements of the orbit elements list
		string exportTitle = $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}";
		// Create a SaveFileDialog to allow the user to select the file path and name for the exported file
		using SaveFileDialog dialog = new()
		{
			Filter = exporter.Filter,
			DefaultExt = exporter.Extension,
			Title = exporter.Title,
			InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments),
			FileName = $"{defaultFileName}.{exporter.Extension}"
		};
		// Show the save file dialog to select the file path and name. If the user cancels, log a warning and return.
		if (dialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			logger.Warn(message: "Export operation canceled by the user.");
			return;
		}
		// Log the file name selected by the user for exporting data
		logger.Info(message: $"User selected file {dialog.FileName} for exporting data.");
		// Create a dictionary to hold the selected orbital elements and their corresponding values
		Dictionary<string, string> selectedData = [];
		// Iterate through the items in the checked list box and add the checked items to the selectedData dictionary
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// If it is checked, add the orbit element to the selectedData dictionary
				string value = i < orbitElements.Count ? orbitElements[index: i] : "N/A";
				string key = checkedListBoxOrbitalElements.Items[index: i].ToString() ?? $"Element_{i}";
				selectedData[key] = value;
			}
		}
		// Perform the export operation using the provided exporter and handle any exceptions that may occur
		try
		{
			// Call the Export method of the exporter to perform the export operation
			exporter.Export(filePath: dialog.FileName, exportTitle: exportTitle, selectedData: selectedData);
			// Log that the data was exported successfully
			logger.Info(message: $"Data exported successfully to {exporter.Extension} file: {dialog.FileName}");
			// Show a message box indicating that the data was exported successfully
			KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
		// Handle any exceptions that may occur during the export operation
		catch (Exception ex)
		{
			// Log the exception and show an error message to the user
			logger.Error(exception: ex, message: $"Failed to export to {exporter.Extension}");
			// Show an error message to the user indicating that the export operation failed
			ShowErrorMessage($"Failed to export to {exporter.Extension}: {ex.Message}");
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the export form loads. Clears the status area and selects all available orbital elements by default.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and set up any necessary data.</remarks>
	private void ExportDataSheetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
		// Update the state of the export button
		UpdateExportButtonState();
	}

	#endregion

	#region Click & ButtonClick event handlers

	/// <summary>Handles the Click event of the Mark All tool strip button. Marks all items in the orbital elements checklist.</summary>
	/// <param name="sender">Event source (the Mark All tool strip button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to mark all items in the orbital elements checklist.</remarks>
	private void ToolStripButtonMarkAll_Click(object sender, EventArgs e)
	{
		// Log that the Mark All button was clicked and that all items in the orbital elements checklist are being marked
		logger.Info(message: "Mark All button clicked. Marking all items in the orbital elements checklist.");
		// Call the MarkAll method to mark all items in the orbital elements checklist
		MarkAll();
	}

	/// <summary>Handles the Click event of the Unmark All tool strip button. Unmarks all items in the orbital elements checklist.</summary>
	/// <param name="sender">Event source (the Unmark All tool strip button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to unmark all items in the orbital elements checklist.</remarks>
	private void ToolStripButtonUnmarkAll_Click(object sender, EventArgs e)
	{
		// Log that the Unmark All button was clicked and that all items in the orbital elements checklist are being unmarked
		logger.Info(message: "Unmark All button clicked. Unmarking all items in the orbital elements checklist.");
		// Call the UnmarkAll method to unmark all items in the orbital elements checklist
		UnmarkAll();
	}

	/// <summary>Handles the Click event of the Export As Text menu item. Exports the selected orbital elements to a text file.</summary>
	/// <param name="sender">Event source (the Export As Text menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to a text file.</remarks>
	private void ToolStripMenuItemExportAsText_Click(object sender, EventArgs e)
	{
		// Log that the Export As Text menu item was clicked
		logger.Info(message: "Export As Text menu item clicked. Exporting selected orbital elements to a text file.");
		// Call the ExecuteExport method to export the selected orbital elements using the TextExporter
		ExecuteExport(exporter: new TextExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As vCalendar menu item. Exports the selected orbital elements to an vCalendar file.</summary>
	/// <param name="sender">Event source (the Export As vCalendar menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to an vCalendar file.</remarks>
	private void ToolStripMenuItemExportAsVcal_Click(object sender, EventArgs e)
	{
		// Log that the Export As vCalendar menu item was clicked
		logger.Info(message: "Export As vCalendar menu item clicked. Exporting selected orbital elements to an vCalendar file.");
		// Call the ExecuteExport method to export the selected orbital elements using the VcalExporter
		ExecuteExport(exporter: new VcalExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As iCalendar menu item. Exports the selected orbital elements to an iCalendar file.</summary>
	/// <param name="sender">Event source (the Export As iCalendar menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to an iCalendar file.</remarks>
	private void ToolStripMenuItemExportAsIcs_Click(object sender, EventArgs e)
	{
		// Log that the Export As iCalendar menu item was clicked
		logger.Info(message: "Export As iCalendar menu item clicked. Exporting selected orbital elements to an iCalendar file.");
		// Call the ExecuteExport method to export the selected orbital elements using the IcsExporter
		ExecuteExport(exporter: new IcsExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As xCalendar menu item. Exports the selected orbital elements to an xCalendar file.</summary>
	/// <param name="sender">Event source (the Export As xCalendar menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to an xCalendar file.</remarks>
	private void ToolStripMenuItemExportAsXcal_Click(object sender, EventArgs e)
	{
		// Log that the Export As xCalendar menu item was clicked
		logger.Info(message: "Export As xCalendar menu item clicked. Exporting selected orbital elements to an xCalendar file.");
		// Call the ExecuteExport method to export the selected orbital elements using the XcalExporter
		ExecuteExport(exporter: new XcalExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As iCalendar menu item. Exports the selected orbital elements to an iCalendar file.</summary>
	/// <param name="sender">Event source (the Export As iCalendar menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to an iCalendar file.</remarks>
	private void ToolStripMenuItemExportAsCreole_Click(object sender, EventArgs e)
	{
		// Log that the Export As Creole menu item was clicked
		logger.Info(message: "Export As Creole menu item clicked. Exporting selected orbital elements to an Creole file.");
		// Call the ExecuteExport method to export the selected orbital elements using the Creole
		ExecuteExport(exporter: new CreoleExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As iCalendar menu item. Exports the selected orbital elements to an iCalendar file.</summary>
	/// <param name="sender">Event source (the Export As iCalendar menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to an iCalendar file.</remarks>
	private void ToolStripMenuItemExportAsBbcode_Click(object sender, EventArgs e)
	{
		// Log that the Export As BBCode menu item was clicked
		logger.Info(message: "Export As BBCode menu item clicked. Exporting selected orbital elements to an BBCode file.");
		// Call the ExecuteExport method to export the selected orbital elements using the BbcodeExporter
		ExecuteExport(exporter: new BbcodeExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As LaTeX menu item. Exports the selected orbital elements to a LaTeX file.</summary>
	/// <param name="sender">Event source (the Export As LaTeX menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to a LaTeX file.</remarks>
	private void ToolStripMenuItemExportAsLatex_Click(object sender, EventArgs e)
	{
		// Log that the Export As LaTeX menu item was clicked
		logger.Info(message: "Export As LaTeX menu item clicked. Exporting selected orbital elements to a LaTeX file.");
		// Call the ExecuteExport method to export the selected orbital elements using the LatexExporter
		ExecuteExport(exporter: new LatexExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As Markdown menu item. Exports the selected orbital elements to a Markdown file.</summary>
	/// <param name="sender">Event source (the Export As Markdown menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to a Markdown file.</remarks>
	private void ToolStripMenuItemExportAsMarkdown_Click(object sender, EventArgs e)
	{
		// Log that the Export As Markdown menu item was clicked
		logger.Info(message: "Export As Markdown menu item clicked. Exporting selected orbital elements to a Markdown file.");
		// Call the ExecuteExport method to export the selected orbital elements using the MarkdownExporter
		ExecuteExport(exporter: new MarkdownExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the Export As Word menu item. Exports the selected orbital elements to a Word document.</summary>
	/// <param name="sender">Event source (the Export As Word menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the selected orbital elements to a Word document.</remarks>
	private void ToolStripMenuItemExportAsWord_Click(object sender, EventArgs e)
	{
		// Log that the Export As Word menu item was clicked
		logger.Info(message: "Export As Word menu item clicked. Exporting selected orbital elements to a Word file.");
		// Call the ExecuteExport method to export the selected orbital elements using the WordExporter
		ExecuteExport(exporter: new WordExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for exporting data as an ODT document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method handles the export of data as an ODT document.</remarks>
	private void ToolStripMenuItemExportAsOdt_Click(object sender, EventArgs e)
	{
		// Log that the Export As ODT menu item was clicked
		logger.Info(message: "Export As ODT menu item clicked. Exporting selected orbital elements to a ODT file.");
		// Call the ExecuteExport method to export the selected orbital elements using the odtExporter
		ExecuteExport(exporter: new OdtExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event to export selected orbital element data as a Rich Text Format (RTF) document.</summary>
	/// <remarks>This method displays a save file dialog allowing the user to specify the location and name of the RTF file. It formats the selected orbital elements into RTF and writes the content to the chosen file. If no elements are selected, the output will indicate this in the document.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing the event data.</param>
	private void ToolStripMenuItemExportAsRtf_Click(object sender, EventArgs e)
	{
		// Log that the Export As RTF menu item was clicked
		logger.Info(message: "Export As RTF menu item clicked. Exporting selected orbital elements to a RTF file.");
		// Call the ExecuteExport method to export the selected orbital elements using the RtfExporter
		ExecuteExport(exporter: new RtfExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event to export selected orbital element data to an Excel file in .xlsx format.</summary>
	/// <remarks>This method displays a save file dialog allowing the user to specify the location and name of the Excel file. It generates an Excel document containing the selected orbital elements from the list, formatted as XML. If no elements are selected, the output will indicate this in the exported file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing the event data.</param>
	private void ToolStripMenuItemExportAsExcel_Click(object sender, EventArgs e)
	{
		// Log that the Export As Excel menu item was clicked
		logger.Info(message: "Export As Excel menu item clicked. Exporting selected orbital elements to a Excel file.");
		// Call the ExecuteExport method to export the selected orbital elements using the RtfExporter
		ExecuteExport(exporter: new ExcelExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for exporting data as an ODS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method creates an ODS file containing the selected orbital elements from the database.</remarks>
	private void ToolStripMenuItemExportAsOds_Click(object sender, EventArgs e)
	{
		// Log that the Export As Excel menu item was clicked
		logger.Info(message: "Export As ODS menu item clicked. Exporting selected orbital elements to a ODS file.");
		// Call the ExecuteExport method to export the selected orbital elements using the RtfExporter
		ExecuteExport(exporter: new ExcelExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsCsv control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a CSV file.</remarks>
	private void ToolStripMenuItemExportAsCsv_Click(object sender, EventArgs e)
	{
		// Log that the Export As CSV menu item was clicked
		logger.Info(message: "Export As CSV menu item clicked. Exporting selected orbital elements to a CSV file.");
		// Call the ExecuteExport method to export the selected orbital elements using the CsvExporter
		ExecuteExport(exporter: new CsvExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsTsv control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a TSV file.</remarks>
	private void ToolStripMenuItemExportAsTsv_Click(object sender, EventArgs e)
	{
		// Log that the Export As TSV menu item was clicked
		logger.Info(message: "Export As TSV menu item clicked. Exporting selected orbital elements to a TSV file.");
		// Call the ExecuteExport method to export the selected orbital elements using the TsvExporter
		ExecuteExport(exporter: new TsvExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsPsv control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a PSV file.</remarks>
	private void ToolStripMenuItemExportAsPsv_Click(object sender, EventArgs e)
	{
		// Log that the Export As PSV menu item was clicked
		logger.Info(message: "Export As PSV menu item clicked. Exporting selected orbital elements to a PSV file.");
		// Call the ExecuteExport method to export the selected orbital elements using the PsvExporter
		ExecuteExport(exporter: new PsvExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsHtml control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as an HTML file.</remarks>
	private void ToolStripMenuItemExportAsHtml_Click(object sender, EventArgs e)
	{
		// Log that the Export As HTML menu item was clicked
		logger.Info(message: "Export As HTML menu item clicked. Exporting selected orbital elements to an HTML file.");
		// Call the ExecuteExport method to export the selected orbital elements using the HtmlExporter
		ExecuteExport(exporter: new HtmlExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsXml control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as an XML file.</remarks>
	private void ToolStripMenuItemExportAsXml_Click(object sender, EventArgs e)
	{
		// Log that the Export As XML menu item was clicked
		logger.Info(message: "Export As XML menu item clicked. Exporting selected orbital elements to an XML file.");
		// Call the ExecuteExport method to export the selected orbital elements using the XmlExporter
		ExecuteExport(exporter: new XmlExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsJson control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a JSON file.</remarks>
	private void ToolStripMenuItemExportAsJson_Click(object sender, EventArgs e)
	{
		// Log that the Export As JSON menu item was clicked
		logger.Info(message: "Export As JSON menu item clicked. Exporting selected orbital elements to a JSON file.");
		// Call the ExecuteExport method to export the selected orbital elements using the JsonExporter
		ExecuteExport(exporter: new JsonExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsYaml control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a YAML file.</remarks>
	private void ToolStripMenuItemExportAsYaml_Click(object sender, EventArgs e)
	{
		// Log that the Export As YAML menu item was clicked
		logger.Info(message: "Export As YAML menu item clicked. Exporting selected orbital elements to a YAML file.");
		// Call the ExecuteExport method to export the selected orbital elements using the YamlExporter
		ExecuteExport(exporter: new YamlExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsSql control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a SQL file.</remarks>
	private void ToolStripMenuItemExportAsSql_Click(object sender, EventArgs e)
	{
		// Log that the Export As SQL menu item was clicked
		logger.Info(message: "Export As SQL menu item clicked. Exporting selected orbital elements to a SQL file.");
		// Call the ExecuteExport method to export the selected orbital elements using the SqlExporter
		ExecuteExport(exporter: new SqlExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the Click event of the ToolStripMenuItemExportAsPdf control.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method allows the user to export the selected orbital elements as a PDF file.</remarks>
	private void ToolStripMenuItemExportAsPdf_Click(object sender, EventArgs e)
	{
		// Log that the Export As PDF menu item was clicked
		logger.Info(message: "Export As PDF menu item clicked. Exporting selected orbital elements to a PDF file.");
		// Call the ExecuteExport method to export the selected orbital elements using the PdfExporter
		ExecuteExport(exporter: new PdfExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for exporting data as a PostScript file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method allows the user to export the current data as a PostScript file.</remarks>
	private void ToolStripMenuItemExportAsPostScript_Click(object sender, EventArgs e)
	{
		// Log that the Export As Postscript menu item was clicked
		logger.Info(message: "Export As Postscript menu item clicked. Exporting selected orbital elements to a Postscript file.");
		// Call the ExecuteExport method to export the selected orbital elements using the PostscriptExporter
		ExecuteExport(exporter: new PostscriptExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the "Export as EPUB" menu item.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method handles the export of database information as an EPUB file.</remarks>
	private void ToolStripMenuItemExportAsEpub_Click(object sender, EventArgs e)
	{
		// Log that the Export As Epub menu item was clicked
		logger.Info(message: "Export As Epub menu item clicked. Exporting selected orbital elements to a Epub file.");
		// Call the ExecuteExport method to export the selected orbital elements using the EpubExporter
		ExecuteExport(exporter: new EpubExporter());
		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the "Export as MOBI" menu item.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method handles the export of database information as a MOBI file.</remarks>
	private void ToolStripMenuItemExportAsMobi_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported MOBI file
		using SaveFileDialog saveFileDialogMobi = new()
		{
			Filter = "MOBI files (*.mobi)|*.mobi|All files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save database information as MOBI"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogMobi.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogMobi.FileName = $"{orbitElements[index: 0]}.{saveFileDialogMobi.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogMobi.ShowDialog(owner: this) != DialogResult.OK)
		{
			logger.Warn(message: "Export operation canceled by the user.");
			return;
		}
		// Log the file name selected by the user for exporting data
		logger.Info(message: $"Exporting data to MOBI file: {saveFileDialogMobi.FileName}");
		// Create a StringBuilder to build the content of the MOBI file
		StringBuilder sb = new();
		// Append the content to the StringBuilder in a simple text format for the MOBI file
		_ = sb.AppendLine(value: "BOOKMOBI");
		_ = sb.AppendLine(value: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}");
		_ = sb.AppendLine();
		// Iterate through the checked items in the checkedListBoxOrbitalElements and append the selected elements to the MOBI content
		bool hasSelectedElements = false;
		// Loop through the items in the checkedListBoxOrbitalElements and check if they are checked
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// If the item is checked, append the orbit element to the MOBI content
			hasSelectedElements = true;
			_ = sb.AppendLine(value: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
		}
		// If no elements were selected, add a message to the MOBI content
		if (!hasSelectedElements)
		{
			_ = sb.AppendLine(value: "No selected elements.");
		}
		// Write the content of the StringBuilder to the specified file path as a MOBI file
		File.WriteAllBytes(path: saveFileDialogMobi.FileName, bytes: Encoding.UTF8.GetBytes(s: sb.ToString()));
		// Log that the data was exported successfully
		logger.Info(message: $"Data exported successfully to MOBI file: {saveFileDialogMobi.FileName}");

		// Show a message box indicating that the data was exported successfully
		_ = KryptonMessageBox.Show(owner: this, text: "Data exported successfully.", caption: "Export Complete", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>Handles the SelectedIndexChanged event of the orbital elements checklist. Enables or disables the export buttons depending on whether any items are checked.</summary>
	/// <param name="sender">Event source (the checked list box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to enable or disable the export buttons based on the selection state of the orbital elements.</remarks>
	private void CheckedListBoxOrbitalElements_SelectedIndexChanged(object sender, EventArgs e)
	{
		// Enable or disable the export buttons based on whether any items are checked
		UpdateExportButtonState();
	}

	#endregion
}