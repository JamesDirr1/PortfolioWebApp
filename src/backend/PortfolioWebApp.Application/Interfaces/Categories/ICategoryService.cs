using PortfolioWebApp.Application.DTOs.Categories;
namespace PortfolioWebApp.Application.Interfaces.Categories;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}