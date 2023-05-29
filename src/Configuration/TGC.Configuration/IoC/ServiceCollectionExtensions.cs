using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TGC.Configuration.Exceptions;

namespace TGC.Configuration.IoC;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Extension method to read environment variables, appsettings.json and local.settings.json into IAppSettings.
	/// </summary>
	/// <param name="serviceCollection">Service collection to extend with IAppSettings</param>
	/// <param name="jsonConfigurationFiles">Additional json configuration files to inject into IAppSettings.</param>
	/// <returns></returns>
	/// <exception cref="UnableToLocateAppSettingsException"></exception>
	public static IServiceCollection AddAppSettingsAbstraction(this IServiceCollection serviceCollection, params string[]? jsonConfigurationFiles)
	{
		var applicationRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		if (applicationRootPath != null)
		{
			var appSettingsFile = Path.Combine(applicationRootPath, "appsettings.json");
			var functionAppSettings = Path.Combine(applicationRootPath, "local.settings.json");

			var config = new ConfigurationBuilder()
				.AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true)
				.AddJsonFile(functionAppSettings, optional: true, reloadOnChange: true)
				.AddEnvironmentVariables();

			if (jsonConfigurationFiles != null && jsonConfigurationFiles.Length > 0)
			{
				foreach (var jsonConfigurationFile in jsonConfigurationFiles)
				{
					var filePath = Path.Combine(applicationRootPath, jsonConfigurationFile);

					if (!File.Exists(filePath))
					{
						throw new UnableToLocateAppSettingsException($"IoC was not able to locate a configuration file {filePath}");
					}

					config.AddJsonFile(filePath, optional: true, reloadOnChange: true);
				}
			}

			var buildedConfig = config.Build();

			var instancedAppSettings = new AppSettings(buildedConfig);
			StaticAppSettings.SetConfiguration(instancedAppSettings);

			serviceCollection.AddSingleton<IAppSettings>(instancedAppSettings);

			return serviceCollection;

			throw new UnableToLocateAppSettingsException($"IoC was not able to locate a configuration file named {appSettingsFile}");
		}

		throw new UnableToLocateAppSettingsException($"IoC was unable to detect location of executing assembly to locate app settings.");
	}
}
