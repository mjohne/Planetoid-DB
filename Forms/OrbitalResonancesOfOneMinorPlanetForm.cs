using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form for displaying orbital resonances of a planetoid relative to the 8 solar system planets.</summary>
/// <remarks>This form computes and presents the orbital resonance of a planetoid with each planet,
/// including the resonance ratio, deviation, and whether a near-resonance is detected.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitalResonancesOfOneMinorPlanetForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>The deviation threshold in percent below which an orbital ratio is considered a near-resonance.</summary>
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
		dialog.FileName = $"Orbital-Resonances_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
		// Show the dialog and return the result
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Sets the semi-major axis of the planetoid used for computing orbital resonances.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <remarks>Call this method before showing the form so that the resonance data is available on load.</remarks>
	public void SetSemiMajorAxis(double semiMajorAxis) =>
		this.semiMajorAxis = semiMajorAxis;

	/// <summary>Populates the <see cref="listView"/> with orbital resonance data from the <see cref="allResonances"/> field, optionally filtering to only true resonances.</summary>
	/// <remarks>Each resonance is shown as one row. The "Is Resonance" column shows "Yes" when the deviation is below 1%.
	/// Rows are colored green for resonances and red for non-resonances.</remarks>
	private void PopulateListView()
	{
		listView.BeginUpdate();
		listView.Items.Clear();
		bool filterOnlyResonances = toolStripButtonFilterResonances.Checked;
		foreach (DerivedElements.OrbitalResonance resonance in allResonances)
		{
			string isResonance = resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No";

			if (filterOnlyResonances && isResonance != "Yes")
			{
				continue;
			}

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

			item.UseItemStyleForSubItems = true;
			item.ForeColor = isResonance == "Yes" ? Color.Green : isResonance == "No" ? Color.Red : Color.Black;

			listView.Items.Add(value: item);
		}
		listView.EndUpdate();
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the form Load event.
	/// Clears the status bar, computes orbital resonances for the stored semi-major axis and populates the list view.</summary>
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

	/// <summary>Implements the manual sorting of items by column.</summary>
	/// <remarks>This class is used internally by the form to provide custom sorting logic for the ListView control.</remarks>
	/// <param name="column">The column index to sort by.</param>
	/// <param name="order">The sort order (Ascending or Descending).</param>
	private class ListViewItemComparer(int column, SortOrder order) : System.Collections.IComparer
	{
		/// <summary>Column index to sort by.</summary>
		/// <remarks>This field stores the index of the column that is currently being sorted. It is used in the Compare method to determine which subitem's text to compare for sorting.</remarks>
		private readonly int col = column;

		/// <summary>Specifies the sort order used by the containing type.</summary>
		/// <remarks>This field indicates whether the sorting should be performed in ascending or descending order. It is used in the Compare method to determine how to return the comparison result.</remarks>
		private readonly SortOrder _order = order;

		/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
		public int Compare(object? x, object? y)
		{
			// Ensure both objects are ListViewItems; if not, consider them equal for sorting purposes
			if (x is not ListViewItem itemX || y is not ListViewItem itemY)
			{
				return 0;
			}
			// Retrieve the text for the specified column from both items; if the column index is out of range for an item, use an empty string for comparison
			string textX = itemX.SubItems.Count > col ? itemX.SubItems[index: col].Text : string.Empty;
			string textY = itemY.SubItems.Count > col ? itemY.SubItems[index: col].Text : string.Empty;
			// Attempt to parse the text as numbers for a more natural sorting order; if both can be parsed as numbers, compare them numerically; otherwise, compare them as strings
			bool hasNumericX = double.TryParse(s: textX, style: System.Globalization.NumberStyles.Any, provider: System.Globalization.CultureInfo.CurrentCulture, result: out double numX);
			bool hasNumericY = double.TryParse(s: textY, style: System.Globalization.NumberStyles.Any, provider: System.Globalization.CultureInfo.CurrentCulture, result: out double numY);
			// If both items have numeric values, compare them as numbers; otherwise, compare them as strings (case-insensitive)
			int result = hasNumericX && hasNumericY
				? numX.CompareTo(value: numY)
				: string.Compare(strA: textX, strB: textY, comparisonType: StringComparison.OrdinalIgnoreCase);
			// Return the comparison result, adjusting for the specified sort order (ascending or descending)
			return _order == SortOrder.Descending ? -result : result;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the filter resonances tool strip button.
	/// Toggles the list view between showing all rows and showing only resonance rows.</summary>
	/// <param name="sender">The source of the event, typically the tool strip button.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void ToolStripButtonFilterResonances_Click(object sender, EventArgs e)
	{
		PopulateListView();
	}

	/// <summary>Handles the Click event of the copy-to-clipboard menu item.
	/// Copies the text of the currently selected list view row to the clipboard.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>All sub-items of the selected row are joined with a tab character before being placed on the clipboard.
	/// If no row is selected the method returns without action.</remarks>
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
}