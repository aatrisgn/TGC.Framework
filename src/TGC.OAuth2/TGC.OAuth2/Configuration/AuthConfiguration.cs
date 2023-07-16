namespace TGC.OAuth2.Configuration;
internal class AuthConfiguration : IAuthConfiguration
{
	public bool UseAzureAdSingleTenant { get; internal set; }

	public bool UseAzureAdMultiTenant { get; internal set; }

	public bool UseAzureB2C { get; internal set; }
	public bool MultipleIdentityProviders { get; internal set; }

	public string? ClientId { get; internal set; }

	public string? ValidIssuer { get; internal set; }
	public string? ValidAuthority { get; internal set; }

	public IEnumerable<string>? ValidAudiences { get; internal set; }
	public IEnumerable<string>? ValidIssuers { get; internal set; }
}
