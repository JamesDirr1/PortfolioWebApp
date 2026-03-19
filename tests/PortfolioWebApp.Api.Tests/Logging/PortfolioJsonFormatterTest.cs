using System.Text.Json;
using FluentAssertions;
using PortfolioWebApp.Api.Logging;
using Serilog.Events;
using Serilog.Parsing;

namespace PortfolioWebApp.Api.Tests.Logging;

public class PortfolioJsonFormatterTest
{
    private static LogEvent CreateLogEvent(
        LogEventLevel level = LogEventLevel.Information,
        string messageTemplate = "Test message",
        Exception? exception = null,
        Dictionary<string, LogEventPropertyValue>? properties = null)
    {
        var parser = new MessageTemplateParser();

        return new LogEvent(
            DateTimeOffset.UtcNow,
            level,
            exception,
            parser.Parse(messageTemplate),
            (properties ?? new Dictionary<string, LogEventPropertyValue>())
            .Select(x => new LogEventProperty(x.Key, x.Value)));
    }

    [Fact]
    public void Format_ValidJson()
        // Ensures that the log message is in a valid JSON format
        // Should return valid JSON
    {
        // Arrange
        var formatter = new PortfolioJsonFormatter();
        var logEvent = CreateLogEvent(
            LogEventLevel.Information,
            "Hello, World!");
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        var action = () => JsonDocument.Parse(result);
        action.Should().NotThrow();
    }

    [Fact]
    public void Format_Base()
        // Base level of the log message
        // Should log timestamp, level, and message
    {
        // Arrange
        var formatter = new PortfolioJsonFormatter();
        var logEvent = CreateLogEvent(
            LogEventLevel.Information,
            "Hello, World!",
            properties: new Dictionary<string, LogEventPropertyValue>
            {
                ["EventId"] = new ScalarValue(123),
                ["SourceContext"] = new ScalarValue("PortfolioWebApp.Api.Controllers.CategoriesController")
            });
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        using var json = JsonDocument.Parse(result);
        var root = json.RootElement;

        root.GetProperty("Timestamp").GetString().Should().NotBeNullOrWhiteSpace();
        root.GetProperty("EventId").GetInt32().Should().Be(123);
        root.GetProperty("Category").GetString().Should().Be("PortfolioWebApp.Api.Controllers.CategoriesController");
        root.GetProperty("LogLevel").GetString().Should().Be("Information");
        root.GetProperty("Message").GetString().Should().Be("Hello, World!");
    }

    [Fact]
    public void Format_IncludeRequestData()
        // Log message includes request data when present 
        // Should return request data such as request id, methods, path,e tc
    {
        // Arrange
        var formatter = new PortfolioJsonFormatter();
        var logEvent = CreateLogEvent(
            messageTemplate: "HTTP request completed",
            properties: new Dictionary<string, LogEventPropertyValue>
            {
                ["Request-Id"] = new ScalarValue("request1234"),
                ["Method"] = new ScalarValue("GET"),
                ["Path"] = new ScalarValue("/api/categories"),
                ["StatusCode"] = new ScalarValue(200),
                ["ElapsedMilliseconds"] = new ScalarValue(45)
            });
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        using var json = JsonDocument.Parse(result);
        var root = json.RootElement;

        root.TryGetProperty("Request", out var request).Should().BeTrue();
        request.GetProperty("Request-Id").GetString().Should().Be("request1234");
        request.GetProperty("Method").GetString().Should().Be("GET");
        request.GetProperty("Path").GetString().Should().Be("/api/categories");
        request.GetProperty("StatusCode").GetInt32().Should().Be(200);
        request.GetProperty("ElapsedMilliseconds").GetInt32().Should().Be(45);
    }

    [Fact]
    public void Format_Should_Include_Exception_When_Present()
    {
        // Arrange
        var formatter = new PortfolioJsonFormatter();
        var exception = new InvalidOperationException("Bad Request");
        var logEvent = CreateLogEvent(
            LogEventLevel.Error,
            "Request failed",
            exception);
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        using var json = JsonDocument.Parse(result);
        var root = json.RootElement;

        root.GetProperty("LogLevel").GetString().Should().Be("Error");
        root.GetProperty("Message").GetString().Should().Be("Request failed");
        root.GetProperty("Exception").GetString().Should().Contain("Bad Request");
    }
}