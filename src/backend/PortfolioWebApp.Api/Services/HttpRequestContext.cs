using Microsoft.AspNetCore.Http;
using PortfolioWebApp.Application.Interfaces;
using PortfolioWebApp.Api.Middleware;

namespace PortfolioWebApp.Api.Services;

public sealed class HttpRequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpRequestContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetRequestId =>
        _httpContextAccessor.HttpContext?.Items[RequestContextLoggingMiddleware.RequestIdItemKey]?.ToString()
        ?? "no-request-id";
}