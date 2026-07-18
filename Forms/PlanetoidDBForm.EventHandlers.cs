using Krypton.Toolkit;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.ComponentModel;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Partial class containing event handler methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains all event handlers including form lifecycle events, Click handlers, BackgroundWorker events, Timer events, and UI interaction handlers.</remarks>
public partial class PlanetoidDbForm
{
	#region form event handlers

	/// <summary>Handles the Load event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to initialize the form and its controls.</remarks>
	private void PlanetoidDBForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		// Set the initial text of the MPCORB.DAT tab to indicate that the database is loading
		kryptonPageMpcorbDat.Text = $"MPCORB.DAT ({I18nStrings.DataLoading})";
		// Configure the BackgroundWorker for loading the database
		backgroundWorkerLoadingDatabase.WorkerReportsProgress = true;
		backgroundWorkerLoadingDatabase.WorkerSupportsCancellation = true;
		backgroundWorkerLoadingDatabase.ProgressChanged += BackgroundWorkerLoadingDatabase_ProgressChanged;
		backgroundWorkerLoadingDatabase.RunWorkerCompleted += BackgroundWorkerLoadingDatabase_RunWorkerCompleted;
		backgroundWorkerLoadingDatabase.RunWorkerAsync();
		// Show the splash screen while loading the database
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
			catch (ArgumentException)
			{
				// Ignore invalid file path and keep the default loading text.
			}
			catch (NotSupportedException)
			{
				// Ignore invalid file path format and keep the default loading text.
			}
			catch (PathTooLongException)
			{
				// Ignore invalid file path length and keep the default loading text.
			}
			catch (UnauthorizedAccessException)
			{
				// Ignore inaccessible file path and keep the default loading text.
			}
		}
	}

	/// <summary>Handles the shown event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the shown event of the form.</remarks>
	private void PlanetoidDBForm_Shown(object sender, EventArgs e)
	{
		// Check if an update is available for the MPCORB database and enable the timer for blinking the update label
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
		// Check if an update is available for the ASTORB database and enable the timer for blinking the update label
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
	}

	/// <summary>Handles the FormClosing event of the PlanetoidDBForm.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the form closing event.</remarks>
	private void PlanetoidDBForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Check if the file exists before attempting to delete it
		if (File.Exists(path: filenameMpcorbTemp))
		{
			// Delete the temporary file if it exists
			File.Delete(path: filenameMpcorbTemp);
		}
	}

	#endregion

	#region BackgroundWorker event handlers for database loading on start up

	/// <summary>Handles the DoWork event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DoWorkEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to load the database in a background thread.</remarks>
	private void BackgroundWorkerLoadingDatabase_DoWork(object sender, DoWorkEventArgs e)
	{
		void InvokeOnUiThread(Action action)
		{
			if (InvokeRequired)
			{
				Invoke(method: action);
				return;
			}

			action();
		}

		InvokeOnUiThread(action: () => Enabled = false); // Disable the form while loading the database
		int lineNum = 0; // Variable to store the line number being read
		string filename = !string.IsNullOrEmpty(value: MpcOrbDatFilePath) ? MpcOrbDatFilePath : filenameMpcorbDat; // Get the file name from the path
		FileInfo fileInfo = new(fileName: filename);
		long fileSize = fileInfo.Length, fileSizeRead = 0; // Get the size of the file in bytes
														   // Open the file stream for reading
		using (FileStream fileStream = new(path: filename, mode: FileMode.Open))
		{
			// Create a new instance of the PlanetoidDatabase class
			StreamReader streamReader = new(stream: fileStream);
			// Show the splash screen
			InvokeOnUiThread(action: formSplashScreen.Show);
			while (streamReader.Peek() != -1 && !backgroundWorkerLoadingDatabase.CancellationPending)
			{
				string? readLine = streamReader.ReadLine(); // Variable to store the read line from the file
				if (readLine != null)
				{
					fileSizeRead += readLine.Length;
				}
				// ReSharper disable once PossibleLossOfFraction
				float percent = 100 * fileSizeRead / fileSize; // Variable to store the percentage of the file read
															   // Report progress to the background worker
				InvokeOnUiThread(action: () => formSplashScreen.SetProgressbar(value: (int)percent));
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
		InvokeOnUiThread(action: formSplashScreen.Close);
		// Create a backup of the loaded database
		planetoidsDatabaseBackup = [.. planetoidsDatabase];
	}

	/// <summary>Handles the ProgressChanged event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the progress changed event during the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_ProgressChanged(object? sender, ProgressChangedEventArgs e)
	{
		//KryptonMessageBox.Show(owner: this, text: e.ProgressPercentage.ToString());
		// TODO: Not implemented yet
		_ = KryptonMessageBox.Show(owner: this, text: "Not implemented yet", caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Error);
	}

	/// <summary>Handles the RunWorkerCompleted event of the BackgroundWorker for loading the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to handle the completion of the database loading process.</remarks>
	private void BackgroundWorkerLoadingDatabase_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
	{
		toolStripTextBoxGotoIndex.Text = 1.ToString(); // Set the initial value of the goto index text box
		currentPosition = 0; // Set the current position to the first record
		stepPosition = 100; // Set the step position to 100
		GotoCurrentPosition(position: currentPosition); // Navigate to the current position
		Enabled = true; // Enable the form
	}

	#endregion

	#region Timer event handlers

	/// <summary>Handles the tick event for checking new MPCORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new MPCORB data file.</remarks>
	private void TimerCheckForNewMpcorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

	/// <summary>Handles the tick event for checking new ASTORB data file. Calls the PlanetoidDBForm_Shown method.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check for a new ASTORB data file.</remarks>
	private void TimerCheckForNewAstorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

	#endregion

	#region Clear event handlers

	/// <summary>Clears the checked state of all navigation step menu items.</summary>
	/// <remarks>This method is used to clear the checked state of all navigation step menu items.</remarks>
	private void ToolStripMenuItem_Clear()
	{
		// Clear the checked state of all navigation step menu items
		toolStripMenuItemNavigateStep10.Checked = false;
		toolStripMenuItemNavigateStep100.Checked = false;
		toolStripMenuItemNavigateStep1000.Checked = false;
		toolStripMenuItemNavigateStep10000.Checked = false;
		toolStripMenuItemNavigateStep100000.Checked = false;
	}

	#endregion

	#region KeyPress event handlers

	/// <summary>Handles the KeyPress event for the ToolStripTextBoxGotoIndex. Ensures only numeric input and handles the Enter key to trigger navigation.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="KeyPressEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to ensure only numeric input is allowed in the ToolStripTextBoxGotoIndex.</remarks>
	private void GotoIndex_KeyPress(object sender, KeyPressEventArgs e)
	{
		// Check if the pressed key is a control character or a digit
		if (!char.IsControl(c: e.KeyChar) && !char.IsDigit(c: e.KeyChar))
		{
			// If the pressed key is not a digit or control character, suppress the key event
			e.Handled = true;
		}
		// Check if the pressed key is a digit or control character
		if (e.KeyChar == Convert.ToChar(value: Keys.Return, provider: CultureInfo.CurrentCulture))
		{
			// If the Enter key is pressed, trigger the click event for the ToolStripButtonGoToIndex
			GoToIndex_Click(sender: null, e: null);
		}
	}

	#endregion

	#region MouseDown event handlers

	/// <summary>Handles the MouseDown event for controls. Stores the control that triggered the event for future reference.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to store the control that triggered the event for future reference.</remarks>
	protected override void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
			// Store the current tag text of the control
			currentTagText = control.Tag?.ToString() ?? string.Empty;
		}
	}

	#endregion

	#region Click & ButtonClick event Handlers

	/// <summary>Handles the click event for the ToolStripMenuItemArchive. Opens the archive.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the archive.</remarks>
	private void Archive_Click(object sender, EventArgs e) => ShowArchive();

	/// <summary>Handles the click event for the ToolStripButtonGoToIndex. Navigates to the specified index in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void GoToIndex_Click(object? sender, EventArgs? e)
	{
		int pos = 0;
		// Try to parse the index from the ToolStripTextBoxGotoIndex
		try
		{
			// Parse the index from the text box
			pos = int.Parse(s: toolStripTextBoxGotoIndex.Text, provider: CultureInfo.CurrentCulture);
		}
		// Catch any exceptions that occur during parsing
		catch (Exception ex)
		{
			// Log the error message
			logger.Error(message: ex.Message);
			// Show an error message box with the exception message
			ShowErrorMessage(message: $"{nameof(GoToIndex_Click)}  {ex.Message}");
		}
		// If the parsed index is out of range, show an error message
		// Otherwise, navigate to the specified index
		if (pos <= 0 || pos >= planetoidsDatabase.Count + 1)
		{
			// Log the error message
			logger.Error(message: "Index out of range");
			// Show an error message if the index is out of range
			ShowErrorMessage(message: $"{I18nStrings.IndexOutOfRange}");
		}
		else
		{
			// Navigate to the specified index
			currentPosition = pos - 1;
			GotoCurrentPosition(position: currentPosition);
		}
	}

	/// <summary>Handles the click event for the ToolStripButtonTerminology. Opens the terminology form with the specified index.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the terminology form with the specified index.</remarks>
	private void Terminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

	/// <summary>Handles the click event for the ToolStripMenuItem10. Sets the navigation step to 10 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep10_Click(object sender, EventArgs e)
	{
		// Set the step position to 10
		stepPosition = 10;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep10.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem100. Sets the navigation step to 100 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep100_Click(object sender, EventArgs e)
	{
		// Set the step position to 100
		stepPosition = 100;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep100.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem1000. Sets the navigation step to 1000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep1000_Click(object sender, EventArgs e)
	{
		// Set the step position to 1000
		stepPosition = 1000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep1000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem10000. Sets the navigation step to 10000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep10000_Click(object sender, EventArgs e)
	{
		// Set the step position to 10000
		stepPosition = 10000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep10000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItem100000. Sets the navigation step to 100000 and updates the menu item checked state.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to a specific index in the data.</remarks>
	private void NavigateStep100000_Click(object sender, EventArgs e)
	{
		// Set the step position to 100000
		stepPosition = 100000;
		// Clear the checked state of all other menu items
		ToolStripMenuItem_Clear();
		// Set the checked state of the menu item to true
		toolStripMenuItemNavigateStep100000.Checked = true;
	}

	/// <summary>Handles the click event for the ToolStripMenuItemExit. Closes the application.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to close the application.</remarks>
	private void Exit_Click(object sender, EventArgs e) => Close();

	/// <summary>Handles the click event for the ToolStripMenuItemOpenWebsiteMPC. Opens the Minor Planet Center website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Minor Planet Center website.</remarks>
	private void OpenWebsiteMPC_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemWebsiteMpc);

	/// <summary>Handles the click event for the ToolStripMenuItemOpenMPCORBWebsite. Opens the MPCORB website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the MPCORB website.</remarks>
	private void OpenMpcorbDatWebsite_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemWebsiteMpcorb);

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadMpcorbDat. Shows the downloader form for the MPCORB database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the MPCORB database.</remarks>
	private void DownloadMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadAstorbDat. Shows the downloader form for the ASTORB database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the ASTORB database.</remarks>
	private void DownloadAstorbDat_Click(object sender, EventArgs e) => ShowAstorbDatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadAllnumCat. Shows the downloader form for the ALLNUMCAT database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the ALLNUMCAT database.</remarks>
	private void DownloadAllnumCat_Click(object sender, EventArgs e) => ShowAllnumCatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadUfitobsCat. Shows the downloader form for the UFITOBS database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the UFITOBS database.</remarks>
	private void DownloadUfitobsCat_Click(object sender, EventArgs e) => ShowUfitobsCatDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemDownloadSingoppCat. Shows the downloader form for the SINGOPP database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the downloader form for the SINGOPP database.</remarks>
	private void DownloadSingoppCat_Click(object sender, EventArgs e) => ShowSingoppCatDownloader();

	/// <summary>Handles the click event for the ToolStripButtonCheckMpcorbDatUpdate. Shows the MPCORB data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the MPCORB data check form.</remarks>
	private void CheckMpcorbDatUpdate_Click(object sender, EventArgs e) => ShowMpcorbDatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripMenuItemCheckAstorbDat. Shows the ASTORB data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the ASTORB data check form.</remarks>
	private void CheckAstorbDatUpdate_Click(object sender, EventArgs e) => ShowAstorbDatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripMenuItemCheckAllnumCat. Shows the ALLNUMCAT data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the ALLNUMCAT data check form.</remarks>
	private void CheckAllnumCatUpdate_Click(object sender, EventArgs e) => ShowAllnumCatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripMenuItemCheckUfitobsCat. Shows the UFITOBS data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the UFITOBS data check form.</remarks>
	private void CheckUfitobsCatUpdate_Click(object sender, EventArgs e) => ShowUfitobsCatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripMenuItemCheckSingoppCat. Shows the SINGOPP data check form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the SINGOPP data check form.</remarks>
	private void CheckSingoppCatUpdate_Click(object sender, EventArgs e) => ShowSingoppCatUpdateCheck();

	/// <summary>Handles the click event for the ToolStripButtonAbout. Shows the application information form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the application information form.</remarks>
	private void About_Click(object sender, EventArgs e) => ShowAppInfo();

	/// <summary>Handles the click event for the ToolStripButtonOpenWebsitePDB. Opens the Planetoid Database website.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the Planetoid Database website.</remarks>
	private void OpenWebsitePDB_Click(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemHomepage);

	/// <summary>Handles the click event for the TableMode button. Opens the table mode form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to open the table mode form.</remarks>
	private void TableMode_Click(object sender, EventArgs e) => OpenTableMode();

	/// <summary>Handles the click event for the ToolStripButtonDatabaseInformation. Shows the database information form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>
	///	This method is used to show the database information form.</remarks>
	private void DatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

	/// <summary>Handles the click event for the ToolStripMenuItemPrint. Shows the print data sheet form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the print data sheet form.</remarks>
	private void PrintDataSheet_Click(object sender, EventArgs e) => PrintDataSheet();

	/// <summary>Handles the click event for the ToolStripMenuItemSearch. Shows the search form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the search form.</remarks>
	private void Search_Click(object sender, EventArgs e) => ShowSearch();

	/// <summary>Handles the click event for the ToolStripButtonLoadRandomMinorPlanet. Loads a random minor planet from the database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to load a random minor planet from the database.</remarks>
	private void LoadRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheBegin. Navigates to the beginning of the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the beginning of the data.</remarks>
	private void NavigateToTheBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateSomeDataBackward. Navigates backward by a specified step in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate backward by a specified step in the data.</remarks>
	private void NavigateSomeDataBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToThePreviousData. Navigates to the previous data entry.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the previous data entry.</remarks>
	private void NavigateToThePreviousData_Click(object sender, EventArgs e) => NavigateToThePreviousData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheNextData. Navigates to the next data entry.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the next data entry.</remarks>
	private void NavigateToTheNextData_Click(object sender, EventArgs e) => NavigateToTheNextData();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateSomeDataForward. Navigates forward by a specified step in the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate forward by a specified step in the data.</remarks>
	private void NavigateSomeDataForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

	/// <summary>Handles the click event for the ToolStripMenuItemNavigateToTheEnd. Navigates to the end of the data.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to navigate to the end of the data.</remarks>
	private void NavigateToTheEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

	/// <summary>Handles the click event for the ToolStripMenuItemSettings. Shows the settings form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the settings form.</remarks>
	private void Settings_Click(object sender, EventArgs e) => ShowSettings();

	/// <summary>Handles the click event for the ToolStripButtonFilter. Shows the filter form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the filter form.</remarks>
	private void Filter_Click(object sender, EventArgs e) => ShowFilter();

	/// <summary>Handles the click event for the ToolStripButtonFilterReset. Resets the filter and restores the original database.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to reset the filter and restore the original database.</remarks>
	private void FilterReset_Click(object sender, EventArgs e)
	{
		// Replace the current database with the backup
		planetoidsDatabase.Clear();
		planetoidsDatabase.AddRange(collection: planetoidsDatabaseBackup);
		// Navigate to the first record of the backup database
		currentPosition = 0;
		GotoCurrentPosition(position: currentPosition);
		logger.Info(message: $"Filter reset: database now contains {planetoidsDatabase.Count} records.");
	}

	/// <summary>Handles the click event for the ToolStripButtonDerivedOrbitElements. Shows the derived orbit elements form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the derived orbit elements form.</remarks>
	private void DerivedOrbitElements_Click(object sender, EventArgs e) => ShowDerivedOrbitElements();

	/// <summary>Handles the click event for the ToolStripMenuItemRestart. Restarts the application.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to restart the application.</remarks>
	private void Restart_Click(object sender, EventArgs e) => Restart();

	/// <summary>Handles the click event for the ToolStripMenuItemStayOnTop. Checks if the form should stay on top of other windows.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to check if the form should stay on top of other windows.</remarks>
	private void StayOnTop_Click(object sender, EventArgs e) => CheckStayOnTop();

	/// <summary>Handles the click event for the ToolStripMenuIndexNumberCopyToClipboard_Click. Copies the index number to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the index number to the clipboard.</remarks>
	private void CopyToClipboardIndexNumber_Click(object sender, EventArgs e) => CopyToClipboard(text: labelIndexData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardReadableDesignation. Copies the readable designation to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the readable designation to the clipboard.</remarks>
	private void CopyToClipboardReadableDesignation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReadableDesignationData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardEpoch. Copies the epoch to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the epoch to the clipboard.</remarks>
	private void CopyToClipboardEpoch_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEpochData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardMeanAnomaly. Copies the mean anomaly to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean anomaly to the clipboard.</remarks>
	private void CopyToClipboardMeanAnomaly_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanAnomalyAtTheEpochData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardArgumentOfThePerihelion. Copies the argument of perihelion to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the argument of perihelion to the clipboard.</remarks>
	private void CopyToClipboardArgumentOfThePerihelion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelArgumentOfThePerihelionData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardLongitudeOfTheAscendingNode. Copies the longitude of the ascending node to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the longitude of the ascending node to the clipboard.</remarks>
	private void CopyToClipboardLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) => CopyToClipboard(text: labelLongitudeOfTheAscendingNodeData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardInclinationToTheEcliptic. Copies the inclination to the ecliptic data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the inclination to the ecliptic data to the clipboard.</remarks>
	private void CopyToClipboardInclinationToTheEcliptic_Click(object sender, EventArgs e) => CopyToClipboard(text: labelInclinationToTheEclipticData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardOrbitalEccentricity. Copies the orbital eccentricity data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the orbital eccentricity data to the clipboard.</remarks>
	private void CopyToClipboardOrbitalEccentricity_Click(object sender, EventArgs e) => CopyToClipboard(text: labelOrbitalEccentricityData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardMeanDailyMotion. Copies the mean daily motion data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the mean daily motion data to the clipboard.</remarks>
	private void CopyToClipboardMeanDailyMotion_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMeanDailyMotionData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardSemiMajorAxis. Copies the semi-major axis data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the semi-major axis data to the clipboard.</remarks>
	private void CopyToClipboardSemiMajorAxis_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSemiMajorAxisData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardAbsoluteMagnitude. Copies the absolute magnitude data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the absolute magnitude data to the clipboard.</remarks>
	private void CopyToClipboardAbsoluteMagnitude_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAbsoluteMagnitudeData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardSlopeParameter. Copies the slope parameter data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the slope parameter data to the clipboard.</remarks>
	private void CopyToClipboardSlopeParameter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSlopeParameterData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardReference. Copies the reference data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the reference data to the clipboard.</remarks>
	private void CopyToClipboardReference_Click(object sender, EventArgs e) => CopyToClipboard(text: labelReferenceData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfOppositions. Copies the number of oppositions data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the number of oppositions data to the clipboard.</remarks>
	private void CopyToClipboardNumberOfOppositions_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfOppositionsData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardNumberOfObservations. Copies the number of observations data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the number of observations data to the clipboard.</remarks>
	private void CopyToClipboardNumberOfObservations_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNumberOfObservationsData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardObservationSpan. Copies the observation span data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the observation span data to the clipboard.</remarks>
	private void CopyToClipboardObservationSpan_Click(object sender, EventArgs e) => CopyToClipboard(text: labelObservationSpanData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardRmsResidual. Copies the RMS residual data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the RMS residual data to the clipboard.</remarks>
	private void CopyToClipboardRmsResidual_Click(object sender, EventArgs e) => CopyToClipboard(text: labelRmsResidualData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardComputerName. Copies the computer name data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the computer name data to the clipboard.</remarks>
	private void CopyToClipboardComputerName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelComputerNameData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardDateOfLastObservation. Copies the date of last observation data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the date of last observation data to the clipboard.</remarks>
	private void CopyToClipboardDateOfLastObservation_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateLastObservationData.Text);

	/// <summary>Handles the click event for the ToolStripMenuItemCopyToClipboardFlags. Copies the flags data to the clipboard.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to copy the flags data to the clipboard.</remarks>
	private void CopyToClipboardFlags_Click(object sender, EventArgs e) => CopyToClipboard(text: labelFlagsData.Text);

	/// <summary>Handles the click event for the ToolStripButtonExport. Exports the data sheet.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to export the data sheet.</remarks>
	private void Export_Click(object sender, EventArgs e) => ExportDataSheet();

	/// <summary>Handles the button click event for the ToolStripSplitButtonTopTenRecords. Shows the records main form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the records main form.</remarks>
	private void Records_Click(object sender, EventArgs e) => ShowRecords();

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanAnomalyAtTheEpoch. Shows the top ten records form for mean anomaly at the epoch.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean anomaly at the epoch.</remarks>
	private void RecordsMeanAnomalyAtTheEpoch_Click(object sender, EventArgs e) =>
		// Show the top ten records form for mean anomaly at the epoch
		ShowRecordsTop10(selectedElement: "Mean anomaly at the epoch");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsArgumentOfThePerihelion. Shows the top ten records form for the argument of the perihelion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the argument of the perihelion.</remarks>
	private void RecordsArgumentOfThePerihelion_Click(object sender, EventArgs e) =>
		// Show the top ten records form for the argument of the perihelion
		ShowRecordsTop10(selectedElement: "Argument of the perihelion");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsLongitudeOfTheAscendingNode. Shows the top ten records form for the longitude of the ascending node.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for the longitude of the ascending node.</remarks>
	private void RecordsLongitudeOfTheAscendingNode_Click(object sender, EventArgs e) =>
		// Show the top ten records form for the longitude of the ascending node
		ShowRecordsTop10(selectedElement: "Longitude of the ascending node");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsInclination. Shows the top ten records form for inclination.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for inclination.</remarks>
	private void RecordsInclination_Click(object sender, EventArgs e) =>
		// Show the top ten records form for inclination
		ShowRecordsTop10(selectedElement: "Inclination");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsOrbitalEccentricity. Shows the top ten records form for orbital eccentricity.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for orbital eccentricity.</remarks>
	private void RecordsOrbitalEccentricity_Click(object sender, EventArgs e) =>
		// Show the top ten records form for orbital eccentricity
		ShowRecordsTop10(selectedElement: "Orbital eccentricity");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsMeanDailyMotion. Shows the top ten records form for mean daily motion.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for mean daily motion.</remarks>
	private void RecordsMeanDailyMotion_Click(object sender, EventArgs e) =>
		// Show the top ten records form for mean daily motion
		ShowRecordsTop10(selectedElement: "Mean daily motion");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSemiMajorAxis. Shows the top ten records form for semi-major axis.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for semi-major axis.</remarks>
	private void RecordsSemiMajorAxis_Click(object sender, EventArgs e) =>
		// Show the top ten records form for semi-major axis
		ShowRecordsTop10(selectedElement: "Semi-major axis");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsAbsoluteMagnitude. Shows the top ten records form for absolute magnitude.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for absolute magnitude.</remarks>
	private void RecordsAbsoluteMagnitude_Click(object sender, EventArgs e) =>
		// Show the top ten records form for absolute magnitude
		ShowRecordsTop10(selectedElement: "Absolute magnitude");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsSlopeParameter. Shows the top ten records form for slope parameter.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for slope parameter.</remarks>
	private void RecordsSlopeParameter_Click(object sender, EventArgs e) =>
		// Show the top ten records form for slope parameter
		ShowRecordsTop10(selectedElement: "Slope parameter");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfOppositions. Shows the top ten records form for number of oppositions.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of oppositions.</remarks>
	private void RecordsNumberOfOppositions_Click(object sender, EventArgs e) =>
		// Show the top ten records form for number of oppositions
		ShowRecordsTop10(selectedElement: "Number of oppositions");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsNumberOfObservations. Shows the top ten records form for number of observations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for number of observations.</remarks>
	private void RecordsNumberOfObservations_Click(object sender, EventArgs e) =>
		// Show the top ten records form for number of observations
		ShowRecordsTop10(selectedElement: "Number of observations");

	/// <summary>Handles the click event for the ToolStripMenuItemRecordsRmsResidual. Shows the top ten records form for RMS residual.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the top ten records form for RMS residual.</remarks>
	private void RecordsRmsResidual_Click(object sender, EventArgs e) =>
		// Show the top ten records form for RMS residual
		ShowRecordsTop10(selectedElement: "r.m.s. residual");

	/// <summary>Handles the click event for the histogram menu item or button. Shows the histogram form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>This method is used to show the histogram form for the selected parameter.</remarks>
	private void Histograms_Click(object sender, EventArgs e) => ShowHistogram();

	/// <summary>Handles the click event for the scatter plot menu item or button. Shows the scatter plot form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	///	<remarks>This method is used to show the scatter plot form for the selected parameter.</remarks>
	private void ScatterPlots_Click(object sender, EventArgs e) => ShowScatterPlot();

	/// <summary>Handles the click event by opening a modal dialog displaying a 3D diagram of planetoids.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An EventArgs that contains the event data.</param>
	/// <remarks>This method is used to show the 3D diagram form for the selected parameter.</remarks>
	private void AEIDiagram3D_Click(object sender, EventArgs e)
	{
		using AEIDiagram3DForm formAEIDiagram = new(planetoids: planetoidsDatabase);
		formAEIDiagram.TopMost = TopMost;
		_ = formAEIDiagram.ShowDialog(owner: this);
	}

	/// <summary>Handles the click event for the Average Asteroid menu item. Shows the average asteroid form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the Average Asteroid form that displays various types of averages for all orbital elements and astrophysical properties.</remarks>
	private void AverageAsteroid_Click(object sender, EventArgs e)
	{
		// Check if the database is loaded and contains data before attempting to show the form
		try
		{
			// If the database is null or empty, show an error message and return
			if (planetoidsDatabase == null || planetoidsDatabase.Count == 0)
			{
				// Log the error and show an error message to the user
				ShowErrorMessage(message: "No planetoid database loaded. Please load a database first.");
				return;
			}
			// Show the Average Asteroid form, passing the current planetoids database as a parameter
			using AverageAsteroidForm formAverageAsteroid = new(planetoids: planetoidsDatabase);
			formAverageAsteroid.TopMost = TopMost;
			_ = formAverageAsteroid.ShowDialog(owner: this);
		}
		// Catch any exceptions that occur while trying to show the form, log the error, and show an error message to the user
		catch (Exception ex)
		{
			// Log the error with the exception details and show an error message to the user
			logger.Error(message: "Failed to open Average Asteroid form: {0}", args: ex);
			ShowErrorMessage(message: $"Failed to open Average Asteroid form: {ex.Message}");
		}
	}

	/// <summary>Handles the click event for the ToolStripMenuItemListReadableDesignations. Lists readable designations.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the distribution form for the selected parameter.</remarks>
	private void ListReadableDesignations_Click(object sender, EventArgs e) => ListReadableDesignations();

	/// <summary>Handles the click event for the ToolStripButtonLicense. Opens the license.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the license.</remarks>
	private void License_Click(object sender, EventArgs e) => ShowLicense();

	/// <summary>Handles the click event for the Asteroid Game menu item. Opens the Asteroids arcade game.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method opens a new instance of the classic Asteroids arcade game implemented using OpenTK.</remarks>
	private void AsteroidGame_Click(object sender, EventArgs e)
	{
		try
		{
			using AsteroidGameForm gameForm = new();
			gameForm.TopMost = TopMost;
			_ = gameForm.ShowDialog(owner: this);
		}
		catch (Exception ex)
		{
			logger.Error(message: "Failed to open Asteroid Game: {0}", args: ex);
			ShowErrorMessage(message: $"Failed to open Asteroid Game: {ex.Message}");
		}
	}

	/// <summary>Handles the click event for the Compare Databases menu item and initiates the process to compare database archives.</summary>
	/// <remarks>This method is intended to be used as an event handler for a menu item click event. It delegates the comparison operation to the ShowCompareArchives method.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void CompareDatabases_Click(object sender, EventArgs e) => ShowCompareArchives();

	/// <summary>Handles the click event for the ToolStripMenuItemOrbitalResonances. Shows the orbital resonances form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbital resonances form.</remarks>
	private void OrbitalResonances_Click(object sender, EventArgs e) => ShowOrbitalResonances();

	/// <summary>Handles the Click event of the ToolStripButtonObservations. Shows the observations form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the observations form.</remarks>
	private void Observations_Click(object sender, EventArgs e) => ShowObservations();

	/// <summary>Handles the click event for the ToolStripMenuItemOrbitElementsGrouping. Shows the orbit elements grouping form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbit elements grouping form.</remarks>
	private void OrbitElementsGrouping_Click(object sender, EventArgs e) => ShowOrbitElementsGrouping();

	/// <summary>Handles the click event for the ToolStripMenuItemAsteroidFamiliesDetection. Shows the asteroid families form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the asteroid families form.</remarks>
	private void AsteroidFamiliesDetection_Click(object sender, EventArgs e) => ShowAsteroidFamiliesDetection();

	/// <summary>Handles the click event for the MenuitemOrbitalResonancesOfAllMinorPlanets. Shows the orbital resonances of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the orbital resonances of all minor planets form.</remarks>
	private void OrbitalResonancesOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowOrbitalResonancesOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemMoids. Shows the MOIDs form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MOIDs form for the currently selected minor planet.</remarks>
	private void Moids_Click(object sender, EventArgs e) => ShowMoids();

	/// <summary>Handles the click event for the ToolStripMenuItemMaxoids. Shows the MAXOIDs form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MAXOIDs form for the currently selected minor planet.</remarks>
	private void Maxoids_Click(object sender, EventArgs e) => ShowMaxoids();

	/// <summary>Handles the click event for the ToolStripMenuItemMoidsAndMaxoids. Shows the MOIDs and MAXOIDs form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MOIDs and MAXOIDs form for the currently selected minor planet.</remarks>
	private void MoidsAndMaxoids_Click(object sender, EventArgs e) => ShowMoidsAndMaxoids();

	/// <summary>Handles the click event for the ToolStripMenuItemMoidsOfAllMinorPlanets. Shows the MOIDs of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MOIDs of all minor planets form.</remarks>
	private void MoidsOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowMoidsOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemMaxoidsOfAllMinorPlanets. Shows the MAXOIDs of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the MAXOIDs of all minor planets form.</remarks>
	private void MaxoidsOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowMaxoidsOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemTisserandParameters. Shows the Tisserand parameters form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the Tisserand parameters form for the currently selected minor planet.</remarks>
	private void TisserandParameters_Click(object sender, EventArgs e) => ShowTisserandParameters();

	/// <summary>Handles the click event for the ToolStripMenuItemTisserandParametersOfAllMinorPlanets. Shows the Tisserand parameters of all minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the Tisserand parameters of all minor planets form.</remarks>
	private void TisserandParametersOfAllMinorPlanets_Click(object sender, EventArgs e) => ShowTisserandParametersOfAllMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemBulkObservationDataDownloader_Click. Shows the bulk observations data downloader form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show the bulk observations data downloader form.</remarks>
	private void BulkObservationDataDownloader_Click(object sender, EventArgs e) => ShowBulkObservationDataDownloader();

	/// <summary>Handles the click event for the ToolStripMenuItemMoidsRelativeToMinorPlanets. Shows the MOIDs relative to minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method opens the form for calculating the MOID between two user-selected minor planets.</remarks>
	private void MoidsRelativeToMinorPlanets_Click(object sender, EventArgs e) => ShowMoidsRelativeToMinorPlanets();

	/// <summary>Handles the click event for the ToolStripMenuItemMaxoidsRelativeToMinorPlanets. Shows the MAXOIDs relative to minor planets form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method opens the form for calculating the MAXOID between two user-selected minor planets.</remarks>
	private void MaxoidsRelativeToMinorPlanets_Click(object sender, EventArgs e) => ShowMaxoidsRelativeToMinorPlanets();

	/// <summary>Handles the click event for the toolbar button that opens a local MPCORB.DAT file. Opens a file dialog to select a local MPCORB.DAT file, and if a valid file is selected, restarts the application with the selected file path as a command-line argument.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method allows the user to select a custom local MPCORB.DAT file instead of using the default one.</remarks>
	private void OpenLocalMpcorbDat_Click(object sender, EventArgs e) => OpenLocalMpcorbDat();

	/// <summary>Handles the Click event for the MPC Database menu item and opens the Minor Planet Center database page for the selected object.</summary>
	/// <remarks>This method constructs a URL to the Minor Planet Center database using the current object's identifier and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageMpcDatabase_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://www.minorplanetcenter.net/db_search/show_object?utf8=%E2%9C%93&object_id=" + labelIndexData.Text);

	/// <summary>Handles the Click event for the JPL Small-Body Database menu item and opens the corresponding web page in the default browser.</summary>
	/// <remarks>This event handler constructs a URL to the JPL Small-Body Database using the current value of the index label and opens it in the user's default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageJplSmallBodyDatabase_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://ssd.jpl.nasa.gov/tools/sbdb_lookup.html#/?sstr=" + labelIndexData.Text + "&view=OPDA");

	/// <summary>Handles the Click event for the Lowell Minor Planet Services menu item, opening the corresponding asteroid data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL for the Lowell Observatory's asteroid search page based on the current designation and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageLowellMinorPlanetServices_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://asteroid.lowell.edu/gui/search/" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text));

	/// <summary>Handles the Click event for the Asteroids Dynamic Site menu item, opening the corresponding asteroid data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL for the selected asteroid using its readable designation and opens the associated data page in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageAsteroidsDynamicSite_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://newton.spacedys.com/astdys/index.php?pc=1.1.0&n=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text));

	/// <summary>Handles the Click event for the menu item that opens the Near-Earth Objects dynamic site in a web browser.</summary>
	/// <remarks>This method constructs a URL for the Near-Earth Objects dynamic site using the current designation and opens it in the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageNearEarthObjectsDynamicSite_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://newton.spacedys.com/neodys/index.php?pc=1.1.0&n=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text));

	/// <summary>Handles the Click event for the Near-Earth Object Coordination Centre menu item, opening the corresponding ESA NEO data page in a web browser.</summary>
	/// <remarks>This event handler constructs a URL to the ESA Near-Earth Object Coordination Centre based on the current designation and opens it in the default web browser. Use this handler to provide quick access to detailed asteroid information from the application.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenDataPageNearEarthObjectCoordinationCentre_Click(object sender, EventArgs e) => OpenWebsite(fileName: "https://neo.ssa.esa.int/search-for-asteroids?tab=summary&des=" + ProcessDesignationForUrl(input: labelReadableDesignationData.Text));


	/// <summary>Handles the Click event for the menu item that opens all relevant data pages for the selected object in the default web browser.</summary>
	/// <remarks>This method constructs URLs for multiple astronomical data sources using the current object's identifiers and opens each page in the default web browser. The method is intended to provide quick access to external resources for further information about the selected object.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void OpenAllDataPages_Click(object sender, EventArgs e)
	{
		OpenDataPageMpcDatabase_Click(sender: sender, e: e);
		OpenDataPageJplSmallBodyDatabase_Click(sender: sender, e: e);
		OpenDataPageLowellMinorPlanetServices_Click(sender: sender, e: e);
		OpenDataPageAsteroidsDynamicSite_Click(sender: sender, e: e);
		OpenDataPageNearEarthObjectsDynamicSite_Click(sender: sender, e: e);
		OpenDataPageNearEarthObjectCoordinationCentre_Click(sender: sender, e: e);
	}

	/// <summary>Handles the Click event for the label that displays MPCORB flag data and initiates decoding of the flags.</summary>
	/// <param name="sender">The source of the event, typically the label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method decodes the MPCORB flags when the label is clicked.</remarks>
	private void LabelFlagsData_Click(object sender, EventArgs e) => DecodeMpcorbFlags();

	/// <summary>Handles the Click event for the label that displays MPCORB reference data and initiates decoding of the reference.</summary>
	/// <param name="sender">The source of the event, typically the label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This method decodes the MPCORB reference when the label is clicked.</remarks>
	private void LabelReferenceData_Click(object sender, EventArgs e) => DecodeMpcorbReference();

	/// <summary>Handles the Click event of the Observatory Codes button to open the <see cref="ObservatoryCodesForm"/>.</summary>
	/// <param name="sender">The source of the event, typically the Observatory Codes button.</param>
	/// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
	/// <remarks>Opens the <see cref="ObservatoryCodesForm"/> as a modal dialog to display the list of observatory codes.</remarks>
	private void ObservatoryCodes_Click(object sender, EventArgs e)
	{
		// Open the ObservatoryCodesForm as a modal dialog to display the list of observatory codes. The form is set to be topmost based on the current state of the main form to ensure it appears above other windows.
		using ObservatoryCodesForm formObservatoryCodes = new();
		formObservatoryCodes.TopMost = TopMost;
		_ = formObservatoryCodes.ShowDialog(owner: this);
	}

	/// <summary>Handles the Click event to display the orbit 2D top view.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
	/// <remarks>This method displays the 2D top view of the orbit when the corresponding control is clicked.</remarks>
	private void Orbit2DTopView_Click(object sender, EventArgs e) => ShowOrbit2DTopView();

	/// <summary>Handles the Click event to display the orbit 2D side view.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
	/// <remarks>This method displays the 2D side view of the orbit when the corresponding control is clicked.</remarks>
	private void Orbit2DSideView_Click(object sender, EventArgs e) => ShowOrbit2DSideView();

	/// <summary>Handles the Click event to display the orbit 3D view.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
	/// <remarks>This method displays the 3D view of the orbit when the corresponding control is clicked.</remarks>
	private void Orbit3DView_Click(object sender, EventArgs e) => ShowOrbit3DView();

	#endregion

	#region DoubleClick event handlers

	/// <summary>Handles double-click events on the control to open the terminology dialog.</summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method attempts to parse the current tag text as an integer and opens the terminology dialog for the corresponding entry if successful.</remarks>
	private void OpenTerminology_DoubleClick(object sender, EventArgs e)
	{
		// Try to parse the index from the current tag text
		if (TryParseInt(input: currentTagText, value: out int index, errorMessage: out string errorMessage))
		{
			// Open the terminology dialog for the parsed index
			OpenTerminology(index: (uint)index);
			return;
		}
		// Log the error and show an error message
		logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
	}

	/// <summary>Handles the double-click event to show an Easter egg message.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to show an Easter egg message when the user double-clicks on a control.</remarks>
	private void EasterEgg_DoubleClick(object sender, EventArgs e) => KryptonMessageBox.Show(owner: this, text: I18nStrings.EasterEgg, caption: I18nStrings.ErrorCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);

	#endregion

	#region Icon set and Options event handlers

	/// <summary>Handles the click event for the Fatcow icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Fatcow.</remarks>
	private void IconSetFatcow_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Fatcow
		_ = KryptonMessageBox.Show(owner: this, text: "Fatcow icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the Silk icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Silk.</remarks>
	private void IconSetSilk_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Silk
		_ = KryptonMessageBox.Show(owner: this, text: "Silk icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for the Fugue icon set menu item.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is used to set the icon set to Fugue.</remarks>
	private void IconSetFugue_Click(object sender, EventArgs e)
	{
		// TODO: Implement icon set change to Fugue
		_ = KryptonMessageBox.Show(owner: this, text: "Fugue icon set not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for enabling copying by double-clicking option.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method toggles the option to enable copying data by double-clicking on controls.</remarks>
	private void EnableCopyingByDoubleClicking_Click(object sender, EventArgs e)
	{
		// TODO: Implement enable/disable copying by double-clicking
		_ = KryptonMessageBox.Show(owner: this, text: "Enable copying by double-clicking not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Handles the click event for enabling linking to terminology option.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method toggles the option to enable linking to terminology.</remarks>
	private void EnableLinkingToTerminology_Click(object sender, EventArgs e)
	{
		// TODO: Implement enable/disable linking to terminology
		_ = KryptonMessageBox.Show(owner: this, text: "Enable linking to terminology not implemented yet", caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
	}

	#endregion
}