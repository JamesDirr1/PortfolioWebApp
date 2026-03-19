using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PortfolioWebApp.Api.Middleware;
using PortfolioWebApp.Api.Services;

namespace PortfolioWebApp.Api.Tests.Middleware;

public class RequestContextLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync()
        // InvokeAsync with Request ID included in header
        // Should use Request ID from header
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.RequestId = "RequestId1234";

        RequestDelegate next = context => Task.CompletedTask;

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        await middleware.InvokeAsync(httpContext);
        // Assert
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey].Should().Be("RequestId1234");
        httpContext.Response.Headers.RequestId.ToString().Should().Be("RequestId1234");
    }

    [Fact]
    public async Task InvokeAsync_MultipleRequestIds()
        // InvokeAsync with a list of Request ID included in header
        // Should use first Request ID from header
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.RequestId = new[] { "first", "second" };

        RequestDelegate next = context => Task.CompletedTask;

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        await middleware.InvokeAsync(httpContext);
        // Assert
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey].Should().Be("first");
        httpContext.Response.Headers.RequestId.ToString().Should().Be("first");
    }


    [Fact]
    public async Task InvokeAsync_MissingHeader()
        // InvokeAsync but with missing header
        // Should use generate a new request id
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        RequestDelegate next = context => Task.CompletedTask;

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        await middleware.InvokeAsync(httpContext);
        // Assert
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey]
            .Should()
            .NotBeNull();
        var requestId = httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey]!.ToString();
        requestId.Should().NotBeNullOrWhiteSpace();
        requestId.Should().HaveLength(32);

        httpContext.Response.Headers.RequestId.ToString()
            .Should()
            .Be(requestId);
    }

    [Fact]
    public async Task InvokeAsync_EmptyHeader()
        // InvokeAsync but with Empty Header
        // Should use generate a new request id
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.RequestId = "";

        RequestDelegate next = context => Task.CompletedTask;

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        await middleware.InvokeAsync(httpContext);
        // Assert
        httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey]
            .Should()
            .NotBeNull();
        var requestId = httpContext.Items[RequestContextLoggingMiddleware.RequestIdItemKey]!.ToString();
        requestId.Should().NotBeNullOrWhiteSpace();
        requestId.Should().HaveLength(32);

        httpContext.Response.Headers.RequestId.ToString()
            .Should()
            .Be(requestId);
    }

    [Fact]
    public async Task InvokeAsync_NextMiddleware()
        // InvokeAsync but testing calling Next Middleware
        // Should be calling next middleware
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        var nextCalled = false;
        RequestDelegate next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        await middleware.InvokeAsync(httpContext);
        // Assert
        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_CatchException()
        // InvokeAsync but catch exception 
        // Should catch exception and throw an error
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        RequestDelegate next = context => throw new InvalidOperationException("Error");

        var loggerMock = new Mock<ILogger<RequestContextLoggingMiddleware>>();
        var middleware = new RequestContextLoggingMiddleware(next, loggerMock.Object);
        // Act
        var action = async () => await middleware.InvokeAsync(httpContext);
        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Error");
    }
}