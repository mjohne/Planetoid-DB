using Krypton.Toolkit;

using NLog;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// 
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class SearchForm : KryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// ArrayList to store planetoids data
		private ArrayList planetoidsDatabase = [];

		// Number of planetoids in the database
		private int
			numberPlanetoids, // Total number of planetoids
			entriesFound, // Number of entries found
			selectedIndex; // Index of the selected planetoids

		// Indicates whether the operation has been cancelled
		private bool isCancelled;

		// The index of the planetoids.
		private string strIndex = string.Empty;

		// The absolute magnitude of the planetoids.
		private string strMagAbs = string.Empty;

		// The slope parameter of the planetoid.
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
		private string strNumberObservations = string.Empty;

		// The number of oppositions of the planetoids.
		private string strNumberOppositions = string.Empty;

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
		private string strObsLastDate = string.Empty;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SearchForm"/> class.
		/// </summary>
		public SearchForm()
		{
			// Initialize the form components
			InitializeComponent();
			KeyDown += SearchForm_KeyDown;
			KeyPreview = true; // Ensures the form receives key events before the controls
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="check"></param>
		private void CheckItems(bool check)
		{
			for (int i = 0; i < checkedListBox.Items.Count; i++)
			{
				checkedListBox.SetItemChecked(index: i, value: check);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void MarkAll() => CheckItems(check: true);

		/// <summary>
		/// 
		/// </summary>
		private void UnmarkAll() => CheckItems(check: false);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arrTemp"></param>
		public void FillArray(ArrayList arrTemp)
		{
			planetoidsDatabase = arrTemp;
			numberPlanetoids = planetoidsDatabase.Count;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentPosition"></param>
		private void FormatRow(int currentPosition)
		{
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
			strIndex = planetoidsDatabase[index: currentPosition].ToString()[..7].Trim();
			strMagAbs = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 8, length: 5).Trim();
			strSlopeParam = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 14, length: 5).Trim();
			strEpoch = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 20, length: 5).Trim();
			strMeanAnomaly = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 26, length: 9).Trim();
			strArgPeri = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 37, length: 9).Trim();
			strLongAscNode = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 48, length: 9).Trim();
			strIncl = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 59, length: 9).Trim();
			strOrbEcc = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 70, length: 9).Trim();
			strMotion = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 80, length: 11).Trim();
			strSemiMajorAxis = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 92, length: 11).Trim();
			strRef = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 107, length: 9).Trim();
			strNumberObservations = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 117, length: 5).Trim();
			strNumberOppositions = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 123, length: 3).Trim();
			strObsSpan = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 127, length: 9).Trim();
			strRmsResidual = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 137, length: 4).Trim();
			strComputerName = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 150, length: 10).Trim();
			strFlags = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 161, length: 4).Trim();
			strDesignationName = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 166, length: 28).Trim();
			strObsLastDate = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 194, length: 8).Trim();
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

			//MessageBox.Show(textBox.Text.Contains(value: strDesgnName).ToString());

			//if (String.Equals(textBox.Text, strDesgnName))

			if (!strDesignationName.Contains(value: textBox.Text))
			{
				return;
			}

			entriesFound++;
			ListViewItem listViewItem = new(text: strIndex)
			{
				ToolTipText = $"{strIndex}: {strDesignationName}"
			};
			_ = listViewItem.SubItems.Add(text: "readable designation");
			_ = listViewItem.SubItems.Add(text: strDesignationName);
			_ = listView.Items.Add(value: listViewItem);
			labelEntriesFound.Text = $"{entriesFound} entries found";



			/*
			listViewItem.SubItems.Add(text: strDesgnName);
			listViewItem.SubItems.Add(text: strEpoch);
			listViewItem.SubItems.Add(text: strMeanAnomaly);
			listViewItem.SubItems.Add(text: strArgPeri);
			listViewItem.SubItems.Add(text: strLongAscNode);
			listViewItem.SubItems.Add(text: strIncl);
			listViewItem.SubItems.Add(text: strOrbEcc);
			listViewItem.SubItems.Add(text: strMotion);
			listViewItem.SubItems.Add(text: strSemiMajorAxis);
			listViewItem.SubItems.Add(text: strMagAbs);
			listViewItem.SubItems.Add(text: strSlopeParam);
			listViewItem.SubItems.Add(text: strRef);
			listViewItem.SubItems.Add(text: strNumbOppos);
			listViewItem.SubItems.Add(text: strNumbObs);
			listViewItem.SubItems.Add(text: strObsSpan);
			listViewItem.SubItems.Add(text: strRmsResdiual);
			listViewItem.SubItems.Add(text: strComputerName);
			listViewItem.SubItems.Add(text: strFlags);
			listViewItem.SubItems.Add(text: strObsLastDate);

			listView.Items.Add(value: listViewItem);
			*/

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maxIndex"></param>
		public void SetMaxIndex(int maxIndex) => numberPlanetoids = maxIndex;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetSelectedIndex() => selectedIndex;

		#endregion

		#region Form* event handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchForm_Load(object sender, EventArgs e)
		{
			buttonLoad.Enabled = buttonSearch.Enabled = false;
			MarkAll();
			ClearStatusBar();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region BackgroundWorker event handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			progressBar.Maximum = numberPlanetoids - 1;
			for (int i = 0; i < numberPlanetoids; i++)
			{
				FormatRow(currentPosition: i);
				backgroundWorker.ReportProgress(percentProgress: i);
				TaskbarProgress.SetValue(windowHandle: Handle, progressValue: i, progressMax: numberPlanetoids);
				if (isCancelled)
				{
					break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) => progressBar.Value = e.ProgressPercentage;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			listView.Visible = true;
			buttonCancel.Enabled = false;
			progressBar.Enabled = false;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
		}

		#endregion

		#region Enter-Handler

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

		#region Leave-Handler

		/// <summary>
		/// Called when the mouse pointer leaves a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click & ButtonClick event handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonClear_Click(object sender, EventArgs e)
		{
			textBox.Text = string.Empty;
			buttonSearch.Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonMarkAll_Click(object sender, EventArgs e) => MarkAll();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonUnmarkAll_Click(object sender, EventArgs e) => UnmarkAll();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonSearch_Click(object sender, EventArgs e)
		{
			entriesFound = 0;
			isCancelled = false;
			listView.Visible = false;
			listView.Items.Clear();
			buttonLoad.Enabled = false;
			buttonCancel.Enabled = progressBar.Enabled = true;
			backgroundWorker.WorkerReportsProgress = backgroundWorker.WorkerSupportsCancellation = true;
#pragma warning disable CS8622 // Die NULL-Zulässigkeit von Verweistypen im Typ des Parameters entspricht (möglicherweise aufgrund von Attributen für die NULL-Zulässigkeit) nicht dem Zieldelegaten.
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
#pragma warning restore CS8622 // Die NULL-Zulässigkeit von Verweistypen im Typ des Parameters entspricht (möglicherweise aufgrund von Attributen für die NULL-Zulässigkeit) nicht dem Zieldelegaten.
			backgroundWorker.RunWorkerAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCancel_Click(object sender, EventArgs e) => isCancelled = true;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonLoad_Click(object sender, EventArgs e)
		{
		}

		#endregion

		#region TextChanged event handlers
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBox_TextChanged(object sender, EventArgs e) => buttonSearch.Enabled = textBox.Text.Length > 0;

		#endregion

		#region SelectedIndexChanged

		private void ListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView.SelectedIndices.Count <= 0)
			{
				return;
			}

			int listViewSelectedIndex = listView.SelectedIndices[index: 0];
			if (listViewSelectedIndex >= 0)
			{
				SetStatusBar(text: $"{I10nStrings.Index}: {listView.Items[index: listViewSelectedIndex].Text} - {listView.Items[index: listViewSelectedIndex].SubItems[index: 1].Text}");
			}
			if (!buttonLoad.Enabled)
			{
				buttonLoad.Enabled = true;
			}
			this.selectedIndex = listViewSelectedIndex;
		}

		#endregion

		#region KeyDown event handler

		/// <summary>
		/// Handles the KeyDown event of the SearchForm.
		/// Closes the form when the Escape key is pressed.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void SearchForm_KeyDown(object? sender, KeyEventArgs e)
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
	}
}
