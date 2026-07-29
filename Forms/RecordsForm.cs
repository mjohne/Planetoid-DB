// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Represents the form that scans all orbital elements for maximum or minimum record values.</summary>
/// <remarks>This form displays the record holder (asteroid with the highest or lowest value) for each orbital element in the database. The user can choose between maximum and minimum records, start and cancel the scan at any time.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class RecordsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Records";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Records";

	#endregion

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the records form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the planetoids database.</summary>
	/// <remarks>This list holds a copy of all planetoid database entries for scanning.</remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>Indicates whether the scan operation has been cancelled by the user.</summary>
	/// <remarks>Set to true when the user clicks the Cancel button to stop an ongoing scan.</remarks>
	private bool isCancelled;

	/// <summary>Stores the current record values for each orbital element (max or min).</summary>
	/// <remarks>Index corresponds to element: 0=MeanAnomaly, 1=ArgPeri, 2=LongAscNode, 3=Incl, 4=OrbEcc, 5=Motion, 6=SemiMajorAxis, 7=MagAbs, 8=SlopeParam, 9=NumberOpposition, 10=NumberObservation, 11=RmsResidual. Note: Observation Span is intentionally skipped.</remarks>
	private readonly double[] recordValues = new double[12];

	/// <summary>Stores the original string values from the database for each orbital element.</summary>
	/// <remarks>These string values are displayed in the UI as-is, preserving the original format from the main application.</remarks>
	private readonly string[] recordStringValues = new string[12];

	/// <summary>Holds a progress update for a single orbital element record, used to marshal label updates from the background thread to the UI thread via <see cref="BackgroundWorker.ReportProgress(int, object)"/>.</summary>
	/// <param name="ElementIndex">Zero-based index of the orbital element.</param>
	/// <param name="Value">The new record value (as double for comparison).</param>
	/// <param name="StringValue">The original string value from the database to display in the UI.</param>
	/// <param name="Designation">The readable designation of the record-holder asteroid.</param>
	/// <remarks>This struct is used to pass progress updates from the background worker to the UI thread.</remarks>
	private readonly record struct RecordProgressUpdate(int ElementIndex, double Value, string StringValue, string Designation);

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="RecordsForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components and wires up BackgroundWorker events.</remarks>
	public RecordsForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Enable double buffering for the TableLayoutPanel to prevent flickering
		DoubleBufferingHelper.EnableDoubleBuffering(control: tableLayoutPanel);
		// Wire up BackgroundWorker events once at construction time
#pragma warning disable CS8622
		backgroundWorker.DoWork += BackgroundWorker_DoWork;
		backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
		backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
