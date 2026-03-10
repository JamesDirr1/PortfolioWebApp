using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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

        var requestGuid = string.IsNullOrWhiteSpace(incomingRequestGuid)
            ? Guid.NewGuid().ToString("N")
            : incomingRequestGuid;

        context.Items[RequestGuidItemKey] = requestGuid;
        context.Response.Headers[RequestGuidHeaderName] = requestGuid;

        var stopwatch = Stopwatch.StartNew();

        using (_logger.BeginScope(new Dictionary<string, object>
               {
                   ["RequestGuid"] = requestGuid
               }))
        {
            _logger.LogInformation(
                "Started processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            try
            {
                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "Completed processing request {Method} {Path} with status code {StatusCode} in {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Error processing request {Method} {Path} after {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}