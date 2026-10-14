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
		comboBoxSource.Items.AddRange(items: ["Horizontal", "Equatorial (J2000)", "Equatorial (of date)", "Ecliptic (of date)", "Galactic", "Supergalactic"]);
		comboBoxTarget.Items.AddRange(items: ["Horizontal", "Equatorial (J2000)", "Equatorial (of date)", "Ecliptic (of date)", "Galactic", "Supergalactic"]);
		comboBoxAngleUnit.Items.AddRange(items: ["Hours (h:m:s)", "Degrees"]);
		comboBoxFirstCoordinateType.Items.AddRange(items: ["Right ascension", "Hour angle"]);
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

		try
		{
			(double firstAngle, double secondAngle) = AstronomicalCoordinateConverter.Convert(
				firstAngleDegrees: inputIsHours ? firstCoordinate * 15.0 : firstCoordinate,
				secondAngleDegrees: secondCoordinate,
				source: source,
				target: target,
				dateTimeUtc: DateTime.SpecifyKind(value: dateTimePickerUtc.Value, kind: DateTimeKind.Utc),
				observerLatitudeDegrees: ParseRequiredCoordinate(text: textBoxObserverLatitude.Text, fieldName: "Observer latitude"),
				observerLongitudeDegrees: ParseRequiredCoordinate(text: textBoxObserverLongitude.Text, fieldName: "Observer longitude"),
				firstCoordinateIsHourAngle: inputIsHourAngle);
			labelResult.Text = FormatResult(firstAngleDegrees: firstAngle, secondAngleDegrees: secondAngle, target: target);
		}
		catch (ArgumentException exception)
		{
			labelResult.Text = exception.Message;
		}
		catch (FormatException exception)
		{
			labelResult.Text = exception.Message;
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

	private static string FirstCoordinateName(AstronomicalCoordinateSystem system)
	{
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => "Azimuth (°)",
			AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate => "RA / hour angle",
			AstronomicalCoordinateSystem.Ecliptic => "Ecliptic longitude (°)",
			AstronomicalCoordinateSystem.Galactic => "Galactic longitude (°)",
			AstronomicalCoordinateSystem.Supergalactic => "Supergalactic longitude (°)",
			_ => "First coordinate"
		};
	}

	private static string SecondCoordinateName(AstronomicalCoordinateSystem system)
	{
		return system switch
		{
			AstronomicalCoordinateSystem.Horizontal => "Altitude (°)",
			AstronomicalCoordinateSystem.EquatorialJ2000 or AstronomicalCoordinateSystem.EquatorialOfDate => "Declination (°)",
			AstronomicalCoordinateSystem.Ecliptic => "Ecliptic latitude (°)",
			AstronomicalCoordinateSystem.Galactic => "Galactic latitude (°)",
			AstronomicalCoordinateSystem.Supergalactic => "Supergalactic latitude (°)",
			_ => "Second coordinate"
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

	private static double ParseRequiredCoordinate(string text, string fieldName)
	{
		return TryParseNumber(text: text, value: out double value)
			? value
			: throw new FormatException(message: $"{fieldName} must be a valid number in degrees.");
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
