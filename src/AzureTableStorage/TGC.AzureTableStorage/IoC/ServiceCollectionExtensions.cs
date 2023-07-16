using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Configuration;
using TGC.AzureTableStorage.Tests;
using TGC.Configuration;
using TGC.Configuration.IoC;

namespace TGC.AzureTableStorage.IoC;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// This is obsolote and should no longer be used. Used to create a table storage client by reading from appsettings.json.
	/// </summary>
	/// <param name="services">Service Collection for application</param>
	/// <returns>Updated service collection</returns>
	[Obsolete("This will be removed in future versions and is no longer supported.")]
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services)
	{
		services.AddAppSettingsAbstraction("appsettings.json");

		var settings = StaticAppSettings.AppSettings;

		if (settings != null)
		{
			AddCoreServices(services, settings.GetTyped<StorageConfiguration>());
		}

		return services;
	}

	/// <summary>
	/// Add services for using IAzureTableStorageRepositories with classes inheriting from AzureTableItem
	/// </summary>
	/// <param name="services">Service Collection for application</param>
	/// <param name="connectionString">Connectionstring for Azure Table Storage</param>
	/// <returns>Updated IServiceCollection</returns>
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, string connectionString)
	{
		var settings = new StorageConfigurationBuilder() { AccountConnectionString = connectionString };

		if (settings != null)
		{
			services.AddCoreServices(settings.ToConfiguration());
		}

		return services;
	}

	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, StorageConfigurationBuilder storageConfigurationBuilder)
	{
		services.AddCoreServices(storageConfigurationBuilder.ToConfiguration());

		return services;
	}

	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, Action<StorageConfigurationBuilder> storageConfigurationBuilderAction)
	{
		var storageConfiguration = new StorageConfigurationBuilder();
		storageConfigurationBuilderAction.Invoke(storageConfiguration);

		services.AddCoreServices(storageConfiguration.ToConfiguration());

		return services;
	}

	private static IServiceCollection AddCoreServices(this IServiceCollection services, IStorageConfiguration storageConfiguration)
	{
		services.AddSingleton<IStorageConfiguration>(storageConfiguration);

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
