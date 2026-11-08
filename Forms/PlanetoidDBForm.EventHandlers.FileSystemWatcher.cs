/*
 * File:        PlanetoidDbForm.FileSystemWatcher.File.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing FileSystemWatcher event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all FileSystemWatcher event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Updates disk size usage when a file system change occurs.</summary>
	/// <param name="source">The source of the event.</param>
	/// <param name="e">The file system event details.</param>
	private void OnChanged(object source, FileSystemEventArgs e)
	{
		DiskSizeUsage();
	}

	/// <summary>Updates disk size usage after a file or directory is renamed.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data for the rename.</param>
	private void OnChanged(object sender, RenamedEventArgs e)
	{
		DiskSizeUsage();
	}
}
