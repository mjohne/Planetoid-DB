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
		// Determine the range to process
		int minIndex = (int)toolStripNumericUpDownMinimum.Value - 1;
		int maxIndex = (int)toolStripNumericUpDownMaximum.Value;
		int count = maxIndex - minIndex;
		// Progress reporting setup
		Progress<int> progress = new(handler: percent =>
		{
			kryptonProgressBar.Value = percent;
			int taskbarPercent = count > 0 ? percent * 100 / count : 0;
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
				IProgress<int> progressReporter = new Progress<int>(handler: value => kryptonProgressBar.Value = value);
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
						progressReporter.Report(value: progressCounter);
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
		if (listView.SelectedIndices.Count == 0)
		{
			return;
		}
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
		if (closeAfterNavigation)
		{
			Close();
		}
	}

	#endregion
}