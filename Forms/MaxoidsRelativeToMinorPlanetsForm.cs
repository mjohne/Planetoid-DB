/*
 * File:        MaxoidsRelativeToMinorPlanetsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Form for calculating the Maximum Orbit Intersection Distance (MAXOID) between two minor planets selected by the user from the loaded MPCORB database.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for calculating the Maximum Orbit Intersection Distance (MAXOID) between two minor planets selected by the user from the loaded MPCORB database.</summary>
/// <remarks>The form presents two combo boxes populated with all planetoid designations from the loaded database. A random-selection button next to each combo box picks a random entry. The MAXOID between the two selected planetoids is computed using the same double-grid search algorithm as <see cref="MaxoidCalculator"/> and displayed in a label below the combo boxes. The calculation is triggered automatically whenever the selection in either combo box changes.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed partial class MaxoidsRelativeToMinorPlanetsForm : BaseKryptonForm
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
	/// <remarks>Populated once during <see cref="MaxoidsRelativeToMinorPlanetsForm_Load"/> and reused for filtering and random selection.</remarks>
	private string[] _allNames = [];

	/// <summary>Guard flag to prevent re-entrant updates in the <c>TextChanged</c> handlers.</summary>
	/// <remarks>Set to <see langword="true"/> while a programmatic combo-box update is in progress so that the <c>TextChanged</c> event does not trigger another recursive update cycle.</remarks>
	private bool _updatingComboBox;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MaxoidsRelativeToMinorPlanetsForm"/> class.</summary>
	/// <param name="planetoids">The list of all planetoid database records to populate the combo boxes.</param>
	/// <remarks>Each element in <paramref name="planetoids"/> must be a raw MPCORB-format string.</remarks>
	public MaxoidsRelativeToMinorPlanetsForm(IReadOnlyList<string> planetoids)
	{
		// Log the initialization of the form with the provided planetoid records
		logger.Info(message: "Initializing MaxoidsRelativeToMinorPlanetsForm with planetoid records.");
		// Guard against null input to ensure the form has valid data to work with
		InitializeComponent();
		// Cache the planetoid records for use in the form
		_planetoids = planetoids;
		// Log the number of planetoid records received for initialization
		logger.Info(message: $"MaxoidsRelativeToMinorPlanetsForm initialized with {_planetoids.Count} planetoid records.");
	}

	#endregion

	#region Helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Extracts the readable designation string from a single MPCORB record line.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <returns>The readable designation (positions 166–193), or the packed index (positions 0–6) as a fallback when the readable field is absent or empty. Returns <see langword="null"/> when the line is too short to contain even the packed index.</returns>
	/// <remarks>The method first checks if the line is long enough to contain the packed index. It then tries to extract the full readable designation from positions 166–193. If that field is empty, it falls back to the packed index in positions 0–6. If both fields are empty or the line is too short, it returns <see langword="null"/>.</remarks>
	private static string? ExtractDesignation(string line)
	{
		// Lines must be at least 7 characters to hold the packed index
		if (line.Length < 7)
		{
			logger.Warn(message: $"Line too short to extract designation: '{line}'");
			ShowErrorMessage(message: $"Line too short to extract designation: '{line}'");
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

	/// <summary>Attempts to parse the five Keplerian orbital elements needed for a MAXOID computation from a raw MPCORB record line.</summary>
	/// <param name="line">The raw MPCORB record string.</param>
	/// <param name="semiMajorAxis">When successful, receives the semi-major axis in AU.</param>
	/// <param name="eccentricity">When successful, receives the orbital eccentricity.</param>
	/// <param name="inclinationDeg">When successful, receives the inclination in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">When successful, receives the longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">When successful, receives the argument of perihelion in degrees.</param>
	/// <returns><see langword="true"/> if all five elements were parsed successfully; <see langword="false"/> otherwise.</returns>
	/// <remarks>The method first checks if the line is long enough to contain all required fields. It then attempts to parse each field individually, returning <see langword="false"/> if any parsing operation fails.</remarks>
	private static bool TryParseOrbitalElements(string line, out double semiMajorAxis, out double eccentricity, out double inclinationDeg, out double longitudeAscendingNodeDeg, out double argumentPerihelionDeg)
	{
		// Initialize out parameters to default values
		semiMajorAxis = 0;
		eccentricity = 0;
		inclinationDeg = 0;
		longitudeAscendingNodeDeg = 0;
		argumentPerihelionDeg = 0;
		// Lines shorter than 103 characters do not contain all required fields
		if (line.Length < 103)
		{
			// Log a warning and show an error message for lines that are too short to parse orbital elements
			logger.Warn(message: $"Line too short to parse orbital elements: '{line}'");
			ShowErrorMessage(message: $"Line too short to parse orbital elements: '{line}'");
			return false;
		}
		// Semi-major axis: positions 92-102
		if (!double.TryParse(s: line.Substring(startIndex: 92, length: 11).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out semiMajorAxis) || semiMajorAxis <= 0)
		{
			// Log a warning and show an error message for failed semi-major axis parsing
			logger.Warn(message: $"Failed to parse semi-major axis: '{line.Substring(startIndex: 92, length: 11).Trim()}'");
			ShowErrorMessage(message: $"Failed to parse semi-major axis: '{line.Substring(startIndex: 92, length: 11).Trim()}'");
			return false;
		}
		// Eccentricity: positions 70-78
		if (!double.TryParse(s: line.Substring(startIndex: 70, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out eccentricity))
		{
			// Log a warning and show an error message for failed eccentricity parsing
			logger.Warn(message: $"Failed to parse eccentricity: '{line.Substring(startIndex: 70, length: 9).Trim()}'");
			ShowErrorMessage(message: $"Failed to parse eccentricity: '{line.Substring(startIndex: 70, length: 9).Trim()}'");
			return false;
		}
		// Inclination: positions 59-67
		if (!double.TryParse(s: line.Substring(startIndex: 59, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out inclinationDeg))
		{
			// Log a warning and show an error message for failed inclination parsing
			logger.Warn(message: $"Failed to parse inclination: '{line.Substring(startIndex: 59, length: 9).Trim()}'");
			ShowErrorMessage(message: $"Failed to parse inclination: '{line.Substring(startIndex: 59, length: 9).Trim()}'");
			return false;
		}
		// Longitude of ascending node: positions 48-56
		if (!double.TryParse(s: line.Substring(startIndex: 48, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out longitudeAscendingNodeDeg))
		{
			// Log a warning and show an error message for failed longitude of ascending node parsing
			logger.Warn(message: $"Failed to parse longitude of ascending node: '{line.Substring(startIndex: 48, length: 9).Trim()}'");
			ShowErrorMessage(message: $"Failed to parse longitude of ascending node: '{line.Substring(startIndex: 48, length: 9).Trim()}'");
			return false;
		}
		// Argument of perihelion: positions 37-45
		if (!double.TryParse(s: line.Substring(startIndex: 37, length: 9).Trim(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out argumentPerihelionDeg))
		{
			// Log a warning and show an error message for failed argument of perihelion parsing
			logger.Warn(message: $"Failed to parse argument of perihelion: '{line.Substring(startIndex: 37, length: 9).Trim()}'");
			ShowErrorMessage(message: $"Failed to parse argument of perihelion: '{line.Substring(startIndex: 37, length: 9).Trim()}'");
			return false;
		}
		return true;
	}

	/// <summary>Finds the raw MPCORB record line whose designation matches the given name.</summary>
	/// <param name="name">The designation string to search for (case-insensitive).</param>
	/// <returns>The first matching raw record line, or <see langword="null"/> when no match is found.</returns>
	/// <remarks>The method iterates through all planetoid lines, extracting the designation from each line and comparing it to the given name. The comparison is case-insensitive. If a match is found, the corresponding line is returned immediately. If no match is found after checking all lines, the method returns <see langword="null"/>.</remarks>
	private string? FindLineByDesignation(string name)
	{
		// Iterate through all planetoid lines to find a matching designation
		foreach (string line in _planetoids)
		{
			// Extract the designation from the current line
			string? designation = ExtractDesignation(line: line);
			// Compare the extracted designation to the given name (case-insensitive)
			if (designation is not null && designation.Equals(value: name, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				logger.Info(message: $"Found matching line for designation '{name}': '{line}'");
				// Return the current line if the designation matches the given name
				return line;
			}
		}
		// Log a warning if no matching line was found for the given designation
		logger.Warn(message: $"No matching line found for designation: '{name}'");
		// Show an error message to the user indicating that no matching line was found
		return null;
	}

	/// <summary>Computes the MAXOID between the two currently selected minor planets and updates the result label.</summary>
	/// <remarks>If either combo box has no valid selection the label is reset to <c>"-"</c>. Parsing errors or calculation failures are logged and the label is set to <c>"-"</c>.</remarks>
	private void CalculateAndDisplayMaxoid()
	{
		// Retrieve the current text from both combo boxes
		string name1 = comboBoxPlanetoid1.Text;
		string name2 = comboBoxPlanetoid2.Text;
		// Both designations must be non-empty to compute a MAXOID
		if (string.IsNullOrWhiteSpace(value: name1) || string.IsNullOrWhiteSpace(value: name2))
		{
			kryptonLabelMaxoidValue.Text = "-";
			return;
		}
		// Look up the raw MPCORB lines for both planetoids
		string? line1 = FindLineByDesignation(name: name1);
		string? line2 = FindLineByDesignation(name: name2);
		if (line1 is null || line2 is null)
		{
			kryptonLabelMaxoidValue.Text = "-";
			return;
		}
		// Attempt to parse the orbital elements and calculate the MAXOID
		try
		{
			// Log the start of the MAXOID calculation between the two selected planetoids
			logger.Info(message: $"Calculating MAXOID between '{name1}' and '{name2}'.");
			// Parse orbital elements for the first planetoid
			if (!TryParseOrbitalElements(line: line1, semiMajorAxis: out double sma1, eccentricity: out double e1, inclinationDeg: out double i1, longitudeAscendingNodeDeg: out double omega1, argumentPerihelionDeg: out double w1))
			{
				// Log a warning if parsing orbital elements for the first planetoid fails
				logger.Warn(message: $"Failed to parse orbital elements for '{name1}'.");
				kryptonLabelMaxoidValue.Text = "-";
				return;
			}
			// Parse orbital elements for the second planetoid
			if (!TryParseOrbitalElements(line: line2, semiMajorAxis: out double sma2, eccentricity: out double e2, inclinationDeg: out double i2, longitudeAscendingNodeDeg: out double omega2, argumentPerihelionDeg: out double w2))
			{
				// Log a warning if parsing orbital elements for the second planetoid fails
				logger.Warn(message: $"Failed to parse orbital elements for '{name2}'.");
				kryptonLabelMaxoidValue.Text = "-";
				return;
			}
			logger.Info(message: $"Parsed orbital elements for '{name1}': SMA={sma1}, e={e1}, i={i1}, Ω={omega1}, ω={w1}");
			logger.Info(message: $"Parsed orbital elements for '{name2}': SMA={sma2}, e={e2}, i={i2}, Ω={omega2}, ω={w2}");
			// Calculate the MAXOID between the two orbits
			double maxoid = MaxoidCalculator.CalculateMaxoidBetweenPlanetoids(semiMajorAxis1: sma1, eccentricity1: e1, inclinationDeg1: i1, longitudeAscendingNodeDeg1: omega1, argumentPerihelionDeg1: w1, semiMajorAxis2: sma2, eccentricity2: e2, inclinationDeg2: i2, longitudeAscendingNodeDeg2: omega2, argumentPerihelionDeg2: w2);
			// Display the MAXOID in AU formatted to 8 decimal places
			kryptonLabelMaxoidValue.Text = maxoid.ToString(provider: CultureInfo.CurrentCulture);
		}
		// Catch any exceptions that occur during parsing or calculation and log them
		catch (Exception ex)
		{
			kryptonLabelMaxoidValue.Text = "-";
			logger.Error(exception: ex, message: $"Error calculating MAXOID between '{name1}' and '{name2}': {ex}");
			ShowErrorMessage(message: $"Error calculating MAXOID between '{name1}' and '{name2}': {ex.Message}");
		}
	}

	/// <summary>Applies a contains-based filter to the specified combo box based on the current text.</summary>
	/// <param name="comboBox">The combo box to filter.</param>
	/// <remarks>The combo box items are replaced with all names that contain the current text (case-insensitive). The cursor is repositioned at the end of the typed text after the update.</remarks>
	private void ApplyContainsFilter(ComboBox comboBox)
	{
		// Prevent re-entrant updates when the combo box is being modified programmatically
		if (_updatingComboBox)
		{
			// Log that the filter application is being skipped due to an ongoing update
			logger.Info(message: $"Skipping ApplyContainsFilter for {comboBox.Name} because an update is already in progress.");
			return;
		}
		// Set the guard flag to indicate that a programmatic update is in progress
		_updatingComboBox = true;
		// Use a try-finally block to ensure the guard flag is reset even if an exception occurs
		try
		{
			// Log the action of applying a contains-based filter to the combo box with the current text
			logger.Info(message: $"Applying contains-based filter to {comboBox.Name} with current text: '{comboBox.Text}'");
			// Get the current text from the combo box
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
		// Catch any exceptions that occur during filtering and log them
		catch (Exception ex)
		{
			// Log the error and show an error message to the user
			logger.Error(exception: ex, message: $"Error applying filter to combo box: {ex}");
			ShowErrorMessage(message: $"Error applying filter to combo box: {ex.Message}");
		}
		finally
		{
			// Reset the guard flag to allow future updates
			_updatingComboBox = false;
		}
	}

	/// <summary>Refreshes the displayed MAXOID state after either combo box text changes.</summary>
	/// <remarks>This ensures that manual typing or pasting updates the displayed result consistently: a valid pair is recalculated, while any invalid text clears a previously displayed stale MAXOID via <see cref="CalculateAndDisplayMaxoid"/>.</remarks>
	private void RecalculateMaxoidIfBothPlanetoidsAreValid()
	{
		// Recalculate and display the MAXOID based on the current selections in both combo boxes
		CalculateAndDisplayMaxoid();
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event. Populates both combo boxes with all planetoid designations from the database and configures autocomplete.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>The method first clears the status bar. It then extracts all planetoid designations from the raw database lines and populates both combo boxes with these names. An autocomplete source is also set up to provide standard "StartsWith" suggestions based on the full list of names. Any errors during this process are logged and displayed to the user.</remarks>
	private void MaxoidsRelativeToMinorPlanetsForm_Load(object sender, EventArgs e)
	{
		// Log that the form has loaded successfully
		logger.Info(message: "MaxoidsRelativeToMinorPlanetsForm loaded.");
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Log the number of planetoid names being loaded into the combo boxes
			logger.Info(message: $"Loading {_planetoids.Count} planetoid names into combo boxes.");
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
		// Catch any exceptions that occur during loading and log them
		catch (Exception ex)
		{
			// Log the error and show an error message to the user
			logger.Error(exception: ex, message: $"Error loading planetoid names: {ex}");
			ShowErrorMessage(message: $"Error loading planetoid names: {ex.Message}");
		}
	}

	/// <summary>Handles the FormClosing event. Logs the closing action and plays the close sound.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
	/// <remarks>This method is triggered when the form is about to close. It logs an informational message indicating that the form is closing and plays a close sound in the background to provide auditory feedback to the user.</remarks>
	private void MaxoidsRelativeToMinorPlanetsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing
		logger.Info(message: "MaxoidsRelativeToMinorPlanetsForm is closing.");
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region ComboBox event handlers

	/// <summary>Handles the SelectionChangeCommitted event for either combo box. Recalculates and displays the MAXOID whenever the user commits a selection.</summary>
	/// <param name="sender">Event source (one of the two combo boxes).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user changes the selection in either combo box. It calls the method to calculate and display the MAXOID based on the current selections.</remarks>
	private void ComboBoxPlanetoid_SelectionChangeCommitted(object sender, EventArgs e)
	{
		// Log the selection change event for debugging purposes
		logger.Info(message: $"SelectionChangeCommitted event triggered for combo box: {sender}");
		// Guard against null event arguments, though this should not happen in normal operation
		ArgumentNullException.ThrowIfNull(argument: e);
		// Recalculate and display the MAXOID based on the current selections in both combo boxes
		CalculateAndDisplayMaxoid();
	}

	#endregion

	#region TextChanged event handlers

	/// <summary>Handles the TextChanged event for the first combo box. Applies a contains-based filter to the first combo box items.</summary>
	/// <param name="sender">Event source (the first combo box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered whenever the text in the first combo box changes. It applies a filter to the items in the combo box so that only names containing the current text (case-insensitive) are shown. The cursor is repositioned at the end of the typed text after the update. If both combo boxes contain valid designations after filtering, the MAXOID is recalculated.</remarks>
	private void ComboBoxPlanetoid1_TextChanged(object sender, EventArgs e)
	{
		// Log the TextChanged event for debugging purposes
		logger.Info(message: $"TextChanged event triggered for ComboBoxPlanetoid1: {comboBoxPlanetoid1.Text}");
		// Guard against re-entrant updates when the combo box is being modified programmatically
		try
		{
			// Apply a contains-based filter to the first combo box items
			logger.Info(message: $"Applying contains-based filter to ComboBoxPlanetoid1 with text: '{comboBoxPlanetoid1.Text}'");
			// Apply a contains-based filter to the first combo box items
			ApplyContainsFilter(comboBox: comboBoxPlanetoid1);
			// Recalculate the MAXOID if both planetoids are valid after filtering
			RecalculateMaxoidIfBothPlanetoidsAreValid();
		}
		// Catch any exceptions that occur during filtering or recalculation and log them
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error handling TextChanged event for ComboBoxPlanetoid1: {ex}");
			ShowErrorMessage(message: $"Error handling TextChanged event for ComboBoxPlanetoid1: {ex.Message}");
		}
	}

	/// <summary>Handles the TextChanged event for the second combo box. Applies a contains-based filter to the second combo box items.</summary>
	/// <param name="sender">Event source (the second combo box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered whenever the text in the second combo box changes. It applies a filter to the items in the combo box so that only names containing the current text (case-insensitive) are shown. The cursor is repositioned at the end of the typed text after the update. If both combo boxes contain valid designations after filtering, the MAXOID is recalculated.</remarks>
	private void ComboBoxPlanetoid2_TextChanged(object sender, EventArgs e)
	{
		// Log the TextChanged event for debugging purposes
		logger.Info(message: $"TextChanged event triggered for ComboBoxPlanetoid2: {comboBoxPlanetoid2.Text}");
		// Guard against re-entrant updates when the combo box is being modified programmatically
		try
		{
			// Log the action of applying a contains-based filter to the second combo box
			logger.Info(message: $"Applying contains-based filter to ComboBoxPlanetoid2 with text: '{comboBoxPlanetoid2.Text}'");
			// Apply a contains-based filter to the first combo box items
			ApplyContainsFilter(comboBox: comboBoxPlanetoid2);
			// Recalculate the MAXOID if both planetoids are valid after filtering
			RecalculateMaxoidIfBothPlanetoidsAreValid();
		}
		// Catch any exceptions that occur during filtering or recalculation and log them
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error handling TextChanged event for ComboBoxPlanetoid2: {ex}");
			ShowErrorMessage(message: $"Error handling TextChanged event for ComboBoxPlanetoid2: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the random-selection button for the first combo box. Picks a random entry from the first combo box and triggers a MAXOID recalculation.</summary>
	/// <param name="sender">Event source (the first random button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user clicks the random-selection button next to the first combo box. It picks a random planetoid name from the full list of names (not the filtered list) and sets it as the text of the first combo box, which in turn triggers a MAXOID recalculation through the TextChanged event.</remarks>
	private void KryptonButtonRandomPlanetoid1_Click(object sender, EventArgs e)
	{
		// Log the action of clicking the random selection button for the first combo box
		logger.Info(message: "Random selection button clicked for ComboBoxPlanetoid1.");
		// Guard against empty planetoid list to avoid exceptions
		if (_allNames.Length > 0)
		{
			// Pick a random index from the full list, not the filtered list
			int index = random.Next(maxValue: _allNames.Length);
			comboBoxPlanetoid1.Text = _allNames[index];
			CalculateAndDisplayMaxoid();
		}
		// Catch any exceptions that occur during random selection and log them
		else
		{
			// Log a warning that random selection was attempted with an empty planetoid list
			logger.Warn(message: "Random selection attempted with empty planetoid list.");
		}
	}

	/// <summary>Handles the Click event of the random-selection button for the second combo box. Picks a random entry from the second combo box and triggers a MAXOID recalculation.</summary>
	/// <param name="sender">Event source (the second random button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is triggered when the user clicks the random-selection button next to the second combo box. It picks a random planetoid name from the full list of names (not the filtered list) and sets it as the text of the second combo box, which in turn triggers a MAXOID recalculation through the TextChanged event.</remarks>
	private void KryptonButtonRandomPlanetoid2_Click(object sender, EventArgs e)
	{
		// Log the action of clicking the random selection button for the second combo box
		logger.Info(message: "Random selection button clicked for ComboBoxPlanetoid2.");
		// Guard against empty planetoid list to avoid exceptions
		if (_allNames.Length > 0)
		{
			// Pick a random index from the full list, not the filtered list
			int index = random.Next(maxValue: _allNames.Length);
			comboBoxPlanetoid2.Text = _allNames[index];
			CalculateAndDisplayMaxoid();
		}
		// Catch any exceptions that occur during random selection and log them
		else
		{
			// Log a warning that random selection was attempted with an empty planetoid list
			logger.Warn(message: "Random selection attempted with empty planetoid list.");
		}
	}

	#endregion
}
