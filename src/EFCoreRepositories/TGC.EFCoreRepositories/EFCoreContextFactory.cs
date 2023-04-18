using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TGC.EFCoreRepositories.Configuration;

namespace TGC.EFCoreRepositories;

public class EFCoreContextFactory : IDesignTimeDbContextFactory<EFCoreContext>
{
	public EFCoreContext CreateDbContext(string[] args)
	{
		var filePath = args[0];

		IConfiguration config = new ConfigurationBuilder()
			.AddJsonFile(filePath)
			.AddEnvironmentVariables()
			.Build();

		var settings = config.GetRequiredSection(nameof(EFCoreConfiguration)).Get<EFCoreConfiguration>();

		if (settings != null)
		{
			var optionsBuilder = new DbContextOptionsBuilder<EFCoreContext>();
			optionsBuilder.UseSqlServer(settings.Connectionstring, b => b.MigrationsAssembly(settings.MigrationAssembly));
			return new EFCoreContext(optionsBuilder.Options);
		}

		throw new NullReferenceException("No configuration file for EFCoreRepositories was found.");
	}
}
