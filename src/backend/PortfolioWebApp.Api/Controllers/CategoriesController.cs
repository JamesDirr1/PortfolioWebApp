using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Utilities;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.DTOs.Categories;
using PortfolioWebApp.Domain.Queries;

namespace PortfolioWebApp.Api.Controllers;

[ApiController, Route("api/categories")]
public class CategoriesController(
    ICategoryService categoryService,
    ILogger<CategoriesController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
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
        var categories = await categoryService.GetAllAsync(query, cancellationToken);
        logger.LogInformation("Returned {Count} categories", categories.Count());
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Categories JSON: {CategoriesJson:l}", JsonLogHelper.ToJson(categories));
        }
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting category with id {id}", id);
        var category = await categoryService.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category with id {id} not found", id);
            return NotFound(new { message = "Category not found" });
        }

        logger.LogInformation("Returned category with id {id}", id);
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Category JSON: {CategoryJson:l}", JsonLogHelper.ToJson(category));
        }

        return Ok(category);
    }
}