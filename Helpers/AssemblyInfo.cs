// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using System.Reflection;

namespace Planetoid_DB.Helpers;

/// <summary>Provide some assembly information</summary>
/// <remarks>This class provides access to assembly-level attributes.</remarks>
public static class AssemblyInfo
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	
	#region Assembly attribute accessors

	/// <summary>Return the title of the assembly</summary>
	/// <remarks>This property retrieves the title of the assembly.</remarks>
	public static string? AssemblyTitle
	{
		get
		{
			try
			{
				object[] attributes = Assembly.GetExecutingAssembly()
					.GetCustomAttributes(attributeType: typeof(AssemblyTitleAttribute), inherit: false);
				return attributes.Length > 0 && attributes[0] is AssemblyTitleAttribute titleAttribute
					? !string.IsNullOrEmpty(value: titleAttribute.Title) ? titleAttribute.Title : Path.GetFileNameWithoutExtension(path: Assembly.GetExecutingAssembly().Location)
					: Path.GetFileNameWithoutExtension(path: Assembly.GetExecutingAssembly().Location);
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly title.");
				return string.Empty;
			}
		}
	}

	/// <summary>Return the version of the assembly</summary>
	/// <remarks>This property retrieves the version of the assembly.</remarks>
	public static string AssemblyVersion
	{
		get
		{
			try
			{
				return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly version.");
				return string.Empty;
			}
		}
	}

	/// <summary>Return the description of the assembly</summary>
	/// <remarks>This property retrieves the description of the assembly.</remarks>
	public static string AssemblyDescription
	{
		get
		{
			try
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(attributeType: typeof(AssemblyDescriptionAttribute), inherit: false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly description.");
				return string.Empty;
			}
		}
	}

	/// <summary>Return the product name of the assembly</summary>
	/// <remarks>This property retrieves the product name of the assembly.</remarks>
	public static string AssemblyProduct
	{
		get
		{
			try
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(attributeType: typeof(AssemblyProductAttribute), inherit: false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute)attributes[0]).Product;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly product.");
				return string.Empty;
			}
		}
	}

	/// <summary>Return the copyright of the assembly</summary>
	/// <remarks>This property retrieves the copyright of the assembly.</remarks>
	public static string AssemblyCopyright
	{
		get
		{
			try
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(attributeType: typeof(AssemblyCopyrightAttribute), inherit: false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly copyright.");
				return string.Empty;
			}
		}
	}

	/// <summary>Return the company name of the assembly</summary>
	/// <remarks>This property retrieves the company name of the assembly.</remarks>
	public static string AssemblyCompany
	{
		get
		{
			try
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(attributeType: typeof(AssemblyCompanyAttribute), inherit: false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
			catch (Exception ex)
			{
				Logger.Error(exception: ex, message: "An error occurred while retrieving the assembly company.");
				return string.Empty;
			}
		}
	}
	#endregion
}
