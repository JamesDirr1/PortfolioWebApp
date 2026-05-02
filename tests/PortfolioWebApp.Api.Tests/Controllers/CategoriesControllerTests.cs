using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PortfolioWebApp.Api.Controllers;
using PortfolioWebApp.Api.Responses;
using PortfolioWebApp.Application.Common;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.QueryParameters;

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
            new()
            {
                Id = 1,
                Title = "Test 1",
                Slug = "Test-1",
                Description = "Some cool Description",
                DisplayOrder = 1,
                IsActive = true
            },
            new()
            {
                Id = 2,
                Title = "Test 2",
                Slug = "Test-2",
                Description = "Some not a Description",
                DisplayOrder = 2,
                IsActive = false
            }
        };

        var pagedResponse = PagedResponse<CategoryDto>.Create(
            categories,
            page: 1,
            pageSize: 10,
            totalCount: 2);

        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CategoryQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResponse);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        var query = new CategoryQueryParameters();
        //Act
        var result = await controller.GetAll(query, CancellationToken.None);
        //Assert 
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should()
            .BeOfType<ApiResponse<PagedResponse<CategoryDto>>>()
            .Subject;
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Categories retrieved successfully.");

        response.Data.Should().NotBeNull();
        response.Data!.Items.Should().HaveCount(2);
        response.Data.Items.Should().BeEquivalentTo(categories);

        response.Data.MetaData.Page.Should().Be(1);
        response.Data.MetaData.PageSize.Should().Be(10);
        response.Data.MetaData.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetAll_ReturnsOK_EmptyList()
        // Get all but no categories found
        // Should return 200 okay and an empty list
    {
        // Arrange
        var categories = new List<CategoryDto>();
        var pagedResponse = PagedResponse<CategoryDto>.Create(
            categories,
            page: 1,
            pageSize: 10,
            totalCount: 0);
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetAllAsync(It.IsAny<CategoryQueryParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResponse);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        var query = new CategoryQueryParameters();
        // Act
        var result = await controller.GetAll(query, CancellationToken.None);
        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should()
            .BeOfType<ApiResponse<PagedResponse<CategoryDto>>>()
            .Subject;
        response.Success.Should().BeTrue();
        response.Message.Should().Be("No categories found.");

        response.Data.Should().NotBeNull();
        response.Data!.Items.Should().HaveCount(0);
        response.Data.Items.Should().BeEquivalentTo(categories);

        response.Data.MetaData.Page.Should().Be(1);
        response.Data.MetaData.PageSize.Should().Be(10);
        response.Data.MetaData.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetById_ReturnsOK()
        // Get Category by id Successful
        // Should return 200 okay and category of matching id
    {
        // Arrange 
        var category = new CategoryDto
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
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should()
            .BeOfType<ApiResponse<CategoryDto>>()
            .Subject;
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Category retrieved successfully.");
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(category);
    }


    [Fact]
    public async Task GetById_ReturnsNotFound()
        // Get Category by id but no category with matching id exists
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
        var notFound = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
        var response = notFound.Value
            .Should()
            .BeOfType<ApiResponse<CategoryDto>>()
            .Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Be("Category not found.");
        response.Data.Should().BeNull();
        response.Errors.Should().NotBeEmpty();
        response.Errors.Should().ContainSingle()
            .Which.Should().Be("No Category exists with id of 1.");
    }

    [Fact]
    public async Task GetById_ReturnsError_InvalidID()
        // Get Category by id but with invalid id
        // Should return client error
    {
        // Arrange
        var serviceMock = new Mock<ICategoryService>();
        serviceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryDto?)null);
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        // Act
        var result = await controller.GetById(0, CancellationToken.None);
        // Assert
        var badResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badResult.StatusCode.Should().Be(400);
        var response = badResult.Value
            .Should()
            .BeOfType<ApiResponse<CategoryDto>>()
            .Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Be("Invalid Category id.");
        response.Data.Should().BeNull();
        response.Errors.Should().NotBeEmpty();
        response.Errors.Should().ContainSingle()
            .Which.Should().Be("Id must be greater than 0.");
    }

    [Fact]
    public void GetByInvalidId_ReturnsError_InvalidType()
        // Get Category by id but with invalid id type (e.g. string instead of int)
        // Should return client error
    {
        // Arrange
        var serviceMock = new Mock<ICategoryService>();
        var loggerMock = new Mock<ILogger<CategoriesController>>();
        var controller = new CategoriesController(serviceMock.Object, loggerMock.Object);
        // Act
        var result = controller.GetByInvalidId("abc");
        // Assert
        var badResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badResult.StatusCode.Should().Be(400);
        var response = badResult.Value
            .Should()
            .BeOfType<ApiResponse<CategoryDto>>()
            .Subject;
        response.Success.Should().BeFalse();
        response.Message.Should().Be("Invalid Category id type.");
        response.Data.Should().BeNull();
        response.Errors.Should().NotBeEmpty();
        response.Errors.Should().ContainSingle()
            .Which.Should().Be("'abc' is not a valid Category id. Id must be an integer greater than 0.");
    }
}