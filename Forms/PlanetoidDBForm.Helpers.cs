using Krypton.Toolkit;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

using static Planetoid_DB.TerminologyForm;

namespace Planetoid_DB;

/// <summary>Partial class containing helper methods for the <see cref="PlanetoidDbForm"/>.</summary>
/// <remarks>This file contains utility methods, navigation logic, dialog launchers, and data processing methods used by the main form.</remarks>
public partial class PlanetoidDbForm
{
	#region helper methods

	/// <summary>Gets the file path of the MPCORB.DAT file.</summary>
	/// <remarks>This property is used to store the file path of the MPCORB.DAT file.</remarks>
	private string MpcOrbDatFilePath { get; set; } = string.Empty;

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom display string for the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Tries to parse an integer from the input string.</summary>
	/// <param name="input">The input string to parse.</param>
	/// <param name="value">The parsed integer value if successful.</param>
	/// <param name="errorMessage">An error message if parsing fails.</param>
	/// <returns>True if parsing was successful; otherwise, false.</returns>
	/// <remarks>This method is used to try parsing an integer from the input string.</remarks>
	public static bool TryParseInt(string input, out int value, out string errorMessage)
	{
		// Initialize output parameters
		value = 0;
		errorMessage = string.Empty;
		// Check if the input is null or whitespace
		if (string.IsNullOrWhiteSpace(value: input))
		{
			// Set the error message and return false
			errorMessage = "The entered text is empty or consists only of spaces.";
			return false;
		}
		// Try to parse the integer
		// If parsing fails, set the error message
		if (!int.TryParse(s: input, result: out value))
		{
			// Set the error message and return false
			errorMessage = $"The value \"{input}\" is not a valid integer.";
			return false;
		}
		// Parsing was successful
		return true;
	}

	/// <summary>Restarts the application.</summary>
	/// <remarks>This method is used to restart the application.</remarks>
	private void Restart()
	{
		// Close the current form and start a new instance of the application
		_ = Process.Start(fileName: Application.ExecutablePath);
		Close();
	}

	/// <summary>Asks the user if they want to restart the application after downloading the database.</summary>
	/// <remarks>This method is used to ask the user if they want to restart the application after downloading the database.</remarks>
	private void AskForRestartAfterDownloadingDatabase()
	{
		// Bring the main form to the foreground before showing the message box, so it cannot appear behind other application windows.
		Activate();
		// Ask the user if they want to restart the application after downloading the database
		// and show a message box with the option to restart or not
		// The message box will have the text "Download complete. Do you want to restart the application?"
		// and the caption "Information"
		// If the user clicks "Yes", restart the application
		// If the user clicks "No", do nothing
		if (KryptonMessageBox.Show(owner: this, text: I18nStrings.DownloadCompleteAndRestartQuestionText, caption: I18nStrings.InformationCaption, buttons: KryptonMessageBoxButtons.YesNo, icon: KryptonMessageBoxIcon.Question, defaultButton: KryptonMessageBoxDefaultButton.Button1) == DialogResult.Yes)
		{
			// Restart the application
			Restart();
		}
	}

	/// <summary>Navigates to the specified position in the planetoids database.</summary>
	/// <param name="position">The position to navigate to.</param>
	/// <remarks>This method is used to navigate to the specified position in the planetoids database.</remarks>
	internal void GotoCurrentPosition(int position)
	{
		// Handle the case where the database is empty
		if (position < 0 || position >= planetoidsDatabase.Count)
		{
			ClearCurrentRecordDisplay();
			toolStripLabelIndexPosition.ToolTipText = "Index: 0";
			return;
		}
		// Get entry string once to avoid repeated ToString() calls
		string? entryStr = planetoidsDatabase[index: position]?.ToString();
		// If the entry string is null or empty, clear all labels and return early
		if (string.IsNullOrEmpty(value: entryStr))
		{
			ClearCurrentRecordDisplay();
			return;
		}
		// Suspend both the panel layout and painting to eliminate all flicker
		tableLayoutPanelData.SuspendLayout();
		try
		{
			// Batch all text updates with minimal overhead
			toolStripLabelIndexPosition.ToolTipText = $"Index: {currentPosition + 1}/{planetoidsDatabase.Count}";
			// Update all labels in one go - use cached string reference
			labelIndexData.Text = entryStr[..7].Trim();
			labelAbsoluteMagnitudeData.Text = entryStr.Substring(startIndex: 8, length: 5).Trim();
			labelSlopeParameterData.Text = entryStr.Substring(startIndex: 14, length: 5).Trim();
			labelEpochData.Text = entryStr.Substring(startIndex: 20, length: 5).Trim();
			labelMeanAnomalyAtTheEpochData.Text = entryStr.Substring(startIndex: 26, length: 9).Trim();
			labelArgumentOfThePerihelionData.Text = entryStr.Substring(startIndex: 37, length: 9).Trim();
			labelLongitudeOfTheAscendingNodeData.Text = entryStr.Substring(startIndex: 48, length: 9).Trim();
			labelInclinationToTheEclipticData.Text = entryStr.Substring(startIndex: 59, length: 9).Trim();
			labelOrbitalEccentricityData.Text = entryStr.Substring(startIndex: 70, length: 9).Trim();
			labelMeanDailyMotionData.Text = entryStr.Substring(startIndex: 80, length: 11).Trim();
			labelSemiMajorAxisData.Text = entryStr.Substring(startIndex: 92, length: 11).Trim();
			labelReferenceData.Text = entryStr.Substring(startIndex: 107, length: 9).Trim();
			labelNumberOfObservationsData.Text = entryStr.Substring(startIndex: 117, length: 5).Trim();
			labelNumberOfOppositionsData.Text = entryStr.Substring(startIndex: 123, length: 3).Trim();
			labelObservationSpanData.Text = entryStr.Substring(startIndex: 127, length: 9).Trim();
			labelRmsResidualData.Text = entryStr.Substring(startIndex: 137, length: 4).Trim();
			labelComputerNameData.Text = entryStr.Substring(startIndex: 150, length: 10).Trim();
			labelFlagsData.Text = entryStr.Substring(startIndex: 161, length: 4).Trim();
			labelReadableDesignationData.Text = entryStr.Substring(startIndex: 166, length: 28).Trim();
			labelDateLastObservationData.Text = entryStr.Substring(startIndex: 194, length: 8).Trim();
			toolStripLabelIndexPosition.Text = $@"{I18nStrings.Index}: {position + 1:N0} / {planetoidsDatabase.Count:N0}";
		}
		finally
		{
			// Resume layout and perform any pending layout logic.
			tableLayoutPanelData.ResumeLayout(performLayout: true);
		}
	}

	/// <summary>Clears all record display labels in the data panel and index indicator.</summary>
	/// <remarks>This method is used to clear all record display labels in the data panel and the index indicator.</remarks>
	private void ClearCurrentRecordDisplay()
	{
		// Clear all labels in the data panel and the index indicator
		toolStripLabelIndexPosition.Text = string.Empty;
		// Suspend the layout of the TableLayoutPanel to prevent flickering during label updates
		tableLayoutPanelData.SuspendLayout();
		// Clear all labels in the TableLayoutPanel
		try
		{
			foreach (Control control in tableLayoutPanelData.Controls)
			{
				if (control is KryptonLabel or Label)
				{
					control.Text = string.Empty;
				}
			}
		}
		// Resume the layout of the TableLayoutPanel after clearing the labels
		finally
		{
			tableLayoutPanelData.ResumeLayout(performLayout: false);
		}
	}

	/// <summary>Jumps to the record with the specified index or designation.</summary>
	/// <param name="index">The index of the record.</param>
	/// <param name="designation">The designation of the record.</param>
	/// <remarks>This method is used to jump to the record with the specified index or designation in the planetoids database.</remarks>
	internal void JumpToRecord(string index, string designation)
	{
		// Loop through the planetoids database to find the record with the specified index or designation
		for (int i = 0; i < planetoidsDatabase.Count; i++)
		{
			// Extract the current entry from the database
			string entry = planetoidsDatabase[index: i];
			// Check if the index matches the current entry's index (first 7 characters)
			if (!string.IsNullOrWhiteSpace(value: index) && entry.Length >= 7 && entry[..7].Trim().Equals(value: index, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the index matches, set the current position to the index and navigate to that position
				currentPosition = i;
				GotoCurrentPosition(position: currentPosition);
				return;
			}
			// If the index does not match, check if the designation matches the current entry's designation (characters 166-193)
			if (!string.IsNullOrEmpty(value: designation) && entry.Length >= 194 && entry.Substring(startIndex: 166, length: 28).Trim().Equals(value: designation, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the designation matches, set the current position to the index and navigate to that position
				currentPosition = i;
				GotoCurrentPosition(position: currentPosition);
				return;
			}
		}
		// If no matching record is found, show an error message box to the user
		ShowErrorMessage(message: "Record not found in the current loaded database.");
	}

	/// <summary>Retrieves the last modified date and time (in UTC) of the resource at the specified URI.</summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>The <see cref="DateTime"/> representing the last modified date and time in UTC if available; otherwise, <see cref="DateTime.MinValue"/>. </returns>
	/// <remarks>This method is used to retrieve the last modified date and time of a resource.</remarks>
	private static DateTime GetLastModified(Uri uri)
	{
		// Validate the input URI
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Use HttpClient to retrieve only the headers (HEAD request)
		using HttpClient client = new();
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request);
		// Check if the request was successful
		if (response.IsSuccessStatusCode)
		{
			// Check if the Last-Modified header is present and return its value
			if (response.Content.Headers.LastModified.HasValue)
			{
				// Return the last modified date in UTC
				return response.Content.Headers.LastModified.Value.UtcDateTime;
			}
		}
		// If the Last-Modified header is not present or the request failed, return DateTime.MinValue
		return DateTime.MinValue;
	}

	/// <summary>Gets the content length of the specified URI.</summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>The content length of the URI.</returns>
	/// <remarks>This method is used to retrieve the content length of a resource.</remarks>
	private static long GetContentLength(Uri uri)
	{
		// Validate the input URI
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Use HttpClient to retrieve only the headers (HEAD request)
		using HttpClient client = new();
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request);
		// Check if the request was successful
		if (response.IsSuccessStatusCode)
		{
			// Check if the Content-Length header is present and return its value
			if (response.Content.Headers.ContentLength.HasValue)
			{
				// Return the content length
				return response.Content.Headers.ContentLength.Value;
			}
		}
		// If the Content-Length header is not present or the request failed, return 0
		return 0;
	}

