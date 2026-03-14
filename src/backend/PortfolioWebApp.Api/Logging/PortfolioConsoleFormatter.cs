using Serilog.Formatting;
using Serilog.Events;

namespace PortfolioWebApp.Api.Logging;

public sealed class PortfolioConsoleFormatter : ITextFormatter

{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var timestamp = logEvent.Timestamp.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        var level = ToShortLevel(logEvent.Level);
        var message = logEvent.RenderMessage();
        var logType = LogEventPropertyReader.GetString(logEvent, "LogType");

        var requestData = LogEventPropertyReader.GetRequestData(logEvent);

        var log = $"[{timestamp} {level}] {message} "; //Base log message

        var isMethodPath = requestData.Method is not null && requestData.Path is not null;
        var isStatusElapsed = requestData.StatusCode is not null && requestData.ElapsedMilliseconds is not null;

        switch (logType)
        {
            case "RequestStart" when isMethodPath:
                log += $"(method: {requestData.Method}) (path: {requestData.Path}) ";
                break;
            case "RequestCompleted" when isMethodPath && isStatusElapsed:
                log += $"(method: {requestData.Method}) (path: {requestData.Path}) (status: {requestData.StatusCode}) (time: {requestData.ElapsedMilliseconds}ms) ";
                break;
        }

        if (!string.IsNullOrWhiteSpace(requestData.RequestGuid))
        {
            log += $"(guid: {requestData.RequestGuid})";
        }

        if (logEvent.Exception is not null)
        {
            log += $"{Environment.NewLine}{logEvent.Exception}";
        }
        
        output.WriteLine(log);
    }

    private static string ToShortLevel(LogEventLevel level) => level switch
    {
        LogEventLevel.Verbose => "VRB",
        LogEventLevel.Debug => "DBG",
        LogEventLevel.Information => "INF",
        LogEventLevel.Warning => "WRN",
        LogEventLevel.Error => "ERR",
        LogEventLevel.Fatal => "FTL",
        _ => "UNK"
    };
}