#pragma warning restore CS8622
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is called to obtain a string representation of the current instance.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Fills the internal database list with a copy of the provided planetoid data.</summary>
	/// <param name="arrTemp">The list of raw planetoid data lines from the main form.</param>
	/// <remarks>This method must be called before starting the scan to provide the data source. The data is stored as a copy so changes in the main form do not affect the scan.</remarks>
	public void FillArray(List<string> arrTemp) => planetoidsDatabase = [.. arrTemp];

	/// <summary>Resets all record labels to the initial state before starting a new scan.</summary>
	/// <remarks>Clears designation and value labels, and resets internal tracking arrays.</remarks>
	private void ResetLabels()
	{
		// Reset all designation and value labels to placeholder text
		KryptonLabel[] designationLabels = GetDesignationLabels();
		KryptonLabel[] valueLabels = GetValueLabels();
		for (int i = 0; i < designationLabels.Length; i++)
		{
			designationLabels[i].Values.Text = "-";
			valueLabels[i].Values.Text = "-";
		}
		// Reset tracking arrays
		bool isAscending = toolStripButtonSortOrderDescending.Checked;
		for (int i = 0; i < recordValues.Length; i++)
		{
			recordValues[i] = isAscending ? double.MinValue : double.MaxValue;
			recordStringValues[i] = string.Empty;
		}
	}

	/// <summary>Gets the array of designation labels in element order.</summary>
	/// <returns>An array of <see cref="KryptonLabel"/> controls for designation display.</returns>
	/// <remarks>The array is ordered to match the orbital element order used in record tracking.</remarks>
	private KryptonLabel[] GetDesignationLabels() =>
	[
		labelDesignationMeanAnomalyAtTheEpoch, labelDesignationArgumentOfThePerihelion, labelDesignationLongitudeOfTheAscendingNode, labelDesignationInclinationToTheEcliptic,
		labelDesignationOrbitalEccentricity, labelDesignationMeanDailyMotion, labelDesignationSemiMajorAxis, labelDesignationAbsoluteMagnitude,
		labelDesignationSlopeParameter, labelDesignationNumberOfOppositions, labelDesignationNumberOfObservations, labelDesignationRmsResidual
	];

	/// <summary>Gets the array of value labels in element order.</summary>
	/// <returns>An array of <see cref="KryptonLabel"/> controls for value display.</returns>
	/// <remarks>The array is ordered to match the orbital element order used in record tracking.</remarks>
	private KryptonLabel[] GetValueLabels() =>
	[
		labelValueMeanAnomalyAtTheEpoch, labelValueArgumentOfThePerihelion, labelValueLongitudeOfTheAscendingNode, labelValueInclinationToTheEcliptic,
		labelValueOrbitalEccentricity, labelValueMeanDailyMotion, labelValueSemiMajorAxis, labelValueAbsoluteMagnitude,
		labelValueSlopeParameter, labelValueNumberOfOppositions, labelValueNumberOfObservations, labelValueRmsResidual
		];

	/// <summary>Checks whether a numeric value beats the current record for the given element index and, if so, fires a <see cref="BackgroundWorker.ReportProgress(int, object)"/> event so the UI labels can be updated safely on the UI thread.</summary>
	/// <param name="elementIndex">Zero-based index of the orbital element.</param>
	/// <param name="value">The numeric value to compare against the current record.</param>
	/// <param name="stringValue">The original string value from the database to display in the UI.</param>
	/// <param name="designation">The readable designation of the asteroid.</param>
	/// <param name="isAscending">If <c>true</c>, checks for maximum; otherwise checks for minimum.</param>
	/// <param name="percent">Current scan percentage, forwarded in the progress report.</param>
	/// <remarks>This method runs on the <see cref="BackgroundWorker"/> DoWork thread. UI updates are performed through the <see cref="BackgroundWorker.ProgressChanged"/> event.</remarks>
	private void CheckAndReportRecord(int elementIndex, double value, string stringValue, string designation, bool isAscending, int percent)
	{
		// Check if this value is a new record
		bool isNewRecord = isAscending
			? value > recordValues[elementIndex]
			: value < recordValues[elementIndex];
		// If it's not a new record, simply return without doing anything
		if (!isNewRecord)
		{
			return;
		}
		// Store the new record value (safe on this thread; only DoWork touches recordValues)
		recordValues[elementIndex] = value;
		recordStringValues[elementIndex] = stringValue;
		// Report the update to the UI thread via ProgressChanged
		backgroundWorker.ReportProgress(
			percentProgress: percent,
			userState: new RecordProgressUpdate(ElementIndex: elementIndex, Value: value, StringValue: stringValue, Designation: designation));
	}

	/// <summary>Processes a single database entry and checks all orbital element values for records.</summary>
	/// <param name="rawLine">The raw fixed-width line from the MPCORB database.</param>
	/// <param name="isAscending">If <c>true</c>, looks for maximum values; otherwise looks for minimum values.</param>
	/// <param name="percent">Current scan percentage, forwarded to record progress reports.</param>
	/// <remarks>Parses the planetoid record and compares each numeric orbital element value against the current records. A progress report is fired immediately for each new record.</remarks>
	private void ProcessEntry(string rawLine, bool isAscending, int percent)
	{
		// Parse the raw line into a PlanetoidRecord
		PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: rawLine);
		// Skip empty records
		if (string.IsNullOrWhiteSpace(value: record.DesignationName))
		{
			return;
		}
		// Readable designation for progress reporting
		string designation = record.DesignationName;
		// Check each orbital element in turn. If parsing succeeds, compare against the current record and report if it's a new record. The element index corresponds to the order defined in the recordValues array and label arrays.		
		// Mean Anomaly at the Epoch
		if (double.TryParse(s: record.MeanAnomaly, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double meanAnomaly))
		{
			CheckAndReportRecord(elementIndex: 0, value: meanAnomaly, stringValue: record.MeanAnomaly, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Argument of the Perihelion
		if (double.TryParse(s: record.ArgPeri, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argPeri))
		{
			CheckAndReportRecord(elementIndex: 1, value: argPeri, stringValue: record.ArgPeri, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Longitude of the Ascending Node
		if (double.TryParse(s: record.LongAscNode, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longAscNode))
		{
			CheckAndReportRecord(elementIndex: 2, value: longAscNode, stringValue: record.LongAscNode, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Inclination to the Ecliptic
		if (double.TryParse(s: record.Incl, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double incl))
		{
			CheckAndReportRecord(elementIndex: 3, value: incl, stringValue: record.Incl, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Orbital Eccentricity
		if (double.TryParse(s: record.OrbEcc, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double orbEcc))
		{
			CheckAndReportRecord(elementIndex: 4, value: orbEcc, stringValue: record.OrbEcc, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Mean Daily Motion
		if (double.TryParse(s: record.Motion, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double motion))
		{
			CheckAndReportRecord(elementIndex: 5, value: motion, stringValue: record.Motion, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Semi-Major Axis
		if (double.TryParse(s: record.SemiMajorAxis, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis))
		{
			CheckAndReportRecord(elementIndex: 6, value: semiMajorAxis, stringValue: record.SemiMajorAxis, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Absolute Magnitude (H)
		if (double.TryParse(s: record.MagAbs, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double magAbs))
		{
			CheckAndReportRecord(elementIndex: 7, value: magAbs, stringValue: record.MagAbs, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Slope Parameter (G)
		if (double.TryParse(s: record.SlopeParam, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double slopeParam))
		{
			CheckAndReportRecord(elementIndex: 8, value: slopeParam, stringValue: record.SlopeParam, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Number of Oppositions
		if (int.TryParse(s: record.NumberOpposition, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int numOpposition))
		{
			CheckAndReportRecord(elementIndex: 9, value: numOpposition, stringValue: record.NumberOpposition, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Number of Observations
		if (int.TryParse(s: record.NumberObservation, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int numObservation))
		{
			CheckAndReportRecord(elementIndex: 10, value: numObservation, stringValue: record.NumberObservation, designation: designation, isAscending: isAscending, percent: percent);
		}
		// RMS Residual (Observation Span is intentionally skipped)
		if (double.TryParse(s: record.RmsResidual, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double rmsResidual))
		{
			CheckAndReportRecord(elementIndex: 11, value: rmsResidual, stringValue: record.RmsResidual, designation: designation, isAscending: isAscending, percent: percent);
		}
		// Note: Observation Span is not included in the record tracking since it can vary based on observation history rather than being an intrinsic orbital property.
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Initializes the form by clearing the status bar.</remarks>
	private void RecordsForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar when the form loads
		ClearStatusBar(label: labelInformation);
		// Ensure the Cancel button is disabled at startup
		toolStripButtonCancel.Enabled = false;
	}

	#endregion

	#region BackgroundWorker event handlers

	/// <summary>Performs the record scanning work on a background thread.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
	/// <remarks>Iterates through all database entries, processes each one for record detection, and reports progress as a percentage. The scan can be cancelled at any time. Progress bar updates are throttled to at most one per percent change.</remarks>
	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		// Determine scan mode (max or min) captured before the background thread starts
		bool isMax = (bool)(e.Argument ?? false);
		int total = planetoidsDatabase.Count;
		if (total == 0)
		{
			return;
		}
		// Iterate through all entries in the database
		int lastPercent = -1;
		for (int i = 0; i < total; i++)
		{
			// Check for cancellation
			if (isCancelled || backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
				break;
			}
			// Process this database entry
			string rawLine = planetoidsDatabase[index: i];
			int percent = (int)((i + 1) * 100L / total);
			// Wrap processing in a try-catch to handle any malformed lines without crashing the entire scan
			try
			{
				// Process the entry and check for records. Any new records will trigger progress reports to update the UI labels.
				ProcessEntry(rawLine: rawLine, isAscending: isMax, percent: percent);
			}
			// Catch any exceptions that occur during processing of this entry, log a warning, and continue with the next entry. This ensures that a single bad line does not stop the entire scan.
			catch (Exception ex)
			{
				logger.Warn(exception: ex, message: $"Skipping entry at index {i}: {ex.Message}");
			}
			// Report plain percentage progress only when the percentage changes
			if (percent != lastPercent)
			{
				backgroundWorker.ReportProgress(percentProgress: percent);
				lastPercent = percent;
			}
		}
	}

	/// <summary>Updates the progress bar and percent label, and optionally updates a record label pair, when the BackgroundWorker reports progress.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
	/// <remarks>Called on the UI thread by the BackgroundWorker. When <see cref="ProgressChangedEventArgs.UserState"/> is a <see cref="RecordProgressUpdate"/>, the corresponding designation and value labels are updated.</remarks>
	private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		// Update the progress bar value and text if the percentage has changed
		int percent = e.ProgressPercentage;
		if (kryptonProgressBar.Value != percent)
		{
			kryptonProgressBar.Value = percent;
			kryptonProgressBar.Text = $"{percent} %";
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)percent, progressMax: 100);
		}
		// If this progress report carries a record update, apply it to the labels
		if (e.UserState is RecordProgressUpdate update)
		{
			KryptonLabel[] designationLabels = GetDesignationLabels();
			KryptonLabel[] valueLabels = GetValueLabels();
			// Display the original string value from the database as-is
			designationLabels[update.ElementIndex].Values.Text = update.Designation;
			valueLabels[update.ElementIndex].Values.Text = update.StringValue;
		}
	}

	/// <summary>Handles scan completion, enabling the Start button and disabling the Cancel button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
	/// <remarks>Called on the UI thread after the BackgroundWorker finishes or is cancelled. Resets the UI state so the user can start a new scan.</remarks>
	private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		// Reset UI state to allow starting a new scan
		toolStripButtonCancel.Enabled = false;
		toolStripButtonStart.Enabled = true;
		toolStripButtonSortOrderAscending.Enabled = true;
		toolStripButtonSortOrderDescending.Enabled = true;
		toolStripDropDownButtonSaveList.Enabled = true;
		contextMenuSaveToFile.Enabled = true;
		// Update the progress bar text based on whether the scan was cancelled, completed with an error, or completed successfully
		if (e.Cancelled)
		{
			kryptonProgressBar.Text = "Scan cancelled";
			ClearStatusBar(label: labelInformation);
		}
		else if (e.Error != null)
		{
			logger.Error(exception: e.Error, message: e.Error.Message);
			kryptonProgressBar.Text = I18nStrings.ErrorCaption;
			ClearStatusBar(label: labelInformation);
		}
		else
		{
			kryptonProgressBar.Text = "100 %";
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the ToolStripButtonStart control. Resets results and starts the record detection scan.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Disables the Start button, enables the Cancel button, resets labels and record arrays, then launches the BackgroundWorker with the selected max/min mode.</remarks>
	private void Start_Click(object sender, EventArgs e)
	{
		// Ensure there is data to scan before starting
		if (planetoidsDatabase.Count == 0)
		{
			ShowErrorMessage(message: I18nStrings.MpcorbDatNotFoundText);
			return;
		}
		// Reset state
		isCancelled = false;
		ResetLabels();
		// Update UI for scan in progress
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0 %";
		toolStripButtonSortOrderAscending.Enabled = false;
		toolStripButtonSortOrderDescending.Enabled = false;
		toolStripDropDownButtonSaveList.Enabled = false;
		contextMenuSaveToFile.Enabled = false;

		// Wire up BackgroundWorker events and start the scan
		backgroundWorker.RunWorkerAsync(argument: toolStripButtonSortOrderDescending.Checked);
	}

	/// <summary>Handles the Click event of the ButtonCancel control. Cancels the ongoing record detection scan.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Sets the cancellation flag and requests the BackgroundWorker to stop. The UI state will be reset in the <see cref="BackgroundWorker_RunWorkerCompleted"/> handler.</remarks>
	private void Cancel_Click(object sender, EventArgs e)
	{
		// Set the cancellation flag and request the BackgroundWorker to cancel. The actual stopping will be handled cooperatively in the DoWork method, and the UI will be updated in the RunWorkerCompleted handler.
		isCancelled = true;
		backgroundWorker.CancelAsync();
		toolStripButtonCancel.Enabled = false;
	}

	/// <summary>Handles the Click event of the ToolStripButtonSortOrderAscending control. Selects ascending sort order and deselects descending sort order.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Ensures that exactly one of Ascending/Descending is selected at all times.</remarks>
	private void SetAscendingSortOrder_Click(object sender, EventArgs e) => toolStripButtonSortOrderDescending.Checked = !toolStripButtonSortOrderAscending.Checked;

	/// <summary>Handles the Click event of the ToolStripButtonSortOrderDescending control. Selects descending sort order and deselects ascending sort order.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Ensures that exactly one of Ascending/Descending is selected at all times.</remarks>
	private void SetDescendingSortOrder_Click(object sender, EventArgs e) => toolStripButtonSortOrderAscending.Checked = !toolStripButtonSortOrderDescending.Checked;

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanAnomalyAtTheEpoch. Shows the top ten records form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean anomaly at the epoch.</remarks>
	private void RecordsMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for mean anomaly at the epoch
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Mean anomaly at the epoch");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanAnomalyAtTheEpoch. Shows the top ten records form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the argument of the perihelion.</remarks>
	private void RecordsArgumentOfThePerihelion_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for the argument of the perihelion
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Argument of the perihelion");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsLongitudeOfTheAscendingNode. Shows the top ten records form for the longitude of the ascending node.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the longitude of the ascending node.</remarks>
	private void RecordsLongitudeOfTheAscendingNode_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for the longitude of the ascending node
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Longitude of the ascending node");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsInclination. Shows the top ten records form for inclination.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for inclination.</remarks>
	private void RecordsInclination_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for inclination
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Inclination");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsOrbitalEccentricity. Shows the top ten records form for orbital eccentricity.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for orbital eccentricity.</remarks>
	private void RecordsOrbitalEccentricity_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for orbital eccentricity
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Orbital eccentricity");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanDailyMotion. Shows the top ten records form for mean daily motion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean daily motion.</remarks>
	private void RecordsMeanDailyMotion_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for mean daily motion
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Mean daily motion");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSemiMajorAxis. Shows the top ten records form for semi-major axis.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for semi-major axis.</remarks>
	private void RecordsSemiMajorAxis_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for semi-major axis
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Semi-major axis");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsAbsoluteMagnitude. Shows the top ten records form for absolute magnitude.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for absolute magnitude.</remarks>
	private void RecordsAbsoluteMagnitude_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for absolute magnitude
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Absolute magnitude");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSlopeParameter. Shows the top ten records form for slope parameter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for slope parameter.</remarks>
	private void RecordsSlopeParameter_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for slope parameter
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Slope parameter");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfOppositions. Shows the top ten records form for number of oppositions.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of oppositions.</remarks>
	private void RecordsNumberOfOppositions_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for number of oppositions
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Number of oppositions");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfObservations. Shows the top ten records form for number of observations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of observations.</remarks>
	private void RecordsNumberOfObservations_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for number of observations
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "Number of observations");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsRmsResidual. Shows the top ten records form for RMS residual.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for RMS residual.</remarks>
	private void RecordsRmsResidual_Click(object sender, EventArgs e)
	{
		// Show the top ten records form for RMS residual
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: "r.m.s. residual");
		formRecordsTop10.TopMost = TopMost;
		_ = formRecordsTop10.ShowDialog(owner: this);
	}

	#endregion
}