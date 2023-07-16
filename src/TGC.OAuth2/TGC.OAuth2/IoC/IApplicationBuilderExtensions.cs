using Microsoft.AspNetCore.Builder;
using TGC.OAuth2.Configuration;
using TGC.OAuth2.Middleware;

namespace TGC.OAuth2.IoC;

/// <summary>
/// Used for injecting middleware into IApplicaitonBuilder.
/// </summary>
public static class IApplicationBuilderExtensions
{
	/// <summary>
	/// Add custom middleware to use IPrincipalContext in .Net web application.
	/// </summary>
	/// <param name="applicationBuilder">Solution IApplicationBuilder for which to use middleware</param>
	/// /// <param name="authConfigurationBuilderAction">Configuration of integration</param>
	/// <returns>IApplicationBuilder</returns>
	public static IApplicationBuilder AddAzureADMiddleware(this IApplicationBuilder applicationBuilder, Action<AuthConfigurationBuilder> authConfigurationBuilderAction)
	{
		//TODO: Fetch via implemented IAuthMiddleware and inject
		applicationBuilder.UseMiddleware<AzureADClaimsHandlerMiddleware>();

		applicationBuilder.AddCoreApplication();

		return applicationBuilder;
	}

	/// <summary>
	/// Add custom middleware to use IPrincipalContext in .Net web application.
	/// </summary>
	/// <param name="applicationBuilder">Solution IApplicationBuilder for which to use middleware</param>
	/// /// <param name="authConfigurationBuilder">Configuration of integration</param>
	/// <returns>IApplicationBuilder</returns>
	public static IApplicationBuilder AddAzureADMiddleware(this IApplicationBuilder applicationBuilder, AuthConfigurationBuilder authConfigurationBuilder)
	{
		//TODO: Fetch via implemented IAuthMiddleware and inject
		applicationBuilder.UseMiddleware<AzureADClaimsHandlerMiddleware>();

		applicationBuilder.AddCoreApplication();

		return applicationBuilder;
	}

	private static IApplicationBuilder AddCoreApplication(this IApplicationBuilder applicationBuilder)
	{
		applicationBuilder.UseAuthentication();
		applicationBuilder.UseAuthorization();

		return applicationBuilder;
	}
}
