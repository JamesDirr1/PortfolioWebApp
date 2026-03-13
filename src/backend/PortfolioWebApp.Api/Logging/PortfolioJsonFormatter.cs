using System.Text.Json;
using Serilog.Formatting;
using Serilog.Events;

namespace PortfolioWebApp.Api.Logging;

public sealed class PortfolioJsonFormatter :  ITextFormatter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public void Format(LogEvent logEvent, TextWriter output)
    {
        var payload = new Dictionary<string, object?> //Main body of log messages
        {
            ["Timestamp"] = logEvent.Timestamp.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            ["EventId"] = GetInt(logEvent, "EventId") ?? 0,
            ["Category"] = GetString(logEvent, "SourceContext") ?? "Unknown",
            ["LogLevel"] = logEvent.Level.ToString(),
            ["Message"] = logEvent.RenderMessage(),
        };
        
        var request = new Dictionary<string, object?>(); //Request subsection of log messages
        AddIfPresent(request, "RequestGuid", GetString(logEvent, "RequestGuid"));
        AddIfPresent(request, "Host", GetString(logEvent, "Host"));
        AddIfPresent(request, "Method", GetString(logEvent, "Method"));
        AddIfPresent(request, "Path", GetString(logEvent, "Path"));
        AddIfPresent(request, "StatusCode", GetInt(logEvent, "StatusCode"));
        AddIfPresent(request, "ElapsedMilliseconds", GetInt(logEvent, "ElapsedMilliseconds"));
        AddIfPresent(request, "Response", GetString(logEvent, "Response"));
    
        if (request.Count > 0) //Adds Request subsection to log messages if there is request information
        {
            payload["Request"] = request;
        }

        if (logEvent.Exception is not null) //Adds Exception subsection to log message if an exception is town
        {
            payload["Exception"] = logEvent.Exception.ToString();
        }

        var json = JsonSerializer.Serialize(payload, JsonOptions);
        output.WriteLine(json);
    }

    /// <summary>
    /// Adds key value pair to provided dictionary if value is not null
    /// </summary>
    private static void AddIfPresent(IDictionary<string, object?> dictionary, string key, object? value)
    {
        if (value is not null)
        {
            dictionary[key] = value;
        }
    }

    private static string? GetString(LogEvent logEvent, string propertyName)
    {
        if (!logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            return null;
        }

        return value switch
        {
            ScalarValue { Value: not null } scalar => scalar.Value.ToString(),
            _ => value.ToString().Trim('"')
        };
    }

    private static int? GetInt(LogEvent logEvent, string propertyName)
    {
        if (!logEvent.Properties.TryGetValue(propertyName, out var value))
        {
            return null;
        }

        return value switch
        {
            ScalarValue { Value: int i } => i,
            ScalarValue { Value: long l } => (int)l,
            ScalarValue { Value: string s } when int.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }
}