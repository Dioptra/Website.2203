using System.Reflection;

namespace Website.Lib;


/// <summary>
/// Supplies information about the assembly/package. 
/// </summary>
public class PackageInformation
{
    /// <summary>
    /// Returns a string with the value of the InformationalVersion
    /// </summary>
    /// <returns></returns>
    public static string Version => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "";
    
    
    /// <summary>
    /// Returns a string with the value of the InformationalVersion
    /// </summary>
    /// <returns></returns>
    public static string Blub => Assembly.GetExecutingAssembly().GetCustomAttribute()?.InformationalVersion ?? "";
}
