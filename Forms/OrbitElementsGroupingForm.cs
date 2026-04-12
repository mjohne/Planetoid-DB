// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Planetoid_DB.Forms;

/// <summary>Form to analyze and group planetoids based on common orbital element ranges.</summary>
/// <remarks>This form provides functionality to group planetoids based on their orbital elements, allowing for analysis of patterns and similarities.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitElementsGroupingForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>The list of planetoid string records to process.</summary>
	/// <remarks>This list is provided during the form's initialization and is used for grouping operations.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>Stores the source used to issue cancellation requests for asynchronous operations.</summary>
	/// <remarks>This field holds a reference to a CancellationTokenSource, which can be used to signal cancellation to one or more tasks. If null, no cancellation source is currently assigned.</remarks>
	private CancellationTokenSource? _cancellationTokenSource;

	/// <summary>Represents immutable data for a planetoid, including its identifier, name, and associated orbital elements.</summary>
	/// <param name="Index">The unique identifier or catalog index for the planetoid.</param>
	/// <param name="Name">The name of the planetoid.</param>
	/// <param name="Elements">An array of double values representing the orbital elements of the planetoid. The array must not be null.</param>
	/// <remarks>This record is used to store and manage the relevant data for each planetoid during the grouping process. The Elements array typically includes values such as mean anomaly, argument of perihelion, longitude of ascending node, inclination, orbital eccentricity, motion, and semi-major axis.</remarks>
	private record PlanetoidData(string Index, string Name, double[] Elements);

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="OrbitElementsGroupingForm"/> class.</summary>
	/// <param name="planetoids">The planetoid string records to process from the database.</param>
	/// <remarks>Initializes the form and sets up necessary data for grouping operations.</remarks>
	public OrbitElementsGroupingForm(IReadOnlyList<string> planetoids)
	{
		// Log the initialization of the form with the count of planetoids provided
		InitializeComponent();
		_planetoids = planetoids;
		logger.Info(message: "OrbitElementsGroupingForm initialized with {0} planetoids.", argument: _planetoids.Count);
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Retrieves the name of the orbital element corresponding to the specified index.</summary>
	/// <remarks>The method maps specific indices to standard orbital element names. If an index outside the range 0–6 is provided, the method returns "Unknown".</remarks>
	/// <param name="index">The zero-based index of the orbital element. Valid values are 0 through 6.</param>
	/// <returns>A string representing the name of the orbital element for the given index, or "Unknown" if the index is not recognized.</returns>
	private static string GetElementName(int index) => index switch
	{
		// Map the indices to their corresponding orbital element names. This mapping is based on the order of elements as they are parsed from the planetoid data.
		0 => "MeanAnomaly",
		1 => "ArgPeri",
		2 => "LongAscNode",
		3 => "Incl",
		4 => "OrbEcc",
		5 => "Motion",
		6 => "SemiMajorAxis",
		_ => "Unknown"
	};

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
		dialog.FileName = $"Orbit-Elements-Grouping_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the text box, title, and file name.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection. It handles the preparation of the dialog, execution of the export action, and manages the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<TextBox, string, string> exportAction)
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
			exportAction(kryptonTextBoxOutput.TextBox, "Orbit Elements Grouping", saveFileDialog.FileName);
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

	/// <summary>Generates all possible combinations of a specified length from the provided array of elements.</summary>
	/// <remarks>The order of elements within each combination matches their order in the input array. The method does not return duplicate combinations. If k is 0, a single empty combination is returned.</remarks>
	/// <param name="elements">The array of elements from which combinations are generated. Cannot be null.</param>
	/// <param name="k">The number of elements in each combination. Must be between 0 and the length of the elements array, inclusive.</param>
	/// <returns>A list of integer arrays, where each array represents a unique combination of k elements from the input array. Returns an empty list if no combinations are possible.</returns>
	private static List<int[]> GetKCombinations(int[] elements, int k)
	{
		// Validate input parameters to ensure they are within acceptable ranges. If k is less than 0 or greater than the length of the elements array, an ArgumentException is thrown.
		if (k == 0)
		{
			return [.. new[] { Array.Empty<int>() }];
		}
		// If the elements array is empty, return an empty list of combinations, as no combinations can be generated from an empty set.
		if (elements.Length == 0)
		{
			return [.. Array.Empty<int[]>()];
		}
		// If the number of elements in the input array matches k, return a single combination that includes all elements. This is a base case for the recursive generation of combinations.
		if (elements.Length == k)
		{
			return [.. new[] { elements }];
		}
		// Initialize a list to hold the resulting combinations. The method uses a recursive approach to generate combinations that include the first element of the input array, as well as combinations that do not include the first element.
		List<int[]> result = [];
		// Generate combinations that include the first element of the input array. For each combination generated from the remaining elements (with k reduced by 1), a new combination is created by adding the first element to the front of the combination. This is done using a foreach loop that iterates through the combinations generated from the recursive call.
		foreach ((int[]? c, int[]? res) in
			from c in GetKCombinations(elements: [.. elements.Skip(count: 1)], k: k - 1)
			let res = new int[k]
			select (c, res))
		{
			// For each combination generated from the recursive call, create a new combination that includes the first element of the input array. The first element is assigned to the first position of the new combination array, and the rest of the elements are copied from the combination generated by the recursive call. This new combination is then added to the result list.
			res[0] = elements[0];
			Array.Copy(sourceArray: c, sourceIndex: 0, destinationArray: res, destinationIndex: 1, length: k - 1);
			result.Add(item: res);
		}
		// Generate combinations that do not include the first element of the input array by making a recursive call to GetKCombinations with the remaining elements and the same value of k. The resulting combinations are added to the result list.
		result.AddRange(collection: GetKCombinations(elements: [.. elements.Skip(count: 1)], k: k));
		// Return the list of generated combinations. Each combination is represented as an array of integers, where the integers correspond to the indices of the orbital elements being analyzed for grouping.
		return result;
	}

	#endregion

	#region Task handlers

	/// <summary>Performs asynchronous grouping of planetoid data based on specified orbital element combinations and a tolerance threshold, reporting progress and status messages throughout the operation.</summary>
	/// <remarks>This method parses planetoid data, generates all possible combinations of the specified number of orbital elements, and groups planetoids whose elements are within the given tolerance. Progress and status messages are reported throughout the process. If the operation is canceled, a cancellation message is reported. Any errors encountered during processing are also reported via the message progress interface.</remarks>
	/// <param name="elementsCount">The number of orbital elements to use when generating combinations for grouping. Must be between 1 and the total number of available elements.</param>
	/// <param name="tolerancePercent">The tolerance, as a percentage, used to determine whether planetoid elements are considered similar for grouping purposes. Must be a non-negative value.</param>
	/// <param name="progress">An object that receives progress updates as integer percentage values representing the overall completion of the operation.</param>
	/// <param name="messageProgress">An object that receives status or informational messages about the current stage of processing.</param>
	/// <param name="cancellationToken">A token that can be used to request cancellation of the operation. If cancellation is requested, the method will terminate early.</param>
	/// <returns>A task that represents the asynchronous grouping operation.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="elementsCount"/> is less than 1, greater than the number of available orbital elements, or when <paramref name="tolerancePercent"/> is negative.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="progress"/> or <paramref name="messageProgress"/> is <see langword="null"/>.</exception>
	private async Task PerformGroupingAsync(int elementsCount, double tolerancePercent, IProgress<int> progress, IProgress<string> messageProgress, CancellationToken cancellationToken)
	{
		const int availableElementsCount = 7;

		ArgumentNullException.ThrowIfNull(argument: progress);
		ArgumentNullException.ThrowIfNull(argument: messageProgress);
		ArgumentOutOfRangeException.ThrowIfNegative(value: tolerancePercent);

		if (elementsCount < 1 || elementsCount > availableElementsCount)
		{
			throw new ArgumentOutOfRangeException(
				paramName: nameof(elementsCount),
				actualValue: elementsCount,
				message: $"The value must be between 1 and {availableElementsCount}.");
		}

		try
		{
			messageProgress.Report(value: "Parsing data...");
			// Parse valid orbital parameters
			List<PlanetoidData> parsedData = [];
			object parseLock = new();
			int parsedCount = 0;
			int totalRecords = _planetoids.Count;
			// Capture the window handle on the UI thread before entering parallel processing.
			IntPtr windowHandle = Handle;
			// Use parallel processing to parse the planetoid data efficiently. Each line is processed to extract the relevant orbital elements, and valid entries are added to the parsedData list. Progress is reported every 1000 records processed.
			_planetoids.AsParallel().WithCancellation(cancellationToken: cancellationToken).ForAll(action: line =>
			{
				if (line.Length >= 103)
				{
					// Extract the index and designation from the line. The index is taken from the first 7 characters, while the designation is taken from characters 166 to 193 if the line is long enough.
					string index = line[..7].Trim();
					string designation = line.Length >= 194 ? line.Substring(startIndex: 166, length: 28).Trim() : "";
					// Attempt to parse the orbital elements from the line. If all elements are successfully parsed, a new PlanetoidData object is created and added to the parsedData list in a thread-safe manner using a lock.
					if (double.TryParse(s: line.Substring(startIndex: 26, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double meanAnomaly) &&
						double.TryParse(s: line.Substring(startIndex: 37, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double argPeri) &&
						double.TryParse(s: line.Substring(startIndex: 48, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longAscNode) &&
						double.TryParse(s: line.Substring(startIndex: 59, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double incl) &&
						double.TryParse(s: line.Substring(startIndex: 70, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double orbEcc) &&
						double.TryParse(s: line.Substring(startIndex: 80, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double motion) &&
						double.TryParse(s: line.Substring(startIndex: 92, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double semiMajorAxis))
					{
						// If all elements are successfully parsed, create a new PlanetoidData object with the index, designation, and elements, and add it to the parsedData list in a thread-safe manner using a lock.
						double[] elements = [meanAnomaly, argPeri, longAscNode, incl, orbEcc, motion, semiMajorAxis];
						lock (parseLock)
						{
							parsedData.Add(item: new PlanetoidData(Index: index, Name: designation, Elements: elements));
						}
					}
				}
				// Increment the parsed count and report progress every 1000 records processed. The progress is calculated as a percentage of the total records.
				int currentCount = Interlocked.Increment(location: ref parsedCount);
				if (currentCount % 1000 == 0)
				{
					// Report progress every 1000 records processed. The progress is calculated as a percentage of the total records, with a step of 10% for parsing.
					progress.Report(value: currentCount * 10 / totalRecords); // 10% step for parsing
					TaskbarProgress.SetValue(windowHandle: windowHandle, progressValue: (ulong)(currentCount * 10 / totalRecords), progressMax: 100);
				}
			});
			// After parsing is complete, check for cancellation before proceeding to the next steps. If cancellation has been requested, an OperationCanceledException will be thrown, which is caught in the outer try-catch block.
			cancellationToken.ThrowIfCancellationRequested();
			// Report that the extraction of element combinations and grouping is starting. This message is displayed in the output text box to inform the user about the current stage of processing.
			messageProgress.Report(value: "Extracting element combinations and grouping...");
			// Define the number of orbital elements to analyze for grouping. This value is taken from the user input and determines how many elements will be considered when generating combinations for grouping the planetoids.
			int combinationThreshold = elementsCount;
			// Define combinations
			int[] indices = [0, 1, 2, 3, 4, 5, 6];
			// Generate all possible combinations of the specified number of orbital elements to analyze and group the planetoids. The GetKCombinations method is used to generate these combinations based on the indices of the orbital elements.
			List<int[]> combinations = [.. GetKCombinations(elements: indices, k: combinationThreshold)];
			// Process each combination
			int comboIndex = 0;
			// Store the total number of combinations to analyze, which is used for progress reporting. This value is calculated based on the number of combinations generated and is used to provide feedback to the user about the progress of the grouping operation.
			int totalCombos = combinations.Count;
			// Iterate through each combination of orbital elements to analyze and group the planetoids. For each combination, the planetoids are sorted based on the first element in the combination, and then clustered using a binning mechanism with a specified tolerance. Progress is reported throughout the process, and if cancellation is requested, an OperationCanceledException will be thrown to terminate the operation.
			foreach (int[] combo in combinations)
			{
				// Check for cancellation at the start of each combination processing. If cancellation has been requested, an OperationCanceledException will be thrown, which is caught in the outer try-catch block to handle cancellation gracefully.
				cancellationToken.ThrowIfCancellationRequested();
				// Report the current combination being analyzed, including the index of the combination and the total number of combinations. This message is displayed in the output text box to inform the user about the current stage of processing.
				messageProgress.Report(value: $"Analyzing combination {comboIndex + 1}/{totalCombos}...");
				// A simplified algorithm clusters planetoids using a generic grouping mechanism (binning with tolerance) to group planetoids that have similar values for the specified elements within the given tolerance.
				List<PlanetoidData> sortedData = [.. parsedData.OrderBy(keySelector: d => d.Elements[combo[0]])];
				// Initialize a list to hold the clusters of planetoids that are found to be similar based on the current combination of elements. Each cluster is a list of PlanetoidData objects that share similar values for the specified elements within the given tolerance.
				List<List<PlanetoidData>> clusters = [];
				// Initialize a temporary list to hold the current cluster of planetoids being analyzed. As the sorted data is processed, planetoids that are found to be similar based on the current combination of elements will be added to this list. When a new cluster is started, this list will be cleared and reused for the next set of similar planetoids.
				List<PlanetoidData> currentCluster = [];
				// Iterate through the sorted planetoid data to identify clusters of planetoids that are similar based on the current combination of elements. For each planetoid, the algorithm checks if it is similar to the representative planetoid of the current cluster (the first planetoid in the cluster) by comparing their values for the specified elements within the given tolerance. If they are similar, the current planetoid is added to the current cluster; otherwise, a new cluster is started.
				for (int i = 0; i < sortedData.Count; i++)
				{
					// Check for cancellation at the start of each iteration. If cancellation has been requested, an OperationCanceledException will be thrown, which is caught in the outer try-catch block to handle cancellation gracefully.
					cancellationToken.ThrowIfCancellationRequested();
					// Get the current planetoid data being analyzed. This planetoid will be compared to the representative planetoid of the current cluster to determine if it should be added to the cluster or if a new cluster should be started.
					PlanetoidData current = sortedData[index: i];
					// If the current cluster is empty, add the current planetoid as the first member of the cluster. This planetoid will serve as the representative for the cluster, and subsequent planetoids will be compared to it to determine if they belong to the same cluster.
					if (currentCluster.Count == 0)
					{
						currentCluster.Add(item: current);
					}
					// If the current cluster is not empty, compare the current planetoid to the representative planetoid of the cluster (the first planetoid in the cluster) by checking if their values for the specified elements are within the given tolerance. If they are similar, add the current planetoid to the current cluster; otherwise, if the current cluster has more than one member, add it to the list of clusters and start a new cluster with the current planetoid as its first member.
					else
					{
						// Get the representative planetoid of the current cluster (the first planetoid in the cluster) to compare against the current planetoid. The algorithm will check if the values of the specified elements for the current planetoid are similar to those of the representative planetoid within the given tolerance.
						PlanetoidData rep = currentCluster[index: 0];
						// Check if the current planetoid is similar to the representative planetoid of the current cluster by comparing their values for the specified elements within the given tolerance. If all specified elements are similar, the current planetoid is added to the current cluster; otherwise, if the current cluster has more than one member, it is added to the list of clusters, and a new cluster is started with the current planetoid as its first member.
						bool similar = true;
						foreach (int idx in combo)
						{
							// Calculate the absolute difference between the current planetoid's element value and the representative planetoid's element value for the current index. Then, calculate the threshold for similarity based on the representative planetoid's element value and the specified tolerance percentage. If the difference exceeds the threshold, the planetoids are not considered similar, and the loop breaks to start a new cluster.
							double diff = Math.Abs(value: current.Elements[idx] - rep.Elements[idx]);
							double threshold = Math.Max(0.001, rep.Elements[idx] * tolerancePercent);
							if (diff > threshold)
							{
								similar = false;
								break;
							}
						}
						// If the current planetoid is similar to the representative planetoid of the current cluster, add it to the current cluster. Otherwise, if the current cluster has more than one member, add it to the list of clusters, and start a new cluster with the current planetoid as its first member.
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
					// Report progress every 5000 records processed. The progress is calculated as a percentage of the total combinations and the current position within the sorted data for the current combination.
					if (i % 5000 == 0)
					{
						int currentProgress = 10 + (int)(90.0 * (((double)comboIndex / totalCombos) + ((double)i / sortedData.Count / totalCombos)));
						progress.Report(value: currentProgress);
					}
				}
				// After processing all planetoids for the current combination, if the current cluster has more than one member, add it to the list of clusters. This ensures that any remaining cluster that was being built at the end of the loop is included in the results if it contains multiple planetoids.
				if (currentCluster.Count > 1)
				{
					clusters.Add(item: currentCluster);
				}
				// If any clusters were found for the current combination, build a message to report the details of the clusters, including the number of planetoids in each cluster and their representative planetoid. The message is built using a StringBuilder for efficiency, and it is reported through the message progress interface to be displayed in the output text box.
				if (clusters.Count != 0)
				{
					StringBuilder sb = new();
					sb.AppendLine(handler: $"--- Clusters for elements {string.Join(separator: ", ", values: combo.Select(GetElementName))} ---");
					// Order clusters by size and take the top 999 groups to display. This ensures that the most significant clusters are shown to the user, while very small clusters are omitted for clarity.
					IEnumerable<List<PlanetoidData>> orderedClusters = clusters.OrderByDescending(keySelector: c => c.Count).Take(count: 999); // Show top 999 groups
					foreach (List<PlanetoidData> group in orderedClusters)
					{
						// For each cluster, append a message that includes the number of planetoids in the cluster and the representative planetoid (the first planetoid in the cluster). Then, for each planetoid in the cluster, append a line with its index, name, and the values of the specified elements. This provides detailed information about each cluster and its members.
						sb.AppendLine(handler: $"Found group with {group.Count} planetoids (Representative: {group[index: 0].Index} - {group[index: 0].Name}):");
						foreach (PlanetoidData? p in group.Take(count: 999))
						{
							// Append a line for each planetoid in the cluster, showing its index, name, and the values of the specified elements. The element values are formatted to four decimal places for readability. This provides detailed information about each member of the cluster.
							sb.AppendLine(value: $"  {p.Index} '{p.Name}' {string.Join(separator: ", ", values: combo.Select(c => $"{GetElementName(index: c)}={p.Elements[c]:F4}"))}");
						}
						// Append a new line after each cluster for better readability in the output text box.
						sb.AppendLine();
					}
					// Report the details of the clusters found for the current combination through the message progress interface, which will display the information in the output text box. This allows the user to see the results of the grouping operation for each combination of elements.
					messageProgress.Report(value: sb.ToString());
				}
				// Increment the combination index to move on to the next combination of elements for analysis. This index is used for progress reporting and to keep track of which combination is currently being processed.
				comboIndex++;
			}
			// After all combinations have been processed, report that the search has been completed successfully. This message is displayed in the output text box to inform the user that the grouping operation has finished.
			progress.Report(value: 100);
			messageProgress.Report(value: "Search completed successfully.");
		}
		// Handle cancellation of the operation gracefully by catching the OperationCanceledException. When cancellation is requested, a message is reported to inform the user that the search has been canceled.
		catch (OperationCanceledException)
		{
			messageProgress.Report(value: "Search canceled by user.");
			logger.Info(message: "Search operation was canceled by the user.");
		}
		// Catch any other exceptions that may occur during processing and report the error message through the message progress interface. Additionally, log the error using the NLog logger to provide details about the exception for troubleshooting purposes.
		catch (Exception ex)
		{
			messageProgress.Report(value: $"Error during processing: {ex.Message}");
			logger.Error(message: $"Error during processing: {ex.Message}", exception: ex);
		}
		// In the finally block, ensure that the cancellation token source is disposed of to free resources, and reset the UI elements (Start and Cancel buttons) to their default states. This ensures that the form is ready for another operation if needed, and that resources are properly cleaned up regardless of how the operation completed.
		finally
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			await InvokeAsync(callback: () =>
			{
				toolStripButtonStart.Enabled = true;
				toolStripButtonCancel.Enabled = false;
			}, cancellationToken: cancellationToken);
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the FormClosing event to ensure that any ongoing operations are properly canceled and resources are released.</summary>
	/// <remarks>Checks if a cancellation token source is available, and if so, issues a cancellation request and disposes of the token source to free resources.</remarks>
	private void OrbitElementsGroupingForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		// Check if a cancellation token source is currently assigned. If it is, call the Cancel method to signal cancellation to any ongoing operations, and dispose of the token source to free resources.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Start button to initiate the planetoid grouping process.</summary>
	/// <remarks>Disables the Start button, enables the Cancel button, resets progress indicators, and starts the grouping operation asynchronously. Displays an informational message if no planetoid data is available.</remarks>
	/// <param name="sender">The source of the event, typically the Start button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private async void ButtonStart_Click(object? sender, EventArgs e)
	{
		// Check if there are any planetoid records to process. If not, show an informational message and return.
		if (_planetoids.Count == 0)
		{
			logger.Error(message: "No planetoid data available to process.");
			MessageBox.Show(text: "No planetoid data available.", caption: "Information", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			return;
		}
		// Disable the Start button to prevent multiple concurrent operations and enable the Cancel button to allow cancellation of the ongoing operation.
		toolStripButtonStart.Enabled = false;
		toolStripButtonCancel.Enabled = true;
		toolStripNumericUpDownTolerance.Enabled = false;
		toolStripNumericUpDownElementsCount.Enabled = false;
		toolStripDropDownButtonSaveList.Enabled = false;
		kryptonTextBoxOutput.Clear();
		kryptonProgressBar.Value = 0;
		kryptonProgressBar.Text = "0%";
		// Retrieve the number of elements to group by and the tolerance percentage from the respective numeric up-down controls.
		int elementsCount = (int)toolStripNumericUpDownElementsCount.Value;
		double tolerancePercent = (double)toolStripNumericUpDownTolerance.Value / 100.0;
		// Initialize a new CancellationTokenSource to manage cancellation of the asynchronous grouping operation.
		_cancellationTokenSource = new();
		// Set up progress reporting for both percentage completion and message updates. The percentage progress updates the progress bar and its text, while the message progress appends messages to the output text box.
		Progress<int> progress = new(handler: percent =>
		{
			kryptonProgressBar.Value = percent;
			kryptonProgressBar.Text = $"{percent}%";
		});
		// The message progress handler appends messages to the output text box, ensuring that each message is followed by a new line for readability.
		Progress<string> messageProgress = new(handler: message => kryptonTextBoxOutput.AppendText(text: message + Environment.NewLine));
		// Start the grouping operation asynchronously using Task.Run, passing the necessary parameters and the cancellation token. The operation will run on a background thread, allowing the UI to remain responsive.
		try
		{
			await Task.Run(function: () => PerformGroupingAsync(elementsCount: elementsCount, tolerancePercent: tolerancePercent, progress: progress, messageProgress: messageProgress, cancellationToken: _cancellationTokenSource.Token), cancellationToken: _cancellationTokenSource.Token);
		}
		// Handle cancellation of the operation gracefully by catching the OperationCanceledException. When cancellation is requested, an informational message is logged to indicate that the grouping task was canceled.
		catch (OperationCanceledException)
		{
			logger.Info(message: "Grouping task was canceled.");
		}
		// Catch any exceptions that may occur during the execution of the grouping operation and log the error message using the NLog logger. This provides details about any issues that arise during processing for troubleshooting purposes.
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during grouping: {ex}");
			MessageBox.Show(text: $"An error has occurred during grouping: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// In the finally block, ensure that the cancellation token source is disposed of to free resources, and reset the UI elements (Start and Cancel buttons) to their default states. This ensures that the form is ready for another operation if needed, and that resources are properly cleaned up regardless of how the operation completed.
		finally
		{
			// Re-enable the Start button and disable the Cancel button regardless of the outcome.
			toolStripButtonStart.Enabled = true;
			toolStripButtonCancel.Enabled = false;
			toolStripNumericUpDownTolerance.Enabled = true;
			toolStripNumericUpDownElementsCount.Enabled = true;
			toolStripDropDownButtonSaveList.Enabled = true;
		}
	}

	/// <summary>Handles the Click event of the Cancel button to request cancellation of the ongoing grouping operation.</summary>
	/// <remarks>Checks if a cancellation token source is available, and if so, issues a cancellation request and disables the Cancel button to prevent multiple cancellation attempts.</remarks>
	private void ButtonCancel_Click(object? sender, EventArgs e)
	{
		// Check if a cancellation token source is currently assigned. If it is, call the Cancel method to signal cancellation to the ongoing operation, and disable the Cancel button to prevent further cancellation attempts.
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
			toolStripButtonCancel.Enabled = false;
		}
	}

	/// <summary>Handles the Click event to export the output as a text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: TextBoxExporter.SaveAsText);

	/// <summary>Handles the Click event to export the output as a LaTeX file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsLatex_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "LaTeX Files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: TextBoxExporter.SaveAsLatex);

	/// <summary>Handles the Click event to export the output as a Markdown file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Markdown Files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: TextBoxExporter.SaveAsMarkdown);

	/// <summary>Handles the Click event to export the output as an AsciiDoc file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc Files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: TextBoxExporter.SaveAsAsciiDoc);

	/// <summary>Handles the Click event to export the output as a ReStructuredText file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ReStructuredText Files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as ReStructuredText", exportAction: TextBoxExporter.SaveAsReStructuredText);

	/// <summary>Handles the Click event to export the output as a Textile file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile Files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: TextBoxExporter.SaveAsTextile);

	/// <summary>Handles the Click event to export the output as a Word file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWord_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Word Files (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: TextBoxExporter.SaveAsWord);

	/// <summary>Handles the Click event to export the output as an OpenDocument Text file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOdt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Text Files (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as OpenDocument Text", exportAction: TextBoxExporter.SaveAsOdt);

	/// <summary>Handles the Click event to export the output as an RTF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsRtf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Rich Text Format Files (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: TextBoxExporter.SaveAsRtf);

	/// <summary>Handles the Click event to export the output as an Abiword file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword Files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: TextBoxExporter.SaveAsAbiword);

	/// <summary>Handles the Click event to export the output as a WPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS", exportAction: TextBoxExporter.SaveAsWps);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: TextBoxExporter.SaveAsExcel);

	/// <summary>Handles the Click event to export the output as an ODS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsOds_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet Files (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: TextBoxExporter.SaveAsOds);

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: TextBoxExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as a TSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsTsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: TextBoxExporter.SaveAsTsv);

	/// <summary>Handles the Click event to export the output as a PSV file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPsv_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: TextBoxExporter.SaveAsPsv);

	/// <summary>Handles the Click event to export the output as an ET file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "ET Files (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as ET", exportAction: TextBoxExporter.SaveAsEt);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "HTML Files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: TextBoxExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: TextBoxExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a DocBook file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: TextBoxExporter.SaveAsDocBook);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: TextBoxExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as a YAML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsYaml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: TextBoxExporter.SaveAsYaml);

	/// <summary>Handles the Click event to export the output as a TOML file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: TextBoxExporter.SaveAsToml);

	/// <summary>Handles the Click event to export the output as a SQL file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSql_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: TextBoxExporter.SaveAsSql);

	/// <summary>Handles the Click event to export the output as a SQLite file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Files (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: TextBoxExporter.SaveAsSqlite);

	/// <summary>Handles the Click event to export the output as a PDF file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPdf_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: TextBoxExporter.SaveAsPdf);

	/// <summary>Handles the Click event to export the output as a PostScript file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsPostScript_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "PostScript Files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: TextBoxExporter.SaveAsPostScript);

	/// <summary>Handles the Click event to export the output as an EPUB file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsEpub_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "EPUB Files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: TextBoxExporter.SaveAsEpub);

	/// <summary>Handles the Click event to export the output as a MOBI file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsMobi_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "MOBI Files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: TextBoxExporter.SaveAsMobi);

	/// <summary>Handles the Click event to export the output as an XPS file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: TextBoxExporter.SaveAsXps);

	/// <summary>Handles the Click event to export the output as a FictionBook2 file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: TextBoxExporter.SaveAsFictionBook2);

	/// <summary>Handles the Click event to export the output as a CHM file.</summary>
	/// <remarks>Invokes the PerformSaveExport method with parameters specific to exporting as a text file, including the file filter, default extension, dialog title, and export action.</remarks>
	/// <param name="sender">The source of the event, typically the menu item for saving as text.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help Files (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: TextBoxExporter.SaveAsChm);

	#endregion
}
