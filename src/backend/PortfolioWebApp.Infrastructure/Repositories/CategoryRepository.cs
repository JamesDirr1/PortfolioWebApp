using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Domain.Queries;
using PortfolioWebApp.Infrastructure.Data;


namespace PortfolioWebApp.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public async Task<List<Category>> GetAllAsync(CategoryQueryParameters query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
        pageSize = Math.Min(pageSize, 100);

        IQueryable<Category> categoriesQuery = dbContext.Categories
            .AsNoTracking()
            .Where(category => category.IsActive);

        // Filtering
        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            var normalizedName = query.Title.Trim();

            categoriesQuery = categoriesQuery.Where(category =>
                EF.Functions.ILike(category.Title, $"%{normalizedName}%"));
        }

        categoriesQuery = ApplySorting(categoriesQuery, query.SortBy, query.SortDirection);
        categoriesQuery = categoriesQuery.Skip((page - 1) * pageSize).Take(pageSize);
        
        return await categoriesQuery.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories
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