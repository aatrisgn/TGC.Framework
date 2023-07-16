using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TGC.OAuth2.Abstractions;

/// <summary>
/// Context for retrieving principal informations via Claims from IdentityProvider
/// </summary>
public interface IPrincipalContext
{
	/// <summary>
	/// Access token
	/// </summary>
	public string? AccessToken { get; }
	/// <summary>
	/// ObjectId for the principal.
	/// </summary>
	public string? ObjectId { get; }
	/// <summary>
	/// ID of the token issuer
	/// </summary>
	public string? IssuerId { get; }
	/// <summary>
	/// All available claims on the principal
	/// </summary>
	public IEnumerable<Claim> Claims { get; }

	void SetBearerToken(IHeaderDictionary headers);
	void SetPrincipalContext(ClaimsPrincipal? identity);
}
