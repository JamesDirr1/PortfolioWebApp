using FluentAssertions;
using PortfolioWebApp.Api.Utilities;

namespace PortfolioWebApp.Api.Tests.Utilities;

public class JsonLogHelperTests
{
    [Fact]
    public void ToJson()
        // Turns an object to JSON
        // Should return a string of JSON
    {
        // Arrange
        var obj = new
        {
            Id = 1,
            Name = "Test"
        };
        // Act
        var result = JsonLogHelper.ToJson(obj);
        // Assert
        result.Should().Contain("\"Id\": 1");
        result.Should().Contain("\"Name\": \"Test\"");
        result.Should().Contain("\n"); //NewLine = indented 
    }
    [Fact]
    public void ToJson_HandlesNulls()
        // Turns a null object to JSON
        // Should return a string of null
    {
        // Arrange
        // Act
        var result = JsonLogHelper.ToJson(null);
        // Assert
        result.Should().Contain("null"); //NewLine = indented 
    }
    
}