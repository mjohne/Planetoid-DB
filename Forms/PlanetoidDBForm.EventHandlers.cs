/*
 * File:        PlanetoidDbForm.EventHandlers.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using System.Text;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all event handlers including form lifecycle events, Click handlers, BackgroundWorker events, Timer events, and UI interaction handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>A <see cref="CompositeFormat"/> instance for formatting the bookmark audit summary.</summary>
	/// <remarks>This field is used to format messages for the bookmark audit summary.</remarks>
	private static readonly CompositeFormat bookmarkAuditSummary = CompositeFormat.Parse(format: I18nStrings.BookmarkAuditSummary);

	/// <summary>Indicates whether shutdown sound playback has started.</summary>
	private bool isShutdownSoundPlaybackStarted;

	/// <summary>Indicates whether shutdown sound playback has completed.</summary>
	private bool isShutdownSoundPlaybackComplete;
}
