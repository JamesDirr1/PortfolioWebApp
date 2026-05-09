using Microsoft.Extensions.Logging;

namespace PortfolioWebApp.Api.Logging.EventIds;

public static class CategoryLogEvents
{
    // Get
    public static readonly EventId GetCategoryRequested =
        new(30000, nameof(GetCategoryRequested));

    public static readonly EventId GetCategorySucceeded =
        new(30001, nameof(GetCategorySucceeded));

    public static readonly EventId GetCategoryFailed =
        new(30002, nameof(GetCategoryFailed));

    // List
    public static readonly EventId ListCategoriesRequested =
        new(30010, nameof(ListCategoriesRequested));

    public static readonly EventId ListCategoriesSucceeded =
        new(30011, nameof(ListCategoriesSucceeded));

    public static readonly EventId ListCategoriesFailed =
        new(30012, nameof(ListCategoriesFailed));

    // Create
    public static readonly EventId CreateCategoryRequested =
        new(30100, nameof(CreateCategoryRequested));

    public static readonly EventId CreateCategorySucceeded =
        new(30101, nameof(CreateCategorySucceeded));

    public static readonly EventId CreateCategoryFailed =
        new(30102, nameof(CreateCategoryFailed));

    // Update
    public static readonly EventId UpdateCategoryRequested =
        new(30200, nameof(UpdateCategoryRequested));

    public static readonly EventId UpdateCategorySucceeded =
        new(30201, nameof(UpdateCategorySucceeded));

    public static readonly EventId UpdateCategoryFailed =
        new(30202, nameof(UpdateCategoryFailed));

    // Delete
    public static readonly EventId DeleteCategoryRequested =
        new(30300, nameof(DeleteCategoryRequested));

    public static readonly EventId DeleteCategorySucceeded =
        new(30301, nameof(DeleteCategorySucceeded));

    public static readonly EventId DeleteCategoryFailed =
        new(30302, nameof(DeleteCategoryFailed));

    // Client errors
    public static readonly EventId ValidationFailed =
        new(30400, nameof(ValidationFailed));

    public static readonly EventId InvalidCategoryId =
        new(30401, nameof(InvalidCategoryId));

    public static readonly EventId CategoryNotFound =
        new(30404, nameof(CategoryNotFound));

    // Business warnings
    public static readonly EventId DuplicateCategorySlug =
        new(30500, nameof(DuplicateCategorySlug));

    public static readonly EventId DuplicateCategoryTitle =
        new(30501, nameof(DuplicateCategoryTitle));

    // Server errors
    public static readonly EventId UnexpectedError =
        new(30550, nameof(UnexpectedError));

    // Debug
    public static readonly EventId CategoryDebugData =
        new(30900, nameof(CategoryDebugData));
}