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

	/// <summary>Returns the list of planet names currently selected via the planet checkbuttons.</summary>
	/// <returns>A list of planet name strings that are checked.</returns>
	/// <remarks>Planet names match those used in <see cref="DerivedElements.CalculateOrbitalResonances"/>.</remarks>
	private List<string> GetSelectedPlanets()
	{
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
		int clampedPercent = Math.Clamp(value: percent, min: 0, max: 100);
		kryptonProgressBar.Value = clampedPercent;
		kryptonProgressBar.Text = $"{clampedPercent}%";
		TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)clampedPercent, progressMax: 100);
	}

	/// <summary>Processes one raw MPCORB database record and appends matching resonance results to <paramref name="results"/>.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="selectedPlanets">The planet names to include in the resonance comparison.</param>
	/// <param name="results">The list to which matching <see cref="ResonanceResult"/> items are appended.</param>
	/// <remarks>Lines that are too short or have an invalid semi-major axis are silently skipped.
	/// Only resonances whose planet name is in <paramref name="selectedPlanets"/> are added.</remarks>
	private static void ProcessPlanetoidLine(string line, List<string> selectedPlanets, List<ResonanceResult> results)
	{
		if (line.Length < 103)
		{
			return;
		}
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
		// Collect all resonances for the selected planets. Any near-resonance
		// classification (e.g. Yes/No indicators) is handled at a higher level.
		List<DerivedElements.OrbitalResonance> resonances = DerivedElements.CalculateOrbitalResonances(semiMajorAxis: semiMajorAxis);
		foreach (DerivedElements.OrbitalResonance resonance in resonances)
		{
			if (selectedPlanets.Contains(item: resonance.PlanetName))
			{
				results.Add(item: new ResonanceResult(PlanetoidName: designation, Resonance: resonance));
			}
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
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}
	}

	#endregion

	#region ListView virtual mode

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
		string isResonance = result.Resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No";
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
		e.Item = item;
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
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		_results = [];
		listView.VirtualListSize = 0;
		UpdateProgress(percent: 0);
		ClearStatusBar(label: labelInformation);
		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;
		List<ResonanceResult> localResults = [];
		IProgress<int> progress = new Progress<int>(handler: UpdateProgress);
		try
		{
			int total = _planetoids.Count;
			int reportInterval = Math.Max(val1: 1, val2: total / 100);
			await Task.Run(action: () =>
			{
				for (int i = 0; i < total; i++)
				{
					token.ThrowIfCancellationRequested();
					ProcessPlanetoidLine(line: _planetoids[i], selectedPlanets: selectedPlanets, results: localResults);
					if (i % reportInterval == 0 || i == total - 1)
					{
						progress.Report(value: (i + 1) * 100 / total);
					}
				}
			}, cancellationToken: token);
		}
		catch (OperationCanceledException)
		{
			logger.Info(message: "Orbital resonance search cancelled by user.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error during search: {ex.Message}");
		}
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
				}
			}
			catch (ObjectDisposedException)
			{
				// Ignore exceptions caused by controls being disposed during form shutdown.
			}
			catch (InvalidOperationException)
			{
				// Ignore exceptions related to invalid control state during form shutdown.
			}
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

	/// <summary>Saves the current list as a CSV file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As CSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CSV file to save the list view results; if the user confirms the save operation, call the SaveAsCsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save as CSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsCsv(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an HTML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As HTML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the HTML file to save the list view results; if the user confirms the save operation, call the SaveAsHtml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "HTML files (*.html)|*.html|All Files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save as HTML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsHtml(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an XML file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As XML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XML file to save the list view results; if the user confirms the save operation, call the SaveAsXml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XML files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as XML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsXml(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a JSON file.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As JSON" menu item.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the JSON file to save the list view results; if the user confirms the save operation, call the SaveAsJson method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "JSON files (*.json)|*.json|All Files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save as JSON"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsJson(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a SQL script.
	/// Exports the list as a series of SQL INSERT statements.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As SQL" menu item.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQL file to save the list view results; if the user confirms the save operation, call the SaveAsSql method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save as SQL"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsSql(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a Markdown table.
	/// Ideal for documentation, GitHub Readmes, or Wikis.</summary>
	/// <param name="e">Event arguments.</param>
	/// <param name="sender">Event source (the menu item).</param>
	/// <remarks>This method is invoked when the user selects the "Save As Markdown" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Markdown file to save the list view results; if the user confirms the save operation, call the SaveAsMarkdown method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Markdown files (*.md)|*.md|All Files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save as Markdown"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsMarkdown(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the list in YAML format.
	/// A human-readable data serialization standard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As YAML" menu item.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the YAML file to save the list view results; if the user confirms the save operation, call the SaveAsYaml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save as YAML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsYaml(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the list as a TSV (Tab-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As TSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TSV file to save the list view results; if the user confirms the save operation, call the SaveAsTsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save as TSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsTsv(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the list as a PSV (Pipe-Separated Values) file.
	/// Ideal for spreadsheet applications.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PSV" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PSV file to save the list view results; if the user confirms the save operation, call the SaveAsPsv method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save as PSV"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsPsv(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the list as a LaTeX document.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As LaTeX" menu item.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the LaTeX file to save the list view results; if the user confirms the save operation, call the SaveAsLatex method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save as LaTeX"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsLatex(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a PostScript (.ps) file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PostScript" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PostScript file to save the list view results; if the user confirms the save operation, call the SaveAsPostScript method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save as PostScript"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsPostScript(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an uncompressed PDF file.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As PDF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the PDF file to save the list view results; if the user confirms the save operation, call the SaveAsPdf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save as PDF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsPdf(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an EPUB file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As EPUB" menu item.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the EPUB file to save the list view results; if the user confirms the save operation, call the SaveAsEpub method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save as EPUB"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsEpub(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a Word document.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Word" menu item.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Word file to save the list view results; if the user confirms the save operation, call the SaveAsWord method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Word documents (*.docx)|*.docx|All Files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save as Word"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsWord(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an Excel file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Excel" menu item.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Excel file to save the list view results; if the user confirms the save operation, call the SaveAsExcel method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save as Excel"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsExcel(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an ODT file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODT" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Text file to save the list view results; if the user confirms the save operation, call the SaveAsOdt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save as ODT"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsOdt(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an ODS file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As ODS" menu item.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the OpenDocument Spreadsheet file to save the list view results; if the user confirms the save operation, call the SaveAsOds method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save as ODS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsOds(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a simplified MOBI file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As MOBI" menu item.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the MOBI file to save the list view results; if the user confirms the save operation, call the SaveAsMobi method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save as MOBI"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsMobi(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as an RTF file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As RTF" menu item.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Rich Text Format file to save the list view results; if the user confirms the save operation, call the SaveAsRtf method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save as RTF"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsRtf(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Saves the current list as a text file.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	/// <remarks>This method is invoked when the user selects the "Save As Text" menu item.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the text file to save the list view results; if the user confirms the save operation, call the SaveAsText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Text files (*.txt)|*.txt|All Files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save as Text"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsText(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save As AsciiDoc' menu item and initiates saving the ListView results in AsciiDoc
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as an AsciiDoc-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the AsciiDoc file to save the list view results; if the user confirms the save operation, call the SaveAsAsciiDoc method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*",
			DefaultExt = "adoc",
			Title = "Save as AsciiDoc"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsAsciiDoc(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save As reStructuredText' menu item and initiates saving the current ListView
	/// results in reStructuredText format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables users to export the current ListView results as a reStructuredText-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the reStructuredText file to save the list view results; if the user confirms the save operation, call the SaveAsReStructuredText method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*",
			DefaultExt = "rst",
			Title = "Save as reStructuredText"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsReStructuredText(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event of the 'Save As Textile' menu item and initiates saving the ListView results in Textile
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler is typically connected to a ToolStripMenuItem in the user interface. It enables
	/// users to export the current ListView results as a Textile-formatted file.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Textile file to save the list view results; if the user confirms the save operation, call the SaveAsTextile method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Textile files (*.textile)|*.textile|All Files (*.*)|*.*",
			DefaultExt = "textile",
			Title = "Save as Textile"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsTextile(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save As Abiword' menu item and initiates saving the current list view results in
	/// Abiword format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Abiword" menu item, this event handler is invoked. It calls the SaveListViewResultsAsAbiword method, which generates an AWML (AbiWord XML) file with a .abw extension that can be opened in Abiword. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the Abiword file to save the list view results; if the user confirms the save operation, call the SaveAsAbiword method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*",
			DefaultExt = "abw",
			Title = "Save as Abiword"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsAbiword(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As WPS menu item and initiates saving the current ListView results in WPS
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the Save As WPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As WPS" menu item, this event handler is invoked. It calls the SaveAsWps method, which generates an HTML file with a .wps extension that can be opened in WPS Writer. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Writer file to save the list view results; if the user confirms the save operation, call the SaveAsWps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*",
			DefaultExt = "wps",
			Title = "Save as WPS Writer"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsWps(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the 'Save As Et' menu item and initiates saving the current ListView results in the Et
	/// format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As Et" menu item, this event handler is invoked. It calls the SaveAsEt method, which exports the data in a format compatible with WPS Spreadsheets (using CSV internally). If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the WPS Spreadsheets file to save the list view results; if the user confirms the save operation, call the SaveAsEt method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*",
			DefaultExt = "et",
			Title = "Save as WPS Spreadsheets"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsEt(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save As DocBook' menu item, initiating the process to save the current list view
	/// results in DocBook format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As DocBook" menu item, this event handler is invoked. It calls the SaveAsDocBook method, which generates an XML document conforming to the DocBook schema, containing the Orbital resonances. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the DocBook file to save the list view results; if the user confirms the save operation, call the SaveAsDocBook method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save as DocBook"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsDocBook(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the click event for the 'Save As TOML' menu item and initiates saving the current results in TOML format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As TOML" menu item, this event handler is invoked. It calls the SaveAsToml method, which generates the necessary TOML structure for the current results and saves it as a .toml file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the TOML file to save the list view results; if the user confirms the save operation, call the SaveAsToml method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*",
			DefaultExt = "toml",
			Title = "Save as TOML"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsToml(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As XPS menu item and initiates saving the current ListView results as an XPS
	/// document.</summary>
	/// <param name="sender">The source of the event, typically the Save As XPS menu item.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As XPS" menu item, this event handler is invoked. It calls the SaveAsXps method, which generates the necessary XML structure for an XPS document and saves it as a .xps file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the XPS file to save the list view results; if the user confirms the save operation, call the SaveAsXps method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*",
			DefaultExt = "xps",
			Title = "Save as XPS"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsXps(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As FictionBook2 menu item and initiates saving the current results in
	/// FictionBook2 format.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As FictionBook2" menu item, this event handler is invoked. It calls the SaveAsFictionBook2 method, which generates an XML document conforming to the FictionBook2 schema, containing the Orbital resonances. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the FictionBook2 file to save the list view results; if the user confirms the save operation, call the SaveAsFictionBook2 method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*",
			DefaultExt = "fb2",
			Title = "Save as FictionBook2"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsFictionBook2(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As CHM menu item and initiates saving the current ListView results as a CHM
	/// file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As CHM" menu item, this event handler is invoked. It calls the SaveAsChm method, which generates the necessary HTML and project files, then uses Microsoft HTML Help Workshop to compile them into a CHM file. If the process is successful, a confirmation message is displayed; otherwise, an error message is shown.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the CHM file to save the list view results; if the user confirms the save operation, call the SaveAsChm method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*",
			DefaultExt = "chm",
			Title = "Save as CHM"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsChm(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	/// <summary>Handles the Click event of the Save As SQLite menu item and initiates saving the current ListView results as a SQLite
	/// file.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the user clicks the "Save As SQLite" menu item, this event handler is invoked. It calls the SaveAsSqlite method.</remarks>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
	{
		// Open a SaveFileDialog to allow the user to specify the location and name of the SQLite file to save the list view results; if the user confirms the save operation, call the SaveAsSqlite method to perform the export
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*",
			DefaultExt = "sqlite",
			Title = "Save as SQLite"
		};
		if (!PrepareSaveDialog(dialog: saveFileDialog, ext: saveFileDialog.DefaultExt))
		{
			return;
		}
		ListViewExporter.SaveAsSqlite(listView: listView, title: "Orbital resonances", fileName: saveFileDialog.FileName);
	}

	#endregion

	#region ColumnClick event handler

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles the sort order for the clicked column (ascending/descending) and re-sorts the results list.
	/// Column headers are updated with a ▲ or ▼ indicator to show the current sort direction.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (_results.Count == 0)
		{
			return;
		}
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
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
		SortResults();
		listView.Refresh();
	}

	/// <summary>Sorts the <see cref="_results"/> list by the currently selected column and sort order.</summary>
	/// <remarks>Numeric columns (Planet Period, Planetoid Period, Ratio, Deviation) are sorted numerically;
	/// all other columns are sorted as strings (case-insensitive, ordinal).</remarks>
	private void SortResults()
	{
		int col = sortColumn;
		bool ascending = sortOrder == SortOrder.Ascending;
		_results = col switch
		{
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
		ColumnIndexPlanetoid => result.PlanetoidName,
		ColumnIndexPlanet => result.Resonance.PlanetName,
		ColumnIndexResonance => $"{result.Resonance.ResonanceP}:{result.Resonance.ResonanceQ}",
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
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			planetoidDbForm.JumpToRecord(index: result.PlanetoidName, designation: result.PlanetoidName);
		}
	}

	#endregion
}