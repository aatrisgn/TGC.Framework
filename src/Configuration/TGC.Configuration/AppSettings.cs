using Microsoft.Extensions.Configuration;
using TGC.Configuration.Exceptions;

namespace TGC.Configuration;
internal class AppSettings : IAppSettings
{
	private readonly IConfiguration _configuration;
	public AppSettings(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public bool GetBoolen(string key)
	{
		return GetValue<bool>(key);
	}

	public double GetDouble(string key)
	{
		return GetValue<double>(key);
	}

	public int GetInt(string key)
	{
		return GetValue<int>(key);
	}

	public string GetString(string key)
	{
		return GetValue<string>(key);
	}

	public T GetTyped<T>()
	{
		return GetTyped<T>(typeof(T).Name);
	}

	public T GetTyped<T>(string key)
	{
		var typedConfiguration = _configuration.GetRequiredSection(key).Get<T>();
		if (typedConfiguration != null)
		{
			return typedConfiguration;
		}
		throw new TypedConfigurationNotFoundException($"No matching configuration section of name {key} was found.");
	}

	private T GetValue<T>(string key)
	{
		var configurationValue = _configuration.GetValue<T>(key);
		if (configurationValue != null)
		{
			return configurationValue;
		}
		throw new TypedConfigurationNotFoundException($"No matching configuration value of name {key} was found.");
	}
}
