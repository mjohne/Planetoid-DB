﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using Office2007Rendering;
using VS2008StripRenderingLibrary;

namespace Planetoid_DB
{
	/// <summary>
	/// 
	/// </summary>
	public partial class PlanetoidDBForm : Form
	{
		private int currentPosition = 0, stepPosition = 0;
		private readonly ArrayList planetoidDatabase = new ArrayList(capacity: 0);
		private readonly ArrayList derivatedOrbitElementsDatabase = new ArrayList(capacity: 0);
		private readonly WebClient webClient = new WebClient();
		private readonly SplashScreenForm formSplashScreen = new SplashScreenForm();
		private readonly string filenameMpcorb = Properties.Resources.FilenameMpcorb;
		private readonly string filenameMpcorbTemp = Properties.Resources.FilenameMpcorbTemp;
		private readonly Uri uriMpcorb = new Uri(uriString: Properties.Resources.MpcorbUrl);

		#region Local methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="control"></param>
		private static void SetDoubleBuffered(Control control)
		{
			if (SystemInformation.TerminalServerSession)
			{
				return;
			}
			PropertyInfo aProp = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			aProp.SetValue(obj: control, value: true, index: null);
		}

		/// <summary>
		/// 
		/// </summary>
		private void Restart()
		{
			Process.Start(fileName: Application.ExecutablePath);
			Close();
		}

