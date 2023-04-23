using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TGC.Configuration;
using TGC.Configuration.IoC;
using TGC.EFCoreRepositories.Configuration;

namespace TGC.EFCoreRepositories.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddEFCoreRepositories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAppSettingsAbstraction("appsettings.json");

		var settings = StaticAppSettings.AppSettings;

		if (settings != null)
		{
			var typedSettings = settings.GetTyped<EFCoreConfiguration>();
			serviceCollection.AddSingleton<IEFCoreConfiguration>(typedSettings);
			serviceCollection.AddDbContext<EFCoreContext>(
			options =>
			{
				options.UseSqlServer(typedSettings.Connectionstring, b => b.MigrationsAssembly(typedSettings.MigrationAssembly));
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
