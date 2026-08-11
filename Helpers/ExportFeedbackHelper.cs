/*
 * File:        ExportFeedbackHelper.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Handles UI feedback and logging during export operations.
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

namespace Planetoid_DB;

/// <summary>Handles UI feedback and logging during export operations.</summary>
/// <remarks>This static class provides methods for displaying success and error messages to the user, as well as logging errors that occur during file export operations.</remarks>
public static class ExportFeedbackHelper
{
	/// <summary>NLog logger for logging export-related messages and errors.</summary>
	/// <remarks>This logger captures error and info messages during export operations.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Displays an error message box to the user.</summary>
	/// <param name="message">The error message to display.</param>
	/// <remarks>This method is used by exporter classes to display error messages to the user when an export operation fails.</remarks>
	public static void ShowErrorMessage(string message)
	{
		_ = KryptonMessageBox.Show(
			text: message,
			caption: I18nStrings.ErrorCaption,
			buttons: KryptonMessageBoxButtons.OK,
			icon: KryptonMessageBoxIcon.Error);
	}

	/// <summary>Shows a success message box after a file has been saved successfully.</summary>
	/// <remarks>Displays a message box to the user confirming the file was saved successfully.</remarks>
	public static void ShowSuccess()
	{
		_ = KryptonMessageBox.Show(
			text: I18nStrings.FileSavedSuccessfully,
			caption: I18nStrings.InformationCaption,
			buttons: KryptonMessageBoxButtons.OK,
			icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Logs and shows an error that occurred while saving a file.</summary>
	/// <param name="ex">The exception that occurred.</param>
	/// <param name="format">A label identifying the file format (e.g. "Text", "LaTeX").</param>
	/// <param name="filePath">The target file path.</param>
	/// <remarks>Logs the error with details about the format and file path, and displays an error message box to the user.</remarks>
	public static void ShowError(Exception ex, string format, string filePath)
	{
		logger.Error(exception: ex, message: $"Error saving as {format} to '{filePath}'.");
		ShowErrorMessage(message: $"Error saving as {format}: {ex.Message}");
	}

	/// <summary>Shows a success message box after settings have been imported successfully.</summary>
	/// <param name="count">The number of settings that were applied.</param>
	/// <remarks>Displays a message box to the user confirming how many settings were imported.</remarks>
	public static void ShowImportSuccess(int count)
	{
		_ = KryptonMessageBox.Show(
			text: $"{count} setting(s) imported successfully.",
			caption: I18nStrings.InformationCaption,
			buttons: KryptonMessageBoxButtons.OK,
			icon: KryptonMessageBoxIcon.Information);
	}

	/// <summary>Logs and shows an error that occurred while importing a settings file.</summary>
	/// <param name="ex">The exception that occurred.</param>
	/// <param name="format">A label identifying the file format (e.g. "CSV", "JSON").</param>
	/// <param name="filePath">The source file path.</param>
	/// <remarks>Logs the error with details about the format and file path, and displays an error message box to the user.</remarks>
	public static void ShowImportError(Exception ex, string format, string filePath)
	{
		logger.Error(exception: ex, message: $"Error importing from {format} file '{filePath}'.");
		ShowErrorMessage(message: $"Error importing from {format}: {ex.Message}");
	}
}