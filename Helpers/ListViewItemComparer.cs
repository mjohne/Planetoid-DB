// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace Planetoid_DB.Helpers;

/// <summary>Implements the manual sorting of items by column.</summary>
/// <remarks>This class is used internally by the form to provide custom sorting logic for the ListView control.</remarks>
/// <param name="column">The column index to sort by.</param>
/// <param name="order">The sort order (Ascending or Descending).</param>
public class ListViewItemComparer(int column, SortOrder order) : System.Collections.IComparer
{
	/// <summary>Column index to sort by.</summary>
	/// <remarks>This field stores the index of the column that is currently being sorted. It is used in the Compare method to determine which subitem's text to compare for sorting.</remarks>
	private readonly int col = column;

	/// <summary>Specifies the sort order used by the containing type.</summary>
	/// <remarks>This field indicates whether the sorting should be performed in ascending or descending order. It is used in the Compare method to determine how to return the comparison result.</remarks>
	private readonly SortOrder _order = order;

	/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
	public int Compare(object? x, object? y)
	{
		// Ensure both objects are ListViewItems; if not, consider them equal for sorting purposes
		if (x is not ListViewItem itemX || y is not ListViewItem itemY)
		{
			return 0;
		}
		// Retrieve the text for the specified column from both items; if the column index is out of range for an item, use an empty string for comparison
		string textX = itemX.SubItems.Count > col ? itemX.SubItems[index: col].Text : string.Empty;
		string textY = itemY.SubItems.Count > col ? itemY.SubItems[index: col].Text : string.Empty;
		// Attempt to parse the text as numbers for a more natural sorting order; if both can be parsed as numbers, compare them numerically; otherwise, compare them as strings
		bool hasNumericX = double.TryParse(s: textX, style: System.Globalization.NumberStyles.Any, provider: System.Globalization.CultureInfo.CurrentCulture, result: out double numX);
		bool hasNumericY = double.TryParse(s: textY, style: System.Globalization.NumberStyles.Any, provider: System.Globalization.CultureInfo.CurrentCulture, result: out double numY);
		// If both items have numeric values, compare them as numbers; otherwise, compare them as strings (case-insensitive)
		int result = hasNumericX && hasNumericY
			? numX.CompareTo(value: numY)
			: string.Compare(strA: textX, strB: textY, comparisonType: StringComparison.OrdinalIgnoreCase);
		// Return the comparison result, adjusting for the specified sort order (ascending or descending)
		return _order == SortOrder.Descending ? -result : result;
	}
}