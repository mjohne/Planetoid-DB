/*
 * File:        PlanetoidDbForm.EventHandlers.Timer.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing timer event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all timer event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the tick event for checking new MPCORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new MPCORB data file.</remarks>
	private void TimerCheckForNewMpcorbDatFile_Tick(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Call the PlanetoidDBForm_Shown method to check for new MPCORB data file
		PlanetoidDBForm_Shown(sender: sender, e: e);
	}

	/// <summary>Handles the tick event for checking new ASTORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new ASTORB data file.</remarks>
	private void TimerCheckForNewAstorbDatFile_Tick(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Call the PlanetoidDBForm_Shown method to check for new ASTORB data file
		PlanetoidDBForm_Shown(sender: sender, e: e);
	}

	/// <summary>Handles the tick event for showing the memory usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the memory usage.</remarks>
	private void TimerMemoryUsage_Tick(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		//DiskSizeUsage();
		toolStripStatusLabelMemoryUsage.Text = $"{Process.GetCurrentProcess().WorkingSet64 / (1024.0 * 1024.0):F2} MB";
	}

	/// <summary>Handles the tick event for showing the time usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the time usage.</remarks>
	private void TimerTimeUsage_Tick(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		TimeSpan pastTime = stopWatch.Elapsed;
		toolStripStatusLabelTimeUsage.Text = $"{pastTime.Hours:00}:{pastTime.Minutes:00}:{pastTime.Seconds:00}.{pastTime.Milliseconds:000}";
	}

	/// <summary>Handles the tick event for showing the processor usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the time usage.</remarks>
	private async void TimerProcessorUsage_TickAsync(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		DateTime startTime = DateTime.UtcNow;
		TimeSpan startCpuTime = currentProcess.TotalProcessorTime;
		await Task.Delay(millisecondsDelay: timerProcessorUsage.Interval).ConfigureAwait(continueOnCapturedContext: true);
		// Updates the process data
		currentProcess.Refresh();
		DateTime endTime = DateTime.UtcNow;
		TimeSpan endCpuTime = currentProcess.TotalProcessorTime;
		TimeSpan passedTime = endTime - startTime;
		TimeSpan totalCpuTimeUsed = endCpuTime - startCpuTime;
		// Calculate processor usage as a percentage (relative to all cores)
		double cpuUsagePercentage = totalCpuTimeUsed.TotalMilliseconds / (passedTime.TotalMilliseconds * processorCount) * 100;
		toolStripStatusLabelProcessorUsage.Text = $"{cpuUsagePercentage:F2} %";
	}

	/// <summary>Updates the current planetoid position when the selection changes.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	private void TimerSelectionDebounce_Tick(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);

		// Stop the timer so it only runs once per selection action
		timerSelectionDebounce.Stop();

		// Verify that a row is selected in VirtualMode
		if (listViewReadableDesignations.SelectedIndices.Count == 0)
		{
			return;
		}

		if (listViewReadableDesignations.SelectedIndices.Count > 0)
		{
			int selectedIndex = listViewReadableDesignations.SelectedIndices[index: 0];
			if (selectedIndex >= 0 && selectedIndex < planetoidsDatabase.Count && selectedIndex != currentMpcorbPosition)
			{
				logger.Info(message: $"ListView selection settled. Navigating to position: {selectedIndex + 1}");

				// Retrieve the entry from the underlying data source
				string entryStr = planetoidsDatabase[index: selectedIndex];
				string planetoidReadableDesignation = entryStr.Length >= mpcorbDatReadableDesignationDelimiterBegin ? entryStr[mpcorbDatReadableDesignationDelimiterBegin..mpcorbDatReadableDesignationDelimiterEnd].Trim() : string.Empty;
				labelInformation.Text = $"Selected item: {selectedIndex + 1} - {planetoidReadableDesignation}";


				// Push current position to navigation history before changing position
				PushNavigationHistory(previousPosition: currentMpcorbPosition);

				// Synchronize positions across all loaded databases
				currentMpcorbPosition = selectedIndex;
				GotoCurrentMpcorbPosition(position: currentMpcorbPosition);

				currentAstorbPosition = currentMpcorbPosition;
				GotoCurrentAstorbPosition(position: currentAstorbPosition);

				currentMpcorbJsonPosition = currentMpcorbPosition;
				GotoCurrentMpcorbJsonPosition(position: currentMpcorbJsonPosition);

				currentAllnumCatPosition = currentMpcorbPosition;
				GotoCurrentAllnumCatPosition(position: currentAllnumCatPosition);

				currentSingoppCatPosition = currentMpcorbPosition;
				GotoCurrentSingoppCatPosition(position: currentSingoppCatPosition);

				currentUfitobsCatPosition = currentMpcorbPosition;
				GotoCurrentUfitobsCatPosition(position: currentUfitobsCatPosition);
			}
		}
	}
}
