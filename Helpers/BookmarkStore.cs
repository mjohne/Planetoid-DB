/*
 * File:        BookmarkStore.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Manages loading and saving of bookmark entries per database file.
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

using System.Text.Json;

namespace Planetoid_DB.Helpers;

/// <summary>Manages loading and saving of bookmark entries per database file.</summary>
/// <remarks>Each database file has its own bookmark JSON file stored in the user's application-data directory under the "Planetoid-DB" sub-folder.</remarks>
public sealed class BookmarkStore
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>Used for logging errors during load/save operations.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>JSON serialisation options shared across all calls.</summary>
	/// <remarks>Configured to produce indented JSON for readability.</remarks>
	private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

	/// <summary>Directory that holds all bookmark files.</summary>			   
	/// <remarks>Located in the user's application-data directory under the "Planetoid-DB\Bookmarks" sub-folder.</remarks>
	private static readonly string bookmarkDirectory = Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ApplicationData), path2: "Planetoid-DB", path3: "Bookmarks");

	/// <summary>Returns the full path of the bookmark file for the given database filename.</summary>
	/// <param name="databaseFilename">The base filename of the database (e.g. "mpcorb.dat").</param>
	/// <returns>The full path to the corresponding bookmark JSON file.</returns>
	/// <exception cref="ArgumentException"><paramref name="databaseFilename"/> is <see langword="null"/>, whitespace, or does not contain a valid file name.</exception>
	/// <remarks>The bookmark file is named "bookmarks_{lowercasedBaseFileName}.json" and is located in the user's application-data directory under the "Planetoid-DB\Bookmarks" sub-folder.</remarks>
	public static string GetBookmarkFilePath(string databaseFilename)
	{
		// Validate the input database filename
		if (string.IsNullOrWhiteSpace(value: databaseFilename))
		{
			throw new ArgumentException(message: "Database filename must not be null or whitespace.", paramName: nameof(databaseFilename));
		}
		// Sanitize the database filename to ensure it is a valid file name
		string sanitised = Path.GetFileName(path: databaseFilename).ToLowerInvariant();
		// Check if the sanitized filename is still valid
		if (string.IsNullOrWhiteSpace(value: sanitised))
		{
			throw new ArgumentException(message: "Database filename must contain a valid file name.", paramName: nameof(databaseFilename));
		}
		// Construct and return the full path to the bookmark file
		return Path.Combine(path1: bookmarkDirectory, path2: $"bookmarks_{sanitised}.json");
	}

	/// <summary>Loads all bookmarks for the given database filename from disk.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	/// <returns>A list of <see cref="BookmarkEntry"/> objects, or an empty list if none are found or an error occurs.</returns>
	public static List<BookmarkEntry> Load(string databaseFilename)
	{
		// Initialize the path variable to null for error handling
		string? path = null;
		// Attempt to load the bookmarks from the corresponding JSON file
		try
		{
			// Get the full path to the bookmark file for the specified database filename
			path = GetBookmarkFilePath(databaseFilename: databaseFilename);
			// Check if the bookmark file exists; if not, log the information and return an empty list
			if (!File.Exists(path: path))
			{
				logger.Info(message: $"No bookmark file found for '{databaseFilename}' at '{path}'. Returning empty list.");
				return [];
			}
			// Read the JSON content from the bookmark file
			string json = File.ReadAllText(path: path);
			// Log the successful loading of bookmarks
			logger.Info(message: $"Loaded bookmarks from '{path}' for database '{databaseFilename}'.");
			// Deserialize the JSON content into a list of BookmarkEntry objects; return an empty list if deserialization fails
			return JsonSerializer.Deserialize<List<BookmarkEntry>>(json: json) ?? [];
		}
		// Catch any exceptions that occur during the loading process, log the error, and return an empty list
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Failed to load bookmarks from '{path ?? databaseFilename}': {ex.Message}");
			return [];
		}
	}

	/// <summary>Saves the given list of bookmarks for the specified database filename to disk.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	/// <param name="entries">The list of bookmark entries to persist.</param>
	/// <remarks>If the bookmark file does not exist, it will be created. If it exists, it will be overwritten.</remarks>
	public static void Save(string databaseFilename, IEnumerable<BookmarkEntry> entries)
	{
		// Initialize the path variable to null for error handling
		string? path = null;
		// Attempt to save the bookmarks to the corresponding JSON file
		try
		{
			// Get the full path to the bookmark file for the specified database filename
			path = GetBookmarkFilePath(databaseFilename: databaseFilename);
			// Ensure the bookmark directory exists; create it if it does not
			Directory.CreateDirectory(path: bookmarkDirectory);
			// Materialise the enumerable once to avoid double enumeration during serialisation and count logging
			List<BookmarkEntry> entriesList = entries as List<BookmarkEntry> ?? [.. entries];
			// Serialize the list of bookmark entries to JSON format with indentation for readability
			string json = JsonSerializer.Serialize(value: entriesList, options: serializerOptions);
			// Write the serialized JSON content to the bookmark file, overwriting any existing content
			File.WriteAllText(path: path, contents: json);
			// Log the successful saving of bookmarks, including the count of entries saved
			logger.Info(message: $"Saved {entriesList.Count} bookmarks to '{path}' for database '{databaseFilename}'.");
		}
		// Catch any exceptions that occur during the saving process and log the error
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Failed to save bookmarks to '{path ?? databaseFilename}': {ex.Message}");
		}
	}

	/// <summary>Deletes all bookmarks for the specified database filename.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	/// <remarks>If the bookmark file does not exist, no action is taken.</remarks>
	public static void ClearAll(string databaseFilename)
	{
		// Initialize the path variable to null for error handling
		string? path = null;
		// Attempt to delete the bookmark file corresponding to the specified database filename
		try
		{
			// Get the full path to the bookmark file for the specified database filename
			path = GetBookmarkFilePath(databaseFilename: databaseFilename);
			// Check if the bookmark file exists; if it does, delete it
			if (File.Exists(path: path))
			{
				File.Delete(path: path);
			}
			// Log the successful deletion of bookmarks
			logger.Info(message: $"Cleared all bookmarks for database '{databaseFilename}' by deleting '{path}'.");
		}
		// Catch any exceptions that occur during the deletion process and log the error
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Failed to clear bookmarks from '{path ?? databaseFilename}': {ex.Message}");
		}
	}
}
