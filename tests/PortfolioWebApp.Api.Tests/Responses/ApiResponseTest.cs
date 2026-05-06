using FluentAssertions;
using PortfolioWebApp.Api.Responses;

namespace PortfolioWebApp.Api.Tests.Responses;

public class ApiResponseTest
{
    [Fact]
    public void SuccessResponse_ShouldSetExpectedProperties()
        // Should return a success response with the expected properties set correctly
    {
        // Arrange & Act
        var result = ApiResponse<string>.SuccessResponse("data", "message");
        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("message");
        result.Data.Should().Be("data");
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void FailureResponse_ShouldSetExpectedProperties_WhenErrorsProvided()
        // Should return a failure response with errors listed
    {
        // Arrange
        var errors = new[] { "Error 1", "Error 2" };
        // Act
        var result = ApiResponse<string>.FailureResponse("failed", errors);
        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("failed");
        result.Data.Should().BeNull();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void FailureResponse_ShouldSetEmptyErrors_WhenNoErrorsProvided()
        // Should return a failure response with no errors listed
    {
        // Arrange & Act
        var result = ApiResponse<string>.FailureResponse("failed");
        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }
}