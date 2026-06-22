// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Represents a form that displays the theoretical average planetoid calculated from all orbital elements and astrophysical values.</summary>
/// <remarks>This form calculates and displays various types of averages (arithmetic, median, mode, geometric, harmonic, quadratic, cubic, logarithmic, Winsor, quartile, shortest half, Gastwirth-Cohen, range, "a", moving, Hölder, Lehmer) for each orbital element and astrophysical property.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AverageAsteroidForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Average Asteroid";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Average-Asteroid";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column in the ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header to toggle the sort order.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Stores the cancellation token source for the ongoing calculation.</summary>
	/// <remarks>This field is used to signal cancellation of the average calculation when the user clicks the Cancel button.</remarks>
	private CancellationTokenSource? _calculationCts;

	/// <summary>Stores the planetoids database.</summary>
	/// <remarks>This list contains all planetoid database entries from the MPCORB file.</remarks>
	private readonly IReadOnlyList<string> planetoidsDatabase;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="AverageAsteroidForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string. Initializes the form components and calculates the average values.</remarks>
	public AverageAsteroidForm(IReadOnlyList<string> planetoids)
	{
		// Validate input and initialize form components
		InitializeComponent();
		// Ensure the planetoids database is not null
		planetoidsDatabase = planetoids ?? throw new ArgumentNullException(paramName: nameof(planetoids));
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Starts the asynchronous, parallel calculation of all average types for the planetoid database.</summary>
	/// <param name="ct">The cancellation token to observe during the calculation.</param>
	/// <remarks>Parsing runs in parallel on a background thread; progress is reported as a percentage on the ProgressBar.</remarks>
	private async Task CalculateAveragesAsync(CancellationToken ct)
	{
		// Clear any existing items and reset the progress bar before starting the calculation
		try
		{
			// Update the status bar and set the cursor to indicate that a calculation is in progress
			SetStatusBar(label: labelInformation, text: "Calculating averages...");
			Cursor.Current = Cursors.WaitCursor;
			ResetProgress();
			// Use invariant culture for consistent parsing of numeric values from the database entries
			IFormatProvider provider = CultureInfo.InvariantCulture;
			int total = planetoidsDatabase.Count;
			// Initialize lists to hold the values for each orbital element and astrophysical property
			List<double> meanAnomalies = [];
			List<double> argumentsOfPerihelion = [];
			List<double> longitudesOfAscendingNode = [];
			List<double> inclinations = [];
			List<double> eccentricities = [];
			List<double> meanDailyMotions = [];
			List<double> semiMajorAxes = [];
			List<double> absoluteMagnitudes = [];
			List<double> slopeParameters = [];
			List<double> numberOfOppositions = [];
			List<double> numberOfObservations = [];
			List<double> rmsResiduals = [];
			// Create a progress reporter to update the progress bar on the UI thread
			IProgress<int> progress = new Progress<int>(handler: value =>
			{
				kryptonProgressBar.Value = value;
				kryptonProgressBar.Text = $"{value}%";
			});

			// Phase 1 (0–90 %): Parse all entries in parallel on a background thread
			await Task.Run(action: () =>
			{
				// Use pre-allocated arrays indexed by source position to preserve MPCORB record order;
				// each parallel iteration writes only to its own slot, so no locking is needed
				double[] localM = new double[total];
				double[] localOmega = new double[total];
				double[] localOmegaBig = new double[total];
				double[] localI = new double[total];
				double[] localE = new double[total];
				double[] localN = new double[total];
				double[] localA = new double[total];
				double[] localH = new double[total];
				double[] localG = new double[total];
				double[] localNOpp = new double[total];
				double[] localNObs = new double[total];
				double[] localRms = new double[total];
				// Use NaN as a sentinel to distinguish parsed values from unparsed slots
				Array.Fill(array: localM, value: double.NaN);
				Array.Fill(array: localOmega, value: double.NaN);
				Array.Fill(array: localOmegaBig, value: double.NaN);
				Array.Fill(array: localI, value: double.NaN);
				Array.Fill(array: localE, value: double.NaN);
				Array.Fill(array: localN, value: double.NaN);
				Array.Fill(array: localA, value: double.NaN);
				Array.Fill(array: localH, value: double.NaN);
				Array.Fill(array: localG, value: double.NaN);
				Array.Fill(array: localNOpp, value: double.NaN);
				Array.Fill(array: localNObs, value: double.NaN);
				Array.Fill(array: localRms, value: double.NaN);
				// Track the number of processed entries and the last reported progress percentage to update the progress bar efficiently
				int processed = 0;
				int lastReported = -1;
				// Process each planetoid entry in parallel by index, extracting the relevant values and storing them at the corresponding slot
				Parallel.For(
					fromInclusive: 0,
					toExclusive: total,
					parallelOptions: new ParallelOptions { CancellationToken = ct },
					body: i =>
					{
						string entry = planetoidsDatabase[i];
						// Validate the entry format before attempting to parse values; skip entries that are too short or empty
						if (string.IsNullOrWhiteSpace(value: entry) || entry.Length < 160)
						{
							return;
						}
						// Attempt to parse each relevant value from the entry using the specified substring positions and lengths; log any parsing errors without throwing exceptions
						try
						{
							if (double.TryParse(s: entry.Substring(startIndex: 26, length: 9).Trim(), style: NumberStyles.Any, provider: provider, result: out double valM))
							{
								localM[i] = valM; // Mean anomaly at the epoch
							}
							if (double.TryParse(s: entry.Substring(startIndex: 37, length: 9).Trim(), style: NumberStyles.Any, provider: provider, result: out double valOmega))
							{
								localOmega[i] = valOmega; // Argument of perihelion
							}
							if (double.TryParse(s: entry.Substring(startIndex: 48, length: 9).Trim(), style: NumberStyles.Any, provider: provider, result: out double valOmegaBig))
							{
								localOmegaBig[i] = valOmegaBig; // Longitude of ascending node
							}
							if (double.TryParse(s: entry.Substring(startIndex: 59, length: 9).Trim(), style: NumberStyles.Any, provider: provider, result: out double valI))
							{
								localI[i] = valI; // Inclination
							}
							if (double.TryParse(s: entry.Substring(startIndex: 70, length: 9).Trim(), style: NumberStyles.Any, provider: provider, result: out double valE))
							{
								localE[i] = valE; // Eccentricity
							}
							if (double.TryParse(s: entry.Substring(startIndex: 80, length: 11).Trim(), style: NumberStyles.Any, provider: provider, result: out double valN))
							{
								localN[i] = valN; // Mean daily motion
							}
							if (double.TryParse(s: entry.Substring(startIndex: 92, length: 11).Trim(), style: NumberStyles.Any, provider: provider, result: out double valA))
							{
								localA[i] = valA; // Semi-major axis
							}
							if (double.TryParse(s: entry.Substring(startIndex: 8, length: 5).Trim(), style: NumberStyles.Any, provider: provider, result: out double valH))
							{
								localH[i] = valH; // Absolute magnitude
							}
							if (double.TryParse(s: entry.Substring(startIndex: 14, length: 5).Trim(), style: NumberStyles.Any, provider: provider, result: out double valG))
							{
								localG[i] = valG; // Slope parameter
							}
							if (double.TryParse(s: entry.Substring(startIndex: 117, length: 6).Trim(), style: NumberStyles.Any, provider: provider, result: out double valNObs))
							{
								localNObs[i] = valNObs; // Number of observations
							}
							if (double.TryParse(s: entry.Substring(startIndex: 123, length: 4).Trim(), style: NumberStyles.Any, provider: provider, result: out double valNOpp))
							{
								localNOpp[i] = valNOpp; // Number of oppositions
							}
							if (double.TryParse(s: entry.Substring(startIndex: 137, length: 5).Trim(), style: NumberStyles.Any, provider: provider, result: out double valRms))
							{
								localRms[i] = valRms; // Root mean square error
							}
						}
						// Log any exceptions that occur during parsing of the entry, but do not rethrow to allow processing of remaining entries
						catch (Exception ex)
						{
							logger.Warn(message: $"Error parsing planetoid entry: {ex.Message}");
						}
						// Scale parse progress to 0–90 % so phase 2 has visible room in the bar
						int current = Interlocked.Increment(location: ref processed);
						int percent = total > 0 ? (int)((double)current / total * 90) : 90;
						// Only report progress if the percentage has changed to avoid excessive updates to the progress bar
						if (percent != lastReported)
						{
							lastReported = percent;
							progress.Report(value: percent);
						}
					});
				// After processing all entries, collect valid values in source-index order for each property
				meanAnomalies.AddRange(collection: localM.Where(predicate: static v => !double.IsNaN(v)));
				argumentsOfPerihelion.AddRange(collection: localOmega.Where(predicate: static v => !double.IsNaN(v)));
				longitudesOfAscendingNode.AddRange(collection: localOmegaBig.Where(predicate: static v => !double.IsNaN(v)));
				inclinations.AddRange(collection: localI.Where(predicate: static v => !double.IsNaN(v)));
				eccentricities.AddRange(collection: localE.Where(predicate: static v => !double.IsNaN(v)));
				meanDailyMotions.AddRange(collection: localN.Where(predicate: static v => !double.IsNaN(v)));
				semiMajorAxes.AddRange(collection: localA.Where(predicate: static v => !double.IsNaN(v)));
				absoluteMagnitudes.AddRange(collection: localH.Where(predicate: static v => !double.IsNaN(v)));
				slopeParameters.AddRange(collection: localG.Where(predicate: static v => !double.IsNaN(v)));
				numberOfOppositions.AddRange(collection: localNOpp.Where(predicate: static v => !double.IsNaN(v)));
				numberOfObservations.AddRange(collection: localNObs.Where(predicate: static v => !double.IsNaN(v)));
				rmsResiduals.AddRange(collection: localRms.Where(predicate: static v => !double.IsNaN(v)));
			}, cancellationToken: ct);
			// Check for cancellation after the parsing phase before proceeding to the average calculations
			ct.ThrowIfCancellationRequested();
			// Phase 2 (90–100 %): Compute all averages on the thread pool; one step per property row
			(string Name, List<double> Values)[] properties =
			[
				("Mean anomaly at the epoch", meanAnomalies),
				("Argument of the perihelion, J2000.0", argumentsOfPerihelion),
				("Longitude of the ascending node, J2000.0", longitudesOfAscendingNode),
				("Inclination to the ecliptic, J2000.0", inclinations),
				("Orbital eccentricity", eccentricities),
				("Mean daily motion", meanDailyMotions),
				("Semi-major axis", semiMajorAxes),
				("Absolute magnitude, H", absoluteMagnitudes),
				("Slope parameter, G", slopeParameters),
				("Number of oppositions", numberOfOppositions),
				("Number of observations", numberOfObservations),
				("r.m.s. residual", rmsResiduals),
			];
			// Compute the average values for each property and build the corresponding ListViewItem rows in parallel on a background thread; report progress as each row is completed
			ListViewItem[] rows = await Task.Run(function: () =>
			{
				// Create an array to hold the ListViewItem rows for each property; the length of the array is determined by the number of properties being processed
				ListViewItem[] items = new ListViewItem[properties.Length];
				for (int i = 0; i < properties.Length; i++)
				{
					// Check for cancellation before starting the calculation for each property to allow for responsive cancellation of the process
					ct.ThrowIfCancellationRequested();
					items[i] = BuildAverageRow(propertyName: properties[i].Name, values: properties[i].Values);
					// Report 90–100 % as each row is computed
					int percent = 90 + (int)((double)(i + 1) / properties.Length * 10);
					progress.Report(value: percent);
				}
				// After computing all rows, return the array of ListViewItem objects to be added to the ListView on the UI thread
				return items;
			}, cancellationToken: ct);
			// Check for cancellation one final time before updating the UI with the computed average rows
			ct.ThrowIfCancellationRequested();
			// Update the ListView on the UI thread with the pre-computed rows
			listView.BeginUpdate();
			listView.Items.Clear();
			listView.Items.AddRange(items: rows);
			listView.EndUpdate();
			// Ensure the progress bar is set to 100 % after all rows have been added to the ListView
			progress.Report(value: 100);
			// Update the status bar to indicate that the calculation is complete and show the total number of planetoids processed
			SetStatusBar(label: labelInformation, text: $"Calculated averages for {planetoidsDatabase.Count} planetoids");
		}
		// Handle cancellation by clearing the ListView, resetting the progress bar, and updating the status bar to indicate that the calculation was cancelled
		catch (OperationCanceledException)
		{
			listView.Items.Clear();
			ResetProgress();
			SetStatusBar(label: labelInformation, text: "Calculation cancelled");
		}
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred while calculating averages: {ex}");
			KryptonMessageBox.Show(owner: this, text: $"An error has occurred while calculating averages: {ex.Message}", caption: "Calculation Error", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
			SetStatusBar(label: labelInformation, text: "Error calculating averages");
		}
		// Ensure the cursor is reset to default and the UI state is updated to reflect that the calculation is no longer running, regardless of success, cancellation, or error
		finally
		{
			Cursor.Current = Cursors.Default;
			SetCalculationRunning(running: false);
		}
	}

	/// <summary>Resets the progress bar to its initial state.</summary>
	/// <remarks>This method sets the progress bar value to 0 and updates the text to "0%".</remarks>
	private void ResetProgress()
	{
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0%";
	}

	/// <summary>Updates the UI state depending on whether a calculation is running.</summary>
	/// <param name="running">True if a calculation is in progress; otherwise false.</param>
	/// <remarks>This method enables or disables the Start and Cancel buttons based on the calculation state.</remarks>
	private void SetCalculationRunning(bool running)
	{
		toolStripButtonStart.Enabled = !running;
		toolStripButtonCancel.Enabled = running;
	}

	/// <summary>Builds a <see cref="ListViewItem"/> with all calculated average types for a given property without adding it to the ListView.</summary>
	/// <param name="propertyName">The name of the orbital element or astrophysical property.</param>
	/// <param name="values">The collection of values to calculate averages from.</param>
	/// <returns>A fully populated <see cref="ListViewItem"/> ready to be inserted into the ListView.</returns>
	/// <remarks>This method calculates all 18 types of averages. It is safe to call from a background thread.</remarks>
	private static ListViewItem BuildAverageRow(string propertyName, List<double> values)
	{
		// Create a new ListViewItem with the property name as the first column
		ListViewItem item = new(text: propertyName);
		// Calculate and add all average types
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.ArithmeticMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.Median(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.Mode(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.GeometricMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.HarmonicMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.QuadraticMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.CubicMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.LogarithmicMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.WinsorMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.QuartileMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.ShortestHalfMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.GastwirthCohenMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.RangeMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.AMean(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.MovingAverage(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.HolderMeanShortestHalf(values: values)));
		item.SubItems.Add(text: FormatValue(value: AverageCalculator.LehmerMean(values: values)));
		return item;
	}

	/// <summary>Formats a numeric value for display in the ListView.</summary>
	/// <param name="value">The numeric value to format.</param>
	/// <returns>A formatted string representation of the value, or "N/A" if the value is NaN or Infinity.</returns>
	/// <remarks>This method formats values with appropriate precision for display.</remarks>
	private static string FormatValue(double value) =>
		// Check for NaN or Infinity and return "N/A" if so; otherwise, format the value with 6 decimal places using invariant culture
		double.IsNaN(d: value) || double.IsInfinity(d: value) ? "N/A" : value.ToString(format: "F6", provider: CultureInfo.InvariantCulture);

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Initialises the UI state on load without starting the calculation.</remarks>
	private void AverageAsteroidForm_Load(object? sender, EventArgs e) => SetCalculationRunning(running: false);

	/// <summary>Handles the FormClosing event to cancel any running calculation and release resources.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Signals cancellation and disposes the token source so that background work does not continue against a disposed form.</remarks>
	private void AverageAsteroidForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		_calculationCts?.Cancel();
		_calculationCts?.Dispose();
		_calculationCts = null;
	}

	/// <summary>Handles the Click event of the Start button to begin the asynchronous average calculation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method initializes the cancellation token source, updates the UI state to indicate that a calculation is running, and starts the asynchronous calculation of averages. It also includes error handling to log any unexpected exceptions that occur during the calculation.</remarks>
	private async void ToolStripButtonStart_Click(object? sender, EventArgs e)
	{
		// Dispose of any existing cancellation token source to ensure that previous calculations are properly cancelled before starting a new one
		_calculationCts?.Dispose();
		// Create a new cancellation token source for the upcoming calculation
		_calculationCts = new CancellationTokenSource();
		// Update the UI to indicate that a calculation is running, which will disable the Start button and enable the Cancel button
		SetCalculationRunning(running: true);
		// Start the asynchronous calculation of averages and handle any unexpected exceptions that may occur during the process
		try
		{
			await CalculateAveragesAsync(ct: _calculationCts.Token);
		}
		catch (Exception ex)
		{
			logger.Error(message: $"Unexpected error in Start handler: {ex}");
		}
		finally
		{
			// Ensure the UI state is updated to reflect that the calculation is no longer running
			SetCalculationRunning(running: false);
		}
	}

	/// <summary>Handles the Click event of the Cancel button to abort a running calculation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method signals cancellation of the ongoing average calculation by calling the Cancel method on the cancellation token source. If no calculation is running, this method has no effect.</remarks>
	private void ToolStripButtonCancel_Click(object? sender, EventArgs e) => _calculationCts?.Cancel();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// Ensure the sender is a ListView and that it contains items before attempting to sort
		if (listView.Items.Count == 0)
		{
			return;
		}
		// Determine the new sort order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to indicate the current sort column and order
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Remove any existing sort indicators from the column header text
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the appropriate sort indicator to the currently sorted column
			if (i == sortColumn)
			{
				// Use an upward arrow for ascending sort and a downward arrow for descending sort
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// For unsorted columns, just set the header text without any indicator
			else
			{
				// Ensure that non-sorted columns do not have any sort indicators
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the copy-to-clipboard menu item. Copies the text of the currently selected list view row to the clipboard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>All sub-items of the selected row are joined with a tab character before being placed on the clipboard. If no row is selected the method returns without action.</remarks>
	private void ToolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
	{
		if (listView.SelectedItems.Count == 0)
		{
			return;
		}
		ListViewItem selectedItem = listView.SelectedItems[index: 0];
		IEnumerable<string> subItemTexts = selectedItem.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(selector: static s => s.Text);
		string text = string.Join(separator: "\t", values: subItemTexts);
		CopyToClipboard(text: text);
	}

	#endregion
}