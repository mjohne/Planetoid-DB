/*
 * File:        ThemeTestForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Provides a form for testing themes.
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

using NLog;

using System.Diagnostics;

namespace Planetoid_DB.Forms
{
	/// <summary>Provides a form for testing themes.</summary>
	// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
	[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
	public partial class ThemeTestForm : KryptonForm
	{
		/// <summary>NLog logger instance.</summary>
		/// <remarks>This logger is used throughout the application to log important events and errors.</remarks>
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();


		/// <summary>Initializes a new instance of the <see cref="ThemeTestForm"/> class.</summary>
		public ThemeTestForm()
		{
			// Log the initialization
			logger.Info(message: "Initializing ThemeTestForm...");
			// Initialize the form components
			InitializeComponent();
			// Log the initialization
			logger.Info(message: "ThemeTestForm initialized.");
		}

		/// <summary>Returns a short debugger display string for this instance.</summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		/// <remarks>This property is used to provide a custom display string for the debugger.</remarks>
		private string DebuggerDisplay => ToString() ?? string.Empty;
	}
}
