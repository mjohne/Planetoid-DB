/*
 * File:        PlanetoidDbForm.EventHandlers.BackgroundWorker.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing BackgroundWorker event handler methods for the PlanetoidDbForm.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Krypton.Toolkit;

using System.ComponentModel;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all BackgroundWorker events event handlers.</remarks>
public partial class PlanetoidDbForm
{
	/// <summary>Handles the DoWork event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to load the database in a background thread.</remarks>
	private void BackgroundWorkerLoadingDatabase_DoWork(object sender, DoWorkEventArgs e)
	{
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Log the start of the database loading process
		logger.Info(message: "Loading database in background worker");
		// Disable the form while loading the database
		Enabled = false;
		// Initialize the line number counter
		int lineNum = 0;
		// Determine the file name to load based on the provided path or default filename
		string filename = !string.IsNullOrEmpty(value: MpcOrbDatFilePath) ? MpcOrbDatFilePath : filenameMpcorbDat;
		// Create a FileInfo object to get information about the file
		FileInfo fileInfo = new(fileName: filename);
		// Get the size of the file in bytes
		long fileSize = fileInfo.Length, fileSizeRead = 0;
		// Open the file stream for reading
		using (FileStream fileStream = new(path: filename, mode: FileMode.Open))
		{
			// Create a new instance of the PlanetoidDatabase class
			StreamReader streamReader = new(stream: fileStream);
			logger.Info(message: $"Loading database from file: {filename}");
			// Show the splash screen
			formSplashScreen.Show();
			// Bring the splash screen to the front
			formSplashScreen.BringToFront();
			// Read the file line by line until the end of the file or cancellation is requested
			while (streamReader.Peek() != -1 && !backgroundWorkerLoadingDatabase.CancellationPending)
			{
				// Read a line from the file
				string? readLine = streamReader.ReadLine();
				// Update the file size read if the line is not null
				if (readLine != null)
				{
					fileSizeRead += readLine.Length;
				}
				// Variable to store the percentage of the file read
				float percent = 100 * fileSizeRead / fileSize;
				// Report progress to the background worker
				formSplashScreen.SetProgressbar(value: (int)percent);
				lineNum++;
				// Check if the line number is greater than or equal to 44
				if ((lineNum >= 44) && (!string.IsNullOrEmpty(value: readLine)))
				{
					// Add the read line to the planetoids database
					planetoidsDatabase.Add(item: readLine);
				}
			}
			fileStream.Close();
			streamReader.Close();
		}
		// Close the splash screen after loading the database
		formSplashScreen.Close();
		// Create a backup of the loaded database
		planetoidsDatabaseBackup = [.. planetoidsDatabase];
	}

	/// <summary>Handles the ProgressChanged event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the progress changed event during the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		//KryptonMessageBox.Show(owner: this, text: e.ProgressPercentage.ToString());
		// TODO: Not implemented yet
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		_ = KryptonMessageBox.Show(owner: this, text: "Not implemented yet", caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
	}

	/// <summary>Handles the RunWorkerCompleted event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the completion of the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		// Log the completion of the database loading process
		logger.Info(message: "Database loading completed");
		// Ensure that an event source was supplied
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Ensure that the event data was supplied
		ArgumentNullException.ThrowIfNull(argument: e);
		// Set the initial value of the goto index text box
		toolStripTextBoxGotoIndex.Text = 1.ToString(provider: CultureInfo.CurrentCulture);
		// Set the current position to the first record
		currentMpcorbPosition = 0;
		// Set the step position to 100
		stepPosition = 100;
		// Navigate to the current position
		GotoCurrentMpcorbPosition(position: currentMpcorbPosition);
		// TODO: remove the following if statement when the other databases are implemented, but keep the code for loading the other databases in place
		// Check if experimental features are enabled and load the corresponding databases
		if (toolStripMenuItemExperimentalFeatures.Checked)
		{
			// If MPCORB.JSON is checked, load the MPCORB.JSON file and show the data for the same position (position 0 initially)
			if (toolStripMenuItemLoadDatabaseOnStartupMpcorbJson.Checked)
			{
				LoadMpcorbJsonDatabase();
				currentMpcorbJsonPosition = 0;
				GotoCurrentMpcorbJsonPosition(position: currentMpcorbJsonPosition);
			}
			// If ASTORB.DAT is checked, load the ASTORB.DAT file and show the data for the same position (position 0 initially)
			if (toolStripMenuItemLoadDatabaseOnStartupAstorbDat.Checked)
			{
				LoadAstorbDatabase();
				currentAstorbPosition = 0;
				GotoCurrentAstorbPosition(position: currentAstorbPosition);
			}
			// If ALLNUM.CAT is checked, load the ALLNUM.CAT file and show the data for the same position (position 0 initially)
			if (toolStripMenuItemLoadDatabaseOnStartupAllnumCat.Checked)
			{
				LoadAllnumCatDatabase();
				currentAllnumCatPosition = 0;
				GotoCurrentAllnumCatPosition(position: currentAllnumCatPosition);
			}
			// If SINGOPP.CAT is checked, load the SINGOPP.CAT file and show the data for the same position (position 0 initially)
			if (toolStripMenuItemLoadDatabaseOnStartupSingoppCat.Checked)
			{
				LoadSingoppCatDatabase();
				currentSingoppCatPosition = 0;
				GotoCurrentSingoppCatPosition(position: currentSingoppCatPosition);
			}
			// If UFITOBS.CAT is checked, load the UFITOBS.CAT file and show the data for the same position (position 0 initially)
			if (toolStripMenuItemLoadDatabaseOnStartupUfitobsCat.Checked)
			{
				LoadUfitobsCatDatabase();
				currentUfitobsCatPosition = 0;
				GotoCurrentUfitobsCatPosition(position: currentUfitobsCatPosition);
			}
		}
		// Enable the form
		Enabled = true;
	}
}
