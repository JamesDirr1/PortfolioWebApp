using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PortfolioWebApp.Api.Middleware;
using PortfolioWebApp.Api.Services;

namespace PortfolioWebApp.Api.Tests.Services;

public class HttpRequestContextTests
{
    [Fact]
    public void GetRequestId()
        // Get Request ID from Http context
        // Should return provided request ID
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey] = "Request1234";

        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock
            .Setup(x => x.HttpContext)
            .Returns(httpContext);
        var requestContext = new HttpRequestContext(accessorMock.Object);
        // Act
        var result = requestContext.GetRequestId;
        // Assert
        result.Should().Be("Request1234");
    }

    [Fact]
    public void GetRequestId_ItemNotFound()
        //  Get Request ID but no request ID in payload
        // Should return "no-request-id"
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock
            .Setup(x => x.HttpContext)
            .Returns(httpContext);
        var requestContext = new HttpRequestContext(accessorMock.Object);
        // Act
        var result = requestContext.GetRequestId;
        // Assert
        result.Should().Be("no-request-id");
    }

    [Fact]
    public void GetRequestId_ItemNull()
        //  Get Request ID but null request ID in payload
        // Should return "no-request-id"
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey] = null;

        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock
            .Setup(x => x.HttpContext)
            .Returns(httpContext);
        var requestContext = new HttpRequestContext(accessorMock.Object);
        // Act
        var result = requestContext.GetRequestId;
        // Assert
        result.Should().Be("no-request-id");
    }
}