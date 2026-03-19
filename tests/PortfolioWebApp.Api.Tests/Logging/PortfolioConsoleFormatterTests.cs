using FluentAssertions;
using PortfolioWebApp.Api.Logging;
using Serilog.Events;
using Serilog.Parsing;

namespace PortfolioWebApp.Api.Tests.Logging;

public class PortfolioConsoleFormatterTests
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
    public void Format_Base()
        // Base level of the log message
        // Should log timestamp, level, and message
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var logEvent = CreateLogEvent(
            LogEventLevel.Information,
            "Hello, World!");
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain("INF");
        result.Should().Contain("Hello, World!");
    }

    [Fact]
    public void Format_RequestStart()
        // Log message for start of Request
        // Should include method and path
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var logEvent = CreateLogEvent(
            messageTemplate: "Received request",
            properties: new Dictionary<string, LogEventPropertyValue>
            {
                ["LogType"] = new ScalarValue("RequestStart"),
                ["Method"] = new ScalarValue("GET"),
                ["Path"] = new ScalarValue("/api/categories")
            });
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain("Received request");
        result.Should().Contain("(method: GET)");
        result.Should().Contain("(path: /api/categories)");
    }

    [Fact]
    public void Format_RequestComplete()
        // Log message for end of Request
        // Should include status code and Elapsed time
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var logEvent = CreateLogEvent(
            messageTemplate: "HTTP request completed",
            properties: new Dictionary<string, LogEventPropertyValue>
            {
                ["LogType"] = new ScalarValue("RequestCompleted"),
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
        result.Should().Contain("HTTP request completed");
        result.Should().Contain("(method: GET)");
        result.Should().Contain("(path: /api/categories)");
        result.Should().Contain("(status: 200)");
        result.Should().Contain("(time: 45ms)");
    }

    [Fact]
    public void Format_IncludeRequestID()
        // Log message contains request id when present
        // Should include request id
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var logEvent = CreateLogEvent(
            messageTemplate: "Received request",
            properties: new Dictionary<string, LogEventPropertyValue>
            {
                ["Request-Id"] = new ScalarValue("request1234")
            });
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain("(req: request1234)");
    }

    [Fact]
    public void Format_IncludeException()
        // Log message contains Exception when present
        // Should include Exception
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var exception = new InvalidOperationException("Bad Request");
        var logEvent = CreateLogEvent(
            messageTemplate: "Request failed",
            level: LogEventLevel.Error,
            exception: exception);
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain("ERR");
        result.Should().Contain("Request failed");
        result.Should().Contain("InvalidOperationException");
        result.Should().Contain("Bad Request");
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose, "VRB")]
    [InlineData(LogEventLevel.Debug, "DBG")]
    [InlineData(LogEventLevel.Information, "INF")]
    [InlineData(LogEventLevel.Warning, "WRN")]
    [InlineData(LogEventLevel.Error, "ERR")]
    [InlineData(LogEventLevel.Fatal, "FTL")]
    public void ToShortLevel(LogEventLevel level, string expectedLevel)
        // Convert log level to 3 charter abbreviation
        // Should return correct abbreviation
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        var logEvent = CreateLogEvent(
            level,
            "Test");
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain(expectedLevel);
    }

    [Fact]
    public void ToShortLevel_LevelNotFound()
        // Convert log level to 3 charter abbreviation when no log level
        // Should return correct unknow abbreviation
    {
        // Arrange
        var formatter = new PortfolioConsoleFormatter();
        const LogEventLevel invalidLevel = (LogEventLevel)999;
        var logEvent = CreateLogEvent(
            messageTemplate: "Test",
            level: invalidLevel);
        using var writer = new StringWriter();
        // Act
        formatter.Format(logEvent, writer);
        var result = writer.ToString();
        // Assert
        result.Should().Contain("UNK");
    }
}