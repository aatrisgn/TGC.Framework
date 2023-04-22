using System.Reflection;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Configuration;

namespace TGC.AzureTableStorage.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAzureTableStorage(this IServiceCollection services)
	{
		var applicationRootPath = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot")  // local_root
					?? (Environment.GetEnvironmentVariable("HOME") == null
						? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
						: $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot"); // azure_root

		services.AddOptions<StorageConfiguration>().Configure<IConfiguration>((settings, configuration) =>
		{
			configuration.GetSection(nameof(StorageConfiguration)).Bind(settings);
		});

		services.AddSingleton<ITableStorageContext, TableStorageContext>();

		return services;
	}

	public static IServiceCollection AddStorageRepository<T>(this IServiceCollection services) where T : class, ITableEntity, new()
	{
		services.AddScoped<IAzureTableStorageRepository<T>, AzureTableStorageRepository<T>>();
		return services;
	}
}
