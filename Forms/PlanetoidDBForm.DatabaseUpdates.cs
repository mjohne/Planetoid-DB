using System.Net;
using System.Net.NetworkInformation;
using Planetoid_DB.Properties;

namespace Planetoid_DB;

public partial class PlanetoidDbForm
{
	/// <summary>Checks if an update for the MPCORB database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsMpcorbDatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameMpcorbDat, sourceUri: uriMpcorbDat, readContentLength: true);

	/// <summary>Checks if an update for the ASTORB database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsAstorbDatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameAstorbDat, sourceUri: uriAstorbDat);

	/// <summary>Checks if an update for the ALLNUM.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsAllnumCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameAllnumCat, sourceUri: uriAllnumCat);

	/// <summary>Checks if an update for the UFITOBS.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsUfitobsCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameUfitobsCat, sourceUri: uriUfitobsCat);

	/// <summary>Checks if an update for the SINGOPP.CAT database is available.</summary>
	/// <returns><see langword="true"/> if an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsSingoppCatUpdateAvailable() => IsDatabaseUpdateAvailable(localFilePath: filenameSingoppCat, sourceUri: uriSingoppCat);

	/// <summary>Shows the downloader form for the MPCORB database.</summary>
	private void ShowMpcorbDatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemMpcorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowMpcorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelMpcorbDatUpdate);

	/// <summary>Shows the downloader form for the ASTORB database.</summary>
	private void ShowAstorbDatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAstorbDatGzUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAstorbDatUpdateIsAvailable,
			updateStatusItem: toolStripStatusLabelAstorbDatUpdate);

	/// <summary>Shows the downloader form for the ALLNUM.CAT database.</summary>
	private void ShowAllnumCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemAllnumCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowAllnumCatUpdateIsAvailable);

	/// <summary>Shows the downloader form for the UFITOBS.CAT database.</summary>
	private void ShowUfitobsCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemUfitobsCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowUfitobsCatUpdateIsAvailable);

	/// <summary>Shows the downloader form for the SINGOPP.CAT database.</summary>
	private void ShowSingoppCatDownloader() =>
		ShowDatabaseDownloader(
			downloadUrl: Settings.Default.systemSingoppCatUrl,
			updateAvailableMenuItem: toolStripMenuItemShowSingoppCatUpdateIsAvailable);

	/// <summary>Checks if a remote database file is newer than the local file.</summary>
	/// <param name="localFilePath">Path to the local database file.</param>
	/// <param name="sourceUri">Remote URI of the database file.</param>
	/// <param name="readContentLength">Whether to also read the remote content length.</param>
	/// <returns><see langword="true"/> when an update is available; otherwise, <see langword="false"/>.</returns>
	private bool IsDatabaseUpdateAvailable(string localFilePath, Uri sourceUri, bool readContentLength = false)
	{
		if (!File.Exists(path: localFilePath))
		{
			return true;
		}

		FileInfo fileInfo = new(fileName: localFilePath);
		DateTime localLastWriteTime = fileInfo.LastWriteTime;

		try
		{
			DateTime remoteLastModified = GetLastModified(uri: sourceUri);
			if (readContentLength)
			{
				_ = GetContentLength(uri: sourceUri);
			}

			_ = fileInfo.Length;
			return remoteLastModified > localLastWriteTime;
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
	private void ShowDatabaseDownloader(string downloadUrl, ToolStripItem updateAvailableMenuItem, ToolStripItem? updateStatusItem = null)
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			updateAvailableMenuItem.Enabled = false;
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}

		using DatabaseDownloaderForm downloaderForm = new(url: downloadUrl);
		downloaderForm.TopMost = TopMost;

		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}

		updateAvailableMenuItem.Enabled = false;
		if (updateStatusItem is not null)
		{
			updateStatusItem.Enabled = false;
		}

		AskForRestartAfterDownloadingDatabase();
	}
}
