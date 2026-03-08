using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Infrastructure.Data;
using static PortfolioWebApp.Domain.Interfaces.ICategoryRepository;

namespace PortfolioWebApp.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .Where(category => category.IsActive)
            .OrderBy(category => category.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == id,
                cancellationToken);
    }
}