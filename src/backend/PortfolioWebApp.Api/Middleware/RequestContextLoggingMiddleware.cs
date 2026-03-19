using System.Diagnostics;
using Serilog.Context;


namespace PortfolioWebApp.Api.Middleware;

public sealed class RequestGuidMiddleware(
    RequestDelegate next,
    ILogger<RequestGuidMiddleware> logger)
{
    private const string RequestIdHeaderName = "Request-Id";
    public const string RequestIdItemKey = "RequestId";

    public async Task InvokeAsync(HttpContext context)
    {
        string? incomingRequestId = null;

        if (context.Request.Headers.TryGetValue(RequestIdHeaderName, out var header))
            incomingRequestId = header.Count > 0 ? header[0] : null;

        //If no request guid is not provided create a new one
        var requestId = string.IsNullOrWhiteSpace(incomingRequestId)
            ? Guid.NewGuid().ToString("N")
            : incomingRequestId;

        context.Items[RequestIdItemKey] = requestId;
        context.Response.Headers[RequestIdHeaderName] = requestId;

        var stopwatch = Stopwatch.StartNew();

        //Push request info to log context
        using (LogContext.PushProperty("Request-Id", requestId))
        using (LogContext.PushProperty("Method", context.Request.Method))
        using (LogContext.PushProperty("Path", context.Request.Path.ToString()))
        using (LogContext.PushProperty("LogType", "RequestStart"))
        {
            logger.LogInformation("Received request"); //Log for every request received 
            try
            {
                await next(context);
                stopwatch.Stop();
                //Push Response info to log context
                using (LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
                using (LogContext.PushProperty("LogType", "RequestCompleted"))
                {
                    logger.LogInformation("HTTP request completed"); //Log for each completed request
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                //Push Response info to log context for exceptions 
                using (LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                using (LogContext.PushProperty("StatusCode", StatusCodes.Status500InternalServerError))
                using (LogContext.PushProperty("LogType", "RequestCompleted"))
                {
                    logger.LogError(ex, "HTTP request failed"); //Log for each failed request
                }

                throw;
            }
        }
    }
}