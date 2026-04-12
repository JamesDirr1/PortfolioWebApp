using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CategoryQueryParameters query, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}