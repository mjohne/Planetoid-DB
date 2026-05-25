// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Provides methods to calculate various types of averages from a collection of numeric values.</summary>
/// <remarks>This class implements multiple averaging methods including arithmetic mean, median, mode, geometric mean, harmonic mean, quadratic mean, cubic mean, logarithmic mean, Winsor mean, quartile mean, shortest half mean, Gastwirth-Cohen mean, range mean, a-mean, moving average, Hölder mean of shortest half, and Lehmer mean.</remarks>
public static class AverageCalculator
{
	/// <summary>Calculates the arithmetic mean (average) of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The arithmetic mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The arithmetic mean is the sum of all values divided by the count of values.</remarks>
	public static double ArithmeticMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		return valueArray.Length == 0 ? double.NaN : valueArray.Average();
	}

	/// <summary>Calculates the median of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The median value, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The median is the middle value when the values are sorted. If there's an even number of values, it returns the average of the two middle values.</remarks>
	public static double Median(IEnumerable<double> values)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;
		int mid = sorted.Length / 2;
		return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
	}

	/// <summary>Calculates the mode (most frequent value) of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The mode value, or <c>double.NaN</c> if the collection is empty or all values are unique.</returns>
	/// <remarks>The mode is the value that appears most frequently. If all values are unique, returns NaN.</remarks>
	public static double Mode(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		var grouped = valueArray.GroupBy(x => Math.Round(x, 6)).OrderByDescending(g => g.Count());
		var modeGroup = grouped.FirstOrDefault();

		// Return NaN if all values appear only once
		return modeGroup?.Count() > 1 ? modeGroup.Key : double.NaN;
	}

	/// <summary>Calculates the geometric mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The geometric mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>The geometric mean is the nth root of the product of n values. All values must be positive.</remarks>
	public static double GeometricMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v) && v > 0).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		double logSum = valueArray.Sum(v => Math.Log(v));
		return Math.Exp(logSum / valueArray.Length);
	}

	/// <summary>Calculates the harmonic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The harmonic mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>The harmonic mean is the reciprocal of the arithmetic mean of the reciprocals.</remarks>
	public static double HarmonicMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v) && v > 0).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		double reciprocalSum = valueArray.Sum(v => 1.0 / v);
		return valueArray.Length / reciprocalSum;
	}

	/// <summary>Calculates the quadratic mean (root mean square) of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The quadratic mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The quadratic mean is the square root of the arithmetic mean of the squares.</remarks>
	public static double QuadraticMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		double sumOfSquares = valueArray.Sum(v => v * v);
		return Math.Sqrt(sumOfSquares / valueArray.Length);
	}

	/// <summary>Calculates the cubic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The cubic mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The cubic mean is the cube root of the arithmetic mean of the cubes.</remarks>
	public static double CubicMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		double sumOfCubes = valueArray.Sum(v => v * v * v);
		return Math.Pow(sumOfCubes / valueArray.Length, 1.0 / 3.0);
	}

	/// <summary>Calculates the logarithmic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The logarithmic mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>For two values, the logarithmic mean is (b-a)/ln(b/a). For multiple values, uses pairwise average.</remarks>
	public static double LogarithmicMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v) && v > 0).OrderBy(x => x).ToArray();
		if (valueArray.Length == 0) return double.NaN;
		if (valueArray.Length == 1) return valueArray[0];

		// For multiple values, calculate pairwise logarithmic means
		double sum = 0;
		int count = 0;
		for (int i = 0; i < valueArray.Length - 1; i++)
		{
			double a = valueArray[i];
			double b = valueArray[i + 1];
			if (Math.Abs(b - a) < 1e-10) sum += a;
			else sum += (b - a) / Math.Log(b / a);
			count++;
		}
		return count > 0 ? sum / count : double.NaN;
	}

	/// <summary>Calculates the Winsorized mean by replacing extreme values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="percentile">The percentile to trim from each end (default 0.1 for 10%).</param>
	/// <returns>The Winsorized mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Replaces values below the lower percentile and above the upper percentile with the percentile values themselves.</remarks>
	public static double WinsorMean(IEnumerable<double> values, double percentile = 0.1)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;

		int lowerIndex = (int)(sorted.Length * percentile);
		int upperIndex = (int)(sorted.Length * (1 - percentile));

		double lowerBound = sorted[lowerIndex];
		double upperBound = sorted[upperIndex - 1];

		double[] winsorized = sorted.Select(v => v < lowerBound ? lowerBound : (v > upperBound ? upperBound : v)).ToArray();
		return winsorized.Average();
	}

	/// <summary>Calculates the quartile mean (average of the middle 50%).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The quartile mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Also known as the interquartile mean or midmean.</remarks>
	public static double QuartileMean(IEnumerable<double> values)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;

		int q1Index = sorted.Length / 4;
		int q3Index = (sorted.Length * 3) / 4;

		return sorted.Skip(q1Index).Take(q3Index - q1Index).Average();
	}

	/// <summary>Calculates the mean of the shortest half.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The mean of the shortest half, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Finds the shortest contiguous half of the sorted values and calculates their mean.</remarks>
	public static double ShortestHalfMean(IEnumerable<double> values)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;

		int halfLength = (sorted.Length + 1) / 2;
		double minRange = double.MaxValue;
		int bestStart = 0;

		for (int i = 0; i <= sorted.Length - halfLength; i++)
		{
			double range = sorted[i + halfLength - 1] - sorted[i];
			if (range < minRange)
			{
				minRange = range;
				bestStart = i;
			}
		}

		return sorted.Skip(bestStart).Take(halfLength).Average();
	}

	/// <summary>Calculates the Gastwirth-Cohen mean.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The Gastwirth-Cohen mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>A weighted combination of quartiles: 0.3*Q1 + 0.4*Median + 0.3*Q3.</remarks>
	public static double GastwirthCohenMean(IEnumerable<double> values)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;

		int n = sorted.Length;
		int q1Index = n / 4;
		int medianIndex = n / 2;
		int q3Index = (n * 3) / 4;

		double q1 = sorted[q1Index];
		double median = n % 2 == 0 ? (sorted[medianIndex - 1] + sorted[medianIndex]) / 2.0 : sorted[medianIndex];
		double q3 = sorted[q3Index];

		return 0.3 * q1 + 0.4 * median + 0.3 * q3;
	}

	/// <summary>Calculates the range mean (midrange).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The range mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The midrange is the average of the minimum and maximum values.</remarks>
	public static double RangeMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		return (valueArray.Min() + valueArray.Max()) / 2.0;
	}

	/// <summary>Calculates the "a"-mean (contraharmonic mean).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The "a"-mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The contraharmonic mean is the sum of squares divided by the sum.</remarks>
	public static double AMean(IEnumerable<double> values)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		double sumOfSquares = valueArray.Sum(v => v * v);
		double sum = valueArray.Sum();

		return sum != 0 ? sumOfSquares / sum : double.NaN;
	}

	/// <summary>Calculates the moving average.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="windowSize">The size of the moving window (default 3).</param>
	/// <returns>The moving average, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Calculates the average of averages of all windows of the specified size.</remarks>
	public static double MovingAverage(IEnumerable<double> values, int windowSize = 3)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).ToArray();
		if (valueArray.Length == 0) return double.NaN;
		if (valueArray.Length < windowSize) windowSize = valueArray.Length;

		List<double> movingAverages = [];
		for (int i = 0; i <= valueArray.Length - windowSize; i++)
		{
			movingAverages.Add(valueArray.Skip(i).Take(windowSize).Average());
		}

		return movingAverages.Count > 0 ? movingAverages.Average() : double.NaN;
	}

	/// <summary>Calculates the Hölder mean of the shortest half.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="p">The Hölder parameter (default 2 for quadratic mean).</param>
	/// <returns>The Hölder mean of the shortest half, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Combines the Hölder mean with the shortest half selection method.</remarks>
	public static double HolderMeanShortestHalf(IEnumerable<double> values, double p = 2)
	{
		double[] sorted = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(x => x).ToArray();
		if (sorted.Length == 0) return double.NaN;

		int halfLength = (sorted.Length + 1) / 2;
		double minRange = double.MaxValue;
		int bestStart = 0;

		for (int i = 0; i <= sorted.Length - halfLength; i++)
		{
			double range = sorted[i + halfLength - 1] - sorted[i];
			if (range < minRange)
			{
				minRange = range;
				bestStart = i;
			}
		}

		double[] shortestHalf = sorted.Skip(bestStart).Take(halfLength).ToArray();

		if (Math.Abs(p) < 1e-10) return GeometricMean(shortestHalf);

		double sumOfPowers = shortestHalf.Sum(v => Math.Pow(Math.Abs(v), p));
		return Math.Pow(sumOfPowers / shortestHalf.Length, 1.0 / p);
	}

	/// <summary>Calculates the Lehmer mean.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="p">The Lehmer parameter (default 2).</param>
	/// <returns>The Lehmer mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The Lehmer mean is a generalization that includes harmonic (p=0), geometric (p→1), arithmetic (p=1), and quadratic (p=2) means.</remarks>
	public static double LehmerMean(IEnumerable<double> values, double p = 2)
	{
		double[] valueArray = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v) && v >= 0).ToArray();
		if (valueArray.Length == 0) return double.NaN;

		if (Math.Abs(p) < 1e-10) return HarmonicMean(valueArray);
		if (Math.Abs(p - 1) < 1e-10) return ArithmeticMean(valueArray);

		double numerator = valueArray.Sum(v => Math.Pow(v, p));
		double denominator = valueArray.Sum(v => Math.Pow(v, p - 1));

		return denominator != 0 ? numerator / denominator : double.NaN;
	}
}
