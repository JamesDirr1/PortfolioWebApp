using FluentAssertions;
using PortfolioWebApp.Api.Responses;

namespace PortfolioWebApp.Api.Tests.Responses;

public class ApiValidationResponseTest
{
        [Fact]
        public void Failure_ShouldSetExpectedProperties()
            // Should return a failure response with the expected properties set correctly
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "Field1", new[] { "Error 1", "Error 2" } },
                { "Field2", new[] { "Error 3" } }
            };
            // Act
            var result = ApiValidationResponse.Failure("Validation failed", errors);
            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Validation failed");
            result.Data.Should().BeNull();
            result.Errors.Should().BeEquivalentTo(errors);
        }
}