namespace PortfolioWebApp.Api.Logging;

public sealed record RequestLogData(
    string? RequestGuid,
    string? Method,
    string? Path,
    int? StatusCode,
    int? ElapsedMilliseconds
);