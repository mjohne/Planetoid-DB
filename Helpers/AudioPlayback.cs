/*
 * File:        AudioPlayback.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Plays embedded audio resources in the background.
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

using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Media;
using System.Reflection;
namespace Planetoid_DB.Helpers;

/// <summary>Plays embedded audio resources in the background.</summary>
/// <remarks>This class is internal and sealed, and it uses NLog for logging. It plays WAV files asynchronously without blocking the UI thread.</remarks>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class AudioPlayback
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Returns a string representation of the current object for debugging purposes.</summary>
	/// <remarks>This property is used by the DebuggerDisplay attribute to show a meaningful representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;

	/// <summary>Plays an embedded WAV resource asynchronously without blocking the UI thread.</summary>
	/// <param name="resourceName">The fully qualified manifest resource name of the WAV file.</param>
	/// <remarks>This method uses a background task to load and play the sound, logging any errors that occur during playback.</remarks>
	private static void PlaySoundInBackground(MemoryStream resourceName)
	{
		// Check if the user has enabled UI sound playback in the application settings.
		if (Settings.Default.userEnableUiSound)
		{
			// Log the request to play the sound resource.
			logger.Info(message: $"Request to play sound resource '{resourceName}' received.");
			// Validate the resource name and throw an exception if it is null or whitespace.
			if (resourceName is null)
			{
				// Log an error message indicating that the resource name is invalid.
				logger.Error(message: "The provided resource name is null.");
				// Throw an ArgumentException to indicate that the resource name is invalid.
				ArgumentNullException.ThrowIfNull(argument: resourceName);
			}
			// Start a background task to play the sound resource.
			_ = Task.Run(() =>
			{
				// Log that the background task for sound playback has started.
				logger.Info(message: "Background task for sound playback started.");
				// Attempt to load and play the sound resource, catching any exceptions that occur.
				try
				{
					// Get the assembly that contains the PlanetoidDbForm type, which is used to locate the embedded resource.
					Assembly assembly = typeof(PlanetoidDbForm).Assembly;
					// Attempt to get the embedded resource stream for the specified resource name.
					using Stream? resourceStream = resourceName;
					// Check if the resource stream is null, indicating that the resource was not found.
					if (resourceStream is null)
					{
						// Log a warning message indicating that the sound resource was not found.
						logger.Warn(message: $"Sound resource '{resourceName}' was not found.");
						// Exit the method early since the resource could not be found.
						return;
					}
					// Create a new MemoryStream to hold the sound data.
					using MemoryStream soundStream = new();
					// Copy the contents of the resource stream to the sound stream.
					resourceStream.CopyTo(destination: soundStream);
					// Check if the sound stream is empty, indicating that the resource was empty.
					if (soundStream.Length == 0)
					{
						// Log a warning message indicating that the sound resource was empty.
						logger.Warn(message: $"Sound resource '{resourceName}' was empty.");
						// Exit the method early since there is no sound data to play.
						return;
					}
					// Reset the position of the sound stream to the beginning so that it can be played from the start.
					soundStream.Position = 0;
					// Create a new SoundPlayer instance using the sound stream.
					using SoundPlayer soundPlayer = new(stream: soundStream);
					// Load the sound data into the SoundPlayer and play it synchronously.
					soundPlayer.Load();
					soundPlayer.PlaySync();
					// Log a message indicating that the sound resource was played successfully.
					logger.Info(message: $"Sound resource '{resourceName}' was played successfully.");
				}
				// Catch any exceptions that occur during the loading or playback of the sound resource.
				catch (Exception ex)
				{
					// Log a warning message indicating that the sound resource could not be played, along with the exception details.
					logger.Warn(exception: ex, message: $"Failed to play sound resource '{resourceName}'.");
				}
			});
		}
	}

	/// <summary>Plays the start sound resource asynchronously without blocking the UI thread.</summary>
	/// <remarks>This method logs the request to play the start sound and calls the PlaySoundInBackground method with the appropriate resource name.</remarks>
	public static void PlayStartSound()
	{
		// Log the request to play the start sound resource.
		logger.Info(message: "Request to play start sound received.");
		// Play the start sound resource in the background.
		PlaySoundInBackground(resourceName: Properties.Resources.ui_start);
	}

	/// <summary>Plays the send data sound resource asynchronously without blocking the UI thread.</summary>
	/// <remarks>This method logs the request to play the send data sound and calls the PlaySoundInBackground method with the appropriate resource name.</remarks>
	public static void PlaySendDataSound()
	{
		// Log the request to play the send data sound resource.
		logger.Info(message: "Request to play send data sound received.");
		// Play the send data sound resource in the background.
		PlaySoundInBackground(resourceName: Properties.Resources.ui_senddata);
	}

	/// <summary>Plays the shutdown sound resource asynchronously without blocking the UI thread.</summary>
	/// <remarks>This method logs the request to play the shutdown sound and calls the PlaySoundInBackground method with the appropriate resource name.</remarks>
	public static void PlayShutDownSound()
	{
		// Log the request to play the shutdown sound resource.
		logger.Info(message: "Request to play shutdown sound received.");
		// Play the shutdown sound resource in the background.
		PlaySoundInBackground(resourceName: Properties.Resources.ui_shutdown);
	}

	/// <summary>Plays the open sound resource asynchronously without blocking the UI thread.</summary>
	/// <remarks>This method logs the request to play the open sound and calls the PlaySoundInBackground method with the appropriate resource name.</remarks>
	public static void PlayOpenSound()
	{
		// Log the request to play the open sound resource.
		logger.Info(message: "Request to play open sound received.");
		// Play the open sound resource in the background.
		PlaySoundInBackground(resourceName: Properties.Resources.ui_open);
	}

	/// <summary>Plays the close sound resource asynchronously without blocking the UI thread.</summary>
	/// <remarks>This method logs the request to play the close sound and calls the PlaySoundInBackground method with the appropriate resource name.</remarks>
	public static void PlayCloseSound()
	{
		// Log the request to play the close sound resource.
		logger.Info(message: "Request to play close sound received.");
		// Play the close sound resource in the background.
		PlaySoundInBackground(resourceName: Properties.Resources.ui_close);
	}

}