	/// <summary>Checks if an update for the MPCORB database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the MPCORB database is available.</remarks>
	private bool IsMpcorbDatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameMpcorbDat))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameMpcorbDat);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriMpcorbDat);
			// Get the content length of the online file
			_ = GetContentLength(uri: uriMpcorbDat);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
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

	/// <summary>Checks if an update for the ASTORB database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the ASTORB database is available.</remarks>
	private bool IsAstorbDatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameAstorbDat))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameAstorbDat);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriAstorbDat);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
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

	/// <summary>Checks if an update for the ALLNUMCAT database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the ALLNUMCAT database is available.</remarks>
	private bool IsAllnumCatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameAllnumCat))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameAllnumCat);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriAllnumCat);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
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

	/// <summary>Checks if an update for the UFITOBSCAT database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the UFITOBSCAT database is available.</remarks>
	private bool IsUfitobsCatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameUfitobsCat))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameUfitobsCat);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriUfitobsCat);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
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

	/// <summary>Checks if an update for the SINGOPP database is available.</summary>
	/// <returns>true if an update is available, otherwise false.</returns>
	/// <remarks>This method is used to check if an update for the SINGOPP database is available.</remarks>
	private bool IsSingoppCatUpdateAvailable()
	{
		// Check if the file exists before attempting to delete it
		if (!File.Exists(path: filenameSingoppCat))
		{
			return true; // If the file does not exist, return true (update available)
		}
		// Get the file information for the local file
		FileInfo fileInfo = new(fileName: filenameSingoppCat);
		// Get the last modified date of the local file
		DateTime datetimeFileLocal = fileInfo.LastWriteTime;
		try
		{
			// Get the last modified date of the online file
			DateTime datetimeFileOnline = GetLastModified(uri: uriSingoppCat);
			// Get the content length of the local file
			_ = fileInfo.Length;
			// Check if the online file is larger than the local file
			// If it greater, return true (update available)
			// Otherwise, return false (no update available)
			return datetimeFileOnline > datetimeFileLocal;
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

	/// <summary>Loads a random minor planet from the database.</summary>
	/// <remarks>This method is used to load a random minor planet from the database.</remarks>
	private void LoadRandomMinorPlanet() => GotoCurrentPosition(position: currentPosition = new Random().Next(maxValue: planetoidsDatabase.Count + 1));

	/// <summary>Navigates to the beginning of the data.</summary>
	/// <remarks>This method is used to navigate to the beginning of the data.</remarks>
	private void NavigateToTheBeginOfTheData() => GotoCurrentPosition(position: currentPosition = 0);

	/// <summary>Navigates backward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate backward by a specified step in the data.</remarks>
	private void NavigateSomeDataBackward()
	{
		// Decrease the current position by the step size
		currentPosition -= stepPosition;
		if (currentPosition < 1)
		{
			// If the current position is less than 1, wrap around to the end of the database
			currentPosition += planetoidsDatabase.Count;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the previous data entry.</summary>
	/// <remarks>This method is used to navigate to the previous data entry in the planetoids database.</remarks>
	private void NavigateToThePreviousData()
	{
		// If the current position is 0, wrap around to the last entry in the database
		if (currentPosition == 0)
		{
			// Set the current position to the last entry in the database
			// This ensures that when the user navigates backward from the first entry, they go to the last entry
			// This is useful for circular navigation
			currentPosition = planetoidsDatabase.Count - 1;
		}
		else
		{
			// Decrease the current position by 1
			currentPosition--;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the next data entry.</summary>
	/// <remarks>This method is used to navigate to the next data entry in the planetoids database.</remarks>
	private void NavigateToTheNextData()
	{
		// If the current position is the last entry in the database, wrap around to the first entry
		if (currentPosition == planetoidsDatabase.Count - 1)
		{
			// Set the current position to 0 (the first entry in the database)
			// This ensures that when the user navigates forward from the last entry, they go to the first entry
			// This is useful for circular navigation
			currentPosition = 0;
		}
		else
		{
			// Increase the current position by 1
			currentPosition++;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates forward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate forward by a specified step in the data.</remarks>
	private void NavigateSomeDataForward()
	{
		// Increase the current position by the step size
		// This allows the user to navigate through the database in larger increments
		currentPosition += stepPosition;
		// If the current position exceeds the total number of entries in the database, wrap around to the beginning
		if (currentPosition > planetoidsDatabase.Count)
		{
			// Set the current position to the beginning of the database
			// This ensures that when the user navigates forward from the last entry, they go to the first entry
			// This is useful for circular navigation
			currentPosition -= planetoidsDatabase.Count;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
	}

	/// <summary>Navigates to the end of the data.</summary>
	/// <remarks>This method is used to navigate to the end of the data.</remarks>
	private void NavigateToTheEndOfTheData() => GotoCurrentPosition(position: currentPosition = planetoidsDatabase.Count - 1);

	/// <summary>Processes a designation string by removing parenthetical content, trimming whitespace, and replacing spaces with plus signs.</summary>
	/// <param name="input">The input designation string to process.</param>
	/// <returns>The processed string with parenthetical content removed, trimmed, and spaces replaced by plus signs.</returns>
	/// <remarks>This method is useful for preparing designation strings for URL queries. For example, "(449127) 2013 AS15" becomes "2013+AS15".</remarks>
	private static string ProcessDesignationForUrl(string input)
	{
		// Validate input
		if (string.IsNullOrWhiteSpace(value: input))
		{
			return string.Empty;
		}
		// Remove all content within parentheses (including the parentheses)
		string result = Regex.Replace(input: input, pattern: @"\([^)]*\)", replacement: string.Empty);
		// Trim leading and trailing whitespace
		result = result.Trim();
		// Replace all remaining spaces with nothing (remove spaces)
		result = result.Replace(oldValue: " ", newValue: "");
		return result;
	}

	/// <summary>Opens the terminology form with the specified index.</summary>
	/// <param name="index">The index to set active in the terminology form.</param>
	/// <remarks>This method is used to open the terminology form with the specified index.</remarks>
	private void OpenTerminology(uint index)
	{
		// Create a new instance of the TerminologyForm
		using TerminologyForm formTerminology = new();
		// Set the active terminology based on the index
		formTerminology.SelectedElement = index switch
		{
			0 => TerminologyElement.IndexNumber,
			1 => TerminologyElement.ReadableDesignation,
			2 => TerminologyElement.Epoch,
			3 => TerminologyElement.MeanAnomalyAtTheEpoch,
			4 => TerminologyElement.ArgumentOfThePerihelion,
			5 => TerminologyElement.LongitudeOfTheAscendingNode,
			6 => TerminologyElement.InclinationToTheEcliptic,
			7 => TerminologyElement.OrbitalEccentricity,
			8 => TerminologyElement.MeanDailyMotion,
			9 => TerminologyElement.SemiMajorAxis,
			10 => TerminologyElement.AbsoluteMagnitude,
			11 => TerminologyElement.SlopeParameter,
			12 => TerminologyElement.Reference,
			13 => TerminologyElement.NumberOfOppositions,
			14 => TerminologyElement.NumberOfObservations,
			15 => TerminologyElement.ObservationSpan,
			16 => TerminologyElement.RmsResidual,
			17 => TerminologyElement.ComputerName,
			18 => TerminologyElement.Flags,
			19 => TerminologyElement.DateOfLastObservation,
			20 => TerminologyElement.LinearEccentricity,
			21 => TerminologyElement.SemiMinorAxis,
			22 => TerminologyElement.MajorAxis,
			23 => TerminologyElement.MinorAxis,
			24 => TerminologyElement.EccentricAnomaly,
			25 => TerminologyElement.TrueAnomaly,
			26 => TerminologyElement.PerihelionDistance,
			27 => TerminologyElement.AphelionDistance,
			28 => TerminologyElement.LongitudeOfTheDescendingNode,
			29 => TerminologyElement.ArgumentOfTheAphelion,
			30 => TerminologyElement.FocalParameter,
			31 => TerminologyElement.SemiLatusRectum,
			32 => TerminologyElement.LatusRectum,
			33 => TerminologyElement.OrbitalPeriod,
			34 => TerminologyElement.OrbitalArea,
			35 => TerminologyElement.OrbitalPerimeter,
			36 => TerminologyElement.SemiMeanAxis,
			37 => TerminologyElement.MeanAxis,
			38 => TerminologyElement.StandardGravitationalParameter,
			_ => TerminologyElement.IndexNumber,
		};
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formTerminology.TopMost = TopMost;
		// Show the terminology form as a modal dialog
		_ = formTerminology.ShowDialog(owner: this);
	}

	/// <summary>Opens the table mode form.</summary>
	/// <remarks>This method is used to open the table mode form.</remarks>
	private void OpenTableMode()
	{
		// Create a new instance of the TableModeForm
		using TableModeForm formTableMode = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formTableMode.TopMost = TopMost;
		// Fill the form with the planetoids database
		formTableMode.FillArray(arrTemp: planetoidsDatabase);
		// Show the table mode form as a modal dialog
		_ = formTableMode.ShowDialog(owner: this);
	}

	/// <summary>Shows the orbital resonances form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis from the UI label and opens the <see cref="OrbitalResonancesOfOneMinorPlanetForm"/>.</remarks>
	private void ShowOrbitalResonances()
	{
		// Try to parse the semi-major axis from the label text using invariant culture to ensure consistent parsing regardless of the user's locale settings
		IFormatProvider provider = CultureInfo.InvariantCulture;
		// If parsing fails, log an error and show an error message to the user, then return early to avoid opening the form with invalid data
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis))
		{
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return;
		}
		// Create a new instance of the OrbitalResonancesOfOneMinorPlanetForm
		using OrbitalResonancesOfOneMinorPlanetForm formOrbitalResonances = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formOrbitalResonances.TopMost = TopMost;
		// Pass the parsed semi-major axis to the form so it can calculate and display the relevant orbital resonances for the current planetoid
		formOrbitalResonances.SetSemiMajorAxis(semiMajorAxis: semiMajorAxis);
		// Show the orbital resonances form as a modal dialog
		_ = formOrbitalResonances.ShowDialog(owner: this);
	}

	/// <summary>Shows the observations form for the current planetoid.</summary>
	/// <remarks>Passes the index data label text to the <see cref="ObservationsForm"/> and shows it as a modal dialog.</remarks>
	private void ShowObservations()
	{
		// Check if the network is available before attempting to show the observations form
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// If the network is not available, show an error message to the user and return early
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		// Create a new instance of the ObservationsForm
		using ObservationsForm formObservations = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formObservations.TopMost = TopMost;
		// Pass the index data label text to the observations form so it can use it to fetch and display the relevant observations for the current planetoid
		formObservations.SetIndexData(indexData: labelIndexData.Text);
		// Show the observations form as a modal dialog
		_ = formObservations.ShowDialog(owner: this);
	}

	/// <summary>Shows the orbit elements grouping form.</summary>
	/// <remarks>Passes the full planetoids database to the <see cref="OrbitElementsGroupingForm"/> and shows it as a modal dialog.</remarks>
	private void ShowOrbitElementsGrouping()
	{
		// Create a new instance of the OrbitElementsGroupingForm and pass the planetoids database to it
		using OrbitElementsGroupingForm formOrbitElementsGrouping = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formOrbitElementsGrouping.TopMost = TopMost;
		// Show the orbit elements grouping form as a modal dialog
		_ = formOrbitElementsGrouping.ShowDialog(owner: this);
	}

	/// <summary>Shows the asteroid families form.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can display asteroid families.</remarks>
	private void ShowAsteroidFamiliesDetection()
	{
		// Create a new instance of the AsteroidFamiliesForm and pass the planetoids database to it
		using AsteroidFamiliesForm formAsteroidFamilies = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formAsteroidFamilies.TopMost = TopMost;
		// Show the asteroid families form as a modal dialog
		_ = formAsteroidFamilies.ShowDialog(owner: this);
	}

	/// <summary>Shows the orbital resonances of all minor planets form. Opens the form to find orbital resonances of all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowOrbitalResonancesOfAllMinorPlanets()
	{
		// Create a new instance of the OrbitalResonancesOfAllMinorPlanetsForm
		using OrbitalResonancesOfAllMinorPlanetsForm formOrbitalResonances = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formOrbitalResonances.TopMost = TopMost;
		// Show the orbital resonances form as a modal dialog
		_ = formOrbitalResonances.ShowDialog(owner: this);
	}

	/// <summary>Tries to parse the current planetoid orbital elements from the UI labels.</summary>
	/// <param name="semiMajorAxis">When this method returns, contains the parsed semi-major axis in AU.</param>
	/// <param name="eccentricity">When this method returns, contains the parsed eccentricity.</param>
	/// <param name="inclinationDeg">When this method returns, contains the parsed inclination in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">When this method returns, contains the parsed longitude of ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">When this method returns, contains the parsed argument of perihelion in degrees.</param>
	/// <returns><see langword="true"/> if all orbital elements were parsed successfully; otherwise, <see langword="false"/>.</returns>
	/// <remarks>This method uses the <see cref="labelSemiMajorAxisData"/>, <see cref="labelOrbitalEccentricityData"/>, <see cref="labelInclinationToTheEclipticData"/>, <see cref="labelLongitudeOfTheAscendingNodeData"/>, and <see cref="labelArgumentOfThePerihelionData"/> labels to parse the orbital elements.</remarks>
	private bool TryParseCurrentOrbitalElements(
		out double semiMajorAxis,
		out double eccentricity,
		out double inclinationDeg,
		out double longitudeAscendingNodeDeg,
		out double argumentPerihelionDeg)
	{
		// Initialize output parameters
		semiMajorAxis = default;
		eccentricity = default;
		inclinationDeg = default;
		longitudeAscendingNodeDeg = default;
		argumentPerihelionDeg = default;
		// Use a consistent culture for parsing to ensure that decimal separators are handled correctly
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		// Try to parse each orbital element from the corresponding label
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out semiMajorAxis))
		{
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return false;
		}
		if (!double.TryParse(s: labelOrbitalEccentricityData.Text, style: NumberStyles.Any, provider: provider, result: out eccentricity))
		{
			logger.Error(message: $"Failed to parse eccentricity: '{labelOrbitalEccentricityData.Text}'");
			ShowErrorMessage(message: $"Could not parse eccentricity value: '{labelOrbitalEccentricityData.Text}'");
			return false;
		}
		if (!double.TryParse(s: labelInclinationToTheEclipticData.Text, style: NumberStyles.Any, provider: provider, result: out inclinationDeg))
		{
			logger.Error(message: $"Failed to parse inclination: '{labelInclinationToTheEclipticData.Text}'");
			ShowErrorMessage(message: $"Could not parse inclination value: '{labelInclinationToTheEclipticData.Text}'");
			return false;
		}
		if (!double.TryParse(s: labelLongitudeOfTheAscendingNodeData.Text, style: NumberStyles.Any, provider: provider, result: out longitudeAscendingNodeDeg))
		{
			logger.Error(message: $"Failed to parse longitude of ascending node: '{labelLongitudeOfTheAscendingNodeData.Text}'");
			ShowErrorMessage(message: $"Could not parse longitude of ascending node value: '{labelLongitudeOfTheAscendingNodeData.Text}'");
			return false;
		}
		if (!double.TryParse(s: labelArgumentOfThePerihelionData.Text, style: NumberStyles.Any, provider: provider, result: out argumentPerihelionDeg))
		{
			logger.Error(message: $"Failed to parse argument of perihelion: '{labelArgumentOfThePerihelionData.Text}'");
			ShowErrorMessage(message: $"Could not parse argument of perihelion value: '{labelArgumentOfThePerihelionData.Text}'");
			return false;
		}
		return true;
	}

	/// <summary>Shows the MOIDs form for the current planetoid.</summary>
	/// <remarks>Parses the orbital elements from the UI labels and opens the <see cref="MoidsOfOneMinorPlanetForm"/>.</remarks>
	private void ShowMoids()
	{
		// Try to parse the current orbital elements from the UI labels
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out double inclinationDeg,
			longitudeAscendingNodeDeg: out double longitudeAscendingNodeDeg,
			argumentPerihelionDeg: out double argumentPerihelionDeg))
		{
			return;
		}
		// Create a new instance of the MoidsOfOneMinorPlanetForm
		using MoidsOfOneMinorPlanetForm formMoids = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formMoids.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formMoids.SetOrbitalElements(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Show the MOIDs form as a modal dialog
		_ = formMoids.ShowDialog(owner: this);
	}

	/// <summary>Shows the MAXOIDs form for the current planetoid.</summary>
	/// <remarks>Parses the orbital elements from the UI labels and opens the <see cref="MaxoidsOfOneMinorPlanetForm"/>.</remarks>
	private void ShowMaxoids()
	{
		// Try to parse the current orbital elements from the UI labels
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out double inclinationDeg,
			longitudeAscendingNodeDeg: out double longitudeAscendingNodeDeg,
			argumentPerihelionDeg: out double argumentPerihelionDeg))
		{
			return;
		}
		// Create a new instance of the MaxoidsOfOneMinorPlanetForm
		using MaxoidsOfOneMinorPlanetForm formMaxoids = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formMaxoids.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formMaxoids.SetOrbitalElements(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Show the MAXOIDs form as a modal dialog
		_ = formMaxoids.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs and MAXOIDs form for the current planetoid.</summary>
	/// <remarks>Parses the orbital elements from the UI labels and opens the <see cref="MoidsAndMaxoidsOfOneMinorPlanetForm"/>.</remarks>
	private void ShowMoidsAndMaxoids()
	{
		// Try to parse the current orbital elements from the UI labels
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out double inclinationDeg,
			longitudeAscendingNodeDeg: out double longitudeAscendingNodeDeg,
			argumentPerihelionDeg: out double argumentPerihelionDeg))
		{
			return;
		}
		// Create a new instance of the MoidsAndMaxoidsOfOneMinorPlanetForm
		using MoidsAndMaxoidsOfOneMinorPlanetForm formMoidsAndMaxoids = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formMoidsAndMaxoids.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formMoidsAndMaxoids.SetOrbitalElements(
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg);
		// Show the MOIDs and MAXOIDs form as a modal dialog
		_ = formMoidsAndMaxoids.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs of all minor planets form. Opens the form to find MOIDs of all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowMoidsOfAllMinorPlanets()
	{
		// Create a new instance of the MoidsOfAllMinorPlanetsForm
		using MoidsOfAllMinorPlanetsForm formMoidsOfAll = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formMoidsOfAll.TopMost = TopMost;
		// Show the MOIDs of all minor planets form as a modal dialog
		_ = formMoidsOfAll.ShowDialog(owner: this);
	}

	/// <summary>Shows the MAXOIDs of all minor planets form. Opens the form to find MAXOIDs of all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowMaxoidsOfAllMinorPlanets()
	{
		// Create a new instance of the MaxoidsOfAllMinorPlanetsForm
		using MaxoidsOfAllMinorPlanetsForm formMaxoidsOfAll = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formMaxoidsOfAll.TopMost = TopMost;
		// Show the MAXOIDs of all minor planets form as a modal dialog
		_ = formMaxoidsOfAll.ShowDialog(owner: this);
	}

	/// <summary>Shows the histogram form. Opens the form to display histograms of orbital elements and properties of all minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can create histograms of various properties.</remarks>
	private void ShowHistogram()
	{
		// Create a new instance of the HistogramsForm
		using HistogramsForm formHistogram = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formHistogram.TopMost = TopMost;
		// Show the histogram form as a modal dialog
		_ = formHistogram.ShowDialog(owner: this);
	}

	/// <summary>Shows the scatterplots form. Opens the form to display scatterplots of orbital elements and properties of all minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can create scatterplots of various properties.</remarks>
	private void ShowScatterPlot()
	{
		// Create a new instance of the ScatterplotsForm
		using ScatterplotsForm formScatterplot = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formScatterplot.TopMost = TopMost;
		// Show the scatterplots form as a modal dialog
		_ = formScatterplot.ShowDialog(owner: this);
	}

	/// <summary>Shows the orbit visualization form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis, eccentricity, and argument of perihelion from the UI labels and opens the <see cref="Orbit2DTopViewForm"/>.</remarks>
	private void ShowOrbit2DTopView()
	{
		// Use the TryParseCurrentOrbitalElements method to parse the necessary orbital elements from the UI labels.
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out _,
			longitudeAscendingNodeDeg: out _,
			argumentPerihelionDeg: out double argumentPerihelionDeg))
		{
			return;
		}
		// Use the readable designation as the planetoid label in the diagram title.
		string planetoidName = labelReadableDesignationData.Text;
		// Create a new instance of the Orbit2DTopViewForm and show it as a modal dialog.
		using Orbit2DTopViewForm formOrbit2DTopView = new(
			planetoidName: planetoidName,
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			argumentPerihelionDeg: argumentPerihelionDeg);
		formOrbit2DTopView.TopMost = TopMost;
		_ = formOrbit2DTopView.ShowDialog(owner: this);
	}

	/// <summary>Shows the 2D side-view orbit diagram for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis, eccentricity, and inclination from the UI labels and opens the <see cref="Orbit2DSideViewForm"/>.</remarks>
	private void ShowOrbit2DSideView()
	{
		// Use the TryParseCurrentOrbitalElements method to parse the necessary orbital elements from the UI labels.
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out double inclinationDeg,
			longitudeAscendingNodeDeg: out _,
			argumentPerihelionDeg: out _))
		{
			return;
		}
		// Use the readable designation as the planetoid label in the diagram title.
		string planetoidName = labelReadableDesignationData.Text;
		// Create a new instance of the Orbit2DSideViewForm and show it as a modal dialog.
		using Orbit2DSideViewForm formOrbit2DSideView = new(
			planetoidName: planetoidName,
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg);
		formOrbit2DSideView.TopMost = TopMost;
		_ = formOrbit2DSideView.ShowDialog(owner: this);
	}

	/// <summary>Shows the 3D orbit visualization for the current planetoid.</summary>
	/// <remarks>Parses all six Keplerian orbital elements plus the mean anomaly and the MPCORB epoch from the UI labels and opens the <see cref="Orbit3DForm"/>.</remarks>
	private void ShowOrbit3DView()
	{
		// Use the TryParseCurrentOrbitalElements method to parse the necessary orbital elements from the UI labels.
		if (!TryParseCurrentOrbitalElements(
			semiMajorAxis: out double semiMajorAxis,
			eccentricity: out double eccentricity,
			inclinationDeg: out double inclinationDeg,
			longitudeAscendingNodeDeg: out double longitudeAscendingNodeDeg,
			argumentPerihelionDeg: out double argumentPerihelionDeg))
		{
			return;
		}
		// Parse the mean anomaly at the epoch from the corresponding label on the form
		IFormatProvider provider = CultureInfo.InvariantCulture;
		// If parsing fails, log the error and show an error message to the user, then return early to avoid opening the form with invalid data
		if (!double.TryParse(s: labelMeanAnomalyAtTheEpochData.Text, style: NumberStyles.Any, provider: provider, result: out double meanAnomalyDeg))
		{
			logger.Error(message: $"Failed to parse mean anomaly: '{labelMeanAnomalyAtTheEpochData.Text}'");
			ShowErrorMessage(message: $"Could not parse mean anomaly value: '{labelMeanAnomalyAtTheEpochData.Text}'");
			return;
		}
		// Use the readable designation as the planetoid label in the diagram title.
		string planetoidName = labelReadableDesignationData.Text;
		// Parse the epoch from the corresponding label on the form
		string epochMpcorb = labelEpochData.Text;
		// Create a new instance of the Orbit3DForm and show it as a modal dialog.
		using Orbit3DForm formOrbit3D = new(
			planetoidName: planetoidName,
			semiMajorAxis: semiMajorAxis,
			eccentricity: eccentricity,
			inclinationDeg: inclinationDeg,
			longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			argumentPerihelionDeg: argumentPerihelionDeg,
			meanAnomalyDeg: meanAnomalyDeg,
			epochMpcorb: epochMpcorb);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formOrbit3D.TopMost = TopMost;
		// Show the 3D orbit visualization form as a modal dialog
		_ = formOrbit3D.ShowDialog(owner: this);
	}

	/// <summary>Shows the Tisserand parameters form for the current planetoid.</summary>
	/// <remarks>Parses the semi-major axis, eccentricity, and inclination from the UI labels and opens the <see cref="TisserandParameterOfOneMinorPlanetForm"/>.</remarks>
	private void ShowTisserandParameters()
	{
		// Create a culture-specific format provider for parsing the orbital elements
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		// Parse the semi-major axis from the corresponding label on the form
		if (!double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse semi-major axis: '{labelSemiMajorAxisData.Text}'");
			ShowErrorMessage(message: $"Could not parse semi-major axis value: '{labelSemiMajorAxisData.Text}'");
			return;
		}
		// Parse the eccentricity from the corresponding label on the form
		if (!double.TryParse(s: labelOrbitalEccentricityData.Text, style: NumberStyles.Any, provider: provider, result: out double eccentricity))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse eccentricity: '{labelOrbitalEccentricityData.Text}'");
			ShowErrorMessage(message: $"Could not parse eccentricity value: '{labelOrbitalEccentricityData.Text}'");
			return;
		}
		// Parse the inclination to the ecliptic from the corresponding label on the form
		if (!double.TryParse(s: labelInclinationToTheEclipticData.Text, style: NumberStyles.Any, provider: provider, result: out double inclinationDeg))
		{
			// If parsing fails, log the error and show an error message to the user
			logger.Error(message: $"Failed to parse inclination: '{labelInclinationToTheEclipticData.Text}'");
			ShowErrorMessage(message: $"Could not parse inclination value: '{labelInclinationToTheEclipticData.Text}'");
			return;
		}
		// Create a new instance of the TisserandParameterOfOneMinorPlanetForm
		using TisserandParameterOfOneMinorPlanetForm formTisserand = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formTisserand.TopMost = TopMost;
		// Pass the parsed orbital elements to the form
		formTisserand.SetOrbitalElements(semiMajorAxis: semiMajorAxis, eccentricity: eccentricity, inclinationDeg: inclinationDeg);
		// Show the Tisserand parameters form as a modal dialog
		_ = formTisserand.ShowDialog(owner: this);
	}

	/// <summary>Shows the Tisserand parameters of all minor planets form. Opens the form to compute Tisserand parameters for all planetoids relative to the solar system planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can iterate over all records.</remarks>
	private void ShowTisserandParametersOfAllMinorPlanets()
	{
		// Create a new instance of the TisserandParameterOfAllMinorPlanetsForm
		using TisserandParameterOfAllMinorPlanetsForm formTisserandOfAll = new(planetoids: planetoidsDatabase);
		formTisserandOfAll.TopMost = TopMost;
		_ = formTisserandOfAll.ShowDialog(owner: this);
	}

	/// <summary>Shows the bulk observations data downloader form. Opens the form to download observation data files for a range of minor planets from the MPC website and save them to disk.</summary>
	/// <remarks>Passes the full planetoids database to the form and pre-populates the minimum (1) and maximum (database record count) spinners.</remarks>
	private void ShowBulkObservationDataDownloader()
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
			return;
		}
		using BulkObservationsDataDownloaderForm formBulkDownloader = new(planetoids: planetoidsDatabase);
		formBulkDownloader.TopMost = TopMost;
		formBulkDownloader.SetMinimum(minimum: 1);
		formBulkDownloader.SetMaximum(maximum: planetoidsDatabase.Count);
		_ = formBulkDownloader.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs relative to minor planets form. Opens the form to calculate the MOID between two user-selected minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can populate the combo boxes with all available planetoid designations.</remarks>
	private void ShowMoidsRelativeToMinorPlanets()
	{
		// Create a new instance of the MoidsRelativeToMinorPlanetsForm
		using MoidsRelativeToMinorPlanetsForm formMoidsRelative = new(planetoids: planetoidsDatabase);
		formMoidsRelative.TopMost = TopMost;
		_ = formMoidsRelative.ShowDialog(owner: this);
	}

	/// <summary>Shows the MAXOIDs relative to minor planets form. Opens the form to calculate the MAXOID between two user-selected minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can populate the combo boxes with all available planetoid designations.</remarks>
	private void ShowMaxoidsRelativeToMinorPlanets()
	{
		// Create a new instance of the MaxoidsRelativeToMinorPlanetsForm
		using MaxoidsRelativeToMinorPlanetsForm formMaxoidsRelative = new(planetoids: planetoidsDatabase);
		formMaxoidsRelative.TopMost = TopMost;
		_ = formMaxoidsRelative.ShowDialog(owner: this);
	}

	/// <summary>Shows the application information form.</summary>
	/// <remarks>This method is used to show the application information form.</remarks>
	private void ShowAppInfo()
	{
		// Create a new instance of the AppInfoForm
		using AppInfoForm formAppInfo = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formAppInfo.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formAppInfo.ShowDialog(owner: this);
	}

	/// <summary>Displays the archive form as a modal dialog, ensuring it remains on top of other windows.</summary>
	/// <remarks>This method creates an instance of the ArchiveMpcorbForm and sets its TopMost property to true, which keeps the form above other application windows. The form is shown modally, meaning the user must interact with it before returning to the main application.</remarks>
	private void ShowArchive()
	{
		// Create a new instance of the ArchiveMpcorbForm
		using ArchiveMpcorbForm formArchive = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formArchive.TopMost = TopMost;
		// Show the archive form as a modal dialog
		_ = formArchive.ShowDialog(owner: this);
	}

	/// <summary>Displays the archive comparison form as a modal dialog, allowing users to view differences between database archives.</summary>
	/// <remarks>The form is set to remain on top of other windows while it is open, ensuring that users can easily interact with it without losing focus.</remarks>
	private void ShowCompareArchives()
	{
		// Create a new instance of the DatabaseDifferencesForm
		using DatabaseDifferencesForm formDataDifferences = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formDataDifferences.TopMost = TopMost;
		// Show the archive form as a modal dialog
		_ = formDataDifferences.ShowDialog(owner: this);
	}

	/// <summary>Shows the license form.</summary>
	/// <remarks>This method is used to show the license form.</remarks>
	private void ShowLicense()
	{
		// Create a new instance of the LicenseForm
		using LicenseForm formLicense = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formLicense.TopMost = TopMost;
		// Show the application information form as a modal dialog
		_ = formLicense.ShowDialog(owner: this);
	}

	/// <summary>Shows the records form that scans all orbital elements for maximum or minimum record values.</summary>
	/// <remarks>This method creates the <see cref="RecordsForm"/>, passes a copy of the current planetoid database, and displays the form as a modal dialog.</remarks>
	private void ShowRecords()
	{
		// Create a new instance of the RecordsForm
		using RecordsForm formRecords = new();
		// Pass a copy of the current database to the form
		formRecords.FillArray(arrTemp: planetoidsDatabase);
		// Set the TopMost property to keep the form on top of other windows
		formRecords.TopMost = TopMost;
		// Show the records form as a modal dialog
		_ = formRecords.ShowDialog(owner: this);
	}

	/// <summary>Shows the top ten records form for the specified orbital element.</summary>
	/// <param name="selectedElement">The orbital element to preselect in the form, or <see langword="null"/> to keep the default selection.</param>
	/// <remarks>This method creates the <see cref="RecordsTop10Form"/>, passes a copy of the current planetoid database, and displays the form as a modal dialog.</remarks>
	private void ShowRecordsTop10(string? selectedElement = null)
	{
		// Create a new instance of the RecordsTop10Form
		using RecordsTop10Form formRecordsTop10 = new(arrTemp: planetoidsDatabase, selectedElement: selectedElement);
		// Set the TopMost property to keep the form on top of other windows
		formRecordsTop10.TopMost = TopMost;
		// Show the records form as a modal dialog
		_ = formRecordsTop10.ShowDialog(owner: this);
	}


	/// <summary>Shows the MPCORB data check form.</summary>
	/// <remarks>This method is used to check the MPCORB data for updates.</remarks>
	private async void ShowMpcorbDatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		//if (!await HasInternetAsync(client: httpClient, url: uriMpcorb.OriginalString))
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the MPCORB data check form
			using CheckDatabaseForm formCheckMpcorbDat = new(url: Settings.Default.systemMpcorbDatUrl, localFilePath: Settings.Default.systemFilenameMpcorbDat, databaseName: "MPCORB.DAT");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckMpcorbDat.TopMost = TopMost;
			// Show the MPCORB data check form as a modal dialog
			_ = formCheckMpcorbDat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the ASTORB data check form.</summary>
	/// <remarks>This method is used to check the ASTORB data for updates.</remarks>
	private void ShowAstorbDatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the ASTORB data check form
			using CheckDatabaseForm formCheckAstorbDat = new(url: Settings.Default.systemAstorbDatUrl, localFilePath: Settings.Default.systemFilenameAstorbDat, databaseName: "ASTORB.DAT");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckAstorbDat.TopMost = TopMost;
			// Show the ASTORB data check form as a modal dialog
			_ = formCheckAstorbDat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the ALLNUM.CAT data check form.</summary>
	/// <remarks>This method is used to check the ALLNUM.CAT data for updates.</remarks>
	private void ShowAllnumCatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the ALLNUM.CAT data check form
			using CheckDatabaseForm formCheckAllnumCat = new(url: Settings.Default.systemAllnumCatUrl, localFilePath: Settings.Default.systemFilenameAllnumCat, databaseName: "allnum.cat");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckAllnumCat.TopMost = TopMost;
			// Show the ALLNUM.CAT data check form as a modal dialog
			_ = formCheckAllnumCat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the UFITOBS.CAT data check form.</summary>
	/// <remarks>This method is used to check the UFITOBS.CAT data for updates.</remarks>
	private void ShowUfitobsCatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the UFITOBS.CAT data check form
			using CheckDatabaseForm formCheckUfitobsCat = new(url: Settings.Default.systemUfitobsCatUrl, localFilePath: Settings.Default.systemFilenameUfitobsCat, databaseName: "ufitobs.cat");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckUfitobsCat.TopMost = TopMost;
			// Show the UFITOBS.CAT data check form as a modal dialog
			_ = formCheckUfitobsCat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the SINGOPP.CAT data check form.</summary>
	/// <remarks>This method is used to check the SINGOPP.CAT data for updates.</remarks>
	private void ShowSingoppCatUpdateCheck()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the SINGOPP.CAT data check form
			using CheckDatabaseForm formCheckSingoppCat = new(url: Settings.Default.systemSingoppCatUrl, localFilePath: Settings.Default.systemFilenameSingoppCat, databaseName: "singopp.cat");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckSingoppCat.TopMost = TopMost;
			// Show the SINGOPP.CAT data check form as a modal dialog
			_ = formCheckSingoppCat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the downloader form for the MPCORB database.</summary>
	/// <remarks>This method is used to show the downloader form for the MPCORB database.</remarks>
	private void ShowMpcorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the MPCORB database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemMpcorbDatGzUrl);
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog; pass 'this' as owner so Windows
			// returns focus here when the dialog closes.
			if (downloaderForm.ShowDialog(owner: this) == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowMpcorbDatUpdateIsAvailable.Enabled = false;
				toolStripStatusLabelMpcorbDatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the downloader form for the ASTORB database.</summary>
	/// <remarks>This method is used to show the downloader form for the ASTORB database.</remarks>
	private void ShowAstorbDatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the ASTORB database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemAstorbDatGzUrl);
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog; pass 'this' as owner so Windows
			// returns focus here when the dialog closes.
			if (downloaderForm.ShowDialog(owner: this) == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowAstorbDatUpdateIsAvailable.Enabled = false;
				toolStripStatusLabelAstorbDatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the downloader form for the ALLNUM.CAT database.</summary>
	/// <remarks>This method is used to show the downloader form for the ALLNUM.CAT database.</remarks>
	private void ShowAllnumCatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowAllnumCatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the ALLNUM.CAT database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemAllnumCatUrl);
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog; pass 'this' as owner so Windows
			// returns focus here when the dialog closes.
			if (downloaderForm.ShowDialog(owner: this) == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowAllnumCatUpdateIsAvailable.Enabled = false;
				//toolStripStatusLabelAllnumCatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the downloader form for the UFITOBS.CAT database.</summary>
	/// <remarks>This method is used to show the downloader form for the UFITOBS.CAT database.</remarks>
	private void ShowUfitobsCatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowUfitobsCatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the UFITOBS.CAT database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemUfitobsCatUrl);
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog; pass 'this' as owner so Windows
			// returns focus here when the dialog closes.
			if (downloaderForm.ShowDialog(owner: this) == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowUfitobsCatUpdateIsAvailable.Enabled = false;
				//toolStripStatusLabelUfitobsCatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the downloader form for the SINGOPP.CAT database.</summary>
	/// <remarks>This method is used to show the downloader form for the SINGOPP.CAT database.</remarks>
	private void ShowSingoppCatDownloader()
	{
		// Check if the network is available before proceeding with the download
		if (!NetworkInterface.GetIsNetworkAvailable())
		{
			// Disable the menu item for showing updates is available
			toolStripMenuItemShowSingoppCatUpdateIsAvailable.Enabled = false;
			// Display an error message if the network is not available
			ShowErrorMessage(message: I18nStrings.NoInternetConnectionText);
		}
		else
		{
			// Create and show the downloader form for the SINGOPP.CAT database
			using DatabaseDownloaderForm downloaderForm = new(url: Settings.Default.systemSingoppCatUrl);
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			downloaderForm.TopMost = TopMost;
			// Show the downloader form as a modal dialog; pass 'this' as owner so Windows
			// returns focus here when the dialog closes.
			if (downloaderForm.ShowDialog(owner: this) == DialogResult.OK)
			{
				// Disable the menu item and the status label for showing updates is available
				toolStripMenuItemShowSingoppCatUpdateIsAvailable.Enabled = false;
				//toolStripStatusLabelSingoppCatUpdate.Enabled = false;
				// Ask the user if they want to restart the application after downloading the database
				AskForRestartAfterDownloadingDatabase();
			}
		}
	}

	/// <summary>Shows the database information form.</summary>
	/// <remarks>This method is used to show the database information form.</remarks>
	private void ShowDatabaseInformation()
	{
		// Create a new instance of the DatabaseInformationForm
		using DatabaseInformationForm formDatabaseInformation = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formDatabaseInformation.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formDatabaseInformation.ShowDialog(owner: this);
	}

	/// <summary>Shows the search form.</summary>
	///	<remarks>This method is used to show the search form.</remarks>
	private void ShowSearch()
	{
		// Create a new instance of the SearchForm
		using SearchForm formSearch = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formSearch.TopMost = TopMost;

		_ = formSearch.ShowDialog(owner: this);

	}

	/// <summary>Shows the filter form.</summary>
	/// <remarks>This method passes a copy of the current planetoids database to the filter form. When the user confirms the filter settings, the filtered result replaces the current database and the view is refreshed to the first record.</remarks>
	private void ShowFilter()
	{
		// Create a new instance of the FilterForm
		using FilterForm formFilter = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formFilter.TopMost = TopMost;
		// Pass a copy of the current database to the filter form
		formFilter.FillArray(arrTemp: planetoidsDatabase);
		// Show the filter form as a modal dialog
		if (formFilter.ShowDialog(owner: this) == DialogResult.OK && formFilter.FilteredDatabase is { } filtered)
		{
			// Replace the current database with the filtered result
			planetoidsDatabase.Clear();
			planetoidsDatabase.AddRange(collection: filtered);
			// Navigate to the first record of the filtered database
			currentPosition = 0;
			GotoCurrentPosition(position: currentPosition);
			logger.Info(message: $"Filter applied: database now contains {planetoidsDatabase.Count} records.");
		}
	}

	/// <summary>Shows the settings form.</summary>
	/// <remarks>This method is used to show the settings form.</remarks>
	private void ShowSettings()
	{
		// Create a new instance of the SettingsForm
		using SettingsForm formSettings = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formSettings.TopMost = TopMost;
		// Fill the form with the planetoids database
		_ = formSettings.ShowDialog(owner: this);
	}

	/// <summary>Lists readable designations.</summary>
	/// <remarks>This method is used to show the list of readable designations.</remarks>
	private void ListReadableDesignations()
	{
		// Create a new instance of the ListReadableDesignationsForm
		using ListReadableDesignationsForm formListReadableDesignations = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formListReadableDesignations.TopMost = TopMost;
		// Fill the form with the planetoids database
		formListReadableDesignations.FillArray(arrTemp: planetoidsDatabase);
		// Set the maximum index for the form
		formListReadableDesignations.SetMaxIndex(maxIndex: planetoidsDatabase.Count);
		// Show the list readable designations form as a modal dialog
		_ = formListReadableDesignations.ShowDialog(owner: this);
		// Check if the dialog result is OK and the selected index is greater than 0
		if (formListReadableDesignations.DialogResult == DialogResult.OK && formListReadableDesignations.GetSelectedIndex() > 0)
		{
			// Navigate to the current position in the database
			GotoCurrentPosition(position: formListReadableDesignations.GetSelectedIndex());
		}
	}

	/// <summary>Exports the data sheet.</summary>
	///	<remarks>This method is used to export the data sheet.</remarks>
	private void ExportDataSheet()
	{
		// Create a new list to store the orbital elements
		List<string> orbitalElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentAphelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		orbitalElements.Add(item: labelIndexData.Text);
		orbitalElements.Add(item: labelReadableDesignationData.Text);
		orbitalElements.Add(item: labelEpochData.Text);
		orbitalElements.Add(item: labelMeanAnomalyAtTheEpochData.Text);
		orbitalElements.Add(item: labelArgumentOfThePerihelionData.Text);
		orbitalElements.Add(item: labelLongitudeOfTheAscendingNodeData.Text);
		orbitalElements.Add(item: labelInclinationToTheEclipticData.Text);
		orbitalElements.Add(item: labelOrbitalEccentricityData.Text);
		orbitalElements.Add(item: labelMeanDailyMotionData.Text);
		orbitalElements.Add(item: labelSemiMajorAxisData.Text);
		orbitalElements.Add(item: labelAbsoluteMagnitudeData.Text);
		orbitalElements.Add(item: labelSlopeParameterData.Text);
		orbitalElements.Add(item: labelReferenceData.Text);
		orbitalElements.Add(item: labelNumberOfOppositionsData.Text);
		orbitalElements.Add(item: labelNumberOfObservationsData.Text);
		orbitalElements.Add(item: labelObservationSpanData.Text);
		orbitalElements.Add(item: labelRmsResidualData.Text);
		orbitalElements.Add(item: labelComputerNameData.Text);
		orbitalElements.Add(item: labelFlagsData.Text);
		orbitalElements.Add(item: labelDateLastObservationData.Text);
		orbitalElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		// Create a new instance of the ExportDataSheetForm
		using ExportDataSheetForm formExportDataSheet = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formExportDataSheet.TopMost = TopMost;
		// Fill the form with the orbital elements
		formExportDataSheet.SetDatabase(list: [.. orbitalElements.Cast<string>()]);
		// Show the export data sheet form as a modal dialog
		_ = formExportDataSheet.ShowDialog(owner: this);
	}

	/// <summary>Shows the print data sheet form.</summary>
	/// <remarks>This method is used to show the print data sheet form.</remarks>
	private void PrintDataSheet()
	{
		// Create a new list to store the orbital elements
		List<string> orbitalElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentAphelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		orbitalElements.Add(item: labelIndexData.Text);
		orbitalElements.Add(item: labelReadableDesignationData.Text);
		orbitalElements.Add(item: labelEpochData.Text);
		orbitalElements.Add(item: labelMeanAnomalyAtTheEpochData.Text);
		orbitalElements.Add(item: labelArgumentOfThePerihelionData.Text);
		orbitalElements.Add(item: labelLongitudeOfTheAscendingNodeData.Text);
		orbitalElements.Add(item: labelInclinationToTheEclipticData.Text);
		orbitalElements.Add(item: labelOrbitalEccentricityData.Text);
		orbitalElements.Add(item: labelMeanDailyMotionData.Text);
		orbitalElements.Add(item: labelSemiMajorAxisData.Text);
		orbitalElements.Add(item: labelAbsoluteMagnitudeData.Text);
		orbitalElements.Add(item: labelSlopeParameterData.Text);
		orbitalElements.Add(item: labelReferenceData.Text);
		orbitalElements.Add(item: labelNumberOfOppositionsData.Text);
		orbitalElements.Add(item: labelNumberOfObservationsData.Text);
		orbitalElements.Add(item: labelObservationSpanData.Text);
		orbitalElements.Add(item: labelRmsResidualData.Text);
		orbitalElements.Add(item: labelComputerNameData.Text);
		orbitalElements.Add(item: labelFlagsData.Text);
		orbitalElements.Add(item: labelDateLastObservationData.Text);
		orbitalElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		orbitalElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));

		// Create a new instance of the PrintDataSheetForm
		using PrintDataSheetForm formPrintDataSheet = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formPrintDataSheet.TopMost = TopMost;
		// Fill the form with the planetoids database
		formPrintDataSheet.SetDatabase(db: [.. orbitalElements]);
		_ = formPrintDataSheet.ShowDialog(owner: this);
	}

	/// <summary>Shows the derived orbit elements form.</summary>
	/// <remarks>This method is used to show the derived orbit elements form.</remarks>
	private void ShowDerivedOrbitElements()
	{
		// Create a new list to store the derived orbit elements
		List<string> derivedOrbitElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentPerihelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		double inclination = double.Parse(s: labelInclinationToTheEclipticData.Text, provider: provider);
		double absoluteMagnitude = double.Parse(s: labelAbsoluteMagnitudeData.Text, provider: provider);

		// Calculate true anomaly for velocity and energy calculations
		double trueAnomaly = DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8);

		// Original 19 elements
		derivedOrbitElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8).ToString(provider: provider));
		derivedOrbitElements.Add(item: trueAnomaly.ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentPerihelion).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));

		// New 22 elements
		derivedOrbitElements.Add(item: DerivedElements.CalculateDirectrix(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculatePerihelionVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateAphelionVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanOrbitalVelocity(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateCurrentOrbitalVelocity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateRadialVelocityComponent(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTangentialVelocityComponent(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSpecificOrbitalEnergy(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSpecificAngularMomentum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateVisVivaEnergy(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateLongitudeOfPerihelion(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanLongitude(longitudeAscendingNode: longitudeAscendingNode, argumentPerihelion: argumentPerihelion, meanAnomaly: meanAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateArgumentOfLatitude(argumentPerihelion: argumentPerihelion, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateFlightPathAngle(numericalEccentricity: numericalEccentricity, trueAnomaly: trueAnomaly).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeSincePerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeToNextPerihelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeSinceAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTimeToNextAphelion(meanAnomaly: meanAnomaly, semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSynodicPeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateTisserandParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity, inclination: inclination).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMeanDistanceFromFocus(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		// Assume standard albedo of 0.154 for C-type asteroids if not specified
		derivedOrbitElements.Add(item: DerivedElements.CalculateGeometricAlbedoAdjustedDiameter(absoluteMagnitude: absoluteMagnitude, geometricAlbedo: 0.154).ToString(provider: provider));

		// Create a new instance of the DerivedOrbitElementsForm
		using DerivedOrbitElementsForm formDerivedOrbitElements = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formDerivedOrbitElements.TopMost = TopMost;
		// Fill the form with the derived orbit elements
		formDerivedOrbitElements.SetDatabase(list: [.. derivedOrbitElements.Cast<object>()]);
		// Show the derived orbit elements form as a modal dialog
		_ = formDerivedOrbitElements.ShowDialog(owner: this);
	}

	/// <summary>Checks if the form should stay on top of other windows.</summary>
	/// <remarks>This method is used to check if the form should stay on top of other windows.</remarks>
	private void CheckStayOnTop() => TopMost = toolStripMenuItemOptionStayOnTop.Checked;

	/// <summary>Displays the form's <see cref="openFileDialog"/> to allow the user to choose a local MPCORB.DAT file and restarts the application to load the selected file if confirmed.</summary>
	/// <remarks>Uses the pre-configured <see cref="openFileDialog"/> component. If the user selects a valid, non-empty file, the application prompts for confirmation and restarts with the selected file as a command-line argument. If the file is invalid or empty, an error message is shown and the operation is aborted. This method is intended for scenarios where the user needs to manually specify a new MPCORB.DAT data source.</remarks>
	private void OpenLocalMpcorbDat()
	{
		// Show the dialog and check if the user selected a file
		if (openFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			return;
		}
		// Get the selected file path
		string selectedFilePath = openFileDialog.FileName;
		// Validate the selected file
		if (string.IsNullOrWhiteSpace(value: selectedFilePath) || !File.Exists(path: selectedFilePath))
		{
			logger.Error(message: $"Selected file does not exist: {selectedFilePath}");
			ShowErrorMessage(message: "The selected file does not exist.");
			return;
		}
		// Check if the file has content
		FileInfo fileInfo = new(fileName: selectedFilePath);
		if (fileInfo.Length == 0)
		{
			logger.Error(message: $"Selected file is empty: {selectedFilePath}");
			ShowErrorMessage(message: "The selected file is empty.");
			return;
		}
		// If the file is valid, prompt the user to confirm restarting the application to load the new file
		try
		{
			logger.Info(message: $"User selected local MPCORB.DAT file: {selectedFilePath}");
			// Ask the user if they want to restart the application
			DialogResult result = KryptonMessageBox.Show(
				owner: this,
				text: $"The application will restart to load the selected file:\n\n{selectedFilePath}\n\nDo you want to continue?",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.YesNo,
				icon: KryptonMessageBoxIcon.Question,
				defaultButton: KryptonMessageBoxDefaultButton.Button1);
			// If the user confirms, restart the application with the new file path as a command line argument
			if (result == DialogResult.Yes)
			{
				logger.Info(message: "Restarting application to load new MPCORB.DAT file");
				// Restart the application with the new file path as command line argument
				ProcessStartInfo startInfo = new()
				{
					FileName = Application.ExecutablePath,
					Arguments = $"\"{selectedFilePath}\"",
					UseShellExecute = true
				};
				_ = Process.Start(startInfo: startInfo);
				// Close the current application instance
				Application.Exit();
			}
		}
		// Handle any exceptions that may occur during the file selection and application restart process
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error while opening local MPCORB.DAT file: {ex.Message}");
			ShowErrorMessage(message: $"Error while opening the file:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes the 4-hexdigit flag from MPCORB.DAT and displays the result in a KryptonMessageBox.</summary>
	/// <remarks>The flag encodes orbit type in the lower 6 bits and additional information in bits 6-15 according to MPC specifications.</remarks>
	private void DecodeMpcorbFlags()
	{
		// Get the flag text from the label
		string flagText = labelFlagsData.Text;
		// Validate that the flag text is not empty
		if (string.IsNullOrWhiteSpace(value: flagText))
		{
			logger.Warn(message: "Flag text is empty or whitespace");
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "No flag data available.",
				caption: "Flag Decoder",
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Validate that the flag text is a valid 4-hexdigit string
		try
		{
			// Parse the hex string to an integer
			int flagValue = Convert.ToInt32(value: flagText, fromBase: 16);
			// Extract orbit type (lower 6 bits)
			int orbitType = flagValue & 0x3F; // 0x3F = 0011 1111 (bits 0-5)
											  // Extract individual flag bits
			bool isNeo = (flagValue & 2048) != 0;          // Bit 11
			bool isLargeNeo = (flagValue & 4096) != 0;     // Bit 12
			bool isOneOppObject = (flagValue & 8192) != 0; // Bit 13
			bool isCriticalList = (flagValue & 16384) != 0;// Bit 14
			bool isPha = (flagValue & 32768) != 0;         // Bit 15
														   // Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: $"MPCORB Flag Decoder");
			_ = result.AppendLine(value: $"==================");
			_ = result.AppendLine(value: $"Hex Value: {flagText}");
			_ = result.AppendLine(value: $"Decimal Value: {flagValue}");
			_ = result.AppendLine();
			// Orbit type classification
			_ = result.AppendLine(value: "Orbit Classification:");
			string orbitTypeName = orbitType switch
			{
				1 => "Atira",
				2 => "Aten",
				3 => "Apollo",
				4 => "Amor",
				5 => "Object with q < 1.665 AU",
				6 => "Hungaria",
				7 => "Unused or internal MPC use only",
				8 => "Hilda",
				9 => "Jupiter Trojan",
				10 => "Distant object",
				_ => $"Undefined (value: {orbitType})"
			};
			_ = result.AppendLine(value: $"  {orbitTypeName}");
			_ = result.AppendLine();
			// Additional flags
			_ = result.AppendLine(value: "Additional Flags:");
			if (isNeo)
			{
				_ = result.AppendLine(value: "  ✓ Near-Earth Object (NEO)");
			}
			if (isLargeNeo)
			{
				_ = result.AppendLine(value: "  ✓ 1-km (or larger) NEO");
			}
			if (isOneOppObject)
			{
				_ = result.AppendLine(value: "  ✓ 1-opposition object seen at earlier opposition");
			}
			if (isCriticalList)
			{
				_ = result.AppendLine(value: "  ✓ Critical list numbered object");
			}
			if (isPha)
			{
				_ = result.AppendLine(value: "  ✓ Potentially Hazardous Asteroid (PHA)");
			}
			// If no additional flags are set
			if (!isNeo && !isLargeNeo && !isOneOppObject && !isCriticalList && !isPha)
			{
				_ = result.AppendLine(value: "  (none)");
			}
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "MPCORB Flag Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);

			logger.Info(message: $"Decoded MPCORB flag: {flagText} = {flagValue} ({orbitTypeName})");
		}
		// Handle format exceptions when parsing the hex string
		catch (FormatException ex)
		{
			logger.Error(exception: ex, message: $"Failed to parse flag value '{flagText}': {ex.Message}");
			ShowErrorMessage(message: $"Failed to parse flag value '{flagText}'.\n\nThe flag must be a valid hexadecimal number.\n\nError: {ex.Message}");
		}
		// Handle overflow exceptions when the hex value is too large to fit in an integer
		catch (OverflowException ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB flag: {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the flag.\n\nError: {ex.Message}");
		}
	}

	/// <summary>Decodes the compressed reference code from MPCORB.DAT and displays the full reference in a KryptonMessageBox.</summary>
	/// <remarks>Decodes various reference formats according to MPC specifications at http://www.minorplanetcenter.org/iau/info/References.html</remarks>
	private void DecodeMpcorbReference()
	{
		// Get the reference text from the label
		string referenceText = labelReferenceData.Text;
		// Validate that the reference text is not empty
		if (string.IsNullOrWhiteSpace(value: referenceText))
		{
			logger.Warn(message: "Reference text is empty or whitespace");
			_ = KryptonMessageBox.Show(owner: this, text: "No reference data available.", caption: "Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Warning);
			return;
		}
		// Attempt to decode the reference and handle any exceptions that may occur during decoding
		try
		{
			string decodedReference = DecodeReference(compressedRef: referenceText.Trim());
			// Build the result message
			System.Text.StringBuilder result = new();
			_ = result.AppendLine(value: "MPCORB Reference Decoder");
			_ = result.AppendLine(value: "========================");
			_ = result.AppendLine(value: $"Compressed: {referenceText}");
			_ = result.AppendLine();
			_ = result.AppendLine(value: "Full Reference:");
			_ = result.AppendLine(value: $"  {decodedReference}");
			// Display the result in a KryptonMessageBox
			_ = KryptonMessageBox.Show(owner: this, text: result.ToString(), caption: "MPCORB Reference Decoder", buttons: KryptonMessageBoxButtons.OK, icon: KryptonMessageBoxIcon.Information);
			logger.Info(message: $"Decoded MPCORB reference: '{referenceText}' → '{decodedReference}'");
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"Error decoding MPCORB reference '{referenceText}': {ex.Message}");
			ShowErrorMessage(message: $"An error occurred while decoding the reference:\n\n{ex.Message}");
		}
	}

	/// <summary>Decodes a compressed MPC reference string to its full form.</summary>
	/// <param name="compressedRef">The compressed reference string (typically 5 characters).</param>
	/// <returns>The full reference description.</returns>
	private static string DecodeReference(string compressedRef)
	{
		if (string.IsNullOrWhiteSpace(value: compressedRef))
		{
			return "Unknown reference";
		}
		// Ensure the reference is exactly 5 characters for proper parsing
		compressedRef = compressedRef.PadRight(totalWidth: 5);
		char firstChar = compressedRef[index: 0];
		// 1: Temporary MPEC References (E + half-month + number)
		if (firstChar == 'E')
		{
			string halfMonth = compressedRef.Substring(startIndex: 1, length: 1);
			string circularNumber = compressedRef.Substring(startIndex: 2, length: 3).TrimStart(trimChar: '0');
			return $"MPEC (temporary) - Half-month {halfMonth}, Circular {circularNumber}";
		}
		// 2A: Five-digit MPC numbers (00001-99999)
		if (char.IsDigit(c: firstChar) && compressedRef.All(predicate: c => char.IsDigit(c: c) || char.IsWhiteSpace(c: c)))
		{
			if (int.TryParse(s: compressedRef.Trim(), result: out int mpcNumber))
			{
				return $"Minor Planet Circular (MPC) {mpcNumber}";
			}
		}
		// 2B: @ + four digits (MPC 100000-109999)
		if (firstChar == '@')
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int excess))
			{
				return $"Minor Planet Circular (MPC) {100000 + excess}";
			}
		}
		// 2C: # + four Base-62 characters (MPC 110000+)
		if (firstChar == '#')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Circular (MPC) {110000 + value}";
		}
		// 2D: Lowercase letter + four digits (MPS)
		if (char.IsLower(c: firstChar))
		{
			int multiplier = firstChar - 'a';
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int remainder))
			{
				int mpsNumber = (multiplier * 10000) + remainder;
				return $"Minor Planet Supplement (MPS) {mpsNumber}";
			}
		}
		// 2E: Tilde + four Base-62 characters (MPS 260000+)
		if (firstChar == '~')
		{
			string base62 = compressedRef.Substring(startIndex: 1, length: 4);
			int value = DecodeBase62(encoded: base62);
			return $"Minor Planet Supplement (MPS) {260000 + value}";
		}
		// 2F: Single uppercase letter + four digits (various journals)
		if (char.IsUpper(c: firstChar) && compressedRef.Length >= 2 && char.IsDigit(c: compressedRef[index: 1]))
		{
			string digits = compressedRef.Substring(startIndex: 1, length: 4);
			if (int.TryParse(s: digits, result: out int number))
			{
				return firstChar switch
				{
					'H' => $"Harvard Announcement Card (HAC) {number}",
					'I' => $"IAU Circular (IAUC) {number}",
					'M' => $"Minor Planet Circular (MPC) {number}",
					'R' => $"Planetenzirkular des Astronomischen Rechen-Institut (RI) {number}",
					_ => $"Journal '{firstChar}' #{number}"
				};
			}
		}
		// 2G: Two or more letters (various journals)
		if (compressedRef.Length >= 2)
		{
			string journalCode = compressedRef[..2].Trim();
			string remainder = compressedRef.Length > 2 ? compressedRef[2..].Trim() : "";
			// Attempt to get the journal name from the code
			string journalName = GetJournalName(code: journalCode);
			if (!string.IsNullOrEmpty(value: journalName))
			{
				return !string.IsNullOrEmpty(value: remainder) && int.TryParse(s: remainder, result: out int volOrCirc)
					? $"{journalName}, Vol./Circ. {volOrCirc}"
					: journalName;
			}
		}
		// If no known format matches, return the original compressed reference with a note
		return $"Unknown reference format: {compressedRef.Trim()}";
	}

	/// <summary>Decodes a Base-62 encoded string to an integer.</summary>
	/// <param name="encoded">The Base-62 encoded string.</param>
	/// <returns>The decoded integer value.</returns>
	/// <remarks>Uses characters 0-9, A-Z, a-z to represent digits 0-61.</remarks>
	private static int DecodeBase62(string encoded)
	{
		// Define the character set for Base-62 encoding
		const string base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		int result = 0;
		// Process each character in the encoded string
		foreach (char c in encoded)
		{
			// Find the index of the character in the Base-62 character set
			int digit = base62Chars.IndexOf(value: c);
			if (digit == -1)
			{
				// If the character is not found in the Base-62 set, throw a format exception
				throw new FormatException(message: $"Invalid Base-62 character: {c}");
			}
			result = (result * 62) + digit;
		}
		// Return the decoded integer value
		return result;
	}

	/// <summary>Gets the full journal name from a two-letter journal code.</summary>
	/// <param name="code">The two-letter journal code.</param>
	/// <returns>The full journal name, or an empty string if not found.</returns>
	private static string GetJournalName(string code) => code switch
	{
		"AA" => "Astronomy and Astrophysics",
		"AB" => "Bulletin des Astrophysikalischen Observatoriums Abastumani",
		"AC" => "Astronomisches Zirkular der Akademie der Wissenschaften der UdSSR",
		"AE" => "Astronomical Papers prepared for the use of the American Ephemeris and Nautical Almanac",
		"AJ" => "Astronomical Journal",
		"AN" => "Astronomische Nachrichten",
		"AP" => "Astrophysical Journal Supplement",
		"As" => "Astronomy and Astrophysics Supplement",
		"BA" => "Bulletin Astronomique",
		"BB" => "Bulletin Astronomique de l'Observatoire Royal de Belgique, Uccle",
		"BC" => "Bulletin of the Astronomical Institutes of Czechoslovakia",
		"BG" => "Bulletin de l'Observatoire Astronomique de Beograd",
		"BN" => "Bulletin of the Astronomical Institutes of the Netherlands",
		"BP" => "Bulletin de la Societe des amis des sciences et des lettres de Poznan",
		"BZ" => "Beobachtungs-Zirkulare der Astronomischen Nachrichten",
		"CB" => "Comet Bulletin of the Orient Astronomical Association",
		"CC" => "Observatorio Astronomico de Cordoba, Serie Contribuciones",
		"CD" => "Tsirkulyari Rasadkhonai Stalinobod",
		"CK" => "Izvestiya Krymskoj Astrofizicheskoj Observatorii",
		"CM" => "Circulaire de l'Observatoire de Marseille",
		"CO" => "Odesskij Gosudarstvennyj Universitet Izvestiya Astronomicheskoj Observattorii",
		"CR" => "Comptes Rendus hebdomadaires de l'academie des sciences de Paris",
		"CS" => "Soobshcheniya Gosudarstvennogo Astronomicheskogo Instituta imeni P. K. Shternberga",
		"GO" => "Greenwich Observations",
		"HA" => "Harvard Annal",
		"HD" => "Veröffentlichungen der Landessternwarte Heidelberg",
		"HTCDR" => "Hipparcos-Tycho CD-ROM",
		"IHW" => "International Halley Watch CD-ROM",
		"Ic" => "Icarus",
		"JB" => "Journal of the British Astronomical Association",
		"JC" => "Japan Astronomical Study Association Circular",
		"JO" => "Journal des Observateurs",
		"KB" => "Bulletin of the Kwasan Observatory, Kyoto",
		"KK" => "Kiev Komet Tsirkular",
		"LB" => "Lick Observatory Bulletin",
		"LO" => "Lowell Observatory Bulletin",
		"LP" => "Publicaciones Observatorio Astronomico de La Plata",
		"MN" => "Monthly Notices of the Royal Astronomical Society",
		"NA" => "Annales de l'Observatoire de Nice",
		"NC" => "Nihondaira Observatory Circular",
		"NO" => "Publications of the U.S. Naval Observatory, Second Series",
		"NZ" => "Nachrichtenblatt der Astronomischen Zentralstelle",
		"OB" => "The Observatory",
		"PA" => "Publications of the Astronomical Society of the Pacific",
		"PC" => "Poulkovo Observatory Circular",
		"PD" => "Tartu Astronoomia Observatooriumi Publikatsioonid",
		"PK" => "Pyublikatsii Kievskoj Astronomicheskoj Observatorii",
		"PO" => "Perth Observatory Communication",
		"PP" => "Izvestiya Glavnoj Astronomicheskoj Observatorii v Pulkove",
		"PT" => "Pubblicazioni del Osservatorio di Torino",
		"PZ" => "Zirkular des Astronomischen Hauptobservatoriums Pulkowo",
		"RA" => "Ricerche Astronomiche",
		"RM" => "Memoirs of the Royal Astronomical Society",
		"SA" => "Monthly Notices of the Astronomical Society of Southern Africa",
		"SOB" => "Observatory Bulletin",
		"TB" => "Tokyo Astronomical Bulletin",
		"TC" => "Transval Observatory Circular",
		"TI" => "Astronomia-Optika Institucio, Universitato de Turku, Informo",
		"UC" => "Circular of the Union Observatory, Johannesburg",
		"WO" => "Astronomical Observations of the U.S. Naval Observatory, Washington",
		"WiA" => "Annalen der Sternwarte der Universität Wien",
		"pM" => "Mitteilungen der Nikolai-Hauptsternwarte zu Pulkowo",
		"CMC" => "Carlsberg Meridian Circle Publications",
		"APO" => "Annales de l'Observatoire de Paris: Observations",
		"AS" => "Acta Astronomica Sinica",
		"AZ" => "Astronomicheskij Zhurnal",
		"AcA" => "Acta Astronomica",
		_ => string.Empty
	};

	#endregion
}
