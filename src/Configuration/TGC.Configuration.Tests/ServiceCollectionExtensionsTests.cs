using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TGC.Configuration.IoC;
using TGC.Configuration.Tests.TestModels;

namespace TGC.Configuration.Tests;

internal class ServiceCollectionExtensionsTests
{
	private IServiceProvider _serviceProvider;
	[SetUp]
	public void Setup()
	{
		var serviceCollection = CreateServiceCollection();
		serviceCollection.AddAppSettingsAbstraction("appsettings.json");

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveExistingStringValue_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();
		if (appSettings != null)
		{
			var stringValue = appSettings.GetString("stringTest");

			stringValue.Should().NotBeNullOrEmpty();
			stringValue.Should().Be("someValue");
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveExistingBoolValue_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var boolValue = appSettings.GetBoolen("boolTest");

			boolValue.Should().NotBe(false);
			boolValue.Should().Be(true);
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveExistingIntValue_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var intValue = appSettings.GetInt("intTest");

			intValue.Should().NotBe(0);
			intValue.Should().Be(24);
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveExistingDoubleValue_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var doubleValue = appSettings.GetDouble("doubleTest");

			doubleValue.Should().NotBe(0);
			doubleValue.Should().Be(24.52);
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveTypedSection_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var typedConfiguration = appSettings.GetTyped<TypedTest>();

			typedConfiguration.stringProperty.Should().Be("something");
			typedConfiguration.boolTest.Should().Be(true);
			typedConfiguration.intTest.Should().Be(24);
			typedConfiguration.doubleTest.Should().Be(24.52);
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveTypedSectionByKey_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var typedConfiguration = appSettings.GetTyped<TypedTest>("TypedTest");

			typedConfiguration.stringProperty.Should().Be("something");
			typedConfiguration.boolTest.Should().Be(true);
			typedConfiguration.intTest.Should().Be(24);
			typedConfiguration.doubleTest.Should().Be(24.52);
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_InjectedConfiguration_WHEN_TryingToRetrieveSubTypedSectionByKey_THEN_Success()
	{
		var appSettings = _serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var typedConfiguration = appSettings.GetTyped<SubTypeTest>("TypedTest:SubTypeTest");

			typedConfiguration.StringTestProperty.Should().Be("somethingelse");
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_CustomInjectedConfiguration_WHEN_TryingToRetrieveExistingBoolValue_THEN_Success()
	{
		var serviceCollection = CreateServiceCollection();
		serviceCollection.AddAppSettingsAbstraction("randomsettings.json");

		var serviceProvider = serviceCollection.BuildServiceProvider();

		var appSettings = serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var boolValue = appSettings.GetBoolen("randomsettingboolvalue");
			boolValue.Should().BeTrue();
		}
		else
		{
			Assert.Fail();
		}
	}

	[Test]
	public void GIVEN_LocalSettingsInjectedConfiguration_WHEN_TryingToRetrieveExistingBoolValue_THEN_Success()
	{
		var serviceCollection = CreateServiceCollection();
		serviceCollection.AddAppSettingsAbstraction();

		var serviceProvider = serviceCollection.BuildServiceProvider();

		var appSettings = serviceProvider.GetService<IAppSettings>();

		if (appSettings != null)
		{
			var boolValue = appSettings.GetBoolen("localsettingsboolvalue");
			boolValue.Should().BeTrue();
		}
		else
		{
			Assert.Fail();
		}
	}

	private IServiceCollection CreateServiceCollection()
	{
		return new ServiceCollection();
	}
}
