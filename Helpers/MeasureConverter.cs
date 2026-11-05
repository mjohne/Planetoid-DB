/*
 * File:        DateFormatterExtensions.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides methods to format date strings in "YYYYMMDD" format.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Numerics;

namespace Planetoid_DB.Helpers;

/// <summary>Provides conversions between decimal and sexagesimal representations of time and angles.</summary>
/// <remarks>Time and angle conversions use the relationship of 24 hours to 360 degrees. Sexagesimal seconds are rounded to four decimal places.</remarks>
internal static class MeasureConverter
{
	#region Helpers

	/// <summary>Splits a decimal value into units, minutes, and seconds.</summary>
	/// <remarks>Seconds are rounded to four decimal places, with rounding overflow carried into minutes and units.</remarks>
	/// <typeparam name="T">The floating-point type of the input value and seconds result.</typeparam>
	/// <param name="value">The decimal value to split.</param>
	/// <returns>A tuple containing the units, minutes, and seconds components.</returns>
	/// <remarks>Seconds are rounded to four decimal places, with rounding overflow carried into minutes and units.</remarks>
	private static (int units, int minutes, T seconds) SplitDecimal<T>(T value) where T : IFloatingPoint<T>
	{
		// Split the decimal value into units, minutes, and seconds

		// Calculate the whole units (hours or degrees)
		T floorVal = T.Floor(x: value);
		int units = int.CreateChecked(value: floorVal);
		// Calculate the remainder for minutes and seconds
		T c60 = T.CreateChecked(value: 60);
		T remainderMinutes = (value - floorVal) * c60;
		// Split the remainder into minutes and seconds
		T floorMinutes = T.Floor(x: remainderMinutes);
		int minutes = int.CreateChecked(value: floorMinutes);
		// Calculate the seconds, rounding to four decimal places
		T rawSeconds = (remainderMinutes - floorMinutes) * c60;
		T seconds = T.Round(x: rawSeconds, digits: 4);
		// Handle overflow via rounding (e.g., 59.99999" -> 60.0")
		if (seconds >= c60)
		{
			seconds = T.Zero;
			minutes++;
		}
		if (minutes >= 60)
		{
			minutes = 0;
			units++;
		}
		// Return the split values as a tuple
		return (units, minutes, seconds);
	}

	#endregion

	#region Decimal time <-> Sexagesimal time

	/// <summary>Converts a sexagesimal time value to decimal time.</summary>
	/// <typeparam name="T">The floating-point type used for the time values.</typeparam>
	/// <param name="time">The time value containing hours, minutes, and seconds.</param>
	/// <returns>The time expressed as decimal hours.</returns>
	/// <remarks>This method converts a sexagesimal time value (hours, minutes, seconds) into decimal hours.</remarks>
	public static T SexagesimalTimeToDecimalTime<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		// Convert hours and minutes to the floating-point type T
		T h = T.CreateChecked(value: time.Hours);
		T m = T.CreateChecked(value: time.Minutes);
		// Convert the time to decimal hours using the formula: decimalHours = hours + (minutes / 60) + (seconds / 3600)
		return h + (m / T.CreateChecked(value: 60)) + (time.Seconds / T.CreateChecked(value: 3600));
	}

	/// <summary>Converts decimal hours to sexagesimal time.</summary>
	/// <typeparam name="T">The floating-point type used for the decimal hours and seconds.</typeparam>
	/// <param name="decimalHours">The time in decimal hours.</param>
	/// <returns>A <see cref="SexagesimalTime{T}"/> representing the time in hours, minutes, and seconds.</returns>
	/// <remarks>This method converts decimal hours into a sexagesimal time representation (hours, minutes, seconds).</remarks>
	public static SexagesimalTime<T> DecimalTimeToSexagesimalTime<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		// Split the decimal hours into hours, minutes, and seconds using the SplitDecimal helper method
		(int units, int minutes, T? seconds) = SplitDecimal(value: decimalHours);
		// Return a new SexagesimalTime instance with the calculated units, minutes, and seconds
		return new SexagesimalTime<T>(Hours: units, Minutes: minutes, Seconds: seconds);
	}

	#endregion

	#region Decimal angle <-> Sexagesimal angle

	/// <summary>Converts a sexagesimal angle to a decimal angle.</summary>
	/// <typeparam name="T">The floating-point type used for the angle values.</typeparam>
	/// <param name="angle">The sexagesimal angle to convert.</param>
	/// <returns>The angle expressed as a decimal value in degrees.</returns>
	/// <remarks>This method converts a sexagesimal angle value (degrees, minutes, seconds) into a decimal angle representation.</remarks>
	public static T SexagesimalAngleToDecimalAngle<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		// Convert degrees and minutes to the floating-point type T
		T d = T.CreateChecked(value: angle.Degrees);
		// Convert minutes to the floating-point type T
		T m = T.CreateChecked(value: angle.Minutes);
		// Convert seconds to the floating-point type T
		T s = T.CreateChecked(value: angle.Seconds);
		// Convert the angle to decimal degrees using the formula: decimalDegrees = degrees + (minutes / 60) + (seconds / 3600)
		return d + (m / T.CreateChecked(value: 60)) + (s / T.CreateChecked(value: 3600));
	}

	/// <summary>Converts an angle in decimal degrees to a sexagesimal angle.</summary>
	/// <typeparam name="T">The floating-point type used for the angle values.</typeparam>
	/// <param name="decimalDegrees">The angle in decimal degrees.</param>
	/// <returns>A sexagesimal angle representing the specified angle.</returns>
	/// <remarks>This method converts an angle in decimal degrees into a sexagesimal representation (degrees, minutes, seconds).</remarks>
	public static SexagesimalAngle<T> DecimalAngleToSexagesimalAngle<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		// Split the decimal degrees into degrees, minutes, and seconds using the SplitDecimal helper method
		(int units, int minutes, T? seconds) = SplitDecimal(value: decimalDegrees);
		// Return a new SexagesimalAngle instance with the calculated degrees, minutes, and seconds
		return new SexagesimalAngle<T>(Degrees: units, Minutes: minutes, Seconds: seconds);
	}

	#endregion

	#region Decimal time <-> Decimal angle

	/// <summary>Converts decimal hours to decimal degrees.</summary>
	/// <typeparam name="T">The floating-point type of the input and result.</typeparam>
	/// <param name="decimalHours">The time in decimal hours.</param>
	/// <returns>The equivalent angle in decimal degrees.</returns>
	/// <remarks>This method converts decimal hours into decimal degrees using the relationship of 24 hours to 360 degrees.</remarks>
	public static T DecimalTimeToDecimalAngle<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		// Convert decimal hours to decimal degrees using the formula: decimalDegrees = decimalHours * 15
		return decimalHours * T.CreateChecked(value: 15);
	}

	/// <summary>Converts an angle in decimal degrees to decimal time.</summary>
	/// <typeparam name="T">The floating-point type used for the input and result.</typeparam>
	/// <param name="decimalDegrees">The angle, in decimal degrees.</param>
	/// <returns>The equivalent time in decimal units.</returns>
	/// <remarks>This method converts an angle in decimal degrees into decimal time using the relationship of 360 degrees to 24 hours.</remarks>
	public static T DecimalAngleToDecimalTime<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		// Convert decimal degrees to decimal hours using the formula: decimalHours = decimalDegrees / 15
		return decimalDegrees / T.CreateChecked(value: 15);
	}

	#endregion

	#region Sexagesimal time <-> Sexagesimal angle

	/// <summary>Converts a sexagesimal time to a sexagesimal angle.</summary>
	/// <typeparam name="T">The floating-point type used for the time and angle values.</typeparam>
	/// <param name="time">The sexagesimal time to convert.</param>
	/// <returns>The equivalent sexagesimal angle.</returns>
	/// <remarks>This method converts a sexagesimal time value (hours, minutes, seconds) into a sexagesimal angle representation (degrees, minutes, seconds) using the relationship of 24 hours to 360 degrees.</remarks>
	public static SexagesimalAngle<T> SexagesimalTimeToSexagesimalAngle<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		// Convert the sexagesimal time to decimal angle and then convert that decimal angle to sexagesimal angle
		return DecimalAngleToSexagesimalAngle(decimalDegrees: SexagesimalTimeToDecimalAngle(time: time));
	}

	/// <summary>Converts a sexagesimal angle to a sexagesimal time.</summary>
	/// <typeparam name="T">The floating-point type used for the angle and time values.</typeparam>
	/// <param name="angle">The sexagesimal angle to convert.</param>
	/// <returns>The equivalent sexagesimal time.</returns>
	/// <remarks>This method converts a sexagesimal angle value (degrees, minutes, seconds) into a sexagesimal time representation (hours, minutes, seconds) using the relationship of 360 degrees to 24 hours.</remarks>
	public static SexagesimalTime<T> SexagesimalAngleToSexagesimalTime<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		// Convert the sexagesimal angle to decimal time and then convert that decimal time to sexagesimal time
		return DecimalTimeToSexagesimalTime(decimalHours: SexagesimalAngleToDecimalTime(angle: angle));
	}

	#endregion

	#region Decimal time <-> Sexagesimal angle

	/// <summary>Converts decimal hours to a sexagesimal angle.</summary>
	/// <typeparam name="T">The floating-point numeric type used for the input and angle components.</typeparam>
	/// <param name="decimalHours">The time in decimal hours.</param>
	/// <returns>The equivalent sexagesimal angle.</returns>
	/// <remarks>This method converts decimal hours into a sexagesimal angle representation (degrees, minutes, seconds) using the relationship of 24 hours to 360 degrees.</remarks>
	public static SexagesimalAngle<T> DecimalTimeToSexagesimalAngle<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		// Convert decimal hours to decimal angle and then convert that decimal angle to sexagesimal angle
		return DecimalAngleToSexagesimalAngle(decimalDegrees: DecimalTimeToDecimalAngle(decimalHours: decimalHours));
	}

	/// <summary>Converts a sexagesimal angle to decimal time.</summary>
	/// <typeparam name="T">The floating-point type used for the angle and result.</typeparam>
	/// <param name="angle">The sexagesimal angle to convert.</param>
	/// <returns>The equivalent decimal time.</returns>
	/// <remarks>This method converts a sexagesimal angle value (degrees, minutes, seconds) into a decimal time representation (hours, minutes, seconds) using the relationship of 360 degrees to 24 hours.</remarks>
	public static T SexagesimalAngleToDecimalTime<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		// Convert the sexagesimal angle to decimal angle and then convert that decimal angle to decimal time
		return DecimalAngleToDecimalTime(decimalDegrees: SexagesimalAngleToDecimalAngle(angle: angle));
	}

	#endregion

	#region Sexagesimal time <-> Decimal angle

	/// <summary>Converts a sexagesimal time to a decimal angle.</summary>
	/// <typeparam name="T">The floating-point type used for the input and result.</typeparam>
	/// <param name="time">The sexagesimal time to convert.</param>
	/// <returns>The decimal angle corresponding to <paramref name="time"/>.</returns>
	/// <remarks>This method converts a sexagesimal time value (hours, minutes, seconds) into a decimal angle representation (degrees) using the relationship of 24 hours to 360 degrees.</remarks>
	public static T SexagesimalTimeToDecimalAngle<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		// Convert the sexagesimal time to decimal time and then convert that decimal time to decimal angle
		return DecimalTimeToDecimalAngle(decimalHours: SexagesimalTimeToDecimalTime(time: time));
	}

	/// <summary>Converts a decimal angle in degrees to sexagesimal time.</summary>
	/// <typeparam name="T">The floating-point numeric type used for the angle and result.</typeparam>
	/// <param name="decimalDegrees">The angle in decimal degrees to convert.</param>
	/// <returns>The angle represented as sexagesimal time.</returns>
	/// <remarks>This method converts a decimal angle value (degrees) into a sexagesimal time representation (hours, minutes, seconds) using the relationship of 360 degrees to 24 hours.</remarks>
	public static SexagesimalTime<T> DecimalAngleToSexagesimalTime<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		// Convert the decimal degrees to decimal time and then convert that decimal time to sexagesimal time
		return DecimalTimeToSexagesimalTime(decimalHours: DecimalAngleToDecimalTime(decimalDegrees: decimalDegrees));
	}

	#endregion
}
