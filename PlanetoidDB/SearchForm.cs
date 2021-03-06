﻿using System;
using System.Collections;
using System.Windows.Forms;

namespace Planetoid_DB
{
	public partial class SearchForm : Form
	{
		private ArrayList planetoidDatabase = new ArrayList(capacity: 0);
		private int numberPlanetoids = 0;
		private bool isCancelled = false;
		private string strIndex, strMagAbs, strSlopeParam, strEpoch, strMeanAnomaly, strArgPeri, strLongAscNode, strIncl, strOrbEcc, strMotion, strSemiMajorAxis, strRef, strNumbObs, strNumbOppos, strObsSpan, strRmsResdiual, strComputerName, strFlags, strDesgnName, strObsLastDate;

		#region local methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arrTemp"></param>
		public void FillDatabase(ArrayList arrTemp)
		{
			planetoidDatabase = arrTemp;
			numberPlanetoids = planetoidDatabase.Count;
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

		#endregion

		#region Form* event handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchForm_Load(object sender, EventArgs e)
		{
			buttonUnselectAllItems.Enabled = false;
			buttonCancelSearch.Enabled = false;
			buttonOpenSelectedObject.Enabled = false;
			listViewResults.Enabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			listViewResults.Dispose();
			Dispose();
		}

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public SearchForm()
		{
			InitializeComponent();
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

		#region Leave-Handler

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearStatusbar_Leave(object sender, EventArgs e) => SetStatusbar(text: "");

		#endregion

		#region Click event handlers

		private void ButtonFindSearchterm_Click(object sender, EventArgs e)
		{
			buttonFindSearchterm.Enabled = !buttonFindSearchterm.Enabled;
			buttonCancelSearch.Enabled = !buttonCancelSearch.Enabled;
			buttonSetDefaultSettings.Enabled = !buttonSetDefaultSettings.Enabled;
			groupBoxSearchIn.Enabled = !groupBoxSearchIn.Enabled;
			groupBoxSearchOptions.Enabled = !groupBoxSearchOptions.Enabled;
			groupBoxRange.Enabled = !groupBoxRange.Enabled;
			listViewResults.Enabled = !listViewResults.Enabled;
		}

		private void ButtonCancelSearch_Click(object sender, EventArgs e)
		{
			buttonFindSearchterm.Enabled = !buttonFindSearchterm.Enabled;
			buttonCancelSearch.Enabled = !buttonCancelSearch.Enabled;
			buttonSetDefaultSettings.Enabled = !buttonSetDefaultSettings.Enabled;
			groupBoxSearchIn.Enabled = !groupBoxSearchIn.Enabled;
			groupBoxSearchOptions.Enabled = !groupBoxSearchOptions.Enabled;
			groupBoxRange.Enabled = !groupBoxRange.Enabled;
		}

		private void ButtonSelectAllItems_Click(object sender, EventArgs e)
		{
			buttonSelectAllItems.Enabled = !buttonSelectAllItems.Enabled;
			buttonUnselectAllItems.Enabled = !buttonUnselectAllItems.Enabled;
		}

		private void ButtonUnselectAllItems_Click(object sender, EventArgs e)
		{
			buttonSelectAllItems.Enabled = !buttonSelectAllItems.Enabled;
			buttonUnselectAllItems.Enabled = !buttonUnselectAllItems.Enabled;
		}

		private void ButtonSetDefaultSettings_Click(object sender, EventArgs e)
		{

		}

		private void ButtonOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

		private void ButtonOpenSelectedObject_Click(object sender, EventArgs e)
		{

		}

		#endregion

	}
}
