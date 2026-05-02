using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Responses;
using PortfolioWebApp.Api.Utilities;
using PortfolioWebApp.Application.Common;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Application.QueryParameters;

namespace PortfolioWebApp.Api.Controllers;

[ApiController, Route("api/categories")]
public class CategoriesController(
    ICategoryService categoryService,
    ILogger<CategoriesController> logger)
    : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiValidationResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PagedResponse<CategoryDto>>>> GetAll(
        [FromQuery] CategoryQueryParameters query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting categories with Title={Title}, SortBy={SortBy}, SortDirection={SortDirection}, Page={Page}, PageSize={PageSize}",
            query.Title,
            query.SortBy,
            query.SortDirection,
            query.Page,
            query.PageSize);

        var pagedResponse = await categoryService.GetAllAsync(query, cancellationToken);

        logger.LogInformation("Returned {Count} categories out of {TotalCount}", pagedResponse.Items.Count,
            pagedResponse.MetaData.TotalCount);
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Categories JSON: {CategoriesJson:l}", JsonLogHelper.ToJson(pagedResponse.Items));
        }

        return Success(pagedResponse, pagedResponse.Items.Count == 0
            ? "No categories found."
            : "Categories retrieved successfully.");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting category with Id={Id}", id);

        if (id <= 0)
        {
            return Failure<CategoryDto>(
                "Invalid Category id.",
                "Id must be greater than 0."
            );
        }

        var category = await categoryService.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category with Id={Id} not found", id);
            return FailureNotFound<CategoryDto>(
                "Category not found.",
                $"No Category exists with id of {id}."
            );
        }

        logger.LogInformation("Returned category with Id={Id}", id);
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Category JSON: {CategoryJson:l}", JsonLogHelper.ToJson(category));
        }

        return Success(category, "Category retrieved successfully.");
    }

    // Used to catch invalid id types (e.g. string) and return a 400 Bad Request with error message instead of 404 Not Found
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public ActionResult<ApiResponse<CategoryDto>> GetByInvalidId(string id)
    {
        logger.LogWarning("Invalid Category id format: {Id}", id);
        return Failure<CategoryDto>(
            "Invalid Category id type.",
            $"'{id}' is not a valid Category id. Id must be an integer greater than 0."
        );
    }
}