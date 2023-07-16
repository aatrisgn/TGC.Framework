namespace TGC.OAuth2.Configuration;
public interface IAuthConfiguration
{
	public bool UseAzureAdSingleTenant { get; }
	public bool UseAzureAdMultiTenant { get; }
	public bool UseAzureB2C { get; }
	public bool MultipleIdentityProviders { get; }
	public string? ClientId { get; }
	public string? ValidIssuer { get; }
	public string? ValidAuthority { get; }
	public IEnumerable<string>? ValidAudiences { get; }
	public IEnumerable<string>? ValidIssuers { get; }
}
