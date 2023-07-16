using System.Security.Claims;
using TGC.OAuth2.Abstractions;

namespace TGC.OAuth2.Tests;

public class StubbedPrincipalContext : IPrincipalContext
{
	public string? AccessToken { get; set; }
	public string? ObjectId { get; set; }
	public string? IssuerId { get; set; }
	public IEnumerable<Claim> Claims { get; set; }

	public StubbedPrincipalContext()
	{
		Claims = new List<Claim>();
	}

	public void SetPrincipalContext(ClaimsPrincipal? identity)
	{
		throw new NotImplementedException();
	}
}
