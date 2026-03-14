using System.Text.Json;
using Serilog.Formatting;
using Serilog.Events;

namespace PortfolioWebApp.Api.Logging;

public sealed class PortfolioJsonFormatter : ITextFormatter
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
            ["EventId"] = LogEventPropertyReader.GetInt(logEvent, "EventId") ?? 0,
            ["Category"] = LogEventPropertyReader.GetString(logEvent, "SourceContext") ?? "Unknown",
            ["LogLevel"] = logEvent.Level.ToString(),
            ["Message"] = logEvent.RenderMessage(),
        };

        var request = new Dictionary<string, object?>(); //Request subsection of log messages
        var requestData =LogEventPropertyReader.GetRequestData(logEvent); 
        
        AddIfPresent(request, "RequestGuid", requestData.RequestGuid);
        AddIfPresent(request, "Method", requestData.Method);
        AddIfPresent(request, "Path", requestData.Path);
        AddIfPresent(request, "StatusCode", requestData.StatusCode);
        AddIfPresent(request, "ElapsedMilliseconds", requestData.ElapsedMilliseconds);

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
}