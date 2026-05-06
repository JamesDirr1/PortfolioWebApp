using FluentAssertions;
using Moq;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.QueryParameters;
using PortfolioWebApp.Application.Services.Categories;
using PortfolioWebApp.Domain.Common;
using PortfolioWebApp.Domain.Entities;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Application.Tests.Services.Categories;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllCategories()
        // Should return a list of mapped CategoryDtos
    {
        // Arrange
        var categories = new List<Category>
        {
            new()
            {
                Id = 1,
                Title = "Test Category 1",
                Description = "Some cool description",
                DisplayOrder = 1,
                IsActive = true,
                Slug = "test-category-1"
            },
            new()
            {
                Id = 2,
                Title = "Test Category 2",
                Description = "Some crazy description",
                DisplayOrder = 2,
                IsActive = true,
                Slug = "test-category-2"
            }
        };
        var pagedResult = PagedResult<Category>.Create(
            categories,
            page: 1,
            pageSize: 10,
            totalCount: 2);
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetAllAsync(It.IsAny<CategoryFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);
        var categoryService = new CategoryService(repoMock.Object);
        var query = new CategoryQueryParameters
        {
            Page = 1,
            PageSize = 10,
            SortBy = "DisplayOrder",
            SortDirection = "asc"
        };
        // Act
        var result = await categoryService.GetAllAsync(query);
        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);

        result.Items[0].Id.Should().Be(1);
        result.Items[0].Title.Should().Be("Test Category 1");
        result.Items[0].Description.Should().Be("Some cool description");
        result.Items[0].DisplayOrder.Should().Be(1);
        result.Items[0].IsActive.Should().Be(true);
        result.Items[0].Slug.Should().Be("test-category-1");

        result.MetaData.Page.Should().Be(1);
        result.MetaData.PageSize.Should().Be(10);
        result.MetaData.TotalCount.Should().Be(2);
        result.MetaData.TotalPages.Should().Be(1);

        repoMock.Verify(
            x => x.GetAllAsync(
                It.Is<CategoryFilter>(f =>
                    f.Page == 1 &&
                    f.PageSize == 10 &&
                    f.SortBy == "DisplayOrder" &&
                    f.SortDirection == "asc"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllCategories_EmptyList()
        // Should return an empty list
    {
        // Arrange
        var categories = new List<Category>();
        var pagedResult = PagedResult<Category>.Create(
            categories,
            page: 1,
            pageSize: 10,
            totalCount: 0);
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetAllAsync(It.IsAny<CategoryFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);
        var categoryService = new CategoryService(repoMock.Object);
        var query = new CategoryQueryParameters
        {
            Page = 1,
            PageSize = 10,
            SortBy = "DisplayOrder",
            SortDirection = "asc"
        };
        // Act
        var result = await categoryService.GetAllAsync(query);
        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(0);

        result.MetaData.Page.Should().Be(1);
        result.MetaData.PageSize.Should().Be(10);
        result.MetaData.TotalCount.Should().Be(0);
        result.MetaData.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task GetAllCategories_NormalizedQuery()
        // Should Normalize query parameters and pass them to repository
    {
        // Arrange
        var categories = new List<Category>();
        var pagedResult = PagedResult<Category>.Create(
            categories,
            page: 1,
            pageSize: 10,
            totalCount: 0);
        var repoMock = new Mock<ICategoryRepository>();
        repoMock
            .Setup(x => x.GetAllAsync(It.IsAny<CategoryFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);
        var categoryService = new CategoryService(repoMock.Object);
        var query = new CategoryQueryParameters
        {
            Page = 0,
            PageSize = 500,
            Title = "  ART    ",
            SortBy = " ",
            SortDirection = "  desc "
        };
        // Act
        await categoryService.GetAllAsync(query);
        // Assert
        repoMock.Verify(
            x => x.GetAllAsync(
                It.Is<CategoryFilter>(f =>
                    f.Page == 1 &&
                    f.PageSize == 100 &&
                    f.Title == "ART" &&
                    f.SortBy == "DisplayOrder" &&
                    f.SortDirection == "desc"),
                It.IsAny<CancellationToken>()),
            Times.Once);
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