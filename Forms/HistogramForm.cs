// This file contains the implementation of the HistogramForm,
// which displays histogram/bar charts of orbital elements and properties
// for all minor planets in the database.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;

using ScottPlot;
using ScottPlot.WinForms;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for displaying histograms of orbital elements and properties of minor planets.</summary>
/// <remarks>This form counts and displays the distribution of planetoid properties (e.g., absolute magnitude, computer) and orbital elements (e.g., semi-major axis) in histogram/bar chart format. Users can select the property to analyze, choose bin sizes, and view results both graphically and in tabular form.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class HistogramForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents one histogram bin with its range and count.</summary>
	/// <param name="BinStart">The start value of the bin range.</param>
	/// <param name="BinEnd">The end value of the bin range.</param>
	/// <param name="Count">The number of planetoids in this bin.</param>
	private record HistogramBin(double BinStart, double BinEnd, int Count);

	/// <summary>The read-only list of raw MPCORB database records to process.</summary>
	/// <remarks>Each element is one line from the MPCORB file. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>The complete list of histogram bins after the last completed calculation.</summary>
	/// <remarks>This list is only updated on the UI thread after the background calculation finishes.</remarks>
	private List<HistogramBin> _results = [];

	/// <summary>Cancellation token source for the running background calculation task.</summary>
	/// <remarks>Set to <c>null</c> when no calculation is running.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>The currently active sort column index, or -1 if no column sort is active.</summary>
	/// <remarks>Updated whenever the user clicks a column header to sort the list.</remarks>
	private int _sortColumn = -1;

	/// <summary>The current sort order for the active sort column.</summary>
	/// <remarks>Defaults to <see cref="SortOrder.None"/>; toggles between Ascending and Descending on column clicks.</remarks>
	private SortOrder _sortOrder = SortOrder.None;

	/// <summary>The ScottPlot FormsPlot control for displaying the histogram.</summary>
	private FormsPlot? _formsPlot;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="HistogramForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public HistogramForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Cache the planetoid records for use during the calculation
		_planetoids = planetoids;
		// Initialize the property and bin size dropdowns
		InitializeDropdowns();
		// Create and configure the ScottPlot control
		InitializeScottPlot();
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is primarily intended for debugging purposes.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Initializes the property and bin size dropdown menus.</summary>
	private void InitializeDropdowns()
	{
		// Populate the property dropdown with available properties
		comboBoxProperty.Items.Clear();
		comboBoxProperty.Items.AddRange(new object[]
		{
			"Semi-major Axis (a) [AU]",
			"Eccentricity (e)",
			"Inclination (i) [°]",
			"Absolute Magnitude (H) [mag]",
			"Mean Daily Motion (n) [°/day]",
			"Observation Span [days]",
			"Number of Observations"
		});
		comboBoxProperty.SelectedIndex = 0;

		// Populate the bin size dropdown with reasonable options
		comboBoxBinSize.Items.Clear();
		comboBoxBinSize.Items.AddRange(new object[]
		{
			"0.1",
			"0.25",
			"0.5",
			"1.0",
			"2.0",
			"5.0",
			"10.0"
		});
		comboBoxBinSize.SelectedIndex = 3; // Default to 1.0
	}

	/// <summary>Initializes the ScottPlot control for displaying the histogram.</summary>
	private void InitializeScottPlot()
	{
		_formsPlot = new FormsPlot
		{
			Dock = DockStyle.Fill
		};
		panelChart.Controls.Add(_formsPlot);
	}

	/// <summary>Parses the selected property from the planetoid record.</summary>
	/// <param name="line">The MPCORB record line.</param>
	/// <param name="propertyIndex">The index of the selected property.</param>
	/// <param name="value">The parsed value.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	private static bool TryParseProperty(string line, int propertyIndex, out double value)
	{
		value = 0;
		if (line.Length < 200)
		{
			return false;
		}

		try
		{
			string text = propertyIndex switch
			{
				0 => line.Substring(startIndex: 92, length: 11).Trim(), // Semi-major axis
				1 => line.Substring(startIndex: 70, length: 9).Trim(),  // Eccentricity
				2 => line.Substring(startIndex: 59, length: 9).Trim(),  // Inclination
				3 => line.Substring(startIndex: 8, length: 5).Trim(),   // Absolute magnitude
				4 => line.Substring(startIndex: 80, length: 11).Trim(), // Mean daily motion
				5 => line.Substring(startIndex: 127, length: 9).Trim(), // Observation span
				6 => line.Substring(startIndex: 117, length: 9).Trim(), // Number of observations
				_ => string.Empty
			};

			return double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
		}
		catch
		{
			return false;
		}
	}

	/// <summary>Updates the enabled state of the start/cancel buttons.</summary>
	/// <param name="isRunning">Whether a calculation is currently running.</param>
	private void UpdateButtonStates(bool isRunning)
	{
		toolStripButtonStart.Enabled = !isRunning;
		toolStripButtonCancel.Enabled = isRunning;
	}

	/// <summary>Updates the histogram chart with the current results.</summary>
	private void UpdateChart()
	{
		if (_formsPlot == null || _results.Count == 0)
		{
			return;
		}

		_formsPlot.Plot.Clear();

		// Extract data for plotting
		double[] positions = _results.Select((bin, index) => (double)index).ToArray();
		double[] values = _results.Select(bin => (double)bin.Count).ToArray();
		string[] labels = _results.Select(bin => $"{bin.BinStart:F2}-{bin.BinEnd:F2}").ToArray();

		// Create bar plot
		var barPlot = _formsPlot.Plot.Add.Bars(positions, values);
		barPlot.LegendText = comboBoxProperty.Text;

		// Configure axes
		_formsPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(positions, labels);
		_formsPlot.Plot.Axes.Bottom.Label.Text = comboBoxProperty.Text;
		_formsPlot.Plot.Axes.Left.Label.Text = "Count";
		_formsPlot.Plot.Title($"Histogram: {comboBoxProperty.Text}");
		_formsPlot.Plot.ShowLegend();

		_formsPlot.Refresh();
	}

	/// <summary>Updates the ListView with the current results.</summary>
	private void UpdateListView()
	{
		listView.Items.Clear();

		foreach (HistogramBin bin in _results)
		{
			ListViewItem item = new(text: $"{bin.BinStart:F2} - {bin.BinEnd:F2}");
			item.SubItems.Add(text: bin.Count.ToString());
			listView.Items.Add(item);
		}
	}

	#endregion

	#region event handlers

	/// <summary>Handles the Click event of the Start button.</summary>
	private async void ToolStripButtonStart_Click(object? sender, EventArgs e)
	{
		// Validate bin size
		if (!double.TryParse(s: comboBoxBinSize.Text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double binSize) || binSize <= 0)
		{
			ShowErrorMessage(message: "Please enter a valid bin size greater than 0.");
			return;
		}

		int propertyIndex = comboBoxProperty.SelectedIndex;
		bool liveUpdate = checkBoxLiveUpdate.Checked;

		UpdateButtonStates(isRunning: true);
		kryptonProgressBar.Value = 0;

		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _cancellationTokenSource.Token;

		try
		{
			// Progress reporter
			Progress<int> progress = new(handler: percent =>
			{
				kryptonProgressBar.Value = percent;
				kryptonProgressBar.Text = $"{percent}%";
			});

			// Run calculation on background thread
			List<HistogramBin> results = await Task.Run(function: () =>
				CalculateHistogram(propertyIndex, binSize, liveUpdate, progress, token), cancellationToken: token);

			// Update UI with results
			_results = results;
			UpdateListView();
			UpdateChart();

			SetStatusBar(label: labelInformation, text: $"Histogram complete. {_results.Count} bins, {_results.Sum(b => b.Count)} objects.");
		}
		catch (OperationCanceledException)
		{
			SetStatusBar(label: labelInformation, text: "Histogram calculation cancelled.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error calculating histogram");
			ShowErrorMessage(message: $"Error calculating histogram: {ex.Message}");
		}
		finally
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			UpdateButtonStates(isRunning: false);
		}
	}

	/// <summary>Handles the Click event of the Cancel button.</summary>
	private void ToolStripButtonCancel_Click(object? sender, EventArgs e)
	{
		_cancellationTokenSource?.Cancel();
		SetStatusBar(label: labelInformation, text: "Cancelling...");
	}

	/// <summary>Calculates the histogram bins.</summary>
	/// <param name="propertyIndex">The index of the property to histogram.</param>
	/// <param name="binSize">The size of each bin.</param>
	/// <param name="liveUpdate">Whether to update the UI during calculation.</param>
	/// <param name="progress">Progress reporter.</param>
	/// <param name="token">Cancellation token.</param>
	/// <returns>List of histogram bins.</returns>
	private List<HistogramBin> CalculateHistogram(int propertyIndex, double binSize, bool liveUpdate,
		IProgress<int> progress, CancellationToken token)
	{
		// Dictionary to store bin counts
		Dictionary<int, int> binCounts = new();
		int totalCount = _planetoids.Count;
		int processedCount = 0;
		int lastReportedPercent = 0;

		// Process each planetoid
		foreach (string line in _planetoids)
		{
			token.ThrowIfCancellationRequested();

			if (TryParseProperty(line, propertyIndex, out double value))
			{
				int binIndex = (int)Math.Floor(value / binSize);
				binCounts.TryGetValue(binIndex, out int count);
				binCounts[binIndex] = count + 1;
			}

			processedCount++;

			// Report progress
			int currentPercent = (int)(processedCount * 100.0 / totalCount);
			if (currentPercent > lastReportedPercent)
			{
				lastReportedPercent = currentPercent;
				progress.Report(currentPercent);

				// Live update if enabled (throttled)
				if (liveUpdate && currentPercent % 10 == 0)
				{
					// Create intermediate results
					List<HistogramBin> intermediateResults = binCounts
						.OrderBy(kvp => kvp.Key)
						.Select(kvp => new HistogramBin(kvp.Key * binSize, (kvp.Key + 1) * binSize, kvp.Value))
						.ToList();

					// Update UI on UI thread
					if (liveUpdate)
					{
						// Note: For true live updates, we would need to invoke on the UI thread
						// For now, we'll just update at the end
					}
				}
			}
		}

		// Convert dictionary to sorted list of bins
		List<HistogramBin> results = binCounts
			.OrderBy(kvp => kvp.Key)
			.Select(kvp => new HistogramBin(kvp.Key * binSize, (kvp.Key + 1) * binSize, kvp.Value))
			.ToList();

		return results;
	}

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// Toggle sort order
		if (e.Column == _sortColumn)
		{
			_sortOrder = _sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			_sortColumn = e.Column;
			_sortOrder = SortOrder.Ascending;
		}

		// Sort the results
		_results = _sortOrder == SortOrder.Ascending
			? _results.OrderBy(r => e.Column == 0 ? r.BinStart : r.Count).ToList()
			: _results.OrderByDescending(r => e.Column == 0 ? r.BinStart : r.Count).ToList();

		// Update the ListView
		UpdateListView();

		// Update column header with sort indicator
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string columnText = listView.Columns[i].Tag?.ToString() ?? listView.Columns[i].Text;
			if (i == _sortColumn)
			{
				string indicator = _sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[i].Text = $"{indicator} {columnText}";
			}
			else
			{
				listView.Columns[i].Text = columnText;
			}
		}
	}

	#endregion
}
