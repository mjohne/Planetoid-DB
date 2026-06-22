// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form for displaying orbital resonances of a planetoid relative to the 8 solar system planets.</summary>
/// <remarks>This form computes and presents the orbital resonance of a planetoid with each planet, including the resonance ratio, deviation, and whether a near-resonance is detected.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitalResonancesOfOneMinorPlanetForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Orbital resonances";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Orbital-Resonances";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The deviation threshold in percent below which an orbital ratio is considered a near-resonance.</summary>
	/// <remarks>This constant defines the percentage deviation threshold for determining whether an orbital ratio is considered a near-resonance. If the deviation of the actual orbital ratio from the ideal resonance ratio is less than this threshold, it will be classified as a resonance in the list view.</remarks>
	private const double ResonanceThresholdPercent = 1.0;

	/// <summary>The semi-major axis of the planetoid in AU, used to calculate orbital resonances.</summary>
	/// <remarks>Set this value via <see cref="SetSemiMajorAxis"/> before the form is shown.</remarks>
	private double semiMajorAxis;

	/// <summary>All computed orbital resonances.</summary>
	/// <remarks>This field holds all resonances to allow filtering the list view.</remarks>
	private List<DerivedElements.OrbitalResonance> allResonances = [];

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
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

	/// <summary>Initializes a new instance of the <see cref="OrbitalResonancesOfOneMinorPlanetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public OrbitalResonancesOfOneMinorPlanetForm() =>
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the semi-major axis of the planetoid used for computing orbital resonances.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <remarks>Call this method before showing the form so that the resonance data is available on load.</remarks>
	public void SetSemiMajorAxis(double semiMajorAxis) =>
		this.semiMajorAxis = semiMajorAxis;

	/// <summary>Populates the <see cref="listView"/> with orbital resonance data from the <see cref="allResonances"/> field, optionally filtering to only true resonances.</summary>
	/// <remarks>Each resonance is shown as one row. The "Is Resonance" column shows "Yes" when the deviation is below 1%. Rows are colored green for resonances and red for non-resonances.</remarks>
	private void PopulateListView()
	{
		// Begin updating the list view to improve performance while adding items; clear existing items before populating with new data
		listView.BeginUpdate();
		listView.Items.Clear();
		// Determine whether to filter the list to show only resonances based on the state of the filter button; iterate through all computed resonances and add them to the list view, applying filtering and coloring based on resonance status
		bool filterOnlyResonances = toolStripButtonFilterResonances.Checked;
		foreach (DerivedElements.OrbitalResonance resonance in allResonances)
		{
			// Determine if the current resonance is considered a true resonance based on the deviation percentage and the defined threshold; this will be used for filtering and coloring the list view items
			string isResonance = resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No";
			// If filtering is enabled and the current resonance is not a true resonance, skip adding this item to the list view
			if (filterOnlyResonances && isResonance != "Yes")
			{
				continue;
			}
			// Create a new ListViewItem for the current resonance, populating the main text and sub-items with the relevant data; set the text color based on whether it is a resonance or not
			ListViewItem item = new(text: resonance.PlanetName);
			item.SubItems.AddRange(items:
			[
				resonance.PlanetPeriod.ToString(format: "F6"),
				resonance.PlanetoidPeriod.ToString(format: "F6"),
				resonance.Ratio.ToString(format: "F6"),
				$"{resonance.ResonanceP}:{resonance.ResonanceQ}",
				resonance.DeviationPercent.ToString(format: "F2"),
				isResonance
			]);
			// Set the UseItemStyleForSubItems property to true to allow coloring of sub-items; set the text color based on resonance status (green for resonances, red for non-resonances, black for unknown)
			item.UseItemStyleForSubItems = true;
			item.ForeColor = isResonance == "Yes" ? Color.Green : isResonance == "No" ? Color.Red : Color.Black;
			// Add the item to the list view
			listView.Items.Add(value: item);
		}
		listView.EndUpdate();
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event. Clears the status bar, computes orbital resonances for the stored semi-major axis and populates the list view.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>If an error occurs during calculation, it is logged and an error message is shown to the user.</remarks>
	private void OrbitalResonanceForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		try
		{
			allResonances = DerivedElements.CalculateOrbitalResonances(semiMajorAxis: semiMajorAxis);
			PopulateListView();
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error calculating orbital resonances: {ex.Message}");
		}
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		// If there are no items, do not attempt to sort
		if (listView.Items.Count == 0)
		{
			return;
		}
		// Determine the new sort order based on the clicked column
		if (e.Column == sortColumn)
		{
			// Toggle sort order if the same column is clicked
			sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}
		else
		{
			// Set new sort column and default to ascending order
			sortColumn = e.Column;
			sortOrder = SortOrder.Ascending;
		}
		// Update column headers with sort indicators
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			// Remove existing sort indicators from the header text
			string headerText = listView.Columns[index: i].Text;
			// Check for existing indicators and remove them
			if (headerText.StartsWith(value: "▲ ") || headerText.StartsWith(value: "▼ "))
			{
				headerText = headerText[2..];
			}
			// Add the new sort indicator to the currently sorted column
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			// For other columns, just update the text without indicators
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
		// Apply the sort using a standard IComparer
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the filter resonances tool strip button. Toggles the list view between showing all rows and showing only resonance rows.</summary>
	/// <param name="sender">The source of the event, typically the tool strip button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>When the button is clicked, it checks the state of the button (checked or unchecked) and calls the PopulateListView method to refresh the list view based on the current filter setting. If the button is checked, only rows representing true resonances will be shown; if unchecked, all rows will be displayed.</remarks>
	private void ToolStripButtonFilterResonances_Click(object sender, EventArgs e)
	{
		PopulateListView();
	}

	/// <summary>Handles the Click event of the copy-to-clipboard menu item. Copies the text of the currently selected list view row to the clipboard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>All sub-items of the selected row are joined with a tab character before being placed on the clipboard. If no row is selected the method returns without action.</remarks>
	private void ToolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
	{
		if (listView.SelectedItems.Count == 0)
		{
			return;
		}
		ListViewItem selectedItem = listView.SelectedItems[index: 0];
		IEnumerable<string> subItemTexts = selectedItem.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(selector: static s => s.Text);
		string text = string.Join(separator: "\t", values: subItemTexts);
		CopyToClipboard(text: text);
	}

	#endregion
}