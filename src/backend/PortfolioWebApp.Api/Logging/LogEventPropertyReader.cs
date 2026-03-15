using Serilog.Events;

namespace PortfolioWebApp.Api.Logging;

/// <summary>
/// Class that holds shared methods to extract and convert values from Serilog Event Scalar values. 
/// </summary>
public static class LogEventPropertyReader
{
    /// <summary>
    /// Returns string version of ScalarValue if one exist for provided property name
    /// </summary>
    public static string? GetString(LogEvent logEvent, string propertyName)
    {
        if (!logEvent.Properties.TryGetValue(propertyName, out var value)) return null;

        return value switch
        {
            ScalarValue { Value: not null } scalar => scalar.Value.ToString(),
            _ => value.ToString().Trim('"')
        };
    }

    /// <summary>
    /// Returns integer version of ScalarValue if one exist for provided property name
    /// </summary>
    public static int? GetInt(LogEvent logEvent, string propertyName)
    {
        if (!logEvent.Properties.TryGetValue(propertyName, out var value)) return null;

        return value switch
        {
            ScalarValue { Value: int i } => i,
            ScalarValue { Value: long l } => (int)l,
            ScalarValue { Value: string s } when int.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }

    /// <summary>
    /// Gets all request data from logEvent and converts to an object
    /// </summary>
    public static RequestLogData GetRequestData(LogEvent logEvent)
    {
        return new RequestLogData(
            GetString(logEvent, "Request-Id"),
            GetString(logEvent, "Method"),
            GetString(logEvent, "Path"),
            GetInt(logEvent, "StatusCode"),
            GetInt(logEvent, "ElapsedMilliseconds")
        );
    }
}