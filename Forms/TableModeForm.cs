using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Represents the form for displaying planetoids data in table mode.
/// </summary>
/// <remarks>
/// This form provides a user interface for viewing and managing planetoids data in a tabular format.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class TableModeForm : BaseKryptonForm
{
	/// <summary>
	/// List of planetoid records from the database
	/// </summary>
	/// <remarks>
	/// This field stores the list of planetoid records retrieved from the database.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>
	/// Number of planetoids in the database.
	/// </summary>
	/// <remarks>
	/// This field stores the number of planetoids retrieved from the database.
	/// </remarks>
	private int numberPlanetoids;

	/// <summary>
	/// Indicates whether the operation is cancelled.
	/// </summary>
	/// <remarks>
	/// This field is used to track the cancellation state of the operation.
	/// </remarks>
	private bool isCancelled;

	/// <summary>
	/// The index of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the index of the planetoids.
	/// </remarks>
	private string strIndex = string.Empty;

	/// <summary>
	/// The absolute magnitude of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the absolute magnitude of the planetoids.
	/// </remarks>
	private string strMagAbs = string.Empty;

	/// <summary>
	/// The slope parameter of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the slope parameter of the planetoids.
	/// </remarks>
	private string strSlopeParam = string.Empty;

	/// <summary>
	/// The epoch of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the epoch of the planetoids.
	/// </remarks>
	private string strEpoch = string.Empty;

	/// <summary>
	/// The mean anomaly of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the mean anomaly of the planetoids.
	/// </remarks>
	private string strMeanAnomaly = string.Empty;

	/// <summary>
	/// The argument of perihelion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the argument of perihelion of the planetoids.
	/// </remarks>
	private string strArgPeri = string.Empty;

	/// <summary>
	/// The longitude of the ascending node of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the longitude of the ascending node of the planetoids.
	/// </remarks>
	private string strLongAscNode = string.Empty;

	/// <summary>
	/// The inclination of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the inclination of the planetoids.
	/// </remarks>
	private string strIncl = string.Empty;

	/// <summary>
	/// The orbital eccentricity of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the orbital eccentricity of the planetoids.
	/// </remarks>
	private string strOrbEcc = string.Empty;

	/// <summary>
	/// The mean daily motion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the mean daily motion of the planetoids.
	/// </remarks>
	private string strMotion = string.Empty;

	/// <summary>
	/// The semi-major axis of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the semi-major axis of the planetoids.
	/// </remarks>
	private string strSemiMajorAxis = string.Empty;

	/// <summary>
	/// The reference for the planetoids data.
	/// </summary>
	/// <remarks>
	/// This field stores the reference for the planetoids data.
	/// </remarks>
	private string strRef = string.Empty;

	/// <summary>
	/// The number of observations of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the number of observations of the planetoids.
	/// </remarks>
	private string strNumberObservation = string.Empty;

	/// <summary>
	/// The number of oppositions of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the number of oppositions of the planetoids.
	/// </remarks>
	private string strNumberOpposition = string.Empty;

	/// <summary>
	/// The observation span of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the observation span of the planetoids.
	/// </remarks>
	private string strObsSpan = string.Empty;

	/// <summary>
	/// The RMS residual of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the RMS residual of the planetoids.
	/// </remarks>
	private string strRmsResidual = string.Empty;

	/// <summary>
	/// The name of the computer that processed the planetoids data.
	/// </summary>
	/// <remarks>
	/// This field stores the name of the computer that processed the planetoids data.
	/// </remarks>
	private string strComputerName = string.Empty;

	/// <summary>
	/// The flags associated with the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the flags associated with the planetoids.
	/// </remarks>
	private string strFlags = string.Empty;

	/// <summary>
	/// The designation name of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the designation name of the planetoids.
	/// </remarks>
	private string strDesignationName = string.Empty;

	/// <summary>
	/// The date of the last observation of the planetoids.
	/// </summary>
	/// <remarks>
	/// This field stores the date of the last observation of the planetoids.
	/// </remarks>
	private string strObservationLastDate = string.Empty;

	/// <summary>
	/// Stopwatch for performance measurement
	/// </summary>
	/// <remarks>
	/// This field stores the stopwatch for performance measurement.
	/// </remarks>
	private readonly Stopwatch stopwatch = new();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field stores the currently selected control for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="TableModeForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public TableModeForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is called to obtain a string representation of the current instance.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is called to set the status bar text and enable the information label.
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
	/// Fills the internal planetoids database from the provided <see cref="ArrayList"/>.
	/// </summary>
	/// <param name="arrTemp">An <see cref="ArrayList"/> containing planetoid records as strings. Each entry is cast to <see cref="string"/> and appended to the internal database.</param>
	/// <remarks>
	/// The method casts the elements of <paramref name="arrTemp"/> to <see cref="string"/>, stores them
	/// in the internal <see cref="planetoidsDatabase"/> list and updates <see cref="numberPlanetoids"/>.
	/// The caller is responsible for providing data in the expected string format.
	/// </remarks>
	public void FillArray(ArrayList arrTemp)
	{
		planetoidsDatabase = [.. arrTemp.Cast<string>()];
		numberPlanetoids = planetoidsDatabase.Count;
	}

	/// <summary>
	/// Formats a single planetoid record and adds it as a <see cref="ListViewItem"/> to the list view.
	/// Extracts fixed-width fields from the record at <paramref name="currentPosition"/>,
	/// trims them and appends the resulting subitems to the list view.
	/// </summary>
	/// <param name="currentPosition">Zero-based index of the record in the internal <see cref="planetoidsDatabase"/> list to format.</param>
	/// <remarks>
	/// Caller must ensure <paramref name="currentPosition"/> is within bounds. The method expects
	/// a fixed-width record layout and may throw <see cref="ArgumentOutOfRangeException"/> if the
	/// record is malformed.
	/// </remarks>
	private void FormatRow(int currentPosition)
	{
		// ReSharper disable once IdentifierTypo
		string planetoid = planetoidsDatabase[index: currentPosition];
		// Check if the planetoids is not null
		// and the length is greater than 0
		if (string.IsNullOrEmpty(value: planetoid))
		{
			return;
		}

		// Extract planetoids data from the string
		// and trim the values to remove leading and trailing whitespace
		strIndex = planetoid[..7].Trim();
		strMagAbs = planetoid.Substring(startIndex: 8, length: 5).Trim();
		strSlopeParam = planetoid.Substring(startIndex: 14, length: 5).Trim();
		strEpoch = planetoid.Substring(startIndex: 20, length: 5).Trim();
		strMeanAnomaly = planetoid.Substring(startIndex: 26, length: 9).Trim();
		strArgPeri = planetoid.Substring(startIndex: 37, length: 9).Trim();
		strLongAscNode = planetoid.Substring(startIndex: 48, length: 9).Trim();
		strIncl = planetoid.Substring(startIndex: 59, length: 9).Trim();
		strOrbEcc = planetoid.Substring(startIndex: 70, length: 9).Trim();
		strMotion = planetoid.Substring(startIndex: 80, length: 11).Trim();
		strSemiMajorAxis = planetoid.Substring(startIndex: 92, length: 11).Trim();
		strRef = planetoid.Substring(startIndex: 107, length: 9).Trim();
		strNumberObservation = planetoid.Substring(startIndex: 117, length: 5).Trim();
		strNumberOpposition = planetoid.Substring(startIndex: 123, length: 3).Trim();
		strObsSpan = planetoid.Substring(startIndex: 127, length: 9).Trim();
		strRmsResidual = planetoid.Substring(startIndex: 137, length: 4).Trim();
		strComputerName = planetoid.Substring(startIndex: 150, length: 10).Trim();
		strFlags = planetoid.Substring(startIndex: 161, length: 4).Trim();
		strDesignationName = planetoid.Substring(startIndex: 166, length: 28).Trim();
		strObservationLastDate = planetoid.Substring(startIndex: 194, length: 8).Trim();
		// Add the planetoids data to the list view
		// and set the tooltip text for the list view item
		ListViewItem listViewItem = new(text: strIndex)
		{
			// Set the tooltip text for the list view item
			// to display the index and designation name
			// when the mouse hovers over it
			ToolTipText = $"{strIndex}: {strDesignationName}"
		};
		// Add the planetoids data as subitems to the list view item
		_ = listViewItem.SubItems.Add(text: strDesignationName); // Add the designation name
		_ = listViewItem.SubItems.Add(text: strEpoch); // Add the epoch
		_ = listViewItem.SubItems.Add(text: strMeanAnomaly); // Add the mean anomaly
		_ = listViewItem.SubItems.Add(text: strArgPeri); // Add the argument of perihelion
		_ = listViewItem.SubItems.Add(text: strLongAscNode); // Add the longitude of ascending node
		_ = listViewItem.SubItems.Add(text: strIncl); // Add the inclination
		_ = listViewItem.SubItems.Add(text: strOrbEcc); // Add the orbital eccentricity
		_ = listViewItem.SubItems.Add(text: strMotion); // Add the mean daily motion
		_ = listViewItem.SubItems.Add(text: strSemiMajorAxis); // Add the semi-major axis
		_ = listViewItem.SubItems.Add(text: strMagAbs); // Add the absolute magnitude
		_ = listViewItem.SubItems.Add(text: strSlopeParam); // Add the slope parameter
		_ = listViewItem.SubItems.Add(text: strRef); // Add the reference
		_ = listViewItem.SubItems.Add(text: strNumberOpposition); // Add the number of oppositions
		_ = listViewItem.SubItems.Add(text: strNumberObservation); // Add the number of observations
		_ = listViewItem.SubItems.Add(text: strObsSpan); // Add the observation span
		_ = listViewItem.SubItems.Add(text: strRmsResidual); // Add the RMS residual
		_ = listViewItem.SubItems.Add(text: strComputerName); // Add the computer name
		_ = listViewItem.SubItems.Add(text: strFlags); // Add the flags
		_ = listViewItem.SubItems.Add(text: strObservationLastDate); // Add the date of last observation
																	 // Add the list view item to the list view
		_ = listView.Items.Add(value: listViewItem);
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the form Load event.
	/// Initializes UI controls, clears the status area and sets up numeric ranges based on the loaded database.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form is loaded.
	/// </remarks>
	private void TableModeForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar text
		ClearStatusBar();
		// Disable the status bar, the list view and the cancel button
		labelInformation.Enabled = listView.Visible = buttonCancel.Enabled = false;
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Set the minimum and maximum values for the numeric up-down controls
		numericUpDownMinimum.Minimum = 1;
		numericUpDownMaximum.Minimum = 1;
		numericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		numericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		numericUpDownMinimum.Value = 1;
		numericUpDownMaximum.Value = planetoidsDatabase.Count;
	}

	#endregion

	#region BackgroundWorker

	/// <summary>
	/// Handles the <see cref="BackgroundWorker.DoWork"/> event.
	/// Processes the planetoid records in the configured range on a background thread,
	/// formats each row and reports progress to the UI. The operation checks the
	/// <see cref="isCancelled"/> flag to support cooperative cancellation.
	/// </summary>
	/// <param name="sender">Event source (the background worker).</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	Background processing for the table mode form.
	/// </remarks>
	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		// Set the maximum value of the progress bar
		progressBar.Maximum = (int)numericUpDownMaximum.Value - 1;
		for (int i = (int)numericUpDownMinimum.Value - 1; i < (int)numericUpDownMaximum.Value; i++)
		{
			// Format the row
			FormatRow(currentPosition: i);
			// Report progress to the UI thread
			backgroundWorker.ReportProgress(percentProgress: i);
			// Update the taskbar progress
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: i, progressMax: (int)numericUpDownMaximum.Value);
			// Check if the operation is cancelled
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
	/// This method is called when the background worker reports progress.
	/// </remarks>
	private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => progressBar.Value = e.ProgressPercentage;

	/// <summary>
	/// Handles the <see cref="BackgroundWorker.RunWorkerCompleted"/> event.
	/// Finalizes background processing: re-enables UI controls, hides progress indicators and resets taskbar state.
	/// </summary>
	/// <param name="sender">Event source (the background worker).</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data, including error or cancellation information.</param>
	/// <remarks>
	/// This method is called when the background worker completes its work.
	/// </remarks>
	private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
	{
		// Show the list view
		listView.Visible = true;
		// Enable the numeric up-down controls
		numericUpDownMinimum.Enabled = true;
		numericUpDownMaximum.Enabled = true;
		// Enable the list button
		buttonList.Enabled = true;
		// Disable the cancel button
		buttonCancel.Enabled = false;
		// Disable the progress bar
		progressBar.Enabled = false;
		// Reset the taskbar progress
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
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
		// If a description is available, set it in the status bar
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
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>
	/// Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and designation name.
	/// If no item is selected the method returns without modifying the UI.
	/// </summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the selected index of the list view changes.
	/// </remarks>
	private void ListViewTableMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		// Cast the sender to a ListView
		if (sender is not ListView listView)
		{
			return;
		}
		// Check if there are any selected indices
		if (listView.SelectedIndices.Count <= 0)
		{
			return;
		}
		// Get the selected index from the list view
		int selectedIndex = listView.SelectedIndices[index: 0];
		if (selectedIndex >= 0)
		{
			// Set the status bar text to the selected planetoids index and designation name
			SetStatusBar(text: $"{I10nStrings.Index}: {listView.Items[index: selectedIndex].Text} - {listView.Items[index: selectedIndex].SubItems[index: 1].Text}");
		}
	}
	#endregion

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	/// This method is called when the mouse button is pressed down on a control.
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
	/// Prepares the list view, disables/enables the appropriate UI controls, enables progress reporting
	/// and starts the background worker to process planetoid records in the configured range.
	/// </summary>
	/// <param name="sender">Event source (the List button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the List button is clicked.
	/// </remarks>
	private void ButtonList_Click(object sender, EventArgs e)
	{
		// Start the stopwatch for performance measurement
		stopwatch.Restart();
		// Clear the list view
		listView.Clear();
		// Add columns to the list view
		listView.Columns.AddRange(values: [
			 columnHeaderIndex,
			 columnHeaderReadableDesignation,
			 columnHeaderEpoch,
			 columnHeaderMeanAnomaly,
			 columnHeaderArgumentPerihelion,
			 columnHeaderLongitudeAscendingNode,
			 columnHeaderInclination,
			 columnHeaderOrbitalEccentricity,
			 columnHeaderMeanDailyMotion,
			 columnHeaderSemimajorAxis,
			 columnHeaderAbsoluteMagnitude,
			 columnHeaderSlopeParameter,
			 columnHeaderReference,
			 columnHeaderNumberOppositions,
			 columnHeaderNumberObservations,
			 columnHeaderObservationSpan,
			 columnHeaderRmsResidual,
			 columnHeaderComputerName,
			 columnHeaderFlags,
			 columnHeaderDateLastObservation]);
		// Hide the list view
		listView.Visible = false;
		// Disable the numeric up-down controls
		numericUpDownMinimum.Enabled = false;
		numericUpDownMaximum.Enabled = false;
		// Enable the cancel button
		buttonCancel.Enabled = true;
		// Disable the button to prevent multiple clicks
		buttonList.Enabled = false;
		// Reset the cancellation flag
		isCancelled = false;
		// Enable the progress bar
		progressBar.Enabled = true;
		// Allow progress reporting from the background worker
		backgroundWorker.WorkerReportsProgress = true;
		// Allow cancellation of the background worker
		backgroundWorker.WorkerSupportsCancellation = true;
		// Handle the ProgressChanged event
		backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
		// Handle the RunWorkerCompleted event
		backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
		// Start the background worker
		backgroundWorker.RunWorkerAsync();
	}

	/// <summary>
	/// Handles the Click event of the Cancel button.
	/// Requests cancellation of the background processing by setting the internal cancellation flag.
	/// </summary>
	/// <param name="sender">Event source (the Cancel button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the Cancel button is clicked.
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
		// Check if the text to copy is not null or empty
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