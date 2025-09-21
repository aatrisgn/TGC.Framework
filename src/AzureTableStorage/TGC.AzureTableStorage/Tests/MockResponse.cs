using System.Diagnostics.CodeAnalysis;
using Azure;
using Azure.Core;

namespace TGC.AzureTableStorage.Tests;

internal class MockResponse : Response
{
	public MockResponse(int status, string reasonPhrase, Stream? contentStream, string clientRequestId)
	{
		Status = status;
		ReasonPhrase = reasonPhrase;
		ContentStream = contentStream;
		ClientRequestId = clientRequestId;
	}

	public static MockResponse Create200OK()
	{
		return new MockResponse(200, "OK", null, Guid.NewGuid().ToString());
	}

	public static MockResponse Create409Conflict()
	{
		return new MockResponse(409, "Conflict", null, Guid.NewGuid().ToString());
	}

	public override void Dispose()
	{
		this.Dispose();
	}

	protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
	{
		throw new NotImplementedException();
	}

	protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
	{
		throw new NotImplementedException();
	}

	protected override bool ContainsHeader(string name)
	{
		throw new NotImplementedException();
	}

	protected override IEnumerable<HttpHeader> EnumerateHeaders()
	{
		throw new NotImplementedException();
	}

	public override int Status { get; }
	public override string ReasonPhrase { get; }
	public override Stream? ContentStream { get; set; }
	public override string ClientRequestId { get; set; }
}
