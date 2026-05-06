using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Domain.Common;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Domain.Queries;
using PortfolioWebApp.Infrastructure.Data;


namespace PortfolioWebApp.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public async Task<PagedResult<Category>> GetAllAsync(
        CategoryFilter filter,
        CancellationToken cancellationToken = default)
    {
        var page = filter.Page <= 0 ? 1 : filter.Page;
        var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

        var categoriesQuery = dbContext.Categories
            .AsNoTracking()
            .Where(category => category.IsActive);

        // Filtering
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            var normalizedTitle = filter.Title.Trim();

            categoriesQuery = categoriesQuery.Where(category =>
                EF.Functions.ILike(category.Title, $"%{normalizedTitle}%"));
        }

        var totalCount = await categoriesQuery.CountAsync(cancellationToken);

        categoriesQuery = ApplySorting(categoriesQuery, filter.SortBy, filter.SortDirection);
        categoriesQuery = categoriesQuery.Skip((page - 1) * pageSize).Take(pageSize);
        var results = await categoriesQuery.ToListAsync(cancellationToken);

        return PagedResult<Category>.Create(results, page, pageSize, totalCount);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id,
                cancellationToken);
    }

    private static IQueryable<Category> ApplySorting(
        IQueryable<Category> query,
        string? sortBy,
        string? sortDirection)
    {
        var normalizedSortBy = sortBy?.Trim().ToLowerInvariant();
        var isDescending = string.Equals(
            sortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        return normalizedSortBy switch
        {
            "id" => isDescending
                ? query.OrderByDescending(category => category.Id)
                : query.OrderBy(category => category.Id),

            "title" => isDescending
                ? query.OrderByDescending(category => category.Title)
                : query.OrderBy(category => category.Title),

            "displayorder" => isDescending
                ? query.OrderByDescending(category => category.DisplayOrder)
                : query.OrderBy(category => category.DisplayOrder),

            _ => query.OrderBy(category => category.DisplayOrder)
        };
    }
}