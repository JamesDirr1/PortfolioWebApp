using System.ComponentModel.DataAnnotations;

namespace PortfolioWebApp.Domain.Queries;

public sealed class CategoryQueryParameters
{
    public string? Title { get; init; }

    public string? SortBy { get; init; } = "DisplayOrder";

    public string? SortDirection { get; init; } = "asc";

    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;

    [Range(1, 100)] public int PageSize { get; init; } = 10;
}