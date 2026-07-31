// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Globalization;

namespace Planetoid_DB.Helpers;

/// <summary>Implements manual sorting of items by column for a <see cref="ListView"/>. Handles numeric values, text comparisons, and ensures strict sorting transitivity.</summary>
/// <param name="column">The column index to sort by.</param>
/// <param name="order">The sort order (<see cref="SortOrder.Ascending"/> or <see cref="SortOrder.Descending"/>).</param>
/// <remarks>This comparer ensures that numeric values are sorted before text values and that sorting is transitive, meaning if A &lt; B and B &lt; C, then A &lt; C.</remarks>
public class ListViewItemComparer(int column, SortOrder order) : System.Collections.IComparer
{
	/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>A signed integer indicating the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
	/// <remarks>This method ensures that numeric values are sorted before text values and that sorting is transitive.</remarks>
	public int Compare(object? x, object? y)
	{
		// Short-circuit if no sorting is required
		if (order == SortOrder.None)
		{
			return 0;
		}
		// Handle null/type mismatches deterministically
		if (x is not ListViewItem itemX)
		{
			return y is ListViewItem ? -1 : 0;
		}
		if (y is not ListViewItem itemY)
		{
			return 1;
		}
		// Extract subitem text safely using the captured primary constructor parameter 'column'
		string textX = column < itemX.SubItems.Count ? itemX.SubItems[column].Text : string.Empty;
		string textY = column < itemY.SubItems.Count ? itemY.SubItems[column].Text : string.Empty;
		// Attempt to parse both texts as numbers using the current culture for accurate numeric comparison
		bool isNumX = double.TryParse(s: textX, style: NumberStyles.Any, provider: CultureInfo.CurrentCulture, result: out double numX);
		bool isNumY = double.TryParse(s: textY, style: NumberStyles.Any, provider: CultureInfo.CurrentCulture, result: out double numY);
		// Compare numeric/text category first to keep numeric values grouped before text values (regardless of sort direction).
		int categoryResult = (isNumX ? 0 : 1).CompareTo(isNumY ? 0 : 1);
		if (categoryResult != 0)
		{
			return categoryResult;
		}
		// Both values are in the same category; compare within that category and apply the requested sort direction.
		int valueResult = isNumX
			? numX.CompareTo(value: numY)
			: string.Compare(strA: textX, strB: textY, comparisonType: StringComparison.OrdinalIgnoreCase);
		return order == SortOrder.Descending ? -valueResult : valueResult;
	}
}