using System;
using System.Collections.Generic;
using Azure;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using TGC.AzureTableStorage.Extensions;

namespace TGC.AzureTableStorage.Benchmark;

[MemoryDiagnoser]
public class Benchmarks
{
    private List<TableEntity> testList = new List<TableEntity>();

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < 1000; i++)
        {
            testList.Add(new TableEntity());
        }
    }
    
    [Benchmark]
    public void Scenario1()
    {
        var page = Page<TableEntity>.FromValues(testList, continuationToken: null, null);
        var pages = AsyncPageable<TableEntity>.FromPages(new[] { page });

        var some = pages.AsIEnumerableAsync();
    }

    [Benchmark]
    public void Scenario2()
    {
        var page = Page<TableEntity>.FromValues(testList, continuationToken: null, null);
        var pages = AsyncPageable<TableEntity>.FromPages(new[] { page });

        var some = pages.AsIEnumerableAsync();
    }
    
    private class TableEntity : AzureTableItem
    {

    }
}