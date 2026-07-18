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

using Settings = Planetoid_DB.Properties.Settings;

namespace Planetoid_DB;

/// <summary>Form for comparing two MPCORB.DAT files and displaying the differences.</summary>
/// <remarks>This form allows users to select two MPCORB.DAT files, compare their contents, and view the differences in a user-friendly interface. The form includes functionality for saving the comparison results in various formats and navigating to specific records in the main form based on the differences identified.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseDifferencesForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's virtual results list.</remarks>
	protected override ListView? ExportListView => listViewResults;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Database Differences";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => fileName;

	/// <summary>Gets the virtual row provider used during export operations.</summary>
	/// <remarks>Overrides the base export row provider to read items from the virtual list.</remarks>
	protected override Func<int, ListViewItem>? ExportVirtualRowProvider => GetVirtualListViewItem;

	#endregion

	/// <summary>NLog logger for logging messages and errors.</summary>
	/// <remarks>This logger is used to log messages and errors that occur within the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the name of the file that stores the database differences.</summary>
	/// <remarks>This field stores the name of the file where the database differences are saved. It is used to identify the file when performing read or write operations related to database differences.</remarks>
	private readonly string fileName = "database-differences";

	/// <summary>Gets or sets the background worker used for asynchronous operations.</summary>
	/// <remarks>This field is initialized to null and should be assigned a valid instance of BackgroundWorker before use. Ensure that the worker is properly configured to handle events such as DoWork, RunWorkerCompleted, and ProgressChanged.</remarks>
	private BackgroundWorker worker = null!;

	/// <summary>Gets or sets the path of the first file.</summary>
	/// <remarks>This field stores the path of the first MPCORB.DAT file to be compared.</remarks>
	private string pathFile1 = string.Empty;

	/// <summary>Gets or sets the path of the second file.</summary>
	/// <remarks>This field stores the path of the second MPCORB.DAT file to be compared.</remarks>
	private string pathFile2 = string.Empty;

	/// <summary>Tracks the number of records that have been added, deleted, or changed during a data operation.</summary>
	/// <remarks>These fields are used to monitor the state of records as operations are performed. Their values are updated to reflect the outcome of add, delete, or change actions, enabling consumers to determine how many records were affected.</remarks>
	private int addedRecords, deletedRecords, changedRecords;

	/// <summary>Represents the result of a comparison, including the index, designation, and the difference observed.</summary>
	/// <param name="Index">The index of the item in the comparison, indicating its position in the original dataset.</param>
	/// <param name="Designation">The designation or label associated with the item being compared, providing context for the comparison.</param>
	/// <param name="Difference">The difference observed between the compared items, detailing the nature of the discrepancy.</param>
	/// <remarks>This record struct is used to encapsulate the result of a comparison between two items, including their index, designation, and the observed difference.</remarks>
	private record struct DifferenceResult(string Index, string Designation, string Difference);

	/// <summary>Contains the list of difference results that have been computed.</summary>
	/// <remarks>This field stores the results generated by difference computations. The list is initialized as empty and is populated during relevant operations. Access to this field is typically provided through a property or method for further analysis or reporting.</remarks>
	private readonly List<DifferenceResult> differenceResults = [];

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>This property provides access to the status label used for displaying information in the form's status strip.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="DatabaseDifferencesForm"/> class.</summary>
	/// <remarks>This constructor sets up the form and initializes the background worker.</remarks>
	public DatabaseDifferencesForm()
	{
		// Initialize the form's components, setting up the user interface elements and layout as defined in the designer file.
		InitializeComponent();
		// Initialize the background worker for asynchronous operations
		InitializeBackgroundWorker();
	}

	#endregion

	#region helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom debugger display string.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Initializes the background worker for performing the database comparison.</summary>
	/// <remarks>This method sets up the background worker with the necessary event handlers.</remarks>
	private void InitializeBackgroundWorker()
	{
		// Create a new instance of BackgroundWorker and configure it to support progress reporting and cancellation
		worker = new BackgroundWorker
		{
			WorkerReportsProgress = true,
			WorkerSupportsCancellation = true
		};
		// Attach event handlers for the DoWork, ProgressChanged, and RunWorkerCompleted events
		worker.DoWork += Worker_DoWork;
		worker.ProgressChanged += Worker_ProgressChanged;
		worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
	}

	/// <summary>Compares two PlanetoidRecord objects and returns a string describing the differences.</summary>
	/// <param name="r1">The first PlanetoidRecord to compare.</param>
	/// <param name="r2">The second PlanetoidRecord to compare.</param>
	/// <returns>A string describing the differences between the two records, or an empty string if they are identical.</returns>
	private static string CompareRecords(PlanetoidRecord r1, PlanetoidRecord r2)
	{
		// Create a list to hold the differences between the two records
		List<string> diffs = [];
		// Compare the Epoch field of the two records and add a description of any differences to the list
		if (r1.Epoch != r2.Epoch)
		{
			diffs.Add(item: $"Epoch: {r1.Epoch} -> {r2.Epoch}");
		}
		// Compare the Mean Anomaly field of the two records and add a description of any differences to the list
		if (r1.MeanAnomaly != r2.MeanAnomaly)
		{
			diffs.Add(item: $"MA: {r1.MeanAnomaly} -> {r2.MeanAnomaly}");
		}
		// Compare the Argument of Perihelion field of the two records and add a description of any differences to the list
		if (r1.ArgPeri != r2.ArgPeri)
		{
			diffs.Add(item: $"ArgPeri: {r1.ArgPeri} -> {r2.ArgPeri}");
		}
		// Compare the Longitude of Ascending Node field of the two records and add a description of any differences to the list
		if (r1.LongAscNode != r2.LongAscNode)
		{
			diffs.Add(item: $"LAN: {r1.LongAscNode} -> {r2.LongAscNode}");
		}
		// Compare the Inclination field of the two records ListViewResults_DoubleClickand add a description of any differences to the list
		if (r1.Incl != r2.Incl)
		{
			diffs.Add(item: $"Incl: {r1.Incl} -> {r2.Incl}");
		}
		// Compare the Orbital Eccentricity field of the two records and add a description of any differences to the list
		if (r1.OrbEcc != r2.OrbEcc)
		{
			diffs.Add(item: $"Ecc: {r1.OrbEcc} -> {r2.OrbEcc}");
		}
		// Compare the Semi-Major Axis field of the two records and add a description of any differences to the list
		if (r1.SemiMajorAxis != r2.SemiMajorAxis)
		{
			diffs.Add(item: $"a: {r1.SemiMajorAxis} -> {r2.SemiMajorAxis}");
		}
		// Compare the Absolute Magnitude field of the two records and add a description of any differences to the list
		if (r1.MagAbs != r2.MagAbs)
		{
			diffs.Add(item: $"H: {r1.MagAbs} -> {r2.MagAbs}");
		}
		// Compare the Slope Parameter field of the two records and add a description of any differences to the list
		if (r1.SlopeParam != r2.SlopeParam)
		{
			diffs.Add(item: $"G: {r1.SlopeParam} -> {r2.SlopeParam}");
		}
		// Compare the Designation Name field of the two records and add a description of any differences to the list
		if (r1.DesignationName != r2.DesignationName)
		{
			diffs.Add(item: $"Desig: {r1.DesignationName} -> {r2.DesignationName}");
		}
		// Compare the Mean Daily Motion field of the two records and add a description of any differences to the list
		if (r1.Motion != r2.Motion)
		{
			diffs.Add(item: $"n: {r1.Motion} -> {r2.Motion}");
		}
		// Compare the Reference field of the two records and add a description of any differences to the list
		if (r1.Ref != r2.Ref)
		{
			diffs.Add(item: $"Ref: {r1.Ref} -> {r2.Ref}");
		}
		// Compare the Number of Oppositions field of the two records and add a description of any differences to the list
		if (r1.NumberOpposition != r2.NumberOpposition)
		{
			diffs.Add(item: $"Opps: {r1.NumberOpposition} -> {r2.NumberOpposition}");
		}
		// Compare the Number of Observations field of the two records and add a description of any differences to the list
		if (r1.NumberObservation != r2.NumberObservation)
		{
			diffs.Add(item: $"Obs: {r1.NumberObservation} -> {r2.NumberObservation}");
		}
		// Compare the Observation Span field of the two records and add a description of any differences to the list
		if (r1.ObsSpan != r2.ObsSpan)
		{
			diffs.Add(item: $"ObsSpan: {r1.ObsSpan} -> {r2.ObsSpan}");
		}
		// Compare the R.M.S. Residual field of the two records and add a description of any differences to the list
		if (r1.RmsResidual != r2.RmsResidual)
		{
			diffs.Add(item: $"rms: {r1.RmsResidual} -> {r2.RmsResidual}");
		}
		// Compare the Computer Name field of the two records and add a description of any differences to the list
		if (r1.ComputerName != r2.ComputerName)
		{
			diffs.Add(item: $"Computer: {r1.ComputerName} -> {r2.ComputerName}");
		}
		// Compare the Flags field of the two records and add a description of any differences to the list
		if (r1.Flags != r2.Flags)
		{
			diffs.Add(item: $"Flags: {r1.Flags} -> {r2.Flags}");
		}
		// Compare the Observation Last Date field of the two records and add a description of any differences to the list
		if (r1.ObservationLastDate != r2.ObservationLastDate)
		{
			diffs.Add(item: $"LastObs: {r1.ObservationLastDate} -> {r2.ObservationLastDate}");
		}
		// Join the list of differences into a single string separated by semicolons and return it; if there are no differences, return an empty string
		return diffs.Count > 0 ? string.Join(separator: "; ", values: diffs) : string.Empty;
	}

	/// <summary>Navigates to the corresponding record in the main form based on the user's selection in the ListView.</summary>
	/// <remarks>If no record is selected, a warning message is displayed. If the selected record has been deleted, a notification is shown. Otherwise, the method attempts to jump to the selected record in the main form. This method is intended to be used in response to user actions that require focusing on a specific record from a list of database differences.</remarks>
	private void GoToObject()
	{
		// Check if there are any selected indices in the ListView, and if not, return early to prevent errors; if there is a selected index, retrieve the corresponding DifferenceResult and either show a message if the record was deleted or jump to the record in the main form if it still exists
		if (listViewResults.SelectedIndices.Count == 0)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "Please select a record to jump to.", caption: "No Record Selected", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Get the first selected index from the ListView and check if it is within the bounds of the difference results list; if so, retrieve the corresponding DifferenceResult and determine whether to show a message about a deleted record or to jump to the record in the main form based on the type of difference
		int selectedIndex = listViewResults.SelectedIndices[index: 0];
		if (selectedIndex >= 0 && selectedIndex < differenceResults.Count)
		{
			DifferenceResult result = differenceResults[index: selectedIndex];
			if (result.Difference.Equals(value: "Deleted record", comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				_ = KryptonMessageBox.Show(owner: this, text: "The selected record has been deleted and is no longer available.", caption: "Record Deleted", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			}
			else
			{
				// Attempt to find the main form of the application and, if it exists, call a method to jump to the record corresponding to the selected difference result based on its index and designation
				if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
				{
					Close();
					mainForm.JumpToRecord(index: result.Index, designation: result.Designation);
				}
			}
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the <see cref="Form.Load"/> event for the <see cref="DatabaseDifferencesForm"/>. Initializes default file paths and updates the initial UI state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>This method initializes the form with default file paths and updates the UI elements accordingly.</remarks>
	private void DatabaseDifferencesForm_Load(object sender, EventArgs e)
	{
		// Default to the currently configured MPCORB file
		pathFile1 = Settings.Default.systemFilenameMpcorbDat;
		// Check if the default file exists and update the label accordingly
		if (!File.Exists(path: pathFile1))
		{
			pathFile1 = string.Empty;
			kryptonLabelFile1.Text = "No file selected";
		}
		else
		{
			kryptonLabelFile1.Text = pathFile1;
		}
		// Initialize the second file path and label
		pathFile2 = string.Empty;
		kryptonLabelFile2.Text = "No file selected";
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for the file selection button, allowing the user to select a file from the system.</summary>
	/// <remarks>The method opens a file dialog with a filter for MPCORB files and updates the label with the selected file path if a file is chosen.</remarks>
	/// <param name="sender">The source of the event, typically the button that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ButtonSelectFile1_Click(object sender, EventArgs e)
	{
		// Open a file dialog to allow the user to select the first MPCORB file for comparison
		using OpenFileDialog dlg = new();
		// Set the filter to show only .DAT files and all files, and set the title of the dialog
		dlg.Filter = "MPCORB Files (*.DAT)|*.DAT|All Files (*.*)|*.*";
		dlg.Title = "Select Reference MPCORB.DAT";
		// Show the dialog and if the user selects a file, update the path and label with the selected file path
		if (dlg.ShowDialog(owner: this) == DialogResult.OK)
		{
			pathFile1 = dlg.FileName;
			kryptonLabelFile1.Text = pathFile1;
		}
	}

	/// <summary>Handles the click event for the file selection button, allowing the user to select a file from the system.</summary>
	/// <remarks>The method opens a file dialog with a filter for MPCORB files and updates the label with the selected file path if a file is chosen.</remarks>
	/// <param name="sender">The source of the event, typically the button that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ButtonSelectFile2_Click(object sender, EventArgs e)
	{
		// Open a file dialog to allow the user to select the second MPCORB file for comparison
		using OpenFileDialog dlg = new();
		// Set the filter to show only .DAT files and all files, and set the title of the dialog
		dlg.Filter = "MPCORB Files (*.DAT)|*.DAT|All Files (*.*)|*.*";
		dlg.Title = "Select Comparison MPCORB.DAT";
		// Show the dialog and if the user selects a file, update the path and label with the selected file path
		if (dlg.ShowDialog(owner: this) == DialogResult.OK)
		{
			pathFile2 = dlg.FileName;
			kryptonLabelFile2.Text = pathFile2;
		}
	}

	/// <summary>Handles the click event for the compare button, initiating the comparison of the selected MPCORB files.</summary>
	/// <param name="sender">The source of the event, typically the button that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>This method validates the selected files and initiates the comparison process by starting the background worker.</remarks>
	private void ButtonCompare_Click(object sender, EventArgs e)
	{
		// Reset the counters for added, deleted, and changed records before starting the comparison
		addedRecords = deletedRecords = changedRecords = 0;
		// Validate that both file paths are set and that the files exist before starting the comparison
		if (string.IsNullOrEmpty(value: pathFile1) || !File.Exists(path: pathFile1))
		{
			// Show an error message if the first file is not valid and return early to prevent starting the comparison
			ShowErrorMessage(message: "Please select a valid reference file (File 1).");
			return;
		}
		// Show an error message if the second file is not valid and return early to prevent starting the comparison
		if (string.IsNullOrEmpty(value: pathFile2) || !File.Exists(path: pathFile2))
		{
			// Show an error message if the second file is not valid and return early to prevent starting the comparison
			ShowErrorMessage(message: "Please select a valid comparison file (File 2).");
			return;
		}
		// Get the last write times of both files to determine if they are identical or to establish which one is newer for comparison purposes
		DateTime date1 = File.GetLastWriteTime(path: pathFile1);
		DateTime date2 = File.GetLastWriteTime(path: pathFile2);
		// If the last write times of both files are identical, show an informational message and abort the comparison since the file contents are likely the same
		if (date1 == date2)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "The file dates of both files are identical. The file contents of file 1 and file 2 are the same. Further comparison is aborted.", caption: "Notice", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Determine if file 1 is newer than file 2 based on the last write times, which will be used to determine the direction of the comparison (i.e., whether differences should be reported as added or deleted records)
		bool file1IsNewer = date1 > date2;
		// Clear previous comparison results and reset the UI elements before starting a new comparison
		differenceResults.Clear();
		listViewResults.VirtualListSize = 0;
		listViewResults.Items.Clear();
		// Reset the counters for added, deleted, and changed records
		kryptonProgressBar.Value = 0;
		toolStripButtonCompare.Enabled = false;
		kryptonButtonSelectFile1.Enabled = false;
		kryptonButtonSelectFile2.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		// Start the background worker to perform the comparison of the selected files, passing the file paths and the date comparison flag
		worker.RunWorkerAsync(argument: new object[] { pathFile1, pathFile2, file1IsNewer });
	}

	/// <summary>Handles the click event for the cancel button, allowing the user to cancel the ongoing comparison or close the form if no comparison is in progress.</summary>
	/// <param name="sender">The source of the event, typically the button that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	/// <remarks>This event is triggered when the user clicks the cancel button, allowing the ongoing comparison to be canceled or the form to be closed if no comparison is in progress.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// If the background worker is currently busy with a comparison, request cancellation and disable the cancel button to prevent multiple clicks
		if (worker.IsBusy)
		{
			worker.CancelAsync();
			toolStripButtonCancel.Enabled = false; // Prevent multiple clicks
		}
		else
		{
			Close();
		}
	}

	/// <summary>Handles the click event for the button that displays a message box containing abbreviations used in the Designation and Difference columns.</summary>
	/// <remarks>The message box presents a predefined list of abbreviations and their meanings to assist users in understanding column values.</remarks>
	/// <param name="sender">The source of the event, typically the button control that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void KryptonButtonNoteAbbreviations_Click(object sender, EventArgs e)
	{
		string message = "Abbreviations used in the Designation and Difference columns:\n\n" +
						 "Epoch - Epoch\n" +
						 "MA - Mean Anomaly\n" +
						 "ArgPeri - Argument of the perihelion\n" +
						 "LAN - Longitude of the Ascending Node\n" +
						 "Inc - Inclination\n" +
						 "Ecc - Eccentricity\n" +
						 "a - Semi-Major Axis\n" +
						 "H - Absolute Magnitude\n" +
						 "G - Slope Parameter\n" +
						 "Desig - Readable Designation\n" +
						 "n - Mean Daily Motion\n" +
						 "Ref - Reference\n" +
						 "Opps - Number of Oppositions\n" +
						 "Obs - Number of Observations\n" +
						 "ObsSpan - Observation Span\n" +
						 "rms - R.M.S. Residual\n" +
						 "Computer - Computer Name\n" +
						 "Flags - 4-Hexdigit Flag\n" +
						 "LastObs - Date of the Last Observation";
		_ = KryptonMessageBox.Show(owner: this, text: message, caption: "Abbreviations", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the Krypton button and initiates navigation to a specific object.</summary>
	/// <remarks>Use this method to respond to user interactions that require navigating to a particular object
	/// within the form. This method is intended for UI event handling scenarios.</remarks>
	/// <param name="sender">The source of the event, typically the Krypton button that was clicked.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void KryptonButtonGoto_Click(object sender, EventArgs e) => GoToObject();

	#endregion

	#region BackgroundWorker event handlers

	/// <summary>Handles the DoWork event of the background worker, performing the comparison of the selected MPCORB files in a background thread.</summary>
	/// <param name="sender">The source of the event, typically the background worker.</param>
	/// <param name="e">The event data associated with the DoWork event.</param>
	/// <remarks>This event is triggered when the background worker starts its task, allowing the comparison of the selected MPCORB files to be performed in a background thread.</remarks>
	private void Worker_DoWork(object? sender, DoWorkEventArgs e)
	{
		// Validate that the argument is an array of objects containing the file paths and the date comparison flag
		if (e.Argument is not object[] args || args.Length < 3)
		{
			return;
		}
		// Extract the file paths and the flag from the argument array
		string p1 = (string)args[0];
		string p2 = (string)args[1];
		bool file1IsNewer = (bool)args[2];
		// Load File 1 into Dictionary
		Dictionary<string, PlanetoidRecord> records1 = [];
		// Report progress to the UI that the reference file is being loaded
		worker.ReportProgress(percentProgress: 0, userState: "Loading Reference File...");
		// Read each line from the first file and parse it into a PlanetoidRecord, storing it in a dictionary for quick lookup by designation name
		foreach (string line in File.ReadLines(path: p1))
		{
			// Check for cancellation before processing each line to allow the user to cancel the operation if needed
			if (worker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}
			// Only process lines that are long enough to be valid records and do not start with a comment character
			if (line.Length > 200 && !line.StartsWith(value: '#'))
			{
				// Parse the line into a PlanetoidRecord and add it to the dictionary if it has a valid designation name
				PlanetoidRecord record = PlanetoidRecord.Parse(rawLine: line);
				if (!string.IsNullOrEmpty(value: record.DesignationName))
				{
					records1[key: record.DesignationName] = record;
				}
			}
		}
		// Report progress to the UI that the files are being compared
		worker.ReportProgress(percentProgress: 0, userState: "Comparing Files...");
		// Estimate total lines for file 2
		long totalLinesFile2 = 0;
		// Attempt to estimate the total number of lines in the second file for progress reporting
		try
		{
			// Use a StreamReader to read through the second file and count the total number of lines, which will be used for progress reporting during the comparison
			using StreamReader r = new(path: p2);
			// Read through the file line by line, incrementing the total line count until the end of the file is reached
			while (r.ReadLine() != null)
			{
				totalLinesFile2++;
			}
		}
		// Catch potential exceptions that may occur while reading the file, such as I/O errors or access issues, and log them using the NLog logger
		catch (IOException ex)
		{
			logger.Error(exception: ex, message: "I/O error while estimating total lines for file 2 from path '{FilePath}'.", args: p2);
			ShowErrorMessage(message: "I/O error while estimating total lines for file 2 from path '{FilePath}'.");
		}
		catch (UnauthorizedAccessException ex)
		{
			logger.Error(exception: ex, message: "Access denied while estimating total lines for file 2 from path '{FilePath}'.", args: p2);
			ShowErrorMessage(message: $"Access denied while estimating total lines for file 2 from path '{p2}'.");
		}
		// Initialize counters and batch results for the comparison process
		long currentLine = 0;
		List<DifferenceResult> batchResults = [];
		// Set up timing for progress reporting to avoid excessive updates while processing large files
		long lastReportTicks = DateTime.Now.Ticks;
		long reportIntervalTicks = TimeSpan.FromMilliseconds(milliseconds: 100).Ticks;
		// Read File 2 and compare with records from File 1
		using (StreamReader reader = new(path: p2))
		{
			// Read each line from the second file and compare it against the records from the first file, tracking differences and reporting progress periodically
			string? line;
			while ((line = reader.ReadLine()) != null)
			{
				// Check for cancellation before processing each line to allow the user to cancel the operation if needed
				if (worker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				// Increment the current line count for progress reporting purposes
				currentLine++;
				// Only process lines that are long enough to be valid records and do not start with a comment character
				if (line.Length >= 200 && !line.StartsWith(value: '#'))
				{
					// Parse the line into a PlanetoidRecord and compare it against the corresponding record from the first file if it exists, tracking any differences or added records
					PlanetoidRecord record2 = PlanetoidRecord.Parse(rawLine: line);
					if (!string.IsNullOrEmpty(value: record2.DesignationName))
					{
						// Check if the record from file 2 exists in the dictionary of records from file 1, and if so, compare the two records for differences; if not, it is an added record
						if (records1.TryGetValue(key: record2.DesignationName, value: out PlanetoidRecord record1))
						{
							string diff = file1IsNewer ? CompareRecords(record2, record1) : CompareRecords(record1, record2);
							// If there are differences between the two records, add a DifferenceResult to the batch results and increment the changed records counter; then remove the record from the dictionary to track deleted records
							if (!string.IsNullOrEmpty(value: diff))
							{
								batchResults.Add(item: new DifferenceResult(Index: record2.Index, Designation: record2.DesignationName, Difference: diff));
								changedRecords++;
							}
							records1.Remove(key: record2.DesignationName);
						}
						// If the record from file 2 does not exist in the dictionary of records from file 1, it is considered an added record (if file 1 is newer) or a deleted record (if file 2 is newer); add a DifferenceResult for this record and increment the respective counter, then remove it from the dictionary to track remaining records
						else
						{
							string diffText = file1IsNewer ? "Deleted record" : "Added record";
							batchResults.Add(item: new DifferenceResult(Index: record2.Index, Designation: record2.DesignationName, Difference: diffText));
							if (file1IsNewer)
							{
								deletedRecords++;
							}
							else
							{
								addedRecords++;
							}
							records1.Remove(key: record2.DesignationName);
						}
					}
				}
				// Periodically report progress to the UI with the current batch of differences, using timing to avoid excessive updates while processing large files; also report progress when the last line is reached to ensure the final batch is sent
				long currentTicks = DateTime.Now.Ticks;
				// Check if the time since the last progress report exceeds the defined interval or if the current line is the last line of file 2, and if so, report progress with the current batch of differences and reset the batch results and timing for the next report
				if (currentTicks - lastReportTicks > reportIntervalTicks || currentLine == totalLinesFile2)
				{
					int percent = totalLinesFile2 > 0 ? (int)((double)currentLine / totalLinesFile2 * 100) : 0;
					worker.ReportProgress(percentProgress: percent, userState: new List<DifferenceResult>(collection: batchResults));
					batchResults.Clear();
					lastReportTicks = currentTicks;
				}
			}
		}
		// After processing file 2, any remaining records in the dictionary from file 1 are considered deleted records; add a DifferenceResult for each deleted record and increment the deleted records counter
		worker.ReportProgress(percentProgress: 100, userState: "Checking for added records...");
		// Iterate through the remaining records in the dictionary from file 1, which represent added/deleted records, and add a DifferenceResult for each one while checking for cancellation
		foreach (KeyValuePair<string, PlanetoidRecord> entry in records1)
		{
			if (worker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}
			// For each remaining record in the dictionary, determine if it is an added or deleted record based on the file date comparison, add a DifferenceResult for it, and increment the respective counter
			string diffText = file1IsNewer ? "Added record" : "Deleted record";
			batchResults.Add(item: new DifferenceResult(Index: entry.Value.Index, Designation: entry.Key, Difference: diffText));
			if (file1IsNewer)
			{
				addedRecords++;
			}
			else
			{
				deletedRecords++;
			}
			// Periodically report progress to the UI with the current batch of differences, using timing to avoid excessive updates while processing large numbers of deleted records
			if (batchResults.Count >= 1000)
			{
				worker.ReportProgress(percentProgress: 100, userState: new List<DifferenceResult>(collection: batchResults));
				batchResults.Clear();
			}
		}
		// Report final progress to the UI with any remaining differences in the batch results after processing all records
		if (batchResults.Count > 0)
		{
			worker.ReportProgress(percentProgress: 100, userState: new List<DifferenceResult>(collection: batchResults));
		}
		else
		{
			worker.ReportProgress(percentProgress: 100);
		}
	}

	/// <summary>Handles the ProgressChanged event of the background worker, updating the progress bar and displaying the current status or batch of differences.</summary>
	/// <param name="sender">The source of the event, typically the background worker.</param>
	/// <param name="e">The event data associated with the ProgressChanged event.</param>
	/// <remarks>This event is triggered when the background worker reports progress, allowing the UI to update the progress bar and display the current status or batch of differences.</remarks>
	private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
	{
		// Update the progress bar value based on the reported progress percentage, ensuring it stays within the valid range of 0 to 100
		kryptonProgressBar.Value = Math.Max(0, Math.Min(val1: 100, val2: e.ProgressPercentage));
		kryptonProgressBar.Text = $"{e.ProgressPercentage}%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)e.ProgressPercentage, progressMax: 100);
		// Check the type of the user state to determine whether to update the status label with a string message or to add a batch of difference results to the list view
		if (e.UserState is string status)
		{
			kryptonProgressBar.Text = status;
		}
		else if (e.UserState is List<DifferenceResult> batch)
		{
			if (batch.Count > 0)
			{
				differenceResults.AddRange(collection: batch);
				listViewResults.VirtualListSize = differenceResults.Count;
			}
		}
	}

	/// <summary>Handles the RunWorkerCompleted event of the background worker, updating the UI based on the completion status of the comparison.</summary>
	/// <param name="sender">The source of the event, typically the background worker.</param>
	/// <param name="e">The event data associated with the RunWorkerCompleted event.</param>
	/// <remarks>This event is triggered when the background worker has completed its task, whether it was cancelled, encountered an error, or finished successfully.</remarks>
	private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
	{
		// Check if the comparison was cancelled by the user, if an error occurred during the comparison, or if it completed successfully, and update the status label and show appropriate messages based on the outcome
		if (e.Cancelled)
		{
			kryptonProgressBar.Text = "Comparison Cancelled";
			_ = KryptonMessageBox.Show(owner: this, text: $"Comparison cancelled by user", caption: "Cancelled", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
		else if (e.Error != null)
		{
			logger.Error(exception: e.Error, message: "Error during comparison");
			kryptonProgressBar.Text = "Error occurred";
			ShowErrorMessage(message: $"An error occurred: {e.Error.Message}");
		}
		else
		{
			kryptonProgressBar.Text = "Comparison Complete";
			_ = KryptonMessageBox.Show(owner: this, text: $"Comparison completed successfully.\n\nAdded records: {addedRecords}\nChanged records: {changedRecords}\nDeleted records: {deletedRecords}", caption: "Summary", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
		}
		// Re-enable the compare and file selection buttons, enable the cancel button, and hide the progress bar now that the comparison is complete or cancelled
		toolStripButtonCompare.Enabled = true;
		kryptonButtonSelectFile1.Enabled = true;
		kryptonButtonSelectFile2.Enabled = true;
		toolStripButtonCancel.Enabled = true;
	}

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the results ListView, allowing the user to jump to the selected record in the main form.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the DoubleClick event.</param>
	/// <remarks>This event is triggered when the user double-clicks on an item in the ListView.</remarks>
	private void ListViewResults_DoubleClick(object? sender, EventArgs e) => GoToObject();

	#endregion

	#region RetrieveVirtualItem event handler

	/// <summary>Handles the RetrieveVirtualItem event for the results ListView. Provides virtual items on demand for display in the list view.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> instance containing the event data.</param>
	/// <remarks>This event is triggered when the ListView requires a virtual item to be displayed.</remarks>
	private void ListViewResults_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		// Check if the requested item index is within the bounds of the difference results list, and if so, create a new ListViewItem based on the corresponding DifferenceResult and assign it to the event's Item property for display in the ListView
		if (e.ItemIndex >= 0 && e.ItemIndex < differenceResults.Count)
		{
			DifferenceResult result = differenceResults[index: e.ItemIndex];
			e.Item = new ListViewItem(items: [result.Index.ToString(), result.Designation, result.Difference]);
		}
	}

	/// <summary>Creates a <see cref="ListViewItem"/> for the specified index from <see cref="differenceResults"/>.</summary>
	/// <param name="index">The zero-based index of the item to retrieve.</param>
	/// <returns>A <see cref="ListViewItem"/> populated with the data from <see cref="differenceResults"/>, or an empty <see cref="ListViewItem"/> when <paramref name="index"/> is out of range.</returns>
	/// <remarks>Used as the <c>virtualRowProvider</c> delegate when exporting via <see cref="Helpers.ListViewExporter"/>.</remarks>
	private ListViewItem GetVirtualListViewItem(int index)
	{
		// Check if the provided index is within the valid range of the difference results list, and if so, create and return a new ListViewItem based on the corresponding DifferenceResult; otherwise, return an empty ListViewItem
		if (index >= 0 && index < differenceResults.Count)
		{
			DifferenceResult result = differenceResults[index: index];
			return new ListViewItem(items: [result.Index.ToString(), result.Designation, result.Difference]);
		}
		return new ListViewItem();
	}

	#endregion
}