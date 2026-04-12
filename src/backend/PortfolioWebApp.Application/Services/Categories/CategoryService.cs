using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Application.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<List<CategoryDto>> GetAllAsync(CategoryQueryParameters query, CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository.GetAllAsync(query, cancellationToken);

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            DisplayOrder = category.DisplayOrder,
            Description = category.Description,
        }).ToList();
        
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