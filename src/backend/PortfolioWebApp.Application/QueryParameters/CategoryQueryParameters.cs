using System.ComponentModel.DataAnnotations;
using PortfolioWebApp.Application.Validation;

namespace PortfolioWebApp.Application.QueryParameters;

public sealed class CategoryQueryParameters
{
    public string? Title { get; init; }

    [AllowedStringValues("Id", "Title", "DisplayOrder",
        ErrorMessage = "SortBy must be one of: Id, Title, DisplayOrder.")]
    public string? SortBy { get; init; } = "DisplayOrder";

    [AllowedStringValues("asc", "desc",
        ErrorMessage = "SortDirection must be either 'asc' or 'desc'.")]
    public string? SortDirection { get; init; } = "asc";

    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;

    [Range(1, 100)] public int PageSize { get; init; } = 10;
}