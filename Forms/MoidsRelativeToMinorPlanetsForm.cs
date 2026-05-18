// This file contains the implementation of the MoidsRelativeToMinorPlanetsForm,
// which calculates and displays the MOID between two user-selected minor planets.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for calculating the Maximum Orbit Intersection Distance (MOID) between two minor planets selected by the user from the loaded MPCORB database.</summary>
/// <remarks>The form presents two combo boxes populated with all planetoid designations from the loaded database. A random-selection button next to each combo box picks a random entry. The MOID between the two selected planetoids is computed using the same double-grid search algorithm as <see cref="MoidCalculator"/> and displayed in a label below the combo boxes. The calculation is triggered automatically whenever the selection in either combo box changes.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class MoidsRelativeToMinorPlanetsForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Shared random number generator for random planetoid selection.</summary>
	/// <remarks>Shared instance to avoid repeatedly seeding a new generator.</remarks>
	private static readonly Random random = new();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>The read-only list of raw MPCORB database records.</summary>
	/// <remarks>Each element is one raw MPCORB-format string. Passed in by the caller via the constructor.</remarks>
	private readonly IReadOnlyList<string> _planetoids;

	/// <summary>All planetoid designation strings extracted from <see cref="_planetoids"/>.</summary>
	/// <remarks>Populated once during <see cref="MoidsRelativeToMinorPlanetsForm_Load"/> and reused for filtering and random selection.</remarks>
	private string[] _allNames = [];

	/// <summary>Guard flag to prevent re-entrant updates in the <c>TextChanged</c> handlers.</summary>
	/// <remarks>Set to <see langword="true"/> while a programmatic combo-box update is in progress so that the <c>TextChanged</c> event does not trigger another recursive update cycle.</remarks>
	private bool _updatingComboBox;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MoidsRelativeToMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to populate the combo boxes.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public MoidsRelativeToMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		InitializeComponent();
		// Cache the planetoid records for use in the form
		_planetoids = planetoids;
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Extracts the readable designation string from a single MPCORB record line.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <returns>The readable designation (positions 166–193), or the packed index (positions 0–6) as a fallback when the readable field is absent or empty. Returns <see langword="null"/> when the line is too short to contain even the packed index.</returns>
	/// <remarks>The method first checks if the line is long enough to contain the packed index. It then tries to extract the full readable designation from positions 166–193. If that field is empty, it falls back to the packed index in positions 0–6. If both fields are empty or the line is too short, it returns <see langword="null"/>.</remarks>
	private static string? ExtractDesignation(string line)
	{
		// Lines must be at least 7 characters to hold the packed index
		if (line.Length < 7)
		{
			return null;
		}
		// Prefer the full readable designation (positions 166-193)
		string designation = line.Length >= 194
			? line.Substring(startIndex: 166, length: 28).Trim()
			: line[..7].Trim();
		// Fall back to the packed index when the readable field is empty
		if (string.IsNullOrEmpty(value: designation))
		{
			designation = line[..7].Trim();
		}
		return string.IsNullOrEmpty(value: designation) ? null : designation;
	}

	/// <summary>Attempts to parse the five Keplerian orbital elements needed for a MOID computation from a raw MPCORB record line.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="semiMajorAxis">When successful, receives the semi-major axis in AU.</param>
	/// <param name="eccentricity">When successful, receives the orbital eccentricity.</param>
	/// <param name="inclinationDeg">When successful, receives the inclination in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">When successful, receives the longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">When successful, receives the argument of perihelion in degrees.</param>
	/// <returns><see langword="true"/> if all five elements were parsed successfully; <see langword="false"/> otherwise.</returns>
	/// <remarks>The method first checks if the line is long enough to contain all required fields. It then attempts to parse each field individually, returning <see langword="false"/> if any parsing operation fails.</remarks>
	private static bool TryParseOrbitalElements(
		string line,
		out double semiMajorAxis,
		out double eccentricity,
		out double inclinationDeg,
		out double longitudeAscendingNodeDeg,
		out double argumentPerihelionDeg)
	{
		semiMajorAxis = 0;
		eccentricity = 0;
		inclinationDeg = 0;
		longitudeAscendingNodeDeg = 0;
		argumentPerihelionDeg = 0;
		// Lines shorter than 103 characters do not contain all required fields
		if (line.Length < 103)
		{
			return false;
		}
		// Semi-major axis: positions 92-102
		if (!double.TryParse(s: line.Substring(startIndex: 92, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out semiMajorAxis) || semiMajorAxis <= 0)
		{
			return false;
		}
		// Eccentricity: positions 70-78
		if (!double.TryParse(s: line.Substring(startIndex: 70, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out eccentricity))
		{
			return false;
		}
		// Inclination: positions 59-67
		if (!double.TryParse(s: line.Substring(startIndex: 59, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out inclinationDeg))
		{
			return false;
		}
		// Longitude of ascending node: positions 48-56
		if (!double.TryParse(s: line.Substring(startIndex: 48, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out longitudeAscendingNodeDeg))
		{
			return false;
		}
		// Argument of perihelion: positions 37-45
		return double.TryParse(s: line.Substring(startIndex: 37, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out argumentPerihelionDeg);
	}

	/// <summary>Finds the raw MPCORB record line whose designation matches the given name.</summary>
	/// <param name="name">The designation string to search for (case-insensitive).</param>
	/// <returns>The first matching raw record line, or <see langword="null"/> when no match is found.</returns>
	/// <remarks>The method iterates through all planetoid lines, extracting the designation from each line and comparing it to the given name. The comparison is case-insensitive. If a match is found, the corresponding line is returned immediately. If no match is found after checking all lines, the method returns <see langword="null"/>.</remarks>
	private string? FindLineByDesignation(string name)
	{
		foreach (string line in _planetoids)
		{
			string? designation = ExtractDesignation(line: line);
			if (designation is not null && designation.Equals(value: name, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				return line;
			}
		}
		return null;
	}

	/// <summary>Computes the MOID between the two currently selected minor planets and updates the result label.</summary>
	/// <remarks>If either combo box has no valid selection the label is reset to <c>"-"</c>. Parsing errors or calculation failures are logged and the label is set to <c>"-"</c>.</remarks>
	private void CalculateAndDisplayMoid()
	{
		string name1 = comboBoxPlanetoid1.Text;
		string name2 = comboBoxPlanetoid2.Text;
		// Both designations must be non-empty to compute a MOID
		if (string.IsNullOrWhiteSpace(value: name1) || string.IsNullOrWhiteSpace(value: name2))
		{
			kryptonLabelMoidValue.Text = "-";
			return;
		}
		// Look up the raw MPCORB lines for both planetoids
		string? line1 = FindLineByDesignation(name: name1);
		string? line2 = FindLineByDesignation(name: name2);
		if (line1 is null || line2 is null)
		{
			kryptonLabelMoidValue.Text = "-";
			return;
		}
		try
		{
			// Parse orbital elements for the first planetoid
			if (!TryParseOrbitalElements(
				line: line1,
				semiMajorAxis: out double sma1,
				eccentricity: out double e1,
				inclinationDeg: out double i1,
				longitudeAscendingNodeDeg: out double omega1,
				argumentPerihelionDeg: out double w1))
			{
				kryptonLabelMoidValue.Text = "-";
				return;
			}
			// Parse orbital elements for the second planetoid
			if (!TryParseOrbitalElements(
				line: line2,
				semiMajorAxis: out double sma2,
				eccentricity: out double e2,
				inclinationDeg: out double i2,
				longitudeAscendingNodeDeg: out double omega2,
				argumentPerihelionDeg: out double w2))
			{
				kryptonLabelMoidValue.Text = "-";
				return;
			}
			// Calculate the MOID between the two orbits
			double moid = MoidCalculator.CalculateMoidBetweenPlanetoids(
				semiMajorAxis1: sma1, eccentricity1: e1, inclinationDeg1: i1,
				longitudeAscendingNodeDeg1: omega1, argumentPerihelionDeg1: w1,
				semiMajorAxis2: sma2, eccentricity2: e2, inclinationDeg2: i2,
				longitudeAscendingNodeDeg2: omega2, argumentPerihelionDeg2: w2);
			// Display the MOID in AU formatted to 8 decimal places
			kryptonLabelMoidValue.Text = moid.ToString(format: "F8");
		}
		catch (Exception ex)
		{
			logger.Error(message: $"Error calculating MOID between '{name1}' and '{name2}': {ex}");
			kryptonLabelMoidValue.Text = "-";
		}
	}

	/// <summary>Applies a contains-based filter to the specified combo box based on the current text.</summary>
	/// <param name="comboBox">The combo box to filter.</param>
	/// <remarks>The combo box items are replaced with all names that contain the current text (case-insensitive). The cursor is repositioned at the end of the typed text after the update.</remarks>
	private void ApplyContainsFilter(ComboBox comboBox)
	{
		if (_updatingComboBox)
		{
			return;
		}
		_updatingComboBox = true;
		try
		{
			string text = comboBox.Text;
			// Rebuild the items list with names that contain the typed text
			string[] filtered = string.IsNullOrEmpty(value: text)
				? _allNames
				: [.. _allNames.Where(predicate: n => n.Contains(value: text, comparisonType: StringComparison.OrdinalIgnoreCase))];
			comboBox.Items.Clear();
			comboBox.Items.AddRange(items: filtered);
			// Restore the typed text and place the caret at the end
			comboBox.Text = text;
			comboBox.SelectionStart = text.Length;
			comboBox.SelectionLength = 0;
		}
		finally
		{
			_updatingComboBox = false;
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event. Populates both combo boxes with all planetoid designations from the database and configures autocomplete.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The method first clears the status bar. It then extracts all planetoid designations from the raw database lines and populates both combo boxes with these names. An autocomplete source is also set up to provide standard "StartsWith" suggestions based on the full list of names. Any errors during this process are logged and displayed to the user.</remarks>
	private void MoidsRelativeToMinorPlanetsForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Extract all planetoid designations from the database
			_allNames = [.. _planetoids
				.Select(selector: ExtractDesignation)
				.Where(predicate: static d => d is not null)
				.Cast<string>()];
			// Populate both combo boxes with all planetoid names
			comboBoxPlanetoid1.Items.AddRange(items: _allNames);
			comboBoxPlanetoid2.Items.AddRange(items: _allNames);
			// Set up the autocomplete source with all names (standard StartsWith suggestions)
			AutoCompleteStringCollection autoCompleteSource = [];
			autoCompleteSource.AddRange(value: _allNames);
			comboBoxPlanetoid1.AutoCompleteCustomSource = autoCompleteSource;
			comboBoxPlanetoid2.AutoCompleteCustomSource = autoCompleteSource;
		}
		catch (Exception ex)
		{
			logger.Error(message: $"Error loading planetoid names: {ex}");
			ShowErrorMessage(message: $"Error loading planetoid names: {ex.Message}");
		}
	}

	/// <summary>Handles the SelectionChangeCommitted event for either combo box. Recalculates and displays the MOID whenever the user commits a selection.</summary>
	/// <param name="sender">Event source (one of the two combo boxes).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user changes the selection in either combo box. It calls the method to calculate and display the MOID based on the current selections.</remarks>
	/// <summary>Handles the SelectionChangeCommitted event for either planetoid combo box and recalculates the MOID.</summary>
	/// <param name="sender">Event source (one of the planetoid combo boxes).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void ComboBoxPlanetoid_SelectionChangeCommitted(object sender, EventArgs e) => CalculateAndDisplayMoid();

	/// <summary>Refreshes the displayed MOID state after either combo box text changes.</summary>
	/// <remarks>This ensures that manual typing or pasting updates the displayed result consistently: a valid pair is recalculated, while any invalid text clears a previously displayed stale MOID via <see cref="CalculateAndDisplayMoid"/>.</remarks>
	private void RecalculateMoidIfBothPlanetoidsAreValid() => CalculateAndDisplayMoid();

	/// <summary>Handles the TextChanged event for the first combo box. Applies a contains-based filter to the first combo box items.</summary>
	/// <param name="sender">Event source (the first combo box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered whenever the text in the first combo box changes. It applies a filter to the items in the combo box so that only names containing the current text (case-insensitive) are shown. The cursor is repositioned at the end of the typed text after the update. If both combo boxes contain valid designations after filtering, the MOID is recalculated.</remarks>
	private void ComboBoxPlanetoid1_TextChanged(object sender, EventArgs e)
	{
		ApplyContainsFilter(comboBox: comboBoxPlanetoid1);
		RecalculateMoidIfBothPlanetoidsAreValid();
	}

	/// <summary>Handles the TextChanged event for the second combo box. Applies a contains-based filter to the second combo box items.</summary>
	/// <param name="sender">Event source (the second combo box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered whenever the text in the second combo box changes. It applies a filter to the items in the combo box so that only names containing the current text (case-insensitive) are shown. The cursor is repositioned at the end of the typed text after the update. If both combo boxes contain valid designations after filtering, the MOID is recalculated.</remarks>
	private void ComboBoxPlanetoid2_TextChanged(object sender, EventArgs e)
	{
		ApplyContainsFilter(comboBox: comboBoxPlanetoid2);
		RecalculateMoidIfBothPlanetoidsAreValid();
	}

	/// <summary>Handles the Click event of the random-selection button for the first combo box. Picks a random entry from the first combo box and triggers a MOID recalculation.</summary>
	/// <param name="sender">Event source (the first random button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user clicks the random-selection button next to the first combo box. It picks a random planetoid name from the full list of names (not the filtered list) and sets it as the text of the first combo box, which in turn triggers a MOID recalculation through the TextChanged event.</remarks>
	private void KryptonButtonRandomPlanetoid1_Click(object sender, EventArgs e)
	{
		if (_allNames.Length > 0)
		{
			// Pick a random index from the full list, not the filtered list
			int index = random.Next(maxValue: _allNames.Length);
			comboBoxPlanetoid1.Text = _allNames[index];
			CalculateAndDisplayMoid();
		}
	}

	/// <summary>Handles the Click event of the random-selection button for the second combo box. Picks a random entry from the second combo box and triggers a MOID recalculation.</summary>
	/// <param name="sender">Event source (the second random button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user clicks the random-selection button next to the second combo box. It picks a random planetoid name from the full list of names (not the filtered list) and sets it as the text of the second combo box, which in turn triggers a MOID recalculation through the TextChanged event.</remarks>
	private void KryptonButtonRandomPlanetoid2_Click(object sender, EventArgs e)
	{
		if (_allNames.Length > 0)
		{
			// Pick a random index from the full list, not the filtered list
			int index = random.Next(maxValue: _allNames.Length);
			comboBoxPlanetoid2.Text = _allNames[index];
			CalculateAndDisplayMoid();
		}
	}

	#endregion
}
