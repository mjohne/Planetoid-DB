// This file contains the implementation of the MoidsOfAllMinorPlanetsForm,
// which computes and displays MOID values for all minor planets
// relative to each of the eight solar system planets.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for displaying the Minimum Orbit Intersection Distance (MOID) of all minor planets
/// relative to each of the eight solar system planets.</summary>
/// <remarks>This form iterates over all planetoids in the database and computes their MOIDs with respect
/// to all eight planets. Results are presented in a ListView where each row corresponds to one planetoid
/// and the eight MOID columns correspond to Mercury through Neptune. The user can start and cancel the
/// calculation at any time and track progress via the integrated progress bar.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class MoidsOfAllMinorPlanetsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents one row in the MOID results list: the planetoid name and one MOID value per planet.</summary>
	/// <param name="PlanetoidName">The designation of the minor planet.</param>
	/// <param name="Moids">Array of eight MOID values in AU, one per planet in order Mercury–Neptune.</param>
	/// <remarks>The <paramref name="Moids"/> array always has exactly eight elements corresponding to the
	/// eight solar system planets: Mercury (0), Venus (1), Earth (2), Mars (3), Jupiter (4), Saturn (5),
	/// Uranus (6), Neptune (7).</remarks>
	private record MoidRowResult(string PlanetoidName, double[] Moids);

	/// <summary>Number of planets whose MOIDs are computed (Mercury through Neptune).</summary>
	/// <remarks>This constant matches the number of planets in <see cref="MoidCalculator"/>.</remarks>
	private const int PlanetCount = 8;

	/// <summary>Zero-based column index of the Planetoid name column.</summary>
	/// <remarks>Used for sorting comparisons against the specific column type.</remarks>
	private const int ColumnIndexPlanetoid = 0;

	/// <summary>Prefix length added to column headers to display sort indicators (e.g., "▲ " or "▼ ").</summary>
	/// <remarks>This is used to trim the sort indicator prefix when updating column headers.</remarks>
	private const int SortIndicatorPrefixLength = 2;

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>The complete list of MOID results after the last completed calculation.</summary>
	/// <remarks>This list is only updated on the UI thread after the background calculation finishes.</remarks>
	private List<MoidRowResult> _results = [];

	/// <summary>Cancellation token source for the running background calculation task.</summary>
	/// <remarks>Set to <c>null</c> when no calculation is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>The currently active sort column index, or -1 if no column sort is active.</summary>
	/// <remarks>Updated whenever the user clicks a column header to sort the list.</remarks>
	private int sortColumn = -1;

	/// <summary>The current sort order for the active sort column.</summary>
	/// <remarks>Defaults to <see cref="SortOrder.None"/>; toggles between Ascending and Descending on column clicks.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MoidsOfAllMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public MoidsOfAllMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Cache the planetoid records for use during the calculation; this allows the calculation method to access the raw data without needing to pass it around or access it from a shared resource
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
		dialog.FileName = $"MoidsOfAllMinorPlanets_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
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
		// Create and configure the save file dialog with the specified filter, default extension, and title.
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
		// If the user selects a file and confirms the dialog, set the cursor to a wait cursor to indicate that an operation is in progress
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(listView, "MOIDs of all minor planets", saveFileDialog.FileName, null);
		}
		// Handle any exceptions that may occur during the export action
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during export: {ex}");
			MessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cursor is reset to the default state regardless of whether the export action succeeds or fails.
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Updates the progress bar value and text label.</summary>
	/// <param name="percent">Progress value from 0 to 100.</param>
	/// <remarks>The percentage is displayed both in the progress bar's <c>Text</c> property and in the adjacent label.</remarks>
	private void UpdateProgress(int percent)
	{
		// Clamp the percentage value to ensure it stays within the valid range of 0 to 100
		int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
		kryptonProgressBar.Value = clampedPercent;
		kryptonProgressBar.Text = $"{clampedPercent}%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
	}

	/// <summary>Processes one raw MPCORB database record and appends the MOID result row to <paramref name="results"/>.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="results">The list to which matching <see cref="MoidRowResult"/> items are appended.</param>
	/// <remarks>Lines that are too short or have invalid orbital elements are silently skipped.</remarks>
	private static void ProcessPlanetoidLine(string line, List<MoidRowResult> results)
	{
		// Validate the line length to ensure it contains the required orbital element fields
		if (line.Length < 103)
		{
			return;
		}
		// Extract and parse the semi-major axis (positions 92-102)
		string semiMajorAxisText = line.Substring(startIndex: 92, length: 11).Trim();
		if (!double.TryParse(s: semiMajorAxisText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis) || semiMajorAxis <= 0)
		{
			return;
		}
		// Extract and parse the eccentricity (positions 70-78)
		string eccentricityText = line.Substring(startIndex: 70, length: 9).Trim();
		if (!double.TryParse(s: eccentricityText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double eccentricity))
		{
			return;
		}
		// Extract and parse the inclination to the ecliptic (positions 59-67)
		string inclinationText = line.Substring(startIndex: 59, length: 9).Trim();
		if (!double.TryParse(s: inclinationText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double inclinationDeg))
		{
			return;
		}
		// Extract and parse the longitude of the ascending node (positions 48-56)
		string longitudeText = line.Substring(startIndex: 48, length: 9).Trim();
		if (!double.TryParse(s: longitudeText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longitudeAscendingNodeDeg))
		{
			return;
		}
		// Extract and parse the argument of perihelion (positions 37-45)
		string argumentText = line.Substring(startIndex: 37, length: 9).Trim();
		if (!double.TryParse(s: argumentText, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argumentPerihelionDeg))
		{
			return;
		}
		// Extract the designation (readable designation or packed designation)
		string designation = line.Length >= 194
			? line.Substring(startIndex: 166, length: 28).Trim()
			: line[..7].Trim();
		if (string.IsNullOrEmpty(value: designation))
		{
			designation = line[..7].Trim();
		}
		// Calculate the MOIDs for all eight planets
		List<MoidCalculator.MoidResult> moidResults = MoidCalculator.CalculateMoids(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Build the MOID array in planet order (Mercury … Neptune)
		double[] moids = new double[PlanetCount];
		for (int i = 0; i < Math.Min(val1: moidResults.Count, val2: PlanetCount); i++)
		{
			moids[i] = moidResults[index: i].MoidAu;
		}
		results.Add(item: new MoidRowResult(PlanetoidName: designation, Moids: moids));
	}

	/// <summary>Sorts <see cref="_results"/> by the currently selected column and sort order.</summary>
	/// <remarks>Column 0 (Planetoid) is sorted as a string; all other columns (MOID values) are sorted numerically.</remarks>
	private void SortResults()
	{
		int col = sortColumn;
		bool ascending = sortOrder == SortOrder.Ascending;
		if (col == ColumnIndexPlanetoid)
		{
			_results = ascending
				? [.. _results.OrderBy(keySelector: static r => r.PlanetoidName, comparer: StringComparer.OrdinalIgnoreCase)]
				: [.. _results.OrderByDescending(keySelector: static r => r.PlanetoidName, comparer: StringComparer.OrdinalIgnoreCase)];
		}
		else if (col >= 1 && col <= PlanetCount)
		{
			int planetIndex = col - 1;
			_results = ascending
				? [.. _results.OrderBy(keySelector: r => r.Moids[planetIndex])]
				: [.. _results.OrderByDescending(keySelector: r => r.Moids[planetIndex])];
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event.
	/// Clears the status bar on startup.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>Clears the status bar when the form is loaded.</remarks>
	private void MoidsOfAllMinorPlanetsForm_Load(object sender, EventArgs e) =>
		ClearStatusBar(label: labelInformation);

	/// <summary>Handles the FormClosing event.
	/// Cancels any running calculation and disposes the cancellation token source.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>Cancels any running calculation and disposes the cancellation token source when the form is closing.</remarks>
	private void MoidsOfAllMinorPlanetsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// If a calculation is currently running, signal cancellation and dispose of the token source
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
		MoidRowResult result = _results[index: e.ItemIndex];
		ListViewItem item = new(text: result.PlanetoidName);
		string[] subItems = new string[PlanetCount];
		for (int i = 0; i < PlanetCount; i++)
		{
			subItems[i] = result.Moids[i].ToString(format: "F6");
		}
		item.SubItems.AddRange(items: subItems);
		e.Item = item;
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start Calculation button.
	/// Validates the input, then starts the MOID calculation for all minor planets asynchronously.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The calculation runs on a background thread. Progress is reported via the progress bar.
	/// The user can cancel at any time using the Cancel button.</remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		if (_planetoids.Count == 0)
		{
			_ = MessageBox.Show(
				text: "No planetoid data available.",
				caption: I18nStrings.InformationCaption,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information);
			return;
		}
		// Disable the Start button and save menu during the calculation
		toolStripDropDownButtonSaveToFile.Enabled = false;
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		_results = [];
		listView.VirtualListSize = 0;
		UpdateProgress(percent: 0);
		ClearStatusBar(label: labelInformation);
		// Create a new cancellation token source for the calculation task
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		// Create a local list to store results from the background calculation
		List<MoidRowResult> localResults = [];
		// Create a progress reporter that updates the progress bar on the UI thread
		IProgress<int> progress = new Progress<int>(handler: UpdateProgress);
		try
		{
			// Calculate the total number of planetoids and determine the interval at which to report progress
			int total = _planetoids.Count;
			int reportInterval = Math.Max(val1: 1, val2: total / 100);
			// Run the calculation on a background thread
			await Task.Run(action: () =>
			{
				for (int i = 0; i < total; i++)
				{
					token.ThrowIfCancellationRequested();
					ProcessPlanetoidLine(line: _planetoids[index: i], results: localResults);
					if (i % reportInterval == 0 || i == total - 1)
					{
						progress.Report(value: (i + 1) * 100 / total);
					}
				}
				logger.Info(message: $"MOID calculation completed. Total results: {localResults.Count}");
			}, cancellationToken: token);
		}
		// Catch the OperationCanceledException to handle user cancellation gracefully
		catch (OperationCanceledException)
		{
			logger.Info(message: "MOID calculation cancelled by user.");
		}
		// Catch any other exceptions that may occur during the calculation
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error during calculation: {ex.Message}");
		}
		// Finally block to clean up resources and reset UI elements after the calculation completes or is cancelled
		finally
		{
			try
			{
				if (IsHandleCreated && !IsDisposed && !Disposing)
				{
					_results = localResults;
					listView.VirtualListSize = _results.Count;
					listView.Refresh();
					toolStripButtonStart.Enabled = true;
					toolStripButtonCancel.Enabled = false;
					toolStripDropDownButtonSaveToFile.Enabled = _results.Count > 0;
				}
			}
			// Catch ObjectDisposedException and InvalidOperationException that may occur if the form is closing
			catch (ObjectDisposedException)
			{
				// Ignore exceptions caused by controls being disposed during form shutdown.
			}
			catch (InvalidOperationException)
			{
				// Ignore exceptions related to invalid control state during form shutdown.
			}
			// Dispose of the cancellation token source to free resources
			finally
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = null;
			}
		}
	}

	/// <summary>Handles the Click event of the Cancel button.
	/// Cancels the currently running calculation.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>The calculation can be cancelled by the user at any time using the Cancel button.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// If a calculation is currently running, request cancellation and prevent repeated cancel clicks.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CSV file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an HTML file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XML file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a JSON file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQL file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: ListViewExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Markdown file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Markdown files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: ListViewExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a YAML file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: ListViewExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TSV file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: ListViewExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PSV file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: ListViewExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a LaTeX file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: ListViewExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PostScript file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: ListViewExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a PDF file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: ListViewExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an EPUB file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: ListViewExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Word file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Word documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: ListViewExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Excel file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: ListViewExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODT file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODT file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as ODT", exportAction: ListViewExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ODS file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: ListViewExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a MOBI file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: ListViewExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an RTF file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: ListViewExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Text files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: ListViewExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an AsciiDoc file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: ListViewExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a reStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a reStructuredText file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: ListViewExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a Textile file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: ListViewExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an Abiword file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: ListViewExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a WPS file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", exportAction: ListViewExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an ET file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", exportAction: ListViewExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a DocBook file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: ListViewExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a TOML file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: ListViewExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as an XPS file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: ListViewExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a FictionBook2 file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: ListViewExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a CHM file.</remarks>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: ListViewExporter.SaveAsChm);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a SQLite file.</remarks>
	/// <param name="sender">The source of the event.</param>
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
		// If there are no results to sort, exit the method early
		if (_results.Count == 0)
		{
			return;
		}
		// Check if the clicked column is the same as the current sort column; if so, toggle the sort order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to reflect the current sort column and direction
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ", comparisonType: StringComparison.Ordinal) || headerText.StartsWith(value: "▼ ", comparisonType: StringComparison.Ordinal))
			{
				headerText = headerText[SortIndicatorPrefixLength..];
			}
			listView.Columns[index: i].Text = i == sortColumn
				? $"{(sortOrder == SortOrder.Ascending ? "▲" : "▼")} {headerText}"
				: headerText;
		}
		// Re-sort the results and refresh the ListView
		SortResults();
		listView.Refresh();
	}

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item is double-clicked, the corresponding planetoid is displayed in the
	/// <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListView_DoubleClick(object sender, EventArgs e)
	{
		// Ensure that an item is selected
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
		int index = listView.SelectedIndices[index: 0];
		if (index < 0 || index >= _results.Count)
		{
			return;
		}
		MoidRowResult result = _results[index];
		// If the Owner of this form is a PlanetoidDbForm, call its JumpToRecord method
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			planetoidDbForm.JumpToRecord(index: result.PlanetoidName, designation: result.PlanetoidName);
		}
	}

	#endregion
}
