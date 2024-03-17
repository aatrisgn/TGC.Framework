using Azure.Data.Tables;
using Azure.Identity;
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
			TableServiceClient tableClient;

			if (storageConfiguration.UseManagedIdentity)
			{
				if (string.IsNullOrEmpty(storageConfiguration.StorageAccountUrl) == false)
				{
					tableClient = new TableServiceClient(new Uri(storageConfiguration.StorageAccountUrl), new DefaultAzureCredential());
				}
				else
				{
					throw new NullReferenceException($"{storageConfiguration.StorageAccountUrl} is not defined.");
				}
			}
			else
			{
				tableClient = new TableServiceClient(storageConfiguration.AccountConnectionString);
			}

			services.AddSingleton<ITableStorageContext>(new TableStorageContext(tableClient));
			services.AddScoped(typeof(IAzureTableStorageRepository<>), typeof(AzureTableStorageRepository<>));
		}

		return services;
	}

	private static IServiceCollection AddStubbedCoreServices(this IServiceCollection services)
	{
		services.AddSingleton<ITableStorageContext, StubbedTableStorageContext>();
		services.AddScoped(typeof(IAzureTableStorageRepository<>), typeof(StubbedAzureStableStorageRepository<>));

		return services;
	}
}
