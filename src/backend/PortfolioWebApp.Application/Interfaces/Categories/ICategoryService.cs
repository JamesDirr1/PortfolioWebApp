using PortfolioWebApp.Application.Common;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.QueryParameters;

namespace PortfolioWebApp.Application.Interfaces.Categories;

public interface ICategoryService
{
    Task<PagedResponse<CategoryDto>> GetAllAsync(CategoryQueryParameters query,
        CancellationToken cancellationToken = default);

    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}