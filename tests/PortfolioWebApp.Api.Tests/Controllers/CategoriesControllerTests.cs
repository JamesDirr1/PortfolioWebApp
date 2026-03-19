using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PortfolioWebApp.Api.Controllers;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.Interfaces.Categories;

namespace PortfolioWebApp.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOk()
        // Get All Categories Successful
        // Should return 200 okay and a list of categories 
    {
        //Arrange 
        var categories = new List<CategoryDto>
        {
            new CategoryDto
            {
                Id = 1,
                Title = "Test 1",
                Slug = "Test-1",
                Description = "Some cool Description",
                DisplayOrder = 1,
                IsActive = true
            },
            new CategoryDto
            {
                Id = 2,
                Title = "Test 2",
                Slug = "Test-2",
                Description = "Some not a Description",
                DisplayOrder = 2,
                IsActive = false
            }
        };
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        //Act
        var result = await controller.GetAll(CancellationToken.None);
        //Assert 
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var value = ((IEnumerable<CategoryDto>)okResult.Value!).ToList();
        value.Should().HaveCount(2);
        value.Should().Equal(categories);
    }

    [Fact]
    public async Task GetAll_ReturnsOK_EmptyList()
        // Get all but no categories found
        // Should return 200 okay and an empty list
    {
        // Arrange
        var categories = new List<CategoryDto>();
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        // Act
        var result = await controller.GetAll(CancellationToken.None);
        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var value = ((IEnumerable<CategoryDto>)okResult.Value!).ToList();
        value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOK()
        // Get Category by id Successful
        // Should return 200 okay and category of matching id
    {
        // Arrange 
        var category = new CategoryDto()
        {
            Id = 1,
            Title = "Test 1",
            Slug = "Test-1",
            Description = "Some cool Description",
            DisplayOrder = 1,
            IsActive = true
        };
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        //Act
        var result = await controller.GetById(1, CancellationToken.None);
        //Assert 
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound()
        // Get Category by id not found
        // Should return Not Found 
    {
        // Arrange
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryDto?)null);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        // Act
        var result = await controller.GetById(1, CancellationToken.None);
        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
        notFound.Value.Should().BeEquivalentTo(new { message = "Category not found" });
    }
}