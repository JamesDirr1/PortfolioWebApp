using PortfolioWebApp.Domain.Entities;

namespace PortfolioWebApp.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}