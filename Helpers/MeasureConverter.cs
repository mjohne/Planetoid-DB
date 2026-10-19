using System.Numerics;

namespace Planetoid_DB.Helpers;

internal static class MeasureConverter
{
	#region Helpers

	private static (int units, int minutes, T seconds) SplitDecimal<T>(T value) where T : IFloatingPoint<T>
	{
		T floorVal = T.Floor(x: value);
		int units = int.CreateChecked(value: floorVal);

		T c60 = T.CreateChecked(value: 60);
		T remainderMinutes = (value - floorVal) * c60;

		T floorMinutes = T.Floor(x: remainderMinutes);
		int minutes = int.CreateChecked(value: floorMinutes);

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

		return (units, minutes, seconds);
	}

	#endregion

	#region Decimal time <-> Sexagesimal time

	public static T SexagesimalTimeToDecimalTime<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		T h = T.CreateChecked(value: time.Hours);
		T m = T.CreateChecked(value: time.Minutes);
		return h + (m / T.CreateChecked(value: 60)) + (time.Seconds / T.CreateChecked(value: 3600));
	}

	public static SexagesimalTime<T> DecimalTimeToSexagesimalTime<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		(int units, int minutes, T? seconds) = SplitDecimal(value: decimalHours);
		return new SexagesimalTime<T>(Hours: units, Minutes: minutes, Seconds: seconds);
	}

	#endregion

	#region Decimal angle <-> Sexagesimal angle

	public static T SexagesimalAngleToDecimalAngle<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		T d = T.CreateChecked(value: angle.Degrees);
		T m = T.CreateChecked(value: angle.Minutes);
		return d + (m / T.CreateChecked(value: 60)) + (angle.Seconds / T.CreateChecked(value: 3600));
	}

	public static SexagesimalAngle<T> DecimalAngleToSexagesimalAngle<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		(int units, int minutes, T? seconds) = SplitDecimal(value: decimalDegrees);
		return new SexagesimalAngle<T>(Degrees: units, Minutes: minutes, Seconds: seconds);
	}

	#endregion

	#region Decimal time <-> Decimal angle

	public static T DecimalTimeToDecimalAngle<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		return decimalHours * T.CreateChecked(value: 15);
	}

	public static T DecimalAngleToDecimalTime<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		return decimalDegrees / T.CreateChecked(value: 15);
	}

	#endregion

	#region Sexagesimal time <-> Sexagesimal angle

	public static SexagesimalAngle<T> SexagesimalTimeToSexagesimalAngle<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		return DecimalAngleToSexagesimalAngle(decimalDegrees: SexagesimalTimeToDecimalAngle(time: time));
	}

	public static SexagesimalTime<T> SexagesimalAngleToSexagesimalTime<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		return DecimalTimeToSexagesimalTime(decimalHours: SexagesimalAngleToDecimalTime(angle: angle));
	}

	#endregion

	#region Decimal time <-> Sexagesimal angle

	public static SexagesimalAngle<T> DecimalTimeToSexagesimalAngle<T>(T decimalHours) where T : IFloatingPoint<T>
	{
		return DecimalAngleToSexagesimalAngle(decimalDegrees: DecimalTimeToDecimalAngle(decimalHours: decimalHours));
	}

	public static T SexagesimalAngleToDecimalTime<T>(SexagesimalAngle<T> angle) where T : IFloatingPoint<T>
	{
		return DecimalAngleToDecimalTime(decimalDegrees: SexagesimalAngleToDecimalAngle(angle: angle));
	}

	#endregion

	#region Sexagesimal time <-> Decimal angle

	public static T SexagesimalTimeToDecimalAngle<T>(SexagesimalTime<T> time) where T : IFloatingPoint<T>
	{
		return DecimalTimeToDecimalAngle(decimalHours: SexagesimalTimeToDecimalTime(time: time));
	}

	public static SexagesimalTime<T> DecimalAngleToSexagesimalTime<T>(T decimalDegrees) where T : IFloatingPoint<T>
	{
		return DecimalTimeToSexagesimalTime(decimalHours: DecimalAngleToDecimalTime(decimalDegrees: decimalDegrees));
	}

	#endregion
}
