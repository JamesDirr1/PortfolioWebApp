using System.ComponentModel.DataAnnotations;

namespace PortfolioWebApp.Application.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class AllowedValuesAttribute(params string[] allowedValues) : ValidationAttribute
{
    private readonly HashSet<string> _allowedValues =
        allowedValues.ToHashSet(StringComparer.OrdinalIgnoreCase);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string stringValue)
        {
            return new ValidationResult("Invalid value type.");
        }

        if (_allowedValues.Contains(stringValue))
        {
            return ValidationResult.Success;
        }

        var allowed = string.Join(", ", _allowedValues.OrderBy(x => x));

        return new ValidationResult(
            ErrorMessage ?? $"{validationContext.MemberName} must be one of: {allowed}.");
    }
}