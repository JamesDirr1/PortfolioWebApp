using FluentAssertions;
using PortfolioWebApp.Application.QueryParameters;
using System.ComponentModel.DataAnnotations;

namespace PortfolioWebApp.Application.Tests.QueryParameters;

public class CategoryQueryParametersTest
{
    [Fact]
    public void CategoryQueryParameters_TestDefaults()
        // Should return default values for SortBy, SortDirection, Page, and PageSize
    {
        // Arrange && Act
        var results = new CategoryQueryParameters();
        // Assert
        results.Title.Should().BeNull();
        results.SortBy.Should().Be("DisplayOrder");
        results.SortDirection.Should().Be("asc");
        results.Page.Should().Be(1);
        results.PageSize.Should().Be(10);
    }

    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenSortByIsInvalid()
        // Should throw validation error when SortBy is set to an invalid value (e.g., "isActive")
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortBy = "isActive"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(x => x.ErrorMessage == "SortBy must be one of: Id, Title, DisplayOrder.");
    }

    [Fact]
    public void CategoryQueryParameters_ShouldPassValidation_WhenSortByIsDifferentCase()
        // Should pass validation when SortBy is set to a valid value with different casing (e.g., "id" instead of "Id")
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortBy = "id"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeTrue();
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenSortDirectionIsInvalid()
        // Should throw validation error when sortDirection is set to an invalid value (e.g., "up")
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortDirection = "up"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(x => x.ErrorMessage == "SortDirection must be either 'asc' or 'desc'.");
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldPassValidation_WhenSortDirectionIsDifferentCase()
        // Should pass validation when sortDirection is set to a valid value with different casing (e.g., "ASC" instead of "asc")
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            SortDirection = "ASC"
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeTrue();
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenPageIsLessThan1()
        // Should throw validation error when page is set to a value less than 1 (e.g., 0)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            Page = 0
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(x => x.ErrorMessage == "The field Page must be between 1 and 2147483647.");
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldPassValidation_WhenAtMaxInt()
        // Should pass validation when page is set to the maximum integer value (2147483647)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            Page = 2147483647
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeTrue();
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldPassValidation_WhenAtMin()
        // Should pass validation when page is set to the minimum valid value (1)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            Page = 1
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeTrue();
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenPageSizeIsLessThan1()
        // Should throw validation error when pageSize is set to a value less than 1 (e.g., 0)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            PageSize = 0
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(x => x.ErrorMessage == "The field PageSize must be between 1 and 100.");
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenPageSizeIsGreaterThan100()
        // Should throw validation error when pageSize is set to a value greater than 100 (e.g., 101)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            PageSize = 101
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(x => x.ErrorMessage == "The field PageSize must be between 1 and 100.");
    }
    
    [Fact]
    public void CategoryQueryParameters_ShouldFailValidation_WhenPageSizeIsInRange()
        // Should pass validation when pageSize is in range (e.g., 50)
    {
        // Arrange
        var model = new CategoryQueryParameters
        {
            PageSize = 50
        };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        // Act
        var isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        // Assert
        isValid.Should().BeTrue();
    }
}