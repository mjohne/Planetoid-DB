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
		// Call the PlanetoidDBForm_Shown method to check for new MPCORB data file
		PlanetoidDBForm_Shown(sender: sender, e: e);
	}

	/// <summary>Handles the tick event for checking new ASTORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new ASTORB data file.</remarks>
	private void TimerCheckForNewAstorbDatFile_Tick(object sender, EventArgs e)
	{
		// Call the PlanetoidDBForm_Shown method to check for new ASTORB data file
		PlanetoidDBForm_Shown(sender: sender, e: e);
	}

	/// <summary>Handles the tick event for showing the memory usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the memory usage.</remarks>
	private void TimerMemoryUsage_Tick(object sender, EventArgs e)
	{
		//DiskSizeUsage();
		toolStripStatusLabelMemoryUsage.Text = $"{Process.GetCurrentProcess().WorkingSet64 / (1024.0 * 1024.0):F2} MB";
	}

	/// <summary>Handles the tick event for showing the time usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the time usage.</remarks>
	private void TimerTimeUsage_Tick(object sender, EventArgs e)
	{
		TimeSpan pastTime = stopWatch.Elapsed;
		toolStripStatusLabelTimeUsage.Text = $"{pastTime.Hours:00}:{pastTime.Minutes:00}:{pastTime.Seconds:00}.{pastTime.Milliseconds:000}";
	}

	/// <summary>Handles the tick event for showing the processor usage.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the time usage.</remarks>
	private async void TimerProcessorUsage_TickAsync(object sender, EventArgs e)
	{
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
}