		/// <summary>
		/// 
		/// </summary>
		private void AskForRestartAfterDownloadingDatabase()
		{
			if (MessageBox.Show(text: I10nStrings.DownloadCompleteAndRestartQuestionText, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information, defaultButton: MessageBoxDefaultButton.Button1) == DialogResult.Yes)
			{
				Restart();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currentPosition"></param>
		private void GotoCurrentPosition(int currentPosition)
		{
			//Achtung: Wenn später die Teilstrings in Zahlen konvertiert werden, dann muss darauf geachtet werden, dass die eingelesenen Zeichenketten keine Lerrstrings sind.
			// if (teilstring == "0") zahl = 0; ...
			labelIndexData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 0, length: 7).Trim();
			labelAbsoluteMagnitudeData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 8, length: 5).Trim();
			labelSlopeParameterData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 14, length: 5).Trim();
			labelEpochData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 20, length: 5).Trim();
			labelMeanAnomalyData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 26, length: 9).Trim();
			labelArgumentPerihelionData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 37, length: 9).Trim();
			labelLongitudeAscendingNodeData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 48, length: 9).Trim();
			labelInclinationData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 59, length: 9).Trim();
			labelOrbitalEccentricityData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 70, length: 9).Trim();
			labelMeanDailyMotionData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 80, length: 11).Trim();
			labelSemiMajorAxisData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 92, length: 11).Trim();
			labelReferenceData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 107, length: 9).Trim();
			labelNumberObservationsData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 117, length: 5).Trim();
			labelNumberOppositionsData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 123, length: 3).Trim();
			labelObservationSpanData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 127, length: 9).Trim();
			labelRmsResidualData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 137, length: 4).Trim();
			labelComputerNameData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 150, length: 10).Trim();
			labelFlagsData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 161, length: 4).Trim();
			labelDesignationNameData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 166, length: 28).Trim();
			labelDateLastObservationData.Text = planetoidDatabase[index: currentPosition].ToString().Substring(startIndex: 194, length: 8).Trim();
			toolStripLabelIndexPosition.Text = I10nStrings.Index + ": " + (currentPosition + 1).ToString() + " / " + planetoidDatabase.Count.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		private DateTime GetLastModified(Uri uri)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri: uri);
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			resp.Close();
			return resp.LastModified;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		private long GetContentLength(Uri uri)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUri: uri);
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			resp.Close();
			return Convert.ToInt64(value: resp.ContentLength);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool IsMpcorbDatUpdateAvailable()
		{
			FileInfo fileInfo = new FileInfo(fileName: filenameMpcorb);
			DateTime datetimeFileLocal = fileInfo.LastWriteTime;
			DateTime datetimeFileOnline = GetLastModified(uri: uriMpcorb);
			return datetimeFileOnline > datetimeFileLocal ? true : false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		private void CopyToClipboard(string text)
		{
			Clipboard.SetText(text: text);
			MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
		}

		/// <summary>
		/// 
		/// </summary>
		private void LoadRandomMinorPlanet() => GotoCurrentPosition(currentPosition: currentPosition = new Random().Next(maxValue: planetoidDatabase.Count + 1));

		/// <summary>
		/// 
		/// </summary>
		private void NavigateToTheBeginOfTheData() => GotoCurrentPosition(currentPosition: currentPosition = 0);

		/// <summary>
		/// 
		/// </summary>
		private void NavigateSomeDataBackward()
		{
			currentPosition -= stepPosition;
			if (currentPosition < 1)
			{
				currentPosition += planetoidDatabase.Count;
			}
			GotoCurrentPosition(currentPosition: currentPosition);
		}

		/// <summary>
		/// 
		/// </summary>
		private void NavigateToThePreviousData()
		{
			if (currentPosition == 0)
			{
				currentPosition = planetoidDatabase.Count - 1;
			}
			else
			{
				currentPosition--;
			}
			GotoCurrentPosition(currentPosition: currentPosition);
		}

		/// <summary>
		/// 
		/// </summary>
		private void NavigateToTheNextData()
		{
			if (currentPosition == planetoidDatabase.Count - 1)
			{
				currentPosition = 0;
			}
			else
			{
				currentPosition++;
			}
			GotoCurrentPosition(currentPosition: currentPosition);
		}

		/// <summary>
		/// 
		/// </summary>
		private void NavigateSomeDataForward()
		{
			currentPosition += stepPosition;
			if (currentPosition > planetoidDatabase.Count)
			{
				currentPosition -= planetoidDatabase.Count;
			}
			GotoCurrentPosition(currentPosition: currentPosition);
		}

		/// <summary>
		/// 
		/// </summary>
		private void NavigateToTheEndOfTheData() => GotoCurrentPosition(currentPosition: currentPosition = planetoidDatabase.Count - 1);

		/// <summary>
		/// 
		/// </summary>
		private void OpenTerminology(uint index)
		{
			using (TerminologyForm formTerminology = new TerminologyForm())
			{
				switch (index)
				{
					case 0: formTerminology.SetLabelIndexActive(sender: null, e: null); break;
					case 1: formTerminology.SetLabelDesgnNameActive(sender: null, e: null); break;
					case 2: formTerminology.SetLabelEpochActive(sender: null, e: null); break;
					case 3: formTerminology.SetLabelMeanAnomalyActive(sender: null, e: null); break;
					case 4: formTerminology.SetLabelArgPeriActive(sender: null, e: null); break;
					case 5: formTerminology.SetLabelLongAscNodeActive(sender: null, e: null); break;
					case 6: formTerminology.SetLabelInclActive(sender: null, e: null); break;
					case 7: formTerminology.SetLabelOrbEccActive(sender: null, e: null); break;
					case 8: formTerminology.SetLabelMotionActive(sender: null, e: null); break;
					case 9: formTerminology.SetLabelSemiMajorAxisActive(sender: null, e: null); break;
					case 10: formTerminology.SetLabelMagAbsActive(sender: null, e: null); break;
					case 11: formTerminology.SetLabelSlopeParamActive(sender: null, e: null); break;
					case 12: formTerminology.SetLabelRefActive(sender: null, e: null); break;
					case 13: formTerminology.SetLabelNumbOpposActive(sender: null, e: null); break;
					case 14: formTerminology.SetLabelNumbObsActive(sender: null, e: null); break;
					case 15: formTerminology.SetLabelObsSpanActive(sender: null, e: null); break;
					case 16: formTerminology.SetLabelRmsResidualActive(sender: null, e: null); break;
					case 17: formTerminology.SetLabelComputerNameActive(sender: null, e: null); break;
					case 18: formTerminology.SetLabelFlagsActive(sender: null, e: null); break;
					case 19: formTerminology.SetLabelObsLastDateActive(sender: null, e: null); break;
					default: break;
				}
				formTerminology.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void OpenTableMode()
		{
			using (TableModeForm formTableMode = new TableModeForm())
			{
				formTableMode.FillDatabase(arrTemp: planetoidDatabase);
				formTableMode.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void OpenSearch()
		{
			using (SearchForm formSearch = new SearchForm())
			{
				formSearch.FillDatabase(arrTemp: planetoidDatabase);
				formSearch.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ShowAppInfo()
		{
			using (AppInfoForm formAppInfo = new AppInfoForm())
			{
				formAppInfo.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ShowMpcorbDatCheck()
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
			else
			{
				using (CheckMpcorbDatForm formCeckMpcorbDat = new CheckMpcorbDatForm())
				{
					formCeckMpcorbDat.ShowDialog();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ShowDownloader()
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				MessageBox.Show(text: I10nStrings.NoInternetConnectionText, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
			else
			{
				using (DownloadUpdateForm formDownloaderForMpcorbDat = new DownloadUpdateForm())
				{
					if (formDownloaderForMpcorbDat.ShowDialog() == DialogResult.OK)
					{
						AskForRestartAfterDownloadingDatabase();
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ShowDatabaseInformation()
		{
			using (DatabaseInformationForm formDatabaseInformation = new DatabaseInformationForm())
			{
				formDatabaseInformation.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		private void SetStatusbar(string text)
		{
			labelInformation.Enabled = text == "" ? false : true;
			labelInformation.Text = text;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateSemiMinorAxis(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * Math.Sqrt(1 - Math.Pow(x: numericalEccentricity, y: 2));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateLinearEccentricity(double semiMajorAxis, double numericalEccentricity)
		{
			if (numericalEccentricity == 0)
			{
				return 0;
			}
			else if (numericalEccentricity < 1 && numericalEccentricity > 0)
			{
				return Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) - Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2));
			}
			else if (numericalEccentricity > 1)
			{
				return Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2));
			}
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <returns></returns>
		private double CalculateMajorAxis(double semiMajorAxis) => 2 * semiMajorAxis;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateMinorAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="meanAnomaly"></param>
		/// <param name="numericalEccentricity"></param>
		/// <param name="numberDecimalPlaces"></param>
		/// <returns></returns>
		private double CalculateEccentricAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
		{
			double K = Math.PI / 180.0;
			int maxIteration = 30, i = 0;
			double delta = Math.Pow(x: 10, y: -numberDecimalPlaces);
			double E, F;
			meanAnomaly /= 360.0;
			meanAnomaly = 2.0 * Math.PI * (meanAnomaly - Math.Floor(d: meanAnomaly));
			if (numericalEccentricity < 0.8)
			{
				E = meanAnomaly;
			}
			else
			{
				E = Math.PI;
			}
			F = E - numericalEccentricity * Math.Sin(a: meanAnomaly) - meanAnomaly;
			while ((Math.Abs(value: F) > delta) && (i < maxIteration))
			{
				E -= F / (1.0 - (numericalEccentricity * Math.Cos(d: E)));
				F = E - numericalEccentricity * Math.Sin(a: E) - meanAnomaly;
				i += 1;
			}
			E /= K;
			return Math.Round(a: E * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="meanAnomaly"></param>
		/// <param name="numericalEccentricity"></param>
		/// <param name="numberDecimalPlaces"></param>
		/// <returns></returns>
		private double CalculateTrueAnomaly(double meanAnomaly, double numericalEccentricity, double numberDecimalPlaces)
		{
			double E = CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: numberDecimalPlaces);
			double K = Math.PI / 180.0;
			double S = Math.Sin(a: E);
			double C = Math.Cos(d: E);
			double fak = Math.Sqrt(d: 1.0 - (numericalEccentricity * numericalEccentricity));
			double phi = Math.Atan2(y: fak * S, x: C - numericalEccentricity) / K;
			return Math.Round(a: phi * Math.Pow(x: 10, y: numberDecimalPlaces)) / Math.Pow(x: 10, y: numberDecimalPlaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculatePerihelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 - numericalEccentricity) * semiMajorAxis;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateAphelionDistance(double semiMajorAxis, double numericalEccentricity) => (1 + numericalEccentricity) * semiMajorAxis;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="longitudeAscendingNode"></param>
		/// <returns></returns>
		private double CalculateLongitudeDescendingNode(double longitudeAscendingNode)
		{
			if (longitudeAscendingNode >= 0 && longitudeAscendingNode < 180)
			{
				return longitudeAscendingNode + 180;
			}
			else if (longitudeAscendingNode >= 180 && longitudeAscendingNode < 360)
			{
				return longitudeAscendingNode - 180;
			}
			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="argumentAphelion"></param>
		/// <returns></returns>
		private double CalculateArgumenOfAphelion(double argumentAphelion)
		{
			if (argumentAphelion >= 0 && argumentAphelion < 180)
			{
				return argumentAphelion + 180;
			}
			else if (argumentAphelion >= 180 && argumentAphelion < 360)
			{
				return argumentAphelion - 180;
			}
			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateFocalParameter(double semiMajorAxis, double numericalEccentricity)
		{
			if (numericalEccentricity > 1)
			{
				return Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2) / Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2));
			}
			else if (numericalEccentricity > 0 && numericalEccentricity < 1)
			{
				return Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2) / Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) - Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity), y: 2));
			}
			return 2 * semiMajorAxis;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateSemiLatusRectum(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * (1 - Math.Pow(x: numericalEccentricity, y: 2));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateLatusRectum(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <returns></returns>
		private double CalculatePeriod(double semiMajorAxis) => Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 3));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateOrbitalArea(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) + ((3 * Math.Pow(x: semiMajorAxis - CalculateSemiMinorAxis(semiMajorAxis, numericalEccentricity), y: 2) / 10 * (semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity))) + Math.Sqrt(d: Math.Pow(x: semiMajorAxis, y: 2) + (14 * semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity)) + Math.Pow(x: CalculateSemiMinorAxis(semiMajorAxis, numericalEccentricity), y: 2)));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateOrbitalPerimeter(double semiMajorAxis, double numericalEccentricity) => semiMajorAxis * CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity) * Math.PI;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateSemiMeanAxis(double semiMajorAxis, double numericalEccentricity) => (semiMajorAxis + CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity)) / 2;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <param name="numericalEccentricity"></param>
		/// <returns></returns>
		private double CalculateMeanAxis(double semiMajorAxis, double numericalEccentricity) => 2 * CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="semiMajorAxis"></param>
		/// <returns></returns>
		private double CalculateStandardGravitationalParameter(double semiMajorAxis) => 4 * Math.Pow(x: Math.PI, y: 2) * Math.Pow(x: semiMajorAxis, y: 3) / CalculatePeriod(semiMajorAxis: semiMajorAxis);

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public PlanetoidDBForm()
		{
			InitializeComponent();
			base.Text = $"{base.Text} {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
			SetStatusbar(text: "");
		}

		#endregion

		#region Form* event handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlanetoidDBForm_Load(object sender, EventArgs e)
		{
			SetDoubleBuffered(control: tableLayoutPanelData);
			ToolStripManager.Renderer = new Office2007Renderer();
			backgroundWorkerLoadingDatabase.WorkerReportsProgress = true;
			backgroundWorkerLoadingDatabase.WorkerSupportsCancellation = true;
			backgroundWorkerLoadingDatabase.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorkerLoadingDatabase_ProgressChanged);
			backgroundWorkerLoadingDatabase.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorkerLoadingDatabase_RunWorkerCompleted);
			backgroundWorkerLoadingDatabase.RunWorkerAsync();
			formSplashScreen.Show();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlanetoidDBForm_Shown(object sender, EventArgs e)
		{
			toolStripStatusLabelBackgroundDownload.Enabled = false;
			toolStripProgressBarBackgroundDownload.Enabled = false;
			toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
			toolStripStatusLabelBackgroundDownload.Visible = false;
			toolStripProgressBarBackgroundDownload.Visible = false;
			toolStripStatusLabelCancelBackgroundDownload.Visible = false;
			if (IsMpcorbDatUpdateAvailable())
			{
				timerBlinkForUpdateAvailable.Enabled = true;
				toolStripStatusLabelUpdate.Enabled = true;
			}
			else
			{
				timerBlinkForUpdateAvailable.Enabled = false;
				toolStripStatusLabelUpdate.Enabled = false;
				toolStripStatusLabelUpdate.Visible = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlanetoidDBForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (File.Exists(path: filenameMpcorbTemp))
			{
				File.Delete(path: filenameMpcorbTemp);
			}
		}

		#endregion

		#region BackgroundWorker for database loading on start up

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorkerLoadingDatabase_DoWork(object sender, DoWorkEventArgs e)
		{
			Enabled = false;
			int lineNum = 0;
			float percent;
			string readLine;
			FileInfo fileInfo = new FileInfo(fileName: filenameMpcorb);
			long fileSize = fileInfo.Length, fileSizeReaded = 0;
			FileStream fileStream = new FileStream(path: filenameMpcorb, mode: FileMode.Open);
			StreamReader streamReader = new StreamReader(stream: fileStream);
			formSplashScreen.Show();
			while (streamReader.Peek() != -1 && !backgroundWorkerLoadingDatabase.CancellationPending)
			{
				readLine = streamReader.ReadLine();
				fileSizeReaded += readLine.Length;
				percent = 100 * fileSizeReaded / fileSize;
				formSplashScreen.SetProgressbar(value: (int)percent);
				lineNum++;
				if ((lineNum >= 44) && (readLine != ""))
				{
					planetoidDatabase.Add(value: readLine);
				}
			}
			fileStream.Close();
			streamReader.Close();
			formSplashScreen.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorkerLoadingDatabase_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackgroundWorkerLoadingDatabase_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			toolStripTextBoxGotoIndex.Text = 1.ToString();
			currentPosition = 0;
			stepPosition = 100;
			GotoCurrentPosition(currentPosition: currentPosition);
			Enabled = true;
		}

		#endregion

		#region Download and update database

		private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			toolStripProgressBarBackgroundDownload.Value = e.ProgressPercentage;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
		}

		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				File.Delete(path: filenameMpcorb);
				File.Copy(sourceFileName: filenameMpcorbTemp, destFileName: Properties.Resources.FilenameMpcorb);
				File.Delete(path: filenameMpcorbTemp);
				AskForRestartAfterDownloadingDatabase();
			}
			else
			{
				if (e.Cancelled)
				{
					MessageBox.Show(text: I10nStrings.DownloadCancelledText, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
				}
				else
				{
					MessageBox.Show(text: I10nStrings.DownloadUnknownError + "\n\r" + e.Error, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
				}
				File.Delete(path: filenameMpcorbTemp);
			}
			webClient.Dispose();
			toolStripStatusLabelBackgroundDownload.Enabled = false;
			toolStripProgressBarBackgroundDownload.Enabled = false;
			toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
			toolStripStatusLabelBackgroundDownload.Visible = false;
			toolStripProgressBarBackgroundDownload.Visible = false;
			toolStripStatusLabelCancelBackgroundDownload.Visible = false;
			toolStripStatusLabelUpdate.IsLink = false;
			toolStripStatusLabelUpdate.Enabled = false;
			toolStripStatusLabelUpdate.Visible = false;
			timerBlinkForUpdateAvailable.Enabled = false;
			toolStripProgressBarBackgroundDownload.Value = toolStripProgressBarBackgroundDownload.Minimum;
			TaskbarProgress.SetValue(windowHandle: Handle, progressValue: 0, progressMax: 100);
		}

		#endregion

		#region Timer

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerCheckForNewMpcorbDatFile_Tick(object sender, EventArgs e) => PlanetoidDBForm_Shown(sender: sender, e: e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimerBlinkForUpdateAvailable_Tick(object sender, EventArgs e)
		{
			if (toolStripStatusLabelUpdate.ForeColor == System.Drawing.SystemColors.HotTrack)
			{
				toolStripStatusLabelUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
			}
			else
			{
				toolStripStatusLabelUpdate.ForeColor = System.Drawing.SystemColors.HotTrack;
			}
		}

		#endregion

		#region Clear-Handler

		/// <summary>
		/// 
		/// </summary>
		private void ToolStripMenuItem_Clear()
		{
			toolStripMenuItem10.Checked = false;
			toolStripMenuItem100.Checked = false;
			toolStripMenuItem1000.Checked = false;
			toolStripMenuItem10000.Checked = false;
			toolStripMenuItem100000.Checked = false;
		}

		#endregion

		#region KeyPress-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripTextBoxGotoIndex_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(c: e.KeyChar) && !char.IsDigit(c: e.KeyChar))
			{
				e.Handled = true;
			}
			if (e.KeyChar == Convert.ToChar(value: Keys.Return))
			{
				ToolStripButtonGoToIndex_Click(sender: null, e: null);
			}
		}

		#endregion

		#region Enter-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SetStatusbar_Enter(object sender, EventArgs e)
		{
			if (sender is TextBox)
			{
				SetStatusbar(text: ((TextBox)sender).AccessibleDescription);
			}
			else if (sender is Button)
			{
				SetStatusbar(text: ((Button)sender).AccessibleDescription);
			}
			else if (sender is RadioButton)
			{
				SetStatusbar(text: ((RadioButton)sender).AccessibleDescription);
			}
			else if (sender is CheckBox)
			{
				SetStatusbar(text: ((CheckBox)sender).AccessibleDescription);
			}
			else if (sender is DateTimePicker)
			{
				SetStatusbar(text: ((DateTimePicker)sender).AccessibleDescription);
			}
			else if (sender is Label)
			{
				SetStatusbar(text: ((Label)sender).AccessibleDescription);
			}
			else if (sender is PictureBox)
			{
				SetStatusbar(text: ((PictureBox)sender).AccessibleDescription);
			}
			else if (sender is ToolStripButton)
			{
				SetStatusbar(text: ((ToolStripButton)sender).AccessibleDescription);
			}
			else if (sender is ToolStripMenuItem)
			{
				SetStatusbar(text: ((ToolStripMenuItem)sender).AccessibleDescription);
			}
			else if (sender is ToolStripLabel)
			{
				SetStatusbar(text: ((ToolStripLabel)sender).AccessibleDescription);
			}
			else if (sender is ToolStripComboBox)
			{
				SetStatusbar(text: ((ToolStripComboBox)sender).AccessibleDescription);
			}
			else if (sender is ToolStripDropDown)
			{
				SetStatusbar(text: ((ToolStripDropDown)sender).AccessibleDescription);
			}
			else if (sender is ToolStripDropDownButton)
			{
				SetStatusbar(text: ((ToolStripDropDownButton)sender).AccessibleDescription);
			}
			else if (sender is ToolStripDropDownItem)
			{
				SetStatusbar(text: ((ToolStripDropDownItem)sender).AccessibleDescription);
			}
			else if (sender is ToolStripDropDownMenu)
			{
				SetStatusbar(text: ((ToolStripDropDownMenu)sender).AccessibleDescription);
			}
			else if (sender is ToolStripProgressBar)
			{
				SetStatusbar(text: ((ToolStripProgressBar)sender).AccessibleDescription);
			}
			else if (sender is ToolStripSplitButton)
			{
				SetStatusbar(text: ((ToolStripSplitButton)sender).AccessibleDescription);
			}
			else if (sender is ToolStripSeparator)
			{
				SetStatusbar(text: ((ToolStripSeparator)sender).AccessibleDescription);
			}
			else if (sender is ToolStripStatusLabel)
			{
				SetStatusbar(text: ((ToolStripStatusLabel)sender).AccessibleDescription);
			}
			else if (sender is ToolStripTextBox)
			{
				SetStatusbar(text: ((ToolStripTextBox)sender).AccessibleDescription);
			}
		}

		#endregion

		#region MouseEnter-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripStatusLabelUpdate_MouseEnter(object sender, EventArgs e)
		{
			if (timerBlinkForUpdateAvailable.Enabled)
			{
				toolStripStatusLabelUpdate.IsLink = true;
			}
			SetStatusbar(text: toolStripStatusLabelUpdate.AccessibleDescription);
		}

		#endregion

		#region Leave-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearStatusbar_Leave(object sender, EventArgs e) => SetStatusbar(text: "");

		#endregion

		#region Click-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopyToClipboard_Click(object sender, EventArgs e)
		{
			if (sender is TextBox)
			{
				CopyToClipboard(text: ((TextBox)sender).Text);
			}
			else if (sender is Button)
			{
				CopyToClipboard(text: ((Button)sender).Text);
			}
			else if (sender is RadioButton)
			{
				CopyToClipboard(text: ((RadioButton)sender).Text);
			}
			else if (sender is CheckBox)
			{
				CopyToClipboard(text: ((CheckBox)sender).Text);
			}
			else if (sender is DateTimePicker)
			{
				CopyToClipboard(text: ((DateTimePicker)sender).Text);
			}
			else if (sender is Label)
			{
				CopyToClipboard(text: ((Label)sender).Text);
			}
			else if (sender is ToolStripButton)
			{
				CopyToClipboard(text: ((ToolStripButton)sender).Text);
			}
			else if (sender is ToolStripMenuItem)
			{
				CopyToClipboard(text: ((ToolStripMenuItem)sender).Text);
			}
			else if (sender is ToolStripLabel)
			{
				CopyToClipboard(text: ((ToolStripLabel)sender).Text);
			}
			else if (sender is ToolStripComboBox)
			{
				CopyToClipboard(text: ((ToolStripComboBox)sender).Text);
			}
			else if (sender is ToolStripDropDown)
			{
				CopyToClipboard(text: ((ToolStripDropDown)sender).Text);
			}
			else if (sender is ToolStripDropDownButton)
			{
				CopyToClipboard(text: ((ToolStripDropDownButton)sender).Text);
			}
			else if (sender is ToolStripDropDownItem)
			{
				CopyToClipboard(text: ((ToolStripDropDownItem)sender).Text);
			}
			else if (sender is ToolStripDropDownMenu)
			{
				CopyToClipboard(text: ((ToolStripDropDownMenu)sender).Text);
			}
			else if (sender is ToolStripProgressBar)
			{
				CopyToClipboard(text: ((ToolStripProgressBar)sender).Text);
			}
			else if (sender is ToolStripSplitButton)
			{
				CopyToClipboard(text: ((ToolStripSplitButton)sender).Text);
			}
			else if (sender is ToolStripSeparator)
			{
				CopyToClipboard(text: ((ToolStripSeparator)sender).Text);
			}
			else if (sender is ToolStripStatusLabel)
			{
				CopyToClipboard(text: ((ToolStripStatusLabel)sender).Text);
			}
			else if (sender is ToolStripTextBox)
			{
				CopyToClipboard(text: ((ToolStripTextBox)sender).Text);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepToBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepBackwardOne_Click(object sender, EventArgs e) => NavigateToThePreviousData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepForwardOne_Click(object sender, EventArgs e) => NavigateToTheNextData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonStepToEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonGoToIndex_Click(object sender, EventArgs e)
		{
			int pos = 0;
			try
			{
				pos = int.Parse(s: toolStripTextBoxGotoIndex.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(text: ex.Message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
			if (pos <= 0 || pos >= planetoidDatabase.Count + 1)
			{
				MessageBox.Show(text: I10nStrings.IndexOutOfRange, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
			}
			else
			{
				currentPosition = pos - 1;
				GotoCurrentPosition(currentPosition: currentPosition);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemTerminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonTerminology_Click(object sender, EventArgs e) => OpenTerminology(index: 0);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripStatusLabelCancelBackgroundDownload_Click(object sender, EventArgs e)
		{
			toolStripStatusLabelBackgroundDownload.Enabled = false;
			toolStripProgressBarBackgroundDownload.Enabled = false;
			toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
			toolStripStatusLabelBackgroundDownload.Visible = false;
			toolStripProgressBarBackgroundDownload.Visible = false;
			toolStripStatusLabelCancelBackgroundDownload.Visible = false;
			webClient.CancelAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem10_Click(object sender, EventArgs e)
		{
			stepPosition = 10;
			ToolStripMenuItem_Clear();
			toolStripMenuItem10.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem100_Click(object sender, EventArgs e)
		{
			stepPosition = 100;
			ToolStripMenuItem_Clear();
			toolStripMenuItem100.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem1000_Click(object sender, EventArgs e)
		{
			stepPosition = 1000;
			ToolStripMenuItem_Clear();
			toolStripMenuItem1000.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem10000_Click(object sender, EventArgs e)
		{
			stepPosition = 10000;
			ToolStripMenuItem_Clear();
			toolStripMenuItem10000.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItem100000_Click(object sender, EventArgs e)
		{
			stepPosition = 100000;
			ToolStripMenuItem_Clear();
			toolStripMenuItem100000.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemExit_Click(object sender, EventArgs e) => Close();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemAbout_Click(object sender, EventArgs e) => ShowAppInfo();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemOpenWebsitePDB_Click(object sender, EventArgs e) => Process.Start(fileName: Properties.Resources.Homepage);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemOpenWebsiteMPC_Click(object sender, EventArgs e) => Process.Start(fileName: Properties.Resources.WebsiteMpc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemOpenMPCORBWebsite_Click(object sender, EventArgs e) => Process.Start(fileName: Properties.Resources.WebsiteMpcorb);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemDownloadMpcorbDat_Click(object sender, EventArgs e) => ShowDownloader();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuitemCheckMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatCheck();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelIndex_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 0);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelDesgnName_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelEpoch_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 2);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelMeanAnomaly_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 3);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelArgPeri_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 4);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelLongAscNode_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 5);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelIncl_Enter(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 6);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelOrbEcc_Enter(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 7);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelMotion_Enter(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 8);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelSemiMajorAxis_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 9);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelMagAbs_Enter(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 10);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelSlopeParam_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 11);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelRef_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 12);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelNumbOppos_Enter(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 13);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelNumbObs_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 14);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelObsSpan_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 15);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelRmsResidual_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 16);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelComputerName_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 17);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelFlags_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 18);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelObsLastDate_Click(object sender, LinkLabelLinkClickedEventArgs e) => OpenTerminology(index: 19);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripStatusLabelUpdate_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(text: I10nStrings.AskForDownloadingLastestMpcorbDatFile, caption: I10nStrings.AskForDownloadingLastestMpcorbDatFileCaption, buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
			{
				toolStripStatusLabelUpdate.IsLink = false;
				toolStripStatusLabelUpdate.Enabled = false;
				toolStripStatusLabelUpdate.Visible = false;
				timerBlinkForUpdateAvailable.Enabled = false;
				toolStripStatusLabelBackgroundDownload.Visible = true;
				toolStripProgressBarBackgroundDownload.Visible = true;
				toolStripStatusLabelCancelBackgroundDownload.Visible = true;
				toolStripStatusLabelBackgroundDownload.Enabled = true;
				toolStripProgressBarBackgroundDownload.Enabled = true;
				toolStripStatusLabelCancelBackgroundDownload.Enabled = true;
				webClient.Proxy = null;
				webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
				webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
				try
				{
					webClient.DownloadFileAsync(address: uriMpcorb, fileName: Properties.Resources.FilenameMpcorbTemp);
				}
				catch (Exception ex)
				{
					MessageBox.Show(text: ex.Message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error, defaultButton: MessageBoxDefaultButton.Button1);
					toolStripStatusLabelUpdate.IsLink = true;
					toolStripStatusLabelUpdate.Enabled = true;
					toolStripStatusLabelUpdate.Visible = true;
					timerBlinkForUpdateAvailable.Enabled = true;
					toolStripStatusLabelBackgroundDownload.Visible = false;
					toolStripProgressBarBackgroundDownload.Visible = false;
					toolStripStatusLabelCancelBackgroundDownload.Visible = false;
					toolStripStatusLabelBackgroundDownload.Enabled = false;
					toolStripProgressBarBackgroundDownload.Enabled = false;
					toolStripStatusLabelCancelBackgroundDownload.Enabled = false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonCheckMpcorbDat_Click(object sender, EventArgs e) => ShowMpcorbDatCheck();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonDownloadMpcorbDat_Click(object sender, EventArgs e) => ShowDownloader();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonAbout_Click(object sender, EventArgs e) => ShowAppInfo();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonOpenWebsitePDB_Click(object sender, EventArgs e) => Process.Start(fileName: Properties.Resources.Homepage);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonTableMode_Click(object sender, EventArgs e) => OpenTableMode();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemTableMode_Click(object sender, EventArgs e) => OpenTableMode();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonDatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemStyleOffice2007_Click(object sender, EventArgs e)
		{
			ToolStripManager.Renderer = new Office2007Renderer();
			toolStripMenuItemStyleProfessional.Checked = false;
			toolStripMenuItemStyleOffice2007.Checked = true;
			toolStripMenuItemStyleSystem.Checked = false;
			toolStripMenuItemStyleVs2008.Checked = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemStyleProfessionell_Click(object sender, EventArgs e)
		{
			ToolStripManager.Renderer = new ToolStripProfessionalRenderer();
			toolStripMenuItemStyleProfessional.Checked = true;
			toolStripMenuItemStyleOffice2007.Checked = false;
			toolStripMenuItemStyleSystem.Checked = false;
			toolStripMenuItemStyleVs2008.Checked = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemSystem_Click(object sender, EventArgs e)
		{
			ToolStripManager.Renderer = new ToolStripSystemRenderer();
			toolStripMenuItemStyleProfessional.Checked = false;
			toolStripMenuItemStyleOffice2007.Checked = false;
			toolStripMenuItemStyleSystem.Checked = true;
			toolStripMenuItemStyleVs2008.Checked = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemVs2008_Click(object sender, EventArgs e)
		{
			ToolStripManager.Renderer = new VS2008ToolStripRenderer();
			toolStripMenuItemStyleProfessional.Checked = false;
			toolStripMenuItemStyleOffice2007.Checked = false;
			toolStripMenuItemStyleSystem.Checked = false;
			toolStripMenuItemStyleVs2008.Checked = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NotifyIconUpdate_Click(object sender, EventArgs e) => contextMenuNotifyIcon.Show();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonPrint_Click(object sender, EventArgs e)
		{
			//todo: add Print here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonCopyToClipboard_Click(object sender, EventArgs e)
		{
			//todo: add CopyToClipboard here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemPrint_Click(object sender, EventArgs e)
		{
			//todo: add Print here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonSearch_Click(object sender, EventArgs e) => OpenSearch();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemCopytoClipboard_Click(object sender, EventArgs e)
		{
			//todo: add CopyToClipboard here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemSearch_Click(object sender, EventArgs e) => OpenSearch();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemDatabaseInformation_Click(object sender, EventArgs e) => ShowDatabaseInformation();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonLoadRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemRandomMinorPlanet_Click(object sender, EventArgs e) => LoadRandomMinorPlanet();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateToTheBegin_Click(object sender, EventArgs e) => NavigateToTheBeginOfTheData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateSomeDataBackward_Click(object sender, EventArgs e) => NavigateSomeDataBackward();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateToThePreviousData_Click(object sender, EventArgs e) => NavigateToThePreviousData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateToTheNextData_Click(object sender, EventArgs e) => NavigateToTheNextData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateSomeDataForward_Click(object sender, EventArgs e) => NavigateSomeDataForward();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemNavigateToTheEnd_Click(object sender, EventArgs e) => NavigateToTheEndOfTheData();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemSettings_Click(object sender, EventArgs e)
		{
			//todo: add Settings here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemFilter_Click(object sender, EventArgs e)
		{
			//todo: add Filter here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripButtonDerivatedOrbitElements_Click(object sender, EventArgs e) => ToolStripMenuItemDerivatedOrbitElements_Click(sender: sender, e: e);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemRestart_Click(object sender, EventArgs e) => Restart();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuItemDerivatedOrbitElements_Click(object sender, EventArgs e)
		{
			derivatedOrbitElementsDatabase.Clear();
			IFormatProvider provider = CultureInfo.CreateSpecificCulture(name: "en");
			double semiMajorAxis = double.Parse(s: labelSemiMajorAxisData.Text, provider: provider);
			double numericalEccentricity = double.Parse(s: labelOrbitalEccentricityData.Text, provider: provider);
			double meanAnomaly = double.Parse(s: labelMeanAnomalyData.Text, provider: provider);
			double longitudeAscendingNode = double.Parse(s: labelLongitudeAscendingNodeData.Text, provider: provider);
			double argumentAphelion = double.Parse(s: labelArgumentPerihelionData.Text, provider: provider);
			derivatedOrbitElementsDatabase.Add(value: CalculateLinearEccentricity(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateSemiMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateMajorAxis(semiMajorAxis: semiMajorAxis));
			derivatedOrbitElementsDatabase.Add(value: CalculateMinorAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateEccentricAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8));
			derivatedOrbitElementsDatabase.Add(value: CalculateTrueAnomaly(meanAnomaly: meanAnomaly, numericalEccentricity: numericalEccentricity, numberDecimalPlaces: 8));
			derivatedOrbitElementsDatabase.Add(value: CalculatePerihelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateAphelionDistance(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateLongitudeDescendingNode(longitudeAscendingNode: longitudeAscendingNode));
			derivatedOrbitElementsDatabase.Add(value: CalculateArgumenOfAphelion(argumentAphelion: argumentAphelion));
			derivatedOrbitElementsDatabase.Add(value: CalculateFocalParameter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateSemiLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateLatusRectum(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculatePeriod(semiMajorAxis: semiMajorAxis));
			derivatedOrbitElementsDatabase.Add(value: CalculateOrbitalArea(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateOrbitalPerimeter(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateSemiMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateMeanAxis(semiMajorAxis: semiMajorAxis, numericalEccentricity: numericalEccentricity));
			derivatedOrbitElementsDatabase.Add(value: CalculateStandardGravitationalParameter(semiMajorAxis: semiMajorAxis));
			using (DerivatedOrbitElementsForm formDerivatedOrbitElements = new DerivatedOrbitElementsForm())
			{
				formDerivatedOrbitElements.SetDatabase(arrayList: derivatedOrbitElementsDatabase);
				formDerivatedOrbitElements.ShowDialog();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemLicense_Click(object sender, EventArgs e)
		{
			using (LicenseForm formLicense = new LicenseForm())
			{
				formLicense.ShowDialog();
			}
		}

		#endregion

		#region DoubleClick-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EasterEgg_DoubleClick(object sender, EventArgs e) => MessageBox.Show(text: I10nStrings.EasterEgg, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);

		#endregion
	}
}