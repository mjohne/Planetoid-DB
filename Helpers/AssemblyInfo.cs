using System.IO;
using System.Reflection;

namespace Planetoid_DB.Helpers;

/// <summary>
/// Provide some assembly information
/// </summary>
/// <remarks>
/// This class provides access to assembly-level attributes.
/// </remarks>
public static class AssemblyInfo
{
	#region Assembly attribute accessors

	/// <summary>
	/// Return the title of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the title of the assembly.
	/// </remarks>
	public static string? AssemblyTitle
	{
		get
		{
			object[] attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(attributeType: typeof(AssemblyTitleAttribute), inherit: false);
			return attributes.Length > 0 && attributes[0] is AssemblyTitleAttribute titleAttribute
				? !string.IsNullOrEmpty(value: titleAttribute.Title) ? titleAttribute.Title : Path.GetFileNameWithoutExtension(path: AppContext.BaseDirectory)
				: Path.GetFileNameWithoutExtension(path: AppContext.BaseDirectory);
		}
	}

	/// <summary>
	/// Return the version of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the version of the assembly.
	/// </remarks>
	public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown Version";

	/// <summary>
	/// Return the description of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the description of the assembly.
	/// </remarks>
	public static string AssemblyDescription
	{
		get
		{
			object[] attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(attributeType: typeof(AssemblyDescriptionAttribute), inherit: false);
			return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
		}
	}

	/// <summary>
	/// Return the product name of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the product name of the assembly.
	/// </remarks>
	public static string AssemblyProduct
	{
		get
		{
			object[] attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(attributeType: typeof(AssemblyProductAttribute), inherit: false);
			return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute)attributes[0]).Product;
		}
	}

	/// <summary>
	/// Return the copyright of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the copyright of the assembly.
	/// </remarks>
	public static string AssemblyCopyright
	{
		get
		{
			object[] attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(attributeType: typeof(AssemblyCopyrightAttribute), inherit: false);
			return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
		}
	}

	/// <summary>
	/// Return the company name of the assembly
	/// </summary>
	/// <remarks>
	/// This property retrieves the company name of the assembly.
	/// </remarks>
	public static string AssemblyCompany
	{
		get
		{
			object[] attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(attributeType: typeof(AssemblyCompanyAttribute), inherit: false);
			return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
		}
	}
	#endregion
}