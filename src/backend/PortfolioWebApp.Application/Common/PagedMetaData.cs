namespace PortfolioWebApp.Application.Common;

public record PagedMetaData
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    public static PagedMetaData Create(int page, int pageSize, int totalCount)
    {
        if (pageSize <= 0)
            throw new ArgumentException("PageSize must be greater than 0", nameof(pageSize));

        if (page <= 0)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedMetaData
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = page > 1,
            HasNextPage = page < totalPages
        };
    }
}