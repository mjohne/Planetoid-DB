using NLog;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Planetoid_DB
{
	/// <summary>
	/// Form to list readable designations from the planetoids database.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class ListReadableDesignationsForm : BaseKryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// List of the database
		private List<string> planetoidsDatabase = [];

		// Number of planetoids and the selected index
		private int numberPlanetoids, selectedIndex;

		// Indicates whether the operation was aborted
		private bool isCancelled;

		// Index and label name as character strings
		private string strIndex, strDesignationName;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
		/// </summary>
		public ListReadableDesignationsForm()
		{
			// Initialize the form components
			InitializeComponent();
			strIndex = string.Empty;
			strDesignationName = string.Empty;
		}

		#endregion

		#region local methods

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

		/// <summary>
		/// Formats a row in the list view with the current planetoids data.
		/// </summary>
		/// <param name="currentPosition">The current position in the planetoids database.</param>
		private void FormatRow(int currentPosition)
		{
			// Check if the current position is within the valid range
			if (currentPosition < 0 || currentPosition >= numberPlanetoids)
			{
				// Log an error message and return
				Logger.Error(message: $"Invalid position: {currentPosition}");
				// Show an error message
				ShowErrorMessage(message: $"Invalid position: {currentPosition}");
				return;
			}
			// Format the row in the list view
			string currentData = planetoidsDatabase[index: currentPosition];
			// Extract the index from the current data
			strIndex = currentData[..7].Trim();
			// Extract the designation name from the current data
			strDesignationName = currentData.Substring(startIndex: 166, length: 28).Trim();
			// Add the formatted row to the list view
			ListViewItem listViewItem = new(text: strIndex)
			{
				// Set the tooltip text to show both the index and the designation name
				ToolTipText = $"{strIndex}: {strDesignationName}"
			};
			// Add the designation name as a subitem
			_ = listViewItem.SubItems.Add(text: strDesignationName);
			// Add the list view item to the list view
			_ = listView.Items.Add(value: listViewItem);
		}

		/// <summary>
		/// Fills the planetoids database with the provided array list.
		/// </summary>
		/// <param name="arrTemp">The array list to fill the database with.</param>
		public void FillArray(ArrayList arrTemp)
		{
			// Fill the planetoids database with the provided array list
			planetoidsDatabase = [.. arrTemp.Cast<string>()];
			// Set the number of planetoids
			numberPlanetoids = planetoidsDatabase.Count;
		}

		/// <summary>
		/// Sets the maximum index for the planetoids database.
		/// </summary>
		/// <param name="maxIndex">The maximum index.</param>
		public void SetMaxIndex(int maxIndex) => numberPlanetoids = maxIndex;

		/// <summary>
		/// Gets the selected index in the list view.
		/// </summary>
		/// <returns>The selected index.</returns>
		public int GetSelectedIndex() => selectedIndex;

		#endregion

		#region Form event handlers

		/// <summary>
		/// Handles the Load event of the form.
		/// Initializes the form controls based on the planetoids database.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ListReadableDesignationsForm_Load(object? sender, EventArgs? e)
		{
			ClearStatusBar();
			labelInformation.Enabled = listView.Visible = buttonCancel.Enabled = buttonLoad.Enabled = dropButtonSaveList.Enabled = false;
			if (planetoidsDatabase.Count <= 0)
			{
				return;
			}

			numericUpDownMinimum.Minimum = 1;
			numericUpDownMaximum.Minimum = 1;
			numericUpDownMinimum.Maximum = planetoidsDatabase.Count;
			numericUpDownMaximum.Maximum = planetoidsDatabase.Count;
			numericUpDownMinimum.Value = 1;
			numericUpDownMaximum.Value = planetoidsDatabase.Count;
		}

		/// <summary>
		/// Handles the FormClosed event of the form.
		/// Disposes the list view and the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void ListReadableDesignationsForm_FormClosed(object? sender, FormClosedEventArgs? e)
		{
			listView.Dispose();
			Dispose();
		}

		#endregion

		#region BackgroundWorker

		/// <summary>
		/// Handles the DoWork event of the background worker.
		/// Formats rows in the list view based on the planetoids database.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
		private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs? e)
		{
			// Set the maximum value of the progress bar
			progressBar.Maximum = (int)numericUpDownMaximum.Value - 1;
			for (int i = (int)numericUpDownMinimum.Value - 1; i < (int)numericUpDownMaximum.Value; i++)
			{
				// Format the row in the list view
				FormatRow(currentPosition: i);
				// Report progress to the UI thread
				backgroundWorker.ReportProgress(percentProgress: i);
				// Update the taskbar progress
				TaskbarProgress.SetValue(windowHandle: Handle, progressValue: i, progressMax: (int)numericUpDownMaximum.Value);
				if (isCancelled)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Handles the RunWorkerCompleted event of the background worker.
		/// Updates the form controls after the background worker completes its task.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
		private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => progressBar.Value = e.ProgressPercentage;

		/// <summary>
		/// Called when the mouse pointer moves over a control.
		/// Sets the status bar text based on the control's accessible description.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs? e)
		{
			listView.Visible = true; // Show the list view
									 // Enable the numeric up-down controls
			numericUpDownMinimum.Enabled = true;
			numericUpDownMaximum.Enabled = true;
			buttonList.Enabled = true; // Enable the list button
			dropButtonSaveList.Enabled = true; // Enable the save button
			buttonCancel.Enabled = false; // Disable the cancel button
			buttonLoad.Enabled = false; // Disable the load button
			progressBar.Enabled = false; // Disable the progress bar
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100); // Reset the taskbar progress
		}

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
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region SelectedIndexChanged event handlers

		/// <summary>
		/// Handles the SelectedIndexChanged event of the list view.
		/// Updates the status bar and enables the load button based on the selected index.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void SelectedIndexChanged(object? sender, EventArgs? e)
		{
			if (listView.SelectedIndices.Count <= 0)
			{
				return;
			}
			// Get the selected index from the list view
			int listViewSelectedIndex = listView.SelectedIndices[index: 0];
			if (listViewSelectedIndex >= 0)
			{
				// Set the status bar text to show the selected index and designation name
				SetStatusBar(text: $"{I10nStrings.Index}: {listView.Items[index: listViewSelectedIndex].Text} - {listView.Items[index: listViewSelectedIndex].SubItems[index: 1].Text}");
			}
			if (!buttonLoad.Enabled)
			{
				// Enable the load button if it is not already enabled
				buttonLoad.Enabled = true;
			}
			// Set the selected index to the current index
			this.selectedIndex = listViewSelectedIndex;
		}

		#endregion

		#region Clicks event handlers

		/// <summary>
		/// Handles the Click event of the List button.
		/// Initializes the list view and starts the background worker to format rows.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonList_Click(object? sender, EventArgs? e)
		{
			// Clear the list view
			listView.Clear();
			// Add columns to the list view
			listView.Columns.AddRange(values: [
				 columnHeaderIndex,
				 columnHeaderReadableDesignation,]);
			listView.Visible = false; // Hide the list view
									  // Disable the numeric up-down controls
			numericUpDownMinimum.Enabled = false;
			numericUpDownMaximum.Enabled = false;
			buttonCancel.Enabled = true; // Enable the cancel button
			buttonLoad.Enabled = false; // Disable the load button
			buttonList.Enabled = false; // Disable the list button
			dropButtonSaveList.Enabled = false; // Disable the save button
			isCancelled = false; // Set the cancel flag to false
			progressBar.Enabled = true; // Set the progress bar to enabled
			backgroundWorker.WorkerReportsProgress = true; // Set the worker to report progress
			backgroundWorker.WorkerSupportsCancellation = true; // Set the worker to support cancellation
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged; // Handle progress changes
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted; // Handle completion
			backgroundWorker.RunWorkerAsync(); // Start the background worker
		}

		/// <summary>
		/// Handles the Click event of the Cancel button.
		/// Cancels the background worker operation.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonCancel_Click(object? sender, EventArgs? e) => isCancelled = true;

		/// <summary>
		/// Handles the Click event of the Save As CSV menu item.
		/// Saves the list view data as a CSV file.
		/// </summary>
		private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		{
			// Set the initial directory for the save file dialog
			saveFileDialogCsv.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the default file name for the CSV file
			saveFileDialogCsv.FileName = $"Readable-Designation-List_{numericUpDownMinimum.Value}-{numericUpDownMaximum.Value}.{saveFileDialogCsv.DefaultExt}";
			// Show the save file dialog to select the CSV file location
			if (saveFileDialogCsv.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new CSV file and write the data to it
			using StreamWriter streamWriter = new(path: saveFileDialogCsv.FileName);
			// Write the header line
			for (int i = (int)numericUpDownMinimum.Value - 1; i < listView.Items.Count; i++)
			{
				// Write the index and designation name to the CSV file
				streamWriter.Write(value: $"{listView.Items[index: i].SubItems[index: 0].Text}; {listView.Items[index: i].SubItems[index: 1].Text}");
				// If this is not the last item, write a new line
				// to separate the items
				if (i < listView.Items.Count - 1)
				{
					// Write a new line to separate the items
					streamWriter.Write(value: Environment.NewLine);
				}
			}
		}

		/// <summary>
		/// Handles the Click event of the Save As HTML menu item.
		/// Saves the list view data as an HTML file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		{
			// Set the initial directory for the save file dialog
			saveFileDialogHtml.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the default file name for the HTML file
			saveFileDialogHtml.FileName = $"Readable-Designation-List_{numericUpDownMinimum.Value}-{numericUpDownMaximum.Value}.{saveFileDialogHtml.DefaultExt}";
			// Show the save file dialog to select the HTML file location
			if (saveFileDialogHtml.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new HTML file and write the data to it
			using StreamWriter streamWriter = new(path: saveFileDialogHtml.FileName);
			// Write the HTML header and metadata
			streamWriter.WriteLine(value: "<!DOCTYPE html>");
			streamWriter.WriteLine(value: "<html lang=\"en\">");
			streamWriter.WriteLine(value: "\t<head>");
			streamWriter.WriteLine(value: "\t\t<meta charset=\"utf-8\">");
			streamWriter.WriteLine(value: "\t\t<meta name=\"description\" content=\"\">");
			streamWriter.WriteLine(value: "\t\t<meta name=\"keywords\" content=\"\">");
			streamWriter.WriteLine(value: "\t\t<meta name=\"generator\" content=\"Planetoid-DB\">");
			streamWriter.WriteLine(value: "\t\t<title>List of readable designations</title>");
			streamWriter.WriteLine(value: "\t\t<style>");
			streamWriter.WriteLine(value: "\t\t\t* {font-family: sans-serif;}");
			streamWriter.WriteLine(value: "\t\t\t.italic {font-style: italic;}");
			streamWriter.WriteLine(value: "\t\t\t.bold {font-weight: bold;}");
			streamWriter.WriteLine(value: "\t\t\t.sup {vertical-align: super; font-size: smaller;}");
			streamWriter.WriteLine(value: "\t\t\t.sub {vertical-align: sub; font-size: smaller;}");
			streamWriter.WriteLine(value: "\t\t\t.block {width:50px; display: inline-block;}");
			streamWriter.WriteLine(value: "\t\t</style>");
			streamWriter.WriteLine(value: "\t</head>");
			streamWriter.WriteLine(value: "\t<body>");
			streamWriter.WriteLine(value: "\t\t<p>");
			for (int i = (int)numericUpDownMinimum.Value - 1; i < listView.Items.Count; i++)
			{
				// Write the index and designation name to the HTML file
				streamWriter.Write(value: $"\t\t\t<span class=\"bold block\" xml:id=\"element-id-{i}\">{listView.Items[index: i].SubItems[index: 0].Text}:</span> <span xml:id=\"value-id-{i}\">{listView.Items[index: i].SubItems[index: 1].Text}</span>");
				// If this is not the last item, write a line break
				// to separate the items
				if (i < listView.Items.Count - 1)
				{
					// Write a line break to separate the items
					streamWriter.WriteLine(value: "<br />");
				}
			}
			// Write the closing tags for the paragraph and body
			streamWriter.WriteLine(value: "\t\t</p>");
			streamWriter.WriteLine(value: "\t</body>");
			streamWriter.Write(value: "</html>");
		}

		/// <summary>
		/// Handles the Click event of the Save As XML menu item.
		/// Saves the list view data as an XML file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		{
			// Set the initial directory for the save file dialog
			saveFileDialogXml.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the default file name for the XML file
			saveFileDialogXml.FileName = $"Readable-Designation-List_{numericUpDownMinimum.Value}-{numericUpDownMaximum.Value}.{saveFileDialogXml.DefaultExt}";
			// Show the save file dialog to select the XML file location
			if (saveFileDialogXml.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new XML file and write the data to it
			using StreamWriter streamWriter = new(path: saveFileDialogXml.FileName);
			// Write the XML header and root element
			streamWriter.WriteLine(value: "<?xml version=\"1.0\" encoding=\"UTF.8\" standalone=\"yes\"?>");
			streamWriter.WriteLine(value: "<ListReadableDesignations xmlns=\"https://planet-db.de\">");
			for (int i = (int)numericUpDownMinimum.Value - 1; i < listView.Items.Count; i++)
			{
				// Write the index and designation name to the XML file
				streamWriter.Write(value: $"\t<item xml:id=\"element-id-{i}\" index=\"{listView.Items[index: i].SubItems[index: 0].Text}\" name=\"{listView.Items[index: i].SubItems[index: 1].Text}\" />");
				// If this is not the last item, write a new line
				// to separate the items
				if (i < listView.Items.Count - 1)
				{
					// Write a new line to separate the items
					streamWriter.Write(value: Environment.NewLine);
				}
			}
			// Write the closing tag for the root element
			streamWriter.Write(value: "</ListReadableDesignations>");
		}

		/// <summary>
		/// Handles the Click event of the Save As Json menu item.
		/// Saves the list view data as a Json file.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		{
			// Set the initial directory for the save file dialog
			saveFileDialogJson.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the default file name for the JSON file
			saveFileDialogJson.FileName = $"Readable-Designation-List_{numericUpDownMinimum.Value}-{numericUpDownMaximum.Value}.{saveFileDialogJson.DefaultExt}";
			// Show the save file dialog to select the JSON file location
			if (saveFileDialogJson.ShowDialog() == DialogResult.OK)
			{
				// Create a new JSON file and write the data to it
				using StreamWriter streamWriter = new(path: saveFileDialogJson.FileName);
				// Write the JSON header and root element
				streamWriter.WriteLine(value: "{");
				for (int i = (int)numericUpDownMinimum.Value - 1; i < listView.Items.Count; i++)
				{
					// Write the index and designation name to the JSON file
					streamWriter.WriteLine(value: "\t\"item\"");
					streamWriter.WriteLine(value: "\t{");
					streamWriter.WriteLine(value: $"\t\t\"index\": \"{listView.Items[index: i].SubItems[index: 0].Text}\",");
					streamWriter.WriteLine(value: $"\t\t\"readable designations\": \"{listView.Items[index: i].SubItems[index: 1].Text}\"");
					streamWriter.WriteLine(value: "\t}");
				}
				// Write the closing tag for the root element
				streamWriter.Write(value: "}");
			}
		}

		#endregion

		#region DoubleClick event handlers

		/// <summary>
		/// Called when a control is double-clicked to copy the text to the clipboard.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
		}

		#endregion
	}
}
