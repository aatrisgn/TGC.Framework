// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using TGC.ConsoleBuilder;
using TGC.ConsoleBuilder.TestConsole;

var consoleAppBuilder = ConsoleBuilder.CreateApp(args);

var consoleApp = consoleAppBuilder.Build();

//await consoleApp.RunAsync();

consoleApp.RunFromServiceProvider(s => s.GetRequiredService<ITestService>().DoStuff());
