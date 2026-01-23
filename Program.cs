using NLog;

using Planetoid_DB.Properties;

using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Planetoid_DB
{
	internal static class Program
	{
		/// <summary>
		/// NLog logger instance for logging application events.
		/// </summary>
		/// <remarks>
		/// This logger is used throughout the application to log important events and errors.
		/// </remarks>
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Feature ID for disabling navigation sounds.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to disable navigation sounds in the application.
		/// </remarks>
		private const int FeatureDisableNavigationSounds = 21;

		/// <summary>
		/// Feature ID for setting the feature on the current thread.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread.
		/// </remarks>
		private const int SetFeatureOnThread = 0x00000001;

		/// <summary>
		/// Feature ID for setting the feature on the current process.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current process.
		/// </remarks>
		private const int SetFeatureOnProcess = 0x00000002;

		/// <summary>
		/// Feature ID for setting the feature in the registry.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature in the registry.
		/// </remarks>
		private const int SetFeatureInRegistry = 0x00000004;

		/// <summary>
		/// Feature ID for setting the feature on the current thread for local machine.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread for local machine.
		/// </remarks>
		private const int SetFeatureOnThreadLocalMachine = 0x00000008;

		/// <summary>
		/// Feature ID for setting the feature on the current thread for intranet.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread for intranet.
		/// </remarks>
		private const int SetFeatureOnThreadIntranet = 0x00000010;

		/// <summary>
		/// Feature ID for setting the feature on the current thread for trusted sites.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread for trusted sites.
		/// </remarks>
		private const int SetFeatureOnThreadTrusted = 0x00000020;

		/// <summary>
		/// Feature ID for setting the feature on the current thread for internet.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread for internet.
		/// </remarks>
		private const int SetFeatureOnThreadInternet = 0x00000040;

		/// <summary>
		/// Feature ID for setting the feature on the current thread for restricted sites.
		/// </summary>
		/// <remarks>
		/// This feature ID is used to set the feature on the current thread for restricted sites.
		/// </remarks>
		private const int SetFeatureOnThreadRestricted = 0x00000080;

		/// <summary>
		/// Disables navigation sounds.
		/// </summary>
		/// <param name="featureEntry">The feature ID.</param>
		/// <param name="dwFlags">The flags.</param>
		/// <param name="fEnable">Specifies whether the feature should be enabled or disabled.</param>
		/// <returns>An HRESULT value indicating success or failure.</returns>
		/// <remarks>
		/// This method sets the specified feature for the current process.
		/// </remarks>
		[DllImport(dllName: "urlmon.dll")]
		[PreserveSig]
		[return: MarshalAs(unmanagedType: UnmanagedType.Error)]
		private static extern int CoInternetSetFeatureEnabled(int featureEntry, [MarshalAs(unmanagedType: UnmanagedType.U4)] int dwFlags, bool fEnable);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <remarks>
		/// This method is responsible for starting the application and initializing the main form.
		/// </remarks>
		[STAThread]
		private static void Main()
		{
			// Try to set the application to use the default font settings
			try
			{
				// Disable navigation sounds
				DisableNavigationSounds();
				// Initialize the application configuration
				ApplicationConfiguration.Initialize();

				if (!File.Exists(path: Settings.Default.systemFilenameMpcorb))
				{
					// Show the PreLoadForm if the file is missing
					HandleMissingFile();
				}
				else
				{
					// Start the main form
					Application.Run(mainForm: new PlanetoidDbForm());
				}
			}
			// Catch specific exceptions and handle them accordingly
			catch (UnauthorizedAccessException ex)
			{
				// Log the error and show a message box
				Logger.Error(exception: ex, message: "Access denied");
				ShowErrorMessage(message: $"Access denied: {ex.Message}");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			catch (FileNotFoundException ex)
			{
				// Log the error and show a message box
				Logger.Error(exception: ex, message: "File not found");
				ShowErrorMessage(message: $"File not found: {ex.Message}");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			catch (IOException ex)
			{
				// Log the error and show a message box
				Logger.Error(exception: ex, message: "I/O error");
				ShowErrorMessage(message: $"I/O error: {ex.Message}");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			catch (NetworkInformationException ex)
			{
				// Log the error and show a message box
				Logger.Error(exception: ex, message: "network error");
				ShowErrorMessage(message: $"network error: {ex.Message}");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			catch (Exception ex)
			{
				// Log the error and show a message box
				Logger.Error(exception: ex, message: "An unexpected error occurred.");
				ShowErrorMessage(message: $"An unexpected error occurred: {ex.Message}");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			finally
			{
				// Ensure that the logger is properly shut down
				LogManager.Shutdown();
			}
		}

		/// <summary>
		/// Disables the navigation sounds.
		/// </summary>
		/// <remarks>
		/// This method disables the navigation sounds for the current process.
		/// </remarks>
		private static void DisableNavigationSounds() =>
			// Disable navigation sounds for the current process
			_ = CoInternetSetFeatureEnabled(featureEntry: FeatureDisableNavigationSounds, dwFlags: SetFeatureOnProcess, fEnable: true);

		/// <summary>
		/// Handles the case when the file is missing.
		/// </summary>
		/// <remarks>
		/// This method handles the case when the file is missing.
		/// </remarks>
		private static void HandleMissingFile()
		{
			// Create an instance of the PreLoadForm
			using PreloadForm formPreload = new();
			// Show the PreLoadForm
			_ = formPreload.ShowDialog();
			// Check if the form is exited with a cancel result
			if (formPreload.DialogResult == DialogResult.Cancel)
			{
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			// Check if the file path is empty
			if (string.IsNullOrEmpty(value: formPreload.MpcOrbDatFilePath))
			{
				// Show an error message if the file path is empty
				Logger.Error(message: "File not found");
				ShowErrorMessage(message: "File not found");
				// Exit the application with a non-zero exit code
				Environment.Exit(exitCode: Environment.ExitCode);
			}
			else
			{
				// Start the main form with the specified file path
				Application.Run(mainForm: new PlanetoidDbForm(mpcorbDatFilePath: formPreload.MpcOrbDatFilePath));
			}
		}

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <remarks>
		/// This method displays an error message to the user.
		/// </remarks>
		private static void ShowErrorMessage(string message) =>
			// Log the error message
			_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
	}
}
