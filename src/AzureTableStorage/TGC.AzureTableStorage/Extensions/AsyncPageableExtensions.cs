using Azure;

namespace TGC.AzureTableStorage.Extensions;

public static class AsyncPageableExtensions
{
	public static async Task<IEnumerable<T>> AsIEnumerableAsync<T>(this AsyncPageable<T> pageable) where T : class
	{
		var resultList = new List<T>(1000);
		await foreach (var item in pageable)
		{
			resultList.Add(item);
		}
		return resultList;
	}

	public static async Task<T?> FirstOrDefault<T>(this AsyncPageable<T> pageable) where T : class
	{
		await foreach (var item in pageable)
		{
			return item;
		}
		return default;
	}

	public static async Task<T> SingleAsync<T>(this AsyncPageable<T> pageable) where T : class
	{
		T? locatedItem = default;
		int locatedItemCount = 0;

		await foreach (var item in pageable)
		{
			locatedItemCount++;
			return item;
		}

		if (locatedItem is null)
		{
			//THis is not ideal - Should be changed at a later point.
			throw new InvalidOperationException("No item found");
		}

		if (locatedItemCount != 1)
		{
			throw new InvalidOperationException("More than one item found");
		}

		return locatedItem;
	}
}
