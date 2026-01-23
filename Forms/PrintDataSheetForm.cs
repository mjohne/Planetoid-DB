using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Drawing.Printing;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents a form for printing data sheets.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class PrintDataSheetForm : BaseKryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// The PrintDocument instance used for printing.
		private readonly PrintDocument printDoc;

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PrintDataSheetForm"/> class.
		/// </summary>
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
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the status bar text and enables the information label when text is provided.
		/// </summary>
		/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
		/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
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

		#endregion

		#region form event handlers

		/// <summary>
		/// Handles the Load event of the form.
		/// Checks all items in the checked list box when the form loads.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void PrintDataSheetForm_Load(object sender, EventArgs e)
		{
			// Clear the status bar text
			ClearStatusBar();
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

		/// <summary>
		/// Handles the FormClosed event of the form.
		/// Disposes the form when it is closed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void PrintDataSheetForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handlers

		/// <summary>
		/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
		/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
		/// <param name="e">Event arguments.</param>
		private void SetStatusBar_Enter(object sender, EventArgs e)
		{
			// Set the status bar text based on the sender's accessible description
			switch (sender)
			{
				// If the sender is a control with an accessible description, set the status bar text
				// If the sender is a ToolStripItem with an accessible description, set the status bar text
				case Control { AccessibleDescription: not null } control:
					SetStatusBar(text: control.AccessibleDescription);
					break;
				case ToolStripItem { AccessibleDescription: not null } item:
					SetStatusBar(text: item.AccessibleDescription);
					break;
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
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click event handlers

		/// <summary>
		/// Handles the Click event of the print button.
		/// Opens a print dialog and prints the document if the user confirms.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
				Logger.Error(exception: ex, message: "Error while printing");
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
		private void ButtonCancelPrint_Click(object sender, EventArgs e) => Close();

		/// <summary>
		/// Handles the Click event of the print preview button.
		/// Opens a print preview dialog to preview the document before printing.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
			Font printFont = new(familyName: "Arial", emSize: 12);
			// Set the text color to black
			e.Graphics.DrawString(s: textToPrint, font: printFont, brush: Brushes.Black, point: new PointF(x: 100, y: 100));
			// Indicate that no more pages are to be printed
			e.HasMorePages = false;
		}

		#endregion
	}
}