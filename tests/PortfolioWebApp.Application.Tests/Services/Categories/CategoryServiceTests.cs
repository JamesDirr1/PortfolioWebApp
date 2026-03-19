using FluentAssertions;
using Moq;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.Services.Categories;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Interfaces;

namespace PortfolioWebApp.Application.Tests.Services.Categories;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllCategories()
        // Get All Categories Successful
        // Should return a list of mapped CategoryDtos
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category()
            {
                Id = 1,
                Title = "Test Category 1",
                Description = "Some cool description",
                DisplayOrder = 1,
                IsActive = true,
                Slug = "test-category-1"
            },
            new Category()
            {
                Id = 2,
                Title = "Test Category 2",
                Description = "Some crazy description",
                DisplayOrder = 2,
                IsActive = true,
                Slug = "test-category-2"
            }
        };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        var categoryService = new CategoryService(repoMock.Object);
        // Act
        var results = await categoryService.GetAllAsync();
        // Assert
        results.Should().HaveCount(2);
        results[0].Id.Should().Be(1);
        results[0].Title.Should().Be("Test Category 1");
        results[0].Description.Should().Be("Some cool description");
        results[0].DisplayOrder.Should().Be(1);
        results[0].IsActive.Should().Be(true);
        results[0].Slug.Should().Be("test-category-1");

        repoMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCategories_EmptyList()
        // Get all but no categories exist 
        // Should return an empty list
    {
        // Arrange
        var categories = new List<Category>();
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        var categoryService = new CategoryService(repoMock.Object);
        // Act
        var results = await categoryService.GetAllAsync();
        // Assert
        results.Should().BeEmpty();
    }


    [Fact]
    public async Task GetCategoryById()
        //Get Category by ID Successful
        //Should return a mapped CategoryDto
    {
        // Arrange
        var category = new Category
        {
            Id = 5,
            Title = "Test Category 5",
            Description = "Some lazy description",
            DisplayOrder = 3,
            IsActive = true,
            Slug = "test-category-5"
        };
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var service = new CategoryService(repoMock.Object);
        // Act
        var result = await service.GetByIdAsync(5);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(5);
        result.Title.Should().Be("Test Category 5");
        result.Description.Should().Be("Some lazy description");
        result.DisplayOrder.Should().Be(3);
        result.IsActive.Should().Be(true);
        result.Slug.Should().Be("test-category-5");
    }

    [Fact]
    public async Task GetCategoryById_NotFound()
        //Get Category by ID category not found
        //Should return null
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);
        var service = new CategoryService(repoMock.Object);
        // Act
        var result = await service.GetByIdAsync(99);
        // Assert
        result.Should().BeNull();
    }
}