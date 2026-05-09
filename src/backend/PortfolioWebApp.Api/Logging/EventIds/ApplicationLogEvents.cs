using Microsoft.Extensions.Logging;

namespace PortfolioWebApp.Api.Logging.EventIds;

public static class ApplicationLogEvents
{
    // Startup
    public static readonly EventId ApplicationStarting =
        new(10000, nameof(ApplicationStarting));

    public static readonly EventId ApplicationStarted =
        new(10001, nameof(ApplicationStarted));

    public static readonly EventId ApplicationConfigLoaded =
        new(10002, nameof(ApplicationConfigLoaded));


    // Shutdown
    public static readonly EventId ApplicationStopping =
        new(10100, nameof(ApplicationStopping));

    public static readonly EventId ApplicationStopped =
        new(10101, nameof(ApplicationStopped));


    // Environment / configuration
    public static readonly EventId ApplicationEnvironmentSet =
        new(10200, nameof(ApplicationEnvironmentSet));

    public static readonly EventId ApplicationConfigurationValidated =
        new(10201, nameof(ApplicationConfigurationValidated));


    // Health checks
    public static readonly EventId ApplicationHealthCheckStarted =
        new(10300, nameof(ApplicationHealthCheckStarted));

    public static readonly EventId ApplicationHealthCheckPassed =
        new(10301, nameof(ApplicationHealthCheckPassed));

    public static readonly EventId ApplicationHealthCheckFailed =
        new(10302, nameof(ApplicationHealthCheckFailed));
    
    // Errors
    public static readonly EventId ApplicationError =
        new(10500, nameof(ApplicationError));

    public static readonly EventId ApplicationStartupFailure =
        new(10501, nameof(ApplicationStartupFailure));

    public static readonly EventId ApplicationBadConfiguration =
        new(10502, nameof(ApplicationBadConfiguration));

    public static readonly EventId ApplicationConfigurationLoadFailed =
        new(10503, nameof(ApplicationConfigurationLoadFailed));
}