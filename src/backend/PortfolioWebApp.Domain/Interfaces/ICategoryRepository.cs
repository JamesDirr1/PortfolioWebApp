using PortfolioWebApp.Domain.Common;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<PagedResult<Category>>GetAllAsync(CategoryFilter filter, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}