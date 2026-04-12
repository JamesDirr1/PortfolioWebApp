using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Application.Interfaces.Categories;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CategoryQueryParameters query, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}