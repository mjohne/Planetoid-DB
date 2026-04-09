// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Planetoid_DB;

/// <summary>Form to detect and display potential asteroid families based on orbital elements (a, e, i).
/// Uses a binning algorithm to group planetoids whose semi-major axis, eccentricity, and inclination
/// fall within user-defined tolerance ranges.</summary>
/// <remarks>This form allows users to detect potential asteroid families by analyzing the orbital elements of planetoids from the MPCORB database. The detection is performed using a binning algorithm that groups planetoids based on their semi-major axis, eccentricity, and inclination, with user-defined tolerances. The results are displayed in a tree view, and users can view the members of each family in a list view. The form also provides options to save the detected families to text files.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AsteroidFamiliesForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Gets the list of raw planetoid data lines from the MPCORB database.</summary>
	/// <remarks>This list is used as the source data for detecting asteroid families.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Gets or sets the cancellation token source used to cancel the ongoing detection.</summary>
	/// <remarks>This token source is used to signal cancellation requests to the detection task.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Gets the list of detected asteroid families.</summary>
	/// <remarks>This list is used to store the results of the asteroid family detection process.</remarks>
	private List<AsteroidFamily> _families = [];

	/// <summary>Gets or sets the currently selected asteroid family.</summary>
	/// <remarks>This property is used to track the user's selection in the UI.</remarks>
	private AsteroidFamily? _selectedFamily;

	/// <summary>Stores the index of the currently sorted column in the member ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int _sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the member ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header in the member list view to toggle the sort order.</remarks>
	private SortOrder _sortOrder = SortOrder.None;

	/// <summary>Initializes a new instance of the <see cref="AsteroidFamiliesForm"/> class.</summary>
	/// <param name="planetoids">The list of raw planetoid data lines from the MPCORB database.</param>
	public AsteroidFamiliesForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		_planetoids = planetoids;
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Represents parsed orbital parameters for a single planetoid.</summary>
	/// <remarks>This record is used to store the orbital elements of a planetoid for family detection.</remarks>
	private sealed record PlanetoidEntry(
		string Index, // MPCORB index or provisional designation
		string Name, // Proper name if available, otherwise empty
		double SemiMajorAxis, // a in AU
		double Eccentricity, // e
		double Inclination, // i in degrees
		double MeanAnomaly, // M in degrees
		double ArgPeri, // ω in degrees
		double LongAscNode); // Ω in degrees

	/// <summary>Represents a detected asteroid family with its member list.</summary>
	/// <remarks>This class is used to store the members of a detected asteroid family.</remarks>
	private sealed class AsteroidFamily
	{
		/// <summary>Gets or sets the display name of this family.</summary>
		/// <remarks>This property is used to store the name of the asteroid family.</remarks>
		public string Name { get; set; } = string.Empty;

		/// <summary>Gets the list of member planetoids.</summary>							  
		/// <remarks>This list contains all planetoids that belong to this family.</remarks>
		public List<PlanetoidEntry> Members { get; } = [];
	}

	/// <summary>Starts the asteroid family detection when the Start button is clicked.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called when the Start button is clicked to initiate the asteroid family detection process.</remarks>
	private void ToolStripButtonStartSearch_Click(object? sender, EventArgs e)
	{
		// If there are no planetoid data lines available, show an informational message and return.
		if (_planetoids.Count == 0)
		{
			KryptonMessageBox.Show(
				text: "No planetoid data available.",
				caption: "Information",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information);
			return;
		}
		// Disable the Start button and enable the Cancel button while detection is in progress. Also disable save and navigation buttons until results are available.
		toolStripButtonStartSearch.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripButtonSaveListSelectedFamily.Enabled = false;
		toolStripButtonSaveListAllFamilies.Enabled = false;
		toolStripButtonGoToObject.Enabled = false;
		// Clear previous results and reset progress indicators.
		treeViewFamilies.Nodes.Clear();
		listViewMembers.VirtualListSize = 0;
		_families.Clear();
		_selectedFamily = null;
		kryptonProgressBarToolStripItem.Value = 0;
		kryptonProgressBarToolStripItem.Text = "0%";
		// Read tolerance values and minimum members from the UI controls.
		double tolA = (double)toolStripNumericUpDownToleranceValueSemiMajorAxis.Value;
		double tolE = (double)toolStripNumericUpDownToleranceValueNumericEccentricity.Value;
		double tolI = (double)toolStripNumericUpDownToleranceValueInclination.Value;
		int minMembers = (int)toolStripNumericUpDownToleranceValueMinimumMembers.Value;
		// Create a new cancellation token source for this detection run.
		_cancellationTokenSource = new CancellationTokenSource();
		// Create a progress reporter to update the progress bar and label on the UI thread.
		Progress<int> progress = new(handler: percent =>
		{
			kryptonProgressBarToolStripItem.Value = percent;
			kryptonProgressBarToolStripItem.Text = $"{percent}%";
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)percent, progressMax: 100);
		});
		// Start the detection process on a background thread to keep the UI responsive.
		Task.Run(
			function: () => PerformDetectionAsync(tolA: tolA, tolE: tolE, tolI: tolI, minMembers: minMembers, progress: progress, cancellationToken: _cancellationTokenSource.Token),
			cancellationToken: _cancellationTokenSource.Token);
	}

	/// <summary>Cancels the ongoing detection when the Cancel button is clicked.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called when the Cancel button is clicked to stop the ongoing detection process.</remarks>
	private void ToolStripButtonCancel_Click(object? sender, EventArgs e)
	{
		// If a cancellation token source exists, signal cancellation to stop the detection process and disable the Cancel button. The Start button will be re-enabled by the finally block in PerformDetectionAsync once the background task has fully completed, preventing a new run from starting while the previous one is still unwinding.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Cancels any active detection and releases resources when the form is closing.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called when the form is closing to cancel any active detection and release resources.</remarks>
	private void AsteroidFamiliesForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		// If a cancellation token source exists, signal cancellation and dispose of it to release resources.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}
	}

	/// <summary>Performs the asteroid family detection asynchronously using an orbital element binning algorithm.</summary>
	/// <param name="tolA">Tolerance for semi-major axis binning (AU).</param>
	/// <param name="tolE">Tolerance for eccentricity binning.</param>
	/// <param name="tolI">Tolerance for inclination binning (degrees).</param>
	/// <param name="minMembers">Minimum number of members required to qualify as a family.</param>
	/// <param name="progress">Progress reporter (0–100).</param>
	/// <param name="cancellationToken">Token to cancel the operation.</param>
	/// <remarks>This method performs the asteroid family detection asynchronously using an orbital element binning algorithm.</remarks>
	private async Task PerformDetectionAsync(
		double tolA,
		double tolE,
		double tolI,
		int minMembers,
		IProgress<int> progress,
		CancellationToken cancellationToken)
	{
		// The detection process is divided into three phases:
		try
		{
			// Phase 1: Parse planetoid orbital elements (progress 0–40 %)
			// We read the raw data lines and extract the relevant orbital parameters (a, e, i, M, ω, Ω) for each planetoid.
			List<PlanetoidEntry> parsedData = new(capacity: _planetoids.Count);
			int total = _planetoids.Count;
			// We loop through each line of the planetoid data, parsing the necessary fields and creating PlanetoidEntry objects. Progress is reported every 5000 entries to keep the user informed.
			for (int i = 0; i < total; i++)
			{
				// Check for cancellation at regular intervals to allow the user to stop the process if needed.
				cancellationToken.ThrowIfCancellationRequested();
				// Each line in the MPCORB database has a fixed-width format. We extract the necessary fields based on their positions.
				string line = _planetoids[index: i];
				// We check if the line has enough length to contain the required fields before attempting to parse them.
				if (line.Length >= 103)
				{
					// The index is typically the first 7 characters, and the name (if available) is at a specific position. We trim the fields to remove extra spaces.
					string index = line[..7].Trim();
					string name = line.Length >= 194 ? line.Substring(startIndex: 166, length: 28).Trim() : string.Empty;
					// We attempt to parse the orbital elements using invariant culture to ensure consistent number formatting. If parsing succeeds, we create a PlanetoidEntry and add it to the list.
					if (double.TryParse(s: line.Substring(startIndex: 92, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double a) &&
						double.TryParse(s: line.Substring(startIndex: 70, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double ecc) &&
						double.TryParse(s: line.Substring(startIndex: 59, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double incl) &&
						double.TryParse(s: line.Substring(startIndex: 26, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double meanAnomaly) &&
						double.TryParse(s: line.Substring(startIndex: 37, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argPeri) &&
						double.TryParse(s: line.Substring(startIndex: 48, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longAscNode))
					{
						// If all parsing operations succeed, we create a new PlanetoidEntry with the extracted values and add it to the parsedData list.
						parsedData.Add(item: new PlanetoidEntry(Index: index, Name: name, SemiMajorAxis: a, Eccentricity: ecc, Inclination: incl, MeanAnomaly: meanAnomaly, ArgPeri: argPeri, LongAscNode: longAscNode));
					}
				}
				// We report progress every 5000 entries to keep the user informed about the ongoing parsing process. The progress is calculated as a percentage of the total number of entries.
				if (i % 5000 == 0)
				{
					progress.Report(value: i * 40 / total);
				}
			}
			// After parsing is complete, we check for cancellation one last time before moving to the next phase and report that we have reached 40% progress.
			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(value: 40);
			// Phase 2: Group planetoids into bins by (a, e, i) (progress 40–90 %)
			// We create a 3D grid of bins based on the specified tolerances for a, e, and i. Each planetoid is assigned to a bin based on its orbital elements.
			Dictionary<(int binA, int binE, int binI), List<PlanetoidEntry>> bins = [];
			int count = parsedData.Count;
			// We loop through the parsed planetoid entries and calculate the bin indices for each orbital element by dividing the element value by the corresponding tolerance and flooring the result. We then group the planetoids into bins using a dictionary, where the key is a tuple of bin indices and the value is a list of planetoids in that bin.
			for (int i = 0; i < count; i++)
			{
				// Check for cancellation at regular intervals to allow the user to stop the process if needed.
				cancellationToken.ThrowIfCancellationRequested();
				// We calculate the bin indices for the current planetoid based on its semi-major axis, eccentricity, and inclination. The indices are calculated by dividing the element values by their respective tolerances and flooring the results to get integer bin indices.
				PlanetoidEntry p = parsedData[index: i];
				(int binA, int binE, int binI) key = (
					binA: (int)Math.Floor(d: p.SemiMajorAxis / tolA),
					binE: (int)Math.Floor(d: p.Eccentricity / tolE),
					binI: (int)Math.Floor(d: p.Inclination / tolI)
				);
				// We attempt to get the list of planetoids for the calculated bin key from the dictionary. If the key does not exist, we create a new list and add it to the dictionary. We then add the current planetoid to the list for that bin.
				if (!bins.TryGetValue(key, value: out List<PlanetoidEntry>? list))
				{
					list = [];
					bins[key] = list;
				}
				// We add the current planetoid to the list of planetoids for the corresponding bin.
				list.Add(item: p);
				// We report progress every 5000 entries to keep the user informed about the ongoing binning process. The progress is calculated as a percentage of the total number of entries, starting from 40% and going up to 90%.
				if (i % 5000 == 0)
				{
					progress.Report(value: 40 + (int)((long)i * 50 / count));
				}
			}
			// After binning is complete, we check for cancellation one last time before moving to the next phase and report that we have reached 90% progress.
			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(value: 90);
			// Phase 3: Filter and build family list (progress 90–100 %)
			// We filter the bins to find those that contain at least the specified minimum number of members. For each qualifying bin, we create an AsteroidFamily object with a name indicating its rank and member count, and we sort the members by their semi-major axis for better presentation.
			List<AsteroidFamily> families = [.. bins.Values
				.Where(predicate: g => g.Count >= minMembers)
				.OrderByDescending(keySelector: g => g.Count)
				.Select(selector: (g, idx) =>
				{ ArgumentNullException.ThrowIfNull(argument: g);
					// For each qualifying bin, we create a new AsteroidFamily object. The name of the family is constructed to indicate its rank (based on member count) and the number of members it contains. We then add the members of the family to the Members list, sorting them by their semi-major axis for better presentation.
					AsteroidFamily family = new() {
						Name = $"Family {idx + 1} ({g.Count} members)"
					};
					// We add the members of the family to the Members list, sorting them by their semi-major axis for better presentation.
					family.Members.AddRange(collection: g.OrderBy(keySelector: p => p.SemiMajorAxis));
					return family;
				})];
			// After filtering and building the family list, we check for cancellation one last time before updating the UI with the results and report that we have reached 100% progress.
			cancellationToken.ThrowIfCancellationRequested();
			progress.Report(value: 100);
			// We use InvokeAsync to marshal the update back to the UI thread, where we set the _families field to the detected families, populate the tree view with the new data, and enable the Save All button if any families were detected. Save Selected and Go to object remain disabled until the user selects a family or a member.
			await InvokeAsync(callback: () =>
			{
				_families = families;
				PopulateTreeView();
				toolStripButtonSaveListAllFamilies.Enabled = _families.Count > 0;
			}, cancellationToken: cancellationToken);
		}
		// We catch OperationCanceledException to handle the case where the detection was cancelled by the user. In this case, we simply ignore the exception since cancellation is an expected outcome.
		catch (OperationCanceledException)
		{
			// Detection was cancelled by the user — no action needed.
		}
		// We catch any other exceptions that may occur during the detection process and show an error message to the user. This ensures that unexpected errors are communicated clearly without crashing the application.
		catch (Exception ex)
		{
			// An unexpected error occurred during detection. We show an error message to the user with details about the exception.
			await InvokeAsync(callback: () =>
				MessageBox.Show(
					text: $"An error occurred during family detection: {ex.Message}",
					caption: "Error",
					buttons: MessageBoxButtons.OK,
					icon: MessageBoxIcon.Error), cancellationToken: cancellationToken);
		}
		// In the finally block, we ensure that we clean up resources and reset the UI state regardless of whether the detection completed successfully, was cancelled, or encountered an error. We dispose of the cancellation token source to release resources and set it to null. We also re-enable the Start button and disable the Cancel button to allow the user to start a new detection if desired.
		finally
		{
			// We dispose of the cancellation token source to release resources and set it to null.
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			// We use InvokeAsync to marshal the update back to the UI thread, where we re-enable the Start button and disable the Cancel button to allow the user to start a new detection if desired.
			await InvokeAsync(callback: () =>
			{
				toolStripButtonStartSearch.Enabled = true;
				toolStripButtonCancel.Enabled = false;
			}, cancellationToken: cancellationToken);
		}
	}

	/// <summary>Populates the tree view with the detected asteroid families.</summary>
	/// <remarks>This method clears the existing nodes in the tree view and adds new nodes for each detected family.
	/// Each node's Tag property is set to the index of the corresponding family in the _families list.</remarks>
	private void PopulateTreeView()
	{
		// We call BeginUpdate to prevent the tree view from repainting until we have finished adding all nodes, which improves performance and prevents flickering.
		treeViewFamilies.BeginUpdate();
		treeViewFamilies.Nodes.Clear();
		// We loop through the list of detected families and create a new TreeNode for each family. The text of the node is set to the family's name, and the Tag property is set to the index of the family in the _families list. This allows us to easily retrieve the corresponding family when a node is selected.
		for (int i = 0; i < _families.Count; i++)
		{
			TreeNode node = new(text: _families[index: i].Name) { Tag = i };
			treeViewFamilies.Nodes.Add(node);
		}
		// After adding all nodes, we call EndUpdate to allow the tree view to repaint with the new nodes.
		treeViewFamilies.EndUpdate();
	}

	/// <summary>Handles TreeView node selection and populates the member ListView accordingly.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A TreeViewEventArgs that contains the event data.</param>
	/// <remarks>This method updates the member ListView based on the selected family in the TreeView.</remarks>
	private void TreeViewFamilies_AfterSelect(object? sender, TreeViewEventArgs e)
	{
		// We check if the selected node's Tag property is an integer index that corresponds to a family in the _families list. If it is, we set the _selectedFamily field to the corresponding family, update the VirtualListSize of the member ListView to match the number of members in the selected family, and enable the Save Selected button.
		if (e.Node?.Tag is int idx && idx >= 0 && idx < _families.Count)
		{
			_selectedFamily = _families[index: idx];
			// Reset sort state when a different family is selected
			_sortColumn = -1;
			_sortOrder = SortOrder.None;
			// Remove any existing sort indicators from all column headers
			for (int i = 0; i < listViewMembers.Columns.Count; i++)
			{
				string headerText = listViewMembers.Columns[index: i].Text;
				if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
				{
					listViewMembers.Columns[index: i].Text = headerText[2..];
				}
			}
			listViewMembers.VirtualListSize = _selectedFamily.Members.Count;
			listViewMembers.Invalidate();
			toolStripButtonSaveListSelectedFamily.Enabled = true;
			// No member is selected yet in the new family — disable Go to object until a member is chosen.
			toolStripButtonGoToObject.Enabled = false;
		}
	}

	/// <summary>Provides ListView items on demand for VirtualMode.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A RetrieveVirtualItemEventArgs that contains the event data.</param>
	/// <remarks>This method provides ListView items on demand for VirtualMode.</remarks>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		// We check if a family is currently selected and if the requested item index is within the bounds of the selected family's member list. If not, we return a placeholder item with a "?" text. Otherwise, we create a new ListViewItem with the planetoid's index and add subitems for the name, semi-major axis, eccentricity, inclination, mean anomaly, argument of perihelion, and longitude of ascending node, all formatted appropriately.
		if (_selectedFamily == null || e.ItemIndex >= _selectedFamily.Members.Count)
		{
			e.Item = new ListViewItem(text: "?");
			return;
		}
		// We retrieve the PlanetoidEntry for the requested item index from the selected family's member list. We then create a new ListViewItem with the planetoid's index as the main text and add subitems for the name, semi-major axis, eccentricity, inclination, mean anomaly, argument of perihelion, and longitude of ascending node. Each numerical value is formatted to four decimal places using invariant culture to ensure consistent formatting regardless of the user's locale.
		PlanetoidEntry p = _selectedFamily.Members[index: e.ItemIndex];
		ListViewItem item = new(text: p.Index);
		item.SubItems.Add(text: p.Name);
		item.SubItems.Add(text: p.SemiMajorAxis.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		item.SubItems.Add(text: p.Eccentricity.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		item.SubItems.Add(text: p.Inclination.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		item.SubItems.Add(text: p.MeanAnomaly.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		item.SubItems.Add(text: p.ArgPeri.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		item.SubItems.Add(text: p.LongAscNode.ToString(format: "F4", provider: CultureInfo.InvariantCulture));
		e.Item = item;
	}

	/// <summary>Saves the currently selected family to a text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An EventArgs that contains the event data.</param>
	/// <remarks>This method saves the currently selected family to a text file.</remarks>
	private void ToolStripButtonSaveSelected_Click(object? sender, EventArgs e)
	{
		// We check if a family is currently selected. If not, we simply return without doing anything. If a family is selected, we call the SaveFamiliesToFile method with a list containing just the selected family to save it to a text file.
		if (_selectedFamily == null)
		{
			return;
		}
		SaveFamiliesToFile(families: [_selectedFamily]);
	}

	/// <summary>Saves all detected families to a single text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An EventArgs that contains the event data.</param>
	/// <remarks>This method saves all detected families to a single text file.</remarks>
	private void ToolStripButtonSaveAll_Click(object? sender, EventArgs e)
	{
		// We check if there are any detected families in the _families list. If the list is empty, we simply return without doing anything. If there are families detected, we call the SaveFamiliesToFile method with the entire list of families to save them all to a single text file.
		if (_families.Count == 0)
		{
			return;
		}
		// We call the SaveFamiliesToFile method with the entire list of detected families to save them all to a single text file.
		SaveFamiliesToFile(families: _families);
	}

	/// <summary>Opens a save dialog and writes the specified families to a text file.</summary>
	/// <param name="families">The families to export.</param>
	/// <remarks>This method opens a save dialog and writes the specified families to a text file.</remarks>
	private static void SaveFamiliesToFile(IReadOnlyList<AsteroidFamily> families)
	{
		// We determine a default file name based on the number of families being saved. If there is only one family, we use its name (truncated at the first parenthesis if present) as the default file name, replacing spaces with underscores. If there are multiple families, we use a generic name "AsteroidFamilies" as the default file name.
		string defaultFileName = families.Count == 1
			? (families[index: 0].Name.Contains(value: '(')
				? families[index: 0].Name[..families[index: 0].Name.IndexOf(value: '(', comparisonType: StringComparison.Ordinal)].Trim().Replace(oldChar: ' ', newChar: '_')
				: families[index: 0].Name.Replace(oldChar: ' ', newChar: '_'))
			: "AsteroidFamilies";
		// We create and configure a SaveFileDialog to allow the user to choose where to save the text file. The dialog is set to filter for text files and all files, with a default extension of "txt". The default file name is set based on the number of families being saved, and the title of the dialog is set accordingly.
		using SaveFileDialog dlg = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			FileName = defaultFileName,
			Title = families.Count == 1 ? "Save Selected Family" : "Save All Families"
		};
		// We show the save file dialog and check if the user clicked OK. If the user cancels the dialog, we simply return without doing anything. If the user selects a file and clicks OK, we proceed to write the family data to the specified file.
		if (dlg.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// We use a StringBuilder to construct the contents of the text file. We define a header string that describes the columns of the data, and a separator string that consists of dashes to visually separate the header from the data. We then loop through each family and its members, appending formatted lines to the StringBuilder for each planetoid in the family.
		StringBuilder sb = new();
		string header = $"{"Index",-10} {"Name",-30} {"a (AU)",-12} {"e",-10} {"i (°)",-10} {"M (°)",-12} {"ArgPeri (°)",-14} {"LongAscNode (°)",-16}";
		string separator = new(c: '-', count: header.Length);
		// We loop through each family in the list of families to be saved. For each family, we append a header line with the family's name, followed by the column header and a separator line. We then loop through each member of the family and append a formatted line with the member's orbital parameters. After processing all members of a family, we append an empty line before moving on to the next family.
		foreach (AsteroidFamily family in families)
		{
			sb.AppendLine(handler: $"=== {family.Name} ===");
			sb.AppendLine(value: header);
			sb.AppendLine(value: separator);
			foreach (PlanetoidEntry p in family.Members)
			{
				sb.AppendLine(
					value: $"{p.Index,-10} {p.Name,-30} {p.SemiMajorAxis,-12:F4} {p.Eccentricity,-10:F4} {p.Inclination,-10:F4} {p.MeanAnomaly,-12:F4} {p.ArgPeri,-14:F4} {p.LongAscNode,-16:F4}");
			}
			// After processing all members of the current family, we append an empty line to separate it from the next family in the output.
			sb.AppendLine();
		}
		// We attempt to write the contents of the StringBuilder to the specified file using UTF-8 encoding. If the write operation is successful, we show a message box confirming that the file was saved successfully. If an IOException occurs (e.g., due to disk issues), we show an error message with details about the failure. If an UnauthorizedAccessException occurs (e.g., due to permission issues), we show an error message indicating that the user does not have permission to save the file. We also catch any other unexpected exceptions and show a generic error message with details about the exception.
		try
		{
			File.WriteAllText(path: dlg.FileName, contents: sb.ToString(), encoding: Encoding.UTF8);
			MessageBox.Show(
				text: $"Successfully saved to:{Environment.NewLine}{dlg.FileName}",
				caption: "Saved",
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information);
		}
		// We catch IOException to handle cases where the file cannot be written due to issues such as disk errors or file locks. We show an error message with details about the failure.
		catch (IOException ex)
		{
			MessageBox.Show(
				text: $"Failed to save the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
		}
		// We catch UnauthorizedAccessException to handle cases where the user does not have permission to write to the specified location. We show an error message indicating that the user does not have permission to save the file, along with details about the exception.
		catch (UnauthorizedAccessException ex)
		{
			MessageBox.Show(
				text: $"You do not have permission to save the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
		}
		// We catch any other unexpected exceptions that may occur during the file save operation and show a generic error message with details about the exception. This ensures that any unforeseen issues are communicated clearly to the user without crashing the application.
		catch (Exception ex)
		{
			MessageBox.Show(
				text: $"An unexpected error occurred while saving the file:{Environment.NewLine}{dlg.FileName}{Environment.NewLine}{Environment.NewLine}Reason: {ex.Message}",
				caption: "Save Error",
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Error);
		}
	}

	#region ColumnClick event handler

	/// <summary>Handles the ColumnClick event for the member ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clicking a column header sorts the member list by that column, alternating between ascending
	/// and descending order. The sort indicator (▲/▼) is shown in the column header text. Because the
	/// ListView operates in virtual mode, sorting is applied directly to the underlying
	/// <see cref="AsteroidFamily.Members"/> list and the control is refreshed.</remarks>
	private void ListViewMembers_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// Nothing to sort if no family is selected or the list is empty
		if (_selectedFamily == null || _selectedFamily.Members.Count == 0)
		{
			return;
		}
		// Determine the new sort order based on the clicked column
		if (e.Column == _sortColumn)
		{
			// Toggle sort order if the same column is clicked again
			_sortOrder = _sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			// New column: start with ascending order
			_sortColumn = e.Column;
			_sortOrder = SortOrder.Ascending;
		}
		// Update column headers with sort indicators
		for (int i = 0; i < listViewMembers.Columns.Count; i++)
		{
			// Remove any existing sort indicator from the header text
			string headerText = listViewMembers.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column only
			if (i == _sortColumn)
			{
				string indicator = _sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listViewMembers.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listViewMembers.Columns[index: i].Text = headerText;
			}
		}
		// Sort the members list in-place based on the selected column and sort order.
		// Column mapping:
		//   0 = Index, 1 = Name, 2 = SemiMajorAxis, 3 = Eccentricity,
		//   4 = Inclination, 5 = MeanAnomaly, 6 = ArgPeri, 7 = LongAscNode
		_selectedFamily.Members.Sort(comparison: (x, y) =>
		{
			int result = _sortColumn switch
			{
				0 => CompareAlphanumeric(x.Index, y.Index),
				1 => string.Compare(strA: x.Name, strB: y.Name, comparisonType: StringComparison.OrdinalIgnoreCase),
				2 => x.SemiMajorAxis.CompareTo(value: y.SemiMajorAxis),
				3 => x.Eccentricity.CompareTo(value: y.Eccentricity),
				4 => x.Inclination.CompareTo(value: y.Inclination),
				5 => x.MeanAnomaly.CompareTo(value: y.MeanAnomaly),
				6 => x.ArgPeri.CompareTo(value: y.ArgPeri),
				7 => x.LongAscNode.CompareTo(value: y.LongAscNode),
				_ => 0
			};
			return _sortOrder == SortOrder.Descending ? -result : result;
		});
		// Refresh the virtual list view after sorting
		listViewMembers.SelectedIndices.Clear();
		listViewMembers.VirtualListSize = _selectedFamily.Members.Count;
		listViewMembers.Invalidate();
	}

	/// <summary>Compares two strings alphanumerically: numerically when both parse as integers, otherwise as strings.</summary>
	/// <param name="a">The first string to compare.</param>
	/// <param name="b">The second string to compare.</param>
	/// <returns>A signed integer that indicates the relative order of <paramref name="a"/> and <paramref name="b"/>.</returns>
	private static int CompareAlphanumeric(string a, string b)
	{
		bool hasNumericA = int.TryParse(s: a, result: out int numA);
		bool hasNumericB = int.TryParse(s: b, result: out int numB);
		return hasNumericA && hasNumericB
			? numA.CompareTo(value: numB)
			: string.Compare(strA: a, strB: b, comparisonType: StringComparison.OrdinalIgnoreCase);
	}

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the member ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item in the member list is double-clicked, the corresponding planetoid is displayed
	/// in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListViewMembers_DoubleClick(object? sender, EventArgs e) =>
		NavigateToSelectedMember(closeAfterNavigation: false);

	#endregion

	#region ItemSelectionChanged event handler

	/// <summary>Handles the ItemSelectionChanged event of the member ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Enables the 'Go to object' toolbar button when at least one member is selected, and disables it when the selection is cleared.</remarks>
	private void ListViewMembers_ItemSelectionChanged(object? sender, ListViewItemSelectionChangedEventArgs e) =>
		toolStripButtonGoToObject.Enabled = listViewMembers.SelectedIndices.Count > 0;

	#endregion

	#region Go to object event handler

	/// <summary>Handles the Click event of the 'Go to object' toolbar button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/>
	/// and this form is closed.</remarks>
	private void ToolStripButtonGoToObject_Click(object? sender, EventArgs e) =>
		NavigateToSelectedMember(closeAfterNavigation: true);

	/// <summary>Navigates to the currently selected member planetoid in the <see cref="PlanetoidDbForm"/>.</summary>
	/// <param name="closeAfterNavigation">If <see langword="true"/>, this form is closed after navigation.</param>
	/// <remarks>Does nothing when no item is selected or the family list is empty.</remarks>
	private void NavigateToSelectedMember(bool closeAfterNavigation)
	{
		if (_selectedFamily == null || listViewMembers.SelectedIndices.Count == 0)
		{
			return;
		}
		int idx = listViewMembers.SelectedIndices[index: 0];
		if (idx < 0 || idx >= _selectedFamily.Members.Count)
		{
			return;
		}
		PlanetoidEntry member = _selectedFamily.Members[index: idx];
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			planetoidDbForm.JumpToRecord(index: member.Index, designation: member.Name);
		}
		if (closeAfterNavigation)
		{
			Close();
		}
	}

	#endregion
}