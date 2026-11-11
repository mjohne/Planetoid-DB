/*
 * File:        EphemerisVirtualListViewForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Demonstrates high-performance virtual ListView rendering for large ephemeris datasets.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Services.Models;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>
/// Form displaying ephemeris calculations using a virtual WinForms ListView.
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
public partial class EphemerisVirtualListViewForm : Form
{
	private readonly List<EphemerisRow> _ephemerisData = [];
	private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

	public EphemerisVirtualListViewForm()
	{
		InitializeComponent();
		ConfigureVirtualListView();
	}

	/// <summary>
	/// Initializes ListView properties, columns, and wires up virtual events.
	/// </summary>
	private void ConfigureVirtualListView()
	{
		listViewEphemeris.BeginUpdate();
		try
		{
			// Enable VirtualMode
			listViewEphemeris.VirtualMode = true;
			listViewEphemeris.View = View.Details;
			listViewEphemeris.FullRowSelect = true;
			listViewEphemeris.GridLines = true;

			// Define columns
			listViewEphemeris.Columns.Clear();
			listViewEphemeris.Columns.Add(text: "Datum (UT)", width: 100);
			listViewEphemeris.Columns.Add(text: "JD (TT)", width: 110);
			listViewEphemeris.Columns.Add(text: "r (AE)", width: 85);
			listViewEphemeris.Columns.Add(text: "Δ (AE)", width: 85);
			listViewEphemeris.Columns.Add(text: "Phasenwinkel α (°)", width: 110);
			listViewEphemeris.Columns.Add(text: "Elongation ψ (°)", width: 105);
			listViewEphemeris.Columns.Add(text: "Helligkeit (mag)", width: 100);
			listViewEphemeris.Columns.Add(text: "Winkeldurchm. (\")", width: 115);

			// Wire mandatory virtual mode event
			listViewEphemeris.RetrieveVirtualItem += OnListViewEphemerisRetrieveVirtualItem;
		}
		finally
		{
			listViewEphemeris.EndUpdate();
		}
	}

	/// <summary>
	/// Binds an ephemeris dataset to the virtual ListView.
	/// </summary>
	/// <param name="ephemerisRows">The list of calculated ephemeris rows.</param>
	public void SetEphemerisData(IReadOnlyList<EphemerisRow> ephemerisRows)
	{
		ArgumentNullException.ThrowIfNull(argument: ephemerisRows);

		_ephemerisData.Clear();
		_ephemerisData.AddRange(collection: ephemerisRows);

		// Set VirtualListSize to inform the OS control of the total row count
		listViewEphemeris.VirtualListSize = _ephemerisData.Count;
		listViewEphemeris.Invalidate();
	}

	/// <summary>
	/// On-demand event handler that constructs ListViewItems dynamically for visible rows only.
	/// </summary>
	private void OnListViewEphemerisRetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
	{
		// Bounds check
		if (e.ItemIndex < 0 || e.ItemIndex >= _ephemerisData.Count)
		{
			e.Item = new ListViewItem();
			return;
		}

		EphemerisRow row = _ephemerisData[e.ItemIndex];

		// Construct ListViewItem with formatted string representations
		ListViewItem item = new(text: row.Date.ToString(format: "yyyy-MM-dd", provider: _culture));
		item.SubItems.Add(text: row.JulianDate.ToString(format: "F4", provider: _culture));
		item.SubItems.Add(text: row.HeliocentricDistanceAu.ToString(format: "F4", provider: _culture));
		item.SubItems.Add(text: row.GeocentricDistanceAu.ToString(format: "F4", provider: _culture));
		item.SubItems.Add(text: row.PhaseAngleDegrees.ToString(format: "F2", provider: _culture));
		item.SubItems.Add(text: row.ElongationDegrees.ToString(format: "F2", provider: _culture));
		item.SubItems.Add(text: row.ApparentMagnitude.ToString(format: "F1", provider: _culture));
		item.SubItems.Add(text: row.AngularDiameterArcsec.ToString(format: "F2", provider: _culture));

		// Assign created item back to event arguments
		e.Item = item;
	}

	private string DebuggerDisplay => ToString() ?? string.Empty;
}