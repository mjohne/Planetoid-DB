// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Drawing.Printing;

namespace Planetoid_DB;

/// <summary>
/// Represents a form for printing data sheets.
/// </summary>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class PrintDataSheetForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger for logging messages and errors.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors that occur within the form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Print document used for printing data sheets.
	/// </summary>
	/// <remarks>
	/// This document is used to print the data sheets.
	/// </remarks>
	private readonly PrintDocument printDoc;

	/// <summary>
	/// List of orbit elements to be exported.
	/// </summary>
	/// <remarks>
	/// This list contains the values of the orbital elements to be printed.
	/// </remarks>
	private List<string> orbitElements = [];

	/// <summary>
	/// The index of the last printed item.
	/// </summary>
	private int lastPrintedIndex = 0;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="PrintDataSheetForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components and sets up the print document.
	/// </remarks>
	public PrintDataSheetForm()
	{
		// Initialize the form components
		InitializeComponent();
		printDoc = new PrintDocument();
		printDoc.PrintPage += PrintDoc_PrintPage;
		printDoc.BeginPrint += PrintDoc_BeginPrint;
	}

	#endregion

	#region helper methods

	/// <summary>
	/// Sets the database of orbital elements.
	/// </summary>
	/// <param name="db">The list of orbital elements.</param>
	public void SetDatabase(List<string> db) => orbitElements = db;

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a custom debugger display string.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Checks or unchecks all items in the orbital elements checklist.
	/// </summary>
	/// <param name="check">If true, all items are checked; if false, all items are unchecked.</param>
	/// <remarks>
	/// This method iterates through all items in the orbital elements checklist
	/// and sets their checked state based on the provided <paramref name="check"/> value.
	/// </remarks>
	private void CheckIt(bool check)
	{
		// Check or uncheck all items in the checked list box
		// based on the provided boolean value
		// and enable or disable the export buttons accordingly
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check or uncheck the item at index i
			checkedListBoxOrbitalElements.SetItemChecked(index: i, value: check);
		}
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the form.
	/// Checks all items in the checked list box when the form loads.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form loads.
	/// </remarks>
	private void PrintDataSheetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
		// Check if the checked list box has items
		if (checkedListBoxOrbitalElements.Items.Count == 0)
		{
			return;
		}
		// Check all items in the checked list box
		// Iterate through all items in the checked list box
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Set the item checked state to true
			checkedListBoxOrbitalElements.SetItemChecked(index: i, value: true);
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the print button.
	/// Opens a print dialog and prints the document if the user confirms.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the print button is clicked.
	/// </remarks>
	private void ButtonPrintDataSheet_Click(object sender, EventArgs e)
	{
		// Create a new PrintDialog instance
		using PrintDialog dialogPrint = new();
		// Set the printer settings for the print document
		dialogPrint.Document = printDoc;
		dialogPrint.AllowSelection = true;
		dialogPrint.AllowSomePages = true;
		if (dialogPrint.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Try to print the document
		try
		{
			// Print the document
			printDoc.Print();
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: "Error while printing");
			ShowErrorMessage(message: $"Error while printing: {ex.Message}");
		}
		// Close the form after printing
		Close();
	}

	/// <summary>
	/// Handles the Click event of the cancel button.
	/// Closes the form without printing.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the cancel button is clicked.
	/// </remarks>
	private void ButtonCancelPrint_Click(object sender, EventArgs e) => Close();

	/// <summary>
	/// Handles the Click event of the print preview button.
	/// Opens a print preview dialog to preview the document before printing.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the print preview button is clicked.
	/// </remarks>
	private void ToolStripButtonPrintPreview_Click(object sender, EventArgs e)
	{
		using PrintPreviewDialog previewDialog = new();
		previewDialog.Document = printDoc;
		_ = previewDialog.ShowDialog();
	}

	/// <summary>
	/// Handles the Click event of the page setup button.
	/// Opens a page setup dialog to configure page settings.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the page setup button is clicked.
	/// </remarks>
	private void ToolStripButtonPageSetup_Click(object sender, EventArgs e)
	{
		using PageSetupDialog pageSetupDialog = new();
		pageSetupDialog.Document = printDoc;
		pageSetupDialog.PageSettings = printDoc.DefaultPageSettings;
		pageSetupDialog.PrinterSettings = printDoc.PrinterSettings;
		pageSetupDialog.ShowNetwork = true;
		if (pageSetupDialog.ShowDialog() == DialogResult.OK)
		{
			printDoc.DefaultPageSettings = pageSetupDialog.PageSettings;
		}
	}

	/// <summary>
	/// Handles the click event for the 'Mark All' button, marking all items as checked.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to mark all items in the orbital elements checklist.
	/// </remarks>
	private void ToolStripButtonMarkAll_Click(object sender, EventArgs e) => CheckIt(check: true);

	/// <summary>
	/// Handles the click event for the 'Unmark All' button, resetting all items to an unchecked state.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>
	/// This method is used to unmark all items in the orbital elements checklist.
	/// </remarks>
	private void ToolStripButtonUnmarkAll_Click(object sender, EventArgs e) => CheckIt(check: false);

	#endregion

	#region PrintPage event handlers

	/// <summary>
	/// Handles the BeginPrint event of the PrintDocument.
	/// Resets the index for pagination.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="PrintEventArgs"/> instance that contains the event data.</param>
	private void PrintDoc_BeginPrint(object sender, PrintEventArgs e) => lastPrintedIndex = 0;

	/// <summary>
	/// Handles the PrintPage event of the PrintDocument.
	/// Configures the print settings and content.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="PrintPageEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when a page is printed.
	/// </remarks>
	private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
	{
		// space between every entry
		int space = 2;

		// Check if the sender is null
		if (e.Graphics is null)
		{
			return;
		}

		// Set the fonts for the text
		using Font fontLabel = new(familyName: "Segoe UI", emSize: 10, style: FontStyle.Bold);
		using Font fontValue = new(familyName: "Segoe UI", emSize: 10, style: FontStyle.Regular);
		using Pen penSeparator = new(color: Color.LightGray);

		float lineHeight = fontLabel.GetHeight(graphics: e.Graphics) + space;
		float linesPerPage = e.MarginBounds.Height / lineHeight;
		float yPos;
		int count = 0;
		float leftMargin = e.MarginBounds.Left;
		float topMargin = e.MarginBounds.Top;
		float valueColumnX = leftMargin + 350;

		// Iterate over the orbital elements
		while (count < linesPerPage && lastPrintedIndex < checkedListBoxOrbitalElements.Items.Count)
		{
			// Check if the item is checked
			if (checkedListBoxOrbitalElements.GetItemChecked(index: lastPrintedIndex))
			{
				string label = checkedListBoxOrbitalElements.Items[index: lastPrintedIndex].ToString() ?? string.Empty;
				string value = lastPrintedIndex < orbitElements.Count ? orbitElements[index: lastPrintedIndex] : string.Empty;

				yPos = topMargin + (count * lineHeight);

				// Draw Label
				e.Graphics.DrawString(s: label + ":", font: fontLabel, brush: Brushes.Black, x: leftMargin, y: yPos);

				// Determine X position for value (align to column, or push if label is too long)
				float labelWidth = e.Graphics.MeasureString(text: label + ":", font: fontLabel).Width;
				float xPosValue = Math.Max(val1: valueColumnX, val2: leftMargin + labelWidth + 10);

				// Draw Value
				e.Graphics.DrawString(s: value, font: fontValue, brush: Brushes.DarkSlateGray, x: xPosValue, y: yPos);

				// Draw separator line
				e.Graphics.DrawLine(pen: penSeparator, x1: leftMargin, y1: yPos + lineHeight - 2, x2: e.MarginBounds.Right, y2: yPos + lineHeight - 2);

				count++;
			}
			lastPrintedIndex++;
		}

		// If more lines exist, print another page
		e.HasMorePages = lastPrintedIndex < checkedListBoxOrbitalElements.Items.Count;
	}

	#endregion
}