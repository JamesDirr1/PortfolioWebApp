using FluentAssertions;
using PortfolioWebApp.Api.Logging;
using Serilog.Events;
using Serilog.Parsing;

namespace PortfolioWebApp.Api.Tests.Logging;

public class LogEventPropertyReaderTests
{
    /// <summary>
    /// A helper function that creates a Serilog log Event
    /// </summary>
    private static LogEvent CreateLogEvent(Dictionary<string, LogEventPropertyValue> properties)
    {
        return new LogEvent(
            DateTimeOffset.UtcNow,
            LogEventLevel.Information,
            exception: null,
            new MessageTemplate("", Enumerable.Empty<MessageTemplateToken>()),
            properties.Select(kvp => new LogEventProperty(kvp.Key, kvp.Value)));
    }

    [Fact]
    public void GetString()
        // Pulls value of type string when key is present in log event
        // Should return string of the value
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["Request-Id"] = new ScalarValue("Request1234")
        });
        // Act
        var result = LogEventPropertyReader.GetString(logEvent, "Request-Id");
        // Assert
        result.Should().Be("Request1234");
    }

    [Fact]
    public void GetString_TypeNotOfString()
        // converts value of type some type  to string when key is present in log event
        // Should return string of the value
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["Request-Id"] = new ScalarValue(null)
        });
        // Act
        var result = LogEventPropertyReader.GetString(logEvent, "Request-Id");
        // Assert
        result.Should().Be("null");
    }

    [Fact]
    public void GetString_KeyNotFound()
        // Pulls value of type null when key is not present in log event
        // Should return null
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>());
        // Act
        var result = LogEventPropertyReader.GetString(logEvent, "Request-Id");
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetInt()
        // Pulls value of type int when key is present in log event
        // Should return int of the value
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["StatusCode"] = new ScalarValue(200)
        });
        // Act
        var result = LogEventPropertyReader.GetInt(logEvent, "StatusCode");
        // Assert
        result.Should().Be(200);
    }

    [Fact]
    public void GetInt_ValueTypeLong()
        // Converts value of type long to int when key is present in log event
        // Should return int of the value
    {
        // Arrange
        const long value = 400;
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["StatusCode"] = new ScalarValue(value)
        });
        // Act
        var result = LogEventPropertyReader.GetInt(logEvent, "StatusCode");
        // Assert
        result.Should().Be(400);
        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public void GetInt_ValueTypeString()
        // Converts value of type string to int when key is present in log event
        // Should return int of the value
    {
        // Arrange
        const string value = "500";
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["StatusCode"] = new ScalarValue(value)
        });
        // Act
        var result = LogEventPropertyReader.GetInt(logEvent, "StatusCode");
        // Assert
        result.Should().Be(500);
        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public void GetInt_ValueTypeNotScalar()
        // Returns nothing when value is not convertable to int
        // Should return null
    {
        // Arrange
        const string value = "500";
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["StatusCode"] = new ScalarValue(new LogEventPropertyValue[]
            {
                new ScalarValue("one"),
                new ScalarValue("two")
            })
        });
        // Act
        var result = LogEventPropertyReader.GetInt(logEvent, "StatusCode");
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetInt_KeyNotFound()
        // Pulls value of type null when key is not present in log event
        // Should return null
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>());
        // Act
        var result = LogEventPropertyReader.GetInt(logEvent, "StatusCode");
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetRequestData()
        // Generates a new RequestLogData object from Serilog event 
        // Should return a RequestLogData
    {
        // Arrange
        var logEvent = CreateLogEvent(new Dictionary<string, LogEventPropertyValue>
        {
            ["Request-Id"] = new ScalarValue("Request1234"),
            ["Method"] = new ScalarValue("GET"),
            ["Path"] = new ScalarValue("/api/categories"),
            ["StatusCode"] = new ScalarValue(200),
            ["ElapsedMilliseconds"] = new ScalarValue(45)
        });
        // Act
        var result = LogEventPropertyReader.GetRequestData(logEvent);
        // Assert
        result.Should().NotBeNull();
        result.RequestId.Should().Be("Request1234");
        result.Method.Should().Be("GET");
        result.Path.Should().Be("/api/categories");
        result.StatusCode.Should().Be(200);
        result.ElapsedMilliseconds.Should().Be(45);
    }
}