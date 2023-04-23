namespace TGC.Configuration;
public class StaticAppSettings
{
	public static IAppSettings? AppSettings;

	internal static void SetConfiguration(IAppSettings appSettings)
	{
		AppSettings = appSettings;
	}
}
