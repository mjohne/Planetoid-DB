using Planetoid_DB.Properties;

using System.Net;
using System.Net.NetworkInformation;

namespace Planetoid_DB;

/// <summary>Partial class for handling database updates in the PlanetoidDbForm.</summary>
/// <remarks>This partial class contains methods for checking for updates to various databases (MPCORB, ASTORB, ALLNUM.CAT, UFITOBS.CAT, SINGOPP.CAT) and displaying the corresponding downloader forms. It also includes a generic method for checking if a remote database file is newer than the local file and a method for showing the downloader workflow.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Checks if an update for the MPCORB database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the MPCORB database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private bool IsMpcorbDatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameMpcorbDat, sourceUri: uriMpcorbDat, readContentLength: true);

	/// <summary>Checks if an update for the ASTORB database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if an update for the ASTORB database is available by comparing the last modified date of the local file with the remote file. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
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

	/// <summary>Shows the downloader form for the MPCORB database.</summary>
	/// <remarks>This method is used to display the downloader form for the MPCORB database.</remarks>
	private void ShowMpcorbDatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemMpcorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowMpcorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelMpcorbDatUpdate);

	/// <summary>Shows the downloader form for the ASTORB database.</summary>
	/// <remarks>This method is used to display the downloader form for the ASTORB database.</remarks>
	private void ShowAstorbDatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAstorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAstorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelAstorbDatUpdate);

	/// <summary>Shows the downloader form for the ALLNUM.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the ALLNUM.CAT database.</remarks>
	private void ShowAllnumCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAllnumCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAllnumCatUpdateIsAvailable);

	/// <summary>Shows the downloader form for the UFITOBS.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the UFITOBS.CAT database.</remarks>
	private void ShowUfitobsCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemUfitobsCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowUfitobsCatUpdateIsAvailable);

	/// <summary>Shows the downloader form for the SINGOPP.CAT database.</summary>
	/// <remarks>This method is used to display the downloader form for the SINGOPP.CAT database.</remarks>
	private void ShowSingoppCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemSingoppCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowSingoppCatUpdateIsAvailable);

	/// <summary>Checks if a remote database file is newer than the local file.</summary>
	/// <param name="localFilePath">Path to the local database file.</param>
	/// <param name="sourceUri">Remote URI of the database file.</param>
	/// <param name="readContentLength">Whether to also read the remote content length.</param>
	/// <returns><see langword="true"/> when an update is available; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method checks if a remote database file is newer than the local file by comparing their last modified dates. If the local file does not exist, it returns <see langword="true"/> (update available). If the remote file is newer, it also returns <see langword="true"/>. If any exceptions occur during the process, it returns <see langword="false"/>.</remarks>
	private static bool IsDatabaseUpdateAvailable(string localFilePath, Uri sourceUri, bool readContentLength = false)
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: localFilePath))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: localFilePath);
		// Get the last modified date of the local file
		DateTime localLastWriteTime = fileInfo.LastWriteTime;
		// Compare the last modified dates of the local and remote files
		try
		{
			// Get the last modified date of the online file
			DateTime remoteLastModified = GetLastModified(uri: sourceUri);
			// If the remote file is newer than the local file, return true (update available)
			if (readContentLength)
			{
				// Get the content length of the online file
				_ = GetContentLength(uri: sourceUri);
			}
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return remoteLastModified > localLastWriteTime;
		}
		catch (HttpRequestException)
		{
			return false;
		}
		catch (WebException)
		{
			return false;
		}
		catch (IOException)
		{
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