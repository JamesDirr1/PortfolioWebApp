namespace PortfolioWebApp.Application.Common;

public record PagedResponse<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public PagedMetaData MetaData { get; init; } = new PagedMetaData();

    public static PagedResponse<T> Create(
        IReadOnlyList<T> items,
        int page,
        int pageSize,   
        int totalCount)
    {
        return new PagedResponse<T>
        {
            Items = items,
            MetaData = PagedMetaData.Create(page, pageSize, totalCount)
        };
    }
}