// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Represents the form for displaying planetoids data in table mode.</summary>
/// <remarks>This form provides a user interface for viewing and managing planetoids data in a tabular format.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class TableModeForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>List of planetoid records from the database</summary>
	/// <remarks>This field stores the list of planetoid records retrieved from the database.</remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>Cancellation token source for managing cancellation of asynchronous operations.</summary>
	/// <remarks>This field is used to cancel ongoing asynchronous operations.</remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>Cache for displaying planetoid records</summary>
	/// <remarks>This field is used to cache planetoid records for display purposes.</remarks>
	private List<PlanetoidRecord> displayCache = [];

	/// <summary>Stopwatch for performance measurement</summary>
	/// <remarks>This field stores the stopwatch for performance measurement.</remarks>
	private readonly Stopwatch stopwatch = new();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

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

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="TableModeForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public TableModeForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Enable virtual mode for the ListView
		listView.VirtualMode = true;
		// Initially disable the save to file button until data is loaded
		toolStripDropDownButtonSaveToFile.Enabled = false;
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is called to obtain a string representation of the current instance.</remarks>
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
		dialog.FileName = $"Table-Mode_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the ListView, title, and file name.</param>
	/// <remarks>This method encapsulates the common logic for displaying a save dialog and performing the export action,
	/// managing the cursor state during the operation.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string> exportAction)
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
			exportAction(listView, "Orbital resonances", saveFileDialog.FileName);
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

	/// <summary>Fills the internal planetoids database from the provided list.</summary>
	/// <param name="arrTemp">A list containing planetoid records as strings. Each entry is appended to the internal database.</param>
	/// <remarks>The method stores the elements of <paramref name="arrTemp"/> in the internal <see cref="planetoidsDatabase"/> list.
	/// The caller is responsible for providing data in the expected string format.</remarks>
	public void FillArray(List<string> arrTemp)
	{
		// Fill the internal planetoids database
		planetoidsDatabase = [.. arrTemp];
		// Update the UI controls if the form handle is created
		if (IsHandleCreated)
		{
			// Update the numeric controls
			UpdateNumericControls();
		}
	}

	/// <summary>Updates the numeric controls based on the current state of the planetoids database.</summary>
	/// <remarks>This method is called to update the numeric controls when the planetoids database changes.</remarks>
	private void UpdateNumericControls()
	{
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			// No data to update the controls
			return;
		}
		// Update the numeric controls based on the current state of the planetoids database.
		toolStripNumericUpDownMinimum.Minimum = 1;
		toolStripNumericUpDownMaximum.Minimum = 1;
		toolStripNumericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		toolStripNumericUpDownMinimum.Value = 1;
		toolStripNumericUpDownMaximum.Value = planetoidsDatabase.Count;
	}

	/// <summary>Sets the UI state based on the processing state.</summary>
	/// <param name="processing">True if processing is ongoing, false otherwise.</param>
	/// <remarks>This method updates the enabled state of the UI controls based on the processing state.</remarks>
	private void SetUiState(bool processing)
	{
		// Update the enabled state of the UI controls based on the processing state
		toolStripNumericUpDownMinimum.Enabled = !processing;
		toolStripNumericUpDownMaximum.Enabled = !processing;
		toolStripButtonList.Enabled = !processing;
		toolStripButtonCancel.Enabled = processing;
		kryptonProgressBar.Enabled = processing;
		toolStripDropDownButtonSaveToFile.Enabled = !processing;
		toolStripButtonGoToObject.Enabled = !processing;
		// Update the taskbar progress state
		if (!processing)
		{
			kryptonProgressBar.Value = 0;
		}
	}

	/// <summary>Gets the value of a specific column from a PlanetoidRecord.</summary>
	/// <param name="p">The PlanetoidRecord to retrieve the value from.</param>
	/// <param name="columnIndex">The index of the column to retrieve.</param>
	/// <returns>The value of the specified column as a string.</returns>
	/// <remarks>This method uses pattern matching to return the value of the specified column.
	/// The order MUST exactly match your column order in the ListView!</remarks>
	private static string GetValueByColumn(PlanetoidRecord p, int columnIndex) => columnIndex switch
	{
		0 => p.Index,
		1 => p.DesignationName,
		2 => p.Epoch,
		3 => p.MeanAnomaly,
		4 => p.ArgPeri,
		5 => p.LongAscNode,
		6 => p.Incl,
		7 => p.OrbEcc,
		8 => p.Motion,
		9 => p.SemiMajorAxis,
		10 => p.MagAbs,
		11 => p.SlopeParam,
		12 => p.Ref,
		13 => p.NumberOpposition,
		14 => p.NumberObservation,
		15 => p.ObsSpan,
		16 => p.RmsResidual,
		17 => p.ComputerName,
		18 => p.Flags,
		19 => p.ObservationLastDate,
		_ => string.Empty
	};

	/// <summary>Sorts the display cache based on the specified column and sort order.</summary>
	/// <param name="columnIndex">The index of the column to sort by.</param>
	/// <param name="order">The sort order (ascending or descending).</param>
	/// <remarks>This method modifies the display cache in place to reflect the new sort order.</remarks>
	private void SortDisplayCache(int columnIndex, SortOrder order)
	{
		// If no sorting is requested, log a warning and return
		if (order == SortOrder.None)
		{
			logger.Warn(message: "SortDisplayCache was called with SortOrder. None for column index {ColumnIndex}. No sorting will be performed.", argument: columnIndex);
			return;
		}
		// Determine the sort direction
		int direction = (order == SortOrder.Ascending) ? 1 : -1;
		// Perform the sort using a custom comparison
		// The comparison handles both numeric and string sorting
		// depending on the content of the column.
		// This ensures that numeric columns are sorted numerically
		// and string columns are sorted alphabetically.
		// List<T>.Sort uses QuickSort (very fast, O(n log n))
		displayCache.Sort(comparison: (x, y) =>
		{
			// Retrieve values based on the column
			string valX = GetValueByColumn(p: x, columnIndex: columnIndex);
			string valY = GetValueByColumn(p: y, columnIndex: columnIndex);
			// Sort numerically (Important for index, magnitude, etc.)
			// We use NumberStyles.Any and InvariantCulture to safely parse American number formats (with a dot).
			bool isNumX = double.TryParse(
				s: valX,
				style: NumberStyles.Any,
				provider: CultureInfo.InvariantCulture,
				result: out double numX
			);
			bool isNumY = double.TryParse(
				s: valY,
				style: NumberStyles.Any,
				provider: CultureInfo.InvariantCulture,
				result: out double numY
			);
			// If both values are numeric, perform numeric comparison
			if (isNumX && isNumY)
			{
				// Numeric comparison
				return numX.CompareTo(value: numY) * direction;
			}
			// Case-insensitive ordinal comparison
			return string.Compare(strA: valX, strB: valY, comparisonType: StringComparison.OrdinalIgnoreCase) * direction;
		});
	}

	/// <summary>Sets up the columns for the ListView control.</summary>
	/// <remarks>This method configures the columns of the ListView control to display the relevant
	/// information for each planetoid record.</remarks>
	private void SetupColumns()
	{
		// Add columns to the ListView
		listView.Columns.AddRange(values: [
			 columnHeaderIndex, columnHeaderReadableDesignation, columnHeaderEpoch,
			 columnHeaderMeanAnomaly, columnHeaderArgumentPerihelion, columnHeaderLongitudeAscendingNode,
			 columnHeaderInclination, columnHeaderOrbitalEccentricity, columnHeaderMeanDailyMotion,
			 columnHeaderSemimajorAxis, columnHeaderAbsoluteMagnitude, columnHeaderSlopeParameter,
			 columnHeaderReference, columnHeaderNumberOppositions, columnHeaderNumberObservations,
			 columnHeaderObservationSpan, columnHeaderRmsResidual, columnHeaderComputerName,
			 columnHeaderFlags, columnHeaderDateLastObservation
		]);
	}

	/// <summary>Creates a ListViewItem from a record.</summary>
	/// <param name="p">The planetoid record to convert.</param>
	/// <returns>A ListViewItem representing the planetoid record.</returns>
	/// <remarks>This method is used to create a ListViewItem from a PlanetoidRecord.</remarks>
	private static ListViewItem CreateListViewItem(PlanetoidRecord p)
	{
		// Create a new ListViewItem with the index as the main text
		ListViewItem item = new(text: p.Index)
		{
			ToolTipText = $"{p.Index}: {p.DesignationName}"
		};
		// Create an array of subitems from the planetoid record
		string[] subItems = [
			p.DesignationName, p.Epoch, p.MeanAnomaly, p.ArgPeri, p.LongAscNode,
			p.Incl, p.OrbEcc, p.Motion, p.SemiMajorAxis, p.MagAbs,
			p.SlopeParam, p.Ref, p.NumberOpposition, p.NumberObservation,
			p.ObsSpan, p.RmsResidual, p.ComputerName, p.Flags, p.ObservationLastDate
		];
		// Add the subitems to the ListViewItem
		item.SubItems.AddRange(items: subItems);
		return item;
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event.
	/// Initializes UI controls, clears the status area and sets up numeric ranges based on the loaded database.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the form is loaded.</remarks>
	private void TableModeForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
		// Disable the status bar, the list view and the cancel button
		labelInformation.Enabled = listView.Visible = toolStripButtonCancel.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Update the numeric up-down controls
		UpdateNumericControls();
	}

	/// <summary>Handles the form Closing event.
	/// Requests cancellation of any ongoing operations while the form and its controls are still valid.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the form begins closing, allowing pending asynchronous work to be cancelled
	/// before UI controls are disposed.</remarks>
	private void TableModeForm_FormClosing(object sender, FormClosingEventArgs e) =>
		// Request cancellation of any ongoing operations while the UI is still alive
		cancellationTokenSource?.Cancel();

	/// <summary>Handles the form Closed event.
	/// Cleans up resources and cancels any ongoing operations.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the form is closed.</remarks>
	private void TableModeForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		// Clearing the token if the window is closed during work
		cancellationTokenSource?.Dispose();
		listView.Dispose();
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and designation name.
	/// If no item is selected the method returns without modifying the UI.</summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the selected index of the list view changes.</remarks>
	private void ListViewTableMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		// Cast the sender to a ListView
		if (sender is not ListView listView)
		{
			return;
		}
		// Enable or disable the "Go To Object" button based on the current selection count
		toolStripButtonGoToObject.Enabled = listView.SelectedIndices.Count > 0;
		// Check if there are any selected indices
		if (listView.SelectedIndices.Count <= 0)
		{
			return;
		}
		// Get the selected index from the list view
		int selectedIndex = listView.SelectedIndices[index: 0];
		if (selectedIndex >= 0)
		{
			// Set the status bar text to the selected planetoids index and designation name
			SetStatusBar(label: labelInformation, text: $"{I18nStrings.Index}: {listView.Items[index: selectedIndex].Text} - {listView.Items[index: selectedIndex].SubItems[index: 1].Text}");
		}
	}
	#endregion

	#region RetrieveVirtualItem event handlers

	/// <summary>Handles the retrieval of virtual items for the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called to retrieve virtual items for the ListView.</remarks>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		// Ensure that the index is within the valid range.
		if (e.ItemIndex >= 0 && e.ItemIndex < displayCache.Count)
		{
			// Retrieve data from our cache
			PlanetoidRecord record = displayCache[index: e.ItemIndex];
			// Create a ListViewItem "on the fly"
			e.Item = CreateListViewItem(p: record);
		}
		else
		{
			// Fallback for error cases
			e.Item = new ListViewItem(text: "Error");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the List button.
	/// Prepares the list view, disables/enables the appropriate UI controls, enables progress reporting
	/// and starts the background worker to process planetoid records in the configured range.</summary>
	/// <param name="sender">Event source (the List button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the List button is clicked.</remarks>
	private async void ToolStripButtonList_ClickAsync(object sender, EventArgs e)
	{
		// Determine the range to process
		int minIndex = (int)toolStripNumericUpDownMinimum.Value - 1;
		int maxIndex = (int)toolStripNumericUpDownMaximum.Value;
		int count = maxIndex - minIndex;
		// Validate that Minimum is less than Maximum before proceeding
		if (count <= 0)
		{
			MessageBox.Show(text: "Minimum value must be less than Maximum value.", caption: "Invalid range", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			return;
		}
		// Start the stopwatch for performance measurement
		stopwatch.Restart();
		// IMPORTANT: In Virtual Mode, set the size to 0 while loading.
		listView.VirtualListSize = 0;
		displayCache.Clear();
		// Hide the list view
		listView.Visible = false;
		SetUiState(processing: true);
		// Initialize the cancellation token source
		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = cancellationTokenSource.Token;
		// Progress reporting setup - must be created on the UI thread so callbacks are marshaled back correctly
		IProgress<int> uiProgress = new Progress<int>(handler: processed =>
		{
			if (processed >= 0 && processed <= count)
			{
				kryptonProgressBar.Value = processed;
			}
			int taskbarPercent = count > 0 ? Math.Min(100, processed * 100 / count) : 0;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: (ulong)taskbarPercent, progressMax: 100);
		});
		// Configure the progress bar
		kryptonProgressBar.Maximum = count;
		kryptonProgressBar.Value = 0;
		// Background data processing
		try
		{
			// Data processing in the background
			// Parse the desired strings into PlanetoidRecords
			List<PlanetoidRecord> parsedData = await Task.Run(function: () =>
			{
				List<PlanetoidRecord> tempResults = new(capacity: count);
				IEnumerable<string> rangeToProcess = planetoidsDatabase.Skip(count: minIndex).Take(count: count);
				int progressCounter = 0;
				// Process each line in the specified range
				foreach (string line in rangeToProcess)
				{
					if (token.IsCancellationRequested)
					{
						break;
					}
					// Parse the line into a PlanetoidRecord and add it to the temporary results
					tempResults.Add(item: PlanetoidRecord.Parse(rawLine: line));
					progressCounter++;
					// Update progress every 500 items for performance
					// Don't flood the UI
					if (progressCounter % 500 == 0)
					{
						uiProgress.Report(value: progressCounter);
					}
				}
				return tempResults;
			}, cancellationToken: token);
			// If not cancelled, update the UI with the parsed data
			if (!token.IsCancellationRequested)
			{
				// Prepare the internal cache for the ListView
				// and store the parsed data in the display cache
				displayCache = parsedData;

				// Ensure columns are set up before the ListView becomes visible and requests items
				if (listView.Columns.Count == 0)
				{
					SetupColumns();
				}
				// Update the ListView size and make it visible
				// The ListView immediately calls "RetrieveVirtualItem" for the visible rows.
				listView.VirtualListSize = displayCache.Count;
				listView.Visible = true;
			}
		}
		// Handle cancellation and other exceptions
		catch (Exception ex)
		{
			logger.Error(message: $"An error occurred during background processing: {ex}");
			MessageBox.Show(text: $"An error has occurred: {ex.Message}", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
		}
		// Final UI updates
		finally
		{
			// Stop the stopwatch and reset the UI state
			stopwatch.Stop();
			SetUiState(processing: false);
			// Show completion or cancellation message
			if (cancellationTokenSource?.IsCancellationRequested == true)
			{
				MessageBox.Show(text: $"{listView.VirtualListSize} objects processed (cancellation).",
					caption: "cancellation", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
			}
			// Show completion message
			else
			{
				MessageBox.Show(text: $"{listView.VirtualListSize} objects processed in {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff} hh:mm:ss.fff",
					caption: I18nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			// Dispose the cancellation token source
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
		}
	}

	/// <summary>Handles the ColumnClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called when a column header is clicked.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		// If empty list, do nothing
		if (displayCache.Count == 0)
		{
			return;
		}
		// Logic:
		// If click on the same column -> reverse direction.
		// If click on new column -> sort ascending.
		if (e.Column != sortColumn)
		{
			// New column clicked, set to ascending
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		else
		{
			// Reverse the sort order
			sortOrder = (sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
		}
		// Set wait cursor and ensure it is always restored
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			// Sort the display cache
			SortDisplayCache(columnIndex: e.Column, order: sortOrder);
			// Refresh the ListView to reflect the new order
			listView.Refresh();
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
		// Update arrows in column headers
		foreach (ColumnHeader ch in listView.Columns)
		{
			// Ensure original header text is stored in Tag
			if (ch.Tag is string originalHeaderText)
			{
				ch.Text = originalHeaderText;
			}
			else
			{
				ch.Tag = ch.Text;
			}
		}
		// Add new arrow to the sorted column
		string arrow = (sortOrder == SortOrder.Ascending) ? "▲" : "▼";
		ColumnHeader sortedColumnHeader = listView.Columns[index: sortColumn];
		string sortedColumnBaseText = sortedColumnHeader.Tag as string ?? sortedColumnHeader.Text;
		sortedColumnHeader.Text = $"{arrow} {sortedColumnBaseText}";
	}

	/// <summary>Handles the Click event of the Cancel button.
	/// Requests cancellation of the background processing by setting the internal cancellation flag.</summary>
	/// <param name="sender">Event source (the Cancel button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the Cancel button is clicked.</remarks>
	private void ButtonCancel_Click(object? sender, EventArgs? e)
	{
		// Stop the stopwatch for performance measurement
		stopwatch.Stop();
		// Set the cancel flag to true to request cancellation
		cancellationTokenSource?.Cancel();
	}

	/// <summary>Handles the Click event to export the ListView as a CSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with CSV-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: static (lv, t, f) => ListViewExporter.SaveAsCsv(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an HTML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with HTML-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: static (lv, t, f) => ListViewExporter.SaveAsHtml(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an XML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with XML-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: static (lv, t, f) => ListViewExporter.SaveAsXml(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a JSON file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with JSON-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: static (lv, t, f) => ListViewExporter.SaveAsJson(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a SQL script.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with SQL-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsSql_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "SQL scripts (*.sql)|*.sql|All Files (*.*)|*.*", defaultExt: "sql", dialogTitle: "Save as SQL", exportAction: static (lv, t, f) => ListViewExporter.SaveAsSql(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a Markdown table.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Markdown-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsMarkdown_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Markdown files (*.md)|*.md|All Files (*.*)|*.*", defaultExt: "md", dialogTitle: "Save as Markdown", exportAction: static (lv, t, f) => ListViewExporter.SaveAsMarkdown(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a YAML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with YAML-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsYaml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "YAML files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Save as YAML", exportAction: static (lv, t, f) => ListViewExporter.SaveAsYaml(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a TSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with TSV-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsTsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Tab-Separated Values (*.tsv)|*.tsv|All Files (*.*)|*.*", defaultExt: "tsv", dialogTitle: "Save as TSV", exportAction: static (lv, t, f) => ListViewExporter.SaveAsTsv(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a PSV file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PSV-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsPsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Pipe-Separated Values (*.psv)|*.psv|All Files (*.*)|*.*", defaultExt: "psv", dialogTitle: "Save as PSV", exportAction: static (lv, t, f) => ListViewExporter.SaveAsPsv(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a LaTeX document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with LaTeX-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsLatex_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "LaTeX files (*.tex)|*.tex|All Files (*.*)|*.*", defaultExt: "tex", dialogTitle: "Save as LaTeX", exportAction: static (lv, t, f) => ListViewExporter.SaveAsLatex(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a PostScript file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PostScript-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsPostScript_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PostScript files (*.ps)|*.ps|All Files (*.*)|*.*", defaultExt: "ps", dialogTitle: "Save as PostScript", exportAction: static (lv, t, f) => ListViewExporter.SaveAsPostScript(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a PDF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with PDF-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsPdf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*", defaultExt: "pdf", dialogTitle: "Save as PDF", exportAction: static (lv, t, f) => ListViewExporter.SaveAsPdf(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an EPUB file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with EPUB-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsEpub_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "EPUB files (*.epub)|*.epub|All Files (*.*)|*.*", defaultExt: "epub", dialogTitle: "Save as EPUB", exportAction: static (lv, t, f) => ListViewExporter.SaveAsEpub(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a Word document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Word-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsWord_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Word documents (*.docx)|*.docx|All Files (*.*)|*.*", defaultExt: "docx", dialogTitle: "Save as Word", exportAction: static (lv, t, f) => ListViewExporter.SaveAsWord(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an Excel file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Excel-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: static (lv, t, f) => ListViewExporter.SaveAsExcel(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an ODT file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ODT-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsOdt_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Text (*.odt)|*.odt|All Files (*.*)|*.*", defaultExt: "odt", dialogTitle: "Save as ODT", exportAction: static (lv, t, f) => ListViewExporter.SaveAsOdt(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an ODS file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ODS-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsOds_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "OpenDocument Spreadsheet (*.ods)|*.ods|All Files (*.*)|*.*", defaultExt: "ods", dialogTitle: "Save as ODS", exportAction: static (lv, t, f) => ListViewExporter.SaveAsOds(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a MOBI file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with MOBI-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsMobi_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "MOBI files (*.mobi)|*.mobi|All Files (*.*)|*.*", defaultExt: "mobi", dialogTitle: "Save as MOBI", exportAction: static (lv, t, f) => ListViewExporter.SaveAsMobi(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an RTF file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with RTF-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsRtf_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*", defaultExt: "rtf", dialogTitle: "Save as RTF", exportAction: static (lv, t, f) => ListViewExporter.SaveAsRtf(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a text file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with text-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsText_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Text files (*.txt)|*.txt|All Files (*.*)|*.*", defaultExt: "txt", dialogTitle: "Save as Text", exportAction: static (lv, t, f) => ListViewExporter.SaveAsText(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an AsciiDoc file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with AsciiDoc-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsAsciiDoc_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "AsciiDoc files (*.adoc)|*.adoc|All Files (*.*)|*.*", defaultExt: "adoc", dialogTitle: "Save as AsciiDoc", exportAction: static (lv, t, f) => ListViewExporter.SaveAsAsciiDoc(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a reStructuredText file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with reStructuredText-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsReStructuredText_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "reStructuredText files (*.rst)|*.rst|All Files (*.*)|*.*", defaultExt: "rst", dialogTitle: "Save as reStructuredText", exportAction: static (lv, t, f) => ListViewExporter.SaveAsReStructuredText(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a Textile file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Textile-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsTextile_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Textile files (*.textile)|*.textile|All Files (*.*)|*.*", defaultExt: "textile", dialogTitle: "Save as Textile", exportAction: static (lv, t, f) => ListViewExporter.SaveAsTextile(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an Abiword file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with Abiword-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsAbiword_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Abiword files (*.abw)|*.abw|All Files (*.*)|*.*", defaultExt: "abw", dialogTitle: "Save as Abiword", exportAction: static (lv, t, f) => ListViewExporter.SaveAsAbiword(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a WPS Writer file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with WPS-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsWps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Writer files (*.wps)|*.wps|All Files (*.*)|*.*", defaultExt: "wps", dialogTitle: "Save as WPS Writer", exportAction: static (lv, t, f) => ListViewExporter.SaveAsWps(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a WPS Spreadsheets (ET) file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with ET-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsEt_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "WPS Spreadsheets (*.et)|*.et|All Files (*.*)|*.*", defaultExt: "et", dialogTitle: "Save as WPS Spreadsheets", exportAction: static (lv, t, f) => ListViewExporter.SaveAsEt(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a DocBook file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with DocBook-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsDocBook_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "DocBook Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as DocBook", exportAction: static (lv, t, f) => ListViewExporter.SaveAsDocBook(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a TOML file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with TOML-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsToml_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "TOML Files (*.toml)|*.toml|All Files (*.*)|*.*", defaultExt: "toml", dialogTitle: "Save as TOML", exportAction: static (lv, t, f) => ListViewExporter.SaveAsToml(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as an XPS document.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with XPS-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsXps_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "XPS Files (*.xps)|*.xps|All Files (*.*)|*.*", defaultExt: "xps", dialogTitle: "Save as XPS", exportAction: static (lv, t, f) => ListViewExporter.SaveAsXps(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a FictionBook2 file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with FictionBook2-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsFictionBook2_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "FictionBook2 Files (*.fb2)|*.fb2|All Files (*.*)|*.*", defaultExt: "fb2", dialogTitle: "Save as FictionBook2", exportAction: static (lv, t, f) => ListViewExporter.SaveAsFictionBook2(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a CHM file.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with CHM-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsChm_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "Compiled HTML Help (*.chm)|*.chm|All Files (*.*)|*.*", defaultExt: "chm", dialogTitle: "Save as CHM", exportAction: static (lv, t, f) => ListViewExporter.SaveAsChm(listView: lv, title: t, fileName: f));

	/// <summary>Handles the Click event to export the ListView as a SQLite database.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Invokes <see cref="PerformSaveExport"/> with SQLite-specific parameters.</remarks>
	private void ToolStripMenuItemSaveAsSqlite_Click(object sender, EventArgs e)
		=> PerformSaveExport(filter: "SQLite Database (*.sqlite)|*.sqlite|All Files (*.*)|*.*", defaultExt: "sqlite", dialogTitle: "Save as SQLite", exportAction: static (lv, t, f) => ListViewExporter.SaveAsSqlite(listView: lv, title: t, fileName: f));

	#endregion

	#region DoubleClick event handler

	/// <summary>Handles the DoubleClick event of the ListView.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When an item in the list is double-clicked, the corresponding planetoid is displayed
	/// in the <see cref="PlanetoidDbForm"/> without closing this form.</remarks>
	private void ListView_DoubleClick(object? sender, EventArgs e) =>
		GoToObject(closeAfterNavigation: false);

	#endregion

	#region Go to object event handler

	/// <summary>Handles the Click event of the 'Go to object' toolbar button.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>When clicked, the corresponding planetoid is displayed in the <see cref="PlanetoidDbForm"/>
	/// and this form is closed.</remarks>
	private void ToolStripButtonGoToObject_Click(object? sender, EventArgs e) =>
		GoToObject(closeAfterNavigation: true);

	/// <summary>Navigates to the currently selected planetoid in the <see cref="PlanetoidDbForm"/>.</summary>
	/// <param name="closeAfterNavigation">If <see langword="true"/>, this form is closed after navigation.</param>
	/// <remarks>Does nothing when no item is selected or the display cache is empty.</remarks>
	private void GoToObject(bool closeAfterNavigation)
	{
		// If no item is selected or the display cache is empty, do nothing
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
		// Get the index of the selected item and validate it against the display cache; if valid, retrieve the corresponding planetoid record and navigate to it in the PlanetoidDbForm; optionally close this form after navigation
		int idx = listView.SelectedIndices[index: 0];
		if (idx < 0 || idx >= displayCache.Count)
		{
			return;
		}
		PlanetoidRecord record = displayCache[index: idx];
		if (Owner is PlanetoidDbForm planetoidDbForm)
		{
			planetoidDbForm.JumpToRecord(index: record.Index, designation: record.DesignationName);
		}
		else if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is { } mainForm)
		{
			mainForm.JumpToRecord(index: record.Index, designation: record.DesignationName);
		}
		if (closeAfterNavigation)
		{
			Close();
		}
	}

	#endregion
}