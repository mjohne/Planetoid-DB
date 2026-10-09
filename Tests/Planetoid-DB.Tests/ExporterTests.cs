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

using System.IO.Compression;
using System.Text.Json;
using System.Xml.Linq;

namespace Planetoid_DB.Tests;

/// <summary>Unit tests for the <see cref="IOrbitDataExporter"/> implementations.</summary>
public sealed class ExporterTests : IDisposable
{
	/// <summary>Temporary directory receiving the exported files.</summary>
	private readonly string tempDirectory = Directory.CreateTempSubdirectory(prefix: "planetoid-db-tests-").FullName;

	/// <summary>Sample data used for the exports.</summary>
	private static readonly Dictionary<string, string> SampleData = new()
	{
		["Designation"] = "Ceres",
		["Semi-major axis"] = "2.7660",
		["Eccentricity"] = "0.0785",
	};

	/// <summary>Gets the type names of all exporters under test.</summary>
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
	private static IOrbitDataExporter Create(string name)
	{
		Type type = typeof(CsvExporter).Assembly.GetType(name: $"Planetoid_DB.Export.{name}", throwOnError: true)!;
		return (IOrbitDataExporter)Activator.CreateInstance(type: type)!;
	}

	/// <summary>Exports the sample data with the given exporter and returns the file path.</summary>
	/// <param name="exporter">The exporter.</param>
	/// <returns>The path of the exported file.</returns>
	private string ExportSample(IOrbitDataExporter exporter)
	{
		string path = Path.Combine(path1: tempDirectory, path2: $"export.{exporter.Extension}");
		exporter.Export(filePath: path, exportTitle: "Test export", selectedData: SampleData);
		return path;
	}

	/// <summary>Deletes the temporary directory.</summary>
	public void Dispose() => Directory.Delete(path: tempDirectory, recursive: true);

	/// <summary>Verifies exporter metadata is consistent.</summary>
	/// <param name="name">The exporter type name.</param>
	[Theory]
	[MemberData(nameof(ExporterNames))]
	public void Exporter_HasConsistentMetadata(string name)
	{
		IOrbitDataExporter exporter = Create(name: name);

		Assert.False(condition: string.IsNullOrWhiteSpace(value: exporter.Extension));
		Assert.False(condition: exporter.Extension.StartsWith(value: '.'));
		Assert.Contains(expectedSubstring: $"*.{exporter.Extension}", actualString: exporter.Filter, comparisonType: StringComparison.OrdinalIgnoreCase);
		Assert.False(condition: string.IsNullOrWhiteSpace(value: exporter.Title));
	}

	/// <summary>Verifies each exporter writes a non-empty file containing the exported values.</summary>
	/// <param name="name">The exporter type name.</param>
	[Theory]
	[MemberData(nameof(ExporterNames))]
	public void Exporter_WritesFileContainingData(string name)
	{
		string path = ExportSample(exporter: Create(name: name));

		Assert.True(condition: File.Exists(path: path));
		byte[] bytes = File.ReadAllBytes(path: path);
		Assert.NotEmpty(collection: bytes);

		string content;
		if (bytes is [(byte)'P', (byte)'K', ..])
		{
			using ZipArchive archive = ZipFile.OpenRead(archiveFileName: path);
			content = string.Concat(values: archive.Entries.Select(selector: static entry =>
			{
				using StreamReader reader = new(stream: entry.Open());
				return reader.ReadToEnd();
			}));
		}
		else
		{
			content = System.Text.Encoding.Latin1.GetString(bytes: bytes);
		}
		Assert.Contains(expectedSubstring: "Ceres", actualString: content);
		Assert.Contains(expectedSubstring: "0.0785", actualString: content);
	}

	/// <summary>Verifies the CSV exporter writes one semicolon-separated line per entry.</summary>
	[Fact]
	public void CsvExporter_WritesKeyValueLines()
	{
		string[] lines = File.ReadAllLines(path: ExportSample(exporter: new CsvExporter()));

		Assert.Equal(expected: ["Designation;Ceres", "Semi-major axis;2.7660", "Eccentricity;0.0785"], actual: lines);
	}

	/// <summary>Verifies the JSON exporter output round-trips to the original data.</summary>
	[Fact]
	public void JsonExporter_RoundTripsData()
	{
		Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json: File.ReadAllText(path: ExportSample(exporter: new JsonExporter())));

		Assert.Equal(expected: SampleData, actual: data);
	}

	/// <summary>Verifies the XML exporter writes well-formed XML containing the title and all fields.</summary>
	[Fact]
	public void XmlExporter_WritesWellFormedXml()
	{
		XDocument document = XDocument.Load(uri: ExportSample(exporter: new XmlExporter()));
		XNamespace ns = "https://github.com/mjohne/Planetoid-DB";

		Assert.Contains(expected: "Test export", collection: document.Descendants(name: ns + "Title").Select(selector: static e => e.Value));
		Assert.Equal(expected: SampleData.Count, actual: document.Descendants(name: ns + "Field").Count());
	}
}
