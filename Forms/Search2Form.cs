using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Text;

namespace Planetoid_DB
{
	public partial class Search2Form : Planetoid_DB.Forms.BaseKryptonForm
	{
		private List<SearchResult> _searchResults = new();
		private CancellationTokenSource? _cts;
		private readonly Dictionary<string, Func<PlanetoidRecord, string>> _propertyMap = new();

		private struct SearchResult
		{
			public string Index;
			public string Designation;
			public string Element;
			public string Value;
		}

		public Search2Form()
		{
			InitializeComponent();
			InitializePropertyMap();
			listViewResults.DoubleClick += ListViewResults_DoubleClick;
		}

		private void InitializePropertyMap()
		{
			_propertyMap.Add("Index No.", static r => r.Index);
			_propertyMap.Add("Readable designation", static r => r.DesignationName);
			_propertyMap.Add("Epoch", static r => r.Epoch);
			_propertyMap.Add("Mean anomaly", static r => r.MeanAnomaly);
			_propertyMap.Add("Argument of perihelion", static r => r.ArgPeri);
			_propertyMap.Add("Longitude of ascending node", static r => r.LongAscNode);
			_propertyMap.Add("Inclination", static r => r.Incl);
			_propertyMap.Add("Orbital eccentricity", static r => r.OrbEcc);
			_propertyMap.Add("Mean daily motion", static r => r.Motion);
			_propertyMap.Add("Semi-major axis", static r => r.SemiMajorAxis);
			_propertyMap.Add("Absolute magnitude", static r => r.MagAbs);
			_propertyMap.Add("Slope parameter", static r => r.SlopeParam);
			_propertyMap.Add("Reference", static r => r.Ref);
			_propertyMap.Add("Number of observations", static r => r.NumberObservation);
			_propertyMap.Add("Number of oppositions", static r => r.NumberOpposition);
			_propertyMap.Add("Observation span", static r => r.ObsSpan);
			_propertyMap.Add("r.m.s. residual", static r => r.RmsResidual);
			_propertyMap.Add("Computer name", static r => r.ComputerName);
			_propertyMap.Add("Flags", static r => r.Flags);
			_propertyMap.Add("Date of last observation", static r => r.ObservationLastDate);
		}

		private void Search2Form_Load(object sender, EventArgs e)
		{
			foreach (var key in _propertyMap.Keys)
			{
				kryptonCheckedListBoxElements.Items.Add(key);
			}
		}

