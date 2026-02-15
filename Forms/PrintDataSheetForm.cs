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
	/// Print document used for printing data sheets.
	/// </summary>
	/// <remarks>
	/// This document is used to print the data sheets.
	/// </remarks>
	private readonly PrintDocument printDoc;

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
	}

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a custom debugger display string.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

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
			SetStatusBar(label: labelInformation, text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

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
	private void ButtonPrintPreview_Click(object sender, EventArgs e)
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
	private void ButtonPageSetup_Click(object sender, EventArgs e)
	{
		using PageSetupDialog pageSetupDialog = new();
		pageSetupDialog.Document = printDoc;
		pageSetupDialog.PageSettings = printDoc.DefaultPageSettings;
		pageSetupDialog.PrinterSettings = printDoc.PrinterSettings;
		if (pageSetupDialog.ShowDialog() == DialogResult.OK)
		{
			printDoc.DefaultPageSettings = pageSetupDialog.PageSettings;
		}
	}

	#endregion

	#region PrintPage event handlers

	/// <summary>
	/// Handles the PrintPage event of the PrintDocument.
	/// Configures the print settings and content.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="PrintPageEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when a page is printed.
	/// </remarks>
	private static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
	{
		// Check if the sender is null
		if (e.Graphics == null)
		{
			return;
		}
		// Set the text to be printed
		const string textToPrint = "This is a sample data sheet.";
		// Set the font for the text
		using Font printFont = new(familyName: "Arial", emSize: 12);
		// Set the text color to black
		e.Graphics.DrawString(s: textToPrint, font: printFont, brush: Brushes.Black, point: new PointF(x: 100, y: 100));
		// Indicate that no more pages are to be printed
		e.HasMorePages = false;
	}

	#endregion
}