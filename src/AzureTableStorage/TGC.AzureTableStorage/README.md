# TGC.AzureTableStorage

**Disclaimer**: This library is still in alpha and is used on your own risk!

## Using the library

Before doing anything code related, you'll need to configure your appsettings.

Create the following section (Ensure naming is correct):

	"StorageConfiguration": {
		"AccountConnectionString": "DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={AccountKey};EndpointSuffix=core.windows.net"
	}

In order to use the library you will need to add the following extension methods on your IServiceCollection:

	builder.Services.AddAzureTableStorage();

The method above takes care of reading configuration values and setting up context.

The library maps classes to individual tables in the Azure Storage Account. For each table you will need to define a related DTO. The definition requires adding a custom attribute and inherit from a specific class.

An example looks like this:

	namespace Your.Namespace;

	[TableItem("FooBars")]
	internal class FooBarItem : AzureTableItem
	{
		public string Foo { get; set; }
		public string Bar { get; set; }
	}

Once your DTO is defined, you will need to register a service for it. That is simply done the on your IServiceProvider as well with the following method:

	builder.Services.AddStorageRepository<FooBarItem>();

Now, you can simply use the interface IAzureTableStorageRepository with any of your registered DTOs and your IServiceProvider takes care of the rest:

	namespace Your.Namespace;

	public class Service
	{
		private readonly ILogger<Service> _logger;
		private readonly IAzureTableStorageRepository<FooBarItem> _azureTableStorageRepository;

		public Service(ILogger<Service> log, IAzureTableStorageRepository<FooBarItem> azureTableStorageRepository)
		{
			_logger = log;
			_azureTableStorageRepository = azureTableStorageRepository;
		}
	}

## Patch notes

### 0.0.3
First alpha-release

### 0.1.0
Upgraded to .Net 7 and updated readme