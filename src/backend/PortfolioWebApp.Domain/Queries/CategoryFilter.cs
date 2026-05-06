namespace PortfolioWebApp.Domain.Queries;

public sealed class CategoryFilter
{
    public string? Title { get; init; }
    public string SortBy { get; init; } = "DisplayOrder";
    public string SortDirection { get; init; } = "asc";
    public int Page { get; init; }
    public int PageSize { get; init; }
}