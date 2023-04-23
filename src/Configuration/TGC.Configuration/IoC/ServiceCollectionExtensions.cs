using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TGC.Configuration.Exceptions;

namespace TGC.Configuration.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAppSettingsAbstraction(this IServiceCollection serviceCollection, string appSettingsFileName)
	{
		var applicationRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		if (applicationRootPath != null)
		{
			var appSettingsFile = Path.Combine(applicationRootPath, appSettingsFileName);

			var foundAppSettings = File.Exists(appSettingsFile);

			if (foundAppSettings)
			{
				IConfiguration config = new ConfigurationBuilder()
					.AddJsonFile(appSettingsFile)
					.AddEnvironmentVariables()
					.Build();

				var instancedAppSettings = new AppSettings(config);
				StaticAppSettings.SetConfiguration(instancedAppSettings);

				serviceCollection.AddSingleton<IAppSettings>(instancedAppSettings);

				return serviceCollection;
			}

			throw new UnableToLocateAppSettingsException($"IoC was not able to locate a configuration file named {appSettingsFile}");
		}

		throw new UnableToLocateAppSettingsException($"IoC was unable to detect location of executing assembly to locate app settings.");
	}
}
