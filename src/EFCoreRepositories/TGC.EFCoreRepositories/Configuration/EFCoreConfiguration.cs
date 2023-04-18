namespace TGC.EFCoreRepositories.Configuration;

/// <summary>
/// Class used for binding configuration in appsettings.json to typed configuration
/// </summary>
public class EFCoreConfiguration
{
	/// <summary>
	/// Property which contains connectionstring for DBContext
	/// </summary>
	public string? Connectionstring { get; set; }
	/// <summary>
	/// Property which contains the definition of which assembly the EF migrations should be put in
	/// </summary>
	public string? MigrationAssembly { get; set; }
}
