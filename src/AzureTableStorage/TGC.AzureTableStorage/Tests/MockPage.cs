using Azure;

namespace TGC.AzureTableStorage.Tests;

internal class MockPage<T> : Page<T>
{
	public MockPage(IReadOnlyList<T> inputData)
	{
		Values = inputData;
	}

	public override Response GetRawResponse()
	{
		throw new NotImplementedException();
	}

	public override IReadOnlyList<T> Values { get; }
	public override string? ContinuationToken { get; }
}
