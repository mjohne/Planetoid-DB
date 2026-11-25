/*
 * File:        CitiesForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Form to display a virtualized, collapsible tree-like list of cities grouped by state and return selected geographic coordinates.
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
using Planetoid_DB.Resources;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Represents a dialog form that displays a virtualized, collapsible tree-like list of cities with their geographic coordinates.</summary>
/// <remarks>Loads city records from an embedded text resource, displays states as collapsed folder-like nodes in virtual <see cref="View.Details"/> mode, expands states on demand to reveal detailed city rows, and exposes the selected coordinates via public properties when confirmed.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class CitiesForm : BaseKryptonForm
{
	#region Constants

	/// <summary>Number of expected pipe-separated fields per city record line.</summary>
	/// <remarks>The expected column order is State, City, Latitude, and Longitude.</remarks>
	private const int expectedFieldCount = 4;

	/// <summary>Glyph prefix indicating a collapsed state node.</summary>
	/// <remarks>Displayed in the first column when <see cref="StateGroupNode.IsExpanded"/> is <see langword="false"/>.</remarks>
	private const string collapsedGlyph = "▶";

	/// <summary>Glyph prefix indicating an expanded state node.</summary>
	/// <remarks>Displayed in the first column when <see cref="StateGroupNode.IsExpanded"/> is <see langword="true"/>.</remarks>
	private const string expandedGlyph = "▼";

	#endregion

	#region Fields

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>Used to log diagnostic and error messages for <see cref="CitiesForm"/>.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Raw pipe-separated text content loaded from the embedded resource.</summary>
	/// <remarks>Initialized in the constructor from <see cref="Properties.Resources"/> or a provided resource string.</remarks>
	private readonly string citiesResourceContent;

	/// <summary>Hierarchical list of collapsible state nodes.</summary>
	/// <remarks>Preserves individual expansion states across column sorting operations.</remarks>
	private List<StateGroupNode> stateGroups = [];

	/// <summary>Flattened list of currently visible rows (all state nodes plus cities of expanded state nodes).</summary>
	/// <remarks>Rebuilt whenever data is loaded, sorted, or a state node is expanded or collapsed.</remarks>
	private List<CityListEntry> visibleEntries = [];

	/// <summary>Stores the index of the currently sorted column.</summary>
	/// <remarks>A value of <c>-1</c> indicates no active column sort.</remarks>
	private int sortColumn = -1;

	/// <summary>Stores the current sort direction for the active sort column.</summary>
	/// <remarks>Toggled when clicking the same column header repeatedly.</remarks>
	private SortOrder sortOrder = SortOrder.None;

	/// <summary>Bold font instance cached for collapsible state rows.</summary>
	/// <remarks>Created once and disposed when the form is disposed.</remarks>
	private readonly Font? stateNodeFont;

	private string textToCopyStateName = string.Empty;
	private string textToCopyCityName = string.Empty;
	private string textToCopyFormattedOverview = string.Empty;
	private string textToCopyLatitudeValue = string.Empty;
	private string textToCopyLongitudeValue = string.Empty;

	#endregion

	#region Public properties

	/// <summary>Gets the latitude in decimal degrees of the committed city.</summary>
	/// <remarks>Populated when the user confirms a valid city selection via <c>Commit coordinates</c> or double-click; otherwise <see langword="null"/>.</remarks>
	public double? SelectedLatitude { get; private set; }

	/// <summary>Gets the longitude in decimal degrees of the committed city.</summary>
	/// <remarks>Populated when the user confirms a valid city selection via <c>Commit coordinates</c> or double-click; otherwise <see langword="null"/>.</remarks>
	public double? SelectedLongitude { get; private set; }

	/// <summary>Gets the name of the committed city.</summary>
	/// <remarks>Populated when the user confirms a valid city selection; otherwise <see langword="null"/>.</remarks>
	public string? SelectedCity { get; private set; }

	/// <summary>Gets the state or territory name of the committed city.</summary>
	/// <remarks>Populated when the user confirms a valid city selection; otherwise <see langword="null"/>.</remarks>
	public string? SelectedState { get; private set; }

	#endregion

	#region Export override properties

	/// <summary>Gets the ListView control used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's results list.</remarks>
	protected override ListView? ExportListView => listView;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "List of cities";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "ListCities";

	#endregion

	#region Protected override properties

	/// <summary>Gets the status label used for displaying contextual status messages.</summary>
	/// <remarks>Overrides the base status label property to target <see cref="labelInformation"/>.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#endregion

	#region Constructors

	/// <summary>Initializes a new instance of the <see cref="CitiesForm"/> class using the embedded <c>cities</c> text resource.</summary>
	/// <remarks>Reads the pipe-separated city definitions directly from <see cref="Properties.Resources.cities"/>.</remarks>
	public CitiesForm() : this(citiesResourceContent: Properties.Resources.cities)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="CitiesForm"/> class with explicit resource text content.</summary>
	/// <param name="citiesResourceContent">The pipe-separated city data string retrieved from an application resource.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="citiesResourceContent"/> is <see langword="null"/>, empty, or whitespace.</exception>
	/// <remarks>Initializes UI components and prepares the cached state node font.</remarks>
	public CitiesForm(string citiesResourceContent)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(argument: citiesResourceContent);
		// Log the initialization of the form
		logger.Info(message: "Initializing CitiesForm...");
		this.citiesResourceContent = citiesResourceContent;
		// Initialize the form components
		InitializeComponent();
		stateNodeFont = new Font(prototype: listView.Font, newStyle: FontStyle.Bold);
		// Log that the form has been initialized successfully
		logger.Info(message: "ObservatoryCodesForm initialized.");
	}

	#endregion

	#region Helper methods

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <remarks>Used by the <see cref="DebuggerDisplayAttribute"/> to inspect the form instance.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Parses city records from <see cref="citiesResourceContent"/> and populates the virtual list view with collapsed state nodes.</summary>
	/// <remarks>Streams lines via <see cref="StringReader"/> to avoid splitting the entire resource string into an intermediate array, groups entries by state in collapsed nodes, and initializes <see cref="ListView.VirtualMode"/>.</remarks>
	private void LoadCitiesData()
	{
		// Log the start of the loading process
		logger.Info(message: "Loading cities resource into ListView.");
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		buttonCommitCoordinates.Enabled = false;
		if (string.IsNullOrWhiteSpace(value: citiesResourceContent))
		{
			logger.Error(message: "Embedded cities resource content is null or empty.");
			ShowErrorMessage(message: "Embedded cities resource data is empty.");
			return;
		}
		try
		{
			listView.BeginUpdate();
			listView.VirtualMode = false;
			listView.SelectedIndices.Clear();
			listView.Items.Clear();
			Dictionary<string, StateGroupNode> groupedMap = [with(comparer: StringComparer.OrdinalIgnoreCase)];
			int lineNumber = 0;
			int totalCities = 0;
			using StringReader reader = new(s: citiesResourceContent);
			string? line;
			while ((line = reader.ReadLine()) != null)
			{
				lineNumber++;
				if (lineNumber == 1 && line.StartsWith(value: "State|", comparisonType: StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				if (string.IsNullOrWhiteSpace(value: line))
				{
					continue;
				}
				if (TryParseCityLine(line: line, entry: out CityListEntry? parsedCity) && parsedCity != null)
				{
					if (!groupedMap.TryGetValue(key: parsedCity.State, value: out StateGroupNode? groupNode))
					{
						groupNode = new StateGroupNode
						{
							State = parsedCity.State,
							IsExpanded = false
						};
						groupedMap[key: parsedCity.State] = groupNode;
					}
					groupNode.Cities.Add(item: new CityListEntry
					{
						IsStateNode = false,
						ParentGroup = groupNode,
						State = parsedCity.State,
						City = parsedCity.City,
						Latitude = parsedCity.Latitude,
						Longitude = parsedCity.Longitude
					});
					totalCities++;
				}
				else
				{
					logger.Warn(message: $"Skipped malformed line {lineNumber} in embedded cities resource: {line}");
				}
			}
			stateGroups = [.. groupedMap.Values.OrderBy(keySelector: g => g.State, comparer: StringComparer.OrdinalIgnoreCase)];
			foreach (StateGroupNode group in stateGroups)
			{
				group.Cities.Sort(comparison: static (a, b) =>
				{
					ArgumentNullException.ThrowIfNull(argument: a);
					ArgumentNullException.ThrowIfNull(argument: b);
					return StringComparer.OrdinalIgnoreCase.Compare(x: a.City, y: b.City);
				});
			}
			RebuildVisibleEntries();
			sortColumn = -1;
			sortOrder = SortOrder.None;
			UpdateColumnHeaders();
			listView.VirtualMode = true;
			listView.VirtualListSize = visibleEntries.Count;
			SetStatusBar(label: labelInformation, text: $"{totalCities:N0} cities in {stateGroups.Count:N0} states loaded.");
			logger.Info(message: $"Loaded {totalCities} cities across {stateGroups.Count} states from embedded resource.");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error loading cities data from embedded resource.");
			ShowErrorMessage(message: $"Error loading cities data: {ex.Message}");
		}
		finally
		{
			listView.EndUpdate();
		}
	}

	/// <summary>Rebuilds the flattened <see cref="visibleEntries"/> list based on each state node's <see cref="StateGroupNode.IsExpanded"/> state.</summary>
	/// <remarks>Includes every <see cref="StateGroupNode"/> and appends its child cities only when <see cref="StateGroupNode.IsExpanded"/> is <see langword="true"/>.</remarks>
	private void RebuildVisibleEntries()
	{
		// Log the start of the loading process
		logger.Info(message: "Rebuilds the flattened visibleEntries list based on each state node's StateGroupNode.IsExpanded state");
		int capacity = stateGroups.Count;
		foreach (StateGroupNode group in stateGroups)
		{
			if (group.IsExpanded)
			{
				capacity += group.Cities.Count;
			}
		}
		List<CityListEntry> flattened = [with(capacity: capacity)];
		foreach (StateGroupNode group in stateGroups)
		{
			flattened.Add(item: new CityListEntry
			{
				IsStateNode = true,
				ParentGroup = group,
				State = group.State
			});
			if (group.IsExpanded)
			{
				flattened.AddRange(collection: group.Cities);
			}
		}
		visibleEntries = flattened;
	}

	/// <summary>Toggles or sets the expansion state of a <see cref="StateGroupNode"/> and updates the virtual list view while preserving selection.</summary>
	/// <param name="groupNode">The state node to expand or collapse.</param>
	/// <param name="expand">Optional explicit expansion state; if <see langword="null"/>, toggles the current state.</param>
	/// <remarks>Updates <see cref="ListView.VirtualListSize"/> and restores selection to the toggled state node.</remarks>
	private void SetGroupExpanded(StateGroupNode groupNode, bool? expand = null)
	{
		// Log the start of the loading process
		logger.Info(message: $"Toggles or sets the expansion state of a StateGroupNode and updates the virtual list view while preserving selection: groupNode: {groupNode}; expand={expand}");
		ArgumentNullException.ThrowIfNull(argument: groupNode);
		bool targetState = expand ?? !groupNode.IsExpanded;
		if (groupNode.IsExpanded == targetState)
		{
			return;
		}
		groupNode.IsExpanded = targetState;
		try
		{
			listView.BeginUpdate();
			RebuildVisibleEntries();
			listView.VirtualListSize = visibleEntries.Count;
			int stateRowIndex = visibleEntries.FindIndex(match: entry => entry.IsStateNode && ReferenceEquals(objA: entry.ParentGroup, objB: groupNode));
			if (stateRowIndex >= 0 && stateRowIndex < listView.VirtualListSize)
			{
				listView.SelectedIndices.Clear();
				_ = listView.SelectedIndices.Add(itemIndex: stateRowIndex);
				listView.EnsureVisible(index: stateRowIndex);
			}
			listView.Invalidate();
		}
		finally
		{
			listView.EndUpdate();
		}
	}

	/// <summary>Selects the parent state node row for a given <see cref="StateGroupNode"/>.</summary>
	/// <param name="groupNode">The parent state node to focus and select.</param>
	/// <remarks>Used when pressing <see cref="Keys.Left"/> on an expanded child city entry.</remarks>
	private void SelectStateNode(StateGroupNode groupNode)
	{
		// Log the start of the loading process
		logger.Info(message: $"Selects the parent state node row for a given StateGroupNode: {groupNode}");
		ArgumentNullException.ThrowIfNull(argument: groupNode);
		int stateRowIndex = visibleEntries.FindIndex(match: entry => entry.IsStateNode && ReferenceEquals(objA: entry.ParentGroup, objB: groupNode));
		if (stateRowIndex >= 0 && stateRowIndex < listView.VirtualListSize)
		{
			listView.SelectedIndices.Clear();
			_ = listView.SelectedIndices.Add(itemIndex: stateRowIndex);
			listView.EnsureVisible(index: stateRowIndex);
		}
	}

	/// <summary>Attempts to parse a single pipe-separated line into a <see cref="CityListEntry"/>, respecting double-quoted fields.</summary>
	/// <param name="line">The raw line read from the embedded resource.</param>
	/// <param name="entry">When this method returns <see langword="true"/>, contains the parsed <see cref="CityListEntry"/>; otherwise <see langword="null"/>.</param>
	/// <returns><see langword="true"/> if the line was parsed successfully; otherwise <see langword="false"/>.</returns>
	/// <remarks>Handles quoted fields containing embedded pipe characters such as <c>"Bahamas| The"|Nassau|25.0781|-77.3386</c> using <see cref="ReadOnlySpan{T}"/> without intermediate split allocations.</remarks>
	private static bool TryParseCityLine(string line, out CityListEntry? entry)
	{
		entry = null;
		ReadOnlySpan<char> span = line.AsSpan();
		Span<Range> fieldRanges = stackalloc Range[expectedFieldCount];
		int fieldIndex = 0;
		int fieldStart = 0;
		bool inQuotes = false;
		for (int i = 0; i < span.Length; i++)
		{
			char currentChar = span[index: i];
			if (currentChar == '"')
			{
				inQuotes = !inQuotes;
			}
			else if (currentChar == '|' && !inQuotes)
			{
				if (fieldIndex >= expectedFieldCount - 1)
				{
					return false;
				}
				fieldRanges[fieldIndex++] = new Range(start: fieldStart, end: i);
				fieldStart = i + 1;
			}
		}
		if (fieldIndex != expectedFieldCount - 1)
		{
			return false;
		}
		fieldRanges[fieldIndex] = new Range(start: fieldStart, end: span.Length);
		ReadOnlySpan<char> stateSpan = UnquoteAndTrim(field: span[fieldRanges[index: 0]]);
		ReadOnlySpan<char> citySpan = UnquoteAndTrim(field: span[fieldRanges[index: 1]]);
		ReadOnlySpan<char> latSpan = UnquoteAndTrim(field: span[fieldRanges[index: 2]]);
		ReadOnlySpan<char> lonSpan = UnquoteAndTrim(field: span[fieldRanges[index: 3]]);
		if (stateSpan.IsEmpty || citySpan.IsEmpty)
		{
			return false;
		}
		if (!double.TryParse(s: latSpan, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double latitude)
			|| !double.TryParse(s: lonSpan, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out double longitude))
		{
			return false;
		}
		entry = new CityListEntry
		{
			IsStateNode = false,
			State = stateSpan.ToString(),
			City = citySpan.ToString(),
			Latitude = latitude,
			Longitude = longitude
		};
		return true;
	}

	/// <summary>Trims whitespace and strips surrounding double quotes from a field slice.</summary>
	/// <param name="field">The raw field slice.</param>
	/// <returns>The trimmed and unquoted <see cref="ReadOnlySpan{T}"/>.</returns>
	/// <remarks>Avoids string allocations during CSV parsing.</remarks>
	private static ReadOnlySpan<char> UnquoteAndTrim(ReadOnlySpan<char> field)
	{
		ReadOnlySpan<char> trimmed = field.Trim();
		if (trimmed.Length >= 2 && trimmed[index: 0] == '"' && trimmed[^1] == '"')
		{
			trimmed = trimmed[1..^1].Trim();
		}
		return trimmed;
	}

	/// <summary>Gets the currently selected <see cref="CityListEntry"/> from the virtual list view.</summary>
	/// <returns>The selected <see cref="CityListEntry"/> if an item is selected and valid; otherwise <see langword="null"/>.</returns>
	/// <remarks>Returns both collapsible state nodes and detailed city entries so callers can distinguish between them.</remarks>
	private CityListEntry? GetSelectedEntry()
	{
		if (listView.SelectedIndices.Count == 0)
		{
			return null;
		}
		int index = listView.SelectedIndices[index: 0];
		return index >= 0 && index < visibleEntries.Count ? visibleEntries[index: index] : null;
	}

	/// <summary>Restores the selection in the virtual list view after sorting.</summary>
	/// <param name="previouslySelected">The <see cref="CityListEntry"/> that was selected prior to sorting.</param>
	/// <remarks>Matches either the exact city entry or the owning <see cref="StateGroupNode"/> and scrolls it into view.</remarks>
	private void RestoreSelection(CityListEntry? previouslySelected)
	{
		if (previouslySelected == null)
		{
			return;
		}
		int newIndex = previouslySelected.IsStateNode
			? visibleEntries.FindIndex(match: entry => entry.IsStateNode && ReferenceEquals(objA: entry.ParentGroup, objB: previouslySelected.ParentGroup))
			: visibleEntries.IndexOf(item: previouslySelected);
		if (newIndex >= 0 && newIndex < listView.VirtualListSize)
		{
			listView.SelectedIndices.Clear();
			_ = listView.SelectedIndices.Add(itemIndex: newIndex);
			listView.EnsureVisible(index: newIndex);
		}
	}

	/// <summary>Updates the column header texts with ascending or descending sort indicators.</summary>
	/// <remarks>Removes any previous sort arrow symbols from non-sorted columns.</remarks>
	private void UpdateColumnHeaders()
	{
		for (int i = 0; i < listView.Columns.Count; i++)
		{
			string headerText = listView.Columns[index: i].Text.TrimStart('▲', '▼', ' ');
			if (i == sortColumn && sortOrder != SortOrder.None)
			{
				string indicator = sortOrder == SortOrder.Ascending ? "▲" : "▼";
				listView.Columns[index: i].Text = $"{indicator} {headerText}";
			}
			else
			{
				listView.Columns[index: i].Text = headerText;
			}
		}
	}

	/// <summary>Sorts the virtualized tree list while preserving state hierarchy and expansion states.</summary>
	/// <remarks>Sorting column 0 sorts the state nodes; sorting columns 1 to 3 sorts the cities within each state node.</remarks>
	private void SortVirtualList()
	{
		if (visibleEntries.Count == 0 || sortColumn < 0 || sortOrder == SortOrder.None)
		{
			return;
		}
		CityListEntry? previouslySelected = GetSelectedEntry();
		int direction = sortOrder == SortOrder.Descending ? -1 : 1;
		if (sortColumn == 0)
		{
			stateGroups.Sort(comparison: (a, b) => direction * StringComparer.OrdinalIgnoreCase.Compare(x: a.State, y: b.State));
		}
		else
		{
			foreach (StateGroupNode group in stateGroups)
			{
				group.Cities.Sort(comparison: (a, b) =>
				{
					ArgumentNullException.ThrowIfNull(argument: a);
					ArgumentNullException.ThrowIfNull(argument: b);
					int result = sortColumn switch
					{
						1 => StringComparer.OrdinalIgnoreCase.Compare(x: a.City, y: b.City),
						2 => a.Latitude.CompareTo(value: b.Latitude),
						3 => a.Longitude.CompareTo(value: b.Longitude),
						_ => 0
					};
					return direction * result;
				});
			}
		}
		RebuildVisibleEntries();
		listView.VirtualListSize = visibleEntries.Count;
		RestoreSelection(previouslySelected: previouslySelected);
		listView.Invalidate();
		logger.Info(message: $"Sorted virtual cities tree list by column {sortColumn} ({sortOrder}).");
	}

	/// <summary>Commits the coordinates of the currently selected city and closes the dialog with <see cref="DialogResult.OK"/>.</summary>
	/// <remarks>Ignores the action if no item is selected or if the selected item is a collapsible state node.</remarks>
	private void CommitSelectedCoordinates()
	{
		CityListEntry? selectedEntry = GetSelectedEntry();
		if (selectedEntry == null || selectedEntry.IsStateNode)
		{
			logger.Warn(message: "CommitSelectedCoordinates invoked without a valid city selection.");
			return;
		}
		SelectedState = selectedEntry.State;
		SelectedCity = selectedEntry.City;
		SelectedLatitude = selectedEntry.Latitude;
		SelectedLongitude = selectedEntry.Longitude;
		logger.Info(message: $"Committed coordinates for {SelectedCity} ({SelectedState}): Lat={SelectedLatitude.Value.ToString(provider: CultureInfo.InvariantCulture)}, Lon={SelectedLongitude.Value.ToString(provider: CultureInfo.InvariantCulture)}.");
		DialogResult = DialogResult.OK;
		Close();
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the <see cref="Form.Load"/> event for <see cref="CitiesForm"/>.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Plays the open sound and loads the cities data into the virtual tree list view.</remarks>
	private void CitiesForm_Load(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		logger.Info(message: "CitiesForm is loading.");
		AudioPlayback.PlayOpenSound();
		LoadCitiesData();
	}

	/// <summary>Handles the <see cref="Form.FormClosing"/> event for <see cref="CitiesForm"/>.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The form-closing event data.</param>
	/// <remarks>Plays the close sound when the form closes.</remarks>
	private void CitiesForm_FormClosing(object? sender, FormClosingEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		logger.Info(message: "CitiesForm is closing.");
		AudioPlayback.PlayCloseSound();
	}

	/// <summary>Processes command keys such as <see cref="Keys.Escape"/> to cancel the dialog.</summary>
	/// <param name="msg">A <see cref="Message"/> that represents the window message to process.</param>
	/// <param name="keyData">One of the <see cref="Keys"/> values that represents the key to process.</param>
	/// <returns><see langword="true"/> if the keystroke was processed by the control; otherwise <see langword="false"/>.</returns>
	/// <remarks>Provides keyboard cancellation because <see cref="Form.ControlBox"/> is disabled.</remarks>
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			DialogResult = DialogResult.Cancel;
			Close();
			return true;
		}
		return base.ProcessCmdKey(msg: ref msg, keyData: keyData);
	}

	#endregion

	#region ListView event handlers

	/// <summary>Handles the <see cref="ListView.RetrieveVirtualItem"/> event to supply collapsible state nodes and detailed city rows.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The virtual item event data containing the requested item index.</param>
	/// <remarks>Renders state nodes with expand/collapse glyphs and city count, and renders child city entries with indented city names and right-aligned coordinates.</remarks>
	private void ListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		if (e.ItemIndex < 0 || e.ItemIndex >= visibleEntries.Count)
		{
			logger.Error(message: $"Virtual item index {e.ItemIndex} is out of bounds (Count={visibleEntries.Count}).");
			ListViewItem errorItem = new(text: "Error");
			_ = errorItem.SubItems.Add(text: "Invalid index");
			_ = errorItem.SubItems.Add(text: string.Empty);
			_ = errorItem.SubItems.Add(text: string.Empty);
			e.Item = errorItem;
			return;
		}
		CityListEntry entry = visibleEntries[index: e.ItemIndex];
		if (entry.IsStateNode && entry.ParentGroup != null)
		{
			string glyph = entry.ParentGroup.IsExpanded ? expandedGlyph : collapsedGlyph;
			int cityCount = entry.ParentGroup.Cities.Count;
			ListViewItem stateItem = new(text: $"{glyph} {entry.State} ({cityCount})")
			{
				Font = stateNodeFont ?? listView.Font,
				BackColor = SystemColors.ControlLight,
				ForeColor = SystemColors.ControlText,
				ToolTipText = $"{entry.State} — {cityCount} {(cityCount == 1 ? "city" : "cities")} (double-click or press Enter to {(entry.ParentGroup.IsExpanded ? "collapse" : "expand")})"
			};
			_ = stateItem.SubItems.Add(text: string.Empty);
			_ = stateItem.SubItems.Add(text: string.Empty);
			_ = stateItem.SubItems.Add(text: string.Empty);
			e.Item = stateItem;
		}
		else
		{
			string latText = entry.Latitude.ToString(format: "F4", provider: CultureInfo.InvariantCulture);
			string lonText = entry.Longitude.ToString(format: "F4", provider: CultureInfo.InvariantCulture);
			ListViewItem cityItem = new(text: string.Empty)
			{
				ToolTipText = $"{entry.City}, {entry.State} ({latText}, {lonText})"
			};
			_ = cityItem.SubItems.Add(text: $"   {entry.City}");
			_ = cityItem.SubItems.Add(text: latText);
			_ = cityItem.SubItems.Add(text: lonText);
			e.Item = cityItem;
		}
	}

	/// <summary>Handles the <see cref="ListView.SelectedIndexChanged"/> event to update the status bar and commit button state.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Enables <see cref="buttonCommitCoordinates"/> only when a concrete city row is selected.</remarks>
	private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		CityListEntry? selectedEntry = GetSelectedEntry();
		textToCopyStateName = textToCopyCityName = textToCopyFormattedOverview = textToCopyLatitudeValue = textToCopyLongitudeValue = string.Empty;
		if (selectedEntry == null)
		{
			buttonCommitCoordinates.Enabled = false;
			SetStatusBar(label: labelInformation, text: string.Empty);
			return;
		}
		if (selectedEntry.IsStateNode && selectedEntry.ParentGroup != null)
		{
			buttonCommitCoordinates.Enabled = false;
			int cityCount = selectedEntry.ParentGroup.Cities.Count;
			SetStatusBar(label: labelInformation, text: $"{selectedEntry.State} ({cityCount} {(cityCount == 1 ? "city" : "cities")})");
			return;
		}
		buttonCommitCoordinates.Enabled = true;
		string latText = selectedEntry.Latitude.ToString(format: "F4", provider: CultureInfo.InvariantCulture);
		string lonText = selectedEntry.Longitude.ToString(format: "F4", provider: CultureInfo.InvariantCulture);
		textToCopyStateName = selectedEntry.State;
		textToCopyCityName = selectedEntry.City;
		textToCopyLatitudeValue = latText;
		textToCopyLongitudeValue = lonText;
		textToCopyFormattedOverview = $"{selectedEntry.State}\t{selectedEntry.City}\t{latText}\t{lonText}";
		SetStatusBar(label: labelInformation, text: $"{selectedEntry.City} ({selectedEntry.State}) — {latText}, {lonText}");
	}

	/// <summary>Handles the <see cref="ListView.ColumnClick"/> event to toggle column sort order.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The column click event data.</param>
	/// <remarks>Sorts state nodes when column 0 is clicked, or cities within each state node when columns 1 to 3 are clicked.</remarks>
	private void ListView_ColumnClick(object? sender, ColumnClickEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		if (listView.VirtualListSize == 0)
		{
			return;
		}
		sortOrder = (e.Column == sortColumn && sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
		sortColumn = e.Column;
		UpdateColumnHeaders();
		SortVirtualList();
	}

	/// <summary>Handles the <see cref="Control.DoubleClick"/> event on <see cref="listView"/> to expand/collapse a state node or commit a selected city.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Toggles expansion when a state node is double-clicked; commits coordinates when a city row is double-clicked.</remarks>
	private void ListView_DoubleClick(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		CityListEntry? selectedEntry = GetSelectedEntry();
		if (selectedEntry == null)
		{
			return;
		}
		if (selectedEntry.IsStateNode && selectedEntry.ParentGroup != null)
		{
			SetGroupExpanded(groupNode: selectedEntry.ParentGroup);
			return;
		}
		CommitSelectedCoordinates();
	}

	/// <summary>Handles the <see cref="Control.KeyDown"/> event on <see cref="listView"/> to navigate the tree structure or commit the selected city.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The key event data.</param>
	/// <remarks>Supports <see cref="Keys.Right"/> to expand a state node, <see cref="Keys.Left"/> to collapse a state node or return to the parent state, and <see cref="Keys.Enter"/> to toggle state nodes or commit city coordinates.</remarks>
	private void ListView_KeyDown(object? sender, KeyEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		CityListEntry? selectedEntry = GetSelectedEntry();
		if (selectedEntry == null)
		{
			return;
		}
		if (selectedEntry.IsStateNode && selectedEntry.ParentGroup != null)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					e.Handled = true;
					e.SuppressKeyPress = true;
					SetGroupExpanded(groupNode: selectedEntry.ParentGroup);
					break;
				case Keys.Right when !selectedEntry.ParentGroup.IsExpanded:
					e.Handled = true;
					e.SuppressKeyPress = true;
					SetGroupExpanded(groupNode: selectedEntry.ParentGroup, expand: true);
					break;
				case Keys.Left when selectedEntry.ParentGroup.IsExpanded:
					e.Handled = true;
					e.SuppressKeyPress = true;
					SetGroupExpanded(groupNode: selectedEntry.ParentGroup, expand: false);
					break;
				default:
					break;
			}
			return;
		}
		if (e.KeyCode == Keys.Enter)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			CommitSelectedCoordinates();
		}
		else if (e.KeyCode == Keys.Left && selectedEntry.ParentGroup != null)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			SelectStateNode(groupNode: selectedEntry.ParentGroup);
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for <see cref="buttonCommitCoordinates"/>.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Commits the selected city's latitude and longitude and closes the dialog with <see cref="DialogResult.OK"/>.</remarks>
	private void ButtonCommitCoordinates_Click(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		CommitSelectedCoordinates();
	}

	/// <summary>Handles the click event for <see cref="menuItemShowInformationLabel"/> to toggle status bar label visibility.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Synchronizes <see cref="ToolStripItem.Visible"/> of <see cref="labelInformation"/> with the checked state of the menu item.</remarks>
	private void MenuItemShowInformationLabel_Click(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		labelInformation.Visible = menuItemShowInformationLabel.Checked;
		logger.Info(message: $"Set visibility of labelInformation to {labelInformation.Visible}.");
	}

	/// <summary>Handles the click event for <see cref="buttonExpandOrCollapseCitiesList"/> to expand or collapse all state nodes.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Uses <see cref="ToolStripButton.Checked"/> (toggled automatically via <see cref="ToolStripButton.CheckOnClick"/>) to expand or collapse all <see cref="StateGroupNode"/> entries and updates the button appearance.</remarks>
	private void ButtonExpandOrCollapseCitiesList_Click(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		if (stateGroups.Count == 0)
		{
			buttonExpandOrCollapseCitiesList.Checked = false;
			return;
		}
		bool expandAll = buttonExpandOrCollapseCitiesList.Checked;
		logger.Info(message: $"{(expandAll ? "Expanding" : "Collapsing")} all {stateGroups.Count} state nodes.");
		CityListEntry? previouslySelected = GetSelectedEntry();
		foreach (StateGroupNode group in stateGroups)
		{
			group.IsExpanded = expandAll;
		}
		buttonExpandOrCollapseCitiesList.Text = expandAll ? "C&ollapse all" : "E&xpand all";
		buttonExpandOrCollapseCitiesList.Image = expandAll ? FatcowIcons16px.fatcow_bullet_toggle_minus_16px : FatcowIcons16px.fatcow_bullet_toggle_plus_16px;
		try
		{
			listView.BeginUpdate();
			RebuildVisibleEntries();
			listView.VirtualListSize = visibleEntries.Count;
			if (previouslySelected != null)
			{
				if (!expandAll && !previouslySelected.IsStateNode && previouslySelected.ParentGroup != null)
				{
					SelectStateNode(groupNode: previouslySelected.ParentGroup);
				}
				else
				{
					RestoreSelection(previouslySelected: previouslySelected);
				}
			}
			listView.Invalidate();
		}
		finally
		{
			listView.EndUpdate();
		}
	}

	/// <summary>Closes the form without returning coordinates.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Sets <see cref="Form.DialogResult"/> to <see cref="DialogResult.Cancel"/>.</remarks>
	private void ButtonCancel_Click(object? sender, EventArgs e)
	{
		// Log that the Cancel button was clicked and the form will close without returning coordinates
		logger.Info(message: "Cancel button clicked. Closing form without returning coordinates.");
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Set the dialog result to Cancel to indicate that no coordinates have been selected
		DialogResult = DialogResult.Cancel;
		Close();
	}

	/// <summary>Copies the selected state name to the clipboard.</summary>
	/// <remarks>Displays a warning when the state name is null or empty.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemCopyStateName_Click(object sender, EventArgs e)
	{
		logger.Info(message: $"Copying the state name \"{textToCopyStateName}\" to the clipboard...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (string.IsNullOrEmpty(value: textToCopyStateName))
		{
			string message = "The state name cannot be copied because it is empty or null.";
			logger.Warn(message: message);
			ShowWarnMessage(caption: I18nStrings.ErrorCaption, message: message);
			return;
		}
		CopyToClipboard(text: textToCopyStateName);
		logger.Info(message: $"The state name \"{textToCopyStateName}\" was copied.");
	}

	/// <summary>Copies the selected city name to the clipboard.</summary>
	/// <remarks>Displays a warning when the city name is null or empty.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemCopyCityName_Click(object sender, EventArgs e)
	{
		logger.Info(message: $"Copying the city name \"{textToCopyCityName}\" to the clipboard...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (string.IsNullOrEmpty(value: textToCopyCityName))
		{
			string message = "The city name cannot be copied because it is empty or null.";
			logger.Warn(message: message);
			ShowWarnMessage(caption: I18nStrings.ErrorCaption, message: message);
			return;
		}
		CopyToClipboard(text: textToCopyCityName);
		logger.Info(message: $"The state name \"{textToCopyCityName}\" was copied.");
	}

	/// <summary>Copies the selected latitude value to the clipboard.</summary>
	/// <remarks>Displays a warning when the latitude value is null or empty.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemCopyLatitudeValue_Click(object sender, EventArgs e)
	{
		logger.Info(message: $"Copying the latitude value \"{textToCopyLatitudeValue}\" to the clipboard...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (string.IsNullOrEmpty(value: textToCopyLatitudeValue))
		{
			string message = "The latitude value cannot be copied because it is empty or null.";
			logger.Warn(message: message);
			ShowWarnMessage(caption: I18nStrings.ErrorCaption, message: message);
			return;
		}
		CopyToClipboard(text: textToCopyLatitudeValue);
		logger.Info(message: $"The latitude value \"{textToCopyLatitudeValue}\" was copied.");
	}

	/// <summary>Copies the selected longitude value to the clipboard.</summary>
	/// <remarks>Displays a warning when the longitude value is null or empty.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemCopyLongitudeValue_Click(object sender, EventArgs e)
	{
		logger.Info(message: $"Copying the longitude value \"{textToCopyLongitudeValue}\" to the clipboard...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (string.IsNullOrEmpty(value: textToCopyLongitudeValue))
		{
			string message = "The longitude value cannot be copied because it is empty or null.";
			logger.Warn(message: message);
			ShowWarnMessage(caption: I18nStrings.ErrorCaption, message: message);
			return;
		}
		CopyToClipboard(text: textToCopyLongitudeValue);
		logger.Info(message: $"The longitude value \"{textToCopyLongitudeValue}\" was copied.");
	}

	/// <summary>Copies the selected and formatted overview to the clipboard.</summary>
	/// <remarks>Displays a warning when the formatted overview is null or empty.</remarks>
	/// <param name="sender">The object that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void MenuItemCopyValueFormattedOverview_Click(object sender, EventArgs e)
	{
		logger.Info(message: $"Copying the formatted overview \"{textToCopyFormattedOverview}\" to the clipboard...");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		if (string.IsNullOrEmpty(value: textToCopyFormattedOverview))
		{
			string message = "The formatted overview cannot be copied because it is empty or null.";
			logger.Warn(message: message);
			ShowWarnMessage(caption: I18nStrings.ErrorCaption, message: message);
			return;
		}
		CopyToClipboard(text: textToCopyFormattedOverview);
		logger.Info(message: $"The formatted overview \"{textToCopyFormattedOverview}\" was copied.");
	}

	#endregion
}
