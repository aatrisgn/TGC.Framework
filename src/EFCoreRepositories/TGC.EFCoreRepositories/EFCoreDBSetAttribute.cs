namespace TGC.EFCoreRepositories;

/// <summary>
/// Attribute which is used to easy locate data classes which should be mapped to EF context.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EFCoreDBSetAttribute : Attribute
{
}
