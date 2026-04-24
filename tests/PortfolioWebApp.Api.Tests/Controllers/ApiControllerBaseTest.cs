using FluentAssertions;

namespace PortfolioWebApp.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Controllers;
using PortfolioWebApp.Api.Responses;
using Xunit;

public class TestApiController : ApiControllerBase
{
    public ActionResult<ApiResponse<string>> SuccessEndpoint(string data, string message = "Request successful.")
        => Success(data, message);

    public ActionResult<ApiResponse<string>> FailureEndpoint(string message = "Request failed.",
        params string[] errors)
        => Failure<string>(message, errors);
}

public class ApiControllerBaseTest
{
    [Fact]
    public void Success_ShouldUseCustomMessage()
        // Should return a success response with the custom message
    {
        // Arrange
        var controller = new TestApiController();
        // Act
        var result = controller.SuccessEndpoint("Test Data", "Custom success message");
        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;

        response.Success.Should().BeTrue();
        response.Message.Should().Be("Custom success message");
        response.Data.Should().Be("Test Data");
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Success_ShouldUseDefaultMessage()
        // Should return a success response with the default message when no custom message is provided
    {
        // Arrange
        var controller = new TestApiController();
        // Act
        var result = controller.SuccessEndpoint("Test Data");
        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;

        response.Success.Should().BeTrue();
        response.Message.Should().Be("Request successful.");
        response.Data.Should().Be("Test Data");
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_ShouldReturnBadRequest()
        // Should return a failure response
    {
        // Arrange
        var controller = new TestApiController();
        // Act
        var result = controller.FailureEndpoint();
        // Assert
        var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;

        response.Success.Should().BeFalse();
        response.Message.Should().Be("Request failed.");
        response.Data.Should().BeNull();
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_ShouldUseCustomMessageAndErrors()
        // Should return a failure response with custom message and errors
    {
        // Arrange
        var controller = new TestApiController();

        // Act
        var result = controller.FailureEndpoint("Something went wrong",
            "Not a teapot", "Invalid input");
        // Assert
        var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;

        response.Success.Should().BeFalse();
        response.Message.Should().Be("Something went wrong");
        response.Data.Should().BeNull();
        response.Errors.Should().BeEquivalentTo("Not a teapot", "Invalid input");
    }

    [Fact]
    public void Failure_ShouldUseDefaultMessageAndNoErrors()
        // Should return a failure response with default message and no errors when no custom message or errors are provided
    {
        // Arrange
        var controller = new TestApiController();
        // Act
        var result = controller.FailureEndpoint();
        // Assert
        var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponse<string>>().Subject;

        response.Success.Should().BeFalse();
        response.Message.Should().Be("Request failed.");
        response.Data.Should().BeNull();
        response.Errors.Should().NotBeNull();
        response.Errors.Should().BeEmpty();
    }
}