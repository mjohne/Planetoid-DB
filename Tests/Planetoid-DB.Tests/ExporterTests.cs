/*
 * File:        ExporterTests.cs
 * Project:     Planetoid-DB.Tests
 * Namespace:   Planetoid_DB.Tests
 * Description: Unit tests for the orbit data exporters.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Export;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using System.Xml.Linq;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for the <see cref="IOrbitDataExporter"/> implementations.</summary>
/// <remarks>These tests verify that the various orbit data exporters correctly write files with the expected content and metadata.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed class ExporterTests : IDisposable
{
	/// <summary>Temporary directory receiving the exported files.</summary>
	/// <remarks>This directory is created for each test run and deleted after the tests complete.</remarks>
	private readonly string tempDirectory = Directory.CreateTempSubdirectory(prefix: "planetoid-db-tests-").FullName;

	/// <summary>Sample data used for the exports.</summary>
	/// <remarks>This dictionary contains sample orbit data used for testing the exporters.</remarks>
	private static readonly Dictionary<string, string> SampleData = new()
	{
		// The sample data contains three key-value pairs representing orbit data for the celestial body Ceres.
		[key: "Designation"] = "Ceres",
		[key: "Semi-major axis"] = "2.7660",
		[key: "Eccentricity"] = "0.0785",
	};

	/// <summary>Gets the type names of all exporters under test.</summary>
	/// <remarks>This property returns a collection of exporter type names used for parameterized tests.</remarks>
	public static TheoryData<string> ExporterNames =>
	[
		nameof(BbcodeExporter), nameof(CreoleExporter), nameof(CsvExporter), nameof(EpubExporter), nameof(ExcelExporter),
		nameof(HtmlExporter), nameof(IcsExporter), nameof(JsonExporter), nameof(LatexExporter), nameof(MarkdownExporter),
		nameof(MobiExporter), nameof(OdsExporter), nameof(OdtExporter), nameof(PdfExporter), nameof(PostscriptExporter),
		nameof(PsvExporter), nameof(RtfExporter), nameof(SqlExporter), nameof(TextExporter), nameof(TsvExporter),
		nameof(VcalExporter), nameof(WordExporter), nameof(XcalExporter), nameof(XmlExporter), nameof(YamlExporter),
	];

	/// <summary>Creates an exporter instance by its type name.</summary>
	/// <param name="name">The exporter type name.</param>
	/// <returns>The exporter instance.</returns>
	/// <remarks>This method uses reflection to create an instance of the specified exporter type.</remarks>
	private static IOrbitDataExporter Create(string name)
	{
		// The exporter types are located in the Planetoid_DB.Export namespace.
		Type type = typeof(CsvExporter).Assembly.GetType(name: $"Planetoid_DB.Export.{name}", throwOnError: true)!;
		// Create an instance of the exporter type using Activator.CreateInstance.
		return (IOrbitDataExporter)Activator.CreateInstance(type: type)!;
	}

	/// <summary>Exports the sample data with the given exporter and returns the file path.</summary>
	/// <param name="exporter">The exporter.</param>
	/// <returns>The path of the exported file.</returns>
	/// <remarks>This method exports the sample data using the specified exporter and returns the path of the generated file.</remarks>
	private string ExportSample(IOrbitDataExporter exporter)
	{
		// Combine the temporary directory path with the exporter's file extension to create the output file path.
		string path = Path.Combine(path1: tempDirectory, path2: $"export.{exporter.Extension}");
		// Call the Export method of the exporter to write the sample data to the specified file path.
		exporter.Export(filePath: path, exportTitle: "Test export", selectedData: SampleData);
		// Return the path of the exported file.
		return path;
	}

	/// <summary>Deletes the temporary directory.</summary>
	/// <remarks>This method is called after the tests complete to clean up the temporary directory.</remarks>
	public void Dispose()
	{
		// Delete the temporary directory and its contents recursively.
		Directory.Delete(path: tempDirectory, recursive: true);
	}

	/// <summary>Verifies exporter metadata is consistent.</summary>
	/// <param name="name">The exporter type name.</param>
	/// <remarks>This test checks that the exporter metadata (extension, filter, title) is valid and consistent.</remarks>
	[Theory]
	[MemberData(nameof(ExporterNames))]
	public void ExporterHasConsistentMetadata(string name)
	{
		// Create an exporter instance using the specified type name.
		IOrbitDataExporter exporter = Create(name: name);
		// Verify that the exporter extension is not null or whitespace.
		Assert.False(condition: string.IsNullOrWhiteSpace(value: exporter.Extension));
		// Verify that the exporter extension does not start with a period.
		Assert.False(condition: exporter.Extension.StartsWith(value: '.'));
		// Verify that the exporter filter contains the expected pattern.
		Assert.Contains(expectedSubstring: $"*.{exporter.Extension}", actualString: exporter.Filter, comparisonType: StringComparison.OrdinalIgnoreCase);
		// Verify that the exporter title is not null or whitespace.
		Assert.False(condition: string.IsNullOrWhiteSpace(value: exporter.Title));
	}

	/// <summary>Verifies each exporter writes a non-empty file containing the exported values.</summary>
	/// <param name="name">The exporter type name.</param>
	/// <remarks>This test checks that each exporter writes a file with the expected content, including the sample data values.</remarks>
	[Theory]
	[MemberData(nameof(ExporterNames))]
	public void ExporterWritesFileContainingData(string name)
	{
		// Export the sample data using the specified exporter and get the path of the exported file.
		string path = ExportSample(exporter: Create(name: name));
		// Verify that the exported file exists.
		Assert.True(condition: File.Exists(path: path));
		// Read the contents of the exported file as a byte array.
		byte[] bytes = File.ReadAllBytes(path: path);
		// Verify that the byte array is not empty.
		Assert.NotEmpty(collection: bytes);
		// Determine the content of the exported file based on its format (ZIP or text).
		string content;
		// If the file is a ZIP archive (e.g., EPUB, ODS, ODT), extract the text content from the entries.
		if (bytes is [(byte)'P', (byte)'K', ..])
		{
			// Open the ZIP archive for reading.
			using ZipArchive archive = ZipFile.OpenRead(archiveFileName: path);
			// Concatenate the text content of all entries in the ZIP archive.
			content = string.Concat(values: archive.Entries.Select(selector: static entry =>
			{
				// Read the content of the entry as text using a StreamReader.
				using StreamReader reader = new(stream: entry.Open());
				// Return the text content of the entry.
				return reader.ReadToEnd();
			}));
		}
		// If the file is not a ZIP archive, decode the byte array as Latin1 text.
		else
		{
			// Decode the byte array as Latin1 text to get the content of the exported file.
			content = System.Text.Encoding.Latin1.GetString(bytes: bytes);
		}
		// Verify that the content contains the expected sample data values.
		Assert.Contains(expectedSubstring: "Ceres", actualString: content);
		// Verify that the content contains the expected sample data values.
		Assert.Contains(expectedSubstring: "0.0785", actualString: content);
	}

	/// <summary>Verifies the CSV exporter writes one semicolon-separated line per entry.</summary>
	[Fact]
	public void CsvExporterWritesKeyValueLines()
	{
		string[] lines = File.ReadAllLines(path: ExportSample(exporter: new CsvExporter()));

		Assert.Equal(expected: ["Designation;Ceres", "Semi-major axis;2.7660", "Eccentricity;0.0785"], actual: lines);
	}

	/// <summary>Verifies the JSON exporter output round-trips to the original data.</summary>
	/// <remarks>This test checks that the JSON exporter produces valid JSON that can be deserialized back into the original dictionary.</remarks>
	[Fact]
	public void JsonExporterRoundTripsData()
	{
		// Export the sample data using the JSON exporter and get the path of the exported file.
		string path = ExportSample(exporter: new JsonExporter());
		// Read the contents of the exported file as a JSON string.
		string json = File.ReadAllText(path: path);
		// Deserialize the JSON string into a dictionary.
		Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json: json);
		// Verify that the deserialized data is not null.
		Assert.Equal(expected: SampleData, actual: data);
	}

	/// <summary>Verifies the XML exporter writes well-formed XML containing the title and all fields.</summary>
	/// <remarks>This test checks that the XML exporter produces valid XML that contains the expected title and field elements with the correct values.</remarks>
	[Fact]
	public void XmlExporterWritesWellFormedXml()
	{
		// Load the exported XML document using the XML exporter.
		XDocument document = XDocument.Load(uri: ExportSample(exporter: new XmlExporter()));
		// Define the XML namespace used in the exported XML document.
		XNamespace ns = "https://github.com/mjohne/Planetoid-DB";
		// Verify that the XML document contains the expected title element with the correct value.
		Assert.Contains(expected: "Test export", collection: document.Descendants(name: ns + "Title").Select(selector: static e => e.Value));
		// Extract the field elements from the XML document and convert them into a dictionary of key-value pairs.
		Dictionary<string, string> exportedData = document.Descendants(name: ns + "Field")
			.ToDictionary(
				keySelector: static field => field.Attribute(name: "name")?.Value ?? string.Empty,
				elementSelector: static field => field.Attribute(name: "value")?.Value ?? string.Empty,
				comparer: StringComparer.Ordinal);
		// Verify that the exported data matches the expected sample data.
		Assert.Equal(expected: SampleData, actual: exportedData);
	}

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This property is used to provide a visual representation of the object in the debugger.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
