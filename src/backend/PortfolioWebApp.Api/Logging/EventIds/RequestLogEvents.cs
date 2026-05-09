using Microsoft.Extensions.Logging;

namespace PortfolioWebApp.Api.Logging.EventIds;

public static class RequestLogEvents
{
    // Request lifecycle
    public static readonly EventId RequestStarted =
        new(20000, nameof(RequestStarted));

    public static readonly EventId RequestCompleted =
        new(20001, nameof(RequestCompleted));


    // Request performance
    public static readonly EventId SlowRequestDetected =
        new(20100, nameof(SlowRequestDetected));


    // Success (2xx)
    public static readonly EventId Created =
        new(20201, nameof(Created));


    // Client errors (4xx)
    public static readonly EventId BadRequest =
        new(20400, nameof(BadRequest));

    public static readonly EventId Unauthorized =
        new(20401, nameof(Unauthorized));

    public static readonly EventId Forbidden =
        new(20403, nameof(Forbidden));

    public static readonly EventId NotFound =
        new(20404, nameof(NotFound));

    public static readonly EventId MethodNotAllowed =
        new(20405, nameof(MethodNotAllowed));

    public static readonly EventId TooManyRequests =
        new(20429, nameof(TooManyRequests));

    public static readonly EventId RequestCancelled =
        new(20499, nameof(RequestCancelled));


    // Server errors (5xx)
    public static readonly EventId InternalServerError =
        new(20500, nameof(InternalServerError));

    public static readonly EventId NotImplemented =
        new(20501, nameof(NotImplemented));

    public static readonly EventId ServiceUnavailable =
        new(20503, nameof(ServiceUnavailable));

    public static readonly EventId RequestTimeout =
        new(20504, nameof(RequestTimeout));

    public static readonly EventId RequestUnhandledException =
        new(20550, nameof(RequestUnhandledException));
}