using System.ComponentModel.DataAnnotations;

namespace PortfolioWebApp.Application.QueryParameters;

public sealed class CategoryQueryParameters
{
    public string? Title { get; init; }

    [AllowedValues("Id", "Title", "DisplayOrder",
        ErrorMessage = "SortBy must be one of: Id, Title, DisplayOrder.")]
    public string? SortBy { get; init; } = "DisplayOrder";

    [AllowedValues("asc", "desc",
        ErrorMessage = "SortDirection must be either 'asc' or 'desc'.")]
    public string? SortDirection { get; init; } = "asc";

    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;

    [Range(1, 100)] public int PageSize { get; init; } = 10;
}