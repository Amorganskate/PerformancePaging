// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PerformancePaging;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PerformancePaging.Entities;
using BenchmarkDotNet.Running;
using Azure;


var summary = BenchmarkRunner.Run<BenchmarkClass>();


public class NaivePaginationService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private const int Page = 9900;
    private const int PageSize = 10;
    private const int Cursor = 10;
    public NaivePaginationService(ApplicationDbContext dbContext)
    {
        _applicationDbContext = dbContext;
    }

    public async Task<object> GetItemsByNaive()
    {
        var items = await _applicationDbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();

        var pagedItems = items.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

        var totalCount = items.Count();

        return (pagedItems, totalCount);

    }
}

public class CursorPaginationService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private const int Page = 9900;
    private const int PageSize = 10;
    private const int Cursor = 10;

    public CursorPaginationService(ApplicationDbContext dbContext)
    {
        _applicationDbContext = dbContext;
    }

    public async Task<object> GetItemsByCursor()
    {
        var query = _applicationDbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        });

        var pagedItems = await query
            .Where(x => x.Id > Cursor)
            .Take(PageSize) 
            .ToListAsync();

        var nextCursor = pagedItems[^1].Id;

        return (pagedItems, nextCursor);
    }
}

public class OffsetPaginationService
{
    private readonly ApplicationDbContext _dbContext;
    private const int Page = 9900;
    private const int PageSize = 10;
    private const int Cursor = 10;

    public OffsetPaginationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<object> GetItems()
    {
        var query = _dbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        });

        var pagedItems = await query
            .Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var totalCount = query.CountAsync();

        return (pagedItems, totalCount);

    }
}


[MemoryDiagnoser]
public class BenchmarkClass
{
    private ApplicationDbContext _applicationDbContext;
    private const int Page = 1;
    private const int PageSize = 10;
    private const int Cursor = 1;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _applicationDbContext = new ApplicationDbContext();
    }

    [Benchmark]
    public async Task<object> CursorPaginationBench()
    {
        var query = _applicationDbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        });

        var pagedItems = await query
            .Where(x => x.Id > Cursor)
            .Take(PageSize)
            .ToListAsync();

        var nextCursor = pagedItems[^1].Id;

        return (pagedItems, nextCursor);
    }

    [Benchmark]
    public async Task<object> OffSetPaginationBenchmark()
    {
        var query = _applicationDbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        });

        var pagedItems = await query
            .Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var totalCount = await query.CountAsync();

        return (pagedItems, totalCount);
    }

    [Benchmark]
    public async Task<object> TrashPaginationBenchmark()
    {
        var items = await _applicationDbContext.Items.Select(x => new Item
        {
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync();

        var pagedItems = items.Skip((Page - 1) * PageSize).Take(PageSize).ToList();

        var totalCount = items.Count;

        return (pagedItems, totalCount);
    }

    [GlobalCleanup] public void GlobalCleanup() { _applicationDbContext.Dispose(); }
}



public class StoryDto
{
    public int StoryId { get; set; }
    public string StoryHeadline { get; set; } = null!;
    public string? StorySubheadline { get; set; }
    public string? StoryByline { get; set; }
    public string? StorySummary { get; set; }
}