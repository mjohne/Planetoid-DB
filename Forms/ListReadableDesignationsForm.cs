using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Planetoid_DB;

/// <summary>
/// Form to list readable designations from the planetoids database.
/// </summary>
/// <remarks>
/// This form is used to display a list of all readable designations from the planetoids database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ListReadableDesignationsForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for the class
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the ListReadableDesignationsForm class.
	/// </remarks>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// List of planetoid records from the database
	/// </summary>
	/// <remarks>
	/// This list contains all the planetoid records retrieved from the database.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>
	/// Number of planetoids in the database and currently selected index
	/// </summary>
	/// <remarks>
	/// This field stores the total number of planetoids in the database.
	/// </remarks>
	private int numberPlanetoids, selectedIndex;

	/// <summary>
	/// Gets a value indicating whether the operation was cancelled.
	/// </summary>
	/// <remarks>
	/// This field is used to track whether the operation was cancelled by the user.
	/// </remarks>
	private bool isCancelled;

	/// <summary>
	/// Index and label name as character strings
	/// </summary>
	/// <remarks>
	/// This field is used to store the index and label name as character strings.
	/// </remarks>
	private string strIndex, strDesignationName;

	/// <summary>
	/// Stopwatch for performance measurement
	/// </summary>
	/// <remarks>
	/// This field is used to measure the time taken for operations.
	/// </remarks>
	private readonly Stopwatch stopwatch = new();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to store the currently selected control for clipboard operations.
	/// </remarks>
	private Control currentControl;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="AppInfoForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form and its components.
	/// </remarks>
	public ListReadableDesignationsForm()
	{
		// Initialize the form components
		InitializeComponent();
		strIndex = string.Empty;
		strDesignationName = string.Empty;
	}

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a short string representation of the current instance for debugging purposes.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to set the status bar text and enable the information label when text is provided.
	/// </remarks>
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

	/// <summary>
	/// Formats a row in the list view with the current planetoids data.
	/// </summary>
	/// <param name="currentPosition">The current position in the planetoids database.</param>
	/// <remarks>
	/// This method is used to format a row in the list view with the current planetoids data.
	/// </remarks>
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
	/// <remarks>
	/// This method is used to fill the planetoids database with the provided array list.
	/// </remarks>
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
	/// <remarks>
	/// This method is used to set the maximum index for the planetoids database.
	/// </remarks>
	public void SetMaxIndex(int maxIndex) => numberPlanetoids = maxIndex;

	/// <summary>
	/// Gets the selected index in the list view.
	/// </summary>
	/// <returns>The selected index.</returns>
	/// <remarks>
	/// This method is used to get the selected index in the list view.
	/// </remarks>
	public int GetSelectedIndex() => selectedIndex;

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the ListReadableDesignationsForm loads.
	/// Initializes UI state: clears the status area, disables controls until data is available,
	/// and sets numeric up/down ranges based on the loaded planetoids database.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form's UI components and state.
	/// </remarks>
	private void ListReadableDesignationsForm_Load(object? sender, EventArgs? e)
	{
		// Clear the status bar on load
		ClearStatusBar();
		// Disable controls until data is available
		labelInformation.Enabled = listView.Visible = buttonCancel.Enabled = buttonLoad.Enabled = dropButtonSaveList.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Set numeric up/down ranges based on the planetoids database
		numericUpDownMinimum.Minimum = 1;
		numericUpDownMaximum.Minimum = 1;
		numericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		numericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		numericUpDownMinimum.Value = 1;
		numericUpDownMaximum.Value = planetoidsDatabase.Count;
	}

	/// <summary>
	/// Fired when the form is closed. Releases list view resources and disposes the form.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to release resources and perform cleanup when the form is closed.
	/// </remarks>
	private void ListReadableDesignationsForm_FormClosed(object? sender, FormClosedEventArgs? e)
	{
		listView.Dispose();
		Dispose();
	}
	#endregion

	#region BackgroundWorker event handlers

	/// <summary>
	/// Handles the <see cref="BackgroundWorker.DoWork"/> event.
	/// Processes the planetoid records in the configured numeric range on a background thread,
	/// formats each row and reports progress to the UI. The operation cooperatively cancels
	/// when the <see cref="isCancelled"/> flag is set.
	/// </summary>
	/// <param name="sender">Event source (the background worker).</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to perform the background work of processing planetoid records.
	/// </remarks>
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
	/// Handles the <see cref="BackgroundWorker.ProgressChanged"/> event.
	/// Updates the UI progress bar on the UI thread with the percentage reported by the background worker.
	/// </summary>
	/// <param name="sender">Event source (the background worker).</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data, including <see cref="ProgressChangedEventArgs.ProgressPercentage"/>.</param>
	/// <remarks>
	/// This method is used to update the UI progress bar on the UI thread with the percentage reported by the background worker.
	/// </remarks>
	private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => progressBar.Value = e.ProgressPercentage;

	/// <summary>
	/// Handles the <see cref="BackgroundWorker.RunWorkerCompleted"/> event.
	/// Finalizes background processing: re-enables UI controls, hides progress indicators and resets taskbar state.
	/// </summary>
	/// <param name="sender">Event source (the background worker).</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data, including error or cancellation information.</param>
	/// <remarks>
	/// This method is used to finalize background processing: re-enables UI controls, hides progress indicators and resets taskbar state.
	/// </remarks>
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
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is used to handle the Enter event for controls and ToolStrip items.
	/// </remarks>
	private void SetStatusBar_Enter(object sender, EventArgs e)
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
		// If we have a description, set it in the status bar
		if (description != null)
		{
			SetStatusBar(text: description);
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
	/// <remarks>
	/// This method is used to handle the Leave event for controls and ToolStrip items.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>
	/// Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and readable designation,
	/// enables the load button if necessary and stores the currently selected index.
	/// </summary>
	/// <param name="sender">Event source (expected to be the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the SelectedIndexChanged event of the ListView.
	/// </remarks>
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

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the MouseDown event for controls.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	#endregion

	#region Clicks event handlers

	/// <summary>
	/// Handles the Click event of the List button.
	/// Prepares the list view (clears columns, hides it), disables/enables the appropriate UI controls,
	/// configures the background worker for progress reporting and cancellation, and starts the background operation
	/// that formats rows for the currently configured numeric range.
	/// </summary>
	/// <param name="sender">Event source (the List button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event for the List button.
	/// </remarks>
	private void ButtonList_Click(object? sender, EventArgs? e)
	{
		// Start the stopwatch for performance measurement
		stopwatch.Restart();
		// Clear the list view
		listView.Clear();
		// Add columns to the list view
		listView.Columns.AddRange(values: [
			 columnHeaderIndex,
			 columnHeaderReadableDesignation,]);
		// Hide the list view
		listView.Visible = false;
		// Disable the numeric up-down controls
		numericUpDownMinimum.Enabled = false;
		numericUpDownMaximum.Enabled = false;
		// Enable the cancel button and disable other buttons
		buttonCancel.Enabled = true;
		buttonLoad.Enabled = false;
		buttonList.Enabled = false;
		dropButtonSaveList.Enabled = false;
		// Reset the progress bar and cancellation flag
		isCancelled = false;
		progressBar.Enabled = true;
		// Configure the background worker
		backgroundWorker.WorkerReportsProgress = true;
		backgroundWorker.WorkerSupportsCancellation = true;
		backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
		backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
		// Start the background worker to process the planetoid records
		backgroundWorker.RunWorkerAsync();
	}

	/// <summary>
	/// Handles the Click event of the Cancel button.
	/// Requests cancellation of the background worker operation by setting the internal <c>isCancelled</c> flag.
	/// </summary>
	/// <param name="sender">Event source (the Cancel button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event for the Cancel button.
	/// </remarks>
	private void ButtonCancel_Click(object? sender, EventArgs? e)
	{
		// Stop the stopwatch for performance measurement
		stopwatch.Stop();
		// Set the cancel flag to true to request cancellation
		isCancelled = true;
		// Show a message box indicating the operation was cancelled
		MessageBox.Show(text: listView.Items.Count + " objects processed in " + stopwatch.Elapsed + " hh:mm:ss.ms", caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the Save As CSV menu item.
	/// Opens a SaveFileDialog, then writes the currently displayed readable designation list to a CSV file.
	/// The file contains index and designation pairs separated by a semicolon.
	/// </summary>
	/// <param name="sender">Event source (the Save As CSV menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event for the Save As CSV menu item.
	/// </remarks>
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
	/// Opens a SaveFileDialog, then writes the currently displayed readable designation list to an HTML file.
	/// The file contains index and designation pairs formatted as HTML.
	/// </summary>
	/// <param name="sender">Event source (the Save As HTML menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event for the Save As HTML menu item.
	/// </remarks>
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
	/// Opens a SaveFileDialog, then writes the currently displayed readable designation list to an XML file.
	/// The file contains index and designation pairs formatted as XML.
	/// </summary>
	/// <param name="sender">Event source (the Save As XML menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event of the Save As XML menu item.
	/// </remarks>
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
	/// Handles the Click event of the Save As JSON menu item.
	/// Opens a SaveFileDialog, then writes the currently displayed readable designation list to a JSON file.
	/// The file contains index and designation pairs formatted as JSON.
	/// </summary>
	/// <param name="sender">Event source (the Save As JSON menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to handle the Click event of the Save As JSON menu item.
	/// </remarks>
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
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// If the <paramref name="sender"/> is a <see cref="Control"/>, its <see cref="Control.Text"/> value is copied to the clipboard.
	/// If the <paramref name="sender"/> is a <see cref="ToolStripItem"/>, its <see cref="ToolStripItem.Text"/> value is copied to the clipboard.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the text to copy based on the sender type
		string? textToCopy = sender switch
		{
			Control c => c.Text,
			ToolStripItem => currentControl?.Text,
			_ => null
		};
		// If we have text to copy, use the helper method to copy it to the clipboard
		if (!string.IsNullOrEmpty(value: textToCopy))
		{
			// Try to set the clipboard text
			try { CopyToClipboard(text: textToCopy); }
			catch
			{ // Throw an exception
				throw new ArgumentException(message: "Unsupported sender type", paramName: nameof(sender));
			}
		}
	}

	#endregion
}