using System.Text.Json;

namespace PortfolioWebApp.Api.Utilities;

public static class JsonLogHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public static string ToJson(object? obj)
    {
        return JsonSerializer.Serialize(obj, Options);
    }
}