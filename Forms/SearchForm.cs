// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

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
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Planetoids database list.
	/// </summary>
	/// <remarks>
	/// This list stores the data of all planetoids in the database.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

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
	private string _index = string.Empty;

	/// <summary>
	/// The absolute magnitude of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the absolute magnitude of the planetoids.
	/// </remarks>
	private string _magAbs = string.Empty;

	/// <summary>
	/// The slope parameter of the planetoid.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the slope parameter of the planetoid.
	/// </remarks>
	private string _slopeParam = string.Empty;

	/// <summary>
	/// The epoch of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the epoch of the planetoids.
	/// </remarks>
	private string _epoch = string.Empty;

	/// <summary>
	/// The mean anomaly of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the mean anomaly of the planetoids.
	/// </remarks>
	private string _meanAnomaly = string.Empty;

	/// <summary>
	/// The argument of perihelion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the argument of perihelion of the planetoids.
	/// </remarks>
	private string _argPeri = string.Empty;

	/// <summary>
	/// The longitude of the ascending node of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the longitude of the ascending node of the planetoids.
	/// </remarks>
	private string _longAscNode = string.Empty;

	/// <summary>
	/// The inclination of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the inclination of the planetoids.
	/// </remarks>
	private string _incl = string.Empty;

	/// <summary>
	/// The orbital eccentricity of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the orbital eccentricity of the planetoids.
	/// </remarks>
	private string _orbEcc = string.Empty;

	/// <summary>
	/// The mean daily motion of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the mean daily motion of the planetoids.
	/// </remarks>
	private string _motion = string.Empty;

	/// <summary>
	/// The semi-major axis of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the semi-major axis of the planetoids.
	/// </remarks>
	private string _semiMajorAxis = string.Empty;

	/// <summary>
	/// The reference for the planetoids data.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the reference for the planetoids data.
	/// </remarks>
	private string _ref = string.Empty;

	/// <summary>
	/// The number of observations of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the number of observations of the planetoids.
	/// </remarks>
	private string _numberObservations = string.Empty;

	/// <summary>
	/// The number of oppositions of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the number of oppositions of the planetoids.
	/// </remarks>
	private string _numberOppositions = string.Empty;

	/// <summary>
	/// The observation span of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the observation span of the planetoids.
	/// </remarks>
	private string _obsSpan = string.Empty;

	/// <summary>
	/// The RMS residual of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the RMS residual of the planetoids.
	/// </remarks>
	private string _rmsResidual = string.Empty;

	/// <summary>
	/// The name of the computer that processed the planetoids data.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the name of the computer that processed the planetoids data.
	/// </remarks>
	private string _computerName = string.Empty;

	/// <summary>
	/// The flags associated with the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the flags associated with the planetoids.
	/// </remarks>
	private string _flags = string.Empty;

	/// <summary>
	/// The designation name of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the designation name of the planetoids.
	/// </remarks>
	private string _designationName = string.Empty;

	/// <summary>
	/// The date of the last observation of the planetoids.
	/// </summary>
	/// <remarks>
	/// This variable is used to store the date of the last observation of the planetoids.
	/// </remarks>
	private string _obsLastDate = string.Empty;

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
	/// Fills the planetoids database with data.
	/// </summary>
	/// <param name="arrTemp">The list containing planetoid data.</param>
	public void FillArray(List<string> arrTemp)
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
		_index = planetoidsDatabase[index: currentPosition].ToString()[..7].Trim();
		_magAbs = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 8, length: 5).Trim();
		_slopeParam = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 14, length: 5).Trim();
		_epoch = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 20, length: 5).Trim();
		_meanAnomaly = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 26, length: 9).Trim();
		_argPeri = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 37, length: 9).Trim();
		_longAscNode = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 48, length: 9).Trim();
		_incl = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 59, length: 9).Trim();
		_orbEcc = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 70, length: 9).Trim();
		_motion = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 80, length: 11).Trim();
		_semiMajorAxis = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 92, length: 11).Trim();
		_ref = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 107, length: 9).Trim();
		_numberObservations = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 117, length: 5).Trim();
		_numberOppositions = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 123, length: 3).Trim();
		_obsSpan = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 127, length: 9).Trim();
		_rmsResidual = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 137, length: 4).Trim();
		_computerName = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 150, length: 10).Trim();
		_flags = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 161, length: 4).Trim();
		_designationName = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 166, length: 28).Trim();
		_obsLastDate = planetoidsDatabase[index: currentPosition].ToString().Substring(startIndex: 194, length: 8).Trim();
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

		//MessageBox.Show(textBox.Text.Contains(value: strDesgnName).ToString());

		//if (String.Equals(textBox.Text, strDesgnName))

		if (!_designationName.Contains(value: textBox.Text))
		{
			return;
		}

		entriesFound++;
		ListViewItem listViewItem = new(text: _index)
		{
			ToolTipText = $"{_index}: {_designationName}"
		};
		_ = listViewItem.SubItems.Add(text: "readable designation");
		_ = listViewItem.SubItems.Add(text: _designationName);
		_ = listView.Items.Add(value: listViewItem);
		labelEntriesFound.Text = $"{entriesFound} entries found";



		/*
		listViewItem.SubItems.Add(text: strDesgnName);
		listViewItem.SubItems.Add(text: _epoch);
		listViewItem.SubItems.Add(text: _meanAnomaly);
		listViewItem.SubItems.Add(text: _argPeri);
		listViewItem.SubItems.Add(text: _longAscNode);
		listViewItem.SubItems.Add(text: _incl);
		listViewItem.SubItems.Add(text: _orbEcc);
		listViewItem.SubItems.Add(text: _motion);
		listViewItem.SubItems.Add(text: _semiMajorAxis);
		listViewItem.SubItems.Add(text: _magAbs);
		listViewItem.SubItems.Add(text: _slopeParam);
		listViewItem.SubItems.Add(text: _ref);
		listViewItem.SubItems.Add(text: strNumbOppos);
		listViewItem.SubItems.Add(text: strNumbObs);
		listViewItem.SubItems.Add(text: _obsSpan);
		listViewItem.SubItems.Add(text: strRmsResdiual);
		listViewItem.SubItems.Add(text: _computerName);
		listViewItem.SubItems.Add(text: _flags);
		listViewItem.SubItems.Add(text: _obsLastDate);

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
		ClearStatusBar(label: labelInformation);
	}

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
			SetStatusBar(label: labelInformation, text: $"{I18nStrings.Index}: {listView.Items[index: listViewSelectedIndex].Text} - {listView.Items[index: listViewSelectedIndex].SubItems[index: 1].Text}");
		}
		if (!buttonLoad.Enabled)
		{
			buttonLoad.Enabled = true;
		}
		this.selectedIndex = listViewSelectedIndex;
	}

	#endregion
}