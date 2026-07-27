// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

namespace Planetoid_DB.Helpers;

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
}