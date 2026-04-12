using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for finding orbital resonances of all minor planets relative to the 8 known solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their orbital resonances
/// with each selected planet. Results are displayed in a VirtualMode ListView.
/// The user can select which planets to include, start and cancel the search at any time.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitalResonancesOfAllMinorPlanetsForm : BaseKryptonForm
{
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
	private const int SortIndicatorPrefixLength = 2;

	/// <summary>Zero-based index of the Planetoid column in the results ListView.</summary>
	private const int ColumnIndexPlanetoid = 0;

	/// <summary>Zero-based index of the Planet column in the results ListView.</summary>
	private const int ColumnIndexPlanet = 1;

	/// <summary>Zero-based index of the Planet Period column in the results ListView.</summary>
	private const int ColumnIndexPlanetPeriod = 2;

	/// <summary>Zero-based index of the Planetoid Period column in the results ListView.</summary>
	private const int ColumnIndexPlanetoidPeriod = 3;

	/// <summary>Zero-based index of the Ratio column in the results ListView.</summary>
	private const int ColumnIndexRatio = 4;

	/// <summary>Zero-based index of the Resonance (P:Q) column in the results ListView.</summary>
	private const int ColumnIndexResonance = 5;

	/// <summary>Zero-based index of the Deviation column in the results ListView.</summary>
	private const int ColumnIndexDeviation = 6;

	/// <summary>Zero-based index of the Is Resonance column in the results ListView.</summary>
	private const int ColumnIndexIsResonance = 7;

	/// <summary>Holds the list of all planetoid database strings passed to this form.</summary>
	/// <remarks>Each string is one raw MPCORB record from the database.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the computed resonance results to back the VirtualMode ListView.</summary>
	/// <remarks>Replaced atomically on the UI thread after each search completes.</remarks>
	private List<ResonanceResult> _results = [];

	/// <summary>Holds the full unfiltered set of resonance results computed by the most recent search.</summary>
	/// <remarks>Filtering and sorting derive <see cref="_results"/> from this collection without modifying it,
	/// so re-filtering (e.g. when the user changes the planet selection or the filter toggle) does not
	/// require re-running the search.</remarks>
	private List<ResonanceResult> _allResults = [];

	/// <summary>Cancellation token source for the currently running search task.</summary>
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

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		// Set up the save dialog properties
		dialog.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set default file name
		dialog.FileName = $"OrbitalResonancesOfAllMinorPlanets_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the list view, title, file name, and an optional virtual row provider.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
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
			exportAction(listView, "Orbital resonances", saveFileDialog.FileName, null);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails. This ensures that the user interface remains responsive and provides appropriate feedback to the user.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Filters the result set
	/// list accordingly.</summary>
	/// <remarks>This method applies the active filter settings to the results and refreshes the list view to
	/// reflect the filtered data. The filter includes only results for selected planets and, if enabled, further restricts
	/// by resonance deviation. The method should be called whenever filter settings or selection change to ensure the
	/// displayed data remains accurate.</remarks>
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
	/// <remarks>Lines that are too short or have an invalid semi-major axis are silently skipped.
	/// All resonances are added to the results list; filtering by selected planets is handled at a higher level.</remarks>
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

	/// <summary>Handles the form Load event.
	/// Clears the status bar on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>Clears the status bar when the form is loaded.</remarks>
	private void OrbitalResonancesOfAllMinorPlanetsForm_Load(object sender, EventArgs e) =>
		ClearStatusBar(label: labelInformation);

	/// <summary>Handles the FormClosing event.
	/// Cancels any running search and disposes the cancellation token source.</summary>
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

	/// <summary>Handles the RetrieveVirtualItem event for the VirtualMode ListView.
	/// Provides the <see cref="ListViewItem"/> for the requested index from <see cref="_results"/>.</summary>
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

	/// <summary>Handles the Click event of the Start Search button.
	/// Validates the selection, then starts the orbital resonance search asynchronously.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The search runs on a background thread. Progress is reported via the progress bar.
	/// The user can cancel at any time using the Cancel button.</remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// Validate that at least one planet is selected before starting the search
		List<string> selectedPlanets = GetSelectedPlanets();
		if (selectedPlanets.Count == 0)
		{
			_ = MessageBox.Show(
				text: "Please select at least one planet.",
				caption: I18nStrings.InformationCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information);
			return;
		}
		if (_planetoids.Count == 0)
		{
			_ = MessageBox.Show(
				text: "No planetoid data available.",
				caption: I18nStrings.InformationCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information);
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

	/// <summary>Handles the Click event of the Cancel button.
	/// Cancels the currently running search.</summary>
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

	/// <summary>Handles the Click event of the copy-to-clipboard menu item.
	/// Copies the text of the currently selected list view row to the clipboard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>All sub-items of the selected row are joined with a tab character before being placed on the clipboard.
	/// If no row is selected the method returns without action.</remarks>
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

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as HTML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as JSON.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQL.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: ListViewExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Markdown.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Markdown files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: ListViewExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as YAML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: ListViewExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: ListViewExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: ListViewExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as LaTeX.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: ListViewExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PostScript.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: ListViewExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as PDF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: ListViewExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as EPUB.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: ListViewExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Word.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Word documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: ListViewExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Excel.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: ListViewExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODT file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODT.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as ODT", exportAction: ListViewExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ODS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: ListViewExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as MOBI.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: ListViewExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as RTF.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: ListViewExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Text files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: ListViewExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as AsciiDoc.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: ListViewExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a reStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a reStructuredText file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as reStructuredText.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: ListViewExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Textile.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: ListViewExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as Abiword.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: ListViewExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as WPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", exportAction: ListViewExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as ET.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", exportAction: ListViewExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as DocBook.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: ListViewExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as TOML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: ListViewExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as XPS.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: ListViewExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as FictionBook2.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: ListViewExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as CHM.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: ListViewExporter.SaveAsChm);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as SQLite.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: ListViewExporter.SaveAsSqlite);

	#endregion

	#region ColumnClick event handler

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles the sort order for the clicked column (ascending/descending) and re-sorts the results list.
	/// Column headers are updated with a ▲ or ▼ indicator to show the current sort direction.</remarks>
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
	/// <remarks>Numeric columns (Planet Period, Planetoid Period, Ratio, Deviation) are sorted numerically;
	/// all other columns are sorted as strings (case-insensitive, ordinal).</remarks>
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
	/// <remarks>When an item is double-clicked, the corresponding planetoid is displayed in the
	/// <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
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