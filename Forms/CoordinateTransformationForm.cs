/*
 * File:        CoordinateTransformationForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Provides an interface for converting astronomical coordinates.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Services;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Provides conversion between common astronomical coordinate systems.</summary>
/// <remarks>Right ascension and local hour angle may be entered either as decimal degrees or as hours (decimal or HH:MM:SS). Horizontal conversions require the observer's latitude and longitude.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed partial class CoordinateTransformationForm : BaseKryptonForm
{
	private static readonly AstronomicalCoordinateSystem[] coordinateSystems =
	[
		AstronomicalCoordinateSystem.Horizontal,
		AstronomicalCoordinateSystem.EquatorialJ2000,
		AstronomicalCoordinateSystem.EquatorialOfDate,
		AstronomicalCoordinateSystem.Ecliptic,
		AstronomicalCoordinateSystem.Galactic,
		AstronomicalCoordinateSystem.Supergalactic
	];

	/// <summary>Initializes a new instance of the <see cref="CoordinateTransformationForm"/> class.</summary>
	public CoordinateTransformationForm()
	{
		InitializeComponent();
		Text = global::Planetoid_DB.I18nStrings.CoordinateConversionTitle;
		comboBoxSource.Items.AddRange(items:
		[
			global::Planetoid_DB.I18nStrings.CoordinateConversionHorizontalSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEquatorialJ2000System,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEquatorialOfDateSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEclipticSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionGalacticSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionSupergalacticSystem
		]);
		comboBoxTarget.Items.AddRange(items:
		[
			global::Planetoid_DB.I18nStrings.CoordinateConversionHorizontalSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEquatorialJ2000System,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEquatorialOfDateSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionEclipticSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionGalacticSystem,
			global::Planetoid_DB.I18nStrings.CoordinateConversionSupergalacticSystem
		]);
		comboBoxAngleUnit.Items.AddRange(items:
		[
			global::Planetoid_DB.I18nStrings.CoordinateConversionHoursUnit,
			global::Planetoid_DB.I18nStrings.CoordinateConversionDegreesUnit
		]);
		comboBoxFirstCoordinateType.Items.AddRange(items:
		[
			global::Planetoid_DB.I18nStrings.CoordinateConversionRightAscension,
			global::Planetoid_DB.I18nStrings.CoordinateConversionHourAngle
		]);
		labelSource.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionSourceLabel;
		labelTarget.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionTargetLabel;
		labelFirstCoordinateType.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionInputTypeLabel;
		labelAngleUnit.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionAngleUnitLabel;
		labelDateTimeUtc.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionDateTimeLabel;
		labelObserverLatitude.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLatitudeLabel;
		labelObserverLongitude.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLongitudeLabel;
		labelResultTitle.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionResultLabel;
		buttonConvert.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionButtonText;
		SetAccessibilityMetadata();
		comboBoxSource.SelectedIndex = (int)AstronomicalCoordinateSystem.EquatorialJ2000;
		comboBoxTarget.SelectedIndex = (int)AstronomicalCoordinateSystem.Galactic;
		comboBoxAngleUnit.SelectedIndex = 0;
		comboBoxFirstCoordinateType.SelectedIndex = 0;
		dateTimePickerUtc.Value = DateTime.UtcNow;
		UpdateCoordinateLabels();
	}

	/// <summary>Releases the resources used by the form.</summary>
	/// <param name="disposing"><see langword="true"/> to release managed resources; otherwise, <see langword="false"/>.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
		}
		base.Dispose(disposing);
	}

	private string DebuggerDisplay => ToString();

	/// <summary>Converts the entered coordinates and displays the result or a validation error.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ButtonConvert_Click(object? sender, EventArgs e)
	{
		AstronomicalCoordinateSystem source = coordinateSystems[comboBoxSource.SelectedIndex];
		AstronomicalCoordinateSystem target = coordinateSystems[comboBoxTarget.SelectedIndex];
		bool isEquatorialInput = source is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate;
		bool inputIsHours = isEquatorialInput && comboBoxAngleUnit.SelectedIndex == 0;
		bool inputIsHourAngle = source == AstronomicalCoordinateSystem.EquatorialOfDate && comboBoxFirstCoordinateType.SelectedIndex == 1;
		if (!TryParseCoordinate(text: textBoxFirstCoordinate.Text, isHours: inputIsHours, value: out double firstCoordinate)
			|| !TryParseCoordinate(text: textBoxSecondCoordinate.Text, isHours: false, value: out double secondCoordinate))
		{
			labelResult.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionInvalidInput;
			return;
		}

		bool needsObserver = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| inputIsHourAngle;
		double observerLatitude = 0.0;
		double observerLongitude = 0.0;
		if (needsObserver
			&& (!TryParseNumber(text: textBoxObserverLatitude.Text, value: out observerLatitude)
				|| !TryParseNumber(text: textBoxObserverLongitude.Text, value: out observerLongitude)))
		{
			labelResult.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionInvalidObserverInput;
			return;
		}

		try
		{
			(double firstAngle, double secondAngle) = AstronomicalCoordinateConverter.Convert(
				firstAngleDegrees: inputIsHours ? firstCoordinate * 15.0 : firstCoordinate,
				secondAngleDegrees: secondCoordinate,
				source: source,
				target: target,
				dateTimeUtc: DateTime.SpecifyKind(value: dateTimePickerUtc.Value, kind: DateTimeKind.Utc),
				observerLatitudeDegrees: observerLatitude,
				observerLongitudeDegrees: observerLongitude,
				firstCoordinateIsHourAngle: inputIsHourAngle);
			labelResult.Text = FormatResult(firstAngleDegrees: firstAngle, secondAngleDegrees: secondAngle, target: target);
		}
		catch (ArgumentException)
		{
			labelResult.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionInvalidCoordinates;
		}
		catch (FormatException)
		{
			labelResult.Text = global::Planetoid_DB.I18nStrings.CoordinateConversionInvalidCoordinates;
		}
	}

	/// <summary>Updates the coordinate labels and enables hour-angle input only for equatorial coordinates of date.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void CoordinateSelection_Changed(object? sender, EventArgs e)
	{
		UpdateCoordinateLabels();
	}

	private void UpdateCoordinateLabels()
	{
		if (comboBoxSource.SelectedIndex < 0 || comboBoxTarget.SelectedIndex < 0)
		{
			return;
		}
		AstronomicalCoordinateSystem source = coordinateSystems[comboBoxSource.SelectedIndex];
		AstronomicalCoordinateSystem target = coordinateSystems[comboBoxTarget.SelectedIndex];
		labelFirstCoordinate.Text = FirstCoordinateName(source);
		labelSecondCoordinate.Text = SecondCoordinateName(source);
		textBoxFirstCoordinate.AccessibleName = labelFirstCoordinate.Text;
		textBoxSecondCoordinate.AccessibleName = labelSecondCoordinate.Text;
		labelAngleUnit.Enabled = source is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate;
		comboBoxAngleUnit.Enabled = labelAngleUnit.Enabled;
		labelFirstCoordinateType.Enabled = source == AstronomicalCoordinateSystem.EquatorialOfDate;
		comboBoxFirstCoordinateType.Enabled = labelFirstCoordinateType.Enabled;
		labelObserverLatitude.Enabled = source == AstronomicalCoordinateSystem.Horizontal
			|| target == AstronomicalCoordinateSystem.Horizontal
			|| (source == AstronomicalCoordinateSystem.EquatorialOfDate && comboBoxFirstCoordinateType.SelectedIndex == 1);
		textBoxObserverLatitude.Enabled = labelObserverLatitude.Enabled;
		labelObserverLongitude.Enabled = labelObserverLatitude.Enabled;
		textBoxObserverLongitude.Enabled = labelObserverLatitude.Enabled;
		labelResult.Text = string.Empty;
	}

	private void SetAccessibilityMetadata()
	{
		AccessibleName = global::Planetoid_DB.I18nStrings.CoordinateConversionTitle;
		AccessibleDescription = global::Planetoid_DB.I18nStrings.CoordinateConversionFormAccessibleDescription;
		AccessibleRole = AccessibleRole.Dialog;
		SetAccessibility(control: comboBoxSource, name: global::Planetoid_DB.I18nStrings.CoordinateConversionSourceLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionSourceAccessibleDescription, role: AccessibleRole.ComboBox);
		SetAccessibility(control: comboBoxTarget, name: global::Planetoid_DB.I18nStrings.CoordinateConversionTargetLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionTargetAccessibleDescription, role: AccessibleRole.ComboBox);
		SetAccessibility(control: textBoxFirstCoordinate, name: string.Empty, description: global::Planetoid_DB.I18nStrings.CoordinateConversionFirstCoordinateAccessibleDescription, role: AccessibleRole.Text);
		SetAccessibility(control: textBoxSecondCoordinate, name: string.Empty, description: global::Planetoid_DB.I18nStrings.CoordinateConversionSecondCoordinateAccessibleDescription, role: AccessibleRole.Text);
		SetAccessibility(control: comboBoxFirstCoordinateType, name: global::Planetoid_DB.I18nStrings.CoordinateConversionInputTypeLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionInputTypeAccessibleDescription, role: AccessibleRole.ComboBox);
		SetAccessibility(control: comboBoxAngleUnit, name: global::Planetoid_DB.I18nStrings.CoordinateConversionAngleUnitLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionAngleUnitAccessibleDescription, role: AccessibleRole.ComboBox);
		SetAccessibility(control: dateTimePickerUtc, name: global::Planetoid_DB.I18nStrings.CoordinateConversionDateTimeLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionDateTimeAccessibleDescription, role: AccessibleRole.SpinButton);
		SetAccessibility(control: textBoxObserverLatitude, name: global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLatitudeLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLatitudeAccessibleDescription, role: AccessibleRole.Text);
		SetAccessibility(control: textBoxObserverLongitude, name: global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLongitudeLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionObserverLongitudeAccessibleDescription, role: AccessibleRole.Text);
		SetAccessibility(control: buttonConvert, name: global::Planetoid_DB.I18nStrings.CoordinateConversionButtonText, description: global::Planetoid_DB.I18nStrings.CoordinateConversionButtonAccessibleDescription, role: AccessibleRole.PushButton);
		SetAccessibility(control: labelResult, name: global::Planetoid_DB.I18nStrings.CoordinateConversionResultLabel, description: global::Planetoid_DB.I18nStrings.CoordinateConversionResultAccessibleDescription, role: AccessibleRole.StaticText);
	}

	private static void SetAccessibility(Control control, string name, string description, AccessibleRole role)
	{
		control.AccessibleName = name;
		control.AccessibleDescription = description;
		control.AccessibleRole = role;
	}

	private string FirstCoordinateName(AstronomicalCoordinateSystem system)
	{
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => global::Planetoid_DB.I18nStrings.CoordinateConversionAzimuthLabel,
			AstronomicalCoordinateSystem.EquatorialJ2000 => global::Planetoid_DB.I18nStrings.CoordinateConversionRightAscensionLabel,
			AstronomicalCoordinateSystem.EquatorialOfDate => comboBoxFirstCoordinateType.SelectedIndex == 1
				? global::Planetoid_DB.I18nStrings.CoordinateConversionHourAngleLabel
				: global::Planetoid_DB.I18nStrings.CoordinateConversionRightAscensionLabel,
			AstronomicalCoordinateSystem.Ecliptic => global::Planetoid_DB.I18nStrings.CoordinateConversionEclipticLongitudeLabel,
			AstronomicalCoordinateSystem.Galactic => global::Planetoid_DB.I18nStrings.CoordinateConversionGalacticLongitudeLabel,
			AstronomicalCoordinateSystem.Supergalactic => global::Planetoid_DB.I18nStrings.CoordinateConversionSupergalacticLongitudeLabel,
			_ => string.Empty
		};
	}

	private static string SecondCoordinateName(AstronomicalCoordinateSystem system)
	{
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => global::Planetoid_DB.I18nStrings.CoordinateConversionAltitudeLabel,
			AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate => global::Planetoid_DB.I18nStrings.CoordinateConversionDeclinationLabel,
			AstronomicalCoordinateSystem.Ecliptic => global::Planetoid_DB.I18nStrings.CoordinateConversionEclipticLatitudeLabel,
			AstronomicalCoordinateSystem.Galactic => global::Planetoid_DB.I18nStrings.CoordinateConversionGalacticLatitudeLabel,
			AstronomicalCoordinateSystem.Supergalactic => global::Planetoid_DB.I18nStrings.CoordinateConversionSupergalacticLatitudeLabel,
			_ => string.Empty
		};
	}

	private static bool TryParseCoordinate(string text, bool isHours, out double value)
	{
		value = 0.0;
		string trimmed = text.Trim();
		if (isHours && trimmed.Contains(':', StringComparison.Ordinal))
		{
			string[] parts = trimmed.Split(':');
			if (parts.Length is < 2 or > 3
				|| !TryParseNumber(parts[0], out double first)
				|| !TryParseNumber(parts[1], out double minutes)
				|| (parts.Length == 3 && !TryParseNumber(parts[2], out _))
				|| minutes is < 0.0 or >= 60.0)
			{
				return false;
			}
			double seconds = parts.Length == 3 && TryParseNumber(parts[2], out double parsedSeconds) ? parsedSeconds : 0.0;
			if (seconds is < 0.0 or >= 60.0)
			{
				return false;
			}
			double sign = trimmed.StartsWith('-') ? -1.0 : 1.0;
			value = first + (sign * (minutes / 60.0)) + (sign * (seconds / 3600.0));
			return double.IsFinite(value);
		}

		return TryParseNumber(trimmed, out value);
	}

	private static bool TryParseNumber(string text, out double value)
	{
		bool parsed = double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.CurrentCulture, result: out value)
			|| double.TryParse(s: text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out value);
		return parsed && double.IsFinite(value);
	}

	private static string FormatResult(double firstAngleDegrees, double secondAngleDegrees, AstronomicalCoordinateSystem target)
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		string first = target is AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate
			? $"{CoordinateTransformationService.FormatRightAscension(hours: firstAngleDegrees / 15.0)} ({firstAngleDegrees.ToString("F8", culture)}°)"
			: $"{firstAngleDegrees.ToString("F8", culture)}°";
		return $"{first}{Environment.NewLine}{secondAngleDegrees.ToString("F8", culture)}°";
	}
}
