using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TGC.OAuth2.Abstractions;
using TGC.OAuth2.Configuration;
using TGC.OAuth2.Exceptions;

namespace TGC.OAuth2.IoC;

public static class IServiceProviderExtensions
{
	public static IServiceCollection AddOAuthServices(this IServiceCollection serviceCollection, AuthConfigurationBuilder authConfigurationBuilder)
	{
		serviceCollection.AddCoreServices(authConfigurationBuilder.ToConfiguration());
		return serviceCollection;
	}

	public static IServiceCollection AddOAuthServices(this IServiceCollection serviceCollection, Action<AuthConfigurationBuilder> authConfigurationBuilderAction)
	{
		serviceCollection.AddScoped<IPrincipalContext, PrincipalContext>();
		return serviceCollection;
	}

	private static IServiceCollection AddCoreServices(this IServiceCollection serviceDescriptors, AuthConfiguration authConfiguration)
	{
		var locatedIdentityProviders = 0;

		if (authConfiguration.UseAzureAdSingleTenant)
		{
			serviceDescriptors.AddAzureADSingleTenant(authConfiguration);
			locatedIdentityProviders++;
		}

		if (authConfiguration.UseAzureAdMultiTenant)
		{
			serviceDescriptors.AddAzureADMultiTenant(authConfiguration);
			locatedIdentityProviders++;
		}

		if (authConfiguration.UseAzureB2C)
		{
			serviceDescriptors.AddAzureADB2C(authConfiguration);
			locatedIdentityProviders++;
		}

		if (!authConfiguration.MultipleIdentityProviders && locatedIdentityProviders > 1)
		{
			throw new MultipleIdentityProvidersRegisteredException("Multiple identity providers has been registered. Ensure this is correct and define it in the configuration");
		}

		if (locatedIdentityProviders == 0)
		{
			throw new NoIdentityProviderRegisteredException("No identityproviders has been registered. Do not inject services if no identity provider is needed.");
		}

		serviceDescriptors.AddScoped<IPrincipalContext, PrincipalContext>();
		return serviceDescriptors;
	}

	private static IServiceCollection AddAzureADSingleTenant(this IServiceCollection serviceDescriptors, AuthConfiguration authConfiguration)
	{
		serviceDescriptors.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = authConfiguration.ValidAuthority;
				options.Audience = authConfiguration.ClientId;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = authConfiguration.ValidIssuer,
					ValidAudiences = authConfiguration.ValidAudiences
				};
			});

		serviceDescriptors.AddAuthorization();

		return serviceDescriptors;
	}

	private static IServiceCollection AddAzureADMultiTenant(this IServiceCollection serviceDescriptors, AuthConfiguration authConfiguration)
	{
		serviceDescriptors.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = authConfiguration.ValidAuthority;
				options.Audience = authConfiguration.ClientId;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuers = authConfiguration.ValidIssuers,
					ValidAudiences = authConfiguration.ValidAudiences
				};
			});

		serviceDescriptors.AddAuthorization();

		return serviceDescriptors;
	}

	private static IServiceCollection AddAzureADB2C(this IServiceCollection serviceDescriptors, AuthConfiguration authConfiguration)
	{
		throw new NotImplementedException("Azure AD B2C is yet to be implemented.");
	}
}
