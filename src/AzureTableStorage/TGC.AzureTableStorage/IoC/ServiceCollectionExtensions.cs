using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Configuration;
using TGC.Configuration;
using TGC.Configuration.IoC;

namespace TGC.AzureTableStorage.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services)
	{
		services.AddAppSettingsAbstraction("appsettings.json");

		var settings = StaticAppSettings.AppSettings;

		if (settings != null)
		{
			services.AddSingleton<IStorageConfiguration>(settings.GetTyped<StorageConfiguration>());
			services.AddSingleton<ITableStorageContext, TableStorageContext>();
		}

		return services;
	}

	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, string connectionString)
	{
		var settings = new StorageConfiguration() { AccountConnectionString = connectionString };

		if (settings != null)
		{
			services.AddSingleton<IStorageConfiguration>(settings);
			services.AddSingleton<ITableStorageContext, TableStorageContext>();
		}

		return services;
	}

	public static IServiceCollection AddStorageRepository<T>(this IServiceCollection services) where T : class, ITableEntity, new()
	{
		services.AddScoped<IAzureTableStorageRepository<T>, AzureTableStorageRepository<T>>();
		return services;
	}
}
