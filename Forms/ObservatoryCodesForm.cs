// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents a form that displays the list of MPC observatory codes and their corresponding locations.</summary>
/// <remarks>This form provides a two-column ListView with observatory codes and location names. All data is built-in and does not require an internet connection.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ObservatoryCodesForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of observatory codes";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "ObservatoryCodes";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Stores the index of the currently sorted column in the ListView.</summary>
	/// <remarks>A value of <c>-1</c> means no column is currently sorted.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the sort order for the currently sorted column in the ListView.</summary>
	/// <remarks>This field is updated when the user clicks a column header to toggle the sort order.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="ObservatoryCodesForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public ObservatoryCodesForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Populates the ListView with the observatory code data from the embedded resource.</summary>
	/// <remarks>This method reads the <c>ObservatoryCodes</c> resource line by line, splits each entry by the pipe character, and populates the ListView with the code and location. It uses BeginUpdate/EndUpdate for performance when adding many items.</remarks>
	private void LoadObservatoryCodesList()
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		// Populate the ListView
		try
		{
			// Set the cursor to wait while loading data
			Cursor.Current = Cursors.WaitCursor;
			// Use BeginUpdate/EndUpdate to improve performance when adding multiple items to the ListView
			listView.BeginUpdate();
			listView.Items.Clear();
			// Add each entry from the ObservatoryCodes resource to the ListView
			string resourceText = Properties.Resources.ObservatoryCodes ?? string.Empty;
			// Split the resource text into lines and process each line
			foreach (string resourceLine in resourceText.Split(separator: '\n'))
			{
				// Trim any trailing carriage return characters and skip empty lines
				string trimmedLine = resourceLine.TrimEnd(trimChars: ['\r']);
				// Skip empty lines
				if (string.IsNullOrEmpty(value: trimmedLine))
				{
					continue;
				}
				// Split the line into parts using the pipe character as a separator
				string[] parts = trimmedLine.Split(separator: '|');
				// If there are at least two parts (code and location), create a ListViewItem and add it to the ListView
				if (parts.Length >= 2)
				{
					ListViewItem item = new(text: parts[0]);
					_ = item.SubItems.Add(text: parts[1]);
					_ = listView.Items.Add(value: item);
				}
			}
			// Update status bar with count
			SetStatusBar(label: labelInformation, text: $"{listView.Items.Count} observatory codes loaded");
		}
		// Log the successful loading of observatory codes
		catch (Exception ex)
		{
			// Log the error and show a message box to the user
			logger.Error(message: $"An error occurred while loading observatory codes: {ex}");
			KryptonMessageBox.Show(owner: this, text: $"An error has occurred while loading observatory codes: {ex.Message}", caption: "Load Error", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
			// Update the status bar to show a persistent error message
			SetStatusBar(label: labelInformation, text: "Error loading observatory codes");
		}
		// Ensure that the cursor is reset and the ListView is updated even if an error occurs
		finally
		{
			// End the update of the ListView and reset the cursor
			listView.EndUpdate();
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar and loads the observatory codes into the ListView.</remarks>
	private void ObservatoryCodesForm_Load(object sender, EventArgs e) =>
		// Clear the status bar and load the observatory codes into the ListView
		LoadObservatoryCodesList();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no items in the ListView, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			return;
		}
		// Determine the new sort order
		if (e.Column == sortColumn)
		{
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update the column headers to indicate the current sort column and order
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text;
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the ToolStripButton that displays information about observatory codes.</summary>
	/// <remarks>Displays a message box with information about observatory codes and a link to the Minor Planet Center website.</remarks>
	/// <param name="sender">The source of the event, typically the ToolStripButton that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripButtonInfoAboutObsCodes_Click(object sender, EventArgs e)
	{
		// Show a message box with information about observatory codes and a link to the Minor Planet Center website
		KryptonMessageBox.Show(owner: this, text: "This application displays a list of observatory codes and their corresponding locations.\n\nYou can find more information about Observatory Codes at the Minor Planet Center website: https://minorplanetcenter.net/iau/info/ObservatoryCodes.html.", caption: "About Observatory Codes", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	#endregion
}