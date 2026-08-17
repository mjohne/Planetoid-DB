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
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>JSON serialisation options shared across all calls.</summary>
	private static readonly JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

	/// <summary>Directory that holds all bookmark files.</summary>
	private static readonly string bookmarkDirectory = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		"Planetoid-DB",
		"Bookmarks");

	/// <summary>Returns the full path of the bookmark file for the given database filename.</summary>
	/// <param name="databaseFilename">The base filename of the database (e.g. "mpcorb.dat").</param>
	/// <returns>The full path to the corresponding bookmark JSON file.</returns>
	public static string GetBookmarkFilePath(string databaseFilename)
	{
		string sanitised = Path.GetFileName(databaseFilename).ToLowerInvariant();
		return Path.Combine(bookmarkDirectory, $"bookmarks_{sanitised}.json");
	}

	/// <summary>Loads all bookmarks for the given database filename from disk.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	/// <returns>A list of <see cref="BookmarkEntry"/> objects, or an empty list if none are found or an error occurs.</returns>
	public static List<BookmarkEntry> Load(string databaseFilename)
	{
		string path = GetBookmarkFilePath(databaseFilename);
		if (!File.Exists(path))
		{
			return [];
		}
		try
		{
			string json = File.ReadAllText(path);
			return JsonSerializer.Deserialize<List<BookmarkEntry>>(json) ?? [];
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Failed to load bookmarks from '{path}': {ex.Message}");
			return [];
		}
	}

	/// <summary>Saves the given list of bookmarks for the specified database filename to disk.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	/// <param name="entries">The list of bookmark entries to persist.</param>
	public static void Save(string databaseFilename, IEnumerable<BookmarkEntry> entries)
	{
		string path = GetBookmarkFilePath(databaseFilename);
		try
		{
			Directory.CreateDirectory(bookmarkDirectory);
			string json = JsonSerializer.Serialize(entries, serializerOptions);
			File.WriteAllText(path, json);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Failed to save bookmarks to '{path}': {ex.Message}");
		}
	}

	/// <summary>Deletes all bookmarks for the specified database filename.</summary>
	/// <param name="databaseFilename">The base filename of the database.</param>
	public static void ClearAll(string databaseFilename)
	{
		Save(databaseFilename, []);
	}
}
