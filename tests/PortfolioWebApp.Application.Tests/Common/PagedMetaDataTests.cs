using FluentAssertions;
using PortfolioWebApp.Application.Common;

namespace PortfolioWebApp.Application.Tests.Common;

public class PagedMetaDataTests
{
    [Fact]
    public void Create_ShouldHandleZeroCount()
        // Should return correct metadata for empty result
    {
        // Arrange & Act
        var result = PagedMetaData.Create(page: 1, pageSize: 10, totalCount: 0);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasPreviousPage.Should().Be(false);
        result.HasNextPage.Should().Be(false);
    }
    
    [Fact]
    public void Create_ShouldCalculateTotalPages_ExactDivision()
        // Should return correct metadata with exact division of total count by page size
    {
        // Arrange & Act
        var result =  PagedMetaData.Create(page: 1, pageSize: 10, totalCount: 20);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.TotalPages.Should().Be(2);
    }
    
    [Fact]
    public void Create_ShouldCalculateTotalPages_NotExactDivision()
        // Should return correct metadata with not exact division (ceiling) of total count by page size
    {
        // Arrange & Act
        var result =  PagedMetaData.Create(page: 1, pageSize: 10, totalCount: 21);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.TotalPages.Should().Be(3);
    }
    
    [Fact]
    public void Create_ShouldSetNavFlags_NoPreviousPage()
        // Should return correct metadata with no previous page (first page)
    {
        // Arrange & Act
        var result =  PagedMetaData.Create(page: 1, pageSize: 10, totalCount: 30);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.HasPreviousPage.Should().Be(false);
        result.HasNextPage.Should().Be(true);
    }
    
    [Fact]
    public void Create_ShouldSetNavFlags_NoNextPage()
        // Should return correct metadata with no next page (last page)
    {
        // Arrange & Act
        var result =  PagedMetaData.Create(page: 3, pageSize: 10, totalCount: 30);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.HasPreviousPage.Should().Be(true);
        result.HasNextPage.Should().Be(false);
    }
    
    [Fact]
    public void Create_ShouldSetNavFlags_MiddlePage()
        // Should return correct metadata with both previous and next pages (middle page)
    {
        // Arrange & Act
        var result =  PagedMetaData.Create(page: 2, pageSize: 10, totalCount: 30);
        // Assert
        result.Should().BeOfType<PagedMetaData>();
        result.HasPreviousPage.Should().Be(true);
        result.HasNextPage.Should().Be(true);
    }
    
    [Fact]
    public void Create_ShouldThrow_PageSizeIsZero()
        // Should throw ArgumentException when page size is zero
    {
        // Arrange & Act
        var action = () =>  PagedMetaData.Create(page: 2, pageSize: 0, totalCount: 30);
        // Assert
        action.Should().Throw<ArgumentException>().WithParameterName("pageSize");
    }
    
    [Fact]
    public void Create_ShouldThrow_PageIsZero()
        // Should throw ArgumentException when page number is zero
    {
        // Arrange & Act
        var action = () =>  PagedMetaData.Create(page: 0, pageSize: 1, totalCount: 30);
        // Assert
        action.Should().Throw<ArgumentException>().WithParameterName("page");
    }
    
}