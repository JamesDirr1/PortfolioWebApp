namespace PortfolioWebApp.Api.Responses;

public record ApiValidationResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public object? Data { get; init; }
    public Dictionary<string, string[]> Errors { get; init; } = [];
    
    public static ApiValidationResponse Failure(
        string message,
        Dictionary<string, string[]> errors)
    {
        return new ApiValidationResponse
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors
        };
    }
}