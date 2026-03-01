using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>
/// Represents the form that scans all orbital elements for maximum or minimum record values.
/// </summary>
/// <remarks>
/// This form displays the record holder (asteroid with the highest or lowest value) for each
/// orbital element in the database. The user can choose between maximum and minimum records,
/// start and cancel the scan at any time.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class RecordsForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the records form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Stores the planetoids database.
	/// </summary>
	/// <remarks>
	/// This list holds a copy of all planetoid database entries for scanning.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>
	/// Indicates whether the scan operation has been cancelled by the user.
	/// </summary>
	/// <remarks>
	/// Set to true when the user clicks the Cancel button to stop an ongoing scan.
	/// </remarks>
	private bool isCancelled;

	/// <summary>
	/// Stores the current record values for each orbital element (max or min).
	/// </summary>
	/// <remarks>
	/// Index corresponds to element: 0=MeanAnomaly, 1=ArgPeri, 2=LongAscNode, 3=Incl,
	/// 4=OrbEcc, 5=Motion, 6=SemiMajorAxis, 7=MagAbs, 8=SlopeParam, 9=NumberOpposition,
	/// 10=NumberObservation, 11=ObsSpan, 12=RmsResidual.
	/// </remarks>
	private readonly double[] recordValues = new double[13];

	/// <summary>
	/// Holds a progress update for a single orbital element record, used to marshal
	/// label updates from the background thread to the UI thread via
	/// <see cref="BackgroundWorker.ReportProgress"/>.
	/// </summary>
	/// <param name="ElementIndex">Zero-based index of the orbital element.</param>
	/// <param name="Value">The new record value.</param>
	/// <param name="Designation">The readable designation of the record-holder asteroid.</param>
	private readonly record struct RecordProgressUpdate(int ElementIndex, double Value, string Designation);

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="RecordsForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components and wires up BackgroundWorker events.
	/// </remarks>
	public RecordsForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Wire up BackgroundWorker events once at construction time
#pragma warning disable CS8622
		backgroundWorker.DoWork += BackgroundWorker_DoWork;
		backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
		backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
