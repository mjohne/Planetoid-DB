/*
 * File:        PlanetoidDbForm.DatabaseUpdates.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class for handling database updates in the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */


using Planetoid_DB.Properties;

using System.Net;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>Partial class for handling database updates in the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This partial class contains methods for checking for updates to various databases (MPCORB.DAT, MPCORB.JSON, ASTORB.DAT, ALLNUM.CAT, UFITOBS.CAT, SINGOPP.CAT) and displaying the corresponding downloader forms. It also includes a generic method for checking if a remote database file is newer than the local file and a method for showing the downloader workflow.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Checks if an update for the MPCORB.DAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the MPCORB.DAT database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsMpcorbDatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameMpcorbDat, sourceUri: uriMpcorbDat, readContentLength: true);

	/// <summary>Checks if an update for the MPCORB.JSON database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the MPCORB.JSON database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsMpcorbJsonUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameMpcorbJson, sourceUri: uriMpcorbJson, readContentLength: true);

	/// <summary>Checks if an update for the ASTORB.DAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the ASTORB.DAT database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsAstorbDatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameAstorbDat, sourceUri: uriAstorbDat);

	/// <summary>Checks if an update for the ALLNUM.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the ALLNUM.CAT database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsAllnumCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameAllnumCat, sourceUri: uriAllnumCat);

	/// <summary>Checks if an update for the UFITOBS.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the UFITOBS.CAT database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsUfitobsCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameUfitobsCat, sourceUri: uriUfitobsCat);

	/// <summary>Checks if an update for the SINGOPP.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the SINGOPP.CAT database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsSingoppCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameSingoppCat, sourceUri: uriSingoppCat);

	/// <summary>Shows the downloader form for the MPCORB.DAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the MPCORB.DAT database.</remarks>
	private void ShowMpcorbDatDownloader()
	{
		// Log the action of showing the MPCORB.DAT downloader form
		logger.Info(message: "Showing MPCORB.DAT downloader form.");
		// Call the generic method to show the database downloader form for MPCORB.DAT
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemMpcorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowMpcorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelMpcorbDatUpdate);
	}

	/// <summary>Shows the downloader form for the MPCORB.JSON database.</summary>
	/// <remarks>This method is used to display the downloader form for the MPCORB.JSON database.</remarks>
	private void ShowMpcorbJsonDownloader()
	{
		// Log the action of showing the MPCORB.JSON downloader form
		logger.Info(message: "Showing MPCORB.JSON downloader form.");
		// Call the generic method to show the database downloader form for MPCORB.JSON
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemMpcorbJsonGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowMpcorbJsonUpdateIsAvailable);
	}

	/// <summary>Shows the downloader form for the ASTORB.DAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the ASTORB.DAT database.</remarks>
	private void ShowAstorbDatDownloader()
	{
		// Log the action of showing the ASTORB.DAT downloader form
		logger.Info(message: "Showing ASTORB.DAT downloader form.");
		// Call the generic method to show the database downloader form for ASTORB.DAT
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAstorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAstorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelAstorbDatUpdate);
	}

	/// <summary>Shows the downloader form for the ALLNUM.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the ALLNUM.CAT database.</remarks>
	private void ShowAllnumCatDownloader()
	{
		// Log the action of showing the ALLNUM.CAT downloader form
		logger.Info(message: "Showing ALLNUM.CAT downloader form.");
		// Call the generic method to show the database downloader form for ALLNUM.CAT
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAllnumCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAllnumCatUpdateIsAvailable);
	}

	/// <summary>Shows the downloader form for the UFITOBS.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the UFITOBS.CAT database.</remarks>
	private void ShowUfitobsCatDownloader()
	{
		// Log the action of showing the UFITOBS.CAT downloader form
		logger.Info(message: "Showing UFITOBS.CAT downloader form.");
		// Call the generic method to show the database downloader form for UFITOBS.CAT
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemUfitobsCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowUfitobsCatUpdateIsAvailable);
	}

	/// <summary>Shows the downloader form for the SINGOPP.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the SINGOPP.CAT database.</remarks>
	private void ShowSingoppCatDownloader()
	{
		// Log the action of showing the SINGOPP.CAT downloader form
		logger.Info(message: "Showing SINGOPP.CAT downloader form.");
		// Call the generic method to show the database downloader form for SINGOPP.CAT
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemSingoppCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowSingoppCatUpdateIsAvailable);
	}

	/// <summary>Checks if a remote database file is newer than the local file.</summary>
	/// <param name="localFilePath">Path to the local database file.</param>
	/// <param name="sourceUri">Remote URI of the database file.</param>
	/// <param name="readContentLength">Whether to also read the remote content length.</param>
	/// <returns><see langword="true"/> when an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if a remote database file is newer than the local file by comparing their last modified dates. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private static bool IsDatabaseUpdateAvailable(string localFilePath, Uri sourceUri, bool readContentLength = false)
	{
		// Check if the file exists before attempting to access its metadata
		if (!File.Exists(path: localFilePath))
		{
			// If the file does not exist, return true (update available)
			return true;
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: localFilePath);
		// Get the last modified date of the local file
		DateTime localLastWriteTime = fileInfo.LastWriteTimeUtc;
		// Compare the last modified dates of the local and remote files
		try
		{
			// Get the last modified date of the online file
			DateTime remoteLastModified = GetLastModified(uri: sourceUri);
			// If the readContentLength flag is set to true, also get the content length of the online file
			if (readContentLength)
			{
				// Get the content length of the online file
				_ = GetContentLength(uri: sourceUri);
			}
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Return true if the remote file is newer than the local file; otherwise, return false
			return remoteLastModified > localLastWriteTime;
		}
		// Catch specific exceptions related to network and file access issues
		catch (Exception ex) when (ex is HttpRequestException or WebException or IOException)
		{
			// Log the exception and return false (no update available)
			logger.Error(exception: ex, message: "Error checking update availability for '{0}'.", args: [sourceUri]);
			return false;
		}
	}

	/// <summary>Shows the generic database downloader workflow for a specific source.</summary>
	/// <param name="downloadUrl">The source URL for downloading the database.</param>
	/// <param name="updateAvailableMenuItem">Menu item that indicates update availability.</param>
	/// <param name="updateStatusItem">Optional status indicator that should be disabled after successful download.</param>
	/// <remarks>This method checks for network availability, displays the downloader form, and handles the post-download workflow, including disabling relevant UI elements and prompting for application restart.</remarks>
	private void ShowDatabaseDownloader(string downloadUrl, ToolStripItem updateAvailableMenuItem, ToolStripItem? updateStatusItem = null)
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// If the network is not available, disable the update menu item and show an error message
			updateAvailableMenuItem.Enabled = false;
			// Show an error message indicating that there is no internet connection
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			// Exit the method early since the download cannot proceed without an internet connection
			return;
		}
		// Create and display the database downloader form with the specified download URL
		using DatabaseDownloaderForm downloaderForm = new(url: downloadUrl);
		// Set the TopMost property of the downloader form to match the main form's TopMost property
		downloaderForm.TopMost = TopMost;
		// Show the downloader form as a modal dialog and check the result
		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			// If the user cancels or closes the downloader form without completing the download, exit the method early
			return;
		}
		// If the download was successful, disable the update menu item and the optional status indicator
		updateAvailableMenuItem.Enabled = false;
		updateStatusItem?.Enabled = false;
		// Prompt the user to restart the application after downloading the database
		AskForRestartAfterDownloadingDatabase();
	}
}