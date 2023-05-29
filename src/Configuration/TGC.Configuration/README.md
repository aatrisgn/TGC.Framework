# TGC.CSharpCodingStandards

This NuGet package is the personal C# coding standards for packages associated with TGC.Framework developed by Asger Thyregod.

## Usage

This package wraps Microsoft.Extensions.Configuration and eases the consumption of configuration through a shared interface.

To use this package, you simply need to use on your service collection:

	builder.Services.AddAppSettingsAbstraction();

This will automatically read environment variables, appsettings.json and local.settings.json (assuming they are located in your root directory). These will be injected into IAppSettings which can be used as a dependency injection in any service needed.

	namespace Your.Namespace;

	public class Service
	{
		private readonly IAppSettings _appSettings;

		public Service(IAppSettings appSettings)
		{
			_appSettings = appSettings;
		}

		public void DoStuff()
		{
			var configurationValue = _appSettings.GetString("My:Key"); // Returns the value for { "My":{ "Key": "SomeKeyValue"} } (SomeKeyValue).
		}
	}

The IAppSettings allows for retrieval of configuration values as strings, bool, integers, doubles and typed:

	namespace Your.Namespace;

	public class Service
	{
		private readonly IAppSettings _appSettings;

		public Service(IAppSettings appSettings)
		{
			_appSettings = appSettings;
		}

		public void DoStuff()
		{
			var stringValue = _appSettings.GetString("My:string");
			var boolValue = _appSettings.GetString("My:bool");
			var intValue = _appSettings.GetString("My:int");
			var doubleValue = _appSettings.GetString("My:double");
		}

		public T GetTypedConfiguration(string? key) where T : class
		{
			// You can either retrieve a typed configuration from root via:
			T someType = _appSettings.GetTyped<T>();

			// Or if it is a sub-section you can also define the key path like:
			T sameType = _appSettings.GetTyped<T>(key);
		}
	}


## Changelog

### 1.0.0
First version ready to be consumed by other packages

### 1.1.0
Automatically reading appsettings.json and local.settings.json. Now possible to define a list of additional json files to read into IAppSettings as well. Also updated unit tests.

### 1.1.1
Fixed unused using and updated documentation.