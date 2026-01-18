using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents the form for displaying planetoids data in table mode.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class TableModeForm : BaseKryptonForm
	{
		// The database of planetoids.
		private List<string> planetoidsDatabase = [];

		// The number of planetoids in the database.
		private int numberPlanetoids;

		// Indicates whether the operation is cancelled.
		private bool isCancelled;

		// The index of the planetoids.
		private string strIndex = string.Empty;

		// The absolute magnitude of the planetoids.
		private string strMagAbs = string.Empty;

		// The slope parameter of the planetoids.
		private string strSlopeParam = string.Empty;

		// The epoch of the planetoids.
		private string strEpoch = string.Empty;

		// The mean anomaly of the planetoids.
		private string strMeanAnomaly = string.Empty;

		// The argument of perihelion of the planetoids.
		private string strArgPeri = string.Empty;

		// The longitude of the ascending node of the planetoids.
		private string strLongAscNode = string.Empty;

		// The inclination of the planetoids.
		private string strIncl = string.Empty;

		// The orbital eccentricity of the planetoids.
		private string strOrbEcc = string.Empty;

		// The mean daily motion of the planetoids.
		private string strMotion = string.Empty;

		// The semi-major axis of the planetoids.
		private string strSemiMajorAxis = string.Empty;

		// The reference for the planetoids data.
		private string strRef = string.Empty;

		// The number of observations of the planetoids.
		private string strNumberObservation = string.Empty;

		// The number of oppositions of the planetoids.
		private string strNumberOpposition = string.Empty;

		// The observation span of the planetoids.
		private string strObsSpan = string.Empty;

		// The RMS residual of the planetoids.
		private string strRmsResidual = string.Empty;

		// The name of the computer that processed the planetoids data.
		private string strComputerName = string.Empty;

		// The flags associated with the planetoids.
		private string strFlags = string.Empty;

		// The designation name of the planetoids.
		private string strDesignationName = string.Empty;

		// The date of the last observation of the planetoids.
		private string strObservationLastDate = string.Empty;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TableModeForm"/> class.
		/// </summary>
		public TableModeForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region local methods

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

		/// <summary>
		/// Fills the planetoids database with the specified array.
		/// </summary>
		/// <param name="arrTemp">The array to fill the database with.</param>
		public void FillArray(ArrayList arrTemp)
		{
			planetoidsDatabase = [.. arrTemp.Cast<string>()];
			numberPlanetoids = planetoidsDatabase.Count;
		}

		/// <summary>
		/// Formats the row at the specified position.
		/// </summary>
		/// <param name="currentPosition">The position of the row to format.</param>
		private void FormatRow(int currentPosition)
		{
			// ReSharper disable once IdentifierTypo
			string planetoid = planetoidsDatabase[index: currentPosition];
			// Check if the planetoids is not null
			// and the length is greater than 0
			if (planetoid.Length <= 0)
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

		#region Form event handlers

		/// <summary>
		/// Handles the Load event of the TableModeForm.
		/// Initializes the form controls and sets their initial values.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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

		/// <summary>
		/// Handles the FormClosed event of the TableModeForm.
		/// Disposes the list view and the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void TableModeForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			listView.Dispose();
			Dispose();
		}

		#endregion

		#region BackgroundWorker

		/// <summary>
		/// Handles the DoWork event of the BackgroundWorker.
		/// Processes the planetoids data in the background.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
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
		/// Handles the ProgressChanged event of the BackgroundWorker.
		/// Updates the progress bar with the current progress.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data.</param>
		private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => progressBar.Value = e.ProgressPercentage;

		/// <summary>
		/// Handles the RunWorkerCompleted event of the BackgroundWorker.
		/// Finalizes the background work and updates the UI.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
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

		#region SelectedIndexChanged event handlers

		/// <summary>
		/// Handles the SelectedIndexChanged event of the ListView.
		/// Updates the status bar with the selected planetoids index and designation name.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ListViewTableMode_SelectedIndexChanged(object sender, EventArgs e)
		{
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

		#region Clicks event handlers

		/// <summary>
		/// Handles the Click event of the List button.
		/// Starts the background worker to process the planetoids data.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonList_Click(object sender, EventArgs e)
		{
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
		/// Cancels the background worker operation.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			isCancelled = true;
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
