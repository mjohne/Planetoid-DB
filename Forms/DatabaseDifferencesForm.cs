using NLog;

using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.ComponentModel;

namespace Planetoid_DB.Forms;

/// <summary>
/// Form for comparing two MPCORB.DAT files and displaying the differences.
/// </summary>
public partial class DatabaseDifferencesForm : BaseKryptonForm
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	private BackgroundWorker worker;
	private string pathFile1 = string.Empty;
	private string pathFile2 = string.Empty;
	private int addedRecords = 0, deletedRecords = 0, changedRecords = 0;

	private record struct DifferenceResult(string Index, string Designation, string Difference);
	private readonly List<DifferenceResult> differenceResults = [];

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseDifferencesForm"/> class.
	/// </summary>
	public DatabaseDifferencesForm()
	{
		InitializeComponent();
		InitializeBackgroundWorker();
		listViewResults.VirtualMode = true;
		listViewResults.RetrieveVirtualItem += ListViewResults_RetrieveVirtualItem;
		listViewResults.DoubleClick += ListViewResults_DoubleClick;
	}

	private void InitializeBackgroundWorker()
	{
		worker = new BackgroundWorker
		{
			WorkerReportsProgress = true,
			WorkerSupportsCancellation = true
		};
		worker.DoWork += Worker_DoWork;
		worker.ProgressChanged += Worker_ProgressChanged;
		worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
	}

	private void DatabaseDifferences2Form_Load(object sender, EventArgs e)
	{
		// Default to the currently configured MPCORB file
		pathFile1 = Settings.Default.systemFilenameMpcorb;
		if (!File.Exists(pathFile1))
		{
			pathFile1 = string.Empty;
			labelFile1.Text = "No file selected";
		}
		else
		{
			labelFile1.Text = pathFile1;
		}

		pathFile2 = string.Empty;
		labelFile2.Text = "No file selected";
		progressBar.Visible = false;
		buttonCancel.Enabled = false;
	}

	private void ButtonSelectFile1_Click(object sender, EventArgs e)
	{
		using OpenFileDialog dlg = new();
		dlg.Filter = "MPCORB Files (*.DAT)|*.DAT|All Files (*.*)|*.*";
		dlg.Title = "Select Reference MPCORB.DAT";
		if (dlg.ShowDialog() == DialogResult.OK)
		{
			pathFile1 = dlg.FileName;
			labelFile1.Text = pathFile1;
		}
	}

	private void ButtonSelectFile2_Click(object sender, EventArgs e)
	{
		using OpenFileDialog dlg = new();
		dlg.Filter = "MPCORB Files (*.DAT)|*.DAT|All Files (*.*)|*.*";
		dlg.Title = "Select Comparison MPCORB.DAT";
		if (dlg.ShowDialog() == DialogResult.OK)
		{
			pathFile2 = dlg.FileName;
			labelFile2.Text = pathFile2;
		}
	}

	private void ButtonCompare_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(pathFile1) || !File.Exists(pathFile1))
		{
			MessageBox.Show("Please select a valid reference file (File 1).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		if (string.IsNullOrEmpty(pathFile2) || !File.Exists(pathFile2))
		{
			MessageBox.Show("Please select a valid comparison file (File 2).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		differenceResults.Clear();
		listViewResults.VirtualListSize = 0;
		listViewResults.Items.Clear(); // Ensure standard items are cleared if any

		progressBar.Value = 0;
		progressBar.Visible = true;
		buttonCompare.Enabled = false;
		buttonSelectFile1.Enabled = false;
		buttonSelectFile2.Enabled = false;
		buttonCancel.Enabled = true;

		worker.RunWorkerAsync(new string[] { pathFile1, pathFile2 });
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		if (worker.IsBusy)
		{
			worker.CancelAsync();
			buttonCancel.Enabled = false; // Prevent multiple clicks
		}
		else
		{
			Close();
		}
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		if (e.Argument is not string[] paths || paths.Length < 2)
		{
			return;
		}

		string p1 = paths[0];
		string p2 = paths[1];

		// Load File 1 into Dictionary
		var records1 = new Dictionary<string, PlanetoidRecord>();

		worker.ReportProgress(0, "Loading Reference File...");

		foreach (var line in File.ReadLines(p1))
		{
			if (worker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}

#pragma warning disable CA1866 // Char-Überladung verwenden
			if (line.Length > 200 && !line.StartsWith("#"))
			{
				var record = PlanetoidRecord.Parse(line);
				if (!string.IsNullOrEmpty(record.DesignationName))
				{
					records1[record.DesignationName] = record;
				}
			}
#pragma warning restore CA1866 // Char-Überladung verwenden
		}

		worker.ReportProgress(0, "Comparing Files...");

		// Estimate total lines for file 2
		long totalLinesFile2 = 0;
		try
		{
			using var r = new StreamReader(p2);
			while (r.ReadLine() != null)
			{
				totalLinesFile2++;
			}
		}
		catch { /* Ignore */ }

		long currentLine = 0;
		var batchResults = new List<DifferenceResult>();
		long lastReportTicks = DateTime.Now.Ticks;
		long reportIntervalTicks = TimeSpan.FromMilliseconds(100).Ticks;

		using (var reader = new StreamReader(p2))
		{
			string? line;
			while ((line = reader.ReadLine()) != null)
			{
				if (worker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}

				currentLine++;

#pragma warning disable CA1866 // Char-Überladung verwenden
				if (line.Length >= 200 && !line.StartsWith("#"))
				{
					var record2 = PlanetoidRecord.Parse(line);
					if (!string.IsNullOrEmpty(record2.DesignationName))
					{
						if (records1.TryGetValue(record2.DesignationName, out var record1))
						{
							string diff = CompareRecords(record1, record2);
							if (!string.IsNullOrEmpty(diff))
							{
								batchResults.Add(new DifferenceResult(record2.Index, record2.DesignationName, diff));
								changedRecords++;
							}
							records1.Remove(record2.DesignationName);
						}
						else
						{
							batchResults.Add(new DifferenceResult(record2.Index, record2.DesignationName, "Deleted record"));
							deletedRecords++;
						}
					}
				}
#pragma warning restore CA1866 // Char-Überladung verwenden

				long currentTicks = DateTime.Now.Ticks;
				if (currentTicks - lastReportTicks > reportIntervalTicks || currentLine == totalLinesFile2)
				{
					int percent = totalLinesFile2 > 0 ? (int)((double)currentLine / totalLinesFile2 * 100) : 0;
					worker.ReportProgress(percent, new List<DifferenceResult>(batchResults));
					batchResults.Clear();
					lastReportTicks = currentTicks;
				}
			}
		}

		worker.ReportProgress(100, "Checking for added records...");

		foreach (var entry in records1)
		{
			if (worker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}

			batchResults.Add(new DifferenceResult(entry.Value.Index, entry.Key, "Added record"));
			addedRecords++;

			if (batchResults.Count >= 1000)
			{
				worker.ReportProgress(100, new List<DifferenceResult>(batchResults));
				batchResults.Clear();
			}
		}

		if (batchResults.Count > 0)
		{
			worker.ReportProgress(100, new List<DifferenceResult>(batchResults));
		}
		else
		{
			worker.ReportProgress(100);
		}
	}

	private static string CompareRecords(PlanetoidRecord r1, PlanetoidRecord r2)
	{
		var diffs = new List<string>();

		if (r1.Epoch != r2.Epoch)
		{
			diffs.Add($"Epoch: {r1.Epoch} -> {r2.Epoch}");
		}

		if (r1.MeanAnomaly != r2.MeanAnomaly)
		{
			diffs.Add($"MA: {r1.MeanAnomaly} -> {r2.MeanAnomaly}");
		}

		if (r1.ArgPeri != r2.ArgPeri)
		{
			diffs.Add($"ArgPeri: {r1.ArgPeri} -> {r2.ArgPeri}");
		}

		if (r1.LongAscNode != r2.LongAscNode)
		{
			diffs.Add($"LAN: {r1.LongAscNode} -> {r2.LongAscNode}");
		}

		if (r1.Incl != r2.Incl)
		{
			diffs.Add($"Incl: {r1.Incl} -> {r2.Incl}");
		}

		if (r1.OrbEcc != r2.OrbEcc)
		{
			diffs.Add($"Ecc: {r1.OrbEcc} -> {r2.OrbEcc}");
		}

		if (r1.SemiMajorAxis != r2.SemiMajorAxis)
		{
			diffs.Add($"a: {r1.SemiMajorAxis} -> {r2.SemiMajorAxis}");
		}

		if (r1.MagAbs != r2.MagAbs)
		{
			diffs.Add($"H: {r1.MagAbs} -> {r2.MagAbs}");
		}

		if (r1.SlopeParam != r2.SlopeParam)
		{
			diffs.Add($"G: {r1.SlopeParam} -> {r2.SlopeParam}");
		}

		return diffs.Count > 0 ? string.Join("; ", diffs) : string.Empty;
	}

	private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar.Value = Math.Max(0, Math.Min(100, e.ProgressPercentage));
		progressBar.Text = $"{e.ProgressPercentage}%";

		if (e.UserState is string status)
		{
			labelInformation.Text = status;
		}
		else if (e.UserState is List<DifferenceResult> batch)
		{
			if (batch.Count > 0)
			{
				differenceResults.AddRange(batch);
				listViewResults.VirtualListSize = differenceResults.Count;
			}
		}
	}

	private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Cancelled)
		{
			labelInformation.Text = "Comparison Cancelled";
			MessageBox.Show("Comparison cancelled by user.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		else if (e.Error != null)
		{
			logger.Error(e.Error, "Error during comparison");
			labelInformation.Text = "Error occurred";
			MessageBox.Show($"An error occurred: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		else
		{
			labelInformation.Text = "Comparison Complete";
			MessageBox.Show("Comparison completed successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
			MessageBox.Show($"Added records: {addedRecords}\nChanged records: {changedRecords}\nDeleted records: {deletedRecords}");
		}

		buttonCompare.Enabled = true;
		buttonSelectFile1.Enabled = true;
		buttonSelectFile2.Enabled = true;
		buttonCancel.Enabled = true;
		progressBar.Visible = false;
	}

	private void ListViewResults_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
	{
		if (e.ItemIndex >= 0 && e.ItemIndex < differenceResults.Count)
		{
			DifferenceResult result = differenceResults[index: e.ItemIndex];
			e.Item = new ListViewItem(items: [result.Index.ToString(), result.Designation, result.Difference]);
		}
	}

	private void ListViewResults_DoubleClick(object? sender, EventArgs e)
	{
		if (listViewResults.SelectedIndices.Count == 0)
		{
			return;
		}

		int selectedIndex = listViewResults.SelectedIndices[0];
		if (selectedIndex >= 0 && selectedIndex < differenceResults.Count)
		{
			var result = differenceResults[selectedIndex];
			if (result.Difference.Equals("Deleted record", StringComparison.OrdinalIgnoreCase))
			{
				MessageBox.Show("The selected record has been deleted and is no longer available.", "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
				{
					mainForm.JumpToRecord(result.Index, result.Designation);
				}
			}
		}
	}
}
