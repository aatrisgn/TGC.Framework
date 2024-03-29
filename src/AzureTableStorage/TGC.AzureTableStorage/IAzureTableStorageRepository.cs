﻿using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public interface IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	Task<Response> CreateAsync(T tableEntity);
	Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default);
	AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default);
}
