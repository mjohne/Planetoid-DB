using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for finding orbital resonances of all minor planets relative to the 8 known solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their orbital resonances with each selected planet. Results are displayed in a VirtualMode ListView. The user can select which planets to include, start and cancel the search at any time.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitalResonancesOfAllMinorPlanetsForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Orbital resonances";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "OrbitalResonancesOfAllMinorPlanets";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Cache for displaying planetoid records</summary>
	/// <remarks>This field is used to cache planetoid records for display purposes.</remarks>
	private readonly List<PlanetoidRecord> displayCache = [];

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>This field stores the index of the currently sorted column.</remarks>
	private int sortColumn = -1;

	/// <summary>The value indicates how items in the currently sorted column are ordered:
	/// <list type="bullet">
	/// <item><description><see cref="SortOrder.None"/>: No sorting is applied.</description></item>
	/// <item><description><see cref="SortOrder.Ascending"/>: Items are sorted in ascending order.</description></item>
	/// <item><description><see cref="SortOrder.Descending"/>: Items are sorted in descending order.</description></item>
	/// </list>
	/// This field is typically updated when the user clicks a column header in the list view to toggle the sort order.</summary>
	/// <remarks>This field stores the current sort order of the list view.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>The deviation threshold in percent below which an orbital ratio is considered a near-resonance.</summary>
	/// <remarks>This value is used to determine if a computed orbital resonance is significant.</remarks>
	private const double ResonanceThresholdPercent = 1.0;

	/// <summary>Length of the sort-direction prefix (e.g. "▲ " or "▼ ") prepended to a column header text.</summary>
	/// <remarks>This constant is used to calculate the original column header text by removing the prefix when sorting columns.</remarks>
	private const int SortIndicatorPrefixLength = 2;

	/// <summary>Zero-based index of the Planetoid column in the results ListView.</summary>
	/// <remarks>This column displays the name or designation of the planetoid for which the resonance is calculated. It is used as the primary identifier for each row in the results and is displayed in the first column of the ListView.</remarks>
	private const int ColumnIndexPlanetoid = 0;

	/// <summary>Zero-based index of the Planet column in the results ListView.</summary>
	/// <remarks>This column displays the name of the planet with which the resonance is calculated. It is used for filtering results based on user selection of planets.</remarks>
	private const int ColumnIndexPlanet = 1;

	/// <summary>Zero-based index of the Planet Period column in the results ListView.</summary>
	/// <remarks>This column displays the orbital period of the planet in Earth years, which is a fixed value for each planet and is used in the resonance calculations. It is calculated from the planet's semi-major axis using Kepler's third law and is stored in the <see cref="DerivedElements.OrbitalResonance"/> data structure for each resonance result.</remarks>
	private const int ColumnIndexPlanetPeriod = 2;

	/// <summary>Zero-based index of the Planetoid Period column in the results ListView.</summary>
	/// <remarks>This column displays the orbital period of the planetoid in Earth years, calculated from its semi-major axis using Kepler's third law. It is used in conjunction with the Planet Period column to compute the Ratio and Resonance columns.</remarks>
	private const int ColumnIndexPlanetoidPeriod = 3;

	/// <summary>Zero-based index of the Ratio column in the results ListView.</summary>
	/// <remarks>This column displays the ratio of the planetoid's orbital period to the planet's orbital period, calculated as Planetoid Period / Planet Period. Values close to a simple fraction (e.g. 0.5 for 1:2, 1.5 for 3:2) indicate potential resonances.</remarks>
	private const int ColumnIndexRatio = 4;

	/// <summary>Zero-based index of the Resonance (P:Q) column in the results ListView.</summary>
	/// <remarks>This column displays the resonance ratio in the form of "P:Q", where P and Q are integers representing the resonance relationship between the planetoid's orbital period and the planet's orbital period. For example, a value of "3:2" indicates that the planetoid completes 3 orbits for every 2 orbits of the planet.</remarks>
	private const int ColumnIndexResonance = 5;

	/// <summary>Zero-based index of the Deviation column in the results ListView.</summary>
	/// <remarks>This column displays the percentage deviation of the actual orbital ratio from the exact resonance ratio. Values below the defined threshold indicate a near-resonance and are highlighted in green, while values above the threshold are highlighted in red.</remarks>
	private const int ColumnIndexDeviation = 6;

	/// <summary>Zero-based index of the Is Resonance column in the results ListView.</summary>
	/// <remarks>This column displays "Yes" if the deviation percent is below the defined threshold, indicating a near-resonance, and "No" otherwise.</remarks>
	private const int ColumnIndexIsResonance = 7;

	/// <summary>Holds the list of all planetoid database strings passed to this form.</summary>
	/// <remarks>Each string is one raw MPCORB record from the database.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the computed resonance results to back the VirtualMode ListView.</summary>
	/// <remarks>Replaced atomically on the UI thread after each search completes.</remarks>
	private List<ResonanceResult> _results = [];

	/// <summary>Holds the full unfiltered set of resonance results computed by the most recent search.</summary>
	/// <remarks>Filtering and sorting derive <see cref="_results"/> from this collection without modifying it, so re-filtering (e.g. when the user changes the planet selection or the filter toggle) does not require re-running the search.</remarks>
	private List<ResonanceResult> _allResults = [];

	/// <summary>Cancellation token source for the currently running search task.</summary>
	/// <remarks>This field is used to signal cancellation to the background search task when the user clicks the Cancel button or when the form is closing. It is created when a search starts and disposed when the search completes or is cancelled.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents a single resonance result combining a planetoid designation with its resonance data.</summary>
	/// <param name="PlanetoidName">The readable designation or packed index of the planetoid.</param>
	/// <param name="Resonance">The computed orbital resonance data.</param>
	/// <remarks>This record is used to store the results of the resonance calculations for each planetoid.</remarks>
	private record ResonanceResult(string PlanetoidName, DerivedElements.OrbitalResonance Resonance);

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="OrbitalResonancesOfAllMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public OrbitalResonancesOfAllMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Cache the planetoid records for use during the search; this allows the search method to access the raw data without needing to pass it around or access it from a shared resource, which can help improve performance and reduce complexity during the resonance calculations
		_planetoids = planetoids;
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is primarily intended for debugging purposes.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Filters the result set list accordingly.</summary>
	/// <remarks>This method applies the active filter settings to the results and refreshes the list view to reflect the filtered data. The filter includes only results for selected planets and, if enabled, further restricts by resonance deviation. The method should be called whenever filter settings or selection change to ensure the displayed data remains accurate.</remarks>
	private void FilterResults()
	{
		// Get the list of selected planets from the UI; this list is used to filter the resonance results to include only those that match the user's selection of planets
		List<string> selectedPlanets = GetSelectedPlanets();
		// Check if the filter for resonances is enabled by checking the state of the corresponding tool strip button; if the filter is enabled, we will further restrict the results to include only those that have a deviation percent below the defined threshold, which indicates that they are near-resonances
		bool filterResonances = toolStripButtonFilterResonances.Checked;
		// Filter the complete results list to include only those results that match the selected planets and, if the filter is enabled, have a deviation percent below the defined threshold; this creates a new list of results that will be displayed in the ListView, while the original _allResults list remains unchanged to allow for re-filtering when the user changes their selection
		_results = [.. _allResults.Where(predicate: r =>
			selectedPlanets.Contains(item: r.Resonance.PlanetName) &&
			(!filterResonances || r.Resonance.DeviationPercent < ResonanceThresholdPercent))];
		// Sort the filtered results only when an actual sort column and direction have been selected
		if (sortColumn >= 0 && sortOrder != SortOrder.None)
		{
			SortResults();
		}
		// Update the VirtualListSize of the ListView to match the count of the filtered results; this tells the ListView how many items it should expect to retrieve in virtual mode, and it will call the RetrieveVirtualItem event handler for each visible item index up to this count; after updating the list size, we call Refresh to force the ListView to redraw itself with the new data
		listView.VirtualListSize = _results.Count;
		listView.Refresh();
	}

	/// <summary>Returns the list of planet names currently selected via the planet checkbuttons.</summary>
	/// <returns>A list of planet name strings that are checked.</returns>
	/// <remarks>Planet names match those used in <see cref="DerivedElements.CalculateOrbitalResonances"/>.</remarks>
	private List<string> GetSelectedPlanets()
	{
		// Check the state of each planet's checkbutton and build a list of selected planet names; this list is used to filter the resonance results based on the user's selection
		List<string> selected = [];
		if (toolStripButtonMercury.Checked)
		{
			selected.Add(item: "Mercury");
		}
		if (toolStripButtonVenus.Checked)
		{
			selected.Add(item: "Venus");
		}
		if (toolStripButtonEarth.Checked)
		{
			selected.Add(item: "Earth");
		}
		if (toolStripButtonMars.Checked)
		{
			selected.Add(item: "Mars");
		}
		if (toolStripButtonJupiter.Checked)
		{
			selected.Add(item: "Jupiter");
		}
		if (toolStripButtonSaturn.Checked)
		{
			selected.Add(item: "Saturn");
		}
		if (toolStripButtonUranus.Checked)
		{
			selected.Add(item: "Uranus");
		}
		if (toolStripButtonNeptune.Checked)
		{
			selected.Add(item: "Neptune");
		}
		return selected;
	}

	/// <summary>Updates the progress bar value and text label.</summary>
	/// <param name="percent">Progress value from 0 to 100.</param>
	/// <remarks>The percentage is displayed both in the progress bar's <c>Text</c> property and in the adjacent label.</remarks>
	private void UpdateProgress(int percent)
	{
		// Clamp the percentage value to ensure it stays within the valid range of 0 to 100; this prevents invalid values from being set on the progress bar and ensures that the displayed percentage is always meaningful
		int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
		kryptonProgressBar.Value = clampedPercent;
		kryptonProgressBar.Text = $"{clampedPercent}%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
	}

	/// <summary>Processes one raw MPCORB database record and appends matching resonance results to <paramref name="results"/>.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="results">The list to which matching <see cref="ResonanceResult"/> items are appended.</param>
	/// <remarks>Lines that are too short or have an invalid semi-major axis are silently skipped. All resonances are added to the results list; filtering by selected planets is handled at a higher level.</remarks>
	private static void ProcessPlanetoidLine(string line, List<ResonanceResult> results)
	{
		// Validate the line length to ensure it contains the expected fields; if the line is too short, it cannot be processed and is skipped without logging an error, as this may be common for malformed records
		if (line.Length < 103)
		{
			return;
		}
		// Extract the semi-major axis from the fixed-width field in the MPCORB record; the semi-major axis is located at characters 92-102 (11 characters total) and is parsed as a double; if parsing fails or if the value is non-positive, the line is skipped without logging an error, as this may be common for records with missing or invalid data
		string semiMajorAxisText = line.Substring(startIndex: 92, length: 11).Trim();
		if (!double.TryParse(s: semiMajorAxisText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis) || semiMajorAxis <= 0)
		{
			return;
		}
		string designation = line.Length >= 194
			? line.Substring(startIndex: 166, length: 28).Trim()
			: line[..7].Trim();
		if (string.IsNullOrEmpty(value: designation))
		{
			designation = line[..7].Trim();
		}
		// Collect all resonances. Any near-resonance classification (e.g. Yes/No indicators) is handled at a higher level.
		List<DerivedElements.OrbitalResonance> resonances = DerivedElements.CalculateOrbitalResonances(semiMajorAxis: semiMajorAxis);
		foreach (DerivedElements.OrbitalResonance resonance in resonances)
		{
			results.Add(item: new ResonanceResult(PlanetoidName: designation, Resonance: resonance));
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event. Clears the status bar on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>Clears the status bar when the form is loaded.</remarks>
	private void OrbitalResonancesOfAllMinorPlanetsForm_Load(object sender, EventArgs e) =>
		ClearStatusBar(label: labelInformation);

	/// <summary>Handles the FormClosing event. Cancels any running search and disposes the cancellation token source.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>Cancels any running search and disposes the cancellation token source when the form is closing.</remarks>
	private void OrbitalResonancesOfAllMinorPlanetsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// If a search is currently running (indicated by a non-null cancellation token source), signal cancellation and dispose of the token source to free resources; this ensures that any background tasks are properly cancelled when the form is closed, preventing potential issues with lingering tasks or resource leaks
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}
	}

	#endregion

	#region RetrieveVirtualItem event handler

	/// <summary>Handles the RetrieveVirtualItem event for the VirtualMode ListView. Provides the <see cref="ListViewItem"/> for the requested index from <see cref="_results"/>.</summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="RetrieveVirtualItemEventArgs"/> containing the requested item index.</param>
	/// <remarks>Called by the ListView for each visible row. Must be fast and must not modify <see cref="_results"/>.</remarks>
	private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		if (e.ItemIndex < 0 || e.ItemIndex >= _results.Count)
		{
			e.Item = new ListViewItem();
			return;
		}
		ResonanceResult result = _results[index: e.ItemIndex];
		bool isResonanceValue = result.Resonance.DeviationPercent < ResonanceThresholdPercent;
		string isResonance = isResonanceValue ? "Yes" : "No";
		ListViewItem item = new(text: result.PlanetoidName);
		item.SubItems.AddRange(items:
		[
			result.Resonance.PlanetName,
			result.Resonance.PlanetPeriod.ToString(format: "F6"),
			result.Resonance.PlanetoidPeriod.ToString(format: "F6"),
			result.Resonance.Ratio.ToString(format: "F6"),
			$"{result.Resonance.ResonanceP}:{result.Resonance.ResonanceQ}",
			result.Resonance.DeviationPercent.ToString(format: "F2"),
			isResonance
		]);

		item.ForeColor = isResonanceValue ? Color.Green : Color.Red;
		e.Item = item;
	}

	#endregion

	#region CheckedChanged event handlers

	/// <summary>Handles the CheckedChanged event for all planet selection buttons and the filter resonances button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called whenever a planet selection button or the filter resonances button is checked or unchecked.</remarks>
	private void PlanetButton_CheckedChanged(object? sender, EventArgs e)
	{
		// If there are no results yet, there is nothing to filter, so we can skip the filtering step; this check prevents unnecessary processing when the user changes the planet selection before running the search for the first time
		if (_allResults.Count > 0)
		{
			FilterResults();
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start Search button. Validates the selection, then starts the orbital resonance search asynchronously.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The search runs on a background thread. Progress is reported via the progress bar. The user can cancel at any time using the Cancel button.</remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// Validate that at least one planet is selected before starting the search
		List<string> selectedPlanets = GetSelectedPlanets();
		if (selectedPlanets.Count == 0)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "Please select at least one planet.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		if (_planetoids.Count == 0)
		{
			_ = KryptonMessageBox.Show(owner: this, text: "No planetoid data available.", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Disable the Start button and planet selection buttons to prevent changes during the search; also disable the save menu since there are no results yet; these controls will be re-enabled when the search completes or is cancelled
		toolStripDropDownButtonSaveToFile.Enabled = false;
		toolStripLabelPlanets.Enabled = false;
		toolStripButtonMercury.Enabled = false;
		toolStripButtonVenus.Enabled = false;
		toolStripButtonEarth.Enabled = false;
		toolStripButtonMars.Enabled = false;
		toolStripButtonJupiter.Enabled = false;
		toolStripButtonSaturn.Enabled = false;
		toolStripButtonUranus.Enabled = false;
		toolStripButtonNeptune.Enabled = false;
		toolStripButtonFilterResonances.Enabled = false;
		// Disable the Start button and enable the Cancel button to reflect the search state; clear previous results and reset the progress bar and status label before starting the new search
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		_allResults.Clear();
		_results = [];
		listView.VirtualListSize = 0;
		UpdateProgress(percent: 0);
		ClearStatusBar(label: labelInformation);
		// Create a new cancellation token source for the search task; the token is passed to the background task to allow cooperative cancellation; if the user clicks the Cancel button, the token will be signaled, and the background task can respond by throwing an OperationCanceledException
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		// Create a local list to store results from the background search; this avoids cross-thread access issues with the form's _results field, which is only updated once when the search completes on the UI thread
		List<ResonanceResult> localResults = [];
		// Create a progress reporter that updates the progress bar and status label on the UI thread; the UpdateProgress method is called with the current percentage of completion, which is calculated based on the number of planetoids processed relative to the total count
		IProgress<int> progress = new Progress<int>(handler: UpdateProgress);
		// Run the search on a background thread to keep the UI responsive; the search iterates over all planetoids, processes each one, and reports progress periodically; if the search is cancelled, an OperationCanceledException is caught and logged; any other exceptions are also caught and logged, and an error message is shown to the user; finally, when the search completes or is cancelled, the results are assigned to the form's field and the ListView is refreshed on the UI thread
		try
		{
			// Calculate the total number of planetoids and determine the interval at which to report progress; the report interval is set to either 1 or 1% of the total count, whichever is greater, to ensure that progress updates are not too frequent for small datasets but still provide regular feedback for larger datasets
			int total = _planetoids.Count;
			int reportInterval = Math.Max(val1: 1, val2: total / 100);
			// The action passed to Task.Run iterates over all planetoids, processes each one, and reports progress at regular intervals; the token is checked for cancellation on each iteration, allowing the search to be cancelled cooperatively; if cancellation is requested, a OperationCanceledException is thrown, which is caught in the outer try-catch block
			await Task.Run(action: () =>
			{
				// Iterate over all planetoids and process each one; the ProcessPlanetoidLine method is called for each line, which computes the resonances and appends matching results to the localResults list; progress is reported every reportInterval iterations or on the last iteration to update the progress bar and status label
				for (int i = 0; i < total; i++)
				{
					token.ThrowIfCancellationRequested();
					ProcessPlanetoidLine(line: _planetoids[index: i], results: localResults);
					if (i % reportInterval == 0 || i == total - 1)
					{
						progress.Report(value: (i + 1) * 100 / total);
					}
				}
				// Log the completion of the search with the total number of resonances found; this provides feedback in the logs about the outcome of the search
				logger.Info(message: $"Orbital resonance search completed. Total resonances found: {localResults.Count}");
			}, cancellationToken: token);
		}
		// Catch the OperationCanceledException to handle user cancellation gracefully; log the cancellation event and update the status label to inform the user that the search was cancelled
		catch (OperationCanceledException)
		{
			logger.Info(message: "Orbital resonance search cancelled by user.");
		}
		// Catch any other exceptions that may occur during the search; log the error and show an error message to the user with details about the exception
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error during search: {ex.Message}");
		}
		// Finally block to clean up resources and reset UI elements after the search completes or is cancelled
		finally
		{
			try
			{
				// Update the form's _results field with the local results from the search and refresh the ListView to display the new results; this is done on the UI thread, and we check if the form is still valid (not disposed) before updating the UI; if the form is closing while the search is still running, it may have been disposed, in which case we skip updating the UI to avoid exceptions
				if (IsHandleCreated && !IsDisposed && !Disposing)
				{
					_allResults = localResults;
					FilterResults();
					toolStripButtonStart.Enabled = true;
					toolStripButtonCancel.Enabled = false;
					// Re-enable the planet selection buttons, filter toggle, and save menu on the UI thread after the background work has fully completed
					toolStripDropDownButtonSaveToFile.Enabled = true;
					toolStripLabelPlanets.Enabled = true;
					toolStripButtonMercury.Enabled = true;
					toolStripButtonVenus.Enabled = true;
					toolStripButtonEarth.Enabled = true;
					toolStripButtonMars.Enabled = true;
					toolStripButtonJupiter.Enabled = true;
					toolStripButtonSaturn.Enabled = true;
					toolStripButtonUranus.Enabled = true;
					toolStripButtonNeptune.Enabled = true;
					toolStripButtonFilterResonances.Enabled = true;
				}
			}
			// Catch ObjectDisposedException and InvalidOperationException that may occur if the form is closing while the search is still running; these exceptions can be safely ignored as they indicate that the form is being disposed and the search is being cancelled
			catch (ObjectDisposedException)
			{
				// Ignore exceptions caused by controls being disposed during form shutdown.
			}
			catch (InvalidOperationException)
			{
				// Ignore exceptions related to invalid control state during form shutdown.
			}
			// Dispose of the cancellation token source to free resources; set it to null to indicate that there is no active search
			finally
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
			}
		}
	}

	/// <summary>Handles the Click event of the Cancel button. Cancels the currently running search.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The search can be cancelled by the user at any time using the Cancel button.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// If a search is currently running, request cancellation and prevent repeated cancel clicks.
		// Keep the save, planet-selection, and filter controls disabled here so they are only
		// re-enabled after the background search has fully completed and final results have been
		// applied on the UI thread by the normal completion logic.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Handles the Click event of the copy-to-clipboard menu item. Copies the text of the currently selected list view row to the clipboard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>All sub-items of the selected row are joined with a tab character before being placed on the clipboard. If no row is selected the method returns without action.</remarks>
	private void ToolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
	{
		// Check if any item is selected in the list view; if not, return without doing anything; if an item is selected, get the index of the first selected item and validate it against the _results list; if the index is valid, retrieve the corresponding ResonanceResult and format its data into a tab-separated string; finally, copy the formatted string to the clipboard
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
		int index = listView.SelectedIndices[index: 0];
		if (index < 0 || index >= _results.Count)
		{
			return;
		}
		ResonanceResult result = _results[index];
		string isResonance = result.Resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No";
		string text = string.Join(separator: "\t", values: new[]
		{
			result.PlanetoidName,
			result.Resonance.PlanetName,
			result.Resonance.PlanetPeriod.ToString(format: "F6"),
			result.Resonance.PlanetoidPeriod.ToString(format: "F6"),
			result.Resonance.Ratio.ToString(format: "F6"),
			$"{result.Resonance.ResonanceP}:{result.Resonance.ResonanceQ}",
			result.Resonance.DeviationPercent.ToString(format: "F2"),
			isResonance
		});
		CopyToClipboard(text: text);
	}

	#endregion

	#region ColumnClick event handler

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles the sort order for the clicked column (ascending/descending) and re-sorts the results list. Column headers are updated with a ▲ or ▼ indicator to show the current sort direction.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no results to sort, exit the method early to avoid unnecessary processing and potential errors when trying to access column headers or sort the empty list
		if (_results.Count == 0)
		{
			return;
		}
		// Check if the clicked column is the same as the current sort column; if so, toggle the sort order between ascending and descending; if it's a different column, set it as the new sort column and default to ascending order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to reflect the current sort column and direction by adding a ▲ or ▼ prefix to the header text of the sorted column, while ensuring that any existing sort indicators are removed from all headers before applying the new indicator
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Get the current header text for the column, removing any existing sort indicators (▲ or ▼) to ensure that only the currently sorted column displays the appropriate indicator; then update the header text for each column, adding the appropriate sort indicator (▲ for ascending, ▼ for descending) to the currently sorted column while leaving other columns without indicators
			string headerText = listView.Columns[index: i].Text;
			// Remove any existing sort indicators (▲ or ▼) from the header text to ensure that only the currently sorted column displays the appropriate indicator
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.Ordinal) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.Ordinal))
			{
				headerText = headerText[SortIndicatorPrefixLength..];
			}
			// Update the header text for each column, adding the appropriate sort indicator (▲ for ascending, ▼ for descending) to the currently sorted column while leaving other columns without indicators
			listView.Columns[index: i].Text = i == sortColumn
				? $"{(sortOrder == SortOrder.Ascending ? "▲" : "▼")} {headerText}"
				: headerText;
		}
		// Re-sort the results based on the newly selected sort column and order, then refresh the ListView to display the sorted results
		SortResults();
		listView.Refresh();
	}

	/// <summary>Sorts the <see cref="_results"/> list by the currently selected column and sort order.</summary>
	/// <remarks>Numeric columns (Planet Period, Planetoid Period, Ratio, Deviation) are sorted numerically; all other columns are sorted as strings (case-insensitive, ordinal).</remarks>
	private void SortResults()
	{
		// Determine the column to sort by and the sort direction, then apply the appropriate sorting logic based on the column type (numeric or string) and update the _results list accordingly
		int col = sortColumn;
		bool ascending = sortOrder == SortOrder.Ascending;
		_results = col switch
		{
			// For the Planet Period column, sort the results by the PlanetPeriod property of the Resonance object in ascending or descending order based on the current sort order
			ColumnIndexPlanetPeriod when ascending => [.. _results.OrderBy(keySelector: static r => r.Resonance.PlanetPeriod)],
			ColumnIndexPlanetPeriod => [.. _results.OrderByDescending(keySelector: static r => r.Resonance.PlanetPeriod)],
			ColumnIndexPlanetoidPeriod when ascending => [.. _results.OrderBy(keySelector: static r => r.Resonance.PlanetoidPeriod)],
			ColumnIndexPlanetoidPeriod => [.. _results.OrderByDescending(keySelector: static r => r.Resonance.PlanetoidPeriod)],
			ColumnIndexRatio when ascending => [.. _results.OrderBy(keySelector: static r => r.Resonance.Ratio)],
			ColumnIndexRatio => [.. _results.OrderByDescending(keySelector: static r => r.Resonance.Ratio)],
			ColumnIndexDeviation when ascending => [.. _results.OrderBy(keySelector: static r => r.Resonance.DeviationPercent)],
			ColumnIndexDeviation => [.. _results.OrderByDescending(keySelector: static r => r.Resonance.DeviationPercent)],
			_ when ascending => [.. _results.OrderBy(keySelector: r => GetColumnText(result: r, column: col), comparer: StringComparer.OrdinalIgnoreCase)],
			_ => [.. _results.OrderByDescending(keySelector: r => GetColumnText(result: r, column: col), comparer: StringComparer.OrdinalIgnoreCase)]
		};
	}

	/// <summary>Returns the display text for a given column of a <see cref="ResonanceResult"/>.</summary>
	/// <param name="result">The resonance result.</param>
	/// <param name="column">The zero-based column index.</param>
	/// <returns>The text value used for sorting and display of the specified column.</returns>
	private static string GetColumnText(ResonanceResult result, int column) => column switch
	{
		// Returns the display text for each column of a ResonanceResult
		ColumnIndexPlanetoid => result.PlanetoidName,
		// For the Planet column, return the name of the planet involved in the resonance
		ColumnIndexPlanet => result.Resonance.PlanetName,
		// For the Resonance column, return a string in the format "P:Q" representing the resonance ratio
		ColumnIndexResonance => $"{result.Resonance.ResonanceP}:{result.Resonance.ResonanceQ}",
		// For the IsResonance column, return "Yes" if the deviation is below the threshold, otherwise "No"
		ColumnIndexIsResonance => result.Resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No",
		_ => string.Empty
	};

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item is double-clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListView_DoubleClick(object sender, EventArgs e)
	{
		// Ensure that an item is selected and that the index is within the bounds of the results list; if so, retrieve the corresponding ResonanceResult and use its PlanetoidName to jump to the record in the PlanetoidDbForm
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
		// Get the index of the first selected item in the ListView; since multi-select is disabled, there will only be one selected item, so we can safely use index 0 to retrieve it
		int index = listView.SelectedIndices[index: 0];
		// Check if the index is valid (non-negative and within the bounds of the _results list); if not, return without taking any action
		if (index < 0 || index >= _results.Count)
		{
			return;
		}
		// Retrieve the ResonanceResult corresponding to the selected item in the ListView
		ResonanceResult result = _results[index];
		// If the Owner of this form is a PlanetoidDbForm, call its JumpToRecord method with the PlanetoidName from the selected ResonanceResult to display the corresponding record
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			planetoidDbForm.JumpToRecord(index: result.PlanetoidName, designation: result.PlanetoidName);
		}
	}

	#endregion
}