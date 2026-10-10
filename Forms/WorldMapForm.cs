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
using System.Net.Http;
using System.Text.Json;
using System.Threading;

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
	private readonly CancellationTokenSource cancellationTokenSource = new();

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
		HttpClient client = new() { Timeout = TimeSpan.FromSeconds(value: 15) };
		client.DefaultRequestHeaders.UserAgent.ParseAdd(input: "Planetoid-DB");
		return client;
	}

	/// <summary>Stores the selected position and updates the UI.</summary>
	/// <param name="latitude">The latitude in degrees.</param>
	/// <param name="longitude">The longitude in degrees.</param>
	/// <remarks>Enables the OK button and shows the coordinates in the toolbar.</remarks>
	private void SetSelection(decimal latitude, decimal longitude)
	{
		Latitude = latitude;
		Longitude = longitude;
		HasSelection = true;
		buttonApply.Enabled = true;
		labelCoordinates.Text = string.Format(CultureInfo.CurrentCulture, I18nStrings.WorldMapCoordinatesFormat, latitude, longitude);
	}

	/// <summary>Searches a location with Nominatim.</summary>
	/// <param name="query">The free-form search text.</param>
	/// <returns>The coordinates of the best match, or <see langword="null"/> if nothing was found.</returns>
	/// <remarks>The request is performed asynchronously.</remarks>
	private static async Task<(decimal Latitude, decimal Longitude)?> GeocodeAsync(string query, CancellationToken cancellationToken)
	{
		string url = "https://nominatim.openstreetmap.org/search?format=jsonv2&limit=1&q=" + Uri.EscapeDataString(stringToEscape: query);
		using HttpResponseMessage response = await httpClient.GetAsync(requestUri: url, cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: true);
		response.EnsureSuccessStatusCode();
		string json = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: true);
		using JsonDocument document = JsonDocument.Parse(json: json);
		if (document.RootElement.ValueKind != JsonValueKind.Array || document.RootElement.GetArrayLength() == 0)
		{
			return null;
		}
		JsonElement first = document.RootElement[index: 0];
		if (decimal.TryParse(s: first.GetProperty(propertyName: "lat").GetString(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out decimal lat)
			&& decimal.TryParse(s: first.GetProperty(propertyName: "lon").GetString(), style: NumberStyles.Float, provider: CultureInfo.InvariantCulture, result: out decimal lon))
		{
			return (lat, lon);
		}
		return null;
	}

	/// <summary>Runs the search for the text in the search box.</summary>
	/// <remarks>On success the marker is placed, the view is moved and the coordinates are stored.</remarks>
	private async Task PerformSearchAsync()
	{
		if (IsDisposed || cancellationTokenSource.IsCancellationRequested || buttonSearch.Enabled is false)
		{
			return;
		}
		string query = textBoxSearch.Text.Trim();
		if (query.Length == 0 || webView.CoreWebView2 is null)
		{
			return;
		}
		buttonSearch.Enabled = false;
		try
		{
			(decimal Latitude, decimal Longitude)? result = await GeocodeAsync(query: query, cancellationToken: cancellationTokenSource.Token);
			if (IsDisposed || cancellationTokenSource.IsCancellationRequested)
			{
				return;
			}
			if (result is null)
			{
				labelInformation.Text = I18nStrings.WorldMapNoResults;
				return;
			}
			SetSelection(latitude: result.Value.Latitude, longitude: result.Value.Longitude);
			string script = string.Create(provider: CultureInfo.InvariantCulture, handler: $"setMarker({result.Value.Latitude}, {result.Value.Longitude}, true);");
			await webView.CoreWebView2.ExecuteScriptAsync(javaScript: script);
		}
		catch (Exception) when (IsDisposed || cancellationTokenSource.IsCancellationRequested)
		{
		}
		catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException)
		{
			logger.Error(exception: ex, message: "Location search failed.");
			labelInformation.Text = I18nStrings.WorldMapSearchFailed;
		}
		finally
		{
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
		try
		{
			await webView.EnsureCoreWebView2Async();
			if (IsDisposed)
			{
				return;
			}
			webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
			webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
			webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			webView.NavigateToString(htmlContent: MapHtml);
		}
		catch (Exception ex)
		{
			if (IsDisposed)
			{
				return;
			}
			logger.Error(exception: ex, message: "WebView2 could not be initialized.");
			labelInformation.Text = I18nStrings.WorldMapInitializationFailed;
		}
	}

	/// <summary>Opens web links externally and prevents navigation away from the map.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The navigation event data.</param>
	private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
	{
		if (Uri.TryCreate(uriString: e.Uri, uriKind: UriKind.Absolute, result: out Uri? uri)
			&& uri.Scheme is "http" or "https")
		{
			e.Cancel = true;
			try
			{
				_ = Process.Start(startInfo: new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
			}
			catch (Exception ex)
			{
				logger.Warn(exception: ex, message: "Could not open external link.");
			}
		}
		else if (e.Uri != "about:blank")
		{
			e.Cancel = true;
		}
	}

	/// <summary>Handles messages posted by the map page.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The message event data.</param>
	/// <remarks>The message is a JSON object with <c>lat</c> and <c>lng</c> numbers.</remarks>
	private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
	{
		try
		{
			using JsonDocument document = JsonDocument.Parse(json: e.WebMessageAsJson);
			decimal lat = document.RootElement.GetProperty(propertyName: "lat").GetDecimal();
			decimal lng = document.RootElement.GetProperty(propertyName: "lng").GetDecimal();
			SetSelection(latitude: Math.Clamp(value: lat, min: -90m, max: 90m), longitude: Math.Clamp(value: lng, min: -180m, max: 180m));
		}
		catch (Exception ex) when (ex is JsonException or KeyNotFoundException or InvalidOperationException or FormatException)
		{
			logger.Warn(exception: ex, message: "Invalid message received from the map.");
		}
	}

	/// <summary>Returns the selected coordinates and closes the form.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Sets <see cref="Form.DialogResult"/> to <see cref="DialogResult.OK"/>.</remarks>
	private void ButtonApply_Click(object? sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>Closes the form without returning coordinates.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Sets <see cref="Form.DialogResult"/> to <see cref="DialogResult.Cancel"/>.</remarks>
	private void ButtonCancel_Click(object? sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
		Close();
	}

	/// <summary>Starts the location search.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>Delegates to <see cref="PerformSearchAsync"/>.</remarks>
	private async void ButtonSearch_Click(object? sender, EventArgs e)
	{
		await PerformSearchAsync();
	}

	/// <summary>Starts the location search when Enter is pressed in the search box.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The key event data.</param>
	/// <remarks>The key press is handled to avoid a system beep.</remarks>
	private async void TextBoxSearch_KeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			await PerformSearchAsync();
		}
	}

	/// <summary>Cancels pending work when the form is closing.</summary>
	/// <param name="e">The event data.</param>
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		cancellationTokenSource.Cancel();
		base.OnFormClosing(e);
	}

	#endregion
}
