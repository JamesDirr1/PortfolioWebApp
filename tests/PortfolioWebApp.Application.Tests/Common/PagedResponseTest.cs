using FluentAssertions;
using PortfolioWebApp.Application.Common;

namespace PortfolioWebApp.Application.Tests.Common;

public class PagedResponseTest
{
    [Fact]
    public void Create_ShouldReturnCorrectMetaData()
        // Should return set Items and correct MetaData based on input parameters
    {
        // Arrange
        var items = new List<string>
        {
            "item1",
            "item2",
            "item3"
        };
        // Act
        var result = PagedResponse<string>.Create(
            items: items,
            page: 1,
            pageSize: 10,
            totalCount: 3);
        // Assert
        result.Should().BeOfType<PagedResponse<string>>();
        result.Items.Should().BeEquivalentTo(items);
        result.MetaData.Page.Should().Be(1);
        result.MetaData.PageSize.Should().Be(10);
        result.MetaData.TotalCount.Should().Be(3);
        result.MetaData.TotalPages.Should().Be(1);
        result.MetaData.HasPreviousPage.Should().Be(false);
        result.MetaData.HasNextPage.Should().Be(false);
    }

    [Fact]
    public void Create_ShouldHandleEmptyItems()
        // Should return correct MetaData even when Items list is empty, based on input parameters
    {
        // Arrange
        var items = new List<string>();
        // Act
        var result = PagedResponse<string>.Create(
            items: items,
            page: 1,
            pageSize: 10,
            totalCount: 0);
        // Assert
        result.Items.Should().BeEmpty();
        result.MetaData.Page.Should().Be(1);
        result.MetaData.TotalCount.Should().Be(0);
        result.MetaData.TotalPages.Should().Be(0);
    }

    [Fact]
    public void Create_ShouldThrow_WhenMetaDataThrows()
        // Should throw exception if PagedMetaData.Create throws due to invalid parameters (e.g. pageSize <= 0)
    {
        // Arrange
        var items = new List<string>
        {
            "item1"
        };
        // Act
        var action = () => PagedResponse<string>.Create(
            items: items,
            page: 1,
            pageSize: 0,
            totalCount: 1);
        // Assert
        action.Should().Throw<ArgumentException>()
            .WithParameterName("pageSize");
    }
}