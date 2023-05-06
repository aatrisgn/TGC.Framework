namespace TGC.WebApiBuilder;

/// <summary>
/// Attribute used for dynamically locating classes which contains logic for handling dependencyInjections
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DependencyInjectionBuilderAttribute : Attribute
{
}
