using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Queries;
using PortfolioWebApp.Infrastructure.Data;
using PortfolioWebApp.Infrastructure.Repositories;
using Testcontainers.PostgreSql;

namespace PortfolioWebApp.Infrastructure.Tests.Repositories;

public class CategoryRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("portfolio_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private AppDbContext _dbContext = null!;
    private CategoryRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();

        await SeedAsync();

        _repository = new CategoryRepository(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyActiveCategories()
        // Should return only active categories and exclude inactive ones
    {
        // Arrange 
        var filter = new CategoryFilter
        {
            Page = 1,
            PageSize = 10,
            SortBy = "DisplayOrder",
            SortDirection = "asc"
        };
        // Act
        var result = await _repository.GetAllAsync(filter);
        // Assert
        result.Items.Should().OnlyContain(category => category.IsActive);
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetAllAsync_FiltersByTitle_NormalizedTitle()
        // Should return categories that match the normalized title filter and ignore leading/trailing whitespace
    {
        // Arrange 
        var filter = new CategoryFilter
        {
            Page = 1,
            PageSize = 10,
            Title = "   web Design   ",
        };
        // Act
        var result = await _repository.GetAllAsync(filter);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Title.Should().Be("Web Design");
    }

    [Fact]
    public async Task GetAllAsync_SortsByTitleDescending()
        // Should return categories sorted by title in descending order when specified in the filter
    {
        // Arrange
        var filter = new CategoryFilter
        {
            Page = 1,
            PageSize = 10,
            SortBy = "Title",
            SortDirection = "desc"
        };
        // Act
        var result = await _repository.GetAllAsync(filter);
        // Assert
        result.Items.Select(category => category.Title)
            .Should()
            .Equal("Web Design", "Painting", "Illustration");
    }
    
    [Fact]
    public async Task GetAllAsync_PaginatesResults()
    // Should return the correct page of results based on the page number and page size specified in the filter
    {
        // Arrange
        var filter = new CategoryFilter
        {
            Page = 2,
            PageSize = 2,
            SortBy = "DisplayOrder",
            SortDirection = "asc"
        };
        // Act
        var result = await _repository.GetAllAsync(filter);
        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Title.Should().Be("Web Design");
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.TotalCount.Should().Be(3);
    }


    private async Task SeedAsync()
    {
        _dbContext.Categories.AddRange(
            new Category
            {
                Id = 1,
                Title = "Illustration",
                Slug = "illustration",
                Description = "Illustration work",
                DisplayOrder = 1,
                IsActive = true
            },
            new Category
            {
                Id = 2,
                Title = "Painting",
                Slug = "painting",
                Description = "Painting work",
                DisplayOrder = 2,
                IsActive = true
            },
            new Category
            {
                Id = 3,
                Title = "Web Design",
                Slug = "web-design",
                Description = "Web design work",
                DisplayOrder = 3,
                IsActive = true
            },
            new Category
            {
                Id = 4,
                Title = "Hidden Category",
                Slug = "hidden-category",
                Description = "Inactive",
                DisplayOrder = 4,
                IsActive = false
            }
        );

        await _dbContext.SaveChangesAsync();
    }
}