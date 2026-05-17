// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

namespace Planetoid_DB;

/// <summary>Main form for managing records in the Planetoid database.</summary>
/// <remarks>This form provides a user interface for viewing and editing records in the database.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class RecordsTop10Form : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the database downloader.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Stores a copy of the planetoids database used for record scanning.</summary>
	/// <remarks>The data is copied so scans remain independent from live edits in the main form.</remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>Holds the currently displayed top-ten result list.</summary>
	/// <remarks>The list order reflects the active sort order from the toolbar.</remarks>
	private List<TopRecordEntry> topRecords = [];

	/// <summary>Cancellation token source for the currently running scan.</summary>
	/// <remarks>When null, no scan is currently active.</remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>Holds one top-ten entry containing designation, numeric value, and display value.</summary>
	/// <param name="Designation">Readable designation of the planetoid.</param>
	/// <param name="StringValue">Original string value as shown from source data.</param>
	/// <param name="NumericValue">Numeric value used for ranking and comparison.</param>
	private readonly record struct TopRecordEntry(string Designation, string StringValue, double NumericValue);

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="RecordsTop10Form"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public RecordsTop10Form()
		: this(arrTemp: [], selectedElement: null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="RecordsTop10Form"/> class with an initial database copy and an optional preselected element.</summary>
	/// <param name="arrTemp">Source database records to copy into this form.</param>
	/// <param name="selectedElement">Optional orbital element text to preselect in the element list.</param>
	/// <remarks>This overload supports opening the form directly on a specific orbital element.</remarks>
	public RecordsTop10Form(List<string> arrTemp, string? selectedElement)
	{
		InitializeComponent();
		FillArray(arrTemp: arrTemp);
		PreselectOrbitalElement(selectedElement: selectedElement);
		InitializeUiBehavior();
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is called to obtain a string representation of the current instance.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Copies the provided planetoid database into this form instance.</summary>
	/// <param name="arrTemp">Source records from the main form.</param>
	/// <remarks>Use this when the parameterless constructor is used and data is assigned afterwards.</remarks>
	public void FillArray(List<string> arrTemp) => planetoidsDatabase = [.. arrTemp];

	/// <summary>Initializes runtime UI wiring and default state for scan interactions.</summary>
	/// <remarks>Wires sort-order toggle behavior, configures defaults, and enables smoother repainting of the table layout.</remarks>
	private void InitializeUiBehavior()
	{
		toolStripButtonSortOrderAscending.Click += SetAscendingSortOrder_Click;
		toolStripButtonSortOrderDescending.Click += SetDescendingSortOrder_Click;
		if (listBox.SelectedIndex < 0 && listBox.Items.Count > 0)
		{
			listBox.SelectedIndex = 0;
		}
		SetGotoButtonsEnabled(isEnabled: false);
		toolStripButtonCancel.Enabled = false;
		try
		{
			PropertyInfo? dbProp = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			dbProp?.SetValue(obj: tableLayoutPanel, value: true, index: null);
			MethodInfo? setStyleMethod = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			setStyleMethod?.Invoke(obj: tableLayoutPanel, parameters: [ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true]);
		}
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Could not set DoubleBuffered on tableLayoutPanel");
		}
	}

	/// <summary>Preselects an orbital element by display text when available.</summary>
	/// <param name="selectedElement">The element name to select, such as "Semi-major axis".</param>
	/// <remarks>Matching is case-insensitive and allows partial text matches.</remarks>
	private void PreselectOrbitalElement(string? selectedElement)
	{
		if (string.IsNullOrWhiteSpace(value: selectedElement) || listBox.Items.Count == 0)
		{
			if (listBox.SelectedIndex < 0)
			{
				listBox.SelectedIndex = 0;
			}
			return;
		}
		int matchIndex = -1;
		for (int i = 0; i < listBox.Items.Count; i++)
		{
			string itemText = listBox.Items[i]?.ToString() ?? string.Empty;
			if (itemText.Equals(value: selectedElement, comparisonType: StringComparison.OrdinalIgnoreCase) || itemText.Contains(value: selectedElement, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				matchIndex = i;
				break;
			}
		}
		listBox.SelectedIndex = matchIndex >= 0 ? matchIndex : 0;
	}

	/// <summary>Gets designation labels in rank order from place 1 to place 10.</summary>
	/// <returns>Array of designation labels bound to rank rows.</returns>
	private KryptonLabel[] GetDesignationLabels() =>
	[
		labelReadableDesignation01, labelReadableDesignation02, labelReadableDesignation03, labelReadableDesignation04, labelReadableDesignation05,
		labelReadableDesignation06, labelReadableDesignation07, labelReadableDesignation08, labelReadableDesignation09, labelReadableDesignation10
	];

	/// <summary>Gets value labels in rank order from place 1 to place 10.</summary>
	/// <returns>Array of value labels bound to rank rows.</returns>
	private KryptonLabel[] GetValueLabels() =>
	[
		labelValue01, labelValue02, labelValue03, labelValue04, labelValue05,
		labelValue06, labelValue07, labelValue08, labelValue09, labelValue10
	];

	/// <summary>Gets "Goto" buttons in rank order from place 1 to place 10.</summary>
	/// <returns>Array of navigation buttons bound to rank rows.</returns>
	private KryptonButton[] GetGotoButtons() =>
	[
		buttonGoto01, buttonGoto02, buttonGoto03, buttonGoto04, buttonGoto05,
		buttonGoto06, buttonGoto07, buttonGoto08, buttonGoto09, buttonGoto10
	];

	/// <summary>Resets all result labels and clears tracked top-ten data.</summary>
	/// <remarks>This method is called before each new scan run.</remarks>
	private void ResetLabels()
	{
		KryptonLabel[] designationLabels = GetDesignationLabels();
		KryptonLabel[] valueLabels = GetValueLabels();
		for (int i = 0; i < designationLabels.Length; i++)
		{
			designationLabels[i].Values.Text = "-";
			valueLabels[i].Values.Text = "-";
		}
		topRecords = [];
	}

	/// <summary>Enables or disables all rank navigation buttons.</summary>
	/// <param name="isEnabled">True to enable, false to disable.</param>
	private void SetGotoButtonsEnabled(bool isEnabled)
	{
		foreach (KryptonButton button in GetGotoButtons())
		{
			button.Enabled = isEnabled;
		}
	}

	/// <summary>Attempts to extract the currently selected orbital element as a numeric and display value.</summary>
	/// <param name="record">Parsed planetoid record.</param>
	/// <param name="selectedElementIndex">Selected orbital element index from the list box.</param>
	/// <param name="stringValue">Returns the original string representation for display.</param>
	/// <param name="numericValue">Returns the numeric value used for comparison.</param>
	/// <returns>True when extraction and parsing succeed; otherwise false.</returns>
	private static bool TryGetSelectedElementValue(PlanetoidRecord record, int selectedElementIndex, [NotNullWhen(true)] out string? stringValue, out double numericValue)
	{
		stringValue = selectedElementIndex switch
		{
			0 => record.MeanAnomaly,
			1 => record.ArgPeri,
			2 => record.LongAscNode,
			3 => record.Incl,
			4 => record.OrbEcc,
			5 => record.Motion,
			6 => record.SemiMajorAxis,
			7 => record.MagAbs,
			8 => record.SlopeParam,
			9 => record.NumberOpposition,
			10 => record.NumberObservation,
			11 => record.RmsResidual,
			_ => null
		};
		if (string.IsNullOrWhiteSpace(value: stringValue))
		{
			numericValue = 0;
			return false;
		}
		bool isIntegerField = selectedElementIndex is 9 or 10;
		if (isIntegerField && int.TryParse(s: stringValue, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int intValue))
		{
			numericValue = intValue;
			return true;
		}
		if (!isIntegerField && double.TryParse(s: stringValue, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double doubleValue))
		{
			numericValue = doubleValue;
			return true;
		}
		numericValue = 0;
		return false;
	}

	/// <summary>Inserts a candidate into the top-ten list when it qualifies for the active sort order.</summary>
	/// <param name="entries">Mutable list with current top-ten entries.</param>
	/// <param name="candidate">Candidate entry to evaluate.</param>
	/// <param name="isAscending">True for ascending (smallest first), false for descending (largest first).</param>
	/// <returns>True when the visible top-ten list changed; otherwise false.</returns>
	private static bool TryInsertTopRecord(List<TopRecordEntry> entries, TopRecordEntry candidate, bool isAscending)
	{
		entries.Add(item: candidate);
		entries.Sort(comparison: (left, right) => isAscending
			? left.NumericValue.CompareTo(value: right.NumericValue)
			: right.NumericValue.CompareTo(value: left.NumericValue));
		if (entries.Count > 10)
		{
			entries.RemoveRange(index: 10, count: entries.Count - 10);
		}
		return entries.Contains(item: candidate);
	}

	/// <summary>Applies a top-ten snapshot to the table labels.</summary>
	/// <param name="entries">Ordered top-ten entries to display.</param>
	/// <remarks>Values are shown exactly as original source strings.</remarks>
	private void ApplyTopRecords(IReadOnlyList<TopRecordEntry> entries)
	{
		KryptonLabel[] designationLabels = GetDesignationLabels();
		KryptonLabel[] valueLabels = GetValueLabels();
		for (int i = 0; i < designationLabels.Length; i++)
		{
			if (i < entries.Count)
			{
				designationLabels[i].Values.Text = entries[i].Designation;
				valueLabels[i].Values.Text = entries[i].StringValue;
			}
			else
			{
				designationLabels[i].Values.Text = "-";
				valueLabels[i].Values.Text = "-";
			}
		}
	}

	/// <summary>Runs the top-ten scan asynchronously for the selected orbital element.</summary>
	/// <param name="selectedElementIndex">Selected orbital element index.</param>
	/// <param name="isAscending">True for ascending (smallest values first), false for descending.</param>
	/// <param name="token">Cancellation token for cooperative cancellation.</param>
	/// <param name="progress">Progress callback for percent and live top-ten updates.</param>
	/// <returns>A task that completes when scanning is finished or cancelled.</returns>
	private async Task ScanTopRecordsAsync(int selectedElementIndex, bool isAscending, CancellationToken token, IProgress<(int Percent, List<TopRecordEntry>? UpdatedEntries)> progress)
	{
		await Task.Run(() =>
		{
			List<TopRecordEntry> currentTopEntries = [];
			int total = planetoidsDatabase.Count;
			int lastPercent = -1;
			for (int i = 0; i < total; i++)
			{
				token.ThrowIfCancellationRequested();
				int percent = (int)((i + 1) * 100L / total);
				try
				{
					PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: planetoidsDatabase[index: i]);
					if (string.IsNullOrWhiteSpace(value: record.DesignationName))
					{
						continue;
					}
					if (TryGetSelectedElementValue(record: record, selectedElementIndex: selectedElementIndex, stringValue: out string? stringValue, numericValue: out double numericValue))
					{
						TopRecordEntry candidate = new(Designation: record.DesignationName, StringValue: stringValue, NumericValue: numericValue);
						if (TryInsertTopRecord(entries: currentTopEntries, candidate: candidate, isAscending: isAscending))
						{
							progress.Report(value: (percent, [.. currentTopEntries]));
						}
					}
				}
				catch (Exception ex)
				{
					logger.Warn(exception: ex, message: $"Skipping entry at index {i}: {ex.Message}");
				}
				if (percent != lastPercent)
				{
					progress.Report(value: (percent, null));
					lastPercent = percent;
				}
			}
		}, cancellationToken: token);
	}

	/// <summary>Updates the progress bar and top-ten labels from a progress callback.</summary>
	/// <param name="percent">Current scan percentage.</param>
	/// <param name="updatedEntries">Optional live top-ten snapshot.</param>
	private void HandleProgressUpdate(int percent, List<TopRecordEntry>? updatedEntries)
	{
		if (kryptonProgressBar.Value != percent)
		{
			kryptonProgressBar.Value = percent;
			kryptonProgressBar.Text = $"{percent} %";
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)percent, progressMax: 100);
		}
		if (updatedEntries != null)
		{
			topRecords = updatedEntries;
			ApplyTopRecords(entries: topRecords);
		}
	}

	/// <summary>Navigates from a rank row to the corresponding planetoid in the main form.</summary>
	/// <param name="rankIndex">Zero-based rank index from 0 to 9.</param>
	/// <remarks>On success, the selected record is opened in the main form and this dialog closes.</remarks>
	private void NavigateToRank(int rankIndex)
	{
		if (rankIndex < 0 || rankIndex >= topRecords.Count)
		{
			return;
		}
		if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is not PlanetoidDbForm mainForm)
		{
			ShowErrorMessage(message: "Main form is not available.");
			return;
		}
		mainForm.JumpToRecord(index: string.Empty, designation: topRecords[rankIndex].Designation);
		mainForm.BringToFront();
		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>Prepares and displays a save file dialog with a default file name and initial directory set to the user's Documents folder.</summary>
	/// <remarks>The method sets the dialog's initial directory to the user's Documents folder and generates a default file name based on the current date and time. The dialog is shown modally, and the result indicates whether the user confirmed the selection.</remarks>
	/// <param name="dialog">The file dialog to configure and display. Must not be null.</param>
	/// <param name="ext">The file extension to use for the default file name, without the leading period.</param>
	/// <returns>true if the user selects a file and confirms the dialog; otherwise, false.</returns>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"Top-Ten-Records_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Displays a save dialog and exports the table layout panel contents using the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the table layout panel, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TableLayoutPanel, string, string> exportAction)
	{
		// Create and configure the save file dialog with the specified filter, default extension, and title. The dialog allows the user to choose where to save the exported file and what name to give it.
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};
		// Prepare and show the save dialog. If the user cancels the dialog, the method returns without performing any export action.
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: defaultExt))
		{
			return;
		}
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress, and then invoke the specified export action with the text box containing the output, the title for the export, and the selected file name. After the export action is completed, reset the cursor to the default state.
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(arg1: tableLayoutPanel, arg2: "Top Ten Records", arg3: saveFileDialog.FileName);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			ShowErrorMessage(message: $"An error has occurred during export: {ex.Message}");
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}


	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the RecordsMainForm is loaded.</remarks>
	private void RecordsMainForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		toolStripButtonCancel.Enabled = false;
		SetGotoButtonsEnabled(isEnabled: false);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start control. Starts the main process.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Start control is clicked.</remarks>
	private async void Start_Click(object sender, EventArgs e)
	{
		if (planetoidsDatabase.Count == 0)
		{
			ShowErrorMessage(message: I18nStrings.MpcorbDatNotFoundText);
			return;
		}
		if (listBox.SelectedIndex < 0)
		{
			ShowErrorMessage(message: "Please select an orbital element first.");
			return;
		}
		cancellationTokenSource?.Cancel();
		cancellationTokenSource?.Dispose();
		cancellationTokenSource = new CancellationTokenSource();
		ResetLabels();
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripButtonSortOrderAscending.Enabled = false;
		toolStripButtonSortOrderDescending.Enabled = false;
		listBox.Enabled = false;
		toolStripDropDownButtonSaveList.Enabled = false;
		contextMenuSaveToFile.Enabled = false;
		SetGotoButtonsEnabled(isEnabled: false);
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0 %";

		bool isAscending = toolStripButtonSortOrderAscending.Checked;
		Progress<(int Percent, List<TopRecordEntry>? UpdatedEntries)> progress = new(handler: update => HandleProgressUpdate(percent: update.Percent, updatedEntries: update.UpdatedEntries));
		try
		{
			await ScanTopRecordsAsync(
				selectedElementIndex: listBox.SelectedIndex,
				isAscending: isAscending,
				token: cancellationTokenSource.Token,
				progress: progress);
			kryptonProgressBar.Text = "100 %";
		}
		catch (OperationCanceledException)
		{
			kryptonProgressBar.Text = "Scan cancelled";
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			kryptonProgressBar.Text = I18nStrings.ErrorCaption;
		}
		finally
		{
			toolStripButtonStart.Enabled = true;
			toolStripButtonCancel.Enabled = false;
			toolStripButtonSortOrderAscending.Enabled = true;
			toolStripButtonSortOrderDescending.Enabled = true;
			listBox.Enabled = true;
			toolStripDropDownButtonSaveList.Enabled = true;
			contextMenuSaveToFile.Enabled = true;
			SetGotoButtonsEnabled(isEnabled: topRecords.Count > 0);
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}
	}

	/// <summary>Handles the Click event of the Cancel control. Cancels the main process.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Cancel control is clicked.</remarks>
	private void Cancel_Click(object sender, EventArgs e)
	{
		cancellationTokenSource?.Cancel();
		toolStripButtonCancel.Enabled = false;
	}

	/// <summary>Handles the Click event of the Goto01 control. Navigates to the first record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto01 control is clicked.</remarks>
	private void Goto01_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 0);
	}

	/// <summary>Handles the Click event of the Goto02 control. Navigates to the second record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto02 control is clicked.</remarks>
	private void Goto02_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 1);
	}

	/// <summary>Handles the Click event of the Goto03 control. Navigates to the third record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto03 control is clicked.</remarks>
	private void Goto03_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 2);
	}

	/// <summary>Handles the Click event of the Goto04 control. Navigates to the fourth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto04 control is clicked.</remarks>
	private void Goto04_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 3);
	}

	/// <summary>Handles the Click event of the Goto05 control. Navigates to the fifth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto05 control is clicked.</remarks>
	private void Goto05_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 4);
	}

	/// <summary>Handles the Click event of the Goto06 control. Navigates to the sixth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto06 control is clicked.</remarks>
	private void Goto06_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 5);
	}

	/// <summary>Handles the Click event of the Goto07 control. Navigates to the seventh record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto07 control is clicked.</remarks>
	private void Goto07_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 6);
	}

	/// <summary>Handles the Click event of the Goto08 control. Navigates to the eighth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto08 control is clicked.</remarks>
	private void Goto08_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 7);
	}

	/// <summary>Handles the Click event of the Goto09 control. Navigates to the ninth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto09 control is clicked.</remarks>
	private void Goto09_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 8);
	}

	/// <summary>Handles the Click event of the Goto10 control. Navigates to the tenth record.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Goto10 control is clicked.</remarks>
	private void Goto10_Click(object sender, EventArgs e)
	{
		NavigateToRank(rankIndex: 9);
	}

	/// <summary>Handles the Click event of the ascending sort order button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data associated with the click.</param>
	/// <remarks>Ensures only one sort direction is checked at a time.</remarks>
	private void SetAscendingSortOrder_Click(object sender, EventArgs e)
	{
		if (!toolStripButtonSortOrderAscending.Checked && !toolStripButtonSortOrderDescending.Checked)
		{
			toolStripButtonSortOrderAscending.Checked = true;
		}
		toolStripButtonSortOrderDescending.Checked = !toolStripButtonSortOrderAscending.Checked;
	}

	/// <summary>Handles the Click event of the descending sort order button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data associated with the click.</param>
	/// <remarks>Ensures only one sort direction is checked at a time.</remarks>
	private void SetDescendingSortOrder_Click(object sender, EventArgs e)
	{
		if (!toolStripButtonSortOrderDescending.Checked && !toolStripButtonSortOrderAscending.Checked)
		{
			toolStripButtonSortOrderDescending.Checked = true;
		}
		toolStripButtonSortOrderAscending.Checked = !toolStripButtonSortOrderDescending.Checked;
	}

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TableLayoutPanelExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TableLayoutPanelExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TableLayoutPanelExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TableLayoutPanelExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a ReStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TableLayoutPanelExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TableLayoutPanelExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word document.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TableLayoutPanelExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TableLayoutPanelExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As RTF menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TableLayoutPanelExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TableLayoutPanelExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TableLayoutPanelExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TableLayoutPanelExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TableLayoutPanelExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TableLayoutPanelExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TableLayoutPanelExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TableLayoutPanelExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TableLayoutPanelExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TableLayoutPanelExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TableLayoutPanelExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TableLayoutPanelExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TableLayoutPanelExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TableLayoutPanelExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TableLayoutPanelExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As SQL menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TableLayoutPanelExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TableLayoutPanelExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TableLayoutPanelExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As PostScript menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TableLayoutPanelExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TableLayoutPanelExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TableLayoutPanelExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TableLayoutPanelExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TableLayoutPanelExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TableLayoutPanelExporter.SaveAsChm);

	#endregion
}
