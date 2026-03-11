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

	/// <summary>The deviation threshold in percent below which an orbital ratio is considered a near-resonance.</summary>
	/// <remarks>This value is used to determine if a computed orbital resonance is significant.</remarks>
	private const double ResonanceThresholdPercent = 1.0;

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

	/// <summary>Returns the list of planet names currently selected via the planet checkboxes.</summary>
	/// <returns>A list of planet name strings that are checked.</returns>
	/// <remarks>Planet names match those used in <see cref="DerivedElements.CalculateOrbitalResonances"/>.</remarks>
	private List<string> GetSelectedPlanets()
	{
		List<string> selected = [];
		if (checkBoxMercury.Checked)
		{
			selected.Add(item: "Mercury");
		}

		if (checkBoxVenus.Checked)
		{
			selected.Add(item: "Venus");
		}

		if (checkBoxEarth.Checked)
		{
			selected.Add(item: "Earth");
		}

		if (checkBoxMars.Checked)
		{
			selected.Add(item: "Mars");
		}

		if (checkBoxJupiter.Checked)
		{
			selected.Add(item: "Jupiter");
		}

		if (checkBoxSaturn.Checked)
		{
			selected.Add(item: "Saturn");
		}

		if (checkBoxUranus.Checked)
		{
			selected.Add(item: "Uranus");
		}

		if (checkBoxNeptune.Checked)
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
	private async void BtnStart_Click(object sender, EventArgs e)
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
		btnStart.Enabled = false;
		btnCancel.Enabled = true;
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
					btnStart.Enabled = true;
					btnCancel.Enabled = false;
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
	private void BtnCancel_Click(object sender, EventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			btnCancel.Enabled = false;
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

	#endregion

	#region static helpers

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
}