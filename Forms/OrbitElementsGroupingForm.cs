using Krypton.Toolkit;

using NLog;

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Planetoid_DB.Forms;

/// <summary>
/// Form to analyze and group planetoids based on common orbital element ranges.
/// </summary>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitElementsGroupingForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used throughout the form to log important events and errors.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label used for displaying information in the status bar.
	/// </summary>
	/// <remarks>
	/// Overrides the base class property to return the form-specific status label.
	/// </remarks>
	//protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	private readonly IReadOnlyList<string> _planetoids;
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>
	/// Initializes a new instance of the <see cref="OrbitElementsGroupingForm"/> class.
	/// </summary>
	/// <param name="planetoids">The planetoid string records to process from the database.</param>
	public OrbitElementsGroupingForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		_planetoids = planetoids;
	}

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	private void BtnStart_Click(object? sender, EventArgs e)
	{
		if (_planetoids.Count == 0)
		{
			KryptonMessageBox.Show("No planetoid data available.", "Information", KryptonMessageBoxButtons.OK, KryptonMessageBoxIcon.Information);
			return;
		}

		btnStart.Enabled = false;
		btnCancel.Enabled = true;
		txtOutput.Clear();
		progressBar.Value = 0;
		lblProgress.Text = "0%";

		int elementsCount = (int)numElementsToCompare.Value;
		double tolerancePercent = (double)numTolerance.Value / 100.0;

		_cancellationTokenSource = new CancellationTokenSource();

		var progress = new Progress<int>(percent =>
		{
			progressBar.Value = percent;
			lblProgress.Text = $"{percent}%";
		});

		var messageProgress = new Progress<string>(message =>
		{
			txtOutput.AppendText(message + Environment.NewLine);
		});

		Task.Run(() => PerformGroupingAsync(elementsCount, tolerancePercent, progress, messageProgress, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
	}

	private void BtnCancel_Click(object? sender, EventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			btnCancel.Enabled = false;
		}
	}

	private void OrbitElementsGroupingForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}
	}

	private async Task PerformGroupingAsync(int elementsCount, double tolerancePercent, IProgress<int> progress, IProgress<string> messageProgress, CancellationToken cancellationToken)
	{
		try
		{
			messageProgress.Report("Parsing data...");

			// Parse valid orbital parameters
			var parsedData = new List<PlanetoidData>();
			object parseLock = new();
			int parsedCount = 0;
			int totalRecords = _planetoids.Count;

			_planetoids.AsParallel().WithCancellation(cancellationToken).ForAll(line =>
			{
				if (line.Length >= 103)
				{
					string index = line[..7].Trim();
					string designation = line.Length >= 194 ? line.Substring(166, 28).Trim() : "";

					if (double.TryParse(line.Substring(26, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double meanAnomaly) &&
						double.TryParse(line.Substring(37, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double argPeri) &&
						double.TryParse(line.Substring(48, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double longAscNode) &&
						double.TryParse(line.Substring(59, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double incl) &&
						double.TryParse(line.Substring(70, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double orbEcc) &&
						double.TryParse(line.Substring(80, 11).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double motion) &&
						double.TryParse(line.Substring(92, 11).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double semiMajorAxis))
					{
						double[] elements = [meanAnomaly, argPeri, longAscNode, incl, orbEcc, motion, semiMajorAxis];
						lock (parseLock)
						{
							parsedData.Add(item: new PlanetoidData(Index: index, Name: designation, Elements: elements));
						}
					}
				}

				int currentCount = Interlocked.Increment(ref parsedCount);
				if (currentCount % 1000 == 0)
				{
					progress.Report(currentCount * 10 / totalRecords); // 10% step for parsing
				}
			});

			cancellationToken.ThrowIfCancellationRequested();

			messageProgress.Report("Extracting element combinations and grouping...");

			int combinationThreshold = elementsCount;

			// Define combinations
			int[] indices = [0, 1, 2, 3, 4, 5, 6];
			List<int[]> combinations = [.. GetKCombinations(elements: indices, k: combinationThreshold)];

			int comboIndex = 0;
			int totalCombos = combinations.Count;

			foreach (int[] combo in combinations)
			{
				cancellationToken.ThrowIfCancellationRequested();

				messageProgress.Report($"Analyzing combination {comboIndex + 1}/{totalCombos}...");
				// A simplified algorithm to cluster them using a generic grouping mechanism (binning with tolerance)
				var sortedData = parsedData.OrderBy(d => d.Elements[combo[0]]).ToList();
				var clusters = new List<List<PlanetoidData>>();
				var currentCluster = new List<PlanetoidData>();

				for (int i = 0; i < sortedData.Count; i++)
				{
					cancellationToken.ThrowIfCancellationRequested();

					var current = sortedData[i];
					if (currentCluster.Count == 0)
					{
						currentCluster.Add(current);
					}
					else
					{
						var rep = currentCluster[0];
						bool similar = true;
						foreach (int idx in combo)
						{
							double diff = Math.Abs(current.Elements[idx] - rep.Elements[idx]);
							double threshold = Math.Max(0.001, rep.Elements[idx] * tolerancePercent);
							if (diff > threshold)
							{
								similar = false;
								break;
							}
						}

						if (similar)
						{
							currentCluster.Add(item: current);
						}
						else
						{
							if (currentCluster.Count > 1)
							{
								clusters.Add(item: [.. currentCluster]);
							}
							currentCluster.Clear();
							currentCluster.Add(item: current);
						}
					}

					if (i % 5000 == 0)
					{
						int currentProgress = 10 + (int)(90.0 * (((double)comboIndex / totalCombos) + ((double)i / sortedData.Count / totalCombos)));
						progress.Report(value: currentProgress);
					}
				}

				if (currentCluster.Count > 1)
				{
					clusters.Add(currentCluster);
				}

				// Output clusters
				if (clusters.Count != 0)
				{
					StringBuilder sb = new();
					sb.AppendLine($"--- Clusters for elements {string.Join(", ", combo.Select(GetElementName))} ---");

					var orderedClusters = clusters.OrderByDescending(c => c.Count).Take(999); // Show top 999 groups
					foreach (var group in orderedClusters)
					{
						sb.AppendLine($"Found group with {group.Count} planetoids (Representative: {group[0].Index} - {group[0].Name}):");
						foreach (var p in group.Take(999))
						{
							sb.AppendLine($"  {p.Index} '{p.Name}' " + string.Join(", ", combo.Select(c => $"{GetElementName(c)}={p.Elements[c]:F4}")));
						}
						//if (group.Count > 5) sb.AppendLine("  ...");
						sb.AppendLine();
					}
					messageProgress.Report(sb.ToString());
				}

				comboIndex++;
			}

			progress.Report(100);
			messageProgress.Report("Search completed successfully.");
		}
		catch (OperationCanceledException)
		{
			messageProgress.Report("Search canceled by user.");
		}
		catch (Exception ex)
		{
			messageProgress.Report($"Error during processing: {ex.Message}");
		}
		finally
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			await InvokeAsync(callback: () =>
			{
				btnStart.Enabled = true;
				btnCancel.Enabled = false;
			}, cancellationToken: cancellationToken);
		}
	}

	private static string GetElementName(int index) => index switch
	{
		0 => "MeanAnomaly",
		1 => "ArgPeri",
		2 => "LongAscNode",
		3 => "Incl",
		4 => "OrbEcc",
		5 => "Motion",
		6 => "SemiMajorAxis",
		_ => "Unknown"
	};

	private static List<int[]> GetKCombinations(int[] elements, int k)
	{
		if (k == 0)
		{
			return [.. new[] { Array.Empty<int>() }];
		}

		if (elements.Length == 0)
		{
			return [.. Array.Empty<int[]>()];
		}

		if (elements.Length == k)
		{
			return [.. new[] { elements }];
		}

		List<int[]> result = [];
		foreach ((int[]? c, int[]? res) in
		// combinations with first element
		from c in GetKCombinations([.. elements.Skip(count: 1)], k: k - 1)
		let res = new int[k]
		select (c, res))
		{
			res[0] = elements[0];
			Array.Copy(sourceArray: c, sourceIndex: 0, destinationArray: res, destinationIndex: 1, length: k - 1);
			result.Add(item: res);
		}
		// combinations without first element
		result.AddRange(collection: GetKCombinations(elements: [.. elements.Skip(count: 1)], k: k));

		return result;
	}

	private record PlanetoidData(string Index, string Name, double[] Elements);
}
