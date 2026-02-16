// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Represents the form for displaying planetoids data in table mode.
/// </summary>
/// <remarks>
/// This form provides a user interface for viewing and managing planetoids data in a tabular format.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class TableModeForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used throughout the application to log important events and errors.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// List of planetoid records from the database
	/// </summary>
	/// <remarks>
	/// This field stores the list of planetoid records retrieved from the database.
	/// </remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>
	/// Cancellation token source for managing cancellation of asynchronous operations.
	/// </summary>
	/// <remarks>
	/// This field is used to cancel ongoing asynchronous operations.
	/// </remarks>
	private CancellationTokenSource? cancellationTokenSource;

	/// <summary>
	/// Cache for displaying planetoid records
	/// </summary>
	/// <remarks>
	/// This field is used to cache planetoid records for display purposes.
	/// </remarks>
	private List<PlanetoidRecord> displayCache = [];

	/// <summary>
	/// Stopwatch for performance measurement
	/// </summary>
	/// <remarks>
	/// This field stores the stopwatch for performance measurement.
	/// </remarks>
	private readonly Stopwatch stopwatch = new();

	/// <summary>
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field stores the currently selected control for clipboard operations.
	/// </remarks>
	private Control? currentControl;

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Stores the index of the currently sorted column.
	/// </summary>
	/// <remarks>
	/// This field stores the index of the currently sorted column.
	/// </remarks>
	private int sortColumn = -1;

	/// <summary>
	/// The value indicates how items in the currently sorted column are ordered:
	/// <list type="bullet">
	/// <item><description><see cref="SortOrder.None"/>: No sorting is applied.</description></item>
	/// <item><description><see cref="SortOrder.Ascending"/>: Items are sorted in ascending order.</description></item>
	/// <item><description><see cref="SortOrder.Descending"/>: Items are sorted in descending order.</description></item>
	/// </list>
	/// This field is typically updated when the user clicks a column header in the list view to toggle the sort order.
	/// </summary>
	private SortOrder sortOrder = SortOrder.None;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="TableModeForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public TableModeForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Enable virtual mode for the ListView
		listView.VirtualMode = true;
		// Handle column click events for sorting
		listView.ColumnClick += ListView_ColumnClick;
	}

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is called to obtain a string representation of the current instance.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Fills the internal planetoids database from the provided list.
	/// </summary>
	/// <param name="arrTemp">A list containing planetoid records as strings. Each entry is appended to the internal database.</param>
	/// <remarks>
	/// The method stores the elements of <paramref name="arrTemp"/> in the internal <see cref="planetoidsDatabase"/> list.
	/// The caller is responsible for providing data in the expected string format.
	/// </remarks>
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

	/// <summary>
	/// Updates the numeric controls based on the current state of the planetoids database.
	/// </summary>
	/// <remarks>
	/// This method is called to update the numeric controls when the planetoids database changes.
	/// </remarks>
	private void UpdateNumericControls()
	{
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			// No data to update the controls
			return;
		}
		// Update the numeric controls based on the current state of the planetoids database.
		numericUpDownMinimum.Minimum = 1;
		numericUpDownMaximum.Minimum = 1;
		numericUpDownMinimum.Maximum = planetoidsDatabase.Count;
		numericUpDownMaximum.Maximum = planetoidsDatabase.Count;
		numericUpDownMinimum.Value = 1;
		numericUpDownMaximum.Value = planetoidsDatabase.Count;
	}

	/// <summary>
	/// Sets the UI state based on the processing state.
	/// </summary>
	/// <param name="processing">True if processing is ongoing, false otherwise.</param>
	/// <remarks>
	/// This method updates the enabled state of the UI controls based on the processing state.
	/// </remarks>
	private void SetUiState(bool processing)
	{
		// Update the enabled state of the UI controls based on the processing state
		numericUpDownMinimum.Enabled = !processing;
		numericUpDownMaximum.Enabled = !processing;
		buttonList.Enabled = !processing;
		buttonCancel.Enabled = processing;
		progressBar.Enabled = processing;
		// Update the taskbar progress state
		if (!processing)
		{
			progressBar.Value = 0;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
		}
	}

	/// <summary>
	/// Gets the value of a specific column from a PlanetoidRecord.
	/// </summary>
	/// <param name="p">The PlanetoidRecord to retrieve the value from.</param>
	/// <param name="columnIndex">The index of the column to retrieve.</param>
	/// <returns>The value of the specified column as a string.</returns>
	/// <remarks>
	/// This method uses pattern matching to return the value of the specified column.
	/// The order MUST exactly match your column order in the ListView!
	/// </remarks>
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

	/// <summary>
	/// Sorts the display cache based on the specified column and sort order.
	/// </summary>
	/// <param name="columnIndex">The index of the column to sort by.</param>
	/// <param name="order">The sort order (ascending or descending).</param>
	/// <remarks>
	/// This method modifies the display cache in place to reflect the new sort order.
	/// </remarks>
	private void SortDisplayCache(int columnIndex, SortOrder order)
	{
		// If no sorting is requested, log a warning and return
		if (order == SortOrder.None)
		{
			logger.Warn(message: "SortDisplayCache was called with SortOrder.None for column index {ColumnIndex}. No sorting will be performed.", argument: columnIndex);
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
			// We use double.TryParse, which is robust.
			bool isNumX = double.TryParse(s: valX, result: out double numX);
			bool isNumY = double.TryParse(s: valY, result: out double numY);
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

	/// <summary>
	/// Sets up the columns for the ListView control.
	/// </summary>
	/// <remarks>
	/// This method configures the columns of the ListView control to display the relevant
	/// information for each planetoid record.
	/// </remarks>
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

	/// <summary>
	/// Creates a ListViewItem from a record.
	/// </summary>
	/// <param name="p">The planetoid record to convert.</param>
	/// <returns>A ListViewItem representing the planetoid record.</returns>
	/// <remarks>
	/// This method is used to create a ListViewItem from a PlanetoidRecord.
	/// </remarks>
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

	/// <summary>
	/// Handles the form Load event.
	/// Initializes UI controls, clears the status area and sets up numeric ranges based on the loaded database.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form is loaded.
	/// </remarks>
	private void TableModeForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
		// Disable the status bar, the list view and the cancel button
		labelInformation.Enabled = listView.Visible = buttonCancel.Enabled = false;
		// Check if the planetoids database is empty
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Update the numeric up-down controls
		UpdateNumericControls();
	}

	/// <summary>
	/// Handles the form Closing event.
	/// Requests cancellation of any ongoing operations while the form and its controls are still valid.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form begins closing, allowing pending asynchronous work to be cancelled
	/// before UI controls are disposed.
	/// </remarks>
	private void TableModeForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Request cancellation of any ongoing operations while the UI is still alive
		cancellationTokenSource?.Cancel();
	}

	/// <summary>
	/// Handles the form Closed event.
	/// Cleans up resources and cancels any ongoing operations.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the form is closed.
	/// </remarks>
	private void TableModeForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		// Clearing the token if the window is closed during work
		cancellationTokenSource?.Dispose();
		listView.Dispose();
	}

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>
	/// Handles the ListView <c>SelectedIndexChanged</c> event.
	/// Updates the status bar with the selected planetoid's index and designation name.
	/// If no item is selected the method returns without modifying the UI.
	/// </summary>
	/// <param name="sender">Event source (the list view).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the selected index of the list view changes.
	/// </remarks>
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

	/// <summary>
	/// Handles the retrieval of virtual items for the ListView.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method is called to retrieve virtual items for the ListView.
	/// </remarks>
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

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	/// This method is called when the mouse button is pressed down on a control.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the List button.
	/// Prepares the list view, disables/enables the appropriate UI controls, enables progress reporting
	/// and starts the background worker to process planetoid records in the configured range.
	/// </summary>
	/// <param name="sender">Event source (the List button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the List button is clicked.
	/// </remarks>
	private async void ButtonList_ClickAsync(object sender, EventArgs e)
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
		int minIndex = (int)numericUpDownMinimum.Value - 1;
		int maxIndex = (int)numericUpDownMaximum.Value;
		int count = maxIndex - minIndex;
		// Progress reporting setup
		Progress<int> progress = new(handler: percent =>
		{
			progressBar.Value = percent;
			int taskbarPercent = count > 0 ? percent * 100 / count : 0;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: taskbarPercent, progressMax: 100);
		});
		// Configure the progress bar
		progressBar.Maximum = count;
		progressBar.Value = 0;
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
				IProgress<int> progressReporter = new Progress<int>(handler: value => progressBar.Value = value);
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

	/// <summary>
	/// Handles the ColumnClick event of the ListView.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method is called when a column header is clicked.
	/// </remarks>
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

	/// <summary>
	/// Handles the Click event of the Cancel button.
	/// Requests cancellation of the background processing by setting the internal cancellation flag.
	/// </summary>
	/// <param name="sender">Event source (the Cancel button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the Cancel button is clicked.
	/// </remarks>
	private void ButtonCancel_Click(object? sender, EventArgs? e)
	{
		// Stop the stopwatch for performance measurement
		stopwatch.Stop();
		// Set the cancel flag to true to request cancellation
		cancellationTokenSource?.Cancel();
	}

	#endregion

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// If the <paramref name="sender"/> is a <see cref="Control"/>, its <see cref="Control.Text"/> value is copied to the clipboard.
	/// If the <paramref name="sender"/> is a <see cref="ToolStripItem"/>, its <see cref="ToolStripItem.Text"/> value is copied to the clipboard.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the text to copy based on the sender type
		string? textToCopy = sender switch
		{
			Control c => c.Text,
			ToolStripItem => currentControl?.Text,
			_ => null
		};
		// Check if the text to copy is not null or empty
		if (!string.IsNullOrEmpty(value: textToCopy))
		{
			// Assuming CopyToClipboard is a helper method in BaseKryptonForm or similar
			// If not, use Clipboard.SetText(textToCopy);
			try
			{
				CopyToClipboard(text: textToCopy);
			}
			// Log any exception that occurs during the clipboard operation
			catch (Exception ex)
			{
				logger.Error(exception: ex, message: "Failed to copy text to the clipboard.");
				throw new InvalidOperationException(message: "Failed to copy text to the clipboard.", innerException: ex);
			}
		}
	}

	#endregion
}