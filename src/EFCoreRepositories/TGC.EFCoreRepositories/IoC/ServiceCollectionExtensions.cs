using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TGC.EFCoreRepositories.Configuration;

namespace TGC.EFCoreRepositories.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddEFCoreRepositories(this IServiceCollection serviceCollection)
	{
		var applicationRootPath = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot")  // local_root
					?? (Environment.GetEnvironmentVariable("HOME") == null
						? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
						: $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot"); // azure_root

		IConfiguration config = new ConfigurationBuilder()
			.AddJsonFile(applicationRootPath + "/appsettings.json")
			.AddEnvironmentVariables()
			.Build();

		serviceCollection.AddOptions<EFCoreConfiguration>().Configure<IConfiguration>((settings, configuration) =>
		{
			configuration.GetSection(nameof(EFCoreConfiguration)).Bind(settings);
		});

		var settings = config.GetRequiredSection(nameof(EFCoreConfiguration)).Get<EFCoreConfiguration>();

		if (settings != null)
		{
			serviceCollection.AddDbContext<EFCoreContext>(
			options =>
			{
				options.UseSqlServer(settings.Connectionstring, b => b.MigrationsAssembly(settings.MigrationAssembly));
			});
		}

		return serviceCollection;
	}

	public static IServiceCollection AddRepository<T>(this IServiceCollection serviceCollection) where T : class, IEFCoreDTO, new()
	{
		serviceCollection.AddScoped<IEFCoreRepository<T>, EFCoreRepository<T>>();
		return serviceCollection;
	}

	internal static void RegisterAllEntities(this ModelBuilder modelBuilder, params Assembly[] assemblies)
	{
		var types = assemblies.SelectMany(a => a.GetTypes()).Where(t => Attribute.IsDefined(t, typeof(EFCoreDBSetAttribute)));

		foreach (var type in types)
		{
			modelBuilder.Entity(type).HasKey("Id");
		}
	}
}
