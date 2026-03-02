// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Planetoid_DB;

/// <summary>
/// Form for finding groups and relationships among planetoid orbital elements.
/// </summary>
/// <remarks>
/// The algorithm receives a copy of the planetoids database, groups the orbital elements
/// into value-range bins based on the configured tolerance, and then identifies groups of
/// planetoids that share common bins across 2 to 7 orbital elements.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class FindRelationshipsForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages and errors for the class.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Cancellation token source used to cancel a running search operation.
	/// </summary>
	/// <remarks>
	/// This token is used to cancel the search operation if needed.
	/// </remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Internal copy of the planetoids database raw lines.
	/// </summary>
	/// <remarks>
	/// Populated via <see cref="FillArray"/> before the form is shown.
	/// </remarks>
	private List<string> database = [];

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="FindRelationshipsForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public FindRelationshipsForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Populates the internal database copy from the supplied raw-line list.
	/// </summary>
	/// <param name="arrTemp">Raw MPCORB lines passed from the main form.</param>
	/// <remarks>
	/// Call this method before showing the form so that the search algorithm has data to work with.
	/// </remarks>
	public void FillArray(List<string> arrTemp)
	{
		// Store a copy of the database to avoid modifying the original
		ArgumentNullException.ThrowIfNull(argument: arrTemp);
		database = [.. arrTemp];
	}

	/// <summary>
	/// Retrieves the names of the orbital elements that the user has checked for inclusion.
	/// </summary>
	/// <returns>A list of element names to include in the search.</returns>
	/// <remarks>
	/// Each element name corresponds to a property of <see cref="PlanetoidRecord"/>.
	/// </remarks>
	private List<string> GetSelectedElements()
	{
		List<string> selected = [];
		if (checkBoxSemiMajorAxis.Checked)
		{
			selected.Add(item: "SemiMajorAxis");
		}
		if (checkBoxOrbEcc.Checked)
		{
			selected.Add(item: "OrbEcc");
		}
		if (checkBoxIncl.Checked)
		{
			selected.Add(item: "Incl");
		}
		if (checkBoxLongAscNode.Checked)
		{
			selected.Add(item: "LongAscNode");
		}
		if (checkBoxArgPeri.Checked)
		{
			selected.Add(item: "ArgPeri");
		}
		if (checkBoxMeanAnomaly.Checked)
		{
			selected.Add(item: "MeanAnomaly");
		}
		if (checkBoxMagAbs.Checked)
		{
			selected.Add(item: "MagAbs");
		}
		if (checkBoxMotion.Checked)
		{
			selected.Add(item: "Motion");
		}
		if (checkBoxSlopeParam.Checked)
		{
			selected.Add(item: "SlopeParam");
		}
		return selected;
	}

	/// <summary>
	/// Extracts the numeric value of a specific orbital element from a <see cref="PlanetoidRecord"/>.
	/// </summary>
	/// <param name="record">The parsed planetoid record.</param>
	/// <param name="elementName">The name of the orbital element property.</param>
	/// <param name="value">The parsed numeric value, or <see cref="double.NaN"/> if parsing fails.</param>
	/// <returns><see langword="true"/> if the value was parsed successfully; otherwise <see langword="false"/>.</returns>
	/// <remarks>
	/// Returns <see langword="false"/> for any element whose raw string cannot be parsed as a double.
	/// </remarks>
	private static bool TryGetElementValue(PlanetoidRecord record, string elementName, out double value)
	{
		// Get the raw string for the element
		string raw = elementName switch
		{
			"SemiMajorAxis" => record.SemiMajorAxis,
			"OrbEcc" => record.OrbEcc,
			"Incl" => record.Incl,
			"LongAscNode" => record.LongAscNode,
			"ArgPeri" => record.ArgPeri,
			"MeanAnomaly" => record.MeanAnomaly,
			"MagAbs" => record.MagAbs,
			"Motion" => record.Motion,
			"SlopeParam" => record.SlopeParam,
			_ => string.Empty
		};
		// Try to parse the raw string as a double
		return double.TryParse(s: raw, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
	}

	/// <summary>
	/// Returns the human-readable label for a given orbital element name.
	/// </summary>
	/// <param name="elementName">Internal element name.</param>
	/// <returns>A descriptive label suitable for display in the results.</returns>
	/// <remarks>
	/// Used to produce human-readable output in the results box.
	/// </remarks>
	private static string GetElementLabel(string elementName) => elementName switch
	{
		"SemiMajorAxis" => "Semi-major axis (a)",
		"OrbEcc" => "Orbital eccentricity (e)",
		"Incl" => "Inclination (i)",
		"LongAscNode" => "Long. ascending node (Ω)",
		"ArgPeri" => "Arg. of perihelion (ω)",
		"MeanAnomaly" => "Mean anomaly (M)",
		"MagAbs" => "Absolute magnitude (H)",
		"Motion" => "Mean daily motion (n)",
		"SlopeParam" => "Slope parameter (G)",
		_ => elementName
	};

	/// <summary>
	/// Generates all combinations of <paramref name="k"/> items chosen from <paramref name="items"/>.
	/// </summary>
	/// <typeparam name="T">Type of each item.</typeparam>
	/// <param name="items">Source list.</param>
	/// <param name="k">Number of items per combination.</param>
	/// <returns>An enumerable of string arrays, each representing one combination.</returns>
	/// <remarks>
	/// Uses a recursive yield approach. The order of items within each combination matches
	/// their original order in <paramref name="items"/>.
	/// </remarks>
	private static IEnumerable<T[]> GetCombinations<T>(IList<T> items, int k)
	{
		// Base case: yield a combination of k items
		if (k == 0)
		{
			yield return [];
			yield break;
		}
		// Iterate through the items and yield combinations
		for (int i = 0; i <= items.Count - k; i++)
		{
			foreach (T[] tail in GetCombinations(items: items.Skip(count: i + 1).ToList(), k: k - 1))
			{
				T[] combination = new T[k];
				combination[0] = items[i];
				Array.Copy(sourceArray: tail, sourceIndex: 0, destinationArray: combination, destinationIndex: 1, length: tail.Length);
				yield return combination;
			}
		}
	}

	/// <summary>
	/// Runs the search algorithm asynchronously, reporting progress and writing results.
	/// </summary>
	/// <param name="token">Token used to cancel the operation.</param>
	/// <returns>A <see cref="Task"/> that completes when the search is finished or cancelled.</returns>
	/// <remarks>
	/// For every combination of <c>k</c> orbital elements (where k ranges from
	/// <c>minK</c> to <c>maxK</c>), the algorithm bins each planetoid's values
	/// using the specified tolerance and groups planetoids that share the same bin
	/// across all selected elements. Groups with at least <c>minMembers</c> members
	/// are written to the results box.
	/// </remarks>
	private async Task RunSearchAsync(CancellationToken token)
	{
		// Retrieve settings from the UI
		int minK = (int)numericUpDownMinRelationships.Value;
		int maxK = (int)numericUpDownMaxRelationships.Value;
		int minMembers = (int)numericUpDownMinGroupMembers.Value;
		double tolerancePercent = (double)numericUpDownTolerance.Value / 100.0;
		List<string> selectedElements = GetSelectedElements();

		// Validate settings
		if (selectedElements.Count < minK)
		{
			SetStatusBar(label: labelInformation, text: "Please select at least as many elements as the minimum relationship count.");
			return;
		}

		// Parse all records once
		SetStatusBar(label: labelInformation, text: "Parsing database records…");
		List<PlanetoidRecord> records = [];
		await Task.Run(action: () =>
		{
			foreach (string line in database)
			{
				token.ThrowIfCancellationRequested();
				PlanetoidRecord rec = PlanetoidRecord.Parse(rawLine: line);
				if (!string.IsNullOrWhiteSpace(value: rec.Index))
				{
					records.Add(item: rec);
				}
			}
		}, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);

		// For each element, compute the value range and bin width
		Dictionary<string, (double min, double binWidth)> elementBinInfo = [];
		await Task.Run(action: () =>
		{
			foreach (string elem in selectedElements)
			{
				token.ThrowIfCancellationRequested();
				List<double> values = [];
				foreach (PlanetoidRecord rec in records)
				{
					if (TryGetElementValue(record: rec, elementName: elem, value: out double v))
					{
						values.Add(item: v);
					}
				}
				if (values.Count == 0)
				{
					continue;
				}
				double minVal = values.Min();
				double maxVal = values.Max();
				double range = maxVal - minVal;
				double binWidth = range > 0 ? range * tolerancePercent : 1.0;
				elementBinInfo[elem] = (minVal, binWidth);
			}
		}, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);

		// Filter to elements that have valid bin info
		List<string> usableElements = selectedElements.Where(predicate: e => elementBinInfo.ContainsKey(key: e)).ToList();

		// Cap maxK to number of usable elements
		maxK = Math.Min(val1: maxK, val2: usableElements.Count);
		if (maxK < minK)
		{
			SetStatusBar(label: labelInformation, text: "Not enough usable elements for the requested relationship count.");
			return;
		}

		// Count total combinations for progress reporting
		int totalCombinations = 0;
		for (int k = minK; k <= maxK; k++)
		{
			totalCombinations += CountCombinations(n: usableElements.Count, k: k);
		}

		StringBuilder results = new();
		int totalGroupsFound = 0;
		int processedCombinations = 0;

		// Search across each k-size combination of elements
		for (int k = minK; k <= maxK; k++)
		{
			token.ThrowIfCancellationRequested();

			foreach (string[] elementCombo in GetCombinations(items: usableElements, k: k))
			{
				token.ThrowIfCancellationRequested();

				// Group planetoids by their bin-tuple for this element combination
				Dictionary<string, List<PlanetoidRecord>> groups = [];
				await Task.Run(action: () =>
				{
					foreach (PlanetoidRecord rec in records)
					{
						token.ThrowIfCancellationRequested();
						StringBuilder keyBuilder = new();
						bool allParsed = true;
						foreach (string elem in elementCombo)
						{
							if (!TryGetElementValue(record: rec, elementName: elem, value: out double v) ||
								!elementBinInfo.TryGetValue(key: elem, value: out (double min, double binWidth) info))
							{
								allParsed = false;
								break;
							}
							long bin = (long)Math.Floor(d: (v - info.min) / info.binWidth);
							_ = keyBuilder.Append(value: $"{elem}:{bin}|");
						}
						if (!allParsed)
						{
							continue;
						}
						string key = keyBuilder.ToString();
						if (!groups.TryGetValue(key: key, value: out List<PlanetoidRecord>? list))
						{
							list = [];
							groups[key] = list;
						}
						list.Add(item: rec);
					}
				}, cancellationToken: token).ConfigureAwait(continueOnCapturedContext: true);

				// Report groups that meet the minimum member threshold
				foreach (KeyValuePair<string, List<PlanetoidRecord>> kvp in groups)
				{
					if (kvp.Value.Count >= minMembers)
					{
						totalGroupsFound++;
						// Build range descriptions for each element in this combination
						StringBuilder groupHeader = new();
						_ = groupHeader.AppendLine(value: $"── Group {totalGroupsFound} ({k} related elements, {kvp.Value.Count} planetoids) ──");
						foreach (string elem in elementCombo)
						{
							if (elementBinInfo.TryGetValue(key: elem, value: out (double min, double binWidth) info))
							{
								// Extract the bin number from the key to compute the range
								string binTag = $"{elem}:";
								int start = kvp.Key.IndexOf(value: binTag, comparisonType: StringComparison.Ordinal);
								if (start >= 0)
								{
									int numStart = start + binTag.Length;
									int numEnd = kvp.Key.IndexOf(value: '|', startIndex: numStart);
									if (numEnd > numStart && long.TryParse(s: kvp.Key[numStart..numEnd], result: out long bin))
									{
										double rangeMin = info.min + bin * info.binWidth;
										double rangeMax = rangeMin + info.binWidth;
										_ = groupHeader.AppendLine(value: $"   {GetElementLabel(elementName: elem)}: [{rangeMin:F4} – {rangeMax:F4}]");
									}
								}
							}
						}
						_ = groupHeader.AppendLine();
						// List planetoids in the group
						foreach (PlanetoidRecord member in kvp.Value.OrderBy(keySelector: r => r.Index))
						{
							_ = groupHeader.AppendLine(value: $"   {member.Index,-8} {member.DesignationName}");
						}
						_ = groupHeader.AppendLine();

						string groupText = groupHeader.ToString();
						results.Append(value: groupText);

						// Append to the RichTextBox on the UI thread periodically
						if (totalGroupsFound % 10 == 0)
						{
							string snapshot = results.ToString();
							results.Clear();
							richTextBoxResults.Invoke(action: () =>
							{
								richTextBoxResults.AppendText(text: snapshot);
							});
						}
					}
				}

				// Update progress
				processedCombinations++;
				int percent = totalCombinations > 0 ? (int)(processedCombinations * 100L / totalCombinations) : 0;
				int capturedPercent = percent;
				Invoke(action: () =>
				{
					if (kryptonProgressBar.Value != capturedPercent)
					{
						kryptonProgressBar.Value = capturedPercent;
						kryptonProgressBar.Text = $"{capturedPercent}%";
					}
					SetStatusBar(label: labelInformation, text: $"Searching… {capturedPercent}% ({totalGroupsFound} groups found so far)");
				});
			}
		}

		// Flush remaining results
		if (results.Length > 0)
		{
			string remaining = results.ToString();
			richTextBoxResults.Invoke(action: () => richTextBoxResults.AppendText(text: remaining));
		}

		// Final status
		Invoke(action: () =>
		{
			kryptonProgressBar.Value = 100;
			kryptonProgressBar.Text = "100%";
			SetStatusBar(label: labelInformation, text: $"Search complete. {totalGroupsFound} group(s) found.");
		});
	}

	/// <summary>
	/// Computes the binomial coefficient C(n, k).
	/// </summary>
	/// <param name="n">Total number of items.</param>
	/// <param name="k">Number of items per combination.</param>
	/// <returns>The number of combinations.</returns>
	/// <remarks>
	/// Used for progress bar calculation.
	/// </remarks>
	private static int CountCombinations(int n, int k)
	{
		// Return 0 if k is greater than n
		if (k > n)
		{
			return 0;
		}
		// Use Pascal's triangle approach for small values
		if (k == 0 || k == n)
		{
			return 1;
		}
		// Optimize by using the smaller of k and n-k
		k = Math.Min(val1: k, val2: n - k);
		long result = 1;
		for (int i = 0; i < k; i++)
		{
			result = result * (n - i) / (i + 1);
		}
		return (int)result;
	}

	/// <summary>
	/// Resets the UI to idle state and re-enables the start button.
	/// </summary>
	/// <remarks>
	/// Called after a search completes or is cancelled.
	/// </remarks>
	private void SetIdleState()
	{
		toolStripButtonStart.Enabled = true;
		toolStripButtonCancel.Enabled = false;
		numericUpDownMinRelationships.Enabled = true;
		numericUpDownMaxRelationships.Enabled = true;
		numericUpDownTolerance.Enabled = true;
		numericUpDownMinGroupMembers.Enabled = true;
		checkBoxSemiMajorAxis.Enabled = true;
		checkBoxOrbEcc.Enabled = true;
		checkBoxIncl.Enabled = true;
		checkBoxLongAscNode.Enabled = true;
		checkBoxArgPeri.Enabled = true;
		checkBoxMeanAnomaly.Enabled = true;
		checkBoxMagAbs.Enabled = true;
		checkBoxMotion.Enabled = true;
		checkBoxSlopeParam.Enabled = true;
	}

	/// <summary>
	/// Sets the UI to the running state and disables settings controls.
	/// </summary>
	/// <remarks>
	/// Called when the search starts.
	/// </remarks>
	private void SetRunningState()
	{
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		numericUpDownMinRelationships.Enabled = false;
		numericUpDownMaxRelationships.Enabled = false;
		numericUpDownTolerance.Enabled = false;
		numericUpDownMinGroupMembers.Enabled = false;
		checkBoxSemiMajorAxis.Enabled = false;
		checkBoxOrbEcc.Enabled = false;
		checkBoxIncl.Enabled = false;
		checkBoxLongAscNode.Enabled = false;
		checkBoxArgPeri.Enabled = false;
		checkBoxMeanAnomaly.Enabled = false;
		checkBoxMagAbs.Enabled = false;
		checkBoxMotion.Enabled = false;
		checkBoxSlopeParam.Enabled = false;
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the Load event of the form.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Clears the status bar and resets the progress bar on form load.
	/// </remarks>
	private void FindRelationshipsForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// Reset the progress bar
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = string.Empty;
	}

	/// <summary>
	/// Handles the FormClosing event to cancel any running search.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Ensures that the background task is cancelled cleanly when the form is closed.
	/// </remarks>
	private void FindRelationshipsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Cancel any running search when the form closes
		cancellationTokenSource?.Cancel();
		cancellationTokenSource?.Dispose();
		cancellationTokenSource = null;
	}

	#endregion

	#region toolbar event handlers

	/// <summary>
	/// Handles the Click event of the Start button.
	/// Starts the group-finding algorithm in the background.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// The search runs on a thread-pool thread; results are marshalled back to the UI thread.
	/// </remarks>
	private async void ButtonStart_Click(object sender, EventArgs e)
	{
		// Validate that at least one element is selected
		if (GetSelectedElements().Count == 0)
		{
			ShowErrorMessage(message: "Please select at least one orbital element to search.");
			return;
		}

		// Cancel any previous search
		cancellationTokenSource?.Cancel();
		cancellationTokenSource?.Dispose();
		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = cancellationTokenSource.Token;

		// Clear previous results
		richTextBoxResults.Clear();
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0%";

		// Switch UI to running state
		SetRunningState();

		try
		{
			await RunSearchAsync(token: token).ConfigureAwait(continueOnCapturedContext: true);
		}
		catch (OperationCanceledException)
		{
			SetStatusBar(label: labelInformation, text: "Search cancelled.");
			logger.Info(message: "Relationship search was cancelled by the user.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"An error occurred during the search: {ex.Message}");
		}
		finally
		{
			SetIdleState();
		}
	}

	/// <summary>
	/// Handles the Click event of the Cancel button.
	/// Cancels the running search operation.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// Signals the cancellation token; the search task will stop at the next check point.
	/// </remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		// Cancel the running search
		cancellationTokenSource?.Cancel();
		SetStatusBar(label: labelInformation, text: "Cancelling search…");
	}

	#endregion
}
