using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using PortfolioWebApp.Application.QueryParameters;
using PortfolioWebApp.Application.Validation;

namespace PortfolioWebApp.Application.Tests.Validation;

public class AllowedStringValuesAttributeTest
{
    private static ValidationContext CreateContext(string memberName = "SortBy")
        => new(new object()) { MemberName = memberName };

    [Fact]
    public void IsValid_ReturnsSuccess_ForAllowedValue()
        // Should return success when the value is in the allowed values list
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortDirection = "desc"
        };
        var context = new ValidationContext(model)
        {
            MemberName = nameof(CategoryQueryParameters.SortDirection)
        };
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateProperty(model.SortDirection, context, results);
        // Assert
        isValid.Should().BeTrue(string.Join("; ", results.Select(x => x.ErrorMessage)));
        results.Should().BeEmpty();
    }

    [Fact]
    public void IsValid_ReturnsSuccess_ForAllowedValueIgnoreCase()
        // Should return success when the value is in the allowed values list
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortBy = "displayorder"
        };
        var context = new ValidationContext(model)
        {
            MemberName = nameof(CategoryQueryParameters.SortBy)
        };
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateProperty(model.SortBy, context, results);
        // Assert
        isValid.Should().BeTrue(string.Join("; ", results.Select(x => x.ErrorMessage)));
        results.Should().BeEmpty();
    }

    [Fact]
    public void IsValid_ReturnsSuccess_WhenValueIsNull()
        // Should return success when the value is null (null values are allowed and should not trigger validation errors)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortBy = null
        };
        var context = new ValidationContext(model)
        {
            MemberName = nameof(CategoryQueryParameters.SortBy)
        };
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateProperty(model.SortBy, context, results);
        // Assert
        isValid.Should().BeTrue(string.Join("; ", results.Select(x => x.ErrorMessage)));
        results.Should().BeEmpty();
    }

    [Fact]
    public void IsValid_ReturnsError_WhenValueIsNotString()
        // Should return Error when value is not a string (invalid value type)
    {
        // Arrange
        var attribute = new AllowedStringValuesAttribute("Id", "Title", "DisplayOrder");
        var context = new ValidationContext(new object())
        {
            MemberName = "SortBy"
        };
        // Act
        var result = attribute.GetValidationResult(123, context);
        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result!.ErrorMessage.Should()
            .Be("Invalid value type.");
    }

    [Fact]
    public void IsValid_ReturnsError_WhenValueInvalid()
        // Should return an error when the value is not in the allowed values list
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortBy = "badValue"
        };
        var context = new ValidationContext(model)
        {
            MemberName = nameof(CategoryQueryParameters.SortBy)
        };
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateProperty(model.SortBy, context, results);
        // Assert
        isValid.Should().BeFalse(string.Join("; ", results.Select(x => x.ErrorMessage)));
        results.Should().Contain(x => x.ErrorMessage == "SortBy must be one of: Id, Title, DisplayOrder.");
    }

    [Fact]
    public void IsValid_ShouldUseCustomErrorMessage_WhenValueIsNotValid()
        // Should return an error with the custom error message when the value is not in the allowed values list
    {
        // Arrange 
        var attribute = new AllowedStringValuesAttribute("Id", "Title", "DisplayOrder")
        {
            ErrorMessage = "SortBy must be one of: Id, Title, DisplayOrder."
        };
        var context = CreateContext();
        // Act
        var result = attribute.GetValidationResult("BadValue", context);
        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result.ErrorMessage.Should().Be("SortBy must be one of: Id, Title, DisplayOrder.");
    }
}