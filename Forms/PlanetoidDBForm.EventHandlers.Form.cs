/*
 * File:        PlanetoidDbForm.EventHandlers.Form.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing form event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all form event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the Load event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and its controls.</remarks>
	private void PlanetoidDBForm_Load(object sender, EventArgs e)
	{
		// Log the Load event of the form
		logger.Info(message: "PlanetoidDBForm_Load event triggered");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Start the stop watch.
		stopWatch = new Stopwatch();
		stopWatch.Start();

		fileSystemWatcherSystemDirectory.Path = AppContext.BaseDirectory;
		fileSystemWatcherAppDirectory.Path = Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.ApplicationData), path2: "Planetoid-Db");

		// Clear the status bar before loading the database
		ClearStatusBar(label: labelInformation);
		// Reload the application settings to ensure the latest configuration is applied
		Settings.Default.Reload();
		// Check if a user upgrade is required and perform the upgrade if necessary
		if (Settings.Default.userUpgradeRequired)
		{
			Settings.Default.Upgrade();
			Settings.Default.userUpgradeRequired = false;
			Settings.Default.Save();
		}

		toolStripStatusLabelWindowSize.Text = $"{Size.Width}x{Size.Height}";

		// Set the checked state of the enable UI sound menu item based on the user settings
		toolStripMenuItemOptionEnableUiSound.Checked = Settings.Default.userEnableUiSound;
		// Set the checked state of the logging menu item based on the user settings
		toolStripMenuItemLogging.Checked = Settings.Default.userEnableLogging;
		// Set the checked state of the check for database updates on startup menu item based on the user settings
		toolStripMenuItemCheckingDatabaseUpdatesOnStartup.Checked = Settings.Default.userCheckingDatabaseUpdatesOnStartup;
		// Set the checked state of the load database on startup menu items based on the user settings
		toolStripMenuItemLoadAdditionalDatabasesOnStartup.Enabled = Settings.Default.userEnableExperimentalFeatures;
		// TODO: Remove this line when the other databases are implemented
		toolStripMenuItemLoadDatabaseOnStartupMpcorbJson.Checked = Settings.Default.userLoadAdditionalDatabaseOnStartupMpcorbJson;
		toolStripMenuItemLoadDatabaseOnStartupAstorbDat.Checked = Settings.Default.userLoadAdditionalDatabaseOnStartupAstorbDat;
		toolStripMenuItemLoadDatabaseOnStartupAllnumCat.Checked = Settings.Default.userLoadAdditionalDatabaseOnStartupAllnumCat;
		toolStripMenuItemLoadDatabaseOnStartupSingoppCat.Checked = Settings.Default.userLoadAdditionalDatabaseOnStartupSingoppCat;
		toolStripMenuItemLoadDatabaseOnStartupUfitobsCat.Checked = Settings.Default.userLoadAdditionalDatabaseOnStartupUfitobsCat;
		// Set the visibility of the additional database tabs to false initially
		kryptonPageMpcorbJson.Visible = false;
		kryptonPageAstorbDat.Visible = false;
		kryptonPageAllnumCat.Visible = false;
		kryptonPageSingoppCat.Visible = false;
		kryptonPageUfitobsCat.Visible = false;
		// If the load additional databases on startup menu item is enabled, set the visibility of the corresponding tabs based on the user settings
		if (toolStripMenuItemLoadAdditionalDatabasesOnStartup.Enabled)
		{
			kryptonPageMpcorbJson.Visible = Settings.Default.userLoadAdditionalDatabaseOnStartupMpcorbJson;
			kryptonPageAstorbDat.Visible = Settings.Default.userLoadAdditionalDatabaseOnStartupAstorbDat;
			kryptonPageAllnumCat.Visible = Settings.Default.userLoadAdditionalDatabaseOnStartupAllnumCat;
			kryptonPageSingoppCat.Visible = Settings.Default.userLoadAdditionalDatabaseOnStartupSingoppCat;
			kryptonPageUfitobsCat.Visible = Settings.Default.userLoadAdditionalDatabaseOnStartupUfitobsCat;
		}
		// Check if experimental features are enabled in the settings and enable them if so
		if (Settings.Default.userEnableExperimentalFeatures)
		{
			toolStripMenuItemExperimentalFeatures.Checked = Settings.Default.userEnableExperimentalFeatures;
			EnableExperimentalFeatures(silent: true);
		}
		// Set the initial text of the MPCORB.DAT tab to indicate that the database is loading
		kryptonPageMpcorbDat.Text = $"MPCORB.DAT ({I18nStrings.DataLoading})";
		// Set the initial text of the MPCORB.JSON tab to indicate that the database is loading
		kryptonPageMpcorbJson.Text = $"MPCORB.JSON ({I18nStrings.DataLoading})";
		// Set the initial text of the ASTORB.DAT tab to indicate that the database is loading
		kryptonPageAstorbDat.Text = $"ASTORB.DAT ({I18nStrings.DataLoading})";
		// Configure the BackgroundWorker for loading the database
		backgroundWorkerLoadingDatabase.WorkerReportsProgress = true;
		backgroundWorkerLoadingDatabase.WorkerSupportsCancellation = true;
		backgroundWorkerLoadingDatabase.ProgressChanged += BackgroundWorkerLoadingDatabase_ProgressChanged;
		backgroundWorkerLoadingDatabase.RunWorkerCompleted += BackgroundWorkerLoadingDatabase_RunWorkerCompleted;
		backgroundWorkerLoadingDatabase.RunWorkerAsync();
		// Show the splash screen while loading the database
		logger.Info(message: "Showing splash screen");
		formSplashScreen.Show();
		// Attempt to get the last modified date of the MPCORB.DAT file and display it in the tab text
		string resolvedMpcOrbDatFilePath = string.IsNullOrWhiteSpace(value: MpcOrbDatFilePath) ? filenameMpcorbDat : MpcOrbDatFilePath;
		if (!string.IsNullOrWhiteSpace(value: resolvedMpcOrbDatFilePath))
		{
			// Use a try-catch block to handle potential exceptions when accessing the file information
			try
			{
				// Get the file information for the MPCORB.DAT file
				FileInfo fileInfo = new(fileName: resolvedMpcOrbDatFilePath);
				// Check if the file exists before attempting to access its properties
				if (fileInfo.Exists)
				{
					// Get the last modified date of the file in local time
					DateTime datetimeFileLocal = fileInfo.LastWriteTime;
					kryptonPageMpcorbDat.Text = $"MPCORB.DAT ({datetimeFileLocal.ToString(provider: CultureInfo.CurrentCulture)})";
				}
			}
			// Catch specific exceptions related to file access and log the errors
			catch (ArgumentException ArgEx)
			{
				// Log the error message for an invalid file path format
				logger.Error(exception: ArgEx, message: $"Invalid file path format: {resolvedMpcOrbDatFilePath}");
			}
			catch (NotSupportedException NotSupEx)
			{
				// Log the error message for an unsupported file path format
				logger.Error(exception: NotSupEx, message: $"Invalid file path format: {resolvedMpcOrbDatFilePath}");
			}
			catch (PathTooLongException PathTooLongEx)
			{
				// Log the error message for a file path that is too long
				logger.Error(exception: PathTooLongEx, message: $"File path is too long: {resolvedMpcOrbDatFilePath}");
			}
			catch (UnauthorizedAccessException UnauthorizedAccessEx)
			{
				// Log the error message for unauthorized access to the file path
				logger.Error(exception: UnauthorizedAccessEx, message: $"Unauthorized access to file path: {resolvedMpcOrbDatFilePath}");
			}
		}

		// ContextMenuStrip für die ListView einrichten
		InitializeListViewreadableDesignationContextMenu();

		// Load the ASTORB.DAT file and update the tab text with the last modified date
		//LoadAstorbDatabase();
		// Load all bookmarks from disk into memory at startup
		LoadAllBookmarks();
		// Get the amount of hard drive space used by the program
		DiskSizeUsage();
		// Log the completion of the Load event of the form
		logger.Info(message: "PlanetoidDBForm_Load event completed");
	}

	/// <summary>Handles the shown event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the shown event of the form.</remarks>
	private void PlanetoidDBForm_Shown(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Check if an update is available for the MPCORB.DAT database and enable the timer for blinking the update label
		if (IsMpcorbDatUpdateAvailable())
		{
			toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = true;
			toolStripStatusLabelMpcorbDatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			toolStripStatusLabelMpcorbDatUpdate.Enabled = false;
			toolStripStatusLabelMpcorbDatUpdate.Visible = false;
		}
		// Check if an update is available for the MPCORB.JSON database and enable the timer for blinking the update label
		if (IsMpcorbJsonUpdateAvailable())
		{
			toolStripMenuItemShowMpcorbJsonUpdateIsAvailable.Enabled = true;
			//toolStripStatusLabelMpcorbJsonUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			//toolStripStatusLabelMpcorbJsonUpdate.Enabled = false;
			//toolStripStatusLabelMpcorbJsonUpdate.Visible = false;
		}
		// Check if an update is available for the ASTORB.DAT database and enable the timer for blinking the update label
		if (IsAstorbDatUpdateAvailable())
		{
			toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = true;
			toolStripStatusLabelAstorbDatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			toolStripStatusLabelAstorbDatUpdate.Enabled = false;
			toolStripStatusLabelAstorbDatUpdate.Visible = false;
		}
		// Check if an update is available for the ALLNUM.CAT database and enable the timer for blinking the update label
		if (IsAllnumCatUpdateAvailable())
		{
			toolStripMenuItemShowAllnumCatUpdateIsAvailable.Enabled = true;
			//toolStripStatusLabelAllnumCatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			//toolStripStatusLabelAllnumCatUpdate.Enabled = false;
			//toolStripStatusLabelAllnumCatUpdate.Visible = false;
		}
		// Check if an update is available for the UFITOBS.CAT database and enable the timer for blinking the update label
		if (IsUfitobsCatUpdateAvailable())
		{
			toolStripMenuItemShowUfitobsCatUpdateIsAvailable.Enabled = true;
			//toolStripStatusLabelUfitobsCatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			//toolStripStatusLabelUfitobsCatUpdate.Enabled = false;
			//toolStripStatusLabelUfitobsCatUpdate.Visible = false;
		}
		// Check if an update is available for the SINGOPP.CAT database and enable the timer for blinking the update label
		if (IsSingoppCatUpdateAvailable())
		{
			toolStripMenuItemShowSingoppCatUpdateIsAvailable.Enabled = true;
			//toolStripStatusLabelSingoppCatUpdate.Enabled = true;
		}
		// Otherwise, disable and hide the update label
		else
		{
			//toolStripStatusLabelSingoppCatUpdate.Enabled = false;
			//toolStripStatusLabelSingoppCatUpdate.Visible = false;
		}
		// Check if the form should stay on top of other windows
		CheckStayOnTop();



		// Check if the database is loaded
		if (planetoidsDatabase.Count == 0)
		{
			logger.Warn(message: "Planetoids database is empty.");
			return;
		}
		// Validate the numeric up/down values
		try
		{
			// Validate the numeric up/down values
			listViewReadableDesignations.BeginUpdate();
			listViewReadableDesignations.Visible = false;
			listViewReadableDesignations.VirtualMode = false;
			// Clear selection before resetting, very important!
			listViewReadableDesignations.SelectedIndices.Clear();
			listViewReadableDesignations.Items.Clear();
			// Calculate the range based on the numeric up/down values
			int min = 0;
			int max = planetoidsDatabase.Count;
			int count = max - min;
			// If the count is less than or equal to zero, show a message and return
			if (count > 0)
			{
				// Virtual Mode configure
				//sortedIndices = null;
				//sortColumn = -1;
				//sortOrder = SortOrder.None;
				//virtualListOffset = min;
				// Enable virtual mode and set the virtual list size
				listViewReadableDesignations.VirtualMode = true;
				listViewReadableDesignations.VirtualListSize = count;
			}
			// If the count is zero or negative, just show an empty list
			listViewReadableDesignations.Visible = true;
		}
		// Handle any exceptions that occur during the list initialization
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Error initializing virtual list.");
			ShowErrorMessage(message: $"Error loading list: {ex.Message}");
		}
		// Ensure that the UI is updated and the save button is enabled regardless of success or failure
		finally
		{
			listViewReadableDesignations.EndUpdate();
			//toolStripDropDownButtonSaveList.Enabled = true;
		}
	}

	/// <summary>Handles the FormClosing event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the form closing event.</remarks>
	private void PlanetoidDBForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log the FormClosing event of the form
		logger.Info(message: "PlanetoidDBForm_FormClosing event triggered");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Keep the form open until the shutdown sound has finished playing
		if (isShutdownSoundPlaybackComplete)
		{
			return;
		}
		// Cancel the form closing event to allow the shutdown sound to play
		e.Cancel = true;
		// Check if the shutdown sound playback has already started to avoid multiple invocations
		if (isShutdownSoundPlaybackStarted)
		{
			// If the shutdown sound playback has already started, simply return to avoid multiple invocations
			return;
		}
		isShutdownSoundPlaybackStarted = true;
		// Log the closing of the application
		logger.Info(message: "Closing application");
		// Save the current application settings before closing the form
		try
		{
			// Log the saving of application settings
			logger.Info(message: "Saving application settings.");
			// Save the current application settings
			Settings.Default.Save();
		}
		// Catch any exceptions that occur during the saving of application settings and log a warning
		catch (Exception ex)
		{
			// Log a warning message if saving application settings fails
			logger.Warn(exception: ex, message: "Failed to save application settings during shutdown.");
		}
		// Check if the file exists before attempting to delete it
		try
		{
			// Log the check for the temporary file
			logger.Info(message: $"Checking for temporary file: {filenameMpcorbTemp}");
			// Check if the temporary file exists
			if (File.Exists(path: filenameMpcorbTemp))
			{
				// Delete the temporary file if it exists
				File.Delete(path: filenameMpcorbTemp);
				// Log the deletion of the temporary file
				logger.Info(message: $"Deleted temporary file: {filenameMpcorbTemp}");
			}
		}
		// Catch any exceptions that occur during the deletion of the temporary file and log a warning
		catch (Exception ex)
		{
			// Log a warning message if deleting the temporary file fails
			logger.Warn(exception: ex, message: "Failed to delete the temporary file during shutdown.");
		}
		// Play the shutdown sound before allowing the application to exit
		try
		{
			// Log the attempt to play the shutdown sound
			logger.Info(message: "Attempting to play shutdown sound.");
			// Play the shutdown sound in the background
			_ = AudioPlayback.PlayShutDownSound();
		}
		// Catch any exceptions that occur during the playback of the shutdown sound and log a warning
		catch (Exception ex)
		{
			// Log a warning message if playing the shutdown sound fails
			logger.Warn(exception: ex, message: "Failed to play the shutdown sound.");
		}
		finally
		{
			stopWatch?.Stop();
			isShutdownSoundPlaybackComplete = true;
			Close();
		}
	}

	/// <summary>Updates the displayed window size.</summary>
	/// <param name="sender">The source of the resize event.</param>
	/// <param name="e">The event data.</param>
	private void PlanetoidDbForm_Resize(object sender, EventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		toolStripStatusLabelWindowSize.Text = $"{Size.Width}x{Size.Height}";
	}
}
