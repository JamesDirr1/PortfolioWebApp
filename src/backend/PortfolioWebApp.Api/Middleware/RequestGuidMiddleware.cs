using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace PortfolioWebApp.Api.Middleware;

public sealed class RequestGuidMiddleware
{
    public const string RequestGuidHeaderName = "X-Request-Guid";
    public const string RequestGuidItemKey = "RequestGuid";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestGuidMiddleware> _logger;

    public RequestGuidMiddleware(
        RequestDelegate next,
        ILogger<RequestGuidMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? incomingRequestGuid = null;

        if (context.Request.Headers.TryGetValue(RequestGuidHeaderName, out var header))
            incomingRequestGuid = header.Count > 0 ? header[0] : null;

        //If no request guid is not provided create a new one
        var requestGuid = string.IsNullOrWhiteSpace(incomingRequestGuid)
            ? Guid.NewGuid().ToString("N")
            : incomingRequestGuid;

        context.Items[RequestGuidItemKey] = requestGuid;
        context.Response.Headers[RequestGuidHeaderName] = requestGuid;

        var stopwatch = Stopwatch.StartNew();

        //Push request info to log context
        using (LogContext.PushProperty("RequestGuid", requestGuid))
        using (LogContext.PushProperty("Method", context.Request.Method))
        using (LogContext.PushProperty("Path", context.Request.Path.ToString()))
        using (LogContext.PushProperty("Host", context.Request.Host.ToString()))
        {
            _logger.LogInformation("Received request"); //Log for every request received 
            try
            {
                await _next(context);
                stopwatch.Stop();
                //Push Response info to log context
                using (LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
                {
                    _logger.LogInformation("HTTP request completed"); //Log for each completed request
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                //Push Response info to log context for exceptions 
                using (LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                using (LogContext.PushProperty("StatusCode", StatusCodes.Status500InternalServerError))
                {
                    _logger.LogError(ex, "HTTP request failed"); //Log for each failed request
                }

                throw;
            }
        }
    }
}