using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Configuration;
using TGC.AzureTableStorage.Tests;

namespace TGC.AzureTableStorage.IoC;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add services for using IAzureTableStorageRepositories with classes inheriting from AzureTableItem
	/// </summary>
	/// <param name="services">Service Collection for application</param>
	/// <param name="connectionString">Connectionstring for Azure Table Storage</param>
	/// <returns>Updated IServiceCollection</returns>
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, string connectionString)
	{
		ArgumentNullException.ThrowIfNull(connectionString);

		var settings = new StorageConfiguration() { AccountConnectionString = connectionString };

		services.AddCoreServices(settings);

		return services;
	}

	/// <summary>
	/// Add services for using IAzureTableStorageRepositories with classes inheriting from AzureTableItem
	/// </summary>
	/// <param name="services">Service Collection for application</param>
	/// <param name="storageConfigurationAction">Configuration options for Azure Table Storage</param>
	/// <returns>Updated IServiceCollection</returns>
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, Action<StorageConfiguration> storageConfigurationAction)
	{
		ArgumentNullException.ThrowIfNull(storageConfigurationAction);

		var storageConfiguration = new StorageConfiguration();

		storageConfigurationAction.Invoke(storageConfiguration);

		services.AddCoreServices(storageConfiguration);

		return services;
	}

	private static IServiceCollection AddCoreServices(this IServiceCollection services, IStorageConfiguration storageConfiguration)
	{
		services.AddSingleton(storageConfiguration);

		if (storageConfiguration.StubServices)
		{
			services.AddStubbedCoreServices();
		}
		else
		{
			services.AddSingleton<ITableClientFactory>(new TableClientFactory(storageConfiguration));
			services.AddScoped(typeof(IAzureTableStorageRepository<>), typeof(AzureTableStorageRepository<>));
			services.AddSingleton(typeof(IAzureTableReadLock<>), typeof(AzureTableStorageRepository<>));
			services.AddSingleton(typeof(IAzureTableWriteLock<>), typeof(AzureTableStorageRepository<>));
		}

		return services;
	}

	private static IServiceCollection AddStubbedCoreServices(this IServiceCollection services)
	{
		services.AddScoped(typeof(IAzureTableStorageRepository<>), typeof(StubbedAzureStableStorageRepository<>));

		return services;
	}
}
