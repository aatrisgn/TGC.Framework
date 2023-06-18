using Microsoft.Extensions.DependencyInjection;
using TGC.AzureStorage.IoC;

namespace TGC.AzureStorage.TestConsole
{
	internal class Program
	{
		public static string ContainerName1 = "SomeContainer";
		public static string ContainerName2 = "OtherContainer";

		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			var serviceCollection = new ServiceCollection();
			serviceCollection.ConfigureAzureStorageConnectionString();
			serviceCollection.AddSingleton<TestService>();

			serviceCollection.RegisterAzureContainer(ContainerName1);
			serviceCollection.RegisterAzureContainer(ContainerName2);

			var serviceProvider = serviceCollection.BuildServiceProvider();
			var testService = serviceProvider.GetRequiredService<TestService>();
			testService.Run();

		}
	}
}