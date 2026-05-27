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
		// Filter out NaN and Infinity values before calculating the average
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to average
		return valueArray.Length == 0 ? double.NaN : valueArray.Average();
	}

	/// <summary>Calculates the median of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The median value, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The median is the middle value when the values are sorted. If there's an even number of values, it returns the average of the two middle values.</remarks>
	public static double Median(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values, sort the remaining values, and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the median
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the median based on whether the count of values is odd or even
		int mid = sorted.Length / 2;
		// If the number of values is even, return the average of the two middle values; otherwise, return the middle value
		return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
	}

	/// <summary>Calculates the mode (most frequent value) of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The mode value, or <c>double.NaN</c> if the collection is empty or all values are unique.</returns>
	/// <remarks>The mode is the value that appears most frequently. If all values are unique, returns NaN.</remarks>
	public static double Mode(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values before calculating the mode
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the mode
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Group values by their rounded value (to handle floating-point precision issues) and order groups by their count in descending order
		IOrderedEnumerable<IGrouping<double, double>> grouped = valueArray.GroupBy(keySelector: static x => Math.Round(value: x, digits: 6)).OrderByDescending(keySelector: static g => g.Count());
		IGrouping<double, double>? modeGroup = grouped.FirstOrDefault();
		// Return NaN if all values appear only once
		return modeGroup?.Count() > 1 ? modeGroup.Key : double.NaN;
	}

	/// <summary>Calculates the geometric mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The geometric mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>The geometric mean is the nth root of the product of n values. All values must be positive.</remarks>
	public static double GeometricMean(IEnumerable<double> values)
	{
		// Filter out NaN, Infinity, and non-positive values before calculating the geometric mean
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v) && v > 0)];
		// Return NaN if there are no valid values to calculate the geometric mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Use logarithms to avoid overflow when multiplying many values together, then exponentiate the average log value
		double logSum = valueArray.Sum(selector: static v => Math.Log(d: v));
		return Math.Exp(d: logSum / valueArray.Length);
	}

	/// <summary>Calculates the harmonic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The harmonic mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>The harmonic mean is the reciprocal of the arithmetic mean of the reciprocals.</remarks>
	public static double HarmonicMean(IEnumerable<double> values)
	{
		// Filter out NaN, Infinity, and non-positive values before calculating the harmonic mean
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v) && v > 0)];
		// Return NaN if there are no valid values to calculate the harmonic mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the sum of the reciprocals of the values, then return the number of values divided by this sum
		double reciprocalSum = valueArray.Sum(selector: static v => 1.0 / v);
		return valueArray.Length / reciprocalSum;
	}

	/// <summary>Calculates the quadratic mean (root mean square) of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The quadratic mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The quadratic mean is the square root of the arithmetic mean of the squares.</remarks>
	public static double QuadraticMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values before calculating the quadratic mean
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the quadratic mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the sum of the squares of the values, then return the square root of the average of these squares
		double sumOfSquares = valueArray.Sum(selector: static v => v * v);
		return Math.Sqrt(d: sumOfSquares / valueArray.Length);
	}

	/// <summary>Calculates the cubic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The cubic mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The cubic mean is the cube root of the arithmetic mean of the cubes.</remarks>
	public static double CubicMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values before calculating the cubic mean
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the cubic mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the sum of the cubes of the values, then return the cube root of the average of these cubes
		double sumOfCubes = valueArray.Sum(selector: static v => v * v * v);
		return Math.Pow(x: sumOfCubes / valueArray.Length, y: 1.0 / 3.0);
	}

	/// <summary>Calculates the logarithmic mean of a collection of values.</summary>
	/// <param name="values">The collection of numeric values (must be positive).</param>
	/// <returns>The logarithmic mean, or <c>double.NaN</c> if the collection is empty or contains non-positive values.</returns>
	/// <remarks>For two values, the logarithmic mean is (b-a)/ln(b/a). For multiple values, uses pairwise average.</remarks>
	public static double LogarithmicMean(IEnumerable<double> values)
	{
		// Filter out NaN, Infinity, and non-positive values before calculating the logarithmic mean
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v) && v > 0).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the logarithmic mean
		switch (valueArray.Length)
		{
			case 0:
				return double.NaN;
			case 1:
				return valueArray[0];
		}
		// For multiple values, calculate pairwise logarithmic means
		double sum = 0;
		int count = 0;
		// Iterate through pairs of consecutive values, calculate their logarithmic mean, and accumulate the sum and count
		for (int i = 0; i < valueArray.Length - 1; i++)
		{
			// Calculate the logarithmic mean for the pair of values at index i and i+1
			double a = valueArray[i];
			double b = valueArray[i + 1];
			// If the two values are very close, use their average to avoid numerical instability; otherwise, calculate the logarithmic mean using the formula
			sum += (double)Math.Abs(value: b - a) switch
			{
				< 1e-10 => a,
				_ => (b - a) / Math.Log(d: b / a)
			};
			// Increment the count of pairs processed
			count++;
		}
		// Return the average of the pairwise logarithmic means, or NaN if no pairs were processed
		return count > 0 ? sum / count : double.NaN;
	}

	/// <summary>Calculates the Winsorized mean by replacing extreme values.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="percentile">The percentile to trim from each end (default 0.1 for 10%).</param>
	/// <returns>The Winsorized mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Replaces values below the lower percentile and above the upper percentile with the percentile values themselves.</remarks>
	public static double WinsorMean(IEnumerable<double> values, double percentile = 0.1)
	{
		// Clamp percentile to [0, 0.5) to ensure valid index calculations
		percentile = Math.Clamp(value: percentile, min: 0.0, max: 0.4999);
		// Filter out NaN and Infinity values, sort the remaining values, and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the Winsorized mean
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// Return the single value directly when there is only one element
		if (sorted.Length == 1)
		{
			return sorted[0];
		}
		// Calculate the indices for the lower and upper bounds based on the specified percentile
		int lowerIndex = (int)(sorted.Length * percentile);
		int upperIndex = Math.Max(val1: lowerIndex + 1, val2: (int)(sorted.Length * (1 - percentile)));
		// Clamp indices to valid array bounds
		lowerIndex = Math.Clamp(value: lowerIndex, min: 0, max: sorted.Length - 1);
		upperIndex = Math.Clamp(value: upperIndex, min: 1, max: sorted.Length);
		double lowerBound = sorted[lowerIndex];
		double upperBound = sorted[upperIndex - 1];
		// Create a new array where values below the lower bound are replaced with the lower bound, and values above the upper bound are replaced with the upper bound
		double[] winsorized = [.. sorted.Select(selector: v => v < lowerBound ? lowerBound : (v > upperBound ? upperBound : v))];
		// Return the average of the Winsorized values
		return winsorized.Average();
	}

	/// <summary>Calculates the quartile mean (average of the middle 50%).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The quartile mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Also known as the interquartile mean or midmean.</remarks>
	public static double QuartileMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values, sort the remaining values, and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the quartile mean
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// For small inputs, return the mean of the entire array to avoid empty slices
		if (sorted.Length < 4)
		{
			return sorted.Average();
		}
		// Calculate the indices for the first and third quartiles
		int q1Index = sorted.Length / 4;
		int q3Index = sorted.Length * 3 / 4;
		// Ensure the slice contains at least one element
		if (q3Index <= q1Index)
		{
			q3Index = q1Index + 1;
		}
		// Return the average of the values between the first and third quartiles
		return sorted.Skip(count: q1Index).Take(count: q3Index - q1Index).Average();
	}

	/// <summary>Calculates the mean of the shortest half.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The mean of the shortest half, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Finds the shortest contiguous half of the sorted values and calculates their mean.</remarks>
	public static double ShortestHalfMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values, sort the remaining values, and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the shortest half mean
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the length of the half to consider, which is half of the total number of values (rounded up)
		int halfLength = (sorted.Length + 1) / 2;
		// Initialize variables to track the minimum range and the starting index of the best half
		double minRange = double.MaxValue;
		int bestStart = 0;
		// Iterate through all possible contiguous halves of the sorted array, calculate their range, and keep track of the half with the smallest range
		for (int i = 0; i <= sorted.Length - halfLength; i++)
		{
			// Calculate the range of the current half (difference between the last and first value in the half)
			double range = sorted[i + halfLength - 1] - sorted[i];
			// If the range of the current half is smaller than the minimum range found so far, update the minimum range and the starting index of the best half
			if (range < minRange)
			{
				minRange = range;
				bestStart = i;
			}
		}
		// Return the average of the values in the shortest half found
		return sorted.Skip(count: bestStart).Take(count: halfLength).Average();
	}

	/// <summary>Calculates the Gastwirth-Cohen mean.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The Gastwirth-Cohen mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>A weighted combination of quartiles: 0.3*Q1 + 0.4*Median + 0.3*Q3.</remarks>
	public static double GastwirthCohenMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values, sort the remaining values, and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the Gastwirth-Cohen mean
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the indices for the first quartile, median, and third quartile
		int n = sorted.Length;
		int q1Index = n / 4;
		int medianIndex = n / 2;
		int q3Index = n * 3 / 4;
		// Calculate the values for the first quartile, median, and third quartile based on the sorted array and the calculated indices
		double q1 = sorted[q1Index];
		double median = n % 2 == 0 ? (sorted[medianIndex - 1] + sorted[medianIndex]) / 2.0 : sorted[medianIndex];
		double q3 = sorted[q3Index];
		// Return the weighted combination of the first quartile, median, and third quartile according to the Gastwirth-Cohen formula
		return (0.3 * q1) + (0.4 * median) + (0.3 * q3);
	}

	/// <summary>Calculates the range mean (midrange).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The range mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The midrange is the average of the minimum and maximum values.</remarks>
	public static double RangeMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values and convert to an array for indexing
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the range mean
		return valueArray.Length == 0 ? double.NaN : (valueArray.Min() + valueArray.Max()) / 2.0;
	}

	/// <summary>Calculates the "a"-mean (contraharmonic mean).</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <returns>The "a"-mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The contraharmonic mean is the sum of squares divided by the sum.</remarks>
	public static double AMean(IEnumerable<double> values)
	{
		// Filter out NaN and Infinity values and convert to an array for indexing
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the "a"-mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the sum of the squares of the values and the sum of the values, then return the contraharmonic mean as the sum of squares divided by the sum
		double sumOfSquares = valueArray.Sum(selector: v => v * v);
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
		// Clamp windowSize to at least 1 to avoid empty windows
		if (windowSize < 1)
		{
			windowSize = 1;
		}
		// Filter out NaN and Infinity values and convert to an array for indexing
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v))];
		// Return NaN if there are no valid values to calculate the moving average
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// If the window size is larger than the number of values, adjust it to the length of the array
		if (valueArray.Length < windowSize)
		{
			windowSize = valueArray.Length;
		}
		// Calculate the moving averages for all windows of the specified size and store them in a list
		List<double> movingAverages = [];
		for (int i = 0; i <= valueArray.Length - windowSize; i++)
		{
			movingAverages.Add(item: valueArray.Skip(count: i).Take(count: windowSize).Average());
		}
		// Return the average of the moving averages, or NaN if there are no moving averages calculated
		return movingAverages.Count > 0 ? movingAverages.Average() : double.NaN;
	}

	/// <summary>Calculates the Hölder mean of the shortest half.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="p">The Hölder parameter (default 2 for quadratic mean).</param>
	/// <returns>The Hölder mean of the shortest half, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>Combines the Hölder mean with the shortest half selection method.</remarks>
	public static double HolderMeanShortestHalf(IEnumerable<double> values, double p = 2)
	{
		// Filter out NaN and Infinity values and convert to an array for indexing
		double[] sorted = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v)).OrderBy(keySelector: static x => x)];
		// Return NaN if there are no valid values to calculate the Hölder mean of the shortest half
		if (sorted.Length == 0)
		{
			return double.NaN;
		}
		// Calculate the length of the half to consider, which is half of the total number of values (rounded up)
		int halfLength = (sorted.Length + 1) / 2;
		double minRange = double.MaxValue;
		int bestStart = 0;
		// Iterate through all possible contiguous halves of the sorted array, calculate their range, and keep track of the half with the smallest range
		for (int i = 0; i <= sorted.Length - halfLength; i++)
		{
			// Calculate the range of the current half (difference between the last and first value in the half)
			double range = sorted[i + halfLength - 1] - sorted[i];
			// If the range of the current half is smaller than the minimum range found so far, update the minimum range and the starting index of the best half
			if (range < minRange)
			{
				minRange = range;
				bestStart = i;
			}
		}
		// Extract the values in the shortest half found for further processing
		double[] shortestHalf = [.. sorted.Skip(count: bestStart).Take(count: halfLength)];
		// If the Hölder parameter p is close to zero, return the geometric mean of the shortest half; otherwise, calculate the Hölder mean using the specified parameter
		if (Math.Abs(value: p) < 1e-10)
		{
			return GeometricMean(values: shortestHalf);
		}
		// Calculate the sum of the absolute values raised to the power of p, then return the p-th root of the average of these values
		double sumOfPowers = shortestHalf.Sum(selector: v => Math.Pow(x: Math.Abs(value: v), y: p));
		return Math.Pow(x: sumOfPowers / shortestHalf.Length, y: 1.0 / p);
	}

	/// <summary>Calculates the Lehmer mean.</summary>
	/// <param name="values">The collection of numeric values.</param>
	/// <param name="p">The Lehmer parameter (default 2).</param>
	/// <returns>The Lehmer mean, or <c>double.NaN</c> if the collection is empty.</returns>
	/// <remarks>The Lehmer mean is a generalization that includes harmonic (p=0), geometric (p→1), arithmetic (p=1), and quadratic (p=2) means.</remarks>
	public static double LehmerMean(IEnumerable<double> values, double p = 2)
	{
		// Filter out NaN, Infinity, and negative values before calculating the Lehmer mean, as it is typically defined for non-negative values
		double[] valueArray = [.. values.Where(predicate: static v => !double.IsNaN(d: v) && !double.IsInfinity(d: v) && v >= 0)];
		// Return NaN if there are no valid values to calculate the Lehmer mean
		if (valueArray.Length == 0)
		{
			return double.NaN;
		}
		// If the Lehmer parameter p is close to zero, return the harmonic mean; if p is close to one, return the arithmetic mean; otherwise, calculate the Lehmer mean using the specified parameter
		if (Math.Abs(value: p) < 1e-10)
		{
			return HarmonicMean(values: valueArray);
		}
		// If p is close to 1, return the arithmetic mean to avoid numerical instability; otherwise, calculate the Lehmer mean using the formula: sum(v^p) / sum(v^(p-1))
		if (Math.Abs(value: p - 1) < 1e-10)
		{
			return ArithmeticMean(values: valueArray);
		}
		// Calculate the numerator as the sum of the values raised to the power of p, and the denominator as the sum of the values raised to the power of (p-1)
		double numerator = valueArray.Sum(selector: v => Math.Pow(x: v, y: p));
		double denominator = valueArray.Sum(selector: v => Math.Pow(x: v, y: p - 1));
		// Return the Lehmer mean as the numerator divided by the denominator, or NaN if the denominator is zero to avoid division by zero
		return denominator != 0 ? numerator / denominator : double.NaN;
	}
}