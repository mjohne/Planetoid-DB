using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Globalization;
using System.Text;


namespace Planetoid_DB;

/// <summary>
/// Form to detect and display potential asteroid families based on orbital elements (a, e, i).
/// Uses a binning algorithm to group planetoids whose semi-major axis, eccentricity, and inclination
/// fall within user-defined tolerance ranges.
/// </summary>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AsteroidFamiliesForm : BaseKryptonForm
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

	private List<AsteroidFamily> _families = [];
	private AsteroidFamily? _selectedFamily;

	/// <summary>
	/// Initializes a new instance of the <see cref="AsteroidFamiliesForm"/> class.
	/// </summary>
	/// <param name="planetoids">The list of raw planetoid data lines from the MPCORB database.</param>
	public AsteroidFamiliesForm(IReadOnlyList<string> planetoids)
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

	/// <summary>
	/// Represents parsed orbital parameters for a single planetoid.
	/// </summary>
	private sealed record PlanetoidEntry(
		string Index,
		string Name,
		double SemiMajorAxis,
		double Eccentricity,
		double Inclination,
		double MeanAnomaly,
		double ArgPeri,
		double LongAscNode);

	/// <summary>
	/// Represents a detected asteroid family with its member list.
	/// </summary>
	private sealed class AsteroidFamily
	{
		/// <summary>Gets or sets the display name of this family.</summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>Gets the list of member planetoids.</summary>
		public List<PlanetoidEntry> Members { get; } = [];
	}

	/// <summary>
	/// Starts the asteroid family detection when the Start button is clicked.
	/// </summary>
	private void BtnStart_Click(object? sender, EventArgs e)
	{
		if (_planetoids.Count == 0)
		{
			KryptonMessageBox.Show(
				text: "No planetoid data available.",
				caption: "Information",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information);
			return;
		}

		btnStart.Enabled = false;
		btnCancel.Enabled = true;
		btnSaveSelected.Enabled = false;
		btnSaveAll.Enabled = false;

		treeViewFamilies.Nodes.Clear();
		listViewMembers.VirtualListSize = 0;
		_families.Clear();
		_selectedFamily = null;

		progressBar.Value = 0;
		lblProgress.Text = "0%";

		double tolA = (double)numTolA.Value;
		double tolE = (double)numTolE.Value;
		double tolI = (double)numTolI.Value;
		int minMembers = (int)numMinMembers.Value;

		_cancellationTokenSource = new CancellationTokenSource();

		var progress = new Progress<int>(percent =>
		{
			progressBar.Value = percent;
			lblProgress.Text = $"{percent}%";
		});

		Task.Run(
			() => PerformDetectionAsync(tolA, tolE, tolI, minMembers, progress, _cancellationTokenSource.Token),
			_cancellationTokenSource.Token);
	}

	/// <summary>
	/// Cancels the ongoing detection when the Cancel button is clicked.
	/// </summary>
	private void BtnCancel_Click(object? sender, EventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			btnCancel.Enabled = false;
		}
	}

	/// <summary>
	/// Cancels any active detection and releases resources when the form is closing.
	/// </summary>
	private void AsteroidFamiliesForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}
	}

	/// <summary>
	/// Performs the asteroid family detection asynchronously using an orbital element binning algorithm.
	/// </summary>
	/// <param name="tolA">Tolerance for semi-major axis binning (AU).</param>
	/// <param name="tolE">Tolerance for eccentricity binning.</param>
	/// <param name="tolI">Tolerance for inclination binning (degrees).</param>
	/// <param name="minMembers">Minimum number of members required to qualify as a family.</param>
	/// <param name="progress">Progress reporter (0–100).</param>
	/// <param name="cancellationToken">Token to cancel the operation.</param>
	private async Task PerformDetectionAsync(
		double tolA,
		double tolE,
		double tolI,
		int minMembers,
		IProgress<int> progress,
		CancellationToken cancellationToken)
	{
		try
		{
			// Phase 1: Parse planetoid orbital elements (progress 0–40 %)
			var parsedData = new List<PlanetoidEntry>(_planetoids.Count);
			int total = _planetoids.Count;

			for (int i = 0; i < total; i++)
			{
				cancellationToken.ThrowIfCancellationRequested();

				string line = _planetoids[i];
				if (line.Length >= 103)
				{
					string index = line[..7].Trim();
					string name = line.Length >= 194 ? line.Substring(166, 28).Trim() : string.Empty;

					if (double.TryParse(line.Substring(92, 11).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double a) &&
						double.TryParse(line.Substring(70, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double ecc) &&
						double.TryParse(line.Substring(59, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double incl) &&
						double.TryParse(line.Substring(26, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double meanAnomaly) &&
						double.TryParse(line.Substring(37, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double argPeri) &&
						double.TryParse(line.Substring(48, 9).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double longAscNode))
					{
						parsedData.Add(new PlanetoidEntry(index, name, a, ecc, incl, meanAnomaly, argPeri, longAscNode));
					}
				}

				if (i % 5000 == 0)
				{
					progress.Report(i * 40 / total);
				}
			}

			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(40);

			// Phase 2: Group planetoids into bins by (a, e, i) (progress 40–90 %)
			var bins = new Dictionary<(int binA, int binE, int binI), List<PlanetoidEntry>>();
			int count = parsedData.Count;

			for (int i = 0; i < count; i++)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var p = parsedData[i];
				var key = (
					binA: (int)Math.Floor(p.SemiMajorAxis / tolA),
					binE: (int)Math.Floor(p.Eccentricity / tolE),
					binI: (int)Math.Floor(p.Inclination / tolI)
				);

				if (!bins.TryGetValue(key, out var list))
				{
					list = [];
					bins[key] = list;
				}

				list.Add(p);

				if (i % 5000 == 0)
				{
					progress.Report(40 + (int)((long)i * 50 / count));
				}
			}

			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(90);

			// Phase 3: Filter and build family list (progress 90–100 %)
			var families = bins.Values
				.Where(g => g.Count >= minMembers)
				.OrderByDescending(g => g.Count)
				.Select((g, idx) =>
				{
					var family = new AsteroidFamily
					{
						Name = $"Family {idx + 1} ({g.Count} members)"
					};
					family.Members.AddRange(g.OrderBy(p => p.SemiMajorAxis));
					return family;
				})
				.ToList();

			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(value: 100);

			// Update UI on the UI thread
			await InvokeAsync(callback: () =>
			{
				_families = families;
				PopulateTreeView();
				btnSaveAll.Enabled = _families.Count > 0;
			}, cancellationToken: cancellationToken);
		}
		catch (OperationCanceledException)
		{
			// Detection was cancelled by the user — no action needed.
		}
		catch (Exception ex)
		{
			await InvokeAsync(callback: () =>
				KryptonMessageBox.Show(
					text: $"An error occurred during family detection: {ex.Message}",
					caption: "Error",
					buttons: KryptonMessageBoxButtons.OK,
					icon: KryptonMessageBoxIcon.Error), cancellationToken: cancellationToken);
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

	/// <summary>
	/// Populates the tree view with the detected asteroid families.
	/// </summary>
	private void PopulateTreeView()
	{
		treeViewFamilies.BeginUpdate();
		treeViewFamilies.Nodes.Clear();
		for (int i = 0; i < _families.Count; i++)
		{
			var node = new TreeNode(_families[i].Name) { Tag = i };
			treeViewFamilies.Nodes.Add(node);
		}

		treeViewFamilies.EndUpdate();
	}

	/// <summary>
	/// Handles TreeView node selection and populates the member ListView accordingly.
	/// </summary>
	private void TreeViewFamilies_AfterSelect(object? sender, TreeViewEventArgs e)
	{
		if (e.Node?.Tag is int idx && idx >= 0 && idx < _families.Count)
		{
			_selectedFamily = _families[idx];
			listViewMembers.VirtualListSize = _selectedFamily.Members.Count;
			listViewMembers.Invalidate();
			btnSaveSelected.Enabled = true;
		}
	}

	/// <summary>
	/// Provides ListView items on demand for VirtualMode.
	/// </summary>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		if (_selectedFamily == null || e.ItemIndex >= _selectedFamily.Members.Count)
		{
			e.Item = new ListViewItem("?");
			return;
		}

		var p = _selectedFamily.Members[e.ItemIndex];
		var item = new ListViewItem(p.Index);
		item.SubItems.Add(p.Name);
		item.SubItems.Add(p.SemiMajorAxis.ToString("F4", CultureInfo.InvariantCulture));
		item.SubItems.Add(p.Eccentricity.ToString("F4", CultureInfo.InvariantCulture));
		item.SubItems.Add(p.Inclination.ToString("F4", CultureInfo.InvariantCulture));
		item.SubItems.Add(p.MeanAnomaly.ToString("F4", CultureInfo.InvariantCulture));
		item.SubItems.Add(p.ArgPeri.ToString("F4", CultureInfo.InvariantCulture));
		item.SubItems.Add(p.LongAscNode.ToString("F4", CultureInfo.InvariantCulture));
		e.Item = item;
	}

	/// <summary>
	/// Saves the currently selected family to a text file.
	/// </summary>
	private void BtnSaveSelected_Click(object? sender, EventArgs e)
	{
		if (_selectedFamily == null)
		{
			return;
		}

		SaveFamiliesToFile([_selectedFamily]);
	}

	/// <summary>
	/// Saves all detected families to a single text file.
	/// </summary>
	private void BtnSaveAll_Click(object? sender, EventArgs e)
	{
		if (_families.Count == 0)
		{
			return;
		}

		SaveFamiliesToFile(_families);
	}

	/// <summary>
	/// Opens a save dialog and writes the specified families to a text file.
	/// </summary>
	/// <param name="families">The families to export.</param>
	private static void SaveFamiliesToFile(IReadOnlyList<AsteroidFamily> families)
	{
		string defaultFileName = families.Count == 1
			? (families[0].Name.Contains('(')
				? families[0].Name[..families[0].Name.IndexOf('(', StringComparison.Ordinal)].Trim().Replace(' ', '_')
				: families[0].Name.Replace(' ', '_'))
			: "AsteroidFamilies";

		using SaveFileDialog dlg = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			FileName = defaultFileName,
			Title = families.Count == 1 ? "Save Selected Family" : "Save All Families"
		};

		if (dlg.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		var sb = new StringBuilder();
		string header = $"{"Index",-10} {"Name",-30} {"a (AU)",-12} {"e",-10} {"i (°)",-10} {"M (°)",-12} {"ArgPeri (°)",-14} {"LongAscNode (°)",-16}";
		string separator = new('-', header.Length);

		foreach (var family in families)
		{
			sb.AppendLine($"=== {family.Name} ===");
			sb.AppendLine(header);
			sb.AppendLine(separator);
			foreach (var p in family.Members)
			{
				sb.AppendLine(
					$"{p.Index,-10} {p.Name,-30} {p.SemiMajorAxis,-12:F4} {p.Eccentricity,-10:F4} {p.Inclination,-10:F4} {p.MeanAnomaly,-12:F4} {p.ArgPeri,-14:F4} {p.LongAscNode,-16:F4}");
			}

			sb.AppendLine();
		}

		try
		{
			File.WriteAllText(path: dlg.FileName, contents: sb.ToString(), encoding: Encoding.UTF8);
			KryptonMessageBox.Show(
				text: $"Successfully saved to:{Environment.NewLine}{dlg.FileName}",
				caption: "Saved",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information);
		}
		catch (IOException ex)
		{
			KryptonMessageBox.Show(
				text: $"Failed to save the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
		}
		catch (UnauthorizedAccessException ex)
		{
			KryptonMessageBox.Show(
				text: $"You do not have permission to save the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
		}
		catch (Exception ex)
		{
			KryptonMessageBox.Show(
				text: $"An unexpected error occurred while saving the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Error);
		}
	}
}
