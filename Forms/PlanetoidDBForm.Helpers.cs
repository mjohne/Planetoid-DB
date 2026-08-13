/*
 * File:        PlanetoidDbForm.helpers.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Partial class containing helper methods for the PlanetoidDbForm.
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

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;
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

	/// <summary>Shared HttpClient instance for making HTTP requests.</summary>
	/// <remarks>This instance is shared across the application to reuse connections and improve performance.</remarks>
	private static readonly HttpClient client = new();

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
	private static void Restart()
	{
		// Restart the application and exit the current instance with an exit code of 0 (indicating a normal exit)
		Application.Restart();
		Environment.Exit(exitCode: 0);
	}

	/// <summary>Asks the user if they want to restart the application after downloading the database.</summary>
	/// <remarks>This method is used to ask the user if they want to restart the application after downloading the database.</remarks>
	private void AskForRestartAfterDownloadingDatabase()
	{
		logger.Info(message: "Asking user if they want to restart the application after downloading the database.");
		// Bring the main form to the foreground before showing the message box, so it cannot appear behind other application windows.
		Activate();
		// Ask the user if they want to restart the application after downloading the database and show a message box with the option to restart or not
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
		// Convert string to ReadOnlySpan<char> to avoid heap allocations during parsing
		ReadOnlySpan<char> entrySpan = entryStr.AsSpan();
		// Local helper method to safely extract, slice, and trim fields without string overhead
		static string ExtractField(ReadOnlySpan<char> span, int start, int length)
		{
			return span.Length < start + length ? string.Empty : span.Slice(start: start, length: length).Trim().ToString();
		}
		// Suspend both the panel layout and painting to eliminate all flicker
		tableLayoutPanelData.SuspendLayout();
		try
		{
			// Batch all text updates with minimal overhead
			toolStripLabelIndexPosition.ToolTipText = $"Index: {position + 1}/{planetoidsDatabase.Count}";
			// Update all labels in one go - use cached string reference
			labelIndexData.Text = ExtractField(span: entrySpan, start: 0, length: 7);
			labelAbsoluteMagnitudeData.Text = ExtractField(span: entrySpan, start: 8, length: 5);
			labelSlopeParameterData.Text = ExtractField(span: entrySpan, start: 14, length: 5);
			labelEpochData.Text = ExtractField(span: entrySpan, start: 20, length: 5);
			labelMeanAnomalyAtTheEpochData.Text = ExtractField(span: entrySpan, start: 26, length: 9);
			labelArgumentOfThePerihelionData.Text = ExtractField(span: entrySpan, start: 37, length: 9);
			labelLongitudeOfTheAscendingNodeData.Text = ExtractField(span: entrySpan, start: 48, length: 9);
			labelInclinationToTheEclipticData.Text = ExtractField(span: entrySpan, start: 59, length: 9);
			labelOrbitalEccentricityData.Text = ExtractField(span: entrySpan, start: 70, length: 9);
			labelMeanDailyMotionData.Text = ExtractField(span: entrySpan, start: 80, length: 11);
			labelSemiMajorAxisData.Text = ExtractField(span: entrySpan, start: 92, length: 11);
			labelReferenceData.Text = ExtractField(span: entrySpan, start: 107, length: 9);
			labelNumberOfObservationsData.Text = ExtractField(span: entrySpan, start: 117, length: 5);
			labelNumberOfOppositionsData.Text = ExtractField(span: entrySpan, start: 123, length: 3);
			labelObservationSpanData.Text = ExtractField(span: entrySpan, start: 127, length: 9);
			labelRmsResidualData.Text = ExtractField(span: entrySpan, start: 137, length: 4);
			labelComputerNameData.Text = ExtractField(span: entrySpan, start: 150, length: 10);
			labelFlagsData.Text = ExtractField(span: entrySpan, start: 161, length: 4);
			labelReadableDesignationData.Text = ExtractField(span: entrySpan, start: 166, length: 28);
			labelDateLastObservationData.Text = ExtractField(span: entrySpan, start: 194, length: 8);
			toolStripLabelIndexPosition.Text = $@"{I18nStrings.Index}: {position + 1:N0} / {planetoidsDatabase.Count:N0}";
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message to the user
			logger.Error(message: $"Error navigating to position {position}: {ex.Message}", exception: ex);
			ShowErrorMessage(message: $"An error occurred while navigating to the record at position {position + 1}. Please try again.");
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
		catch (Exception ex)
		{
			// Log the exception and show an error message to the user
			logger.Error(message: $"Error clearing record display: {ex.Message}", exception: ex);
			ShowErrorMessage(message: "An error occurred while clearing the record display. Please try again.");
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
				currentAstorbPosition = currentPosition;
				GotoCurrentAstorbPosition(position: currentAstorbPosition);
				return;
			}
			// If the index does not match, check if the designation matches the current entry's designation (characters 166-193)
			if (!string.IsNullOrEmpty(value: designation) && entry.Length >= 194 && entry.Substring(startIndex: 166, length: 28).Trim().Equals(value: designation, comparisonType: StringComparison.OrdinalIgnoreCase))
			{
				// If the designation matches, set the current position to the index and navigate to that position
				currentPosition = i;
				GotoCurrentPosition(position: currentPosition);
				currentAstorbPosition = currentPosition;
				GotoCurrentAstorbPosition(position: currentAstorbPosition);
				return;
			}
		}
		// If no matching record is found, log and show an error message box to the user
		logger.Warn(message: $"Record not found in the current loaded database. Index: {index}, Designation: {designation}");
		ShowErrorMessage(message: "Record not found in the current loaded database.");
	}

	/// <summary>Retrieves the last modified date and time (in UTC) of the resource at the specified URI.</summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>The <see cref="DateTime"/> representing the last modified date and time in UTC if available; otherwise, <see cref="DateTime.MinValue"/>. </returns>
	/// <remarks>This method is used to retrieve the last modified date and time of a resource.</remarks>
	private static DateTime GetLastModified(Uri uri)
	{
		// Throw an exception if the URI is null
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request: request);
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
		// If the Last-Modified header is not present or the request failed, log a warning and return DateTime.MinValue
		logger.Warn(message: $"Failed to retrieve Last-Modified header for URI: {uri}");
		return DateTime.MinValue;
	}

	/// <summary>Retrieves the last modified date and time (in UTC) asynchronously.</summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>The <see cref="DateTime"/> representing the last modified date and time in UTC if available; otherwise, <see cref="DateTime.MinValue"/>.</returns>
	/// <remarks>This method is used to retrieve the last modified date and time of a resource asynchronously.</remarks>
	private static async Task<DateTime> GetLastModifiedAsync(Uri uri)
	{
		// Throw an exception if the URI is null
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Create a HEAD request to get only the headers of the resource
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request asynchronously and get the response
		using HttpResponseMessage response = await client.SendAsync(request: request);
		// Check if the request was successful and if the Last-Modified header is present
		if (response.IsSuccessStatusCode && response.Content.Headers.LastModified.HasValue)
		{
			// Return the last modified date in UTC
			return response.Content.Headers.LastModified.Value.UtcDateTime;
		}
		// If the Last-Modified header is not present or the request failed, log a warning and return DateTime.MinValue
		logger.Warn(message: $"Failed to retrieve Last-Modified header for URI: {uri}");
		return DateTime.MinValue;
	}

	/// <summary>Gets the content length of the specified URI.</summary>
	/// <param name="uri">The URI to check.</param>
	/// <returns>The content length of the URI.</returns>
	/// <remarks>This method is used to retrieve the content length of a resource.</remarks>
	private static long GetContentLength(Uri uri)
	{
		// Throw an exception if the URI is null
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Create a HEAD request to get only the headers
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request and get the response
		using HttpResponseMessage response = client.Send(request: request);
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
		// If the Content-Length header is not present or the request failed, log a warning and return 0
		logger.Warn(message: $"Failed to retrieve Content-Length header for URI: {uri}");
		return 0;
	}

	/// <summary>Asynchronously retrieves the content length of the specified URI.</summary>
	/// <param name="uri">The URI of the resource to check.</param>
	/// <returns>The content length of the resource if available; otherwise, 0.</returns>
	private static async Task<long> GetContentLengthAsync(Uri uri)
	{
		// Throw an exception if the URI is null
		ArgumentNullException.ThrowIfNull(argument: uri);
		// Create a HEAD request to get only the headers of the resource
		using HttpRequestMessage request = new(method: HttpMethod.Head, requestUri: uri);
		// Send the request asynchronously and get the response
		using HttpResponseMessage response = await client.SendAsync(request: request);
		// Check if the request was successful and if the Content-Length header is present
		if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength.HasValue)
		{
			// Return the content length
			return response.Content.Headers.ContentLength.Value;
		}
		// If the Content-Length header is not present or the request failed, log a warning and return 0
		logger.Warn(message: $"Failed to retrieve Content-Length header for URI: {uri}");
		return 0;
	}

	/// <summary>Loads a random minor planet from the database.</summary>
	/// <remarks>This method is used to load a random minor planet from the database.</remarks>
	private void LoadRandomMinorPlanet()
	{
		// Check if the planetoids database is empty before attempting to load a random minor planet
		if (planetoidsDatabase.Count == 0)
		{
			logger.Warn(message: "Attempted to load a random minor planet, but the database is empty.");
			return;
		}
		// Generate a random index within the bounds of the planetoids database
		currentPosition = Random.Shared.Next(maxValue: planetoidsDatabase.Count);
		GotoCurrentPosition(position: currentPosition);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates to the beginning of the data.</summary>
	/// <remarks>This method is used to navigate to the beginning of the data.</remarks>
	private void NavigateToTheBeginOfTheData()
	{
		GotoCurrentPosition(position: currentPosition = 0);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates backward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate backward by a specified step in the data.</remarks>
	private void NavigateSomeDataBackward()
	{
		// Check if the planetoids database is empty before attempting to navigate backward
		if (planetoidsDatabase.Count == 0)
		{
			logger.Warn(message: "Attempted to navigate backward, but the planetoids database is empty.");
			return;
		}
		// Calculate the new position by subtracting the step position and wrapping around using modulo
		currentPosition = (currentPosition - stepPosition) % planetoidsDatabase.Count;
		if (currentPosition < 0)
		{
			currentPosition += planetoidsDatabase.Count;
		}
		// Navigate to the new position
		GotoCurrentPosition(position: currentPosition);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates to the previous data entry.</summary>
	/// <remarks>This method is used to navigate to the previous data entry in the planetoids database.</remarks>
	private void NavigateToThePreviousData()
	{
		// If the current position is 0, wrap around to the last entry in the database
		if (currentPosition == 0)
		{
			// Set the current position to the last entry in the database
			currentPosition = planetoidsDatabase.Count - 1;
		}
		else
		{
			// Decrease the current position by 1
			currentPosition--;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates to the next data entry.</summary>
	/// <remarks>This method is used to navigate to the next data entry in the planetoids database.</remarks>
	private void NavigateToTheNextData()
	{
		// If the current position is the last entry in the database, wrap around to the first entry
		if (currentPosition == planetoidsDatabase.Count - 1)
		{
			// Set the current position to 0 (the first entry in the database)
			currentPosition = 0;
		}
		else
		{
			// Increase the current position by 1
			currentPosition++;
		}
		// Navigate to the current position
		GotoCurrentPosition(position: currentPosition);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates forward by a specified step in the data.</summary>
	/// <remarks>This method is used to navigate forward by a specified step in the data.</remarks>
	private void NavigateSomeDataForward()
	{
		// Check if the planetoids database is empty before attempting to navigate forward
		if (planetoidsDatabase.Count == 0)
		{
			logger.Warn(message: "Attempted to navigate forward, but the planetoids database is empty.");
			return;
		}
		// Calculate the new position by adding the step position and wrapping around using modulo
		currentPosition = (currentPosition + stepPosition) % planetoidsDatabase.Count;
		// Navigate to the new position
		GotoCurrentPosition(position: currentPosition);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Navigates to the end of the data.</summary>
	/// <remarks>This method is used to navigate to the end of the data.</remarks>
	private void NavigateToTheEndOfTheData()
	{
		GotoCurrentPosition(position: currentPosition = planetoidsDatabase.Count - 1);
		currentAstorbPosition = currentPosition;
		GotoCurrentAstorbPosition(position: currentAstorbPosition);
	}

	/// <summary>Processes a designation string by removing parenthetical content, trimming whitespace, and removing spaces.</summary>
	/// <param name="input">The input designation string to process.</param>
	/// <returns>The processed string with parenthetical content removed, trimmed, and spaces removed.</returns>
	/// <remarks>This method is useful for preparing designation strings for URL queries. For example, "(449127) 2013 AS15" becomes "2013AS15".</remarks>
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
		// Log the action of opening the terminology form with the selected element
		logger.Info(message: $"Opening terminology form with selected element: {formTerminology.SelectedElement}");
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
		// Log the action of opening the table mode form with the current planetoids database
		logger.Info(message: "Opening table mode form with the current planetoids database.");
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
		// Log the action of opening the orbital resonances form with the parsed semi-major axis
		logger.Info(message: $"Opening orbital resonances form with semi-major axis: {semiMajorAxis}");
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
		// Log the action of opening the observations form with the provided index data
		logger.Info(message: $"Opening observations form with index data: {labelIndexData.Text}");
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
		// Log the action of opening the orbit elements grouping form with the current planetoids database
		logger.Info(message: "Opening orbit elements grouping form with the current planetoids database.");
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
		// Log the action of opening the asteroid families form with the current planetoids database
		logger.Info(message: "Opening asteroid families form with the current planetoids database.");
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
		// Log the action of opening the orbital resonances form with the current planetoids database
		logger.Info(message: "Opening orbital resonances form with the current planetoids database.");
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
			logger.Error(message: "Failed to parse orbital elements for MOIDs form.");
			return;
		}
		// Log the action of opening the MOIDs form with the parsed orbital elements
		logger.Info(message: $"Opening MOIDs form with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}, longitude of ascending node={longitudeAscendingNodeDeg}, argument of perihelion={argumentPerihelionDeg}");
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
			logger.Error(message: "Failed to parse orbital elements for MAXOIDs form.");
			return;
		}
		// Log the action of opening the MAXOIDs form with the parsed orbital elements
		logger.Info(message: $"Opening MAXOIDs form with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}, longitude of ascending node={longitudeAscendingNodeDeg}, argument of perihelion={argumentPerihelionDeg}");
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
			logger.Error(message: "Failed to parse orbital elements for MOIDs and MAXOIDs form.");
			return;
		}
		// Log the action of opening the MOIDs and MAXOIDs form with the parsed orbital elements
		logger.Info(message: $"Opening MOIDs and MAXOIDs form with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}, longitude of ascending node={longitudeAscendingNodeDeg}, argument of perihelion={argumentPerihelionDeg}");
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
		// Log the action of opening the MOIDs of all minor planets form with the current planetoids database
		logger.Info(message: "Opening MOIDs of all minor planets form with the current planetoids database.");
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
		// Log the action of opening the MAXOIDs of all minor planets form with the current planetoids database
		logger.Info(message: "Opening MAXOIDs of all minor planets form with the current planetoids database.");
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
		// Log the action of opening the histogram form with the current planetoids database
		logger.Info(message: "Opening histogram form with the current planetoids database.");
		// Create a new instance of the HistogramsForm
		using DistributionsForm formHistogram = new(planetoids: planetoidsDatabase);
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formHistogram.TopMost = TopMost;
		// Show the histogram form as a modal dialog
		_ = formHistogram.ShowDialog(owner: this);
	}

	/// <summary>Shows the scatterplots form. Opens the form to display scatterplots of orbital elements and properties of all minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can create scatterplots of various properties.</remarks>
	private void ShowScatterPlot()
	{
		// Log the action of opening the scatterplots form with the current planetoids database
		logger.Info(message: "Opening scatterplots form with the current planetoids database.");
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
			logger.Error(message: "Failed to parse orbital elements for Orbit2DTopView form.");
			return;
		}
		// Log the action of opening the Orbit2DTopView form with the parsed orbital elements
		logger.Info(message: $"Opening Orbit2DTopView form with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, argument of perihelion={argumentPerihelionDeg}");
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
			logger.Error(message: "Failed to parse orbital elements for Orbit2DSideView form.");
			return;
		}
		// Log the action of opening the Orbit2DSideView form with the parsed orbital elements
		logger.Info(message: $"Opening Orbit2DSideView form with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}");
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
			logger.Error(message: "Failed to parse orbital elements for Orbit3DForm.");
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
		// Log the action of opening the Orbit3DForm with the parsed orbital elements
		logger.Info(message: $"Opening Orbit3DForm with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}, longitude of ascending node={longitudeAscendingNodeDeg}, argument of perihelion={argumentPerihelionDeg}, mean anomaly={meanAnomalyDeg}, epoch={epochMpcorb}");
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
		// Log the action of opening the TisserandParameterOfOneMinorPlanetForm with the parsed orbital elements
		logger.Info(message: $"Opening TisserandParameterOfOneMinorPlanetForm with orbital elements: semi-major axis={semiMajorAxis}, eccentricity={eccentricity}, inclination={inclinationDeg}");
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
		// Log the action of opening the BulkObservationsDataDownloaderForm with the current planetoids database
		logger.Info(message: "Opening BulkObservationsDataDownloaderForm with the current planetoids database.");
		_ = formBulkDownloader.ShowDialog(owner: this);
	}

	/// <summary>Shows the MOIDs relative to minor planets form. Opens the form to calculate the MOID between two user-selected minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can populate the combo boxes with all available planetoid designations.</remarks>
	private void ShowMoidsRelativeToMinorPlanets()
	{
		// Create a new instance of the MoidsRelativeToMinorPlanetsForm
		using MoidsRelativeToMinorPlanetsForm formMoidsRelative = new(planetoids: planetoidsDatabase);
		formMoidsRelative.TopMost = TopMost;
		// Log the action of opening the MoidsRelativeToMinorPlanetsForm with the current planetoids database
		logger.Info(message: "Opening MoidsRelativeToMinorPlanetsForm with the current planetoids database.");
		_ = formMoidsRelative.ShowDialog(owner: this);
	}

	/// <summary>Shows the MAXOIDs relative to minor planets form. Opens the form to calculate the MAXOID between two user-selected minor planets.</summary>
	/// <remarks>Passes the full planetoids database to the form so it can populate the combo boxes with all available planetoid designations.</remarks>
	private void ShowMaxoidsRelativeToMinorPlanets()
	{
		// Create a new instance of the MaxoidsRelativeToMinorPlanetsForm
		using MaxoidsRelativeToMinorPlanetsForm formMaxoidsRelative = new(planetoids: planetoidsDatabase);
		formMaxoidsRelative.TopMost = TopMost;
		// Log the action of opening the MaxoidsRelativeToMinorPlanetsForm with the current planetoids database
		logger.Info(message: "Opening MaxoidsRelativeToMinorPlanetsForm with the current planetoids database.");
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
		// Log the action of opening the AppInfoForm
		logger.Info(message: "Opening AppInfoForm.");
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
		// Log the action of opening the ArchiveMpcorbForm
		logger.Info(message: "Opening ArchiveMpcorbForm.");
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
		// Log the action of opening the DatabaseDifferencesForm
		logger.Info(message: "Opening DatabaseDifferencesForm.");
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
		// Log the action of opening the LicenseForm
		logger.Info(message: "Opening LicenseForm.");
		// Show the license form as a modal dialog
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
		// Log the action of opening the RecordsForm
		logger.Info(message: "Opening RecordsForm with the current planetoids database.");
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
		// Log the action of opening the RecordsTop10Form
		logger.Info(message: $"Opening RecordsTop10Form with the current planetoids database and selected element: {selectedElement}");
		// Show the records form as a modal dialog
		_ = formRecordsTop10.ShowDialog(owner: this);
	}


	/// <summary>Shows the MPCORB.DAT data check form.</summary>
	/// <remarks>This method is used to check the MPCORB.DAT data for updates.</remarks>
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
			// Log the action of opening the CheckDatabaseForm for MPCORB.DAT
			logger.Info(message: "Opening CheckDatabaseForm for MPCORB.DAT.");
			// Show the MPCORB data check form as a modal dialog
			_ = formCheckMpcorbDat.ShowDialog(owner: this);
		}
	}

	/// <summary>Shows the MPCORB.JSON data check form.</summary>
	/// <remarks>This method is used to check the MPCORB.JSON data for updates.</remarks>
	private async void ShowMpcorbJsonUpdateCheck()
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
			// Create and show the MPCORB.JSON data check form
			using CheckDatabaseForm formCheckMpcorbJson = new(url: Settings.Default.systemMpcorbJsonGzUrl, localFilePath: Settings.Default.systemFilenameMpcorbJson, databaseName: "MPCORB.JSON");
			// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
			formCheckMpcorbJson.TopMost = TopMost;
			// Log the action of opening the CheckDatabaseForm for MPCORB.JSON
			logger.Info(message: "Opening CheckDatabaseForm for MPCORB.JSON.");
			// Show the MPCORB.JSON data check form as a modal dialog
			_ = formCheckMpcorbJson.ShowDialog(owner: this);
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
			// Log the action of opening the CheckDatabaseForm for ASTORB.DAT
			logger.Info(message: "Opening CheckDatabaseForm for ASTORB.DAT.");
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
			// Log the action of opening the CheckDatabaseForm for ALLNUM.CAT
			logger.Info(message: "Opening CheckDatabaseForm for ALLNUM.CAT.");
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
			// Log the action of opening the CheckDatabaseForm for UFITOBS.CAT
			logger.Info(message: "Opening CheckDatabaseForm for UFITOBS.CAT.");
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
			// Log the action of opening the CheckDatabaseForm for SINGOPP.CAT
			logger.Info(message: "Opening CheckDatabaseForm for SINGOPP.CAT.");
			// Show the SINGOPP.CAT data check form as a modal dialog
			_ = formCheckSingoppCat.ShowDialog(owner: this);
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
		// Log the action of opening the DatabaseInformationForm
		logger.Info(message: "Opening DatabaseInformationForm.");
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
		// Log the action of opening the SearchForm
		logger.Info(message: "Opening SearchForm.");
		// Show the search form as a modal dialog
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
		// Log the action of opening the FilterForm with the current planetoids database
		logger.Info(message: "Opening FilterForm with the current planetoids database.");
		// Show the filter form as a modal dialog
		if (formFilter.ShowDialog(owner: this) == DialogResult.OK && formFilter.FilteredDatabase is { } filtered)
		{
			// Replace the current database with the filtered result
			planetoidsDatabase.Clear();
			planetoidsDatabase.AddRange(collection: filtered);
			// Navigate to the first record of the filtered database
			currentPosition = 0;
			GotoCurrentPosition(position: currentPosition);
			currentAstorbPosition = currentPosition;
			GotoCurrentAstorbPosition(position: currentAstorbPosition);
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
		// Log the action of opening the SettingsForm
		logger.Info(message: "Opening SettingsForm.");
		// Fill the form with the planetoids database
		_ = formSettings.ShowDialog(owner: this);
	}

	/// <summary>Shows the settings export form.</summary>
	/// <remarks>Opens a modal dialog that lets the user export all program settings to CSV, INI, XML, JSON, or YAML.</remarks>
	private void ShowSettingsExport()
	{
		// Create a new instance of the SettingsExportForm
		using SettingsExportForm formSettingsExport = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formSettingsExport.TopMost = TopMost;
		// Log the action of opening the SettingsExportForm
		logger.Info(message: "Opening SettingsExportForm.");
		_ = formSettingsExport.ShowDialog(owner: this);
	}

	/// <summary>Shows the settings import form.</summary>
	/// <remarks>Opens a modal dialog that lets the user import all user-scoped program settings from CSV, INI, XML, JSON, or YAML.</remarks>
	private void ShowSettingsImport()
	{
		// Create a new instance of the SettingsImportForm
		using SettingsImportForm formSettingsImport = new();
		// Set the TopMost property to match the current form's TopMost value to maintain consistent window layering
		formSettingsImport.TopMost = TopMost;
		// Log the action of opening the SettingsImportForm
		logger.Info(message: "Opening SettingsImportForm.");
		_ = formSettingsImport.ShowDialog(owner: this);
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
		// Log the action of opening the ListReadableDesignationsForm
		logger.Info(message: "Opening ListReadableDesignationsForm.");
		// Show the list readable designations form as a modal dialog
		_ = formListReadableDesignations.ShowDialog(owner: this);
		// Check if the dialog result is OK and the selected index is greater than 0
		if (formListReadableDesignations.DialogResult == DialogResult.OK && formListReadableDesignations.GetSelectedIndex() > 0)
		{
			// Navigate to the current position in the database
			GotoCurrentPosition(position: formListReadableDesignations.GetSelectedIndex());
			currentAstorbPosition = currentPosition;
			GotoCurrentAstorbPosition(position: currentAstorbPosition);
		}
	}

	/// <summary>Generates a complete list of orbital and derived elements safely.</summary>
	/// <remarks>This method parses the orbital elements from the UI labels, calculates derived elements, and returns a list of strings representing all relevant data.</remarks>
	private List<string> GenerateFullOrbitalElementsList()
	{
		// Create a list to hold the orbital and derived elements
		List<string> elements = [];
		// Use the invariant culture for consistent parsing of numeric values
		IFormatProvider provider = CultureInfo.InvariantCulture;
		// Parse the necessary orbital elements from the UI labels, using TryParse to handle potential parsing errors gracefully
		double.TryParse(s: labelSemiMajorAxisData.Text, style: NumberStyles.Any, provider: provider, result: out double semiMajorAxis);
		double.TryParse(s: labelOrbitalEccentricityData.Text, style: NumberStyles.Any, provider: provider, result: out double numericalEccentricity);
		double.TryParse(s: labelMeanAnomalyAtTheEpochData.Text, style: NumberStyles.Any, provider: provider, result: out double meanAnomaly);
		double.TryParse(s: labelLongitudeOfTheAscendingNodeData.Text, style: NumberStyles.Any, provider: provider, result: out double longitudeAscendingNode);
		double.TryParse(s: labelArgumentOfThePerihelionData.Text, style: NumberStyles.Any, provider: provider, result: out double argumentAphelion);
		// Add the original orbital elements to the list
		elements.Add(item: labelIndexData.Text);
		elements.Add(item: labelReadableDesignationData.Text);
		elements.Add(item: labelEpochData.Text);
		elements.Add(item: labelMeanAnomalyAtTheEpochData.Text);
		elements.Add(item: labelArgumentOfThePerihelionData.Text);
		elements.Add(item: labelLongitudeOfTheAscendingNodeData.Text);
		elements.Add(item: labelInclinationToTheEclipticData.Text);
		elements.Add(item: labelOrbitalEccentricityData.Text);
		elements.Add(item: labelMeanDailyMotionData.Text);
		elements.Add(item: labelSemiMajorAxisData.Text);
		elements.Add(item: labelAbsoluteMagnitudeData.Text);
		elements.Add(item: labelSlopeParameterData.Text);
		elements.Add(item: labelReferenceData.Text);
		elements.Add(item: labelNumberOfOppositionsData.Text);
		elements.Add(item: labelNumberOfObservationsData.Text);
		elements.Add(item: labelObservationSpanData.Text);
		elements.Add(item: labelRmsResidualData.Text);
		elements.Add(item: labelComputerNameData.Text);
		elements.Add(item: labelFlagsData.Text);
		elements.Add(item: labelDateLastObservationData.Text);
		elements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateArgumentOfAphelion(argumentAphelion: argumentAphelion).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculatePeriod(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		elements.Add(item: DerivedElements.CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		// Return the complete list of orbital and derived elements
		return elements;
	}

	/// <summary>Exports the data sheet.</summary>
	///	<remarks>This method is used to export the data sheet.</remarks>
	private void ExportDataSheet()
	{
		// Create a new instance of the ExportDataSheetForm
		using ExportDataSheetForm formExportDataSheet = new() { TopMost = this.TopMost };
		// Fill the form with the complete list of orbital and derived elements
		formExportDataSheet.SetDatabase(list: GenerateFullOrbitalElementsList());
		// Log the action of opening the ExportDataSheetForm with the current planetoids database
		logger.Info(message: "Opening ExportDataSheetForm with the current planetoids database.");
		// Show the export data sheet form as a modal dialog
		_ = formExportDataSheet.ShowDialog(owner: this);
	}

	/// <summary>Shows the print data sheet form.</summary>
	/// <remarks>This method is used to show the print data sheet form.</remarks>
	private void PrintDataSheet()
	{
		// Create a new instance of the PrintDataSheetForm
		using PrintDataSheetForm formPrintDataSheet = new() { TopMost = this.TopMost };
		// Fill the form with the complete list of orbital and derived elements
		formPrintDataSheet.SetDatabase(db: GenerateFullOrbitalElementsList());
		// Log the action of opening the PrintDataSheetForm with the current planetoids database
		logger.Info(message: "Opening PrintDataSheetForm with the current planetoids database.");
		// Show the print data sheet form as a modal dialog
		_ = formPrintDataSheet.ShowDialog(owner: this);
	}

	/// <summary>Shows the derived orbit elements form.</summary>
	/// <remarks>This method is used to show the derived orbit elements form.</remarks>
	private void ShowDerivedOrbitElements()
	{
		// Create a new list to store the derived orbit elements
		List<string> derivedOrbitElements = [];
		// Create a specific culture for formatting
		IFormatProvider provider = CultureInfo.InvariantCulture;
		double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
		double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
		double meanAnomaly = double.Parse(s: labelMeanAnomalyAtTheEpochData.Text, provider: provider);
		double longitudeAscendingNode = double.Parse(s: labelLongitudeOfTheAscendingNodeData.Text, provider: provider);
		double argumentPerihelion = double.Parse(s: labelArgumentOfThePerihelionData.Text, provider: provider);
		double inclination = double.Parse(s: labelInclinationToTheEclipticData.Text, provider: provider);
		double absoluteMagnitude = double.Parse(s: labelAbsoluteMagnitudeData.Text, provider: provider);
		// Calculate true anomaly for velocity and energy calculations
		double trueAnomaly = DerivedElements.CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity);
		// Original 19 elements
		derivedOrbitElements.Add(item: DerivedElements.CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMajorAxis(semiMajorAxis: semiMajorAxis).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
		derivedOrbitElements.Add(item: DerivedElements.CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity).ToString(provider: provider));
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
		// Log the action of opening the DerivedOrbitElementsForm with the current planetoids database
		logger.Info(message: "Opening DerivedOrbitElementsForm with the current planetoids database.");
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
			logger.Warn(message: "User canceled the file selection dialog for local MPCORB.DAT.");
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
			DialogResult result = _ = KryptonMessageBox.Show(
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

	/// <summary>Enables experimental features in the application and updates the user settings accordingly. If the 'silent' parameter is set to false, a message box will be displayed to inform the user about the enabled experimental features.</summary>
	/// <param name="silent">If set to true, no message box will be displayed.</param>
	/// <remarks>This method updates the user settings and logs the changes. It also enables the corresponding UI elements for the experimental features.</remarks>
	private void EnableExperimentalFeatures(bool silent = false)
	{
		// Enable experimental features in the application
		toolStripMenuItemDistributions.Enabled = true;
		toolStripMenuItemScatterPlots.Enabled = true;
		toolStripMenuItemAEIDiagram3D.Enabled = true;
		toolStripMenuItemOrbit.Enabled = true;
		toolStripButtonDistributions.Enabled = true;
		toolStripButtonScatterPlots.Enabled = true;
		toolStripDropDownButtonOrbit.Enabled = true;
		// Persist and log only when the setting actually changes
		if (!Settings.Default.userEnableExperimentalFeatures)
		{
			Settings.Default.userEnableExperimentalFeatures = true;
			Settings.Default.Save();
			logger.Info(message: "Experimental features enabled.");
		}
		// Show a message box to inform the user about the enabled experimental features
		if (!silent)
		{
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "Experimental features have been enabled. Please note that these features are in development and may not be fully stable.",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information,
				defaultButton: KryptonMessageBoxDefaultButton.Button1);
		}
	}

	/// <summary>Disables experimental features in the application and updates the user settings accordingly. If the 'silent' parameter is set to false, a message box will be displayed to inform the user about the disabled experimental features.</summary>
	/// <param name="silent">If set to true, no message box will be displayed.</param>
	/// <remarks>This method updates the user settings and logs the changes. It also disables the corresponding UI elements for the experimental features.</remarks>
	private void DisableExperimentalFeatures(bool silent = false)
	{
		// Disable experimental features in the application
		toolStripMenuItemDistributions.Enabled = false;
		toolStripMenuItemScatterPlots.Enabled = false;
		toolStripMenuItemAEIDiagram3D.Enabled = false;
		toolStripMenuItemOrbit.Enabled = false;
		toolStripButtonDistributions.Enabled = false;
		toolStripButtonScatterPlots.Enabled = false;
		toolStripDropDownButtonOrbit.Enabled = false;
		// Persist and log only when the setting actually changes
		if (Settings.Default.userEnableExperimentalFeatures)
		{
			Settings.Default.userEnableExperimentalFeatures = false;
			Settings.Default.Save();
			logger.Info(message: "Experimental features disabled.");
		}
		// Show a message box to inform the user about the disabled experimental features
		if (!silent)
		{
			_ = KryptonMessageBox.Show(
				owner: this,
				text: "Experimental features have been disabled.",
				caption: I18nStrings.InformationCaption,
				buttons: KryptonMessageBoxButtons.OK,
				icon: KryptonMessageBoxIcon.Information,
				defaultButton: KryptonMessageBoxDefaultButton.Button1);
		}
	}

	/// <summary>Loads the ASTORB.DAT database from the configured file path into <see cref="astorbDatabase"/>.</summary>
	/// <remarks>This method reads all lines from the ASTORB.DAT file, populates the <see cref="astorbDatabase"/> list, and updates the tab page text with the file's last-write date. If the file does not exist, the tab text is updated to reflect that the file is missing.</remarks>
	internal void LoadAstorbDatabase()
	{
		// Clear any previously loaded entries
		astorbDatabase.Clear();
		// Check if the ASTORB.DAT file exists
		if (!File.Exists(path: filenameAstorbDat))
		{
			logger.Warn(message: $"ASTORB.DAT file not found: {filenameAstorbDat}");
			kryptonPageAstorbDat.Text = "ASTORB.DAT (file not found)";
			return;
		}
		try
		{
			// Read all lines from the ASTORB.DAT file and add them to the database list
			astorbDatabase.AddRange(collection: File.ReadAllLines(path: filenameAstorbDat));
			// Get the last write time of the ASTORB.DAT file for display in the tab
			string fileDate = File.GetLastWriteTime(path: filenameAstorbDat).ToString(format: "yyyy-MM-dd", provider: CultureInfo.InvariantCulture);
			kryptonPageAstorbDat.Text = $"ASTORB.DAT ({fileDate})";
			logger.Info(message: $"ASTORB.DAT loaded: {astorbDatabase.Count} lines, dated {fileDate}.");
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			logger.Error(exception: ex, message: $"Error loading ASTORB.DAT: {ex.Message}");
			kryptonPageAstorbDat.Text = $"ASTORB.DAT ({I18nStrings.ErrorCaption})";
		}
	}

	/// <summary>Navigates to the specified position in the ASTORB.DAT database and updates all ASTORB labels.</summary>
	/// <param name="position">The zero-based position to navigate to in <see cref="astorbDatabase"/>.</param>
	/// <remarks>This method parses the fixed-width fields of the ASTORB.DAT record at the given position and updates the corresponding UI labels. If the position is out of range or the database is empty, all labels are cleared.</remarks>
	internal void GotoCurrentAstorbPosition(int position)
	{
		// Handle the case where the database is empty or position is out of range
		if (astorbDatabase.Count == 0 || position < 0 || position >= astorbDatabase.Count)
		{
			ClearCurrentAstorbRecordDisplay();
			return;
		}
		// Get the entry string for the requested position
		string? entryStr = astorbDatabase[index: position]?.ToString();
		// If the entry string is null or empty, clear all labels and return early
		if (string.IsNullOrEmpty(value: entryStr))
		{
			ClearCurrentAstorbRecordDisplay();
			return;
		}
		// Convert string to ReadOnlySpan<char> to avoid heap allocations during parsing
		ReadOnlySpan<char> entrySpan = entryStr.AsSpan();
		// Local helper to safely extract and trim a fixed-width field (1-based column indices from ASTORB format)
		static string ExtractField(ReadOnlySpan<char> span, int start, int length)
		{
			return span.Length < start + length ? string.Empty : span.Slice(start: start, length: length).Trim().ToString();
		}
		// Suspend layout to avoid flicker while updating labels
		tableLayoutPanelAstorbData.SuspendLayout();
		try
		{
			// ASTORB.DAT fixed-width field definitions (0-based start, length):
			// Col 1-6 (0-based 0,6): Asteroid number
			labelAstorbNumberData.Text = ExtractField(span: entrySpan, start: 0, length: 6);
			// Col 7-25 (0-based 6,19): Name
			labelAstorbNameData.Text = ExtractField(span: entrySpan, start: 6, length: 19);
			// Col 26-44 (0-based 25,19): Provisional designation
			labelAstorbDesignationData.Text = ExtractField(span: entrySpan, start: 25, length: 19);
			// Col 45-50 (0-based 44,6): Computer name
			labelAstorbComputerNameData.Text = ExtractField(span: entrySpan, start: 44, length: 6);
			// Col 51-55 (0-based 50,5): H (absolute magnitude)
			labelAstorbAbsoluteMagnitudeData.Text = ExtractField(span: entrySpan, start: 50, length: 5);
			// Col 56-60 (0-based 55,5): G (slope parameter)
			labelAstorbSlopeParameterData.Text = ExtractField(span: entrySpan, start: 55, length: 5);
			// Col 61-63 (0-based 60,3): B-V color index
			labelAstorbColorIndexData.Text = ExtractField(span: entrySpan, start: 60, length: 3);
			// Col 64-68 (0-based 63,5): IRAS diameter (km)
			labelAstorbIrasDiameterData.Text = ExtractField(span: entrySpan, start: 63, length: 5);
			// Col 69-72 (0-based 68,4): IRAS taxonomic class
			labelAstorbIrasTaxClassData.Text = ExtractField(span: entrySpan, start: 68, length: 4);
			// Col 73-78 (0-based 72,6): 6-digit flags
			labelAstorbFlagsData.Text = ExtractField(span: entrySpan, start: 72, length: 6);
			// Col 79-82 (0-based 78,4): Orbital arc (days)
			labelAstorbOrbitalArcData.Text = ExtractField(span: entrySpan, start: 78, length: 4);
			// Col 83-87 (0-based 82,5): Number of observations
			labelAstorbNumberOfObsData.Text = ExtractField(span: entrySpan, start: 82, length: 5);
			// Col 88-95 (0-based 87,8): Epoch (YYYYMMDD)
			labelAstorbEpochData.Text = ExtractField(span: entrySpan, start: 87, length: 8);
			// Col 96-105 (0-based 95,10): Mean anomaly (degrees)
			labelAstorbMeanAnomalyData.Text = ExtractField(span: entrySpan, start: 95, length: 10);
			// Col 106-115 (0-based 105,10): Argument of perihelion (degrees)
			labelAstorbArgOfPerihelionData.Text = ExtractField(span: entrySpan, start: 105, length: 10);
			// Col 116-125 (0-based 115,10): Longitude of ascending node (degrees)
			labelAstorbLongAscNodeData.Text = ExtractField(span: entrySpan, start: 115, length: 10);
			// Col 126-135 (0-based 125,10): Inclination (degrees)
			labelAstorbInclinationData.Text = ExtractField(span: entrySpan, start: 125, length: 10);
			// Col 136-144 (0-based 135,9): Orbital eccentricity
			labelAstorbEccentricityData.Text = ExtractField(span: entrySpan, start: 135, length: 9);
			// Col 145-154 (0-based 144,10): Semi-major axis (AU)
			labelAstorbSemiMajorAxisData.Text = ExtractField(span: entrySpan, start: 144, length: 10);
			// Col 155-162 (0-based 154,8): Date of first observation (YYYYMMDD)
			labelAstorbDateFirstObsData.Text = ExtractField(span: entrySpan, start: 154, length: 8);
			// Col 163-170 (0-based 162,8): Date of last observation (YYYYMMDD)
			labelAstorbDateLastObsData.Text = ExtractField(span: entrySpan, start: 162, length: 8);
			// Col 171-179 (0-based 170,9): Earth MOID (AU)
			labelAstorbEarthMoidData.Text = ExtractField(span: entrySpan, start: 170, length: 9);
			// Col 180-188 (0-based 179,9): Earth MOID date (YYYYMMDD)
			labelAstorbEarthMoidDateData.Text = ExtractField(span: entrySpan, start: 179, length: 9);
			// Col 189-197 (0-based 188,9): Orbital period (years)
			labelAstorbPeriodData.Text = ExtractField(span: entrySpan, start: 188, length: 9);
			// Col 198-206 (0-based 197,9): Perihelion date (YYYYMMDD)
			labelAstorbPerihelionDateData.Text = ExtractField(span: entrySpan, start: 197, length: 9);
			// Col 207-214 (0-based 206,8): Tisserand parameter w.r.t. Jupiter
			labelAstorbTisserandJupData.Text = ExtractField(span: entrySpan, start: 206, length: 8);
			// Col 215-223 (0-based 214,9): Perihelion distance (AU)
			labelAstorbPerihelionDistData.Text = ExtractField(span: entrySpan, start: 214, length: 9);
			// Col 224-232 (0-based 223,9): Aphelion distance (AU)
			labelAstorbAphelionDistData.Text = ExtractField(span: entrySpan, start: 223, length: 9);
			logger.Debug(message: $"ASTORB record at position {position} displayed.");
		}
		catch (Exception ex)
		{
			logger.Error(message: $"Error navigating to ASTORB position {position}: {ex.Message}", exception: ex);
		}
		finally
		{
			tableLayoutPanelAstorbData.ResumeLayout(performLayout: true);
		}
	}

	/// <summary>Clears all ASTORB record display labels in the ASTORB data panel.</summary>
	/// <remarks>This method clears all UI labels used to display ASTORB.DAT record fields.</remarks>
	private void ClearCurrentAstorbRecordDisplay()
	{
		tableLayoutPanelAstorbData.SuspendLayout();
		try
		{
			labelAstorbNumberData.Text = string.Empty;
			labelAstorbNameData.Text = string.Empty;
			labelAstorbDesignationData.Text = string.Empty;
			labelAstorbComputerNameData.Text = string.Empty;
			labelAstorbAbsoluteMagnitudeData.Text = string.Empty;
			labelAstorbSlopeParameterData.Text = string.Empty;
			labelAstorbColorIndexData.Text = string.Empty;
			labelAstorbIrasDiameterData.Text = string.Empty;
			labelAstorbIrasTaxClassData.Text = string.Empty;
			labelAstorbFlagsData.Text = string.Empty;
			labelAstorbOrbitalArcData.Text = string.Empty;
			labelAstorbNumberOfObsData.Text = string.Empty;
			labelAstorbEpochData.Text = string.Empty;
			labelAstorbMeanAnomalyData.Text = string.Empty;
			labelAstorbArgOfPerihelionData.Text = string.Empty;
			labelAstorbLongAscNodeData.Text = string.Empty;
			labelAstorbInclinationData.Text = string.Empty;
			labelAstorbEccentricityData.Text = string.Empty;
			labelAstorbSemiMajorAxisData.Text = string.Empty;
			labelAstorbDateFirstObsData.Text = string.Empty;
			labelAstorbDateLastObsData.Text = string.Empty;
			labelAstorbEarthMoidData.Text = string.Empty;
			labelAstorbEarthMoidDateData.Text = string.Empty;
			labelAstorbPeriodData.Text = string.Empty;
			labelAstorbPerihelionDateData.Text = string.Empty;
			labelAstorbTisserandJupData.Text = string.Empty;
			labelAstorbPerihelionDistData.Text = string.Empty;
			labelAstorbAphelionDistData.Text = string.Empty;
		}
		finally
		{
			tableLayoutPanelAstorbData.ResumeLayout(performLayout: false);
		}
	}

	#endregion
}