using System.Collections.Immutable;
using Azure;

namespace TGC.AzureTableStorage.Tests;

internal class MockAsyncPageable<T> : AsyncPageable<T> where T : notnull
{
	private readonly List<T> _data;

	public MockAsyncPageable(IEnumerable<T> data)
	{
		_data = data.ToList();
	}

	public override async IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
	{
		if (pageSizeHint is not null && pageSizeHint.Value > 0)
		{
			for (int i = 0; i < _data.Count; i++)
			{
				var relevantData = _data.Skip(i * pageSizeHint.Value).Take(pageSizeHint.Value);
				var singlePage = new MockPage<T>(relevantData.ToImmutableList());
				yield return singlePage;
			}
		}
		else
		{
			var singlePage = new MockPage<T>(_data.AsReadOnly());
			yield return singlePage;
		}
	}
}
