using Krypton.Toolkit;

using NLog;

using System.Diagnostics;
using System.Drawing.Printing;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents a form for printing data sheets.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class PrintDataSheetForm : KryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// The PrintDocument instance used for printing.
		private readonly PrintDocument printDoc;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PrintDataSheetForm"/> class.
		/// </summary>
		public PrintDataSheetForm()
		{
			// Initialize the form components
			InitializeComponent();
			KeyDown += PrintDataSheetForm_KeyDown;
			KeyPreview = true; // Ensures the form receives key events before the controls
			printDoc = new PrintDocument();
			printDoc.PrintPage += PrintDoc_PrintPage;
		}

		#endregion

		#region Local methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		private static void ShowErrorMessage(string message) =>
			// Show an error message box with the specified message
			_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

		/// <summary>
		/// Copies the specified text to the clipboard and displays a confirmation message.
		/// </summary>
		/// <param name="text">The text to be copied.</param>
		private static void CopyToClipboard(string text)
		{
			try
			{
				// Copy the text to the clipboard
				Clipboard.SetText(text: text);
				_ = MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				// Log the exception and show an error message
				Logger.Error(exception: ex, message: ex.Message);
				// Show an error message
				ShowErrorMessage(message: $"File not found: {ex.Message}");
			}
		}

		/// <summary>
		/// Sets the status bar text.
		/// </summary>
		/// <param name="text">The main text to be displayed on the status bar.</param>
		/// <param name="additionalInfo">Additional information to be displayed alongside the main text.</param>
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
		/// Clears the status bar text.
		/// </summary>
		private void ClearStatusBar()
		{
			// Clear the status bar text and disable it
			labelInformation.Enabled = false;
			labelInformation.Text = string.Empty;
		}

		#endregion

		#region Form event handlers

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
		/// Called when the mouse pointer moves over a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
		/// Called when the mouse pointer leaves a control.
		/// Clears the status bar text.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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

		#region KeyDown event handler

		/// <summary>
		/// Handles the KeyDown event of the PrintDataSheetForm.
		/// Closes the form when the Escape key is pressed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="KeyEventArgs"/> instance that contains the event data.</param>
		private void PrintDataSheetForm_KeyDown(object? sender, KeyEventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			// Check if the Escape key is pressed
			if (e.KeyCode == Keys.Escape)
			{
				// Close the form
				Close();
			}
		}

		#endregion

		#region PrintPage event handler

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