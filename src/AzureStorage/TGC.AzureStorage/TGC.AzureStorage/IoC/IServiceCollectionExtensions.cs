using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using TGC.Configuration;
using TGC.Configuration.IoC;

namespace TGC.AzureStorage.IoC;

public static class IServiceCollectionExtensions
{
	/// <summary>
	/// Configure access to blob storage via connectionstring in appsettings.
	/// </summary>
	/// <param name="serviceDescriptors"></param>
	public static void ConfigureAzureStorageConnectionString(this IServiceCollection serviceDescriptors)
	{
		serviceDescriptors.AddAppSettingsAbstraction();

		if (StaticAppSettings.AppSettings != null)
		{
			var azureStorageConfiguration = StaticAppSettings.AppSettings.GetTyped<AzureStorageConfiguration>("TGC.AzureStorage");

			if (azureStorageConfiguration.StorageConnectionString != null)
			{
				var azureStorageContainerRegistry = new AzureStorageContainerRegistry(azureStorageConfiguration.StorageConnectionString);
				serviceDescriptors.AddSingleton<IAzureStorageContainerRegistry>(azureStorageContainerRegistry);
				return;
			}
		}
		throw new FileNotFoundException("Could not locate an appsettings.json file to read configuration from");
	}

	/// <summary>
	/// Configures access to blob storage through Azure DefaultCredentials. Only use if managed identity has access to blob storage.
	/// </summary>
	/// <param name="serviceDescriptors"></param>
	public static void ConfigureAzureStorageDefaultCredentials(this IServiceCollection serviceDescriptors)
	{
		serviceDescriptors.AddAppSettingsAbstraction();

		if (StaticAppSettings.AppSettings != null)
		{
			var azureStorageConfiguration = StaticAppSettings.AppSettings.GetTyped<AzureStorageConfiguration>("TGC.AzureStorage");

			if (azureStorageConfiguration.StorageUri != null)
			{
				var azureStorageContainerRegistry = new AzureStorageContainerRegistry(azureStorageConfiguration.StorageUri, new DefaultAzureCredential());
				serviceDescriptors.AddSingleton<IAzureStorageContainerRegistry>(azureStorageContainerRegistry);
				return;
			}
		}
		throw new FileNotFoundException("Could not locate an appsettings.json file to read configuration from");
	}

	public static void AddTypedAzureContainer<T>(this IServiceCollection services, T container) where T : class
	{
		services.AddSingleton<T>(container); //TODO: Figure out what to do with it
	}

	public static void RegisterAzureContainer(this IServiceCollection services, string containerName)
	{
		services.AddSingleton(containerName);
	}
}
