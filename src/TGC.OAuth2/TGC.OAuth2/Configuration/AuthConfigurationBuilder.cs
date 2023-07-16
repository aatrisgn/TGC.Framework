namespace TGC.OAuth2.Configuration;
public class AuthConfigurationBuilder : IAuthConfiguration
{
	public bool UseAzureAdSingleTenant { get; internal set; }
	public bool UseAzureAdMultiTenant { get; internal set; }
	public bool UseAzureB2C { get; internal set; }
	public string? ClientId { get; internal set; }
	public string? ValidIssuer { get; internal set; }
	public string? ValidAuthority { get; internal set; }
	public IEnumerable<string>? ValidAudiences { get; internal set; }
	public IEnumerable<string>? ValidIssuers { get; internal set; }

	public bool MultipleIdentityProviders { get; internal set; }

	internal AuthConfiguration ToConfiguration()
	{
		return new AuthConfiguration
		{
			ClientId = ClientId,
			ValidAudiences = ValidAudiences,
			ValidAuthority = ValidAuthority,
			ValidIssuers = ValidIssuers,
			ValidIssuer = ValidIssuer,
			UseAzureAdMultiTenant = UseAzureAdMultiTenant,
			UseAzureAdSingleTenant = UseAzureAdSingleTenant,
			UseAzureB2C = UseAzureB2C
		};
	}
}
