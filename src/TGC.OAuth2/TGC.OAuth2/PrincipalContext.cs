using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TGC.OAuth2.Abstractions;

namespace TGC.OAuth2;

internal class PrincipalContext : IPrincipalContext
{
	public string? AccessToken { get; internal set; }
	public string? ObjectId { get; internal set; }
	public string? IssuerId { get; internal set; }
	public IEnumerable<Claim> Claims { get; internal set; }

	public PrincipalContext()
	{
		Claims = new List<Claim>();
	}

	public void SetPrincipalContext(ClaimsPrincipal? claimsPrincipal)
	{
		if (claimsPrincipal != null)
		{
			Claims = claimsPrincipal.Claims;

			AccessToken = TryGetClaimValue(Claims, "");
			IssuerId = TryGetClaimValue(Claims, "");
			ObjectId = TryGetClaimValue(Claims, "");
		}
	}

	private static string TryGetClaimValue(IEnumerable<Claim> claims, string claimKey)
	{
		var claim = claims.SingleOrDefault(c => c.Type == claimKey);

		if (claim != null)
		{
			return claim.Value != null ? claim.Value : string.Empty;
		}

		return string.Empty;
	}

	public void SetBearerToken(IHeaderDictionary headers)
	{
		AccessToken = headers["bearer"];
	}
}
