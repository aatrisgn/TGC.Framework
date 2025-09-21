using System.Collections.Immutable;
using Azure;

namespace TGC.AzureTableStorage.Tests;

internal class MockPageable<T> : Pageable<T> where T : notnull
{
	private readonly List<T> _data;

	public MockPageable(IEnumerable<T> data)
	{
		_data = data.ToList();
	}

	public override IEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
	{
		var returnList = new List<Page<T>>();

		if (pageSizeHint is not null && pageSizeHint.Value > 0)
		{
			for (int i = 0; i < _data.Count; i++)
			{
				var relevantData = _data.Skip(i * pageSizeHint.Value).Take(pageSizeHint.Value);
				var singlePage = new MockPage<T>(relevantData.ToImmutableList());
				returnList.Add(singlePage);
			}
		}
		else
		{
			var singlePage = new MockPage<T>(_data.AsReadOnly());
			returnList.Add(singlePage);
		}

		return returnList;
	}
}
