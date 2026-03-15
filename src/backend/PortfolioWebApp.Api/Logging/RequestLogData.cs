namespace PortfolioWebApp.Api.Logging;

public sealed record RequestLogData(
    string? RequestId,
    string? Method,
    string? Path,
    int? StatusCode,
    int? ElapsedMilliseconds
);