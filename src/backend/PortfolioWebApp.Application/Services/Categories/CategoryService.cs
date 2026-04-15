using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.Common;
using PortfolioWebApp.Application.QueryParameters;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Application.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<PagedResponse<CategoryDto>> GetAllAsync(CategoryQueryParameters query,
        CancellationToken cancellationToken = default)
    {
        var filter = new CategoryFilter()
        {
            Title = string.IsNullOrWhiteSpace(query.Title)
                ? null
                : query.Title.Trim(),
            SortBy = string.IsNullOrWhiteSpace(query.SortBy)
                ? "DisplayOrder"
                : query.SortBy.Trim(),
            SortDirection = string.IsNullOrWhiteSpace(query.SortDirection)
                ? "asc"
                : query.SortDirection.Trim(),
            Page = query.Page <= 0 ? 1 : query.Page,
            PageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 100)
        };
        
        var pagedResult = await categoryRepository.GetAllAsync(filter, cancellationToken);

        var categoryDtos = pagedResult.Items.Select(category => new CategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            DisplayOrder = category.DisplayOrder,
            Description = category.Description,
        }).ToList();

        return PagedResponse<CategoryDto>.Create(categoryDtos, pagedResult.Page, pagedResult.PageSize,
            pagedResult.TotalCount);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return null;
        }

        return new CategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            DisplayOrder = category.DisplayOrder,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}