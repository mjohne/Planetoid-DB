/*
 * File:        CoordinateTransformationForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Provides an interface for converting astronomical coordinates.
 * Remarks:     This form allows users to convert between various astronomical coordinate systems, including horizontal, equatorial (J2000 and of date), ecliptic, galactic, and supergalactic coordinates. It supports input in both decimal degrees and hours for right ascension and local hour angle. The form also requires the observer's latitude and longitude for horizontal conversions.
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

using Planetoid_DB.Helpers;
using Planetoid_DB.Services;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Provides conversion between common astronomical coordinate systems.</summary>
/// <remarks>Right ascension and local hour angle may be entered either as decimal degrees or as hours (decimal or HH:MM:SS). Horizontal conversions require the observer's latitude and longitude.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed partial class CoordinateTransformationForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>The list of astronomical coordinate systems available for conversion.</summary>
	/// <remarks>This array defines the order in which the coordinate systems appear in the source and target combo boxes.</remarks>
	private static readonly AstronomicalCoordinateSystem[] coordinateSystems = [
		AstronomicalCoordinateSystem.Horizontal,
		AstronomicalCoordinateSystem.EquatorialJ2000,
		AstronomicalCoordinateSystem.EquatorialOfDate,
		AstronomicalCoordinateSystem.Ecliptic,
		AstronomicalCoordinateSystem.Galactic,
		AstronomicalCoordinateSystem.Supergalactic
	];

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="CoordinateTransformationForm"/> class.</summary>
	/// <remarks>This constructor sets up the form's controls, populates combo boxes with coordinate system options, and initializes default values for the controls.</remarks>
	public CoordinateTransformationForm()
	{
		// Log the initialization of the form for debugging purposes.
		logger.Info(message: "Initializing CoordinateTransformationForm.");
		// Call the method to initialize the form's components and controls.
		InitializeComponent();
		// Enable double buffering for the TableLayoutPanel to prevent flickering
		DoubleBufferingHelper.EnableDoubleBuffering(control: kryptonTableLayoutPanel);
		// Set the form's title text using the localized string for coordinate conversion.
		Text = I18nStrings.CoordinateConversionTitle;
		// Populate the source and target combo boxes with the available astronomical coordinate systems.
		kryptonComboBoxSource.Items.AddRange(items:
		[
			I18nStrings.CoordinateConversionHorizontalSystem,
			I18nStrings.CoordinateConversionEquatorialJ2000System,
			I18nStrings.CoordinateConversionEquatorialOfDateSystem,
			I18nStrings.CoordinateConversionEclipticSystem,
			I18nStrings.CoordinateConversionGalacticSystem,
			I18nStrings.CoordinateConversionSupergalacticSystem
		]);
		kryptonComboBoxTarget.Items.AddRange(items:
		[
			I18nStrings.CoordinateConversionHorizontalSystem,
			I18nStrings.CoordinateConversionEquatorialJ2000System,
			I18nStrings.CoordinateConversionEquatorialOfDateSystem,
			I18nStrings.CoordinateConversionEclipticSystem,
			I18nStrings.CoordinateConversionGalacticSystem,
			I18nStrings.CoordinateConversionSupergalacticSystem
		]);
		// Populate the angle unit combo box with options for hours and degrees.
		kryptonComboBoxAngleUnit.Items.AddRange(items: [I18nStrings.CoordinateConversionHoursUnit, I18nStrings.CoordinateConversionDegreesUnit]);
		// Populate the first coordinate type combo box with options for right ascension and hour angle.
		kryptonComboBoxFirstCoordinateType.Items.AddRange(items: [I18nStrings.CoordinateConversionRightAscension, I18nStrings.CoordinateConversionHourAngle]);
		// Set the text for various labels on the form using localized strings.
		kryptonLabelSource.Text = I18nStrings.CoordinateConversionSourceLabel;
		kryptonLabelTarget.Text = I18nStrings.CoordinateConversionTargetLabel;
		kryptonLabelFirstCoordinateType.Text = I18nStrings.CoordinateConversionInputTypeLabel;
		kryptonLabelAngleUnit.Text = I18nStrings.CoordinateConversionAngleUnitLabel;
		kryptonLabelDateTimeUtc.Text = I18nStrings.CoordinateConversionDateTimeLabel;
		kryptonLabelObserverLatitude.Text = I18nStrings.CoordinateConversionObserverLatitudeLabel;
		kryptonLabelObserverLongitude.Text = I18nStrings.CoordinateConversionObserverLongitudeLabel;
		kryptonLabelResultTitle.Text = I18nStrings.CoordinateConversionResultLabel;
		toolStripButtonButtonConvert.Text = I18nStrings.CoordinateConversionButtonText;
		// Set the default selections for the combo boxes and date/time picker.
		kryptonComboBoxSource.SelectedIndex = (int)AstronomicalCoordinateSystem.EquatorialJ2000;
		kryptonComboBoxTarget.SelectedIndex = (int)AstronomicalCoordinateSystem.Galactic;
		kryptonComboBoxAngleUnit.SelectedIndex = 0;
		kryptonComboBoxFirstCoordinateType.SelectedIndex = 0;
		kryptonDateTimePickerUtc.Value = DateTime.UtcNow;
		// Update the coordinate labels based on the initial selections in the combo boxes.
		UpdateCoordinateLabels();
		// Log that the form has been initialized successfully
		logger.Info(message: "CoordinateTransformationForm initialized successfully.");
	}

	#endregion

	#region helpers

	/// <summary>Releases the resources used by the form.</summary>
	/// <param name="disposing"><see langword="true"/> to release managed resources; otherwise, <see langword="false"/>.</param>
	/// <remarks>This method ensures that all components of the form are properly disposed of when the form is closed, preventing memory leaks.</remarks>
	protected override void Dispose(bool disposing)
	{
		// Log the disposal of the form for debugging purposes.
		logger.Info($"Disposing CoordinateTransformationForm, disposing={disposing}");
		// Dispose of managed resources if disposing is true.
		if (disposing)
		{
			// Dispose of the components if they are not null.
			components?.Dispose();
		}
		// Call the base class Dispose method to ensure proper cleanup.
		base.Dispose(disposing);
	}

	/// <summary>Gets a string representation of the form for debugging purposes.</summary>
	/// <remarks>This property is used by the debugger to display a concise description of the form's state.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Runs the conversion when Enter is used as the dialog's default action.</summary>
	/// <param name="keyData">The key and modifier keys to process.</param>
	/// <returns>True if the key was handled; otherwise, the base implementation's result.</returns>
	protected override bool ProcessDialogKey(Keys keyData)
	{
		// Log the key press event for debugging purposes.
		logger.Info($"Key pressed: {keyData}");
		// Check if the Enter key was pressed to trigger the conversion.
		if (keyData == Keys.Enter)
		{
			// Log that the Enter key was pressed and the conversion will be triggered.
			logger.Info("Enter key pressed. Triggering conversion.");
			toolStripButtonButtonConvert.PerformClick();
			return true;
		}
		// Call the base implementation for other keys.
		return base.ProcessDialogKey(keyData: keyData);
	}

	/// <summary>Updates the coordinate labels based on the selected source coordinate system and enables or disables relevant input fields.</summary>
	/// <remarks>This method ensures that the form's labels and input fields accurately reflect the currently selected source and target coordinate systems, enabling or disabling controls as appropriate.</remarks>
	private void UpdateCoordinateLabels()
	{
		// Log the update of coordinate labels for debugging purposes.
		logger.Info("Updating coordinate labels based on selected source and target coordinate systems.");
		// Ensure that both source and target coordinate systems are selected before proceeding.
		if (kryptonComboBoxSource.SelectedIndex < 0 || kryptonComboBoxTarget.SelectedIndex < 0)
		{
			// Log a warning if either source or target coordinate system is not selected.
			logger.Warn("Source or target coordinate system not selected. Cannot update labels.");
			// Exit the method early since the labels cannot be updated without valid selections.
			return;
		}
		// Log the selected source and target coordinate systems for debugging purposes.
		logger.Info($"Selected source coordinate system: {coordinateSystems[kryptonComboBoxSource.SelectedIndex]}, Selected target coordinate system: {coordinateSystems[kryptonComboBoxTarget.SelectedIndex]}");
		// Retrieve the selected source and target coordinate systems.
		AstronomicalCoordinateSystem source = coordinateSystems[kryptonComboBoxSource.SelectedIndex];
		AstronomicalCoordinateSystem target = coordinateSystems[kryptonComboBoxTarget.SelectedIndex];
		// Update the labels for the first and second coordinates based on the selected source coordinate system.
		kryptonLabelFirstCoordinate.Text = FirstCoordinateName(system: source);
		kryptonLabelSecondCoordinate.Text = SecondCoordinateName(system: source);
		// Set the accessible names for the coordinate text boxes to match their corresponding labels for accessibility purposes.
		kryptonTextBoxFirstCoordinate.AccessibleName = kryptonLabelFirstCoordinate.Text;
		kryptonTextBoxSecondCoordinate.AccessibleName = kryptonLabelSecondCoordinate.Text;
		// Enable or disable the angle unit controls based on the selected source coordinate system.
		kryptonLabelAngleUnit.Enabled = source is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate;
		kryptonComboBoxAngleUnit.Enabled = kryptonLabelAngleUnit.Enabled;
		// Enable or disable the first coordinate type controls based on the selected source coordinate system.
		kryptonLabelFirstCoordinateType.Enabled = source == AstronomicalCoordinateSystem.EquatorialOfDate;
		kryptonComboBoxFirstCoordinateType.Enabled = kryptonLabelFirstCoordinateType.Enabled;
		// Enable or disable the observer latitude controls based on the selected source and target coordinate systems.
		kryptonLabelObserverLatitude.Enabled = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| (source == AstronomicalCoordinateSystem.EquatorialOfDate && kryptonComboBoxFirstCoordinateType.SelectedIndex == 1);
		// Enable or disable the observer longitude controls based on the observer latitude controls.
		kryptonTextBoxObserverLatitude.Enabled = kryptonLabelObserverLatitude.Enabled;
		kryptonLabelObserverLongitude.Enabled = kryptonLabelObserverLatitude.Enabled;
		kryptonTextBoxObserverLongitude.Enabled = kryptonLabelObserverLatitude.Enabled;
		// Clear the result label to ensure that previous results do not persist when the coordinate systems are changed.
		kryptonLabelResult.Text = string.Empty;
	}

	/// <summary>Gets the name of the first coordinate for a given astronomical coordinate system.</summary>
	/// <param name="system">The astronomical coordinate system.</param>
	/// <returns>The name of the first coordinate.</returns>
	/// <remarks>This method returns the appropriate label for the first coordinate based on the selected astronomical coordinate system, which is used to update the form's labels dynamically.</remarks>
	private string FirstCoordinateName(AstronomicalCoordinateSystem system)
	{
		// Log the retrieval of the first coordinate name for debugging purposes.
		logger.Info($"Retrieving first coordinate name for system: {system}");
		// Return the appropriate label for the first coordinate based on the selected astronomical coordinate system.
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => I18nStrings.CoordinateConversionAzimuthLabel,
			AstronomicalCoordinateSystem.EquatorialJ2000 => I18nStrings.CoordinateConversionRightAscensionLabel,
			AstronomicalCoordinateSystem.EquatorialOfDate => kryptonComboBoxFirstCoordinateType.SelectedIndex == 1
				? I18nStrings.CoordinateConversionHourAngleLabel
				: I18nStrings.CoordinateConversionRightAscensionLabel,
			AstronomicalCoordinateSystem.Ecliptic => I18nStrings.CoordinateConversionEclipticLongitudeLabel,
			AstronomicalCoordinateSystem.Galactic => I18nStrings.CoordinateConversionGalacticLongitudeLabel,
			AstronomicalCoordinateSystem.Supergalactic => I18nStrings.CoordinateConversionSupergalacticLongitudeLabel,
			_ => string.Empty
		};
	}

	/// <summary>Gets the name of the second coordinate for a given astronomical coordinate system.</summary>
	/// <param name="system">The astronomical coordinate system.</param>
	/// <returns>The name of the second coordinate.</returns>
	/// <remarks>This method returns the appropriate label for the second coordinate based on the selected astronomical coordinate system, which is used to update the form's labels dynamically.</remarks>
	private static string SecondCoordinateName(AstronomicalCoordinateSystem system)
	{
		// Log the retrieval of the second coordinate name for debugging purposes.
		logger.Info($"Retrieving second coordinate name for system: {system}");
		// Return the appropriate label for the second coordinate based on the selected astronomical coordinate system.
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => I18nStrings.CoordinateConversionAltitudeLabel,
			AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate => I18nStrings.CoordinateConversionDeclinationLabel,
			AstronomicalCoordinateSystem.Ecliptic => I18nStrings.CoordinateConversionEclipticLatitudeLabel,
			AstronomicalCoordinateSystem.Galactic => I18nStrings.CoordinateConversionGalacticLatitudeLabel,
			AstronomicalCoordinateSystem.Supergalactic => I18nStrings.CoordinateConversionSupergalacticLatitudeLabel,
			_ => string.Empty
		};
	}

	/// <summary>Attempts to parse a coordinate value from a string, supporting both decimal degrees and hours (for right ascension and local hour angle).</summary>
	/// <param name="text">The text representation of the coordinate.</param>
	/// <param name="isHours">Indicates whether the coordinate is in hours (true) or degrees (false).</param>
	/// <param name="value">The parsed coordinate value.</param>
	/// <returns>True if the coordinate was successfully parsed; otherwise, false.</returns>
	/// <remarks>This method handles the parsing of coordinate values, including support for the HH:MM:SS format when the input is in hours. It ensures that the parsed values are valid and finite.</remarks>
	private static bool TryParseCoordinate(string text, bool isHours, out double value)
	{
		// Initialize the output value to 0.0.
		value = 0.0;
		// Trim whitespace from the input text to ensure accurate parsing.
		string trimmed = text.Trim();
		// Check if the input is in hours and contains a colon, indicating the HH:MM:SS format.
		if (isHours && trimmed.Contains(value: ':', comparisonType: StringComparison.Ordinal))
		{
			// Split the input string into parts based on the colon separator.
			string[] parts = trimmed.Split(separator: ':');
			// Validate the number of parts and attempt to parse each part as a number, ensuring that minutes and seconds are within valid ranges.
			if (parts.Length is < 2 or > 3
				|| !TryParseNumber(text: parts[0], value: out double first)
				|| !TryParseNumber(text: parts[1], value: out double minutes)
				|| (parts.Length == 3 && !TryParseNumber(text: parts[2], value: out _))
				|| minutes is < 0.0 or >= 60.0)
			{
				// Log a warning if the input format is invalid or if any part of the coordinate is out of range.
				logger.Warn($"Invalid HH:MM:SS format or out-of-range values: '{text}'");
				// Return false to indicate that the coordinate could not be parsed successfully.
				return false;
			}
			// Parse the seconds part if it exists, defaulting to 0.0 if not provided.
			double seconds = parts.Length == 3 && TryParseNumber(text: parts[2], value: out double parsedSeconds) ? parsedSeconds : 0.0;
			// Validate that the seconds value is within the valid range.
			if (seconds is < 0.0 or >= 60.0)
			{
				// Log a warning if the seconds value is out of range.
				logger.Warn($"Seconds value out of range: {seconds} in input '{text}'");
				// Return false to indicate that the coordinate could not be parsed successfully.
				return false;
			}
			// Determine the sign of the coordinate based on whether it starts with a negative sign.
			double sign = trimmed.StartsWith(value: '-') ? -1.0 : 1.0;
			// Calculate the final coordinate value in hours, converting minutes and seconds to fractional hours.
			value = first + (sign * (minutes / 60.0)) + (sign * (seconds / 3600.0));
			// Log the successfully parsed coordinate value for debugging purposes.
			return double.IsFinite(d: value);
		}
		// If the input is not in HH:MM:SS format, attempt to parse it as a simple numeric value (in hours or degrees).
		return TryParseNumber(text: trimmed, value: out value);
	}

	/// <summary>Attempts to parse a numeric value from a string, supporting both the current culture and invariant culture formats.</summary>
	/// <param name="text">The text representation of the numeric value.</param>
	/// <param name="value">The parsed numeric value.</param>
	/// <returns>True if the numeric value was successfully parsed; otherwise, false.</returns>
	/// <remarks>This method ensures that the parsed numeric values are valid and finite, supporting both the current culture and invariant culture formats for flexibility in user input.</remarks>
	private static bool TryParseNumber(string text, out double value)
	{
		// Attempt to parse the numeric value using both the current culture and invariant culture formats.
		bool parsed = double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.CurrentCulture, result: out value)
			|| double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
		// Return true if the value was successfully parsed and is finite; otherwise, return false.
		return parsed && double.IsFinite(d: value);
	}

	/// <summary>Formats the conversion result for display, including right ascension in hours if applicable, and appends the corresponding degree values.</summary>
	/// <param name="firstAngleDegrees">The first angle in degrees.</param>
	/// <param name="secondAngleDegrees">The second angle in degrees.</param>
	/// <param name="target">The target astronomical coordinate system.</param>
	/// <returns>A formatted string representing the conversion result.</returns>
	/// <remarks>This method formats the conversion result for display, including right ascension in hours if applicable, and appends the corresponding degree values. It ensures that the output is user-friendly and informative.</remarks>
	private static string FormatResult(double firstAngleDegrees, double secondAngleDegrees, AstronomicalCoordinateSystem target)
	{
		// Use the current culture for formatting the output values.
		CultureInfo culture = CultureInfo.CurrentCulture;
		// Format the first angle based on whether the target coordinate system is equatorial (J2000 or of date), converting to hours if necessary.
		string first = target is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate
			? $"{CoordinateTransformationService.FormatRightAscension(hours: firstAngleDegrees / 15.0)} ({firstAngleDegrees.ToString(format: "F8", provider: culture)}°)"
			: $"{firstAngleDegrees.ToString(format: "F8", provider: culture)}°";
		// Format the second angle in degrees.
		return $"{first}{Environment.NewLine}{secondAngleDegrees.ToString(format: "F8", provider: culture)}°";
	}

	#endregion

	#region Form event handlers

	/// <summary>Initializes the coordinate transformation form and performs startup actions.</summary>
	/// <param name="sender">The event source that raised the load event.</param>
	/// <param name="e">The event data associated with the load event.</param>
	/// <remarks>This method is called when the form is loaded, allowing for any necessary initialization or startup actions, such as logging and playing sounds.</remarks>
	private void CoordinateTransformationForm_Load(object sender, EventArgs e)
	{
		// Log that the form has loaded successfully
		logger.Info(message: "CoordinateTransformationForm loaded.");
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
	}

	/// <summary>Handles the form closing event, logs the closure, and plays the close sound.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data for the closing event.</param>
	/// <remarks>This method is called when the form is closing, allowing for any necessary cleanup or logging before the form is disposed of.</remarks>
	private void CoordinateTransformationForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log that the form is closing
		logger.Info(message: "CoordinateTransformationForm is closing.");
		// Play the close sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion

	#region Click event handlers

	/// <summary>Converts the entered coordinates and displays the result or a validation error.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method retrieves the selected source and target coordinate systems, parses the input coordinates, and performs the conversion. It handles validation errors and displays appropriate messages to the user.</remarks>
	private void ButtonConvert_Click(object? sender, EventArgs e)
	{
		// Log the button click event for debugging purposes.
		logger.Info("Convert button clicked.");
		// Validate that both source and target coordinate systems are selected.
		AstronomicalCoordinateSystem source = coordinateSystems[kryptonComboBoxSource.SelectedIndex];
		AstronomicalCoordinateSystem target = coordinateSystems[kryptonComboBoxTarget.SelectedIndex];
		// Determine if the input coordinate system is equatorial (J2000 or of date).
		bool isEquatorialInput = source is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate;
		// Determine if the first coordinate input is in hours (for right ascension or hour angle).
		bool inputIsHours = isEquatorialInput && kryptonComboBoxAngleUnit.SelectedIndex == 0;
		// Determine if the first coordinate input is an hour angle (only applicable for equatorial coordinates of date).
		bool inputIsHourAngle = source == AstronomicalCoordinateSystem.EquatorialOfDate && kryptonComboBoxFirstCoordinateType.SelectedIndex == 1;
		//
		if (!TryParseCoordinate(text: kryptonTextBoxFirstCoordinate.Text, isHours: inputIsHours, value: out double firstCoordinate)
			|| !TryParseCoordinate(text: kryptonTextBoxSecondCoordinate.Text, isHours: false, value: out double secondCoordinate))
		{
			// Log the invalid input for debugging purposes.
			logger.Warn($"Invalid coordinate input: FirstCoordinate='{kryptonTextBoxFirstCoordinate.Text}', SecondCoordinate='{kryptonTextBoxSecondCoordinate.Text}'");
			// Display an error message to the user indicating invalid input.
			kryptonLabelResult.Text = I18nStrings.CoordinateConversionInvalidInput;
			// Exit the method early due to invalid input.
			return;
		}
		// Determine if observer latitude and longitude are needed for the conversion (required for horizontal coordinates or hour angle input).
		bool needsObserver = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| inputIsHourAngle;
		// Initialize observer latitude and longitude to default values.
		double observerLatitude = 0.0;
		double observerLongitude = 0.0;
		// Validate and parse observer latitude and longitude if needed.
		if (needsObserver
			&& (!TryParseNumber(text: kryptonTextBoxObserverLatitude.Text, value: out observerLatitude)
				|| !TryParseNumber(text: kryptonTextBoxObserverLongitude.Text, value: out observerLongitude)))
		{
			// Log the invalid observer input for debugging purposes.
			logger.Warn($"Invalid observer input: ObserverLatitude='{kryptonTextBoxObserverLatitude.Text}', ObserverLongitude='{kryptonTextBoxObserverLongitude.Text}'");
			// Display an error message to the user indicating invalid observer input.
			kryptonLabelResult.Text = I18nStrings.CoordinateConversionInvalidObserverInput;
			// Exit the method early due to invalid observer input.
			return;
		}
		// Perform the coordinate conversion and handle any exceptions that may arise during the process.
		try
		{
			// Log the conversion attempt for debugging purposes.
			logger.Info($"Attempting coordinate conversion: Source={source}, Target={target}, FirstCoordinate={firstCoordinate}, SecondCoordinate={secondCoordinate}, ObserverLatitude={observerLatitude}, ObserverLongitude={observerLongitude}, DateTimeUtc={kryptonDateTimePickerUtc.Value}");
			// Convert the coordinates using the AstronomicalCoordinateConverter service.
			(double firstAngle, double secondAngle) = AstronomicalCoordinateConverter.Convert(
				firstAngleDegrees: inputIsHours ? firstCoordinate * 15.0 : firstCoordinate,
				secondAngleDegrees: secondCoordinate,
				source: source,
				target: target,
				dateTimeUtc: DateTime.SpecifyKind(value: kryptonDateTimePickerUtc.Value, kind: DateTimeKind.Utc),
				observerLatitudeDegrees: observerLatitude,
				observerLongitudeDegrees: observerLongitude,
				firstCoordinateIsHourAngle: inputIsHourAngle);
			// Display the conversion result to the user.
			kryptonLabelResult.Text = FormatResult(firstAngleDegrees: firstAngle, secondAngleDegrees: secondAngle, target: target);
		}
		// Catch specific exceptions related to invalid coordinates and display appropriate error messages.
		catch (ArgumentException)
		{
			// Log the argument exception for debugging purposes.
			logger.Error("ArgumentException occurred during coordinate conversion.");
			// Display an error message to the user indicating invalid coordinates.
			kryptonLabelResult.Text = I18nStrings.CoordinateConversionInvalidCoordinates;
		}
		// Catch any other exceptions that may occur during the conversion process and log them for debugging purposes.
		catch (FormatException)
		{
			// Log the format exception for debugging purposes.
			logger.Error("FormatException occurred during coordinate conversion.");
			// Display an error message to the user indicating invalid coordinates.
			kryptonLabelResult.Text = I18nStrings.CoordinateConversionInvalidCoordinates;
		}
	}

	#endregion

	#region ListView event handlers

	/// <summary>Updates the coordinate labels and enables hour-angle input only for equatorial coordinates of date.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>This method is called when the user changes the selected source or target coordinate system, ensuring that the form's labels and input fields reflect the current selection.</remarks>
	private void CoordinateSelection_Changed(object? sender, EventArgs e)
	{
		// Log the coordinate selection change for debugging purposes.
		logger.Info($"Coordinate selection changed: SourceIndex={kryptonComboBoxSource.SelectedIndex}, TargetIndex={kryptonComboBoxTarget.SelectedIndex}");
		// Update the coordinate labels and input fields based on the selected coordinate systems.
		UpdateCoordinateLabels();
	}

	#endregion
}