		private async void kryptonButtonSearch_Click(object sender, EventArgs e)
		{
			var searchText = kryptonTextBoxSearch.Text;
			if (string.IsNullOrWhiteSpace(searchText))
			{
				MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (kryptonCheckedListBoxElements.CheckedItems.Count == 0)
			{
				MessageBox.Show("Please select at least one orbital element to search in.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var filePath = Settings.Default.systemFilenameMpcorb;
			if (!File.Exists(filePath))
			{
				MessageBox.Show($"Database file not found at: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (_cts != null)
			{
				_cts.Cancel();
				_cts.Dispose();
			}

			_cts = new CancellationTokenSource();
			var token = _cts.Token;

			var selectedKeys = kryptonCheckedListBoxElements.CheckedItems.Cast<string>().ToList();
			var fullText = kryptonCheckBoxFullText.Checked;

			kryptonButtonSearch.Enabled = false;
			kryptonButtonCancel.Enabled = true;
			kryptonProgressBar.Value = 0;
			kryptonProgressBar.Values.Text = "0 %";

			lock (_searchResults)
			{
				_searchResults.Clear();
			}
			listViewResults.VirtualListSize = 0;
			listViewResults.Invalidate();

			kryptonLabelStatus.Text = "Searching...";

			var activeFilters = _propertyMap.Where(kv => selectedKeys.Contains(kv.Key)).ToList();

			try
			{
				await Task.Run(() =>
				{
					using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
					using var reader = new StreamReader(fs, Encoding.UTF8);

					long totalLength = fs.Length;
					long processedBytes = 0;
					int lineCount = 0;

					string? line;
					while ((line = reader.ReadLine()) != null)
					{
						if (token.IsCancellationRequested)
						{
							break;
						}

						processedBytes += Encoding.UTF8.GetByteCount(line) + 2;
						lineCount++;

						if (line.Length < 200)
						{
							continue;
						}

						var record = PlanetoidRecord.Parse(line);
						if (string.IsNullOrWhiteSpace(record.DesignationName))
						{
							continue;
						}

						foreach (var filter in activeFilters)
						{
							var value = filter.Value(record);
							bool match = false;
							if (fullText)
							{
								if (string.Equals(value, searchText, StringComparison.OrdinalIgnoreCase))
								{
									match = true;
								}
							}
							else
							{
								if (value.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									match = true;
								}
							}

							if (match)
							{
								var result = new SearchResult
								{
									Index = record.Index,
									Designation = record.DesignationName,
									Element = filter.Key,
									Value = value
								};

								lock (_searchResults)
								{
									_searchResults.Add(result);
								}
							}
						}

						if (lineCount % 2000 == 0)
						{
							int pct = (int)((double)processedBytes / totalLength * 100);
							if (pct > 100)
							{
								pct = 100;
							}

							if (IsHandleCreated)
							{
								Invoke(new Action(() =>
								{
									kryptonProgressBar.Value = pct;
									kryptonProgressBar.Values.Text = $"{pct} %";

									lock (_searchResults)
									{
										listViewResults.VirtualListSize = _searchResults.Count;
									}
								}));
							}
						}
					}
				}, token);

				kryptonLabelStatus.Text = token.IsCancellationRequested ? "Search cancelled." : $"Search completed. Found {_searchResults.Count} entries.";
				kryptonProgressBar.Value = 100;
				kryptonProgressBar.Values.Text = "100 %";
			}
			catch (Exception ex)
			{
				kryptonLabelStatus.Text = "Error during search.";
				MessageBox.Show($"Search failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				kryptonButtonSearch.Enabled = true;
				kryptonButtonCancel.Enabled = false;

				lock (_searchResults)
				{
					listViewResults.VirtualListSize = _searchResults.Count;
				}
				listViewResults.Invalidate();
				_cts?.Dispose();
				_cts = null;
			}
		}

		private void kryptonButtonCancel_Click(object sender, EventArgs e)
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				_cts.Cancel();
			}
		}

		private void listViewResults_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			SearchResult item;
			lock (_searchResults)
			{
				if (e.ItemIndex >= 0 && e.ItemIndex < _searchResults.Count)
				{
					item = _searchResults[e.ItemIndex];
				}
				else
				{
					return; // Should not happen if VirtualListSize is managed correctly
				}
			}

			var lvi = new ListViewItem(item.Index);
			lvi.SubItems.Add(item.Designation);
			lvi.SubItems.Add(item.Element);
			lvi.SubItems.Add(item.Value);
			e.Item = lvi;
		}

		private void ListViewResults_DoubleClick(object? sender, EventArgs e)
		{
			if (_cts != null)
			{
				return;
			}

			if (listViewResults.SelectedIndices.Count == 0)
			{
				return;
			}

			int selectedIndex = listViewResults.SelectedIndices[0];
			SearchResult item;
			lock (_searchResults)
			{
				if (selectedIndex >= 0 && selectedIndex < _searchResults.Count)
				{
					item = _searchResults[selectedIndex];
				}
				else
				{
					return;
				}
			}

			if (Application.OpenForms.OfType<PlanetoidDbForm>().FirstOrDefault() is PlanetoidDbForm mainForm)
			{
				mainForm.JumpToRecord(item.Index, item.Designation);
				mainForm.BringToFront();
			}
		}
	}
}
