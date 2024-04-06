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

**Disclaimer:** If you are using Azure Function App, the current version does not support *Microsoft.Extensions.Configuration.Abstractions* which this method is dependent on. If so, you can use:

	builder.Services.AddAzureTableStorage("YourConnectionString");

The library maps classes to individual tables in the Azure Storage Account. For each table you will need to define a related DTO. The definition requires adding a custom attribute and inherit from a specific class.

An example looks like this:

	namespace Your.Namespace;

	[TableItem("FooBars")]
	internal class FooBarItem : AzureTableItem
	{
		public string Foo { get; set; }
		public string Bar { get; set; }
	}

Now, you can simply use the interface IAzureTableStorageRepository with any of your registered DTOs. The repositories are defined as unbound generics, meaning you can easily inject it with any type which inherits from ITableEntity (via AzureTableItem). Your IServiceProvider takes care of the rest:

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

## Changelog

Patch notes ordered by newest change

### 0.3.1
Changed provision of TableClient to factory pattern and upgraded unit tests to .Net 8 due to .Net version mismatch.

### 0.3.0
**Breaking change**. Upgraded package and .Net version to .Net 8. Updated configuration flow for client library. Minor changes to interfaces.

### 0.2.0
**Breaking change**. Support for Azure Managed Identity and changed configuration type for public injection.

### 0.1.5
Updated versions of TGC.Configuration and TGC.CodingStandards.

### 0.1.4
Fixed bug with not defining RowKey

### 0.1.3
Enhanced unit tests and elobrated Readme.md

### 0.1.2
Added another way of configuring storage context with connectionstring directly

### 0.1.1
Fixed failing unit tests

### 0.1.0
Upgraded to .Net 7 and updated readme

### 0.0.3
First alpha-release
