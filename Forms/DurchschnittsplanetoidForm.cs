// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Represents a form that displays the theoretical average planetoid calculated from all orbital elements and astrophysical values.</summary>
/// <remarks>This form calculates and displays various types of averages (arithmetic, median, mode, geometric, harmonic, quadratic, cubic, logarithmic, Winsor, quartile, shortest half, Gastwirth-Cohen, range, "a", moving, Hölder, Lehmer) for each orbital element and astrophysical property.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DurchschnittsplanetoidForm : BaseKryptonForm
{
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

	/// <summary>Stores the planetoids database.</summary>
	/// <remarks>This list contains all planetoid database entries from the MPCORB file.</remarks>
	private readonly IReadOnlyList<string> planetoidsDatabase;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="DurchschnittsplanetoidForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to process.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string. Initializes the form components and calculates the average values.</remarks>
	public DurchschnittsplanetoidForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		planetoidsDatabase = planetoids ?? throw new ArgumentNullException(nameof(planetoids));
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Calculates all average types for the planetoid database.</summary>
	/// <remarks>This method calculates all types of averages for each orbital element and populates the ListView with the results.</remarks>
	private void CalculateAverages()
	{
		try
		{
			SetStatusBar(label: labelInformation, text: "Calculating averages...");
			Cursor.Current = Cursors.WaitCursor;

			// Extract all orbital elements from the database
			List<double> meanAnomalies = [];
			List<double> argumentsOfPerihelion = [];
			List<double> longitudesOfAscendingNode = [];
			List<double> inclinations = [];
			List<double> eccentricities = [];
			List<double> meanDailyMotions = [];
			List<double> semiMajorAxes = [];
			List<double> absoluteMagnitudes = [];
			List<double> slopeParameters = [];
			List<double> numberOfOppositions = [];
			List<double> numberOfObservations = [];
			List<double> rmsResiduals = [];

			// Parse all planetoid entries
			foreach (string entry in planetoidsDatabase)
			{
				if (string.IsNullOrWhiteSpace(entry) || entry.Length < 160) continue;

				try
				{
					// Parse orbital elements and astrophysical properties
					IFormatProvider provider = CultureInfo.InvariantCulture;

					if (double.TryParse(entry.Substring(26, 9).Trim(), NumberStyles.Any, provider, out double M))
						meanAnomalies.Add(M);
					if (double.TryParse(entry.Substring(37, 9).Trim(), NumberStyles.Any, provider, out double omega))
						argumentsOfPerihelion.Add(omega);
					if (double.TryParse(entry.Substring(48, 9).Trim(), NumberStyles.Any, provider, out double Omega))
						longitudesOfAscendingNode.Add(Omega);
					if (double.TryParse(entry.Substring(59, 9).Trim(), NumberStyles.Any, provider, out double i))
						inclinations.Add(i);
					if (double.TryParse(entry.Substring(70, 9).Trim(), NumberStyles.Any, provider, out double e))
						eccentricities.Add(e);
					if (double.TryParse(entry.Substring(80, 11).Trim(), NumberStyles.Any, provider, out double n))
						meanDailyMotions.Add(n);
					if (double.TryParse(entry.Substring(92, 11).Trim(), NumberStyles.Any, provider, out double a))
						semiMajorAxes.Add(a);
					if (double.TryParse(entry.Substring(8, 5).Trim(), NumberStyles.Any, provider, out double H))
						absoluteMagnitudes.Add(H);
					if (double.TryParse(entry.Substring(14, 5).Trim(), NumberStyles.Any, provider, out double G))
						slopeParameters.Add(G);
					if (double.TryParse(entry.Substring(117, 5).Trim(), NumberStyles.Any, provider, out double nOpp))
						numberOfOppositions.Add(nOpp);
					if (double.TryParse(entry.Substring(123, 5).Trim(), NumberStyles.Any, provider, out double nObs))
						numberOfObservations.Add(nObs);
					if (double.TryParse(entry.Substring(137, 5).Trim(), NumberStyles.Any, provider, out double rms))
						rmsResiduals.Add(rms);
				}
				catch (Exception ex)
				{
					logger.Warn($"Error parsing planetoid entry: {ex.Message}");
					continue;
				}
			}

			// Populate the ListView with calculated averages
			listView.BeginUpdate();
			listView.Items.Clear();

			AddAverageRow("Mean anomaly at the epoch", meanAnomalies);
			AddAverageRow("Argument of the perihelion, J2000.0", argumentsOfPerihelion);
			AddAverageRow("Longitude of the ascending node, J2000.0", longitudesOfAscendingNode);
			AddAverageRow("Inclination to the ecliptic, J2000.0", inclinations);
			AddAverageRow("Orbital eccentricity", eccentricities);
			AddAverageRow("Mean daily motion", meanDailyMotions);
			AddAverageRow("Semi-major axis", semiMajorAxes);
			AddAverageRow("Absolute magnitude, H", absoluteMagnitudes);
			AddAverageRow("Slope parameter, G", slopeParameters);
			AddAverageRow("Number of oppositions", numberOfOppositions);
			AddAverageRow("Number of observations", numberOfObservations);
			AddAverageRow("r.m.s. residual", rmsResiduals);

			listView.EndUpdate();
			SetStatusBar(label: labelInformation, text: $"Calculated averages for {planetoidsDatabase.Count} planetoids");
		}
		catch (Exception ex)
		{
			logger.Error($"An error occurred while calculating averages: {ex}");
			KryptonMessageBox.Show(text: $"An error has occurred while calculating averages: {ex.Message}", caption: "Calculation Error", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
			SetStatusBar(label: labelInformation, text: "Error calculating averages");
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	/// <summary>Adds a row to the ListView with all calculated average types for a given property.</summary>
	/// <param name="propertyName">The name of the orbital element or astrophysical property.</param>
	/// <param name="values">The collection of values to calculate averages from.</param>
	/// <remarks>This method calculates all 16 types of averages and adds them as subitems to the ListView.</remarks>
	private void AddAverageRow(string propertyName, List<double> values)
	{
		ListViewItem item = new(propertyName);

		// Calculate and add all average types
		item.SubItems.Add(FormatValue(AverageCalculator.ArithmeticMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.Median(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.Mode(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.GeometricMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.HarmonicMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.QuadraticMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.CubicMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.LogarithmicMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.WinsorMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.QuartileMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.ShortestHalfMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.GastwirthCohenMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.RangeMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.AMean(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.MovingAverage(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.HolderMeanShortestHalf(values)));
		item.SubItems.Add(FormatValue(AverageCalculator.LehmerMean(values)));

		_ = listView.Items.Add(item);
	}

	/// <summary>Formats a numeric value for display in the ListView.</summary>
	/// <param name="value">The numeric value to format.</param>
	/// <returns>A formatted string representation of the value, or "N/A" if the value is NaN or Infinity.</returns>
	/// <remarks>This method formats values with appropriate precision for display.</remarks>
	private static string FormatValue(double value)
	{
		if (double.IsNaN(value) || double.IsInfinity(value))
			return "N/A";

		return value.ToString("F6", CultureInfo.InvariantCulture);
	}

	/// <summary>Prepares the save dialog for exporting data.</summary>
	/// <param name="dialog">The file dialog to prepare.</param>
	/// <param name="ext">The file extension.</param>
	/// <returns>True if the dialog was shown successfully; otherwise, false.</returns>
	/// <remarks>This method is used to prepare the save dialog for exporting data.</remarks>
	private static bool PrepareSaveDialog(FileDialog dialog, string ext)
	{
		dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
		dialog.FileName = $"DurchschnittsplanetoidForm_{timestamp}.{ext}";
		return dialog.ShowDialog() == DialogResult.OK;
	}

	/// <summary>Performs the save export operation by displaying a save dialog and invoking the specified export action.</summary>
	/// <param name="filter">The file type filter for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="exportAction">The export action to invoke with the list view, title, file name, and an optional virtual row provider.</param>
	/// <remarks>This method encapsulates the logic for displaying a save dialog and performing the export action based on the user's selection.</remarks>
	private void PerformSaveExport(string filter, string defaultExt, string dialogTitle, Action<ListView, string, string, Func<int, ListViewItem>?> exportAction)
	{
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle
		};

		if (!PrepareSaveDialog(saveFileDialog, ext: defaultExt))
		{
			return;
		}

		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exportAction(arg1: listView, arg2: "Durchschnittsplanetoid", arg3: saveFileDialog.FileName, arg4: null);
		}
		catch (Exception ex)
		{
			logger.Error($"An error occurred during export: {ex}");
			KryptonMessageBox.Show(text: $"An error has occurred during export: {ex.Message}", caption: "Export Error", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Calculates all averages when the form is loaded.</remarks>
	private void DurchschnittsplanetoidForm_Load(object sender, EventArgs e) => CalculateAverages();

	#endregion

	#region ListView event handlers

	/// <summary>Handles the ColumnClick event for the ListView to sort columns alphanumerically.</summary>
	/// <param name="sender">Event source (the ListView).</param>
	/// <param name="e">The <see cref="ColumnClickEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method determines the sort order and initiates the sorting process for the selected column.</remarks>
	private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
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
			string headerText = listView.Columns[i].Text;
			if (headerText.StartsWith("▲ ") || headerText.StartsWith("▼ "))
			{
				headerText = headerText[2..];
			}
			if (i == sortColumn)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[i].Text = headerText;
			}
		}

		// Apply the sort
		listView.ListViewItemSorter = new ListViewItemComparer(column: e.Column, order: sortOrder);
		listView.Sort();
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event to export the output as a CSV file.</summary>
	/// <param name="sender">The source of the event, typically the menu item for saving as CSV.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsCsv_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Comma-Separated Values (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Save as CSV", exportAction: ListViewExporter.SaveAsCsv);

	/// <summary>Handles the Click event to export the output as an HTML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item for saving as HTML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsHtml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "HTML files (*.html)|*.html|All Files (*.*)|*.*", defaultExt: "html", dialogTitle: "Save as HTML", exportAction: ListViewExporter.SaveAsHtml);

	/// <summary>Handles the Click event to export the output as an XML file.</summary>
	/// <param name="sender">The source of the event, typically the menu item for saving as XML.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsXml_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "XML files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Save as XML", exportAction: ListViewExporter.SaveAsXml);

	/// <summary>Handles the Click event to export the output as a JSON file.</summary>
	/// <param name="sender">The source of the event, typically the menu item for saving as JSON.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsJson_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "JSON files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Save as JSON", exportAction: ListViewExporter.SaveAsJson);

	/// <summary>Handles the Click event to export the output as an Excel file.</summary>
	/// <param name="sender">The source of the event, typically the menu item for saving as Excel.</param>
	/// <param name="e">The event data associated with the click event.</param>
	private void ToolStripMenuItemSaveAsExcel_Click(object? sender, EventArgs? e)
		=> PerformSaveExport(filter: "Excel Spreadsheet (*.xlsx)|*.xlsx|All Files (*.*)|*.*", defaultExt: "xlsx", dialogTitle: "Save as Excel", exportAction: ListViewExporter.SaveAsExcel);

	#endregion
}
