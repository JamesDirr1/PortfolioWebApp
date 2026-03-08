using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Domain.Entities;

namespace PortfolioWebApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Category> Categories => Set<Category>();
}