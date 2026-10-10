/*
 * File:        WorldMapForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a form that shows an interactive world map and returns the selected geographic coordinates.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Microsoft.Web.WebView2.Core;

using NLog;

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Planetoid_DB;

/// <summary>Represents a form that shows an interactive world map and returns the selected geographic coordinates.</summary>
/// <remarks>The map is rendered with Leaflet and OpenStreetMap tiles inside a WebView2 control. Clicking the map sets a marker; places can also be searched via OpenStreetMap Nominatim. After <see cref="Form.ShowDialog()"/> returns <see cref="DialogResult.OK"/>, <see cref="Latitude"/> and <see cref="Longitude"/> contain the selected position.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal partial class WorldMapForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Shared HTTP client used for geocoding requests.</summary>
	/// <remarks>Nominatim requires an identifying User-Agent header.</remarks>
	private static readonly HttpClient httpClient = CreateHttpClient();

	/// <summary>Cancellation source for work that should stop when the form closes.</summary>
	/// <remarks>This is used to cancel any ongoing geocoding requests when the form is closing.</remarks>
	private readonly CancellationTokenSource cancellationTokenSource = new();

	/// <summary>Format string for displaying coordinates in the status bar.</summary>
	/// <remarks>This format string is parsed from the localized resource string <see cref="I18nStrings.WorldMapCoordinatesFormat"/>.</remarks>
	private static readonly CompositeFormat coordinatesFormat = CompositeFormat.Parse(format: I18nStrings.WorldMapCoordinatesFormat);

	/// <summary>HTML page hosting the Leaflet map.</summary>
	/// <remarks>Map clicks are posted back to the host as JSON messages with <c>lat</c> and <c>lng</c> properties. The host calls <c>setMarker(lat, lng, zoomTo)</c> to place a marker.</remarks>
	private static readonly string MapHtml = $$"""
		<!DOCTYPE html>
		<html>
		<head>
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1.0" />
		<link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
		<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
		<style>html, body, #map { height: 100%; margin: 0; padding: 0; }</style>
		</head>
		<body>
		<div id="map"></div>
		<script>
		var map = L.map('map', { worldCopyJump: true }).setView([20, 0], 2);
		L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
			maxZoom: 19,
			attribution: '{{I18nStrings.WorldMapAttribution}}'
		}).addTo(map);
		var marker = null;
		function setMarker(lat, lng, zoomTo) {
			if (marker === null) { marker = L.marker([lat, lng]).addTo(map); }
			else { marker.setLatLng([lat, lng]); }
			if (zoomTo) { map.setView([lat, lng], Math.max(map.getZoom(), 12)); }
		}

		map.on('click', function (e) {
			var ll = e.latlng.wrap();
			setMarker(ll.lat, ll.lng, false);
			window.chrome.webview.postMessage({ lat: ll.lat, lng: ll.lng });
		});
		</script>
		</body>
		</html>
		""";

	/// <summary>Gets the latitude of the selected position in degrees.</summary>
	/// <remarks>Only meaningful after the dialog returned <see cref="DialogResult.OK"/>.</remarks>
	public decimal Latitude { get; private set; }

	/// <summary>Gets the longitude of the selected position in degrees.</summary>
	/// <remarks>Only meaningful after the dialog returned <see cref="DialogResult.OK"/>.</remarks>
	public decimal Longitude { get; private set; }

	/// <summary>Gets a value indicating whether a position has been selected.</summary>
	/// <remarks>The OK button is enabled only when this value is <see langword="true"/>.</remarks>
	public bool HasSelection { get; private set; }

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region Constructor

	/// <summary>Initializes a new instance of the <see cref="WorldMapForm"/> class.</summary>
	/// <remarks>Initializes the form components.</remarks>
	public WorldMapForm()
	{
		// Initialize the form components
		InitializeComponent();
		// Log that the form has been initialized
		logger.Info(message: "WorldMapForm initialized.");
	}

	#endregion

	#region Helpers

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString();

	/// <summary>Creates the HTTP client used for geocoding.</summary>
	/// <returns>A configured <see cref="HttpClient"/>.</returns>
	/// <remarks>Sets a User-Agent as required by the Nominatim usage policy.</remarks>
	private static HttpClient CreateHttpClient()
	{
		// Log the creation of the HttpClient for geocoding requests
		logger.Info(message: "Creating HttpClient for geocoding requests.");
		// Create a new HttpClient with a 15-second timeout and a custom User-Agent header
		HttpClient client = new() { Timeout = TimeSpan.FromSeconds(value: 15) };
		// Nominatim requires an identifying User-Agent header
		client.DefaultRequestHeaders.UserAgent.ParseAdd(input: "Planetoid-DB");
		// Log the successful creation of the HttpClient
		logger.Info(message: "HttpClient created with User-Agent 'Planetoid-DB' and 15-second timeout.");
		// Return the configured HttpClient
		return client;
	}

	/// <summary>Stores the selected position and updates the UI.</summary>
	/// <param name="latitude">The latitude in degrees.</param>
	/// <param name="longitude">The longitude in degrees.</param>
	/// <remarks>Enables the OK button and shows the coordinates in the toolbar.</remarks>
	private void SetSelection(decimal latitude, decimal longitude)
	{
		// Log the selected position
		logger.Info(message: $"Position selected: Latitude={latitude}, Longitude={longitude}");
		// Store the selected coordinates
		Latitude = latitude;
		Longitude = longitude;
		// Update the UI to reflect the selection
		HasSelection = true;
		buttonApply.Enabled = true;
		// Update the status label with the formatted coordinates
		labelCoordinates.Text = string.Format(CultureInfo.CurrentCulture, coordinatesFormat, latitude, longitude);
	}

	/// <summary>Searches a location with Nominatim.</summary>
	/// <param name="query">The free-form search text.</param>
	/// <param name="cancellationToken">A token to cancel the request.</param>
	/// <returns>The coordinates of the best match, or <see langword="null"/> if nothing was found.</returns>
	/// <remarks>The request is performed asynchronously.</remarks>
	private static async Task<(decimal Latitude, decimal Longitude)?> GeocodeAsync(string query, CancellationToken cancellationToken)
	{
		// Log the geocoding query
		logger.Info(message: $"Geocoding query: {query}");
		// Construct the request URI for the Nominatim API with the query and proper encoding
		Uri requestUri = new("https://nominatim.openstreetmap.org/search?format=jsonv2&limit=1&q=" + Uri.EscapeDataString(stringToEscape: query));
		// Perform the HTTP GET request asynchronously
		using HttpResponseMessage response = await httpClient.GetAsync(requestUri: requestUri, cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: true);
		// Ensure the response indicates success; throw an exception if not
		_ = response.EnsureSuccessStatusCode();
		// Read the response content as a string asynchronously
		string json = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: true);
		// Log the received JSON response
		logger.Info(message: $"Received JSON response: {json}");
		// Parse the JSON response and extract the coordinates
		using JsonDocument document = JsonDocument.Parse(json: json);
		// Check if the root element is an array and has at least one element
		if (document.RootElement.ValueKind != JsonValueKind.Array || document.RootElement.GetArrayLength() == 0)
		{
			// Log a warning if no results were found in the geocoding response
			logger.Warn(message: "No results found in the geocoding response.");
			return null;
		}
		// Extract the first result from the JSON array
		JsonElement first = document.RootElement[index: 0];
		// Try to parse the latitude and longitude from the first result; return them as a tuple if successful, otherwise return null
		if (decimal.TryParse(s: first.GetProperty(propertyName: "lat").GetString(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out decimal lat)
			&& decimal.TryParse(s: first.GetProperty(propertyName: "lon").GetString(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out decimal lon))
		{
			// Log the successfully parsed coordinates
			logger.Info(message: $"Parsed coordinates: Latitude={lat}, Longitude={lon}");
			return (lat, lon);
		}
		else
		{
			// Log a warning if the coordinates could not be parsed
			logger.Warn(message: "Failed to parse coordinates from the geocoding response.");
			return null;
		}
	}

	/// <summary>Runs the search for the text in the search box.</summary>
	/// <remarks>On success the marker is placed, the view is moved and the coordinates are stored.</remarks>
	private async Task PerformSearchAsync()
	{
		// Log the start of the location search
		logger.Info(message: "Starting location search.");
		// Check if the form is disposed, the cancellation token is requested, or the search button is disabled; if so, return early
		if (IsDisposed || cancellationTokenSource.IsCancellationRequested || !buttonSearch.Enabled)
		{
			// Log a warning that the location search was aborted due to form disposal, cancellation, or button state
			logger.Warn(message: "Location search aborted: form disposed, cancellation requested, or search button disabled.");
			return;
		}
		// Get the trimmed search query from the text box
		string query = textBoxSearch.Text.Trim();
		// Check if the query is empty or if the WebView2 control is not initialized; if so, return early
		if (query.Length == 0 || webView.CoreWebView2 is null)
		{
			// Log a warning that the location search was aborted due to an empty query or uninitialized WebView2
			logger.Warn(message: "Location search aborted: empty query or WebView2 not initialized.");
			return;
		}
		// Disable the search button to prevent multiple concurrent searches
		buttonSearch.Enabled = false;
		// Clear any previous information messages
		labelInformation.Text = string.Empty;
		// Log that the location search is being performed with the specified query
		logger.Info(message: $"Performing location search for query: {query}");
		// Perform the geocoding asynchronously and handle exceptions
		try
		{
			// Await the result of the geocoding request
			(decimal Latitude, decimal Longitude)? result = await GeocodeAsync(query: query, cancellationToken: cancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: true);
			// Check if the form is disposed or the cancellation token is requested; if so, return early
			if (IsDisposed || cancellationTokenSource.IsCancellationRequested)
			{
				// Log a warning that the location search was aborted after geocoding due to form disposal or cancellation
				logger.Warn(message: "Location search aborted after geocoding: form disposed or cancellation requested.");
				return;
			}
			// Check if the result is null (no results found); if so, update the information label and return
			if (result is null)
			{
				// Log a warning that no results were found for the location search query
				logger.Warn(message: "No results found for the location search query.");
				// Update the information label to indicate that no results were found
				labelInformation.Text = I18nStrings.WorldMapNoResults;
				return;
			}
			// Log that a result was found and the selection is being set
			logger.Info(message: $"Location search result found: Latitude={result.Value.Latitude}, Longitude={result.Value.Longitude}. Setting selection.");
			// Set the selected position and update the UI
			SetSelection(latitude: result.Value.Latitude, longitude: result.Value.Longitude);
			// Create the JavaScript code to set the marker on the map with the found coordinates
			string script = string.Create(provider: CultureInfo.InvariantCulture, handler: $"setMarker({result.Value.Latitude}, {result.Value.Longitude}, true);");
			// Log the JavaScript code that will be executed in the WebView2 control
			logger.Info(message: $"Executing JavaScript in WebView2: {script}");
			// Execute the JavaScript code in the WebView2 control to place the marker and adjust the view
			_ = await webView.CoreWebView2.ExecuteScriptAsync(javaScript: script).ConfigureAwait(continueOnCapturedContext: true);
		}
		// Catch exceptions that occur due to form disposal or cancellation and ignore them
		catch (Exception) when (IsDisposed || cancellationTokenSource.IsCancellationRequested)
		{
			// Log a warning that the location search was aborted due to form disposal or cancellation
			logger.Warn(message: "Location search aborted due to form disposal or cancellation.");
		}
		// Catch specific exceptions related to HTTP requests, task cancellation, JSON parsing, or missing keys
		catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException)
		{
			// Log an error that the location search failed due to an exception
			logger.Error(exception: ex, message: "Location search failed.");
			// Update the information label to indicate that the search failed
			labelInformation.Text = I18nStrings.WorldMapSearchFailed;
		}
		// Catch any other unexpected exceptions
		catch (Exception ex)
		{
			// Log an error that an unexpected error occurred during the location search
			logger.Error(exception: ex, message: "Unexpected error during location search.");
			// Update the information label to indicate that an unexpected error occurred
			labelInformation.Text = I18nStrings.WorldMapSearchFailed;
		}
		// Ensure that the search button is re-enabled if the form is not disposed and the cancellation token is not requested
		finally
		{
			// Log that the location search has completed and the search button will be re-enabled if appropriate
			logger.Info(message: "Location search completed. Re-enabling search button if form is not disposed and cancellation is not requested.");
			// Re-enable the search button if the form is still active and not cancelled
			if (!IsDisposed && !cancellationTokenSource.IsCancellationRequested)
			{
				buttonSearch.Enabled = true;
			}
		}
	}

	#endregion

	#region Event handlers

	/// <summary>Initializes WebView2 and loads the map when the form is loaded.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Shows an error message in the status bar if the WebView2 runtime is unavailable.</remarks>
	private async void WorldMapForm_Load(object? sender, EventArgs e)
	{
		// Log that the form is loading and WebView2 initialization is starting
		logger.Info(message: "WorldMapForm loading. Initializing WebView2.");
		// Attempt to ensure that the WebView2 control is initialized and ready
		try
		{
			// Await the asynchronous initialization of the WebView2 control
			await webView.EnsureCoreWebView2Async().ConfigureAwait(continueOnCapturedContext: true);
			// Check if the form has been disposed during the asynchronous operation; if so, return early
			if (IsDisposed)
			{
				// Log a warning that the form was disposed during WebView2 initialization and further setup is being aborted
				logger.Warn(message: "WorldMapForm disposed during WebView2 initialization. Aborting further setup.");
				return;
			}
			// Log that WebView2 has been successfully initialized and event handlers are being attached
			logger.Info(message: "WebView2 initialized successfully. Attaching event handlers.");
			// Attach event handlers for navigation starting and web message received events
			webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
			webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
			// Disable the default context menus in the WebView2 control to prevent right-click actions
			webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			// Log that the map HTML content is being loaded into the WebView2 control
			logger.Info(message: "Loading map HTML content into WebView2.");
			// Load the HTML content for the map into the WebView2 control
			webView.NavigateToString(htmlContent: MapHtml);
		}
		// Catch any exceptions that occur during WebView2 initialization
		catch (Exception ex)
		{
			// Log an error that WebView2 could not be initialized and update the information label with an error message
			if (IsDisposed)
			{
				// Log a warning that the form was disposed during WebView2 initialization exception handling and further setup is being aborted
				logger.Warn(message: "WorldMapForm disposed during WebView2 initialization exception handling. Aborting further setup.");
				return;
			}
			// Log the exception details and update the information label to indicate that initialization failed
			logger.Error(exception: ex, message: "WebView2 could not be initialized.");
			// Update the information label to indicate that the world map initialization failed
			labelInformation.Text = I18nStrings.WorldMapInitializationFailed;
		}
	}

	/// <summary>Opens web links externally and prevents navigation away from the map.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The navigation event data.</param>
	/// <remarks>Links with <c>http</c> or <c>https</c> schemes are opened in the default browser; all other navigation is canceled except for <c>about:blank</c>.</remarks>
	private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
	{
		// Log the navigation attempt with the target URI
		logger.Info(message: $"Navigation attempt to URI: {e.Uri}");
		// Check if the URI is a valid absolute URI with http or https scheme
		if (Uri.TryCreate(uriString: e.Uri, uriKind: UriKind.Absolute, result: out Uri? uri) && uri.Scheme is "http" or "https")
		{
			// Log that the external link will be opened in the default browser and navigation will be canceled
			logger.Info(message: $"Opening external link in default browser: {uri.AbsoluteUri}. Navigation will be canceled.");
			// Cancel the navigation to prevent leaving the map page
			e.Cancel = true;
			// Attempt to open the external link in the default browser
			try
			{
				// Use Process.Start with UseShellExecute to open the link in the default browser
				_ = Process.Start(startInfo: new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
			}
			// Catch any exceptions that occur while trying to open the external link
			catch (Exception ex)
			{
				// Log a warning that the external link could not be opened
				logger.Warn(exception: ex, message: "Could not open external link.");
			}
		}
		// Check if the URI is not "about:blank"; if so, cancel the navigation to prevent leaving the map page
		else if (e.Uri != "about:blank")
		{
			// Log a warning that navigation to the specified URI is not allowed and will be canceled
			logger.Warn(message: $"Navigation to URI '{e.Uri}' is not allowed. Navigation will be canceled.");
			// Cancel the navigation to prevent leaving the map page
			e.Cancel = true;
		}
	}

	/// <summary>Handles messages posted by the map page.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The message event data.</param>
	/// <remarks>The message is a JSON object with <c>lat</c> and <c>lng</c> numbers.</remarks>
	private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
	{
		// Log the received web message from the map page
		logger.Info(message: $"Received web message from the map page: {e.WebMessageAsJson}");
		// Attempt to parse the JSON message and extract the latitude and longitude
		try
		{
			// Parse the JSON message received from the map page
			using JsonDocument document = JsonDocument.Parse(json: e.WebMessageAsJson);
			// Extract the latitude and longitude properties from the JSON document
			decimal lat = document.RootElement.GetProperty(propertyName: "lat").GetDecimal();
			decimal lng = document.RootElement.GetProperty(propertyName: "lng").GetDecimal();
			// Log the extracted latitude and longitude values
			logger.Info(message: $"Extracted coordinates from web message: Latitude={lat}, Longitude={lng}");
			// Store the selected position and update the UI, clamping the values to valid ranges
			SetSelection(latitude: Math.Clamp(value: lat, min: -90m, max: 90m), longitude: Math.Clamp(value: lng, min: -180m, max: 180m));
		}
		// Catch exceptions related to JSON parsing, missing keys, invalid operations, or format issues
		catch (Exception ex) when (ex is JsonException or KeyNotFoundException or InvalidOperationException or FormatException)
		{
			// Log a warning that an invalid message was received from the map
			logger.Warn(exception: ex, message: "Invalid message received from the map.");
		}
	}

	/// <summary>Returns the selected coordinates and closes the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Sets <see cref="Form.DialogResult"/> to <see cref="DialogResult.OK"/>.</remarks>
	private void ButtonApply_Click(object? sender, EventArgs e)
	{
		// Log that the OK button was clicked and the form will close with the selected coordinates
		logger.Info(message: "OK button clicked. Closing form with selected coordinates.");
		// Set the dialog result to OK to indicate that coordinates have been selected
		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>Closes the form without returning coordinates.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Sets <see cref="Form.DialogResult"/> to <see cref="DialogResult.Cancel"/>.</remarks>
	private void ButtonCancel_Click(object? sender, EventArgs e)
	{
		// Log that the Cancel button was clicked and the form will close without returning coordinates
		logger.Info(message: "Cancel button clicked. Closing form without returning coordinates.");
		// Set the dialog result to Cancel to indicate that no coordinates have been selected
		DialogResult = DialogResult.Cancel;
		Close();
	}

	/// <summary>Starts the location search.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Delegates to <see cref="PerformSearchAsync"/>.</remarks>
	private async void ButtonSearch_Click(object? sender, EventArgs e)
	{
		// Log that the Search button was clicked and a location search will start
		logger.Info(message: "Search button clicked. Starting location search.");
		// Start the location search asynchronously
		await PerformSearchAsync().ConfigureAwait(continueOnCapturedContext: true);
	}

	/// <summary>Starts the location search when Enter is pressed in the search box.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The key event data.</param>
	/// <remarks>The key press is handled to avoid a system beep.</remarks>
	private async void TextBoxSearch_KeyDown(object? sender, KeyEventArgs e)
	{
		// Log that a key was pressed in the search box and check if it is the Enter key
		logger.Info(message: $"Key pressed in search box: {e.KeyCode}");
		// Check if the pressed key is Enter
		if (e.KeyCode == Keys.Enter)
		{
			// Log that the Enter key was pressed and the location search will start
			logger.Info(message: "Enter key pressed in search box. Starting location search.");
			// Mark the key event as handled and suppress the key press to avoid a system beep
			e.Handled = true;
			e.SuppressKeyPress = true;
			// Start the location search asynchronously
			await PerformSearchAsync().ConfigureAwait(continueOnCapturedContext: true);
		}
	}

	/// <summary>Cancels pending work when the form is closing.</summary>
	/// <param name="e">The event data.</param>
	/// <remarks>Cancels the <see cref="CancellationTokenSource"/> to stop any ongoing geocoding requests.</remarks>
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		// Log that the form is closing and any pending work will be canceled
		logger.Info(message: "WorldMapForm is closing. Canceling any pending work.");
		// Cancel the cancellation token source to stop any ongoing geocoding requests
		cancellationTokenSource.Cancel();
		// Call the base class implementation to ensure proper form closing behavior
		base.OnFormClosing(e: e);
	}

	#endregion
}
