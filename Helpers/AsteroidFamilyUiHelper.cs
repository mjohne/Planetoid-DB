/*
 * File:        AsteroidFamilyUiHelper.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Provides UI helper methods for displaying identified asteroid family clusters in WinForms ListView controls.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Globalization;

namespace Planetoid_DB.Helpers;

/// <summary>Provides UI helper methods for displaying identified asteroid family clusters in WinForms ListView controls.</summary>
internal static class AsteroidFamilyUiHelper
{
	/// <summary>Configures columns and populates a WinForms ListView with members of an identified <see cref="AsteroidFamilyCluster"/>.</summary>
	/// <param name="listView">The target WinForms ListView control.</param>
	/// <param name="cluster">The asteroid family cluster result to display.</param>
	public static void PopulateFamilyClusterListView(ListView listView, AsteroidFamilyCluster cluster)
	{
		ArgumentNullException.ThrowIfNull(argument: listView);
		listView.BeginUpdate();
		try
		{
			listView.Items.Clear();
			// Configure columns if not already initialized
			if (listView.Columns.Count == 0)
			{
				listView.View = View.Details;
				listView.FullRowSelect = true;
				listView.GridLines = true;
				_ = listView.Columns.Add(text: "Bezeichnung", width: 110);
				_ = listView.Columns.Add(text: "a_p (AE)", width: 85);
				_ = listView.Columns.Add(text: "e_p", width: 75);
				_ = listView.Columns.Add(text: "sin(i_p)", width: 75);
				_ = listView.Columns.Add(text: "i_p (°)", width: 75);
				_ = listView.Columns.Add(text: "g (\"/Jahr)", width: 95);
				_ = listView.Columns.Add(text: "s (\"/Jahr)", width: 95);
				_ = listView.Columns.Add(text: "Rolle", width: 130);
			}
			CultureInfo culture = CultureInfo.InvariantCulture;
			string seedDesignation = cluster.SeedElement.Designation;
			foreach (ProperOrbitalElement member in cluster.Members)
			{
				bool isSeed = string.Equals(a: member.Designation, b: seedDesignation, comparisonType: StringComparison.OrdinalIgnoreCase);
				ListViewItem item = new(text: member.Designation)
				{
					UseItemStyleForSubItems = true
				};
				_ = item.SubItems.Add(text: member.ProperSemiMajorAxisAu.ToString(format: "F4", provider: culture));
				_ = item.SubItems.Add(text: member.ProperEccentricity.ToString(format: "F4", provider: culture));
				_ = item.SubItems.Add(text: member.ProperSinInclination.ToString(format: "F4", provider: culture));
				_ = item.SubItems.Add(text: member.ProperInclinationDegrees.ToString(format: "F2", provider: culture));
				_ = item.SubItems.Add(text: member.PerihelionFrequencyG.ToString(format: "F3", provider: culture));
				_ = item.SubItems.Add(text: member.NodalFrequencyS.ToString(format: "F3", provider: culture));
				_ = item.SubItems.Add(text: isSeed ? "Mutterkörper (Seed)" : "Familienmitglied");
				// Visual highlight for the seed/parent body of the cluster
				if (isSeed)
				{
					item.BackColor = Color.FromArgb(red: 255, green: 245, blue: 204); // Soft gold/yellow highlight
					item.ForeColor = Color.DarkSlateGray;
					item.Font = new Font(prototype: listView.Font, FontStyle.Bold);
				}
				_ = listView.Items.Add(item);
			}
		}
		finally
		{
			listView.EndUpdate();
		}
	}
}