#pragma warning restore CS8622
	}

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
	/// Fills the internal database list with a copy of the provided planetoid data.
	/// </summary>
	/// <param name="arrTemp">The list of raw planetoid data lines from the main form.</param>
	/// <remarks>
	/// This method must be called before starting the scan to provide the data source.
	/// The data is stored as a copy so changes in the main form do not affect the scan.
	/// </remarks>
	public void FillArray(List<string> arrTemp) => planetoidsDatabase = new List<string>(arrTemp);

	/// <summary>
	/// Resets all record labels to the initial state before starting a new scan.
	/// </summary>
	/// <remarks>
	/// Clears designation and value labels, and resets internal tracking arrays.
	/// </remarks>
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
		bool isMax = checkButtonMax.Checked;
		for (int i = 0; i < recordValues.Length; i++)
		{
			recordValues[i] = isMax ? double.MinValue : double.MaxValue;
		}
	}

	/// <summary>
	/// Gets the array of designation labels in element order.
	/// </summary>
	/// <returns>An array of <see cref="KryptonLabel"/> controls for designation display.</returns>
	/// <remarks>
	/// The array is ordered to match the orbital element order used in record tracking.
	/// </remarks>
	private KryptonLabel[] GetDesignationLabels() =>
	[
		labelDesignation01, labelDesignation02, labelDesignation03, labelDesignation04,
		labelDesignation05, labelDesignation06, labelDesignation07, labelDesignation08,
		labelDesignation09, labelDesignation10, labelDesignation11, labelDesignation12,
		labelDesignation13
	];

	/// <summary>
	/// Gets the array of value labels in element order.
	/// </summary>
	/// <returns>An array of <see cref="KryptonLabel"/> controls for value display.</returns>
	/// <remarks>
	/// The array is ordered to match the orbital element order used in record tracking.
	/// </remarks>
	private KryptonLabel[] GetValueLabels() =>
	[
		labelValue01, labelValue02, labelValue03, labelValue04,
		labelValue05, labelValue06, labelValue07, labelValue08,
		labelValue09, labelValue10, labelValue11, labelValue12,
		labelValue13
	];

	/// <summary>
	/// Checks whether a numeric value beats the current record for the given element index
	/// and, if so, fires a <see cref="BackgroundWorker.ReportProgress"/> event so the UI
	/// labels can be updated safely on the UI thread.
	/// </summary>
	/// <param name="elementIndex">Zero-based index of the orbital element.</param>
	/// <param name="value">The numeric value to compare against the current record.</param>
	/// <param name="designation">The readable designation of the asteroid.</param>
	/// <param name="isMax">If <c>true</c>, checks for maximum; otherwise checks for minimum.</param>
	/// <param name="percent">Current scan percentage, forwarded in the progress report.</param>
	/// <remarks>
	/// This method runs on the <see cref="BackgroundWorker"/> DoWork thread.
	/// UI updates are performed through <see cref="BackgroundWorker_ProgressChanged"/>.
	/// </remarks>
	private void CheckAndReportRecord(int elementIndex, double value, string designation, bool isMax, int percent)
	{
		// Check if this value is a new record
		bool isNewRecord = isMax
			? value > recordValues[elementIndex]
			: value < recordValues[elementIndex];

		if (!isNewRecord)
		{
			return;
		}

		// Store the new record value (safe on this thread; only DoWork touches recordValues)
		recordValues[elementIndex] = value;

		// Report the update to the UI thread via ProgressChanged
		backgroundWorker.ReportProgress(
			percentProgress: percent,
			userState: new RecordProgressUpdate(ElementIndex: elementIndex, Value: value, Designation: designation));
	}

	/// <summary>
	/// Processes a single database entry and checks all orbital element values for records.
	/// </summary>
	/// <param name="rawLine">The raw fixed-width line from the MPCORB database.</param>
	/// <param name="isMax">If <c>true</c>, looks for maximum values; otherwise looks for minimum values.</param>
	/// <param name="percent">Current scan percentage, forwarded to record progress reports.</param>
	/// <remarks>
	/// Parses the planetoid record and compares each numeric orbital element value
	/// against the current records. A progress report is fired immediately for each new record.
	/// </remarks>
	private void ProcessEntry(string rawLine, bool isMax, int percent)
	{
		// Parse the raw line into a PlanetoidRecord
		PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: rawLine);

		// Skip empty records
		if (string.IsNullOrWhiteSpace(value: record.DesignationName))
		{
			return;
		}

		string designation = record.DesignationName;

		// Mean Anomaly at the Epoch
		if (double.TryParse(s: record.MeanAnomaly, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double meanAnomaly))
		{
			CheckAndReportRecord(elementIndex: 0, value: meanAnomaly, designation: designation, isMax: isMax, percent: percent);
		}

		// Argument of Perihelion
		if (double.TryParse(s: record.ArgPeri, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argPeri))
		{
			CheckAndReportRecord(elementIndex: 1, value: argPeri, designation: designation, isMax: isMax, percent: percent);
		}

		// Longitude of the Ascending Node
		if (double.TryParse(s: record.LongAscNode, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longAscNode))
		{
			CheckAndReportRecord(elementIndex: 2, value: longAscNode, designation: designation, isMax: isMax, percent: percent);
		}

		// Inclination to the Ecliptic
		if (double.TryParse(s: record.Incl, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double incl))
		{
			CheckAndReportRecord(elementIndex: 3, value: incl, designation: designation, isMax: isMax, percent: percent);
		}

		// Orbital Eccentricity
		if (double.TryParse(s: record.OrbEcc, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double orbEcc))
		{
			CheckAndReportRecord(elementIndex: 4, value: orbEcc, designation: designation, isMax: isMax, percent: percent);
		}

		// Mean Daily Motion
		if (double.TryParse(s: record.Motion, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double motion))
		{
			CheckAndReportRecord(elementIndex: 5, value: motion, designation: designation, isMax: isMax, percent: percent);
		}

		// Semi-Major Axis
		if (double.TryParse(s: record.SemiMajorAxis, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis))
		{
			CheckAndReportRecord(elementIndex: 6, value: semiMajorAxis, designation: designation, isMax: isMax, percent: percent);
		}

		// Absolute Magnitude (H)
		if (double.TryParse(s: record.MagAbs, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double magAbs))
		{
			CheckAndReportRecord(elementIndex: 7, value: magAbs, designation: designation, isMax: isMax, percent: percent);
		}

		// Slope Parameter (G)
		if (double.TryParse(s: record.SlopeParam, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double slopeParam))
		{
			CheckAndReportRecord(elementIndex: 8, value: slopeParam, designation: designation, isMax: isMax, percent: percent);
		}

		// Number of Oppositions
		if (int.TryParse(s: record.NumberOpposition, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int numOpposition))
		{
			CheckAndReportRecord(elementIndex: 9, value: numOpposition, designation: designation, isMax: isMax, percent: percent);
		}

		// Number of Observations
		if (int.TryParse(s: record.NumberObservation, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int numObservation))
		{
			CheckAndReportRecord(elementIndex: 10, value: numObservation, designation: designation, isMax: isMax, percent: percent);
		}

		// Observation Span
		if (int.TryParse(s: record.ObsSpan, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int obsSpan))
		{
			CheckAndReportRecord(elementIndex: 11, value: obsSpan, designation: designation, isMax: isMax, percent: percent);
		}

		// RMS Residual
		if (double.TryParse(s: record.RmsResidual, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double rmsResidual))
		{
			CheckAndReportRecord(elementIndex: 12, value: rmsResidual, designation: designation, isMax: isMax, percent: percent);
		}
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Initializes the form by clearing the status bar.
	/// </remarks>
	private void RecordsForm_Load(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

	#endregion

	#region BackgroundWorker event handlers

	/// <summary>
	/// Performs the record scanning work on a background thread.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// Iterates through all database entries, processes each one for record detection,
	/// and reports progress as a percentage. The scan can be cancelled at any time.
	/// Progress bar updates are throttled to at most one per percent change.
	/// </remarks>
	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		// Determine scan mode (max or min) captured before the background thread starts
		bool isMax = (bool)(e.Argument ?? true);
		int total = planetoidsDatabase.Count;
		if (total == 0)
		{
			return;
		}

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
			try
			{
				ProcessEntry(rawLine: rawLine, isMax: isMax, percent: percent);
			}
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

	/// <summary>
	/// Updates the progress bar and percent label, and optionally updates a record label pair,
	/// when the BackgroundWorker reports progress.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// Called on the UI thread by the BackgroundWorker.
	/// When <see cref="ProgressChangedEventArgs.UserState"/> is a <see cref="RecordProgressUpdate"/>,
	/// the corresponding designation and value labels are updated.
	/// </remarks>
	private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		int percent = e.ProgressPercentage;
		if (progressBar.Value != percent)
		{
			progressBar.Value = percent;
		}

		labelPercent.Values.Text = $"{percent} %";

		// If this progress report carries a record update, apply it to the labels
		if (e.UserState is RecordProgressUpdate update)
		{
			KryptonLabel[] designationLabels = GetDesignationLabels();
			KryptonLabel[] valueLabels = GetValueLabels();
			string formattedValue = update.Value.ToString(format: "G", provider: CultureInfo.InvariantCulture);
			designationLabels[update.ElementIndex].Values.Text = update.Designation;
			valueLabels[update.ElementIndex].Values.Text = formattedValue;
		}
	}

	/// <summary>
	/// Handles scan completion, enabling the Start button and disabling the Cancel button.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// Called on the UI thread after the BackgroundWorker finishes or is cancelled.
	/// Resets the UI state so the user can start a new scan.
	/// </remarks>
	private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		buttonCancel.Enabled = false;
		progressBar.Enabled = false;
		buttonStart.Enabled = true;
		checkButtonMax.Enabled = true;
		checkButtonMin.Enabled = true;
		groupBoxRecordType.Enabled = true;

		if (e.Cancelled)
		{
			labelPercent.Values.Text = I18nStrings.DownloadCancelledText;
			ClearStatusBar(label: labelInformation);
		}
		else if (e.Error != null)
		{
			logger.Error(exception: e.Error, message: e.Error.Message);
			labelPercent.Values.Text = I18nStrings.ErrorCaption;
			ClearStatusBar(label: labelInformation);
		}
		else
		{
			labelPercent.Values.Text = "100 %";
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the ButtonStart control.
	/// Resets results and starts the record detection scan.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Disables the Start button, enables the Cancel button, resets labels and record arrays,
	/// then launches the BackgroundWorker with the selected max/min mode.
	/// </remarks>
	private void ButtonStart_Click(object sender, EventArgs e)
	{
		if (planetoidsDatabase.Count == 0)
		{
			ShowErrorMessage(message: I18nStrings.MpcorbDatNotFoundText);
			return;
		}

		// Reset state
		isCancelled = false;
		ResetLabels();

		// Update UI for scan in progress
		buttonStart.Enabled = false;
		buttonCancel.Enabled = true;
		progressBar.Enabled = true;
		progressBar.Value = 0;
		progressBar.Text = "0 %";
		labelPercent.Values.Text = "0 %";
		checkButtonMax.Enabled = false;
		checkButtonMin.Enabled = false;
		groupBoxRecordType.Enabled = false;

		// Wire up BackgroundWorker events and start the scan
		backgroundWorker.RunWorkerAsync(argument: checkButtonMax.Checked);
	}

	/// <summary>
	/// Handles the Click event of the ButtonCancel control.
	/// Cancels the ongoing record detection scan.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Sets the cancellation flag and requests the BackgroundWorker to stop.
	/// The UI state will be reset in the <see cref="BackgroundWorker_RunWorkerCompleted"/> handler.
	/// </remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		isCancelled = true;
		backgroundWorker.CancelAsync();
		buttonCancel.Enabled = false;
	}

	/// <summary>
	/// Handles the Click event of the CheckButtonMax control.
	/// Selects maximum record mode and deselects minimum mode.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Ensures that exactly one of Max/Min is selected at all times.
	/// </remarks>
	private void CheckButtonMax_Click(object sender, EventArgs e) => checkButtonMin.Checked = !checkButtonMax.Checked;

	/// <summary>
	/// Handles the Click event of the CheckButtonMin control.
	/// Selects minimum record mode and deselects maximum mode.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Ensures that exactly one of Max/Min is selected at all times.
	/// </remarks>
	private void CheckButtonMin_Click(object sender, EventArgs e) => checkButtonMax.Checked = !checkButtonMin.Checked;

	#endregion
}
