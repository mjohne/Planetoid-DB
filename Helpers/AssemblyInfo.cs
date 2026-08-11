/*
 * File:        AssemblyInfo.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Provide some assembly information.
 *
 * Autor:       Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using System.Reflection;

namespace Planetoid_DB;

/// <summary>Provide some assembly information</summary>
/// <remarks>This class provides access to assembly-level attributes.</remarks>
public static class AssemblyInfo
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the current assembly.</summary>
	/// <remarks>This field holds a reference to the assembly that contains this code.</remarks>
	private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

	/// <summary>Lazy-loaded assembly title.</summary>
	/// <remarks>This field retrieves the assembly title when first accessed.</remarks>
	private static readonly Lazy<string> assemblyTitle = new(valueFactory: GetAssemblyTitle);

	/// <summary>Lazy-loaded assembly version.</summary>
	/// <remarks>This field retrieves the assembly version when first accessed.</remarks>
	private static readonly Lazy<string> assemblyVersion = new(valueFactory: () => CurrentAssembly.GetName().Version?.ToString() ?? string.Empty);

	/// <summary>Lazy-loaded assembly description.</summary>
	/// <remarks>This field retrieves the assembly description when first accessed.</remarks>
	private static readonly Lazy<string> assemblyDescription = new(valueFactory: () => GetAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty);

	/// <summary>Lazy-loaded assembly product.</summary>
	/// <remarks>This field retrieves the assembly product when first accessed.</remarks>
	private static readonly Lazy<string> assemblyProduct = new(valueFactory: () => GetAttribute<AssemblyProductAttribute>()?.Product ?? string.Empty);

	/// <summary>Lazy-loaded assembly copyright.</summary>
	/// <remarks>This field retrieves the assembly copyright when first accessed.</remarks>
	private static readonly Lazy<string> assemblyCopyright = new(valueFactory: () => GetAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? string.Empty);

	/// <summary>Lazy-loaded assembly company.</summary>
	/// <remarks>This field retrieves the assembly company when first accessed.</remarks>
	private static readonly Lazy<string> assemblyCompany = new(valueFactory: () => GetAttribute<AssemblyCompanyAttribute>()?.Company ?? string.Empty);

	#region Helpers

	/// <summary>Helper method to safely retrieve a specific assembly attribute.</summary>
	// <remarks>This method retrieves a specific assembly attribute of type <typeparamref name="T"/>. If the attribute is not found, it returns null. If an error occurs, it logs the error and returns null.</remarks>
	private static T? GetAttribute<T>() where T : Attribute
	{
		// Attempt to retrieve the specified assembly attribute
		try
		{
			// Use reflection to get the custom attribute of type T from the current assembly
			foreach (T attribute in CurrentAssembly.GetCustomAttributes<T>())
			{
				return attribute;
			}
			return null;
		}
		// Catch any exceptions that may occur during the retrieval process
		catch (Exception ex)
		{
			// Log the error with the logger, including the type of attribute that caused the error
			logger.Error(exception: ex, message: $"An error occurred while retrieving the assembly attribute: {typeof(T).Name}");
			return null;
		}
	}

	/// <summary>Specific helper for the title, as it has a fallback logic.</summary>
	/// <remarks>This method retrieves the assembly title. If the title attribute is not present or is empty, it falls back to using the assembly's file name without the extension. Any errors during this process are logged.</remarks>
	private static string GetAssemblyTitle()
	{
		// Attempt to retrieve the AssemblyTitleAttribute
		AssemblyTitleAttribute? titleAttribute = GetAttribute<AssemblyTitleAttribute>();
		// If the title attribute is present and not empty, return its value
		if (titleAttribute != null && !string.IsNullOrWhiteSpace(value: titleAttribute.Title))
		{
			return titleAttribute.Title;
		}
		// If the title attribute is not present or is empty, fall back to using the assembly's file name without the extension
		try
		{
			return Path.GetFileNameWithoutExtension(path: CurrentAssembly.Location) ?? string.Empty;
		}
		// Catch any exceptions that may occur while accessing the assembly's location
		catch (Exception ex)
		{
			// Log the error with the logger, indicating that there was an issue reading the assembly path for the title fallback
			logger.Error(exception: ex, message: "An error occurred while reading the assembly path for the title fallback.");
			// Return an empty string as a fallback if an error occurs
			return string.Empty;
		}
	}

	#endregion

	#region Assembly attribute accessors

	/// <summary>Return the title of the assembly</summary>
	/// <remarks>This property retrieves the title of the assembly.</remarks>
	public static string AssemblyTitle => assemblyTitle.Value;

	/// <summary>Return the version of the assembly</summary>
	/// <remarks>This property retrieves the version of the assembly.</remarks>
	public static string AssemblyVersion => assemblyVersion.Value;

	/// <summary>Return the description of the assembly</summary>
	/// <remarks>This property retrieves the description of the assembly.</remarks>
	public static string AssemblyDescription => assemblyDescription.Value;

	/// <summary>Return the product name of the assembly</summary>
	/// <remarks>This property retrieves the product name of the assembly.</remarks>
	public static string AssemblyProduct => assemblyProduct.Value;

	/// <summary>Return the copyright of the assembly</summary>
	/// <remarks>This property retrieves the copyright of the assembly.</remarks>
	public static string AssemblyCopyright => assemblyCopyright.Value;

	/// <summary>Return the company name of the assembly</summary>
	/// <remarks>This property retrieves the company name of the assembly.</remarks>
	public static string AssemblyCompany => assemblyCompany.Value;

	#endregion
}
