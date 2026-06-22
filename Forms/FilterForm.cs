// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Planetoid_DB;

/// <summary>Represents a form for filtering data in the Planetoid database.</summary>
/// <remarks>This form allows users to specify filter criteria for querying the database.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class FilterForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>The internal copy of the planetoids database used for filtering.</summary>
	/// <remarks>This list is filled via <see cref="FillArray"/> before the form is shown.</remarks>
	private List<string> planetoidsDatabase = [];

	/// <summary>Gets the filtered database that results from applying the current filter settings.</summary>
	/// <remarks>This property is populated when the user clicks Apply. It is <c>null</c> if the form was cancelled.</remarks>
	public List<string>? FilteredDatabase { get; private set; }

	/// <summary>Stores the initial (data-derived) minimum values for each orbital element, indexed by element name.</summary>
	private readonly Dictionary<string, decimal> defaultMinima = [];

	/// <summary>Stores the initial (data-derived) maximum values for each orbital element, indexed by element name.</summary>
	private readonly Dictionary<string, decimal> defaultMaxima = [];

	/// <summary>Flag used to suppress re-entrant ValueChanged events while programmatically setting spinbutton values.</summary>
	private bool suppressValueChangedEvents;

	/// <summary>Defines one fixed-width MPCORB orbital element field and the corresponding min/max spinbuttons.</summary>
	private readonly record struct OrbitalElementFilter(
		string Name,
		int Start,
		int Length,
		KryptonNumericUpDown MinimumControl,
		KryptonNumericUpDown MaximumControl);

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="FilterForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public FilterForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Center values in all numeric spin buttons
		foreach (KryptonNumericUpDown nud in Controls.OfType<KryptonNumericUpDown>()
			.Concat(second: tableLayoutPanel.Controls.OfType<KryptonNumericUpDown>()))
		{
			nud.TextAlign = HorizontalAlignment.Center;
		}
		// Enable double buffering for the TableLayoutPanel to prevent flickering
		try
		{
			// Set the DoubleBuffered property (protected)
			PropertyInfo? dbProp = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			dbProp?.SetValue(obj: tableLayoutPanel, value: true, index: null);
			// Also set specific control styles via reflection just in case
			MethodInfo? setStyleMethod = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			setStyleMethod?.Invoke(obj: tableLayoutPanel, parameters: [ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true]);
		}
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Could not set DoubleBuffered on tableLayoutPanel");
		}
	}

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Fills the internal planetoids database from the provided list.</summary>
	/// <param name="arrTemp">A list containing planetoid records as strings.</param>
	/// <remarks>Call this method before showing the form to supply the data that will be filtered.</remarks>
	public void FillArray(List<string> arrTemp) => planetoidsDatabase = [.. arrTemp];

	/// <summary>Tries to parse a value from a fixed-width MPCORB record line.</summary>
	/// <param name="line">The raw record line.</param>
	/// <param name="startIndex">Zero-based start index of the field.</param>
	/// <param name="length">Length of the field in characters.</param>
	/// <param name="value">When this method returns, contains the parsed value if parsing succeeded.</param>
	/// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
	private static bool TryParseField(string line, int startIndex, int length, out double value)
	{
		value = default;
		return line.Length >= startIndex + length && double.TryParse(
			s: line.Substring(startIndex: startIndex, length: length).Trim(),
			style: NumberStyles.Float,
			provider: CultureInfo.InvariantCulture,
			result: out value);
	}

	/// <summary>Creates the shared orbital-element field definition list used by scanning, resetting, and filtering.</summary>
	/// <returns>An array of orbital-element filter definitions.</returns>
	private OrbitalElementFilter[] GetOrbitalElementFilters() =>
	[
		new(Name: "MeanAnomaly",          Start: 26,  Length: 9,  MinimumControl: numericUpDownMinimumMeanAnomalyAtTheEpoch,       MaximumControl: numericUpDownMaximumMeanAnomalyAtTheEpoch),
		new(Name: "ArgPeri",              Start: 37,  Length: 9,  MinimumControl: numericUpDownMinimumArgumentOfThePerihelion,     MaximumControl: numericUpDownMaximumArgumentOfThePerihelion),
		new(Name: "LongAscNode",          Start: 48,  Length: 9,  MinimumControl: numericUpDownMinimumLongitudeOfTheAscendingNode, MaximumControl: numericUpDownMaximumLongitudeOfTheAscendingNode),
		new(Name: "Incl",                 Start: 59,  Length: 9,  MinimumControl: numericUpDownMinimumInclination,                 MaximumControl: numericUpDownMaximumInclination),
		new(Name: "OrbEcc",               Start: 70,  Length: 9,  MinimumControl: numericUpDownMinimumOrbitalEccentricity,         MaximumControl: numericUpDownMaximumOrbitalEccentricity),
		new(Name: "Motion",               Start: 80,  Length: 11, MinimumControl: numericUpDownMinimumMeanDailyMotion,             MaximumControl: numericUpDownMaximumMeanDailyMotion),
		new(Name: "SemiMajorAxis",        Start: 92,  Length: 11, MinimumControl: numericUpDownMinimumSemiMajorAxis,               MaximumControl: numericUpDownMaximumSemiMajorAxis),
		new(Name: "AbsoluteMagnitude",    Start: 8,   Length: 5,  MinimumControl: numericUpDownMinimumAbsoluteMagnitude,           MaximumControl: numericUpDownMaximumAbsoluteMagnitude),
		new(Name: "SlopeParameter",       Start: 14,  Length: 5,  MinimumControl: numericUpDownMinimumSlopeParameter,              MaximumControl: numericUpDownMaximumSlopeParameter),
		new(Name: "NumberOfOppositions",  Start: 123, Length: 3,  MinimumControl: numericUpDownMinimumNumberOfOppositions,         MaximumControl: numericUpDownMaximumNumberOfOppositions),
		new(Name: "NumberOfObservations", Start: 117, Length: 5,  MinimumControl: numericUpDownMinimumNumberOfObservations,        MaximumControl: numericUpDownMaximumNumberOfObservations),
		new(Name: "RmsResidual",          Start: 137, Length: 4,  MinimumControl: numericUpDownMinimumRmsResidual,                 MaximumControl: numericUpDownMaximumRmsResidual),
	];

	/// <summary>Computes the minimum and maximum values for all filtered orbital elements across the entire database.</summary>
	/// <remarks>This method iterates through all records and extracts min/max for each element using fixed-width parsing.</remarks>
	private void ComputeMinMaxFromDatabase()
	{
		OrbitalElementFilter[] elements = GetOrbitalElementFilters();

		// Initialize with inverted extremes
		Dictionary<string, double> minVals = [];
		Dictionary<string, double> maxVals = [];
		foreach (OrbitalElementFilter el in elements)
		{
			minVals[el.Name] = double.MaxValue;
			maxVals[el.Name] = double.MinValue;
		}

		// Scan all records
		foreach (string line in planetoidsDatabase)
		{
			foreach (OrbitalElementFilter el in elements)
			{
				if (TryParseField(line: line, startIndex: el.Start, length: el.Length, value: out double val))
				{
					if (val < minVals[el.Name]) { minVals[el.Name] = val; }
					if (val > maxVals[el.Name]) { maxVals[el.Name] = val; }
				}
			}
		}

		// Store defaults; fall back to 0 when no valid values were found
		defaultMinima.Clear();
		defaultMaxima.Clear();
		foreach (OrbitalElementFilter el in elements)
		{
			double mn = minVals[el.Name] == double.MaxValue ? 0 : minVals[el.Name];
			double mx = maxVals[el.Name] == double.MinValue ? 0 : maxVals[el.Name];
			if (minVals[el.Name] == double.MaxValue || maxVals[el.Name] == double.MinValue)
			{
				logger.Warn(message: $"No valid values found for orbital element '{el.Name}' in the database; defaulting min/max to 0.");
			}
			defaultMinima[el.Name] = (decimal)mn;
			defaultMaxima[el.Name] = (decimal)mx;
		}
	}

	/// <summary>Clamps and assigns a value to a <see cref="KryptonNumericUpDown"/> within the provided range.</summary>
	/// <param name="control">The spinbutton control to update.</param>
	/// <param name="value">The value to set.</param>
	/// <param name="lower">One bound for the spinbutton range.</param>
	/// <param name="upper">The other bound for the spinbutton range.</param>
	/// <remarks>If <paramref name="lower"/> is greater than <paramref name="upper"/>, the bounds are reordered automatically.</remarks>
	private static void SetNumericValue(KryptonNumericUpDown control, decimal value, decimal lower, decimal upper)
	{
		decimal minimum = Math.Min(val1: lower, val2: upper);
		decimal maximum = Math.Max(val1: lower, val2: upper);
		control.Minimum = minimum;
		control.Maximum = maximum;
		control.Value = Math.Max(val1: minimum, val2: Math.Min(val2: maximum, val1: value));
	}

	/// <summary>Applies the stored default min/max for one orbital element to its pair of spinbuttons.</summary>
	/// <param name="key">The dictionary key identifying the orbital element.</param>
	/// <param name="minimumControl">The minimum spinbutton.</param>
	/// <param name="maximumControl">The maximum spinbutton.</param>
	private void ResetElement(string key, KryptonNumericUpDown minimumControl, KryptonNumericUpDown maximumControl)
	{
		suppressValueChangedEvents = true;
		try
		{
			decimal minVal = defaultMinima.TryGetValue(key: key, value: out decimal mn) ? mn : 0m;
			decimal maxVal = defaultMaxima.TryGetValue(key: key, value: out decimal mx) ? mx : 0m;
			SetNumericValue(control: minimumControl, value: minVal, lower: minVal, upper: maxVal);
			SetNumericValue(control: maximumControl, value: maxVal, lower: minVal, upper: maxVal);
		}
		finally
		{
			suppressValueChangedEvents = false;
		}
	}

	/// <summary>Resets all spinbuttons to their data-derived default min/max values.</summary>
	private void ResetAllElements()
	{
		foreach (OrbitalElementFilter filter in GetOrbitalElementFilters())
		{
			ResetElement(key: filter.Name, minimumControl: filter.MinimumControl, maximumControl: filter.MaximumControl);
		}
	}

	/// <summary>Enables or disables the toolbar actions while default ranges are being computed.</summary>
	/// <param name="isEnabled"><see langword="true"/> to enable toolbar actions; otherwise, <see langword="false"/>.</param>
	private void SetToolbarActionsEnabled(bool isEnabled)
	{
		toolStripButtonApply.Enabled = isEnabled;
		toolStripButtonReset.Enabled = isEnabled;
	}

	/// <summary>Enforces that the minimum spinbutton value does not exceed the maximum spinbutton value.</summary>
	/// <param name="minimumControl">The minimum spinbutton.</param>
	/// <param name="maximumControl">The maximum spinbutton.</param>
	private void EnforceMinNotExceedsMax(KryptonNumericUpDown minimumControl, KryptonNumericUpDown maximumControl)
	{
		if (suppressValueChangedEvents)
		{
			return;
		}
		suppressValueChangedEvents = true;
		try
		{
			if (minimumControl.Value > maximumControl.Value)
			{
				maximumControl.Value = minimumControl.Value;
			}
		}
		finally
		{
			suppressValueChangedEvents = false;
		}
	}

	/// <summary>Enforces that the maximum spinbutton value is not less than the minimum spinbutton value.</summary>
	/// <param name="minimumControl">The minimum spinbutton.</param>
	/// <param name="maximumControl">The maximum spinbutton.</param>
	private void EnforceMaxNotBelowMin(KryptonNumericUpDown minimumControl, KryptonNumericUpDown maximumControl)
	{
		if (suppressValueChangedEvents)
		{
			return;
		}
		suppressValueChangedEvents = true;
		try
		{
			if (maximumControl.Value < minimumControl.Value)
			{
				minimumControl.Value = maximumControl.Value;
			}
		}
		finally
		{
			suppressValueChangedEvents = false;
		}
	}

	#endregion

	#region form event handlers

	/// <summary>Fired when the filter form has finished loading. Computes min/max values from the database and initialises the spinbuttons.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and set up any necessary data.</remarks>
	private async void FilterForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		// If the database is empty, skip computing min/max and just leave all spinbuttons at their default 0..100 range
		if (planetoidsDatabase.Count <= 0)
		{
			return;
		}
		// Compute min/max values for all orbital elements in a background task to avoid freezing the UI, and disable toolbar actions while this is in progress
		SetToolbarActionsEnabled(isEnabled: false);
		UseWaitCursor = true;
		// When the task completes, update the spinbuttons with the computed ranges. If an error occurs, log it and show an error message to the user.
		try
		{
			// Compute min/max values from the database in a background task
			try
			{
				// This may take a while for large databases, so run it in a background task to keep the UI responsive
				await Task.Run(action: ComputeMinMaxFromDatabase);
				ResetAllElements();
			}
			// Catch any exceptions that occur during the computation and log them, then show an error message to the user
			catch (Exception ex)
			{
				// Log the exception with an error level
				logger.Error(exception: ex, message: "Failed to compute initial filter ranges.");
				// Show an error message to the user indicating that the computation failed
				_ = KryptonMessageBox.Show(
					owner: this,
					text: "Unable to compute filter ranges from the current database.",
					caption: I18nStrings.ErrorCaption,
					buttons: KryptonMessageBoxButtons.OK,
					icon: KryptonMessageBoxIcon.Error);
			}
		}
		// Ensure the wait cursor is reset and toolbar actions are re-enabled even if an exception occurs
		finally
		{
			UseWaitCursor = false;
			SetToolbarActionsEnabled(isEnabled: true);
		}
	}

	#endregion

	#region Click & ButtonClick event handlers

	/// <summary>Handles the Click event of the ButtonResetMeanAnomalyAtTheEpoch. Resets the mean anomaly at the epoch filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the mean anomaly at the epoch filter.</remarks>
	private void ButtonResetMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e) =>
		ResetElement(key: "MeanAnomaly", minimumControl: numericUpDownMinimumMeanAnomalyAtTheEpoch, maximumControl: numericUpDownMaximumMeanAnomalyAtTheEpoch);

	/// <summary>Handles the Click event of the ButtonResetArgumentOfThePerihelion. Resets the argument of the perihelion filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the argument of the perihelion filter.</remarks>
	private void ButtonResetArgumentOfThePerihelion_Click(object sender, EventArgs e) =>
		ResetElement(key: "ArgPeri", minimumControl: numericUpDownMinimumArgumentOfThePerihelion, maximumControl: numericUpDownMaximumArgumentOfThePerihelion);

	/// <summary>Handles the Click event of the ButtonResetLongitudeOfTheAscendingNode. Resets the longitude of the ascending node filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the longitude of the ascending node filter.</remarks>
	private void ButtonResetLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) =>
		ResetElement(key: "LongAscNode", minimumControl: numericUpDownMinimumLongitudeOfTheAscendingNode, maximumControl: numericUpDownMaximumLongitudeOfTheAscendingNode);

	/// <summary>Handles the Click event of the ButtonResetInclination. Resets the inclination filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the inclination filter.</remarks>
	private void ButtonResetInclination_Click(object sender, EventArgs e) =>
		ResetElement(key: "Incl", minimumControl: numericUpDownMinimumInclination, maximumControl: numericUpDownMaximumInclination);

	/// <summary>Handles the Click event of the ButtonResetOrbitalEccentricity. Resets the orbital eccentricity filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the orbital eccentricity filter.</remarks>
	private void ButtonResetOrbitalEccentricity_Click(object sender, EventArgs e) =>
		ResetElement(key: "OrbEcc", minimumControl: numericUpDownMinimumOrbitalEccentricity, maximumControl: numericUpDownMaximumOrbitalEccentricity);

	/// <summary>Handles the Click event of the ButtonResetMeanDailyMotion. Resets the mean daily motion filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the mean daily motion filter.</remarks>
	private void ButtonResetMeanDailyMotion_Click(object sender, EventArgs e) =>
		ResetElement(key: "Motion", minimumControl: numericUpDownMinimumMeanDailyMotion, maximumControl: numericUpDownMaximumMeanDailyMotion);

	/// <summary>Handles the Click event of the ButtonResetSemiMajorAxis. Resets the semi-major axis filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the semi-major axis filter.</remarks>
	private void ButtonResetSemiMajorAxis_Click(object sender, EventArgs e) =>
		ResetElement(key: "SemiMajorAxis", minimumControl: numericUpDownMinimumSemiMajorAxis, maximumControl: numericUpDownMaximumSemiMajorAxis);

	/// <summary>Handles the Click event of the ButtonResetAbsoluteMagnitude. Resets the absolute magnitude filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the absolute magnitude filter.</remarks>
	private void ButtonResetAbsoluteMagnitude_Click(object sender, EventArgs e) =>
		ResetElement(key: "AbsoluteMagnitude", minimumControl: numericUpDownMinimumAbsoluteMagnitude, maximumControl: numericUpDownMaximumAbsoluteMagnitude);

	/// <summary>Handles the Click event of the ButtonResetSlopeParameter. Resets the slope parameter filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the slope parameter filter.</remarks>
	private void ButtonResetSlopeParameter_Click(object sender, EventArgs e) =>
		ResetElement(key: "SlopeParameter", minimumControl: numericUpDownMinimumSlopeParameter, maximumControl: numericUpDownMaximumSlopeParameter);

	/// <summary>Handles the Click event of the ButtonNumberOfOppositions. Resets the number of oppositions filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the number of oppositions filter.</remarks>
	private void ButtonNumberOfOppositions_Click(object sender, EventArgs e) =>
		ResetElement(key: "NumberOfOppositions", minimumControl: numericUpDownMinimumNumberOfOppositions, maximumControl: numericUpDownMaximumNumberOfOppositions);

	/// <summary>Handles the Click event of the ButtonResetNumberOfObservations. Resets the number of observations filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the number of observations filter.</remarks>
	private void ButtonResetNumberOfObservations_Click(object sender, EventArgs e) =>
		ResetElement(key: "NumberOfObservations", minimumControl: numericUpDownMinimumNumberOfObservations, maximumControl: numericUpDownMaximumNumberOfObservations);

	/// <summary>Handles the Click event of the ButtonResetRmsResidual. Resets the RMS residual filter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the RMS residual filter.</remarks>
	private void ButtonResetRmsResidual_Click(object sender, EventArgs e) =>
		ResetElement(key: "RmsResidual", minimumControl: numericUpDownMinimumRmsResidual, maximumControl: numericUpDownMaximumRmsResidual);

	/// <summary>Handles the Click event of the ButtonApply. Applies the filter settings and returns the filtered database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Rows that do not fall within the min/max range of every orbital element used in this form are removed. The filtered result is stored in <see cref="FilteredDatabase"/>.</remarks>
	private void ButtonApply_Click(object sender, EventArgs e)
	{
		OrbitalElementFilter[] filters = GetOrbitalElementFilters();

		FilteredDatabase = planetoidsDatabase
			.Where(predicate: line => filters.All(predicate: f =>
				TryParseField(line: line, startIndex: f.Start, length: f.Length, value: out double val) &&
				val >= (double)f.MinimumControl.Value &&
				val <= (double)f.MaximumControl.Value))
			.ToList();

		logger.Info(message: $"Filter applied: {FilteredDatabase.Count} of {planetoidsDatabase.Count} records retained.");
		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>Handles the Click event of the ButtonCancel. Cancels the filter settings and closes the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to cancel the filter settings and close the form.</remarks>
	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		FilteredDatabase = null;
		DialogResult = DialogResult.Cancel;
		Close();
	}

	/// <summary>Handles the Click event of the ButtonReset. Resets all filter settings to their default values.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset all filter settings to their default values.</remarks>
	private void ButtonReset_Click(object sender, EventArgs e) => ResetAllElements();

	#endregion

	#region ValueChanged event handlers

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumMeanAnomalyAtTheEpoch. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum mean anomaly at the epoch filter.</remarks>
	private void NumericUpDownMinimumMeanAnomalyAtTheEpoch_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumMeanAnomalyAtTheEpoch, maximumControl: numericUpDownMaximumMeanAnomalyAtTheEpoch);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumMeanAnomalyAtTheEpoch. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum mean anomaly at the epoch filter.</remarks>
	private void NumericUpDownMaximumMeanAnomalyAtTheEpoch_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumMeanAnomalyAtTheEpoch, maximumControl: numericUpDownMaximumMeanAnomalyAtTheEpoch);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumArgumentOfThePerihelion. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum argument of the perihelion filter.</remarks>
	private void NumericUpDownMinimumArgumentOfThePerihelion_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumArgumentOfThePerihelion, maximumControl: numericUpDownMaximumArgumentOfThePerihelion);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumArgumentOfThePerihelion. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum argument of the perihelion filter.</remarks>
	private void NumericUpDownMaximumArgumentOfThePerihelion_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumArgumentOfThePerihelion, maximumControl: numericUpDownMaximumArgumentOfThePerihelion);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumLongitudeOfTheAscendingNode. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum longitude of the ascending node filter.</remarks>
	private void NumericUpDownMinimumLongitudeOfTheAscendingNode_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumLongitudeOfTheAscendingNode, maximumControl: numericUpDownMaximumLongitudeOfTheAscendingNode);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumLongitudeOfTheAscendingNode. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum longitude of the ascending node filter.</remarks>
	private void NumericUpDownMaximumLongitudeOfTheAscendingNode_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumLongitudeOfTheAscendingNode, maximumControl: numericUpDownMaximumLongitudeOfTheAscendingNode);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumInclination. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum inclination filter.</remarks>
	private void NumericUpDownMinimumInclination_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumInclination, maximumControl: numericUpDownMaximumInclination);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumInclination. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum inclination filter.</remarks>
	private void NumericUpDownMaximumInclination_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumInclination, maximumControl: numericUpDownMaximumInclination);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumOrbitalEccentricity. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum orbital eccentricity filter.</remarks>
	private void NumericUpDownMinimumOrbitalEccentricity_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumOrbitalEccentricity, maximumControl: numericUpDownMaximumOrbitalEccentricity);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumOrbitalEccentricity. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum orbital eccentricity filter.</remarks>
	private void NumericUpDownMaximumOrbitalEccentricity_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumOrbitalEccentricity, maximumControl: numericUpDownMaximumOrbitalEccentricity);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumMeanDailyMotion. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum mean daily motion filter.</remarks>
	private void NumericUpDownMinimumMeanDailyMotion_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumMeanDailyMotion, maximumControl: numericUpDownMaximumMeanDailyMotion);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumMeanDailyMotion. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum mean daily motion filter.</remarks>
	private void NumericUpDownMaximumMeanDailyMotion_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumMeanDailyMotion, maximumControl: numericUpDownMaximumMeanDailyMotion);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumSemiMajorAxis. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum semi-major axis filter.</remarks>
	private void NumericUpDownMinimumSemiMajorAxis_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumSemiMajorAxis, maximumControl: numericUpDownMaximumSemiMajorAxis);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumSemiMajorAxis. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum semi-major axis filter.</remarks>
	private void NumericUpDownMaximumSemiMajorAxis_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumSemiMajorAxis, maximumControl: numericUpDownMaximumSemiMajorAxis);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumAbsoluteMagnitude. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum absolute magnitude filter.</remarks>
	private void NumericUpDownMinimumAbsoluteMagnitude_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumAbsoluteMagnitude, maximumControl: numericUpDownMaximumAbsoluteMagnitude);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumAbsoluteMagnitude. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum absolute magnitude filter.</remarks>
	private void NumericUpDownMaximumAbsoluteMagnitude_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumAbsoluteMagnitude, maximumControl: numericUpDownMaximumAbsoluteMagnitude);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumSlopeParameter. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum slope parameter filter.</remarks>
	private void NumericUpDownMinimumSlopeParameter_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumSlopeParameter, maximumControl: numericUpDownMaximumSlopeParameter);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumSlopeParameter. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum slope parameter filter.</remarks>
	private void NumericUpDownMaximumSlopeParameter_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumSlopeParameter, maximumControl: numericUpDownMaximumSlopeParameter);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumNumberOfOppositions. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum number of oppositions filter.</remarks>
	private void NumericUpDownMinimumNumberOfOppositions_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumNumberOfOppositions, maximumControl: numericUpDownMaximumNumberOfOppositions);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumNumberOfOppositions. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum number of oppositions filter.</remarks>
	private void NumericUpDownMaximumNumberOfOppositions_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumNumberOfOppositions, maximumControl: numericUpDownMaximumNumberOfOppositions);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumNumberOfObservations. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum number of observations filter.</remarks>
	private void NumericUpDownMinimumNumberOfObservations_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumNumberOfObservations, maximumControl: numericUpDownMaximumNumberOfObservations);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumNumberOfObservations. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum number of observations filter.</remarks>
	private void NumericUpDownMaximumNumberOfObservations_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumNumberOfObservations, maximumControl: numericUpDownMaximumNumberOfObservations);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMinimumRmsResidual. Ensures the minimum does not exceed the maximum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the minimum RMS residual filter.</remarks>
	private void NumericUpDownMinimumRmsResidual_ValueChanged(object sender, EventArgs e) =>
		EnforceMinNotExceedsMax(minimumControl: numericUpDownMinimumRmsResidual, maximumControl: numericUpDownMaximumRmsResidual);

	/// <summary>Handles the ValueChanged event of the NumericUpDownMaximumRmsResidual. Ensures the maximum is not below the minimum.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to update the maximum RMS residual filter.</remarks>
	private void NumericUpDownMaximumRmsResidual_ValueChanged(object sender, EventArgs e) =>
		EnforceMaxNotBelowMin(minimumControl: numericUpDownMinimumRmsResidual, maximumControl: numericUpDownMaximumRmsResidual);

	#endregion
}