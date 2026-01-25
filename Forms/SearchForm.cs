using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Represents the form for searching planetoids.
/// </summary>
/// <remarks>
/// This form provides a user interface for searching planetoids in the database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class SearchForm : BaseKryptonForm
{
	/// <summary>
	/// Planetoids database array.
	/// </summary>
	/// <remarks>
	/// This array stores the data of all planetoids in the database.
	/// </remarks>
	private ArrayList planetoidsDatabase = [];

	/// <summary>
	/// Variables for tracking the number of planetoids, entries found, and selected index.
	/// </summary>
	/// <remarks>
	/// These variables are used to keep track of the search results and user selections.
	/// </remarks>
	private int
		numberPlanetoids, // Total number of planetoids
		entriesFound, // Number of entries found
		selectedIndex; // Index of the selected planetoids

	/// <summary>
	/// Indicates whether the operation has been cancelled
	/// </summary>
	/// <remarks>
	/// This variable is used to track the cancellation state of the operation.
	/// </remarks>
	private bool isCancelled;

	/// <summary>
	/// The index of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the index of the planetoids.
	/// </remarks>
	private string strIndex = string.Empty;

	/// <summary>
	/// The absolute magnitude of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the absolute magnitude of the planetoids.
	/// </remarks>
	private string strMagAbs = string.Empty;

	/// <summary>
	/// The slope parameter of the planetoid.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the slope parameter of the planetoid.
	/// </remarks>
	private string strSlopeParam = string.Empty;

	/// <summary>
	/// The epoch of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the epoch of the planetoids.
	/// </remarks>
	private string strEpoch = string.Empty;

	/// <summary>
	/// The mean anomaly of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the mean anomaly of the planetoids.
	/// </remarks>
	private string strMeanAnomaly = string.Empty;

	/// <summary>
	/// The argument of perihelion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the argument of perihelion of the planetoids.
	/// </remarks>
	private string strArgPeri = string.Empty;

	/// <summary>
	/// The longitude of the ascending node of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the longitude of the ascending node of the planetoids.
	/// </remarks>
	private string strLongAscNode = string.Empty;

	/// <summary>
	/// The inclination of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the inclination of the planetoids.
	/// </remarks>
	private string strIncl = string.Empty;

	/// <summary>
	/// The orbital eccentricity of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the orbital eccentricity of the planetoids.
	/// </remarks>
	private string strOrbEcc = string.Empty;

	/// <summary>
	/// The mean daily motion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the mean daily motion of the planetoids.
	/// </remarks>
	private string strMotion = string.Empty;

	/// <summary>
	/// The semi-major axis of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the semi-major axis of the planetoids.
	/// </remarks>
	private string strSemiMajorAxis = string.Empty;

	/// <summary>
	/// The reference for the planetoids data.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the reference for the planetoids data.
	/// </remarks>
	private string strRef = string.Empty;

	/// <summary>
	/// The number of observations of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the number of observations of the planetoids.
	/// </remarks>
	private string strNumberObservations = string.Empty;

	/// <summary>
	/// The number of oppositions of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the number of oppositions of the planetoids.
	/// </remarks>
	private string strNumberOppositions = string.Empty;

	/// <summary>
	/// The observation span of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the observation span of the planetoids.
	/// </remarks>
	private string strObsSpan = string.Empty;

	/// <summary>
	/// The RMS residual of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the RMS residual of the planetoids.
	/// </remarks>
	private string strRmsResidual = string.Empty;

	/// <summary>
	/// The name of the computer that processed the planetoids data.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the name of the computer that processed the planetoids data.
	/// </remarks>
	private string strComputerName = string.Empty;

	/// <summary>
	/// The flags associated with the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the flags associated with the planetoids.
	/// </remarks>
	private string strFlags = string.Empty;

	/// <summary>
	/// The designation name of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the designation name of the planetoids.
	/// </remarks>
	private string strDesignationName = string.Empty;

	/// <summary>
	/// The date of the last observation of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the date of the last observation of the planetoids.
	/// </remarks>
	private string strObsLastDate = string.Empty;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="SearchForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public SearchForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is called by the debugger to display the object in a readable format.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to set the status bar text and enable the information label.
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

	#region form event handlers

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

	#region BackgroundWorker event handlers

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

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when a control or ToolStrip item receives focus or is hovered over.
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

	#region SelectedIndexChanged event handlers

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
}