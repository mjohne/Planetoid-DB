/*
 * File:        NeaListDownloadForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Downloads the NEA lists-
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

using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Planetoid_DB.Forms;

/// <summary>Converts between angle and time measure.</summary>
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class NeaListDownloadForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="NeaListDownloadForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public NeaListDownloadForm()
	{
		// Log the initialization of the form for debugging purposes
		logger.Info(message: "Initializing NeaListDownloadForm...");
		// Initialize the form components
		InitializeComponent();
		// Log that the form has been initialized successfully
		logger.Info(message: "NeaListDownloadForm initialized successfully.");
	}

	#endregion

	#region Helpers

	/// <summary>Gets a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	private void DownloadNeaList(string url, bool autoClose = false)
	{
		// Log the menu item click event for debugging purposes
		logger.Info(message: $"Download NEA list menu item clicked; checking network availability for NEA list download: {url}");
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Log that there is no internet connection
			logger.Warn(message: "No internet connection available; cannot download observatory codes.");
			// Show an error message indicating that there is no internet connection
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			// Exit the method early since the download cannot proceed without an internet connection
			return;
		}
		// Determine the URL to use for downloading NEA list
		if (string.IsNullOrWhiteSpace(value: url))
		{
			// Log that the user canceled the download and no action was taken
			logger.Info(message: "Url is null or white space; no action taken.");
			// Exit the method early since the user canceled the download
			return;
		}
		// Create and display the database downloader form with the specified download URL
		using DownloaderForm downloaderForm = new(url: url, autoClose: autoClose);
		// Set the TopMost property of the downloader form to match the main form's TopMost property
		downloaderForm.TopMost = TopMost;
		// Show the downloader form as a modal dialog and check the result
		if (downloaderForm.ShowDialog(owner: this) != DialogResult.OK)
		{
			// Log that the user canceled the download and no action was taken
			logger.Info(message: "User canceled the NEA list download; no action taken.");
			// Exit the method early since the user canceled the download
			return;
		}
	}

	#endregion

	#region Form event handlers

	/// <summary>Handles the Load event of the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	private void NeaListDownloadForm_Load(object sender, EventArgs e)
	{
		// Log the form load event for debugging purposes
		logger.Info(message: "NeaListDownloadForm loaded.");
		// Play the open sound in the background to indicate that the form has been opened
		AudioPlayback.PlayOpenSound();
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles the FormClosing event: cancels a running calculation and releases the information.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="FormClosingEventArgs"/> instance that contains the event data.</param>
	/// <remarks>If a calculation is running, it is canceled and the form will close after the checking stops. If no checking is running, the it is disposed.</remarks>
	private void NeaListDownloadForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		// Log the form closing event for debugging purposes
		logger.Info(message: "NeaListDownloadForm is closing.");
		// Play the open sound in the background to indicate that the form is closing
		AudioPlayback.PlayCloseSound();
	}

	#endregion


	private void KryptonButtonDownloadNeaListTodayPlus15Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp15.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus14Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp14.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus13Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp13.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus12Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp12.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus11Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp11.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus10Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp10.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus09Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp09.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus08Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp08.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus07Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp07.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus06Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp06.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus05Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp05.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus04Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp04.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus03Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp03.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus02Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp02.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus01Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAp01.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListToday_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm00.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus01Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm01.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus02Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm02.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus03Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm03.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus04Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm04.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus05Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm05.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus06Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm06.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus07Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm07.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus08Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm08.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus09Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm09.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus10Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm10.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus11Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm11.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus12Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm12.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus13Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm13.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus14Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm14.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus15Days_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://www.minorplanetcenter.net/iau/MPCORB/NEAm15.txt", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus15DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap15_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus14DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap14_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus13DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap13_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus12DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap12_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus11DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap11_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus10DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap10_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus09DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap09_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus08DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap08_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus07DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap07_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus06DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap06_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus05DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap05_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus04DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap04_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus03DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap03_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus02DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap02_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus01DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap01_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam00_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus01DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam01_extended.dat.gz", autoClose: false);

	}

	private void KryptonButtonDownloadNeaListTodayMinus02DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam02_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus03DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam03_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus04DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam04_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus05DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam05_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus06DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam06_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus07DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam07_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus08DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam08_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus09DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam09_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus10DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam10_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus11DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam11_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus12DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam12_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus13DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam13_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus14DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam14_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus15DaysExtDat_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam15_extended.dat.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus15DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap15_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus14DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap14_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus13DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap13_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus12DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap12_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus11DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap11_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus10DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap10_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus09DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap09_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus08DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap08_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus07DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap07_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus06DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap06_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus05DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap05_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus04DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap04_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus03DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap03_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus02DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap02_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayPlus01DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neap01_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam00_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus01DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam01_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus02DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam02_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus03DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam03_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus04DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam04_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus05DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam05_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus06DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam06_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus07DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam07_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus08DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam08_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus09DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam09_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus10DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam10_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus11DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam11_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus12DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam12_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus13DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam13_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus14DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam14_extended.json.gz", autoClose: false);
	}

	private void KryptonButtonDownloadNeaListTodayMinus15DaysExtJson_Click(object sender, EventArgs e)
	{
		DownloadNeaList(url: "https://minorplanetcenter.net/Extended_Files/neam15_extended.json.gz", autoClose: false);
	}
}
